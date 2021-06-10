// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGameStats
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game
{
  internal class MyGameStats
  {
    private DateTime m_lastStatMeasurePerSecond;
    private long m_previousUpdateCount;

    public static MyGameStats Static { get; private set; }

    public long UpdateCount { get; private set; }

    public long UpdatesPerSecond { get; private set; }

    static MyGameStats() => MyGameStats.Static = new MyGameStats();

    private MyGameStats()
    {
      this.m_previousUpdateCount = 0L;
      this.UpdateCount = 0L;
    }

    public void Update()
    {
      ++this.UpdateCount;
      if ((DateTime.UtcNow - this.m_lastStatMeasurePerSecond).TotalSeconds < 1.0)
        return;
      this.UpdatesPerSecond = this.UpdateCount - this.m_previousUpdateCount;
      this.m_previousUpdateCount = this.UpdateCount;
      this.m_lastStatMeasurePerSecond = DateTime.UtcNow;
    }
  }
}
