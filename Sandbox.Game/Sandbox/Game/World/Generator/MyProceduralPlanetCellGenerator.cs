// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyProceduralPlanetCellGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyProceduralPlanetCellGenerator : MyProceduralWorldModule
  {
    public const int MOON_SIZE_MIN_LIMIT = 4000;
    public const int MOON_SIZE_MAX_LIMIT = 30000;
    public const int PLANET_SIZE_MIN_LIMIT = 8000;
    public const int PLANET_SIZE_MAX_LIMIT = 120000;
    internal readonly float PLANET_SIZE_MIN;
    internal readonly float PLANET_SIZE_MAX;
    internal const int MOONS_MAX = 3;
    internal readonly float MOON_SIZE_MIN;
    internal readonly float MOON_SIZE_MAX;
    internal const int MOON_DISTANCE_MIN = 4000;
    internal const int MOON_DISTANCE_MAX = 32000;
    internal const double MOON_DENSITY = 0.0;
    internal const int FALLOFF = 16000;
    internal const double GRAVITY_SIZE_MULTIPLIER = 1.1;
    internal readonly double OBJECT_SEED_RADIUS;
    private List<BoundingBoxD> m_tmpClusterBoxes = new List<BoundingBoxD>(4);
    private List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();

    public MyProceduralPlanetCellGenerator(
      int seed,
      double density,
      float planetSizeMax,
      float planetSizeMin,
      float moonSizeMax,
      float moonSizeMin,
      MyProceduralWorldModule parent = null)
      : base(2048000.0, 250, seed, (density + 1.0) / 2.0 - 1.0, parent)
    {
      if ((double) planetSizeMax < (double) planetSizeMin)
      {
        double num = (double) planetSizeMax;
        planetSizeMax = planetSizeMin;
        planetSizeMin = (float) num;
      }
      this.PLANET_SIZE_MAX = MathHelper.Clamp(planetSizeMax, 8000f, 120000f);
      this.PLANET_SIZE_MIN = MathHelper.Clamp(planetSizeMin, 8000f, planetSizeMax);
      if ((double) moonSizeMax < (double) moonSizeMin)
      {
        double num = (double) moonSizeMax;
        moonSizeMax = moonSizeMin;
        moonSizeMin = (float) num;
      }
      this.MOON_SIZE_MAX = MathHelper.Clamp(moonSizeMax, 4000f, 30000f);
      this.MOON_SIZE_MIN = MathHelper.Clamp(moonSizeMin, 4000f, moonSizeMax);
      this.OBJECT_SEED_RADIUS = (double) this.PLANET_SIZE_MAX / 2.0 * 1.1 + 2.0 * ((double) this.MOON_SIZE_MAX / 2.0 * 1.1 + 64000.0);
      this.AddDensityFunctionFilled((IMyAsteroidFieldDensityFunction) new MyInfiniteDensityFunction(MyRandom.Instance, 0.001));
    }

    protected override MyProceduralCell GenerateProceduralCell(ref Vector3I cellId)
    {
      MyProceduralCell cell = new MyProceduralCell(cellId, this.CELL_SIZE);
      IMyModule densityFunctionFilled = this.GetCellDensityFunctionFilled(cell.BoundingVolume);
      if (densityFunctionFilled == null)
        return (MyProceduralCell) null;
      IMyModule densityFunctionRemoved = this.GetCellDensityFunctionRemoved(cell.BoundingVolume);
      int cellSeed = this.GetCellSeed(ref cellId);
      MyRandom instance = MyRandom.Instance;
      using (instance.PushSeed(cellSeed))
      {
        Vector3D vector3D = (new Vector3D(instance.NextDouble(), instance.NextDouble(), instance.NextDouble()) * ((this.CELL_SIZE - 2.0 * this.OBJECT_SEED_RADIUS) / this.CELL_SIZE) + this.OBJECT_SEED_RADIUS / this.CELL_SIZE + (Vector3D) cellId) * this.CELL_SIZE;
        if (MyEntities.IsInsideWorld(vector3D))
        {
          if (densityFunctionFilled.GetValue(vector3D.X, vector3D.Y, vector3D.Z) < this.m_objectDensity)
          {
            double size = MathHelper.Lerp((double) this.PLANET_SIZE_MIN, (double) this.PLANET_SIZE_MAX, instance.NextDouble());
            MyObjectSeed objectSeed = new MyObjectSeed(cell, vector3D, size);
            objectSeed.Params.Type = MyObjectSeedType.Planet;
            objectSeed.Params.Seed = instance.Next();
            objectSeed.Params.Index = 0;
            objectSeed.UserData = (object) new MySphereDensityFunction(vector3D, (double) this.PLANET_SIZE_MAX / 2.0 * 1.1 + 16000.0, 16000.0);
            int index = 1;
            this.GenerateObject(cell, objectSeed, ref index, instance, densityFunctionFilled, densityFunctionRemoved);
          }
        }
      }
      return cell;
    }

    private void GenerateObject(
      MyProceduralCell cell,
      MyObjectSeed objectSeed,
      ref int index,
      MyRandom random,
      IMyModule densityFunctionFilled,
      IMyModule densityFunctionRemoved)
    {
      cell.AddObject(objectSeed);
      if (objectSeed.UserData is IMyAsteroidFieldDensityFunction userData)
        this.ChildrenAddDensityFunctionRemoved(userData);
      switch (objectSeed.Params.Type)
      {
        case MyObjectSeedType.Empty:
          break;
        case MyObjectSeedType.Planet:
          this.m_tmpClusterBoxes.Add(objectSeed.BoundingVolume);
          for (int index1 = 0; index1 < 3; ++index1)
          {
            Vector3D randomDirection = MyProceduralWorldGenerator.GetRandomDirection(random);
            double size = MathHelper.Lerp((double) this.MOON_SIZE_MIN, (double) this.MOON_SIZE_MAX, random.NextDouble());
            double num1 = MathHelper.Lerp(4000.0, 32000.0, random.NextDouble());
            BoundingBoxD boundingVolume = objectSeed.BoundingVolume;
            Vector3D center = boundingVolume.Center;
            Vector3D vector3D1 = randomDirection;
            double num2 = size;
            boundingVolume = objectSeed.BoundingVolume;
            double num3 = boundingVolume.HalfExtents.Length() * 2.0;
            double num4 = num2 + num3 + num1;
            Vector3D vector3D2 = vector3D1 * num4;
            Vector3D vector3D3 = center + vector3D2;
            if (densityFunctionFilled.GetValue(vector3D3.X, vector3D3.Y, vector3D3.Z) < 0.0)
            {
              MyObjectSeed objectSeed1 = new MyObjectSeed(cell, vector3D3, size);
              objectSeed1.Params.Seed = random.Next();
              objectSeed1.Params.Type = MyObjectSeedType.Moon;
              objectSeed1.Params.Index = index++;
              objectSeed1.UserData = (object) new MySphereDensityFunction(vector3D3, (double) this.MOON_SIZE_MAX / 2.0 * 1.1 + 16000.0, 16000.0);
              bool flag = false;
              foreach (BoundingBoxD tmpClusterBox in this.m_tmpClusterBoxes)
              {
                int num5 = flag ? 1 : 0;
                boundingVolume = objectSeed1.BoundingVolume;
                int num6 = boundingVolume.Intersects(tmpClusterBox) ? 1 : 0;
                if (flag = (num5 | num6) != 0)
                  break;
              }
              if (!flag)
              {
                this.m_tmpClusterBoxes.Add(objectSeed1.BoundingVolume);
                this.GenerateObject(cell, objectSeed1, ref index, random, densityFunctionFilled, densityFunctionRemoved);
              }
            }
          }
          this.m_tmpClusterBoxes.Clear();
          break;
        case MyObjectSeedType.Moon:
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    public override void GenerateObjects(
      List<MyObjectSeed> objectsList,
      HashSet<MyObjectSeedParams> existingObjectsSeeds)
    {
      foreach (MyObjectSeed objects in objectsList)
      {
        if (!objects.Params.Generated)
        {
          objects.Params.Generated = true;
          using (MyRandom.Instance.PushSeed(this.GetObjectIdSeed(objects)))
          {
            BoundingBoxD boundingVolume = objects.BoundingVolume;
            MyGamePruningStructure.GetAllVoxelMapsInBox(ref boundingVolume, this.m_tmpVoxelMapsList);
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", (object) objects.Params.Type, (object) objects.CellId.X, (object) objects.CellId.Y, (object) objects.CellId.Z, (object) objects.Params.Index, (object) objects.Params.Seed);
            bool flag = false;
            foreach (MyVoxelBase tmpVoxelMaps in this.m_tmpVoxelMapsList)
            {
              if (tmpVoxelMaps.StorageName == str)
              {
                flag = true;
                break;
              }
            }
            this.m_tmpVoxelMapsList.Clear();
            int num = flag ? 1 : 0;
          }
        }
      }
    }

    private long GetPlanetEntityId(MyObjectSeed objectSeed)
    {
      Vector3I cellId = objectSeed.CellId;
      return (((((((long) Math.Abs(cellId.X) * 397L ^ (long) Math.Abs(cellId.Y)) * 397L ^ (long) Math.Abs(cellId.Z)) * 397L ^ (long) (Math.Sign(cellId.X) + 240)) * 397L ^ (long) (Math.Sign(cellId.Y) + 312)) * 397L ^ (long) (Math.Sign(cellId.Z) + 462)) * 397L ^ (long) objectSeed.Params.Index * 16785407L) & 72057594037927935L | 504403158265495552L;
    }

    private static Vector3I GetPlanetVoxelSize(double size) => new Vector3I(Math.Max(64, (int) Math.Ceiling(size)));

    protected override void CloseObjectSeed(MyObjectSeed objectSeed)
    {
      if (objectSeed.UserData is IMyAsteroidFieldDensityFunction userData)
        this.ChildrenRemoveDensityFunctionRemoved(userData);
      BoundingBoxD boundingVolume = objectSeed.BoundingVolume;
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref boundingVolume, this.m_tmpVoxelMapsList);
      string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", (object) objectSeed.Params.Type, (object) objectSeed.CellId.X, (object) objectSeed.CellId.Y, (object) objectSeed.CellId.Z, (object) objectSeed.Params.Index, (object) objectSeed.Params.Seed);
      foreach (MyVoxelBase tmpVoxelMaps in this.m_tmpVoxelMapsList)
      {
        if (tmpVoxelMaps.StorageName == str)
        {
          if (!tmpVoxelMaps.Save)
          {
            tmpVoxelMaps.Close();
            break;
          }
          break;
        }
      }
      this.m_tmpVoxelMapsList.Clear();
    }

    public override void ReclaimObject(object reclaimedObject)
    {
    }
  }
}
