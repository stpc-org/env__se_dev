// Decompiled with JetBrains decompiler
// Type: ParallelTasks.WorkStealingScheduler
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Library;

namespace ParallelTasks
{
  public class WorkStealingScheduler : IWorkScheduler
  {
    private Queue<Task> tasks;
    private FastResourceLock tasksLock;

    internal List<Worker> Workers { get; private set; }

    public int ThreadCount => this.Workers.Count;

    public WorkStealingScheduler()
      : this(MyEnvironment.ProcessorCount, ThreadPriority.BelowNormal)
    {
    }

    public WorkStealingScheduler(int numThreads, ThreadPriority priority)
    {
      this.tasks = new Queue<Task>();
      this.tasksLock = new FastResourceLock();
      this.Workers = new List<Worker>(numThreads);
      for (int index = 0; index < numThreads; ++index)
        this.Workers.Add(new Worker(this, index, priority));
      for (int index = 0; index < numThreads; ++index)
        this.Workers[index].Start();
    }

    internal bool TryGetTask(out Task task)
    {
      if (this.tasks.Count == 0)
      {
        task = new Task();
        return false;
      }
      using (this.tasksLock.AcquireExclusiveUsing())
      {
        if (this.tasks.Count > 0)
        {
          task = this.tasks.Dequeue();
          return true;
        }
        task = new Task();
        return false;
      }
    }

    public void Schedule(Task task)
    {
      if (task.Item.Work == null)
        return;
      int maximumThreads = task.Item.Work.Options.MaximumThreads;
      Worker currentWorker = Worker.CurrentWorker;
      if (!task.Item.Work.Options.QueueFIFO && currentWorker != null)
      {
        currentWorker.AddWork(task);
      }
      else
      {
        using (this.tasksLock.AcquireExclusiveUsing())
          this.tasks.Enqueue(task);
      }
      for (int index = 0; index < this.Workers.Count; ++index)
        this.Workers[index].Gate.Set();
    }

    public bool WaitForTasksToFinish(TimeSpan waitTimeout) => Parallel.WaitForAll((WaitHandle[]) this.Workers.Select<Worker, ManualResetEvent>((Func<Worker, ManualResetEvent>) (s => s.HasNoWork)).ToArray<ManualResetEvent>(), waitTimeout);

    public void ScheduleOnEachWorker(Action action)
    {
      foreach (Worker worker in this.Workers)
      {
        DelegateWork instance = DelegateWork.GetInstance();
        instance.Action = action;
        instance.Options = new WorkOptions()
        {
          MaximumThreads = 1,
          QueueFIFO = false
        };
        WorkItem workItem = WorkItem.Get();
        workItem.CompletionCallbacks = (ConcurrentCachingList<WorkItem>) null;
        workItem.Callback = (Action) null;
        workItem.WorkData = (WorkData) null;
        Task task = workItem.PrepareStart((IWork) instance);
        worker.AddWork(task);
        worker.Gate.Set();
        task.Wait();
      }
    }

    public int ReadAndClearExecutionTime() => throw new NotImplementedException();

    public void SuspendThreads(TimeSpan waitTimeout) => throw new NotImplementedException();

    public void ResumeThreads() => throw new NotImplementedException();
  }
}
