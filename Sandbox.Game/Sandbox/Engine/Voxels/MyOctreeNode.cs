// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyOctreeNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sandbox.Engine.Voxels
{
  internal struct MyOctreeNode
  {
    public const int CHILD_COUNT = 8;
    public const int SERIALIZED_SIZE = 9;
    [ThreadStatic]
    private static Dictionary<byte, int> Histogram;
    public static readonly MyOctreeNode.FilterFunction ContentFilter = new MyOctreeNode.FilterFunction(MyOctreeNode.SignedDistanceFilterInternal);
    public static readonly MyOctreeNode.FilterFunction MaterialFilter = new MyOctreeNode.FilterFunction(MyOctreeNode.HistogramFilterInternal);
    public byte ChildMask;
    public unsafe fixed byte Data[8];

    public MyOctreeNode(byte allContent)
    {
      this.ChildMask = (byte) 0;
      this.SetAllData(allContent);
    }

    public bool HasChildren => this.ChildMask > (byte) 0;

    public void ClearChildren() => this.ChildMask = (byte) 0;

    public void SetChildren() => this.ChildMask = byte.MaxValue;

    public bool HasChild(int childIndex) => ((uint) this.ChildMask & (uint) (1 << childIndex)) > 0U;

    public void SetChild(int childIndex, bool childPresent)
    {
      int num = 1 << childIndex;
      if (childPresent)
        this.ChildMask |= (byte) num;
      else
        this.ChildMask &= (byte) ~num;
    }

    public unsafe void SetAllData(byte value)
    {
      fixed (byte* dst = this.Data)
        MyOctreeNode.SetAllData(dst, value);
    }

    public static unsafe void SetAllData(byte* dst, byte value)
    {
      for (int index = 0; index < 8; ++index)
        dst[index] = value;
    }

    public unsafe void SetData(int childIndex, byte data)
    {
      fixed (byte* numPtr = this.Data)
        numPtr[childIndex] = data;
    }

    public unsafe byte GetData(int cellIndex)
    {
      fixed (byte* numPtr = this.Data)
        return numPtr[cellIndex];
    }

    public unsafe byte ComputeFilteredValue(MyOctreeNode.FilterFunction filter, int lod)
    {
      fixed (byte* pData = this.Data)
        return filter(pData, lod);
    }

    public unsafe bool AllDataSame()
    {
      fixed (byte* pData = this.Data)
        return MyOctreeNode.AllDataSame(pData);
    }

    public static unsafe bool AllDataSame(byte* pData)
    {
      byte num = *pData;
      for (int index = 1; index < 8; ++index)
      {
        if ((int) pData[index] != (int) num)
          return false;
      }
      return true;
    }

    public unsafe bool AllDataSame(byte value)
    {
      fixed (byte* pData = this.Data)
        return MyOctreeNode.AllDataSame(pData, value);
    }

    public static unsafe bool AllDataSame(byte* pData, byte value)
    {
      for (int index = 1; index < 8; ++index)
      {
        if ((int) pData[index] != (int) value)
          return false;
      }
      return true;
    }

    public override unsafe string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(20);
      stringBuilder.Append("0x").Append(this.ChildMask.ToString("X2")).Append(": ");
      fixed (byte* numPtr = this.Data)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (index != 0)
            stringBuilder.Append(", ");
          stringBuilder.Append(numPtr[index]);
        }
      }
      return stringBuilder.ToString();
    }

    [Conditional("DEBUG")]
    private void AssertChildIndex(int cellIndex)
    {
    }

    private static unsafe byte AverageFilter(byte* pData, int lod)
    {
      int num = 0;
      for (int index = 0; index < 8; ++index)
        num += (int) pData[index];
      return (byte) (num / 8);
    }

    private static float ToSignedDistance(byte value) => (float) ((double) value / (double) byte.MaxValue * 2.0 - 1.0);

    private static byte FromSignedDistance(float value) => (byte) (((double) value * 0.5 + 0.5) * (double) byte.MaxValue + 0.5);

    private static unsafe byte SignedDistanceFilterInternal(byte* pData, int lod)
    {
      float signedDistance = MyOctreeNode.ToSignedDistance(*pData);
      if ((double) MyOctreeNode.ToSignedDistance(MyOctreeNode.AverageValueFilterInternal(pData, lod)) != (double) signedDistance || (double) signedDistance != 1.0 && (double) signedDistance != -1.0)
        signedDistance *= 0.5f;
      return MyOctreeNode.FromSignedDistance(signedDistance);
    }

    private static unsafe byte AverageValueFilterInternal(byte* pData, int lod)
    {
      float num1 = 0.0f;
      for (int index = 0; index < 8; ++index)
        num1 += MyOctreeNode.ToSignedDistance(pData[index]);
      float num2 = num1 / 8f;
      if ((double) num2 != 1.0 && (double) num2 != -1.0)
        num2 *= 0.5f;
      return MyOctreeNode.FromSignedDistance(num2);
    }

    private static unsafe byte IsoSurfaceFilterInternal(byte* pData, int lod)
    {
      byte num1 = 0;
      byte num2 = byte.MaxValue;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < 8; ++index)
      {
        byte num5 = pData[index];
        if (num5 < (byte) 127)
        {
          ++num4;
          if ((int) num5 > (int) num1)
            num1 = num5;
        }
        else
        {
          ++num3;
          if ((int) num5 < (int) num2)
            num2 = num5;
        }
      }
      float num6 = (float) ((num4 > num3 ? (double) num1 : (double) num2) / (double) byte.MaxValue * 2.0 - 1.0);
      if ((double) num6 != 1.0 && (double) num6 != -1.0)
        num6 *= 0.5f;
      return (byte) (((double) num6 * 0.5 + 0.5) * (double) byte.MaxValue);
    }

    private static unsafe byte HistogramFilterInternal(byte* pdata, int lod)
    {
      if (MyOctreeNode.Histogram == null)
        MyOctreeNode.Histogram = new Dictionary<byte, int>(8);
      for (int index = 0; index < 8; ++index)
      {
        byte key = pdata[index];
        if (key != byte.MaxValue)
        {
          int num;
          MyOctreeNode.Histogram.TryGetValue(key, out num);
          ++num;
          MyOctreeNode.Histogram[key] = num;
        }
      }
      if (MyOctreeNode.Histogram.Count == 0)
        return byte.MaxValue;
      byte num1 = 0;
      int num2 = 0;
      foreach (KeyValuePair<byte, int> keyValuePair in MyOctreeNode.Histogram)
      {
        if (keyValuePair.Value > num2)
        {
          num2 = keyValuePair.Value;
          num1 = keyValuePair.Key;
        }
      }
      MyOctreeNode.Histogram.Clear();
      return num1;
    }

    public unsafe bool AnyAboveIso()
    {
      fixed (byte* numPtr = this.Data)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (numPtr[index] > (byte) 127)
            return true;
        }
      }
      return false;
    }

    public unsafe delegate byte FilterFunction(byte* pData, int lod);
  }
}
