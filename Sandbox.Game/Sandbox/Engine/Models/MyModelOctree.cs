// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Models.MyModelOctree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Models
{
  internal class MyModelOctree : IMyTriangePruningStructure
  {
    private MyModel m_model;
    private MyModelOctreeNode m_rootNode;

    private MyModelOctree()
    {
    }

    public MyModelOctree(MyModel model)
    {
      this.m_model = model;
      this.m_rootNode = new MyModelOctreeNode(model.BoundingBox);
      for (int triangleIndex = 0; triangleIndex < this.m_model.Triangles.Length; ++triangleIndex)
        this.m_rootNode.AddTriangle(model, triangleIndex, 0);
      this.m_rootNode.OptimizeChilds();
    }

    public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity physObject,
      ref LineD line,
      IntersectionFlags flags)
    {
      BoundingSphereD worldVolume = physObject.WorldVolume;
      if (!MyUtils.IsLineIntersectingBoundingSphere(ref line, ref worldVolume))
        return new MyIntersectionResultLineTriangleEx?();
      MatrixD matrixNormalizedInv = physObject.GetWorldMatrixNormalizedInv();
      return this.GetIntersectionWithLine(physObject, ref line, ref matrixNormalizedInv, flags);
    }

    public MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity physObject,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags)
    {
      Line line1 = new Line((Vector3) Vector3D.Transform(line.From, ref customInvMatrix), (Vector3) Vector3D.Transform(line.To, ref customInvMatrix));
      return this.m_rootNode.GetIntersectionWithLine(physObject, this.m_model, ref line1, new double?(), flags);
    }

    public void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result)
    {
      MatrixD matrixNormalizedInv = entity.GetWorldMatrixNormalizedInv();
      this.GetTrianglesIntersectingLine(entity, ref line, ref matrixNormalizedInv, flags, result);
    }

    public void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result)
    {
    }

    public void GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles)
    {
      this.m_rootNode.GetTrianglesIntersectingSphere(this.m_model, ref sphere, referenceNormalVector, maxAngle, retTriangles, maxNeighbourTriangles);
    }

    public bool GetIntersectionWithSphere(IMyEntity physObject, ref BoundingSphereD sphere)
    {
      MatrixD matrixNormalizedInv = physObject.GetWorldMatrixNormalizedInv();
      BoundingSphere sphere1 = new BoundingSphere((Vector3) Vector3D.Transform(sphere.Center, ref matrixNormalizedInv), (float) sphere.Radius);
      return this.m_rootNode.GetIntersectionWithSphere(this.m_model, ref sphere1);
    }

    public bool GetIntersectionWithSphere(ref BoundingSphere sphere) => this.m_rootNode.GetIntersectionWithSphere(this.m_model, ref sphere);

    public bool GetIntersectionWithAABB(IMyEntity physObject, ref BoundingBoxD aabb) => false;

    public void GetTrianglesIntersectingAABB(
      ref BoundingBox box,
      List<MyTriangle_Vertex_Normal> retTriangles,
      int maxNeighbourTriangles)
    {
    }

    public void Close()
    {
    }

    public int Size => 0;
  }
}
