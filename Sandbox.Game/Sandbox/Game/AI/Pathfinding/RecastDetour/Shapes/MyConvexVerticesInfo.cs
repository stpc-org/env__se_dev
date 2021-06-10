// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes.MyConvexVerticesInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
  public class MyConvexVerticesInfo
  {
    public Matrix m_rdWorldMatrix;

    public Vector3[] Vertices { get; set; }

    public MyConvexVerticesInfo(Matrix rdWorldMatrix, Vector3[] vertices)
    {
      this.m_rdWorldMatrix = rdWorldMatrix;
      this.Vertices = vertices;
    }
  }
}
