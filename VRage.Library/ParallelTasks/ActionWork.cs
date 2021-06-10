// Decompiled with JetBrains decompiler
// Type: ParallelTasks.ActionWork
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace ParallelTasks
{
  public class ActionWork : AbstractWork
  {
    public readonly Action<WorkData> _Action;

    public ActionWork(Action<WorkData> action)
      : this(action, Parallel.DefaultOptions)
    {
    }

    public ActionWork(Action<WorkData> action, WorkOptions options)
    {
      this._Action = action;
      this.Options = options;
    }

    public override void DoWork(WorkData workData = null) => this._Action(workData);

    private class ParallelTasks_ActionWork\u003C\u003EActor
    {
    }
  }
}
