// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugTiming
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Network;
using VRage.Stats;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal class MyGuiScreenDebugTiming : MyGuiScreenDebugBase
  {
    private long m_startTime;
    private long m_ticks;
    private int m_frameCounter;
    private double m_updateLag;
    private static int m_callChainDepth;
    private static int m_numInstructions;

    public MyGuiScreenDebugTiming()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(new Vector2()), new Vector4?(), true)
    {
      this.m_isTopMostScreen = true;
      this.m_drawEvenWithoutFocus = true;
      this.CanHaveFocus = false;
      this.m_canShareInput = false;
    }

    public override void LoadData()
    {
      base.LoadData();
      MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.SimpleTimingStats;
    }

    public override void UnloadData()
    {
      base.UnloadData();
      MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.NoDraw;
    }

    public override string GetFriendlyName() => "DebugTimingScreen";

    public override bool Update(bool hasFocus)
    {
      this.m_ticks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks;
      ++this.m_frameCounter;
      double num1 = Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(this.m_ticks - this.m_startTime) / 1000.0;
      if (num1 > 1.0)
      {
        this.m_updateLag = (num1 - (double) this.m_frameCounter * 0.0166666675359011) / num1 * 1000.0;
        this.m_startTime = this.m_ticks;
        this.m_frameCounter = 0;
      }
      VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.Frame, MySandboxGame.Static != null ? (float) MySandboxGame.Static.SimulationFrameCounter : 0.0f, MyStatTypeEnum.CurrentValue, 0, 0);
      VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.FPS, (float) MyFpsManager.GetFps(), MyStatTypeEnum.CurrentValue, 0, 0);
      VRageRender.Utils.Stats.Timing.Increment(MyStatKeys.StatKeysEnum.UPS, 1000);
      VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.SimSpeed, MyPhysics.SimulationRatio, MyStatTypeEnum.CurrentValue, 100, 2);
      VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.SimCpuLoad, (float) (int) MySandboxGame.Static.CPULoadSmooth, MySandboxGame.Static.CPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
      VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ThreadCpuLoad, (float) (int) MySandboxGame.Static.ThreadLoadSmooth, MySandboxGame.Static.ThreadTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
      VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.RenderCpuLoad, (float) (int) MyRenderProxy.CPULoadSmooth, MyRenderProxy.CPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
      VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.RenderGpuLoad, (float) (int) MyRenderProxy.GPULoadSmooth, MyRenderProxy.GPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
      if (Sync.Layer != null)
      {
        VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.ServerSimSpeed, Sync.ServerSimulationRatio, MyStatTypeEnum.CurrentValue, 100, 2);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerSimCpuLoad, (float) (int) Sync.ServerCPULoadSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerThreadCpuLoad, (float) (int) Sync.ServerThreadLoadSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Up, MyGeneralStats.Static.SentPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Down, MyGeneralStats.Static.ReceivedPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerUp, MyGeneralStats.Static.ServerSentPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerDown, MyGeneralStats.Static.ServerReceivedPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Roundtrip, (float) MyGeneralStats.Static.Ping, MyStatTypeEnum.CurrentValue, 0, 0);
      }
      if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.ComplexTimingStats)
      {
        int cachedChuncks = 0;
        int pendingCachedChuncks = 0;
        if (MySession.Static != null)
          MySession.Static.VoxelMaps.GetCacheStats(out cachedChuncks, out pendingCachedChuncks);
        VRageRender.Utils.Stats.Timing.WriteFormat("Voxel cache size: {0} / {3}", (float) cachedChuncks, (float) pendingCachedChuncks, MyStatTypeEnum.CurrentValue, 0, 1);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PlayoutDelayBuffer, (float) MyGeneralStats.Static.PlayoutDelayBufferSize, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameTime, MyFpsManager.FrameTime, MyStatTypeEnum.CurrentValue, 0, 1);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameAvgTime, MyFpsManager.FrameTimeAvg, MyStatTypeEnum.CurrentValue, 0, 1);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameMinTime, MyFpsManager.FrameTimeMin, MyStatTypeEnum.CurrentValue, 0, 1);
        VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameMaxTime, MyFpsManager.FrameTimeMax, MyStatTypeEnum.CurrentValue, 0, 1);
        VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.UpdateLag, (float) this.m_updateLag, MyStatTypeEnum.CurrentValue, 0, 4);
        float used;
        MyVRage.Platform.System.GetGCMemory(out float _, out used);
        VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.GcMemory, used, MyStatTypeEnum.CurrentValue, 0, 0);
        VRageRender.Utils.Stats.Timing.Write(MyStatKeys.StatKeysEnum.ActiveParticleEffs, (float) MyParticlesManager.InstanceCount, MyStatTypeEnum.CurrentValue, 0, 0);
        ListReader<object>? clusterList = MyPhysics.GetClusterList();
        if (clusterList.HasValue)
        {
          double num2 = 0.0;
          double num3 = 0.0;
          double num4 = 0.0;
          long num5 = 0;
          clusterList = MyPhysics.GetClusterList();
          foreach (HkWorld hkWorld in clusterList.Value)
          {
            ++num2;
            double totalMilliseconds = hkWorld.StepDuration.TotalMilliseconds;
            num3 += totalMilliseconds;
            if (totalMilliseconds > num4)
              num4 = totalMilliseconds;
            num5 += (long) hkWorld.ActiveRigidBodies.Count;
          }
          VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysWorldCount, (float) num2, MyStatTypeEnum.CurrentValue, 0, 0);
          VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ActiveRigBodies, (float) num5, MyStatTypeEnum.CurrentValue, 0, 1);
          VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeSum, (float) num3, MyStatTypeEnum.CurrentValue, 0, 1);
          VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeAvg, (float) (num3 / num2), MyStatTypeEnum.CurrentValue, 0, 1);
          VRageRender.Utils.Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeMax, (float) num4, MyStatTypeEnum.CurrentValue, 0, 1);
        }
      }
      return base.Update(hasFocus);
    }
  }
}
