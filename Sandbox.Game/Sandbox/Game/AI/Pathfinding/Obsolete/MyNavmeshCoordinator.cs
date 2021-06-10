// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyNavmeshCoordinator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyNavmeshCoordinator
  {
    private static readonly List<MyEntity> m_tmpEntityList = new List<MyEntity>();
    private static readonly List<MyGridPathfinding.CubeId> m_tmpLinkCandidates = new List<MyGridPathfinding.CubeId>();
    private static readonly List<MyNavigationTriangle> m_tmpNavTris = new List<MyNavigationTriangle>();
    private static readonly List<MyNavigationPrimitive> m_tmpNavPrims = new List<MyNavigationPrimitive>(4);
    private MyGridPathfinding m_gridPathfinding;
    private MyVoxelPathfinding m_voxelPathfinding;
    private readonly MyDynamicObstacles m_obstacles;
    private readonly Dictionary<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>> m_voxelLinkDictionary = new Dictionary<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>>();
    private readonly Dictionary<MyGridPathfinding.CubeId, int> m_gridLinkCounter = new Dictionary<MyGridPathfinding.CubeId, int>();

    public MyNavgroupLinks Links { get; }

    public MyNavgroupLinks HighLevelLinks { get; }

    public MyNavmeshCoordinator(MyDynamicObstacles obstacles)
    {
      this.Links = new MyNavgroupLinks();
      this.HighLevelLinks = new MyNavgroupLinks();
      this.m_obstacles = obstacles;
    }

    public void SetGridPathfinding(MyGridPathfinding gridPathfinding) => this.m_gridPathfinding = gridPathfinding;

    public void SetVoxelPathfinding(MyVoxelPathfinding myVoxelPathfinding) => this.m_voxelPathfinding = myVoxelPathfinding;

    public void PrepareVoxelTriangleTests(
      BoundingBoxD cellBoundingBox,
      List<MyCubeGrid> gridsToTestOutput)
    {
      MyNavmeshCoordinator.m_tmpEntityList.Clear();
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
      cellBoundingBox.Inflate((double) cubeSize);
      if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
      {
        Vector3D min = cellBoundingBox.Min;
        min.Y -= (double) cubeSize;
        cellBoundingBox.Min = min;
      }
      MyGamePruningStructure.GetAllEntitiesInBox(ref cellBoundingBox, MyNavmeshCoordinator.m_tmpEntityList);
      foreach (MyEntity tmpEntity in MyNavmeshCoordinator.m_tmpEntityList)
      {
        if (tmpEntity is MyCubeGrid grid && MyGridPathfinding.GridCanHaveNavmesh(grid))
          gridsToTestOutput.Add(grid);
      }
      MyNavmeshCoordinator.m_tmpEntityList.Clear();
    }

    public void TestVoxelNavmeshTriangle(
      ref Vector3D a,
      ref Vector3D b,
      ref Vector3D c,
      List<MyCubeGrid> gridsToTest,
      List<MyGridPathfinding.CubeId> linkCandidatesOutput,
      out bool intersecting)
    {
      if (this.m_obstacles.IsInObstacle((a + b + c) / 3.0))
      {
        intersecting = true;
      }
      else
      {
        Vector3D normal = Vector3D.Zero;
        if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
          normal = (Vector3D) (Vector3.Down * 2f);
        MyNavmeshCoordinator.m_tmpLinkCandidates.Clear();
        intersecting = false;
        foreach (MyCubeGrid myCubeGrid in gridsToTest)
        {
          MatrixD matrix = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
          Vector3D result1;
          Vector3D.Transform(ref a, ref matrix, out result1);
          Vector3D result2;
          Vector3D.Transform(ref b, ref matrix, out result2);
          Vector3D result3;
          Vector3D.Transform(ref c, ref matrix, out result3);
          Vector3D result4;
          Vector3D.TransformNormal(ref normal, ref matrix, out result4);
          BoundingBoxD boundingBoxD1 = new BoundingBoxD(Vector3D.MaxValue, Vector3D.MinValue);
          boundingBoxD1.Include(ref result1, ref result2, ref result3);
          Vector3I gridInteger1 = myCubeGrid.LocalToGridInteger((Vector3) boundingBoxD1.Min);
          Vector3I gridInteger2 = myCubeGrid.LocalToGridInteger((Vector3) boundingBoxD1.Max);
          Vector3I next = gridInteger1 - Vector3I.One;
          Vector3I end = gridInteger2 + Vector3I.One;
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref end);
          while (vector3IRangeIterator.IsValid())
          {
            if (myCubeGrid.GetCubeBlock(next) != null)
            {
              Vector3 vector3_1 = ((Vector3) next - Vector3.One) * myCubeGrid.GridSize;
              Vector3 vector3_2 = ((Vector3) next + Vector3.One) * myCubeGrid.GridSize;
              Vector3 vector3_3 = ((Vector3) next - Vector3.Half) * myCubeGrid.GridSize;
              Vector3 vector3_4 = ((Vector3) next + Vector3.Half) * myCubeGrid.GridSize;
              BoundingBoxD boundingBoxD2 = new BoundingBoxD((Vector3D) vector3_1, (Vector3D) vector3_2);
              BoundingBoxD boundingBoxD3 = new BoundingBoxD((Vector3D) vector3_3, (Vector3D) vector3_4);
              boundingBoxD2.Include(vector3_1 + result4);
              boundingBoxD2.Include(vector3_2 + result4);
              boundingBoxD3.Include(vector3_3 + result4);
              boundingBoxD3.Include(vector3_4 + result4);
              if (boundingBoxD2.IntersectsTriangle(ref result1, ref result2, ref result3))
              {
                if (boundingBoxD3.IntersectsTriangle(ref result1, ref result2, ref result3))
                {
                  intersecting = true;
                  break;
                }
                if (Math.Min(Math.Abs(gridInteger1.X - next.X), Math.Abs(gridInteger2.X - next.X)) + Math.Min(Math.Abs(gridInteger1.Y - next.Y), Math.Abs(gridInteger2.Y - next.Y)) + Math.Min(Math.Abs(gridInteger1.Z - next.Z), Math.Abs(gridInteger2.Z - next.Z)) < 3)
                  MyNavmeshCoordinator.m_tmpLinkCandidates.Add(new MyGridPathfinding.CubeId()
                  {
                    Grid = myCubeGrid,
                    Coords = next
                  });
              }
            }
            vector3IRangeIterator.GetNext(out next);
          }
          if (intersecting)
            break;
        }
        if (!intersecting)
        {
          for (int index = 0; index < MyNavmeshCoordinator.m_tmpLinkCandidates.Count; ++index)
            linkCandidatesOutput.Add(MyNavmeshCoordinator.m_tmpLinkCandidates[index]);
        }
        MyNavmeshCoordinator.m_tmpLinkCandidates.Clear();
      }
    }

    public void TryAddVoxelNavmeshLinks(
      MyNavigationTriangle addedPrimitive,
      MyVoxelPathfinding.CellId cellId,
      List<MyGridPathfinding.CubeId> linkCandidates)
    {
      MyNavmeshCoordinator.m_tmpNavTris.Clear();
      foreach (MyGridPathfinding.CubeId linkCandidate in linkCandidates)
      {
        this.m_gridPathfinding.GetCubeTriangles(linkCandidate, MyNavmeshCoordinator.m_tmpNavTris);
        double num = double.MaxValue;
        MyNavigationTriangle navigationTriangle = (MyNavigationTriangle) null;
        foreach (MyNavigationTriangle tmpNavTri in MyNavmeshCoordinator.m_tmpNavTris)
        {
          Vector3D vector3D = addedPrimitive.WorldPosition - tmpNavTri.WorldPosition;
          if (MyPerGameSettings.NavmeshPresumesDownwardGravity && Math.Abs(vector3D.Y) < 0.3 && vector3D.LengthSquared() < num)
          {
            num = vector3D.LengthSquared();
            navigationTriangle = tmpNavTri;
          }
        }
        if (navigationTriangle != null)
        {
          bool flag = true;
          List<MyNavigationPrimitive> links = this.Links.GetLinks((MyNavigationPrimitive) navigationTriangle);
          List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
          this.m_voxelLinkDictionary.TryGetValue(cellId, out navigationPrimitiveList);
          if (links != null)
          {
            MyNavmeshCoordinator.m_tmpNavPrims.Clear();
            this.CollectClosePrimitives((MyNavigationPrimitive) addedPrimitive, MyNavmeshCoordinator.m_tmpNavPrims, 2);
            for (int index = 0; index < MyNavmeshCoordinator.m_tmpNavPrims.Count; ++index)
            {
              if (links.Contains(MyNavmeshCoordinator.m_tmpNavPrims[index]) && navigationPrimitiveList != null && navigationPrimitiveList.Contains(MyNavmeshCoordinator.m_tmpNavPrims[index]))
              {
                if ((MyNavmeshCoordinator.m_tmpNavPrims[index].WorldPosition - navigationTriangle.WorldPosition).LengthSquared() < num)
                {
                  flag = false;
                  break;
                }
                this.Links.RemoveLink((MyNavigationPrimitive) navigationTriangle, MyNavmeshCoordinator.m_tmpNavPrims[index]);
                if (this.Links.GetLinkCount(MyNavmeshCoordinator.m_tmpNavPrims[index]) == 0)
                  this.RemoveVoxelLinkFromDictionary(cellId, MyNavmeshCoordinator.m_tmpNavPrims[index]);
                this.DecreaseGridLinkCounter(linkCandidate);
              }
            }
            MyNavmeshCoordinator.m_tmpNavPrims.Clear();
          }
          if (flag)
          {
            this.Links.AddLink((MyNavigationPrimitive) addedPrimitive, (MyNavigationPrimitive) navigationTriangle);
            this.SaveVoxelLinkToDictionary(cellId, (MyNavigationPrimitive) addedPrimitive);
            this.IncreaseGridLinkCounter(linkCandidate);
          }
        }
        MyNavmeshCoordinator.m_tmpNavTris.Clear();
      }
    }

    public void TryAddVoxelNavmeshLinks2(
      MyVoxelPathfinding.CellId cellId,
      Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> linkCandidates)
    {
      foreach (KeyValuePair<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> linkCandidate in linkCandidates)
      {
        double num1 = double.MaxValue;
        MyNavigationTriangle navigationTriangle = (MyNavigationTriangle) null;
        MyNavigationPrimitive navigationPrimitive1 = (MyNavigationPrimitive) null;
        MyNavmeshCoordinator.m_tmpNavTris.Clear();
        this.m_gridPathfinding.GetCubeTriangles(linkCandidate.Key, MyNavmeshCoordinator.m_tmpNavTris);
        foreach (MyNavigationTriangle tmpNavTri in MyNavmeshCoordinator.m_tmpNavTris)
        {
          Vector3 a;
          Vector3 b;
          Vector3 c;
          tmpNavTri.GetVertices(out a, out b, out c);
          a = (Vector3) tmpNavTri.Parent.LocalToGlobal(a);
          b = (Vector3) tmpNavTri.Parent.LocalToGlobal(b);
          c = (Vector3) tmpNavTri.Parent.LocalToGlobal(c);
          Vector3D vector2 = (Vector3D) (c - a).Cross(b - a);
          Vector3D vector3D = (Vector3D) ((a + b + c) / 3f);
          double num2 = (double) Math.Min(a.Y, Math.Min(b.Y, c.Y));
          double num3 = (double) Math.Max(a.Y, Math.Max(b.Y, c.Y));
          double num4 = num2 - 0.25;
          double num5 = num3 + 0.25;
          foreach (MyNavigationPrimitive navigationPrimitive2 in linkCandidate.Value)
          {
            Vector3D worldPosition = navigationPrimitive2.WorldPosition;
            Vector3D vector1 = worldPosition - vector3D;
            double num6 = vector1.Length();
            vector1 /= num6;
            double result;
            Vector3D.Dot(ref vector1, ref vector2, out result);
            if (result > -0.200000002980232 && worldPosition.Y < num5 && worldPosition.Y > num4)
            {
              double num7 = num6 / (result + 0.300000011920929);
              if (num7 < num1)
              {
                num1 = num7;
                navigationTriangle = tmpNavTri;
                navigationPrimitive1 = navigationPrimitive2;
              }
            }
          }
        }
        MyNavmeshCoordinator.m_tmpNavTris.Clear();
        if (navigationTriangle != null)
        {
          this.Links.AddLink(navigationPrimitive1, (MyNavigationPrimitive) navigationTriangle);
          this.SaveVoxelLinkToDictionary(cellId, navigationPrimitive1);
          this.IncreaseGridLinkCounter(linkCandidate.Key);
        }
      }
    }

    public void RemoveVoxelNavmeshLinks(MyVoxelPathfinding.CellId cellId)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      if (!this.m_voxelLinkDictionary.TryGetValue(cellId, out navigationPrimitiveList))
        return;
      foreach (MyNavigationPrimitive primitive in navigationPrimitiveList)
        this.Links.RemoveAllLinks(primitive);
      this.m_voxelLinkDictionary.Remove(cellId);
    }

    public void RemoveGridNavmeshLinks(MyCubeGrid grid)
    {
      MyGridNavigationMesh navmesh = this.m_gridPathfinding.GetNavmesh(grid);
      if (navmesh == null)
        return;
      MyNavmeshCoordinator.m_tmpNavPrims.Clear();
      MyVector3ISet.Enumerator cubes = navmesh.GetCubes();
      while (cubes.MoveNext())
      {
        MyGridPathfinding.CubeId key = new MyGridPathfinding.CubeId()
        {
          Grid = grid,
          Coords = cubes.Current
        };
        if (this.m_gridLinkCounter.TryGetValue(key, out int _))
        {
          MyNavmeshCoordinator.m_tmpNavTris.Clear();
          navmesh.GetCubeTriangles(cubes.Current, MyNavmeshCoordinator.m_tmpNavTris);
          foreach (MyNavigationTriangle tmpNavTri in MyNavmeshCoordinator.m_tmpNavTris)
          {
            this.Links.RemoveAllLinks((MyNavigationPrimitive) tmpNavTri);
            MyHighLevelPrimitive highLevelPrimitive = tmpNavTri.GetHighLevelPrimitive();
            if (!MyNavmeshCoordinator.m_tmpNavPrims.Contains((MyNavigationPrimitive) highLevelPrimitive))
              MyNavmeshCoordinator.m_tmpNavPrims.Add((MyNavigationPrimitive) highLevelPrimitive);
          }
          MyNavmeshCoordinator.m_tmpNavTris.Clear();
          this.m_gridLinkCounter.Remove(key);
        }
      }
      cubes.Dispose();
      foreach (MyNavigationPrimitive tmpNavPrim in MyNavmeshCoordinator.m_tmpNavPrims)
        this.HighLevelLinks.RemoveAllLinks(tmpNavPrim);
      MyNavmeshCoordinator.m_tmpNavPrims.Clear();
    }

    private void SaveVoxelLinkToDictionary(
      MyVoxelPathfinding.CellId cellId,
      MyNavigationPrimitive linkedPrimitive)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      if (!this.m_voxelLinkDictionary.TryGetValue(cellId, out navigationPrimitiveList))
        navigationPrimitiveList = new List<MyNavigationPrimitive>();
      else if (navigationPrimitiveList.Contains(linkedPrimitive))
        return;
      navigationPrimitiveList.Add(linkedPrimitive);
      this.m_voxelLinkDictionary[cellId] = navigationPrimitiveList;
    }

    private void RemoveVoxelLinkFromDictionary(
      MyVoxelPathfinding.CellId cellId,
      MyNavigationPrimitive linkedPrimitive)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      if (!this.m_voxelLinkDictionary.TryGetValue(cellId, out navigationPrimitiveList))
        return;
      navigationPrimitiveList.Remove(linkedPrimitive);
      if (navigationPrimitiveList.Count != 0)
        return;
      this.m_voxelLinkDictionary.Remove(cellId);
    }

    private void IncreaseGridLinkCounter(MyGridPathfinding.CubeId candidate)
    {
      int num1 = 0;
      int num2 = this.m_gridLinkCounter.TryGetValue(candidate, out num1) ? num1 + 1 : 1;
      this.m_gridLinkCounter[candidate] = num2;
    }

    private void DecreaseGridLinkCounter(MyGridPathfinding.CubeId candidate)
    {
      int num1 = 0;
      if (!this.m_gridLinkCounter.TryGetValue(candidate, out num1))
        return;
      int num2 = num1 - 1;
      if (num2 == 0)
        this.m_gridLinkCounter.Remove(candidate);
      else
        this.m_gridLinkCounter[candidate] = num2;
    }

    private void CollectClosePrimitives(
      MyNavigationPrimitive addedPrimitive,
      List<MyNavigationPrimitive> output,
      int depth)
    {
      if (depth < 0)
        return;
      int num1 = output.Count;
      output.Add(addedPrimitive);
      int num2 = output.Count;
      for (int index = 0; index < addedPrimitive.GetOwnNeighborCount(); ++index)
      {
        if (addedPrimitive.GetOwnNeighbor(index) is MyNavigationPrimitive ownNeighbor)
          output.Add(ownNeighbor);
      }
      int count = output.Count;
      for (--depth; depth > 0; --depth)
      {
        for (int index1 = num2; index1 < count; ++index1)
        {
          MyNavigationPrimitive navigationPrimitive = output[index1];
          for (int index2 = 0; index2 < navigationPrimitive.GetOwnNeighborCount(); ++index2)
          {
            MyNavigationPrimitive ownNeighbor = navigationPrimitive.GetOwnNeighbor(index2) as MyNavigationPrimitive;
            bool flag = false;
            for (int index3 = num1; index3 < count; ++index3)
            {
              if (output[index3] == ownNeighbor)
              {
                flag = true;
                break;
              }
            }
            if (!flag && ownNeighbor != null)
              output.Add(ownNeighbor);
          }
        }
        num1 = num2;
        num2 = count;
        count = output.Count;
      }
    }

    public void UpdateVoxelNavmeshCellHighLevelLinks(MyVoxelPathfinding.CellId cellId)
    {
      List<MyNavigationPrimitive> navigationPrimitiveList = (List<MyNavigationPrimitive>) null;
      if (!this.m_voxelLinkDictionary.TryGetValue(cellId, out navigationPrimitiveList))
        return;
      foreach (MyNavigationPrimitive primitive in navigationPrimitiveList)
      {
        MyNavigationPrimitive highLevelPrimitive1 = (MyNavigationPrimitive) primitive.GetHighLevelPrimitive();
        List<MyNavigationPrimitive> links = this.Links.GetLinks(primitive);
        if (links != null)
        {
          foreach (MyNavigationPrimitive navigationPrimitive in links)
          {
            MyNavigationPrimitive highLevelPrimitive2 = (MyNavigationPrimitive) navigationPrimitive.GetHighLevelPrimitive();
            this.HighLevelLinks.AddLink(highLevelPrimitive1, highLevelPrimitive2, true);
          }
        }
      }
    }

    public void InvalidateVoxelsBBox(ref BoundingBoxD bbox) => this.m_voxelPathfinding.InvalidateBox(ref bbox);

    public void DebugDraw()
    {
      if (!MyFakes.DEBUG_DRAW_NAVMESH_LINKS || !MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
        return;
      foreach (KeyValuePair<MyVoxelPathfinding.CellId, List<MyNavigationPrimitive>> voxelLink in this.m_voxelLinkDictionary)
      {
        MyVoxelBase voxelMap = voxelLink.Key.VoxelMap;
        Vector3I pos = voxelLink.Key.Pos;
        BoundingBoxD worldAABB = new BoundingBoxD();
        MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(voxelMap.PositionLeftBottomCorner, ref pos, out worldAABB);
        MyRenderProxy.DebugDrawText3D(worldAABB.Center, "LinkNum: " + (object) voxelLink.Value.Count, Color.Red, 1f, false);
      }
    }
  }
}
