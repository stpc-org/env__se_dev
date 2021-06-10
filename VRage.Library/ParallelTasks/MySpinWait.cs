// Decompiled with JetBrains decompiler
// Type: ParallelTasks.MySpinWait
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Threading;

namespace ParallelTasks
{
  public struct MySpinWait
  {
    internal const int YIELD_THRESHOLD = 10;
    internal const int SLEEP_0_EVERY_HOW_MANY_TIMES = 5;
    internal const int SLEEP_1_EVERY_HOW_MANY_MS = 10;
    internal const int SLEEP_LONG_EVERY_HOW_MANY_MS = 100;
    private int m_count;
    private long m_startTime;
    private long m_startTimeLong;

    public int Count => this.m_count;

    public bool NextSpinWillYield => this.m_count > 10 || PlatformHelper.IsSingleProcessor;

    public void SpinOnce()
    {
      if (this.NextSpinWillYield)
      {
        if (this.m_startTime == 0L)
        {
          this.m_startTime = TimeoutHelper.GetTime();
          this.m_startTimeLong = TimeoutHelper.GetTime();
        }
        int num = this.m_count >= 10 ? this.m_count - 10 : this.m_count;
        if (TimeoutHelper.UpdateTimeOut(this.m_startTimeLong, 100) <= 0)
        {
          Thread.Sleep(13);
          this.m_startTime = this.m_startTimeLong = TimeoutHelper.GetTime();
        }
        else if (TimeoutHelper.UpdateTimeOut(this.m_startTime, 10) <= 0)
        {
          Thread.Sleep(1);
          this.m_startTime = TimeoutHelper.GetTime();
        }
        else if (num % 5 == 4)
          Thread.Sleep(0);
        else
          Thread.Yield();
      }
      else
        Thread.SpinWait(4 << this.m_count);
      this.m_count = this.m_count == int.MaxValue ? 10 : this.m_count + 1;
    }

    public void Reset() => this.m_count = 0;

    public static void SpinUntil(Func<bool> condition) => MySpinWait.SpinUntil(condition, -1);

    public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, "SpinWait_SpinUntil_TimeoutWrong");
      return MySpinWait.SpinUntil(condition, (int) timeout.TotalMilliseconds);
    }

    public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, "SpinWait_SpinUntil_TimeoutWrong");
      if (condition == null)
        throw new ArgumentNullException(nameof (condition), "SpinWait_SpinUntil_ArgumentNull");
      long startTime = 0;
      if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
        startTime = TimeoutHelper.GetTime();
      MySpinWait mySpinWait = new MySpinWait();
      while (!condition())
      {
        if (millisecondsTimeout == 0)
          return false;
        mySpinWait.SpinOnce();
        if (millisecondsTimeout != -1 && mySpinWait.NextSpinWillYield && TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout) <= 0)
          return false;
      }
      return true;
    }
  }
}
