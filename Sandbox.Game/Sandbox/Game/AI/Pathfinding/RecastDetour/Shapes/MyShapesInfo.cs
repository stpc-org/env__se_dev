// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes.MyShapesInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
  public class MyShapesInfo
  {
    public List<MyBoxShapeInfo> Boxes { get; set; } = new List<MyBoxShapeInfo>();

    public List<MySphereShapeInfo> Spheres { get; set; } = new List<MySphereShapeInfo>();

    public List<MyConvexVerticesInfo> ConvexVertices { get; set; } = new List<MyConvexVerticesInfo>();
  }
}
