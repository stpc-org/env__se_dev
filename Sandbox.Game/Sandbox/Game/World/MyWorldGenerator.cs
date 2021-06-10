// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyWorldGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Common;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
  public class MyWorldGenerator
  {
    private static List<MyCubeGrid> m_tmpSpawnedGridList = new List<MyCubeGrid>();

    public static event ActionRef<MyWorldGenerator.Args> OnAfterGenerate;

    static MyWorldGenerator()
    {
      if (!MyFakes.TEST_PREFABS_FOR_INCONSISTENCIES)
        return;
      foreach (string file in Directory.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data", "Prefabs")))
      {
        if (!(Path.GetExtension(file) != ".sbc"))
        {
          MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) null;
          MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_CubeGrid>(Path.Combine(MyFileSystem.ContentPath, file), out objectBuilder);
          if (objectBuilder != null)
          {
            foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilder.CubeBlocks)
            {
              if ((double) cubeBlock.IntegrityPercent == 0.0)
                break;
            }
          }
        }
      }
      foreach (string directory in Directory.GetDirectories(Path.Combine(MyFileSystem.ContentPath, "Worlds")))
      {
        foreach (string file in Directory.GetFiles(directory))
        {
          if (!(Path.GetExtension(file) != ".sbs"))
          {
            MyObjectBuilder_Sector objectBuilder = (MyObjectBuilder_Sector) null;
            MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(Path.Combine(MyFileSystem.ContentPath, file), out objectBuilder);
            foreach (MyObjectBuilder_EntityBase sectorObject in objectBuilder.SectorObjects)
            {
              if (sectorObject.TypeId == typeof (MyObjectBuilder_CubeGrid))
              {
                foreach (MyObjectBuilder_CubeBlock cubeBlock in ((MyObjectBuilder_CubeGrid) sectorObject).CubeBlocks)
                {
                  if ((double) cubeBlock.IntegrityPercent == 0.0)
                    break;
                }
              }
            }
          }
        }
      }
    }

    public static string GetPrefabTypeName(MyObjectBuilder_EntityBase entity)
    {
      switch (entity)
      {
        case MyObjectBuilder_VoxelMap _:
          return "Asteroid";
        case MyObjectBuilder_CubeGrid _:
          MyObjectBuilder_CubeGrid objectBuilderCubeGrid = (MyObjectBuilder_CubeGrid) entity;
          if (objectBuilderCubeGrid.IsStatic)
            return "Station";
          return objectBuilderCubeGrid.GridSizeEnum == MyCubeSize.Large ? "LargeShip" : "SmallShip";
        case MyObjectBuilder_Character _:
          return "Character";
        default:
          return "Unknown";
      }
    }

    public static void GenerateWorld(MyWorldGenerator.Args args)
    {
      MySandboxGame.Log.WriteLine("MyWorldGenerator.GenerateWorld - START");
      using (MySandboxGame.Log.IndentUsing())
      {
        MyWorldGenerator.RunGeneratorOperations(ref args);
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          MyWorldGenerator.SetupPlayer(ref args);
        MyWorldGenerator.CallOnAfterGenerate(ref args);
      }
      MySandboxGame.Log.WriteLine("MyWorldGenerator.GenerateWorld - END");
    }

    public static void CallOnAfterGenerate(ref MyWorldGenerator.Args args)
    {
      if (MyWorldGenerator.OnAfterGenerate == null)
        return;
      MyWorldGenerator.OnAfterGenerate(ref args);
    }

    public static void InitInventoryWithDefaults(MyInventory inventory)
    {
      MyObjectBuilder_Inventory newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
      MyWorldGenerator.FillInventoryWithDefaults(newObject, MySession.Static.Scenario);
      inventory.Init(newObject);
    }

    private static void SetupPlayer(ref MyWorldGenerator.Args args)
    {
      MyPlayer newPlayer = Sync.Players.CreateNewPlayer(Sync.Players.CreateNewIdentity(Sync.Clients.LocalClient.DisplayName, (string) null, new Vector3?(), false, false), Sync.Clients.LocalClient, Sync.MyName, true);
      MyWorldGeneratorStartingStateBase[] possiblePlayerStarts = args.Scenario.PossiblePlayerStarts;
      if (possiblePlayerStarts == null || possiblePlayerStarts.Length == 0)
        Sync.Players.RespawnComponent.SetupCharacterDefault(newPlayer, args);
      else
        Sync.Players.RespawnComponent.SetupCharacterFromStarts(newPlayer, possiblePlayerStarts, args);
      MyObjectBuilder_Toolbar defaultToolbar = args.Scenario.DefaultToolbar;
      if (defaultToolbar == null)
        return;
      MyToolbar toolbar = new MyToolbar(MyToolbarType.Character);
      toolbar.Init(defaultToolbar, (MyEntity) newPlayer.Character, true);
      MySession.Static.Toolbars.RemovePlayerToolbar(newPlayer.Id);
      MySession.Static.Toolbars.AddPlayerToolbar(newPlayer.Id, toolbar);
      MyToolbarComponent.InitToolbar(MyToolbarType.Character, defaultToolbar);
      MyToolbarComponent.InitCharacterToolbar(defaultToolbar);
    }

    public static void FillInventoryWithDefaults(
      MyObjectBuilder_Inventory inventory,
      MyScenarioDefinition scenario)
    {
      if (inventory.Items == null)
        inventory.Items = new List<MyObjectBuilder_InventoryItem>();
      else
        inventory.Items.Clear();
      if (scenario == null || !MySession.Static.Settings.SpawnWithTools)
        return;
      MyStringId[] myStringIdArray = !MySession.Static.CreativeMode ? scenario.SurvivalModeWeapons : scenario.CreativeModeWeapons;
      uint num = 0;
      if (myStringIdArray != null)
      {
        foreach (MyStringId myStringId in myStringIdArray)
        {
          MyObjectBuilder_InventoryItem newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
          newObject.Amount = (MyFixedPoint) 1;
          newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(myStringId.ToString());
          newObject.ItemId = num++;
          inventory.Items.Add(newObject);
        }
        inventory.nextItemId = num;
      }
      MyScenarioDefinition.StartingItem[] startingItemArray1 = !MySession.Static.CreativeMode ? scenario.SurvivalModeComponents : scenario.CreativeModeComponents;
      if (startingItemArray1 != null)
      {
        foreach (MyScenarioDefinition.StartingItem startingItem in startingItemArray1)
        {
          MyObjectBuilder_InventoryItem newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
          newObject.Amount = startingItem.amount;
          newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Component>(startingItem.itemName.ToString());
          newObject.ItemId = num++;
          inventory.Items.Add(newObject);
        }
        inventory.nextItemId = num;
      }
      MyScenarioDefinition.StartingPhysicalItem[] startingPhysicalItemArray = !MySession.Static.CreativeMode ? scenario.SurvivalModePhysicalItems : scenario.CreativeModePhysicalItems;
      if (startingPhysicalItemArray != null)
      {
        foreach (MyScenarioDefinition.StartingPhysicalItem startingPhysicalItem in startingPhysicalItemArray)
        {
          MyObjectBuilder_InventoryItem newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
          newObject.Amount = startingPhysicalItem.amount;
          if (startingPhysicalItem.itemType.ToString().Equals("Ore"))
            newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(startingPhysicalItem.itemName.ToString());
          else if (startingPhysicalItem.itemType.ToString().Equals("Ingot"))
            newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ingot>(startingPhysicalItem.itemName.ToString());
          else if (startingPhysicalItem.itemType.ToString().Equals("OxygenBottle"))
          {
            newObject.Amount = (MyFixedPoint) 1;
            newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_OxygenContainerObject>(startingPhysicalItem.itemName.ToString());
            (newObject.PhysicalContent as MyObjectBuilder_GasContainerObject).GasLevel = (float) startingPhysicalItem.amount;
          }
          else if (startingPhysicalItem.itemType.ToString().Equals("GasBottle"))
          {
            newObject.Amount = (MyFixedPoint) 1;
            newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GasContainerObject>(startingPhysicalItem.itemName.ToString());
            (newObject.PhysicalContent as MyObjectBuilder_GasContainerObject).GasLevel = (float) startingPhysicalItem.amount;
          }
          newObject.ItemId = num++;
          inventory.Items.Add(newObject);
        }
        inventory.nextItemId = num;
      }
      MyScenarioDefinition.StartingItem[] startingItemArray2 = !MySession.Static.CreativeMode ? scenario.SurvivalModeAmmoItems : scenario.CreativeModeAmmoItems;
      if (startingItemArray2 != null)
      {
        foreach (MyScenarioDefinition.StartingItem startingItem in startingItemArray2)
        {
          MyObjectBuilder_InventoryItem newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
          newObject.Amount = startingItem.amount;
          newObject.PhysicalContent = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AmmoMagazine>(startingItem.itemName.ToString());
          newObject.ItemId = num++;
          inventory.Items.Add(newObject);
        }
        inventory.nextItemId = num;
      }
      MyObjectBuilder_InventoryItem[] builderInventoryItemArray = MySession.Static.CreativeMode ? scenario.CreativeInventoryItems : scenario.SurvivalInventoryItems;
      if (builderInventoryItemArray == null)
        return;
      foreach (MyObjectBuilder_Base objectBuilderBase in builderInventoryItemArray)
      {
        MyObjectBuilder_InventoryItem builderInventoryItem = objectBuilderBase.Clone() as MyObjectBuilder_InventoryItem;
        builderInventoryItem.ItemId = num++;
        inventory.Items.Add(builderInventoryItem);
      }
      inventory.nextItemId = num;
    }

    private static void RunGeneratorOperations(ref MyWorldGenerator.Args args)
    {
      MyWorldGeneratorOperationBase[] generatorOperations = args.Scenario.WorldGeneratorOperations;
      if (generatorOperations == null || generatorOperations.Length == 0)
        return;
      foreach (MyWorldGeneratorOperationBase generatorOperationBase in generatorOperations)
        generatorOperationBase.Apply();
    }

    public static MyVoxelMap AddAsteroidPrefab(
      string prefabName,
      MatrixD worldMatrix,
      string name)
    {
      MyStorageBase storage = MyWorldGenerator.LoadRandomizedVoxelMapPrefab(MyWorldGenerator.GetVoxelPrefabPath(prefabName));
      return MyWorldGenerator.AddVoxelMap(name, storage, worldMatrix);
    }

    public static MyVoxelMap AddAsteroidPrefab(
      string prefabName,
      Vector3D position,
      string name)
    {
      MyStorageBase storage = MyWorldGenerator.LoadRandomizedVoxelMapPrefab(MyWorldGenerator.GetVoxelPrefabPath(prefabName));
      return MyWorldGenerator.AddVoxelMap(name, storage, position);
    }

    public static MyVoxelMap AddAsteroidPrefabCentered(
      string prefabName,
      Vector3D position,
      MatrixD rotation,
      string name)
    {
      MyStorageBase storage = MyWorldGenerator.LoadRandomizedVoxelMapPrefab(MyWorldGenerator.GetVoxelPrefabPath(prefabName));
      Vector3 vector3 = storage.Size * 0.5f;
      rotation.Translation = position - vector3;
      return MyWorldGenerator.AddVoxelMap(name, storage, rotation);
    }

    public static MyVoxelMap AddAsteroidPrefabCentered(
      string prefabName,
      Vector3D position,
      string name)
    {
      MyStorageBase storage = MyWorldGenerator.LoadRandomizedVoxelMapPrefab(MyWorldGenerator.GetVoxelPrefabPath(prefabName));
      Vector3 vector3 = storage.Size * 0.5f;
      return MyWorldGenerator.AddVoxelMap(name, storage, position - vector3);
    }

    public static MyVoxelMap AddVoxelMap(
      string storageName,
      MyStorageBase storage,
      Vector3D positionMinCorner,
      long entityId = 0)
    {
      MyVoxelMap myVoxelMap = new MyVoxelMap();
      if (entityId != 0L)
        myVoxelMap.EntityId = entityId;
      myVoxelMap.Init(storageName, (VRage.Game.Voxels.IMyStorage) storage, positionMinCorner);
      MyEntities.RaiseEntityCreated((MyEntity) myVoxelMap);
      MyEntities.Add((MyEntity) myVoxelMap);
      return myVoxelMap;
    }

    public static MyVoxelMap AddVoxelMap(
      string storageName,
      MyStorageBase storage,
      MatrixD worldMatrix,
      long entityId = 0,
      bool lazyPhysics = false,
      bool useVoxelOffset = true)
    {
      if (entityId != 0L && MyEntityIdentifier.ExistsById(entityId))
      {
        if (MyEntityIdentifier.GetEntityById(entityId) is MyVoxelMap entityById && entityById.StorageName == storageName)
        {
          MyLog.Default.WriteLine(string.Format("CRITICAL-VOXEL MAP!!! ---- VoxelMap already loaded. This must not happen ({0})", (object) storageName), LoggingOptions.VOXEL_MAPS);
        }
        else
        {
          IMyEntity entityById = MyEntityIdentifier.GetEntityById(entityId);
          if (entityById != null)
            MyLog.Default.WriteLine(string.Format("CRITICAL-VOXEL MAP!!! ---- VoxelMap entity collision. Entity with id {0} is already registered in place of VoxelMap{1}. ( entity ({2}) ({3}) ({4}) )", (object) entityId, (object) storageName, (object) entityById.DisplayName, (object) entityById.GetType(), (object) entityById.ToString()), LoggingOptions.VOXEL_MAPS);
          else
            MyLog.Default.WriteLine(string.Format("CRITICAL-VOXEL MAP!!! ---- VoxelMap entity collision. Entity (null) with id {0} is already registered in place of VoxelMap{1}.", (object) entityId, (object) storageName), LoggingOptions.VOXEL_MAPS);
        }
        return (MyVoxelMap) null;
      }
      MyVoxelMap myVoxelMap = new MyVoxelMap();
      if (entityId != 0L)
        myVoxelMap.EntityId = entityId;
      myVoxelMap.DelayRigidBodyCreation = lazyPhysics;
      myVoxelMap.Init(storageName, (VRage.Game.Voxels.IMyStorage) storage, worldMatrix, useVoxelOffset);
      MyEntities.Add((MyEntity) myVoxelMap);
      MyEntities.RaiseEntityCreated((MyEntity) myVoxelMap);
      return myVoxelMap;
    }

    public static void AddEntity(MyObjectBuilder_EntityBase entityBuilder) => MyEntities.CreateFromObjectBuilderAndAdd(entityBuilder, false);

    private static void AddObjectsPrefab(string prefabName)
    {
      foreach (MyObjectBuilder_EntityBase objectBuilder in MyWorldGenerator.LoadObjectsPrefab(prefabName))
        MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, false);
    }

    private static void SetupBase(
      string basePrefabName,
      Vector3 offset,
      string voxelFilename,
      string beaconName = null,
      long ownerId = 0)
    {
      MyPrefabManager.Static.SpawnPrefab(basePrefabName, (Vector3D) (new Vector3(-3f, 11f, 15f) + offset), Vector3.Forward, Vector3.Up, beaconName: beaconName);
      MyPrefabManager.Static.AddShipPrefab("SmallShip_SingleBlock", new Matrix?(Matrix.CreateTranslation(new Vector3(-5.208184f, -0.4429844f, -8.315228f) + offset)), ownerId);
      if (voxelFilename == null)
        return;
      MyStorageBase storage = MyWorldGenerator.LoadRandomizedVoxelMapPrefab(MyWorldGenerator.GetVoxelPrefabPath("VerticalIsland_128x128x128"));
      MyWorldGenerator.AddVoxelMap(voxelFilename, storage, (Vector3D) (new Vector3(-20f, -110f, -60f) + offset));
    }

    public static MyStorageBase LoadRandomizedVoxelMapPrefab(string prefabFilePath)
    {
      MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(prefabFilePath);
      myStorageBase.DataProvider = (IMyStorageDataProvider) MyCompositeShapeProvider.CreateAsteroidShape(MyUtils.GetRandomInt(2147483646) + 1, (float) myStorageBase.Size.AbsMax() * 1f);
      myStorageBase.Reset(MyStorageDataTypeFlags.Material);
      return myStorageBase;
    }

    private static string GetObjectsPrefabPath(string prefabName) => Path.Combine("Data", "Prefabs", prefabName + ".sbs");

    public static string GetVoxelPrefabPath(string prefabName)
    {
      MyVoxelMapStorageDefinition definition;
      if (!MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(prefabName, out definition))
        return Path.Combine(MyFileSystem.ContentPath, "VoxelMaps", prefabName + ".vx2");
      return definition.Context.IsBaseGame ? Path.Combine(MyFileSystem.ContentPath, definition.StorageFile) : definition.StorageFile;
    }

    private static List<MyObjectBuilder_EntityBase> LoadObjectsPrefab(
      string file)
    {
      MyObjectBuilder_Sector objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(Path.Combine(MyFileSystem.ContentPath, MyWorldGenerator.GetObjectsPrefabPath(file)), out objectBuilder);
      foreach (MyObjectBuilder_EntityBase sectorObject in objectBuilder.SectorObjects)
        sectorObject.EntityId = 0L;
      return objectBuilder.SectorObjects;
    }

    public static void SetProceduralSettings(
      int? asteroidAmount,
      MyObjectBuilder_SessionSettings sessionSettings)
    {
      sessionSettings.ProceduralSeed = MyRandom.Instance.Next();
      switch (asteroidAmount.Value)
      {
        case -4:
          sessionSettings.ProceduralDensity = 0.0f;
          break;
        case -3:
          sessionSettings.ProceduralDensity = 0.5f;
          break;
        case -2:
          sessionSettings.ProceduralDensity = 0.35f;
          break;
        case -1:
          sessionSettings.ProceduralDensity = 0.25f;
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    public static void AddPlanetPrefab(
      string planetName,
      string definitionName,
      Vector3D position,
      bool addGPS,
      bool fadeIn)
    {
      foreach (MyPlanetPrefabDefinition prefabsDefinition in MyDefinitionManager.Static.GetPlanetsPrefabsDefinitions())
      {
        if (prefabsDefinition.Id.SubtypeName == planetName)
        {
          MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(definitionName));
          MyPlanet myPlanet = new MyPlanet();
          MyObjectBuilder_Planet planetBuilder = prefabsDefinition.PlanetBuilder;
          string absoluteFilePath = MyFileSystem.ContentPath + "\\VoxelMaps\\" + planetBuilder.StorageName + ".vx2";
          myPlanet.EntityId = planetBuilder.EntityId;
          MyPlanetInitArguments arguments;
          arguments.StorageName = planetBuilder.StorageName;
          arguments.Seed = planetBuilder.Seed;
          arguments.Storage = (VRage.Game.Voxels.IMyStorage) MyStorageBase.LoadFromFile(absoluteFilePath);
          arguments.PositionMinCorner = position;
          arguments.Radius = planetBuilder.Radius;
          arguments.AtmosphereRadius = planetBuilder.AtmosphereRadius;
          arguments.MaxRadius = planetBuilder.MaximumHillRadius;
          arguments.MinRadius = planetBuilder.MinimumSurfaceRadius;
          arguments.HasAtmosphere = definition.HasAtmosphere;
          arguments.AtmosphereWavelengths = planetBuilder.AtmosphereWavelengths;
          arguments.GravityFalloff = definition.GravityFalloffPower;
          arguments.MarkAreaEmpty = true;
          arguments.AtmosphereSettings = planetBuilder.AtmosphereSettings ?? MyAtmosphereSettings.Defaults();
          arguments.SurfaceGravity = definition.SurfaceGravity;
          arguments.AddGps = addGPS;
          arguments.SpherizeWithDistance = true;
          arguments.Generator = definition;
          arguments.UserCreated = false;
          arguments.InitializeComponents = true;
          arguments.FadeIn = fadeIn;
          myPlanet.Init(arguments);
          MyEntities.Add((MyEntity) myPlanet);
          MyEntities.RaiseEntityCreated((MyEntity) myPlanet);
        }
      }
    }

    public static MyPlanet AddPlanet(
      string storageName,
      string planetName,
      string definitionName,
      Vector3D positionMinCorner,
      int seed,
      float size,
      bool fadeIn,
      long entityId = 0,
      bool addGPS = false,
      bool userCreated = false)
    {
      MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(definitionName));
      return MyWorldGenerator.CreatePlanet(storageName, planetName, ref positionMinCorner, seed, size, entityId, ref definition, addGPS, userCreated, fadeIn);
    }

    private static MyPlanet CreatePlanet(
      string storageName,
      string planetName,
      ref Vector3D positionMinCorner,
      int seed,
      float size,
      long entityId,
      ref MyPlanetGeneratorDefinition generatorDef,
      bool addGPS,
      bool userCreated = false,
      bool fadeIn = false)
    {
      if (!MyFakes.ENABLE_PLANETS)
        return (MyPlanet) null;
      MyRandom instance = MyRandom.Instance;
      using (MyRandom.Instance.PushSeed(seed))
      {
        MyPlanetStorageProvider planetStorageProvider = new MyPlanetStorageProvider();
        planetStorageProvider.Init((long) seed, generatorDef, (double) size / 2.0, true);
        VRage.Game.Voxels.IMyStorage myStorage = (VRage.Game.Voxels.IMyStorage) new MyOctreeStorage((IMyStorageDataProvider) planetStorageProvider, planetStorageProvider.StorageSize);
        float num1 = planetStorageProvider.Radius * generatorDef.HillParams.Min;
        float num2 = planetStorageProvider.Radius * generatorDef.HillParams.Max;
        double radius = (double) planetStorageProvider.Radius;
        float num3 = (float) radius + num2;
        float num4 = (float) radius + num1;
        float num5 = (!generatorDef.AtmosphereSettings.HasValue || (double) generatorDef.AtmosphereSettings.Value.Scale <= 1.0 ? 1.75f : 1f + generatorDef.AtmosphereSettings.Value.Scale) * planetStorageProvider.Radius;
        Vector3 vector3 = new Vector3(0.65f + instance.NextFloat(generatorDef.HostileAtmosphereColorShift.R.Min, generatorDef.HostileAtmosphereColorShift.R.Max), 0.57f + instance.NextFloat(generatorDef.HostileAtmosphereColorShift.G.Min, generatorDef.HostileAtmosphereColorShift.G.Max), 0.475f + instance.NextFloat(generatorDef.HostileAtmosphereColorShift.B.Min, generatorDef.HostileAtmosphereColorShift.B.Max));
        vector3.X = MathHelper.Clamp(vector3.X, 0.1f, 1f);
        vector3.Y = MathHelper.Clamp(vector3.Y, 0.1f, 1f);
        vector3.Z = MathHelper.Clamp(vector3.Z, 0.1f, 1f);
        MyPlanet myPlanet = (MyPlanet) null;
        try
        {
          myPlanet = new MyPlanet();
          myPlanet.EntityId = entityId;
          MyPlanetInitArguments arguments;
          arguments.StorageName = storageName;
          arguments.Seed = seed;
          arguments.Storage = myStorage;
          arguments.PositionMinCorner = positionMinCorner;
          arguments.Radius = planetStorageProvider.Radius;
          arguments.AtmosphereRadius = num5;
          arguments.MaxRadius = num3;
          arguments.MinRadius = num4;
          arguments.HasAtmosphere = generatorDef.HasAtmosphere;
          arguments.AtmosphereWavelengths = vector3;
          arguments.GravityFalloff = generatorDef.GravityFalloffPower;
          arguments.MarkAreaEmpty = true;
          arguments.AtmosphereSettings = generatorDef.AtmosphereSettings ?? MyAtmosphereSettings.Defaults();
          arguments.SurfaceGravity = generatorDef.SurfaceGravity;
          arguments.AddGps = addGPS;
          arguments.SpherizeWithDistance = true;
          arguments.Generator = generatorDef;
          arguments.UserCreated = userCreated;
          arguments.InitializeComponents = true;
          arguments.FadeIn = fadeIn;
          myPlanet.Init(arguments);
          myPlanet.AsteroidName = planetName;
        }
        catch (MyPlanetWhitelistException ex)
        {
          if (myPlanet != null)
            myPlanet.EntityId = 0L;
          return (MyPlanet) null;
        }
        MyEntities.RaiseEntityCreated((MyEntity) myPlanet);
        MyEntities.Add((MyEntity) myPlanet);
        return myPlanet;
      }
    }

    public struct Args
    {
      public MyScenarioDefinition Scenario;
      public int AsteroidAmount;
    }

    public class OperationTypeAttribute : MyFactoryTagAttribute
    {
      public OperationTypeAttribute(Type objectBuilderType)
        : base(objectBuilderType)
      {
      }
    }

    public static class OperationFactory
    {
      private static MyObjectFactory<MyWorldGenerator.OperationTypeAttribute, MyWorldGeneratorOperationBase> m_objectFactory = new MyObjectFactory<MyWorldGenerator.OperationTypeAttribute, MyWorldGeneratorOperationBase>();

      static OperationFactory()
      {
        MyWorldGenerator.OperationFactory.m_objectFactory.RegisterFromCreatedObjectAssembly();
        MyWorldGenerator.OperationFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
        MyWorldGenerator.OperationFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
        MyWorldGenerator.OperationFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
      }

      public static MyWorldGeneratorOperationBase CreateInstance(
        MyObjectBuilder_WorldGeneratorOperation builder)
      {
        MyWorldGeneratorOperationBase instance = MyWorldGenerator.OperationFactory.m_objectFactory.CreateInstance(builder.TypeId);
        instance.Init(builder);
        return instance;
      }

      public static MyObjectBuilder_WorldGeneratorOperation CreateObjectBuilder(
        MyWorldGeneratorOperationBase instance)
      {
        return MyWorldGenerator.OperationFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_WorldGeneratorOperation>(instance);
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab))]
    public class OperationAddShipPrefab : MyWorldGeneratorOperationBase
    {
      public string PrefabFile;
      public bool UseFirstGridOrigin;
      public MyPositionAndOrientation Transform = MyPositionAndOrientation.Default;
      public float RandomRadius;

      public override void Apply()
      {
        MyFaction myFaction = (MyFaction) null;
        if (this.FactionTag != null)
          myFaction = MySession.Static.Factions.TryGetOrCreateFactionByTag(this.FactionTag);
        long ownerId1 = myFaction != null ? myFaction.FounderId : 0L;
        if ((double) this.RandomRadius == 0.0)
        {
          MyPrefabManager myPrefabManager = MyPrefabManager.Static;
          string prefabFile = this.PrefabFile;
          MatrixD matrix = this.Transform.GetMatrix();
          Matrix? worldMatrix = new Matrix?((Matrix) ref matrix);
          long ownerId2 = ownerId1;
          int num = this.UseFirstGridOrigin ? 1 : 0;
          myPrefabManager.AddShipPrefab(prefabFile, worldMatrix, ownerId2, num != 0);
        }
        else
          MyPrefabManager.Static.AddShipPrefabRandomPosition(this.PrefabFile, (Vector3D) this.Transform.Position, this.RandomRadius, ownerId1);
      }

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab operationAddShipPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab;
        this.PrefabFile = operationAddShipPrefab.PrefabFile;
        this.UseFirstGridOrigin = operationAddShipPrefab.UseFirstGridOrigin;
        this.Transform = operationAddShipPrefab.Transform;
        this.RandomRadius = operationAddShipPrefab.RandomRadius;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddShipPrefab;
        objectBuilder.PrefabFile = this.PrefabFile;
        objectBuilder.Transform = this.Transform;
        objectBuilder.RandomRadius = this.RandomRadius;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab))]
    public class OperationAddAsteroidPrefab : MyWorldGeneratorOperationBase
    {
      public string Name;
      public string PrefabName;
      public Vector3 Position;

      public override void Apply() => MyWorldGenerator.AddAsteroidPrefab(this.PrefabName, (Vector3D) this.Position, this.Name);

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab addAsteroidPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab;
        this.Name = addAsteroidPrefab.Name;
        this.PrefabName = addAsteroidPrefab.PrefabFile;
        this.Position = (Vector3) addAsteroidPrefab.Position;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab;
        objectBuilder.Name = this.Name;
        objectBuilder.PrefabFile = this.PrefabName;
        objectBuilder.Position = (SerializableVector3) this.Position;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab))]
    public class OperationAddObjectsPrefab : MyWorldGeneratorOperationBase
    {
      public string PrefabFile;

      public override void Apply() => MyWorldGenerator.AddObjectsPrefab(this.PrefabFile);

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        this.PrefabFile = (builder as MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab).PrefabFile;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab;
        objectBuilder.PrefabFile = this.PrefabFile;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab))]
    public class OperationSetupBasePrefab : MyWorldGeneratorOperationBase
    {
      public string PrefabFile;
      public Vector3 Offset;
      public string AsteroidName;
      public string BeaconName;

      public override void Apply()
      {
        MyFaction myFaction = (MyFaction) null;
        if (this.FactionTag != null)
          myFaction = MySession.Static.Factions.TryGetOrCreateFactionByTag(this.FactionTag);
        MyWorldGenerator.SetupBase(this.PrefabFile, this.Offset, this.AsteroidName, this.BeaconName, myFaction != null ? myFaction.FounderId : 0L);
      }

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab operationSetupBasePrefab = builder as MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab;
        this.PrefabFile = operationSetupBasePrefab.PrefabFile;
        this.Offset = (Vector3) operationSetupBasePrefab.Offset;
        this.AsteroidName = operationSetupBasePrefab.AsteroidName;
        this.BeaconName = operationSetupBasePrefab.BeaconName;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab;
        objectBuilder.PrefabFile = this.PrefabFile;
        objectBuilder.Offset = (SerializableVector3) this.Offset;
        objectBuilder.AsteroidName = this.AsteroidName;
        objectBuilder.BeaconName = this.BeaconName;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab))]
    public class OperationAddPlanetPrefab : MyWorldGeneratorOperationBase
    {
      public string PrefabName;
      public string DefinitionName;
      public Vector3D Position;
      public bool AddGPS;

      public override void Apply() => MyWorldGenerator.AddPlanetPrefab(this.PrefabName, this.DefinitionName, this.Position, this.AddGPS, true);

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab operationAddPlanetPrefab = builder as MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab;
        this.DefinitionName = operationAddPlanetPrefab.DefinitionName;
        this.PrefabName = operationAddPlanetPrefab.PrefabName;
        this.Position = (Vector3D) operationAddPlanetPrefab.Position;
        this.AddGPS = operationAddPlanetPrefab.AddGPS;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab;
        objectBuilder.DefinitionName = this.DefinitionName;
        objectBuilder.PrefabName = this.PrefabName;
        objectBuilder.Position = (SerializableVector3D) this.Position;
        objectBuilder.AddGPS = this.AddGPS;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    [MyWorldGenerator.OperationType(typeof (MyObjectBuilder_WorldGeneratorOperation_CreatePlanet))]
    public class OperationCreatePlanet : MyWorldGeneratorOperationBase
    {
      public string DefinitionName;
      public bool AddGPS;
      public Vector3D PositionMinCorner;
      public Vector3D PositionCenter;
      public float Diameter;

      public override void Apply()
      {
        MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(this.DefinitionName));
        if (definition == null)
        {
          MyLog.Default.WriteLine(string.Format("Definition for planet {0} could not be found. Skipping.", (object) this.DefinitionName));
        }
        else
        {
          Vector3D positionMinCorner = this.PositionMinCorner;
          if (this.PositionCenter.IsValid())
            positionMinCorner = this.PositionCenter - (Vector3D) MyVoxelCoordSystems.FindBestOctreeSize(this.Diameter * (1f + definition.HillParams.Max)) / 2.0;
          int seed = MyRandom.Instance.Next();
          MyWorldGenerator.CreatePlanet(this.DefinitionName + "-" + (object) seed + "d" + (object) this.Diameter, definition.FolderName, ref positionMinCorner, seed, this.Diameter, MyRandom.Instance.NextLong(), ref definition, this.AddGPS);
        }
      }

      public override void Init(MyObjectBuilder_WorldGeneratorOperation builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorOperation_CreatePlanet operationCreatePlanet = builder as MyObjectBuilder_WorldGeneratorOperation_CreatePlanet;
        this.DefinitionName = operationCreatePlanet.DefinitionName;
        this.DefinitionName = operationCreatePlanet.DefinitionName;
        this.AddGPS = operationCreatePlanet.AddGPS;
        this.Diameter = operationCreatePlanet.Diameter;
        this.PositionMinCorner = (Vector3D) operationCreatePlanet.PositionMinCorner;
        this.PositionCenter = (Vector3D) operationCreatePlanet.PositionCenter;
      }

      public override MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorOperation_CreatePlanet objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorOperation_CreatePlanet;
        objectBuilder.DefinitionName = this.DefinitionName;
        objectBuilder.DefinitionName = this.DefinitionName;
        objectBuilder.AddGPS = this.AddGPS;
        objectBuilder.Diameter = this.Diameter;
        objectBuilder.PositionMinCorner = (SerializableVector3D) this.PositionMinCorner;
        objectBuilder.PositionCenter = (SerializableVector3D) this.PositionCenter;
        return (MyObjectBuilder_WorldGeneratorOperation) objectBuilder;
      }
    }

    public class StartingStateTypeAttribute : MyFactoryTagAttribute
    {
      public StartingStateTypeAttribute(Type objectBuilderType)
        : base(objectBuilderType)
      {
      }
    }

    public static class StartingStateFactory
    {
      private static MyObjectFactory<MyWorldGenerator.StartingStateTypeAttribute, MyWorldGeneratorStartingStateBase> m_objectFactory = new MyObjectFactory<MyWorldGenerator.StartingStateTypeAttribute, MyWorldGeneratorStartingStateBase>();

      static StartingStateFactory()
      {
        MyWorldGenerator.StartingStateFactory.m_objectFactory.RegisterFromCreatedObjectAssembly();
        MyWorldGenerator.StartingStateFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
        MyWorldGenerator.StartingStateFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
        MyWorldGenerator.StartingStateFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
      }

      public static MyWorldGeneratorStartingStateBase CreateInstance(
        MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
      {
        MyWorldGeneratorStartingStateBase instance = MyWorldGenerator.StartingStateFactory.m_objectFactory.CreateInstance(builder.TypeId);
        instance?.Init(builder);
        return instance;
      }

      public static MyObjectBuilder_WorldGeneratorPlayerStartingState CreateObjectBuilder(
        MyWorldGeneratorStartingStateBase instance)
      {
        return MyWorldGenerator.StartingStateFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_WorldGeneratorPlayerStartingState>(instance);
      }
    }

    [MyWorldGenerator.StartingStateType(typeof (MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform))]
    public class MyTransformState : MyWorldGeneratorStartingStateBase
    {
      public MyPositionAndOrientation? Transform;
      public bool JetpackEnabled;
      public bool DampenersEnabled;

      public override void SetupCharacter(MyWorldGenerator.Args generatorArgs)
      {
        if (MySession.Static.LocalHumanPlayer == null)
          return;
        MyObjectBuilder_Character builderCharacter = MyCharacter.Random();
        if (this.Transform.HasValue && MyPerGameSettings.CharacterStartsOnVoxel)
        {
          MyPositionAndOrientation positionAndOrientation = this.Transform.Value;
          positionAndOrientation.Position = (SerializableVector3D) this.FixPositionToVoxel((Vector3D) positionAndOrientation.Position);
          builderCharacter.PositionAndOrientation = new MyPositionAndOrientation?(positionAndOrientation);
        }
        else
          builderCharacter.PositionAndOrientation = this.Transform;
        builderCharacter.JetpackEnabled = this.JetpackEnabled;
        builderCharacter.DampenersEnabled = this.DampenersEnabled;
        if (builderCharacter.Inventory == null)
          builderCharacter.Inventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
        MyWorldGenerator.FillInventoryWithDefaults(builderCharacter.Inventory, generatorArgs.Scenario);
        MyCharacter character = new MyCharacter();
        character.Name = "Player";
        character.Init((MyObjectBuilder_EntityBase) builderCharacter);
        MyEntities.RaiseEntityCreated((MyEntity) character);
        MyEntities.Add((MyEntity) character);
        this.CreateAndSetPlayerFaction();
        MySession.Static.LocalHumanPlayer.SpawnIntoCharacter(character);
      }

      public override void Init(
        MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
      {
        base.Init(builder);
        MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform startingStateTransform = builder as MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform;
        this.Transform = startingStateTransform.Transform;
        this.JetpackEnabled = startingStateTransform.JetpackEnabled;
        this.DampenersEnabled = startingStateTransform.DampenersEnabled;
      }

      public override MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
      {
        MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform;
        objectBuilder.Transform = this.Transform;
        objectBuilder.JetpackEnabled = this.JetpackEnabled;
        objectBuilder.DampenersEnabled = this.DampenersEnabled;
        return (MyObjectBuilder_WorldGeneratorPlayerStartingState) objectBuilder;
      }

      public override Vector3D? GetStartingLocation() => this.Transform.HasValue && MyPerGameSettings.CharacterStartsOnVoxel ? new Vector3D?(this.FixPositionToVoxel((Vector3D) this.Transform.Value.Position)) : new Vector3D?();

      private class Sandbox_Game_World_MyWorldGenerator\u003C\u003EMyTransformState\u003C\u003EActor : IActivator, IActivator<MyWorldGenerator.MyTransformState>
      {
        object IActivator.CreateInstance() => (object) new MyWorldGenerator.MyTransformState();

        MyWorldGenerator.MyTransformState IActivator<MyWorldGenerator.MyTransformState>.CreateInstance() => new MyWorldGenerator.MyTransformState();
      }
    }
  }
}
