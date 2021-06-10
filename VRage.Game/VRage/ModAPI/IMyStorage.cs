// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyStorage
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Voxels;
using VRageMath;

namespace VRage.ModAPI
{
  public interface IMyStorage
  {
    bool Closed { get; }

    bool MarkedForClose { get; }

    void Save(out byte[] outCompressedData);

    Vector3I Size { get; }

    [Obsolete]
    void OverwriteAllMaterials(byte materialIndex);

    ContainmentType Intersect(ref BoundingBox box, bool lazy);

    bool Intersect(ref LineD line);

    void PinAndExecute(Action action);

    void PinAndExecute(Action<IMyStorage> action);

    void Reset(MyStorageDataTypeFlags dataToReset);

    void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      ref MyVoxelRequestFlags requestFlags);

    void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataToRead,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax);

    void WriteRange(
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify = true,
      bool skipCache = false);

    bool DeleteSupported { get; }

    void DeleteRange(
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax,
      bool notify);

    void ExecuteOperationFast<TVoxelOperator>(
      ref TVoxelOperator voxelOperator,
      MyStorageDataTypeFlags dataToWrite,
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      bool notifyRangeChanged)
      where TVoxelOperator : struct, IVoxelOperator;

    void NotifyRangeChanged(
      ref Vector3I voxelRangeMin,
      ref Vector3I voxelRangeMax,
      MyStorageDataTypeFlags dataChanged);
  }
}
