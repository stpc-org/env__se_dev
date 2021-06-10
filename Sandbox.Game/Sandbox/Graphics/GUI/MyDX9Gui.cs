// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyDX9Gui
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.AppCode;
using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
  public class MyDX9Gui : IMyGuiSandbox
  {
    private static MyGuiScreenDebugBase m_currentDebugScreen;
    private static bool m_lookAroundEnabled;
    private MyGuiScreenMessageBox m_currentModErrorsMessageBox;
    private MyGuiScreenDebugBase m_currentStatisticsScreen;
    private bool m_debugScreensEnabled = true;
    private StringBuilder m_debugText = new StringBuilder();
    private Vector2 m_oldVisPos;
    private Vector2 m_oldNonVisPos;
    private bool m_oldMouseVisibilityState;
    private bool m_cameraControllerMovementAllowed;
    private Vector2 m_gameLogoSize = new Vector2(0.43875f, 0.1975f) * 0.8f;
    private bool m_wasInputToNonFocusedScreens;
    private StringBuilder m_inputSharingText;
    private StringBuilder m_renderOverloadedText = new StringBuilder("WARNING: Render is overloaded, optimize your scene!");
    private bool m_shapeRenderingMessageBoxShown;
    private List<Type> m_pausingScreenTypes;
    internal List<MyDebugComponent> UserDebugInputComponents = new List<MyDebugComponent>();
    public string GameLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";

    public static bool LookaroundEnabled => MyDX9Gui.m_lookAroundEnabled;

    public bool IsDebugScreenEnabled() => this.m_debugScreensEnabled;

    public Action<float, Vector2> DrawGameLogoHandler { get; set; }

    public Vector2 MouseCursorPosition => MyGuiManager.GetNormalizedMousePosition(MyInput.Static.GetMousePosition(), MyInput.Static.GetMouseAreaSize());

    public MyDX9Gui()
    {
      MySandboxGame.Log.WriteLine("MyGuiManager()");
      this.DrawGameLogoHandler = new Action<float, Vector2>(this.DrawGameLogo);
      this.m_inputSharingText = !MyFakes.ALT_AS_DEBUG_KEY ? new StringBuilder("WARNING: Sharing input enabled (release Scroll Lock to disable it)") : new StringBuilder("WARNING: Sharing input enabled (release ALT to disable it)");
      MyGuiScreenBase.EnableSlowTransitionAnimations = MyFakes.ENABLE_SLOW_WINDOW_TRANSITION_ANIMATIONS;
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyGlobalInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyCharacterInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyPetaInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyVisualScriptingDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyRendererStatsComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyTestersInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyAsteroidsDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyPlanetsDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyRenderDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyComponentsDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyResearchDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyAIDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyMartinInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyTomasInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyOndraInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyHonzaInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyCestmirDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyAlexDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyMichalDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyAlesDebugInputComponent());
      this.UserDebugInputComponents.Add((MyDebugComponent) new MyTomasDDebugInputComponent());
      this.LoadDebugInputsFromConfig();
    }

    public void SetMouseCursorVisibility(bool visible, bool changePosition = true)
    {
      if (this.m_oldMouseVisibilityState && visible != this.m_oldMouseVisibilityState)
      {
        this.m_oldVisPos = MyInput.Static.GetMousePosition();
        this.m_oldMouseVisibilityState = visible;
      }
      if (!this.m_oldMouseVisibilityState && visible != this.m_oldMouseVisibilityState)
      {
        this.m_oldNonVisPos = MyInput.Static.GetMousePosition();
        this.m_oldMouseVisibilityState = visible;
        if (changePosition)
          MyInput.Static.SetMousePosition((int) this.m_oldVisPos.X, (int) this.m_oldVisPos.Y);
      }
      MySandboxGame.Static.SetMouseVisible(visible);
    }

    public void LoadData()
    {
      MyScreenManager.LoadData();
      MyGuiManager.LoadData();
      MyLanguage.CurrentLanguage = MySandboxGame.Config.Language;
      if (!MyFakes.SHOW_AUDIO_DEV_SCREEN)
        return;
      this.AddScreen((MyGuiScreenBase) new MyGuiScreenDebugAudio());
    }

    public void LoadContent()
    {
      MySandboxGame.Log.WriteLine("MyGuiManager.LoadContent() - START");
      MySandboxGame.Log.IncreaseIndent();
      MyGuiManager.SetMouseCursorTexture("Textures\\GUI\\MouseCursor.dds");
      MyGuiManager.LoadContent();
      MyGuiManager.CurrentLanguage = MySandboxGame.Config.Language;
      MyScreenManager.LoadContent();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiManager.LoadContent() - END");
    }

    public void UnloadContent() => MyScreenManager.UnloadContent();

    public void SwitchDebugScreensEnabled() => this.m_debugScreensEnabled = !this.m_debugScreensEnabled;

    public bool HandleRenderProfilerInput() => MyRenderProfiler.HandleInput();

    public void AddScreen(MyGuiScreenBase screen) => MyScreenManager.AddScreen(screen);

    public void InsertScreen(MyGuiScreenBase screen, int index) => MyScreenManager.InsertScreen(screen, index);

    public void RemoveScreen(MyGuiScreenBase screen) => MyScreenManager.RemoveScreen(screen);

    public void HandleInput()
    {
      try
      {
        if (MySandboxGame.Static.PauseInput || this.HandleRenderProfilerInput())
          return;
        MyTexts.SetGlobalVariantSelector(MyInput.Static.IsJoystickLastUsed ? MyTexts.GAMEPAD_VARIANT_ID : MyStringId.NullOrEmpty);
        if (MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.F4))
        {
          if (MySpaceAnalytics.Instance != null)
          {
            if (MySession.Static != null)
              MySpaceAnalytics.Instance.ReportWorldEnd();
            MySpaceAnalytics.Instance.ReportSessionEnd("Alt+F4");
          }
          MySandboxGame.ExitThreadSafe();
        }
        else
        {
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SCREENSHOT))
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            this.TakeScreenshot((string) null, false, new Vector2?(), true);
          }
          bool flag1 = MyInput.Static.IsNewKeyPressed(MyKeys.F12);
          if (MyInput.Static.IsNewKeyPressed(MyKeys.F2) | flag1 && MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyAltKeyPressed())
          {
            if (MySession.Static != null && MySession.Static.CreativeMode)
            {
              if (!flag1)
                return;
              MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS;
              if (this.m_shapeRenderingMessageBoxShown)
                return;
              this.m_shapeRenderingMessageBoxShown = true;
              this.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Enabled physics shapes rendering. This feature is for modders only and is not part of the gameplay."), messageCaption: new StringBuilder("PHYSICS SHAPES")));
            }
            else
              this.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Use of helper key combinations for modders is only allowed in creative mode."), messageCaption: new StringBuilder("MODDING HELPER KEYS")));
          }
          else
          {
            if (MyInput.Static.IsNewKeyPressed(MyKeys.H) && MyInput.Static.IsAnyCtrlKeyPressed())
              MyGeneralStats.ToggleProfiler();
            if (MyInput.Static.IsNewKeyPressed(MyKeys.F11) && MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
              this.SwitchTimingScreen();
            if (MyFakes.ENABLE_MISSION_SCREEN && MyInput.Static.IsNewKeyPressed(MyKeys.U))
              MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenMission());
            if (!MyFakes.ENABLE_F12_MENU && Sync.MultiplayerActive && MyDX9Gui.m_currentDebugScreen is MyGuiScreenDebugOfficial)
            {
              this.RemoveScreen((MyGuiScreenBase) MyDX9Gui.m_currentDebugScreen);
              MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) null;
            }
            bool flag2 = false;
            if (MySession.Static != null && MySession.Static.CreativeMode || MyFakes.ENABLE_F12_MENU)
              this.F12Handling();
            if (MyFakes.ENABLE_F12_MENU)
            {
              if (MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
                this.SwitchStatisticsScreen();
              if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyAltKeyPressed() && (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Home)))
                throw new InvalidOperationException("Controlled crash");
              if (MyInput.Static.IsNewKeyPressed(MyKeys.Pause) && MyInput.Static.IsAnyShiftKeyPressed())
                GC.Collect(GC.MaxGeneration);
              if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.F2))
              {
                if (MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed())
                  MyDefinitionManager.Static.ReloadParticles();
                else if (MyInput.Static.IsAnyShiftKeyPressed())
                {
                  MyDefinitionManager.Static.ReloadDecalMaterials();
                  MyRenderProxy.ReloadTextures();
                }
                else if (MyInput.Static.IsAnyAltKeyPressed())
                  MyRenderProxy.ReloadModels();
                else
                  MyRenderProxy.ReloadEffects();
              }
              flag2 = this.HandleDebugInput();
            }
            if (flag2)
              return;
            MyScreenManager.HandleInput();
          }
        }
      }
      finally
      {
      }
    }

    private void F12Handling()
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.F12))
      {
        if (MyFakes.ENABLE_F12_MENU)
          this.ShowDeveloperDebugScreen();
        else if (MyDX9Gui.m_currentDebugScreen is MyGuiScreenDebugDeveloper)
        {
          this.RemoveScreen((MyGuiScreenBase) MyDX9Gui.m_currentDebugScreen);
          MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) null;
        }
      }
      MyScreenManager.InputToNonFocusedScreens = !MyFakes.ALT_AS_DEBUG_KEY ? MyInput.Static.IsKeyPress(MyKeys.ScrollLock) && !MyInput.Static.IsKeyPress(MyKeys.Tab) : MyInput.Static.IsAnyAltKeyPressed() && !MyInput.Static.IsKeyPress(MyKeys.Tab);
      if (MyScreenManager.InputToNonFocusedScreens == this.m_wasInputToNonFocusedScreens)
        return;
      if (MyScreenManager.InputToNonFocusedScreens && MyDX9Gui.m_currentDebugScreen != null)
        this.SetMouseCursorVisibility(MyScreenManager.InputToNonFocusedScreens, true);
      this.m_wasInputToNonFocusedScreens = MyScreenManager.InputToNonFocusedScreens;
    }

    public static void SwitchModDebugScreen()
    {
      if (!MyFakes.ENABLE_F12_MENU && Sync.MultiplayerActive)
        return;
      if (MyDX9Gui.m_currentDebugScreen != null)
      {
        if (!(MyDX9Gui.m_currentDebugScreen is MyGuiScreenDebugOfficial))
          return;
        MyDX9Gui.m_currentDebugScreen.CloseScreen();
        MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) null;
      }
      else
        MyDX9Gui.ShowModDebugScreen();
    }

    private static void ShowModDebugScreen()
    {
      if (MyDX9Gui.m_currentDebugScreen == null)
      {
        MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) new MyGuiScreenDebugOfficial();
        MyScreenManager.AddScreen((MyGuiScreenBase) MyDX9Gui.m_currentDebugScreen);
        MyDX9Gui.m_currentDebugScreen.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) => MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) null);
      }
      else
      {
        if (!(MyDX9Gui.m_currentDebugScreen is MyGuiScreenDebugOfficial))
          return;
        MyDX9Gui.m_currentDebugScreen.RecreateControls(false);
      }
    }

    private void ShowModErrorsMessageBox()
    {
      ListReader<MyDefinitionErrors.Error> errors = MyDefinitionErrors.GetErrors();
      if (this.m_currentModErrorsMessageBox != null)
        this.RemoveScreen((MyGuiScreenBase) this.m_currentModErrorsMessageBox);
      StringBuilder messageText = new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxErrorModLoadingFailure));
      messageText.Append("\n");
      HashSet<string> stringSet = new HashSet<string>();
      foreach (MyDefinitionErrors.Error error in errors)
      {
        string modName = error.ModName;
        if (error.Severity == TErrorSeverity.Critical && !string.IsNullOrWhiteSpace(modName) && stringSet.Add(modName))
        {
          messageText.Append("\n");
          messageText.Append(modName);
        }
      }
      messageText.Append("\n");
      this.m_currentModErrorsMessageBox = MyGuiSandbox.CreateMessageBox(messageText: messageText, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
      Vector2 vector2 = this.m_currentModErrorsMessageBox.Size.Value;
      vector2.Y *= 1.5f;
      this.m_currentModErrorsMessageBox.Size = new Vector2?(vector2);
      this.m_currentModErrorsMessageBox.RecreateControls(false);
      this.AddScreen((MyGuiScreenBase) this.m_currentModErrorsMessageBox);
    }

    public void ShowModErrors()
    {
      if (MyFakes.ENABLE_F12_MENU || !Sync.MultiplayerActive)
        MyDX9Gui.ShowModDebugScreen();
      else
        this.ShowModErrorsMessageBox();
    }

    private void ShowDeveloperDebugScreen()
    {
      switch (MyDX9Gui.m_currentDebugScreen)
      {
        case MyGuiScreenDebugOfficial _:
          break;
        case MyGuiScreenDebugDeveloper _:
          break;
        case null:
          this.AddScreen((MyGuiScreenBase) (MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) new MyGuiScreenDebugDeveloper()));
          MyDX9Gui.m_currentDebugScreen.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) => MyDX9Gui.m_currentDebugScreen = (MyGuiScreenDebugBase) null);
          break;
        default:
          this.RemoveScreen((MyGuiScreenBase) MyDX9Gui.m_currentDebugScreen);
          goto case null;
      }
    }

    public void HandleInputAfterSimulation()
    {
      if (MySession.Static == null)
        return;
      bool flag1 = MyScreenManager.GetScreenWithFocus() == MyGuiScreenGamePlay.Static && MyGuiScreenGamePlay.Static != null && !MyScreenManager.InputToNonFocusedScreens;
      bool flag2 = MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) || MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.PrimaryLookaround;
      bool flag3 = MySession.Static.ControlledEntity != null && (!flag1 && this.m_cameraControllerMovementAllowed != flag1);
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      if (MySandboxGame.IsPaused && screenWithFocus is MyGuiScreenGamePlay && !MyScreenManager.InputToNonFocusedScreens)
      {
        if ((MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.SpectatorDelta || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.SpectatorFixed ? 1 : (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.SpectatorOrbit ? 1 : 0)) == 0)
          return;
      }
      else if (flag2)
      {
        if (flag1)
        {
          float roll = MyInput.Static.GetRoll();
          MySession.Static.CameraController.Rotate(MyInput.Static.IsJoystickLastUsed ? MyInput.Static.GetLookAroundRotation() : MyInput.Static.GetRotation(), roll);
          if (!MyDX9Gui.m_lookAroundEnabled & flag3)
            MySession.Static.ControlledEntity.MoveAndRotateStopped();
        }
        if (flag3)
          MySession.Static.CameraController.RotateStopped();
      }
      else if (MySession.Static.CameraController is MySpectatorCameraController && MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.ConstantDelta && flag1)
      {
        float roll = MyInput.Static.GetRoll();
        Vector2 rotation = MyInput.Static.GetRotation();
        Vector3 positionDelta = MyInput.Static.GetPositionDelta();
        MySpectatorCameraController.Static.MoveAndRotate(positionDelta, rotation, roll);
      }
      MyScreenManager.HandleInputAfterSimulation();
      if (flag3)
        MySession.Static.ControlledEntity.MoveAndRotateStopped();
      this.m_cameraControllerMovementAllowed = flag1;
      MyDX9Gui.m_lookAroundEnabled = flag2;
    }

    private void SwitchTimingScreen()
    {
      if (!(this.m_currentStatisticsScreen is MyGuiScreenDebugTiming))
      {
        if (this.m_currentStatisticsScreen != null)
          this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.AddScreen((MyGuiScreenBase) (this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) new MyGuiScreenDebugTiming()));
      }
      else if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.SimpleTimingStats)
        MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.ComplexTimingStats;
      else
        MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.MoveNext;
    }

    private void SwitchStatisticsScreen()
    {
      if (this.m_currentStatisticsScreen is MyGuiScreenDebugStatistics statisticsScreen)
      {
        if (statisticsScreen.Cycle())
          return;
        this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) null;
      }
      else
      {
        if (this.m_currentStatisticsScreen != null)
          this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.AddScreen((MyGuiScreenBase) (this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) new MyGuiScreenDebugStatistics()));
      }
    }

    private void SwitchInputScreen()
    {
      if (!(this.m_currentStatisticsScreen is MyGuiScreenDebugInput))
      {
        if (this.m_currentStatisticsScreen != null)
          this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.AddScreen((MyGuiScreenBase) (this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) new MyGuiScreenDebugInput()));
      }
      else
      {
        this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) null;
      }
    }

    public void Update(int totalTimeInMS)
    {
      MyScreenManager.Update(totalTimeInMS);
      MyScreenManager.GetScreenWithFocus();
      bool gameFocused = MySandboxGame.Static.IsActive && (MyExternalAppBase.Static == null && MyVRage.Platform.Windows.Window.IsActive || MyExternalAppBase.Static != null && !MyExternalAppBase.IsEditorActive);
      if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.Last)
      {
        this.RemoveScreen((MyGuiScreenBase) this.m_currentStatisticsScreen);
        this.m_currentStatisticsScreen = (MyGuiScreenDebugBase) null;
      }
      MyInput.Static.Update(gameFocused);
      MyGuiManager.Update(totalTimeInMS);
      MyGuiManager.MouseCursorPosition = this.MouseCursorPosition;
      MyGuiManager.TotalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
    }

    private void DrawMouseCursor(string mouseCursorTexture)
    {
      if (mouseCursorTexture == null)
        return;
      Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(new Vector2(64f), 1f);
      MyGuiManager.DrawSpriteBatch(mouseCursorTexture, this.MouseCursorPosition, normalizedSize, new Color(MyGuiConstants.MOUSE_CURSOR_COLOR), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, waitTillLoaded: false);
    }

    private string GetMouseOverTexture(MyGuiScreenBase screen)
    {
      if (screen != null)
      {
        MyGuiControlBase mouseOverControl = screen.GetMouseOverControl();
        if (mouseOverControl != null)
          return mouseOverControl.GetMouseCursorTexture() ?? MyGuiManager.GetMouseCursorTexture();
      }
      return MyGuiManager.GetMouseCursorTexture();
    }

    public void Draw()
    {
      MyScreenManager.Draw();
      this.m_debugText.Clear();
      if (MyFakes.ENABLE_F12_MENU && MySandboxGame.Config.DebugComponentsInfo != MyDebugComponent.MyDebugComponentInfoState.NoInfo)
      {
        float y = 0.0f;
        int index = 0;
        bool flag = false;
        MyDebugComponent.ResetFrame();
        foreach (MyDebugComponent debugInputComponent in this.UserDebugInputComponents)
        {
          if (debugInputComponent.Enabled)
          {
            if ((double) y == 0.0)
            {
              this.m_debugText.AppendLine("Debug input:");
              this.m_debugText.AppendLine();
              y += 0.063f;
            }
            this.m_debugText.ConcatFormat<string, int>("{0} (Ctrl + numPad{1})", this.UserDebugInputComponents[index].GetName(), index);
            this.m_debugText.AppendLine();
            y += 0.0265f;
            if (MySession.Static != null)
              debugInputComponent.DispatchUpdate();
            debugInputComponent.Draw();
            flag = true;
          }
          ++index;
        }
        if (flag)
        {
          MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\rectangle_dark_center.dds", new Vector2(MyGuiManager.GetMaxMouseCoord().X, 0.0f), new Vector2(MyGuiManager.MeasureString("White", this.m_debugText, 1f).X + 0.012f, y), new Color(0, 0, 0, 130), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          MyGuiManager.DrawString("White", this.m_debugText.ToString(), new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.01f, 0.0f), 1f, new Color?(Color.White), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
        }
      }
      bool flag1 = MyVideoSettingsManager.IsHardwareCursorUsed();
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      if (screenWithFocus != null && screenWithFocus.GetDrawMouseCursor() || MyScreenManager.InputToNonFocusedScreens && MyScreenManager.GetScreensCount() > 1)
      {
        this.SetMouseCursorVisibility(flag1 && !MyInput.Static.IsJoystickLastUsed, false);
        if (flag1 && !MyFakes.FORCE_SOFTWARE_MOUSE_DRAW)
          return;
        this.DrawMouseCursor(this.GetMouseOverTexture(screenWithFocus));
      }
      else
      {
        if (!flag1 || screenWithFocus == null)
          return;
        this.SetMouseCursorVisibility(screenWithFocus.GetDrawMouseCursor(), true);
      }
    }

    public void BackToIntroLogos(Action afterLogosAction)
    {
      MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null, true);
      uint introVideoId = MySandboxGame.Static.IntroVideoId;
      MyDX9Gui.LogoItem[] logoItemArray = new MyDX9Gui.LogoItem[3];
      MyDX9Gui.LogoItem logoItem1 = new MyDX9Gui.LogoItem();
      logoItem1.Screen = typeof (MyGuiScreenIntroVideo);
      ref MyDX9Gui.LogoItem local = ref logoItem1;
      string[] strArray;
      if (introVideoId != 0U)
        strArray = (string[]) null;
      else
        strArray = new string[1]{ "Videos\\KSH.wmv" };
      local.Args = strArray;
      logoItem1.Id = introVideoId;
      logoItemArray[0] = logoItem1;
      logoItem1 = new MyDX9Gui.LogoItem();
      logoItem1.Screen = typeof (MyGuiScreenLogo);
      logoItem1.Args = new string[1]
      {
        "Textures\\Logo\\vrage_logo_2_0_white.dds"
      };
      logoItemArray[1] = logoItem1;
      logoItem1 = new MyDX9Gui.LogoItem();
      logoItem1.Screen = typeof (MyGuiScreenLogo);
      logoItem1.Args = new string[1]
      {
        "Textures\\Logo\\se.dds"
      };
      logoItemArray[2] = logoItem1;
      MyGuiScreenBase previousScreen = (MyGuiScreenBase) null;
      foreach (MyDX9Gui.LogoItem logoItem2 in logoItemArray)
      {
        List<string> stringList;
        if (logoItem2.Args != null)
        {
          stringList = new List<string>();
          foreach (string path2 in logoItem2.Args)
          {
            if (MyFileSystem.FileExists(Path.Combine(MyFileSystem.ContentPath, path2)))
              stringList.Add(path2);
          }
          if (stringList.Count == 0)
            continue;
        }
        else
          stringList = (List<string>) null;
        Type screen = logoItem2.Screen;
        object[] objArray;
        if (stringList == null)
          objArray = new object[2]
          {
            null,
            (object) logoItem2.Id
          };
        else
          objArray = new object[2]
          {
            (object) stringList.ToArray(),
            (object) logoItem2.Id
          };
        MyGuiScreenBase instance = (MyGuiScreenBase) Activator.CreateInstance(screen, objArray);
        if (previousScreen != null)
          this.AddCloseHandler(previousScreen, instance, afterLogosAction);
        else
          this.AddScreen(instance);
        previousScreen = instance;
      }
      if (previousScreen != null)
        previousScreen.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) => afterLogosAction());
      else
        afterLogosAction();
    }

    private void AddCloseHandler(
      MyGuiScreenBase previousScreen,
      MyGuiScreenBase logoScreen,
      Action afterLogosAction)
    {
      previousScreen.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) =>
      {
        if (!screen.Cancelled)
          this.AddScreen(logoScreen);
        else
          afterLogosAction();
      });
    }

    public void BackToMainMenu() => MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));

    public float GetDefaultTextScaleWithLanguage() => 0.8f * MyGuiManager.LanguageTextScale;

    public void TakeScreenshot(
      int width,
      int height,
      string saveToPath = null,
      bool ignoreSprites = false,
      bool showNotification = true)
    {
      this.TakeScreenshot(saveToPath, ignoreSprites, new Vector2?(new Vector2((float) width, (float) height) / (Vector2) MySandboxGame.ScreenSize), showNotification);
    }

    public void TakeScreenshot(
      string saveToPath = null,
      bool ignoreSprites = false,
      Vector2? sizeMultiplier = null,
      bool showNotification = true)
    {
      if (!sizeMultiplier.HasValue)
        sizeMultiplier = new Vector2?(new Vector2(MySandboxGame.Config.ScreenshotSizeMultiplier));
      MyRenderProxy.TakeScreenshot(sizeMultiplier.Value, saveToPath, false, ignoreSprites, showNotification);
    }

    public Vector2 GetNormalizedCoordsAndPreserveOriginalSize(int width, int height) => new Vector2((float) width / (float) MySandboxGame.ScreenSize.X, (float) height / (float) MySandboxGame.ScreenSize.Y);

    public void DrawGameLogo(float transitionAlpha, Vector2 position)
    {
      Color color = Color.White * transitionAlpha;
      MyGuiManager.DrawSpriteBatch(this.GameLogoTexture, position, new Vector2(0.43875f, 0.1975f), color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
    }

    public void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
    {
      Color color = Color.White * transitionAlpha;
      MyGuiManager.DrawSpriteBatch(texture, position, size, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
    }

    private bool HandleDebugInput()
    {
      if (MyInput.Static.IsAnyCtrlKeyPressed())
      {
        int index1 = -1;
        for (int index2 = 0; index2 < 10; ++index2)
        {
          if (MyInput.Static.IsNewKeyPressed((MyKeys) (96 + index2)))
          {
            index1 = index2;
            if (MyInput.Static.IsAnyAltKeyPressed())
            {
              index1 += 10;
              break;
            }
            break;
          }
        }
        if (index1 > -1 && index1 < this.UserDebugInputComponents.Count)
        {
          MyDebugComponent debugInputComponent = this.UserDebugInputComponents[index1];
          debugInputComponent.Enabled = !debugInputComponent.Enabled;
          this.SaveDebugInputsToConfig();
          return false;
        }
      }
      bool flag = false;
      foreach (MyDebugComponent debugInputComponent in this.UserDebugInputComponents)
      {
        if (debugInputComponent.Enabled && !MyInput.Static.IsAnyAltKeyPressed())
          flag = debugInputComponent.HandleInput() | flag;
        if (flag)
          break;
      }
      return flag;
    }

    private void SaveDebugInputsToConfig()
    {
      MySandboxGame.Config.DebugInputComponents.Dictionary.Clear();
      SerializableDictionary<string, MyConfig.MyDebugInputData> debugInputComponents = MySandboxGame.Config.DebugInputComponents;
      foreach (MyDebugComponent debugInputComponent in this.UserDebugInputComponents)
      {
        string name = debugInputComponent.GetName();
        MyConfig.MyDebugInputData myDebugInputData;
        debugInputComponents.Dictionary.TryGetValue(name, out myDebugInputData);
        myDebugInputData.Enabled = debugInputComponent.Enabled;
        myDebugInputData.Data = debugInputComponent.InputData;
        debugInputComponents[name] = myDebugInputData;
      }
      MySandboxGame.Config.Save();
    }

    private void LoadDebugInputsFromConfig()
    {
      foreach (KeyValuePair<string, MyConfig.MyDebugInputData> keyValuePair in MySandboxGame.Config.DebugInputComponents.Dictionary)
      {
        for (int index = 0; index < this.UserDebugInputComponents.Count; ++index)
        {
          if (this.UserDebugInputComponents[index].GetName() == keyValuePair.Key)
          {
            this.UserDebugInputComponents[index].Enabled = keyValuePair.Value.Enabled;
            try
            {
              this.UserDebugInputComponents[index].InputData = keyValuePair.Value.Data;
            }
            catch
            {
            }
          }
        }
      }
    }

    private struct LogoItem
    {
      public Type Screen;
      public string[] Args;
      public uint Id;
    }
  }
}
