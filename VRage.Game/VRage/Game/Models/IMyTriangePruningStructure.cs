// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.IMyTriangePruningStructure
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
  public interface IMyTriangePruningStructure
  {
    MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity entity,
      ref LineD line,
      IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES);

    MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(
      IMyEntity entity,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags = IntersectionFlags.DIRECT_TRIANGLES);

    void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      ref MatrixD customInvMatrix,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result);

    void GetTrianglesIntersectingLine(
      IMyEntity entity,
      ref LineD line,
      IntersectionFlags flags,
      List<MyIntersectionResultLineTriangleEx> result);

    void GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles);

    bool GetIntersectionWithSphere(IMyEntity physObject, ref BoundingSphereD sphere);

    bool GetIntersectionWithSphere(ref BoundingSphere localSphere);

    bool GetIntersectionWithAABB(IMyEntity physObject, ref BoundingBoxD aabb);

    void GetTrianglesIntersectingAABB(
      ref BoundingBox sphere,
      List<MyTriangle_Vertex_Normal> retTriangles,
      int maxNeighbourTriangles);

    void Close();

    int Size { get; }
  }
}
