// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Replication;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [StaticEventOwner]
  [PreloadRequired]
  public abstract class MyMultiplayerBase : MyMultiplayerMinimalBase, IDisposable
  {
    private static readonly int NUMBER_OF_WRONG_PASSWORD_TRIES_BEFORE_KICK = 3;
    public readonly MySyncLayer SyncLayer;
    private IMyPeer2Peer m_networkService;
    private ulong m_serverId;
    protected LRUCache<string, byte[]> m_voxelMapData;
    private readonly ConcurrentDictionary<ulong, int> m_kickedClients;
    private readonly Dictionary<ulong, int> m_wrongPasswordClients;
    private readonly MyConcurrentHashSet<ulong> m_bannedClients;
    private static readonly List<ulong> m_tmpClientList = new List<ulong>();
    private int m_lastKickUpdate;
    private readonly Dictionary<int, ITransportCallback> m_controlMessageHandlers = new Dictionary<int, ITransportCallback>();
    private readonly Dictionary<Type, MyControlMessageEnum> m_controlMessageTypes = new Dictionary<Type, MyControlMessageEnum>();
    private TimeSpan m_lastSentTimeTimestamp;
    private readonly BitStream m_sendPhysicsStream = new BitStream();
    public const int KICK_TIMEOUT_MS = 300000;
    private float m_serverSimulationRatio = 1f;
    public Action<string> ProfilerDone;

    public bool IsServerExperimental { get; protected set; }

    public MyReplicationLayer ReplicationLayer { get; private set; }

    public ConcurrentDictionary<ulong, int> KickedClients => this.m_kickedClients;

    public MyConcurrentHashSet<ulong> BannedClients => this.m_bannedClients;

    public ulong ServerId { get; protected set; }

    public float ServerSimulationRatio
    {
      get => (float) Math.Round((double) this.m_serverSimulationRatio, 2);
      set => this.m_serverSimulationRatio = value;
    }

    [Event(null, 221)]
    [Server]
    [Reliable]
    public static void OnSetPriorityMultiplier(float priority) => MyMultiplayer.Static.ReplicationLayer.SetPriorityMultiplier(MyEventContext.Current.Sender, priority);

    [Event(null, 226)]
    [Server]
    [Reliable]
    public static void OnSetDebugEntity(long entityId) => MyFakes.VDB_ENTITY = MyEntities.GetEntityById(entityId);

    [Event(null, 231)]
    [Server]
    [Reliable]
    public static void OnCharacterParentChangeTimeOut(double delay) => MyCharacterPhysicsStateGroup.ParentChangeTimeOut = MyTimeSpan.FromMilliseconds(delay);

    [Event(null, 236)]
    [Server]
    [Reliable]
    public static void OnCharacterMaxJetpackGridDistance(float distance)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = distance;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, distance);
    }

    [Event(null, 242)]
    [Server]
    [Reliable]
    public static void OnCharacterMaxJetpackGridDisconnectDistance(float distance)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = distance;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, distance);
    }

    [Event(null, 248)]
    [Server]
    [Reliable]
    public static void OnCharacterMinJetpackGridSpeed(float speed)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = speed;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, speed);
    }

    [Event(null, 254)]
    [Server]
    [Reliable]
    public static void OnCharacterMinJetpackDisconnectGridSpeed(float speed)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = speed;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, speed);
    }

    [Event(null, 260)]
    [Server]
    [Reliable]
    public static void OnCharacterMaxJetpackGridAcceleration(float accel)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = accel;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, accel);
    }

    [Event(null, 266)]
    [Server]
    [Reliable]
    public static void OnCharacterMaxJetpackDisconnectGridAcceleration(float accel)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = accel;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, accel);
    }

    [Event(null, 272)]
    [Server]
    [Reliable]
    public static void OnCharacterMinJetpackInsideGridSpeed(float accel)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = accel;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, accel);
    }

    [Event(null, 278)]
    [Server]
    [Reliable]
    public static void OnCharacterMinJetpackDisconnectInsideGridSpeed(float accel)
    {
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = accel;
      MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, accel);
    }

    public LRUCache<string, byte[]> VoxelMapData => this.m_voxelMapData;

    public uint FrameCounter { get; private set; }

    public abstract string WorldName { get; set; }

    public abstract MyGameModeEnum GameMode { get; set; }

    public abstract float InventoryMultiplier { get; set; }

    public abstract float BlocksInventoryMultiplier { get; set; }

    public abstract float AssemblerMultiplier { get; set; }

    public abstract float RefineryMultiplier { get; set; }

    public abstract float WelderMultiplier { get; set; }

    public abstract float GrinderMultiplier { get; set; }

    public abstract string HostName { get; set; }

    public abstract ulong WorldSize { get; set; }

    public abstract int AppVersion { get; set; }

    public abstract string DataHash { get; set; }

    public abstract int MaxPlayers { get; }

    public abstract int ModCount { get; protected set; }

    public abstract List<MyObjectBuilder_Checkpoint.ModItem> Mods { get; set; }

    public abstract int ViewDistance { get; set; }

    public abstract bool Scenario { get; set; }

    public abstract string ScenarioBriefing { get; set; }

    public abstract DateTime ScenarioStartTime { get; set; }

    public virtual int SyncDistance { get; set; }

    public abstract bool ExperimentalMode { get; set; }

    public event Action<ulong, string> ClientJoined;

    public event Action<ulong, MyChatMemberStateChangeEnum> ClientLeft;

    public event Action HostLeft;

    public event Action<ulong, string, ChatChannel, long, string> ChatMessageReceived;

    public event Action<string, string, string, Color> ScriptedChatMessageReceived;

    public event Action<ulong> ClientKicked;

    public event Action PendingReplicablesDone;

    public event Action LocalRespawnRequested;

    internal MyMultiplayerBase(MySyncLayer syncLayer)
    {
      this.SyncLayer = syncLayer;
      this.IsConnectionDirect = true;
      this.IsConnectionAlive = true;
      this.m_kickedClients = new ConcurrentDictionary<ulong, int>();
      this.m_bannedClients = new MyConcurrentHashSet<ulong>();
      this.m_wrongPasswordClients = new Dictionary<ulong, int>();
      this.m_lastKickUpdate = MySandboxGame.TotalTimeInMilliseconds;
      MyNetworkMonitor.StartSession();
      MyNetworkReader.SetHandler(0, new NetworkMessageDelegate(this.ControlMessageReceived), new Action<ulong>(this.DisconnectClient));
      this.RegisterControlMessage<MyControlKickClientMsg>(MyControlMessageEnum.Kick, new ControlMessageHandler<MyControlKickClientMsg>(this.OnClientKick), MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
      this.RegisterControlMessage<MyControlDisconnectedMsg>(MyControlMessageEnum.Disconnected, new ControlMessageHandler<MyControlDisconnectedMsg>(this.OnDisconnectedClient), MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
      this.RegisterControlMessage<MyControlBanClientMsg>(MyControlMessageEnum.Ban, new ControlMessageHandler<MyControlBanClientMsg>(this.OnClientBan), MyMessagePermissions.FromServer | MyMessagePermissions.ToServer);
      this.RegisterControlMessage<MyControlSendPasswordHashMsg>(MyControlMessageEnum.SendPasswordHash, new ControlMessageHandler<MyControlSendPasswordHashMsg>(this.OnPasswordHash), MyMessagePermissions.ToServer);
      syncLayer.TransportLayer.DisconnectPeerOnError = new Action<ulong>(this.DisconnectClient);
      this.ClientKicked += new Action<ulong>(this.KickClient);
    }

    private void KickClient(ulong client) => this.KickClient(client, add: false);

    protected virtual void OnPasswordHash(ref MyControlSendPasswordHashMsg message, ulong sender)
    {
    }

    protected void SetReplicationLayer(MyReplicationLayer layer)
    {
      this.ReplicationLayer = this.ReplicationLayer == null ? layer : throw new InvalidOperationException("Replication layer already set");
      this.ReplicationLayer.RegisterFromGameAssemblies();
    }

    public bool IsConnectionDirect { get; protected set; }

    public bool IsConnectionAlive { get; protected set; }

    public DateTime LastMessageReceived => MyMultiplayer.ReplicationLayer.LastMessageFromServer;

    internal void RegisterControlMessage<T>(
      MyControlMessageEnum msg,
      ControlMessageHandler<T> handler,
      MyMessagePermissions permission)
      where T : struct
    {
      MyControlMessageCallback<T> controlMessageCallback = new MyControlMessageCallback<T>(handler, MySyncLayer.GetSerializer<T>(), permission);
      this.m_controlMessageHandlers.Add((int) msg, (ITransportCallback) controlMessageCallback);
      this.m_controlMessageTypes.Add(typeof (T), msg);
    }

    private void ControlMessageReceived(MyPacket p)
    {
      ITransportCallback transportCallback;
      if (this.m_controlMessageHandlers.TryGetValue((int) (byte) p.ByteStream.ReadUShort(), out transportCallback))
        transportCallback.Receive(p.ByteStream, p.Sender.Id.Value);
      p.Return();
    }

    protected void SendControlMessage<T>(ulong user, ref T message, bool reliable = true) where T : struct
    {
      MyControlMessageEnum controlMessageEnum;
      this.m_controlMessageTypes.TryGetValue(typeof (T), out controlMessageEnum);
      ITransportCallback transportCallback;
      this.m_controlMessageHandlers.TryGetValue((int) controlMessageEnum, out transportCallback);
      MyControlMessageCallback<T> controlMessageCallback = (MyControlMessageCallback<T>) transportCallback;
      if (!MySyncLayer.CheckSendPermissions(user, controlMessageCallback.Permission))
        return;
      MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.GetPacketDescriptor(new EndpointId(user), reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, 0);
      packetDescriptor.Header.WriteUShort((ushort) controlMessageEnum);
      controlMessageCallback.Write(packetDescriptor.Header, ref message);
      MyNetworkWriter.SendPacket(packetDescriptor);
    }

    internal void SendControlMessageToAll<T>(ref T message, ulong exceptUserId = 0) where T : struct
    {
      foreach (ulong member in this.Members)
      {
        if ((long) member != (long) Sync.MyId && (long) member != (long) exceptUserId)
          this.SendControlMessage<T>(member, ref message);
      }
    }

    protected abstract void OnClientKick(ref MyControlKickClientMsg data, ulong sender);

    protected abstract void OnClientBan(ref MyControlBanClientMsg data, ulong sender);

    public virtual void OnChatMessage(ref ChatMsg msg)
    {
    }

    protected void OnScriptedChatMessage(ref ScriptedChatMsg msg) => this.RaiseScriptedChatMessageReceived(msg.Author, msg.Text, msg.Font, msg.Color);

    private void OnDisconnectedClient(ref MyControlDisconnectedMsg data, ulong sender)
    {
      this.RaiseClientLeft(data.Client, MyChatMemberStateChangeEnum.Disconnected);
      MyLog.Default.WriteLineAndConsole("Disconnected: " + EndpointId.Format(sender));
    }

    public virtual void DownloadWorld(int appVersion)
    {
    }

    public virtual void DownloadProfiler()
    {
    }

    public abstract void DisconnectClient(ulong userId);

    public abstract void KickClient(ulong userId, bool kicked = true, bool add = true);

    public abstract void BanClient(ulong userId, bool banned);

    protected void AddWrongPasswordClient(ulong userId)
    {
      int num1;
      if (this.m_wrongPasswordClients.TryGetValue(userId, out num1))
      {
        int num2 = num1 + 1;
        this.m_wrongPasswordClients[userId] = num2;
      }
      else
        this.m_wrongPasswordClients[userId] = 1;
    }

    protected bool IsOutOfWrongPasswordTries(ulong userId)
    {
      int num;
      return this.m_wrongPasswordClients.TryGetValue(userId, out num) && num >= MyMultiplayerBase.NUMBER_OF_WRONG_PASSWORD_TRIES_BEFORE_KICK;
    }

    protected void ResetWrongPasswordCounter(ulong userId)
    {
      if (!this.m_wrongPasswordClients.ContainsKey(userId))
        return;
      this.m_wrongPasswordClients.Remove(userId);
    }

    protected void AddKickedClient(ulong userId)
    {
      if (this.m_kickedClients.TryAdd(userId, MySandboxGame.TotalTimeInMilliseconds))
        return;
      MySandboxGame.Log.WriteLine("Trying to kick player who was already kicked!");
    }

    protected void RemoveKickedClient(ulong userId) => this.m_kickedClients.Remove<ulong, int>(userId);

    protected void AddBannedClient(ulong userId)
    {
      if (this.m_bannedClients.Contains(userId))
        MySandboxGame.Log.WriteLine("Trying to ban player who was already banned!");
      else
        this.m_bannedClients.Add(userId);
    }

    protected void RemoveBannedClient(ulong userId) => this.m_bannedClients.Remove(userId);

    protected bool IsClientKickedOrBanned(ulong userId) => this.m_kickedClients.ContainsKey(userId) || this.m_bannedClients.Contains(userId);

    protected bool IsClientKicked(ulong userId) => this.m_kickedClients.ContainsKey(userId);

    protected bool IsClientBanned(ulong userId) => this.m_bannedClients.Contains(userId);

    public void StartProcessingClientMessages() => this.SyncLayer.TransportLayer.IsBuffering = false;

    public virtual void StartProcessingClientMessagesWithEmptyWorld() => this.StartProcessingClientMessages();

    public void ReportReplicatedObjects()
    {
      if (!VRage.Profiler.MyRenderProfiler.ProfilerVisible)
        return;
      this.ReplicationLayer.ReportReplicatedObjects();
    }

    public virtual void Tick()
    {
      ++this.FrameCounter;
      if (this.IsServer && (MySession.Static.ElapsedGameTime - this.m_lastSentTimeTimestamp).Seconds > 30)
      {
        this.m_lastSentTimeTimestamp = MySession.Static.ElapsedGameTime;
        MyMultiplayerBase.SendElapsedGameTime();
      }
      int timeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
      if (timeInMilliseconds - this.m_lastKickUpdate > 20000)
      {
        MyMultiplayerBase.m_tmpClientList.Clear();
        foreach (ulong key in (IEnumerable<ulong>) this.m_kickedClients.Keys)
          MyMultiplayerBase.m_tmpClientList.Add(key);
        foreach (ulong tmpClient in MyMultiplayerBase.m_tmpClientList)
        {
          if (timeInMilliseconds - this.m_kickedClients[tmpClient] > 300000)
          {
            this.m_kickedClients.Remove<ulong, int>(tmpClient);
            if (this.m_wrongPasswordClients.ContainsKey(tmpClient))
              this.m_wrongPasswordClients.Remove(tmpClient);
          }
        }
        MyMultiplayerBase.m_tmpClientList.Clear();
        this.m_lastKickUpdate = timeInMilliseconds;
      }
      this.ReplicationLayer.SendUpdate();
    }

    public abstract void SendChatMessage(
      string text,
      ChatChannel channel,
      long targetId = 0,
      string customAuthor = null);

    public virtual void Dispose()
    {
      MyNetworkMonitor.EndSession();
      this.m_voxelMapData = (LRUCache<string, byte[]>) null;
      MyNetworkReader.ClearHandler(0);
      this.SyncLayer.TransportLayer.Clear();
      MyNetworkReader.Clear();
      this.m_sendPhysicsStream.Dispose();
      this.ReplicationLayer.Dispose();
      this.ClientKicked -= new Action<ulong>(this.KickClient);
      MyMultiplayer.Static = (MyMultiplayerBase) null;
    }

    public abstract IEnumerable<ulong> Members { get; }

    public abstract int MemberCount { get; }

    public abstract bool IsSomeoneElseConnected { get; }

    public abstract string GetMemberServiceName(ulong steamUserID);

    public abstract string GetMemberName(ulong steamUserID);

    protected void RaiseChatMessageReceived(
      ulong steamUserID,
      string messageText,
      ChatChannel channel,
      long targetId,
      string customAuthorName = null)
    {
      if (MyGameService.GetPlayerMutedState(steamUserID) == MyPlayerChatState.Muted || channel != ChatChannel.GlobalScripted && channel != ChatChannel.ChatBot && !MyGameService.Networking.Chat.IsTextChatAvailableForUserId(steamUserID, this.IsCrossMember(steamUserID)))
        return;
      this.ChatMessageReceived.InvokeIfNotNull<ulong, string, ChatChannel, long, string>(steamUserID, messageText, channel, targetId, customAuthorName);
      MyAPIUtilities.Static.RecieveMessage(steamUserID, messageText);
    }

    public bool IsCrossMember(ulong steamUserID) => this.GetMemberServiceName(steamUserID) != MyGameService.Service.ServiceName;

    private void RaiseScriptedChatMessageReceived(
      string author,
      string messageText,
      string font,
      Color color)
    {
      Action<string, string, string, Color> chatMessageReceived = this.ScriptedChatMessageReceived;
      if (chatMessageReceived == null)
        return;
      chatMessageReceived(messageText, author, font, color);
    }

    protected void RaiseHostLeft()
    {
      Action hostLeft = this.HostLeft;
      if (hostLeft == null)
        return;
      hostLeft();
    }

    protected void RaiseClientLeft(ulong changedUser, MyChatMemberStateChangeEnum stateChange)
    {
      Action<ulong, MyChatMemberStateChangeEnum> clientLeft = this.ClientLeft;
      if (clientLeft == null)
        return;
      clientLeft(changedUser, stateChange);
    }

    protected void RaiseClientJoined(ulong changedUser, string userName)
    {
      Action<ulong, string> clientJoined = this.ClientJoined;
      if (clientJoined == null)
        return;
      clientJoined(changedUser, userName);
    }

    protected void RaiseClientKicked(ulong user)
    {
      Action<ulong> clientKicked = this.ClientKicked;
      if (clientKicked == null)
        return;
      clientKicked(user);
    }

    public abstract ulong LobbyId { get; }

    public abstract ulong GetOwner();

    public abstract int MemberLimit { get; set; }

    public virtual bool IsLobby => false;

    public bool IsTextChatAvailable => MyGameService.IsTextChatAvailable;

    public bool IsVoiceChatAvailable => MyGameService.IsVoiceChatAvailable;

    public abstract MyLobbyType GetLobbyType();

    public abstract void SetLobbyType(MyLobbyType type);

    public abstract void SetMemberLimit(int limit);

    protected void CloseMemberSessions()
    {
      foreach (ulong member in this.Members)
      {
        if ((long) member != (long) Sync.MyId && (long) member == (long) this.ServerId)
          MyGameService.Peer2Peer.CloseSession(member);
      }
    }

    protected void SendAllMembersDataToClient(ulong clientId)
    {
      AllMembersDataMsg allMembersDataMsg = new AllMembersDataMsg();
      if (Sync.Players != null)
      {
        allMembersDataMsg.Identities = Sync.Players.SaveIdentities();
        allMembersDataMsg.Players = Sync.Players.SavePlayers(MySession.Static.RemoteAdminSettings, MySession.Static.PromotedUsers, MySession.Static.CreativeTools);
      }
      if (MySession.Static.Factions != null)
        allMembersDataMsg.Factions = MySession.Static.Factions.SaveFactions();
      allMembersDataMsg.Clients = MySession.Static.SaveMembers(true);
      MyMultiplayer.RaiseStaticEvent<AllMembersDataMsg>((Func<IMyEventOwner, Action<AllMembersDataMsg>>) (s => new Action<AllMembersDataMsg>(MyMultiplayerBase.OnAllMembersReceived)), allMembersDataMsg, new EndpointId(clientId));
    }

    protected virtual void OnAllMembersData(ref AllMembersDataMsg msg)
    {
    }

    protected void ProcessAllMembersData(ref AllMembersDataMsg msg)
    {
      Sync.Players.ClearIdentities();
      if (msg.Identities != null)
        Sync.Players.LoadIdentities(msg.Identities);
      Sync.Players.ClearPlayers();
      if (msg.Players != null)
        Sync.Players.LoadPlayers(msg.Players);
      MySession.Static.Factions.LoadFactions(msg.Factions);
    }

    protected MyClientState CreateClientState() => Activator.CreateInstance(MyPerGameSettings.ClientStateType) as MyClientState;

    private static void SendElapsedGameTime() => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyMultiplayerBase.OnElapsedGameTime)), MySession.Static.ElapsedGameTime.Ticks);

    [Event(null, 781)]
    [Broadcast]
    private static void OnElapsedGameTime(long elapsedGameTicks) => MySession.Static.ElapsedGameTime = new TimeSpan(elapsedGameTicks);

    protected static void SendChatMessage(ref ChatMsg msg) => MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_Server)), msg);

    public static void SendScriptedChatMessage(ref ScriptedChatMsg msg) => MyMultiplayer.RaiseStaticEvent<ScriptedChatMsg>((Func<IMyEventOwner, Action<ScriptedChatMsg>>) (s => new Action<ScriptedChatMsg>(MyMultiplayerBase.OnScriptedChatMessageReceived)), msg);

    [Event(null, 797)]
    [Reliable]
    [Client]
    private static void OnAllMembersReceived(AllMembersDataMsg msg) => MyMultiplayer.Static.OnAllMembersData(ref msg);

    [Event(null, 803)]
    [Reliable]
    [Server]
    private static void OnChatMessageReceived_Server(ChatMsg msg)
    {
      string playerName = MyEventContext.Current.Sender.ToString();
      string str = msg.TargetId.ToString();
      switch (msg.Channel)
      {
        case 0:
          playerName = MyMultiplayerBase.GetPlayerName(MyEventContext.Current.Sender.Value);
          str = "everyone";
          MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_BroadcastExcept)), msg, MyEventContext.Current.Sender);
          break;
        case 1:
          if (msg.TargetId <= 0L)
          {
            MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_BroadcastExcept)), msg);
            break;
          }
          ulong steamId1 = MySession.Static.Players.TryGetSteamId(msg.TargetId);
          if (steamId1 != 0UL)
          {
            playerName = MyMultiplayerBase.GetPlayerName(MyEventContext.Current.Sender.Value);
            str = MyMultiplayerBase.GetPlayerName(msg.TargetId);
            MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_SingleTarget)), msg, new EndpointId(steamId1));
            break;
          }
          break;
        case 2:
          IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(msg.TargetId);
          if (factionById != null)
          {
            playerName = MyMultiplayerBase.GetPlayerName(MyEventContext.Current.Sender.Value);
            str = factionById.Tag;
            using (Dictionary<long, MyFactionMember>.Enumerator enumerator = factionById.Members.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<long, MyFactionMember> current = enumerator.Current;
                if (MySession.Static.Players.IsPlayerOnline(current.Value.PlayerId))
                {
                  ulong steamId2 = MySession.Static.Players.TryGetSteamId(current.Value.PlayerId);
                  if (steamId2 != 0UL && (long) steamId2 != (long) MyEventContext.Current.Sender.Value)
                    MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_SingleTarget)), msg, new EndpointId(steamId2));
                }
              }
              break;
            }
          }
          else
            break;
        case 3:
          ulong steamId3 = MySession.Static.Players.TryGetSteamId(msg.TargetId);
          if (steamId3 != 0UL && (long) steamId3 != (long) MyEventContext.Current.Sender.Value)
          {
            playerName = MyMultiplayerBase.GetPlayerName(MyEventContext.Current.Sender.Value);
            str = MyMultiplayerBase.GetPlayerName(msg.TargetId);
            MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_SingleTarget)), msg, new EndpointId(steamId3));
            break;
          }
          break;
        case 4:
          playerName = MyMultiplayerBase.GetPlayerName(MyEventContext.Current.Sender.Value);
          str = MyMultiplayerBase.GetPlayerName(msg.TargetId);
          break;
        default:
          throw new InvalidBranchException("Unknown channel " + (object) (ChatChannel) msg.Channel);
      }
      MyMultiplayer.Static.OnChatMessage(ref msg);
      if (!Sandbox.Engine.Platform.Game.IsDedicated || !MySandboxGame.ConfigDedicated.SaveChatToLog)
        return;
      MyLog.Default.WriteLine("CHAT - channel: [" + ((ChatChannel) msg.Channel).ToString() + "], from [" + playerName + "] to [" + str + "], message: '" + msg.Text + "'");
    }

    private static string GetPlayerName(ulong steamId)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(steamId, 0);
      return identityId == 0L ? steamId.ToString() : MyMultiplayerBase.GetPlayerName(identityId);
    }

    private static string GetPlayerName(long identityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      return identity == null ? identityId.ToString() : identity.DisplayName;
    }

    [Event(null, 910)]
    [Reliable]
    [Client]
    private static void OnChatMessageReceived_SingleTarget(ChatMsg msg) => MyMultiplayer.Static.OnChatMessage(ref msg);

    [Event(null, 916)]
    [Reliable]
    [BroadcastExcept]
    private static void OnChatMessageReceived_BroadcastExcept(ChatMsg msg) => MyMultiplayer.Static.OnChatMessage(ref msg);

    [Event(null, 922)]
    [Reliable]
    [Client]
    protected static void OnChatMessageReceived_ToPlayer(ChatMsg msg) => MyMultiplayer.Static.OnChatMessage(ref msg);

    [Event(null, 928)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnScriptedChatMessageReceived(ScriptedChatMsg msg)
    {
      if (MySession.Static == null || msg.Target != 0L && MySession.Static.LocalPlayerId != msg.Target)
        return;
      MyMultiplayer.Static.OnScriptedChatMessage(ref msg);
    }

    [Event(null, 940)]
    [Reliable]
    [Server]
    public static void InvalidateVoxelCache(string storageName) => MyMultiplayer.GetReplicationServer().InvalidateSingleClientCache(storageName, MyEventContext.Current.Sender);

    public abstract void OnSessionReady();

    public void ReceivePendingReplicablesDone() => this.PendingReplicablesDone.InvokeIfNotNull();

    public void InvokeLocalRespawnRequested() => this.LocalRespawnRequested.InvokeIfNotNull();

    protected struct MyConnectedClientData
    {
      public string Name;
      public bool IsAdmin;
      public bool IsProfiling;
      public string ServiceName;
    }

    protected sealed class OnSetPriorityMultiplier\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float priority,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnSetPriorityMultiplier(priority);
      }
    }

    protected sealed class OnSetDebugEntity\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyMultiplayerBase.OnSetDebugEntity(entityId);
      }
    }

    protected sealed class OnCharacterParentChangeTimeOut\u003C\u003ESystem_Double : ICallSite<IMyEventOwner, double, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in double delay,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterParentChangeTimeOut(delay);
      }
    }

    protected sealed class OnCharacterMaxJetpackGridDistance\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float distance,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMaxJetpackGridDistance(distance);
      }
    }

    protected sealed class OnCharacterMaxJetpackGridDisconnectDistance\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float distance,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMaxJetpackGridDisconnectDistance(distance);
      }
    }

    protected sealed class OnCharacterMinJetpackGridSpeed\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float speed,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMinJetpackGridSpeed(speed);
      }
    }

    protected sealed class OnCharacterMinJetpackDisconnectGridSpeed\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float speed,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMinJetpackDisconnectGridSpeed(speed);
      }
    }

    protected sealed class OnCharacterMaxJetpackGridAcceleration\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float accel,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMaxJetpackGridAcceleration(accel);
      }
    }

    protected sealed class OnCharacterMaxJetpackDisconnectGridAcceleration\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float accel,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMaxJetpackDisconnectGridAcceleration(accel);
      }
    }

    protected sealed class OnCharacterMinJetpackInsideGridSpeed\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float accel,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMinJetpackInsideGridSpeed(accel);
      }
    }

    protected sealed class OnCharacterMinJetpackDisconnectInsideGridSpeed\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float accel,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnCharacterMinJetpackDisconnectInsideGridSpeed(accel);
      }
    }

    protected sealed class OnElapsedGameTime\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long elapsedGameTicks,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnElapsedGameTime(elapsedGameTicks);
      }
    }

    protected sealed class OnAllMembersReceived\u003C\u003ESandbox_Engine_Multiplayer_AllMembersDataMsg : ICallSite<IMyEventOwner, AllMembersDataMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in AllMembersDataMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnAllMembersReceived(msg);
      }
    }

    protected sealed class OnChatMessageReceived_Server\u003C\u003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ChatMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnChatMessageReceived_Server(msg);
      }
    }

    protected sealed class OnChatMessageReceived_SingleTarget\u003C\u003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ChatMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnChatMessageReceived_SingleTarget(msg);
      }
    }

    protected sealed class OnChatMessageReceived_BroadcastExcept\u003C\u003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ChatMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnChatMessageReceived_BroadcastExcept(msg);
      }
    }

    protected sealed class OnChatMessageReceived_ToPlayer\u003C\u003EVRage_Network_ChatMsg : ICallSite<IMyEventOwner, ChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ChatMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnChatMessageReceived_ToPlayer(msg);
      }
    }

    protected sealed class OnScriptedChatMessageReceived\u003C\u003ESandbox_Engine_Multiplayer_ScriptedChatMsg : ICallSite<IMyEventOwner, ScriptedChatMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ScriptedChatMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.OnScriptedChatMessageReceived(msg);
      }
    }

    protected sealed class InvalidateVoxelCache\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string storageName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerBase.InvalidateVoxelCache(storageName);
      }
    }
  }
}
