// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.WaitForTargetFrameRate
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Threading;

namespace VRage.Library.Utils
{
  public class WaitForTargetFrameRate
  {
    private long m_targetTicks;
    public bool EnableMaxSpeed;
    private const bool EnableUpdateWait = true;
    private readonly MyGameTimer m_timer;
    private readonly float m_targetFrequency;
    private float m_delta;

    public long TickPerFrame => (long) (int) Math.Round((double) MyGameTimer.Frequency / (double) this.m_targetFrequency);

    public WaitForTargetFrameRate(MyGameTimer timer, float targetFrequency = 60f)
    {
      this.m_timer = timer;
      this.m_targetFrequency = targetFrequency;
    }

    public void SetNextFrameDelayDelta(float delta) => this.m_delta = delta;

    public void Wait()
    {
      this.m_timer.AddElapsed(MyTimeSpan.FromMilliseconds(-(double) this.m_delta));
      long elapsedTicks = this.m_timer.ElapsedTicks;
      this.m_targetTicks += this.TickPerFrame;
      if (elapsedTicks > this.m_targetTicks + this.TickPerFrame * 5L || this.EnableMaxSpeed)
      {
        this.m_targetTicks = elapsedTicks;
      }
      else
      {
        int millisecondsTimeout = (int) (MyTimeSpan.FromTicks(this.m_targetTicks - elapsedTicks).Milliseconds - 0.1);
        if (millisecondsTimeout > 0)
          Thread.CurrentThread.Join(millisecondsTimeout);
        if (this.m_targetTicks < this.m_timer.ElapsedTicks + this.TickPerFrame + this.TickPerFrame / 4L)
        {
          while (this.m_timer.ElapsedTicks < this.m_targetTicks)
            ;
        }
        else
          this.m_targetTicks = this.m_timer.ElapsedTicks;
      }
      this.m_delta = 0.0f;
    }
  }
}
