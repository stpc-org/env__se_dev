// Decompiled with JetBrains decompiler
// Type: ParallelTasks.ForLoopWork
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Threading;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
  internal class ForLoopWork : AbstractWork, IPrioritizedWork, IWork
  {
    private static MyConcurrentPool<ForLoopWork> pool = new MyConcurrentPool<ForLoopWork>(10);
    private int index;
    private int length;
    private int stride;
    private Action<int> action;

    public WorkPriority Priority { get; private set; }

    public void Prepare(
      Action<int> action,
      int startInclusive,
      int endExclusive,
      int stride,
      WorkPriority priority)
    {
      this.action = action;
      this.index = startInclusive;
      this.length = endExclusive;
      this.stride = stride;
      this.Priority = priority;
    }

    public override void DoWork(WorkData workData = null)
    {
      int num1;
      while ((num1 = this.IncrementIndex()) < this.length)
      {
        int num2 = Math.Min(num1 + this.stride, this.length);
        for (int index = num1; index < num2; ++index)
          this.action(index);
      }
    }

    private int IncrementIndex() => Interlocked.Add(ref this.index, this.stride) - this.stride;

    protected override void FillDebugInfo(ref WorkOptions info) => this.FillDebugInfo(ref info, this.action.Method.Name);

    public static ForLoopWork Get() => ForLoopWork.pool.Get();

    public void Return()
    {
      this.action = (Action<int>) null;
      ForLoopWork.pool.Return(this);
    }

    private class ParallelTasks_ForLoopWork\u003C\u003EActor : IActivator, IActivator<ForLoopWork>
    {
      object IActivator.CreateInstance() => (object) new ForLoopWork();

      ForLoopWork IActivator<ForLoopWork>.CreateInstance() => new ForLoopWork();
    }
  }
}
