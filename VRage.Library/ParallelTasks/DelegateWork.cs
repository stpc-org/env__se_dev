// Decompiled with JetBrains decompiler
// Type: ParallelTasks.DelegateWork
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage.Collections;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
  internal class DelegateWork : AbstractWork, IPrioritizedWork, IWork
  {
    private static MyConcurrentPool<DelegateWork> instances = new MyConcurrentPool<DelegateWork>(100, expectedAllocations: 100000);

    public Action Action { get; set; }

    public Action<WorkData> DataAction { get; set; }

    public WorkPriority Priority { get; set; } = WorkPriority.Normal;

    public override WorkOptions Options
    {
      get => base.Options;
      set => base.Options = value.MaximumThreads == 1 ? value : throw new Exception("WorkOptions.MaximumThreads must be 1 for delegate work");
    }

    public override void DoWork(WorkData workData = null)
    {
      try
      {
        if (this.Action != null)
        {
          this.Action();
          this.Action = (Action) null;
        }
        if (this.DataAction == null)
          return;
        this.DataAction(workData);
        this.DataAction = (Action<WorkData>) null;
      }
      finally
      {
        this.Action = (Action) null;
        this.DataAction = (Action<WorkData>) null;
        DelegateWork.instances.Return(this);
      }
    }

    internal static DelegateWork GetInstance() => DelegateWork.instances.Get();

    protected override void FillDebugInfo(ref WorkOptions info)
    {
      if (info.DebugName == null)
        info.DebugName = this.Action == null ? (this.DataAction == null ? string.Empty : this.DataAction.Method.Name) : this.Action.Method.Name;
      if (info.TaskType != MyProfiler.TaskType.None)
        return;
      info.TaskType = MyProfiler.TaskType.WorkItem;
    }

    private class ParallelTasks_DelegateWork\u003C\u003EActor : IActivator, IActivator<DelegateWork>
    {
      object IActivator.CreateInstance() => (object) new DelegateWork();

      DelegateWork IActivator<DelegateWork>.CreateInstance() => new DelegateWork();
    }
  }
}
