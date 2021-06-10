// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.DebugInputComponents.MyTomasDDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
  public class MyTomasDDebugInputComponent : MyDebugComponent
  {
    private bool m_profiling;
    private List<MyTomasDDebugInputComponent.VoxelStreamingStats> m_statistics = new List<MyTomasDDebugInputComponent.VoxelStreamingStats>();
    private DateTime m_profStart;
    private float m_progress;
    private float m_profTime = 120f;
    private bool m_use_replay = true;
    private MyTomasDDebugInputComponent.VoxelStreamingStats m_median;

    public override string GetName() => "Tomas Drinovsky";

    public MyTomasDDebugInputComponent()
    {
      this.AddShortcut(MyKeys.P, true, false, false, false, (Func<string>) (() => "Profiling start/end"), (Func<bool>) (() =>
      {
        if (!this.m_profiling)
        {
          MyFpsManager.Reset();
          this.m_statistics.Clear();
          this.m_profStart = DateTime.UtcNow;
          if (this.m_use_replay)
            MySessionComponentReplay.Static.StartReplay();
          this.m_profiling = true;
        }
        else
          this.FinishProfiling();
        return true;
      }));
      this.AddShortcut(MyKeys.O, true, false, false, false, (Func<string>) (() => "Use replay"), (Func<bool>) (() =>
      {
        this.m_use_replay = !this.m_use_replay;
        return true;
      }));
      this.AddShortcut(MyKeys.L, true, false, false, false, (Func<string>) (() => "Reload world"), (Func<bool>) (() =>
      {
        MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
        return true;
      }));
    }

    private void FinishProfiling()
    {
      MySessionComponentReplay.Static.StopReplay();
      this.PrintData();
      this.ComputeData();
      this.m_profiling = false;
    }

    private void ComputeData()
    {
      if (this.m_statistics.Count == 0)
        return;
      int index = this.m_statistics.Count / 2;
      this.m_statistics.Sort((Comparison<MyTomasDDebugInputComponent.VoxelStreamingStats>) ((a, b) => a.FrameTime.CompareTo(b.FrameTime)));
      this.m_median.FrameTime = this.m_statistics[index].FrameTime;
      this.m_statistics.Sort((Comparison<MyTomasDDebugInputComponent.VoxelStreamingStats>) ((a, b) => a.CPUTimeSmooth.CompareTo(b.CPUTimeSmooth)));
      this.m_median.CPUTimeSmooth = this.m_statistics[index].CPUTimeSmooth;
      this.m_statistics.Sort((Comparison<MyTomasDDebugInputComponent.VoxelStreamingStats>) ((a, b) => a.GPUTimeSmooth.CompareTo(b.GPUTimeSmooth)));
      this.m_median.GPUTimeSmooth = this.m_statistics[index].GPUTimeSmooth;
    }

    private void PrintData()
    {
      double totalSeconds = (DateTime.UtcNow - this.m_profStart).TotalSeconds;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(string.Format("FrameTime,GPUTimeSmooth,CPUTimeSmooth,,,{0}", (object) totalSeconds));
      foreach (MyTomasDDebugInputComponent.VoxelStreamingStats statistic in this.m_statistics)
        stringBuilder.AppendLine(statistic.ToString());
      File.WriteAllText("C:\\Temp\\frameStats.csv", stringBuilder.ToString());
    }

    public override void Draw()
    {
      base.Draw();
      int num1 = 100;
      float scale = 0.6f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, (float) num1), string.Format("Profiling in progress: {0} {1}%", (object) this.m_profiling, (object) (int) ((double) this.m_progress * 100.0)), Color.White, scale);
      int num2 = num1 + 15;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, (float) num2), string.Format("Use replay: {0}", (object) this.m_use_replay), Color.White, scale);
      int num3 = num2 + 20;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, (float) num3), string.Format("MEDIAN FrameTime: {0}", (object) this.m_median.FrameTime), Color.White, scale);
      int num4 = num3 + 15;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, (float) num4), string.Format("MEDIAN CPUTimeSmooth: {0}", (object) this.m_median.CPUTimeSmooth), Color.White, scale);
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, (float) (num4 + 15)), string.Format("MEDIAN GPUTimeSmooth: {0}", (object) this.m_median.GPUTimeSmooth), Color.White, scale);
    }

    public override void DispatchUpdate()
    {
      base.DispatchUpdate();
      if (!this.m_profiling)
        return;
      this.m_statistics.Add(new MyTomasDDebugInputComponent.VoxelStreamingStats()
      {
        FrameTime = MyFpsManager.FrameTime,
        GPUTimeSmooth = MyRenderProxy.GPUTimeSmooth,
        CPUTimeSmooth = MyRenderProxy.CPUTimeSmooth
      });
    }

    public override void Update100()
    {
      base.Update100();
      if (!this.m_profiling)
        return;
      float totalSeconds = (float) (DateTime.UtcNow - this.m_profStart).TotalSeconds;
      if ((double) totalSeconds > (double) this.m_profTime)
        this.FinishProfiling();
      this.m_progress = totalSeconds / this.m_profTime;
    }

    private struct VoxelStreamingStats
    {
      public float FrameTime;
      public float GPUTimeSmooth;
      public float CPUTimeSmooth;
      public float TotalSwapsPerformed;

      public override string ToString() => string.Format("{0},{1},{2}", (object) this.FrameTime, (object) this.GPUTimeSmooth, (object) this.CPUTimeSmooth);
    }
  }
}
