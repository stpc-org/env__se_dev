// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenPlayers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.Trading;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyGuiScreenPlayers : MyGuiScreenBase
  {
    protected const string OWNER_MARKER = "*****";
    protected const string SERVICE_XBL = "Xbox Live";
    protected const string SERVICE_Steam = "Steam";
    protected int GamePlatformColumn;
    protected int PlayerNameColumn;
    protected int PlayerFactionNameColumn;
    protected int GameAdminColumn;
    protected int GamePingColumn;
    protected int PlayerMutedColumn;
    protected int PlayerTableColumnsCount;
    private int m_warfareUpdate_frameCount;
    private int m_warfareUpdate_frameCount_current;
    private bool m_getPingAndRefresh;
    protected MyGuiControlButton m_profileButton;
    protected MyGuiControlButton m_promoteButton;
    protected MyGuiControlButton m_demoteButton;
    protected MyGuiControlButton m_kickButton;
    protected MyGuiControlButton m_banButton;
    protected MyGuiControlButton m_inviteButton;
    protected MyGuiControlButton m_tradeButton;
    protected bool m_isScenarioRunning;
    protected MyGuiControlLabel m_maxPlayersValueLabel;
    protected MyGuiControlTable m_playersTable;
    protected MyGuiControlCombobox m_lobbyTypeCombo;
    protected MyGuiControlSlider m_maxPlayersSlider;
    protected Dictionary<ulong, short> pings;
    private MyGuiControlLabel m_warfare_timeRemainting_time;
    private MyGuiControlLabel m_warfare_timeRemainting_label;
    protected ulong m_lastSelected;
    private MyGuiControlLabel m_caption;
    private readonly MyGuiControlButton.StyleDefinition m_buttonSizeStyleMuted;
    private readonly MyGuiControlButton.StyleDefinition m_buttonSizeStyleUnMuted;
    private bool m_waitingTradeResponse;
    private int m_maxPlayers;
    private int m_lastMuteIndicatorsUpdate;

    public MyGuiScreenPlayers()
    {
      Vector2? position = new Vector2?();
      Vector2? nullable = new Vector2?(new Vector2(0.837f, 0.813f));
      Vector4? backgroundColor = new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR);
      Vector2? size = nullable;
      string texture = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture;
      double uiBkOpacity = (double) MySandboxGame.Config.UIBkOpacity;
      double uiOpacity = (double) MySandboxGame.Config.UIOpacity;
      int? gamepadSlot = new int?();
      // ISSUE: explicit constructor call
      base.\u002Ector(position, backgroundColor, size, backgroundTexture: texture, backgroundTransition: ((float) uiBkOpacity), guiTransition: ((float) uiOpacity), gamepadSlot: gamepadSlot);
      this.EnabledBackgroundFade = true;
      MyMultiplayer.Static.ClientJoined += new Action<ulong, string>(this.Multiplayer_PlayerJoined);
      MyMultiplayer.Static.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.Multiplayer_PlayerLeft);
      MySession.Static.Factions.FactionCreated += new Action<long>(this.OnFactionCreated);
      MySession.Static.Factions.FactionEdited += new Action<long>(this.OnFactionEdited);
      MySession.Static.Factions.FactionStateChanged += new Action<MyFactionStateChange, long, long, long, long>(this.OnFactionStateChanged);
      MySession.Static.OnUserPromoteLevelChanged += new Action<ulong, MyPromoteLevel>(this.OnUserPromoteLevelChanged);
      if (MyMultiplayer.Static is MyMultiplayerLobby multiplayerLobby)
        multiplayerLobby.OnLobbyDataUpdated += new MyLobbyDataUpdated(this.Matchmaking_LobbyDataUpdate);
      if (MyMultiplayer.Static is MyMultiplayerLobbyClient multiplayerLobbyClient)
        multiplayerLobbyClient.OnLobbyDataUpdated += new MyLobbyDataUpdated(this.Matchmaking_LobbyDataUpdate);
      this.RecreateControls(true);
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenPlayers.SyncRemainingTimeWithClient)), MyEventContext.Current.Sender);
      MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged += new Action<ulong, bool>(this.OnPlayerMutedStateChanged);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenPlayers);

    protected override void OnClosed()
    {
      if (this.m_waitingTradeResponse)
      {
        MyTradingManager.Static.TradeCancel_Client();
        this.m_waitingTradeResponse = false;
      }
      base.OnClosed();
      if (MyMultiplayer.Static != null)
      {
        MyMultiplayer.Static.ClientJoined -= new Action<ulong, string>(this.Multiplayer_PlayerJoined);
        MyMultiplayer.Static.ClientLeft -= new Action<ulong, MyChatMemberStateChangeEnum>(this.Multiplayer_PlayerLeft);
      }
      if (MySession.Static != null)
      {
        MySession.Static.Factions.FactionCreated -= new Action<long>(this.OnFactionCreated);
        MySession.Static.Factions.FactionEdited -= new Action<long>(this.OnFactionEdited);
        MySession.Static.Factions.FactionStateChanged -= new Action<MyFactionStateChange, long, long, long, long>(this.OnFactionStateChanged);
        MySession.Static.OnUserPromoteLevelChanged -= new Action<ulong, MyPromoteLevel>(this.OnUserPromoteLevelChanged);
      }
      if (MyMultiplayer.Static is MyMultiplayerLobby multiplayerLobby)
        multiplayerLobby.OnLobbyDataUpdated -= new MyLobbyDataUpdated(this.Matchmaking_LobbyDataUpdate);
      if (MyMultiplayer.Static is MyMultiplayerLobbyClient multiplayerLobbyClient)
        multiplayerLobbyClient.OnLobbyDataUpdated -= new MyLobbyDataUpdated(this.Matchmaking_LobbyDataUpdate);
      MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged -= new Action<ulong, bool>(this.OnPlayerMutedStateChanged);
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.CloseButtonEnabled = true;
      Vector2 vector2_1 = -0.5f * this.Size.Value + this.Size.Value / MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.SizeGui * MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.PaddingSizeGui * 1.1f;
      this.m_caption = this.AddCaption(MyCommonTexts.ScreenCaptionPlayers, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(-0.364f, -0.331f), 0.728f);
      Vector2 start = new Vector2(-0.364f, 0.358f);
      controlSeparatorList.AddHorizontal(start, 0.728f);
      controlSeparatorList.AddHorizontal(new Vector2(-0.36f, 0.05f), 0.17f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      Vector2 vector2_2 = new Vector2(0.0f, 57f / 1000f);
      Vector2 vector2_3 = new Vector2(-0.361f, -0.304f);
      this.m_profileButton = new MyGuiControlButton(new Vector2?(vector2_3), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Profile));
      this.m_profileButton.ButtonClicked += new Action<MyGuiControlButton>(this.profileButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_profileButton);
      Vector2 vector2_4 = vector2_3 + vector2_2;
      this.m_promoteButton = new MyGuiControlButton(new Vector2?(vector2_4), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Promote));
      this.m_promoteButton.ButtonClicked += new Action<MyGuiControlButton>(this.promoteButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_promoteButton);
      Vector2 vector2_5 = vector2_4 + vector2_2;
      this.m_demoteButton = new MyGuiControlButton(new Vector2?(vector2_5), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Demote));
      this.m_demoteButton.ButtonClicked += new Action<MyGuiControlButton>(this.demoteButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_demoteButton);
      Vector2 vector2_6 = vector2_5 + vector2_2;
      this.m_kickButton = new MyGuiControlButton(new Vector2?(vector2_6), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Kick));
      this.m_kickButton.ButtonClicked += new Action<MyGuiControlButton>(this.kickButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_kickButton);
      Vector2 vector2_7 = vector2_6 + vector2_2;
      this.m_banButton = new MyGuiControlButton(new Vector2?(vector2_7), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Ban));
      this.m_banButton.ButtonClicked += new Action<MyGuiControlButton>(this.banButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_banButton);
      Vector2 vector2_8 = vector2_7 + vector2_2;
      this.m_tradeButton = new MyGuiControlButton(new Vector2?(vector2_8), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.PlayersScreen_TradeBtn));
      this.m_tradeButton.SetTooltip(MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_TTP));
      this.m_tradeButton.ButtonClicked += new Action<MyGuiControlButton>(this.tradeButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_tradeButton);
      int num1 = MyMultiplayer.Static == null ? 0 : (MyMultiplayer.Static.IsLobby ? 1 : 0);
      Vector2 vector2_9 = vector2_8 + new Vector2(-1f / 500f, this.m_tradeButton.Size.Y + 0.03f);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(vector2_9), text: MyTexts.GetString(MySpaceTexts.PlayersScreen_LobbyType), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      if (num1 != 0)
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      Vector2 vector2_10 = vector2_9 + new Vector2(0.0f, 0.033f);
      this.m_lobbyTypeCombo = new MyGuiControlCombobox(new Vector2?(vector2_10), openAreaItemsCount: 3);
      this.m_lobbyTypeCombo.Size = new Vector2(0.175f, 0.04f);
      this.m_lobbyTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_lobbyTypeCombo.AddItem(0L, MyCommonTexts.ScreenPlayersLobby_Private);
      this.m_lobbyTypeCombo.AddItem(1L, MyCommonTexts.ScreenPlayersLobby_Friends);
      this.m_lobbyTypeCombo.AddItem(2L, MyCommonTexts.ScreenPlayersLobby_Public);
      this.m_lobbyTypeCombo.SelectItemByKey((long) MyMultiplayer.Static.GetLobbyType());
      if (num1 != 0)
        this.Controls.Add((MyGuiControlBase) this.m_lobbyTypeCombo);
      Vector2 vector2_11 = vector2_10 + new Vector2(0.0f, 0.05f);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(vector2_11 + new Vector2(1f / 1000f, 0.0f)), text: (MyTexts.GetString(MyCommonTexts.MaxPlayers) + ":"), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      if (num1 != 0)
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_maxPlayersValueLabel = new MyGuiControlLabel(new Vector2?(vector2_11 + new Vector2(0.169f, 0.0f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      if (num1 != 0)
        this.Controls.Add((MyGuiControlBase) this.m_maxPlayersValueLabel);
      Vector2 vector2_12 = vector2_11 + new Vector2(0.0f, 0.03f);
      this.m_maxPlayers = Sync.IsServer ? MyMultiplayerLobby.MAX_PLAYERS : 16;
      this.m_maxPlayersSlider = new MyGuiControlSlider(new Vector2?(vector2_12), 2f, (float) Math.Max(this.m_maxPlayers, 3), 0.177f, new float?(Sync.IsServer ? (float) MySession.Static.MaxPlayers : (float) MyMultiplayer.Static.MemberLimit), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
      this.m_isScenarioRunning = MyCampaignManager.Static.IsScenarioRunning;
      this.m_maxPlayersSlider.Enabled = !this.m_isScenarioRunning;
      this.m_maxPlayersValueLabel.Text = this.m_maxPlayersSlider.Value.ToString();
      this.m_maxPlayersSlider.ValueChanged = new Action<MyGuiControlSlider>(this.MaxPlayersSlider_Changed);
      if (num1 != 0)
        this.Controls.Add((MyGuiControlBase) this.m_maxPlayersSlider);
      this.m_inviteButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.361f, 0.285f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenPlayers_Invite));
      this.m_inviteButton.ButtonClicked += new Action<MyGuiControlButton>(this.inviteButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_inviteButton);
      Vector2 vector2_13 = new Vector2(0.364f, -0.307f);
      Vector2 vector2_14 = new Vector2(0.54f, 0.813f);
      int num2 = 18;
      float num3 = 0.0f;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component.IsEnabled)
      {
        Vector2 vector2_15 = this.GetPositionAbsolute() + vector2_13 + new Vector2(-vector2_14.X, 0.0f);
        this.m_warfare_timeRemainting_label = new MyGuiControlLabel(new Vector2?(vector2_13 - new Vector2(vector2_14.X, 0.0f)));
        this.m_warfare_timeRemainting_label.Text = MyTexts.GetString(MySpaceTexts.WarfareCounter_TimeRemaining).ToString() + ": ";
        this.Controls.Add((MyGuiControlBase) this.m_warfare_timeRemainting_label);
        TimeSpan timeSpan = TimeSpan.FromMinutes((double) component.RemainingMinutes);
        this.m_warfare_timeRemainting_time = new MyGuiControlLabel(new Vector2?(vector2_13 - new Vector2(vector2_14.X, 0.0f) + new Vector2(this.m_warfare_timeRemainting_label.Size.X, 0.0f)));
        this.m_warfare_timeRemainting_time.Text = timeSpan.ToString(timeSpan.TotalHours >= 1.0 ? "hh\\:mm\\:ss" : "mm\\:ss");
        this.Controls.Add((MyGuiControlBase) this.m_warfare_timeRemainting_time);
        float height = 0.09f;
        float width = (float) ((double) vector2_14.X / 3.0 - 2.0 * (double) num3);
        int num4 = 0;
        foreach (MyFaction allFaction in MySession.Static.Factions.GetAllFactions())
        {
          if ((allFaction.Name.StartsWith("Red") || allFaction.Name.StartsWith("Green") || allFaction.Name.StartsWith("Blue")) && allFaction.Name.EndsWith("Faction"))
          {
            this.Controls.Add((MyGuiControlBase) new MyGuiScreenPlayersWarfareTeamScoreTable(vector2_15 + new Vector2((float) num4 * (width + num3), this.m_warfare_timeRemainting_label.Size.Y + num3), width, height, allFaction.Name, allFaction.FactionIcon.Value.String, MyTexts.GetString(MySpaceTexts.WarfareCounter_EscapePod), allFaction.FactionId, isLocalPlayersFaction: allFaction.IsMember(MySession.Static.LocalHumanPlayer.Identity.IdentityId)));
            ++num4;
          }
        }
        vector2_13.Y += (float) ((double) this.m_warfare_timeRemainting_label.Size.Y + (double) height + (double) num3 * 2.0);
        num2 -= 3;
      }
      MyGuiControlTable myGuiControlTable = new MyGuiControlTable();
      myGuiControlTable.Position = vector2_13;
      myGuiControlTable.Size = vector2_14;
      myGuiControlTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      myGuiControlTable.ColumnsCount = this.PlayerTableColumnsCount;
      this.m_playersTable = myGuiControlTable;
      this.m_playersTable.GamepadHelpTextId = MySpaceTexts.PlayersScreen_Help_PlayersList;
      this.m_playersTable.VisibleRowsCount = num2;
      float num5 = 0.3f;
      float num6 = 0.12f;
      float num7 = 0.12f;
      float num8 = 0.13f;
      float num9 = 0.1f;
      this.m_playersTable.SetCustomColumnWidths(new float[6]
      {
        num9,
        num5,
        1f - num5 - num6 - num8 - num7 - num9,
        num6,
        num7,
        num8
      });
      this.m_playersTable.SetColumnComparison(this.PlayerNameColumn, (Comparison<MyGuiControlTable.Cell>) ((a, b) => a.Text.CompareToIgnoreCase(b.Text)));
      this.m_playersTable.SetColumnName(this.PlayerNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_PlayerName));
      this.m_playersTable.SetColumnComparison(this.PlayerFactionNameColumn, (Comparison<MyGuiControlTable.Cell>) ((a, b) => a.Text.CompareToIgnoreCase(b.Text)));
      this.m_playersTable.SetColumnName(this.PlayerFactionNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_FactionName));
      this.m_playersTable.SetColumnName(this.PlayerMutedColumn, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenPlayers_Muted)));
      this.m_playersTable.SetColumnComparison(this.GameAdminColumn, new Comparison<MyGuiControlTable.Cell>(this.GameAdminCompare));
      this.m_playersTable.SetColumnName(this.GameAdminColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Rank));
      this.m_playersTable.SetColumnComparison(this.GamePingColumn, new Comparison<MyGuiControlTable.Cell>(this.GamePingCompare));
      this.m_playersTable.SetColumnName(this.GamePingColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Ping));
      this.m_playersTable.SetColumnComparison(this.GamePlatformColumn, new Comparison<MyGuiControlTable.Cell>(this.PlatformCompare));
      this.m_playersTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.playersTable_ItemSelected);
      this.m_playersTable.UpdateTableSortHelpText();
      this.Controls.Add((MyGuiControlBase) this.m_playersTable);
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Id.SerialId == 0)
        {
          for (int index = 0; index < this.m_playersTable.RowsCount; ++index)
          {
            MyGuiControlTable.Row row = this.m_playersTable.GetRow(index);
            if (row.UserData is ulong)
            {
              long userData = (long) (ulong) row.UserData;
              long steamId = (long) onlinePlayer.Id.SteamId;
            }
          }
          this.AddPlayer(onlinePlayer.Id.SteamId);
        }
      }
      this.m_lobbyTypeCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.lobbyTypeCombo_OnSelect);
      if (this.m_lastSelected != 0UL)
      {
        MyGuiControlTable.Row row = this.m_playersTable.Find((Predicate<MyGuiControlTable.Row>) (r => (long) (ulong) r.UserData == (long) this.m_lastSelected));
        if (row != null)
          this.m_playersTable.SelectedRow = row;
      }
      this.UpdateButtonsEnabledState();
      this.UpdateCaption();
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, start.Y + minSizeGui.Y / 2f)));
      myGuiControlLabel3.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.PlayersScreen_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_playersTable;
    }

    private void profileButton_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_playersTable.SelectedRow;
      if (selectedRow == null)
        return;
      MyGameService.OpenOverlayUser((ulong) selectedRow.UserData);
    }

    private void OnPlayerMutedStateChanged(ulong playerId, bool isMuted) => this.RefreshMuteIcons();

    private void tradeButton_ButtonClicked(MyGuiControlButton obj)
    {
      ulong requestedId = this.m_playersTable.SelectedRow != null ? (ulong) this.m_playersTable.SelectedRow.UserData : 0UL;
      if (requestedId == 0UL || (long) requestedId == (long) MyGameService.UserId)
        return;
      this.m_waitingTradeResponse = true;
      MyTradingManager.Static.TradeRequest_Client(MyGameService.UserId, requestedId, new Action<MyTradeResponseReason>(this.OnAnswerRecieved));
      if (!this.m_waitingTradeResponse)
        return;
      this.m_tradeButton.Enabled = false;
      this.m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_Waiting);
    }

    private void OnAnswerRecieved(MyTradeResponseReason reason)
    {
      this.m_waitingTradeResponse = false;
      this.UpdateTradeButton();
      this.m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn);
    }

    private int GameAdminCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      ulong userData1 = (ulong) a.Row.UserData;
      ulong userData2 = (ulong) b.Row.UserData;
      return ((int) MySession.Static.GetUserPromoteLevel(userData1)).CompareTo((int) MySession.Static.GetUserPromoteLevel(userData2));
    }

    private int GamePingCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      int result1;
      if (!int.TryParse(a.Text.ToString(), out result1))
        result1 = -1;
      int result2;
      if (!int.TryParse(b.Text.ToString(), out result2))
        result2 = -1;
      return result1.CompareTo(result2);
    }

    private int PlatformCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareTo(b.Text);

    protected void OnToggleMutePressed(MyGuiControlButton button)
    {
      ulong userData = (ulong) button.UserData;
      bool muted = button.CustomStyle == this.m_buttonSizeStyleUnMuted;
      string memberServiceName = MyMultiplayer.Static.GetMemberServiceName(userData);
      if ((!(MyGameService.Service.ServiceName == "Xbox Live") ? 0 : (memberServiceName == "Xbox Live" ? 1 : 0)) != 0)
      {
        this.profileButton_ButtonClicked((MyGuiControlButton) null);
      }
      else
      {
        MyVoiceChatSessionComponent.Static.SetPlayerMuted(userData, muted);
        this.RefreshMuteIcons();
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.F3))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
        this.CloseScreen();
      }
      if (this.FocusedControl == this.m_playersTable && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
      {
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
        this.tradeButton_ButtonClicked((MyGuiControlButton) null);
      }
      if (this.FocusedControl == this.m_playersTable && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
      {
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
        this.profileButton_ButtonClicked((MyGuiControlButton) null);
      }
      if (this.FocusedControl != this.m_playersTable || !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y) || !(this.m_playersTable.GetInnerControlsFromCurrentCell(this.PlayerMutedColumn) is MyGuiControlButton controlsFromCurrentCell))
        return;
      MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      controlsFromCurrentCell.PressButton();
    }

    protected void AddPlayer(ulong userId)
    {
      string memberName = MyMultiplayer.Static.GetMemberName(userId);
      if (string.IsNullOrEmpty(memberName))
        return;
      MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) userId);
      string memberServiceName = MyMultiplayer.Static.GetMemberServiceName(userId);
      MyGuiControlTable.Row row2 = row1;
      StringBuilder text1 = new StringBuilder();
      string toolTip = memberServiceName;
      MyGuiHighlightTexture? nullable = new MyGuiHighlightTexture?(new MyGuiHighlightTexture()
      {
        Normal = "Textures\\GUI\\Icons\\Services\\" + memberServiceName + ".png",
        Highlight = "Textures\\GUI\\Icons\\Services\\" + memberServiceName + ".png",
        SizePx = new Vector2(24f, 24f)
      });
      Color? textColor = new Color?(Color.White);
      MyGuiHighlightTexture? icon = nullable;
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell(text1, toolTip: toolTip, textColor: textColor, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      row2.AddCell(cell1);
      row1.AddCell(new MyGuiControlTable.Cell(new StringBuilder(memberName), (object) memberName));
      long identityId = Sync.Players.TryGetIdentityId(userId, 0);
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(identityId);
      StringBuilder text2 = new StringBuilder();
      if (playerFaction != null)
      {
        text2.Append(MyStatControlText.SubstituteTexts(playerFaction.Name));
        if (playerFaction.IsLeader(identityId))
          text2.Append(" (").Append((object) MyTexts.Get(MyCommonTexts.Leader)).Append(")");
      }
      row1.AddCell(new MyGuiControlTable.Cell(text2));
      StringBuilder text3 = new StringBuilder();
      MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(userId);
      for (int index = 0; (MyPromoteLevel) index < userPromoteLevel; ++index)
        text3.Append("*");
      row1.AddCell(new MyGuiControlTable.Cell(text3));
      if (this.pings.ContainsKey(userId))
        row1.AddCell(new MyGuiControlTable.Cell(new StringBuilder(this.pings[userId].ToString())));
      else
        row1.AddCell(new MyGuiControlTable.Cell(new StringBuilder("----")));
      MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(new StringBuilder(""));
      row1.AddCell(cell2);
      if ((long) userId != (long) Sync.MyId)
      {
        MyGuiControlButton guiControlButton = new MyGuiControlButton();
        guiControlButton.CustomStyle = this.m_buttonSizeStyleUnMuted;
        guiControlButton.Size = new Vector2(0.03f, 0.04f);
        guiControlButton.CueEnum = GuiSounds.None;
        guiControlButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnToggleMutePressed);
        guiControlButton.UserData = (object) userId;
        cell2.Control = (MyGuiControlBase) guiControlButton;
        this.m_playersTable.Controls.Add((MyGuiControlBase) guiControlButton);
        this.RefreshMuteIcons();
      }
      this.m_playersTable.Add(row1);
      this.UpdateCaption();
    }

    protected void RemovePlayer(ulong userId)
    {
      this.m_playersTable.Remove((Predicate<MyGuiControlTable.Row>) (row => (long) (ulong) row.UserData == (long) userId));
      this.UpdateButtonsEnabledState();
      if (MySession.Static == null)
        return;
      this.UpdateCaption();
    }

    private void UpdateCaption()
    {
      string str = string.Empty;
      if (MyMultiplayer.Static is MyMultiplayerClient multiplayerClient)
      {
        if (multiplayerClient.Server != null)
          str = multiplayerClient.Server.Name;
      }
      else if (MyMultiplayer.Static is MyMultiplayerLobbyClient multiplayerLobbyClient)
        str = multiplayerLobbyClient.HostName;
      if (string.IsNullOrEmpty(str))
        this.m_caption.Text = MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers).ToString() + " (" + (object) this.m_playersTable.RowsCount + " / " + (object) MySession.Static.MaxPlayers + ")";
      else
        this.m_caption.Text = MyTexts.Get(MyCommonTexts.ScreenCaptionServerName).ToString() + str + "  -  " + (object) MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers) + " (" + (object) this.m_playersTable.RowsCount + " / " + (object) MySession.Static.MaxPlayers + ")";
    }

    protected void UpdateButtonsEnabledState()
    {
      if (MyMultiplayer.Static == null)
        return;
      bool flag1 = this.m_playersTable.SelectedRow != null;
      ulong userId = MyGameService.UserId;
      ulong owner = MyMultiplayer.Static.GetOwner();
      ulong target = flag1 ? (ulong) this.m_playersTable.SelectedRow.UserData : 0UL;
      bool flag2 = (long) userId == (long) target;
      bool flag3 = MySession.Static.IsUserAdmin(userId);
      bool flag4 = (long) userId == (long) owner;
      bool flag5 = flag1 && MySession.Static.CanPromoteUser(Sync.MyId, target);
      bool flag6 = flag1 && MySession.Static.CanDemoteUser(Sync.MyId, target);
      int selectedKey = (int) this.m_lobbyTypeCombo.GetSelectedKey();
      if (flag1 && !flag2)
      {
        this.m_promoteButton.Enabled = flag5;
        this.m_demoteButton.Enabled = flag6;
        this.m_kickButton.Enabled = flag5 & flag3;
        this.m_banButton.Enabled = flag5 & flag3;
      }
      else
      {
        this.m_promoteButton.Enabled = false;
        this.m_demoteButton.Enabled = false;
        this.m_kickButton.Enabled = false;
        this.m_banButton.Enabled = false;
      }
      this.m_banButton.Enabled &= MyMultiplayer.Static is MyMultiplayerClient;
      this.m_inviteButton.Enabled = MyGameService.IsInviteSupported();
      this.m_lobbyTypeCombo.Enabled = flag4;
      this.m_maxPlayersSlider.Enabled = flag4 && this.m_maxPlayers > 2 && !this.m_isScenarioRunning;
      this.m_profileButton.Enabled = flag1;
      this.UpdateTradeButton();
    }

    private void UpdateTradeButton()
    {
      int num = this.m_playersTable.SelectedRow != null ? 1 : 0;
      ulong userId = MyGameService.UserId;
      ulong playerId2 = num != 0 ? (ulong) this.m_playersTable.SelectedRow.UserData : 0UL;
      this.m_tradeButton.Enabled = (long) userId != (long) playerId2 & MyTradingManager.ValidateTradeProssible(userId, playerId2, out MyPlayer _, out MyPlayer _) == MyTradeResponseReason.Ok;
    }

    protected void Multiplayer_PlayerJoined(ulong userId, string userName) => this.AddPlayer(userId);

    protected void Multiplayer_PlayerLeft(ulong userId, MyChatMemberStateChangeEnum arg2) => this.RemovePlayer(userId);

    protected void Matchmaking_LobbyDataUpdate(bool success, IMyLobby lobby, ulong memberOrLobby)
    {
      if (!success)
        return;
      ulong newOwnerId = lobby.OwnerId;
      MyGuiControlTable.Row row1 = this.m_playersTable.Find((Predicate<MyGuiControlTable.Row>) (row => row.GetCell(this.GameAdminColumn).Text.Length == "*****".Length));
      MyGuiControlTable.Row row2 = this.m_playersTable.Find((Predicate<MyGuiControlTable.Row>) (row => (long) (ulong) row.UserData == (long) newOwnerId));
      row1?.GetCell(this.GameAdminColumn).Text.Clear();
      row2?.GetCell(this.GameAdminColumn).Text.Clear().Append("*****");
      MyLobbyType lobbyType = lobby.LobbyType;
      this.m_lobbyTypeCombo.SelectItemByKey((long) lobbyType, false);
      MySession.Static.Settings.OnlineMode = this.GetOnlineMode(lobbyType);
      this.UpdateButtonsEnabledState();
      if (Sync.IsServer)
        return;
      this.m_maxPlayersSlider.ValueChanged = (Action<MyGuiControlSlider>) null;
      MySession.Static.Settings.MaxPlayers = (short) MyMultiplayer.Static.MemberLimit;
      this.m_maxPlayersSlider.Value = (float) MySession.Static.MaxPlayers;
      this.m_maxPlayersSlider.ValueChanged = new Action<MyGuiControlSlider>(this.MaxPlayersSlider_Changed);
      this.m_maxPlayersValueLabel.Text = this.m_maxPlayersSlider.Value.ToString();
      this.UpdateCaption();
    }

    protected MyOnlineModeEnum GetOnlineMode(MyLobbyType lobbyType)
    {
      switch (lobbyType)
      {
        case MyLobbyType.Private:
          return MyOnlineModeEnum.PRIVATE;
        case MyLobbyType.FriendsOnly:
          return MyOnlineModeEnum.FRIENDS;
        case MyLobbyType.Public:
          return MyOnlineModeEnum.PUBLIC;
        default:
          return MyOnlineModeEnum.PUBLIC;
      }
    }

    protected void playersTable_ItemSelected(
      MyGuiControlTable table,
      MyGuiControlTable.EventArgs args)
    {
      this.UpdateButtonsEnabledState();
      if (this.m_playersTable.SelectedRow == null)
        return;
      this.m_lastSelected = (ulong) this.m_playersTable.SelectedRow.UserData;
    }

    protected void inviteButton_ButtonClicked(MyGuiControlButton obj) => MyGameService.OpenInviteOverlay();

    protected void promoteButton_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_playersTable.SelectedRow;
      if (selectedRow == null || !MySession.Static.CanPromoteUser(Sync.MyId, (ulong) selectedRow.UserData))
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, bool>((Func<IMyEventOwner, Action<ulong, bool>>) (x => new Action<ulong, bool>(MyGuiScreenPlayers.Promote)), (ulong) selectedRow.UserData, true);
    }

    protected void demoteButton_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_playersTable.SelectedRow;
      if (selectedRow == null || !MySession.Static.CanDemoteUser(Sync.MyId, (ulong) selectedRow.UserData))
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, bool>((Func<IMyEventOwner, Action<ulong, bool>>) (x => new Action<ulong, bool>(MyGuiScreenPlayers.Promote)), (ulong) selectedRow.UserData, false);
    }

    [Event(null, 860)]
    [Reliable]
    [Server]
    public static void Promote(ulong playerId, bool promote)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (promote && !MySession.Static.CanPromoteUser(MyEventContext.Current.Sender.Value, playerId) || !promote && !MySession.Static.CanDemoteUser(MyEventContext.Current.Sender.Value, playerId)))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, false, (string) null, true);
      else
        MyGuiScreenPlayers.PromoteImplementation(playerId, promote);
    }

    public static void PromoteImplementation(ulong playerId, bool promote)
    {
      MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(playerId);
      MyPromoteLevel level;
      if (promote)
      {
        if (userPromoteLevel >= MyPromoteLevel.Admin)
          return;
        level = userPromoteLevel + 1;
        if (!MySession.Static.EnableScripterRole && level == MyPromoteLevel.Scripter)
          ++level;
      }
      else
      {
        if (userPromoteLevel == MyPromoteLevel.None)
          return;
        level = userPromoteLevel - 1;
        if (!MySession.Static.EnableScripterRole && level == MyPromoteLevel.Scripter)
          --level;
      }
      MySession.Static.SetUserPromoteLevel(playerId, level);
      MyMultiplayer.RaiseStaticEvent<MyPromoteLevel, bool>((Func<IMyEventOwner, Action<MyPromoteLevel, bool>>) (x => new Action<MyPromoteLevel, bool>(MyGuiScreenPlayers.ShowPromoteMessage)), level, promote, new EndpointId(playerId));
    }

    [Event(null, 909)]
    [Reliable]
    [Client]
    protected static void ShowPromoteMessage(MyPromoteLevel promoteLevel, bool promote)
    {
      MyGuiScreenPlayers.ClearPromoteNotificaions();
      switch (promoteLevel)
      {
        case MyPromoteLevel.None:
          MyHud.Notifications.Add(MyNotificationSingletons.PlayerDemotedNone);
          break;
        case MyPromoteLevel.Scripter:
          MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedScripter : MyNotificationSingletons.PlayerDemotedScripter);
          break;
        case MyPromoteLevel.Moderator:
          MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedModerator : MyNotificationSingletons.PlayerDemotedModerator);
          break;
        case MyPromoteLevel.SpaceMaster:
          MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedSpaceMaster : MyNotificationSingletons.PlayerDemotedSpaceMaster);
          break;
        case MyPromoteLevel.Admin:
          MyHud.Notifications.Add(MyNotificationSingletons.PlayerPromotedAdmin);
          break;
        case MyPromoteLevel.Owner:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (promoteLevel), (object) promoteLevel, (string) null);
      }
    }

    private static void ClearPromoteNotificaions()
    {
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedNone);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedScripter);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedModerator);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedSpaceMaster);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedScripter);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedModerator);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedSpaceMaster);
      MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedAdmin);
    }

    protected static void Refresh()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>()?.RecreateControls(false);
    }

    protected void kickButton_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_playersTable.SelectedRow;
      if (selectedRow == null)
        return;
      MyMultiplayer.Static.KickClient((ulong) selectedRow.UserData);
    }

    protected void banButton_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTable.Row selectedRow = this.m_playersTable.SelectedRow;
      if (selectedRow == null)
        return;
      MyMultiplayer.Static.BanClient((ulong) selectedRow.UserData, true);
    }

    protected void lobbyTypeCombo_OnSelect()
    {
      MyLobbyType selectedKey = (MyLobbyType) this.m_lobbyTypeCombo.GetSelectedKey();
      this.m_lobbyTypeCombo.SelectItemByKey((long) MyMultiplayer.Static.GetLobbyType(), false);
      MyMultiplayer.Static.SetLobbyType(selectedKey);
    }

    protected void MaxPlayersSlider_Changed(MyGuiControlSlider control)
    {
      MySession.Static.Settings.MaxPlayers = (short) this.m_maxPlayersSlider.Value;
      MyMultiplayer.Static.SetMemberLimit((int) MySession.Static.MaxPlayers);
      this.m_maxPlayersValueLabel.Text = this.m_maxPlayersSlider.Value.ToString();
      this.UpdateCaption();
    }

    private void OnFactionCreated(long insertedId) => MyGuiScreenPlayers.RefreshPlusPings();

    private void OnFactionEdited(long editedId) => MyGuiScreenPlayers.RefreshPlusPings();

    private void OnFactionStateChanged(
      MyFactionStateChange action,
      long fromFactionId,
      long toFactionId,
      long playerId,
      long senderId)
    {
      MyGuiScreenPlayers.RefreshPlusPings();
    }

    private void OnUserPromoteLevelChanged(ulong steamId, MyPromoteLevel promotionLevel)
    {
      for (int index1 = 0; index1 < this.m_playersTable.RowsCount; ++index1)
      {
        MyGuiControlTable.Row row = this.m_playersTable.GetRow(index1);
        if (row.UserData is ulong && (long) (ulong) row.UserData == (long) steamId)
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index2 = 0; (MyPromoteLevel) index2 < promotionLevel; ++index2)
            stringBuilder.Append("*");
          MyGuiControlTable.Cell cell = row.GetCell(this.GameAdminColumn);
          cell.Text.Clear();
          cell.Text.Append((object) stringBuilder);
          break;
        }
      }
      this.UpdateButtonsEnabledState();
      if (!this.m_promoteButton.Enabled && this.m_promoteButton.HasFocus)
      {
        if (!this.m_demoteButton.Enabled)
          return;
        this.FocusedControl = (MyGuiControlBase) this.m_demoteButton;
      }
      else
      {
        if (this.m_demoteButton.Enabled || !this.m_demoteButton.HasFocus || !this.m_promoteButton.Enabled)
          return;
        this.FocusedControl = (MyGuiControlBase) this.m_promoteButton;
      }
    }

    public static void RefreshPlusPings()
    {
      if (Sync.IsServer)
      {
        if (Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static == null || !(MyMultiplayer.Static.ReplicationLayer is MyReplicationServer replicationLayer))
          return;
        SerializableDictionary<ulong, short> pings;
        replicationLayer.GetClientPings(out pings);
        MyGuiScreenPlayers.SendPingsAndRefresh(pings);
      }
      else
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenPlayers.RequestPingsAndRefresh)));
    }

    [Event(null, 1075)]
    [Reliable]
    [Server]
    public static void RequestPingsAndRefresh()
    {
      if (!Sync.IsServer || MySession.Static == null || !(MyMultiplayer.Static.ReplicationLayer is MyReplicationServer replicationLayer))
        return;
      SerializableDictionary<ulong, short> pings;
      replicationLayer.GetClientPings(out pings);
      MyMultiplayer.RaiseStaticEvent<SerializableDictionary<ulong, short>>((Func<IMyEventOwner, Action<SerializableDictionary<ulong, short>>>) (s => new Action<SerializableDictionary<ulong, short>>(MyGuiScreenPlayers.SendPingsAndRefresh)), pings, new EndpointId(MyEventContext.Current.Sender.Value));
    }

    [Event(null, 1096)]
    [Reliable]
    [Client]
    private static void SendPingsAndRefresh(SerializableDictionary<ulong, short> dictionary)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MyGuiScreenPlayers firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>();
      if (firstScreenOfType == null)
        return;
      firstScreenOfType.pings.Clear();
      foreach (KeyValuePair<ulong, short> keyValuePair in dictionary.Dictionary)
        firstScreenOfType.pings[keyValuePair.Key] = keyValuePair.Value;
      firstScreenOfType.RecreateControls(false);
    }

    public override bool Draw()
    {
      int num = base.Draw() ? 1 : 0;
      if (!this.m_getPingAndRefresh)
        return num != 0;
      this.m_getPingAndRefresh = false;
      MyGuiScreenPlayers.RefreshPlusPings();
      return num != 0;
    }

    private void RefreshMuteIcons() => this.m_lastMuteIndicatorsUpdate = 0;

    public override bool Update(bool hasFocus)
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component.IsEnabled && this.m_warfareUpdate_frameCount_current >= this.m_warfareUpdate_frameCount)
      {
        this.m_warfareUpdate_frameCount_current = 0;
        foreach (MyGuiControlBase control in this.Controls)
        {
          if (control.GetType() == typeof (MyGuiScreenPlayersWarfareTeamScoreTable))
          {
            IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId);
            if (factionById != null && factionById.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId)
              MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyFactionCollection.RequestFactionScoreAndPercentageUpdate)), factionById.FactionId, MyEventContext.Current.Sender);
          }
        }
      }
      ++this.m_warfareUpdate_frameCount_current;
      if (component != null && component.IsEnabled)
      {
        TimeSpan timeSpan = TimeSpan.FromMinutes((double) component.RemainingMinutes);
        string str = timeSpan.ToString(timeSpan.TotalHours >= 1.0 ? "hh\\:mm\\:ss" : "mm\\:ss");
        if (this.m_warfare_timeRemainting_time.Text != str)
          this.m_warfare_timeRemainting_time.Text = str;
      }
      foreach (MyGuiControlBase control in this.Controls)
      {
        if (control.GetType() == typeof (MyGuiScreenPlayersWarfareTeamScoreTable))
        {
          IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId);
          if (factionById != null && factionById.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId)
          {
            ((MyGuiScreenPlayersWarfareTeamScoreTable) control).ScorePoints = factionById.Score;
            ((MyGuiScreenPlayersWarfareTeamScoreTable) control).ObjectiveFinishedPercentage = factionById.ObjectivePercentageCompleted;
          }
        }
      }
      this.m_tradeButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_profileButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (MyGuiManager.TotalTimeInMilliseconds - this.m_lastMuteIndicatorsUpdate > 1000)
      {
        for (int index = 0; index < this.m_playersTable.RowsCount; ++index)
        {
          if (this.m_playersTable.GetRow(index).GetCell(this.PlayerMutedColumn).Control is MyGuiControlButton control)
          {
            bool flag = MyGameService.GetPlayerMutedState((ulong) control.UserData) == MyPlayerChatState.Muted;
            control.CustomStyle = flag ? this.m_buttonSizeStyleMuted : this.m_buttonSizeStyleUnMuted;
          }
        }
      }
      return base.Update(hasFocus);
    }

    [Event(null, 1190)]
    [Reliable]
    [Server]
    private static void SyncRemainingTimeWithClient()
    {
      if (!MySession.Static.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent<float, float>((Func<IMyEventOwner, Action<float, float>>) (s => new Action<float, float>(MyGuiScreenPlayers.RecieveTimeSync)), (float) MySession.Static.ElapsedGameTime.TotalSeconds, (float) MySession.Static.GetComponent<MySessionComponentMatch>().RemainingTime.Seconds, MyEventContext.Current.Sender);
    }

    [Event(null, 1200)]
    [Reliable]
    [Client]
    private static void RecieveTimeSync(float syncTimeSeconds, float timeLeftSeconds) => MySessionComponentMatch.SetTimeRemainingInternal(syncTimeSeconds, timeLeftSeconds);

    protected sealed class Promote\u003C\u003ESystem_UInt64\u0023System_Boolean : ICallSite<IMyEventOwner, ulong, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong playerId,
        in bool promote,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.Promote(playerId, promote);
      }
    }

    protected sealed class ShowPromoteMessage\u003C\u003EVRage_Game_ModAPI_MyPromoteLevel\u0023System_Boolean : ICallSite<IMyEventOwner, MyPromoteLevel, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyPromoteLevel promoteLevel,
        in bool promote,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.ShowPromoteMessage(promoteLevel, promote);
      }
    }

    protected sealed class RequestPingsAndRefresh\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.RequestPingsAndRefresh();
      }
    }

    protected sealed class SendPingsAndRefresh\u003C\u003EVRage_Serialization_SerializableDictionary`2\u003CSystem_UInt64\u0023System_Int16\u003E : ICallSite<IMyEventOwner, SerializableDictionary<ulong, short>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in SerializableDictionary<ulong, short> dictionary,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.SendPingsAndRefresh(dictionary);
      }
    }

    protected sealed class SyncRemainingTimeWithClient\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.SyncRemainingTimeWithClient();
      }
    }

    protected sealed class RecieveTimeSync\u003C\u003ESystem_Single\u0023System_Single : ICallSite<IMyEventOwner, float, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float syncTimeSeconds,
        in float timeLeftSeconds,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenPlayers.RecieveTimeSync(syncTimeSeconds, timeLeftSeconds);
      }
    }
  }
}
