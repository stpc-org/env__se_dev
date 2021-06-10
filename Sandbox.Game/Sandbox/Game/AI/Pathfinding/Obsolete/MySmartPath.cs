// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MySmartPath
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MySmartPath : IMyHighLevelPrimitiveObserver, IMyPath
  {
    private readonly MyPathfinding m_pathfinding;
    private int m_lastInitTime;
    private bool m_valid;
    private readonly List<MyHighLevelPrimitive> m_pathNodes;
    private readonly List<Vector4D> m_expandedPath;
    private int m_pathNodePosition;
    private int m_expandedPathPosition;
    private MyNavigationPrimitive m_currentPrimitive;
    private MyHighLevelPrimitive m_hlBegin;
    private Vector3D m_startPoint;
    private MySmartGoal m_goal;
    private static MySmartPath m_pathfindingStatic;

    public IMyDestinationShape Destination => this.m_goal.Destination;

    public IMyEntity EndEntity => (IMyEntity) this.m_goal.EndEntity;

    public bool IsValid
    {
      get
      {
        if (!this.m_goal.IsValid)
        {
          if (this.m_valid)
            this.Invalidate();
          return false;
        }
        if (this.m_valid)
          return true;
        this.m_goal.Invalidate();
        return false;
      }
    }

    public bool PathCompleted { get; private set; }

    public bool IsWaitingForTileGeneration { get; }

    public MySmartPath(MyPathfinding pathfinding)
    {
      this.m_pathfinding = pathfinding;
      this.m_pathNodes = new List<MyHighLevelPrimitive>();
      this.m_expandedPath = new List<Vector4D>();
    }

    public void Init(Vector3D start, MySmartGoal goal)
    {
      this.m_lastInitTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_startPoint = start;
      this.m_goal = goal;
      this.m_currentPrimitive = this.m_pathfinding.FindClosestPrimitive(start, false);
      if (this.m_currentPrimitive != null)
      {
        this.m_hlBegin = this.m_currentPrimitive.GetHighLevelPrimitive();
        if (this.m_hlBegin != null && !this.m_pathNodes.Contains(this.m_hlBegin))
          this.m_hlBegin.Parent.ObservePrimitive(this.m_hlBegin, (IMyHighLevelPrimitiveObserver) this);
      }
      if (this.m_currentPrimitive == null)
      {
        this.m_currentPrimitive = (MyNavigationPrimitive) null;
        this.Invalidate();
      }
      else
      {
        this.m_pathNodePosition = 0;
        this.m_expandedPathPosition = 0;
        this.m_expandedPath.Clear();
        this.m_pathNodes.Clear();
        this.PathCompleted = false;
        this.m_valid = true;
      }
    }

    public void ReInit(Vector3D newStart)
    {
      MySmartGoal goal = this.m_goal;
      MyEntity endEntity = goal.EndEntity;
      this.ClearPathNodes();
      this.m_expandedPath.Clear();
      this.m_expandedPathPosition = 0;
      this.m_currentPrimitive = (MyNavigationPrimitive) null;
      this.m_hlBegin?.Parent.StopObservingPrimitive(this.m_hlBegin, (IMyHighLevelPrimitiveObserver) this);
      this.m_hlBegin = (MyHighLevelPrimitive) null;
      this.m_valid = false;
      this.m_goal.Reinit();
      this.Init(newStart, goal);
    }

    public bool GetNextTarget(
      Vector3D currentPosition,
      out Vector3D targetWorld,
      out float radius,
      out IMyEntity relativeEntity)
    {
      bool flag = false;
      targetWorld = new Vector3D();
      radius = 1f;
      relativeEntity = (IMyEntity) null;
      if (this.m_pathNodePosition > 1)
        this.ClearFirstPathNode();
      if (this.m_expandedPathPosition >= this.m_expandedPath.Count)
      {
        if (!this.PathCompleted)
          flag = this.ShouldReinitPath();
        if (flag)
          this.ReInit(currentPosition);
        if (!this.IsValid)
          return false;
        this.ExpandPath(currentPosition);
        if (this.m_expandedPath.Count == 0)
          return false;
      }
      if (this.m_expandedPathPosition >= this.m_expandedPath.Count)
        return false;
      Vector4D xyz = this.m_expandedPath[this.m_expandedPathPosition];
      targetWorld = new Vector3D(xyz);
      radius = (float) xyz.W;
      ++this.m_expandedPathPosition;
      if (this.m_expandedPathPosition == this.m_expandedPath.Count && this.m_pathNodePosition >= this.m_pathNodes.Count - 1)
        this.PathCompleted = true;
      relativeEntity = (IMyEntity) null;
      return true;
    }

    public void Invalidate()
    {
      if (!this.m_valid)
        return;
      this.ClearPathNodes();
      this.m_expandedPath.Clear();
      this.m_expandedPathPosition = 0;
      this.m_currentPrimitive = (MyNavigationPrimitive) null;
      if (this.m_goal.IsValid)
        this.m_goal.Invalidate();
      this.m_hlBegin?.Parent.StopObservingPrimitive(this.m_hlBegin, (IMyHighLevelPrimitiveObserver) this);
      this.m_hlBegin = (MyHighLevelPrimitive) null;
      this.m_valid = false;
    }

    private void ExpandPath(Vector3D currentPosition)
    {
      if (this.m_pathNodePosition >= this.m_pathNodes.Count - 1)
        this.GenerateHighLevelPath();
      if (this.m_pathNodePosition >= this.m_pathNodes.Count)
        return;
      MyPath<MyNavigationPrimitive> path = (MyPath<MyNavigationPrimitive>) null;
      bool flag = false;
      this.m_expandedPath.Clear();
      if (this.m_pathNodePosition + 1 < this.m_pathNodes.Count)
      {
        if (this.m_pathNodes[this.m_pathNodePosition].IsExpanded && this.m_pathNodes[this.m_pathNodePosition + 1].IsExpanded)
        {
          IMyHighLevelComponent component = this.m_pathNodes[this.m_pathNodePosition].GetComponent();
          IMyHighLevelComponent otherComponent = this.m_pathNodes[this.m_pathNodePosition + 1].GetComponent();
          path = this.m_pathfinding.FindPath(this.m_currentPrimitive, this.m_goal.PathfindingHeuristic, (Func<MyNavigationPrimitive, float>) (prim => !otherComponent.Contains(prim) ? float.PositiveInfinity : 0.0f), (Predicate<MyNavigationPrimitive>) (prim => component.Contains(prim) || otherComponent.Contains(prim)), true);
        }
      }
      else if (this.m_pathNodes[this.m_pathNodePosition].IsExpanded)
      {
        IMyHighLevelComponent component = this.m_pathNodes[this.m_pathNodePosition].GetComponent();
        path = this.m_pathfinding.FindPath(this.m_currentPrimitive, this.m_goal.PathfindingHeuristic, (Func<MyNavigationPrimitive, float>) (prim => !component.Contains(prim) ? 30f : this.m_goal.TerminationCriterion(prim)), (Predicate<MyNavigationPrimitive>) (prim => component.Contains(prim)), true);
        if (path != null)
        {
          if (path.Count != 0 && component.Contains(path[path.Count - 1].Vertex as MyNavigationPrimitive))
            flag = true;
          else
            this.m_goal.IgnoreHighLevel(this.m_pathNodes[this.m_pathNodePosition]);
        }
      }
      if (path == null || path.Count == 0)
        return;
      Vector3D vector3D = new Vector3D();
      MyNavigationPrimitive vertex = path[path.Count - 1].Vertex as MyNavigationPrimitive;
      Vector3D end;
      if (flag)
      {
        Vector3 bestPoint = (Vector3) this.m_goal.Destination.GetBestPoint(vertex.WorldPosition);
        Vector3 local = vertex.Group.GlobalToLocal((Vector3D) bestPoint);
        Vector3 localPos = vertex.ProjectLocalPoint(local);
        end = vertex.Group.LocalToGlobal(localPos);
      }
      else
        end = vertex.WorldPosition;
      this.RefineFoundPath(ref currentPosition, ref end, path);
      if (!(this.m_pathNodes.Count <= 1 & flag) || this.m_expandedPath.Count <= 0 || (path.Count > 2 || this.m_goal.ShouldReinitPath()))
        return;
      Vector4D vector4D = this.m_expandedPath[this.m_expandedPath.Count - 1];
      if (Vector3D.DistanceSquared(currentPosition, end) >= vector4D.W * vector4D.W / 256.0)
        return;
      this.m_expandedPath.Clear();
    }

    private bool ShouldReinitPath() => MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastInitTime >= 1000 && this.m_goal.ShouldReinitPath();

    private void GenerateHighLevelPath()
    {
      this.ClearPathNodes();
      if (this.m_hlBegin == null)
        return;
      MyPath<MyNavigationPrimitive> highLevelPath = this.m_goal.FindHighLevelPath(this.m_pathfinding, this.m_hlBegin);
      if (highLevelPath == null)
        return;
      foreach (MyPath<MyNavigationPrimitive>.PathNode pathNode in highLevelPath)
      {
        MyHighLevelPrimitive vertex = pathNode.Vertex as MyHighLevelPrimitive;
        this.m_pathNodes.Add(vertex);
        if (vertex != this.m_hlBegin)
          vertex.Parent.ObservePrimitive(vertex, (IMyHighLevelPrimitiveObserver) this);
      }
      this.m_pathNodePosition = 0;
    }

    private void RefineFoundPath(
      ref Vector3D begin,
      ref Vector3D end,
      MyPath<MyNavigationPrimitive> path)
    {
      if (!MyPerGameSettings.EnablePathfinding || path == null)
        return;
      this.m_currentPrimitive = path[path.Count - 1].Vertex as MyNavigationPrimitive;
      if (this.m_hlBegin != null && !this.m_pathNodes.Contains(this.m_hlBegin))
        this.m_hlBegin.Parent.StopObservingPrimitive(this.m_hlBegin, (IMyHighLevelPrimitiveObserver) this);
      this.m_hlBegin = this.m_currentPrimitive.GetHighLevelPrimitive();
      if (this.m_hlBegin != null && !this.m_pathNodes.Contains(this.m_hlBegin))
        this.m_hlBegin.Parent.ObservePrimitive(this.m_hlBegin, (IMyHighLevelPrimitiveObserver) this);
      IMyNavigationGroup myNavigationGroup = (IMyNavigationGroup) null;
      int begin1 = 0;
      Vector3 startPoint = new Vector3();
      Vector3 vector3 = new Vector3();
      for (int position = 0; position < path.Count; ++position)
      {
        MyNavigationPrimitive vertex = path[position].Vertex as MyNavigationPrimitive;
        IMyNavigationGroup group = vertex.Group;
        if (myNavigationGroup == null)
        {
          myNavigationGroup = group;
          startPoint = myNavigationGroup.GlobalToLocal(begin);
        }
        bool flag = position == path.Count - 1;
        int end1;
        Vector3 local;
        if (group != myNavigationGroup)
        {
          end1 = position - 1;
          local = myNavigationGroup.GlobalToLocal(vertex.WorldPosition);
        }
        else if (flag)
        {
          end1 = position;
          local = myNavigationGroup.GlobalToLocal(end);
        }
        else
          continue;
        int count1 = this.m_expandedPath.Count;
        myNavigationGroup.RefinePath(path, this.m_expandedPath, ref startPoint, ref local, begin1, end1);
        int count2 = this.m_expandedPath.Count;
        for (int index = count1; index < count2; ++index)
        {
          Vector3D vector3D = new Vector3D(this.m_expandedPath[index]);
          vector3D = myNavigationGroup.LocalToGlobal((Vector3) vector3D);
          this.m_expandedPath[index] = new Vector4D(vector3D, this.m_expandedPath[index].W);
        }
        if (flag && group != myNavigationGroup)
          this.m_expandedPath.Add(new Vector4D(vertex.WorldPosition, this.m_expandedPath[count2 - 1].W));
        myNavigationGroup = group;
        begin1 = position;
        if (this.m_expandedPath.Count != 0)
          startPoint = group.GlobalToLocal(new Vector3D(this.m_expandedPath[this.m_expandedPath.Count - 1]));
      }
      ++this.m_pathNodePosition;
      this.m_expandedPathPosition = 0;
    }

    private void ClearPathNodes()
    {
      foreach (MyHighLevelPrimitive pathNode in this.m_pathNodes)
      {
        if (pathNode != this.m_hlBegin)
          pathNode.Parent.StopObservingPrimitive(pathNode, (IMyHighLevelPrimitiveObserver) this);
      }
      this.m_pathNodes.Clear();
      this.m_pathNodePosition = 0;
    }

    private void ClearFirstPathNode()
    {
      using (List<MyHighLevelPrimitive>.Enumerator enumerator = this.m_pathNodes.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          MyHighLevelPrimitive current = enumerator.Current;
          if (current != this.m_hlBegin)
            current.Parent.StopObservingPrimitive(current, (IMyHighLevelPrimitiveObserver) this);
        }
      }
      this.m_pathNodes.RemoveAt(0);
      --this.m_pathNodePosition;
    }

    public void DebugDraw()
    {
      MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
      Vector3D? nullable1 = new Vector3D?();
      foreach (MyHighLevelPrimitive pathNode in this.m_pathNodes)
      {
        Vector3D vector3D1 = (Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(pathNode.WorldPosition);
        if (Vector3D.IsZero(vector3D1, 0.001))
          vector3D1 = Vector3D.Down;
        vector3D1.Normalize();
        Vector3D vector3D2 = pathNode.WorldPosition + vector3D1 * -10.0;
        MyRenderProxy.DebugDrawSphere(vector3D2, 1f, Color.IndianRed, depthRead: false);
        MyRenderProxy.DebugDrawLine3D(pathNode.WorldPosition, vector3D2, Color.IndianRed, Color.IndianRed, false);
        if (nullable1.HasValue)
          MyRenderProxy.DebugDrawLine3D(vector3D2, nullable1.Value, Color.IndianRed, Color.IndianRed, false);
        nullable1 = new Vector3D?(vector3D2);
      }
      MyRenderProxy.DebugDrawSphere(this.m_startPoint, 0.5f, Color.HotPink, depthRead: false);
      this.m_goal?.DebugDraw();
      if (!MyFakes.DEBUG_DRAW_FOUND_PATH)
        return;
      Vector3D? nullable2 = new Vector3D?();
      for (int index = 0; index < this.m_expandedPath.Count; ++index)
      {
        Vector3D vector3D = new Vector3D(this.m_expandedPath[index]);
        float w = (float) this.m_expandedPath[index].W;
        Color color = index == this.m_expandedPath.Count - 1 ? Color.OrangeRed : Color.Orange;
        MyRenderProxy.DebugDrawPoint(vector3D, color, false);
        MyRenderProxy.DebugDrawText3D(vector3D + viewMatrix.Right * 0.100000001490116, w.ToString(), color, 0.7f, false);
        if (nullable2.HasValue)
          MyRenderProxy.DebugDrawLine3D(nullable2.Value, vector3D, Color.Pink, Color.Pink, false);
        nullable2 = new Vector3D?(vector3D);
      }
    }
  }
}
