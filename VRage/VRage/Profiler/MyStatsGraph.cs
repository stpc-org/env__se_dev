// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyStatsGraph
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;

namespace VRage.Profiler
{
  public static class MyStatsGraph
  {
    public static string PROFILER_NAME = "Statistics";
    private static readonly MyProfiler m_profiler;
    private static readonly Stack<float> m_stack = new Stack<float>(32);

    public static bool Started { get; private set; }

    static MyStatsGraph()
    {
      MyStatsGraph.m_profiler = MyRenderProfiler.CreateProfiler(MyStatsGraph.PROFILER_NAME, "B");
      MyStatsGraph.m_profiler.AutoCommit = false;
      MyStatsGraph.m_profiler.SetNewLevelLimit(-1);
      MyStatsGraph.m_profiler.AutoScale = true;
      MyStatsGraph.m_profiler.IgnoreRoot = true;
    }

    private static MyTimeSpan? ToTime(this float customTime) => new MyTimeSpan?(MyTimeSpan.FromMilliseconds((double) customTime));

    public static void Begin(
      string blockName = null,
      int forceOrder = 2147483647,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      if (MyRenderProfiler.Paused)
        return;
      MyStatsGraph.m_profiler.StartBlock(blockName, member, line, file, forceOrder);
      MyStatsGraph.m_stack.Push(0.0f);
    }

    public static void End(
      float? bytesTransfered = null,
      float customValue = 0.0f,
      string customValueFormat = "",
      string byteFormat = "{0} B",
      string callFormat = null,
      int numCalls = 0,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      if (MyRenderProfiler.Paused)
        return;
      float num = MyStatsGraph.m_stack.Pop();
      float? nullable = bytesTransfered;
      float customTime = nullable.HasValue ? nullable.GetValueOrDefault() : num;
      MyStatsGraph.m_profiler.EndBlock(member, line, file, customTime.ToTime(), customValue, byteFormat, customValueFormat, callFormat, numCalls);
      if (MyStatsGraph.m_stack.Count <= 0)
        return;
      MyStatsGraph.m_stack.Push(MyStatsGraph.m_stack.Pop() + customTime);
    }

    public static void CustomTime(
      string name,
      float customTime,
      string timeFormat = null,
      float customValue = 0.0f,
      string customValueFormat = "",
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      if (MyRenderProfiler.Paused)
        return;
      MyStatsGraph.m_profiler.StartBlock(name, member, line, file);
      MyStatsGraph.m_profiler.EndBlock(member, line, file, customTime.ToTime(), customValue, timeFormat, customValueFormat);
    }

    public static void Commit()
    {
      if (MyRenderProfiler.Paused)
        MyStatsGraph.m_profiler.ClearFrame();
      else
        MyStatsGraph.m_profiler.CommitFrame();
    }

    public static void ProfileAdvanced(bool begin)
    {
      if (begin)
        MyStatsGraph.Begin("Advanced", member: nameof (ProfileAdvanced), line: 95, file: "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
      else
        MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (ProfileAdvanced), line: 99, file: "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
    }

    public static void ProfilePacketStatistics(bool begin)
    {
      if (begin)
        MyStatsGraph.Begin("Packet statistics", member: nameof (ProfilePacketStatistics), line: 107, file: "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
      else
        MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (ProfilePacketStatistics), line: 111, file: "E:\\Repo3\\Sources\\VRage\\Profiler\\MyStatsGraph.cs");
    }

    public static void Start()
    {
      if (!MyStatsGraph.Started && MyRenderProfiler.Paused)
        MyRenderProfiler.SwitchPause();
      MyStatsGraph.Started = true;
    }
  }
}
