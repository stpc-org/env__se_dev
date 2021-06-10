// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Parallel
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Profiler;

namespace ParallelTasks
{
  public static class Parallel
  {
    public static readonly bool THROW_WORKER_EXCEPTIONS = false;
    public static readonly WorkOptions DefaultOptions = new WorkOptions()
    {
      MaximumThreads = 1,
      TaskType = MyProfiler.TaskType.WorkItem
    };
    private static IWorkScheduler scheduler;
    private static Pool<List<Task>> taskPool = new Pool<List<Task>>();
    private static readonly Dictionary<Thread, ConcurrentCachingList<WorkItem>> Buffers = new Dictionary<Thread, ConcurrentCachingList<WorkItem>>(8);
    [ThreadStatic]
    private static ConcurrentCachingList<WorkItem> m_callbackBuffer;
    private static int[] _processorAffinity = new int[4]
    {
      3,
      4,
      5,
      1
    };

    public static ConcurrentCachingList<WorkItem> CallbackBuffer
    {
      get
      {
        Task? currentTask = WorkItem.CurrentTask;
        Task valueOrDefault = currentTask.GetValueOrDefault();
        if (currentTask.HasValue)
          return valueOrDefault.Item.CompletionCallbacks;
        if (Parallel.m_callbackBuffer == null)
        {
          Parallel.m_callbackBuffer = new ConcurrentCachingList<WorkItem>(16);
          lock (Parallel.Buffers)
            Parallel.Buffers.Add(Thread.CurrentThread, Parallel.m_callbackBuffer);
        }
        return Parallel.m_callbackBuffer;
      }
    }

    public static void RunCallbacks()
    {
      Parallel.CallbackBuffer.ApplyChanges();
      for (int index = 0; index < Parallel.CallbackBuffer.Count; ++index)
      {
        WorkItem workItem = Parallel.CallbackBuffer[index];
        if (workItem != null)
        {
          if (workItem.Callback != null)
          {
            workItem.Callback();
            workItem.Callback = (Action) null;
          }
          if (workItem.DataCallback != null)
          {
            workItem.DataCallback(workItem.WorkData);
            workItem.DataCallback = (Action<WorkData>) null;
          }
          workItem.WorkData = (WorkData) null;
          workItem.Requeue();
        }
      }
      Parallel.CallbackBuffer.ClearList();
    }

    public static int[] ProcessorAffinity
    {
      get => Parallel._processorAffinity;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        if (value.Length < 1)
          throw new ArgumentException("The Parallel.ProcessorAffinity must contain at least one value.", nameof (value));
        Parallel._processorAffinity = !((IEnumerable<int>) value).Any<int>((Func<int, bool>) (id => id < 0)) ? value : throw new ArgumentException("The processor affinity must not be negative.", nameof (value));
      }
    }

    public static IWorkScheduler Scheduler
    {
      get => Parallel.scheduler;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        Interlocked.Exchange<IWorkScheduler>(ref Parallel.scheduler, value);
      }
    }

    public static Task StartBackground(IWork work) => Parallel.StartBackground(work, (Action) null);

    public static Task StartBackground(IWork work, Action completionCallback)
    {
      if (work == null)
        throw new ArgumentNullException(nameof (work));
      if (work.Options.MaximumThreads < 1)
        throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
      WorkItem workItem = WorkItem.Get();
      workItem.CompletionCallbacks = Parallel.CallbackBuffer;
      if (completionCallback != null)
        workItem.Callback = completionCallback;
      workItem.WorkData = (WorkData) null;
      Task work1 = workItem.PrepareStart(work);
      BackgroundWorker.StartWork(work1);
      return work1;
    }

    public static Task StartBackground(Action action) => Parallel.StartBackground(action, (Action) null);

    public static Task StartBackground(Action action, Action completionCallback)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      DelegateWork instance = DelegateWork.GetInstance();
      instance.Action = action;
      instance.Options = Parallel.DefaultOptions;
      return Parallel.StartBackground((IWork) instance, completionCallback);
    }

    public static Task StartBackground(
      Action<WorkData> action,
      Action<WorkData> completionCallback,
      WorkData workData)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      DelegateWork instance = DelegateWork.GetInstance();
      instance.DataAction = action;
      instance.Options = Parallel.DefaultOptions;
      WorkItem workItem = WorkItem.Get();
      workItem.CompletionCallbacks = Parallel.CallbackBuffer;
      if (completionCallback != null)
        workItem.DataCallback = completionCallback;
      workItem.WorkData = workData;
      Task work = workItem.PrepareStart((IWork) instance);
      BackgroundWorker.StartWork(work);
      return work;
    }

    public static Task Start(IWork work) => Parallel.Start(work, (Action) null);

    public static Task Start(IWork work, Action completionCallback)
    {
      if (work == null)
        throw new ArgumentNullException(nameof (work));
      if (work.Options.MaximumThreads < 1)
        throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
      WorkItem workItem = WorkItem.Get();
      workItem.CompletionCallbacks = Parallel.CallbackBuffer;
      if (completionCallback != null)
        workItem.Callback = completionCallback;
      workItem.WorkData = (WorkData) null;
      Task task = workItem.PrepareStart(work);
      Parallel.Scheduler.Schedule(task);
      return task;
    }

    public static Task Start(Action action, WorkPriority priority = WorkPriority.Normal) => Parallel.Start(action, (Action) null, priority);

    public static Task Start(WorkPriority priority, Action action) => Parallel.Start(priority, action, Parallel.DefaultOptions);

    public static Task Start(WorkPriority priority, Action action, WorkOptions options)
    {
      DelegateWork instance = DelegateWork.GetInstance();
      instance.Priority = priority;
      instance.Action = action;
      instance.Options = options;
      return Parallel.Start((IWork) instance, (Action) null);
    }

    public static Task Start(Action action, Action completionCallback, WorkPriority priority = WorkPriority.Normal) => Parallel.Start(action, new WorkOptions()
    {
      MaximumThreads = 1,
      QueueFIFO = false
    }, completionCallback, priority);

    public static Task Start(Action action, WorkOptions options, WorkPriority priority = WorkPriority.Normal) => Parallel.Start(action, options, (Action) null, priority);

    public static Task Start(
      Action action,
      WorkOptions options,
      Action completionCallback,
      WorkPriority priority = WorkPriority.Normal)
    {
      DelegateWork instance = DelegateWork.GetInstance();
      instance.Action = action;
      instance.Options = options;
      instance.Priority = priority;
      return Parallel.Start((IWork) instance, completionCallback);
    }

    public static Task Start(
      Action<WorkData> action,
      Action<WorkData> completionCallback,
      WorkData workData)
    {
      return Parallel.Start(action, completionCallback, workData, Parallel.DefaultOptions);
    }

    public static Task Start(
      Action<WorkData> action,
      Action<WorkData> completionCallback,
      WorkData workData,
      WorkOptions options,
      WorkPriority priority = WorkPriority.Normal)
    {
      DelegateWork instance = DelegateWork.GetInstance();
      instance.DataAction = action;
      instance.Options = options;
      instance.Priority = priority;
      WorkItem workItem = WorkItem.Get();
      workItem.CompletionCallbacks = Parallel.CallbackBuffer;
      if (completionCallback != null)
        workItem.DataCallback = completionCallback;
      workItem.WorkData = workData;
      Task task = workItem.PrepareStart((IWork) instance);
      Parallel.Scheduler.Schedule(task);
      return task;
    }

    public static Task ScheduleForThread(
      Action<WorkData> action,
      WorkData workData,
      Thread thread = null)
    {
      if (thread == null)
        thread = Thread.CurrentThread;
      WorkOptions workOptions = new WorkOptions()
      {
        MaximumThreads = 1,
        QueueFIFO = false
      };
      DelegateWork instance = DelegateWork.GetInstance();
      instance.Options = workOptions;
      WorkItem entity = WorkItem.Get();
      lock (Parallel.Buffers)
        entity.CompletionCallbacks = Parallel.Buffers[thread];
      entity.DataCallback = action;
      entity.WorkData = workData;
      Task task = entity.PrepareStart((IWork) instance);
      entity.CompletionCallbacks.Add(entity);
      return task;
    }

    public static void StartOnEachWorker(Action action) => Parallel.Scheduler.ScheduleOnEachWorker(action);

    public static Future<T> Start<T>(Func<T> function, WorkPriority priority = WorkPriority.Normal) => Parallel.Start<T>(function, (Action) null, priority);

    public static Future<T> Start<T>(
      Func<T> function,
      Action completionCallback,
      WorkPriority priority = WorkPriority.Normal)
    {
      return Parallel.Start<T>(function, Parallel.DefaultOptions, completionCallback, priority);
    }

    public static Future<T> Start<T>(
      Func<T> function,
      WorkOptions options,
      WorkPriority priority = WorkPriority.Normal)
    {
      return Parallel.Start<T>(function, options, (Action) null, priority);
    }

    public static Future<T> Start<T>(
      Func<T> function,
      WorkOptions options,
      Action completionCallback,
      WorkPriority priority = WorkPriority.Normal)
    {
      if (options.MaximumThreads < 1)
        throw new ArgumentOutOfRangeException(nameof (options), "options.MaximumThreads cannot be less than 1.");
      FutureWork<T> instance = FutureWork<T>.GetInstance();
      instance.Function = function;
      instance.Options = options;
      instance.Priority = priority;
      return new Future<T>(Parallel.Start((IWork) instance, completionCallback), instance);
    }

    public static void Do(IWork a, IWork b)
    {
      Task task = Parallel.Start(b);
      a.DoWork();
      task.WaitOrExecute();
    }

    public static void Do(params IWork[] work)
    {
      List<Task> instance = Parallel.taskPool.Get(Thread.CurrentThread);
      for (int index = 0; index < work.Length; ++index)
        instance.Add(Parallel.Start(work[index]));
      for (int index = 0; index < instance.Count; ++index)
        instance[index].WaitOrExecute();
      instance.Clear();
      Parallel.taskPool.Return(Thread.CurrentThread, instance);
    }

    public static void Do(Action action1, Action action2)
    {
      DelegateWork instance = DelegateWork.GetInstance();
      instance.Action = action2;
      instance.Options = Parallel.DefaultOptions;
      Task task = Parallel.Start((IWork) instance);
      action1();
      task.WaitOrExecute();
    }

    public static void Do(params Action[] actions)
    {
      List<Task> instance1 = Parallel.taskPool.Get(Thread.CurrentThread);
      for (int index = 0; index < actions.Length; ++index)
      {
        DelegateWork instance2 = DelegateWork.GetInstance();
        instance2.Action = actions[index];
        instance2.Options = Parallel.DefaultOptions;
        instance1.Add(Parallel.Start((IWork) instance2));
      }
      for (int index = 0; index < actions.Length; ++index)
        instance1[index].WaitOrExecute();
      instance1.Clear();
      Parallel.taskPool.Return(Thread.CurrentThread, instance1);
    }

    public static void For(
      int startInclusive,
      int endExclusive,
      Action<int> body,
      WorkPriority priority = WorkPriority.Normal,
      WorkOptions? options = null)
    {
      Parallel.For(startInclusive, endExclusive, body, 1, priority, options);
    }

    public static void For(
      int startInclusive,
      int endExclusive,
      Action<int> body,
      int stride,
      WorkPriority priority = WorkPriority.Normal,
      WorkOptions? options = null,
      bool blocking = false)
    {
      int num = (endExclusive - startInclusive + (stride - 1)) / stride;
      if (num <= 0)
        return;
      if (num == 1)
      {
        for (int index = startInclusive; index < endExclusive; ++index)
          body(index);
      }
      else
      {
        ForLoopWork forLoopWork = ForLoopWork.Get();
        forLoopWork.Prepare(body, startInclusive, endExclusive, stride, priority);
        WorkOptions workOptions = options ?? Parallel.DefaultOptions;
        workOptions.MaximumThreads = num;
        forLoopWork.Options = workOptions;
        Parallel.Start((IWork) forLoopWork).WaitOrExecute(blocking);
        forLoopWork.Return();
      }
    }

    public static void ForEach<T>(
      IEnumerable<T> collection,
      Action<T> action,
      WorkPriority priority = WorkPriority.Normal,
      WorkOptions? options = null,
      bool blocking = false)
    {
      ForEachLoopWork<T> forEachLoopWork = ForEachLoopWork<T>.Get();
      forEachLoopWork.Prepare(action, collection.GetEnumerator(), priority);
      WorkOptions defaultOptions;
      if (options.HasValue)
      {
        defaultOptions = options.Value;
      }
      else
      {
        defaultOptions = Parallel.DefaultOptions;
        defaultOptions.MaximumThreads = int.MaxValue;
      }
      forEachLoopWork.Options = defaultOptions;
      Parallel.Start((IWork) forEachLoopWork).WaitOrExecute(blocking);
      forEachLoopWork.Return();
    }

    public static void Clean()
    {
      Parallel.CallbackBuffer.ApplyChanges();
      Parallel.CallbackBuffer.ClearList();
      Parallel.taskPool.Clean();
      lock (Parallel.Buffers)
      {
        foreach (ConcurrentCachingList<WorkItem> concurrentCachingList in Parallel.Buffers.Values)
          concurrentCachingList.ClearImmediate();
        Parallel.Buffers.Clear();
      }
      WorkItem.Clean();
    }

    public static bool WaitForAll(WaitHandle[] waitHandles, TimeSpan timeout)
    {
      if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
        return WaitHandle.WaitAll(waitHandles, timeout);
      bool result = false;
      Thread thread = new Thread((ThreadStart) (() => result = WaitHandle.WaitAll(waitHandles, timeout)));
      thread.SetApartmentState(ApartmentState.MTA);
      thread.Start();
      thread.Join();
      return result;
    }
  }
}
