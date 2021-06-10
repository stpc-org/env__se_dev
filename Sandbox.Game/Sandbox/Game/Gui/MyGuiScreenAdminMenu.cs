// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenAdminMenu
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.AdminMenu;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [PreloadRequired]
  [StaticEventOwner]
  public class MyGuiScreenAdminMenu : MyGuiScreenDebugBase
  {
    public static readonly int BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE = 2000;
    internal static readonly float TEXT_ALIGN_CONST = 0.05f;
    private static readonly Vector2 CB_OFFSET = new Vector2(-0.05f, 0.0f);
    private static MyGuiScreenAdminMenu m_static;
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private readonly Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private readonly float m_textScale = 0.8f;
    protected static MyEntityCyclingOrder m_order;
    private static float m_metricValue = 0.0f;
    private static long m_entityId;
    private static bool m_showMedbayNotification = true;
    private long m_attachCamera;
    private bool m_attachIsNpcStation;
    private MyGuiControlLabel m_errorLabel;
    private MyGuiControlLabel m_labelCurrentIndex;
    private MyGuiControlLabel m_labelEntityName;
    private MyGuiControlLabel m_labelNumVisible;
    protected MyGuiControlButton m_removeItemButton;
    private MyGuiControlButton m_depowerItemButton;
    protected MyGuiControlButton m_stopItemButton;
    protected MyGuiControlCheckbox m_onlySmallGridsCheckbox;
    private MyGuiControlCheckbox m_onlyLargeGridsCheckbox;
    private static CyclingOptions m_cyclingOptions = new CyclingOptions();
    protected Vector4 m_labelColor = Color.White.ToVector4();
    protected MyGuiControlCheckbox m_creativeCheckbox;
    private readonly List<IMyGps> m_gpsList = new List<IMyGps>();
    protected MyGuiControlCombobox m_modeCombo;
    protected MyGuiControlCombobox m_onlinePlayerCombo;
    protected long m_onlinePlayerCombo_SelectedPlayerIdentityId;
    protected MyGuiControlTextbox m_addCurrencyTextbox;
    protected MyGuiControlButton m_addCurrencyConfirmButton;
    protected MyGuiControlLabel m_labelCurrentBalanceValue;
    protected MyGuiControlLabel m_labelFinalBalanceValue;
    protected int m_playerCount;
    protected int m_factionCount;
    protected bool m_isPlayerSelected;
    protected bool m_isFactionSelected;
    protected long m_currentBalance;
    protected long m_finalBalance;
    protected long m_balanceDifference;
    protected MyGuiControlCombobox m_playerReputationCombo;
    protected long m_playerReputationCombo_SelectedPlayerIdentityId;
    protected MyGuiControlCombobox m_factionReputationCombo;
    protected long m_factionReputationCombo_SelectedPlayerIdentityId;
    protected MyGuiControlTextbox m_addReputationTextbox;
    protected MyGuiControlButton m_addReputationConfirmButton;
    protected MyGuiControlLabel m_labelCurrentReputationValue;
    protected MyGuiControlLabel m_labelFinalReputationValue;
    protected MyGuiControlCheckbox m_addReputationPropagate;
    protected int m_currentReputation;
    protected int m_finalReputation;
    protected int m_reputationDifference;
    protected MyGuiControlCheckbox m_invulnerableCheckbox;
    protected MyGuiControlCheckbox m_untargetableCheckbox;
    protected MyGuiControlCheckbox m_showPlayersCheckbox;
    protected MyGuiControlCheckbox m_keepOriginalOwnershipOnPasteCheckBox;
    protected MyGuiControlCheckbox m_ignoreSafeZonesCheckBox;
    protected MyGuiControlCheckbox m_ignorePcuCheckBox;
    protected MyGuiControlCheckbox m_canUseTerminals;
    protected MyGuiControlSlider m_timeDelta;
    protected MyGuiControlLabel m_timeDeltaValue;
    protected MyGuiControlListbox m_entityListbox;
    protected MyGuiControlCombobox m_entityTypeCombo;
    protected MyGuiControlCombobox m_entitySortCombo;
    private MyEntityList.MyEntityTypeEnum m_selectedType;
    private MyEntityList.MyEntitySortOrder m_selectedSort;
    private static bool m_invertOrder;
    private static bool m_damageHandler;
    private static HashSet<long> m_protectedCharacters = new HashSet<long>();
    private static MyGuiScreenAdminMenu.MyPageEnum m_currentPage;
    private int m_currentGpsIndex;
    private bool m_unsavedTrashSettings;
    private MyGuiScreenAdminMenu.AdminSettings m_newSettings;
    private bool m_unsavedTrashExitBoxIsOpened;
    private MyGuiControlCombobox m_trashRemovalCombo;
    private MyGuiControlStackPanel m_trashRemovalContentPanel;
    private Dictionary<MyTabControlEnum, MyTabContainer> m_tabs = new Dictionary<MyTabControlEnum, MyTabContainer>();
    private MyGuiScreenMessageBox m_cleanupRequestingMessageBox;
    protected MyGuiControlLabel m_enabledCheckboxGlobalLabel;
    protected MyGuiControlLabel m_damageCheckboxGlobalLabel;
    protected MyGuiControlLabel m_shootingCheckboxGlobalLabel;
    protected MyGuiControlLabel m_drillingCheckboxGlobalLabel;
    protected MyGuiControlLabel m_weldingCheckboxGlobalLabel;
    protected MyGuiControlLabel m_grindingCheckboxGlobalLabel;
    protected MyGuiControlLabel m_voxelHandCheckboxGlobalLabel;
    protected MyGuiControlLabel m_buildingCheckboxGlobalLabel;
    protected MyGuiControlLabel m_buildingProjectionsCheckboxGlobalLabel;
    protected MyGuiControlLabel m_landingGearCheckboxGlobalLabel;
    protected MyGuiControlLabel m_convertToStationCheckboxGlobalLabel;
    protected MyGuiControlCheckbox m_enabledGlobalCheckbox;
    protected MyGuiControlCheckbox m_damageGlobalCheckbox;
    protected MyGuiControlCheckbox m_shootingGlobalCheckbox;
    protected MyGuiControlCheckbox m_drillingGlobalCheckbox;
    protected MyGuiControlCheckbox m_weldingGlobalCheckbox;
    protected MyGuiControlCheckbox m_grindingGlobalCheckbox;
    protected MyGuiControlCheckbox m_voxelHandGlobalCheckbox;
    protected MyGuiControlCheckbox m_buildingGlobalCheckbox;
    protected MyGuiControlCheckbox m_buildingProjectionsGlobalCheckbox;
    protected MyGuiControlCheckbox m_landingGearGlobalCheckbox;
    protected MyGuiControlCheckbox m_convertToStationGlobalCheckbox;
    private static readonly int LOOP_LIMIT = 5;
    private static List<MyGuiScreenAdminMenu> m_matchSyncReceivers = new List<MyGuiScreenAdminMenu>();
    private MyGuiControlLabel m_labelEnabled;
    private MyGuiControlLabel m_labelRunning;
    private MyGuiControlLabel m_labelState;
    private MyGuiControlLabel m_labelTime;
    private MyGuiControlButton m_buttonStart;
    private MyGuiControlButton m_buttonStop;
    private MyGuiControlButton m_buttonPause;
    private MyGuiControlButton m_buttonAdvanced;
    private MyGuiControlButton m_buttonSetTime;
    private MyGuiControlButton m_buttonAddTime;
    private MyGuiControlTextbox m_textboxTime;
    private bool m_isMatchEnabled;
    private bool m_isMatchRunning;
    private MyMatchState m_matchCurrentState;
    private MyTimeSpan m_matchRemainingTime;
    private MyTimeSpan m_matchLastUpdateTime;
    protected MyGuiControlScrollablePanel m_optionsGroup;
    protected MyGuiControlLabel m_selectSafeZoneLabel;
    protected MyGuiControlLabel m_selectZoneShapeLabel;
    protected MyGuiControlLabel m_selectAxisLabel;
    protected MyGuiControlLabel m_zoneRadiusLabel;
    protected MyGuiControlLabel m_zoneSizeLabel;
    protected MyGuiControlLabel m_zoneRadiusValueLabel;
    protected MyGuiControlCombobox m_safeZonesCombo;
    protected MyGuiControlCombobox m_safeZonesTypeCombo;
    protected MyGuiControlCombobox m_safeZonesAxisCombo;
    protected MyGuiControlSlider m_sizeSlider;
    protected MyGuiControlSlider m_radiusSlider;
    protected MyGuiControlButton m_addSafeZoneButton;
    protected MyGuiControlButton m_repositionSafeZoneButton;
    protected MyGuiControlButton m_moveToSafeZoneButton;
    protected MyGuiControlButton m_removeSafeZoneButton;
    protected MyGuiControlButton m_renameSafeZoneButton;
    protected MyGuiControlButton m_configureFilterButton;
    protected MyGuiControlLabel m_enabledCheckboxLabel;
    protected MyGuiControlLabel m_damageCheckboxLabel;
    protected MyGuiControlLabel m_shootingCheckboxLabel;
    protected MyGuiControlLabel m_drillingCheckboxLabel;
    protected MyGuiControlLabel m_weldingCheckboxLabel;
    protected MyGuiControlLabel m_grindingCheckboxLabel;
    protected MyGuiControlLabel m_voxelHandCheckboxLabel;
    protected MyGuiControlLabel m_buildingCheckboxLabel;
    protected MyGuiControlLabel m_buildingProjectionsCheckboxLabel;
    protected MyGuiControlLabel m_landingGearLockCheckboxLabel;
    protected MyGuiControlLabel m_convertToStationCheckboxLabel;
    protected MyGuiControlCheckbox m_enabledCheckbox;
    protected MyGuiControlCheckbox m_damageCheckbox;
    protected MyGuiControlCheckbox m_shootingCheckbox;
    protected MyGuiControlCheckbox m_drillingCheckbox;
    protected MyGuiControlCheckbox m_weldingCheckbox;
    protected MyGuiControlCheckbox m_grindingCheckbox;
    protected MyGuiControlCheckbox m_voxelHandCheckbox;
    protected MyGuiControlCheckbox m_buildingCheckbox;
    protected MyGuiControlCheckbox m_buildingProjectionsCheckbox;
    protected MyGuiControlCheckbox m_landingGearLockCheckbox;
    protected MyGuiControlCheckbox m_convertToStationCheckbox;
    private MyGuiControlCombobox m_textureCombo;
    private MyGuiControlColor m_colorSelector;
    private MySafeZone m_selectedSafeZone;
    private bool m_recreateInProgress;
    private MyGuiControlCombobox m_cameraModeCombo;
    private MyGuiControlSlider m_cameraSmoothness;
    private MyGuiControlLabel m_cameraSmoothnessValueLabel;
    private MyGuiControlListbox m_trackedListbox;
    private const int UPDATE_INTERVAL = 100;
    private string m_selectedWeatherSubtypeId;
    private MyGuiControlCombobox m_weatherSelectionCombo;
    private MyGuiControlMultilineText messageMultiline;
    private Dictionary<int, Tuple<string, string>> m_weatherNamesAndSubtypesDictionary;
    private int m_weatherUpdateCounter;
    private MyWeatherEffectDefinition[] m_definitions;

    public MyGuiScreenAdminMenu()
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenAdminMenu.SCREEN_SIZE.X * 0.5f + MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiScreenAdminMenu.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
      if (MyPlatformGameSettings.ENABLE_LOW_MEM_WORLD_LOCKDOWN && MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical)
      {
        this.ShowCleanupRequest();
        MyGuiScreenAdminMenu.m_currentPage = MyGuiScreenAdminMenu.MyPageEnum.EntityList;
      }
      if (!Sync.IsServer)
      {
        MyGuiScreenAdminMenu.m_static = this;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.RequestSettingFromServer_Implementation)));
      }
      else
        this.CreateScreen();
      MySessionComponentSafeZones.OnAddSafeZone += new EventHandler(this.MySafeZones_OnAddSafeZone);
      MySessionComponentSafeZones.OnRemoveSafeZone += new EventHandler(this.MySafeZones_OnRemoveSafeZone);
    }

    private void CreateScreen()
    {
      this.m_closeOnEsc = false;
      this.CanBeHidden = true;
      this.CanHideOthers = true;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_canShareInput = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.StoreTrashSettings_RealToTmp();
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_tabs.Clear();
      Vector2 controlPadding = new Vector2(0.02f, 0.02f);
      float num1 = 0.8f;
      float separatorSize = 0.01f;
      float num2 = (float) ((double) MyGuiScreenAdminMenu.SCREEN_SIZE.X - (double) MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT - (double) controlPadding.X * 2.0);
      float num3 = (float) (((double) MyGuiScreenAdminMenu.SCREEN_SIZE.Y - 1.0) / 2.0);
      MyGuiScreenAdminMenu.m_static = this;
      this.m_currentPosition = -this.m_size.Value / 2f;
      this.m_currentPosition = this.m_currentPosition + controlPadding;
      this.m_currentPosition.Y += num3;
      this.m_scale = num1;
      this.AddCaption(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ModeSelect).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(this.m_controlPadding + new Vector2(-MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT, num3 - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.365f), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      this.m_currentPosition.X += 0.018f;
      this.m_currentPosition.Y += (float) ((double) MyGuiConstants.SCREEN_CAPTION_DELTA_Y + (double) controlPadding.Y - 0.0120000001043081);
      this.m_modeCombo = this.AddCombo();
      if (MySession.Static.IsUserSpaceMaster(Sync.MyId))
      {
        this.m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
        this.m_modeCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_CycleObjects);
        if (MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING || MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)
          this.m_modeCombo.AddItem(1L, MySpaceTexts.ScreenDebugAdminMenu_Cleanup);
        this.m_modeCombo.AddItem(3L, MySpaceTexts.ScreenDebugAdminMenu_EntityList);
        if (MySession.Static.IsUserAdmin(Sync.MyId))
        {
          this.m_modeCombo.AddItem(4L, MySpaceTexts.ScreenDebugAdminMenu_SafeZones);
          this.m_modeCombo.AddItem(5L, MySpaceTexts.ScreenDebugAdminMenu_GlobalSafeZone);
          this.m_modeCombo.AddItem(6L, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool);
          this.m_modeCombo.AddItem(7L, MySpaceTexts.ScreenDebugAdminMenu_Economy);
          this.m_modeCombo.AddItem(8L, MySpaceTexts.ScreenDebugAdminMenu_Weather);
          this.m_modeCombo.AddItem(9L, "Spectator Tool");
          this.m_modeCombo.AddItem(10L, MySpaceTexts.ScreenDebugAdminMenu_Match);
        }
        else if (MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.GlobalSafeZone || MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.SafeZones || (MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.ReplayTool || MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.Economy))
          MyGuiScreenAdminMenu.m_currentPage = MyGuiScreenAdminMenu.MyPageEnum.CycleObjects;
        this.m_modeCombo.SelectItemByKey((long) MyGuiScreenAdminMenu.m_currentPage);
      }
      else
      {
        this.m_modeCombo.AddItem(0L, MySpaceTexts.ScreenDebugAdminMenu_AdminTools);
        MyGuiScreenAdminMenu.m_currentPage = MyGuiScreenAdminMenu.MyPageEnum.AdminTools;
        this.m_modeCombo.SelectItemByKey((long) MyGuiScreenAdminMenu.m_currentPage);
      }
      this.m_modeCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnModeComboSelect);
      switch (MyGuiScreenAdminMenu.m_currentPage)
      {
        case MyGuiScreenAdminMenu.MyPageEnum.AdminTools:
          this.m_currentPosition.Y += 0.03f;
          MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
          myGuiControlLabel1.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode);
          myGuiControlLabel1.IsAutoScaleEnabled = true;
          myGuiControlLabel1.IsAutoEllipsisEnabled = true;
          MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
          myGuiControlLabel2.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
          this.m_creativeCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_creativeCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnEnableAdminModeChanged);
          this.m_creativeCheckbox.SetToolTip(MyCommonTexts.ScreenDebugAdminMenu_EnableAdminMode_Tooltip);
          this.m_creativeCheckbox.IsChecked = MySession.Static.CreativeToolsEnabled(Sync.MyId);
          this.m_creativeCheckbox.Enabled = MySession.Static.HasCreativeRights;
          this.Controls.Add((MyGuiControlBase) this.m_creativeCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
          myGuiControlLabel3.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Invulnerable);
          MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
          this.m_invulnerableCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_invulnerableCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnInvulnerableChanged);
          this.m_invulnerableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_InvulnerableToolTip);
          this.m_invulnerableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.Invulnerable);
          this.m_invulnerableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
          this.Controls.Add((MyGuiControlBase) this.m_invulnerableCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
          myGuiControlLabel5.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel5.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Untargetable);
          MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
          this.m_untargetableCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_untargetableCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnUntargetableChanged);
          this.m_untargetableCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UntargetableToolTip);
          this.m_untargetableCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.Untargetable);
          this.m_untargetableCheckbox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
          this.Controls.Add((MyGuiControlBase) this.m_untargetableCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
          myGuiControlLabel7.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel7.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayers);
          MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
          this.m_showPlayersCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_showPlayersCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnShowPlayersChanged);
          this.m_showPlayersCheckbox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_ShowPlayersToolTip);
          this.m_showPlayersCheckbox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.ShowPlayers);
          this.m_showPlayersCheckbox.Enabled = MySession.Static.IsUserModerator(Sync.MyId);
          this.Controls.Add((MyGuiControlBase) this.m_showPlayersCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
          myGuiControlLabel9.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel9.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_UseTerminals);
          MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
          this.m_canUseTerminals = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_canUseTerminals.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnUseTerminalsChanged);
          this.m_canUseTerminals.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_UseTerminalsToolTip);
          this.m_canUseTerminals.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
          this.m_canUseTerminals.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
          this.Controls.Add((MyGuiControlBase) this.m_canUseTerminals);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
          if (MyPlatformGameSettings.IsIgnorePcuAllowed)
          {
            this.m_currentPosition.Y += 0.045f;
            MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel();
            myGuiControlLabel11.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
            myGuiControlLabel11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
            myGuiControlLabel11.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPaste);
            myGuiControlLabel11.IsAutoEllipsisEnabled = true;
            myGuiControlLabel11.IsAutoScaleEnabled = true;
            MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
            myGuiControlLabel12.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
            this.m_keepOriginalOwnershipOnPasteCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
            this.m_keepOriginalOwnershipOnPasteCheckBox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnKeepOwnershipChanged);
            this.m_keepOriginalOwnershipOnPasteCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_KeepOriginalOwnershipOnPasteTip);
            this.m_keepOriginalOwnershipOnPasteCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.KeepOriginalOwnershipOnPaste);
            this.m_keepOriginalOwnershipOnPasteCheckBox.Enabled = MySession.Static.IsUserSpaceMaster(Sync.MyId);
            this.Controls.Add((MyGuiControlBase) this.m_keepOriginalOwnershipOnPasteCheckBox);
            this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
          }
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel();
          myGuiControlLabel13.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel13.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel13.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZones);
          myGuiControlLabel13.IsAutoEllipsisEnabled = true;
          MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
          myGuiControlLabel14.SetMaxSize(new Vector2(0.25f, float.PositiveInfinity));
          this.m_ignoreSafeZonesCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_ignoreSafeZonesCheckBox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnIgnoreSafeZonesChanged);
          this.m_ignoreSafeZonesCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnoreSafeZonesTip);
          this.m_ignoreSafeZonesCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.IgnoreSafeZones);
          this.m_ignoreSafeZonesCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId);
          this.Controls.Add((MyGuiControlBase) this.m_ignoreSafeZonesCheckBox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
          if (MyPlatformGameSettings.IsIgnorePcuAllowed)
          {
            this.m_currentPosition.Y += 0.045f;
            MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel();
            myGuiControlLabel11.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
            myGuiControlLabel11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
            myGuiControlLabel11.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Pcu);
            MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
            this.m_ignorePcuCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
            this.m_ignorePcuCheckBox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnIgnorePcuChanged);
            this.m_ignorePcuCheckBox.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_IgnorePcuTip);
            this.m_ignorePcuCheckBox.IsChecked = MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.IgnorePcu);
            this.m_ignorePcuCheckBox.Enabled = MySession.Static.IsUserAdmin(Sync.MyId) && (MySession.Static.IsRunningExperimental || (uint) MySession.Static.OnlineMode > 0U);
            this.Controls.Add((MyGuiControlBase) this.m_ignorePcuCheckBox);
            this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
          }
          if (!MySession.Static.IsUserAdmin(Sync.MyId))
            break;
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel();
          myGuiControlLabel15.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel15.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel15.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_TimeOfDay);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel15);
          MyGuiControlLabel myGuiControlLabel16 = new MyGuiControlLabel();
          myGuiControlLabel16.Position = this.m_currentPosition + new Vector2(0.285f, 0.0f);
          myGuiControlLabel16.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
          myGuiControlLabel16.Text = "0.00";
          this.m_timeDeltaValue = myGuiControlLabel16;
          this.Controls.Add((MyGuiControlBase) this.m_timeDeltaValue);
          this.m_currentPosition.Y += 0.035f;
          this.m_timeDelta = new MyGuiControlSlider(new Vector2?(this.m_currentPosition + new Vector2(1f / 1000f, 0.0f)), maxValue: (MySession.Static == null ? 1f : MySession.Static.Settings.SunRotationIntervalMinutes));
          this.m_timeDelta.Size = new Vector2(0.285f, 1f);
          this.m_timeDelta.Value = MyTimeOfDayHelper.TimeOfDay;
          this.m_timeDelta.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          this.m_timeDelta.ValueChanged += new Action<MyGuiControlSlider>(this.TimeDeltaChanged);
          this.m_timeDeltaValue.Text = string.Format("{0:0.00}", (object) this.m_timeDelta.Value);
          this.Controls.Add((MyGuiControlBase) this.m_timeDelta);
          this.m_currentPosition.Y += 0.07f;
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.TrashRemoval:
          this.m_currentPosition.Y += 0.016f;
          bool flag = false;
          if (this.m_trashRemovalCombo == null)
          {
            MyGuiControlCombobox guiControlCombobox = new MyGuiControlCombobox();
            guiControlCombobox.Position = this.m_currentPosition;
            guiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
            this.m_trashRemovalCombo = guiControlCombobox;
            this.m_trashRemovalCombo.AddItem(0L, MyCommonTexts.ScreenDebugAdminMenu_GeneralTabButton);
            this.m_trashRemovalCombo.AddItem(1L, MyCommonTexts.ScreenDebugAdminMenu_VoxelTabButton);
            this.m_trashRemovalCombo.AddItem(2L, MyCommonTexts.ScreenDebugAdminMenu_OtherTabButton);
            this.m_trashRemovalCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnTrashRemovalItemSelected);
            flag = true;
          }
          this.Controls.Add((MyGuiControlBase) this.m_trashRemovalCombo);
          this.m_currentPosition.Y += this.m_trashRemovalCombo.Size.Y + 0.016f;
          this.m_tabs.Add(MyTabControlEnum.General, MyAdminMenuTabFactory.CreateTab((MyGuiScreenBase) this, MyTabControlEnum.General));
          this.m_tabs.Add(MyTabControlEnum.Voxel, MyAdminMenuTabFactory.CreateTab((MyGuiScreenBase) this, MyTabControlEnum.Voxel));
          this.m_tabs.Add(MyTabControlEnum.Other, MyAdminMenuTabFactory.CreateTab((MyGuiScreenBase) this, MyTabControlEnum.Other));
          MyGuiControlStackPanel controlStackPanel1 = new MyGuiControlStackPanel();
          controlStackPanel1.Position = this.m_currentPosition;
          controlStackPanel1.Orientation = MyGuiOrientation.Vertical;
          controlStackPanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          this.m_trashRemovalContentPanel = controlStackPanel1;
          MyGuiControlStackPanel controlStackPanel2 = new MyGuiControlStackPanel();
          controlStackPanel2.Position = Vector2.Zero;
          controlStackPanel2.Orientation = MyGuiOrientation.Horizontal;
          controlStackPanel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          MyGuiControlStackPanel controlStackPanel3 = controlStackPanel2;
          this.m_currentPosition.Y += 0.06f;
          MyGuiControlButton debugButton1 = this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButton, new Action<MyGuiControlButton>(this.OnSubmitButtonClicked), tooltip: new MyStringId?(MyCommonTexts.ScreenDebugAdminMenu_SubmitChangesButtonTooltip), addToControls: false);
          debugButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          debugButton1.PositionX = -0.088f;
          debugButton1.IsAutoScaleEnabled = true;
          debugButton1.IsAutoEllipsisEnabled = true;
          controlStackPanel3.Add((MyGuiControlBase) debugButton1);
          MyGuiControlButton debugButton2 = this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_CancleChangesButton, new Action<MyGuiControlButton>(this.OnCancelButtonClicked), addToControls: false);
          debugButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          debugButton2.PositionX = 0.055f;
          debugButton2.PositionY -= 0.0435f;
          debugButton2.Margin = new Thickness(0.005f, 0.0f, 0.0f, 0.0f);
          debugButton2.IsAutoScaleEnabled = true;
          debugButton2.IsAutoEllipsisEnabled = true;
          controlStackPanel3.Add((MyGuiControlBase) debugButton2);
          controlStackPanel3.UpdateArrange();
          controlStackPanel3.UpdateMeasure();
          this.m_trashRemovalContentPanel.Add((MyGuiControlBase) controlStackPanel3);
          this.m_trashRemovalContentPanel.UpdateArrange();
          this.m_trashRemovalContentPanel.UpdateMeasure();
          this.Controls.Add((MyGuiControlBase) controlStackPanel3);
          this.Controls.Add((MyGuiControlBase) debugButton1);
          this.Controls.Add((MyGuiControlBase) debugButton2);
          this.Controls.Add((MyGuiControlBase) this.m_trashRemovalContentPanel);
          if (flag)
          {
            this.m_trashRemovalCombo.SelectItemByKey(0L);
            break;
          }
          this.OnTrashRemovalItemSelected();
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.CycleObjects:
          this.m_currentPosition.Y += 0.03f;
          MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
          controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.19f), this.m_size.Value.X * 0.73f);
          controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.138f), this.m_size.Value.X * 0.73f);
          controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.305f), this.m_size.Value.X * 0.73f);
          this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
          MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel();
          myGuiControlLabel17.Position = new Vector2(-0.16f, -0.335f);
          myGuiControlLabel17.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel17.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":";
          myGuiControlLabel17.IsAutoScaleEnabled = true;
          myGuiControlLabel17.IsAutoEllipsisEnabled = true;
          MyGuiControlLabel myGuiControlLabel18 = myGuiControlLabel17;
          myGuiControlLabel18.SetMaxWidth(0.065f);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
          MyGuiControlCombobox guiControlCombobox1 = this.AddCombo<MyEntityCyclingOrder>(MyGuiScreenAdminMenu.m_order, new Action<MyEntityCyclingOrder>(this.OnOrderChanged), color: new Vector4?(this.m_labelColor), isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
          guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
          guiControlCombobox1.PositionX = 0.122f;
          guiControlCombobox1.Size = new Vector2(0.21f, 1f);
          this.m_currentPosition.Y += 0.005f;
          MyGuiControlLabel myGuiControlLabel19 = new MyGuiControlLabel();
          myGuiControlLabel19.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel19.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel19.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_SmallGrids);
          MyGuiControlLabel myGuiControlLabel20 = myGuiControlLabel19;
          myGuiControlLabel20.SetMaxWidth(0.065f);
          this.m_onlySmallGridsCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_onlySmallGridsCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSmallGridChanged);
          this.m_onlySmallGridsCheckbox.IsChecked = MyGuiScreenAdminMenu.m_cyclingOptions.OnlySmallGrids;
          this.Controls.Add((MyGuiControlBase) this.m_onlySmallGridsCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlLabel myGuiControlLabel21 = new MyGuiControlLabel();
          myGuiControlLabel21.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel21.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel21.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_LargeGrids);
          MyGuiControlLabel myGuiControlLabel22 = myGuiControlLabel21;
          myGuiControlLabel22.SetMaxWidth(0.065f);
          this.m_onlyLargeGridsCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          this.m_onlyLargeGridsCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnLargeGridChanged);
          this.m_onlyLargeGridsCheckbox.IsChecked = MyGuiScreenAdminMenu.m_cyclingOptions.OnlyLargeGrids;
          this.Controls.Add((MyGuiControlBase) this.m_onlyLargeGridsCheckbox);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel22);
          this.m_currentPosition.Y += 0.12f;
          float y1 = this.m_currentPosition.Y;
          MyGuiControlButton debugButton3 = this.CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_First, (Action<MyGuiControlButton>) (c => this.OnCycleClicked(true, true)));
          debugButton3.PositionX += 3f / 1000f;
          debugButton3.PositionY -= 0.0435f;
          this.m_currentPosition.Y = y1;
          this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Next, (Action<MyGuiControlButton>) (c => this.OnCycleClicked(false, false))).PositionX = -0.088f;
          this.m_currentPosition.Y = y1;
          this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Previous, (Action<MyGuiControlButton>) (c => this.OnCycleClicked(false, true))).PositionX = 0.055f;
          MyGuiControlLabel myGuiControlLabel23 = new MyGuiControlLabel();
          myGuiControlLabel23.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel23.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel23.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + " -";
          myGuiControlLabel23.IsAutoScaleEnabled = true;
          myGuiControlLabel23.IsAutoEllipsisEnabled = true;
          this.m_labelEntityName = myGuiControlLabel23;
          this.m_labelEntityName.SetMaxWidth(0.35f);
          this.Controls.Add((MyGuiControlBase) this.m_labelEntityName);
          this.m_currentPosition.Y += 0.035f;
          MyGuiControlLabel myGuiControlLabel24 = new MyGuiControlLabel();
          myGuiControlLabel24.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
          myGuiControlLabel24.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel24.Text = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), MyGuiScreenAdminMenu.m_entityId == 0L ? (object) "-" : (object) MyGuiScreenAdminMenu.m_metricValue.ToString()).ToString();
          this.m_labelCurrentIndex = myGuiControlLabel24;
          this.Controls.Add((MyGuiControlBase) this.m_labelCurrentIndex);
          this.m_currentPosition.Y += 0.208f;
          float y2 = this.m_currentPosition.Y;
          this.m_removeItemButton = this.CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Remove, (Action<MyGuiControlButton>) (c => this.OnEntityOperationClicked(MyEntityList.EntityListAction.Remove)));
          this.m_removeItemButton.PositionX += 3f / 1000f;
          this.m_currentPosition.Y = y2;
          this.m_stopItemButton = this.CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_Stop, (Action<MyGuiControlButton>) (c => this.OnEntityOperationClicked(MyEntityList.EntityListAction.Stop)));
          this.m_stopItemButton.PositionX += 3f / 1000f;
          this.m_stopItemButton.PositionY += 0.0435f;
          this.m_currentPosition.Y = y2;
          this.m_depowerItemButton = this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_Depower, (Action<MyGuiControlButton>) (c => this.OnEntityOperationClicked(MyEntityList.EntityListAction.Depower)));
          this.m_depowerItemButton.PositionX += 3f / 1000f;
          this.m_depowerItemButton.PositionY += 0.087f;
          this.m_currentPosition.Y += 0.125f;
          float y3 = this.m_currentPosition.Y;
          this.m_currentPosition.Y = y3;
          MyGuiControlButton debugButton4 = this.CreateDebugButton(0.284f, MyCommonTexts.SpectatorControls_None, new Action<MyGuiControlButton>(this.OnPlayerControl), tooltip: new MyStringId?(MySpaceTexts.SpectatorControls_None_Desc), isAutoScaleEnabled: true);
          debugButton4.PositionX += 3f / 1000f;
          this.m_currentPosition.Y = y3;
          MyGuiControlButton debugButton5 = this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, new Action<MyGuiControlButton>(this.OnTeleportButton), MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null && MySession.Static.IsUserSpaceMaster(Sync.MyId), new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip), isAutoScaleEnabled: true);
          debugButton5.PositionX += 3f / 1000f;
          debugButton5.PositionY += 0.0435f;
          debugButton5.TextScale = debugButton4.TextScale;
          bool enabled = !Sync.IsServer;
          this.CreateDebugButton(0.284f, MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything, new Action<MyGuiControlButton>(this.OnReplicateEverything), enabled, new MyStringId?(enabled ? MyCommonTexts.ScreenDebugAdminMenu_ReplicateEverything_Tooltip : MySpaceTexts.ScreenDebugAdminMenu_ReplicateEverythingServer_Tooltip)).PositionX += 3f / 1000f;
          debugButton5.PositionY += 0.0435f;
          this.OnOrderChanged(MyGuiScreenAdminMenu.m_order);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.EntityList:
          this.m_currentPosition.Y += 0.095f;
          MyGuiControlLabel myGuiControlLabel25 = new MyGuiControlLabel();
          myGuiControlLabel25.Position = new Vector2(-0.16f, -0.334f);
          myGuiControlLabel25.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel25.Text = MyTexts.GetString(MyCommonTexts.Select) + ":";
          myGuiControlLabel25.IsAutoEllipsisEnabled = true;
          myGuiControlLabel25.IsAutoScaleEnabled = true;
          MyGuiControlLabel myGuiControlLabel26 = myGuiControlLabel25;
          myGuiControlLabel26.SetMaxWidth(0.064f);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel26);
          this.m_currentPosition.Y -= 0.065f;
          this.m_entityTypeCombo = this.AddCombo<MyEntityList.MyEntityTypeEnum>(this.m_selectedType, new Action<MyEntityList.MyEntityTypeEnum>(this.ValueChanged), color: new Vector4?(this.m_labelColor));
          this.m_entityTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
          this.m_entityTypeCombo.PositionX = 0.122f;
          this.m_entityTypeCombo.Size = new Vector2(0.21f, 1f);
          MyGuiControlLabel myGuiControlLabel27 = new MyGuiControlLabel();
          myGuiControlLabel27.Position = new Vector2(-0.16f, -0.284f);
          myGuiControlLabel27.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel27.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SortBy) + ":";
          myGuiControlLabel27.IsAutoScaleEnabled = true;
          myGuiControlLabel27.IsAutoEllipsisEnabled = true;
          MyGuiControlLabel myGuiControlLabel28 = myGuiControlLabel27;
          myGuiControlLabel28.SetMaxWidth(0.064f);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel28);
          this.m_entitySortCombo = this.AddCombo<MyEntityList.MyEntitySortOrder>(this.m_selectedSort, new Action<MyEntityList.MyEntitySortOrder>(this.ValueChanged), color: new Vector4?(this.m_labelColor), isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
          this.m_entitySortCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
          this.m_entitySortCombo.PositionX = 0.122f;
          this.m_entitySortCombo.Size = new Vector2(0.21f, 1f);
          MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
          controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.231f), this.m_size.Value.X * 0.73f);
          this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
          MyGuiControlLabel myGuiControlLabel29 = new MyGuiControlLabel();
          myGuiControlLabel29.Position = new Vector2(-0.153f, -0.205f);
          myGuiControlLabel29.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel29.Text = MyTexts.GetString(MySpaceTexts.SafeZone_ListOfEntities);
          MyGuiControlLabel myGuiControlLabel30 = myGuiControlLabel29;
          MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(new Vector2(myGuiControlLabel30.PositionX - 0.0085f, myGuiControlLabel30.Position.Y - 0.005f)), new Vector2?(new Vector2(0.2865f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
          myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
          this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel30);
          this.m_currentPosition.Y += 0.065f;
          this.m_entityListbox = new MyGuiControlListbox(new Vector2?(Vector2.Zero), MyGuiControlListboxStyleEnum.Blueprints);
          this.m_entityListbox.Size = new Vector2(num2, 0.0f);
          this.m_entityListbox.Enabled = true;
          this.m_entityListbox.VisibleRowsCount = 12;
          this.m_entityListbox.Position = this.m_entityListbox.Size / 2f + this.m_currentPosition;
          this.m_entityListbox.ItemClicked += new Action<MyGuiControlListbox>(this.EntityListItemClicked);
          this.m_entityListbox.MultiSelect = true;
          MyGuiControlSeparatorList controlSeparatorList4 = new MyGuiControlSeparatorList();
          controlSeparatorList4.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.271f), this.m_size.Value.X * 0.73f);
          this.Controls.Add((MyGuiControlBase) controlSeparatorList4);
          this.m_currentPosition = this.m_entityListbox.GetPositionAbsoluteBottomLeft();
          this.m_currentPosition.Y += 0.045f;
          MyGuiControlButton debugButton6 = this.CreateDebugButton(0.14f, MyCommonTexts.SpectatorControls_None, new Action<MyGuiControlButton>(this.OnPlayerControl), tooltip: new MyStringId?(MySpaceTexts.SpectatorControls_None_Desc), isAutoScaleEnabled: true);
          debugButton6.PositionX = -0.088f;
          MyGuiControlButton debugButton7 = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_TeleportHere, new Action<MyGuiControlButton>(this.OnTeleportButton), MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Parent == null, new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_TeleportHereToolTip), isAutoScaleEnabled: true);
          debugButton7.PositionX = 0.055f;
          debugButton7.PositionY = debugButton6.PositionY;
          float y4 = this.m_currentPosition.Y;
          this.m_currentPosition.Y = y4;
          this.m_stopItemButton = this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Stop, (Action<MyGuiControlButton>) (c => this.OnEntityListActionClicked(MyEntityList.EntityListAction.Stop)));
          this.m_stopItemButton.PositionX = -0.088f;
          this.m_stopItemButton.PositionY -= 0.0435f;
          this.m_currentPosition.Y = y4;
          this.m_depowerItemButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Depower, (Action<MyGuiControlButton>) (c => this.OnEntityListActionClicked(MyEntityList.EntityListAction.Depower)));
          this.m_depowerItemButton.PositionX = 0.055f;
          this.m_depowerItemButton.PositionY = this.m_stopItemButton.PositionY;
          this.m_removeItemButton = this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, (Action<MyGuiControlButton>) (c => this.OnEntityListActionClicked(MyEntityList.EntityListAction.Remove)));
          this.m_removeItemButton.PositionX -= 0.068f;
          this.m_removeItemButton.PositionY -= 0.0435f;
          MyGuiControlButton debugButton8 = this.CreateDebugButton(0.14f, MySpaceTexts.buttonRefresh, new Action<MyGuiControlButton>(this.OnRefreshButton), tooltip: new MyStringId?(MySpaceTexts.ProgrammableBlock_ButtonRefreshScripts));
          debugButton8.PositionX += 0.075f;
          debugButton8.PositionY = this.m_removeItemButton.PositionY;
          MyGuiControlButton debugButton9 = this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugAdminMenu_RemoveOwner, new Action<MyGuiControlButton>(this.OnRemoveOwnerButton), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_RemoveOwnerToolTip), isAutoScaleEnabled: true);
          debugButton9.PositionX += 3f / 1000f;
          debugButton9.PositionY -= 0.087f;
          this.Controls.Add((MyGuiControlBase) this.m_entityListbox);
          this.ValueChanged((MyEntityList.MyEntityTypeEnum) this.m_entityTypeCombo.GetSelectedKey());
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.SafeZones:
          this.RecreateSafeZonesControls(ref controlPadding, separatorSize, num2);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.GlobalSafeZone:
          this.RecreateGlobalSafeZoneControls(ref controlPadding, separatorSize, num2);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.ReplayTool:
          this.RecreateReplayToolControls(ref controlPadding, separatorSize, num2);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.Economy:
          this.m_currentPosition.X += 3f / 1000f;
          this.m_currentPosition.Y += 0.03f;
          MyGuiControlLabel myGuiControlLabel31 = new MyGuiControlLabel();
          myGuiControlLabel31.Position = this.m_currentPosition;
          myGuiControlLabel31.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel31.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Label);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel31);
          this.m_currentPosition.Y += 0.05f;
          ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
          this.m_playerCount = onlinePlayers.Count;
          this.m_factionCount = MySession.Static.Factions.Count<KeyValuePair<long, MyFaction>>();
          this.m_onlinePlayerCombo = new MyGuiControlCombobox();
          this.m_onlinePlayerCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_Player_Tooltip));
          List<MyGuiScreenAdminMenu.MyIdNamePair> myIdNamePairList1 = new List<MyGuiScreenAdminMenu.MyIdNamePair>();
          List<MyGuiScreenAdminMenu.MyIdNamePair> myIdNamePairList2 = new List<MyGuiScreenAdminMenu.MyIdNamePair>();
          MyGuiScreenAdminMenu.MyIdNamePairComparer namePairComparer = new MyGuiScreenAdminMenu.MyIdNamePairComparer();
          foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
            myIdNamePairList1.Add(new MyGuiScreenAdminMenu.MyIdNamePair()
            {
              Id = myPlayer.Identity.IdentityId,
              Name = myPlayer.DisplayName
            });
          foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
            myIdNamePairList2.Add(new MyGuiScreenAdminMenu.MyIdNamePair()
            {
              Id = faction.Key,
              Name = faction.Value.Tag
            });
          myIdNamePairList1.Sort((IComparer<MyGuiScreenAdminMenu.MyIdNamePair>) namePairComparer);
          myIdNamePairList2.Sort((IComparer<MyGuiScreenAdminMenu.MyIdNamePair>) namePairComparer);
          int num4 = 0;
          foreach (MyGuiScreenAdminMenu.MyIdNamePair myIdNamePair in myIdNamePairList1)
          {
            this.m_onlinePlayerCombo.AddItem(myIdNamePair.Id, myIdNamePair.Name, new int?(num4));
            ++num4;
          }
          foreach (MyGuiScreenAdminMenu.MyIdNamePair myIdNamePair in myIdNamePairList2)
          {
            this.m_onlinePlayerCombo.AddItem(myIdNamePair.Id, myIdNamePair.Name, new int?(num4));
            ++num4;
          }
          this.m_onlinePlayerCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnlinePlayerCombo_ItemSelected);
          this.m_onlinePlayerCombo.SelectItemByIndex(-1);
          this.m_onlinePlayerCombo.Position = this.m_currentPosition + new Vector2(0.14f, 0.0f);
          this.Controls.Add((MyGuiControlBase) this.m_onlinePlayerCombo);
          this.m_currentPosition.Y += 0.04f;
          string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
          string texture = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
          Vector2 fromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(new Vector2(1f));
          float num5 = fromNormalizedSize.X / fromNormalizedSize.Y;
          Vector2 vector2_1 = new Vector2(0.28f, 0.0033f);
          Vector2 vector2_2 = new Vector2(0.018f, num5 * 0.018f);
          float num6 = vector2_2.X + 0.01f;
          MyGuiControlLabel myGuiControlLabel32 = new MyGuiControlLabel();
          myGuiControlLabel32.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel32.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CurrentBalance;
          myGuiControlLabel32.IsAutoEllipsisEnabled = false;
          myGuiControlLabel32.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel32);
          MyGuiControlLabel myGuiControlLabel33 = new MyGuiControlLabel();
          myGuiControlLabel33.Position = this.m_currentPosition + new Vector2(0.28f - num6, 0.0f);
          myGuiControlLabel33.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          myGuiControlLabel33.Text = MyBankingSystem.GetFormatedValue(0L);
          myGuiControlLabel33.IsAutoEllipsisEnabled = false;
          myGuiControlLabel33.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.m_labelCurrentBalanceValue = myGuiControlLabel33;
          this.Controls.Add((MyGuiControlBase) this.m_labelCurrentBalanceValue);
          MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
          myGuiControlImage1.Position = this.m_currentPosition + vector2_1;
          myGuiControlImage1.Size = vector2_2;
          myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
          myGuiControlImage2.SetTexture(texture);
          this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel34 = new MyGuiControlLabel();
          myGuiControlLabel34.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel34.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_ChangeBalance;
          myGuiControlLabel34.IsAutoEllipsisEnabled = false;
          myGuiControlLabel34.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel34);
          this.m_addCurrencyTextbox = new MyGuiControlTextbox(defaultText: "0", type: MyGuiControlTextboxType.DigitsOnly);
          this.m_addCurrencyTextbox.Position = this.m_currentPosition + new Vector2((float) (0.217999994754791 - (double) num6 / 2.0), 0.0f);
          this.m_addCurrencyTextbox.Size = new Vector2(this.m_addCurrencyTextbox.Size.X * 0.4f - num6, this.m_addCurrencyTextbox.Size.Y);
          this.m_addCurrencyTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.AddCurrency_TextChanged);
          this.Controls.Add((MyGuiControlBase) this.m_addCurrencyTextbox);
          MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage();
          myGuiControlImage3.Position = this.m_currentPosition + vector2_1;
          myGuiControlImage3.Size = vector2_2;
          myGuiControlImage3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          MyGuiControlImage myGuiControlImage4 = myGuiControlImage3;
          myGuiControlImage4.SetTexture(texture);
          this.Controls.Add((MyGuiControlBase) myGuiControlImage4);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel35 = new MyGuiControlLabel();
          myGuiControlLabel35.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel35.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_FinalBalance;
          myGuiControlLabel35.IsAutoEllipsisEnabled = false;
          myGuiControlLabel35.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel35);
          MyGuiControlLabel myGuiControlLabel36 = new MyGuiControlLabel();
          myGuiControlLabel36.Position = this.m_currentPosition + new Vector2(0.28f - num6, 0.0f);
          myGuiControlLabel36.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          myGuiControlLabel36.Text = MyBankingSystem.GetFormatedValue(0L);
          myGuiControlLabel36.IsAutoEllipsisEnabled = false;
          myGuiControlLabel36.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.m_labelFinalBalanceValue = myGuiControlLabel36;
          this.Controls.Add((MyGuiControlBase) this.m_labelFinalBalanceValue);
          MyGuiControlImage myGuiControlImage5 = new MyGuiControlImage();
          myGuiControlImage5.Position = this.m_currentPosition + vector2_1;
          myGuiControlImage5.Size = vector2_2;
          myGuiControlImage5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          MyGuiControlImage myGuiControlImage6 = myGuiControlImage5;
          myGuiControlImage6.SetTexture(texture);
          this.Controls.Add((MyGuiControlBase) myGuiControlImage6);
          this.m_currentPosition.Y += 0.06f;
          this.m_addCurrencyConfirmButton = new MyGuiControlButton(text: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddCurrency_CoonfirmButton), onButtonClick: new Action<MyGuiControlButton>(this.AddCurrency_ButtonClicked));
          this.m_addCurrencyConfirmButton.Position = this.m_currentPosition + new Vector2(0.14f, 0.0f);
          this.Controls.Add((MyGuiControlBase) this.m_addCurrencyConfirmButton);
          this.m_currentBalance = 0L;
          this.m_finalBalance = 0L;
          this.m_balanceDifference = 0L;
          this.m_currentPosition.Y += 0.1f;
          MyGuiControlLabel myGuiControlLabel37 = new MyGuiControlLabel();
          myGuiControlLabel37.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel37.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel37.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_PlayerLabel);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel37);
          this.m_currentPosition.Y += 0.05f;
          this.m_playerReputationCombo = new MyGuiControlCombobox();
          this.m_playerReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Player_Tooltip));
          List<MyGuiScreenAdminMenu.MyIdNamePair> myIdNamePairList3 = new List<MyGuiScreenAdminMenu.MyIdNamePair>();
          foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
            myIdNamePairList3.Add(new MyGuiScreenAdminMenu.MyIdNamePair()
            {
              Id = myPlayer.Identity.IdentityId,
              Name = myPlayer.DisplayName
            });
          myIdNamePairList3.Sort((IComparer<MyGuiScreenAdminMenu.MyIdNamePair>) namePairComparer);
          int num7 = 0;
          foreach (MyGuiScreenAdminMenu.MyIdNamePair myIdNamePair in myIdNamePairList3)
          {
            this.m_playerReputationCombo.AddItem(myIdNamePair.Id, myIdNamePair.Name, new int?(num7));
            ++num7;
          }
          this.m_playerReputationCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.playerReputationCombo_ItemSelected);
          this.m_playerReputationCombo.SelectItemByIndex(-1);
          this.m_playerReputationCombo.Position = this.m_currentPosition + new Vector2(0.14f, 0.0f);
          this.Controls.Add((MyGuiControlBase) this.m_playerReputationCombo);
          this.m_currentPosition.Y += 0.03f;
          MyGuiControlLabel myGuiControlLabel38 = new MyGuiControlLabel();
          myGuiControlLabel38.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel38.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          myGuiControlLabel38.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FactionLabel);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel38);
          this.m_currentPosition.Y += 0.05f;
          this.m_factionReputationCombo = new MyGuiControlCombobox();
          this.m_factionReputationCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_Faction_Tooltip));
          List<MyGuiScreenAdminMenu.MyIdNamePair> myIdNamePairList4 = new List<MyGuiScreenAdminMenu.MyIdNamePair>();
          foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
            myIdNamePairList4.Add(new MyGuiScreenAdminMenu.MyIdNamePair()
            {
              Id = faction.Key,
              Name = faction.Value.Tag
            });
          myIdNamePairList4.Sort((IComparer<MyGuiScreenAdminMenu.MyIdNamePair>) namePairComparer);
          int num8 = 0;
          foreach (MyGuiScreenAdminMenu.MyIdNamePair myIdNamePair in myIdNamePairList4)
          {
            this.m_factionReputationCombo.AddItem(myIdNamePair.Id, myIdNamePair.Name, new int?(num8));
            ++num8;
          }
          this.m_factionReputationCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.factionReputationCombo_ItemSelected);
          this.m_factionReputationCombo.SelectItemByIndex(-1);
          this.m_factionReputationCombo.Position = this.m_currentPosition + new Vector2(0.14f, 0.0f);
          this.Controls.Add((MyGuiControlBase) this.m_factionReputationCombo);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel39 = new MyGuiControlLabel();
          myGuiControlLabel39.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel39.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_CurrentReputation;
          myGuiControlLabel39.IsAutoEllipsisEnabled = false;
          myGuiControlLabel39.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel39);
          MyGuiControlLabel myGuiControlLabel40 = new MyGuiControlLabel();
          myGuiControlLabel40.Position = this.m_currentPosition + new Vector2(0.28f, 0.0f);
          myGuiControlLabel40.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          myGuiControlLabel40.Text = 0.ToString();
          myGuiControlLabel40.IsAutoEllipsisEnabled = false;
          myGuiControlLabel40.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.m_labelCurrentReputationValue = myGuiControlLabel40;
          this.Controls.Add((MyGuiControlBase) this.m_labelCurrentReputationValue);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel41 = new MyGuiControlLabel();
          myGuiControlLabel41.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel41.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ChangeReputation;
          myGuiControlLabel41.IsAutoEllipsisEnabled = false;
          myGuiControlLabel41.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel41);
          this.m_addReputationTextbox = new MyGuiControlTextbox(defaultText: "0", type: MyGuiControlTextboxType.DigitsOnly);
          this.m_addReputationTextbox.Position = this.m_currentPosition + new Vector2(0.218f, 0.0f);
          this.m_addReputationTextbox.Size = new Vector2(this.m_addReputationTextbox.Size.X * 0.4f, this.m_addCurrencyTextbox.Size.Y);
          this.m_addReputationTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.AddReputation_TextChanged);
          this.Controls.Add((MyGuiControlBase) this.m_addReputationTextbox);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel42 = new MyGuiControlLabel();
          myGuiControlLabel42.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel42.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_FinalReputation;
          myGuiControlLabel42.IsAutoEllipsisEnabled = false;
          myGuiControlLabel42.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel42);
          MyGuiControlLabel myGuiControlLabel43 = new MyGuiControlLabel();
          myGuiControlLabel43.Position = this.m_currentPosition + new Vector2(0.28f, 0.0f);
          myGuiControlLabel43.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
          myGuiControlLabel43.Text = 0.ToString();
          myGuiControlLabel43.IsAutoEllipsisEnabled = false;
          myGuiControlLabel43.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.m_labelFinalReputationValue = myGuiControlLabel43;
          this.Controls.Add((MyGuiControlBase) this.m_labelFinalReputationValue);
          this.m_currentPosition.Y += 0.04f;
          MyGuiControlLabel myGuiControlLabel44 = new MyGuiControlLabel();
          myGuiControlLabel44.Position = this.m_currentPosition + new Vector2(0.0f, 0.0f);
          myGuiControlLabel44.TextEnum = MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate;
          myGuiControlLabel44.IsAutoEllipsisEnabled = false;
          myGuiControlLabel44.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel44);
          MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox();
          guiControlCheckbox.Position = this.m_currentPosition + new Vector2(0.273f, 0.0f);
          guiControlCheckbox.IsChecked = false;
          this.m_addReputationPropagate = guiControlCheckbox;
          this.m_addReputationPropagate.SetToolTip(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ReputationPropagate_Tooltip));
          this.Controls.Add((MyGuiControlBase) this.m_addReputationPropagate);
          this.m_currentPosition.Y += 0.06f;
          this.m_addReputationConfirmButton = new MyGuiControlButton(text: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_AddReputation_ConfirmButton), onButtonClick: new Action<MyGuiControlButton>(this.AddReputation_ButtonClicked));
          this.m_addReputationConfirmButton.Position = this.m_currentPosition + new Vector2(0.14f, 0.0f);
          this.Controls.Add((MyGuiControlBase) this.m_addReputationConfirmButton);
          this.m_currentReputation = 0;
          this.m_finalReputation = 0;
          this.m_reputationDifference = 0;
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.Weather:
          this.RecreateWeatherControls(false);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.Spectator:
          this.RecreateSpectatorControls(false);
          break;
        case MyGuiScreenAdminMenu.MyPageEnum.Match:
          this.RecreateMatchControls(false);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void factionReputationCombo_ItemSelected()
    {
      this.m_factionReputationCombo_SelectedPlayerIdentityId = this.m_factionReputationCombo.GetSelectedKey();
      this.UpdateReputation();
    }

    private void playerReputationCombo_ItemSelected()
    {
      this.m_playerReputationCombo_SelectedPlayerIdentityId = this.m_playerReputationCombo.GetSelectedKey();
      this.UpdateReputation();
    }

    private void UpdateReputation()
    {
      if (this.m_factionReputationCombo_SelectedPlayerIdentityId == 0L || this.m_playerReputationCombo_SelectedPlayerIdentityId == 0L)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyGuiScreenAdminMenu.RequestReputation)), this.m_playerReputationCombo_SelectedPlayerIdentityId, this.m_factionReputationCombo_SelectedPlayerIdentityId);
    }

    [Event(null, 1226)]
    [Reliable]
    [Server]
    private static void RequestReputation(long playerIdentityId, long factionId)
    {
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        return;
      Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerIdentityId, factionId);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long, int>((Func<IMyEventOwner, Action<long, long, int>>) (x => new Action<long, long, int>(MyGuiScreenAdminMenu.RequestReputationCallback)), playerIdentityId, factionId, playerAndFaction.Item2, MyEventContext.Current.Sender);
    }

    [Event(null, 1238)]
    [Reliable]
    [Client]
    private static void RequestReputationCallback(
      long playerIdentityId,
      long factionId,
      int reputation)
    {
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestReputationCallback_Internal(playerIdentityId, factionId, reputation);
    }

    protected void RequestReputationCallback_Internal(
      long playerIdentityId,
      long factionId,
      int reputation)
    {
      if (this.m_playerReputationCombo_SelectedPlayerIdentityId != playerIdentityId || this.m_factionReputationCombo_SelectedPlayerIdentityId != factionId)
        return;
      this.m_currentReputation = this.ClampReputation(reputation);
      this.m_finalReputation = this.ClampReputation(reputation + this.m_reputationDifference);
      this.UpdateReputationTexts();
    }

    protected void UpdateReputationTexts()
    {
      this.m_labelCurrentReputationValue.Text = this.m_currentReputation.ToString();
      this.m_labelFinalReputationValue.Text = this.m_finalReputation.ToString();
    }

    private void AddReputation_ButtonClicked(MyGuiControlButton obj)
    {
      bool isChecked = this.m_addReputationPropagate.IsChecked;
      if (this.m_playerReputationCombo_SelectedPlayerIdentityId == 0L && this.m_factionReputationCombo_SelectedPlayerIdentityId == 0L)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long, int, bool>((Func<IMyEventOwner, Action<long, long, int, bool>>) (x => new Action<long, long, int, bool>(MyGuiScreenAdminMenu.RequestChangeReputation)), this.m_playerReputationCombo_SelectedPlayerIdentityId, this.m_factionReputationCombo_SelectedPlayerIdentityId, this.m_reputationDifference, isChecked);
    }

    [Event(null, 1266)]
    [Reliable]
    [Server]
    private static void RequestChangeReputation(
      long identityId,
      long factionId,
      int reputationChange,
      bool shouldPropagate)
    {
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        return;
      MySession.Static.Factions.AddFactionPlayerReputation(identityId, factionId, reputationChange, shouldPropagate);
      Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(identityId, factionId);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long, int>((Func<IMyEventOwner, Action<long, long, int>>) (x => new Action<long, long, int>(MyGuiScreenAdminMenu.RequestReputationCallback)), identityId, factionId, playerAndFaction.Item2, MyEventContext.Current.Sender);
    }

    private void AddReputation_TextChanged(MyGuiControlTextbox obj)
    {
      this.m_finalReputation = !int.TryParse(obj.Text, out this.m_reputationDifference) ? this.ClampReputation(this.m_currentReputation) : this.ClampReputation(this.m_currentReputation + this.m_reputationDifference);
      this.UpdateReputationTexts();
    }

    private int ClampReputation(int reputation)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      int hostileMax = component.GetHostileMax();
      if (reputation < hostileMax)
        return hostileMax;
      int friendlyMax = component.GetFriendlyMax();
      return reputation > friendlyMax ? friendlyMax : reputation;
    }

    private void OnTrashRemovalItemSelected()
    {
      MyTabControlEnum selectedKey = (MyTabControlEnum) this.m_trashRemovalCombo.GetSelectedKey();
      if (!this.m_tabs.TryGetValue(selectedKey, out MyTabContainer _))
        return;
      MyGuiControlParent control = this.m_tabs[selectedKey].Control;
      control.UpdateArrange();
      control.UpdateMeasure();
      if (this.m_trashRemovalContentPanel.GetControlCount() > 1)
      {
        MyGuiControlBase at = this.m_trashRemovalContentPanel.GetAt(0);
        at.Visible = false;
        this.m_trashRemovalContentPanel.Remove(at);
      }
      this.m_trashRemovalContentPanel.AddAt(0, (MyGuiControlBase) control);
      control.Visible = true;
      this.m_trashRemovalContentPanel.UpdateArrange();
      this.m_trashRemovalContentPanel.UpdateMeasure();
    }

    private void AddCurrency_TextChanged(MyGuiControlTextbox obj)
    {
      this.m_finalBalance = !long.TryParse(obj.Text, out this.m_balanceDifference) ? this.m_currentBalance : this.m_currentBalance + this.m_balanceDifference;
      this.UpdateBalanceTexts();
    }

    protected void UpdateBalanceTexts()
    {
      this.m_labelCurrentBalanceValue.Text = MyBankingSystem.GetFormatedValue(this.m_currentBalance);
      this.m_labelFinalBalanceValue.Text = MyBankingSystem.GetFormatedValue(this.m_finalBalance > 0L ? this.m_finalBalance : 0L);
    }

    private void AddCurrency_ButtonClicked(MyGuiControlButton obj)
    {
      if (this.m_onlinePlayerCombo_SelectedPlayerIdentityId == 0L)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyGuiScreenAdminMenu.RequestChange)), this.m_onlinePlayerCombo_SelectedPlayerIdentityId, this.m_balanceDifference);
    }

    private void OnlinePlayerCombo_ItemSelected()
    {
      int selectedIndex = this.m_onlinePlayerCombo.GetSelectedIndex();
      this.m_onlinePlayerCombo_SelectedPlayerIdentityId = this.m_onlinePlayerCombo.GetSelectedKey();
      if (selectedIndex < this.m_playerCount)
      {
        this.m_isPlayerSelected = true;
        this.m_isFactionSelected = false;
      }
      else if (selectedIndex - this.m_playerCount < this.m_factionCount)
      {
        this.m_isPlayerSelected = false;
        this.m_isFactionSelected = true;
      }
      if (this.m_onlinePlayerCombo_SelectedPlayerIdentityId == 0L)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyGuiScreenAdminMenu.RequestBalance)), this.m_onlinePlayerCombo_SelectedPlayerIdentityId);
    }

    [Event(null, 1370)]
    [Reliable]
    [Server]
    private static void RequestBalance(long accountOwner)
    {
      MyAccountInfo account;
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value) || !MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out account))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyGuiScreenAdminMenu.RequestBalanceCallback)), accountOwner, account.Balance, MyEventContext.Current.Sender);
    }

    [Event(null, 1383)]
    [Reliable]
    [Client]
    private static void RequestBalanceCallback(long accountOwner, long balance) => MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>()?.RequestBalanceCallback_Internal(accountOwner, balance);

    protected void RequestBalanceCallback_Internal(long accountOwner, long balance)
    {
      if (this.m_onlinePlayerCombo_SelectedPlayerIdentityId != accountOwner)
        return;
      this.m_currentBalance = balance;
      this.m_finalBalance = balance + this.m_balanceDifference;
      this.UpdateBalanceTexts();
    }

    [Event(null, 1401)]
    [Reliable]
    [Server]
    private static void RequestChange(long accountOwner, long balanceChange)
    {
      MyAccountInfo account;
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value) || !MyBankingSystem.ChangeBalance(accountOwner, balanceChange) || !MyBankingSystem.Static.TryGetAccountInfo(accountOwner, out account))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyGuiScreenAdminMenu.RequestBalanceCallback)), accountOwner, account.Balance, MyEventContext.Current.Sender);
    }

    private void CircleGps(bool reset, bool forward)
    {
      this.m_onlyLargeGridsCheckbox.Enabled = false;
      this.m_onlySmallGridsCheckbox.Enabled = false;
      this.m_depowerItemButton.Enabled = false;
      this.m_removeItemButton.Enabled = false;
      this.m_stopItemButton.Enabled = false;
      if (MySession.Static == null || MySession.Static.Gpss == null || MySession.Static.LocalHumanPlayer == null)
        return;
      if (forward)
        --this.m_currentGpsIndex;
      else
        ++this.m_currentGpsIndex;
      this.m_gpsList.Clear();
      MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, this.m_gpsList);
      if (this.m_gpsList.Count == 0)
      {
        this.m_currentGpsIndex = 0;
      }
      else
      {
        if (this.m_currentGpsIndex < 0)
          this.m_currentGpsIndex = this.m_gpsList.Count - 1;
        if (this.m_gpsList.Count <= this.m_currentGpsIndex | reset)
          this.m_currentGpsIndex = 0;
        IMyGps gps = this.m_gpsList[this.m_currentGpsIndex];
        Vector3D coords = gps.Coords;
        this.m_labelEntityName.TextToDraw.Clear();
        this.m_labelEntityName.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName));
        this.m_labelEntityName.TextToDraw.Append(string.IsNullOrEmpty(gps.Name) ? "-" : gps.Name);
        this.m_labelCurrentIndex.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), (object) this.m_currentGpsIndex);
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
        Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace(coords + Vector3D.One, 2f, 30);
        MySpectatorCameraController.Static.Position = freePlace.HasValue ? freePlace.Value : coords + Vector3D.One;
        MySpectatorCameraController.Static.Target = coords;
      }
    }

    internal static void RecalcTrash()
    {
      if (Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyGuiScreenAdminMenu.AdminSettings>((Func<IMyEventOwner, Action<MyGuiScreenAdminMenu.AdminSettings>>) (x => new Action<MyGuiScreenAdminMenu.AdminSettings>(MyGuiScreenAdminMenu.UploadSettingsToServer)), new MyGuiScreenAdminMenu.AdminSettings()
      {
        Flags = MySession.Static.Settings.TrashFlags,
        Enable = MySession.Static.Settings.TrashRemovalEnabled,
        BlockCount = MySession.Static.Settings.BlockCountThreshold,
        PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold,
        GridCount = MySession.Static.Settings.OptimalGridCount,
        PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold,
        CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold,
        StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin,
        RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH,
        VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold,
        VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold,
        VoxelAge = MySession.Static.Settings.VoxelAgeThreshold,
        VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled
      });
    }

    private static bool TryAttachCamera(long entityId)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
        return false;
      BoundingSphereD worldVolume = entity.PositionComp.WorldVolume;
      MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
      MySpectatorCameraController.Static.Position = worldVolume.Center + Math.Max((float) worldVolume.Radius, 1f) * Vector3.One;
      MySpectatorCameraController.Static.Target = worldVolume.Center;
      MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = entity;
      return true;
    }

    private void UpdateCyclingAndDepower()
    {
      bool flag = MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Characters && MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.FloatingObjects && MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Gps;
      MyGuiScreenAdminMenu.m_cyclingOptions.Enabled = flag;
      if (this.m_depowerItemButton == null)
        return;
      this.m_depowerItemButton.Enabled = flag;
    }

    private void UpdateSmallLargeGridSelection()
    {
      if (MyGuiScreenAdminMenu.m_currentPage != MyGuiScreenAdminMenu.MyPageEnum.CycleObjects)
        return;
      bool flag = MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Characters && MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.FloatingObjects && MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Gps;
      this.m_removeItemButton.Enabled = true;
      this.m_onlySmallGridsCheckbox.Enabled = flag;
      this.m_onlyLargeGridsCheckbox.Enabled = flag;
    }

    private static void UpdateRemoveAndDepowerButton(
      MyGuiScreenAdminMenu menu,
      long entityId,
      bool disableOverride = false)
    {
      if (menu == null || menu.m_removeItemButton == null)
        return;
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity);
      if (entity == null)
        return;
      bool flag = MyGuiScreenAdminMenu.m_currentPage != MyGuiScreenAdminMenu.MyPageEnum.CycleObjects || MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Gps;
      menu.m_removeItemButton.Enabled = ((!flag ? 0 : (!menu.m_attachIsNpcStation ? 1 : 0)) | (disableOverride ? 1 : 0)) != 0;
      if (menu.m_depowerItemButton != null)
        menu.m_depowerItemButton.Enabled = ((!(entity is MyCubeGrid & flag) ? 0 : (!menu.m_attachIsNpcStation ? 1 : 0)) | (disableOverride ? 1 : 0)) != 0;
      if (menu.m_stopItemButton != null)
        menu.m_stopItemButton.Enabled = ((((entity == null ? 0 : (!(entity is MyVoxelBase) ? 1 : 0)) & (flag ? 1 : 0)) == 0 ? 0 : (!menu.m_attachIsNpcStation ? 1 : 0)) | (disableOverride ? 1 : 0)) != 0;
      if (MyGuiScreenAdminMenu.m_currentPage != MyGuiScreenAdminMenu.MyPageEnum.CycleObjects)
        return;
      string str = entity is MyVoxelBase ? MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + ((MyVoxelBase) entity).StorageName : MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_EntityName) + (entity == null ? "-" : entity.DisplayName);
      if (!(menu.m_labelEntityName.Text != str))
        return;
      menu.m_labelEntityName.Text = str;
      menu.m_labelEntityName.SetMaxWidth(0.27f);
      menu.m_labelEntityName.DoEllipsisAndScaleAdjust(true, 0.8f, true);
    }

    [Event(null, 1612)]
    [Reliable]
    [Server]
    private static void RequestSettingFromServer_Implementation() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyGuiScreenAdminMenu.AdminSettings>((Func<IMyEventOwner, Action<MyGuiScreenAdminMenu.AdminSettings>>) (x => new Action<MyGuiScreenAdminMenu.AdminSettings>(MyGuiScreenAdminMenu.DownloadSettingFromServer)), new MyGuiScreenAdminMenu.AdminSettings()
    {
      Flags = MySession.Static.Settings.TrashFlags,
      Enable = MySession.Static.Settings.TrashRemovalEnabled,
      BlockCount = MySession.Static.Settings.BlockCountThreshold,
      PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold,
      GridCount = MySession.Static.Settings.OptimalGridCount,
      PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold,
      CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold,
      AdminSettingsFlags = MySession.Static.RemoteAdminSettings.GetValueOrDefault<ulong, AdminSettingsEnum>(MyEventContext.Current.Sender.Value, AdminSettingsEnum.None),
      StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin,
      RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH,
      VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold,
      VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold,
      VoxelAge = MySession.Static.Settings.VoxelAgeThreshold,
      VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled
    }, MyEventContext.Current.Sender);

    private void ValueChanged(MyEntityList.MyEntitySortOrder selectedOrder)
    {
      MyGuiScreenAdminMenu.m_invertOrder = this.m_selectedSort == selectedOrder && !MyGuiScreenAdminMenu.m_invertOrder;
      this.m_selectedSort = selectedOrder;
      List<MyEntityList.MyEntityListInfoItem> items = new List<MyEntityList.MyEntityListInfoItem>(this.m_entityListbox.Items.Count);
      foreach (MyGuiControlListbox.Item obj in this.m_entityListbox.Items)
        items.Add((MyEntityList.MyEntityListInfoItem) obj.UserData);
      MyEntityList.SortEntityList(selectedOrder, ref items, MyGuiScreenAdminMenu.m_invertOrder);
      this.m_entityListbox.Items.Clear();
      MyEntityList.MyEntityTypeEnum selectedKey = (MyEntityList.MyEntityTypeEnum) this.m_entityTypeCombo.GetSelectedKey();
      switch (selectedKey)
      {
        case MyEntityList.MyEntityTypeEnum.Grids:
        case MyEntityList.MyEntityTypeEnum.LargeGrids:
          break;
        default:
          1 = selectedKey == MyEntityList.MyEntityTypeEnum.SmallGrids ? 1 : 0;
          break;
      }
      foreach (MyEntityList.MyEntityListInfoItem entityListInfoItem in items)
        this.m_entityListbox.Add(new MyGuiControlListbox.Item(MyEntityList.GetFormattedDisplayName(selectedOrder, entityListInfoItem), MyEntityList.GetDescriptionText(entityListInfoItem), userData: ((object) entityListInfoItem)));
    }

    private void EntityListItemClicked(MyGuiControlListbox myGuiControlListbox)
    {
      if (myGuiControlListbox.SelectedItems.Count <= 0)
        return;
      MyEntityList.MyEntityListInfoItem userData = (MyEntityList.MyEntityListInfoItem) myGuiControlListbox.SelectedItems[myGuiControlListbox.SelectedItems.Count - 1].UserData;
      this.m_attachCamera = userData.EntityId;
      if (!MyGuiScreenAdminMenu.TryAttachCamera(userData.EntityId))
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(userData.Position + Vector3.One * 50f));
      if (this.m_attachCamera == 0L)
        return;
      MyGuiScreenAdminMenu.UpdateRemoveAndDepowerButton(this, this.m_attachCamera, true);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyGuiScreenAdminMenu.AskIsValidForEdit_Server)), this.m_attachCamera);
    }

    [Event(null, 1690)]
    [Reliable]
    [Server]
    private static void AskIsValidForEdit_Server(long entityId)
    {
      bool canEdit = true;
      if (MySession.Static != null && MySession.Static.Factions.GetStationByGridId(entityId) != null)
        canEdit = false;
      if (MyEventContext.Current.IsLocallyInvoked)
        MyGuiScreenAdminMenu.AskIsValidForEdit_Reponse(entityId, canEdit);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, bool>((Func<IMyEventOwner, Action<long, bool>>) (x => new Action<long, bool>(MyGuiScreenAdminMenu.AskIsValidForEdit_Reponse)), entityId, canEdit, MyEventContext.Current.Sender);
    }

    [Event(null, 1707)]
    [Reliable]
    [Client]
    private static void AskIsValidForEdit_Reponse(long entityId, bool canEdit)
    {
      MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
      if (firstScreenOfType == null || firstScreenOfType.m_attachCamera != entityId)
        return;
      firstScreenOfType.m_attachIsNpcStation = !canEdit;
      MyGuiScreenAdminMenu.UpdateRemoveAndDepowerButton(firstScreenOfType, firstScreenOfType.m_attachCamera);
    }

    private void TimeDeltaChanged(MyGuiControlSlider slider)
    {
      MyTimeOfDayHelper.UpdateTimeOfDay(slider.Value);
      this.m_timeDeltaValue.Text = string.Format("{0:0.00}", (object) slider.Value);
    }

    public void ValueChanged(MyEntityList.MyEntityTypeEnum myEntityTypeEnum)
    {
      this.m_selectedType = myEntityTypeEnum;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyEntityList.MyEntityTypeEnum, bool>((Func<IMyEventOwner, Action<MyEntityList.MyEntityTypeEnum, bool>>) (x => new Action<MyEntityList.MyEntityTypeEnum, bool>(MyGuiScreenAdminMenu.EntityListRequest)), myEntityTypeEnum, false);
    }

    public static void RequestEntityList(MyEntityList.MyEntityTypeEnum myEntityTypeEnum) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyEntityList.MyEntityTypeEnum, bool>((Func<IMyEventOwner, Action<MyEntityList.MyEntityTypeEnum, bool>>) (x => new Action<MyEntityList.MyEntityTypeEnum, bool>(MyGuiScreenAdminMenu.EntityListRequest)), myEntityTypeEnum, true);

    private void OnModeComboSelect()
    {
      if (MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.TrashRemoval && this.m_unsavedTrashSettings)
      {
        if (MyGuiScreenAdminMenu.m_currentPage == (MyGuiScreenAdminMenu.MyPageEnum) this.m_modeCombo.GetSelectedKey())
          return;
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.FinishTrashUnsavedTabChange);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash), callback: callback));
      }
      else
        this.NewTabSelected();
    }

    private void NewTabSelected()
    {
      MyGuiScreenAdminMenu.m_currentPage = (MyGuiScreenAdminMenu.MyPageEnum) this.m_modeCombo.GetSelectedKey();
      this.RecreateControls(false);
    }

    private void FinishTrashUnsavedTabChange(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.StoreTrashSettings_RealToTmp();
        this.NewTabSelected();
      }
      else
        this.m_modeCombo.SelectItemByKey((long) MyGuiScreenAdminMenu.m_currentPage);
    }

    private void StoreTrashSettings_RealToTmp()
    {
      this.m_newSettings.Flags = MySession.Static.Settings.TrashFlags;
      this.m_newSettings.Enable = MySession.Static.Settings.TrashRemovalEnabled;
      this.m_newSettings.BlockCount = MySession.Static.Settings.BlockCountThreshold;
      this.m_newSettings.PlayerDistance = MySession.Static.Settings.PlayerDistanceThreshold;
      this.m_newSettings.GridCount = MySession.Static.Settings.OptimalGridCount;
      this.m_newSettings.PlayerInactivity = MySession.Static.Settings.PlayerInactivityThreshold;
      this.m_newSettings.CharacterRemovalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
      this.m_newSettings.StopGridsPeriod = MySession.Static.Settings.StopGridsPeriodMin;
      this.m_newSettings.RemoveOldIdentities = MySession.Static.Settings.RemoveOldIdentitiesH;
      this.m_newSettings.VoxelDistanceFromPlayer = MySession.Static.Settings.VoxelPlayerDistanceThreshold;
      this.m_newSettings.VoxelDistanceFromGrid = MySession.Static.Settings.VoxelGridDistanceThreshold;
      this.m_newSettings.VoxelAge = MySession.Static.Settings.VoxelAgeThreshold;
      this.m_newSettings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
      this.m_newSettings.AfkTimeout = MySession.Static.Settings.AFKTimeountMin;
      this.m_unsavedTrashSettings = false;
    }

    private void StoreTrashSettings_TmpToReal()
    {
      if (Sync.IsServer && ((this.m_newSettings.Flags & MyTrashRemovalFlags.RevertBoulders) != MyTrashRemovalFlags.None && (MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) == MyTrashRemovalFlags.None || this.m_newSettings.VoxelAge != MySession.Static.Settings.VoxelAgeThreshold))
      {
        MySessionComponentTrash component = MySession.Static.GetComponent<MySessionComponentTrash>();
        if (component != null)
          component.ClearBoulders = true;
      }
      MySession.Static.Settings.TrashFlags = this.m_newSettings.Flags;
      MySession.Static.Settings.TrashRemovalEnabled = this.m_newSettings.Enable;
      MySession.Static.Settings.BlockCountThreshold = this.m_newSettings.BlockCount;
      MySession.Static.Settings.PlayerDistanceThreshold = this.m_newSettings.PlayerDistance;
      MySession.Static.Settings.OptimalGridCount = this.m_newSettings.GridCount;
      MySession.Static.Settings.PlayerInactivityThreshold = this.m_newSettings.PlayerInactivity;
      MySession.Static.Settings.PlayerCharacterRemovalThreshold = this.m_newSettings.CharacterRemovalThreshold;
      MySession.Static.Settings.StopGridsPeriodMin = this.m_newSettings.StopGridsPeriod;
      MySession.Static.Settings.RemoveOldIdentitiesH = this.m_newSettings.RemoveOldIdentities;
      MySession.Static.Settings.VoxelPlayerDistanceThreshold = this.m_newSettings.VoxelDistanceFromPlayer;
      MySession.Static.Settings.VoxelGridDistanceThreshold = this.m_newSettings.VoxelDistanceFromGrid;
      MySession.Static.Settings.VoxelAgeThreshold = this.m_newSettings.VoxelAge;
      MySession.Static.Settings.VoxelTrashRemovalEnabled = this.m_newSettings.VoxelEnable;
      MySession.Static.Settings.AFKTimeountMin = this.m_newSettings.AfkTimeout;
    }

    private void OnSubmitButtonClicked(MyGuiControlButton obj)
    {
      this.CheckAndStoreTrashTextboxChanges();
      if (MySession.Static.Settings.OptimalGridCount == 0 && MySession.Static.Settings.OptimalGridCount != this.m_newSettings.GridCount)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.FinishTrashSetting);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_GridCountWarning), callback: callback));
      }
      else
        this.FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum.YES);
    }

    private bool CheckAndStoreTrashTextboxChanges()
    {
      MyTabControlEnum selectedKey = (MyTabControlEnum) this.m_trashRemovalCombo.GetSelectedKey();
      if (!this.m_tabs.TryGetValue(selectedKey, out MyTabContainer _))
        return false;
      this.m_unsavedTrashSettings |= this.m_tabs[selectedKey].GetSettings(ref this.m_newSettings);
      return this.m_unsavedTrashSettings;
    }

    private void FinishTrashSetting(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      this.StoreTrashSettings_TmpToReal();
      this.m_unsavedTrashSettings = false;
      MySession.Static.GetComponent<MySessionComponentTrash>()?.SetPlayerAFKTimeout(this.m_newSettings.AfkTimeout);
      MyGuiScreenAdminMenu.RecalcTrash();
      this.RecreateControls(false);
    }

    private void OnCancelButtonClicked(MyGuiControlButton obj)
    {
      this.StoreTrashSettings_RealToTmp();
      this.RecreateControls(false);
      this.m_unsavedTrashSettings = false;
    }

    private void OnCycleClicked(bool reset, bool forward)
    {
      if (MyGuiScreenAdminMenu.m_order != MyEntityCyclingOrder.Gps)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>((Func<IMyEventOwner, Action<MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>>) (x => new Action<MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>(MyGuiScreenAdminMenu.CycleRequest_Implementation)), MyGuiScreenAdminMenu.m_order, reset, forward, MyGuiScreenAdminMenu.m_metricValue, MyGuiScreenAdminMenu.m_entityId, MyGuiScreenAdminMenu.m_cyclingOptions);
      else
        this.CircleGps(reset, forward);
    }

    private void OnPlayerControl(MyGuiControlButton obj)
    {
      this.m_attachCamera = 0L;
      this.m_attachIsNpcStation = false;
      MySessionComponentAnimationSystem.Static.EntitySelectedForDebug = (MyEntity) null;
      MyGuiScreenGamePlay.SetCameraController();
    }

    private void OnTeleportButton(MyGuiControlButton obj)
    {
      if (MySession.Static.CameraController == MySession.Static.LocalCharacter)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.TeleportControlledEntity(MySpectatorCameraController.Static.Position);
    }

    private void OnRefreshButton(MyGuiControlButton obj) => this.RecreateControls(true);

    private void OnRemoveOwnerButton(MyGuiControlButton obj)
    {
      HashSet<long> source = new HashSet<long>();
      List<long> longList = new List<long>();
      foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
      {
        long owner = ((MyEntityList.MyEntityListInfoItem) selectedItem.UserData).Owner;
        if (owner == 0L)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("No owner!"), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
        else if (MySession.Static != null && MySession.Static.ControlledEntity != null && owner == MySession.Static.ControlledEntity.ControllerInfo.Controller.Player.Identity.IdentityId)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Cannot remove yourself!"), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
        }
        else
        {
          MyPlayer.PlayerId result;
          MyPlayer player;
          if (MySession.Static.Players.TryGetPlayerId(owner, out result) && MySession.Static.Players.TryGetPlayerById(result, out player) && MySession.Static.Players.GetOnlinePlayers().Contains(player))
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Cannot remove online player " + player.DisplayName + ", kick him first!"), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
          else
            source.Add(owner);
        }
      }
      List<MyGuiControlListbox.Item> objList = new List<MyGuiControlListbox.Item>();
      foreach (MyGuiControlListbox.Item obj1 in this.m_entityListbox.Items)
      {
        if (source.Contains(((MyEntityList.MyEntityListInfoItem) obj1.UserData).Owner))
        {
          objList.Add(obj1);
          longList.Add(((MyEntityList.MyEntityListInfoItem) obj1.UserData).EntityId);
        }
      }
      this.m_entityListbox.SelectedItems.Clear();
      foreach (MyGuiControlListbox.Item obj1 in objList)
        this.m_entityListbox.Items.Remove(obj1);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<List<long>, List<long>>((Func<IMyEventOwner, Action<List<long>, List<long>>>) (x => new Action<List<long>, List<long>>(MyGuiScreenAdminMenu.RemoveOwner_Implementation)), source.ToList<long>(), longList);
    }

    protected void OnOrderChanged(MyEntityCyclingOrder obj)
    {
      MyGuiScreenAdminMenu.m_order = obj;
      this.UpdateSmallLargeGridSelection();
      this.UpdateCyclingAndDepower();
      this.OnCycleClicked(true, true);
    }

    private bool ValidCharacter(long entityId)
    {
      MyEntity entity;
      MyPlayer.PlayerId result;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) || !(entity is MyCharacter myCharacter) || (!Sync.Players.TryGetPlayerId(myCharacter.ControllerInfo.ControllingIdentityId, out result) || Sync.Players.GetPlayerById(result) == null))
        return true;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_RemoveCharacterNotification)));
      return false;
    }

    private void OnEntityListActionClicked(MyEntityList.EntityListAction action)
    {
      List<long> longList = new List<long>();
      List<MyGuiControlListbox.Item> objList = new List<MyGuiControlListbox.Item>();
      foreach (MyGuiControlListbox.Item selectedItem in this.m_entityListbox.SelectedItems)
      {
        if (!this.ValidCharacter(((MyEntityList.MyEntityListInfoItem) selectedItem.UserData).EntityId))
          return;
        longList.Add(((MyEntityList.MyEntityListInfoItem) selectedItem.UserData).EntityId);
        objList.Add(selectedItem);
      }
      if (action == MyEntityList.EntityListAction.Remove)
      {
        this.m_entityListbox.SelectedItems.Clear();
        foreach (MyGuiControlListbox.Item obj in objList)
          this.m_entityListbox.Items.Remove(obj);
        this.m_entityListbox.ScrollToolbarToTop();
        foreach (long entityId in longList)
        {
          MyEntity entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) && entity is MyVoxelBase myVoxelBase && !myVoxelBase.SyncFlag)
            myVoxelBase.Close();
        }
      }
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<List<long>, MyEntityList.EntityListAction>((Func<IMyEventOwner, Action<List<long>, MyEntityList.EntityListAction>>) (x => new Action<List<long>, MyEntityList.EntityListAction>(MyGuiScreenAdminMenu.ProceedEntitiesAction_Implementation)), longList, action);
    }

    private void OnEntityOperationClicked(MyEntityList.EntityListAction action)
    {
      if (this.m_attachCamera == 0L || !this.ValidCharacter(this.m_attachCamera))
        return;
      MyEntity entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_attachCamera, out entity) && entity is MyVoxelBase myVoxelBase)
        Sandbox.Game.Entities.MyEntities.SendCloseRequest((IMyEntity) myVoxelBase);
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, MyEntityList.EntityListAction>((Func<IMyEventOwner, Action<long, MyEntityList.EntityListAction>>) (x => new Action<long, MyEntityList.EntityListAction>(MyGuiScreenAdminMenu.ProceedEntity_Implementation)), this.m_attachCamera, action);
    }

    private void RaiseAdminSettingsChanged() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<AdminSettingsEnum, ulong>((Func<IMyEventOwner, Action<AdminSettingsEnum, ulong>>) (x => new Action<AdminSettingsEnum, ulong>(MyGuiScreenAdminMenu.AdminSettingsChanged)), MySession.Static.AdminSettings, Sync.MyId);

    private void OnReplicateEverything(MyGuiControlButton button) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.ReplicateEverything_Implementation)));

    private void OnEnableAdminModeChanged(MyGuiControlCheckbox checkbox) => MySession.Static.EnableCreativeTools(Sync.MyId, checkbox.IsChecked);

    private void OnInvulnerableChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.Invulnerable;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.Invulnerable;
      this.RaiseAdminSettingsChanged();
    }

    private void OnUntargetableChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.Untargetable;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.Untargetable;
      this.RaiseAdminSettingsChanged();
    }

    private void OnKeepOwnershipChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.KeepOriginalOwnershipOnPaste;
      this.RaiseAdminSettingsChanged();
    }

    private void OnIgnoreSafeZonesChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.IgnoreSafeZones;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnoreSafeZones;
      this.RaiseAdminSettingsChanged();
    }

    private void OnIgnorePcuChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.IgnorePcu;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.IgnorePcu;
      this.RaiseAdminSettingsChanged();
    }

    private void OnUseTerminalsChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.UseTerminals;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.UseTerminals;
      this.RaiseAdminSettingsChanged();
    }

    private void OnShowPlayersChanged(MyGuiControlCheckbox checkbox)
    {
      if (checkbox.IsChecked)
        MySession.Static.AdminSettings |= AdminSettingsEnum.ShowPlayers;
      else
        MySession.Static.AdminSettings &= ~AdminSettingsEnum.ShowPlayers;
      this.RaiseAdminSettingsChanged();
    }

    private void OnSmallGridChanged(MyGuiControlCheckbox checkbox)
    {
      MyGuiScreenAdminMenu.m_cyclingOptions.OnlySmallGrids = checkbox.IsChecked;
      if (!MyGuiScreenAdminMenu.m_cyclingOptions.OnlySmallGrids || this.m_onlyLargeGridsCheckbox == null)
        return;
      this.m_onlyLargeGridsCheckbox.IsChecked = false;
    }

    private void OnLargeGridChanged(MyGuiControlCheckbox checkbox)
    {
      MyGuiScreenAdminMenu.m_cyclingOptions.OnlyLargeGrids = checkbox.IsChecked;
      if (!MyGuiScreenAdminMenu.m_cyclingOptions.OnlyLargeGrids)
        return;
      this.m_onlySmallGridsCheckbox.IsChecked = false;
    }

    [Event(null, 2184)]
    [Reliable]
    [Server]
    private static void EntityListRequest(
      MyEntityList.MyEntityTypeEnum selectedType,
      bool requestedBySafeZoneFilter)
    {
      if (!requestedBySafeZoneFilter && (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value)))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        List<MyEntityList.MyEntityListInfoItem> entityList = MyEntityList.GetEntityList(selectedType);
        if (!MyEventContext.Current.IsLocallyInvoked)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<List<MyEntityList.MyEntityListInfoItem>>((Func<IMyEventOwner, Action<List<MyEntityList.MyEntityListInfoItem>>>) (x => new Action<List<MyEntityList.MyEntityListInfoItem>>(MyGuiScreenAdminMenu.EntityListResponse)), entityList, MyEventContext.Current.Sender);
        else
          MyGuiScreenAdminMenu.EntityListResponse(entityList);
      }
    }

    [Event(null, 2201)]
    [Reliable]
    [Server]
    private static void CycleRequest_Implementation(
      MyEntityCyclingOrder order,
      bool reset,
      bool findLarger,
      float metricValue,
      long currentEntityId,
      CyclingOptions options)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (reset)
        {
          metricValue = float.MinValue;
          currentEntityId = 0L;
          findLarger = false;
        }
        MyEntityCycling.FindNext(order, ref metricValue, ref currentEntityId, findLarger, options);
        MyEntity entityByIdOrDefault = Sandbox.Game.Entities.MyEntities.GetEntityByIdOrDefault(currentEntityId);
        Vector3D position = entityByIdOrDefault != null ? entityByIdOrDefault.WorldMatrix.Translation : Vector3D.Zero;
        bool isNpcStation = false;
        if (MySession.Static != null)
        {
          MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
          if (component != null && component.IsGridStation(currentEntityId))
            isNpcStation = true;
        }
        if (MyEventContext.Current.IsLocallyInvoked)
          MyGuiScreenAdminMenu.Cycle_Implementation(metricValue, currentEntityId, position, isNpcStation);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float, long, Vector3D, bool>((Func<IMyEventOwner, Action<float, long, Vector3D, bool>>) (x => new Action<float, long, Vector3D, bool>(MyGuiScreenAdminMenu.Cycle_Implementation)), metricValue, currentEntityId, position, isNpcStation, MyEventContext.Current.Sender);
      }
    }

    [Event(null, 2240)]
    [Server]
    [Reliable]
    private static void RemoveOwner_Implementation(List<long> owners, List<long> entityIds)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        foreach (long entityId in entityIds)
        {
          MyEntity entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
            MyEntityList.ProceedEntityAction(entity, MyEntityList.EntityListAction.Remove);
        }
        foreach (long owner in owners)
        {
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(owner);
          if (identity.Character != null)
            identity.Character.Close();
          foreach (long savedCharacter in identity.SavedCharacters)
          {
            MyCharacter entity;
            if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(savedCharacter, out entity, true) && (!entity.Closed || entity.MarkedForClose))
              entity.Close();
          }
          if (identity != null && identity.BlockLimits.BlocksBuilt == 0)
            MySession.Static.Players.RemoveIdentity(owner);
        }
      }
    }

    [Event(null, 2283)]
    [Server]
    [Reliable]
    private static void ProceedEntitiesAction_Implementation(
      List<long> entityIds,
      MyEntityList.EntityListAction action)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        foreach (long entityId in entityIds)
        {
          MyEntity entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
            MyEntityList.ProceedEntityAction(entity, action);
        }
      }
    }

    [Event(null, 2300)]
    [Reliable]
    [Server]
    private static void UploadSettingsToServer(MyGuiScreenAdminMenu.AdminSettings settings)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != MyTrashRemovalFlags.None && (double) settings.PlayerDistance < (double) MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
          settings.PlayerDistance = (float) MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
        if (MySession.Static.Settings.TrashFlags != settings.Flags)
        {
          MyLog.Default.Info(string.Format("Trash flags changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.Flags));
          if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != MyTrashRemovalFlags.None && (MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) == MyTrashRemovalFlags.None)
          {
            MySessionComponentTrash component = MySession.Static.GetComponent<MySessionComponentTrash>();
            if (component != null)
              component.ClearBoulders = true;
          }
        }
        MySession.Static.Settings.TrashFlags = settings.Flags;
        if (MySession.Static.Settings.TrashRemovalEnabled != settings.Enable)
          MyLog.Default.Info(string.Format("Trash Trash Removal changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.Enable));
        MySession.Static.Settings.TrashRemovalEnabled = settings.Enable;
        if (MySession.Static.Settings.BlockCountThreshold != settings.BlockCount)
          MyLog.Default.Info(string.Format("Trash Block Count changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.BlockCount));
        MySession.Static.Settings.BlockCountThreshold = settings.BlockCount;
        if ((double) MySession.Static.Settings.PlayerDistanceThreshold != (double) settings.PlayerDistance)
          MyLog.Default.Info(string.Format("Trash Player Distance Treshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.PlayerDistance));
        MySession.Static.Settings.PlayerDistanceThreshold = settings.PlayerDistance;
        if (MySession.Static.Settings.OptimalGridCount != settings.GridCount)
          MyLog.Default.Info(string.Format("Trash Optimal Grid Count changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.GridCount));
        MySession.Static.Settings.OptimalGridCount = settings.GridCount;
        if ((double) MySession.Static.Settings.PlayerInactivityThreshold != (double) settings.PlayerInactivity)
          MyLog.Default.Info(string.Format("Trash Player Inactivity Threshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.PlayerInactivity));
        MySession.Static.Settings.PlayerInactivityThreshold = settings.PlayerInactivity;
        if (MySession.Static.Settings.PlayerCharacterRemovalThreshold != settings.CharacterRemovalThreshold)
          MyLog.Default.Info(string.Format("Trash Player Character Removal Threshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.CharacterRemovalThreshold));
        MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.CharacterRemovalThreshold;
        if (MySession.Static.Settings.StopGridsPeriodMin != settings.StopGridsPeriod)
          MyLog.Default.Info(string.Format("Trash Stop Grids Period changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.StopGridsPeriod));
        MySession.Static.Settings.StopGridsPeriodMin = settings.StopGridsPeriod;
        if ((double) MySession.Static.Settings.VoxelPlayerDistanceThreshold != (double) settings.VoxelDistanceFromPlayer)
          MyLog.Default.Info(string.Format("Trash Voxel Player Distance Threshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.VoxelDistanceFromPlayer));
        MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.VoxelDistanceFromPlayer;
        if ((double) MySession.Static.Settings.VoxelGridDistanceThreshold != (double) settings.VoxelDistanceFromGrid)
          MyLog.Default.Info(string.Format("Trash Voxel Grid Distance Threshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.VoxelDistanceFromGrid));
        MySession.Static.Settings.VoxelGridDistanceThreshold = settings.VoxelDistanceFromGrid;
        if (MySession.Static.Settings.VoxelAgeThreshold != settings.VoxelAge)
        {
          MyLog.Default.Info(string.Format("Trash Voxel Age Threshold changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.VoxelAge));
          if ((settings.Flags & MyTrashRemovalFlags.RevertBoulders) != MyTrashRemovalFlags.None)
          {
            MySessionComponentTrash component = MySession.Static.GetComponent<MySessionComponentTrash>();
            if (component != null)
              component.ClearBoulders = true;
          }
        }
        MySession.Static.Settings.VoxelAgeThreshold = settings.VoxelAge;
        if (MySession.Static.Settings.VoxelTrashRemovalEnabled != settings.VoxelEnable)
          MyLog.Default.Info(string.Format("Trash Voxel Trash Removal Enabled changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.VoxelEnable));
        MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.VoxelEnable;
        if (MySession.Static.Settings.RemoveOldIdentitiesH != settings.RemoveOldIdentities)
          MyLog.Default.Info(string.Format("Trash Identities removal time changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) settings.RemoveOldIdentities));
        MySession.Static.Settings.RemoveOldIdentitiesH = settings.RemoveOldIdentities;
      }
    }

    [Event(null, 2386)]
    [Reliable]
    [Server]
    private static void ProceedEntity_Implementation(
      long entityId,
      MyEntityList.EntityListAction action)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyEntity entity;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity))
          return;
        MyEntityList.ProceedEntityAction(entity, action);
      }
    }

    [Event(null, 2400)]
    [Reliable]
    [Server]
    private static void ReplicateEverything_Implementation()
    {
      if (MyEventContext.Current.IsLocallyInvoked)
        return;
      if (!MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        ((MyReplicationServer) Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer).ForceEverything(new Endpoint(MyEventContext.Current.Sender, (byte) 0));
    }

    [Event(null, 2416)]
    [Reliable]
    [Server]
    private static void AdminSettingsChanged(AdminSettingsEnum settings, ulong steamId)
    {
      if (MySession.Static.OnlineMode != MyOnlineModeEnum.OFFLINE && ((settings & AdminSettingsEnum.AdminOnly) > AdminSettingsEnum.None && !MySession.Static.IsUserAdmin(steamId) || !MySession.Static.IsUserModerator(steamId)))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MySession.Static.RemoteAdminSettings[steamId] = settings;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<AdminSettingsEnum, ulong>((Func<IMyEventOwner, Action<AdminSettingsEnum, ulong>>) (x => new Action<AdminSettingsEnum, ulong>(MyGuiScreenAdminMenu.AdminSettingsChangedClient)), settings, steamId);
      }
    }

    [Event(null, 2432)]
    [Reliable]
    [BroadcastExcept]
    private static void AdminSettingsChangedClient(AdminSettingsEnum settings, ulong steamId) => MySession.Static.RemoteAdminSettings[steamId] = settings;

    [Event(null, 2442)]
    [Reliable]
    [Client]
    private static void EntityListResponse(List<MyEntityList.MyEntityListInfoItem> entities)
    {
      if (entities == null)
        return;
      MyGuiScreenSafeZoneFilter firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenSafeZoneFilter>();
      if (firstScreenOfType != null)
      {
        MyGuiControlListbox entityListbox = firstScreenOfType.m_entityListbox;
        entityListbox.Items.Clear();
        MyEntityList.SortEntityList(MyEntityList.MyEntitySortOrder.DisplayName, ref entities, MyGuiScreenAdminMenu.m_invertOrder);
        foreach (MyEntityList.MyEntityListInfoItem entity in entities)
        {
          if (firstScreenOfType.m_selectedSafeZone == null || firstScreenOfType.m_selectedSafeZone.Entities == null || !firstScreenOfType.m_selectedSafeZone.Entities.Contains(entity.EntityId))
          {
            StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(MyEntityList.MyEntitySortOrder.DisplayName, entity);
            if (formattedDisplayName != null)
              entityListbox.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName, userData: ((object) entity.EntityId)));
          }
        }
      }
      else
      {
        MyGuiScreenAdminMenu guiScreenAdminMenu = MyGuiScreenAdminMenu.m_static;
        if (guiScreenAdminMenu == null)
          return;
        MyGuiControlListbox entityListbox = guiScreenAdminMenu.m_entityListbox;
        if (entityListbox == null)
          return;
        entityListbox.Items.Clear();
        MyEntityList.SortEntityList(guiScreenAdminMenu.m_selectedSort, ref entities, MyGuiScreenAdminMenu.m_invertOrder);
        int num = guiScreenAdminMenu.m_selectedType == MyEntityList.MyEntityTypeEnum.Grids || guiScreenAdminMenu.m_selectedType == MyEntityList.MyEntityTypeEnum.LargeGrids ? 1 : (guiScreenAdminMenu.m_selectedType == MyEntityList.MyEntityTypeEnum.SmallGrids ? 1 : 0);
        foreach (MyEntityList.MyEntityListInfoItem entity in entities)
        {
          StringBuilder formattedDisplayName = MyEntityList.GetFormattedDisplayName(guiScreenAdminMenu.m_selectedSort, entity);
          entityListbox.Items.Add(new MyGuiControlListbox.Item(formattedDisplayName, MyEntityList.GetDescriptionText(entity), userData: ((object) entity)));
        }
      }
    }

    [Event(null, 2490)]
    [Reliable]
    [Client]
    private static void Cycle_Implementation(
      float newMetricValue,
      long newEntityId,
      Vector3D position,
      bool isNpcStation)
    {
      MyGuiScreenAdminMenu.m_metricValue = newMetricValue;
      MyGuiScreenAdminMenu.m_entityId = newEntityId;
      if (MyGuiScreenAdminMenu.m_entityId != 0L && !MyGuiScreenAdminMenu.TryAttachCamera(MyGuiScreenAdminMenu.m_entityId))
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(position + Vector3.One * 50f));
      MyGuiScreenAdminMenu firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenAdminMenu>();
      if (firstScreenOfType == null)
        return;
      firstScreenOfType.m_attachCamera = MyGuiScreenAdminMenu.m_entityId;
      firstScreenOfType.m_attachIsNpcStation = isNpcStation;
      MyGuiScreenAdminMenu.UpdateRemoveAndDepowerButton(firstScreenOfType, MyGuiScreenAdminMenu.m_entityId);
      firstScreenOfType.m_labelCurrentIndex?.TextToDraw.Clear().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_CurrentValue), MyGuiScreenAdminMenu.m_entityId == 0L ? (object) "-" : (object) MyGuiScreenAdminMenu.m_metricValue.ToString());
    }

    [Event(null, 2526)]
    [Reliable]
    [Client]
    private static void DownloadSettingFromServer(MyGuiScreenAdminMenu.AdminSettings settings)
    {
      MySession.Static.Settings.TrashFlags = settings.Flags;
      MySession.Static.Settings.TrashRemovalEnabled = settings.Enable;
      MySession.Static.Settings.BlockCountThreshold = settings.BlockCount;
      MySession.Static.Settings.PlayerDistanceThreshold = settings.PlayerDistance;
      MySession.Static.Settings.OptimalGridCount = settings.GridCount;
      MySession.Static.Settings.PlayerInactivityThreshold = settings.PlayerInactivity;
      MySession.Static.Settings.PlayerCharacterRemovalThreshold = settings.CharacterRemovalThreshold;
      MySession.Static.Settings.StopGridsPeriodMin = settings.StopGridsPeriod;
      MySession.Static.Settings.RemoveOldIdentitiesH = settings.RemoveOldIdentities;
      MySession.Static.Settings.VoxelPlayerDistanceThreshold = settings.VoxelDistanceFromPlayer;
      MySession.Static.Settings.VoxelGridDistanceThreshold = settings.VoxelDistanceFromGrid;
      MySession.Static.Settings.VoxelAgeThreshold = settings.VoxelAge;
      MySession.Static.Settings.VoxelTrashRemovalEnabled = settings.VoxelEnable;
      MySession.Static.AdminSettings = settings.AdminSettingsFlags;
      if (MyGuiScreenAdminMenu.m_static == null)
        return;
      MyGuiScreenAdminMenu.m_static.CreateScreen();
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_attachCamera != 0L)
      {
        MyGuiScreenAdminMenu.TryAttachCamera(this.m_attachCamera);
        MyGuiScreenAdminMenu.UpdateRemoveAndDepowerButton(this, this.m_attachCamera);
      }
      if (MyPlatformGameSettings.ENABLE_LOW_MEM_WORLD_LOCKDOWN)
      {
        if (this.m_cleanupRequestingMessageBox != null && MySandboxGame.Static.MemoryState != MySandboxGame.MemState.Critical)
        {
          this.m_cleanupRequestingMessageBox.CloseScreen();
          this.m_cleanupRequestingMessageBox = (MyGuiScreenMessageBox) null;
        }
        else if (MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical && this.m_cleanupRequestingMessageBox == null)
          this.ShowCleanupRequest();
      }
      this.UpdateWeatherInfo();
      this.UpdateMatch();
      return base.Update(hasFocus);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenAdminMenu);

    public override bool RegisterClicks() => true;

    public override bool Draw() => base.Draw();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.A) && this.FocusedControl == this.m_entityListbox)
      {
        this.m_entityListbox.SelectedItems.Clear();
        this.m_entityListbox.SelectedItems.AddRange((IEnumerable<MyGuiControlListbox.Item>) this.m_entityListbox.Items);
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || this.m_defaultJoystickCancelUse && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL) || (MyInput.Static.IsNewKeyPressed(MyKeys.F12) || MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsNewKeyPressed(MyKeys.F10)))
        this.ExitButtonPressed();
      if (!MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NONE))
        return;
      this.SelectNextCharacter();
    }

    public void ExitButtonPressed()
    {
      if (this.m_cleanupRequestingMessageBox != null)
        return;
      if (MyGuiScreenAdminMenu.m_currentPage == MyGuiScreenAdminMenu.MyPageEnum.TrashRemoval)
      {
        this.CheckAndStoreTrashTextboxChanges();
        if (this.m_unsavedTrashSettings)
        {
          if (this.m_unsavedTrashExitBoxIsOpened)
            return;
          this.m_unsavedTrashExitBoxIsOpened = true;
          Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.FinishTrashUnsavedExiting);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.ScreenDebugAdminMenu_UnsavedTrash), callback: callback));
        }
        else
          this.CloseScreen(false);
      }
      else
        this.CloseScreen(false);
    }

    private void FinishTrashUnsavedExiting(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.StoreTrashSettings_RealToTmp();
        this.CloseScreen(false);
      }
      this.m_unsavedTrashExitBoxIsOpened = false;
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyGuiScreenAdminMenu.m_static = (MyGuiScreenAdminMenu) null;
      MySessionComponentSafeZones.OnAddSafeZone -= new EventHandler(this.MySafeZones_OnAddSafeZone);
      MySessionComponentSafeZones.OnRemoveSafeZone -= new EventHandler(this.MySafeZones_OnRemoveSafeZone);
      return base.CloseScreen(isUnloading);
    }

    private void ShowCleanupRequest()
    {
      this.m_cleanupRequestingMessageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.NONE, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCriticalMemory), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionCriticalMemory), position: new Vector2?(new Vector2((float) (0.5 - (double) this.m_size.Value.X / 2.0), 0.5f)), focusable: false);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) this.m_cleanupRequestingMessageBox);
    }

    protected virtual void CreateSelectionCombo() => this.AddCombo<MyEntityCyclingOrder>(MyGuiScreenAdminMenu.m_order, new Action<MyEntityCyclingOrder>(this.OnOrderChanged), color: new Vector4?(this.m_labelColor));

    private MyGuiControlButton CreateDebugButton(
      float usableWidth,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      bool increaseSpacing = true,
      bool addToControls = true,
      bool isAutoScaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
    {
      MyGuiControlButton guiControlButton = this.AddButton(MyTexts.Get(text), onClick, increaseSpacing: increaseSpacing, addToControls: addToControls);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = this.m_scale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2((float) (-(double) MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT / 2.0), 0.0f);
      guiControlButton.Enabled = enabled;
      guiControlButton.IsAutoScaleEnabled = isAutoScaleEnabled;
      guiControlButton.IsAutoEllipsisEnabled = isAutoEllipsisEnabled;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    private void AddSeparator()
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.Size = new Vector2(1f, 0.01f);
      controlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlSeparatorList.AddHorizontal(Vector2.Zero, 1f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
    }

    private MyGuiControlLabel CreateSliderWithDescription(
      MyGuiControlList list,
      float usableWidth,
      float min,
      float max,
      string description,
      ref MyGuiControlSlider slider)
    {
      MyGuiControlLabel myGuiControlLabel1 = this.AddLabel(description, Vector4.One, this.m_scale);
      this.Controls.Remove((MyGuiControlBase) myGuiControlLabel1);
      list.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.CreateSlider(list, usableWidth, min, max, ref slider);
      MyGuiControlLabel myGuiControlLabel2 = this.AddLabel("", Vector4.One, this.m_scale);
      this.Controls.Remove((MyGuiControlBase) myGuiControlLabel2);
      list.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      return myGuiControlLabel2;
    }

    private void CreateSlider(
      MyGuiControlList list,
      float usableWidth,
      float min,
      float max,
      ref MyGuiControlSlider slider)
    {
      ref MyGuiControlSlider local = ref slider;
      Vector2? position = new Vector2?(this.m_currentPosition);
      float num1 = 400f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      double num2 = (double) min;
      double num3 = (double) max;
      double num4 = (double) num1;
      float? defaultValue = new float?();
      Vector4? color = new Vector4?();
      string empty = string.Empty;
      double num5 = 0.75 * (double) this.m_scale;
      MyGuiControlSlider guiControlSlider = new MyGuiControlSlider(position, (float) num2, (float) num3, (float) num4, defaultValue, color, empty, 4, (float) num5, labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      local = guiControlSlider;
      slider.DebugScale = this.m_sliderDebugScale;
      slider.ColorMask = Color.White.ToVector4();
      list.Controls.Add((MyGuiControlBase) slider);
    }

    private void RecreateGlobalSafeZoneControls(
      ref Vector2 controlPadding,
      float separatorSize,
      float usableWidth)
    {
      this.m_recreateInProgress = true;
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage);
      this.m_damageCheckboxGlobalLabel = myGuiControlLabel1;
      this.m_damageGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_damageCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_damageGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting);
      this.m_shootingCheckboxGlobalLabel = myGuiControlLabel2;
      this.m_shootingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_shootingCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_shootingGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling);
      this.m_drillingCheckboxGlobalLabel = myGuiControlLabel3;
      this.m_drillingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_drillingCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_drillingGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel4.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding);
      this.m_weldingCheckboxGlobalLabel = myGuiControlLabel4;
      this.m_weldingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_weldingCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_weldingGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding);
      this.m_grindingCheckboxGlobalLabel = myGuiControlLabel5;
      this.m_grindingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_grindingCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_grindingGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel6.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding);
      this.m_buildingCheckboxGlobalLabel = myGuiControlLabel6;
      this.m_buildingGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_buildingCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_buildingGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
      myGuiControlLabel7.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel7.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections);
      this.m_buildingProjectionsCheckboxGlobalLabel = myGuiControlLabel7;
      this.m_buildingProjectionsGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_buildingProjectionsCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_buildingProjectionsGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel();
      myGuiControlLabel8.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel8.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands);
      this.m_voxelHandCheckboxGlobalLabel = myGuiControlLabel8;
      this.m_voxelHandGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_voxelHandCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_voxelHandGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel9.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear);
      myGuiControlLabel9.IsAutoScaleEnabled = true;
      this.m_landingGearCheckboxGlobalLabel = myGuiControlLabel9;
      this.m_landingGearCheckboxGlobalLabel.SetMaxWidth(0.25f);
      this.m_landingGearGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_landingGearGlobalCheckbox.UserData = (object) MySafeZoneAction.LandingGearLock;
      this.Controls.Add((MyGuiControlBase) this.m_landingGearCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_landingGearGlobalCheckbox);
      this.m_currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel();
      myGuiControlLabel10.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel10.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel10.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation);
      this.m_convertToStationCheckboxGlobalLabel = myGuiControlLabel10;
      this.m_convertToStationGlobalCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(this.m_currentPosition.X + 0.293f, this.m_currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_convertToStationGlobalCheckbox.UserData = (object) MySafeZoneAction.ConvertToStation;
      this.Controls.Add((MyGuiControlBase) this.m_convertToStationCheckboxGlobalLabel);
      this.Controls.Add((MyGuiControlBase) this.m_convertToStationGlobalCheckbox);
      this.UpdateSelectedGlobalData();
      this.m_voxelHandGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.VoxelHandCheckGlobalChanged);
      this.m_buildingGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.BuildingCheckGlobalChanged);
      this.m_buildingProjectionsGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.BuildingProjectionsCheckGlobalChanged);
      this.m_grindingGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.GrindingCheckGlobalChanged);
      this.m_weldingGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.WeldingCheckGlobalChanged);
      this.m_drillingGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.DrillingCheckGlobalChanged);
      this.m_shootingGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.ShootingCheckGlobalChanged);
      this.m_damageGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.DamageCheckGlobalChanged);
      this.m_landingGearGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSettingCheckGlobalChanged);
      this.m_convertToStationGlobalCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSettingCheckGlobalChanged);
    }

    private void UpdateSelectedGlobalData()
    {
      this.m_damageGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction) 0;
      this.m_shootingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction) 0;
      this.m_drillingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction) 0;
      this.m_weldingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction) 0;
      this.m_grindingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction) 0;
      this.m_voxelHandGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction) 0;
      this.m_buildingGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction) 0;
      this.m_buildingProjectionsGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.BuildingProjections) > (MySafeZoneAction) 0;
      this.m_landingGearGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction) 0;
      this.m_convertToStationGlobalCheckbox.IsChecked = (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction) 0;
    }

    private void DamageCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Damage;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Damage;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void ShootingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Shooting;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Shooting;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void DrillingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Drilling;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Drilling;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void WeldingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Welding;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Welding;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void GrindingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Grinding;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Grinding;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void VoxelHandCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.VoxelHand;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.VoxelHand;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void BuildingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Building;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.Building;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void BuildingProjectionsCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.BuildingProjections;
      else
        MySessionComponentSafeZones.AllowedActions &= ~MySafeZoneAction.BuildingProjections;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void OnSettingCheckGlobalChanged(MyGuiControlCheckbox checkBox)
    {
      if (checkBox.IsChecked)
        MySessionComponentSafeZones.AllowedActions |= (MySafeZoneAction) checkBox.UserData;
      else
        MySessionComponentSafeZones.AllowedActions &= ~(MySafeZoneAction) checkBox.UserData;
      MySessionComponentSafeZones.RequestUpdateGlobalSafeZone();
    }

    private void RecreateMatchControls(bool constructor)
    {
      this.m_recreateInProgress = true;
      float num = 0.16f;
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = this.m_currentPosition + vector2;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = "";
      myGuiControlLabel1.IsAutoScaleEnabled = true;
      myGuiControlLabel1.IsAutoEllipsisEnabled = true;
      this.m_labelEnabled = myGuiControlLabel1;
      this.m_labelEnabled.SetMaxWidth(0.3f);
      this.Controls.Add((MyGuiControlBase) this.m_labelEnabled);
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = this.m_currentPosition + vector2;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = "";
      myGuiControlLabel2.IsAutoScaleEnabled = true;
      myGuiControlLabel2.IsAutoEllipsisEnabled = true;
      this.m_labelRunning = myGuiControlLabel2;
      this.m_labelRunning.SetMaxWidth(0.3f);
      this.Controls.Add((MyGuiControlBase) this.m_labelRunning);
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = this.m_currentPosition + vector2;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = "";
      myGuiControlLabel3.IsAutoScaleEnabled = true;
      myGuiControlLabel3.IsAutoEllipsisEnabled = true;
      this.m_labelState = myGuiControlLabel3;
      this.m_labelState.SetMaxWidth(0.3f);
      this.Controls.Add((MyGuiControlBase) this.m_labelState);
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = this.m_currentPosition + vector2;
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel4.Text = "";
      myGuiControlLabel4.IsAutoScaleEnabled = true;
      myGuiControlLabel4.IsAutoEllipsisEnabled = true;
      this.m_labelTime = myGuiControlLabel4;
      this.m_labelTime.SetMaxWidth(0.3f);
      this.Controls.Add((MyGuiControlBase) this.m_labelTime);
      this.m_currentPosition.Y += 0.1f;
      this.m_buttonStart = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Start, new Action<MyGuiControlButton>(this.StartMatch), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_Start_Tooltip));
      this.m_currentPosition.Y += 1f / 500f;
      if (MyFakes.SHOW_MATCH_STOP)
      {
        this.m_buttonStop = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Stop, new Action<MyGuiControlButton>(this.StopMatch), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_Stop_Tooltip));
        this.m_currentPosition.Y += 1f / 500f;
      }
      this.m_buttonPause = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Pause, new Action<MyGuiControlButton>(this.PauseMatch), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip));
      this.m_currentPosition.Y += 1f / 500f;
      this.m_buttonAdvanced = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_Advance, new Action<MyGuiControlButton>(this.ProgressMatch), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_Advance_Tooltip));
      this.UpdatePauseButtonTexts();
      this.m_currentPosition.Y += 0.04f;
      this.m_textboxTime = new MyGuiControlTextbox(defaultText: "0", type: MyGuiControlTextboxType.DigitsOnly);
      this.m_textboxTime.TextAlignment = TextAlingmentMode.Right;
      this.m_textboxTime.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_textboxTime.Position = this.m_currentPosition + new Vector2(num - MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT / 2f, 0.0f);
      this.m_textboxTime.Size = new Vector2(num, this.m_textboxTime.Size.Y);
      this.m_textboxTime.TextChanged += new Action<MyGuiControlTextbox>(this.MatchTimeTextbox_Changed);
      this.Controls.Add((MyGuiControlBase) this.m_textboxTime);
      this.m_currentPosition.Y += 0.035f;
      this.m_buttonSetTime = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime, new Action<MyGuiControlButton>(this.SetTime), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_SetTime_Tooltip));
      this.m_currentPosition.Y += 1f / 500f;
      this.m_buttonAddTime = this.CreateDebugButton(num, MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime, new Action<MyGuiControlButton>(this.AddTime), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Match_AddTime_Tooltip));
      this.SyncData();
    }

    private void MatchTimeTextbox_Changed(MyGuiControlTextbox obj) => this.ClearTextColor();

    private void SyncData()
    {
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.SendDataToClient)));
    }

    [Event(null, 146)]
    [Reliable]
    [Server]
    public static void SendDataToClient()
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyMatchState, float, bool, bool>((Func<IMyEventOwner, Action<MyMatchState, float, bool, bool>>) (x => new Action<MyMatchState, float, bool, bool>(MyGuiScreenAdminMenu.ReciveClientData)), component.State, component.RemainingMinutes, component.IsRunning, component.IsEnabled, MyEventContext.Current.Sender);
    }

    [Event(null, 156)]
    [Reliable]
    [Client]
    public static void ReciveClientData(
      MyMatchState state,
      float remainingTime,
      bool isRunning,
      bool isEnabled)
    {
      foreach (MyGuiScreenAdminMenu matchSyncReceiver in MyGuiScreenAdminMenu.m_matchSyncReceivers)
        matchSyncReceiver.SetSyncData(state, remainingTime, isRunning, isEnabled);
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Clear();
    }

    private void SetSyncData(
      MyMatchState state,
      float remainingTime,
      bool isRunning,
      bool isEnabled)
    {
      this.m_matchCurrentState = state;
      this.m_matchRemainingTime = MyTimeSpan.FromMinutes((double) remainingTime);
      this.m_isMatchRunning = isRunning;
      this.m_isMatchEnabled = isEnabled;
      this.m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      this.ResetStateTexts();
      this.UpdatePauseButtonTexts();
    }

    private void ResetStateTexts()
    {
      this.m_labelEnabled.Text = !this.m_isMatchEnabled ? string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_DisableText)) : string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_EnableText));
      this.m_labelRunning.Text = !this.m_isMatchRunning ? string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_NotRunningText)) : string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_RunningText));
      this.m_labelState.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_StateText), (object) this.m_matchCurrentState.ToString());
      this.UpdateTimeText();
    }

    private void UpdatePauseButtonTexts()
    {
      if (this.m_isMatchRunning)
      {
        this.m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause);
        this.m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Pause_Tooltip);
      }
      else
      {
        this.m_buttonPause.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause);
        this.m_buttonPause.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Match_Unpause_Tooltip);
      }
    }

    private void StartMatch(MyGuiControlButton obj)
    {
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.StartMatchInternal)));
    }

    private void StopMatch(MyGuiControlButton obj)
    {
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.StopMatchInternal)));
    }

    private void PauseMatch(MyGuiControlButton obj)
    {
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.PauseMatchInternal)));
    }

    private void ProgressMatch(MyGuiControlButton obj)
    {
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenAdminMenu.ProgressMatchInternal)));
    }

    private void SetTime(MyGuiControlButton obj)
    {
      float result;
      if (!float.TryParse(this.m_textboxTime.Text, out result))
      {
        this.SetTextRed();
      }
      else
      {
        if ((double) result < 0.0)
          result = 0.0f;
        MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyGuiScreenAdminMenu.SetTimeInternal)), result);
        this.ClearTextColor();
      }
    }

    private void AddTime(MyGuiControlButton obj)
    {
      float result;
      if (!float.TryParse(this.m_textboxTime.Text, out result))
        return;
      MyGuiScreenAdminMenu.m_matchSyncReceivers.Add(this);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyGuiScreenAdminMenu.AddTimeInternal)), result);
    }

    [Event(null, 262)]
    [Reliable]
    [Server]
    public static void StartMatchInternal()
    {
      if (!MyGuiScreenAdminMenu.ValidatePlayer())
        return;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      component.ResetToFirstState();
      if (component.State == MyMatchState.PreMatch)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Started), (object) playerName));
      }
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    [Event(null, 282)]
    [Reliable]
    [Server]
    public static void StopMatchInternal()
    {
      if (!MyGuiScreenAdminMenu.ValidatePlayer())
        return;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      int loopLimit = MyGuiScreenAdminMenu.LOOP_LIMIT;
      bool flag = true;
      while (component.State != MyMatchState.Finished)
      {
        int state1 = (int) component.State;
        component.AdvanceToNextState();
        int state2 = (int) component.State;
        if (state1 == state2)
          --loopLimit;
        else
          loopLimit = MyGuiScreenAdminMenu.LOOP_LIMIT;
        if (loopLimit < 0)
        {
          flag = false;
          break;
        }
      }
      if (flag && component.State == MyMatchState.Finished)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Stopped), (object) playerName));
      }
      else
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_StopFailed)));
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    [Event(null, 324)]
    [Reliable]
    [Server]
    public static void PauseMatchInternal()
    {
      if (!MyGuiScreenAdminMenu.ValidatePlayer())
        return;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      bool isRunning = component.IsRunning;
      component.SetIsRunning(!isRunning);
      if (isRunning)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Paused), (object) playerName));
      }
      else
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Unpaused), (object) playerName));
      }
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    [Event(null, 352)]
    [Reliable]
    [Server]
    public static void ProgressMatchInternal()
    {
      if (!MyGuiScreenAdminMenu.ValidatePlayer())
        return;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      int state1 = (int) component.State;
      component.AdvanceToNextState();
      int state2 = (int) component.State;
      if (state1 != state2)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_Advanced), (object) playerName, (object) component.State.ToString()));
      }
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    [Event(null, 374)]
    [Reliable]
    [Server]
    public static void SetTimeInternal(float value)
    {
      if (!MyGuiScreenAdminMenu.ValidatePlayer())
        return;
      if ((double) value < 0.0)
        value = 0.0f;
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      float remainingMinutes = component.RemainingMinutes;
      component.SetRemainingTime(value);
      if ((double) component.RemainingMinutes != (double) remainingMinutes)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_SetTime), (object) playerName, (object) value));
      }
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    [Event(null, 398)]
    [Reliable]
    [Server]
    public static void AddTimeInternal(float value)
    {
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (component == null)
        return;
      float remainingMinutes = component.RemainingMinutes;
      component.AddRemainingTime(value);
      if ((double) component.RemainingMinutes != (double) remainingMinutes)
      {
        string playerName = MyGuiScreenAdminMenu.GetPlayerName(MyEventContext.Current.Sender.Value);
        MyGuiScreenAdminMenu.SendGlobalMessage(string.Format(MyTexts.GetString(MyCommonTexts.Notification_Match_AddTime), (object) playerName, (object) value));
      }
      MyGuiScreenAdminMenu.SendDataToClient();
    }

    private static string GetPlayerName(ulong steamId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0));
      return identity == null ? string.Empty : identity.DisplayName;
    }

    private static void SendGlobalMessage(string message) => MyVisualScriptLogicProvider.SendChatMessage(message);

    private void SetTextRed() => this.m_textboxTime.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_RED;

    private void ClearTextColor() => this.m_textboxTime.ColorMask = (Vector4) MyTerminalFactionController.COLOR_CUSTOM_GREY;

    private static bool ValidatePlayer() => MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value);

    protected void UpdateMatch() => this.UpdateTime();

    private void UpdateTime()
    {
      if (MyGuiScreenAdminMenu.m_currentPage != MyGuiScreenAdminMenu.MyPageEnum.Match)
        return;
      MyTimeSpan matchRemainingTime = this.m_matchRemainingTime;
      if (!this.m_isMatchEnabled || !this.m_isMatchRunning)
        this.m_matchLastUpdateTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      this.m_matchRemainingTime -= myTimeSpan - this.m_matchLastUpdateTime;
      if (this.m_matchRemainingTime < MyTimeSpan.Zero)
      {
        this.SyncData();
        this.m_matchRemainingTime = MyTimeSpan.Zero;
      }
      this.UpdateTimeText();
      this.m_matchLastUpdateTime = myTimeSpan;
    }

    private void UpdateTimeText()
    {
      if (this.m_labelTime == null)
        return;
      this.m_labelTime.Text = string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Match_TimeText), (object) this.FormatTime(this.m_matchRemainingTime));
    }

    private string FormatTime(MyTimeSpan remainingTime)
    {
      StringBuilder output = new StringBuilder();
      MyValueFormatter.AppendTimeExactHoursMinSec((int) remainingTime.Seconds, output);
      return output.ToString();
    }

    private void RecreateReplayToolControls(
      ref Vector2 controlPadding,
      float separatorSize,
      float usableWidth)
    {
      this.m_recreateInProgress = true;
      this.m_currentPosition.Y += 0.03f;
      if (!MySession.Static.IsServer)
      {
        MyGuiControlButton debugButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, new Action<MyGuiControlButton>(this.ReloadWorld), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorldClient_Tooltip));
        debugButton.Enabled = false;
        debugButton.ShowTooltipWhenDisabled = true;
      }
      else
        this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld, new Action<MyGuiControlButton>(this.ReloadWorld), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ReloadWorld_Tooltip));
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageCharacters);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      Vector2 currentPosition = this.m_currentPosition;
      this.m_buttonXOffset -= 0.075f;
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter, new Action<MyGuiControlButton>(this.AddCharacter), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_AddCharacter_Tooltip), isAutoScaleEnabled: true);
      this.m_currentPosition.Y = currentPosition.Y;
      this.m_buttonXOffset += 0.15f;
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter, new Action<MyGuiControlButton>(this.TryRemoveCurrentCharacter), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RemoveCharacter_Tooltip), isAutoScaleEnabled: true);
      this.m_buttonXOffset = 0.0f;
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeCharacter, new Action<MyGuiControlButton>(this.TryChangeCurrentCharacter), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ChangeCharacter_Tooltip), isAutoScaleEnabled: true);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_ManageRecordings);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_currentPosition.Y += 0.03f;
      if (MySessionComponentReplay.Static.IsReplaying)
        this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay, new Action<MyGuiControlButton>(this.OnReplayButtonPressed), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopReplay_Tooltip));
      else
        this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay, new Action<MyGuiControlButton>(this.OnReplayButtonPressed), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Replay_Tooltip));
      if (MySessionComponentReplay.Static.IsRecording)
        this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording, new Action<MyGuiControlButton>(this.OnRecordButtonPressed), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_StopRecording_Tooltip));
      else
        this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay, new Action<MyGuiControlButton>(this.OnRecordButtonPressed), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_RecordAndReplay_Tooltip), isAutoScaleEnabled: true);
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings, new Action<MyGuiControlButton>(this.DeleteRecordings), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings_Tooltip), isAutoScaleEnabled: true);
      this.m_currentPosition.Y += 0.02f;
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText();
      controlMultilineText1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      controlMultilineText1.Size = new Vector2(0.3f, 0.6f);
      controlMultilineText1.Font = "Blue";
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_0));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_1));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_2));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_3));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_4));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_5));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_6));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_7));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_8));
      controlMultilineText2.AppendLine();
      controlMultilineText2.AppendText(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_Tutorial_9));
      this.Controls.Add((MyGuiControlBase) controlMultilineText2);
    }

    private void ReloadWorld(MyGuiControlButton obj)
    {
      if (!Directory.Exists(MySession.Static.CurrentPath))
        return;
      MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
    }

    private void AddCharacter(MyGuiControlButton obj) => MyCharacterInputComponent.SpawnCharacter();

    private void TryRemoveCurrentCharacter(MyGuiControlButton obj)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return;
      this.SelectNextCharacter();
      if (MySession.Static.ControlledEntity == controlledEntity)
        return;
      controlledEntity.Entity.Close();
    }

    private void TryChangeCurrentCharacter(MyGuiControlButton obj) => MyGuiScreenGamePlay.SetSpectatorNone();

    private void SelectNextCharacter()
    {
      switch (MySession.Static.GetCameraControllerEnum())
      {
        case MyCameraControllerEnum.Entity:
        case MyCameraControllerEnum.ThirdPersonSpectator:
          if (MySession.Static.VirtualClients.Any() && Sync.Clients.LocalClient != null)
          {
            MyPlayer myPlayer = MySession.Static.VirtualClients.GetNextControlledPlayer(MySession.Static.LocalHumanPlayer) ?? Sync.Clients.LocalClient.GetPlayer(0);
            if (myPlayer != null)
            {
              Sync.Clients.LocalClient.ControlledPlayerSerialId = myPlayer.Id.SerialId;
              break;
            }
            break;
          }
          long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
          List<MyEntity> myEntityList1 = new List<MyEntity>();
          foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
          {
            if (entity is MyCharacter myCharacter && !myCharacter.IsDead && (myCharacter.GetIdentity() != null && myCharacter.GetIdentity().IdentityId == identityId))
              myEntityList1.Add(entity);
            if (entity is MyCubeGrid myCubeGrid)
            {
              foreach (MySlimBlock block in myCubeGrid.GetBlocks())
              {
                if (block.FatBlock is MyCockpit fatBlock && fatBlock.Pilot != null && (fatBlock.Pilot.GetIdentity() != null && fatBlock.Pilot.GetIdentity().IdentityId == identityId))
                  myEntityList1.Add((MyEntity) fatBlock);
              }
            }
          }
          int num = myEntityList1.IndexOf(MySession.Static.ControlledEntity.Entity);
          List<MyEntity> myEntityList2 = new List<MyEntity>();
          if (num + 1 < myEntityList1.Count)
            myEntityList2.AddRange((IEnumerable<MyEntity>) myEntityList1.GetRange(num + 1, myEntityList1.Count - num - 1));
          if (num != -1)
            myEntityList2.AddRange((IEnumerable<MyEntity>) myEntityList1.GetRange(0, num + 1));
          IMyControllableEntity entity1 = (IMyControllableEntity) null;
          for (int index = 0; index < myEntityList2.Count; ++index)
          {
            if (myEntityList2[index] is IMyControllableEntity)
            {
              entity1 = myEntityList2[index] as IMyControllableEntity;
              break;
            }
          }
          if (MySession.Static.LocalHumanPlayer != null && entity1 != null)
          {
            MySession.Static.LocalHumanPlayer.Controller.TakeControl(entity1);
            if (!(MySession.Static.ControlledEntity is MyCharacter character) && MySession.Static.ControlledEntity is MyCockpit)
              character = (MySession.Static.ControlledEntity as MyCockpit).Pilot;
            if (character != null)
            {
              MySession.Static.LocalHumanPlayer.Identity.ChangeCharacter(character);
              break;
            }
            break;
          }
          break;
      }
      if (MySession.Static.ControlledEntity is MyCharacter)
        return;
      MySession.Static.GameFocusManager.Clear();
    }

    private void OnReplayButtonPressed(MyGuiControlButton obj)
    {
      if (MySessionComponentReplay.Static == null)
        return;
      if (!MySessionComponentReplay.Static.IsReplaying)
      {
        if (!MySessionComponentReplay.Static.HasRecordedData)
          return;
        MySessionComponentReplay.Static.StartReplay();
        this.CloseScreen(false);
      }
      else
      {
        MySessionComponentReplay.Static.StopReplay();
        this.RecreateControls(false);
      }
    }

    private void OnRecordButtonPressed(MyGuiControlButton obj)
    {
      if (MySessionComponentReplay.Static == null)
        return;
      if (!MySessionComponentReplay.Static.IsRecording)
      {
        MySessionComponentReplay.Static.StartRecording();
        MySessionComponentReplay.Static.StartReplay();
        this.CloseScreen(false);
      }
      else
      {
        MySessionComponentReplay.Static.StopRecording();
        MySessionComponentReplay.Static.StopReplay();
        this.RecreateControls(false);
      }
    }

    private void DeleteRecordings(MyGuiControlButton obj) => MySessionComponentReplay.Static.DeleteRecordings();

    private void RecreateSafeZonesControls(
      ref Vector2 controlPadding,
      float separatorSize,
      float usableWidth)
    {
      this.m_recreateInProgress = true;
      this.m_currentPosition.Y += 0.015f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_SelectSafeZone);
      this.m_selectSafeZoneLabel = myGuiControlLabel1;
      this.Controls.Add((MyGuiControlBase) this.m_selectSafeZoneLabel);
      this.m_currentPosition.Y += 0.03f;
      this.m_safeZonesCombo = this.AddCombo();
      this.m_currentPosition.Y += 1f / 1000f;
      MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
      guiControlParent1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlParent1.Position = Vector2.Zero;
      guiControlParent1.Size = new Vector2(0.32f, 0.88f);
      MyGuiControlParent guiControlParent2 = guiControlParent1;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2((float) (-(double) this.m_size.Value.X * 0.829999983310699 / 2.0), this.m_currentPosition.Y), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      this.m_currentPosition.Y += 0.005f;
      MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) guiControlParent2);
      controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel.Position = this.m_currentPosition;
      controlScrollablePanel.Size = new Vector2(0.32f, 0.62f);
      this.m_optionsGroup = controlScrollablePanel;
      this.m_optionsGroup.ScrollbarVEnabled = true;
      this.m_optionsGroup.ScrollBarOffset = new Vector2(-0.01f, 0.0f);
      this.Controls.Add((MyGuiControlBase) this.m_optionsGroup);
      Vector2 position = -guiControlParent2.Size * 0.5f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.SafeZone_SelectZoneShape);
      this.m_selectZoneShapeLabel = myGuiControlLabel2;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_selectZoneShapeLabel);
      position.Y += 0.03f;
      this.m_safeZonesTypeCombo = this.AddCombo(addToControls: false, overridePos: new Vector2?(position));
      position.Y += this.m_safeZonesTypeCombo.Size.Y + 0.01f + this.Spacing;
      this.m_safeZonesTypeCombo.AddItem(0L, MyTexts.GetString(MySpaceTexts.SafeZone_Spherical));
      this.m_safeZonesTypeCombo.AddItem(1L, MyTexts.GetString(MySpaceTexts.SafeZone_Cubical));
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_safeZonesTypeCombo);
      position.Y += 1f / 1000f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneRadius);
      this.m_zoneRadiusLabel = myGuiControlLabel3;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_zoneRadiusLabel);
      this.m_zoneRadiusLabel.Visible = false;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = new Vector2(position.X + 0.285f, position.Y);
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      myGuiControlLabel4.Text = "1";
      this.m_zoneRadiusValueLabel = myGuiControlLabel4;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_zoneRadiusValueLabel);
      position.Y += 0.03f;
      this.m_radiusSlider = new MyGuiControlSlider(new Vector2?(position), MySafeZone.MIN_RADIUS, MySafeZone.MAX_RADIUS, 0.285f, new float?(1f), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_radiusSlider.Visible = false;
      this.m_radiusSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnRadiusChange);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_radiusSlider);
      position.Y -= 0.03f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = MyTexts.GetString(MySpaceTexts.SafeZone_CubeAxis);
      this.m_selectAxisLabel = myGuiControlLabel5;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_selectAxisLabel);
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Position = new Vector2(position.X + 0.09f, position.Y);
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel6.Text = MyTexts.GetString(MyCommonTexts.Size);
      this.m_zoneSizeLabel = myGuiControlLabel6;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_zoneSizeLabel);
      position.Y += 0.03f;
      this.m_safeZonesAxisCombo = this.AddCombo(addToControls: false, overridePos: new Vector2?(position));
      position.Y += this.m_safeZonesAxisCombo.Size.Y + 0.01f + this.Spacing;
      this.m_safeZonesAxisCombo.Size = new Vector2(0.08f, 1f);
      this.m_safeZonesAxisCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_safeZonesAxisCombo_ItemSelected);
      this.m_safeZonesAxisCombo.AddItem(0L, MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X.ToString());
      this.m_safeZonesAxisCombo.AddItem(1L, MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y.ToString());
      this.m_safeZonesAxisCombo.AddItem(2L, MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z.ToString());
      this.m_safeZonesAxisCombo.SelectItemByIndex(0);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_safeZonesAxisCombo);
      this.m_sizeSlider = new MyGuiControlSlider(new Vector2?(position + new Vector2(0.09f, -0.05f)), 20f, 500f, 0.195f, new float?(1f), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_sizeSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnSizeChange);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_sizeSlider);
      position.Y += 0.018f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
      myGuiControlLabel7.Position = position + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel7.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ZoneEnabled);
      this.m_enabledCheckboxLabel = myGuiControlLabel7;
      this.m_enabledCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_enabledCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.EnabledCheckedChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_enabledCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_enabledCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel();
      myGuiControlLabel8.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel8.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage);
      this.m_damageCheckboxLabel = myGuiControlLabel8;
      this.m_damageCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_damageCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.DamageCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_damageCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_damageCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel9.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting);
      this.m_shootingCheckboxLabel = myGuiControlLabel9;
      this.m_shootingCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_shootingCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.ShootingCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_shootingCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_shootingCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel();
      myGuiControlLabel10.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel10.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel10.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling);
      this.m_drillingCheckboxLabel = myGuiControlLabel10;
      this.m_drillingCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_drillingCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.DrillingCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_drillingCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_drillingCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel();
      myGuiControlLabel11.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel11.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding);
      this.m_weldingCheckboxLabel = myGuiControlLabel11;
      this.m_weldingCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_weldingCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.WeldingCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_weldingCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_weldingCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel12 = new MyGuiControlLabel();
      myGuiControlLabel12.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel12.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel12.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding);
      this.m_grindingCheckboxLabel = myGuiControlLabel12;
      this.m_grindingCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_grindingCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.GrindingCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_grindingCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_grindingCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel();
      myGuiControlLabel13.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel13.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel13.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding);
      this.m_buildingCheckboxLabel = myGuiControlLabel13;
      this.m_buildingCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_buildingCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.BuildingCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_buildingCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_buildingCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel14 = new MyGuiControlLabel();
      myGuiControlLabel14.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel14.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel14.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections);
      this.m_buildingProjectionsCheckboxLabel = myGuiControlLabel14;
      this.m_buildingProjectionsCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_buildingProjectionsCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.BuildingProjectionsCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_buildingProjectionsCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_buildingProjectionsCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel();
      myGuiControlLabel15.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel15.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel15.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands);
      this.m_voxelHandCheckboxLabel = myGuiControlLabel15;
      this.m_voxelHandCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_voxelHandCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.VoxelHandCheckChanged);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_voxelHandCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_voxelHandCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel16 = new MyGuiControlLabel();
      myGuiControlLabel16.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel16.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel16.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear);
      myGuiControlLabel16.IsAutoScaleEnabled = true;
      this.m_landingGearLockCheckboxLabel = myGuiControlLabel16;
      this.m_landingGearLockCheckboxLabel.SetMaxWidth(0.25f);
      this.m_landingGearLockCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_landingGearLockCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSettingsCheckChanged);
      this.m_landingGearLockCheckbox.UserData = (object) MySafeZoneAction.LandingGearLock;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_landingGearLockCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_landingGearLockCheckbox);
      position.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel();
      myGuiControlLabel17.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel17.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel17.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation);
      this.m_convertToStationCheckboxLabel = myGuiControlLabel17;
      this.m_convertToStationCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(position.X + 0.293f, position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_convertToStationCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnSettingsCheckChanged);
      this.m_convertToStationCheckbox.UserData = (object) MySafeZoneAction.ConvertToStation;
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_convertToStationCheckboxLabel);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_convertToStationCheckbox);
      position.Y += 0.04f;
      MyGuiControlLabel myGuiControlLabel18 = new MyGuiControlLabel();
      myGuiControlLabel18.Position = new Vector2(position.X + 1f / 1000f, position.Y);
      myGuiControlLabel18.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel18.Text = MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_TextureColorLabel);
      MyGuiControlLabel myGuiControlLabel19 = myGuiControlLabel18;
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlLabel19);
      position.Y += 0.03f;
      this.m_textureCombo = this.AddCombo(addToControls: false, overridePos: new Vector2?(position));
      IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
      if (allDefinitions != null)
      {
        foreach (MySafeZoneTexturesDefinition texturesDefinition in allDefinitions)
          this.m_textureCombo.AddItem((long) (int) texturesDefinition.DisplayTextId, MyStringId.GetOrCompute(texturesDefinition.DisplayTextId.String));
      }
      else
        MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_textureCombo);
      position.Y += 0.055f;
      this.m_colorSelector = new MyGuiControlColor(MyTexts.GetString(MySpaceTexts.ScreenAdmin_Safezone_ColorLabel), 1f, position, Color.SkyBlue, Color.Red, MyCommonTexts.DialogAmount_SetValueCaption, true);
      this.m_colorSelector.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_colorSelector.Size = new Vector2(0.285f, this.m_colorSelector.Size.Y);
      guiControlParent2.Controls.Add((MyGuiControlBase) this.m_colorSelector);
      position.Y += 0.17f;
      this.m_optionsGroup.RefreshInternals();
      this.m_currentPosition.Y += this.m_optionsGroup.Size.Y;
      this.m_currentPosition.Y += 0.005f;
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2((float) (-(double) this.m_size.Value.X * 0.829999983310699 / 2.0), this.m_currentPosition.Y), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.m_currentPosition.Y += 0.018f;
      float y1 = this.m_currentPosition.Y;
      this.m_addSafeZoneButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_NewSafeZone, (Action<MyGuiControlButton>) (c => this.OnAddSafeZone()), isAutoScaleEnabled: true, isAutoEllipsisEnabled: true);
      this.m_addSafeZoneButton.PositionX = -0.088f;
      this.m_currentPosition.Y = y1;
      this.m_moveToSafeZoneButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_MoveToSafeZone, (Action<MyGuiControlButton>) (c => this.OnMoveToSafeZone()), isAutoScaleEnabled: true, isAutoEllipsisEnabled: true);
      this.m_moveToSafeZoneButton.PositionX = 0.055f;
      float y2 = this.m_currentPosition.Y;
      this.m_repositionSafeZoneButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ChangePosition, (Action<MyGuiControlButton>) (c => this.OnRepositionSafeZone()));
      this.m_repositionSafeZoneButton.PositionX = -0.088f;
      this.m_currentPosition.Y = y2;
      this.m_configureFilterButton = this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter, (Action<MyGuiControlButton>) (c => this.OnConfigureFilter()));
      this.m_configureFilterButton.PositionX = 0.055f;
      float y3 = this.m_currentPosition.Y;
      this.m_removeSafeZoneButton = this.CreateDebugButton(0.14f, MyCommonTexts.ScreenDebugAdminMenu_Remove, (Action<MyGuiControlButton>) (c => this.OnRemoveSafeZone()));
      this.m_removeSafeZoneButton.PositionX = -0.088f;
      this.m_currentPosition.Y = y3;
      this.m_renameSafeZoneButton = this.CreateDebugButton(0.14f, MySpaceTexts.DetailScreen_Button_Rename, (Action<MyGuiControlButton>) (c => this.OnRenameSafeZone()));
      this.m_renameSafeZoneButton.PositionX = 0.055f;
      this.RefreshSafeZones();
      this.UpdateZoneType();
      this.UpdateSelectedData();
      this.m_safeZonesCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_safeZonesCombo_ItemSelected);
      this.m_safeZonesTypeCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_safeZonesTypeCombo_ItemSelected);
      this.m_textureCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnTextureSelected);
      this.m_colorSelector.OnChange += new Action<MyGuiControlColor>(this.OnColorChanged);
      this.m_recreateInProgress = false;
    }

    private void OnColorChanged(MyGuiControlColor obj)
    {
      if (this.m_selectedSafeZone == null)
        return;
      MyObjectBuilder_SafeZone objectBuilder = (MyObjectBuilder_SafeZone) this.m_selectedSafeZone.GetObjectBuilder(false);
      objectBuilder.ModelColor = (SerializableVector3) obj.GetColor().ToVector3();
      MySessionComponentSafeZones.RequestUpdateSafeZone(objectBuilder);
    }

    private void OnTextureSelected()
    {
      if (this.m_selectedSafeZone == null)
        return;
      IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
      if (allDefinitions == null)
      {
        MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
      }
      else
      {
        MyObjectBuilder_SafeZone objectBuilder = (MyObjectBuilder_SafeZone) this.m_selectedSafeZone.GetObjectBuilder(false);
        MyStringHash myStringHash = MyStringHash.TryGet((int) this.m_textureCombo.GetSelectedKey());
        bool flag = false;
        foreach (MySafeZoneTexturesDefinition texturesDefinition in allDefinitions)
        {
          if (texturesDefinition.DisplayTextId == myStringHash)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          MyLog.Default.Error("Safe zone texture not found.");
        }
        else
        {
          objectBuilder.Texture = myStringHash.String;
          MySessionComponentSafeZones.RequestUpdateSafeZone(objectBuilder);
        }
      }
    }

    private void UpdateSelectedData()
    {
      this.m_recreateInProgress = true;
      bool flag = this.m_selectedSafeZone != null;
      this.m_enabledCheckbox.Enabled = flag;
      this.m_damageCheckbox.Enabled = flag;
      this.m_shootingCheckbox.Enabled = flag;
      this.m_drillingCheckbox.Enabled = flag;
      this.m_weldingCheckbox.Enabled = flag;
      this.m_grindingCheckbox.Enabled = flag;
      this.m_voxelHandCheckbox.Enabled = flag;
      this.m_buildingCheckbox.Enabled = flag;
      this.m_buildingProjectionsCheckbox.Enabled = flag;
      this.m_convertToStationCheckbox.Enabled = flag;
      this.m_landingGearLockCheckbox.Enabled = flag;
      this.m_radiusSlider.Enabled = flag;
      this.m_renameSafeZoneButton.Enabled = flag;
      this.m_removeSafeZoneButton.Enabled = flag;
      this.m_repositionSafeZoneButton.Enabled = flag;
      this.m_moveToSafeZoneButton.Enabled = flag;
      this.m_configureFilterButton.Enabled = flag;
      this.m_safeZonesCombo.Enabled = flag;
      this.m_safeZonesTypeCombo.Enabled = flag;
      this.m_safeZonesAxisCombo.Enabled = flag;
      this.m_sizeSlider.Enabled = flag;
      this.m_colorSelector.Enabled = flag;
      this.m_textureCombo.Enabled = flag;
      if (this.m_selectedSafeZone != null)
      {
        this.m_enabledCheckbox.IsChecked = this.m_selectedSafeZone.Enabled;
        if (this.m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
        {
          this.m_radiusSlider.Value = this.m_selectedSafeZone.Radius;
          this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Radius.ToString();
        }
        else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 0)
        {
          this.m_sizeSlider.Value = this.m_selectedSafeZone.Size.X;
          this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.X.ToString();
        }
        else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 1)
        {
          this.m_sizeSlider.Value = this.m_selectedSafeZone.Size.Y;
          this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.Y.ToString();
        }
        else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 2)
        {
          this.m_sizeSlider.Value = this.m_selectedSafeZone.Size.Z;
          this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.Z.ToString();
        }
        this.m_safeZonesTypeCombo.SelectItemByKey((long) this.m_selectedSafeZone.Shape);
        this.m_damageCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Damage) > (MySafeZoneAction) 0;
        this.m_shootingCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Shooting) > (MySafeZoneAction) 0;
        this.m_drillingCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Drilling) > (MySafeZoneAction) 0;
        this.m_weldingCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Welding) > (MySafeZoneAction) 0;
        this.m_grindingCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Grinding) > (MySafeZoneAction) 0;
        this.m_voxelHandCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.VoxelHand) > (MySafeZoneAction) 0;
        this.m_buildingCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.Building) > (MySafeZoneAction) 0;
        this.m_buildingProjectionsCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.BuildingProjections) > (MySafeZoneAction) 0;
        this.m_landingGearLockCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.LandingGearLock) > (MySafeZoneAction) 0;
        this.m_convertToStationCheckbox.IsChecked = (this.m_selectedSafeZone.AllowedActions & MySafeZoneAction.ConvertToStation) > (MySafeZoneAction) 0;
        this.m_textureCombo.SelectItemByKey((long) (int) this.m_selectedSafeZone.CurrentTexture);
        this.m_colorSelector.SetColor(this.m_selectedSafeZone.ModelColor);
      }
      this.m_recreateInProgress = false;
    }

    private void m_safeZonesTypeCombo_ItemSelected()
    {
      if (this.m_selectedSafeZone.Shape == (MySafeZoneShape) this.m_safeZonesTypeCombo.GetSelectedKey())
        return;
      this.m_selectedSafeZone.Shape = (MySafeZoneShape) this.m_safeZonesTypeCombo.GetSelectedKey();
      this.m_selectedSafeZone.RecreatePhysics();
      this.UpdateZoneType();
      this.RequestUpdateSafeZone();
    }

    private void UpdateZoneType()
    {
      this.m_zoneRadiusLabel.Visible = false;
      this.m_radiusSlider.Visible = false;
      this.m_selectAxisLabel.Visible = false;
      this.m_zoneSizeLabel.Visible = false;
      this.m_safeZonesAxisCombo.Visible = false;
      this.m_sizeSlider.Visible = false;
      if (this.m_selectedSafeZone == null || this.m_selectedSafeZone.Shape == MySafeZoneShape.Box)
      {
        this.m_selectAxisLabel.Visible = true;
        this.m_zoneSizeLabel.Visible = true;
        this.m_safeZonesAxisCombo.Visible = true;
        this.m_sizeSlider.Visible = true;
      }
      else if (this.m_selectedSafeZone.Shape == MySafeZoneShape.Sphere)
      {
        this.m_zoneRadiusLabel.Visible = true;
        this.m_radiusSlider.Visible = true;
      }
      this.UpdateSelectedData();
    }

    private void m_safeZonesAxisCombo_ItemSelected()
    {
      if (this.m_selectedSafeZone == null)
        return;
      if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 0)
        this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.X.ToString();
      else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 1)
        this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.Y.ToString();
      else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 2)
        this.m_zoneRadiusValueLabel.Text = this.m_selectedSafeZone.Size.Z.ToString();
      this.UpdateSelectedData();
    }

    private void m_safeZonesCombo_ItemSelected()
    {
      this.m_selectedSafeZone = (MySafeZone) Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_safeZonesCombo.GetItemByIndex(this.m_safeZonesCombo.GetSelectedIndex()).Key);
      this.UpdateZoneType();
      this.UpdateSelectedData();
    }

    private void OnAddSafeZone() => MySessionComponentSafeZones.RequestCreateSafeZone(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector);

    private void OnRemoveSafeZone()
    {
      if (this.m_selectedSafeZone == null)
        return;
      MySessionComponentSafeZones.RequestDeleteSafeZone(this.m_selectedSafeZone.EntityId);
      this.RequestUpdateSafeZone();
    }

    private void OnRenameSafeZone()
    {
      if (this.m_selectedSafeZone == null)
        return;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), (Action<string>) (result =>
      {
        if (result == null)
          return;
        this.m_selectedSafeZone.DisplayName = result;
        this.RequestUpdateSafeZone();
        this.RefreshSafeZones();
      }), "New Name", MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename), 50, 0.3f));
    }

    private void OnConfigureFilter()
    {
      if (this.m_selectedSafeZone == null)
        return;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenSafeZoneFilter(new Vector2(0.5f, 0.5f), this.m_selectedSafeZone));
    }

    private void OnMoveToSafeZone()
    {
      if (this.m_selectedSafeZone == null || MySession.Static.ControlledEntity == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.TeleportControlledEntity(this.m_selectedSafeZone.PositionComp.WorldMatrixRef.Translation);
    }

    private void OnRepositionSafeZone()
    {
      if (this.m_selectedSafeZone == null)
        return;
      this.m_selectedSafeZone.PositionComp.SetWorldMatrix(ref MySector.MainCamera.WorldMatrix);
      this.m_selectedSafeZone.RecreatePhysics();
      this.RequestUpdateSafeZone();
    }

    private void MySafeZones_OnAddSafeZone(object sender, System.EventArgs e)
    {
      this.m_selectedSafeZone = (MySafeZone) sender;
      if (MyGuiScreenAdminMenu.m_currentPage != MyGuiScreenAdminMenu.MyPageEnum.SafeZones)
        return;
      this.m_recreateInProgress = true;
      this.RefreshSafeZones();
      this.UpdateSelectedData();
      this.m_recreateInProgress = false;
    }

    private void MySafeZones_OnRemoveSafeZone(object sender, System.EventArgs e)
    {
      if (this.m_safeZonesCombo == null)
        return;
      if (this.m_selectedSafeZone == sender)
      {
        this.m_selectedSafeZone = (MySafeZone) null;
        this.RefreshSafeZones();
        this.m_selectedSafeZone = this.m_safeZonesCombo.GetItemsCount() > 0 ? (MySafeZone) Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_safeZonesCombo.GetItemByIndex(this.m_safeZonesCombo.GetItemsCount() - 1).Key) : (MySafeZone) null;
        this.m_recreateInProgress = true;
        this.UpdateSelectedData();
        this.m_recreateInProgress = false;
      }
      else
        this.m_safeZonesCombo.RemoveItem(((MyEntity) sender).EntityId);
    }

    private void RequestUpdateSafeZone()
    {
      if (this.m_selectedSafeZone == null)
        return;
      MySessionComponentSafeZones.RequestUpdateSafeZone((MyObjectBuilder_SafeZone) this.m_selectedSafeZone.GetObjectBuilder(false));
    }

    private void RefreshSafeZones()
    {
      this.m_safeZonesCombo.ClearItems();
      List<MySafeZone> list = MySessionComponentSafeZones.SafeZones.ToList<MySafeZone>();
      list.Sort((IComparer<MySafeZone>) new MyGuiScreenAdminMenu.MySafezoneNameComparer());
      foreach (MySafeZone mySafeZone in list)
      {
        if (mySafeZone.SafeZoneBlockId == 0L)
          this.m_safeZonesCombo.AddItem(mySafeZone.EntityId, mySafeZone.DisplayName != null ? mySafeZone.DisplayName : mySafeZone.ToString(), new int?(1));
      }
      if (this.m_selectedSafeZone == null)
        this.m_selectedSafeZone = this.m_safeZonesCombo.GetItemsCount() > 0 ? (MySafeZone) Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_safeZonesCombo.GetItemByIndex(this.m_safeZonesCombo.GetItemsCount() - 1).Key) : (MySafeZone) null;
      if (this.m_selectedSafeZone == null)
        return;
      this.m_safeZonesCombo.SelectItemByKey(this.m_selectedSafeZone.EntityId);
    }

    private void EnabledCheckedChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked && MySessionComponentSafeZones.IsSafeZoneColliding(this.m_selectedSafeZone.EntityId, this.m_selectedSafeZone.WorldMatrix, this.m_selectedSafeZone.Shape, this.m_selectedSafeZone.Radius, this.m_selectedSafeZone.Size))
      {
        checkBox.IsChecked = false;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.AdminScreen_Safezone_Collision), messageCaption: messageCaption));
      }
      else
      {
        if (this.m_selectedSafeZone.Enabled != checkBox.IsChecked)
        {
          this.m_selectedSafeZone.Enabled = checkBox.IsChecked;
          this.m_selectedSafeZone.RefreshGraphics();
        }
        this.RequestUpdateSafeZone();
      }
    }

    private void OnRadiusChange(MyGuiControlSlider slider)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (this.m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(this.m_selectedSafeZone.EntityId, this.m_selectedSafeZone.WorldMatrix, this.m_selectedSafeZone.Shape, slider.Value))
      {
        slider.Value = this.m_selectedSafeZone.Radius;
      }
      else
      {
        this.m_zoneRadiusValueLabel.Text = slider.Value.ToString();
        this.m_selectedSafeZone.Radius = slider.Value;
        this.m_selectedSafeZone.RecreatePhysics();
        this.RequestUpdateSafeZone();
      }
    }

    private void OnSizeChange(MyGuiControlSlider slider)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      Vector3 newSize = Vector3.Zero;
      float num = 0.0f;
      if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 0)
      {
        num = this.m_selectedSafeZone.Size.X;
        newSize = new Vector3(slider.Value, this.m_selectedSafeZone.Size.Y, this.m_selectedSafeZone.Size.Z);
      }
      else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 1)
      {
        num = this.m_selectedSafeZone.Size.Y;
        newSize = new Vector3(this.m_selectedSafeZone.Size.X, slider.Value, this.m_selectedSafeZone.Size.Z);
      }
      else if (this.m_safeZonesAxisCombo.GetSelectedIndex() == 2)
      {
        num = this.m_selectedSafeZone.Size.Z;
        newSize = new Vector3(this.m_selectedSafeZone.Size.X, this.m_selectedSafeZone.Size.Y, slider.Value);
      }
      if (this.m_selectedSafeZone.Enabled && MySessionComponentSafeZones.IsSafeZoneColliding(this.m_selectedSafeZone.EntityId, this.m_selectedSafeZone.WorldMatrix, this.m_selectedSafeZone.Shape, newSize: newSize))
      {
        slider.Value = num;
      }
      else
      {
        this.m_zoneRadiusValueLabel.Text = slider.Value.ToString();
        this.m_selectedSafeZone.Size = newSize;
        this.m_selectedSafeZone.RecreatePhysics();
        this.RequestUpdateSafeZone();
      }
    }

    private void DamageCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Damage;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Damage;
      this.RequestUpdateSafeZone();
    }

    private void ShootingCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Shooting;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Shooting;
      this.RequestUpdateSafeZone();
    }

    private void DrillingCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Drilling;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Drilling;
      this.RequestUpdateSafeZone();
    }

    private void WeldingCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Welding;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Welding;
      this.RequestUpdateSafeZone();
    }

    private void GrindingCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Grinding;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Grinding;
      this.RequestUpdateSafeZone();
    }

    private void VoxelHandCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.VoxelHand;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.VoxelHand;
      this.RequestUpdateSafeZone();
    }

    private void OnSettingsCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= (MySafeZoneAction) checkBox.UserData;
      else
        this.m_selectedSafeZone.AllowedActions &= ~(MySafeZoneAction) checkBox.UserData;
      this.RequestUpdateSafeZone();
    }

    private void BuildingCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.Building;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.Building;
      this.RequestUpdateSafeZone();
    }

    private void BuildingProjectionsCheckChanged(MyGuiControlCheckbox checkBox)
    {
      if (this.m_selectedSafeZone == null || this.m_recreateInProgress)
        return;
      if (checkBox.IsChecked)
        this.m_selectedSafeZone.AllowedActions |= MySafeZoneAction.BuildingProjections;
      else
        this.m_selectedSafeZone.AllowedActions &= ~MySafeZoneAction.BuildingProjections;
      this.RequestUpdateSafeZone();
    }

    private void RecreateSpectatorControls(bool constructor)
    {
      this.m_recreateInProgress = true;
      Vector2 vector2 = new Vector2(0.02f, 0.02f);
      float x = (float) ((double) MyGuiScreenAdminMenu.SCREEN_SIZE.X - (double) MyGuiScreenAdminMenu.HIDDEN_PART_RIGHT - (double) vector2.X * 2.0);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = "Camera Mode";
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
      this.m_cameraModeCombo = this.AddCombo();
      this.m_cameraModeCombo.AddItem(0L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_None));
      this.m_cameraModeCombo.AddItem(1L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Free));
      this.m_cameraModeCombo.AddItem(2L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Follow));
      this.m_cameraModeCombo.AddItem(3L, MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Orbit));
      this.m_cameraModeCombo.SelectItemByKey((long) component.GetMode());
      this.m_cameraModeCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnCameraModeSelected);
      this.m_currentPosition.Y += 0.015f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_CameraSmoothness);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = this.m_currentPosition + new Vector2(0.285f, 0.0f);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_cameraSmoothnessValueLabel = myGuiControlLabel3;
      this.Controls.Add((MyGuiControlBase) this.m_cameraSmoothnessValueLabel);
      this.m_currentPosition.Y += 0.03f;
      this.m_cameraSmoothness = new MyGuiControlSlider(new Vector2?(this.m_currentPosition + new Vector2(1f / 1000f, 0.0f)));
      this.m_cameraSmoothness.Size = new Vector2(0.285f, 1f);
      this.m_cameraSmoothness.Value = component.SmoothCameraLERP;
      this.m_cameraSmoothness.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_cameraSmoothness.ValueChanged += new Action<MyGuiControlSlider>(this.CameraSmoothnessChanged);
      this.m_cameraSmoothnessValueLabel.Text = component.SmoothCameraLERP.ToString("0.00#.##");
      this.Controls.Add((MyGuiControlBase) this.m_cameraSmoothness);
      this.m_currentPosition.Y += 0.03f;
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = this.m_currentPosition;
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel4.Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_SavedPositions);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.m_currentPosition.Y += 0.03f;
      this.m_trackedListbox = new MyGuiControlListbox(new Vector2?(Vector2.Zero), MyGuiControlListboxStyleEnum.Blueprints);
      this.m_trackedListbox.Size = new Vector2(x, 0.0f);
      this.m_trackedListbox.Enabled = true;
      this.m_trackedListbox.VisibleRowsCount = 10;
      this.m_trackedListbox.Position = this.m_trackedListbox.Size / 2f + this.m_currentPosition;
      this.m_trackedListbox.ItemClicked += new Action<MyGuiControlListbox>(this.OnListboxClicked);
      this.m_trackedListbox.MultiSelect = false;
      this.UpdateTrackedEntities();
      this.Controls.Add((MyGuiControlBase) this.m_trackedListbox);
      this.m_currentPosition.Y += 0.4f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Shortcuts);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel6.Text = this.GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_LOCK);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
      myGuiControlLabel7.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel7.Text = this.GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_NEXTPLAYER);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel();
      myGuiControlLabel8.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel8.Text = this.GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_PREVPLAYER);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel9.Text = this.GetControlButtonNameWithDesc(MyControlsSpace.SPECTATOR_SWITCHMODE);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel();
      myGuiControlLabel10.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel10.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel10.Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Save);
      myGuiControlLabel10.IsAutoScaleEnabled = true;
      myGuiControlLabel10.IsAutoEllipsisEnabled = true;
      MyGuiControlLabel myGuiControlLabel11 = myGuiControlLabel10;
      myGuiControlLabel11.SetMaxWidth(this.m_cameraSmoothness.Size.X);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel11);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel12 = new MyGuiControlLabel();
      myGuiControlLabel12.Position = this.m_currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel12.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel12.Text = MyTexts.GetString(MySpaceTexts.ScreenSpectatorAdminMenu_Load);
      myGuiControlLabel12.IsAutoScaleEnabled = true;
      myGuiControlLabel12.IsAutoEllipsisEnabled = true;
      MyGuiControlLabel myGuiControlLabel13 = myGuiControlLabel12;
      myGuiControlLabel13.SetMaxWidth(this.m_cameraSmoothness.Size.X);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel13);
      this.m_currentPosition.Y += 0.05f;
    }

    public string GetControlButtonNameWithDesc(MyStringId control)
    {
      MyControl gameControl = MyInput.Static.GetGameControl(control);
      StringBuilder output = new StringBuilder();
      gameControl.AppendBoundButtonNames(ref output, unassignedText: MyInput.Static.GetUnassignedName());
      return output.ToString() + " - " + MyTexts.GetString(gameControl.GetControlName());
    }

    private void UpdateTrackedEntities()
    {
      MySessionComponentSpectatorTools component = MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
      this.m_trackedListbox.Items.Clear();
      int num = 0;
      foreach (MyLockEntityState trackedSlot in (IEnumerable<MyLockEntityState>) component.TrackedSlots)
      {
        string entityDisplayName = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Empty);
        if (trackedSlot.LockEntityID != -1L)
          entityDisplayName = trackedSlot.LockEntityDisplayName;
        this.m_trackedListbox.Items.Add(new MyGuiControlListbox.Item(new StringBuilder("Num " + num.ToString() + " - " + entityDisplayName), userData: ((object) num++)));
      }
    }

    private void OnListboxClicked(MyGuiControlListbox obj)
    {
      if (this.m_trackedListbox.SelectedItems.Count <= 0)
        return;
      MySession.Static.GetComponent<MySessionComponentSpectatorTools>().SelectTrackedSlot((int) this.m_trackedListbox.SelectedItems[0].UserData);
    }

    private void OnCameraModeSelected() => MySession.Static.GetComponent<MySessionComponentSpectatorTools>().SetMode((MyCameraMode) this.m_cameraModeCombo.GetSelectedKey());

    private void CameraSmoothnessChanged(MyGuiControlSlider slider)
    {
      MySession.Static.GetComponent<MySessionComponentSpectatorTools>().SmoothCameraLERP = this.m_cameraSmoothness.Value;
      this.m_cameraSmoothnessValueLabel.Text = this.m_cameraSmoothness.Value.ToString("0.00#.##");
    }

    private void RecreateWeatherControls(bool constructor)
    {
      MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.WorldMatrix.Translation);
      this.m_recreateInProgress = true;
      this.m_currentPosition.Y += 0.03f;
      this.CreateDebugButton(0.16f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Generate, new Action<MyGuiControlButton>(this.GenerateWeather), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Weather_Generate_Tooltip), isAutoScaleEnabled: true);
      this.CreateDebugButton(0.16f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Lightning, new Action<MyGuiControlButton>(this.CreateLightning), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Weather_Lightning_Tooltip));
      this.m_currentPosition.Y += 0.05f;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Position = this.m_currentPosition + new Vector2(0.135f, 0.0f);
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      myGuiControlLabel.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_Weather_Modify);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += 0.05f;
      this.m_weatherSelectionCombo = this.AddCombo(textColor: new Vector4?(this.m_labelColor));
      this.m_weatherSelectionCombo.SetToolTip(MySpaceTexts.ScreenDebugAdminMenu_Weather_CreateCombo_Tooltip);
      this.m_weatherSelectionCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.M_weatherSelectionCombo_ItemSelected);
      this.m_weatherSelectionCombo.BorderColor = this.m_labelColor;
      this.m_weatherSelectionCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_weatherSelectionCombo.PositionX = -0.0225f;
      this.m_weatherSelectionCombo.Size = new Vector2(0.22f, 1f);
      this.PrepareWeatherDefinitions();
      this.m_currentPosition.Y -= 0.01f;
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Create, new Action<MyGuiControlButton>(this.CreateWeather), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Weather_Create_Tooltip));
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Replace, new Action<MyGuiControlButton>(this.ReplaceWeather), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Weather_Replace_Tooltip));
      this.CreateDebugButton(0.14f, MySpaceTexts.ScreenDebugAdminMenu_Weather_Remove, new Action<MyGuiControlButton>(this.RemoveWeather), tooltip: new MyStringId?(MySpaceTexts.ScreenDebugAdminMenu_Weather_Remove_Tooltip));
      this.m_currentPosition.Y += 0.05f;
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      controlMultilineText.Size = new Vector2(0.7f, 0.6f);
      controlMultilineText.Font = "Blue";
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText.Name = "weatherinfo";
      this.messageMultiline = controlMultilineText;
      this.Controls.Add((MyGuiControlBase) this.messageMultiline);
      this.UpdateWeatherInfo();
    }

    private void PrepareWeatherDefinitions()
    {
      this.m_definitions = MyDefinitionManager.Static.GetWeatherDefinitions().OrderBy<MyWeatherEffectDefinition, string>((Func<MyWeatherEffectDefinition, string>) (x => x.Id.SubtypeName)).ToArray<MyWeatherEffectDefinition>();
      this.m_weatherSelectionCombo.ClearItems();
      this.m_weatherNamesAndSubtypesDictionary = new Dictionary<int, Tuple<string, string>>();
      for (int key = 0; key < this.m_definitions.Length; ++key)
      {
        string keyString = this.m_definitions[key].Id.SubtypeId.ToString();
        string str = MyTexts.GetString(keyString);
        if (string.IsNullOrEmpty(str))
          str = keyString;
        this.m_weatherSelectionCombo.AddItem((long) key, str);
        this.m_weatherNamesAndSubtypesDictionary.Add(key, new Tuple<string, string>(str, keyString));
      }
    }

    private void M_weatherSelectionCombo_ItemSelected()
    {
      int selectedKey = (int) this.m_weatherSelectionCombo.GetSelectedKey();
      this.m_selectedWeatherSubtypeId = "Clear";
      if (!this.m_weatherNamesAndSubtypesDictionary.ContainsKey(selectedKey))
        return;
      this.m_selectedWeatherSubtypeId = this.m_weatherNamesAndSubtypesDictionary[selectedKey].Item2;
    }

    private void UpdateWeatherInfo()
    {
      if (this.m_weatherUpdateCounter-- != 0 || MySector.MainCamera == null)
        return;
      MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
      if (component == null)
        return;
      MyObjectBuilder_WeatherEffect weatherEffect;
      component.GetWeather(MySector.MainCamera.Position, out weatherEffect);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Current).ToString() + ": " + (weatherEffect != null ? (object) weatherEffect.Weather : (object) "None") + "\n");
      stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_CurrentIntensity).ToString() + ": " + (object) component.GetWeatherIntensity(MySector.MainCamera.Position) + "\n");
      stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_CurrentTemperature).ToString() + ": " + MySectorWeatherComponent.TemperatureToLevel(MySectorWeatherComponent.GetTemperatureInPoint(MySector.MainCamera.Position)).ToString() + "\n");
      if (weatherEffect != null)
      {
        if ((double) component.GetWeatherIntensity(MySector.MainCamera.Position) == 0.0)
          stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming).ToString() + ": " + weatherEffect.Weather + "\n");
        else
          stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming).ToString() + ": None\n");
      }
      else
        stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Incoming).ToString() + ": None\n");
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
      if (closestPlanet != null)
      {
        foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in component.GetWeatherPlanetData())
        {
          if (closestPlanet.EntityId == weatherPlanetData.PlanetId && weatherPlanetData.NextWeather > 0)
          {
            stringBuilder.Append(MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Weather_Next).ToString() + ": " + (object) (int) Math.Round((double) weatherPlanetData.NextWeather * 0.0166666675359011) + "s \n");
            break;
          }
        }
      }
      if (this.Controls.GetControlByName("weatherinfo") != null)
        (this.Controls.GetControlByName("weatherinfo") as MyGuiControlMultilineText).Text = stringBuilder;
      this.m_weatherUpdateCounter = 100;
    }

    private void GenerateWeather(MyGuiControlButton obj)
    {
      MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
      MyObjectBuilder_WeatherEffect weatherEffect;
      if (component.GetWeather(MySector.MainCamera.Position, out weatherEffect))
        component.RemoveWeather(weatherEffect);
      component.CreateRandomWeather(MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position));
      this.UpdateWeatherInfo();
    }

    private void CreateLightning(MyGuiControlButton obj)
    {
      IHitInfo hitInfo;
      MyAPIGateway.Physics.CastRay(MySector.MainCamera.WorldMatrix.Translation, MySector.MainCamera.WorldMatrix.Translation + MySector.MainCamera.WorldMatrix.Forward * 10000.0, out hitInfo);
      if (hitInfo != null)
      {
        MyObjectBuilder_WeatherLightning lightning = new MyObjectBuilder_WeatherLightning();
        MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
        string weather = component.GetWeather(MySector.MainCamera.WorldMatrix.Translation);
        if (weather != null)
        {
          MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather);
          if (weatherEffect != null && weatherEffect.Lightning != null)
            lightning = weatherEffect.Lightning;
        }
        component.CreateLightning(hitInfo.Position, lightning, true);
      }
      this.UpdateWeatherInfo();
    }

    private void CreateWeather(MyGuiControlButton obj)
    {
      if (this.m_selectedWeatherSubtypeId == null)
        this.m_selectedWeatherSubtypeId = "Clear";
      MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather(this.m_selectedWeatherSubtypeId, 0.0f, new Vector3D?(), false, (Vector3D) Vector3.Zero, 0, 1f);
      this.UpdateWeatherInfo();
    }

    private void ReplaceWeather(MyGuiControlButton obj)
    {
      MySectorWeatherComponent component = MySession.Static.GetComponent<MySectorWeatherComponent>();
      if (this.m_selectedWeatherSubtypeId == null)
        return;
      component.ReplaceWeather(this.m_selectedWeatherSubtypeId, new Vector3D?());
    }

    private void RemoveWeather(MyGuiControlButton obj)
    {
      MySession.Static.GetComponent<MySectorWeatherComponent>().SetWeather("Clear", 0.0f, new Vector3D?(), false, (Vector3D) Vector3.Zero, 0, 1f);
      this.UpdateWeatherInfo();
    }

    private enum TrashTab
    {
      General,
      Voxels,
    }

    public enum MyPageEnum
    {
      AdminTools,
      TrashRemoval,
      CycleObjects,
      EntityList,
      SafeZones,
      GlobalSafeZone,
      ReplayTool,
      Economy,
      Weather,
      Spectator,
      Match,
    }

    private struct MyIdNamePair
    {
      public long Id;
      public string Name;
    }

    private class MyIdNamePairComparer : IComparer<MyGuiScreenAdminMenu.MyIdNamePair>
    {
      public int Compare(MyGuiScreenAdminMenu.MyIdNamePair x, MyGuiScreenAdminMenu.MyIdNamePair y) => string.Compare(x.Name, y.Name);
    }

    [Serializable]
    internal struct AdminSettings
    {
      public MyTrashRemovalFlags Flags;
      public bool Enable;
      public int BlockCount;
      public float PlayerDistance;
      public int GridCount;
      public float PlayerInactivity;
      public int CharacterRemovalThreshold;
      public int AfkTimeout;
      public int StopGridsPeriod;
      public int RemoveOldIdentities;
      public bool VoxelEnable;
      public float VoxelDistanceFromPlayer;
      public float VoxelDistanceFromGrid;
      public int VoxelAge;
      public AdminSettingsEnum AdminSettingsFlags;

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EFlags\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, MyTrashRemovalFlags>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenAdminMenu.AdminSettings owner,
          in MyTrashRemovalFlags value)
        {
          owner.Flags = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenAdminMenu.AdminSettings owner,
          out MyTrashRemovalFlags value)
        {
          value = owner.Flags;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EEnable\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in bool value) => owner.Enable = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out bool value) => value = owner.Enable;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EBlockCount\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.BlockCount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.BlockCount;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EPlayerDistance\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in float value) => owner.PlayerDistance = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out float value) => value = owner.PlayerDistance;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EGridCount\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.GridCount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.GridCount;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EPlayerInactivity\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in float value) => owner.PlayerInactivity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out float value) => value = owner.PlayerInactivity;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003ECharacterRemovalThreshold\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.CharacterRemovalThreshold = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.CharacterRemovalThreshold;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EAfkTimeout\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.AfkTimeout = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.AfkTimeout;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EStopGridsPeriod\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.StopGridsPeriod = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.StopGridsPeriod;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003ERemoveOldIdentities\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.RemoveOldIdentities = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.RemoveOldIdentities;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EVoxelEnable\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in bool value) => owner.VoxelEnable = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out bool value) => value = owner.VoxelEnable;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EVoxelDistanceFromPlayer\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in float value) => owner.VoxelDistanceFromPlayer = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out float value) => value = owner.VoxelDistanceFromPlayer;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EVoxelDistanceFromGrid\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in float value) => owner.VoxelDistanceFromGrid = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out float value) => value = owner.VoxelDistanceFromGrid;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EVoxelAge\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGuiScreenAdminMenu.AdminSettings owner, in int value) => owner.VoxelAge = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGuiScreenAdminMenu.AdminSettings owner, out int value) => value = owner.VoxelAge;
      }

      protected class Sandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings\u003C\u003EAdminSettingsFlags\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenAdminMenu.AdminSettings, AdminSettingsEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenAdminMenu.AdminSettings owner,
          in AdminSettingsEnum value)
        {
          owner.AdminSettingsFlags = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenAdminMenu.AdminSettings owner,
          out AdminSettingsEnum value)
        {
          value = owner.AdminSettingsFlags;
        }
      }
    }

    public enum MyZoneAxisTypeEnum
    {
      X,
      Y,
      Z,
    }

    public enum MyRestrictedTypeEnum
    {
      Player,
      Faction,
      Grid,
      FloatingObjects,
    }

    private class MySafezoneNameComparer : IComparer<MySafeZone>
    {
      public int Compare(MySafeZone x, MySafeZone y)
      {
        if (x == null)
          return -1;
        return y == null ? 0 : string.Compare(x.DisplayName, y.DisplayName);
      }
    }

    protected sealed class RequestReputation\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerIdentityId,
        in long factionId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestReputation(playerIdentityId, factionId);
      }
    }

    protected sealed class RequestReputationCallback\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long playerIdentityId,
        in long factionId,
        in int reputation,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestReputationCallback(playerIdentityId, factionId, reputation);
      }
    }

    protected sealed class RequestChangeReputation\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, long, long, int, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in long factionId,
        in int reputationChange,
        in bool shouldPropagate,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestChangeReputation(identityId, factionId, reputationChange, shouldPropagate);
      }
    }

    protected sealed class RequestBalance\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long accountOwner,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestBalance(accountOwner);
      }
    }

    protected sealed class RequestBalanceCallback\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long accountOwner,
        in long balance,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestBalanceCallback(accountOwner, balance);
      }
    }

    protected sealed class RequestChange\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long accountOwner,
        in long balanceChange,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RequestChange(accountOwner, balanceChange);
      }
    }

    protected sealed class RequestSettingFromServer_Implementation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.RequestSettingFromServer_Implementation();
      }
    }

    protected sealed class AskIsValidForEdit_Server\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.AskIsValidForEdit_Server(entityId);
      }
    }

    protected sealed class AskIsValidForEdit_Reponse\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in bool canEdit,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.AskIsValidForEdit_Reponse(entityId, canEdit);
      }
    }

    protected sealed class EntityListRequest\u003C\u003ESandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityTypeEnum\u0023System_Boolean : ICallSite<IMyEventOwner, MyEntityList.MyEntityTypeEnum, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyEntityList.MyEntityTypeEnum selectedType,
        in bool requestedBySafeZoneFilter,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.EntityListRequest(selectedType, requestedBySafeZoneFilter);
      }
    }

    protected sealed class CycleRequest_Implementation\u003C\u003ESandbox_Game_Entities_MyEntityCyclingOrder\u0023System_Boolean\u0023System_Boolean\u0023System_Single\u0023System_Int64\u0023Sandbox_Game_Entities_CyclingOptions : ICallSite<IMyEventOwner, MyEntityCyclingOrder, bool, bool, float, long, CyclingOptions>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyEntityCyclingOrder order,
        in bool reset,
        in bool findLarger,
        in float metricValue,
        in long currentEntityId,
        in CyclingOptions options)
      {
        MyGuiScreenAdminMenu.CycleRequest_Implementation(order, reset, findLarger, metricValue, currentEntityId, options);
      }
    }

    protected sealed class RemoveOwner_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int64\u003E\u0023System_Collections_Generic_List`1\u003CSystem_Int64\u003E : ICallSite<IMyEventOwner, List<long>, List<long>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<long> owners,
        in List<long> entityIds,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.RemoveOwner_Implementation(owners, entityIds);
      }
    }

    protected sealed class ProceedEntitiesAction_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int64\u003E\u0023Sandbox_Game_Entities_MyEntityList\u003C\u003EEntityListAction : ICallSite<IMyEventOwner, List<long>, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<long> entityIds,
        in MyEntityList.EntityListAction action,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.ProceedEntitiesAction_Implementation(entityIds, action);
      }
    }

    protected sealed class UploadSettingsToServer\u003C\u003ESandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings : ICallSite<IMyEventOwner, MyGuiScreenAdminMenu.AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGuiScreenAdminMenu.AdminSettings settings,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.UploadSettingsToServer(settings);
      }
    }

    protected sealed class ProceedEntity_Implementation\u003C\u003ESystem_Int64\u0023Sandbox_Game_Entities_MyEntityList\u003C\u003EEntityListAction : ICallSite<IMyEventOwner, long, MyEntityList.EntityListAction, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in MyEntityList.EntityListAction action,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.ProceedEntity_Implementation(entityId, action);
      }
    }

    protected sealed class ReplicateEverything_Implementation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.ReplicateEverything_Implementation();
      }
    }

    protected sealed class AdminSettingsChanged\u003C\u003ESandbox_Game_World_AdminSettingsEnum\u0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in AdminSettingsEnum settings,
        in ulong steamId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.AdminSettingsChanged(settings, steamId);
      }
    }

    protected sealed class AdminSettingsChangedClient\u003C\u003ESandbox_Game_World_AdminSettingsEnum\u0023System_UInt64 : ICallSite<IMyEventOwner, AdminSettingsEnum, ulong, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in AdminSettingsEnum settings,
        in ulong steamId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.AdminSettingsChangedClient(settings, steamId);
      }
    }

    protected sealed class EntityListResponse\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003E : ICallSite<IMyEventOwner, List<MyEntityList.MyEntityListInfoItem>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyEntityList.MyEntityListInfoItem> entities,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.EntityListResponse(entities);
      }
    }

    protected sealed class Cycle_Implementation\u003C\u003ESystem_Single\u0023System_Int64\u0023VRageMath_Vector3D\u0023System_Boolean : ICallSite<IMyEventOwner, float, long, Vector3D, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float newMetricValue,
        in long newEntityId,
        in Vector3D position,
        in bool isNpcStation,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.Cycle_Implementation(newMetricValue, newEntityId, position, isNpcStation);
      }
    }

    protected sealed class DownloadSettingFromServer\u003C\u003ESandbox_Game_Gui_MyGuiScreenAdminMenu\u003C\u003EAdminSettings : ICallSite<IMyEventOwner, MyGuiScreenAdminMenu.AdminSettings, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGuiScreenAdminMenu.AdminSettings settings,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.DownloadSettingFromServer(settings);
      }
    }

    protected sealed class SendDataToClient\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.SendDataToClient();
      }
    }

    protected sealed class ReciveClientData\u003C\u003E_MyMatchState\u0023System_Single\u0023System_Boolean\u0023System_Boolean : ICallSite<IMyEventOwner, MyMatchState, float, bool, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyMatchState state,
        in float remainingTime,
        in bool isRunning,
        in bool isEnabled,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.ReciveClientData(state, remainingTime, isRunning, isEnabled);
      }
    }

    protected sealed class StartMatchInternal\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.StartMatchInternal();
      }
    }

    protected sealed class StopMatchInternal\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.StopMatchInternal();
      }
    }

    protected sealed class PauseMatchInternal\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.PauseMatchInternal();
      }
    }

    protected sealed class ProgressMatchInternal\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenAdminMenu.ProgressMatchInternal();
      }
    }

    protected sealed class SetTimeInternal\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float value,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.SetTimeInternal(value);
      }
    }

    protected sealed class AddTimeInternal\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float value,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenAdminMenu.AddTimeInternal(value);
      }
    }
  }
}
