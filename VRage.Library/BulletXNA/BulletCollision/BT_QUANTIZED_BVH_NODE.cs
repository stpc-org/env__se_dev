// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.BT_QUANTIZED_BVH_NODE
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;
using System;

namespace BulletXNA.BulletCollision
{
  public struct BT_QUANTIZED_BVH_NODE
  {
    public UShortVector3 m_quantizedAabbMin;
    public UShortVector3 m_quantizedAabbMax;
    public byte m_size;
    public unsafe fixed int m_escapeIndexOrDataIndex[6];

    public unsafe bool IsLeafNode() => this.m_escapeIndexOrDataIndex.FixedElementField >= 0;

    public unsafe int GetEscapeIndex() => -this.m_escapeIndexOrDataIndex.FixedElementField;

    public unsafe void SetEscapeIndex(int index)
    {
      this.m_size = (byte) 1;
      this.m_escapeIndexOrDataIndex[0] = -index;
    }

    public unsafe void SetDataIndices(Span<int> indices)
    {
      this.m_size = (byte) indices.Length;
      for (int index = 0; index < (int) this.m_size; ++index)
        this.m_escapeIndexOrDataIndex[index] = indices[index];
    }

    public bool TestQuantizedBoxOverlapp(
      ref UShortVector3 quantizedMin,
      ref UShortVector3 quantizedMax)
    {
      return (int) this.m_quantizedAabbMin.X <= (int) quantizedMax.X && (int) this.m_quantizedAabbMax.X >= (int) quantizedMin.X && ((int) this.m_quantizedAabbMin.Y <= (int) quantizedMax.Y && (int) this.m_quantizedAabbMax.Y >= (int) quantizedMin.Y) && ((int) this.m_quantizedAabbMin.Z <= (int) quantizedMax.Z && (int) this.m_quantizedAabbMax.Z >= (int) quantizedMin.Z);
    }
  }
}
