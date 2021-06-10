// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyPlane
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Utils
{
  public struct MyPlane
  {
    public Vector3 Point;
    public Vector3 Normal;

    public MyPlane(Vector3 point, Vector3 normal)
    {
      this.Point = point;
      this.Normal = normal;
    }

    public MyPlane(ref Vector3 point, ref Vector3 normal)
    {
      this.Point = point;
      this.Normal = normal;
    }

    public MyPlane(ref MyTriangle_Vertices triangle)
    {
      this.Point = triangle.Vertex0;
      this.Normal = MyUtils.Normalize(Vector3.Cross(triangle.Vertex1 - triangle.Vertex0, triangle.Vertex2 - triangle.Vertex0));
    }

    public float GetPlaneDistance() => (float) -((double) this.Normal.X * (double) this.Point.X + (double) this.Normal.Y * (double) this.Point.Y + (double) this.Normal.Z * (double) this.Point.Z);
  }
}
