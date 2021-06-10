// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyIntersectionResultLineTriangle
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game.Models
{
  public struct MyIntersectionResultLineTriangle
  {
    public float Distance;
    public MyTriangle_Vertices InputTriangle;
    public MyTriangle_BoneIndicesWeigths? BoneWeights;
    public Vector3 InputTriangleNormal;
    public int TriangleIndex;

    public MyIntersectionResultLineTriangle(
      int triangleIndex,
      ref MyTriangle_Vertices triangle,
      ref Vector3 triangleNormal,
      float distance)
    {
      this.InputTriangle = triangle;
      this.InputTriangleNormal = triangleNormal;
      this.Distance = distance;
      this.BoneWeights = new MyTriangle_BoneIndicesWeigths?();
      this.TriangleIndex = triangleIndex;
    }

    public MyIntersectionResultLineTriangle(
      int triangleIndex,
      ref MyTriangle_Vertices triangle,
      ref MyTriangle_BoneIndicesWeigths? boneWeigths,
      ref Vector3 triangleNormal,
      float distance)
    {
      this.InputTriangle = triangle;
      this.InputTriangleNormal = triangleNormal;
      this.Distance = distance;
      this.BoneWeights = boneWeigths;
      this.TriangleIndex = triangleIndex;
    }

    public static MyIntersectionResultLineTriangle? GetCloserIntersection(
      ref MyIntersectionResultLineTriangle? a,
      ref MyIntersectionResultLineTriangle? b)
    {
      return !a.HasValue && b.HasValue || a.HasValue && b.HasValue && (double) b.Value.Distance < (double) a.Value.Distance ? b : a;
    }
  }
}
