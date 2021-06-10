// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyProceduralWorldGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof (MyObjectBuilder_WorldGenerator), null, false)]
  [StaticEventOwner]
  public class MyProceduralWorldGenerator : MySessionComponentBase
  {
    private bool m_trackStaticGrids = true;
    public static MyProceduralWorldGenerator Static;
    private int m_seed;
    private double m_objectDensity = -1.0;
    private List<MyProceduralWorldModule> m_modules = new List<MyProceduralWorldModule>();
    private Dictionary<MyEntity, MyEntityTracker> m_trackedEntities = new Dictionary<MyEntity, MyEntityTracker>();
    private Dictionary<MyEntity, MyEntityTracker> m_toAddTrackedEntities = new Dictionary<MyEntity, MyEntityTracker>();
    private HashSet<EmptyArea> m_markedAreas = new HashSet<EmptyArea>();
    private HashSet<EmptyArea> m_deletedAreas = new HashSet<EmptyArea>();
    private HashSet<MyObjectSeedParams> m_existingObjectsSeeds = new HashSet<MyObjectSeedParams>();
    private List<MyProceduralCell> m_tempProceduralCellsList = new List<MyProceduralCell>();
    private List<MyObjectSeed> m_tempObjectSeedList = new List<MyObjectSeed>();
    private MyProceduralPlanetCellGenerator m_planetsModule;
    private MyProceduralAsteroidCellGenerator m_asteroidsModule;

    public bool Enabled { get; private set; }

    public DictionaryReader<MyEntity, MyEntityTracker> GetTrackedEntities() => new DictionaryReader<MyEntity, MyEntityTracker>(this.m_trackedEntities);

    public void GetAllExistingCells(List<MyProceduralCell> list)
    {
      list.Clear();
      foreach (MyProceduralWorldModule module in this.m_modules)
        module.GetAllCells(list);
    }

    public void GetAllExisting(List<MyObjectSeed> list)
    {
      list.Clear();
      this.GetAllExistingCells(this.m_tempProceduralCellsList);
      foreach (MyProceduralCell tempProceduralCells in this.m_tempProceduralCellsList)
        tempProceduralCells.GetAll(list, false);
      this.m_tempProceduralCellsList.Clear();
    }

    public void GetAllInSphere(BoundingSphereD area, List<MyObjectSeed> list)
    {
      foreach (MyProceduralWorldModule module in this.m_modules)
      {
        module.GetObjectSeeds(area, list, false);
        module.MarkCellsDirty(area, scale: false);
      }
    }

    public void GetAllInSphere<T>(BoundingSphereD area, List<MyObjectSeed> list) where T : MyProceduralWorldModule
    {
      foreach (MyProceduralWorldModule module in this.m_modules)
      {
        if (module is T)
        {
          module.GetObjectSeeds(area, list, false);
          break;
        }
      }
    }

    public void GetOverlapAllBoundingBox<T>(BoundingBoxD area, List<MyObjectSeed> list) where T : MyProceduralWorldModule
    {
      foreach (MyProceduralWorldModule module in this.m_modules)
      {
        if (module is T)
        {
          module.OverlapAllBoundingBox(ref area, list);
          break;
        }
      }
    }

    public void OverlapAllPlanetSeedsInSphere(BoundingSphereD area, List<MyObjectSeed> list)
    {
      if (this.m_planetsModule == null)
        return;
      this.m_planetsModule.GetObjectSeeds(area, list, false);
      this.m_planetsModule.MarkCellsDirty(area, scale: false);
    }

    public void OverlapAllAsteroidSeedsInSphere(BoundingSphereD area, List<MyObjectSeed> list)
    {
      if (this.m_asteroidsModule == null)
        return;
      this.m_asteroidsModule.GetObjectSeeds(area, list, false);
      this.m_asteroidsModule.MarkCellsDirty(area, scale: false);
    }

    public override void LoadData()
    {
      MyProceduralWorldGenerator.Static = this;
      this.Enabled = true;
      MySandboxGame.Log.WriteLine("Loading Procedural World Generator");
      if (Sync.IsServer)
        this.m_modules.Add((MyProceduralWorldModule) new MyStationCellGenerator(25000.0, 1, 0, 1.0));
      if (!MyFakes.ENABLE_ASTEROID_FIELDS)
        return;
      MyObjectBuilder_SessionSettings settings = MySession.Static.Settings;
      if ((double) settings.ProceduralDensity == 0.0)
      {
        MySandboxGame.Log.WriteLine("Procedural Density is 0. Skipping Procedural World Generator for asteroids and encounters.");
      }
      else
      {
        this.m_seed = settings.ProceduralSeed;
        this.m_objectDensity = (double) MathHelper.Clamp((float) ((double) settings.ProceduralDensity * 2.0 - 1.0), -1f, 1f);
        MySandboxGame.Log.WriteLine(string.Format("Loading Procedural World Generator: Seed = '{0}' = {1}, Density = {2}", (object) settings.ProceduralSeed, (object) this.m_seed, (object) settings.ProceduralDensity));
        using (MyRandom.Instance.PushSeed(this.m_seed))
        {
          this.m_asteroidsModule = new MyProceduralAsteroidCellGenerator(this.m_seed, this.m_objectDensity);
          this.m_modules.Add((MyProceduralWorldModule) this.m_asteroidsModule);
        }
      }
    }

    protected override void UnloadData()
    {
      this.Enabled = false;
      MySandboxGame.Log.WriteLine("Unloading Procedural World Generator");
      foreach (MyProceduralWorldModule module in this.m_modules)
        module.CleanUp();
      this.m_modules.Clear();
      this.m_trackedEntities.Clear();
      this.m_tempObjectSeedList.Clear();
      this.m_existingObjectsSeeds.Clear();
      this.m_tempProceduralCellsList.Clear();
      this.Session = (IMySession) null;
      MyProceduralWorldGenerator.Static = (MyProceduralWorldGenerator) null;
    }

    public void ReclaimObject(object reclaimedObject)
    {
      foreach (MyProceduralWorldModule module in this.m_modules)
        module.ReclaimObject(reclaimedObject);
    }

    public override void UpdateBeforeSimulation()
    {
      if (this.Enabled)
      {
        if (this.m_toAddTrackedEntities.Count != 0)
        {
          foreach (KeyValuePair<MyEntity, MyEntityTracker> addTrackedEntity in this.m_toAddTrackedEntities)
            this.m_trackedEntities.Add(addTrackedEntity.Key, addTrackedEntity.Value);
          this.m_toAddTrackedEntities.Clear();
        }
        foreach (MyEntityTracker myEntityTracker in this.m_trackedEntities.Values)
        {
          if (myEntityTracker.ShouldGenerate(this.m_trackStaticGrids))
          {
            BoundingSphereD boundingVolume = myEntityTracker.BoundingVolume;
            myEntityTracker.UpdateLastPosition();
            foreach (MyProceduralWorldModule module in this.m_modules)
            {
              module.GetObjectSeeds(myEntityTracker.BoundingVolume, this.m_tempObjectSeedList);
              module.GenerateObjects(this.m_tempObjectSeedList, this.m_existingObjectsSeeds);
              this.m_tempObjectSeedList.Clear();
              module.MarkCellsDirty(boundingVolume, new BoundingSphereD?(myEntityTracker.BoundingVolume));
            }
          }
        }
        foreach (MyProceduralWorldModule module in this.m_modules)
          module.ProcessDirtyCells(this.m_trackedEntities);
        if (this.m_trackStaticGrids && MySession.Static.GameplayFrameCounter % 100 == 99)
        {
          this.m_trackStaticGrids = false;
          List<MyCubeGrid> myCubeGridList = new List<MyCubeGrid>();
          foreach (KeyValuePair<MyEntity, MyEntityTracker> trackedEntity in this.m_trackedEntities)
          {
            MyEntity k;
            trackedEntity.Deconstruct<MyEntity, MyEntityTracker>(out k, out MyEntityTracker _);
            if (k is MyCubeGrid myCubeGrid && myCubeGrid.IsStatic)
              myCubeGridList.Add(myCubeGrid);
          }
          foreach (MyCubeGrid grid in myCubeGridList)
            this.OnGridStaticChanged(grid, true);
        }
      }
      if (MySandboxGame.AreClipmapsReady || (MySession.Static.VoxelMaps.Instances.Count != 0 || !Sync.IsServer))
        return;
      MySandboxGame.AreClipmapsReady = true;
    }

    public void TrackEntity(MyEntity entity)
    {
      if (!this.Enabled)
        return;
      if (entity is MyCharacter)
      {
        int num = MySession.Static.Settings.ViewDistance;
        if (MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
          num = 50000;
        this.TrackEntity(entity, (double) num);
      }
      if (!(entity is MyCubeGrid myCubeGrid) || myCubeGrid.IsStatic && !this.m_trackStaticGrids)
        return;
      this.TrackEntity(entity, entity.PositionComp.WorldAABB.HalfExtents.Length());
      myCubeGrid.OnStaticChanged += new Action<MyCubeGrid, bool>(this.OnGridStaticChanged);
    }

    private void OnGridStaticChanged(MyCubeGrid grid, bool newIsStatic)
    {
      if (!newIsStatic || this.m_trackStaticGrids)
        MyProceduralWorldGenerator.Static.TrackEntity((MyEntity) grid, grid.PositionComp.WorldAABB.HalfExtents.Length());
      else
        MyProceduralWorldGenerator.Static.RemoveTrackedEntity((MyEntity) grid);
    }

    private void TrackEntity(MyEntity entity, double range)
    {
      MyEntityTracker myEntityTracker;
      if (this.m_trackedEntities.TryGetValue(entity, out myEntityTracker) || this.m_toAddTrackedEntities.TryGetValue(entity, out myEntityTracker))
      {
        myEntityTracker.Radius = range;
      }
      else
      {
        myEntityTracker = new MyEntityTracker(entity, range);
        this.m_toAddTrackedEntities.Add(entity, myEntityTracker);
        entity.OnClose += (Action<MyEntity>) (e =>
        {
          this.RemoveTrackedEntity(e);
          if (!(entity is MyCubeGrid myCubeGrid))
            return;
          myCubeGrid.OnStaticChanged -= new Action<MyCubeGrid, bool>(this.OnGridStaticChanged);
        });
      }
    }

    public void RemoveTrackedEntity(MyEntity entity)
    {
      MyEntityTracker myEntityTracker;
      if (!this.m_trackedEntities.TryGetValue(entity, out myEntityTracker))
        return;
      this.m_trackedEntities.Remove(entity);
      this.m_toAddTrackedEntities.Remove(entity);
      foreach (MyProceduralWorldModule module in this.m_modules)
        module.MarkCellsDirty(myEntityTracker.BoundingVolume);
    }

    public override bool UpdatedBeforeInit() => true;

    public static Vector3D GetRandomDirection(MyRandom random)
    {
      double num1 = random.NextDouble() * 2.0 * Math.PI;
      double z = random.NextDouble() * 2.0 - 1.0;
      double num2 = Math.Sqrt(1.0 - z * z);
      return new Vector3D(num2 * Math.Cos(num1), num2 * Math.Sin(num1), z);
    }

    public void MarkDeletedArea(Vector3D pos, float radius)
    {
      this.MarkModules(pos, radius, false);
      lock (this.m_deletedAreas)
        this.m_deletedAreas.Add(new EmptyArea()
        {
          Position = pos,
          Radius = radius
        });
    }

    public void MarkEmptyArea(Vector3D pos, float radius)
    {
      this.MarkModules(pos, radius, true);
      lock (this.m_deletedAreas)
        this.m_markedAreas.Add(new EmptyArea()
        {
          Position = pos,
          Radius = radius
        });
    }

    private void MarkModules(Vector3D pos, float radius, bool planet)
    {
      MySphereDensityFunction sphereDensityFunction = !planet ? new MySphereDensityFunction(pos, (double) radius, 0.0) : new MySphereDensityFunction(pos, (double) radius * 1.1 + 16000.0, 16000.0);
      foreach (MyProceduralWorldModule module in this.m_modules)
        module.AddDensityFunctionRemoved((IMyAsteroidFieldDensityFunction) sphereDensityFunction);
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_WorldGenerator builderWorldGenerator = (MyObjectBuilder_WorldGenerator) sessionComponent;
      if (!Sync.IsServer)
        this.m_markedAreas = builderWorldGenerator.MarkedAreas;
      this.m_deletedAreas = builderWorldGenerator.DeletedAreas;
      this.m_existingObjectsSeeds = builderWorldGenerator.ExistingObjectsSeeds;
      if (this.m_markedAreas == null)
        this.m_markedAreas = new HashSet<EmptyArea>();
      foreach (EmptyArea markedArea in this.m_markedAreas)
        this.MarkModules(markedArea.Position, markedArea.Radius, true);
      foreach (EmptyArea deletedArea in this.m_deletedAreas)
        this.MarkModules(deletedArea.Position, deletedArea.Radius, false);
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_WorldGenerator objectBuilder = (MyObjectBuilder_WorldGenerator) base.GetObjectBuilder();
      objectBuilder.MarkedAreas = this.m_markedAreas;
      objectBuilder.DeletedAreas = this.m_deletedAreas;
      objectBuilder.ExistingObjectsSeeds = this.m_existingObjectsSeeds;
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    [Event(null, 603)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    public static void AddExistingObjectsSeed(MyObjectSeedParams seed) => MyProceduralWorldGenerator.Static.m_existingObjectsSeeds.Add(seed);

    public override Type[] Dependencies => new Type[2]
    {
      typeof (MySector),
      typeof (MyEncounterGenerator)
    };

    internal void CloseAsteroidSeed(Vector3I cellId, int seed)
    {
      foreach (MyProceduralWorldModule module in this.m_modules)
      {
        if (module is MyProceduralAsteroidCellGenerator asteroidCellGenerator)
          asteroidCellGenerator.CloseObjectSeed(cellId, seed);
      }
    }

    protected sealed class AddExistingObjectsSeed\u003C\u003EVRage_Game_MyObjectSeedParams : ICallSite<IMyEventOwner, MyObjectSeedParams, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectSeedParams seed,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyProceduralWorldGenerator.AddExistingObjectsSeed(seed);
      }
    }
  }
}
