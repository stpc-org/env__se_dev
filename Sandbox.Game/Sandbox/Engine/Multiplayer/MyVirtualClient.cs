// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyVirtualClient
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  internal class MyVirtualClient
  {
    private readonly MyClientStateBase m_clientState;
    private readonly List<byte> m_acks = new List<byte>();
    private byte m_lastStateSyncPacketId;
    private byte m_clientPacketId;

    public MyPlayer.PlayerId PlayerId { get; private set; }

    private static MyTransportLayer TransportLayer => MyMultiplayer.Static.SyncLayer.TransportLayer;

    public MyVirtualClient(
      Endpoint endPoint,
      MyClientStateBase clientState,
      MyPlayer.PlayerId playerId)
    {
      this.m_clientState = clientState;
      this.m_clientState.EndpointId = endPoint;
      this.m_clientState.PlayerSerialId = playerId.SerialId;
      this.PlayerId = playerId;
      MyVirtualClient.TransportLayer.Register(MyMessageId.SERVER_DATA, endPoint.Index, new Action<MyPacket>(this.OnServerData));
      MyVirtualClient.TransportLayer.Register(MyMessageId.REPLICATION_CREATE, endPoint.Index, new Action<MyPacket>(this.OnReplicationCreate));
      MyVirtualClient.TransportLayer.Register(MyMessageId.REPLICATION_DESTROY, endPoint.Index, new Action<MyPacket>(this.OnReplicationDestroy));
      MyVirtualClient.TransportLayer.Register(MyMessageId.SERVER_STATE_SYNC, endPoint.Index, new Action<MyPacket>(this.OnServerStateSync));
      MyVirtualClient.TransportLayer.Register(MyMessageId.RPC, endPoint.Index, new Action<MyPacket>(this.OnEvent));
      MyVirtualClient.TransportLayer.Register(MyMessageId.REPLICATION_STREAM_BEGIN, endPoint.Index, new Action<MyPacket>(this.OnReplicationStreamBegin));
      MyVirtualClient.TransportLayer.Register(MyMessageId.JOIN_RESULT, endPoint.Index, new Action<MyPacket>(this.OnJoinResult));
      MyVirtualClient.TransportLayer.Register(MyMessageId.WORLD_DATA, endPoint.Index, new Action<MyPacket>(this.OnWorldData));
      MyVirtualClient.TransportLayer.Register(MyMessageId.CLIENT_CONNECTED, endPoint.Index, new Action<MyPacket>(this.OnClientConnected));
      MyVirtualClient.TransportLayer.Register(MyMessageId.REPLICATION_ISLAND_DONE, endPoint.Index, new Action<MyPacket>(this.OnReplicationIslandDone));
    }

    public void Tick() => this.SendUpdate();

    private void SendUpdate()
    {
      MyPacketDataBitStreamBase streamPacketData1 = MyNetworkWriter.GetBitStreamPacketData();
      BitStream stream1 = streamPacketData1.Stream;
      stream1.WriteByte(this.m_lastStateSyncPacketId);
      byte count = (byte) this.m_acks.Count;
      stream1.WriteByte(count);
      foreach (byte ack in this.m_acks)
        stream1.WriteByte(ack);
      stream1.Terminate();
      this.m_acks.Clear();
      this.SendClientAcks((IPacketData) streamPacketData1);
      MyPacketDataBitStreamBase streamPacketData2 = MyNetworkWriter.GetBitStreamPacketData();
      BitStream stream2 = streamPacketData2.Stream;
      ++this.m_clientPacketId;
      stream2.WriteByte(this.m_clientPacketId);
      stream2.WriteDouble(MyTimeSpan.FromTicks(Stopwatch.GetTimestamp()).Milliseconds);
      stream2.WriteDouble(0.0);
      this.m_clientState.Serialize(stream2, false);
      stream2.Terminate();
      this.SendClientUpdate((IPacketData) streamPacketData2);
    }

    private void OnReplicationIslandDone(MyPacket packet) => packet.Return();

    private void OnClientConnected(MyPacket packet) => throw new NotImplementedException();

    private void OnWorldData(MyPacket packet) => throw new NotImplementedException();

    private void OnJoinResult(MyPacket packet) => throw new NotImplementedException();

    private void OnReplicationCreate(MyPacket packet)
    {
      packet.BitStream.ReadTypeId();
      NetworkId networkId = packet.BitStream.ReadNetworkId();
      MyPacketDataBitStreamBase streamPacketData = MyNetworkWriter.GetBitStreamPacketData();
      streamPacketData.Stream.WriteNetworkId(networkId);
      streamPacketData.Stream.WriteBool(true);
      streamPacketData.Stream.Terminate();
      this.SendReplicableReady((IPacketData) streamPacketData);
      packet.Return();
    }

    private void OnReplicationStreamBegin(MyPacket packet)
    {
      this.OnReplicationCreate(packet);
      packet.Return();
    }

    private void OnEvent(MyPacket packet) => throw new NotImplementedException();

    private void OnServerStateSync(MyPacket packet)
    {
      int num1 = packet.BitStream.ReadBool() ? 1 : 0;
      byte num2 = packet.BitStream.ReadByte();
      if (num1 == 0 && !this.m_acks.Contains(num2))
        this.m_acks.Add(num2);
      this.m_lastStateSyncPacketId = num2;
      packet.Return();
    }

    private void OnReplicationDestroy(MyPacket packet) => packet.Return();

    private void OnServerData(MyPacket packet) => packet.Return();

    private void SendClientUpdate(IPacketData data) => MyVirtualClient.TransportLayer.SendMessage(MyMessageId.CLIENT_UPDATE, data, false, new EndpointId(Sync.ServerId), this.m_clientState.EndpointId.Index);

    private void SendClientAcks(IPacketData data) => MyVirtualClient.TransportLayer.SendMessage(MyMessageId.CLIENT_ACKS, data, true, new EndpointId(Sync.ServerId), this.m_clientState.EndpointId.Index);

    private void SendEvent(IPacketData data, bool reliable) => MyVirtualClient.TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, new EndpointId(Sync.ServerId), this.m_clientState.EndpointId.Index);

    private void SendReplicableReady(IPacketData data) => MyVirtualClient.TransportLayer.SendMessage(MyMessageId.REPLICATION_READY, data, true, new EndpointId(Sync.ServerId), this.m_clientState.EndpointId.Index);

    private void SendConnectRequest(IPacketData data) => MyVirtualClient.TransportLayer.SendMessage(MyMessageId.CLIENT_CONNECTED, data, true, new EndpointId(Sync.ServerId), this.m_clientState.EndpointId.Index);
  }
}
