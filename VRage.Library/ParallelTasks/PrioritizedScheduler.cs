// Decompiled with JetBrains decompiler
// Type: ParallelTasks.PrioritizedScheduler
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using VRage.Collections;

namespace ParallelTasks
{
  public class PrioritizedScheduler : IWorkScheduler
  {
    private readonly int[] m_mappingPriorityToWorker = new int[5]
    {
      0,
      1,
      1,
      1,
      2
    };
    private readonly ThreadPriority[] m_mappingWorkerToThreadPriority = new ThreadPriority[3]
    {
      ThreadPriority.Highest,
      ThreadPriority.Normal,
      ThreadPriority.Lowest
    };
    private readonly float[] m_mappingWorkerToThreadFactor = new float[3]
    {
      1f,
      1f,
      2f
    };
    private readonly float[] m_mappingWorkerToThreadFactorAMD = new float[3]
    {
      1f,
      1f,
      1f
    };
    private PrioritizedScheduler.WorkerArray[] m_workerArrays;
    private WaitHandle[] m_hasNoWork;

    public int ThreadCount => this.m_workerArrays[0].Workers.Length;

    public PrioritizedScheduler(int threadCount, bool amd) => this.InitializeWorkerArrays(threadCount, amd);

    private void InitializeWorkerArrays(int threadCount, bool amd)
    {
      int num1 = 0;
      foreach (int num2 in this.m_mappingPriorityToWorker)
        num1 = num2 > num1 ? num2 : num1;
      float[] numArray1 = amd ? this.m_mappingWorkerToThreadFactorAMD : this.m_mappingWorkerToThreadFactor;
      int length = 0;
      int[] numArray2 = new int[numArray1.Length];
      for (int index = 0; index < numArray2.Length; ++index)
      {
        numArray2[index] = (int) Math.Floor((double) numArray1[index] * (double) threadCount);
        length += numArray2[index];
      }
      this.m_workerArrays = new PrioritizedScheduler.WorkerArray[num1 + 1];
      this.m_hasNoWork = new WaitHandle[length];
      int num3 = 0;
      for (int workerArrayIndex = 0; workerArrayIndex <= num1; ++workerArrayIndex)
      {
        int threadCount1 = numArray2[workerArrayIndex];
        this.m_workerArrays[workerArrayIndex] = new PrioritizedScheduler.WorkerArray(this, workerArrayIndex, threadCount1, this.m_mappingWorkerToThreadPriority[workerArrayIndex]);
        for (int index = 0; index < threadCount1; ++index)
          this.m_hasNoWork[num3++] = (WaitHandle) this.m_workerArrays[workerArrayIndex].Workers[index].HasNoWork;
      }
    }

    private PrioritizedScheduler.WorkerArray GetWorkerArray(
      WorkPriority priority)
    {
      return this.m_workerArrays[this.m_mappingPriorityToWorker[(int) priority]];
    }

    public void Schedule(Task task)
    {
      if (task.Item.Work == null)
        return;
      WorkPriority priority = task.Item.WorkData != null ? task.Item.WorkData.Priority : WorkPriority.Normal;
      if (task.Item.Work is IPrioritizedWork work)
        priority = work.Priority;
      this.GetWorkerArray(priority).Schedule(task);
    }

    public bool WaitForTasksToFinish(TimeSpan waitTimeout) => Parallel.WaitForAll(this.m_hasNoWork, waitTimeout);

    public void ScheduleOnEachWorker(Action action)
    {
      List<Task> taskList = new List<Task>();
      foreach (PrioritizedScheduler.WorkerArray workerArray in this.m_workerArrays)
        taskList.Add(workerArray.ScheduleOnEachWorker(action));
      foreach (Task task in taskList)
        task.Wait();
    }

    public int ReadAndClearExecutionTime()
    {
      int num = 0;
      foreach (PrioritizedScheduler.WorkerArray workerArray in this.m_workerArrays)
        num += workerArray.ReadAndClearWorklog();
      return num;
    }

    public void SuspendThreads(TimeSpan waitTimeout)
    {
      foreach (PrioritizedScheduler.WorkerArray workerArray in this.m_workerArrays)
        workerArray.Suspend();
      this.WaitForTasksToFinish(waitTimeout);
    }

    public void ResumeThreads()
    {
      foreach (PrioritizedScheduler.WorkerArray workerArray in this.m_workerArrays)
        workerArray.Resume();
    }

    public void SuspendThreads(WorkPriority priority) => this.GetWorkerArray(priority).Suspend();

    public void ResumeThreads(WorkPriority priority) => this.GetWorkerArray(priority).Resume();

    private class WorkerArray
    {
      private bool m_suspended;
      private PrioritizedScheduler m_prioritizedScheduler;
      private readonly int m_workerArrayIndex;
      private readonly Queue<Task> m_taskQueue = new Queue<Task>(64);
      private readonly PrioritizedScheduler.Worker[] m_workers;
      private const int DEFAULT_QUEUE_CAPACITY = 64;

      public PrioritizedScheduler.Worker[] Workers => this.m_workers;

      public WorkerArray(
        PrioritizedScheduler prioritizedScheduler,
        int workerArrayIndex,
        int threadCount,
        ThreadPriority systemThreadPriority)
      {
        this.m_workerArrayIndex = workerArrayIndex;
        this.m_prioritizedScheduler = prioritizedScheduler;
        this.m_workers = new PrioritizedScheduler.Worker[threadCount];
        for (int workerIndex = 0; workerIndex < threadCount; ++workerIndex)
          this.m_workers[workerIndex] = new PrioritizedScheduler.Worker(this, "Parallel " + (object) systemThreadPriority + "_" + (object) workerIndex, systemThreadPriority, workerIndex);
      }

      public bool TryGetTask(out Task task)
      {
        if (this.m_suspended)
        {
          task = new Task();
          return false;
        }
        lock (this.m_taskQueue)
          return this.m_taskQueue.TryDequeue<Task>(out task);
      }

      public void Schedule(Task task)
      {
        int val1 = task.Item.Work.Options.MaximumThreads;
        if (val1 < 1)
          val1 = 1;
        int num = Math.Min(val1, this.m_workers.Length);
        lock (this.m_taskQueue)
        {
          for (int index = 0; index < num; ++index)
            this.m_taskQueue.Enqueue(task);
        }
        foreach (PrioritizedScheduler.Worker worker in this.m_workers)
          worker.Gate.Set();
      }

      public Task ScheduleOnEachWorker(Action action)
      {
        Barrier barrier = new Barrier(this.Workers.Length);
        ActionWork actionWork = new ActionWork((Action<WorkData>) (_ =>
        {
          barrier.SignalAndWait();
          action();
        }), Parallel.DefaultOptions.WithMaxThreads(this.Workers.Length));
        WorkItem workItem = WorkItem.Get();
        workItem.Callback = (Action) null;
        workItem.WorkData = (WorkData) null;
        workItem.CompletionCallbacks = (ConcurrentCachingList<WorkItem>) null;
        Task task = workItem.PrepareStart((IWork) actionWork);
        this.Schedule(task);
        return task;
      }

      public int ReadAndClearWorklog()
      {
        int num = 0;
        foreach (PrioritizedScheduler.Worker worker in this.m_workers)
          num += worker.ReadAndClearWorklog();
        return num;
      }

      public void Suspend() => this.m_suspended = true;

      public void Resume()
      {
        if (!this.m_suspended)
          return;
        this.m_suspended = false;
        foreach (PrioritizedScheduler.Worker worker in this.m_workers)
          worker.Gate.Set();
      }
    }

    private class Worker
    {
      private readonly PrioritizedScheduler.WorkerArray m_workerArray;
      private readonly int m_workerIndex;
      private readonly Thread m_thread;
      public readonly ManualResetEvent HasNoWork;
      public readonly AutoResetEvent Gate;
      private long Worklog;
      private int ExecutedWork;

      public Thread Thread => this.m_thread;

      public Worker(
        PrioritizedScheduler.WorkerArray workerArray,
        string name,
        ThreadPriority priority,
        int workerIndex)
      {
        this.m_workerArray = workerArray;
        this.m_workerIndex = workerIndex;
        this.m_thread = new Thread(new ThreadStart(this.WorkerLoop));
        this.HasNoWork = new ManualResetEvent(false);
        this.Gate = new AutoResetEvent(false);
        this.m_thread.Name = name;
        this.m_thread.IsBackground = true;
        this.m_thread.Priority = priority;
        this.m_thread.CurrentCulture = CultureInfo.InvariantCulture;
        this.m_thread.CurrentUICulture = CultureInfo.InvariantCulture;
        this.m_thread.Start();
      }

      private void OpenWork() => Interlocked.Exchange(ref this.Worklog, Stopwatch.GetTimestamp());

      private void CloseWork() => Interlocked.Add(ref this.ExecutedWork, (int) (Stopwatch.GetTimestamp() - Interlocked.Exchange(ref this.Worklog, 0L)));

      public int ReadAndClearWorklog()
      {
        long worklog = this.Worklog;
        if (worklog != 0L)
          Interlocked.CompareExchange(ref this.Worklog, Stopwatch.GetTimestamp(), worklog);
        return Interlocked.Exchange(ref this.ExecutedWork, 0);
      }

      private void WorkerLoop()
      {
        while (true)
        {
          Task task;
          while (!this.m_workerArray.TryGetTask(out task))
          {
            this.HasNoWork.Set();
            this.Gate.WaitOne();
            this.HasNoWork.Reset();
          }
          this.OpenWork();
          task.DoWork();
          this.CloseWork();
        }
      }
    }
  }
}
