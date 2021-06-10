// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenOptionsGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenOptionsGame : MyGuiScreenBase
  {
    private MyGuiControlCombobox m_languageCombobox;
    private MyGuiControlCombobox m_buildingModeCombobox;
    private MyGuiControlCheckbox m_experimentalCheckbox;
    private MyGuiControlCheckbox m_controlHintsCheckbox;
    private MyGuiControlCheckbox m_goodbotHintsCheckbox;
    private MyGuiControlCombobox m_ironSightCombobox;
    private MyGuiControlCheckbox m_enableNewNewGameScreen;
    private MyGuiControlButton m_goodBotResetButton;
    private MyGuiControlButton m_tab1GeneralButton;
    private MyGuiControlButton m_tab2CrosshairButton;
    private MyGuiControlCheckbox m_rotationHintsCheckbox;
    private MyGuiControlCombobox m_crosshairCombobox;
    private MyGuiControlCheckbox m_cloudCheckbox;
    private MyGuiControlCheckbox m_GDPRConsentCheckbox;
    private MyGuiControlCheckbox m_enableTradingCheckbox;
    private MyGuiControlCheckbox m_areaInteractionCheckbox;
    private MyGuiControlSlider m_spriteMainViewportScaleSlider;
    private MyGuiControlSlider m_UIOpacitySlider;
    private MyGuiControlSlider m_UIBkOpacitySlider;
    private MyGuiControlSlider m_HUDBkOpacitySlider;
    private MyGuiControlPanel m_colorPreview;
    private MyGuiScreenOptionsGame.ColorPickerControlWrap m_colorPicker;
    private MyGuiControlButton m_localizationWebButton;
    private MyGuiControlLabel m_localizationWarningLabel;
    private MyGuiScreenOptionsGame.OptionsGameSettings m_settings = new MyGuiScreenOptionsGame.OptionsGameSettings()
    {
      SpriteMainViewportScale = 1f,
      UIOpacity = 1f,
      UIBkOpacity = 1f,
      HUDBkOpacity = 0.6f
    };
    private MyGuiControlElementGroup m_elementGroup;
    private MyGuiControlCombobox m_hitCombobox;
    private MyGuiControlButton m_crosshairLookNextButton;
    private MyGuiControlButton m_crosshairLookPrevButton;
    private MyGuiControlButton m_buttonOk;
    private MyGuiControlButton m_buttonCancel;
    private MyGuiControlImage m_crosshairLookImage;
    private MyGuiControlImage m_crosshairLookPrevImage;
    private MyGuiControlImage m_crosshairLookPrev2Image;
    private MyGuiControlImage m_crosshairLookNextImage;
    private MyGuiControlImage m_crosshairLookNext2Image;
    private List<string> m_crosshairFiles = new List<string>();
    private bool m_isIniting;
    private bool m_ignoreUpdateForFrame;
    private bool m_languangeChanged;
    private bool m_isLimitedMenu;
    private MyLanguagesEnum m_loadedLanguage;
    private MyGuiScreenOptionsGame.PageEnum m_currentPage;

    public MyGuiScreenOptionsGame(bool isLimitedMenu = false)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(isLimitedMenu ? new Vector2(0.6535714f, 0.8587787f) : new Vector2(0.6535714f, 0.9379771f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_isIniting = true;
      this.SaveOriginalSettings();
      this.m_isLimitedMenu = isLimitedMenu;
      this.EnabledBackgroundFade = true;
      this.InitCrosshairIndicators();
      this.RecreateControls(true);
      this.m_isIniting = false;
    }

    private void InitCrosshairIndicators()
    {
      Path.Combine(MyFileSystem.ContentPath);
      foreach (string enumerateFile in Directory.EnumerateFiles(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Indicators")))
      {
        if (enumerateFile.Contains("HitIndicator"))
          this.m_crosshairFiles.Add(enumerateFile);
      }
    }

    private void OnResetGoodbotPressed(MyGuiControlButton obj)
    {
      if (MySession.Static != null)
      {
        MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
      }
      else
      {
        MySandboxGame.Config.TutorialsFinished.Clear();
        MySandboxGame.Config.Save();
      }
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaption_GoodBotHintsReset);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxText_GoodBotHintReset)), messageCaption: messageCaption));
    }

    private void checkboxChanged(MyGuiControlCheckbox obj)
    {
      if (obj == this.m_experimentalCheckbox)
      {
        if (this.m_isIniting)
          return;
        if (obj.IsChecked)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmExperimental)), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
          {
            if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
            {
              this.m_settings.ExperimentalMode = obj.IsChecked;
            }
            else
            {
              this.m_settings.ExperimentalMode = false;
              obj.IsChecked = false;
            }
          })), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
        }
        else
          this.m_settings.ExperimentalMode = false;
      }
      else if (obj == this.m_controlHintsCheckbox)
        this.m_settings.ControlHints = obj.IsChecked;
      else if (obj == this.m_rotationHintsCheckbox)
        this.m_settings.RotationHints = obj.IsChecked;
      else if (obj == this.m_enableTradingCheckbox)
        this.m_settings.EnableTrading = obj.IsChecked;
      else if (obj == this.m_areaInteractionCheckbox)
        this.m_settings.AreaInteraction = obj.IsChecked;
      else if (obj == this.m_cloudCheckbox)
        this.m_settings.EnableSteamCloud = obj.IsChecked;
      else if (obj == this.m_goodbotHintsCheckbox)
        this.m_settings.GoodBotHints = this.m_goodBotResetButton.Enabled = obj.IsChecked;
      else if (obj == this.m_enableNewNewGameScreen)
      {
        this.m_settings.EnableNewNewGameScreen = obj.IsChecked;
      }
      else
      {
        if (obj != this.m_GDPRConsentCheckbox)
          return;
        this.m_settings.GDPR = new bool?(obj.IsChecked);
      }
    }

    private void sliderChanged(MyGuiControlSlider obj)
    {
      if (obj == this.m_UIOpacitySlider)
      {
        this.m_settings.UIOpacity = obj.Value;
        this.m_guiTransition = obj.Value;
      }
      else if (obj == this.m_UIBkOpacitySlider)
      {
        this.m_settings.UIBkOpacity = obj.Value;
        this.m_backgroundTransition = obj.Value;
      }
      else if (obj == this.m_HUDBkOpacitySlider)
      {
        this.m_settings.HUDBkOpacity = obj.Value;
      }
      else
      {
        if (obj != this.m_spriteMainViewportScaleSlider)
          return;
        this.m_settings.SpriteMainViewportScale = obj.Value;
      }
    }

    private void m_buildingModeCombobox_ItemSelected() => this.m_settings.BuildingMode = (MyCubeBuilder.BuildingModeEnum) this.m_buildingModeCombobox.GetSelectedKey();

    private void LocalizationWebButtonClicked(MyGuiControlButton obj) => MyGuiSandbox.OpenUrl(MyPerGameSettings.LocalizationWebUrl, UrlOpenMode.SteamOrExternalWithConfirm);

    private void m_languageCombobox_ItemSelected()
    {
      this.m_settings.Language = (MyLanguagesEnum) this.m_languageCombobox.GetSelectedKey();
      if (this.m_localizationWarningLabel == null)
        return;
      if (MyTexts.Languages[this.m_settings.Language].IsCommunityLocalized)
        this.m_localizationWarningLabel.ColorMask = Color.Red.ToVector4();
      else
        this.m_localizationWarningLabel.ColorMask = Color.White.ToVector4();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenOptionsGame);

    private void UpdateControls(bool constructor)
    {
      switch (this.CurrentPage)
      {
        case MyGuiScreenOptionsGame.PageEnum.General:
          this.Tab1UpdateControl(constructor);
          break;
        case MyGuiScreenOptionsGame.PageEnum.Crosshair:
          this.Tab2UpdateControls(constructor);
          break;
      }
    }

    public void Tab1UpdateControl(bool constructor)
    {
      this.m_languageCombobox.SelectItemByKey((long) this.m_settings.Language);
      if (constructor)
        this.m_loadedLanguage = (MyLanguagesEnum) this.m_languageCombobox.GetSelectedKey();
      this.m_buildingModeCombobox.SelectItemByKey((long) this.m_settings.BuildingMode);
      this.m_controlHintsCheckbox.IsChecked = this.m_settings.ControlHints;
      this.m_goodbotHintsCheckbox.IsChecked = this.m_settings.GoodBotHints;
      if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
        this.m_enableNewNewGameScreen.IsChecked = this.m_settings.EnableNewNewGameScreen;
      this.m_goodBotResetButton.Enabled = this.m_goodbotHintsCheckbox.IsChecked;
      if (this.m_experimentalCheckbox != null)
        this.m_experimentalCheckbox.IsChecked = this.m_settings.ExperimentalMode;
      if (this.m_rotationHintsCheckbox != null)
        this.m_rotationHintsCheckbox.IsChecked = this.m_settings.RotationHints;
      this.m_enableTradingCheckbox.IsChecked = this.m_settings.EnableTrading;
      if (this.m_cloudCheckbox != null)
        this.m_cloudCheckbox.IsChecked = this.m_settings.EnableSteamCloud;
      if (this.m_areaInteractionCheckbox == null)
        return;
      this.m_areaInteractionCheckbox.IsChecked = this.m_settings.AreaInteraction;
    }

    private void SaveOriginalSettings()
    {
      this.m_settings.Language = MySandboxGame.Config.Language;
      this.m_settings.BuildingMode = MyCubeBuilder.BuildingMode;
      this.m_settings.ControlHints = MySandboxGame.Config.ControlsHints;
      this.m_settings.EnableNewNewGameScreen = MySandboxGame.Config.EnableNewNewGameScreen;
      this.m_settings.GoodBotHints = MySandboxGame.Config.GoodBotHints;
      this.m_settings.ExperimentalMode = MySandboxGame.Config.ExperimentalMode;
      this.m_settings.RotationHints = MySandboxGame.Config.RotationHints;
      this.m_settings.EnableTrading = MySandboxGame.Config.EnableTrading;
      this.m_settings.EnableSteamCloud = MySandboxGame.Config.EnableSteamCloud;
      this.m_settings.SpriteMainViewportScale = MySandboxGame.Config.SpriteMainViewportScale * 100f;
      this.m_settings.UIOpacity = MySandboxGame.Config.UIOpacity;
      this.m_settings.UIBkOpacity = MySandboxGame.Config.UIBkOpacity;
      this.m_settings.HUDBkOpacity = MySandboxGame.Config.HUDBkOpacity;
      this.m_settings.GDPR = MySandboxGame.Config.GDPRConsent;
      this.m_settings.AreaInteraction = MySandboxGame.Config.AreaInteraction;
      this.m_settings.ShowCrosshair = MySandboxGame.Config.ShowCrosshair;
      this.m_settings.IronSight = MySandboxGame.Config.IronSightSwitchState;
      this.m_settings.HitIndicatorSettings = new Dictionary<MySession.MyHitIndicatorTarget, MyGuiScreenOptionsGame.HitIndicatorSettings>();
      string path1 = Path.Combine(MyFileSystem.ContentPath);
      this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Character] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = Path.Combine(path1, MySandboxGame.Config.HitIndicatorTextureCharacter),
        Color = MySandboxGame.Config.HitIndicatorColorCharacter
      };
      this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Friend] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = Path.Combine(path1, MySandboxGame.Config.HitIndicatorTextureFriend),
        Color = MySandboxGame.Config.HitIndicatorColorFriend
      };
      this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Grid] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = Path.Combine(path1, MySandboxGame.Config.HitIndicatorTextureGrid),
        Color = MySandboxGame.Config.HitIndicatorColorGrid
      };
      this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Headshot] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = Path.Combine(path1, MySandboxGame.Config.HitIndicatorTextureHeadshot),
        Color = MySandboxGame.Config.HitIndicatorColorHeadshot
      };
      this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Kill] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = Path.Combine(path1, MySandboxGame.Config.HitIndicatorTextureKill),
        Color = MySandboxGame.Config.HitIndicatorColorKill
      };
    }

    private void DoChanges()
    {
      this.Tab1DoChanges();
      this.Tab2DoChanges();
      MySandboxGame.Config.Save();
    }

    public void Tab1DoChanges()
    {
      MySandboxGame.Config.ExperimentalMode = this.m_settings.ExperimentalMode;
      bool? gdpr = this.m_settings.GDPR;
      bool? gdprConsent = MySandboxGame.Config.GDPRConsent;
      if (!(gdpr.GetValueOrDefault() == gdprConsent.GetValueOrDefault() & gdpr.HasValue == gdprConsent.HasValue))
      {
        MySandboxGame.Config.GDPRConsent = this.m_settings.GDPR;
        ConsentSenderGDPR.TrySendConsent();
      }
      MySandboxGame.Config.EnableTrading = this.m_settings.EnableTrading;
      MySandboxGame.Config.EnableSteamCloud = this.m_settings.EnableSteamCloud;
      MyLanguage.CurrentLanguage = this.m_settings.Language;
      if (this.m_loadedLanguage != MyLanguage.CurrentLanguage)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.MessageBoxTextRestartNeededAfterLanguageSwitch), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
        MyScreenManager.RecreateControls();
      }
      MyCubeBuilder.BuildingMode = this.m_settings.BuildingMode;
      MySandboxGame.Config.ControlsHints = this.m_settings.ControlHints;
      if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
        MySandboxGame.Config.EnableNewNewGameScreen = this.m_settings.EnableNewNewGameScreen;
      MySandboxGame.Config.GoodBotHints = this.m_settings.GoodBotHints;
      MySandboxGame.Config.RotationHints = this.m_settings.RotationHints;
      if (MyGuiScreenHudSpace.Static != null)
        MyGuiScreenHudSpace.Static.RegisterAlphaMultiplier(VisualStyleCategory.Background, this.m_settings.HUDBkOpacity);
      MySandboxGame.Config.AreaInteraction = this.m_settings.AreaInteraction;
    }

    public void OnCancelClick(MyGuiControlButton sender) => this.CloseScreen();

    public void OnOkClick(MyGuiControlButton sender)
    {
      this.DoChanges();
      this.CloseScreen();
    }

    private void OnWorkshopConsentClick(MyGuiControlButton sender)
    {
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(new Action(this.SetIgnoreUpdateForOneFrame), new Action(this.SetIgnoreUpdateForOneFrame));
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
    }

    private void SetIgnoreUpdateForOneFrame() => this.m_ignoreUpdateForFrame = true;

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (this.m_ignoreUpdateForFrame)
      {
        this.m_ignoreUpdateForFrame = false;
        return num != 0;
      }
      if (!hasFocus)
        return num != 0;
      if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y).IsNewPressed())
        this.OnResetGoodbotPressed((MyGuiControlButton) null);
      if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X).IsNewPressed())
        this.OnOkClick((MyGuiControlButton) null);
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_goodBotResetButton != null)
        this.m_goodBotResetButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_colorPicker == null)
        return num != 0;
      this.m_colorPicker.HexVisible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private MyGuiScreenOptionsGame.PageEnum CurrentPage
    {
      get => this.m_currentPage;
      set
      {
        if (this.m_currentPage == value)
          return;
        this.m_currentPage = value;
        this.RecreateControls(false);
      }
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_isIniting = true;
      this.CreateTabs();
      switch (this.CurrentPage)
      {
        case MyGuiScreenOptionsGame.PageEnum.General:
          this.CreateTab1Menu(constructor);
          break;
        case MyGuiScreenOptionsGame.PageEnum.Crosshair:
          this.CreateTab2Menu(constructor);
          break;
      }
      this.UpdateControls(constructor);
      this.m_isIniting = false;
    }

    private void SelectTab(MyGuiScreenOptionsGame.PageEnum tab)
    {
      this.CurrentPage = tab;
      if (tab != MyGuiScreenOptionsGame.PageEnum.General)
      {
        if (tab != MyGuiScreenOptionsGame.PageEnum.Crosshair)
          return;
        this.m_tab1GeneralButton.Selected = false;
        this.m_tab1GeneralButton.Checked = false;
        this.m_tab2CrosshairButton.Selected = true;
        this.m_tab2CrosshairButton.Checked = true;
        this.FocusedControl = (MyGuiControlBase) this.m_tab2CrosshairButton;
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.GameOptions_Help_Screen_TabCrosshair);
      }
      else
      {
        this.m_tab1GeneralButton.Selected = true;
        this.m_tab1GeneralButton.Checked = true;
        this.m_tab2CrosshairButton.Selected = false;
        this.m_tab2CrosshairButton.Checked = false;
        this.FocusedControl = (MyGuiControlBase) this.m_tab1GeneralButton;
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.GameOptions_Help_Screen_TabGeneral);
      }
    }

    public override bool Draw()
    {
      base.Draw();
      if (MyInput.Static.IsJoystickLastUsed)
      {
        MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        Vector2 positionAbsoluteTopLeft = this.m_tab1GeneralButton.GetPositionAbsoluteTopLeft();
        Vector2 size = this.m_tab1GeneralButton.Size;
        Vector2 normalizedCoord1 = positionAbsoluteTopLeft;
        normalizedCoord1.Y += size.Y / 2f;
        normalizedCoord1.X -= size.X / 6f;
        Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
        normalizedCoord2.Y = normalizedCoord1.Y;
        int num = 2;
        Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, true, this.m_transitionAlpha);
        normalizedCoord2.X += (float) ((double) num * (double) size.X + (double) size.X / 6.0);
        MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord1, 1f, new Color?(color), drawAlign);
        MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, new Color?(color), drawAlign);
      }
      return true;
    }

    private void CreateTabs()
    {
      this.m_elementGroup = new MyGuiControlElementGroup();
      Vector2 vector2_1 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float num1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x = 25f;
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      float num2 = 0.0015f;
      Vector2 vector2_3 = new Vector2(0.0f, 0.042f);
      Vector2 vector2_4 = new Vector2(0.0f, 0.008f);
      Vector2 vector2_5 = (this.m_size.Value / 2f - vector2_1) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_2) * new Vector2(0.0f, 1f);
      Vector2 vector2_8 = new Vector2(vector2_6.X - (num1 + num2), vector2_6.Y);
      Vector2 vector2_9 = (vector2_5 + vector2_6) / 2f;
      float num3 = 0.02f;
      this.AddCaption(MyCommonTexts.ScreenCaptionGameOptions, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.150000005960464)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
      Vector2 vector2_10 = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0 - 3.0 / 1000.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0949999988079071));
      this.m_tab1GeneralButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MySpaceTexts.ScreenOptionsGame_GeneralTab), onButtonClick: new Action<MyGuiControlButton>(this.OnTabSwitchClick));
      this.m_tab1GeneralButton.Position = vector2_10 + this.m_tab1GeneralButton.Size / 2f * Vector2.UnitX;
      this.m_tab1GeneralButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_tab1GeneralButton.UserData = (object) MyGuiScreenOptionsGame.PageEnum.General;
      this.m_tab1GeneralButton.Selected = this.CurrentPage == MyGuiScreenOptionsGame.PageEnum.General;
      vector2_10.X += this.m_tab1GeneralButton.Size.X + num3 / 3.6f;
      this.m_tab2CrosshairButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MySpaceTexts.ScreenOptionsGame_UITab), onButtonClick: new Action<MyGuiControlButton>(this.OnTabSwitchClick));
      this.m_tab2CrosshairButton.Position = vector2_10 + this.m_tab2CrosshairButton.Size / 2f * Vector2.UnitX;
      this.m_tab2CrosshairButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_tab2CrosshairButton.UserData = (object) MyGuiScreenOptionsGame.PageEnum.Crosshair;
      this.m_tab2CrosshairButton.Selected = this.CurrentPage == MyGuiScreenOptionsGame.PageEnum.Crosshair;
      this.m_buttonOk = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_buttonOk.Position = vector2_7 + new Vector2(-x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_buttonCancel.Position = vector2_7 + new Vector2(x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_tab1GeneralButton);
      this.Controls.Add((MyGuiControlBase) this.m_tab2CrosshairButton);
      this.Controls.Add((MyGuiControlBase) this.m_buttonOk);
      this.Controls.Add((MyGuiControlBase) this.m_buttonCancel);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(vector2_5.X, this.m_buttonOk.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.GameOptions_Help_Screen_TabGeneral);
    }

    private void CreateTab1Menu(bool constructor)
    {
      MyGuiDrawAlignEnum guiDrawAlignEnum1 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiDrawAlignEnum guiDrawAlignEnum2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      Vector2 vector2_1 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float num1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      float num2 = 0.0015f;
      Vector2 vector2_3 = new Vector2(0.0f, 0.042f);
      float num3 = 0.0f;
      Vector2 vector2_4 = new Vector2(0.0f, 0.008f);
      Vector2 vector2_5 = (this.m_size.Value / 2f - vector2_1) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_2) * new Vector2(0.0f, 1f);
      Vector2 vector2_8 = new Vector2(vector2_6.X - (num1 + num2), vector2_6.Y);
      float num4 = num3 + 2f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.Language));
      myGuiControlLabel1.Position = vector2_5 + num4 * vector2_3 + vector2_4;
      myGuiControlLabel1.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.Position = vector2_6 + num4 * vector2_3;
      guiControlCombobox1.OriginAlign = guiDrawAlignEnum2;
      this.m_languageCombobox = guiControlCombobox1;
      this.m_languageCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_Language));
      foreach (MyLanguagesEnum supportedLanguage in MyLanguage.SupportedLanguages)
      {
        MyTexts.MyLanguageDescription language = MyTexts.Languages[supportedLanguage];
        string name = language.Name;
        if (!language.IsCommunityLocalized || MyPlatformGameSettings.SUPPORT_COMMUNITY_TRANSLATIONS)
        {
          if (language.IsCommunityLocalized)
            name += " *";
          this.m_languageCombobox.AddItem((long) supportedLanguage, name);
        }
      }
      this.m_languageCombobox.CustomSortItems((Comparison<MyGuiControlCombobox.Item>) ((a, b) => a.Key.CompareTo(b.Key)));
      this.m_languageCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_languageCombobox_ItemSelected);
      float num5 = num4 + 1f;
      if (!this.m_isLimitedMenu)
      {
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_LocalizationWarning), textScale: 0.578f);
        myGuiControlLabel3.Position = vector2_6 + num5 * vector2_3 - new Vector2(num1 - 0.005f, 0.0f);
        myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
        this.m_localizationWarningLabel = myGuiControlLabel3;
        Vector2? position = new Vector2?(vector2_6 + num5 * vector2_3 - new Vector2(num1 - 0.008f - this.m_localizationWarningLabel.Size.X, 0.0f));
        Vector2? size = new Vector2?();
        Vector4? colorMask = new Vector4?();
        StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.ScreenOptionsGame_MoreInfo);
        Action<MyGuiControlButton> action = new Action<MyGuiControlButton>(this.LocalizationWebButtonClicked);
        int num6 = (int) guiDrawAlignEnum1;
        StringBuilder text = stringBuilder;
        Action<MyGuiControlButton> onButtonClick = action;
        int? buttonIndex = new int?();
        this.m_localizationWebButton = new MyGuiControlButton(position, size: size, colorMask: colorMask, originAlign: ((MyGuiDrawAlignEnum) num6), text: text, textScale: 0.6f, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
        this.m_localizationWebButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
        num5 += 0.83f;
      }
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_BuildingMode));
      myGuiControlLabel4.Position = vector2_5 + num5 * vector2_3 + vector2_4;
      myGuiControlLabel4.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel5 = myGuiControlLabel4;
      MyGuiControlCombobox guiControlCombobox2 = new MyGuiControlCombobox();
      guiControlCombobox2.Position = vector2_6 + num5 * vector2_3;
      guiControlCombobox2.OriginAlign = guiDrawAlignEnum2;
      this.m_buildingModeCombobox = guiControlCombobox2;
      this.m_buildingModeCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_BuildingMode));
      this.m_buildingModeCombobox.AddItem(0L, MyCommonTexts.ScreenOptionsGame_SingleBlock);
      this.m_buildingModeCombobox.AddItem(1L, MyCommonTexts.ScreenOptionsGame_Line);
      this.m_buildingModeCombobox.AddItem(2L, MyCommonTexts.ScreenOptionsGame_Plane);
      this.m_buildingModeCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_buildingModeCombobox_ItemSelected);
      float num7 = num5 + 1.26f;
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ExperimentalMode));
      myGuiControlLabel6.Position = vector2_5 + num7 * vector2_3 + vector2_4;
      myGuiControlLabel6.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel7 = myGuiControlLabel6;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsExperimentalMode));
      guiControlCheckbox1.Position = vector2_8 + num7 * vector2_3;
      guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
      this.m_experimentalCheckbox = guiControlCheckbox1;
      this.m_experimentalCheckbox.Enabled = myGuiControlLabel7.Enabled = MyGuiScreenGamePlay.Static == null;
      float num8 = num7 + 1f;
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ShowControlsHints));
      myGuiControlLabel8.Position = vector2_5 + num8 * vector2_3 + vector2_4;
      myGuiControlLabel8.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel9 = myGuiControlLabel8;
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowControlsHints));
      guiControlCheckbox2.Position = vector2_8 + num8 * vector2_3;
      guiControlCheckbox2.OriginAlign = guiDrawAlignEnum1;
      this.m_controlHintsCheckbox = guiControlCheckbox2;
      this.m_controlHintsCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      float num9 = num8 + 1f;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ShowGoodBotHints));
      myGuiControlLabel10.Position = vector2_5 + num9 * vector2_3 + vector2_4;
      myGuiControlLabel10.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel11 = myGuiControlLabel10;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowGoodBotHints));
      guiControlCheckbox3.Position = vector2_8 + num9 * vector2_3;
      guiControlCheckbox3.OriginAlign = guiDrawAlignEnum1;
      this.m_goodbotHintsCheckbox = guiControlCheckbox3;
      this.m_goodbotHintsCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      Vector2? position1 = new Vector2?();
      Vector2? size1 = new Vector2?();
      Vector4? colorMask1 = new Vector4?();
      Action<MyGuiControlButton> action1 = new Action<MyGuiControlButton>(this.OnResetGoodbotPressed);
      string toolTip = MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints_TTIP);
      Action<MyGuiControlButton> onButtonClick1 = action1;
      int? buttonIndex1 = new int?();
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(position1, size: size1, colorMask: colorMask1, toolTip: toolTip, onButtonClick: onButtonClick1, buttonIndex: buttonIndex1);
      guiControlButton1.Position = new Vector2(this.Size.Value.X * 0.5f - vector2_1.X, this.m_goodbotHintsCheckbox.Position.Y);
      guiControlButton1.OriginAlign = guiDrawAlignEnum2;
      guiControlButton1.Text = MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_ResetGoodbotHints);
      guiControlButton1.IsAutoScaleEnabled = true;
      guiControlButton1.IsAutoEllipsisEnabled = true;
      this.m_goodBotResetButton = guiControlButton1;
      this.m_goodBotResetButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      MyGuiControlLabel myGuiControlLabel12 = (MyGuiControlLabel) null;
      if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
      {
        ++num9;
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.EnableNewNewGameScreen));
        myGuiControlLabel3.Position = vector2_5 + num9 * vector2_3 + vector2_4;
        myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel12 = myGuiControlLabel3;
        MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableNewNewGameScreen));
        guiControlCheckbox4.Position = vector2_8 + num9 * vector2_3;
        guiControlCheckbox4.OriginAlign = guiDrawAlignEnum1;
        this.m_enableNewNewGameScreen = guiControlCheckbox4;
        this.m_enableNewNewGameScreen.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      }
      MyGuiControlLabel myGuiControlLabel13 = (MyGuiControlLabel) null;
      if (MyFakes.ENABLE_ROTATION_HINTS && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        ++num9;
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ShowRotationHints));
        myGuiControlLabel3.Position = vector2_5 + num9 * vector2_3 + vector2_4;
        myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel13 = myGuiControlLabel3;
        MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowRotationHints));
        guiControlCheckbox4.Position = vector2_8 + num9 * vector2_3;
        guiControlCheckbox4.OriginAlign = guiDrawAlignEnum1;
        this.m_rotationHintsCheckbox = guiControlCheckbox4;
        this.m_rotationHintsCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      }
      MyGuiControlLabel myGuiControlLabel14 = (MyGuiControlLabel) null;
      if (!this.m_isLimitedMenu)
      {
        ++num9;
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MyCommonTexts.EnableSteamCloud), (object) MyGameService.Service.ServiceName));
        myGuiControlLabel3.Position = vector2_5 + num9 * vector2_3 + vector2_4;
        myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
        myGuiControlLabel14 = myGuiControlLabel3;
        MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox(toolTip: string.Format(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnableSteamCloud), (object) MyGameService.Service.ServiceName));
        guiControlCheckbox4.Position = vector2_8 + num9 * vector2_3;
        guiControlCheckbox4.OriginAlign = guiDrawAlignEnum1;
        this.m_cloudCheckbox = guiControlCheckbox4;
        this.m_cloudCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      }
      float num10 = num9 + 1f;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading));
      myGuiControlLabel15.Position = vector2_5 + num10 * vector2_3 + vector2_4;
      myGuiControlLabel15.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel16 = myGuiControlLabel15;
      MyGuiControlCheckbox guiControlCheckbox5 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.GameOptions_EnableTrading_TTIP));
      guiControlCheckbox5.Position = vector2_8 + num10 * vector2_3;
      guiControlCheckbox5.OriginAlign = guiDrawAlignEnum1;
      this.m_enableTradingCheckbox = guiControlCheckbox5;
      this.m_enableTradingCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      float num11 = num10 + 1f;
      MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.GDPR_Caption));
      myGuiControlLabel17.Position = vector2_5 + num11 * vector2_3 + vector2_4;
      myGuiControlLabel17.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel18 = myGuiControlLabel17;
      MyGuiControlCheckbox guiControlCheckbox6 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_GDPRConsent));
      guiControlCheckbox6.Position = vector2_8 + num11 * vector2_3;
      guiControlCheckbox6.OriginAlign = guiDrawAlignEnum1;
      this.m_GDPRConsentCheckbox = guiControlCheckbox6;
      MyGuiControlCheckbox gdprConsentCheckbox = this.m_GDPRConsentCheckbox;
      bool? gdprConsent = MySandboxGame.Config.GDPRConsent;
      bool flag = true;
      int num12 = gdprConsent.GetValueOrDefault() == flag & gdprConsent.HasValue ? 1 : (!MySandboxGame.Config.GDPRConsent.HasValue ? 1 : 0);
      gdprConsentCheckbox.IsChecked = num12 != 0;
      this.m_GDPRConsentCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton(onButtonClick: new Action<MyGuiControlButton>(this.OnWorkshopConsentClick));
      guiControlButton2.Position = new Vector2(this.Size.Value.X * 0.5f - vector2_1.X, this.m_GDPRConsentCheckbox.Position.Y);
      guiControlButton2.OriginAlign = guiDrawAlignEnum2;
      guiControlButton2.Text = "Mod.Io " + MyTexts.GetString(MySpaceTexts.Consent);
      guiControlButton2.IsAutoScaleEnabled = true;
      guiControlButton2.IsAutoEllipsisEnabled = true;
      MyGuiControlButton guiControlButton3 = guiControlButton2;
      if (MyFakes.ENABLE_AREA_INTERACTIONS)
      {
        num11 += 1.08f;
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.AreaInteration_Label));
        myGuiControlLabel3.Position = vector2_5 + num11 * vector2_3 + vector2_4;
        myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
        MyGuiControlLabel myGuiControlLabel19 = myGuiControlLabel3;
        MyGuiControlCheckbox guiControlCheckbox4 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGame_AreaInteraction));
        guiControlCheckbox4.Position = vector2_8 + num11 * vector2_3;
        guiControlCheckbox4.OriginAlign = guiDrawAlignEnum1;
        this.m_areaInteractionCheckbox = guiControlCheckbox4;
        this.m_areaInteractionCheckbox.IsChecked = MySandboxGame.Config.AreaInteraction;
        this.m_areaInteractionCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel19);
        this.Controls.Add((MyGuiControlBase) this.m_areaInteractionCheckbox);
      }
      float num13 = num11 + 1.35f;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_languageCombobox);
      if (this.m_localizationWebButton != null)
        this.Controls.Add((MyGuiControlBase) this.m_localizationWebButton);
      if (this.m_localizationWebButton != null)
        this.Controls.Add((MyGuiControlBase) this.m_localizationWarningLabel);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      this.Controls.Add((MyGuiControlBase) this.m_buildingModeCombobox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      this.Controls.Add((MyGuiControlBase) this.m_experimentalCheckbox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel11);
      this.Controls.Add((MyGuiControlBase) this.m_controlHintsCheckbox);
      this.Controls.Add((MyGuiControlBase) this.m_goodbotHintsCheckbox);
      if (MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
        this.Controls.Add((MyGuiControlBase) this.m_enableNewNewGameScreen);
      }
      this.Controls.Add((MyGuiControlBase) this.m_goodBotResetButton);
      if (MyFakes.ENABLE_ROTATION_HINTS && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel13);
        this.Controls.Add((MyGuiControlBase) this.m_rotationHintsCheckbox);
      }
      if (!this.m_isLimitedMenu)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
        this.Controls.Add((MyGuiControlBase) this.m_cloudCheckbox);
      }
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
      this.Controls.Add((MyGuiControlBase) this.m_enableTradingCheckbox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
      this.Controls.Add((MyGuiControlBase) this.m_GDPRConsentCheckbox);
      this.Controls.Add((MyGuiControlBase) guiControlButton3);
      this.FocusedControl = (MyGuiControlBase) this.m_buttonOk;
      this.CloseButtonEnabled = true;
      this.m_experimentalCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.checkboxChanged);
    }

    private void CreateTab2Menu(bool constructor)
    {
      float x = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      Vector2 vector2_1 = new Vector2(0.0f, 0.042f);
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      Vector2 vector2_2 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_3 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float num1 = 0.0f;
      Vector2 vector2_4 = (this.m_size.Value / 2f - vector2_2) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_5 = (this.m_size.Value / 2f - vector2_2) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_3) * new Vector2(0.0f, 1f);
      MyGuiDrawAlignEnum guiDrawAlignEnum1 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiDrawAlignEnum guiDrawAlignEnum2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      Vector2 vector2_7 = new Vector2(0.0f, 0.008f);
      float num2 = 0.0015f;
      Vector2 vector2_8 = new Vector2(vector2_5.X - (x + num2), vector2_5.Y);
      float num3 = num1 + 2f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ShowCrosshair));
      myGuiControlLabel1.Position = vector2_4 + num3 * vector2_1 + vector2_7;
      myGuiControlLabel1.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox();
      guiControlCombobox1.Position = vector2_5 + num3 * vector2_1;
      guiControlCombobox1.OriginAlign = guiDrawAlignEnum2;
      this.m_crosshairCombobox = guiControlCombobox1;
      this.m_crosshairCombobox.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsShowCrosshair));
      this.m_crosshairCombobox.AddItem(1L, MyCommonTexts.Crosshair_AlwaysVisible);
      this.m_crosshairCombobox.AddItem(0L, MyCommonTexts.Crosshair_VisibleWithHUD);
      this.m_crosshairCombobox.AddItem(2L, MyCommonTexts.Crosshair_AlwaysHidden);
      this.m_crosshairCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.crosshairCombobox_ItemSelected);
      float num4 = num3 + 1f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.IronSightSwitch));
      myGuiControlLabel3.Position = vector2_4 + num4 * vector2_1 + vector2_7;
      myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlCombobox guiControlCombobox2 = new MyGuiControlCombobox();
      guiControlCombobox2.Position = vector2_5 + num4 * vector2_1;
      guiControlCombobox2.OriginAlign = guiDrawAlignEnum2;
      this.m_ironSightCombobox = guiControlCombobox2;
      this.m_ironSightCombobox.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsIronsightSwitchType));
      this.m_ironSightCombobox.AddItem(1L, MyCommonTexts.IronSight_Toggle);
      this.m_ironSightCombobox.AddItem(0L, MyCommonTexts.IronSight_Hold);
      this.m_ironSightCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.ironSightCombobox_ItemSelected);
      float num5 = num4 + 1f;
      float hitPreviewWidth = 0.16f;
      float buttonWidth = 0.0396522f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_HitIndicatorLabel));
      myGuiControlLabel5.Position = vector2_4 + num5 * vector2_1 + vector2_7;
      myGuiControlLabel5.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      MyGuiControlCombobox guiControlCombobox3 = new MyGuiControlCombobox();
      guiControlCombobox3.Position = vector2_5 + num5 * vector2_1;
      guiControlCombobox3.OriginAlign = guiDrawAlignEnum2;
      this.m_hitCombobox = guiControlCombobox3;
      this.m_hitCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_HitIndicatorTooltip));
      this.m_hitCombobox.AddItem(0L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Character);
      this.m_hitCombobox.AddItem(5L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Friendly);
      this.m_hitCombobox.AddItem(1L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Headshot);
      this.m_hitCombobox.AddItem(2L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Kill);
      this.m_hitCombobox.AddItem(3L, MySpaceTexts.ScreenOptionsGame_HitIndicator_Grid);
      this.m_hitCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.HitCombobox_ItemSelected);
      this.m_hitCombobox.SelectItemByIndex(0);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      this.Controls.Add((MyGuiControlBase) this.m_hitCombobox);
      float num6 = num5 + 1f;
      float num7;
      if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
      {
        this.m_crosshairLookImage = new MyGuiControlImage();
        this.m_crosshairLookImage.Size = new Vector2(0.16f, 0.16f);
        this.m_crosshairLookImage.BorderColor = (Vector4) Color.Gray;
        this.m_crosshairLookImage.BorderSize = 1;
        this.m_crosshairLookImage.BorderEnabled = true;
        this.m_crosshairLookImage.UserData = (object) 0;
        MyGuiControlImage crosshairLookImage = this.m_crosshairLookImage;
        MyGuiControlImage.MyDrawTexture[] textures1 = new MyGuiControlImage.MyDrawTexture[1];
        MyGuiControlImage.MyDrawTexture myDrawTexture = new MyGuiControlImage.MyDrawTexture();
        myDrawTexture.ColorMask = new Vector4?((Vector4) Color.Red);
        myDrawTexture.Texture = "";
        textures1[0] = myDrawTexture;
        crosshairLookImage.SetTextures(textures1);
        float num8 = (float) (((double) vector2_5.X - (double) vector2_4.X - (double) this.m_crosshairLookImage.Size.X) / 4.0);
        this.m_crosshairLookNextImage = new MyGuiControlImage();
        this.m_crosshairLookNextImage.UserData = (object) 1;
        this.m_crosshairLookNextImage.Size = new Vector2(num8, num8);
        MyGuiControlImage crosshairLookNextImage = this.m_crosshairLookNextImage;
        MyGuiControlImage.MyDrawTexture[] textures2 = new MyGuiControlImage.MyDrawTexture[1];
        myDrawTexture = new MyGuiControlImage.MyDrawTexture();
        myDrawTexture.ColorMask = new Vector4?((Vector4) Color.Magenta);
        myDrawTexture.Texture = "";
        textures2[0] = myDrawTexture;
        crosshairLookNextImage.SetTextures(textures2);
        this.m_crosshairLookPrevImage = new MyGuiControlImage();
        this.m_crosshairLookPrevImage.UserData = (object) -1;
        this.m_crosshairLookPrevImage.Size = new Vector2(num8, num8);
        MyGuiControlImage crosshairLookPrevImage = this.m_crosshairLookPrevImage;
        MyGuiControlImage.MyDrawTexture[] textures3 = new MyGuiControlImage.MyDrawTexture[1];
        myDrawTexture = new MyGuiControlImage.MyDrawTexture();
        myDrawTexture.ColorMask = new Vector4?((Vector4) Color.Yellow);
        myDrawTexture.Texture = "";
        textures3[0] = myDrawTexture;
        crosshairLookPrevImage.SetTextures(textures3);
        this.m_crosshairLookNext2Image = new MyGuiControlImage();
        this.m_crosshairLookNext2Image.UserData = (object) 2;
        this.m_crosshairLookNext2Image.Size = new Vector2(num8, num8);
        MyGuiControlImage crosshairLookNext2Image = this.m_crosshairLookNext2Image;
        MyGuiControlImage.MyDrawTexture[] textures4 = new MyGuiControlImage.MyDrawTexture[1];
        myDrawTexture = new MyGuiControlImage.MyDrawTexture();
        myDrawTexture.ColorMask = new Vector4?((Vector4) Color.Blue);
        myDrawTexture.Texture = "";
        textures4[0] = myDrawTexture;
        crosshairLookNext2Image.SetTextures(textures4);
        this.m_crosshairLookPrev2Image = new MyGuiControlImage();
        this.m_crosshairLookPrev2Image.UserData = (object) -2;
        this.m_crosshairLookPrev2Image.Size = new Vector2(num8, num8);
        MyGuiControlImage crosshairLookPrev2Image = this.m_crosshairLookPrev2Image;
        MyGuiControlImage.MyDrawTexture[] textures5 = new MyGuiControlImage.MyDrawTexture[1];
        myDrawTexture = new MyGuiControlImage.MyDrawTexture();
        myDrawTexture.ColorMask = new Vector4?((Vector4) Color.Green);
        myDrawTexture.Texture = "";
        textures5[0] = myDrawTexture;
        crosshairLookPrev2Image.SetTextures(textures5);
        this.m_crosshairLookImage.Position = vector2_4 * Vector2.UnitY + num6 * vector2_1;
        this.m_crosshairLookImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToRightCenterOf((MyGuiControlBase) this.m_crosshairLookNextImage, (MyGuiControlBase) this.m_crosshairLookImage, Vector2.Zero);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToRightCenterOf((MyGuiControlBase) this.m_crosshairLookNext2Image, (MyGuiControlBase) this.m_crosshairLookNextImage, Vector2.Zero);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) this.m_crosshairLookPrevImage, (MyGuiControlBase) this.m_crosshairLookImage, Vector2.Zero);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) this.m_crosshairLookPrev2Image, (MyGuiControlBase) this.m_crosshairLookPrevImage, Vector2.Zero);
        this.m_crosshairLookNextButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Square, onButtonClick: new Action<MyGuiControlButton>(this.ChangeLook));
        this.m_crosshairLookNextButton.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE);
        this.m_crosshairLookNextButton.IconRotation = 3.141593f;
        this.m_crosshairLookNextButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        this.m_crosshairLookNextButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
        this.m_crosshairLookNextButton.Position = this.m_crosshairLookImage.Position - this.m_crosshairLookImage.Size * new Vector2(0.5f, -1f) - this.m_crosshairLookNextButton.Size;
        this.m_crosshairLookNextButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_crosshairLookPrevButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Square, onButtonClick: new Action<MyGuiControlButton>(this.ChangeLook));
        this.m_crosshairLookPrevButton.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE);
        this.m_crosshairLookPrevButton.IconRotation = 0.0f;
        this.m_crosshairLookPrevButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        this.m_crosshairLookPrevButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
        this.m_crosshairLookPrevButton.Position = this.m_crosshairLookImage.Position + this.m_crosshairLookImage.Size * new Vector2(0.5f, 1f) - this.m_crosshairLookNextButton.Size * Vector2.UnitY + this.m_crosshairLookNextButton.Size * Vector2.UnitX;
        this.m_crosshairLookPrevButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookNextButton);
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookPrevButton);
        num7 = num6 + 3.996f;
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookImage);
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookNextImage);
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookNext2Image);
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookPrevImage);
        this.Controls.Add((MyGuiControlBase) this.m_crosshairLookPrev2Image);
      }
      else
      {
        MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_CrosshairColor));
        myGuiControlLabel7.Position = vector2_4 + num6 * vector2_1 + vector2_7;
        myGuiControlLabel7.OriginAlign = guiDrawAlignEnum1;
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
        MyGuiScreenOptionsGame.ColorPreviewControl colorPreviewControl = new MyGuiScreenOptionsGame.ColorPreviewControl();
        colorPreviewControl.Position = vector2_5 + num6 * vector2_1;
        colorPreviewControl.Size = this.m_ironSightCombobox.Size;
        colorPreviewControl.OriginAlign = guiDrawAlignEnum2;
        this.m_colorPreview = (MyGuiControlPanel) colorPreviewControl;
        this.m_colorPreview.ColorMask = (Vector4) Color.Transparent;
        this.m_colorPreview.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
        this.m_colorPreview.CanHaveFocus = false;
        this.Controls.Add((MyGuiControlBase) this.m_colorPreview);
        num7 = num6 + 1f;
      }
      this.m_colorPicker = new MyGuiScreenOptionsGame.ColorPickerControlWrap();
      Vector2 end = vector2_5 + num7 * vector2_1;
      this.m_colorPicker.Init(this, vector2_4 + num7 * vector2_1, end, hitPreviewWidth, buttonWidth);
      this.m_colorPicker.ColorChanged += new Action<Color>(this.CrosshairColorChanged);
      float num9 = num7 + 2.376f;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_crosshairCombobox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.Controls.Add((MyGuiControlBase) this.m_ironSightCombobox);
      MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.LabelOptionsDisplay_SpriteMainViewportScale));
      myGuiControlLabel8.Position = vector2_4 + num9 * vector2_1 + vector2_7;
      myGuiControlLabel8.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel9 = myGuiControlLabel8;
      Vector2? position1 = new Vector2?();
      string str1 = new StringBuilder("{0}%").ToString();
      float? defaultValue1 = new float?(MySandboxGame.Config.SpriteMainViewportScale * 100f);
      Vector4? color1 = new Vector4?();
      string labelText = str1;
      string toolTip1 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsDisplay_SpriteMainViewportScale);
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider(position1, 70f, 100f, defaultValue: defaultValue1, color: color1, labelText: labelText, labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: toolTip1, showLabel: true);
      guiControlSlider1.Position = vector2_5 + num9 * vector2_1;
      guiControlSlider1.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider1.Size = new Vector2(x, 0.0f);
      this.m_spriteMainViewportScaleSlider = guiControlSlider1;
      this.m_spriteMainViewportScaleSlider.ValueChanged += new Action<MyGuiControlSlider>(this.sliderChanged);
      float num10 = num9 + 1.08f;
      MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIOpacity));
      myGuiControlLabel10.Position = vector2_4 + num10 * vector2_1 + vector2_7;
      myGuiControlLabel10.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel11 = myGuiControlLabel10;
      Vector2? position2 = new Vector2?();
      string str2 = MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIOpacity);
      float? defaultValue2 = new float?(1f);
      Vector4? color2 = new Vector4?();
      string toolTip2 = str2;
      MyGuiControlSlider guiControlSlider2 = new MyGuiControlSlider(position2, 0.1f, defaultValue: defaultValue2, color: color2, toolTip: toolTip2);
      guiControlSlider2.Position = vector2_5 + num10 * vector2_1;
      guiControlSlider2.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider2.Size = new Vector2(x, 0.0f);
      this.m_UIOpacitySlider = guiControlSlider2;
      this.m_UIOpacitySlider.ValueChanged += new Action<MyGuiControlSlider>(this.sliderChanged);
      float num11 = num10 + 1.08f;
      MyGuiControlLabel myGuiControlLabel12 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_UIBkOpacity));
      myGuiControlLabel12.Position = vector2_4 + num11 * vector2_1 + vector2_7;
      myGuiControlLabel12.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel13 = myGuiControlLabel12;
      Vector2? position3 = new Vector2?();
      string str3 = MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsUIBkOpacity);
      float? defaultValue3 = new float?(1f);
      Vector4? color3 = new Vector4?();
      string toolTip3 = str3;
      MyGuiControlSlider guiControlSlider3 = new MyGuiControlSlider(position3, defaultValue: defaultValue3, color: color3, toolTip: toolTip3);
      guiControlSlider3.Position = vector2_5 + num11 * vector2_1;
      guiControlSlider3.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider3.Size = new Vector2(x, 0.0f);
      this.m_UIBkOpacitySlider = guiControlSlider3;
      this.m_UIBkOpacitySlider.ValueChanged += new Action<MyGuiControlSlider>(this.sliderChanged);
      float num12 = num11 + 1.08f;
      MyGuiControlLabel myGuiControlLabel14 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_HUDBkOpacity));
      myGuiControlLabel14.Position = vector2_4 + num12 * vector2_1 + vector2_7;
      myGuiControlLabel14.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel15 = myGuiControlLabel14;
      Vector2? position4 = new Vector2?();
      string str4 = MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsHUDBkOpacity);
      float? defaultValue4 = new float?(1f);
      Vector4? color4 = new Vector4?();
      string toolTip4 = str4;
      MyGuiControlSlider guiControlSlider4 = new MyGuiControlSlider(position4, defaultValue: defaultValue4, color: color4, toolTip: toolTip4);
      guiControlSlider4.Position = vector2_5 + num12 * vector2_1;
      guiControlSlider4.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider4.Size = new Vector2(x, 0.0f);
      this.m_HUDBkOpacitySlider = guiControlSlider4;
      this.m_HUDBkOpacitySlider.ValueChanged += new Action<MyGuiControlSlider>(this.sliderChanged);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel9);
      this.Controls.Add((MyGuiControlBase) this.m_spriteMainViewportScaleSlider);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel11);
      this.Controls.Add((MyGuiControlBase) this.m_UIOpacitySlider);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel13);
      this.Controls.Add((MyGuiControlBase) this.m_UIBkOpacitySlider);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel15);
      this.Controls.Add((MyGuiControlBase) this.m_HUDBkOpacitySlider);
    }

    private void crosshairCombobox_ItemSelected() => this.m_settings.ShowCrosshair = (MyConfig.CrosshairSwitch) this.m_crosshairCombobox.GetSelectedKey();

    private void ironSightCombobox_ItemSelected() => this.m_settings.IronSight = (IronSightSwitchStateType) this.m_ironSightCombobox.GetSelectedKey();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
        return;
      int currentPage = (int) this.CurrentPage;
      int num = !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) ? currentPage - 1 : currentPage + 1;
      if (num < 0)
        num = 1;
      else if (num > 1)
        num = 0;
      this.SelectTab((MyGuiScreenOptionsGame.PageEnum) num);
    }

    public void Tab2UpdateControls(bool constructor)
    {
      this.ChangeLook(this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Texture);
      this.m_colorPicker.Color = this.m_settings.HitIndicatorSettings[MySession.MyHitIndicatorTarget.Character].Color;
      this.m_crosshairCombobox.SelectItemByKey((long) this.m_settings.ShowCrosshair);
      this.m_ironSightCombobox.SelectItemByKey((long) this.m_settings.IronSight);
      this.m_spriteMainViewportScaleSlider.Value = this.m_settings.SpriteMainViewportScale;
      this.m_UIOpacitySlider.Value = this.m_settings.UIOpacity;
      this.m_UIBkOpacitySlider.Value = this.m_settings.UIBkOpacity;
      this.m_HUDBkOpacitySlider.Value = this.m_settings.HUDBkOpacity;
    }

    public void Tab2DoChanges()
    {
      Dictionary<MySession.MyHitIndicatorTarget, MyGuiScreenOptionsGame.HitIndicatorSettings> indicatorSettings = this.m_settings.HitIndicatorSettings;
      string fromPath = Path.Combine(MyFileSystem.ContentPath, "Textures");
      if (indicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Character))
      {
        MySandboxGame.Config.HitIndicatorColorCharacter = indicatorSettings[MySession.MyHitIndicatorTarget.Character].Color;
        MySandboxGame.Config.HitIndicatorTextureCharacter = MyFileSystem.MakeRelativePath(fromPath, indicatorSettings[MySession.MyHitIndicatorTarget.Character].Texture);
      }
      if (indicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Friend))
      {
        MySandboxGame.Config.HitIndicatorColorFriend = indicatorSettings[MySession.MyHitIndicatorTarget.Friend].Color;
        MySandboxGame.Config.HitIndicatorTextureFriend = MyFileSystem.MakeRelativePath(fromPath, indicatorSettings[MySession.MyHitIndicatorTarget.Friend].Texture);
      }
      if (indicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Grid))
      {
        MySandboxGame.Config.HitIndicatorColorGrid = indicatorSettings[MySession.MyHitIndicatorTarget.Grid].Color;
        MySandboxGame.Config.HitIndicatorTextureGrid = MyFileSystem.MakeRelativePath(fromPath, indicatorSettings[MySession.MyHitIndicatorTarget.Grid].Texture);
      }
      if (indicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Headshot))
      {
        MySandboxGame.Config.HitIndicatorColorHeadshot = indicatorSettings[MySession.MyHitIndicatorTarget.Headshot].Color;
        MySandboxGame.Config.HitIndicatorTextureHeadshot = MyFileSystem.MakeRelativePath(fromPath, indicatorSettings[MySession.MyHitIndicatorTarget.Headshot].Texture);
      }
      if (indicatorSettings.ContainsKey(MySession.MyHitIndicatorTarget.Kill))
      {
        MySandboxGame.Config.HitIndicatorColorKill = indicatorSettings[MySession.MyHitIndicatorTarget.Kill].Color;
        MySandboxGame.Config.HitIndicatorTextureKill = MyFileSystem.MakeRelativePath(fromPath, indicatorSettings[MySession.MyHitIndicatorTarget.Kill].Texture);
      }
      MySandboxGame.Config.ShowCrosshair = this.m_settings.ShowCrosshair;
      MySandboxGame.Config.IronSightSwitchState = this.m_settings.IronSight;
      MySandboxGame.Config.SpriteMainViewportScale = this.m_settings.SpriteMainViewportScale / 100f;
      MySandboxGame.Static.UpdateUIScale();
      MySandboxGame.Config.UIOpacity = this.m_settings.UIOpacity;
      MySandboxGame.Config.UIBkOpacity = this.m_settings.UIBkOpacity;
      MySandboxGame.Config.HUDBkOpacity = this.m_settings.HUDBkOpacity;
    }

    private void CrosshairColorChanged(Color obj)
    {
      if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
      {
        this.m_crosshairLookImage.Textures[0].ColorMask = new Vector4?((Vector4) obj);
        this.m_crosshairLookNextImage.Textures[0].ColorMask = new Vector4?((Vector4) obj);
        this.m_crosshairLookPrevImage.Textures[0].ColorMask = new Vector4?((Vector4) obj);
        this.m_crosshairLookNext2Image.Textures[0].ColorMask = new Vector4?((Vector4) obj);
        this.m_crosshairLookPrev2Image.Textures[0].ColorMask = new Vector4?((Vector4) obj);
      }
      else
        this.m_colorPreview.ColorMask = (Vector4) obj;
      this.RecordTextureOrColorChange();
    }

    private void HitCombobox_ItemSelected()
    {
      if (this.m_isIniting)
        return;
      MySession.MyHitIndicatorTarget selectedKey = (MySession.MyHitIndicatorTarget) this.m_hitCombobox.GetSelectedKey();
      if (this.m_settings.HitIndicatorSettings == null || !this.m_settings.HitIndicatorSettings.ContainsKey(selectedKey))
        return;
      MyGuiScreenOptionsGame.HitIndicatorSettings indicatorSetting = this.m_settings.HitIndicatorSettings[selectedKey];
      this.m_colorPicker.Color = indicatorSetting.Color;
      this.ChangeLook(indicatorSetting.Texture);
    }

    private void RecordTextureOrColorChange()
    {
      if (this.m_isIniting)
        return;
      MySession.MyHitIndicatorTarget selectedKey = (MySession.MyHitIndicatorTarget) this.m_hitCombobox.GetSelectedKey();
      this.m_settings.HitIndicatorSettings[selectedKey] = new MyGuiScreenOptionsGame.HitIndicatorSettings()
      {
        Texture = this.m_settings.HitIndicatorSettings[selectedKey].Texture,
        Color = this.m_colorPicker.Color
      };
    }

    public void ChangeLook(string texture) => this.ChangeLook(this.m_crosshairFiles.IndexOf(texture));

    public void ChangeLook(MyGuiControlButton sender)
    {
      int num = this.m_crosshairFiles.IndexOf(this.m_crosshairLookImage.Textures[0].Texture);
      if (num == -1)
        num = 0;
      this.ChangeLook(sender != this.m_crosshairLookNextButton ? num - 1 : num + 1);
    }

    private void ChangeLook(int index)
    {
      if (!MyFakes.HIDE_CROSSHAIR_OPTIONS)
      {
        this.m_crosshairLookImage.Textures[0].Texture = this.m_crosshairFiles[Loop(index)];
        this.m_crosshairLookNextImage.Textures[0].Texture = this.m_crosshairFiles[Loop(index - 1)];
        this.m_crosshairLookPrevImage.Textures[0].Texture = this.m_crosshairFiles[Loop(index + 1)];
        this.m_crosshairLookNext2Image.Textures[0].Texture = this.m_crosshairFiles[Loop(index - 2)];
        this.m_crosshairLookPrev2Image.Textures[0].Texture = this.m_crosshairFiles[Loop(index + 2)];
      }
      this.RecordTextureOrColorChange();

      int Loop(int i)
      {
        if (i >= this.m_crosshairFiles.Count)
          i -= this.m_crosshairFiles.Count;
        else if (i < 0)
          i += this.m_crosshairFiles.Count;
        return i;
      }
    }

    public void OnTabSwitchClick(MyGuiControlButton sender) => this.SelectTab((MyGuiScreenOptionsGame.PageEnum) sender.UserData);

    private struct OptionsGameSettings
    {
      public MyLanguagesEnum Language;
      public MyCubeBuilder.BuildingModeEnum BuildingMode;
      public MyStringId SkinId;
      public bool ExperimentalMode;
      public bool ControlHints;
      public bool GoodBotHints;
      public bool EnableNewNewGameScreen;
      public bool RotationHints;
      public MyConfig.CrosshairSwitch ShowCrosshair;
      public IronSightSwitchStateType IronSight;
      public bool EnableTrading;
      public bool AreaInteraction;
      public bool EnableSteamCloud;
      public bool EnablePrediction;
      public bool ShowPlayerNamesOnHud;
      public bool EnablePerformanceWarnings;
      public float UIOpacity;
      public float UIBkOpacity;
      public float HUDBkOpacity;
      public float SpriteMainViewportScale;
      public bool? GDPR;
      public Dictionary<MySession.MyHitIndicatorTarget, MyGuiScreenOptionsGame.HitIndicatorSettings> HitIndicatorSettings;
    }

    private struct HitIndicatorSettings
    {
      public string Texture;
      public Color Color;
    }

    private enum PageEnum
    {
      General,
      Crosshair,
    }

    private class ColorPreviewControl : MyGuiControlPanel
    {
      protected override void DrawBackground(float transitionAlpha)
      {
        if (this.BackgroundTexture == null || (double) this.ColorMask.W <= 0.0)
          return;
        this.BackgroundTexture.Draw(this.GetPositionAbsoluteTopLeft(), this.Size, (Color) this.ColorMask);
      }
    }

    private class ColorPickerControlWrap
    {
      private static readonly Regex HEX_REGEX = new Regex("^(#{0,1})([0-9A-Fa-f]{6})([0-9A-Fa-f]{2})?$");
      private const float m_fixTextboxHeight = 0.2093023f;
      private readonly StringBuilder m_hexSb = new StringBuilder();
      private MyGuiControlSlider m_hueSlider;
      private MyGuiControlSlider m_saturationSlider;
      private MyGuiControlSlider m_valueSlider;
      private MyGuiControlSlider m_transparencySlider;
      private MyGuiControlTextbox m_hexTextbox;
      private MyGuiControlLabel m_hexLabel;

      public event Action<Color> ColorChanged;

      public Color Color
      {
        get => this.GetCurrentColor();
        set
        {
          if (value == this.GetCurrentColor())
            return;
          this.SetColorInSlider(value);
          this.UpdateHex();
          Action<Color> colorChanged = this.ColorChanged;
          if (colorChanged == null)
            return;
          colorChanged(value);
        }
      }

      public bool HexVisible
      {
        get => this.m_hexTextbox.Visible;
        set
        {
          this.m_hexTextbox.Visible = value;
          this.m_hexLabel.Visible = value;
        }
      }

      private void SetColorInSlider(Color c)
      {
        Vector3 hsv = c.ColorToHSV();
        this.m_hueSlider.Value = hsv.X * 360f;
        this.m_saturationSlider.Value = hsv.Y;
        this.m_valueSlider.Value = hsv.Z;
        this.m_transparencySlider.Value = (float) c.A / (float) byte.MaxValue;
      }

      public void Init(
        MyGuiScreenOptionsGame parent,
        Vector2 start,
        Vector2 end,
        float hitPreviewWidth,
        float buttonWidth)
      {
        MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: "H:");
        MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: "S:");
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: "V:");
        MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenOptionsGame_CrosshairTransparency));
        this.m_hexLabel = new MyGuiControlLabel(text: "Hex:");
        this.m_hueSlider = new MyGuiControlSlider(maxValue: 360f, labelText: string.Empty, labelDecimalPlaces: 0, visualStyle: MyGuiControlSliderStyleEnum.Hue);
        this.m_hueSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChange);
        this.m_saturationSlider = new MyGuiControlSlider(defaultValue: new float?(0.0f), labelText: string.Empty);
        this.m_saturationSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChange);
        this.m_valueSlider = new MyGuiControlSlider(defaultValue: new float?(0.0f), labelText: string.Empty);
        this.m_valueSlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChange);
        this.m_transparencySlider = new MyGuiControlSlider(defaultValue: new float?(0.0f), labelText: string.Empty);
        this.m_transparencySlider.ValueChanged += new Action<MyGuiControlSlider>(this.OnSliderValueChange);
        Vector2 margin = new Vector2(Math.Abs(end.X - start.X) / 15f / 8f, 0.0f);
        this.m_hexTextbox = new MyGuiControlTextbox();
        this.m_hexTextbox.MaxLength = 9;
        this.m_hexTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.HexTextboxOnEnterPressed);
        this.m_hexTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.HexTextboxOnFocusChanged);
        this.m_saturationSlider.Size = new Vector2(hitPreviewWidth, this.m_saturationSlider.Size.X);
        this.m_saturationSlider.PositionX = (float) (0.0 - (double) hitPreviewWidth / 2.0);
        this.m_saturationSlider.PositionY = start.Y;
        this.m_saturationSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_valueSlider.PositionX = (float) (0.0 + (double) hitPreviewWidth / 2.0) + buttonWidth;
        this.m_valueSlider.Size = new Vector2(end.X - this.m_valueSlider.PositionX, this.m_saturationSlider.Size.Y);
        this.m_valueSlider.PositionY = start.Y;
        this.m_valueSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_transparencySlider.Size = this.m_saturationSlider.Size;
        this.m_transparencySlider.PositionX = this.m_saturationSlider.PositionX;
        this.m_transparencySlider.PositionY = this.m_saturationSlider.PositionY + this.m_saturationSlider.Size.Y;
        this.m_transparencySlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_hueSlider.PositionX = start.X + myGuiControlLabel1.Size.X + margin.X;
        this.m_hueSlider.PositionY = start.Y;
        this.m_hueSlider.Size = new Vector2(this.m_valueSlider.Size.X * 0.95f, this.m_hueSlider.Size.Y);
        this.m_hueSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_hexTextbox.PositionX = this.m_valueSlider.PositionX;
        this.m_hexTextbox.PositionY = this.m_valueSlider.PositionY + this.m_valueSlider.Size.Y;
        this.m_hexTextbox.Size = new Vector2(this.m_valueSlider.Size.X, this.m_valueSlider.Size.Y);
        this.m_hexTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) myGuiControlLabel1, (MyGuiControlBase) this.m_hueSlider, margin);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) myGuiControlLabel2, (MyGuiControlBase) this.m_saturationSlider, margin);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) myGuiControlLabel3, (MyGuiControlBase) this.m_valueSlider, margin);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) myGuiControlLabel4, (MyGuiControlBase) this.m_transparencySlider, margin);
        MyGuiScreenOptionsGame.ColorPickerControlWrap.AttachToLeftCenterOf((MyGuiControlBase) this.m_hexLabel, (MyGuiControlBase) this.m_hexTextbox, margin);
        myGuiControlLabel4.PositionX = start.X + myGuiControlLabel4.Size.X;
        parent.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
        parent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        parent.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
        parent.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
        parent.Controls.Add((MyGuiControlBase) this.m_hexLabel);
        parent.Controls.Add((MyGuiControlBase) this.m_hueSlider);
        parent.Controls.Add((MyGuiControlBase) this.m_saturationSlider);
        parent.Controls.Add((MyGuiControlBase) this.m_valueSlider);
        parent.Controls.Add((MyGuiControlBase) this.m_transparencySlider);
        parent.Controls.Add((MyGuiControlBase) this.m_hexTextbox);
        this.m_hexLabel.Position = new Vector2(this.m_hexLabel.Position.X, myGuiControlLabel4.Position.Y);
        MyGuiControlTextbox hexTextbox = this.m_hexTextbox;
        hexTextbox.Position = hexTextbox.Position + new Vector2(0.0f, (float) ((double) this.m_hexTextbox.Size.Y * 0.209302321076393 / 2.0));
        this.Color = Color.Red;
      }

      private void UpdateHex()
      {
        Color currentColor = this.GetCurrentColor();
        this.m_hexSb.Clear();
        this.m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}{3:X2}", (object) currentColor.R, (object) currentColor.G, (object) currentColor.B, (object) currentColor.A);
        this.m_hexTextbox.SetText(this.m_hexSb);
      }

      private void HexTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
      {
        if (state || !(obj is MyGuiControlTextbox guiControlTextbox))
          return;
        this.HexTextboxOnEnterPressed(guiControlTextbox);
      }

      private void HexTextboxOnEnterPressed(MyGuiControlTextbox obj)
      {
        this.m_hexSb.Clear();
        obj.GetText(this.m_hexSb);
        string input = Regex.Replace(this.m_hexSb.ToString(), "\\s+", "");
        Match match = MyGuiScreenOptionsGame.ColorPickerControlWrap.HEX_REGEX.Match(input);
        if (!match.Success || match.Length == 0)
        {
          this.UpdateHex();
        }
        else
        {
          string str = match.Value;
          if (str.StartsWith("#"))
            str = str.Substring(1);
          byte num1 = byte.Parse(str.Substring(0, 2), NumberStyles.HexNumber);
          byte num2 = byte.Parse(str.Substring(2, 2), NumberStyles.HexNumber);
          byte num3 = byte.Parse(str.Substring(4, 2), NumberStyles.HexNumber);
          int maxValue = (int) byte.MaxValue;
          if (str.Length == 8)
            maxValue = (int) byte.Parse(str.Substring(6, 2), NumberStyles.HexNumber);
          this.Color = new Color((int) num1, (int) num2, (int) num3, maxValue);
        }
      }

      private void OnSliderValueChange(MyGuiControlSlider sender)
      {
        this.UpdateHex();
        Action<Color> colorChanged = this.ColorChanged;
        if (colorChanged == null)
          return;
        colorChanged(this.GetCurrentColor());
      }

      private Color GetCurrentColor()
      {
        Color color = new Vector3(this.m_hueSlider.Value / 360f, this.m_saturationSlider.Value, this.m_valueSlider.Value).HSVtoColor();
        color.A = (byte) ((double) this.m_transparencySlider.Value * (double) byte.MaxValue);
        return color;
      }

      public static void ShowInColumns(
        MyGuiScreenOptionsGame parent,
        Vector2 start,
        Vector2 end,
        int columns,
        Vector4 marginsInside,
        Vector2 marginsBetween,
        float rowHeight,
        params MyGuiControlBase[] controls)
      {
        float num1 = (Math.Abs(end.X - start.X) - marginsBetween.X * (float) (columns - 1)) / (float) columns;
        for (int index = 0; index < controls.Length; ++index)
        {
          if (controls[index] != null)
          {
            int num2 = index % columns;
            int num3 = index / columns;
            Vector2 vector2_1 = start + ((float) num2 * (num1 + marginsBetween.X) + marginsInside.X) * Vector2.UnitX + ((float) num3 * (marginsBetween.Y + rowHeight) + marginsInside.Y) * Vector2.UnitY;
            Vector2 vector2_2 = vector2_1 + (num1 - marginsInside.X - marginsInside.Z) * Vector2.UnitX + (rowHeight - marginsInside.W - marginsInside.Y) * Vector2.UnitY;
            controls[index].Size = new Vector2(Math.Abs(vector2_2.X - vector2_1.X), Math.Abs(vector2_2.Y - vector2_1.Y));
            controls[index].Position = vector2_1;
            controls[index].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          }
        }
      }

      public static void AttachToLeftCenterOf(
        MyGuiControlBase leftView,
        MyGuiControlBase rightView,
        Vector2 margin)
      {
        Vector2 vector2 = rightView.GetPositionAbsoluteCenterLeft() - margin;
        leftView.Position = vector2;
        leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      }

      public static void AttachToRightCenterOf(
        MyGuiControlBase leftView,
        MyGuiControlBase rightView,
        Vector2 margin)
      {
        Vector2 vector2 = rightView.GetPositionAbsoluteCenterRight() + margin;
        leftView.Position = vector2;
        leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      }

      public void FitInLine(
        MyGuiControlParent parent,
        Vector2 start,
        Vector2 end,
        params MyGuiControlBase[] controls)
      {
        controls[0].Position = start;
        controls[0].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        controls[3].Position = end;
        controls[3].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        controls[2].Position = end - 0.1046512f * controls[2].Size * Vector2.UnitY;
        controls[2].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        end -= controls[2].Size * new Vector2(1.1f, 0.0f);
        controls[1].Size = new Vector2((end - start).X, controls[1].Size.Y);
        controls[1].Position = end;
        controls[1].OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        parent.Controls.Add(controls[0]);
        parent.Controls.Add(controls[1]);
        parent.Controls.Add(controls[2]);
        parent.Controls.Add(controls[3]);
      }
    }
  }
}
