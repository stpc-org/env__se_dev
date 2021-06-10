// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyVoxelGeometry
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyVoxelGeometry
  {
    private static List<Vector3I> m_sweepResultCache = new List<Vector3I>();
    private static List<int> m_overlapElementCache = new List<int>();
    private MyStorageBase m_storage;
    private Vector3I m_cellsCount;
    private readonly Dictionary<ulong, MyVoxelGeometry.CellData> m_cellsByCoordinate = new Dictionary<ulong, MyVoxelGeometry.CellData>();
    private readonly Dictionary<ulong, MyIsoMesh> m_coordinateToMesh = new Dictionary<ulong, MyIsoMesh>();
    private readonly FastResourceLock m_lock = new FastResourceLock();
    private readonly LRUCache<ulong, ulong> m_isEmptyCache = new LRUCache<ulong, ulong>(128);

    public Vector3I CellsCount => this.m_cellsCount;

    public void Init(MyStorageBase storage)
    {
      this.m_storage = storage;
      this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChanged);
      Vector3I size = this.m_storage.Size;
      this.m_cellsCount.X = size.X >> 3;
      this.m_cellsCount.Y = size.Y >> 3;
      this.m_cellsCount.Z = size.Z >> 3;
    }

    public bool Intersects(ref BoundingSphere localSphere)
    {
      BoundingBox invalid1 = BoundingBox.CreateInvalid();
      invalid1.Include(ref localSphere);
      Vector3 min = invalid1.Min;
      Vector3 max = invalid1.Max;
      Vector3I geometryCellCoord1;
      MyVoxelCoordSystems.LocalPositionToGeometryCellCoord(ref min, out geometryCellCoord1);
      Vector3I geometryCellCoord2;
      MyVoxelCoordSystems.LocalPositionToGeometryCellCoord(ref max, out geometryCellCoord2);
      this.ClampCellCoord(ref geometryCellCoord1);
      this.ClampCellCoord(ref geometryCellCoord2);
      MyCellCoord cell1 = new MyCellCoord();
      for (cell1.CoordInLod.X = geometryCellCoord1.X; cell1.CoordInLod.X <= geometryCellCoord2.X; ++cell1.CoordInLod.X)
      {
        for (cell1.CoordInLod.Y = geometryCellCoord1.Y; cell1.CoordInLod.Y <= geometryCellCoord2.Y; ++cell1.CoordInLod.Y)
        {
          for (cell1.CoordInLod.Z = geometryCellCoord1.Z; cell1.CoordInLod.Z <= geometryCellCoord2.Z; ++cell1.CoordInLod.Z)
          {
            BoundingBox localAABB;
            MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref cell1.CoordInLod, out localAABB);
            if (localAABB.Intersects(ref localSphere))
            {
              MyVoxelGeometry.CellData cell2 = this.GetCell(ref cell1);
              if (cell2 != null)
              {
                for (int index = 0; index < cell2.VoxelTrianglesCount; ++index)
                {
                  MyVoxelTriangle voxelTriangle = cell2.VoxelTriangles[index];
                  MyTriangle_Vertices triangle;
                  cell2.GetUnpackedPosition((int) voxelTriangle.V0, out triangle.Vertex0);
                  cell2.GetUnpackedPosition((int) voxelTriangle.V1, out triangle.Vertex1);
                  cell2.GetUnpackedPosition((int) voxelTriangle.V2, out triangle.Vertex2);
                  BoundingBox invalid2 = BoundingBox.CreateInvalid();
                  invalid2.Include(ref triangle.Vertex0);
                  invalid2.Include(ref triangle.Vertex1);
                  invalid2.Include(ref triangle.Vertex2);
                  if (invalid2.Intersects(ref localSphere))
                  {
                    Plane trianglePlane = new Plane(triangle.Vertex0, triangle.Vertex1, triangle.Vertex2);
                    if (MyUtils.GetSphereTriangleIntersection(ref localSphere, ref trianglePlane, ref triangle).HasValue)
                      return true;
                  }
                }
              }
            }
          }
        }
      }
      return false;
    }

    public bool Intersect(
      ref Line localLine,
      out MyIntersectionResultLineTriangle result,
      IntersectionFlags flags)
    {
      MyVoxelGeometry.m_sweepResultCache.Clear();
      MyGridIntersection.Calculate(MyVoxelGeometry.m_sweepResultCache, 8f, (Vector3D) localLine.From, (Vector3D) localLine.To, new Vector3I(0, 0, 0), this.m_cellsCount - 1);
      float? minDistanceUntilNow = new float?();
      MyCellCoord cell1 = new MyCellCoord();
      MyIntersectionResultLineTriangle? result1 = new MyIntersectionResultLineTriangle?();
      for (int index = 0; index < MyVoxelGeometry.m_sweepResultCache.Count; ++index)
      {
        cell1.CoordInLod = MyVoxelGeometry.m_sweepResultCache[index];
        BoundingBox localAABB;
        MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref cell1.CoordInLod, out localAABB);
        float? boundingBoxIntersection = MyUtils.GetLineBoundingBoxIntersection(ref localLine, ref localAABB);
        if (minDistanceUntilNow.HasValue && boundingBoxIntersection.HasValue)
        {
          float? nullable1 = minDistanceUntilNow;
          float num1 = 15.58846f;
          float? nullable2 = nullable1.HasValue ? new float?(nullable1.GetValueOrDefault() + num1) : new float?();
          float num2 = boundingBoxIntersection.Value;
          if ((double) nullable2.GetValueOrDefault() < (double) num2 & nullable2.HasValue)
            break;
        }
        MyVoxelGeometry.CellData cell2 = this.GetCell(ref cell1);
        if (cell2 != null && cell2.VoxelTrianglesCount != 0)
          this.GetCellLineIntersectionOctree(ref result1, ref localLine, ref minDistanceUntilNow, cell2, flags);
      }
      result = result1 ?? new MyIntersectionResultLineTriangle();
      return result1.HasValue;
    }

    private bool TryGetCell(
      MyCellCoord cell,
      out bool isEmpty,
      out MyVoxelGeometry.CellData nonEmptyCell)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        if (this.IsEmpty(ref cell))
        {
          isEmpty = true;
          nonEmptyCell = (MyVoxelGeometry.CellData) null;
          return true;
        }
        if (this.m_cellsByCoordinate.TryGetValue(cell.PackId64(), out nonEmptyCell))
        {
          isEmpty = false;
          return true;
        }
        isEmpty = false;
        nonEmptyCell = (MyVoxelGeometry.CellData) null;
        return false;
      }
    }

    public bool TryGetMesh(MyCellCoord cell, out bool isEmpty, out MyIsoMesh nonEmptyMesh)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        if (this.IsEmpty(ref cell))
        {
          isEmpty = true;
          nonEmptyMesh = (MyIsoMesh) null;
          return true;
        }
        if (this.m_coordinateToMesh.TryGetValue(cell.PackId64(), out nonEmptyMesh))
        {
          isEmpty = false;
          return true;
        }
        isEmpty = false;
        nonEmptyMesh = (MyIsoMesh) null;
        return false;
      }
    }

    public void SetMesh(MyCellCoord cell, MyIsoMesh mesh)
    {
      if (cell.Lod != 0)
        return;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (mesh != null)
          this.m_coordinateToMesh[cell.PackId64()] = mesh;
        else
          this.SetEmpty(ref cell, true);
      }
    }

    private void storage_RangeChanged(
      Vector3I minChanged,
      Vector3I maxChanged,
      MyStorageDataTypeFlags changedData)
    {
      Vector3I voxelCoord1 = minChanged - MyPrecalcComponent.InvalidatedRangeInflate;
      Vector3I voxelCoord2 = maxChanged + MyPrecalcComponent.InvalidatedRangeInflate;
      this.m_storage.ClampVoxelCoord(ref voxelCoord1);
      this.m_storage.ClampVoxelCoord(ref voxelCoord2);
      Vector3I start1 = voxelCoord1 >> 3;
      Vector3I end1 = voxelCoord2 >> 3;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (start1 == Vector3I.Zero && end1 == this.m_cellsCount - 1)
        {
          this.m_cellsByCoordinate.Clear();
          this.m_coordinateToMesh.Clear();
          this.m_isEmptyCache.Reset();
        }
        else
        {
          MyCellCoord cell = new MyCellCoord();
          if (this.m_cellsByCoordinate.Count > 0 || this.m_coordinateToMesh.Count > 0)
          {
            cell.CoordInLod = start1;
            Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start1, ref end1);
            while (vector3IRangeIterator.IsValid())
            {
              ulong key = cell.PackId64();
              this.m_cellsByCoordinate.Remove(key);
              this.m_coordinateToMesh.Remove(key);
              vector3IRangeIterator.GetNext(out cell.CoordInLod);
            }
          }
          if ((end1 - start1).Volume() > 100000)
          {
            Vector3I start2 = start1 >> 2;
            Vector3I end2 = (end1 >> 2) + 1;
            cell.CoordInLod = start2;
            Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start2, ref end2);
            while (vector3IRangeIterator.IsValid())
            {
              cell.CoordInLod <<= 2;
              this.RemoveEmpty(ref cell);
              vector3IRangeIterator.GetNext(out cell.CoordInLod);
            }
          }
          else
          {
            Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start1, ref end1);
            while (vector3IRangeIterator.IsValid())
            {
              this.SetEmpty(ref cell, false);
              vector3IRangeIterator.GetNext(out cell.CoordInLod);
            }
          }
        }
      }
    }

    private void GetCellLineIntersectionOctree(
      ref MyIntersectionResultLineTriangle? result,
      ref Line modelSpaceLine,
      ref float? minDistanceUntilNow,
      MyVoxelGeometry.CellData cachedDataCell,
      IntersectionFlags flags)
    {
      MyVoxelGeometry.m_overlapElementCache.Clear();
      if (cachedDataCell.Octree != null)
      {
        Vector3 packed1;
        cachedDataCell.GetPackedPosition(ref modelSpaceLine.From, out packed1);
        Vector3 packed2;
        cachedDataCell.GetPackedPosition(ref modelSpaceLine.To, out packed2);
        Ray ray = new Ray(packed1, packed2 - packed1);
        cachedDataCell.Octree.GetIntersectionWithLine(ref ray, MyVoxelGeometry.m_overlapElementCache);
      }
      for (int index1 = 0; index1 < MyVoxelGeometry.m_overlapElementCache.Count; ++index1)
      {
        int index2 = MyVoxelGeometry.m_overlapElementCache[index1];
        if (cachedDataCell.VoxelTriangles != null && index2 < cachedDataCell.VoxelTriangles.Length)
        {
          MyVoxelTriangle voxelTriangle = cachedDataCell.VoxelTriangles[index2];
          MyTriangle_Vertices triangleVertices;
          cachedDataCell.GetUnpackedPosition((int) voxelTriangle.V0, out triangleVertices.Vertex0);
          cachedDataCell.GetUnpackedPosition((int) voxelTriangle.V1, out triangleVertices.Vertex1);
          cachedDataCell.GetUnpackedPosition((int) voxelTriangle.V2, out triangleVertices.Vertex2);
          Vector3 vectorFromTriangle = MyUtils.GetNormalVectorFromTriangle(ref triangleVertices);
          if (vectorFromTriangle.IsValid() && ((flags & IntersectionFlags.FLIPPED_TRIANGLES) != (IntersectionFlags) 0 || (double) Vector3.Dot(modelSpaceLine.Direction, vectorFromTriangle) <= 0.0))
          {
            float? triangleIntersection = MyUtils.GetLineTriangleIntersection(ref modelSpaceLine, ref triangleVertices);
            if (triangleIntersection.HasValue && (!result.HasValue || (double) triangleIntersection.Value < (double) result.Value.Distance))
            {
              minDistanceUntilNow = new float?(triangleIntersection.Value);
              result = new MyIntersectionResultLineTriangle?(new MyIntersectionResultLineTriangle(0, ref triangleVertices, ref vectorFromTriangle, triangleIntersection.Value));
            }
          }
        }
      }
    }

    private void ClampCellCoord(ref Vector3I cellCoord)
    {
      Vector3I max = this.m_cellsCount - 1;
      Vector3I.Clamp(ref cellCoord, ref Vector3I.Zero, ref max, out cellCoord);
    }

    internal MyVoxelGeometry.CellData GetCell(ref MyCellCoord cell)
    {
      bool isEmpty;
      MyVoxelGeometry.CellData nonEmptyCell;
      if (this.TryGetCell(cell, out isEmpty, out nonEmptyCell))
        return nonEmptyCell;
      MyIsoMesh nonEmptyMesh;
      if (!this.TryGetMesh(cell, out isEmpty, out nonEmptyMesh))
      {
        Vector3I vector3I1 = cell.CoordInLod << 3;
        Vector3I vector3I2 = vector3I1 + 8;
        nonEmptyMesh = MyPrecalcComponent.IsoMesher.Precalc((IMyStorage) this.m_storage, 0, vector3I1 - 1, vector3I2 + 2, MyStorageDataTypeFlags.Content);
      }
      if (nonEmptyMesh != null)
      {
        nonEmptyCell = new MyVoxelGeometry.CellData();
        nonEmptyCell.Init((Vector3) nonEmptyMesh.PositionOffset, nonEmptyMesh.PositionScale, nonEmptyMesh.Positions.GetInternalArray(), nonEmptyMesh.VerticesCount, nonEmptyMesh.Triangles.GetInternalArray(), nonEmptyMesh.TrianglesCount);
      }
      if (cell.Lod == 0)
      {
        using (this.m_lock.AcquireExclusiveUsing())
        {
          if (nonEmptyCell != null)
            this.m_cellsByCoordinate[cell.PackId64()] = nonEmptyCell;
          else
            this.SetEmpty(ref cell, true);
        }
      }
      return nonEmptyCell;
    }

    private void ComputeIsEmptyLookup(MyCellCoord cell, out ulong outCacheKey, out int outBit)
    {
      Vector3I vector3I = cell.CoordInLod % 4;
      cell.CoordInLod >>= 2;
      outCacheKey = cell.PackId64();
      outBit = vector3I.X + 4 * (vector3I.Y + 4 * vector3I.Z);
    }

    private bool IsEmpty(ref MyCellCoord cell)
    {
      ulong outCacheKey;
      int outBit;
      this.ComputeIsEmptyLookup(cell, out outCacheKey, out outBit);
      return (this.m_isEmptyCache.Read(outCacheKey) & (ulong) (1L << outBit)) > 0UL;
    }

    private void RemoveEmpty(ref MyCellCoord cell)
    {
      ulong outCacheKey;
      this.ComputeIsEmptyLookup(cell, out outCacheKey, out int _);
      this.m_isEmptyCache.Remove(outCacheKey);
    }

    private void SetEmpty(ref MyCellCoord cell, bool value)
    {
      ulong outCacheKey;
      int outBit;
      this.ComputeIsEmptyLookup(cell, out outCacheKey, out outBit);
      ulong num1 = this.m_isEmptyCache.Read(outCacheKey);
      ulong num2 = !value ? num1 & (ulong) ~(1L << outBit) : num1 | (ulong) (1L << outBit);
      this.m_isEmptyCache.Write(outCacheKey, num2);
    }

    public class CellData
    {
      public int VoxelTrianglesCount;
      public int VoxelVerticesCount;
      public MyVoxelTriangle[] VoxelTriangles;
      private Vector3 m_positionOffset;
      private Vector3 m_positionScale;
      private Vector3[] m_positions;
      private MyOctree m_octree;

      internal MyOctree Octree
      {
        get
        {
          if (this.m_octree == null && this.VoxelTrianglesCount > 0)
          {
            this.m_octree = new MyOctree();
            this.m_octree.Init(this.m_positions, this.VoxelVerticesCount, this.VoxelTriangles, this.VoxelTrianglesCount, out this.VoxelTriangles);
          }
          return this.m_octree;
        }
      }

      public void Init(
        Vector3 positionOffset,
        Vector3 positionScale,
        Vector3[] positions,
        int vertexCount,
        MyVoxelTriangle[] triangles,
        int triangleCount)
      {
        if (vertexCount == 0)
        {
          this.VoxelVerticesCount = 0;
          this.VoxelTrianglesCount = 0;
          this.m_octree = (MyOctree) null;
          this.m_positions = (Vector3[]) null;
        }
        else
        {
          this.m_positionOffset = positionOffset;
          this.m_positionScale = positionScale;
          this.m_positions = new Vector3[vertexCount];
          Array.Copy((Array) positions, (Array) this.m_positions, vertexCount);
          if (this.m_octree == null)
            this.m_octree = new MyOctree();
          this.m_octree.Init(this.m_positions, vertexCount, triangles, triangleCount, out this.VoxelTriangles);
          this.VoxelVerticesCount = vertexCount;
          this.VoxelTrianglesCount = triangleCount;
        }
      }

      public void GetUnpackedPosition(int index, out Vector3 unpacked) => unpacked = this.m_positions[index] * this.m_positionScale + this.m_positionOffset;

      public void GetPackedPosition(ref Vector3 unpacked, out Vector3 packed) => packed = (unpacked - this.m_positionOffset) / this.m_positionScale;
    }
  }
}
