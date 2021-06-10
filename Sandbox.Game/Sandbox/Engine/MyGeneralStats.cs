// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.MyGeneralStats
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Memory;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;
using VRage.Replication;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine
{
  public class MyGeneralStats
  {
    private MyTimeSpan m_lastTime;
    private static int AVERAGE_WINDOW_SIZE = 60;
    private static int SERVER_AVERAGE_WINDOW_SIZE = 6;
    private readonly MyMovingAverage m_received = new MyMovingAverage(MyGeneralStats.AVERAGE_WINDOW_SIZE);
    private readonly MyMovingAverage m_sent = new MyMovingAverage(MyGeneralStats.AVERAGE_WINDOW_SIZE);
    private readonly MyMovingAverage m_timeIntervals = new MyMovingAverage(MyGeneralStats.AVERAGE_WINDOW_SIZE);
    private readonly MyMovingAverage m_serverReceived = new MyMovingAverage(MyGeneralStats.SERVER_AVERAGE_WINDOW_SIZE);
    private readonly MyMovingAverage m_serverSent = new MyMovingAverage(MyGeneralStats.SERVER_AVERAGE_WINDOW_SIZE);
    private readonly MyMovingAverage m_serverTimeIntervals = new MyMovingAverage(MyGeneralStats.SERVER_AVERAGE_WINDOW_SIZE);
    public MyTimeSpan LogInterval = MyTimeSpan.FromSeconds(60.0);
    private bool m_first = true;
    private MyTimeSpan m_lastLogTime;
    private MyTimeSpan m_lastGridLogTime;
    private MyTimeSpan m_firstLogTime;
    private int m_gridsCount;
    private bool m_wasMemoryCritical;
    private int[] m_lastGcCount = new int[GC.MaxGeneration + 1];
    private int[] m_collectionsThisFrame = new int[GC.MaxGeneration + 1];
    private readonly Dictionary<string, (MyValueAggregator Aggregator, string AnalyticsName, bool Bytes)> m_statAggregators;
    private double m_aggregatorTime;
    private const string DataReceivedAggregatorName = "$$data_received";
    private const string DataSentAggregatorName = "$$data_sent";
    private const string PingAggregatorName = "$$ping";
    public readonly int[] PercentileValues = new int[8]
    {
      50,
      90,
      91,
      92,
      93,
      94,
      95,
      99
    };
    private static MyGeneralStats.MyStringMemoryLogger m_stringMemoryLogger = new MyGeneralStats.MyStringMemoryLogger(false);

    public static MyGeneralStats Static { get; private set; }

    public float Received { get; private set; }

    public float Sent { get; private set; }

    public float ReceivedPerSecond { get; private set; }

    public float SentPerSecond { get; private set; }

    public float PeakReceivedPerSecond { get; private set; }

    public float PeakSentPerSecond { get; private set; }

    public long OverallReceived { get; private set; }

    public long OverallSent { get; private set; }

    public float ServerReceivedPerSecond { get; private set; }

    public float ServerSentPerSecond { get; private set; }

    public byte PlayoutDelayBufferSize { get; private set; }

    public float ServerGCMemory { get; private set; }

    public float ServerProcessMemory { get; private set; }

    public int GridsCount => this.m_gridsCount;

    public long Ping { get; set; }

    public bool LowNetworkQuality { get; private set; }

    static MyGeneralStats() => MyGeneralStats.Static = new MyGeneralStats();

    private MyGeneralStats() => this.m_statAggregators = new Dictionary<string, (MyValueAggregator, string, bool)>()
    {
      {
        "Total Sent",
        (new MyValueAggregator(300, this.PercentileValues), "p2p_data_sent", true)
      },
      {
        "Total Received",
        (new MyValueAggregator(300, this.PercentileValues), "p2p_data_received", true)
      },
      {
        "RTT (ms)",
        (new MyValueAggregator(300, this.PercentileValues), "p2p_rtt", false)
      },
      {
        "RTT Var (ms)",
        (new MyValueAggregator(300, this.PercentileValues), "p2p_rtt_var", false)
      },
      {
        "$$data_received",
        (new MyValueAggregator(300, this.PercentileValues), "data_received", true)
      },
      {
        "$$data_sent",
        (new MyValueAggregator(300, this.PercentileValues), "data_sent", true)
      },
      {
        "$$ping",
        (new MyValueAggregator(300, this.PercentileValues), "ping", false)
      }
    };

    public void Update()
    {
      int received;
      int tamperred;
      MyNetworkReader.GetAndClearStats(out received, out tamperred);
      int andClearStats = MyNetworkWriter.GetAndClearStats();
      this.OverallReceived += (long) received;
      this.OverallSent += (long) andClearStats;
      MyTimeSpan simulationTime = MySandboxGame.Static.SimulationTime;
      float seconds = (float) (simulationTime - this.m_lastTime).Seconds;
      this.m_lastTime = simulationTime;
      this.m_received.Enqueue((float) received);
      this.m_sent.Enqueue((float) andClearStats);
      this.m_timeIntervals.Enqueue(seconds);
      this.Received = this.m_received.Avg;
      this.Sent = this.m_sent.Avg;
      this.ReceivedPerSecond = (float) (this.m_received.Sum / this.m_timeIntervals.Sum);
      this.SentPerSecond = (float) (this.m_sent.Sum / this.m_timeIntervals.Sum);
      this.m_aggregatorTime += (double) seconds;
      if (this.m_aggregatorTime > 1.0)
      {
        --this.m_aggregatorTime;
        this.AggregateStatistics();
      }
      if ((double) this.ReceivedPerSecond > (double) this.PeakReceivedPerSecond)
        this.PeakReceivedPerSecond = this.ReceivedPerSecond;
      if ((double) this.SentPerSecond > (double) this.PeakSentPerSecond)
        this.PeakSentPerSecond = this.SentPerSecond;
      float allocated;
      float used;
      MyVRage.Platform.System.GetGCMemory(out allocated, out used);
      float remainingMemoryForGame = (float) MyVRage.Platform.System.RemainingMemoryForGame;
      float num1 = (float) ((double) MyVRage.Platform.System.ProcessPrivateMemory / 1024.0 / 1024.0);
      for (int generation = 0; generation < GC.MaxGeneration; ++generation)
      {
        int num2 = GC.CollectionCount(generation);
        this.m_collectionsThisFrame[generation] = num2 - this.m_lastGcCount[generation];
        this.m_lastGcCount[generation] = num2;
      }
      if (Sync.MultiplayerActive && Sync.IsServer)
        MyMultiplayer.Static.ReplicationLayer.UpdateStatisticsData(andClearStats, received, tamperred, used, num1);
      bool flag1 = MySandboxGame.Static.MemoryState == MySandboxGame.MemState.Critical;
      bool flag2 = flag1 && !this.m_wasMemoryCritical;
      this.m_wasMemoryCritical = flag1;
      if (MySession.Static != null && simulationTime > this.m_lastLogTime + this.LogInterval | flag2)
      {
        this.m_lastLogTime = simulationTime;
        if (this.m_first)
        {
          this.m_firstLogTime = simulationTime;
          this.m_first = false;
        }
        MyLog.Default.WriteLine("STATISTICS LEGEND,time,ReceivedPerSecond,SentPerSecond,PeakReceivedPerSecond,PeakSentPerSecond,OverallReceived,OverallSent,CPULoadSmooth,ThreadLoadSmooth,GetOnlinePlayerCount,Ping,GCMemoryUsed,ProcessMemory,PCUBuilt,PCU,GridsCount,RenderCPULoadSmooth,RenderGPULoadSmooth,HardwareCPULoad,HardwareAvailableMemory,FrameTime,LowSimQuality,FrameTimeLimit,FrameTimeCPU,FrameTimeGPU,CPULoadLimit,TrackedMemory,GCMemoryAllocated,PersistedEncounters,EncounterEntities");
        float cpuCounter = MyVRage.Platform.System.CPUCounter;
        float ramCounter = MyVRage.Platform.System.RAMCounter;
        float num2 = 16.66667f;
        if (MyFakes.ENABLE_PERFORMANCELOGGING)
        {
          PerformanceLogMessage toSerialize = new PerformanceLogMessage()
          {
            Time = (simulationTime - this.m_firstLogTime).Seconds,
            ReceivedPerSecond = (float) ((double) this.ReceivedPerSecond / 1024.0 / 1024.0),
            SentPerSecond = (float) ((double) this.SentPerSecond / 1024.0 / 1024.0),
            PeakReceivedPerSecond = (float) ((double) this.PeakReceivedPerSecond / 1024.0 / 1024.0),
            PeakSentPerSecond = (float) ((double) this.PeakSentPerSecond / 1024.0 / 1024.0),
            OverallReceived = (float) ((double) this.OverallReceived / 1024.0 / 1024.0),
            OverallSent = (float) ((double) this.OverallSent / 1024.0 / 1024.0),
            CPULoadSmooth = MySandboxGame.Static.CPULoadSmooth,
            ThreadLoadSmooth = MySandboxGame.Static.ThreadLoadSmooth,
            GetOnlinePlayerCount = Sync.Players.GetOnlinePlayerCount(),
            Ping = (float) this.Ping,
            GCMemoryUsed = used,
            ProcessMemory = num1,
            PCUBuilt = (float) MySession.Static.SessionBlockLimits.PCUBuilt,
            PCU = (float) MySession.Static.SessionBlockLimits.PCU,
            GridsCount = (float) this.GridsCount,
            RenderCPULoadSmooth = MyRenderProxy.CPULoadSmooth,
            RenderGPULoadSmooth = MyRenderProxy.GPULoadSmooth,
            HardwareCPULoad = cpuCounter,
            HardwareAvailableMemory = ramCounter,
            FrameTime = MyFpsManager.FrameTimeAvg,
            LowSimQuality = !MySession.Static.HighSimulationQuality ? 1f : 0.0f,
            FrameTimeLimit = num2,
            FrameTimeCPU = (float) ((double) MyRenderProxy.CPULoadSmooth * (double) MyFpsManager.FrameTimeAvg / 100.0),
            FrameTimeGPU = (float) ((double) MyRenderProxy.GPULoadSmooth * (double) MyFpsManager.FrameTimeAvg / 100.0),
            CPULoadLimit = 100f,
            TrackedMemory = (float) Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory(),
            GCMemoryAllocated = allocated,
            ExePath = Environment.CurrentDirectory,
            GameVersion = MyPerGameSettings.BasicGameInfo.GameVersion.Value,
            SavePath = MySession.Static.CurrentPath
          };
          if (MyTestingToolHelper.CurrentTestPath != null)
            toSerialize.SavePath = MyTestingToolHelper.CurrentTestPath;
          Console.WriteLine(PerformanceLogMessage.SerializeObject<PerformanceLogMessage>(toSerialize));
        }
        int persistentEncounters = 0;
        int encounterEntities = 0;
        MyEncounterGenerator.Static?.GetStats(out persistentEncounters, out encounterEntities);
        MyLog.Default.WriteLine(string.Join(",", (object) "STATISTICS", (object) (simulationTime - this.m_firstLogTime).Seconds, (object) (float) ((double) this.ReceivedPerSecond / 1024.0 / 1024.0), (object) (float) ((double) this.SentPerSecond / 1024.0 / 1024.0), (object) (float) ((double) this.PeakReceivedPerSecond / 1024.0 / 1024.0), (object) (float) ((double) this.PeakSentPerSecond / 1024.0 / 1024.0), (object) (float) ((double) this.OverallReceived / 1024.0 / 1024.0), (object) (float) ((double) this.OverallSent / 1024.0 / 1024.0), (object) MySandboxGame.Static.CPULoadSmooth, (object) MySandboxGame.Static.ThreadLoadSmooth, (object) (Sync.MultiplayerActive ? MyMultiplayer.Static.MemberCount : 1), (object) this.Ping, (object) used, (object) num1, (object) MySession.Static.TotalSessionPCU, (object) MySession.Static.SessionBlockLimits.PCU, (object) this.GridsCount, (object) MyRenderProxy.CPULoadSmooth, (object) MyRenderProxy.GPULoadSmooth, (object) cpuCounter, (object) ramCounter, (object) MyFpsManager.FrameTimeAvg, (object) (!MySession.Static.HighSimulationQuality ? 1 : 0), (object) num2, (object) (float) ((double) MyRenderProxy.CPULoadSmooth * (double) MyFpsManager.FrameTimeAvg / 100.0), (object) (float) ((double) MyRenderProxy.GPULoadSmooth * (double) MyFpsManager.FrameTimeAvg / 100.0), (object) 100, (object) Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory(), (object) allocated, (object) persistentEncounters, (object) encounterEntities));
        MyGeneralStats.m_stringMemoryLogger.PrintToLog(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem, MyLog.Default);
        if (MyGameService.Peer2Peer != null && MyPlatformGameSettings.VERBOSE_NETWORK_LOGGING)
        {
          (string, double)[] array = MyGameService.Peer2Peer.Stats.ToArray<(string, double)>();
          if (array.Length != 0)
          {
            MyLog.Default.WriteLine("Detailed Transport Stats Legend: " + string.Join(", ", ((IEnumerable<(string, double)>) array).Select<(string, double), string>((Func<(string, double), string>) (x => x.Name))));
            MyLog.Default.WriteLine("Detailed Transport Stats: " + string.Join(", ", ((IEnumerable<(string, double)>) array).Select<(string, double), string>((Func<(string, double), string>) (x => x.Value.ToString("#0.00")))));
          }
          bool flag3 = true;
          foreach ((string Client4, IEnumerable<(string Stat, double Value)> tuples4) in MyGameService.Peer2Peer.ClientStats)
          {
            if (flag3)
            {
              MyLog.Default.WriteLine("Client Stats Legend: Client, " + string.Join(", ", tuples4.Select<(string, double), string>((Func<(string, double), string>) (x => x.Stat))));
              flag3 = false;
            }
            MyLog.Default.WriteLine("Client Stats: " + Client4 + ", " + string.Join(", ", tuples4.Select<(string, double), string>((Func<(string, double), string>) (x => x.Value.ToString("#0.00")))));
          }
          string detailedStats = MyGameService.Peer2Peer.DetailedStats;
          if (!string.IsNullOrWhiteSpace(detailedStats))
            MyLog.Default.WriteLine("Detailed Transport Stats (Human Readable):\n" + detailedStats);
          MyLog.Default.WriteLine(string.Format("Pending bytes in network writer: {0}", (object) MyNetworkWriter.QueuedBytes));
        }
      }
      MyPacketStatistics packetStatistics1 = new MyPacketStatistics();
      if (Sync.IsServer)
      {
        this.ServerReceivedPerSecond = this.ReceivedPerSecond;
        this.ServerSentPerSecond = this.SentPerSecond;
      }
      else if (Sync.MultiplayerActive)
      {
        packetStatistics1 = MyMultiplayer.Static.ReplicationLayer.ClearServerStatistics();
        if ((double) packetStatistics1.TimeInterval > 0.0)
        {
          this.m_serverReceived.Enqueue((float) packetStatistics1.IncomingData);
          this.m_serverSent.Enqueue((float) packetStatistics1.OutgoingData);
          this.m_serverTimeIntervals.Enqueue(packetStatistics1.TimeInterval);
          this.ServerReceivedPerSecond = (float) (this.m_serverReceived.Sum / this.m_serverTimeIntervals.Sum);
          this.ServerSentPerSecond = (float) (this.m_serverSent.Sum / this.m_serverTimeIntervals.Sum);
          this.PlayoutDelayBufferSize = packetStatistics1.PlayoutDelayBufferSize;
          this.ServerGCMemory = packetStatistics1.GCMemory;
          this.ServerProcessMemory = packetStatistics1.ProcessMemory;
        }
      }
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyStatsGraph.Started)
        return;
      MyStatsGraph.Begin("Client Traffic Avg", member: nameof (Update), line: 342, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Outgoing avg", this.SentPerSecond / 1024f, "{0} kB/s", member: nameof (Update), line: 343, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Incoming avg", this.ReceivedPerSecond / 1024f, "{0} kB/s", member: nameof (Update), line: 344, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?((float) (((double) this.SentPerSecond + (double) this.ReceivedPerSecond) / 1024.0)), byteFormat: "{0} kB/s", member: nameof (Update), line: 345, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Server Traffic Avg", member: nameof (Update), line: 346, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Outgoing avg", this.ServerSentPerSecond / 1024f, "{0} kB/s", member: nameof (Update), line: 347, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Incoming avg", this.ServerReceivedPerSecond / 1024f, "{0} kB/s", member: nameof (Update), line: 348, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?((float) (((double) this.ServerSentPerSecond + (double) this.ServerReceivedPerSecond) / 1024.0)), byteFormat: "{0} kB/s", member: nameof (Update), line: 349, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Client Perf Avg", member: nameof (Update), line: 351, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoadSmooth, "{0}%", member: nameof (Update), line: 352, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoadSmooth, "{0}%", member: nameof (Update), line: 353, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoadSmooth, "{0}%", member: nameof (Update), line: 354, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoadSmooth, "{0}%", member: nameof (Update), line: 355, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Render Frame", MyFpsManager.FrameTimeAvg, "{0}ms", member: nameof (Update), line: 356, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?(MySandboxGame.Static.CPULoadSmooth), customValueFormat: ((string) null), byteFormat: "{0}%", member: nameof (Update), line: 357, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Server Perf Avg", member: nameof (Update), line: 358, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", member: nameof (Update), line: 359, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", member: nameof (Update), line: 360, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?(Sync.ServerCPULoadSmooth), customValueFormat: ((string) null), byteFormat: "{0}%", member: nameof (Update), line: 361, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      float? bytesTransfered;
      if (MySession.Static != null)
      {
        MyStatsGraph.Begin("World", member: nameof (Update), line: 366, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("PCUBuilt", (float) MySession.Static.SessionBlockLimits.PCUBuilt, "{0}", member: nameof (Update), line: 367, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("PCU", (float) MySession.Static.SessionBlockLimits.PCU, "{0}", member: nameof (Update), line: 368, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("NPC PCUBuilt", (float) MySession.Static.NPCBlockLimits.PCUBuilt, "{0}", member: nameof (Update), line: 369, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("NPC PCU", (float) MySession.Static.NPCBlockLimits.PCU, "{0}", member: nameof (Update), line: 370, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Session PCU", (float) MySession.Static.TotalSessionPCU, "{0}", member: nameof (Update), line: 371, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("GridsCount", (float) this.GridsCount, "{0}", member: nameof (Update), line: 372, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        if (!Sync.IsServer && MyMultiplayer.Static.ReplicationLayer is MyReplicationClient replicationLayer)
        {
          bytesTransfered = replicationLayer.ReplicationRange;
          MyStatsGraph.CustomTime("Sync Distance", (float) (bytesTransfered.HasValue ? (double) bytesTransfered.GetValueOrDefault() : (double) MySession.Static.Settings.SyncDistance), "{0}", member: nameof (Update), line: 375, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        }
        bytesTransfered = new float?();
        MyStatsGraph.End(bytesTransfered, member: nameof (Update), line: 377, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      }
      MyStatsGraph.Begin("Memory", member: nameof (Update), line: 380, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Overview", member: nameof (Update), line: 384, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Untracked", (float) ((double) num1 - (double) allocated - (double) Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.GetTotalMemory() / 1024.0 / 1024.0), "{0} MB", member: nameof (Update), line: 387, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Collections", member: nameof (Update), line: 390, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      for (int index = 0; index < this.m_collectionsThisFrame.Length; ++index)
        MyStatsGraph.CustomTime("Gen" + (object) index, (float) this.m_collectionsThisFrame[index], "{0}", member: nameof (Update), line: 393, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      bytesTransfered = new float?();
      MyStatsGraph.End(bytesTransfered, member: nameof (Update), line: 395, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("GC Used", used, "{0} MB", member: nameof (Update), line: 397, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("GC Allocated", allocated, "{0} MB", member: nameof (Update), line: 398, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Client Process", num1, "{0} MB", member: nameof (Update), line: 399, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Remaining game memory", remainingMemoryForGame, "{0} MB", member: nameof (Update), line: 400, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Server GC", this.ServerGCMemory, "{0} MB", member: nameof (Update), line: 401, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Server Process", this.ServerProcessMemory, "{0} MB", member: nameof (Update), line: 402, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      bytesTransfered = new float?();
      MyStatsGraph.End(bytesTransfered, byteFormat: "{0} MB", member: nameof (Update), line: 406, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyGeneralStats.MemoryLogger logger = new MyGeneralStats.MemoryLogger();
      Singleton<MyMemoryTracker>.Instance.LogMemoryStats<MyGeneralStats.MemoryLogger>(ref logger);
      bytesTransfered = new float?();
      MyStatsGraph.End(bytesTransfered, member: nameof (Update), line: 412, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      if (Sync.MultiplayerActive)
      {
        MyPacketStatistics packetStatistics2 = MyMultiplayer.Static.ReplicationLayer.ClearClientStatistics();
        int num2 = packetStatistics2.Drops + packetStatistics2.OutOfOrder + packetStatistics2.Duplicates + (int) packetStatistics1.PendingPackets + packetStatistics1.Drops + packetStatistics1.OutOfOrder + packetStatistics1.Duplicates;
        MyStatsGraph.Begin("Packet errors", member: nameof (Update), line: 419, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Client Drops", (float) packetStatistics2.Drops, "{0}", member: nameof (Update), line: 420, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Client OutOfOrder", (float) packetStatistics2.OutOfOrder, "{0}", member: nameof (Update), line: 421, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Client Duplicates", (float) packetStatistics2.Duplicates, "{0}", member: nameof (Update), line: 422, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Client Tamperred", (float) packetStatistics2.Tamperred, "{0}", member: nameof (Update), line: 423, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Server Pending Packets", (float) packetStatistics1.PendingPackets, "{0}", member: nameof (Update), line: 424, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Server Drops", (float) packetStatistics1.Drops, "{0}", member: nameof (Update), line: 425, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Server OutOfOrder", (float) packetStatistics1.OutOfOrder, "{0}", member: nameof (Update), line: 426, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Server Duplicates", (float) packetStatistics1.Duplicates, "{0}", member: nameof (Update), line: 427, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Server Tamperred", (float) packetStatistics2.Tamperred, "{0}", member: nameof (Update), line: 428, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.End(new float?((float) num2), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (Update), line: 429, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        this.LowNetworkQuality = num2 > 5;
      }
      else
        this.LowNetworkQuality = false;
      if (MySession.Static != null)
      {
        MyStatsGraph.Begin("Physics", member: nameof (Update), line: 439, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("Clusters", (float) MyPhysics.Clusters.GetClusters().Count, "{0}", member: nameof (Update), line: 441, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("VoxelBodies", (float) MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies, "{0}", member: nameof (Update), line: 442, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.CustomTime("LargeVoxelBodies", (float) MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache, "{0}", member: nameof (Update), line: 443, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (Update), line: 445, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      }
      MyStatsGraph.ProfileAdvanced(true);
      MyStatsGraph.Begin("Traffic", member: nameof (Update), line: 450, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Outgoing", this.Sent / 1024f, "{0} kB", member: nameof (Update), line: 451, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Incoming", this.Received / 1024f, "{0} kB", member: nameof (Update), line: 452, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?((float) (((double) this.SentPerSecond + (double) this.ReceivedPerSecond) / 1024.0)), byteFormat: "{0} kB", member: nameof (Update), line: 453, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Server Perf Avg", member: nameof (Update), line: 455, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", member: nameof (Update), line: 456, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", member: nameof (Update), line: 457, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (Update), line: 458, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Client Performance", member: nameof (Update), line: 460, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoad, "{0}%", member: nameof (Update), line: 461, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoad, "{0}%", member: nameof (Update), line: 462, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoad, "{0}%", member: nameof (Update), line: 463, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoad, "{0}%", member: nameof (Update), line: 464, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (Update), line: 465, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.Begin("Server Performance", member: nameof (Update), line: 467, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoad, "{0}%", member: nameof (Update), line: 468, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoad, "{0}%", member: nameof (Update), line: 469, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyStatsGraph.End(new float?(0.0f), customValueFormat: ((string) null), byteFormat: "{0}", member: nameof (Update), line: 470, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      MyMultiplayer.Static?.ReplicationLayer?.ReportEvents();
      MyStatsGraph.ProfileAdvanced(false);
    }

    private void AggregateStatistics()
    {
      this.m_statAggregators["$$data_received"].Aggregator.Push((double) this.ReceivedPerSecond);
      this.m_statAggregators["$$data_sent"].Aggregator.Push((double) this.SentPerSecond);
      if (this.Ping != 0L)
        this.m_statAggregators["$$ping"].Aggregator.Push((double) this.Ping);
      if (MyGameService.Peer2Peer == null)
        return;
      foreach ((string Name, double Value) stat in MyGameService.Peer2Peer.Stats)
      {
        (MyValueAggregator Aggregator, string AnalyticsName, bool Bytes) tuple;
        if (this.m_statAggregators.TryGetValue(stat.Name, out tuple))
          tuple.Aggregator.Push(stat.Value);
      }
      foreach ((string Client, IEnumerable<(string Stat, double Value)> Stats) clientStat in MyGameService.Peer2Peer.ClientStats)
      {
        foreach ((string Stat, double Value) tuple1 in clientStat.Stats)
        {
          (MyValueAggregator Aggregator, string AnalyticsName, bool Bytes) tuple2;
          if (this.m_statAggregators.TryGetValue(tuple1.Stat, out tuple2))
          {
            if (!tuple2.Bytes)
            {
              if (tuple1.Value > 0.0)
              {
                tuple2.Aggregator.Push(tuple1.Value);
                break;
              }
              break;
            }
            break;
          }
        }
      }
    }

    public IEnumerable<(string Name, double[] Value, bool Bytes)> AggregatedStats
    {
      get
      {
        foreach (KeyValuePair<string, (MyValueAggregator, string, bool)> statAggregator in this.m_statAggregators)
        {
          if (statAggregator.Value.Item1.HasData)
            yield return (statAggregator.Value.Item2, statAggregator.Value.Item1.PercentileValues, statAggregator.Value.Item3);
        }
      }
    }

    public void LoadData()
    {
      this.m_gridsCount = 0;
      MyEntities.OnEntityCreate += new Action<MyEntity>(this.OnEntityCreate);
      MyEntities.OnEntityDelete += new Action<MyEntity>(this.OnEntityDelete);
    }

    private void OnEntityCreate(MyEntity entity)
    {
      if (!(entity is MyCubeGrid))
        return;
      Interlocked.Increment(ref this.m_gridsCount);
    }

    private void OnEntityDelete(MyEntity entity)
    {
      if (!(entity is MyCubeGrid))
        return;
      Interlocked.Decrement(ref this.m_gridsCount);
    }

    public static void Clear()
    {
      MyNetworkWriter.GetAndClearStats();
      MyNetworkReader.GetAndClearStats(out int _, out int _);
      foreach (KeyValuePair<string, (MyValueAggregator Aggregator, string AnalyticsName, bool Bytes)> statAggregator in MyGeneralStats.Static.m_statAggregators)
        statAggregator.Value.Aggregator.Clear();
    }

    public static void ToggleProfiler()
    {
      Sandbox.MyRenderProfiler.EnableAutoscale(MyStatsGraph.PROFILER_NAME);
      Sandbox.MyRenderProfiler.ToggleProfiler(MyStatsGraph.PROFILER_NAME);
    }

    private struct MemoryLogger : MyMemoryTracker.ILogger
    {
      private string m_currentSystem;

      public void BeginSystem(string systemName)
      {
        if (this.m_currentSystem != null)
          MyStatsGraph.Begin(this.m_currentSystem, member: string.Empty, line: 568, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
        this.m_currentSystem = systemName;
      }

      public void EndSystem(long systemBytes, int totalAllocations)
      {
        float customValue = (float) totalAllocations;
        string customValueFormat = totalAllocations > 0 ? "Allocs: {0}" : string.Empty;
        if (this.m_currentSystem != null)
        {
          MyStatsGraph.CustomTime(this.m_currentSystem, (float) ((double) systemBytes / 1024.0 / 1024.0), "{0} MB", customValue, customValueFormat, nameof (EndSystem), 581, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
          this.m_currentSystem = (string) null;
        }
        else
          MyStatsGraph.End(new float?((float) ((double) systemBytes / 1024.0 / 1024.0)), customValue, customValueFormat, "{0} MB", member: string.Empty, line: 586, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
      }
    }

    private struct MyStringMemoryLogger : MyMemoryTracker.ILogger
    {
      private const int MaxDepth = 4;
      private Stack<string> m_blockStack;
      private StringBuilder m_stringBuilder;
      private Dictionary<string, int> m_blockValues;
      private string[] m_blockValuesOrder;
      private string m_blockValuesHeader;

      public MyStringMemoryLogger(bool _)
      {
        this.m_blockStack = new Stack<string>();
        this.m_stringBuilder = new StringBuilder();
        this.m_blockValues = new Dictionary<string, int>();
        this.m_blockValuesOrder = new string[36]
        {
          "Srv",
          "Uav",
          "Read",
          "Debug",
          "Index",
          "Audio",
          "SrvUav",
          "Vertex",
          "Buffers",
          "Physics",
          "Planets",
          "Systems",
          "Textures",
          "Indirect",
          "Constant",
          "RwTextures",
          "Dx11Render",
          "MeshBuffers",
          "FileTextures",
          "DepthStencil",
          "TileTextures",
          "Voxels-Native",
          "CustomTextures",
          "HeightmapFaces",
          "Mesh GPU Buffers",
          "BitStreamBuffers",
          "GeneratedTextures",
          "FileArrayTextures",
          "NativeDictionaries",
          "CubemapDataBuffers",
          "HeightDetailTexture",
          "MyDeviceWriteBuffers",
          "ShadowCascadesStatsBuffers",
          "AI_PathFinding",
          "EpicOnlineServices",
          "EpicOnlineServicesWrapper"
        };
        this.m_blockValuesHeader = "MEMORY LEGEND," + string.Join(",", this.m_blockValuesOrder);
      }

      public void BeginSystem(string systemName) => this.m_blockStack.Push(systemName);

      public void EndSystem(long systemBytes, int totalAllocations)
      {
        string key = this.m_blockStack.Pop();
        if (this.m_blockStack.Count >= 4)
          return;
        this.m_blockValues[key] = (int) ((double) systemBytes / 1024.0 / 1024.0);
      }

      public void PrintToLog(MyMemorySystem memorySystem, MyLog log)
      {
        memorySystem.LogMemoryStats<MyGeneralStats.MyStringMemoryLogger>(ref this);
        log.WriteLine(this.m_blockValuesHeader);
        StringBuilder stringBuilder = this.m_stringBuilder;
        stringBuilder.Append("MEMORY VALUES");
        foreach (string key in this.m_blockValuesOrder)
        {
          int num;
          this.m_blockValues.TryGetValue(key, out num);
          stringBuilder.Append(',').Append(num);
        }
        log.WriteLine(stringBuilder.ToString());
        stringBuilder.Clear();
      }
    }
  }
}
