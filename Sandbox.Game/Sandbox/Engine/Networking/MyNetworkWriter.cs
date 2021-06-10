// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyNetworkWriter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
  public static class MyNetworkWriter
  {
    public const byte PACKET_MAGIC = 206;
    public const int SIZE_MTR = 1000000;
    private static int m_byteCountSent;
    private static readonly Stopwatch m_timer = Stopwatch.StartNew();
    private static readonly ConcurrentQueue<MyNetworkWriter.MyPacketDescriptor> m_packetsToSend = new ConcurrentQueue<MyNetworkWriter.MyPacketDescriptor>();
    private static readonly MyConcurrentPool<MyNetworkWriter.MyPacketDescriptor> m_descriptorPool = new MyConcurrentPool<MyNetworkWriter.MyPacketDescriptor>(clear: ((Action<MyNetworkWriter.MyPacketDescriptor>) (x => x.Reset())));
    private static readonly MyConcurrentPool<MyNetworkWriter.MyPacketDataBitStream> m_bitStreamPool = new MyConcurrentPool<MyNetworkWriter.MyPacketDataBitStream>(deactivator: ((Action<MyNetworkWriter.MyPacketDataBitStream>) (x => x.Dispose())));
    private static readonly MyConcurrentPool<MyNetworkWriter.MyPacketDataArray> m_arrayPacketPool = new MyConcurrentPool<MyNetworkWriter.MyPacketDataArray>();
    private static readonly Crc32 m_crc32 = new Crc32();
    private static readonly List<IPacketData> m_packetsTmp = new List<IPacketData>();
    private static readonly ByteStream m_streamTmp = new ByteStream(1001024);
    public const int PACKET_HEADER_SIZE = 10;

    public static int QueuedBytes => MyNetworkWriter.m_packetsToSend.Sum<MyNetworkWriter.MyPacketDescriptor>((Func<MyNetworkWriter.MyPacketDescriptor, int>) (x =>
    {
      IPacketData data = x.Data;
      return data == null ? 0 : data.Size;
    }));

    public static TimeSpan MaxPacketAge
    {
      get
      {
        MyNetworkWriter.MyPacketDescriptor result;
        return !MyNetworkWriter.m_packetsToSend.TryPeek(out result) ? TimeSpan.Zero : MyNetworkWriter.m_timer.Elapsed - result.Stamp;
      }
    }

    static MyNetworkWriter() => MyVRage.RegisterExitCallback((Action) (() =>
    {
      foreach (MyNetworkWriter.MyPacketDescriptor packetDescriptor in MyNetworkWriter.m_packetsToSend)
        packetDescriptor.Data?.Return();
      MyNetworkWriter.m_bitStreamPool.Clean();
    }));

    public static void SendPacket(MyNetworkWriter.MyPacketDescriptor packet)
    {
      packet.Stamp = MyNetworkWriter.m_timer.Elapsed;
      MyNetworkWriter.m_packetsToSend.Enqueue(packet);
    }

    public static void SendAll()
    {
      int num1 = 0;
      int count1 = MyNetworkWriter.m_packetsToSend.Count;
      MyNetworkWriter.MyPacketDescriptor result;
      while (count1-- > 0 && MyNetworkWriter.m_packetsToSend.TryDequeue(out result))
      {
        MyNetworkWriter.m_packetsTmp.Clear();
        MyNetworkWriter.m_packetsTmp.Add(result.Data);
        uint v = uint.MaxValue;
        switch (result.MsgType)
        {
          case MyP2PMessageEnum.Reliable:
            if (result.Data != null && result.Data.Size >= 999998)
            {
              MyNetworkWriter.Split(result);
              break;
            }
            break;
          case MyP2PMessageEnum.ReliableWithBuffering:
            if (MyNetworkWriter.IsLastReliable(result.Channel))
              result.MsgType = MyP2PMessageEnum.Reliable;
            if (result.Data != null && result.Data.Size >= 999998)
            {
              MyNetworkWriter.Split(result);
              break;
            }
            break;
        }
        byte num2 = 0;
        byte num3 = result.MsgType == MyP2PMessageEnum.Unreliable || result.MsgType == MyP2PMessageEnum.UnreliableNoDelay ? (byte) 1 : (byte) 0;
        foreach (IPacketData packetData in MyNetworkWriter.m_packetsTmp)
        {
          MyNetworkWriter.m_streamTmp.Position = 0L;
          MyNetworkWriter.m_streamTmp.WriteNoAlloc((byte) 206);
          MyNetworkWriter.m_streamTmp.WriteNoAlloc(num3);
          int position1 = (int) MyNetworkWriter.m_streamTmp.Position;
          MyNetworkWriter.m_streamTmp.WriteNoAlloc(v);
          MyNetworkWriter.m_streamTmp.WriteNoAlloc(num2);
          MyNetworkWriter.m_streamTmp.WriteNoAlloc((byte) MyNetworkWriter.m_packetsTmp.Count);
          if (num2 == (byte) 0)
            MyNetworkWriter.m_streamTmp.Write(result.Header.Data, 0, (int) result.Header.Position);
          if (packetData != null)
          {
            int count2 = Math.Min(packetData.Size, 1000000);
            if (num2 == (byte) 0 && MyNetworkWriter.m_packetsTmp.Count > 1)
              count2 -= (int) result.Header.Position;
            if (packetData.Data != null)
              MyNetworkWriter.m_streamTmp.Write(packetData.Data, packetData.Offset, count2);
            else
              MyNetworkWriter.m_streamTmp.Write(packetData.Ptr, packetData.Offset, count2);
          }
          if (num3 > (byte) 0)
          {
            int position2 = (int) MyNetworkWriter.m_streamTmp.Position;
            MyNetworkWriter.m_crc32.Initialize();
            MyNetworkWriter.m_crc32.ComputeHash(MyNetworkWriter.m_streamTmp.Data, position1 + 4, position2 - position1 - 4);
            v = MyNetworkWriter.m_crc32.CrcValue;
            MyNetworkWriter.m_streamTmp.Position = (long) position1;
            MyNetworkWriter.m_streamTmp.WriteNoAlloc(v);
            MyNetworkWriter.m_streamTmp.Position = (long) position2;
          }
          foreach (EndpointId recipient in result.Recipients)
          {
            num1 += (int) MyNetworkWriter.m_streamTmp.Position;
            if (!MyGameService.Peer2Peer.SendPacket(recipient.Value, MyNetworkWriter.m_streamTmp.Data, (int) MyNetworkWriter.m_streamTmp.Position, result.MsgType, result.Channel))
              MyLog.Default.WriteLine("P2P packet not sent");
          }
          ++num2;
        }
        foreach (IPacketData packetData in MyNetworkWriter.m_packetsTmp)
          packetData?.Return();
        result.Data = (IPacketData) null;
        MyNetworkWriter.m_descriptorPool.Return(result);
      }
      Interlocked.Add(ref MyNetworkWriter.m_byteCountSent, num1);
    }

    private static void Split(MyNetworkWriter.MyPacketDescriptor packet)
    {
      int offset = 1000000 - (int) packet.Header.Position;
      int size1;
      for (int size2 = packet.Data.Size; offset < size2; offset += size1)
      {
        size1 = Math.Min(size2 - offset, 1000000);
        IPacketData packetData = packet.Data.Data == null ? MyNetworkWriter.GetPacketData(packet.Data.Ptr, offset, size1) : MyNetworkWriter.GetPacketData(packet.Data.Data, offset, size1);
        MyNetworkWriter.m_packetsTmp.Add(packetData);
      }
    }

    private static bool IsLastReliable(int channel)
    {
      foreach (MyNetworkWriter.MyPacketDescriptor packetDescriptor in MyNetworkWriter.m_packetsToSend)
      {
        if (packetDescriptor.Channel == channel && (packetDescriptor.MsgType == MyP2PMessageEnum.Reliable || packetDescriptor.MsgType == MyP2PMessageEnum.ReliableWithBuffering))
          return false;
      }
      return true;
    }

    public static int GetAndClearStats() => Interlocked.Exchange(ref MyNetworkWriter.m_byteCountSent, 0);

    public static MyPacketDataBitStreamBase GetBitStreamPacketData()
    {
      MyNetworkWriter.MyPacketDataBitStream packetDataBitStream = MyNetworkWriter.m_bitStreamPool.Get();
      packetDataBitStream.Acquire();
      return (MyPacketDataBitStreamBase) packetDataBitStream;
    }

    public static IPacketData GetPacketData(IntPtr data, int offset, int size)
    {
      MyNetworkWriter.MyPacketDataArray myPacketDataArray = MyNetworkWriter.m_arrayPacketPool.Get();
      myPacketDataArray.Ptr = data;
      myPacketDataArray.Offset = offset;
      myPacketDataArray.Size = size;
      return (IPacketData) myPacketDataArray;
    }

    public static IPacketData GetPacketData(byte[] data, int offset, int size)
    {
      MyNetworkWriter.MyPacketDataArray myPacketDataArray = MyNetworkWriter.m_arrayPacketPool.Get();
      myPacketDataArray.Data = data;
      myPacketDataArray.Offset = offset;
      myPacketDataArray.Size = size;
      return (IPacketData) myPacketDataArray;
    }

    internal static MyNetworkWriter.MyPacketDescriptor GetPacketDescriptor(
      EndpointId userId,
      MyP2PMessageEnum msgType,
      int channel)
    {
      MyNetworkWriter.MyPacketDescriptor packetDescriptor = MyNetworkWriter.m_descriptorPool.Get();
      packetDescriptor.MsgType = msgType;
      packetDescriptor.Channel = channel;
      if (userId.IsValid)
        packetDescriptor.Recipients.Add(userId);
      return packetDescriptor;
    }

    [GenerateActivator]
    private class MyPacketDataBitStream : MyPacketDataBitStreamBase
    {
      public override IntPtr Ptr => this.Stream.DataPointer;

      public override int Size => this.Stream.BytePosition;

      public override byte[] Data => (byte[]) null;

      public override int Offset => 0;

      public override void Return()
      {
        this.Stream.ResetWrite();
        this.m_returned = true;
        MyNetworkWriter.m_bitStreamPool.Return(this);
      }

      public void Acquire() => this.m_returned = false;

      private class Sandbox_Engine_Networking_MyNetworkWriter\u003C\u003EMyPacketDataBitStream\u003C\u003EActor : IActivator, IActivator<MyNetworkWriter.MyPacketDataBitStream>
      {
        object IActivator.CreateInstance() => (object) new MyNetworkWriter.MyPacketDataBitStream();

        MyNetworkWriter.MyPacketDataBitStream IActivator<MyNetworkWriter.MyPacketDataBitStream>.CreateInstance() => new MyNetworkWriter.MyPacketDataBitStream();
      }
    }

    [GenerateActivator]
    private class MyPacketDataArray : IPacketData
    {
      public byte[] Data { get; set; }

      public IntPtr Ptr { get; set; }

      public int Size { get; set; }

      public int Offset { get; set; }

      public void Return()
      {
        this.Data = (byte[]) null;
        this.Ptr = IntPtr.Zero;
        MyNetworkWriter.m_arrayPacketPool.Return(this);
      }

      private class Sandbox_Engine_Networking_MyNetworkWriter\u003C\u003EMyPacketDataArray\u003C\u003EActor : IActivator, IActivator<MyNetworkWriter.MyPacketDataArray>
      {
        object IActivator.CreateInstance() => (object) new MyNetworkWriter.MyPacketDataArray();

        MyNetworkWriter.MyPacketDataArray IActivator<MyNetworkWriter.MyPacketDataArray>.CreateInstance() => new MyNetworkWriter.MyPacketDataArray();
      }
    }

    [GenerateActivator]
    public class MyPacketDescriptor
    {
      public readonly List<EndpointId> Recipients = new List<EndpointId>();
      public MyP2PMessageEnum MsgType;
      public int Channel;
      public TimeSpan Stamp;
      public readonly ByteStream Header = new ByteStream(16);
      public IPacketData Data;

      public void Reset()
      {
        if (this.Data != null)
          this.Data.Return();
        this.Header.Position = 0L;
        this.Data = (IPacketData) null;
        this.Recipients.Clear();
      }

      private class Sandbox_Engine_Networking_MyNetworkWriter\u003C\u003EMyPacketDescriptor\u003C\u003EActor : IActivator, IActivator<MyNetworkWriter.MyPacketDescriptor>
      {
        object IActivator.CreateInstance() => (object) new MyNetworkWriter.MyPacketDescriptor();

        MyNetworkWriter.MyPacketDescriptor IActivator<MyNetworkWriter.MyPacketDescriptor>.CreateInstance() => new MyNetworkWriter.MyPacketDescriptor();
      }
    }
  }
}
