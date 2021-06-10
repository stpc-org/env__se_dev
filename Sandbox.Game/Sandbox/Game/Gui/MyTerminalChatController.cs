// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalChatController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTerminalChatController : MyTerminalController
  {
    private MyGuiControlListbox m_playerList;
    private MyGuiControlListbox m_factionList;
    private MyGuiControlListbox.Item m_chatBotItem;
    private MyGuiControlListbox.Item m_broadcastItem;
    private MyGuiControlListbox.Item m_globalItem;
    private MyGuiControlMultilineText m_chatHistory;
    private MyGuiControlTextbox m_chatbox;
    private MyGuiControlButton m_sendButton;
    private StringBuilder m_chatboxText = new StringBuilder();
    private StringBuilder m_tempStringBuilder = new StringBuilder();
    private bool m_closed = true;
    private bool m_pendingUpdatePlayerList;
    private bool m_waitedOneFrameBeforeUpdating;
    private int m_frameCount;

    private bool AllowPlayerDrivenChat
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsTextChatAvailable;
      }
    }

    public void Init(IMyGuiControlsParent controlsParent)
    {
      this.m_playerList = (MyGuiControlListbox) controlsParent.Controls.GetControlByName("PlayerListbox");
      this.m_factionList = (MyGuiControlListbox) controlsParent.Controls.GetControlByName("FactionListbox");
      this.m_chatHistory = (MyGuiControlMultilineText) controlsParent.Controls.GetControlByName("ChatHistory");
      this.m_chatbox = (MyGuiControlTextbox) controlsParent.Controls.GetControlByName("Chatbox");
      this.m_chatbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminaMessageBox));
      this.m_playerList.ItemsSelected += new Action<MyGuiControlListbox>(this.m_playerList_ItemsSelected);
      this.m_playerList.MultiSelect = false;
      this.m_factionList.ItemsSelected += new Action<MyGuiControlListbox>(this.m_factionList_ItemsSelected);
      this.m_factionList.MultiSelect = false;
      this.m_sendButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("SendButton");
      this.m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessage));
      this.m_sendButton.ButtonClicked += new Action<MyGuiControlButton>(this.m_sendButton_ButtonClicked);
      this.m_sendButton.ShowTooltipWhenDisabled = true;
      this.m_chatbox.TextChanged += new Action<MyGuiControlTextbox>(this.m_chatbox_TextChanged);
      this.m_chatbox.EnterPressed += new Action<MyGuiControlTextbox>(this.m_chatbox_EnterPressed);
      if (MySession.Static.LocalCharacter != null)
      {
        MySession.Static.ChatSystem.PlayerMessageReceived += new Action<long>(this.MyChatSystem_PlayerMessageReceived);
        MySession.Static.ChatSystem.FactionMessageReceived += new Action<long>(this.MyChatSystem_FactionMessageReceived);
      }
      MySession.Static.Players.PlayersChanged += new Action<bool, MyPlayer.PlayerId>(this.Players_PlayersChanged);
      this.RefreshLists();
      this.m_chatbox.Text = string.Empty;
      this.m_sendButton.Enabled = false;
      if (MyMultiplayer.Static != null)
        MyMultiplayer.Static.ChatMessageReceived += new Action<ulong, string, ChatChannel, long, string>(this.Multiplayer_ChatMessageReceived);
      this.m_closed = false;
    }

    private void m_chatbox_TextChanged(MyGuiControlTextbox obj)
    {
      this.m_chatboxText.Clear();
      obj.GetText(this.m_chatboxText);
      if (this.m_chatboxText.Length == 0)
      {
        this.m_sendButton.Enabled = false;
        this.m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessageDisabled));
      }
      else
      {
        if (MySession.Static.LocalCharacter != null)
        {
          this.m_sendButton.Enabled = true;
          this.m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessage));
        }
        if (this.m_chatboxText.Length <= 200)
          return;
        this.m_chatboxText.Length = 200;
        this.m_chatbox.SetText(this.m_chatboxText);
      }
    }

    private void m_chatbox_EnterPressed(MyGuiControlTextbox obj)
    {
      if (this.m_chatboxText.Length <= 0)
        return;
      this.SendMessage();
    }

    private void m_sendButton_ButtonClicked(MyGuiControlButton obj) => this.SendMessage();

    private void m_playerList_ItemsSelected(MyGuiControlListbox obj)
    {
      if (this.m_playerList.SelectedItems.Count <= 0)
        return;
      MyGuiControlListbox.Item selectedItem = this.m_playerList.SelectedItems[0];
      if (selectedItem == this.m_globalItem)
        this.RefreshGlobalChatHistory();
      else if (selectedItem == this.m_chatBotItem)
        this.RefreshChatBotHistory();
      else
        this.RefreshPlayerChatHistory((MyIdentity) selectedItem.UserData);
      this.m_chatbox.Text = string.Empty;
      this.m_chatbox.Enabled = this.AllowPlayerDrivenChat || selectedItem == this.m_chatBotItem;
    }

    private void m_factionList_ItemsSelected(MyGuiControlListbox obj)
    {
      if (this.m_factionList.SelectedItems.Count <= 0)
        return;
      this.RefreshFactionChatHistory((MyFaction) this.m_factionList.SelectedItems[0].UserData);
      this.m_chatbox.Enabled = true;
      this.m_chatbox.Text = string.Empty;
    }

    private void MyChatSystem_PlayerMessageReceived(long playerId)
    {
      if (this.m_playerList == null || this.m_playerList.SelectedItems == null || this.m_playerList.SelectedItems.Count <= 0)
        return;
      MyIdentity userData = (MyIdentity) this.m_playerList.SelectedItems[0].UserData;
    }

    private void MyChatSystem_FactionMessageReceived(long factionId)
    {
      if (this.m_factionList.SelectedItems.Count <= 0)
        return;
      MyFaction userData = (MyFaction) this.m_factionList.SelectedItems[0].UserData;
      if (userData.FactionId != factionId)
        return;
      this.RefreshFactionChatHistory(userData);
    }

    private void OnChatBotResponse(string text)
    {
      if (MySession.Static == null)
        return;
      MyUnifiedChatItem chatbotMessage = MyUnifiedChatItem.CreateChatbotMessage(text, DateTime.UtcNow, 0L, MySession.Static.LocalPlayerId, MyTexts.GetString(MySpaceTexts.ChatBotName));
      MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref chatbotMessage);
      this.RefreshChatBotHistory();
    }

    private void Multiplayer_ChatMessageReceived(
      ulong steamUserId,
      string messageText,
      ChatChannel channel,
      long targetId,
      string customAuthorName = null)
    {
      if (this.m_playerList.SelectedItems.Count > 0)
      {
        MyGuiControlListbox.Item selectedItem = this.m_playerList.SelectedItems[0];
        if (selectedItem == this.m_globalItem)
          this.RefreshGlobalChatHistory();
        else if (selectedItem == this.m_chatBotItem)
        {
          this.RefreshChatBotHistory();
        }
        else
        {
          MyIdentity userData = (MyIdentity) selectedItem.UserData;
          MyPlayer.PlayerId result;
          if (!MySession.Static.Players.TryGetPlayerId(userData.IdentityId, out result) || (MySession.Static.LocalPlayerId != targetId || (long) result.SteamId != (long) steamUserId) && (long) MySession.Static.LocalHumanPlayer.Id.SteamId != (long) steamUserId)
            return;
          this.RefreshPlayerChatHistory(userData);
        }
      }
      else
      {
        if (this.m_factionList.SelectedItems.Count <= 0)
          return;
        MyFaction userData = (MyFaction) this.m_factionList.SelectedItems[0].UserData;
        if (userData.FactionId != targetId)
          return;
        this.RefreshFactionChatHistory(userData);
      }
    }

    private void Players_PlayersChanged(bool added, MyPlayer.PlayerId playerId)
    {
      if (this.m_closed)
        return;
      this.UpdatePlayerList();
    }

    private void SendMessage()
    {
      if (MySession.Static.LocalCharacter == null)
        return;
      this.m_chatboxText.Clear();
      this.m_chatbox.GetText(this.m_chatboxText);
      if (this.m_playerList.SelectedItems.Count > 0)
      {
        MyGuiControlListbox.Item selectedItem = this.m_playerList.SelectedItems[0];
        if (this.AllowPlayerDrivenChat || selectedItem == this.m_chatBotItem)
        {
          if (selectedItem == this.m_globalItem)
          {
            if (MyMultiplayer.Static != null)
              MyMultiplayer.Static.SendChatMessage(this.m_chatboxText.ToString(), ChatChannel.Global);
            else if (MyGameService.IsActive)
              MyHud.Chat.ShowMessageColoredSP(this.m_chatboxText.ToString(), ChatChannel.Global);
            else
              MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer?.DisplayName ?? "Player", this.m_chatboxText.ToString());
            this.RefreshGlobalChatHistory();
          }
          else if (selectedItem == this.m_chatBotItem)
          {
            string str = this.m_chatboxText.ToString();
            MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(str, ChatChannel.ChatBot, MySession.Static.LocalPlayerId, -1L, new DateTime?(DateTime.UtcNow));
            this.RefreshChatBotHistory();
            if (MySession.Static.ChatBot != null && !MySession.Static.ChatBot.FilterMessage(str, new Action<string>(this.OnChatBotResponse)))
              MySession.Static.ChatBot.FilterMessage("? " + str, new Action<string>(this.OnChatBotResponse));
          }
          else
          {
            MyIdentity userData = (MyIdentity) selectedItem.UserData;
            MyMultiplayer.Static.SendChatMessage(this.m_chatboxText.ToString(), ChatChannel.Private, userData.IdentityId);
            this.RefreshPlayerChatHistory(userData);
          }
        }
      }
      else if (this.m_factionList.SelectedItems.Count > 0)
      {
        MyFaction userData = (MyFaction) this.m_factionList.SelectedItems[0].UserData;
        if (!userData.IsMember(MySession.Static.LocalPlayerId))
          return;
        if (MyMultiplayer.Static != null)
          MyMultiplayer.Static.SendChatMessage(this.m_chatboxText.ToString(), ChatChannel.Faction, userData.FactionId);
        else if (MyGameService.IsActive)
          MyHud.Chat.ShowMessageColoredSP(this.m_chatboxText.ToString(), ChatChannel.Faction, userData.FactionId);
        else
          MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer?.DisplayName ?? "Player", this.m_chatboxText.ToString());
        this.RefreshFactionChatHistory(userData);
      }
      this.m_chatbox.Text = string.Empty;
    }

    private void RefreshPlayerChatHistory(MyIdentity playerIdentity)
    {
      if (playerIdentity == null || MySession.Static.ChatSystem == null)
        return;
      this.m_chatHistory.Clear();
      List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
      MySession.Static.ChatSystem.ChatHistory.GetPrivateHistory(ref list, playerIdentity.IdentityId);
      foreach (MyUnifiedChatItem myUnifiedChatItem in list)
      {
        if (myUnifiedChatItem != null)
        {
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(myUnifiedChatItem.SenderId);
          if (identity != null)
          {
            Color relationColor = MyChatSystem.GetRelationColor(myUnifiedChatItem.SenderId);
            Color channelColor = MyChatSystem.GetChannelColor(myUnifiedChatItem.Channel);
            this.m_chatHistory.AppendText(identity.DisplayName, "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
            this.m_chatHistory.AppendText(": ", "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
            this.m_chatHistory.AppendText(myUnifiedChatItem.Text, "White", this.m_chatHistory.TextScale, (Vector4) channelColor);
            this.m_chatHistory.AppendLine();
          }
        }
      }
      this.m_factionList.SelectedItems.Clear();
      this.m_chatHistory.ScrollbarOffsetV = 1f;
    }

    private void RefreshFactionChatHistory(MyFaction faction)
    {
      this.m_chatHistory.Clear();
      if (MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId) == null && !MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
      MySession.Static.ChatSystem.ChatHistory.GetFactionHistory(ref list, faction.FactionId);
      foreach (MyUnifiedChatItem myUnifiedChatItem in list)
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(myUnifiedChatItem.SenderId);
        if (identity != null)
        {
          Color relationColor = MyChatSystem.GetRelationColor(myUnifiedChatItem.SenderId);
          Color channelColor = MyChatSystem.GetChannelColor(myUnifiedChatItem.Channel);
          this.m_chatHistory.AppendText(identity.DisplayName, "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
          this.m_chatHistory.AppendText(": ", "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
          this.m_chatHistory.AppendText(myUnifiedChatItem.Text, "White", this.m_chatHistory.TextScale, (Vector4) channelColor);
          this.m_chatHistory.AppendLine();
        }
      }
      this.m_playerList.SelectedItems.Clear();
      this.m_chatHistory.ScrollbarOffsetV = 1f;
    }

    private void RefreshGlobalChatHistory()
    {
      this.m_chatHistory.Clear();
      List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
      if (this.AllowPlayerDrivenChat)
        MySession.Static.ChatSystem.ChatHistory.GetGeneralHistory(ref list);
      else
        list.Add(new MyUnifiedChatItem()
        {
          Channel = ChatChannel.GlobalScripted,
          Text = MyTexts.GetString(MyCommonTexts.ChatRestricted),
          AuthorFont = "White"
        });
      foreach (MyUnifiedChatItem myUnifiedChatItem in list)
      {
        if (myUnifiedChatItem.Channel == ChatChannel.GlobalScripted)
        {
          Color relationColor = MyChatSystem.GetRelationColor(myUnifiedChatItem.SenderId);
          Color channelColor = MyChatSystem.GetChannelColor(myUnifiedChatItem.Channel);
          if (myUnifiedChatItem.CustomAuthor.Length > 0)
            this.m_chatHistory.AppendText(myUnifiedChatItem.CustomAuthor + ": ", myUnifiedChatItem.AuthorFont, this.m_chatHistory.TextScale, (Vector4) relationColor);
          else
            this.m_chatHistory.AppendText(MyTexts.GetString(MySpaceTexts.ChatBotName) + ": ", myUnifiedChatItem.AuthorFont, this.m_chatHistory.TextScale, (Vector4) relationColor);
          this.m_chatHistory.AppendText(myUnifiedChatItem.Text, "White", this.m_chatHistory.TextScale, (Vector4) channelColor);
          this.m_chatHistory.AppendLine();
        }
        else if (myUnifiedChatItem.Channel == ChatChannel.Global)
        {
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(myUnifiedChatItem.SenderId);
          if (identity != null)
          {
            Color relationColor = MyChatSystem.GetRelationColor(myUnifiedChatItem.SenderId);
            Color channelColor = MyChatSystem.GetChannelColor(myUnifiedChatItem.Channel);
            this.m_chatHistory.AppendText(identity.DisplayName, "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
            this.m_chatHistory.AppendText(": ", "White", this.m_chatHistory.TextScale, (Vector4) relationColor);
            this.m_chatHistory.AppendText(myUnifiedChatItem.Text, "White", this.m_chatHistory.TextScale, (Vector4) channelColor);
            this.m_chatHistory.AppendLine();
          }
        }
      }
      this.m_factionList.SelectedItems.Clear();
      this.m_chatHistory.ScrollbarOffsetV = 1f;
    }

    private void RefreshChatBotHistory()
    {
      this.m_chatHistory.Clear();
      List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
      MySession.Static.ChatSystem.ChatHistory.GetChatbotHistory(ref list);
      foreach (MyUnifiedChatItem myUnifiedChatItem in list)
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(myUnifiedChatItem.SenderId != 0L ? myUnifiedChatItem.SenderId : myUnifiedChatItem.TargetId);
        if (identity != null)
        {
          Vector4 one = Vector4.One;
          Color white = Color.White;
          this.m_chatHistory.AppendText(myUnifiedChatItem.CustomAuthor.Length > 0 ? myUnifiedChatItem.CustomAuthor : identity.DisplayName, "White", this.m_chatHistory.TextScale, one);
          this.m_chatHistory.AppendText(": ", "White", this.m_chatHistory.TextScale, one);
          this.m_chatHistory.Parse(myUnifiedChatItem.Text, (MyFontEnum) "White", this.m_chatHistory.TextScale, white);
          this.m_chatHistory.AppendLine();
        }
      }
      this.m_factionList.SelectedItems.Clear();
      this.m_chatHistory.ScrollbarOffsetV = 1f;
    }

    private void ClearChat()
    {
      this.m_chatHistory.Clear();
      this.m_chatbox.Text = string.Empty;
    }

    private void RefreshLists()
    {
      this.RefreshPlayerList();
      this.RefreshFactionList();
    }

    private void RefreshPlayerList()
    {
      this.m_globalItem = new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.TerminalTab_Chat_ChatHistory), MyTexts.GetString(MySpaceTexts.TerminalTab_Chat_ChatHistory));
      if (this.AllowPlayerDrivenChat)
        this.m_playerList.Add(this.m_globalItem);
      this.m_tempStringBuilder.Clear();
      this.m_tempStringBuilder.Append((object) MyTexts.Get(MySpaceTexts.TerminalTab_Chat_GlobalChat));
      this.m_tempStringBuilder.Clear();
      this.m_tempStringBuilder.Append("-");
      this.m_tempStringBuilder.Append((object) MyTexts.Get(MySpaceTexts.ChatBotName));
      this.m_tempStringBuilder.Append("-");
      this.m_chatBotItem = new MyGuiControlListbox.Item(this.m_tempStringBuilder, this.m_tempStringBuilder.ToString());
      this.m_playerList.Add(this.m_chatBotItem);
      if (this.AllowPlayerDrivenChat)
      {
        foreach (MyPlayer.PlayerId allPlayer in (IEnumerable<MyPlayer.PlayerId>) MySession.Static.Players.GetAllPlayers())
        {
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId, allPlayer.SerialId));
          if (identity != null && identity.IdentityId != MySession.Static.LocalPlayerId && allPlayer.SerialId == 0)
          {
            this.m_tempStringBuilder.Clear();
            this.m_tempStringBuilder.Append(identity.DisplayName);
            StringBuilder tempStringBuilder = this.m_tempStringBuilder;
            object obj = (object) identity;
            string toolTip = this.m_tempStringBuilder.ToString();
            object userData = obj;
            this.m_playerList.Add(new MyGuiControlListbox.Item(tempStringBuilder, toolTip, userData: userData));
          }
        }
      }
      else
        this.m_playerList.Add(this.m_globalItem);
    }

    private void RefreshFactionList()
    {
      if (!this.AllowPlayerDrivenChat)
        return;
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
      if (playerFaction != null)
      {
        this.m_tempStringBuilder.Clear();
        this.m_tempStringBuilder.Append(MyStatControlText.SubstituteTexts(playerFaction.Name));
        StringBuilder tempStringBuilder = this.m_tempStringBuilder;
        object obj = (object) playerFaction;
        string toolTip = this.m_tempStringBuilder.ToString();
        object userData = obj;
        this.m_factionList.Add(new MyGuiControlListbox.Item(tempStringBuilder, toolTip, userData: userData));
        this.m_factionList.SetToolTip(string.Empty);
      }
      else
      {
        this.m_factionList.SelectedItems.Clear();
        this.m_factionList.Items.Clear();
        this.m_factionList.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Chat_NoFaction));
      }
    }

    public void Update()
    {
      if (this.m_closed)
        return;
      this.UpdateLists();
    }

    private void UpdatePlayerList()
    {
      MyGuiControlListbox playerList = this.m_playerList;
      long num1 = -1;
      bool flag1 = false;
      bool flag2 = false;
      if (this.m_playerList.SelectedItems != null && this.m_playerList.SelectedItems.Count > 0)
      {
        if (this.m_playerList.SelectedItems[0] == this.m_globalItem)
          flag1 = true;
        else if (this.m_playerList.SelectedItems[0] == this.m_chatBotItem)
          flag2 = true;
        else if (this.m_playerList.SelectedItems[0].UserData is MyIdentity userData)
          num1 = userData.IdentityId;
      }
      int num2 = this.m_playerList.FirstVisibleRow;
      string str = this.m_playerList.FocusedItem != null ? this.m_playerList.FocusedItem.Text.ToString() : string.Empty;
      this.m_playerList.SelectedItems.Clear();
      this.m_playerList.Items.Clear();
      this.RefreshPlayerList();
      if (!string.IsNullOrEmpty(str))
      {
        foreach (MyGuiControlListbox.Item obj in this.m_playerList.Items)
        {
          if (obj.Text.ToString() == str)
          {
            this.m_playerList.FocusedItem = obj;
            break;
          }
        }
      }
      if (num1 != -1L)
      {
        bool flag3 = false;
        foreach (MyGuiControlListbox.Item obj in this.m_playerList.Items)
        {
          if (obj.UserData != null && ((MyIdentity) obj.UserData).IdentityId == num1)
          {
            this.m_playerList.SelectedItems.Clear();
            this.m_playerList.SelectedItems.Add(obj);
            flag3 = true;
            break;
          }
        }
        if (!flag3)
          this.ClearChat();
      }
      else if (flag1)
      {
        this.m_playerList.SelectedItems.Clear();
        this.m_playerList.SelectedItems.Add(this.m_globalItem);
      }
      else if (flag2)
      {
        this.m_playerList.SelectedItems.Clear();
        this.m_playerList.SelectedItems.Add(this.m_chatBotItem);
      }
      if (num2 >= this.m_playerList.Items.Count)
        num2 = this.m_playerList.Items.Count - 1;
      this.m_playerList.FirstVisibleRow = num2;
    }

    private void UpdateLists()
    {
      this.UpdateFactionList(false);
      if (this.m_frameCount > 100)
      {
        this.m_frameCount = 0;
        this.UpdatePlayerList();
      }
      ++this.m_frameCount;
    }

    private void UpdateFactionList(bool forceRefresh)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      if (factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId) == null)
      {
        if (this.m_factionList.Items.Count == 0)
          return;
        this.RefreshFactionList();
      }
      else
      {
        if (!forceRefresh && this.m_factionList.Items.Count == factions.Count<KeyValuePair<long, MyFaction>>())
          return;
        long num1 = -1;
        if (this.m_factionList.SelectedItems.Count > 0)
          num1 = ((MyFaction) this.m_factionList.SelectedItems[0].UserData).FactionId;
        int num2 = this.m_factionList.FirstVisibleRow;
        this.m_factionList.SelectedItems.Clear();
        this.m_factionList.Items.Clear();
        this.RefreshFactionList();
        if (num1 != -1L)
        {
          bool flag = false;
          foreach (MyGuiControlListbox.Item obj in this.m_factionList.Items)
          {
            if (((MyFaction) obj.UserData).FactionId == num1)
            {
              this.m_factionList.SelectedItems.Clear();
              this.m_factionList.SelectedItems.Add(obj);
              flag = true;
              break;
            }
          }
          if (!flag)
            this.ClearChat();
        }
        if (num2 >= this.m_factionList.Items.Count)
          num2 = this.m_factionList.Items.Count - 1;
        this.m_factionList.FirstVisibleRow = num2;
      }
    }

    public void Close()
    {
      this.m_closed = false;
      this.m_playerList.ItemsSelected -= new Action<MyGuiControlListbox>(this.m_playerList_ItemsSelected);
      this.m_factionList.ItemsSelected -= new Action<MyGuiControlListbox>(this.m_factionList_ItemsSelected);
      this.m_sendButton.ButtonClicked -= new Action<MyGuiControlButton>(this.m_sendButton_ButtonClicked);
      this.m_chatbox.TextChanged -= new Action<MyGuiControlTextbox>(this.m_chatbox_TextChanged);
      this.m_chatbox.EnterPressed -= new Action<MyGuiControlTextbox>(this.m_chatbox_EnterPressed);
      if (MyMultiplayer.Static != null)
        MyMultiplayer.Static.ChatMessageReceived -= new Action<ulong, string, ChatChannel, long, string>(this.Multiplayer_ChatMessageReceived);
      if (MySession.Static.LocalCharacter != null)
      {
        MySession.Static.ChatSystem.PlayerMessageReceived -= new Action<long>(this.MyChatSystem_PlayerMessageReceived);
        MySession.Static.ChatSystem.FactionMessageReceived -= new Action<long>(this.MyChatSystem_FactionMessageReceived);
      }
      MySession.Static.Players.PlayersChanged -= new Action<bool, MyPlayer.PlayerId>(this.Players_PlayersChanged);
    }
  }
}
