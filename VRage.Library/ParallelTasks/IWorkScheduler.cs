// Decompiled with JetBrains decompiler
// Type: ParallelTasks.IWorkScheduler
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace ParallelTasks
{
  public interface IWorkScheduler
  {
    int ThreadCount { get; }

    void Schedule(Task item);

    bool WaitForTasksToFinish(TimeSpan waitTimeout);

    void ScheduleOnEachWorker(Action action);

    int ReadAndClearExecutionTime();

    void SuspendThreads(TimeSpan waitTimeout);

    void ResumeThreads();
  }
}
