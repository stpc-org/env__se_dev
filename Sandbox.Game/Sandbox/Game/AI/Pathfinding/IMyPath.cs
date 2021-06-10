// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.IMyPath
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public interface IMyPath
  {
    IMyDestinationShape Destination { get; }

    IMyEntity EndEntity { get; }

    bool IsValid { get; }

    bool PathCompleted { get; }

    bool IsWaitingForTileGeneration { get; }

    void Invalidate();

    bool GetNextTarget(
      Vector3D position,
      out Vector3D target,
      out float targetRadius,
      out IMyEntity relativeEntity);

    void ReInit(Vector3D position);

    void DebugDraw();
  }
}
