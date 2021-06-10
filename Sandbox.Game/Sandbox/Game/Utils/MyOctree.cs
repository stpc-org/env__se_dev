// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Utils.MyOctree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Voxels;
using VRageMath;

namespace Sandbox.Game.Utils
{
  internal class MyOctree
  {
    private const int NODE_COUNT = 73;
    private byte[] m_childEmpty = new byte[9];
    private short[] m_firstTriangleIndex = new short[73];
    private byte[] m_triangleCount = new byte[73];
    private Vector3 m_bbMin;
    private Vector3 m_bbInvScale;
    private const float CHILD_SIZE = 0.65f;
    private static readonly BoundingBox[] box = new BoundingBox[73];

    public void Init(
      Vector3[] positions,
      int vertexCount,
      MyVoxelTriangle[] triangles,
      int triangleCount,
      out MyVoxelTriangle[] sortedTriangles)
    {
      for (int index = 0; index < 73; ++index)
      {
        this.m_firstTriangleIndex[index] = (short) 0;
        this.m_triangleCount[index] = (byte) 0;
      }
      for (int index = 0; index < 9; ++index)
        this.m_childEmpty[index] = (byte) 0;
      BoundingBox invalid1 = BoundingBox.CreateInvalid();
      for (int index = 0; index < vertexCount; ++index)
        invalid1.Include(ref positions[index]);
      this.m_bbMin = invalid1.Min;
      Vector3 vector3 = invalid1.Max - invalid1.Min;
      this.m_bbInvScale = Vector3.One;
      if ((double) vector3.X > 1.0)
        this.m_bbInvScale.X = 1f / vector3.X;
      if ((double) vector3.Y > 1.0)
        this.m_bbInvScale.Y = 1f / vector3.Y;
      if ((double) vector3.Z > 1.0)
        this.m_bbInvScale.Z = 1f / vector3.Z;
      for (int index = 0; index < triangleCount; ++index)
      {
        MyVoxelTriangle triangle = triangles[index];
        BoundingBox invalid2 = BoundingBox.CreateInvalid();
        invalid2.Include(ref positions[(int) triangle.V0], ref positions[(int) triangle.V1], ref positions[(int) triangle.V2]);
        ++this.m_triangleCount[this.GetNode(ref invalid2)];
      }
      this.m_firstTriangleIndex[0] = (short) this.m_triangleCount[0];
      for (int index = 1; index < 73; ++index)
        this.m_firstTriangleIndex[index] = (short) ((int) this.m_firstTriangleIndex[index - 1] + (int) this.m_triangleCount[index]);
      MyVoxelTriangle[] myVoxelTriangleArray = new MyVoxelTriangle[triangleCount];
      for (int index = 0; index < triangleCount; ++index)
      {
        MyVoxelTriangle triangle = triangles[index];
        BoundingBox invalid2 = BoundingBox.CreateInvalid();
        invalid2.Include(ref positions[(int) triangle.V0], ref positions[(int) triangle.V1], ref positions[(int) triangle.V2]);
        myVoxelTriangleArray[(int) --this.m_firstTriangleIndex[this.GetNode(ref invalid2)]] = triangle;
      }
      sortedTriangles = myVoxelTriangleArray;
      for (int index = 72; index > 0; --index)
      {
        if (this.m_triangleCount[index] == (byte) 0 && (index > 8 || this.m_childEmpty[index] == byte.MaxValue))
          this.m_childEmpty[index - 1 >> 3] |= (byte) (1 << (index - 1 & 7));
      }
    }

    public void BoxQuery(ref BoundingBox bbox, List<int> triangleIndices)
    {
      BoundingBox box = new BoundingBox((bbox.Min - this.m_bbMin) * this.m_bbInvScale, (bbox.Max - this.m_bbMin) * this.m_bbInvScale);
      bool result;
      MyOctree.box[0].Intersects(ref box, out result);
      if (!result)
        return;
      for (int index = 0; index < (int) this.m_triangleCount[0]; ++index)
        triangleIndices.Add((int) this.m_firstTriangleIndex[0] + index);
      int index1 = 1;
      int num1 = 1;
      while (index1 < 9)
      {
        if (((int) this.m_childEmpty[0] & num1) == 0)
        {
          MyOctree.box[index1].Intersects(ref box, out result);
          if (result)
          {
            for (int index2 = 0; index2 < (int) this.m_triangleCount[index1]; ++index2)
              triangleIndices.Add((int) this.m_firstTriangleIndex[index1] + index2);
            int index3 = index1 * 8 + 1;
            int num2 = 1;
            while (index3 < index1 * 8 + 9)
            {
              if (((int) this.m_childEmpty[index1] & num2) == 0)
              {
                MyOctree.box[index3].Intersects(ref box, out result);
                if (result)
                {
                  for (int index2 = 0; index2 < (int) this.m_triangleCount[index3]; ++index2)
                    triangleIndices.Add((int) this.m_firstTriangleIndex[index3] + index2);
                }
              }
              ++index3;
              num2 <<= 1;
            }
          }
        }
        ++index1;
        num1 <<= 1;
      }
    }

    public void GetIntersectionWithLine(ref Ray ray, List<int> triangleIndices)
    {
      Ray ray1 = new Ray((ray.Position - this.m_bbMin) * this.m_bbInvScale, ray.Direction * this.m_bbInvScale);
      float? result;
      MyOctree.box[0].Intersects(ref ray1, out result);
      if (!result.HasValue)
        return;
      for (int index = 0; index < (int) this.m_triangleCount[0]; ++index)
        triangleIndices.Add((int) this.m_firstTriangleIndex[0] + index);
      int index1 = 1;
      int num1 = 1;
      while (index1 < 9)
      {
        if (((int) this.m_childEmpty[0] & num1) == 0)
        {
          MyOctree.box[index1].Intersects(ref ray1, out result);
          if (result.HasValue)
          {
            for (int index2 = 0; index2 < (int) this.m_triangleCount[index1]; ++index2)
              triangleIndices.Add((int) this.m_firstTriangleIndex[index1] + index2);
            int index3 = index1 * 8 + 1;
            int num2 = 1;
            while (index3 < index1 * 8 + 9)
            {
              if (((int) this.m_childEmpty[index1] & num2) == 0)
              {
                MyOctree.box[index3].Intersects(ref ray1, out result);
                if (result.HasValue)
                {
                  for (int index2 = 0; index2 < (int) this.m_triangleCount[index3]; ++index2)
                    triangleIndices.Add((int) this.m_firstTriangleIndex[index3] + index2);
                }
              }
              ++index3;
              num2 <<= 1;
            }
          }
        }
        ++index1;
        num1 <<= 1;
      }
    }

    static MyOctree()
    {
      int index = 0;
      int num1 = 0;
      while (num1 < 1)
      {
        MyOctree.box[index].Min = Vector3.Zero;
        MyOctree.box[index].Max = Vector3.One;
        ++num1;
        ++index;
      }
      int num2 = 0;
      while (num2 < 8)
      {
        if ((num2 & 4) == 0)
        {
          MyOctree.box[index].Min.Z = 0.0f;
          MyOctree.box[index].Max.Z = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.Z = 0.35f;
          MyOctree.box[index].Max.Z = 1f;
        }
        if ((num2 & 2) == 0)
        {
          MyOctree.box[index].Min.Y = 0.0f;
          MyOctree.box[index].Max.Y = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.Y = 0.35f;
          MyOctree.box[index].Max.Y = 1f;
        }
        if ((num2 & 1) == 0)
        {
          MyOctree.box[index].Min.X = 0.0f;
          MyOctree.box[index].Max.X = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.X = 0.35f;
          MyOctree.box[index].Max.X = 1f;
        }
        ++num2;
        ++index;
      }
      int num3 = 0;
      while (num3 < 64)
      {
        if ((num3 & 32) == 0)
        {
          MyOctree.box[index].Min.Z = 0.0f;
          MyOctree.box[index].Max.Z = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.Z = 0.35f;
          MyOctree.box[index].Max.Z = 1f;
        }
        if ((num3 & 16) == 0)
        {
          MyOctree.box[index].Min.Y = 0.0f;
          MyOctree.box[index].Max.Y = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.Y = 0.35f;
          MyOctree.box[index].Max.Y = 1f;
        }
        if ((num3 & 8) == 0)
        {
          MyOctree.box[index].Min.X = 0.0f;
          MyOctree.box[index].Max.X = 0.65f;
        }
        else
        {
          MyOctree.box[index].Min.X = 0.35f;
          MyOctree.box[index].Max.X = 1f;
        }
        if ((num3 & 4) == 0)
          MyOctree.box[index].Max.Z = MyOctree.box[index].Min.Z + (float) (((double) MyOctree.box[index].Max.Z - (double) MyOctree.box[index].Min.Z) * 0.649999976158142);
        else
          MyOctree.box[index].Min.Z += (float) (((double) MyOctree.box[index].Max.Z - (double) MyOctree.box[index].Min.Z) * 0.350000023841858);
        if ((num3 & 2) == 0)
          MyOctree.box[index].Max.Y = MyOctree.box[index].Min.Y + (float) (((double) MyOctree.box[index].Max.Y - (double) MyOctree.box[index].Min.Y) * 0.649999976158142);
        else
          MyOctree.box[index].Min.Y += (float) (((double) MyOctree.box[index].Max.Y - (double) MyOctree.box[index].Min.Y) * 0.350000023841858);
        if ((num3 & 1) == 0)
          MyOctree.box[index].Max.X = MyOctree.box[index].Min.X + (float) (((double) MyOctree.box[index].Max.X - (double) MyOctree.box[index].Min.X) * 0.649999976158142);
        else
          MyOctree.box[index].Min.X += (float) (((double) MyOctree.box[index].Max.X - (double) MyOctree.box[index].Min.X) * 0.350000023841858);
        ++num3;
        ++index;
      }
    }

    private int GetNode(ref BoundingBox triangleAabb)
    {
      BoundingBox boundingBox = new BoundingBox((triangleAabb.Min - this.m_bbMin) * this.m_bbInvScale, (triangleAabb.Max - this.m_bbMin) * this.m_bbInvScale);
      int num = 0;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        int index2 = num * 8 + 1;
        if ((double) boundingBox.Min.X > (double) MyOctree.box[index2 + 1].Min.X)
          ++index2;
        else if ((double) boundingBox.Max.X >= (double) MyOctree.box[index2].Max.X)
          break;
        if ((double) boundingBox.Min.Y > (double) MyOctree.box[index2 + 2].Min.Y)
          index2 += 2;
        else if ((double) boundingBox.Max.Y >= (double) MyOctree.box[index2].Max.Y)
          break;
        if ((double) boundingBox.Min.Z > (double) MyOctree.box[index2 + 4].Min.Z)
          index2 += 4;
        else if ((double) boundingBox.Max.Z >= (double) MyOctree.box[index2].Max.Z)
          break;
        num = index2;
      }
      return num;
    }
  }
}
