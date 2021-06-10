// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyVoxelHighLevelHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyVoxelHighLevelHelper
  {
    public static readonly bool DO_CONSISTENCY_CHECKS = true;
    private readonly MyVoxelNavigationMesh m_mesh;
    private bool m_cellOpen;
    private readonly MyIntervalList m_triangleList;
    private int m_currentComponentRel;
    private int m_currentComponentMarker;
    private Vector3I m_currentCell;
    private ulong m_packedCoord;
    private readonly List<List<MyVoxelHighLevelHelper.ConnectionInfo>> m_currentCellConnections;
    private static MyVoxelHighLevelHelper m_currentHelper;
    private readonly Dictionary<ulong, MyIntervalList> m_triangleLists;
    private readonly MyVector3ISet m_exploredCells;
    private readonly MyNavmeshComponents m_navmeshComponents;
    private readonly Predicate<MyNavigationPrimitive> m_processTrianglePredicate = new Predicate<MyNavigationPrimitive>(MyVoxelHighLevelHelper.ProcessTriangleForHierarchyStatic);
    private readonly List<MyNavigationTriangle> m_tmpComponentTriangles = new List<MyNavigationTriangle>();
    private readonly List<int> m_tmpNeighbors = new List<int>();
    private static readonly List<ulong> m_removedHLpackedCoord = new List<ulong>();

    public MyVoxelHighLevelHelper(MyVoxelNavigationMesh mesh)
    {
      this.m_mesh = mesh;
      this.m_triangleList = new MyIntervalList();
      this.m_triangleLists = new Dictionary<ulong, MyIntervalList>();
      this.m_exploredCells = new MyVector3ISet();
      this.m_navmeshComponents = new MyNavmeshComponents();
      this.m_currentCellConnections = new List<List<MyVoxelHighLevelHelper.ConnectionInfo>>();
      for (int index = 0; index < 8; ++index)
        this.m_currentCellConnections.Add(new List<MyVoxelHighLevelHelper.ConnectionInfo>());
    }

    public void OpenNewCell(MyCellCoord coord)
    {
      this.m_cellOpen = true;
      this.m_currentCell = coord.CoordInLod;
      this.m_packedCoord = coord.PackId64();
      this.m_triangleList.Clear();
    }

    public void AddTriangle(int triIndex) => this.m_triangleList.Add(triIndex);

    public void CloseCell()
    {
      this.m_cellOpen = false;
      this.m_packedCoord = 0UL;
      this.m_triangleList.Clear();
    }

    public void ProcessCellComponents()
    {
      this.m_triangleLists.Add(this.m_packedCoord, this.m_triangleList.GetCopy());
      MyNavmeshComponents.ClosedCellInfo cellInfo = this.ConstructComponents();
      this.UpdateHighLevelPrimitives(ref cellInfo);
      this.MarkExploredDirections(ref cellInfo);
      for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
      {
        MyHighLevelPrimitive primitive = this.m_mesh.HighLevelGroup.GetPrimitive(cellInfo.StartingIndex + index);
        if (primitive != null)
          primitive.IsExpanded = true;
      }
    }

    private void MarkExploredDirections(ref MyNavmeshComponents.ClosedCellInfo cellInfo)
    {
      foreach (Base6Directions.Direction enumDirection in Base6Directions.EnumDirections)
      {
        Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(enumDirection);
        if (!cellInfo.ExploredDirections.HasFlag((Enum) directionFlag))
        {
          Vector3I intVector = Base6Directions.GetIntVector(enumDirection);
          MyCellCoord myCellCoord = new MyCellCoord();
          myCellCoord.Lod = 0;
          myCellCoord.CoordInLod = this.m_currentCell + intVector;
          if (myCellCoord.CoordInLod.X != -1 && myCellCoord.CoordInLod.Y != -1 && myCellCoord.CoordInLod.Z != -1)
          {
            ulong num = myCellCoord.PackId64();
            if (this.m_triangleLists.ContainsKey(num))
            {
              this.m_navmeshComponents.MarkExplored(num, Base6Directions.GetFlippedDirection(enumDirection));
              cellInfo.ExploredDirections |= Base6Directions.GetDirectionFlag(enumDirection);
            }
          }
        }
      }
      this.m_navmeshComponents.SetExplored(this.m_packedCoord, cellInfo.ExploredDirections);
    }

    private void UpdateHighLevelPrimitives(ref MyNavmeshComponents.ClosedCellInfo cellInfo)
    {
      int startingIndex = cellInfo.StartingIndex;
      foreach (MyNavigationTriangle componentTriangle in this.m_tmpComponentTriangles)
      {
        if (componentTriangle == null)
          ++startingIndex;
        else
          componentTriangle.ComponentIndex = startingIndex;
      }
      this.m_tmpComponentTriangles.Clear();
      if (!cellInfo.NewCell && (int) cellInfo.ComponentNum != (int) cellInfo.OldComponentNum)
      {
        for (int index = 0; index < (int) cellInfo.OldComponentNum; ++index)
          this.m_mesh.HighLevelGroup.RemovePrimitive(cellInfo.OldStartingIndex + index);
      }
      if (cellInfo.NewCell || (int) cellInfo.ComponentNum != (int) cellInfo.OldComponentNum)
      {
        for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
          this.m_mesh.HighLevelGroup.AddPrimitive(cellInfo.StartingIndex + index, this.m_navmeshComponents.GetComponentCenter(index));
      }
      if (!cellInfo.NewCell && (int) cellInfo.ComponentNum == (int) cellInfo.OldComponentNum)
      {
        for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
          this.m_mesh.HighLevelGroup.GetPrimitive(cellInfo.StartingIndex + index).UpdatePosition(this.m_navmeshComponents.GetComponentCenter(index));
      }
      for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
      {
        int num = cellInfo.StartingIndex + index;
        this.m_mesh.HighLevelGroup.GetPrimitive(num).GetNeighbours(this.m_tmpNeighbors);
        foreach (MyVoxelHighLevelHelper.ConnectionInfo connectionInfo in this.m_currentCellConnections[index])
        {
          if (!this.m_tmpNeighbors.Remove(connectionInfo.ComponentIndex))
            this.m_mesh.HighLevelGroup.ConnectPrimitives(num, connectionInfo.ComponentIndex);
        }
        foreach (int tmpNeighbor in this.m_tmpNeighbors)
        {
          MyHighLevelPrimitive primitive = this.m_mesh.HighLevelGroup.TryGetPrimitive(tmpNeighbor);
          if (primitive != null && primitive.IsExpanded)
            this.m_mesh.HighLevelGroup.DisconnectPrimitives(num, tmpNeighbor);
        }
        this.m_tmpNeighbors.Clear();
        this.m_currentCellConnections[index].Clear();
      }
    }

    private MyNavmeshComponents.ClosedCellInfo ConstructComponents()
    {
      long start = this.m_mesh.GetCurrentTimestamp() + 1L;
      long end = start;
      this.m_currentComponentRel = 0;
      this.m_navmeshComponents.OpenCell(this.m_packedCoord);
      this.m_tmpComponentTriangles.Clear();
      foreach (int triangle1 in this.m_triangleList)
      {
        this.m_currentComponentMarker = -2 - this.m_currentComponentRel;
        MyNavigationTriangle triangle2 = this.m_mesh.GetTriangle(triangle1);
        if (!this.m_mesh.VisitedBetween((MyNavigationPrimitive) triangle2, start, end))
        {
          this.m_navmeshComponents.OpenComponent();
          if (this.m_currentComponentRel >= this.m_currentCellConnections.Count)
            this.m_currentCellConnections.Add(new List<MyVoxelHighLevelHelper.ConnectionInfo>());
          MyVoxelHighLevelHelper.m_currentHelper = this;
          this.m_navmeshComponents.AddComponentTriangle(triangle2, triangle2.Center);
          triangle2.ComponentIndex = this.m_currentComponentMarker;
          this.m_tmpComponentTriangles.Add(triangle2);
          this.m_mesh.PrepareTraversal((MyNavigationPrimitive) triangle2, vertexTraversable: this.m_processTrianglePredicate);
          this.m_mesh.PerformTraversal();
          this.m_tmpComponentTriangles.Add((MyNavigationTriangle) null);
          this.m_navmeshComponents.CloseComponent();
          end = this.m_mesh.GetCurrentTimestamp();
          ++this.m_currentComponentRel;
        }
      }
      MyNavmeshComponents.ClosedCellInfo output = new MyNavmeshComponents.ClosedCellInfo();
      this.m_navmeshComponents.CloseAndCacheCell(ref output);
      return output;
    }

    public MyIntervalList TryGetTriangleList(ulong packedCellCoord)
    {
      MyIntervalList myIntervalList = (MyIntervalList) null;
      this.m_triangleLists.TryGetValue(packedCellCoord, out myIntervalList);
      return myIntervalList;
    }

    public void CollectComponents(ulong packedCoord, List<int> output)
    {
      MyNavmeshComponents.CellInfo cellInfo = new MyNavmeshComponents.CellInfo();
      if (!this.m_navmeshComponents.TryGetCell(packedCoord, out cellInfo))
        return;
      for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
        output.Add(cellInfo.StartingIndex + index);
    }

    public IMyHighLevelComponent GetComponent(MyHighLevelPrimitive primitive)
    {
      ulong cellIndex;
      if (!this.m_navmeshComponents.GetComponentCell(primitive.Index, out cellIndex))
        return (IMyHighLevelComponent) null;
      Base6Directions.DirectionFlags exploredDirections;
      if (!this.m_navmeshComponents.GetComponentInfo(primitive.Index, cellIndex, out exploredDirections))
        return (IMyHighLevelComponent) null;
      MyCellCoord myCellCoord = new MyCellCoord();
      myCellCoord.SetUnpack(cellIndex);
      foreach (Base6Directions.Direction enumDirection in Base6Directions.EnumDirections)
      {
        Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(enumDirection);
        if (!exploredDirections.HasFlag((Enum) directionFlag))
        {
          Vector3I position = myCellCoord.CoordInLod + Base6Directions.GetIntVector(enumDirection);
          if (this.m_exploredCells.Contains(ref position))
            exploredDirections |= directionFlag;
        }
      }
      return (IMyHighLevelComponent) new MyVoxelHighLevelHelper.Component(primitive.Index, exploredDirections);
    }

    public void ClearCachedCell(ulong packedCoord)
    {
      this.m_triangleLists.Remove(packedCoord);
      MyNavmeshComponents.CellInfo cellInfo;
      if (!this.m_navmeshComponents.TryGetCell(packedCoord, out cellInfo))
        return;
      for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
      {
        MyHighLevelPrimitive primitive = this.m_mesh.HighLevelGroup.GetPrimitive(cellInfo.StartingIndex + index);
        if (primitive != null)
          primitive.IsExpanded = false;
      }
    }

    public void TryClearCell(ulong packedCoord)
    {
      if (this.m_triangleLists.ContainsKey(packedCoord))
        this.ClearCachedCell(packedCoord);
      this.RemoveExplored(packedCoord);
      MyNavmeshComponents.CellInfo cellInfo1;
      if (!this.m_navmeshComponents.TryGetCell(packedCoord, out cellInfo1))
        return;
      for (int index = 0; index < (int) cellInfo1.ComponentNum; ++index)
        this.m_mesh.HighLevelGroup.RemovePrimitive(cellInfo1.StartingIndex + index);
      foreach (Base6Directions.Direction enumDirection in Base6Directions.EnumDirections)
      {
        Base6Directions.DirectionFlags directionFlag1 = Base6Directions.GetDirectionFlag(enumDirection);
        if (cellInfo1.ExploredDirections.HasFlag((Enum) directionFlag1))
        {
          Vector3I intVector = Base6Directions.GetIntVector(enumDirection);
          MyCellCoord myCellCoord = new MyCellCoord();
          myCellCoord.SetUnpack(packedCoord);
          myCellCoord.CoordInLod += intVector;
          MyNavmeshComponents.CellInfo cellInfo2;
          if (this.m_navmeshComponents.TryGetCell(myCellCoord.PackId64(), out cellInfo2))
          {
            Base6Directions.DirectionFlags directionFlag2 = Base6Directions.GetDirectionFlag(Base6Directions.GetFlippedDirection(enumDirection));
            this.m_navmeshComponents.SetExplored(myCellCoord.PackId64(), cellInfo2.ExploredDirections & ~directionFlag2);
          }
        }
      }
      this.m_navmeshComponents.ClearCell(packedCoord, ref cellInfo1);
    }

    public MyHighLevelPrimitive GetHighLevelNavigationPrimitive(
      MyNavigationTriangle triangle)
    {
      if (triangle.Parent != this.m_mesh)
        return (MyHighLevelPrimitive) null;
      return triangle.ComponentIndex != -1 ? this.m_mesh.HighLevelGroup.GetPrimitive(triangle.ComponentIndex) : (MyHighLevelPrimitive) null;
    }

    public void AddExplored(ref Vector3I cellPos) => this.m_exploredCells.Add(ref cellPos);

    private void RemoveExplored(ulong packedCoord)
    {
      MyCellCoord myCellCoord = new MyCellCoord();
      myCellCoord.SetUnpack(packedCoord);
      this.m_exploredCells.Remove(ref myCellCoord.CoordInLod);
    }

    private static bool ProcessTriangleForHierarchyStatic(MyNavigationPrimitive primitive)
    {
      MyNavigationTriangle triangle = primitive as MyNavigationTriangle;
      return MyVoxelHighLevelHelper.m_currentHelper.ProcessTriangleForHierarchy(triangle);
    }

    private bool ProcessTriangleForHierarchy(MyNavigationTriangle triangle)
    {
      if (triangle.Parent != this.m_mesh)
        return false;
      if (triangle.ComponentIndex == -1)
      {
        this.m_navmeshComponents.AddComponentTriangle(triangle, triangle.Center);
        this.m_tmpComponentTriangles.Add(triangle);
        triangle.ComponentIndex = this.m_currentComponentMarker;
        return true;
      }
      if (triangle.ComponentIndex == this.m_currentComponentMarker)
        return true;
      ulong cellIndex;
      if (this.m_navmeshComponents.GetComponentCell(triangle.ComponentIndex, out cellIndex))
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        myCellCoord.SetUnpack(cellIndex);
        Vector3I vec = myCellCoord.CoordInLod - this.m_currentCell;
        if (vec.RectangularLength() != 1)
          return false;
        MyVoxelHighLevelHelper.ConnectionInfo connectionInfo = new MyVoxelHighLevelHelper.ConnectionInfo();
        connectionInfo.Direction = Base6Directions.GetDirection(vec);
        connectionInfo.ComponentIndex = triangle.ComponentIndex;
        if (!this.m_currentCellConnections[this.m_currentComponentRel].Contains(connectionInfo))
          this.m_currentCellConnections[this.m_currentComponentRel].Add(connectionInfo);
      }
      return false;
    }

    [Conditional("DEBUG")]
    public void CheckConsistency()
    {
      if (!MyVoxelHighLevelHelper.DO_CONSISTENCY_CHECKS)
        return;
      MyCellCoord myCellCoord = new MyCellCoord();
      foreach (KeyValuePair<ulong, MyIntervalList> triangleList in this.m_triangleLists)
        myCellCoord.SetUnpack(triangleList.Key);
    }

    public void DebugDraw()
    {
      if (MyFakes.DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS)
      {
        foreach (Vector3I exploredCell in this.m_exploredCells)
        {
          BoundingBoxD worldAABB;
          MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(this.m_mesh.VoxelMapReferencePosition, ref exploredCell, out worldAABB);
          MyRenderProxy.DebugDrawAABB(worldAABB, Color.Sienna, depthRead: false);
        }
      }
      if (!MyFakes.DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS)
        return;
      foreach (ulong presentCell in (IEnumerable<ulong>) this.m_navmeshComponents.GetPresentCells())
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        myCellCoord.SetUnpack(presentCell);
        Vector3I coordInLod = myCellCoord.CoordInLod;
        if (this.m_exploredCells.Contains(ref coordInLod))
        {
          MyNavmeshComponents.CellInfo cellInfo = new MyNavmeshComponents.CellInfo();
          if (this.m_navmeshComponents.TryGetCell(presentCell, out cellInfo))
          {
            for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
            {
              MyHighLevelPrimitive primitive = this.m_mesh.HighLevelGroup.GetPrimitive(cellInfo.StartingIndex + index);
              foreach (Base6Directions.Direction enumDirection in Base6Directions.EnumDirections)
              {
                Base6Directions.DirectionFlags directionFlag = Base6Directions.GetDirectionFlag(enumDirection);
                if (!cellInfo.ExploredDirections.HasFlag((Enum) directionFlag) && !this.m_exploredCells.Contains(coordInLod + Base6Directions.GetIntVector(enumDirection)))
                {
                  Vector3 vector = Base6Directions.GetVector(enumDirection);
                  MyRenderProxy.DebugDrawLine3D(primitive.WorldPosition, primitive.WorldPosition + vector * 3f, Color.Red, Color.Red, false);
                }
              }
            }
          }
        }
      }
    }

    public void RemoveTooFarCells(
      List<Vector3D> importantPositions,
      float maxDistance,
      MyVector3ISet processedCells)
    {
      MyVoxelHighLevelHelper.m_removedHLpackedCoord.Clear();
      foreach (Vector3I exploredCell in this.m_exploredCells)
      {
        Vector3D worldPos;
        MyVoxelCoordSystems.GeometryCellCenterCoordToWorldPos(this.m_mesh.VoxelMapReferencePosition, ref exploredCell, out worldPos);
        float num1 = float.PositiveInfinity;
        foreach (Vector3D importantPosition in importantPositions)
        {
          float num2 = Vector3.RectangularDistance((Vector3) importantPosition, (Vector3) worldPos);
          if ((double) num2 < (double) num1)
            num1 = num2;
        }
        if ((double) num1 > (double) maxDistance && !processedCells.Contains(exploredCell))
        {
          MyCellCoord myCellCoord = new MyCellCoord(0, exploredCell);
          MyVoxelHighLevelHelper.m_removedHLpackedCoord.Add(myCellCoord.PackId64());
        }
      }
      foreach (ulong packedCoord in MyVoxelHighLevelHelper.m_removedHLpackedCoord)
        this.TryClearCell(packedCoord);
    }

    public void GetCellsOfPrimitives(
      ref HashSet<ulong> cells,
      ref List<MyHighLevelPrimitive> primitives)
    {
      foreach (MyHighLevelPrimitive highLevelPrimitive in primitives)
      {
        ulong cellIndex;
        if (this.m_navmeshComponents.GetComponentCell(highLevelPrimitive.Index, out cellIndex))
          cells.Add(cellIndex);
      }
    }

    public class Component : IMyHighLevelComponent
    {
      private readonly int m_componentIndex;
      private readonly Base6Directions.DirectionFlags m_exploredDirections;

      public Component(int index, Base6Directions.DirectionFlags exploredDirections)
      {
        this.m_componentIndex = index;
        this.m_exploredDirections = exploredDirections;
      }

      bool IMyHighLevelComponent.Contains(MyNavigationPrimitive primitive) => primitive is MyNavigationTriangle navigationTriangle && navigationTriangle.ComponentIndex == this.m_componentIndex;

      bool IMyHighLevelComponent.IsFullyExplored => this.m_exploredDirections == Base6Directions.DirectionFlags.All;
    }

    public struct ConnectionInfo
    {
      public int ComponentIndex;
      public Base6Directions.Direction Direction;
    }
  }
}
