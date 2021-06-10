// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.WorldVerticesInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class WorldVerticesInfo
  {
    public MyList<Vector3> Vertices = new MyList<Vector3>();
    public int VerticesMaxValue;
    public MyList<int> Triangles = new MyList<int>();
  }
}
