// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudChat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudChat
  {
    private static readonly int MAX_MESSAGES_IN_CHAT_DEFAULT = 10;
    private static readonly int MAX_MESSAGE_TIME_DEFAULT = 15000;
    public static int MaxMessageTime = MyHudChat.MAX_MESSAGE_TIME_DEFAULT;
    public static int MaxMessageCount = MyHudChat.MAX_MESSAGES_IN_CHAT_DEFAULT;
    public Queue<MyChatItem> MessagesQueue = new Queue<MyChatItem>();
    public List<MyChatItem> MessageHistory = new List<MyChatItem>();
    private int m_lastUpdateTime = int.MaxValue;
    private int m_lastScreenUpdateTime = int.MaxValue;
    public MyHudControlChat ChatControl;
    private bool m_chatScreenOpen;

    public int Timestamp { get; private set; }

    public MyHudChat() => this.Timestamp = 0;

    public void RegisterChat(MyMultiplayerBase multiplayer)
    {
      if (multiplayer == null)
        return;
      multiplayer.ChatMessageReceived += new Action<ulong, string, ChatChannel, long, string>(this.Multiplayer_ChatMessageReceived);
      multiplayer.ScriptedChatMessageReceived += new Action<string, string, string, Color>(this.multiplayer_ScriptedChatMessageReceived);
    }

    public void UnregisterChat(MyMultiplayerBase multiplayer)
    {
      if (multiplayer == null)
        return;
      multiplayer.ChatMessageReceived -= new Action<ulong, string, ChatChannel, long, string>(this.Multiplayer_ChatMessageReceived);
      multiplayer.ScriptedChatMessageReceived -= new Action<string, string, string, Color>(this.multiplayer_ScriptedChatMessageReceived);
      this.MessagesQueue.Clear();
      this.UpdateTimestamp();
    }

    public void ShowMessageScripted(string sender, string messageText)
    {
      Color paleGoldenrod = Color.PaleGoldenrod;
      Color white = Color.White;
      this.ShowMessage(sender, messageText, paleGoldenrod, white);
    }

    public void ShowMessage(string sender, string messageText, Color color, string font = "Blue")
    {
      MyChatItem myChatItem = new MyChatItem(sender, messageText, font, color);
      this.MessagesQueue.Enqueue(myChatItem);
      this.MessageHistory.Add(myChatItem);
      this.m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.MessagesQueue.Count > MyHudChat.MaxMessageCount)
        this.MessagesQueue.Dequeue();
      this.UpdateTimestamp();
    }

    public void ShowMessage(string sender, string messageText, string font = "Blue") => this.ShowMessage(sender, messageText, Color.White, font);

    public void ShowMessageColoredSP(
      string text,
      ChatChannel channel,
      long targetId = 0,
      string customAuthorName = null)
    {
      string empty = string.Empty;
      string str;
      if (channel == ChatChannel.Private)
      {
        if (targetId == MySession.Static.LocalPlayerId)
        {
          string displayName = MySession.Static.LocalHumanPlayer.DisplayName;
          str = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), (object) displayName);
        }
        else
          str = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), (object) targetId);
      }
      else
        str = MySession.Static.LocalHumanPlayer.DisplayName;
      long identityId = MySession.Static.Players.TryGetIdentityId(Sync.MyId, 0);
      Color relationColor = MyChatSystem.GetRelationColor(identityId);
      Color channelColor = MyChatSystem.GetChannelColor(channel);
      this.ShowMessage(string.IsNullOrEmpty(customAuthorName) ? str : customAuthorName, text, relationColor, channelColor);
      if (channel == ChatChannel.GlobalScripted)
        MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(text, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
      else
        MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, channel, identityId, targetId);
    }

    public void ShowMessage(string sender, string message, Color senderColor) => this.ShowMessage(sender, message, senderColor, Color.White);

    public void ShowMessage(string sender, string message, Color senderColor, Color messageColor)
    {
      MyChatItem myChatItem = new MyChatItem(sender, message, "White", senderColor, messageColor);
      this.MessagesQueue.Enqueue(myChatItem);
      this.MessageHistory.Add(myChatItem);
      this.m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.MessagesQueue.Count > MyHudChat.MaxMessageCount)
        this.MessagesQueue.Dequeue();
      this.UpdateTimestamp();
    }

    private void Multiplayer_ChatMessageReceived(
      ulong steamUserId,
      string messageText,
      ChatChannel channel,
      long targetId,
      string customAuthorName = null)
    {
      if (!MyGameService.IsActive)
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(steamUserId, 0);
      string str = string.Empty;
      if (channel == ChatChannel.Private)
      {
        if (targetId == MySession.Static.LocalPlayerId)
        {
          string memberName = MyMultiplayer.Static.GetMemberName(steamUserId);
          str = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), (object) memberName);
        }
        else
        {
          if (identityId != MySession.Static.LocalPlayerId)
            return;
          ulong steamId = MySession.Static.Players.TryGetSteamId(targetId);
          if (steamId != 0UL)
            str = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), (object) MyMultiplayer.Static.GetMemberName(steamId));
        }
      }
      else
        str = MyMultiplayer.Static.GetMemberName(steamUserId);
      Color relationColor = MyChatSystem.GetRelationColor(identityId);
      Color channelColor = MyChatSystem.GetChannelColor(channel);
      this.ShowMessage(string.IsNullOrEmpty(customAuthorName) ? str : customAuthorName, messageText, relationColor, channelColor);
      if (channel == ChatChannel.GlobalScripted)
        MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(messageText, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
      else
        MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(messageText, channel, identityId, targetId);
    }

    public void multiplayer_ScriptedChatMessageReceived(
      string message,
      string author,
      string font,
      Color color)
    {
      this.ShowMessage(author, message, color, font);
      MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(message, author, font);
    }

    private void UpdateTimestamp()
    {
      ++this.Timestamp;
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public int LastUpdateTime => this.m_lastUpdateTime;

    public int TimeSinceLastUpdate => MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastScreenUpdateTime;

    public void Update()
    {
      if (this.m_chatScreenOpen)
        this.m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime <= MyHudChat.MaxMessageTime || this.MessagesQueue.Count <= 0)
        return;
      this.MessagesQueue.Dequeue();
      this.UpdateTimestamp();
    }

    public static void ResetChatSettings()
    {
      MyHudChat.MaxMessageTime = MyHudChat.MAX_MESSAGE_TIME_DEFAULT;
      MyHudChat.MaxMessageCount = MyHudChat.MAX_MESSAGES_IN_CHAT_DEFAULT;
    }

    public void ChatOpened() => this.m_chatScreenOpen = true;

    public void ChatClosed() => this.m_chatScreenOpen = false;
  }
}
