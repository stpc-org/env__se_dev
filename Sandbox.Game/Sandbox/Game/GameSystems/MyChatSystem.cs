// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyChatSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 900)]
  public class MyChatSystem : MySessionComponentBase
  {
    private MyChatCommandSystem m_commandSystem = new MyChatCommandSystem();
    private ChatChannel m_currentChannel;
    private long m_sourceFactionId;
    private long m_targetPlayerId;
    public MyUnifiedChatHistory ChatHistory = new MyUnifiedChatHistory();
    private int m_frameCount;

    public static void AddFactionChatItem(MyUnifiedChatItem chatItem) => MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref chatItem);

    public event Action<long> PlayerMessageReceived;

    public event Action<long> FactionMessageReceived;

    public MyChatCommandSystem CommandSystem => this.m_commandSystem;

    public ChatChannel CurrentChannel => this.m_currentChannel;

    public long CurrentTarget
    {
      get
      {
        switch (this.m_currentChannel)
        {
          case ChatChannel.Global:
            return 0;
          case ChatChannel.Faction:
            IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
            return playerFaction == null ? 0L : playerFaction.FactionId;
          case ChatChannel.Private:
            return this.m_targetPlayerId;
          default:
            return 0;
        }
      }
    }

    public void ChangeChatChannel_Global()
    {
      this.m_currentChannel = ChatChannel.Global;
      this.m_sourceFactionId = 0L;
      this.m_targetPlayerId = 0L;
    }

    public void ChangeChatChannel_Faction()
    {
      this.m_currentChannel = ChatChannel.Faction;
      this.m_sourceFactionId = 0L;
      this.m_targetPlayerId = 0L;
    }

    public void ChangeChatChannel_Whisper(long playerId)
    {
      this.m_currentChannel = ChatChannel.Private;
      this.m_sourceFactionId = 0L;
      this.m_targetPlayerId = playerId;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.CommandSystem.Init();
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      this.CommandSystem.Init();
    }

    protected override void UnloadData()
    {
      this.CommandSystem.Unload();
      base.UnloadData();
    }

    public void OnNewFactionMessage(ref MyUnifiedChatItem item)
    {
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
      if (Sync.IsDedicated || playerFaction == null && MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      Action<long> factionMessageReceived = this.FactionMessageReceived;
      if (factionMessageReceived == null)
        return;
      factionMessageReceived(item.TargetId);
    }

    private void ShowNewMessageHudNotification(MyHudNotification notification) => MyHud.Notifications.Add((MyHudNotificationBase) notification);

    public override void UpdateAfterSimulation()
    {
    }

    public void SendNewFactionMessage(MyUnifiedChatItem chatItem) => MyMultiplayer.RaiseStaticEvent<MyUnifiedChatItem>((Func<IMyEventOwner, Action<MyUnifiedChatItem>>) (x => new Action<MyUnifiedChatItem>(MyChatSystem.OnFactionMessageRequest)), chatItem);

    [Event(null, 148)]
    [Reliable]
    [Server(ValidationType.Controlled)]
    private static void OnFactionMessageRequest(MyUnifiedChatItem item)
    {
      if (item.Text.Length == 0 || item.Text.Length > 200)
        return;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(item.SenderId);
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(item.TargetId);
      if (identity == null || factionById == null)
        return;
      bool flag = false;
      ulong steamId1 = 0;
      MyPlayer.PlayerId result1;
      if (!factionById.IsMember(item.SenderId) && MySession.Static.Players.TryGetPlayerId(item.SenderId, out result1))
      {
        steamId1 = result1.SteamId;
        flag |= MySession.Static.IsUserAdmin(steamId1);
      }
      foreach (KeyValuePair<long, MyFactionMember> member in factionById.Members)
      {
        MyPlayer.PlayerId result2;
        MySession.Static.Players.TryGetPlayerId(member.Value.PlayerId, out result2);
        ulong steamId2 = result2.SteamId;
        if (steamId2 != 0UL)
          MyMultiplayer.RaiseStaticEvent<MyUnifiedChatItem>((Func<IMyEventOwner, Action<MyUnifiedChatItem>>) (x => new Action<MyUnifiedChatItem>(MyChatSystem.OnFactionMessageSuccess)), item, new EndpointId(steamId2));
      }
      if (!flag)
        return;
      MyMultiplayer.RaiseStaticEvent<MyUnifiedChatItem>((Func<IMyEventOwner, Action<MyUnifiedChatItem>>) (x => new Action<MyUnifiedChatItem>(MyChatSystem.OnFactionMessageSuccess)), item, new EndpointId(steamId1));
    }

    [Event(null, 191)]
    [Reliable]
    [Client]
    private static void OnFactionMessageSuccess(MyUnifiedChatItem item)
    {
      long senderId = item.SenderId;
      if (!Sync.IsServer || senderId == MySession.Static.LocalPlayerId)
      {
        MyChatSystem.AddFactionChatItem(item);
        if (MyMultiplayer.Static != null)
        {
          ulong steamId = MySession.Static.Players.TryGetSteamId(item.SenderId);
          ChatMsg msg = new ChatMsg()
          {
            Text = item.Text,
            Author = steamId,
            Channel = (byte) item.Channel,
            TargetId = item.TargetId,
            CustomAuthorName = string.Empty
          };
          MyMultiplayer.Static.OnChatMessage(ref msg);
        }
      }
      if (senderId != MySession.Static.LocalPlayerId)
        MySession.Static.Gpss.ScanText(item.Text, MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromFactionComms));
      MySession.Static.ChatSystem.OnNewFactionMessage(ref item);
    }

    public static Color GetRelationColor(long identityId)
    {
      if (MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(identityId)))
        return Color.Purple;
      switch (MyIDModule.GetRelationPlayerPlayer(MySession.Static.LocalPlayerId, identityId))
      {
        case MyRelationsBetweenPlayers.Self:
          return Color.CornflowerBlue;
        case MyRelationsBetweenPlayers.Allies:
          return Color.LightGreen;
        case MyRelationsBetweenPlayers.Neutral:
          return Color.PaleGoldenrod;
        case MyRelationsBetweenPlayers.Enemies:
          return Color.Crimson;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static Color GetChannelColor(ChatChannel channel)
    {
      switch (channel)
      {
        case ChatChannel.Global:
        case ChatChannel.GlobalScripted:
        case ChatChannel.ChatBot:
          return Color.White;
        case ChatChannel.Faction:
          return Color.LimeGreen;
        case ChatChannel.Private:
          return Color.Violet;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected sealed class OnFactionMessageRequest\u003C\u003ESandbox_Game_Entities_Character_MyUnifiedChatItem : ICallSite<IMyEventOwner, MyUnifiedChatItem, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyUnifiedChatItem item,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyChatSystem.OnFactionMessageRequest(item);
      }
    }

    protected sealed class OnFactionMessageSuccess\u003C\u003ESandbox_Game_Entities_Character_MyUnifiedChatItem : ICallSite<IMyEventOwner, MyUnifiedChatItem, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyUnifiedChatItem item,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyChatSystem.OnFactionMessageSuccess(item);
      }
    }
  }
}
