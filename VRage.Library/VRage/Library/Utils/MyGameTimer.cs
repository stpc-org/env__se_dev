// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyGameTimer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;

namespace VRage.Library.Utils
{
  public class MyGameTimer
  {
    private long m_startTicks;
    private long m_elapsedTicks;
    public static readonly long Frequency = Stopwatch.Frequency;

    public TimeSpan ElapsedTimeSpan => this.Elapsed.TimeSpan;

    public long ElapsedTicks => this.m_elapsedTicks + (Stopwatch.GetTimestamp() - this.m_startTicks);

    public MyTimeSpan Elapsed => new MyTimeSpan(this.ElapsedTicks);

    public void AddElapsed(MyTimeSpan timespan) => this.m_startTicks -= timespan.Ticks;

    public MyGameTimer()
    {
      this.m_startTicks = Stopwatch.GetTimestamp();
      this.m_elapsedTicks = 0L;
    }
  }
}
