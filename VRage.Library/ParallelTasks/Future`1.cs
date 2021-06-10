// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Future`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Threading;

namespace ParallelTasks
{
  public struct Future<T>
  {
    private Task task;
    private FutureWork<T> work;
    private int id;

    public bool IsComplete => this.task.IsComplete;

    public Exception[] Exceptions => this.task.Exceptions;

    internal Future(Task task, FutureWork<T> work)
    {
      this.task = task;
      this.work = work;
      this.id = work.ID;
    }

    public T GetResult()
    {
      if (this.work == null || Interlocked.CompareExchange(ref this.work.ID, this.id + 1, this.id) != this.id)
        throw new InvalidOperationException("The result of a future can only be retrieved once.");
      this.task.WaitOrExecute();
      T result = this.work.Result;
      this.work.ReturnToPool();
      this.work = (FutureWork<T>) null;
      return result;
    }
  }
}
