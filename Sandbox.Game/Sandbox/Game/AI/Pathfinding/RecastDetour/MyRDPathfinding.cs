// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyRDPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class MyRDPathfinding : IMyPathfinding
  {
    private const int DEBUG_PATH_MAX_TICKS = 150;
    private const int TILE_SIZE = 16;
    private const int TILE_HEIGHT = 70;
    private const int TILE_LINE_COUNT = 25;
    private readonly double MIN_NAVMESH_MANAGER_SQUARED_DISTANCE = Math.Pow(160.0, 2.0);
    private readonly Dictionary<MyPlanet, List<MyNavmeshManager>> m_planetManagers = new Dictionary<MyPlanet, List<MyNavmeshManager>>();
    private readonly HashSet<MyCubeGrid> m_grids = new HashSet<MyCubeGrid>();
    private bool m_drawNavmesh;
    private BoundingBoxD? m_debugInvalidateTileAABB;
    private readonly List<MyRDPathfinding.RequestedPath> m_debugDrawPaths = new List<MyRDPathfinding.RequestedPath>();
    private int m_frameCounter;

    public MyRDPathfinding()
    {
      Sandbox.Game.Entities.MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      Sandbox.Game.Entities.MyEntities.OnEntityRemove += new Action<MyEntity>(this.MyEntities_OnEntityRemove);
    }

    public IMyPath FindPathGlobal(
      Vector3D begin,
      IMyDestinationShape end,
      MyEntity relativeEntity)
    {
      MyRDPath myRdPath = new MyRDPath(this, begin, end);
      if (!myRdPath.GetNextTarget(begin, out Vector3D _, out float _, out IMyEntity _) && !myRdPath.IsWaitingForTileGeneration)
        myRdPath = (MyRDPath) null;
      return (IMyPath) myRdPath;
    }

    public bool ReachableUnderThreshold(
      Vector3D begin,
      IMyDestinationShape end,
      float thresholdDistance)
    {
      return true;
    }

    public IMyPathfindingLog GetPathfindingLog() => (IMyPathfindingLog) null;

    public void Update()
    {
      foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in this.m_planetManagers)
      {
        for (int index = 0; index < planetManager.Value.Count; ++index)
        {
          if (!planetManager.Value[index].Update())
          {
            planetManager.Value.RemoveAt(index);
            --index;
          }
        }
      }
      if (this.m_frameCounter++ % 100 != 0)
        return;
      MyRDWrapper.UpdateMemory();
    }

    public void UnloadData()
    {
      Sandbox.Game.Entities.MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      foreach (MyCubeGrid grid in this.m_grids)
      {
        grid.OnBlockAdded -= new Action<MySlimBlock>(this.Grid_OnBlockAdded);
        grid.OnBlockRemoved -= new Action<MySlimBlock>(this.Grid_OnBlockRemoved);
      }
      this.m_grids.Clear();
      foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in this.m_planetManagers)
      {
        foreach (MyNavmeshManager myNavmeshManager in planetManager.Value)
          myNavmeshManager.UnloadData();
      }
      MyRDWrapper.UpdateMemory();
    }

    public void DebugDraw()
    {
      foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in this.m_planetManagers)
      {
        foreach (MyNavmeshManager myNavmeshManager in planetManager.Value)
          myNavmeshManager.DebugDraw();
      }
      if (this.m_debugInvalidateTileAABB.HasValue)
        MyRenderProxy.DebugDrawAABB(this.m_debugInvalidateTileAABB.Value, Color.Yellow, 0.0f);
      this.DebugDrawPaths();
    }

    public static BoundingBoxD GetVoxelAreaAABB(
      MyVoxelBase storage,
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged)
    {
      Vector3D worldPosition1;
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref minVoxelChanged, out worldPosition1);
      Vector3D worldPosition2;
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(storage.PositionLeftBottomCorner, ref maxVoxelChanged, out worldPosition2);
      return new BoundingBoxD(worldPosition1, worldPosition2);
    }

    public List<Vector3D> GetPath(
      MyPlanet planet,
      Vector3D initialPosition,
      Vector3D targetPosition,
      out bool allTilesGenerated)
    {
      if (!this.m_planetManagers.ContainsKey(planet))
      {
        this.m_planetManagers[planet] = new List<MyNavmeshManager>();
        planet.RangeChanged += new MyVoxelBase.StorageChanged(this.VoxelChanged);
      }
      List<Vector3D> pathFromManagers = this.GetBestPathFromManagers(planet, initialPosition, targetPosition, out allTilesGenerated);
      if (pathFromManagers.Count > 0)
        this.m_debugDrawPaths.Add(new MyRDPathfinding.RequestedPath()
        {
          Path = pathFromManagers,
          LocalTicks = 0
        });
      return pathFromManagers;
    }

    public bool AddToTrackedGrids(MyCubeGrid cubeGrid)
    {
      if (!this.m_grids.Add(cubeGrid))
        return false;
      cubeGrid.OnBlockAdded += new Action<MySlimBlock>(this.Grid_OnBlockAdded);
      cubeGrid.OnBlockRemoved += new Action<MySlimBlock>(this.Grid_OnBlockRemoved);
      return true;
    }

    public void InvalidateArea(BoundingBoxD areaBox) => this.AreaChanged(MyRDPathfinding.GetPlanet(areaBox.Center), areaBox);

    public void SetDrawNavmesh(bool drawNavmesh)
    {
      this.m_drawNavmesh = drawNavmesh;
      foreach (KeyValuePair<MyPlanet, List<MyNavmeshManager>> planetManager in this.m_planetManagers)
      {
        foreach (MyNavmeshManager myNavmeshManager in planetManager.Value)
          myNavmeshManager.DrawNavmesh = this.m_drawNavmesh;
      }
    }

    private static MyPlanet GetPlanet(Vector3D position)
    {
      BoundingBoxD box = new BoundingBoxD(position - 250.0, position + 250f);
      return MyGamePruningStructure.GetClosestPlanet(ref box);
    }

    private void MyEntities_OnEntityAdd(MyEntity obj)
    {
      if (!(obj is MyCubeGrid cubeGrid))
        return;
      MyPlanet planet = MyRDPathfinding.GetPlanet(cubeGrid.PositionComp.WorldAABB.Center);
      List<MyNavmeshManager> myNavmeshManagerList;
      if (planet == null || !this.m_planetManagers.TryGetValue(planet, out myNavmeshManagerList))
        return;
      bool flag = false;
      foreach (MyNavmeshManager myNavmeshManager in myNavmeshManagerList)
        flag |= myNavmeshManager.InvalidateArea(cubeGrid.PositionComp.WorldAABB);
      if (!flag)
        return;
      this.AddToTrackedGrids(cubeGrid);
    }

    private void MyEntities_OnEntityRemove(MyEntity obj)
    {
      if (!(obj is MyCubeGrid myCubeGrid) || !this.m_grids.Remove(myCubeGrid))
        return;
      myCubeGrid.OnBlockAdded -= new Action<MySlimBlock>(this.Grid_OnBlockAdded);
      myCubeGrid.OnBlockRemoved -= new Action<MySlimBlock>(this.Grid_OnBlockRemoved);
      MyPlanet planet = MyRDPathfinding.GetPlanet(myCubeGrid.PositionComp.WorldAABB.Center);
      List<MyNavmeshManager> myNavmeshManagerList;
      if (planet == null || !this.m_planetManagers.TryGetValue(planet, out myNavmeshManagerList))
        return;
      foreach (MyNavmeshManager myNavmeshManager in myNavmeshManagerList)
        myNavmeshManager.InvalidateArea(myCubeGrid.PositionComp.WorldAABB);
    }

    private void Grid_OnBlockAdded(MySlimBlock slimBlock)
    {
      MyPlanet planet = MyRDPathfinding.GetPlanet(slimBlock.WorldPosition);
      List<MyNavmeshManager> myNavmeshManagerList;
      if (planet == null || !this.m_planetManagers.TryGetValue(planet, out myNavmeshManagerList))
        return;
      BoundingBoxD worldAabb = slimBlock.WorldAABB;
      foreach (MyNavmeshManager myNavmeshManager in myNavmeshManagerList)
        myNavmeshManager.InvalidateArea(worldAabb);
    }

    private void Grid_OnBlockRemoved(MySlimBlock slimBlock)
    {
      MyPlanet planet = MyRDPathfinding.GetPlanet(slimBlock.WorldPosition);
      List<MyNavmeshManager> myNavmeshManagerList;
      if (planet == null || !this.m_planetManagers.TryGetValue(planet, out myNavmeshManagerList))
        return;
      BoundingBoxD worldAabb = slimBlock.WorldAABB;
      foreach (MyNavmeshManager myNavmeshManager in myNavmeshManagerList)
        myNavmeshManager.InvalidateArea(worldAabb);
    }

    private void VoxelChanged(
      MyVoxelBase storage,
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged,
      MyStorageDataTypeFlags changedData)
    {
      if (!(storage is MyPlanet planet))
        return;
      BoundingBoxD voxelAreaAabb = MyRDPathfinding.GetVoxelAreaAABB((MyVoxelBase) planet, minVoxelChanged, maxVoxelChanged);
      this.AreaChanged(planet, voxelAreaAabb);
      this.m_debugInvalidateTileAABB = new BoundingBoxD?(voxelAreaAabb);
    }

    private void AreaChanged(MyPlanet planet, BoundingBoxD areaBox)
    {
      List<MyNavmeshManager> myNavmeshManagerList;
      if (!this.m_planetManagers.TryGetValue(planet, out myNavmeshManagerList))
        return;
      foreach (MyNavmeshManager myNavmeshManager in myNavmeshManagerList)
        myNavmeshManager.InvalidateArea(areaBox);
    }

    private List<Vector3D> GetBestPathFromManagers(
      MyPlanet planet,
      Vector3D initialPosition,
      Vector3D targetPosition,
      out bool allTilesGenerated)
    {
      allTilesGenerated = true;
      List<MyNavmeshManager> list = this.m_planetManagers[planet].Where<MyNavmeshManager>((Func<MyNavmeshManager, bool>) (m => m.ContainsPosition(initialPosition))).ToList<MyNavmeshManager>();
      if (list.Count > 0)
      {
        List<Vector3D> path;
        bool noTilesToGenerate;
        foreach (MyNavmeshManager myNavmeshManager in list)
        {
          if (myNavmeshManager.ContainsPosition(targetPosition) && myNavmeshManager.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate))
            return path;
        }
        MyNavmeshManager myNavmeshManager1 = (MyNavmeshManager) null;
        double num1 = double.MaxValue;
        foreach (MyNavmeshManager myNavmeshManager2 in list)
        {
          double num2 = (myNavmeshManager2.Center - initialPosition).LengthSquared();
          if (num1 > num2)
          {
            num1 = num2;
            myNavmeshManager1 = myNavmeshManager2;
          }
        }
        int num3 = myNavmeshManager1.GetPathPoints(initialPosition, targetPosition, out path, out noTilesToGenerate) ? 1 : 0;
        allTilesGenerated = noTilesToGenerate;
        if (num3 == 0 & noTilesToGenerate && path.Count <= 2 && num1 > this.MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
        {
          double num2 = (initialPosition - targetPosition).LengthSquared();
          if ((myNavmeshManager1.Center - targetPosition).LengthSquared() - num2 > this.MIN_NAVMESH_MANAGER_SQUARED_DISTANCE)
          {
            this.CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
            allTilesGenerated = false;
          }
        }
        return path;
      }
      this.CreateManager(initialPosition).TilesToGenerate(initialPosition, targetPosition);
      allTilesGenerated = false;
      return new List<Vector3D>();
    }

    private MyNavmeshManager CreateManager(
      Vector3D center,
      Vector3D? forwardDirection = null)
    {
      if (!forwardDirection.HasValue)
        forwardDirection = new Vector3D?(Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(center))));
      MyRecastOptions recastOptions = MyRDPathfinding.GetRecastOptions((MyCharacter) null);
      MyNavmeshManager myNavmeshManager = new MyNavmeshManager(this, center, forwardDirection.Value, 16, 70, 25, recastOptions)
      {
        DrawNavmesh = this.m_drawNavmesh
      };
      this.m_planetManagers[myNavmeshManager.Planet].Add(myNavmeshManager);
      return myNavmeshManager;
    }

    private static MyRecastOptions GetRecastOptions(MyCharacter character)
    {
      if (MyAIComponent.Static.Bots.BotsDictionary.First<KeyValuePair<int, IMyBot>>().Value.BotDefinition.BehaviorSubtype == "Wolf")
        return new MyRecastOptions()
        {
          cellHeight = 0.2f,
          agentHeight = 0.75f,
          agentRadius = 0.25f,
          agentMaxClimb = 0.7f,
          agentMaxSlope = 55f,
          regionMinSize = 1f,
          regionMergeSize = 10f,
          edgeMaxLen = 50f,
          edgeMaxError = 3f,
          vertsPerPoly = 6f,
          detailSampleDist = 6f,
          detailSampleMaxError = 1f,
          partitionType = 1
        };
      return new MyRecastOptions()
      {
        cellHeight = 0.2f,
        agentHeight = 1.5f,
        agentRadius = 0.8f,
        agentMaxClimb = 1.3f,
        agentMaxSlope = 90f,
        regionMinSize = 1f,
        regionMergeSize = 10f,
        edgeMaxLen = 50f,
        edgeMaxError = 3f,
        vertsPerPoly = 6f,
        detailSampleDist = 6f,
        detailSampleMaxError = 1f,
        partitionType = 1
      };
    }

    private static void DebugDrawSinglePath(List<Vector3D> path)
    {
      for (int index = 1; index < path.Count; ++index)
      {
        MyRenderProxy.DebugDrawSphere(path[index], 0.5f, Color.Yellow, 0.0f, false);
        MyRenderProxy.DebugDrawLine3D(path[index - 1], path[index], Color.Yellow, Color.Yellow, false);
      }
    }

    private void DebugDrawPaths()
    {
      for (int index = 0; index < this.m_debugDrawPaths.Count; ++index)
      {
        MyRDPathfinding.RequestedPath debugDrawPath = this.m_debugDrawPaths[index];
        ++debugDrawPath.LocalTicks;
        if (debugDrawPath.LocalTicks > 150)
        {
          this.m_debugDrawPaths.RemoveAt(index);
          --index;
        }
        else
          MyRDPathfinding.DebugDrawSinglePath(debugDrawPath.Path);
      }
    }

    private class RequestedPath
    {
      public List<Vector3D> Path;
      public int LocalTicks;
    }
  }
}
