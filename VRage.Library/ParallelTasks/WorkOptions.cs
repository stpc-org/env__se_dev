// Decompiled with JetBrains decompiler
// Type: ParallelTasks.WorkOptions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using VRage.Profiler;

namespace ParallelTasks
{
  public struct WorkOptions
  {
    public int MaximumThreads { get; set; }

    public bool QueueFIFO { get; set; }

    public string DebugName { get; set; }

    public MyProfiler.TaskType TaskType { get; set; }

    public WorkOptions WithDebugInfo(MyProfiler.TaskType taskType, string debugName = null)
    {
      WorkOptions workOptions = this;
      workOptions.TaskType = taskType;
      workOptions.DebugName = debugName;
      return workOptions;
    }

    public WorkOptions WithMaxThreads(int maxThreads)
    {
      WorkOptions workOptions = this;
      workOptions.MaximumThreads = maxThreads;
      return workOptions;
    }

    public override string ToString() => string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}", (object) "MaximumThreads", (object) this.MaximumThreads, (object) "QueueFIFO", (object) this.QueueFIFO, (object) "DebugName", (object) this.DebugName, (object) "TaskType", (object) this.TaskType);
  }
}
