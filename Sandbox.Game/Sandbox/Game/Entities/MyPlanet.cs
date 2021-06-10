// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyPlanet
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Memory;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_Planet), true)]
  public class MyPlanet : MyVoxelBase, IMyOxygenProvider
  {
    public static MyMemorySystem MemoryTracker = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Planets");
    public const int PHYSICS_SECTOR_SIZE_METERS = 1024;
    private const double INTRASECTOR_OBJECT_CLUSTER_SIZE = 512.0;
    public static bool RUN_SECTORS = false;
    private List<BoundingBoxD> m_clustersIntersection = new List<BoundingBoxD>();
    private MyConcurrentDictionary<Vector3I, MyVoxelPhysics> m_physicsShapes;
    private HashSet<Vector3I> m_sectorsPhysicsToRemove = new HashSet<Vector3I>();
    private Vector3I m_numCells;
    private bool m_canSpawnSectors = true;
    private MyPlanetInitArguments m_planetInitValues;
    private List<MyEntity> m_entities = new List<MyEntity>();
    private MyDynamicAABBTreeD m_children;

    public float AtmosphereAltitude { get; private set; }

    public bool IsUnderGround(Vector3D position)
    {
      double num = (position - this.WorldMatrix.Translation).Length();
      return num < (double) this.MinimumRadius || num <= (double) this.MaximumRadius && num - (this.GetClosestSurfacePointGlobal(position) - this.WorldMatrix.Translation).Length() < 0.0;
    }

    bool IMyOxygenProvider.IsPositionInRange(Vector3D worldPoint) => this.Generator != null && this.Generator.HasAtmosphere && this.Generator.Atmosphere.Breathable && (this.WorldMatrix.Translation - worldPoint).Length() < (double) this.AtmosphereAltitude + (double) this.AverageRadius;

    public float GetOxygenForPosition(Vector3D worldPoint)
    {
      if (this.Generator == null || !this.Generator.Atmosphere.Breathable)
        return 0.0f;
      return MySession.Static.GetComponent<MySectorWeatherComponent>() != null && MySession.Static.GetComponent<MySectorWeatherComponent>().Initialized ? this.GetAirDensity(worldPoint) * this.Generator.Atmosphere.OxygenDensity * MySession.Static.GetComponent<MySectorWeatherComponent>().GetOxygenMultiplier(worldPoint) : this.GetAirDensity(worldPoint) * this.Generator.Atmosphere.OxygenDensity;
    }

    public float GetAirDensity(Vector3D worldPosition) => this.Generator == null || !this.Generator.HasAtmosphere ? 0.0f : (float) MathHelper.Clamp(1.0 - ((worldPosition - this.WorldMatrix.Translation).Length() - (double) this.AverageRadius) / (double) this.AtmosphereAltitude, 0.0, 1.0) * this.Generator.Atmosphere.Density;

    public float GetWindSpeed(Vector3D worldPosition) => this.Generator == null ? 0.0f : this.Generator.Atmosphere.MaxWindSpeed * this.GetAirDensity(worldPosition);

    public MyPlanetStorageProvider Provider { get; private set; }

    public override MyVoxelBase RootVoxel => (MyVoxelBase) this;

    public MyPlanetGeneratorDefinition Generator { get; private set; }

    public new VRage.Game.Voxels.IMyStorage Storage
    {
      get => this.m_storage;
      set
      {
        bool flag = false;
        if (this.m_storage != null)
        {
          this.m_storage.RangeChanged -= new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChangedPlanet);
          this.m_storage.Close();
          flag = true;
        }
        this.m_storage = value;
        this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChangedPlanet);
        this.m_storageMax = this.m_storage.Size;
        this.Provider = (MyPlanetStorageProvider) this.Storage.DataProvider;
        if (!flag)
          return;
        this.ClearPhysicsShapes();
        (this.Render as MyRenderComponentVoxelMap).Clipmap.InvalidateAll();
      }
    }

    public override Vector3D PositionLeftBottomCorner
    {
      get => base.PositionLeftBottomCorner;
      set
      {
        if (!(value != base.PositionLeftBottomCorner))
          return;
        base.PositionLeftBottomCorner = value;
        if (this.m_physicsShapes == null)
          return;
        foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in this.m_physicsShapes)
        {
          if (physicsShape.Value != null)
          {
            Vector3D vector3D = this.PositionLeftBottomCorner + physicsShape.Key * 1024 * 1f;
            physicsShape.Value.PositionLeftBottomCorner = vector3D;
            physicsShape.Value.PositionComp.SetPosition(vector3D + physicsShape.Value.Size * 0.5f);
          }
        }
      }
    }

    public void AddToStationOreBlockTree(
      ref MyDynamicAABBTree stationOreBlockTree,
      Vector3D position,
      float radius)
    {
      MyVoxelBase rootVoxel = this.RootVoxel;
      Vector3 vector3 = new Vector3(radius);
      Vector3D worldPosition1 = position - vector3;
      Vector3D worldPosition2 = position + vector3;
      Vector3 localPosition1;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(rootVoxel.PositionLeftBottomCorner, ref worldPosition1, out localPosition1);
      Vector3 localPosition2;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(rootVoxel.PositionLeftBottomCorner, ref worldPosition2, out localPosition2);
      BoundingBox aabb = new BoundingBox(localPosition1, localPosition2);
      stationOreBlockTree.AddProxy(ref aabb, (object) null, 0U);
    }

    public void SetStationOreBlockTree(MyDynamicAABBTree tree)
    {
      this.Provider.Material.SetStationOreBlockTree(tree);
      if (!(this.Storage.DataProvider is MyPlanetStorageProvider dataProvider))
        return;
      dataProvider.Material.SetStationOreBlockTree(tree);
    }

    internal void VoxelStorageUpdated() => this.SetStationOreBlockTree(MySession.Static.GetComponent<MySessionComponentEconomy>().GetStationBlockTree(this.EntityId));

    public MyPlanetInitArguments GetInitArguments => this.m_planetInitValues;

    public Vector3 AtmosphereWavelengths => this.m_planetInitValues.AtmosphereWavelengths;

    public MyAtmosphereSettings AtmosphereSettings
    {
      get => this.m_planetInitValues.AtmosphereSettings;
      set
      {
        this.m_planetInitValues.AtmosphereSettings = value;
        (this.Render as MyRenderComponentPlanet).UpdateAtmosphereSettings(value);
      }
    }

    public float MinimumRadius => this.Provider == null ? 0.0f : this.Provider.Shape.InnerRadius;

    public float AverageRadius => this.Provider == null ? 0.0f : this.Provider.Shape.Radius;

    public float MaximumRadius => this.Provider == null ? 0.0f : this.Provider.Shape.OuterRadius;

    public float AtmosphereRadius => this.m_planetInitValues.AtmosphereRadius;

    public bool HasAtmosphere => this.m_planetInitValues.HasAtmosphere;

    public bool SpherizeWithDistance => this.m_planetInitValues.SpherizeWithDistance;

    public MyPlanet()
    {
      (this.PositionComp as MyPositionComponent).WorldPositionChanged = new Action<object>(((MyVoxelBase) this).WorldPositionChanged);
      this.Render = (MyRenderComponentBase) new MyRenderComponentPlanet();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentPlanet(this));
      this.Render.DrawOutsideViewDistance = true;
    }

    public override void Init(MyObjectBuilder_EntityBase builder) => this.Init(builder, (VRage.Game.Voxels.IMyStorage) null);

    public override void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage)
    {
      this.SyncFlag = true;
      base.Init(builder);
      MyObjectBuilder_Planet objectBuilderPlanet = (MyObjectBuilder_Planet) builder;
      if (objectBuilderPlanet == null)
        return;
      MyLog.Default.WriteLine("Planet init info - MutableStorage:" + objectBuilderPlanet.MutableStorage.ToString() + " StorageName:" + objectBuilderPlanet.StorageName + " storage?:" + (storage != null).ToString());
      if (objectBuilderPlanet.MutableStorage)
        this.StorageName = objectBuilderPlanet.StorageName;
      else
        this.StorageName = string.Format("{0}", (object) objectBuilderPlanet.StorageName);
      this.m_planetInitValues.Seed = objectBuilderPlanet.Seed;
      this.m_planetInitValues.StorageName = this.StorageName;
      this.m_planetInitValues.PositionMinCorner = (Vector3D) objectBuilderPlanet.PositionAndOrientation.Value.Position;
      this.m_planetInitValues.HasAtmosphere = objectBuilderPlanet.HasAtmosphere;
      this.m_planetInitValues.AtmosphereRadius = objectBuilderPlanet.AtmosphereRadius;
      this.m_planetInitValues.AtmosphereWavelengths = objectBuilderPlanet.AtmosphereWavelengths;
      this.m_planetInitValues.GravityFalloff = objectBuilderPlanet.GravityFalloff;
      this.m_planetInitValues.MarkAreaEmpty = objectBuilderPlanet.MarkAreaEmpty;
      this.m_planetInitValues.SurfaceGravity = objectBuilderPlanet.SurfaceGravity;
      this.m_planetInitValues.AddGps = objectBuilderPlanet.ShowGPS;
      this.m_planetInitValues.SpherizeWithDistance = objectBuilderPlanet.SpherizeWithDistance;
      this.m_planetInitValues.Generator = objectBuilderPlanet.PlanetGenerator == "" ? (MyPlanetGeneratorDefinition) null : MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(objectBuilderPlanet.PlanetGenerator));
      if (this.m_planetInitValues.Generator == null)
      {
        string str = "No definition found for planet generator " + objectBuilderPlanet.PlanetGenerator + ".";
        MyLog.Default.WriteLine(str);
        throw new MyIncompatibleDataException(str);
      }
      this.m_planetInitValues.AtmosphereSettings = this.m_planetInitValues.Generator.AtmosphereSettings ?? MyAtmosphereSettings.Defaults();
      this.m_planetInitValues.UserCreated = false;
      if (storage != null)
      {
        this.m_planetInitValues.Storage = storage;
      }
      else
      {
        this.m_planetInitValues.Storage = (VRage.Game.Voxels.IMyStorage) MyStorageBase.Load(objectBuilderPlanet.StorageName, false);
        if (this.m_planetInitValues.Storage == null)
        {
          string str = "No storage loaded for planet " + objectBuilderPlanet.StorageName + ".";
          MyLog.Default.WriteLine(str);
          throw new MyIncompatibleDataException(str);
        }
      }
      this.m_planetInitValues.InitializeComponents = false;
      MyLog.Default.Log(MyLogSeverity.Info, "Planet generator name: {0}", (object) (objectBuilderPlanet.PlanetGenerator ?? "<null>"));
      this.Init(this.m_planetInitValues);
    }

    public void Init(MyPlanetInitArguments arguments)
    {
      this.SyncFlag = true;
      this.m_planetInitValues = arguments;
      MyLog.Default.Log(MyLogSeverity.Info, "Planet init values: {0}", (object) this.m_planetInitValues.ToString());
      if (this.m_planetInitValues.Storage == null)
      {
        MyLog.Default.Log(MyLogSeverity.Error, "MyPlanet.Init: Planet storage is null! Init of the planet was cancelled.");
      }
      else
      {
        this.Provider = this.m_planetInitValues.Storage.DataProvider as MyPlanetStorageProvider;
        if (this.Provider == null)
          MyLog.Default.Error("MyPlanet.Init: Planet storage provider is null! Init of the planet was cancelled.");
        else if (arguments.Generator == null)
        {
          MyLog.Default.Error("MyPlanet.Init: Planet generator is null! Init of the planet was cancelled.");
        }
        else
        {
          this.m_planetInitValues.Radius = this.Provider.Radius;
          this.m_planetInitValues.MaxRadius = this.Provider.Shape.OuterRadius;
          this.m_planetInitValues.MinRadius = this.Provider.Shape.InnerRadius;
          this.Generator = arguments.Generator;
          this.AtmosphereAltitude = this.Provider.Shape.MaxHillHeight * (this.Generator != null ? this.Generator.Atmosphere.LimitAltitude : 1f);
          this.Init(this.m_planetInitValues.StorageName, this.m_planetInitValues.Storage, this.m_planetInitValues.PositionMinCorner);
          ((MyStorageBase) this.Storage).InitWriteCache();
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
          this.m_storage.RangeChanged += new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChangedPlanet);
          if (this.m_planetInitValues.MarkAreaEmpty && MyProceduralWorldGenerator.Static != null)
            MyProceduralWorldGenerator.Static.MarkEmptyArea(this.PositionComp.GetPosition(), this.m_planetInitValues.MaxRadius);
          if (this.Physics != null)
          {
            this.Physics.Enabled = false;
            this.Physics.Close();
            this.Physics = (MyPhysicsComponentBase) null;
          }
          if (this.Name == null)
            this.Name = this.StorageName + "-" + this.EntityId.ToString();
          Vector3I size = this.m_planetInitValues.Storage.Size;
          this.m_numCells = new Vector3I(size.X / 1024, size.Y / 1024, size.Z / 1024);
          this.m_numCells -= 1;
          this.m_numCells = Vector3I.Max(Vector3I.Zero, this.m_numCells);
          this.StorageName = this.m_planetInitValues.StorageName;
          this.AsteroidName = this.m_planetInitValues.StorageName;
          this.m_storageMax = this.m_planetInitValues.Storage.Size;
          this.PrepareSectors();
          if (this.Generator != null && this.Generator.EnvironmentDefinition != null)
          {
            if (!this.Components.Contains(typeof (MyPlanetEnvironmentComponent)))
              this.Components.Add<MyPlanetEnvironmentComponent>(new MyPlanetEnvironmentComponent());
            this.Components.Get<MyPlanetEnvironmentComponent>().InitEnvironment();
          }
          this.Components.Add<MyGravityProviderComponent>((MyGravityProviderComponent) new MySphericalNaturalGravityComponent((double) this.m_planetInitValues.MinRadius, (double) this.m_planetInitValues.MaxRadius, (double) this.m_planetInitValues.GravityFalloff, (double) this.m_planetInitValues.SurfaceGravity));
          this.CreatedByUser = this.m_planetInitValues.UserCreated;
          this.Render.FadeIn = this.m_planetInitValues.FadeIn;
        }
      }
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_Planet objectBuilder = (MyObjectBuilder_Planet) base.GetObjectBuilder(copy);
      objectBuilder.Radius = this.m_planetInitValues.Radius;
      objectBuilder.Seed = this.m_planetInitValues.Seed;
      objectBuilder.HasAtmosphere = this.m_planetInitValues.HasAtmosphere;
      objectBuilder.AtmosphereRadius = this.m_planetInitValues.AtmosphereRadius;
      objectBuilder.MinimumSurfaceRadius = this.m_planetInitValues.MinRadius;
      objectBuilder.MaximumHillRadius = this.m_planetInitValues.MaxRadius;
      objectBuilder.AtmosphereWavelengths = this.m_planetInitValues.AtmosphereWavelengths;
      objectBuilder.GravityFalloff = this.m_planetInitValues.GravityFalloff;
      objectBuilder.MarkAreaEmpty = this.m_planetInitValues.MarkAreaEmpty;
      objectBuilder.AtmosphereSettings = new MyAtmosphereSettings?(this.m_planetInitValues.AtmosphereSettings);
      objectBuilder.SurfaceGravity = this.m_planetInitValues.SurfaceGravity;
      objectBuilder.ShowGPS = this.m_planetInitValues.AddGps;
      objectBuilder.SpherizeWithDistance = this.m_planetInitValues.SpherizeWithDistance;
      objectBuilder.PlanetGenerator = this.Generator?.Id.SubtypeId.ToString();
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public static void RevertBoulderServer(MyVoxelBase voxels)
    {
      if (!Sync.IsServer || !voxels.BoulderInfo.HasValue)
        return;
      MyMultiplayer.RaiseStaticEvent<long, long, int>((Func<IMyEventOwner, Action<long, long, int>>) (x => new Action<long, long, int>(MyPlanet.RevertBoulderBroadcast)), voxels.BoulderInfo.Value.PlanetId, voxels.BoulderInfo.Value.SectorId, voxels.BoulderInfo.Value.ItemId);
      MyPlanet.RevertBoulder(voxels.BoulderInfo.Value);
    }

    [Event(null, 612)]
    [Reliable]
    [Broadcast]
    public static void RevertBoulderBroadcast(long planetId, long sectorId, int itemId) => MyPlanet.RevertBoulder(new MyBoulderInformation()
    {
      PlanetId = planetId,
      SectorId = sectorId,
      ItemId = itemId
    });

    public static void RevertBoulder(MyBoulderInformation boulder)
    {
      if (!MyEntities.EntityExists(boulder.PlanetId) || !(MyEntities.GetEntityById(boulder.PlanetId) is MyPlanet entityById) || entityById.Closed)
        return;
      entityById.Components.Get<MyPlanetEnvironmentComponent>()?.RevertBoulder(boulder);
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_physicsShapes == null)
        return;
      foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in this.m_physicsShapes)
      {
        if (physicsShape.Value != null)
          physicsShape.Value.Close();
      }
    }

    protected override void BeforeDelete()
    {
      base.BeforeDelete();
      if (this.m_physicsShapes != null)
      {
        foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in this.m_physicsShapes)
        {
          if (physicsShape.Value != null)
          {
            MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase) physicsShape.Value);
            physicsShape.Value.RemoveFromGamePruningStructure();
          }
        }
      }
      MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase) this);
      if (this.m_storage != null)
      {
        this.m_storage.RangeChanged -= new Action<Vector3I, Vector3I, MyStorageDataTypeFlags>(this.storage_RangeChangedPlanet);
        this.m_storage.Close();
        this.m_storage = (VRage.Game.Voxels.IMyStorage) null;
      }
      this.Provider = (MyPlanetStorageProvider) null;
      this.m_planetInitValues = new MyPlanetInitArguments();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      MyPlanets.Register(this);
      MyGravityProviderSystem.AddNaturalGravityProvider((IMyGravityProvider) this.Components.Get<MyGravityProviderComponent>());
      MyOxygenProviderSystem.AddOxygenGenerator((IMyOxygenProvider) this);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      MyPlanets.UnRegister(this);
      MyGravityProviderSystem.RemoveNaturalGravityProvider((IMyGravityProvider) this.Components.Get<MyGravityProviderComponent>());
      MyOxygenProviderSystem.RemoveOxygenGenerator((IMyOxygenProvider) this);
    }

    private void storage_RangeChangedPlanet(
      Vector3I minChanged,
      Vector3I maxChanged,
      MyStorageDataTypeFlags dataChanged)
    {
      Vector3I start = minChanged / 1024;
      Vector3I end = maxChanged / 1024;
      if (this.m_physicsShapes != null)
      {
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
        while (vector3IRangeIterator.IsValid())
        {
          MyVoxelPhysics myVoxelPhysics;
          if (this.m_physicsShapes.TryGetValue(vector3IRangeIterator.Current, out myVoxelPhysics) && myVoxelPhysics != null)
            myVoxelPhysics.OnStorageChanged(minChanged, maxChanged, dataChanged);
          vector3IRangeIterator.MoveNext();
        }
      }
      if (this.Render is MyRenderComponentVoxelMap)
        (this.Render as MyRenderComponentVoxelMap).InvalidateRange(minChanged, maxChanged);
      this.OnRangeChanged(minChanged, maxChanged, dataChanged);
      this.ContentChanged = true;
    }

    private MyVoxelPhysics CreateVoxelPhysics(
      ref Vector3I increment,
      ref Vector3I_RangeIterator it)
    {
      if (this.m_physicsShapes == null)
        this.m_physicsShapes = new MyConcurrentDictionary<Vector3I, MyVoxelPhysics>();
      MyVoxelPhysics myVoxelPhysics = (MyVoxelPhysics) null;
      if (!this.m_physicsShapes.TryGetValue(it.Current, out myVoxelPhysics) || myVoxelPhysics == null)
      {
        Vector3I storageMin = it.Current * increment;
        Vector3I storageMax = storageMin + increment;
        BoundingBox box = new BoundingBox((Vector3) storageMin, (Vector3) storageMax);
        if (this.Storage.Intersect(ref box, false) == ContainmentType.Intersects)
        {
          myVoxelPhysics = new MyVoxelPhysics();
          myVoxelPhysics.Init(this.m_storage, this.PositionLeftBottomCorner + storageMin * 1f, storageMin, storageMax, this);
          myVoxelPhysics.Save = false;
          MyEntities.Add((MyEntity) myVoxelPhysics);
        }
        this.m_physicsShapes[it.Current] = myVoxelPhysics;
      }
      else if (myVoxelPhysics != null && !myVoxelPhysics.Valid)
        myVoxelPhysics.RefreshPhysics(this.m_storage);
      return myVoxelPhysics;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateFloraAndPhysics(true);
      if (!this.m_planetInitValues.AddGps)
        return;
      MyGps gps = new MyGps()
      {
        Name = this.StorageName,
        Coords = this.PositionComp.GetPosition(),
        ShowOnHud = true
      };
      gps.UpdateHash();
      MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.UpdateFloraAndPhysics();
    }

    public override void BeforePaste()
    {
    }

    public override void AfterPaste()
    {
    }

    private void UpdateFloraAndPhysics(bool serial = false)
    {
      BoundingBoxD worldAabb = this.PositionComp.WorldAABB;
      worldAabb.Min -= 1024.0;
      worldAabb.Max += 1024f;
      this.UpdatePlanetPhysics(ref worldAabb);
    }

    private void UpdatePlanetPhysics(ref BoundingBoxD box)
    {
      Vector3I increment = this.m_storage.Size / (this.m_numCells + 1);
      MyGamePruningStructure.GetAproximateDynamicClustersForSize(ref box, 512.0, this.m_clustersIntersection);
      foreach (BoundingBoxD shapeBox in this.m_clustersIntersection)
      {
        shapeBox.Inflate(32.0);
        this.GeneratePhysicalShapeForBox(ref increment, ref shapeBox);
      }
      if (MySession.Static.ControlledEntity != null)
      {
        BoundingBoxD worldAabb = MySession.Static.ControlledEntity.Entity.PositionComp.WorldAABB;
        worldAabb.Inflate(32.0);
        this.GeneratePhysicalShapeForBox(ref increment, ref worldAabb);
      }
      this.m_clustersIntersection.Clear();
    }

    private void GeneratePhysicalShapeForBox(ref Vector3I increment, ref BoundingBoxD shapeBox)
    {
      if (!shapeBox.Intersects(this.PositionComp.WorldAABB))
        return;
      if (!shapeBox.Valid || !shapeBox.Min.IsValid() || !shapeBox.Max.IsValid())
        throw new ArgumentOutOfRangeException(nameof (shapeBox), "Invalid shapeBox: " + (object) shapeBox);
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref shapeBox.Min, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref shapeBox.Max, out voxelCoord2);
      Vector3I start = voxelCoord1 / 1024;
      Vector3I end = voxelCoord2 / 1024;
      Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref start, ref end);
      while (it.IsValid())
      {
        this.CreateVoxelPhysics(ref increment, ref it);
        it.MoveNext();
      }
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.m_physicsShapes == null)
        return;
      foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in this.m_physicsShapes)
      {
        BoundingBoxD box;
        if (physicsShape.Value != null)
        {
          box = physicsShape.Value.PositionComp.WorldAABB;
          box.Min -= box.HalfExtents;
          box.Max += box.HalfExtents;
        }
        else
        {
          Vector3 vector3 = (Vector3) ((Vector3) physicsShape.Key * 1024f + this.PositionLeftBottomCorner);
          box = new BoundingBoxD((Vector3D) vector3, (Vector3D) (vector3 + 1024f));
        }
        bool flag = false;
        using (MyUtils.ReuseCollection<MyEntity>(ref this.m_entities))
        {
          MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, this.m_entities);
          foreach (MyEntity entity in this.m_entities)
          {
            if (entity.Physics != null)
            {
              if (entity.Physics.IsStatic)
              {
                if (entity is MyCubeGrid myCubeGrid && !myCubeGrid.IsStatic)
                  flag = true;
              }
              else
                flag = true;
            }
          }
        }
        if (!flag)
          this.m_sectorsPhysicsToRemove.Add(physicsShape.Key);
      }
      foreach (Vector3I key in this.m_sectorsPhysicsToRemove)
      {
        MyVoxelPhysics myVoxelPhysics;
        if (this.m_physicsShapes.TryGetValue(key, out myVoxelPhysics) && myVoxelPhysics != null)
          myVoxelPhysics.Close();
        this.m_physicsShapes.Remove(key);
      }
      this.m_sectorsPhysicsToRemove.Clear();
    }

    private void ClearPhysicsShapes()
    {
      if (this.m_physicsShapes == null)
        return;
      foreach (MyVoxelPhysics myVoxelPhysics in this.m_physicsShapes.Values)
      {
        if (myVoxelPhysics != null)
        {
          myVoxelPhysics.Valid = false;
          myVoxelPhysics.Storage = this.Storage;
        }
      }
    }

    public override MyClipmapScaleEnum ScaleGroup => MyClipmapScaleEnum.Massive;

    public bool CorrectSpawnLocation2(ref Vector3D position, double radius, bool resumeSearch = false)
    {
      Vector3D vector3D1 = position - this.WorldMatrix.Translation;
      vector3D1.Normalize();
      Vector3D vector3D2 = new Vector3D(radius, radius, radius);
      Vector3D worldPosition;
      Vector3 localPosition;
      BoundingBox box;
      if (resumeSearch)
      {
        worldPosition = position;
      }
      else
      {
        MyVoxelCoordSystems.WorldPositionToLocalPosition(this.PositionLeftBottomCorner, ref position, out localPosition);
        box = new BoundingBox((Vector3) (localPosition - vector3D2), (Vector3) (localPosition + vector3D2));
        if (this.Storage.Intersect(ref box) == ContainmentType.Disjoint)
          return true;
        worldPosition = this.GetClosestSurfacePointGlobal(ref position);
      }
      for (int index = 0; index < 10; ++index)
      {
        worldPosition += vector3D1 * radius;
        MyVoxelCoordSystems.WorldPositionToLocalPosition(this.PositionLeftBottomCorner, ref worldPosition, out localPosition);
        box = new BoundingBox((Vector3) (localPosition - vector3D2), (Vector3) (localPosition + vector3D2));
        if (this.Storage.Intersect(ref box) == ContainmentType.Disjoint)
        {
          position = worldPosition;
          return true;
        }
      }
      return false;
    }

    public void CorrectSpawnLocation(ref Vector3D position, double radius)
    {
      Vector3D vector3D1 = position - this.WorldMatrix.Translation;
      vector3D1.Normalize();
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(this.PositionLeftBottomCorner, ref position, out localPosition);
      Vector3D vector3D2 = new Vector3D(radius, radius, radius);
      BoundingBox box = new BoundingBox((Vector3) (localPosition - vector3D2), (Vector3) (localPosition + vector3D2));
      ContainmentType containmentType = this.Storage.Intersect(ref box);
      for (int index = 0; index < 10 && (containmentType == ContainmentType.Intersects || containmentType == ContainmentType.Contains); ++index)
      {
        Vector3D surfacePointGlobal = this.GetClosestSurfacePointGlobal(ref position);
        position = surfacePointGlobal + vector3D1 * radius;
        MyVoxelCoordSystems.WorldPositionToLocalPosition(this.PositionLeftBottomCorner, ref position, out localPosition);
        box = new BoundingBox((Vector3) (localPosition - vector3D2), (Vector3) (localPosition + vector3D2));
        containmentType = this.Storage.Intersect(ref box);
      }
    }

    public Vector3D GetClosestSurfacePointGlobal(Vector3D globalPos) => this.GetClosestSurfacePointGlobal(ref globalPos);

    public Vector3D GetClosestSurfacePointGlobal(ref Vector3D globalPos)
    {
      Vector3D translation = this.WorldMatrix.Translation;
      Vector3 localPos = (Vector3) (globalPos - translation);
      return this.GetClosestSurfacePointLocal(ref localPos) + translation;
    }

    public Vector3D GetClosestSurfacePointLocal(ref Vector3 localPos)
    {
      if (!localPos.IsValid())
        return Vector3D.Zero;
      Vector3 surface = Vector3.Zero;
      this.Provider?.Shape.ProjectToSurface(localPos, out surface);
      return (Vector3D) surface;
    }

    public override void DebugDrawPhysics()
    {
      if (this.m_physicsShapes == null)
        return;
      foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in this.m_physicsShapes)
      {
        Vector3 vector3 = (Vector3) ((Vector3) physicsShape.Key * 1024f + this.PositionLeftBottomCorner);
        BoundingBoxD aabb = new BoundingBoxD((Vector3D) vector3, (Vector3D) (vector3 + 1024f));
        if (physicsShape.Value != null && !physicsShape.Value.Closed)
        {
          physicsShape.Value.Physics.DebugDraw();
          MyRenderProxy.DebugDrawAABB(aabb, Color.Cyan);
        }
        else
          MyRenderProxy.DebugDrawAABB(aabb, Color.DarkGreen);
      }
    }

    public override int GetOrePriority() => -1;

    public int GetInstanceHash() => this.Name.GetUniversalHashCode();

    public void PrefetchShapeOnRay(ref LineD ray)
    {
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref ray.From, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.PositionLeftBottomCorner, ref ray.To, out voxelCoord2);
      Vector3I start = voxelCoord1 / 1024;
      Vector3I end = voxelCoord2 / 1024;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        if (this.m_physicsShapes.ContainsKey(vector3IRangeIterator.Current))
          this.m_physicsShapes[vector3IRangeIterator.Current].PrefetchShapeOnRay(ref ray);
        vector3IRangeIterator.MoveNext();
      }
    }

    public bool IntersectsWithGravityFast(ref BoundingBoxD boundingBox)
    {
      ContainmentType result;
      new BoundingSphereD(this.PositionComp.GetPosition(), (double) ((MySphericalNaturalGravityComponent) this.Components.Get<MyGravityProviderComponent>()).GravityLimit).Contains(ref boundingBox, out result);
      return (uint) result > 0U;
    }

    private void PrepareSectors()
    {
      this.m_children = new MyDynamicAABBTreeD(Vector3D.Zero);
      this.Hierarchy.QueryAABBImpl = new Action<BoundingBoxD, List<MyEntity>>(this.Hierarchy_QueryAABB);
      this.Hierarchy.QueryLineImpl = new Action<LineD, List<MyLineSegmentOverlapResult<MyEntity>>>(this.Hierarchy_QueryLine);
      this.Hierarchy.QuerySphereImpl = new Action<BoundingSphereD, List<MyEntity>>(this.Hierarchy_QuerySphere);
    }

    private void Hierarchy_QueryAABB(BoundingBoxD query, List<MyEntity> results) => this.m_children.OverlapAllBoundingBox<MyEntity>(ref query, results, clear: false);

    private void Hierarchy_QuerySphere(BoundingSphereD query, List<MyEntity> results) => this.m_children.OverlapAllBoundingSphere<MyEntity>(ref query, results, false);

    private void Hierarchy_QueryLine(
      LineD query,
      List<MyLineSegmentOverlapResult<MyEntity>> results)
    {
      this.m_children.OverlapAllLineSegment<MyEntity>(ref query, results, false);
    }

    public void AddChildEntity(MyEntity child)
    {
      if (MyFakes.ENABLE_PLANET_HIERARCHY)
      {
        BoundingBoxD worldAabb = child.PositionComp.WorldAABB;
        int num = this.m_children.AddProxy(ref worldAabb, (object) child, 0U);
        this.Hierarchy.AddChild((IMyEntity) child, true);
        child.Components.Get<MyHierarchyComponentBase>().ChildId = (long) num;
      }
      else
        MyEntities.Add(child);
    }

    public void RemoveChildEntity(MyEntity child)
    {
      if (!MyFakes.ENABLE_PLANET_HIERARCHY || child.Parent != this)
        return;
      this.m_children.RemoveProxy((int) child.Components.Get<MyHierarchyComponentBase>().ChildId);
      this.Hierarchy.RemoveChild((IMyEntity) child, true);
    }

    internal void CloseChildEntity(MyEntity child)
    {
      this.RemoveChildEntity(child);
      child.Close();
    }

    protected sealed class RevertBoulderBroadcast\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long planetId,
        in long sectorId,
        in int itemId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlanet.RevertBoulderBroadcast(planetId, sectorId, itemId);
      }
    }

    private class Sandbox_Game_Entities_MyPlanet\u003C\u003EActor : IActivator, IActivator<MyPlanet>
    {
      object IActivator.CreateInstance() => (object) new MyPlanet();

      MyPlanet IActivator<MyPlanet>.CreateInstance() => new MyPlanet();
    }
  }
}
