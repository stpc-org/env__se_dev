// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyProfiler
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Utils;

namespace VRage.Profiler
{
  [DebuggerDisplay("{DisplayName} Blocks({m_profilingBlocks.Count}) Tasks({FinishedTasks.Count})")]
  public class MyProfiler
  {
    public static long LastInterestingFrameTime;
    public static long LastFrameTime;
    private static readonly bool m_enableAsserts = true;
    public static readonly int MAX_FRAMES = 1024;
    public static readonly int UPDATE_WINDOW = 16;
    private const int ROOT_ID = 0;
    private int m_nextId = 1;
    private Dictionary<MyProfilerBlockKey, MyProfilerBlock> m_profilingBlocks = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>(8192, (IEqualityComparer<MyProfilerBlockKey>) new MyProfilerBlockKeyComparer());
    private List<MyProfilerBlock> m_rootBlocks = new List<MyProfilerBlock>(32);
    private readonly Stack<MyProfilerBlock> m_currentProfilingStack = new Stack<MyProfilerBlock>(1024);
    private int m_levelLimit = -1;
    private int m_levelSkipCount;
    private volatile int m_newLevelLimit = -1;
    private int m_remainingWindow = MyProfiler.UPDATE_WINDOW;
    private readonly FastResourceLock m_historyLock = new FastResourceLock();
    public readonly object TaskLock = new object();
    private string m_customName;
    private string m_axisName;
    private readonly Dictionary<MyProfilerBlockKey, MyProfilerBlock> m_blocksToAdd = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>(8192, (IEqualityComparer<MyProfilerBlockKey>) new MyProfilerBlockKeyComparer());
    private volatile int m_lastFrameIndex;
    public int[] TotalCalls = new int[MyProfiler.MAX_FRAMES];
    public long[] CommitTimes = new long[MyProfiler.MAX_FRAMES];
    public bool AutoCommit = true;
    public bool AutoScale;
    public bool IgnoreRoot;
    public bool AverageTimes;
    private bool AssertCommitFromOwningThread = true;
    public int ViewPriority;
    public bool SimulationProfiler;
    public int AllowAutocommit;
    public bool EnableOptimizations = true;
    private int m_shallowMarker;
    public bool ShallowProfileEnabled;
    public volatile bool PendingShallowProfileState;
    public readonly bool AllocationProfiling;
    private readonly Thread m_ownerThread;
    public bool Paused;
    private readonly Stack<MyProfiler.TaskInfo> m_runningTasks = new Stack<MyProfiler.TaskInfo>(5);
    private readonly List<MyProfiler.TaskInfo> m_pendingTasks = new List<MyProfiler.TaskInfo>();
    public MyQueue<MyProfiler.TaskInfo> FinishedTasks = new MyQueue<MyProfiler.TaskInfo>(100);

    public MyProfilerBlock SelectedRoot { get; set; }

    public List<MyProfilerBlock> SelectedRootChildren => this.SelectedRoot == null ? this.m_rootBlocks : this.SelectedRoot.Children;

    public List<MyProfilerBlock> RootBlocks => this.m_rootBlocks;

    public string DisplayName => this.m_customName;

    public string AxisName => this.m_axisName;

    public int LevelLimit => this.m_levelLimit;

    public Thread OwnerThread => this.m_ownerThread;

    private int GetParentId() => this.m_currentProfilingStack.Count > 0 ? this.m_currentProfilingStack.Peek().Id : 0;

    public MyProfiler(
      bool allocationProfiling,
      string name,
      string axisName,
      bool shallowProfile,
      int viewPriority,
      int levelLimit)
    {
      this.m_ownerThread = Thread.CurrentThread;
      this.m_customName = name ?? this.m_ownerThread.Name;
      this.m_newLevelLimit = levelLimit;
      this.m_axisName = axisName;
      this.AllocationProfiling = allocationProfiling;
      this.PendingShallowProfileState = this.ShallowProfileEnabled = shallowProfile;
      this.m_lastFrameIndex = MyProfiler.MAX_FRAMES - 1;
      this.ViewPriority = viewPriority;
    }

    private void OnHistorySafe() => Interlocked.Exchange(ref this.m_remainingWindow, MyProfiler.UPDATE_WINDOW);

    public static MyProfilerBlock CreateExternalBlock(string name, int blockId)
    {
      MyProfilerBlockKey key = new MyProfilerBlockKey(string.Empty, string.Empty, name, 0, 0);
      MyProfilerBlock myProfilerBlock = new MyProfilerBlock();
      myProfilerBlock.SetBlockData(ref key, blockId);
      return myProfilerBlock;
    }

    public void SetNewLevelLimit(int newLevelLimit) => this.m_newLevelLimit = newLevelLimit;

    public MyProfiler.HistoryLock LockHistory(out int lastValidFrame)
    {
      MyProfiler.HistoryLock historyLock = new MyProfiler.HistoryLock(this, this.m_historyLock);
      lastValidFrame = this.m_lastFrameIndex;
      return historyLock;
    }

    public void CommitFrame() => this.CommitInternal();

    private void CommitInternal()
    {
      int num1 = this.AssertCommitFromOwningThread ? 1 : 0;
      if (this.m_currentProfilingStack.Count != 0)
      {
        int num2 = this.ShallowProfileEnabled ? 1 : 0;
      }
      this.m_shallowMarker = 0;
      this.m_currentProfilingStack.Clear();
      if (this.m_blocksToAdd.Count > 0)
      {
        using (this.m_historyLock.AcquireExclusiveUsing())
        {
          foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> keyValuePair in this.m_blocksToAdd)
          {
            if (keyValuePair.Value.Parent != null)
              keyValuePair.Value.Parent.Children.AddOrInsert<MyProfilerBlock>(keyValuePair.Value, keyValuePair.Value.ForceOrder);
            else
              this.m_rootBlocks.AddOrInsert<MyProfilerBlock>(keyValuePair.Value, keyValuePair.Value.ForceOrder);
            this.m_profilingBlocks.Add(keyValuePair.Key, keyValuePair.Value);
          }
          this.m_blocksToAdd.Clear();
          Interlocked.Exchange(ref this.m_remainingWindow, MyProfiler.UPDATE_WINDOW - 1);
        }
      }
      else if (this.m_historyLock.TryAcquireExclusive())
      {
        Interlocked.Exchange(ref this.m_remainingWindow, MyProfiler.UPDATE_WINDOW - 1);
        this.m_historyLock.ReleaseExclusive();
      }
      else if (Interlocked.Decrement(ref this.m_remainingWindow) < 0)
      {
        using (this.m_historyLock.AcquireExclusiveUsing())
          Interlocked.Exchange(ref this.m_remainingWindow, MyProfiler.UPDATE_WINDOW - 1);
      }
      int num3 = 0;
      this.m_levelLimit = this.m_newLevelLimit;
      int frame = (this.m_lastFrameIndex + 1) % MyProfiler.MAX_FRAMES;
      foreach (MyProfilerBlock myProfilerBlock in this.m_profilingBlocks.Values)
      {
        num3 += myProfilerBlock.NumCalls;
        myProfilerBlock.NumCallsArray[frame] = myProfilerBlock.NumCalls;
        myProfilerBlock.CustomValues[frame] = myProfilerBlock.CustomValue;
        myProfilerBlock.RawAllocations[frame] = (float) myProfilerBlock.Allocated;
        myProfilerBlock.AverageMilliseconds = (float) (0.899999976158142 * (double) myProfilerBlock.AverageMilliseconds + 0.100000001490116 * myProfilerBlock.Elapsed.Milliseconds);
        myProfilerBlock.RawMilliseconds[frame] = this.AverageTimes ? myProfilerBlock.AverageMilliseconds : (float) myProfilerBlock.Elapsed.Milliseconds;
        myProfilerBlock.Clear();
      }
      bool flag = this.m_pendingTasks.Count > 0;
      if (flag)
        this.m_pendingTasks.SortNoAlloc<MyProfiler.TaskInfo>((Comparison<MyProfiler.TaskInfo>) ((x, y) => x.Started.CompareTo(y.Started)));
      lock (this.TaskLock)
      {
        if (flag)
        {
          foreach (MyProfiler.TaskInfo pendingTask in this.m_pendingTasks)
          {
            if (this.FinishedTasks.Count >= 10000000)
              this.FinishedTasks.Dequeue();
            this.FinishedTasks.Enqueue(pendingTask);
          }
          this.m_pendingTasks.Clear();
        }
        long interestingFrameTime = MyProfiler.LastInterestingFrameTime;
        while (this.FinishedTasks.Count > 0)
        {
          if (this.FinishedTasks.Peek().Finished < interestingFrameTime)
            this.FinishedTasks.Dequeue();
          else
            break;
        }
      }
      this.m_lastFrameIndex = frame;
      this.TotalCalls[frame] = num3;
      this.CommitTimes[frame] = Stopwatch.GetTimestamp();
      this.ShallowProfileEnabled = this.PendingShallowProfileState;
    }

    public void ClearFrame()
    {
      int num = this.AssertCommitFromOwningThread ? 1 : 0;
      this.m_currentProfilingStack.Clear();
      if (this.m_blocksToAdd.Count > 0)
        this.m_blocksToAdd.Clear();
      this.m_levelLimit = this.m_newLevelLimit;
      foreach (MyProfilerBlock myProfilerBlock in this.m_profilingBlocks.Values)
        myProfilerBlock.Clear();
      this.m_pendingTasks.Clear();
    }

    public void Reset()
    {
      using (new MyProfiler.HistoryLock(this, this.m_historyLock))
      {
        foreach (MyProfilerBlock myProfilerBlock in this.m_profilingBlocks.Values)
        {
          myProfilerBlock.AverageMilliseconds = 0.0f;
          for (int frame = 0; frame < MyProfiler.MAX_FRAMES; ++frame)
          {
            myProfilerBlock.CustomValues[frame] = 0.0f;
            myProfilerBlock.NumCallsArray[frame] = 0;
            myProfilerBlock.RawAllocations[frame] = 0.0f;
            myProfilerBlock.RawMilliseconds[frame] = 0.0f;
          }
        }
        this.m_lastFrameIndex = MyProfiler.MAX_FRAMES - 1;
      }
      lock (this.TaskLock)
        this.FinishedTasks.Clear();
    }

    public void StartBlock(
      string name,
      string memberName,
      int line,
      string file,
      int forceOrder = 2147483647,
      bool isDeepTreeRoot = false)
    {
      if (this.m_levelLimit != -1 && this.m_currentProfilingStack.Count >= this.m_levelLimit || this.m_shallowMarker > 0 && this.ShallowProfileEnabled)
      {
        ++this.m_levelSkipCount;
      }
      else
      {
        if (isDeepTreeRoot)
          ++this.m_shallowMarker;
        MyProfilerBlock myProfilerBlock = (MyProfilerBlock) null;
        MyProfilerBlockKey key = new MyProfilerBlockKey(file, memberName, name, line, this.GetParentId());
        if (!this.m_profilingBlocks.TryGetValue(key, out myProfilerBlock) && !this.m_blocksToAdd.TryGetValue(key, out myProfilerBlock))
        {
          myProfilerBlock = new MyProfilerBlock();
          myProfilerBlock.SetBlockData(ref key, this.m_nextId++, forceOrder, isDeepTreeRoot);
          if (this.m_currentProfilingStack.Count > 0)
            myProfilerBlock.Parent = this.m_currentProfilingStack.Peek();
          this.m_blocksToAdd.Add(key, myProfilerBlock);
        }
        myProfilerBlock.Start(this.AllocationProfiling);
        this.m_currentProfilingStack.Push(myProfilerBlock);
      }
    }

    [Conditional("DEBUG")]
    private static void CheckEndBlock(
      MyProfilerBlock profilingBlock,
      string member,
      string file,
      int parentId)
    {
      if ((!MyProfiler.m_enableAsserts || profilingBlock.Key.Member.Equals(member)) && (profilingBlock.Key.ParentId == parentId && !(profilingBlock.Key.File != file)))
        return;
      StackTrace stackTrace = new StackTrace(2, true);
      for (int index = 0; index < stackTrace.FrameCount; ++index)
      {
        StackFrame frame = stackTrace.GetFrame(index);
        if (frame.GetFileName() == profilingBlock.Key.File && frame.GetMethod().Name == member)
          break;
      }
    }

    public void EndBlock(
      string member,
      int line,
      string file,
      MyTimeSpan? customTime = null,
      float customValue = 0.0f,
      string timeFormat = null,
      string valueFormat = null,
      string callFormat = null,
      int numCalls = 0)
    {
      if (this.m_levelSkipCount > 0)
      {
        --this.m_levelSkipCount;
      }
      else
      {
        if (this.m_currentProfilingStack.Count > 0)
        {
          MyProfilerBlock myProfilerBlock = this.m_currentProfilingStack.Pop();
          myProfilerBlock.CustomValue = Math.Max(myProfilerBlock.CustomValue, customValue);
          myProfilerBlock.TimeFormat = timeFormat;
          myProfilerBlock.ValueFormat = valueFormat;
          myProfilerBlock.CallFormat = callFormat;
          myProfilerBlock.End(this.AllocationProfiling, customTime, numCalls);
          if (myProfilerBlock.IsDeepTreeRoot)
            --this.m_shallowMarker;
        }
        this.CommitWorklogIfNeeded();
      }
    }

    private void CommitWorklogIfNeeded()
    {
      if (this.m_currentProfilingStack.Count > 0)
        return;
      if (!this.AutoCommit)
      {
        this.m_levelLimit = this.m_newLevelLimit;
      }
      else
      {
        if (Interlocked.Exchange(ref this.AllowAutocommit, 0) != 1)
          return;
        long timestamp = Stopwatch.GetTimestamp();
        if (this.Paused)
          this.ClearFrame();
        else
          this.CommitInternal();
        this.m_pendingTasks.Add(new MyProfiler.TaskInfo()
        {
          CustomValue = 0.0f,
          Finished = Stopwatch.GetTimestamp(),
          Name = "CommitProfiler",
          Scheduled = 0L,
          TaskType = MyProfiler.TaskType.Profiler,
          Started = timestamp
        });
      }
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public void ProfileCustomValue(
      string name,
      string member,
      int line,
      string file,
      float value,
      MyTimeSpan? customTime,
      string timeFormat,
      string valueFormat,
      string callFormat = null)
    {
      this.StartBlock(name, member, line, file);
      this.EndBlock(member, line, file, customTime, value, timeFormat, valueFormat, callFormat);
    }

    public void OnTaskStarted(MyProfiler.TaskType taskType, string name, long scheduledTimestamp)
    {
      long timestamp = Stopwatch.GetTimestamp();
      this.m_runningTasks.Push(new MyProfiler.TaskInfo()
      {
        Name = name,
        TaskType = taskType,
        Started = timestamp,
        Scheduled = scheduledTimestamp
      });
    }

    public void OnTaskFinished(MyProfiler.TaskType? taskType, float customValue)
    {
      if (this.m_runningTasks.Count == 0)
        return;
      MyProfiler.TaskInfo task = this.m_runningTasks.Pop();
      task.Finished = Stopwatch.GetTimestamp();
      task.CustomValue = customValue;
      if (taskType.HasValue)
        task.TaskType = taskType.Value;
      this.CommitTask(task);
    }

    public void CommitTask(MyProfiler.TaskInfo task)
    {
      this.m_pendingTasks.Add(task);
      if (this.m_runningTasks.Count != 0)
        return;
      this.CommitWorklogIfNeeded();
    }

    public StringBuilder Dump()
    {
      StringBuilder sb = new StringBuilder();
      foreach (MyProfilerBlock rootBlock in this.m_rootBlocks)
        rootBlock.Dump(sb, this.m_lastFrameIndex);
      return sb;
    }

    public MyProfiler.MyProfilerObjectBuilderInfo GetObjectBuilderInfo()
    {
      MyProfiler.MyProfilerObjectBuilderInfo objectBuilderInfo = new MyProfiler.MyProfilerObjectBuilderInfo()
      {
        ProfilingBlocks = this.m_profilingBlocks,
        RootBlocks = this.m_rootBlocks,
        CustomName = this.m_customName,
        AxisName = this.m_axisName,
        TotalCalls = this.TotalCalls,
        ShallowProfile = this.ShallowProfileEnabled,
        CommitTimes = this.CommitTimes
      };
      lock (this.TaskLock)
      {
        objectBuilderInfo.Tasks = new List<MyProfiler.TaskInfo>(this.FinishedTasks.Count);
        objectBuilderInfo.Tasks.AddRange((IEnumerable<MyProfiler.TaskInfo>) this.FinishedTasks);
      }
      return objectBuilderInfo;
    }

    public void SetShallowProfile(bool shallowProfile) => this.PendingShallowProfileState = shallowProfile;

    public void Init(MyProfiler.MyProfilerObjectBuilderInfo data)
    {
      this.m_profilingBlocks = data.ProfilingBlocks;
      foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in this.m_profilingBlocks)
      {
        if (profilingBlock.Value.Id >= this.m_nextId)
          this.m_nextId = profilingBlock.Value.Id + 1;
      }
      this.m_rootBlocks = data.RootBlocks;
      this.m_customName = data.CustomName;
      this.m_axisName = data.AxisName;
      this.TotalCalls = data.TotalCalls;
      this.CommitTimes = data.CommitTimes ?? new long[MyProfiler.MAX_FRAMES];
      this.PendingShallowProfileState = this.ShallowProfileEnabled = data.ShallowProfile;
      this.FinishedTasks = new MyQueue<MyProfiler.TaskInfo>((IEnumerable<MyProfiler.TaskInfo>) data.Tasks);
    }

    public void SubtractFrom(MyProfiler otherProfiler)
    {
      Dictionary<MyProfilerBlock, MyProfilerBlock> dictionary = new Dictionary<MyProfilerBlock, MyProfilerBlock>();
      foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock1 in this.m_profilingBlocks)
      {
        bool flag = false;
        foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock2 in otherProfiler.m_profilingBlocks)
        {
          MyProfilerBlockKey key = profilingBlock2.Key;
          if (key.IsSimilarLocation(profilingBlock1.Key))
          {
            MyProfilerBlock parent1 = profilingBlock1.Value;
            MyProfilerBlock parent2;
            for (parent2 = profilingBlock2.Value; parent1.Parent != null && parent2.Parent != null; parent2 = parent2.Parent)
            {
              key = parent1.Parent.Key;
              if (key.IsSimilarLocation(parent2.Parent.Key))
                parent1 = parent1.Parent;
              else
                break;
            }
            if (parent1.Parent == null && parent2.Parent == null)
            {
              flag = true;
              profilingBlock1.Value.SubtractFrom(profilingBlock2.Value);
              dictionary.Add(profilingBlock2.Value, profilingBlock1.Value);
              break;
            }
          }
        }
        if (!flag)
          profilingBlock1.Value.Invert();
      }
      Stack<MyProfilerBlock> myProfilerBlockStack = new Stack<MyProfilerBlock>();
      foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in otherProfiler.m_profilingBlocks)
      {
        if (!dictionary.ContainsKey(profilingBlock.Value))
        {
          MyProfilerBlock parent1 = profilingBlock.Value;
          myProfilerBlockStack.Push(parent1);
          while (parent1.Parent != null && !dictionary.ContainsKey(parent1.Parent))
          {
            parent1 = parent1.Parent;
            myProfilerBlockStack.Push(parent1);
          }
          MyProfilerBlock parent2 = parent1.Parent != null ? dictionary[parent1.Parent] : (MyProfilerBlock) null;
          while (myProfilerBlockStack.Count > 0)
          {
            MyProfilerBlock key = myProfilerBlockStack.Pop();
            MyProfilerBlock myProfilerBlock = key.Duplicate(this.m_nextId++, parent2);
            if (parent2 == null)
              this.m_rootBlocks.Add(myProfilerBlock);
            this.m_profilingBlocks.Add(myProfilerBlock.Key, myProfilerBlock);
            parent2 = myProfilerBlock;
            dictionary.Add(key, myProfilerBlock);
          }
          myProfilerBlockStack.Clear();
        }
      }
      for (int index = 0; index < MyProfiler.MAX_FRAMES; ++index)
        this.TotalCalls[index] = otherProfiler.TotalCalls[index] - this.TotalCalls[index];
    }

    public struct HistoryLock : IDisposable
    {
      private readonly MyProfiler m_profiler;
      private FastResourceLock m_lock;

      public HistoryLock(MyProfiler profiler, FastResourceLock historyLock)
      {
        this.m_profiler = profiler;
        this.m_lock = historyLock;
        this.m_lock.AcquireExclusive();
        this.m_profiler.OnHistorySafe();
      }

      public void Dispose()
      {
        this.m_profiler.OnHistorySafe();
        this.m_lock.ReleaseExclusive();
        this.m_lock = (FastResourceLock) null;
      }
    }

    public enum TaskType
    {
      None = 0,
      Wait = 1,
      SyncWait = 2,
      WorkItem = 3,
      Block = 4,
      Physics = 5,
      RenderCull = 6,
      Voxels = 7,
      Precalc = 8,
      Deformations = 9,
      PreparePass = 10, // 0x0000000A
      RenderPass = 11, // 0x0000000B
      ClipMap = 12, // 0x0000000C
      AssetLoad = 13, // 0x0000000D
      GUI = 14, // 0x0000000E
      Profiler = 15, // 0x0000000F
      Loading = 16, // 0x00000010
      Networking = 17, // 0x00000011
      HK_Schedule = 101, // 0x00000065
      HK_Execute = 102, // 0x00000066
      HK_AwaitTasks = 103, // 0x00000067
      HK_Finish = 104, // 0x00000068
      HK_JOB_TYPE_DYNAMICS = 105, // 0x00000069
      HK_JOB_TYPE_COLLIDE = 106, // 0x0000006A
      HK_JOB_TYPE_COLLISION_QUERY = 107, // 0x0000006B
      HK_JOB_TYPE_RAYCAST_QUERY = 108, // 0x0000006C
      HK_JOB_TYPE_DESTRUCTION = 109, // 0x0000006D
      HK_JOB_TYPE_CHARACTER_PROXY = 110, // 0x0000006E
      HK_JOB_TYPE_COLLIDE_STATIC_COMPOUND = 111, // 0x0000006F
      HK_JOB_TYPE_OTHER = 112, // 0x00000070
    }

    public struct TaskInfo
    {
      public long Started;
      public long Finished;
      public long Scheduled;
      public string Name;
      public MyProfiler.TaskType TaskType;
      public float CustomValue;
    }

    public class MyProfilerObjectBuilderInfo
    {
      public Dictionary<MyProfilerBlockKey, MyProfilerBlock> ProfilingBlocks;
      public List<MyProfilerBlock> RootBlocks;
      public string CustomName;
      public string AxisName;
      public int[] TotalCalls;
      public long[] CommitTimes;
      public bool ShallowProfile;
      public List<MyProfiler.TaskInfo> Tasks;
    }
  }
}
