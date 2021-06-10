// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes.MySphereShapeInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
  public class MySphereShapeInfo
  {
    public Vector3 RdWorldTranslation { get; set; }

    public float Radius { get; set; }

    public MySphereShapeInfo(Vector3 rdWorldTranslation, float radius)
    {
      this.RdWorldTranslation = rdWorldTranslation;
      this.Radius = radius;
    }
  }
}
