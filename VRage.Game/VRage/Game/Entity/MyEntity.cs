// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyEntity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage.Game.Components;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.Networking;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace VRage.Game.Entity
{
  [GenerateActivator]
  [MyEntityType(typeof (MyObjectBuilder_EntityBase), true)]
  public class MyEntity : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    protected readonly VRage.Sync.Sync<MyEntity.ContactPointData, SyncDirection.FromServer> m_contactPoint;
    public MyDefinitionId? DefinitionId;
    public string Name;
    public bool DebugAsyncLoading;
    private List<MyEntity> m_tmpOnPhysicsChanged = new List<MyEntity>();
    protected List<MyHudEntityParams> m_hudParams = new List<MyHudEntityParams>();
    private string m_displayNameText;
    private MyPositionComponentBase m_position;
    public bool m_positionResetFromServer;
    private MyRenderComponentBase m_render;
    private List<MyDebugRenderComponentBase> m_debugRenderers = new List<MyDebugRenderComponentBase>();
    protected MyModel m_modelCollision;
    public int GamePruningProxyId = -1;
    public int TopMostPruningProxyId = -1;
    public bool StaticForPruningStructure;
    public int TargetPruningProxyId = -1;
    private bool m_raisePhysicsCalled;
    private long m_pins;
    private MyGameLogicComponent m_gameLogic;
    private long m_entityId;
    private MySyncComponentBase m_syncObject;
    private MyModStorageComponentBase m_storage;
    private MyEntityStorageComponent m_entityStorage;
    private bool m_isPreview;
    public Action<bool> IsPreviewChanged;
    private bool m_isreadyForReplication;
    public Dictionary<IMyReplicable, Action> ReadyForReplicationAction = new Dictionary<IMyReplicable, Action>();
    private MyHierarchyComponent<MyEntity> m_hierarchy;
    private MyPhysicsComponentBase m_physics;
    private string m_displayName;
    public Action ReplicationStarted;
    public Action ReplicationEnded;
    public Action<MyEntity> OnEntityCloseRequest;
    public static Action<MyEntity> AddToGamePruningStructureExtCallBack;
    public static Action<MyEntity> RemoveFromGamePruningStructureExtCallBack;
    public static Action<MyEntity> UpdateGamePruningStructureExtCallBack;
    public static MyEntity.MyEntityFactoryCreateObjectBuilderDelegate MyEntityFactoryCreateObjectBuilderExtCallback;
    public static MyEntity.CreateDefaultSyncEntityDelegate CreateDefaultSyncEntityExtCallback;
    public static Action<MyEntity> MyWeldingGroupsAddNodeExtCallback;
    public static Action<MyEntity> MyWeldingGroupsRemoveNodeExtCallback;
    public static Action<MyEntity, List<MyEntity>> MyWeldingGroupsGetGroupNodesExtCallback;
    public static MyEntity.MyWeldingGroupsGroupExistsDelegate MyWeldingGroupsGroupExistsExtCallback;
    public static Action<MyEntity> MyProceduralWorldGeneratorTrackEntityExtCallback;
    public static Action<MyEntity> CreateStandardRenderComponentsExtCallback;
    public static Action<MyComponentContainer, MyObjectBuilderType, MyStringHash, MyObjectBuilder_ComponentContainer> InitComponentsExtCallback;
    public static Func<MyObjectBuilder_EntityBase, bool, MyEntity> MyEntitiesCreateFromObjectBuilderExtCallback;

    public MyEntityComponentContainer Components { get; private set; }

    public MyPositionComponentBase PositionComp
    {
      get => this.m_position;
      set => this.Components.Add<MyPositionComponentBase>(value);
    }

    public MyRenderComponentBase Render
    {
      get => this.m_render;
      set => this.Components.Add<MyRenderComponentBase>(value);
    }

    public void DebugDraw()
    {
      if (this.Hierarchy != null)
      {
        foreach (MyEntityComponentBase child in this.Hierarchy.Children)
          child.Container.Entity.DebugDraw();
      }
      foreach (MyDebugRenderComponentBase debugRenderer in this.m_debugRenderers)
        debugRenderer.DebugDraw();
    }

    public void DebugDrawInvalidTriangles()
    {
      foreach (MyDebugRenderComponentBase debugRenderer in this.m_debugRenderers)
        debugRenderer.DebugDrawInvalidTriangles();
    }

    public void AddDebugRenderComponent(MyDebugRenderComponentBase render) => this.m_debugRenderers.Add(render);

    public bool ContainsDebugRenderComponent(Type render)
    {
      foreach (object debugRenderer in this.m_debugRenderers)
      {
        if (debugRenderer.GetType() == render)
          return true;
      }
      return false;
    }

    public void RemoveDebugRenderComponent(Type t)
    {
      int count = this.m_debugRenderers.Count;
      while (count > 0)
      {
        --count;
        if (this.m_debugRenderers[count].GetType() == t)
          this.m_debugRenderers.RemoveAt(count);
      }
    }

    public void RemoveDebugRenderComponent(MyDebugRenderComponentBase render) => this.m_debugRenderers.Remove(render);

    public void ClearDebugRenderComponents() => this.m_debugRenderers.Clear();

    public virtual MyGameLogicComponent GameLogic
    {
      get => this.m_gameLogic;
      set => this.Components.Add<MyGameLogicComponent>(value);
    }

    public long EntityId
    {
      get => this.m_entityId;
      set
      {
        if (this.m_entityId != 0L)
        {
          long entityId = this.m_entityId;
          if (value == 0L)
          {
            this.m_entityId = 0L;
            MyEntityIdentifier.RemoveEntity(entityId);
          }
          else
          {
            this.m_entityId = value;
            MyEntityIdentifier.SwapRegisteredEntityId((VRage.ModAPI.IMyEntity) this, entityId, this.m_entityId);
          }
        }
        else
        {
          if (value == 0L)
            return;
          this.m_entityId = value;
          MyEntityIdentifier.AddEntityWithId((VRage.ModAPI.IMyEntity) this);
        }
      }
    }

    public MySyncComponentBase SyncObject
    {
      get => this.m_syncObject;
      protected set => this.Components.Add<MySyncComponentBase>(value);
    }

    public MyModStorageComponentBase Storage
    {
      get => this.m_storage;
      set => this.Components.Add<MyModStorageComponentBase>(value);
    }

    public MyEntityStorageComponent EntityStorage
    {
      get => this.m_entityStorage;
      set => this.Components.Add<MyEntityStorageComponent>(value);
    }

    public bool Closed { get; protected set; }

    public bool MarkedForClose { get; protected set; }

    public virtual float MaxGlassDistSq
    {
      get
      {
        IMyCamera myCamera = MyAPIGatewayShortcuts.GetMainCamera != null ? MyAPIGatewayShortcuts.GetMainCamera() : (IMyCamera) null;
        return myCamera != null ? 0.01f * myCamera.FarPlaneDistance * myCamera.FarPlaneDistance : 4000000f;
      }
    }

    public bool Save
    {
      get => (uint) (this.Flags & EntityFlags.Save) > 0U;
      set
      {
        if (value)
          this.Flags |= EntityFlags.Save;
        else
          this.Flags &= ~EntityFlags.Save;
      }
    }

    public bool IsPreview
    {
      get => this.m_isPreview;
      set
      {
        if (this.m_isPreview == value)
          return;
        this.m_isPreview = value;
        this.IsPreviewChanged.InvokeIfNotNull<bool>(value);
      }
    }

    public bool IsReadyForReplication
    {
      get => this.m_isreadyForReplication;
      private set
      {
        this.m_isreadyForReplication = value;
        if (!this.m_isreadyForReplication || this.ReadyForReplicationAction.Count <= 0)
          return;
        foreach (Action action in this.ReadyForReplicationAction.Values)
          action();
        this.ReadyForReplicationAction.Clear();
      }
    }

    public MyEntityUpdateEnum NeedsUpdate
    {
      get
      {
        MyEntityUpdateEnum entityUpdateEnum = MyEntityUpdateEnum.NONE;
        if ((this.Flags & EntityFlags.NeedsUpdate) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_FRAME;
        if ((this.Flags & EntityFlags.NeedsUpdate10) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        if ((this.Flags & EntityFlags.NeedsUpdate100) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        if ((this.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if ((this.Flags & EntityFlags.NeedsSimulate) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.SIMULATE;
        return entityUpdateEnum;
      }
      set
      {
        MyEntityUpdateEnum needsUpdate = this.NeedsUpdate;
        if (value == needsUpdate)
          return;
        if (this.InScene)
          MyEntitiesInterface.UnregisterUpdate(this, false);
        this.Flags &= ~EntityFlags.NeedsUpdateBeforeNextFrame;
        this.Flags &= ~EntityFlags.NeedsUpdate;
        this.Flags &= ~EntityFlags.NeedsUpdate10;
        this.Flags &= ~EntityFlags.NeedsUpdate100;
        this.Flags &= ~EntityFlags.NeedsSimulate;
        if ((value & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != MyEntityUpdateEnum.NONE)
          this.Flags |= EntityFlags.NeedsUpdateBeforeNextFrame;
        if ((value & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE)
          this.Flags |= EntityFlags.NeedsUpdate;
        if ((value & MyEntityUpdateEnum.EACH_10TH_FRAME) != MyEntityUpdateEnum.NONE)
          this.Flags |= EntityFlags.NeedsUpdate10;
        if ((value & MyEntityUpdateEnum.EACH_100TH_FRAME) != MyEntityUpdateEnum.NONE)
          this.Flags |= EntityFlags.NeedsUpdate100;
        if ((value & MyEntityUpdateEnum.SIMULATE) != MyEntityUpdateEnum.NONE)
          this.Flags |= EntityFlags.NeedsSimulate;
        if (!this.InScene)
          return;
        MyEntitiesInterface.RegisterUpdate(this);
      }
    }

    public MatrixD WorldMatrix
    {
      get => this.PositionComp == null ? MatrixD.Zero : this.PositionComp.WorldMatrixRef;
      set
      {
        if (this.PositionComp == null)
          return;
        this.PositionComp.SetWorldMatrix(ref value);
      }
    }

    public MyEntity Parent
    {
      get => this.m_hierarchy?.Parent?.Container.Entity as MyEntity;
      private set => this.m_hierarchy.Parent = (MyHierarchyComponentBase) value.Hierarchy;
    }

    public MyEntity GetTopMostParent(Type type = null)
    {
      MyEntity myEntity = this;
      while (myEntity.Parent != null && (type == (Type) null || !myEntity.GetType().IsSubclassOf(type)))
        myEntity = myEntity.Parent;
      return myEntity;
    }

    public MyHierarchyComponent<MyEntity> Hierarchy
    {
      get => this.m_hierarchy;
      set => this.Components.Add<MyHierarchyComponentBase>((MyHierarchyComponentBase) value);
    }

    MyHierarchyComponentBase VRage.ModAPI.IMyEntity.Hierarchy
    {
      get => (MyHierarchyComponentBase) this.m_hierarchy;
      set
      {
        if (!(value is MyHierarchyComponent<MyEntity>))
          return;
        this.Components.Add<MyHierarchyComponentBase>(value);
      }
    }

    MyPhysicsComponentBase VRage.ModAPI.IMyEntity.Physics
    {
      get => this.m_physics;
      set => this.Components.Add<MyPhysicsComponentBase>(value);
    }

    public MyPhysicsComponentBase Physics
    {
      get => this.m_physics;
      set
      {
        MyPhysicsComponentBase physics = this.m_physics;
        this.Components.Add<MyPhysicsComponentBase>(value);
        this.OnPhysicsComponentChanged.InvokeIfNotNull<MyPhysicsComponentBase, MyPhysicsComponentBase>(physics, value);
      }
    }

    public bool InvalidateOnMove
    {
      get => (uint) (this.Flags & EntityFlags.InvalidateOnMove) > 0U;
      set
      {
        if (value)
          this.Flags |= EntityFlags.InvalidateOnMove;
        else
          this.Flags &= ~EntityFlags.InvalidateOnMove;
      }
    }

    public bool SyncFlag
    {
      get => (uint) (this.Flags & EntityFlags.Sync) > 0U;
      set => this.Flags = value ? this.Flags | EntityFlags.Sync : this.Flags & ~EntityFlags.Sync;
    }

    public bool NeedsWorldMatrix
    {
      get => (uint) (this.Flags & EntityFlags.NeedsWorldMatrix) > 0U;
      set
      {
        this.Flags = value ? this.Flags | EntityFlags.NeedsWorldMatrix : this.Flags & ~EntityFlags.NeedsWorldMatrix;
        this.Hierarchy?.UpdateNeedsWorldMatrix();
      }
    }

    public bool InScene
    {
      get => this.Render != null && (this.Render.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
      set
      {
        if (this.Render == null)
          return;
        if (value)
          this.Render.PersistentFlags |= MyPersistentEntityFlags2.InScene;
        else
          this.Render.PersistentFlags &= ~MyPersistentEntityFlags2.InScene;
      }
    }

    public virtual bool IsVolumetric => false;

    public virtual Vector3D LocationForHudMarker => this.PositionComp == null ? Vector3D.Zero : this.PositionComp.GetPosition();

    public virtual List<MyHudEntityParams> GetHudParams(bool allowBlink) => this.m_hudParams;

    protected virtual bool CanBeAddedToRender() => true;

    public MyModel Model => this.Render.GetModel();

    public MyModel ModelCollision => this.m_modelCollision != null ? this.m_modelCollision : this.Render.GetModel();

    public string DisplayName
    {
      get => this.m_displayName;
      set => this.m_displayName = value;
    }

    public string DebugName => ((this.m_displayName ?? this.Name) ?? "") + " (" + this.GetType().Name + ", " + this.EntityId.ToString() + ")";

    public Dictionary<string, MyEntitySubpart> Subparts { get; private set; }

    public virtual bool IsCCDForProjectiles => false;

    public bool Pinned => Interlocked.Read(ref this.m_pins) > 0L;

    public bool IsReplicated { get; private set; }

    public MyEntity()
    {
      this.Components = new MyEntityComponentContainer((VRage.ModAPI.IMyEntity) this);
      this.Components.ComponentAdded += new Action<Type, MyEntityComponentBase>(this.Components_ComponentAdded);
      this.Components.ComponentRemoved += new Action<Type, MyEntityComponentBase>(this.Components_ComponentRemoved);
      this.Flags = EntityFlags.Default;
      this.InitComponents();
    }

    public virtual void InitComponents()
    {
      if (this.Hierarchy == null)
        this.Hierarchy = new MyHierarchyComponent<MyEntity>();
      if (this.GameLogic == null)
        this.GameLogic = (MyGameLogicComponent) new MyNullGameLogicComponent();
      if (this.PositionComp == null)
        this.PositionComp = (MyPositionComponentBase) new MyPositionComponent();
      this.PositionComp.SetLocalMatrix(ref Matrix.Identity);
      if (this.Render != null)
        return;
      MyEntity.CreateStandardRenderComponentsExtCallback(this);
    }

    private void Components_ComponentAdded(Type t, MyEntityComponentBase c)
    {
      if (typeof (MyPhysicsComponentBase).IsAssignableFrom(t))
        this.m_physics = c as MyPhysicsComponentBase;
      else if (typeof (MySyncComponentBase).IsAssignableFrom(t))
        this.m_syncObject = c as MySyncComponentBase;
      else if (typeof (MyGameLogicComponent).IsAssignableFrom(t))
        this.m_gameLogic = c as MyGameLogicComponent;
      else if (typeof (MyPositionComponentBase).IsAssignableFrom(t))
      {
        this.m_position = c as MyPositionComponentBase;
        if (this.m_position != null)
          return;
        this.PositionComp = (MyPositionComponentBase) new MyNullPositionComponent();
      }
      else if (typeof (MyHierarchyComponentBase).IsAssignableFrom(t))
        this.m_hierarchy = c as MyHierarchyComponent<MyEntity>;
      else if (typeof (MyRenderComponentBase).IsAssignableFrom(t))
      {
        this.m_render = c as MyRenderComponentBase;
        if (this.m_render != null)
          return;
        this.Render = (MyRenderComponentBase) new MyNullRenderComponent();
      }
      else if (typeof (MyInventoryBase).IsAssignableFrom(t))
        this.OnInventoryComponentAdded(c as MyInventoryBase);
      else if (typeof (MyModStorageComponentBase).IsAssignableFrom(t))
      {
        this.m_storage = c as MyModStorageComponentBase;
      }
      else
      {
        if (!typeof (MyEntityStorageComponent).IsAssignableFrom(t))
          return;
        this.m_entityStorage = c as MyEntityStorageComponent;
      }
    }

    private void Components_ComponentRemoved(Type t, MyEntityComponentBase c)
    {
      if (typeof (MyPhysicsComponentBase).IsAssignableFrom(t))
        this.m_physics = (MyPhysicsComponentBase) null;
      else if (typeof (MySyncComponentBase).IsAssignableFrom(t))
        this.m_syncObject = (MySyncComponentBase) null;
      else if (typeof (MyGameLogicComponent).IsAssignableFrom(t))
        this.m_gameLogic = (MyGameLogicComponent) null;
      else if (typeof (MyPositionComponentBase).IsAssignableFrom(t))
        this.PositionComp = (MyPositionComponentBase) new MyNullPositionComponent();
      else if (typeof (MyHierarchyComponentBase).IsAssignableFrom(t))
        this.m_hierarchy = (MyHierarchyComponent<MyEntity>) null;
      else if (typeof (MyRenderComponentBase).IsAssignableFrom(t))
        this.Render = (MyRenderComponentBase) new MyNullRenderComponent();
      else if (typeof (MyInventoryBase).IsAssignableFrom(t))
        this.OnInventoryComponentRemoved(c as MyInventoryBase);
      else if (typeof (MyModStorageComponentBase).IsAssignableFrom(t))
      {
        this.m_storage = (MyModStorageComponentBase) null;
      }
      else
      {
        if (!typeof (MyEntityStorageComponent).IsAssignableFrom(t))
          return;
        this.m_entityStorage = (MyEntityStorageComponent) null;
      }
    }

    protected virtual MySyncComponentBase OnCreateSync() => MyEntity.CreateDefaultSyncEntityExtCallback(this);

    public void CreateSync() => this.SyncObject = this.OnCreateSync();

    public MyEntitySubpart GetSubpart(string name) => this.Subparts[name];

    public bool TryGetSubpart(string name, out MyEntitySubpart subpart) => this.Subparts.TryGetValue(name, out subpart);

    public virtual void UpdateOnceBeforeFrame() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateOnceBeforeFrame(true);

    public virtual void UpdateBeforeSimulation() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateBeforeSimulation(true);

    public virtual void Simulate()
    {
    }

    public virtual void UpdateAfterSimulation() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateAfterSimulation(true);

    public virtual void UpdatingStopped()
    {
    }

    public virtual void UpdateBeforeSimulation10() => ((IMyGameLogicComponent) this.m_gameLogic)?.UpdateBeforeSimulation10(true);

    public virtual void UpdateAfterSimulation10() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateAfterSimulation10(true);

    public virtual void UpdateBeforeSimulation100() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateBeforeSimulation100(true);

    public virtual void UpdateAfterSimulation100() => ((IMyGameLogicComponent) this.m_gameLogic).UpdateAfterSimulation100(true);

    public virtual string GetFriendlyName() => string.Empty;

    public virtual MatrixD GetViewMatrix() => this.PositionComp.WorldMatrixNormalizedInv;

    public virtual void Teleport(MatrixD worldMatrix, object source = null, bool ignoreAssert = false)
    {
      if (this.Closed || this.Hierarchy == null)
        return;
      HashSet<VRage.ModAPI.IMyEntity> myEntitySet1 = new HashSet<VRage.ModAPI.IMyEntity>();
      HashSet<VRage.ModAPI.IMyEntity> myEntitySet2 = new HashSet<VRage.ModAPI.IMyEntity>();
      myEntitySet1.Add((VRage.ModAPI.IMyEntity) this);
      this.Hierarchy.GetChildrenRecursive(myEntitySet1);
      foreach (VRage.ModAPI.IMyEntity myEntity in myEntitySet1)
      {
        if (myEntity.Physics != null)
        {
          if (myEntity.Physics.Enabled)
            myEntity.Physics.Enabled = false;
          else
            myEntitySet2.Add(myEntity);
        }
      }
      this.PositionComp.SetWorldMatrix(ref worldMatrix, source, skipTeleportCheck: true, ignoreAssert: ignoreAssert);
      foreach (VRage.ModAPI.IMyEntity myEntity in myEntitySet1.Reverse<VRage.ModAPI.IMyEntity>())
      {
        if (myEntity.Physics != null && !myEntitySet2.Contains(myEntity))
          myEntity.Physics.Enabled = true;
      }
      if (this.OnTeleport == null)
        return;
      this.OnTeleport(this);
    }

    public virtual void DebugDrawPhysics()
    {
      foreach (MyEntityComponentBase child in this.Hierarchy.Children)
        (child.Container.Entity as MyEntity).DebugDrawPhysics();
      if (this.m_physics == null || this.GetDistanceBetweenCameraAndBoundingSphere() > 200.0)
        return;
      this.m_physics.DebugDraw();
    }

    public virtual bool GetIntersectionWithLine(
      ref LineD line,
      out Vector3D? v,
      bool useCollisionModel = true,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      v = new Vector3D?();
      MyModel myModel = this.Model;
      if (useCollisionModel)
        myModel = this.ModelCollision;
      if (myModel != null)
      {
        MyIntersectionResultLineTriangleEx? intersectionWithLine = myModel.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, flags);
        if (intersectionWithLine.HasValue)
        {
          v = new Vector3D?(intersectionWithLine.Value.IntersectionPointInWorldSpace);
          return true;
        }
      }
      return false;
    }

    public virtual bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      bool flag = false;
      t = new MyIntersectionResultLineTriangleEx?();
      MyModel model = this.Model;
      if (model != null)
      {
        MyIntersectionResultLineTriangleEx? intersectionWithLine = model.GetTrianglePruningStructure().GetIntersectionWithLine((VRage.ModAPI.IMyEntity) this, ref line, flags);
        if (intersectionWithLine.HasValue)
        {
          t = new MyIntersectionResultLineTriangleEx?(intersectionWithLine.Value);
          flag = true;
        }
      }
      return flag;
    }

    internal virtual bool GetIntersectionsWithLine(
      ref LineD line,
      List<MyIntersectionResultLineTriangleEx> result,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      this.Model?.GetTrianglePruningStructure().GetTrianglesIntersectingLine((VRage.ModAPI.IMyEntity) this, ref line, flags, result);
      return result.Count > 0;
    }

    public virtual Vector3D? GetIntersectionWithLineAndBoundingSphere(
      ref LineD line,
      float boundingSphereRadiusMultiplier)
    {
      if (this.Render.GetModel() == null)
        return new Vector3D?();
      BoundingSphereD worldVolume = this.PositionComp.WorldVolume;
      worldVolume.Radius *= (double) boundingSphereRadiusMultiplier;
      return !MyUtils.IsLineIntersectingBoundingSphere(ref line, ref worldVolume) ? new Vector3D?() : new Vector3D?(worldVolume.Center);
    }

    public virtual bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
    {
      MyModel model = this.Model;
      return model != null && model.GetTrianglePruningStructure().GetIntersectionWithSphere((VRage.ModAPI.IMyEntity) this, ref sphere);
    }

    public virtual bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      MyModel model = this.Model;
      return model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) this, ref aabb);
    }

    public void GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles)
    {
      this.Model?.GetTrianglePruningStructure().GetTrianglesIntersectingSphere(ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
    }

    public virtual bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos) => false;

    public double GetSmallestDistanceBetweenCameraAndBoundingSphere()
    {
      Vector3D position = MyAPIGatewayShortcuts.GetMainCamera().Position;
      BoundingSphereD worldVolume = this.PositionComp.WorldVolume;
      return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref position, ref worldVolume);
    }

    public double GetLargestDistanceBetweenCameraAndBoundingSphere()
    {
      Vector3D position = MyAPIGatewayShortcuts.GetMainCamera().Position;
      BoundingSphereD worldVolume = this.PositionComp.WorldVolume;
      return MyUtils.GetLargestDistanceToSphere(ref position, ref worldVolume);
    }

    public double GetDistanceBetweenCameraAndBoundingSphere()
    {
      Vector3D position = MyAPIGatewayShortcuts.GetMainCamera().Position;
      BoundingSphereD worldVolume = this.PositionComp.WorldVolume;
      return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref position, ref worldVolume);
    }

    public double GetDistanceBetweenPlayerPositionAndBoundingSphere()
    {
      Vector3D from = MyAPIGatewayShortcuts.GetLocalPlayerPosition();
      BoundingSphereD worldVolume = this.PositionComp.WorldVolume;
      return MyUtils.GetSmallestDistanceToSphereAlwaysPositive(ref from, ref worldVolume);
    }

    public double GetDistanceBetweenCameraAndPosition() => Vector3D.Distance(MyAPIGatewayShortcuts.GetMainCamera().Position, this.PositionComp.GetPosition());

    public virtual MyEntity GetBaseEntity() => this;

    public virtual void OnAddedToScene(object source)
    {
      if (!this.IsPreview)
        this.SetReadyForReplication();
      this.InScene = true;
      MyEntitiesInterface.RegisterUpdate(this);
      if (this.GameLogic != null)
        ((IMyGameLogicComponent) this.GameLogic).RegisterForUpdate();
      if (this.Render.NeedsDraw)
        MyEntitiesInterface.RegisterDraw(this);
      if (this.m_physics != null)
        this.m_physics.Activate();
      this.AddToGamePruningStructure();
      this.Components.OnAddedToScene();
      if (this.Hierarchy != null)
      {
        foreach (MyHierarchyComponentBase child in this.Hierarchy.Children)
        {
          if (!child.Container.Entity.InScene)
            child.Container.Entity.OnAddedToScene(source);
        }
      }
      if ((this.Flags & EntityFlags.UpdateRender) > (EntityFlags) 0)
        this.Render.UpdateRenderObject(true, false);
      MyEntity.MyProceduralWorldGeneratorTrackEntityExtCallback(this);
      this.AddedToScene.InvokeIfNotNull<MyEntity>(this);
    }

    private void SetReadyForReplication()
    {
      this.IsReadyForReplication = true;
      if (this.Hierarchy == null)
        return;
      foreach (MyEntityComponentBase child in this.Hierarchy.Children)
        ((MyEntity) child.Entity).SetReadyForReplication();
    }

    public virtual void OnReplicationStarted()
    {
      this.IsReplicated = true;
      this.ReplicationStarted.InvokeIfNotNull();
    }

    public virtual void OnReplicationEnded()
    {
      this.IsReplicated = false;
      this.ReplicationEnded.InvokeIfNotNull();
    }

    public void SetFadeOut(bool state)
    {
      this.Render.FadeOut = state;
      if (this.Hierarchy == null)
        return;
      foreach (MyEntityComponentBase child in this.Hierarchy.Children)
        child.Container.Entity.Render.FadeOut = state;
    }

    public virtual void OnRemovedFromScene(object source)
    {
      this.InScene = false;
      if (this.Hierarchy != null)
      {
        foreach (MyEntityComponentBase child in this.Hierarchy.Children)
          child.Container.Entity.OnRemovedFromScene(source);
      }
      this.Components.OnRemovedFromScene();
      MyEntitiesInterface.UnregisterUpdate(this, false);
      MyEntitiesInterface.UnregisterDraw(this);
      if (this.GameLogic != null)
        ((IMyGameLogicComponent) this.GameLogic).UnregisterForUpdate();
      if (this.m_physics != null && this.m_physics.Enabled)
        this.m_physics.Deactivate();
      if (this.Parent != null)
        this.Render.FadeOut = this.Parent.Render.FadeOut;
      this.Render.RemoveRenderObjects();
      MyEntity.RemoveFromGamePruningStructureExtCallBack(this);
    }

    public event Action<MyEntity> OnMarkForClose;

    public event Action<MyEntity> OnClose;

    public event Action<MyEntity> OnClosing;

    public event Action<MyEntity> OnPhysicsChanged;

    public event Action<MyPhysicsComponentBase, MyPhysicsComponentBase> OnPhysicsComponentChanged;

    public event Action<MyEntity> AddedToScene;

    public event Action<MyEntity> OnTeleport;

    public void AddToGamePruningStructure()
    {
      if (!this.UsePrunning())
        return;
      MyEntity.AddToGamePruningStructureExtCallBack(this);
    }

    public void RemoveFromGamePruningStructure()
    {
      if (!this.UsePrunning())
        return;
      MyEntity.RemoveFromGamePruningStructureExtCallBack(this);
    }

    private bool UsePrunning()
    {
      EntityFlags entityFlags = this.Parent == null ? EntityFlags.IsNotGamePrunningStructureObject : EntityFlags.IsGamePrunningStructureObject;
      return this.InScene && (this.Flags & entityFlags) == (EntityFlags) 0;
    }

    public void UpdateGamePruningStructure()
    {
      if (!this.UsePrunning())
        return;
      MyEntity.UpdateGamePruningStructureExtCallBack(this);
    }

    public void RaisePhysicsChanged()
    {
      if (this.m_raisePhysicsCalled)
        return;
      this.m_raisePhysicsCalled = true;
      if (!this.InScene)
      {
        Action<MyEntity> onPhysicsChanged = this.OnPhysicsChanged;
        if (onPhysicsChanged != null)
          onPhysicsChanged(this);
      }
      else
      {
        MyEntity.MyWeldingGroupsGetGroupNodesExtCallback(this, this.m_tmpOnPhysicsChanged);
        foreach (MyEntity myEntity in this.m_tmpOnPhysicsChanged)
        {
          Action<MyEntity> onPhysicsChanged = myEntity.OnPhysicsChanged;
          if (onPhysicsChanged != null)
            onPhysicsChanged(myEntity);
        }
        this.m_tmpOnPhysicsChanged.Clear();
      }
      this.m_raisePhysicsCalled = false;
    }

    public virtual void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      this.MarkedForClose = false;
      this.Closed = false;
      this.Render.PersistentFlags = MyPersistentEntityFlags2.CastShadows;
      if (objectBuilder != null)
      {
        if (objectBuilder.EntityId != 0L)
          this.EntityId = objectBuilder.EntityId;
        else
          this.AllocateEntityID();
        this.Name = !string.IsNullOrEmpty(objectBuilder.Name) ? objectBuilder.Name : this.EntityId.ToString();
        this.DefinitionId = new MyDefinitionId?(objectBuilder.GetId());
        if (objectBuilder.EntityDefinitionId.HasValue)
          this.DefinitionId = new MyDefinitionId?((MyDefinitionId) objectBuilder.EntityDefinitionId.Value);
        if (objectBuilder.PositionAndOrientation.HasValue)
        {
          MyPositionAndOrientation positionAndOrientation = objectBuilder.PositionAndOrientation.Value;
          if (!positionAndOrientation.Position.x.IsValid())
            positionAndOrientation.Position.x = 0.0;
          if (!positionAndOrientation.Position.y.IsValid())
            positionAndOrientation.Position.y = 0.0;
          if (!positionAndOrientation.Position.z.IsValid())
            positionAndOrientation.Position.z = 0.0;
          MatrixD worldMatrix = MatrixD.CreateWorld((Vector3D) positionAndOrientation.Position, (Vector3) positionAndOrientation.Forward, (Vector3) positionAndOrientation.Up);
          if (!worldMatrix.IsValid())
            worldMatrix = MatrixD.Identity;
          this.PositionComp.SetWorldMatrix(ref worldMatrix, ignoreAssert: true);
          this.ClampToWorld();
        }
        this.Render.PersistentFlags = objectBuilder.PersistentFlags & ~MyPersistentEntityFlags2.InScene;
        MyEntity.InitComponentsExtCallback((MyComponentContainer) this.Components, this.DefinitionId.Value.TypeId, this.DefinitionId.Value.SubtypeId, objectBuilder.ComponentContainer);
      }
      else
        this.AllocateEntityID();
      if (this.SyncFlag)
        this.CreateSync();
      this.GameLogic.Init(objectBuilder);
      MyEntitiesInterface.SetEntityName(this, false);
    }

    protected virtual void ClampToWorld()
    {
      Vector3D position = this.PositionComp.GetPosition();
      float num = 10f;
      BoundingBoxD boundingBoxD = MyAPIGatewayShortcuts.GetWorldBoundaries != null ? MyAPIGatewayShortcuts.GetWorldBoundaries() : new BoundingBoxD();
      if (boundingBoxD.Max.X <= boundingBoxD.Min.X || boundingBoxD.Max.Y <= boundingBoxD.Min.Y || boundingBoxD.Max.Z <= boundingBoxD.Min.Z)
        return;
      if (position.X > boundingBoxD.Max.X)
        position.X = boundingBoxD.Max.X - (double) num;
      else if (position.X < boundingBoxD.Min.X)
        position.X = boundingBoxD.Min.X + (double) num;
      if (position.Y > boundingBoxD.Max.Y)
        position.Y = boundingBoxD.Max.Y - (double) num;
      else if (position.Y < boundingBoxD.Min.Y)
        position.Y = boundingBoxD.Min.Y + (double) num;
      if (position.Z > boundingBoxD.Max.Z)
        position.Z = boundingBoxD.Max.Z - (double) num;
      else if (position.Z < boundingBoxD.Min.Z)
        position.Z = boundingBoxD.Min.Z + (double) num;
      this.PositionComp.SetPosition(position);
    }

    private void AllocateEntityID()
    {
      if (this.EntityId != 0L || MyEntityIdentifier.AllocationSuspended)
        return;
      this.EntityId = MyEntityIdentifier.AllocateId();
    }

    public virtual void Init(
      StringBuilder displayName,
      string model,
      MyEntity parentObject,
      float? scale,
      string modelCollision = null)
    {
      this.MarkedForClose = false;
      this.Closed = false;
      this.Render.PersistentFlags = MyPersistentEntityFlags2.CastShadows;
      this.DisplayName = displayName?.ToString();
      this.RefreshModels(model, modelCollision);
      parentObject?.Hierarchy.AddChild((VRage.ModAPI.IMyEntity) this, insertIntoSceneIfNeeded: false);
      if (!this.PositionComp.Scale.HasValue)
        this.PositionComp.Scale = scale;
      this.AllocateEntityID();
    }

    public virtual void RefreshModels(string model, string modelCollision)
    {
      float valueOrDefault = this.PositionComp.Scale.GetValueOrDefault(1f);
      if (model != null)
      {
        this.Render.ModelStorage = (object) MyModels.GetModelOnlyData(model);
        MyModel model1 = this.Render.GetModel();
        this.PositionComp.LocalVolumeOffset = model1 == null ? Vector3.Zero : model1.BoundingSphere.Center * valueOrDefault;
      }
      if (modelCollision != null)
        this.m_modelCollision = MyModels.GetModelOnlyData(modelCollision);
      if (this.Render.ModelStorage != null)
      {
        BoundingBox boundingBox = this.Render.GetModel().BoundingBox;
        boundingBox.Min *= valueOrDefault;
        boundingBox.Max *= valueOrDefault;
        this.PositionComp.LocalAABB = boundingBox;
        bool allocationSuspended = MyEntityIdentifier.AllocationSuspended;
        try
        {
          MyEntityIdentifier.AllocationSuspended = false;
          if (this.Subparts == null)
          {
            this.Subparts = new Dictionary<string, MyEntitySubpart>();
          }
          else
          {
            foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
            {
              this.Hierarchy.RemoveChild((VRage.ModAPI.IMyEntity) subpart.Value);
              subpart.Value.Close();
            }
            this.Subparts.Clear();
          }
          MyEntitySubpart.Data data = new MyEntitySubpart.Data();
          foreach (KeyValuePair<string, MyModelDummy> dummy in this.Render.GetModel().Dummies)
          {
            if (MyEntitySubpart.GetSubpartFromDummy(model, dummy.Key, dummy.Value, ref data))
            {
              MyEntitySubpart myEntitySubpart = this.InstantiateSubpart(dummy.Value, ref data);
              myEntitySubpart.Render.EnableColorMaskHsv = this.Render.EnableColorMaskHsv;
              myEntitySubpart.Render.ColorMaskHsv = this.Render.ColorMaskHsv;
              myEntitySubpart.Render.TextureChanges = this.Render.TextureChanges;
              myEntitySubpart.Render.MetalnessColorable = this.Render.MetalnessColorable;
              MyModel modelOnlyData = MyModels.GetModelOnlyData(data.File);
              if (modelOnlyData != null && this.Model != null)
                modelOnlyData.Rescale(this.Model.ScaleFactor);
              myEntitySubpart.Init((StringBuilder) null, data.File, this, this.PositionComp.Scale);
              myEntitySubpart.Render.NeedsDrawFromParent = false;
              myEntitySubpart.Render.PersistentFlags = this.Render.PersistentFlags & ~MyPersistentEntityFlags2.InScene;
              myEntitySubpart.PositionComp.SetLocalMatrix(ref data.InitialTransform);
              this.Subparts[data.Name] = myEntitySubpart;
              if (this.InScene)
                myEntitySubpart.OnAddedToScene((object) this);
              myEntitySubpart.Flags &= ~EntityFlags.IsGamePrunningStructureObject;
            }
          }
        }
        finally
        {
          MyEntityIdentifier.AllocationSuspended = allocationSuspended;
        }
      }
      else
      {
        float num = 0.5f;
        this.PositionComp.LocalAABB = new BoundingBox(new Vector3(-num), new Vector3(num));
      }
    }

    public void Delete()
    {
      if (this.Closed)
        return;
      this.Render.RemoveRenderObjects();
      this.Close();
      this.BeforeDelete();
      if (this.GameLogic != null)
        ((IMyGameLogicComponent) this.GameLogic).Close();
      MyHierarchyComponent<MyEntity> hierarchy = this.Hierarchy;
      hierarchy?.Delete();
      this.CallAndClearOnClosing();
      MyEntitiesInterface.RemoveName(this);
      MyEntitiesInterface.RemoveFromClosedEntities(this);
      if (this.m_physics != null)
      {
        this.m_physics.Close();
        this.Physics = (MyPhysicsComponentBase) null;
        this.RaisePhysicsChanged();
      }
      MyEntitiesInterface.UnregisterUpdate(this, true);
      MyEntitiesInterface.UnregisterDraw(this);
      MyEntity parent = this.Parent;
      if (parent == null)
      {
        int num = MyEntitiesInterface.Remove(this) ? 1 : 0;
      }
      else
      {
        parent.Hierarchy.RemoveByJN((MyHierarchyComponentBase) hierarchy);
        if (parent.InScene)
        {
          this.OnRemovedFromScene((object) this);
          MyEntitiesInterface.RaiseEntityRemove(this);
        }
      }
      if (this.EntityId != 0L && MyEntityIdentifier.GetEntityById(this.EntityId, true) == this)
        MyEntityIdentifier.RemoveEntity(this.EntityId);
      this.CallAndClearOnClose();
      this.ClearDebugRenderComponents();
      this.Components.Clear();
      this.Closed = true;
    }

    protected virtual void BeforeDelete()
    {
    }

    protected virtual void Closing()
    {
    }

    public void Close()
    {
      if (this.MarkedForClose)
        return;
      this.MarkedForClose = true;
      this.Closing();
      MyEntitiesInterface.Close(this);
      this.GameLogic.MarkForClose();
      this.OnMarkForClose.InvokeIfNotNull<MyEntity>(this);
    }

    private void CallAndClearOnClose()
    {
      this.OnClose.InvokeIfNotNull<MyEntity>(this);
      this.OnClose = (Action<MyEntity>) null;
    }

    private void CallAndClearOnClosing()
    {
      this.OnClosing.InvokeIfNotNull<MyEntity>(this);
      this.OnClosing = (Action<MyEntity>) null;
    }

    public virtual MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EntityBase builderEntityBase = MyEntity.MyEntityFactoryCreateObjectBuilderExtCallback(this);
      if (builderEntityBase != null)
      {
        builderEntityBase.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation()
        {
          Position = (SerializableVector3D) this.PositionComp.GetPosition(),
          Up = (SerializableVector3) (Vector3) this.WorldMatrix.Up,
          Forward = (SerializableVector3) (Vector3) this.WorldMatrix.Forward
        });
        builderEntityBase.EntityId = this.EntityId;
        builderEntityBase.Name = this.Name;
        builderEntityBase.PersistentFlags = this.Render.PersistentFlags;
        builderEntityBase.ComponentContainer = this.Components.Serialize(copy);
        if (this.DefinitionId.HasValue)
          builderEntityBase.SubtypeName = this.DefinitionId.Value.SubtypeName;
      }
      return builderEntityBase;
    }

    public virtual void BeforeSave()
    {
    }

    public virtual void PrepareForDraw()
    {
      foreach (MyDebugRenderComponentBase debugRenderer in this.m_debugRenderers)
        debugRenderer.PrepareForDraw();
    }

    public virtual void BeforePaste()
    {
    }

    public virtual void AfterPaste()
    {
    }

    public void SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity) => MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], emissiveName, emissivePartColor, emissivity);

    public void SetEmissivePartsForSubparts(
      string emissiveName,
      Color emissivePartColor,
      float emissivity)
    {
      if (this.Subparts == null)
        return;
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
        subpart.Value.SetEmissiveParts(emissiveName, emissivePartColor, emissivity);
    }

    protected static void UpdateNamedEmissiveParts(
      uint renderObjectId,
      string emissiveName,
      Color emissivePartColor,
      float emissivity)
    {
      if (renderObjectId == uint.MaxValue)
        return;
      MyRenderProxy.UpdateColorEmissivity(renderObjectId, 0, emissiveName, emissivePartColor, emissivity);
    }

    protected virtual MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      return new MyEntitySubpart();
    }

    public override string ToString() => this.GetType().Name + " {" + this.EntityId.ToString("X8") + "}";

    public virtual MyInventoryBase GetInventoryBase(int index)
    {
      MyInventoryBase component = (MyInventoryBase) null;
      return !this.Components.TryGet<MyInventoryBase>(out component) ? (MyInventoryBase) null : component.IterateInventory(index);
    }

    public MyInventoryBase GetInventoryBase()
    {
      MyInventoryBase component = (MyInventoryBase) null;
      this.Components.TryGet<MyInventoryBase>(out component);
      return component;
    }

    public int InventoryCount
    {
      get
      {
        MyInventoryBase component = (MyInventoryBase) null;
        return this.Components.TryGet<MyInventoryBase>(out component) ? component.GetInventoryCount() : 0;
      }
    }

    public bool HasInventory => this.InventoryCount > 0;

    public virtual string DisplayNameText { get; set; }

    public MySnapshotFlags LastSnapshotFlags { get; set; }

    protected virtual void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
    }

    protected virtual void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
    }

    public virtual void SerializeControls(BitStream stream) => stream.WriteBool(false);

    public virtual void DeserializeControls(BitStream stream, bool outOfOrder) => stream.ReadBool();

    public virtual void ApplyLastControls()
    {
    }

    public virtual void ResetControls()
    {
    }

    public MyEntity.EntityPin Pin() => new MyEntity.EntityPin(this);

    public void Unpin() => Interlocked.Decrement(ref this.m_pins);

    public void UpdateSoundContactPoint(
      long entityId,
      Vector3 localPosition,
      Vector3 normal,
      Vector3 separatingVelocity,
      float separatingSpeed)
    {
      this.m_contactPoint.SetLocalValue(new MyEntity.ContactPointData()
      {
        EntityId = entityId,
        LocalPosition = localPosition,
        Normal = normal,
        ContactPointType = MyEntity.ContactPointData.ContactPointDataTypes.Sounds,
        SeparatingVelocity = separatingVelocity,
        SeparatingSpeed = separatingSpeed
      });
    }

    public EntityFlags Flags { get; set; }

    VRage.ModAPI.IMyEntity VRage.ModAPI.IMyEntity.Parent => (VRage.ModAPI.IMyEntity) this.Parent;

    VRage.ModAPI.IMyEntity VRage.ModAPI.IMyEntity.GetTopMostParent(Type type) => (VRage.ModAPI.IMyEntity) this.GetTopMostParent(type);

    void VRage.ModAPI.IMyEntity.GetChildren(
      List<VRage.ModAPI.IMyEntity> children,
      Func<VRage.ModAPI.IMyEntity, bool> collect)
    {
      foreach (VRage.ModAPI.IMyEntity child in children)
      {
        if (collect == null || collect(child))
          children.Add(child);
      }
    }

    string VRage.ModAPI.IMyEntity.Name
    {
      get => this.Name;
      set => this.Name = value;
    }

    bool VRage.ModAPI.IMyEntity.DebugAsyncLoading => this.DebugAsyncLoading;

    private Action<MyEntity> GetDelegate(Action<VRage.ModAPI.IMyEntity> value) => (Action<MyEntity>) Delegate.CreateDelegate(typeof (Action<MyEntity>), value.Target, value.Method);

    event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnClose
    {
      add => this.OnClose += this.GetDelegate(value);
      remove => this.OnClose -= this.GetDelegate(value);
    }

    event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnClosing
    {
      add => this.OnClosing += this.GetDelegate(value);
      remove => this.OnClosing -= this.GetDelegate(value);
    }

    event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnMarkForClose
    {
      add => this.OnMarkForClose += this.GetDelegate(value);
      remove => this.OnMarkForClose -= this.GetDelegate(value);
    }

    event Action<VRage.ModAPI.IMyEntity> VRage.ModAPI.IMyEntity.OnPhysicsChanged
    {
      add => this.OnPhysicsChanged += this.GetDelegate(value);
      remove => this.OnPhysicsChanged -= this.GetDelegate(value);
    }

    string VRage.ModAPI.IMyEntity.DisplayName
    {
      get => this.DisplayName;
      set => this.DisplayName = value;
    }

    string VRage.ModAPI.IMyEntity.GetFriendlyName() => this.GetFriendlyName();

    void VRage.ModAPI.IMyEntity.Close() => this.Close();

    bool VRage.ModAPI.IMyEntity.MarkedForClose => this.MarkedForClose;

    void VRage.ModAPI.IMyEntity.Delete() => this.Delete();

    bool VRage.ModAPI.IMyEntity.Closed => this.Closed;

    IMyModel VRage.ModAPI.IMyEntity.Model => (IMyModel) this.Model;

    MyEntityComponentBase VRage.ModAPI.IMyEntity.GameLogic
    {
      get => (MyEntityComponentBase) this.GameLogic;
      set => this.GameLogic = (MyGameLogicComponent) value;
    }

    MyEntityUpdateEnum VRage.ModAPI.IMyEntity.NeedsUpdate
    {
      get => this.NeedsUpdate;
      set => this.NeedsUpdate = value;
    }

    bool VRage.ModAPI.IMyEntity.NearFlag
    {
      get => this.Render.NearFlag;
      set => this.Render.NearFlag = value;
    }

    bool VRage.ModAPI.IMyEntity.CastShadows
    {
      get => this.Render.CastShadows;
      set => this.Render.CastShadows = value;
    }

    bool VRage.ModAPI.IMyEntity.FastCastShadowResolve
    {
      get => this.Render.FastCastShadowResolve;
      set => this.Render.FastCastShadowResolve = value;
    }

    bool VRage.ModAPI.IMyEntity.NeedsResolveCastShadow
    {
      get => this.Render.NeedsResolveCastShadow;
      set => this.Render.NeedsResolveCastShadow = value;
    }

    Vector3 VRage.ModAPI.IMyEntity.GetDiffuseColor() => (Vector3) this.Render.GetDiffuseColor();

    float VRage.ModAPI.IMyEntity.MaxGlassDistSq => this.MaxGlassDistSq;

    bool VRage.ModAPI.IMyEntity.NeedsDraw
    {
      get => this.Render.NeedsDraw;
      set => this.Render.NeedsDraw = value;
    }

    bool VRage.ModAPI.IMyEntity.NeedsDrawFromParent
    {
      get => this.Render.NeedsDrawFromParent;
      set => this.Render.NeedsDrawFromParent = value;
    }

    bool VRage.ModAPI.IMyEntity.Transparent
    {
      get => (double) this.Render.Transparency != 0.0;
      set => this.Render.Transparency = value ? 0.25f : 0.0f;
    }

    bool VRage.ModAPI.IMyEntity.ShadowBoxLod
    {
      get => this.Render.ShadowBoxLod;
      set => this.Render.ShadowBoxLod = value;
    }

    bool VRage.ModAPI.IMyEntity.SkipIfTooSmall
    {
      get => this.Render.SkipIfTooSmall;
      set => this.Render.SkipIfTooSmall = value;
    }

    MyModStorageComponentBase VRage.ModAPI.IMyEntity.Storage
    {
      get => this.Storage;
      set => this.Storage = value;
    }

    bool VRage.ModAPI.IMyEntity.Visible
    {
      get => this.Render.Visible;
      set => this.Render.Visible = value;
    }

    float VRage.ModAPI.IMyEntity.GetDistanceBetweenCameraAndBoundingSphere() => (float) this.GetDistanceBetweenCameraAndBoundingSphere();

    float VRage.ModAPI.IMyEntity.GetDistanceBetweenCameraAndPosition() => (float) this.GetDistanceBetweenCameraAndPosition();

    float VRage.ModAPI.IMyEntity.GetLargestDistanceBetweenCameraAndBoundingSphere() => (float) this.GetLargestDistanceBetweenCameraAndBoundingSphere();

    float VRage.ModAPI.IMyEntity.GetSmallestDistanceBetweenCameraAndBoundingSphere() => (float) this.GetSmallestDistanceBetweenCameraAndBoundingSphere();

    Vector3D? VRage.ModAPI.IMyEntity.GetIntersectionWithLineAndBoundingSphere(
      ref LineD line,
      float boundingSphereRadiusMultiplier)
    {
      return this.GetIntersectionWithLineAndBoundingSphere(ref line, boundingSphereRadiusMultiplier);
    }

    bool VRage.ModAPI.IMyEntity.GetIntersectionWithSphere(ref BoundingSphereD sphere) => this.GetIntersectionWithSphere(ref sphere);

    VRage.Game.ModAPI.IMyInventory VRage.ModAPI.IMyEntity.GetInventory() => this.GetInventoryBase(0) as VRage.Game.ModAPI.IMyInventory;

    VRage.Game.ModAPI.IMyInventory VRage.ModAPI.IMyEntity.GetInventory(
      int index)
    {
      return this.GetInventoryBase(index) as VRage.Game.ModAPI.IMyInventory;
    }

    void VRage.ModAPI.IMyEntity.GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles)
    {
      this.GetTrianglesIntersectingSphere(ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
    }

    bool VRage.ModAPI.IMyEntity.DoOverlapSphereTest(float sphereRadius, Vector3D spherePos) => this.DoOverlapSphereTest(sphereRadius, spherePos);

    MyObjectBuilder_EntityBase VRage.ModAPI.IMyEntity.GetObjectBuilder(
      bool copy)
    {
      return this.GetObjectBuilder(copy);
    }

    bool VRage.ModAPI.IMyEntity.Save
    {
      get => this.Save;
      set => this.Save = value;
    }

    MyPersistentEntityFlags2 VRage.ModAPI.IMyEntity.PersistentFlags
    {
      get => this.Render.PersistentFlags;
      set => this.Render.PersistentFlags = value;
    }

    bool VRage.ModAPI.IMyEntity.InScene
    {
      get => this.InScene;
      set => this.InScene = value;
    }

    bool VRage.ModAPI.IMyEntity.InvalidateOnMove => this.InvalidateOnMove;

    bool VRage.ModAPI.IMyEntity.IsCCDForProjectiles => this.IsCCDForProjectiles;

    bool VRage.ModAPI.IMyEntity.IsVisible() => this.Render.IsVisible();

    bool VRage.ModAPI.IMyEntity.IsVolumetric => this.IsVolumetric;

    MatrixD VRage.ModAPI.IMyEntity.GetViewMatrix() => this.GetViewMatrix();

    MatrixD VRage.ModAPI.IMyEntity.GetWorldMatrixNormalizedInv() => this.PositionComp.WorldMatrixNormalizedInv;

    BoundingBox VRage.ModAPI.IMyEntity.LocalAABB
    {
      get => this.PositionComp.LocalAABB;
      set => this.PositionComp.LocalAABB = value;
    }

    BoundingBox VRage.ModAPI.IMyEntity.LocalAABBHr => this.PositionComp.LocalAABB;

    Matrix VRage.ModAPI.IMyEntity.LocalMatrix
    {
      get => this.PositionComp.LocalMatrixRef;
      set => this.PositionComp.SetLocalMatrix(ref value);
    }

    BoundingSphere VRage.ModAPI.IMyEntity.LocalVolume
    {
      get => this.PositionComp.LocalVolume;
      set => this.PositionComp.LocalVolume = value;
    }

    Vector3 VRage.ModAPI.IMyEntity.LocalVolumeOffset
    {
      get => this.PositionComp.LocalVolumeOffset;
      set => this.PositionComp.LocalVolumeOffset = value;
    }

    Vector3D VRage.ModAPI.IMyEntity.LocationForHudMarker => this.LocationForHudMarker;

    bool VRage.ModAPI.IMyEntity.Synchronized
    {
      get => !this.IsPreview;
      set => this.IsPreview = !value;
    }

    void VRage.ModAPI.IMyEntity.SetLocalMatrix(Matrix localMatrix, object source) => this.PositionComp.SetLocalMatrix(ref localMatrix, source);

    void VRage.ModAPI.IMyEntity.SetWorldMatrix(MatrixD worldMatrix, object source) => this.PositionComp.SetWorldMatrix(ref worldMatrix, source);

    MatrixD VRage.ModAPI.IMyEntity.WorldMatrix
    {
      get => this.PositionComp.WorldMatrixRef;
      set => this.PositionComp.SetWorldMatrix(ref value);
    }

    MatrixD VRage.ModAPI.IMyEntity.WorldMatrixInvScaled => this.PositionComp.WorldMatrixInvScaled;

    MatrixD VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv => this.PositionComp.WorldMatrixNormalizedInv;

    void VRage.ModAPI.IMyEntity.SetPosition(Vector3D pos) => this.PositionComp.SetPosition(pos);

    void VRage.ModAPI.IMyEntity.EnableColorMaskForSubparts(bool value)
    {
      if (this.Subparts == null)
        return;
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
        subpart.Value.Render.EnableColorMaskHsv = value;
    }

    void VRage.ModAPI.IMyEntity.SetColorMaskForSubparts(Vector3 colorMaskHsv)
    {
      if (this.Subparts == null)
        return;
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
        subpart.Value.Render.ColorMaskHsv = colorMaskHsv;
    }

    void VRage.ModAPI.IMyEntity.SetTextureChangesForSubparts(
      Dictionary<string, MyTextureChange> textureChanges)
    {
      if (this.Subparts == null)
        return;
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
        subpart.Value.Render.TextureChanges = textureChanges;
    }

    void VRage.ModAPI.IMyEntity.SetEmissiveParts(
      string emissiveName,
      Color emissivePartColor,
      float emissivity)
    {
      this.SetEmissiveParts(emissiveName, emissivePartColor, emissivity);
    }

    void VRage.ModAPI.IMyEntity.SetEmissivePartsForSubparts(
      string emissiveName,
      Color emissivePartColor,
      float emissivity)
    {
      this.SetEmissivePartsForSubparts(emissiveName, emissivePartColor, emissivity);
    }

    bool VRage.Game.ModAPI.Ingame.IMyEntity.HasInventory => this.HasInventory;

    int VRage.Game.ModAPI.Ingame.IMyEntity.InventoryCount => this.InventoryCount;

    VRage.Game.ModAPI.Ingame.IMyInventory VRage.Game.ModAPI.Ingame.IMyEntity.GetInventory() => this.GetInventoryBase(0) as VRage.Game.ModAPI.Ingame.IMyInventory;

    VRage.Game.ModAPI.Ingame.IMyInventory VRage.Game.ModAPI.Ingame.IMyEntity.GetInventory(
      int index)
    {
      return this.GetInventoryBase(index) as VRage.Game.ModAPI.Ingame.IMyInventory;
    }

    string VRage.Game.ModAPI.Ingame.IMyEntity.DisplayName => this.DisplayName;

    string VRage.Game.ModAPI.Ingame.IMyEntity.Name => this.Name;

    BoundingBoxD VRage.Game.ModAPI.Ingame.IMyEntity.WorldAABB => this.PositionComp.WorldAABB;

    BoundingBoxD VRage.Game.ModAPI.Ingame.IMyEntity.WorldAABBHr => this.PositionComp.WorldAABB;

    MatrixD VRage.Game.ModAPI.Ingame.IMyEntity.WorldMatrix => this.PositionComp.WorldMatrixRef;

    BoundingSphereD VRage.Game.ModAPI.Ingame.IMyEntity.WorldVolume => this.PositionComp.WorldVolume;

    BoundingSphereD VRage.Game.ModAPI.Ingame.IMyEntity.WorldVolumeHr => this.PositionComp.WorldVolume;

    Vector3D VRage.Game.ModAPI.Ingame.IMyEntity.GetPosition() => this.PositionComp.GetPosition();

    public struct EntityPin : IDisposable
    {
      private MyEntity m_entity;

      public EntityPin(MyEntity entity)
      {
        this.m_entity = entity;
        Interlocked.Increment(ref entity.m_pins);
      }

      public void Dispose() => this.m_entity.Unpin();
    }

    [Serializable]
    public struct ContactPointData
    {
      public long EntityId;
      public Vector3 LocalPosition;
      public Vector3 Normal;
      public MyEntity.ContactPointData.ContactPointDataTypes ContactPointType;
      public Vector3 SeparatingVelocity;
      public float SeparatingSpeed;
      public float Impulse;

      [System.Flags]
      public enum ContactPointDataTypes
      {
        None = 0,
        Sounds = 1,
        Particle_PlanetCrash = 2,
        Particle_Collision = 4,
        Particle_GridCollision = 8,
        Particle_Dust = 16, // 0x00000010
        AnySound = Sounds, // 0x00000001
        AnyParticle = Particle_Dust | Particle_GridCollision | Particle_Collision | Particle_PlanetCrash, // 0x0000001E
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out long value) => value = owner.EntityId;
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003ELocalPosition\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in Vector3 value) => owner.LocalPosition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out Vector3 value) => value = owner.LocalPosition;
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in Vector3 value) => owner.Normal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out Vector3 value) => value = owner.Normal;
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003EContactPointType\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, MyEntity.ContactPointData.ContactPointDataTypes>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyEntity.ContactPointData owner,
          in MyEntity.ContactPointData.ContactPointDataTypes value)
        {
          owner.ContactPointType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyEntity.ContactPointData owner,
          out MyEntity.ContactPointData.ContactPointDataTypes value)
        {
          value = owner.ContactPointType;
        }
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003ESeparatingVelocity\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in Vector3 value) => owner.SeparatingVelocity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out Vector3 value) => value = owner.SeparatingVelocity;
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003ESeparatingSpeed\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in float value) => owner.SeparatingSpeed = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out float value) => value = owner.SeparatingSpeed;
      }

      protected class VRage_Game_Entity_MyEntity\u003C\u003EContactPointData\u003C\u003EImpulse\u003C\u003EAccessor : IMemberAccessor<MyEntity.ContactPointData, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntity.ContactPointData owner, in float value) => owner.Impulse = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntity.ContactPointData owner, out float value) => value = owner.Impulse;
      }
    }

    public delegate MyObjectBuilder_EntityBase MyEntityFactoryCreateObjectBuilderDelegate(
      MyEntity entity);

    public delegate MySyncComponentBase CreateDefaultSyncEntityDelegate(
      MyEntity thisEntity);

    public delegate bool MyWeldingGroupsGroupExistsDelegate(MyEntity entity);

    protected class m_contactPoint\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyEntity.ContactPointData, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyEntity.ContactPointData, SyncDirection.FromServer>(obj1, obj2));
        ((MyEntity) obj0).m_contactPoint = (VRage.Sync.Sync<MyEntity.ContactPointData, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class VRage_Game_Entity_MyEntity\u003C\u003EActor : IActivator, IActivator<MyEntity>
    {
      object IActivator.CreateInstance() => (object) new MyEntity();

      MyEntity IActivator<MyEntity>.CreateInstance() => new MyEntity();
    }
  }
}
