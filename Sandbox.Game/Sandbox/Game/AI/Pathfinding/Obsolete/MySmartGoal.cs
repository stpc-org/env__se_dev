// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MySmartGoal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MySmartGoal : IMyHighLevelPrimitiveObserver
  {
    private MyNavigationPrimitive m_end;
    private MyHighLevelPrimitive m_hlEnd;
    private bool m_hlEndIsApproximate;
    private Vector3D m_destinationCenter;
    private static readonly Func<MyNavigationPrimitive, float> m_hlPathfindingHeuristic = new Func<MyNavigationPrimitive, float>(MySmartGoal.HlHeuristic);
    private static readonly Func<MyNavigationPrimitive, float> m_hlTerminationCriterion = new Func<MyNavigationPrimitive, float>(MySmartGoal.HlCriterion);
    private static MySmartGoal m_pathfindingStatic;
    private readonly HashSet<MyHighLevelPrimitive> m_ignoredPrimitives;

    public IMyDestinationShape Destination { get; }

    public MyEntity EndEntity { get; private set; }

    public Func<MyNavigationPrimitive, float> PathfindingHeuristic { get; }

    public Func<MyNavigationPrimitive, float> TerminationCriterion { get; }

    public bool IsValid { get; private set; }

    public MySmartGoal(IMyDestinationShape goal, MyEntity entity = null)
    {
      this.Destination = goal;
      this.m_destinationCenter = goal.GetDestination();
      this.EndEntity = entity;
      if (this.EndEntity != null)
      {
        this.Destination.SetRelativeTransform(this.EndEntity.PositionComp.WorldMatrixNormalizedInv);
        this.EndEntity.OnClosing += new Action<MyEntity>(this.m_endEntity_OnClosing);
      }
      this.PathfindingHeuristic = new Func<MyNavigationPrimitive, float>(this.Heuristic);
      this.TerminationCriterion = new Func<MyNavigationPrimitive, float>(this.Criterion);
      this.m_ignoredPrimitives = new HashSet<MyHighLevelPrimitive>();
      this.IsValid = true;
    }

    public void Invalidate()
    {
      if (this.EndEntity != null)
      {
        this.EndEntity.OnClosing -= new Action<MyEntity>(this.m_endEntity_OnClosing);
        this.EndEntity = (MyEntity) null;
      }
      foreach (MyHighLevelPrimitive ignoredPrimitive in this.m_ignoredPrimitives)
        ignoredPrimitive.Parent.StopObservingPrimitive(ignoredPrimitive, (IMyHighLevelPrimitiveObserver) this);
      this.m_ignoredPrimitives.Clear();
      this.IsValid = false;
    }

    public bool ShouldReinitPath() => this.TargetMoved();

    public void Reinit()
    {
      if (this.EndEntity == null)
        return;
      this.Destination.UpdateWorldTransform(this.EndEntity.WorldMatrix);
      this.m_destinationCenter = this.Destination.GetDestination();
    }

    public MyPath<MyNavigationPrimitive> FindHighLevelPath(
      MyPathfinding pathfinding,
      MyHighLevelPrimitive startPrimitive)
    {
      MySmartGoal.m_pathfindingStatic = this;
      MyPath<MyNavigationPrimitive> path = pathfinding.FindPath((MyNavigationPrimitive) startPrimitive, MySmartGoal.m_hlPathfindingHeuristic, MySmartGoal.m_hlTerminationCriterion, (Predicate<MyNavigationPrimitive>) null, false);
      pathfinding.LastHighLevelTimestamp = pathfinding.GetCurrentTimestamp();
      MySmartGoal.m_pathfindingStatic = (MySmartGoal) null;
      return path;
    }

    public MyPath<MyNavigationPrimitive> FindPath(
      MyPathfinding pathfinding,
      MyNavigationPrimitive startPrimitive)
    {
      throw new NotImplementedException();
    }

    public void IgnoreHighLevel(MyHighLevelPrimitive primitive)
    {
      if (this.m_ignoredPrimitives.Contains(primitive))
        return;
      primitive.Parent.ObservePrimitive(primitive, (IMyHighLevelPrimitiveObserver) this);
      this.m_ignoredPrimitives.Add(primitive);
    }

    private bool TargetMoved() => Vector3D.DistanceSquared(this.m_destinationCenter, this.Destination.GetDestination()) > 4.0;

    private void m_endEntity_OnClosing(MyEntity obj)
    {
      this.EndEntity = (MyEntity) null;
      this.IsValid = false;
    }

    private float Heuristic(MyNavigationPrimitive primitive) => (float) Vector3D.Distance(primitive.WorldPosition, this.m_destinationCenter);

    private float Criterion(MyNavigationPrimitive primitive) => this.Destination.PointAdmissibility(primitive.WorldPosition, 2f);

    private static float HlHeuristic(MyNavigationPrimitive primitive) => (float) Vector3D.RectangularDistance(primitive.WorldPosition, MySmartGoal.m_pathfindingStatic.m_destinationCenter) * 2f;

    private static float HlCriterion(MyNavigationPrimitive primitive)
    {
      if (!(primitive is MyHighLevelPrimitive highLevelPrimitive) || MySmartGoal.m_pathfindingStatic.m_ignoredPrimitives.Contains(highLevelPrimitive))
        return float.PositiveInfinity;
      float num = MySmartGoal.m_pathfindingStatic.Destination.PointAdmissibility(primitive.WorldPosition, 8.7f);
      if ((double) num < double.PositiveInfinity)
        return num * 4f;
      IMyHighLevelComponent component = highLevelPrimitive.GetComponent();
      return component == null || component.IsFullyExplored ? float.PositiveInfinity : (float) Vector3D.RectangularDistance(primitive.WorldPosition, MySmartGoal.m_pathfindingStatic.m_destinationCenter) * 8f;
    }

    public void DebugDraw()
    {
      this.Destination.DebugDraw();
      foreach (MyNavigationPrimitive ignoredPrimitive in this.m_ignoredPrimitives)
        MyRenderProxy.DebugDrawSphere(ignoredPrimitive.WorldPosition, 0.5f, Color.Red, depthRead: false);
    }
  }
}
