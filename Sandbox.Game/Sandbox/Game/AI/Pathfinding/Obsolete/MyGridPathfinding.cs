// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyGridPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyGridPathfinding
  {
    private readonly Dictionary<MyCubeGrid, MyGridNavigationMesh> m_navigationMeshes;
    private readonly MyNavmeshCoordinator m_coordinator;
    private bool m_highLevelNavigationDirty;

    public MyGridPathfinding(MyNavmeshCoordinator coordinator)
    {
      this.m_navigationMeshes = new Dictionary<MyCubeGrid, MyGridNavigationMesh>();
      this.m_coordinator = coordinator;
      this.m_coordinator.SetGridPathfinding(this);
      this.m_highLevelNavigationDirty = false;
    }

    public void GridAdded(MyCubeGrid grid)
    {
      if (!MyGridPathfinding.GridCanHaveNavmesh(grid))
        return;
      this.m_navigationMeshes.Add(grid, new MyGridNavigationMesh(grid, this.m_coordinator, timestampFunction: MyCestmirPathfindingShorts.Pathfinding.NextTimestampFunction));
      this.RegisterGridEvents(grid);
    }

    private void RegisterGridEvents(MyCubeGrid grid) => grid.OnClose += new Action<MyEntity>(this.grid_OnClose);

    public static bool GridCanHaveNavmesh(MyCubeGrid grid) => false;

    private void grid_OnClose(MyEntity entity)
    {
      if (!(entity is MyCubeGrid myCubeGrid) || !MyGridPathfinding.GridCanHaveNavmesh(myCubeGrid))
        return;
      this.m_coordinator.RemoveGridNavmeshLinks(myCubeGrid);
      this.m_navigationMeshes.Remove(myCubeGrid);
    }

    public void Update()
    {
      if (!this.m_highLevelNavigationDirty)
        return;
      foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in this.m_navigationMeshes)
      {
        MyGridNavigationMesh gridNavigationMesh = navigationMesh.Value;
        if (gridNavigationMesh.HighLevelDirty)
          gridNavigationMesh.UpdateHighLevel();
      }
      this.m_highLevelNavigationDirty = false;
    }

    public List<Vector4D> FindPathGlobal(
      MyCubeGrid startGrid,
      MyCubeGrid endGrid,
      ref Vector3D start,
      ref Vector3D end)
    {
      if (startGrid != endGrid)
        return (List<Vector4D>) null;
      Vector3D vector3D1 = Vector3D.Transform(start, startGrid.PositionComp.WorldMatrixInvScaled);
      Vector3D vector3D2 = Vector3D.Transform(end, endGrid.PositionComp.WorldMatrixInvScaled);
      MyGridNavigationMesh gridNavigationMesh = (MyGridNavigationMesh) null;
      return this.m_navigationMeshes.TryGetValue(startGrid, out gridNavigationMesh) ? gridNavigationMesh.FindPath((Vector3) vector3D1, (Vector3) vector3D2) : (List<Vector4D>) null;
    }

    public MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistSq,
      MyCubeGrid grid = null)
    {
      if (highLevel)
        return (MyNavigationPrimitive) null;
      MyNavigationPrimitive navigationPrimitive = (MyNavigationPrimitive) null;
      if (grid != null)
      {
        MyGridNavigationMesh gridNavigationMesh;
        if (this.m_navigationMeshes.TryGetValue(grid, out gridNavigationMesh))
          navigationPrimitive = gridNavigationMesh.FindClosestPrimitive(point, highLevel, ref closestDistSq);
      }
      else
      {
        foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in this.m_navigationMeshes)
        {
          MyNavigationPrimitive closestPrimitive = navigationMesh.Value.FindClosestPrimitive(point, highLevel, ref closestDistSq);
          if (closestPrimitive != null)
            navigationPrimitive = closestPrimitive;
        }
      }
      return navigationPrimitive;
    }

    public void GetCubeTriangles(
      MyGridPathfinding.CubeId cubeId,
      List<MyNavigationTriangle> trianglesOut)
    {
      ((MyGridNavigationMesh) null)?.GetCubeTriangles(cubeId.Coords, trianglesOut);
    }

    public MyGridNavigationMesh GetNavmesh(MyCubeGrid grid)
    {
      MyGridNavigationMesh gridNavigationMesh = (MyGridNavigationMesh) null;
      this.m_navigationMeshes.TryGetValue(grid, out gridNavigationMesh);
      return gridNavigationMesh;
    }

    public void MarkHighLevelDirty() => this.m_highLevelNavigationDirty = true;

    [Conditional("DEBUG")]
    public void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES == MyWEMDebugDrawMode.NONE)
        return;
      foreach (KeyValuePair<MyCubeGrid, MyGridNavigationMesh> navigationMesh in this.m_navigationMeshes)
      {
        MatrixD worldMatrix = navigationMesh.Key.WorldMatrix;
        Matrix matrix = (Matrix) ref worldMatrix;
        Matrix.Rescale(ref matrix, 2.5f);
      }
    }

    [Conditional("DEBUG")]
    public void RemoveTriangle(int index)
    {
      if (this.m_navigationMeshes.Count == 0)
        return;
      foreach (MyNavigationMesh myNavigationMesh in this.m_navigationMeshes.Values)
        myNavigationMesh.RemoveFace(index);
    }

    public struct CubeId
    {
      public MyCubeGrid Grid;
      public Vector3I Coords;

      public override bool Equals(object obj) => obj is MyGridPathfinding.CubeId cubeId && cubeId.Grid == this.Grid && cubeId.Coords == this.Coords;

      public override int GetHashCode() => this.Grid.GetHashCode() * 1610612741 + this.Coords.GetHashCode();
    }
  }
}
