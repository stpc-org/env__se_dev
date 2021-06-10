// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelMaps
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyVoxelMaps : IMyVoxelMaps
  {
    private readonly Dictionary<long, MyVoxelBase> m_voxelMapsByEntityId = new Dictionary<long, MyVoxelBase>();
    private readonly List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();
    private static MyShapeBox m_boxVoxelShape = new MyShapeBox();
    private static MyShapeCapsule m_capsuleShape = new MyShapeCapsule();
    private static MyShapeSphere m_sphereShape = new MyShapeSphere();
    private static MyShapeRamp m_rampShape = new MyShapeRamp();
    private static readonly List<MyVoxelBase> m_voxelsTmpStorage = new List<MyVoxelBase>();

    public void Clear()
    {
      foreach (KeyValuePair<long, MyVoxelBase> keyValuePair in this.m_voxelMapsByEntityId)
        keyValuePair.Value.Close();
      this.m_voxelMapsByEntityId.Clear();
    }

    public DictionaryValuesReader<long, MyVoxelBase> Instances => (DictionaryValuesReader<long, MyVoxelBase>) this.m_voxelMapsByEntityId;

    public bool Exist(MyVoxelBase voxelMap) => this.m_voxelMapsByEntityId.ContainsKey(voxelMap.EntityId);

    public void RemoveVoxelMap(MyVoxelBase voxelMap) => this.m_voxelMapsByEntityId.Remove(voxelMap.EntityId);

    public MyVoxelBase GetOverlappingWithSphere(ref BoundingSphereD sphere)
    {
      MyVoxelBase myVoxelBase = (MyVoxelBase) null;
      MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, this.m_tmpVoxelMapsList);
      foreach (MyVoxelBase tmpVoxelMaps in this.m_tmpVoxelMapsList)
      {
        if (tmpVoxelMaps.DoOverlapSphereTest((float) sphere.Radius, sphere.Center))
        {
          myVoxelBase = tmpVoxelMaps;
          break;
        }
      }
      this.m_tmpVoxelMapsList.Clear();
      return myVoxelBase;
    }

    public void GetAllOverlappingWithSphere(ref BoundingSphereD sphere, List<MyVoxelBase> voxels) => MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, voxels);

    public List<MyVoxelBase> GetAllOverlappingWithSphere(ref BoundingSphereD sphere)
    {
      List<MyVoxelBase> result = new List<MyVoxelBase>();
      MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, result);
      return result;
    }

    public void Add(MyVoxelBase voxelMap)
    {
      if (this.Exist(voxelMap))
        return;
      this.m_voxelMapsByEntityId.Add(voxelMap.EntityId, voxelMap);
    }

    public MyVoxelBase GetVoxelMapWhoseBoundingBoxIntersectsBox(
      ref BoundingBoxD boundingBox,
      MyVoxelBase ignoreVoxelMap)
    {
      MyVoxelBase myVoxelBase1 = (MyVoxelBase) null;
      double num1 = double.MaxValue;
      foreach (MyVoxelBase myVoxelBase2 in this.m_voxelMapsByEntityId.Values)
      {
        if (!myVoxelBase2.MarkedForClose && !myVoxelBase2.Closed && (myVoxelBase2 != ignoreVoxelMap && myVoxelBase2.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox)))
        {
          double num2 = Vector3D.DistanceSquared(myVoxelBase2.PositionComp.WorldAABB.Center, boundingBox.Center);
          if (num2 < num1)
          {
            num1 = num2;
            myVoxelBase1 = myVoxelBase2;
          }
        }
      }
      return myVoxelBase1;
    }

    public bool GetVoxelMapsWhoseBoundingBoxesIntersectBox(
      ref BoundingBoxD boundingBox,
      MyVoxelBase ignoreVoxelMap,
      List<MyVoxelBase> voxelList)
    {
      int num = 0;
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (!myVoxelBase.MarkedForClose && !myVoxelBase.Closed && (myVoxelBase != ignoreVoxelMap && myVoxelBase.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox)))
        {
          voxelList.Add(myVoxelBase);
          ++num;
        }
      }
      return num > 0;
    }

    public MyVoxelBase TryGetVoxelMapByNameStart(string name)
    {
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (myVoxelBase.StorageName != null && myVoxelBase.StorageName.StartsWith(name))
          return myVoxelBase;
      }
      return (MyVoxelBase) null;
    }

    public MyVoxelBase TryGetVoxelMapByName(string name)
    {
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (myVoxelBase.StorageName == name)
          return myVoxelBase;
      }
      return (MyVoxelBase) null;
    }

    public MyVoxelBase TryGetVoxelBaseById(long id) => !this.m_voxelMapsByEntityId.ContainsKey(id) ? (MyVoxelBase) null : this.m_voxelMapsByEntityId[id];

    public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged)
    {
      Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (myVoxelBase.Storage != null && (includeChanged || !myVoxelBase.ContentChanged && !myVoxelBase.BeforeContentChanged) && (myVoxelBase.Save && !dictionary.ContainsKey(myVoxelBase.StorageName)))
        {
          byte[] outCompressedData = (byte[]) null;
          myVoxelBase.Storage.Save(out outCompressedData);
          dictionary.Add(myVoxelBase.StorageName, outCompressedData);
        }
      }
      return dictionary;
    }

    public Dictionary<string, byte[]> GetVoxelMapsData(
      bool includeChanged,
      bool compressed,
      Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = null)
    {
      Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (myVoxelBase.Storage != null && (includeChanged || !myVoxelBase.ContentChanged && !myVoxelBase.BeforeContentChanged) && (myVoxelBase.Save && !dictionary.ContainsKey(myVoxelBase.StorageName)))
        {
          byte[] outCompressedData = (byte[]) null;
          if (myVoxelBase.Storage.AreDataCached)
          {
            if (compressed == myVoxelBase.Storage.AreDataCachedCompressed)
              myVoxelBase.Storage.Save(out outCompressedData);
            else
              continue;
          }
          else if (!compressed)
            outCompressedData = myVoxelBase.Storage.GetVoxelData();
          else
            continue;
          dictionary.Add(myVoxelBase.StorageName, outCompressedData);
          voxelStorageNameCache?.Add(myVoxelBase.StorageName, myVoxelBase.Storage);
        }
      }
      return dictionary;
    }

    public void DebugDraw(MyVoxelDebugDrawMode drawMode)
    {
      foreach (MyVoxelBase myVoxelBase in this.m_voxelMapsByEntityId.Values)
      {
        if (!(myVoxelBase is MyVoxelPhysics))
        {
          MatrixD worldMatrix = myVoxelBase.WorldMatrix;
          worldMatrix.Translation = myVoxelBase.PositionLeftBottomCorner;
          myVoxelBase.Storage.DebugDraw(ref worldMatrix, drawMode);
        }
      }
    }

    public void GetCacheStats(out int cachedChuncks, out int pendingCachedChuncks)
    {
      cachedChuncks = pendingCachedChuncks = 0;
      foreach (KeyValuePair<long, MyVoxelBase> keyValuePair in this.m_voxelMapsByEntityId)
      {
        if (!(keyValuePair.Value is MyVoxelPhysics) && keyValuePair.Value.Storage is MyOctreeStorage storage)
        {
          cachedChuncks += storage.CachedChunksCount;
          pendingCachedChuncks += storage.PendingCachedChunksCount;
        }
      }
    }

    internal void GetAllIds(ref List<long> list)
    {
      foreach (long key in this.m_voxelMapsByEntityId.Keys)
        list.Add(key);
    }

    void IMyVoxelMaps.Clear() => this.Clear();

    bool IMyVoxelMaps.Exist(IMyVoxelBase voxelMap) => this.Exist(voxelMap as MyVoxelBase);

    IMyVoxelBase IMyVoxelMaps.GetOverlappingWithSphere(
      ref BoundingSphereD sphere)
    {
      MyVoxelMaps.m_voxelsTmpStorage.Clear();
      this.GetAllOverlappingWithSphere(ref sphere, MyVoxelMaps.m_voxelsTmpStorage);
      return MyVoxelMaps.m_voxelsTmpStorage.Count == 0 ? (IMyVoxelBase) null : (IMyVoxelBase) MyVoxelMaps.m_voxelsTmpStorage[0];
    }

    IMyVoxelBase IMyVoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(
      ref BoundingBoxD boundingBox,
      IMyVoxelBase ignoreVoxelMap)
    {
      return (IMyVoxelBase) this.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, ignoreVoxelMap as MyVoxelBase);
    }

    void IMyVoxelMaps.GetInstances(
      List<IMyVoxelBase> voxelMaps,
      Func<IMyVoxelBase, bool> collect)
    {
      foreach (MyVoxelBase instance in this.Instances)
      {
        if (collect == null || collect((IMyVoxelBase) instance))
          voxelMaps.Add((IMyVoxelBase) instance);
      }
    }

    VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(Vector3I size) => (VRage.ModAPI.IMyStorage) new MyOctreeStorage((IMyStorageDataProvider) null, size);

    IMyVoxelMap IMyVoxelMaps.CreateVoxelMap(
      string storageName,
      VRage.ModAPI.IMyStorage storage,
      Vector3D position,
      long voxelMapId)
    {
      MyVoxelMap myVoxelMap = new MyVoxelMap();
      myVoxelMap.EntityId = voxelMapId;
      myVoxelMap.Init(storageName, storage as VRage.Game.Voxels.IMyStorage, position);
      MyEntities.Add((MyEntity) myVoxelMap);
      return (IMyVoxelMap) myVoxelMap;
    }

    IMyVoxelMap IMyVoxelMaps.CreateVoxelMapFromStorageName(
      string storageName,
      string prefabVoxelMapName,
      Vector3D position)
    {
      MyStorageBase storage = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(prefabVoxelMapName));
      if (storage == null)
        return (IMyVoxelMap) null;
      storage.DataProvider = (IMyStorageDataProvider) MyCompositeShapeProvider.CreateAsteroidShape(0, (float) storage.Size.AbsMax() * 1f);
      return (IMyVoxelMap) MyWorldGenerator.AddVoxelMap(storageName, storage, position);
    }

    VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(byte[] data)
    {
      bool isOldFormat = false;
      return (VRage.ModAPI.IMyStorage) MyStorageBase.Load(data, out isOldFormat);
    }

    IMyVoxelShapeBox IMyVoxelMaps.GetBoxVoxelHand() => (IMyVoxelShapeBox) MyVoxelMaps.m_boxVoxelShape;

    IMyVoxelShapeCapsule IMyVoxelMaps.GetCapsuleVoxelHand() => (IMyVoxelShapeCapsule) MyVoxelMaps.m_capsuleShape;

    IMyVoxelShapeSphere IMyVoxelMaps.GetSphereVoxelHand() => (IMyVoxelShapeSphere) MyVoxelMaps.m_sphereShape;

    IMyVoxelShapeRamp IMyVoxelMaps.GetRampVoxelHand() => (IMyVoxelShapeRamp) MyVoxelMaps.m_rampShape;

    void IMyVoxelMaps.PaintInShape(
      IMyVoxelBase voxelMap,
      IMyVoxelShape voxelShape,
      byte materialIdx)
    {
      MyVoxelGenerator.RequestPaintInShape(voxelMap, voxelShape, materialIdx);
    }

    void IMyVoxelMaps.CutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape) => MyVoxelGenerator.RequestCutOutShape(voxelMap, voxelShape);

    void IMyVoxelMaps.FillInShape(
      IMyVoxelBase voxelMap,
      IMyVoxelShape voxelShape,
      byte materialIdx)
    {
      MyVoxelGenerator.RequestFillInShape(voxelMap, voxelShape, materialIdx);
    }

    void IMyVoxelMaps.RevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape) => MyVoxelGenerator.RequestRevertShape(voxelMap, voxelShape);

    int IMyVoxelMaps.VoxelMaterialCount => MyDefinitionManager.Static.VoxelMaterialCount;

    void IMyVoxelMaps.MakeCrater(
      IMyVoxelBase voxelMap,
      BoundingSphereD sphere,
      Vector3 direction,
      byte materialIdx)
    {
      MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIdx);
      MyVoxelGenerator.MakeCrater((MyVoxelBase) voxelMap, sphere, direction, materialDefinition);
    }
  }
}
