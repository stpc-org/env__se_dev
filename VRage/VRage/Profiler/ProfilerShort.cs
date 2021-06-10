// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.ProfilerShort
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ParallelTasks;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;

namespace VRage.Profiler
{
  public static class ProfilerShort
  {
    public const string PerformanceProfilingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";
    private static MyRenderProfiler m_profiler;

    public static MyRenderProfiler Profiler
    {
      get => ProfilerShort.m_profiler;
      private set => ProfilerShort.m_profiler = value;
    }

    public static void SetProfiler(MyRenderProfiler profiler, bool simulation) => ProfilerShort.Profiler = profiler;

    public static void Init()
    {
      DelegateExtensions.SetupProfiler((Action<string>) (x => {}), (Action<int>) (x => {}));
      WorkItem.SetupProfiler((Action<MyProfiler.TaskType, string, long>) ((x, y, z) => {}), (Action) (() => {}), (Action<string>) (x => {}), (Action<float>) (x => {}), (Action<int, bool>) ((x, y) => {}));
    }

    public static bool Autocommit
    {
      get => MyRenderProfiler.GetAutocommit();
      set => MyRenderProfiler.SetAutocommit(value);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Begin(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BeginDeepTree(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BeginNextBlock(
      string blockName = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "",
      float previousBlockCustomValue = 0.0f)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void End(
      float customValue = 0.0f,
      MyTimeSpan? customTime = null,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void CustomValue(
      string name,
      float value,
      MyTimeSpan? customTime,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void End(
      float customValue,
      float customTimeMs,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void CustomValue(
      string name,
      float value,
      float customTimeMs,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskStarted(
      MyProfiler.TaskType taskType,
      string debugName,
      long scheduledTimestamp = -1)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskFinished(MyProfiler.TaskType? taskType = null, float customValue = 0.0f)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void CommitTask(MyProfiler.TaskInfo task)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnBeginSimulationFrame(long frameNumber)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void InitThread(int viewPriority, bool simulation)
    {
    }

    public static void Commit(bool? simulation = false)
    {
      if (ProfilerShort.Profiler == null)
        return;
      MyRenderProfiler.Commit(simulation, nameof (Commit), (int) sbyte.MaxValue, "E:\\Repo3\\Sources\\VRage\\Profiler\\ProfilerShort.cs");
    }

    public static void DestroyThread()
    {
      if (ProfilerShort.Profiler == null)
        return;
      MyRenderProfiler.DestroyThread();
    }
  }
}
