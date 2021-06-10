// Decompiled with JetBrains decompiler
// Type: ParallelTasks.FutureWork`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
  internal class FutureWork<T> : AbstractWork, IPrioritizedWork, IWork
  {
    private static readonly MyConcurrentPool<FutureWork<T>> m_workPool = new MyConcurrentPool<FutureWork<T>>(10, expectedAllocations: 1000);
    internal int ID;

    internal Func<T> Function { get; set; }

    internal T Result { get; set; }

    public WorkPriority Priority { get; set; }

    public override void DoWork(WorkData workData = null) => this.Result = this.Function();

    public static FutureWork<T> GetInstance() => FutureWork<T>.m_workPool.Get();

    public void ReturnToPool()
    {
      if (this.ID == 0)
        return;
      this.Result = default (T);
      this.Function = (Func<T>) null;
      FutureWork<T>.m_workPool.Return(this);
    }

    private class ParallelTasks_FutureWork`1\u003C\u003EActor : IActivator, IActivator<FutureWork<T>>
    {
      object IActivator.CreateInstance() => (object) new FutureWork<T>();

      FutureWork<T> IActivator<FutureWork<T>>.CreateInstance() => new FutureWork<T>();
    }
  }
}
