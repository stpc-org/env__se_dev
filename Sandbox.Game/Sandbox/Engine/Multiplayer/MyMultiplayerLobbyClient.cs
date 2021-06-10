// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerLobbyClient
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
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Engine.Multiplayer
{
  public sealed class MyMultiplayerLobbyClient : MyMultiplayerClientBase
  {
    private readonly IMyLobby m_lobby;
    private bool m_serverDataValid;

    public override bool IsLobby => true;

    protected override bool IsServerInternal => false;

    public override string WorldName
    {
      get => MyMultiplayerLobbyClient.GetLobbyWorldName(this.m_lobby);
      set => this.m_lobby.SetData("world", value);
    }

    public override MyGameModeEnum GameMode
    {
      get => MyMultiplayerLobbyClient.GetLobbyGameMode(this.m_lobby);
      set => this.m_lobby.SetData("gameMode", ((int) value).ToString());
    }

    public override float InventoryMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("inventoryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("inventoryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float BlocksInventoryMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("blocksInventoryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("blocksInventoryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float AssemblerMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("assemblerMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("assemblerMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float RefineryMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("refineryMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("refineryMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float WelderMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("welderMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("welderMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override float GrinderMultiplier
    {
      get => MyMultiplayerLobbyClient.GetLobbyFloat("grinderMultiplier", this.m_lobby, 1f);
      set => this.m_lobby.SetData("grinderMultiplier", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override string HostName
    {
      get => MyMultiplayerLobbyClient.GetLobbyHostName(this.m_lobby);
      set => this.m_lobby.SetData("host", value);
    }

    public override ulong WorldSize
    {
      get => MyMultiplayerLobbyClient.GetLobbyWorldSize(this.m_lobby);
      set => this.m_lobby.SetData("worldSize", value.ToString());
    }

    public override int AppVersion
    {
      get => MyMultiplayerLobbyClient.GetLobbyAppVersion(this.m_lobby);
      set => this.m_lobby.SetData("appVersion", value.ToString());
    }

    public override string DataHash
    {
      get => this.m_lobby.GetData("dataHash");
      set => this.m_lobby.SetData("dataHash", value);
    }

    public override int MaxPlayers => MyMultiplayerLobby.MAX_PLAYERS;

    public override int ModCount
    {
      get => MyMultiplayerLobbyClient.GetLobbyModCount(this.m_lobby);
      protected set => this.m_lobby.SetData("mods", value.ToString());
    }

    public override List<MyObjectBuilder_Checkpoint.ModItem> Mods
    {
      get => MyMultiplayerLobbyClient.GetLobbyMods(this.m_lobby);
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
      get => MyMultiplayerLobbyClient.GetLobbyViewDistance(this.m_lobby);
      set => this.m_lobby.SetData("view", value.ToString());
    }

    public override bool Scenario
    {
      get => MyMultiplayerLobbyClient.GetLobbyBool("scenario", this.m_lobby, false);
      set => this.m_lobby.SetData("scenario", value.ToString());
    }

    public override string ScenarioBriefing
    {
      get => this.m_lobby.GetData("scenarioBriefing");
      set => this.m_lobby.SetData("scenarioBriefing", value == null || value.Length < 1 ? " " : value);
    }

    public override DateTime ScenarioStartTime
    {
      get => MyMultiplayerLobbyClient.GetLobbyDateTime("scenarioStartTime", this.m_lobby, DateTime.MinValue);
      set => this.m_lobby.SetData("scenarioStartTime", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override bool ExperimentalMode
    {
      get => MyMultiplayerLobbyClient.GetLobbyBool("experimentalMode", this.m_lobby, false);
      set => this.m_lobby.SetData("experimentalMode", value.ToString());
    }

    public event MyLobbyDataUpdated OnLobbyDataUpdated;

    internal MyMultiplayerLobbyClient(IMyLobby lobby, MySyncLayer syncLayer)
      : base(syncLayer)
    {
      this.m_lobby = lobby;
      this.ServerId = this.m_lobby.OwnerId;
      MyGameService.Networking.Chat.UpdateChatAvailability();
      MyGameService.Networking.Chat.WarmupPlayerCache(this.ServerId, false);
      this.SyncLayer.RegisterClientEvents((MyMultiplayerBase) this);
      this.SyncLayer.TransportLayer.IsBuffering = true;
      if (!this.SyncLayer.Clients.HasClient(this.ServerId))
        this.SyncLayer.Clients.AddClient(this.ServerId, this.HostName);
      this.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.MyMultiplayerLobby_ClientLeft);
      lobby.OnChatUpdated += new MyLobbyChatUpdated(this.Matchmaking_LobbyChatUpdate);
      lobby.OnChatReceived += new MessageReceivedDelegate(this.Matchmaking_LobbyChatMsg);
      lobby.OnDataReceived += new MyLobbyDataUpdated(this.lobby_OnDataReceived);
      this.AcceptMemberSessions();
      this.IsServerExperimental = this.ExperimentalMode;
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
      MySandboxGame.Log.WriteLineAndConsole("Player left: " + this.GetMemberName(userId) + " (" + (object) userId + ")");
    }

    public bool IsCorrectVersion() => MyMultiplayerLobbyClient.IsLobbyCorrectVersion(this.m_lobby);

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
        string memberName = this.GetMemberName(changedUser);
        MySandboxGame.Log.WriteLineAndConsole("Player entered: " + memberName + " (" + (object) changedUser + ")");
        MyGameService.Peer2Peer.AcceptSession(changedUser);
        MyGameService.Networking.Chat.WarmupPlayerCache(changedUser, this.IsCrossMember(changedUser));
        if (Sync.Clients == null || !Sync.Clients.HasClient(changedUser))
          this.RaiseClientJoined(changedUser, memberName);
        if (!MySandboxGame.IsGameReady || (long) changedUser == (long) this.ServerId || MyHud.Notifications == null)
          return;
        MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.NotificationClientConnected, 5000, level: MyNotificationLevel.Important);
        myHudNotification.SetTextFormatArguments((object) memberName);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      else
      {
        if (Sync.Clients == null || Sync.Clients.HasClient(changedUser))
          this.RaiseClientLeft(changedUser, stateChange);
        if ((long) changedUser == (long) this.ServerId || (long) changedUser == (long) MyGameService.UserId)
        {
          this.RaiseHostLeft();
          StringBuilder messageText = reason != MyLobbyStatusCode.Success ? MyJoinGameHelper.GetErrorMessage(reason) : ((long) changedUser != (long) MyGameService.UserId ? MyTexts.Get(MyCommonTexts.LobbyConnectionProblems) : MyTexts.Get(MyCommonTexts.MultiplayerErrorServerHasLeft));
          MyLog.Default.WriteLine("Matchmaking_LobbyChatUpdate: " + (object) reason + " / " + (object) messageText + " / " + (object) stateChange);
          MySessionLoader.UnloadAndExitToMenu();
          if (MyGuiScreenServerReconnector.ReconnectToLastSession() != null)
            return;
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: messageText, messageCaption: messageCaption));
        }
        else
        {
          if (!MySandboxGame.IsGameReady || MyHud.Notifications == null)
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
      foreach (ulong member in this.Members)
      {
        if ((long) member != (long) Sync.MyId && (long) member == (long) this.ServerId)
          MyGameService.Peer2Peer.AcceptSession(member);
      }
    }

    public override void DisconnectClient(ulong userId) => this.RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Disconnected);

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
      if (channel == ChatChannel.GlobalScripted)
        return;
      this.m_lobby.SendChatMessage(text, (byte) channel, targetId, customAuthor);
    }

    public override void Dispose()
    {
      this.m_lobby.OnChatUpdated -= new MyLobbyChatUpdated(this.Matchmaking_LobbyChatUpdate);
      this.m_lobby.OnChatReceived -= new MessageReceivedDelegate(this.Matchmaking_LobbyChatMsg);
      if (this.m_lobby.IsValid)
      {
        this.CloseMemberSessions();
        this.m_lobby.Leave();
      }
      base.Dispose();
    }

    public override IEnumerable<ulong> Members => this.m_lobby.MemberList;

    public override int MemberCount => this.m_lobby.MemberCount;

    public override bool IsSomeoneElseConnected => true;

    public override ulong LobbyId => this.m_lobby.LobbyId;

    public override int MemberLimit
    {
      get => this.m_lobby.MemberLimit;
      set
      {
      }
    }

    public override ulong GetOwner() => this.m_lobby.OwnerId;

    public override MyLobbyType GetLobbyType() => this.m_lobby.LobbyType;

    public override void SetLobbyType(MyLobbyType type) => this.m_lobby.LobbyType = type;

    public override void SetMemberLimit(int limit) => this.m_lobby.MemberLimit = limit;

    public static bool IsLobbyCorrectVersion(IMyLobby lobby) => MyMultiplayerLobbyClient.GetLobbyAppVersion(lobby) == (int) MyFinalBuildConstants.APP_VERSION;

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
      string data = lobby.GetData("worldSize");
      return !string.IsNullOrEmpty(data) ? Convert.ToUInt64(data) : 0UL;
    }

    public static string GetLobbyHostName(IMyLobby lobby) => lobby.GetData("host");

    public static int GetLobbyAppVersion(IMyLobby lobby)
    {
      int result;
      return !int.TryParse(lobby.GetData("appVersion"), out result) ? 0 : result;
    }

    public static string GetDataHash(IMyLobby lobby) => lobby.GetData("dataHash");

    public static bool HasSameData(IMyLobby lobby)
    {
      string dataHash = MyMultiplayerLobbyClient.GetDataHash(lobby);
      return dataHash == "" || dataHash == MyDataIntegrityChecker.GetHashBase64();
    }

    public static int GetLobbyModCount(IMyLobby lobby) => MyMultiplayerLobbyClient.GetLobbyInt("mods", lobby, 0);

    public static List<MyObjectBuilder_Checkpoint.ModItem> GetLobbyMods(
      IMyLobby lobby)
    {
      int lobbyModCount = MyMultiplayerLobbyClient.GetLobbyModCount(lobby);
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
          int length2 = str1.IndexOf("_");
          if (length2 != -1)
          {
            string publishedServiceName = str2.Substring(0, length2);
            string str3 = str2.Substring(length2 + 1);
            modItemList.Add(new MyObjectBuilder_Checkpoint.ModItem(str3, result, publishedServiceName, str3));
          }
        }
        else
          MySandboxGame.Log.WriteLineAndConsole(string.Format("Failed to parse mod details from LobbyData. '{0}'", (object) str1));
      }
      return modItemList;
    }

    public static int GetLobbyViewDistance(IMyLobby lobby) => MyMultiplayerLobbyClient.GetLobbyInt("view", lobby, 20000);

    public static bool GetLobbyScenario(IMyLobby lobby) => MyMultiplayerLobbyClient.GetLobbyBool("scenario", lobby, false);

    public static string GetLobbyScenarioBriefing(IMyLobby lobby) => lobby.GetData("scenarioBriefing");

    public override string GetMemberName(ulong steamUserID) => this.m_lobby.GetMemberName(steamUserID);

    public override string GetMemberServiceName(ulong steamUserID) => MyGameService.Service.ServiceName;

    protected override void OnClientBan(ref MyControlBanClientMsg data, ulong sender)
    {
    }

    protected override void OnAllMembersData(ref AllMembersDataMsg msg)
    {
      int num = Sync.IsServer ? 1 : 0;
      this.ProcessAllMembersData(ref msg);
    }
  }
}
