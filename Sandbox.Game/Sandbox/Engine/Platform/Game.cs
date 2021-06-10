// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.Game
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Engine.Platform
{
  public abstract class Game
  {
    public static bool IsDedicated;
    public static bool IsPirated;
    public static bool IgnoreLastSession;
    public static string ConnectToServer;
    public static bool EnableSimSpeedLocking;
    [Obsolete("Remove asap, it is here only because of main menu music..")]
    protected readonly MyGameTimer m_gameTimer = new MyGameTimer();
    private MyTimeSpan m_drawTime;
    private MyTimeSpan m_totalTime;
    private ulong m_updateCounter;
    private MyTimeSpan m_simulationTimeWithSpeed;
    public const double TARGET_MS_PER_FRAME = 16.6666666666667;
    private const int NUM_FRAMES_FOR_DROP = 5;
    private const float NUM_MS_TO_INCREASE = 2000f;
    private const float PEAK_TRESHOLD_RATIO = 0.4f;
    private const float RATIO_TO_INCREASE_INSTANTLY = 0.25f;
    private float m_currentFrameIncreaseTime;
    private long m_currentMin;
    private long m_targetTicks;
    private MyQueue<long> m_lastFrameTiming = new MyQueue<long>(5);
    private bool isFirstUpdateDone;
    private bool isMouseVisible;
    public long FrameTimeTicks;
    private static long m_lastFrameTime;
    private static float m_targetMs;
    private readonly FixedLoop m_renderLoop = new FixedLoop(Stats.Generic, "WaitForUpdate");

    public MyTimeSpan DrawTime => this.m_drawTime;

    public MyTimeSpan TotalTime => this.m_totalTime;

    public ulong SimulationFrameCounter => this.m_updateCounter;

    public MyTimeSpan SimulationTime => MyTimeSpan.FromMilliseconds((double) this.m_updateCounter * (50.0 / 3.0));

    public MyTimeSpan SimulationTimeWithSpeed => this.m_simulationTimeWithSpeed;

    public Thread UpdateThread { get; protected set; }

    public Thread DrawThread { get; protected set; }

    public float CPULoad { get; private set; }

    public float CPULoadSmooth { get; private set; }

    public float CPUTimeSmooth { get; private set; }

    public float ThreadLoad { get; private set; }

    public float ThreadLoadSmooth { get; private set; }

    public float ThreadTimeSmooth { get; private set; }

    public static float SimulationRatio => 16.66667f / Sandbox.Engine.Platform.Game.m_targetMs;

    public Game()
    {
      this.IsActive = true;
      this.CPULoadSmooth = 1f;
    }

    public event Action OnGameExit;

    public bool IsActive { get; private set; }

    public bool IsRunning { get; private set; }

    public bool IsFirstUpdateDone => this.isFirstUpdateDone;

    public bool EnableMaxSpeed
    {
      get => this.m_renderLoop.EnableMaxSpeed;
      set => this.m_renderLoop.EnableMaxSpeed = value;
    }

    public bool Exiting => this.m_renderLoop.IsDone;

    public void SetNextFrameDelayDelta(float delta) => this.m_renderLoop.SetNextFrameDelayDelta(delta);

    public void Exit()
    {
      Action onGameExit = this.OnGameExit;
      if (onGameExit != null)
        onGameExit();
      this.m_renderLoop.IsDone = true;
    }

    protected void RunLoop()
    {
      try
      {
        this.m_targetTicks = this.m_renderLoop.TickPerFrame;
        MyLog.Default.WriteLine("Timer Frequency: " + (object) MyGameTimer.Frequency);
        MyLog.Default.WriteLine("Ticks per frame: " + (object) this.m_renderLoop.TickPerFrame);
        this.m_renderLoop.Run(new GenericLoop.VoidAction(this.RunSingleFrame));
      }
      catch (SEHException ex)
      {
        MyLog.Default.WriteLine("SEHException caught. Error code: " + ex.ErrorCode.ToString());
        throw ex;
      }
    }

    public void RunSingleFrame()
    {
      int num = this.IsFirstUpdateDone ? 1 : 0;
      long elapsedTicks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks;
      this.UpdateInternal();
      this.FrameTimeTicks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - elapsedTicks;
      float seconds1 = (float) new MyTimeSpan(this.FrameTimeTicks).Seconds;
      this.CPULoad = (float) ((double) seconds1 / 0.0166666675359011 * 100.0);
      this.CPULoadSmooth = MathHelper.Smooth(this.CPULoad, this.CPULoadSmooth);
      this.CPUTimeSmooth = MathHelper.Smooth(seconds1 * 1000f, this.CPUTimeSmooth);
      float seconds2 = (float) new MyTimeSpan((long) Parallel.Scheduler.ReadAndClearExecutionTime()).Seconds;
      this.ThreadLoad = (float) ((double) seconds2 / 0.0166666675359011 * 100.0);
      this.ThreadLoadSmooth = MathHelper.Smooth(this.ThreadLoad, this.ThreadLoadSmooth);
      this.ThreadTimeSmooth = MathHelper.Smooth(seconds2 * 1000f, this.ThreadTimeSmooth);
      if (MyFakes.PRECISE_SIM_SPEED)
        Sandbox.Engine.Platform.Game.m_targetMs = (float) Math.Max(50.0 / 3.0, Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(Math.Min(Math.Max(this.m_renderLoop.TickPerFrame, this.UpdateCurrentFrame()), 10L * this.m_renderLoop.TickPerFrame)));
      if (!Sandbox.Engine.Platform.Game.EnableSimSpeedLocking || !MyFakes.ENABLE_SIMSPEED_LOCKING)
        return;
      this.Lock(elapsedTicks);
    }

    private void Lock(long beforeUpdate)
    {
      long val1 = Math.Min(Math.Max(this.m_renderLoop.TickPerFrame, this.UpdateCurrentFrame()), 10L * this.m_renderLoop.TickPerFrame);
      this.m_currentMin = Math.Max(val1, this.m_currentMin);
      this.m_currentFrameIncreaseTime += Sandbox.Engine.Platform.Game.m_targetMs;
      if (val1 > this.m_targetTicks)
      {
        this.m_targetTicks = val1;
        this.m_currentFrameIncreaseTime = 0.0f;
        this.m_currentMin = 0L;
        Sandbox.Engine.Platform.Game.m_targetMs = (float) Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(this.m_targetTicks);
      }
      else if ((double) this.m_currentFrameIncreaseTime > 2000.0 | (double) (this.m_targetTicks - this.m_currentMin) > 0.25 * (double) this.m_renderLoop.TickPerFrame)
      {
        this.m_targetTicks = this.m_currentMin;
        this.m_currentFrameIncreaseTime = 0.0f;
        this.m_currentMin = 0L;
        Sandbox.Engine.Platform.Game.m_targetMs = (float) Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(this.m_targetTicks);
      }
      int millisecondsTimeout = (int) (MyTimeSpan.FromTicks(this.m_targetTicks - (Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate)).Milliseconds - 0.1);
      if (millisecondsTimeout > 0 && !this.EnableMaxSpeed)
        Thread.CurrentThread.Join(millisecondsTimeout);
      long num = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate;
      while (this.m_targetTicks > num)
        num = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate;
    }

    private long UpdateCurrentFrame()
    {
      if (this.m_lastFrameTiming.Count > 5)
        this.m_lastFrameTiming.Dequeue();
      this.m_lastFrameTiming.Enqueue(this.FrameTimeTicks);
      long val1_1 = long.MaxValue;
      long val1_2 = 0;
      double num1 = 0.0;
      for (int index = 0; index < this.m_lastFrameTiming.Count; ++index)
      {
        val1_1 = Math.Min(val1_1, this.m_lastFrameTiming[index]);
        val1_2 = Math.Max(val1_2, this.m_lastFrameTiming[index]);
        num1 += (double) this.m_lastFrameTiming[index];
      }
      double num2 = num1 / (double) this.m_lastFrameTiming.Count;
      double num3 = (double) (val1_2 - val1_1) * 0.400000005960464;
      long num4 = 0;
      for (int index = 0; index < this.m_lastFrameTiming.Count; ++index)
      {
        if (Math.Abs((double) this.m_lastFrameTiming[index] - num2) < num3)
          num4 = Math.Max(val1_2, this.m_lastFrameTiming[index]);
      }
      return num4 == 0L ? (long) num2 : num4;
    }

    protected abstract void PrepareForDraw();

    protected abstract void AfterDraw();

    protected abstract void LoadData_UpdateThread();

    protected abstract void UnloadData_UpdateThread();

    private void UpdateInternal()
    {
      MySimpleProfiler.BeginBlock("UpdateFrame", MySimpleProfiler.ProfilingBlockType.INTERNAL);
      using (Stats.Generic.Measure("BeforeUpdate"))
        MyRenderProxy.BeforeUpdate();
      this.m_totalTime = this.m_gameTimer.Elapsed;
      ++this.m_updateCounter;
      if (MySession.Static != null)
        this.m_simulationTimeWithSpeed += MyTimeSpan.FromMilliseconds(50.0 / 3.0 * (double) MyFakes.SIMULATION_SPEED);
      int num = MyCompilationSymbols.EnableNetworkPacketTracking ? 1 : 0;
      this.Update();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.PrepareForDraw();
      using (Stats.Generic.Measure("AfterUpdate"))
        this.AfterDraw();
      MySimpleProfiler.End("UpdateFrame");
      MySimpleProfiler.Commit();
    }

    protected virtual void Update() => this.isFirstUpdateDone = true;
  }
}
