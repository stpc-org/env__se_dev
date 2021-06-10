// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyRDPath
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class MyRDPath : IMyPath
  {
    private readonly MyRDPathfinding m_pathfinding;
    private List<Vector3D> m_pathPoints;
    private int m_currentPointIndex;
    private readonly MyPlanet m_planet;

    public MyRDPath(MyRDPathfinding pathfinding, Vector3D begin, IMyDestinationShape destination)
    {
      this.m_pathPoints = new List<Vector3D>();
      this.m_pathfinding = pathfinding;
      this.Destination = destination;
      this.m_currentPointIndex = 0;
      this.m_planet = MyRDPath.GetClosestPlanet(begin);
      this.IsValid = this.m_planet != null;
    }

    public IMyDestinationShape Destination { get; }

    public IMyEntity EndEntity => (IMyEntity) null;

    public bool IsValid { get; private set; }

    public bool PathCompleted { get; private set; }

    public bool IsWaitingForTileGeneration { get; private set; }

    public void Invalidate() => this.IsValid = false;

    public bool GetNextTarget(
      Vector3D position,
      out Vector3D target,
      out float targetRadius,
      out IMyEntity relativeEntity)
    {
      target = Vector3D.Zero;
      relativeEntity = (IMyEntity) null;
      targetRadius = 0.8f;
      if (!this.IsValid)
        return false;
      if (this.m_pathPoints.Count == 0 || this.PathCompleted || !this.IsValid)
      {
        bool allTilesGenerated;
        this.m_pathPoints = this.m_pathfinding.GetPath(this.m_planet, position, this.Destination.GetDestination(), out allTilesGenerated);
        this.IsWaitingForTileGeneration = !allTilesGenerated;
        if (this.m_pathPoints.Count < 2)
          return false;
        this.m_currentPointIndex = 1;
      }
      int currentPointIndex = this.m_currentPointIndex;
      int num = this.m_pathPoints.Count - 1;
      target = this.m_pathPoints[this.m_currentPointIndex];
      if ((double) Math.Abs(Vector3.Distance((Vector3) target, (Vector3) position)) < (double) targetRadius)
      {
        if (this.m_currentPointIndex == this.m_pathPoints.Count - 1)
        {
          this.PathCompleted = true;
          return false;
        }
        ++this.m_currentPointIndex;
        target = this.m_pathPoints[this.m_currentPointIndex];
      }
      return true;
    }

    public void ReInit(Vector3D position)
    {
    }

    public void DebugDraw()
    {
      if (this.m_pathPoints.Count <= 0)
        return;
      for (int index = 0; index < this.m_pathPoints.Count - 1; ++index)
      {
        Vector3D pathPoint1 = this.m_pathPoints[index];
        Vector3D pathPoint2 = this.m_pathPoints[index + 1];
        Vector3D pointTo = pathPoint2;
        Color blue = Color.Blue;
        Color red = Color.Red;
        MyRenderProxy.DebugDrawLine3D(pathPoint1, pointTo, blue, red, true);
        MyRenderProxy.DebugDrawSphere(pathPoint2, 0.3f, Color.Yellow);
      }
    }

    private static MyPlanet GetClosestPlanet(Vector3D position)
    {
      BoundingBoxD box = new BoundingBoxD(position - 100.0, position + 100f);
      return MyGamePruningStructure.GetClosestPlanet(ref box);
    }
  }
}
