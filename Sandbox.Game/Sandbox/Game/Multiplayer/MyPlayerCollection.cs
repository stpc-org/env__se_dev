// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyPlayerCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public class MyPlayerCollection : MyIdentity.Friend, IMyPlayerCollection
  {
    private readonly ConcurrentDictionary<MyPlayer.PlayerId, MyPlayer> m_players = new ConcurrentDictionary<MyPlayer.PlayerId, MyPlayer>((IEqualityComparer<MyPlayer.PlayerId>) MyPlayer.PlayerId.Comparer);
    private List<MyPlayer> m_tmpRemovedPlayers = new List<MyPlayer>();
    private CachingDictionary<long, MyPlayer.PlayerId> m_controlledEntities = new CachingDictionary<long, MyPlayer.PlayerId>();
    private Dictionary<long, MyPlayer.PlayerId> m_previousControlledEntities = new Dictionary<long, MyPlayer.PlayerId>();
    private ConcurrentDictionary<long, MyIdentity> m_allIdentities = new ConcurrentDictionary<long, MyIdentity>();
    private readonly ConcurrentDictionary<MyPlayer.PlayerId, long> m_playerIdentityIds = new ConcurrentDictionary<MyPlayer.PlayerId, long>((IEqualityComparer<MyPlayer.PlayerId>) MyPlayer.PlayerId.Comparer);
    private readonly Dictionary<long, MyPlayer.PlayerId> m_identityPlayerIds = new Dictionary<long, MyPlayer.PlayerId>();
    private HashSet<long> m_npcIdentities = new HashSet<long>();
    private List<EndpointId> m_tmpPlayersLinkedToBlockLimit = new List<EndpointId>();
    private Action<MyEntity> m_entityClosingHandler;
    private static Dictionary<long, MyPlayer.PlayerId> m_controlledEntitiesClientCache;
    private static long m_lastCreatedCharacter;

    public MyRespawnComponentBase RespawnComponent { get; set; }

    public static event Action<ulong> OnRespawnRequestFailureEvent;

    public event Action<MyPlayer.PlayerId> NewPlayerRequestSucceeded;

    public event Action<int> NewPlayerRequestFailed;

    public event Action<int> LocalPlayerRemoved;

    public event Action<int> LocalPlayerLoaded;

    public event Action<string, Color> LocalRespawnRequested;

    public event Action<MyPlayer.PlayerId> PlayerRemoved;

    public event PlayerRequestDelegate PlayerRequesting;

    public event Action<bool, MyPlayer.PlayerId> PlayersChanged;

    public event Action<long> PlayerCharacterDied;

    public event Action IdentitiesChanged;

    public DictionaryReader<long, MyPlayer.PlayerId> ControlledEntities => this.m_controlledEntities.Reader;

    public MyPlayerCollection()
    {
      this.m_entityClosingHandler = new Action<MyEntity>(this.EntityClosing);
      MyPlayerCollection.m_controlledEntitiesClientCache = !Sync.IsServer ? new Dictionary<long, MyPlayer.PlayerId>() : (Dictionary<long, MyPlayer.PlayerId>) null;
    }

    public void LoadIdentities(
      MyObjectBuilder_Checkpoint checkpoint,
      MyPlayer.PlayerId? savingPlayerId = null)
    {
      if (checkpoint.NonPlayerIdentities != null)
        this.LoadNpcIdentities(checkpoint.NonPlayerIdentities);
      if (checkpoint.AllPlayers.Count != 0)
        this.LoadIdentitiesObsolete(checkpoint.AllPlayers, savingPlayerId);
      else
        this.LoadIdentities(checkpoint.Identities);
    }

    private void LoadNpcIdentities(List<long> list)
    {
      foreach (long identityId in list)
        this.MarkIdentityAsNPC(identityId);
    }

    public List<MyObjectBuilder_Identity> SaveIdentities()
    {
      List<MyObjectBuilder_Identity> objectBuilderIdentityList = new List<MyObjectBuilder_Identity>();
      foreach (KeyValuePair<long, MyIdentity> allIdentity in this.m_allIdentities)
      {
        MyPlayer.PlayerId result;
        if (MySession.Static != null && MySession.Static.Players.TryGetPlayerId(allIdentity.Key, out result) && MySession.Static.Players.GetPlayerById(result) != null)
          allIdentity.Value.LastLogoutTime = DateTime.Now;
        objectBuilderIdentityList.Add(allIdentity.Value.GetObjectBuilder());
      }
      return objectBuilderIdentityList;
    }

    public List<long> SaveNpcIdentities()
    {
      List<long> longList = new List<long>();
      foreach (long npcIdentity in this.m_npcIdentities)
        longList.Add(npcIdentity);
      return longList;
    }

    public void LoadControlledEntities(
      SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> controlledEntities,
      long controlledObject,
      MyPlayer.PlayerId? savingPlayerId = null)
    {
      if (controlledEntities == null)
        return;
      foreach (KeyValuePair<long, MyObjectBuilder_Checkpoint.PlayerId> keyValuePair in controlledEntities.Dictionary)
      {
        MyPlayer.PlayerId id = new MyPlayer.PlayerId(keyValuePair.Value.GetClientId(), keyValuePair.Value.SerialId);
        if (savingPlayerId.HasValue && (long) id.SteamId == (long) savingPlayerId.Value.SteamId)
          id = new MyPlayer.PlayerId(Sync.MyId, id.SerialId);
        MyPlayer playerById = Sync.Players.GetPlayerById(id);
        if (!Sync.IsServer)
          MyPlayerCollection.m_controlledEntitiesClientCache[keyValuePair.Key] = id;
        if (playerById != null)
          this.TryTakeControl(playerById, keyValuePair.Key);
      }
    }

    private void TryTakeControl(MyPlayer player, long controlledEntityId)
    {
      MyEntity entity;
      MyEntities.TryGetEntityById(controlledEntityId, out entity);
      if (entity != null)
      {
        if (entity is Sandbox.Game.Entities.IMyControllableEntity)
        {
          player.Controller.TakeControl(entity as Sandbox.Game.Entities.IMyControllableEntity);
          if (!(entity is MyCharacter character))
          {
            if (entity is MyShipController)
              character = (entity as MyShipController).Pilot;
            else if (entity is MyLargeTurretBase)
              character = (entity as MyLargeTurretBase).Pilot;
          }
          if (character != null)
          {
            player.Identity.ChangeCharacter(character);
            character.SetPlayer(player, false);
          }
        }
        else
        {
          this.m_controlledEntities.Add(controlledEntityId, player.Id, true);
          if (Sync.IsServer)
            MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), player.Id.SteamId, player.Id.SerialId, controlledEntityId, true);
        }
        if (player.CachedControllerId == null)
          return;
        player.CachedControllerId.Remove(controlledEntityId);
        if (player.CachedControllerId.Count != 0)
          return;
        player.CachedControllerId = (List<long>) null;
      }
      else
      {
        if (player.CachedControllerId == null)
          player.CachedControllerId = new List<long>();
        player.CachedControllerId.Add(controlledEntityId);
      }
    }

    public SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> SerializeControlledEntities()
    {
      SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> serializableDictionary = new SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId>();
      foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in this.m_controlledEntities)
      {
        MyObjectBuilder_Checkpoint.PlayerId playerId = new MyObjectBuilder_Checkpoint.PlayerId(controlledEntity.Value.SteamId, controlledEntity.Value.SerialId);
        serializableDictionary.Dictionary.Add(controlledEntity.Key, playerId);
      }
      return serializableDictionary;
    }

    private void ChangeDisplayNameOfPlayerAndIdentity(MyObjectBuilder_Player playerOb, string name)
    {
      playerOb.DisplayName = MyGameService.UserName;
      this.TryGetIdentity(playerOb.IdentityId)?.SetDisplayName(MyGameService.UserName);
    }

    public void LoadPlayers(
      List<MyPlayerCollection.AllPlayerData> allPlayersData)
    {
      if (allPlayersData == null)
        return;
      foreach (MyPlayerCollection.AllPlayerData allPlayerData in allPlayersData)
      {
        MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(allPlayerData.SteamId, allPlayerData.SerialId);
        this.LoadPlayerInternal(ref playerId, allPlayerData.Player);
      }
    }

    public void LoadConnectedPlayers(
      MyObjectBuilder_Checkpoint checkpoint,
      MyPlayer.PlayerId? savingPlayerId = null)
    {
      if (checkpoint.AllPlayers != null && checkpoint.AllPlayers.Count != 0)
      {
        foreach (MyObjectBuilder_Checkpoint.PlayerItem allPlayer in checkpoint.AllPlayers)
        {
          long playerId1 = allPlayer.PlayerId;
          MyObjectBuilder_Player playerOb = new MyObjectBuilder_Player()
          {
            Connected = true,
            DisplayName = allPlayer.Name,
            IdentityId = playerId1
          };
          MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(allPlayer.SteamId, 0);
          if (savingPlayerId.HasValue)
          {
            MyPlayer.PlayerId playerId3 = playerId2;
            MyPlayer.PlayerId? nullable = savingPlayerId;
            if ((nullable.HasValue ? (playerId3 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            {
              playerId2 = new MyPlayer.PlayerId(Sync.MyId);
              this.ChangeDisplayNameOfPlayerAndIdentity(playerOb, MyGameService.UserName);
            }
          }
          this.LoadPlayerInternal(ref playerId2, playerOb, true);
        }
      }
      else if (checkpoint.ConnectedPlayers != null && checkpoint.ConnectedPlayers.Dictionary.Count != 0)
      {
        foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.ConnectedPlayers.Dictionary)
        {
          MyPlayer.PlayerId playerId1 = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
          if (savingPlayerId.HasValue)
          {
            MyPlayer.PlayerId playerId2 = playerId1;
            MyPlayer.PlayerId? nullable = savingPlayerId;
            if ((nullable.HasValue ? (playerId2 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            {
              playerId1 = new MyPlayer.PlayerId(Sync.MyId);
              this.ChangeDisplayNameOfPlayerAndIdentity(keyValuePair.Value, MyGameService.UserName);
            }
          }
          keyValuePair.Value.Connected = true;
          this.LoadPlayerInternal(ref playerId1, keyValuePair.Value);
        }
        foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, long> keyValuePair in checkpoint.DisconnectedPlayers.Dictionary)
        {
          MyPlayer.PlayerId playerId1 = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
          MyObjectBuilder_Player playerOb = new MyObjectBuilder_Player()
          {
            Connected = false,
            IdentityId = keyValuePair.Value,
            DisplayName = (string) null
          };
          if (savingPlayerId.HasValue)
          {
            MyPlayer.PlayerId playerId2 = playerId1;
            MyPlayer.PlayerId? nullable = savingPlayerId;
            if ((nullable.HasValue ? (playerId2 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            {
              playerId1 = new MyPlayer.PlayerId(Sync.MyId);
              this.ChangeDisplayNameOfPlayerAndIdentity(playerOb, MyGameService.UserName);
            }
          }
          this.LoadPlayerInternal(ref playerId1, playerOb);
        }
      }
      else if (checkpoint.AllPlayersData != null)
      {
        foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
        {
          MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
          if (savingPlayerId.HasValue && (long) playerId.SteamId == (long) savingPlayerId.Value.SteamId)
          {
            playerId = new MyPlayer.PlayerId(Sync.MyId, playerId.SerialId);
            if (playerId.SerialId == 0)
              this.ChangeDisplayNameOfPlayerAndIdentity(keyValuePair.Value, MyGameService.UserName);
          }
          this.LoadPlayerInternal(ref playerId, keyValuePair.Value);
          MyPlayer myPlayer = (MyPlayer) null;
          if (this.m_players.TryGetValue(playerId, out myPlayer))
          {
            List<Vector3> newColors = (List<Vector3>) null;
            if (checkpoint.AllPlayersColors != null && checkpoint.AllPlayersColors.Dictionary.TryGetValue(keyValuePair.Key, out newColors))
              myPlayer.SetBuildColorSlots(newColors);
            else if (checkpoint.CharacterToolbar != null && checkpoint.CharacterToolbar.ColorMaskHSVList != null && checkpoint.CharacterToolbar.ColorMaskHSVList.Count > 0)
              myPlayer.SetBuildColorSlots(checkpoint.CharacterToolbar.ColorMaskHSVList);
          }
        }
      }
      if (MyCubeBuilder.AllPlayersColors == null || checkpoint.AllPlayersColors == null)
        return;
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>> keyValuePair in checkpoint.AllPlayersColors.Dictionary)
      {
        MyPlayer.PlayerId key = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
        if (!MyCubeBuilder.AllPlayersColors.ContainsKey(key))
          MyCubeBuilder.AllPlayersColors.Add(key, keyValuePair.Value);
      }
    }

    public void SavePlayers(
      MyObjectBuilder_Checkpoint checkpoint,
      Dictionary<ulong, AdminSettingsEnum> remoteAdminSettings,
      Dictionary<ulong, MyPromoteLevel> promoteLevels,
      HashSet<ulong> creativeTools)
    {
      checkpoint.ConnectedPlayers = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>();
      checkpoint.DisconnectedPlayers = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long>();
      checkpoint.AllPlayersData = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>();
      checkpoint.AllPlayersColors = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>>();
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) this.m_players.Values)
      {
        MyObjectBuilder_Checkpoint.PlayerId key = new MyObjectBuilder_Checkpoint.PlayerId(myPlayer.Id.SteamId, myPlayer.Id.SerialId);
        MyObjectBuilder_Player objectBuilder = myPlayer.GetObjectBuilder();
        checkpoint.AllPlayersData.Dictionary.Add(key, objectBuilder);
      }
      foreach (KeyValuePair<MyPlayer.PlayerId, long> playerIdentityId in this.m_playerIdentityIds)
      {
        if (!this.m_players.ContainsKey(playerIdentityId.Key))
        {
          MyObjectBuilder_Checkpoint.PlayerId key = new MyObjectBuilder_Checkpoint.PlayerId(playerIdentityId.Key.SteamId, playerIdentityId.Key.SerialId);
          MyIdentity identity = this.TryGetIdentity(playerIdentityId.Value);
          MyObjectBuilder_Player objectBuilderPlayer = new MyObjectBuilder_Player()
          {
            DisplayName = identity?.DisplayName,
            IdentityId = playerIdentityId.Value,
            Connected = false
          };
          if (MyCubeBuilder.AllPlayersColors != null)
            MyCubeBuilder.AllPlayersColors.TryGetValue(playerIdentityId.Key, out objectBuilderPlayer.BuildColorSlots);
          checkpoint.AllPlayersData.Dictionary.Add(key, objectBuilderPlayer);
        }
      }
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
      {
        ulong clientId = keyValuePair.Key.GetClientId();
        AdminSettingsEnum adminSettingsEnum;
        if (remoteAdminSettings.TryGetValue(clientId, out adminSettingsEnum))
          keyValuePair.Value.RemoteAdminSettings = (int) adminSettingsEnum;
        MyPromoteLevel myPromoteLevel;
        if (promoteLevels.TryGetValue(clientId, out myPromoteLevel))
          keyValuePair.Value.PromoteLevel = myPromoteLevel;
        keyValuePair.Value.CreativeToolsEnabled = creativeTools.Contains(clientId);
      }
      if (MyCubeBuilder.AllPlayersColors == null)
        return;
      foreach (KeyValuePair<MyPlayer.PlayerId, List<Vector3>> allPlayersColor in MyCubeBuilder.AllPlayersColors)
      {
        if (!this.m_players.ContainsKey(allPlayersColor.Key) && !this.m_playerIdentityIds.ContainsKey(allPlayersColor.Key))
        {
          MyObjectBuilder_Checkpoint.PlayerId key = new MyObjectBuilder_Checkpoint.PlayerId(allPlayersColor.Key.SteamId, allPlayersColor.Key.SerialId);
          checkpoint.AllPlayersColors.Dictionary.Add(key, allPlayersColor.Value);
        }
      }
    }

    public List<MyPlayerCollection.AllPlayerData> SavePlayers(
      Dictionary<ulong, AdminSettingsEnum> remoteAdminSettings,
      Dictionary<ulong, MyPromoteLevel> promoteLevels,
      HashSet<ulong> creativeTools)
    {
      List<MyPlayerCollection.AllPlayerData> allPlayerDataList = new List<MyPlayerCollection.AllPlayerData>();
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) this.m_players.Values)
      {
        MyPlayerCollection.AllPlayerData allPlayerData = new MyPlayerCollection.AllPlayerData()
        {
          SteamId = myPlayer.Id.SteamId,
          SerialId = myPlayer.Id.SerialId,
          Player = myPlayer.GetObjectBuilder()
        };
        AdminSettingsEnum adminSettingsEnum;
        if (remoteAdminSettings.TryGetValue(myPlayer.Id.SteamId, out adminSettingsEnum))
          allPlayerData.Player.RemoteAdminSettings = (int) adminSettingsEnum;
        MyPromoteLevel myPromoteLevel;
        if (promoteLevels.TryGetValue(myPlayer.Id.SteamId, out myPromoteLevel))
          allPlayerData.Player.PromoteLevel = myPromoteLevel;
        allPlayerData.Player.CreativeToolsEnabled = creativeTools.Contains(myPlayer.Id.SteamId);
        allPlayerDataList.Add(allPlayerData);
      }
      return allPlayerDataList;
    }

    private void RemovePlayerFromDictionary(MyPlayer.PlayerId playerId)
    {
      if (this.m_players.ContainsKey(playerId))
      {
        if (Sync.IsServer && MyVisualScriptLogicProvider.PlayerDisconnected != null)
          MyVisualScriptLogicProvider.PlayerDisconnected(this.m_players[playerId].Identity.IdentityId);
        this.m_players[playerId].Identity.LastLogoutTime = DateTime.Now;
      }
      this.m_players.Remove<MyPlayer.PlayerId, MyPlayer>(playerId);
      this.OnPlayersChanged(false, playerId);
    }

    private void AddPlayer(MyPlayer.PlayerId playerId, MyPlayer newPlayer)
    {
      if (Sync.IsServer && MyVisualScriptLogicProvider.PlayerConnected != null)
        MyVisualScriptLogicProvider.PlayerConnected(newPlayer.Identity.IdentityId);
      newPlayer.Identity.LastLoginTime = DateTime.Now;
      newPlayer.Identity.BlockLimits.SetAllDirty();
      this.m_players.TryAdd(playerId, newPlayer);
      this.OnPlayersChanged(true, playerId);
    }

    public void RegisterEvents() => Sync.Clients.ClientRemoved += new Action<ulong>(this.Multiplayer_ClientRemoved);

    public void UnregisterEvents()
    {
      if (Sync.Clients == null)
        return;
      Sync.Clients.ClientRemoved -= new Action<ulong>(this.Multiplayer_ClientRemoved);
    }

    private void OnPlayersChanged(bool added, MyPlayer.PlayerId playerId)
    {
      Action<bool, MyPlayer.PlayerId> playersChanged = this.PlayersChanged;
      if (playersChanged == null)
        return;
      playersChanged(added, playerId);
    }

    public void ClearPlayers()
    {
      this.m_players.Clear();
      this.m_controlledEntities.Clear();
      this.m_playerIdentityIds.Clear();
      this.m_identityPlayerIds.Clear();
    }

    [Event(null, 604)]
    [Reliable]
    [Broadcast]
    private static void OnControlChangedSuccess(
      ulong clientSteamId,
      int playerSerialId,
      long entityId,
      bool justUpdateClientCache)
    {
      MyPlayer.PlayerId id = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
      MyEntity entity = (MyEntity) null;
      if (MyPlayerCollection.m_controlledEntitiesClientCache != null)
      {
        if (id.IsValid)
          MyPlayerCollection.m_controlledEntitiesClientCache[entityId] = id;
        else if (MyPlayerCollection.m_controlledEntitiesClientCache.ContainsKey(entityId))
          MyPlayerCollection.m_controlledEntitiesClientCache.Remove(entityId);
      }
      if (justUpdateClientCache || !MyEntities.TryGetEntityById(entityId, out entity))
        return;
      Sync.Players.SetControlledEntityInternal(id, entity);
    }

    public static void UpdateControl(MyEntity entity)
    {
      MyPlayer.PlayerId id;
      MyPlayer.PlayerId playerId;
      if (MyPlayerCollection.m_controlledEntitiesClientCache == null || !MyPlayerCollection.m_controlledEntitiesClientCache.TryGetValue(entity.EntityId, out id) || Sync.Players.m_controlledEntities.TryGetValue(entity.EntityId, out playerId) && playerId == id)
        return;
      Sync.Players.SetControlledEntityInternal(id, entity);
    }

    [Event(null, 653)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnControlReleased(long entityId)
    {
      if (!Sync.IsServer && MyPlayerCollection.m_controlledEntitiesClientCache != null && MyPlayerCollection.m_controlledEntitiesClientCache.ContainsKey(entityId))
        MyPlayerCollection.m_controlledEntitiesClientCache.Remove(entityId);
      if (MyEventContext.Current.IsLocallyInvoked)
        return;
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      Sync.Players.RemoveControlledEntityInternal(entity);
    }

    [Event(null, 671)]
    [Reliable]
    [Broadcast]
    private static void OnIdentityCreated(bool isNpc, long identityId, string displayName)
    {
      if (isNpc)
        Sync.Players.CreateNewNpcIdentity(displayName, identityId);
      else
        Sync.Players.CreateNewIdentity(displayName, identityId, (string) null, new Vector3?());
    }

    [Event(null, 684)]
    [Reliable]
    [Server]
    private static void OnIdentityRemovedRequest(long identityId, ulong steamId, int serialId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) steamId != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!Sync.Players.RemoveIdentityInternal(identityId, new MyPlayer.PlayerId(steamId, serialId)))
          return;
        MyMultiplayer.RaiseStaticEvent<long, ulong, int>((Func<IMyEventOwner, Action<long, ulong, int>>) (s => new Action<long, ulong, int>(MyPlayerCollection.OnIdentityRemovedSuccess)), identityId, steamId, serialId);
      }
    }

    [Event(null, 701)]
    [Reliable]
    [Broadcast]
    private static void OnIdentityRemovedSuccess(long identityId, ulong steamId, int serialId) => Sync.Players.RemoveIdentityInternal(identityId, new MyPlayer.PlayerId(steamId, serialId));

    [Event(null, 707)]
    [Reliable]
    [Broadcast]
    private static void OnPlayerIdentityChanged(
      ulong clientSteamId,
      int playerSerialId,
      long identityId)
    {
      MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(clientSteamId, playerSerialId));
      if (playerById == null)
        return;
      MyIdentity myIdentity = (MyIdentity) null;
      Sync.Players.m_allIdentities.TryGetValue(identityId, out myIdentity);
      if (myIdentity == null)
        return;
      playerById.Identity = myIdentity;
    }

    [Event(null, 724)]
    [Reliable]
    [Client]
    private static void OnRespawnRequestFailure(int playerSerialId)
    {
      if (playerSerialId != 0)
        return;
      MyPlayerCollection.OnRespawnRequestFailureEvent.InvokeIfNotNull<ulong>(Sync.MyId);
      MyPlayerCollection.RequestLocalRespawn();
    }

    [Event(null, 734)]
    [Reliable]
    [Server]
    private static void OnSetPlayerDeadRequest(
      ulong clientSteamId,
      int playerSerialId,
      bool isDead,
      bool resetIdentity)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) clientSteamId != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!Sync.Players.SetPlayerDeadInternal(clientSteamId, playerSerialId, isDead, resetIdentity))
          return;
        MyMultiplayer.RaiseStaticEvent<ulong, int, bool, bool>((Func<IMyEventOwner, Action<ulong, int, bool, bool>>) (s => new Action<ulong, int, bool, bool>(MyPlayerCollection.OnSetPlayerDeadSuccess)), clientSteamId, playerSerialId, isDead, resetIdentity);
      }
    }

    [Event(null, 751)]
    [Reliable]
    [Broadcast]
    private static void OnSetPlayerDeadSuccess(
      ulong clientSteamId,
      int playerSerialId,
      bool isDead,
      bool resetIdentity)
    {
      Sync.Players.SetPlayerDeadInternal(clientSteamId, playerSerialId, isDead, resetIdentity);
    }

    [Event(null, 757)]
    [Reliable]
    [Server]
    private static void OnNewPlayerRequest(
      MyPlayerCollection.NewPlayerRequestParameters parameters)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) parameters.ClientSteamId != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(parameters.ClientSteamId, parameters.PlayerSerialId);
        if (Sync.Players.m_players.ContainsKey(playerId))
          return;
        if (Sync.Players.PlayerRequesting != null)
        {
          PlayerRequestArgs args = new PlayerRequestArgs(playerId);
          Sync.Players.PlayerRequesting(args);
          if (args.Cancel)
          {
            if (MyEventContext.Current.IsLocallyInvoked || parameters.IsWildlifeEntity)
            {
              MyPlayerCollection.OnNewPlayerFailure(parameters.ClientSteamId, parameters.PlayerSerialId);
              return;
            }
            MyMultiplayer.RaiseStaticEvent<ulong, int>((Func<IMyEventOwner, Action<ulong, int>>) (s => new Action<ulong, int>(MyPlayerCollection.OnNewPlayerFailure)), parameters.ClientSteamId, parameters.PlayerSerialId, MyEventContext.Current.Sender);
            return;
          }
        }
        MyIdentity identity = Sync.Players.TryGetPlayerIdentity(playerId);
        bool newIdentity = identity == null;
        if (newIdentity)
          identity = Sync.Players.RespawnComponent.CreateNewIdentity(parameters.DisplayName, playerId, parameters.CharacterModel, parameters.InitialPlayer);
        MyPlayer newPlayer = Sync.Players.CreateNewPlayer(identity, playerId, identity.DisplayName, parameters.RealPlayer, parameters.InitialPlayer, newIdentity, parameters.IsWildlifeEntity);
        newPlayer.IsWildlifeAgent = parameters.IsWildlifeEntity;
        MySession.Static.Factions.CompatDefaultFactions(new MyPlayer.PlayerId?(playerId));
        if (MyEventContext.Current.IsLocallyInvoked || parameters.IsWildlifeEntity)
          MyPlayerCollection.OnNewPlayerSuccess(parameters.ClientSteamId, parameters.PlayerSerialId);
        else
          MyMultiplayer.RaiseStaticEvent<ulong, int>((Func<IMyEventOwner, Action<ulong, int>>) (s => new Action<ulong, int>(MyPlayerCollection.OnNewPlayerSuccess)), parameters.ClientSteamId, parameters.PlayerSerialId, MyEventContext.Current.Sender);
        if (!parameters.IsWildlifeEntity && !MyBankingSystem.Static.TryGetAccountInfo(identity.IdentityId, out MyAccountInfo _))
          MyBankingSystem.Static.CreateAccount(identity.IdentityId);
        MyPlayerCollection.m_lastCreatedCharacter = newPlayer.Character != null ? newPlayer.Character.EntityId : 0L;
      }
    }

    [Event(null, 809)]
    [Reliable]
    [Client]
    private static void OnNewPlayerSuccess(ulong clientSteamId, int playerSerialId)
    {
      MyPlayer.PlayerId playerId1 = new MyPlayer.PlayerId(Sync.MyId, 0);
      MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
      if (playerId2 == playerId1 && (!MySession.Static.IsScenario || MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE))
        MyPlayerCollection.RequestLocalRespawn();
      Action<MyPlayer.PlayerId> requestSucceeded = Sync.Players.NewPlayerRequestSucceeded;
      if (requestSucceeded == null)
        return;
      requestSucceeded(playerId2);
    }

    [Event(null, 825)]
    [Reliable]
    [Client]
    private static void OnNewPlayerFailure(ulong clientSteamId, int playerSerialId)
    {
      if ((long) clientSteamId != (long) Sync.MyId)
        return;
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
      if (Sync.Players.NewPlayerRequestFailed == null)
        return;
      Sync.Players.NewPlayerRequestFailed(playerId.SerialId);
    }

    public void OnInitialPlayerCreated(
      ulong clientSteamId,
      int playerSerialId,
      bool newIdentity,
      MyObjectBuilder_Player playerBuilder)
    {
      if (newIdentity)
        MyPlayerCollection.OnIdentityCreated(false, playerBuilder.IdentityId, playerBuilder.DisplayName);
      MyPlayerCollection.OnPlayerCreated(clientSteamId, playerSerialId, playerBuilder);
      if ((long) clientSteamId != (long) Sync.MyId)
        return;
      MyMultiplayer.Static.StartProcessingClientMessages();
    }

    [Event(null, 852)]
    [Reliable]
    [Broadcast]
    private static void OnPlayerCreated(
      ulong clientSteamId,
      int playerSerialId,
      MyObjectBuilder_Player playerBuilder)
    {
      if (Sync.Players.TryGetIdentity(playerBuilder.IdentityId) == null)
        return;
      MyNetworkClient client = (MyNetworkClient) null;
      Sync.Clients.TryGetClient(clientSteamId, out client);
      if (client == null)
        return;
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
      MySession.Static.PromotedUsers[playerId.SteamId] = playerBuilder.PromoteLevel;
      if (playerBuilder.CreativeToolsEnabled)
        MySession.Static.CreativeTools.Add(playerId.SteamId);
      else
        MySession.Static.CreativeTools.Remove(playerId.SteamId);
      Sync.Players.CreateNewPlayerInternal(client, playerId, playerBuilder);
    }

    [Event(null, 873)]
    [Reliable]
    [Server]
    private static void OnPlayerRemoveRequest(
      ulong clientSteamId,
      int playerSerialId,
      bool removeCharacter)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) clientSteamId != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(clientSteamId, playerSerialId));
        if (playerById == null)
          return;
        Sync.Players.RemovePlayer(playerById, removeCharacter);
      }
    }

    [Event(null, 893)]
    [Reliable]
    [Broadcast]
    private static void OnPlayerRemoved(ulong clientSteamId, int playerSerialId)
    {
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
      if ((long) clientSteamId == (long) Sync.MyId)
        Sync.Players.RaiseLocalPlayerRemoved(playerSerialId);
      Sync.Players.RemovePlayerFromDictionary(playerId);
    }

    [Event(null, 905)]
    [Reliable]
    [Server]
    private static void OnPlayerColorChangedRequest(int serialId, int colorIndex, Vector3 newColor)
    {
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(!MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender.Value : Sync.MyId, serialId);
      MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
      if (playerById == null)
      {
        List<Vector3> vector3List;
        if (!MyCubeBuilder.AllPlayersColors.TryGetValue(playerId, out vector3List))
          return;
        vector3List[colorIndex] = newColor;
      }
      else
      {
        playerById.SelectedBuildColorSlot = colorIndex;
        playerById.ChangeOrSwitchToColor(newColor);
      }
    }

    [Event(null, 930)]
    [Reliable]
    [Server]
    private static void OnPlayerColorsChangedRequest(int serialId, [Serialize(MyObjectFlags.DefaultZero)] List<Vector3> newColors)
    {
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(!MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender.Value : Sync.MyId, serialId);
      MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
      if (playerById == null)
      {
        List<Vector3> vector3List;
        if (!MyCubeBuilder.AllPlayersColors.TryGetValue(playerId, out vector3List))
          return;
        vector3List.Clear();
        foreach (Vector3 newColor in newColors)
          vector3List.Add(newColor);
      }
      else
        playerById.SetBuildColorSlots(newColors);
    }

    [Event(null, 959)]
    [Reliable]
    [Server]
    private static void OnNpcIdentityRequest()
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyIdentity newNpcIdentity = Sync.Players.CreateNewNpcIdentity("NPC " + (object) MyRandom.Instance.Next(1000, 9999));
        if (newNpcIdentity == null)
          return;
        long identityId = newNpcIdentity.IdentityId;
        if (MyEventContext.Current.IsLocallyInvoked)
          MyPlayerCollection.OnNpcIdentitySuccess(identityId);
        else
          MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.OnNpcIdentitySuccess)), identityId, MyEventContext.Current.Sender);
      }
    }

    [Event(null, 982)]
    [Reliable]
    [Client]
    private static void OnNpcIdentitySuccess(long identidyId)
    {
      MyIdentity identity = Sync.Players.TryGetIdentity(identidyId);
      if (identity == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NPCIdentityAdded), (object) identity.DisplayName), messageCaption: messageCaption));
    }

    [Event(null, 995)]
    [Reliable]
    [Broadcast]
    private static void OnIdentityFirstSpawn(long identidyId) => Sync.Players.TryGetIdentity(identidyId)?.PerformFirstSpawn();

    [Event(null, 1005)]
    [Reliable]
    [Client]
    private static void SetIdentityBlockTypesBuilt(MyBlockLimits.MyTypeLimitData limits)
    {
      if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
      {
        MyLog.Default.Error("Missing session or local player in SetIdentityBlockTypesBuilt");
      }
      else
      {
        MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
        if (identity == null)
          return;
        if (identity.BlockLimits == null)
          MyLog.Default.Error("BlockLimits is null in SetIdentityBlockTypesBuilt");
        else if (MyEventContext.Current.IsLocallyInvoked)
          identity.BlockLimits.CallLimitsChanged();
        else
          identity.BlockLimits.SetTypeLimitsFromServer(limits);
      }
    }

    [Event(null, 1032)]
    [Reliable]
    [Client]
    private static void SetIdentityGridBlocksBuilt(
      MyBlockLimits.MyGridLimitData limits,
      int pcu,
      int pcuBuilt,
      int blocksBuilt,
      int transferedDelta)
    {
      MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
      if (identity == null)
        return;
      if (MyEventContext.Current.IsLocallyInvoked)
        identity.BlockLimits.CallLimitsChanged();
      else
        identity.BlockLimits.SetGridLimitsFromServer(limits, pcu, pcuBuilt, blocksBuilt, transferedDelta);
    }

    [Event(null, 1047)]
    [Reliable]
    [Client]
    private static void SetPCU_Client(int pcu, int transferedDelta)
    {
      MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
      if (identity == null)
        return;
      if (MyEventContext.Current.IsLocallyInvoked)
        identity.BlockLimits.CallLimitsChanged();
      else
        identity.BlockLimits.SetPCUFromServer(pcu, transferedDelta);
    }

    public void RequestPlayerColorChanged(int playerSerialId, int colorIndex, Vector3 newColor) => MyMultiplayer.RaiseStaticEvent<int, int, Vector3>((Func<IMyEventOwner, Action<int, int, Vector3>>) (s => new Action<int, int, Vector3>(MyPlayerCollection.OnPlayerColorChangedRequest)), playerSerialId, colorIndex, newColor);

    public void RequestPlayerColorsChanged(int playerSerialId, List<Vector3> newColors) => MyMultiplayer.RaiseStaticEvent<int, List<Vector3>>((Func<IMyEventOwner, Action<int, List<Vector3>>>) (s => new Action<int, List<Vector3>>(MyPlayerCollection.OnPlayerColorsChangedRequest)), playerSerialId, newColors);

    public long RequestNewPlayer(
      ulong steamId,
      int serialNumber,
      string playerName,
      string characterModel,
      bool realPlayer,
      bool initialPlayer,
      bool isWildlifeAgent = false)
    {
      MyPlayerCollection.m_lastCreatedCharacter = 0L;
      MyMultiplayer.RaiseStaticEvent<MyPlayerCollection.NewPlayerRequestParameters>((Func<IMyEventOwner, Action<MyPlayerCollection.NewPlayerRequestParameters>>) (s => new Action<MyPlayerCollection.NewPlayerRequestParameters>(MyPlayerCollection.OnNewPlayerRequest)), new MyPlayerCollection.NewPlayerRequestParameters(steamId, serialNumber, playerName, characterModel, realPlayer, initialPlayer, isWildlifeAgent));
      return MyPlayerCollection.m_lastCreatedCharacter;
    }

    public void RequestNewNpcIdentity() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyPlayerCollection.OnNpcIdentityRequest)));

    public MyPlayer CreateNewPlayer(
      MyIdentity identity,
      MyNetworkClient steamClient,
      string playerName,
      bool realPlayer)
    {
      MyPlayer.PlayerId freePlayerId = this.FindFreePlayerId(steamClient.SteamUserId);
      MyObjectBuilder_Player playerBuilder = new MyObjectBuilder_Player()
      {
        DisplayName = playerName,
        IdentityId = identity.IdentityId
      };
      return this.CreateNewPlayerInternal(steamClient, freePlayerId, playerBuilder);
    }

    public MyPlayer CreateNewPlayer(
      MyIdentity identity,
      MyPlayer.PlayerId id,
      string playerName,
      bool realPlayer,
      bool initialPlayer,
      bool newIdentity,
      bool isWildlifeAgent = false)
    {
      MyNetworkClient client;
      Sync.Clients.TryGetClient(id.SteamId, out client);
      if (client == null)
        return (MyPlayer) null;
      AdminSettingsEnum adminSettingsEnum;
      MySession.Static.RemoteAdminSettings.TryGetValue(id.SteamId, out adminSettingsEnum);
      MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(id.SteamId);
      bool flag = MySession.Static.CreativeToolsEnabled(id.SteamId);
      MyObjectBuilder_Player playerBuilder = new MyObjectBuilder_Player()
      {
        DisplayName = playerName,
        IdentityId = identity.IdentityId,
        ForceRealPlayer = realPlayer,
        PromoteLevel = userPromoteLevel,
        CreativeToolsEnabled = flag,
        RemoteAdminSettings = (int) adminSettingsEnum,
        IsWildlifeAgent = isWildlifeAgent
      };
      MyPlayer newPlayerInternal = this.CreateNewPlayerInternal(client, id, playerBuilder);
      if (newPlayerInternal != null)
      {
        if (!MyPlayer.IsColorsSetToDefaults(newPlayerInternal.BuildColorSlots))
        {
          List<Vector3> buildColorSlots = newPlayerInternal.BuildColorSlots;
        }
        MyObjectBuilder_Player objectBuilder = newPlayerInternal.GetObjectBuilder();
        objectBuilder.PromoteLevel = userPromoteLevel;
        objectBuilder.CreativeToolsEnabled = flag;
        objectBuilder.RemoteAdminSettings = (int) adminSettingsEnum;
        if (!initialPlayer || MyMultiplayer.Static == null)
        {
          MyMultiplayer.RaiseStaticEvent<ulong, int, MyObjectBuilder_Player>((Func<IMyEventOwner, Action<ulong, int, MyObjectBuilder_Player>>) (x => new Action<ulong, int, MyObjectBuilder_Player>(MyPlayerCollection.OnPlayerCreated)), id.SteamId, id.SerialId, objectBuilder);
        }
        else
        {
          PlayerDataMsg msg = new PlayerDataMsg()
          {
            ClientSteamId = id.SteamId,
            PlayerSerialId = id.SerialId,
            NewIdentity = newIdentity,
            PlayerBuilder = objectBuilder
          };
          MyMultiplayer.GetReplicationServer().SendPlayerData((Action<MyPacketDataBitStreamBase>) (x => MySerializer.Write<PlayerDataMsg>(x.Stream, ref msg)));
        }
      }
      return newPlayerInternal;
    }

    public MyPlayer InitNewPlayer(MyPlayer.PlayerId id, MyObjectBuilder_Player playerOb)
    {
      if (playerOb.IsWildlifeAgent)
        return this.CreateNewPlayerInternal(Sync.Clients.LocalClient, id, playerOb);
      MyNetworkClient client;
      Sync.Clients.TryGetClient(id.SteamId, out client);
      return client == null ? (MyPlayer) null : this.CreateNewPlayerInternal(client, id, playerOb);
    }

    public void RemovePlayer(MyPlayer player, bool removeCharacter = true)
    {
      if (Sync.IsServer)
      {
        if (removeCharacter && player.Character != null && !(player.Character.Parent is MyCryoChamber))
          player.Character.Close();
        this.KillPlayer(player);
        if (player.IsLocalPlayer)
          this.RaiseLocalPlayerRemoved(player.Id.SerialId);
        if (this.PlayerRemoved != null)
          this.PlayerRemoved(player.Id);
        this.RespawnComponent.AfterRemovePlayer(player);
        MyMultiplayer.RaiseStaticEvent<ulong, int>((Func<IMyEventOwner, Action<ulong, int>>) (s => new Action<ulong, int>(MyPlayerCollection.OnPlayerRemoved)), player.Id.SteamId, player.Id.SerialId);
        this.RemovePlayerFromDictionary(player.Id);
      }
      else
      {
        if (player.IsRemotePlayer)
          return;
        MyMultiplayer.RaiseStaticEvent<ulong, int, bool>((Func<IMyEventOwner, Action<ulong, int, bool>>) (s => new Action<ulong, int, bool>(MyPlayerCollection.OnPlayerRemoveRequest)), player.Id.SteamId, player.Id.SerialId, removeCharacter);
      }
    }

    public MyPlayer GetPlayerById(MyPlayer.PlayerId id)
    {
      MyPlayer myPlayer = (MyPlayer) null;
      this.m_players.TryGetValue(id, out myPlayer);
      return myPlayer;
    }

    public MyPlayer GetPlayerByName(string name)
    {
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
      {
        if (player.Value.DisplayName.Equals(name))
          return player.Value;
      }
      return (MyPlayer) null;
    }

    public bool TryGetPlayerById(MyPlayer.PlayerId id, out MyPlayer player) => this.m_players.TryGetValue(id, out player);

    public bool TrySetControlledEntity(MyPlayer.PlayerId id, MyEntity entity)
    {
      MyPlayer controllingPlayer = this.GetControllingPlayer(entity);
      if (controllingPlayer != null)
        return controllingPlayer.Id == id;
      this.SetControlledEntity(id, entity);
      return true;
    }

    public void SetControlledEntity(ulong steamUserId, MyEntity entity) => this.SetControlledEntity(new MyPlayer.PlayerId(steamUserId), entity);

    public void SetControlledEntityLocally(MyPlayer.PlayerId id, MyEntity entity) => this.SetControlledEntityInternal(id, entity, false);

    public void SetControlledEntity(MyPlayer.PlayerId id, MyEntity entity)
    {
      if (!Sync.IsServer)
        return;
      this.SetControlledEntityInternal(id, entity);
    }

    public List<MyPlayer> GetPlayersStartingNameWith(string prefix)
    {
      List<MyPlayer> myPlayerList = new List<MyPlayer>();
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
      {
        string displayName = player.Value.DisplayName;
        if (prefix.Length == 0 || displayName.Length >= prefix.Length && prefix.Equals(displayName.Substring(0, prefix.Length)))
          myPlayerList.Add(player.Value);
      }
      return myPlayerList;
    }

    public void RemoveControlledEntity(MyEntity entity) => this.RemoveControlledEntityProxy(entity, true);

    private void RemoveControlledEntityProxy(MyEntity entity, bool immediateOnServer)
    {
      if (Sync.IsServer)
        this.RemoveControlledEntityInternal(entity, immediateOnServer);
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.OnControlReleased)), entity.EntityId);
    }

    public void SetPlayerToCockpit(MyPlayer player, MyEntity controlledEntity)
    {
      Sync.Players.SetControlledEntityInternal(player.Id, controlledEntity);
      if (player != MySession.Static.LocalHumanPlayer || MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity || MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
        return;
      MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.LocalCharacter, new Vector3D?());
    }

    public void SetPlayerCharacter(MyPlayer player, MyCharacter newCharacter, MyEntity spawnedBy)
    {
      newCharacter.SetPlayer(player);
      if (MyVisualScriptLogicProvider.PlayerSpawned != null && !newCharacter.IsBot && newCharacter.ControllerInfo.Controller != null)
        MyVisualScriptLogicProvider.PlayerSpawned(newCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
      if (spawnedBy == null)
        return;
      long entityId = spawnedBy.EntityId;
      Vector3 translation = (Vector3) (newCharacter.WorldMatrix * MatrixD.Invert(spawnedBy.WorldMatrix)).Translation;
      MyMultiplayer.RaiseEvent<MyCharacter, long, Vector3>(newCharacter, (Func<MyCharacter, Action<long, Vector3>>) (x => new Action<long, Vector3>(x.SpawnCharacterRelative)), entityId, translation);
    }

    public MyPlayer GetControllingPlayer(MyEntity entity)
    {
      MyPlayer.PlayerId key;
      MyPlayer myPlayer;
      return this.m_controlledEntities.TryGetValue(entity.EntityId, out key) && this.m_players.TryGetValue(key, out myPlayer) ? myPlayer : (MyPlayer) null;
    }

    public MyPlayer GetPreviousControllingPlayer(MyEntity entity)
    {
      MyPlayer.PlayerId key;
      MyPlayer myPlayer;
      return this.m_previousControlledEntities.TryGetValue(entity.EntityId, out key) && this.m_players.TryGetValue(key, out myPlayer) ? myPlayer : (MyPlayer) null;
    }

    public MyEntityController GetEntityController(MyEntity entity) => this.GetControllingPlayer(entity)?.Controller;

    public ICollection<MyPlayer> GetOnlinePlayers() => this.m_players.Values;

    public int GetOnlinePlayerCount() => this.m_players.Values.Count;

    public bool IsPlayerOnline(ref MyPlayer.PlayerId playerId) => this.m_players.ContainsKey(playerId);

    public bool IsPlayerOnline(long identityId)
    {
      MyPlayer.PlayerId result;
      return MySession.Static.Players.TryGetPlayerId(identityId, out result) && MySession.Static.Players.IsPlayerOnline(ref result);
    }

    public ICollection<MyIdentity> GetAllIdentities() => this.m_allIdentities.Values;

    public IOrderedEnumerable<KeyValuePair<long, MyIdentity>> GetAllIdentitiesOrderByName() => this.m_allIdentities.OrderBy<KeyValuePair<long, MyIdentity>, string>((Func<KeyValuePair<long, MyIdentity>, string>) (pair => pair.Value.DisplayName));

    public HashSet<long> GetNPCIdentities() => this.m_npcIdentities;

    public ICollection<MyPlayer.PlayerId> GetAllPlayers() => this.m_playerIdentityIds.Keys;

    public void UpdatePlayerControllers(long controllerId)
    {
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
      {
        if (player.Value.CachedControllerId != null && player.Value.CachedControllerId.Contains(controllerId))
          this.TryTakeControl(player.Value, controllerId);
      }
    }

    public void SendDirtyBlockLimits()
    {
      switch (MySession.Static.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.GLOBALLY:
          foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) this.GetOnlinePlayers())
          {
            if (onlinePlayer.Identity != null && onlinePlayer.IsRealPlayer)
              this.m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer.Id.SteamId));
          }
          this.SendDirtyBlockLimit(MySession.Static.GlobalBlockLimits, this.m_tmpPlayersLinkedToBlockLimit);
          this.m_tmpPlayersLinkedToBlockLimit.Clear();
          break;
        case MyBlockLimitsEnabledEnum.PER_FACTION:
          foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
          {
            foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) this.GetOnlinePlayers())
            {
              if (faction.Value.IsMember(onlinePlayer.Identity.IdentityId))
                this.m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer.Id.SteamId));
            }
            if (this.m_tmpPlayersLinkedToBlockLimit.Count > 0)
              this.SendDirtyBlockLimit(faction.Value.BlockLimits, this.m_tmpPlayersLinkedToBlockLimit);
            this.m_tmpPlayersLinkedToBlockLimit.Clear();
          }
          using (IEnumerator<MyPlayer> enumerator = this.GetOnlinePlayers().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyPlayer current = enumerator.Current;
              if (MySession.Static.Factions.GetPlayerFaction(current.Identity.IdentityId) == null)
              {
                this.m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(current.Id.SteamId));
                this.SendDirtyBlockLimit(current.Identity.BlockLimits, this.m_tmpPlayersLinkedToBlockLimit);
              }
            }
            break;
          }
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          using (IEnumerator<MyPlayer> enumerator = this.GetOnlinePlayers().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyPlayer current = enumerator.Current;
              if (current.Identity != null && current.IsRealPlayer)
              {
                this.m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(current.Id.SteamId));
                this.SendDirtyBlockLimit(current.Identity.BlockLimits, this.m_tmpPlayersLinkedToBlockLimit);
                this.m_tmpPlayersLinkedToBlockLimit.Clear();
              }
            }
            break;
          }
      }
    }

    public void SendDirtyBlockLimit(MyBlockLimits blockLimit, List<EndpointId> playersToSendTo)
    {
      foreach (MyBlockLimits.MyTypeLimitData myTypeLimitData in (IEnumerable<MyBlockLimits.MyTypeLimitData>) blockLimit.BlockTypeBuilt.Values)
      {
        if (Interlocked.CompareExchange(ref myTypeLimitData.Dirty, 0, 1) > 0)
        {
          foreach (EndpointId targetEndpoint in playersToSendTo)
            MyMultiplayer.RaiseStaticEvent<MyBlockLimits.MyTypeLimitData>((Func<IMyEventOwner, Action<MyBlockLimits.MyTypeLimitData>>) (x => new Action<MyBlockLimits.MyTypeLimitData>(MyPlayerCollection.SetIdentityBlockTypesBuilt)), myTypeLimitData, targetEndpoint);
        }
      }
      foreach (MyBlockLimits.MyGridLimitData myGridLimitData in (IEnumerable<MyBlockLimits.MyGridLimitData>) blockLimit.BlocksBuiltByGrid.Values)
      {
        if (Interlocked.CompareExchange(ref myGridLimitData.Dirty, 0, 1) > 0)
        {
          foreach (EndpointId targetEndpoint in playersToSendTo)
            MyMultiplayer.RaiseStaticEvent<MyBlockLimits.MyGridLimitData, int, int, int, int>((Func<IMyEventOwner, Action<MyBlockLimits.MyGridLimitData, int, int, int, int>>) (x => new Action<MyBlockLimits.MyGridLimitData, int, int, int, int>(MyPlayerCollection.SetIdentityGridBlocksBuilt)), myGridLimitData, blockLimit.PCU, blockLimit.PCUBuilt, blockLimit.BlocksBuilt, blockLimit.TransferedDelta, targetEndpoint);
        }
        if (Interlocked.CompareExchange(ref myGridLimitData.NameDirty, 0, 1) > 0)
        {
          foreach (EndpointId targetEndpoint in playersToSendTo)
            MyMultiplayer.RaiseStaticEvent<long, string>((Func<IMyEventOwner, Action<long, string>>) (x => new Action<long, string>(MyBlockLimits.SetGridNameFromServer)), myGridLimitData.EntityId, myGridLimitData.GridName, targetEndpoint);
        }
      }
      if (blockLimit.CompareExchangePCUDirty())
      {
        foreach (EndpointId targetEndpoint in playersToSendTo)
          MyMultiplayer.RaiseStaticEvent<int, int>((Func<IMyEventOwner, Action<int, int>>) (x => new Action<int, int>(MyPlayerCollection.SetPCU_Client)), blockLimit.PCU, blockLimit.TransferedDelta, targetEndpoint);
      }
label_36:
      long key;
      MyBlockLimits.MyGridLimitData myGridLimitData1;
      do
      {
        key = blockLimit.GridsRemoved.Keys.ElementAtOrDefault<long>(0);
        if (key == 0L)
          goto label_42;
      }
      while (!blockLimit.GridsRemoved.TryRemove(key, out myGridLimitData1));
      goto label_38;
label_42:
      return;
label_38:
      using (List<EndpointId>.Enumerator enumerator = playersToSendTo.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          EndpointId current = enumerator.Current;
          MyMultiplayer.RaiseStaticEvent<MyBlockLimits.MyGridLimitData, int, int, int, int>((Func<IMyEventOwner, Action<MyBlockLimits.MyGridLimitData, int, int, int, int>>) (x => new Action<MyBlockLimits.MyGridLimitData, int, int, int, int>(MyPlayerCollection.SetIdentityGridBlocksBuilt)), myGridLimitData1, blockLimit.PCU, blockLimit.PCUBuilt, blockLimit.BlocksBuilt, blockLimit.TransferedDelta, current);
        }
        goto label_36;
      }
    }

    public void TryExtendControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityGettingControl)
    {
      MyEntityController controller = baseEntity.ControllerInfo.Controller;
      if (controller == null)
        return;
      this.TrySetControlledEntity(controller.Player.Id, entityGettingControl);
    }

    public void ExtendControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityGettingControl)
    {
      MyEntityController controller = baseEntity.ControllerInfo.Controller;
      if (controller != null)
      {
        this.TrySetControlledEntity(controller.Player.Id, entityGettingControl);
      }
      else
      {
        MyRemoteControl myRemoteControl = baseEntity as MyRemoteControl;
      }
    }

    public bool TryReduceControl(
      Sandbox.Game.Entities.IMyControllableEntity baseEntity,
      MyEntity entityWhichLoosesControl)
    {
      MyEntityController controller = baseEntity.ControllerInfo.Controller;
      MyPlayer.PlayerId playerId;
      if (controller == null || !this.m_controlledEntities.TryGetValue(entityWhichLoosesControl.EntityId, out playerId) || !(controller.Player.Id == playerId))
        return false;
      this.RemoveControlledEntity(entityWhichLoosesControl);
      return true;
    }

    public void ReduceControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityWhichLoosesControl)
    {
      if (this.TryReduceControl(baseEntity, entityWhichLoosesControl))
        return;
      MyRemoteControl myRemoteControl = baseEntity as MyRemoteControl;
    }

    public void ReduceAllControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity)
    {
      MyPlayer.PlayerId playerId;
      if (!this.m_controlledEntities.TryGetValue(baseEntity.Entity.EntityId, out playerId))
        return;
      foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in this.m_controlledEntities)
      {
        if (!(controlledEntity.Value != playerId) && controlledEntity.Key != baseEntity.Entity.EntityId)
        {
          MyEntity entity = (MyEntity) null;
          MyEntities.TryGetEntityById(controlledEntity.Key, out entity, true);
          if (entity != null)
            this.RemoveControlledEntityProxy(entity, false);
        }
      }
      this.m_controlledEntities.ApplyRemovals();
    }

    public bool HasExtendedControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity secondEntity) => baseEntity.ControllerInfo.Controller == this.GetEntityController(secondEntity);

    [Event(null, 1607)]
    [Reliable]
    [Server]
    public static void SetDampeningEntity(long controlledEntityId)
    {
      if (!(MyEntities.GetEntityByIdOrDefault(controlledEntityId) is Sandbox.Game.Entities.IMyControllableEntity entityByIdOrDefault) || entityByIdOrDefault.Entity == null)
        return;
      MatrixD headMatrix = entityByIdOrDefault.GetHeadMatrix(true);
      Vector3D forward = headMatrix.Forward;
      Vector3D translation = headMatrix.Translation;
      Vector3D to = translation + forward * 1000.0;
      LineD line = new LineD(translation, to);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) entityByIdOrDefault, entityByIdOrDefault.Entity.GetTopMostParent((Type) null), ignoreFloatingObjects: false);
      if (intersectionWithLine.HasValue && intersectionWithLine.Value.Entity != null)
      {
        IMyEntity topMostParent = intersectionWithLine.Value.Entity.GetTopMostParent();
        entityByIdOrDefault.RelativeDampeningEntity = (MyEntity) topMostParent;
      }
      else
        entityByIdOrDefault.RelativeDampeningEntity = (MyEntity) null;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyPlayerCollection.SetDampeningEntityClient)), entityByIdOrDefault.Entity.EntityId, entityByIdOrDefault.RelativeDampeningEntity != null ? entityByIdOrDefault.RelativeDampeningEntity.EntityId : 0L);
    }

    [Event(null, 1643)]
    [Reliable]
    [Server]
    public static void ClearDampeningEntity(long controlledEntityId)
    {
      if (!(MyEntities.GetEntityByIdOrDefault(controlledEntityId) is Sandbox.Game.Entities.IMyControllableEntity entityByIdOrDefault))
        return;
      entityByIdOrDefault.RelativeDampeningEntity = (MyEntity) null;
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (s => new Action<long, long>(MyPlayerCollection.SetDampeningEntityClient)), entityByIdOrDefault.Entity.EntityId, 0L);
    }

    [Event(null, 1656)]
    [Reliable]
    [BroadcastExcept]
    public static void SetDampeningEntityClient(long controlledEntityId, long dampeningEntityId)
    {
      if (!(MyEntities.GetEntityByIdOrDefault(controlledEntityId) is Sandbox.Game.Entities.IMyControllableEntity entityByIdOrDefault))
        return;
      MyEntity entityByIdOrDefault1 = MyEntities.GetEntityByIdOrDefault(dampeningEntityId);
      if (entityByIdOrDefault1 != null)
        entityByIdOrDefault.RelativeDampeningEntity = entityByIdOrDefault1;
      else
        entityByIdOrDefault.RelativeDampeningEntity = (MyEntity) null;
    }

    public MyIdentity CreateNewNpcIdentity(string name, long identityId = 0)
    {
      MyIdentity identity = identityId != 0L ? base.CreateNewIdentity(name, identityId, (string) null, new Vector3?()) : this.CreateNewIdentity(name, (string) null, new Vector3?());
      if (Sync.IsServer && !MyBankingSystem.Static.TryGetAccountInfo(identity.IdentityId, out MyAccountInfo _))
        MyBankingSystem.Static.CreateAccount(identity.IdentityId);
      this.AfterCreateIdentity(identity, true);
      return identity;
    }

    public MyIdentity CreateNewIdentity(
      string name,
      string model = null,
      Vector3? colorMask = null,
      bool initialPlayer = false,
      bool addToNpcs = false)
    {
      MyIdentity newIdentity = this.CreateNewIdentity(name, model, new Vector3?());
      MyIdentity identity = newIdentity;
      bool flag = !initialPlayer;
      int num1 = addToNpcs ? 1 : 0;
      int num2 = flag ? 1 : 0;
      this.AfterCreateIdentity(identity, num1 != 0, num2 != 0);
      return newIdentity;
    }

    public override MyIdentity CreateNewIdentity(
      string name,
      long identityId,
      string model,
      Vector3? colorMask)
    {
      bool addToNpcs = false;
      switch (MyEntityIdentifier.GetIdObjectType(identityId))
      {
        case MyEntityIdentifier.ID_OBJECT_TYPE.NPC:
        case MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP:
          addToNpcs = ((addToNpcs ? 1 : 0) | 1) != 0;
          break;
      }
      MyIdentity newIdentity = base.CreateNewIdentity(name, identityId, model, colorMask);
      this.AfterCreateIdentity(newIdentity, addToNpcs);
      return newIdentity;
    }

    public override MyIdentity CreateNewIdentity(MyObjectBuilder_Identity objectBuilder)
    {
      bool addToNpcs = false;
      switch (MyEntityIdentifier.GetIdObjectType(objectBuilder.IdentityId))
      {
        case MyEntityIdentifier.ID_OBJECT_TYPE.NPC:
        case MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP:
          addToNpcs = true;
          break;
      }
      MyIdentity newIdentity = base.CreateNewIdentity(objectBuilder);
      this.AfterCreateIdentity(newIdentity, addToNpcs);
      return newIdentity;
    }

    public void RemoveIdentity(long identityId, MyPlayer.PlayerId playerId = default (MyPlayer.PlayerId))
    {
      if (Sync.IsServer)
      {
        if (!this.RemoveIdentityInternal(identityId, playerId))
          return;
        MyMultiplayer.RaiseStaticEvent<long, ulong, int>((Func<IMyEventOwner, Action<long, ulong, int>>) (s => new Action<long, ulong, int>(MyPlayerCollection.OnIdentityRemovedSuccess)), identityId, playerId.SteamId, playerId.SerialId);
      }
      else
        MyMultiplayer.RaiseStaticEvent<long, ulong, int>((Func<IMyEventOwner, Action<long, ulong, int>>) (s => new Action<long, ulong, int>(MyPlayerCollection.OnIdentityRemovedRequest)), identityId, playerId.SteamId, playerId.SerialId);
    }

    public bool HasIdentity(long identityId) => this.m_allIdentities.ContainsKey(identityId);

    public MyIdentity TryGetIdentity(long identityId)
    {
      MyIdentity myIdentity;
      this.m_allIdentities.TryGetValue(identityId, out myIdentity);
      return myIdentity;
    }

    public bool TryGetPlayerId(long identityId, out MyPlayer.PlayerId result) => this.m_identityPlayerIds.TryGetValue(identityId, out result);

    public MyIdentity TryGetPlayerIdentity(MyPlayer.PlayerId playerId)
    {
      MyIdentity myIdentity = (MyIdentity) null;
      long identityId = this.TryGetIdentityId(playerId.SteamId, playerId.SerialId);
      if (identityId != 0L)
        myIdentity = this.TryGetIdentity(identityId);
      return myIdentity;
    }

    public long TryGetIdentityId(ulong steamId, int serialId = 0)
    {
      long num = 0;
      this.m_playerIdentityIds.TryGetValue(new MyPlayer.PlayerId(steamId, serialId), out num);
      return num;
    }

    public ulong TryGetSteamId(long identityId)
    {
      MyPlayer.PlayerId result;
      return !this.TryGetPlayerId(identityId, out result) ? 0UL : result.SteamId;
    }

    public int TryGetSerialId(long identityId)
    {
      MyPlayer.PlayerId result;
      return !this.TryGetPlayerId(identityId, out result) ? 0 : result.SerialId;
    }

    public string TryGetIdentityNameFromSteamId(ulong steamId)
    {
      MyIdentity identity = this.TryGetIdentity(this.TryGetIdentityId(steamId, 0));
      return identity != null ? identity.DisplayName : string.Empty;
    }

    public void MarkIdentityAsNPC(long identityId) => this.m_npcIdentities.Add(identityId);

    public void UnmarkIdentityAsNPC(long identityId) => this.m_npcIdentities.Remove(identityId);

    public bool IdentityIsNpc(long identityId) => this.m_npcIdentities.Contains(identityId);

    public void LoadIdentities(List<MyObjectBuilder_Identity> list)
    {
      if (list == null)
        return;
      foreach (MyObjectBuilder_Identity objectBuilder in list)
      {
        MyIdentity newIdentity = this.CreateNewIdentity(objectBuilder);
        if (Sync.IsServer && !MyBankingSystem.Static.TryGetAccountInfo(newIdentity.IdentityId, out MyAccountInfo _))
          MyBankingSystem.Static.CreateAccount(newIdentity.IdentityId);
      }
    }

    public void ClearIdentities()
    {
      this.m_allIdentities.Clear();
      this.m_npcIdentities.Clear();
    }

    public void SetRespawnComponent(MyRespawnComponentBase respawnComponent) => this.RespawnComponent = respawnComponent;

    public static void RequestLocalRespawn()
    {
      MySandboxGame.Log.WriteLine("RequestRespawn");
      if (Sandbox.Engine.Platform.Game.IsDedicated || Sync.Players == null)
        return;
      string model = (string) null;
      Color red = Color.Red;
      MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref red);
      Sync.Players.LocalRespawnRequested.InvokeIfNotNull<string, Color>(model, red);
      if (MyMultiplayer.Static == null)
        return;
      MyMultiplayer.Static.InvokeLocalRespawnRequested();
    }

    public static void RespawnRequest(
      bool joinGame,
      bool newIdentity,
      long respawnEntityId,
      string shipPrefabId,
      int playerSerialId,
      string modelName,
      Color color)
    {
      MyMultiplayer.RaiseStaticEvent<MyPlayerCollection.RespawnMsg>((Func<IMyEventOwner, Action<MyPlayerCollection.RespawnMsg>>) (s => new Action<MyPlayerCollection.RespawnMsg>(MyPlayerCollection.OnRespawnRequest)), new MyPlayerCollection.RespawnMsg()
      {
        JoinGame = joinGame,
        RespawnEntityId = respawnEntityId,
        NewIdentity = newIdentity,
        RespawnShipId = shipPrefabId,
        PlayerSerialId = playerSerialId,
        ModelName = modelName,
        Color = color
      });
    }

    public void KillPlayer(MyPlayer player) => this.SetPlayerDead(player, true, MySession.Static.Settings.PermanentDeath.Value);

    public void RevivePlayer(MyPlayer player) => this.SetPlayerDead(player, false, false);

    private void SetPlayerDead(MyPlayer player, bool deadState, bool resetIdentity)
    {
      if (Sync.IsServer)
      {
        if (!this.SetPlayerDeadInternal(player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity))
          return;
        MyMultiplayer.RaiseStaticEvent<ulong, int, bool, bool>((Func<IMyEventOwner, Action<ulong, int, bool, bool>>) (s => new Action<ulong, int, bool, bool>(MyPlayerCollection.OnSetPlayerDeadSuccess)), player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity);
      }
      else
        MyMultiplayer.RaiseStaticEvent<ulong, int, bool, bool>((Func<IMyEventOwner, Action<ulong, int, bool, bool>>) (s => new Action<ulong, int, bool, bool>(MyPlayerCollection.OnSetPlayerDeadRequest)), player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity);
    }

    private bool SetPlayerDeadInternal(
      ulong playerSteamId,
      int playerSerialId,
      bool deadState,
      bool resetIdentity)
    {
      MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(playerSteamId, playerSerialId));
      if (playerById == null || playerById.Identity == null)
        return false;
      playerById.Identity.SetDead(resetIdentity);
      if (deadState)
      {
        playerById.Controller.TakeControl((Sandbox.Game.Entities.IMyControllableEntity) null);
        foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in this.m_controlledEntities)
        {
          if (controlledEntity.Value == playerById.Id)
          {
            MyEntity entity = (MyEntity) null;
            MyEntities.TryGetEntityById(controlledEntity.Key, out entity);
            if (entity != null)
              this.RemoveControlledEntityInternal(entity, false);
          }
        }
        this.m_controlledEntities.ApplyRemovals();
        if (Sync.Clients.LocalClient != null && playerById == Sync.Clients.LocalClient.FirstPlayer)
          MyPlayerCollection.RequestLocalRespawn();
      }
      return true;
    }

    [Event(null, 1958)]
    [Reliable]
    [Server]
    private static void OnRespawnRequest(MyPlayerCollection.RespawnMsg msg) => MyPlayerCollection.OnRespawnRequest(msg.JoinGame, msg.NewIdentity, msg.RespawnEntityId, msg.RespawnShipId, new Vector3D?(), new Vector3?(), new Vector3?(), new SerializableDefinitionId?(), true, msg.PlayerSerialId, msg.ModelName, msg.Color);

    public static void OnRespawnRequest(
      bool joinGame,
      bool newIdentity,
      long respawnEntityId,
      string respawnShipId,
      Vector3D? spawnPosition,
      Vector3? direction,
      Vector3? up,
      SerializableDefinitionId? botDefinitionId,
      bool realPlayer,
      int playerSerialId,
      string modelName,
      Color color)
    {
      if (!Sync.IsServer)
        return;
      EndpointId targetEndpoint = MyEventContext.Current.IsLocallyInvoked || !realPlayer && MySession.Static.IsServer ? new EndpointId(Sync.MyId) : MyEventContext.Current.Sender;
      if (Sync.Players.RespawnComponent == null)
        return;
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(targetEndpoint.Value, playerSerialId);
      if (Sync.Players.RespawnComponent.HandleRespawnRequest(joinGame, newIdentity, respawnEntityId, respawnShipId, playerId, spawnPosition, direction, up, botDefinitionId, realPlayer, modelName, color))
      {
        MyIdentity playerIdentity = Sync.Players.TryGetPlayerIdentity(playerId);
        if (playerIdentity != null)
        {
          if (!playerIdentity.FirstSpawnDone)
          {
            MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.OnIdentityFirstSpawn)), playerIdentity.IdentityId);
            playerIdentity.PerformFirstSpawn();
          }
          playerIdentity.LogRespawnTime();
        }
        MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
        if (playerById == null || playerById.Controller == null || !(playerById.Controller.ControlledEntity is MyEntity controlledEntity))
          return;
        MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), playerId.SteamId, playerId.SerialId, controlledEntity.EntityId, true);
      }
      else
        MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyPlayerCollection.OnRespawnRequestFailure)), playerSerialId, targetEndpoint);
    }

    private MyPlayer CreateNewPlayerInternal(
      MyNetworkClient steamClient,
      MyPlayer.PlayerId playerId,
      MyObjectBuilder_Player playerBuilder)
    {
      if (!this.m_playerIdentityIds.ContainsKey(playerId))
      {
        this.m_playerIdentityIds.TryAdd(playerId, playerBuilder.IdentityId);
        if (!this.m_identityPlayerIds.ContainsKey(playerBuilder.IdentityId))
          this.m_identityPlayerIds.Add(playerBuilder.IdentityId, playerId);
      }
      MyPlayer newPlayer = this.GetPlayerById(playerId);
      if (newPlayer == null)
      {
        newPlayer = new MyPlayer(steamClient, playerId);
        newPlayer.Init(playerBuilder);
        newPlayer.IdentityChanged += new Action<MyPlayer, MyIdentity>(this.player_IdentityChanged);
        newPlayer.Controller.ControlledEntityChanged += new Action<Sandbox.Game.Entities.IMyControllableEntity, Sandbox.Game.Entities.IMyControllableEntity>(this.controller_ControlledEntityChanged);
        this.AddPlayer(playerId, newPlayer);
      }
      return newPlayer;
    }

    public static void ChangePlayerCharacter(
      MyPlayer player,
      MyCharacter characterEntity,
      MyEntity entity)
    {
      if (player == null)
      {
        MySandboxGame.Log.WriteLine("Player not found");
      }
      else
      {
        if (player.Identity == null)
          MySandboxGame.Log.WriteLine("Player identity was null");
        player.Identity.ChangeCharacter(characterEntity);
        if (player.Controller == null || player.Controller.ControlledEntity == null)
          Sync.Players.SetControlledEntityInternal(player.Id, entity, false);
        if (player != MySession.Static.LocalHumanPlayer || MySession.Static.ControlledEntity is MyShipController controlledEntity && controlledEntity.Pilot == characterEntity || MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
          return;
        MySession.Static.SetCameraController(MySession.Static.LocalCharacter.IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, (IMyEntity) MySession.Static.LocalCharacter, new Vector3D?());
      }
    }

    private void SetControlledEntityInternal(MyPlayer.PlayerId id, MyEntity entity, bool sync = true)
    {
      MyPlayer.PlayerId playerId;
      if (!Sync.IsServer && (!MyPlayerCollection.m_controlledEntitiesClientCache.TryGetValue(entity.EntityId, out playerId) ? 0 : (playerId == id ? 1 : 0)) == 0)
        return;
      MyPlayer playerById = Sync.Players.GetPlayerById(id);
      this.RemoveControlledEntityInternal(entity);
      entity.OnClosing += this.m_entityClosingHandler;
      if (playerById != null && playerById.Controller != null && entity is Sandbox.Game.Entities.IMyControllableEntity)
      {
        if (entity is MyCharacter && playerById.Identity != null && entity != playerById.Identity.Character)
          playerById.Identity.ChangeCharacter(entity as MyCharacter);
        playerById.Controller.TakeControl((Sandbox.Game.Entities.IMyControllableEntity) entity);
      }
      else if (playerById != null)
        this.m_controlledEntities.Add(entity.EntityId, playerById.Id, true);
      if (Sync.IsServer & sync && playerById != null)
        MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), playerById.Id.SteamId, playerById.Id.SerialId, entity.EntityId, false);
      if (MySession.Static.LocalHumanPlayer == null || !(id == MySession.Static.LocalHumanPlayer.Id) || (!(entity is IMyCameraController cameraController) || MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning))
        return;
      MySession.Static.SetCameraController(cameraController.IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, (IMyEntity) entity, new Vector3D?());
    }

    private void controller_ControlledEntityChanged(
      Sandbox.Game.Entities.IMyControllableEntity oldEntity,
      Sandbox.Game.Entities.IMyControllableEntity newEntity)
    {
      MyEntityController entityController = newEntity == null ? oldEntity.ControllerInfo.Controller : newEntity.ControllerInfo.Controller;
      if (oldEntity is MyEntity myEntity)
      {
        MyPlayer.PlayerId playerId;
        if (this.m_controlledEntities.TryGetValue(myEntity.EntityId, out playerId))
          this.m_previousControlledEntities[myEntity.EntityId] = playerId;
        this.m_controlledEntities.Remove(myEntity.EntityId, true);
        if (Sync.IsServer)
          MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), 0UL, 0, myEntity.EntityId, true);
      }
      if (!(newEntity is MyEntity myEntity) || entityController == null)
        return;
      this.m_controlledEntities.Add(myEntity.EntityId, entityController.Player.Id, true);
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), entityController.Player.Id.SteamId, entityController.Player.Id.SerialId, myEntity.EntityId, true);
    }

    private void RemoveControlledEntityInternal(MyEntity entity, bool immediate = true)
    {
      entity.OnClosing -= this.m_entityClosingHandler;
      MyPlayer.PlayerId playerId;
      if (this.m_controlledEntities.TryGetValue(entity.EntityId, out playerId))
        this.m_previousControlledEntities[entity.EntityId] = playerId;
      this.m_controlledEntities.Remove(entity.EntityId, immediate);
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, int, long, bool>((Func<IMyEventOwner, Action<ulong, int, long, bool>>) (s => new Action<ulong, int, long, bool>(MyPlayerCollection.OnControlChangedSuccess)), 0UL, 0, entity.EntityId, true);
    }

    private void EntityClosing(MyEntity entity)
    {
      entity.OnClosing -= this.m_entityClosingHandler;
      if (entity is Sandbox.Game.Entities.IMyControllableEntity)
        return;
      this.m_controlledEntities.Remove(entity.EntityId, true);
      this.m_previousControlledEntities.Remove(entity.EntityId);
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.OnControlReleased)), entity.EntityId);
    }

    private void Multiplayer_ClientRemoved(ulong steamId)
    {
      if (!Sync.IsServer)
        return;
      this.m_tmpRemovedPlayers.Clear();
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
      {
        if ((long) player.Key.SteamId == (long) steamId)
          this.m_tmpRemovedPlayers.Add(player.Value);
      }
      foreach (MyPlayer tmpRemovedPlayer in this.m_tmpRemovedPlayers)
        this.RemovePlayer(tmpRemovedPlayer, false);
      this.m_tmpRemovedPlayers.Clear();
    }

    private void RaiseLocalPlayerRemoved(int serialId)
    {
      Action<int> localPlayerRemoved = this.LocalPlayerRemoved;
      if (localPlayerRemoved == null)
        return;
      localPlayerRemoved(serialId);
    }

    private bool RemoveIdentityInternal(long identityId, MyPlayer.PlayerId playerId)
    {
      if (playerId.IsValid && this.m_players.ContainsKey(playerId))
        return false;
      MyIdentity myIdentity;
      if (this.m_allIdentities.TryGetValue(identityId, out myIdentity))
      {
        myIdentity.ChangeCharacter((MyCharacter) null);
        myIdentity.CharacterChanged -= new Action<MyCharacter, MyCharacter>(this.Identity_CharacterChanged);
      }
      this.m_allIdentities.Remove<long, MyIdentity>(identityId);
      this.m_npcIdentities.Remove(identityId);
      if (playerId.IsValid)
      {
        long key;
        if (this.m_playerIdentityIds.TryGetValue(playerId, out key))
        {
          this.m_identityPlayerIds.Remove(key);
          if (key == identityId)
          {
            MySession.Static.Factions.RemovePlayerFromVisibility(playerId);
            MyCubeBuilder.RemovePlayerColors(playerId);
          }
        }
        this.m_playerIdentityIds.Remove<MyPlayer.PlayerId, long>(playerId);
      }
      if (Sync.IsServer)
      {
        MyBankingSystem.Static.RemoveAccount(myIdentity.IdentityId);
        MySession.Static.GetComponent<MySessionComponentContainerDropSystem>()?.RemovePlayerDataForIdentity(myIdentity.IdentityId);
        MySession.Static.Gpss.RemovePlayerGpss(myIdentity.IdentityId);
      }
      MySession.Static.Factions.DeletePlayerRelations(myIdentity.IdentityId);
      Action identitiesChanged = this.IdentitiesChanged;
      if (identitiesChanged != null)
        identitiesChanged();
      return true;
    }

    private void LoadIdentitiesObsolete(
      List<MyObjectBuilder_Checkpoint.PlayerItem> playersFromSession,
      MyPlayer.PlayerId? savingPlayerId = null)
    {
      foreach (MyObjectBuilder_Checkpoint.PlayerItem playerItem in playersFromSession)
      {
        MyIdentity newIdentity = this.CreateNewIdentity(playerItem.Name, playerItem.PlayerId, playerItem.Model, new Vector3?());
        MyPlayer.PlayerId key = new MyPlayer.PlayerId(playerItem.SteamId);
        if (savingPlayerId.HasValue && key == savingPlayerId.Value)
          key = new MyPlayer.PlayerId(Sync.MyId);
        if (!playerItem.IsDead && !this.m_playerIdentityIds.ContainsKey(key))
        {
          this.m_playerIdentityIds.TryAdd(key, newIdentity.IdentityId);
          this.m_identityPlayerIds.Add(newIdentity.IdentityId, key);
          newIdentity.SetDead(false);
        }
      }
    }

    private void AfterCreateIdentity(MyIdentity identity, bool addToNpcs = false, bool sendToClients = true)
    {
      if (addToNpcs)
        this.MarkIdentityAsNPC(identity.IdentityId);
      if (!this.m_allIdentities.ContainsKey(identity.IdentityId))
      {
        this.m_allIdentities.TryAdd(identity.IdentityId, identity);
        identity.CharacterChanged += new Action<MyCharacter, MyCharacter>(this.Identity_CharacterChanged);
        if (identity.Character != null)
          identity.Character.CharacterDied += new Action<MyCharacter>(this.Character_CharacterDied);
      }
      if (((!Sync.IsServer ? 0 : (Sync.MyId > 0UL ? 1 : 0)) & (sendToClients ? 1 : 0)) != 0)
        MyMultiplayer.RaiseStaticEvent<bool, long, string>((Func<IMyEventOwner, Action<bool, long, string>>) (s => new Action<bool, long, string>(MyPlayerCollection.OnIdentityCreated)), addToNpcs, identity.IdentityId, identity.DisplayName);
      Action identitiesChanged = this.IdentitiesChanged;
      if (identitiesChanged != null)
        identitiesChanged();
      MySession.Static.Factions.ForceRelationsOnNewIdentity(identity.IdentityId);
    }

    private void Character_CharacterDied(MyCharacter diedCharacter)
    {
      if (this.PlayerCharacterDied == null || diedCharacter == null || diedCharacter.ControllerInfo.ControllingIdentityId == 0L)
        return;
      this.PlayerCharacterDied(diedCharacter.ControllerInfo.ControllingIdentityId);
    }

    private void Identity_CharacterChanged(MyCharacter oldCharacter, MyCharacter newCharacter)
    {
      if (oldCharacter != null)
        oldCharacter.CharacterDied -= new Action<MyCharacter>(this.Character_CharacterDied);
      if (newCharacter == null)
        return;
      newCharacter.CharacterDied += new Action<MyCharacter>(this.Character_CharacterDied);
    }

    private void LoadPlayerInternal(
      ref MyPlayer.PlayerId playerId,
      MyObjectBuilder_Player playerOb,
      bool obsolete = false)
    {
      MyIdentity identity = this.TryGetIdentity(playerOb.IdentityId);
      if (identity == null || obsolete && identity.IsDead)
        return;
      if (Sync.IsServer && (long) Sync.MyId != (long) playerId.SteamId)
        playerOb.Connected = Sync.Clients.HasClient(playerId.SteamId);
      if (playerOb.IsWildlifeAgent)
        playerOb.Connected = true;
      if (!playerOb.IsWildlifeAgent && !playerOb.Connected)
      {
        if (!this.m_playerIdentityIds.ContainsKey(playerId))
        {
          this.m_playerIdentityIds.TryAdd(playerId, playerOb.IdentityId);
          this.m_identityPlayerIds.Add(playerOb.IdentityId, playerId);
        }
        identity.SetDead(true);
      }
      else
      {
        if (!this.InitNewPlayer(playerId, playerOb).IsLocalPlayer)
          return;
        Action<int> localPlayerLoaded = Sync.Players.LocalPlayerLoaded;
        if (localPlayerLoaded == null)
          return;
        localPlayerLoaded(playerId.SerialId);
      }
    }

    public MyPlayer.PlayerId FindFreePlayerId(ulong steamId)
    {
      MyPlayer.PlayerId key = new MyPlayer.PlayerId(steamId);
      while (this.m_playerIdentityIds.ContainsKey(key))
        ++key;
      return key;
    }

    private void player_IdentityChanged(MyPlayer player, MyIdentity identity)
    {
      this.m_identityPlayerIds.Remove(this.m_playerIdentityIds[player.Id]);
      this.m_playerIdentityIds[player.Id] = identity.IdentityId;
      this.m_identityPlayerIds[identity.IdentityId] = player.Id;
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, int, long>((Func<IMyEventOwner, Action<ulong, int, long>>) (s => new Action<ulong, int, long>(MyPlayerCollection.OnPlayerIdentityChanged)), player.Id.SteamId, player.Id.SerialId, identity.IdentityId);
    }

    private string GetPlayerCharacter(MyPlayer player) => player.Identity.Character != null ? player.Identity.Character.Entity.ToString() : "<empty>";

    private string GetControlledEntity(MyPlayer player) => player.Controller.ControlledEntity != null ? player.Controller.ControlledEntity.Entity.ToString() : "<empty>";

    [Conditional("DEBUG")]
    public void WriteDebugInfo()
    {
      StackFrame frame = new StackTrace().GetFrame(1);
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player1 in this.m_players)
      {
        KeyValuePair<MyPlayer.PlayerId, MyPlayer> player = player1;
        bool isLocalPlayer = player.Value.IsLocalPlayer;
        string str = frame.GetMethod().Name + (isLocalPlayer ? "; Control: [L] " : "; Control: ") + player.Value.Id.ToString();
        this.m_controlledEntities.Where<KeyValuePair<long, MyPlayer.PlayerId>>((Func<KeyValuePair<long, MyPlayer.PlayerId>, bool>) (s => s.Value == player.Value.Id)).Select<KeyValuePair<long, MyPlayer.PlayerId>, string>((Func<KeyValuePair<long, MyPlayer.PlayerId>, string>) (s => s.Key.ToString("X"))).ToArray<string>();
      }
      foreach (MyEntity entity in MyEntities.GetEntities())
        ;
    }

    [Conditional("DEBUG")]
    public void DebugDraw()
    {
      int num1 = 0;
      int num2 = num1 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num1 * 13.0)), "Clients:", Color.GreenYellow, 0.5f);
      foreach (MyNetworkClient client in Sync.Clients.GetClients())
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num2++ * 13f), "  Id: " + (object) client.SteamUserId + ", Name: " + client.DisplayName, Color.LightYellow, 0.5f);
      int num3 = num2;
      int num4 = num3 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num3 * 13.0)), "Online players:", Color.GreenYellow, 0.5f);
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num4++ * 13f), "  PlayerId: " + player.Key.ToString() + ", Name: " + player.Value.DisplayName, Color.LightYellow, 0.5f);
      int num5 = num4;
      int num6 = num5 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num5 * 13.0)), "Player identities:", Color.GreenYellow, 0.5f);
      foreach (KeyValuePair<MyPlayer.PlayerId, long> playerIdentityId in this.m_playerIdentityIds)
      {
        MyPlayer myPlayer;
        this.m_players.TryGetValue(playerIdentityId.Key, out myPlayer);
        string str1 = myPlayer == null ? "N.A." : myPlayer.DisplayName;
        MyIdentity myIdentity;
        this.m_allIdentities.TryGetValue(playerIdentityId.Value, out myIdentity);
        Color color = myIdentity == null || myIdentity.IsDead ? Color.Salmon : Color.LightYellow;
        string str2 = myIdentity == null ? "N.A." : myIdentity.DisplayName;
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num6++ * 13f), "  PlayerId: " + playerIdentityId.Key.ToString() + ", Name: " + str1 + "; IdentityId: " + playerIdentityId.Value.ToString() + ", Name: " + str2, color, 0.5f);
      }
      int num7 = num6;
      int num8 = num7 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num7 * 13.0)), "All identities:", Color.GreenYellow, 0.5f);
      foreach (KeyValuePair<long, MyIdentity> allIdentity in this.m_allIdentities)
      {
        bool isDead = allIdentity.Value.IsDead;
        Color color = isDead ? Color.Salmon : Color.LightYellow;
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num8++ * 13f), "  IdentityId: " + allIdentity.Key.ToString() + ", Name: " + allIdentity.Value.DisplayName + ", State: " + (isDead ? "DEAD" : "ALIVE"), color, 0.5f);
      }
      int num9 = num8;
      int num10 = num9 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num9 * 13.0)), "Control:", Color.GreenYellow, 0.5f);
      foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in this.m_controlledEntities)
      {
        MyEntity entity;
        MyEntities.TryGetEntityById(controlledEntity.Key, out entity);
        Color color = entity == null ? Color.Salmon : Color.LightYellow;
        string str1 = entity == null ? "Unknown entity" : entity.ToString();
        string str2 = entity == null ? "N.A." : entity.EntityId.ToString();
        MyPlayer myPlayer;
        this.m_players.TryGetValue(controlledEntity.Value, out myPlayer);
        string str3 = myPlayer == null ? "N.A." : myPlayer.DisplayName;
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num10++ * 13f), "  " + str1 + " controlled by " + str3 + " (entityId = " + str2 + ", playerId = " + controlledEntity.Value.ToString() + ")", color, 0.5f);
      }
      if (MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyShipController controlledEntity) || !(controlledEntity.Parent is MyCubeGrid parent))
        return;
      int num11;
      parent.GridSystems.ControlSystem.DebugDraw((float) (num11 = num10 + 1) * 13f);
    }

    long IMyPlayerCollection.Count => (long) this.m_players.Count;

    void IMyPlayerCollection.GetPlayers(
      List<IMyPlayer> players,
      Func<IMyPlayer, bool> collect)
    {
      foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in this.m_players)
      {
        if (collect == null || collect((IMyPlayer) player.Value))
          players.Add((IMyPlayer) player.Value);
      }
    }

    void IMyPlayerCollection.ExtendControl(
      VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWithControl,
      IMyEntity entityGettingControl)
    {
      Sandbox.Game.Entities.IMyControllableEntity baseEntity = entityWithControl as Sandbox.Game.Entities.IMyControllableEntity;
      MyEntity entityGettingControl1 = entityGettingControl as MyEntity;
      if (baseEntity == null || entityGettingControl1 == null)
        return;
      this.ExtendControl(baseEntity, entityGettingControl1);
    }

    bool IMyPlayerCollection.HasExtendedControl(
      VRage.Game.ModAPI.Interfaces.IMyControllableEntity firstEntity,
      IMyEntity secondEntity)
    {
      Sandbox.Game.Entities.IMyControllableEntity baseEntity = firstEntity as Sandbox.Game.Entities.IMyControllableEntity;
      MyEntity secondEntity1 = secondEntity as MyEntity;
      return baseEntity != null && secondEntity1 != null && this.HasExtendedControl(baseEntity, secondEntity1);
    }

    void IMyPlayerCollection.ReduceControl(
      VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWhichKeepsControl,
      IMyEntity entityWhichLoosesControl)
    {
      Sandbox.Game.Entities.IMyControllableEntity baseEntity = entityWhichKeepsControl as Sandbox.Game.Entities.IMyControllableEntity;
      MyEntity entityWhichLoosesControl1 = entityWhichLoosesControl as MyEntity;
      if (baseEntity == null || entityWhichLoosesControl1 == null)
        return;
      this.ReduceControl(baseEntity, entityWhichLoosesControl1);
    }

    void IMyPlayerCollection.RemoveControlledEntity(IMyEntity entity)
    {
      if (!(entity is MyEntity entity1))
        return;
      this.RemoveControlledEntity(entity1);
    }

    void IMyPlayerCollection.SetControlledEntity(
      ulong steamUserId,
      IMyEntity entity)
    {
      if (!(entity is MyEntity entity1))
        return;
      this.SetControlledEntity(steamUserId, entity1);
    }

    void IMyPlayerCollection.TryExtendControl(
      VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWithControl,
      IMyEntity entityGettingControl)
    {
      Sandbox.Game.Entities.IMyControllableEntity baseEntity = entityWithControl as Sandbox.Game.Entities.IMyControllableEntity;
      MyEntity entityGettingControl1 = entityGettingControl as MyEntity;
      if (baseEntity == null || entityGettingControl1 == null)
        return;
      this.TryExtendControl(baseEntity, entityGettingControl1);
    }

    bool IMyPlayerCollection.TryReduceControl(
      VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWhichKeepsControl,
      IMyEntity entityWhichLoosesControl)
    {
      Sandbox.Game.Entities.IMyControllableEntity baseEntity = entityWhichKeepsControl as Sandbox.Game.Entities.IMyControllableEntity;
      MyEntity entityWhichLoosesControl1 = entityWhichLoosesControl as MyEntity;
      return baseEntity != null && entityWhichLoosesControl1 != null && this.TryReduceControl(baseEntity, entityWhichLoosesControl1);
    }

    IMyPlayer IMyPlayerCollection.GetPlayerControllingEntity(
      IMyEntity entity)
    {
      if (entity is MyEntity entity1)
      {
        MyEntityController entityController = this.GetEntityController(entity1);
        if (entityController != null)
          return (IMyPlayer) entityController.Player;
      }
      return (IMyPlayer) null;
    }

    void IMyPlayerCollection.GetAllIdentites(
      List<IMyIdentity> identities,
      Func<IMyIdentity, bool> collect)
    {
      foreach (KeyValuePair<long, MyIdentity> allIdentity in this.m_allIdentities)
      {
        if (collect == null || collect((IMyIdentity) allIdentity.Value))
          identities.Add((IMyIdentity) allIdentity.Value);
      }
    }

    long IMyPlayerCollection.TryGetIdentityId(ulong steamId) => this.TryGetIdentityId(steamId, 0);

    ulong IMyPlayerCollection.TryGetSteamId(long identityId) => this.TryGetSteamId(identityId);

    void IMyPlayerCollection.RequestChangeBalance(long identityId, long amount)
    {
      if (MyBankingSystem.Static == null)
        return;
      MyBankingSystem.ChangeBalance(identityId, amount);
    }

    [ProtoContract]
    [Serializable]
    public struct NewPlayerRequestParameters
    {
      public ulong ClientSteamId;
      public int PlayerSerialId;
      public string DisplayName;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string CharacterModel;
      public bool RealPlayer;
      public bool InitialPlayer;
      public bool IsWildlifeEntity;

      public NewPlayerRequestParameters(
        ulong clientSteamId,
        int playerSerialId,
        string displayName,
        string characterModel,
        bool realPlayer = false,
        bool initialPlayer = false,
        bool isWildlifeEntity = false)
      {
        this.ClientSteamId = clientSteamId;
        this.PlayerSerialId = playerSerialId;
        this.DisplayName = displayName;
        this.CharacterModel = characterModel;
        this.RealPlayer = realPlayer;
        this.InitialPlayer = initialPlayer;
        this.IsWildlifeEntity = isWildlifeEntity;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EClientSteamId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in ulong value)
        {
          owner.ClientSteamId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out ulong value)
        {
          value = owner.ClientSteamId;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EPlayerSerialId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in int value)
        {
          owner.PlayerSerialId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out int value)
        {
          value = owner.PlayerSerialId;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in string value)
        {
          owner.DisplayName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out string value)
        {
          value = owner.DisplayName;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003ECharacterModel\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in string value)
        {
          owner.CharacterModel = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out string value)
        {
          value = owner.CharacterModel;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003ERealPlayer\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in bool value)
        {
          owner.RealPlayer = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out bool value)
        {
          value = owner.RealPlayer;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EInitialPlayer\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in bool value)
        {
          owner.InitialPlayer = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out bool value)
        {
          value = owner.InitialPlayer;
        }
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EIsWildlifeEntity\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.NewPlayerRequestParameters, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          in bool value)
        {
          owner.IsWildlifeEntity = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.NewPlayerRequestParameters owner,
          out bool value)
        {
          value = owner.IsWildlifeEntity;
        }
      }

      private class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters\u003C\u003EActor : IActivator, IActivator<MyPlayerCollection.NewPlayerRequestParameters>
      {
        object IActivator.CreateInstance() => (object) new MyPlayerCollection.NewPlayerRequestParameters();

        MyPlayerCollection.NewPlayerRequestParameters IActivator<MyPlayerCollection.NewPlayerRequestParameters>.CreateInstance() => new MyPlayerCollection.NewPlayerRequestParameters();
      }
    }

    [Serializable]
    public struct RespawnMsg
    {
      public bool JoinGame;
      public bool NewIdentity;
      public long RespawnEntityId;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string RespawnShipId;
      public int PlayerSerialId;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string ModelName;
      public Color Color;

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003EJoinGame\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in bool value) => owner.JoinGame = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out bool value) => value = owner.JoinGame;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003ENewIdentity\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in bool value) => owner.NewIdentity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out bool value) => value = owner.NewIdentity;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003ERespawnEntityId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in long value) => owner.RespawnEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out long value) => value = owner.RespawnEntityId;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003ERespawnShipId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in string value) => owner.RespawnShipId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out string value) => value = owner.RespawnShipId;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003EPlayerSerialId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in int value) => owner.PlayerSerialId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out int value) => value = owner.PlayerSerialId;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003EModelName\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in string value) => owner.ModelName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out string value) => value = owner.ModelName;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.RespawnMsg, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.RespawnMsg owner, in Color value) => owner.Color = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.RespawnMsg owner, out Color value) => value = owner.Color;
      }
    }

    [ProtoContract]
    public struct AllPlayerData
    {
      [ProtoMember(1)]
      public ulong SteamId;
      [ProtoMember(4)]
      public int SerialId;
      [ProtoMember(7)]
      public MyObjectBuilder_Player Player;

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003EAllPlayerData\u003C\u003ESteamId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.AllPlayerData, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.AllPlayerData owner, in ulong value) => owner.SteamId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.AllPlayerData owner, out ulong value) => value = owner.SteamId;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003EAllPlayerData\u003C\u003ESerialId\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.AllPlayerData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyPlayerCollection.AllPlayerData owner, in int value) => owner.SerialId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyPlayerCollection.AllPlayerData owner, out int value) => value = owner.SerialId;
      }

      protected class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003EAllPlayerData\u003C\u003EPlayer\u003C\u003EAccessor : IMemberAccessor<MyPlayerCollection.AllPlayerData, MyObjectBuilder_Player>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyPlayerCollection.AllPlayerData owner,
          in MyObjectBuilder_Player value)
        {
          owner.Player = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyPlayerCollection.AllPlayerData owner,
          out MyObjectBuilder_Player value)
        {
          value = owner.Player;
        }
      }

      private class Sandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003EAllPlayerData\u003C\u003EActor : IActivator, IActivator<MyPlayerCollection.AllPlayerData>
      {
        object IActivator.CreateInstance() => (object) new MyPlayerCollection.AllPlayerData();

        MyPlayerCollection.AllPlayerData IActivator<MyPlayerCollection.AllPlayerData>.CreateInstance() => new MyPlayerCollection.AllPlayerData();
      }
    }

    public delegate void RespawnRequestedDelegate(
      ref MyPlayerCollection.RespawnMsg respawnMsg,
      MyNetworkClient client);

    protected sealed class OnControlChangedSuccess\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, long, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in long entityId,
        in bool justUpdateClientCache,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnControlChangedSuccess(clientSteamId, playerSerialId, entityId, justUpdateClientCache);
      }
    }

    protected sealed class OnControlReleased\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyPlayerCollection.OnControlReleased(entityId);
      }
    }

    protected sealed class OnIdentityCreated\u003C\u003ESystem_Boolean\u0023System_Int64\u0023System_String : ICallSite<IMyEventOwner, bool, long, string, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool isNpc,
        in long identityId,
        in string displayName,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnIdentityCreated(isNpc, identityId, displayName);
      }
    }

    protected sealed class OnIdentityRemovedRequest\u003C\u003ESystem_Int64\u0023System_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, long, ulong, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in ulong steamId,
        in int serialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnIdentityRemovedRequest(identityId, steamId, serialId);
      }
    }

    protected sealed class OnIdentityRemovedSuccess\u003C\u003ESystem_Int64\u0023System_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, long, ulong, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in ulong steamId,
        in int serialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnIdentityRemovedSuccess(identityId, steamId, serialId);
      }
    }

    protected sealed class OnPlayerIdentityChanged\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in long identityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerIdentityChanged(clientSteamId, playerSerialId, identityId);
      }
    }

    protected sealed class OnRespawnRequestFailure\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int playerSerialId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnRespawnRequestFailure(playerSerialId);
      }
    }

    protected sealed class OnSetPlayerDeadRequest\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in bool isDead,
        in bool resetIdentity,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnSetPlayerDeadRequest(clientSteamId, playerSerialId, isDead, resetIdentity);
      }
    }

    protected sealed class OnSetPlayerDeadSuccess\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in bool isDead,
        in bool resetIdentity,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnSetPlayerDeadSuccess(clientSteamId, playerSerialId, isDead, resetIdentity);
      }
    }

    protected sealed class OnNewPlayerRequest\u003C\u003ESandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ENewPlayerRequestParameters : ICallSite<IMyEventOwner, MyPlayerCollection.NewPlayerRequestParameters, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyPlayerCollection.NewPlayerRequestParameters parameters,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnNewPlayerRequest(parameters);
      }
    }

    protected sealed class OnNewPlayerSuccess\u003C\u003ESystem_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnNewPlayerSuccess(clientSteamId, playerSerialId);
      }
    }

    protected sealed class OnNewPlayerFailure\u003C\u003ESystem_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnNewPlayerFailure(clientSteamId, playerSerialId);
      }
    }

    protected sealed class OnPlayerCreated\u003C\u003ESystem_UInt64\u0023System_Int32\u0023VRage_Game_MyObjectBuilder_Player : ICallSite<IMyEventOwner, ulong, int, MyObjectBuilder_Player, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in MyObjectBuilder_Player playerBuilder,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerCreated(clientSteamId, playerSerialId, playerBuilder);
      }
    }

    protected sealed class OnPlayerRemoveRequest\u003C\u003ESystem_UInt64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in bool removeCharacter,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerRemoveRequest(clientSteamId, playerSerialId, removeCharacter);
      }
    }

    protected sealed class OnPlayerRemoved\u003C\u003ESystem_UInt64\u0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong clientSteamId,
        in int playerSerialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerRemoved(clientSteamId, playerSerialId);
      }
    }

    protected sealed class OnPlayerColorChangedRequest\u003C\u003ESystem_Int32\u0023System_Int32\u0023VRageMath_Vector3 : ICallSite<IMyEventOwner, int, int, Vector3, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int serialId,
        in int colorIndex,
        in Vector3 newColor,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerColorChangedRequest(serialId, colorIndex, newColor);
      }
    }

    protected sealed class OnPlayerColorsChangedRequest\u003C\u003ESystem_Int32\u0023System_Collections_Generic_List`1\u003CVRageMath_Vector3\u003E : ICallSite<IMyEventOwner, int, List<Vector3>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int serialId,
        in List<Vector3> newColors,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnPlayerColorsChangedRequest(serialId, newColors);
      }
    }

    protected sealed class OnNpcIdentityRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyPlayerCollection.OnNpcIdentityRequest();
      }
    }

    protected sealed class OnNpcIdentitySuccess\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identidyId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnNpcIdentitySuccess(identidyId);
      }
    }

    protected sealed class OnIdentityFirstSpawn\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identidyId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnIdentityFirstSpawn(identidyId);
      }
    }

    protected sealed class SetIdentityBlockTypesBuilt\u003C\u003ESandbox_Game_World_MyBlockLimits\u003C\u003EMyTypeLimitData : ICallSite<IMyEventOwner, MyBlockLimits.MyTypeLimitData, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyBlockLimits.MyTypeLimitData limits,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.SetIdentityBlockTypesBuilt(limits);
      }
    }

    protected sealed class SetIdentityGridBlocksBuilt\u003C\u003ESandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u0023System_Int32\u0023System_Int32\u0023System_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, MyBlockLimits.MyGridLimitData, int, int, int, int, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyBlockLimits.MyGridLimitData limits,
        in int pcu,
        in int pcuBuilt,
        in int blocksBuilt,
        in int transferedDelta,
        in DBNull arg6)
      {
        MyPlayerCollection.SetIdentityGridBlocksBuilt(limits, pcu, pcuBuilt, blocksBuilt, transferedDelta);
      }
    }

    protected sealed class SetPCU_Client\u003C\u003ESystem_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, int, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int pcu,
        in int transferedDelta,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.SetPCU_Client(pcu, transferedDelta);
      }
    }

    protected sealed class SetDampeningEntity\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long controlledEntityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.SetDampeningEntity(controlledEntityId);
      }
    }

    protected sealed class ClearDampeningEntity\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long controlledEntityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.ClearDampeningEntity(controlledEntityId);
      }
    }

    protected sealed class SetDampeningEntityClient\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long controlledEntityId,
        in long dampeningEntityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.SetDampeningEntityClient(controlledEntityId, dampeningEntityId);
      }
    }

    protected sealed class OnRespawnRequest\u003C\u003ESandbox_Game_Multiplayer_MyPlayerCollection\u003C\u003ERespawnMsg : ICallSite<IMyEventOwner, MyPlayerCollection.RespawnMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyPlayerCollection.RespawnMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyPlayerCollection.OnRespawnRequest(msg);
      }
    }
  }
}
