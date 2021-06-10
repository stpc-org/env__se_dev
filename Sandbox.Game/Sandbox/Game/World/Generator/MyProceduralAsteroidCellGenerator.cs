// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyProceduralAsteroidCellGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Noise;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyProceduralAsteroidCellGenerator : MyProceduralWorldModule
  {
    private MyAsteroidGeneratorDefinition m_data;
    private double m_seedTypeProbabilitySum;
    private double m_seedClusterTypeProbabilitySum;
    private bool m_isClosingEntities;
    private List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();
    private List<BoundingBoxD> m_tmpClusterBoxes;
    private bool m_enabled;

    public MyProceduralAsteroidCellGenerator(
      int seed,
      double density,
      MyProceduralWorldModule parent = null)
      : base(MyProceduralAsteroidCellGenerator.GetSubCellInfo(), 1, seed, density, parent)
    {
      this.m_enabled = MyFakes.ENABLE_ASTEROIDS;
      this.m_data = MyProceduralAsteroidCellGenerator.GetData();
      this.AddDensityFunctionFilled((IMyAsteroidFieldDensityFunction) new MyInfiniteDensityFunction(MyRandom.Instance, 0.003));
      this.m_seedTypeProbabilitySum = 0.0;
      foreach (double num in this.m_data.SeedTypeProbability.Values)
        this.m_seedTypeProbabilitySum += num;
      this.m_seedClusterTypeProbabilitySum = 0.0;
      foreach (double num in this.m_data.SeedClusterTypeProbability.Values)
        this.m_seedClusterTypeProbabilitySum += num;
    }

    public static long GetAsteroidEntityId(string storageName) => storageName.GetHashCode64() & 72057594037927935L | 432345564227567616L;

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
        int index = 0;
        Vector3I next = Vector3I.Zero;
        Vector3I end = new Vector3I(this.m_data.SubCells - 1);
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref Vector3I.Zero, ref end);
        while (vector3IRangeIterator.IsValid())
        {
          Vector3D vector3D = new Vector3D(instance.NextDouble(), instance.NextDouble(), instance.NextDouble());
          vector3D = (vector3D + (Vector3D) next / (double) this.m_data.SubcellSize + cellId) * this.CELL_SIZE;
          if (MyEntities.IsInsideWorld(vector3D))
          {
            double num1 = -1.0;
            if (densityFunctionRemoved != null)
            {
              num1 = densityFunctionRemoved.GetValue(vector3D.X, vector3D.Y, vector3D.Z);
              if (num1 <= -1.0)
                goto label_9;
            }
            double num2 = densityFunctionFilled.GetValue(vector3D.X, vector3D.Y, vector3D.Z);
            if ((densityFunctionRemoved == null || num1 >= num2) && num2 < this.m_objectDensity)
              this.GenerateObject(cell, new MyObjectSeed(cell, vector3D, this.GetObjectSize(instance.NextDouble()))
              {
                Params = {
                  Type = this.GetSeedType(instance.NextDouble()),
                  Seed = instance.Next(),
                  Index = index++
                }
              }, ref index, instance, densityFunctionFilled, densityFunctionRemoved);
          }
label_9:
          vector3IRangeIterator.GetNext(out next);
        }
      }
      return cell;
    }

    public override void GenerateObjects(
      List<MyObjectSeed> objectsList,
      HashSet<MyObjectSeedParams> existingObjectsSeeds)
    {
      if (!this.m_enabled)
        return;
      foreach (MyObjectSeed objects in objectsList)
      {
        if (!objects.Params.Generated && !existingObjectsSeeds.Contains(objects.Params))
        {
          objects.Params.Generated = true;
          using (MyRandom.Instance.PushSeed(this.GetObjectIdSeed(objects)))
          {
            switch (objects.Params.Type)
            {
              case MyObjectSeedType.Empty:
                continue;
              case MyObjectSeedType.Asteroid:
                BoundingBoxD boundingVolume = objects.BoundingVolume;
                string storageName = string.Format("Asteroid_{0}_{1}_{2}_{3}_{4}", (object) objects.CellId.X, (object) objects.CellId.Y, (object) objects.CellId.Z, (object) objects.Params.Index, (object) objects.Params.Seed);
                bool flag = false;
                long asteroidEntityId = MyProceduralAsteroidCellGenerator.GetAsteroidEntityId(storageName);
                if (!flag && MyEntities.EntityExists(asteroidEntityId))
                {
                  if (MyEntities.GetEntityById(asteroidEntityId) is MyVoxelMap entityById && entityById.StorageName == storageName)
                    entityById.IsSeedOpen = new bool?(true);
                  flag = true;
                }
                if (!flag)
                {
                  MyGamePruningStructure.GetAllVoxelMapsInBox(ref boundingVolume, this.m_tmpVoxelMapsList);
                  foreach (MyVoxelBase tmpVoxelMaps in this.m_tmpVoxelMapsList)
                  {
                    if (tmpVoxelMaps.StorageName == storageName)
                    {
                      tmpVoxelMaps.IsSeedOpen = new bool?(true);
                      flag = true;
                      break;
                    }
                  }
                  this.m_tmpVoxelMapsList.Clear();
                }
                if (!flag)
                {
                  MyStorageBase storage = (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) MyCompositeShapeProvider.CreateAsteroidShape(objects.Params.Seed, objects.Size, this.m_data.UseGeneratorSeed ? objects.Params.GeneratorSeed : 0), MyProceduralAsteroidCellGenerator.GetAsteroidVoxelSize((double) objects.Size));
                  Vector3D vector3D = objects.BoundingVolume.Center - (double) (MathHelper.GetNearestBiggerPowerOfTwo(objects.Size) / 2);
                  MyVoxelMap voxelMap;
                  if (this.m_data.RotateAsteroids)
                  {
                    using (MyRandom.Instance.PushSeed(objects.Params.Seed))
                    {
                      MatrixD asteroidRotation = MyProceduralAsteroidCellGenerator.CreateAsteroidRotation(MyRandom.Instance, vector3D, storage.Size);
                      voxelMap = MyWorldGenerator.AddVoxelMap(storageName, storage, asteroidRotation, asteroidEntityId);
                    }
                  }
                  else
                    voxelMap = MyWorldGenerator.AddVoxelMap(storageName, storage, vector3D, asteroidEntityId);
                  if (voxelMap != null)
                  {
                    voxelMap.Save = false;
                    voxelMap.IsSeedOpen = new bool?(true);
                    MyVoxelBase.StorageChanged OnStorageRangeChanged = (MyVoxelBase.StorageChanged) null;
                    OnStorageRangeChanged = (MyVoxelBase.StorageChanged) ((voxel, minVoxelChanged, maxVoxelChanged, changedData) =>
                    {
                      voxelMap.Save = true;
                      voxelMap.RangeChanged -= OnStorageRangeChanged;
                    });
                    voxelMap.RangeChanged += OnStorageRangeChanged;
                    MyObjectSeedParams voxelParams = objects.Params;
                    if (Sync.IsServer)
                    {
                      MyVoxelMap myVoxelMap = voxelMap;
                      myVoxelMap.OnEntityCloseRequest = myVoxelMap.OnEntityCloseRequest + (Action<MyEntity>) (voxel =>
                      {
                        if (this.m_isClosingEntities)
                          return;
                        MyMultiplayer.RaiseStaticEvent<MyObjectSeedParams>((Func<IMyEventOwner, Action<MyObjectSeedParams>>) (x => new Action<MyObjectSeedParams>(MyProceduralWorldGenerator.AddExistingObjectsSeed)), voxelParams);
                      });
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              case MyObjectSeedType.EncounterAlone:
              case MyObjectSeedType.EncounterSingle:
                MyEncounterGenerator.Static.PlaceEncounterToWorld(objects.BoundingVolume, objects.Params.Seed);
                continue;
              default:
                throw new InvalidBranchException();
            }
          }
        }
      }
    }

    private void GenerateObject(
      MyProceduralCell cell,
      MyObjectSeed objectSeed,
      ref int index,
      MyRandom random,
      IMyModule densityFunctionFilled,
      IMyModule densityFunctionRemoved)
    {
      if (this.m_data.UseGeneratorSeed && objectSeed.Params.GeneratorSeed == 0)
        objectSeed.Params.GeneratorSeed = random.Next();
      if (this.m_data.UseClusterDefAsAsteroid)
        cell.AddObject(objectSeed);
      switch (objectSeed.Params.Type)
      {
        case MyObjectSeedType.Empty:
          break;
        case MyObjectSeedType.Asteroid:
        case MyObjectSeedType.EncounterAlone:
        case MyObjectSeedType.EncounterSingle:
          if (this.m_data.UseClusterDefAsAsteroid)
            break;
          cell.AddObject(objectSeed);
          break;
        case MyObjectSeedType.AsteroidCluster:
          if (this.m_data.UseClusterDefAsAsteroid)
            objectSeed.Params.Type = MyObjectSeedType.Asteroid;
          using (MyUtils.ReuseCollection<BoundingBoxD>(ref this.m_tmpClusterBoxes))
          {
            int num1 = this.m_data.UseGeneratorSeed ? random.Next() : 0;
            double num2 = (double) this.m_data.ObjectMaxDistanceInClusterMin;
            if (this.m_data.UseClusterVariableSize)
              num2 = MathHelper.Lerp((double) this.m_data.ObjectMaxDistanceInClusterMin, (double) this.m_data.ObjectMaxDistanceInClusterMax, random.NextDouble());
            for (int index1 = 0; index1 < this.m_data.ObjectMaxInCluster; ++index1)
            {
              Vector3D randomDirection = MyProceduralWorldGenerator.GetRandomDirection(random);
              double clusterObjectSize = this.GetClusterObjectSize(random.NextDouble());
              double num3 = MathHelper.Lerp((double) this.m_data.ObjectMinDistanceInCluster, num2, random.NextDouble());
              double num4 = this.m_data.ClusterDispersionAbsolute ? num3 : clusterObjectSize + objectSeed.BoundingVolume.HalfExtents.Length() * 2.0 + num3;
              Vector3D position = objectSeed.BoundingVolume.Center + randomDirection * num4;
              double num5 = -1.0;
              if (densityFunctionRemoved != null)
              {
                num5 = densityFunctionRemoved.GetValue(position.X, position.Y, position.Z);
                if (num5 <= -1.0)
                  continue;
              }
              double num6 = densityFunctionFilled.GetValue(position.X, position.Y, position.Z);
              if ((densityFunctionRemoved == null || num5 >= num6) && num6 < this.m_data.ObjectDensityCluster)
              {
                MyObjectSeed objectSeed1 = new MyObjectSeed(cell, position, clusterObjectSize);
                objectSeed1.Params.Seed = random.Next();
                objectSeed1.Params.Index = index++;
                objectSeed1.Params.Type = this.GetClusterSeedType(random.NextDouble());
                objectSeed1.Params.GeneratorSeed = num1;
                BoundingBoxD hitBox = objectSeed1.BoundingVolume;
                if (this.m_data.AllowPartialClusterObjectOverlap)
                {
                  Vector3D center = hitBox.Center;
                  Vector3D halfExtents = hitBox.HalfExtents;
                  hitBox = new BoundingBoxD(center - halfExtents * 0.300000011920929, center + halfExtents * 0.300000011920929);
                }
                if (this.m_tmpClusterBoxes.All<BoundingBoxD>((Func<BoundingBoxD, bool>) (box => !hitBox.Intersects(box))))
                {
                  this.m_tmpClusterBoxes.Add(hitBox);
                  this.GenerateObject(cell, objectSeed1, ref index, random, densityFunctionFilled, densityFunctionRemoved);
                }
              }
            }
            break;
          }
        default:
          throw new InvalidBranchException();
      }
    }

    private MyObjectSeedType GetSeedType(double d)
    {
      d *= this.m_seedTypeProbabilitySum;
      foreach (KeyValuePair<MyObjectSeedType, double> keyValuePair in this.m_data.SeedTypeProbability)
      {
        if (keyValuePair.Value >= d)
          return keyValuePair.Key;
        d -= keyValuePair.Value;
      }
      return MyObjectSeedType.Asteroid;
    }

    private MyObjectSeedType GetClusterSeedType(double d)
    {
      d *= this.m_seedClusterTypeProbabilitySum;
      foreach (KeyValuePair<MyObjectSeedType, double> keyValuePair in this.m_data.SeedClusterTypeProbability)
      {
        if (keyValuePair.Value >= d)
          return keyValuePair.Key;
        d -= keyValuePair.Value;
      }
      return MyObjectSeedType.Asteroid;
    }

    private double GetObjectSize(double noise) => this.m_data.UseLinearPowOfTwoSizeDistribution ? (double) (1 << (int) Math.Round(MathHelper.Lerp(Math.Log((double) MathHelper.GetNearestBiggerPowerOfTwo(this.m_data.ObjectSizeMin), 2.0), Math.Log((double) MathHelper.GetNearestBiggerPowerOfTwo(this.m_data.ObjectSizeMax), 2.0), noise))) : (double) this.m_data.ObjectSizeMin + noise * noise * (double) (this.m_data.ObjectSizeMax - this.m_data.ObjectSizeMin);

    private double GetClusterObjectSize(double noise) => this.m_data.UseLinearPowOfTwoSizeDistribution ? (double) (1 << (int) Math.Round(MathHelper.Lerp(Math.Log((double) MathHelper.GetNearestBiggerPowerOfTwo(this.m_data.ObjectSizeMinCluster), 2.0), Math.Log((double) MathHelper.GetNearestBiggerPowerOfTwo(this.m_data.ObjectSizeMaxCluster), 2.0), noise))) : (double) this.m_data.ObjectSizeMinCluster + noise * (double) (this.m_data.ObjectSizeMaxCluster - this.m_data.ObjectSizeMinCluster);

    private static Vector3I GetAsteroidVoxelSize(double asteroidRadius) => new Vector3I(Math.Max(64, (int) Math.Ceiling(asteroidRadius)));

    public override void ReclaimObject(object reclaimedObject)
    {
      if (!(reclaimedObject is MyVoxelBase myVoxelBase) || myVoxelBase.Storage == null || !(myVoxelBase.Storage.DataProvider is MyCompositeShapeProvider))
        return;
      Vector3I_RangeIterator cellsIterator = this.GetCellsIterator(MyGamePruningStructure.GetEntityAABB((MyEntity) myVoxelBase));
      while (cellsIterator.IsValid())
      {
        if (!this.m_cells.TryGetValue(cellsIterator.Current, out MyProceduralCell _))
          myVoxelBase.Close();
        cellsIterator.MoveNext();
      }
    }

    internal void CloseObjectSeed(Vector3I cellId, int seed)
    {
      MyProceduralCell myProceduralCell;
      if (!this.m_cells.TryGetValue(cellId, out myProceduralCell))
        return;
      List<MyObjectSeed> list = new List<MyObjectSeed>();
      myProceduralCell.GetAll(list);
      foreach (MyObjectSeed objectSeed in list)
      {
        if (objectSeed.Params.Seed == seed)
        {
          this.CloseObjectSeed(objectSeed);
          break;
        }
      }
    }

    protected override void CloseObjectSeed(MyObjectSeed objectSeed)
    {
      switch (objectSeed.Params.Type)
      {
        case MyObjectSeedType.Empty:
          break;
        case MyObjectSeedType.Asteroid:
        case MyObjectSeedType.AsteroidCluster:
          BoundingBoxD boundingVolume = objectSeed.BoundingVolume;
          MyGamePruningStructure.GetAllVoxelMapsInBox(ref boundingVolume, this.m_tmpVoxelMapsList);
          string str = string.Format("Asteroid_{0}_{1}_{2}_{3}_{4}", (object) objectSeed.CellId.X, (object) objectSeed.CellId.Y, (object) objectSeed.CellId.Z, (object) objectSeed.Params.Index, (object) objectSeed.Params.Seed);
          foreach (MyVoxelBase tmpVoxelMaps in this.m_tmpVoxelMapsList)
          {
            if (tmpVoxelMaps.StorageName == str)
            {
              if (!tmpVoxelMaps.Save)
              {
                this.m_isClosingEntities = true;
                tmpVoxelMaps.Close();
                this.m_isClosingEntities = false;
              }
              tmpVoxelMaps.IsSeedOpen = new bool?(false);
              break;
            }
          }
          this.m_tmpVoxelMapsList.Clear();
          break;
        case MyObjectSeedType.EncounterAlone:
        case MyObjectSeedType.EncounterSingle:
          MyEncounterGenerator.Static.RemoveEncounter(objectSeed.BoundingVolume, objectSeed.Params.Seed);
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    private static double GetSubCellInfo()
    {
      MyAsteroidGeneratorDefinition data = MyProceduralAsteroidCellGenerator.GetData();
      return (double) (data.SubCells * data.SubcellSize);
    }

    private static MatrixD CreateAsteroidRotation(
      MyRandom random,
      Vector3D offset,
      Vector3I storageSize)
    {
      MatrixD translation = MatrixD.CreateTranslation(offset + storageSize / 2);
      Matrix matrix = Matrix.CreateRotationZ((float) ((double) random.NextFloat() * Math.PI * 2.0)) * Matrix.CreateRotationX((float) ((double) random.NextFloat() * Math.PI * 2.0)) * Matrix.CreateRotationY((float) ((double) random.NextFloat() * Math.PI * 2.0));
      return MatrixD.CreateTranslation(new Vector3(storageSize / 2)) * matrix * translation;
    }

    private static MyAsteroidGeneratorDefinition GetData()
    {
      MyAsteroidGeneratorDefinition generatorDefinition1 = (MyAsteroidGeneratorDefinition) null;
      int generatorVersion = MySession.Static.Settings.VoxelGeneratorVersion;
      foreach (MyAsteroidGeneratorDefinition generatorDefinition2 in MyDefinitionManager.Static.GetAsteroidGeneratorDefinitions().Values)
      {
        if (generatorDefinition2.Version == generatorVersion)
        {
          generatorDefinition1 = generatorDefinition2;
          break;
        }
      }
      if (generatorDefinition1 == null)
      {
        MyLog.Default.WriteLine("Generator of version " + (object) generatorVersion + "not found!");
        foreach (MyAsteroidGeneratorDefinition generatorDefinition2 in MyDefinitionManager.Static.GetAsteroidGeneratorDefinitions().Values)
        {
          if (generatorDefinition1 == null || generatorDefinition2.Version > generatorVersion && (generatorDefinition1.Version < generatorVersion || generatorDefinition2.Version < generatorDefinition1.Version))
            generatorDefinition1 = generatorDefinition2;
        }
      }
      return generatorDefinition1;
    }
  }
}
