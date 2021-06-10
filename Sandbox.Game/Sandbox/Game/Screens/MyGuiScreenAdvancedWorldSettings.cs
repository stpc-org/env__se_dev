// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenAdvancedWorldSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiScreenAdvancedWorldSettings : MyGuiScreenBase
  {
    private const int MIN_DAY_TIME_MINUTES = 1;
    private const int MAX_DAY_TIME_MINUTES = 1440;
    private readonly float MIN_SAFE_TIME_FOR_SUN = 0.4668752f;
    private MyGuiScreenWorldSettings m_parent;
    private bool m_isNewGame;
    private bool m_isConfirmed;
    private bool m_showWarningForOxygen;
    private bool m_recreating_control;
    private bool m_isHostilityChanged;
    private MyGuiControlTextbox m_passwordTextbox;
    private MyGuiControlCombobox m_onlineMode;
    private MyGuiControlCombobox m_worldSizeCombo;
    private MyGuiControlCombobox m_spawnShipTimeCombo;
    private MyGuiControlCombobox m_viewDistanceCombo;
    private MyGuiControlCombobox m_physicsOptionsCombo;
    private MyGuiControlCombobox m_assembler;
    private MyGuiControlCombobox m_charactersInventory;
    private MyGuiControlCombobox m_refinery;
    private MyGuiControlCombobox m_welder;
    private MyGuiControlCombobox m_grinder;
    private MyGuiControlCombobox m_soundModeCombo;
    private MyGuiControlCombobox m_asteroidAmountCombo;
    private MyGuiControlCombobox m_environment;
    private MyGuiControlCombobox m_blocksInventory;
    private MyGuiControlCheckbox m_autoHealing;
    private MyGuiControlCheckbox m_enableCopyPaste;
    private MyGuiControlCheckbox m_weaponsEnabled;
    private MyGuiControlCheckbox m_showPlayerNamesOnHud;
    private MyGuiControlCheckbox m_thrusterDamage;
    private MyGuiControlCheckbox m_cargoShipsEnabled;
    private MyGuiControlCheckbox m_enableSpectator;
    private MyGuiControlCheckbox m_respawnShipDelete;
    private MyGuiControlCheckbox m_resetOwnership;
    private MyGuiControlCheckbox m_permanentDeath;
    private MyGuiControlCheckbox m_destructibleBlocks;
    private MyGuiControlCheckbox m_enableIngameScripts;
    private MyGuiControlCheckbox m_enableToolShake;
    private MyGuiControlCheckbox m_enableOxygen;
    private MyGuiControlCheckbox m_enableOxygenPressurization;
    private MyGuiControlCheckbox m_enable3rdPersonCamera;
    private MyGuiControlCheckbox m_enableEncounters;
    private MyGuiControlCheckbox m_enableRespawnShips;
    private MyGuiControlCheckbox m_scenarioEditMode;
    private MyGuiControlCheckbox m_enableConvertToStation;
    private MyGuiControlCheckbox m_enableStationVoxelSupport;
    private MyGuiControlCheckbox m_enableSunRotation;
    private MyGuiControlCheckbox m_enableJetpack;
    private MyGuiControlCheckbox m_spawnWithTools;
    private MyGuiControlCheckbox m_enableVoxelDestruction;
    private MyGuiControlCheckbox m_enableDrones;
    private MyGuiControlCheckbox m_enableWolfs;
    private MyGuiControlCheckbox m_enableSpiders;
    private MyGuiControlCheckbox m_enableRemoteBlockRemoval;
    private MyGuiControlCheckbox m_enableContainerDrops;
    private MyGuiControlCheckbox m_blockLimits;
    private MyGuiControlCheckbox m_enableTurretsFriendlyFire;
    private MyGuiControlCheckbox m_enableSubGridDamage;
    private MyGuiControlCheckbox m_enableRealisticDampeners;
    private MyGuiControlCheckbox m_enableAdaptiveSimulationQuality;
    private MyGuiControlCheckbox m_enableVoxelHand;
    private MyGuiControlCheckbox m_enableResearch;
    private MyGuiControlCheckbox m_enableAutoRespawn;
    private MyGuiControlCheckbox m_enableSupergridding;
    private MyGuiControlCheckbox m_enableEconomy;
    private MyGuiControlCheckbox m_enableWeatherSystem;
    private MyGuiControlCheckbox m_enableBountyContracts;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlButton m_survivalModeButton;
    private MyGuiControlButton m_creativeModeButton;
    private MyGuiControlSlider m_maxPlayersSlider;
    private MyGuiControlSlider m_sunRotationIntervalSlider;
    private MyGuiControlLabel m_enableCopyPasteLabel;
    private MyGuiControlLabel m_maxPlayersLabel;
    private MyGuiControlLabel m_maxFloatingObjectsLabel;
    private MyGuiControlLabel m_maxBackupSavesLabel;
    private MyGuiControlLabel m_sunRotationPeriod;
    private MyGuiControlLabel m_sunRotationPeriodValue;
    private MyGuiControlLabel m_enableWolfsLabel;
    private MyGuiControlLabel m_enableSpidersLabel;
    private MyGuiControlLabel m_maxGridSizeValue;
    private MyGuiControlLabel m_maxBlocksPerPlayerValue;
    private MyGuiControlLabel m_totalPCUValue;
    private MyGuiControlLabel m_maxBackupSavesValue;
    private MyGuiControlLabel m_maxFloatingObjectsValue;
    private MyGuiControlLabel m_enableContainerDropsLabel;
    private MyGuiControlLabel m_optimalSpawnDistanceValue;
    private MyGuiControlSlider m_maxFloatingObjectsSlider;
    private MyGuiControlSlider m_maxGridSizeSlider;
    private MyGuiControlSlider m_maxBlocksPerPlayerSlider;
    private MyGuiControlSlider m_totalPCUSlider;
    private MyGuiControlSlider m_optimalSpawnDistanceSlider;
    private MyGuiControlSlider m_maxBackupSavesSlider;
    private StringBuilder m_tempBuilder = new StringBuilder();
    private int m_customWorldSize;
    private int m_customViewDistance = 20000;
    private int? m_asteroidAmount;

    public int AsteroidAmount
    {
      get => !this.m_asteroidAmount.HasValue ? -1 : this.m_asteroidAmount.Value;
      set
      {
        this.m_asteroidAmount = new int?(value);
        switch (value)
        {
          case -4:
            this.m_asteroidAmountCombo.SelectItemByKey(-4L);
            break;
          case -3:
            this.m_asteroidAmountCombo.SelectItemByKey(-3L);
            break;
          case -2:
            this.m_asteroidAmountCombo.SelectItemByKey(-2L);
            break;
          case -1:
            this.m_asteroidAmountCombo.SelectItemByKey(-1L);
            break;
          case 0:
            this.m_asteroidAmountCombo.SelectItemByKey(0L);
            break;
          case 4:
            this.m_asteroidAmountCombo.SelectItemByKey(4L);
            break;
          case 7:
            this.m_asteroidAmountCombo.SelectItemByKey(7L);
            break;
          case 16:
            this.m_asteroidAmountCombo.SelectItemByKey(16L);
            break;
        }
      }
    }

    public string Password => this.m_passwordTextbox.Text;

    public bool IsConfirmed => this.m_isConfirmed;

    public event Action OnOkButtonClicked;

    public MyGuiScreenAdvancedWorldSettings(MyGuiScreenWorldSettings parent)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(MyGuiScreenAdvancedWorldSettings.CalcSize()), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedWorldSettings.ctor START");
      this.m_parent = parent;
      this.EnabledBackgroundFade = true;
      this.m_isNewGame = parent.Checkpoint == null;
      this.m_isConfirmed = false;
      this.RecreateControls(true);
      this.m_isHostilityChanged = !this.m_isNewGame;
      MySandboxGame.Log.WriteLine("MyGuiScreenAdvancedWorldSettings.ctor END");
    }

    public static Vector2 CalcSize() => new Vector2(0.6535714f, 0.9398855f);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_recreating_control = true;
      this.BuildControls();
      this.LoadValues();
      this.m_recreating_control = false;
    }

    public void BuildControls()
    {
      Vector2 vector2_1 = new Vector2(50f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float y = this.m_isNewGame ? 2.138f : 2.0855f;
      if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
        y -= 0.07f;
      MyGuiControlParent guiControlParent = new MyGuiControlParent(size: new Vector2?(new Vector2(this.Size.Value.X - vector2_1.X * 2f, y)));
      MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) guiControlParent);
      controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel.ScrollbarVEnabled = true;
      controlScrollablePanel.Size = new Vector2((float) ((double) this.Size.Value.X - (double) vector2_1.X * 2.0 - 0.0350000001490116), 0.74f);
      controlScrollablePanel.Position = new Vector2(-0.27f, -0.394f);
      this.AddCaption(MyCommonTexts.ScreenCaptionAdvancedSettings, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      int num1 = 0;
      float maxSize = 0.205f;
      this.MakeLabel(MySpaceTexts.WorldSettings_Password);
      this.MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
      this.m_maxPlayersLabel = this.MakeLabel(MyCommonTexts.MaxPlayers);
      this.m_maxFloatingObjectsLabel = this.MakeLabel(MySpaceTexts.MaxFloatingObjects);
      this.m_maxBackupSavesLabel = this.MakeLabel(MySpaceTexts.MaxBackupSaves, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel1 = this.MakeLabel(MySpaceTexts.WorldSettings_EnvironmentHostility);
      MyGuiControlLabel myGuiControlLabel2 = this.MakeLabel(MySpaceTexts.WorldSettings_MaxGridSize);
      this.m_maxGridSizeValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_maxGridSizeValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlLabel myGuiControlLabel3 = this.MakeLabel(MySpaceTexts.WorldSettings_MaxBlocksPerPlayer);
      this.m_maxBlocksPerPlayerValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_maxBlocksPerPlayerValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlLabel myGuiControlLabel4 = this.MakeLabel(MySpaceTexts.WorldSettings_TotalPCU);
      this.m_totalPCUValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_totalPCUValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      MyGuiControlLabel myGuiControlLabel5 = this.MakeLabel(MySpaceTexts.WorldSettings_OptimalSpawnDistance, true, maxSize, true);
      this.m_optimalSpawnDistanceValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_optimalSpawnDistanceValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_maxFloatingObjectsValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_maxFloatingObjectsValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_maxBackupSavesValue = this.MakeLabel(MyCommonTexts.Disabled);
      this.m_maxBackupSavesValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_sunRotationPeriod = this.MakeLabel(MySpaceTexts.SunRotationPeriod);
      this.m_sunRotationPeriodValue = this.MakeLabel(MySpaceTexts.SunRotationPeriod);
      this.m_sunRotationPeriodValue.SetMaxWidth(0.065f);
      this.m_sunRotationPeriodValue.IsAutoEllipsisEnabled = true;
      this.m_sunRotationPeriodValue.IsAutoScaleEnabled = true;
      this.m_sunRotationPeriodValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.MakeLabel(MyCommonTexts.WorldSettings_GameMode);
      this.MakeLabel(MySpaceTexts.WorldSettings_GameStyle);
      this.MakeLabel(MySpaceTexts.WorldSettings_Scenario);
      MyGuiControlLabel myGuiControlLabel6 = this.MakeLabel(MySpaceTexts.WorldSettings_AutoHealing);
      MyGuiControlLabel myGuiControlLabel7 = this.MakeLabel(MySpaceTexts.WorldSettings_ThrusterDamage, true, maxSize, true);
      MyGuiControlLabel label1 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableSpectator);
      MyGuiControlLabel label2 = this.MakeLabel(MySpaceTexts.WorldSettings_ResetOwnership);
      MyGuiControlLabel label3 = this.MakeLabel(MySpaceTexts.WorldSettings_PermanentDeath);
      MyGuiControlLabel myGuiControlLabel8 = this.MakeLabel(MySpaceTexts.WorldSettings_DestructibleBlocks);
      MyGuiControlLabel label4 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableIngameScripts);
      MyGuiControlLabel myGuiControlLabel9 = this.MakeLabel(MySpaceTexts.WorldSettings_Enable3rdPersonCamera);
      MyGuiControlLabel myGuiControlLabel10 = this.MakeLabel(MySpaceTexts.WorldSettings_Encounters);
      MyGuiControlLabel myGuiControlLabel11 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableToolShake, true, maxSize, true);
      MyGuiControlLabel label5 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableAdaptiveSimulationQuality, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel12 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableVoxelHand);
      MyGuiControlLabel myGuiControlLabel13 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableCargoShips);
      this.m_enableCopyPasteLabel = this.MakeLabel(MySpaceTexts.WorldSettings_EnableCopyPaste);
      MyGuiControlLabel myGuiControlLabel14 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableWeapons);
      MyGuiControlLabel myGuiControlLabel15 = this.MakeLabel(MySpaceTexts.WorldSettings_ShowPlayerNamesOnHud);
      MyGuiControlLabel myGuiControlLabel16 = this.MakeLabel(MySpaceTexts.WorldSettings_CharactersInventorySize, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel17 = this.MakeLabel(MySpaceTexts.WorldSettings_BlocksInventorySize, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel18 = this.MakeLabel(MySpaceTexts.WorldSettings_RefinerySpeed);
      MyGuiControlLabel myGuiControlLabel19 = this.MakeLabel(MySpaceTexts.WorldSettings_AssemblerEfficiency);
      MyGuiControlLabel myGuiControlLabel20 = this.MakeLabel(MySpaceTexts.World_Settings_EnableOxygen);
      MyGuiControlLabel oxygenPressurizationLabel = this.MakeLabel(MySpaceTexts.World_Settings_EnableOxygenPressurization);
      MyGuiControlLabel myGuiControlLabel21 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableRespawnShips, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel22 = this.MakeLabel(MySpaceTexts.WorldSettings_RespawnShipDelete, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel23 = this.MakeLabel(MySpaceTexts.WorldSettings_LimitWorldSize);
      MyGuiControlLabel myGuiControlLabel24 = this.MakeLabel(MySpaceTexts.WorldSettings_WelderSpeed);
      MyGuiControlLabel myGuiControlLabel25 = this.MakeLabel(MySpaceTexts.WorldSettings_GrinderSpeed);
      MyGuiControlLabel myGuiControlLabel26 = this.MakeLabel(MySpaceTexts.WorldSettings_RespawnShipCooldown, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel27 = this.MakeLabel(MySpaceTexts.WorldSettings_ViewDistance);
      MyGuiControlLabel myGuiControlLabel28 = this.MakeLabel(MyCommonTexts.WorldSettings_Physics);
      MyGuiControlLabel label6 = this.MakeLabel(MyCommonTexts.WorldSettings_BlockLimits);
      MyGuiControlLabel myGuiControlLabel29 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableConvertToStation);
      MyGuiControlLabel label7 = this.MakeLabel(MySpaceTexts.WorldSettings_StationVoxelSupport);
      MyGuiControlLabel myGuiControlLabel30 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableSunRotation);
      MyGuiControlLabel myGuiControlLabel31 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableTurrerFriendlyDamage, true, maxSize, true);
      MyGuiControlLabel label8 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableSubGridDamage, true, maxSize, true);
      this.MakeLabel(MySpaceTexts.WorldSettings_EnableRealisticDampeners);
      MyGuiControlLabel myGuiControlLabel32 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableJetpack);
      MyGuiControlLabel myGuiControlLabel33 = this.MakeLabel(MySpaceTexts.WorldSettings_SpawnWithTools);
      MyGuiControlLabel myGuiControlLabel34 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableDrones);
      this.m_enableWolfsLabel = this.MakeLabel(MySpaceTexts.WorldSettings_EnableWolfs);
      this.m_enableSpidersLabel = this.MakeLabel(MySpaceTexts.WorldSettings_EnableSpiders);
      MyGuiControlLabel myGuiControlLabel35 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableRemoteBlockRemoval);
      MyGuiControlLabel myGuiControlLabel36 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableResearch);
      MyGuiControlLabel myGuiControlLabel37 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableAutorespawn, true, maxSize, true);
      MyGuiControlLabel label9 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableSupergridding);
      MyGuiControlLabel myGuiControlLabel38 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableBountyContracts, true, maxSize, true);
      MyGuiControlLabel myGuiControlLabel39 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableEconomy);
      MyGuiControlLabel myGuiControlLabel40 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableWeatherSystem, true, maxSize, true);
      this.m_enableContainerDropsLabel = this.MakeLabel(MySpaceTexts.WorldSettings_EnableContainerDrops);
      MyGuiControlLabel myGuiControlLabel41 = this.MakeLabel(MySpaceTexts.WorldSettings_EnableVoxelDestruction);
      MyGuiControlLabel myGuiControlLabel42 = this.MakeLabel(MySpaceTexts.WorldSettings_SoundMode);
      MyGuiControlLabel myGuiControlLabel43 = this.MakeLabel(MySpaceTexts.Asteroid_Amount);
      float x1 = 0.309375f;
      this.m_passwordTextbox = new MyGuiControlTextbox(maxLength: 256);
      this.m_onlineMode = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_environment = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_environment.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnvironment));
      this.m_environment.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.HostilityChanged);
      this.m_autoHealing = new MyGuiControlCheckbox();
      this.m_thrusterDamage = new MyGuiControlCheckbox();
      this.m_cargoShipsEnabled = new MyGuiControlCheckbox();
      this.m_enableSpectator = new MyGuiControlCheckbox();
      this.m_resetOwnership = new MyGuiControlCheckbox();
      this.m_permanentDeath = new MyGuiControlCheckbox();
      this.m_destructibleBlocks = new MyGuiControlCheckbox();
      this.m_enableIngameScripts = new MyGuiControlCheckbox();
      this.m_enable3rdPersonCamera = new MyGuiControlCheckbox();
      this.m_enableEncounters = new MyGuiControlCheckbox();
      this.m_enableRespawnShips = new MyGuiControlCheckbox();
      this.m_enableToolShake = new MyGuiControlCheckbox();
      this.m_enableAdaptiveSimulationQuality = new MyGuiControlCheckbox();
      this.m_enableVoxelHand = new MyGuiControlCheckbox();
      this.m_enableOxygen = new MyGuiControlCheckbox();
      this.m_enableOxygen.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (x =>
      {
        if (this.m_showWarningForOxygen && x.IsChecked)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.MessageBoxTextAreYouSureEnableOxygen), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (v =>
          {
            if (v != MyGuiScreenMessageBox.ResultEnum.NO)
              return;
            x.IsChecked = false;
          }))));
        if (!x.IsChecked)
        {
          this.m_enableOxygenPressurization.IsChecked = false;
          this.m_enableOxygenPressurization.Enabled = false;
          oxygenPressurizationLabel.Enabled = false;
        }
        else
        {
          this.m_enableOxygenPressurization.Enabled = true;
          oxygenPressurizationLabel.Enabled = true;
        }
      });
      this.m_enableOxygenPressurization = new MyGuiControlCheckbox();
      if (!this.m_enableOxygen.IsChecked)
      {
        this.m_enableOxygenPressurization.Enabled = false;
        oxygenPressurizationLabel.Enabled = false;
      }
      this.m_enableCopyPaste = new MyGuiControlCheckbox();
      this.m_weaponsEnabled = new MyGuiControlCheckbox();
      this.m_showPlayerNamesOnHud = new MyGuiControlCheckbox();
      this.m_enableSunRotation = new MyGuiControlCheckbox();
      this.m_enableSunRotation.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (control =>
      {
        this.m_sunRotationIntervalSlider.Enabled = control.IsChecked;
        this.m_sunRotationPeriodValue.Visible = control.IsChecked;
      });
      this.m_enableJetpack = new MyGuiControlCheckbox();
      this.m_spawnWithTools = new MyGuiControlCheckbox();
      this.m_enableAutoRespawn = new MyGuiControlCheckbox();
      this.m_enableSupergridding = new MyGuiControlCheckbox();
      this.m_enableBountyContracts = new MyGuiControlCheckbox();
      this.m_enableEconomy = new MyGuiControlCheckbox();
      this.m_enableWeatherSystem = new MyGuiControlCheckbox();
      this.m_enableConvertToStation = new MyGuiControlCheckbox();
      this.m_enableStationVoxelSupport = new MyGuiControlCheckbox();
      Vector2? position = new Vector2?(Vector2.Zero);
      float num2 = this.m_onlineMode.Size.X * 0.9f;
      double maxPlayers = (double) MyMultiplayerLobby.MAX_PLAYERS;
      double num3 = (double) num2;
      float? defaultValue = new float?();
      Vector4? color = new Vector4?();
      string labelText = new StringBuilder("{0}").ToString();
      this.m_maxPlayersSlider = new MyGuiControlSlider(position, 2f, (float) maxPlayers, (float) num3, defaultValue, color, labelText, 0, labelSpaceWidth: 0.05f, intValue: true);
      this.m_maxFloatingObjectsSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), 16f, 56f, this.m_onlineMode.Size.X * 0.9f, labelDecimalPlaces: 0, labelScale: 0.7f, labelSpaceWidth: 0.05f, intValue: true);
      this.m_maxFloatingObjectsSlider.Value = 0.0f;
      if (MySandboxGame.Config.ExperimentalMode)
        this.m_maxFloatingObjectsSlider.MaxValue = 1024f;
      this.m_maxGridSizeSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), maxValue: 50000f, width: (this.m_onlineMode.Size.X * 0.9f), labelSpaceWidth: 0.05f, intValue: true);
      this.m_maxBlocksPerPlayerSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), maxValue: 100000f, width: (this.m_onlineMode.Size.X * 0.9f), labelSpaceWidth: 0.05f, intValue: true);
      this.m_totalPCUSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), 100f, 100000f, this.m_onlineMode.Size.X * 0.9f, labelSpaceWidth: 0.05f, intValue: true);
      this.m_totalPCUSlider.Value = 0.0f;
      if (MySandboxGame.Config.ExperimentalMode)
      {
        this.m_totalPCUSlider.MinValue = 0.0f;
        this.m_totalPCUSlider.MaxValue = (float) (MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX ?? 1000000);
      }
      this.m_maxBackupSavesSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), maxValue: 1000f, width: (this.m_onlineMode.Size.X * 0.9f), labelDecimalPlaces: 0, labelSpaceWidth: 0.05f, intValue: true);
      this.m_optimalSpawnDistanceSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), 900f, 25000f, this.m_onlineMode.Size.X * 0.9f, labelSpaceWidth: 0.05f, intValue: true);
      this.m_enableVoxelDestruction = new MyGuiControlCheckbox();
      this.m_enableDrones = new MyGuiControlCheckbox();
      this.m_enableWolfs = new MyGuiControlCheckbox();
      this.m_enableSpiders = new MyGuiControlCheckbox();
      this.m_enableRemoteBlockRemoval = new MyGuiControlCheckbox();
      this.m_enableContainerDrops = new MyGuiControlCheckbox();
      this.m_enableTurretsFriendlyFire = new MyGuiControlCheckbox();
      this.m_enableSubGridDamage = new MyGuiControlCheckbox();
      this.m_enableRealisticDampeners = new MyGuiControlCheckbox();
      this.m_enableResearch = new MyGuiControlCheckbox();
      this.m_respawnShipDelete = new MyGuiControlCheckbox();
      this.m_worldSizeCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_spawnShipTimeCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_soundModeCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_soundModeCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_soundModeCombo_ItemSelected);
      this.m_asteroidAmountCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_assembler = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_charactersInventory = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_blocksInventory = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_refinery = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_welder = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_grinder = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_viewDistanceCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_physicsOptionsCombo = new MyGuiControlCombobox(size: new Vector2?(new Vector2(x1, 0.04f)));
      this.m_creativeModeButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Small, text: MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative), onButtonClick: new Action<MyGuiControlButton>(this.CreativeClicked));
      this.m_creativeModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeCreative);
      this.m_survivalModeButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Small, text: MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival), onButtonClick: new Action<MyGuiControlButton>(this.SurvivalClicked));
      this.m_survivalModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeSurvival);
      if (MyFakes.ENABLE_ASTEROID_FIELDS)
      {
        this.m_asteroidAmountCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_asteroidAmountCombo_ItemSelected);
        this.m_asteroidAmountCombo.AddItem(-4L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNone);
        this.m_asteroidAmountCombo.AddItem(-5L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLowest);
        this.m_asteroidAmountCombo.AddItem(-1L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralLow);
        this.m_asteroidAmountCombo.AddItem(-2L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralNormal);
        if (MySandboxGame.Config.ExperimentalMode)
          this.m_asteroidAmountCombo.AddItem(-3L, MySpaceTexts.WorldSettings_AsteroidAmountProceduralHigh);
        this.m_asteroidAmountCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAsteroidAmount));
      }
      this.m_soundModeCombo.AddItem(0L, MySpaceTexts.WorldSettings_ArcadeSound);
      this.m_soundModeCombo.AddItem(1L, MySpaceTexts.WorldSettings_RealisticSound);
      this.m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
      this.m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
      this.m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
      this.m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
      this.m_environment.AddItem(0L, MySpaceTexts.WorldSettings_EnvironmentHostilitySafe);
      this.m_environment.AddItem(1L, MySpaceTexts.WorldSettings_EnvironmentHostilityNormal);
      this.m_environment.AddItem(2L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysm);
      this.m_environment.AddItem(3L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysmUnreal);
      this.m_worldSizeCombo.AddItem(0L, MySpaceTexts.WorldSettings_WorldSize10Km);
      this.m_worldSizeCombo.AddItem(1L, MySpaceTexts.WorldSettings_WorldSize20Km);
      this.m_worldSizeCombo.AddItem(2L, MySpaceTexts.WorldSettings_WorldSize50Km);
      this.m_worldSizeCombo.AddItem(3L, MySpaceTexts.WorldSettings_WorldSize100Km);
      this.m_worldSizeCombo.AddItem(4L, MySpaceTexts.WorldSettings_WorldSizeUnlimited);
      this.m_spawnShipTimeCombo.AddItem(0L, MySpaceTexts.WorldSettings_RespawnShip_CooldownsDisabled);
      this.m_spawnShipTimeCombo.AddItem(1L, MySpaceTexts.WorldSettings_RespawnShip_x01);
      this.m_spawnShipTimeCombo.AddItem(2L, MySpaceTexts.WorldSettings_RespawnShip_x02);
      this.m_spawnShipTimeCombo.AddItem(5L, MySpaceTexts.WorldSettings_RespawnShip_x05);
      this.m_spawnShipTimeCombo.AddItem(10L, MySpaceTexts.WorldSettings_RespawnShip_Default);
      this.m_spawnShipTimeCombo.AddItem(20L, MySpaceTexts.WorldSettings_RespawnShip_x2);
      this.m_spawnShipTimeCombo.AddItem(50L, MySpaceTexts.WorldSettings_RespawnShip_x5);
      this.m_spawnShipTimeCombo.AddItem(100L, MySpaceTexts.WorldSettings_RespawnShip_x10);
      this.m_spawnShipTimeCombo.AddItem(200L, MySpaceTexts.WorldSettings_RespawnShip_x20);
      this.m_spawnShipTimeCombo.AddItem(500L, MySpaceTexts.WorldSettings_RespawnShip_x50);
      this.m_spawnShipTimeCombo.AddItem(1000L, MySpaceTexts.WorldSettings_RespawnShip_x100);
      this.m_assembler.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
      this.m_assembler.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
      this.m_assembler.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
      this.m_charactersInventory.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
      this.m_charactersInventory.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
      this.m_charactersInventory.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_x5);
      this.m_charactersInventory.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
      this.m_blocksInventory.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
      this.m_blocksInventory.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
      this.m_blocksInventory.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_x5);
      this.m_blocksInventory.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
      this.m_refinery.AddItem(1L, MySpaceTexts.WorldSettings_Realistic);
      this.m_refinery.AddItem(3L, MySpaceTexts.WorldSettings_Realistic_x3);
      this.m_refinery.AddItem(10L, MySpaceTexts.WorldSettings_Realistic_x10);
      this.m_welder.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_half);
      this.m_welder.AddItem(10L, MySpaceTexts.WorldSettings_Realistic);
      this.m_welder.AddItem(20L, MySpaceTexts.WorldSettings_Realistic_x2);
      this.m_welder.AddItem(50L, MySpaceTexts.WorldSettings_Realistic_x5);
      this.m_grinder.AddItem(5L, MySpaceTexts.WorldSettings_Realistic_half);
      this.m_grinder.AddItem(10L, MySpaceTexts.WorldSettings_Realistic);
      this.m_grinder.AddItem(20L, MySpaceTexts.WorldSettings_Realistic_x2);
      this.m_grinder.AddItem(50L, MySpaceTexts.WorldSettings_Realistic_x5);
      this.m_viewDistanceCombo.AddItem(5000L, MySpaceTexts.WorldSettings_ViewDistance_5_Km);
      this.m_viewDistanceCombo.AddItem(7000L, MySpaceTexts.WorldSettings_ViewDistance_7_Km);
      this.m_viewDistanceCombo.AddItem(10000L, MySpaceTexts.WorldSettings_ViewDistance_10_Km);
      this.m_viewDistanceCombo.AddItem(15000L, MySpaceTexts.WorldSettings_ViewDistance_15_Km);
      if (MySandboxGame.Config.ExperimentalMode)
      {
        this.m_viewDistanceCombo.AddItem(20000L, MySpaceTexts.WorldSettings_ViewDistance_20_Km);
        this.m_viewDistanceCombo.AddItem(30000L, MySpaceTexts.WorldSettings_ViewDistance_30_Km);
        this.m_viewDistanceCombo.AddItem(40000L, MySpaceTexts.WorldSettings_ViewDistance_40_Km);
        this.m_viewDistanceCombo.AddItem(50000L, MySpaceTexts.WorldSettings_ViewDistance_50_Km);
      }
      this.m_physicsOptionsCombo.SetToolTip(MyCommonTexts.WorldSettings_Physics_Tooltip);
      this.m_physicsOptionsCombo.AddItem(4L, MyCommonTexts.WorldSettings_Physics_Fast);
      this.m_physicsOptionsCombo.AddItem(8L, MyCommonTexts.WorldSettings_Physics_Normal);
      this.m_physicsOptionsCombo.AddItem(32L, MyCommonTexts.WorldSettings_Physics_Precise);
      this.m_blockLimits = new MyGuiControlCheckbox();
      this.CheckExperimental((MyGuiControlBase) this.m_blockLimits, label6, MyCommonTexts.ToolTipWorldSettingsBlockLimits, false);
      this.m_soundModeCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsSoundMode));
      this.m_autoHealing.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAutoHealing), (object) (int) ((double) MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO * 100.0)));
      this.m_thrusterDamage.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsThrusterDamage));
      this.m_cargoShipsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnableCargoShips));
      this.CheckExperimental((MyGuiControlBase) this.m_enableSpectator, label1, MySpaceTexts.ToolTipWorldSettingsEnableSpectator);
      this.CheckExperimental((MyGuiControlBase) this.m_resetOwnership, label2, MySpaceTexts.ToolTipWorldSettingsResetOwnership);
      this.CheckExperimental((MyGuiControlBase) this.m_permanentDeath, label3, MySpaceTexts.ToolTipWorldSettingsPermanentDeath);
      this.m_destructibleBlocks.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsDestructibleBlocks));
      this.m_environment.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnvironment));
      this.m_onlineMode.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOnlineMode));
      this.m_enableCopyPaste.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnableCopyPaste));
      this.m_showPlayerNamesOnHud.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsShowPlayerNamesOnHud));
      this.m_maxFloatingObjectsSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxFloatingObjects));
      this.m_maxBackupSavesSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxBackupSaves));
      this.m_maxGridSizeSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxGridSize));
      this.m_maxBlocksPerPlayerSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxBlocksPerPlayer));
      this.m_totalPCUSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsTotalPCU));
      this.m_optimalSpawnDistanceSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOptimalSpawnDistance));
      this.m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
      this.m_weaponsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsWeapons));
      this.m_worldSizeCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsLimitWorldSize));
      this.m_viewDistanceCombo.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsViewDistance));
      this.m_respawnShipDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettingsRespawnShipDelete));
      this.m_enableToolShake.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_ToolShake));
      this.CheckExperimental((MyGuiControlBase) this.m_enableAdaptiveSimulationQuality, label5, MySpaceTexts.ToolTipWorldSettings_AdaptiveSimulationQuality, false);
      this.m_enableOxygen.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableOxygen));
      this.m_enableOxygenPressurization.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableOxygenPressurization));
      this.m_enableJetpack.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableJetpack));
      this.m_enableAutoRespawn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsAutorespawn));
      this.CheckExperimental((MyGuiControlBase) this.m_enableSupergridding, label9, MySpaceTexts.ToolTipWorldSettingsSupergridding);
      this.m_enableBountyContracts.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsBountyContracts));
      this.m_enableEconomy.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEconomy));
      this.m_spawnWithTools.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_SpawnWithTools));
      this.m_enableWeatherSystem.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsEnableWeatherSystem));
      this.m_enableEncounters.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableEncounters));
      this.m_enableSunRotation.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableSunRotation));
      this.m_enable3rdPersonCamera.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_Enable3rdPersonCamera));
      this.CheckExperimental((MyGuiControlBase) this.m_enableIngameScripts, label4, MySpaceTexts.ToolTipWorldSettings_EnableIngameScripts);
      this.m_cargoShipsEnabled.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_CargoShipsEnabled));
      this.m_enableWolfs.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableWolfs));
      this.m_enableSpiders.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableSpiders));
      this.m_enableRemoteBlockRemoval.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableRemoteBlockRemoval));
      this.m_enableContainerDrops.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableContainerDrops));
      this.m_enableConvertToStation.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableConvertToStation));
      this.CheckExperimental((MyGuiControlBase) this.m_enableStationVoxelSupport, label7, MySpaceTexts.ToolTipWorldSettings_StationVoxelSupport);
      this.m_enableRespawnShips.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableRespawnShips));
      this.m_enableVoxelDestruction.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableVoxelDestruction));
      this.m_enableTurretsFriendlyFire.SetToolTip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettings_EnableTurrerFriendlyDamage));
      this.CheckExperimental((MyGuiControlBase) this.m_enableSubGridDamage, label8, MySpaceTexts.TooltipWorldSettings_EnableSubGridDamage);
      this.m_enableRealisticDampeners.SetToolTip(MyTexts.GetString(MySpaceTexts.TooltipWorldSettings_EnableRealisticDampeners));
      this.m_enableResearch.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_EnableResearch));
      this.m_charactersInventory.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_InventorySize));
      this.m_blocksInventory.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_BlocksInventorySize));
      this.m_assembler.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_AssemblerEfficiency));
      this.m_refinery.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_RefinerySpeed));
      this.m_welder.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_WeldingSpeed));
      this.m_grinder.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_GrindingSpeed));
      this.m_spawnShipTimeCombo.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_RespawnShipCooldown));
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_charactersInventory);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel17);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_blocksInventory);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel19);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_assembler);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_refinery);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel24);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_welder);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel25);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_grinder);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_environment);
      if (this.m_isNewGame)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel43);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_asteroidAmountCombo);
      }
      if (MyFakes.ENABLE_NEW_SOUNDS)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel42);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_soundModeCombo);
      }
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel23);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_worldSizeCombo);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel27);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_viewDistanceCombo);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel26);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_spawnShipTimeCombo);
      this.m_sunRotationIntervalSlider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), width: (this.m_onlineMode.Size.X * 0.9f), labelSpaceWidth: 0.05f);
      this.m_sunRotationIntervalSlider.MinValue = !MySandboxGame.Config.ExperimentalMode ? this.MIN_SAFE_TIME_FOR_SUN : 0.0f;
      this.m_sunRotationIntervalSlider.MaxValue = 1f;
      this.m_sunRotationIntervalSlider.DefaultValue = new float?(0.0f);
      this.m_sunRotationIntervalSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        this.m_tempBuilder.Clear();
        MyValueFormatter.AppendTimeInBestUnit(MathHelper.Clamp(MathHelper.InterpLog(s.Value, 1f, 1440f), 1f, 1440f) * 60f, this.m_tempBuilder);
        this.m_sunRotationPeriodValue.Text = this.m_tempBuilder.ToString();
      });
      this.m_sunRotationIntervalSlider.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettings_DayDuration));
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel30);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSunRotation);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_sunRotationPeriod);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_sunRotationIntervalSlider);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxFloatingObjectsLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxFloatingObjectsSlider);
      if (!MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX.HasValue)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) label6);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_blockLimits);
      }
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxGridSizeSlider);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxBlocksPerPlayerSlider);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_totalPCUSlider);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxBackupSavesLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxBackupSavesSlider);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_optimalSpawnDistanceSlider);
      if (MyFakes.ENABLE_PHYSICS_SETTINGS)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel28);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_physicsOptionsCombo);
      }
      float x2 = 0.21f;
      Vector2 vector2_2 = new Vector2(0.0f, 0.052f);
      Vector2 vector2_3 = -guiControlParent.Size / 2f + new Vector2(0.0f, (float) ((double) this.m_creativeModeButton.Size.Y / 2.0 + (double) vector2_2.Y / 3.0));
      Vector2 vector2_4 = vector2_3 + new Vector2(x2, 0.0f);
      Vector2 size = this.m_onlineMode.Size;
      foreach (MyGuiControlBase control in guiControlParent.Controls)
      {
        control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        if (control is MyGuiControlLabel)
        {
          control.Position = vector2_3 + vector2_2 * (float) num1;
        }
        else
        {
          control.Position = vector2_4 + vector2_2 * (float) num1++;
          if (num1 == 5 || num1 == 9 || (num1 == 17 || num1 == 19))
          {
            vector2_3.Y += vector2_2.Y / 5f;
            vector2_4.Y += vector2_2.Y / 5f;
          }
        }
      }
      this.m_survivalModeButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_survivalModeButton.Position = this.m_creativeModeButton.Position + new Vector2(this.m_onlineMode.Size.X, 0.0f);
      this.m_maxBackupSavesSlider.ValueChanged += (Action<MyGuiControlSlider>) (s => this.m_maxBackupSavesValue.Text = s.Value.ToString());
      this.m_maxFloatingObjectsSlider.ValueChanged += (Action<MyGuiControlSlider>) (s => this.m_maxFloatingObjectsValue.Text = s.Value.ToString());
      this.m_maxGridSizeSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        if ((double) s.Value >= 100.0)
          this.m_maxGridSizeValue.Text = (s.Value - s.Value % 100f).ToString();
        else
          this.m_maxGridSizeValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
      });
      this.m_maxBlocksPerPlayerSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        if ((double) s.Value >= 100.0)
          this.m_maxBlocksPerPlayerValue.Text = (s.Value - s.Value % 100f).ToString();
        else
          this.m_maxBlocksPerPlayerValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
      });
      this.m_totalPCUSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        if ((double) s.Value >= 100.0)
          this.m_totalPCUValue.Text = (s.Value - s.Value % 100f).ToString();
        else
          this.m_totalPCUValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
      });
      this.m_optimalSpawnDistanceSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        if ((double) s.Value >= 1000.0)
          this.m_optimalSpawnDistanceValue.Text = (s.Value - s.Value % 100f).ToString();
        else
          this.m_optimalSpawnDistanceValue.Text = MyTexts.GetString(MyCommonTexts.Disabled);
      });
      this.m_maxGridSizeValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_maxGridSizeSlider.Position.Y);
      this.m_maxBlocksPerPlayerValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_maxBlocksPerPlayerSlider.Position.Y);
      this.m_totalPCUValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_totalPCUSlider.Position.Y);
      this.m_optimalSpawnDistanceValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_optimalSpawnDistanceSlider.Position.Y);
      this.m_maxFloatingObjectsValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_maxFloatingObjectsSlider.Position.Y);
      this.m_maxBackupSavesValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_maxBackupSavesSlider.Position.Y);
      this.m_maxGridSizeValue.IsAutoScaleEnabled = true;
      this.m_maxBlocksPerPlayerValue.IsAutoScaleEnabled = true;
      this.m_totalPCUValue.IsAutoScaleEnabled = true;
      this.m_optimalSpawnDistanceValue.IsAutoScaleEnabled = true;
      this.m_maxFloatingObjectsValue.IsAutoScaleEnabled = true;
      this.m_maxBackupSavesValue.IsAutoScaleEnabled = true;
      this.m_maxGridSizeValue.IsAutoEllipsisEnabled = true;
      this.m_maxBlocksPerPlayerValue.IsAutoEllipsisEnabled = true;
      this.m_totalPCUValue.IsAutoEllipsisEnabled = true;
      this.m_optimalSpawnDistanceValue.IsAutoEllipsisEnabled = true;
      this.m_maxFloatingObjectsValue.IsAutoEllipsisEnabled = true;
      this.m_maxBackupSavesValue.IsAutoEllipsisEnabled = true;
      this.m_maxGridSizeValue.SetMaxWidth(0.05f);
      this.m_maxBlocksPerPlayerValue.SetMaxWidth(0.05f);
      this.m_totalPCUValue.SetMaxWidth(0.05f);
      this.m_optimalSpawnDistanceValue.SetMaxWidth(0.05f);
      this.m_maxFloatingObjectsValue.SetMaxWidth(0.05f);
      this.m_maxBackupSavesValue.SetMaxWidth(0.05f);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxGridSizeValue);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxBlocksPerPlayerValue);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_totalPCUValue);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_optimalSpawnDistanceValue);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxFloatingObjectsValue);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_maxBackupSavesValue);
      float num4 = 0.055f;
      myGuiControlLabel6.Position = new Vector2(myGuiControlLabel6.Position.X - x2 / 2f, myGuiControlLabel6.Position.Y + num4);
      this.m_autoHealing.Position = new Vector2(this.m_autoHealing.Position.X - x2 / 2f, this.m_autoHealing.Position.Y + num4);
      this.m_sunRotationPeriodValue.Position = new Vector2(this.m_sunRotationIntervalSlider.Position.X + 0.31f, this.m_sunRotationIntervalSlider.Position.Y);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_sunRotationPeriodValue);
      int count = guiControlParent.Controls.Count;
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_autoHealing);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel22);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_respawnShipDelete);
      guiControlParent.Controls.Add((MyGuiControlBase) label1);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSpectator);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableCopyPasteLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableCopyPaste);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel15);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_showPlayerNamesOnHud);
      guiControlParent.Controls.Add((MyGuiControlBase) label2);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_resetOwnership);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_thrusterDamage);
      guiControlParent.Controls.Add((MyGuiControlBase) label3);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_permanentDeath);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_weaponsEnabled);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel13);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_cargoShipsEnabled);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_destructibleBlocks);
      if (MyFakes.ENABLE_PROGRAMMABLE_BLOCK && !MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) label4);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableIngameScripts);
      }
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel11);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableToolShake);
      guiControlParent.Controls.Add((MyGuiControlBase) label5);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableAdaptiveSimulationQuality);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableVoxelHand);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableEncounters);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enable3rdPersonCamera);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableOxygen);
      guiControlParent.Controls.Add((MyGuiControlBase) oxygenPressurizationLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableOxygenPressurization);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel29);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableConvertToStation);
      guiControlParent.Controls.Add((MyGuiControlBase) label7);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableStationVoxelSupport);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel32);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableJetpack);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel33);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_spawnWithTools);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel41);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableVoxelDestruction);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel34);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableDrones);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableWolfsLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableWolfs);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSpidersLabel);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSpiders);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel35);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableRemoteBlockRemoval);
      guiControlParent.Controls.Add((MyGuiControlBase) label8);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSubGridDamage);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel31);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableTurretsFriendlyFire);
      if (MyFakes.ENABLE_PROGRAMMABLE_BLOCK && !MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableContainerDropsLabel);
        guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableContainerDrops);
      }
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel21);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableRespawnShips);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel36);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableResearch);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel37);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableAutoRespawn);
      guiControlParent.Controls.Add((MyGuiControlBase) label9);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableSupergridding);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel39);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableEconomy);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel38);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableBountyContracts);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel40);
      guiControlParent.Controls.Add((MyGuiControlBase) this.m_enableWeatherSystem);
      float num5 = 0.018f;
      Vector2 vector2_5 = new Vector2((float) ((double) x2 + (double) num5 + 0.0500000007450581), 0.0f);
      int num6 = 2;
      double num7 = ((double) vector2_5.X * (double) num6 - 0.0500000007450581) / 2.0;
      vector2_4.X += num5;
      for (int index = count; index < guiControlParent.Controls.Count; ++index)
      {
        MyGuiControlBase control = guiControlParent.Controls[index];
        int num8 = (index - count) % 2;
        int num9 = (index - count) / 2 % num6;
        if (num8 == 0)
        {
          control.Position = vector2_3 + (float) num9 * vector2_5 + vector2_2 * (float) num1;
        }
        else
        {
          control.Position = vector2_4 + (float) num9 * vector2_5 + vector2_2 * (float) num1;
          if (num9 == num6 - 1)
            ++num1;
        }
      }
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(0.0f, 1f);
      float x3 = 25f;
      this.m_okButton = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OkButtonClicked));
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_okButton.Position = vector2_6 + new Vector2(-x3, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.CancelButtonClicked));
      this.m_cancelButton.Position = vector2_6 + new Vector2(x3, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.Controls.Add((MyGuiControlBase) controlScrollablePanel);
      this.CloseButtonEnabled = true;
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel44 = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_okButton.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel44.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel44);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.AdvancedWorldSettings_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_charactersInventory;
    }

    private void CheckExperimental(
      MyGuiControlBase control,
      MyGuiControlLabel label,
      MyStringId toolTip,
      bool enabled = true)
    {
      if (MySandboxGame.Config.ExperimentalMode)
        control.SetToolTip(MyTexts.GetString(toolTip));
      else if (enabled)
      {
        control.SetEnabledByExperimental();
        label.SetEnabledByExperimental();
      }
      else
      {
        control.SetDisabledByExperimental();
        label.SetDisabledByExperimental();
      }
    }

    private MyGuiControlLabel MakeLabel(
      MyStringId textEnum,
      bool is_autoEllipsisEnabled = false,
      float maxSize = float.PositiveInfinity,
      bool is_autoScaleEnabled = false)
    {
      return new MyGuiControlLabel(text: MyTexts.GetString(textEnum), isAutoEllipsisEnabled: is_autoEllipsisEnabled, maxWidth: maxSize, isAutoScaleEnabled: is_autoScaleEnabled);
    }

    private void LoadValues()
    {
      this.m_passwordTextbox.Text = !this.m_isNewGame ? this.m_parent.Checkpoint.Password : "";
      this.SetSettings(this.m_parent.Settings);
    }

    private void CheckButton(float value, params MyGuiControlButton[] allButtons)
    {
      bool flag = false;
      foreach (MyGuiControlButton allButton in allButtons)
      {
        if (allButton.UserData is float)
        {
          if ((double) (float) allButton.UserData == (double) value && !allButton.Checked)
          {
            flag = true;
            allButton.Checked = true;
          }
          else if ((double) (float) allButton.UserData != (double) value && allButton.Checked)
            allButton.Checked = false;
        }
      }
      if (flag)
        return;
      allButtons[0].Checked = true;
    }

    private void CheckButton(MyGuiControlButton active, params MyGuiControlButton[] allButtons)
    {
      foreach (MyGuiControlButton allButton in allButtons)
      {
        if (allButton == active && !allButton.Checked)
          allButton.Checked = true;
        else if (allButton != active && allButton.Checked)
          allButton.Checked = false;
      }
    }

    public void UpdateSurvivalState(bool survivalEnabled)
    {
      this.m_creativeModeButton.Checked = !survivalEnabled;
      this.m_survivalModeButton.Checked = survivalEnabled;
      this.m_enableCopyPaste.Enabled = !survivalEnabled;
      this.m_enableCopyPasteLabel.Enabled = !survivalEnabled;
      this.UpdateContainerDropsState(survivalEnabled);
    }

    private MyGameModeEnum GetGameMode() => !this.m_survivalModeButton.Checked ? MyGameModeEnum.Creative : MyGameModeEnum.Survival;

    private float GetMultiplier(params MyGuiControlButton[] buttons)
    {
      foreach (MyGuiControlButton button in buttons)
      {
        if (button.Checked && button.UserData is float)
          return (float) button.UserData;
      }
      return 1f;
    }

    private float GetInventoryMultiplier() => (float) this.m_charactersInventory.GetSelectedKey();

    private float GetBlocksInventoryMultiplier() => (float) this.m_blocksInventory.GetSelectedKey();

    private float GetRefineryMultiplier() => (float) this.m_refinery.GetSelectedKey();

    private float GetAssemblerMultiplier() => (float) this.m_assembler.GetSelectedKey();

    private float GetWelderMultiplier() => (float) this.m_welder.GetSelectedKey() / 10f;

    private float GetGrinderMultiplier() => (float) this.m_grinder.GetSelectedKey() / 10f;

    private float GetSpawnShipTimeMultiplier() => (float) this.m_spawnShipTimeCombo.GetSelectedKey() / 10f;

    public int GetWorldSize()
    {
      int worldSizeKm = this.m_parent.Settings.WorldSizeKm;
      switch (this.m_worldSizeCombo.GetSelectedKey())
      {
        case 0:
          return 10;
        case 1:
          return 20;
        case 2:
          return 50;
        case 3:
          return 100;
        case 4:
          return 0;
        case 5:
          return this.m_customWorldSize;
        default:
          return 0;
      }
    }

    private MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum WorldSizeEnumKey(
      int worldSize)
    {
      switch (worldSize)
      {
        case 0:
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.UNLIMITED;
        case 10:
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.TEN_KM;
        case 20:
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.TWENTY_KM;
        case 50:
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.FIFTY_KM;
        case 100:
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.HUNDRED_KM;
        default:
          this.m_worldSizeCombo.AddItem(5L, MySpaceTexts.WorldSettings_WorldSizeCustom);
          this.m_customWorldSize = worldSize;
          return MyGuiScreenAdvancedWorldSettings.MyWorldSizeEnum.CUSTOM;
      }
    }

    public int GetViewDistance()
    {
      long selectedKey = this.m_viewDistanceCombo.GetSelectedKey();
      return selectedKey == 0L ? this.m_customViewDistance : (int) selectedKey;
    }

    private MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum ViewDistanceEnumKey(
      int viewDistance)
    {
      MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum viewDistanceEnum = (MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum) viewDistance;
      if (viewDistanceEnum != MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum.CUSTOM && Enum.IsDefined(typeof (MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum), (object) viewDistanceEnum))
        return (MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum) viewDistance;
      this.m_viewDistanceCombo.AddItem(5L, MySpaceTexts.WorldSettings_ViewDistance_Custom);
      this.m_viewDistanceCombo.SelectItemByKey(5L);
      this.m_customViewDistance = viewDistance;
      return MyGuiScreenAdvancedWorldSettings.MyViewDistanceEnum.CUSTOM;
    }

    public void GetSettings(MyObjectBuilder_SessionSettings output)
    {
      output.OnlineMode = (MyOnlineModeEnum) this.m_onlineMode.GetSelectedKey();
      output.EnvironmentHostility = (MyEnvironmentHostilityEnum) this.m_environment.GetSelectedKey();
      switch (this.AsteroidAmount)
      {
        case -5:
          output.ProceduralDensity = 0.1f;
          break;
        case -4:
          output.ProceduralDensity = 0.0f;
          break;
        case -3:
          output.ProceduralDensity = 0.5f;
          break;
        case -2:
          output.ProceduralDensity = 0.35f;
          break;
        case -1:
          output.ProceduralDensity = 0.25f;
          break;
        default:
          throw new InvalidBranchException();
      }
      output.AutoHealing = this.m_autoHealing.IsChecked;
      output.CargoShipsEnabled = this.m_cargoShipsEnabled.IsChecked;
      output.EnableCopyPaste = this.m_enableCopyPaste.IsChecked;
      output.EnableSpectator = this.m_enableSpectator.IsChecked;
      output.ResetOwnership = this.m_resetOwnership.IsChecked;
      output.PermanentDeath = new bool?(this.m_permanentDeath.IsChecked);
      output.DestructibleBlocks = this.m_destructibleBlocks.IsChecked;
      output.EnableIngameScripts = this.m_enableIngameScripts.IsChecked;
      output.Enable3rdPersonView = this.m_enable3rdPersonCamera.IsChecked;
      output.EnableEncounters = this.m_enableEncounters.IsChecked;
      output.EnableToolShake = this.m_enableToolShake.IsChecked;
      output.AdaptiveSimulationQuality = this.m_enableAdaptiveSimulationQuality.IsChecked;
      output.EnableVoxelHand = this.m_enableVoxelHand.IsChecked;
      output.ShowPlayerNamesOnHud = this.m_showPlayerNamesOnHud.IsChecked;
      output.ThrusterDamage = this.m_thrusterDamage.IsChecked;
      output.WeaponsEnabled = this.m_weaponsEnabled.IsChecked;
      output.EnableOxygen = this.m_enableOxygen.IsChecked;
      if (output.EnableOxygen && output.VoxelGeneratorVersion < 1)
        output.VoxelGeneratorVersion = 1;
      output.EnableOxygenPressurization = this.m_enableOxygenPressurization.IsChecked;
      output.RespawnShipDelete = this.m_respawnShipDelete.IsChecked;
      output.EnableConvertToStation = this.m_enableConvertToStation.IsChecked;
      output.StationVoxelSupport = this.m_enableStationVoxelSupport.IsChecked;
      output.EnableAutorespawn = this.m_enableAutoRespawn.IsChecked;
      output.EnableSupergridding = this.m_enableSupergridding.IsChecked;
      output.EnableEconomy = this.m_enableEconomy.IsChecked;
      output.EnableBountyContracts = this.m_enableBountyContracts.IsChecked;
      output.EnableRespawnShips = this.m_enableRespawnShips.IsChecked;
      output.EnableWolfs = this.m_enableWolfs.IsChecked;
      output.EnableSunRotation = this.m_enableSunRotation.IsChecked;
      output.EnableJetpack = this.m_enableJetpack.IsChecked;
      output.SpawnWithTools = this.m_spawnWithTools.IsChecked;
      output.EnableVoxelDestruction = this.m_enableVoxelDestruction.IsChecked;
      output.EnableDrones = this.m_enableDrones.IsChecked;
      output.EnableTurretsFriendlyFire = this.m_enableTurretsFriendlyFire.IsChecked;
      output.EnableSubgridDamage = this.m_enableSubGridDamage.IsChecked;
      output.WeatherSystem = this.m_enableWeatherSystem.IsChecked;
      output.EnableSpiders = this.m_enableSpiders.IsChecked;
      output.EnableRemoteBlockRemoval = this.m_enableRemoteBlockRemoval.IsChecked;
      output.EnableResearch = this.m_enableResearch.IsChecked;
      output.MaxPlayers = (short) this.m_maxPlayersSlider.Value;
      output.MaxFloatingObjects = (short) this.m_maxFloatingObjectsSlider.Value;
      output.MaxBackupSaves = (short) this.m_maxBackupSavesSlider.Value;
      output.MaxGridSize = (int) ((double) this.m_maxGridSizeSlider.Value - (double) this.m_maxGridSizeSlider.Value % 100.0);
      output.MaxBlocksPerPlayer = (int) ((double) this.m_maxBlocksPerPlayerSlider.Value - (double) this.m_maxBlocksPerPlayerSlider.Value % 100.0);
      output.TotalPCU = (int) ((double) this.m_totalPCUSlider.Value - (double) this.m_totalPCUSlider.Value % 100.0);
      output.OptimalSpawnDistance = (float) (int) ((double) this.m_optimalSpawnDistanceSlider.Value - (double) this.m_optimalSpawnDistanceSlider.Value % 100.0);
      output.BlockLimitsEnabled = this.m_blockLimits.IsChecked ? MyBlockLimitsEnabledEnum.GLOBALLY : MyBlockLimitsEnabledEnum.NONE;
      output.SunRotationIntervalMinutes = MathHelper.Clamp(MathHelper.InterpLog(this.m_sunRotationIntervalSlider.Value, 1f, 1440f), 1f, 1440f);
      output.AssemblerEfficiencyMultiplier = this.GetAssemblerMultiplier();
      output.AssemblerSpeedMultiplier = this.GetAssemblerMultiplier();
      output.InventorySizeMultiplier = this.GetInventoryMultiplier();
      output.BlocksInventorySizeMultiplier = this.GetBlocksInventoryMultiplier();
      output.RefinerySpeedMultiplier = this.GetRefineryMultiplier();
      output.WelderSpeedMultiplier = this.GetWelderMultiplier();
      output.GrinderSpeedMultiplier = this.GetGrinderMultiplier();
      output.SpawnShipTimeMultiplier = this.GetSpawnShipTimeMultiplier();
      output.RealisticSound = (int) this.m_soundModeCombo.GetSelectedKey() == 1;
      output.EnvironmentHostility = (MyEnvironmentHostilityEnum) this.m_environment.GetSelectedKey();
      output.WorldSizeKm = this.GetWorldSize();
      output.ViewDistance = this.GetViewDistance();
      output.PhysicsIterations = (int) this.m_physicsOptionsCombo.GetSelectedKey();
      output.GameMode = this.GetGameMode();
      if (output.GameMode == MyGameModeEnum.Creative)
        return;
      output.EnableContainerDrops = this.m_enableContainerDrops.IsChecked;
    }

    public void SetSettings(MyObjectBuilder_SessionSettings settings)
    {
      this.m_onlineMode.SelectItemByKey((long) settings.OnlineMode);
      this.m_environment.SelectItemByKey((long) settings.EnvironmentHostility);
      this.m_worldSizeCombo.SelectItemByKey((long) this.WorldSizeEnumKey(settings.WorldSizeKm));
      this.m_spawnShipTimeCombo.SelectItemByKey((long) (int) ((double) settings.SpawnShipTimeMultiplier * 10.0));
      this.m_viewDistanceCombo.SelectItemByKey((long) this.ViewDistanceEnumKey(settings.ViewDistance));
      this.m_soundModeCombo.SelectItemByKey(settings.RealisticSound ? 1L : 0L);
      this.m_asteroidAmountCombo.SelectItemByKey((long) (int) settings.ProceduralDensity);
      switch ((int) ((double) settings.ProceduralDensity * 100.0))
      {
        case 0:
          this.m_asteroidAmountCombo.SelectItemByKey(-4L);
          break;
        case 10:
          this.m_asteroidAmountCombo.SelectItemByKey(-5L);
          break;
        case 25:
          this.m_asteroidAmountCombo.SelectItemByKey(-1L);
          break;
        case 35:
          this.m_asteroidAmountCombo.SelectItemByKey(-2L);
          break;
        case 50:
          this.m_asteroidAmountCombo.SelectItemByKey(-3L);
          break;
        default:
          this.m_asteroidAmountCombo.SelectItemByKey(-1L);
          break;
      }
      this.m_environment.SelectItemByKey((long) settings.EnvironmentHostility);
      if (this.m_physicsOptionsCombo.TryGetItemByKey((long) settings.PhysicsIterations) != null)
        this.m_physicsOptionsCombo.SelectItemByKey((long) settings.PhysicsIterations);
      else
        this.m_physicsOptionsCombo.SelectItemByKey(4L);
      this.m_autoHealing.IsChecked = settings.AutoHealing;
      this.m_cargoShipsEnabled.IsChecked = settings.CargoShipsEnabled;
      this.m_enableCopyPaste.IsChecked = settings.EnableCopyPaste;
      this.m_enableSpectator.IsChecked = settings.EnableSpectator;
      this.m_resetOwnership.IsChecked = settings.ResetOwnership;
      this.m_permanentDeath.IsChecked = settings.PermanentDeath.Value;
      this.m_destructibleBlocks.IsChecked = settings.DestructibleBlocks;
      this.m_enableEncounters.IsChecked = settings.EnableEncounters;
      this.m_enable3rdPersonCamera.IsChecked = settings.Enable3rdPersonView;
      this.m_enableIngameScripts.IsChecked = settings.EnableIngameScripts;
      this.m_enableToolShake.IsChecked = settings.EnableToolShake;
      this.m_enableAdaptiveSimulationQuality.IsChecked = settings.AdaptiveSimulationQuality;
      this.m_enableVoxelHand.IsChecked = settings.EnableVoxelHand;
      this.m_showPlayerNamesOnHud.IsChecked = settings.ShowPlayerNamesOnHud;
      this.m_thrusterDamage.IsChecked = settings.ThrusterDamage;
      this.m_weaponsEnabled.IsChecked = settings.WeaponsEnabled;
      this.m_enableOxygen.IsChecked = settings.EnableOxygen;
      if (settings.VoxelGeneratorVersion < 1)
        this.m_showWarningForOxygen = true;
      this.m_enableOxygenPressurization.IsChecked = settings.EnableOxygenPressurization;
      this.m_enableRespawnShips.IsChecked = settings.EnableRespawnShips;
      this.m_respawnShipDelete.IsChecked = settings.RespawnShipDelete;
      this.m_enableConvertToStation.IsChecked = settings.EnableConvertToStation;
      this.m_enableStationVoxelSupport.IsChecked = settings.StationVoxelSupport;
      this.m_enableSunRotation.IsChecked = settings.EnableSunRotation;
      this.m_enableJetpack.IsChecked = settings.EnableJetpack;
      this.m_spawnWithTools.IsChecked = settings.SpawnWithTools;
      this.m_enableAutoRespawn.IsChecked = settings.EnableAutorespawn;
      this.m_enableSupergridding.IsChecked = settings.EnableSupergridding;
      this.m_enableEconomy.IsChecked = settings.EnableEconomy;
      this.m_enableWeatherSystem.IsChecked = settings.WeatherSystem;
      this.m_enableBountyContracts.IsChecked = settings.EnableBountyContracts;
      this.m_sunRotationIntervalSlider.Enabled = this.m_enableSunRotation.IsChecked;
      this.m_sunRotationPeriodValue.Visible = this.m_enableSunRotation.IsChecked;
      int? nullable = settings.OnlineMode == MyOnlineModeEnum.OFFLINE ? MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX : MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX;
      this.m_sunRotationIntervalSlider.Value = 0.03f;
      this.m_sunRotationIntervalSlider.Value = MathHelper.Clamp(MathHelper.InterpLogInv(settings.SunRotationIntervalMinutes, 1f, 1440f), 0.0f, 1f);
      this.m_maxPlayersSlider.Value = (float) settings.MaxPlayers;
      this.m_maxFloatingObjectsSlider.Value = (float) settings.MaxFloatingObjects;
      this.m_blockLimits.IsChecked = nullable.HasValue || (uint) settings.BlockLimitsEnabled > 0U;
      this.m_blockLimits.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.blockLimits_CheckedChanged);
      this.blockLimits_CheckedChanged(this.m_blockLimits);
      this.m_maxBlocksPerPlayerSlider.Value = (float) settings.MaxBlocksPerPlayer;
      this.m_maxGridSizeSlider.Value = (float) settings.MaxGridSize;
      this.m_totalPCUSlider.MinValue = nullable.HasValue ? 100f : 0.0f;
      this.m_totalPCUSlider.MaxValue = (float) (nullable ?? 1000000);
      this.m_totalPCUSlider.Value = (float) settings.TotalPCU;
      this.m_optimalSpawnDistanceSlider.DefaultValue = new float?(this.m_optimalSpawnDistanceSlider.Value = settings.OptimalSpawnDistance);
      this.m_onlineMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnOnlineModeItemSelected);
      this.OnOnlineModeItemSelected();
      this.m_maxBackupSavesSlider.Value = (float) settings.MaxBackupSaves;
      this.m_enableSubGridDamage.IsChecked = settings.EnableSubgridDamage;
      this.m_enableTurretsFriendlyFire.IsChecked = settings.EnableTurretsFriendlyFire;
      this.m_enableVoxelDestruction.IsChecked = settings.EnableVoxelDestruction;
      this.m_enableDrones.IsChecked = settings.EnableDrones;
      this.m_enableWolfs.IsChecked = settings.EnableWolfs;
      this.m_enableSpiders.IsChecked = settings.EnableSpiders;
      this.m_enableRemoteBlockRemoval.IsChecked = settings.EnableRemoteBlockRemoval;
      this.m_enableResearch.IsChecked = settings.EnableResearch;
      this.m_enableContainerDrops.IsChecked = settings.GameMode != MyGameModeEnum.Creative && settings.EnableContainerDrops;
      this.m_assembler.SelectItemByKey((long) (int) settings.AssemblerEfficiencyMultiplier);
      this.m_charactersInventory.SelectItemByKey((long) (int) settings.InventorySizeMultiplier);
      this.m_blocksInventory.SelectItemByKey((long) (int) settings.BlocksInventorySizeMultiplier);
      this.m_refinery.SelectItemByKey((long) (int) settings.RefinerySpeedMultiplier);
      this.m_welder.SelectItemByKey((long) (int) ((double) settings.WelderSpeedMultiplier * 10.0));
      this.m_grinder.SelectItemByKey((long) (int) ((double) settings.GrinderSpeedMultiplier * 10.0));
      this.UpdateSurvivalState(settings.GameMode == MyGameModeEnum.Survival);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenAdvancedWorldSettings);

    private void UpdateContainerDropsState(bool survivalEnabled)
    {
      this.m_enableContainerDrops.Enabled = survivalEnabled;
      this.m_enableContainerDropsLabel.Enabled = survivalEnabled;
    }

    private void CancelButtonClicked(object sender) => this.CloseScreen();

    private void OkButtonClicked(object sender)
    {
      this.m_isConfirmed = true;
      if (this.OnOkButtonClicked != null)
        this.OnOkButtonClicked();
      this.CloseScreen();
    }

    private void CreativeClicked(object sender)
    {
      this.UpdateSurvivalState(false);
      this.m_enableContainerDrops.IsChecked = false;
    }

    private void SurvivalClicked(object sender)
    {
      this.UpdateSurvivalState(true);
      this.m_enableContainerDrops.IsChecked = this.m_survivalModeButton.Checked;
    }

    private void m_soundModeCombo_ItemSelected()
    {
      if (this.m_soundModeCombo.GetSelectedIndex() != 1)
        return;
      this.m_parent.Settings.EnableOxygenPressurization = true;
    }

    private void m_asteroidAmountCombo_ItemSelected() => this.m_asteroidAmount = new int?((int) this.m_asteroidAmountCombo.GetSelectedKey());

    private void HostilityChanged() => this.m_isHostilityChanged = true;

    private void blockLimits_CheckedChanged(MyGuiControlCheckbox checkbox)
    {
      if (!checkbox.IsChecked)
      {
        if (!this.m_recreating_control)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextBlockLimitDisableWarning), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
        this.m_maxGridSizeSlider.Value = 0.0f;
        this.m_maxGridSizeSlider.Enabled = false;
        this.m_maxBlocksPerPlayerSlider.Value = 0.0f;
        this.m_maxBlocksPerPlayerSlider.Enabled = false;
        this.m_totalPCUSlider.Value = 0.0f;
        this.m_totalPCUSlider.Enabled = false;
      }
      else
      {
        this.m_maxBlocksPerPlayerSlider.Value = 100000f;
        this.m_maxBlocksPerPlayerSlider.Enabled = true;
        this.m_maxGridSizeSlider.Value = 50000f;
        this.m_maxGridSizeSlider.Enabled = true;
        this.m_totalPCUSlider.Value = 100000f;
        this.m_totalPCUSlider.Enabled = true;
      }
    }

    private void OnOnlineModeItemSelected()
    {
      if ((int) this.m_onlineMode.GetSelectedKey() == 0)
      {
        this.m_optimalSpawnDistanceSlider.Value = this.m_optimalSpawnDistanceSlider.MinValue;
        this.m_optimalSpawnDistanceSlider.Enabled = false;
      }
      else
      {
        this.m_optimalSpawnDistanceSlider.Enabled = true;
        this.m_optimalSpawnDistanceSlider.Value = this.m_optimalSpawnDistanceSlider.DefaultValue.GetValueOrDefault(4000f);
      }
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OkButtonClicked((object) null);
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private enum AsteroidAmountEnum
    {
      ProceduralLowest = -5, // 0xFFFFFFFB
      ProceduralNone = -4, // 0xFFFFFFFC
      ProceduralHigh = -3, // 0xFFFFFFFD
      ProceduralNormal = -2, // 0xFFFFFFFE
      ProceduralLow = -1, // 0xFFFFFFFF
      None = 0,
      Normal = 4,
      More = 7,
      Many = 16, // 0x00000010
    }

    private enum MyFloraDensityEnum
    {
      NONE = 0,
      LOW = 10, // 0x0000000A
      MEDIUM = 20, // 0x00000014
      HIGH = 30, // 0x0000001E
      EXTREME = 40, // 0x00000028
    }

    private enum MySoundModeEnum
    {
      Arcade,
      Realistic,
    }

    private enum MyWorldSizeEnum
    {
      TEN_KM,
      TWENTY_KM,
      FIFTY_KM,
      HUNDRED_KM,
      UNLIMITED,
      CUSTOM,
    }

    private enum MyViewDistanceEnum
    {
      CUSTOM = 0,
      FIVE_KM = 5000, // 0x00001388
      SEVEN_KM = 7000, // 0x00001B58
      TEN_KM = 10000, // 0x00002710
      FIFTEEN_KM = 15000, // 0x00003A98
      TWENTY_KM = 20000, // 0x00004E20
      THIRTY_KM = 30000, // 0x00007530
      FORTY_KM = 40000, // 0x00009C40
      FIFTY_KM = 50000, // 0x0000C350
    }
  }
}
