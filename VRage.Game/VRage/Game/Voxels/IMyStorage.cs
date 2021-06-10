// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.IMyStorage
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Voxels;
using VRageMath;

namespace VRage.Game.Voxels
{
  public interface IMyStorage : VRage.ModAPI.IMyStorage
  {
    uint StorageId { get; }

    void Close();

    bool Shared { get; }

    IMyStorage Copy();

    StoragePin Pin();

    void Unpin();

    ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true);

    new bool Intersect(ref LineD line);

    event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

    void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode);

    IMyStorageDataProvider DataProvider { get; }

    void SetDataCache(byte[] data, bool compressed);

    byte[] GetVoxelData();

    bool AreDataCached { get; }

    bool AreDataCachedCompressed { get; }

    void NotifyChanged(
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      MyStorageDataTypeFlags changedData);
  }
}
