// Decompiled with JetBrains decompiler
// Type: ParallelTasks.WorkItem
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
  [GenerateActivator]
  public class WorkItem
  {
    public static Action<Exception> ErrorReportingFunction;
    private IWork m_work;
    private int m_executing;
    private long m_scheduledTimestamp;
    private volatile int m_runCount;
    private List<Exception> m_exceptionBuffer;
    private object m_executionLock = new object();
    private readonly ManualResetEvent m_resetEvent;
    private Exception[] m_exceptions;
    private static readonly MyConcurrentPool<WorkItem> m_idleWorkItems = new MyConcurrentPool<WorkItem>(1000, expectedAllocations: 100000);
    private static readonly ConcurrentDictionary<Thread, Stack<Task>> m_runningTasks = new ConcurrentDictionary<Thread, Stack<Task>>(Environment.ProcessorCount, Environment.ProcessorCount);
    public const string PerformanceProfilingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";
    private static Action<MyProfiler.TaskType, string, long> m_onTaskStartedDelegate = (Action<MyProfiler.TaskType, string, long>) ((x, y, z) => {});
    private static Action m_onTaskFinishedDelegate = (Action) (() => {});
    private static Action<string> m_onProfilerBeginDelegate = (Action<string>) (x => {});
    private static Action<float> m_onProfilerEndDelegate = (Action<float>) (x => {});
    private static Action<int, bool> m_initThread = (Action<int, bool>) ((x, y) => {});

    public IWork Work => this.m_work;

    public WorkData WorkData { get; set; }

    public Action Callback { get; set; }

    public Action<WorkData> DataCallback { get; set; }

    public ConcurrentCachingList<WorkItem> CompletionCallbacks { get; set; }

    public int RunCount => this.m_runCount;

    public static Stack<Task> ThisThreadTasks
    {
      get
      {
        Thread currentThread = Thread.CurrentThread;
        Stack<Task> taskStack;
        if (!WorkItem.m_runningTasks.TryGetValue(currentThread, out taskStack))
        {
          taskStack = new Stack<Task>(5);
          WorkItem.m_runningTasks.TryAdd(currentThread, taskStack);
        }
        return taskStack;
      }
    }

    public static Task? CurrentTask
    {
      get
      {
        Stack<Task> thisThreadTasks = WorkItem.ThisThreadTasks;
        return thisThreadTasks.Count == 0 ? new Task?() : new Task?(thisThreadTasks.Peek());
      }
    }

    public WorkItem() => this.m_resetEvent = new ManualResetEvent(true);

    public Task PrepareStart(IWork work, Thread thread = null)
    {
      if (this.m_exceptions != null)
        this.m_exceptions = (Exception[]) null;
      this.m_work = work;
      this.m_resetEvent.Reset();
      return new Task(this);
    }

    public bool DoWork(int expectedID)
    {
      lock (this.m_executionLock)
      {
        if (expectedID < this.m_runCount)
          return true;
        if (this.m_work == null || this.m_executing == this.m_work.Options.MaximumThreads)
          return false;
        ++this.m_executing;
      }
      Stack<Task> thisThreadTasks = WorkItem.ThisThreadTasks;
      thisThreadTasks.Push(new Task(this));
      try
      {
        this.m_work.DoWork(this.WorkData);
      }
      catch (Exception ex)
      {
        ex.Data[(object) "Exception Info"] = (object) string.Format("Holder ID: {0}; m_runCount: {1}; m_executing: {2}; MaximumThreads: {3}", (object) expectedID, (object) this.m_runCount, (object) this.m_executing, (object) this.m_work.Options.MaximumThreads);
        ex.Data[(object) "Work Options"] = (object) this.m_work.Options.ToString();
        if (Parallel.THROW_WORKER_EXCEPTIONS)
        {
          WorkItem.ErrorReportingFunction(ex);
          throw;
        }
        else
        {
          if (this.m_exceptionBuffer == null)
            Interlocked.CompareExchange<List<Exception>>(ref this.m_exceptionBuffer, new List<Exception>(), (List<Exception>) null);
          lock (this.m_exceptionBuffer)
            this.m_exceptionBuffer.Add(ex);
        }
      }
      thisThreadTasks.Pop();
      lock (this.m_executionLock)
      {
        --this.m_executing;
        if (this.m_executing != 0)
          return false;
        if (this.m_exceptionBuffer != null)
        {
          this.m_exceptions = this.m_exceptionBuffer.ToArray();
          this.m_exceptionBuffer = (List<Exception>) null;
        }
        ++this.m_runCount;
        this.m_resetEvent.Set();
        if (this.Callback == null && this.DataCallback == null)
          this.Requeue();
        else
          this.CompletionCallbacks.Add(this);
        return true;
      }
    }

    public void Requeue()
    {
      if (this.m_runCount >= int.MaxValue || this.m_exceptions != null)
        return;
      this.m_work = (IWork) null;
      WorkItem.m_idleWorkItems.Return(this);
    }

    public Exception[] GetExceptions(int runId)
    {
      lock (this.m_executionLock)
        return this.GetExceptionsInternal(runId);
    }

    public void WaitOrExecute(int id, bool blocking = false)
    {
      this.WaitOrExecuteInternal(id, blocking);
      this.ThrowExceptionsInternal(id);
    }

    public void Execute(int id)
    {
      if (this.m_runCount != id || !this.DoWork(id))
        return;
      this.ThrowExceptionsInternal(id);
    }

    private void WaitOrExecuteInternal(int id, bool blocking = false)
    {
      if (this.m_runCount != id || this.DoWork(id))
        return;
      this.Wait(id, blocking);
    }

    private void ThrowExceptionsInternal(int runId)
    {
      Exception[] exceptionsInternal = this.GetExceptionsInternal(runId);
      if (exceptionsInternal != null)
        throw new TaskException(exceptionsInternal);
    }

    public Exception[] GetExceptionsInternal(int runId)
    {
      int runCount = this.m_runCount;
      return this.m_exceptions != null && runCount == runId + 1 ? this.m_exceptions : (Exception[]) null;
    }

    public static WorkItem Get() => WorkItem.m_idleWorkItems.Get();

    public static void Clean() => WorkItem.m_idleWorkItems.Clean();

    public void Wait(int id, bool blocking)
    {
      if (this.m_runCount != id)
        return;
      try
      {
        MySpinWait mySpinWait = new MySpinWait();
        if (blocking)
        {
          while (this.m_runCount == id)
            mySpinWait.SpinOnce();
        }
        else
        {
          while (this.m_runCount == id)
          {
            if (mySpinWait.Count > 1000)
              this.m_resetEvent.WaitOne();
            else
              mySpinWait.SpinOnce();
          }
        }
      }
      finally
      {
      }
    }

    public static void SetupProfiler(
      Action<MyProfiler.TaskType, string, long> onTaskStarted,
      Action onTaskFinished,
      Action<string> begin,
      Action<float> end,
      Action<int, bool> initThread)
    {
      WorkItem.m_onTaskStartedDelegate = onTaskStarted;
      WorkItem.m_onTaskFinishedDelegate = onTaskFinished;
      WorkItem.m_onProfilerBeginDelegate = begin;
      WorkItem.m_onProfilerEndDelegate = end;
      WorkItem.m_initThread = initThread;
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private static void OnTaskScheduled(WorkItem task) => task.m_scheduledTimestamp = Stopwatch.GetTimestamp();

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskStarted(WorkItem task)
    {
      WorkOptions options = task.Work.Options;
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskFinished(WorkItem task)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskStarted(
      MyProfiler.TaskType taskType,
      string debugName,
      long scheduledTimestamp = -1)
    {
      WorkItem.m_onTaskStartedDelegate(taskType, debugName, scheduledTimestamp);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void OnTaskFinished() => WorkItem.m_onTaskFinishedDelegate();

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void ProfilerBegin(string symbol) => WorkItem.m_onProfilerBeginDelegate(symbol);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void ProfilerEnd(float customValue = 0.0f) => WorkItem.m_onProfilerEndDelegate(customValue);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void InitThread(int priority, bool simulation) => WorkItem.m_initThread(priority, simulation);

    private class ParallelTasks_WorkItem\u003C\u003EActor : IActivator, IActivator<WorkItem>
    {
      object IActivator.CreateInstance() => (object) new WorkItem();

      WorkItem IActivator<WorkItem>.CreateInstance() => new WorkItem();
    }
  }
}
