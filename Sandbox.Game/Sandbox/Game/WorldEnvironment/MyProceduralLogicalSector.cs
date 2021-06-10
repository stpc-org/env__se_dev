// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyProceduralLogicalSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.Modules;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyProceduralLogicalSector : MyLogicalEnvironmentSectorBase
  {
    private readonly Dictionary<Type, MyObjectBuilder_EnvironmentModuleBase> m_moduleData = new Dictionary<Type, MyObjectBuilder_EnvironmentModuleBase>();
    private bool m_scanning;
    private bool m_serverOwned;
    private int m_itemOffset;
    private int m_locationOffset;
    private readonly int m_x;
    private readonly int m_y;
    private readonly int m_lod;
    private readonly int[] m_itemCountForLod = new int[16];
    private readonly MyProceduralEnvironmentProvider m_provider;
    private readonly HashSet<MyProceduralDataView> m_viewers = new HashSet<MyProceduralDataView>();
    private readonly Vector3 m_basisX;
    private readonly Vector3 m_basisY;
    internal bool Replicable;
    private readonly FastResourceLock m_lock = new FastResourceLock();
    private readonly MyList<ItemInfo> m_items;
    private readonly int m_itemCountTotal;
    private readonly MyProceduralEnvironmentDefinition m_environment;
    private int m_minimumScannedLod = 16;
    private int m_totalSpawned;
    private readonly Dictionary<Type, MyProceduralLogicalSector.ModuleData> m_modules = new Dictionary<Type, MyProceduralLogicalSector.ModuleData>();
    private readonly int m_seed;
    private readonly MyRandom m_itemPositionRng;
    private readonly List<MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>> m_candidates = new List<MyDiscreteSampler<MyRuntimeEnvironmentItemInfo>>();
    private readonly MyProceduralLogicalSector.ProgressiveScanHelper m_scanHelper;

    public MyProceduralLogicalSector(
      MyProceduralEnvironmentProvider provider,
      int x,
      int y,
      int localLod,
      MyObjectBuilder_ProceduralEnvironmentSector moduleData)
    {
      this.m_provider = provider;
      this.Owner = provider.Owner;
      this.m_x = x;
      this.m_y = y;
      this.m_lod = localLod;
      provider.GeSectorWorldParameters(x, y, localLod * provider.LodFactor, out this.WorldPos, out this.m_basisX, out this.m_basisY);
      this.m_environment = (MyProceduralEnvironmentDefinition) provider.Owner.EnvironmentDefinition;
      this.m_seed = provider.GetSeed() ^ (x * 377 + y) * 377 + this.m_lod;
      this.m_itemPositionRng = new MyRandom(this.m_seed);
      this.m_itemCountTotal = (int) ((double) (Vector3.Cross(this.m_basisX, this.m_basisY).Length() * 4f) * this.m_environment.ItemDensity);
      this.m_scanHelper = new MyProceduralLogicalSector.ProgressiveScanHelper(this.m_itemCountTotal, localLod * provider.LodFactor);
      this.Bounds = this.Owner.GetBoundingShape(ref this.WorldPos, ref this.m_basisX, ref this.m_basisY);
      this.m_items = new MyList<ItemInfo>();
      this.m_totalSpawned = 0;
      this.UpdateModuleBuilders(moduleData);
    }

    private void UpdateModuleBuilders(
      MyObjectBuilder_ProceduralEnvironmentSector moduleData)
    {
      this.m_moduleData.Clear();
      if (moduleData == null)
        return;
      for (int index = 0; index < moduleData.SavedModules.Length; ++index)
      {
        MyObjectBuilder_ProceduralEnvironmentSector.Module savedModule = moduleData.SavedModules[index];
        MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>((MyDefinitionId) savedModule.ModuleId);
        if (definition != null)
        {
          this.m_moduleData.Add(definition.ModuleType, savedModule.Builder);
          MyProceduralLogicalSector.ModuleData moduleData1;
          if (this.m_modules.TryGetValue(definition.ModuleType, out moduleData1))
            moduleData1.Module.Init((MyLogicalEnvironmentSectorBase) this, (MyObjectBuilder_Base) savedModule.Builder);
        }
      }
    }

    public override void EnableItem(int itemId, bool enabled)
    {
      if (itemId < 0 && itemId >= this.m_items.Count)
        return;
      short definitionIndex = this.m_items[itemId].DefinitionIndex;
      if (!this.m_items[itemId].IsEnabled || definitionIndex == (short) -1)
        return;
      MyRuntimeEnvironmentItemInfo def;
      this.GetItemDefinition((ushort) definitionIndex, out def);
      this.GetModuleForDefinition(def)?.OnItemEnable(itemId, enabled);
    }

    public override int GetItemDefinitionId(int itemId) => (int) this.m_items[itemId].DefinitionIndex;

    public override void UpdateItemModel(int itemId, short modelId)
    {
      if (itemId >= this.m_items.Count)
        return;
      if (!this.m_scanning)
      {
        foreach (MyProceduralDataView viewer in this.m_viewers)
        {
          if (viewer.Listener != null)
          {
            int sectorIndex = viewer.GetSectorIndex(this.m_x, this.m_y);
            int sectorOffset = viewer.SectorOffsets[sectorIndex];
            if (itemId < this.m_itemCountForLod[viewer.Lod])
              viewer.Listener.OnItemChange(itemId + sectorOffset, modelId);
          }
        }
      }
      ItemInfo itemInfo = this.m_items[itemId];
      itemInfo.ModelIndex = modelId;
      this.m_items[itemId] = itemInfo;
    }

    public override void UpdateItemModelBatch(List<int> itemIds, short newModelId)
    {
      int count = itemIds.Count;
      if (!this.m_scanning)
      {
        foreach (MyProceduralDataView viewer in this.m_viewers)
        {
          if (viewer.Listener != null)
          {
            int sectorIndex = viewer.GetSectorIndex(this.m_x, this.m_y);
            viewer.Listener.OnItemsChange(sectorIndex, itemIds, newModelId);
          }
        }
      }
      for (int index = 0; index < count; ++index)
      {
        ItemInfo itemInfo = this.m_items[itemIds[index]];
        itemInfo.ModelIndex = newModelId;
        this.m_items[itemIds[index]] = itemInfo;
      }
    }

    private IMyEnvironmentModule GetModuleForDefinition(
      MyRuntimeEnvironmentItemInfo def)
    {
      if (def.Type.StorageModule.Type == (Type) null)
        return (IMyEnvironmentModule) null;
      MyProceduralLogicalSector.ModuleData moduleData;
      return this.m_modules.TryGetValue(def.Type.StorageModule.Type, out moduleData) ? moduleData.Module : (IMyEnvironmentModule) null;
    }

    private Vector3 ComputeRandomItemPosition() => this.m_basisX * this.m_itemPositionRng.NextFloat(-1f, 1f) + this.m_basisY * this.m_itemPositionRng.NextFloat(-1f, 1f);

    private static Vector3 GetRandomPerpendicularVector(ref Vector3 axis, int seed)
    {
      Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(axis);
      Vector3 result;
      Vector3.Cross(ref axis, ref perpendicularVector, out result);
      double num = (double) MyHashRandomUtils.UniformFloatFromSeed(seed) * 2.0 * 3.14159297943115;
      return (float) Math.Cos(num) * perpendicularVector + (float) Math.Sin(num) * result;
    }

    private MyProceduralLogicalSector.ModuleData GetModule(
      MyRuntimeEnvironmentItemInfo info)
    {
      Type type = info.Type.StorageModule.Type;
      if (type == (Type) null)
        return (MyProceduralLogicalSector.ModuleData) null;
      MyProceduralLogicalSector.ModuleData moduleData;
      if (!this.m_modules.TryGetValue(type, out moduleData))
      {
        moduleData = new MyProceduralLogicalSector.ModuleData(type, info.Type.StorageModule.Definition);
        if (this.m_moduleData != null && this.m_moduleData.ContainsKey(type))
          moduleData.Module.Init((MyLogicalEnvironmentSectorBase) this, (MyObjectBuilder_Base) this.m_moduleData[type]);
        else
          moduleData.Module.Init((MyLogicalEnvironmentSectorBase) this, (MyObjectBuilder_Base) null);
        this.m_modules[type] = moduleData;
      }
      return moduleData;
    }

    private MyRuntimeEnvironmentItemInfo GetItemForPosition(
      ref MySurfaceParams surface,
      int lod)
    {
      MyBiomeMaterial key = new MyBiomeMaterial(surface.Biome, surface.Material);
      this.m_candidates.Clear();
      List<MyEnvironmentItemMapping> environmentItemMappingList;
      if (this.m_environment.MaterialEnvironmentMappings.TryGetValue(key, out environmentItemMappingList))
      {
        foreach (MyEnvironmentItemMapping environmentItemMapping in environmentItemMappingList)
        {
          MyDiscreteSampler<MyRuntimeEnvironmentItemInfo> myDiscreteSampler = environmentItemMapping.Sampler(lod);
          if (myDiscreteSampler != null && environmentItemMapping.Rule.Check(surface.HeightRatio, surface.Latitude, surface.Longitude, surface.Normal.Z))
            this.m_candidates.Add(myDiscreteSampler);
        }
      }
      int hashCode = surface.Position.GetHashCode();
      float sample = MyHashRandomUtils.UniformFloatFromSeed(hashCode);
      switch (this.m_candidates.Count)
      {
        case 0:
          return (MyRuntimeEnvironmentItemInfo) null;
        case 1:
          return this.m_candidates[0].Sample(sample);
        default:
          return this.m_candidates[(int) ((double) MyHashRandomUtils.UniformFloatFromSeed(~hashCode) * (double) this.m_candidates.Count)].Sample(sample);
      }
    }

    private void ScanItems(int targetLod)
    {
      int length = this.m_minimumScannedLod - targetLod;
      if (length < 1)
        return;
      int changedLodMin = this.m_minimumScannedLod - 1;
      int capacity = 0;
      int[] numArray = new int[length];
      for (int lod = changedLodMin; lod >= targetLod; --lod)
      {
        int itemsForLod = this.m_scanHelper.GetItemsForLod(lod);
        numArray[lod - targetLod] = capacity + this.m_totalSpawned;
        capacity += itemsForLod;
      }
      List<Vector3> queries = new List<Vector3>(capacity);
      for (int index = 0; index < capacity; ++index)
        queries.Add(this.ComputeRandomItemPosition());
      BoundingBoxD fromPoints = BoundingBoxD.CreateFromPoints((IEnumerable<Vector3D>) this.Bounds);
      MyList<MySurfaceParams> results = new MyList<MySurfaceParams>(capacity);
      this.m_provider.Owner.QuerySurfaceParameters(this.WorldPos, ref fromPoints, queries, results);
      this.m_items.Capacity = this.m_items.Count + queries.Count;
      int index1 = 0;
      for (int lod = changedLodMin; lod >= targetLod; --lod)
      {
        int index2 = lod - targetLod;
        int num = lod > targetLod ? numArray[index2 - 1] - numArray[index2] : capacity - numArray[index2];
        for (int index3 = 0; index3 < num; ++index3)
        {
          MySurfaceParams surface = results[index1];
          MyRuntimeEnvironmentItemInfo itemForPosition = this.GetItemForPosition(ref surface, lod);
          if (itemForPosition != null && lod >= itemForPosition.Type.LodTo)
          {
            MyProceduralLogicalSector.ModuleData module = this.GetModule(itemForPosition);
            if (module != null)
            {
              MyLodEnvironmentItemSet environmentItemSet;
              if (!module.ItemsPerDefinition.TryGetValue(itemForPosition.Index, out environmentItemSet))
                module.ItemsPerDefinition[itemForPosition.Index] = environmentItemSet = new MyLodEnvironmentItemSet()
                {
                  Items = new List<int>()
                };
              environmentItemSet.Items.Add(this.m_totalSpawned);
            }
            Vector3 axis = -surface.Gravity;
            this.m_items.Add(new ItemInfo()
            {
              IsEnabled = itemForPosition.Index != (short) -1,
              Position = surface.Position,
              ModelIndex = (short) -1,
              Rotation = Quaternion.CreateFromForwardUp(MyProceduralLogicalSector.GetRandomPerpendicularVector(ref axis, surface.Position.GetHashCode()), axis),
              DefinitionIndex = itemForPosition.Index
            });
            ++this.m_totalSpawned;
          }
          ++index1;
        }
        this.m_itemCountForLod[lod] = this.m_totalSpawned;
        foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
        {
          if (moduleData != null)
          {
            foreach (short key in moduleData.ItemsPerDefinition.Keys.ToArray<short>())
            {
              MyLodEnvironmentItemSet environmentItemSet = moduleData.ItemsPerDefinition[key];
              moduleData.ItemsPerDefinition[key] = environmentItemSet;
            }
          }
        }
      }
      this.m_scanning = true;
      foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
        moduleData?.Module.ProcessItems(moduleData.ItemsPerDefinition, changedLodMin, targetLod);
      this.m_scanning = false;
      this.m_minimumScannedLod = targetLod;
    }

    public override void Init(MyObjectBuilder_EnvironmentSector sectorBuilder) => this.UpdateModuleBuilders((MyObjectBuilder_ProceduralEnvironmentSector) sectorBuilder);

    public void ReenableItem(int itemId)
    {
      foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
      {
        if (moduleData.Module is MyMemoryEnvironmentModule module)
          module.OnItemEnable(itemId, true);
      }
      this.RevalidateItem(itemId);
    }

    public override MyObjectBuilder_EnvironmentSector GetObjectBuilder()
    {
      List<MyObjectBuilder_ProceduralEnvironmentSector.Module> moduleList = new List<MyObjectBuilder_ProceduralEnvironmentSector.Module>(this.m_modules.Count);
      foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
      {
        MyObjectBuilder_EnvironmentModuleBase objectBuilder = moduleData.Module.GetObjectBuilder();
        if (objectBuilder != null)
          moduleList.Add(new MyObjectBuilder_ProceduralEnvironmentSector.Module()
          {
            ModuleId = (SerializableDefinitionId) moduleData.Definition,
            Builder = objectBuilder
          });
      }
      if (moduleList.Count <= 0)
        return (MyObjectBuilder_EnvironmentSector) null;
      moduleList.Capacity = moduleList.Count;
      MyObjectBuilder_ProceduralEnvironmentSector environmentSector = new MyObjectBuilder_ProceduralEnvironmentSector();
      environmentSector.SavedModules = moduleList.ToArray();
      environmentSector.SectorId = this.Id;
      return (MyObjectBuilder_EnvironmentSector) environmentSector;
    }

    public override void GetItemDefinition(ushort key, out MyRuntimeEnvironmentItemInfo it) => it = this.m_environment.Items[(int) key];

    public override void Close()
    {
      foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
        moduleData.Module.Close();
      this.m_modules.Clear();
      this.m_items.Clear();
      base.Close();
    }

    public override void DebugDraw(int lod)
    {
      Vector3D vector3D = this.WorldPos + MySector.MainCamera.UpVector * 1f;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        ItemInfo itemInfo = this.m_items[index];
        Vector3D worldCoord = itemInfo.Position + vector3D;
        MyRuntimeEnvironmentItemInfo def;
        this.Owner.GetDefinition((ushort) itemInfo.DefinitionIndex, out def);
        string text = string.Format("{0} i{1} m{2} d{3}", (object) def.Type.Name, (object) index, (object) itemInfo.ModelIndex, (object) itemInfo.DefinitionIndex);
        Color purple = Color.Purple;
        MyRenderProxy.DebugDrawText3D(worldCoord, text, purple, 0.7f, true);
      }
      foreach (MyProceduralLogicalSector.ModuleData moduleData in this.m_modules.Values)
        moduleData.Module.DebugDraw();
    }

    public override void DisableItemsInBox(Vector3D center, ref BoundingBoxD box)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        Vector3D point = center + this.m_items[index].Position;
        ContainmentType result;
        box.Contains(ref point, out result);
        if (result == ContainmentType.Contains)
          this.EnableItem(index, false);
      }
    }

    public override void GetItemsInAabb(ref BoundingBoxD aabb, List<int> itemsInBox)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if (this.m_items[index].IsEnabled && this.m_items[index].DefinitionIndex >= (short) 0 && aabb.Contains((Vector3D) this.m_items[index].Position) != ContainmentType.Disjoint)
          itemsInBox.Add(index);
      }
    }

    public override void GetItem(int logicalItem, out ItemInfo item)
    {
      if (logicalItem >= this.m_items.Count || logicalItem < 0)
        item = new ItemInfo();
      else
        item = this.m_items[logicalItem];
    }

    public override void IterateItems(MyLogicalEnvironmentSectorBase.ItemIterator action)
    {
      ItemInfo[] internalArray = this.m_items.GetInternalArray();
      for (int index = 0; index < this.m_items.Count; ++index)
        action(index, ref internalArray[index]);
    }

    public override void InvalidateItem(int itemId)
    {
      if (itemId < 0 || itemId >= this.m_items.Count)
        return;
      ItemInfo itemInfo = this.m_items[itemId];
      itemInfo.IsEnabled = false;
      this.m_items[itemId] = itemInfo;
    }

    public override void RevalidateItem(int itemId)
    {
      if (itemId < 0 || itemId >= this.m_items.Count)
        return;
      ItemInfo itemInfo = this.m_items[itemId];
      itemInfo.IsEnabled = true;
      this.m_items[itemId] = itemInfo;
    }

    public override bool ServerOwned
    {
      get => this.m_serverOwned;
      internal set
      {
        this.m_serverOwned = value;
        if (Sync.IsServer || this.m_viewers.Count != 0 || this.OnViewerEmpty == null)
          return;
        this.OnViewerEmpty(this);
      }
    }

    public void RaiseItemEvent<TModule>(int logicalItem, object eventData, bool fromClient = false) where TModule : IMyEnvironmentModule
    {
      MyDefinitionId definition = this.m_modules[typeof (TModule)].Definition;
      this.RaiseItemEvent<object>(logicalItem, ref definition, eventData, fromClient);
    }

    public override void RaiseItemEvent<T>(
      int logicalItem,
      ref MyDefinitionId modDef,
      T eventData,
      bool fromClient)
    {
      if (fromClient)
        MyMultiplayer.RaiseEvent<MyProceduralLogicalSector, int, SerializableDefinitionId, object>(this, (Func<MyProceduralLogicalSector, Action<int, SerializableDefinitionId, object>>) (x => new Action<int, SerializableDefinitionId, object>(x.HandleItemEventClient)), logicalItem, (SerializableDefinitionId) modDef, (object) eventData);
      else
        MyMultiplayer.RaiseEvent<MyProceduralLogicalSector, int, SerializableDefinitionId, object>(this, (Func<MyProceduralLogicalSector, Action<int, SerializableDefinitionId, object>>) (x => new Action<int, SerializableDefinitionId, object>(x.HandleItemEventServer)), logicalItem, (SerializableDefinitionId) modDef, (object) eventData);
    }

    [Broadcast]
    [Event(null, 740)]
    [Reliable]
    private void HandleItemEventServer(int logicalItem, SerializableDefinitionId def, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyDynamicObjectResolver))] object data) => this.HandleItemEvent(logicalItem, def, data, false);

    [Event(null, 749)]
    [Reliable]
    [Server]
    private void HandleItemEventClient(int logicalItem, SerializableDefinitionId def, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyDynamicObjectResolver))] object data) => this.HandleItemEvent(logicalItem, def, data, true);

    private void HandleItemEvent(
      int logicalItem,
      SerializableDefinitionId def,
      object data,
      bool fromClient)
    {
      if (typeof (MyObjectBuilder_ProceduralEnvironmentModuleDefinition).IsAssignableFrom((Type) def.TypeId))
      {
        MyProceduralEnvironmentModuleDefinition definition = MyDefinitionManager.Static.GetDefinition<MyProceduralEnvironmentModuleDefinition>((MyDefinitionId) def);
        if (definition == null)
        {
          MyLog.Default.Error("Received message about unknown logical module {0}", (object) def);
        }
        else
        {
          MyProceduralLogicalSector.ModuleData moduleData;
          if (!this.m_modules.TryGetValue(definition.ModuleType, out moduleData))
            return;
          moduleData.Module.HandleSyncEvent(logicalItem, data, fromClient);
        }
      }
      else
      {
        MyEnvironmentModuleProxyDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentModuleProxyDefinition>((MyDefinitionId) def);
        if (definition == null)
        {
          MyLog.Default.Error("Received message about unknown module proxy {0}", (object) def);
        }
        else
        {
          foreach (MyProceduralDataView viewer in this.m_viewers)
          {
            if (viewer.Listener != null)
            {
              IMyEnvironmentModuleProxy module = viewer.Listener.GetModule(definition.ModuleType);
              int sectorIndex = viewer.GetSectorIndex(this.m_x, this.m_y);
              int sectorOffset = viewer.SectorOffsets[sectorIndex];
              if (logicalItem < this.m_itemCountForLod[viewer.Lod] && module != null)
                module.HandleSyncEvent(logicalItem + sectorOffset, data, fromClient);
            }
          }
        }
      }
    }

    private void UpdateMinLod()
    {
      this.MinLod = int.MaxValue;
      foreach (MyEnvironmentDataView viewer in this.m_viewers)
        this.MinLod = Math.Min(viewer.Lod, this.MinLod);
      bool flag = this.MinLod <= this.m_provider.SyncLod;
      if (flag == this.Replicable)
        return;
      if (flag)
        this.m_provider.MarkReplicable(this);
      else
        this.m_provider.UnmarkReplicable(this);
      this.Replicable = flag;
    }

    public override string ToString() => string.Format("x{0} y{1} l{2} : {3}", (object) this.m_x, (object) this.m_y, (object) this.m_lod, (object) this.m_items.Count);

    public override string DebugData => string.Format("x:{0} y:{1} highLod:{2} localLod:{3} seed:{4:X} count:{5} ", (object) this.m_x, (object) this.m_y, (object) this.m_lod, (object) this.m_minimumScannedLod, (object) this.m_seed, (object) this.m_items.Count);

    public void AddView(MyProceduralDataView view, Vector3D localOrigin, int logicalLod)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        this.ScanItems(logicalLod);
        this.m_viewers.Add(view);
        this.UpdateMinLod();
        view.AddSector(this);
        int num = this.m_itemCountForLod[logicalLod];
        view.Items.Capacity = view.Items.Count + this.m_items.Count;
        Vector3 vector3 = (Vector3) (this.WorldPos - localOrigin);
        for (int index = 0; index < num; ++index)
        {
          ItemInfo itemInfo = this.m_items[index];
          itemInfo.Position += vector3;
          view.Items.Add(itemInfo);
        }
      }
    }

    public void RemoveView(MyProceduralDataView view)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        this.m_viewers.Remove(view);
        this.UpdateMinLod();
        if (this.m_viewers.Count != 0 || this.OnViewerEmpty == null)
          return;
        this.OnViewerEmpty(this);
      }
    }

    public event Action<MyProceduralLogicalSector> OnViewerEmpty;

    private class ModuleData
    {
      public readonly Dictionary<short, MyLodEnvironmentItemSet> ItemsPerDefinition = new Dictionary<short, MyLodEnvironmentItemSet>();
      public readonly IMyEnvironmentModule Module;
      public readonly MyDefinitionId Definition;

      public ModuleData(Type type, MyDefinitionId definition)
      {
        this.Module = (IMyEnvironmentModule) Activator.CreateInstance(type);
        this.Definition = definition;
      }
    }

    private class ProgressiveScanHelper
    {
      private readonly int m_itemsTotal;
      private readonly int m_offset;
      private const bool EXAGERATE = true;
      private readonly double m_base;
      private readonly double m_logMaxLodRecip;

      public ProgressiveScanHelper(int finalCount, int offset)
      {
        this.m_itemsTotal = finalCount;
        this.m_logMaxLodRecip = 1.0 / Math.Log(4.0);
        this.m_base = Math.Log(10.0) * this.m_logMaxLodRecip;
        this.m_offset = offset;
      }

      private double F(double x) => -Math.Pow(this.m_base, -x) * this.m_logMaxLodRecip;

      public int GetItemsForLod(int lod)
      {
        lod += this.m_offset;
        return (int) ((double) this.m_itemsTotal * (this.F((double) (lod + 1)) - this.F((double) lod)));
      }
    }

    protected sealed class HandleItemEventServer\u003C\u003ESystem_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Object : ICallSite<MyProceduralLogicalSector, int, SerializableDefinitionId, object, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProceduralLogicalSector @this,
        in int logicalItem,
        in SerializableDefinitionId def,
        in object data,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.HandleItemEventServer(logicalItem, def, data);
      }
    }

    protected sealed class HandleItemEventClient\u003C\u003ESystem_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Object : ICallSite<MyProceduralLogicalSector, int, SerializableDefinitionId, object, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProceduralLogicalSector @this,
        in int logicalItem,
        in SerializableDefinitionId def,
        in object data,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.HandleItemEventClient(logicalItem, def, data);
      }
    }
  }
}
