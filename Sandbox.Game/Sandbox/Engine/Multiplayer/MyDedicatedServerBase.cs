// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyDedicatedServerBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Multiplayer
{
  public abstract class MyDedicatedServerBase : MyMultiplayerServerBase
  {
    protected string m_worldName;
    protected MyGameModeEnum m_gameMode;
    protected string m_hostName;
    protected ulong m_worldSize;
    protected int m_appVersion = (int) MyFinalBuildConstants.APP_VERSION;
    protected int m_membersLimit;
    protected string m_dataHash;
    protected ulong m_groupId;
    private readonly Func<string, string> m_filterOffensive;
    private readonly List<ulong> m_members = new List<ulong>();
    private readonly Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData> m_memberData = new Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData>();
    private bool m_gameServerDataDirty;
    private readonly Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData> m_pendingMembers = new Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData>();
    private readonly HashSet<ulong> m_waitingForGroup = new HashSet<ulong>();
    private int m_modCount;
    private List<MyObjectBuilder_Checkpoint.ModItem> m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
    private const string STEAM_ID_PREFIX = "STEAM_";
    private const ulong STEAM_ID_MAGIC_CONSTANT = 76561197960265728;

    protected override bool IsServerInternal => true;

    public bool ServerStarted { get; private set; }

    public bool HasServerResponded { get; private set; }

    public string ServerInitError { get; private set; }

    public override string WorldName
    {
      get => this.m_worldName;
      set
      {
        this.m_worldName = string.IsNullOrEmpty(value) ? "noname" : value;
        this.m_gameServerDataDirty = true;
      }
    }

    public override MyGameModeEnum GameMode
    {
      get => this.m_gameMode;
      set => this.m_gameMode = value;
    }

    public override string HostName
    {
      get => this.m_hostName;
      set => this.m_hostName = value;
    }

    public override ulong WorldSize
    {
      get => this.m_worldSize;
      set => this.m_worldSize = value;
    }

    public override int AppVersion
    {
      get => this.m_appVersion;
      set => this.m_appVersion = value;
    }

    public override string DataHash
    {
      get => this.m_dataHash;
      set => this.m_dataHash = value;
    }

    public override int MaxPlayers => 1024;

    public override int ModCount
    {
      get => this.m_modCount;
      protected set
      {
        this.m_modCount = value;
        MyGameService.GameServer.SetKeyValue("mods", this.m_modCount.ToString());
      }
    }

    public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
    {
      get => this.m_mods;
      set
      {
        this.m_mods = value;
        this.ModCount = this.m_mods.Count;
      }
    }

    public override int ViewDistance { get; set; }

    public override int SyncDistance
    {
      get => MyLayers.GetSyncDistance();
      set => MyLayers.SetSyncDistance(value);
    }

    public bool IsPasswordProtected { get; set; }

    private static string ConvertSteamIDFrom64(ulong from)
    {
      from -= 76561197960265728UL;
      return new StringBuilder("STEAM_").Append("0:").Append(from % 2UL).Append(':').Append(from / 2UL).ToString();
    }

    private static ulong ConvertSteamIDTo64(string from)
    {
      string[] strArray = from.Replace("STEAM_", "").Split(':');
      return strArray.Length != 3 ? 0UL : (ulong) (76561197960265728L + (long) Convert.ToUInt64(strArray[1]) + (long) Convert.ToUInt64(strArray[2]) * 2L);
    }

    protected MyDedicatedServerBase(MySyncLayer syncLayer, Func<string, string> filterOffensive)
      : base(syncLayer, new EndpointId(Sync.MyId))
    {
      syncLayer.TransportLayer.Register(MyMessageId.CLIENT_CONNECTED, byte.MaxValue, new Action<MyPacket>(this.ClientConnected));
      this.m_filterOffensive = filterOffensive;
    }

    protected void Initialize(IPEndPoint serverEndpoint)
    {
      this.m_groupId = MySandboxGame.ConfigDedicated.GroupID;
      this.ServerStarted = false;
      this.SetMemberLimit(this.MaxPlayers);
      MyGameService.Peer2Peer.SessionRequest += new Action<ulong>(this.Peer2Peer_SessionRequest);
      MyGameService.Peer2Peer.ConnectionFailed += new Action<ulong, string>(this.Peer2Peer_ConnectionFailed);
      this.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.MyDedicatedServer_ClientLeft);
      MyGameService.GameServer.PlatformConnected += new Action(this.GameServer_ServersConnected);
      MyGameService.GameServer.PlatformConnectionFailed += new Action<string>(this.GameServer_ServersConnectFailure);
      MyGameService.GameServer.PlatformDisconnected += new Action<string>(this.GameServer_ServersDisconnected);
      MyGameService.GameServer.PolicyResponse += new Action<sbyte>(this.GameServer_PolicyResponse);
      MyGameService.GameServer.ValidateAuthTicketResponse += new Action<ulong, JoinResult, ulong, string>(this.GameServer_ValidateAuthTicketResponse);
      MyGameService.GameServer.UserGroupStatusResponse += new Action<ulong, ulong, bool, bool>(this.GameServer_UserGroupStatus);
      string serverName = MySandboxGame.ConfigDedicated.ServerName;
      this.HostName = serverName;
      if (string.IsNullOrWhiteSpace(serverName))
        serverName = "Unnamed server";
      MyGameService.GameServer.SetServerName(serverName);
      int num = MyGameService.GameServer.Start(serverEndpoint, (ushort) MySandboxGame.ConfigDedicated.SteamPort, MyFinalBuildConstants.APP_VERSION.ToString()) ? 1 : 0;
      MyGameService.Peer2Peer.SetServer(true);
      if (num == 0)
        return;
      MyGameService.GameServer.SetModDir(MyPerGameSettings.SteamGameServerGameDir);
      MyGameService.GameServer.GameDescription = MyPerGameSettings.SteamGameServerDescription;
      MyGameService.GameServer.SetDedicated(true);
      if (!string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordHash) && !string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordSalt))
      {
        MyGameService.GameServer.SetPasswordProtected(true);
        this.IsPasswordProtected = true;
      }
      MyGameService.GameServer.LogOnAnonymous();
      MyGameService.GameServer.EnableHeartbeats(true);
      if (this.m_groupId != 0UL && MyGameService.GetServerAccountType(this.m_groupId) != MyGameServiceAccountType.Clan)
        MyLog.Default.WriteLineAndConsole("Specified group ID is invalid: " + (object) this.m_groupId);
      if (!MyGameService.GameServer.WaitStart(10000))
      {
        MyLog.Default.WriteLineAndConsole("Error: No IP assigned.");
      }
      else
      {
        MyGameService.UserId = MyGameService.GameServer.ServerId;
        uint publicIp = MyGameService.GameServer.GetPublicIP();
        IPAddress ipAddress = IPAddressExtensions.FromIPv4NetworkOrder(publicIp);
        this.ServerId = MyGameService.GameServer.ServerId;
        this.ReplicationLayer.SetLocalEndpoint(new EndpointId(MyGameService.GameServer.ServerId));
        this.m_members.Add(this.ServerId);
        this.MemberDataAdd(this.ServerId, new MyMultiplayerBase.MyConnectedClientData()
        {
          Name = MyTexts.GetString(MySpaceTexts.ChatBotName),
          IsAdmin = true
        });
        this.SyncLayer.RegisterClientEvents((MyMultiplayerBase) this);
        MyLog.Default.WriteLineAndConsole("Networking service: " + MyGameService.Networking.ServiceName);
        MyLog.Default.WriteLineAndConsole("Server successfully started");
        MyLog.Default.WriteLineAndConsole("Product name: " + MyGameService.Networking.ProductName);
        MyLog.Default.WriteLineAndConsole("Desc: " + MyGameService.GameServer.GameDescription);
        MyLog.Default.WriteLineAndConsole("Public IP: " + (publicIp == 0U ? "Undisclosed" : ipAddress.ToString()));
        MyLog.Default.WriteLineAndConsole("User ID: " + EndpointId.Format(MyGameService.UserId));
        for (int index = 1000; !this.HasServerResponded && index > 0; --index)
        {
          MyGameService.Update();
          Thread.Sleep(100);
        }
        this.ServerStarted = true;
      }
    }

    private void MemberDataAdd(ulong steamId, MyMultiplayerBase.MyConnectedClientData data)
    {
      this.m_memberData.Add(steamId, data);
      this.m_gameServerDataDirty = true;
    }

    private void MemberDataRemove(ulong steamId)
    {
      this.m_memberData.Remove(steamId);
      this.m_gameServerDataDirty = true;
    }

    protected bool MemberDataGet(ulong steamId, out MyMultiplayerBase.MyConnectedClientData data) => this.m_memberData.TryGetValue(steamId, out data);

    protected void MemberDataSet(ulong steamId, MyMultiplayerBase.MyConnectedClientData data)
    {
      this.m_memberData[steamId] = data;
      this.m_gameServerDataDirty = true;
    }

    internal abstract void SendGameTagsToSteam();

    protected abstract void SendServerData();

    private void Peer2Peer_SessionRequest(ulong remoteUserId)
    {
      MyLog.Default.WriteLineAndConsole("Peer2Peer_SessionRequest " + EndpointId.Format(remoteUserId));
      MyGameService.Peer2Peer.AcceptSession(remoteUserId);
    }

    private void Peer2Peer_ConnectionFailed(ulong remoteUserId, string error)
    {
      MyLog.Default.WriteLineAndConsole("Peer2Peer_ConnectionFailed " + EndpointId.Format(remoteUserId) + ", " + error);
      MySandboxGame.Static.Invoke((Action) (() => this.RaiseClientLeft(remoteUserId, MyChatMemberStateChangeEnum.Disconnected)), "RaiseClientLeft");
    }

    private void MyDedicatedServer_ClientLeft(ulong user, MyChatMemberStateChangeEnum arg2)
    {
      MyGameService.Peer2Peer.CloseSession(user);
      MyLog.Default.WriteLineAndConsole("User left " + this.GetMemberName(user));
      if (this.m_members.Contains(user))
        this.m_members.Remove(user);
      if (this.m_pendingMembers.ContainsKey(user))
        this.m_pendingMembers.Remove(user);
      if (this.m_waitingForGroup.Contains(user))
        this.m_waitingForGroup.Remove(user);
      if (arg2 != MyChatMemberStateChangeEnum.Kicked && arg2 != MyChatMemberStateChangeEnum.Banned)
      {
        foreach (ulong member in this.m_members)
        {
          if ((long) member != (long) this.ServerId)
          {
            MyControlDisconnectedMsg message = new MyControlDisconnectedMsg()
            {
              Client = user
            };
            this.SendControlMessage<MyControlDisconnectedMsg>(member, ref message);
          }
        }
      }
      MyGameService.GameServer.SendUserDisconnect(user);
      this.MemberDataRemove(user);
    }

    private void GameServer_ValidateAuthTicketResponse(
      ulong steamID,
      JoinResult response,
      ulong steamOwner,
      string serviceName)
    {
      MyLog.Default.WriteLineAndConsole(string.Format("Server ValidateAuthTicketResponse ({0}), {1} ID:{2} Owner ID:{3}", (object) response, (object) serviceName, (object) EndpointId.Format(steamID), (object) EndpointId.Format(steamOwner)));
      if (this.IsClientBanned(steamID) || MySandboxGame.ConfigDedicated.Banned.Contains(steamID))
      {
        this.UserRejected(steamID, JoinResult.BannedByAdmins);
        this.RaiseClientKicked(steamID);
      }
      else if (this.IsClientKicked(steamID))
      {
        this.UserRejected(steamID, JoinResult.KickedRecently);
        this.RaiseClientKicked(steamID);
      }
      else if (this.IsClientFamilySharingKicked(steamID, steamOwner))
      {
        this.UserRejected(steamID, JoinResult.FamilySharing);
        this.RaiseClientKicked(steamID);
      }
      else if (response == JoinResult.OK)
      {
        if (MySandboxGame.ConfigDedicated.Administrators.Contains(steamID.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(MyDedicatedServerBase.ConvertSteamIDFrom64(steamID)) || MySandboxGame.ConfigDedicated.Reserved.Contains(steamID))
          this.UserAccepted(steamID);
        else if (this.MemberLimit > 0 && this.m_members.Count - 1 >= this.MemberLimit)
          this.UserRejected(steamID, JoinResult.ServerFull);
        else if (this.ClientIsProfiling(steamID))
          this.UserRejected(steamID, JoinResult.ProfilingNotAllowed);
        else if (this.m_groupId == 0UL)
          this.UserAccepted(steamID);
        else if (MyGameService.GetServerAccountType(this.m_groupId) != MyGameServiceAccountType.Clan)
          this.UserRejected(steamID, JoinResult.GroupIdInvalid);
        else if (MyGameService.GameServer.RequestGroupStatus(steamID, this.m_groupId))
          this.m_waitingForGroup.Add(steamID);
        else
          this.UserRejected(steamID, JoinResult.SteamServersOffline);
      }
      else
        this.UserRejected(steamID, response);
    }

    protected bool IsClientFamilySharingKicked(ulong steamID, ulong ownerId)
    {
      if (MySession.Static.Settings.FamilySharing || (long) steamID == (long) ownerId)
        return false;
      MySandboxGame.Log.WriteLineAndConsole(string.Format("User: {0} using family sharing of {1}.", (object) steamID, (object) ownerId));
      return true;
    }

    private void GameServer_UserGroupStatus(
      ulong userId,
      ulong groupId,
      bool member,
      bool officier)
    {
      if ((long) groupId != (long) this.m_groupId || !this.m_waitingForGroup.Remove(userId))
        return;
      if (member | officier)
        this.UserAccepted(userId);
      else
        this.UserRejected(userId, JoinResult.NotInGroup);
    }

    private void GameServer_PolicyResponse(sbyte result) => MyLog.Default.WriteLineAndConsole("Server PolicyResponse (" + (object) result + ")");

    private void GameServer_ServersDisconnected(string result) => MyLog.Default.WriteLineAndConsole("Server disconnected (" + result + ")");

    private void GameServer_ServersConnectFailure(string result)
    {
      MyLog.Default.WriteLineAndConsole("Server connect failure (" + result + ")");
      this.HasServerResponded = true;
    }

    private void GameServer_ServersConnected()
    {
      MyLog.Default.WriteLineAndConsole("Server connected to " + MyGameService.Networking.ServiceName);
      this.HasServerResponded = true;
    }

    private void UserRejected(ulong steamID, JoinResult reason)
    {
      this.m_pendingMembers.Remove(steamID);
      this.m_waitingForGroup.Remove(steamID);
      if (this.m_members.Contains(steamID))
        this.RaiseClientLeft(steamID, MyChatMemberStateChangeEnum.Disconnected);
      else
        this.SendJoinResult(steamID, reason);
    }

    private void UserAccepted(ulong steamID)
    {
      this.m_members.Add(steamID);
      MyMultiplayerBase.MyConnectedClientData data;
      if (this.m_pendingMembers.TryGetValue(steamID, out data))
      {
        this.m_pendingMembers.Remove(steamID);
        this.MemberDataSet(steamID, data);
        foreach (ulong member in this.m_members)
        {
          if ((long) member != (long) this.ServerId)
            this.SendClientData(member, steamID, data.Name, true, data.ServiceName);
        }
      }
      this.SendServerData();
      if (this.IsPasswordProtected)
        this.SendJoinResult(steamID, JoinResult.PasswordRequired);
      else
        this.SendJoinResult(steamID, JoinResult.OK);
    }

    private bool ClientIsProfiling(ulong steamID)
    {
      MyMultiplayerBase.MyConnectedClientData connectedClientData;
      return this.m_pendingMembers.TryGetValue(steamID, out connectedClientData) && connectedClientData.IsProfiling;
    }

    protected override void OnPasswordHash(ref MyControlSendPasswordHashMsg message, ulong sender)
    {
      base.OnPasswordHash(ref message, sender);
      if (!this.IsPasswordProtected || string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.ServerPasswordHash))
      {
        this.SendJoinResult(sender, JoinResult.OK);
      }
      else
      {
        byte[] passwordHash = message.PasswordHash;
        byte[] numArray = Convert.FromBase64String(MySandboxGame.ConfigDedicated.ServerPasswordHash);
        if (passwordHash == null || passwordHash.Length != numArray.Length)
        {
          this.RejectUserWithWrongPassword(sender);
        }
        else
        {
          for (int index = 0; index < numArray.Length; ++index)
          {
            if ((int) numArray[index] != (int) passwordHash[index])
            {
              this.RejectUserWithWrongPassword(sender);
              return;
            }
          }
          this.ResetWrongPasswordCounter(sender);
          this.SendJoinResult(sender, JoinResult.OK);
        }
      }
    }

    private void RejectUserWithWrongPassword(ulong sender)
    {
      this.AddWrongPasswordClient(sender);
      if (this.IsOutOfWrongPasswordTries(sender))
        this.KickClient(sender, true, true);
      else
        this.SendJoinResult(sender, JoinResult.WrongPassword);
    }

    public virtual bool IsCorrectVersion() => this.m_appVersion == (int) MyFinalBuildConstants.APP_VERSION;

    public override void DownloadWorld(int appVersion)
    {
    }

    public override void DisconnectClient(ulong userId)
    {
      MyControlDisconnectedMsg message = new MyControlDisconnectedMsg()
      {
        Client = this.ServerId
      };
      this.SendControlMessage<MyControlDisconnectedMsg>(userId, ref message);
      this.RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Disconnected);
    }

    public override void BanClient(ulong userId, bool banned)
    {
      if (banned)
      {
        MyLog.Default.WriteLineAndConsole("Player " + this.GetMemberName(userId) + " banned");
        MyControlBanClientMsg message = new MyControlBanClientMsg()
        {
          BannedClient = userId,
          Banned = (BoolBlit) true
        };
        this.SendControlMessageToAll<MyControlBanClientMsg>(ref message);
        this.AddBannedClient(userId);
        if (this.m_members.Contains(userId))
          this.RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Banned);
        MySandboxGame.ConfigDedicated.Banned.Add(userId);
      }
      else
      {
        MyLog.Default.WriteLineAndConsole("Player " + EndpointId.Format(userId) + " unbanned");
        MyControlBanClientMsg message = new MyControlBanClientMsg()
        {
          BannedClient = userId,
          Banned = (BoolBlit) false
        };
        this.SendControlMessageToAll<MyControlBanClientMsg>(ref message);
        this.RemoveBannedClient(userId);
        MySandboxGame.ConfigDedicated.Banned.Remove(userId);
      }
      MySandboxGame.ConfigDedicated.Save();
    }

    public override void Tick()
    {
      base.Tick();
      this.UpdateSteamServerData();
    }

    private void UpdateSteamServerData()
    {
      if (!this.m_gameServerDataDirty)
        return;
      MyGameService.GameServer.SetMapName(this.m_worldName);
      MyGameService.GameServer.SetMaxPlayerCount(this.m_membersLimit);
      foreach (KeyValuePair<ulong, MyMultiplayerBase.MyConnectedClientData> keyValuePair in this.m_memberData)
        MyGameService.GameServer.BrowserUpdateUserData(keyValuePair.Key, keyValuePair.Value.Name, 0);
      this.m_gameServerDataDirty = false;
    }

    public override void SendChatMessage(
      string text,
      ChatChannel channel,
      long targetId,
      string customAuthor)
    {
      ChatMsg msg = new ChatMsg()
      {
        Text = text,
        Author = Sync.MyId,
        Channel = (byte) channel,
        TargetId = targetId,
        CustomAuthorName = customAuthor ?? string.Empty
      };
      MyMultiplayerBase.SendChatMessage(ref msg);
    }

    public void SendChatMessageToPlayer(string text, ulong steamId)
    {
      if (!MyMultiplayer.Static.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<ChatMsg>((Func<IMyEventOwner, Action<ChatMsg>>) (s => new Action<ChatMsg>(MyMultiplayerBase.OnChatMessageReceived_ToPlayer)), new ChatMsg()
      {
        Text = text,
        Author = Sync.MyId,
        Channel = (byte) 3,
        CustomAuthorName = string.Empty
      }, new EndpointId(steamId));
    }

    private void SendJoinResult(ulong sendTo, JoinResult joinResult, ulong adminID = 0)
    {
      JoinResultMsg msg = new JoinResultMsg()
      {
        JoinResult = joinResult,
        ServerExperimental = MySession.Static.IsSettingsExperimental(),
        Admin = adminID
      };
      this.ReplicationLayer.SendJoinResult(ref msg, sendTo);
    }

    public override void Dispose()
    {
      string serviceName = MyGameService.Networking.ServiceName;
      foreach (ulong member in this.m_members)
      {
        MyControlDisconnectedMsg message = new MyControlDisconnectedMsg()
        {
          Client = this.ServerId
        };
        if ((long) member != (long) this.ServerId)
          this.SendControlMessage<MyControlDisconnectedMsg>(member, ref message);
      }
      Thread.Sleep(200);
      try
      {
        MyNetworkMonitor.Done();
        this.CloseMemberSessions();
        MyGameService.GameServer.EnableHeartbeats(false);
        base.Dispose();
        MyLog.Default.WriteLineAndConsole("Logging off " + serviceName + "...");
        MyGameService.GameServer.LogOff();
        MyLog.Default.WriteLineAndConsole("Shutting down server...");
        MyGameService.GameServer.Shutdown();
        MyLog.Default.WriteLineAndConsole("Done");
        MyGameService.Peer2Peer.SessionRequest -= new Action<ulong>(this.Peer2Peer_SessionRequest);
        MyGameService.Peer2Peer.ConnectionFailed -= new Action<ulong, string>(this.Peer2Peer_ConnectionFailed);
        this.ClientLeft -= new Action<ulong, MyChatMemberStateChangeEnum>(this.MyDedicatedServer_ClientLeft);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLineAndConsole("catch exception : " + (object) ex);
      }
    }

    public override IEnumerable<ulong> Members => (IEnumerable<ulong>) this.m_members;

    public override int MemberCount => this.m_members.Count;

    public override bool IsSomeoneElseConnected => this.MemberCount >= 2;

    public override ulong LobbyId => 0;

    public override int MemberLimit
    {
      get => this.m_membersLimit;
      set => this.SetMemberLimit(value);
    }

    public override ulong GetOwner() => this.ServerId;

    public override MyLobbyType GetLobbyType() => MyLobbyType.Public;

    public override void SetLobbyType(MyLobbyType myLobbyType)
    {
    }

    public override void SetMemberLimit(int limit)
    {
      int membersLimit = this.m_membersLimit;
      this.m_membersLimit = MyDedicatedServerOverrides.MaxPlayers.HasValue ? MyDedicatedServerOverrides.MaxPlayers.Value : Math.Max(limit, 2);
      this.m_gameServerDataDirty |= membersLimit != this.m_membersLimit;
    }

    private void OnConnectedClient(ref ConnectedClientDataMsg msg, ulong steamId)
    {
      if (!MyGameService.GameServer.BeginAuthSession(steamId, msg.Token, msg.ServiceName))
        MyLog.Default.WriteLineAndConsole("Authentication failed.");
      else if (!msg.ExperimentalMode && this.ExperimentalMode)
      {
        MyLog.Default.WriteLineAndConsole("Server and client Experimental Mode does not match.");
        this.SendJoinResult(steamId, JoinResult.ExperimentalMode);
      }
      else
      {
        if (MyVisualScriptLogicProvider.PlayerConnectRequest != null)
        {
          JoinResult result = JoinResult.OK;
          MyVisualScriptLogicProvider.PlayerConnectRequest(steamId, ref result);
          if (result != JoinResult.OK)
          {
            this.SendJoinResult(steamId, result);
            return;
          }
        }
        msg.Name = this.m_filterOffensive(msg.Name);
        msg.Name = this.MakeMemberNameUnique(msg.Name);
        this.RaiseClientJoined(steamId, msg.Name);
        MyLog.Default.WriteLineAndConsole("OnConnectedClient " + msg.Name + " attempt");
        if (this.m_members.Contains(steamId))
        {
          MyLog.Default.WriteLineAndConsole("Already joined");
          this.SendJoinResult(steamId, JoinResult.AlreadyJoined);
        }
        else if (MySandboxGame.ConfigDedicated.Banned.Contains(steamId))
        {
          MyLog.Default.WriteLineAndConsole("User is banned by admins");
          ulong result = 0;
          foreach (KeyValuePair<ulong, MyMultiplayerBase.MyConnectedClientData> keyValuePair in this.m_memberData)
          {
            if (keyValuePair.Value.IsAdmin && (long) keyValuePair.Key != (long) this.ServerId)
            {
              result = keyValuePair.Key;
              break;
            }
          }
          if (result == 0UL && MySandboxGame.ConfigDedicated.Administrators.Count > 0)
            ulong.TryParse(MySandboxGame.ConfigDedicated.Administrators[0], out result);
          this.SendJoinResult(steamId, JoinResult.BannedByAdmins, result);
        }
        else
          this.m_pendingMembers.Add(steamId, new MyMultiplayerBase.MyConnectedClientData()
          {
            Name = msg.Name,
            IsAdmin = MySandboxGame.ConfigDedicated.Administrators.Contains(steamId.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(MyDedicatedServerBase.ConvertSteamIDFrom64(steamId)),
            IsProfiling = msg.IsProfiling,
            ServiceName = msg.ServiceName
          });
      }
    }

    private bool IsUniqueMemberName(string name)
    {
      foreach (MyMultiplayerBase.MyConnectedClientData connectedClientData in this.m_pendingMembers.Values)
      {
        if (connectedClientData.Name == name)
          return false;
      }
      foreach (MyMultiplayerBase.MyConnectedClientData connectedClientData in this.m_memberData.Values)
      {
        if (connectedClientData.Name == name)
          return false;
      }
      return true;
    }

    private string MakeMemberNameUnique(string name)
    {
      string name1 = name;
      int num = 0;
      for (; !this.IsUniqueMemberName(name1); name1 = name + string.Format(" ({0})", (object) num))
        ++num;
      return name1;
    }

    public override string GetMemberName(ulong steamUserID)
    {
      MyMultiplayerBase.MyConnectedClientData data;
      this.MemberDataGet(steamUserID, out data);
      return data.Name != null ? data.Name : EndpointId.Format(steamUserID);
    }

    public override string GetMemberServiceName(ulong steamUserID)
    {
      MyMultiplayerBase.MyConnectedClientData data;
      this.MemberDataGet(steamUserID, out data);
      return data.ServiceName;
    }

    private void SendClientData(
      ulong steamTo,
      ulong connectedSteamID,
      string connectedClientName,
      bool join,
      string serviceName)
    {
      ConnectedClientDataMsg msg = new ConnectedClientDataMsg()
      {
        ClientId = new EndpointId(connectedSteamID),
        Name = connectedClientName,
        IsAdmin = MySandboxGame.ConfigDedicated.Administrators.Contains(connectedSteamID.ToString()) || MySandboxGame.ConfigDedicated.Administrators.Contains(MyDedicatedServerBase.ConvertSteamIDFrom64(connectedSteamID)),
        Join = join,
        ServiceName = serviceName
      };
      this.ReplicationLayer.SendClientConnected(ref msg, steamTo);
    }

    protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
    {
      if (!MySession.Static.IsUserAdmin(sender))
        return;
      this.BanClient(data.BannedClient, (bool) data.Banned);
    }

    private void ClientConnected(MyPacket packet)
    {
      ConnectedClientDataMsg msg = this.ReplicationLayer.OnClientConnected(packet);
      Sync.ClientConnected(packet.Sender.Id.Value, msg.Name);
      this.OnConnectedClient(ref msg, packet.Sender.Id.Value);
      packet.Return();
    }
  }
}
