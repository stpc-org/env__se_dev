// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyProviderLeaf
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  internal class MyProviderLeaf : IMyOctreeLeafNode, IDisposable
  {
    [ThreadStatic]
    private static MyStorageData m_filteredValueBuffer;
    private IMyStorageDataProvider m_provider;
    private MyStorageDataTypeEnum m_dataType;
    private MyCellCoord m_cell;

    private static MyStorageData FilteredValueBuffer
    {
      get
      {
        if (MyProviderLeaf.m_filteredValueBuffer == null)
        {
          MyProviderLeaf.m_filteredValueBuffer = new MyStorageData();
          MyProviderLeaf.m_filteredValueBuffer.Resize(Vector3I.One);
        }
        return MyProviderLeaf.m_filteredValueBuffer;
      }
    }

    public MyProviderLeaf(
      IMyStorageDataProvider provider,
      MyStorageDataTypeEnum dataType,
      ref MyCellCoord cell)
    {
      this.m_provider = provider;
      this.m_dataType = dataType;
      this.m_cell = cell;
    }

    [Conditional("DEBUG")]
    private void AssertRangeIsInside(int lodIndex, ref Vector3I globalMin, ref Vector3I globalMax)
    {
      int num1 = this.m_cell.Lod - lodIndex;
      int num2 = 1 << num1;
      Vector3I vector3I = (this.m_cell.CoordInLod << num1) + (num2 - 1);
    }

    MyOctreeStorage.ChunkTypeEnum IMyOctreeLeafNode.SerializedChunkType => this.m_dataType != MyStorageDataTypeEnum.Content ? MyOctreeStorage.ChunkTypeEnum.MaterialLeafProvider : MyOctreeStorage.ChunkTypeEnum.ContentLeafProvider;

    int IMyOctreeLeafNode.SerializedChunkSize => 0;

    Vector3I IMyOctreeLeafNode.VoxelRangeMin => this.m_cell.CoordInLod << this.m_cell.Lod;

    bool IMyOctreeLeafNode.ReadOnly => true;

    byte IMyOctreeLeafNode.GetFilteredValue()
    {
      MyStorageData filteredValueBuffer = MyProviderLeaf.FilteredValueBuffer;
      this.m_provider.ReadRange(filteredValueBuffer, this.m_dataType.ToFlags(), ref Vector3I.Zero, this.m_cell.Lod, ref this.m_cell.CoordInLod, ref this.m_cell.CoordInLod);
      return this.m_dataType != MyStorageDataTypeEnum.Material ? filteredValueBuffer.Content(0) : filteredValueBuffer.Material(0);
    }

    void IMyOctreeLeafNode.ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags types,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      ref MyVoxelRequestFlags flags)
    {
      Vector3I vector3I1 = this.m_cell.CoordInLod << this.m_cell.Lod - lodIndex;
      Vector3I vector3I2 = minInLod + vector3I1;
      Vector3I vector3I3 = maxInLod + vector3I1;
      MyVoxelDataRequest request = new MyVoxelDataRequest()
      {
        Target = target,
        Offset = writeOffset,
        Lod = lodIndex,
        MinInLod = vector3I2,
        MaxInLod = vector3I3,
        RequestFlags = flags,
        RequestedData = types
      };
      this.m_provider.ReadRange(ref request);
      flags = request.Flags;
    }

    void IMyOctreeLeafNode.ExecuteOperation<TOperator>(
      ref TOperator source,
      ref Vector3I readOffset,
      ref Vector3I min,
      ref Vector3I max)
    {
      throw new NotSupportedException();
    }

    void IMyOctreeLeafNode.OnDataProviderChanged(IMyStorageDataProvider newProvider) => this.m_provider = newProvider;

    void IMyOctreeLeafNode.ReplaceValues(Dictionary<byte, byte> oldToNewValueMap)
    {
    }

    public ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true)
    {
      return this.m_provider.Intersect(box, lod);
    }

    public bool Intersect(ref LineD line, out double startOffset, out double endOffset) => this.m_provider.Intersect(ref line, out startOffset, out endOffset);

    public bool TryGetUniformValue(out byte uniformValue)
    {
      MyStorageData filteredValueBuffer = MyProviderLeaf.FilteredValueBuffer;
      MyVoxelDataRequest request = new MyVoxelDataRequest()
      {
        Target = (MyStorageData) null,
        Offset = Vector3I.Zero,
        Lod = this.m_cell.Lod,
        MinInLod = this.m_cell.CoordInLod,
        MaxInLod = this.m_cell.CoordInLod,
        RequestedData = this.m_dataType.ToFlags()
      };
      this.m_provider.ReadRange(ref request, true);
      if ((request.Flags & MyVoxelRequestFlags.EmptyData) > (MyVoxelRequestFlags) 0)
      {
        uniformValue = this.m_dataType == MyStorageDataTypeEnum.Material ? byte.MaxValue : (byte) 0;
        return true;
      }
      if (this.m_dataType == MyStorageDataTypeEnum.Content && (request.Flags & MyVoxelRequestFlags.FullContent) > (MyVoxelRequestFlags) 0)
      {
        uniformValue = byte.MaxValue;
        return true;
      }
      uniformValue = (byte) 0;
      return false;
    }

    public void Dispose()
    {
    }
  }
}
