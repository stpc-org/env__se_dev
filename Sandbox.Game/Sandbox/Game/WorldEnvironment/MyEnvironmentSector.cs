// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyEnvironmentSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyEnvironmentSector : MyEntity
  {
    private int m_parallelWorksInProgress;
    private MyProceduralEnvironmentDefinition m_environment;
    private MyInstancedRenderSector m_render;
    private IMyEnvironmentOwner m_owner;
    private IMyEnvironmentDataProvider m_provider;
    private MyConcurrentQueue<MyEnvironmentSector.LodHEntry> m_lodHistory = new MyConcurrentQueue<MyEnvironmentSector.LodHEntry>();
    private Vector3D m_sectorCenter;
    private BoundingBox2I m_dataRange;
    private Dictionary<int, HkShape> m_modelsToShapes;
    private MyEnvironmentSector.CompoundInstancedShape m_activeShape;
    private MyEnvironmentSector.CompoundInstancedShape m_newShape;
    private bool m_togglePhysics;
    private bool m_recalculateShape;
    private int m_lodSwitchedFrom = -1;
    private volatile int m_currentLod = -1;
    private volatile int m_lodToSwitch = -1;
    private List<short> m_modelToItem;
    private readonly Dictionary<System.Type, MyEnvironmentSector.Module> m_modules = new Dictionary<System.Type, MyEnvironmentSector.Module>();
    private bool m_modulesPendingUpdate;
    private HashSet<MySectorContactEvent> m_contactListeners;
    private int m_hasParallelWorkPending;

    public Vector3D SectorCenter
    {
      get => this.m_sectorCenter;
      private set => this.m_sectorCenter = value;
    }

    public Vector3D[] Bounds { get; private set; }

    public MyWorldEnvironmentDefinition EnvironmentDefinition { get; private set; }

    public void Init(IMyEnvironmentOwner owner, ref MyEnvironmentSectorParameters parameters)
    {
      this.SectorCenter = parameters.Center;
      this.Bounds = parameters.Bounds;
      this.m_dataRange = parameters.DataRange;
      this.m_environment = (MyProceduralEnvironmentDefinition) parameters.Environment;
      this.EnvironmentDefinition = parameters.Environment;
      this.m_owner = owner;
      this.m_provider = parameters.Provider;
      Vector3D center = parameters.Center;
      owner.ProjectPointToSurface(ref center);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.m_render = new MyInstancedRenderSector(string.Format("{0}:Sector(0x{1:X})", (object) owner, (object) parameters.SectorId), MatrixD.CreateTranslation(center));
      this.SectorId = parameters.SectorId;
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      for (int index = 0; index < 8; ++index)
        invalid.Include(this.Bounds[index]);
      this.PositionComp.SetPosition(parameters.Center);
      this.PositionComp.WorldAABB = invalid;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentEnvironmentSector((IMyEntity) this));
      this.GameLogic = (MyGameLogicComponent) new MyNullGameLogicComponent();
      this.Save = false;
      this.IsClosed = false;
    }

    protected override void Closing() => this.CloseInternal(true);

    public new void Close() => this.CloseInternal(false);

    private void CloseInternal(bool entityClosing)
    {
      if (this.m_render != null)
        this.m_render.DetachEnvironment(this);
      if (this.DataView != null)
      {
        this.DataView.Close();
        this.DataView = (MyEnvironmentDataView) null;
      }
      foreach (MyEnvironmentSector.Module module in this.m_modules.Values)
        module.Proxy.Close();
      this.HasPhysics = false;
      this.m_currentLod = -1;
      base.Close();
      this.IsClosed = true;
    }

    public void SetLod(int lod)
    {
      if (this.Closed || lod == this.m_currentLod && lod == this.m_lodToSwitch)
        return;
      if (Interlocked.Exchange(ref this.m_hasParallelWorkPending, 1) == 0)
        this.Owner.ScheduleWork(this, true);
      this.m_lodToSwitch = lod;
      if (this.m_render == null)
        return;
      this.m_render.Lod = this.m_lodToSwitch;
    }

    public void EnablePhysics(bool physics)
    {
      if (this.Closed)
        return;
      bool flag = this.HasPhysics != physics;
      if (flag != this.m_togglePhysics & flag)
      {
        if (this.m_activeShape == null || this.m_recalculateShape)
        {
          if (Interlocked.Exchange(ref this.m_hasParallelWorkPending, 1) == 0)
            this.Owner.ScheduleWork(this, true);
        }
        else
        {
          if (this.Physics != null)
            this.Physics.Enabled = physics;
          flag = false;
          this.HasPhysics = physics;
          if (!physics)
          {
            Action onPhysicsClose = this.OnPhysicsClose;
            if (onPhysicsClose != null)
              onPhysicsClose();
          }
        }
      }
      this.m_togglePhysics = flag;
    }

    public bool IsLoaded => true;

    public bool IsClosed { get; private set; }

    public int LodLevel => this.m_currentLod;

    public bool HasPhysics { get; private set; }

    public bool IsPinned { get; internal set; }

    public bool IsPendingLodSwitch => this.m_currentLod != this.m_lodToSwitch;

    public bool IsPendingPhysicsToggle => this.m_togglePhysics;

    public void CancelParallel()
    {
    }

    public bool HasSerialWorkPending { get; private set; }

    public bool HasParallelWorkPending
    {
      get => Interlocked.CompareExchange(ref this.m_hasParallelWorkPending, 0, 0) == 1;
      private set => Interlocked.Exchange(ref this.m_hasParallelWorkPending, value ? 1 : 0);
    }

    public bool HasParallelWorkInProgress => Volatile.Read(ref this.m_parallelWorksInProgress) > 0;

    public long SectorId { get; private set; }

    public void DoParallelWork()
    {
      try
      {
        if (Interlocked.Increment(ref this.m_parallelWorksInProgress) > 1 || Interlocked.Exchange(ref this.m_hasParallelWorkPending, 0) != 1)
          return;
        this.HasParallelWorkPending = false;
        if (this.Closed)
        {
          this.m_lodToSwitch = this.m_currentLod;
          this.m_togglePhysics = false;
        }
        else
        {
          bool flag = false;
          if (this.m_lodToSwitch != this.m_currentLod)
          {
            flag = true;
            if (this.m_lodToSwitch == -1)
            {
              this.m_render.Close();
            }
            else
            {
              this.FetchData(this.m_lodToSwitch);
              this.BuildInstanceBuffers(this.m_lodToSwitch);
            }
            this.m_lodSwitchedFrom = this.m_currentLod;
          }
          if (this.m_togglePhysics && !this.HasPhysics || this.HasPhysics && this.m_recalculateShape)
          {
            flag = true;
            this.BuildShape();
          }
          this.HasSerialWorkPending = true;
          if (!flag)
            return;
          this.Owner.ScheduleWork(this, false);
        }
      }
      finally
      {
        Interlocked.Decrement(ref this.m_parallelWorksInProgress);
      }
    }

    public bool DoSerialWork()
    {
      if (this.Closed || this.HasParallelWorkPending)
        return false;
      bool flag = false;
      if (this.m_togglePhysics || this.m_lodSwitchedFrom != this.m_lodToSwitch)
      {
        foreach (KeyValuePair<System.Type, MyEnvironmentSector.Module> module in this.m_modules)
        {
          if (this.m_lodSwitchedFrom != this.m_lodToSwitch)
            module.Value.Proxy.CommitLodChange(this.m_lodSwitchedFrom, this.m_lodToSwitch);
          if (this.m_togglePhysics)
            module.Value.Proxy.CommitPhysicsChange(!this.HasPhysics);
        }
        flag = true;
      }
      this.m_currentLod = this.m_lodToSwitch;
      if (this.m_lodSwitchedFrom != this.m_currentLod && this.m_lodToSwitch == this.m_currentLod)
        this.RaiseOnLodCommitEvent(this.m_currentLod);
      if (this.m_togglePhysics)
        this.RaiseOnPhysicsCommitEvent(this.HasPhysics);
      if (this.m_render != null && this.m_render.HasChanges() && this.m_lodToSwitch == this.m_currentLod)
      {
        this.m_render.CommitChangesToRenderer();
        flag = true;
        this.m_lodSwitchedFrom = this.m_currentLod;
      }
      if (this.m_togglePhysics)
      {
        if (this.HasPhysics)
        {
          this.Physics.Enabled = false;
          this.HasPhysics = false;
          this.m_togglePhysics = false;
        }
        else if (this.m_newShape != null)
        {
          this.PreparePhysicsBody();
          flag = true;
          this.HasPhysics = true;
          this.m_togglePhysics = false;
        }
      }
      if (this.m_recalculateShape)
      {
        this.m_recalculateShape = false;
        if (this.HasPhysics && this.m_newShape != null)
          this.PreparePhysicsBody();
      }
      this.HasSerialWorkPending = false;
      return flag;
    }

    public void OnItemChange(int index, short newModelIndex)
    {
      if (this.m_currentLod == -1 && !this.HasPhysics)
        return;
      foreach (MyEnvironmentSector.Module module in this.m_modules.Values)
        module.Proxy.OnItemChange(index, newModelIndex);
      if (this.m_currentLod != -1)
      {
        this.UpdateItemModel(index, newModelIndex);
        this.m_render.CommitChangesToRenderer();
      }
      if (this.HasPhysics)
      {
        this.UpdateItemShape(index, newModelIndex);
      }
      else
      {
        if (newModelIndex < (short) 0)
          return;
        this.m_recalculateShape = true;
      }
    }

    public void OnItemsChange(int sector, List<int> indices, short newModelIndex)
    {
      if (this.m_currentLod == -1 && !this.HasPhysics)
        return;
      int sectorOffset = this.DataView.SectorOffsets[sector];
      int num = (sector < this.DataView.SectorOffsets.Count - 1 ? this.DataView.SectorOffsets[sector + 1] : this.DataView.Items.Count) - sectorOffset;
      for (int index = 0; index < indices.Count; ++index)
      {
        if (indices[index] >= num)
        {
          indices.RemoveAtFast<int>(index);
          --index;
        }
      }
      foreach (MyEnvironmentSector.Module module in this.m_modules.Values)
        module.Proxy.OnItemChangeBatch(indices, sectorOffset, newModelIndex);
      if (this.m_currentLod != -1)
      {
        foreach (int index in indices)
          this.UpdateItemModel(index + sectorOffset, newModelIndex);
        this.m_render.CommitChangesToRenderer();
      }
      if (this.HasPhysics)
      {
        foreach (int index in indices)
          this.UpdateItemShape(index + sectorOffset, newModelIndex);
      }
      else
      {
        if (newModelIndex <= (short) 0)
          return;
        this.m_recalculateShape = true;
      }
    }

    public MyEnvironmentDataView DataView { get; private set; }

    [Conditional("DEBUG")]
    private void RecordHistory(int lod, bool set)
    {
      if (this.m_lodHistory.Count > 10)
        this.m_lodHistory.Dequeue();
      this.m_lodHistory.Enqueue(new MyEnvironmentSector.LodHEntry()
      {
        Lod = lod,
        Set = set,
        Trace = new StackTrace()
      });
    }

    private unsafe void FetchData(int lodToSwitch)
    {
      MyEnvironmentDataView dataView = this.DataView;
      if (dataView != null && dataView.Lod == lodToSwitch)
        return;
      this.DataView = this.m_provider.GetItemView(lodToSwitch, ref this.m_dataRange.Min, ref this.m_dataRange.Max, ref this.m_sectorCenter);
      this.DataView.Listener = this;
      dataView?.Close();
      foreach (MyEnvironmentSector.Module module in this.m_modules.Values)
        module.Proxy.Close();
      this.m_modules.Clear();
      int count = this.DataView.Items.Count;
      fixed (ItemInfo* itemInfoPtr = this.DataView.Items.GetInternalArray())
      {
        for (int index = 0; index < count; ++index)
        {
          if (itemInfoPtr[index].IsEnabled && itemInfoPtr[index].DefinitionIndex != (short) -1)
          {
            MyItemTypeDefinition.Module[] proxyModules = this.m_environment.Items[(int) itemInfoPtr[index].DefinitionIndex].Type.ProxyModules;
            if (proxyModules != null)
            {
              foreach (MyItemTypeDefinition.Module module1 in proxyModules)
              {
                MyEnvironmentSector.Module module2;
                if (!this.m_modules.TryGetValue(module1.Type, out module2))
                {
                  module2 = new MyEnvironmentSector.Module((IMyEnvironmentModuleProxy) Activator.CreateInstance(module1.Type));
                  module2.Definition = module1.Definition;
                  this.m_modules[module1.Type] = module2;
                }
                module2.Items.Add(index);
              }
            }
          }
        }
      }
      foreach (KeyValuePair<System.Type, MyEnvironmentSector.Module> module in this.m_modules)
      {
        module.Value.Proxy.Init(this, module.Value.Items);
        module.Value.Items = (List<int>) null;
      }
    }

    public event Action OnPhysicsClose;

    private unsafe void BuildShape()
    {
      this.FetchData(0);
      MyEnvironmentSector.CompoundInstancedShape compoundInstancedShape = new MyEnvironmentSector.CompoundInstancedShape();
      if (this.m_modelsToShapes == null)
        this.m_modelsToShapes = new Dictionary<int, HkShape>();
      int count = this.DataView.Items.Count;
      fixed (ItemInfo* itemInfoPtr = this.DataView.Items.GetInternalArray())
      {
        for (int itemId = 0; itemId < count; ++itemId)
        {
          short modelIndex = itemInfoPtr[itemId].ModelIndex;
          if (modelIndex >= (short) 0 && this.Owner.GetModelForId(modelIndex) != null)
          {
            HkShape shape;
            if (!this.m_modelsToShapes.TryGetValue((int) modelIndex, out shape))
            {
              MyModel modelOnlyData = MyModels.GetModelOnlyData(this.Owner.GetModelForId(modelIndex).Model);
              HkShape[] havokCollisionShapes = modelOnlyData.HavokCollisionShapes;
              if (havokCollisionShapes != null)
              {
                if (havokCollisionShapes.Length == 0)
                  MyLog.Default.Warning("Model {0} has an empty list of shapes, something wrong with export?", (object) modelOnlyData.AssetName);
                else
                  shape = havokCollisionShapes.Length != 1 ? (HkShape) new HkListShape(havokCollisionShapes, HkReferencePolicy.TakeOwnership) : havokCollisionShapes[0];
              }
              this.m_modelsToShapes[(int) modelIndex] = shape;
            }
            compoundInstancedShape.AddInstance(itemId, ref itemInfoPtr[itemId], shape);
          }
        }
      }
      if (!compoundInstancedShape.IsEmpty)
        compoundInstancedShape.Bake();
      this.m_newShape = compoundInstancedShape;
    }

    private void UpdateItemShape(int index, short newModelIndex)
    {
      int shapeInstance;
      if (this.m_activeShape != null && this.m_activeShape.TryGetInstance(index, out shapeInstance))
      {
        this.m_activeShape.Shape.EnableInstance(shapeInstance, newModelIndex >= (short) 0);
      }
      else
      {
        if (this.m_recalculateShape)
          return;
        this.m_recalculateShape = true;
        if (Interlocked.Exchange(ref this.m_hasParallelWorkPending, 1) != 0)
          return;
        this.Owner.ScheduleWork(this, true);
      }
    }

    private void PreparePhysicsBody()
    {
      this.m_activeShape = this.m_newShape;
      this.m_newShape = (MyEnvironmentSector.CompoundInstancedShape) null;
      if (this.Physics != null)
        this.Physics.Close();
      if (this.m_activeShape.IsEmpty)
        return;
      this.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((IMyEntity) this, RigidBodyFlag.RBF_STATIC);
      MyPhysicsBody physics = (MyPhysicsBody) this.Physics;
      HkMassProperties hkMassProperties = new HkMassProperties();
      physics.CreateFromCollisionObject((HkShape) this.m_activeShape.Shape, Vector3.Zero, this.PositionComp.WorldMatrixRef, new HkMassProperties?(hkMassProperties));
      physics.ContactPointCallback += new MyPhysicsBody.PhysicsContactHandler(this.Physics_onContactPoint);
      physics.IsStaticForCluster = true;
      if (this.m_contactListeners != null && this.m_contactListeners.Count != 0)
        this.Physics.RigidBody.ContactPointCallbackEnabled = true;
      this.Physics.Enabled = true;
    }

    private void Physics_onContactPoint(ref MyPhysics.MyContactPointEvent evt)
    {
      MyPhysicsBody physicsBody1 = evt.ContactPointEvent.GetPhysicsBody(0);
      if (physicsBody1 == null)
        return;
      int bodyIdx = physicsBody1.Entity == this ? 0 : 1;
      uint shapeKey = evt.ContactPointEvent.GetShapeKey(bodyIdx);
      if (shapeKey == uint.MaxValue)
        return;
      MyPhysicsBody physicsBody2 = evt.ContactPointEvent.GetPhysicsBody(1 ^ bodyIdx);
      if (physicsBody2 == null)
        return;
      IMyEntity entity = physicsBody2.Entity;
      int itemFromShapeKey = this.GetItemFromShapeKey(shapeKey);
      foreach (MySectorContactEvent contactListener in this.m_contactListeners)
        contactListener(itemFromShapeKey, (MyEntity) entity, ref evt);
    }

    public void BuildInstanceBuffers(int lod)
    {
      Dictionary<short, MyList<MyInstanceData>> dictionary = new Dictionary<short, MyList<MyInstanceData>>();
      this.m_modelToItem = new List<short>(this.DataView.Items.Count);
      Vector3D vector3D = this.SectorCenter - this.m_render.WorldMatrix.Translation;
      int count = this.DataView.Items.Count;
      ItemInfo[] internalArray = this.DataView.Items.GetInternalArray();
      for (int index = 0; index < count; ++index)
      {
        if (internalArray[index].ModelIndex < (short) 0)
        {
          this.m_modelToItem.Add((short) -1);
        }
        else
        {
          MyList<MyInstanceData> myList;
          if (!dictionary.TryGetValue(internalArray[index].ModelIndex, out myList))
          {
            myList = new MyList<MyInstanceData>();
            dictionary[internalArray[index].ModelIndex] = myList;
          }
          Matrix result;
          Matrix.CreateFromQuaternion(ref internalArray[index].Rotation, out result);
          result.Translation = (Vector3) (internalArray[index].Position + vector3D);
          this.m_modelToItem.Add((short) myList.Count);
          myList.Add(new MyInstanceData(result));
        }
      }
      foreach (KeyValuePair<short, MyList<MyInstanceData>> keyValuePair in dictionary)
      {
        MyPhysicalModelDefinition modelForId = this.m_owner.GetModelForId(keyValuePair.Key);
        if (modelForId != null)
          this.m_render.AddInstances(MyModel.GetId(modelForId.Model), keyValuePair.Value);
      }
    }

    private void UpdateItemModel(int index, short newModelIndex)
    {
      ItemInfo itemInfo = this.DataView.Items[index];
      if ((int) itemInfo.ModelIndex == (int) newModelIndex)
        return;
      if (this.m_currentLod == this.m_lodToSwitch)
      {
        if (itemInfo.ModelIndex >= (short) 0 && this.m_owner.GetModelForId(itemInfo.ModelIndex) != null)
        {
          this.m_render.RemoveInstance(MyModel.GetId(this.m_owner.GetModelForId(itemInfo.ModelIndex).Model), this.m_modelToItem[index]);
          this.m_modelToItem[index] = (short) -1;
        }
        if (newModelIndex >= (short) 0 && this.m_owner.GetModelForId(newModelIndex) != null)
        {
          int id = MyModel.GetId(this.m_owner.GetModelForId(newModelIndex).Model);
          Vector3D vector3D = this.SectorCenter - this.m_render.WorldMatrix.Translation;
          Matrix result;
          Matrix.CreateFromQuaternion(ref itemInfo.Rotation, out result);
          result.Translation = (Vector3) (itemInfo.Position + vector3D);
          MyInstanceData data = new MyInstanceData(result);
          this.m_modelToItem[index] = this.m_render.AddInstance(id, ref data);
        }
      }
      itemInfo.ModelIndex = newModelIndex;
      this.DataView.Items[index] = itemInfo;
    }

    public void GetItemInfo(int itemId, out uint renderObjectId, out int instanceIndex)
    {
      int id = MyModel.GetId(this.m_owner.GetModelForId(this.DataView.Items[itemId].ModelIndex).Model);
      renderObjectId = this.m_render.GetRenderEntity(id);
      instanceIndex = (int) this.m_modelToItem[itemId];
    }

    public IMyEnvironmentOwner Owner => this.m_owner;

    public void EnableItem(int itemId, bool enabled)
    {
      int logicalItem;
      MyLogicalEnvironmentSectorBase sector;
      this.DataView.GetLogicalSector(itemId, out logicalItem, out sector);
      sector.EnableItem(logicalItem, enabled);
    }

    public void ReEnableSectorItem(int itemId)
    {
      MyLogicalEnvironmentSectorBase sector;
      this.DataView.GetLogicalSector(itemId, out int _, out sector);
      sector.RevalidateItem(itemId);
    }

    public int GetItemDefinitionId(int itemId)
    {
      int logicalItem;
      MyLogicalEnvironmentSectorBase sector;
      this.DataView.GetLogicalSector(itemId, out logicalItem, out sector);
      return sector.GetItemDefinitionId(logicalItem);
    }

    public int GetItemFromShapeKey(uint shapekey)
    {
      int instanceId;
      this.m_activeShape.Shape.DecomposeShapeKey(shapekey, out instanceId, out uint _);
      return this.m_activeShape.GetItemId(instanceId);
    }

    public event MySectorContactEvent OnContactPoint
    {
      add
      {
        if (this.m_contactListeners == null)
          this.m_contactListeners = new HashSet<MySectorContactEvent>();
        if (this.m_contactListeners.Count == 0 && this.Physics != null && (HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null)
          this.Physics.RigidBody.ContactPointCallbackEnabled = true;
        this.m_contactListeners.Add(value);
      }
      remove
      {
        if (this.m_contactListeners == null)
          return;
        this.m_contactListeners.Remove(value);
        if (this.m_contactListeners.Count != 0 || this.Physics == null || !((HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null))
          return;
        this.Physics.RigidBody.ContactPointCallbackEnabled = false;
      }
    }

    public T GetModuleForDefinition<T>(MyRuntimeEnvironmentItemInfo itemEnvDefinition) where T : class, IMyEnvironmentModuleProxy
    {
      MyItemTypeDefinition.Module[] proxyModules = itemEnvDefinition.Type.ProxyModules;
      if (proxyModules == null || !((IEnumerable<MyItemTypeDefinition.Module>) proxyModules).Any<MyItemTypeDefinition.Module>((Func<MyItemTypeDefinition.Module, bool>) (x => typeof (T).IsAssignableFrom(x.Type))))
        return default (T);
      MyEnvironmentSector.Module module;
      this.m_modules.TryGetValue(typeof (T), out module);
      return (T) module?.Proxy;
    }

    public T GetModule<T>() where T : class, IMyEnvironmentModuleProxy
    {
      MyEnvironmentSector.Module module;
      this.m_modules.TryGetValue(typeof (T), out module);
      return (T) module?.Proxy;
    }

    public IMyEnvironmentModuleProxy GetModule(System.Type moduleType)
    {
      MyEnvironmentSector.Module module;
      this.m_modules.TryGetValue(moduleType, out module);
      return module?.Proxy;
    }

    public void RaiseItemEvent<TModule>(TModule module, int item, bool fromClient = false) where TModule : IMyEnvironmentModuleProxy => this.RaiseItemEvent<TModule, object>(module, item, (object) null, fromClient);

    public void RaiseItemEvent<TModule, TArgument>(
      TModule module,
      int item,
      TArgument eventData,
      bool fromClient = false)
      where TModule : IMyEnvironmentModuleProxy
    {
      MyDefinitionId definition = this.m_modules[typeof (TModule)].Definition;
      int logicalItem;
      MyLogicalEnvironmentSectorBase sector;
      this.DataView.GetLogicalSector(item, out logicalItem, out sector);
      sector.RaiseItemEvent<TArgument>(logicalItem, ref definition, eventData, fromClient);
    }

    public new void DebugDraw()
    {
      if (this.LodLevel < 0 && !this.HasPhysics)
        return;
      Color color1 = Color.Red;
      if (MyPlanetEnvironmentSessionComponent.ActiveSector == this)
      {
        color1 = Color.LimeGreen;
        if (this.DataView != null)
        {
          if (MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems)
          {
            for (int index = 0; index < this.DataView.Items.Count; ++index)
            {
              ItemInfo itemInfo = this.DataView.Items[index];
              Vector3D worldCoord = itemInfo.Position + this.SectorCenter;
              MyRuntimeEnvironmentItemInfo def;
              this.Owner.GetDefinition((ushort) itemInfo.DefinitionIndex, out def);
              string text = string.Format("{0} i{1} m{2} d{3}", (object) def.Type.Name, (object) index, (object) itemInfo.ModelIndex, (object) itemInfo.DefinitionIndex);
              Color color2 = color1;
              MyRenderProxy.DebugDrawText3D(worldCoord, text, color2, 0.7f, true);
            }
          }
          if (MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider)
          {
            foreach (MyLogicalEnvironmentSectorBase logicalSector in this.DataView.LogicalSectors)
              logicalSector.DebugDraw(this.DataView.Lod);
          }
        }
      }
      else if (this.HasPhysics && this.LodLevel == -1)
        color1 = Color.RoyalBlue;
      Vector3D worldCoord1 = (this.Bounds[4] + this.Bounds[7]) / 2.0;
      if (MyPlanetEnvironmentSessionComponent.ActiveSector == this || Vector3D.DistanceSquared(worldCoord1, MySector.MainCamera.Position) < (double) MyPlanetEnvironmentSessionComponent.DebugDrawDistance * (double) MyPlanetEnvironmentSessionComponent.DebugDrawDistance)
      {
        string text = this.ToString();
        MyRenderProxy.DebugDrawText3D(worldCoord1, text, color1, 1f, true);
      }
      MyRenderProxy.DebugDraw6FaceConvex(this.Bounds, color1, 1f, true, false);
    }

    public override string ToString()
    {
      long sectorId = this.SectorId;
      int num1 = (int) (sectorId & 16777215L);
      long num2 = sectorId >> 24;
      int num3 = (int) (num2 & 16777215L);
      long num4 = num2 >> 24;
      int num5 = (int) (num4 & 7L);
      int num6 = (int) (num4 >> 3 & (long) byte.MaxValue);
      return string.Format("S(x{0} y{1} f{2} l{3}({4}) c{6} {5})", (object) num1, (object) num3, (object) num5, (object) num6, (object) this.LodLevel, this.HasPhysics ? (object) " p" : (object) "", (object) (this.DataView != null ? this.DataView.Items.Count : 0));
    }

    public override int GetHashCode() => this.SectorId.GetHashCode();

    public event Action<MyEnvironmentSector, int> OnLodCommit;

    public void RaiseOnLodCommitEvent(int lod)
    {
      if (this.OnLodCommit == null)
        return;
      this.OnLodCommit(this, lod);
    }

    public event Action<MyEnvironmentSector, bool> OnPhysicsCommit;

    public void RaiseOnPhysicsCommitEvent(bool enabled)
    {
      if (this.OnPhysicsCommit == null)
        return;
      this.OnPhysicsCommit(this, enabled);
    }

    public short GetModelIndex(int itemId)
    {
      int logicalItem;
      MyLogicalEnvironmentSectorBase sector;
      this.DataView.GetLogicalSector(itemId, out logicalItem, out sector);
      ItemInfo itemInfo;
      sector.GetItem(logicalItem, out itemInfo);
      return itemInfo.ModelIndex;
    }

    private struct LodHEntry
    {
      public int Lod;
      public bool Set;
      public StackTrace Trace;

      public override string ToString() => string.Format("{0} {1} @ {2}", this.Set ? (object) "Set" : (object) "Requested", (object) this.Lod, (object) this.Trace.GetFrame(1));
    }

    private class CompoundInstancedShape
    {
      public HkStaticCompoundShape Shape = new HkStaticCompoundShape(HkReferencePolicy.TakeOwnership);
      private readonly Dictionary<int, int> m_itemToShapeInstance = new Dictionary<int, int>();
      private readonly Dictionary<int, int> m_shapeInstanceToItem = new Dictionary<int, int>();
      private bool m_baked;

      public bool IsEmpty => this.Shape.InstanceCount == 0;

      public void AddInstance(int itemId, ref ItemInfo item, HkShape shape)
      {
        if (shape.IsZero)
          return;
        Matrix result;
        Matrix.CreateFromQuaternion(ref item.Rotation, out result);
        result.Translation = item.Position;
        int key = this.Shape.AddInstance(shape, result);
        this.m_itemToShapeInstance[itemId] = key;
        this.m_shapeInstanceToItem[key] = itemId;
      }

      public void Bake()
      {
        this.Shape.Bake();
        this.m_baked = true;
      }

      public bool TryGetInstance(int itemId, out int shapeInstance) => this.m_itemToShapeInstance.TryGetValue(itemId, out shapeInstance);

      public bool TryGetItemId(int shapeInstance, out int itemId) => this.m_shapeInstanceToItem.TryGetValue(shapeInstance, out itemId);

      public int GetItemId(int shapeInstance) => this.m_shapeInstanceToItem[shapeInstance];
    }

    private class Module
    {
      public readonly IMyEnvironmentModuleProxy Proxy;
      public List<int> Items = new List<int>();
      public MyDefinitionId Definition;

      public Module(IMyEnvironmentModuleProxy proxy) => this.Proxy = proxy;
    }

    private class Sandbox_Game_WorldEnvironment_MyEnvironmentSector\u003C\u003EActor : IActivator, IActivator<MyEnvironmentSector>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentSector();

      MyEnvironmentSector IActivator<MyEnvironmentSector>.CreateInstance() => new MyEnvironmentSector();
    }
  }
}
