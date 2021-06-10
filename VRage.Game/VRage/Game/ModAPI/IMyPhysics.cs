// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyPhysics
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyPhysics
  {
    int StepsLastSecond { get; }

    float SimulationRatio { get; }

    float ServerSimulationRatio { get; }

    bool CastLongRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, bool any);

    void CastRay(Vector3D from, Vector3D to, List<IHitInfo> toList, int raycastFilterLayer = 0);

    bool CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, int raycastFilterLayer = 0);

    bool CastRay(
      Vector3D from,
      Vector3D to,
      out IHitInfo hitInfo,
      uint raycastCollisionFilter,
      bool ignoreConvexShape);

    void EnsurePhysicsSpace(BoundingBoxD aabb);

    int GetCollisionLayer(string strLayer);

    void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      int raycastCollisionFilter,
      Action<IHitInfo> callback);

    void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      List<IHitInfo> toList,
      int raycastCollisionFilter,
      Action<List<IHitInfo>> callback);

    Vector3 CalculateNaturalGravityAt(
      Vector3D worldPosition,
      out float naturalGravityInterference);

    Vector3 CalculateArtificialGravityAt(
      Vector3D worldPosition,
      float naturalGravityInterference = 1f);
  }
}
