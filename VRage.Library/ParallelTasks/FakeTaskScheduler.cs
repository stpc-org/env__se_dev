// Decompiled with JetBrains decompiler
// Type: ParallelTasks.FakeTaskScheduler
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage.Collections;

namespace ParallelTasks
{
  public class FakeTaskScheduler : IWorkScheduler
  {
    public int ThreadCount => 1;

    public void Schedule(Task item) => item.DoWork();

    public bool WaitForTasksToFinish(TimeSpan waitTimeout) => true;

    public void ScheduleOnEachWorker(Action action)
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
      workItem.PrepareStart((IWork) instance).DoWork();
    }

    public int ReadAndClearExecutionTime() => 0;

    public void SuspendThreads(TimeSpan waitTimeout) => throw new NotImplementedException();

    public void ResumeThreads() => throw new NotImplementedException();
  }
}
