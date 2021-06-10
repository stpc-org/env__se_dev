// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyGridHighLevelHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyGridHighLevelHelper
  {
    private readonly MyGridNavigationMesh m_mesh;
    private readonly Vector3I m_cellSize;
    private ulong m_packedCoord;
    private int m_currentComponentRel;
    private readonly List<List<int>> m_currentCellConnections;
    private readonly MyVector3ISet m_changedCells;
    private readonly MyVector3ISet m_changedCubes;
    private readonly Dictionary<Vector3I, List<int>> m_triangleRegistry;
    private readonly MyNavmeshComponents m_components;
    private readonly List<MyNavigationTriangle> m_tmpComponentTriangles = new List<MyNavigationTriangle>();
    private readonly List<int> m_tmpNeighbors = new List<int>();
    private static readonly HashSet<int> m_tmpCellTriangles = new HashSet<int>();
    private static MyGridHighLevelHelper m_currentHelper;
    private static readonly Vector3I CELL_COORD_SHIFT = new Vector3I(524288);
    private readonly Predicate<MyNavigationPrimitive> m_processTrianglePredicate = new Predicate<MyNavigationPrimitive>(MyGridHighLevelHelper.ProcessTriangleForHierarchyStatic);

    public bool IsDirty => !this.m_changedCells.Empty;

    public MyGridHighLevelHelper(
      MyGridNavigationMesh mesh,
      Dictionary<Vector3I, List<int>> triangleRegistry,
      Vector3I cellSize)
    {
      this.m_mesh = mesh;
      this.m_cellSize = cellSize;
      this.m_packedCoord = 0UL;
      this.m_currentCellConnections = new List<List<int>>();
      this.m_changedCells = new MyVector3ISet();
      this.m_changedCubes = new MyVector3ISet();
      this.m_triangleRegistry = triangleRegistry;
      this.m_components = new MyNavmeshComponents();
    }

    public void MarkBlockChanged(MySlimBlock block)
    {
      Vector3I cube1 = block.Min - Vector3I.One;
      Vector3I cube2 = block.Max + Vector3I.One;
      Vector3I next = cube1;
      Vector3I_RangeIterator vector3IRangeIterator1 = new Vector3I_RangeIterator(ref block.Min, ref block.Max);
      while (vector3IRangeIterator1.IsValid())
      {
        this.m_changedCubes.Add(next);
        vector3IRangeIterator1.GetNext(out next);
      }
      Vector3I cell1 = this.CubeToCell(ref cube1);
      Vector3I cell2 = this.CubeToCell(ref cube2);
      next = cell1;
      Vector3I_RangeIterator vector3IRangeIterator2 = new Vector3I_RangeIterator(ref cell1, ref cell2);
      while (vector3IRangeIterator2.IsValid())
      {
        this.m_changedCells.Add(next);
        vector3IRangeIterator2.GetNext(out next);
      }
    }

    public void ProcessChangedCellComponents()
    {
      MyGridHighLevelHelper.m_currentHelper = this;
      foreach (Vector3I changedCell in this.m_changedCells)
      {
        Vector3I lowestCube = this.CellToLowestCube(changedCell);
        Vector3I end1 = lowestCube + this.m_cellSize - Vector3I.One;
        Vector3I next = lowestCube;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref lowestCube, ref end1);
        while (vector3IRangeIterator.IsValid())
        {
          List<int> intList;
          if (this.m_triangleRegistry.TryGetValue(next, out intList))
          {
            foreach (int num in intList)
              MyGridHighLevelHelper.m_tmpCellTriangles.Add(num);
          }
          vector3IRangeIterator.GetNext(out next);
        }
        if (MyGridHighLevelHelper.m_tmpCellTriangles.Count != 0)
        {
          this.m_components.OpenCell(new MyCellCoord(0, changedCell).PackId64());
          long start = this.m_mesh.GetCurrentTimestamp() + 1L;
          long end2 = start;
          this.m_currentComponentRel = 0;
          this.m_tmpComponentTriangles.Clear();
          foreach (int tmpCellTriangle in MyGridHighLevelHelper.m_tmpCellTriangles)
          {
            MyNavigationTriangle triangle = this.m_mesh.GetTriangle(tmpCellTriangle);
            if (this.m_currentComponentRel == 0 || !this.m_mesh.VisitedBetween((MyNavigationPrimitive) triangle, start, end2))
            {
              this.m_components.OpenComponent();
              if (this.m_currentComponentRel >= this.m_currentCellConnections.Count)
                this.m_currentCellConnections.Add(new List<int>());
              this.m_components.AddComponentTriangle(triangle, triangle.Center);
              triangle.ComponentIndex = this.m_currentComponentRel;
              this.m_tmpComponentTriangles.Add(triangle);
              this.m_mesh.PrepareTraversal((MyNavigationPrimitive) triangle, vertexTraversable: this.m_processTrianglePredicate);
              this.m_mesh.PerformTraversal();
              this.m_tmpComponentTriangles.Add((MyNavigationTriangle) null);
              this.m_components.CloseComponent();
              end2 = this.m_mesh.GetCurrentTimestamp();
              if (this.m_currentComponentRel == 0)
                start = end2;
              ++this.m_currentComponentRel;
            }
          }
          MyGridHighLevelHelper.m_tmpCellTriangles.Clear();
          MyNavmeshComponents.ClosedCellInfo output = new MyNavmeshComponents.ClosedCellInfo();
          this.m_components.CloseAndCacheCell(ref output);
          int startingIndex = output.StartingIndex;
          foreach (MyNavigationTriangle componentTriangle in this.m_tmpComponentTriangles)
          {
            if (componentTriangle == null)
              ++startingIndex;
            else
              componentTriangle.ComponentIndex = startingIndex;
          }
          this.m_tmpComponentTriangles.Clear();
          if (!output.NewCell && (int) output.ComponentNum != (int) output.OldComponentNum)
          {
            for (int index = 0; index < (int) output.OldComponentNum; ++index)
              this.m_mesh.HighLevelGroup.RemovePrimitive(output.OldStartingIndex + index);
          }
          if (output.NewCell || (int) output.ComponentNum != (int) output.OldComponentNum)
          {
            for (int index = 0; index < (int) output.ComponentNum; ++index)
              this.m_mesh.HighLevelGroup.AddPrimitive(output.StartingIndex + index, this.m_components.GetComponentCenter(index));
          }
          if (!output.NewCell && (int) output.ComponentNum == (int) output.OldComponentNum)
          {
            for (int index = 0; index < (int) output.ComponentNum; ++index)
              this.m_mesh.HighLevelGroup.GetPrimitive(output.StartingIndex + index).UpdatePosition(this.m_components.GetComponentCenter(index));
          }
          for (int index = 0; index < (int) output.ComponentNum; ++index)
          {
            int num = output.StartingIndex + index;
            this.m_mesh.HighLevelGroup.GetPrimitive(num).GetNeighbours(this.m_tmpNeighbors);
            foreach (int b in this.m_currentCellConnections[index])
            {
              if (!this.m_tmpNeighbors.Remove(b))
                this.m_mesh.HighLevelGroup.ConnectPrimitives(num, b);
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
          for (int index = 0; index < (int) output.ComponentNum; ++index)
          {
            MyHighLevelPrimitive primitive = this.m_mesh.HighLevelGroup.GetPrimitive(output.StartingIndex + index);
            if (primitive != null)
              primitive.IsExpanded = true;
          }
        }
      }
      this.m_changedCells.Clear();
      MyGridHighLevelHelper.m_currentHelper = (MyGridHighLevelHelper) null;
    }

    private static bool ProcessTriangleForHierarchyStatic(MyNavigationPrimitive primitive)
    {
      MyNavigationTriangle triangle = primitive as MyNavigationTriangle;
      return MyGridHighLevelHelper.m_currentHelper.ProcessTriangleForHierarchy(triangle);
    }

    private bool ProcessTriangleForHierarchy(MyNavigationTriangle triangle)
    {
      if (triangle.Parent != this.m_mesh)
        return false;
      if (MyGridHighLevelHelper.m_tmpCellTriangles.Contains(triangle.Index))
      {
        this.m_components.AddComponentTriangle(triangle, triangle.Center);
        this.m_tmpComponentTriangles.Add(triangle);
        return true;
      }
      if (this.m_components.TryGetComponentCell(triangle.ComponentIndex, out ulong _) && !this.m_currentCellConnections[this.m_currentComponentRel].Contains(triangle.ComponentIndex))
        this.m_currentCellConnections[this.m_currentComponentRel].Add(triangle.ComponentIndex);
      return false;
    }

    public MyHighLevelPrimitive GetHighLevelNavigationPrimitive(
      MyNavigationTriangle triangle)
    {
      if (triangle == null)
        return (MyHighLevelPrimitive) null;
      if (triangle.Parent != this.m_mesh)
        return (MyHighLevelPrimitive) null;
      return triangle.ComponentIndex != -1 ? this.m_mesh.HighLevelGroup.GetPrimitive(triangle.ComponentIndex) : (MyHighLevelPrimitive) null;
    }

    private void TryClearCell(ulong packedCoord)
    {
      MyNavmeshComponents.CellInfo cellInfo;
      if (!this.m_components.TryGetCell(packedCoord, out cellInfo))
        return;
      this.m_components.ClearCell(packedCoord, ref cellInfo);
    }

    private Vector3I CubeToCell(ref Vector3I cube)
    {
      Vector3D v = (Vector3D) cube / (Vector3D) this.m_cellSize;
      Vector3I r;
      Vector3I.Floor(ref v, out r);
      return r + MyGridHighLevelHelper.CELL_COORD_SHIFT;
    }

    private Vector3I CellToLowestCube(Vector3I cell) => (cell - MyGridHighLevelHelper.CELL_COORD_SHIFT) * this.m_cellSize;
  }
}
