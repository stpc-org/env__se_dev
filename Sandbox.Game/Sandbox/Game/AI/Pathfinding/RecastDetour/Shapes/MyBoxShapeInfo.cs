// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes.MyBoxShapeInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
  public class MyBoxShapeInfo
  {
    public Matrix RdWorldMatrix { get; set; }

    public float HalfExtentsX { get; set; }

    public float HalfExtentsY { get; set; }

    public float HalfExtentsZ { get; set; }

    public MyBoxShapeInfo(
      Matrix rdWorldTranslation,
      float halfExtentsX,
      float halfExtentsY,
      float halfExtentsZ)
    {
      this.RdWorldMatrix = rdWorldTranslation;
      this.HalfExtentsX = halfExtentsX;
      this.HalfExtentsY = halfExtentsY;
      this.HalfExtentsZ = halfExtentsZ;
    }
  }
}
