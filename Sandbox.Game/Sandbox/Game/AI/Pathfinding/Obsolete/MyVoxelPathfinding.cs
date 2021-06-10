// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyVoxelPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyVoxelPathfinding
  {
    private int m_updateCtr;
    private const int UPDATE_PERIOD = 5;
    private readonly Dictionary<MyVoxelBase, MyVoxelNavigationMesh> m_navigationMeshes;
    private readonly List<Vector3D> m_tmpUpdatePositions;
    private readonly List<MyVoxelBase> m_tmpVoxelMaps;
    private readonly List<MyVoxelNavigationMesh> m_tmpNavmeshes;
    private readonly MyNavmeshCoordinator m_coordinator;
    public MyVoxelPathfindingLog DebugLog;
    private static float MESH_DIST = 40f;

    public MyVoxelPathfinding(MyNavmeshCoordinator coordinator)
    {
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      this.m_navigationMeshes = new Dictionary<MyVoxelBase, MyVoxelNavigationMesh>();
      this.m_tmpUpdatePositions = new List<Vector3D>(8);
      this.m_tmpVoxelMaps = new List<MyVoxelBase>();
      this.m_tmpNavmeshes = new List<MyVoxelNavigationMesh>();
      this.m_coordinator = coordinator;
      coordinator.SetVoxelPathfinding(this);
      if (!MyFakes.REPLAY_NAVMESH_GENERATION && !MyFakes.LOG_NAVMESH_GENERATION)
        return;
      this.DebugLog = new MyVoxelPathfindingLog("PathfindingLog.log");
    }

    private void MyEntities_OnEntityAdd(MyEntity entity)
    {
      if (!(entity is MyVoxelBase myVoxelBase) || MyPerGameSettings.Game == GameEnum.SE_GAME && !(myVoxelBase is MyPlanet))
        return;
      this.m_navigationMeshes.Add(myVoxelBase, new MyVoxelNavigationMesh(myVoxelBase, this.m_coordinator, MyCestmirPathfindingShorts.Pathfinding.NextTimestampFunction));
      this.RegisterVoxelMapEvents(myVoxelBase);
    }

    private void RegisterVoxelMapEvents(MyVoxelBase voxelMap) => voxelMap.OnClose += new Action<MyEntity>(this.voxelMap_OnClose);

    private void voxelMap_OnClose(MyEntity entity)
    {
      if (!(entity is MyVoxelBase key) || MyPerGameSettings.Game == GameEnum.SE_GAME && !(key is MyPlanet))
        return;
      this.m_navigationMeshes.Remove(key);
    }

    public void UnloadData()
    {
      if (this.DebugLog != null)
      {
        this.DebugLog.Close();
        this.DebugLog = (MyVoxelPathfindingLog) null;
      }
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
    }

    public void Update()
    {
      ++this.m_updateCtr;
      int num = this.m_updateCtr % 6;
      switch (num)
      {
        case 0:
        case 2:
        case 4:
          if (MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING)
          {
            if (!MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP)
              return;
            MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP = false;
            break;
          }
          break;
      }
      if (MyFakes.REPLAY_NAVMESH_GENERATION)
      {
        this.DebugLog.PerformOneOperation(MyFakes.REPLAY_NAVMESH_GENERATION_TRIGGER);
        MyFakes.REPLAY_NAVMESH_GENERATION_TRIGGER = false;
      }
      else
      {
        switch (num)
        {
          case 0:
            this.GetUpdatePositions();
            this.PerformCellMarking(this.m_tmpUpdatePositions);
            this.PerformCellUpdates();
            this.m_tmpUpdatePositions.Clear();
            break;
          case 2:
            this.GetUpdatePositions();
            this.PerformCellMarking(this.m_tmpUpdatePositions);
            this.PerformCellAdditions(this.m_tmpUpdatePositions);
            this.m_tmpUpdatePositions.Clear();
            break;
          case 4:
            this.GetUpdatePositions();
            this.PerformCellRemovals(this.m_tmpUpdatePositions);
            this.RemoveFarHighLevelGroups(this.m_tmpUpdatePositions);
            this.m_tmpUpdatePositions.Clear();
            break;
        }
      }
    }

    private void GetUpdatePositions()
    {
      this.m_tmpUpdatePositions.Clear();
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
        if (controlledEntity != null)
          this.m_tmpUpdatePositions.Add(controlledEntity.Entity.PositionComp.GetPosition());
      }
    }

    private void PerformCellRemovals(List<Vector3D> updatePositions)
    {
      this.ShuffleMeshes();
      foreach (MyVoxelNavigationMesh tmpNavmesh in this.m_tmpNavmeshes)
      {
        if (tmpNavmesh.RemoveOneUnusedCell(updatePositions))
          break;
      }
      this.m_tmpNavmeshes.Clear();
    }

    private void RemoveFarHighLevelGroups(List<Vector3D> updatePositions)
    {
      foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
        navigationMesh.Value.RemoveFarHighLevelGroups(updatePositions);
    }

    private void PerformCellAdditions(List<Vector3D> updatePositions)
    {
      this.MarkCellsOnPaths();
      this.ShuffleMeshes();
      foreach (MyVoxelNavigationMesh tmpNavmesh in this.m_tmpNavmeshes)
      {
        if (tmpNavmesh.AddOneMarkedCell(updatePositions))
          break;
      }
      this.m_tmpNavmeshes.Clear();
    }

    private void PerformCellUpdates()
    {
      this.ShuffleMeshes();
      foreach (MyVoxelNavigationMesh tmpNavmesh in this.m_tmpNavmeshes)
      {
        if (tmpNavmesh.RefreshOneChangedCell())
          break;
      }
      this.m_tmpNavmeshes.Clear();
    }

    private void ShuffleMeshes()
    {
      this.m_tmpNavmeshes.Clear();
      foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
        this.m_tmpNavmeshes.Add(navigationMesh.Value);
      this.m_tmpNavmeshes.ShuffleList<MyVoxelNavigationMesh>();
    }

    private void PerformCellMarking(List<Vector3D> updatePositions)
    {
      Vector3D vector3D = new Vector3D(1.0);
      foreach (Vector3D updatePosition in updatePositions)
      {
        BoundingBoxD box = new BoundingBoxD(updatePosition - vector3D, updatePosition + vector3D);
        this.m_tmpVoxelMaps.Clear();
        MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, this.m_tmpVoxelMaps);
        foreach (MyVoxelBase tmpVoxelMap in this.m_tmpVoxelMaps)
        {
          MyVoxelNavigationMesh voxelNavigationMesh = (MyVoxelNavigationMesh) null;
          this.m_navigationMeshes.TryGetValue(tmpVoxelMap, out voxelNavigationMesh);
          voxelNavigationMesh?.MarkBoxForAddition(box);
        }
      }
      this.m_tmpVoxelMaps.Clear();
    }

    private void MarkCellsOnPaths()
    {
      foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
        navigationMesh.Value.MarkCellsOnPaths();
    }

    public void InvalidateBox(ref BoundingBoxD bbox)
    {
      foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
      {
        Vector3I min;
        Vector3I max;
        if (navigationMesh.Key.GetContainedVoxelCoords(ref bbox, out min, out max))
          navigationMesh.Value.InvalidateRange(min, max);
      }
    }

    public MyVoxelNavigationMesh GetVoxelMapNavmesh(MyVoxelBase map)
    {
      MyVoxelNavigationMesh voxelNavigationMesh = (MyVoxelNavigationMesh) null;
      this.m_navigationMeshes.TryGetValue(map, out voxelNavigationMesh);
      return voxelNavigationMesh;
    }

    public MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq,
      MyVoxelBase voxelMap = null)
    {
      MyNavigationPrimitive navigationPrimitive = (MyNavigationPrimitive) null;
      if (voxelMap != null)
      {
        MyVoxelNavigationMesh voxelNavigationMesh = (MyVoxelNavigationMesh) null;
        if (this.m_navigationMeshes.TryGetValue(voxelMap, out voxelNavigationMesh))
          navigationPrimitive = voxelNavigationMesh.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
      }
      else
      {
        foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
        {
          MyNavigationPrimitive closestPrimitive = navigationMesh.Value.FindClosestPrimitive(point, highLevel, ref closestDistanceSq);
          if (closestPrimitive != null)
            navigationPrimitive = closestPrimitive;
        }
      }
      return navigationPrimitive;
    }

    [Conditional("DEBUG")]
    public void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      this.DebugLog?.DebugDraw();
      foreach (KeyValuePair<MyVoxelBase, MyVoxelNavigationMesh> navigationMesh in this.m_navigationMeshes)
      {
        MatrixD worldMatrix = navigationMesh.Key.WorldMatrix;
        Matrix matrix = (Matrix) ref worldMatrix;
      }
    }

    [Conditional("DEBUG")]
    public void RemoveTriangle(int index)
    {
      if (this.m_navigationMeshes.Count == 0)
        return;
      foreach (MyVoxelNavigationMesh voxelNavigationMesh in this.m_navigationMeshes.Values)
        voxelNavigationMesh.RemoveTriangle(index);
    }

    public struct CellId : IEquatable<MyVoxelPathfinding.CellId>
    {
      public MyVoxelBase VoxelMap;
      public Vector3I Pos;

      public override bool Equals(object obj) => obj != null && !(obj.GetType() != typeof (MyVoxelPathfinding.CellId)) && this.Equals((MyVoxelPathfinding.CellId) obj);

      public override int GetHashCode() => this.VoxelMap.GetHashCode() * 1610612741 + this.Pos.GetHashCode();

      public bool Equals(MyVoxelPathfinding.CellId other) => this.VoxelMap == other.VoxelMap && this.Pos == other.Pos;
    }
  }
}
