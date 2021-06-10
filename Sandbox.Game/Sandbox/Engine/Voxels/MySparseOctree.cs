// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MySparseOctree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MySparseOctree : IDisposable
  {
    private readonly NativeDictionary<uint, MyOctreeNode> m_nodes = new NativeDictionary<uint, MyOctreeNode>();
    private MyOctreeNode.FilterFunction m_nodeFilter;
    private int m_treeHeight;
    private int m_treeWidth;
    private byte m_defaultContent;

    public int TreeWidth => this.m_treeWidth;

    public unsafe bool IsAllSame
    {
      get
      {
        MyOctreeNode node = this.m_nodes[this.ComputeRootKey()];
        return !node.HasChildren && MyOctreeNode.AllDataSame(node.Data);
      }
    }

    public MySparseOctree(int height, MyOctreeNode.FilterFunction nodeFilter, byte defaultContent = 0)
    {
      this.m_treeHeight = height;
      this.m_treeWidth = 1 << height;
      this.m_defaultContent = defaultContent;
      this.m_nodeFilter = nodeFilter;
    }

    public void Build<TDataEnum>(TDataEnum data) where TDataEnum : struct, IEnumerator<byte>
    {
      this.m_nodes.Clear();
      MySparseOctree.StackData<TDataEnum> stack;
      stack.Data = data;
      stack.Cell = new MyCellCoord(this.m_treeHeight - 1, ref Vector3I.Zero);
      stack.DefaultNode = new MyOctreeNode(this.m_defaultContent);
      MyOctreeNode builtNode;
      this.BuildNode<TDataEnum>(ref stack, out builtNode);
      this.m_nodes[this.ComputeRootKey()] = builtNode;
    }

    private unsafe void BuildNode<TDataEnum>(
      ref MySparseOctree.StackData<TDataEnum> stack,
      out MyOctreeNode builtNode)
      where TDataEnum : struct, IEnumerator<byte>
    {
      MyOctreeNode defaultNode = stack.DefaultNode;
      if (stack.Cell.Lod == 0)
      {
        for (int index = 0; index < 8; ++index)
        {
          stack.Data.MoveNext();
          defaultNode.Data[index] = stack.Data.Current;
        }
      }
      else
      {
        --stack.Cell.Lod;
        Vector3I coordInLod = stack.Cell.CoordInLod;
        Vector3I vector3I = coordInLod << 1;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          this.ComputeChildCoord(index, out relativeCoord);
          stack.Cell.CoordInLod = vector3I + relativeCoord;
          MyOctreeNode builtNode1;
          this.BuildNode<TDataEnum>(ref stack, out builtNode1);
          if (!builtNode1.HasChildren && MyOctreeNode.AllDataSame(builtNode1.Data))
          {
            defaultNode.SetChild(index, false);
            // ISSUE: reference to a compiler-generated field
            defaultNode.Data[index] = builtNode1.Data.FixedElementField;
          }
          else
          {
            defaultNode.SetChild(index, true);
            defaultNode.Data[index] = this.m_nodeFilter(builtNode1.Data, stack.Cell.Lod);
            this.m_nodes.Add(stack.Cell.PackId32(), builtNode1);
          }
        }
        ++stack.Cell.Lod;
        stack.Cell.CoordInLod = coordInLod;
      }
      builtNode = defaultNode;
    }

    public unsafe void Build(byte singleValue)
    {
      this.m_nodes.Clear();
      MyOctreeNode myOctreeNode;
      myOctreeNode.ChildMask = (byte) 0;
      for (int index = 0; index < 8; ++index)
        myOctreeNode.Data[index] = singleValue;
      this.m_nodes[this.ComputeRootKey()] = myOctreeNode;
    }

    internal unsafe byte GetFilteredValue() => this.m_nodeFilter(this.m_nodes[this.ComputeRootKey()].Data, this.m_treeHeight);

    internal void ReadRange(
      MyStorageData target,
      MyStorageDataTypeEnum type,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod)
    {
      MyStorageReadOperator target1 = new MyStorageReadOperator(target);
      this.ReadRange<MyStorageReadOperator>(ref target1, type, ref writeOffset, lodIndex, ref minInLod, ref maxInLod);
    }

    internal unsafe void ReadRange<TOperator>(
      ref TOperator target,
      MyStorageDataTypeEnum type,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod)
      where TOperator : struct, IVoxelOperator
    {
      try
      {
        int num1 = 0;
        MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(this.m_treeHeight)];
        MyCellCoord myCellCoord1 = new MyCellCoord(this.m_treeHeight - 1, ref Vector3I.Zero);
        MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
        int num2 = num1;
        int num3 = num2 + 1;
        IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
        *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
        while (num3 > 0)
        {
          MyCellCoord myCellCoord2 = myCellCoordPtr1[--num3];
          MyOctreeNode node = this.m_nodes[myCellCoord2.PackId32()];
          int num5 = myCellCoord2.Lod - lodIndex;
          Vector3I min = minInLod >> num5;
          Vector3I max = maxInLod >> num5;
          Vector3I vector3I1 = myCellCoord2.CoordInLod << 1;
          min -= vector3I1;
          max -= vector3I1;
          for (int index = 0; index < 8; ++index)
          {
            Vector3I relativeCoord;
            this.ComputeChildCoord(index, out relativeCoord);
            if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
            {
              if (lodIndex < myCellCoord2.Lod && node.HasChild(index))
              {
                myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord2.Lod - 1, vector3I1 + relativeCoord);
              }
              else
              {
                byte inOutContent = node.Data[index];
                Vector3I vector3I2 = vector3I1 + relativeCoord;
                if (num5 == 0)
                {
                  Vector3I position = writeOffset + vector3I2 - minInLod;
                  target.Op(ref position, type, ref inOutContent);
                }
                else
                {
                  Vector3I result1 = vector3I2 << num5;
                  Vector3I result2 = result1 + (1 << num5) - 1;
                  Vector3I.Max(ref result1, ref minInLod, out result1);
                  Vector3I.Min(ref result2, ref maxInLod, out result2);
                  for (int z = result1.Z; z <= result2.Z; ++z)
                  {
                    for (int y = result1.Y; y <= result2.Y; ++y)
                    {
                      for (int x = result1.X; x <= result2.X; ++x)
                      {
                        Vector3I position = writeOffset;
                        position.X += x - minInLod.X;
                        position.Y += y - minInLod.Y;
                        position.Z += z - minInLod.Z;
                        target.Op(ref position, type, ref inOutContent);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
      }
    }

    internal void ExecuteOperation<TOperator>(
      ref TOperator source,
      MyStorageDataTypeEnum type,
      ref Vector3I readOffset,
      ref Vector3I min,
      ref Vector3I max)
      where TOperator : struct, IVoxelOperator
    {
      if (source.Flags == VoxelOperatorFlags.Read)
        this.ReadRange<TOperator>(ref source, type, ref readOffset, 0, ref min, ref max);
      else
        this.WriteRange<TOperator>(new MyCellCoord(this.m_treeHeight - 1, Vector3I.Zero), this.m_defaultContent, ref source, type, ref readOffset, ref min, ref max);
    }

    private unsafe void WriteRange<TOperator>(
      MyCellCoord cell,
      byte defaultData,
      ref TOperator source,
      MyStorageDataTypeEnum type,
      ref Vector3I readOffset,
      ref Vector3I min,
      ref Vector3I max)
      where TOperator : struct, IVoxelOperator
    {
      uint key1 = cell.PackId32();
      MyOctreeNode myOctreeNode;
      if (!this.m_nodes.TryGetValue(key1, out myOctreeNode))
      {
        for (int index = 0; index < 8; ++index)
          myOctreeNode.Data[index] = defaultData;
      }
      if (cell.Lod == 0)
      {
        Vector3I vector3I = cell.CoordInLod << 1;
        for (int childIdx = 0; childIdx < 8; ++childIdx)
        {
          Vector3I relativeCoord;
          this.ComputeChildCoord(childIdx, out relativeCoord);
          Vector3I position = vector3I + relativeCoord;
          if (position.IsInsideInclusiveEnd(ref min, ref max))
          {
            position -= min;
            position += readOffset;
            source.Op(ref position, type, ref myOctreeNode.Data[childIdx]);
          }
        }
        this.m_nodes[key1] = myOctreeNode;
      }
      else
      {
        Vector3I vector3I = cell.CoordInLod << 1;
        Vector3I min1 = (min >> cell.Lod) - vector3I;
        Vector3I max1 = (max >> cell.Lod) - vector3I;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          this.ComputeChildCoord(index, out relativeCoord);
          if (relativeCoord.IsInsideInclusiveEnd(ref min1, ref max1))
          {
            MyCellCoord cell1 = new MyCellCoord(cell.Lod - 1, vector3I + relativeCoord);
            this.WriteRange<TOperator>(cell1, myOctreeNode.Data[index], ref source, type, ref readOffset, ref min, ref max);
            uint key2 = cell1.PackId32();
            MyOctreeNode node = this.m_nodes[key2];
            if (!node.HasChildren && MyOctreeNode.AllDataSame(node.Data))
            {
              myOctreeNode.SetChild(index, false);
              // ISSUE: reference to a compiler-generated field
              myOctreeNode.Data[index] = node.Data.FixedElementField;
              this.m_nodes.Remove(key2);
            }
            else
            {
              myOctreeNode.SetChild(index, true);
              myOctreeNode.Data[index] = this.m_nodeFilter(node.Data, cell.Lod);
            }
          }
        }
        this.m_nodes[key1] = myOctreeNode;
      }
    }

    [Conditional("DEBUG")]
    private void CheckData<T>(T data) where T : struct, IEnumerator<byte>
    {
    }

    [Conditional("DEBUG")]
    private void CheckData<T>(ref T data, MyCellCoord cell) where T : struct, IEnumerator<byte>
    {
      MyOctreeNode node = this.m_nodes[cell.PackId32()];
      for (int index = 0; index < 8; ++index)
      {
        if (node.HasChild(index))
        {
          this.ComputeChildCoord(index, out Vector3I _);
        }
        else
        {
          int num1 = 1 << 3 * cell.Lod;
          int num2 = 0;
          while (num2 < num1)
            ++num2;
        }
      }
    }

    private uint ComputeRootKey() => new MyCellCoord(this.m_treeHeight - 1, ref Vector3I.Zero).PackId32();

    private void ComputeChildCoord(int childIdx, out Vector3I relativeCoord)
    {
      relativeCoord.X = childIdx & 1;
      relativeCoord.Y = childIdx >> 1 & 1;
      relativeCoord.Z = childIdx >> 2 & 1;
    }

    internal void DebugDraw(
      IMyDebugDrawBatchAabb batch,
      Vector3 worldPos,
      MyVoxelDebugDrawMode mode)
    {
      switch (mode)
      {
        case MyVoxelDebugDrawMode.Content_MicroNodes:
          using (NativeDictionary<uint, MyOctreeNode>.Enumerator enumerator = this.m_nodes.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<uint, MyOctreeNode> current = enumerator.Current;
              MyCellCoord myCellCoord = new MyCellCoord();
              myCellCoord.SetUnpack(current.Key);
              MyOctreeNode myOctreeNode = current.Value;
              for (int index = 0; index < 8; ++index)
              {
                if (!myOctreeNode.HasChild(index) || myCellCoord.Lod == 0)
                {
                  Vector3I relativeCoord;
                  this.ComputeChildCoord(index, out relativeCoord);
                  Vector3I vector3I = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
                  BoundingBoxD aabb;
                  aabb.Min = (Vector3D) (worldPos + vector3I * 1f);
                  aabb.Max = aabb.Min + 1f * (float) (1 << myCellCoord.Lod);
                  if (myOctreeNode.GetData(index) != (byte) 0)
                    batch.Add(ref aabb);
                }
              }
            }
            break;
          }
        case MyVoxelDebugDrawMode.Content_MicroNodesScaled:
          using (NativeDictionary<uint, MyOctreeNode>.Enumerator enumerator = this.m_nodes.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<uint, MyOctreeNode> current = enumerator.Current;
              MyCellCoord myCellCoord = new MyCellCoord();
              myCellCoord.SetUnpack(current.Key);
              MyOctreeNode myOctreeNode = current.Value;
              for (int index = 0; index < 8; ++index)
              {
                if (!myOctreeNode.HasChild(index))
                {
                  Vector3I relativeCoord;
                  this.ComputeChildCoord(index, out relativeCoord);
                  float num1 = (float) myOctreeNode.GetData(index) / (float) byte.MaxValue;
                  if ((double) num1 != 0.0)
                  {
                    float num2 = (float) Math.Pow((double) num1 * 1.0, 0.3333);
                    Vector3I vector3I = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
                    float num3 = 1f * (float) (1 << myCellCoord.Lod);
                    Vector3 vector3 = worldPos + vector3I * 1f + 0.5f * num3;
                    BoundingBoxD aabb;
                    aabb.Min = (Vector3D) (vector3 - 0.5f * num2 * num3);
                    aabb.Max = (Vector3D) (vector3 + 0.5f * num2 * num3);
                    batch.Add(ref aabb);
                  }
                }
              }
            }
            break;
          }
      }
    }

    public int SerializedSize => 5 + this.m_nodes.Count * 13;

    internal unsafe void WriteTo(Stream stream)
    {
      stream.WriteNoAlloc(this.m_treeHeight);
      stream.WriteNoAlloc(this.m_defaultContent);
      foreach (KeyValuePair<uint, MyOctreeNode> node in this.m_nodes)
      {
        stream.WriteNoAlloc(node.Key);
        MyOctreeNode myOctreeNode = node.Value;
        stream.WriteNoAlloc(myOctreeNode.ChildMask);
        stream.WriteNoAlloc(myOctreeNode.Data, 0, 8);
      }
    }

    internal unsafe void ReadFrom(MyOctreeStorage.ChunkHeader header, Stream stream)
    {
      this.m_treeHeight = stream.ReadInt32();
      this.m_treeWidth = 1 << this.m_treeHeight;
      this.m_defaultContent = stream.ReadByteNoAlloc();
      header.Size -= 5;
      int num = header.Size / 13;
      this.m_nodes.Clear();
      for (int index = 0; index < num; ++index)
      {
        uint key = stream.ReadUInt32();
        MyOctreeNode myOctreeNode;
        myOctreeNode.ChildMask = stream.ReadByteNoAlloc();
        stream.ReadNoAlloc(myOctreeNode.Data, 0, 8);
        this.m_nodes.Add(key, myOctreeNode);
      }
    }

    internal static int EstimateStackSize(int treeHeight) => (treeHeight - 1) * 7 + 8;

    public void ReplaceValues(Dictionary<byte, byte> oldToNewValueMap) => MySparseOctree.ReplaceValues<uint>((IDictionary<uint, MyOctreeNode>) this.m_nodes, oldToNewValueMap);

    public static unsafe void ReplaceValues<TKey>(
      IDictionary<TKey, MyOctreeNode> nodeCollection,
      Dictionary<byte, byte> oldToNewValueMap)
      where TKey : unmanaged
    {
      foreach (KeyValuePair<TKey, MyOctreeNode> keyValuePair in nodeCollection.ToArray<KeyValuePair<TKey, MyOctreeNode>>())
      {
        MyOctreeNode myOctreeNode = keyValuePair.Value;
        for (int index = 0; index < 8; ++index)
        {
          byte num;
          if (oldToNewValueMap.TryGetValue(myOctreeNode.Data[index], out num))
            myOctreeNode.Data[index] = num;
        }
        nodeCollection[keyValuePair.Key] = myOctreeNode;
      }
    }

    internal unsafe ContainmentType Intersect(
      ref BoundingBoxI box,
      int lod,
      bool exhaustiveContainmentCheck = true)
    {
      int num1 = 0;
      MyCellCoord* myCellCoordPtr1 = stackalloc MyCellCoord[MySparseOctree.EstimateStackSize(this.m_treeHeight)];
      MyCellCoord myCellCoord1 = new MyCellCoord(this.m_treeHeight - 1, ref Vector3I.Zero);
      MyCellCoord* myCellCoordPtr2 = myCellCoordPtr1;
      int num2 = num1;
      int num3 = num2 + 1;
      IntPtr num4 = (IntPtr) num2 * sizeof (MyCellCoord);
      *(MyCellCoord*) ((IntPtr) myCellCoordPtr2 + num4) = myCellCoord1;
      Vector3I min1 = box.Min;
      Vector3I max1 = box.Max;
      ContainmentType containmentType = ContainmentType.Disjoint;
      while (num3 > 0)
      {
        MyCellCoord myCellCoord2 = myCellCoordPtr1[--num3];
        MyOctreeNode node = this.m_nodes[myCellCoord2.PackId32()];
        int lod1 = myCellCoord2.Lod;
        Vector3I min2 = min1 >> lod1;
        Vector3I max2 = max1 >> lod1;
        Vector3I vector3I = myCellCoord2.CoordInLod << 1;
        min2 -= vector3I;
        max2 -= vector3I;
        for (int index = 0; index < 8; ++index)
        {
          Vector3I relativeCoord;
          this.ComputeChildCoord(index, out relativeCoord);
          if (relativeCoord.IsInsideInclusiveEnd(ref min2, ref max2))
          {
            if (myCellCoord2.Lod > 0 && node.HasChild(index))
            {
              myCellCoordPtr1[num3++] = new MyCellCoord(myCellCoord2.Lod - 1, vector3I + relativeCoord);
            }
            else
            {
              byte num5 = node.Data[index];
              if (lod1 == 0)
              {
                if (num5 != (byte) 0)
                  return ContainmentType.Intersects;
              }
              else
              {
                BoundingBoxI box1;
                box1.Min = vector3I + relativeCoord;
                box1.Min <<= lod1;
                box1.Max = box1.Min + (1 << lod1) - 1;
                Vector3I.Max(ref box1.Min, ref min1, out box1.Min);
                Vector3I.Min(ref box1.Max, ref max1, out box1.Max);
                bool result;
                box1.Intersects(ref box1, out result);
                if (result)
                  return ContainmentType.Intersects;
              }
            }
          }
        }
      }
      return containmentType;
    }

    internal bool Intersect(ref LineD line, out double startOffset, out double endOffset)
    {
      startOffset = 0.0;
      endOffset = 1.0;
      return true;
    }

    public void Dispose() => this.m_nodes.Dispose();

    private struct StackData<T> where T : struct, IEnumerator<byte>
    {
      public T Data;
      public MyCellCoord Cell;
      public MyOctreeNode DefaultNode;
    }
  }
}
