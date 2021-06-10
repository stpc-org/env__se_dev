// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.IMyOctreeLeafNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public interface IMyOctreeLeafNode : IDisposable
  {
    MyOctreeStorage.ChunkTypeEnum SerializedChunkType { get; }

    int SerializedChunkSize { get; }

    Vector3I VoxelRangeMin { get; }

    bool ReadOnly { get; }

    byte GetFilteredValue();

    void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags types,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      ref MyVoxelRequestFlags flags);

    void ExecuteOperation<TOperator>(
      ref TOperator source,
      ref Vector3I readOffset,
      ref Vector3I min,
      ref Vector3I max)
      where TOperator : struct, IVoxelOperator;

    void OnDataProviderChanged(IMyStorageDataProvider newProvider);

    void ReplaceValues(Dictionary<byte, byte> oldToNewValueMap);

    ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true);

    bool Intersect(ref LineD box, out double startOffset, out double endOffset);

    bool TryGetUniformValue(out byte uniformValue);
  }
}
