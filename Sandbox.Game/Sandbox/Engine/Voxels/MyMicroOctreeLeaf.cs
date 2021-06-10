// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyMicroOctreeLeaf
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game.Voxels;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MyMicroOctreeLeaf : IMyOctreeLeafNode, IDisposable
  {
    private const bool DEBUG_WRITES = false;
    private MySparseOctree m_octree;
    private MyStorageDataTypeEnum m_dataType;
    private Vector3I m_voxelRangeMin;

    public MyMicroOctreeLeaf(MyStorageDataTypeEnum dataType, int height, Vector3I voxelRangeMin)
    {
      this.m_octree = dataType != MyStorageDataTypeEnum.Content ? new MySparseOctree(height, MyOctreeNode.MaterialFilter, byte.MaxValue) : new MySparseOctree(height, MyOctreeNode.ContentFilter);
      this.m_dataType = dataType;
      this.m_voxelRangeMin = voxelRangeMin;
    }

    internal void BuildFrom(MyStorageData source) => this.m_octree.Build<MyStorageData.MortonEnumerator>(new MyStorageData.MortonEnumerator(source, this.m_dataType));

    internal void BuildFrom(byte singleValue) => this.m_octree.Build(singleValue);

    internal void WriteTo(Stream stream) => this.m_octree.WriteTo(stream);

    internal void ReadFrom(MyOctreeStorage.ChunkHeader header, Stream stream)
    {
      if (this.m_octree == null)
        this.m_octree = new MySparseOctree(0, header.ChunkType == MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree ? MyOctreeNode.ContentFilter : MyOctreeNode.MaterialFilter);
      this.m_octree.ReadFrom(header, stream);
    }

    public bool TryGetUniformValue(out byte uniformValue)
    {
      if (this.m_octree.IsAllSame)
      {
        uniformValue = this.m_octree.GetFilteredValue();
        return true;
      }
      uniformValue = (byte) 0;
      return false;
    }

    internal void DebugDraw(
      IMyDebugDrawBatchAabb batch,
      Vector3 worldPos,
      MyVoxelDebugDrawMode mode)
    {
      this.m_octree.DebugDraw(batch, worldPos, mode);
    }

    Vector3I IMyOctreeLeafNode.VoxelRangeMin => this.m_voxelRangeMin;

    bool IMyOctreeLeafNode.ReadOnly => false;

    void IMyOctreeLeafNode.ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags types,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      ref MyVoxelRequestFlags flags)
    {
      this.m_octree.ReadRange(target, this.m_dataType, ref writeOffset, lodIndex, ref minInLod, ref maxInLod);
      flags = (MyVoxelRequestFlags) 0;
    }

    void IMyOctreeLeafNode.ExecuteOperation<TOperator>(
      ref TOperator source,
      ref Vector3I readOffset,
      ref Vector3I min,
      ref Vector3I max)
    {
      this.m_octree.ExecuteOperation<TOperator>(ref source, this.m_dataType, ref readOffset, ref min, ref max);
    }

    byte IMyOctreeLeafNode.GetFilteredValue() => this.m_octree.GetFilteredValue();

    void IMyOctreeLeafNode.OnDataProviderChanged(IMyStorageDataProvider newProvider)
    {
    }

    MyOctreeStorage.ChunkTypeEnum IMyOctreeLeafNode.SerializedChunkType => this.m_dataType != MyStorageDataTypeEnum.Content ? MyOctreeStorage.ChunkTypeEnum.MaterialLeafOctree : MyOctreeStorage.ChunkTypeEnum.ContentLeafOctree;

    int IMyOctreeLeafNode.SerializedChunkSize => this.m_octree.SerializedSize;

    void IMyOctreeLeafNode.ReplaceValues(Dictionary<byte, byte> oldToNewValueMap) => this.m_octree.ReplaceValues(oldToNewValueMap);

    public ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true)
    {
      BoundingBoxI box1 = box;
      box1.Translate(-this.m_voxelRangeMin);
      return this.m_octree.Intersect(ref box1, lod, exhaustiveContainmentCheck);
    }

    public bool Intersect(ref LineD line, out double startOffset, out double endOffset)
    {
      line.From -= (Vector3D) this.m_voxelRangeMin;
      line.To -= (Vector3D) this.m_voxelRangeMin;
      if (!this.m_octree.Intersect(ref line, out startOffset, out endOffset))
        return false;
      line.From += (Vector3D) this.m_voxelRangeMin;
      line.To += (Vector3D) this.m_voxelRangeMin;
      return true;
    }

    public void Dispose() => this.m_octree?.Dispose();
  }
}
