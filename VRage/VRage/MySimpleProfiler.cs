// Decompiled with JetBrains decompiler
// Type: VRage.MySimpleProfiler
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage
{
  public class MySimpleProfiler
  {
    public static bool ENABLE_SIMPLE_PROFILER = true;
    private const int MAX_LEVELS = 100;
    private const int MAX_ITEMS_IN_SYNC_QUEUE = 20;
    private static volatile Dictionary<string, MySimpleProfiler.MySimpleProfilingBlock> m_profilingBlocks = new Dictionary<string, MySimpleProfiler.MySimpleProfilingBlock>();
    private static readonly ConcurrentQueue<MySimpleProfiler.MySimpleProfilingBlock> m_addUponSync = new ConcurrentQueue<MySimpleProfiler.MySimpleProfilingBlock>();
    [ThreadStatic]
    private static Stack<MySimpleProfiler.TimeKeepingItem> m_timers;
    private static readonly Stack<string> m_gpuBlocks = new Stack<string>();
    private static bool m_performanceTestEnabled;
    private static readonly List<int> m_updateTimes = new List<int>();
    private static readonly List<int> m_renderTimes = new List<int>();
    private static readonly List<int> m_gpuTimes = new List<int>();
    private static readonly List<int> m_memoryAllocs = new List<int>();
    private static ulong m_lastAllocationStamp = 0;
    private static int m_skipFrames;

    public static event Action<MySimpleProfiler.MySimpleProfilingBlock> ShowPerformanceWarning;

    public static Dictionary<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning> CurrentWarnings { get; private set; }

    private static bool SkipProfiling => MySimpleProfiler.m_skipFrames > 0;

    static MySimpleProfiler()
    {
      MySimpleProfiler.CurrentWarnings = new Dictionary<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning>();
      MySimpleProfiler.Reset(true);
    }

    public static void BeginBlock(string key, MySimpleProfiler.ProfilingBlockType type = MySimpleProfiler.ProfilingBlockType.OTHER)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      MySimpleProfiler.Begin(key, type, nameof (BeginBlock));
    }

    public static void Begin(
      string key,
      MySimpleProfiler.ProfilingBlockType type = MySimpleProfiler.ProfilingBlockType.OTHER,
      [CallerMemberName] string callingMember = null)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      if (MySimpleProfiler.m_timers == null)
        MySimpleProfiler.m_timers = new Stack<MySimpleProfiler.TimeKeepingItem>();
      MySimpleProfiler.MySimpleProfilingBlock orMakeBlock = MySimpleProfiler.GetOrMakeBlock(key, type);
      MySimpleProfiler.m_timers.Push(new MySimpleProfiler.TimeKeepingItem(callingMember, orMakeBlock));
    }

    public static void End([CallerMemberName] string callingMember = "")
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      MySimpleProfiler.EndNoMemberPairingCheck();
    }

    public static void EndNoMemberPairingCheck()
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER || MySimpleProfiler.m_timers.Count == 0)
        return;
      double microseconds = new MyTimeSpan(Stopwatch.GetTimestamp()).Microseconds;
      MySimpleProfiler.TimeKeepingItem timeKeepingItem = MySimpleProfiler.m_timers.Pop();
      timeKeepingItem.ProfilingBlock.CommitTime((int) (microseconds - timeKeepingItem.Timespan.Microseconds));
    }

    public static void EndMemberPairingCheck([CallerMemberName] string callingMember = "")
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER || MySimpleProfiler.m_timers.Count == 0)
        return;
      double microseconds = new MyTimeSpan(Stopwatch.GetTimestamp()).Microseconds;
      MySimpleProfiler.TimeKeepingItem timeKeepingItem = MySimpleProfiler.m_timers.Pop();
      if (callingMember != timeKeepingItem.InvokingMember)
        MySimpleProfiler.EndMemberPairingCheck(callingMember);
      timeKeepingItem.ProfilingBlock.CommitTime((int) (microseconds - timeKeepingItem.Timespan.Microseconds));
    }

    public static void BeginGPUBlock(string key)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      MySimpleProfiler.m_gpuBlocks.Push(key);
      if (MySimpleProfiler.m_gpuBlocks.Count <= 100)
        return;
      MySimpleProfiler.m_gpuBlocks.Clear();
    }

    public static void EndGPUBlock(MyTimeSpan time)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER || MySimpleProfiler.m_gpuBlocks.Count == 0)
        return;
      string key = MySimpleProfiler.m_gpuBlocks.Pop();
      if (!MySimpleProfiler.m_profilingBlocks.ContainsKey(key))
        return;
      MySimpleProfiler.GetOrMakeBlock(key, MySimpleProfiler.ProfilingBlockType.GPU).CommitTime((int) time.Microseconds);
    }

    private static MySimpleProfiler.MySimpleProfilingBlock GetOrMakeBlock(
      string key,
      MySimpleProfiler.ProfilingBlockType type,
      bool forceAdd = false)
    {
      MySimpleProfiler.MySimpleProfilingBlock simpleProfilingBlock1;
      if (MySimpleProfiler.m_profilingBlocks.TryGetValue(key, out simpleProfilingBlock1))
        return simpleProfilingBlock1;
      MySimpleProfiler.MySimpleProfilingBlock simpleProfilingBlock2 = new MySimpleProfiler.MySimpleProfilingBlock(key, type);
      if (forceAdd || MySimpleProfiler.m_addUponSync.Count < 20)
        MySimpleProfiler.m_addUponSync.Enqueue(simpleProfilingBlock2);
      return simpleProfilingBlock2;
    }

    public static void Reset(bool resetFrameCounter = false)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      MySimpleProfiler.CurrentWarnings.Clear();
      MySimpleProfiler.m_profilingBlocks = new Dictionary<string, MySimpleProfiler.MySimpleProfilingBlock>();
      int microseconds1 = (int) MyTimeSpan.FromMilliseconds(100.0).Microseconds;
      int microseconds2 = (int) MyTimeSpan.FromMilliseconds(40.0).Microseconds;
      MySimpleProfiler.SetBlockSettings("GPUFrame", microseconds1, microseconds2, MySimpleProfiler.ProfilingBlockType.GPU);
      MySimpleProfiler.SetBlockSettings("RenderFrame", microseconds1, microseconds2, MySimpleProfiler.ProfilingBlockType.RENDER);
      if (!resetFrameCounter)
        return;
      MySimpleProfiler.m_skipFrames = 10;
    }

    public static void Commit()
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      Dictionary<string, MySimpleProfiler.MySimpleProfilingBlock> dictionary = MySimpleProfiler.m_profilingBlocks;
      bool flag = false;
      MySimpleProfiler.MySimpleProfilingBlock result;
      while (MySimpleProfiler.m_addUponSync.TryDequeue(out result))
      {
        if (!flag)
        {
          flag = true;
          dictionary = new Dictionary<string, MySimpleProfiler.MySimpleProfilingBlock>((IDictionary<string, MySimpleProfiler.MySimpleProfilingBlock>) dictionary);
        }
        string name = result.Name;
        MySimpleProfiler.MySimpleProfilingBlock simpleProfilingBlock;
        if (dictionary.TryGetValue(name, out simpleProfilingBlock))
          simpleProfilingBlock.MergeFrom(result);
        else
          dictionary.Add(name, result);
      }
      if (flag)
        MySimpleProfiler.m_profilingBlocks = dictionary;
      foreach (MySimpleProfiler.MySimpleProfilingBlock block in dictionary.Values)
      {
        int tickTime;
        int avgTime;
        block.CommitSimulationFrame(out tickTime, out avgTime);
        MySimpleProfiler.CheckPerformance(block, tickTime, avgTime);
        if (MySimpleProfiler.m_performanceTestEnabled)
        {
          if (block.Name == "UpdateFrame")
            MySimpleProfiler.m_updateTimes.Add(tickTime);
          else if (block.Name == "RenderFrame")
            MySimpleProfiler.m_renderTimes.Add(tickTime);
          else if (block.Name == "GPUFrame")
            MySimpleProfiler.m_gpuTimes.Add(avgTime);
        }
      }
      if (MySimpleProfiler.m_performanceTestEnabled)
      {
        ulong allocationsStamp = MyVRage.Platform.System.GetGlobalAllocationsStamp();
        MySimpleProfiler.m_memoryAllocs.Add((int) ((long) allocationsStamp - (long) MySimpleProfiler.m_lastAllocationStamp));
        MySimpleProfiler.m_lastAllocationStamp = allocationsStamp;
      }
      foreach (KeyValuePair<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning> currentWarning in MySimpleProfiler.CurrentWarnings)
        ++currentWarning.Value.Time;
      if (MySimpleProfiler.m_skipFrames <= 0)
        return;
      --MySimpleProfiler.m_skipFrames;
      if (MySimpleProfiler.m_skipFrames != 0)
        return;
      MySimpleProfiler.Reset();
    }

    public static void SetBlockSettings(
      string key,
      int thresholdFrame = 100000,
      int thresholdAverage = 10000,
      MySimpleProfiler.ProfilingBlockType type = MySimpleProfiler.ProfilingBlockType.OTHER)
    {
      if (!MySimpleProfiler.ENABLE_SIMPLE_PROFILER)
        return;
      MySimpleProfiler.MySimpleProfilingBlock orMakeBlock = MySimpleProfiler.GetOrMakeBlock(key, type, true);
      orMakeBlock.ThresholdFrame = thresholdFrame;
      orMakeBlock.ThresholdAverage = thresholdAverage;
    }

    private static void CheckPerformance(
      MySimpleProfiler.MySimpleProfilingBlock block,
      int tickTime,
      int average)
    {
      if (block.Type == MySimpleProfiler.ProfilingBlockType.INTERNAL || block.Type == MySimpleProfiler.ProfilingBlockType.INTERNALGPU)
        return;
      bool flag = false;
      if (block.ThresholdFrame > 0)
        flag |= tickTime > block.ThresholdFrame;
      else if (block.ThresholdFrame < 0)
        flag |= tickTime < -block.ThresholdFrame;
      if (block.ThresholdAverage > 0)
        flag |= average > block.ThresholdAverage;
      else if (block.ThresholdAverage < 0)
        flag |= average < -block.ThresholdAverage;
      if (!flag || MySimpleProfiler.SkipProfiling)
        return;
      MySimpleProfiler.InvokePerformanceWarning(block);
    }

    public static void ShowServerPerformanceWarning(
      string key,
      MySimpleProfiler.ProfilingBlockType type)
    {
      MySimpleProfiler.InvokePerformanceWarning(MySimpleProfiler.GetOrMakeBlock(key, type));
    }

    private static void AddWarningToCurrent(MySimpleProfiler.MySimpleProfilingBlock block)
    {
      if (MySimpleProfiler.CurrentWarnings.ContainsKey(block))
        MySimpleProfiler.CurrentWarnings[block].Time = 0;
      else
        MySimpleProfiler.CurrentWarnings.Add(block, new MySimpleProfiler.PerformanceWarning(block));
    }

    private static void InvokePerformanceWarning(MySimpleProfiler.MySimpleProfilingBlock block)
    {
      MySimpleProfiler.AddWarningToCurrent(block);
      MySimpleProfiler.ShowPerformanceWarning.InvokeIfNotNull<MySimpleProfiler.MySimpleProfilingBlock>(block);
    }

    public static void LogPerformanceTestResults()
    {
      if (!MySimpleProfiler.m_performanceTestEnabled)
        return;
      GC.Collect();
      long totalMemory = GC.GetTotalMemory(true);
      Stream stream = MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Performance_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".csv"));
      StreamWriter streamWriter = new StreamWriter(stream, (Encoding) new UTF8Encoding(false, false));
      streamWriter.WriteLine("Update, Render, GPU, Memory");
      for (int index = 0; index < MySimpleProfiler.m_updateTimes.Count && (index < MySimpleProfiler.m_renderTimes.Count && index < MySimpleProfiler.m_gpuTimes.Count) && index < MySimpleProfiler.m_memoryAllocs.Count; ++index)
      {
        object[] objArray = new object[4]
        {
          (object) MySimpleProfiler.m_updateTimes[index],
          (object) MySimpleProfiler.m_renderTimes[index],
          (object) MySimpleProfiler.m_gpuTimes[index],
          (object) MySimpleProfiler.m_memoryAllocs[index]
        };
        streamWriter.WriteLine("{0},{1},{2},{3}", objArray);
      }
      streamWriter.WriteLine("Final memory: {0}", (object) totalMemory);
      streamWriter.Close();
      stream.Close();
    }

    public static void AttachTestingTool() => MySimpleProfiler.m_performanceTestEnabled = MyVRage.Platform.System.IsAllocationProfilingReady;

    public enum ProfilingBlockType : byte
    {
      GPU,
      MOD,
      BLOCK,
      RENDER,
      OTHER,
      INTERNAL,
      INTERNALGPU,
    }

    public class MySimpleProfilingBlock
    {
      public const int MEASURE_AVG_OVER_FRAMES = 60;
      public readonly string Name;
      public readonly MyStringId Description;
      public readonly MyStringId DescriptionSimple;
      public readonly MyStringId DisplayStringId;
      public readonly MySimpleProfiler.ProfilingBlockType Type;
      public int ThresholdFrame = 100000;
      public int ThresholdAverage = 10000;
      private int m_tickTime;
      private float m_avgTickTime;

      public string DisplayName => !(this.DisplayStringId == MyStringId.NullOrEmpty) ? MyTexts.GetString(this.DisplayStringId) : this.Name;

      public MySimpleProfilingBlock(string key, MySimpleProfiler.ProfilingBlockType type)
      {
        this.Name = key;
        this.Type = type;
        if (type == MySimpleProfiler.ProfilingBlockType.MOD)
        {
          this.ThresholdFrame = 50000;
          this.ThresholdAverage = 10000;
          this.DisplayStringId = MyStringId.GetOrCompute(key);
          this.Description = MyStringId.TryGet("PerformanceWarningAreaModsDescription");
        }
        else
        {
          this.DisplayStringId = MyStringId.TryGet("PerformanceWarningArea" + key);
          this.Description = MyStringId.TryGet("PerformanceWarningArea" + key + nameof (Description));
          this.DescriptionSimple = MyStringId.TryGet("PerformanceWarningArea" + key + nameof (DescriptionSimple));
        }
        if (!(this.DisplayStringId == MyStringId.NullOrEmpty) || type != MySimpleProfiler.ProfilingBlockType.GPU)
          return;
        this.DisplayStringId = MyStringId.TryGet("PerformanceWarningAreaRenderGPU");
        this.Description = MyStringId.TryGet("PerformanceWarningAreaRenderGPUDescription");
      }

      public void CommitTime(int microseconds)
      {
        if (this.Type != MySimpleProfiler.ProfilingBlockType.GPU && this.Type != MySimpleProfiler.ProfilingBlockType.INTERNALGPU && this.Type != MySimpleProfiler.ProfilingBlockType.RENDER)
          Interlocked.Add(ref this.m_tickTime, microseconds);
        else
          this.CommitAvgTime(microseconds);
      }

      public void CommitSimulationFrame(out int tickTime, out int avgTime)
      {
        if (this.Type != MySimpleProfiler.ProfilingBlockType.GPU && this.Type != MySimpleProfiler.ProfilingBlockType.RENDER)
        {
          tickTime = Interlocked.Exchange(ref this.m_tickTime, 0);
          this.CommitAvgTime(tickTime);
          avgTime = (int) this.m_avgTickTime;
        }
        else
        {
          tickTime = 0;
          avgTime = (int) Interlocked.CompareExchange(ref this.m_avgTickTime, 0.0f, 0.0f);
        }
      }

      private void CommitAvgTime(int tickTime)
      {
        float avgTickTime = this.m_avgTickTime;
        this.m_avgTickTime += (float) (((double) tickTime - (double) avgTickTime) / 60.0);
      }

      public void MergeFrom(MySimpleProfiler.MySimpleProfilingBlock other) => this.CommitTime(other.m_tickTime);
    }

    public class PerformanceWarning
    {
      public int Time;
      public MySimpleProfiler.MySimpleProfilingBlock Block;

      public PerformanceWarning(MySimpleProfiler.MySimpleProfilingBlock block)
      {
        this.Time = 0;
        this.Block = block;
      }
    }

    private struct TimeKeepingItem
    {
      public readonly string InvokingMember;
      public readonly MyTimeSpan Timespan;
      public readonly MySimpleProfiler.MySimpleProfilingBlock ProfilingBlock;

      public TimeKeepingItem(
        string invokingMember,
        MySimpleProfiler.MySimpleProfilingBlock profilingBlock,
        MyTimeSpan? timeSpan = null)
      {
        this.InvokingMember = invokingMember;
        this.ProfilingBlock = profilingBlock;
        this.Timespan = timeSpan ?? new MyTimeSpan(Stopwatch.GetTimestamp());
      }
    }
  }
}
