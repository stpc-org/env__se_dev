// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenLoading
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.AppCode;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Diagnostics;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenLoading : MyGuiScreenBase
  {
    public static readonly int STREAMING_TIMEOUT = 900;
    public static MyGuiScreenLoading Static;
    private MyGuiScreenBase m_screenToLoad;
    private readonly MyGuiScreenGamePlay m_screenToUnload;
    private string m_backgroundScreenTexture;
    private string m_backgroundTextureFromConstructor;
    private string m_customTextFromConstructor;
    private string m_rotatingWheelTexture;
    private string m_gameLogoTexture;
    private MyLoadingScreenText m_currentText;
    private MyGuiControlMultilineText m_multiTextControl;
    private StringBuilder m_authorWithDash;
    private MyGuiControlRotatingWheel m_wheel;
    private bool m_exceptionDuringLoad;
    public static string LastBackgroundTexture;
    public Action OnLoadingXMLAllowed;
    public static int m_currentTextIdx = 0;
    private volatile bool m_loadInDrawFinished;
    private bool m_loadFinished;
    private bool m_isStreamed;
    private int m_streamingTimeout;
    private string m_font = "LoadingScreen";
    private MyTimeSpan m_loadingTimeStart;
    private static long lastEnvWorkingSet = 0;
    private static long lastGc = 0;
    private static long lastVid = 0;

    public event Action OnScreenLoadingFinished;

    public MyGuiScreenLoading(
      MyGuiScreenBase screenToLoad,
      MyGuiScreenGamePlay screenToUnload,
      string textureFromConstructor,
      string customText = null)
      : base(new Vector2?(Vector2.Zero))
    {
      this.CanBeHidden = false;
      this.m_isTopMostScreen = true;
      MyLoadingPerformance.Instance.StartTiming();
      MyGuiScreenLoading.Static = this;
      this.m_screenToLoad = screenToLoad;
      this.m_screenToUnload = screenToUnload;
      this.m_closeOnEsc = false;
      this.DrawMouseCursor = false;
      this.m_loadInDrawFinished = false;
      this.m_drawEvenWithoutFocus = true;
      this.m_currentText = MyLoadingScreenText.GetRandomText();
      this.m_isFirstForUnload = true;
      MyGuiSandbox.SetMouseCursorVisibility(false);
      this.m_rotatingWheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";
      this.m_backgroundTextureFromConstructor = textureFromConstructor;
      this.m_customTextFromConstructor = customText;
      this.m_loadFinished = false;
      if (this.m_screenToLoad != null)
      {
        MySandboxGame.IsUpdateReady = false;
        MySandboxGame.AreClipmapsReady = !Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || MyExternalAppBase.Static != null;
        MySandboxGame.RenderTasksFinished = Sandbox.Engine.Platform.Game.IsDedicated || MyExternalAppBase.Static != null;
      }
      this.m_authorWithDash = new StringBuilder();
      this.RecreateControls(true);
      MyInput.Static.EnableInput(false);
      if (Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || MyMultiplayer.Static == null)
        this.m_isStreamed = true;
      else
        MyMultiplayer.Static.LocalRespawnRequested += new Action(this.OnLocalRespawnRequested);
    }

    private void OnLocalRespawnRequested()
    {
      (MyMultiplayer.Static as MyMultiplayerClientBase).RequestBatchConfirmation();
      MyMultiplayer.Static.PendingReplicablesDone += new Action(this.MyMultiplayer_PendingReplicablesDone);
      MyMultiplayer.Static.LocalRespawnRequested -= new Action(this.OnLocalRespawnRequested);
      this.m_streamingTimeout = 0;
    }

    private void MyMultiplayer_PendingReplicablesDone()
    {
      this.m_isStreamed = true;
      if (MySession.Static.VoxelMaps.Instances.Count > 0)
        MySandboxGame.AreClipmapsReady = false;
      MyMultiplayer.Static.PendingReplicablesDone -= new Action(this.MyMultiplayer_PendingReplicablesDone);
    }

    public MyGuiScreenLoading(MyGuiScreenBase screenToLoad, MyGuiScreenGamePlay screenToUnload)
      : this(screenToLoad, screenToUnload, (string) null)
    {
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2 = MyGuiManager.MeasureString(this.m_font, MyTexts.Get(MyCommonTexts.LoadingPleaseWaitUppercase), 1.1f);
      this.m_wheel = new MyGuiControlRotatingWheel(new Vector2?(MyGuiConstants.LOADING_PLEASE_WAIT_POSITION - new Vector2(0.0f, 0.09f + vector2.Y)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), texture: this.m_rotatingWheelTexture, manualRotationUpdate: false, multipleSpinningWheels: MyPerGameSettings.GUI.MultipleSpinningWheels);
      StringBuilder contents = string.IsNullOrEmpty(this.m_customTextFromConstructor) ? new StringBuilder(this.m_currentText.ToString()) : new StringBuilder(this.m_customTextFromConstructor);
      this.m_multiTextControl = new MyGuiControlMultilineText(new Vector2?(Vector2.One * 0.5f), new Vector2?(new Vector2(0.9f, 0.2f)), new Vector4?(Vector4.One), this.m_font, 1f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, contents, false, false);
      this.m_multiTextControl.BorderEnabled = false;
      this.m_multiTextControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.m_multiTextControl.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.Controls.Add((MyGuiControlBase) this.m_wheel);
      this.RefreshText();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenLoading);

    public override void LoadContent()
    {
      this.m_loadingTimeStart = new MyTimeSpan(Stopwatch.GetTimestamp());
      MySandboxGame.Log.WriteLine("MyGuiScreenLoading.LoadContent - START");
      MySandboxGame.Log.IncreaseIndent();
      this.m_backgroundScreenTexture = this.m_backgroundTextureFromConstructor ?? MyGuiScreenLoading.GetRandomBackgroundTexture();
      this.m_gameLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";
      if (this.m_screenToUnload != null)
      {
        this.m_screenToUnload.IsLoaded = false;
        this.m_screenToUnload.CloseScreenNow();
      }
      base.LoadContent();
      MyRenderProxy.LimitMaxQueueSize = true;
      if (this.m_screenToLoad != null && !this.m_loadInDrawFinished && this.m_loadFinished)
      {
        this.m_screenToLoad.State = MyGuiScreenState.OPENING;
        this.m_screenToLoad.LoadContent();
      }
      else
        this.m_loadFinished = false;
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyGuiScreenLoading.LoadContent - END");
    }

    private static string GetRandomBackgroundTexture() => "Textures\\GUI\\Screens\\loading_background_" + MyUtils.GetRandomInt(MyPerGameSettings.GUI.LoadingScreenIndexRange.X, MyPerGameSettings.GUI.LoadingScreenIndexRange.Y + 1).ToString().PadLeft(3, '0') + ".dds";

    public override void UnloadContent()
    {
      if (this.m_backgroundScreenTexture != null)
        MyRenderProxy.UnloadTexture(this.m_backgroundScreenTexture);
      if (this.m_backgroundTextureFromConstructor != null)
        MyRenderProxy.UnloadTexture(this.m_backgroundTextureFromConstructor);
      if (this.m_backgroundScreenTexture != null)
        MyRenderProxy.UnloadTexture(this.m_rotatingWheelTexture);
      if (this.m_screenToLoad != null && !this.m_loadFinished && this.m_loadInDrawFinished)
      {
        this.m_screenToLoad.UnloadContent();
        this.m_screenToLoad.UnloadData();
        this.m_screenToLoad = (MyGuiScreenBase) null;
      }
      if (this.m_screenToLoad != null && !this.m_loadInDrawFinished)
        this.m_screenToLoad.UnloadContent();
      MyRenderProxy.LimitMaxQueueSize = false;
      base.UnloadContent();
      MyGuiScreenLoading.Static = (MyGuiScreenLoading) null;
    }

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      if (this.State == MyGuiScreenState.OPENED && !this.m_loadFinished)
      {
        this.m_loadFinished = true;
        MyHud.ScreenEffects.FadeScreen(0.0f);
        MyAudio.Static.Mute = true;
        MyAudio.Static.StopMusic();
        MyAudio.Static.ChangeGlobalVolume(0.0f, 0.0f);
        MyRenderProxy.DeferStateChanges(true);
        this.DrawLoading();
        if (this.m_screenToLoad != null)
        {
          MySandboxGame.Log.WriteLine("RunLoadingAction - START");
          this.RunLoad();
          MySandboxGame.Log.WriteLine("RunLoadingAction - END");
        }
        if (this.m_screenToLoad != null)
        {
          MyScreenManager.AddScreenNow(this.m_screenToLoad);
          this.m_screenToLoad.Update(false);
        }
        this.m_screenToLoad = (MyGuiScreenBase) null;
        this.m_wheel.ManualRotationUpdate = true;
      }
      ++this.m_streamingTimeout;
      bool flag = Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated || (MyMultiplayer.Static == null || !MyFakes.ENABLE_WAIT_UNTIL_MULTIPLAYER_READY) || this.m_isStreamed || MyFakes.LOADING_STREAMING_TIMEOUT_ENABLED && this.m_streamingTimeout >= MyGuiScreenLoading.STREAMING_TIMEOUT;
      if (this.m_loadFinished && (MySandboxGame.IsGameReady & flag && MySandboxGame.AreClipmapsReady || this.m_exceptionDuringLoad))
      {
        MyRenderProxy.DeferStateChanges(false);
        MyHud.ScreenEffects.FadeScreen(1f, 5f);
        if (!this.m_exceptionDuringLoad && this.OnScreenLoadingFinished != null)
        {
          this.OnScreenLoadingFinished();
          this.OnScreenLoadingFinished = (Action) null;
        }
        this.CloseScreenNow();
        this.DrawLoading();
        MyTimeSpan myTimeSpan = new MyTimeSpan(Stopwatch.GetTimestamp()) - this.m_loadingTimeStart;
        MySandboxGame.Log.WriteLine("Loading duration: " + (object) myTimeSpan.Seconds);
      }
      else if (this.m_loadFinished && !MySandboxGame.AreClipmapsReady && MySession.Static != null && MySession.Static.VoxelMaps.Instances.Count == 0)
        MySandboxGame.AreClipmapsReady = true;
      return true;
    }

    private void RunLoad()
    {
      this.m_exceptionDuringLoad = false;
      try
      {
        this.m_screenToLoad.RunLoadingAction();
      }
      catch (MyLoadingNeedXMLException ex)
      {
        this.m_exceptionDuringLoad = true;
        if (this.OnLoadingXMLAllowed != null)
        {
          this.UnloadOnException(false);
          if (MySandboxGame.Static.SuppressLoadingDialogs)
            this.OnLoadingXMLAllowed();
          else
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.LoadingNeedsXML), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (_param1 => this.OnLoadingXMLAllowed()))));
        }
        else
          this.OnLoadException((Exception) ex, new StringBuilder(ex.Message), 1.5f);
      }
      catch (MyLoadingException ex)
      {
        this.OnLoadException((Exception) ex, new StringBuilder(ex.Message), 1.5f);
        this.m_exceptionDuringLoad = true;
      }
      catch (OutOfMemoryException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        this.OnLoadException(ex, MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded));
        this.m_exceptionDuringLoad = true;
      }
    }

    protected override void OnClosed()
    {
      MyRenderProxy.DeferStateChanges(false);
      base.OnClosed();
      MyInput.Static.EnableInput(true);
      MyAudio.Static.Mute = false;
    }

    private void UnloadOnException(bool exitToMainMenu)
    {
      MyRenderProxy.DeferStateChanges(false);
      this.DrawLoading();
      this.m_screenToLoad = (MyGuiScreenBase) null;
      if (MyGuiScreenGamePlay.Static != null)
      {
        MyGuiScreenGamePlay.Static.UnloadData();
        MyGuiScreenGamePlay.Static.UnloadContent();
      }
      MySandboxGame.IsUpdateReady = true;
      MySandboxGame.AreClipmapsReady = true;
      MySandboxGame.RenderTasksFinished = true;
      if (exitToMainMenu)
        MySessionLoader.UnloadAndExitToMenu();
      else
        MySessionLoader.Unload();
    }

    private void OnLoadException(Exception e, StringBuilder errorText, float heightMultiplier = 1f)
    {
      MySandboxGame.Log.WriteLine("ERROR: Loading screen failed");
      MySandboxGame.Log.WriteLine(e);
      this.UnloadOnException(true);
      MyGuiScreenMessageBox messageBox;
      MyLoadingNeedDLCException exception;
      if ((exception = e as MyLoadingNeedDLCException) != null)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.RequiresAnyDlc);
        messageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScenarioRequiresDlc), (object) MyTexts.GetString(exception.RequiredDLC.DisplayName)), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          MyGameService.OpenDlcInShop(exception.RequiredDLC.AppId);
        })));
      }
      else
      {
        messageBox = MyGuiSandbox.CreateMessageBox(messageText: errorText, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        Vector2 vector2 = messageBox.Size.Value;
        vector2.Y *= heightMultiplier;
        messageBox.Size = new Vector2?(vector2);
        messageBox.RecreateControls(false);
      }
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate) => base.HandleInput(receivedFocusInThisUpdate);

    public bool DrawLoading()
    {
      MyRenderProxy.AfterUpdate(new MyTimeSpan?(), false);
      MyRenderProxy.BeforeUpdate();
      this.DrawInternal();
      int num = base.Draw() ? 1 : 0;
      MyRenderProxy.AfterUpdate(new MyTimeSpan?(), false);
      MyRenderProxy.BeforeUpdate();
      return num != 0;
    }

    private void DrawInternal()
    {
      Color color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 250);
      color.A = (byte) ((double) color.A * (double) this.m_transitionAlpha);
      MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", MyGuiManager.GetFullscreenRectangle(), Color.Black, false, true);
      Rectangle outRect;
      MyGuiManager.GetSafeHeightFullScreenPictureSize(MyGuiConstants.LOADING_BACKGROUND_TEXTURE_REAL_SIZE, out outRect);
      MyGuiManager.DrawSpriteBatch(this.m_backgroundScreenTexture, outRect, new Color(new Vector4(1f, 1f, 1f, this.m_transitionAlpha)), true, true);
      MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background_fade.dds", outRect, new Color(new Vector4(1f, 1f, 1f, this.m_transitionAlpha)), true, true);
      MyGuiSandbox.DrawGameLogoHandler(this.m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
      MyGuiScreenLoading.LastBackgroundTexture = this.m_backgroundScreenTexture;
      MyGuiManager.DrawString(this.m_font, MyTexts.GetString(MyCommonTexts.LoadingPleaseWaitUppercase), MyGuiConstants.LOADING_PLEASE_WAIT_POSITION, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.1f, new Color?(new Color(MyGuiConstants.LOADING_PLEASE_WAIT_COLOR * this.m_transitionAlpha)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
      if (string.IsNullOrEmpty(this.m_customTextFromConstructor))
      {
        string font = this.m_font;
        Vector2 vector2 = this.m_multiTextControl.GetPositionAbsoluteBottomLeft() + new Vector2((float) (((double) this.m_multiTextControl.Size.X - (double) this.m_multiTextControl.TextSize.X) * 0.5 + 0.025000000372529), 0.025f);
        string text = this.m_authorWithDash.ToString();
        Vector2 normalizedCoord = vector2;
        double scaleWithLanguage = (double) MyGuiSandbox.GetDefaultTextScaleWithLanguage();
        Color? colorMask = new Color?();
        MyGuiManager.DrawString(font, text, normalizedCoord, (float) scaleWithLanguage, colorMask);
      }
      this.m_multiTextControl.Draw(1f, 1f);
    }

    public override bool Draw()
    {
      this.DrawInternal();
      return base.Draw();
    }

    private void RefreshText()
    {
      if (!string.IsNullOrEmpty(this.m_customTextFromConstructor))
        return;
      this.m_multiTextControl.TextEnum = MyStringId.GetOrCompute(this.m_currentText.ToString());
      if (!(this.m_currentText is MyLoadingScreenQuote))
        return;
      this.m_authorWithDash.Clear().Append("- ").AppendStringBuilder(MyTexts.Get((this.m_currentText as MyLoadingScreenQuote).Author)).Append(" -");
    }

    public override void OnRemoved() => base.OnRemoved();
  }
}
