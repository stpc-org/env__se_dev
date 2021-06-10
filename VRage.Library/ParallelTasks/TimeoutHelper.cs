// Decompiled with JetBrains decompiler
// Type: ParallelTasks.TimeoutHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;

namespace ParallelTasks
{
  internal static class TimeoutHelper
  {
    public static long GetTime() => Stopwatch.GetTimestamp();

    public static int UpdateTimeOut(long startTime, int originalWaitMillisecondsTimeout)
    {
      long num1 = Stopwatch.Frequency / 1000L;
      long num2 = (TimeoutHelper.GetTime() - startTime) / num1;
      return (int) ((long) originalWaitMillisecondsTimeout - num2);
    }
  }
}
