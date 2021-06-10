// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyVoxelMaps
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyVoxelMaps
  {
    void Clear();

    bool Exist(IMyVoxelBase voxelMap);

    IMyVoxelBase GetOverlappingWithSphere(ref BoundingSphereD sphere);

    IMyVoxelBase GetVoxelMapWhoseBoundingBoxIntersectsBox(
      ref BoundingBoxD boundingBox,
      IMyVoxelBase ignoreVoxelMap);

    void GetInstances(List<IMyVoxelBase> outInstances, Func<IMyVoxelBase, bool> collect = null);

    IMyStorage CreateStorage(Vector3I size);

    IMyStorage CreateStorage(byte[] data);

    IMyVoxelMap CreateVoxelMap(
      string storageName,
      IMyStorage storage,
      Vector3D position,
      long voxelMapId);

    IMyVoxelMap CreateVoxelMapFromStorageName(
      string storageName,
      string prefabVoxelMapName,
      Vector3D position);

    IMyVoxelShapeBox GetBoxVoxelHand();

    IMyVoxelShapeCapsule GetCapsuleVoxelHand();

    IMyVoxelShapeSphere GetSphereVoxelHand();

    IMyVoxelShapeRamp GetRampVoxelHand();

    void PaintInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx);

    void CutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape);

    void FillInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx);

    void RevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape);

    int VoxelMaterialCount { get; }

    void MakeCrater(
      IMyVoxelBase voxelMap,
      BoundingSphereD sphere,
      Vector3 direction,
      byte materialIdx);
  }
}
