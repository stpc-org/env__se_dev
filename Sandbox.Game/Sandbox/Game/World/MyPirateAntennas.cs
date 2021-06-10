// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyPirateAntennas
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 1000, typeof (MyObjectBuilder_PirateAntennas), null, false)]
  public class MyPirateAntennas : MySessionComponentBase
  {
    private static readonly string IDENTITY_NAME = "Pirate";
    private static readonly string PIRATE_FACTION_TAG = "SPRT";
    private static readonly int DRONE_DESPAWN_TIMER = 600000;
    private static readonly int DRONE_DESPAWN_RETRY = 5000;
    private static CachingDictionary<long, MyPirateAntennas.PirateAntennaInfo> m_pirateAntennas;
    private static bool m_iteratingAntennas;
    private static Dictionary<string, MyPirateAntennaDefinition> m_definitionsByAntennaName;
    private static int m_ctr = 0;
    private static int m_ctr2 = 0;
    private static CachingDictionary<long, MyPirateAntennas.DroneInfo> m_droneInfos;
    private static long m_piratesIdentityId = 0;

    public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

    public override void LoadData()
    {
      base.LoadData();
      MyPirateAntennas.m_piratesIdentityId = 0L;
      MyPirateAntennas.m_pirateAntennas = new CachingDictionary<long, MyPirateAntennas.PirateAntennaInfo>();
      MyPirateAntennas.m_definitionsByAntennaName = new Dictionary<string, MyPirateAntennaDefinition>();
      MyPirateAntennas.m_droneInfos = new CachingDictionary<long, MyPirateAntennas.DroneInfo>();
      foreach (MyPirateAntennaDefinition antennaDefinition in MyDefinitionManager.Static.GetPirateAntennaDefinitions())
        MyPirateAntennas.m_definitionsByAntennaName[antennaDefinition.Name] = antennaDefinition;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_PirateAntennas builderPirateAntennas = sessionComponent as MyObjectBuilder_PirateAntennas;
      MyPirateAntennas.m_piratesIdentityId = builderPirateAntennas.PiratesIdentity;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (builderPirateAntennas.Drones != null)
      {
        foreach (MyObjectBuilder_PirateAntennas.MyPirateDrone drone in builderPirateAntennas.Drones)
          MyPirateAntennas.m_droneInfos.Add(drone.EntityId, MyPirateAntennas.DroneInfo.Allocate(drone.AntennaEntityId, timeInMilliseconds + drone.DespawnTimer), true);
      }
      MyPirateAntennas.m_iteratingAntennas = false;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      MyFaction factionByTag = MySession.Static.Factions.TryGetFactionByTag(MyPirateAntennas.PIRATE_FACTION_TAG, (IMyFaction) null);
      if (factionByTag != null)
      {
        if (MyPirateAntennas.m_piratesIdentityId != 0L)
        {
          if (Sync.IsServer)
          {
            (Sync.Players.TryGetIdentity(MyPirateAntennas.m_piratesIdentityId) ?? Sync.Players.CreateNewIdentity(MyPirateAntennas.IDENTITY_NAME, MyPirateAntennas.m_piratesIdentityId, (string) null, new Vector3?())).LastLoginTime = DateTime.Now;
            if (MySession.Static.Factions.GetPlayerFaction(MyPirateAntennas.m_piratesIdentityId) == null)
              MyFactionCollection.SendJoinRequest(factionByTag.FactionId, MyPirateAntennas.m_piratesIdentityId);
          }
        }
        else
          MyPirateAntennas.m_piratesIdentityId = factionByTag.FounderId;
        if (!Sync.Players.IdentityIsNpc(MyPirateAntennas.m_piratesIdentityId))
          Sync.Players.MarkIdentityAsNPC(MyPirateAntennas.m_piratesIdentityId);
      }
      foreach (KeyValuePair<long, MyPirateAntennas.DroneInfo> droneInfo in MyPirateAntennas.m_droneInfos)
      {
        MyEntity entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(droneInfo.Key, out entity);
        if (entity == null)
        {
          MyPirateAntennas.DroneInfo.Deallocate(droneInfo.Value);
          MyPirateAntennas.m_droneInfos.Remove(droneInfo.Key);
        }
        else if (!MySession.Static.Settings.EnableDrones)
        {
          MyCubeGrid myCubeGrid = entity as MyCubeGrid;
          MyRemoteControl myRemoteControl = entity as MyRemoteControl;
          if (myCubeGrid == null)
            myCubeGrid = myRemoteControl.CubeGrid;
          this.UnregisterDrone(entity, false);
          myCubeGrid.Close();
        }
        else
          this.RegisterDrone(droneInfo.Value.AntennaEntityId, entity, false);
      }
      MyPirateAntennas.m_droneInfos.ApplyRemovals();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyPirateAntennas.m_definitionsByAntennaName = (Dictionary<string, MyPirateAntennaDefinition>) null;
      foreach (KeyValuePair<long, MyPirateAntennas.DroneInfo> droneInfo in MyPirateAntennas.m_droneInfos)
      {
        MyEntity entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(droneInfo.Key, out entity);
        if (entity != null)
          this.UnregisterDrone(entity, false);
      }
      MyPirateAntennas.m_droneInfos.Clear();
      MyPirateAntennas.m_droneInfos = (CachingDictionary<long, MyPirateAntennas.DroneInfo>) null;
      MyPirateAntennas.m_pirateAntennas = (CachingDictionary<long, MyPirateAntennas.PirateAntennaInfo>) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_PirateAntennas objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_PirateAntennas;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      objectBuilder.PiratesIdentity = MyPirateAntennas.m_piratesIdentityId;
      DictionaryReader<long, MyPirateAntennas.DroneInfo> reader = MyPirateAntennas.m_droneInfos.Reader;
      objectBuilder.Drones = new MyObjectBuilder_PirateAntennas.MyPirateDrone[reader.Count];
      int index = 0;
      foreach (KeyValuePair<long, MyPirateAntennas.DroneInfo> keyValuePair in reader)
      {
        objectBuilder.Drones[index] = new MyObjectBuilder_PirateAntennas.MyPirateDrone();
        objectBuilder.Drones[index].EntityId = keyValuePair.Key;
        objectBuilder.Drones[index].AntennaEntityId = keyValuePair.Value.AntennaEntityId;
        objectBuilder.Drones[index].DespawnTimer = Math.Max(0, keyValuePair.Value.DespawnTime - timeInMilliseconds);
        ++index;
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      int num = MyDebugDrawSettings.ENABLE_DEBUG_DRAW ? 1 : 0;
      if (!Sync.IsServer)
        return;
      if (++MyPirateAntennas.m_ctr > 30)
      {
        MyPirateAntennas.m_ctr = 0;
        this.UpdateDroneSpawning();
      }
      if (++MyPirateAntennas.m_ctr2 <= 100)
        return;
      MyPirateAntennas.m_ctr2 = 0;
      this.UpdateDroneDespawning();
    }

    private void UpdateDroneSpawning()
    {
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MyPirateAntennas.m_iteratingAntennas = true;
      foreach (KeyValuePair<long, MyPirateAntennas.PirateAntennaInfo> pirateAntenna in MyPirateAntennas.m_pirateAntennas)
      {
        MyPirateAntennas.PirateAntennaInfo pirateAntennaInfo = pirateAntenna.Value;
        if (pirateAntennaInfo.IsActive && pirateAntennaInfo.AntennaDefinition != null && timeInMilliseconds - pirateAntennaInfo.LastGenerationGameTime > pirateAntennaInfo.AntennaDefinition.SpawnTimeMs)
        {
          MyRadioAntenna entity = (MyRadioAntenna) null;
          Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyRadioAntenna>(pirateAntenna.Key, out entity);
          if (pirateAntennaInfo.AntennaDefinition.SpawnGroupSampler == null)
            return;
          MySpawnGroupDefinition spawnGroup = pirateAntennaInfo.AntennaDefinition.SpawnGroupSampler.Sample();
          if (entity == null || spawnGroup == null)
          {
            pirateAntennaInfo.LastGenerationGameTime = timeInMilliseconds;
          }
          else
          {
            bool flag1 = true;
            if (entity.OwnerId != 0L)
            {
              MyIdentity identity = MySession.Static.Players.TryGetIdentity(entity.OwnerId);
              flag1 = identity != null && identity.BlockLimits.HasRemainingPCU;
            }
            if (!MySession.Static.Settings.EnableDrones || pirateAntennaInfo.SpawnedDrones >= pirateAntennaInfo.AntennaDefinition.MaxDrones || !flag1 || MyPirateAntennas.m_droneInfos.Reader.Count >= MySession.Static.Settings.MaxDrones)
            {
              pirateAntennaInfo.LastGenerationGameTime = timeInMilliseconds;
            }
            else
            {
              spawnGroup.ReloadPrefabs();
              BoundingSphereD boundingSphereD;
              ref BoundingSphereD local = ref boundingSphereD;
              MatrixD worldMatrix = entity.WorldMatrix;
              Vector3D translation = worldMatrix.Translation;
              double radius = (double) entity.GetRadius();
              local = new BoundingSphereD(translation, radius);
              ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
              bool flag2 = false;
              foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
              {
                if (boundingSphereD.Contains(myPlayer.GetPosition()) == ContainmentType.Contains && MyIDModule.GetRelationPlayerPlayer(entity.OwnerId, myPlayer.Identity.IdentityId) == MyRelationsBetweenPlayers.Enemies)
                {
                  Vector3D? nullable = new Vector3D?();
                  for (int index = 0; index < 10; ++index)
                  {
                    worldMatrix = entity.WorldMatrix;
                    nullable = Sandbox.Game.Entities.MyEntities.FindFreePlace(worldMatrix.Translation + MyUtils.GetRandomVector3Normalized() * pirateAntennaInfo.AntennaDefinition.SpawnDistance, spawnGroup.SpawnRadius);
                    if (nullable.HasValue)
                      break;
                  }
                  flag2 = nullable.HasValue && this.SpawnDrone(entity, entity.OwnerId, nullable.Value, spawnGroup);
                  break;
                }
              }
              if (flag2)
                pirateAntennaInfo.LastGenerationGameTime = timeInMilliseconds;
            }
          }
        }
      }
      MyPirateAntennas.m_pirateAntennas.ApplyChanges();
      MyPirateAntennas.m_iteratingAntennas = false;
    }

    private void UpdateDroneDespawning()
    {
      foreach (KeyValuePair<long, MyPirateAntennas.DroneInfo> droneInfo in MyPirateAntennas.m_droneInfos)
      {
        if (droneInfo.Value.DespawnTime < MySandboxGame.TotalGamePlayTimeInMilliseconds)
        {
          MyEntity entity = (MyEntity) null;
          Sandbox.Game.Entities.MyEntities.TryGetEntityById(droneInfo.Key, out entity);
          if (entity != null)
          {
            MyCubeGrid grid = entity as MyCubeGrid;
            MyRemoteControl remote = entity as MyRemoteControl;
            if (grid == null)
              grid = remote.CubeGrid;
            if (this.CanDespawn(grid, remote))
            {
              this.UnregisterDrone(entity, false);
              Sandbox.Game.Entities.MyEntities.SendCloseRequest((IMyEntity) grid);
            }
            else
              droneInfo.Value.DespawnTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + MyPirateAntennas.DRONE_DESPAWN_RETRY;
          }
          else
          {
            MyPirateAntennas.DroneInfo.Deallocate(droneInfo.Value);
            MyPirateAntennas.m_droneInfos.Remove(droneInfo.Key);
          }
        }
      }
      MyPirateAntennas.m_droneInfos.ApplyChanges();
    }

    public bool CanDespawn(MyCubeGrid grid, MyRemoteControl remote)
    {
      if (remote != null && !remote.IsFunctional)
        return false;
      BoundingSphereD worldVolume = grid.PositionComp.WorldVolume;
      worldVolume.Radius += 4000.0;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (worldVolume.Contains(onlinePlayer.GetPosition()) == ContainmentType.Contains)
          return false;
      }
      foreach (HashSet<IMyGunObject<MyDeviceBase>> myGunObjectSet in grid.GridSystems.WeaponSystem.GetGunSets().Values)
      {
        foreach (IMyGunObject<MyDeviceBase> myGunObject in myGunObjectSet)
        {
          if (myGunObject.IsShooting)
            return false;
        }
      }
      return true;
    }

    private bool SpawnDrone(
      MyRadioAntenna antenna,
      long ownerId,
      Vector3D position,
      MySpawnGroupDefinition spawnGroup,
      Vector3? spawnUp = null,
      Vector3? spawnForward = null)
    {
      long antennaEntityId = antenna.EntityId;
      Vector3D position1 = antenna.PositionComp.GetPosition();
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      Vector3D axis;
      if (closestPlanet != null)
      {
        if (!MyGravityProviderSystem.IsPositionInNaturalGravity(position1) && MyGravityProviderSystem.IsPositionInNaturalGravity(position))
        {
          MySandboxGame.Log.WriteLine("Couldn't spawn drone; antenna is not in natural gravity but spawn location is.");
          return false;
        }
        closestPlanet.CorrectSpawnLocation(ref position, (double) spawnGroup.SpawnRadius * 2.0);
        axis = position - closestPlanet.PositionComp.GetPosition();
        axis.Normalize();
      }
      else
      {
        Vector3 totalGravityInPoint = MyGravityProviderSystem.CalculateTotalGravityInPoint(position);
        if (totalGravityInPoint != Vector3.Zero)
        {
          axis = (Vector3D) -totalGravityInPoint;
          axis.Normalize();
        }
        else
          axis = !spawnUp.HasValue ? (Vector3D) MyUtils.GetRandomVector3Normalized() : (Vector3D) spawnUp.Value;
      }
      Vector3D forward = MyUtils.GetRandomPerpendicularVector(ref axis);
      if (spawnForward.HasValue)
      {
        Vector3 vector1 = spawnForward.Value;
        Vector3 vector3;
        if ((double) Math.Abs(Vector3.Dot(vector1, (Vector3) axis)) >= 0.980000019073486)
        {
          vector3 = Vector3.CalculatePerpendicularVector((Vector3) axis);
        }
        else
        {
          Vector3 vector2 = Vector3.Cross(vector1, (Vector3) axis);
          double num1 = (double) vector2.Normalize();
          vector3 = Vector3.Cross((Vector3) axis, vector2);
          double num2 = (double) vector3.Normalize();
        }
        forward = (Vector3D) vector3;
      }
      MatrixD world = MatrixD.CreateWorld(position, forward, axis);
      foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroup.Prefabs)
      {
        MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab = prefab;
        Vector3D position2 = Vector3D.Transform((Vector3D) shipPrefab.Position, world);
        Stack<Action> callbacks = new Stack<Action>();
        List<MyCubeGrid> createdGrids = new List<MyCubeGrid>();
        if (!string.IsNullOrEmpty(shipPrefab.Behaviour))
          callbacks.Push((Action) (() =>
          {
            foreach (MyCubeGrid grid in createdGrids)
            {
              if (!MyDroneAI.SetAIToGrid(grid, shipPrefab.Behaviour, shipPrefab.BehaviourActivationDistance))
                MyLog.Default.Error("Could not inject AI to encounter {0}. No remote control.", (object) grid.DisplayName);
            }
          }));
        callbacks.Push((Action) (() => this.ChangeDroneOwnership(createdGrids, ownerId, antennaEntityId)));
        MyPrefabManager.Static.SpawnPrefab(createdGrids, shipPrefab.SubtypeId, position2, (Vector3) forward, (Vector3) axis, spawningOptions: (SpawningOptions.SpawnRandomCargo | SpawningOptions.SetAuthorship), ownerId: ownerId, updateSync: true, callbacks: callbacks);
      }
      return true;
    }

    private void ChangeDroneOwnership(
      List<MyCubeGrid> gridList,
      long ownerId,
      long antennaEntityId)
    {
      foreach (MyCubeGrid grid in gridList)
      {
        grid.ChangeGridOwnership(ownerId, MyOwnershipShareModeEnum.None);
        MyRemoteControl myRemoteControl = (MyRemoteControl) null;
        foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
        {
          if (cubeBlock.FatBlock != null)
          {
            if (cubeBlock.FatBlock is MyProgrammableBlock fatBlock)
              fatBlock.SendRecompile();
            MyRemoteControl fatBlock1 = cubeBlock.FatBlock as MyRemoteControl;
            if (myRemoteControl == null)
              myRemoteControl = fatBlock1;
          }
        }
        this.RegisterDrone(antennaEntityId, (MyEntity) myRemoteControl ?? (MyEntity) grid);
      }
    }

    private void RegisterDrone(long antennaEntityId, MyEntity droneMainEntity, bool immediate = true)
    {
      MyPirateAntennas.DroneInfo droneInfo = MyPirateAntennas.DroneInfo.Allocate(antennaEntityId, MySandboxGame.TotalGamePlayTimeInMilliseconds + MyPirateAntennas.DRONE_DESPAWN_TIMER);
      MyPirateAntennas.m_droneInfos.Add(droneMainEntity.EntityId, droneInfo, immediate);
      droneMainEntity.OnClosing += new Action<MyEntity>(this.DroneMainEntityOnClosing);
      MyPirateAntennas.PirateAntennaInfo pirateAntennaInfo = (MyPirateAntennas.PirateAntennaInfo) null;
      MyEntity entity;
      if (!MyPirateAntennas.m_pirateAntennas.TryGetValue(antennaEntityId, out pirateAntennaInfo) && Sandbox.Game.Entities.MyEntities.TryGetEntityById(antennaEntityId, out entity) && entity is MyRadioAntenna myRadioAntenna)
      {
        myRadioAntenna.UpdatePirateAntenna();
        MyPirateAntennas.m_pirateAntennas.TryGetValue(antennaEntityId, out pirateAntennaInfo);
      }
      if (pirateAntennaInfo != null)
        ++pirateAntennaInfo.SpawnedDrones;
      if (!(droneMainEntity is MyRemoteControl myRemoteControl))
        return;
      myRemoteControl.OwnershipChanged += new Action<MyTerminalBlock>(this.DroneRemoteOwnershipChanged);
    }

    private void UnregisterDrone(MyEntity entity, bool immediate = true)
    {
      long key = 0;
      MyPirateAntennas.DroneInfo toDeallocate = (MyPirateAntennas.DroneInfo) null;
      MyPirateAntennas.m_droneInfos.TryGetValue(entity.EntityId, out toDeallocate);
      if (toDeallocate != null)
      {
        key = toDeallocate.AntennaEntityId;
        MyPirateAntennas.DroneInfo.Deallocate(toDeallocate);
      }
      MyPirateAntennas.m_droneInfos.Remove(entity.EntityId, immediate);
      MyPirateAntennas.PirateAntennaInfo pirateAntennaInfo = (MyPirateAntennas.PirateAntennaInfo) null;
      MyPirateAntennas.m_pirateAntennas.TryGetValue(key, out pirateAntennaInfo);
      if (pirateAntennaInfo != null)
        --pirateAntennaInfo.SpawnedDrones;
      entity.OnClosing -= new Action<MyEntity>(this.DroneMainEntityOnClosing);
      if (!(entity is MyRemoteControl myRemoteControl))
        return;
      myRemoteControl.OwnershipChanged -= new Action<MyTerminalBlock>(this.DroneRemoteOwnershipChanged);
    }

    private void DroneMainEntityOnClosing(MyEntity entity) => this.UnregisterDrone(entity);

    private void DroneRemoteOwnershipChanged(MyTerminalBlock remote)
    {
      if (Sync.Players.IdentityIsNpc(remote.OwnerId))
        return;
      this.UnregisterDrone((MyEntity) remote);
    }

    public static void UpdatePirateAntenna(
      long antennaEntityId,
      bool remove,
      bool activeState,
      StringBuilder antennaName)
    {
      if (MyPirateAntennas.m_pirateAntennas == null)
        return;
      if (remove)
      {
        MyPirateAntennas.m_pirateAntennas.Remove(antennaEntityId, !MyPirateAntennas.m_iteratingAntennas);
      }
      else
      {
        string key = antennaName.ToString();
        MyPirateAntennas.PirateAntennaInfo toDeallocate = (MyPirateAntennas.PirateAntennaInfo) null;
        if (!MyPirateAntennas.m_pirateAntennas.TryGetValue(antennaEntityId, out toDeallocate))
        {
          MyPirateAntennaDefinition antennaDef = (MyPirateAntennaDefinition) null;
          if (!MyPirateAntennas.m_definitionsByAntennaName.TryGetValue(key, out antennaDef))
            return;
          toDeallocate = MyPirateAntennas.PirateAntennaInfo.Allocate(antennaDef);
          toDeallocate.IsActive = activeState;
          MyPirateAntennas.m_pirateAntennas.Add(antennaEntityId, toDeallocate, !MyPirateAntennas.m_iteratingAntennas);
        }
        else if (toDeallocate.AntennaDefinition.Name != key)
        {
          MyPirateAntennaDefinition antennaDef = (MyPirateAntennaDefinition) null;
          if (!MyPirateAntennas.m_definitionsByAntennaName.TryGetValue(key, out antennaDef))
          {
            MyPirateAntennas.PirateAntennaInfo.Deallocate(toDeallocate);
            MyPirateAntennas.m_pirateAntennas.Remove(antennaEntityId, !MyPirateAntennas.m_iteratingAntennas);
          }
          else
          {
            toDeallocate.Reset(antennaDef);
            toDeallocate.IsActive = activeState;
          }
        }
        else
          toDeallocate.IsActive = activeState;
      }
    }

    public static long GetPiratesId() => MyPirateAntennas.m_piratesIdentityId;

    private static void DebugDraw()
    {
      foreach (KeyValuePair<long, MyPirateAntennas.PirateAntennaInfo> pirateAntenna in MyPirateAntennas.m_pirateAntennas)
      {
        MyRadioAntenna entity = (MyRadioAntenna) null;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyRadioAntenna>(pirateAntenna.Key, out entity);
        if (entity != null)
        {
          int num = Math.Max(0, pirateAntenna.Value.AntennaDefinition.SpawnTimeMs - MySandboxGame.TotalGamePlayTimeInMilliseconds + pirateAntenna.Value.LastGenerationGameTime);
          MyRenderProxy.DebugDrawText3D(entity.WorldMatrix.Translation, "Time remaining: " + num.ToString(), Color.Red, 1f, false);
        }
      }
      foreach (KeyValuePair<long, MyPirateAntennas.PirateAntennaInfo> pirateAntenna in MyPirateAntennas.m_pirateAntennas)
      {
        MyEntity entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(pirateAntenna.Key, out entity);
        if (entity != null)
          MyRenderProxy.DebugDrawSphere(entity.WorldMatrix.Translation, (float) entity.PositionComp.WorldVolume.Radius, Color.BlueViolet, depthRead: false);
      }
      foreach (KeyValuePair<long, MyPirateAntennas.DroneInfo> droneInfo in MyPirateAntennas.m_droneInfos)
      {
        MyEntity entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(droneInfo.Key, out entity);
        if (entity != null)
        {
          if (!(entity is MyCubeGrid myCubeGrid))
            myCubeGrid = (entity as MyRemoteControl).CubeGrid;
          MyRenderProxy.DebugDrawSphere(myCubeGrid.PositionComp.WorldVolume.Center, (float) myCubeGrid.PositionComp.WorldVolume.Radius, Color.Cyan, depthRead: false);
          MyRenderProxy.DebugDrawText3D(myCubeGrid.PositionComp.WorldVolume.Center, ((droneInfo.Value.DespawnTime - MySandboxGame.TotalGamePlayTimeInMilliseconds) / 1000).ToString(), Color.Cyan, 0.7f, false);
        }
      }
    }

    private static void RandomShuffle<T>(List<T> input)
    {
      for (int index = input.Count - 1; index > 1; --index)
      {
        int randomInt = MyUtils.GetRandomInt(0, index);
        T obj = input[index];
        input[index] = input[randomInt];
        input[randomInt] = obj;
      }
    }

    private class PirateAntennaInfo
    {
      public MyPirateAntennaDefinition AntennaDefinition;
      public int LastGenerationGameTime;
      public int SpawnedDrones;
      public bool IsActive;
      public List<int> SpawnPositionsIndexes;
      public int CurrentSpawnPositionsIndex = -1;
      public static List<MyPirateAntennas.PirateAntennaInfo> m_pool = new List<MyPirateAntennas.PirateAntennaInfo>();

      public static MyPirateAntennas.PirateAntennaInfo Allocate(
        MyPirateAntennaDefinition antennaDef)
      {
        MyPirateAntennas.PirateAntennaInfo pirateAntennaInfo;
        if (MyPirateAntennas.PirateAntennaInfo.m_pool.Count == 0)
        {
          pirateAntennaInfo = new MyPirateAntennas.PirateAntennaInfo();
        }
        else
        {
          pirateAntennaInfo = MyPirateAntennas.PirateAntennaInfo.m_pool[MyPirateAntennas.PirateAntennaInfo.m_pool.Count - 1];
          MyPirateAntennas.PirateAntennaInfo.m_pool.RemoveAt(MyPirateAntennas.PirateAntennaInfo.m_pool.Count - 1);
        }
        pirateAntennaInfo.Reset(antennaDef);
        return pirateAntennaInfo;
      }

      public static void Deallocate(MyPirateAntennas.PirateAntennaInfo toDeallocate)
      {
        toDeallocate.AntennaDefinition = (MyPirateAntennaDefinition) null;
        toDeallocate.SpawnPositionsIndexes = (List<int>) null;
        MyPirateAntennas.PirateAntennaInfo.m_pool.Add(toDeallocate);
      }

      public void Reset(MyPirateAntennaDefinition antennaDef)
      {
        this.AntennaDefinition = antennaDef;
        this.LastGenerationGameTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + antennaDef.FirstSpawnTimeMs - antennaDef.SpawnTimeMs;
        this.SpawnedDrones = 0;
        this.IsActive = false;
        this.SpawnPositionsIndexes = (List<int>) null;
        this.CurrentSpawnPositionsIndex = -1;
      }
    }

    private class DroneInfo
    {
      public long AntennaEntityId;
      public int DespawnTime;
      public static List<MyPirateAntennas.DroneInfo> m_pool = new List<MyPirateAntennas.DroneInfo>();

      public static MyPirateAntennas.DroneInfo Allocate(
        long antennaEntityId,
        int despawnTime)
      {
        MyPirateAntennas.DroneInfo droneInfo;
        if (MyPirateAntennas.DroneInfo.m_pool.Count == 0)
        {
          droneInfo = new MyPirateAntennas.DroneInfo();
        }
        else
        {
          droneInfo = MyPirateAntennas.DroneInfo.m_pool[MyPirateAntennas.DroneInfo.m_pool.Count - 1];
          MyPirateAntennas.DroneInfo.m_pool.RemoveAt(MyPirateAntennas.DroneInfo.m_pool.Count - 1);
        }
        droneInfo.AntennaEntityId = antennaEntityId;
        droneInfo.DespawnTime = despawnTime;
        return droneInfo;
      }

      public static void Deallocate(MyPirateAntennas.DroneInfo toDeallocate)
      {
        toDeallocate.AntennaEntityId = 0L;
        toDeallocate.DespawnTime = 0;
        MyPirateAntennas.DroneInfo.m_pool.Add(toDeallocate);
      }
    }
  }
}
