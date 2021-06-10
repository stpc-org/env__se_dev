// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenMedicals
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
  [StaticEventOwner]
  public class MyGuiScreenMedicals : MyGuiScreenBase
  {
    private static readonly TimeSpan m_refreshInterval = TimeSpan.FromSeconds(10.0);
    private MyGuiControlLabel m_labelNoRespawn;
    private StringBuilder m_noRespawnHeader = new StringBuilder();
    private MyGuiControlTable m_respawnsTable;
    private MyGuiControlButton m_respawnButton;
    private MyGuiControlButton m_refreshButton;
    private MyGuiControlButton m_MotdButton;
    private MyGuiControlMultilineText m_noRespawnText;
    private MyGuiControlButton m_backToFactionsButton;
    private MyGuiControlButton m_showPlayersButton;
    private MyGuiControlTable m_factionsTable;
    private MyGuiControlButton m_selectFactionButton;
    private bool m_showFactions;
    private bool m_showMotD = true;
    private bool m_hideEmptyMotD = true;
    private bool m_isMotdOpen;
    private string m_lastMotD = string.Empty;
    private MyGuiControlMultilineText m_motdMultiline;
    private bool m_blackgroundDrawFull = true;
    private float m_blackgroundFade = 1f;
    private bool m_isMultiplayerReady;
    private bool m_paused;
    private bool m_medbaySelect_SuppressNext;
    private MyGuiControlMultilineText m_multilineRespawnWhenShipReady;
    private object m_selectedRespawn;
    private bool m_haveSelection;
    private object m_selectedRowData;
    private MyGuiControlRotatingWheel m_rotatingWheelControl;
    private MyGuiControlLabel m_rotatingWheelLabel;
    private int m_streamingTimeout = 240;
    private bool m_streamingStarted;
    private bool m_selectedRowIsStreamable;
    private DateTime m_nextRefresh;
    private long m_requestedReplicable;
    private int m_showPreviewTime;
    private bool m_respawning;
    private MyGuiControlTable.Row m_previouslySelected;
    private MyGuiControlParent m_descriptionControl;
    private List<string> m_preloadedTextures = new List<string>();
    private StringBuilder m_factionTooltip = new StringBuilder();
    private MyGuiControlTable.Row m_lastSelectedFactionRow;
    private MyFaction m_applyingToFaction;
    private long m_restrictedRespawn;
    private bool m_waitingForRespawnShip;
    private const int SAFE_FRAME_COUNT = 5;
    private int m_blackgroundCounter;
    private int m_lastTimeSec = -1;
    private readonly List<MyPhysics.HitInfo> m_raycastList = new List<MyPhysics.HitInfo>(16);
    private float m_cameraRayLength = 20f;
    private long m_lastMedicalRoomId;
    private MyGuiControlLabel m_spectatorHintLabel;
    private bool m_refocusMotDButton;
    private bool m_respawnButtonVisible;
    private bool m_gamepadHelpVisible;
    private const int CHANGE_SELECTION_RESPONSE_DELAY = 500;

    public static MyGuiScreenMedicals Static { get; private set; }

    public static StringBuilder NoRespawnText
    {
      set
      {
        if (MyGuiScreenMedicals.Static == null)
          return;
        MyGuiScreenMedicals.Static.m_noRespawnText.Text = value;
      }
    }

    public static int ItemsInTable => MyGuiScreenMedicals.Static == null || MyGuiScreenMedicals.Static.m_respawnsTable == null ? 0 : MyGuiScreenMedicals.Static.m_respawnsTable.RowsCount;

    public bool IsBlackgroundVisible => this.m_blackgroundDrawFull || (double) this.m_blackgroundFade > 0.0;

    public bool IsBlackgroundFading => !this.m_blackgroundDrawFull && (double) this.m_blackgroundFade > 0.0;

    public MyGuiScreenMedicals(bool showFactions, long restrictedRespawn)
      : base(new Vector2?(new Vector2(0.1f, 0.1f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4f, 0.9f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_showFactions = showFactions;
      this.m_restrictedRespawn = restrictedRespawn;
      this.m_showMotD = MySession.ShowMotD && !Sandbox.Engine.Platform.Game.IsDedicated;
      this.m_position = this.GetPositionFromRatio();
      MyGuiScreenMedicals.Static = this;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = false;
      this.m_closeOnEsc = false;
      this.m_selectedRespawn = (object) null;
      this.CanBeHidden = false;
      this.RecreateControls(true);
      if (!Sync.MultiplayerActive)
      {
        MySandboxGame.PausePush();
        this.m_paused = true;
      }
      MySession.Static.Factions.OnPlayerJoined += new Action<MyFaction, long>(this.OnPlayerJoinedFaction);
      MySession.Static.Factions.OnPlayerLeft += new Action<MyFaction, long>(this.OnPlayerKickedFromFaction);
      MyCampaignManager.AfterCampaignLocalizationsLoaded += new Action(this.AfterLocalizationLoaded);
    }

    public override string GetFriendlyName() => "MyGuiScreenMyGuiScreenMedicals";

    protected override void OnShow() => MyHud.Notifications.Clear();

    protected override void OnClosed()
    {
      if (this.m_paused)
      {
        this.m_paused = false;
        MySandboxGame.PausePop();
      }
      MyHud.RotatingWheelText = MyHud.Empty;
      this.UnrequestReplicable();
      MySession.Static.Factions.OnPlayerJoined -= new Action<MyFaction, long>(this.OnPlayerJoinedFaction);
      MySession.Static.Factions.OnPlayerLeft -= new Action<MyFaction, long>(this.OnPlayerKickedFromFaction);
      MyCampaignManager.AfterCampaignLocalizationsLoaded -= new Action(this.AfterLocalizationLoaded);
      base.OnClosed();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MAIN_MENU))
      {
        if (!MyInput.Static.IsAnyShiftKeyPressed())
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenMainMenu());
        }
        else if (MySession.Static.HasPlayerSpectatorRights(Sync.MyId))
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.ScreenMedicals_ActivateSpectator_Confirm), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
          {
            if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            MyGuiScreenMedicals.Close();
          })), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
        }
      }
      if (MyInput.Static.IsJoystickLastUsed == this.m_gamepadHelpVisible)
        return;
      this.ChangeGamepadHelpVisibility(MyInput.Static.IsJoystickLastUsed);
    }

    public override void RecreateControls(bool constructor)
    {
      Vector2 offsetting = Vector2.Zero;
      StringBuilder text;
      if (this.m_showMotD)
      {
        this.m_isMotdOpen = true;
        this.m_size = new Vector2?(new Vector2(0.4f, 0.9f));
        this.m_position = new Vector2(0.8f, 0.5f);
        offsetting = new Vector2(0.0f, 0.0f);
        text = MyTexts.Get(MyCommonTexts.HideMotD);
      }
      else
      {
        this.m_isMotdOpen = false;
        this.m_size = new Vector2?(new Vector2(0.4f, 0.9f));
        this.m_position = new Vector2(0.8f, 0.5f);
        offsetting = new Vector2(0.0f, 0.0f);
        text = MyTexts.Get(MyCommonTexts.ShowMotD);
      }
      base.RecreateControls(constructor);
      if (this.m_showMotD && !string.IsNullOrEmpty(this.m_lastMotD))
      {
        MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
        myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
        myGuiControlImage.SetTexture(MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture);
        myGuiControlImage.Size = new Vector2(0.58f, 0.9f);
        myGuiControlImage.Position = new Vector2(-0.23f, -0.45f);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage);
        MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
        controlSeparatorList.AddHorizontal(new Vector2(-0.61f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.899999976158142 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 1.35f);
        this.Controls.Add((MyGuiControlBase) controlSeparatorList);
        MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
        controlMultilineText.Position = new Vector2(-0.79f, -0.358f);
        controlMultilineText.Size = new Vector2(0.54f, 0.783f);
        controlMultilineText.Font = "Blue";
        controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_motdMultiline = controlMultilineText;
        this.m_motdMultiline.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBordered;
        this.m_motdMultiline.CanHaveFocus = true;
        this.m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(this.m_lastMotD));
        this.m_motdMultiline.TextPadding = new MyGuiBorderThickness(0.01f);
        this.Controls.Add((MyGuiControlBase) this.m_motdMultiline);
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, (float) (-(double) this.m_size.Value.Y / 2.0) + MyGuiConstants.SCREEN_CAPTION_DELTA_Y) + new Vector2(-0.52f, 3f / 1000f)), text: MyTexts.GetString(MyCommonTexts.MotD_Caption), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        myGuiControlLabel.Name = "CaptionLabel";
        myGuiControlLabel.Font = "ScreenCaption";
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      if (!this.m_showFactions || MySession.Static.Settings.EnableTeamBalancing)
        this.RecreateControlsRespawn(offsetting);
      else
        this.RecreateControlsFactions(offsetting);
      this.m_MotdButton = new MyGuiControlButton(new Vector2?(new Vector2(3f / 1000f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.155000001192093)) + offsetting), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(0.36f, 0.033f)), text: text, onButtonClick: new Action<MyGuiControlButton>(this.onMotdClick));
      this.Controls.Add((MyGuiControlBase) this.m_MotdButton);
      if (Sandbox.Engine.Platform.Game.IsDedicated || string.IsNullOrEmpty(this.m_lastMotD))
        this.m_MotdButton.Enabled = false;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.175f, -0.34f)), text: MyTexts.GetString(MyCommonTexts.MotDCaption), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      this.m_lastMedicalRoomId = 0L;
      this.m_rotatingWheelControl = new MyGuiControlRotatingWheel(new Vector2?(new Vector2(0.5f, 0.8f) - this.m_position));
      this.Controls.Add((MyGuiControlBase) this.m_rotatingWheelControl);
      this.Controls.Add((MyGuiControlBase) (this.m_rotatingWheelLabel = new MyGuiControlLabel()));
      MyHud.RotatingWheelText = MyTexts.Get(MySpaceTexts.LoadingWheel_Streaming);
      if (MySession.Static.HasPlayerSpectatorRights(Sync.MyId))
      {
        this.m_spectatorHintLabel = new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, 0.51f) * this.m_size.Value), text: MyTexts.GetString(MySpaceTexts.ScreenMedicals_ActivateSpectator), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
        this.m_spectatorHintLabel.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.Controls.Add((MyGuiControlBase) this.m_spectatorHintLabel);
      }
      if (!this.m_showFactions)
        this.FocusedControl = (MyGuiControlBase) this.m_respawnsTable;
      else
        this.FocusedControl = (MyGuiControlBase) this.m_factionsTable;
      if (!this.m_refocusMotDButton || this.m_MotdButton == null || !this.m_MotdButton.Enabled)
        return;
      this.m_refocusMotDButton = false;
      this.FocusedControl = (MyGuiControlBase) this.m_MotdButton;
    }

    private void RecreateControlsRespawn(Vector2 offsetting)
    {
      this.AddCaption(MyCommonTexts.Medicals_Title, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.899999976158142 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.9f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.899999976158142 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.180000007152557 + 0.00999999977648258)), this.m_size.Value.X * 0.9f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Position = new Vector2(0.0f, (float) (-0.5 * (double) this.Size.Value.Y + 80.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.Y));
      controlMultilineText.Size = new Vector2(this.Size.Value.X * 0.85f, 75f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
      controlMultilineText.Font = "Red";
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_multilineRespawnWhenShipReady = controlMultilineText;
      this.Controls.Add((MyGuiControlBase) this.m_multilineRespawnWhenShipReady);
      this.UpdateRespawnShipLabel();
      this.m_respawnsTable = new MyGuiControlTable();
      this.m_respawnsTable.Position = new Vector2(0.0f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.699999988079071)) + offsetting + new Vector2(0.0f, -0.03f);
      this.m_respawnsTable.Size = new Vector2(575f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1.3f);
      this.m_respawnsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.m_respawnsTable.VisibleRowsCount = 16;
      this.Controls.Add((MyGuiControlBase) this.m_respawnsTable);
      this.m_respawnsTable.ColumnsCount = 2;
      this.m_respawnsTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_respawnsTable.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemDoubleClick);
      this.m_respawnsTable.ItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemDoubleClick);
      this.m_respawnsTable.ItemMouseOver += new Action<MyGuiControlTable.Row>(this.respawnsTable_ItemMouseOver);
      this.m_respawnsTable.SetCustomColumnWidths(new float[2]
      {
        0.5f,
        0.5f
      });
      this.m_respawnsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
      this.m_respawnsTable.SetColumnName(1, MyTexts.Get(MySpaceTexts.ScreenMedicals_OwnerTimeoutColumn));
      this.m_respawnsTable.GamepadHelpTextId = MySpaceTexts.MedicalsScreen_Help_RespawnList;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(0.0f, -0.35f) + offsetting;
      myGuiControlLabel1.ColorMask = (Vector4) Color.Red;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_labelNoRespawn = myGuiControlLabel1;
      this.Controls.Add((MyGuiControlBase) this.m_labelNoRespawn);
      if (this.m_applyingToFaction != null)
        this.Controls.Add((MyGuiControlBase) new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.02f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.209999993443489)) + offsetting), new Vector2?(new Vector2(0.32f, 0.5f)), font: "Red", contents: MyTexts.Get(MySpaceTexts.ScreenMedicals_WaitingForAcceptance)));
      this.m_backToFactionsButton = new MyGuiControlButton(new Vector2?(new Vector2(3f / 1000f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0450000017881393)) + offsetting), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(0.36f, 0.033f)), text: MyTexts.Get(MySpaceTexts.ScreenMedicals_BackToFactionSelection), onButtonClick: new Action<MyGuiControlButton>(this.OnBackToFactionsClick));
      this.Controls.Add((MyGuiControlBase) this.m_backToFactionsButton);
      this.m_backToFactionsButton.Enabled = !this.IsPlayerInFaction() && !MySession.Static.Settings.EnableTeamBalancing;
      this.m_backToFactionsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_respawnButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.09f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.100000001490116)) + offsetting), text: MyTexts.Get(MyCommonTexts.Respawn), onButtonClick: new Action<MyGuiControlButton>(this.onRespawnClick));
      this.Controls.Add((MyGuiControlBase) this.m_respawnButton);
      this.m_respawnButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_respawnButtonVisible = true;
      this.m_refreshButton = new MyGuiControlButton(new Vector2?(new Vector2(0.095f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.100000001490116)) + offsetting), text: MyTexts.Get(MyCommonTexts.Refresh), onButtonClick: new Action<MyGuiControlButton>(this.OnRefreshClick));
      this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_refreshButton);
      this.m_noRespawnText = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.02f, -0.19f) + offsetting), new Vector2?(new Vector2(0.32f, 0.5f)), font: "Red", contents: MyTexts.Get(MySpaceTexts.ScreenMedicals_NoRespawnPossible));
      this.Controls.Add((MyGuiControlBase) this.m_noRespawnText);
      this.CreateDetailInfoControl();
      this.RefreshRespawnPoints(false);
      this.Controls.Add((MyGuiControlBase) this.m_descriptionControl);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_respawnButton.Position.X - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.X / 2f, this.m_respawnButton.Position.Y)));
      myGuiControlLabel2.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.GamepadHelpTextId = new MyStringId?(this.IsPlayerInFaction() ? MySpaceTexts.MedicalsScreen_Help_Respawn_Factionless : MySpaceTexts.MedicalsScreen_Help_Respawn);
    }

    private void RecreateControlsFactions(Vector2 offsetting)
    {
      this.AddCaption(MyCommonTexts.Factions, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.899999976158142 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.9f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.899999976158142 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.180000007152557 + 0.00999999977648258)), this.m_size.Value.X * 0.9f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_factionsTable = new MyGuiControlTable();
      this.m_factionsTable.Position = new Vector2(0.0f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.699999988079071)) + offsetting + new Vector2(0.0f, -0.604f);
      this.m_factionsTable.Size = new Vector2(575f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1.3f);
      this.m_factionsTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_factionsTable.VisibleRowsCount = 16;
      this.Controls.Add((MyGuiControlBase) this.m_factionsTable);
      this.m_factionsTable.ColumnsCount = 2;
      this.m_factionsTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnFactionsTableItemSelected);
      this.m_factionsTable.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnFactionsTableItemDoubleClick);
      this.m_factionsTable.ItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnFactionsTableItemDoubleClick);
      this.m_factionsTable.ItemMouseOver += new Action<MyGuiControlTable.Row>(this.OnFactionsTableItemMouseOver);
      this.m_factionsTable.ItemFocus += new Action<MyGuiControlTable.Row>(this.OnFactionsTableItemMouseOver);
      this.m_factionsTable.SetCustomColumnWidths(new float[2]
      {
        0.2f,
        0.8f
      });
      this.m_factionsTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Tag));
      this.m_factionsTable.SetColumnName(1, MyTexts.Get(MyCommonTexts.Name));
      this.m_factionsTable.GamepadHelpTextId = MySpaceTexts.MedicalsScreen_Help_FactionList;
      this.m_showPlayersButton = new MyGuiControlButton(new Vector2?(new Vector2(3f / 1000f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0450000017881393)) + offsetting), MyGuiControlButtonStyleEnum.Rectangular, new Vector2?(new Vector2(0.36f, 0.033f)), text: MyTexts.Get(MyCommonTexts.ScreenMenuButtonPlayers), onButtonClick: new Action<MyGuiControlButton>(this.OnShowPlayersClick));
      this.m_showPlayersButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_showPlayersButton);
      this.m_showPlayersButton.Enabled = MyMultiplayer.Static != null;
      this.m_selectFactionButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.09f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.100000001490116)) + offsetting), text: MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Join), onButtonClick: new Action<MyGuiControlButton>(this.OnFactionSelectClick));
      this.Controls.Add((MyGuiControlBase) this.m_selectFactionButton);
      this.m_selectFactionButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_refreshButton = new MyGuiControlButton(new Vector2?(new Vector2(0.095f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.100000001490116)) + offsetting), text: MyTexts.Get(MyCommonTexts.Refresh), onButtonClick: new Action<MyGuiControlButton>(this.OnFactionsRefreshClick));
      this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_refreshButton);
      this.RefreshFactions();
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_selectFactionButton.Position.X - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.X / 2f, this.m_selectFactionButton.Position.Y)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      if (MyMultiplayer.Static != null)
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MedicalsScreen_Help_FactionsMultiplayer);
      else
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MedicalsScreen_Help_Factions);
    }

    public void CreateDetailInfoControl()
    {
      float x = 0.25f;
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.BorderSize = 1;
      guiControlParent.BorderEnabled = true;
      guiControlParent.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      guiControlParent.Visible = false;
      guiControlParent.Size = new Vector2(x, 0.0f);
      guiControlParent.Position = new Vector2(-0.33f, -0.224f);
      guiControlParent.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      MyGuiControls controls1 = guiControlParent.Controls;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.TextToDraw = new StringBuilder();
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      controls1.Add((MyGuiControlBase) myGuiControlLabel);
      MyGuiControls controls2 = guiControlParent.Controls;
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.BorderSize = 1;
      myGuiControlImage.BorderEnabled = true;
      myGuiControlImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      myGuiControlImage.Visible = false;
      myGuiControlImage.Size = x * new Vector2(1f, 0.7f);
      myGuiControlImage.Padding = new MyGuiBorderThickness(2f, 2f, 2f, 2f);
      myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      controls2.Add((MyGuiControlBase) myGuiControlImage);
      MyGuiControls controls3 = guiControlParent.Controls;
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(drawScrollbarV: false, drawScrollbarH: false);
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlMultilineText.Size = new Vector2(x, 0.05f);
      controls3.Add((MyGuiControlBase) controlMultilineText);
      this.m_descriptionControl = guiControlParent;
    }

    private void ChangeGamepadHelpVisibility(bool gamepadHelpVisible)
    {
      this.m_gamepadHelpVisible = gamepadHelpVisible;
      this.UpdateGamepadHelp(this.FocusedControl);
      if (this.m_backToFactionsButton != null)
        this.m_backToFactionsButton.Visible = !gamepadHelpVisible;
      if (this.m_respawnButton != null)
        this.m_respawnButton.Visible = !gamepadHelpVisible && this.m_respawnButtonVisible;
      if (this.m_refreshButton != null)
        this.m_refreshButton.Visible = !gamepadHelpVisible;
      if (this.m_showPlayersButton != null)
        this.m_showPlayersButton.Visible = !gamepadHelpVisible;
      if (this.m_selectFactionButton != null)
        this.m_selectFactionButton.Visible = !gamepadHelpVisible;
      if (this.m_spectatorHintLabel == null)
        return;
      this.m_spectatorHintLabel.Visible = !gamepadHelpVisible;
    }

    private Vector2 GetPositionFromRatio()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      switch (MyVideoSettingsManager.GetClosestAspectRatio((float) fullscreenRectangle.Width / (float) fullscreenRectangle.Height))
      {
        case MyAspectRatioEnum.Normal_4_3:
          return new Vector2(0.79f, 0.52f);
        case MyAspectRatioEnum.Normal_16_9:
          return new Vector2(0.95f, 0.52f);
        case MyAspectRatioEnum.Normal_16_10:
          return new Vector2(0.88f, 0.52f);
        case MyAspectRatioEnum.Unsupported_5_4:
          return new Vector2(0.76f, 0.52f);
        default:
          return new Vector2(0.95f, 0.52f);
      }
    }

    private void RefreshRotatingWheelLabel()
    {
      bool flag = MyHud.RotatingWheelVisible || this.m_respawning || this.m_blackgroundDrawFull && this.m_selectedRowIsStreamable;
      this.m_rotatingWheelLabel.Visible = flag;
      this.m_rotatingWheelControl.Visible = flag;
      if (!MyHud.RotatingWheelVisible && !this.m_respawning || this.m_rotatingWheelLabel.TextToDraw == MyHud.RotatingWheelText)
        return;
      this.m_rotatingWheelLabel.Position = this.m_rotatingWheelControl.Position + new Vector2(0.0f, 0.05f);
      this.m_rotatingWheelLabel.TextToDraw = MyHud.RotatingWheelText;
      this.m_rotatingWheelLabel.PositionX -= this.m_rotatingWheelLabel.GetTextSize().X / 2f;
    }

    private void RefreshRespawnPoints(bool clear)
    {
      if (clear)
        this.m_respawnsTable.Clear();
      this.m_nextRefresh = DateTime.UtcNow + MyGuiScreenMedicals.m_refreshInterval;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyGuiScreenMedicals.RefreshRespawnPointsRequest)));
    }

    private void UpdateSpawnTimes()
    {
      for (int index = 0; index < this.m_respawnsTable.RowsCount; ++index)
      {
        MyGuiControlTable.Row row = this.m_respawnsTable.GetRow(index);
        string respawnShipId = (row.UserData is MyRespawnShipDefinition userData ? userData.Id.SubtypeName : (string) null) ?? (row.UserData is MyGuiScreenMedicals.MyPlanetInfo userData ? userData.RespawnShipForCooldownCheck : (string) null);
        if (respawnShipId != null)
        {
          MyGuiControlTable.Cell cell1 = row.GetCell(0);
          MyGuiControlTable.Cell cell2 = row.GetCell(1);
          Color cooldownInfo = MyGuiScreenMedicals.GetCooldownInfo(respawnShipId, cell2.Text.Clear());
          cell1.TextColor = new Color?(cooldownInfo);
          cell2.TextColor = new Color?(cooldownInfo);
        }
      }
    }

    private static Color GetCooldownInfo(string respawnShipId, StringBuilder text)
    {
      MySpaceRespawnComponent respawnComponent = MySpaceRespawnComponent.Static;
      int timeInSeconds = MySession.Static.LocalHumanPlayer == null ? 0 : respawnComponent.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShipId);
      bool enabled = timeInSeconds == 0;
      if (!respawnComponent.IsSynced)
        text.Append((object) MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipNotReady));
      else if (enabled)
        text.Append((object) MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipReady));
      else
        MyValueFormatter.AppendTimeExact(timeInSeconds, text);
      return MyGuiControlBase.ApplyColorMaskModifiers((Vector4) Color.White, enabled, 1f);
    }

    private void UpdateRespawnShipLabel()
    {
      if (this.m_selectedRespawn == null)
      {
        this.m_multilineRespawnWhenShipReady.Visible = false;
      }
      else
      {
        string str;
        string respawnShipId;
        switch (this.m_selectedRespawn)
        {
          case MyGuiScreenMedicals.MyPlanetInfo myPlanetInfo:
            str = myPlanetInfo.PlanetName;
            respawnShipId = myPlanetInfo.RespawnShipForCooldownCheck;
            break;
          case MyRespawnShipDefinition respawnShipDefinition:
            str = respawnShipDefinition.DisplayNameText;
            respawnShipId = respawnShipDefinition.Id.SubtypeName;
            break;
          default:
            throw new Exception("Invalid branch " + this.m_selectedRespawn);
        }
        MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShipId);
        this.m_multilineRespawnWhenShipReady.Text.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScreenMedicals_RespawnWhenShipReady), (object) str);
        this.m_multilineRespawnWhenShipReady.RefreshText(false);
        this.m_multilineRespawnWhenShipReady.Visible = true;
      }
    }

    private static StringBuilder GetOwnerDisplayName(long owner)
    {
      if (owner == 0L)
        return MyTexts.Get(MySpaceTexts.BlockOwner_Nobody);
      MyIdentity identity = Sync.Players.TryGetIdentity(owner);
      return identity != null ? new StringBuilder(identity.DisplayName) : MyTexts.Get(MySpaceTexts.BlockOwner_Unknown);
    }

    [Event(null, 796)]
    [Reliable]
    [Server]
    private static void RefreshRespawnPointsRequest()
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (identityId == 0L)
        return;
      using (ClearToken<MySpaceRespawnComponent.MyRespawnPointInfo> availableRespawnPoints = MySpaceRespawnComponent.GetAvailableRespawnPoints(new long?(identityId), true))
      {
        MyGuiScreenMedicals.MyPlanetInfo[] array = EmptyArray<MyGuiScreenMedicals.MyPlanetInfo>.Value;
        if (MySession.Static.Settings.EnableRespawnShips)
          array = MyPlanets.GetPlanets().Select<MyPlanet, MyGuiScreenMedicals.MyPlanetInfo>((Func<MyPlanet, MyGuiScreenMedicals.MyPlanetInfo>) (planet =>
          {
            using (ClearToken<MyRespawnShipDefinition> respawnShips = MySpaceRespawnComponent.GetRespawnShips(planet))
            {
              if (respawnShips.List.Count == 0)
                return (MyGuiScreenMedicals.MyPlanetInfo) null;
              string str = (string) null;
              if (respawnShips.List.Count == 1)
                str = respawnShips.List[0].Id.SubtypeId.String;
              float num = 0.0f;
              if (planet.HasAtmosphere)
              {
                MyPlanetAtmosphere atmosphere = planet.Generator.Atmosphere;
                if (atmosphere.Breathable)
                  num = atmosphere.OxygenDensity;
              }
              MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(MyEventContext.Current.Sender.Value, 0);
              MyRespawnShipDefinition respawnShipDefinition = respawnShips.List.MinBy<MyRespawnShipDefinition>((Func<MyRespawnShipDefinition, float>) (ship => (float) MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(playerId, ship.Id.SubtypeName)));
              return new MyGuiScreenMedicals.MyPlanetInfo()
              {
                PlanetName = planet.Name,
                PlanetId = planet.EntityId,
                OxygenLevel = num,
                DropPodForDetail = str,
                WorldAABB = planet.PositionComp.WorldAABB,
                Gravity = planet.GetInitArguments.SurfaceGravity,
                Difficulty = planet.GetInitArguments.Generator.Difficulty.ToString(),
                RespawnShipForCooldownCheck = respawnShipDefinition.Id.SubtypeName
              };
            }
          })).Where<MyGuiScreenMedicals.MyPlanetInfo>((Func<MyGuiScreenMedicals.MyPlanetInfo, bool>) (x => x != null)).ToArray<MyGuiScreenMedicals.MyPlanetInfo>();
        MyMultiplayer.RaiseStaticEvent<List<MySpaceRespawnComponent.MyRespawnPointInfo>, MyGuiScreenMedicals.MyPlanetInfo[]>((Func<IMyEventOwner, Action<List<MySpaceRespawnComponent.MyRespawnPointInfo>, MyGuiScreenMedicals.MyPlanetInfo[]>>) (s => new Action<List<MySpaceRespawnComponent.MyRespawnPointInfo>, MyGuiScreenMedicals.MyPlanetInfo[]>(MyGuiScreenMedicals.RequestRespawnPointsResponse)), availableRespawnPoints.List, array, MyEventContext.Current.Sender);
      }
    }

    [Event(null, 859)]
    [Reliable]
    [Client]
    private static void RequestRespawnPointsResponse(
      List<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms,
      MyGuiScreenMedicals.MyPlanetInfo[] planetInfos)
    {
      MyGuiScreenMedicals.Static.RefreshMedicalRooms((ListReader<MySpaceRespawnComponent.MyRespawnPointInfo>) medicalRooms, planetInfos);
    }

    public static bool EqualRespawns(object first, object second)
    {
      if (first == null || second == null)
        return first == second;
      if (first.GetType() != second.GetType())
        return false;
      switch (first)
      {
        case MySpaceRespawnComponent.MyRespawnPointInfo _:
          if ((first as MySpaceRespawnComponent.MyRespawnPointInfo).MedicalRoomId != (second as MySpaceRespawnComponent.MyRespawnPointInfo).MedicalRoomId)
            return false;
          break;
        case MyRespawnShipDefinition _:
          MyRespawnShipDefinition respawnShipDefinition1 = first as MyRespawnShipDefinition;
          MyRespawnShipDefinition respawnShipDefinition2 = second as MyRespawnShipDefinition;
          if (respawnShipDefinition1.Prefab == null || respawnShipDefinition2.Prefab == null || respawnShipDefinition1.Prefab.PrefabPath != respawnShipDefinition2.Prefab.PrefabPath)
            return false;
          break;
        case MyGuiScreenMedicals.MyPlanetInfo _:
          if ((first as MyGuiScreenMedicals.MyPlanetInfo).PlanetId != (second as MyGuiScreenMedicals.MyPlanetInfo).PlanetId)
            return false;
          break;
      }
      return true;
    }

    private void RefreshMedicalRooms(
      ListReader<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms,
      MyGuiScreenMedicals.MyPlanetInfo[] planetInfos)
    {
      this.m_respawnsTable.Clear();
      AddMedicalRespawnPoints();
      if (!MySession.Static.CreativeMode && MySession.Static.Settings.EnableRespawnShips && !MySession.Static.Settings.Scenario)
      {
        AddPlanetSpawns();
        AddSpaceRespawnShips();
        AddManuallyPositionedShips();
      }
      if (MySession.Static.Settings.EnableJetpack)
        AddSuitRespawn();
      if (this.m_respawnsTable.RowsCount > 0)
      {
        if (this.m_haveSelection)
        {
          int indexByUserData = this.m_respawnsTable.FindIndexByUserData(ref this.m_selectedRowData, new MyGuiControlTable.EqualUserData(MyGuiScreenMedicals.EqualRespawns));
          if (indexByUserData >= 0)
          {
            this.m_medbaySelect_SuppressNext = true;
            this.m_respawnsTable.SelectedRowIndex = new int?(indexByUserData);
          }
          else
            this.m_respawnsTable.SelectedRowIndex = new int?(0);
        }
        else
          this.m_respawnsTable.SelectedRowIndex = new int?(0);
        this.OnTableItemSelected(this.m_respawnsTable, new MyGuiControlTable.EventArgs());
        this.m_noRespawnText.Visible = false;
      }
      else
        this.m_noRespawnText.Visible = true;

      void AddMedicalRespawnPoints()
      {
        foreach (MySpaceRespawnComponent.MyRespawnPointInfo medicalRoom in medicalRooms)
        {
          MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) medicalRoom);
          bool flag = this.m_restrictedRespawn == 0L || medicalRoom.MedicalRoomId == this.m_restrictedRespawn;
          row.AddCell(new MyGuiControlTable.Cell(medicalRoom.MedicalRoomName, textColor: (flag ? new Color?() : new Color?(Color.Gray))));
          row.AddCell(new MyGuiControlTable.Cell(flag ? MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipReady) : MyTexts.Get(MySpaceTexts.ScreenMedicals_RespawnShipNotReady), textColor: (flag ? new Color?() : new Color?(Color.Gray))));
          this.m_respawnsTable.Add(row);
        }
      }

      void AddPlanetSpawns()
      {
        BoundingBoxD worldBoundaries = GetWorldBoundaries();
        foreach (MyGuiScreenMedicals.MyPlanetInfo planetInfo in planetInfos)
        {
          if (worldBoundaries.Contains(planetInfo.WorldAABB) == ContainmentType.Contains)
          {
            MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) planetInfo);
            row.AddCell(new MyGuiControlTable.Cell(string.Format(MyTexts.GetString(MySpaceTexts.PlanetRespawnPod), (object) planetInfo.PlanetName)));
            MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(string.Empty);
            MyGuiScreenMedicals.GetCooldownInfo(planetInfo.RespawnShipForCooldownCheck, cell.Text);
            row.AddCell(cell);
            this.m_respawnsTable.Add(row);
          }
        }
      }

      void AddSpaceRespawnShips()
      {
        if (!DoesEmptySpaceExist())
          return;
        foreach (MyRespawnShipDefinition respawnShipDefinition in MyDefinitionManager.Static.GetRespawnShipDefinitions().Values)
        {
          if (respawnShipDefinition.UseForSpace && !respawnShipDefinition.SpawnPosition.HasValue)
          {
            MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) respawnShipDefinition);
            row.AddCell(new MyGuiControlTable.Cell(respawnShipDefinition.DisplayNameText));
            MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(string.Empty);
            MyGuiScreenMedicals.GetCooldownInfo(respawnShipDefinition.Id.SubtypeName, cell.Text);
            row.AddCell(cell);
            this.m_respawnsTable.Add(row);
          }
        }
      }

      void AddManuallyPositionedShips()
      {
        foreach (MyRespawnShipDefinition respawnShipDefinition in MyDefinitionManager.Static.GetRespawnShipDefinitions().Values)
        {
          if (respawnShipDefinition.SpawnPosition.HasValue && !respawnShipDefinition.UseForPlanetsWithAtmosphere && (!respawnShipDefinition.UseForPlanetsWithoutAtmosphere && respawnShipDefinition.PlanetTypes == null))
          {
            MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) respawnShipDefinition);
            row.AddCell(new MyGuiControlTable.Cell(respawnShipDefinition.DisplayNameText));
            MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(string.Empty);
            MyGuiScreenMedicals.GetCooldownInfo(respawnShipDefinition.Id.SubtypeName, cell.Text);
            row.AddCell(cell);
            this.m_respawnsTable.Add(row);
          }
        }
      }

      void AddSuitRespawn()
      {
        if (!DoesEmptySpaceExist())
          return;
        MyGuiControlTable.Row row = new MyGuiControlTable.Row();
        row.AddCell(new MyGuiControlTable.Cell(MyTexts.GetString(MySpaceTexts.SpawnInSpaceSuit)));
        row.AddCell(new MyGuiControlTable.Cell(MyTexts.GetString(MySpaceTexts.ScreenMedicals_RespawnShipReady)));
        this.m_respawnsTable.Add(row);
      }

      bool DoesEmptySpaceExist()
      {
        BoundingBoxD worldBoundaries = GetWorldBoundaries();
        foreach (MyGuiScreenMedicals.MyPlanetInfo planetInfo in planetInfos)
        {
          if (planetInfo.WorldAABB.Contains(worldBoundaries) == ContainmentType.Contains)
            return false;
        }
        return true;
      }

      BoundingBoxD GetWorldBoundaries()
      {
        if (MySession.Static.Settings.WorldSizeKm <= 0)
          return new BoundingBoxD(new Vector3D(double.MinValue, double.MinValue, double.MinValue), new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue));
        double num = (double) (MySession.Static.Settings.WorldSizeKm * 500);
        return new BoundingBoxD(new Vector3D(-num, -num, -num), new Vector3D(num, num, num));
      }
    }

    private void RefreshFactions()
    {
      this.m_factionsTable.Clear();
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (localHumanPlayer == null)
        return;
      if (MySession.Static.Settings.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION || MySession.Static.IsUserAdmin(Sync.MyId))
      {
        MyGuiControlTable.Row row = new MyGuiControlTable.Row();
        row.AddCell(new MyGuiControlTable.Cell());
        row.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.ScreenMedicals_NoFaction)));
        this.m_factionsTable.Add(row);
      }
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        int num = MySession.Static.Factions.IsNpcFaction(faction.Value.Tag) ? 1 : 0;
        bool flag1 = MySession.Static.Factions.IsFactionDiscovered(localHumanPlayer.Id, faction.Value.FactionId);
        if (num == 0 || flag1)
        {
          bool flag2 = faction.Value.AcceptHumans && (faction.Value.AutoAcceptMember || faction.Value.IsAnyLeaderOnline);
          if (flag2)
          {
            MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) faction.Value);
            row.AddCell(new MyGuiControlTable.Cell(faction.Value.Tag, textColor: (flag2 ? new Color?() : new Color?(Color.Red))));
            row.AddCell(new MyGuiControlTable.Cell(faction.Value.Name, textColor: (flag2 ? new Color?() : new Color?(Color.Red))));
            this.m_factionsTable.Add(row);
          }
        }
      }
      this.RefreshSelectFactionButton();
    }

    private static void BuildOxygenLevelInfo(StringBuilder ownerText, float oxygenLevel)
    {
      if (!MySession.Static.Settings.EnableOxygen)
        return;
      ownerText.Append(MyTexts.GetString(MySpaceTexts.HudInfoOxygen));
      ownerText.Append(": ");
      ownerText.Append((oxygenLevel * 100f).ToString("F0"));
      ownerText.Append("% ");
    }

    public override bool Update(bool hasFocus)
    {
      if (!this.m_showFactions)
        this.UpdateSpawnTimes();
      int num = base.Update(hasFocus) ? 1 : 0;
      if (MySandboxGame.IsPaused)
        MyHud.Notifications.UpdateBeforeSimulation();
      if (this.IsBlackgroundVisible)
      {
        this.UpdateBlackground();
        MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background.dds", MyGuiManager.GetSafeFullscreenRectangle(), new Color(new Vector4(0.0f, 0.0f, 0.0f, this.m_blackgroundFade)), true, true);
      }
      if (!this.m_showFactions)
      {
        if (this.m_selectedRespawn != null)
        {
          MySpaceRespawnComponent respawnComponent = MySpaceRespawnComponent.Static;
          MyGuiScreenMedicals.MyPlanetInfo myPlanetInfo1;
          string respawnShipId;
          switch (this.m_selectedRespawn)
          {
            case MyGuiScreenMedicals.MyPlanetInfo myPlanetInfo:
              respawnShipId = (myPlanetInfo1 = myPlanetInfo).RespawnShipForCooldownCheck;
              break;
            case MyRespawnShipDefinition respawnShipDefinition:
              myPlanetInfo1 = (MyGuiScreenMedicals.MyPlanetInfo) null;
              respawnShipId = respawnShipDefinition.Id.SubtypeName;
              break;
            default:
              throw new Exception("Invalid branch " + this.m_selectedRespawn);
          }
          int respawnCooldownSeconds = respawnComponent.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShipId);
          if (respawnComponent.IsSynced && respawnCooldownSeconds == 0)
            this.RespawnImmediately(myPlanetInfo1 == null ? respawnShipId : (string) null, myPlanetInfo1?.PlanetId);
        }
        if (DateTime.UtcNow > this.m_nextRefresh)
          this.RefreshRespawnPoints(false);
        if (this.m_labelNoRespawn.Text == null)
          this.m_labelNoRespawn.Visible = false;
        else
          this.m_labelNoRespawn.Visible = true;
      }
      if (!this.m_respawning && this.m_showPreviewTime != 0 && this.m_showPreviewTime <= MyGuiManager.TotalTimeInMilliseconds)
        this.ShowPreview();
      this.m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible;
      this.RefreshRotatingWheelLabel();
      if (this.m_respawning && MySession.Static.LocalCharacter != null && (!MySession.Static.LocalCharacter.IsDead && !this.m_blackgroundDrawFull))
      {
        if (this.m_paused)
        {
          this.m_paused = false;
          MySandboxGame.PausePop();
        }
        --this.m_blackgroundCounter;
        if (this.m_blackgroundCounter <= 0)
          this.CloseScreen();
      }
      MyHud.IsVisible = false;
      if (!hasFocus)
        return num != 0;
      if (!this.m_showFactions && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.CANCEL) && !this.IsPlayerInFaction())
        this.OnBackToFactionsClick((MyGuiControlButton) null);
      if (this.m_showFactions && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
        this.OnShowPlayersClick((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        return num != 0;
      if (this.m_showFactions)
      {
        this.RefreshFactions();
        return num != 0;
      }
      this.RefreshRespawnPoints(true);
      return num != 0;
    }

    private void UpdateBlackground()
    {
      if (this.m_blackgroundDrawFull)
      {
        if (!this.m_selectedRowIsStreamable && !this.m_respawning || !MySandboxGame.IsGameReady || !Sync.IsServer && MyFakes.ENABLE_WAIT_UNTIL_MULTIPLAYER_READY && !this.m_isMultiplayerReady || (this.m_waitingForRespawnShip || !MySandboxGame.AreClipmapsReady))
          return;
        this.m_blackgroundDrawFull = false;
      }
      else
      {
        if ((double) this.m_blackgroundFade <= 0.0)
          return;
        this.m_blackgroundFade -= 0.1f;
      }
    }

    public static void Close()
    {
      if (MyGuiScreenMedicals.Static == null)
        return;
      MyGuiScreenMedicals.Static.CloseScreen();
    }

    public override bool HandleInputAfterSimulation()
    {
      if (!this.m_showFactions && this.m_respawnsTable.SelectedRow != null && (MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity && this.m_respawnsTable.SelectedRow.UserData is MySpaceRespawnComponent.MyRespawnPointInfo userData))
      {
        this.m_respawnButton.Enabled = false;
        MyEntity entity;
        if ((this.m_restrictedRespawn == 0L || userData.MedicalRoomId == this.m_restrictedRespawn) && MyEntities.TryGetEntityById(userData.MedicalRoomId, out entity))
        {
          if (this.m_lastMedicalRoomId != userData.MedicalRoomId && (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.IsDead))
          {
            MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) entity, new Vector3D?());
            MyThirdPersonSpectator.Static.ResetInternalTimers();
            MyThirdPersonSpectator.Static.ResetViewerDistance(new double?((double) this.m_cameraRayLength));
            MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
            MyThirdPersonSpectator.Static.ResetSpring();
            this.m_lastMedicalRoomId = userData.MedicalRoomId;
          }
          this.m_respawnButton.Enabled = true;
        }
      }
      return true;
    }

    public static void SetNoRespawnText(StringBuilder text, int timeSec)
    {
      if (MyGuiScreenMedicals.Static == null)
        return;
      MyGuiScreenMedicals.Static.SetNoRespawnTexts(text, timeSec);
    }

    public void SetNoRespawnTexts(StringBuilder text, int timeSec)
    {
      MyGuiScreenMedicals.NoRespawnText = text;
      if (timeSec == this.m_lastTimeSec)
        return;
      this.m_lastTimeSec = timeSec;
      int num = timeSec / 60;
      this.m_noRespawnHeader.Clear().AppendFormat(MyTexts.GetString(MySpaceTexts.ScreenMedicals_NoRespawnPlaceHeader), (object) num, (object) (timeSec - num * 60));
      this.m_labelNoRespawn.Text = this.m_noRespawnHeader.ToString();
    }

    private bool IsPlayerInFaction() => MySession.Static.Factions.GetPlayerFaction(MySession.Static.LocalPlayerId) != null;

    private void respawnsTable_ItemMouseOver(MyGuiControlTable.Row row) => this.UpdateDetailedInfo(row);

    private void UpdateDetailedInfo(MyGuiControlTable.Row row)
    {
      MyGuiControlParent descriptionControl = this.m_descriptionControl;
      MyGuiControlImage control1 = (MyGuiControlImage) descriptionControl.Controls[1];
      MyGuiControlLabel control2 = (MyGuiControlLabel) descriptionControl.Controls[0];
      MyGuiControlMultilineText control3 = (MyGuiControlMultilineText) descriptionControl.Controls[2];
      descriptionControl.Position = new Vector2(-0.33f, -0.3f);
      descriptionControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      string texture = (string) null;
      StringBuilder ownerText = control2.TextToDraw.Clear();
      if (row != null && row.UserData != null)
      {
        string keyString = (string) null;
        if (row.UserData is MyRespawnShipDefinition userData)
        {
          if (!userData.Icons.IsNullOrEmpty<string>())
            texture = userData.Icons[0];
          keyString = userData.HelpTextLocalizationId;
          MyStringId difficultyHard = MySpaceTexts.DifficultyHard;
          ownerText.Append(MyTexts.GetString(MySpaceTexts.Difficulty)).Append(": ");
          ownerText.Append(MyTexts.GetString(difficultyHard)).AppendLine();
        }
        else if (row.UserData is MyGuiScreenMedicals.MyPlanetInfo userData)
        {
          if (!string.IsNullOrEmpty(userData.DropPodForDetail))
          {
            MyRespawnShipDefinition respawnShipDefinition = MyDefinitionManager.Static.GetRespawnShipDefinition(userData.DropPodForDetail);
            if (respawnShipDefinition != null)
            {
              if (!respawnShipDefinition.Icons.IsNullOrEmpty<string>())
                texture = respawnShipDefinition.Icons[0];
              keyString = respawnShipDefinition.HelpTextLocalizationId;
              ownerText.Append(MyTexts.GetString(MySpaceTexts.Difficulty)).Append(": ");
              ownerText.AppendLine(MyTexts.GetString(userData.Difficulty)).AppendLine();
              MyGuiScreenMedicals.BuildOxygenLevelInfo(ownerText, userData.OxygenLevel);
              ownerText.AppendLine();
              ownerText.Append(MyTexts.GetString(MySpaceTexts.HudInfoGravityNatural));
              ownerText.Append(' ').Append(userData.Gravity.ToString("F2")).AppendLine("g");
            }
          }
        }
        else if (row.UserData is MySpaceRespawnComponent.MyRespawnPointInfo userData)
        {
          ownerText.Append(MyTexts.GetString(MySpaceTexts.ScreenMedicals_Owner));
          ownerText.Append(": ").Append((object) MyGuiScreenMedicals.GetOwnerDisplayName(userData.OwnerId));
          ownerText.AppendLine();
          MyGuiScreenMedicals.BuildOxygenLevelInfo(ownerText, userData.OxygenLevel);
        }
        if (!string.IsNullOrEmpty(keyString))
        {
          control3.Text = new StringBuilder(MyTexts.GetString(keyString));
          control3.Visible = MySession.Static.Settings.EnableAutorespawn;
          control3.ScrollToShowCarriage();
        }
        else
          control3.Visible = false;
      }
      bool flag1 = ownerText.Length > 0;
      bool flag2 = !string.IsNullOrEmpty(texture);
      control1.Visible = flag2;
      control2.Visible = flag1;
      if (!flag2 && !flag1)
      {
        descriptionControl.Visible = false;
      }
      else
      {
        if (flag2 && (control1.Textures == null || control1.Textures.Length == 0 || control1.Textures[0].Texture != texture))
        {
          using (MyUtils.ReuseCollection<string>(ref this.m_preloadedTextures))
          {
            this.m_preloadedTextures.Add(texture);
            MyRenderProxy.PreloadTextures((IEnumerable<string>) this.m_preloadedTextures, TextureType.GUIWithoutPremultiplyAlpha);
            control1.SetTexture(texture);
          }
        }
        descriptionControl.Visible = true;
        control2.Size = new Vector2(descriptionControl.Size.X, control2.GetTextSize().Y * control2.TextScale);
        float y = 0.0f;
        if (flag2)
          y += control1.Size.Y;
        if (flag1)
        {
          y += control2.Size.Y + 0.02f;
          if (!flag2)
            y += 0.01f;
        }
        if (control3.Visible)
          y += control3.Size.Y + 0.02f;
        descriptionControl.Size = new Vector2(descriptionControl.Size.X, y);
        control1.PositionY = descriptionControl.Size.Y / 2f;
        control2.PositionY = (float) (-(double) descriptionControl.Size.Y / 2.0 + 0.00999999977648258);
        if (control2.Visible)
          control3.PositionY = (float) ((double) control2.PositionY + (double) control2.Size.Y + 0.0399999991059303);
        else
          control3.PositionY = (float) (-(double) descriptionControl.Size.Y / 2.0 + 0.0399999991059303);
      }
    }

    private void OnTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      if (this.m_medbaySelect_SuppressNext)
      {
        this.m_medbaySelect_SuppressNext = false;
      }
      else
      {
        if (this.m_respawnsTable.SelectedRow == this.m_previouslySelected)
          return;
        this.FocusedControl = (MyGuiControlBase) sender;
        this.m_previouslySelected = this.m_respawnsTable.SelectedRow;
        if (this.m_respawnsTable.SelectedRow != null)
        {
          this.m_respawnButton.Enabled = true;
          this.m_haveSelection = true;
          this.m_selectedRowData = this.m_respawnsTable.SelectedRow.UserData;
          this.m_isMultiplayerReady = false;
          this.m_showPreviewTime = MyGuiManager.TotalTimeInMilliseconds + 500;
        }
        else
        {
          this.m_haveSelection = false;
          this.m_selectedRowData = (object) null;
          this.m_respawnButton.Enabled = false;
          this.ShowEmptyPreview();
        }
      }
    }

    private void ShowPreview()
    {
      this.m_showPreviewTime = 0;
      if (this.m_selectedRowData is MySpaceRespawnComponent.MyRespawnPointInfo selectedRowData)
      {
        this.m_selectedRowIsStreamable = true;
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
        this.m_lastMedicalRoomId = 0L;
        this.m_isMultiplayerReady = false;
        this.ShowBlackground();
        MySession.RequestVicinityCache(selectedRowData.MedicalRoomGridId);
        if (!Sync.IsServer && MyEntities.EntityExists(selectedRowData.MedicalRoomGridId))
        {
          this.RequestConfirmation();
        }
        else
        {
          this.RequestReplicable(selectedRowData.MedicalRoomGridId);
          MyEntities.OnEntityAdd += new Action<MyEntity>(this.OnEntityStreamedIn);
        }
      }
      else if (this.m_selectedRowData is MyGuiScreenMedicals.MyPlanetInfo selectedRowData)
      {
        this.m_selectedRowIsStreamable = true;
        Vector3 directionToSunNormalized = MySector.DirectionToSunNormalized;
        BoundingSphereD fromBoundingBox = BoundingSphereD.CreateFromBoundingBox(selectedRowData.WorldAABB);
        double tmax;
        fromBoundingBox.IntersectRaySphere(new RayD(fromBoundingBox.Center, (Vector3D) directionToSunNormalized), out double _, out tmax);
        Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector((Vector3D) directionToSunNormalized);
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(fromBoundingBox.Center + (Vector3D) directionToSunNormalized * (tmax * 1.5)));
        MySpectatorCameraController.Static.SetTarget(fromBoundingBox.Center, new Vector3D?(perpendicularVector));
        this.m_isMultiplayerReady = true;
      }
      else
      {
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(new Vector3D(1000000.0)));
        this.ShowEmptyPreview();
      }
    }

    private void ShowEmptyPreview()
    {
      this.ShowBlackground();
      this.UnrequestReplicable();
      this.m_selectedRowIsStreamable = false;
    }

    private void RequestReplicable(long replicableId)
    {
      if (this.m_requestedReplicable == replicableId)
        return;
      this.UnrequestReplicable();
      this.m_requestedReplicable = replicableId;
      if (!(MyMultiplayer.ReplicationLayer is MyReplicationClient replicationLayer))
        return;
      replicationLayer.RequestReplicable(this.m_requestedReplicable, (byte) 0, true);
    }

    private void UnrequestReplicable()
    {
      if (this.m_requestedReplicable == 0L || !(MyMultiplayer.ReplicationLayer is MyReplicationClient replicationLayer))
        return;
      replicationLayer.RequestReplicable(this.m_requestedReplicable, (byte) 0, false);
      this.m_requestedReplicable = 0L;
    }

    private void OnTableItemDoubleClick(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      if (this.m_respawnsTable.SelectedRow == null)
        return;
      long? nullable = new long?();
      object userData = this.m_respawnsTable.SelectedRow.UserData;
      if (userData is MySpaceRespawnComponent.MyRespawnPointInfo respawnPointInfo)
        nullable = new long?(respawnPointInfo.MedicalRoomId);
      else if (userData is MyGuiScreenMedicals.MyPlanetInfo myPlanetInfo)
        nullable = new long?(myPlanetInfo.PlanetId);
      if (!nullable.HasValue || MyEntities.TryGetEntityById(nullable.Value, out MyEntity _))
      {
        this.onRespawnClick(this.m_respawnButton);
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionNotReady);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextNotReady), messageCaption: messageCaption, canHideOthers: false));
      }
    }

    private void OnFactionSelectClick(MyGuiControlButton sender)
    {
      if (this.m_factionsTable.SelectedRow == null)
        return;
      MyFaction userData = this.m_factionsTable.SelectedRow.UserData as MyFaction;
      if (this.m_applyingToFaction != null)
        MyFactionCollection.CancelJoinRequest(this.m_applyingToFaction.FactionId, MySession.Static.LocalPlayerId);
      this.m_applyingToFaction = userData;
      if (userData != null)
      {
        if (!userData.AcceptHumans || !userData.AutoAcceptMember && !userData.IsAnyLeaderOnline)
          return;
        MyFactionCollection.SendJoinRequest(userData.FactionId, MySession.Static.LocalPlayerId);
      }
      this.m_showFactions = false;
      this.RecreateControls(true);
    }

    private void OnPlayerJoinedFaction(MyFaction faction, long identityId)
    {
      if (identityId != MySession.Static.LocalPlayerId)
        return;
      this.m_showFactions = false;
      this.m_applyingToFaction = (MyFaction) null;
      this.RecreateControls(true);
      if (this.m_showFactions)
        return;
      this.m_backToFactionsButton.Enabled = false;
    }

    private void OnPlayerKickedFromFaction(MyFaction faction, long identityId)
    {
      if (identityId != MySession.Static.LocalPlayerId)
        return;
      this.m_showFactions = false;
      this.RecreateControls(true);
      if (this.m_showFactions)
        return;
      this.m_backToFactionsButton.Enabled = true;
    }

    private void OnPlayerRejected(MyFaction faction, long identityId)
    {
    }

    private void OnFactionsTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      this.RefreshSelectFactionButton();
    }

    private void RefreshSelectFactionButton()
    {
      if (this.m_factionsTable.SelectedRow == null)
      {
        this.m_selectFactionButton.Enabled = false;
      }
      else
      {
        MyFaction userData = this.m_factionsTable.SelectedRow.UserData as MyFaction;
        this.m_selectFactionButton.Enabled = userData == null || userData.AcceptHumans && (userData.AutoAcceptMember || userData.IsAnyLeaderOnline);
      }
    }

    private void OnFactionsTableItemDoubleClick(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      this.OnFactionSelectClick((MyGuiControlButton) null);
    }

    private void OnFactionsTableItemMouseOver(MyGuiControlTable.Row row)
    {
      this.m_factionsTable.TooltipDelay = 500;
      if (this.m_lastSelectedFactionRow != row)
      {
        this.m_lastSelectedFactionRow = row;
        this.m_factionsTable.HideToolTip();
      }
      this.m_factionsTable.SetToolTip(this.GetFactionTooltip(row.UserData as MyFaction));
    }

    private string GetFactionTooltip(MyFaction faction)
    {
      this.m_factionTooltip.Clear();
      if (faction != null)
      {
        bool flag = faction.Description != null && faction.Description != string.Empty;
        if (!faction.AcceptHumans)
        {
          this.m_factionTooltip.Append((object) MyTexts.Get(MySpaceTexts.ScreenMedicals_DoesNotAcceptPlayers));
          if (flag)
            this.m_factionTooltip.Append("\n");
        }
        else if (!faction.AutoAcceptMember)
        {
          this.m_factionTooltip.Append((object) MyTexts.Get(MySpaceTexts.ScreenMedicals_RequiresAcceptance));
          if (!faction.IsAnyLeaderOnline)
          {
            this.m_factionTooltip.Append("\n");
            this.m_factionTooltip.Append((object) MyTexts.Get(MySpaceTexts.ScreenMedicals_LeaderNotOnline));
          }
          if (flag)
            this.m_factionTooltip.Append("\n");
        }
        if (flag)
          this.m_factionTooltip.Append(faction.Description);
        DictionaryReader<long, MyFactionMember> members = faction.Members;
        if (members.Count > 0)
        {
          this.m_factionTooltip.Append("\n").Append("\n");
          this.m_factionTooltip.Append((object) MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Members));
          members = faction.Members;
          foreach (KeyValuePair<long, MyFactionMember> keyValuePair in members)
          {
            MyIdentity identity = MySession.Static.Players.TryGetIdentity(keyValuePair.Key);
            if (identity != null)
            {
              this.m_factionTooltip.Append("\n");
              this.m_factionTooltip.Append(identity.DisplayName);
              if (keyValuePair.Value.IsLeader)
                this.m_factionTooltip.Append(" (").Append((object) MyTexts.Get(MyCommonTexts.Leader)).Append(")");
            }
          }
        }
      }
      return this.m_factionTooltip.ToString();
    }

    private void OnShowPlayersClick(MyGuiControlButton sender)
    {
      if (MyMultiplayer.Static == null)
        return;
      MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
    }

    private void onRespawnClick(MyGuiControlButton sender)
    {
      if (this.m_respawnsTable.SelectedRow == null)
        return;
      object userData = this.m_respawnsTable.SelectedRow.UserData;
      if (userData == null)
      {
        this.CheckPermaDeathAndRespawn((Action) (() =>
        {
          this.RespawnImmediately((string) null, new long?());
          if (Sync.IsServer)
            return;
          this.RequestConfirmation();
        }));
      }
      else
      {
        MyRespawnShipDefinition respawnShip;
        if ((respawnShip = userData as MyRespawnShipDefinition) != null)
        {
          if (MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, respawnShip.Id.SubtypeName) != 0)
            return;
          this.CheckPermaDeathAndRespawn((Action) (() =>
          {
            this.RespawnShip(respawnShip.Id.SubtypeName, (MyGuiScreenMedicals.MyPlanetInfo) null);
            if (Sync.IsServer)
              return;
            this.RequestConfirmation();
          }));
        }
        else
        {
          MyGuiScreenMedicals.MyPlanetInfo planetInfo;
          if ((planetInfo = userData as MyGuiScreenMedicals.MyPlanetInfo) != null)
          {
            if (MySpaceRespawnComponent.Static.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, planetInfo.RespawnShipForCooldownCheck) != 0)
              return;
            this.CheckPermaDeathAndRespawn((Action) (() =>
            {
              this.RespawnShip(planetInfo.RespawnShipForCooldownCheck, planetInfo);
              this.ShowBlackground();
              if (!Sync.IsServer)
                this.RequestConfirmation();
              else
                MySandboxGame.AreClipmapsReady = false;
            }));
          }
          else
          {
            if (this.m_restrictedRespawn != 0L && this.m_restrictedRespawn != ((MySpaceRespawnComponent.MyRespawnPointInfo) this.m_respawnsTable.SelectedRow.UserData).MedicalRoomId)
              return;
            this.CheckPermaDeathAndRespawn((Action) (() => this.RespawnAtMedicalRoom(((MySpaceRespawnComponent.MyRespawnPointInfo) this.m_respawnsTable.SelectedRow.UserData).MedicalRoomId)));
          }
        }
      }
    }

    private void CheckPermaDeathAndRespawn(Action respawnAction)
    {
      MyIdentity identity = Sync.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
      if (identity == null)
        return;
      if (MySession.Static.Settings.PermanentDeath.Value && identity.FirstSpawnDone)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.MessageBoxCaptionRespawn), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
        {
          if (retval != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          respawnAction();
        }))));
      }
      else
        respawnAction();
    }

    private void RespawnShip(string shipPrefabId, MyGuiScreenMedicals.MyPlanetInfo planetInfo)
    {
      MySpaceRespawnComponent respawnComponent = MySpaceRespawnComponent.Static;
      int num = MySession.Static.LocalHumanPlayer == null ? 0 : respawnComponent.GetRespawnCooldownSeconds(MySession.Static.LocalHumanPlayer.Id, shipPrefabId);
      if (respawnComponent.IsSynced && num == 0)
      {
        if (planetInfo == null)
          this.RespawnImmediately(shipPrefabId, new long?());
        else
          this.RespawnImmediately((string) null, new long?(planetInfo.PlanetId));
      }
      else
      {
        this.m_selectedRespawn = (object) planetInfo ?? (object) MyDefinitionManager.Static.GetRespawnShipDefinition(shipPrefabId);
        this.UpdateRespawnShipLabel();
      }
    }

    private void RespawnAtMedicalRoom(long medicalId)
    {
      string model = (string) null;
      Color red = Color.Red;
      MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref red);
      MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, false, medicalId, (string) null, 0, model, red);
      this.m_respawning = true;
      this.m_respawnButton.Visible = this.m_respawnButtonVisible = false;
      this.m_respawnsTable.Enabled = false;
    }

    private void RespawnImmediately(string shipPrefabId, long? planetId)
    {
      MyIdentity identity = Sync.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
      bool newIdentity = identity == null || identity.FirstSpawnDone;
      if (Sync.IsServer && (!string.IsNullOrEmpty(shipPrefabId) || planetId.HasValue))
      {
        this.m_waitingForRespawnShip = true;
        MySpaceRespawnComponent.Static.RespawnDoneEvent += new Action<ulong>(this.RespawnShipDoneEvent);
        MyPlayerCollection.OnRespawnRequestFailureEvent += new Action<ulong>(this.RespawnShipDoneEvent);
      }
      else
        this.m_waitingForRespawnShip = false;
      string model = (string) null;
      Color red = Color.Red;
      MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref red);
      MyPlayerCollection.RespawnRequest(MySession.Static.LocalCharacter == null, newIdentity, planetId ?? 0L, shipPrefabId, 0, model, red);
      this.m_respawning = true;
      this.m_respawnButton.Visible = this.m_respawnButtonVisible = false;
      this.m_respawnsTable.Enabled = false;
    }

    private void RespawnShipDoneEvent(ulong steamId)
    {
      this.m_waitingForRespawnShip = false;
      MySpaceRespawnComponent.Static.RespawnDoneEvent -= new Action<ulong>(this.RespawnShipDoneEvent);
      MyPlayerCollection.OnRespawnRequestFailureEvent -= new Action<ulong>(this.RespawnShipDoneEvent);
    }

    private void OnRefreshClick(MyGuiControlButton sender) => this.RefreshRespawnPoints(true);

    private void OnFactionsRefreshClick(MyGuiControlButton sender) => this.RefreshFactions();

    private void OnBackToFactionsClick(MyGuiControlButton sender)
    {
      this.m_showFactions = true;
      this.RecreateControls(true);
    }

    private void onMotdClick(MyGuiControlButton sender)
    {
      this.m_hideEmptyMotD = false;
      this.m_showMotD = !this.m_isMotdOpen && !Sandbox.Engine.Platform.Game.IsDedicated;
      this.m_refocusMotDButton = true;
      this.RecreateControls(false);
    }

    public void SetMotD(string motd)
    {
      this.m_lastMotD = motd;
      if (this.m_motdMultiline != null)
        this.m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(this.m_lastMotD));
      if (string.IsNullOrEmpty(this.m_lastMotD) || Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.m_showMotD = false;
        this.m_MotdButton.Enabled = false;
      }
      else
        this.m_MotdButton.Enabled = true;
      if (!this.m_showMotD)
        return;
      this.RecreateControls(false);
    }

    public void ShowBlackground()
    {
      this.m_blackgroundCounter = 5;
      this.m_blackgroundDrawFull = true;
      this.m_blackgroundFade = 1f;
      this.m_streamingTimeout = 120;
    }

    internal static void ShowMotDUrl(string url)
    {
      if (!MySession.ShowMotD)
        return;
      MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
    }

    private void AfterLocalizationLoaded()
    {
      if (this.m_motdMultiline == null)
        return;
      this.m_motdMultiline.Text = new StringBuilder(MyTexts.SubstituteTexts(this.m_lastMotD));
    }

    private void OnEntityStreamedIn(MyEntity entity)
    {
      if (entity.EntityId != this.m_requestedReplicable)
        return;
      this.RequestConfirmation();
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.OnEntityStreamedIn);
    }

    private void RequestConfirmation()
    {
      this.m_isMultiplayerReady = false;
      (MyMultiplayer.Static as MyMultiplayerClientBase).RequestBatchConfirmation();
      MyMultiplayer.Static.PendingReplicablesDone += new Action(this.OnPendingReplicablesDone);
    }

    private void OnPendingReplicablesDone()
    {
      this.m_isMultiplayerReady = true;
      MyMultiplayer.Static.PendingReplicablesDone -= new Action(this.OnPendingReplicablesDone);
      if (MySession.Static.VoxelMaps.Instances.Count <= 0)
        return;
      MySandboxGame.AreClipmapsReady = false;
    }

    [Serializable]
    private class MyPlanetInfo
    {
      public long PlanetId;
      public string PlanetName;
      public BoundingBoxD WorldAABB;
      public float Gravity;
      public float OxygenLevel;
      public string Difficulty;
      [Nullable]
      public string DropPodForDetail;
      public string RespawnShipForCooldownCheck;

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EPlanetId\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in long value) => owner.PlanetId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out long value) => value = owner.PlanetId;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EPlanetName\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in string value) => owner.PlanetName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out string value) => value = owner.PlanetName;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EWorldAABB\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, BoundingBoxD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in BoundingBoxD value) => owner.WorldAABB = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out BoundingBoxD value) => value = owner.WorldAABB;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EGravity\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in float value) => owner.Gravity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out float value) => value = owner.Gravity;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EOxygenLevel\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in float value) => owner.OxygenLevel = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out float value) => value = owner.OxygenLevel;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EDifficulty\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in string value) => owner.Difficulty = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out string value) => value = owner.Difficulty;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003EDropPodForDetail\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in string value) => owner.DropPodForDetail = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out string value) => value = owner.DropPodForDetail;
      }

      protected class SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u003ERespawnShipForCooldownCheck\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenMedicals.MyPlanetInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenMedicals.MyPlanetInfo owner, in string value) => owner.RespawnShipForCooldownCheck = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenMedicals.MyPlanetInfo owner, out string value) => value = owner.RespawnShipForCooldownCheck;
      }
    }

    protected sealed class RefreshRespawnPointsRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenMedicals.RefreshRespawnPointsRequest();
      }
    }

    protected sealed class RequestRespawnPointsResponse\u003C\u003ESystem_Collections_Generic_List`1\u003CSpaceEngineers_Game_World_MySpaceRespawnComponent\u003C\u003EMyRespawnPointInfo\u003E\u0023SpaceEngineers_Game_GUI_MyGuiScreenMedicals\u003C\u003EMyPlanetInfo\u003C\u0023\u003E : ICallSite<IMyEventOwner, List<MySpaceRespawnComponent.MyRespawnPointInfo>, MyGuiScreenMedicals.MyPlanetInfo[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MySpaceRespawnComponent.MyRespawnPointInfo> medicalRooms,
        in MyGuiScreenMedicals.MyPlanetInfo[] planetInfos,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenMedicals.RequestRespawnPointsResponse(medicalRooms, planetInfos);
      }
    }
  }
}
