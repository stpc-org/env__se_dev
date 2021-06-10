// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using System;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyPathfinding : MyPathFindingSystem<MyNavigationPrimitive>, IMyPathfinding
  {
    public readonly Func<long> NextTimestampFunction;
    private MyNavigationPrimitive m_reachEndPrimitive;
    private float m_reachPredicateDistance;

    public MyGridPathfinding GridPathfinding { get; private set; }

    public MyVoxelPathfinding VoxelPathfinding { get; private set; }

    public MyNavmeshCoordinator Coordinator { get; private set; }

    public MyDynamicObstacles Obstacles { get; private set; }

    public long LastHighLevelTimestamp { get; set; }

    private long GenerateNextTimestamp()
    {
      this.CalculateNextTimestamp();
      return this.GetCurrentTimestamp();
    }

    public MyPathfinding()
      : base()
    {
      this.NextTimestampFunction = new Func<long>(this.GenerateNextTimestamp);
      this.Obstacles = new MyDynamicObstacles();
      this.Coordinator = new MyNavmeshCoordinator(this.Obstacles);
      this.GridPathfinding = new MyGridPathfinding(this.Coordinator);
      this.VoxelPathfinding = new MyVoxelPathfinding(this.Coordinator);
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
    }

    public void Update()
    {
      if (!MyPerGameSettings.EnablePathfinding)
        return;
      this.Obstacles.Update();
      this.GridPathfinding.Update();
      this.VoxelPathfinding.Update();
    }

    public IMyPathfindingLog GetPathfindingLog() => (IMyPathfindingLog) this.VoxelPathfinding.DebugLog;

    public void UnloadData()
    {
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      this.VoxelPathfinding.UnloadData();
      this.GridPathfinding = (MyGridPathfinding) null;
      this.VoxelPathfinding = (MyVoxelPathfinding) null;
      this.Coordinator = (MyNavmeshCoordinator) null;
      this.Obstacles.Clear();
      this.Obstacles = (MyDynamicObstacles) null;
    }

    private void MyEntities_OnEntityAdd(MyEntity newEntity)
    {
      this.Obstacles.TryCreateObstacle(newEntity);
      if (!(newEntity is MyCubeGrid grid))
        return;
      this.GridPathfinding.GridAdded(grid);
    }

    public IMyPath FindPathGlobal(Vector3D begin, IMyDestinationShape end, MyEntity entity = null)
    {
      if (!MyPerGameSettings.EnablePathfinding)
        return (IMyPath) null;
      MySmartPath mySmartPath = new MySmartPath(this);
      MySmartGoal goal = new MySmartGoal(end, entity);
      mySmartPath.Init(begin, goal);
      return (IMyPath) mySmartPath;
    }

    private bool ReachablePredicate(MyNavigationPrimitive primitive) => (this.m_reachEndPrimitive.WorldPosition - primitive.WorldPosition).LengthSquared() <= (double) this.m_reachPredicateDistance * (double) this.m_reachPredicateDistance;

    public bool ReachableUnderThreshold(
      Vector3D begin,
      IMyDestinationShape end,
      float thresholdDistance)
    {
      this.m_reachPredicateDistance = thresholdDistance;
      MyNavigationPrimitive closestPrimitive1 = this.FindClosestPrimitive(begin, false);
      MyNavigationPrimitive closestPrimitive2 = this.FindClosestPrimitive(end.GetDestination(), false);
      if (closestPrimitive1 == null || closestPrimitive2 == null)
        return false;
      MyHighLevelPrimitive highLevelPrimitive = closestPrimitive1.GetHighLevelPrimitive();
      closestPrimitive2.GetHighLevelPrimitive();
      if (new MySmartGoal(end).FindHighLevelPath(this, highLevelPrimitive) == null)
        return false;
      this.m_reachEndPrimitive = closestPrimitive2;
      this.PrepareTraversal(closestPrimitive1, vertexTraversable: new Predicate<MyNavigationPrimitive>(this.ReachablePredicate));
      try
      {
        foreach (object obj in (MyPathFindingSystem<MyNavigationPrimitive>) this)
        {
          if (obj.Equals((object) this.m_reachEndPrimitive))
            return true;
        }
      }
      finally
      {
      }
      return false;
    }

    public MyPath<MyNavigationPrimitive> FindPathLowlevel(
      Vector3D begin,
      Vector3D end)
    {
      MyPath<MyNavigationPrimitive> myPath = (MyPath<MyNavigationPrimitive>) null;
      if (!MyPerGameSettings.EnablePathfinding)
        return myPath;
      MyNavigationPrimitive closestPrimitive1 = this.FindClosestPrimitive(begin, false);
      MyNavigationPrimitive closestPrimitive2 = this.FindClosestPrimitive(end, false);
      if (closestPrimitive1 != null && closestPrimitive2 != null)
        myPath = this.FindPath(closestPrimitive1, closestPrimitive2, (Predicate<MyNavigationPrimitive>) null, (Predicate<IMyPathEdge<MyNavigationPrimitive>>) null);
      return myPath;
    }

    public MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      MyEntity entity = null)
    {
      double num = double.PositiveInfinity;
      MyNavigationPrimitive navigationPrimitive = (MyNavigationPrimitive) null;
      MyVoxelMap myVoxelMap = entity as MyVoxelMap;
      MyCubeGrid grid = entity as MyCubeGrid;
      if (myVoxelMap != null)
        navigationPrimitive = this.VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref num, (MyVoxelBase) myVoxelMap);
      else if (grid != null)
      {
        navigationPrimitive = this.GridPathfinding.FindClosestPrimitive(point, highLevel, ref num, grid);
      }
      else
      {
        MyNavigationPrimitive closestPrimitive1 = this.VoxelPathfinding.FindClosestPrimitive(point, highLevel, ref num);
        if (closestPrimitive1 != null)
          navigationPrimitive = closestPrimitive1;
        MyNavigationPrimitive closestPrimitive2 = this.GridPathfinding.FindClosestPrimitive(point, highLevel, ref num);
        if (closestPrimitive2 != null)
          navigationPrimitive = closestPrimitive2;
      }
      return navigationPrimitive;
    }

    public void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != MyWEMDebugDrawMode.NONE)
        this.Coordinator.Links.DebugDraw(Color.Khaki);
      if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
        this.Coordinator.HighLevelLinks.DebugDraw(Color.LightGreen);
      this.Coordinator.DebugDraw();
      this.Obstacles.DebugDraw();
    }
  }
}
