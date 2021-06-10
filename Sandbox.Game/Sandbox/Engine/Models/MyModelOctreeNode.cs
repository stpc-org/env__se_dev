// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Models.MyModelOctreeNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Models
{
  internal class MyModelOctreeNode
  {
    private const int OCTREE_CHILDS_COUNT = 8;
    private const int MAX_RECURSIVE_LEVEL = 8;
    private const float CHILD_BOUNDING_BOX_EXPAND = 0.3f;
    private List<MyModelOctreeNode> m_childs;
    private BoundingBox m_boundingBox;
    private BoundingBox m_realBoundingBox;
    private List<int> m_triangleIndices;

    private MyModelOctreeNode()
    {
    }

    public MyModelOctreeNode(BoundingBox boundingBox)
    {
      this.m_childs = new List<MyModelOctreeNode>(8);
      for (int index = 0; index < 8; ++index)
        this.m_childs.Add((MyModelOctreeNode) null);
      this.m_boundingBox = boundingBox;
      this.m_realBoundingBox = BoundingBox.CreateInvalid();
      this.m_triangleIndices = new List<int>();
    }

    public void OptimizeChilds()
    {
      this.m_boundingBox = this.m_realBoundingBox;
      for (int index = 0; index < this.m_childs.Count; ++index)
      {
        if (this.m_childs[index] != null)
          this.m_childs[index].OptimizeChilds();
      }
      do
        ;
      while (this.m_childs.Remove((MyModelOctreeNode) null));
      for (; this.m_childs != null && this.m_childs.Count == 1; this.m_childs = this.m_childs[0].m_childs)
      {
        foreach (int triangleIndex in this.m_childs[0].m_triangleIndices)
          this.m_triangleIndices.Add(triangleIndex);
      }
      if (this.m_childs == null || this.m_childs.Count != 0)
        return;
      this.m_childs = (List<MyModelOctreeNode>) null;
    }

    public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity physObject,
      MyModel model,
      ref Line line,
      double? minDistanceUntilNow,
      IntersectionFlags flags)
    {
      MyIntersectionResultLineTriangle? withLineRecursive = this.GetIntersectionWithLineRecursive(model, ref line, minDistanceUntilNow);
      return withLineRecursive.HasValue ? new MyIntersectionResultLineTriangleEx?(new MyIntersectionResultLineTriangleEx(withLineRecursive.Value, physObject, ref line)) : new MyIntersectionResultLineTriangleEx?();
    }

    private MyIntersectionResultLineTriangle? GetIntersectionWithLineRecursive(
      MyModel model,
      ref Line line,
      double? minDistanceUntilNow)
    {
      float? boundingBoxIntersection = MyUtils.GetLineBoundingBoxIntersection(ref line, ref this.m_boundingBox);
      double? nullable1 = boundingBoxIntersection.HasValue ? new double?((double) boundingBoxIntersection.GetValueOrDefault()) : new double?();
      if (nullable1.HasValue)
      {
        double? nullable2;
        if (minDistanceUntilNow.HasValue)
        {
          nullable2 = minDistanceUntilNow;
          double num = nullable1.Value;
          if (nullable2.GetValueOrDefault() < num & nullable2.HasValue)
            goto label_3;
        }
        MyIntersectionResultLineTriangle? a = new MyIntersectionResultLineTriangle?();
        BoundingBox boundingBox = new BoundingBox();
        BoundingBox box = BoundingBox.CreateInvalid().Include(line.From);
        box = box.Include(line.To);
        for (int index = 0; index < this.m_triangleIndices.Count; ++index)
        {
          int triangleIndex = this.m_triangleIndices[index];
          model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
          if (boundingBox.Intersects(ref box))
          {
            MyTriangleVertexIndices triangle = model.Triangles[triangleIndex];
            MyTriangle_Vertices triangleVertices;
            triangleVertices.Vertex0 = model.GetVertex(triangle.I0);
            triangleVertices.Vertex1 = model.GetVertex(triangle.I2);
            triangleVertices.Vertex2 = model.GetVertex(triangle.I1);
            float? triangleIntersection = MyUtils.GetLineTriangleIntersection(ref line, ref triangleVertices);
            if (triangleIntersection.HasValue && (!a.HasValue || (double) triangleIntersection.Value < (double) a.Value.Distance))
            {
              Vector3 vectorFromTriangle = MyUtils.GetNormalVectorFromTriangle(ref triangleVertices);
              a = new MyIntersectionResultLineTriangle?(new MyIntersectionResultLineTriangle(triangleIndex, ref triangleVertices, ref vectorFromTriangle, triangleIntersection.Value));
            }
          }
        }
        if (this.m_childs != null)
        {
          for (int index = 0; index < this.m_childs.Count; ++index)
          {
            MyModelOctreeNode child = this.m_childs[index];
            MyModel model1 = model;
            ref Line local = ref line;
            double? minDistanceUntilNow1;
            if (a.HasValue)
            {
              minDistanceUntilNow1 = new double?((double) a.Value.Distance);
            }
            else
            {
              nullable2 = new double?();
              minDistanceUntilNow1 = nullable2;
            }
            MyIntersectionResultLineTriangle? withLineRecursive = child.GetIntersectionWithLineRecursive(model1, ref local, minDistanceUntilNow1);
            a = MyIntersectionResultLineTriangle.GetCloserIntersection(ref a, ref withLineRecursive);
          }
        }
        return a;
      }
label_3:
      return new MyIntersectionResultLineTriangle?();
    }

    public void GetTrianglesIntersectingSphere(
      MyModel model,
      ref BoundingSphereD sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normal> retTriangles,
      int maxNeighbourTriangles)
    {
      BoundingSphere sphere1 = (BoundingSphere) sphere;
      if (!this.m_boundingBox.Intersects(ref sphere))
        return;
      BoundingBox boundingBox = new BoundingBox();
      for (int index = 0; index < this.m_triangleIndices.Count; ++index)
      {
        if (retTriangles.Count == maxNeighbourTriangles)
          return;
        int triangleIndex = this.m_triangleIndices[index];
        model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
        if (boundingBox.Intersects(ref sphere))
        {
          MyTriangleVertexIndices triangle = model.Triangles[triangleIndex];
          MyTriangle_Vertices triangleVertices;
          triangleVertices.Vertex0 = model.GetVertex(triangle.I0);
          triangleVertices.Vertex1 = model.GetVertex(triangle.I2);
          triangleVertices.Vertex2 = model.GetVertex(triangle.I1);
          Vector3 vectorFromTriangle1 = MyUtils.GetNormalVectorFromTriangle(ref triangleVertices);
          Plane trianglePlane = new Plane(triangleVertices.Vertex0, triangleVertices.Vertex1, triangleVertices.Vertex2);
          if (MyUtils.GetSphereTriangleIntersection(ref sphere1, ref trianglePlane, ref triangleVertices).HasValue)
          {
            Vector3 vectorFromTriangle2 = MyUtils.GetNormalVectorFromTriangle(ref triangleVertices);
            if (referenceNormalVector.HasValue && maxAngle.HasValue)
            {
              double angleBetweenVectors = (double) MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, vectorFromTriangle2);
              float? nullable = maxAngle;
              double valueOrDefault = (double) nullable.GetValueOrDefault();
              if (!(angleBetweenVectors <= valueOrDefault & nullable.HasValue))
                continue;
            }
            MyTriangle_Vertex_Normal triangleVertexNormal;
            triangleVertexNormal.Vertexes = triangleVertices;
            triangleVertexNormal.Normal = vectorFromTriangle1;
            retTriangles.Add(triangleVertexNormal);
          }
        }
      }
      if (this.m_childs == null)
        return;
      for (int index = 0; index < this.m_childs.Count; ++index)
        this.m_childs[index].GetTrianglesIntersectingSphere(model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
    }

    public void GetTrianglesIntersectingSphere(
      MyModel model,
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles)
    {
      if (!this.m_boundingBox.Intersects(ref sphere))
        return;
      BoundingBox boundingBox = new BoundingBox();
      for (int index = 0; index < this.m_triangleIndices.Count; ++index)
      {
        if (retTriangles.Count == maxNeighbourTriangles)
          return;
        int triangleIndex = this.m_triangleIndices[index];
        model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
        if (boundingBox.Intersects(ref sphere))
        {
          MyTriangleVertexIndices triangle = model.Triangles[triangleIndex];
          MyTriangle_Vertices triangleVertices;
          triangleVertices.Vertex0 = model.GetVertex(triangle.I0);
          triangleVertices.Vertex1 = model.GetVertex(triangle.I2);
          triangleVertices.Vertex2 = model.GetVertex(triangle.I1);
          MyTriangle_Normals myTriangleNormals;
          myTriangleNormals.Normal0 = model.GetVertexNormal(triangle.I0);
          myTriangleNormals.Normal1 = model.GetVertexNormal(triangle.I2);
          myTriangleNormals.Normal2 = model.GetVertexNormal(triangle.I1);
          Plane trianglePlane = new Plane(triangleVertices.Vertex0, triangleVertices.Vertex1, triangleVertices.Vertex2);
          if (MyUtils.GetSphereTriangleIntersection(ref sphere, ref trianglePlane, ref triangleVertices).HasValue)
          {
            Vector3 vectorFromTriangle = MyUtils.GetNormalVectorFromTriangle(ref triangleVertices);
            if (referenceNormalVector.HasValue && maxAngle.HasValue)
            {
              double angleBetweenVectors = (double) MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, vectorFromTriangle);
              float? nullable = maxAngle;
              double valueOrDefault = (double) nullable.GetValueOrDefault();
              if (!(angleBetweenVectors <= valueOrDefault & nullable.HasValue))
                continue;
            }
            MyTriangle_Vertex_Normals triangleVertexNormals;
            triangleVertexNormals.Vertices = triangleVertices;
            triangleVertexNormals.Normals = myTriangleNormals;
            retTriangles.Add(triangleVertexNormals);
          }
        }
      }
      if (this.m_childs == null)
        return;
      for (int index = 0; index < this.m_childs.Count; ++index)
        this.m_childs[index].GetTrianglesIntersectingSphere(model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
    }

    public bool GetIntersectionWithSphere(MyModel model, ref BoundingSphere sphere)
    {
      if (!this.m_boundingBox.Intersects(ref sphere))
        return false;
      BoundingBox boundingBox = new BoundingBox();
      for (int index = 0; index < this.m_triangleIndices.Count; ++index)
      {
        int triangleIndex = this.m_triangleIndices[index];
        model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
        if (boundingBox.Intersects(ref sphere))
        {
          MyTriangleVertexIndices triangle1 = model.Triangles[triangleIndex];
          MyTriangle_Vertices triangle2;
          triangle2.Vertex0 = model.GetVertex(triangle1.I0);
          triangle2.Vertex1 = model.GetVertex(triangle1.I2);
          triangle2.Vertex2 = model.GetVertex(triangle1.I1);
          Plane trianglePlane = new Plane(triangle2.Vertex0, triangle2.Vertex1, triangle2.Vertex2);
          if (MyUtils.GetSphereTriangleIntersection(ref sphere, ref trianglePlane, ref triangle2).HasValue)
            return true;
        }
      }
      if (this.m_childs != null)
      {
        for (int index = 0; index < this.m_childs.Count; ++index)
        {
          if (this.m_childs[index].GetIntersectionWithSphere(model, ref sphere))
            return true;
        }
      }
      return false;
    }

    public void AddTriangle(MyModel model, int triangleIndex, int recursiveLevel)
    {
      BoundingBox boundingBox = new BoundingBox();
      model.GetTriangleBoundingBox(triangleIndex, ref boundingBox);
      if (recursiveLevel != 8)
      {
        for (int index = 0; index < 8; ++index)
        {
          BoundingBox childBoundingBox = this.GetChildBoundingBox(this.m_boundingBox, index);
          if (childBoundingBox.Contains(boundingBox) == ContainmentType.Contains)
          {
            if (this.m_childs[index] == null)
              this.m_childs[index] = new MyModelOctreeNode(childBoundingBox);
            this.m_childs[index].AddTriangle(model, triangleIndex, recursiveLevel + 1);
            this.m_realBoundingBox = this.m_realBoundingBox.Include(ref boundingBox.Min);
            this.m_realBoundingBox = this.m_realBoundingBox.Include(ref boundingBox.Max);
            return;
          }
        }
      }
      this.m_triangleIndices.Add(triangleIndex);
      this.m_realBoundingBox = this.m_realBoundingBox.Include(ref boundingBox.Min);
      this.m_realBoundingBox = this.m_realBoundingBox.Include(ref boundingBox.Max);
    }

    private BoundingBox GetChildBoundingBox(
      BoundingBox parentBoundingBox,
      int childIndex)
    {
      Vector3 vector3_1;
      switch (childIndex)
      {
        case 0:
          vector3_1 = new Vector3(0.0f, 0.0f, 0.0f);
          break;
        case 1:
          vector3_1 = new Vector3(1f, 0.0f, 0.0f);
          break;
        case 2:
          vector3_1 = new Vector3(1f, 0.0f, 1f);
          break;
        case 3:
          vector3_1 = new Vector3(0.0f, 0.0f, 1f);
          break;
        case 4:
          vector3_1 = new Vector3(0.0f, 1f, 0.0f);
          break;
        case 5:
          vector3_1 = new Vector3(1f, 1f, 0.0f);
          break;
        case 6:
          vector3_1 = new Vector3(1f, 1f, 1f);
          break;
        case 7:
          vector3_1 = new Vector3(0.0f, 1f, 1f);
          break;
        default:
          throw new InvalidBranchException();
      }
      Vector3 vector3_2 = (parentBoundingBox.Max - parentBoundingBox.Min) / 2f;
      BoundingBox boundingBox = new BoundingBox()
      {
        Min = parentBoundingBox.Min + vector3_1 * vector3_2
      };
      boundingBox.Max = boundingBox.Min + vector3_2;
      boundingBox.Min -= vector3_2 * 0.3f;
      boundingBox.Max += vector3_2 * 0.3f;
      boundingBox.Min = Vector3.Max(boundingBox.Min, parentBoundingBox.Min);
      boundingBox.Max = Vector3.Min(boundingBox.Max, parentBoundingBox.Max);
      return boundingBox;
    }
  }
}
