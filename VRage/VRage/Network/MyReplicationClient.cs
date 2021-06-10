// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyReplicationClient
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Network
{
  public class MyReplicationClient : MyReplicationLayer
  {
    private const float NO_RESPONSE_ACTION_SECONDS = 5f;
    public static SerializableVector3I StressSleep = new SerializableVector3I(0, 0, 0);
    private readonly MyClientStateBase m_clientState;
    private bool m_clientReady;
    private bool m_hasTypeTable;
    private readonly IReplicationClientCallback m_callback;
    private readonly CacheList<IMyStateGroup> m_tmpGroups = new CacheList<IMyStateGroup>(4);
    private readonly List<byte> m_acks = new List<byte>();
    private bool m_receivedStreamingPackets;
    private byte m_lastStateSyncPacketId;
    private byte m_lastStreamingPacketId;
    private byte m_clientPacketId;
    private MyTimeSpan m_lastServerTimestamp = MyTimeSpan.Zero;
    private MyTimeSpan m_lastServerTimeStampReceivedTime = MyTimeSpan.Zero;
    private bool m_clientPaused;
    private readonly CachingDictionary<NetworkId, MyPendingReplicable> m_pendingReplicables = new CachingDictionary<NetworkId, MyPendingReplicable>();
    private readonly HashSet<NetworkId> m_destroyedReplicables = new HashSet<NetworkId>();
    private readonly MyEventsBuffer m_eventBuffer;
    private readonly MyEventsBuffer.Handler m_eventHandler;
    private readonly MyEventsBuffer.IsBlockedHandler m_isBlockedHandler;
    private MyTimeSpan m_clientStartTimeStamp = MyTimeSpan.Zero;
    private readonly float m_simulationTimeStep;
    private readonly ConcurrentCachingHashSet<IMyStateGroup> m_stateGroupsForUpdate = new ConcurrentCachingHashSet<IMyStateGroup>();
    private readonly Action<string> m_failureCallback;
    private const int MAX_TIMESTAMP_DIFF_LOW = 80;
    private const int MAX_TIMESTAMP_DIFF_HIGH = 500;
    private const int MAX_TIMESTAMP_DIFF_VERY_HIGH = 5000;
    private MyTimeSpan m_lastTime;
    private MyTimeSpan m_ping;
    private MyTimeSpan m_smoothPing;
    private MyTimeSpan m_lastPingTime;
    private MyTimeSpan m_correctionSmooth;
    public static MyReplicationClient.TimingType SynchronizationTimingType = MyReplicationClient.TimingType.None;
    private MyTimeSpan m_lastClientTime;
    private MyTimeSpan m_lastServerTime;
    private MyPacketStatistics m_serverStats;
    private readonly MyPacketTracker m_serverTracker = new MyPacketTracker();
    private MyPacketStatistics m_clientStats;
    private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();
    private readonly bool m_predictionReset;
    private MyTimeSpan m_lastClientTimestamp;
    private float m_timeDiffSmoothed;

    public MyTimeSpan Timestamp { get; private set; }

    public int PendingStreamingRelicablesCount { get; private set; }

    public float? ReplicationRange => this.m_clientState.ReplicationRange;

    public event Action<IMyReplicable> OnReplicableReady;

    public MyReplicationClient(
      Endpoint endpointId,
      IReplicationClientCallback callback,
      MyClientStateBase clientState,
      float simulationTimeStep,
      Action<string> failureCallback,
      bool predictionReset,
      Thread mainThread)
      : base(false, endpointId.Id, mainThread)
    {
      this.m_eventBuffer = new MyEventsBuffer(mainThread);
      this.m_replicables = (MyReplicablesBase) new MyReplicablesHierarchy(mainThread);
      this.m_simulationTimeStep = simulationTimeStep;
      this.m_callback = callback;
      this.m_clientState = clientState;
      this.m_clientState.EndpointId = endpointId;
      this.m_eventHandler = new MyEventsBuffer.Handler(((MyReplicationLayer) this).OnEvent);
      this.m_isBlockedHandler = new MyEventsBuffer.IsBlockedHandler(this.IsBlocked);
      this.m_failureCallback = failureCallback;
      this.m_predictionReset = predictionReset;
    }

    public override void Dispose()
    {
      this.m_eventBuffer.Dispose();
      base.Dispose();
    }

    protected override MyPacketDataBitStreamBase GetBitStreamPacketData() => this.m_callback.GetBitStreamPacketData();

    public override void Disconnect() => this.m_callback.DisconnectFromHost();

    public void OnLocalClientReady() => this.m_clientReady = true;

    protected override void AddNetworkObject(NetworkId networkId, IMyNetObject obj)
    {
      base.AddNetworkObject(networkId, obj);
      if (!(obj is IMyStateGroup myStateGroup) || !myStateGroup.NeedsUpdate)
        return;
      this.m_stateGroupsForUpdate.Add(myStateGroup);
    }

    protected override void RemoveNetworkedObjectInternal(NetworkId networkID, IMyNetObject obj)
    {
      base.RemoveNetworkedObjectInternal(networkID, obj);
      if (!(obj is IMyStateGroup myStateGroup))
        return;
      this.m_stateGroupsForUpdate.Remove(myStateGroup);
    }

    [HandleProcessCorruptedStateExceptions]
    [SecurityCritical]
    private void SetReplicableReady(
      NetworkId networkId,
      NetworkId parentId,
      IMyReplicable replicable,
      bool loaded)
    {
      try
      {
        MyPendingReplicable pendingReplicable1;
        if (this.m_pendingReplicables.TryGetValue(networkId, out pendingReplicable1) && pendingReplicable1.Replicable == replicable)
        {
          if (loaded)
          {
            this.m_pendingReplicables.Remove(networkId);
            this.m_pendingReplicables.ApplyRemovals();
            List<NetworkId> stateGroupIds = pendingReplicable1.StateGroupIds;
            this.AddNetworkObject(networkId, (IMyNetObject) replicable);
            this.m_replicables.Add(replicable, out IMyReplicable _);
            using (this.m_tmpGroups)
            {
              replicable.GetStateGroups((List<IMyStateGroup>) this.m_tmpGroups);
              for (int index = 0; index < this.m_tmpGroups.Count; ++index)
              {
                if (this.m_tmpGroups[index].IsStreaming)
                  --this.PendingStreamingRelicablesCount;
                else
                  this.AddNetworkObject(stateGroupIds[index], (IMyNetObject) this.m_tmpGroups[index]);
              }
            }
            if (pendingReplicable1.DependentReplicables != null)
            {
              foreach (KeyValuePair<NetworkId, MyPendingReplicable> dependentReplicable1 in pendingReplicable1.DependentReplicables)
              {
                KeyValuePair<NetworkId, MyPendingReplicable> dependentReplicable = dependentReplicable1;
                dependentReplicable.Value.Replicable.Reload((Action<bool>) (dependentLoaded => this.SetReplicableReady(dependentReplicable.Key, networkId, dependentReplicable.Value.Replicable, dependentLoaded)));
              }
            }
            this.m_eventBuffer.ProcessEvents(networkId, this.m_eventHandler, this.m_isBlockedHandler, NetworkId.Invalid);
            MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
            streamPacketData.Stream.WriteNetworkId(networkId);
            streamPacketData.Stream.WriteBool(loaded);
            streamPacketData.Stream.Terminate();
            this.m_callback.SendReplicableReady((IPacketData) streamPacketData);
            Action<IMyReplicable> onReplicableReady = this.OnReplicableReady;
            if (onReplicableReady == null)
              return;
            onReplicableReady(replicable);
          }
          else
          {
            MyPendingReplicable pendingReplicable2;
            if (this.m_pendingReplicables.TryGetValue(parentId, out pendingReplicable2))
            {
              if (pendingReplicable2.DependentReplicables == null)
                pendingReplicable2.DependentReplicables = new Dictionary<NetworkId, MyPendingReplicable>();
              pendingReplicable2.DependentReplicables.Add(networkId, pendingReplicable1);
            }
            else
              this.ReplicableDestroy(replicable, false);
          }
        }
        else
          replicable.OnDestroyClient();
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        throw;
      }
    }

    public void ProcessReplicationCreateBegin(MyPacket packet)
    {
      TypeId typeId = packet.BitStream.ReadTypeId();
      NetworkId networkID = packet.BitStream.ReadNetworkId();
      NetworkId parentID = packet.BitStream.ReadNetworkId();
      byte num = packet.BitStream.ReadByte();
      MyPendingReplicable pendingReplicable = new MyPendingReplicable();
      for (int index = 0; index < (int) num; ++index)
      {
        NetworkId networkId = packet.BitStream.ReadNetworkId();
        pendingReplicable.StateGroupIds.Add(networkId);
      }
      IMyReplicable replicable = (IMyReplicable) Activator.CreateInstance(this.GetTypeByTypeId(typeId));
      pendingReplicable.Replicable = replicable;
      pendingReplicable.ParentID = parentID;
      if (!this.m_pendingReplicables.ContainsKey(networkID))
      {
        this.m_pendingReplicables.Add(networkID, pendingReplicable);
        this.m_pendingReplicables.ApplyAdditionsAndModifications();
      }
      List<NetworkId> stateGroupIds = pendingReplicable.StateGroupIds;
      IMyStreamableReplicable streamableReplicable = replicable as IMyStreamableReplicable;
      pendingReplicable.IsStreaming = true;
      streamableReplicable.CreateStreamingStateGroup();
      ++this.PendingStreamingRelicablesCount;
      this.AddNetworkObject(stateGroupIds[0], (IMyNetObject) streamableReplicable.GetStreamingStateGroup());
      pendingReplicable.StreamingGroupId = stateGroupIds[0];
      streamableReplicable.OnLoadBegin((Action<bool>) (loaded => this.SetReplicableReady(networkID, parentID, replicable, loaded)));
      packet.Return();
    }

    public void ProcessReplicationCreate(MyPacket packet)
    {
      TypeId typeId = packet.BitStream.ReadTypeId();
      NetworkId networkID = packet.BitStream.ReadNetworkId();
      NetworkId parentID = packet.BitStream.ReadNetworkId();
      byte num = packet.BitStream.ReadByte();
      if (parentID.IsValid)
      {
        if (!this.m_pendingReplicables.ContainsKey(parentID) && this.GetObjectByNetworkId(parentID) == null)
        {
          packet.Return();
          return;
        }
      }
      else
        this.m_destroyedReplicables.Remove(networkID);
      MyPendingReplicable pendingReplicable = new MyPendingReplicable();
      pendingReplicable.ParentID = parentID;
      for (int index = 0; index < (int) num; ++index)
      {
        NetworkId networkId = packet.BitStream.ReadNetworkId();
        pendingReplicable.StateGroupIds.Add(networkId);
      }
      IMyReplicable replicable = (IMyReplicable) Activator.CreateInstance(this.GetTypeByTypeId(typeId));
      pendingReplicable.Replicable = replicable;
      pendingReplicable.IsStreaming = false;
      if (!this.m_pendingReplicables.ContainsKey(networkID))
      {
        this.m_pendingReplicables.Add(networkID, pendingReplicable);
        this.m_pendingReplicables.ApplyAdditionsAndModifications();
      }
      replicable.OnLoad(packet.BitStream, (Action<bool>) (loaded => this.SetReplicableReady(networkID, parentID, replicable, loaded)));
      packet.Return();
    }

    public void ProcessReplicationDestroy(MyPacket packet)
    {
      NetworkId networkId = packet.BitStream.ReadNetworkId();
      MyPendingReplicable pendingReplicable;
      if (!this.m_pendingReplicables.TryGetValue(networkId, out pendingReplicable))
      {
        if (this.GetObjectByNetworkId(networkId) is IMyReplicable objectByNetworkId)
        {
          using (this.m_tmp)
          {
            this.m_replicables.GetAllChildren(objectByNetworkId, (List<IMyReplicable>) this.m_tmp);
            foreach (IMyReplicable replicable in (List<IMyReplicable>) this.m_tmp)
              this.ReplicableDestroy(replicable);
            this.ReplicableDestroy(objectByNetworkId);
          }
        }
      }
      else
      {
        this.PendingReplicableDestroy(networkId, pendingReplicable);
        this.m_pendingReplicables.ApplyRemovals();
      }
      this.m_destroyedReplicables.Add(networkId);
      packet.Return();
    }

    private void PendingReplicableDestroy(
      NetworkId networkID,
      MyPendingReplicable pendingReplicable,
      bool calledByParent = false)
    {
      if (pendingReplicable.DependentReplicables != null)
      {
        foreach (KeyValuePair<NetworkId, MyPendingReplicable> dependentReplicable in pendingReplicable.DependentReplicables)
          this.PendingReplicableDestroy(dependentReplicable.Key, dependentReplicable.Value, true);
      }
      foreach (KeyValuePair<NetworkId, MyPendingReplicable> pendingReplicable1 in this.m_pendingReplicables)
      {
        if (pendingReplicable1.Value.ParentID.Equals(networkID))
          this.PendingReplicableDestroy(pendingReplicable1.Key, pendingReplicable1.Value);
      }
      MyPendingReplicable pendingReplicable2;
      if (!calledByParent && this.m_pendingReplicables.TryGetValue(pendingReplicable.ParentID, out pendingReplicable2) && pendingReplicable2.DependentReplicables != null)
        pendingReplicable2.DependentReplicables.Remove(networkID);
      using (this.m_tmpGroups)
      {
        pendingReplicable.Replicable.GetStateGroups((List<IMyStateGroup>) this.m_tmpGroups);
        foreach (IMyStateGroup tmpGroup in (List<IMyStateGroup>) this.m_tmpGroups)
        {
          if (tmpGroup != null)
          {
            if (tmpGroup.IsStreaming)
            {
              this.RemoveNetworkedObject((IMyNetObject) tmpGroup);
              --this.PendingStreamingRelicablesCount;
            }
            tmpGroup.Destroy();
          }
        }
      }
      this.m_eventBuffer.RemoveEvents(networkID);
      this.m_pendingReplicables.Remove(networkID);
    }

    private void ReplicableDestroy(IMyReplicable replicable, bool removeNetworkObject = true)
    {
      NetworkId networkId;
      if (this.TryGetNetworkIdByObject((IMyNetObject) replicable, out networkId))
      {
        this.m_pendingReplicables.Remove(networkId);
        this.m_pendingReplicables.ApplyRemovals();
        this.m_eventBuffer.RemoveEvents(networkId);
      }
      using (this.m_tmpGroups)
      {
        replicable.GetStateGroups((List<IMyStateGroup>) this.m_tmpGroups);
        foreach (IMyStateGroup tmpGroup in (List<IMyStateGroup>) this.m_tmpGroups)
        {
          if (tmpGroup != null)
          {
            if (removeNetworkObject)
              this.RemoveNetworkedObject((IMyNetObject) tmpGroup);
            tmpGroup.Destroy();
          }
        }
      }
      if (removeNetworkObject)
        this.RemoveNetworkedObject((IMyNetObject) replicable);
      replicable.OnDestroyClient();
      this.m_replicables.RemoveHierarchy(replicable);
    }

    public void ProcessReplicationIslandDone(MyPacket packet)
    {
      byte index1 = packet.BitStream.ReadByte();
      int num = packet.BitStream.ReadInt32();
      Dictionary<long, MatrixD> matrices = new Dictionary<long, MatrixD>();
      for (int index2 = 0; index2 < num; ++index2)
      {
        long key = packet.BitStream.ReadInt64();
        Vector3D vector3D = packet.BitStream.ReadVector3D();
        Quaternion quaternion = packet.BitStream.ReadQuaternion();
        if (key != 0L)
        {
          MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(quaternion);
          fromQuaternion.Translation = vector3D;
          matrices.Add(key, fromQuaternion);
        }
      }
      this.m_callback.SetIslandDone(index1, matrices);
      packet.Return();
    }

    public void OnServerData(MyPacket packet)
    {
      long bitPosition = packet.BitStream.BitPosition;
      try
      {
        this.SerializeTypeTable(packet.BitStream);
        this.m_hasTypeTable = true;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Server sent bad data!");
        byte[] bytes = new byte[packet.BitStream.ByteLength];
        packet.BitStream.ReadBytes(bytes, 0, packet.BitStream.ByteLength);
        MyLog.Default.WriteLine("Server Data: " + string.Join<byte>(", ", (IEnumerable<byte>) bytes));
        MyLog.Default.WriteLine(ex);
        this.m_failureCallback("Failed to connect to server. See log for details.");
      }
      packet.Return();
    }

    public MyTimeSpan Ping => !this.UseSmoothPing ? this.m_ping : this.m_smoothPing;

    public float? ServerReplicationRange { get; private set; }

    public override MyTimeSpan GetSimulationUpdateTime()
    {
      MyTimeSpan updateTime = this.m_callback.GetUpdateTime();
      if (this.m_clientStartTimeStamp == MyTimeSpan.Zero)
        this.m_clientStartTimeStamp = updateTime;
      return updateTime - this.m_clientStartTimeStamp;
    }

    public override void UpdateBefore()
    {
    }

    public override void UpdateAfter()
    {
      if (!this.m_clientReady || !this.m_hasTypeTable || this.m_clientState == null)
        return;
      MyStatsGraph.ProfileAdvanced(true);
      MyStatsGraph.Begin("Replication client update", 0, nameof (UpdateAfter), 494, "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      this.UpdatePingSmoothing();
      MyStatsGraph.CustomTime("Ping", (float) this.m_ping.Milliseconds, "{0} ms", member: nameof (UpdateAfter), line: 497, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("SmoothPing", (float) this.m_smoothPing.Milliseconds, "{0} ms", member: nameof (UpdateAfter), line: 498, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      switch (MyReplicationClient.SynchronizationTimingType)
      {
        case MyReplicationClient.TimingType.ServerTimestep:
          this.Timestamp = this.UpdateServerTimestep();
          break;
        case MyReplicationClient.TimingType.LastServerTime:
          this.Timestamp = this.UpdateLastServerTime();
          break;
      }
      MyStatsGraph.End(member: nameof (UpdateAfter), line: 512, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.ProfileAdvanced(false);
      if (MyReplicationClient.StressSleep.X > 0)
        Thread.Sleep(MyReplicationClient.StressSleep.Z != 0 ? (int) (Math.Sin(this.GetSimulationUpdateTime().Milliseconds * Math.PI / (double) MyReplicationClient.StressSleep.Z) * (double) MyReplicationClient.StressSleep.Y + (double) MyReplicationClient.StressSleep.X) : MyRandom.Instance.Next(MyReplicationClient.StressSleep.X, MyReplicationClient.StressSleep.Y));
      if ((MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()) - this.m_lastServerTimeStampReceivedTime).Seconds <= 5.0 || this.m_clientPaused)
        return;
      this.m_clientPaused = true;
      this.m_callback.PauseClient(true);
    }

    private MyTimeSpan UpdateLastServerTime()
    {
      MyTimeSpan myTimeSpan1 = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      MyTimeSpan myTimeSpan2 = myTimeSpan1 - this.m_lastClientTime;
      this.m_lastClientTime = myTimeSpan1;
      MyTimeSpan myTimeSpan3 = myTimeSpan1 - this.m_lastServerTimeStampReceivedTime;
      MyTimeSpan myTimeSpan4 = this.m_lastServerTimestamp + myTimeSpan3;
      MyTimeSpan myTimeSpan5 = myTimeSpan4 - this.m_lastServerTime;
      this.m_lastServerTime = myTimeSpan4;
      MyStatsGraph.CustomTime("ClientTimeDelta", (float) myTimeSpan2.Milliseconds, "{0} ms", member: nameof (UpdateLastServerTime), line: 558, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("ServerTimeDelta", (float) myTimeSpan5.Milliseconds, "{0} ms", member: nameof (UpdateLastServerTime), line: 559, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("TimeDeltaFromPacket", (float) myTimeSpan3.Milliseconds, "{0} ms", member: nameof (UpdateLastServerTime), line: 560, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      if (myTimeSpan3.Seconds > 1.0)
        return this.Timestamp;
      float seconds = (float) (this.Timestamp - myTimeSpan4).Seconds;
      MyStatsGraph.CustomTime("ServerClientTimeDiff", (float) (-(double) seconds * 1000.0), "{0} ms", member: nameof (UpdateLastServerTime), line: 572, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      double clientSimulationRatio = (double) this.m_callback.GetClientSimulationRatio();
      float serverSimulationRatio = this.m_callback.GetServerSimulationRatio();
      double num1 = (double) serverSimulationRatio;
      float val1 = (float) (int) (clientSimulationRatio / num1 * 50.0) / 50f;
      if ((double) val1 > 0.800000011920929)
        val1 = 1f;
      float newValue = seconds + (float) (0.0599999986588955 * (0.600000023841858 - (double) val1));
      if ((double) newValue < -1.0)
      {
        MyTimeSpan myTimeSpan6 = myTimeSpan3.Seconds < 1.0 ? myTimeSpan4 : this.Timestamp;
        this.m_clientStartTimeStamp -= this.Timestamp - this.GetSimulationUpdateTime();
        return myTimeSpan6;
      }
      if ((double) newValue > 0.200000002980232)
      {
        this.m_clientStartTimeStamp -= this.Timestamp - this.GetSimulationUpdateTime();
        return this.Timestamp;
      }
      this.m_timeDiffSmoothed = MathHelper.Smooth(newValue, this.m_timeDiffSmoothed);
      float timeDiffSmoothed = this.m_timeDiffSmoothed;
      float num2 = MathHelper.Clamp(Math.Sign(timeDiffSmoothed) <= 0 ? (float) Math.Exp(Math.Max(-(double) timeDiffSmoothed - 0.04, 0.0) * 2.0) : 1f / (float) Math.Exp((double) timeDiffSmoothed * 6.0), 0.1f, 10f);
      float customTime1 = Math.Max(Math.Min(val1, 1f), 0.1f);
      float customTime2 = this.m_simulationTimeStep / customTime1 * num2;
      MyStatsGraph.CustomTime("TimeAdvance", customTime2, "{0} ms", member: nameof (UpdateLastServerTime), line: 632, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("ServerClientSimRatio", customTime1, "{0}", member: nameof (UpdateLastServerTime), line: 633, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyTimeSpan myTimeSpan7 = this.Timestamp + MyTimeSpan.FromMilliseconds((double) customTime2);
      this.m_clientStartTimeStamp -= myTimeSpan7 - this.GetSimulationUpdateTime();
      float num3 = this.m_simulationTimeStep / serverSimulationRatio - this.m_simulationTimeStep;
      if ((double) num3 > 0.0)
        this.m_callback.SetNextFrameDelayDelta(num3);
      MyStatsGraph.CustomTime("FrameDelayTime", num3, "{0} ms", member: nameof (UpdateLastServerTime), line: 648, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      return myTimeSpan7;
    }

    private MyTimeSpan UpdateServerTimestep()
    {
      MyTimeSpan simulationUpdateTime = this.GetSimulationUpdateTime();
      MyTimeSpan myTimeSpan1 = this.UseSmoothPing ? this.m_smoothPing : this.m_ping;
      double num1 = -myTimeSpan1.Milliseconds * (double) this.m_callback.GetServerSimulationRatio();
      double num2 = simulationUpdateTime.Milliseconds - this.m_lastServerTimestamp.Milliseconds;
      double num3 = num2 + num1;
      int num4 = 0;
      MyTimeSpan myTimeSpan2 = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      MyTimeSpan myTimeSpan3 = myTimeSpan2 - this.m_lastTime;
      double val1 = num3 - (double) this.m_simulationTimeStep;
      double num5 = Math.Min(myTimeSpan3.Seconds / (double) this.SmoothCorrectionAmplitude, 1.0);
      this.m_correctionSmooth = MyTimeSpan.FromMilliseconds(val1 * num5 + this.m_correctionSmooth.Milliseconds * (1.0 - num5));
      int num6 = (int) ((double) this.m_simulationTimeStep * 2.0 / (double) this.m_callback.GetServerSimulationRatio());
      double num7 = Math.Min(val1, (double) num6);
      this.m_correctionSmooth = MyTimeSpan.FromMilliseconds(Math.Min(this.m_correctionSmooth.Milliseconds, (double) num6));
      MyStatsGraph.CustomTime("Correction", (float) num7, "{0} ms", member: nameof (UpdateServerTimestep), line: 674, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("SmoothCorrection", (float) this.m_correctionSmooth.Milliseconds, "{0} ms", member: nameof (UpdateServerTimestep), line: 675, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      if (num2 < -80.0 || num2 > 500.0 + myTimeSpan1.Milliseconds && !this.m_predictionReset || num2 > 5000.0)
      {
        this.m_clientStartTimeStamp = MyTimeSpan.FromMilliseconds(this.m_clientStartTimeStamp.Milliseconds + num2);
        simulationUpdateTime = this.GetSimulationUpdateTime();
        this.m_correctionSmooth = MyTimeSpan.Zero;
        if (this.m_predictionReset && num2 > 5000.0)
          this.TimestampReset();
        if (!MyCompilationSymbols.EnableNetworkPositionTracking)
          ;
      }
      else
      {
        int num8 = num2 >= 0.0 ? (this.UseSmoothCorrection ? (int) this.m_correctionSmooth.Milliseconds : (int) num7) : (int) num7;
        if ((double) (this.LastMessageFromServer - DateTime.UtcNow).Seconds < 1.0)
        {
          if (num2 < 0.0)
          {
            num4 = num8;
            this.m_callback.SetNextFrameDelayDelta((float) num4);
          }
          else if (Math.Abs(num8) > this.TimestampCorrectionMinimum)
          {
            num4 = (Math.Abs(num8) - this.TimestampCorrectionMinimum) * Math.Sign(num8);
            this.m_callback.SetNextFrameDelayDelta((float) num4);
          }
        }
      }
      MyTimeSpan myTimeSpan4 = simulationUpdateTime - this.Timestamp;
      MyStatsGraph.CustomTime("GameTimeDelta", (float) myTimeSpan4.Milliseconds, "{0} ms", member: nameof (UpdateServerTimestep), line: 712, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      MyStatsGraph.CustomTime("RealTimeDelta", (float) myTimeSpan3.Milliseconds, "{0} ms", member: nameof (UpdateServerTimestep), line: 713, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
      object[] objArray = new object[22];
      objArray[0] = (object) "realtime delta: ";
      objArray[1] = (object) myTimeSpan3;
      objArray[2] = (object) ", client: ";
      objArray[3] = (object) this.Timestamp;
      objArray[4] = (object) ", server: ";
      objArray[5] = (object) this.m_lastServerTimestamp;
      objArray[6] = (object) ", diff: ";
      objArray[7] = (object) num2.ToString("##.#");
      objArray[8] = (object) " => ";
      myTimeSpan4 = this.Timestamp;
      objArray[9] = (object) (myTimeSpan4.Milliseconds - this.m_lastServerTimestamp.Milliseconds).ToString("##.#");
      objArray[10] = (object) ", Ping: ";
      objArray[11] = (object) this.m_ping.Milliseconds.ToString("##.#");
      objArray[12] = (object) " / ";
      objArray[13] = (object) this.m_smoothPing.Milliseconds.ToString("##.#");
      objArray[14] = (object) "ms, Correction ";
      objArray[15] = (object) num7;
      objArray[16] = (object) " / ";
      objArray[17] = (object) this.m_correctionSmooth.Milliseconds;
      objArray[18] = (object) " / ";
      objArray[19] = (object) num4;
      objArray[20] = (object) ", ratio ";
      objArray[21] = (object) this.m_callback.GetServerSimulationRatio();
      string.Concat(objArray);
      this.m_lastTime = myTimeSpan2;
      return simulationUpdateTime;
    }

    public override void UpdateClientStateGroups()
    {
      this.m_stateGroupsForUpdate.ApplyChanges();
      foreach (IMyStateGroup myStateGroup in this.m_stateGroupsForUpdate)
      {
        myStateGroup.ClientUpdate(this.Timestamp);
        if (!myStateGroup.NeedsUpdate)
          this.m_stateGroupsForUpdate.Remove(myStateGroup);
      }
    }

    public override void Simulate() => this.m_callback.UpdateSnapshotCache();

    private void TimestampReset()
    {
      this.m_stateGroupsForUpdate.ApplyChanges();
      foreach (IMyStateGroup myStateGroup in this.m_stateGroupsForUpdate)
        myStateGroup.Reset(false, this.Timestamp);
    }

    public override void SendUpdate()
    {
      MyPacketDataBitStreamBase streamPacketData1 = this.m_callback.GetBitStreamPacketData();
      streamPacketData1.Stream.WriteByte(this.m_lastStateSyncPacketId);
      streamPacketData1.Stream.WriteBool(this.m_receivedStreamingPackets);
      this.m_receivedStreamingPackets = false;
      streamPacketData1.Stream.WriteByte(this.m_lastStreamingPacketId);
      byte count = (byte) this.m_acks.Count;
      streamPacketData1.Stream.WriteByte(count);
      for (int index = 0; index < (int) count; ++index)
        streamPacketData1.Stream.WriteByte(this.m_acks[index]);
      streamPacketData1.Stream.Terminate();
      this.m_acks.Clear();
      this.m_callback.SendClientAcks((IPacketData) streamPacketData1);
      MyPacketDataBitStreamBase streamPacketData2 = this.m_callback.GetBitStreamPacketData();
      ++this.m_clientPacketId;
      streamPacketData2.Stream.WriteByte(this.m_clientPacketId);
      streamPacketData2.Stream.WriteDouble(this.Timestamp.Milliseconds);
      streamPacketData2.Stream.WriteDouble(MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()).Milliseconds);
      int num = MyCompilationSymbols.EnableNetworkPacketTracking ? 1 : 0;
      this.m_clientState.Serialize(streamPacketData2.Stream, false);
      streamPacketData2.Stream.Terminate();
      this.m_callback.SendClientUpdate((IPacketData) streamPacketData2);
    }

    protected override bool DispatchBlockingEvent(
      IPacketData data,
      CallSite site,
      EndpointId recipient,
      IMyNetObject eventInstance,
      Vector3D? position,
      IMyNetObject blockedNetObj)
    {
      return this.DispatchEvent(data, site, recipient, eventInstance, position);
    }

    public override void UpdateStatisticsData(
      int outgoing,
      int incoming,
      int tamperred,
      float gcMemory,
      float processMemory)
    {
    }

    public override MyPacketStatistics ClearClientStatistics()
    {
      MyPacketStatistics clientStats = this.m_clientStats;
      this.m_clientStats.Reset();
      return clientStats;
    }

    public override MyPacketStatistics ClearServerStatistics()
    {
      MyPacketStatistics serverStats = this.m_serverStats;
      this.m_serverStats.Reset();
      return serverStats;
    }

    protected override bool DispatchEvent(
      IPacketData data,
      CallSite site,
      EndpointId target,
      IMyNetObject instance,
      Vector3D? position)
    {
      if (site.HasServerFlag)
      {
        this.m_callback.SendEvent(data, site.IsReliable);
      }
      else
      {
        int num = site.HasClientFlag ? 1 : 0;
        data.Return();
      }
      return false;
    }

    private bool IsBlocked(NetworkId networkId, NetworkId blockedNetId)
    {
      bool flag1 = this.m_pendingReplicables.ContainsKey(networkId) || this.m_pendingReplicables.ContainsKey(blockedNetId);
      bool flag2 = this.GetObjectByNetworkId(networkId) == null || blockedNetId.IsValid && this.GetObjectByNetworkId(blockedNetId) == null;
      return networkId.IsValid && flag1 | flag2;
    }

    protected override void OnEvent(
      MyPacketDataBitStreamBase data,
      NetworkId networkId,
      NetworkId blockedNetId,
      uint eventId,
      EndpointId sender,
      Vector3D? position)
    {
      this.LastMessageFromServer = DateTime.UtcNow;
      bool flag = this.m_eventBuffer.ContainsEvents(networkId) || this.m_eventBuffer.ContainsEvents(blockedNetId);
      if (this.IsBlocked(networkId, blockedNetId) | flag)
      {
        this.m_eventBuffer.EnqueueEvent(data, networkId, blockedNetId, eventId, sender, position);
        if (!blockedNetId.IsValid)
          return;
        this.m_eventBuffer.EnqueueBarrier(blockedNetId, networkId);
      }
      else
        base.OnEvent(data, networkId, blockedNetId, eventId, sender, position);
    }

    protected override void OnEvent(
      MyPacketDataBitStreamBase data,
      CallSite site,
      object obj,
      IMyNetObject sendAs,
      Vector3D? position,
      EndpointId source)
    {
      this.LastMessageFromServer = DateTime.UtcNow;
      this.Invoke(site, data.Stream, obj, source, (MyClientStateBase) null, false);
      data.Return();
    }

    public void OnServerStateSync(MyPacket packet)
    {
      try
      {
        this.LastMessageFromServer = DateTime.UtcNow;
        bool flag = packet.BitStream.ReadBool();
        byte num1 = packet.BitStream.ReadByte();
        if (!flag)
        {
          MyPacketTracker.OrderType type = this.m_serverTracker.Add(num1);
          this.m_clientStats.Update(type);
          if (type == MyPacketTracker.OrderType.Duplicate)
            return;
          this.m_lastStateSyncPacketId = num1;
          if (!this.m_acks.Contains(num1))
            this.m_acks.Add(num1);
        }
        else
        {
          this.m_lastStreamingPacketId = num1;
          this.m_receivedStreamingPackets = true;
        }
        MyPacketStatistics statistics = new MyPacketStatistics();
        statistics.Read(packet.BitStream);
        this.m_serverStats.Add(statistics);
        MyTimeSpan myTimeSpan1 = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
        if (this.m_lastServerTimestamp < myTimeSpan1)
        {
          this.m_lastServerTimestamp = myTimeSpan1;
          this.m_lastServerTimeStampReceivedTime = packet.ReceivedTime;
        }
        MyTimeSpan myTimeSpan2 = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
        if (myTimeSpan2 > this.m_lastClientTimestamp)
          this.m_lastClientTimestamp = myTimeSpan2;
        double milliseconds = packet.BitStream.ReadDouble();
        if (milliseconds > 0.0)
        {
          MyTimeSpan ping = packet.ReceivedTime - MyTimeSpan.FromMilliseconds(milliseconds);
          if (ping.Milliseconds < 1000.0)
            this.SetPing(ping);
        }
        MyTimeSpan serverTimestamp = myTimeSpan1;
        this.m_callback.ReadCustomState(packet.BitStream);
        while (packet.BitStream.BytePosition + 2 < packet.BitStream.ByteLength)
        {
          if (!packet.BitStream.CheckTerminator())
          {
            MyLog.Default.WriteLine("OnServerStateSync: Invalid stream terminator");
            return;
          }
          IMyStateGroup objectByNetworkId = this.GetObjectByNetworkId(packet.BitStream.ReadNetworkId()) as IMyStateGroup;
          int num2 = !flag ? (int) packet.BitStream.ReadInt16() : packet.BitStream.ReadInt32();
          if (objectByNetworkId == null)
            packet.BitStream.SetBitPositionRead(packet.BitStream.BitPosition + (long) num2);
          else if (flag != objectByNetworkId.IsStreaming)
          {
            MyLog.Default.WriteLine("received streaming flag but group is not streaming !");
            packet.BitStream.SetBitPositionRead(packet.BitStream.BitPosition + (long) num2);
          }
          else
          {
            int bytePosition = packet.BitStream.BytePosition;
            long bitPosition1 = packet.BitStream.BitPosition;
            if (MyCompilationSymbols.EnableNetworkPacketTracking)
            {
              long bitPosition2 = packet.BitStream.BitPosition;
              objectByNetworkId.Owner.ToString();
              string fullName = objectByNetworkId.GetType().FullName;
            }
            MyStatsGraph.Begin(objectByNetworkId.GetType().Name, member: nameof (OnServerStateSync), line: 1027, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
            objectByNetworkId.Serialize(packet.BitStream, new MyClientInfo(), serverTimestamp, this.m_lastClientTimestamp, num1, 0, (HashSet<string>) null);
            MyStatsGraph.End(new float?((float) (packet.BitStream.ByteLength - bytePosition)), member: nameof (OnServerStateSync), line: 1029, file: "E:\\Repo3\\Sources\\VRage\\Replication\\MyReplicationClient.cs");
          }
        }
        if (!packet.BitStream.CheckTerminator())
          MyLog.Default.WriteLine("OnServerStateSync: Invalid stream terminator");
        if (!this.m_clientPaused)
          return;
        this.m_clientPaused = false;
        this.m_callback.PauseClient(false);
      }
      finally
      {
        packet.Return();
      }
    }

    private void SetPing(MyTimeSpan ping)
    {
      this.m_ping = ping;
      this.UpdatePingSmoothing();
      this.m_callback.SetPing((long) this.m_smoothPing.Milliseconds);
      this.SetClientStatePing((short) this.m_smoothPing.Milliseconds);
    }

    private void UpdatePingSmoothing()
    {
      MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      double num = Math.Min((myTimeSpan - this.m_lastPingTime).Seconds / (double) this.PingSmoothFactor, 1.0);
      this.m_smoothPing = MyTimeSpan.FromMilliseconds(this.m_ping.Milliseconds * num + this.m_smoothPing.Milliseconds * (1.0 - num));
      this.m_lastPingTime = myTimeSpan;
    }

    public JoinResultMsg OnJoinResult(MyPacket packet) => MySerializer.CreateAndRead<JoinResultMsg>(packet.BitStream);

    public ServerDataMsg OnWorldData(MyPacket packet) => MySerializer.CreateAndRead<ServerDataMsg>(packet.BitStream);

    public ChatMsg OnChatMessage(MyPacket packet) => MySerializer.CreateAndRead<ChatMsg>(packet.BitStream);

    public ConnectedClientDataMsg OnClientConnected(MyPacket packet) => MySerializer.CreateAndRead<ConnectedClientDataMsg>(packet.BitStream);

    public void SendClientConnected(ref ConnectedClientDataMsg msg)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<ConnectedClientDataMsg>(streamPacketData.Stream, ref msg);
      this.m_callback.SendConnectRequest((IPacketData) streamPacketData);
    }

    public void SendClientReady(ref ClientReadyDataMsg msg)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      MySerializer.Write<ClientReadyDataMsg>(streamPacketData.Stream, ref msg);
      this.m_callback.SendClientReady(streamPacketData);
      this.OnLocalClientReady();
    }

    public void AddToUpdates(IMyStateGroup group) => this.m_stateGroupsForUpdate.Add(group);

    public void RequestReplicable(long entityId, byte layer, bool add)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      streamPacketData.Stream.WriteInt64(entityId);
      streamPacketData.Stream.WriteBool(add);
      if (add)
        streamPacketData.Stream.WriteByte(layer);
      this.m_callback.SendReplicableRequest((IPacketData) streamPacketData);
    }

    public void SetClientStatePing(short ping) => this.m_clientState.Ping = ping;

    public void SetClientReplicationRange(float? range) => this.m_clientState.ReplicationRange = range;

    public override string GetMultiplayerStat()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string multiplayerStat = base.GetMultiplayerStat();
      stringBuilder.Append(multiplayerStat);
      stringBuilder.AppendLine("Pending Replicables:");
      foreach (KeyValuePair<NetworkId, MyPendingReplicable> pendingReplicable in this.m_pendingReplicables)
      {
        string str = "   NetworkId: " + pendingReplicable.Key.ToString() + ", IsStreaming: " + pendingReplicable.Value.IsStreaming.ToString();
        stringBuilder.AppendLine(str);
      }
      stringBuilder.Append(this.m_eventBuffer.GetEventsBufferStat());
      return stringBuilder.ToString();
    }

    public enum TimingType
    {
      None,
      ServerTimestep,
      LastServerTime,
    }
  }
}
