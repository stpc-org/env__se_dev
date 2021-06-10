// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenGamePlay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenGamePlay : MyGuiScreenBase
  {
    private bool audioSet;
    public static MyGuiScreenGamePlay Static;
    private int[] m_lastBeginShootTime;
    private static MyGuiScreenBase m_activeGameplayScreen;
    public static MyGuiScreenBase TmpGameplayScreenHolder;
    public static bool DisableInput;
    private IMyControlMenuInitializer m_controlMenu;
    private bool m_isAnselCameraInit;
    private bool m_reloadSessionNextFrame;
    private bool m_recording;
    private Task? m_drawComponentsTask;
    private Action m_drawComponentsAction;

    public static bool[] DoubleClickDetected { get; private set; }

    public static MyGuiScreenBase ActiveGameplayScreen
    {
      get => MyGuiScreenGamePlay.m_activeGameplayScreen;
      set => MyGuiScreenGamePlay.m_activeGameplayScreen = value;
    }

    public event Action OnHelmetChanged;

    public event Action OnHeadlightChanged;

    public bool CanSwitchCamera
    {
      get
      {
        if (!MyClipboardComponent.Static.Clipboard.AllowSwitchCameraMode || !MySession.Static.Settings.Enable3rdPersonView)
          return false;
        MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
        return cameraControllerEnum == MyCameraControllerEnum.Entity || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator;
      }
    }

    public TimeSpan SuppressMovement { get; set; }

    public static bool SpectatorEnabled
    {
      get
      {
        if (MySession.Static == null)
          return false;
        return MySession.Static.CreativeToolsEnabled(Sync.MyId) || !MySession.Static.SurvivalMode || MyMultiplayer.Static != null && MySession.Static.IsUserModerator(Sync.MyId) || MySession.Static.Settings.EnableSpectator;
      }
    }

    public bool MouseCursorVisible
    {
      get => this.DrawMouseCursor;
      set => this.DrawMouseCursor = value;
    }

    public bool LoadingDone { get; set; }

    public MyGuiScreenGamePlay()
      : base(new Vector2?(Vector2.Zero))
    {
      MyGuiScreenGamePlay.Static = this;
      this.DrawMouseCursor = false;
      this.m_closeOnEsc = false;
      this.m_drawEvenWithoutFocus = true;
      this.EnabledBackgroundFade = false;
      this.m_canShareInput = false;
      this.CanBeHidden = false;
      this.m_isAlwaysFirst = true;
      MyGuiScreenGamePlay.DisableInput = false;
      this.m_controlMenu = Activator.CreateInstance(MyPerGameSettings.ControlMenuInitializerType) as IMyControlMenuInitializer;
      MyGuiScreenToolbarConfigBase.ReinitializeBlockScrollbarPosition();
      this.m_lastBeginShootTime = new int[(int) (MyEnum<MyShootActionEnum>.Range.Max + (byte) 1)];
      MyGuiScreenGamePlay.DoubleClickDetected = new bool[this.m_lastBeginShootTime.Length];
    }

    public static void StartLoading(Action loadingAction, string backgroundOverride = null)
    {
      if (MySpaceAnalytics.Instance != null)
        MySpaceAnalytics.Instance.StoreWorldLoadingStartTime();
      MyGuiScreenGamePlay newGameplayScreen = new MyGuiScreenGamePlay();
      MyGuiScreenGamePlay guiScreenGamePlay = newGameplayScreen;
      guiScreenGamePlay.OnLoadingAction = guiScreenGamePlay.OnLoadingAction + loadingAction;
      MyGuiScreenLoading guiScreenLoading = new MyGuiScreenLoading((MyGuiScreenBase) newGameplayScreen, MyGuiScreenGamePlay.Static, backgroundOverride);
      guiScreenLoading.OnScreenLoadingFinished += (Action) (() =>
      {
        if (MySession.Static == null)
          return;
        MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
        newGameplayScreen.LoadingDone = true;
      });
      MyGuiSandbox.AddScreen((MyGuiScreenBase) guiScreenLoading);
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyScreenManager.EndOfDraw -= new Action(this.ScreenManagerOnEndOfDraw);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenGamePlay);

    public override void LoadData()
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadData - START");
      MySandboxGame.Log.IncreaseIndent();
      base.LoadData();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadData - END");
      MyScreenManager.EndOfDraw += new Action(this.ScreenManagerOnEndOfDraw);
      this.m_drawComponentsAction = (Action) (() =>
      {
        using (Stats.Generic.Measure("GamePrepareDraw"))
        {
          if (MySession.Static == null)
            return;
          MySession.Static.DrawAsync();
        }
      });
    }

    public override void LoadContent()
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadContent - START");
      MySandboxGame.Log.IncreaseIndent();
      MyGuiScreenGamePlay.Static = this;
      base.LoadContent();
      MySandboxGame.IsUpdateReady = true;
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.LoadContent - END");
    }

    public override void UnloadData()
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadData - START");
      MySandboxGame.Log.IncreaseIndent();
      base.UnloadData();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadData - END");
    }

    public override void UnloadContent()
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadContent - START");
      MySandboxGame.Log.IncreaseIndent();
      base.UnloadContent();
      GC.Collect();
      MyGuiScreenGamePlay.Static = (MyGuiScreenGamePlay) null;
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiScreenGamePlay.UnloadContent - END");
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      bool flag = false;
      if (!MyVRage.Platform.Ansel.IsInitializedSuccessfuly && MyInput.Static.IsKeyPress(MyKeys.F2) && MyInput.Static.IsKeyPress(MyKeys.Alt) && (!MyInput.Static.WasKeyPress(MyKeys.F2) || !MyInput.Static.WasKeyPress(MyKeys.Alt)))
      {
        if (MyVideoSettingsManager.IsCurrentAdapterNvidia())
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAnselWrongDriverOrCard), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
        else
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAnselNotNvidiaGpu), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
      }
      if (MyClipboardComponent.Static != null)
        flag = MyClipboardComponent.Static.HandleGameInput();
      if (!flag && MyCubeBuilder.Static != null)
        flag = MyCubeBuilder.Static.HandleGameInput();
      if (flag)
        return;
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public override void InputLost()
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.InputLost();
    }

    private static void SetAudioVolumes()
    {
      MyAudio.Static.StopMusic();
      MyAudio.Static.ChangeGlobalVolume(1f, 5f);
      if (MyPerGameSettings.UseMusicController && MyFakes.ENABLE_MUSIC_CONTROLLER && (MySandboxGame.Config.EnableDynamicMusic && !Sandbox.Engine.Platform.Game.IsDedicated) && MyMusicController.Static == null)
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
      MyAudio.Static.MusicAllowed = MyMusicController.Static == null;
      if (MyMusicController.Static != null)
        MyMusicController.Static.Active = true;
      else
        MyAudio.Static.PlayMusic();
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      IMyCameraController cameraController = MySession.Static.CameraController;
      MyStringId context = MySpaceBindingCreator.CX_SPECTATOR;
      if (!MySession.Static.IsCameraUserControlledSpectator())
        context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      MyStringId auxiliaryContext = controlledEntity != null ? controlledEntity.AuxiliaryContext : MySpaceBindingCreator.AX_BASE;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(context, MyControlsGUI.MAIN_MENU))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
        if (MySessionComponentReplay.Static.IsReplaying || MySessionComponentReplay.Static.IsRecording)
        {
          MySessionComponentReplay.Static.StopRecording();
          MySessionComponentReplay.Static.StopReplay();
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
        }
        else
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu, (object) !MySandboxGame.IsPaused));
      }
      if (MyGuiScreenGamePlay.DisableInput)
      {
        if (MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.CANCEL)) || MyControllerHelper.IsControl(context, MyControlsSpace.CUTSCENE_SKIPPER)))
          MyVisualScriptLogicProvider.SkipCutscene();
        MySession.Static.ControlledEntity?.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0.0f);
      }
      else
      {
        if (MySession.Static.LocalHumanPlayer != null && MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId))
        {
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NONE) || MyControllerHelper.IsControl(context, MyControlsSpace.SPECTATOR_PLAYER_CONTROL))
            MyGuiScreenGamePlay.SetSpectatorNone();
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_DELTA))
            MyGuiScreenGamePlay.SetSpectatorDelta();
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_FREE))
            MyGuiScreenGamePlay.SetSpectatorFree();
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_STATIC))
            MyGuiScreenGamePlay.SetSpectatorStatic();
          if ((MyInput.Static.IsNewKeyPressed(MyKeys.Space) && MyInput.Static.IsAnyCtrlKeyPressed() || MyControllerHelper.IsControl(context, MyControlsSpace.SPECTATOR_TELEPORT)) && (MySession.Static.CameraController == MySpectator.Static && MySession.Static != null && MySession.Static.IsUserSpaceMaster(Sync.MyId)))
            MyMultiplayer.TeleportControlledEntity(MySpectator.Static.Position);
          if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CONSOLE) && MyInput.Static.IsAnyAltKeyPressed())
            MyGuiScreenConsole.Show();
        }
        if (MyDefinitionErrors.ShouldShowModErrors)
        {
          MyDefinitionErrors.ShouldShowModErrors = false;
          MyGuiSandbox.ShowModErrors();
        }
        if ((MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CAMERA_MODE) || MyControllerHelper.IsControl(MyControllerHelper.CX_CHARACTER, MyControlsSpace.CAMERA_MODE)) && (!MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyAltKeyPressed()))
        {
          if (this.CanSwitchCamera)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
            this.SwitchCamera();
          }
          else if (MySession.Static.CameraController != null && MySession.Static.LocalHumanPlayer != null && (MySession.Static.LocalHumanPlayer.Character != null && MySession.Static.ControlledEntity != null))
            MySession.Static.LocalHumanPlayer.Character.ResetHeadRotation();
        }
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELP_SCREEN))
        {
          if (MyInput.Static.IsAnyCtrlKeyPressed())
          {
            switch (MySandboxGame.Config.DebugComponentsInfo)
            {
              case MyDebugComponent.MyDebugComponentInfoState.NoInfo:
                MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.EnabledInfo;
                break;
              case MyDebugComponent.MyDebugComponentInfoState.EnabledInfo:
                MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.FullInfo;
                break;
              case MyDebugComponent.MyDebugComponentInfoState.FullInfo:
                MySandboxGame.Config.DebugComponentsInfo = MyDebugComponent.MyDebugComponentInfoState.NoInfo;
                break;
            }
            MySandboxGame.Config.Save();
          }
          else if (MyInput.Static.IsAnyShiftKeyPressed() && MyPerGameSettings.GUI.PerformanceWarningScreen != (Type) null)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
          }
          else if (MyGuiScreenGamePlay.ActiveGameplayScreen == null)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
          }
        }
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.WARNING_SCREEN) && MyPerGameSettings.GUI.PerformanceWarningScreen != (Type) null)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PerformanceWarningScreen));
        }
        if (MyPerGameSettings.SimplePlayerNames && MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BROADCASTING))
          MyHud.LocationMarkers.Visible = !MyHud.LocationMarkers.Visible;
        bool flag = false;
        if (MySession.Static.ControlledEntity is IMyUseObject)
          flag = (MySession.Static.ControlledEntity as IMyUseObject).HandleInput();
        if (controlledEntity != null && !flag)
        {
          if (!MySandboxGame.IsPaused)
          {
            if (MyFakes.ENABLE_NON_PUBLIC_GUI_ELEMENTS && MyInput.Static.IsNewKeyPressed(MyKeys.F2) && (MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed()) && !MyInput.Static.IsAnyAltKeyPressed())
              MySession.Static.Settings.GameMode = MySession.Static.Settings.GameMode != MyGameModeEnum.Creative ? MyGameModeEnum.Creative : MyGameModeEnum.Survival;
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PRIMARY_TOOL_ACTION, useXinput: true))
            {
              if (MyToolbarComponent.CurrentToolbar.ShouldActivateSlot)
              {
                MyToolbarComponent.CurrentToolbar.ActivateStagedSelectedItem();
              }
              else
              {
                if (context != MySpaceBindingCreator.CX_SPACESHIP)
                {
                  if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastBeginShootTime[0]) < 500.0)
                  {
                    MyGuiScreenGamePlay.DoubleClickDetected[0] = true;
                  }
                  else
                  {
                    MyGuiScreenGamePlay.DoubleClickDetected[0] = false;
                    this.m_lastBeginShootTime[0] = MySandboxGame.TotalGamePlayTimeInMilliseconds;
                  }
                }
                controlledEntity.BeginShoot(MyShootActionEnum.PrimaryAction);
              }
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED, useXinput: true))
            {
              if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastBeginShootTime[0]) > 500.0)
                MyGuiScreenGamePlay.DoubleClickDetected[0] = false;
              controlledEntity.EndShoot(MyShootActionEnum.PrimaryAction);
              MyGuiScreenGamePlay.DoubleClickDetected[0] = false;
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.SECONDARY_TOOL_ACTION, useXinput: true))
            {
              if (context != MySpaceBindingCreator.CX_SPACESHIP)
              {
                if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastBeginShootTime[1]) < 500.0)
                {
                  MyGuiScreenGamePlay.DoubleClickDetected[1] = true;
                }
                else
                {
                  MyGuiScreenGamePlay.DoubleClickDetected[1] = false;
                  this.m_lastBeginShootTime[1] = MySandboxGame.TotalGamePlayTimeInMilliseconds;
                }
              }
              controlledEntity.BeginShoot(MyShootActionEnum.SecondaryAction);
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED, useXinput: true))
            {
              if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastBeginShootTime[1]) > 500.0)
                MyGuiScreenGamePlay.DoubleClickDetected[1] = false;
              controlledEntity.EndShoot(MyShootActionEnum.SecondaryAction);
              if (MySandboxGame.Config.IronSightSwitchState == IronSightSwitchStateType.Hold && controlledEntity is MyCharacter myCharacter && myCharacter.ZoomMode == MyZoomModeEnum.IronSight)
              {
                myCharacter.Shoot(MyShootActionEnum.SecondaryAction, Vector3.Forward);
                controlledEntity.EndShoot(MyShootActionEnum.SecondaryAction);
              }
              MyGuiScreenGamePlay.DoubleClickDetected[1] = false;
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.ACTIVE_CONTRACT_SCREEN))
            {
              MyContractsActiveViewModel contractsActiveViewModel = new MyContractsActiveViewModel();
              ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) contractsActiveViewModel);
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.CHAT_SCREEN) && !VRage.Profiler.MyRenderProfiler.ProfilerVisible)
            {
              Vector2 hudPos = new Vector2(0.029f, 0.8f);
              hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
              MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenChat(hudPos));
            }
            if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.TOGGLE_HUD) && MyInput.Static.IsJoystickLastUsed)
              MyHud.ToggleGamepadHud();
            if (context == MySpaceBindingCreator.CX_CHARACTER || context == MySpaceBindingCreator.CX_JETPACK || (context == MySpaceBindingCreator.CX_SPACESHIP || context == MySpaceBindingCreator.CX_SPECTATOR))
            {
              if (MyControllerHelper.IsControl(context, MyControlsSpace.USE) && context != MySpaceBindingCreator.CX_SPECTATOR)
              {
                if (cameraController != null)
                {
                  if (!cameraController.HandleUse())
                  {
                    this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.UseData = new UseData?(new UseData()
                    {
                      Use = true
                    })));
                    controlledEntity.Use();
                  }
                }
                else
                {
                  controlledEntity.Use();
                  this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.UseData = new UseData?(new UseData()
                  {
                    Use = true
                  })));
                }
              }
              else if (MyControllerHelper.IsControl(context, MyControlsSpace.USE, MyControlStateType.PRESSED))
              {
                controlledEntity.UseContinues();
                this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.UseData = new UseData?(new UseData()
                {
                  UseContinues = true
                })));
              }
              else if (MyControllerHelper.IsControl(context, MyControlsSpace.USE, MyControlStateType.NEW_RELEASED) && context != MySpaceBindingCreator.CX_SPECTATOR)
              {
                controlledEntity.UseFinished();
                this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.UseData = new UseData?(new UseData()
                {
                  UseFinished = true
                })));
              }
              if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP))
              {
                if (cameraController != null)
                {
                  if (!cameraController.HandlePickUp())
                    controlledEntity.PickUp();
                }
                else
                  controlledEntity.PickUp();
              }
              else if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP, MyControlStateType.PRESSED))
                controlledEntity.PickUpContinues();
              else if (MyControllerHelper.IsControl(context, MyControlsSpace.PICK_UP, MyControlStateType.NEW_RELEASED))
                controlledEntity.PickUpFinished();
              if (!MySession.Static.IsCameraUserControlledSpectator())
              {
                string str = MyInput.Static.GetGameControl(MyControlsSpace.CROUCH) != null ? MyInput.Static.GetGameControl(MyControlsSpace.CROUCH).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : (string) null;
                if (!MyInput.Static.IsAnyCtrlKeyPressed() || str == "LCtrl")
                {
                  if (MyControllerHelper.IsControl(context, MyControlsSpace.CROUCH))
                    controlledEntity.Crouch();
                  if (MyControllerHelper.IsControl(context, MyControlsSpace.CROUCH, MyControlStateType.PRESSED))
                    controlledEntity.Down();
                }
                controlledEntity.Sprint(MyControllerHelper.IsControl(context, MyControlsSpace.SPRINT, MyControlStateType.PRESSED));
                if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP))
                  controlledEntity.Jump(MyInput.Static.GetPositionDelta());
                if (MyControllerHelper.IsControl(context, MyControlsSpace.JUMP, MyControlStateType.PRESSED))
                  controlledEntity.Up();
                if (controlledEntity is MyShipController myShipController)
                {
                  myShipController.WheelJump(MyControllerHelper.IsControl(context, MyControlsSpace.WHEEL_JUMP, MyControlStateType.PRESSED));
                  bool? brake = myShipController.CubeGrid?.GridSystems?.WheelSystem?.Brake;
                  bool enable = MyControllerHelper.IsControl(context, MyControlsSpace.JUMP, MyControlStateType.PRESSED);
                  if (brake.HasValue && brake.Value != enable)
                    myShipController.TryEnableBrakes(enable);
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.SWITCH_WALK))
                  controlledEntity.SwitchWalk();
                if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BROADCASTING))
                {
                  controlledEntity.SwitchBroadcasting();
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.HELMET))
                {
                  controlledEntity.SwitchHelmet();
                  this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                  {
                    SwitchHelmet = true
                  })));
                  this.OnHelmetChanged.InvokeIfNotNull();
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.DAMPING_RELATIVE))
                {
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  if (!controlledEntity.EnabledDamping)
                    controlledEntity.SwitchDamping();
                  MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.SetDampeningEntity)), controlledEntity.Entity.EntityId);
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.DAMPING))
                {
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  if (MyInput.Static.IsAnyCtrlKeyPressed())
                  {
                    if (!controlledEntity.EnabledDamping)
                      controlledEntity.SwitchDamping();
                    MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.SetDampeningEntity)), controlledEntity.Entity.EntityId);
                  }
                  else
                  {
                    controlledEntity.SwitchDamping();
                    MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyPlayerCollection.ClearDampeningEntity)), controlledEntity.Entity.EntityId);
                  }
                  this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                  {
                    SwitchDamping = true
                  })));
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.THRUSTS))
                {
                  if (!(controlledEntity is MyCharacter))
                    MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  if (!MyInput.Static.IsAnyCtrlKeyPressed())
                  {
                    controlledEntity.SwitchThrusts();
                    this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                    {
                      SwitchThrusts = true
                    })));
                  }
                }
                if (MyControllerHelper.IsControl(context, MyControlsSpace.CONSUME_HEALTH))
                  this.ConsumeHealthItem();
                if (MyControllerHelper.IsControl(context, MyControlsSpace.CONSUME_ENERGY))
                  this.ConsumeEnergyItem();
                if (MyControllerHelper.IsControl(context, MyControlsSpace.COLOR_TOOL))
                  this.EquipColorTool();
              }
              if (MyControllerHelper.IsControl(context, MyControlsSpace.HEADLIGHTS))
              {
                if (MySession.Static.IsCameraUserControlledSpectator())
                {
                  MySpectatorCameraController.Static.SwitchLight();
                }
                else
                {
                  MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                  controlledEntity.SwitchLights();
                  this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                  {
                    SwitchLights = true
                  })));
                }
                this.OnHeadlightChanged.InvokeIfNotNull();
              }
              if (MyControllerHelper.IsControl(context, MyControlsSpace.TOGGLE_REACTORS))
              {
                MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
                controlledEntity.SwitchReactors();
                this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                {
                  SwitchReactors = true
                })));
              }
              if (MyControllerHelper.IsControl(context, MyControlsSpace.LANDING_GEAR))
              {
                controlledEntity.SwitchLandingGears();
                this.MakeRecord(controlledEntity, (MySessionComponentReplay.ActionRef<PerFrameData>) ((ref PerFrameData x) => x.ControlSwitchesData = new ControlSwitchesData?(new ControlSwitchesData()
                {
                  SwitchLandingGears = true
                })));
              }
              if (MyControllerHelper.IsControl(context, MyControlsSpace.SUICIDE))
              {
                MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
                if (component != null && component.CustomRespawnEnabled)
                {
                  if (MySession.Static.ControlledEntity != null)
                    MyVisualScriptLogicProvider.CustomRespawnRequest(controlledEntity.ControllerInfo.ControllingIdentityId);
                }
                else
                  controlledEntity.Die();
              }
              if (controlledEntity is MyCockpit && MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_COLOR_CHANGE))
                (controlledEntity as MyCockpit).SwitchWeaponMode();
              HandleRadialToolbarInput();
              HandleRadialSystemMenuInput();
              HandleRadialVoxelHandInput();
            }
            this.BuildPlannerControls(context);
          }
          else if (controlledEntity.ShouldEndShootingOnPause(MyShootActionEnum.PrimaryAction) && controlledEntity.ShouldEndShootingOnPause(MyShootActionEnum.SecondaryAction))
          {
            controlledEntity.EndShoot(MyShootActionEnum.PrimaryAction);
            controlledEntity.EndShoot(MyShootActionEnum.SecondaryAction);
          }
          if (!MySandboxGame.IsPaused)
          {
            MySession.Static.GetComponent<MyEmoteSwitcher>();
            if (MyControllerHelper.IsControl(context, MyControlsSpace.TERMINAL) && MyGuiScreenGamePlay.ActiveGameplayScreen == null)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
              controlledEntity.ShowTerminal();
            }
            if (MyControllerHelper.IsControl(context, MyControlsSpace.INVENTORY) && MyGuiScreenGamePlay.ActiveGameplayScreen == null)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
              controlledEntity.ShowInventory();
            }
            if (MyControllerHelper.IsControl(context, MyControlsSpace.CONTROL_MENU) && MyGuiScreenGamePlay.ActiveGameplayScreen == null)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
              this.m_controlMenu.OpenControlMenu(controlledEntity);
            }
          }
        }
        if (!MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION && MyPerGameSettings.VoiceChatEnabled && MyVoiceChatSessionComponent.Static != null)
        {
          if (MyControllerHelper.IsControl(context, MyControlsSpace.VOICE_CHAT))
            MyVoiceChatSessionComponent.Static.StartRecording();
          else if (MyVoiceChatSessionComponent.Static.IsRecording && !MyControllerHelper.IsControl(context, MyControlsSpace.VOICE_CHAT, MyControlStateType.PRESSED))
            MyVoiceChatSessionComponent.Static.StopRecording();
        }
        this.MoveAndRotatePlayerOrCamera();
        if (MyInput.Static.IsNewKeyPressed(MyKeys.F5) || this.m_reloadSessionNextFrame || MyInput.Static.IsJoystickLastUsed && MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J05) && MyInput.Static.IsJoystickButtonPressed(MyJoystickButtonsEnum.J06))
        {
          this.m_reloadSessionNextFrame = false;
          if (MySession.Static.Settings.EnableSaving)
          {
            string currentPath = MySession.Static.CurrentPath;
            if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsJoystickLastUsed && MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J03))
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
              if (Sync.IsServer)
              {
                if (!MyAsyncSaving.InProgress)
                {
                  MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToQuickSave), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
                  {
                    if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
                      return;
                    MyAsyncSaving.Start((Action) (() => MySector.ResetEyeAdaptation = true));
                  })));
                  messageBox.SkipTransition = true;
                  messageBox.CloseBeforeCallback = true;
                  MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
                }
              }
              else
                MyHud.Notifications.Add(MyNotificationSingletons.ClientCannotSave);
            }
            else if (Sync.IsServer && !MyInput.Static.IsJoystickLastUsed || Sync.IsServer && MyInput.Static.IsJoystickLastUsed && MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J04))
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
              if (MyAsyncSaving.InProgress)
              {
                MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
                messageBox.SkipTransition = true;
                messageBox.InstantClose = false;
                MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
              }
              else
              {
                if (!Directory.Exists(currentPath))
                  return;
                this.ShowLoadMessageBox(currentPath);
              }
            }
            else if (!MyInput.Static.IsJoystickLastUsed)
            {
              MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
              this.ShowReconnectMessageBox();
            }
          }
          else
            MyHud.Notifications.Add(MyNotificationSingletons.CannotSave);
        }
        if (MyInput.Static.IsNewKeyPressed(MyKeys.F3))
        {
          if (Sync.MultiplayerActive)
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
          }
          else
            MyHud.Notifications.Add(MyNotificationSingletons.MultiplayerDisabled);
        }
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.FACTIONS_MENU) && !MyInput.Static.IsAnyCtrlKeyPressed())
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyScreenManager.AddScreenNow(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.FactionScreen));
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ACTIVE_CONTRACT_SCREEN) && MyGuiScreenGamePlay.ActiveGameplayScreen == null)
        {
          MyContractsActiveViewModel contractsActiveViewModel = new MyContractsActiveViewModel();
          ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) contractsActiveViewModel);
        }
        if ((MyInput.Static.IsKeyPress(MyKeys.LeftWindows) ? 1 : (MyInput.Static.IsKeyPress(MyKeys.RightWindows) ? 1 : 0)) == 0 && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.BUILD_SCREEN) && (!MyInput.Static.IsAnyCtrlKeyPressed() && MyGuiScreenGamePlay.ActiveGameplayScreen == null) && (MyPerGameSettings.GUI.EnableToolbarConfigScreen && MyGuiScreenToolbarConfigBase.Static == null && (MySession.Static.ControlledEntity is MyShipController || MySession.Static.ControlledEntity is MyCharacter)))
        {
          int num = 0;
          if (MyInput.Static.IsAnyShiftKeyPressed())
            num += 6;
          if (MyInput.Static.IsAnyCtrlKeyPressed())
            num += 12;
          MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
          MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) num, (object) (MySession.Static.ControlledEntity as MyShipController), null));
        }
        if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.VOXEL_SELECT_SPHERE))
        {
          MySessionComponentVoxelHand.Static.EquipVoxelHand("Sphere");
          MyCubeBuilder.Static.Deactivate();
        }
        if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.PROGRESSION_MENU))
          MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) (MySession.Static.ControlledEntity as MyShipController), (object) "ResearchPage", (object) true, null));
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME) && Sync.Clients.Count < 2)
          MySandboxGame.PauseToggle();
        if (MySession.Static != null)
        {
          if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.ADMIN_MENU))
          {
            if (MySession.Static.IsAdminMenuEnabled && MyPerGameSettings.Game != GameEnum.UNKNOWN_GAME)
              MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
            else
              MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
          }
          if (MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.BLUEPRINTS_MENU))
            MyBlueprintUtils.OpenBlueprintScreen();
          if (MyInput.Static.IsNewKeyPressed(MyKeys.F10))
          {
            if (MyInput.Static.IsAnyAltKeyPressed())
            {
              if (MySession.Static.IsAdminMenuEnabled && MyPerGameSettings.Game != GameEnum.UNKNOWN_GAME)
                MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
              else
                MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
            }
            else if (MyPerGameSettings.GUI.VoxelMapEditingScreen != (Type) null && (MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode) && MyInput.Static.IsAnyShiftKeyPressed())
              MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.VoxelMapEditingScreen));
            else
              MyBlueprintUtils.OpenBlueprintScreen();
          }
        }
        if (!MyInput.Static.IsNewKeyPressed(MyKeys.F11) || MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyCtrlKeyPressed())
          return;
        MyDX9Gui.SwitchModDebugScreen();
      }

      bool HandleRadialToolbarInput()
      {
        if (!MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.TOOLBAR_RADIAL_MENU))
          return false;
        // ISSUE: method pointer
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiControlRadialMenuBlock(MyDefinitionManager.Static.GetRadialMenuDefinition("Toolbar"), MyControlsGUI.LEFT_STICK_BUTTON, new Func<bool>((object) this, __methodptr(\u003CHandleUnhandledInput\u003Eg__HandleRadialSystemMenuInput\u007C8))));
        return true;
      }

      bool HandleRadialSystemMenuInput()
      {
        if (!MyControllerHelper.IsControl(context, MyControlsSpace.SYSTEM_RADIAL_MENU))
          return false;
        // ISSUE: method pointer
        MySession.Static.GetComponent<MyRadialMenuComponent>().ShowSystemRadialMenu(auxiliaryContext, new Func<bool>((object) this, __methodptr(\u003CHandleUnhandledInput\u003Eg__HandleRadialToolbarInput\u007C7)));
        return true;
      }

      bool HandleRadialVoxelHandInput()
      {
        if (!MyControllerHelper.IsControl(auxiliaryContext, MyControlsSpace.VOXEL_MATERIAL_SELECT))
          return false;
        // ISSUE: method pointer
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiControlRadialMenuVoxel(MyDefinitionManager.Static.GetRadialMenuDefinition("VoxelHand"), MyControlsSpace.VOXEL_MATERIAL_SELECT, new Func<bool>((object) this, __methodptr(\u003CHandleUnhandledInput\u003Eg__HandleRadialSystemMenuInput\u007C8))));
        return true;
      }
    }

    public static void SetSpectatorStatic()
    {
      if (MySession.Static.ControlledEntity == null)
        return;
      MySpectatorCameraController.Static.TurnLightOff();
      MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed, (IMyEntity) null, new Vector3D?());
      if (!MyInput.Static.IsAnyCtrlKeyPressed())
        return;
      MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
      MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), new Vector3D?(MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up));
    }

    public static void SetSpectatorFree()
    {
      if (!MyGuiScreenGamePlay.SpectatorEnabled)
        return;
      if (MyInput.Static.IsAnyShiftKeyPressed())
      {
        MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorOrbit, (IMyEntity) null, new Vector3D?());
        MySpectatorCameraController.Static.Reset();
      }
      else
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER, MyControlStateType.PRESSED) || MySession.Static.ControlledEntity == null)
        return;
      MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
      MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), new Vector3D?(MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up));
    }

    public static void SetSpectatorDelta()
    {
      if (MyGuiScreenGamePlay.SpectatorEnabled)
      {
        MySpectatorCameraController.Static.TurnLightOff();
        MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorDelta, (IMyEntity) null, new Vector3D?());
      }
      if (MySession.Static.ControlledEntity != null)
      {
        MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
        MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), new Vector3D?(MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up));
        MySpectatorCameraController.Static.TrackedEntity = MySession.Static.ControlledEntity.Entity.EntityId;
      }
      else
      {
        MyEntity targetEntity = MyCubeGrid.GetTargetEntity();
        if (targetEntity == null)
          return;
        MySpectator.Static.Position = targetEntity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
        MySpectator.Static.SetTarget(targetEntity.PositionComp.GetPosition(), new Vector3D?(targetEntity.PositionComp.WorldMatrixRef.Up));
        MySpectatorCameraController.Static.TrackedEntity = targetEntity.EntityId;
      }
    }

    public static void SetSpectatorNone()
    {
      if (MySession.Static.ControlledEntity == null)
        return;
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
          Sandbox.Game.Entities.IMyControllableEntity entity1 = (Sandbox.Game.Entities.IMyControllableEntity) null;
          for (int index = 0; index < myEntityList2.Count; ++index)
          {
            if (myEntityList2[index] is Sandbox.Game.Entities.IMyControllableEntity)
            {
              entity1 = myEntityList2[index] as Sandbox.Game.Entities.IMyControllableEntity;
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
        default:
          MyGuiScreenGamePlay.SetCameraController();
          break;
      }
      if (MySession.Static.ControlledEntity is MyCharacter)
        return;
      MySession.Static.GameFocusManager.Clear();
    }

    private void EquipColorTool() => MyCubeBuilder.Static.ActivateColorTool();

    public void BuildPlannerControls(MyStringId context)
    {
      if (MySession.Static == null || MySession.Static.LocalCharacter == null)
        return;
      MyCharacterDetectorComponent detectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent == null || detectorComponent.UseObject == null || !detectorComponent.UseObject.SupportedActions.HasFlag((Enum) UseActionEnum.BuildPlanner))
        return;
      bool flag = MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER);
      if (!(detectorComponent.UseObject is MyUseObjectBase useObject) || useObject.PrimaryAction != UseActionEnum.OpenInventory && useObject.SecondaryAction != UseActionEnum.OpenInventory)
        return;
      if (useObject.Owner is MyCubeBlock owner && !owner.GetUserRelationToOwner(MySession.Static.LocalCharacter.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
      {
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
      }
      else
      {
        MyGuiScreenGamePlay.MyBuildPlannerAction buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.None;
        if (MyInput.Static.IsJoystickLastUsed)
        {
          if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS))
            buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.AddProduction1;
          else if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS))
            buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.Withdraw1;
          else if (MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE))
            buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.DepositOre;
        }
        else if (flag)
        {
          buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.Withdraw1;
          if (MyInput.Static.IsAnyShiftKeyPressed())
            buildPlannerAction = !MyInput.Static.IsAnyCtrlKeyPressed() ? MyGuiScreenGamePlay.MyBuildPlannerAction.AddProduction1 : MyGuiScreenGamePlay.MyBuildPlannerAction.AddProduction10;
          else if (MyInput.Static.IsAnyCtrlKeyPressed())
            buildPlannerAction = !MyInput.Static.IsAnyAltKeyPressed() ? MyGuiScreenGamePlay.MyBuildPlannerAction.WithdrawKeep10 : MyGuiScreenGamePlay.MyBuildPlannerAction.WithdrawKeep1;
          else if (MyInput.Static.IsAnyAltKeyPressed())
            buildPlannerAction = MyGuiScreenGamePlay.MyBuildPlannerAction.DepositOre;
        }
        switch (buildPlannerAction)
        {
          case MyGuiScreenGamePlay.MyBuildPlannerAction.Withdraw1:
            if (MyCubeBuilder.Static.ToolbarBlockDefinition != null && MySession.Static.LocalCharacter.BuildPlanner.Count == 0 && MySession.Static.LocalCharacter.AddToBuildPlanner(MyCubeBuilder.Static.CurrentBlockDefinition))
            {
              MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerComponentsAdded);
              MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
            }
            MyGuiScreenGamePlay.ProcessWithdraw((MyEntity) useObject.Owner, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), new int?());
            break;
          case MyGuiScreenGamePlay.MyBuildPlannerAction.WithdrawKeep1:
            MyGuiScreenGamePlay.ProcessWithdraw((MyEntity) useObject.Owner, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), new int?(1));
            break;
          case MyGuiScreenGamePlay.MyBuildPlannerAction.WithdrawKeep10:
            MyGuiScreenGamePlay.ProcessWithdraw((MyEntity) useObject.Owner, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), new int?(10));
            break;
          case MyGuiScreenGamePlay.MyBuildPlannerAction.AddProduction1:
            if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
            {
              int production = MyTerminalInventoryController.AddComponentsToProduction((MyEntity) useObject.Owner, new int?());
              if (production > 0)
              {
                MyHud.Notifications.Get(MyNotificationSingletons.PutToProductionFailed)?.SetTextFormatArguments((object) production);
                MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed);
                break;
              }
              MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
              break;
            }
            MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
            break;
          case MyGuiScreenGamePlay.MyBuildPlannerAction.AddProduction10:
            if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
            {
              int production = MyTerminalInventoryController.AddComponentsToProduction((MyEntity) useObject.Owner, new int?(10));
              if (production > 0)
              {
                MyHud.Notifications.Get(MyNotificationSingletons.PutToProductionFailed)?.SetTextFormatArguments((object) production);
                MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionFailed);
                break;
              }
              MyHud.Notifications.Add(MyNotificationSingletons.PutToProductionSuccessful);
              break;
            }
            MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
            break;
          case MyGuiScreenGamePlay.MyBuildPlannerAction.DepositOre:
            int num = MyTerminalInventoryController.DepositAll(MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), (MyEntity) useObject.Owner);
            if (num > 0)
            {
              MyHud.Notifications.Get(MyNotificationSingletons.DepositFailed)?.SetTextFormatArguments((object) num);
              MyHud.Notifications.Add(MyNotificationSingletons.DepositFailed);
              break;
            }
            MyHud.Notifications.Add(MyNotificationSingletons.DepositSuccessful);
            break;
        }
      }
    }

    private void CreateModIoConsentScreen(Action onConsentAgree = null, Action onConsentOptOut = null)
    {
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
    }

    private void ConsumeEnergyItem()
    {
      MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ConsumableItem), "Powerkit");
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyInventory inventory;
      if (localCharacter == null || !localCharacter.TryGetInventory(out inventory))
        return;
      this.ConsumeItem(inventory, id);
    }

    private void ConsumeHealthItem()
    {
      MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ConsumableItem), "Medkit");
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      MyInventory inventory;
      if (localCharacter == null || !localCharacter.TryGetInventory(out inventory))
        return;
      this.ConsumeItem(inventory, id);
    }

    private void ConsumeItem(MyInventory inventory, MyDefinitionId id)
    {
      MyFixedPoint a = inventory != null ? inventory.GetItemAmount(id, MyItemFlags.None, false) : (MyFixedPoint) 0;
      if (!(a > (MyFixedPoint) 0))
        return;
      MyCharacter controlledEntity = MySession.Static.ControlledEntity as MyCharacter;
      MyFixedPoint amount = MyFixedPoint.Min(a, (MyFixedPoint) 1);
      if (controlledEntity == null || controlledEntity.StatComp == null || !(amount > (MyFixedPoint) 0))
        return;
      if (controlledEntity.StatComp.HasAnyComsumableEffect() || controlledEntity.SuitBattery != null && controlledEntity.SuitBattery.HasAnyComsumableEffect())
      {
        if (MyHud.Notifications == null)
          return;
        MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      else
        inventory.ConsumeItem(id, amount, controlledEntity.EntityId);
    }

    private static void ProcessWithdraw(MyEntity owner, MyInventory inventory, int? multiplier)
    {
      if (MySession.Static.LocalCharacter.BuildPlanner.Count == 0)
      {
        MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
      }
      else
      {
        HashSet<MyInventory> usedTargetInventories = new HashSet<MyInventory>();
        List<MyIdentity.BuildPlanItem.Component> missingComponents = MyTerminalInventoryController.Withdraw(owner, inventory, ref usedTargetInventories, multiplier);
        if (missingComponents.Count == 0)
        {
          MyHud.Notifications.Add(MyNotificationSingletons.WithdrawSuccessful);
        }
        else
        {
          string missingComponentsText = MyTerminalInventoryController.GetMissingComponentsText(missingComponents);
          MyHud.Notifications.Get(MyNotificationSingletons.WithdrawFailed)?.SetTextFormatArguments((object) missingComponentsText);
          MyHud.Notifications.Add(MyNotificationSingletons.WithdrawFailed);
        }
      }
    }

    public static void ShowEmptyBuildPlannerNotification()
    {
      string str = !MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed ? "[" + MyInput.Static.GetGameControl(MyControlsSpace.BUILD_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]" : "[" + MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.TOOLBAR_RADIAL_MENU) + "]";
      MyHud.Notifications.Get(MyNotificationSingletons.BuildPlannerEmpty).SetTextFormatArguments((object) str);
      MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerEmpty);
    }

    private void MakeRecord(
      Sandbox.Game.Entities.IMyControllableEntity controlledObject,
      MySessionComponentReplay.ActionRef<PerFrameData> action)
    {
      if (!MySessionComponentReplay.Static.IsEntityBeingRecorded(controlledObject.Entity.GetTopMostParent((Type) null).EntityId))
        return;
      PerFrameData data = new PerFrameData();
      action(ref data);
      MySessionComponentReplay.Static.ProvideEntityRecordData(controlledObject.Entity.GetTopMostParent((Type) null).EntityId, data);
    }

    public void MoveAndRotatePlayerOrCamera()
    {
      MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
      bool flag1 = cameraControllerEnum == MyCameraControllerEnum.Spectator;
      bool flag2 = flag1 || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator && MyInput.Static.IsAnyAltKeyPressed();
      bool flag3 = MyScreenManager.GetScreenWithFocus() is MyGuiScreenDebugBase && !MyInput.Static.IsAnyAltKeyPressed();
      int num = MySessionComponentVoxelHand.Static.BuildMode ? 0 : (!MyCubeBuilder.Static.IsBuildMode ? 1 : 0);
      bool flag4 = !MySessionComponentVoxelHand.Static.BuildMode && !MyCubeBuilder.Static.IsBuildMode;
      float rollIndicator = num != 0 ? MyInput.Static.GetRoll() : 0.0f;
      Vector2 rotationIndicator = MyInput.Static.GetRotation();
      Vector3 moveIndicator = flag4 ? MyInput.Static.GetPositionDelta() : Vector3.Zero;
      if (MySession.Static.ElapsedGameTime < this.SuppressMovement)
      {
        if (!(moveIndicator == Vector3.Zero) || !(rotationIndicator == Vector2.Zero) || (double) rollIndicator != 0.0)
          return;
        this.SuppressMovement = MySession.Static.ElapsedGameTime;
      }
      if (MyPetaInputComponent.MovementDistanceCounter > 0)
      {
        moveIndicator = Vector3.Forward;
        --MyPetaInputComponent.MovementDistanceCounter;
      }
      if (MySession.Static.ControlledEntity != null)
      {
        if (MySandboxGame.IsPaused)
        {
          if (!flag1 && !flag2)
            return;
          if (!flag2 | flag3)
            rotationIndicator = Vector2.Zero;
          rollIndicator = 0.0f;
        }
        if (MySession.Static.IsCameraUserControlledSpectator())
        {
          MySpectatorCameraController.Static.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
        }
        else
        {
          if (!MySession.Static.CameraController.IsInFirstPersonView)
            MyThirdPersonSpectator.Static.UpdateZoom();
          if (MySessionComponentReplay.Static.IsEntityBeingReplayed(MySession.Static.ControlledEntity.Entity.GetTopMostParent((Type) null).EntityId))
            return;
          if (!MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED))
          {
            MySession.Static.ControlledEntity.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
          }
          else
          {
            if (MySession.Static.ControlledEntity is MyRemoteControl)
            {
              rotationIndicator = Vector2.Zero;
              rollIndicator = 0.0f;
            }
            else if (MySession.Static.ControlledEntity is MyCockpit || !MySession.Static.CameraController.IsInFirstPersonView)
            {
              if (!MyInput.Static.IsJoystickLastUsed)
                rotationIndicator = Vector2.Zero;
            }
            else if (MySession.Static.ControlledEntity is MyCharacter)
              rotationIndicator.X = 0.0f;
            if (MyInput.Static.IsJoystickLastUsed)
            {
              moveIndicator = Vector3.Zero;
              rotationIndicator = Vector2.Zero;
            }
            MySession.Static.ControlledEntity.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
            if (MySession.Static.CameraController.IsInFirstPersonView)
              return;
            MyThirdPersonSpectator.Static.SaveSettings();
          }
        }
      }
      else
        MySpectatorCameraController.Static.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
    }

    public static void SetCameraController()
    {
      if (MySession.Static.ControlledEntity == null)
        return;
      if (MySession.Static.ControlledEntity.Entity is MyRemoteControl entity)
      {
        if (!(entity.PreviousControlledEntity is IMyCameraController))
          return;
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) entity.PreviousControlledEntity.Entity, new Vector3D?());
      }
      else
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.ControlledEntity.Entity, new Vector3D?());
    }

    public void SwitchCamera()
    {
      if (MySession.Static.CameraController == null)
        return;
      MySession.Static.CameraController.IsInFirstPersonView = !MySession.Static.CameraController.IsInFirstPersonView;
      if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator)
      {
        MyEntityCameraSettings cameraSettings = (MyEntityCameraSettings) null;
        if (MySession.Static.LocalHumanPlayer != null && MySession.Static.ControlledEntity != null)
        {
          MyThirdPersonSpectator.Static.ResetInternalTimers();
          if (MySession.Static.Cameras.TryGetCameraSettings(MySession.Static.LocalHumanPlayer.Id, MySession.Static.ControlledEntity.Entity.EntityId, MySession.Static.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == MySession.Static.ControlledEntity, out cameraSettings))
          {
            MyThirdPersonSpectator.Static.ResetViewerDistance(new double?(cameraSettings.Distance));
          }
          else
          {
            MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
            MySession.Static.ControlledEntity.ControllerInfo.Controller.SaveCamera();
          }
        }
      }
      MySession.Static.SaveControlledEntityCameraSettings(MySession.Static.CameraController.IsInFirstPersonView);
    }

    public void ShowReconnectMessageBox()
    {
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToReconnect), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        switch (MyMultiplayer.Static)
        {
          case MyMultiplayerLobbyClient _:
            long lobbyId = (long) MyMultiplayer.Static.LobbyId;
            MySessionLoader.UnloadAndExitToMenu();
            MyJoinGameHelper.JoinGame((ulong) lobbyId);
            break;
          case MyMultiplayerClient _:
            MyGameServerItem server = (MyMultiplayer.Static as MyMultiplayerClient).Server;
            MySessionLoader.UnloadAndExitToMenu();
            MyJoinGameHelper.JoinGame(server);
            break;
        }
      })));
      messageBox.SkipTransition = true;
      messageBox.CloseBeforeCallback = true;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }

    public void ShowLoadMessageBox(string currentSession)
    {
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToQuickLoad), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MySessionLoader.Unload();
        MySessionLoader.LoadSingleplayerSession(currentSession);
      })));
      messageBox.SkipTransition = true;
      messageBox.CloseBeforeCallback = true;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }

    public void RequestSessionReload() => this.m_reloadSessionNextFrame = true;

    public override bool Update(bool hasFocus)
    {
      base.Update(hasFocus);
      if (!this.audioSet && MySandboxGame.IsGameReady && (MyAudio.Static != null && MyRenderProxy.VisibleObjectsRead != null) && MyRenderProxy.VisibleObjectsRead.Count > 0)
      {
        MyGuiScreenGamePlay.SetAudioVolumes();
        this.audioSet = true;
        MyVisualScriptLogicProvider.GameIsReady = true;
        MyHud.MinimalHud = false;
        MyAudio.Static.EnableReverb = MySandboxGame.Config.EnableReverb && MyFakes.AUDIO_ENABLE_REVERB;
      }
      MySpectator.Static.Update();
      return true;
    }

    public override bool Draw()
    {
      this.m_drawComponentsTask = new Task?(Parallel.Start(this.m_drawComponentsAction, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.WorkItem, "Draw Session Components")));
      if (MyVRage.Platform.Ansel.IsSessionRunning)
      {
        MyCameraSetup cameraSetup;
        if (!this.m_isAnselCameraInit)
        {
          cameraSetup = new MyCameraSetup()
          {
            ViewMatrix = MySector.MainCamera.ViewMatrix,
            FOV = MySector.MainCamera.FieldOfView,
            AspectRatio = MySector.MainCamera.AspectRatio,
            NearPlane = MySector.MainCamera.NearPlaneDistance,
            FarPlane = MySector.MainCamera.FarFarPlaneDistance,
            Position = MySector.MainCamera.Position
          };
          MyVRage.Platform.Ansel.SetCamera(ref cameraSetup);
          this.m_isAnselCameraInit = true;
        }
        MyVRage.Platform.Ansel.GetCamera(out cameraSetup);
        MyRenderProxy.SetCameraViewMatrix(cameraSetup.ViewMatrix, cameraSetup.ProjectionMatrix, cameraSetup.ProjectionMatrix, cameraSetup.FOV, cameraSetup.FOV, cameraSetup.NearPlane, cameraSetup.FarPlane, cameraSetup.FarPlane, cameraSetup.Position);
        MySector.MainCamera.SetViewMatrix(cameraSetup.ViewMatrix);
        MySector.MainCamera.Update(0.0f);
      }
      else
      {
        this.m_isAnselCameraInit = false;
        if (!MyVRage.Platform.Ansel.IsCaptureRunning && MySector.MainCamera != null)
        {
          MySession.Static.CameraController.ControlCamera(MySector.MainCamera);
          MySector.MainCamera.Update(0.01666667f);
          MySector.MainCamera.UploadViewMatrixToRender();
        }
      }
      MySector.UpdateSunLight();
      int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
      MyRenderProxy.UpdateGameplayFrame(gameplayFrameCounter);
      if (gameplayFrameCounter == 1200 && MyVRage.Platform.System.IsMemoryLimited)
        MyRenderProxy.ClearLargeMessages();
      MyRenderFogSettings settings1 = new MyRenderFogSettings()
      {
        FogMultiplier = MySector.FogProperties.FogMultiplier,
        FogColor = (Color) MySector.FogProperties.FogColor,
        FogDensity = MySector.FogProperties.FogDensity,
        FogSkybox = MySector.FogProperties.FogSkybox,
        FogAtmo = MySector.FogProperties.FogAtmo
      };
      MyRenderProxy.UpdateFogSettings(ref settings1);
      MyRenderPlanetSettings settings2 = new MyRenderPlanetSettings()
      {
        AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier,
        AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier,
        AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward,
        CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier
      };
      MyRenderProxy.UpdatePlanetSettings(ref settings2);
      MyRenderProxy.UpdateSSAOSettings(ref MySector.SSAOSettings);
      MyRenderProxy.UpdateHBAOSettings(ref MySector.HBAOSettings);
      MyEnvironmentData environmentData = MySector.SunProperties.EnvironmentData;
      environmentData.Skybox = !string.IsNullOrEmpty(MySession.Static.CustomSkybox) ? MySession.Static.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture;
      environmentData.SkyboxOrientation = MySector.EnvironmentDefinition.EnvironmentOrientation.ToQuaternion();
      environmentData.EnvironmentLight.SunLightDirection = -MySector.SunProperties.SunDirectionNormalized;
      Vector3D position = MySector.MainCamera.Position;
      MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(position);
      if (closestPlanet != null && closestPlanet.PositionComp.WorldAABB.Contains(position) != ContainmentType.Disjoint)
      {
        float airDensity = closestPlanet.GetAirDensity(position);
        if (closestPlanet.AtmosphereSettings.SunColorLinear.HasValue)
        {
          Vector3 vector3_1 = environmentData.EnvironmentLight.SunColorRaw / MySector.SunProperties.SunIntensity;
          Vector3 vector3_2 = closestPlanet.AtmosphereSettings.SunColorLinear.Value;
          Vector3.Lerp(ref vector3_1, ref vector3_2, airDensity, out environmentData.EnvironmentLight.SunColorRaw);
          environmentData.EnvironmentLight.SunColorRaw *= MySector.SunProperties.SunIntensity;
        }
        if (closestPlanet.AtmosphereSettings.SunSpecularColorLinear.HasValue)
        {
          Vector3 specularColorRaw = environmentData.EnvironmentLight.SunSpecularColorRaw;
          Vector3 vector3 = closestPlanet.AtmosphereSettings.SunSpecularColorLinear.Value;
          Vector3.Lerp(ref specularColorRaw, ref vector3, airDensity, out environmentData.EnvironmentLight.SunSpecularColorRaw);
        }
      }
      MyRenderProxy.UpdateRenderEnvironment(ref environmentData, MySector.ResetEyeAdaptation);
      MySector.ResetEyeAdaptation = false;
      MyRenderProxy.UpdateEnvironmentMap();
      if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled != MyPostprocessSettingsWrapper.AllEnabled || MyPostprocessSettingsWrapper.IsDirty)
      {
        if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled)
          MyPostprocessSettingsWrapper.ReloadSettingsFrom(MySector.EnvironmentDefinition.PostProcessSettings);
        else
          MyPostprocessSettingsWrapper.ReducePostProcessing();
      }
      MyRenderProxy.SwitchPostprocessSettings(ref MyPostprocessSettingsWrapper.Settings);
      if (MyRenderProxy.SettingsDirty)
        MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
      if (this.LoadingDone)
        MyRenderProxy.Draw3DScene();
      if (MySandboxGame.IsPaused && !MyHud.MinimalHud)
        this.DrawPauseIndicator();
      if (MySession.Static != null)
        MySession.Static.DrawSync();
      return true;
    }

    private void ScreenManagerOnEndOfDraw()
    {
      ref Task? local = ref this.m_drawComponentsTask;
      if (local.HasValue)
        local.GetValueOrDefault().WaitOrExecute(true);
      this.m_drawComponentsTask = new Task?();
    }

    private void DrawPauseIndicator()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      fullscreenRectangle.Height /= 18;
      string text = MyTexts.GetString(MyCommonTexts.GamePaused);
      MyGuiManager.DrawSpriteBatch(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_RED2.Texture, fullscreenRectangle, Color.White, false, true);
      MyGuiManager.DrawString("Blue", text, new Vector2(0.5f, 0.024f), 1f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
    }

    private enum MyBuildPlannerAction
    {
      None,
      Withdraw1,
      WithdrawKeep1,
      WithdrawKeep10,
      AddProduction1,
      AddProduction10,
      DepositOre,
    }
  }
}
