// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.World.MySpaceRespawnComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using ParallelTasks;
using Sandbox;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using SpaceEngineers.Game.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.World
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MySpaceRespawnComponent : MyRespawnComponentBase
  {
    private int m_lastUpdate;
    private bool m_updatingStopped;
    private int m_updateCtr;
    private bool m_synced;
    private readonly List<MySpaceRespawnComponent.RespawnCooldownEntry> m_tmpRespawnTimes = new List<MySpaceRespawnComponent.RespawnCooldownEntry>();
    private const int REPEATED_DEATH_TIME_SECONDS = 10;
    private readonly CachingDictionary<MySpaceRespawnComponent.RespawnKey, int> m_globalRespawnTimesMs = new CachingDictionary<MySpaceRespawnComponent.RespawnKey, int>();
    private static List<MyRespawnShipDefinition> m_respawnShipsCache;
    private static readonly List<MySpaceRespawnComponent.MyRespawnPointInfo> m_respanwPointsCache = new List<MySpaceRespawnComponent.MyRespawnPointInfo>();
    private static readonly List<Vector3D> m_playerPositionsCache = new List<Vector3D>();

    public bool IsSynced => this.m_synced;

    public static MySpaceRespawnComponent Static => Sync.Players.RespawnComponent as MySpaceRespawnComponent;

    public event Action<ulong> RespawnDoneEvent;

    public void RequestSync() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySpaceRespawnComponent.OnSyncCooldownRequest)));

    public override void InitFromCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
    {
      List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> respawnCooldowns = checkpoint.RespawnCooldowns;
      this.m_lastUpdate = MySandboxGame.TotalTimeInMilliseconds;
      this.m_globalRespawnTimesMs.Clear();
      if (respawnCooldowns == null)
        return;
      foreach (MyObjectBuilder_Checkpoint.RespawnCooldownItem respawnCooldownItem in respawnCooldowns)
      {
        MyPlayer.PlayerId result = new MyPlayer.PlayerId()
        {
          SteamId = respawnCooldownItem.PlayerSteamId,
          SerialId = respawnCooldownItem.PlayerSerialId
        };
        if (respawnCooldownItem.IdentityId != 0L)
          Sync.Players.TryGetPlayerId(respawnCooldownItem.IdentityId, out result);
        this.m_globalRespawnTimesMs.Add(new MySpaceRespawnComponent.RespawnKey()
        {
          ControllerId = result,
          RespawnShipId = respawnCooldownItem.RespawnShipId
        }, respawnCooldownItem.Cooldown + this.m_lastUpdate, true);
      }
    }

    public override void SaveToCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
    {
      List<MyObjectBuilder_Checkpoint.RespawnCooldownItem> respawnCooldowns = checkpoint.RespawnCooldowns;
      foreach (KeyValuePair<MySpaceRespawnComponent.RespawnKey, int> globalRespawnTimesM in this.m_globalRespawnTimesMs)
      {
        int num = globalRespawnTimesM.Value - this.m_lastUpdate;
        if (num > 0)
          respawnCooldowns.Add(new MyObjectBuilder_Checkpoint.RespawnCooldownItem()
          {
            IdentityId = Sync.Players.TryGetIdentityId(globalRespawnTimesM.Key.ControllerId.SteamId, globalRespawnTimesM.Key.ControllerId.SerialId),
            RespawnShipId = globalRespawnTimesM.Key.RespawnShipId,
            Cooldown = num
          });
      }
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      this.m_lastUpdate = MySandboxGame.TotalTimeInMilliseconds;
      this.m_updatingStopped = true;
      this.m_updateCtr = 0;
      if (!Sync.IsServer)
      {
        this.m_synced = false;
        this.RequestSync();
      }
      else
      {
        this.RequestSync();
        this.m_synced = true;
      }
    }

    public override void LoadData()
    {
      base.LoadData();
      Sync.Players.RespawnComponent = (MyRespawnComponentBase) this;
      Sync.Players.LocalRespawnRequested += new Action<string, Color>(this.OnLocalRespawnRequest);
      MyRespawnComponentBase.ShowPermaWarning = false;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      Sync.Players.LocalRespawnRequested -= new Action<string, Color>(this.OnLocalRespawnRequest);
      Sync.Players.RespawnComponent = (MyRespawnComponentBase) null;
    }

    [Event(null, 174)]
    [Reliable]
    [Server]
    private static void OnSyncCooldownRequest()
    {
      if (MyEventContext.Current.IsLocallyInvoked)
        MySpaceRespawnComponent.Static.SyncCooldownToPlayer(Sync.MyId, true);
      else
        MySpaceRespawnComponent.Static.SyncCooldownToPlayer(MyEventContext.Current.Sender.Value, false);
    }

    [Event(null, 188)]
    [Reliable]
    [Client]
    private static void OnSyncCooldownResponse(
      List<MySpaceRespawnComponent.RespawnCooldownEntry> entries)
    {
      MySpaceRespawnComponent.Static.SyncCooldownResponse(entries);
    }

    private void SyncCooldownResponse(
      List<MySpaceRespawnComponent.RespawnCooldownEntry> entries)
    {
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      if (entries != null)
      {
        foreach (MySpaceRespawnComponent.RespawnCooldownEntry entry in entries)
        {
          MyPlayer.PlayerId playerId = new MyPlayer.PlayerId()
          {
            SteamId = Sync.MyId,
            SerialId = entry.ControllerId
          };
          this.m_globalRespawnTimesMs.Add(new MySpaceRespawnComponent.RespawnKey()
          {
            ControllerId = playerId,
            RespawnShipId = entry.ShipId
          }, timeInMilliseconds + entry.RelativeRespawnTime, true);
        }
      }
      this.m_synced = true;
    }

    public void SyncCooldownToPlayer(ulong steamId, bool isLocal)
    {
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      this.m_tmpRespawnTimes.Clear();
      foreach (KeyValuePair<MySpaceRespawnComponent.RespawnKey, int> globalRespawnTimesM in this.m_globalRespawnTimesMs)
      {
        if ((long) globalRespawnTimesM.Key.ControllerId.SteamId == (long) steamId)
          this.m_tmpRespawnTimes.Add(new MySpaceRespawnComponent.RespawnCooldownEntry()
          {
            ControllerId = globalRespawnTimesM.Key.ControllerId.SerialId,
            ShipId = globalRespawnTimesM.Key.RespawnShipId,
            RelativeRespawnTime = globalRespawnTimesM.Value - timeInMilliseconds
          });
      }
      if (isLocal)
        MySpaceRespawnComponent.OnSyncCooldownResponse(this.m_tmpRespawnTimes);
      else
        MyMultiplayer.RaiseStaticEvent<List<MySpaceRespawnComponent.RespawnCooldownEntry>>((Func<IMyEventOwner, Action<List<MySpaceRespawnComponent.RespawnCooldownEntry>>>) (s => new Action<List<MySpaceRespawnComponent.RespawnCooldownEntry>>(MySpaceRespawnComponent.OnSyncCooldownResponse)), this.m_tmpRespawnTimes, new EndpointId(steamId));
      this.m_tmpRespawnTimes.Clear();
    }

    public override void UpdatingStopped()
    {
      base.UpdatingStopped();
      this.m_updatingStopped = true;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      int delta = timeInMilliseconds - this.m_lastUpdate;
      if (this.m_updatingStopped)
      {
        this.UpdateRespawnTimes(delta);
        this.m_lastUpdate = timeInMilliseconds;
        this.m_updatingStopped = false;
      }
      else
      {
        ++this.m_updateCtr;
        this.m_lastUpdate = timeInMilliseconds;
        if (this.m_updateCtr % 100 != 0)
          return;
        this.RemoveOldRespawnTimes();
      }
    }

    private void UpdateRespawnTimes(int delta)
    {
      foreach (MySpaceRespawnComponent.RespawnKey key in this.m_globalRespawnTimesMs.Keys)
        this.m_globalRespawnTimesMs[key] += delta;
      this.m_globalRespawnTimesMs.ApplyAdditionsAndModifications();
    }

    private void RemoveOldRespawnTimes()
    {
      MyDefinitionManager.Static.GetRespawnShipDefinitions();
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      foreach (MySpaceRespawnComponent.RespawnKey key in this.m_globalRespawnTimesMs.Keys)
      {
        int globalRespawnTimesM = this.m_globalRespawnTimesMs[key];
        if (timeInMilliseconds - globalRespawnTimesM >= 0)
          this.m_globalRespawnTimesMs.Remove(key);
      }
      this.m_globalRespawnTimesMs.ApplyRemovals();
    }

    public void ResetRespawnCooldown(MyPlayer.PlayerId controllerId)
    {
      DictionaryReader<string, MyRespawnShipDefinition> respawnShipDefinitions = MyDefinitionManager.Static.GetRespawnShipDefinitions();
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      float shipTimeMultiplier = MySession.Static.Settings.SpawnShipTimeMultiplier;
      foreach (KeyValuePair<string, MyRespawnShipDefinition> keyValuePair in respawnShipDefinitions)
      {
        MySpaceRespawnComponent.RespawnKey key = new MySpaceRespawnComponent.RespawnKey()
        {
          ControllerId = controllerId,
          RespawnShipId = keyValuePair.Key
        };
        if ((double) shipTimeMultiplier != 0.0)
          this.m_globalRespawnTimesMs.Add(key, timeInMilliseconds + (int) ((double) (keyValuePair.Value.Cooldown * 1000) * (double) shipTimeMultiplier), true);
        else
          this.m_globalRespawnTimesMs.Remove(key);
      }
    }

    public int GetRespawnCooldownSeconds(MyPlayer.PlayerId controllerId, string respawnShipId)
    {
      if (MyDefinitionManager.Static.GetRespawnShipDefinition(respawnShipId) == null)
        return 0;
      MySpaceRespawnComponent.RespawnKey key = new MySpaceRespawnComponent.RespawnKey()
      {
        ControllerId = controllerId,
        RespawnShipId = respawnShipId
      };
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      int num = timeInMilliseconds;
      this.m_globalRespawnTimesMs.TryGetValue(key, out num);
      return Math.Max((num - timeInMilliseconds) / 1000, 0);
    }

    private void OnLocalRespawnRequest(string model, Color color)
    {
      if (MyFakes.SHOW_FACTIONS_GUI)
        MyMultiplayer.RaiseStaticEvent<ulong, int>((Func<IMyEventOwner, Action<ulong, int>>) (s => new Action<ulong, int>(MySpaceRespawnComponent.RespawnRequest_Implementation)), MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.Id.SteamId : Sync.MyId, MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.Id.SerialId : 0);
      else
        MyPlayerCollection.RespawnRequest(MySession.Static.LocalHumanPlayer == null, false, 0L, (string) null, 0, model, color);
    }

    [Event(null, 352)]
    [Reliable]
    [Server]
    private static void RespawnRequest_Implementation(ulong steamPlayerId, int serialId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MyEventContext.Current.Sender.Value != (long) steamPlayerId)
      {
        ((MyMultiplayerServerBase) MyMultiplayer.Static).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(steamPlayerId, serialId));
        if (!MySpaceRespawnComponent.TryFindExistingCharacter(playerById))
        {
          bool flag = true;
          long num1 = 0;
          Vector3D? nullable;
          if (MySession.Static.Settings.EnableAutorespawn)
          {
            nullable = playerById.Identity.LastDeathPosition;
            if (nullable.HasValue)
            {
              using (ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo> availableRespawnPoints = MySpaceRespawnComponent.GetAvailableRespawnPoints(new long?(playerById.Identity.IdentityId), false))
              {
                if (availableRespawnPoints.List.Count > 0)
                {
                  nullable = playerById.Identity.LastDeathPosition;
                  Vector3D lastPlayerPosition = nullable.Value;
                  MySpaceRespawnComponent.MyRespawnPointInfo respawnPointInfo = availableRespawnPoints.List.MinBy<MySpaceRespawnComponent.MyRespawnPointInfo>((Func<MySpaceRespawnComponent.MyRespawnPointInfo, float>) (x => (float) Vector3D.Distance(x.MedicalRoomPosition, lastPlayerPosition)));
                  if (MySession.Static.ElapsedGameTime - playerById.Identity.LastRespawnTime < TimeSpan.FromSeconds(10.0))
                  {
                    num1 = respawnPointInfo.MedicalRoomId;
                  }
                  else
                  {
                    long medicalRoomId = respawnPointInfo.MedicalRoomId;
                    EndpointId targetEndpoint = new EndpointId(steamPlayerId);
                    nullable = new Vector3D?();
                    Vector3D? position = nullable;
                    MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySpaceRespawnComponent.RequestRespawnAtSpawnPoint)), medicalRoomId, targetEndpoint, position);
                    flag = false;
                  }
                }
              }
            }
          }
          if (flag)
          {
            MySpaceRespawnComponent.MOTDData motdData = MySpaceRespawnComponent.MOTDData.Construct();
            long num2 = num1;
            EndpointId targetEndpoint = new EndpointId(steamPlayerId);
            nullable = new Vector3D?();
            Vector3D? position = nullable;
            MyMultiplayer.RaiseStaticEvent<MySpaceRespawnComponent.MOTDData, long>((Func<IMyEventOwner, Action<MySpaceRespawnComponent.MOTDData, long>>) (s => new Action<MySpaceRespawnComponent.MOTDData, long>(MySpaceRespawnComponent.ShowMedicalScreen_Implementation)), motdData, num2, targetEndpoint, position);
          }
        }
        else if (!Sandbox.Engine.Platform.Game.IsDedicated && (long) Sync.MyId == (long) steamPlayerId)
          MySpaceRespawnComponent.ShowMotD(MySpaceRespawnComponent.MOTDData.Construct());
        else
          MyMultiplayer.RaiseStaticEvent<MySpaceRespawnComponent.MOTDData>((Func<IMyEventOwner, Action<MySpaceRespawnComponent.MOTDData>>) (s => new Action<MySpaceRespawnComponent.MOTDData>(MySpaceRespawnComponent.ShowMotD)), MySpaceRespawnComponent.MOTDData.Construct(), new EndpointId(steamPlayerId));
        MyRespawnComponentBase.NotifyRespawnRequested(playerById);
      }
    }

    private static bool TryFindExistingCharacter(MyPlayer player)
    {
      if (player == null)
        return false;
      foreach (long savedCharacter in player.Identity.SavedCharacters)
      {
        if (Sandbox.Game.Entities.MyEntities.GetEntityById(savedCharacter) is MyCharacter entityById && !entityById.IsDead)
        {
          if (entityById.Parent == null)
          {
            MySession.Static.Players.SetControlledEntity(player.Id, (MyEntity) entityById);
            player.Identity.ChangeCharacter(entityById);
            MySession.SendVicinityInformation(entityById.EntityId, new EndpointId(player.Client.SteamUserId));
            return true;
          }
          if (entityById.Parent is MyCockpit)
          {
            MyCockpit parent = entityById.Parent as MyCockpit;
            MySession.Static.Players.SetControlledEntity(player.Id, (MyEntity) parent);
            player.Identity.ChangeCharacter(parent.Pilot);
            MySession.SendVicinityInformation(entityById.EntityId, new EndpointId(player.Client.SteamUserId));
            return true;
          }
        }
      }
      return false;
    }

    [Event(null, 447)]
    [Reliable]
    [Client]
    private static void RequestRespawnAtSpawnPoint(long spawnPointId)
    {
      string model = (string) null;
      Color red = Color.Red;
      MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref red);
      MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, false, spawnPointId, (string) null, 0, model, red);
    }

    [Event(null, 456)]
    [Reliable]
    [Client]
    private static void ShowMedicalScreen_Implementation(
      MySpaceRespawnComponent.MOTDData motd,
      long restrictedRespawn)
    {
      MyGuiScreenMedicals guiScreenMedicals = new MyGuiScreenMedicals((MySession.Static.Factions.JoinableFactionsPresent || MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION) && MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId) == null, restrictedRespawn);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) guiScreenMedicals);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (motd.HasMessage)
        guiScreenMedicals.SetMotD(motd.GetMessage().ToString());
      if (motd.HasUrl)
        MyGuiScreenMedicals.ShowMotDUrl(motd.Url);
      MySession.ShowMotD = false;
    }

    [Event(null, 479)]
    [Reliable]
    [Client]
    private static void ShowMotD(MySpaceRespawnComponent.MOTDData motd)
    {
      if (motd.HasMessage)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenMotD(motd.GetMessage()));
      if (motd.HasUrl)
        MyGuiScreenMedicals.ShowMotDUrl(motd.Url);
      MySession.ShowMotD = false;
    }

    public static ClearToken<MyRespawnShipDefinition> GetRespawnShips(
      MyPlanet planet)
    {
      MyUtils.Init<List<MyRespawnShipDefinition>>(ref MySpaceRespawnComponent.m_respawnShipsCache).AssertEmpty<MyRespawnShipDefinition>();
      IEnumerable<MyRespawnShipDefinition> values = MyDefinitionManager.Static.GetRespawnShipDefinitions().Values;
      if (planet.HasAtmosphere)
      {
        float num = planet.Generator.Atmosphere.Density * 0.95f;
        foreach (MyRespawnShipDefinition respawnShipDefinition in values)
        {
          if (respawnShipDefinition.UseForPlanetsWithAtmosphere && (double) num >= (double) respawnShipDefinition.MinimalAirDensity)
            MySpaceRespawnComponent.m_respawnShipsCache.Add(respawnShipDefinition);
        }
      }
      else
      {
        foreach (MyRespawnShipDefinition respawnShipDefinition in values)
        {
          if (respawnShipDefinition.UseForPlanetsWithoutAtmosphere)
            MySpaceRespawnComponent.m_respawnShipsCache.Add(respawnShipDefinition);
        }
      }
      string subtypeName = planet.Generator.Id.SubtypeName;
      for (int index = MySpaceRespawnComponent.m_respawnShipsCache.Count - 1; index >= 0; --index)
      {
        bool flag = false;
        MyRespawnShipDefinition respawnShipDefinition = MySpaceRespawnComponent.m_respawnShipsCache[index];
        if (respawnShipDefinition.PlanetTypes != null && !respawnShipDefinition.PlanetTypes.Contains<string>(subtypeName))
          flag = true;
        if (respawnShipDefinition.SpawnPosition.HasValue)
          flag = true;
        if (flag)
          MySpaceRespawnComponent.m_respawnShipsCache.RemoveAt(index);
      }
      return MySpaceRespawnComponent.m_respawnShipsCache.GetClearToken<MyRespawnShipDefinition>();
    }

    public static MyRespawnShipDefinition GetRandomRespawnShip(
      MyPlanet planet)
    {
      using (ClearToken<MyRespawnShipDefinition> respawnShips = MySpaceRespawnComponent.GetRespawnShips(planet))
      {
        List<MyRespawnShipDefinition> list = respawnShips.List;
        return list.Count == 0 ? (MyRespawnShipDefinition) null : list[MyUtils.GetRandomInt(list.Count)];
      }
    }

    public override bool HandleRespawnRequest(
      bool joinGame,
      bool newIdentity,
      long respawnEntityId,
      string respawnShipId,
      MyPlayer.PlayerId playerId,
      Vector3D? spawnPosition,
      Vector3? direction,
      Vector3? up,
      SerializableDefinitionId? botDefinitionId,
      bool realPlayer,
      string modelName,
      Color color)
    {
      MyPlayer player = Sync.Players.GetPlayerById(playerId);
      bool flag = newIdentity || player == null;
      Vector3D vector3D1 = Vector3D.Zero;
      if (player != null && player.Character != null)
        vector3D1 = player.Character.PositionComp.GetPosition();
      if (MySpaceRespawnComponent.TryFindExistingCharacter(player))
        return true;
      MyBotDefinition botDefinition1 = (MyBotDefinition) null;
      if (botDefinitionId.HasValue)
        MyDefinitionManager.Static.TryGetBotDefinition((MyDefinitionId) botDefinitionId.Value, out botDefinition1);
      long? planetId = new long?();
      if (Sandbox.Game.Entities.MyEntities.GetEntityById(respawnEntityId) is MyPlanet entityById)
      {
        planetId = new long?(respawnEntityId);
        if (string.IsNullOrEmpty(respawnShipId))
        {
          MyRespawnShipDefinition randomRespawnShip = MySpaceRespawnComponent.GetRandomRespawnShip(entityById);
          if (randomRespawnShip != null)
            respawnShipId = randomRespawnShip.Id.SubtypeName;
        }
      }
      if (!flag)
      {
        if (respawnShipId != null)
        {
          this.SpawnAtShip(player, respawnShipId, botDefinition1, modelName, new Color?(color), planetId);
          return true;
        }
        if (spawnPosition.HasValue)
        {
          Vector3 up1;
          if (up.HasValue)
          {
            up1 = up.Value;
          }
          else
          {
            Vector3D vector3D2 = (Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(spawnPosition.Value);
            if (Vector3D.IsZero(vector3D2))
              vector3D2 = Vector3D.Down;
            else
              vector3D2.Normalize();
            up1 = (Vector3) -vector3D2;
          }
          Vector3 result;
          if (direction.HasValue)
            result = direction.Value;
          else
            up1.CalculatePerpendicularVector(out result);
          player.SpawnAt(MatrixD.CreateWorld(spawnPosition.Value, result, up1), Vector3.Zero, (MyEntity) null, botDefinition1, modelName: modelName, color: new Color?(color));
          return true;
        }
        MyRespawnComponent respawn = (MyRespawnComponent) null;
        if (respawnEntityId == 0L || !MyFakes.SHOW_FACTIONS_GUI)
        {
          using (ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo> availableRespawnPoints = MySpaceRespawnComponent.GetAvailableRespawnPoints(MySession.Static.CreativeMode ? new long?() : new long?(player.Identity.IdentityId), false))
          {
            List<MySpaceRespawnComponent.MyRespawnPointInfo> list = availableRespawnPoints.List;
            if (joinGame)
            {
              if (list.Count > 0)
                respawn = this.FindRespawnById(list[MyRandom.Instance.Next(0, list.Count)].MedicalRoomId, (MyPlayer) null);
            }
          }
        }
        else
        {
          respawn = this.FindRespawnById(respawnEntityId, player);
          if (respawn == null)
            return false;
        }
        if (respawn != null)
          this.SpawnInRespawn(player, respawn, botDefinition1, modelName, color);
        else
          flag = true;
      }
      if (flag)
      {
        bool resetIdentity = false;
        if (MySession.Static.Settings.PermanentDeath.Value)
        {
          MyIdentity playerIdentity = Sync.Players.TryGetPlayerIdentity(playerId);
          if (playerIdentity != null)
            resetIdentity = playerIdentity.FirstSpawnDone;
        }
        if (player == null)
        {
          player = Sync.Players.CreateNewPlayer(Sync.Players.CreateNewIdentity(playerId.SteamId.ToString(), (string) null, new Vector3?(), false, false), playerId, playerId.SteamId.ToString(), realPlayer, false, false);
          resetIdentity = false;
        }
        if (MySession.Static.CreativeMode)
        {
          Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace(vector3D1, 2f, 200);
          if (freePlace.HasValue)
            vector3D1 = freePlace.Value;
          MyPlayer myPlayer = player;
          Matrix translation = Matrix.CreateTranslation((Vector3) vector3D1);
          MatrixD worldMatrix = (MatrixD) ref translation;
          Vector3 zero = Vector3.Zero;
          MyBotDefinition botDefinition2 = botDefinition1;
          string modelName1 = modelName;
          Color? color1 = new Color?(color);
          myPlayer.SpawnAt(worldMatrix, zero, (MyEntity) null, botDefinition2, modelName: modelName1, color: color1);
        }
        else
          this.SpawnAsNewPlayer(player, vector3D1, respawnShipId, planetId, resetIdentity, botDefinition1, modelName, color);
      }
      return true;
    }

    private void SpawnInRespawn(
      MyPlayer player,
      MyRespawnComponent respawn,
      MyBotDefinition botDefinition,
      string modelName,
      Color color)
    {
      if (respawn.Entity == null)
      {
        this.SpawnInSuit(player, (MyEntity) null, botDefinition, modelName, color);
      }
      else
      {
        MyEntity topMostParent = respawn.Entity.GetTopMostParent((Type) null);
        if (topMostParent.Physics == null)
          this.SpawnInSuit(player, topMostParent, botDefinition, modelName, color);
        else if (respawn.Entity is MyCockpit entity)
        {
          this.SpawnInCockpit(player, entity, botDefinition, entity.WorldMatrix, modelName, new Color?(color), true);
        }
        else
        {
          MatrixD spawnPosition = respawn.GetSpawnPosition();
          Vector3 velocityAtPoint = topMostParent.Physics.GetVelocityAtPoint(spawnPosition.Translation);
          player.SpawnAt(spawnPosition, velocityAtPoint, topMostParent, botDefinition, modelName: modelName, color: new Color?(color));
          if (!(respawn.Entity is MyMedicalRoom entity))
            return;
          entity.TryTakeSpawneeOwnership(player);
          entity.TrySetFaction(player);
          if (!entity.ForceSuitChangeOnRespawn)
            return;
          player.Character.ChangeModelAndColor(entity.RespawnSuitName, player.Character.ColorMask);
          if (!MySession.Static.Settings.EnableOxygen || player.Character.OxygenComponent == null || !player.Character.OxygenComponent.NeedsOxygenFromSuit)
            return;
          player.Character.OxygenComponent.SwitchHelmet();
        }
      }
    }

    private MyRespawnComponent FindRespawnById(
      long respawnBlockId,
      MyPlayer player)
    {
      MyCubeBlock entity = (MyCubeBlock) null;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeBlock>(respawnBlockId, out entity))
        return (MyRespawnComponent) null;
      if (!entity.IsWorking)
        return (MyRespawnComponent) null;
      MyRespawnComponent respawnComponent = (MyRespawnComponent) entity.Components.Get<MyEntityRespawnComponentBase>();
      if (respawnComponent == null)
        return (MyRespawnComponent) null;
      if (!respawnComponent.SpawnWithoutOxygen && (double) respawnComponent.GetOxygenLevel() == 0.0)
        return (MyRespawnComponent) null;
      return player != null && !respawnComponent.CanPlayerSpawn(player.Identity.IdentityId, true) ? (MyRespawnComponent) null : respawnComponent;
    }

    public static ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo> GetAvailableRespawnPoints(
      long? identityId,
      bool includePublicSpawns)
    {
      MySpaceRespawnComponent.m_respanwPointsCache.AssertEmpty<MySpaceRespawnComponent.MyRespawnPointInfo>();
      foreach (MyRespawnComponent allRespawn in MyRespawnComponent.GetAllRespawns())
      {
        MyTerminalBlock entity = allRespawn.Entity;
        if (entity != null && !entity.Closed)
        {
          if (Sync.IsServer)
          {
            entity.CubeGrid?.GridSystems?.UpdatePower();
            entity.UpdateIsWorking();
          }
          if (entity.IsWorking && (!identityId.HasValue || allRespawn.CanPlayerSpawn(identityId.Value, includePublicSpawns)))
          {
            MySpaceRespawnComponent.MyRespawnPointInfo respawnPointInfo = new MySpaceRespawnComponent.MyRespawnPointInfo();
            IMySpawnBlock mySpawnBlock = (IMySpawnBlock) entity;
            respawnPointInfo.MedicalRoomId = entity.EntityId;
            respawnPointInfo.MedicalRoomGridId = entity.CubeGrid.EntityId;
            respawnPointInfo.MedicalRoomName = !string.IsNullOrEmpty(mySpawnBlock.SpawnName) ? mySpawnBlock.SpawnName : (entity.CustomName != null ? entity.CustomName.ToString() : (entity.Name != null ? entity.Name : entity.ToString()));
            respawnPointInfo.OxygenLevel = allRespawn.GetOxygenLevel();
            respawnPointInfo.OwnerId = entity.IDModule.Owner;
            MatrixD worldMatrix = entity.WorldMatrix;
            Vector3D translation = worldMatrix.Translation;
            Vector3D basePos = translation + worldMatrix.Up * 20.0 + entity.WorldMatrix.Right * 20.0 + worldMatrix.Forward * 20.0;
            Vector3D? nullable = Sandbox.Game.Entities.MyEntities.FindFreePlace(basePos, 1f);
            if (!nullable.HasValue)
              nullable = new Vector3D?(basePos);
            respawnPointInfo.PrefferedCameraPosition = nullable.Value;
            respawnPointInfo.MedicalRoomPosition = translation;
            respawnPointInfo.MedicalRoomUp = worldMatrix.Up;
            if (entity.CubeGrid.Physics != null)
              respawnPointInfo.MedicalRoomVelocity = entity.CubeGrid.Physics.LinearVelocity;
            MySpaceRespawnComponent.m_respanwPointsCache.Add(respawnPointInfo);
          }
        }
      }
      return new ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo>()
      {
        List = MySpaceRespawnComponent.m_respanwPointsCache
      };
    }

    public void SpawnAsNewPlayer(
      MyPlayer player,
      Vector3D currentPosition,
      string respawnShipId,
      long? planetId,
      bool resetIdentity,
      MyBotDefinition botDefinition,
      string modelName,
      Color color)
    {
      if (!Sync.IsServer || player == null || player.Identity == null)
        return;
      if (resetIdentity)
        this.ResetPlayerIdentity(player, modelName, color);
      if (respawnShipId != null)
        this.SpawnAtShip(player, respawnShipId, botDefinition, modelName, new Color?(color), planetId);
      else
        this.SpawnInSuit(player, (MyEntity) null, botDefinition, modelName, color);
      if (MySession.Static == null || player.Character == null || (!MySession.Static.Settings.EnableOxygen || player.Character.OxygenComponent == null) || !player.Character.OxygenComponent.NeedsOxygenFromSuit)
        return;
      player.Character.OxygenComponent.SwitchHelmet();
    }

    public void SpawnAtShip(
      MyPlayer player,
      string respawnShipId,
      MyBotDefinition botDefinition,
      string modelName,
      Color? color,
      long? planetId = null)
    {
      if (!Sync.IsServer)
        return;
      this.ResetRespawnCooldown(player.Id);
      if (Sync.MultiplayerActive)
        this.SyncCooldownToPlayer(player.Id.SteamId, (long) player.Id.SteamId == (long) Sync.MyId);
      MyRespawnShipDefinition respawnShip = MyDefinitionManager.Static.GetRespawnShipDefinition(respawnShipId);
      if (respawnShip == null)
        return;
      MySpaceRespawnComponent.SpawnInfo spawnInfo = new MySpaceRespawnComponent.SpawnInfo()
      {
        SpawnNearPlayers = true,
        IdentityId = player.Identity.IdentityId,
        PlanetDeployAltitude = respawnShip.PlanetDeployAltitude,
        CollisionRadius = respawnShip.Prefab.BoundingSphere.Radius,
        MinimalAirDensity = respawnShip.UseForPlanetsWithoutAtmosphere ? 0.0f : respawnShip.MinimalAirDensity
      };
      Vector3 up = Vector3.Zero;
      Vector3 forward = Vector3.Zero;
      Vector3D position = Vector3D.Zero;
      if (!planetId.HasValue && respawnShip.PlanetTypes != null)
        planetId = MyPlanets.GetPlanets().FirstOrDefault<MyPlanet>((Func<MyPlanet, bool>) (x => respawnShip.PlanetTypes.Contains<string>(x.Generator.Id.SubtypeName)))?.EntityId;
      if (planetId.HasValue)
      {
        if (Sandbox.Game.Entities.MyEntities.GetEntityById(planetId.Value) is MyPlanet entityById)
        {
          spawnInfo.Planet = entityById;
          MySpaceRespawnComponent.GetSpawnPositionNearPlanet(in spawnInfo, out position, out forward, out up);
        }
        else
          planetId = new long?();
      }
      if (!planetId.HasValue)
      {
        if (respawnShip.SpawnPosition.HasValue)
        {
          position = respawnShip.SpawnPosition.Value;
          MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(position);
          Vector2 spawnPositionDispersion = new Vector2(respawnShip.SpawnPositionDispersionMin, respawnShip.SpawnPositionDispersionMax);
          // ISSUE: explicit non-virtual call
          if (closestPlanet != null && __nonvirtual (closestPlanet.PositionComp).WorldAABB.Contains(position) == ContainmentType.Contains)
          {
            spawnInfo.Planet = closestPlanet;
            MySpaceRespawnComponent.FindSpawnPositionAbovePlanetInPredefinedArea(in spawnInfo, spawnPositionDispersion, ref position, out forward, out up);
          }
          else
          {
            spawnInfo.Planet = (MyPlanet) null;
            MySpaceRespawnComponent.FindSpawnPositionInSpaceInPredefinedArea(in spawnInfo, respawnShip.SpawnNearProceduralAsteroids, spawnPositionDispersion, ref position, out forward, out up);
          }
        }
        else
        {
          spawnInfo.Planet = (MyPlanet) null;
          MySpaceRespawnComponent.GetSpawnPositionInSpace(spawnInfo, out position, out forward, out up);
        }
      }
      MyMultiplayer.RaiseStaticEvent<Vector3D>((Func<IMyEventOwner, Action<Vector3D>>) (s => new Action<Vector3D>(MySession.SetSpectatorPositionFromServer)), position, new EndpointId(player.Id.SteamId));
      Stack<Action> callbacks = new Stack<Action>();
      List<MyCubeGrid> respawnGrids = new List<MyCubeGrid>();
      if (!MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
        callbacks.Push((Action) (() =>
        {
          if (respawnGrids.Count == 0)
            return;
          MyCubeGrid myCubeGrid = respawnGrids[0];
          MyGps gps = new MyGps()
          {
            ShowOnHud = true,
            Name = new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.GPS_Respawn_Location_Name)).Append(" - ").Append(myCubeGrid.EntityId).ToString(),
            DisplayName = MyTexts.GetString(MySpaceTexts.GPS_Respawn_Location_Name),
            DiscardAt = new TimeSpan?(),
            Coords = (Vector3D) new Vector3(0.0f, 0.0f, 0.0f),
            Description = MyTexts.GetString(MySpaceTexts.GPS_Respawn_Location_Desc),
            AlwaysVisible = true,
            GPSColor = new Color(117, 201, 241),
            IsContainerGPS = true
          };
          MySession.Static.Gpss.SendAddGps(spawnInfo.IdentityId, ref gps, myCubeGrid.EntityId, false);
        }));
      if (!Vector3.IsZero(ref respawnShip.InitialAngularVelocity) || !Vector3.IsZero(ref respawnShip.InitialLinearVelocity))
        callbacks.Push((Action) (() =>
        {
          if (respawnGrids.Count == 0)
            return;
          MyCubeGrid myCubeGrid = respawnGrids[0];
          MatrixD worldMatrix = myCubeGrid.WorldMatrix;
          MyGridPhysics physics1 = myCubeGrid.Physics;
          physics1.LinearVelocity = (Vector3) Vector3D.TransformNormal(respawnShip.InitialLinearVelocity, worldMatrix);
          physics1.AngularVelocity = (Vector3) Vector3D.TransformNormal(respawnShip.InitialAngularVelocity, worldMatrix);
          for (int index = 1; index < respawnGrids.Count; ++index)
          {
            MyGridPhysics physics2 = respawnGrids[index].Physics;
            physics2.AngularVelocity = physics1.AngularVelocity;
            physics2.LinearVelocity = physics1.GetVelocityAtPoint(physics2.CenterOfMassWorld);
          }
        }));
      callbacks.Push((Action) (() =>
      {
        this.PutPlayerInRespawnGrid(player, respawnGrids, botDefinition, modelName, color, respawnShip.SpawnWithDefaultItems);
        if (respawnGrids.Count != 0)
          MySession.SendVicinityInformation(respawnGrids[0].EntityId, new EndpointId(player.Client.SteamUserId));
        this.RespawnDoneEvent.InvokeIfNotNull<ulong>(player.Client.SteamUserId);
      }));
      callbacks.Push((Action) (() =>
      {
        if (respawnGrids.Count <= 0)
          return;
        RespawnShipSpawnedEvent respawnShipSpawned = MyVisualScriptLogicProvider.RespawnShipSpawned;
        if (respawnShipSpawned == null)
          return;
        respawnShipSpawned(respawnGrids[0].EntityId, spawnInfo.IdentityId, respawnShip.Prefab.Id.SubtypeName);
      }));
      MyPrefabManager.Static.SpawnPrefab(respawnGrids, respawnShip.Prefab.Id.SubtypeName, position, forward, up, spawningOptions: (SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SetAuthorship), ownerId: spawnInfo.IdentityId, updateSync: true, callbacks: callbacks);
    }

    private void SpawnInCockpit(
      MyPlayer player,
      MyCockpit cockpit,
      MyBotDefinition botDefinition,
      MatrixD matrix,
      string modelName,
      Color? color,
      bool spawnWithDefaultItems)
    {
      MyCharacter character = MyCharacter.CreateCharacter(matrix, Vector3.Zero, player.Identity.DisplayName, modelName, color.HasValue ? new Vector3?(color.Value.ToVector3()) : new Vector3?(), botDefinition, cockpit: cockpit, identityId: player.Identity.IdentityId, addDefaultItems: spawnWithDefaultItems);
      if (cockpit == null)
      {
        Sync.Players.SetPlayerCharacter(player, character, (MyEntity) null);
      }
      else
      {
        cockpit.AttachPilot(character, false, merged: true);
        character.SetPlayer(player);
        Sync.Players.SetControlledEntity(player.Id, (MyEntity) cockpit);
        string name1 = cockpit.Name;
        long entityId;
        if (string.IsNullOrWhiteSpace(name1))
        {
          entityId = cockpit.EntityId;
          name1 = entityId.ToString();
        }
        string name2 = cockpit.CubeGrid.Name;
        if (string.IsNullOrWhiteSpace(name2))
        {
          entityId = cockpit.CubeGrid.EntityId;
          name2 = entityId.ToString();
        }
        if (MyVisualScriptLogicProvider.PlayerEnteredCockpit != null)
          MyVisualScriptLogicProvider.PlayerEnteredCockpit(name1, player.Identity.IdentityId, name2);
      }
      Sync.Players.RevivePlayer(player);
      if (!MySession.Static.Settings.EnableEconomy)
        return;
      bool flag = false;
      MyFaction faction = (MyFaction) null;
      station = (MyStation) null;
      Vector3D position = player.GetPosition();
      BoundingSphereD area = new BoundingSphereD(position, 150000.0);
      List<MyObjectSeed> list = new List<MyObjectSeed>();
      MyProceduralWorldGenerator.Static.GetAllInSphere<MyStationCellGenerator>(area, list);
      if (list.Count <= 0)
        flag = false;
      else if (list.Count == 1)
      {
        if (list[0].UserData is MyStation station)
        {
          flag = true;
          faction = MySession.Static.Factions.TryGetFactionById(station.FactionId) as MyFaction;
        }
        else
          flag = false;
      }
      else
      {
        double num1 = double.MaxValue;
        int num2 = -1;
        for (int index = 0; index < list.Count; ++index)
        {
          if (list[index].UserData is MyStation userData)
          {
            double num3 = (userData.Position - position).LengthSquared();
            if (num3 < num1 && MySession.Static.Factions.TryGetFactionById(userData.FactionId) is MyFaction factionById && MyIDModule.GetRelationPlayerPlayer(player.Identity.IdentityId, factionById.FounderId) != MyRelationsBetweenPlayers.Enemies)
            {
              num1 = num3;
              station = userData;
              faction = factionById;
              num2 = index;
            }
          }
        }
        if (num2 >= 0)
          flag = true;
      }
      if (!flag)
        return;
      MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(MySession.Static.GetComponent<MySessionComponentEconomy>().GetDatapadDefinitionId());
      if (newObject is MyObjectBuilder_Datapad datapad)
        MySessionComponentEconomy.PrepareDatapad(ref datapad, faction, station);
      if (cockpit.InventoryCount <= 0)
        return;
      MyEntityExtensions.GetInventory(cockpit).AddItems((MyFixedPoint) 1, newObject);
    }

    private void PutPlayerInRespawnGrid(
      MyPlayer player,
      List<MyCubeGrid> respawnGrids,
      MyBotDefinition botDefinition,
      string modelName,
      Color? color,
      bool spawnWithDefaultItems)
    {
      List<MyCockpit> myCockpitList = new List<MyCockpit>();
      foreach (MyCubeGrid respawnGrid in respawnGrids)
      {
        foreach (MyCockpit fatBlock in respawnGrid.GetFatBlocks<MyCockpit>())
        {
          if (fatBlock.IsFunctional)
            myCockpitList.Add(fatBlock);
        }
      }
      if (myCockpitList.Count > 1)
        myCockpitList.Sort((Comparison<MyCockpit>) ((cockpitA, cockpitB) =>
        {
          int num1 = cockpitB.EnableShipControl.CompareTo(cockpitA.EnableShipControl);
          if (num1 != 0)
            return num1;
          int num2 = cockpitB.IsMainCockpit.CompareTo(cockpitA.IsMainCockpit);
          return num2 != 0 ? num2 : 0;
        }));
      MyCockpit cockpit = (MyCockpit) null;
      if (myCockpitList.Count > 0)
        cockpit = myCockpitList[0];
      MatrixD matrix = MatrixD.Identity;
      if (cockpit != null)
      {
        matrix = cockpit.WorldMatrix;
        matrix.Translation = cockpit.WorldMatrix.Translation - Vector3.Up - Vector3.Forward;
      }
      else if (respawnGrids.Count > 0)
        matrix.Translation = respawnGrids[0].PositionComp.WorldAABB.Center + respawnGrids[0].PositionComp.WorldAABB.HalfExtents;
      MySessionComponentTrash.CloseRespawnShip(player);
      foreach (MyCubeGrid respawnGrid in respawnGrids)
      {
        respawnGrid.ChangeGridOwnership(player.Identity.IdentityId, MyOwnershipShareModeEnum.None);
        respawnGrid.IsRespawnGrid = true;
        respawnGrid.m_playedTime = 0;
        player.RespawnShip.Add(respawnGrid.EntityId);
      }
      this.SpawnInCockpit(player, cockpit, botDefinition, matrix, modelName, color, spawnWithDefaultItems);
    }

    public override void AfterRemovePlayer(MyPlayer player)
    {
    }

    private static void SaveRespawnShip(MyPlayer player)
    {
      if (!MySession.Static.Settings.RespawnShipDelete || player.RespawnShip == null || !Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(player.RespawnShip[0], out MyCubeGrid _))
        return;
      ulong sizeInBytes = 0;
      string sessionPath = MySession.Static.CurrentPath;
      Console.WriteLine(sessionPath);
      string fileName = "RS_" + (object) player.Client.SteamUserId + ".sbr";
      Parallel.Start((Action) (() => MyLocalCache.SaveRespawnShip((MyObjectBuilder_CubeGrid) oldHome.GetObjectBuilder(false), sessionPath, fileName, out sizeInBytes)));
    }

    private void SpawnInSuit(
      MyPlayer player,
      MyEntity spawnedBy,
      MyBotDefinition botDefinition,
      string modelName,
      Color color)
    {
      Vector3D position;
      Vector3 forward;
      Vector3 up;
      MySpaceRespawnComponent.GetSpawnPositionInSpace(new MySpaceRespawnComponent.SpawnInfo()
      {
        CollisionRadius = 10f,
        SpawnNearPlayers = false,
        PlanetDeployAltitude = 10f,
        IdentityId = player.Identity.IdentityId
      }, out position, out forward, out up);
      Matrix world = Matrix.CreateWorld((Vector3) position, forward, up);
      MyCharacter character = MyCharacter.CreateCharacter((MatrixD) ref world, Vector3.Zero, player.Identity.DisplayName, modelName, new Vector3?(color.ToVector3()), botDefinition, identityId: player.Identity.IdentityId);
      Sync.Players.SetPlayerCharacter(player, character, spawnedBy);
      Sync.Players.RevivePlayer(player);
    }

    private static void GetSpawnPositionNearPlanet(
      in MySpaceRespawnComponent.SpawnInfo info,
      out Vector3D position,
      out Vector3 forward,
      out Vector3 up)
    {
      bool flag = false;
      position = Vector3D.Zero;
      if (info.SpawnNearPlayers)
      {
        using (ClearToken<Vector3D> friendlyPlayerPositions = MySpaceRespawnComponent.GetFriendlyPlayerPositions(info.IdentityId))
        {
          List<Vector3D> list = friendlyPlayerPositions.List;
          BoundingBoxD worldAabb = info.Planet.PositionComp.WorldAABB;
          for (int index = list.Count - 1; index >= 0; --index)
          {
            if (worldAabb.Contains(list[index]) == ContainmentType.Disjoint)
              list.RemoveAt(index);
          }
          for (int distanceIteration = 0; distanceIteration < 30; distanceIteration += 3)
          {
            if (!flag)
            {
              foreach (Vector3D friendPosition in list)
              {
                Vector3D? positionAbovePlanet = MySpaceRespawnComponent.FindPositionAbovePlanet(friendPosition, in info, true, distanceIteration, distanceIteration + 3, out forward, out up);
                if (positionAbovePlanet.HasValue)
                {
                  position = positionAbovePlanet.Value;
                  flag = true;
                  break;
                }
              }
            }
            else
              break;
          }
        }
      }
      if (!flag)
      {
        MyPlanet planet = info.Planet;
        Vector3D center = planet.PositionComp.WorldVolume.Center;
        for (int index = 0; index < 50; ++index)
        {
          Vector3 vector3 = MyUtils.GetRandomVector3Normalized();
          if ((double) vector3.Dot(MySector.DirectionToSunNormalized) < 0.0 && index < 20)
            vector3 = -vector3;
          position = center + vector3 * planet.AverageRadius;
          Vector3D? positionAbovePlanet = MySpaceRespawnComponent.FindPositionAbovePlanet(position, in info, index < 20, 0, 30, out forward, out up);
          if (positionAbovePlanet.HasValue)
          {
            position = positionAbovePlanet.Value;
            if ((position - center).Dot(MySector.DirectionToSunNormalized) > 0.0)
              return;
          }
        }
      }
      position = MySpaceRespawnComponent.GetShipOrientationForPlanetSpawn(in info, in position, out forward, out up) ?? position;
    }

    private static Vector3D? GetShipOrientationForPlanetSpawn(
      in MySpaceRespawnComponent.SpawnInfo info,
      in Vector3D landingPosition,
      out Vector3 forward,
      out Vector3 up)
    {
      Vector3 vector3 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(landingPosition);
      if (Vector3.IsZero(vector3))
        vector3 = Vector3.Up;
      Vector3D vector3D1 = Vector3D.Normalize((Vector3D) vector3);
      Vector3D vector3D2 = -vector3D1;
      Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace(landingPosition + vector3D2 * (double) info.PlanetDeployAltitude, info.CollisionRadius);
      forward = Vector3.CalculatePerpendicularVector((Vector3) -vector3D1);
      up = (Vector3) -vector3D1;
      return freePlace;
    }

    private static Vector3D? FindPositionAbovePlanet(
      Vector3D friendPosition,
      in MySpaceRespawnComponent.SpawnInfo info,
      bool testFreeZone,
      int distanceIteration,
      int maxDistanceIterations,
      out Vector3 forward,
      out Vector3 up)
    {
      MyPlanet planet = info.Planet;
      Vector3D center = planet.PositionComp.WorldAABB.Center;
      Vector3D axis = Vector3D.Normalize(friendPosition - center);
      float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
      float minimalClearance = (float) (((double) optimalSpawnDistance - (double) optimalSpawnDistance * 0.5) * 0.899999976158142);
      for (int index = 0; index < 20; ++index)
      {
        Vector3D perpendicularVector = MyUtils.GetRandomPerpendicularVector(ref axis);
        float num = optimalSpawnDistance * (MyUtils.GetRandomFloat(0.55f, 1.65f) + (float) distanceIteration * 0.05f);
        Vector3D globalPos = friendPosition + perpendicularVector * (double) num;
        Vector3D landingPosition = planet.GetClosestSurfacePointGlobal(ref globalPos);
        if (!MySpaceRespawnComponent.TestPlanetLandingPosition(in info, planet, landingPosition, testFreeZone, minimalClearance, ref distanceIteration))
        {
          if (distanceIteration > maxDistanceIterations)
            break;
        }
        else
        {
          Vector3D? orientationForPlanetSpawn = MySpaceRespawnComponent.GetShipOrientationForPlanetSpawn(in info, in landingPosition, out forward, out up);
          if (orientationForPlanetSpawn.HasValue)
            return new Vector3D?(orientationForPlanetSpawn.Value);
        }
      }
      forward = new Vector3();
      up = new Vector3();
      return new Vector3D?();
    }

    private static bool TestPlanetLandingPosition(
      in MySpaceRespawnComponent.SpawnInfo info,
      MyPlanet planet,
      Vector3D landingPosition,
      bool testFreeZone,
      float minimalClearance,
      ref int distanceIteration)
    {
      if (testFreeZone && (double) info.MinimalAirDensity > 0.0 && (double) planet.GetAirDensity(landingPosition) < (double) info.MinimalAirDensity)
        return false;
      Vector3D center = planet.PositionComp.WorldAABB.Center;
      Vector3D gravityVector = Vector3D.Normalize(landingPosition - center);
      Vector3D deviationNormal = MyUtils.GetRandomPerpendicularVector(ref gravityVector);
      float collisionRadius = info.CollisionRadius;
      if (!IsTerrainEven())
        return false;
      if (!testFreeZone || MySpaceRespawnComponent.IsZoneFree(new BoundingSphereD(landingPosition, (double) minimalClearance)))
        return true;
      ++distanceIteration;
      return false;

      bool IsTerrainEven()
      {
        Vector3 vector1 = (Vector3) deviationNormal * collisionRadius;
        Vector3 vector3 = Vector3.Cross(vector1, (Vector3) gravityVector);
        MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(landingPosition, new Vector3D((double) collisionRadius * 2.0, (double) Math.Min(10f, collisionRadius * 0.5f), (double) collisionRadius * 2.0), Quaternion.CreateFromForwardUp((Vector3) deviationNormal, (Vector3) gravityVector));
        int num1 = -1;
        for (int index = 0; index < 4; ++index)
        {
          num1 = -num1;
          int num2 = index > 1 ? -1 : 1;
          Vector3D surfacePointGlobal = planet.GetClosestSurfacePointGlobal(landingPosition + vector1 * (float) num1 + vector3 * (float) num2);
          if (!orientedBoundingBoxD.Contains(ref surfacePointGlobal))
            return false;
        }
        return true;
      }
    }

    private static void FindSpawnPositionAbovePlanetInPredefinedArea(
      in MySpaceRespawnComponent.SpawnInfo info,
      Vector2 spawnPositionDispersion,
      ref Vector3D position,
      out Vector3 forward,
      out Vector3 up)
    {
      double optimalSpawnDistance = (double) MySession.Static.Settings.OptimalSpawnDistance;
      float minimalClearance = (float) ((optimalSpawnDistance - optimalSpawnDistance * 0.5) * 0.899999976158142);
      MyPlanet planet = info.Planet;
      Vector3D landingPosition = Vector3D.Zero;
      for (int index = 0; index < 100; ++index)
      {
        landingPosition = position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(spawnPositionDispersion.X, spawnPositionDispersion.Y);
        landingPosition = planet.GetClosestSurfacePointGlobal(ref landingPosition);
        int distanceIteration = 0;
        if (MySpaceRespawnComponent.TestPlanetLandingPosition(in info, planet, landingPosition, index < 80, minimalClearance, ref distanceIteration))
        {
          Vector3D? orientationForPlanetSpawn = MySpaceRespawnComponent.GetShipOrientationForPlanetSpawn(in info, in landingPosition, out forward, out up);
          if (orientationForPlanetSpawn.HasValue)
          {
            position = orientationForPlanetSpawn.Value;
            return;
          }
        }
      }
      position = MySpaceRespawnComponent.GetShipOrientationForPlanetSpawn(in info, in landingPosition, out forward, out up) ?? position;
    }

    private static void FindSpawnPositionInSpaceInPredefinedArea(
      in MySpaceRespawnComponent.SpawnInfo info,
      bool spawnNearAsteroid,
      Vector2 spawnPositionDispersion,
      ref Vector3D position,
      out Vector3 forward,
      out Vector3 up)
    {
      double optimalSpawnDistance = (double) MySession.Static.Settings.OptimalSpawnDistance;
      float minFreeRange = (float) ((optimalSpawnDistance - optimalSpawnDistance * 0.5) * 0.899999976158142);
      BoundingSphereD searchArea = new BoundingSphereD(position, (double) spawnPositionDispersion.Y);
      BoundingSphereD boundingSphereD = new BoundingSphereD(position, (double) spawnPositionDispersion.X);
      if (spawnNearAsteroid)
      {
        Vector3D? locationCloseToAsteroid = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(searchArea, new BoundingSphereD?(boundingSphereD), false, false, info.CollisionRadius, minFreeRange, out forward, out up);
        if (locationCloseToAsteroid.HasValue)
        {
          position = locationCloseToAsteroid.Value;
          return;
        }
      }
      Vector3D vector3D = Vector3D.Zero;
      for (int index = 0; index < 100; ++index)
      {
        vector3D = position + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(spawnPositionDispersion.X, spawnPositionDispersion.Y);
        MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(vector3D);
        // ISSUE: explicit non-virtual call
        if ((closestPlanet != null ? (__nonvirtual (closestPlanet.PositionComp).WorldAABB.Contains(vector3D) == ContainmentType.Contains ? 1 : 0) : 0) == 0)
        {
          vector3D = Sandbox.Game.Entities.MyEntities.FindFreePlace(vector3D, info.CollisionRadius) ?? vector3D;
          if (MySpaceRespawnComponent.IsZoneFree(new BoundingSphereD(vector3D, (double) minFreeRange)))
            break;
        }
      }
      position = vector3D;
      forward = MyUtils.GetRandomVector3Normalized();
      up = Vector3.CalculatePerpendicularVector(forward);
    }

    private static void GetSpawnPositionInSpace(
      MySpaceRespawnComponent.SpawnInfo info,
      out Vector3D position,
      out Vector3 forward,
      out Vector3 up)
    {
      float optimalSpawnDistance = MySession.Static.Settings.OptimalSpawnDistance;
      float minFreeRange = (float) (((double) optimalSpawnDistance - (double) optimalSpawnDistance * 0.5) * 0.899999976158142);
      float collisionRadius = info.CollisionRadius;
      if (info.SpawnNearPlayers)
      {
        using (ClearToken<Vector3D> friendlyPlayerPositions = MySpaceRespawnComponent.GetFriendlyPlayerPositions(info.IdentityId))
        {
          foreach (Vector3D vector3D in friendlyPlayerPositions.List)
          {
            Vector3D friendPosition = vector3D;
            if (!MyPlanets.Static.GetPlanetAABBs().Any<BoundingBoxD>((Func<BoundingBoxD, bool>) (x => (uint) x.Contains(friendPosition) > 0U)))
            {
              Vector3D? locationCloseToAsteroid = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(new BoundingSphereD(friendPosition + MyUtils.GetRandomVector3Normalized() * (optimalSpawnDistance * MyUtils.GetRandomFloat(0.5f, 1.5f)), 100000.0), new BoundingSphereD?(new BoundingSphereD(friendPosition, (double) minFreeRange)), false, true, collisionRadius, minFreeRange, out forward, out up);
              if (locationCloseToAsteroid.HasValue)
              {
                position = locationCloseToAsteroid.Value;
                return;
              }
            }
          }
        }
      }
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      BoundingBoxD box = new BoundingBoxD(new Vector3D(-25000.0), new Vector3D(25000.0));
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity.Parent == null)
        {
          BoundingBoxD worldAabb = entity.PositionComp.WorldAABB;
          if (entity is MyPlanet)
          {
            if (worldAabb.Contains(Vector3D.Zero) != ContainmentType.Disjoint)
              invalid.Include(worldAabb);
          }
          else
            box.Include(worldAabb);
        }
      }
      box.Include(invalid.GetInflated(25000.0));
      if (Sandbox.Game.Entities.MyEntities.IsWorldLimited())
      {
        Vector3D max = new Vector3D((double) Sandbox.Game.Entities.MyEntities.WorldSafeHalfExtent());
        box = new BoundingBoxD(Vector3D.Clamp(box.Min, -max, Vector3D.Zero), Vector3D.Clamp(box.Max, Vector3D.Zero, max));
      }
      Vector3D vector3D1 = Vector3D.Zero;
      for (int index = 0; index < 50; ++index)
      {
        vector3D1 = MyUtils.GetRandomPosition(ref box);
        if (invalid.Contains(vector3D1) == ContainmentType.Disjoint)
          break;
      }
      Vector3D? locationCloseToAsteroid1 = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(new BoundingSphereD(vector3D1, 100000.0), new BoundingSphereD?(new BoundingSphereD(invalid.Center, Math.Max(0.0, invalid.HalfExtents.Min()))), true, true, collisionRadius, minFreeRange, out forward, out up);
      if (locationCloseToAsteroid1.HasValue)
        position = locationCloseToAsteroid1.Value;
      else if (MyGamePruningStructure.GetClosestPlanet(vector3D1) != null)
      {
        MySpaceRespawnComponent.GetSpawnPositionNearPlanet(in info, out position, out forward, out up);
      }
      else
      {
        forward = MyUtils.GetRandomVector3Normalized();
        up = Vector3.CalculatePerpendicularVector(forward);
        position = Sandbox.Game.Entities.MyEntities.FindFreePlace(vector3D1, collisionRadius) ?? vector3D1;
      }
    }

    private static bool IsZoneFree(BoundingSphereD safeZone)
    {
      using (ClearToken<MyEntity> clearToken = Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(ref safeZone).GetClearToken<MyEntity>())
      {
        foreach (MyEntity myEntity in clearToken.List)
        {
          if (myEntity is MyCubeGrid)
            return false;
        }
      }
      return true;
    }

    private static ClearToken<Vector3D> GetFriendlyPlayerPositions(long identityId)
    {
      MySpaceRespawnComponent.m_playerPositionsCache.AssertEmpty<Vector3D>();
      if (MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
      {
        if (!MySession.Static.Gpss.ExistsForPlayer(identityId))
          return new List<Vector3D>().GetClearToken<Vector3D>();
        List<Vector3D> list = MySession.Static.Gpss[identityId].Values.Select<MyGps, Vector3D>((Func<MyGps, Vector3D>) (x => x.Coords)).ToList<Vector3D>();
        list.ShuffleList<Vector3D>();
        return list.GetClearToken<Vector3D>();
      }
      int offset = 0;
      foreach (MyIdentity allIdentity in (IEnumerable<MyIdentity>) MySession.Static.Players.GetAllIdentities())
      {
        MyCharacter character = allIdentity.Character;
        if (character != null && !character.IsDead && !character.MarkedForClose)
        {
          MyIDModule component;
          ((IMyComponentOwner<MyIDModule>) character).GetComponent(out component);
          MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(component.Owner, identityId, MyOwnershipShareModeEnum.Faction, MyRelationsBetweenPlayerAndBlock.Neutral, MyRelationsBetweenFactions.Neutral);
          Vector3D position = character.PositionComp.GetPosition();
          switch (relationPlayerBlock)
          {
            case MyRelationsBetweenPlayerAndBlock.FactionShare:
              MySpaceRespawnComponent.m_playerPositionsCache.Insert(offset++, position);
              continue;
            case MyRelationsBetweenPlayerAndBlock.Neutral:
              MySpaceRespawnComponent.m_playerPositionsCache.Add(position);
              continue;
            default:
              continue;
          }
        }
      }
      MySpaceRespawnComponent.m_playerPositionsCache.ShuffleList<Vector3D>(count: new int?(offset));
      MySpaceRespawnComponent.m_playerPositionsCache.ShuffleList<Vector3D>(offset);
      return MySpaceRespawnComponent.m_playerPositionsCache.GetClearToken<Vector3D>();
    }

    public override MyIdentity CreateNewIdentity(
      string identityName,
      MyPlayer.PlayerId playerId,
      string modelName,
      bool initialPlayer = false)
    {
      return Sync.Players.CreateNewIdentity(identityName, modelName, initialPlayer: initialPlayer);
    }

    public override void SetupCharacterDefault(MyPlayer player, MyWorldGenerator.Args args)
    {
      string firstRespawnShip = MyDefinitionManager.Static.GetFirstRespawnShip();
      this.SpawnAtShip(player, firstRespawnShip, (MyBotDefinition) null, (string) null, new Color?());
    }

    public override bool IsInRespawnScreen() => MyGuiScreenMedicals.Static != null && MyGuiScreenMedicals.Static.State == MyGuiScreenState.OPENED;

    public override void CloseRespawnScreen() => MyGuiScreenMedicals.Close();

    public override void CloseRespawnScreenNow()
    {
      if (MyGuiScreenMedicals.Static == null)
        return;
      MyGuiScreenMedicals.Static.CloseScreenNow();
    }

    public override void SetNoRespawnText(StringBuilder text, int timeSec) => MyGuiScreenMedicals.SetNoRespawnText(text, timeSec);

    public override void SetupCharacterFromStarts(
      MyPlayer player,
      MyWorldGeneratorStartingStateBase[] playerStarts,
      MyWorldGenerator.Args args)
    {
      playerStarts[MyUtils.GetRandomInt(playerStarts.Length)].SetupCharacter(args);
    }

    [Serializable]
    private struct RespawnCooldownEntry
    {
      public int ControllerId;
      public string ShipId;
      public int RelativeRespawnTime;

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003ERespawnCooldownEntry\u003C\u003EControllerId\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.RespawnCooldownEntry, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          in int value)
        {
          owner.ControllerId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          out int value)
        {
          value = owner.ControllerId;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003ERespawnCooldownEntry\u003C\u003EShipId\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.RespawnCooldownEntry, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          in string value)
        {
          owner.ShipId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          out string value)
        {
          value = owner.ShipId;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003ERespawnCooldownEntry\u003C\u003ERelativeRespawnTime\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.RespawnCooldownEntry, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          in int value)
        {
          owner.RelativeRespawnTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.RespawnCooldownEntry owner,
          out int value)
        {
          value = owner.RelativeRespawnTime;
        }
      }
    }

    private struct RespawnKey : IEquatable<MySpaceRespawnComponent.RespawnKey>
    {
      public MyPlayer.PlayerId ControllerId;
      public string RespawnShipId;

      public bool Equals(MySpaceRespawnComponent.RespawnKey other) => this.ControllerId == other.ControllerId && this.RespawnShipId == other.RespawnShipId;

      public override int GetHashCode() => this.ControllerId.GetHashCode() ^ (this.RespawnShipId == null ? 0 : this.RespawnShipId.GetHashCode());
    }

    [Serializable]
    public class MyRespawnPointInfo
    {
      public long MedicalRoomId;
      public long MedicalRoomGridId;
      public string MedicalRoomName;
      public float OxygenLevel;
      public long OwnerId;
      public Vector3D PrefferedCameraPosition;
      public Vector3D MedicalRoomPosition;
      public Vector3D MedicalRoomUp;
      public Vector3 MedicalRoomVelocity;

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomId\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in long value)
        {
          owner.MedicalRoomId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out long value)
        {
          value = owner.MedicalRoomId;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomGridId\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in long value)
        {
          owner.MedicalRoomGridId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out long value)
        {
          value = owner.MedicalRoomGridId;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomName\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in string value)
        {
          owner.MedicalRoomName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out string value)
        {
          value = owner.MedicalRoomName;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EOxygenLevel\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in float value)
        {
          owner.OxygenLevel = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out float value)
        {
          value = owner.OxygenLevel;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EOwnerId\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in long value)
        {
          owner.OwnerId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out long value)
        {
          value = owner.OwnerId;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EPrefferedCameraPosition\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in Vector3D value)
        {
          owner.PrefferedCameraPosition = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out Vector3D value)
        {
          value = owner.PrefferedCameraPosition;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomPosition\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in Vector3D value)
        {
          owner.MedicalRoomPosition = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out Vector3D value)
        {
          value = owner.MedicalRoomPosition;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomUp\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in Vector3D value)
        {
          owner.MedicalRoomUp = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out Vector3D value)
        {
          value = owner.MedicalRoomUp;
        }
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003C\u003EMedicalRoomVelocity\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MyRespawnPointInfo, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          in Vector3 value)
        {
          owner.MedicalRoomVelocity = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySpaceRespawnComponent.MyRespawnPointInfo owner,
          out Vector3 value)
        {
          value = owner.MedicalRoomVelocity;
        }
      }
    }

    private struct SpawnInfo
    {
      public long IdentityId;
      public MyPlanet Planet;
      public float CollisionRadius;
      public float PlanetDeployAltitude;
      public float MinimalAirDensity;
      public bool SpawnNearPlayers;
    }

    [Serializable]
    private struct MOTDData
    {
      public string Url;
      public string Message;

      public bool HasMessage => !string.IsNullOrEmpty(this.Message);

      public bool HasUrl => string.IsNullOrEmpty(this.Url) && MyGuiSandbox.IsUrlValid(this.Url);

      public bool HasAnything() => this.HasMessage || this.HasUrl;

      public MOTDData(string url, string message)
      {
        this.Url = url;
        this.Message = message;
      }

      public StringBuilder GetMessage()
      {
        StringBuilder stringBuilder = new StringBuilder(this.Message);
        if (MySession.Static.LocalHumanPlayer != null)
          stringBuilder.Replace(MyPerGameSettings.MotDCurrentPlayerVariable, MySession.Static.LocalHumanPlayer.DisplayName);
        return stringBuilder;
      }

      public static MySpaceRespawnComponent.MOTDData Construct()
      {
        if (!Sync.IsDedicated)
          return new MySpaceRespawnComponent.MOTDData(string.Empty, MySession.Static.Description ?? string.Empty);
        string messageOfTheDayUrl = MySandboxGame.ConfigDedicated.MessageOfTheDayUrl;
        string messageOfTheDay = MySandboxGame.ConfigDedicated.MessageOfTheDay;
        if (!string.IsNullOrEmpty(messageOfTheDay))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(messageOfTheDay);
          stringBuilder.Replace(MyPerGameSettings.MotDServerNameVariable, MySandboxGame.ConfigDedicated.ServerName);
          stringBuilder.Replace(MyPerGameSettings.MotDWorldNameVariable, MySandboxGame.ConfigDedicated.WorldName);
          stringBuilder.Replace(MyPerGameSettings.MotDServerDescriptionVariable, MySandboxGame.ConfigDedicated.ServerDescription);
          stringBuilder.Replace(MyPerGameSettings.MotDPlayerCountVariable, Sync.Players.GetOnlinePlayerCount().ToString());
          messageOfTheDay = stringBuilder.ToString();
        }
        string message = messageOfTheDay;
        return new MySpaceRespawnComponent.MOTDData(messageOfTheDayUrl, message);
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMOTDData\u003C\u003EUrl\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MOTDData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySpaceRespawnComponent.MOTDData owner, in string value) => owner.Url = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySpaceRespawnComponent.MOTDData owner, out string value) => value = owner.Url;
      }

      protected class SpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMOTDData\u003C\u003EMessage\u003C\u003EAccessor : IMemberAccessor<MySpaceRespawnComponent.MOTDData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MySpaceRespawnComponent.MOTDData owner, in string value) => owner.Message = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MySpaceRespawnComponent.MOTDData owner, out string value) => value = owner.Message;
      }
    }

    protected sealed class OnSyncCooldownRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.OnSyncCooldownRequest();
      }
    }

    protected sealed class OnSyncCooldownResponse\u003C\u003ESystem_Collections_Generic_List`1\u003CSpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003ERespawnCooldownEntry\u003E : ICallSite<IMyEventOwner, List<MySpaceRespawnComponent.RespawnCooldownEntry>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MySpaceRespawnComponent.RespawnCooldownEntry> entries,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.OnSyncCooldownResponse(entries);
      }
    }

    protected sealed class RespawnRequest_Implementation\u003C\u003ESystem_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong steamPlayerId,
        in int serialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.RespawnRequest_Implementation(steamPlayerId, serialId);
      }
    }

    protected sealed class RequestRespawnAtSpawnPoint\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long spawnPointId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.RequestRespawnAtSpawnPoint(spawnPointId);
      }
    }

    protected sealed class ShowMedicalScreen_Implementation\u003C\u003ESpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMOTDData\u0023System_Int64 : ICallSite<IMyEventOwner, MySpaceRespawnComponent.MOTDData, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MySpaceRespawnComponent.MOTDData motd,
        in long restrictedRespawn,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.ShowMedicalScreen_Implementation(motd, restrictedRespawn);
      }
    }

    protected sealed class ShowMotD\u003C\u003ESpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMOTDData : ICallSite<IMyEventOwner, MySpaceRespawnComponent.MOTDData, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MySpaceRespawnComponent.MOTDData motd,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceRespawnComponent.ShowMotD(motd);
      }
    }
  }
}
