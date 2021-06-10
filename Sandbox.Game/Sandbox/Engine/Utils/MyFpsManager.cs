// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyFpsManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Debugging;
using Sandbox.Game.World;
using System;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public static class MyFpsManager
  {
    private static long m_lastTime = 0;
    private static uint m_fpsCounter = 0;
    private static uint m_sessionTotalFrames = 0;
    private static uint m_maxSessionFPS = 0;
    private static uint m_minSessionFPS = (uint) int.MaxValue;
    private static uint m_lastFpsDrawn = 0;
    private static long m_lastFrameTime = 0;
    private static long m_lastFrameMin = long.MaxValue;
    private static long m_lastFrameMax = long.MinValue;
    private static byte m_firstFrames = 0;
    private static readonly MyMovingAverage m_frameTimeAvg = new MyMovingAverage(60);

    public static int GetFps() => (int) MyFpsManager.m_lastFpsDrawn;

    public static int GetSessionTotalFrames() => (int) MyFpsManager.m_sessionTotalFrames;

    public static int GetMaxSessionFPS() => (int) MyFpsManager.m_maxSessionFPS;

    public static int GetMinSessionFPS() => (int) MyFpsManager.m_minSessionFPS;

    public static float FrameTime { get; private set; }

    public static float FrameTimeAvg => MyFpsManager.m_frameTimeAvg.Avg;

    public static float FrameTimeMin { get; private set; }

    public static float FrameTimeMax { get; private set; }

    public static void Update()
    {
      ++MyFpsManager.m_fpsCounter;
      ++MyFpsManager.m_sessionTotalFrames;
      if (MySession.Static == null)
      {
        MyFpsManager.m_sessionTotalFrames = 0U;
        MyFpsManager.m_maxSessionFPS = 0U;
        MyFpsManager.m_minSessionFPS = (uint) int.MaxValue;
      }
      long ticks = MyPerformanceCounter.ElapsedTicks - MyFpsManager.m_lastFrameTime;
      MyFpsManager.FrameTime = (float) MyPerformanceCounter.TicksToMs(ticks);
      MyFpsManager.m_lastFrameTime = MyPerformanceCounter.ElapsedTicks;
      MyFpsManager.m_frameTimeAvg.Enqueue(MyFpsManager.FrameTime);
      if (ticks > MyFpsManager.m_lastFrameMax)
        MyFpsManager.m_lastFrameMax = ticks;
      if (ticks < MyFpsManager.m_lastFrameMin)
        MyFpsManager.m_lastFrameMin = ticks;
      if (MyPerformanceCounter.TicksToMs(MyPerformanceCounter.ElapsedTicks - MyFpsManager.m_lastTime) < 1000.0)
        return;
      MyFpsManager.FrameTimeMin = (float) MyPerformanceCounter.TicksToMs(MyFpsManager.m_lastFrameMin);
      MyFpsManager.FrameTimeMax = (float) MyPerformanceCounter.TicksToMs(MyFpsManager.m_lastFrameMax);
      MyFpsManager.m_lastFrameMin = long.MaxValue;
      MyFpsManager.m_lastFrameMax = long.MinValue;
      if (MySession.Static != null && MyFpsManager.m_firstFrames > (byte) 20)
      {
        MyFpsManager.m_minSessionFPS = Math.Min(MyFpsManager.m_minSessionFPS, MyFpsManager.m_fpsCounter);
        MyFpsManager.m_maxSessionFPS = Math.Max(MyFpsManager.m_maxSessionFPS, MyFpsManager.m_fpsCounter);
      }
      if (MyFpsManager.m_firstFrames <= (byte) 20)
        ++MyFpsManager.m_firstFrames;
      MyFpsManager.m_lastTime = MyPerformanceCounter.ElapsedTicks;
      MyFpsManager.m_lastFpsDrawn = MyFpsManager.m_fpsCounter;
      MyFpsManager.m_fpsCounter = 0U;
    }

    public static void Reset()
    {
      MyFpsManager.m_maxSessionFPS = 0U;
      MyFpsManager.m_minSessionFPS = (uint) int.MaxValue;
      MyFpsManager.m_fpsCounter = 0U;
      MyFpsManager.m_sessionTotalFrames = 0U;
      MyFpsManager.m_lastTime = MyPerformanceCounter.ElapsedTicks;
      MyFpsManager.m_firstFrames = (byte) 0;
    }

    public static void PrepareMinMax()
    {
      if (MyFpsManager.m_firstFrames > (byte) 20)
        return;
      MyFpsManager.m_minSessionFPS = MyFpsManager.m_lastFpsDrawn;
      MyFpsManager.m_maxSessionFPS = MyFpsManager.m_lastFpsDrawn;
    }
  }
}
