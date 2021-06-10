// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.GImpactQuantizedBvh
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
  public class GImpactQuantizedBvh
  {
    private QuantizedBvhTree m_box_tree;
    private IPrimitiveManagerBase m_primitive_manager;
    private int m_size;

    public byte[] Save() => this.m_box_tree.Save();

    public int Size => this.m_size;

    public void Load(byte[] byteArray)
    {
      this.m_box_tree.Load(byteArray);
      this.m_size = byteArray.Length;
    }

    public GImpactQuantizedBvh() => this.m_box_tree = new QuantizedBvhTree();

    public GImpactQuantizedBvh(IPrimitiveManagerBase primitive_manager)
    {
      this.m_primitive_manager = primitive_manager;
      this.m_box_tree = new QuantizedBvhTree();
    }

    public void BuildSet()
    {
      int primitiveCount = this.m_primitive_manager.GetPrimitiveCount();
      GIM_BVH_DATA_ARRAY primitive_boxes = new GIM_BVH_DATA_ARRAY(primitiveCount);
      primitive_boxes.Resize(primitiveCount);
      GIM_BVH_DATA[] rawArray = primitive_boxes.GetRawArray();
      for (int prim_index = 0; prim_index < primitiveCount; ++prim_index)
      {
        this.m_primitive_manager.GetPrimitiveBox(prim_index, out rawArray[prim_index].m_bound);
        rawArray[prim_index].m_data = prim_index;
      }
      this.m_box_tree.BuildTree(primitive_boxes);
    }

    public unsafe bool BoxQuery(ref AABB box, ProcessHandler handler)
    {
      int num = 0;
      int nodeCount = this.GetNodeCount();
      UShortVector3 quantizedpoint1;
      this.m_box_tree.QuantizePoint(out quantizedpoint1, ref box.m_min);
      UShortVector3 quantizedpoint2;
      this.m_box_tree.QuantizePoint(out quantizedpoint2, ref box.m_max);
      while (num < nodeCount)
      {
        bool flag1 = this.m_box_tree.TestQuantizedBoxOverlap(num, ref quantizedpoint1, ref quantizedpoint2);
        bool flag2 = this.IsLeafNode(num);
        if (flag2 & flag1)
        {
          BT_QUANTIZED_BVH_NODE node = this.GetNode(num);
          for (int index = 0; index < (int) node.m_size; ++index)
          {
            if (handler(node.m_escapeIndexOrDataIndex[index]))
              return true;
          }
        }
        if (flag1 | flag2)
          ++num;
        else
          num += this.GetEscapeNodeIndex(num);
      }
      return false;
    }

    public unsafe bool RayQueryClosest(
      ref IndexedVector3 ray_dir,
      ref IndexedVector3 ray_origin,
      ProcessCollisionHandler handler)
    {
      int nodeindex = 0;
      int nodeCount = this.GetNodeCount();
      float num = float.PositiveInfinity;
      while (nodeindex < nodeCount)
      {
        AABB bound;
        this.GetNodeBound(nodeindex, out bound);
        float? nullable1 = bound.CollideRayDistance(ref ray_origin, ref ray_dir);
        bool flag1 = this.IsLeafNode(nodeindex);
        bool flag2 = nullable1.HasValue && (double) nullable1.Value < (double) num;
        if (flag2 & flag1)
        {
          BT_QUANTIZED_BVH_NODE node = this.GetNode(nodeindex);
          for (int index = 0; index < (int) node.m_size; ++index)
          {
            float? nullable2 = handler(node.m_escapeIndexOrDataIndex[index]);
            if (nullable2.HasValue && (double) nullable2.Value < (double) num)
              num = nullable2.Value;
          }
        }
        if (flag2 | flag1)
          ++nodeindex;
        else
          nodeindex += this.GetEscapeNodeIndex(nodeindex);
      }
      return (double) num != double.PositiveInfinity;
    }

    public unsafe bool RayQuery(
      ref IndexedVector3 ray_dir,
      ref IndexedVector3 ray_origin,
      ProcessCollisionHandler handler)
    {
      int nodeindex = 0;
      int nodeCount = this.GetNodeCount();
      bool flag1 = false;
      while (nodeindex < nodeCount)
      {
        AABB bound;
        this.GetNodeBound(nodeindex, out bound);
        bool flag2 = bound.CollideRay(ref ray_origin, ref ray_dir);
        bool flag3 = this.IsLeafNode(nodeindex);
        if (flag3 & flag2)
        {
          BT_QUANTIZED_BVH_NODE node = this.GetNode(nodeindex);
          for (int index = 0; index < (int) node.m_size; ++index)
          {
            float? nullable = handler(node.m_escapeIndexOrDataIndex[index]);
            flag1 = true;
          }
        }
        if (flag2 | flag3)
          ++nodeindex;
        else
          nodeindex += this.GetEscapeNodeIndex(nodeindex);
      }
      return flag1;
    }

    private int GetNodeCount() => this.m_box_tree.GetNodeCount();

    private bool IsLeafNode(int nodeindex) => this.m_box_tree.IsLeafNode(nodeindex);

    private BT_QUANTIZED_BVH_NODE GetNode(int nodeindex) => this.m_box_tree.GetNode(nodeindex);

    private void GetNodeBound(int nodeindex, out AABB bound) => this.m_box_tree.GetNodeBound(nodeindex, out bound);

    private int GetEscapeNodeIndex(int nodeindex) => this.m_box_tree.GetEscapeNodeIndex(nodeindex);
  }
}
