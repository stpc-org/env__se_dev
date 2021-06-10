// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyQuantizedBvhAdapter
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using BulletXNA.BulletCollision;
using BulletXNA.LinearMath;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Import;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
  public class MyQuantizedBvhAdapter : IMyTriangePruningStructure
  {
    private readonly GImpactQuantizedBvh m_bvh;
    private readonly MyModel m_model;
    [ThreadStatic]
    private static MyQuantizedBvhResult m_resultThreadStatic;
    [ThreadStatic]
    private static MyQuantizedBvhAllTrianglesResult m_resultAllThreadStatic;
    private BoundingBox[] m_bounds;
    private Plane[] m_planes;

    private static MyQuantizedBvhResult Result => MyQuantizedBvhAdapter.m_resultThreadStatic ?? (MyQuantizedBvhAdapter.m_resultThreadStatic = new MyQuantizedBvhResult());

    private static MyQuantizedBvhAllTrianglesResult ResultAll => MyQuantizedBvhAdapter.m_resultAllThreadStatic ?? (MyQuantizedBvhAdapter.m_resultAllThreadStatic = new MyQuantizedBvhAllTrianglesResult());

    public MyQuantizedBvhAdapter(GImpactQuantizedBvh bvh, MyModel model)
    {
      this.m_bvh = bvh;
      this.m_model = model;
    }

    public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity entity,
      ref LineD line,
      IntersectionFlags flags)
    {
      BoundingSphereD worldVolume = entity.PositionComp.WorldVolume;
      if (!MyUtils.IsLineIntersectingBoundingSphere(ref line, ref worldVolume))
        return new MyIntersectionResultLineTriangleEx?();
      MatrixD customInvMatrix = entity.PositionComp.WorldMatrixInvScaled;
      return this.GetIntersectionWithLine(entity, ref line, ref customInvMatrix, flags);
    }

    public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity entity,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags)
    {
      Line line1 = new Line((Vector3) Vector3D.Transform(line.From, ref customInvMatrix), (Vector3) Vector3D.Transform(line.To, ref customInvMatrix));
      this.UpdateCache();
      MyQuantizedBvhAdapter.Result.Start(this.m_model, line1, this.m_planes, flags);
      IndexedVector3 bullet1 = line1.Direction.ToBullet();
      IndexedVector3 bullet2 = line1.From.ToBullet();
      this.m_bvh.RayQueryClosest(ref bullet1, ref bullet2, MyQuantizedBvhAdapter.Result.ProcessTriangleHandler);
      return MyQuantizedBvhAdapter.Result.Result.HasValue ? new MyIntersectionResultLineTriangleEx?(new MyIntersectionResultLineTriangleEx(MyQuantizedBvhAdapter.Result.Result.Value, entity, ref line1)) : new MyIntersectionResultLineTriangleEx?();
    }

    public void GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles)
    {
      if (retTriangles.Count == maxNeighbourTriangles)
        return;
      this.UpdateCache();
      BoundingBox invalid = BoundingBox.CreateInvalid();
      BoundingSphere sphereLocal = sphere;
      invalid.Include(ref sphere);
      AABB box = new AABB(invalid.Min.ToBullet(), invalid.Max.ToBullet());
      this.m_bvh.BoxQuery(ref box, (ProcessHandler) (triangleIndex =>
      {
        if (this.CheckSphereTriangleIntersection(ref sphereLocal, triangleIndex))
        {
          if (referenceNormalVector.HasValue && maxAngle.HasValue)
          {
            double angleBetweenVectors = (double) MyUtils.GetAngleBetweenVectors(referenceNormalVector.Value, this.m_planes[triangleIndex].Normal);
            float? nullable = maxAngle;
            double valueOrDefault = (double) nullable.GetValueOrDefault();
            if (!(angleBetweenVectors <= valueOrDefault & nullable.HasValue))
              goto label_5;
          }
          MyTriangleVertexIndices triangle = this.m_model.Triangles[triangleIndex];
          retTriangles.Add(new MyTriangle_Vertex_Normals()
          {
            Vertices = new MyTriangle_Vertices()
            {
              Vertex0 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I0].Position),
              Vertex1 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I1].Position),
              Vertex2 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I2].Position)
            },
            Normals = {
              Normal0 = this.m_model.GetVertexNormal(triangle.I0),
              Normal1 = this.m_model.GetVertexNormal(triangle.I2),
              Normal2 = this.m_model.GetVertexNormal(triangle.I1)
            }
          });
          if (retTriangles.Count == maxNeighbourTriangles)
            return true;
        }
label_5:
        return false;
      }));
    }

    public bool GetIntersectionWithAABB(IMyEntity entity, ref BoundingBoxD aabb)
    {
      this.UpdateCache();
      MatrixD matrix = entity.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D = Vector3D.Transform(aabb.Center, ref matrix);
      BoundingBoxD boundingBoxD = aabb;
      boundingBoxD.Translate(vector3D - aabb.Center);
      AABB box = new AABB(boundingBoxD.Min.ToBullet(), boundingBoxD.Max.ToBullet());
      return this.m_bvh.BoxQuery(ref box, (ProcessHandler) (triangleIndex => true));
    }

    public bool GetIntersectionWithSphere(IMyEntity entity, ref BoundingSphereD sphere)
    {
      MatrixD matrix = entity.PositionComp.WorldMatrixNormalizedInv;
      BoundingSphere sphereInObjectSpace = new BoundingSphere((Vector3) Vector3D.Transform(sphere.Center, ref matrix), (float) sphere.Radius);
      return this.GetIntersectionWithSphere(ref sphereInObjectSpace);
    }

    private void UpdateCache()
    {
      if (this.m_bounds != null)
        return;
      lock (this.m_bvh)
      {
        if (this.m_bounds != null)
          return;
        int trianglesCount = this.m_model.GetTrianglesCount();
        this.m_bounds = new BoundingBox[trianglesCount];
        this.m_planes = new Plane[trianglesCount];
        for (int index = 0; index < trianglesCount; ++index)
        {
          MyTriangleVertexIndices triangle = this.m_model.Triangles[index];
          MyTriangle_Vertices triangleVertices = new MyTriangle_Vertices()
          {
            Vertex0 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I0].Position),
            Vertex1 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I1].Position),
            Vertex2 = PositionPacker.UnpackPosition(ref this.m_model.Vertices[triangle.I2].Position)
          };
          this.m_bounds[index].Min = this.m_bounds[index].Max = triangleVertices.Vertex0;
          this.m_bounds[index].Include(ref triangleVertices.Vertex1);
          this.m_bounds[index].Include(ref triangleVertices.Vertex2);
          this.m_planes[index] = new Plane(ref triangleVertices.Vertex0, ref triangleVertices.Vertex1, ref triangleVertices.Vertex2);
        }
      }
    }

    public bool GetIntersectionWithSphere(ref BoundingSphere sphereInObjectSpace)
    {
      this.UpdateCache();
      BoundingBox invalid = BoundingBox.CreateInvalid();
      BoundingSphere sphereF = sphereInObjectSpace;
      invalid.Include(ref sphereInObjectSpace);
      AABB box = new AABB(invalid.Min.ToBullet(), invalid.Max.ToBullet());
      return this.m_bvh.BoxQuery(ref box, (ProcessHandler) (triangleIndex => this.CheckSphereTriangleIntersection(ref sphereF, triangleIndex)));
    }

    private bool CheckSphereTriangleIntersection(ref BoundingSphere sphereF, int triangleIndex)
    {
      if (this.m_bounds[triangleIndex].Intersects(ref sphereF))
      {
        MyTriangleVertexIndices triangle1 = this.m_model.Triangles[triangleIndex];
        MyTriangle_Vertices triangle2;
        this.m_model.GetVertex(triangle1.I0, triangle1.I1, triangle1.I2, out triangle2.Vertex0, out triangle2.Vertex1, out triangle2.Vertex2);
        if (MyUtils.GetSphereTriangleIntersection(ref sphereF, ref this.m_planes[triangleIndex], ref triangle2).HasValue)
          return true;
      }
      return false;
    }

    public void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result)
    {
      MatrixD customInvMatrix = entity.PositionComp.WorldMatrixNormalizedInv;
      this.GetTrianglesIntersectingLine(entity, ref line, ref customInvMatrix, flags, result);
    }

    public void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result)
    {
      this.UpdateCache();
      Line line1 = new Line((Vector3) Vector3D.Transform(line.From, ref customInvMatrix), (Vector3) Vector3D.Transform(line.To, ref customInvMatrix));
      MyQuantizedBvhAdapter.ResultAll.Start(this.m_model, line1, this.m_planes, flags);
      IndexedVector3 bullet1 = line1.Direction.ToBullet();
      IndexedVector3 bullet2 = line1.From.ToBullet();
      this.m_bvh.RayQuery(ref bullet1, ref bullet2, MyQuantizedBvhAdapter.ResultAll.ProcessTriangleHandler);
      MyQuantizedBvhAdapter.ResultAll.End();
      foreach (MyIntersectionResultLineTriangle triangle in MyQuantizedBvhAdapter.ResultAll.Result)
        result.Add(new MyIntersectionResultLineTriangleEx(triangle, entity, ref line1));
    }

    public void GetTrianglesIntersectingAABB(
      ref BoundingBox aabb,
      List<MyTriangle_Vertex_Normal> retTriangles,
      int maxNeighbourTriangles)
    {
      if (retTriangles.Count == maxNeighbourTriangles)
        return;
      this.UpdateCache();
      IndexedVector3 bullet1 = aabb.Min.ToBullet();
      IndexedVector3 bullet2 = aabb.Max.ToBullet();
      AABB box = new AABB(ref bullet1, ref bullet2);
      this.m_bvh.BoxQuery(ref box, (ProcessHandler) (triangleIndex =>
      {
        MyTriangleVertexIndices triangle = this.m_model.Triangles[triangleIndex];
        MyTriangle_Vertices triangleVertices = new MyTriangle_Vertices();
        this.m_model.GetVertex(triangle.I0, triangle.I1, triangle.I2, out triangleVertices.Vertex0, out triangleVertices.Vertex1, out triangleVertices.Vertex2);
        MyTriangle_Vertex_Normal triangleVertexNormal;
        triangleVertexNormal.Vertexes = triangleVertices;
        triangleVertexNormal.Normal = Vector3.Forward;
        retTriangles.Add(triangleVertexNormal);
        return retTriangles.Count == maxNeighbourTriangles;
      }));
    }

    public void Close()
    {
    }

    public int Size => this.m_bvh.Size;
  }
}
