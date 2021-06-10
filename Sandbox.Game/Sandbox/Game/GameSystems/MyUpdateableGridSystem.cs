// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyUpdateableGridSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;

namespace Sandbox.Game.GameSystems
{
  public abstract class MyUpdateableGridSystem
  {
    private bool m_scheduled;

    protected MyCubeGrid Grid { get; private set; }

    public abstract MyCubeGrid.UpdateQueue Queue { get; }

    public virtual int UpdatePriority => int.MaxValue;

    public virtual bool UpdateInParallel => true;

    protected MyUpdateableGridSystem(MyCubeGrid grid) => this.Grid = grid;

    protected abstract void Update();

    protected void Schedule()
    {
      if (this.m_scheduled)
        return;
      this.Grid.Schedule(this.Queue, new Action(this.Update), this.UpdatePriority);
      if (this.Queue.IsExecutedOnce())
        return;
      this.m_scheduled = true;
    }

    protected void DeSchedule()
    {
      this.Grid.DeSchedule(this.Queue, new Action(this.Update));
      this.m_scheduled = false;
    }
  }
}
