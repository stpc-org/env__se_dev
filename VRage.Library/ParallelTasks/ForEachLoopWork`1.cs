// Decompiled with JetBrains decompiler
// Type: ParallelTasks.ForEachLoopWork`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
  internal class ForEachLoopWork<T> : AbstractWork, IPrioritizedWork, IWork
  {
    private static MyConcurrentPool<ForEachLoopWork<T>> pool = new MyConcurrentPool<ForEachLoopWork<T>>(10);
    private bool done;
    private object syncLock;
    private Action<T> action;
    private IEnumerator<T> enumerator;

    public WorkPriority Priority { get; private set; }

    public ForEachLoopWork() => this.syncLock = new object();

    public void Prepare(Action<T> action, IEnumerator<T> enumerator, WorkPriority priority)
    {
      this.done = false;
      this.action = action;
      this.Priority = priority;
      this.enumerator = enumerator;
    }

    public override void DoWork(WorkData workData = null)
    {
      while (!this.done)
      {
        T current;
        lock (this.syncLock)
        {
          if (this.done)
            break;
          this.done = !this.enumerator.MoveNext();
          if (this.done)
            break;
          current = this.enumerator.Current;
        }
        this.action(current);
      }
    }

    protected override void FillDebugInfo(ref WorkOptions info) => this.FillDebugInfo(ref info, this.action.Method.Name);

    public static ForEachLoopWork<T> Get() => ForEachLoopWork<T>.pool.Get();

    public void Return()
    {
      this.enumerator = (IEnumerator<T>) null;
      this.action = (Action<T>) null;
      ForEachLoopWork<T>.pool.Return(this);
    }

    private class ParallelTasks_ForEachLoopWork`1\u003C\u003EActor : IActivator, IActivator<ForEachLoopWork<T>>
    {
      object IActivator.CreateInstance() => (object) new ForEachLoopWork<T>();

      ForEachLoopWork<T> IActivator<ForEachLoopWork<T>>.CreateInstance() => new ForEachLoopWork<T>();
    }
  }
}
