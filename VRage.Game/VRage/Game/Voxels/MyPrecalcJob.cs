// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.MyPrecalcJob
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ParallelTasks;
using System;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Voxels
{
  [GenerateActivator]
  public abstract class MyPrecalcJob
  {
    public readonly Action OnCompleteDelegate;
    public bool IsValid;
    public volatile bool Started;

    public virtual bool IsCanceled => false;

    protected MyPrecalcJob(bool enableCompletionCallback)
    {
      if (!enableCompletionCallback)
        return;
      this.OnCompleteDelegate = new Action(this.OnComplete);
    }

    public void DoWorkInternal()
    {
      this.Started = true;
      this.DoWork();
    }

    public abstract void DoWork();

    public abstract void Cancel();

    protected virtual void OnComplete()
    {
    }

    public WorkOptions Options => Parallel.DefaultOptions;

    public virtual int Priority => 0;

    public virtual void DebugDraw(Color c)
    {
    }
  }
}
