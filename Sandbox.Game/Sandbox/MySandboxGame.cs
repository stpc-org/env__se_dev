// Decompiled with JetBrains decompiler
// Type: Sandbox.MySandboxGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ObjectBuilders.SafeZone;
using ParallelTasks;
using ProtoBuf;
using Sandbox.AppCode;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.GUI;
using VRage.Game.GUI.TextPanel;
using VRage.Game.Localization;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.News;
using VRage.Game.ObjectBuilder;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting;
using VRage.GameServices;
using VRage.Http;
using VRage.Input;
using VRage.Library.Threading;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Scripting;
using VRage.Serialization;
using VRage.UserInterface.Media;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.ExternalApp;
using VRageRender.Lights;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox
{
  public class MySandboxGame : Sandbox.Engine.Platform.Game, IDisposable
  {
    private const int MilisToMin = 60000;
    private readonly int m_framesToShowControllerError = 60;
    public static readonly MyStringId DirectX11RendererKey = MyStringId.GetOrCompute("DirectX 11");
    public const string CONSOLE_OUTPUT_AUTORESTART = "AUTORESTART";
    public static Version BuildVersion = Assembly.GetExecutingAssembly().GetName().Version;
    public static DateTime BuildDateTime;
    public static MySandboxGame Static;
    public static Vector2I ScreenSize;
    public static Vector2I ScreenSizeHalf;
    public static MyViewport ScreenViewport;
    private bool m_isControllerErrorMessageBoxVisible;
    private int m_controllerCheckFrameCounter;
    private bool m_isPendingLobbyInvite;
    private IMyLobby m_invitingLobby;
    public static bool ExperimentalOutOfMemoryCrash;
    private static ParallelTasks.Task? m_currentPreloadingTask;
    private static bool m_reconfirmClipmaps;
    private static bool m_areClipmapsReady;
    private static bool m_renderTasksFinished;
    public static bool IsUpdateReady;
    private static EnumAutorestartStage m_autoRestartState;
    private int m_lastRestartCheckInMilis;
    private DateTime m_lastUpdateCheckTime = DateTime.UtcNow;
    private int m_autoUpdateRestartTimeInMin = int.MaxValue;
    private bool m_isGoingToUpdate;
    public static bool IsRenderUpdateSyncEnabled;
    public static bool IsVideoRecordingEnabled;
    public static bool IsConsoleVisible;
    public static bool IsReloading;
    public static bool FatalErrorDuringInit;
    protected static ManualResetEvent m_windowCreatedEvent;
    public static MyLog Log;
    public static MySandboxGame.NonInteractiveReportAction PerformNotInteractiveReport;
    private bool hasFocus = true;
    private static int m_pauseStartTimeInMilliseconds;
    private static int m_totalPauseTimeInMilliseconds;
    private static long m_lastFrameTimeStamp;
    public static int NumberOfCores;
    public static uint CPUFrequency;
    public static bool InsufficientHardware;
    private bool m_dataLoadedDebug;
    private ulong? m_joinLobbyId;
    private string m_launchScenario;
    private string m_launchScenarioInstance;
    private string m_launchScenarioPlatform;
    public static bool ShowIsBetterGCAvailableNotification;
    public static bool ShowGpuUnderMinimumNotification;
    private MyConcurrentQueue<MySandboxGame.MyInvokeData> m_invokeQueue = new MyConcurrentQueue<MySandboxGame.MyInvokeData>(32);
    private MyConcurrentQueue<MySandboxGame.MyInvokeData> m_invokeQueueExecuting = new MyConcurrentQueue<MySandboxGame.MyInvokeData>(32);
    public MyGameRenderComponent GameRenderComponent;
    public MySessionCompatHelper SessionCompatHelper;
    public static MyConfig Config;
    public static IMyConfigDedicated ConfigDedicated;
    private bool m_enableDamageEffects = true;
    private bool m_unpauseInput;
    private DateTime m_inputPauseTime;
    private const int INPUT_UNPAUSE_DELAY = 10;
    private static int m_timerTTHelper;
    private static readonly int m_timerTTHelper_Max;
    private MyGuiScreenMessageBox m_noUserMessageBox;
    private bool continuet = true;
    public Action<Vector2I> OnScreenSize;
    private static bool ShowWhitelistPopup;
    private static bool CanShowWhitelistPopup;
    private static bool ShowHotfixPopup;
    private static bool CanShowHotfixPopup;
    private static bool m_isPaused;
    private static int m_pauseStackCount;
    private ParallelTasks.Task? m_soundUpdate;
    private MyNews m_changelog;
    private XmlSerializer m_changelogSerializer;
    private int m_queueSize;
    private static IErrorConsumer m_errorConsumer;
    private IVRageWindow form;
    private bool m_joystickLastUsed;

    public static bool IsDirectX11 => MyVideoSettingsManager.RunningGraphicsRenderer == MySandboxGame.DirectX11RendererKey;

    public static bool IsGameReady
    {
      get
      {
        if (!MySandboxGame.IsUpdateReady)
          return false;
        return MySandboxGame.AreClipmapsReady && MySandboxGame.RenderTasksFinished || Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null;
      }
    }

    public static bool AreClipmapsReady
    {
      get => MySandboxGame.m_areClipmapsReady || !MyFakes.ENABLE_WAIT_UNTIL_CLIPMAPS_READY;
      set
      {
        MySandboxGame.m_areClipmapsReady = !MySandboxGame.m_reconfirmClipmaps & value;
        if (MySession.Static != null)
        {
          if (MySession.Static.VoxelMaps.Instances.Count == 0)
          {
            MySandboxGame.m_areClipmapsReady = true;
            MySandboxGame.m_reconfirmClipmaps = false;
          }
          else if (!value || MySandboxGame.m_reconfirmClipmaps)
          {
            foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
            {
              if (instance.Render is MyRenderComponentVoxelMap render)
                render.ResetLoading();
            }
          }
        }
        MySandboxGame.m_reconfirmClipmaps = !value;
      }
    }

    public static bool RenderTasksFinished
    {
      get => true;
      set => MySandboxGame.m_renderTasksFinished = value;
    }

    public static EnumAutorestartStage AutoRestartState => MySandboxGame.m_autoRestartState;

    public static bool IsAutoRestarting => MySandboxGame.m_autoRestartState == EnumAutorestartStage.Restarting;

    public MySandboxGame.MemState MemoryState { get; private set; }

    public bool IsGoingToUpdate => this.m_isGoingToUpdate;

    public bool IsRestartingForUpdate => this.IsGoingToUpdate && MySandboxGame.IsAutoRestarting;

    public bool SuppressLoadingDialogs { get; private set; }

    public static int TotalGamePlayTimeInMilliseconds => (MySandboxGame.IsPaused ? MySandboxGame.m_pauseStartTimeInMilliseconds : MySandboxGame.TotalSimulationTimeInMilliseconds) - MySandboxGame.m_totalPauseTimeInMilliseconds;

    public static int TotalTimeInMilliseconds => (int) MySandboxGame.Static.TotalTime.Milliseconds;

    public static int TotalTimeInTicks => (int) MySandboxGame.Static.TotalTime.Ticks;

    public static int TotalSimulationTimeInMilliseconds => (int) MySandboxGame.Static.SimulationTimeWithSpeed.Milliseconds;

    public static double SecondsSinceLastFrame { get; private set; }

    public bool EnableDamageEffects
    {
      get => this.m_enableDamageEffects;
      set
      {
        this.m_enableDamageEffects = value;
        this.UpdateDamageEffectsInScene();
      }
    }

    public event EventHandler OnGameLoaded;

    public event EventHandler OnScreenshotTaken;

    public static MySandboxGame.IGameCustomInitialization GameCustomInitialization { get; set; }

    public bool IsCursorVisible { get; private set; }

    public bool PauseInput { get; private set; }

    public static bool IsExitForced { get; set; }

    public MySandboxGame(string[] commandlineArgs, IntPtr windowHandle = default (IntPtr))
    {
      MyUtils.MainThread = Thread.CurrentThread;
      if (MySandboxGame.Config.SyncRendering)
      {
        MyRandom.EnableDeterminism = true;
        MyFakes.ENABLE_WORKSHOP_MODS = false;
        MyFakes.ENABLE_HAVOK_MULTITHREADING = false;
        MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = false;
        MyRenderProxy.SetFrameTimeStep(0.01666667f);
        MyRenderProxy.Settings.IgnoreOcclusionQueries = !MySandboxGame.IsVideoRecordingEnabled;
        MyRenderProxy.SetSettingsDirty();
      }
      if (commandlineArgs != null)
      {
        if (commandlineArgs.Contains<string>("-skipintro"))
          MyPlatformGameSettings.ENABLE_LOGOS = false;
        if (commandlineArgs.Contains<string>("-suppressLoadingDialogs"))
          this.SuppressLoadingDialogs = true;
      }
      this.UpdateThread = Thread.CurrentThread;
      MyScreenManager.UpdateThread = this.UpdateThread;
      MyScreenManager.OnValidateText += new Func<StringBuilder, string>(this.IsTextOffensive);
      MyPrecalcComponent.UpdateThreadManagedId = this.UpdateThread.ManagedThreadId;
      this.InitializeRender(windowHandle);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        Console.CancelKeyPress += new ConsoleCancelEventHandler(this.Console_CancelKeyPress);
      this.RegisterAssemblies(commandlineArgs);
      this.ProcessCommandLine(commandlineArgs);
      MySandboxGame.Log.WriteLine("MySandboxGame.Constructor() - START");
      MySandboxGame.Log.IncreaseIndent();
      MySandboxGame.Log.WriteLine("Game dir: " + MyFileSystem.ExePath);
      MySandboxGame.Log.WriteLine("Content dir: " + MyFileSystem.ContentPath);
      MySandboxGame.Static = this;
      MyLanguage.Init();
      MyGlobalTypeMetadata.Static.Init();
      MySandboxGame.Preallocate();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        IPEndPoint serverEndpoint = new IPEndPoint(MyDedicatedServerOverrides.IpAddress ?? IPAddressExtensions.ParseOrAny(MySandboxGame.ConfigDedicated.IP), (int) (ushort) (MyDedicatedServerOverrides.Port ?? MySandboxGame.ConfigDedicated.ServerPort));
        MyLog.Default.WriteLineAndConsole("Bind IP : " + serverEndpoint.ToString());
        MySandboxGame.FatalErrorDuringInit = !((MyDedicatedServerBase) (Sandbox.Engine.Multiplayer.MyMultiplayer.Static = (MyMultiplayerBase) new MyDedicatedServer(serverEndpoint, new Func<string, string>(this.FilterOffensive)))).ServerStarted;
        if (MySandboxGame.FatalErrorDuringInit)
          throw new Exception("Fatal error during dedicated server init see log for more information.")
          {
            Data = {
              [(object) "Silent"] = (object) true
            }
          };
      }
      if (Sandbox.Engine.Platform.Game.IsDedicated && !MySandboxGame.FatalErrorDuringInit)
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyDedicatedServerBase).SendGameTagsToSteam();
      this.SessionCompatHelper = Activator.CreateInstance(MyPerGameSettings.CompatHelperType) as MySessionCompatHelper;
      MyGameService.OnThreadpoolInitialized();
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
        MyGameService.OnOverlayActivated += new Action<bool>(this.OverlayActivated);
      MyCampaignManager.Static.Init();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MySandboxGame.Constructor() - END");
      if (MyFakes.OWN_ALL_ITEMS)
        MyGameService.AddUnownedItems();
      MyGameService.OnUserChanged += new Action<bool>(this.OnUserChanged);
    }

    public string FilterOffensive(string text)
    {
      if (!MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        MySession mySession = MySession.Static;
        if ((mySession != null ? (mySession.Settings.OffensiveWordsFiltering ? 1 : 0) : 0) == 0)
          return text;
      }
      return MyOffensiveWords.Instance.FixOffensiveString(text);
    }

    private string IsTextOffensive(StringBuilder sb)
    {
      if (!MyPlatformGameSettings.CONSOLE_COMPATIBLE)
      {
        MySession mySession = MySession.Static;
        if ((mySession != null ? (mySession.Settings.OffensiveWordsFiltering ? 1 : 0) : 0) == 0)
          return (string) null;
      }
      return MyOffensiveWords.Instance.IsTextOffensive(sb);
    }

    private void OnUserChanged(bool differentUserLoggedIn)
    {
      if (!differentUserLoggedIn)
        return;
      bool flag = MySession.Static != null & differentUserLoggedIn;
      MySandboxGame.Config.Clear();
      if (flag)
      {
        MyAudio.Static.Mute = true;
        MyAudio.Static.StopMusic();
        MySessionLoader.UnloadAndExitToMenu();
      }
      MyGuiScreenMainMenuBase menuScreen = MyScreenManager.GetFirstScreenOfType<MyGuiScreenMainMenuBase>();
      if (menuScreen != null)
        MyScreenManager.CloseAllScreensExceptThisOneAndAllTopMost((MyGuiScreenBase) menuScreen);
      Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicationLayer.SetLocalEndpoint(new EndpointId(Sync.MyId));
      if (MySpaceAnalytics.Instance != null & differentUserLoggedIn)
      {
        if (this.m_noUserMessageBox == null)
          MySpaceAnalytics.Instance.ReportSessionEnd("UserChanged");
        if (MyGameService.UserId != 0UL)
          MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(MyGameService.UserId, false);
      }
      if (MyGameService.UserId != 0UL)
      {
        bool syncLoad = true;
        MyGuiScreenMessageBox messageBox = (MyGuiScreenMessageBox) null;
        if (flag)
        {
          syncLoad = false;
          messageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE, MyTexts.Get(MyCommonTexts.MessageBox_LoadingUserData));
          MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
        }
        this.InitCloud(syncLoad, (Action) (() =>
        {
          messageBox?.CloseScreen();
          menuScreen?.RecreateControls(false);
          if (menuScreen == null)
            return;
          this.ShowIntroMessages();
        }));
      }
      else
      {
        if (menuScreen == null)
          return;
        this.ShowIntroMessages();
      }
    }

    private void CheckNoUser()
    {
      if (MyGameService.UserId == 0UL)
      {
        if (this.m_noUserMessageBox != null)
          return;
        this.m_noUserMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE, MyTexts.Get(MyCommonTexts.MessageBoxTextNoUser), MyTexts.Get(MyCommonTexts.MessageBoxCaptionUser));
        MyGuiSandbox.AddScreen((MyGuiScreenBase) this.m_noUserMessageBox);
      }
      else
      {
        if (this.m_noUserMessageBox == null)
          return;
        this.m_noUserMessageBox.CloseScreen();
        this.m_noUserMessageBox = (MyGuiScreenMessageBox) null;
      }
    }

    protected virtual void InitializeRender(IntPtr windowHandle)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.GameRenderComponent = new MyGameRenderComponent();
      this.StartRenderComponent(MyVideoSettingsManager.Initialize(), windowHandle);
      MySandboxGame.m_windowCreatedEvent.WaitOne();
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      e.Cancel = true;
      MySandboxGame.ExitThreadSafe();
    }

    public void Run(bool customRenderLoop = false, Action disposeSplashScreen = null)
    {
      if (MySandboxGame.FatalErrorDuringInit)
        return;
      if (this.GameRenderComponent != null)
      {
        MyVideoSettingsManager.LogApplicationInformation();
        MyVRage.Platform.System.LogEnvironmentInformation();
      }
      if ((1 & (this.Initialize() ? 1 : 0)) == 0 && Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (MySession.Static == null)
          throw new ApplicationException("Session can not start. Save is corrupted or not valid. See log file for more information.");
        MySession.Static.SetSaveOnUnloadOverride_Dedicated(new bool?(false));
      }
      this.ProcessInvoke();
      if (disposeSplashScreen != null)
        disposeSplashScreen();
      this.LoadData_UpdateThread();
      foreach (IPlugin plugin in MyPlugins.Plugins)
      {
        MySandboxGame.Log.WriteLineAndConsole("Plugin Init: " + (object) plugin.GetType());
        plugin.Init((object) this);
      }
      if (MyPerGameSettings.Destruction && !HkBaseSystem.DestructionEnabled)
      {
        MyLog.Default.WriteLine("Havok Destruction is not availiable in this build. Exiting game.");
        MySandboxGame.ExitThreadSafe();
      }
      else
      {
        if (customRenderLoop)
          return;
        using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
          this.RunLoop();
        this.EndLoop();
      }
    }

    private void UpdateAudioSettings()
    {
      MyAudio.Static.EnableDoppler = MySandboxGame.Config.EnableDoppler;
      MyAudio.Static.VolumeMusic = MySandboxGame.Config.MusicVolume;
      MyAudio.Static.VolumeGame = MySandboxGame.Config.GameVolume;
      MyAudio.Static.VolumeHud = MySandboxGame.Config.GameVolume;
      MyAudio.Static.VolumeVoiceChat = MySandboxGame.Config.VoiceChatVolume;
      MyAudio.Static.EnableVoiceChat = MySandboxGame.Config.EnableVoiceChat;
      MyGuiAudio.HudWarnings = MySandboxGame.Config.HudWarnings;
    }

    public void UpdateUIScale()
    {
      float mainViewportScale = MySandboxGame.Config.SpriteMainViewportScale;
      MyGuiManager.UIScale = mainViewportScale;
      MyInput.Static.SetMousePositionScale(mainViewportScale);
      MyRenderProxy.SetSpriteMainViewportScale(mainViewportScale);
    }

    private void UpdateConfigFromCloud(bool syncLoad, Action onDone = null)
    {
      if (!MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
        return;
      MySandboxGame.Config.LoadFromCloud(syncLoad, (Action) (() =>
      {
        this.UpdateUIScale();
        this.UpdateAudioSettings();
        this.UpdateMouseCapture();
        MyLanguage.CurrentLanguage = MySandboxGame.Config.Language;
        MyInput.Static.LoadControls(MySandboxGame.Config.ControlsGeneral, MySandboxGame.Config.ControlsButtons);
        onDone.InvokeIfNotNull();
      }));
    }

    public void EndLoop()
    {
      MyLog.Default.WriteLineAndConsole("Exiting..");
      if (MySpaceAnalytics.Instance != null)
        MySpaceAnalytics.Instance.ReportSessionEnd(nameof (EndLoop));
      this.UnloadData_UpdateThread();
    }

    public virtual void SwitchSettings(MyRenderDeviceSettings settings) => MyRenderProxy.SwitchDeviceSettings(settings);

    protected virtual void InitInput()
    {
      MyLog.Default.WriteLine("MyGuiGameControlsHelpers()");
      MyGuiGameControlsHelpers.Add(MyControlsSpace.FORWARD, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Forward));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.BACKWARD, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Backward));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.STRAFE_LEFT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_StrafeLeft));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.STRAFE_RIGHT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_StrafeRight));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROLL_LEFT, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RollLeft));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROLL_RIGHT, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RollRight));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPRINT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_HoldToSprint));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.PRIMARY_TOOL_ACTION, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_FirePrimaryWeapon));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SECONDARY_TOOL_ACTION, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_FireSecondaryWeapon));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.JUMP, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_UpOrJump));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CROUCH, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_DownOrCrouch));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_WALK, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_SwitchWalk));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.DAMPING, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_InertialDampenersOnOff));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.THRUSTS, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_Jetpack));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.BROADCASTING, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_Broadcasting));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.HELMET, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_Helmet));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.USE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_UseOrInteract));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_REACTORS, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_PowerSwitchOnOff));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TERMINAL, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_TerminalOrInventory));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.INVENTORY, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.Inventory));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.HELP_SCREEN, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Help));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ACTIVE_CONTRACT_SCREEN, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ActiveContractScreen));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SUICIDE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Suicide));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.PAUSE_GAME, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_PauseGame, new MyStringId?(MyCommonTexts.ControlDescPauseGame)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_LEFT, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RotationLeft));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_RIGHT, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RotationRight));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_UP, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RotationUp));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_DOWN, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_RotationDown));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CAMERA_MODE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_FirstOrThirdPerson));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.HEADLIGHTS, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_ToggleHeadlights));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CHAT_SCREEN, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.Chat_screen));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CONSOLE, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_Console));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SCREENSHOT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Screenshot));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.LOOKAROUND, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_HoldToLookAround));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.LANDING_GEAR, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_LandingGear));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.COLOR_PICKER, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_ColorPicker));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_LEFT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_PreviousColor));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_RIGHT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_NextColor));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.BUILD_SCREEN, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarConfig));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateVerticalPos));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateVerticalNeg));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateHorizontalPos));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateHorizontalNeg));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateRollPos));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateRollNeg));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_COLOR_CHANGE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeColorChange));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.BUILD_PLANNER, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_BuildPlanner));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_CubeSizeMode, new MyStringId?(MyCommonTexts.ControlName_CubeSizeMode_Tooltip)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SYMMETRY_SWITCH, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_SymmetrySwitch));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.USE_SYMMETRY, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_UseSymmetry));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.ControlName_CubeDefaultMountpoint));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT1, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot1));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT2, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot2));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT3, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot3));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT4, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot4));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT5, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot5));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT6, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot6));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT7, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot7));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT8, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot8));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT9, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot9));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT0, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Slot0));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_DOWN, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarDown));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_UP, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarUp));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_NEXT_ITEM, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarNextItem));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_PREV_ITEM, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarPreviousItem));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_NONE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.SpectatorControls_None, new MyStringId?(MySpaceTexts.SpectatorControls_None_Desc)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_DELTA, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Delta, new MyStringId?(MyCommonTexts.SpectatorControls_Delta_Desc)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_FREE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Free, new MyStringId?(MySpaceTexts.SpectatorControls_Free_Desc)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_STATIC, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Static, new MyStringId?(MySpaceTexts.SpectatorControls_Static_Desc)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_HUD, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_HudOnOff));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.VOXEL_HAND_SETTINGS, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_VoxelHandSettings));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.CONTROL_MENU, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ControlMenu));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.FREE_ROTATION, (MyDescriptor) new MyGuiDescriptor(MySpaceTexts.StationRotation_Static, new MyStringId?(MySpaceTexts.StationRotation_Static_Desc)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_SIGNALS, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_ToggleSignalsMode, new MyStringId?(MyCommonTexts.ControlName_ToggleSignalsMode_Tooltip)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.RELOAD, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_Reload, new MyStringId?(MyCommonTexts.ControlName_Reload_Tooltip)));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_LOCK, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_SpectatorLock));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_SWITCHMODE, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_SpectatorSwitchMode));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_NEXTPLAYER, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_SpectatorNextPlayer));
      MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_PREVPLAYER, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_SpectatorPrevPlayer));
      if (MyPerGameSettings.VoiceChatEnabled)
        MyGuiGameControlsHelpers.Add(MyControlsSpace.VOICE_CHAT, (MyDescriptor) new MyGuiDescriptor(MyCommonTexts.ControlName_VoiceChat));
      Dictionary<MyStringId, MyControl> dictionary = new Dictionary<MyStringId, MyControl>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.FORWARD, key: new MyKeys?(MyKeys.W));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.BACKWARD, key: new MyKeys?(MyKeys.S));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.STRAFE_LEFT, key: new MyKeys?(MyKeys.A));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.STRAFE_RIGHT, key: new MyKeys?(MyKeys.D));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_LEFT, key: new MyKeys?(MyKeys.Left));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_RIGHT, key: new MyKeys?(MyKeys.Right));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_UP, key: new MyKeys?(MyKeys.Up));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_DOWN, key: new MyKeys?(MyKeys.Down));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROLL_LEFT, key: new MyKeys?(MyKeys.Q));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROLL_RIGHT, key: new MyKeys?(MyKeys.E));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.SPRINT, key: new MyKeys?(MyKeys.Shift));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.SWITCH_WALK, key: new MyKeys?(MyKeys.CapsLock));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.JUMP, key: new MyKeys?(MyKeys.Space));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.CROUCH, key: new MyKeys?(MyKeys.C));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.PRIMARY_TOOL_ACTION, new MyMouseButtonsEnum?(MyMouseButtonsEnum.Left));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.SECONDARY_TOOL_ACTION, new MyMouseButtonsEnum?(MyMouseButtonsEnum.Right));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.RELOAD, key: new MyKeys?(MyKeys.R));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.USE, key: new MyKeys?(MyKeys.F));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.HELMET, key: new MyKeys?(MyKeys.J));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.THRUSTS, key: new MyKeys?(MyKeys.X));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.DAMPING, key: new MyKeys?(MyKeys.Z));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.BROADCASTING, key: new MyKeys?(MyKeys.O));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.TOGGLE_REACTORS, key: new MyKeys?(MyKeys.Y));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.HEADLIGHTS, key: new MyKeys?(MyKeys.L));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.CHAT_SCREEN, key: new MyKeys?(MyKeys.Enter));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.TERMINAL, key: new MyKeys?(MyKeys.K));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.INVENTORY, key: new MyKeys?(MyKeys.I));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.SUICIDE, key: new MyKeys?(MyKeys.Back));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.BUILD_SCREEN, key: new MyKeys?(MyKeys.G));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, key: new MyKeys?(MyKeys.PageDown));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, key: new MyKeys?(MyKeys.Delete));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, key: new MyKeys?(MyKeys.Home));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, key: new MyKeys?(MyKeys.End));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, key: new MyKeys?(MyKeys.Insert));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, key: new MyKeys?(MyKeys.PageUp));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_COLOR_CHANGE, new MyMouseButtonsEnum?(MyMouseButtonsEnum.Middle));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, key: new MyKeys?(MyKeys.T));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.USE_SYMMETRY, key: new MyKeys?(MyKeys.N));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.SYMMETRY_SWITCH, key: new MyKeys?(MyKeys.M));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.FREE_ROTATION, key: new MyKeys?(MyKeys.B));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.BUILD_PLANNER, new MyMouseButtonsEnum?(MyMouseButtonsEnum.Middle));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, key: new MyKeys?(MyKeys.R));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_UP, key: new MyKeys?(MyKeys.OemPeriod));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_DOWN, key: new MyKeys?(MyKeys.OemComma));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_NEXT_ITEM);
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_PREV_ITEM);
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT1, key: new MyKeys?(MyKeys.D1));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT2, key: new MyKeys?(MyKeys.D2));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT3, key: new MyKeys?(MyKeys.D3));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT4, key: new MyKeys?(MyKeys.D4));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT5, key: new MyKeys?(MyKeys.D5));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT6, key: new MyKeys?(MyKeys.D6));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT7, key: new MyKeys?(MyKeys.D7));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT8, key: new MyKeys?(MyKeys.D8));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT9, key: new MyKeys?(MyKeys.D9));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT0, key: new MyKeys?(MyKeys.D0), key2: new MyKeys?(MyKeys.OemTilde));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.SWITCH_LEFT, key: new MyKeys?(MyKeys.OemOpenBrackets));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.SWITCH_RIGHT, key: new MyKeys?(MyKeys.OemCloseBrackets));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.LANDING_GEAR, key: new MyKeys?(MyKeys.P));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.COLOR_PICKER, key: new MyKeys?(MyKeys.P));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.VOXEL_HAND_SETTINGS, key: new MyKeys?(MyKeys.K));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.CONTROL_MENU, key: new MyKeys?(MyKeys.OemMinus));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.PAUSE_GAME, key: new MyKeys?(MyKeys.Pause));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.CONSOLE, key: new MyKeys?(MyKeys.OemTilde));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.HELP_SCREEN, key: new MyKeys?(MyKeys.F1));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, key: new MyKeys?(MyKeys.OemSemicolon));
      if (MyPerGameSettings.VoiceChatEnabled)
        MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.VOICE_CHAT, key: new MyKeys?(MyKeys.U));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_NONE, key: new MyKeys?(MyKeys.F6));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_DELTA, key: new MyKeys?(MyKeys.F7));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_FREE, key: new MyKeys?(MyKeys.F8));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_STATIC, key: new MyKeys?(MyKeys.F9));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.CAMERA_MODE, key: new MyKeys?(MyKeys.V));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.LOOKAROUND, key: new MyKeys?(MyKeys.Alt));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SCREENSHOT, key: new MyKeys?(MyKeys.F4));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.TOGGLE_HUD, key: new MyKeys?(MyKeys.Tab));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.TOGGLE_SIGNALS, key: new MyKeys?(MyKeys.H));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_LOCK, key: new MyKeys?(MyKeys.Multiply));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_SWITCHMODE, key: new MyKeys?(MyKeys.Divide));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_NEXTPLAYER, key: new MyKeys?(MyKeys.Add));
      MySandboxGame.AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_PREVPLAYER, key: new MyKeys?(MyKeys.Subtract));
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MyInput.Initialize((VRage.Input.IMyInput) new MyNullInput());
      }
      else
      {
        MyInput.Initialize((VRage.Input.IMyInput) new MyVRageInput(MyVRage.Platform.Input, (IMyControlNameLookup) new MyKeysToString(), dictionary, MyFakes.ENABLE_F12_MENU, (Action) (() => MyFakes.ENABLE_F12_MENU = true)));
        MyInput.Static.SetMousePositionScale(MySandboxGame.Config.SpriteMainViewportScale);
      }
      MySpaceBindingCreator.CreateBindingDefault();
      MySpaceBindingCreator.RegisterEvaluators();
    }

    private void InitJoystick()
    {
      List<string> stringList = MyInput.Static.EnumerateJoystickNames();
      if (!MyFakes.ENFORCE_CONTROLLER || stringList.Count <= 0)
        return;
      MyInput.Static.JoystickInstanceName = stringList[0];
    }

    protected virtual void InitSteamWorkshop() => MyWorkshop.Init(new MyWorkshop.Category[14]
    {
      new MyWorkshop.Category()
      {
        Id = "block",
        LocalizableName = MyCommonTexts.WorkshopTag_Block,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "skybox",
        LocalizableName = MyCommonTexts.WorkshopTag_Skybox,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "character",
        LocalizableName = MyCommonTexts.WorkshopTag_Character,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "animation",
        LocalizableName = MyCommonTexts.WorkshopTag_Animation,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "respawn ship",
        LocalizableName = MySpaceTexts.WorkshopTag_RespawnShip,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "production",
        LocalizableName = MySpaceTexts.WorkshopTag_Production,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "script",
        LocalizableName = MyCommonTexts.WorkshopTag_Script,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "modpack",
        LocalizableName = MyCommonTexts.WorkshopTag_ModPack,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "asteroid",
        LocalizableName = MySpaceTexts.WorkshopTag_Asteroid,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "planet",
        LocalizableName = MySpaceTexts.WorkshopTag_Planet,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "hud",
        LocalizableName = MySpaceTexts.WorkshopTag_Hud,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "other",
        LocalizableName = MyCommonTexts.WorkshopTag_Other,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "npc",
        LocalizableName = MyCommonTexts.WorkshopTag_Npc,
        IsVisibleForFilter = false
      },
      new MyWorkshop.Category()
      {
        Id = "ServerScripts",
        LocalizableName = MyCommonTexts.WorkshopTag_ServerScripts,
        IsVisibleForFilter = false
      }
    }, new MyWorkshop.Category[5]
    {
      new MyWorkshop.Category()
      {
        Id = "story",
        LocalizableName = MySpaceTexts.WorkshopTag_Story,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "pvp",
        LocalizableName = MySpaceTexts.WorkshopTag_PvP,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "exploration",
        LocalizableName = MySpaceTexts.WorkshopTag_Exploration,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "survival",
        LocalizableName = MySpaceTexts.WorkshopTag_Survival,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "other_world",
        LocalizableName = MyCommonTexts.WorkshopTag_Other,
        IsVisibleForFilter = true
      }
    }, new MyWorkshop.Category[4]
    {
      new MyWorkshop.Category()
      {
        Id = "ship",
        LocalizableName = MySpaceTexts.WorkshopTag_Ship,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "rover",
        LocalizableName = MySpaceTexts.WorkshopTag_Rover,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "base",
        LocalizableName = MySpaceTexts.WorkshopTag_Base,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "other_blueprint",
        LocalizableName = MyCommonTexts.WorkshopTag_Other,
        IsVisibleForFilter = true
      }
    }, new MyWorkshop.Category[5]
    {
      new MyWorkshop.Category()
      {
        Id = "story",
        LocalizableName = MySpaceTexts.WorkshopTag_Story,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "pvp",
        LocalizableName = MySpaceTexts.WorkshopTag_PvP,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "exploration",
        LocalizableName = MySpaceTexts.WorkshopTag_Exploration,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "survival",
        LocalizableName = MySpaceTexts.WorkshopTag_Survival,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "other_world",
        LocalizableName = MyCommonTexts.WorkshopTag_Other,
        IsVisibleForFilter = true
      }
    }, new MyWorkshop.Category[4]
    {
      new MyWorkshop.Category()
      {
        Id = "inventory management",
        LocalizableName = MyCommonTexts.WorkshopTag_InventoryManagement,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "visualization",
        LocalizableName = MyCommonTexts.WorkshopTag_Visualization,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "autopilot",
        LocalizableName = MyCommonTexts.WorkshopTag_Autopilot,
        IsVisibleForFilter = true
      },
      new MyWorkshop.Category()
      {
        Id = "other_script",
        LocalizableName = MyCommonTexts.WorkshopTag_Other,
        IsVisibleForFilter = true
      }
    });

    protected static void AddDefaultGameControl(
      Dictionary<MyStringId, MyControl> self,
      MyGuiControlTypeEnum controlTypeEnum,
      MyStringId controlId,
      MyMouseButtonsEnum? mouse = null,
      MyKeys? key = null,
      MyKeys? key2 = null)
    {
      MyDescriptor gameControlHelper = MyGuiGameControlsHelpers.GetGameControlHelper(controlId);
      self[controlId] = new MyControl(controlId, gameControlHelper.NameEnum, controlTypeEnum, mouse, key, defaultControlKey2: key2, description: gameControlHelper.DescriptionEnum);
    }

    private void RegisterAssemblies(string[] args)
    {
      MyPlugins.RegisterGameAssemblyFile(MyPerGameSettings.GameModAssembly);
      if (MyPerGameSettings.GameModBaseObjBuildersAssembly != null)
        MyPlugins.RegisterBaseGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModBaseObjBuildersAssembly);
      MyPlugins.RegisterGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModObjBuildersAssembly);
      MyPlugins.RegisterSandboxAssemblyFile(MyPerGameSettings.SandboxAssembly);
      MyPlugins.RegisterSandboxGameAssemblyFile(MyPerGameSettings.SandboxGameAssembly);
      MyPlugins.RegisterFromArgs(args);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        MyPlugins.RegisterUserAssemblyFiles(MySandboxGame.ConfigDedicated.Plugins);
      MyPlugins.Load();
    }

    private void ProcessCommandLine(string[] args)
    {
      if (args == null)
        return;
      for (int index = 0; index < args.Length; ++index)
      {
        string str = args[index];
        if (str == "+connect_lobby" && args.Length > index + 1)
        {
          ++index;
          ulong result;
          if (ulong.TryParse(args[index], out result))
            this.m_joinLobbyId = new ulong?(result);
        }
        if (str == "+launch_scenario" && args.Length > index + 1)
        {
          ++index;
          this.m_launchScenario = args[index];
        }
        if (str == "+launch_scenario_instance" && args.Length > index + 1)
        {
          ++index;
          this.m_launchScenarioInstance = args[index];
        }
        if (str == "+launch_scenario_platform" && args.Length > index + 1)
        {
          ++index;
          this.m_launchScenarioPlatform = args[index];
        }
        if (str == "+debugger")
          MyVRage.Platform.Scripting.VSTAssemblyProvider.DebugEnabled = true;
      }
    }

    public void ShowIntroMessages()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MyGuiScreenMainMenuBase firstScreenOfType1 = MyScreenManager.GetFirstScreenOfType<MyGuiScreenMainMenuBase>();
      if (firstScreenOfType1 == null)
      {
        MyGuiSandbox.BackToMainMenu();
        firstScreenOfType1 = MyScreenManager.GetFirstScreenOfType<MyGuiScreenMainMenuBase>();
      }
      MyGuiScreenBase firstScreenOfType2 = (MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenGDPR>();
      if (MyFakes.ENABLE_GDPR_MESSAGE)
      {
        bool? gdprConsentSent = MySandboxGame.Config.GDPRConsentSent;
        bool flag = false;
        if (!(gdprConsentSent.GetValueOrDefault() == flag & gdprConsentSent.HasValue))
        {
          gdprConsentSent = MySandboxGame.Config.GDPRConsentSent;
          if (gdprConsentSent.HasValue)
            goto label_8;
        }
        if (firstScreenOfType2 == null)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenGDPR());
          goto label_10;
        }
        else
          goto label_10;
      }
label_8:
      firstScreenOfType2?.CloseScreen();
label_10:
      firstScreenOfType1.CloseUserRelatedScreens();
      if (MyGameService.UserId != 0UL)
        firstScreenOfType1.OpenUserRelatedScreens();
      MyGuiScreenBase firstScreenOfType3 = (MyGuiScreenBase) MyScreenManager.GetFirstScreenOfType<MyGuiScreenWelcomeScreen>();
      if (MySandboxGame.Config.WelcomScreenCurrentStatus == MyConfig.WelcomeScreenStatus.NotSeen)
      {
        if (firstScreenOfType3 == null)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWelcomeScreen());
      }
      else
        firstScreenOfType3?.CloseScreen();
      if (MySandboxGame.ExperimentalOutOfMemoryCrash)
      {
        MySandboxGame.ExperimentalOutOfMemoryCrash = false;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.ExperimentalOutOfMemoryCrashMessageBox), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnExperimentalOutOfMemoryCrashMessageBox)));
      }
      this.CheckNoUser();
    }

    private void InitCloud(bool syncLoad, Action onDone = null) => this.UpdateConfigFromCloud(syncLoad, (Action) (() =>
    {
      MyLocalCache.UpdateLastSessionFromCloud();
      onDone.InvokeIfNotNull();
    }));

    private bool InitQuickLaunch()
    {
      if (MyVRage.Platform.Windows.Window != null)
        MyVRage.Platform.Windows.Window.ShowAndFocus();
      MyWorkshop.CancelToken cancelToken = new MyWorkshop.CancelToken();
      MyQuickLaunchType? nullable = new MyQuickLaunchType?();
      this.InitCloud(true);
      if (this.m_joinLobbyId.HasValue)
      {
        IMyLobby lobby = MyGameService.CreateLobby(this.m_joinLobbyId.Value);
        if (lobby.IsValid)
        {
          MyJoinGameHelper.JoinGame(lobby);
          return true;
        }
      }
      if (this.m_launchScenarioInstance != null)
      {
        MyCampaignManager.Static.RunCampaign(this.m_launchScenarioInstance, platform: this.m_launchScenarioPlatform);
        return true;
      }
      if (this.m_launchScenario != null)
      {
        MyCampaignManager.Static.RunCampaign(this.m_launchScenario, false, this.m_launchScenarioPlatform);
        return true;
      }
      if (nullable.HasValue && !Sandbox.Engine.Platform.Game.IsDedicated && string.IsNullOrEmpty(Sandbox.Engine.Platform.Game.ConnectToServer))
      {
        if ((uint) nullable.Value > 1U)
          throw new InvalidBranchException();
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenStartQuickLaunch(nullable.Value, MyCommonTexts.StartGameInProgressPleaseWait));
      }
      else if (MyPlatformGameSettings.ENABLE_LOGOS)
      {
        MyGuiSandbox.BackToIntroLogos(new Action(this.ShowIntroMessages));
        MyGuiScreenInitialLoading.CloseInstance();
      }
      else
        this.ShowIntroMessages();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        bool flag = false;
        if (string.IsNullOrWhiteSpace(MySandboxGame.ConfigDedicated.WorldName))
        {
          string str1 = MyTexts.Get(MyCommonTexts.DefaultSaveName).ToString() + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
        else
          MySandboxGame.ConfigDedicated.WorldName.Trim();
        try
        {
          string lastSessionPath = MyLocalCache.GetLastSessionPath();
          if (!Sandbox.Engine.Platform.Game.IgnoreLastSession && !MySandboxGame.ConfigDedicated.IgnoreLastSession && lastSessionPath != null)
          {
            MyLog.Default.WriteLineAndConsole("Loading last session " + lastSessionPath);
            ulong sizeInBytes;
            MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(lastSessionPath, out sizeInBytes);
            if (MySession.IsCompatibleVersion(checkpoint))
            {
              if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
              {
                MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
                MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed. No UGC consent.");
                MySandboxGame.Static.Exit();
                return false;
              }
              if (MyWorkshop.DownloadWorldModsBlocking(checkpoint.Mods, cancelToken).Success)
              {
                MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
                MySession.Load(lastSessionPath, checkpoint, sizeInBytes);
                MySession.Static.StartServer(Sandbox.Engine.Multiplayer.MyMultiplayer.Static);
              }
              else
              {
                MyLog.Default.WriteLineAndConsole("Unable to download mods");
                MySandboxGame.Static.Exit();
                return false;
              }
            }
            else
            {
              MyLog.Default.WriteLineAndConsole(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
              MySandboxGame.Static.Exit();
              return false;
            }
          }
          else if (!string.IsNullOrEmpty(MySandboxGame.ConfigDedicated.LoadWorld))
          {
            string str2 = MySandboxGame.ConfigDedicated.LoadWorld;
            if (!Path.IsPathRooted(str2))
              str2 = Path.Combine(MyFileSystem.SavesPath, str2);
            if (Path.HasExtension(str2))
              str2 = Path.GetDirectoryName(str2);
            if (Directory.Exists(str2))
            {
              MySessionLoader.LoadDedicatedSession(str2, cancelToken);
            }
            else
            {
              MyLog.Default.WriteLineAndConsole("World " + Path.GetFileName(MySandboxGame.ConfigDedicated.LoadWorld) + " not found.");
              MyLog.Default.WriteLineAndConsole("Creating new one with same name");
              flag = true;
              Path.GetFileName(MySandboxGame.ConfigDedicated.LoadWorld);
            }
          }
          else
            flag = true;
          if (flag)
          {
            MyObjectBuilder_SessionSettings sessionSettings = MySandboxGame.ConfigDedicated.SessionSettings;
            if (MySandboxGame.ConfigDedicated.PremadeCheckpointPath.ToLower().EndsWith(".scf"))
            {
              MyCampaignManager.Static.RunCampaign(MySandboxGame.ConfigDedicated.PremadeCheckpointPath, platform: MySandboxGame.ConfigDedicated.WorldPlatform);
            }
            else
            {
              string path = MySandboxGame.ConfigDedicated.PremadeCheckpointPath;
              if (Path.HasExtension(path))
                path = Path.GetDirectoryName(path);
              if (MyFileSystem.DirectoryExists(path))
              {
                string str2 = MyLocalCache.GetSessionPathFromScenario(path, MySandboxGame.ConfigDedicated.WorldPlatform == "XBox", out bool _);
                if (string.IsNullOrEmpty(str2))
                  str2 = path;
                if (Path.HasExtension(str2))
                  str2 = Path.GetDirectoryName(str2);
                ulong sizeInBytes;
                MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(str2, out sizeInBytes);
                if (checkpoint == null)
                {
                  MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed.");
                  MySandboxGame.Static.Exit();
                  return false;
                }
                if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
                {
                  MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
                  MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed. No UGC consent.");
                  MySandboxGame.Static.Exit();
                  return false;
                }
                checkpoint.Settings = sessionSettings;
                checkpoint.SessionName = MySandboxGame.ConfigDedicated.WorldName;
                if (MyWorkshop.DownloadWorldModsBlocking(checkpoint.Mods, cancelToken).Success)
                {
                  MySession.Load(str2, checkpoint, sizeInBytes);
                  MySession.Static.Save(Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-')));
                  MySession.Static.StartServer(Sandbox.Engine.Multiplayer.MyMultiplayer.Static);
                }
                else
                {
                  MyLog.Default.WriteLineAndConsole("Unable to download mods");
                  MySandboxGame.Static.Exit();
                  return false;
                }
              }
              else
              {
                MyLog.Default.WriteLineAndConsole("Cannot start new world - Premade world not found " + MySandboxGame.ConfigDedicated.PremadeCheckpointPath);
                MySandboxGame.Static.Exit();
                return false;
              }
            }
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLineAndConsole("Exception while loading world: " + ex.Message);
          MyLog.Default.WriteLine(ex.StackTrace);
          MySandboxGame.Static.Exit();
          return false;
        }
      }
      if (!string.IsNullOrEmpty(Sandbox.Engine.Platform.Game.ConnectToServer) && MyGameService.IsActive)
      {
        MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(this.ServerResponded);
        MyGameService.OnPingServerFailedToRespond += new EventHandler(this.ServerFailedToRespond);
        MyGameService.PingServer(Sandbox.Engine.Platform.Game.ConnectToServer);
        Sandbox.Engine.Platform.Game.ConnectToServer = (string) null;
      }
      return true;
    }

    public void ServerResponded(object sender, MyGameServerItem serverItem)
    {
      MyLog.Default.WriteLineAndConsole("Server responded");
      this.CloseHandlers();
      MyJoinGameHelper.JoinGame(serverItem);
    }

    public void ServerFailedToRespond(object sender, object e)
    {
      MyLog.Default.WriteLineAndConsole("Server failed to respond");
      this.CloseHandlers();
    }

    private void CloseHandlers()
    {
      MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>(this.ServerResponded);
      MyGameService.OnPingServerFailedToRespond -= new EventHandler(this.ServerFailedToRespond);
    }

    public static void InitMultithreading()
    {
      ProfilerShort.Init();
      uint physicalCores;
      string infoCpu = MyVRage.Platform.System.GetInfoCPU(out uint _, out physicalCores);
      bool amd = infoCpu.StartsWith("AMD") || infoCpu.StartsWith("Xbox");
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MySandboxGame.Config.SyncRendering)
        MyFakes.FORCE_NO_WORKER = true;
      MySandboxGame.NumberOfCores = VRage.Library.MyEnvironment.ProcessorCount;
      if (MySandboxGame.NumberOfCores > 8 && physicalCores > 0U)
        MySandboxGame.NumberOfCores = (int) physicalCores;
      MySandboxGame.Log.WriteLine("Found processor count: " + (object) MySandboxGame.NumberOfCores);
      MySandboxGame.NumberOfCores = MyUtils.GetClampInt(MySandboxGame.NumberOfCores, 1, 16);
      MySandboxGame.Log.WriteLine("Using processor count: " + (object) MySandboxGame.NumberOfCores);
      if (MyFakes.FORCE_SINGLE_WORKER)
        ParallelTasks.Parallel.Scheduler = (IWorkScheduler) new FixedPriorityScheduler(1, ThreadPriority.Normal);
      else if (MyFakes.FORCE_NO_WORKER)
        ParallelTasks.Parallel.Scheduler = (IWorkScheduler) new FakeTaskScheduler();
      else
        ParallelTasks.Parallel.Scheduler = (IWorkScheduler) new PrioritizedScheduler(Math.Max(MySandboxGame.NumberOfCores / 2, 1), amd);
    }

    private void OnExit()
    {
      if (MySession.Static != null && MySpaceAnalytics.Instance != null)
      {
        MySpaceAnalytics.Instance.ReportWorldEnd();
        MySpaceAnalytics.Instance.ReportSessionEnd("Window closed");
      }
      MySandboxGame.ExitThreadSafe();
    }

    protected virtual IVRageWindow InitializeRenderThread()
    {
      this.DrawThread = Thread.CurrentThread;
      MyVRage.Platform.Windows.CreateWindow(MyPerGameSettings.GameName, MyPerGameSettings.GameIcon, MySandboxGame.Config.Language == MyLanguagesEnum.ChineseChina ? typeof (MyGuiControlContextMenu) : (System.Type) null);
      this.form = MyVRage.Platform.Windows.Window;
      MyVRage.Platform.Windows.Window.OnExit += new Action(this.OnExit);
      this.UpdateMouseCapture();
      if (MySandboxGame.Config.SyncRendering)
      {
        MyViewport viewport;
        ref MyViewport local = ref viewport;
        int? nullable = MySandboxGame.Config.ScreenWidth;
        double num1 = (double) nullable.Value;
        nullable = MySandboxGame.Config.ScreenHeight;
        double num2 = (double) nullable.Value;
        local = new MyViewport(0.0f, 0.0f, (float) num1, (float) num2);
        this.RenderThread_SizeChanged((int) viewport.Width, (int) viewport.Height, viewport);
      }
      return this.form;
    }

    protected void RenderThread_SizeChanged(int width, int height, MyViewport viewport)
    {
      Action<Vector2I> onScreenSize = this.OnScreenSize;
      if (onScreenSize != null)
        onScreenSize(new Vector2I(width, height));
      this.Invoke((Action) (() => MySandboxGame.UpdateScreenSize(width, height, viewport)), "MySandboxGame::UpdateScreenSize");
    }

    protected void RenderThread_BeforeDraw() => MyFpsManager.Update();

    private void ShowUpdateDriverDialog(MyAdapterInfo adapter)
    {
      switch (MyErrorReporter.ReportOldDrivers(MyPerGameSettings.GameName, adapter.DeviceName, adapter.DriverUpdateLink))
      {
        case VRage.MessageBoxResult.Yes:
          MySandboxGame.ExitThreadSafe();
          MyVRage.Platform.System.OpenUrl(adapter.DriverUpdateLink);
          break;
        case VRage.MessageBoxResult.No:
          MySandboxGame.Config.DisableUpdateDriverNotification = true;
          MySandboxGame.Config.Save();
          break;
      }
    }

    protected virtual void CheckGraphicsCard(
      MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
    {
      MyAdapterInfo adapter1 = msgVideoAdapters.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
      if (MyGpuIds.IsUnsupported(adapter1.VendorId, adapter1.DeviceId) || MyGpuIds.IsUnderMinimum(adapter1.VendorId, adapter1.DeviceId))
      {
        MySandboxGame.Log.WriteLine("Error: It seems that your graphics card is currently unsupported or it does not meet minimum requirements.");
        MySandboxGame.Log.WriteLine(string.Format("Graphics card name: {0}, vendor id: 0x{1:X}, device id: 0x{2:X}.", (object) adapter1.Name, (object) adapter1.VendorId, (object) adapter1.DeviceId));
        MyErrorReporter.ReportNotCompatibleGPU(MyPerGameSettings.GameName, MySandboxGame.Log.GetFilePath(), MyPerGameSettings.MinimumRequirementsPage);
      }
      if (MySandboxGame.Config.DisableUpdateDriverNotification)
        return;
      if (!adapter1.IsNvidiaNotebookGpu)
      {
        if (!adapter1.DriverUpdateNecessary)
          return;
        this.ShowUpdateDriverDialog(adapter1);
      }
      else
      {
        for (int index = 0; index < msgVideoAdapters.Adapters.Length; ++index)
        {
          MyAdapterInfo adapter2 = msgVideoAdapters.Adapters[index];
          if (adapter2.DriverUpdateNecessary)
            this.ShowUpdateDriverDialog(adapter2);
        }
      }
    }

    protected virtual bool Initialize()
    {
      bool flag1 = true;
      MySandboxGame.Log.WriteLine("MySandboxGame.Initialize() - START");
      MySandboxGame.Log.IncreaseIndent();
      MySandboxGame.Log.WriteLine("Installed DLCs: ");
      MySandboxGame.Log.IncreaseIndent();
      List<MyDlcPackage> packages = new List<MyDlcPackage>();
      foreach (KeyValuePair<uint, MyDLCs.MyDLC> dlC in MyDLCs.DLCs)
        packages.Add(new MyDlcPackage()
        {
          AppId = dlC.Value.AppId,
          PackageId = dlC.Value.PackageId,
          StoreId = dlC.Value.StoreId
        });
      MyGameService.AddDlcPackages(packages);
      if (!Sync.IsDedicated)
      {
        foreach (KeyValuePair<uint, MyDLCs.MyDLC> dlC in MyDLCs.DLCs)
        {
          if (MyGameService.IsDlcInstalled(dlC.Value.AppId))
            MySandboxGame.Log.WriteLine(string.Format("{0} ({1})", (object) MyTexts.GetString(dlC.Value.DisplayName), (object) dlC.Value.AppId));
        }
      }
      MySandboxGame.Log.DecreaseIndent();
      MyNetworkMonitor.Init();
      this.InitInput();
      this.InitSteamWorkshop();
      this.LoadData();
      MyOffensiveWordsDefinition wordsForPlatform = MyDefinitionManager.Static.GetOffensiveWordsForPlatform();
      MyOffensiveWords.Instance.Init(wordsForPlatform?.Blacklist, wordsForPlatform?.Whitelist);
      MyVisualScriptingProxy.Init();
      MyVisualScriptingProxy.RegisterLogicProvider(typeof (VRage.Game.VisualScripting.MyVisualScriptLogicProvider));
      MyVisualScriptingProxy.RegisterLogicProvider(typeof (Sandbox.Game.MyVisualScriptLogicProvider));
      bool flag2 = flag1 & this.InitQuickLaunch();
      MyObjectBuilder_ProfilerSnapshot.SetDelegates();
      this.InitServices();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MySandboxGame.Initialize() - END");
      return flag2;
    }

    protected virtual void InitServices()
    {
    }

    protected virtual void StartRenderComponent(
      MyRenderDeviceSettings? settingsToTry,
      IntPtr windowHandle)
    {
      if (settingsToTry.HasValue)
      {
        MyRenderDeviceSettings renderDeviceSettings = settingsToTry.Value;
        renderDeviceSettings.DisableWindowedModeForOldDriver = MySandboxGame.Config.DisableUpdateDriverNotification;
        settingsToTry = new MyRenderDeviceSettings?(renderDeviceSettings);
      }
      if (MySandboxGame.Config.SyncRendering)
        this.GameRenderComponent.StartSync(this.m_gameTimer, this.InitializeRenderThread(), settingsToTry, MyPerGameSettings.MaxFrameRate);
      else
        this.GameRenderComponent.Start(this.m_gameTimer, new InitHandler(this.InitializeRenderThread), settingsToTry, MyPerGameSettings.MaxFrameRate);
      this.GameRenderComponent.RenderThread.SizeChanged += new SizeChangedHandler(this.RenderThread_SizeChanged);
      this.GameRenderComponent.RenderThread.BeforeDraw += new Action(this.RenderThread_BeforeDraw);
    }

    public static void UpdateScreenSize(int width, int height, MyViewport viewport)
    {
      MySandboxGame.ScreenSize = new Vector2I(width, height);
      MySandboxGame.ScreenSizeHalf = new Vector2I(MySandboxGame.ScreenSize.X / 2, MySandboxGame.ScreenSize.Y / 2);
      MySandboxGame.ScreenViewport = viewport;
      if (MyGuiManager.UpdateScreenSize(MySandboxGame.ScreenSize, MySandboxGame.ScreenSizeHalf, MyVideoSettingsManager.IsTripleHead(MySandboxGame.ScreenSize)))
        MyScreenManager.RecreateControls();
      if (MySector.MainCamera != null)
        MySector.MainCamera.UpdateScreenSize(MySandboxGame.ScreenViewport);
      MySandboxGame.CanShowHotfixPopup = true;
      MySandboxGame.CanShowWhitelistPopup = true;
    }

    private static void Preallocate()
    {
      MySandboxGame.Log.WriteLine("Preallocate - START");
      MySandboxGame.Log.IncreaseIndent();
      System.Type[] types = new System.Type[6]
      {
        typeof (Sandbox.Game.Entities.MyEntities),
        typeof (MyObjectBuilder_Base),
        typeof (MyTransparentGeometry),
        typeof (MyCubeGridDeformationTables),
        typeof (MyMath),
        typeof (MySimpleObjectDraw)
      };
      try
      {
        MySandboxGame.PreloadTypesFrom(MyPlugins.GameAssembly);
        MySandboxGame.PreloadTypesFrom(MyPlugins.SandboxAssembly);
        MySandboxGame.PreloadTypesFrom(MyPlugins.UserAssemblies);
        MySandboxGame.ForceStaticCtor(types);
        MySandboxGame.PreloadTypesFrom(typeof (MySandboxGame).Assembly);
      }
      catch (ReflectionTypeLoadException ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Exception loaderException in ex.LoaderExceptions)
        {
          stringBuilder.AppendLine(loaderException.Message);
          if (loaderException is FileNotFoundException)
          {
            FileNotFoundException notFoundException = loaderException as FileNotFoundException;
            if (!string.IsNullOrEmpty(notFoundException.FusionLog))
            {
              stringBuilder.AppendLine("Fusion Log:");
              stringBuilder.AppendLine(notFoundException.FusionLog);
            }
          }
          stringBuilder.AppendLine();
        }
        stringBuilder.ToString();
      }
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("Preallocate - END");
    }

    private static void PreloadTypesFrom(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MySandboxGame.PreloadTypesFrom(assembly);
    }

    private static void PreloadTypesFrom(Assembly assembly)
    {
      if (!(assembly != (Assembly) null))
        return;
      MySandboxGame.ForceStaticCtor(((IEnumerable<System.Type>) assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (type => Attribute.IsDefined((MemberInfo) type, typeof (PreloadRequiredAttribute)))).ToArray<System.Type>());
    }

    public static void ForceStaticCtor(System.Type[] types)
    {
      foreach (System.Type type in types)
      {
        MySandboxGame.Log.WriteLine(type.Name + " - START");
        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        MySandboxGame.Log.WriteLine(type.Name + " - END");
      }
    }

    private void LoadData()
    {
      if (MySession.Static != null)
        MySession.Static.SetAsNotReady();
      else if (MyAudio.Static != null)
        MyAudio.Static.Mute = false;
      if (MyInput.Static != null)
        MyInput.Static.LoadContent();
      if (!MyInput.Static.IsDirectInputInitialized)
      {
        int num = (int) MyMessageBox.Show(MyTexts.GetString(MyCoreTexts.Error_DirectInputNotInitialized_Description), MyTexts.GetString(MyCoreTexts.Error_DirectInputNotInitialized), MessageBoxOptions.IconExclamation);
        throw new ApplicationException("DirectX Input was not initialized. See previous errors in log for more information.");
      }
      HkBaseSystem.Init(16777216, new Action<string>(this.LogWriter), false);
      this.WriteHavokCodeToLog();
      ParallelTasks.Parallel.StartOnEachWorker((Action) (() => HkBaseSystem.InitThread(Thread.CurrentThread.Name)));
      MyPhysicsDebugDraw.DebugGeometry = new HkGeometry();
      MySandboxGame.Log.WriteLine("MySandboxGame.LoadData() - START");
      MySandboxGame.Log.IncreaseIndent();
      if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
        MyDefinitionManager.Static.LoadScenarios();
      MyDefinitionManager.Static.PreloadDefinitions();
      MyAudio.LoadData(new MyAudioInitParams()
      {
        Instance = Sandbox.Engine.Platform.Game.IsDedicated ? (IMyAudio) new MyNullAudio() : MyVRage.Platform.Audio,
        SimulateNoSoundCard = MyFakes.SIMULATE_NO_SOUND_CARD,
        DisablePooling = MyFakes.DISABLE_SOUND_POOLING,
        CacheLoaded = true,
        OnSoundError = MyAudioExtensions.OnSoundError
      }, MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
      if (MyPerGameSettings.UseVolumeLimiter)
        MyAudio.Static.UseVolumeLimiter = true;
      if (MyPerGameSettings.UseSameSoundLimiter)
      {
        MyAudio.Static.UseSameSoundLimiter = true;
        MyAudio.Static.SetSameSoundLimiter();
      }
      if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
      {
        if (MySandboxGame.Config.EnableReverb && MyAudio.Static.SampleRate > MyAudio.MAX_SAMPLE_RATE)
          MySandboxGame.Config.EnableReverb = false;
        else
          MyAudio.Static.EnableReverb = MySandboxGame.Config.EnableReverb;
      }
      else
      {
        MySandboxGame.Config.EnableReverb = false;
        MyAudio.Static.EnableReverb = false;
      }
      this.UpdateUIScale();
      this.UpdateAudioSettings();
      MyGuiSoundManager.Audio = MyGuiAudio.Static;
      MySandboxGame.StartPreload();
      if (EmptyKeys.UserInterface.Engine.Instance != null && EmptyKeys.UserInterface.Engine.Instance.AudioDevice is MyAudioDevice audioDevice)
        audioDevice.GuiAudio = MyGuiAudio.Static;
      MyLocalization.Initialize();
      MyGuiSandbox.LoadData(Sandbox.Engine.Platform.Game.IsDedicated);
      this.LoadGui();
      this.m_dataLoadedDebug = true;
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
      {
        MyGameService.LobbyJoinRequested += new MyLobbyJoinRequested(this.Matchmaking_LobbyJoinRequest);
        MyGameService.ServerChangeRequested += new MyServerChangeRequested(this.Matchmaking_ServerChangeRequest);
      }
      MyInput.Static.LoadData(MySandboxGame.Config.ControlsGeneral, MySandboxGame.Config.ControlsButtons);
      this.InitJoystick();
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        MyParticlesManager.Enabled = false;
      Func<Vector3D, Vector3> calculateGravityInPoint = new Func<Vector3D, Vector3>(MyGravityProviderSystem.CalculateTotalGravityInPoint);
      MyRenderProxy.SetGravityProvider(calculateGravityInPoint);
      MyParticlesManager.CalculateGravityInPoint = calculateGravityInPoint;
      MySectorWeatherComponent.CalculateGravityInPoint = calculateGravityInPoint;
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MySandboxGame.LoadData() - END");
      this.InitModAPI();
      MyPositionComponentBase.OnReportInvalidMatrix += new Action<VRage.ModAPI.IMyEntity>(this.ReportInvalidMatrix);
      EventHandler onGameLoaded = this.OnGameLoaded;
      if (onGameLoaded == null)
        return;
      onGameLoaded((object) this, (System.EventArgs) null);
    }

    private void ReportInvalidMatrix(VRage.Game.ModAPI.Ingame.IMyEntity entity)
    {
      if (!(entity is MyEntity) || !Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.SendLogToKeen)
        return;
      if (MySession.Static.Players.GetEntityController((MyEntity) entity) == null)
      {
        MyLog log = MySandboxGame.Log;
        string message = entity.Name;
        if (message == null)
          message = entity.ToString() + " with ID:" + (object) entity.EntityId + " has invalid world matrix! Deleted.";
        object[] objArray = Array.Empty<object>();
        log.Error(message, objArray);
        ((MyEntity) entity).Close();
      }
      MyReportException myReportException = new MyReportException();
      MyLog.Default.WriteLineAndConsole("Exception with invalid matrix");
      MyLog.Default.WriteLine(myReportException.ToString());
      MyLog.Default.WriteLine(Environment.StackTrace);
      MySandboxGame.NonInteractiveReportAction interactiveReport = MySandboxGame.PerformNotInteractiveReport;
      if (interactiveReport == null)
        return;
      interactiveReport(true);
    }

    public static void StartPreload()
    {
      ref ParallelTasks.Task? local = ref MySandboxGame.m_currentPreloadingTask;
      if (local.HasValue)
        local.GetValueOrDefault().WaitOrExecute();
      MySandboxGame.m_currentPreloadingTask = new ParallelTasks.Task?(ParallelTasks.Parallel.Start(new Action(MySandboxGame.PerformPreloading)));
    }

    public static void WaitForPreload()
    {
      ref ParallelTasks.Task? local = ref MySandboxGame.m_currentPreloadingTask;
      if (!local.HasValue)
        return;
      local.GetValueOrDefault().WaitOrExecute();
    }

    private static void PerformPreloading()
    {
      MySandboxGame.Log.WriteLine("MySandboxGame.PerformPreload() - START");
      ParallelTasks.Task? nullable = Sandbox.Engine.Multiplayer.MyMultiplayer.InitOfflineReplicationLayer();
      MyMath.InitializeFastSin();
      List<Tuple<MyObjectBuilder_Definitions, string>> tupleList = (List<Tuple<MyObjectBuilder_Definitions, string>>) null;
      try
      {
        tupleList = MyDefinitionManager.Static.GetSessionPreloadDefinitions();
      }
      catch (MyLoadingException ex)
      {
        string message = ex.Message;
        MySandboxGame.Log.WriteLineAndConsole(message);
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(message), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(MySandboxGame.ClosePopup));
        Vector2 vector2 = messageBox.Size.Value;
        vector2.Y *= 1.5f;
        messageBox.Size = new Vector2?(vector2);
        messageBox.RecreateControls(false);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
      if (MySandboxGame.Static.Exiting)
        return;
      if (tupleList != null)
      {
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          MyGuiTextures.Static.Reload((IEnumerable<string>) new string[1]
          {
            "Textures\\GUI\\screens\\screen_loading_wheel.dds"
          });
        MySandboxGame.Log.WriteLine("MySandboxGame.PerformPreload() - PRELOAD VANILLA SOUNDS AND VOXELS");
        List<MyObjectBuilder_AudioDefinition> sounds = new List<MyObjectBuilder_AudioDefinition>();
        List<MyObjectBuilder_VoxelMapStorageDefinition> voxels = new List<MyObjectBuilder_VoxelMapStorageDefinition>();
        foreach (Tuple<MyObjectBuilder_Definitions, string> tuple in tupleList)
        {
          MyObjectBuilder_Definitions builderDefinitions = tuple.Item1;
          if (builderDefinitions.Sounds != null && !Sandbox.Engine.Platform.Game.IsDedicated)
            sounds.AddRange((IEnumerable<MyObjectBuilder_AudioDefinition>) builderDefinitions.Sounds);
          if (builderDefinitions.VoxelMapStorages != null && MyFakes.ENABLE_ASTEROIDS)
            voxels.AddRange((IEnumerable<MyObjectBuilder_VoxelMapStorageDefinition>) builderDefinitions.VoxelMapStorages);
        }
        MyDefinitionManager.Static.ReloadVoxelMaterials();
        string defaultAsteroidGeneratorVersion = MyPlatformGameSettings.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR.ToString();
        ParallelTasks.Parallel.For(0, voxels.Count + sounds.Count, (Action<int>) (i =>
        {
          if (MySandboxGame.Static.Exiting)
            return;
          if (i < voxels.Count)
          {
            MyObjectBuilder_VoxelMapStorageDefinition storageDefinition = voxels[i];
            if (string.IsNullOrEmpty(storageDefinition.StorageFile))
              return;
            string[] generatorVersions = storageDefinition.ExplicitProceduralGeneratorVersions;
            if ((generatorVersions != null ? (!generatorVersions.Contains<string>(defaultAsteroidGeneratorVersion) ? 1 : 0) : 0) != 0 || !storageDefinition.UseAsPrimaryProceduralAdditionShape && !storageDefinition.UseForProceduralAdditions && !storageDefinition.UseForProceduralRemovals)
              return;
            MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(Path.Combine(MyFileSystem.ContentPath, storageDefinition.StorageFile));
            if (!MyVRage.Platform.System.IsMemoryLimited)
              return;
            myStorageBase.ResetDataCache();
          }
          else
          {
            MyObjectBuilder_AudioDefinition builderAudioDefinition = sounds[i - voxels.Count];
            if (MyAudio.Static == null)
              return;
            MyStringHash.GetOrCompute(builderAudioDefinition.Id.SubtypeName);
            foreach (MyAudioWave wave in builderAudioDefinition.Waves)
            {
              if (!string.IsNullOrEmpty(wave.Start))
                MyAudio.Static.Preload(wave.Start);
              if (!string.IsNullOrEmpty(wave.Loop))
                MyAudio.Static.Preload(wave.Loop);
              if (!string.IsNullOrEmpty(wave.End))
                MyAudio.Static.Preload(wave.End);
            }
          }
        }), WorkPriority.VeryLow);
        MyDefinitionManager.Static.GetAllSessionPreloadObjectBuilders();
        MySandboxGame.Log.WriteLine("MySandboxGame.PerformPreload() - END VANILLA SOUNDS AND VOXELS");
        nullable?.WaitOrExecute();
      }
      else if (!Sandbox.Engine.Platform.Game.IsDedicated)
        MyGuiTextures.Static.Reload(preload: MyFakes.ENABLE_PRELOAD_DEFINITIONS);
      MySandboxGame.Log.WriteLine("MySandboxGame.PerformPreload() - END");
    }

    private BoundingFrustumD GetCameraFrustum() => MySector.MainCamera == null ? new BoundingFrustumD(MatrixD.Identity) : MySector.MainCamera.BoundingFrustumFar;

    protected virtual void LoadGui() => MyGuiSandbox.LoadContent();

    private void WriteHavokCodeToLog()
    {
      MySandboxGame.Log.WriteLine("HkGameName: " + HkBaseSystem.GameName);
      foreach (string keyCode in HkBaseSystem.GetKeyCodes())
      {
        if (!string.IsNullOrWhiteSpace(keyCode))
          MySandboxGame.Log.WriteLine("HkCode: " + keyCode);
      }
    }

    private void InitModAPI()
    {
      try
      {
        if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
          return;
        this.InitIlCompiler();
        MySandboxGame.InitIlChecker();
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.Error("Error during ModAPI initialization: {0}", (object) ex.Message);
        MySandboxGame.ShowHotfixPopup = true;
      }
    }

    private static void OnDotNetHotfixPopupClosed(MyGuiScreenMessageBox.ResultEnum result)
    {
      Process.Start("https://support.microsoft.com/kb/3120241");
      MySandboxGame.ClosePopup(result);
    }

    private static void OnWhitelistIntegrityPopupClosed(MyGuiScreenMessageBox.ResultEnum result) => MySandboxGame.ClosePopup(result);

    private static void ClosePopup(MyGuiScreenMessageBox.ResultEnum result) => Process.GetCurrentProcess().Kill();

    private void InitIlCompiler()
    {
      MySandboxGame.Log.IncreaseIndent();
      if (MySandboxGame.GameCustomInitialization != null)
        MySandboxGame.GameCustomInitialization.InitIlCompiler();
      MySandboxGame.Log.DecreaseIndent();
    }

    internal static void InitIlChecker()
    {
      if (MySandboxGame.GameCustomInitialization != null)
        MySandboxGame.GameCustomInitialization.InitIlChecker();
      using (IMyWhitelistBatch myWhitelistBatch = MyVRage.Platform.Scripting.OpenWhitelistBatch())
      {
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, (MemberInfo) typeof (MyCubeBuilder).GetField("Static"), (MemberInfo) typeof (MyCubeBuilder).GetProperty("CubeBuilderState"), (MemberInfo) typeof (MyCubeBuilderState).GetProperty("CurrentBlockDefinition"), (MemberInfo) typeof (MyHud).GetField("BlockInfo"));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (MyHudBlockInfo), typeof (MyHudBlockInfo.ComponentInfo), typeof (MyObjectBuilder_CubeBuilderDefinition), typeof (MyPlacementSettings), typeof (MyGridPlacementSettings), typeof (SnapMode), typeof (VoxelPlacementMode), typeof (VoxelPlacementSettings));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof (ListExtensions), typeof (VRage.Game.ModAPI.Ingame.IMyCubeBlock), typeof (MyIni), typeof (Sandbox.ModAPI.Ingame.IMyTerminalBlock), typeof (Vector3), typeof (MySprite));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof (MyAPIUtilities), typeof (Sandbox.ModAPI.Interfaces.ITerminalAction), typeof (IMyTerminalAction), typeof (VRage.Game.ModAPI.IMyCubeBlock), typeof (MyAPIGateway), typeof (IMyCameraController), typeof (VRage.ModAPI.IMyEntity), typeof (MyEntity), typeof (MyEntityExtensions), typeof (EnvironmentItemsEntry), typeof (MyObjectBuilder_GasProperties), typeof (MyObjectBuilder_AdvancedDoor), typeof (MyObjectBuilder_AdvancedDoorDefinition), typeof (MyObjectBuilder_ComponentBase), typeof (MyObjectBuilder_Base), typeof (MyIngameScript), typeof (MyResourceSourceComponent), typeof (MyCharacterOxygenComponent), typeof (IMyUseObject), typeof (IMyModelDummy), typeof (IMyTextSurfaceScript), typeof (MyObjectBuilder_SafeZoneBlock));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (MyBillboard.BlendTypeEnum));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof (MyObjectBuilder_EntityStatRegenEffect));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof (MyStatLogic), typeof (MyEntityStatComponent), typeof (MyEnvironmentSector), typeof (SerializableVector3), typeof (MyDefinitionManager), typeof (MyFixedPoint), typeof (ListReader<>), typeof (MyStorageData), typeof (MyEventArgs), typeof (MyStringId), typeof (MyGameTimer), typeof (MyLight), typeof (IMyAutomaticRifleGun), typeof (MyContractAcquisition));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, (MemberInfo) typeof (MySpectatorCameraController).GetProperty("IsLightOn"));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof (TerminalActionExtensions), typeof (Sandbox.ModAPI.Interfaces.ITerminalAction), typeof (ITerminalProperty), typeof (ITerminalProperty<>), typeof (TerminalPropertyExtensions), typeof (MySpaceTexts), typeof (StringBuilderExtensions_Format), typeof (MyFixedPoint));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof (MyTuple), typeof (MyTuple<>), typeof (MyTuple<,>), typeof (MyTuple<,,>), typeof (MyTuple<,,,>), typeof (MyTuple<,,,,>), typeof (MyTuple<,,,,,>), typeof (MyTupleComparer<,>), typeof (MyTupleComparer<,,>));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof (MyTexts.MyLanguageDescription), typeof (MyLanguagesEnum));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof (VRage.ModAPI.IMyInput));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (MyInputExtensions), typeof (MyKeys), typeof (MyJoystickAxesEnum), typeof (MyJoystickButtonsEnum), typeof (MyMouseButtonsEnum), typeof (MySharedButtonsEnum), typeof (MyGuiControlTypeEnum), typeof (MyGuiInputDeviceEnum));
        IEnumerable<MethodInfo> source = ((IEnumerable<MethodInfo>) typeof (MyComponentContainer).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.Name == "TryGet" && method.ContainsGenericParameters && method.GetParameters().Length == 1));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, (MemberInfo) source.FirstOrDefault<MethodInfo>(), (MemberInfo) typeof (MyComponentContainer).GetMethod("Has"), (MemberInfo) typeof (MyComponentContainer).GetMethod("Get"), (MemberInfo) typeof (MyComponentContainer).GetMethod("TryGet", new System.Type[2]
        {
          typeof (System.Type),
          typeof (MyComponentBase).MakeByRefType()
        }));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.Ingame, typeof (ListReader<>), typeof (MyDefinitionId), typeof (MyRelationsBetweenPlayerAndBlock), typeof (MyRelationsBetweenPlayerAndBlockExtensions), typeof (MyResourceSourceComponentBase), typeof (MyObjectBuilder_GasProperties), typeof (SerializableDefinitionId), typeof (MyCubeSize));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, (MemberInfo) typeof (MyComponentBase).GetMethod("GetAs"), (MemberInfo) typeof (MyComponentBase).GetProperty("ContainerBase"));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, (MemberInfo) typeof (MyObjectBuilder_Base).GetProperty("TypeId"), (MemberInfo) typeof (MyObjectBuilder_Base).GetProperty("SubtypeId"));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("CurrentOutput"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("MaxOutput"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("DefinedOutput"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("ProductionEnabled"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("RemainingCapacity"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("HasCapacityRemaining"), (MemberInfo) typeof (MyResourceSourceComponent).GetProperty("ResourceTypes"), (MemberInfo) typeof (MyResourceSinkComponent).GetProperty("AcceptedResources"), (MemberInfo) typeof (MyResourceSinkComponent).GetProperty("RequiredInput"), (MemberInfo) typeof (MyResourceSinkComponent).GetProperty("SuppliedRatio"), (MemberInfo) typeof (MyResourceSinkComponent).GetProperty("CurrentInput"), (MemberInfo) typeof (MyResourceSinkComponent).GetProperty("IsPowered"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetProperty("AcceptedResources"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("CurrentInputByType"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("IsPowerAvailable"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("IsPoweredByType"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("MaxRequiredInputByType"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("RequiredInputByType"), (MemberInfo) typeof (MyResourceSinkComponentBase).GetMethod("SuppliedRatioByType"));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (MyPhysicsHelper), typeof (MyPhysics.CollisionLayers));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (MyLodTypeEnum), typeof (MyMaterialsSettings), typeof (MyShadowsSettings), typeof (MyPostprocessSettings), typeof (MyHBAOData), typeof (MySSAOSettings), typeof (MyEnvironmentLightData), typeof (MyEnvironmentData), typeof (MyPostprocessSettings.Layout), typeof (MySSAOSettings.Layout), typeof (MyShadowsSettings.Struct), typeof (MyShadowsSettings.Cascade), typeof (MyMaterialsSettings.Struct), typeof (MyMaterialsSettings.MyChangeableMaterial), typeof (MyGlareTypeEnum), typeof (SerializableDictionary<,>), typeof (MyToolBase), typeof (MyGunBase), typeof (MyDeviceBase), typeof (Stopwatch), typeof (ConditionalAttribute), typeof (Version), typeof (ObsoleteAttribute));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (IWork), typeof (ParallelTasks.Task), typeof (WorkOptions), typeof (VRage.Library.Threading.SpinLock), typeof (SpinLockRef), typeof (Monitor), typeof (AutoResetEvent), typeof (ManualResetEvent), typeof (Interlocked));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof (ProtoMemberAttribute), typeof (ProtoContractAttribute), typeof (ProtoIncludeAttribute), typeof (ProtoIgnoreAttribute), typeof (ProtoEnumAttribute), typeof (MemberSerializationOptions), typeof (DataFormat));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, (MemberInfo) typeof (WorkData).GetMethod("FlagAsFailed"));
        myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, (MemberInfo) typeof (ArrayExtensions).GetMethod("Contains"));
        myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof (MyPhysicalInventoryItemExtensions_ModAPI));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof (ImmutableArray));
      }
    }

    private void Matchmaking_LobbyJoinRequest(
      IMyLobby lobby,
      ulong invitedBy,
      string invitedByName)
    {
      if (this.m_isPendingLobbyInvite || !lobby.IsValid || MySession.Static != null && Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null && (long) Sandbox.Engine.Multiplayer.MyMultiplayer.Static.LobbyId == (long) lobby.LobbyId)
        return;
      this.m_isPendingLobbyInvite = true;
      this.m_invitingLobby = lobby;
      if ((long) invitedBy == (long) MyGameService.UserId)
      {
        this.OnAcceptLobbyInvite(MyGuiScreenMessageBox.ResultEnum.YES);
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.InvitedToLobbyCaption);
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnAcceptLobbyInvite);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.InvitedToLobby), (object) invitedByName)), messageCaption, callback: callback));
      }
    }

    private void Matchmaking_ServerChangeRequest(string server, string password)
    {
      MySessionLoader.UnloadAndExitToMenu();
      MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(this.ServerResponded);
      MyGameService.OnPingServerFailedToRespond += new EventHandler(this.ServerFailedToRespond);
      MyGameService.PingServer(server);
    }

    public void OnAcceptLobbyInvite(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (!this.m_isPendingLobbyInvite)
        return;
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MySessionLoader.UnloadAndExitToMenu();
        if (this.m_invitingLobby.ConnectionStrategy == ConnectionStrategy.ConnectImmediately)
          MyJoinGameHelper.JoinGame(this.m_invitingLobby.LobbyId);
        else
          MyJoinGameHelper.JoinGame(this.m_invitingLobby);
      }
      this.m_isPendingLobbyInvite = false;
    }

    private void UnloadData()
    {
      MySandboxGame.Log.WriteLine("MySandboxGame.UnloadData() - START");
      MySandboxGame.Log.IncreaseIndent();
      this.UnloadAudio();
      this.UnloadInput();
      MyAudio.UnloadData();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MySandboxGame.UnloadData() - END");
      MyModels.UnloadData();
      MyGuiSandbox.UnloadContent();
    }

    private void UnloadAudio()
    {
      if (MyAudio.Static == null)
        return;
      MyAudio.Static.Mute = true;
      MyEntity3DSoundEmitter.ClearEntityEmitters();
      MyAudio.Static.ClearSounds();
      MyHud.ScreenEffects.FadeScreen(1f);
    }

    private void UnloadInput()
    {
      MyInput.UnloadData();
      MyGuiGameControlsHelpers.Reset();
    }

    public static bool IsPaused
    {
      get => MySandboxGame.m_isPaused;
      set
      {
        if (!Sync.MultiplayerActive || !Sync.IsServer)
        {
          if (MySandboxGame.m_isPaused != value)
          {
            MySandboxGame.m_isPaused = value;
            if (MySandboxGame.IsPaused)
              MySandboxGame.m_pauseStartTimeInMilliseconds = MySandboxGame.TotalSimulationTimeInMilliseconds;
            else
              MySandboxGame.m_totalPauseTimeInMilliseconds += MySandboxGame.TotalSimulationTimeInMilliseconds - MySandboxGame.m_pauseStartTimeInMilliseconds;
            MyRenderProxy.PauseTimer(value);
          }
        }
        else
        {
          if (MySandboxGame.m_isPaused)
            MyAudio.Static.ResumeGameSounds();
          MySandboxGame.m_isPaused = false;
        }
        if (!MySandboxGame.m_isPaused)
          MySandboxGame.m_pauseStackCount = 0;
        MyParticlesManager.Paused = MySandboxGame.m_isPaused;
      }
    }

    public static void PausePush() => MySandboxGame.UpdatePauseState(++MySandboxGame.m_pauseStackCount);

    public static void PausePop()
    {
      --MySandboxGame.m_pauseStackCount;
      if (MySandboxGame.m_pauseStackCount < 0)
        MySandboxGame.m_pauseStackCount = 0;
      MySandboxGame.UpdatePauseState(MySandboxGame.m_pauseStackCount);
    }

    public static void PauseToggle()
    {
      if (Sync.MultiplayerActive)
        return;
      if (MySandboxGame.IsPaused)
        MySandboxGame.PausePop();
      else
        MySandboxGame.PausePush();
    }

    [Conditional("DEBUG")]
    public static void AssertUpdateThread()
    {
    }

    private static void UpdatePauseState(int pauseStackCount)
    {
      if (pauseStackCount > 0)
        MySandboxGame.IsPaused = true;
      else
        MySandboxGame.IsPaused = false;
    }

    protected override void Update()
    {
      if (MySandboxGame.IsRenderUpdateSyncEnabled && this.GameRenderComponent != null && this.GameRenderComponent.RenderThread != null)
      {
        if (this.GameRenderComponent.RenderThread.RenderUpdateSyncEvent != null)
        {
          this.GameRenderComponent.RenderThread.RenderUpdateSyncEvent.Set();
          this.GameRenderComponent.RenderThread.RenderUpdateSyncEvent.Reset();
        }
        else
          this.GameRenderComponent.RenderThread.RenderUpdateSyncEvent = new ManualResetEvent(false);
      }
      MyVRage.Platform.Update();
      long remainingMemoryForGame = MyVRage.Platform.System.RemainingMemoryForGame;
      if (remainingMemoryForGame < 100L)
      {
        if (this.MemoryState != MySandboxGame.MemState.Critical)
        {
          this.MemoryState = MySandboxGame.MemState.Critical;
          MyLog.Default.WriteLine("Game is at critically low memory");
          if (MyPlatformGameSettings.ENABLE_LOW_MEM_WORLD_LOCKDOWN && Sync.IsServer && (!Sync.IsDedicated && MySession.Static != null))
          {
            foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
            {
              if (!onlinePlayer.IsLocalPlayer)
                Sandbox.Engine.Multiplayer.MyMultiplayer.Static?.KickClient(onlinePlayer.Id.SteamId, add: false);
            }
            if (!MyScreenManager.IsScreenOfTypeOpen(MyPerGameSettings.GUI.AdminMenuScreen))
              MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
          }
        }
      }
      else if (this.MemoryState != MySandboxGame.MemState.Critical || remainingMemoryForGame > 150L)
        this.MemoryState = remainingMemoryForGame >= 200L ? MySandboxGame.MemState.Normal : MySandboxGame.MemState.Low;
      bool joystickLastUsed = MyInput.Static.IsJoystickLastUsed;
      if (joystickLastUsed != this.m_joystickLastUsed)
        this.m_joystickLastUsed = joystickLastUsed;
      if (MySandboxGame.ShowHotfixPopup && MySandboxGame.CanShowHotfixPopup)
      {
        MySandboxGame.ShowHotfixPopup = false;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.ErrorPopup_Hotfix_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ErrorPopup_Hotfix_Text), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(MySandboxGame.OnDotNetHotfixPopupClosed), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
      }
      if (MySandboxGame.ShowWhitelistPopup && MySandboxGame.CanShowWhitelistPopup)
      {
        MySandboxGame.ShowHotfixPopup = false;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.ErrorPopup_Whitelist_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ErrorPopup_Whitelist_Text), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(MySandboxGame.OnWhitelistIntegrityPopupClosed), focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
      }
      long timestamp = Stopwatch.GetTimestamp();
      long num = timestamp - MySandboxGame.m_lastFrameTimeStamp;
      MySandboxGame.m_lastFrameTimeStamp = timestamp;
      MySandboxGame.SecondsSinceLastFrame = MyRandom.EnableDeterminism ? 0.0166666675359011 : (double) num / (double) Stopwatch.Frequency;
      if (MySandboxGame.ShowIsBetterGCAvailableNotification)
      {
        MySandboxGame.ShowIsBetterGCAvailableNotification = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.BetterGCIsAvailable), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
      }
      if (MySandboxGame.ShowGpuUnderMinimumNotification)
      {
        MySandboxGame.ShowGpuUnderMinimumNotification = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.GpuUnderMinimumNotification), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
      }
      this.form?.UpdateMainThread();
      ref ParallelTasks.Task? local = ref this.m_soundUpdate;
      if (local.HasValue)
        local.GetValueOrDefault().WaitOrExecute();
      this.m_soundUpdate = new ParallelTasks.Task?();
      using (Stats.Generic.Measure("InvokeQueue"))
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          this.ProcessInvoke();
      }
      MyGeneralStats.Static.Update();
      if (MySandboxGame.Config.SyncRendering)
      {
        if (MySandboxGame.IsVideoRecordingEnabled && MySession.Static != null && MySandboxGame.IsGameReady)
        {
          string pathToSave = Path.Combine(MyFileSystem.UserDataPath, "Recording", "img_" + this.SimulationFrameCounter.ToString("D8") + ".png");
          MyRenderProxy.TakeScreenshot(Vector2.One, pathToSave, false, true, false);
        }
        this.GameRenderComponent.RenderThread.TickSync();
      }
      using (Stats.Generic.Measure("RenderRequests"))
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          MySandboxGame.ProcessRenderOutput();
      }
      using (Stats.Generic.Measure("Network"))
      {
        if (Sync.Layer != null)
          Sync.Layer.TransportLayer.Tick();
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          MyGameService.Update();
        MyNetworkMonitor.Update();
        try
        {
          using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
            MyNetworkReader.Process();
        }
        catch (MyIncompatibleDataException ex)
        {
          Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Dispose();
          MySessionLoader.UnloadAndExitToMenu();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.IncompatibleDataNotification), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
        }
      }
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReportReplicatedObjects();
      using (Stats.Generic.Measure("GuiUpdate"))
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          MyGuiSandbox.Update(16);
      }
      foreach (IHandleInputPlugin handleInputPlugin in MyPlugins.HandleInputPlugins)
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          handleInputPlugin.HandleInput();
      }
      using (Stats.Generic.Measure("Input"))
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
        {
          MyGuiSandbox.HandleInput();
          if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
            MySession.Static.HandleInput();
          this.CheckIfControllerIsConnected();
        }
      }
      using (Stats.Generic.Measure("GameLogic"))
      {
        if (MySession.Static != null)
        {
          bool flag = true;
          if (Sandbox.Engine.Platform.Game.IsDedicated && MySandboxGame.ConfigDedicated.PauseGameWhenEmpty)
            flag = Sync.Clients.Count > 1 || !MySession.Static.Ready;
          if (flag)
          {
            using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
              MySession.Static.Update(this.TotalTime);
          }
        }
      }
      using (Stats.Generic.Measure("InputAfter"))
      {
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
        {
          using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
            MyGuiSandbox.HandleInputAfterSimulation();
        }
      }
      if (MyFakes.SIMULATE_SLOW_UPDATE)
        Thread.Sleep(40);
      foreach (IPlugin plugin in MyPlugins.Plugins)
      {
        using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
          plugin.Update();
      }
      using (Stats.Generic.Measure("Audio"))
      {
        if (!this.Exiting)
          this.m_soundUpdate = new ParallelTasks.Task?(ParallelTasks.Parallel.Start(new Action(this.UpdateSound)));
      }
      base.Update();
      MyGameStats.Static.Update();
      MySpaceAnalytics.Instance?.Update(this.TotalTime);
      if (this.m_unpauseInput && (DateTime.Now - MySandboxGame.Static.m_inputPauseTime).TotalMilliseconds >= 10.0)
      {
        MySandboxGame.Static.m_unpauseInput = false;
        MySandboxGame.Static.PauseInput = false;
      }
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicationLayer != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.ReplicationLayer.AdvanceSyncTime();
      if (Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
      {
        this.CheckAutoUpdateForDedicatedServer();
        this.CheckAutoRestartForDedicatedServer();
      }
      if (MyFakes.ENABLE_TESTING_TOOL_HEPER)
        MyTestingToolHelper.Instance.Update();
      MyStatsGraph.Commit();
    }

    private void OnExperimentalOutOfMemoryCrashMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MySandboxGame.Config.ExperimentalMode = false;
      MySandboxGame.Config.Save();
    }

    private void CheckIfControllerIsConnected()
    {
      if (!this.m_isControllerErrorMessageBoxVisible && MyInput.Static.IsJoystickLastUsed && (!MyInput.Static.IsJoystickConnected() && !string.IsNullOrEmpty(MyInput.Static.JoystickInstanceName)))
      {
        ++this.m_controllerCheckFrameCounter;
        if (this.m_framesToShowControllerError > this.m_controllerCheckFrameCounter)
          return;
        this.m_controllerCheckFrameCounter = 0;
        if (MyScreenManager.IsScreenOfTypeOpen(typeof (MyGuiScreenMessageBox)) || MyScreenManager.ExistsScreenOfType(typeof (MyGuiScreenLoading)))
          return;
        if (MyGuiScreenGamePlay.Static != null && MySandboxGame.IsGameReady && MyGuiScreenGamePlay.Static.State == MyGuiScreenState.OPENED)
          MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu, (object) !MySandboxGame.IsPaused));
        this.m_isControllerErrorMessageBoxVisible = true;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.ControllerErrorCaption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ControllerErrorText), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => this.m_isControllerErrorMessageBoxVisible = false))));
      }
      else
        this.m_controllerCheckFrameCounter = 0;
    }

    private void UpdateSound()
    {
      Vector3 listenerUp = Vector3.Up;
      Vector3 listenerFront = Vector3.Forward;
      if (MySector.MainCamera != null)
      {
        listenerUp = MySector.MainCamera.UpVector;
        listenerFront = -MySector.MainCamera.ForwardVector;
      }
      Vector3 zero = Vector3.Zero;
      this.GetListenerVelocity(ref zero);
      MyAudio.Static.Update(16, Vector3.Zero, listenerUp, listenerFront, zero);
      if (MyMusicController.Static != null && MyMusicController.Static.Active)
        MyMusicController.Static.Update();
      if (!MySandboxGame.Config.EnableMuteWhenNotInFocus || this.form == null)
        return;
      if (!this.form.IsActive)
      {
        if (!this.hasFocus)
          return;
        MyAudio.Static.VolumeMusic = 0.0f;
        MyAudio.Static.VolumeGame = 0.0f;
        MyAudio.Static.VolumeHud = 0.0f;
        MyAudio.Static.VolumeVoiceChat = 0.0f;
        this.hasFocus = false;
      }
      else
      {
        if (this.hasFocus)
          return;
        MyAudio.Static.VolumeMusic = MySandboxGame.Config.MusicVolume;
        MyAudio.Static.VolumeGame = MySandboxGame.Config.GameVolume;
        MyAudio.Static.VolumeHud = MySandboxGame.Config.GameVolume;
        MyAudio.Static.VolumeVoiceChat = MySandboxGame.Config.VoiceChatVolume;
        this.hasFocus = true;
      }
    }

    private void CheckAutoRestartForDedicatedServer()
    {
      if ((!MySandboxGame.ConfigDedicated.AutoRestartEnabled || MySandboxGame.ConfigDedicated.AutoRestatTimeInMin <= 0) && !this.IsGoingToUpdate || MySandboxGame.TotalTimeInMilliseconds <= this.m_lastRestartCheckInMilis)
        return;
      this.m_lastRestartCheckInMilis = MySandboxGame.TotalTimeInMilliseconds + 60000;
      int val1 = int.MaxValue;
      if (MySandboxGame.ConfigDedicated.AutoRestartEnabled && MySandboxGame.ConfigDedicated.AutoRestatTimeInMin > 0)
        val1 = Math.Min(val1, MySandboxGame.ConfigDedicated.AutoRestatTimeInMin);
      if (this.IsGoingToUpdate)
        val1 = Math.Min(val1, this.m_autoUpdateRestartTimeInMin);
      switch (MySandboxGame.AutoRestartState)
      {
        case EnumAutorestartStage.NotWarned:
          if (MySandboxGame.TotalTimeInMilliseconds < (val1 - 10) * 60000)
            break;
          MySandboxGame.AutoRestartWarning(10);
          MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_10Min;
          break;
        case EnumAutorestartStage.Warned_10Min:
          if (MySandboxGame.TotalTimeInMilliseconds < (val1 - 5) * 60000)
            break;
          MySandboxGame.AutoRestartWarning(5);
          MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_5Min;
          break;
        case EnumAutorestartStage.Warned_5Min:
          if (MySandboxGame.TotalTimeInMilliseconds < (val1 - 1) * 60000)
            break;
          MySandboxGame.AutoRestartWarning(1);
          MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_1Min;
          break;
        case EnumAutorestartStage.Warned_1Min:
          if (MySandboxGame.TotalTimeInMilliseconds < val1 * 60000)
            break;
          MySandboxGame.m_autoRestartState = EnumAutorestartStage.Restarting;
          this.m_lastRestartCheckInMilis = MySandboxGame.TotalTimeInMilliseconds;
          break;
        case EnumAutorestartStage.Restarting:
          if (MySandboxGame.ConfigDedicated.AutoRestatTimeInMin > 60)
            MyLog.Default.WriteLineAndConsole(string.Format("Automatic stop after {0} hours and {1} minutes", (object) (val1 / 60), (object) (val1 % 60)));
          else
            MyLog.Default.WriteLineAndConsole(string.Format("Automatic stop after {0} minutes", (object) val1));
          if (MySession.Static != null)
            MySession.Static.SetSaveOnUnloadOverride_Dedicated(new bool?(MySandboxGame.ConfigDedicated.AutoRestartSave));
          MySandboxGame.ExitThreadSafe();
          break;
        case EnumAutorestartStage.NoRestart:
          int time = val1 - MySandboxGame.TotalTimeInMilliseconds / 60000;
          if (time > 10)
            MySandboxGame.m_autoRestartState = EnumAutorestartStage.NotWarned;
          else if (time > 5)
            MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_10Min;
          else if (time > 1)
          {
            MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_5Min;
          }
          else
          {
            if (time <= 0)
              break;
            MySandboxGame.m_autoRestartState = EnumAutorestartStage.Warned_1Min;
          }
          if (this.IsGoingToUpdate)
          {
            MySandboxGame.AutoUpdateWarning(time);
            break;
          }
          MySandboxGame.AutoRestartWarning(time);
          break;
      }
    }

    private void CheckAutoUpdateForDedicatedServer()
    {
      if (!MySandboxGame.ConfigDedicated.AutoUpdateEnabled || this.IsGoingToUpdate || (DateTime.UtcNow - this.m_lastUpdateCheckTime).TotalMinutes < (double) MySandboxGame.ConfigDedicated.AutoUpdateCheckIntervalInMin)
        return;
      this.m_lastUpdateCheckTime = DateTime.UtcNow;
      System.Threading.Tasks.Task.Run((Action) (() => this.DownloadChangelog())).ContinueWith((Action<System.Threading.Tasks.Task>) (task => this.DownloadChangelogCompleted()));
      if (!MyGameService.IsUpdateAvailable())
        return;
      this.StartAutoUpdateCountdown();
    }

    private void DownloadChangelogCompleted()
    {
      if (this.m_changelog == null || this.m_changelog.Entry.Count <= 0)
        return;
      MyNewsEntry myNewsEntry = this.m_changelog.Entry[0];
      int result;
      if (!int.TryParse(myNewsEntry.Version, out result) || !myNewsEntry.Public || result <= (int) MyFinalBuildConstants.APP_VERSION)
        return;
      this.StartAutoUpdateCountdown();
    }

    private void DownloadChangelog()
    {
      if (this.m_changelogSerializer == null)
        this.m_changelogSerializer = new XmlSerializer(typeof (MyNews));
      try
      {
        string content;
        if (MyVRage.Platform.Http.SendRequest(MyPerGameSettings.ChangeLogUrl, (HttpData[]) null, HttpMethod.GET, out content) != HttpStatusCode.OK)
          return;
        using (StringReader stringReader = new StringReader(content))
        {
          this.m_changelog = (MyNews) this.m_changelogSerializer.Deserialize((TextReader) stringReader);
          this.m_changelog.Entry = this.m_changelog.Entry.Where<MyNewsEntry>((Func<MyNewsEntry, bool>) (x => x.Public)).ToList<MyNewsEntry>();
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while downloading changelog: " + ex.ToString());
      }
    }

    private void StartAutoUpdateCountdown()
    {
      this.m_isGoingToUpdate = true;
      this.m_autoUpdateRestartTimeInMin = MySandboxGame.TotalTimeInMilliseconds / 60000 + MySandboxGame.ConfigDedicated.AutoUpdateRestartDelayInMin;
    }

    public static void WriteConsoleOutputs()
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated || !MySandboxGame.IsAutoRestarting)
        return;
      MyLog.Default.WriteLineAndConsole("AUTORESTART");
    }

    public static void AutoRestartWarning(int time)
    {
      MyLog.Default.WriteLineAndConsole(string.Format("Server will restart in {0} minute{1}", (object) time, time == 1 ? (object) "" : (object) "s"));
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(string.Format(MyTexts.GetString(MyCommonTexts.Server_Restart_Warning), (object) time, time == 1 ? (object) "" : (object) "s"), ChatChannel.Global);
    }

    public static void AutoUpdateWarning(int time)
    {
      MyLog.Default.WriteLineAndConsole(string.Format("New version available. Server will restart in {0} minute{1}", (object) time, time == 1 ? (object) "" : (object) "s"));
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(string.Format(MyTexts.GetString(MyCommonTexts.Server_Update_Warning), (object) time, time == 1 ? (object) "" : (object) "s"), ChatChannel.Global);
    }

    private void GetListenerVelocity(ref Vector3 velocity)
    {
      if (MySession.Static == null)
        return;
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return;
      controlledEntity.GetLinearVelocity(ref velocity, false);
    }

    private void LogWriter(string text) => MySandboxGame.Log.WriteLine("Havok: " + text);

    protected override void LoadData_UpdateThread()
    {
    }

    protected override void UnloadData_UpdateThread()
    {
      using (HkAccessControl.PushState(HkAccessControl.AccessState.Exclusive))
      {
        if (MySession.Static != null)
          MySession.Static.Unload();
        this.UnloadData();
      }
      if (this.GameRenderComponent != null)
      {
        this.GameRenderComponent.Stop();
        this.GameRenderComponent.Dispose();
      }
      StringBuilder output = new StringBuilder();
      output.AppendLine("Havok memory statistics:");
      HkBaseSystem.GetMemoryStatistics(output);
      MyLog.Default.WriteLine(output.ToString());
      MyPhysicsDebugDraw.DebugGeometry.Dispose();
      ParallelTasks.Parallel.StartOnEachWorker(new Action(HkBaseSystem.QuitThread));
      if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
        HkBaseSystem.Quit();
      MySimpleProfiler.LogPerformanceTestResults();
    }

    protected override void PrepareForDraw()
    {
      using (Stats.Generic.Measure("GuiPrepareDraw"))
        MyGuiSandbox.Draw();
      using (Stats.Generic.Measure("DebugDraw"))
        Sandbox.Game.Entities.MyEntities.DebugDraw();
      using (Stats.Generic.Measure("Hierarchy"))
      {
        if (MyGridPhysicalHierarchy.Static == null)
          return;
        MyGridPhysicalHierarchy.Static.Draw();
      }
    }

    protected override void AfterDraw() => MyRenderProxy.AfterUpdate(new MyTimeSpan?(this.TotalTime));

    public void Invoke(Action action, string invokerName) => this.m_invokeQueue.Enqueue(new MySandboxGame.MyInvokeData()
    {
      Action = action,
      Invoker = invokerName
    });

    public void Invoke(string invokerName, object context, Action<object> action) => this.m_invokeQueue.Enqueue(new MySandboxGame.MyInvokeData()
    {
      ContextualAction = action,
      Context = context,
      Invoker = invokerName
    });

    public bool ProcessInvoke()
    {
      MyUtils.Swap<MyConcurrentQueue<MySandboxGame.MyInvokeData>>(ref this.m_invokeQueue, ref this.m_invokeQueueExecuting);
      bool flag = this.m_invokeQueueExecuting.Count > 0;
      MySandboxGame.MyInvokeData instance;
      while (this.m_invokeQueueExecuting.TryDequeue(out instance))
      {
        if (instance.Action != null)
          instance.Action();
        else
          instance.ContextualAction(instance.Context);
      }
      if (MyVRage.Platform.ImeProcessor != null)
        MyVRage.Platform.ImeProcessor.ProcessInvoke();
      return flag;
    }

    public void ClearInvokeQueue() => this.m_invokeQueue.Clear();

    public void SetMouseVisible(bool visible)
    {
      if (MyExternalAppBase.IsEditorActive)
        return;
      this.IsCursorVisible = visible;
      MyVRage.Platform.Input.ShowCursor = visible;
    }

    public static IErrorConsumer ErrorConsumer
    {
      get => MySandboxGame.m_errorConsumer;
      set => MySandboxGame.m_errorConsumer = value;
    }

    public uint IntroVideoId { get; protected set; }

    public static void ProcessRenderOutput()
    {
      MyRenderMessageBase renderMessageBase;
      while (MyRenderProxy.OutputQueue.TryDequeue(out renderMessageBase))
      {
        if (renderMessageBase != null)
        {
          switch (renderMessageBase.MessageType)
          {
            case MyRenderMessageEnum.ClipmapsReady:
              MySandboxGame.AreClipmapsReady = true;
              break;
            case MyRenderMessageEnum.ScreenshotTaken:
              if (MySession.Static != null)
              {
                MyRenderMessageScreenshotTaken messageScreenshotTaken = (MyRenderMessageScreenshotTaken) renderMessageBase;
                if (messageScreenshotTaken.ShowNotification)
                {
                  MyHudNotification myHudNotification = new MyHudNotification(messageScreenshotTaken.Success ? MyCommonTexts.ScreenshotSaved : MyCommonTexts.ScreenshotFailed, 2000);
                  if (messageScreenshotTaken.Success)
                    myHudNotification.SetTextFormatArguments((object) Path.GetFileName(messageScreenshotTaken.Filename));
                  MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
                }
                if (MySandboxGame.Static != null && MySandboxGame.Static.OnScreenshotTaken != null)
                  MySandboxGame.Static.OnScreenshotTaken((object) MySandboxGame.Static, (System.EventArgs) null);
                MyGuiBlueprintScreen_Reworked.ScreenshotTaken(messageScreenshotTaken.Success, messageScreenshotTaken.Filename);
                break;
              }
              break;
            case MyRenderMessageEnum.Error:
              MyRenderMessageError renderMessageError = (MyRenderMessageError) renderMessageBase;
              MySandboxGame.ErrorConsumer.OnError("Renderer error", renderMessageError.Message, renderMessageError.Callstack);
              if (renderMessageError.ShouldTerminate)
              {
                MySandboxGame.ExitThreadSafe();
                break;
              }
              break;
            case MyRenderMessageEnum.VideoAdaptersResponse:
              MyRenderMessageVideoAdaptersResponse adaptersResponse = (MyRenderMessageVideoAdaptersResponse) renderMessageBase;
              MyVideoSettingsManager.OnVideoAdaptersResponse(adaptersResponse);
              MySandboxGame.Static.CheckGraphicsCard(adaptersResponse);
              bool firstTimeRun = MySandboxGame.Config.FirstTimeRun;
              if (firstTimeRun)
              {
                MySandboxGame.Config.FirstTimeRun = false;
                MySandboxGame.Config.ExperimentalMode = false;
                MyVideoSettingsManager.WriteCurrentSettingsToConfig();
                MySandboxGame.Config.Save();
              }
              if (firstTimeRun)
                MySandboxGame.Config.FirstVTTimeRun = false;
              if ((!MySandboxGame.Config.FirstVTTimeRun ? 0 : (!firstTimeRun ? 1 : 0)) != 0 && (MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality == MyRenderQualityEnum.HIGH || MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality == MyRenderQualityEnum.EXTREME))
              {
                MySandboxGame.Config.SetToMediumQuality();
                MySandboxGame.Config.FirstVTTimeRun = false;
                MySandboxGame.Config.Save();
                StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
                MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.SwitchToNormalVT), messageCaption: messageCaption));
              }
              if (MySpaceAnalytics.Instance != null)
              {
                MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(MyGameService.UserId, firstTimeRun);
                break;
              }
              break;
            case MyRenderMessageEnum.CreatedDeviceSettings:
              MyVideoSettingsManager.OnCreatedDeviceSettings((MyRenderMessageCreatedDeviceSettings) renderMessageBase);
              break;
            case MyRenderMessageEnum.MainThreadCallback:
              MyRenderMessageMainThreadCallback mainThreadCallback = (MyRenderMessageMainThreadCallback) renderMessageBase;
              if (mainThreadCallback.Callback != null)
                mainThreadCallback.Callback();
              mainThreadCallback.Callback = (Action) null;
              break;
            case MyRenderMessageEnum.TasksFinished:
              MySandboxGame.RenderTasksFinished = true;
              break;
            case MyRenderMessageEnum.ParticleEffectRemoved:
              MyParticlesManager.OnRemoved(((MyRenderMessageParticleEffectRemoved) renderMessageBase).Id);
              break;
          }
          renderMessageBase.Dispose();
        }
      }
    }

    public static void ExitThreadSafe()
    {
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (MySandboxGame.Static != null)
        {
          MySandboxGame.Static.Exit();
          if (!Sandbox.Engine.Platform.Game.IsDedicated)
            MySandboxGame.Static.form.Hide();
        }
        ParallelTasks.Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromSeconds(10.0));
      }), "MySandboxGame::Exit");
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyPlatformGameSettings.FEEDBACK_ON_EXIT || string.IsNullOrEmpty(MyPlatformGameSettings.FEEDBACK_URL))
        return;
      MyGuiSandbox.OpenUrl(MyPlatformGameSettings.FEEDBACK_URL, UrlOpenMode.ExternalBrowser);
      MyPlatformGameSettings.FEEDBACK_URL = "";
    }

    public void Dispose()
    {
      if (MySessionComponentExtDebug.Static != null)
      {
        MySessionComponentExtDebug.Static.Dispose();
        MySessionComponentExtDebug.Static = (MySessionComponentExtDebug) null;
      }
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null)
        Sandbox.Engine.Multiplayer.MyMultiplayer.Static.Dispose();
      MyNetworkMonitor.Done();
      if (this.GameRenderComponent != null)
      {
        this.GameRenderComponent.Dispose();
        this.GameRenderComponent = (MyGameRenderComponent) null;
      }
      MyPlugins.Unload();
      ParallelTasks.Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromSeconds(10.0));
      MySandboxGame.m_windowCreatedEvent.Dispose();
      MyVRage.Platform.Scripting.ClearWhitelist();
      MyObjectBuilderType.UnregisterAssemblies();
      MyObjectBuilderSerializer.UnregisterAssembliesAndSerializers();
    }

    private void UpdateDamageEffectsInScene()
    {
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid myCubeGrid && myCubeGrid.Projector == null)
        {
          foreach (MySlimBlock block in myCubeGrid.GetBlocks())
          {
            if (this.m_enableDamageEffects)
              block.ResumeDamageEffect();
            else if (block.FatBlock != null)
              block.FatBlock.StopDamageEffect(false);
          }
        }
      }
    }

    public static void ReloadDedicatedServerSession()
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MyLog.Default.WriteLineAndConsole("Reloading dedicated server");
      MySandboxGame.IsReloading = true;
      MySandboxGame.Static.Exit();
    }

    internal void UpdateMouseCapture() => MyVRage.Platform.Input.MouseCapture = MySandboxGame.Config.CaptureMouse && MySandboxGame.Config.WindowMode != MyWindowModeEnum.Fullscreen;

    private void OverlayActivated(bool isActive)
    {
      if (isActive)
      {
        if (!Sync.MultiplayerActive)
          MySandboxGame.PausePush();
        MySandboxGame.Static.PauseInput = true;
        MySandboxGame.Static.m_unpauseInput = false;
      }
      else
      {
        if (!Sync.MultiplayerActive)
          MySandboxGame.PausePop();
        MySandboxGame.Static.m_unpauseInput = true;
        MySandboxGame.Static.m_inputPauseTime = DateTime.Now;
      }
    }

    static MySandboxGame()
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      dateTime = dateTime.AddDays((double) MySandboxGame.BuildVersion.Build);
      MySandboxGame.BuildDateTime = dateTime.AddSeconds((double) (MySandboxGame.BuildVersion.Revision * 2));
      MySandboxGame.m_reconfirmClipmaps = false;
      MySandboxGame.m_areClipmapsReady = true;
      MySandboxGame.m_renderTasksFinished = false;
      MySandboxGame.IsUpdateReady = true;
      MySandboxGame.m_autoRestartState = EnumAutorestartStage.NoRestart;
      MySandboxGame.IsRenderUpdateSyncEnabled = false;
      MySandboxGame.IsVideoRecordingEnabled = false;
      MySandboxGame.IsConsoleVisible = false;
      MySandboxGame.IsReloading = false;
      MySandboxGame.FatalErrorDuringInit = false;
      MySandboxGame.m_windowCreatedEvent = new ManualResetEvent(false);
      MySandboxGame.Log = new MyLog(true);
      MySandboxGame.PerformNotInteractiveReport = (MySandboxGame.NonInteractiveReportAction) null;
      MySandboxGame.m_totalPauseTimeInMilliseconds = 0;
      MySandboxGame.m_lastFrameTimeStamp = 0L;
      MySandboxGame.ShowIsBetterGCAvailableNotification = false;
      MySandboxGame.ShowGpuUnderMinimumNotification = false;
      MySandboxGame.m_timerTTHelper = 0;
      MySandboxGame.m_timerTTHelper_Max = 100;
      MySandboxGame.ShowWhitelistPopup = false;
      MySandboxGame.CanShowWhitelistPopup = false;
      MySandboxGame.ShowHotfixPopup = false;
      MySandboxGame.CanShowHotfixPopup = false;
      MySandboxGame.m_pauseStackCount = 0;
      MySandboxGame.m_errorConsumer = (IErrorConsumer) new MyGameErrorConsumer();
    }

    public enum MemState
    {
      Normal,
      Low,
      Critical,
    }

    public delegate void NonInteractiveReportAction(
      bool includeAdditionalLogs,
      IEnumerable<string> additionalFiles = null);

    private struct MyInvokeData
    {
      public Action Action;
      public object Context;
      public Action<object> ContextualAction;
      public string Invoker;
    }

    public interface IGameCustomInitialization
    {
      void InitIlChecker();

      void InitIlCompiler();
    }
  }
}
