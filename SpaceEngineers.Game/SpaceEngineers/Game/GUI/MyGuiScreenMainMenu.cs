// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenMainMenu
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.News;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
  public class MyGuiScreenMainMenu : MyGuiScreenMainMenuBase
  {
    private const int CONTROLS_PER_BANNER = 3;
    private readonly int DLC_UPDATE_INTERVAL = 5000;
    private MyGuiControlNews m_newsControl;
    private MyGuiControlDLCBanners m_dlcBannersControl;
    private MyGuiControlBase m_continueTooltipcontrol;
    private MyGuiControlButton m_continueButton;
    private MyGuiControlElementGroup m_elementGroup;
    private int m_currentDLCcounter;
    private MyBadgeHelper m_myBadgeHelper;
    private MyGuiControlButton m_exitGameButton;
    private MyGuiControlImageButton m_lastClickedBanner;
    private MyGuiScreenIntroVideo m_backgroundScreen;
    private bool m_parallelLoadIsRunning;

    public MyGuiScreenMainMenu()
      : this(false)
    {
    }

    public MyGuiScreenMainMenu(bool pauseGame)
      : base(pauseGame)
    {
      this.m_myBadgeHelper = new MyBadgeHelper();
      if (!pauseGame && MyGuiScreenGamePlay.Static == null)
        this.AddIntroScreen();
      MyGuiSandbox.DrawGameLogoHandler = new Action<float, Vector2>(this.m_myBadgeHelper.DrawGameLogo);
      MyInput.Static.IsJoystickLastUsed = MySandboxGame.Config.ControllerDefaultOnStart || MyPlatformGameSettings.CONTROLLER_DEFAULT_ON_START;
    }

    private void AddIntroScreen()
    {
      if (!MyFakes.ENABLE_MENU_VIDEO_BACKGROUND)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) (this.m_backgroundScreen = MyGuiScreenIntroVideo.CreateBackgroundScreen()));
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      Vector2 leftButtonPositionOrigin = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM) + new Vector2(minSizeGui.X / 2f, 0.0f) + new Vector2(15f, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      leftButtonPositionOrigin.Y += 0.043f;
      Vector2 vector2 = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) + new Vector2((float) (-(double) minSizeGui.X / 2.0), 0.0f);
      Vector2 lastButtonPosition = Vector2.Zero;
      if (MyGuiScreenGamePlay.Static == null)
        this.CreateMainMenu(leftButtonPositionOrigin, out lastButtonPosition);
      else
        this.CreateInGameMenu(leftButtonPositionOrigin, out lastButtonPosition);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(lastButtonPosition + new Vector2((float) (-(double) minSizeGui.X / 2.0), minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 49, 82)), new Vector2?(MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
      this.m_myBadgeHelper.RefreshGameLogo();
      this.CreateRightSection(minSizeGui);
      this.CheckLowMemSwitchToLow();
      if (MyGuiScreenGamePlay.Static != null || MyPlatformGameSettings.FEEDBACK_ON_EXIT || string.IsNullOrEmpty(MyPlatformGameSettings.FEEDBACK_URL))
        return;
      MyGuiSandbox.OpenUrl(MyPlatformGameSettings.FEEDBACK_URL, UrlOpenMode.ExternalWithConfirm, MyTexts.Get(MyCommonTexts.MessageBoxTextBetaFeedback), MyTexts.Get(MyCommonTexts.MessageBoxCaptionBetaFeedback));
    }

    private void CreateRightSection(Vector2 buttonSize)
    {
      MyGuiControlNews myGuiControlNews = new MyGuiControlNews();
      myGuiControlNews.Position = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) - 5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      myGuiControlNews.Size = new Vector2(0.4f, 0.28f);
      myGuiControlNews.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_newsControl = myGuiControlNews;
      this.Controls.Add((MyGuiControlBase) this.m_newsControl);
      Vector2 vector2 = new Vector2(this.m_newsControl.Size.X, (float) (((double) this.m_newsControl.Size.X - 0.00400000018998981) * 0.4072265625 * 1.33333337306976) + 0.052f);
      MyGuiControlDLCBanners controlDlcBanners = new MyGuiControlDLCBanners();
      controlDlcBanners.Position = new Vector2(this.m_newsControl.Position.X, 0.26f);
      controlDlcBanners.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_dlcBannersControl = controlDlcBanners;
      this.m_dlcBannersControl.Size = vector2;
      this.m_dlcBannersControl.Visible = false;
      (MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM) + new Vector2(buttonSize.X / 2f, 0.0f) + new Vector2(15f, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE).Y += 0.043f;
      this.Controls.Add((MyGuiControlBase) this.m_dlcBannersControl);
    }

    private bool IsControllerHelpGoingToFitInMiddleBottomOfScreen()
    {
      MyConfig config = MySandboxGame.Config;
      return (double) MyVideoSettingsManager.CurrentDeviceSettings.BackBufferWidth / (double) MyVideoSettingsManager.CurrentDeviceSettings.BackBufferHeight >= 1.77777779102325;
    }

    private void CreateInGameMenu(Vector2 leftButtonPositionOrigin, out Vector2 lastButtonPosition)
    {
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_ScreenIngame);
      this.EnabledBackgroundFade = true;
      int num1 = Sync.MultiplayerActive ? 6 : 5;
      int num2;
      MyGuiControlButton guiControlButton1 = this.MakeButton(leftButtonPositionOrigin - (float) (num2 = num1 - 1) * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonSave, new Action<MyGuiControlButton>(this.OnClickSaveWorld));
      int num3;
      MyGuiControlButton guiControlButton2 = this.MakeButton(leftButtonPositionOrigin - (float) (num3 = num2 - 1) * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.LoadScreenButtonSaveAs, new Action<MyGuiControlButton>(this.OnClickSaveAs));
      if (!Sync.IsServer || MySession.Static != null && !MySession.Static.Settings.EnableSaving)
      {
        MyStringId text = !Sync.IsServer ? MyCommonTexts.NotificationClientCannotSave : MyCommonTexts.NotificationSavingDisabled;
        guiControlButton1.Enabled = false;
        guiControlButton1.ShowTooltipWhenDisabled = true;
        guiControlButton1.SetToolTip(text);
        guiControlButton2.Enabled = false;
        guiControlButton2.ShowTooltipWhenDisabled = true;
        guiControlButton2.SetToolTip(text);
      }
      this.Controls.Add((MyGuiControlBase) guiControlButton1);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton1);
      this.Controls.Add((MyGuiControlBase) guiControlButton2);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton2);
      if (Sync.MultiplayerActive)
      {
        MyGuiControlButton guiControlButton3 = this.MakeButton(leftButtonPositionOrigin - (float) --num3 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonPlayers, new Action<MyGuiControlButton>(this.OnClickPlayers));
        this.Controls.Add((MyGuiControlBase) guiControlButton3);
        this.m_elementGroup.Add((MyGuiControlBase) guiControlButton3);
      }
      int num4;
      MyGuiControlButton guiControlButton4 = this.MakeButton(leftButtonPositionOrigin - (float) (num4 = num3 - 1) * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonOptions, new Action<MyGuiControlButton>(this.OnClickOptions));
      this.Controls.Add((MyGuiControlBase) guiControlButton4);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton4);
      int num5;
      this.m_exitGameButton = this.MakeButton(leftButtonPositionOrigin - (float) (num5 = num4 - 1) * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonExitToMainMenu, new Action<MyGuiControlButton>(this.OnExitToMainMenuClick));
      this.Controls.Add((MyGuiControlBase) this.m_exitGameButton);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_exitGameButton);
      lastButtonPosition = this.m_exitGameButton.Position;
    }

    private void CreateMainMenu(Vector2 leftButtonPositionOrigin, out Vector2 lastButtonPosition)
    {
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_Screen);
      this.EnabledBackgroundFade = false;
      int num1 = MyPerGameSettings.MultiplayerEnabled ? 7 : 6;
      MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
      int num2;
      if (lastSession != null && (lastSession.Path == null || MyPlatformGameSettings.GAME_SAVES_TO_CLOUD || Directory.Exists(lastSession.Path)) && (!lastSession.IsLobby || MyGameService.LobbyDiscovery.ContinueToLobbySupported))
      {
        Vector2 vector2_1 = leftButtonPositionOrigin;
        int num3 = num1;
        num2 = num3 - 1;
        Vector2 vector2_2 = (float) num3 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
        this.m_continueButton = this.MakeButton(vector2_1 - vector2_2 - MyGuiConstants.MENU_BUTTONS_POSITION_DELTA / 2f, MyCommonTexts.ScreenMenuButtonContinueGame, new Action<MyGuiControlButton>(this.OnContinueGameClicked));
        this.Controls.Add((MyGuiControlBase) this.m_continueButton);
        this.m_elementGroup.Add((MyGuiControlBase) this.m_continueButton);
        this.GenerateContinueTooltip(lastSession, this.m_continueButton, new Vector2(3f / 1000f, -1f / 400f));
        this.m_continueButton.FocusChanged += new Action<MyGuiControlBase, bool>(this.FocusChangedContinue);
      }
      else
        num2 = num1 - 1;
      Vector2 vector2_3 = leftButtonPositionOrigin;
      int num4 = num2;
      int num5 = num4 - 1;
      Vector2 vector2_4 = (float) num4 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton1 = this.MakeButton(vector2_3 - vector2_4, MyCommonTexts.ScreenMenuButtonCampaign, new Action<MyGuiControlButton>(this.OnClickNewGame));
      this.Controls.Add((MyGuiControlBase) guiControlButton1);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton1);
      Vector2 vector2_5 = leftButtonPositionOrigin;
      int num6 = num5;
      int num7 = num6 - 1;
      Vector2 vector2_6 = (float) num6 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton2 = this.MakeButton(vector2_5 - vector2_6, MyCommonTexts.ScreenMenuButtonLoadGame, new Action<MyGuiControlButton>(this.OnClickLoad));
      this.Controls.Add((MyGuiControlBase) guiControlButton2);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton2);
      if (MyPerGameSettings.MultiplayerEnabled)
      {
        MyGuiControlButton guiControlButton3 = this.MakeButton(leftButtonPositionOrigin - (float) num7-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonJoinGame, new Action<MyGuiControlButton>(this.OnJoinWorld));
        this.Controls.Add((MyGuiControlBase) guiControlButton3);
        this.m_elementGroup.Add((MyGuiControlBase) guiControlButton3);
      }
      Vector2 vector2_7 = leftButtonPositionOrigin;
      int num8 = num7;
      int num9 = num8 - 1;
      Vector2 vector2_8 = (float) num8 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton4 = this.MakeButton(vector2_7 - vector2_8, MyCommonTexts.ScreenMenuButtonOptions, new Action<MyGuiControlButton>(this.OnClickOptions));
      this.Controls.Add((MyGuiControlBase) guiControlButton4);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton4);
      lastButtonPosition = guiControlButton4.Position;
      if (MyFakes.ENABLE_MAIN_MENU_INVENTORY_SCENE)
      {
        MyGuiControlButton guiControlButton3 = this.MakeButton(leftButtonPositionOrigin - (float) num9-- * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, MyCommonTexts.ScreenMenuButtonInventory, new Action<MyGuiControlButton>(this.OnClickInventory));
        this.Controls.Add((MyGuiControlBase) guiControlButton3);
        this.m_elementGroup.Add((MyGuiControlBase) guiControlButton3);
        lastButtonPosition = guiControlButton3.Position;
      }
      if (MyPlatformGameSettings.LIMITED_MAIN_MENU)
        return;
      Vector2 vector2_9 = leftButtonPositionOrigin;
      int num10 = num9;
      int num11 = num10 - 1;
      Vector2 vector2_10 = (float) num10 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      this.m_exitGameButton = this.MakeButton(vector2_9 - vector2_10, MyCommonTexts.ScreenMenuButtonExitToWindows, new Action<MyGuiControlButton>(this.OnClickExitToWindows));
      this.Controls.Add((MyGuiControlBase) this.m_exitGameButton);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_exitGameButton);
      lastButtonPosition = this.m_exitGameButton.Position;
    }

    private void GenerateContinueTooltip(
      MyObjectBuilder_LastSession lastSession,
      MyGuiControlButton button,
      Vector2 correction)
    {
      string thumbnail = this.GetThumbnail(lastSession);
      string text;
      if (lastSession.IsOnline)
        text = string.Format("{0}{1}{2} - {3}", (object) MyTexts.GetString(MyCommonTexts.ToolTipContinueGame), (object) Environment.NewLine, (object) lastSession.GameName, (object) lastSession.GetConnectionString());
      else
        text = string.Format("{0}{1}{2}", (object) MyTexts.GetString(MyCommonTexts.ToolTipContinueGame), (object) Environment.NewLine, (object) lastSession.GameName);
      if (thumbnail != null)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) new List<string>()
        {
          thumbnail
        }, TextureType.GUIWithoutPremultiplyAlpha);
      MyGuiControlBase imageTooltip = this.CreateImageTooltip(thumbnail, text);
      imageTooltip.Visible = false;
      imageTooltip.Position = button.Position + new Vector2(0.5f * button.Size.X, -1f * button.Size.Y) + correction;
      this.m_continueTooltipcontrol = imageTooltip;
      this.Controls.Add(this.m_continueTooltipcontrol);
    }

    private void FocusChangedContinue(MyGuiControlBase controls, bool focused) => this.m_continueTooltipcontrol.Visible = focused;

    private string GetThumbnail(MyObjectBuilder_LastSession session)
    {
      string path = session?.Path;
      if (path == null)
        return (string) null;
      if (Directory.Exists(path + MyGuiScreenLoadSandbox.CONST_BACKUP))
      {
        string[] directories = Directory.GetDirectories(path + MyGuiScreenLoadSandbox.CONST_BACKUP);
        if (((IEnumerable<string>) directories).Any<string>())
        {
          string str = Path.Combine(((IEnumerable<string>) directories).Last<string>(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
          if (File.Exists(str) && new FileInfo(str).Length > 0L)
            return str;
        }
      }
      string str1 = Path.Combine(path, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
      if (File.Exists(str1) && new FileInfo(str1).Length > 0L)
        return str1;
      if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
      {
        byte[] bytes = MyGameService.LoadFromCloud(MyCloudHelper.Combine(MyCloudHelper.LocalToCloudWorldPath(path + "/"), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
        if (bytes != null)
        {
          try
          {
            string str2 = Path.Combine(path, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
            Directory.CreateDirectory(path);
            File.WriteAllBytes(str2, bytes);
            MyRenderProxy.UnloadTexture(str2);
            return str2;
          }
          catch
          {
          }
        }
      }
      return (string) null;
    }

    private MyGuiControlBase CreateImageTooltip(string path, string text)
    {
      MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
      guiControlParent1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlParent1.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      guiControlParent1.ColorMask = (Vector4) MyGuiConstants.THEMED_GUI_BACKGROUND_COLOR;
      MyGuiControlParent guiControlParent2 = guiControlParent1;
      guiControlParent2.CanHaveFocus = false;
      guiControlParent2.HighlightType = MyGuiControlHighlightType.NEVER;
      guiControlParent2.BorderEnabled = true;
      Vector2 vector2 = new Vector2(0.005f, 1f / 500f);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(Vector2.Zero), text: text);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      myGuiControlLabel2.CanHaveFocus = false;
      myGuiControlLabel2.HighlightType = MyGuiControlHighlightType.NEVER;
      MyGuiControlImage myGuiControlImage1;
      if (!string.IsNullOrEmpty(path))
      {
        MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2?(Vector2.Zero), new Vector2?(new Vector2(0.175625f, 0.1317188f)));
        myGuiControlImage2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
        myGuiControlImage1 = myGuiControlImage2;
        myGuiControlImage1.SetTexture(path);
        myGuiControlImage1.CanHaveFocus = false;
        myGuiControlImage1.HighlightType = MyGuiControlHighlightType.NEVER;
      }
      else
      {
        MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2?(Vector2.Zero), new Vector2?(Vector2.Zero));
        myGuiControlImage2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
        myGuiControlImage1 = myGuiControlImage2;
      }
      guiControlParent2.Size = new Vector2(Math.Max(myGuiControlLabel2.Size.X, myGuiControlImage1.Size.X) + vector2.X * 2f, (float) ((double) myGuiControlLabel2.Size.Y + (double) myGuiControlImage1.Size.Y + (double) vector2.Y * 4.0));
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlImage1);
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      myGuiControlLabel2.Position = -guiControlParent2.Size / 2f + vector2;
      myGuiControlImage1.Position = new Vector2(0.0f, guiControlParent2.Size.Y / 2f - vector2.Y);
      return (MyGuiControlBase) guiControlParent2;
    }

    private void MenuRefocusImageButton(MyGuiControlImageButton sender) => this.m_lastClickedBanner = sender;

    private void OnClickBack(MyGuiControlButton obj) => this.RecreateControls(false);

    private void OnPlayClicked(MyGuiControlButton obj) => this.RecreateControls(false);

    private void OnClickInventory(MyGuiControlButton obj)
    {
      if (MyGameService.IsActive)
      {
        if (MyGameService.Service.GetInstallStatus(out int _))
        {
          if (MySession.Static == null)
          {
            MyGuiScreenLoadInventory inventory = MyGuiSandbox.CreateScreen<MyGuiScreenLoadInventory>();
            MyGuiScreenLoading guiScreenLoading = new MyGuiScreenLoading((MyGuiScreenBase) inventory, (MyGuiScreenGamePlay) null);
            MyGuiScreenLoadInventory screenLoadInventory = inventory;
            screenLoadInventory.OnLoadingAction = screenLoadInventory.OnLoadingAction + (Action) (() =>
            {
              MySessionLoader.LoadInventoryScene();
              MySandboxGame.IsUpdateReady = true;
              inventory.Initialize(false, (HashSet<string>) null);
            });
            MyGuiSandbox.AddScreen((MyGuiScreenBase) guiScreenLoading);
          }
          else
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenLoadInventory>((object) false, null));
        }
        else
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.InventoryScreen_InstallInProgress), messageCaption: messageCaption));
        }
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SteamIsOfflinePleaseRestart), messageCaption: messageCaption));
      }
    }

    private void OnContinueGameClicked(MyGuiControlButton myGuiControlButton) => this.RunWithTutorialCheck((Action) (() => this.ContinueGameInternal()));

    private void ContinueGameInternal()
    {
      MyObjectBuilder_LastSession mySession = MyLocalCache.GetLastSession();
      if (mySession == null)
        return;
      if (mySession.IsOnline)
      {
        if (mySession.IsLobby)
          MyJoinGameHelper.JoinGame(ulong.Parse(mySession.ServerIP));
        else
          MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
          {
            switch (granted)
            {
              case PermissionResult.Granted:
                MyGameService.Service.RequestPermissions(Permissions.CrossMultiplayer, true, (Action<PermissionResult>) (crossGranted =>
                {
                  switch (crossGranted)
                  {
                    case PermissionResult.Granted:
                      MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
                      {
                        switch (ugcGranted)
                        {
                          case PermissionResult.Granted:
                            this.JoinServer(mySession);
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
                break;
              case PermissionResult.Error:
                MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                break;
            }
          }));
      }
      else
      {
        if (this.m_parallelLoadIsRunning)
          return;
        this.m_parallelLoadIsRunning = true;
        MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
        MyScreenManager.AddScreen((MyGuiScreenBase) progressScreen);
        Parallel.StartBackground((Action) (() => MySessionLoader.LoadLastSession()), (Action) (() =>
        {
          progressScreen.CloseScreen();
          this.m_parallelLoadIsRunning = false;
        }));
      }
    }

    private void JoinServer(MyObjectBuilder_LastSession mySession)
    {
      try
      {
        MyGuiScreenProgress prog = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.DialogTextCheckServerStatus));
        MyGuiSandbox.AddScreen((MyGuiScreenBase) prog);
        MyGameService.OnPingServerResponded += (EventHandler<MyGameServerItem>) ((sender, item) =>
        {
          MyGuiSandbox.RemoveScreen((MyGuiScreenBase) prog);
          MySandboxGame.Static.ServerResponded(sender, item);
          // ISSUE: method pointer
          MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>((object) this, __methodptr(\u003CJoinServer\u003Eg__OnPingSuccess\u007C0));
          // ISSUE: method pointer
          MyGameService.OnPingServerFailedToRespond -= new EventHandler((object) this, __methodptr(\u003CJoinServer\u003Eg__OnPingFailure\u007C1));
        });
        MyGameService.OnPingServerFailedToRespond += (EventHandler) ((sender, data) =>
        {
          MyGuiSandbox.RemoveScreen((MyGuiScreenBase) prog);
          MySandboxGame.Static.ServerFailedToRespond(sender, data);
          // ISSUE: method pointer
          MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>((object) this, __methodptr(\u003CJoinServer\u003Eg__OnPingSuccess\u007C0));
          // ISSUE: method pointer
          MyGameService.OnPingServerFailedToRespond -= new EventHandler((object) this, __methodptr(\u003CJoinServer\u003Eg__OnPingFailure\u007C1));
        });
        MyGameService.PingServer(mySession.GetConnectionString());
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
      }
    }

    private void OnCustomGameClicked(MyGuiControlButton myGuiControlButton) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenWorldSettings>());

    private void OnClickReportBug(MyGuiControlButton obj) => MyGuiSandbox.OpenUrl(MyPerGameSettings.BugReportUrl, UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextOpenBrowser, (object) "forums.keenswh.com"));

    private void OnJoinWorld(MyGuiControlButton sender) => this.RunWithTutorialCheck((Action) (() =>
    {
      if (MyGameService.IsOnline)
      {
        MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
        {
          if (granted == PermissionResult.Granted)
          {
            MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
            {
              switch (ugcGranted)
              {
                case PermissionResult.Granted:
                  MyGameService.Service.RequestPermissions(Permissions.CrossMultiplayer, true, (Action<PermissionResult>) (crossGranted =>
                  {
                    MyGuiScreenJoinGame guiScreenJoinGame = new MyGuiScreenJoinGame(crossGranted == PermissionResult.Granted);
                    guiScreenJoinGame.Closed += new MyGuiScreenBase.ScreenHandler(this.joinGameScreen_Closed);
                    MyGuiSandbox.AddScreen((MyGuiScreenBase) guiScreenJoinGame);
                  }));
                  break;
                case PermissionResult.Error:
                  MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info)), "New Game screen");
                  break;
              }
            }));
          }
          else
          {
            if (granted != PermissionResult.Error)
              return;
            MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info);
          }
        }));
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyGameService.IsActive ? MyCommonTexts.SteamIsOfflinePleaseRestart : MyCommonTexts.ErrorJoinSessionNoUser), (object) MySession.GameServiceName), messageCaption: messageCaption));
      }
    }));

    private void joinGameScreen_Closed(MyGuiScreenBase source, bool isUnloading)
    {
      if (!source.Cancelled)
        return;
      this.State = MyGuiScreenState.OPENING;
      source.Closed -= new MyGuiScreenBase.ScreenHandler(this.joinGameScreen_Closed);
    }

    private void RunWithTutorialCheck(Action afterTutorial)
    {
      if (MySandboxGame.Config.FirstTimeTutorials)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenTutorialsScreen(afterTutorial));
      else
        afterTutorial();
    }

    private void OnClickNewGame(MyGuiControlButton sender)
    {
      if (MySandboxGame.Config.EnableNewNewGameScreen)
        this.RunWithTutorialCheck((Action) (() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenSimpleNewGame>())));
      else
        this.RunWithTutorialCheck((Action) (() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenNewGame>((object) true, (object) true, (object) true))));
    }

    private void OnClickLoad(MyGuiControlBase sender) => this.RunWithTutorialCheck((Action) (() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenLoadSandbox())));

    private void OnClickPlayers(MyGuiControlButton obj) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenPlayers>());

    private void OnExitToMainMenuClick(MyGuiControlButton sender)
    {
      this.CanBeHidden = false;
      MyGuiScreenMessageBox screenMessageBox = Sync.IsServer ? (!MySession.Static.Settings.EnableSaving || !Sync.IsServer ? MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextCampaignBeforeExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnExitToMainMenuFromCampaignMessageBoxCallback)) : MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO_CANCEL, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSaveChangesBeforeExit), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnExitToMainMenuMessageBoxCallback))) : MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextAnyWorldBeforeExit), MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnExitToMainMenuFromCampaignMessageBoxCallback));
      screenMessageBox.SkipTransition = true;
      screenMessageBox.InstantClose = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenMessageBox);
    }

    private void OnExitToMainMenuMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      switch (callbackReturn)
      {
        case MyGuiScreenMessageBox.ResultEnum.YES:
          MyAudio.Static.Mute = true;
          MyAudio.Static.StopMusic();
          MyAsyncSaving.Start((Action) (() => MySandboxGame.Static.OnScreenshotTaken += new EventHandler(this.UnloadAndExitAfterScreeshotWasTaken)));
          break;
        case MyGuiScreenMessageBox.ResultEnum.NO:
          MyAudio.Static.Mute = true;
          MyAudio.Static.StopMusic();
          MySessionLoader.UnloadAndExitToMenu();
          break;
        case MyGuiScreenMessageBox.ResultEnum.CANCEL:
          this.CanBeHidden = true;
          break;
      }
    }

    private void OnExitToMainMenuFromCampaignMessageBoxCallback(
      MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MyAudio.Static.Mute = true;
        MyAudio.Static.StopMusic();
        MySessionLoader.UnloadAndExitToMenu();
      }
      else
        this.CanBeHidden = true;
    }

    private void UnloadAndExitAfterScreeshotWasTaken(object sender, System.EventArgs e)
    {
      MySandboxGame.Static.OnScreenshotTaken -= new EventHandler(this.UnloadAndExitAfterScreeshotWasTaken);
      MySessionLoader.UnloadAndExitToMenu();
    }

    private void OnClickOptions(MyGuiControlButton sender) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateScreen<MyGuiScreenOptionsSpace>((object) MyPlatformGameSettings.LIMITED_MAIN_MENU));

    private void OnClickExitToWindows(MyGuiControlButton sender) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextAreYouSureYouWantToExit), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionExit), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnExitToWindowsMessageBoxCallback)));

    private void OnExitToWindowsMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.OnLogoutProgressClosed();
      }
      else
      {
        if (this.m_exitGameButton == null || !this.m_exitGameButton.Visible)
          return;
        this.FocusedControl = (MyGuiControlBase) this.m_exitGameButton;
        this.m_exitGameButton.Selected = true;
      }
    }

    private void OnLogoutProgressClosed()
    {
      MySandboxGame.Config.ControllerDefaultOnStart = MyInput.Static.IsJoystickLastUsed;
      MySandboxGame.Config.Save();
      MySandboxGame.Log.WriteLine("Application closed by user");
      if (MySpaceAnalytics.Instance != null)
        MySpaceAnalytics.Instance.ReportSessionEnd("Exit to Windows");
      MyScreenManager.CloseAllScreensNowExcept((MyGuiScreenBase) null);
      MySandboxGame.ExitThreadSafe();
    }

    private void OnClickSaveWorld(MyGuiControlButton sender)
    {
      this.CanBeHidden = false;
      MyGuiScreenMessageBox screenMessageBox = !MyAsyncSaving.InProgress ? MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToSaveYourProgress), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnSaveWorldMessageBoxCallback)) : MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
      screenMessageBox.SkipTransition = true;
      screenMessageBox.InstantClose = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenMessageBox);
    }

    private void OnSaveWorldMessageBoxCallback(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
        MyAsyncSaving.Start();
      else
        this.CanBeHidden = true;
    }

    private void OnClickSaveAs(MyGuiControlButton sender) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenSaveAs(MySession.Static.Name));

    public override bool Update(bool hasFocus)
    {
      base.Update(hasFocus);
      if (MySession.Static == null)
        Parallel.RunCallbacks();
      this.m_currentDLCcounter += 16;
      if (this.m_currentDLCcounter > this.DLC_UPDATE_INTERVAL)
      {
        this.m_currentDLCcounter = 0;
        this.m_myBadgeHelper.RefreshGameLogo();
      }
      if (hasFocus && MyGuiScreenGamePlay.Static == null && MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
        this.OnClickExitToWindows((MyGuiControlButton) null);
      if (hasFocus && this.m_lastClickedBanner != null)
      {
        this.FocusedControl = (MyGuiControlBase) null;
        this.m_lastClickedBanner = (MyGuiControlImageButton) null;
      }
      if (this.m_newsControl != null)
      {
        MyNewsLink[] newsLinks = this.m_newsControl.NewsLinks;
        if ((newsLinks != null ? ((uint) newsLinks.Length > 0U ? 1 : 0) : 0) != 0)
        {
          if (MyGuiScreenGamePlay.Static == null)
          {
            this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_ScreenWithLink);
            goto label_16;
          }
          else
          {
            this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_ScreenInGameWithLink);
            goto label_16;
          }
        }
      }
      if (MyGuiScreenGamePlay.Static == null)
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_Screen);
      else
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.MainMenuScreen_Help_ScreenIngame);
label_16:
      return true;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!receivedFocusInThisUpdate && MyGuiScreenGamePlay.Static != null && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
        this.CloseScreen(false);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        return;
      this.ShowCurrentNews();
    }

    private void ShowCurrentNews()
    {
      if (this.m_newsControl == null)
        return;
      MyNewsLink[] newsLinks = this.m_newsControl.NewsLinks;
      if ((newsLinks != null ? ((uint) newsLinks.Length > 0U ? 1 : 0) : 0) == 0)
        return;
      MyNewsLink newsLink = this.m_newsControl.NewsLinks[0];
      if (string.IsNullOrEmpty(newsLink.Link))
        return;
      MyGuiSandbox.OpenUrlWithFallback(newsLink.Link, newsLink.Text);
    }

    private void CreateModIoConsentScreen(Action onConsentAgree = null, Action onConsentOptOut = null)
    {
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
    }

    protected override void OnShow()
    {
      base.OnShow();
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
    }

    public override void CloseScreenNow(bool isUnloading = false)
    {
      base.CloseScreenNow(isUnloading);
      if (this.m_backgroundScreen != null)
        this.m_backgroundScreen.CloseScreenNow(isUnloading);
      this.m_backgroundScreen = (MyGuiScreenIntroVideo) null;
    }

    public override void OpenUserRelatedScreens()
    {
    }

    public override void CloseUserRelatedScreens() => this.m_newsControl?.CloseNewVersionScreen();

    public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
    {
      base.OnScreenOrderChanged(oldLast, newLast);
      if (newLast != this)
        return;
      this.CheckContinueButtonVisibility();
    }

    private void CheckContinueButtonVisibility()
    {
      if (this.m_continueButton == null)
        return;
      MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
      this.m_continueButton.Visible = lastSession != null && (lastSession.Path == null || Directory.Exists(lastSession.Path)) && (!lastSession.IsLobby || MyGameService.LobbyDiscovery.ContinueToLobbySupported);
    }
  }
}
