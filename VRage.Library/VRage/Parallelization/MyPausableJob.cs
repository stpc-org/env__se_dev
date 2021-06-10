// Decompiled with JetBrains decompiler
// Type: VRage.Parallelization.MyPausableJob
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

namespace VRage.Parallelization
{
  public class MyPausableJob
  {
    private volatile bool m_pause;
    private AutoResetEvent m_pausedEvent = new AutoResetEvent(false);
    private AutoResetEvent m_resumedEvent = new AutoResetEvent(false);

    public bool IsPaused => this.m_pause;

    public void Pause()
    {
      this.m_pause = true;
      this.m_pausedEvent.WaitOne();
    }

    public void Resume()
    {
      this.m_pause = false;
      this.m_resumedEvent.Set();
    }

    public void AllowPauseHere()
    {
      if (!this.m_pause)
        return;
      this.m_pausedEvent.Set();
      this.m_resumedEvent.WaitOne();
    }
  }
}
