// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyEncounterGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof (MyObjectBuilder_Encounters), null, false)]
  internal class MyEncounterGenerator : MySessionComponentBase
  {
    private const double MIN_DISTANCE_TO_RECOGNIZE_MOVEMENT = 1000.0;
    private HashSet<MyEncounterId> m_persistentEncounters = new HashSet<MyEncounterId>();
    private HashSet<MyEncounterId> m_encounterSpawnInProgress = new HashSet<MyEncounterId>();
    private HashSet<MyEncounterId> m_encounterRemoveRequested = new HashSet<MyEncounterId>();
    private Dictionary<MyEntity, MyEncounterId> m_entityToEncounterId = new Dictionary<MyEntity, MyEncounterId>();
    private Dictionary<MyEncounterId, List<MyEntity>> m_encounterEntities = new Dictionary<MyEncounterId, List<MyEntity>>();
    private MyRandom m_random = new MyRandom();
    private MySpawnGroupDefinition[] m_encounterSpawnGroups;
    private MyConcurrentPool<List<MyEntity>> m_entityListsPool = new MyConcurrentPool<List<MyEntity>>(10, (Action<List<MyEntity>>) (x => x.Clear()), 1000, (Func<List<MyEntity>>) (() => new List<MyEntity>(10)));
    private static List<float> m_spawnGroupCumulativeFrequencies;
    private readonly MyVoxelBase.StorageChanged m_OnVoxelChanged;
    private readonly Action<MySlimBlock> m_OnBlockChanged;
    private readonly Action<MyCubeGrid> m_OnGridChanged;
    private readonly Action<MyEntity> m_OnEntityClosed;
    private readonly Action<MyPositionComponentBase> m_OnEntityPositionChanged;

    public static MyEncounterGenerator Static { get; private set; }

    public void RemoveEncounter(BoundingBoxD boundingVolume, int seed)
    {
      if (!MySession.Static.Settings.EnableEncounters || !MyEncounterGenerator.IsAcceptableEncounter(boundingVolume))
        return;
      MyEncounterId myEncounterId = new MyEncounterId(boundingVolume, seed, 0);
      if (this.m_encounterSpawnInProgress.Contains(myEncounterId))
        this.m_encounterRemoveRequested.Add(myEncounterId);
      else if (!this.m_encounterEntities.ContainsKey(myEncounterId))
        this.m_persistentEncounters.Contains(myEncounterId);
      else
        this.RemoveEncounter(myEncounterId, false, true);
    }

    private void PersistEncounter(MyEntity encounterEntity)
    {
      MyEncounterId id;
      if (!this.m_entityToEncounterId.TryGetValue(encounterEntity, out id))
        return;
      this.PersistEncounter(id);
    }

    private void PersistEncounter(MyEncounterId id)
    {
      if (!MySession.Static.Ready)
        return;
      this.RemoveEncounter(id, true, false);
      MyMultiplayer.RaiseStaticEvent<MyEncounterId>((Func<IMyEventOwner, Action<MyEncounterId>>) (x => new Action<MyEncounterId>(MyEncounterGenerator.PersistEncounterClient)), id);
    }

    [Event(null, 112)]
    [Reliable]
    [Broadcast]
    private static void PersistEncounterClient(MyEncounterId encounterId)
    {
      if (MyEncounterGenerator.Static.m_encounterEntities.ContainsKey(encounterId))
        MyEncounterGenerator.Static.RemoveEncounter(encounterId, true, false);
      else
        MyEncounterGenerator.Static.m_persistentEncounters.Add(encounterId);
    }

    private void RemoveEncounter(MyEncounterId encounter, bool markPersistent, bool close)
    {
      if (this.m_persistentEncounters.Contains(encounter))
        return;
      if (markPersistent)
        this.m_persistentEncounters.Add(encounter);
      List<MyEntity> instance;
      if (!this.m_encounterEntities.TryGetValue(encounter, out instance))
        return;
      foreach (MyEntity key in instance)
      {
        MyCubeGrid myCubeGrid = key as MyCubeGrid;
        MyVoxelBase myVoxelBase = key as MyVoxelBase;
        key.OnMarkForClose -= this.m_OnEntityClosed;
        key.PositionComp.OnPositionChanged -= this.m_OnEntityPositionChanged;
        if (myCubeGrid != null)
        {
          myCubeGrid.OnBlockAdded -= this.m_OnBlockChanged;
          myCubeGrid.OnGridChanged -= this.m_OnGridChanged;
          myCubeGrid.OnBlockRemoved -= this.m_OnBlockChanged;
          myCubeGrid.OnBlockIntegrityChanged -= this.m_OnBlockChanged;
        }
        if (myVoxelBase != null)
          myVoxelBase.RangeChanged -= this.m_OnVoxelChanged;
        if (close)
        {
          if (Sync.IsServer || key is MyVoxelBase)
            key.Close();
        }
        else if (markPersistent && !key.MarkedForClose)
          key.Save = true;
        this.m_entityToEncounterId.Remove(key);
      }
      this.m_entityListsPool.Return(instance);
      this.m_encounterEntities.Remove(encounter);
      this.m_encounterRemoveRequested.Remove(encounter);
    }

    public void PlaceEncounterToWorld(BoundingBoxD boundingVolume, int seed)
    {
      if (!MySession.Static.Settings.EnableEncounters || this.m_encounterSpawnGroups.Length == 0 || !MyEncounterGenerator.IsAcceptableEncounter(boundingVolume))
        return;
      MyEncounterId myEncounterId = new MyEncounterId(boundingVolume, seed, 0);
      if (this.m_encounterEntities.ContainsKey(myEncounterId))
      {
        this.m_encounterRemoveRequested.Remove(myEncounterId);
      }
      else
      {
        if (this.m_persistentEncounters.Contains(myEncounterId))
          return;
        this.OpenEncounter(myEncounterId);
        this.m_random.SetSeed(seed);
        MySpawnGroupDefinition spawnGroup = MyEncounterGenerator.PickRandomEncounter(this.m_random, this.m_encounterSpawnGroups);
        Vector3D center = boundingVolume.Center;
        for (int index = 0; index < spawnGroup.Voxels.Count; ++index)
        {
          MySpawnGroupDefinition.SpawnGroupVoxel voxel = spawnGroup.Voxels[index];
          MyStorageBase storage = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(voxel.StorageName));
          if (storage != null)
          {
            Vector3D vector3D = center + voxel.Offset;
            string storageName = string.Format("Asteroid_{0}_{1}_{2}", (object) myEncounterId.GetHashCode(), (object) boundingVolume.Round().GetHashCode(), (object) index);
            long asteroidEntityId = MyProceduralAsteroidCellGenerator.GetAsteroidEntityId(storageName);
            MyEntity entity = !voxel.CenterOffset ? (MyEntity) MyWorldGenerator.AddVoxelMap(storageName, storage, vector3D, asteroidEntityId) : (MyEntity) MyWorldGenerator.AddVoxelMap(storageName, storage, MatrixD.CreateWorld(vector3D), asteroidEntityId, useVoxelOffset: false);
            this.RegisterEntityToEncounter(myEncounterId, entity);
          }
        }
        if (!Sync.IsServer)
          return;
        bool flag = true;
        if (spawnGroup.IsPirate && MySession.Static.Players.TryGetIdentity(MyPirateAntennas.GetPiratesId()) == null)
        {
          MyLog.Default.Error("Missing pirate identity. Encounter will not spawn.");
          flag = false;
        }
        if (!MySession.Static.NPCBlockLimits.HasRemainingPCU)
        {
          MyLog.Default.Log(MyLogSeverity.Info, "Exhausted NPC PCUs. Encounter will not spawn.");
          flag = false;
        }
        if (flag)
        {
          this.SpawnEncounterGrids(myEncounterId, center, spawnGroup);
        }
        else
        {
          if (spawnGroup.Prefabs.Count <= 0)
            return;
          this.PersistEncounter(myEncounterId);
        }
      }
    }

    private void SpawnEncounterGrids(
      MyEncounterId encounterId,
      Vector3D placePosition,
      MySpawnGroupDefinition spawnGroup)
    {
      this.m_encounterSpawnInProgress.Add(encounterId);
      long num1 = 0;
      if (spawnGroup.IsPirate)
        num1 = MyPirateAntennas.GetPiratesId();
      int remainingPrefabsToSpawn = spawnGroup.Prefabs.Count + 1;
      Action action = (Action) (() =>
      {
        --remainingPrefabsToSpawn;
        if (remainingPrefabsToSpawn != 0)
          return;
        this.m_encounterSpawnInProgress.Remove(encounterId);
        if (!this.m_encounterRemoveRequested.Contains(encounterId))
          return;
        this.RemoveEncounter(encounterId, false, true);
      });
      foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroup.Prefabs)
      {
        MySpawnGroupDefinition.SpawnGroupPrefab selectedPrefab = prefab;
        List<MyCubeGrid> createdGrids = new List<MyCubeGrid>();
        Vector3D v = Vector3D.Forward;
        Vector3D vector3D1 = Vector3D.Up;
        SpawningOptions spawningOptions = SpawningOptions.DisableSave | SpawningOptions.UseGridOrigin | SpawningOptions.SetAuthorship;
        if ((double) selectedPrefab.Speed > 0.0)
        {
          spawningOptions |= SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SpawnRandomCargo | SpawningOptions.DisableDampeners;
          float minValue = (float) Math.Atan(2000.0 / placePosition.Length());
          Vector3D vector3D2 = -Vector3D.Normalize(placePosition);
          float num2 = this.m_random.NextFloat(minValue, minValue + 0.5f);
          float num3 = this.m_random.NextFloat(0.0f, 6.283186f);
          Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(vector3D2);
          Vector3D vector3D3 = Vector3D.Cross(vector3D2, perpendicularVector);
          Vector3D vector3D4 = perpendicularVector * (Math.Sin((double) num2) * Math.Cos((double) num3));
          Vector3D vector3D5 = vector3D3 * (Math.Sin((double) num2) * Math.Sin((double) num3));
          v = vector3D2 * Math.Cos((double) num2) + vector3D4 + vector3D5;
          vector3D1 = Vector3D.CalculatePerpendicularVector(v);
        }
        if (selectedPrefab.PlaceToGridOrigin)
          spawningOptions |= SpawningOptions.UseGridOrigin;
        if (selectedPrefab.ResetOwnership && !spawnGroup.IsPirate)
          spawningOptions |= SpawningOptions.SetNeutralOwner;
        if (!spawnGroup.ReactorsOn)
          spawningOptions |= SpawningOptions.TurnOffReactors;
        Stack<Action> actionStack = new Stack<Action>();
        actionStack.Push(action);
        actionStack.Push((Action) (() =>
        {
          foreach (MyEntity entity in createdGrids)
            this.RegisterEntityToEncounter(encounterId, entity);
          string behaviour = selectedPrefab.Behaviour;
          if (string.IsNullOrWhiteSpace(behaviour))
            return;
          foreach (MyCubeGrid grid in createdGrids)
          {
            if (!MyDroneAI.SetAIToGrid(grid, behaviour, selectedPrefab.BehaviourActivationDistance))
              MyLog.Default.Error("Could not inject AI to encounter {0}. No remote control.", (object) grid.DisplayName);
          }
        }));
        MyPrefabManager myPrefabManager = MyPrefabManager.Static;
        List<MyCubeGrid> resultList = createdGrids;
        string subtypeId = selectedPrefab.SubtypeId;
        Vector3D position = placePosition + selectedPrefab.Position;
        Vector3 forward = (Vector3) v;
        Vector3 up = (Vector3) vector3D1;
        string beaconText = selectedPrefab.BeaconText;
        Vector3 initialLinearVelocity = (Vector3) (v * (double) selectedPrefab.Speed);
        Vector3 initialAngularVelocity = new Vector3();
        string beaconName = beaconText;
        int num4 = (int) spawningOptions;
        long ownerId = num1;
        Stack<Action> callbacks = actionStack;
        myPrefabManager.SpawnPrefab(resultList, subtypeId, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, spawningOptions: ((SpawningOptions) num4), ownerId: ownerId, updateSync: true, callbacks: callbacks);
      }
      action();
    }

    private static MySpawnGroupDefinition PickRandomEncounter(
      MyRandom random,
      MySpawnGroupDefinition[] candidates)
    {
      using (MyUtils.ReuseCollection<float>(ref MyEncounterGenerator.m_spawnGroupCumulativeFrequencies))
      {
        float maxValue = 0.0f;
        foreach (MySpawnGroupDefinition candidate in candidates)
        {
          maxValue += candidate.Frequency;
          MyEncounterGenerator.m_spawnGroupCumulativeFrequencies.Add(maxValue);
        }
        int index = 0;
        float num = random.NextFloat(0.0f, maxValue);
        while (index < MyEncounterGenerator.m_spawnGroupCumulativeFrequencies.Count && (double) num > (double) MyEncounterGenerator.m_spawnGroupCumulativeFrequencies[index])
          ++index;
        if (index >= MyEncounterGenerator.m_spawnGroupCumulativeFrequencies.Count)
          index = MyEncounterGenerator.m_spawnGroupCumulativeFrequencies.Count - 1;
        return candidates[index];
      }
    }

    private void OpenEncounter(MyEncounterId id)
    {
      if (this.m_encounterEntities.TryGetValue(id, out List<MyEntity> _))
        return;
      List<MyEntity> myEntityList = this.m_entityListsPool.Get();
      this.m_encounterEntities.Add(id, myEntityList);
    }

    private void RegisterEntityToEncounter(MyEncounterId id, MyEntity entity)
    {
      entity.Save = false;
      List<MyEntity> myEntityList;
      if (!this.m_encounterEntities.TryGetValue(id, out myEntityList))
        return;
      MyCubeGrid myCubeGrid = entity as MyCubeGrid;
      MyVoxelBase myVoxelBase = entity as MyVoxelBase;
      if (Sync.IsServer)
      {
        entity.OnMarkForClose += this.m_OnEntityClosed;
        entity.PositionComp.OnPositionChanged += this.m_OnEntityPositionChanged;
        if (myCubeGrid != null)
        {
          myCubeGrid.OnGridChanged += this.m_OnGridChanged;
          myCubeGrid.OnBlockAdded += this.m_OnBlockChanged;
          myCubeGrid.OnBlockRemoved += this.m_OnBlockChanged;
          myCubeGrid.OnBlockIntegrityChanged += this.m_OnBlockChanged;
        }
        if (myVoxelBase != null)
          myVoxelBase.RangeChanged += this.m_OnVoxelChanged;
      }
      myEntityList.Add(entity);
      this.m_entityToEncounterId.Add(entity, id);
    }

    private void OnVoxelChanged(
      MyVoxelBase voxel,
      Vector3I minvoxelchanged,
      Vector3I maxvoxelchanged,
      MyStorageDataTypeFlags changeddata)
    {
      this.PersistEncounter((MyEntity) voxel);
    }

    private void OnBlockChanged(MySlimBlock block)
    {
      MyEncounterGenerator.AssertNotClosed((MyEntity) block.CubeGrid);
      this.PersistEncounter((MyEntity) block.CubeGrid);
    }

    private void OnGridChanged(MyCubeGrid grid)
    {
      MyEncounterGenerator.AssertNotClosed((MyEntity) grid);
      MyEncounterGenerator.Static.PersistEncounter((MyEntity) grid);
    }

    private void OnEntityClosed(MyEntity entity) => this.PersistEncounter(entity);

    private void OnEntityPositionChanged(MyPositionComponentBase obj)
    {
      MyEntity entity = (MyEntity) obj.Container.Entity;
      MyEncounterGenerator.AssertNotClosed(entity);
      MyEncounterId myEncounterId;
      if (!this.m_entityToEncounterId.TryGetValue(entity, out myEncounterId))
        return;
      Vector3D position = obj.GetPosition();
      if (Vector3D.Distance(myEncounterId.BoundingBox.Center, position) <= 1000.0)
        return;
      this.PersistEncounter(entity);
    }

    private static void AssertNotClosed(MyEntity entity)
    {
      int num = entity.MarkedForClose ? 1 : 0;
    }

    private static bool IsAcceptableEncounter(BoundingBoxD boundingBox)
    {
      Vector3D center = boundingBox.Center;
      foreach (MyWorldGeneratorStartingStateBase possiblePlayerStart in MySession.Static.Scenario.PossiblePlayerStarts)
      {
        Vector3D vector3D = possiblePlayerStart.GetStartingLocation() ?? Vector3D.Zero;
        if (Vector3D.DistanceSquared(center, vector3D) < 225000000.0)
          return false;
      }
      return true;
    }

    public MyEncounterGenerator()
    {
      this.m_OnGridChanged = new Action<MyCubeGrid>(this.OnGridChanged);
      this.m_OnBlockChanged = new Action<MySlimBlock>(this.OnBlockChanged);
      this.m_OnEntityClosed = new Action<MyEntity>(this.OnEntityClosed);
      this.m_OnVoxelChanged = new MyVoxelBase.StorageChanged(this.OnVoxelChanged);
      this.m_OnEntityPositionChanged = new Action<MyPositionComponentBase>(this.OnEntityPositionChanged);
    }

    public override void LoadData()
    {
      MyEncounterGenerator.Static = this;
      this.m_encounterSpawnGroups = MyDefinitionManager.Static.GetSpawnGroupDefinitions().Where<MySpawnGroupDefinition>((Func<MySpawnGroupDefinition, bool>) (x => x.IsEncounter)).ToArray<MySpawnGroupDefinition>();
    }

    protected override void UnloadData()
    {
      while (this.m_encounterEntities.Count > 0)
        this.RemoveEncounter(this.m_encounterEntities.FirstPair<MyEncounterId, List<MyEntity>>().Key, false, true);
      base.UnloadData();
      MyEncounterGenerator.Static = (MyEncounterGenerator) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_Encounters objectBuilder = (MyObjectBuilder_Encounters) base.GetObjectBuilder();
      objectBuilder.SavedEncounters = new HashSet<MyEncounterId>((IEnumerable<MyEncounterId>) this.m_persistentEncounters);
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void Init(MyObjectBuilder_SessionComponent objectBuilder)
    {
      base.Init(objectBuilder);
      MyObjectBuilder_Encounters builderEncounters = (MyObjectBuilder_Encounters) objectBuilder;
      if (builderEncounters.SavedEncounters == null)
        return;
      this.m_persistentEncounters = builderEncounters.SavedEncounters;
    }

    public void DebugDraw()
    {
      Vector3D position = MySector.MainCamera.Position;
      foreach (MyEncounterId key in this.m_encounterEntities.Keys)
      {
        MyRenderProxy.DebugDrawAABB(key.BoundingBox, Color.Blue);
        Vector3D center = key.BoundingBox.Center;
        if (Vector3D.Distance(position, center) < 500.0)
          MyRenderProxy.DebugDrawText3D(center, key.ToString(), Color.Blue, 0.7f, false);
      }
      foreach (MyEncounterId persistentEncounter in this.m_persistentEncounters)
      {
        MyRenderProxy.DebugDrawAABB(persistentEncounter.BoundingBox, Color.Red);
        Vector3D center = persistentEncounter.BoundingBox.Center;
        if (Vector3D.Distance(position, center) < 500.0)
          MyRenderProxy.DebugDrawText3D(center, persistentEncounter.ToString(), Color.Red, 0.7f, false);
      }
    }

    public bool IsEncounter(MyEntity entity) => this.m_entityToEncounterId.ContainsKey(entity);

    public void GetStats(out int persistentEncounters, out int encounterEntities)
    {
      encounterEntities = this.m_entityToEncounterId.Count;
      persistentEncounters = this.m_persistentEncounters.Count;
    }

    public override Type[] Dependencies => new Type[2]
    {
      typeof (MySector),
      typeof (MyPirateAntennas)
    };

    protected sealed class PersistEncounterClient\u003C\u003EVRage_Game_MyEncounterId : ICallSite<IMyEventOwner, MyEncounterId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyEncounterId encounterId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEncounterGenerator.PersistEncounterClient(encounterId);
      }
    }
  }
}
