// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.MyPerformanceCounter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;

namespace Sandbox.Game.Debugging
{
  internal static class MyPerformanceCounter
  {
    private static Stopwatch m_timer = new Stopwatch();

    static MyPerformanceCounter() => MyPerformanceCounter.m_timer.Start();

    public static double TicksToMs(long ticks) => (double) ticks / (double) Stopwatch.Frequency * 1000.0;

    public static long ElapsedTicks => MyPerformanceCounter.m_timer.ElapsedTicks;

    private struct Timer
    {
      public static readonly MyPerformanceCounter.Timer Empty = new MyPerformanceCounter.Timer()
      {
        Runtime = 0,
        StartTime = long.MaxValue
      };
      public long StartTime;
      public long Runtime;

      public bool IsRunning => this.StartTime != long.MaxValue;
    }
  }
}
