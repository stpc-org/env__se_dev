// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyReceiveQueue
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using VRage;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Networking
{
  internal sealed class MyReceiveQueue : IDisposable
  {
    private static readonly MyConcurrentBucketPool<MyReceiveQueue.MyPacketDataPooled> m_messagePool = (MyConcurrentBucketPool<MyReceiveQueue.MyPacketDataPooled>) new MyConcurrentBucketPool<MyReceiveQueue.MyPacketDataPooled, MyReceiveQueue.MessageAllocator>("MessagePool");
    private readonly ConcurrentQueue<MyPacket> m_receiveQueue;
    private readonly Func<MyTimeSpan> m_timestampProvider;
    private readonly int m_channel;
    private bool m_started;
    private readonly Action<ulong> m_disconnectPeerOnError;
    private static readonly Crc32 m_crc32 = new Crc32();
    private byte[] m_largePacket;
    private int m_largePacketCounter;
    private const int PACKET_HEADER_SIZE = 8;

    public MyReceiveQueue(int channel, Action<ulong> disconnectPeerOnError)
    {
      this.m_channel = channel;
      this.m_receiveQueue = new ConcurrentQueue<MyPacket>();
      this.m_disconnectPeerOnError = disconnectPeerOnError;
    }

    private MyReceiveQueue.MyPacketDataPooled GetMessage(int size) => MyReceiveQueue.m_messagePool.Get(MathHelper.GetNearestBiggerPowerOfTwo(Math.Max(size, 256)));

    public MyReceiveQueue.ReceiveStatus ReceiveOne(out uint length)
    {
      length = 0U;
      if (!MyGameService.Peer2Peer.IsPacketAvailable(out length, this.m_channel))
        return MyReceiveQueue.ReceiveStatus.None;
      MyReceiveQueue.MyPacketDataPooled message = this.GetMessage((int) length);
      ulong remoteUser;
      if (!MyGameService.Peer2Peer.ReadPacket(message.Data, ref length, out remoteUser, this.m_channel))
        return MyReceiveQueue.ReceiveStatus.None;
      MyReceiveQueue.MyPacketData myPacketData1 = (MyReceiveQueue.MyPacketData) message;
      int num1 = (int) message.Data[0];
      bool flag = message.Data[1] > (byte) 0;
      if (num1 != 206 || flag && !this.CheckCrc(message.Data, 2, 6, (int) length - 6))
      {
        string str = (string) null;
        for (int index = 0; (long) index < (long) Math.Min(10U, length); ++index)
          str = str + message.Data[index].ToString("X") + " ";
        MyLog.Default.WriteLine(string.Format("ERROR! Invalid packet from channel #{0} length {1} from {2} initial bytes: {3}", (object) this.m_channel, (object) length, (object) remoteUser, (object) str));
        message.Return();
        return MyReceiveQueue.ReceiveStatus.TamperredPacket;
      }
      byte num2 = message.Data[6];
      byte num3 = message.Data[7];
      int byteOffset = 8;
      if (num3 > (byte) 1)
      {
        if (num2 == (byte) 0)
          this.m_largePacket = new byte[(int) num3 * 1000000];
        else if (this.m_largePacket == null)
        {
          MyLog.Default.WriteLine(string.Format("ERROR! Invalid packet from channel #{0} length {1} from {2}.", (object) this.m_channel, (object) length, (object) remoteUser) + string.Format("Packet claims to be index {0} of a split message but previous parts were not received.", (object) num2));
          message.Return();
          return MyReceiveQueue.ReceiveStatus.TamperredPacket;
        }
        Array.Copy((Array) message.Data, 8L, (Array) this.m_largePacket, (long) ((int) num2 * 1000000), (long) (length - 8U));
        ++this.m_largePacketCounter;
        message.Return();
        if ((int) num2 != (int) num3 - 1)
          return this.ReceiveOne(out length);
        MyReceiveQueue.MyPacketData myPacketData2 = new MyReceiveQueue.MyPacketData();
        myPacketData2.Data = this.m_largePacket;
        myPacketData2.BitStream = new BitStream(0);
        myPacketData2.ByteStream = new ByteStream();
        myPacketData1 = myPacketData2;
        byteOffset = 0;
        length -= 8U;
        length += (uint) (((int) num3 - 1) * 1000000);
        this.m_largePacketCounter = 0;
        this.m_largePacket = (byte[]) null;
      }
      myPacketData1.BitStream.ResetRead(myPacketData1.Data, byteOffset, (long) ((int) ((long) length - (long) byteOffset) * 8), false);
      myPacketData1.ByteStream.Reset(myPacketData1.Data, (int) length);
      myPacketData1.ByteStream.Position = (long) byteOffset;
      myPacketData1.ReceivedTime = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
      myPacketData1.Sender = new Endpoint(remoteUser, (byte) 0);
      this.m_receiveQueue.Enqueue((MyPacket) myPacketData1);
      return MyReceiveQueue.ReceiveStatus.Success;
    }

    private unsafe bool CheckCrc(byte[] data, int crcIndex, int dataIndex, int dataLength)
    {
      MyReceiveQueue.m_crc32.Initialize();
      MyReceiveQueue.m_crc32.ComputeHash(data, dataIndex, dataLength);
      int num;
      fixed (byte* numPtr = data)
        num = (int) *(uint*) (numPtr + crcIndex);
      int crcValue = (int) MyReceiveQueue.m_crc32.CrcValue;
      return num == crcValue;
    }

    public void Dispose()
    {
      MyPacket result;
      while (this.m_receiveQueue.TryDequeue(out result))
        result.Return();
    }

    [HandleProcessCorruptedStateExceptions]
    [SecurityCritical]
    public void Process(NetworkMessageDelegate handler)
    {
      MyPacket result;
      while (this.m_receiveQueue.TryDequeue(out result))
      {
        try
        {
          handler(result);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex);
          MyLog.Default.WriteLine("Packet processing error, disconnecting " + (object) result.Sender);
          if (!Sync.IsServer)
            throw;
          else
            this.m_disconnectPeerOnError(result.Sender.Id.Value);
        }
      }
    }

    private class MyPacketData : MyPacket
    {
      public byte[] Data;

      public override void Return()
      {
        this.Data = (byte[]) null;
        this.BitStream.Dispose();
        this.BitStream = (BitStream) null;
        this.ByteStream = (ByteStream) null;
      }
    }

    private class MyPacketDataPooled : MyReceiveQueue.MyPacketData
    {
      private bool m_returned;

      public void Init() => this.m_returned = false;

      public override void Return()
      {
        this.m_returned = true;
        MyReceiveQueue.m_messagePool.Return(this);
      }
    }

    private class MessageAllocator : IMyElementAllocator<MyReceiveQueue.MyPacketDataPooled>
    {
      public bool ExplicitlyDisposeAllElements => true;

      public MyReceiveQueue.MyPacketDataPooled Allocate(int size)
      {
        MyReceiveQueue.MyPacketDataPooled packetDataPooled = new MyReceiveQueue.MyPacketDataPooled();
        packetDataPooled.Data = new byte[size];
        packetDataPooled.BitStream = new BitStream(0);
        packetDataPooled.ByteStream = new ByteStream();
        return packetDataPooled;
      }

      public void Dispose(MyReceiveQueue.MyPacketDataPooled message)
      {
        message.Data = (byte[]) null;
        message.BitStream.Dispose();
        message.BitStream = (BitStream) null;
        message.ByteStream = (ByteStream) null;
      }

      public void Init(MyReceiveQueue.MyPacketDataPooled message) => message.Init();

      public int GetBucketId(MyReceiveQueue.MyPacketDataPooled message) => message.Data.Length;

      public int GetBytes(MyReceiveQueue.MyPacketDataPooled message) => message.Data.Length;
    }

    public enum ReceiveStatus
    {
      None,
      TamperredPacket,
      Success,
    }
  }
}
