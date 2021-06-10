// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenNewGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenNewGame : MyGuiScreenBase
  {
    private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;
    private MyGuiControlList m_campaignList;
    private MyGuiControlRadioButtonGroup m_campaignTypesGroup;
    private MyObjectBuilder_Campaign m_selectedCampaign;
    private MyGuiControlContentButton m_selectedButton;
    private MyLayoutTable m_tableLayout;
    private MyGuiControlLabel m_nameLabel;
    private MyGuiControlLabel m_nameText;
    private MyGuiControlLabel m_onlineModeLabel;
    private MyGuiControlCombobox m_onlineMode;
    private MyGuiControlSlider m_maxPlayersSlider;
    private MyGuiControlLabel m_maxPlayersLabel;
    private MyGuiControlLabel m_authorLabel;
    private MyGuiControlLabel m_authorText;
    private MyGuiControlLabel m_ratingLabel;
    private MyGuiControlRating m_ratingDisplay;
    private MyGuiControlMultilineText m_descriptionMultilineText;
    private MyGuiControlPanel m_descriptionPanel;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_publishButton;
    private MyGuiControlRotatingWheel m_asyncLoadingWheel;
    private Task m_refreshTask;
    private MyGuiControlLabel m_workshopError;
    private float MARGIN_TOP = 0.22f;
    private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
    private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private bool m_displayTabScenario;
    private bool m_displayTabWorkshop;
    private bool m_displayTabCustom;
    private int m_maxPlayers;
    private bool m_parallelLoadIsRunning;
    private bool m_workshopPermitted;

    public MyGuiScreenNewGame(
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.878f, 0.97f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_workshopPermitted = true;
      MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (x => this.m_workshopPermitted = x));
      this.m_displayTabScenario = displayTabScenario;
      this.m_displayTabWorkshop = displayTabWorkshop;
      this.m_displayTabCustom = displayTabCustom;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => "New Game";

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.379999995231628 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.625f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_workshopError = new MyGuiControlLabel(colorMask: new Vector4?((Vector4) Color.Red));
      this.m_workshopError.Position = new Vector2(-0.382f, 0.46f);
      this.m_workshopError.Visible = false;
      this.Controls.Add((MyGuiControlBase) this.m_workshopError);
      this.m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel((MyGuiScreenBase) this, MyTexts.Get(MyCommonTexts.NewGameScreen_Description), this.m_displayTabScenario, this.m_displayTabWorkshop, this.m_displayTabCustom);
      this.InitCampaignList();
      this.InitRightSide();
      this.m_refreshTask = MyCampaignManager.Static.RefreshModData();
      this.m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2((float) ((double) this.m_size.Value.X / 2.0 - 0.0769999995827675), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.108000002801418))), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
      this.Controls.Add((MyGuiControlBase) this.m_asyncLoadingWheel);
      this.CheckUGCServices();
    }

    public void SetWorkshopErrorText(string text = "", bool visible = true, bool skipUGCCheck = false)
    {
      if (!skipUGCCheck && string.IsNullOrEmpty(text))
      {
        this.CheckUGCServices();
      }
      else
      {
        this.m_workshopError.Text = text;
        this.m_workshopError.Visible = visible;
      }
    }

    public override bool RegisterClicks() => true;

    public override bool Update(bool hasFocus)
    {
      this.m_publishButton.Visible = this.m_selectedCampaign != null && this.m_selectedCampaign.IsLocalMod && !MyInput.Static.IsJoystickLastUsed;
      if (this.m_refreshTask.valid && this.m_refreshTask.IsComplete)
      {
        this.m_refreshTask.valid = false;
        this.m_asyncLoadingWheel.Visible = false;
        this.RefreshCampaignList();
      }
      else if (this.FocusedControl == null)
        this.FocusedControl = this.m_screenSwitchPanel.Controls.GetControlByName("CampaignButton");
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    private void CheckUGCServices()
    {
      string str = "";
      foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
      {
        if (!aggregate.IsConsentGiven)
          str = aggregate.ServiceName;
      }
      if (str != "")
        this.SetWorkshopErrorText(str + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent));
      else
        this.SetWorkshopErrorText(visible: false, skipUGCCheck: true);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.StartSelectedWorld();
      if (this.m_selectedCampaign == null || !this.m_selectedCampaign.IsLocalMod || !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        return;
      this.OnPublishButtonOnClick((MyGuiControlButton) null);
    }

    private void InitCampaignList()
    {
      Vector2 vector2 = -this.m_size.Value / 2f + new Vector2(this.MARGIN_LEFT_LIST, this.MARGIN_TOP);
      this.m_campaignTypesGroup = new MyGuiControlRadioButtonGroup();
      this.m_campaignTypesGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.CampaignSelectionChanged);
      this.m_campaignTypesGroup.MouseDoubleClick += new Action<MyGuiControlRadioButton>(this.CampaignDoubleClick);
      MyGuiControlList myGuiControlList = new MyGuiControlList();
      myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlList.Position = vector2;
      myGuiControlList.Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, (float) ((double) this.m_size.Value.Y - (double) this.MARGIN_TOP - 0.0480000004172325));
      this.m_campaignList = myGuiControlList;
      this.Controls.Add((MyGuiControlBase) this.m_campaignList);
    }

    private void CampaignSelectionChanged(MyGuiControlRadioButtonGroup args)
    {
      if (!(args.SelectedButton is MyGuiControlContentButton selectedButton) || !(selectedButton.UserData is MyObjectBuilder_Campaign userData))
        return;
      MyCampaignManager.Static.ReloadMenuLocalization(string.IsNullOrEmpty(userData.ModFolderPath) ? userData.Name : Path.Combine(userData.ModFolderPath, userData.Name));
      string contextName = (string) null;
      MyLocalizationContext localizationContext = (MyLocalizationContext) null;
      if (string.IsNullOrEmpty(userData.DescriptionLocalizationFile))
      {
        localizationContext = MyLocalization.Static[userData.Name];
      }
      else
      {
        Dictionary<string, string> contextTranslator = MyLocalization.Static.PathToContextTranslator;
        string key = string.IsNullOrEmpty(userData.ModFolderPath) ? Path.Combine(MyFileSystem.ContentPath, userData.DescriptionLocalizationFile) : Path.Combine(userData.ModFolderPath, userData.DescriptionLocalizationFile);
        if (contextTranslator.ContainsKey(key))
          contextName = contextTranslator[key];
        if (!string.IsNullOrEmpty(contextName))
          localizationContext = MyLocalization.Static[contextName];
      }
      this.m_descriptionMultilineText.Text = (StringBuilder) null;
      if (localizationContext != null)
      {
        StringBuilder stringBuilder = localizationContext["Name"];
        this.m_nameText.Text = stringBuilder == null ? "name" : stringBuilder.ToString();
        this.m_descriptionMultilineText.Text = localizationContext["Description"];
      }
      if (this.m_descriptionMultilineText.Text == null || this.m_descriptionMultilineText.Text != null && string.IsNullOrEmpty(this.m_descriptionMultilineText.Text.ToString()))
      {
        this.m_nameText.Text = userData.Name;
        this.m_descriptionMultilineText.Text = new StringBuilder(userData.Description);
      }
      this.m_maxPlayers = 0;
      if (userData != null && userData.IsMultiplayer)
      {
        int val2 = userData.MaxPlayers;
        this.m_onlineMode.Enabled = true;
        if (!Sync.IsDedicated)
        {
          string platform = MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : (string) null;
          MyObjectBuilder_CampaignSM stateMachine = userData.GetStateMachine(platform);
          int num = MySandboxGame.Config.ExperimentalMode ? stateMachine.MaxLobbyPlayersExperimental : stateMachine.MaxLobbyPlayers;
          if (stateMachine != null && num > 0)
          {
            if (num == 1)
              this.m_onlineMode.Enabled = false;
            else
              val2 = num;
          }
        }
        this.m_maxPlayers = Math.Min(MyMultiplayerLobby.MAX_PLAYERS, val2);
        this.m_maxPlayersSlider.MaxValue = (float) Math.Max(this.m_maxPlayers, 3);
        this.m_maxPlayersSlider.Value = (float) this.m_maxPlayers;
        this.FillOnlineMode(userData.IsOfflineEnabled);
        this.m_onlineMode.SelectItemByIndex(0);
      }
      else
      {
        this.m_onlineMode.Enabled = false;
        this.m_onlineMode.SelectItemByIndex(0);
      }
      this.m_authorText.Text = userData.Author;
      this.m_maxPlayersSlider.Enabled = this.m_onlineMode.Enabled && this.m_onlineMode.GetSelectedIndex() > 0 && this.m_maxPlayers > 2;
      uint id = 0;
      if (!string.IsNullOrEmpty(userData.DLC))
      {
        uint appId = MyDLCs.GetDLC(userData.DLC).AppId;
        if (!MyGameService.IsDlcInstalled(appId) || (long) Sync.MyId != (long) MyGameService.UserId)
          id = appId;
      }
      if (id != 0U)
      {
        this.m_okButton.Text = MyTexts.GetString(MyCommonTexts.VisitStore);
        this.m_okButton.SetToolTip(MyDLCs.GetRequiredDLCStoreHint(id));
      }
      else
      {
        this.m_okButton.Text = MyTexts.GetString(MyCommonTexts.Start);
        this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
      }
      this.m_selectedCampaign = userData;
      if (this.m_selectedButton != null)
        this.m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_selectedButton = selectedButton;
      this.m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
    }

    private void InitRightSide()
    {
      int num1 = 5;
      Vector2 topLeft = -this.m_size.Value / 2f + new Vector2((float) ((double) this.MARGIN_LEFT_LIST + (double) this.m_campaignList.Size.X + (double) this.MARGIN_LEFT_INFO + 0.0120000001043081), this.MARGIN_TOP - 11f / 1000f);
      Vector2 vector2_1 = this.m_size.Value;
      Vector2 size = new Vector2(vector2_1.X / 2f - topLeft.X, (float) ((double) vector2_1.Y - (double) this.MARGIN_TOP - (double) this.MARGIN_BOTTOM - 0.0344999991357327)) - new Vector2(this.MARGIN_RIGHT, 0.12f);
      float num2 = size.X * 0.6f;
      float num3 = size.X - num2;
      float num4 = 0.052f;
      float num5 = size.Y - (float) num1 * num4;
      this.m_tableLayout = new MyLayoutTable((IMyGuiControlsParent) this, topLeft, size);
      this.m_tableLayout.SetColumnWidthsNormalized(num2 - 0.055f, num3 + 0.055f);
      this.m_tableLayout.SetRowHeightsNormalized(num4, num4, num4, num4, num4, num5);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.Name);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_nameLabel = myGuiControlLabel1;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_nameText = myGuiControlLabel2;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Author);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_authorLabel = myGuiControlLabel3;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_authorText = myGuiControlLabel4;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Rating);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_ratingLabel = myGuiControlLabel5;
      MyGuiControlRating guiControlRating = new MyGuiControlRating(10);
      guiControlRating.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_ratingDisplay = guiControlRating;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_OnlineMode);
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_onlineModeLabel = myGuiControlLabel6;
      MyGuiControlCombobox guiControlCombobox = new MyGuiControlCombobox();
      guiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_onlineMode = guiControlCombobox;
      this.FillOnlineMode();
      this.m_onlineMode.SelectItemByIndex(0);
      this.m_onlineMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_onlineMode_ItemSelected);
      this.m_onlineMode.Enabled = false;
      this.m_maxPlayers = MyMultiplayerLobby.MAX_PLAYERS;
      Vector2? position = new Vector2?(Vector2.Zero);
      float x = this.m_onlineMode.Size.X;
      double num6 = (double) Math.Max(this.m_maxPlayers, 3);
      double num7 = (double) x;
      float? defaultValue = new float?();
      Vector4? color = new Vector4?();
      string labelText = new StringBuilder("{0}").ToString();
      this.m_maxPlayersSlider = new MyGuiControlSlider(position, 2f, (float) num6, (float) num7, defaultValue, color, labelText, 0, labelSpaceWidth: 0.028f, intValue: true, showLabel: true);
      this.m_maxPlayersSlider.Value = (float) this.m_maxPlayers;
      this.m_maxPlayersLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.MaxPlayers));
      this.m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(), new Vector2?(), new Vector4?(), "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, (StringBuilder) null, true, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new int?(), false, false, (MyGuiCompositeTexture) null, new MyGuiBorderThickness?());
      controlMultilineText.Name = "BriefingMultilineText";
      controlMultilineText.Position = new Vector2(-0.009f, -0.115f);
      controlMultilineText.Size = new Vector2(0.419f, 0.412f);
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_descriptionMultilineText = controlMultilineText;
      MyGuiControlCompositePanel controlCompositePanel = new MyGuiControlCompositePanel();
      controlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.m_descriptionPanel = (MyGuiControlPanel) controlCompositePanel;
      this.m_tableLayout.Add((MyGuiControlBase) this.m_nameLabel, MyAlignH.Left, MyAlignV.Center, 0, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_authorLabel, MyAlignH.Left, MyAlignV.Center, 1, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_onlineModeLabel, MyAlignH.Left, MyAlignV.Center, 2, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_maxPlayersLabel, MyAlignH.Left, MyAlignV.Center, 3, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_ratingLabel, MyAlignH.Left, MyAlignV.Center, 4, 0);
      this.m_nameLabel.PositionX -= 3f / 1000f;
      this.m_authorLabel.PositionX -= 3f / 1000f;
      this.m_onlineModeLabel.PositionX -= 3f / 1000f;
      this.m_maxPlayersLabel.PositionX -= 3f / 1000f;
      this.m_ratingLabel.PositionX -= 3f / 1000f;
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_nameText, MyAlignH.Left, MyAlignV.Center, 0, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_authorText, MyAlignH.Left, MyAlignV.Center, 1, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_onlineMode, MyAlignH.Left, MyAlignV.Center, 2, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_maxPlayersSlider, MyAlignH.Left, MyAlignV.Center, 3, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_ratingDisplay, MyAlignH.Left, MyAlignV.Center, 4, 1);
      this.m_nameText.PositionX -= 1f / 1000f;
      MyGuiControlLabel nameText = this.m_nameText;
      nameText.Size = nameText.Size + new Vector2(1f / 500f, 0.0f);
      this.m_onlineMode.PositionX -= 1f / 500f;
      this.m_onlineMode.PositionY -= 0.005f;
      this.m_maxPlayersSlider.PositionX -= 3f / 1000f;
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_descriptionPanel, MyAlignH.Left, MyAlignV.Top, 5, 0, colSpan: 2);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_descriptionMultilineText, MyAlignH.Left, MyAlignV.Top, 5, 0, colSpan: 2);
      this.m_descriptionMultilineText.PositionY += 0.012f;
      float num8 = 0.01f;
      this.m_descriptionPanel.Position = new Vector2(this.m_descriptionPanel.PositionX - num8, (float) ((double) this.m_descriptionPanel.PositionY - (double) num8 + 0.0120000001043081));
      this.m_descriptionPanel.Size = new Vector2(this.m_descriptionPanel.Size.X + num8, (float) ((double) this.m_descriptionPanel.Size.Y + (double) num8 * 2.0 - 0.0120000001043081));
      Vector2 vector2_2 = this.m_size.Value / 2f;
      vector2_2.X -= this.MARGIN_RIGHT + 0.004f;
      vector2_2.Y -= this.MARGIN_BOTTOM + 0.004f;
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      Vector2 genericButtonSpacing1 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      Vector2 genericButtonSpacing2 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      this.m_okButton = new MyGuiControlButton(new Vector2?(vector2_2), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Start), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClicked));
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_publishButton = new MyGuiControlButton(new Vector2?(vector2_2 - new Vector2(backButtonSize.X + 0.0245f, 0.0f)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), onButtonClick: new Action<MyGuiControlButton>(this.OnPublishButtonOnClick));
      this.m_publishButton.Visible = true;
      this.m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
      this.m_descriptionPanel.Size = new Vector2(this.m_descriptionPanel.Size.X, this.m_descriptionPanel.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
      this.m_descriptionMultilineText.Size = new Vector2(this.m_descriptionMultilineText.Size.X, this.m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
      this.Controls.Add((MyGuiControlBase) this.m_publishButton);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.CloseButtonEnabled = true;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_nameLabel.Position.X, this.m_okButton.Position.Y - backButtonSize.Y / 2f)));
      myGuiControlLabel7.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.NewGameScenarios_Help_Screen);
    }

    private void FillOnlineMode(bool isOfflineEnabled = true)
    {
      this.m_onlineMode.ClearItems();
      if (isOfflineEnabled)
        this.m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
      this.m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
      this.m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
      this.m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
    }

    private void m_onlineMode_ItemSelected() => this.m_maxPlayersSlider.Enabled = this.m_onlineMode.Enabled && this.m_onlineMode.GetSelectedIndex() > 0 && this.m_maxPlayers > 2;

    private void OnPublishButtonOnClick(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_selectedCampaign == null)
        return;
      OnPublishConsent();

      void OnPublishConsent()
      {
        MyCampaignManager.Static.SwitchCampaign(this.m_selectedCampaign.Name, this.m_selectedCampaign.IsVanilla, this.m_selectedCampaign.PublishedFileId, this.m_selectedCampaign.PublishedServiceName, this.m_selectedCampaign.ModFolderPath);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorkshopTags("campaign", MyWorkshop.ScenarioCategories, (string[]) null, new Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]>(this.OnPublishWorkshopTagsResult)));
      }
    }

    private void OnPublishWorkshopTagsResult(
      MyGuiScreenMessageBox.ResultEnum result,
      string[] outTags,
      string[] serviceNames)
    {
      if (result != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MyCampaignManager.Static.PublishActive(outTags, serviceNames);
    }

    private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_parallelLoadIsRunning)
        return;
      this.m_parallelLoadIsRunning = true;
      MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
      MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
      Parallel.StartBackground((Action) (() => this.StartSelectedWorld()), (Action) (() =>
      {
        progressScreen.CloseScreen();
        this.m_parallelLoadIsRunning = false;
      }));
    }

    private void CampaignDoubleClick(MyGuiControlRadioButton obj)
    {
      if (this.m_parallelLoadIsRunning)
        return;
      this.m_parallelLoadIsRunning = true;
      MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
      MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
      Parallel.StartBackground((Action) (() => this.StartSelectedWorld()), (Action) (() =>
      {
        progressScreen.CloseScreen();
        this.m_parallelLoadIsRunning = false;
      }));
    }

    private void StartSelectedWorld()
    {
      MyObjectBuilder_Campaign campaignToStart = this.m_selectedCampaign;
      if (MyInput.Static.IsJoystickLastUsed && this.FocusedControl != null && (this.FocusedControl.GetType() == typeof (MyGuiControlContentButton) && this.FocusedControl.UserData != null) && this.FocusedControl.UserData.GetType() == typeof (MyObjectBuilder_Campaign))
        campaignToStart = this.FocusedControl.UserData as MyObjectBuilder_Campaign;
      if (campaignToStart == null)
        return;
      uint num = 0;
      if (!string.IsNullOrEmpty(campaignToStart.DLC))
      {
        uint appId = MyDLCs.GetDLC(campaignToStart.DLC).AppId;
        if (!MyGameService.IsDlcInstalled(appId) || (long) Sync.MyId != (long) MyGameService.UserId)
          num = appId;
      }
      if (num != 0U)
      {
        MyGameService.OpenDlcInShop(MyDLCs.GetDLC(campaignToStart.DLC).AppId);
      }
      else
      {
        MyCampaignManager.Static.SwitchCampaign(campaignToStart.Name, campaignToStart.IsVanilla, campaignToStart.PublishedFileId, campaignToStart.PublishedServiceName, campaignToStart.ModFolderPath, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : (string) null);
        MyOnlineModeEnum onlineMode = (MyOnlineModeEnum) this.m_onlineMode.GetSelectedKey();
        int maxPlayers = (int) this.m_maxPlayersSlider.Value;
        if (onlineMode != MyOnlineModeEnum.OFFLINE)
          MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
          {
            switch (granted)
            {
              case PermissionResult.Granted:
                MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
                {
                  switch (ugcGranted)
                  {
                    case PermissionResult.Granted:
                      Run();
                      break;
                    case PermissionResult.Error:
                      MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                      break;
                  }
                }));
                break;
              case PermissionResult.Error:
                MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                break;
            }
          }));
        else
          Run();

        void Run()
        {
          MyStringId errorMessage;
          if (!MyCloudHelper.IsError(MyCampaignManager.Static.RunNewCampaign(campaignToStart.Name, onlineMode, maxPlayers, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : (string) null), out errorMessage))
            return;
          MySandboxGame.Static.Invoke((Action) (() =>
          {
            MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
            messageBox.SkipTransition = true;
            messageBox.InstantClose = false;
            MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
          }), "New Game screen");
        }
      }
    }

    private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
    {
      base.OnScreenOrderChanged(oldLast, newLast);
      this.CheckUGCServices();
    }

    private void RefreshCampaignList()
    {
      (MyGameServiceCallResult, string) subscribedModDataResult = MyCampaignManager.Static.RefreshSubscribedModDataResult;
      if (subscribedModDataResult.Item1 != MyGameServiceCallResult.OK)
        this.SetWorkshopErrorText(MyWorkshop.GetWorkshopErrorText(subscribedModDataResult.Item1, subscribedModDataResult.Item2, this.m_workshopPermitted));
      else
        this.SetWorkshopErrorText(visible: false);
      List<MyObjectBuilder_Campaign> list = MyCampaignManager.Static.Campaigns.ToList<MyObjectBuilder_Campaign>();
      List<MyObjectBuilder_Campaign> objectBuilderCampaignList1 = new List<MyObjectBuilder_Campaign>();
      List<MyObjectBuilder_Campaign> objectBuilderCampaignList2 = new List<MyObjectBuilder_Campaign>();
      List<MyObjectBuilder_Campaign> objectBuilderCampaignList3 = new List<MyObjectBuilder_Campaign>();
      List<MyObjectBuilder_Campaign> objectBuilderCampaignList4 = new List<MyObjectBuilder_Campaign>();
      foreach (MyObjectBuilder_Campaign objectBuilderCampaign in list.OrderBy<MyObjectBuilder_Campaign, int>((Func<MyObjectBuilder_Campaign, int>) (x => x.Order)).ToList<MyObjectBuilder_Campaign>())
      {
        if (objectBuilderCampaign.IsVanilla && !objectBuilderCampaign.IsDebug)
          objectBuilderCampaignList1.Add(objectBuilderCampaign);
        else if (objectBuilderCampaign.IsLocalMod)
          objectBuilderCampaignList2.Add(objectBuilderCampaign);
        else if (objectBuilderCampaign.IsVanilla && objectBuilderCampaign.IsDebug)
          objectBuilderCampaignList4.Add(objectBuilderCampaign);
        else
          objectBuilderCampaignList3.Add(objectBuilderCampaign);
      }
      this.m_campaignList.Controls.Clear();
      this.m_campaignTypesGroup.Clear();
      foreach (MyObjectBuilder_Campaign campaign in objectBuilderCampaignList1)
        this.AddCampaignButton(campaign);
      if (MySandboxGame.Config.ExperimentalMode && !MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        if (objectBuilderCampaignList3.Count > 0)
          this.AddSeparator(MyTexts.Get(MyCommonTexts.Workshop).ToString());
        foreach (MyObjectBuilder_Campaign campaign in objectBuilderCampaignList3)
          this.AddCampaignButton(campaign);
        if (objectBuilderCampaignList2.Count > 0)
          this.AddSeparator(MyTexts.Get(MyCommonTexts.Local).ToString());
        foreach (MyObjectBuilder_Campaign campaign in objectBuilderCampaignList2)
          this.AddCampaignButton(campaign, true);
      }
      if (this.m_campaignList.Controls.Count > 0)
      {
        this.m_campaignTypesGroup.SelectByIndex(0);
        this.FocusedControl = this.m_campaignList.Controls[0];
      }
      this.CheckUGCServices();
    }

    private void AddCampaignButton(MyObjectBuilder_Campaign campaign, bool isLocalMod = false)
    {
      string dlcIcon = (string) null;
      if (!string.IsNullOrEmpty(campaign.DLC))
      {
        MyDLCs.MyDLC dlc = MyDLCs.GetDLC(campaign.DLC);
        if (dlc == null)
          return;
        dlcIcon = dlc.Icon;
      }
      string name = campaign.Name;
      MyLocalizationContext localizationContext = MyLocalization.Static[campaign.Name];
      if (localizationContext != null)
      {
        StringBuilder stringBuilder = localizationContext["Name"];
        if (stringBuilder != null)
          name = stringBuilder.ToString();
      }
      MyGuiControlContentButton controlContentButton1 = new MyGuiControlContentButton(name, this.GetImagePath(campaign), dlcIcon);
      controlContentButton1.UserData = (object) campaign;
      controlContentButton1.Key = this.m_campaignTypesGroup.Count;
      MyGuiControlContentButton controlContentButton2 = controlContentButton1;
      controlContentButton2.FocusHighlightAlsoSelects = true;
      controlContentButton2.SetModType(isLocalMod ? MyBlueprintTypeEnum.LOCAL : (campaign.IsVanilla ? MyBlueprintTypeEnum.DEFAULT : MyBlueprintTypeEnum.WORKSHOP), campaign.PublishedServiceName);
      if (!string.IsNullOrEmpty(campaign.DLC))
      {
        if (MyGameService.IsDlcInstalled(MyDLCs.GetDLC(campaign.DLC).AppId) && (long) Sync.MyId == (long) MyGameService.UserId)
          controlContentButton2.ColorMask = new Vector4(1f);
        else
          controlContentButton2.ColorMask = new Vector4(0.5f);
      }
      controlContentButton2.HighlightChanged += new Action<MyGuiControlBase>(this.Button_HighlightChanged);
      this.m_campaignTypesGroup.Add((MyGuiControlRadioButton) controlContentButton2);
      this.m_campaignList.Controls.Add((MyGuiControlBase) controlContentButton2);
      if (campaign == null || !campaign.IsLocalMod)
        return;
      controlContentButton2.GamepadHelpTextId = MySpaceTexts.NewGameScenarios_Help_ScenarioWithPublish;
    }

    private void Button_HighlightChanged(MyGuiControlBase obj)
    {
    }

    private string GetImagePath(MyObjectBuilder_Campaign campaign)
    {
      string path = campaign.ImagePath;
      if (string.IsNullOrEmpty(campaign.ImagePath))
        return string.Empty;
      if (!campaign.IsVanilla)
      {
        path = campaign.ModFolderPath != null ? Path.Combine(campaign.ModFolderPath, campaign.ImagePath) : string.Empty;
        if (!MyFileSystem.FileExists(path))
          path = Path.Combine(MyFileSystem.ContentPath, campaign.ImagePath);
      }
      return path;
    }

    private void AddSeparator(string sectionName)
    {
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      controlCompositePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlCompositePanel1.Position = Vector2.Zero;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = sectionName;
      myGuiControlLabel1.Font = "Blue";
      myGuiControlLabel1.PositionX = 0.005f;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      float num = 3f / 1000f;
      Color themedGuiLineColor = MyGuiConstants.THEMED_GUI_LINE_COLOR;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(textures: new string[1]
      {
        "Textures\\GUI\\FogSmall3.dds"
      });
      myGuiControlImage1.Size = new Vector2(myGuiControlLabel2.Size.X + num * 10f, 0.007f);
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage1.ColorMask = themedGuiLineColor.ToVector4();
      myGuiControlImage1.Position = new Vector2(-num, myGuiControlLabel2.Size.Y);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
      guiControlParent1.Size = new Vector2(this.m_campaignList.Size.X, myGuiControlLabel2.Size.Y);
      guiControlParent1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlParent1.Position = Vector2.Zero;
      MyGuiControlParent guiControlParent2 = guiControlParent1;
      controlCompositePanel2.Size = guiControlParent2.Size + new Vector2(-0.035f, 0.01f);
      MyGuiControlCompositePanel controlCompositePanel3 = controlCompositePanel2;
      controlCompositePanel3.Position = controlCompositePanel3.Position - (guiControlParent2.Size / 2f - new Vector2(-0.01f, 0.0f));
      MyGuiControlLabel myGuiControlLabel3 = myGuiControlLabel2;
      myGuiControlLabel3.Position = myGuiControlLabel3.Position - guiControlParent2.Size / 2f;
      MyGuiControlImage myGuiControlImage3 = myGuiControlImage2;
      myGuiControlImage3.Position = myGuiControlImage3.Position - guiControlParent2.Size / 2f;
      guiControlParent2.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_campaignList.Controls.Add((MyGuiControlBase) guiControlParent2);
    }
  }
}
