// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyPacketStatistics
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Replication
{
  public struct MyPacketStatistics
  {
    public int Duplicates;
    public int Drops;
    public int OutOfOrder;
    public int Tamperred;
    public int OutgoingData;
    public int IncomingData;
    public float TimeInterval;
    public byte PendingPackets;
    public float GCMemory;
    public float ProcessMemory;
    public byte PlayoutDelayBufferSize;
    private MyTimeSpan m_nextTime;
    private static readonly MyTimeSpan SEND_TIMEOUT = MyTimeSpan.FromSeconds(0.100000001490116);

    public void Reset()
    {
      this.Duplicates = this.OutOfOrder = this.Drops = this.OutgoingData = this.IncomingData = this.Tamperred = 0;
      this.TimeInterval = 0.0f;
      this.PlayoutDelayBufferSize = (byte) 0;
    }

    public void UpdateData(
      int outgoing,
      int incoming,
      int incomingTamperred,
      float gcMemory,
      float processMemory)
    {
      this.OutgoingData += outgoing;
      this.IncomingData += incoming;
      this.Tamperred += incomingTamperred;
      this.GCMemory = gcMemory;
      this.ProcessMemory = processMemory;
    }

    public void Update(MyPacketTracker.OrderType type)
    {
      switch (type)
      {
        case MyPacketTracker.OrderType.InOrder:
          break;
        case MyPacketTracker.OrderType.OutOfOrder:
          ++this.OutOfOrder;
          break;
        case MyPacketTracker.OrderType.Duplicate:
          ++this.Duplicates;
          break;
        default:
          this.Drops += (int) (type - 3 + 1);
          break;
      }
    }

    public void Write(BitStream sendStream, MyTimeSpan currentTime)
    {
      if (currentTime > this.m_nextTime)
      {
        sendStream.WriteBool(true);
        sendStream.WriteByte((byte) this.Duplicates);
        sendStream.WriteByte((byte) this.OutOfOrder);
        sendStream.WriteByte((byte) this.Drops);
        sendStream.WriteByte((byte) this.Tamperred);
        sendStream.WriteInt32(this.OutgoingData);
        sendStream.WriteInt32(this.IncomingData);
        sendStream.WriteFloat((float) (currentTime - this.m_nextTime + MyPacketStatistics.SEND_TIMEOUT).Seconds);
        sendStream.WriteByte(this.PendingPackets);
        sendStream.WriteFloat(this.GCMemory);
        sendStream.WriteFloat(this.ProcessMemory);
        sendStream.WriteByte(this.PlayoutDelayBufferSize);
        this.Reset();
        this.m_nextTime = currentTime + MyPacketStatistics.SEND_TIMEOUT;
      }
      else
        sendStream.WriteBool(false);
    }

    public void Read(BitStream receiveStream)
    {
      if (!receiveStream.ReadBool())
        return;
      this.Duplicates = (int) receiveStream.ReadByte();
      this.OutOfOrder = (int) receiveStream.ReadByte();
      this.Drops = (int) receiveStream.ReadByte();
      this.Tamperred = (int) receiveStream.ReadByte();
      this.OutgoingData = receiveStream.ReadInt32();
      this.IncomingData = receiveStream.ReadInt32();
      this.TimeInterval = receiveStream.ReadFloat();
      this.PendingPackets = receiveStream.ReadByte();
      this.GCMemory = receiveStream.ReadFloat();
      this.ProcessMemory = receiveStream.ReadFloat();
      this.PlayoutDelayBufferSize = receiveStream.ReadByte();
    }

    public void Add(MyPacketStatistics statistics)
    {
      this.Duplicates += statistics.Duplicates;
      this.OutOfOrder += statistics.OutOfOrder;
      this.Drops += statistics.Drops;
      this.Tamperred += statistics.Tamperred;
      this.OutgoingData += statistics.OutgoingData;
      this.IncomingData += statistics.IncomingData;
      this.TimeInterval += statistics.TimeInterval;
      this.PendingPackets = statistics.PendingPackets;
      this.GCMemory = statistics.GCMemory;
      this.ProcessMemory = statistics.ProcessMemory;
      this.PlayoutDelayBufferSize = statistics.PlayoutDelayBufferSize;
    }
  }
}
