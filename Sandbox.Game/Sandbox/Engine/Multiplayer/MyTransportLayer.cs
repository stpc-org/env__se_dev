// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyTransportLayer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Profiler;

namespace Sandbox.Engine.Multiplayer
{
  internal class MyTransportLayer
  {
    private static readonly int m_messageTypeCount = (int) (MyEnum<MyMessageId>.Range.Max + (byte) 1);
    private readonly Queue<int>[] m_slidingWindows = Enumerable.Range(0, MyTransportLayer.m_messageTypeCount).Select<int, Queue<int>>((Func<int, Queue<int>>) (s => new Queue<int>(120))).ToArray<Queue<int>>();
    private readonly int[] m_thisFrameTraffic = new int[MyTransportLayer.m_messageTypeCount];
    private bool m_isBuffering;
    private readonly int m_channel;
    private List<MyPacket> m_buffer;
    private byte m_largeMessageParts;
    private int m_largeMessageSize;
    private readonly Dictionary<MyTransportLayer.HandlerId, Action<MyPacket>> m_handlers = new Dictionary<MyTransportLayer.HandlerId, Action<MyPacket>>();

    public bool IsProcessingBuffer { get; private set; }

    public bool IsBuffering
    {
      get => this.m_isBuffering;
      set
      {
        this.m_isBuffering = value;
        if (this.m_isBuffering && this.m_buffer == null)
        {
          this.m_buffer = new List<MyPacket>();
        }
        else
        {
          if (this.m_isBuffering || this.m_buffer == null)
            return;
          this.ProcessBuffer();
          this.m_buffer = (List<MyPacket>) null;
        }
      }
    }

    public Action<ulong> DisconnectPeerOnError { get; set; }

    public MyTransportLayer(int channel)
    {
      this.m_channel = channel;
      this.DisconnectPeerOnError = (Action<ulong>) null;
      MyNetworkReader.SetHandler(channel, new NetworkMessageDelegate(this.HandleMessage), (Action<ulong>) (x => this.DisconnectPeerOnError(x)));
    }

    public void SendFlush(ulong sendTo) => MyNetworkWriter.SendPacket(this.InitSendStream(new EndpointId(sendTo), MyP2PMessageEnum.ReliableWithBuffering, MyMessageId.FLUSH));

    private MyNetworkWriter.MyPacketDescriptor InitSendStream(
      EndpointId endpoint,
      MyP2PMessageEnum msgType,
      MyMessageId msgId,
      byte index = 0)
    {
      MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.GetPacketDescriptor(endpoint, msgType, this.m_channel);
      packetDescriptor.Header.WriteByte((byte) msgId);
      packetDescriptor.Header.WriteByte(index);
      return packetDescriptor;
    }

    public void SendMessage(
      MyMessageId id,
      IPacketData data,
      bool reliable,
      EndpointId endpoint,
      byte index = 0)
    {
      MyNetworkWriter.MyPacketDescriptor packet = this.InitSendStream(endpoint, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
      packet.Data = data;
      MyNetworkWriter.SendPacket(packet);
    }

    public void SendMessage(
      MyMessageId id,
      IPacketData data,
      bool reliable,
      List<EndpointId> endpoints,
      byte index = 0)
    {
      MyNetworkWriter.MyPacketDescriptor packet = this.InitSendStream(EndpointId.Null, reliable ? MyP2PMessageEnum.ReliableWithBuffering : MyP2PMessageEnum.Unreliable, id, index);
      packet.Recipients.AddRange((IEnumerable<EndpointId>) endpoints);
      packet.Data = data;
      MyNetworkWriter.SendPacket(packet);
    }

    private void ProfilePacketStatistics(bool begin)
    {
      if (begin)
      {
        MyStatsGraph.ProfileAdvanced(true);
        MyStatsGraph.ProfilePacketStatistics(true);
      }
      else
      {
        MyStatsGraph.ProfilePacketStatistics(false);
        MyStatsGraph.ProfileAdvanced(false);
      }
    }

    public void Tick()
    {
      int num1 = 0;
      this.ProfilePacketStatistics(true);
      MyStatsGraph.Begin("Average data", member: nameof (Tick), line: 137, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
      for (int index = 0; index < MyTransportLayer.m_messageTypeCount; ++index)
      {
        Queue<int> slidingWindow = this.m_slidingWindows[index];
        slidingWindow.Enqueue(this.m_thisFrameTraffic[index]);
        this.m_thisFrameTraffic[index] = 0;
        while (slidingWindow.Count > 60)
          slidingWindow.Dequeue();
        int num2 = 0;
        foreach (int num3 in slidingWindow)
          num2 += num3;
        if (num2 > 0)
        {
          MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName((MyMessageId) index), member: nameof (Tick), line: 154, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
          MyStatsGraph.End(new float?((float) num2 / 60f), (float) num2 / 1024f, "{0} KB/s", member: nameof (Tick), line: 155, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
        }
        num1 += num2;
      }
      MyStatsGraph.End(new float?((float) num1 / 60f), (float) num1 / 1024f, "{0} KB/s", member: nameof (Tick), line: 159, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
      this.ProfilePacketStatistics(false);
    }

    private void ProcessBuffer()
    {
      try
      {
        this.IsProcessingBuffer = true;
        this.ProfilePacketStatistics(true);
        MyStatsGraph.Begin("Live data", 0, nameof (ProcessBuffer), 169, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
        foreach (MyPacket p in this.m_buffer)
          this.ProcessMessage(p);
        MyStatsGraph.End(member: nameof (ProcessBuffer), line: 174, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
        this.ProfilePacketStatistics(false);
      }
      finally
      {
        this.IsProcessingBuffer = false;
      }
    }

    private void HandleMessage(MyPacket p)
    {
      long bitPosition = p.BitStream.BitPosition;
      MyMessageId myMessageId = (MyMessageId) p.BitStream.ReadByte();
      if (myMessageId == MyMessageId.FLUSH)
      {
        this.ClearBuffer();
        p.Return();
      }
      else
      {
        p.BitStream.SetBitPositionRead(bitPosition);
        if (this.IsBuffering && myMessageId != MyMessageId.JOIN_RESULT && (myMessageId != MyMessageId.WORLD_DATA && myMessageId != MyMessageId.WORLD) && myMessageId != MyMessageId.PLAYER_DATA)
        {
          this.m_buffer.Add(p);
        }
        else
        {
          this.ProfilePacketStatistics(true);
          MyStatsGraph.Begin("Live data", 0, nameof (HandleMessage), 204, "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
          this.ProcessMessage(p);
          MyStatsGraph.End(member: nameof (HandleMessage), line: 206, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
          this.ProfilePacketStatistics(false);
        }
      }
    }

    private void ProcessMessage(MyPacket p)
    {
      MyTransportLayer.HandlerId key;
      key.messageId = (MyMessageId) p.BitStream.ReadByte();
      key.receiverIndex = p.BitStream.ReadByte();
      if ((long) key.messageId < (long) this.m_thisFrameTraffic.Length)
        this.m_thisFrameTraffic[(int) key.messageId] += p.BitStream.ByteLength;
      p.Sender = new Endpoint(p.Sender.Id, key.receiverIndex);
      Action<MyPacket> action;
      if (!this.m_handlers.TryGetValue(key, out action))
        this.m_handlers.TryGetValue(new MyTransportLayer.HandlerId()
        {
          messageId = key.messageId,
          receiverIndex = byte.MaxValue
        }, out action);
      if (action != null)
      {
        int byteLength = p.BitStream.ByteLength;
        MyStatsGraph.Begin(MyEnum<MyMessageId>.GetName(key.messageId), member: nameof (ProcessMessage), line: 233, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
        action(p);
        MyStatsGraph.End(new float?((float) byteLength), member: nameof (ProcessMessage), line: 236, file: "E:\\Repo3\\Sources\\Sandbox.Game\\Engine\\Multiplayer\\MyTransportLayer.cs");
      }
      else
        p.Return();
    }

    public void AddMessageToBuffer(MyPacket packet) => this.m_buffer.Add(packet);

    [Conditional("DEBUG")]
    private void TraceMessage(
      string text,
      string messageText,
      ulong userId,
      long messageSize,
      MyP2PMessageEnum sendType)
    {
      MyNetworkClient client;
      if (MyMultiplayer.Static != null && MyMultiplayer.Static.SyncLayer.Clients.TryGetClient(userId, out client))
      {
        string displayName = client.DisplayName;
      }
      else
        userId.ToString();
      if (sendType == MyP2PMessageEnum.Reliable)
        ;
    }

    public void Register(MyMessageId messageId, byte receiverIndex, Action<MyPacket> handler) => this.m_handlers.Add(new MyTransportLayer.HandlerId()
    {
      messageId = messageId,
      receiverIndex = receiverIndex
    }, handler);

    public void Unregister(MyMessageId messageId, byte receiverIndex) => this.m_handlers.Remove(new MyTransportLayer.HandlerId()
    {
      messageId = messageId,
      receiverIndex = receiverIndex
    });

    private void ClearBuffer()
    {
      if (this.m_buffer == null)
        return;
      foreach (MyPacket myPacket in this.m_buffer)
        myPacket.Return();
      this.m_buffer.Clear();
    }

    public void Clear()
    {
      MyNetworkReader.ClearHandler(2);
      this.ClearBuffer();
    }

    private struct HandlerId : IEquatable<MyTransportLayer.HandlerId>
    {
      public MyMessageId messageId;
      public byte receiverIndex;

      public bool Equals(MyTransportLayer.HandlerId other) => this.messageId == other.messageId && (int) this.receiverIndex == (int) other.receiverIndex;
    }
  }
}
