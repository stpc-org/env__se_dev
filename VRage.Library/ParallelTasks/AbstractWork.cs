// Decompiled with JetBrains decompiler
// Type: ParallelTasks.AbstractWork
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
  [GenerateActivator]
  public abstract class AbstractWork : IWork
  {
    private WorkOptions m_options;
    private string m_cachedDebugName;

    public virtual WorkOptions Options
    {
      get => this.m_options;
      set => this.m_options = value;
    }

    public abstract void DoWork(WorkData workData = null);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void FillDebugInfoInternal(ref WorkOptions info) => this.FillDebugInfo(ref info);

    protected virtual void FillDebugInfo(ref WorkOptions info)
    {
      if (this.m_cachedDebugName == null)
        this.m_cachedDebugName = this.GetType().Name;
      this.FillDebugInfo(ref info, this.m_cachedDebugName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void FillDebugInfo(
      ref WorkOptions info,
      string debugName,
      MyProfiler.TaskType taskType = MyProfiler.TaskType.WorkItem)
    {
      if (info.DebugName == null)
        info.DebugName = debugName;
      if (info.TaskType != MyProfiler.TaskType.None)
        return;
      info.TaskType = taskType;
    }
  }
}
