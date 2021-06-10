// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyClient
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.IO;
using VRage.Collections;
using VRage.Library;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRageMath;

namespace VRage.Network
{
  internal class MyClient
  {
    public readonly MyClientStateBase State;
    private readonly IReplicationServerCallback m_callback;
    public float PriorityMultiplier = 1f;
    public bool IsReady;
    public bool PlayerControllableUsesPredictedPhysics = true;
    private MyTimeSpan m_lastClientRealtime;
    private MyTimeSpan m_lastClientTimestamp;
    private MyTimeSpan m_lastStateSyncTimeStamp;
    private byte m_stateSyncPacketId;
    private byte m_streamingPacketId;
    private byte m_lastClientStreamingAck;
    public readonly Dictionary<IMyReplicable, byte> PermanentReplicables = new Dictionary<IMyReplicable, byte>();
    public readonly HashSet<IMyReplicable> CrucialReplicables = new HashSet<IMyReplicable>();
    public readonly MyConcurrentDictionary<IMyReplicable, MyReplicableClientData> Replicables = new MyConcurrentDictionary<IMyReplicable, MyReplicableClientData>((IEqualityComparer<IMyReplicable>) InstanceComparer<IMyReplicable>.Default);
    public int PendingReplicables;
    public bool WantsBatchCompleteConfirmation = true;
    public readonly MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker> BlockedReplicables = new MyConcurrentDictionary<IMyReplicable, MyReplicationServer.MyDestroyBlocker>();
    public readonly Dictionary<IMyStateGroup, MyStateDataEntry> StateGroups = new Dictionary<IMyStateGroup, MyStateDataEntry>((IEqualityComparer<IMyStateGroup>) InstanceComparer<IMyStateGroup>.Default);
    public readonly FastPriorityQueue<MyStateDataEntry> DirtyQueue = new FastPriorityQueue<MyStateDataEntry>(1024);
    private readonly HashSet<string> m_clientCachedData = new HashSet<string>();
    public int? PCULimit;
    public MyPacketStatistics Statistics;
    public MyClient.UpdateLayer[] UpdateLayers;
    public readonly Dictionary<IMyReplicable, MyClient.UpdateLayer> ReplicableToLayer = new Dictionary<IMyReplicable, MyClient.UpdateLayer>();
    public int LastEnabledLayer;
    private readonly List<MyClient.MyOrderedPacket> m_incomingBuffer = new List<MyClient.MyOrderedPacket>();
    private bool m_incomingBuffering = true;
    private byte m_lastProcessedClientPacketId = byte.MaxValue;
    private readonly MyPacketTracker m_clientTracker = new MyPacketTracker();
    private MyTimeSpan m_lastReceivedTimeStamp = MyTimeSpan.Zero;
    private const int MINIMUM_INCOMING_BUFFER = 4;
    private bool m_enablePlayoutDelayBuffer;
    private int m_orderedCounter;
    private const byte OUT_OF_ORDER_RESET_PROTECTION = 64;
    private const byte OUT_OF_ORDER_ACCEPT_THRESHOLD = 6;
    private byte m_lastReceivedAckId;
    private bool m_waitingForReset;
    private readonly List<IMyStateGroup>[] m_pendingStateSyncAcks = System.Linq.Enumerable.Range(0, 512).Select<int, List<IMyStateGroup>>((Func<int, List<IMyStateGroup>>) (s => new List<IMyStateGroup>(8))).ToArray<List<IMyStateGroup>>();
    private static readonly MyTimeSpan MAXIMUM_PACKET_GAP = MyTimeSpan.FromSeconds(0.400000005960464);

    public MyClient(MyClientStateBase emptyState, IReplicationServerCallback callback)
    {
      this.m_callback = callback;
      this.State = emptyState;
      this.InitLayers();
    }

    private void InitLayers()
    {
      this.UpdateLayers = new MyClient.UpdateLayer[MyLayers.UpdateLayerDescriptors.Count];
      for (int index = 0; index < MyLayers.UpdateLayerDescriptors.Count; ++index)
      {
        MyLayers.UpdateLayerDesc updateLayerDescriptor = MyLayers.UpdateLayerDescriptors[index];
        this.UpdateLayers[index] = new MyClient.UpdateLayer(updateLayerDescriptor, index, index + 1);
      }
      this.LastEnabledLayer = this.UpdateLayers.Length - 1;
    }

    public MyClient.UpdateLayer CalculateLayerOfReplicable(IMyReplicable rep)
    {
      BoundingBoxD aabb = rep.GetAABB();
      if (!this.State.Position.HasValue)
        return (MyClient.UpdateLayer) null;
      for (int index = 0; index < this.UpdateLayers.Length; ++index)
      {
        MyClient.UpdateLayer updateLayer = this.UpdateLayers[index];
        if (new BoundingBoxD(this.State.Position.Value - new Vector3D((double) updateLayer.Descriptor.Radius), this.State.Position.Value + new Vector3D((double) updateLayer.Descriptor.Radius)).Intersects(aabb))
          return updateLayer;
      }
      return (MyClient.UpdateLayer) null;
    }

    public bool ForcePlayoutDelayBuffer { get; set; }

    public bool UsePlayoutDelayBufferForCharacter { get; set; }

    public bool UsePlayoutDelayBufferForJetpack { get; set; }

    public bool UsePlayoutDelayBufferForGrids { get; set; }

    private bool UsePlayoutDelayBuffer
    {
      get
      {
        if (!this.m_enablePlayoutDelayBuffer && !this.ForcePlayoutDelayBuffer)
          return false;
        if (this.State.IsControllingCharacter && this.UsePlayoutDelayBufferForCharacter || this.State.IsControllingJetpack && this.UsePlayoutDelayBufferForJetpack)
          return true;
        return this.State.IsControllingGrid && this.UsePlayoutDelayBufferForGrids;
      }
    }

    private void AddIncomingPacketSorted(byte packetId, MyPacket packet)
    {
      MyClient.MyOrderedPacket myOrderedPacket = new MyClient.MyOrderedPacket()
      {
        Id = packetId,
        Packet = packet
      };
      int index = this.m_incomingBuffer.Count - 1;
      while (index >= 0 && (int) packetId < (int) this.m_incomingBuffer[index].Id && (packetId >= (byte) 64 || this.m_incomingBuffer[index].Id <= (byte) 192))
        --index;
      this.m_incomingBuffer.Insert(index + 1, myOrderedPacket);
    }

    private bool ProcessIncomingPacket(
      MyPacket packet,
      bool skipControls,
      MyTimeSpan serverTimeStamp)
    {
      byte id = packet.BitStream.ReadByte();
      this.m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
      this.m_lastClientRealtime = MyTimeSpan.FromMilliseconds(packet.BitStream.ReadDouble());
      this.m_lastReceivedTimeStamp = serverTimeStamp;
      this.Statistics.Update(this.m_clientTracker.Add(id));
      bool flag = (int) id <= (int) this.m_lastProcessedClientPacketId && (this.m_lastProcessedClientPacketId <= (byte) 192 || id >= (byte) 64);
      if (!flag)
        this.m_lastProcessedClientPacketId = id;
      this.State.Serialize(packet.BitStream, flag | skipControls);
      if (!packet.BitStream.CheckTerminator())
        throw new EndOfStreamException("Invalid BitStream terminator");
      return flag;
    }

    private void UpdateIncoming(
      MyTimeSpan serverTimeStamp,
      bool usePlayoutDelayBuffer,
      bool skipAll = false)
    {
      if (this.m_incomingBuffer.Count == 0 || usePlayoutDelayBuffer && this.m_incomingBuffering && (this.m_incomingBuffer.Count < 4 && !skipAll))
      {
        if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
        {
          int count = this.m_incomingBuffer.Count;
        }
        this.m_incomingBuffering = true;
        this.m_lastProcessedClientPacketId = byte.MaxValue;
        this.State.Update();
      }
      else
      {
        if (this.m_incomingBuffering)
          this.m_lastProcessedClientPacketId = (byte) ((uint) this.m_incomingBuffer[0].Id - 1U);
        this.m_incomingBuffering = false;
        string str = "";
        bool flag;
        do
        {
          bool skipControls = this.m_incomingBuffer.Count > 4 | skipAll;
          int num = this.ProcessIncomingPacket(this.m_incomingBuffer[0].Packet, skipControls, serverTimeStamp) ? 1 : 0;
          flag = (num | (skipControls ? 1 : 0)) != 0;
          if (num != 0)
          {
            this.m_enablePlayoutDelayBuffer = true;
            this.m_orderedCounter = 0;
          }
          else if (this.m_enablePlayoutDelayBuffer)
          {
            ++this.m_orderedCounter;
            if (this.m_orderedCounter > 3600)
              this.m_enablePlayoutDelayBuffer = false;
          }
          if (MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking)
          {
            str = this.m_incomingBuffer[0].Id.ToString() + ", " + str;
            if (flag)
              str = "-" + str;
          }
          this.m_incomingBuffer[0].Packet.Return();
          this.m_incomingBuffer.RemoveAt(0);
        }
        while (this.m_incomingBuffer.Count > 4 || !usePlayoutDelayBuffer | flag && this.m_incomingBuffer.Count > 0);
        int num1 = MyCompilationSymbols.EnableNetworkServerIncomingPacketTracking ? 1 : 0;
      }
    }

    private void ClearBufferedIncomingPackets(MyTimeSpan serverTimeStamp)
    {
      if (this.m_incomingBuffer.Count <= 0)
        return;
      this.UpdateIncoming(serverTimeStamp, false, true);
    }

    public void OnClientUpdate(MyPacket packet, MyTimeSpan serverTimeStamp)
    {
      if (!this.UsePlayoutDelayBuffer)
        this.ClearBufferedIncomingPackets(serverTimeStamp);
      long bitPosition = packet.BitStream.BitPosition;
      byte packetId = packet.BitStream.ReadByte();
      packet.BitStream.SetBitPositionRead(bitPosition);
      this.AddIncomingPacketSorted(packetId, packet);
    }

    public void Update(MyTimeSpan serverTimeStamp)
    {
      this.UpdateIncoming(serverTimeStamp, this.UsePlayoutDelayBuffer);
      if (serverTimeStamp > this.m_lastReceivedTimeStamp + MyClient.MAXIMUM_PACKET_GAP)
        this.State.ResetControlledEntityControls();
      this.Statistics.PlayoutDelayBufferSize = (byte) this.m_incomingBuffer.Count;
    }

    private static bool IsPreceding(int currentPacketId, int lastPacketId, int threshold)
    {
      if (lastPacketId < currentPacketId)
        lastPacketId += 256;
      return lastPacketId - currentPacketId <= threshold;
    }

    public bool IsAckAvailable()
    {
      byte num1 = (byte) ((uint) this.m_lastReceivedAckId - 6U);
      byte num2 = (byte) ((uint) this.m_stateSyncPacketId + 1U);
      if (!this.m_waitingForReset && (int) num2 != (int) num1)
        return true;
      this.m_waitingForReset = true;
      return false;
    }

    public void OnClientAcks(MyPacket packet)
    {
      byte num1 = packet.BitStream.ReadByte();
      int num2 = packet.BitStream.ReadBool() ? 1 : 0;
      byte num3 = packet.BitStream.ReadByte();
      if (num2 != 0)
      {
        byte clientStreamingAck = this.m_lastClientStreamingAck;
        do
        {
          ++clientStreamingAck;
          this.RaiseAck(clientStreamingAck, true, true);
        }
        while ((int) clientStreamingAck != (int) num3);
        this.m_lastClientStreamingAck = num3;
      }
      byte num4 = packet.BitStream.ReadByte();
      for (int index = 0; index < (int) num4; ++index)
        this.OnAck(packet.BitStream.ReadByte());
      if (!packet.BitStream.CheckTerminator())
        throw new EndOfStreamException("Invalid BitStream terminator");
      byte num5;
      byte num6;
      if (this.m_waitingForReset)
      {
        this.m_stateSyncPacketId = (byte) ((uint) num1 + 64U);
        this.CheckStateSyncPacketId();
        num5 = (byte) ((uint) this.m_stateSyncPacketId + 1U);
        num6 = (byte) ((uint) num5 - 64U);
        this.m_waitingForReset = false;
      }
      else
      {
        num5 = (byte) ((uint) this.m_stateSyncPacketId + 1U);
        num6 = (byte) ((uint) this.m_lastReceivedAckId - 6U);
      }
      for (byte ackId = num5; (int) ackId != (int) num6; ++ackId)
        this.RaiseAck(ackId, false);
    }

    private void OnAck(byte ackId)
    {
      if (MyClient.IsPreceding((int) ackId, (int) this.m_lastReceivedAckId, 6))
      {
        this.RaiseAck(ackId, true);
      }
      else
      {
        this.RaiseAck(ackId, true);
        this.m_lastReceivedAckId = ackId;
      }
    }

    private void RaiseAck(byte ackId, bool delivered, bool streaming = false)
    {
      List<IMyStateGroup> pendingStateSyncAck = this.m_pendingStateSyncAcks[streaming ? (int) ackId + 256 : (int) ackId];
      foreach (IMyStateGroup key in pendingStateSyncAck)
      {
        if (this.StateGroups.ContainsKey(key))
          key.OnAck(this.State, ackId, delivered);
      }
      pendingStateSyncAck.Clear();
    }

    private void AddPendingAck(byte stateSyncPacketId, IMyStateGroup group) => this.m_pendingStateSyncAcks[(int) stateSyncPacketId].Add(group);

    public void AddPendingAck(IMyStateGroup group, bool streaming) => this.m_pendingStateSyncAcks[streaming ? (int) this.m_streamingPacketId + 256 : (int) this.m_stateSyncPacketId].Add(group);

    public byte CurrentStreamingPacketId => this.m_streamingPacketId;

    public bool IsReplicableReady(IMyReplicable replicable)
    {
      MyReplicableClientData replicableClientData;
      return this.Replicables.TryGetValue(replicable, out replicableClientData) && !replicableClientData.IsPending && !replicableClientData.IsStreaming;
    }

    public bool IsReplicablePending(IMyReplicable replicable)
    {
      MyReplicableClientData replicableClientData;
      if (!this.Replicables.TryGetValue(replicable, out replicableClientData))
        return false;
      return replicableClientData.IsPending || replicableClientData.IsStreaming;
    }

    public bool HasReplicable(IMyReplicable replicable) => this.Replicables.ContainsKey(replicable);

    public bool WritePacketHeader(
      BitStream sendStream,
      bool streaming,
      MyTimeSpan serverTimeStamp,
      out MyTimeSpan clientTimestamp)
    {
      this.m_lastStateSyncTimeStamp = serverTimeStamp;
      if (streaming)
        ++this.m_streamingPacketId;
      else
        ++this.m_stateSyncPacketId;
      if (!this.CheckStateSyncPacketId(streaming))
      {
        clientTimestamp = MyTimeSpan.Zero;
        return false;
      }
      byte num = streaming ? this.m_streamingPacketId : this.m_stateSyncPacketId;
      sendStream.WriteBool(streaming);
      sendStream.WriteByte(num);
      this.Statistics.Write(sendStream, this.m_callback.GetUpdateTime());
      sendStream.WriteDouble(serverTimeStamp.Milliseconds);
      sendStream.WriteDouble(this.m_lastClientTimestamp.Milliseconds);
      this.m_lastClientTimestamp = MyTimeSpan.FromMilliseconds(-1.0);
      sendStream.WriteDouble(this.m_lastClientRealtime.Milliseconds);
      this.m_lastClientRealtime = MyTimeSpan.FromMilliseconds(-1.0);
      this.m_callback.WriteCustomState(sendStream);
      clientTimestamp = serverTimeStamp;
      return true;
    }

    private bool CheckStateSyncPacketId(bool streaming = false)
    {
      byte num1;
      int num2;
      if (streaming)
      {
        num1 = this.m_streamingPacketId;
        num2 = 256;
      }
      else
      {
        num1 = this.m_stateSyncPacketId;
        num2 = 0;
      }
      byte num3 = 0;
      byte num4 = num1;
      while (this.m_pendingStateSyncAcks[(int) num4 + num2].Count != 0)
      {
        ++num4;
        ++num3;
        if ((int) num1 == (int) num4)
        {
          this.Statistics.PendingPackets = num3;
          return false;
        }
      }
      if (streaming)
        this.m_streamingPacketId = num4;
      else
        this.m_stateSyncPacketId = num4;
      this.Statistics.PendingPackets = num3;
      return true;
    }

    public void Serialize(
      IMyStateGroup group,
      BitStream sendStream,
      MyTimeSpan timeStamp,
      int messageBitSize = 2147483647,
      bool streaming = false)
    {
      if (timeStamp == MyTimeSpan.Zero)
        timeStamp = this.State.ClientTimeStamp;
      group.Serialize(sendStream, new MyClientInfo(this), timeStamp, this.m_lastClientTimestamp, streaming ? this.m_streamingPacketId : this.m_stateSyncPacketId, messageBitSize, this.m_clientCachedData);
    }

    public bool SendStateSync(
      MyStateDataEntry stateGroupEntry,
      int mtuBytes,
      ref MyPacketDataBitStreamBase data,
      MyTimeSpan serverTimeStamp)
    {
      if (data == null)
      {
        data = this.m_callback.GetBitStreamPacketData();
        MyTimeSpan clientTimestamp;
        if (!this.WritePacketHeader(data.Stream, false, serverTimeStamp, out clientTimestamp))
        {
          data.Return();
          data = (MyPacketDataBitStreamBase) null;
          return false;
        }
        this.State.ClientTimeStamp = clientTimestamp;
      }
      BitStream stream = data.Stream;
      int messageBitSize = 8 * (mtuBytes - 2);
      long bitPosition1 = stream.BitPosition;
      int num1 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
      stream.Terminate();
      int num2 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
      stream.WriteNetworkId(stateGroupEntry.GroupId);
      if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
      {
        stateGroupEntry.Group.Owner.ToString();
        string fullName = stateGroupEntry.Group.GetType().FullName;
      }
      long bitPosition2 = stream.BitPosition;
      stream.WriteInt16((short) 0);
      long bitPosition3 = stream.BitPosition;
      this.Serialize(stateGroupEntry.Group, stream, MyTimeSpan.Zero, messageBitSize);
      long bitPosition4 = stream.BitPosition;
      stream.SetBitPositionWrite(bitPosition2);
      stream.WriteInt16((short) (bitPosition4 - bitPosition3));
      stream.SetBitPositionWrite(bitPosition4);
      long num3 = stream.BitPosition - bitPosition1;
      int num4 = MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking ? 1 : 0;
      if (num3 > 0L && stream.BitPosition <= (long) messageBitSize)
      {
        this.AddPendingAck(this.m_stateSyncPacketId, stateGroupEntry.Group);
      }
      else
      {
        if (MyCompilationSymbols.EnableNetworkServerOutgoingPacketTracking)
        {
          stateGroupEntry.Group.Owner.ToString();
          string fullName = stateGroupEntry.Group.GetType().FullName;
        }
        stateGroupEntry.Group.OnAck(this.State, this.m_stateSyncPacketId, false);
        stream.SetBitPositionWrite(bitPosition1);
        data.Stream.Terminate();
        this.m_callback.SendStateSync((IPacketData) data, this.State.EndpointId, false);
        data = (MyPacketDataBitStreamBase) null;
      }
      return true;
    }

    private void SendEmptyStateSync(MyTimeSpan serverTimeStamp)
    {
      MyPacketDataBitStreamBase streamPacketData = this.m_callback.GetBitStreamPacketData();
      if (!this.WritePacketHeader(streamPacketData.Stream, false, serverTimeStamp, out MyTimeSpan _))
      {
        streamPacketData.Return();
      }
      else
      {
        streamPacketData.Stream.Terminate();
        this.m_callback.SendStateSync((IPacketData) streamPacketData, this.State.EndpointId, false);
      }
    }

    public void SendUpdate(MyTimeSpan serverTimeStamp)
    {
      if (!(serverTimeStamp > this.m_lastStateSyncTimeStamp + MyClient.MAXIMUM_PACKET_GAP))
        return;
      this.SendEmptyStateSync(serverTimeStamp);
    }

    public bool RemoveCache(IMyReplicable replicable, string storageName) => (replicable == null || !this.Replicables.ContainsKey(replicable)) && this.m_clientCachedData.Remove(storageName);

    public void ResetLayerTimers()
    {
      int num = 0;
      foreach (MyClient.UpdateLayer updateLayer in this.UpdateLayers)
        updateLayer.UpdateTimer = num++;
    }

    public class UpdateLayer
    {
      public readonly MyLayers.UpdateLayerDesc Descriptor;
      public readonly int Index;
      public readonly MyDistributedUpdater<List<IMyReplicable>, IMyReplicable> Sender;
      public HashSet<IMyReplicable> Replicables;
      public int UpdateTimer;
      public int LayerPCU;
      public int PreviousLayersPCU;
      public bool Enabled;

      public int TotalCumulativePCU => this.LayerPCU + this.PreviousLayersPCU;

      public UpdateLayer(MyLayers.UpdateLayerDesc descriptor, int index, int updateTimer)
      {
        this.Descriptor = descriptor;
        this.Index = index;
        this.UpdateTimer = updateTimer;
        this.Replicables = new HashSet<IMyReplicable>();
        this.Sender = new MyDistributedUpdater<List<IMyReplicable>, IMyReplicable>(descriptor.SendInterval);
        this.Enabled = true;
      }
    }

    private struct MyOrderedPacket
    {
      public byte Id;
      public MyPacket Packet;

      public override string ToString() => this.Id.ToString();
    }
  }
}
