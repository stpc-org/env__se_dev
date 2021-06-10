// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugReporting
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Game;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Reporting")]
  internal class MyGuiScreenDebugReporting : MyGuiScreenDebugBase
  {
    private MyGuiControlLabel m_cnt;
    private static int m_nextProfilerDumpIndex;
    private int m_recordCounter;

    public MyGuiScreenDebugReporting()
      : base()
    {
      MyGuiScreenDebugReporting.m_nextProfilerDumpIndex = MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpCount();
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugReporting);

    public override void RecreateControls(bool constructor)
    {
      this.m_scale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Reporting", new Vector4?(Color.Yellow.ToVector4()));
      this.AddButton("Send Logs", new Action<MyGuiControlButton>(this.SendLog));
      this.AddLabel("Profiler", (Vector4) Color.Yellow, 1f);
      this.m_cnt = this.AddLabel(string.Format("Dump Count: {0}", (object) MyGuiScreenDebugReporting.m_nextProfilerDumpIndex), (Vector4) Color.Yellow, 1f);
      this.AddButton("Record Profiler Dump", new Action<MyGuiControlButton>(this.RecordProfiler));
      this.AddButton("Clear Profiler Dumps", new Action<MyGuiControlButton>(this.CleanDumps));
      this.AddButton("Send Logs With Profiler", new Action<MyGuiControlButton>(this.SendLogWithProfilerData));
    }

    private void CleanDumps(MyGuiControlButton obj)
    {
      MyObjectBuilder_ProfilerSnapshot.ClearProfilerDumps();
      MyGuiScreenDebugReporting.m_nextProfilerDumpIndex = MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpCount();
      this.m_cnt.Text = string.Format("Dump Count: {0}", (object) MyGuiScreenDebugReporting.m_nextProfilerDumpIndex);
    }

    private void RecordProfiler(MyGuiControlButton obj)
    {
      MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Pause, 1, (string) null);
      MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.EnableShallowProfile, 0, (string) null);
      this.m_recordCounter = 5;
    }

    private void SendLog(MyGuiControlButton button)
    {
      MyLog.Default.WriteLine("Send log requested");
      MyLog.Default.Flush();
      MySandboxGame.NonInteractiveReportAction interactiveReport = MySandboxGame.PerformNotInteractiveReport;
      if (interactiveReport != null)
        interactiveReport(true);
      button.Text = "Sent";
    }

    private void SendLogWithProfilerData(MyGuiControlButton button)
    {
      MyLog.Default.WriteLine("Send logs with profiler data.");
      MyLog.Default.Flush();
      MySandboxGame.NonInteractiveReportAction interactiveReport = MySandboxGame.PerformNotInteractiveReport;
      if (interactiveReport != null)
        interactiveReport(true, Enumerable.Range(0, MyGuiScreenDebugReporting.m_nextProfilerDumpIndex).Select<int, string>(new Func<int, string>(MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpPath)));
      button.Text = "Sent";
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_recordCounter > 0)
      {
        --this.m_recordCounter;
        if (this.m_recordCounter == 0)
        {
          MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Pause, -1, (string) null);
          MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.EnableShallowProfile, 1, (string) null);
          MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.SaveToFile, MyGuiScreenDebugReporting.m_nextProfilerDumpIndex++, (string) null);
          VRage.Profiler.MyRenderProfiler.OnProfilerSnapshotSaved = (Action) (() =>
          {
            MyHud.Notifications?.Add((MyHudNotificationBase) new MyHudNotificationDebug("Profiler Snapshot Saved"));
            VRage.Profiler.MyRenderProfiler.OnProfilerSnapshotSaved = (Action) null;
            this.m_cnt.Text = string.Format("Dump Count: {0}", (object) MyGuiScreenDebugReporting.m_nextProfilerDumpIndex);
          });
        }
      }
      return base.Update(hasFocus);
    }
  }
}
