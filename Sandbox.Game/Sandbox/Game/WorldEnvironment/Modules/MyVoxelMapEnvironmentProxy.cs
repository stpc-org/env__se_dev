// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyVoxelMapEnvironmentProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  public class MyVoxelMapEnvironmentProxy : IMyEnvironmentModuleProxy
  {
    protected MyEnvironmentSector m_sector;
    protected MyPlanet m_planet;
    protected readonly MyRandom m_random = new MyRandom();
    protected readonly MyVoxelBase.StorageChanged m_voxelMap_RangeChangedDelegate;
    protected List<int> m_items;
    protected List<MyVoxelMapEnvironmentProxy.VoxelMapInfo> m_voxelMapsToAdd = new List<MyVoxelMapEnvironmentProxy.VoxelMapInfo>();
    protected Dictionary<MyVoxelMap, int> m_voxelMaps = new Dictionary<MyVoxelMap, int>();
    private static List<MyEntity> m_entities = new List<MyEntity>();

    public MyVoxelMapEnvironmentProxy() => this.m_voxelMap_RangeChangedDelegate = new MyVoxelBase.StorageChanged(this.VoxelMap_RangeChanged);

    public void Init(MyEnvironmentSector sector, List<int> items)
    {
      this.m_sector = sector;
      this.m_planet = ((MyEntityComponentBase) this.m_sector.Owner).Entity as MyPlanet;
      this.m_items = items;
      this.LoadVoxelMapsInfo();
    }

    public void Close() => this.RemoveVoxelMaps();

    public void CommitLodChange(int lodBefore, int lodAfter)
    {
      if (lodAfter >= 0)
      {
        this.AddVoxelMaps();
      }
      else
      {
        if (this.m_sector.HasPhysics)
          return;
        this.RemoveVoxelMaps();
      }
    }

    public void CommitPhysicsChange(bool enabled)
    {
      if (enabled)
      {
        this.AddVoxelMaps();
      }
      else
      {
        if (this.m_sector.LodLevel != -1)
          return;
        this.RemoveVoxelMaps();
      }
    }

    public void OnItemChange(int index, short newModel)
    {
    }

    public void OnItemChangeBatch(List<int> items, int offset, short newModel)
    {
    }

    public void HandleSyncEvent(int item, object data, bool fromClient)
    {
    }

    public void DebugDraw()
    {
    }

    private void LoadVoxelMapsInfo()
    {
      this.m_voxelMapsToAdd.Clear();
      foreach (int index in this.m_items)
      {
        Sandbox.Game.WorldEnvironment.ItemInfo itemInfo = this.m_sector.DataView.Items[index];
        MyRuntimeEnvironmentItemInfo def;
        this.m_sector.Owner.GetDefinition((ushort) itemInfo.DefinitionIndex, out def);
        MyVoxelMapCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyVoxelMapCollectionDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_VoxelMapCollectionDefinition), def.Subtype));
        if (definition != null)
        {
          int logicalItem;
          MyLogicalEnvironmentSectorBase sector;
          this.m_sector.DataView.GetLogicalSector(index, out logicalItem, out sector);
          string uniqueString = string.Format("P({0})S({1})A({2}__{3})", (object) this.m_sector.Owner.Entity.Name, (object) sector.Id, (object) def.Subtype, (object) logicalItem);
          MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(itemInfo.Rotation);
          fromQuaternion.Translation = this.m_sector.SectorCenter + itemInfo.Position;
          long num = MyEntityIdentifier.ConstructIdFromString(MyEntityIdentifier.ID_OBJECT_TYPE.PLANET_VOXEL_DETAIL, uniqueString);
          MyBoulderInformation boulderInformation = new MyBoulderInformation();
          boulderInformation.PlanetId = this.m_planet.EntityId;
          boulderInformation.SectorId = sector.Id;
          boulderInformation.ItemId = index;
          using (this.m_random.PushSeed(itemInfo.Rotation.GetHashCode()))
            this.m_voxelMapsToAdd.Add(new MyVoxelMapEnvironmentProxy.VoxelMapInfo()
            {
              Name = uniqueString,
              Storage = definition.StorageFiles.Sample(this.m_random),
              Matrix = fromQuaternion,
              Item = index,
              Modifier = definition.Modifier,
              EntityId = num,
              BoulderInfo = new MyBoulderInformation?(boulderInformation)
            });
        }
      }
    }

    private void AddVoxelMaps()
    {
      if (this.m_voxelMaps.Count > 0)
        return;
      foreach (MyVoxelMapEnvironmentProxy.VoxelMapInfo voxelMapInfo in this.m_voxelMapsToAdd)
      {
        MyVoxelMap entity;
        if (MyEntities.TryGetEntityById<MyVoxelMap>(voxelMapInfo.EntityId, out entity))
        {
          if (!entity.Save)
            this.RegisterVoxelMap(voxelMapInfo.Item, entity);
        }
        else
        {
          MyVoxelMaterialModifierDefinition definition = MyDefinitionManager.Static.GetDefinition<MyVoxelMaterialModifierDefinition>(voxelMapInfo.Modifier);
          Dictionary<byte, byte> modifiers = (Dictionary<byte, byte>) null;
          if (definition != null)
            modifiers = definition.Options.Sample(MyHashRandomUtils.UniformFloatFromSeed(voxelMapInfo.Item + this.m_sector.SectorId.GetHashCode())).Changes;
          this.AddVoxelMap(voxelMapInfo.Item, voxelMapInfo.Storage.SubtypeName, voxelMapInfo.Matrix, voxelMapInfo.Name, voxelMapInfo.EntityId, modifiers, voxelMapInfo.BoulderInfo);
        }
      }
    }

    private void AddVoxelMap(
      int item,
      string prefabName,
      MatrixD matrix,
      string name,
      long entityId,
      Dictionary<byte, byte> modifiers = null,
      MyBoulderInformation? boulderInfo = null)
    {
      MyStorageBase storage = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(prefabName), modifiers);
      if (storage == null)
        return;
      MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(matrix.Translation, (Vector3D) (storage.Size * 0.5f), Quaternion.CreateFromRotationMatrix(in matrix));
      BoundingBoxD aabb = other.GetAABB();
      using (MyUtils.ReuseCollection<MyEntity>(ref MyVoxelMapEnvironmentProxy.m_entities))
      {
        MyGamePruningStructure.GetTopMostEntitiesInBox(ref aabb, MyVoxelMapEnvironmentProxy.m_entities, MyEntityQueryType.Static);
        foreach (MyEntity entity in MyVoxelMapEnvironmentProxy.m_entities)
        {
          switch (entity)
          {
            case MyVoxelPhysics _:
            case MyPlanet _:
            case MyEnvironmentSector _:
              continue;
            default:
              MyPositionComponentBase positionComp = entity.PositionComp;
              if (MyOrientedBoundingBoxD.Create((BoundingBoxD) positionComp.LocalAABB, positionComp.WorldMatrixRef).Intersects(ref other))
                return;
              continue;
          }
        }
      }
      MyVoxelMap voxelMap = MyWorldGenerator.AddVoxelMap(name, storage, matrix, entityId, true);
      if (voxelMap == null)
        return;
      voxelMap.BoulderInfo = boulderInfo;
      this.RegisterVoxelMap(item, voxelMap);
    }

    private void RegisterVoxelMap(int item, MyVoxelMap voxelMap)
    {
      voxelMap.Save = false;
      voxelMap.RangeChanged += this.m_voxelMap_RangeChangedDelegate;
      this.m_voxelMaps[voxelMap] = item;
      MyEntityReferenceComponent component;
      if (!voxelMap.Components.TryGet<MyEntityReferenceComponent>(out component))
        voxelMap.Components.Add<MyEntityReferenceComponent>(component = new MyEntityReferenceComponent());
      this.DisableOtherItemsInVMap((MyVoxelBase) voxelMap);
      component.Ref();
    }

    private unsafe void DisableOtherItemsInVMap(MyVoxelBase voxelMap)
    {
      MyOrientedBoundingBoxD obb = MyOrientedBoundingBoxD.Create((BoundingBoxD) voxelMap.PositionComp.LocalAABB, voxelMap.PositionComp.WorldMatrixRef);
      Vector3D center = obb.Center;
      BoundingBoxD worldAabb = voxelMap.PositionComp.WorldAABB;
      using (MyUtils.ReuseCollection<MyEntity>(ref MyVoxelMapEnvironmentProxy.m_entities))
      {
        MyGamePruningStructure.GetAllEntitiesInBox(ref worldAabb, MyVoxelMapEnvironmentProxy.m_entities, MyEntityQueryType.Static);
        for (int index1 = 0; index1 < MyVoxelMapEnvironmentProxy.m_entities.Count; ++index1)
        {
          MyEnvironmentSector sector = MyVoxelMapEnvironmentProxy.m_entities[index1] as MyEnvironmentSector;
          if (sector != null && sector.DataView != null)
          {
            obb.Center = center - sector.SectorCenter;
            for (int index2 = 0; index2 < sector.DataView.LogicalSectors.Count; ++index2)
            {
              MyLogicalEnvironmentSectorBase logicalSector = sector.DataView.LogicalSectors[index2];
              logicalSector.IterateItems((MyLogicalEnvironmentSectorBase.ItemIterator) ((int i, ref Sandbox.Game.WorldEnvironment.ItemInfo x) =>
              {
                Vector3D vector3D = x.Position + sector.SectorCenter;
                if (!x.IsEnabled || x.DefinitionIndex < (short) 0 || (!obb.Contains(ref x.Position) || voxelMap.CountPointsInside(&vector3D, 1) <= 0) || MyVoxelMapEnvironmentProxy.IsVoxelItem(sector, x.DefinitionIndex))
                  return;
                logicalSector.EnableItem(i, false);
              }));
            }
          }
        }
      }
    }

    private static bool IsVoxelItem(MyEnvironmentSector sector, short definitionIndex)
    {
      MyItemTypeDefinition.Module[] proxyModules = sector.Owner.EnvironmentDefinition.Items[(int) definitionIndex].Type.ProxyModules;
      if (proxyModules == null)
        return false;
      for (int index = 0; index < proxyModules.Length; ++index)
      {
        if (proxyModules[index].Type.IsSubclassOf(typeof (MyVoxelMapEnvironmentProxy)) || proxyModules[index].Type == typeof (MyVoxelMapEnvironmentProxy))
          return true;
      }
      return false;
    }

    private void RemoveVoxelMaps()
    {
      foreach (KeyValuePair<MyVoxelMap, int> voxelMap in this.m_voxelMaps)
      {
        MyVoxelMap key = voxelMap.Key;
        if (!key.Closed)
        {
          if (Sync.IsServer || !key.Save)
            key.Components.Get<MyEntityReferenceComponent>().Unref();
          key.RangeChanged -= this.m_voxelMap_RangeChangedDelegate;
        }
      }
      this.m_voxelMaps.Clear();
      this.m_voxelMapsToAdd.Clear();
    }

    private int GetBoulderDefinition(int itemId) => this.m_sector.GetItemDefinitionId(itemId);

    private void RemoveVoxelMap(MyVoxelMap map)
    {
      map.Save = true;
      map.RangeChanged -= this.m_voxelMap_RangeChangedDelegate;
      if (!this.m_voxelMaps.ContainsKey(map))
        return;
      this.m_sector.EnableItem(this.m_voxelMaps[map], false);
      this.m_voxelMaps.Remove(map);
    }

    private void VoxelMap_RangeChanged(
      MyVoxelBase voxel,
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged,
      MyStorageDataTypeFlags changedData)
    {
      this.RemoveVoxelMap((MyVoxelMap) voxel);
    }

    protected struct VoxelMapInfo
    {
      public string Name;
      public MyDefinitionId Storage;
      public MatrixD Matrix;
      public int Item;
      public MyStringHash Modifier;
      public long EntityId;
      public MyBoulderInformation? BoulderInfo;
    }
  }
}
