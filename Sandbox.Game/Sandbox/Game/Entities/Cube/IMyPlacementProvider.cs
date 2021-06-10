// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.IMyPlacementProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public interface IMyPlacementProvider
  {
    Vector3D RayStart { get; }

    Vector3D RayDirection { get; }

    MyPhysics.HitInfo? HitInfo { get; }

    MyCubeGrid ClosestGrid { get; }

    MyVoxelBase ClosestVoxelMap { get; }

    bool CanChangePlacementObjectSize { get; }

    float IntersectionDistance { get; set; }

    void RayCastGridCells(
      MyCubeGrid grid,
      List<Vector3I> outHitPositions,
      Vector3I gridSizeInflate,
      float maxDist);

    void UpdatePlacement();
  }
}
