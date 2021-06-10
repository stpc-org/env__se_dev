// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyDedicatedServer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  public class MyDedicatedServer : MyDedicatedServerBase
  {
    private float m_inventoryMultiplier;
    private float m_blocksInventoryMultiplier;
    private float m_assemblerMultiplier;
    private float m_refineryMultiplier;
    private float m_welderMultiplier;
    private float m_grinderMultiplier;
    private List<MyChatMessage> m_globalChatHistory = new List<MyChatMessage>();

    public List<MyChatMessage> GlobalChatHistory => this.m_globalChatHistory;

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

    public override bool Scenario { get; set; }

    public override string ScenarioBriefing { get; set; }

    public override DateTime ScenarioStartTime { get; set; }

    public override bool ExperimentalMode { get; set; }

    internal MyDedicatedServer(IPEndPoint serverEndpoint, Func<string, string> filterOffensive)
      : base(new MySyncLayer(new MyTransportLayer(2)), filterOffensive)
      => this.Initialize(serverEndpoint);

    internal override void SendGameTagsToSteam()
    {
      if (MyGameService.GameServer == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.GameMode)
      {
        case MyGameModeEnum.Creative:
          stringBuilder.Append("C");
          break;
        case MyGameModeEnum.Survival:
          stringBuilder.Append(string.Format("S{0}-{1}-{2}-{3}", (object) (int) this.InventoryMultiplier, (object) (int) this.BlocksInventoryMultiplier, (object) (int) this.AssemblerMultiplier, (object) (int) this.RefineryMultiplier));
          break;
      }
      MyGameService.GameServer.SetGameTags("groupId" + (object) this.m_groupId + " version" + (object) MyFinalBuildConstants.APP_VERSION + " datahash" + MyDataIntegrityChecker.GetHashBase64() + " mods" + (object) this.ModCount + " gamemode" + (object) stringBuilder + " view" + (object) this.SyncDistance);
      MyGameService.GameServer.SetGameData(MyFinalBuildConstants.APP_VERSION.ToString());
      MyGameService.GameServer.SetKeyValue("CONSOLE_COMPATIBLE", MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "1" : "0");
    }

    protected override void SendServerData()
    {
      ServerDataMsg msg = new ServerDataMsg()
      {
        WorldName = this.m_worldName,
        GameMode = this.m_gameMode,
        InventoryMultiplier = this.m_inventoryMultiplier,
        BlocksInventoryMultiplier = this.m_blocksInventoryMultiplier,
        AssemblerMultiplier = this.m_assemblerMultiplier,
        RefineryMultiplier = this.m_refineryMultiplier,
        WelderMultiplier = this.m_welderMultiplier,
        GrinderMultiplier = this.m_grinderMultiplier,
        HostName = this.m_hostName,
        WorldSize = this.m_worldSize,
        AppVersion = this.m_appVersion,
        MembersLimit = this.m_membersLimit,
        DataHash = this.m_dataHash,
        ServerPasswordSalt = MySandboxGame.ConfigDedicated.ServerPasswordSalt,
        ServerAnalyticsId = MySpaceAnalytics.Instance.UserId
      };
      this.ReplicationLayer.SendWorldData(ref msg);
    }

    public override void OnChatMessage(ref ChatMsg msg)
    {
      bool flag = false;
      MyMultiplayerBase.MyConnectedClientData data;
      if (this.MemberDataGet(msg.Author, out data) && data.IsAdmin | flag)
        MyServerDebugCommands.Process(msg.Text, msg.Author);
      string identityNameFromSteamId = Sync.Players.TryGetIdentityNameFromSteamId(msg.Author);
      if (string.IsNullOrEmpty(identityNameFromSteamId) && (long) msg.Author == (long) Sync.MyId)
        identityNameFromSteamId = MyTexts.GetString(MySpaceTexts.ChatBotName);
      if (!string.IsNullOrEmpty(identityNameFromSteamId))
        this.m_globalChatHistory.Add(new MyChatMessage()
        {
          SteamId = msg.Author,
          AuthorName = string.IsNullOrEmpty(msg.CustomAuthorName) ? identityNameFromSteamId : msg.CustomAuthorName,
          Text = msg.Text,
          Timestamp = DateTime.UtcNow
        });
      this.RaiseChatMessageReceived(msg.Author, msg.Text, (ChatChannel) msg.Channel, msg.TargetId, string.IsNullOrEmpty(msg.CustomAuthorName) ? (string) null : msg.CustomAuthorName);
    }
  }
}
