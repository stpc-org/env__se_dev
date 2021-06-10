// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.IMyPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public interface IMyPathfinding
  {
    IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity relativeEntity);

    bool ReachableUnderThreshold(Vector3D begin, IMyDestinationShape end, float thresholdDistance);

    IMyPathfindingLog GetPathfindingLog();

    void Update();

    void UnloadData();

    void DebugDraw();
  }
}
