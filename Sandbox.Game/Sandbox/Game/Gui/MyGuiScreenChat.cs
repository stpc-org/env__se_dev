// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenChat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenChat : MyGuiScreenBase
  {
    private readonly MyGuiControlTextbox m_chatTextbox;
    private readonly MyGuiControlLabel m_channelInfo;
    private readonly MyGuiControlLabel m_chatUnavailable;
    public static MyGuiScreenChat Static = (MyGuiScreenChat) null;
    private const int MESSAGE_HISTORY_SIZE = 20;
    private static StringBuilder[] m_messageHistory = new StringBuilder[20];
    private static int m_messageHistoryPushTo = 0;
    private static int m_messageHistoryShown = 0;
    private MyGuiScreenChat.MyNameFillState m_currentNameFillState = MyGuiScreenChat.MyNameFillState.Inactive;
    private string[] NAMEFILL_BASES = new string[2]
    {
      "/w \"",
      "/w "
    };
    private string m_namefillPrefix_completeBefore = string.Empty;
    private string m_namefillPrefix_completeNew = string.Empty;
    private string m_namefillPrefix_name = string.Empty;
    private string m_namefillPrefix_command = string.Empty;
    private int m_currentNamefillIndex = int.MaxValue;
    private List<MyPlayer> m_currentPlayerList;

    public MyGuiControlTextbox ChatTextbox => this.m_chatTextbox;

    static MyGuiScreenChat()
    {
      for (int index = 0; index < 20; ++index)
        MyGuiScreenChat.m_messageHistory[index] = new StringBuilder();
    }

    public MyGuiScreenChat(Vector2 position)
      : base(new Vector2?(position), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenChat.ctor START");
      this.EnabledBackgroundFade = false;
      this.m_isTopMostScreen = true;
      this.CanHideOthers = false;
      this.CanBeHidden = false;
      this.DrawMouseCursor = false;
      this.m_closeOnEsc = true;
      this.m_chatTextbox = new MyGuiControlTextbox(new Vector2?(Vector2.Zero), maxLength: MyGameService.GetChatMaxMessageSize(), enableJoystickTextPaste: true);
      this.m_chatTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_chatTextbox.Size = new Vector2(0.27f, 0.05f);
      this.m_chatTextbox.TextScale = 0.8f;
      this.m_chatTextbox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
      this.m_chatTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.OnInputFieldActivated);
      ChatChannel currentChannel = MySession.Static.ChatSystem.CurrentChannel;
      string empty = string.Empty;
      Color channelColor = MyChatSystem.GetChannelColor(currentChannel);
      string text;
      switch (currentChannel)
      {
        case ChatChannel.Global:
          text = MyTexts.GetString(MyCommonTexts.Chat_NameModifier_Global);
          break;
        case ChatChannel.Faction:
          string str1 = "faction";
          IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(MySession.Static.ChatSystem.CurrentTarget);
          if (factionById != null)
            str1 = factionById.Tag;
          text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ToBracketed), (object) str1);
          break;
        case ChatChannel.Private:
          string str2 = "player";
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.ChatSystem.CurrentTarget);
          if (identity != null)
            str2 = identity.DisplayName.Length > 9 ? identity.DisplayName.Substring(0, 9) : identity.DisplayName;
          text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ToBracketed), (object) str2);
          break;
        default:
          text = MyTexts.GetString(MyCommonTexts.Chat_NameModifier_ReportThis);
          break;
      }
      this.m_channelInfo = new MyGuiControlLabel(new Vector2?(new Vector2(-0.016f, -0.042f)), text: text);
      this.m_channelInfo.ColorMask = (Vector4) channelColor;
      this.m_chatTextbox.Size = new Vector2(0.3215f - this.m_channelInfo.Size.X, 0.032f);
      this.m_chatTextbox.Position = new Vector2(-0.01f, -0.06f) + new Vector2(this.m_channelInfo.Size.X, 0.0f);
      MyMultiplayerBase myMultiplayerBase = Sandbox.Engine.Multiplayer.MyMultiplayer.Static;
      if ((myMultiplayerBase != null ? (!myMultiplayerBase.IsTextChatAvailable ? 1 : 0) : 0) != 0)
      {
        this.m_chatUnavailable = new MyGuiControlLabel(new Vector2?(new Vector2(-0.016f, -0.042f - this.m_chatTextbox.Size.Y)));
        this.m_chatUnavailable.ColorMask = (Vector4) Color.Red;
        this.m_chatUnavailable.TextEnum = MyCommonTexts.ChatRestricted;
        this.Controls.Add((MyGuiControlBase) this.m_chatUnavailable);
      }
      this.Controls.Add((MyGuiControlBase) this.m_chatTextbox);
      this.Controls.Add((MyGuiControlBase) this.m_channelInfo);
      MySandboxGame.Log.WriteLine("MyGuiScreenChat.ctor END");
      MyHud.Chat.ChatOpened();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Down))
      {
        this.HardResetFill();
        this.HistoryUp();
      }
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Up))
      {
        this.HardResetFill();
        this.HistoryDown();
      }
      else if (MyInput.Static.IsKeyPress(MyKeys.PageUp) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP, MyControlStateType.NEW_PRESSED_REPEATING))
      {
        this.HardResetFill();
        MyHud.Chat.ChatControl.ScrollUp();
      }
      else if (MyInput.Static.IsKeyPress(MyKeys.PageDown) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN, MyControlStateType.NEW_PRESSED_REPEATING))
      {
        this.HardResetFill();
        MyHud.Chat.ChatControl.ScrollDown();
      }
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) && this.m_currentNameFillState == MyGuiScreenChat.MyNameFillState.Active)
        this.SoftResetFill();
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
      {
        switch (this.m_currentNameFillState)
        {
          case MyGuiScreenChat.MyNameFillState.Inactive:
            if (!this.InitiateNamefill())
              break;
            this.CycleNamefill();
            break;
          case MyGuiScreenChat.MyNameFillState.Active:
            if (this.m_chatTextbox.Text.Equals(this.m_namefillPrefix_completeNew))
            {
              this.CycleNamefill();
              break;
            }
            this.SoftResetFill();
            if (!this.InitiateNamefill())
              break;
            this.CycleNamefill();
            break;
        }
      }
      else
        base.HandleInput(receivedFocusInThisUpdate);
    }

    public void OnInputFieldActivated(MyGuiControlTextbox textBox)
    {
      string text = textBox.Text;
      MyGuiScreenChat.PushHistory(text);
      if (MySession.Static.ChatSystem.CommandSystem.CanHandle(text))
      {
        MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer.DisplayName, text);
        MySession.Static.ChatSystem.CommandSystem.Handle(text);
      }
      else if (!string.IsNullOrWhiteSpace(text))
      {
        if (MySession.Static.ChatBot != null && MySession.Static.ChatBot.FilterMessage(text, new Action<string>(this.OnChatBotResponse)))
        {
          MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, ChatChannel.ChatBot, MySession.Static.LocalPlayerId, -1L, new DateTime?(DateTime.UtcNow));
          MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer == null ? "Player" : MySession.Static.LocalHumanPlayer.DisplayName, text);
        }
        else
        {
          bool sendToOthers = true;
          MyAPIUtilities.Static.EnterMessage(MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.Id.SteamId : 0UL, text, ref sendToOthers);
          if (sendToOthers)
            MyGuiScreenChat.SendChatMessage(text);
        }
      }
      textBox.FocusEnded();
      this.CloseScreenNow();
    }

    public static void SendChatMessage(string message)
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(message, MySession.Static.ChatSystem.CurrentChannel, MySession.Static.ChatSystem.CurrentTarget);
      else if (MyGameService.IsActive)
        MyHud.Chat.ShowMessageColoredSP(message, MySession.Static.ChatSystem.CurrentChannel, MySession.Static.ChatSystem.CurrentTarget);
      else
        MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer.DisplayName, message);
    }

    private bool InitiateNamefill()
    {
      string text = this.m_chatTextbox.Text;
      string command = string.Empty;
      string nameStump = string.Empty;
      if (!this.TestNamefillPrefixse(text, out command, out nameStump))
        return false;
      this.m_currentNameFillState = MyGuiScreenChat.MyNameFillState.Active;
      this.m_namefillPrefix_completeBefore = this.m_namefillPrefix_completeNew = text;
      this.m_namefillPrefix_command = command;
      this.m_namefillPrefix_name = nameStump;
      this.m_currentPlayerList = MySession.Static.Players.GetPlayersStartingNameWith(nameStump);
      this.m_currentNamefillIndex = this.m_currentPlayerList.Count - 1;
      return true;
    }

    private void CycleNamefill()
    {
      if (this.m_currentPlayerList.Count == 0)
        return;
      ++this.m_currentNamefillIndex;
      if (this.m_currentNamefillIndex >= this.m_currentPlayerList.Count)
        this.m_currentNamefillIndex = 0;
      this.m_namefillPrefix_completeNew = this.m_namefillPrefix_command + this.m_currentPlayerList[this.m_currentNamefillIndex].DisplayName;
      this.m_chatTextbox.Text = this.m_namefillPrefix_completeNew;
    }

    private bool TestNamefillPrefixse(string complete, out string command, out string nameStump)
    {
      command = string.Empty;
      nameStump = string.Empty;
      foreach (string str in this.NAMEFILL_BASES)
      {
        if (complete.Length >= str.Length && str.Equals(complete.Substring(0, str.Length)))
        {
          command = str;
          nameStump = complete.Substring(str.Length, complete.Length - str.Length);
          return true;
        }
      }
      return false;
    }

    private void HardResetFill()
    {
      if (this.m_currentNameFillState == MyGuiScreenChat.MyNameFillState.Active)
        this.m_chatTextbox.Text = this.m_namefillPrefix_completeBefore;
      this.SoftResetFill();
    }

    private void SoftResetFill()
    {
      if (this.m_currentNameFillState != MyGuiScreenChat.MyNameFillState.Active)
        return;
      this.m_currentNameFillState = MyGuiScreenChat.MyNameFillState.Inactive;
      this.m_namefillPrefix_completeBefore = this.m_namefillPrefix_completeNew = this.m_namefillPrefix_command = this.m_namefillPrefix_name = string.Empty;
      this.m_currentNamefillIndex = int.MaxValue;
      this.m_currentPlayerList = (List<MyPlayer>) null;
    }

    private void OnChatBotResponse(string text)
    {
      if (MySession.Static == null)
        return;
      MyUnifiedChatItem chatbotMessage = MyUnifiedChatItem.CreateChatbotMessage(text, DateTime.UtcNow, 0L, MySession.Static.LocalPlayerId, MyTexts.GetString(MySpaceTexts.ChatBotName));
      MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref chatbotMessage);
      MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), text);
    }

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      Vector2 hudPos = this.m_position;
      hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
      return true;
    }

    public override bool Draw() => base.Draw();

    private static void PushHistory(string message)
    {
      MyGuiScreenChat.m_messageHistory[MyGuiScreenChat.m_messageHistoryPushTo].Clear().Append(message);
      MyGuiScreenChat.m_messageHistoryPushTo = MyGuiScreenChat.HistoryIndexUp(MyGuiScreenChat.m_messageHistoryPushTo);
      MyGuiScreenChat.m_messageHistoryShown = MyGuiScreenChat.m_messageHistoryPushTo;
      MyGuiScreenChat.m_messageHistory[MyGuiScreenChat.m_messageHistoryPushTo].Clear();
    }

    private void HistoryDown()
    {
      int num = MyGuiScreenChat.HistoryIndexDown(MyGuiScreenChat.m_messageHistoryShown);
      if (num == MyGuiScreenChat.m_messageHistoryPushTo)
        return;
      MyGuiScreenChat.m_messageHistoryShown = num;
      this.m_chatTextbox.Text = MyGuiScreenChat.m_messageHistory[MyGuiScreenChat.m_messageHistoryShown].ToString() ?? "";
    }

    private void HistoryUp()
    {
      if (MyGuiScreenChat.m_messageHistoryShown == MyGuiScreenChat.m_messageHistoryPushTo)
        return;
      MyGuiScreenChat.m_messageHistoryShown = MyGuiScreenChat.HistoryIndexUp(MyGuiScreenChat.m_messageHistoryShown);
      this.m_chatTextbox.Text = MyGuiScreenChat.m_messageHistory[MyGuiScreenChat.m_messageHistoryShown].ToString() ?? "";
    }

    private static int HistoryIndexUp(int index)
    {
      ++index;
      return index >= 20 ? 0 : index;
    }

    private static int HistoryIndexDown(int index)
    {
      --index;
      return index < 0 ? 19 : index;
    }

    private void Process(string message)
    {
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenChat);

    public override void LoadContent()
    {
      base.LoadContent();
      MyGuiScreenChat.Static = this;
    }

    public override void UnloadContent()
    {
      if (this.m_chatTextbox != null)
        this.m_chatTextbox.FocusEnded();
      MyGuiScreenChat.Static = (MyGuiScreenChat) null;
      base.UnloadContent();
    }

    public override bool HideScreen()
    {
      this.UnloadContent();
      return base.HideScreen();
    }

    protected override void OnClosed()
    {
      MyHud.Chat.ChatClosed();
      base.OnClosed();
    }

    private enum MyNameFillState
    {
      Disabled,
      Inactive,
      Active,
    }
  }
}
