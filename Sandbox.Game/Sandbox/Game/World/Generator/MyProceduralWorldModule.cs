// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyProceduralWorldModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Noise;
using VRage.Noise.Combiners;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public abstract class MyProceduralWorldModule
  {
    private static List<MyObjectSeed> m_asteroidsCache;
    protected MyProceduralWorldModule m_parent;
    protected List<MyProceduralWorldModule> m_children = new List<MyProceduralWorldModule>();
    protected int m_seed;
    protected double m_objectDensity;
    protected MyDynamicAABBTreeD m_cellsTree = new MyDynamicAABBTreeD(Vector3D.Zero);
    protected Dictionary<Vector3I, MyProceduralCell> m_cells = new Dictionary<Vector3I, MyProceduralCell>();
    protected CachingHashSet<MyProceduralCell> m_dirtyCells = new CachingHashSet<MyProceduralCell>();
    protected static List<MyObjectSeed> m_tempObjectSeedList = new List<MyObjectSeed>();
    protected static List<MyProceduralCell> m_tempProceduralCellsList = new List<MyProceduralCell>();
    protected List<IMyAsteroidFieldDensityFunction> m_densityFunctionsFilled = new List<IMyAsteroidFieldDensityFunction>();
    protected List<IMyAsteroidFieldDensityFunction> m_densityFunctionsRemoved = new List<IMyAsteroidFieldDensityFunction>();
    public readonly double CELL_SIZE;
    public readonly int SCALE;
    protected const int BIG_PRIME1 = 16785407;
    protected const int BIG_PRIME2 = 39916801;
    protected const int BIG_PRIME3 = 479001599;
    protected const int TWIN_PRIME_MIDDLE1 = 240;
    protected const int TWIN_PRIME_MIDDLE2 = 312;
    protected const int TWIN_PRIME_MIDDLE3 = 462;
    private List<IMyModule> tmpDensityFunctions = new List<IMyModule>();

    protected MyProceduralWorldModule(
      double cellSize,
      int radiusMultiplier,
      int seed,
      double density,
      MyProceduralWorldModule parent = null)
    {
      this.CELL_SIZE = cellSize;
      this.SCALE = radiusMultiplier;
      this.m_seed = seed;
      this.m_objectDensity = density;
      this.m_parent = parent;
      parent?.m_children.Add(this);
    }

    protected void ChildrenAddDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
    {
      foreach (MyProceduralWorldModule child in this.m_children)
      {
        child.AddDensityFunctionFilled(func);
        child.ChildrenAddDensityFunctionFilled(func);
      }
    }

    protected void ChildrenRemoveDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
    {
      foreach (MyProceduralWorldModule child in this.m_children)
      {
        child.ChildrenRemoveDensityFunctionFilled(func);
        child.RemoveDensityFunctionFilled(func);
      }
    }

    protected void ChildrenAddDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
    {
      foreach (MyProceduralWorldModule child in this.m_children)
      {
        child.AddDensityFunctionRemoved(func);
        child.ChildrenAddDensityFunctionRemoved(func);
      }
    }

    protected void ChildrenRemoveDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
    {
      foreach (MyProceduralWorldModule child in this.m_children)
      {
        child.ChildrenRemoveDensityFunctionRemoved(func);
        child.RemoveDensityFunctionRemoved(func);
      }
    }

    protected void AddDensityFunctionFilled(IMyAsteroidFieldDensityFunction func) => this.m_densityFunctionsFilled.Add(func);

    protected void RemoveDensityFunctionFilled(IMyAsteroidFieldDensityFunction func) => this.m_densityFunctionsFilled.Remove(func);

    public void AddDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
    {
      lock (this.m_densityFunctionsRemoved)
        this.m_densityFunctionsRemoved.Add(func);
    }

    protected void RemoveDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
    {
      lock (this.m_densityFunctionsRemoved)
        this.m_densityFunctionsRemoved.Remove(func);
    }

    public void GetObjectSeeds(BoundingSphereD sphere, List<MyObjectSeed> list, bool scale = true)
    {
      BoundingSphereD sphere1 = sphere;
      if (scale)
        sphere1.Radius *= (double) this.SCALE;
      this.GenerateObjectSeeds(ref sphere1);
      this.OverlapAllBoundingSphere(ref sphere1, list);
    }

    protected abstract MyProceduralCell GenerateProceduralCell(ref Vector3I cellId);

    protected int GetCellSeed(ref Vector3I cell) => this.m_seed + cell.X * 16785407 + cell.Y * 39916801 + cell.Z * 479001599;

    protected int GetObjectIdSeed(MyObjectSeed objectSeed) => ((objectSeed.CellId.GetHashCode() * 397 ^ this.m_seed) * 397 ^ objectSeed.Params.Index) * 397 ^ objectSeed.Params.Seed;

    public abstract void GenerateObjects(
      List<MyObjectSeed> list,
      HashSet<MyObjectSeedParams> existingObjectsSeeds);

    protected void GenerateObjectSeeds(ref BoundingSphereD sphere)
    {
      Vector3I_RangeIterator cellsIterator = this.GetCellsIterator(sphere);
      while (cellsIterator.IsValid())
      {
        Vector3I current = cellsIterator.Current;
        if (!this.m_cells.ContainsKey(current))
        {
          BoundingBoxD box = new BoundingBoxD(current * this.CELL_SIZE, (current + 1) * this.CELL_SIZE);
          if (sphere.Contains(box) != ContainmentType.Disjoint)
          {
            MyProceduralCell proceduralCell = this.GenerateProceduralCell(ref current);
            if (proceduralCell != null)
            {
              this.m_cells.Add(current, proceduralCell);
              BoundingBoxD boundingVolume = proceduralCell.BoundingVolume;
              proceduralCell.proxyId = this.m_cellsTree.AddProxy(ref boundingVolume, (object) proceduralCell, 0U);
            }
          }
        }
        cellsIterator.MoveNext();
      }
    }

    protected IMyModule GetCellDensityFunctionFilled(BoundingBoxD bbox)
    {
      foreach (IMyAsteroidFieldDensityFunction fieldDensityFunction in this.m_densityFunctionsFilled)
      {
        if (fieldDensityFunction.ExistsInCell(ref bbox))
          this.tmpDensityFunctions.Add((IMyModule) fieldDensityFunction);
      }
      if (this.tmpDensityFunctions.Count == 0)
        return (IMyModule) null;
      for (int index1 = this.tmpDensityFunctions.Count; index1 > 1; index1 = index1 / 2 + index1 % 2)
      {
        for (int index2 = 0; index2 < index1 / 2; ++index2)
          this.tmpDensityFunctions[index2] = (IMyModule) new MyMax(this.tmpDensityFunctions[index2 * 2], this.tmpDensityFunctions[index2 * 2 + 1]);
        if (index1 % 2 == 1)
          this.tmpDensityFunctions[index1 - 1] = this.tmpDensityFunctions[index1 / 2];
      }
      IMyModule tmpDensityFunction = this.tmpDensityFunctions[0];
      this.tmpDensityFunctions.Clear();
      return tmpDensityFunction;
    }

    protected IMyModule GetCellDensityFunctionRemoved(BoundingBoxD bbox)
    {
      foreach (IMyAsteroidFieldDensityFunction fieldDensityFunction in this.m_densityFunctionsRemoved)
      {
        if (fieldDensityFunction != null && fieldDensityFunction.ExistsInCell(ref bbox))
          this.tmpDensityFunctions.Add((IMyModule) fieldDensityFunction);
      }
      if (this.tmpDensityFunctions.Count == 0)
        return (IMyModule) null;
      for (int index1 = this.tmpDensityFunctions.Count; index1 > 1; index1 = index1 / 2 + index1 % 2)
      {
        for (int index2 = 0; index2 < index1 / 2; ++index2)
          this.tmpDensityFunctions[index2] = (IMyModule) new MyMin(this.tmpDensityFunctions[index2 * 2], this.tmpDensityFunctions[index2 * 2 + 1]);
        if (index1 % 2 == 1)
          this.tmpDensityFunctions[index1 - 1] = this.tmpDensityFunctions[index1 / 2];
      }
      IMyModule tmpDensityFunction = this.tmpDensityFunctions[0];
      this.tmpDensityFunctions.Clear();
      return tmpDensityFunction;
    }

    public void MarkCellsDirty(BoundingSphereD toMark, BoundingSphereD? toExclude = null, bool scale = true)
    {
      BoundingSphereD sphere = new BoundingSphereD(toMark.Center, toMark.Radius * (scale ? (double) this.SCALE : 1.0));
      BoundingSphereD boundingSphereD = new BoundingSphereD();
      if (toExclude.HasValue)
      {
        boundingSphereD = toExclude.Value;
        if (scale)
          boundingSphereD.Radius *= (double) this.SCALE;
      }
      Vector3I_RangeIterator cellsIterator = this.GetCellsIterator(sphere);
      while (cellsIterator.IsValid())
      {
        MyProceduralCell myProceduralCell;
        if (this.m_cells.TryGetValue(cellsIterator.Current, out myProceduralCell) && (!toExclude.HasValue || boundingSphereD.Contains(myProceduralCell.BoundingVolume) == ContainmentType.Disjoint))
          this.m_dirtyCells.Add(myProceduralCell);
        cellsIterator.MoveNext();
      }
    }

    public void ProcessDirtyCells(
      Dictionary<MyEntity, MyEntityTracker> trackedEntities)
    {
      this.m_dirtyCells.ApplyAdditions();
      if (this.m_dirtyCells.Count == 0)
        return;
      foreach (MyProceduralCell dirtyCell in this.m_dirtyCells)
      {
        foreach (MyEntityTracker myEntityTracker in trackedEntities.Values)
        {
          BoundingSphereD boundingVolume = myEntityTracker.BoundingVolume;
          boundingVolume.Radius *= (double) this.SCALE;
          if (boundingVolume.Contains(dirtyCell.BoundingVolume) != ContainmentType.Disjoint)
          {
            this.m_dirtyCells.Remove(dirtyCell);
            break;
          }
        }
      }
      this.m_dirtyCells.ApplyRemovals();
      foreach (MyProceduralCell dirtyCell in this.m_dirtyCells)
      {
        dirtyCell.GetAll(MyProceduralWorldModule.m_tempObjectSeedList);
        foreach (MyObjectSeed tempObjectSeed in MyProceduralWorldModule.m_tempObjectSeedList)
        {
          if (tempObjectSeed.Params.Generated)
            this.CloseObjectSeed(tempObjectSeed);
        }
        MyProceduralWorldModule.m_tempObjectSeedList.Clear();
      }
      foreach (MyProceduralCell dirtyCell in this.m_dirtyCells)
      {
        this.m_cells.Remove(dirtyCell.CellId);
        this.m_cellsTree.RemoveProxy(dirtyCell.proxyId);
      }
      this.m_dirtyCells.Clear();
    }

    protected abstract void CloseObjectSeed(MyObjectSeed objectSeed);

    public abstract void ReclaimObject(object reclaimedObject);

    protected Vector3I_RangeIterator GetCellsIterator(BoundingSphereD sphere) => this.GetCellsIterator(BoundingBoxD.CreateFromSphere(sphere));

    protected Vector3I_RangeIterator GetCellsIterator(BoundingBoxD bbox)
    {
      Vector3I start = Vector3I.Floor(bbox.Min / this.CELL_SIZE);
      Vector3I end = Vector3I.Floor(bbox.Max / this.CELL_SIZE);
      return new Vector3I_RangeIterator(ref start, ref end);
    }

    protected void OverlapAllBoundingSphere(ref BoundingSphereD sphere, List<MyObjectSeed> list)
    {
      this.m_cellsTree.OverlapAllBoundingSphere<MyProceduralCell>(ref sphere, MyProceduralWorldModule.m_tempProceduralCellsList);
      foreach (MyProceduralCell tempProceduralCells in MyProceduralWorldModule.m_tempProceduralCellsList)
        tempProceduralCells.OverlapAllBoundingSphere(ref sphere, list);
      MyProceduralWorldModule.m_tempProceduralCellsList.Clear();
    }

    public static Vector3D? FindFreeLocationCloseToAsteroid(
      BoundingSphereD searchArea,
      BoundingSphereD? suppressedArea,
      bool takeOccupiedPositions,
      bool sortByDistance,
      float collisionRadius,
      float minFreeRange,
      out Vector3 forward,
      out Vector3 up)
    {
      for (int index1 = sortByDistance ? 3 : 1; index1 > 0; --index1)
      {
        bool flag1 = index1 == 1;
        BoundingSphereD boundingSphereD1 = new BoundingSphereD(searchArea.Center, searchArea.Radius / (double) index1);
        BoundingSphereD boundingSphereD2;
        if (!flag1 && suppressedArea.HasValue)
        {
          boundingSphereD2 = suppressedArea.Value;
          if (boundingSphereD2.Contains(boundingSphereD1) == ContainmentType.Contains)
            continue;
        }
        using (MyUtils.ReuseCollection<MyObjectSeed>(ref MyProceduralWorldModule.m_asteroidsCache))
        {
          List<MyObjectSeed> asteroidsCache = MyProceduralWorldModule.m_asteroidsCache;
          MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(boundingSphereD1, asteroidsCache);
          asteroidsCache.RemoveAll((Predicate<MyObjectSeed>) (x =>
          {
            MyObjectSeedType type = x.Params.Type;
            return type != MyObjectSeedType.Asteroid && type != MyObjectSeedType.AsteroidCluster;
          }));
          if (asteroidsCache.Count != 0)
          {
            if (sortByDistance)
            {
              Vector3D spawnPosition = searchArea.Center;
              asteroidsCache.Sort((Comparison<MyObjectSeed>) ((a, b) => Vector3D.DistanceSquared(spawnPosition, a.BoundingVolume.Center).CompareTo(Vector3D.DistanceSquared(spawnPosition, b.BoundingVolume.Center))));
            }
            else
              asteroidsCache.ShuffleList<MyObjectSeed>();
            bool flag2 = false;
            for (int index2 = asteroidsCache.Count - 1; index2 >= 0; --index2)
            {
              BoundingBoxD boundingVolume = asteroidsCache[index2].BoundingVolume;
              bool flag3;
              if (suppressedArea.HasValue)
              {
                boundingSphereD2 = suppressedArea.Value;
                if (boundingSphereD2.Contains(boundingVolume) != ContainmentType.Disjoint)
                {
                  flag3 = true;
                  goto label_13;
                }
              }
              double radius = Math.Max(boundingVolume.HalfExtents.AbsMax() * 2.0, (double) minFreeRange);
              flag3 = !MyProceduralWorldModule.IsZoneFree(new BoundingSphereD(boundingVolume.Center, radius));
label_13:
              if (flag3)
              {
                if (takeOccupiedPositions)
                  asteroidsCache.Add(asteroidsCache[index2]);
                asteroidsCache.RemoveAt(index2);
              }
              else
                flag2 = true;
            }
            if (!flag2)
            {
              if (!flag1)
                continue;
            }
            foreach (MyObjectSeed myObjectSeed in asteroidsCache)
            {
              BoundingBoxD boundingVolume = myObjectSeed.BoundingVolume;
              Vector3D vector3Normalized = (Vector3D) MyUtils.GetRandomVector3Normalized();
              Vector3D? freePosition = MyEntities.FindFreePlace(Vector3D.Clamp(boundingVolume.Center + vector3Normalized * boundingVolume.HalfExtents.AbsMax() * 10.0, boundingVolume.Min, boundingVolume.Max) + (boundingVolume.HalfExtents.AbsMax() / 2.0 + (double) collisionRadius) * vector3Normalized, collisionRadius);
              if (freePosition.HasValue && !MyPlanets.Static.GetPlanetAABBs().Any<BoundingBoxD>((Func<BoundingBoxD, bool>) (x => (uint) x.Contains(freePosition.Value) > 0U)) && !asteroidsCache.Any<MyObjectSeed>((Func<MyObjectSeed, bool>) (x => (uint) x.BoundingVolume.Contains(freePosition.Value) > 0U)))
              {
                forward = (Vector3) Vector3D.Normalize(boundingVolume.Center - freePosition.Value);
                up = MyUtils.GetRandomPerpendicularVector(in forward);
                return new Vector3D?(freePosition.Value);
              }
            }
          }
        }
      }
      up = (Vector3) new Vector3D();
      forward = (Vector3) new Vector3D();
      return new Vector3D?();
    }

    public static bool IsZoneFree(BoundingSphereD safeZone)
    {
      using (ClearToken<MyEntity> clearToken = MyEntities.GetTopMostEntitiesInSphere(ref safeZone).GetClearToken<MyEntity>())
      {
        foreach (MyEntity myEntity in clearToken.List)
        {
          if (myEntity is MyCubeGrid)
            return false;
        }
      }
      return true;
    }

    internal void OverlapAllBoundingBox(ref BoundingBoxD box, List<MyObjectSeed> list)
    {
      this.m_cellsTree.OverlapAllBoundingBox<MyProceduralCell>(ref box, MyProceduralWorldModule.m_tempProceduralCellsList);
      foreach (MyProceduralCell tempProceduralCells in MyProceduralWorldModule.m_tempProceduralCellsList)
        tempProceduralCells.OverlapAllBoundingBox(ref box, list);
      MyProceduralWorldModule.m_tempProceduralCellsList.Clear();
    }

    internal void GetAllCells(List<MyProceduralCell> list) => this.m_cellsTree.GetAll<MyProceduralCell>(list, false);

    internal void CleanUp()
    {
      if (MyProceduralWorldModule.m_asteroidsCache != null)
        MyProceduralWorldModule.m_asteroidsCache.Clear();
      foreach (MyProceduralWorldModule child in this.m_children)
        child.CleanUp();
      this.m_children.Clear();
      this.m_cellsTree.Clear();
      this.m_cells.Clear();
      this.m_dirtyCells.Clear();
      MyProceduralWorldModule.m_tempObjectSeedList.Clear();
      MyProceduralWorldModule.m_tempProceduralCellsList.Clear();
      this.m_densityFunctionsFilled.Clear();
      this.m_densityFunctionsRemoved.Clear();
    }
  }
}
