// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.FixedLoop
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using VRage.Library.Utils;
using VRage.Stats;

namespace Sandbox.Engine.Platform
{
  public class FixedLoop : GenericLoop
  {
    private static readonly MyGameTimer m_gameTimer = new MyGameTimer();
    public readonly MyStats StatGroup;
    public readonly string StatName;
    private readonly WaitForTargetFrameRate m_waiter = new WaitForTargetFrameRate(FixedLoop.m_gameTimer);

    public long TickPerFrame => this.m_waiter.TickPerFrame;

    public bool EnableMaxSpeed
    {
      get => this.m_waiter.EnableMaxSpeed;
      set => this.m_waiter.EnableMaxSpeed = value;
    }

    public FixedLoop(MyStats statGroup = null, string statName = null)
    {
      this.StatGroup = statGroup ?? new MyStats();
      this.StatName = statName ?? "WaitForUpdate";
    }

    public void SetNextFrameDelayDelta(float delta) => this.m_waiter.SetNextFrameDelayDelta(delta);

    public override void Run(GenericLoop.VoidAction tickCallback) => base.Run((GenericLoop.VoidAction) (() =>
    {
      using (this.StatGroup.Measure(this.StatName))
        this.m_waiter.Wait();
      tickCallback();
    }));
  }
}
