// Decompiled with JetBrains decompiler
// Type: VRage.MyTriangle_Vertices
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage
{
  public struct MyTriangle_Vertices
  {
    public Vector3 Vertex0;
    public Vector3 Vertex1;
    public Vector3 Vertex2;

    public void Transform(ref Matrix transform)
    {
      this.Vertex0 = Vector3.Transform(this.Vertex0, ref transform);
      this.Vertex1 = Vector3.Transform(this.Vertex1, ref transform);
      this.Vertex2 = Vector3.Transform(this.Vertex2, ref transform);
    }
  }
}
