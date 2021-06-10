// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugStatistics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenDebugStatistics : MyGuiScreenDebugBase
  {
    private static StringBuilder m_frameDebugText = new StringBuilder(1024);
    private static StringBuilder m_frameDebugTextRA = new StringBuilder(2048);
    private static List<StringBuilder> m_texts = new List<StringBuilder>(32);
    private static List<StringBuilder> m_rightAlignedtexts = new List<StringBuilder>(32);
    private List<MyGuiScreenDebugStatistics.Tab> m_tabs;
    private int m_currentTab;
    private List<MyKeys> m_pressedKeys = new List<MyKeys>(10);
    private static List<StringBuilder> m_statsStrings = new List<StringBuilder>();
    private static int m_stringIndex = 0;

    public MyGuiScreenDebugStatistics()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(new Vector2()), new Vector4?(), true)
    {
      this.m_isTopMostScreen = true;
      this.m_drawEvenWithoutFocus = true;
      this.CanHaveFocus = false;
      this.m_canShareInput = false;
      this.m_tabs = new List<MyGuiScreenDebugStatistics.Tab>()
      {
        new MyGuiScreenDebugStatistics.Tab("Stats", new Action(this.DrawStats)),
        new MyGuiScreenDebugStatistics.Tab("Keys", new Action(this.DrawKeys)),
        new MyGuiScreenDebugStatistics.Tab("Sounds", new Action(MyGuiScreenDebugStatistics.DrawSounds)),
        new MyGuiScreenDebugStatistics.Tab("Network", new Action(this.DrawNetworkStats))
      };
    }

    public bool Cycle()
    {
      ++this.m_currentTab;
      if (this.m_currentTab != this.m_tabs.Count)
        return true;
      this.m_currentTab = 0;
      return false;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugStatistics);

    public void AddToFrameDebugText(string s) => MyGuiScreenDebugStatistics.m_frameDebugText.AppendLine(s);

    public void AddToFrameDebugText(StringBuilder s)
    {
      MyGuiScreenDebugStatistics.m_frameDebugText.AppendStringBuilder(s);
      MyGuiScreenDebugStatistics.m_frameDebugText.AppendLine();
    }

    public void AddDebugTextRA(string s)
    {
      MyGuiScreenDebugStatistics.m_frameDebugTextRA.Append(s);
      MyGuiScreenDebugStatistics.m_frameDebugTextRA.AppendLine();
    }

    public void AddDebugTextRA(StringBuilder s)
    {
      MyGuiScreenDebugStatistics.m_frameDebugTextRA.AppendStringBuilder(s);
      MyGuiScreenDebugStatistics.m_frameDebugTextRA.AppendLine();
    }

    public void ClearFrameDebugText()
    {
      MyGuiScreenDebugStatistics.m_frameDebugText.Clear();
      MyGuiScreenDebugStatistics.m_frameDebugTextRA.Clear();
    }

    public Vector2 GetScreenLeftTopPosition()
    {
      double num = 25.0 * (double) MyGuiManager.GetSafeScreenScale();
      MyGuiManager.GetSafeFullscreenRectangle();
      return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float) num, (float) num));
    }

    public Vector2 GetScreenRightTopPosition()
    {
      float y = 25f * MyGuiManager.GetSafeScreenScale();
      return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float) MyGuiManager.GetSafeFullscreenRectangle().Width - y, y));
    }

    public static StringBuilder StringBuilderCache
    {
      get
      {
        if (MyGuiScreenDebugStatistics.m_stringIndex >= MyGuiScreenDebugStatistics.m_statsStrings.Count)
          MyGuiScreenDebugStatistics.m_statsStrings.Add(new StringBuilder(1024));
        return MyGuiScreenDebugStatistics.m_statsStrings[MyGuiScreenDebugStatistics.m_stringIndex++].Clear();
      }
    }

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      this.m_tabs[this.m_currentTab].Draw();
      return true;
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      float statisticsRowDistance = MyGuiConstants.DEBUG_STATISTICS_ROW_DISTANCE;
      float scale = MyGuiConstants.DEBUG_STATISTICS_TEXT_SCALE * 0.9f;
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.Clear());
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.m_frameDebugText);
      MyGuiScreenDebugStatistics.m_rightAlignedtexts.Add(MyGuiScreenDebugStatistics.m_frameDebugTextRA);
      Vector2 screenLeftTopPosition = this.GetScreenLeftTopPosition();
      Vector2 rightTopPosition = this.GetScreenRightTopPosition();
      string font = "Debug";
      float x = 0.0f;
      for (int index = 0; index < this.m_tabs.Count; ++index)
      {
        MyGuiScreenDebugStatistics.Tab tab = this.m_tabs[index];
        bool flag = index == this.m_currentTab;
        StringBuilder text = flag ? tab.NameUpper : tab.Name;
        MyGuiManager.DrawString(font, text.ToString(), screenLeftTopPosition + new Vector2(x, 0.0f), scale * 1.4f, new Color?(flag ? Color.White : Color.Yellow));
        x += MyGuiManager.MeasureString(font, text, scale * 1.4f).X + 0.01f;
      }
      screenLeftTopPosition.Y += statisticsRowDistance * 1.4f;
      for (int index = 0; index < MyGuiScreenDebugStatistics.m_texts.Count; ++index)
        MyGuiManager.DrawString(font, MyGuiScreenDebugStatistics.m_texts[index].ToString(), screenLeftTopPosition + new Vector2(0.0f, (float) index * statisticsRowDistance), scale, new Color?(Color.Yellow));
      for (int index = 0; index < MyGuiScreenDebugStatistics.m_rightAlignedtexts.Count; ++index)
        MyGuiManager.DrawString(font, MyGuiScreenDebugStatistics.m_rightAlignedtexts[index].ToString(), rightTopPosition + new Vector2(-0.3f, (float) index * statisticsRowDistance), scale, new Color?(Color.Yellow));
      this.ClearFrameDebugText();
      MyGuiScreenDebugStatistics.m_stringIndex = 0;
      MyGuiScreenDebugStatistics.m_texts.Clear();
      MyGuiScreenDebugStatistics.m_rightAlignedtexts.Clear();
      return true;
    }

    private static StringBuilder GetFormatedVector3(
      StringBuilder sb,
      string before,
      Vector3D value,
      string after = "")
    {
      sb.Clear();
      sb.Append(before);
      sb.Append("{");
      sb.ConcatFormat<double>("{0: #,000} ", value.X);
      sb.ConcatFormat<double>("{0: #,000} ", value.Y);
      sb.ConcatFormat<double>("{0: #,000} ", value.Z);
      sb.Append("}");
      sb.Append(after);
      return sb;
    }

    private void DrawStats()
    {
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedFloat("FPS: ", (float) MyFpsManager.GetFps()));
      MyGuiScreenDebugStatistics.m_texts.Add(new StringBuilder("Renderer: ").Append(MyRenderProxy.RendererInterfaceName()));
      if (MySector.MainCamera != null)
        MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.GetFormatedVector3(MyGuiScreenDebugStatistics.StringBuilderCache, "Camera pos: ", MySector.MainCamera.Position));
      MyGuiScreenDebugStatistics.m_texts.Add(MyScreenManager.GetGuiScreensForDebug());
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedBool("Paused: ", MySandboxGame.IsPaused));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total GAME-PLAY Time: ", TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds)));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Session Time: ", MySession.Static == null ? new TimeSpan(0L) : MySession.Static.ElapsedPlayTime));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Foot Time: ", MySession.Static == null ? new TimeSpan(0L) : MySession.Static.TimeOnFoot));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Jetpack Time: ", MySession.Static == null ? new TimeSpan(0L) : MySession.Static.TimeOnJetpack));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Small Ship Time: ", MySession.Static == null ? new TimeSpan(0L) : MySession.Static.TimePilotingSmallShip));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Big Ship Time: ", MySession.Static == null ? new TimeSpan(0L) : MySession.Static.TimePilotingBigShip));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedTimeSpan("Total Time: ", TimeSpan.FromMilliseconds((double) MySandboxGame.TotalTimeInMilliseconds)));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedLong("GC.GetTotalMemory: ", GC.GetTotalMemory(false), " bytes"));
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedFloat("Allocated videomemory: ", 0.0f, " MB"));
    }

    private static void DrawSounds()
    {
      MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.GetFormatedInt("Sound Instances Total: ", MyAudio.Static.GetSoundInstancesTotal2D()).Append(" 2d / ").AppendInt32(MyAudio.Static.GetSoundInstancesTotal3D()).Append(" 3d"));
      if (MyMusicController.Static != null)
      {
        if (MyMusicController.Static.CategoryPlaying.Equals(MyStringId.NullOrEmpty))
          MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.Append("No music playing, last category: " + MyMusicController.Static.CategoryLast.ToString() + ", next track in ").AppendDecimal(Math.Max(0.0f, MyMusicController.Static.NextMusicTrackIn), 1).Append("s"));
        else
          MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.Append("Playing music category: " + MyMusicController.Static.CategoryPlaying.ToString()));
      }
      if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
        MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.Append("Current reverb effect: " + (MyAudio.Static.EnableReverb ? MyEntityReverbDetectorComponent.CurrentReverbPreset.ToLower() : "disabled")));
      StringBuilder stringBuilderCache = MyGuiScreenDebugStatistics.StringBuilderCache;
      MyAudio.Static.WriteDebugInfo(stringBuilderCache);
      MyGuiScreenDebugStatistics.m_texts.Add(stringBuilderCache);
      for (int index = 0; index < 8; ++index)
        MyGuiScreenDebugStatistics.m_texts.Add(MyGuiScreenDebugStatistics.StringBuilderCache.Clear());
      MyGuiScreenDebugStatistics.m_texts.Add(new StringBuilder("Last played sounds:"));
      MyAudio.Static.EnumerateLastSounds((Action<StringBuilder, bool>) ((name, colored) => MyGuiScreenDebugStatistics.m_texts.Add(name)));
    }

    private void DrawKeys()
    {
      MyInput.Static.GetPressedKeys(this.m_pressedKeys);
      this.AddPressedKeys("Current keys              : ", this.m_pressedKeys);
    }

    private void AddPressedKeys(string groupName, List<MyKeys> keys)
    {
      StringBuilder stringBuilderCache = MyGuiScreenDebugStatistics.StringBuilderCache;
      stringBuilderCache.Append(groupName);
      for (int index = 0; index < keys.Count; ++index)
      {
        if (index > 0)
          stringBuilderCache.Append(", ");
        stringBuilderCache.Append(MyInput.Static.GetKeyName(keys[index]));
      }
      MyGuiScreenDebugStatistics.m_texts.Add(stringBuilderCache);
    }

    private void DrawNetworkStats()
    {
      string detailedStats = MyGameService.Peer2Peer?.DetailedStats;
      if (string.IsNullOrEmpty(detailedStats))
        return;
      MyGuiScreenDebugStatistics.m_texts.Add(new StringBuilder(detailedStats));
    }

    private StringBuilder GetShadowText(string text, int cascade, int value)
    {
      StringBuilder stringBuilderCache = MyGuiScreenDebugStatistics.StringBuilderCache;
      stringBuilderCache.Clear();
      stringBuilderCache.ConcatFormat<string, int>("{0} (c {1}): ", text, cascade);
      stringBuilderCache.Concat(value);
      return stringBuilderCache;
    }

    private StringBuilder GetLodText(string text, int lod, int value)
    {
      StringBuilder stringBuilderCache = MyGuiScreenDebugStatistics.StringBuilderCache;
      stringBuilderCache.Clear();
      stringBuilderCache.ConcatFormat<string, int>("{0}_LOD{1}: ", text, lod);
      stringBuilderCache.Concat(value);
      return stringBuilderCache;
    }

    private readonly struct Tab
    {
      public readonly StringBuilder Name;
      public readonly StringBuilder NameUpper;
      public readonly Action Draw;

      public Tab(string name, Action draw)
      {
        this.Name = new StringBuilder(name);
        this.NameUpper = new StringBuilder(name.ToUpper());
        this.Draw = draw;
      }
    }
  }
}
