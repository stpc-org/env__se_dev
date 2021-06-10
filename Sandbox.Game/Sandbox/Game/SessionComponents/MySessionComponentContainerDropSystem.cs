// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentContainerDropSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 888, typeof (MyObjectBuilder_SessionComponentContainerDropSystem), null, false)]
  public class MySessionComponentContainerDropSystem : MySessionComponentBase
  {
    private readonly Random random = new Random();
    private readonly int DESPAWN_SMOKE_TIME = 15;
    private static readonly short ONE_MINUTE = 60;
    private static readonly short TWO_MINUTES = 120;
    public const string DROP_TRIGGER_NAME = "Special Content";
    public const string DROP_DEPOWER_NAME = "Special Content Power";
    private static MySoundPair m_explosionSound = new MySoundPair("WepSmallWarheadExpl", false);
    private MyContainerDropSystemDefinition m_definition;
    private int m_counter;
    private uint m_containerIdSmall = 1;
    private uint m_containerIdLarge = 1;
    private List<MyContainerGPS> m_delayedGPSForRemoval;
    private List<MyEntityForRemoval> m_delayedEntitiesForRemoval;
    private List<MySessionComponentContainerDropSystem.MyPlayerContainerData> m_playerData = new List<MySessionComponentContainerDropSystem.MyPlayerContainerData>();
    private Dictionary<MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>, List<MyDropContainerDefinition>> m_dropContainerLists;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyPersonalSpace;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyPersonalAtmosphere;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyPersonalMoon;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyCompetetiveSpace;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyCompetetiveAtmosphere;
    private MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool> m_keyCompetetiveMoon;
    private bool m_hasNewItems;
    private List<MyGameInventoryItem> m_newGameItems;
    private Dictionary<MyEntity, MyParticleEffect> m_smokeParticles = new Dictionary<MyEntity, MyParticleEffect>();
    private Dictionary<MyGps, MyEntityForRemoval> m_gpsList = new Dictionary<MyGps, MyEntityForRemoval>();
    private List<MyGps> m_gpsToRemove = new List<MyGps>();
    private bool m_nothingDropped;
    private bool m_enableWindowPopups = true;
    private int m_minDropContainerRespawnTime;
    private int m_maxDropContainerRespawnTime;
    private float m_DropContainerRespawnTimeMultiplier = 1f;

    public bool EnableWindowPopups
    {
      get => this.m_enableWindowPopups;
      set => this.m_enableWindowPopups = value;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      if (Sync.IsServer)
      {
        MyObjectBuilder_SessionComponentContainerDropSystem containerDropSystem = sessionComponent as MyObjectBuilder_SessionComponentContainerDropSystem;
        this.m_delayedGPSForRemoval = containerDropSystem.GPSForRemoval != null ? containerDropSystem.GPSForRemoval : new List<MyContainerGPS>();
        this.m_delayedEntitiesForRemoval = containerDropSystem.EntitiesForRemoval != null ? containerDropSystem.EntitiesForRemoval : new List<MyEntityForRemoval>();
        this.m_containerIdSmall = containerDropSystem.ContainerIdSmall;
        this.m_containerIdLarge = containerDropSystem.ContainerIdLarge;
        if (containerDropSystem.PlayerData != null)
        {
          foreach (PlayerContainerData playerContainerData in containerDropSystem.PlayerData)
            this.m_playerData.Add(new MySessionComponentContainerDropSystem.MyPlayerContainerData(playerContainerData.PlayerId, playerContainerData.Timer, playerContainerData.Active, playerContainerData.Competetive, playerContainerData.ContainerId));
        }
      }
      if (MyGameService.IsActive)
      {
        MyGameService.ItemsAdded += new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
        MyGameService.NoItemsReceived += new EventHandler(this.MyGameServiceNoItemsReceived);
      }
      this.m_minDropContainerRespawnTime = MySession.Static.MinDropContainerRespawnTime;
      this.m_maxDropContainerRespawnTime = MySession.Static.MaxDropContainerRespawnTime;
      if (this.m_minDropContainerRespawnTime <= this.m_maxDropContainerRespawnTime)
        return;
      MyLog.Default.WriteLine("MinDropContainerRespawnTime is higher than MaxDropContainerRespawnTime. Clamping to Max.");
      this.m_minDropContainerRespawnTime = this.m_maxDropContainerRespawnTime;
    }

    protected override void UnloadData()
    {
      if (MyGameService.IsActive)
      {
        MyGameService.ItemsAdded -= new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
        MyGameService.NoItemsReceived -= new EventHandler(this.MyGameServiceNoItemsReceived);
      }
      base.UnloadData();
    }

    private void MyGameServiceNoItemsReceived(object sender, System.EventArgs e) => this.m_nothingDropped = true;

    private void MyGameService_ItemsAdded(object sender, MyGameItemsEventArgs e)
    {
      this.m_newGameItems = e.NewItems;
      this.m_hasNewItems = this.m_newGameItems != null && this.m_newGameItems.Count > 0;
      if (this.m_newGameItems.Count != 1)
        return;
      this.m_newGameItems[0].IsNew = true;
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      this.m_definition = definition as MyContainerDropSystemDefinition;
      DictionaryReader<string, MyDropContainerDefinition> containerDefinitions = MyDefinitionManager.Static.GetDropContainerDefinitions();
      this.m_dropContainerLists = new Dictionary<MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>, List<MyDropContainerDefinition>>();
      this.m_keyPersonalSpace = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Space, false);
      this.m_keyPersonalAtmosphere = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Atmosphere, false);
      this.m_keyPersonalMoon = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Moon, false);
      this.m_keyCompetetiveSpace = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Space, true);
      this.m_keyCompetetiveAtmosphere = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Atmosphere, true);
      this.m_keyCompetetiveMoon = new MyTuple<MySessionComponentContainerDropSystem.SpawnType, bool>(MySessionComponentContainerDropSystem.SpawnType.Moon, true);
      this.m_dropContainerLists[this.m_keyPersonalSpace] = new List<MyDropContainerDefinition>();
      this.m_dropContainerLists[this.m_keyPersonalAtmosphere] = new List<MyDropContainerDefinition>();
      this.m_dropContainerLists[this.m_keyPersonalMoon] = new List<MyDropContainerDefinition>();
      this.m_dropContainerLists[this.m_keyCompetetiveSpace] = new List<MyDropContainerDefinition>();
      this.m_dropContainerLists[this.m_keyCompetetiveAtmosphere] = new List<MyDropContainerDefinition>();
      this.m_dropContainerLists[this.m_keyCompetetiveMoon] = new List<MyDropContainerDefinition>();
      foreach (KeyValuePair<string, MyDropContainerDefinition> keyValuePair in containerDefinitions)
      {
        if ((double) keyValuePair.Value.Priority > 0.0 && keyValuePair.Value.Prefab != null)
        {
          if (keyValuePair.Value.SpawnRules.CanBePersonal)
          {
            if (keyValuePair.Value.SpawnRules.CanSpawnInSpace)
              this.m_dropContainerLists[this.m_keyPersonalSpace].Add(keyValuePair.Value);
            if (keyValuePair.Value.SpawnRules.CanSpawnInAtmosphere)
              this.m_dropContainerLists[this.m_keyPersonalAtmosphere].Add(keyValuePair.Value);
            if (keyValuePair.Value.SpawnRules.CanSpawnOnMoon)
              this.m_dropContainerLists[this.m_keyPersonalMoon].Add(keyValuePair.Value);
          }
          if (keyValuePair.Value.SpawnRules.CanBeCompetetive)
          {
            if (keyValuePair.Value.SpawnRules.CanSpawnInSpace)
              this.m_dropContainerLists[this.m_keyCompetetiveSpace].Add(keyValuePair.Value);
            if (keyValuePair.Value.SpawnRules.CanSpawnInAtmosphere)
              this.m_dropContainerLists[this.m_keyCompetetiveAtmosphere].Add(keyValuePair.Value);
            if (keyValuePair.Value.SpawnRules.CanSpawnOnMoon)
              this.m_dropContainerLists[this.m_keyCompetetiveMoon].Add(keyValuePair.Value);
          }
        }
      }
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentContainerDropSystem objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentContainerDropSystem;
      objectBuilder.PlayerData = new List<PlayerContainerData>();
      foreach (MySessionComponentContainerDropSystem.MyPlayerContainerData playerContainerData in this.m_playerData)
        objectBuilder.PlayerData.Add(new PlayerContainerData(playerContainerData.PlayerId, playerContainerData.Timer, playerContainerData.Active, playerContainerData.Competetive, playerContainerData.Container != null ? playerContainerData.Container.EntityId : 0L));
      objectBuilder.GPSForRemoval = this.m_delayedGPSForRemoval;
      objectBuilder.EntitiesForRemoval = this.m_delayedEntitiesForRemoval;
      objectBuilder.ContainerIdSmall = this.m_containerIdSmall;
      objectBuilder.ContainerIdLarge = this.m_containerIdLarge;
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.UpdateSmokeParticles();
      if (this.m_counter % 60 != 0)
        return;
      foreach (KeyValuePair<MyGps, MyEntityForRemoval> gps in this.m_gpsList)
      {
        if (gps.Value.TimeLeft > (int) MySessionComponentContainerDropSystem.TWO_MINUTES)
        {
          if (gps.Value.TimeLeft % (int) MySessionComponentContainerDropSystem.ONE_MINUTE == 59)
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int>((Func<IMyEventOwner, Action<string, int>>) (x => new Action<string, int>(MySessionComponentContainerDropSystem.UpdateGPSRemainingTime)), gps.Key.Name, gps.Value.TimeLeft);
        }
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int>((Func<IMyEventOwner, Action<string, int>>) (x => new Action<string, int>(MySessionComponentContainerDropSystem.UpdateGPSRemainingTime)), gps.Key.Name, gps.Value.TimeLeft);
        if (gps.Value.TimeLeft <= 0)
          this.m_gpsToRemove.Add(gps.Key);
      }
      foreach (MyGps key in this.m_gpsToRemove)
        this.m_gpsList.Remove(key);
      this.m_gpsToRemove.Clear();
    }

    public void RemovePlayerDataForIdentity(long identityId)
    {
      if (!Sync.IsServer)
        return;
      for (int index = 0; index < this.m_playerData.Count; ++index)
      {
        if (this.m_playerData[index].PlayerId == identityId)
        {
          this.m_playerData.RemoveAt(index);
          --index;
        }
      }
    }

    private void UpdateSmokeParticles()
    {
      foreach (KeyValuePair<MyEntity, MyParticleEffect> smokeParticle in this.m_smokeParticles)
        smokeParticle.Value.WorldMatrix = smokeParticle.Key.WorldMatrix;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!MySession.Static.EnableContainerDrops || MySandboxGame.IsPaused || this.m_counter++ % 60 != 0)
        return;
      if (this.EnableWindowPopups && this.m_hasNewItems && this.m_newGameItems != null)
      {
        this.m_hasNewItems = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenNewGameItems>((object) this.m_newGameItems));
        this.m_newGameItems.Clear();
      }
      if (this.EnableWindowPopups && this.m_nothingDropped)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenNoGameItemDrop>());
        this.m_nothingDropped = false;
      }
      if (!Sync.IsServer)
        return;
      this.UpdateContainerSpawner();
      this.UpdateGPSRemoval();
      this.UpdateContainerEntityRemoval();
      int timer = MathHelper.RoundToInt((float) this.random.Next(this.m_minDropContainerRespawnTime, this.m_maxDropContainerRespawnTime) * this.m_DropContainerRespawnTimeMultiplier);
      if (this.m_playerData.Count == 0 && !Sandbox.Engine.Platform.Game.IsDedicated)
        this.m_playerData.Add(new MySessionComponentContainerDropSystem.MyPlayerContainerData(MySession.Static.LocalPlayerId, timer, true, false, 0L));
      if (this.m_counter < 3600)
        return;
      this.m_counter = 1;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        bool flag = false;
        for (int index = 0; index < this.m_playerData.Count; ++index)
        {
          if (this.m_playerData[index].PlayerId == onlinePlayer.Identity.IdentityId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.m_playerData.Add(new MySessionComponentContainerDropSystem.MyPlayerContainerData(onlinePlayer.Identity.IdentityId, timer, true, false, 0L));
      }
    }

    private void UpdateContainerSpawner()
    {
      for (int index = 0; index < this.m_playerData.Count; ++index)
      {
        MySessionComponentContainerDropSystem.MyPlayerContainerData playerData = this.m_playerData[index];
        if (playerData.ContainerId != 0L)
        {
          MyEntity entity = (MyEntity) null;
          Sandbox.Game.Entities.MyEntities.TryGetEntityByName("Special Content", out entity);
          playerData.Container = entity as MyTerminalBlock;
          playerData.ContainerId = 0L;
        }
        if (playerData.Active)
        {
          --playerData.Timer;
          if (playerData.Timer <= 0)
          {
            bool flag = this.SpawnContainerDrop(playerData);
            int num = MathHelper.RoundToInt((float) this.random.Next(this.m_minDropContainerRespawnTime, this.m_maxDropContainerRespawnTime) * this.m_DropContainerRespawnTimeMultiplier);
            playerData.Timer = flag ? num : (int) MySessionComponentContainerDropSystem.ONE_MINUTE;
            playerData.Active = !flag || playerData.Competetive;
          }
        }
        else if (playerData.Container != null && (playerData.Container.Closed || !playerData.Container.InScene || !playerData.Container.Components.Contains(typeof (MyContainerDropComponent))))
        {
          playerData.Container = (MyTerminalBlock) null;
          playerData.Active = true;
        }
      }
    }

    private void UpdateContainerEntityRemoval()
    {
      if (this.m_delayedEntitiesForRemoval == null)
        return;
      for (int index = 0; index < this.m_delayedEntitiesForRemoval.Count; ++index)
      {
        MyEntityForRemoval entityForRemoval = this.m_delayedEntitiesForRemoval[index];
        --entityForRemoval.TimeLeft;
        if ((double) entityForRemoval.TimeLeft <= 0.0)
        {
          MyEntity entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityForRemoval.EntityId, out entity))
          {
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, string, bool>((Func<IMyEventOwner, Action<long, string, bool>>) (s => new Action<long, string, bool>(MySessionComponentContainerDropSystem.PlayParticleBroadcast)), entityForRemoval.EntityId, "Explosion_Missile", false);
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySessionComponentContainerDropSystem.StopSmoke)), entityForRemoval.EntityId);
            entity.Close();
          }
          this.m_delayedEntitiesForRemoval.RemoveAt(index);
          --index;
        }
        else
        {
          MyEntity entity;
          if (entityForRemoval.TimeLeft == this.DESPAWN_SMOKE_TIME && Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityForRemoval.EntityId, out entity) && !this.m_smokeParticles.ContainsKey(entity))
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, string, bool>((Func<IMyEventOwner, Action<long, string, bool>>) (s => new Action<long, string, bool>(MySessionComponentContainerDropSystem.PlayParticleBroadcast)), entityForRemoval.EntityId, "Smoke_Container", true);
        }
      }
    }

    [Event(null, 429)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void StopSmoke(long entityId)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
        return;
      MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().StopSmoke(entity);
    }

    private void StopSmoke(MyEntity entity)
    {
      if (!this.m_smokeParticles.ContainsKey(entity))
        return;
      this.m_smokeParticles[entity].Stop();
      this.m_smokeParticles.Remove(entity);
    }

    [Event(null, 449)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlayParticleBroadcast(long entityId, string particleName, bool smoke)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
        return;
      MyParticleEffect effect = MySessionComponentContainerDropSystem.PlayParticle(entity, particleName);
      if (smoke)
      {
        if (effect == null)
          return;
        MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().AddSmoke(entity, effect);
      }
      else
      {
        MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
        if (soundEmitter == null)
          return;
        soundEmitter.SetPosition(new Vector3D?(entity.PositionComp.GetPosition()));
        soundEmitter.Entity = entity;
        soundEmitter.PlaySound(MySessionComponentContainerDropSystem.m_explosionSound);
      }
    }

    private void AddSmoke(MyEntity entity, MyParticleEffect effect) => this.m_smokeParticles[entity] = effect;

    private static MyParticleEffect PlayParticle(
      MyEntity entity,
      string particleName)
    {
      MyParticleEffect effect = (MyParticleEffect) null;
      if (MyParticlesManager.TryCreateParticleEffect(particleName, entity.WorldMatrix, out effect))
        effect.Play();
      return effect;
    }

    private void UpdateGPSRemoval()
    {
      if (this.m_delayedGPSForRemoval == null)
        return;
      for (int index = 0; index < this.m_delayedGPSForRemoval.Count; ++index)
      {
        MyContainerGPS myContainerGps = this.m_delayedGPSForRemoval[index];
        --myContainerGps.TimeLeft;
        if ((double) myContainerGps.TimeLeft <= 0.0)
        {
          MySessionComponentContainerDropSystem.RemoveGPS(myContainerGps.GPSName);
          this.m_delayedGPSForRemoval.RemoveAt(index);
          --index;
        }
      }
    }

    public void RegisterDelayedGPSRemovalInternal(string name, int time) => this.m_delayedGPSForRemoval.Add(new MyContainerGPS(time, name));

    public void ContainerDestroyed(MyContainerDropComponent container)
    {
      if (!container.Competetive)
      {
        for (int index = 0; index < this.m_playerData.Count; ++index)
        {
          if (this.m_playerData[index].PlayerId == container.Owner)
            this.m_playerData[index].Active = true;
        }
        MySessionComponentContainerDropSystem.RemoveGPS(container.GPSName, container.Owner);
      }
      else
        MySessionComponentContainerDropSystem.RemoveGPS(container.GPSName);
    }

    public void ContainerOpened(MyContainerDropComponent container, long playerId)
    {
      if (container.Entity == null)
        return;
      if (container.Competetive)
      {
        MyGameService.TriggerCompetitiveContainer();
        if (Sync.IsServer)
          MySessionComponentContainerDropSystem.CompetetiveContainerOpened(container.GPSName, this.m_definition.CompetetiveContainerGPSTimeOut, playerId, this.m_definition.CompetetiveContainerGPSColorClaimed);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, long, Color>((Func<IMyEventOwner, Action<string, int, long, Color>>) (x => new Action<string, int, long, Color>(MySessionComponentContainerDropSystem.CompetetiveContainerOpened)), container.GPSName, this.m_definition.CompetetiveContainerGPSTimeOut, playerId, this.m_definition.CompetetiveContainerGPSColorClaimed);
      }
      else
      {
        MyGameService.TriggerPersonalContainer();
        if (Sync.IsServer)
          MySessionComponentContainerDropSystem.RemoveGPS(container.GPSName, container.Owner);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, long>((Func<IMyEventOwner, Action<string, long>>) (x => new Action<string, long>(MySessionComponentContainerDropSystem.RemoveGPS)), container.GPSName, container.Owner);
        for (int index = 0; index < this.m_playerData.Count; ++index)
        {
          if (this.m_playerData[index].PlayerId == playerId)
            this.m_playerData[index].Active = true;
        }
      }
      if (container.Entity == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySessionComponentContainerDropSystem.RemoveContainerDropComponent)), container.Entity.EntityId);
    }

    private bool SpawnContainerDrop(
      MySessionComponentContainerDropSystem.MyPlayerContainerData playerData)
    {
      bool flag = false;
      ICollection<MyPlayer> players = Sync.Players.GetOnlinePlayers();
      Vector3D basePosition = Vector3D.Zero;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) players)
      {
        if (myPlayer.Identity.IdentityId == playerData.PlayerId && myPlayer.Controller.ControlledEntity != null)
        {
          flag = true;
          basePosition = myPlayer.Controller.ControlledEntity.Entity.PositionComp.GetPosition();
          break;
        }
      }
      if (!flag)
      {
        playerData.Competetive = true;
        return true;
      }
      bool personal = !Sync.MultiplayerActive || Sync.Players.GetOnlinePlayerCount() <= 1 || (double) MyUtils.GetRandomFloat() <= 0.949999988079071;
      playerData.Competetive = !personal;
      bool validSpawn;
      MyPlanet planet;
      Vector3D newSpawnPosition = this.FindNewSpawnPosition(personal, out validSpawn, out planet, basePosition);
      if (!validSpawn)
        return false;
      Vector3D gpsPosition = newSpawnPosition;
      Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(gpsPosition);
      if (planet != null)
        gpsPosition = planet.GetClosestSurfacePointGlobal(ref newSpawnPosition);
      List<MyDropContainerDefinition> containerDefinitionList = planet == null || naturalGravityInPoint == Vector3D.Zero ? this.m_dropContainerLists[personal ? this.m_keyPersonalSpace : this.m_keyCompetetiveSpace] : (!planet.HasAtmosphere ? this.m_dropContainerLists[personal ? this.m_keyPersonalMoon : this.m_keyCompetetiveMoon] : this.m_dropContainerLists[personal ? this.m_keyPersonalAtmosphere : this.m_keyCompetetiveAtmosphere]);
      MyDropContainerDefinition containerDefinition1 = (MyDropContainerDefinition) null;
      if (containerDefinitionList.Count == 0)
        return false;
      if (containerDefinitionList.Count == 1)
      {
        containerDefinition1 = containerDefinitionList[0];
      }
      else
      {
        float maxValue = 0.0f;
        foreach (MyDropContainerDefinition containerDefinition2 in containerDefinitionList)
          maxValue += containerDefinition2.Priority;
        float randomFloat = MyUtils.GetRandomFloat(0.0f, maxValue);
        foreach (MyDropContainerDefinition containerDefinition2 in containerDefinitionList)
        {
          if ((double) randomFloat <= (double) containerDefinition2.Priority)
          {
            containerDefinition1 = containerDefinition2;
            break;
          }
          randomFloat -= containerDefinition2.Priority;
        }
      }
      if (containerDefinition1 == null)
        return false;
      List<MyCubeGrid> resultGridList = new List<MyCubeGrid>();
      Action action1 = (Action) (() =>
      {
        playerData.Container = (MyTerminalBlock) null;
        MyCubeGrid myCubeGrid = resultGridList.Count > 0 ? resultGridList[0] : (MyCubeGrid) null;
        if (myCubeGrid != null)
        {
          foreach (MyTerminalBlock fatBlock in myCubeGrid.GetFatBlocks<MyTerminalBlock>())
          {
            if (fatBlock != null && (fatBlock.CustomName != null ? fatBlock.CustomName.ToString() : string.Empty).Equals("Special Content"))
            {
              playerData.Container = fatBlock;
              break;
            }
          }
        }
        if (myCubeGrid == null || playerData.Container == null)
        {
          playerData.Active = true;
        }
        else
        {
          myCubeGrid.IsRespawnGrid = true;
          MyEntityForRemoval entityForRemoval = new MyEntityForRemoval(playerData.Competetive ? this.m_definition.CompetetiveContainerGridTimeOut : this.m_definition.PersonalContainerGridTimeOut, myCubeGrid.EntityId);
          this.m_delayedEntitiesForRemoval.Add(entityForRemoval);
          string str1 = playerData.Competetive ? MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerLarge) : MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerSmall);
          string message = string.Format(MyTexts.GetString(MySpaceTexts.ContainerDropSystemContainerWasDetected), (object) str1);
          string str2 = str1 + " ";
          string gpsName;
          if (personal)
          {
            gpsName = str2 + this.m_containerIdSmall.ToString();
            ++this.m_containerIdSmall;
          }
          else
          {
            gpsName = str2 + this.m_containerIdLarge.ToString();
            ++this.m_containerIdLarge;
          }
          playerData.Container.Components.Add(typeof (MyContainerDropComponent), (MyComponentBase) new MyContainerDropComponent(playerData.Competetive, gpsName, playerData.PlayerId, this.m_definition.ContainerAudioCue)
          {
            GridEntityId = myCubeGrid.EntityId
          });
          playerData.Container.ChangeOwner(0L, MyOwnershipShareModeEnum.All);
          List<long> longList = new List<long>();
          if (playerData.Competetive)
          {
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<string, int, string, long>((Func<IMyEventOwner, Action<string, int, string, long>>) (s => new Action<string, int, string, long>(MySessionComponentContainerDropSystem.ShowNotificationSync)), message, 5000, "Blue".ToString(), 0L);
            foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) players)
              longList.Add(myPlayer.Identity.IdentityId);
          }
          else
          {
            MySessionComponentContainerDropSystem.ShowNotificationSync(message, 5000, "Blue".ToString(), playerData.PlayerId);
            longList.Add(playerData.PlayerId);
          }
          Color color = playerData.Competetive ? this.m_definition.CompetetiveContainerGPSColorFree : this.m_definition.PersonalContainerGPSColor;
          foreach (long identityId in longList)
          {
            MyGps gps = new MyGps()
            {
              ShowOnHud = true,
              Name = gpsName,
              DisplayName = str1,
              DiscardAt = new TimeSpan?(),
              Coords = gpsPosition,
              Description = "",
              AlwaysVisible = true,
              GPSColor = color,
              IsContainerGPS = true
            };
            this.m_gpsList.Add(gps, entityForRemoval);
            MySession.Static.Gpss.SendAddGps(identityId, ref gps, playerData.Container.EntityId);
          }
        }
      });
      Action action2 = (Action) (() =>
      {
        foreach (MyCubeGrid myCubeGrid in resultGridList)
        {
          foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
          {
            if (fatBlock is MyTerminalBlock myTerminalBlock && myTerminalBlock.CustomName.ToString() != "Special Content")
              myTerminalBlock.SetCustomName("Special Content Power");
          }
        }
      });
      Stack<Action> actionStack1 = new Stack<Action>();
      actionStack1.Push(action2);
      actionStack1.Push(action1);
      Vector3 vector1 = naturalGravityInPoint != Vector3.Zero ? Vector3.Normalize(naturalGravityInPoint) * -1f : Vector3.Normalize(MyUtils.GetRandomVector3());
      Vector3 vector2 = !(vector1 != Vector3.Left) || !(vector1 != Vector3.Right) ? Vector3.Forward : Vector3.Right;
      Vector3 vector3 = Vector3.Normalize(Vector3.Cross(vector1, vector2));
      MyPrefabManager myPrefabManager = MyPrefabManager.Static;
      List<MyCubeGrid> resultList = resultGridList;
      string subtypeName = containerDefinition1.Prefab.Id.SubtypeName;
      Vector3D position = newSpawnPosition;
      Vector3 forward = vector3;
      Vector3 up = vector1;
      string str = MyTexts.GetString(MySpaceTexts.ContainerDropSystemBeaconText);
      Stack<Action> actionStack2 = actionStack1;
      Vector3 initialLinearVelocity = (Vector3) naturalGravityInPoint;
      Vector3 initialAngularVelocity = new Vector3();
      string beaconName = str;
      Stack<Action> callbacks = actionStack2;
      myPrefabManager.SpawnPrefab(resultList, subtypeName, position, forward, up, initialLinearVelocity, initialAngularVelocity, beaconName, spawningOptions: SpawningOptions.SpawnRandomCargo, updateSync: true, callbacks: callbacks);
      return true;
    }

    private Vector3D FindNewSpawnPosition(
      bool personal,
      out bool validSpawn,
      out MyPlanet planet,
      Vector3D basePosition)
    {
      validSpawn = false;
      planet = (MyPlanet) null;
      Vector3D vector3D = Vector3D.Zero;
      float minValue = personal ? this.m_definition.PersonalContainerDistMin : this.m_definition.CompetetiveContainerDistMin;
      float maxValue = personal ? this.m_definition.PersonalContainerDistMax : this.m_definition.CompetetiveContainerDistMax;
      for (int index = 15; index > 0; --index)
      {
        vector3D = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(minValue, maxValue) + basePosition;
        if (this.IsSpawnPositionFree(vector3D, 50.0))
        {
          if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D) != Vector3D.Zero)
          {
            planet = MyGamePruningStructure.GetClosestPlanet(vector3D);
            vector3D = this.GetPlanetarySpawnPosition(vector3D, planet);
            if (this.IsSpawnPositionFree(vector3D, 50.0))
            {
              validSpawn = true;
              break;
            }
          }
          else
          {
            validSpawn = true;
            break;
          }
        }
      }
      return vector3D;
    }

    private bool IsSpawnPositionFree(Vector3D position, double size)
    {
      BoundingSphereD sphere = new BoundingSphereD(position, size);
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, result);
      bool flag = true;
      foreach (MyEntity myEntity in result)
      {
        if (!(myEntity is MyPlanet))
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    private Vector3D GetPlanetarySpawnPosition(Vector3D position, MyPlanet planet)
    {
      if (planet == null)
        return position;
      Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      return planet.GetClosestSurfacePointGlobal(ref position) - Vector3D.Normalize(naturalGravityInPoint) * (planet.HasAtmosphere ? 2000.0 : 10.0);
    }

    [Event(null, 853)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ShowNotificationSync(
      string message,
      int showTime,
      string font,
      long playerId)
    {
      if (Sync.IsValidEventOnServer && !MyEventContext.Current.IsLocallyInvoked)
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
      else
      {
        if (MyAPIGateway.Utilities == null || playerId != 0L && playerId != MySession.Static.LocalPlayerId)
          return;
        MyAPIGateway.Utilities.ShowNotification(message, showTime, font);
      }
    }

    public static void ModifyGPSColorForAll(string name, Color color)
    {
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, name) is MyGps gpsByName)
        {
          gpsByName.GPSColor = color;
          MySession.Static.Gpss.SendModifyGps(onlinePlayer.Identity.IdentityId, gpsByName);
        }
      }
    }

    [Event(null, 882)]
    [Reliable]
    [Server]
    public static void RemoveGPS(string name, long playerId = 0)
    {
      if (playerId == 0L)
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, name);
          if (gpsByName != null)
            MySession.Static.Gpss.SendDelete(onlinePlayer.Identity.IdentityId, gpsByName.Hash);
        }
      }
      else
      {
        IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(playerId, name);
        if (gpsByName == null)
          return;
        MySession.Static.Gpss.SendDelete(playerId, gpsByName.Hash);
      }
    }

    [Event(null, 903)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateGPSRemainingTime(string gpsName, int remainingTime)
    {
      if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
      else
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          IMyGps gpsByName = MySession.Static.Gpss.GetGpsByName(onlinePlayer.Identity.IdentityId, gpsName);
          if (gpsByName != null)
          {
            string empty = string.Empty;
            string str;
            if (remainingTime >= (int) MySessionComponentContainerDropSystem.TWO_MINUTES)
            {
              int num = remainingTime / (int) MySessionComponentContainerDropSystem.ONE_MINUTE;
              str = string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMins), (object) num);
            }
            else if (remainingTime >= (int) MySessionComponentContainerDropSystem.ONE_MINUTE)
            {
              int num1 = remainingTime / (int) MySessionComponentContainerDropSystem.ONE_MINUTE;
              int num2 = remainingTime % (int) MySessionComponentContainerDropSystem.ONE_MINUTE;
              str = num2 != 1 ? string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMinSecs), (object) num1, (object) num2) : string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeMinSec), (object) num1, (object) num2);
            }
            else
              str = remainingTime <= 1 || remainingTime >= (int) MySessionComponentContainerDropSystem.ONE_MINUTE ? string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeSec), (object) remainingTime) : string.Format(MyTexts.GetString(MyCommonTexts.GpsContainerRemainingTimeSecs), (object) remainingTime);
            gpsByName.ContainerRemainingTime = str;
          }
        }
      }
    }

    [Event(null, 954)]
    [Reliable]
    [Server]
    public static void CompetetiveContainerOpened(
      string name,
      int time,
      long playerId,
      Color color)
    {
      MySessionComponentContainerDropSystem.RemoveGPS(name, playerId);
      MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().RegisterDelayedGPSRemovalInternal(name, time);
      MySessionComponentContainerDropSystem.ModifyGPSColorForAll(name, color);
    }

    [Event(null, 962)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveContainerDropComponent(long entityId)
    {
      MyEntity entity1;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity1))
        return;
      MyContainerDropComponent containerDropComponent = entity1.Components.Get<MyContainerDropComponent>();
      MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
      if (component != null && containerDropComponent != null)
      {
        component.RemoveDelayedRemovalEntity(containerDropComponent.GridEntityId);
        if (containerDropComponent.GridEntityId == 0L)
        {
          if (entity1 is MyCubeBlock myCubeBlock && myCubeBlock.CubeGrid != null)
            myCubeBlock.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -2L);
        }
        else
        {
          MyEntity entity2;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(containerDropComponent.GridEntityId, out entity2) && entity2 is MyCubeGrid myCubeGrid)
            myCubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -2L);
        }
      }
      if (entity1.Components == null)
        return;
      entity1.Components.Remove<MyContainerDropComponent>();
    }

    private void RemoveDelayedRemovalEntity(long entityId)
    {
      if (this.m_delayedEntitiesForRemoval == null)
        return;
      MyEntityForRemoval entityForRemoval = this.m_delayedEntitiesForRemoval.FirstOrDefault<MyEntityForRemoval>((Func<MyEntityForRemoval, bool>) (e => e.EntityId == entityId));
      if (entityForRemoval == null)
        return;
      this.m_delayedEntitiesForRemoval.Remove(entityForRemoval);
    }

    [Conditional("DEBUG")]
    public void SetRespawnTimeMultiplier(float multiplier = 1f)
    {
      if ((double) Math.Abs(multiplier) < 9.99999974737875E-06)
        return;
      this.m_DropContainerRespawnTimeMultiplier = multiplier;
    }

    public float GetRespawnTimeMultiplier() => this.m_DropContainerRespawnTimeMultiplier;

    private class MyPlayerContainerData
    {
      public long PlayerId;
      public int Timer;
      public bool Active;
      public bool Competetive;
      public MyTerminalBlock Container;
      public long ContainerId;

      public MyPlayerContainerData(
        long playerId,
        int timer,
        bool active,
        bool competetive,
        long cargoId)
      {
        this.PlayerId = playerId;
        this.Timer = timer;
        this.Active = active;
        this.Competetive = competetive;
        this.ContainerId = cargoId;
      }
    }

    private enum SpawnType
    {
      Space,
      Atmosphere,
      Moon,
    }

    protected sealed class StopSmoke\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.StopSmoke(entityId);
      }
    }

    protected sealed class PlayParticleBroadcast\u003C\u003ESystem_Int64\u0023System_String\u0023System_Boolean : ICallSite<IMyEventOwner, long, string, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in string particleName,
        in bool smoke,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.PlayParticleBroadcast(entityId, particleName, smoke);
      }
    }

    protected sealed class ShowNotificationSync\u003C\u003ESystem_String\u0023System_Int32\u0023System_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, int, string, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string message,
        in int showTime,
        in string font,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.ShowNotificationSync(message, showTime, font, playerId);
      }
    }

    protected sealed class RemoveGPS\u003C\u003ESystem_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string name,
        in long playerId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.RemoveGPS(name, playerId);
      }
    }

    protected sealed class UpdateGPSRemainingTime\u003C\u003ESystem_String\u0023System_Int32 : ICallSite<IMyEventOwner, string, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string gpsName,
        in int remainingTime,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.UpdateGPSRemainingTime(gpsName, remainingTime);
      }
    }

    protected sealed class CompetetiveContainerOpened\u003C\u003ESystem_String\u0023System_Int32\u0023System_Int64\u0023VRageMath_Color : ICallSite<IMyEventOwner, string, int, long, Color, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string name,
        in int time,
        in long playerId,
        in Color color,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.CompetetiveContainerOpened(name, time, playerId, color);
      }
    }

    protected sealed class RemoveContainerDropComponent\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContainerDropSystem.RemoveContainerDropComponent(entityId);
      }
    }
  }
}
