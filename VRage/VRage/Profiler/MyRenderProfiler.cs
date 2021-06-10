// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyRenderProfiler
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Profiler
{
  public abstract class MyRenderProfiler
  {
    private static readonly object m_pauseLock = new object();
    protected static RenderProfilerSortingOrder m_sortingOrder = RenderProfilerSortingOrder.MillisecondsLastFrame;
    protected static ProfilerGraphContent m_graphContent = ProfilerGraphContent.Elapsed;
    protected static BlockRender m_blockRender = BlockRender.Name;
    protected static SnapshotType m_dataType = SnapshotType.Online;
    public const string PERFORMANCE_PROFILING_SYMBOL = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";
    private static bool m_profilerProcessingEnabled = false;
    public static bool ShallowProfileOnly = true;
    public static bool AverageTimes = false;
    protected static readonly MyDrawArea MemoryGraphScale = new MyDrawArea(0.49f, 0.0f, 0.745f, 0.6f, 1f / 1000f);
    private static readonly MyDrawArea m_milisecondsGraphScale = new MyDrawArea(0.49f, 0.0f, 0.745f, 0.9f, 25f);
    private static readonly MyDrawArea m_allocationsGraphScale = new MyDrawArea(0.49f, 0.0f, 0.745f, 0.9f, 25000f);
    private static readonly Color[] m_colors = new Color[19]
    {
      new Color(0, 192, 192),
      Color.Orange,
      Color.BlueViolet * 1.5f,
      Color.BurlyWood,
      Color.Chartreuse,
      Color.CornflowerBlue,
      Color.Cyan,
      Color.ForestGreen,
      Color.Fuchsia,
      Color.Gold,
      Color.GreenYellow,
      Color.LightBlue,
      Color.LightGreen,
      Color.LimeGreen,
      Color.Magenta,
      Color.MintCream,
      Color.Orchid,
      Color.PeachPuff,
      Color.Purple
    };
    protected readonly StringBuilder Text = new StringBuilder(100);
    protected const bool ALLOCATION_PROFILING = false;
    protected static readonly MyProfilerBlock FpsBlock;
    protected static float m_fpsPctg;
    private static int m_pauseCount;
    public static bool Paused;
    public static Action GetProfilerFromServer;
    public static Action<int> SaveProfilerToFile;
    public static Action<int, bool> LoadProfilerFromFile;
    public static Action OnProfilerSnapshotSaved;
    [ThreadStatic]
    private static MyProfiler m_threadProfiler;
    private static MyProfiler m_gpuProfiler;
    public static List<MyProfiler> ThreadProfilers = new List<MyProfiler>(16);
    private static readonly List<MyProfiler> m_onlineThreadProfilers = MyRenderProfiler.ThreadProfilers;
    protected static MyProfiler m_selectedProfiler;
    protected static bool m_enabled;
    protected static int m_selectedFrame;
    private static int m_levelLimit;
    protected static bool m_useCustomFrame;
    protected static int m_frameLocalArea = MyProfiler.MAX_FRAMES;
    private int m_currentDumpNumber;
    protected static long m_targetTaskRenderTime;
    protected static long m_taskRenderDispersion = MyTimeSpan.FromMilliseconds(30.0).Ticks;
    public static ConcurrentQueue<MyRenderProfiler.FrameInfo> FrameTimestamps = new ConcurrentQueue<MyRenderProfiler.FrameInfo>();
    private static readonly ConcurrentQueue<MyRenderProfiler.FrameInfo> m_onlineFrameTimestamps = MyRenderProfiler.FrameTimestamps;
    private static MyTimeSpan m_nextAutoScale;
    private static readonly MyTimeSpan AUTO_SCALE_UPDATE = MyTimeSpan.FromSeconds(1.0);

    protected static bool ProfilerProcessingEnabled => MyRenderProfiler.m_profilerProcessingEnabled;

    public static bool ProfilerVisible => MyRenderProfiler.m_enabled;

    protected static Color IndexToColor(int index) => MyRenderProfiler.m_colors[index % MyRenderProfiler.m_colors.Length];

    private static MyProfiler GpuProfiler
    {
      get
      {
        MyProfiler myProfiler = MyRenderProfiler.m_gpuProfiler;
        if (myProfiler == null)
        {
          MyRenderProfiler.m_gpuProfiler = myProfiler = MyRenderProfiler.CreateProfiler("GPU");
          myProfiler.ViewPriority = 30;
          lock (MyRenderProfiler.ThreadProfilers)
            MyRenderProfiler.SortProfilersLocked();
        }
        return myProfiler;
      }
    }

    public static MyProfiler ThreadProfiler => MyRenderProfiler.m_threadProfiler ?? (MyRenderProfiler.m_threadProfiler = MyRenderProfiler.CreateProfiler((string) null));

    public static MyProfiler SelectedProfiler
    {
      get => MyRenderProfiler.m_selectedProfiler;
      set => MyRenderProfiler.m_selectedProfiler = value;
    }

    static MyRenderProfiler()
    {
      MyRenderProfiler.m_levelLimit = -1;
      MyRenderProfiler.FpsBlock = MyProfiler.CreateExternalBlock("FPS", -2);
      MyRenderProfiler.Paused = true;
    }

    public static MyProfiler CreateProfiler(
      string name,
      string axisName = null,
      bool allocationProfiling = false)
    {
      lock (MyRenderProfiler.ThreadProfilers)
      {
        MyProfiler myProfiler = new MyProfiler(allocationProfiling, name, axisName ?? "[ms]", MyRenderProfiler.ShallowProfileOnly, 1000, MyRenderProfiler.m_profilerProcessingEnabled ? MyRenderProfiler.m_levelLimit : 0);
        MyRenderProfiler.ThreadProfilers.Add(myProfiler);
        MyRenderProfiler.SortProfilersLocked();
        myProfiler.Paused = MyRenderProfiler.Paused;
        if (MyRenderProfiler.m_selectedProfiler == null)
          MyRenderProfiler.m_selectedProfiler = myProfiler;
        return myProfiler;
      }
    }

    public static List<MyProfilerBlock> GetSortedChildren(int frameToSortBy)
    {
      List<MyProfilerBlock> myProfilerBlockList = new List<MyProfilerBlock>((IEnumerable<MyProfilerBlock>) MyRenderProfiler.m_selectedProfiler.SelectedRootChildren);
      switch (MyRenderProfiler.m_sortingOrder)
      {
        case RenderProfilerSortingOrder.Id:
          myProfilerBlockList.Sort((Comparison<MyProfilerBlock>) ((a, b) => a.Id.CompareTo(b.Id)));
          break;
        case RenderProfilerSortingOrder.MillisecondsLastFrame:
          myProfilerBlockList.Sort((Comparison<MyProfilerBlock>) ((a, b) =>
          {
            int num = b.RawMilliseconds[frameToSortBy].CompareTo(a.RawMilliseconds[frameToSortBy]);
            return num != 0 ? num : a.Id.CompareTo(b.Id);
          }));
          break;
        case RenderProfilerSortingOrder.AllocatedLastFrame:
          myProfilerBlockList.Sort((Comparison<MyProfilerBlock>) ((a, b) =>
          {
            int num = b.RawAllocations[frameToSortBy].CompareTo(a.RawAllocations[frameToSortBy]);
            return num != 0 ? num : a.Id.CompareTo(b.Id);
          }));
          break;
        case RenderProfilerSortingOrder.MillisecondsAverage:
          myProfilerBlockList.Sort((Comparison<MyProfilerBlock>) ((a, b) =>
          {
            int num = b.AverageMilliseconds.CompareTo(a.AverageMilliseconds);
            return num != 0 ? num : a.Id.CompareTo(b.Id);
          }));
          break;
        case RenderProfilerSortingOrder.CallsCount:
          myProfilerBlockList.Sort((Comparison<MyProfilerBlock>) ((a, b) =>
          {
            int num = b.NumCallsArray[frameToSortBy].CompareTo(a.NumCallsArray[frameToSortBy]);
            return num != 0 ? num : a.Id.CompareTo(b.Id);
          }));
          break;
      }
      return myProfilerBlockList;
    }

    private static MyProfilerBlock FindBlockByIndex(int index)
    {
      List<MyProfilerBlock> sortedChildren = MyRenderProfiler.GetSortedChildren(MyRenderProfiler.m_selectedFrame);
      if (index >= 0 && index < sortedChildren.Count)
        return sortedChildren[index];
      return index == -1 && MyRenderProfiler.m_selectedProfiler.SelectedRoot != null ? MyRenderProfiler.m_selectedProfiler.SelectedRoot.Parent : (MyProfilerBlock) null;
    }

    protected static bool IsValidIndex(int frameIndex, int lastValidFrame) => (frameIndex > lastValidFrame ? frameIndex : frameIndex + MyProfiler.MAX_FRAMES) > lastValidFrame + MyProfiler.UPDATE_WINDOW;

    public static void HandleInput(RenderProfilerCommand command, int index, string value)
    {
      switch (command)
      {
        case RenderProfilerCommand.Enable:
          if (MyRenderProfiler.m_enabled)
            break;
          MyRenderProfiler.m_enabled = true;
          MyRenderProfiler.m_profilerProcessingEnabled = true;
          MyRenderProfiler.SetLevel();
          MyRenderProfiler.SelectThead(value);
          break;
        case RenderProfilerCommand.ToggleEnabled:
          if (MyRenderProfiler.m_enabled)
          {
            MyRenderProfiler.m_enabled = false;
            MyRenderProfiler.m_useCustomFrame = false;
            break;
          }
          MyRenderProfiler.m_enabled = true;
          MyRenderProfiler.m_profilerProcessingEnabled = true;
          MyRenderProfiler.SelectThead(value);
          break;
        case RenderProfilerCommand.JumpToLevel:
          if (index == 0 && !MyRenderProfiler.m_enabled)
          {
            MyRenderProfiler.m_enabled = true;
            MyRenderProfiler.m_profilerProcessingEnabled = true;
            break;
          }
          MyRenderProfiler.m_selectedProfiler.SelectedRoot = MyRenderProfiler.FindBlockByIndex(index - 1);
          MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
          break;
        case RenderProfilerCommand.JumpToRoot:
          MyRenderProfiler.m_selectedProfiler.SelectedRoot = (MyProfilerBlock) null;
          break;
        case RenderProfilerCommand.Pause:
          if (index == 0)
          {
            MyRenderProfiler.SwitchPause();
            break;
          }
          MyRenderProfiler.Pause(index > 0);
          break;
        case RenderProfilerCommand.NextFrame:
          MyRenderProfiler.NextFrame(index);
          break;
        case RenderProfilerCommand.PreviousFrame:
          MyRenderProfiler.PreviousFrame(index);
          break;
        case RenderProfilerCommand.DisableFrameSelection:
          MyRenderProfiler.m_useCustomFrame = false;
          break;
        case RenderProfilerCommand.NextThread:
          if (MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
          {
            long num = (long) ((double) MyRenderProfiler.m_taskRenderDispersion / 1.1);
            if (num <= 10L)
              break;
            MyRenderProfiler.m_taskRenderDispersion = num;
            break;
          }
          lock (MyRenderProfiler.ThreadProfilers)
          {
            int index1 = (MyRenderProfiler.ThreadProfilers.IndexOf(MyRenderProfiler.m_selectedProfiler) + 1) % MyRenderProfiler.ThreadProfilers.Count;
            MyRenderProfiler.m_selectedProfiler = MyRenderProfiler.ThreadProfilers[index1];
            MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
            break;
          }
        case RenderProfilerCommand.PreviousThread:
          if (MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
          {
            MyRenderProfiler.m_taskRenderDispersion = (long) ((double) MyRenderProfiler.m_taskRenderDispersion * 1.1);
            break;
          }
          lock (MyRenderProfiler.ThreadProfilers)
          {
            int index1 = (MyRenderProfiler.ThreadProfilers.IndexOf(MyRenderProfiler.m_selectedProfiler) - 1 + MyRenderProfiler.ThreadProfilers.Count) % MyRenderProfiler.ThreadProfilers.Count;
            MyRenderProfiler.m_selectedProfiler = MyRenderProfiler.ThreadProfilers[index1];
            MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
            break;
          }
        case RenderProfilerCommand.IncreaseLevel:
          ++MyRenderProfiler.m_levelLimit;
          MyRenderProfiler.SetLevel();
          break;
        case RenderProfilerCommand.DecreaseLevel:
          --MyRenderProfiler.m_levelLimit;
          if (MyRenderProfiler.m_levelLimit < -1)
            MyRenderProfiler.m_levelLimit = -1;
          MyRenderProfiler.SetLevel();
          break;
        case RenderProfilerCommand.IncreaseLocalArea:
          MyRenderProfiler.m_frameLocalArea = Math.Min(MyProfiler.MAX_FRAMES, MyRenderProfiler.m_frameLocalArea * 2);
          break;
        case RenderProfilerCommand.DecreaseLocalArea:
          MyRenderProfiler.m_frameLocalArea = Math.Max(2, MyRenderProfiler.m_frameLocalArea / 2);
          break;
        case RenderProfilerCommand.IncreaseRange:
          MyRenderProfiler.m_selectedProfiler.AutoScale = false;
          MyRenderProfiler.GetCurrentGraphScale().IncreaseYRange();
          break;
        case RenderProfilerCommand.DecreaseRange:
          MyRenderProfiler.m_selectedProfiler.AutoScale = false;
          MyRenderProfiler.GetCurrentGraphScale().DecreaseYRange();
          break;
        case RenderProfilerCommand.Reset:
          lock (MyRenderProfiler.ThreadProfilers)
          {
            foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
              threadProfiler.Reset();
            MyRenderProfiler.FrameTimestamps = new ConcurrentQueue<MyRenderProfiler.FrameInfo>();
            MyRenderProfiler.m_selectedFrame = 0;
            break;
          }
        case RenderProfilerCommand.SetLevel:
          MyRenderProfiler.m_levelLimit = index;
          if (MyRenderProfiler.m_levelLimit < -1)
            MyRenderProfiler.m_levelLimit = -1;
          MyRenderProfiler.SetLevel();
          break;
        case RenderProfilerCommand.ChangeSortingOrder:
          ++MyRenderProfiler.m_sortingOrder;
          if (MyRenderProfiler.m_sortingOrder < RenderProfilerSortingOrder.NumSortingTypes)
            break;
          MyRenderProfiler.m_sortingOrder = RenderProfilerSortingOrder.Id;
          break;
        case RenderProfilerCommand.CopyPathToClipboard:
          StringBuilder stringBuilder = new StringBuilder(200);
          for (MyProfilerBlock myProfilerBlock = MyRenderProfiler.m_selectedProfiler.SelectedRoot; myProfilerBlock != null; myProfilerBlock = myProfilerBlock.Parent)
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Insert(0, " > ");
            stringBuilder.Insert(0, myProfilerBlock.Name);
          }
          if (stringBuilder.Length <= 0)
            break;
          MyVRage.Platform.System.Clipboard = stringBuilder.ToString();
          break;
        case RenderProfilerCommand.TryGoToPathInClipboard:
          string fullPath = string.Empty;
          Thread thread = new Thread((ThreadStart) (() =>
          {
            try
            {
              fullPath = MyVRage.Platform.System.Clipboard;
            }
            catch
            {
            }
          }));
          thread.SetApartmentState(ApartmentState.STA);
          thread.Start();
          thread.Join();
          if (string.IsNullOrEmpty(fullPath))
            break;
          string[] strArray = fullPath.Split(new string[1]
          {
            " > "
          }, StringSplitOptions.None);
          MyProfilerBlock myProfilerBlock1 = (MyProfilerBlock) null;
          List<MyProfilerBlock> myProfilerBlockList = MyRenderProfiler.m_selectedProfiler.RootBlocks;
          for (int index1 = 0; index1 < strArray.Length; ++index1)
          {
            string str = strArray[index1];
            MyProfilerBlock myProfilerBlock2 = myProfilerBlock1;
            for (int index2 = 0; index2 < myProfilerBlockList.Count; ++index2)
            {
              MyProfilerBlock myProfilerBlock3 = myProfilerBlockList[index2];
              if (myProfilerBlock3.Name == str)
              {
                myProfilerBlock1 = myProfilerBlock3;
                myProfilerBlockList = myProfilerBlock1.Children;
                break;
              }
            }
            if (myProfilerBlock2 == myProfilerBlock1)
              break;
          }
          if (myProfilerBlock1 == null)
            break;
          MyRenderProfiler.m_selectedProfiler.SelectedRoot = myProfilerBlock1;
          break;
        case RenderProfilerCommand.GetFomServer:
          if (!MyRenderProfiler.m_enabled || MyRenderProfiler.GetProfilerFromServer == null)
            break;
          MyRenderProfiler.Pause();
          MyRenderProfiler.GetProfilerFromServer();
          break;
        case RenderProfilerCommand.GetFromClient:
          MyRenderProfiler.RestoreOnlineSnapshot();
          break;
        case RenderProfilerCommand.SaveToFile:
          MyRenderProfiler.SaveProfilerToFile(index);
          break;
        case RenderProfilerCommand.LoadFromFile:
          MyRenderProfiler.Pause();
          MyRenderProfiler.LoadProfilerFromFile(index, false);
          break;
        case RenderProfilerCommand.SwapBlockOptimized:
          if (index == 0)
          {
            using (List<MyProfilerBlock>.Enumerator enumerator = MyRenderProfiler.m_selectedProfiler.SelectedRootChildren.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                MyProfilerBlock current = enumerator.Current;
                current.IsOptimized = !current.IsOptimized;
              }
              break;
            }
          }
          else
          {
            MyProfilerBlock blockByIndex = MyRenderProfiler.FindBlockByIndex(index - 1);
            if (blockByIndex == null)
              break;
            blockByIndex.IsOptimized = !blockByIndex.IsOptimized;
            break;
          }
        case RenderProfilerCommand.ToggleOptimizationsEnabled:
          MyRenderProfiler.m_selectedProfiler.EnableOptimizations = !MyRenderProfiler.m_selectedProfiler.EnableOptimizations;
          break;
        case RenderProfilerCommand.ResetAllOptimizations:
          using (List<MyProfilerBlock>.Enumerator enumerator = MyRenderProfiler.m_selectedProfiler.RootBlocks.GetEnumerator())
          {
            while (enumerator.MoveNext())
              MyRenderProfiler.ResetOptimizationsRecursive(enumerator.Current);
            break;
          }
        case RenderProfilerCommand.SwitchBlockRender:
          ++MyRenderProfiler.m_blockRender;
          if (MyRenderProfiler.m_blockRender != BlockRender.BlockRenderMax)
            break;
          MyRenderProfiler.m_blockRender = BlockRender.Name;
          break;
        case RenderProfilerCommand.SwitchGraphContent:
          ++MyRenderProfiler.m_graphContent;
          if (MyRenderProfiler.m_graphContent >= ProfilerGraphContent.ProfilerGraphContentMax)
            MyRenderProfiler.m_graphContent = ProfilerGraphContent.Elapsed;
          switch (MyRenderProfiler.m_graphContent)
          {
            case ProfilerGraphContent.Elapsed:
              MyRenderProfiler.m_sortingOrder = RenderProfilerSortingOrder.MillisecondsLastFrame;
              break;
            case ProfilerGraphContent.Tasks:
              if (!MyRenderProfiler.FrameTimestamps.IsEmpty)
              {
                MyRenderProfiler.m_targetTaskRenderTime = MyRenderProfiler.m_selectedProfiler != null ? MyRenderProfiler.m_selectedProfiler.CommitTimes[MyRenderProfiler.m_selectedFrame] : MyRenderProfiler.FrameTimestamps.Last<MyRenderProfiler.FrameInfo>().Time - MyRenderProfiler.m_taskRenderDispersion;
                break;
              }
              break;
            case ProfilerGraphContent.Allocations:
              MyRenderProfiler.m_sortingOrder = RenderProfilerSortingOrder.AllocatedLastFrame;
              break;
          }
          MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
          break;
        case RenderProfilerCommand.SwitchShallowProfile:
          MyRenderProfiler.ShallowProfileOnly = !MyRenderProfiler.ShallowProfileOnly;
          lock (MyRenderProfiler.ThreadProfilers)
          {
            using (List<MyProfiler>.Enumerator enumerator = MyRenderProfiler.ThreadProfilers.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.SetShallowProfile(MyRenderProfiler.ShallowProfileOnly);
              break;
            }
          }
        case RenderProfilerCommand.ToggleAutoScale:
          MyRenderProfiler.m_selectedProfiler.AutoScale = !MyRenderProfiler.m_selectedProfiler.AutoScale;
          MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
          break;
        case RenderProfilerCommand.SwitchAverageTimes:
          MyRenderProfiler.AverageTimes = !MyRenderProfiler.AverageTimes;
          lock (MyRenderProfiler.ThreadProfilers)
          {
            using (List<MyProfiler>.Enumerator enumerator = MyRenderProfiler.ThreadProfilers.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.AverageTimes = MyRenderProfiler.AverageTimes;
              break;
            }
          }
        case RenderProfilerCommand.SubtractFromFile:
          MyRenderProfiler.Pause();
          MyRenderProfiler.LoadProfilerFromFile(index, true);
          break;
        case RenderProfilerCommand.EnableAutoScale:
          MyRenderProfiler.m_selectedProfiler.AutoScale = true;
          break;
        case RenderProfilerCommand.EnableShallowProfile:
          MyRenderProfiler.ShallowProfileOnly = index > 0;
          lock (MyRenderProfiler.ThreadProfilers)
          {
            using (List<MyProfiler>.Enumerator enumerator = MyRenderProfiler.ThreadProfilers.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.SetShallowProfile(MyRenderProfiler.ShallowProfileOnly);
              break;
            }
          }
      }
    }

    private static void SelectThead(string threadName)
    {
      if (threadName == null)
        return;
      lock (MyRenderProfiler.ThreadProfilers)
      {
        foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
        {
          if (threadProfiler.DisplayName == threadName)
          {
            MyRenderProfiler.m_selectedProfiler = threadProfiler;
            MyRenderProfiler.m_graphContent = ProfilerGraphContent.Elapsed;
            MyRenderProfiler.m_nextAutoScale = MyTimeSpan.Zero;
          }
        }
      }
    }

    private static void Pause(bool state = true)
    {
      lock (MyRenderProfiler.m_pauseLock)
      {
        MyRenderProfiler.m_pauseCount = state ? 1 : 0;
        MyRenderProfiler.ApplyPause(state);
      }
    }

    public static void SwitchPause()
    {
      lock (MyRenderProfiler.m_pauseLock)
      {
        MyRenderProfiler.m_pauseCount = MyRenderProfiler.Paused ? 0 : 1;
        MyRenderProfiler.ApplyPause(!MyRenderProfiler.Paused);
      }
    }

    public static void AddPause(bool pause)
    {
      lock (MyRenderProfiler.m_pauseLock)
      {
        MyRenderProfiler.m_pauseCount += pause ? 1 : -1;
        MyRenderProfiler.ApplyPause(MyRenderProfiler.m_pauseCount > 0);
      }
    }

    private static void ApplyPause(bool paused)
    {
      if (!paused && MyRenderProfiler.m_dataType != SnapshotType.Online)
        MyRenderProfiler.RestoreOnlineSnapshot();
      if (paused && MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
        MyRenderProfiler.m_targetTaskRenderTime = Stopwatch.GetTimestamp() - MyRenderProfiler.m_taskRenderDispersion;
      Thread.MemoryBarrier();
      MyRenderProfiler.Paused = paused;
      foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
        threadProfiler.Paused = paused;
    }

    private static void ResetOptimizationsRecursive(MyProfilerBlock block)
    {
      foreach (MyProfilerBlock child in block.Children)
        MyRenderProfiler.ResetOptimizationsRecursive(child);
      block.IsOptimized = false;
    }

    private static void SetLevel()
    {
      lock (MyRenderProfiler.ThreadProfilers)
      {
        foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
          threadProfiler.SetNewLevelLimit(MyRenderProfiler.m_profilerProcessingEnabled ? MyRenderProfiler.m_levelLimit : 0);
      }
    }

    private static void PreviousFrame(int step)
    {
      if (MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
      {
        MyRenderProfiler.m_targetTaskRenderTime -= (long) ((double) (MyRenderProfiler.m_taskRenderDispersion * (long) step) * 0.0500000007450581);
      }
      else
      {
        MyRenderProfiler.m_useCustomFrame = true;
        MyRenderProfiler.m_selectedFrame -= step;
        while (MyRenderProfiler.m_selectedFrame < 0)
          MyRenderProfiler.m_selectedFrame += MyProfiler.MAX_FRAMES - 1;
      }
    }

    private static void NextFrame(int step)
    {
      if (MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
      {
        MyRenderProfiler.m_targetTaskRenderTime += (long) ((double) (MyRenderProfiler.m_taskRenderDispersion * (long) step) * 0.0500000007450581);
      }
      else
      {
        MyRenderProfiler.m_useCustomFrame = true;
        MyRenderProfiler.m_selectedFrame += step;
        while (MyRenderProfiler.m_selectedFrame >= MyProfiler.MAX_FRAMES)
          MyRenderProfiler.m_selectedFrame -= MyProfiler.MAX_FRAMES;
      }
    }

    private static void FindMax(
      MyProfilerBlock.DataReader data,
      int start,
      int end,
      ref float max,
      ref int maxIndex)
    {
      for (int frame = start; frame <= end; ++frame)
      {
        float num = data[frame];
        if ((double) num > (double) max)
        {
          max = num;
          maxIndex = frame;
        }
      }
    }

    private static void FindMax(
      MyProfilerBlock.DataReader data,
      int lower,
      int upper,
      int lastValidFrame,
      ref float max,
      ref int maxIndex)
    {
      int val2 = (lastValidFrame + 1 + MyProfiler.UPDATE_WINDOW) % MyProfiler.MAX_FRAMES;
      if (lastValidFrame > val2)
      {
        MyRenderProfiler.FindMax(data, Math.Max(lower, val2), Math.Min(lastValidFrame, upper), ref max, ref maxIndex);
      }
      else
      {
        MyRenderProfiler.FindMax(data, lower, Math.Min(lastValidFrame, upper), ref max, ref maxIndex);
        MyRenderProfiler.FindMax(data, Math.Max(lower, val2), upper, ref max, ref maxIndex);
      }
    }

    protected static float FindMaxWrap(
      MyProfilerBlock.DataReader data,
      int lower,
      int upper,
      int lastValidFrame,
      out int maxIndex)
    {
      lower = (lower + MyProfiler.MAX_FRAMES) % MyProfiler.MAX_FRAMES;
      upper = (upper + MyProfiler.MAX_FRAMES) % MyProfiler.MAX_FRAMES;
      float max = 0.0f;
      maxIndex = -1;
      if (upper > lower)
      {
        MyRenderProfiler.FindMax(data, lower, upper, lastValidFrame, ref max, ref maxIndex);
      }
      else
      {
        MyRenderProfiler.FindMax(data, 0, upper, lastValidFrame, ref max, ref maxIndex);
        MyRenderProfiler.FindMax(data, lower, MyProfiler.MAX_FRAMES - 1, lastValidFrame, ref max, ref maxIndex);
      }
      return max;
    }

    public static bool GetAutocommit() => MyRenderProfiler.ThreadProfiler.AutoCommit;

    public static void SetAutocommit(bool val) => MyRenderProfiler.ThreadProfiler.AutoCommit = val;

    public static void Commit(bool? simulation, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
    {
      long timestamp = Stopwatch.GetTimestamp();
      MyProfiler threadProfiler = MyRenderProfiler.ThreadProfiler;
      if (simulation.HasValue)
        MyRenderProfiler.CommitProfilers(simulation.Value);
      if (!MyRenderProfiler.Paused)
      {
        threadProfiler.CommitFrame();
        MyRenderProfiler.m_useCustomFrame = true;
      }
      else
        threadProfiler.ClearFrame();
      MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
    }

    public void Draw([CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
    {
      if (!MyRenderProfiler.m_enabled)
        return;
      MyProfiler selectedProfiler = MyRenderProfiler.m_selectedProfiler;
      if (selectedProfiler == null)
        return;
      int lastValidFrame;
      using (selectedProfiler.LockHistory(out lastValidFrame))
      {
        int frameToDraw = MyRenderProfiler.m_useCustomFrame ? MyRenderProfiler.m_selectedFrame : lastValidFrame;
        this.Draw(selectedProfiler, lastValidFrame, frameToDraw);
      }
    }

    protected abstract void Draw(MyProfiler drawProfiler, int lastFrameIndex, int frameToDraw);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StartProfilingBlock(
      string blockName = null,
      bool isDeepTreeRoot = false,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      MyRenderProfiler.ThreadProfiler.StartBlock(blockName, member, line, file, isDeepTreeRoot: isDeepTreeRoot);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EndProfilingBlock(
      float customValue = 0.0f,
      MyTimeSpan? customTime = null,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      MyRenderProfiler.ThreadProfiler.EndBlock(member, line, file, customTime, customValue, timeFormat, valueFormat);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void GPU_StartProfilingBlock(
      string blockName = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      MyRenderProfiler.GpuProfiler.StartBlock(blockName, member, line, file);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void GPU_EndProfilingBlock(
      float customValue = 0.0f,
      MyTimeSpan? customTime = null,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      MyRenderProfiler.GpuProfiler.EndBlock(member, line, file, customTime, customValue, timeFormat, valueFormat);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StartNextBlock(
      string name,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "",
      float previousBlockCustomValue = 0.0f)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void ProfileCustomValue(
      string name,
      float value,
      MyTimeSpan? customTime = null,
      string timeFormat = null,
      string valueFormat = null,
      [CallerMemberName] string member = "",
      [CallerLineNumber] int line = 0,
      [CallerFilePath] string file = "")
    {
      int levelLimit = MyRenderProfiler.m_levelLimit;
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskStarted(
      MyProfiler.TaskType taskType,
      string debugName,
      long scheduledTimestamp)
    {
      MyRenderProfiler.ThreadProfiler.OnTaskStarted(taskType, debugName, scheduledTimestamp);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskFinished(MyProfiler.TaskType? taskType, float customValue) => MyRenderProfiler.ThreadProfiler.OnTaskFinished(taskType, customValue);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void CommitTask(MyProfiler.TaskInfo task) => MyRenderProfiler.ThreadProfiler.CommitTask(task);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void InitThraedInfo(int viewPriority, bool simulation)
    {
      MyRenderProfiler.ThreadProfiler.ViewPriority = viewPriority;
      MyRenderProfiler.ThreadProfiler.SimulationProfiler = simulation;
      lock (MyRenderProfiler.ThreadProfilers)
        MyRenderProfiler.SortProfilersLocked();
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnBeginSimulationFrame(long frameNumber)
    {
      if (MyRenderProfiler.Paused)
        return;
      long timestamp = Stopwatch.GetTimestamp();
      MyRenderProfiler.FrameTimestamps.Enqueue(new MyRenderProfiler.FrameInfo()
      {
        Time = timestamp,
        FrameNumber = frameNumber
      });
      if (MyRenderProfiler.FrameTimestamps.Count > MyProfiler.MAX_FRAMES)
        MyRenderProfiler.FrameTimestamps.TryDequeue(out MyRenderProfiler.FrameInfo _);
      MyRenderProfiler.FrameInfo result;
      MyRenderProfiler.FrameTimestamps.TryPeek(out result);
      MyProfiler.LastFrameTime = timestamp;
      MyProfiler.LastInterestingFrameTime = result.Time;
    }

    internal static void DestroyThread()
    {
      lock (MyRenderProfiler.ThreadProfilers)
      {
        MyRenderProfiler.ThreadProfilers.Remove(MyRenderProfiler.m_threadProfiler);
        if (MyRenderProfiler.m_selectedProfiler == MyRenderProfiler.m_threadProfiler)
          MyRenderProfiler.m_selectedProfiler = MyRenderProfiler.ThreadProfilers.Count > 0 ? MyRenderProfiler.ThreadProfilers[0] : (MyProfiler) null;
        MyRenderProfiler.m_threadProfiler = (MyProfiler) null;
      }
    }

    public static void SetLevel(int index)
    {
      MyRenderProfiler.m_levelLimit = index;
      if (MyRenderProfiler.m_levelLimit < -1)
        MyRenderProfiler.m_levelLimit = -1;
      MyRenderProfiler.SetLevel();
    }

    public void Dump()
    {
      try
      {
        string path = (string) null;
        for (; this.m_currentDumpNumber < 100; ++this.m_currentDumpNumber)
        {
          path = MyFileSystem.UserDataPath + string.Format("\\dump{0}.xml", (object) this.m_currentDumpNumber);
          if (!MyFileSystem.FileExists(path))
            break;
        }
        if (path == null)
          return;
        Stream stream = MyFileSystem.OpenWrite(path);
        if (stream == null)
          return;
        StreamWriter streamWriter = new StreamWriter(stream);
        streamWriter.Write((object) MyRenderProfiler.ThreadProfiler.Dump());
        streamWriter.Close();
        stream.Close();
      }
      catch
      {
      }
    }

    protected static MyDrawArea GetCurrentGraphScale()
    {
      switch (MyRenderProfiler.m_graphContent)
      {
        case ProfilerGraphContent.Elapsed:
        case ProfilerGraphContent.Tasks:
          return MyRenderProfiler.m_milisecondsGraphScale;
        case ProfilerGraphContent.Allocations:
          return MyRenderProfiler.m_allocationsGraphScale;
        default:
          throw new Exception("Unhandled enum value" + (object) MyRenderProfiler.m_graphContent);
      }
    }

    protected static MyProfilerBlock.DataReader GetGraphData(MyProfilerBlock block)
    {
      bool enableOptimizations = MyRenderProfiler.m_selectedProfiler.EnableOptimizations;
      switch (MyRenderProfiler.m_graphContent)
      {
        case ProfilerGraphContent.Elapsed:
          return block.GetMillisecondsReader(enableOptimizations);
        case ProfilerGraphContent.Allocations:
          return block.GetAllocationsReader(enableOptimizations);
        default:
          throw new Exception("Unhandled enum value" + (object) MyRenderProfiler.m_graphContent);
      }
    }

    protected static int GetWindowEnd(int lastFrameIndex) => (lastFrameIndex + 1 + MyProfiler.UPDATE_WINDOW) % MyProfiler.MAX_FRAMES;

    private static void UpdateStatsSeparated(
      ref MyRenderProfiler.MyStats stats,
      MyProfilerBlock.DataReader data,
      int lastFrameIndex,
      int windowEnd)
    {
      if (lastFrameIndex > windowEnd)
      {
        MyRenderProfiler.UpdateStats(ref stats, data, windowEnd, lastFrameIndex);
      }
      else
      {
        MyRenderProfiler.UpdateStats(ref stats, data, 0, lastFrameIndex);
        MyRenderProfiler.UpdateStats(ref stats, data, windowEnd, MyProfiler.MAX_FRAMES - 1);
      }
    }

    private static void UpdateStats(
      ref MyRenderProfiler.MyStats stats,
      MyProfilerBlock.DataReader data,
      int start,
      int end)
    {
      for (int frame = start; frame <= end; ++frame)
      {
        float num = data[frame];
        if ((double) num > 0.00999999977648258)
        {
          if ((double) num > (double) stats.Min)
          {
            ++stats.MinCount;
            if ((double) num > (double) stats.Max)
              ++stats.MaxCount;
          }
          stats.Any = true;
        }
      }
    }

    protected static void UpdateAutoScale(int lastFrameIndex)
    {
      MyTimeSpan myTimeSpan = new MyTimeSpan(Stopwatch.GetTimestamp());
      if (!MyRenderProfiler.m_selectedProfiler.AutoScale || !(myTimeSpan > MyRenderProfiler.m_nextAutoScale))
        return;
      MyDrawArea currentGraphScale = MyRenderProfiler.GetCurrentGraphScale();
      MyRenderProfiler.MyStats stats = new MyRenderProfiler.MyStats()
      {
        Min = currentGraphScale.GetYRange(currentGraphScale.Index - 1),
        Max = currentGraphScale.YRange
      };
      int windowEnd = MyRenderProfiler.GetWindowEnd(lastFrameIndex);
      List<MyProfilerBlock> selectedRootChildren = MyRenderProfiler.m_selectedProfiler.SelectedRootChildren;
      if (MyRenderProfiler.m_selectedProfiler.SelectedRoot != null && (!MyRenderProfiler.m_selectedProfiler.IgnoreRoot || selectedRootChildren.Count == 0))
      {
        MyProfilerBlock.DataReader graphData = MyRenderProfiler.GetGraphData(MyRenderProfiler.m_selectedProfiler.SelectedRoot);
        MyRenderProfiler.UpdateStatsSeparated(ref stats, graphData, lastFrameIndex, windowEnd);
      }
      foreach (MyProfilerBlock block in selectedRootChildren)
      {
        MyProfilerBlock.DataReader graphData = MyRenderProfiler.GetGraphData(block);
        MyRenderProfiler.UpdateStatsSeparated(ref stats, graphData, lastFrameIndex, windowEnd);
      }
      if (stats.MaxCount > 0)
      {
        if (stats.MaxCount > 10)
        {
          currentGraphScale.IncreaseYRange();
          MyRenderProfiler.UpdateAutoScale(lastFrameIndex);
        }
      }
      else if (stats.MinCount < 10 && stats.Any)
      {
        currentGraphScale.DecreaseYRange();
        MyRenderProfiler.UpdateAutoScale(lastFrameIndex);
      }
      MyRenderProfiler.m_nextAutoScale = myTimeSpan + MyRenderProfiler.AUTO_SCALE_UPDATE;
    }

    public static void PushOnlineSnapshot(
      SnapshotType type,
      List<MyProfiler> threadProfilers,
      ConcurrentQueue<MyRenderProfiler.FrameInfo> frameTimestamps)
    {
      MyRenderProfiler.m_dataType = type;
      if (!MyRenderProfiler.FrameTimestamps.IsEmpty)
      {
        MyProfiler.LastFrameTime = MyRenderProfiler.FrameTimestamps.Last<MyRenderProfiler.FrameInfo>().Time;
        MyProfiler.LastInterestingFrameTime = MyRenderProfiler.FrameTimestamps.First<MyRenderProfiler.FrameInfo>().Time;
        MyRenderProfiler.m_targetTaskRenderTime = MyProfiler.LastFrameTime - MyRenderProfiler.m_taskRenderDispersion;
      }
      Volatile.Write<List<MyProfiler>>(ref MyRenderProfiler.ThreadProfilers, threadProfilers);
      MyRenderProfiler.FrameTimestamps = frameTimestamps;
    }

    public static void SubtractOnlineSnapshot(
      SnapshotType type,
      List<MyProfiler> threadProfilers,
      ConcurrentQueue<MyRenderProfiler.FrameInfo> frameTimestamps)
    {
      MyRenderProfiler.m_dataType = type;
      if (!MyRenderProfiler.FrameTimestamps.IsEmpty)
      {
        MyProfiler.LastFrameTime = MyRenderProfiler.FrameTimestamps.Last<MyRenderProfiler.FrameInfo>().Time;
        MyProfiler.LastInterestingFrameTime = MyRenderProfiler.FrameTimestamps.First<MyRenderProfiler.FrameInfo>().Time;
        MyRenderProfiler.m_targetTaskRenderTime = MyProfiler.LastFrameTime - MyRenderProfiler.m_taskRenderDispersion;
      }
      foreach (MyProfiler threadProfiler1 in threadProfilers)
      {
        if (!string.IsNullOrEmpty(threadProfiler1.DisplayName))
        {
          foreach (MyProfiler threadProfiler2 in MyRenderProfiler.ThreadProfilers)
          {
            if (threadProfiler2.DisplayName == threadProfiler1.DisplayName)
              threadProfiler1.SubtractFrom(threadProfiler2);
          }
        }
      }
      Volatile.Write<List<MyProfiler>>(ref MyRenderProfiler.ThreadProfilers, threadProfilers);
      MyRenderProfiler.FrameTimestamps = frameTimestamps;
    }

    private static void RestoreOnlineSnapshot()
    {
      MyRenderProfiler.m_dataType = SnapshotType.Online;
      MyRenderProfiler.ThreadProfilers = MyRenderProfiler.m_onlineThreadProfilers;
      MyRenderProfiler.FrameTimestamps = MyRenderProfiler.m_onlineFrameTimestamps;
      lock (MyRenderProfiler.ThreadProfilers)
      {
        MyRenderProfiler.SelectedProfiler = MyRenderProfiler.ThreadProfilers[0];
        long time = MyRenderProfiler.FrameTimestamps.LastOrDefault<MyRenderProfiler.FrameInfo>().Time;
        if (time <= 0L)
          return;
        MyProfiler.LastFrameTime = time;
        MyProfiler.LastInterestingFrameTime = time;
      }
    }

    private static void CommitProfilers(bool simulation)
    {
      lock (MyRenderProfiler.ThreadProfilers)
      {
        foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
        {
          if (threadProfiler.SimulationProfiler == simulation)
            Volatile.Write(ref threadProfiler.AllowAutocommit, 1);
        }
      }
    }

    private static void SortProfilersLocked() => MyRenderProfiler.ThreadProfilers.SortNoAlloc<MyProfiler>((Comparison<MyProfiler>) ((x, y) => x.ViewPriority.CompareTo(y.ViewPriority)));

    public struct FrameInfo
    {
      public long Time;
      public long FrameNumber;
    }

    private struct MyStats
    {
      public float Min;
      public float Max;
      public int MinCount;
      public int MaxCount;
      public bool Any;
    }
  }
}
