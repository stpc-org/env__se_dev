// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerClient
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
  internal sealed class MyMultiplayerClient : MyMultiplayerClientBase
  {
    private string m_worldName;
    private MyGameModeEnum m_gameMode;
    private float m_inventoryMultiplier;
    private float m_blocksInventoryMultiplier;
    private float m_assemblerMultiplier;
    private float m_refineryMultiplier;
    private float m_welderMultiplier;
    private float m_grinderMultiplier;
    private string m_hostName;
    private string m_hostAnalyticsId;
    private ulong m_worldSize;
    private int m_appVersion;
    private int m_membersLimit;
    private string m_dataHash;
    private string m_serverPasswordSalt;
    private bool m_sessionClosed;
    private readonly List<ulong> m_members = new List<ulong>();
    private readonly Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData> m_memberData = new Dictionary<ulong, MyMultiplayerBase.MyConnectedClientData>();
    public Action OnJoin;
    private List<MyObjectBuilder_Checkpoint.ModItem> m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();

    protected override bool IsServerInternal => false;

    public override string WorldName
    {
      get => this.m_worldName;
      set => this.m_worldName = value;
    }

    public override MyGameModeEnum GameMode
    {
      get => this.m_gameMode;
      set => this.m_gameMode = value;
    }

    public override float InventoryMultiplier
    {
      get => this.m_inventoryMultiplier;
      set => this.m_inventoryMultiplier = value;
    }

    public override float BlocksInventoryMultiplier
    {
      get => this.m_blocksInventoryMultiplier;
      set => this.m_blocksInventoryMultiplier = value;
    }

    public override float AssemblerMultiplier
    {
      get => this.m_assemblerMultiplier;
      set => this.m_assemblerMultiplier = value;
    }

    public override float RefineryMultiplier
    {
      get => this.m_refineryMultiplier;
      set => this.m_refineryMultiplier = value;
    }

    public override float WelderMultiplier
    {
      get => this.m_welderMultiplier;
      set => this.m_welderMultiplier = value;
    }

    public override float GrinderMultiplier
    {
      get => this.m_grinderMultiplier;
      set => this.m_grinderMultiplier = value;
    }

    public override string HostName
    {
      get => this.m_hostName;
      set => this.m_hostName = value;
    }

    public string HostAnalyticsId => this.m_hostAnalyticsId;

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

    public override int MaxPlayers => 65536;

    public override int ModCount { get; protected set; }

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

    public override bool Scenario { get; set; }

    public override string ScenarioBriefing { get; set; }

    public override DateTime ScenarioStartTime { get; set; }

    public MyGameServerItem Server { get; private set; }

    public override bool ExperimentalMode { get; set; }

    internal MyMultiplayerClient(MyGameServerItem server, MySyncLayer syncLayer)
      : base(syncLayer)
    {
      this.SyncLayer.RegisterClientEvents((MyMultiplayerBase) this);
      this.SyncLayer.TransportLayer.IsBuffering = true;
      this.Server = server;
      this.ServerId = server.SteamID;
      this.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.MyMultiplayerClient_ClientLeft);
      syncLayer.TransportLayer.Register(MyMessageId.JOIN_RESULT, (byte) 0, new Action<MyPacket>(this.OnJoinResult));
      syncLayer.TransportLayer.Register(MyMessageId.WORLD_DATA, (byte) 0, new Action<MyPacket>(this.OnWorldData));
      syncLayer.TransportLayer.Register(MyMessageId.CLIENT_CONNECTED, (byte) 0, new Action<MyPacket>(this.OnClientConnected));
      this.ClientJoined += new Action<ulong, string>(this.MyMultiplayerClient_ClientJoined);
      this.HostLeft += new Action(this.MyMultiplayerClient_HostLeft);
      MyGameService.ConnectToServer(server, (Action<JoinResult>) (joinResult => MySandboxGame.Static.Invoke((Action) (() => this.OnConnectToServer(joinResult)), "OnConnectToServer")));
      MyGameService.Peer2Peer.ConnectionFailed += new Action<ulong, string>(this.Peer2Peer_ConnectionFailed);
    }

    private void OnConnectToServer(JoinResult joinResult)
    {
      if (this.m_sessionClosed)
        return;
      if (joinResult == JoinResult.OK)
      {
        MyGameService.Networking.Chat.UpdateChatAvailability();
        this.SendPlayerData(MyGameService.UserName);
      }
      else
      {
        JoinResultMsg msg = new JoinResultMsg()
        {
          JoinResult = joinResult
        };
        this.OnUserJoined(ref msg);
      }
      this.DisconnectReason = (string) null;
    }

    private void OnWorldData(MyPacket packet)
    {
      ServerDataMsg msg = this.ReplicationLayer.OnWorldData(packet);
      this.OnServerData(ref msg);
      packet.Return();
    }

    private void OnJoinResult(MyPacket packet)
    {
      JoinResultMsg msg = this.ReplicationLayer.OnJoinResult(packet);
      this.OnUserJoined(ref msg);
      packet.Return();
    }

    private void OnClientConnected(MyPacket packet)
    {
      ConnectedClientDataMsg msg = this.ReplicationLayer.OnClientConnected(packet);
      Sync.ClientConnected(packet.Sender.Id.Value, msg.Name);
      this.OnConnectedClient(ref msg, msg.ClientId);
      packet.Return();
    }

    public override void Dispose()
    {
      if (!this.m_sessionClosed)
        this.CloseClient();
      base.Dispose();
    }

    private void MyMultiplayerClient_HostLeft()
    {
      this.CloseSession();
      MySessionLoader.UnloadAndExitToMenu();
      MyGuiScreenServerReconnector.ReconnectToLastSession();
    }

    private void MyMultiplayerClient_ClientLeft(ulong user, MyChatMemberStateChangeEnum stateChange)
    {
      if ((long) user == (long) this.ServerId)
      {
        this.RaiseHostLeft();
      }
      else
      {
        if (this.m_members.Contains(user))
        {
          this.m_members.Remove(user);
          MyMultiplayerBase.MyConnectedClientData connectedClientData;
          string str = this.m_memberData.TryGetValue(user, out connectedClientData) ? connectedClientData.Name : "Unknown";
          MySandboxGame.Log.WriteLineAndConsole("Player disconnected: " + str + " (" + EndpointId.Format(user) + ")");
          if (MySandboxGame.IsGameReady && (long) Sync.MyId != (long) user)
          {
            MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientDisconnected, 5000, level: MyNotificationLevel.Important);
            myHudNotification.SetTextFormatArguments((object) str);
            MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
          }
        }
        this.m_memberData.Remove(user);
      }
    }

    private void MyMultiplayerClient_ClientJoined(ulong user, string userName)
    {
      if (this.m_members.Contains(user))
        return;
      this.m_members.Add(user);
    }

    private void Peer2Peer_ConnectionFailed(ulong remoteUserId, string error)
    {
      this.DisconnectReason = error;
      if ((long) remoteUserId != (long) this.ServerId)
        return;
      this.RaiseHostLeft();
    }

    public bool IsCorrectVersion() => this.m_appVersion == (int) MyFinalBuildConstants.APP_VERSION;

    public override void DisconnectClient(ulong userId)
    {
      if (this.m_sessionClosed)
        return;
      this.CloseClient();
    }

    public override void BanClient(ulong client, bool ban)
    {
      MyControlBanClientMsg message = new MyControlBanClientMsg()
      {
        BannedClient = client,
        Banned = (BoolBlit) ban
      };
      this.SendControlMessage<MyControlBanClientMsg>(this.ServerId, ref message);
    }

    private void CloseClient()
    {
      MyControlDisconnectedMsg message = new MyControlDisconnectedMsg()
      {
        Client = Sync.MyId
      };
      this.SendControlMessage<MyControlDisconnectedMsg>(this.ServerId, ref message);
      this.OnJoin = (Action) null;
      Thread.Sleep(200);
      this.CloseSession();
    }

    private void CloseSession()
    {
      this.OnJoin = (Action) null;
      if (MyGameService.Networking != null)
      {
        MyGameService.Peer2Peer.ConnectionFailed -= new Action<ulong, string>(this.Peer2Peer_ConnectionFailed);
        MyGameService.Peer2Peer.CloseSession(this.ServerId);
        MyGameService.DisconnectFromServer();
      }
      this.m_sessionClosed = true;
    }

    public override IEnumerable<ulong> Members => (IEnumerable<ulong>) this.m_members;

    public override int MemberCount => this.m_members.Count;

    public override bool IsSomeoneElseConnected => this.MemberCount >= 3;

    public override ulong LobbyId => 0;

    public override int MemberLimit
    {
      get => this.m_membersLimit;
      set => this.m_membersLimit = value;
    }

    public string DisconnectReason { get; private set; }

    public override ulong GetOwner() => this.ServerId;

    public override MyLobbyType GetLobbyType() => MyLobbyType.Public;

    public override void SetLobbyType(MyLobbyType myLobbyType)
    {
    }

    public override void SetMemberLimit(int limit) => this.m_membersLimit = limit;

    public override void OnChatMessage(ref ChatMsg msg)
    {
      bool flag = false;
      if (this.m_memberData.ContainsKey(msg.Author) && this.m_memberData[msg.Author].IsAdmin | flag)
        MyClientDebugCommands.Process(msg.Text, msg.Author);
      this.RaiseChatMessageReceived(msg.Author, msg.Text, (ChatChannel) msg.Channel, msg.TargetId, msg.CustomAuthorName ?? string.Empty);
    }

    public override void SendChatMessage(
      string text,
      ChatChannel channel,
      long targetId,
      string customAuthor)
    {
      if (channel == ChatChannel.GlobalScripted)
        return;
      ChatMsg msg = new ChatMsg()
      {
        Text = text,
        Author = Sync.MyId,
        Channel = (byte) channel,
        TargetId = targetId,
        CustomAuthorName = string.Empty
      };
      this.OnChatMessage(ref msg);
      MyMultiplayerBase.SendChatMessage(ref msg);
    }

    private void OnServerData(ref ServerDataMsg msg)
    {
      this.m_worldName = msg.WorldName;
      this.m_gameMode = msg.GameMode;
      this.m_inventoryMultiplier = msg.InventoryMultiplier;
      this.m_blocksInventoryMultiplier = msg.BlocksInventoryMultiplier;
      this.m_assemblerMultiplier = msg.AssemblerMultiplier;
      this.m_refineryMultiplier = msg.RefineryMultiplier;
      this.m_welderMultiplier = msg.WelderMultiplier;
      this.m_grinderMultiplier = msg.GrinderMultiplier;
      this.m_hostName = msg.HostName;
      this.m_hostAnalyticsId = msg.ServerAnalyticsId;
      this.m_worldSize = msg.WorldSize;
      this.m_appVersion = msg.AppVersion;
      this.m_membersLimit = msg.MembersLimit;
      this.m_dataHash = msg.DataHash;
      this.m_serverPasswordSalt = msg.ServerPasswordSalt;
    }

    private void OnUserJoined(ref JoinResultMsg msg)
    {
      if (msg.JoinResult == JoinResult.OK)
      {
        this.IsServerExperimental = msg.ServerExperimental;
        if (this.OnJoin == null)
          return;
        this.OnJoin();
        this.OnJoin = (Action) null;
      }
      else if (msg.JoinResult == JoinResult.NotInGroup)
      {
        MySessionLoader.UnloadAndExitToMenu();
        this.Dispose();
        ulong groupId = this.Server.GetGameTagByPrefixUlong("groupId");
        string clanName = MyGameService.GetClanName(groupId);
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerErrorNotInGroup), (object) clanName)), messageCaption: messageCaption);
        messageBox.ResultCallback = (Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          MyGameService.OpenOverlayUser(groupId);
        });
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      else if (msg.JoinResult == JoinResult.BannedByAdmins)
      {
        MySessionLoader.UnloadAndExitToMenu();
        this.Dispose();
        ulong admin = msg.Admin;
        if (admin != 0UL)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorBannedByAdminsWithDialog), messageCaption: messageCaption);
          messageBox.ResultCallback = (Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            MyGameService.OpenOverlayUser(admin);
          });
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
        }
        else
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorBannedByAdmins), messageCaption: messageCaption));
        }
      }
      else
      {
        StringBuilder messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorConnectionFailed);
        switch (msg.JoinResult)
        {
          case JoinResult.AlreadyJoined:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorAlreadyJoined);
            break;
          case JoinResult.TicketInvalid:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketInvalid);
            break;
          case JoinResult.SteamServersOffline:
            messageText = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MultiplayerErrorSteamServersOffline), (object) MySession.GameServiceName);
            break;
          case JoinResult.GroupIdInvalid:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorGroupIdInvalid);
            break;
          case JoinResult.ServerFull:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorServerFull);
            break;
          case JoinResult.KickedRecently:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorKickedByAdmins);
            break;
          case JoinResult.TicketCanceled:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketCanceled);
            break;
          case JoinResult.TicketAlreadyUsed:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorTicketAlreadyUsed);
            break;
          case JoinResult.LoggedInElseWhere:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorLoggedInElseWhere);
            break;
          case JoinResult.NoLicenseOrExpired:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorNoLicenseOrExpired);
            break;
          case JoinResult.UserNotConnected:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorUserNotConnected);
            break;
          case JoinResult.VACBanned:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorVACBanned);
            break;
          case JoinResult.VACCheckTimedOut:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorVACCheckTimedOut);
            break;
          case JoinResult.PasswordRequired:
            MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenServerPassword((Action<string>) (password => this.SendPasswordHash(password))));
            return;
          case JoinResult.WrongPassword:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorWrongPassword);
            break;
          case JoinResult.ExperimentalMode:
            messageText = !MySandboxGame.Config.ExperimentalMode ? MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental) : MyTexts.Get(MyCommonTexts.MultiplayerErrorNotExperimental);
            break;
          case JoinResult.ProfilingNotAllowed:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorProfilingNotAllowed);
            break;
          case JoinResult.FamilySharing:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerError_FamilyShareDisabled);
            break;
          case JoinResult.NotFound:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorNotFound);
            break;
          case JoinResult.IncorrectTime:
            messageText = MyTexts.Get(MyCommonTexts.MultiplayerErrorIncorrectTime);
            break;
        }
        this.Dispose();
        MySessionLoader.UnloadAndExitToMenu();
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: messageText, messageCaption: messageCaption));
      }
    }

    public void SendPasswordHash(string password)
    {
      if (string.IsNullOrEmpty(this.m_serverPasswordSalt))
      {
        MyLog.Default.Error("Empty password salt on the server.");
      }
      else
      {
        byte[] salt = Convert.FromBase64String(this.m_serverPasswordSalt);
        byte[] bytes = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(20);
        MyControlSendPasswordHashMsg message = new MyControlSendPasswordHashMsg()
        {
          PasswordHash = bytes
        };
        this.SendControlMessage<MyControlSendPasswordHashMsg>(this.ServerId, ref message);
      }
    }

    public void LoadMembersFromWorld(List<MyObjectBuilder_Client> clients)
    {
      if (clients == null)
        return;
      foreach (MyObjectBuilder_Client client in clients)
      {
        this.m_memberData.Add(client.SteamId, new MyMultiplayerBase.MyConnectedClientData()
        {
          Name = client.Name,
          IsAdmin = client.IsAdmin,
          ServiceName = client.ClientService
        });
        this.RaiseClientJoined(client.SteamId, client.Name);
      }
    }

    public override string GetMemberName(ulong steamUserID)
    {
      MyMultiplayerBase.MyConnectedClientData connectedClientData;
      this.m_memberData.TryGetValue(steamUserID, out connectedClientData);
      return connectedClientData.Name;
    }

    public override string GetMemberServiceName(ulong steamUserID)
    {
      MyMultiplayerBase.MyConnectedClientData connectedClientData;
      this.m_memberData.TryGetValue(steamUserID, out connectedClientData);
      return connectedClientData.ServiceName;
    }

    private void OnConnectedClient(ref ConnectedClientDataMsg msg, EndpointId playerId)
    {
      MySandboxGame.Log.WriteLineAndConsole("Client connected: " + msg.Name + " (" + (object) playerId + ")");
      if (MySandboxGame.IsGameReady && (long) playerId.Value != (long) this.ServerId && ((long) Sync.MyId != (long) playerId.Value && msg.Join))
      {
        MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientConnected, 5000, level: MyNotificationLevel.Important);
        myHudNotification.SetTextFormatArguments((object) msg.Name);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      this.m_memberData[playerId.Value] = new MyMultiplayerBase.MyConnectedClientData()
      {
        Name = msg.Name,
        IsAdmin = msg.IsAdmin,
        IsProfiling = msg.IsProfiling,
        ServiceName = msg.ServiceName
      };
      MyGameService.Networking.Chat.WarmupPlayerCache(playerId.Value, this.IsCrossMember(playerId.Value));
      this.RaiseClientJoined(playerId.Value, msg.Name);
    }

    private void SendPlayerData(string clientName)
    {
      ConnectedClientDataMsg msg = new ConnectedClientDataMsg()
      {
        Name = clientName,
        Join = true,
        ExperimentalMode = this.ExperimentalMode,
        IsProfiling = MyDebugDrawSettings.ENABLE_DEBUG_DRAW,
        ServiceName = MyGameService.Service.ServiceName
      };
      byte[] buffer = new byte[1024];
      uint length;
      if (!MyGameService.GetAuthSessionTicket(out uint _, buffer, out length))
      {
        MySessionLoader.UnloadAndExitToMenu();
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorConnectionFailed), messageCaption: messageCaption));
      }
      else
      {
        msg.Token = new byte[(int) length];
        Array.Copy((Array) buffer, (Array) msg.Token, (long) length);
        this.ReplicationLayer.SendClientConnected(ref msg);
      }
    }

    protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
    {
      if ((long) data.BannedClient == (long) Sync.MyId && (bool) data.Banned)
      {
        MySessionLoader.UnloadAndExitToMenu();
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionKicked);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextYouHaveBeenBanned), messageCaption: messageCaption));
      }
      else
      {
        if ((bool) data.Banned)
          this.AddBannedClient(data.BannedClient);
        else
          this.RemoveBannedClient(data.BannedClient);
        if (!this.m_members.Contains(data.BannedClient) || !(bool) data.Banned)
          return;
        this.RaiseClientLeft(data.BannedClient, MyChatMemberStateChangeEnum.Banned);
      }
    }

    public override void StartProcessingClientMessagesWithEmptyWorld()
    {
      if (!Sync.Clients.HasClient(this.ServerId))
        Sync.Clients.AddClient(this.ServerId, this.HostName);
      base.StartProcessingClientMessagesWithEmptyWorld();
      if (Sync.Clients.LocalClient != null)
        return;
      Sync.Clients.SetLocalSteamId(Sync.MyId, !Sync.Clients.HasClient(Sync.MyId), MyGameService.UserName);
    }

    protected override void OnAllMembersData(ref AllMembersDataMsg msg)
    {
      if (msg.Clients != null)
      {
        foreach (MyObjectBuilder_Client client in msg.Clients)
        {
          if (!this.m_memberData.ContainsKey(client.SteamId))
            this.m_memberData.Add(client.SteamId, new MyMultiplayerBase.MyConnectedClientData()
            {
              Name = client.Name,
              IsAdmin = client.IsAdmin,
              ServiceName = client.ClientService
            });
          if (!this.m_members.Contains(client.SteamId))
            this.m_members.Add(client.SteamId);
          if (!Sync.Clients.HasClient(client.SteamId))
            Sync.Clients.AddClient(client.SteamId, client.Name);
        }
      }
      this.ProcessAllMembersData(ref msg);
    }
  }
}
