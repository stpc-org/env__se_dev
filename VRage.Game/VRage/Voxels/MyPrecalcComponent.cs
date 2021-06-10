// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyPrecalcComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Library.Threading;
using VRage.Profiler;
using VRage.Voxels.DualContouring;

namespace VRage.Voxels
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyPrecalcComponent : MySessionComponentBase
  {
    private static bool MULTITHREADED = true;
    private static Type m_isoMesherType = typeof (MyDualContouringMesher);
    public static long MaxPrecalcTime = 20;
    public static bool DebugDrawSorted = false;
    private static MyPrecalcComponent m_instance;
    private static SpinLockRef m_queueLock = new SpinLockRef();
    [ThreadStatic]
    private static IMyIsoMesher m_isoMesher;
    public static int UpdateThreadManagedId;
    private static readonly MyPrecalcComponent.MyPrecalcJobComparer m_comparer = new MyPrecalcComponent.MyPrecalcJobComparer();
    private readonly MyConcurrentQueue<MyPrecalcJob> m_workQueue = new MyConcurrentQueue<MyPrecalcJob>();
    private readonly MyConcurrentList<MyPrecalcJob> m_finishedJobs = new MyConcurrentList<MyPrecalcJob>();
    private MyDynamicObjectPool<MyPrecalcComponent.Work> m_workPool;
    private volatile int m_activeWorkers;

    public static Type IsoMesherType
    {
      get => MyPrecalcComponent.m_isoMesherType;
      set
      {
        if (!typeof (IMyIsoMesher).IsAssignableFrom(MyPrecalcComponent.m_isoMesherType))
          return;
        MyPrecalcComponent.m_isoMesherType = value;
      }
    }

    public static IMyIsoMesher IsoMesher => MyPrecalcComponent.m_isoMesher ?? (MyPrecalcComponent.m_isoMesher = (IMyIsoMesher) Activator.CreateInstance(MyPrecalcComponent.IsoMesherType));

    public static int InvalidatedRangeInflate => MyPrecalcComponent.IsoMesher.InvalidatedRangeInflate;

    public MyPrecalcComponent() => this.UpdateOnPause = true;

    public static bool EnqueueBack(MyPrecalcJob job)
    {
      using (MyPrecalcComponent.m_queueLock.Acquire())
      {
        if (MyPrecalcComponent.m_instance == null)
          return false;
        MyPrecalcComponent.m_instance.Enqueue(job);
        return true;
      }
    }

    [Conditional("DEBUG")]
    public static void AssertUpdateThread()
    {
    }

    public override void LoadData()
    {
      base.LoadData();
      MyPrecalcComponent.m_instance = this;
      if (!MyPrecalcComponent.MULTITHREADED)
        MyPrecalcComponent.MaxPrecalcTime = 6L;
      this.m_workPool = new MyDynamicObjectPool<MyPrecalcComponent.Work>(Parallel.Scheduler.ThreadCount);
    }

    protected override void UnloadData()
    {
      using (MyPrecalcComponent.m_queueLock.Acquire())
      {
        base.UnloadData();
        MyPrecalcComponent.m_instance = (MyPrecalcComponent) null;
      }
      while (this.m_activeWorkers > 0)
        Thread.Yield();
      foreach (MyPrecalcJob myPrecalcJob in this.m_finishedJobs.Concat<MyPrecalcJob>((IEnumerable<MyPrecalcJob>) this.m_workQueue))
      {
        myPrecalcJob.Cancel();
        if (myPrecalcJob.OnCompleteDelegate != null)
          myPrecalcJob.OnCompleteDelegate();
      }
      this.m_finishedJobs.Clear();
      this.m_workQueue.Clear();
    }

    private void Enqueue(MyPrecalcJob job)
    {
      job.Started = false;
      this.m_workQueue.Enqueue(job);
    }

    private bool TryDequeue(out MyPrecalcJob job) => this.m_workQueue.TryDequeue(out job);

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.UpdateQueue();
    }

    public override bool UpdatedBeforeInit() => true;

    public void UpdateQueue()
    {
      bool flag1 = false;
      MyPrecalcJob instance1 = (MyPrecalcJob) null;
      while (this.m_workQueue.TryPeek(out instance1))
      {
        if (instance1.IsCanceled)
        {
          bool flag2 = false;
          MyPrecalcJob instance2;
          if (this.m_workQueue.TryDequeue(out instance2))
            flag2 = instance1 == instance2;
          if (flag2)
          {
            if (instance1.OnCompleteDelegate != null)
              this.m_finishedJobs.Add(instance1);
          }
          else
          {
            flag1 = false;
            if (instance2 != null)
              this.m_workQueue.Enqueue(instance2);
          }
        }
        else
        {
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        while (this.m_workPool.Count > 0)
        {
          MyPrecalcComponent.Work work = this.m_workPool.Allocate();
          work.Parent = this;
          work.Priority = WorkPriority.Low;
          work.MaxPrecalcTime = MyPrecalcComponent.MaxPrecalcTime * 10000L;
          Interlocked.Increment(ref this.m_activeWorkers);
          if (MyPrecalcComponent.MULTITHREADED)
          {
            Parallel.Start((IWork) work, work.CompletionCallback);
          }
          else
          {
            ((IWork) work).DoWork();
            work.CompletionCallback();
          }
        }
      }
      while (this.m_finishedJobs.TryDequeueBack(out instance1))
        instance1.OnCompleteDelegate();
    }

    private class MyPrecalcJobComparer : IComparer<MyPrecalcJob>
    {
      public int Compare(MyPrecalcJob x, MyPrecalcJob y) => y.Priority.CompareTo(x.Priority);
    }

    private class Work : IPrioritizedWork, IWork
    {
      private readonly List<MyPrecalcJob> m_finishedList = new List<MyPrecalcJob>();
      private readonly Stopwatch m_timer = new Stopwatch();
      public long MaxPrecalcTime;
      public readonly Action CompletionCallback;
      public MyPrecalcComponent Parent;

      public Work() => this.CompletionCallback = new Action(this.OnComplete);

      private void OnComplete()
      {
        this.Parent.m_workPool.Deallocate(this);
        this.Parent = (MyPrecalcComponent) null;
      }

      public WorkPriority Priority { get; set; }

      void IWork.DoWork(WorkData workData)
      {
        this.m_timer.Start();
        try
        {
          MyPrecalcJob job;
          while (this.Parent.Loaded && this.Parent.TryDequeue(out job))
          {
            if (job.IsCanceled && job.OnCompleteDelegate != null)
            {
              this.m_finishedList.Add(job);
            }
            else
            {
              job.DoWorkInternal();
              if (job.OnCompleteDelegate != null)
                this.m_finishedList.Add(job);
              if (this.m_timer.ElapsedTicks >= this.MaxPrecalcTime)
                break;
            }
          }
          this.Parent.m_finishedJobs.AddRange((IEnumerable<MyPrecalcJob>) this.m_finishedList);
          this.m_finishedList.Clear();
        }
        finally
        {
          Interlocked.Decrement(ref this.Parent.m_activeWorkers);
        }
        this.m_timer.Reset();
      }

      WorkOptions IWork.Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Precalc, "Precalc");

      public bool ShouldRequeue
      {
        get
        {
          if (!this.Parent.Loaded || this.Parent.m_workQueue.Count <= 0)
            return false;
          Interlocked.Increment(ref this.Parent.m_activeWorkers);
          return true;
        }
      }
    }
  }
}
