// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyPathfindingStopwatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;
using VRage.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
  internal static class MyPathfindingStopwatch
  {
    private static readonly Stopwatch m_stopWatch = new Stopwatch();
    private static readonly Stopwatch m_globalStopwatch = new Stopwatch();
    private static readonly MyLog m_log = new MyLog();
    private const int STOP_TIME_MS = 10000;
    private static int m_levelOfStarting;

    [Conditional("DEBUG")]
    public static void StartMeasuring()
    {
      MyPathfindingStopwatch.m_stopWatch.Reset();
      MyPathfindingStopwatch.m_globalStopwatch.Reset();
      MyPathfindingStopwatch.m_globalStopwatch.Start();
    }

    [Conditional("DEBUG")]
    public static void CheckStopMeasuring()
    {
      if (!MyPathfindingStopwatch.m_globalStopwatch.IsRunning)
        return;
      long elapsedMilliseconds = MyPathfindingStopwatch.m_globalStopwatch.ElapsedMilliseconds;
    }

    [Conditional("DEBUG")]
    public static void StopMeasuring()
    {
      MyPathfindingStopwatch.m_globalStopwatch.Stop();
      string msg = string.Format("pathfinding elapsed time: {0} ms / in {1} ms", (object) MyPathfindingStopwatch.m_stopWatch.ElapsedMilliseconds, (object) 10000);
      MyPathfindingStopwatch.m_log.WriteLineAndConsole(msg);
    }

    [Conditional("DEBUG")]
    public static void Start()
    {
      if (!MyPathfindingStopwatch.m_stopWatch.IsRunning)
      {
        MyPathfindingStopwatch.m_stopWatch.Start();
        MyPathfindingStopwatch.m_levelOfStarting = 1;
      }
      else
        ++MyPathfindingStopwatch.m_levelOfStarting;
    }

    [Conditional("DEBUG")]
    public static void Stop()
    {
      if (!MyPathfindingStopwatch.m_stopWatch.IsRunning)
        return;
      --MyPathfindingStopwatch.m_levelOfStarting;
      if (MyPathfindingStopwatch.m_levelOfStarting != 0)
        return;
      MyPathfindingStopwatch.m_stopWatch.Stop();
    }

    [Conditional("DEBUG")]
    public static void Reset() => MyPathfindingStopwatch.m_stopWatch.Reset();
  }
}
