// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerLobby
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Multiplayer
{
  public sealed class MyMultiplayerLobby : MyMultiplayerServerBase
  {
    private IMyLobby m_lobby;
    private bool m_serverDataValid;

    protected override bool IsServerInternal => true;

    public override string WorldName
    {
      get => MyMultiplayerLobby.GetLobbyWorldName(this.m_lobby);
      set => this.m_lobby.SetData("world", value ?? "Unnamed");
    }

    public override MyGameModeEnum GameMode
    {
      get => MyMultiplayerLobby.GetLobbyGameMode(this.m_lobby);
      set => this.m_lobby.SetData("gameMode", ((int) value).ToString());
    }

    public override float InventoryMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("inventoryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float BlocksInventoryMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("blocksInventoryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float AssemblerMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("assemblerMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float RefineryMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("refineryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float WelderMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("welderMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("welderMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float GrinderMultiplier
    {
      get => MyMultiplayerLobby.GetLobbyFloat("grinderMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("grinderMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override string HostName
    {
      get => MyMultiplayerLobby.GetLobbyHostName(this.m_lobby);
      set => this.m_lobby.SetData("host", value);
    }

    public override ulong WorldSize
    {
      get => MyMultiplayerLobby.GetLobbyWorldSize(this.m_lobby);
      set => this.m_lobby.SetData("worldSize", value.ToString());
    }

    public override int AppVersion
    {
      get => MyMultiplayerLobby.GetLobbyAppVersion(this.m_lobby);
      set => this.m_lobby.SetData("appVersion", value.ToString());
    }

    public override string DataHash
    {
      get => this.m_lobby.GetData("dataHash");
      set => this.m_lobby.SetData("dataHash", value);
    }

    public static int MAX_PLAYERS => MyPlatformGameSettings.LOBBY_MAX_PLAYERS;

    public override int MaxPlayers => MyMultiplayerLobby.MAX_PLAYERS;

    public override int ModCount
    {
      get => MyMultiplayerLobby.GetLobbyModCount(this.m_lobby);
      protected set => this.m_lobby.SetData("mods", value.ToString());
    }

    public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
    {
      get => MyMultiplayerLobby.GetLobbyMods(this.m_lobby);
      set
      {
        this.ModCount = value.Count;
        int num = this.ModCount - 1;
        foreach (MyObjectBuilder_Checkpoint.ModItem modItem in value)
        {
          string str = modItem.PublishedFileId.ToString() + "_" + modItem.PublishedServiceName + "_" + modItem.FriendlyName;
          this.m_lobby.SetData("mod" + (object) num--, str, false);
        }
      }
    }

    public override int ViewDistance
    {
      get => MyMultiplayerLobby.GetLobbyViewDistance(this.m_lobby);
      set => this.m_lobby.SetData("view", value.ToString());
    }

    public override int SyncDistance
    {
      get => MyLayers.GetSyncDistance();
      set => MyLayers.SetSyncDistance(value);
    }

    public override bool Scenario
    {
      get => MyMultiplayerLobby.GetLobbyBool("scenario", this.m_lobby, false);
      set => this.m_lobby.SetData("scenario", value.ToString());
    }

    public override string ScenarioBriefing
    {
      get => this.m_lobby.GetData("scenarioBriefing");
      set => this.m_lobby.SetData("scenarioBriefing", value == null || value.Length < 1 ? " " : value);
    }

    public override DateTime ScenarioStartTime
    {
      get => MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", this.m_lobby, DateTime.MinValue);
      set => this.m_lobby.SetData("scenarioStartTime", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public ulong HostSteamId
    {
      get => MyMultiplayerLobby.GetLobbyULong("host_steamId", this.m_lobby, 0UL);
      set => this.m_lobby.SetData("host_steamId", value.ToString());
    }

    public override bool ExperimentalMode
    {
      get => MyMultiplayerLobby.GetLobbyBool("experimentalMode", this.m_lobby, false);
      set => this.m_lobby.SetData("experimentalMode", value.ToString());
    }

    public event MyLobbyDataUpdated OnLobbyDataUpdated;

    public MyMultiplayerLobby(
      MySyncLayer syncLayer,
      MyLobbyType lobbyType,
      uint maxPlayers,
      MyLobbyCreated action)
      : base(syncLayer, new EndpointId(Sync.MyId))
    {
      MyGameService.CreateLobby(lobbyType, maxPlayers, action);
      MyGameService.Networking.Chat.UpdateChatAvailability();
    }

    public void SetLobby(IMyLobby lobby)
    {
      this.m_lobby = lobby;
      this.ServerId = this.m_lobby.OwnerId;
      this.SyncLayer.RegisterClientEvents((MyMultiplayerBase) this);
      this.HostName = MyGameService.UserName;
      lobby.OnChatUpdated += new MyLobbyChatUpdated(this.Matchmaking_LobbyChatUpdate);
      lobby.OnChatReceived += new MessageReceivedDelegate(this.Matchmaking_LobbyChatMsg);
      lobby.OnDataReceived += new MyLobbyDataUpdated(this.lobby_OnDataReceived);
      this.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.MyMultiplayerLobby_ClientLeft);
      this.AcceptMemberSessions();
    }

    private void lobby_OnDataReceived(bool success, IMyLobby lobby, ulong memberOrLobby)
    {
      MyLobbyDataUpdated lobbyDataUpdated = this.OnLobbyDataUpdated;
      if (lobbyDataUpdated == null)
        return;
      lobbyDataUpdated(success, lobby, memberOrLobby);
    }

    private void MyMultiplayerLobby_ClientLeft(
      ulong userId,
      MyChatMemberStateChangeEnum stateChange)
    {
      if ((long) userId == (long) this.ServerId)
        MyGameService.Peer2Peer.CloseSession(userId);
      if (stateChange == MyChatMemberStateChangeEnum.Kicked || stateChange == MyChatMemberStateChangeEnum.Banned)
      {
        MyControlKickClientMsg message = new MyControlKickClientMsg()
        {
          KickedClient = userId,
          Kicked = (BoolBlit) true,
          Add = (BoolBlit) false
        };
        MyLog.Default.WriteLineAndConsole("Player " + this.GetMemberName(userId) + " kicked");
        this.SendControlMessageToAll<MyControlKickClientMsg>(ref message);
      }
      MySandboxGame.Log.WriteLineAndConsole("Player left: " + this.GetMemberName(userId) + " (" + (object) userId + ")");
    }

    public bool IsCorrectVersion() => MyMultiplayerLobby.IsLobbyCorrectVersion(this.m_lobby);

    private void Matchmaking_LobbyChatUpdate(
      IMyLobby lobby,
      ulong changedUser,
      ulong makingChangeUser,
      MyChatMemberStateChangeEnum stateChange,
      MyLobbyStatusCode reason)
    {
      if ((long) lobby.LobbyId != (long) this.m_lobby.LobbyId)
        return;
      if (stateChange == MyChatMemberStateChangeEnum.Entered)
      {
        if (MyVisualScriptLogicProvider.PlayerConnectRequest != null)
        {
          JoinResult result = JoinResult.OK;
          MyVisualScriptLogicProvider.PlayerConnectRequest(changedUser, ref result);
          if (result != JoinResult.OK)
          {
            this.RaiseClientLeft(changedUser, MyChatMemberStateChangeEnum.Disconnected);
            return;
          }
        }
        MyGameService.Networking.Chat.WarmupPlayerCache(changedUser, this.IsCrossMember(changedUser));
        string memberName = this.GetMemberName(changedUser);
        MySandboxGame.Log.WriteLineAndConsole("Player entered: " + memberName + " (" + (object) changedUser + ")");
        MyGameService.Peer2Peer.AcceptSession(changedUser);
        if (Sync.Clients == null || !Sync.Clients.HasClient(changedUser))
        {
          this.RaiseClientJoined(changedUser, memberName);
          if (this.Scenario && (long) changedUser != (long) Sync.MyId)
            this.SendAllMembersDataToClient(changedUser);
        }
        if (!MySandboxGame.IsGameReady || (long) changedUser == (long) this.ServerId)
          return;
        MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientConnected, 5000, level: MyNotificationLevel.Important);
        myHudNotification.SetTextFormatArguments((object) memberName);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      else
      {
        if (Sync.Clients == null || Sync.Clients.HasClient(changedUser))
          this.RaiseClientLeft(changedUser, stateChange);
        if ((long) changedUser == (long) this.ServerId)
        {
          this.RaiseHostLeft();
          StringBuilder errorMessage = MyJoinGameHelper.GetErrorMessage(reason);
          MyLog.Default.WriteLine("Matchmaking_LobbyChatUpdate: " + (object) reason + " / " + (object) errorMessage);
          MySessionLoader.UnloadAndExitToMenu();
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: errorMessage, messageCaption: messageCaption));
        }
        else
        {
          if (!MySandboxGame.IsGameReady)
            return;
          MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientDisconnected, 5000, level: MyNotificationLevel.Important);
          string memberName = this.GetMemberName(changedUser);
          myHudNotification.SetTextFormatArguments((object) memberName);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
        }
      }
    }

    private void Matchmaking_LobbyChatMsg(
      ulong memberId,
      string message,
      byte channel,
      long targetId,
      string author)
    {
      this.RaiseChatMessageReceived(memberId, message, (ChatChannel) channel, targetId, author);
    }

    private void AcceptMemberSessions()
    {
      foreach (ulong member in this.m_lobby.MemberList)
      {
        if ((long) member != (long) Sync.MyId && (long) member == (long) this.ServerId)
          MyGameService.Peer2Peer.AcceptSession(member);
      }
    }

    public override void DisconnectClient(ulong userId)
    {
      MyLog.Default.WriteLineAndConsole("User forcibly disconnected " + this.GetMemberName(userId));
      this.RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Disconnected);
    }

    public override void BanClient(ulong userId, bool banned)
    {
    }

    public override void Tick()
    {
      base.Tick();
      if (this.m_serverDataValid)
        return;
      if (this.AppVersion == 0)
        MySession.Static.StartServer((MyMultiplayerBase) this);
      this.m_serverDataValid = true;
    }

    public override void SendChatMessage(
      string text,
      ChatChannel channel,
      long targetId,
      string customAuthor)
    {
      this.m_lobby.SendChatMessage(text, (byte) channel, targetId, customAuthor);
    }

    public override void Dispose()
    {
      if (this.m_lobby != null)
      {
        this.m_lobby.OnChatUpdated -= new MyLobbyChatUpdated(this.Matchmaking_LobbyChatUpdate);
        this.m_lobby.OnChatReceived -= new MessageReceivedDelegate(this.Matchmaking_LobbyChatMsg);
        this.m_lobby.OnDataReceived -= new MyLobbyDataUpdated(this.lobby_OnDataReceived);
        if (this.m_lobby.IsValid)
        {
          this.CloseMemberSessions();
          this.m_lobby.Leave();
        }
      }
      base.Dispose();
      this.m_lobby = (IMyLobby) null;
    }

    public override IEnumerable<ulong> Members => this.m_lobby.MemberList;

    public override int MemberCount => this.m_lobby.MemberCount;

    public override bool IsSomeoneElseConnected => this.MemberCount >= 2;

    public override ulong LobbyId => this.m_lobby.LobbyId;

    public override int MemberLimit
    {
      get => this.m_lobby.MemberLimit;
      set
      {
      }
    }

    public override ulong GetOwner() => this.m_lobby.OwnerId;

    public override bool IsLobby => true;

    public override MyLobbyType GetLobbyType() => this.m_lobby.LobbyType;

    public override void SetLobbyType(MyLobbyType type) => this.m_lobby.LobbyType = type;

    public override void SetMemberLimit(int limit) => this.m_lobby.MemberLimit = limit;

    public static bool IsLobbyCorrectVersion(IMyLobby lobby) => MyMultiplayerLobby.GetLobbyAppVersion(lobby) == (int) MyFinalBuildConstants.APP_VERSION;

    public static MyGameModeEnum GetLobbyGameMode(IMyLobby lobby)
    {
      int result;
      return int.TryParse(lobby.GetData("gameMode"), out result) ? (MyGameModeEnum) result : MyGameModeEnum.Creative;
    }

    public static float GetLobbyFloat(string key, IMyLobby lobby, float defValue)
    {
      float result;
      return float.TryParse(lobby.GetData(key), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : defValue;
    }

    public static int GetLobbyInt(string key, IMyLobby lobby, int defValue)
    {
      int result;
      return int.TryParse(lobby.GetData(key), NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : defValue;
    }

    public static DateTime GetLobbyDateTime(string key, IMyLobby lobby, DateTime defValue)
    {
      DateTime result;
      return DateTime.TryParse(lobby.GetData(key), (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? result : defValue;
    }

    public static long GetLobbyLong(string key, IMyLobby lobby, long defValue)
    {
      long result;
      return long.TryParse(lobby.GetData(key), out result) ? result : defValue;
    }

    public static ulong GetLobbyULong(string key, IMyLobby lobby, ulong defValue)
    {
      ulong result;
      return ulong.TryParse(lobby.GetData(key), out result) ? result : defValue;
    }

    public static bool GetLobbyBool(string key, IMyLobby lobby, bool defValue)
    {
      bool result;
      return bool.TryParse(lobby.GetData(key), out result) ? result : defValue;
    }

    public static string GetLobbyWorldName(IMyLobby lobby) => lobby.GetData("world");

    public static ulong GetLobbyWorldSize(IMyLobby lobby)
    {
      ulong result;
      return ulong.TryParse(lobby.GetData("worldSize"), out result) ? result : 0UL;
    }

    public static string GetLobbyHostName(IMyLobby lobby) => lobby.GetData("host");

    public static int GetLobbyAppVersion(IMyLobby lobby)
    {
      int result;
      return !int.TryParse(lobby.GetData("appVersion"), out result) ? 0 : result;
    }

    public static ulong GetLobbyHostSteamId(IMyLobby lobby) => MyMultiplayerLobby.GetLobbyULong("host_steamId", lobby, 0UL);

    public static string GetDataHash(IMyLobby lobby) => lobby.GetData("dataHash");

    public static bool HasSameData(IMyLobby lobby)
    {
      string dataHash = MyMultiplayerLobby.GetDataHash(lobby);
      return dataHash == "" || dataHash == MyDataIntegrityChecker.GetHashBase64();
    }

    public static int GetLobbyModCount(IMyLobby lobby) => MyMultiplayerLobby.GetLobbyInt("mods", lobby, 0);

    public static List<MyObjectBuilder_Checkpoint.ModItem> GetLobbyMods(
      IMyLobby lobby)
    {
      int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(lobby);
      List<MyObjectBuilder_Checkpoint.ModItem> modItemList = new List<MyObjectBuilder_Checkpoint.ModItem>(lobbyModCount);
      for (int index = 0; index < lobbyModCount; ++index)
      {
        string str1 = lobby.GetData("mod" + (object) index) ?? string.Empty;
        int length1 = str1.IndexOf("_");
        if (length1 != -1)
        {
          ulong result;
          ulong.TryParse(str1.Substring(0, length1), out result);
          string str2 = str1.Substring(length1 + 1);
          int length2 = str2.IndexOf("_");
          if (length2 != -1)
          {
            string publishedServiceName = str2.Substring(0, length2);
            string str3 = str2.Substring(length2 + 1);
            modItemList.Add(new MyObjectBuilder_Checkpoint.ModItem(str3, result, publishedServiceName, str3));
            continue;
          }
        }
        MySandboxGame.Log.WriteLineAndConsole("Failed to parse mod details from LobbyData. '" + str1 + "'");
      }
      return modItemList;
    }

    public static int GetLobbyViewDistance(IMyLobby lobby) => MyMultiplayerLobby.GetLobbyInt("view", lobby, 20000);

    public static bool GetLobbyScenario(IMyLobby lobby) => MyMultiplayerLobby.GetLobbyBool("scenario", lobby, false);

    public static string GetLobbyScenarioBriefing(IMyLobby lobby) => lobby.GetData("scenarioBriefing");

    public override string GetMemberName(ulong steamUserID) => this.m_lobby.GetMemberName(steamUserID);

    public override string GetMemberServiceName(ulong steamUserID) => MyGameService.Service.ServiceName;

    protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
    {
    }
  }
}
