// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenIntroVideo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenIntroVideo : MyGuiScreenBase
  {
    private uint m_videoID = uint.MaxValue;
    private bool m_playbackStarted;
    private string[] m_videos;
    private string m_currentVideo = "";
    private List<MyGuiScreenIntroVideo.Subtitle> m_subtitles = new List<MyGuiScreenIntroVideo.Subtitle>();
    private int m_currentSubtitleIndex;
    private float m_volume = 1f;
    private int m_transitionTime = 300;
    private Vector4 m_colorMultiplier = Vector4.One;
    private static readonly string m_videoOverlay = "Textures\\GUI\\Screens\\main_menu_overlay.dds";
    private bool m_loop = true;
    private bool m_videoOverlayEnabled = true;

    public Vector4 OverlayColorMask { get; set; }

    public MyGuiScreenIntroVideo(string[] videos, uint videoId)
      : this(videos, false, false, true, 1f, true, 300, videoId)
    {
      MyRenderProxy.Settings.RenderThreadHighPriority = true;
      Thread.CurrentThread.Priority = ThreadPriority.Highest;
    }

    public MyGuiScreenIntroVideo(
      string[] videos,
      bool loop,
      bool videoOverlayEnabled,
      bool canHaveFocus,
      float volume,
      bool closeOnEsc,
      int transitionTime,
      uint videoId = 0)
      : base(new Vector2?(Vector2.Zero))
    {
      this.DrawMouseCursor = false;
      this.CanHaveFocus = canHaveFocus;
      this.m_closeOnEsc = closeOnEsc;
      this.m_drawEvenWithoutFocus = true;
      this.m_videos = videos;
      this.m_videoOverlayEnabled = videoOverlayEnabled;
      this.m_loop = loop;
      this.m_volume = volume;
      this.m_transitionTime = transitionTime;
      this.m_canCloseInCloseAllScreenCalls = false;
      if (videos == null && videoId != 0U)
      {
        this.m_transitionTime = 0;
        this.m_playbackStarted = true;
        this.m_videoID = videoId;
      }
      this.OverlayColorMask = new Vector4(1f, 1f, 1f, 1f);
    }

    public static MyGuiScreenIntroVideo CreateBackgroundScreen() => new MyGuiScreenIntroVideo(MyPerGameSettings.GUI.MainMenuBackgroundVideos, true, true, false, 0.0f, false, 1500);

    private static void AddCloseEvent(Action onVideoFinished, MyGuiScreenIntroVideo result) => result.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) => onVideoFinished());

    public override string GetFriendlyName() => nameof (MyGuiScreenIntroVideo);

    private void LoadRandomVideo()
    {
      int randomInt = MyUtils.GetRandomInt(0, this.m_videos.Length);
      if (this.m_videos.Length == 0)
        return;
      this.m_currentVideo = this.m_videos[randomInt];
    }

    public override void LoadContent()
    {
      if (this.m_videos != null)
      {
        this.m_playbackStarted = false;
        this.LoadRandomVideo();
      }
      base.LoadContent();
    }

    public override void CloseScreenNow(bool isUnloading = false)
    {
      if (this.State != MyGuiScreenState.CLOSED)
        this.UnloadContent();
      MyRenderProxy.Settings.RenderThreadHighPriority = false;
      Thread.CurrentThread.Priority = ThreadPriority.Normal;
      base.CloseScreenNow(isUnloading);
    }

    private void CloseVideo()
    {
      if (this.m_videoID == uint.MaxValue)
        return;
      MyRenderProxy.CloseVideo(this.m_videoID);
      this.m_videoID = uint.MaxValue;
    }

    public override void UnloadContent()
    {
      this.CloseVideo();
      this.m_currentVideo = "";
      base.UnloadContent();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewLeftMousePressed() && !MyInput.Static.IsNewRightMousePressed() && (!MyInput.Static.IsNewKeyPressed(MyKeys.Space) && !MyInput.Static.IsNewKeyPressed(MyKeys.Enter)) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CUTSCENE_SKIPPER, MyControlStateType.PRESSED))
        return;
      this.Canceling();
    }

    private void Loop()
    {
      this.m_currentSubtitleIndex = 0;
      this.LoadRandomVideo();
      this.TryPlayVideo();
    }

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      if (!this.m_playbackStarted)
      {
        this.TryPlayVideo();
        this.m_playbackStarted = true;
      }
      else
      {
        if (MyRenderProxy.IsVideoValid(this.m_videoID) && MyRenderProxy.GetVideoState(this.m_videoID) != VideoState.Playing)
        {
          if (this.m_loop)
            this.Loop();
          else
            this.CloseScreen(false);
        }
        if (this.State == MyGuiScreenState.CLOSING && MyRenderProxy.IsVideoValid(this.m_videoID))
          MyRenderProxy.SetVideoVolume(this.m_videoID, this.m_transitionAlpha);
      }
      return true;
    }

    public override int GetTransitionOpeningTime() => this.m_transitionTime;

    public override int GetTransitionClosingTime() => this.m_transitionTime;

    private void TryPlayVideo()
    {
      if (!MyFakes.ENABLE_VIDEO_PLAYER)
        return;
      this.CloseVideo();
      string str = Path.Combine(MyFileSystem.ContentPath, this.m_currentVideo);
      if (!File.Exists(str))
        return;
      this.m_videoID = MyRenderProxy.PlayVideo(str, this.m_volume);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      int num = base.CloseScreen(isUnloading) ? 1 : 0;
      MyRenderProxy.Settings.RenderThreadHighPriority = false;
      Thread.CurrentThread.Priority = ThreadPriority.Normal;
      if (num == 0)
        return num != 0;
      this.CloseVideo();
      return num != 0;
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      if (MyRenderProxy.IsVideoValid(this.m_videoID))
      {
        MyRenderProxy.UpdateVideo(this.m_videoID);
        Vector4 vector = this.m_colorMultiplier * this.m_transitionAlpha;
        MyRenderProxy.DrawVideo(this.m_videoID, MyGuiManager.GetSafeFullscreenRectangle(), new Color(vector), MyVideoRectangleFitMode.AutoFit, true);
      }
      if (this.m_videoOverlayEnabled)
        this.DrawVideoOverlay();
      return true;
    }

    private void DrawVideoOverlay() => MyGuiManager.DrawSpriteBatch(MyGuiScreenIntroVideo.m_videoOverlay, MyGuiManager.GetSafeFullscreenRectangle(), (Color) (this.OverlayColorMask * this.m_transitionAlpha), true, true);

    private struct Subtitle
    {
      public TimeSpan StartTime;
      public TimeSpan Length;
      public StringBuilder Text;

      public Subtitle(int startMs, int lengthMs, MyStringId textEnum)
      {
        this.StartTime = TimeSpan.FromMilliseconds((double) startMs);
        this.Length = TimeSpan.FromMilliseconds((double) lengthMs);
        this.Text = MyTexts.Get(textEnum);
      }
    }
  }
}
