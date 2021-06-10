// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Physics.MyPhysics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI.Physics
{
  internal class MyPhysics : IMyPhysics
  {
    public static readonly MyPhysics Static = new MyPhysics();
    private MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>> m_collectorsPool = new MyConcurrentPool<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>>(10, (Action<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>>) (x => x.Clear()));

    int IMyPhysics.StepsLastSecond => Sandbox.Engine.Physics.MyPhysics.StepsLastSecond;

    float IMyPhysics.SimulationRatio => Sandbox.Engine.Physics.MyPhysics.SimulationRatio;

    float IMyPhysics.ServerSimulationRatio => Sync.ServerSimulationRatio;

    bool IMyPhysics.CastLongRay(
      Vector3D from,
      Vector3D to,
      out IHitInfo hitInfo,
      bool any)
    {
      this.AssertMainThread();
      Sandbox.Engine.Physics.MyPhysics.HitInfo? nullable = Sandbox.Engine.Physics.MyPhysics.CastLongRay(from, to, any);
      if (nullable.HasValue)
      {
        hitInfo = (IHitInfo) nullable;
        return true;
      }
      hitInfo = (IHitInfo) null;
      return false;
    }

    bool IMyPhysics.CastRay(
      Vector3D from,
      Vector3D to,
      out IHitInfo hitInfo,
      int raycastFilterLayer)
    {
      this.AssertMainThread();
      Sandbox.Engine.Physics.MyPhysics.HitInfo? nullable = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, raycastFilterLayer);
      if (nullable.HasValue)
      {
        hitInfo = (IHitInfo) nullable;
        return true;
      }
      hitInfo = (IHitInfo) null;
      return false;
    }

    void IMyPhysics.CastRay(
      Vector3D from,
      Vector3D to,
      List<IHitInfo> toList,
      int raycastFilterLayer)
    {
      this.AssertMainThread();
      List<Sandbox.Engine.Physics.MyPhysics.HitInfo> hitInfoList = this.m_collectorsPool.Get();
      toList.Clear();
      Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, hitInfoList, raycastFilterLayer);
      foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo hitInfo in hitInfoList)
        toList.Add((IHitInfo) hitInfo);
      this.m_collectorsPool.Return(hitInfoList);
    }

    bool IMyPhysics.CastRay(
      Vector3D from,
      Vector3D to,
      out IHitInfo hitInfo,
      uint raycastCollisionFilter,
      bool ignoreConvexShape)
    {
      this.AssertMainThread();
      Sandbox.Engine.Physics.MyPhysics.HitInfo hitInfo1;
      int num = Sandbox.Engine.Physics.MyPhysics.CastRay(from, to, out hitInfo1, raycastCollisionFilter, ignoreConvexShape) ? 1 : 0;
      hitInfo = (IHitInfo) hitInfo1;
      return num != 0;
    }

    public void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      int raycastCollisionFilter,
      Action<IHitInfo> callback)
    {
      Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, raycastCollisionFilter, (Action<Sandbox.Engine.Physics.MyPhysics.HitInfo?>) (x => callback((IHitInfo) x)));
    }

    public void CastRayParallel(
      ref Vector3D from,
      ref Vector3D to,
      List<IHitInfo> toList,
      int raycastCollisionFilter,
      Action<List<IHitInfo>> callback)
    {
      Sandbox.Engine.Physics.MyPhysics.CastRayParallel(ref from, ref to, this.m_collectorsPool.Get(), raycastCollisionFilter, (Action<List<Sandbox.Engine.Physics.MyPhysics.HitInfo>>) (hits =>
      {
        foreach (Sandbox.Engine.Physics.MyPhysics.HitInfo hit in hits)
          toList.Add((IHitInfo) hit);
        this.m_collectorsPool.Return(hits);
        callback(toList);
      }));
    }

    void IMyPhysics.EnsurePhysicsSpace(BoundingBoxD aabb)
    {
      this.AssertMainThread();
      Sandbox.Engine.Physics.MyPhysics.EnsurePhysicsSpace(aabb);
    }

    int IMyPhysics.GetCollisionLayer(string strLayer) => Sandbox.Engine.Physics.MyPhysics.GetCollisionLayer(strLayer);

    public Vector3 CalculateNaturalGravityAt(
      Vector3D worldPosition,
      out float naturalGravityInterference)
    {
      return MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPosition, out naturalGravityInterference);
    }

    public Vector3 CalculateArtificialGravityAt(
      Vector3D worldPosition,
      float naturalGravityInterference = 1f)
    {
      float strengthMultiplier = MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(naturalGravityInterference);
      return MyGravityProviderSystem.CalculateArtificialGravityInPoint(worldPosition, strengthMultiplier);
    }

    private void AssertMainThread()
    {
      if (MyUtils.MainThread == Thread.CurrentThread)
        return;
      MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_PhysicsParallelAccess);
    }
  }
}
