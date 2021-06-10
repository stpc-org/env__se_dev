// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWorldSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenWorldSettings : MyGuiScreenBase
  {
    private float MARGIN_TOP = 0.22f;
    private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
    private float MARGIN_LEFT_INFO = 29.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private string m_sessionPath;
    private MyGuiScreenWorldSettings m_parent;
    private List<MyObjectBuilder_Checkpoint.ModItem> m_mods;
    private MyObjectBuilder_Checkpoint m_checkpoint;
    private MyGuiControlTextbox m_nameTextbox;
    private MyGuiControlTextbox m_descriptionTextbox;
    private MyGuiControlCombobox m_onlineMode;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_survivalModeButton;
    private MyGuiControlButton m_creativeModeButton;
    private MyGuiControlButton m_worldGeneratorButton;
    private MyGuiControlSlider m_maxPlayersSlider;
    private MyGuiControlCheckbox m_autoSave;
    private MyGuiControlLabel m_maxPlayersLabel;
    private MyGuiControlLabel m_autoSaveLabel;
    private MyGuiControlList m_scenarioTypesList;
    private MyGuiControlRadioButtonGroup m_scenarioTypesGroup;
    private MyGuiControlRotatingWheel m_asyncLoadingWheel;
    private IMyAsyncResult m_loadingTask;
    private MyGuiControlRadioButton m_selectedButton;
    private MyGuiControlButton m_advanced;
    private bool m_displayTabScenario;
    private bool m_displayTabWorkshop;
    private bool m_displayTabCustom;
    private bool m_descriptionChanged;
    protected bool m_isNewGame;
    protected MyObjectBuilder_SessionSettings m_settings;
    internal MyGuiScreenAdvancedWorldSettings Advanced;
    internal MyGuiScreenWorldGeneratorSettings WorldGenerator;
    internal MyGuiScreenMods ModsScreen;
    private MyGuiControlButton m_modsButton;
    private readonly bool m_isCloudPath;
    private int m_maxPlayers;
    private bool m_parallelLoadIsRunning;

    public MyObjectBuilder_SessionSettings Settings
    {
      get
      {
        this.GetSettingsFromControls();
        return this.m_settings;
      }
    }

    public MyObjectBuilder_Checkpoint Checkpoint => this.m_checkpoint;

    public MyGuiScreenWorldSettings(
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true)
      : this((MyObjectBuilder_Checkpoint) null, (string) null, displayTabScenario, displayTabWorkshop, displayTabCustom)
    {
    }

    public MyGuiScreenWorldSettings(
      MyObjectBuilder_Checkpoint checkpoint,
      string path,
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true,
      bool isCloudPath = false)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(MyGuiScreenWorldSettings.CalcSize(checkpoint)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_displayTabScenario = displayTabScenario;
      this.m_displayTabWorkshop = displayTabWorkshop;
      this.m_displayTabCustom = displayTabCustom;
      this.m_isCloudPath = isCloudPath;
      MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor START");
      this.EnabledBackgroundFade = true;
      this.m_checkpoint = checkpoint;
      this.m_mods = checkpoint == null || checkpoint.Mods == null ? new List<MyObjectBuilder_Checkpoint.ModItem>() : checkpoint.Mods;
      this.m_sessionPath = path;
      this.m_isNewGame = checkpoint == null;
      this.RecreateControls(true);
      MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor END");
    }

    public static Vector2 CalcSize(MyObjectBuilder_Checkpoint checkpoint) => new Vector2(checkpoint == null ? 0.878f : 0.6535714f, checkpoint == null ? 0.97f : 0.9398855f);

    public override bool RegisterClicks() => true;

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (this.WorldGenerator != null)
        this.WorldGenerator.CloseScreen(isUnloading);
      this.WorldGenerator = (MyGuiScreenWorldGeneratorSettings) null;
      if (this.Advanced != null)
        this.Advanced.CloseScreen(isUnloading);
      this.Advanced = (MyGuiScreenAdvancedWorldSettings) null;
      if (this.ModsScreen != null)
        this.ModsScreen.CloseScreen(isUnloading);
      this.ModsScreen = (MyGuiScreenMods) null;
      return base.CloseScreen(isUnloading);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenWorldSettings);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BuildControls();
      if (this.m_isNewGame)
      {
        this.SetDefaultValues();
        MyGuiControlScreenSwitchPanel screenSwitchPanel = new MyGuiControlScreenSwitchPanel((MyGuiScreenBase) this, MyTexts.Get(MyCommonTexts.WorldSettingsScreen_Description), this.m_displayTabScenario, this.m_displayTabWorkshop, this.m_displayTabCustom);
        this.GamepadHelpTextId = new MyStringId?(!MyPlatformGameSettings.IsModdingAllowed || !MyFakes.ENABLE_WORKSHOP_MODS ? MySpaceTexts.WorldSettings_Help_ScreenNewGame_Modless : MySpaceTexts.WorldSettings_Help_ScreenNewGame);
      }
      else
      {
        this.LoadValues();
        this.m_nameTextbox.MoveCarriageToEnd();
        this.m_descriptionTextbox.MoveCarriageToEnd();
        this.GamepadHelpTextId = new MyStringId?(!MyPlatformGameSettings.IsModdingAllowed || !MyFakes.ENABLE_WORKSHOP_MODS ? MySpaceTexts.WorldSettings_Help_Screen_Modless : MySpaceTexts.WorldSettings_Help_Screen);
      }
    }

    private void InitCampaignList()
    {
      if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
        MyDefinitionManager.Static.LoadScenarios();
      Vector2 vector2 = -this.m_size.Value / 2f + new Vector2(this.MARGIN_LEFT_LIST, this.MARGIN_TOP);
      this.m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
      this.m_scenarioTypesGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.scenario_SelectedChanged);
      this.m_scenarioTypesGroup.MouseDoubleClick += (Action<MyGuiControlRadioButton>) (_ => this.OnOkButtonClick((object) null));
      MyGuiControlList myGuiControlList = new MyGuiControlList();
      myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlList.Position = vector2;
      myGuiControlList.Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, (float) ((double) this.m_size.Value.Y - (double) this.MARGIN_TOP - 0.0480000004172325));
      this.m_scenarioTypesList = myGuiControlList;
    }

    protected virtual void BuildControls()
    {
      if (this.m_isNewGame)
        this.AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
      else
        this.AddCaption(MyCommonTexts.ScreenCaptionEditSettings);
      int num1 = 0;
      if (this.m_isNewGame)
        this.InitCampaignList();
      Vector2 vector2_1 = new Vector2(0.0f, 0.052f);
      Vector2 vector2_2 = -this.m_size.Value / 2f + new Vector2(this.m_isNewGame ? this.MARGIN_LEFT_LIST + this.m_scenarioTypesList.Size.X + this.MARGIN_LEFT_INFO : this.MARGIN_LEFT_LIST, this.m_isNewGame ? this.MARGIN_TOP + 0.015f : this.MARGIN_TOP - 0.105f);
      Vector2 vector2_3 = this.m_size.Value / 2f - vector2_2;
      vector2_3.X -= this.MARGIN_RIGHT + 0.005f;
      vector2_3.Y -= this.MARGIN_BOTTOM;
      Vector2 vector2_4 = vector2_3 * (this.m_isNewGame ? 0.339f : 0.329f);
      Vector2 vector2_5 = vector2_2 + new Vector2(vector2_4.X, 0.0f);
      Vector2 vector2_6 = vector2_3 - vector2_4;
      MyGuiControlLabel myGuiControlLabel1 = this.MakeLabel(MyCommonTexts.Name);
      MyGuiControlLabel myGuiControlLabel2 = this.MakeLabel(MyCommonTexts.Description);
      MyGuiControlLabel myGuiControlLabel3 = this.MakeLabel(MyCommonTexts.WorldSettings_GameMode);
      MyGuiControlLabel myGuiControlLabel4 = this.MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
      this.m_maxPlayersLabel = this.MakeLabel(MyCommonTexts.MaxPlayers);
      this.m_autoSaveLabel = this.MakeLabel(MyCommonTexts.WorldSettings_AutoSave);
      this.m_nameTextbox = new MyGuiControlTextbox(maxLength: 90);
      this.m_descriptionTextbox = new MyGuiControlTextbox(maxLength: 7999);
      this.m_descriptionTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.OnDescriptionChanged);
      this.m_onlineMode = new MyGuiControlCombobox(size: new Vector2?(new Vector2(vector2_6.X, 0.04f)));
      this.m_maxPlayers = MyMultiplayerLobby.MAX_PLAYERS;
      Vector2? position1 = new Vector2?(Vector2.Zero);
      float x = this.m_onlineMode.Size.X;
      double num2 = (double) Math.Max(this.m_maxPlayers, 3);
      double num3 = (double) x;
      float? defaultValue = new float?();
      Vector4? color = new Vector4?();
      string labelText = new StringBuilder("{0}").ToString();
      this.m_maxPlayersSlider = new MyGuiControlSlider(position1, 2f, (float) num2, (float) num3, defaultValue, color, labelText, 0, labelSpaceWidth: 0.028f, intValue: true, showLabel: true);
      this.m_maxPlayersSlider.Value = (float) this.m_maxPlayers;
      this.m_autoSave = new MyGuiControlCheckbox();
      this.m_autoSave.SetToolTip(new StringBuilder().AppendFormat(MyCommonTexts.ToolTipWorldSettingsAutoSave, (object) 5U).ToString());
      this.m_creativeModeButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Small, text: MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative), onButtonClick: new Action<MyGuiControlButton>(this.OnCreativeClick));
      this.m_creativeModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeCreative);
      this.m_survivalModeButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Small, text: MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival), onButtonClick: new Action<MyGuiControlButton>(this.OnSurvivalClick));
      this.m_survivalModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeSurvival);
      this.m_onlineMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnOnlineModeSelect);
      this.m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
      this.m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
      this.m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
      this.m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
      if (this.m_isNewGame)
      {
        if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
          MyDefinitionManager.Static.LoadScenarios();
        this.m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
        this.m_scenarioTypesGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.scenario_SelectedChanged);
        this.m_scenarioTypesGroup.MouseDoubleClick += new Action<MyGuiControlRadioButton>(this.OnOkButtonClick);
        this.m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2((float) ((double) this.m_size.Value.X / 2.0 - 0.0769999995827675), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.108000002801418))), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
        this.m_loadingTask = this.StartLoadingWorldInfos();
      }
      this.m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), (object) 5, (object) 90));
      this.m_nameTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.NameFocusChanged);
      this.m_descriptionTextbox.SetToolTip(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsDescription));
      this.m_descriptionTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.DescriptionFocusChanged);
      this.m_onlineMode.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOnlineMode), (object) MySession.GameServiceName));
      this.m_onlineMode.HideToolTip();
      this.m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
      this.m_worldGeneratorButton = new MyGuiControlButton(text: MyTexts.Get(MySpaceTexts.WorldSettings_WorldGenerator), onButtonClick: new Action<MyGuiControlButton>(this.OnWorldGeneratorClick));
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_descriptionTextbox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.Controls.Add((MyGuiControlBase) this.m_creativeModeButton);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.Controls.Add((MyGuiControlBase) this.m_onlineMode);
      this.Controls.Add((MyGuiControlBase) this.m_maxPlayersLabel);
      this.Controls.Add((MyGuiControlBase) this.m_maxPlayersSlider);
      this.Controls.Add((MyGuiControlBase) this.m_autoSaveLabel);
      this.Controls.Add((MyGuiControlBase) this.m_autoSave);
      Vector2 vector2_7 = this.m_size.Value / 2f;
      vector2_7.X -= this.MARGIN_RIGHT + 0.004f;
      vector2_7.Y -= this.MARGIN_BOTTOM + 0.004f;
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      Vector2 genericButtonSpacing1 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      Vector2 genericButtonSpacing2 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      Vector2? position2 = new Vector2?(vector2_7 - new Vector2(backButtonSize.X + 0.0245f, 0.0f));
      Vector2? size1 = new Vector2?(backButtonSize);
      Vector4? colorMask1 = new Vector4?();
      StringBuilder stringBuilder1 = MyTexts.Get(MySpaceTexts.WorldSettings_Advanced);
      string toolTip1 = MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Advanced);
      StringBuilder text1 = stringBuilder1;
      Action<MyGuiControlButton> onButtonClick1 = new Action<MyGuiControlButton>(this.OnAdvancedClick);
      int? buttonIndex1 = new int?();
      this.m_advanced = new MyGuiControlButton(position2, size: size1, colorMask: colorMask1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick1, buttonIndex: buttonIndex1);
      this.m_advanced.Visible = !MyInput.Static.IsJoystickLastUsed;
      Vector2? position3 = new Vector2?(vector2_7 - new Vector2((float) ((double) backButtonSize.X * 2.0 + 0.0489999987185001), 0.0f));
      Vector2? size2 = new Vector2?(backButtonSize);
      Vector4? colorMask2 = new Vector4?();
      StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.WorldSettings_Mods);
      string toolTip2 = MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Mods);
      StringBuilder text2 = stringBuilder2;
      Action<MyGuiControlButton> onButtonClick2 = new Action<MyGuiControlButton>(this.OnModsClick);
      int? buttonIndex2 = new int?();
      this.m_modsButton = new MyGuiControlButton(position3, size: size2, colorMask: colorMask2, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, toolTip: toolTip2, text: text2, onButtonClick: onButtonClick2, buttonIndex: buttonIndex2);
      this.m_modsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
        this.Controls.Add((MyGuiControlBase) this.m_modsButton);
      this.Controls.Add((MyGuiControlBase) this.m_advanced);
      this.m_modsButton.SetEnabledByExperimental();
      foreach (MyGuiControlBase control in this.Controls)
      {
        control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        control.Position = !(control is MyGuiControlLabel) ? vector2_5 + vector2_1 * (float) num1++ : vector2_2 + vector2_1 * (float) num1;
      }
      if (this.m_isNewGame)
        this.Controls.Add((MyGuiControlBase) this.m_scenarioTypesList);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      Vector2 start;
      if (this.m_isNewGame)
      {
        start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.379999995231628 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
        controlSeparatorList.AddHorizontal(start, this.m_size.Value.X * 0.625f);
      }
      else
      {
        start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
        controlSeparatorList.AddHorizontal(start, this.m_size.Value.X * 0.835f);
        controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      }
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      if (this.m_isNewGame)
      {
        this.m_okButton = new MyGuiControlButton(new Vector2?(vector2_7), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Start), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClick));
        this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
      }
      else
      {
        this.m_okButton = new MyGuiControlButton(new Vector2?(vector2_7), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClick));
        this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      }
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_survivalModeButton);
      this.m_survivalModeButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_creativeModeButton.PositionX += 1f / 400f;
      this.m_creativeModeButton.PositionY += 0.005f;
      this.m_survivalModeButton.Position = this.m_creativeModeButton.Position + new Vector2(this.m_onlineMode.Size.X + 0.0005f, 0.0f);
      this.m_nameTextbox.Size = this.m_onlineMode.Size;
      this.m_descriptionTextbox.Size = this.m_nameTextbox.Size;
      this.m_maxPlayersSlider.PositionX = this.m_nameTextbox.PositionX - 1f / 1000f;
      this.m_autoSave.PositionX = this.m_maxPlayersSlider.PositionX;
      float num4 = 0.007f;
      if (this.m_modsButton != null)
      {
        this.m_modsButton.PositionX = this.m_maxPlayersSlider.PositionX + 3f / 1000f;
        this.m_modsButton.PositionY += num4;
      }
      if (this.m_advanced != null)
      {
        this.m_advanced.PositionX += (float) (0.0044999998062849 + (double) this.m_modsButton.Size.X + 0.00999999977648258);
        if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
          this.m_advanced.PositionY = this.m_modsButton.Position.Y;
        else
          this.m_advanced.PositionY += num4;
      }
      if (this.m_isNewGame)
        this.Controls.Add((MyGuiControlBase) this.m_asyncLoadingWheel);
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_okButton.Position.Y - backButtonSize.Y / 2f)));
      myGuiControlLabel5.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      this.CloseButtonEnabled = true;
    }

    private void OnDescriptionChanged(MyGuiControlTextbox obj) => this.m_descriptionChanged = true;

    private void NameFocusChanged(MyGuiControlBase obj, bool focused)
    {
      if (!focused || this.m_nameTextbox.IsImeActive)
        return;
      this.m_nameTextbox.SelectAll();
      this.m_nameTextbox.MoveCarriageToEnd();
    }

    private void DescriptionFocusChanged(MyGuiControlBase obj, bool focused)
    {
      if (!focused || this.m_descriptionTextbox.IsImeActive)
        return;
      this.m_descriptionTextbox.SelectAll();
      this.m_descriptionTextbox.MoveCarriageToEnd();
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_loadingTask != null && this.m_loadingTask.IsCompleted)
      {
        this.OnLoadingFinished(this.m_loadingTask, (MyGuiScreenProgressAsync) null);
        this.m_loadingTask = (IMyAsyncResult) null;
        this.m_asyncLoadingWheel.Visible = false;
      }
      if (hasFocus)
      {
        this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_advanced.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_modsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      }
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkButtonClick((object) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnAdvancedClick((object) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU, MyControlStateType.NEW_RELEASED) || !MyPlatformGameSettings.IsModdingAllowed || !MyFakes.ENABLE_WORKSHOP_MODS)
        return;
      this.OnModsClick((object) null);
    }

    private void scenario_SelectedChanged(MyGuiControlRadioButtonGroup group)
    {
      this.SetDefaultName();
      if (MyFakes.ENABLE_PLANETS)
      {
        this.m_worldGeneratorButton.Enabled = true;
        if (this.m_worldGeneratorButton.Enabled && this.WorldGenerator != null)
          this.WorldGenerator.GetSettings(this.m_settings);
      }
      MyObjectBuilder_Checkpoint builderCheckpoint = MyLocalCache.LoadCheckpoint(group.SelectedButton.UserData as string, out ulong _);
      if (builderCheckpoint != null)
      {
        this.m_settings = this.CopySettings(builderCheckpoint.Settings);
        this.SetSettingsToControls();
      }
      if (this.m_selectedButton != null)
        this.m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_selectedButton = group.SelectedButton;
      this.m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
    }

    private MyGuiControlLabel MakeLabel(MyStringId textEnum) => new MyGuiControlLabel(text: MyTexts.GetString(textEnum));

    private void SetDefaultName()
    {
      if (this.m_scenarioTypesGroup.SelectedButton == null)
        return;
      string title = ((MyGuiControlContentButton) this.m_scenarioTypesGroup.SelectedButton).Title;
      if (title != null)
        this.m_nameTextbox.Text = title.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
      this.m_descriptionTextbox.Text = string.Empty;
    }

    private void LoadValues()
    {
      this.m_nameTextbox.Text = this.m_checkpoint.SessionName ?? "";
      this.m_descriptionTextbox.TextChanged -= new Action<MyGuiControlTextbox>(this.OnDescriptionChanged);
      this.m_descriptionTextbox.Text = string.IsNullOrEmpty(this.m_checkpoint.Description) ? "" : MyTexts.SubstituteTexts(this.m_checkpoint.Description);
      this.m_descriptionTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.OnDescriptionChanged);
      this.m_descriptionChanged = false;
      this.m_settings = this.CopySettings(this.m_checkpoint.Settings);
      this.m_mods = this.m_checkpoint.Mods;
      this.SetSettingsToControls();
    }

    private void SetDefaultValues()
    {
      this.m_settings = this.GetDefaultSettings();
      this.m_settings.EnableToolShake = true;
      this.m_settings.EnableSunRotation = MyPerGameSettings.Game == GameEnum.SE_GAME;
      this.m_settings.VoxelGeneratorVersion = 4;
      this.m_settings.EnableOxygen = true;
      this.m_settings.CargoShipsEnabled = true;
      this.m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
      this.SetSettingsToControls();
      this.SetDefaultName();
    }

    protected virtual MyObjectBuilder_SessionSettings GetDefaultSettings() => MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();

    protected virtual MyObjectBuilder_SessionSettings CopySettings(
      MyObjectBuilder_SessionSettings source)
    {
      return source.Clone() as MyObjectBuilder_SessionSettings;
    }

    private void OnOnlineModeSelect()
    {
      bool flag = this.m_onlineMode.GetSelectedKey() == 0L;
      this.m_maxPlayersSlider.Enabled = !flag && this.m_maxPlayers > 2;
      this.m_maxPlayersLabel.Enabled = !flag && this.m_maxPlayers > 2;
      int? nullable1 = flag ? MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX : MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
      if (!nullable1.HasValue)
        return;
      if (flag)
      {
        int totalPcu = this.m_settings.TotalPCU;
        int? lobbyTotalPcuMax = MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
        int valueOrDefault = lobbyTotalPcuMax.GetValueOrDefault();
        if (!(totalPcu == valueOrDefault & lobbyTotalPcuMax.HasValue))
          return;
        this.m_settings.TotalPCU = nullable1.Value;
      }
      else
      {
        int totalPcu = this.m_settings.TotalPCU;
        int? nullable2 = nullable1;
        int valueOrDefault = nullable2.GetValueOrDefault();
        if (!(totalPcu > valueOrDefault & nullable2.HasValue))
          return;
        this.m_settings.TotalPCU = nullable1.Value;
        if (this.m_isNewGame)
          return;
        MyGuiSandbox.Show(MyCommonTexts.MessageBoxTextBlockLimitsInMP, MyCommonTexts.MessageBoxCaptionWarning, MyMessageBoxStyleEnum.Info);
      }
    }

    private void OnCreativeClick(object sender)
    {
      this.UpdateSurvivalState(false);
      this.Settings.EnableCopyPaste = true;
    }

    private void OnSurvivalClick(object sender)
    {
      this.UpdateSurvivalState(true);
      this.Settings.EnableCopyPaste = false;
    }

    private void OnAdvancedClick(object sender)
    {
      this.Advanced = new MyGuiScreenAdvancedWorldSettings(this);
      this.Advanced.UpdateSurvivalState(this.GetGameMode() == MyGameModeEnum.Survival);
      this.Advanced.OnOkButtonClicked += new Action(this.Advanced_OnOkButtonClicked);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) this.Advanced);
    }

    private void OnWorldGeneratorClick(object sender)
    {
      this.WorldGenerator = new MyGuiScreenWorldGeneratorSettings(this);
      this.WorldGenerator.OnOkButtonClicked += new Action(this.WorldGenerator_OnOkButtonClicked);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) this.WorldGenerator);
    }

    private void WorldGenerator_OnOkButtonClicked()
    {
      this.WorldGenerator.GetSettings(this.m_settings);
      this.SetSettingsToControls();
    }

    private void OnModsClick(object sender)
    {
      if (this.m_modsButton.Enabled)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenMods(this.m_mods));
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldSettings_ModsNeedExperimental), messageCaption: messageCaption));
      }
    }

    private void UpdateSurvivalState(bool survivalEnabled)
    {
      this.m_creativeModeButton.Checked = !survivalEnabled;
      this.m_survivalModeButton.Checked = survivalEnabled;
    }

    private void Advanced_OnOkButtonClicked()
    {
      this.Advanced.GetSettings(this.m_settings);
      this.SetSettingsToControls();
    }

    private void OnOkButtonClick(object sender)
    {
      bool flag = this.m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
      if (flag || this.m_nameTextbox.Text.Length < 5 || this.m_nameTextbox.Text.Length > 90)
      {
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(!flag ? (this.m_nameTextbox.Text.Length >= 5 ? MyCommonTexts.ErrorNameTooLong : MyCommonTexts.ErrorNameTooShort) : MyCommonTexts.ErrorNameInvalid), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      else if (this.m_descriptionTextbox.Text.Length > 7999)
      {
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ErrorDescriptionTooLong), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      else
      {
        this.GetSettingsFromControls();
        if (this.m_settings.OnlineMode != MyOnlineModeEnum.OFFLINE && !MyGameService.IsActive)
        {
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ErrorStartSessionNoUser), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
          messageBox.SkipTransition = true;
          messageBox.InstantClose = false;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
        }
        else if (this.m_isNewGame)
        {
          if (this.m_parallelLoadIsRunning)
            return;
          this.m_parallelLoadIsRunning = true;
          MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
          MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
          Parallel.StartBackground((Action) (() => this.StartNewSandbox()), (Action) (() =>
          {
            progressScreen.CloseScreen();
            this.m_parallelLoadIsRunning = false;
          }));
        }
        else
          this.OnOkButtonClickQuestions(0);
      }
    }

    private void OnOkButtonClickQuestions(int skipQuestions)
    {
      if (skipQuestions <= 0)
      {
        int num = this.m_checkpoint.Settings.GameMode != MyGameModeEnum.Creative ? 0 : (this.GetGameMode() == MyGameModeEnum.Survival ? 1 : 0);
        bool flag = this.m_checkpoint.Settings.GameMode == MyGameModeEnum.Survival && this.GetGameMode() == MyGameModeEnum.Creative;
        if (num != 0 || !flag && (double) this.m_checkpoint.Settings.InventorySizeMultiplier > (double) this.m_settings.InventorySizeMultiplier)
        {
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.HarvestingWarningInventoryMightBeTruncatedAreYouSure), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (x => this.OnOkButtonClickAnswer(x, 1))));
          messageBox.SkipTransition = true;
          messageBox.InstantClose = false;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
          return;
        }
      }
      if (skipQuestions <= 1 && (this.m_checkpoint.Settings.WorldSizeKm == 0 || this.m_checkpoint.Settings.WorldSizeKm > this.m_settings.WorldSizeKm ? ((uint) this.m_settings.WorldSizeKm > 0U ? 1 : 0) : 0) != 0)
      {
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.WorldSettings_WarningChangingWorldSize), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (x => this.OnOkButtonClickAnswer(x, 2))));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      else
        this.ChangeWorldSettings();
    }

    private void OnOkButtonClickAnswer(MyGuiScreenMessageBox.ResultEnum answer, int skipQuestions)
    {
      if (answer != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      this.OnOkButtonClickQuestions(skipQuestions);
    }

    private MyGameModeEnum GetGameMode() => !this.m_survivalModeButton.Checked ? MyGameModeEnum.Creative : MyGameModeEnum.Survival;

    protected virtual bool GetSettingsFromControls()
    {
      if (this.m_onlineMode == null || this.m_settings == null || (this.m_maxPlayersSlider == null || this.m_autoSave == null))
        return false;
      this.m_settings.OnlineMode = (MyOnlineModeEnum) this.m_onlineMode.GetSelectedKey();
      if (this.m_checkpoint != null)
        this.m_checkpoint.PreviousEnvironmentHostility = new MyEnvironmentHostilityEnum?(this.m_settings.EnvironmentHostility);
      this.m_settings.MaxPlayers = (short) this.m_maxPlayersSlider.Value;
      this.m_settings.GameMode = this.GetGameMode();
      this.m_settings.ScenarioEditMode = false;
      this.m_settings.AutoSaveInMinutes = this.m_autoSave.IsChecked ? 5U : 0U;
      return true;
    }

    protected virtual void SetSettingsToControls()
    {
      this.m_onlineMode.SelectItemByKey((long) this.m_settings.OnlineMode);
      this.m_maxPlayersSlider.Value = (float) Math.Min((int) this.m_settings.MaxPlayers, this.m_maxPlayers);
      this.UpdateSurvivalState(this.m_settings.GameMode == MyGameModeEnum.Survival);
      this.m_autoSave.IsChecked = this.m_settings.AutoSaveInMinutes > 0U;
    }

    private string GetPassword()
    {
      if (this.Advanced != null && this.Advanced.IsConfirmed)
        return this.Advanced.Password;
      return this.m_checkpoint != null ? this.m_checkpoint.Password : "";
    }

    private string GetDescription() => this.m_checkpoint != null ? this.m_checkpoint.Description : this.m_descriptionTextbox.Text;

    private bool DescriptionChanged() => this.m_descriptionChanged;

    private void CopySaveTo(string destName, out bool fileExists, out CloudResult copyResult)
    {
      fileExists = false;
      copyResult = CloudResult.Failed;
      if (!this.m_isCloudPath)
      {
        string sessionPath = this.m_sessionPath;
        string str = this.m_sessionPath.Replace(this.m_checkpoint.SessionName, destName);
        if (str == this.m_sessionPath)
          str = Path.Combine(MyFileSystem.SavesPath, destName);
        if (Directory.Exists(str))
        {
          fileExists = true;
        }
        else
        {
          try
          {
            Directory.CreateDirectory(Path.GetDirectoryName(str));
            Directory.Move(sessionPath, str);
            this.m_sessionPath = str;
            copyResult = CloudResult.Ok;
          }
          catch
          {
            copyResult = CloudResult.Failed;
          }
        }
      }
      else
      {
        string sessionPath = this.m_sessionPath;
        string str = this.m_sessionPath.Replace(this.m_checkpoint.SessionName, destName);
        if (str == this.m_sessionPath)
          str = MyLocalCache.GetSessionSavesPath(destName, false, false, true);
        List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(str);
        if (cloudFiles != null && cloudFiles.Count > 0)
        {
          fileExists = true;
        }
        else
        {
          CloudResult cloudResult = MyCloudHelper.CopyFiles(sessionPath, str);
          if (cloudResult == CloudResult.Ok)
          {
            MyCloudHelper.Delete(sessionPath);
            this.m_sessionPath = str;
          }
          copyResult = cloudResult;
        }
      }
    }

    private void ChangeWorldSettings()
    {
      if (this.m_nameTextbox.Text != this.m_checkpoint.SessionName)
      {
        bool fileExists;
        CloudResult copyResult;
        this.CopySaveTo(MyUtils.StripInvalidChars(this.m_nameTextbox.Text), out fileExists, out copyResult);
        if (fileExists)
        {
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.WorldSettings_Error_NameExists), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
          messageBox.SkipTransition = true;
          messageBox.InstantClose = false;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
          return;
        }
        MyStringId errorMessage;
        if (MyCloudHelper.IsError(copyResult, out errorMessage, new MyStringId?(MySpaceTexts.WorldSettings_Error_SavingFailed)))
        {
          MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
          messageBox.SkipTransition = true;
          messageBox.InstantClose = false;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
          return;
        }
      }
      this.m_checkpoint.SessionName = this.m_nameTextbox.Text;
      if (this.DescriptionChanged())
      {
        this.m_checkpoint.Description = this.m_descriptionTextbox.Text;
        this.m_descriptionChanged = false;
      }
      this.GetSettingsFromControls();
      this.m_checkpoint.Settings = this.m_settings;
      this.m_checkpoint.Mods = this.m_mods;
      MyCampaignManager.Static?.SetExperimentalCampaign(this.m_checkpoint);
      if (this.m_isCloudPath)
      {
        int cloud = (int) MyLocalCache.SaveCheckpointToCloud(this.m_checkpoint, this.m_sessionPath);
      }
      else
        MyLocalCache.SaveCheckpoint(this.m_checkpoint, this.m_sessionPath);
      if (MySession.Static != null && MySession.Static.Name == this.m_checkpoint.SessionName && this.m_sessionPath == MySession.Static.CurrentPath)
      {
        MySession mySession = MySession.Static;
        mySession.Password = this.GetPassword();
        mySession.Description = this.GetDescription();
        mySession.Settings = this.m_checkpoint.Settings;
        mySession.Mods = this.m_checkpoint.Mods;
      }
      this.CloseScreen(false);
    }

    private void OnCancelButtonClick(object sender) => this.CloseScreen(false);

    private void OnSwitchAnswer(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MySandboxGame.Config.GraphicsRenderer = MySandboxGame.DirectX11RendererKey;
        MySandboxGame.Config.Save();
        MyGuiSandbox.BackToMainMenu();
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.QuickstartDX11PleaseRestartGame), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      }
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.QuickstartSelectDifferent), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
    }

    private void StartNewSandbox()
    {
      MyLog.Default.WriteLine("StartNewSandbox - Start");
      if (this.m_scenarioTypesGroup.SelectedButton == null || this.m_scenarioTypesGroup.SelectedButton.UserData == null || !this.GetSettingsFromControls())
        return;
      string sessionPath = this.m_scenarioTypesGroup.SelectedButton.UserData as string;
      ulong checkpointSizeInBytes;
      MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out checkpointSizeInBytes);
      if (checkpoint == null)
        return;
      if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
      {
        MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
      }
      else
      {
        this.GetSettingsFromControls();
        checkpoint.Settings = this.m_settings;
        checkpoint.SessionName = this.m_nameTextbox.Text;
        checkpoint.Password = this.GetPassword();
        checkpoint.Description = this.GetDescription();
        checkpoint.Mods = this.m_mods;
        if (checkpoint.Settings.OnlineMode != MyOnlineModeEnum.OFFLINE)
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
                      StartSession();
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
          StartSession();
      }

      void StartSession() => MySessionLoader.LoadSingleplayerSession(checkpoint, sessionPath, checkpointSizeInBytes, (Action) (() =>
      {
        string saveName = Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-'));
        MySession.Static.CurrentPath = saveName;
        MyAsyncSaving.DelayedSaveAfterLoad(saveName);
      }));
    }

    private IMyAsyncResult StartLoadingWorldInfos()
    {
      string str = Path.Combine(MyFileSystem.ContentPath, "CustomWorlds");
      if (this.m_isNewGame)
        return (IMyAsyncResult) new MyNewCustomWorldInfoListResult(new List<string>()
        {
          str
        });
      return (IMyAsyncResult) new MyLoadWorldInfoListResult(new List<string>()
      {
        str
      });
    }

    private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      MyLoadListResult myLoadListResult = (MyLoadListResult) result;
      this.m_scenarioTypesGroup.Clear();
      this.m_scenarioTypesList.Clear();
      if (myLoadListResult.AvailableSaves.Count != 0)
        myLoadListResult.AvailableSaves.Sort((Comparison<Tuple<string, MyWorldInfo>>) ((a, b) => a.Item2.SessionName.CompareTo(b.Item2.SessionName)));
      foreach (Tuple<string, MyWorldInfo> availableSave in myLoadListResult.AvailableSaves)
      {
        if ((MySandboxGame.Config.ExperimentalMode || !availableSave.Item2.IsExperimental) && (MyFakes.ENABLE_PLANETS || !availableSave.Item2.HasPlanets))
        {
          string directoryName = availableSave.Item1;
          if (Path.HasExtension(availableSave.Item1))
            directoryName = Path.GetDirectoryName(availableSave.Item1);
          MyGuiControlContentButton controlContentButton1 = new MyGuiControlContentButton(availableSave.Item2.SessionName, Path.Combine(directoryName, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
          controlContentButton1.UserData = (object) directoryName;
          controlContentButton1.Key = this.m_scenarioTypesGroup.Count;
          MyGuiControlContentButton controlContentButton2 = controlContentButton1;
          controlContentButton2.FocusHighlightAlsoSelects = true;
          this.m_scenarioTypesGroup.Add((MyGuiControlRadioButton) controlContentButton2);
          this.m_scenarioTypesList.Controls.Add((MyGuiControlBase) controlContentButton2);
        }
      }
      if (this.m_scenarioTypesList.Controls.Count > 0)
      {
        this.m_scenarioTypesGroup.SelectByIndex(0);
        this.FocusedControl = this.m_scenarioTypesList.Controls[0];
      }
      else
        this.SetDefaultValues();
    }
  }
}
