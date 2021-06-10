// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyStreamingEntityStateGroup`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Replication.StateGroups
{
  internal class MyStreamingEntityStateGroup<T> : IMyStreamingStateGroup, IMyStateGroup, IMyNetObject, IMyEventOwner
    where T : IMyStreamableReplicable
  {
    private long m_streamSize = 8000;
    private const int HEADER_SIZE = 97;
    private const int SAFE_VALUE = 128;
    private const int BitStreamLengthBits = 34;
    private bool m_streamed;
    private Dictionary<Endpoint, MyStreamingEntityStateGroup<T>.StreamClientData> m_clientStreamData;
    private SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]> m_receivedParts;
    private short m_numPartsToReceive;
    private int m_receivedBytes;
    private long m_uncompressedSize;

    private T Instance { get; set; }

    public IMyReplicable Owner { get; private set; }

    public bool HasStreamed(Endpoint endpoint)
    {
      MyStreamingEntityStateGroup<T>.StreamClientData streamClientData;
      return this.m_clientStreamData != null && this.m_clientStreamData.TryGetValue(endpoint, out streamClientData) && !streamClientData.Dirty && !streamClientData.Incomplete;
    }

    public bool NeedsUpdate => false;

    public bool IsValid => this.Owner != null && this.Owner.IsValid;

    public bool IsHighPriority => false;

    public MyStreamingEntityStateGroup(T obj, IMyReplicable owner)
    {
      this.Instance = obj;
      this.Owner = owner;
    }

    public bool IsStreaming => true;

    public void CreateClientData(MyClientStateBase forClient)
    {
      if (this.m_clientStreamData == null)
        this.m_clientStreamData = new Dictionary<Endpoint, MyStreamingEntityStateGroup<T>.StreamClientData>();
      if (this.m_clientStreamData.TryGetValue(forClient.EndpointId, out MyStreamingEntityStateGroup<T>.StreamClientData _))
        return;
      this.m_clientStreamData[forClient.EndpointId] = new MyStreamingEntityStateGroup<T>.StreamClientData();
    }

    public void DestroyClientData(MyClientStateBase forClient)
    {
      if (this.m_clientStreamData == null)
        return;
      this.m_clientStreamData.Remove(forClient.EndpointId);
    }

    public void ClientUpdate(MyTimeSpan clientTimestamp)
    {
    }

    public void Destroy()
    {
      if (this.m_receivedParts == null)
        return;
      this.m_receivedParts.Clear();
      this.m_receivedParts = (SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]>) null;
    }

    private unsafe bool ReadPart(ref BitStream stream, byte packetId)
    {
      this.m_numPartsToReceive = stream.ReadInt16();
      short num1 = stream.ReadInt16();
      long num2 = stream.ReadInt64(34);
      int divisionCeil = (int) MyLibraryUtils.GetDivisionCeil(num2, 8L);
      long num3 = stream.BitLength - stream.BitPosition;
      if (num3 < num2)
      {
        MyLog.Default.WriteLine("trying to read more than there is in stream. Total num parts : " + this.m_numPartsToReceive.ToString() + " current part : " + num1.ToString() + " bits to read : " + num2.ToString() + " bits in stream : " + num3.ToString() + " replicable : " + this.Instance.ToString());
        return false;
      }
      if (this.m_receivedParts == null)
        this.m_receivedParts = new SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]>();
      this.m_receivedBytes += divisionCeil;
      byte[] numArray = new byte[divisionCeil];
      fixed (byte* numPtr = numArray)
        stream.ReadMemory((void*) numPtr, num2);
      this.m_receivedParts[new MyStreamingEntityStateGroup<T>.StreamPartInfo()
      {
        NumBits = num2,
        StartIndex = (int) num1
      }] = numArray;
      return true;
    }

    private void ProcessRead(BitStream stream, byte packetId)
    {
      if (stream.BitLength == stream.BitPosition)
        return;
      if (this.m_streamed)
      {
        stream.ReadBool();
        stream.ReadInt64(34);
      }
      else if (stream.ReadBool())
      {
        long num = stream.ReadInt64(34);
        if (num == 0L)
          return;
        this.m_uncompressedSize = num;
        if (!this.ReadPart(ref stream, packetId))
        {
          this.m_receivedParts = (SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]>) null;
          this.Instance.LoadCancel();
        }
        else
        {
          if (this.m_receivedParts.Count != (int) this.m_numPartsToReceive)
            return;
          this.m_streamed = true;
          this.CreateReplicable(this.m_uncompressedSize);
        }
      }
      else
      {
        MyLog.Default.WriteLine("received empty state group");
        if (this.m_receivedParts != null)
          this.m_receivedParts.Clear();
        this.m_receivedParts = (SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]>) null;
        this.Instance.LoadCancel();
      }
    }

    private unsafe void CreateReplicable(long uncompressedSize)
    {
      byte[] bytes = new byte[this.m_receivedBytes];
      int dstOffset = 0;
      foreach (KeyValuePair<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]> receivedPart in this.m_receivedParts)
      {
        Buffer.BlockCopy((Array) receivedPart.Value, 0, (Array) bytes, dstOffset, receivedPart.Value.Length);
        dstOffset += receivedPart.Value.Length;
      }
      byte[] numArray1 = MemoryCompressor.Decompress(bytes);
      BitStream stream = new BitStream();
      stream.ResetWrite();
      byte[] numArray2 = numArray1;
      byte* numPtr = numArray1 == null || numArray2.Length == 0 ? (byte*) null : &numArray2[0];
      stream.SerializeMemory((void*) numPtr, uncompressedSize);
      numArray2 = (byte[]) null;
      stream.ResetRead();
      this.Instance.LoadDone(stream);
      if (!stream.CheckTerminator())
        MyLog.Default.WriteLine("Streaming entity: Invalid stream terminator");
      stream.Dispose();
      if (this.m_receivedParts != null)
        this.m_receivedParts.Clear();
      this.m_receivedParts = (SortedList<MyStreamingEntityStateGroup<T>.StreamPartInfo, byte[]>) null;
      this.m_receivedBytes = 0;
    }

    private void ProcessWrite(
      int maxBitPosition,
      BitStream stream,
      Endpoint forClient,
      byte packetId,
      HashSet<string> cachedData)
    {
      MyStreamingEntityStateGroup<T>.StreamClientData clientData = this.m_clientStreamData[forClient];
      if (clientData.FailedIncompletePackets.Count > 0)
        this.WriteIncompletePacket(clientData, packetId, ref stream);
      else if (clientData.ObjectData == null)
        this.SaveReplicable(clientData, cachedData, forClient);
      else if (clientData.LastSent.HasValue || !clientData.Incomplete)
      {
        stream.WriteBool(true);
        stream.WriteInt64(0L, 34);
      }
      else
      {
        clientData.LastSent = new byte?(packetId);
        this.m_streamSize = MyLibraryUtils.GetDivisionCeil((long) Math.Min(maxBitPosition, 8388608) - stream.BitPosition - 97L - 128L, 8L) * 8L;
        clientData.NumParts = (short) MyLibraryUtils.GetDivisionCeil((long) (clientData.ObjectData.Length * 8), this.m_streamSize);
        long remainingBits = clientData.RemainingBits;
        if (remainingBits == 0L)
        {
          clientData.ForceSend = false;
          clientData.Dirty = false;
          stream.WriteBool(false);
        }
        else
        {
          stream.WriteBool(true);
          stream.WriteInt64(clientData.UncompressedSize, 34);
          if (remainingBits > this.m_streamSize || clientData.Incomplete)
          {
            this.WritePart(ref remainingBits, clientData, packetId, ref stream);
            clientData.Incomplete = clientData.RemainingBits > 0L;
          }
          else
            this.WriteWhole(remainingBits, clientData, packetId, ref stream);
        }
      }
    }

    private unsafe void WriteIncompletePacket(
      MyStreamingEntityStateGroup<T>.StreamClientData clientData,
      byte packetId,
      ref BitStream stream)
    {
      if (clientData.ObjectData == null)
      {
        clientData.FailedIncompletePackets.Clear();
      }
      else
      {
        MyStreamingEntityStateGroup<T>.StreamPartInfo incompletePacket = clientData.FailedIncompletePackets[0];
        clientData.FailedIncompletePackets.Remove(incompletePacket);
        clientData.SendPackets[packetId] = incompletePacket;
        stream.WriteBool(true);
        stream.WriteInt64(clientData.UncompressedSize, 34);
        stream.WriteInt16(clientData.NumParts);
        stream.WriteInt16(incompletePacket.Position);
        stream.WriteInt64(incompletePacket.NumBits, 34);
        fixed (byte* numPtr = &clientData.ObjectData[incompletePacket.StartIndex])
          stream.WriteMemory((void*) numPtr, incompletePacket.NumBits);
      }
    }

    private unsafe void WritePart(
      ref long bitsToSend,
      MyStreamingEntityStateGroup<T>.StreamClientData clientData,
      byte packetId,
      ref BitStream stream)
    {
      bitsToSend = Math.Min(this.m_streamSize, clientData.RemainingBits);
      MyStreamingEntityStateGroup<T>.StreamPartInfo streamPartInfo = new MyStreamingEntityStateGroup<T>.StreamPartInfo()
      {
        StartIndex = clientData.LastPosition,
        NumBits = bitsToSend
      };
      clientData.LastPosition = streamPartInfo.StartIndex + (int) MyLibraryUtils.GetDivisionCeil(this.m_streamSize, 8L);
      clientData.SendPackets[packetId] = streamPartInfo;
      clientData.RemainingBits = Math.Max(0L, clientData.RemainingBits - this.m_streamSize);
      stream.WriteInt16(clientData.NumParts);
      stream.WriteInt16(clientData.CurrentPart);
      streamPartInfo.Position = clientData.CurrentPart;
      ++clientData.CurrentPart;
      stream.WriteInt64(bitsToSend, 34);
      fixed (byte* numPtr = &clientData.ObjectData[streamPartInfo.StartIndex])
        stream.WriteMemory((void*) numPtr, bitsToSend);
    }

    private unsafe void WriteWhole(
      long bitsToSend,
      MyStreamingEntityStateGroup<T>.StreamClientData clientData,
      byte packetId,
      ref BitStream stream)
    {
      MyStreamingEntityStateGroup<T>.StreamPartInfo streamPartInfo = new MyStreamingEntityStateGroup<T>.StreamPartInfo()
      {
        StartIndex = 0,
        NumBits = bitsToSend,
        Position = 0
      };
      clientData.SendPackets[packetId] = streamPartInfo;
      clientData.RemainingBits = 0L;
      clientData.Dirty = false;
      clientData.ForceSend = false;
      stream.WriteInt16((short) 1);
      stream.WriteInt16((short) 0);
      stream.WriteInt64(bitsToSend, 34);
      fixed (byte* numPtr = clientData.ObjectData)
        stream.WriteMemory((void*) numPtr, bitsToSend);
    }

    public void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData)
    {
      if (stream != null && stream.Reading)
        this.ProcessRead(stream, packetId);
      else
        this.ProcessWrite(maxBitPosition, stream, forClient.EndpointId, packetId, cachedData);
    }

    private void SaveReplicable(
      MyStreamingEntityStateGroup<T>.StreamClientData clientData,
      HashSet<string> cachedData,
      Endpoint forClient)
    {
      BitStream str = new BitStream();
      str.ResetWrite();
      clientData.CreatingData = true;
      this.Instance.Serialize(str, cachedData, forClient, (Action) (() => this.WriteClientData(str, clientData)));
    }

    private unsafe void WriteClientData(
      BitStream str,
      MyStreamingEntityStateGroup<T>.StreamClientData clientData)
    {
      str.Terminate();
      str.ResetRead();
      long bitLength = str.BitLength;
      byte[] bytes = new byte[str.ByteLength];
      fixed (byte* numPtr = bytes)
        str.SerializeMemory((void*) numPtr, bitLength);
      str.Dispose();
      clientData.CurrentPart = (short) 0;
      clientData.ObjectData = MemoryCompressor.Compress(bytes);
      clientData.UncompressedSize = bitLength;
      clientData.RemainingBits = (long) (clientData.ObjectData.Length * 8);
      clientData.CreatingData = false;
    }

    public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
    {
      MyStreamingEntityStateGroup<T>.StreamClientData streamClientData;
      if (!this.m_clientStreamData.TryGetValue(forClient.EndpointId, out streamClientData))
        return;
      byte? lastSent = streamClientData.LastSent;
      int? nullable = lastSent.HasValue ? new int?((int) lastSent.GetValueOrDefault()) : new int?();
      int num = (int) packetId;
      if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
        return;
      streamClientData.LastSent = new byte?();
      if (streamClientData.RemainingBits != 0L)
        return;
      streamClientData.Dirty = false;
      streamClientData.ForceSend = false;
    }

    public void ForceSend(MyClientStateBase clientData)
    {
      MyStreamingEntityStateGroup<T>.StreamClientData clientData1 = this.m_clientStreamData[clientData.EndpointId];
      clientData1.ForceSend = true;
      this.SaveReplicable(clientData1, (HashSet<string>) null, clientData.EndpointId);
    }

    public void Reset(bool reinit, MyTimeSpan clientTimestamp)
    {
    }

    public bool IsStillDirty(Endpoint forClient) => this.m_clientStreamData[forClient].Dirty;

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
    {
      MyStreamingEntityStateGroup<T>.StreamClientData streamClientData;
      if (!this.m_clientStreamData.TryGetValue(forClient, out streamClientData))
        return MyStreamProcessingState.None;
      if (streamClientData.CreatingData)
        return MyStreamProcessingState.Processing;
      return streamClientData.ObjectData == null ? MyStreamProcessingState.None : MyStreamProcessingState.Finished;
    }

    [Conditional("__RANDOM_UNDEFINED_SYMBOL__")]
    private void Log(string message)
    {
      NetworkId networkId;
      ((MyReplicationLayer) MyMultiplayer.ReplicationLayer).TryGetNetworkIdByObject((IMyNetObject) this, out networkId);
      MyLog.Default.WriteLine(string.Format("Streaming[{0}]: {1}", (object) networkId, (object) message));
    }

    private class StreamPartInfo : IComparable<MyStreamingEntityStateGroup<T>.StreamPartInfo>
    {
      public int StartIndex;
      public long NumBits;
      public short Position;

      public int CompareTo(MyStreamingEntityStateGroup<T>.StreamPartInfo b) => this.StartIndex.CompareTo(b.StartIndex);
    }

    private class StreamClientData
    {
      public short CurrentPart;
      public short NumParts;
      public int LastPosition;
      public byte[] ObjectData;
      public bool CreatingData;
      public bool Incomplete = true;
      public bool Dirty = true;
      public long RemainingBits;
      public long UncompressedSize;
      public bool ForceSend;
      public byte? LastSent;
      public readonly Dictionary<byte, MyStreamingEntityStateGroup<T>.StreamPartInfo> SendPackets = new Dictionary<byte, MyStreamingEntityStateGroup<T>.StreamPartInfo>();
      public readonly List<MyStreamingEntityStateGroup<T>.StreamPartInfo> FailedIncompletePackets = new List<MyStreamingEntityStateGroup<T>.StreamPartInfo>();
    }
  }
}
