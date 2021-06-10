// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.QuantizedBvhTree
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;
using System;
using System.IO;

namespace BulletXNA.BulletCollision
{
  public class QuantizedBvhTree
  {
    public const int MAX_INDICES_PER_NODE = 6;
    private int m_num_nodes;
    private GIM_QUANTIZED_BVH_NODE_ARRAY m_node_array;
    private AABB m_global_bound;
    private IndexedVector3 m_bvhQuantization;

    private static void WriteIndexedVector3(IndexedVector3 vector, BinaryWriter bw)
    {
      bw.Write(vector.X);
      bw.Write(vector.Y);
      bw.Write(vector.Z);
    }

    private static IndexedVector3 ReadIndexedVector3(BinaryReader br) => new IndexedVector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

    private static void WriteUShortVector3(UShortVector3 vector, BinaryWriter bw)
    {
      bw.Write(vector.X);
      bw.Write(vector.Y);
      bw.Write(vector.Z);
    }

    private static UShortVector3 ReadUShortVector3(BinaryReader br) => new UShortVector3()
    {
      X = br.ReadUInt16(),
      Y = br.ReadUInt16(),
      Z = br.ReadUInt16()
    };

    internal unsafe byte[] Save()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter bw = new BinaryWriter((Stream) memoryStream))
        {
          bw.Write(this.m_num_nodes);
          QuantizedBvhTree.WriteIndexedVector3(this.m_global_bound.m_min, bw);
          QuantizedBvhTree.WriteIndexedVector3(this.m_global_bound.m_max, bw);
          QuantizedBvhTree.WriteIndexedVector3(this.m_bvhQuantization, bw);
          for (int index1 = 0; index1 < this.m_num_nodes; ++index1)
          {
            BT_QUANTIZED_BVH_NODE node = this.m_node_array[index1];
            bw.Write((int) node.m_size);
            for (int index2 = 0; index2 < (int) node.m_size; ++index2)
              bw.Write(node.m_escapeIndexOrDataIndex[index2]);
            QuantizedBvhTree.WriteUShortVector3(node.m_quantizedAabbMin, bw);
            QuantizedBvhTree.WriteUShortVector3(node.m_quantizedAabbMax, bw);
          }
          return memoryStream.ToArray();
        }
      }
    }

    internal unsafe void Load(byte[] byteArray)
    {
      using (MemoryStream memoryStream = new MemoryStream(byteArray))
      {
        using (BinaryReader br = new BinaryReader((Stream) memoryStream))
        {
          this.m_num_nodes = br.ReadInt32();
          IndexedVector3 min = QuantizedBvhTree.ReadIndexedVector3(br);
          IndexedVector3 max = QuantizedBvhTree.ReadIndexedVector3(br);
          this.m_global_bound = new AABB(ref min, ref max);
          this.m_bvhQuantization = QuantizedBvhTree.ReadIndexedVector3(br);
          this.m_node_array = new GIM_QUANTIZED_BVH_NODE_ARRAY(this.m_num_nodes);
          for (int index1 = 0; index1 < this.m_num_nodes; ++index1)
          {
            int num = br.ReadInt32();
            if (num > 6)
              throw new Exception();
            BT_QUANTIZED_BVH_NODE quantizedBvhNode = new BT_QUANTIZED_BVH_NODE();
            quantizedBvhNode.m_size = (byte) num;
            for (int index2 = 0; index2 < num; ++index2)
              quantizedBvhNode.m_escapeIndexOrDataIndex[index2] = br.ReadInt32();
            quantizedBvhNode.m_quantizedAabbMin = QuantizedBvhTree.ReadUShortVector3(br);
            quantizedBvhNode.m_quantizedAabbMax = QuantizedBvhTree.ReadUShortVector3(br);
            this.m_node_array.Add(quantizedBvhNode);
          }
        }
      }
    }

    private void CalcQuantization(GIM_BVH_DATA_ARRAY primitive_boxes) => this.CalcQuantization(primitive_boxes, 1f);

    private void CalcQuantization(GIM_BVH_DATA_ARRAY primitive_boxes, float boundMargin)
    {
      AABB aabb = new AABB();
      aabb.Invalidate();
      int count = primitive_boxes.Count;
      for (int index = 0; index < count; ++index)
        aabb.Merge(ref primitive_boxes.GetRawArray()[index].m_bound);
      GImpactQuantization.CalcQuantizationParameters(out this.m_global_bound.m_min, out this.m_global_bound.m_max, out this.m_bvhQuantization, ref aabb.m_min, ref aabb.m_max, boundMargin);
    }

    private int SortAndCalcSplittingIndex(
      GIM_BVH_DATA_ARRAY primitive_boxes,
      int startIndex,
      int endIndex,
      int splitAxis)
    {
      int index1 = startIndex;
      int num1 = endIndex - startIndex;
      IndexedVector3 zero = IndexedVector3.Zero;
      for (int index = startIndex; index < endIndex; ++index)
      {
        IndexedVector3 indexedVector3 = 0.5f * (primitive_boxes[index].m_bound.m_max + primitive_boxes[index].m_bound.m_min);
        zero += indexedVector3;
      }
      float num2 = (zero * (1f / (float) num1))[splitAxis];
      for (int index = startIndex; index < endIndex; ++index)
      {
        if ((double) (0.5f * (primitive_boxes[index].m_bound.m_max + primitive_boxes[index].m_bound.m_min))[splitAxis] > (double) num2)
        {
          primitive_boxes.Swap(index, index1);
          ++index1;
        }
      }
      int num3 = num1 / 3;
      if (index1 <= startIndex + num3 || index1 >= endIndex - 1 - num3)
        index1 = startIndex + (num1 >> 1);
      return index1;
    }

    private int CalcSplittingAxis(GIM_BVH_DATA_ARRAY primitive_boxes, int startIndex, int endIndex)
    {
      IndexedVector3 zero1 = IndexedVector3.Zero;
      IndexedVector3 zero2 = IndexedVector3.Zero;
      int num = endIndex - startIndex;
      for (int index = startIndex; index < endIndex; ++index)
      {
        IndexedVector3 indexedVector3 = 0.5f * (primitive_boxes[index].m_bound.m_max + primitive_boxes[index].m_bound.m_min);
        zero1 += indexedVector3;
      }
      IndexedVector3 indexedVector3_1 = zero1 * (1f / (float) num);
      for (int index = startIndex; index < endIndex; ++index)
      {
        IndexedVector3 indexedVector3_2 = 0.5f * (primitive_boxes[index].m_bound.m_max + primitive_boxes[index].m_bound.m_min) - indexedVector3_1;
        IndexedVector3 indexedVector3_3 = indexedVector3_2 * indexedVector3_2;
        zero2 += indexedVector3_3;
      }
      IndexedVector3 a = zero2 * (float) (1.0 / ((double) num - 1.0));
      return MathUtil.MaxAxis(ref a);
    }

    private unsafe void BuildSubTree(
      GIM_BVH_DATA_ARRAY primitive_boxes,
      int startIndex,
      int endIndex)
    {
      int numNodes = this.m_num_nodes;
      ++this.m_num_nodes;
      if (endIndex - startIndex <= 6)
      {
        int num = endIndex - startIndex;
        int length = num;
        // ISSUE: untyped stack allocation
        Span<int> indices = new Span<int>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length) * 4)), length);
        AABB bound = new AABB();
        bound.Invalidate();
        for (int index = 0; index < num; ++index)
        {
          indices[index] = primitive_boxes[startIndex + index].m_data;
          bound.Merge(primitive_boxes.GetRawArray()[startIndex + index].m_bound);
        }
        this.SetNodeBound(numNodes, ref bound);
        this.m_node_array.GetRawArray()[numNodes].SetDataIndices(indices);
      }
      else
      {
        int splitAxis = this.CalcSplittingAxis(primitive_boxes, startIndex, endIndex);
        int num = this.SortAndCalcSplittingIndex(primitive_boxes, startIndex, endIndex, splitAxis);
        AABB bound = new AABB();
        bound.Invalidate();
        for (int index = startIndex; index < endIndex; ++index)
          bound.Merge(ref primitive_boxes.GetRawArray()[index].m_bound);
        this.SetNodeBound(numNodes, ref bound);
        this.BuildSubTree(primitive_boxes, startIndex, num);
        this.BuildSubTree(primitive_boxes, num, endIndex);
        this.m_node_array.GetRawArray()[numNodes].SetEscapeIndex(this.m_num_nodes - numNodes);
      }
    }

    internal QuantizedBvhTree()
    {
      this.m_num_nodes = 0;
      this.m_node_array = new GIM_QUANTIZED_BVH_NODE_ARRAY();
    }

    internal void BuildTree(GIM_BVH_DATA_ARRAY primitive_boxes)
    {
      this.CalcQuantization(primitive_boxes);
      this.m_num_nodes = 0;
      this.m_node_array.Resize(primitive_boxes.Count * 2);
      this.BuildSubTree(primitive_boxes, 0, primitive_boxes.Count);
    }

    internal void QuantizePoint(out UShortVector3 quantizedpoint, ref IndexedVector3 point) => GImpactQuantization.QuantizeClamp(out quantizedpoint, ref point, ref this.m_global_bound.m_min, ref this.m_global_bound.m_max, ref this.m_bvhQuantization);

    internal bool TestQuantizedBoxOverlap(
      int node_index,
      ref UShortVector3 quantizedMin,
      ref UShortVector3 quantizedMax)
    {
      return this.m_node_array[node_index].TestQuantizedBoxOverlapp(ref quantizedMin, ref quantizedMax);
    }

    internal int GetNodeCount() => this.m_num_nodes;

    internal bool IsLeafNode(int nodeindex) => this.m_node_array[nodeindex].IsLeafNode();

    internal BT_QUANTIZED_BVH_NODE GetNode(int nodeindex) => this.m_node_array[nodeindex];

    internal void GetNodeBound(int nodeindex, out AABB bound)
    {
      bound.m_min = GImpactQuantization.Unquantize(ref this.m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMin, ref this.m_global_bound.m_min, ref this.m_bvhQuantization);
      bound.m_max = GImpactQuantization.Unquantize(ref this.m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMax, ref this.m_global_bound.m_min, ref this.m_bvhQuantization);
    }

    private void SetNodeBound(int nodeindex, ref AABB bound)
    {
      GImpactQuantization.QuantizeClamp(out this.m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMin, ref bound.m_min, ref this.m_global_bound.m_min, ref this.m_global_bound.m_max, ref this.m_bvhQuantization);
      GImpactQuantization.QuantizeClamp(out this.m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMax, ref bound.m_max, ref this.m_global_bound.m_min, ref this.m_global_bound.m_max, ref this.m_bvhQuantization);
    }

    internal int GetEscapeNodeIndex(int nodeindex) => this.m_node_array[nodeindex].GetEscapeIndex();
  }
}
