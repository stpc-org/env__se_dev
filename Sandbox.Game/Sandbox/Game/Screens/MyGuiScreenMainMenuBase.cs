// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenMainMenuBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public abstract class MyGuiScreenMainMenuBase : MyGuiScreenBase
  {
    protected const float TEXT_LINE_HEIGHT = 0.014f;
    protected const int INITIAL_TRANSITION_TIME = 1500;
    private MyGuiControlLabel m_warningLabel = new MyGuiControlLabel();
    protected bool m_pauseGame;
    protected bool m_musicPlayed;
    private static bool m_firstLoadup = true;
    private List<MyStringId> m_warningNotifications = new List<MyStringId>();
    private static readonly string BUILD_DATE = "Build: " + MySandboxGame.BuildDateTime.ToString("yyyy-MM-dd hh:mm", (IFormatProvider) CultureInfo.InvariantCulture);
    private static readonly StringBuilder APP_VERSION = MyFinalBuildConstants.APP_VERSION_STRING;
    private const string STEAM_INACTIVE = "SERVICE NOT AVAILABLE";
    private const string NOT_OBFUSCATED = "NOT OBFUSCATED";
    private const string NON_OFFICIAL = " NON-OFFICIAL";
    private const string PROFILING = " PROFILING";
    private static readonly StringBuilder PLATFORM = new StringBuilder(Environment.Is64BitProcess ? " 64-bit" : " 32-bit");
    private static StringBuilder BranchName = new StringBuilder(50);

    public bool DrawBuildInformation { get; set; }

    public override string GetFriendlyName() => "MyGuiScreenMainMenu";

    public override bool RegisterClicks() => true;

    public MyGuiScreenMainMenuBase(bool pauseGame = false)
      : base(new Vector2?(Vector2.Zero))
    {
      if (MyScreenManager.IsScreenOfTypeOpen(typeof (MyGuiScreenGamePlay)))
      {
        this.m_pauseGame = pauseGame;
        if (this.m_pauseGame && !Sync.MultiplayerActive)
          MySandboxGame.PausePush();
      }
      else
        this.m_closeOnEsc = false;
      this.m_drawEvenWithoutFocus = false;
      this.DrawBuildInformation = true;
    }

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      if (!this.m_musicPlayed)
      {
        if (MyGuiScreenGamePlay.Static == null)
          MyAudio.Static.PlayMusic(MyPerGameSettings.MainMenuTrack);
        this.m_musicPlayed = true;
      }
      if (MyReloadTestComponent.Enabled && this.State == MyGuiScreenState.OPENED)
        MyReloadTestComponent.DoReload();
      return true;
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      if (MySandboxGame.Config.EnablePerformanceWarnings)
      {
        if (!MyGameService.Service.GetInstallStatus(out int _))
        {
          if (!this.m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_InstallInProgress))
            this.m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_InstallInProgress);
        }
        else if (MySandboxGame.Config.ExperimentalMode && !this.m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode))
          this.m_warningNotifications.Add(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode);
      }
      MyGuiSandbox.DrawGameLogoHandler(this.m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
      this.DrawPerformanceWarning();
      if (this.DrawBuildInformation)
      {
        this.DrawObfuscationStatus();
        this.DrawSteamStatus();
        this.DrawAppVersion();
      }
      return true;
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (this.m_pauseGame && !Sync.MultiplayerActive)
        MySandboxGame.PausePop();
      int num = base.CloseScreen(isUnloading) ? 1 : 0;
      MyGuiScreenMainMenuBase.m_firstLoadup = false;
      this.m_musicPlayed = false;
      return num != 0;
    }

    public override void CloseScreenNow(bool isUnloading = false)
    {
      MyGuiScreenMainMenuBase.m_firstLoadup = false;
      base.CloseScreenNow(isUnloading);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELP_SCREEN) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
      {
        if (MyInput.Static.IsAnyShiftKeyPressed() && MyPerGameSettings.GUI.PerformanceWarningScreen != (Type) null)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
        }
        else
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
        }
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.WARNING_SCREEN) && MyPerGameSettings.GUI.PerformanceWarningScreen != (Type) null)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
        MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
      }
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.RecreateControls(true);
    }

    public override bool HideScreen()
    {
      MyGuiScreenMainMenuBase.m_firstLoadup = false;
      return base.HideScreen();
    }

    public override int GetTransitionOpeningTime() => MyGuiScreenMainMenuBase.m_firstLoadup ? 1500 : base.GetTransitionOpeningTime();

    private void DrawPerformanceWarning()
    {
      if (!this.Controls.Contains((MyGuiControlBase) this.m_warningLabel))
        this.Controls.Add((MyGuiControlBase) this.m_warningLabel);
      if (this.m_warningNotifications.Count == 0)
        return;
      Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 4, 42) - new Vector2(MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE.SizeGui.X / 1.5f, 0.0f);
      MyGuiPaddedTexture hudBgPerformance = MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE;
      MyGuiManager.DrawSpriteBatch(hudBgPerformance.Texture, normalizedCoord, hudBgPerformance.SizeGui / 1.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      StringBuilder stringBuilder = new StringBuilder();
      if (MyInput.Static.IsJoystickLastUsed)
        stringBuilder.AppendFormat(MyCommonTexts.PerformanceWarningCombinationGamepad, (object) MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_BASE, MyControlsSpace.WARNING_SCREEN));
      else
        stringBuilder.AppendFormat(MyCommonTexts.PerformanceWarningCombination, (object) MyGuiSandbox.GetKeyName(MyControlsSpace.HELP_SCREEN));
      MyGuiManager.DrawString("White", MyTexts.GetString(this.m_warningNotifications[0]), normalizedCoord + new Vector2(0.09f, -11f / 1000f), 0.7f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("White", stringBuilder.ToString(), normalizedCoord + new Vector2(0.09f, 0.018f), 0.6f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      stringBuilder.Clear();
      MyGuiManager.DrawString("White", stringBuilder.AppendFormat("({0})", (object) this.m_warningNotifications.Count).ToString(), normalizedCoord + new Vector2(0.177f, -23f / 1000f), 0.55f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.m_warningNotifications.RemoveAt(0);
    }

    private void DrawBuildDate()
    {
      Vector2 fullscreenGuiCoordinate = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
      fullscreenGuiCoordinate.Y -= 0.0f;
      MyGuiManager.DrawString("BuildInfo", MyGuiScreenMainMenuBase.BUILD_DATE, fullscreenGuiCoordinate, 0.6f, new Color?(new Color((Color) (MyGuiConstants.LABEL_TEXT_COLOR * this.m_transitionAlpha), 0.6f)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
    }

    private void DrawAppVersion()
    {
      Vector2 zero = Vector2.Zero;
      Vector2 fullscreenGuiCoordinate = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, 8, 8);
      if (!string.IsNullOrEmpty(MyGameService.BranchName))
      {
        MyGuiScreenMainMenuBase.BranchName.Clear();
        MyGuiScreenMainMenuBase.BranchName.Append(" ");
        MyGuiScreenMainMenuBase.BranchName.Append(MyGameService.BranchName);
        Vector2 vector2 = MyGuiManager.MeasureString("BuildInfoHighlight", MyGuiScreenMainMenuBase.BranchName, 0.6f);
        MyGuiManager.DrawString("BuildInfoHighlight", MyGuiScreenMainMenuBase.BranchName.ToString(), fullscreenGuiCoordinate, 0.6f, new Color?(new Color((Color) (MyGuiConstants.LABEL_TEXT_COLOR * this.m_transitionAlpha), 0.6f)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        fullscreenGuiCoordinate.X -= vector2.X;
      }
      MyGuiManager.DrawString("BuildInfo", MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString(), fullscreenGuiCoordinate, 0.6f, new Color?(new Color((Color) (MyGuiConstants.LABEL_TEXT_COLOR * this.m_transitionAlpha), 0.6f)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
    }

    private void DrawSteamStatus()
    {
      if (MyGameService.IsActive)
        return;
      Vector2 fullscreenGuiCoordinate = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
      fullscreenGuiCoordinate.Y -= 0.028f;
      MyGuiManager.DrawString("BuildInfo", "SERVICE NOT AVAILABLE".ToString(), fullscreenGuiCoordinate, 0.6f, new Color?(new Color((Color) (MyGuiConstants.LABEL_TEXT_COLOR * this.m_transitionAlpha), 0.6f)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
    }

    private void DrawObfuscationStatus()
    {
      if (!MyPerGameSettings.ShowObfuscationStatus || MyObfuscation.Enabled)
        return;
      Vector2 fullscreenGuiCoordinate = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
      fullscreenGuiCoordinate.Y -= 0.042f;
      MyGuiManager.DrawString("BuildInfoHighlight", "NOT OBFUSCATED", fullscreenGuiCoordinate, 0.6f, new Color?(new Color((Color) (MyGuiConstants.LABEL_TEXT_COLOR * this.m_transitionAlpha), 0.6f)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
    }

    protected MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      MyStringId? tooltip = null,
      MyStringId? gamepadHelpTextId = null)
    {
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(position), MyGuiControlButtonStyleEnum.StripeLeft, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(text), onButtonClick: onClick);
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(MyTexts.GetString(tooltip.Value));
      guiControlButton.BorderEnabled = false;
      guiControlButton.BorderSize = 0;
      guiControlButton.BorderHighlightEnabled = false;
      guiControlButton.BorderColor = Vector4.Zero;
      if (gamepadHelpTextId.HasValue)
        guiControlButton.GamepadHelpTextId = gamepadHelpTextId.Value;
      return guiControlButton;
    }

    protected void CheckLowMemSwitchToLow()
    {
      if (MySandboxGame.Config.LowMemSwitchToLow != MyConfig.LowMemSwitch.TRIGGERED)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.LowMemSwitchToLowQuestion), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result == MyGuiScreenMessageBox.ResultEnum.YES)
        {
          MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.ARMED;
          MySandboxGame.Config.SetToLowQuality();
          MySandboxGame.Config.Save();
          if (MySpaceAnalytics.Instance != null)
            MySpaceAnalytics.Instance.ReportSessionEnd("Exit to Windows");
          MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null, true);
          MySandboxGame.ExitThreadSafe();
        }
        else
        {
          MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.USER_SAID_NO;
          MySandboxGame.Config.Save();
        }
      }))));
    }

    public abstract void OpenUserRelatedScreens();

    public abstract void CloseUserRelatedScreens();
  }
}
