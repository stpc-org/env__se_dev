// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyClipmapTiming
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace VRage.Voxels.Clipmap
{
  public class MyClipmapTiming
  {
    [ThreadStatic]
    private static Stopwatch m_threadStopwatch;
    private static Dictionary<Thread, Stopwatch> m_stopwatches = new Dictionary<Thread, Stopwatch>();
    private static TimeSpan m_total;

    private static Stopwatch Stopwatch
    {
      get
      {
        if (MyClipmapTiming.m_threadStopwatch == null)
        {
          lock (MyClipmapTiming.m_stopwatches)
          {
            MyClipmapTiming.m_threadStopwatch = new Stopwatch();
            MyClipmapTiming.m_stopwatches[Thread.CurrentThread] = MyClipmapTiming.m_threadStopwatch;
          }
        }
        return MyClipmapTiming.m_threadStopwatch;
      }
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void StartTiming() => MyClipmapTiming.Stopwatch.Start();

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void StopTiming() => MyClipmapTiming.Stopwatch.Stop();

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void Reset()
    {
      lock (MyClipmapTiming.m_stopwatches)
      {
        foreach (Stopwatch stopwatch in MyClipmapTiming.m_stopwatches.Values)
          stopwatch.Reset();
      }
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private static void ReadTotal()
    {
      lock (MyClipmapTiming.m_stopwatches)
      {
        long ticks = 0;
        foreach (Stopwatch stopwatch in MyClipmapTiming.m_stopwatches.Values)
          ticks += stopwatch.ElapsedTicks;
        MyClipmapTiming.m_total = new TimeSpan(ticks);
      }
    }

    public static TimeSpan Total => MyClipmapTiming.m_total;
  }
}
