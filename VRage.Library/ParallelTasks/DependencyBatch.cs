// Decompiled with JetBrains decompiler
// Type: ParallelTasks.DependencyBatch
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Profiler;

namespace ParallelTasks
{
  public class DependencyBatch : AbstractWork, IPrioritizedWork, IWork, IDisposable
  {
    public static Action<Exception> ErrorReportingFunction;
    private int m_maxThreads;
    private int m_jobCount;
    private int m_completedJobs;
    private Action[] m_jobs;
    private int[] m_jobStates;
    private int[] m_dependencies;
    private int[] m_dependencyStarts;
    private List<Exception> m_exceptionBuffer;
    private readonly AutoResetEvent m_completionAwaiter;
    private int m_controlThreadState = 3;
    private const int WORKERS_MASK = 65535;
    private const int JOBS_MASK = -65536;
    private const int JOBS_SHIFT = 16;
    private long m_scheduledJobsAndWorkers;
    private int m_allCompletedCachedIndex;

    public WorkPriority Priority { get; private set; }

    WorkOptions IWork.Options
    {
      get
      {
        WorkOptions options = base.Options;
        options.MaximumThreads = 1;
        return options;
      }
    }

    public int MaxThreads => this.m_maxThreads;

    public override sealed WorkOptions Options
    {
      get => base.Options;
      set
      {
        base.Options = value;
        this.m_maxThreads = Math.Min(value.MaximumThreads, Parallel.Scheduler.ThreadCount + 1);
      }
    }

    public int Add(Action job)
    {
      int index = this.m_jobCount++;
      this.EnsureCapacity(this.m_jobCount);
      this.m_jobs[index] = job;
      return index;
    }

    public DependencyBatch.StartToken Job(int jobId)
    {
      if (this.m_dependencyStarts[jobId] == -1)
        this.m_dependencyStarts[jobId] = this.GetLastInitializedStart(jobId - 1);
      return new DependencyBatch.StartToken(jobId, this);
    }

    public void Preallocate(int size)
    {
      if (this.m_jobs == null || this.m_jobs.Length < size)
        this.AllocateInternal(size);
      this.Clear(this.m_jobs.Length);
    }

    private void AllocateInternal(int size)
    {
      this.m_jobs = new Action[size];
      this.m_jobStates = new int[size];
      this.m_dependencyStarts = new int[size + 1];
      if (this.m_dependencies != null && this.m_dependencies.Length >= size)
        return;
      this.m_dependencies = new int[size];
    }

    public void Execute()
    {
      this.m_completedJobs = 0;
      if (this.m_jobCount == 0)
        return;
      int jobCount = this.m_jobCount;
      int num1 = this.GetLastInitializedStart(this.m_jobCount) - 1;
      for (int index = 0; index <= num1; ++index)
      {
        int dependency = this.m_dependencies[index];
        int jobState = this.m_jobStates[dependency];
        if (jobState == 2147483644)
          --jobCount;
        this.m_jobStates[dependency] = jobState - 1;
      }
      this.m_controlThreadState = 0;
      this.RegisterJobsForConsumption(jobCount);
      int num2 = Interlocked.CompareExchange(ref this.m_controlThreadState, 2, 1);
      this.m_completionAwaiter.Reset();
      if (num2 != 3)
      {
        do
        {
          this.WorkerLoop();
        }
        while (!this.MainThreadAwaiter());
      }
      try
      {
        if (this.m_exceptionBuffer != null && this.m_exceptionBuffer.Count > 0)
          throw new TaskException(this.m_exceptionBuffer.ToArray());
      }
      finally
      {
        this.Clear(this.m_jobCount);
      }
    }

    private bool MainThreadAwaiter()
    {
      if (Interlocked.CompareExchange(ref this.m_controlThreadState, 0, 2) == 3)
        return true;
      this.m_completionAwaiter.WaitOne();
      return Interlocked.CompareExchange(ref this.m_controlThreadState, 2, 1) == 3;
    }

    private void ReleaseMainThread()
    {
      int comparand = 2;
      bool flag1;
      bool flag2;
      while (true)
      {
        flag1 = false;
        flag2 = false;
        switch (comparand)
        {
          case 0:
            flag1 = true;
            goto case 2;
          case 1:
            flag2 = true;
            goto case 2;
          case 2:
            int num = Interlocked.CompareExchange(ref this.m_controlThreadState, 3, comparand);
            if (num != comparand)
            {
              comparand = num;
              continue;
            }
            goto label_7;
          case 3:
            goto label_2;
          default:
            goto label_12;
        }
      }
label_2:
      return;
label_12:
      return;
label_7:
      if (flag2)
        this.TryAcquireJob();
      if (!flag1)
        return;
      this.m_completionAwaiter.Set();
    }

    private bool TryWakingUpMainThread()
    {
      if (Interlocked.CompareExchange(ref this.m_controlThreadState, 1, 0) != 0)
        return false;
      this.m_completionAwaiter.Set();
      return true;
    }

    private void WorkerLoop()
    {
      if (!this.TryAcquireJob())
        return;
      do
      {
        int num = this.ExecuteSingleJob();
        if (num > 0)
        {
          if (num > 1)
          {
            this.RegisterJobsForConsumption(num - 1);
          }
          else
          {
            Interlocked.Add(ref this.m_scheduledJobsAndWorkers, 65536L);
            if (this.TryWakingUpMainThread())
              goto label_1;
          }
        }
      }
      while (this.TryAcquireJob());
      goto label_8;
label_1:
      return;
label_8:;
    }

    private int ExecuteSingleJob()
    {
      int index1 = -1;
      do
      {
        bool flag = false;
        int completedCachedIndex1;
        for (completedCachedIndex1 = this.m_allCompletedCachedIndex; completedCachedIndex1 < this.m_jobCount && this.m_jobStates[completedCachedIndex1] >= 2147483645; ++completedCachedIndex1)
          flag = true;
        if (flag)
        {
          int num = completedCachedIndex1 - 1;
          int completedCachedIndex2 = this.m_allCompletedCachedIndex;
          if (completedCachedIndex2 < num)
            Interlocked.CompareExchange(ref this.m_allCompletedCachedIndex, num, completedCachedIndex2);
        }
        for (; completedCachedIndex1 < this.m_jobCount; ++completedCachedIndex1)
        {
          if (this.m_jobStates[completedCachedIndex1] == 2147483644 && Interlocked.CompareExchange(ref this.m_jobStates[completedCachedIndex1], 2147483645, 2147483644) == 2147483644)
          {
            index1 = completedCachedIndex1;
            goto label_14;
          }
        }
      }
      while (Volatile.Read(ref this.m_completedJobs) < this.m_jobCount);
      if (index1 == -1)
        return 0;
label_14:
      Exception exception = (Exception) null;
      try
      {
        this.m_jobs[index1]();
      }
      catch (Exception ex)
      {
        exception = ex;
        Action<Exception> reportingFunction = DependencyBatch.ErrorReportingFunction;
        if (reportingFunction != null)
          reportingFunction(ex);
      }
      Volatile.Write(ref this.m_jobStates[index1], 2147483646);
      int dependencyStart1 = this.m_dependencyStarts[index1];
      int dependencyStart2 = this.m_dependencyStarts[index1 + 1];
      int num1;
      if (dependencyStart1 == -1 || dependencyStart2 == -1)
      {
        num1 = 0;
      }
      else
      {
        num1 = dependencyStart2 - dependencyStart1;
        for (int index2 = dependencyStart1; index2 < dependencyStart2; ++index2)
        {
          if (Interlocked.Increment(ref this.m_jobStates[this.m_dependencies[index2]]) != 2147483644)
            --num1;
        }
      }
      if (exception != null)
      {
        if (this.m_exceptionBuffer == null)
          Interlocked.CompareExchange<List<Exception>>(ref this.m_exceptionBuffer, new List<Exception>(), (List<Exception>) null);
        lock (this.m_exceptionBuffer)
          this.m_exceptionBuffer.Add(exception);
      }
      if (Interlocked.Increment(ref this.m_completedJobs) == this.m_jobCount)
        this.ReleaseMainThread();
      return num1;
    }

    private void RegisterJobsForConsumption(int count)
    {
      long comparand = this.m_scheduledJobsAndWorkers;
      int num1;
      while (true)
      {
        int num2 = (int) ((comparand & -65536L) >> 16);
        long num3 = comparand & (long) ushort.MaxValue;
        num1 = Math.Min(this.m_maxThreads, (int) num3 + count) - (int) num3;
        long num4 = num3 + (long) num1;
        long num5 = Interlocked.CompareExchange(ref this.m_scheduledJobsAndWorkers, (long) (num2 + count) << 16 | num4, comparand);
        if (num5 != comparand)
          comparand = num5;
        else
          break;
      }
      if (num1 <= 0)
        return;
      if (this.TryWakingUpMainThread())
        --num1;
      if (num1 <= 0)
        return;
      for (int index = 0; index < num1; ++index)
        Parallel.Start((IWork) this);
    }

    private bool TryAcquireJob()
    {
      long comparand = this.m_scheduledJobsAndWorkers;
      bool flag;
      while (true)
      {
        long num1 = comparand & (long) ushort.MaxValue;
        long num2 = comparand & -65536L;
        flag = (ulong) num2 > 0UL;
        if (flag)
          num2 = (num2 >> 16) - 1L << 16;
        else
          --num1;
        long num3 = Interlocked.CompareExchange(ref this.m_scheduledJobsAndWorkers, num2 | num1, comparand);
        if (num3 != comparand)
          comparand = num3;
        else
          break;
      }
      return flag;
    }

    public void Clear(int length)
    {
      this.m_jobCount = 0;
      this.m_completedJobs = 0;
      this.m_dependencyStarts[0] = 0;
      this.m_allCompletedCachedIndex = 0;
      if (this.m_exceptionBuffer != null)
        this.m_exceptionBuffer.Clear();
      for (int index = length - 1; index >= 0; --index)
      {
        this.m_jobs[index] = (Action) null;
        this.m_dependencyStarts[index + 1] = -1;
        this.m_jobStates[index] = 2147483644;
      }
    }

    private int GetLastInitializedStart(int maxIndex)
    {
      int dependencyStart;
      while (true)
      {
        dependencyStart = this.m_dependencyStarts[maxIndex];
        if (dependencyStart == -1)
          --maxIndex;
        else
          break;
      }
      return dependencyStart;
    }

    public DependencyBatch(WorkPriority priority = WorkPriority.Normal)
    {
      this.Priority = priority;
      this.m_completionAwaiter = new AutoResetEvent(false);
      WorkOptions defaultOptions = Parallel.DefaultOptions;
      defaultOptions.MaximumThreads = int.MaxValue;
      this.Options = defaultOptions.WithDebugInfo(MyProfiler.TaskType.Wait, "Batch");
    }

    public override void DoWork(WorkData workData = null) => this.WorkerLoop();

    private void EnsureCapacity(int size)
    {
      int length = this.m_jobs == null ? 0 : this.m_jobs.Length;
      if (length >= size)
        return;
      Action[] jobs = this.m_jobs;
      int[] jobStates = this.m_jobStates;
      int[] dependencies = this.m_dependencies;
      int[] dependencyStarts = this.m_dependencyStarts;
      this.AllocateInternal(this.m_jobs == null ? 50 : length * 2);
      if (jobs != null)
      {
        Array.Copy((Array) jobs, (Array) this.m_jobs, length);
        Array.Copy((Array) jobStates, (Array) this.m_jobStates, length);
        Array.Copy((Array) dependencyStarts, (Array) this.m_dependencyStarts, length + 1);
      }
      for (int index = length; index < this.m_jobStates.Length; ++index)
      {
        this.m_dependencyStarts[index + 1] = -1;
        this.m_jobStates[index] = 2147483644;
      }
      if (this.m_dependencies == dependencies || dependencies == null)
        return;
      Array.Copy((Array) dependencies, (Array) this.m_dependencies, dependencies.Length);
    }

    [Conditional("DEBUG")]
    private void AssertDependencyOrder(int currentJobId)
    {
      int num = currentJobId + 1;
      while (num < this.m_dependencyStarts.Length)
        ++num;
    }

    [Conditional("DEBUG")]
    private void AssertExecutionConsistency()
    {
      int num1 = this.m_dependencyStarts[0];
      for (int index = 1; index <= this.m_jobCount; ++index)
      {
        int dependencyStart = this.m_dependencyStarts[index];
        if (dependencyStart != -1)
          num1 = dependencyStart;
      }
      if (num1 <= 0)
        return;
      int num2 = 0;
      while (num2 < num1)
        ++num2;
    }

    public void Dispose() => this.m_completionAwaiter.Dispose();

    private static class JobState
    {
      public const int DependencyPending = 2147483643;
      public const int Scheduled = 2147483644;
      public const int Running = 2147483645;
      public const int Finished = 2147483646;
    }

    private static class ControlThreadState
    {
      public const int Waiting = 0;
      public const int Scheduled = 1;
      public const int Running = 2;
      public const int Exit = 3;
    }

    public struct StartToken : IDisposable
    {
      private readonly int m_jobId;
      private readonly DependencyBatch m_batch;
      private int m_writeOffset;

      public StartToken(int jobId, DependencyBatch batch)
      {
        this.m_jobId = jobId;
        this.m_batch = batch;
        this.m_writeOffset = batch.m_dependencyStarts[jobId];
      }

      public void Starts(int jobId)
      {
        int[] dependencies = this.m_batch.m_dependencies;
        int index = this.m_writeOffset++;
        if (dependencies.Length <= index)
        {
          Array.Resize<int>(ref dependencies, dependencies.Length * 2);
          this.m_batch.m_dependencies = dependencies;
        }
        dependencies[index] = jobId;
      }

      public void Dispose() => this.m_batch.m_dependencyStarts[this.m_jobId + 1] = this.m_writeOffset;
    }

    private class ParallelTasks_DependencyBatch\u003C\u003EActor
    {
    }
  }
}
