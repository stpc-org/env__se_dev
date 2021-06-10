// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyVoxelNavigationMesh
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyVoxelNavigationMesh : MyNavigationMesh
  {
    private static readonly bool DO_CONSISTENCY_CHECKS = false;
    private readonly MyVoxelBase m_voxelMap;
    private Vector3 m_cellSize;
    private readonly MyVector3ISet m_processedCells;
    private HashSet<ulong> m_cellsOnWayCoords;
    private readonly List<Vector3I> m_cellsOnWay;
    private List<MyHighLevelPrimitive> m_primitivesOnPath;
    private readonly MyBinaryHeap<float, MyVoxelNavigationMesh.CellToAddHeapItem> m_toAdd;
    private readonly List<MyVoxelNavigationMesh.CellToAddHeapItem> m_heapItemList;
    private readonly MyVector3ISet m_markedForAddition;
    private static readonly MyDynamicObjectPool<MyVoxelNavigationMesh.CellToAddHeapItem> m_heapItemAllocator = new MyDynamicObjectPool<MyVoxelNavigationMesh.CellToAddHeapItem>(128);
    private static readonly MyVector3ISet m_tmpCellSet = new MyVector3ISet();
    private static List<MyCubeGrid> m_tmpGridList = new List<MyCubeGrid>();
    private static List<MyGridPathfinding.CubeId> m_tmpLinkCandidates = new List<MyGridPathfinding.CubeId>();
    private static Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>> m_tmpCubeLinkCandidates = new Dictionary<MyGridPathfinding.CubeId, List<MyNavigationPrimitive>>();
    private static MyDynamicObjectPool<List<MyNavigationPrimitive>> m_primitiveListPool = new MyDynamicObjectPool<List<MyNavigationPrimitive>>(8);
    private readonly LinkedList<Vector3I> m_cellsToChange;
    private readonly MyVector3ISet m_cellsToChangeSet;
    private static readonly MyUnionFind m_vertexMapping = new MyUnionFind();
    private static readonly List<int> m_tmpIntList = new List<int>();
    private readonly MyVoxelConnectionHelper m_connectionHelper;
    private readonly MyNavmeshCoordinator m_navmeshCoordinator;
    private readonly MyHighLevelGroup m_higherLevel;
    private readonly MyVoxelHighLevelHelper m_higherLevelHelper;
    private static HashSet<Vector3I> m_adjacentCells = new HashSet<Vector3I>();
    private static Dictionary<Vector3I, BoundingBoxD> m_adjacentBBoxes = new Dictionary<Vector3I, BoundingBoxD>();
    private static Vector3D m_halfMeterOffset = new Vector3D(0.5);
    private static BoundingBoxD m_cellBB = new BoundingBoxD();
    private static Vector3D m_bbMinOffset = new Vector3D(-0.125);
    private readonly Vector3I m_maxCellCoord;
    private const float ExploredRemovingDistance = 200f;
    private const float ProcessedRemovingDistance = 50f;
    private const float AddRemoveKoef = 0.5f;
    private const float MaxAddToProcessingDistance = 25f;
    private readonly float LimitAddingWeight = MyVoxelNavigationMesh.GetWeight(25f);
    private const float CellsOnWayAdvance = 8f;
    public static float PresentEntityWeight = 100f;
    public static float RecountCellWeight = 10f;
    public static float JustAddedAdjacentCellWeight = 0.02f;
    public static float TooFarWeight = -100f;
    private Vector3 m_debugPos1;
    private Vector3 m_debugPos2;
    private Vector3 m_debugPos3;
    private Vector3 m_debugPos4;
    private readonly Dictionary<ulong, List<MyVoxelNavigationMesh.DebugDrawEdge>> m_debugCellEdges;
    public const int NAVMESH_LOD = 0;
    private static readonly Vector3I[] m_cornerOffsets = new Vector3I[8]
    {
      new Vector3I(-1, -1, -1),
      new Vector3I(0, -1, -1),
      new Vector3I(-1, 0, -1),
      new Vector3I(0, 0, -1),
      new Vector3I(-1, -1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(0, 0, 0)
    };

    public static MyVoxelBase VoxelMap { get; private set; }

    public Vector3D VoxelMapReferencePosition => this.m_voxelMap.PositionLeftBottomCorner;

    public Vector3D VoxelMapWorldPosition => this.m_voxelMap.PositionComp.GetPosition();

    public MyVoxelNavigationMesh(
      MyVoxelBase voxelMap,
      MyNavmeshCoordinator coordinator,
      Func<long> timestampFunction)
      : base(coordinator.Links, timestampFunction: timestampFunction)
    {
      this.m_voxelMap = voxelMap;
      MyVoxelNavigationMesh.VoxelMap = this.m_voxelMap;
      this.m_processedCells = new MyVector3ISet();
      this.m_cellsOnWayCoords = new HashSet<ulong>();
      this.m_cellsOnWay = new List<Vector3I>();
      this.m_primitivesOnPath = new List<MyHighLevelPrimitive>(128);
      this.m_toAdd = new MyBinaryHeap<float, MyVoxelNavigationMesh.CellToAddHeapItem>(128);
      this.m_heapItemList = new List<MyVoxelNavigationMesh.CellToAddHeapItem>();
      this.m_markedForAddition = new MyVector3ISet();
      this.m_cellsToChange = new LinkedList<Vector3I>();
      this.m_cellsToChangeSet = new MyVector3ISet();
      this.m_connectionHelper = new MyVoxelConnectionHelper();
      this.m_navmeshCoordinator = coordinator;
      this.m_higherLevel = new MyHighLevelGroup((IMyNavigationGroup) this, coordinator.HighLevelLinks, timestampFunction);
      this.m_higherLevelHelper = new MyVoxelHighLevelHelper(this);
      this.m_debugCellEdges = new Dictionary<ulong, List<MyVoxelNavigationMesh.DebugDrawEdge>>();
      voxelMap.Storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.OnStorageChanged);
      this.m_maxCellCoord = this.m_voxelMap.Size / 8 - Vector3I.One;
    }

    public override string ToString() => "Voxel NavMesh: " + this.m_voxelMap.StorageName;

    private void OnStorageChanged(
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged,
      MyStorageDataTypeFlags changedData)
    {
      if (!changedData.HasFlag((Enum) MyStorageDataTypeFlags.Content))
        return;
      this.InvalidateRange(minVoxelChanged, maxVoxelChanged);
    }

    public void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
    {
      minVoxelChanged -= MyPrecalcComponent.InvalidatedRangeInflate;
      maxVoxelChanged += MyPrecalcComponent.InvalidatedRangeInflate;
      this.m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
      Vector3I geometryCellCoord1;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref minVoxelChanged, out geometryCellCoord1);
      Vector3I geometryCellCoord2;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref maxVoxelChanged, out geometryCellCoord2);
      Vector3I next = geometryCellCoord1;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref geometryCellCoord1, ref geometryCellCoord2);
      while (vector3IRangeIterator.IsValid())
      {
        if (this.m_processedCells.Contains(ref next))
        {
          if (!this.m_cellsToChangeSet.Contains(ref next))
          {
            this.m_cellsToChange.AddLast(next);
            this.m_cellsToChangeSet.Add(next);
          }
        }
        else
          this.m_higherLevelHelper.TryClearCell(new MyCellCoord(0, next).PackId64());
        vector3IRangeIterator.GetNext(out next);
      }
    }

    public void MarkBoxForAddition(BoundingBoxD box)
    {
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.m_voxelMap.PositionLeftBottomCorner, ref box.Min, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.m_voxelMap.PositionLeftBottomCorner, ref box.Max, out voxelCoord2);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord1);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord2);
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord1, out voxelCoord1);
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out voxelCoord2);
      Vector3 vector3 = (Vector3) (voxelCoord1 + voxelCoord2) * 0.5f;
      Vector3I next = voxelCoord1 / 1;
      Vector3I end = voxelCoord2 / 1;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        if ((double) Vector3.RectangularDistance((Vector3) next, vector3) <= 1.0)
          this.MarkCellForAddition(next, MyVoxelNavigationMesh.PresentEntityWeight);
        vector3IRangeIterator.GetNext(out next);
      }
    }

    private static float GetWeight(float rectDistance) => (double) rectDistance < 0.0 ? 1f : (float) (1.0 / (1.0 + (double) rectDistance));

    private bool IsCellPosValid(ref Vector3I cellPos) => cellPos.X <= this.m_maxCellCoord.X && cellPos.Y <= this.m_maxCellCoord.Y && cellPos.Z <= this.m_maxCellCoord.Z && new MyCellCoord(0, cellPos).IsCoord64Valid();

    private void MarkCellForAddition(Vector3I cellPos, float weight)
    {
      if (this.m_processedCells.Contains(ref cellPos) || this.m_markedForAddition.Contains(ref cellPos) || !this.IsCellPosValid(ref cellPos))
        return;
      if (!this.m_toAdd.Full)
      {
        this.MarkCellForAdditionInternal(ref cellPos, weight);
      }
      else
      {
        float heapKey = this.m_toAdd.Min().HeapKey;
        if ((double) weight <= (double) heapKey)
          return;
        this.RemoveMinMarkedForAddition();
        this.MarkCellForAdditionInternal(ref cellPos, weight);
      }
    }

    private void MarkCellForAdditionInternal(ref Vector3I cellPos, float weight)
    {
      MyVoxelNavigationMesh.CellToAddHeapItem cellToAddHeapItem = MyVoxelNavigationMesh.m_heapItemAllocator.Allocate();
      cellToAddHeapItem.Position = cellPos;
      this.m_toAdd.Insert(cellToAddHeapItem, weight);
      this.m_markedForAddition.Add(cellPos);
    }

    private void RemoveMinMarkedForAddition()
    {
      MyVoxelNavigationMesh.CellToAddHeapItem cellToAddHeapItem = this.m_toAdd.RemoveMin();
      MyVoxelNavigationMesh.m_heapItemAllocator.Deallocate(cellToAddHeapItem);
      this.m_markedForAddition.Remove(cellToAddHeapItem.Position);
    }

    public bool RefreshOneChangedCell()
    {
      bool flag = false;
      while (!flag && this.m_cellsToChange.Count != 0)
      {
        Vector3I position = this.m_cellsToChange.First.Value;
        this.m_cellsToChange.RemoveFirst();
        this.m_cellsToChangeSet.Remove(ref position);
        if (this.m_processedCells.Contains(ref position))
        {
          this.RemoveCell(position);
          this.MarkCellForAddition(position, MyVoxelNavigationMesh.RecountCellWeight);
          flag = true;
        }
        else
          this.m_higherLevelHelper.TryClearCell(new MyCellCoord(0, position).PackId64());
      }
      return flag;
    }

    public bool AddOneMarkedCell(List<Vector3D> importantPositions)
    {
      bool flag = false;
      foreach (Vector3I position in this.m_cellsOnWay)
      {
        if (!this.m_processedCells.Contains(ref position) && !this.m_markedForAddition.Contains(ref position))
        {
          float cellWeight = this.CalculateCellWeight(importantPositions, position);
          this.MarkCellForAddition(position, cellWeight);
        }
      }
      while (!flag && this.m_toAdd.Count != 0)
      {
        this.m_toAdd.QueryAll(this.m_heapItemList);
        float num = float.NegativeInfinity;
        MyVoxelNavigationMesh.CellToAddHeapItem cellToAddHeapItem = (MyVoxelNavigationMesh.CellToAddHeapItem) null;
        foreach (MyVoxelNavigationMesh.CellToAddHeapItem heapItem in this.m_heapItemList)
        {
          float cellWeight = this.CalculateCellWeight(importantPositions, heapItem.Position);
          if ((double) cellWeight > (double) num)
          {
            num = cellWeight;
            cellToAddHeapItem = heapItem;
          }
          this.m_toAdd.Modify(heapItem, cellWeight);
        }
        this.m_heapItemList.Clear();
        if (cellToAddHeapItem == null || (double) num < (double) this.LimitAddingWeight)
          return flag;
        this.m_toAdd.Remove(cellToAddHeapItem);
        Vector3I position = cellToAddHeapItem.Position;
        MyVoxelNavigationMesh.m_heapItemAllocator.Deallocate(cellToAddHeapItem);
        this.m_markedForAddition.Remove(position);
        MyVoxelNavigationMesh.m_adjacentCells.Clear();
        if (this.AddCell(position, ref MyVoxelNavigationMesh.m_adjacentCells))
        {
          foreach (Vector3I adjacentCell in MyVoxelNavigationMesh.m_adjacentCells)
          {
            float cellWeight = this.CalculateCellWeight(importantPositions, adjacentCell);
            this.MarkCellForAddition(adjacentCell, cellWeight);
          }
          flag = true;
          break;
        }
      }
      return flag;
    }

    private float CalculateCellWeight(List<Vector3D> importantPositions, Vector3I cellPos)
    {
      Vector3I geometryCellCoord = cellPos;
      Vector3D worldPos;
      MyVoxelCoordSystems.GeometryCellCenterCoordToWorldPos(this.m_voxelMap.PositionLeftBottomCorner, ref geometryCellCoord, out worldPos);
      float rectDistance = float.PositiveInfinity;
      foreach (Vector3D importantPosition in importantPositions)
      {
        float num = Vector3.RectangularDistance((Vector3) importantPosition, (Vector3) worldPos);
        if ((double) num < (double) rectDistance)
          rectDistance = num;
      }
      if (this.m_cellsOnWayCoords.Contains(MyCellCoord.PackId64Static(0, cellPos)))
        rectDistance -= 8f;
      return MyVoxelNavigationMesh.GetWeight(rectDistance);
    }

    [Conditional("DEBUG")]
    private void AddDebugOuterEdge(
      ushort a,
      ushort b,
      List<MyVoxelNavigationMesh.DebugDrawEdge> debugEdgesList,
      Vector3D aTformed,
      Vector3D bTformed)
    {
      if (this.m_connectionHelper.IsInnerEdge(a, b))
        return;
      debugEdgesList.Add(new MyVoxelNavigationMesh.DebugDrawEdge((Vector3) aTformed, (Vector3) bTformed));
    }

    private bool AddCell(Vector3I cellPos, ref HashSet<Vector3I> adjacentCellPos)
    {
      if (MyFakes.LOG_NAVMESH_GENERATION)
        MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.DebugLog.LogCellAddition(this, cellPos);
      MyCellCoord myCellCoord = new MyCellCoord(0, cellPos);
      Vector3I vector3I = cellPos * 8 + 8 + 1;
      return true;
    }

    private void PreprocessTriangles(MyIsoMesh generatedMesh, Vector3 centerDisplacement)
    {
      for (int index = 0; index < generatedMesh.TrianglesCount; ++index)
      {
        ushort v0 = generatedMesh.Triangles[index].V0;
        ushort v1 = generatedMesh.Triangles[index].V1;
        ushort v2 = generatedMesh.Triangles[index].V2;
        Vector3 position;
        generatedMesh.GetUnpackedPosition((int) v0, out position);
        Vector3 vector3_1 = position - centerDisplacement;
        generatedMesh.GetUnpackedPosition((int) v1, out position);
        Vector3 vector3_2 = position - centerDisplacement;
        generatedMesh.GetUnpackedPosition((int) v2, out position);
        Vector3 vector3_3 = position - centerDisplacement;
        bool flag = false;
        Vector3 vector3_4 = vector3_2 - vector3_1;
        if ((double) vector3_4.LengthSquared() <= (double) MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
        {
          MyVoxelNavigationMesh.m_vertexMapping.Union((int) v0, (int) v1);
          flag = true;
        }
        vector3_4 = vector3_3 - vector3_1;
        if ((double) vector3_4.LengthSquared() <= (double) MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
        {
          MyVoxelNavigationMesh.m_vertexMapping.Union((int) v0, (int) v2);
          flag = true;
        }
        vector3_4 = vector3_3 - vector3_2;
        if ((double) vector3_4.LengthSquared() <= (double) MyVoxelConnectionHelper.OUTER_EDGE_EPSILON_SQ)
        {
          MyVoxelNavigationMesh.m_vertexMapping.Union((int) v1, (int) v2);
          flag = true;
        }
        if (!flag)
        {
          this.m_connectionHelper.PreprocessInnerEdge(v0, v1);
          this.m_connectionHelper.PreprocessInnerEdge(v1, v2);
          this.m_connectionHelper.PreprocessInnerEdge(v2, v0);
        }
      }
    }

    private bool RemoveCell(Vector3I cell)
    {
      if (!MyFakes.REMOVE_VOXEL_NAVMESH_CELLS)
        return true;
      if (!this.m_processedCells.Contains(cell))
        return false;
      if (MyFakes.LOG_NAVMESH_GENERATION)
        MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.DebugLog.LogCellRemoval(this, cell);
      this.m_navmeshCoordinator.RemoveVoxelNavmeshLinks(new MyVoxelPathfinding.CellId()
      {
        VoxelMap = this.m_voxelMap,
        Pos = cell
      });
      ulong num = new MyCellCoord(0, cell).PackId64();
      MyIntervalList triangleList = this.m_higherLevelHelper.TryGetTriangleList(num);
      if (triangleList != null)
      {
        foreach (int index in triangleList)
          this.RemoveTerrainTriangle(this.GetTriangle(index));
        this.m_higherLevelHelper.ClearCachedCell(num);
      }
      this.m_processedCells.Remove(ref cell);
      return triangleList != null;
    }

    private void RemoveTerrainTriangle(MyNavigationTriangle tri)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = tri.GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      Vector3 current1 = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      Vector3 current2 = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      Vector3 current3 = vertexEnumerator.Current;
      int edgeIndex1 = tri.GetEdgeIndex(0);
      int edgeIndex2 = tri.GetEdgeIndex(1);
      int edgeIndex3 = tri.GetEdgeIndex(2);
      int edgeIndex4 = edgeIndex1;
      if (!this.m_connectionHelper.TryRemoveOuterEdge(ref current1, ref current2, ref edgeIndex4) && this.Mesh.GetEdge(edgeIndex1).OtherFace(tri.Index) != -1)
        this.m_connectionHelper.AddOuterEdgeIndex(ref current2, ref current1, edgeIndex1);
      int edgeIndex5 = edgeIndex2;
      if (!this.m_connectionHelper.TryRemoveOuterEdge(ref current2, ref current3, ref edgeIndex5) && this.Mesh.GetEdge(edgeIndex2).OtherFace(tri.Index) != -1)
        this.m_connectionHelper.AddOuterEdgeIndex(ref current3, ref current2, edgeIndex2);
      int edgeIndex6 = edgeIndex3;
      if (!this.m_connectionHelper.TryRemoveOuterEdge(ref current3, ref current1, ref edgeIndex6) && this.Mesh.GetEdge(edgeIndex3).OtherFace(tri.Index) != -1)
        this.m_connectionHelper.AddOuterEdgeIndex(ref current1, ref current3, edgeIndex3);
      this.RemoveTriangle(tri);
    }

    public void RemoveTriangle(int index) => this.RemoveTerrainTriangle(this.GetTriangle(index));

    public bool RemoveOneUnusedCell(List<Vector3D> importantPositions)
    {
      MyVoxelNavigationMesh.m_tmpCellSet.Clear();
      MyVoxelNavigationMesh.m_tmpCellSet.Union(this.m_processedCells);
      bool flag1 = false;
      foreach (Vector3I tmpCell in MyVoxelNavigationMesh.m_tmpCellSet)
      {
        Vector3I geometryCellCoord = tmpCell * 1;
        Vector3 localPosition;
        MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out localPosition);
        localPosition = (Vector3) (localPosition + new Vector3D(0.5));
        Vector3D worldPosition;
        MyVoxelCoordSystems.LocalPositionToWorldPosition(this.m_voxelMap.PositionLeftBottomCorner, ref localPosition, out worldPosition);
        bool flag2 = true;
        foreach (Vector3D importantPosition in importantPositions)
        {
          if (Vector3D.RectangularDistance(worldPosition, importantPosition) < 50.0)
          {
            flag2 = false;
            break;
          }
        }
        if (flag2 && !this.m_markedForAddition.Contains(tmpCell) && this.RemoveCell(tmpCell))
        {
          Vector3I cellPos = tmpCell;
          float cellWeight = this.CalculateCellWeight(importantPositions, cellPos);
          this.MarkCellForAddition(cellPos, cellWeight);
          flag1 = true;
          break;
        }
      }
      MyVoxelNavigationMesh.m_tmpCellSet.Clear();
      return flag1;
    }

    public void RemoveFarHighLevelGroups(List<Vector3D> updatePositions) => this.m_higherLevelHelper.RemoveTooFarCells(updatePositions, 200f, this.m_processedCells);

    public void MarkCellsOnPaths()
    {
      this.m_primitivesOnPath.Clear();
      this.m_higherLevel.GetPrimitivesOnPath(ref this.m_primitivesOnPath);
      this.m_cellsOnWayCoords.Clear();
      this.m_higherLevelHelper.GetCellsOfPrimitives(ref this.m_cellsOnWayCoords, ref this.m_primitivesOnPath);
      this.m_cellsOnWay.Clear();
      foreach (ulong cellsOnWayCoord in this.m_cellsOnWayCoords)
      {
        MyCellCoord myCellCoord = new MyCellCoord();
        myCellCoord.SetUnpack(cellsOnWayCoord);
        this.m_cellsOnWay.Add(myCellCoord.CoordInLod);
      }
    }

    [Conditional("DEBUG")]
    public void AddCellDebug(Vector3I cellPos)
    {
      HashSet<Vector3I> adjacentCellPos = new HashSet<Vector3I>();
      this.AddCell(cellPos, ref adjacentCellPos);
    }

    [Conditional("DEBUG")]
    public void RemoveCellDebug(Vector3I cellPos) => this.RemoveCell(cellPos);

    public List<Vector4D> FindPathGlobal(Vector3D start, Vector3D end)
    {
      start = Vector3D.Transform(start, this.m_voxelMap.PositionComp.WorldMatrixNormalizedInv);
      end = Vector3D.Transform(end, this.m_voxelMap.PositionComp.WorldMatrixNormalizedInv);
      return this.FindPath((Vector3) start, (Vector3) end);
    }

    public List<Vector4D> FindPath(Vector3 start, Vector3 end)
    {
      float closestDistanceSq1 = float.PositiveInfinity;
      MyNavigationTriangle navigationTriangle1 = this.GetClosestNavigationTriangle(ref start, ref closestDistanceSq1);
      if (navigationTriangle1 == null)
        return (List<Vector4D>) null;
      float closestDistanceSq2 = float.PositiveInfinity;
      MyNavigationTriangle navigationTriangle2 = this.GetClosestNavigationTriangle(ref end, ref closestDistanceSq2);
      if (navigationTriangle2 == null)
        return (List<Vector4D>) null;
      this.m_debugPos1 = (Vector3) Vector3.Transform(navigationTriangle1.Position, this.m_voxelMap.PositionComp.WorldMatrixRef);
      this.m_debugPos2 = (Vector3) Vector3.Transform(navigationTriangle2.Position, this.m_voxelMap.PositionComp.WorldMatrixRef);
      this.m_debugPos3 = (Vector3) Vector3.Transform(start, this.m_voxelMap.PositionComp.WorldMatrixRef);
      this.m_debugPos4 = (Vector3) Vector3.Transform(end, this.m_voxelMap.PositionComp.WorldMatrixRef);
      return this.FindRefinedPath(navigationTriangle1, navigationTriangle2, ref start, ref end);
    }

    private MyNavigationTriangle GetClosestNavigationTriangle(
      ref Vector3 point,
      ref float closestDistanceSq)
    {
      MyNavigationTriangle navigationTriangle = (MyNavigationTriangle) null;
      Vector3I vector3I1 = Vector3I.Round((Vector3) (point + (this.m_voxelMap.PositionComp.GetPosition() - this.m_voxelMap.PositionLeftBottomCorner)) / this.m_cellSize);
      for (int index1 = 0; index1 < 8; ++index1)
      {
        Vector3I vector3I2 = vector3I1 + MyVoxelNavigationMesh.m_cornerOffsets[index1];
        if (this.m_processedCells.Contains(vector3I2))
        {
          MyIntervalList triangleList = this.m_higherLevelHelper.TryGetTriangleList(new MyCellCoord(0, vector3I2).PackId64());
          if (triangleList != null)
          {
            foreach (int index2 in triangleList)
            {
              MyNavigationTriangle triangle = this.GetTriangle(index2);
              float num = Vector3.DistanceSquared(triangle.Center, point);
              if ((double) num < (double) closestDistanceSq)
              {
                closestDistanceSq = num;
                navigationTriangle = triangle;
              }
            }
          }
        }
      }
      return navigationTriangle;
    }

    private MyHighLevelPrimitive GetClosestHighLevelPrimitive(
      ref Vector3 point,
      ref float closestDistanceSq)
    {
      MyHighLevelPrimitive highLevelPrimitive = (MyHighLevelPrimitive) null;
      Vector3 vector3 = (Vector3) (point + (this.m_voxelMap.PositionComp.GetPosition() - this.m_voxelMap.PositionLeftBottomCorner));
      MyVoxelNavigationMesh.m_tmpIntList.Clear();
      Vector3 cellSize = this.m_cellSize;
      Vector3I vector3I = Vector3I.Round(vector3 / cellSize);
      for (int index = 0; index < 8; ++index)
        this.m_higherLevelHelper.CollectComponents(new MyCellCoord(0, vector3I + MyVoxelNavigationMesh.m_cornerOffsets[index]).PackId64(), MyVoxelNavigationMesh.m_tmpIntList);
      foreach (int tmpInt in MyVoxelNavigationMesh.m_tmpIntList)
      {
        MyHighLevelPrimitive primitive = this.m_higherLevel.GetPrimitive(tmpInt);
        if (primitive != null)
        {
          float num = Vector3.DistanceSquared(primitive.Position, point);
          if ((double) num < (double) closestDistanceSq)
          {
            closestDistanceSq = num;
            highLevelPrimitive = primitive;
          }
        }
      }
      MyVoxelNavigationMesh.m_tmpIntList.Clear();
      return highLevelPrimitive;
    }

    public override MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq)
    {
      MatrixD matrix = this.m_voxelMap.PositionComp.WorldMatrixNormalizedInv;
      Vector3 point1 = (Vector3) Vector3D.Transform(point, matrix);
      float closestDistanceSq1 = (float) closestDistanceSq;
      MyNavigationPrimitive navigationPrimitive = !highLevel ? (MyNavigationPrimitive) this.GetClosestNavigationTriangle(ref point1, ref closestDistanceSq1) : (MyNavigationPrimitive) this.GetClosestHighLevelPrimitive(ref point1, ref closestDistanceSq1);
      if (navigationPrimitive != null)
        closestDistanceSq = (double) closestDistanceSq1;
      return navigationPrimitive;
    }

    public override MatrixD GetWorldMatrix() => this.m_voxelMap.WorldMatrix;

    public override Vector3 GlobalToLocal(Vector3D globalPos) => (Vector3) Vector3D.Transform(globalPos, this.m_voxelMap.PositionComp.WorldMatrixNormalizedInv);

    public override Vector3D LocalToGlobal(Vector3 localPos) => Vector3D.Transform(localPos, this.m_voxelMap.WorldMatrix);

    public override MyHighLevelGroup HighLevelGroup => this.m_higherLevel;

    public override MyHighLevelPrimitive GetHighLevelPrimitive(
      MyNavigationPrimitive myNavigationTriangle)
    {
      return this.m_higherLevelHelper.GetHighLevelNavigationPrimitive(myNavigationTriangle as MyNavigationTriangle);
    }

    public override IMyHighLevelComponent GetComponent(
      MyHighLevelPrimitive highLevelPrimitive)
    {
      return this.m_higherLevelHelper.GetComponent(highLevelPrimitive);
    }

    [Conditional("DEBUG")]
    private void CheckOuterEdgeConsistency()
    {
      if (!MyVoxelNavigationMesh.DO_CONSISTENCY_CHECKS)
        return;
      foreach (MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3> myTuple in new List<MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3>>())
      {
        MyWingedEdgeMesh.Edge edge = this.Mesh.GetEdge(myTuple.Item1.EdgeIndex);
        if (myTuple.Item1.FirstPoint)
          edge.GetFaceSuccVertex(-1);
        else
          edge.GetFacePredVertex(-1);
      }
    }

    public override void DebugDraw(ref Matrix drawMatrix)
    {
      if (MyFakes.DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS)
      {
        Vector3 vector3_1 = Vector3.TransformNormal(this.m_cellSize, drawMatrix);
        Vector3 vector3_2 = Vector3.Transform((Vector3) (this.m_voxelMap.PositionLeftBottomCorner - this.m_voxelMap.PositionComp.GetPosition()), drawMatrix);
        foreach (Vector3I processedCell in this.m_processedCells)
        {
          BoundingBoxD aabb;
          aabb.Min = (Vector3D) (vector3_2 + vector3_1 * (new Vector3(1f / 16f) + (Vector3) processedCell));
          aabb.Max = aabb.Min + vector3_1;
          aabb.Inflate(-0.200000002980232);
          MyRenderProxy.DebugDrawAABB(aabb, Color.Orange, depthRead: false);
          MyRenderProxy.DebugDrawText3D(aabb.Center, processedCell.ToString(), Color.Orange, 0.5f, false);
        }
      }
      if (MyFakes.DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS)
      {
        Vector3 vector3_1 = Vector3.TransformNormal(this.m_cellSize, drawMatrix);
        Vector3 vector3_2 = Vector3.Transform((Vector3) (this.m_voxelMap.PositionLeftBottomCorner - this.m_voxelMap.PositionComp.GetPosition()), drawMatrix);
        MyCellCoord myCellCoord = new MyCellCoord();
        foreach (ulong cellsOnWayCoord in this.m_cellsOnWayCoords)
        {
          myCellCoord.SetUnpack(cellsOnWayCoord);
          Vector3I coordInLod = myCellCoord.CoordInLod;
          BoundingBoxD aabb;
          aabb.Min = (Vector3D) (vector3_2 + vector3_1 * (new Vector3(1f / 16f) + (Vector3) coordInLod));
          aabb.Max = aabb.Min + vector3_1;
          aabb.Inflate(-0.300000011920929);
          MyRenderProxy.DebugDrawAABB(aabb, Color.Green, depthRead: false);
        }
      }
      if (MyFakes.DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS)
      {
        Vector3 vector3_1 = Vector3.TransformNormal(this.m_cellSize, drawMatrix);
        Vector3 vector3_2 = Vector3.Transform((Vector3) (this.m_voxelMap.PositionLeftBottomCorner - this.m_voxelMap.PositionComp.GetPosition()), drawMatrix);
        float num = float.NegativeInfinity;
        Vector3I other = Vector3I.Zero;
        for (int index = 0; index < this.m_toAdd.Count; ++index)
        {
          MyVoxelNavigationMesh.CellToAddHeapItem cellToAddHeapItem = this.m_toAdd.GetItem(index);
          float heapKey = cellToAddHeapItem.HeapKey;
          if ((double) heapKey > (double) num)
          {
            num = heapKey;
            other = cellToAddHeapItem.Position;
          }
        }
        for (int index = 0; index < this.m_toAdd.Count; ++index)
        {
          MyVoxelNavigationMesh.CellToAddHeapItem cellToAddHeapItem = this.m_toAdd.GetItem(index);
          float heapKey = cellToAddHeapItem.HeapKey;
          Vector3I position = cellToAddHeapItem.Position;
          BoundingBoxD aabb;
          aabb.Min = (Vector3D) (vector3_2 + vector3_1 * (new Vector3(1f / 16f) + (Vector3) position));
          aabb.Max = aabb.Min + vector3_1;
          aabb.Inflate(-0.100000001490116);
          Color color = Color.Aqua;
          if (position.Equals(other))
            color = Color.Red;
          MyRenderProxy.DebugDrawAABB(aabb, color, depthRead: false);
          string text = heapKey.ToString("n2") ?? "";
          MyRenderProxy.DebugDrawText3D(aabb.Center, text, color, 0.7f, false);
        }
      }
      MyRenderProxy.DebugDrawSphere((Vector3D) this.m_debugPos1, 0.2f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawSphere((Vector3D) this.m_debugPos2, 0.2f, Color.Green, depthRead: false);
      MyRenderProxy.DebugDrawSphere((Vector3D) this.m_debugPos3, 0.1f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawSphere((Vector3D) this.m_debugPos4, 0.1f, Color.Green, depthRead: false);
      if (MyFakes.DEBUG_DRAW_VOXEL_CONNECTION_HELPER)
        this.m_connectionHelper.DebugDraw(ref drawMatrix, this.Mesh);
      if (MyFakes.DEBUG_DRAW_NAVMESH_CELL_BORDERS)
      {
        foreach (KeyValuePair<ulong, List<MyVoxelNavigationMesh.DebugDrawEdge>> debugCellEdge in this.m_debugCellEdges)
        {
          foreach (MyVoxelNavigationMesh.DebugDrawEdge debugDrawEdge in debugCellEdge.Value)
            MyRenderProxy.DebugDrawLine3D((Vector3D) debugDrawEdge.V1, (Vector3D) debugDrawEdge.V2, Color.Orange, Color.Orange, false);
        }
      }
      else
        this.m_debugCellEdges.Clear();
      if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
      {
        if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE)
        {
          this.m_higherLevel.DebugDraw(true);
        }
        else
        {
          this.m_higherLevel.DebugDraw(false);
          this.m_higherLevelHelper.DebugDraw();
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != MyWEMDebugDrawMode.LINES || this.m_voxelMap is MyVoxelPhysics)
        return;
      int num1 = 0;
      MyWingedEdgeMesh.EdgeEnumerator edges = this.Mesh.GetEdges();
      Vector3D position1 = this.m_voxelMap.PositionComp.GetPosition();
      while (edges.MoveNext())
      {
        Vector3D vector3D = (this.Mesh.GetVertexPosition(edges.Current.Vertex1) + position1 + (this.Mesh.GetVertexPosition(edges.Current.Vertex2) + position1)) * 0.5;
        if (MyCestmirPathfindingShorts.Pathfinding.Obstacles.IsInObstacle(vector3D))
          MyRenderProxy.DebugDrawSphere(vector3D, 0.05f, Color.Red, depthRead: false);
        ++num1;
      }
    }

    private class CellToAddHeapItem : HeapItem<float>
    {
      public Vector3I Position;
    }

    private struct DebugDrawEdge
    {
      public readonly Vector3 V1;
      public readonly Vector3 V2;

      public DebugDrawEdge(Vector3 v1, Vector3 v2)
      {
        this.V1 = v1;
        this.V2 = v2;
      }
    }
  }
}
