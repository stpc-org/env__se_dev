// Decompiled with JetBrains decompiler
// Type: VRage.Library.Net.DefaultQoSProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage.Library.Debugging;
using VRage.Library.Memory;
using VRage.Network;
using VRage.Trace;

namespace VRage.Library.Net
{
  public class DefaultQoSProvider : IQoSProvider, IDisposable
  {
    private uint m_lastSentControlSequence;
    private uint m_lastReceivedControlSequence;
    public static bool TraceControlFrames = false;
    private TimeSpan m_averageEstimateRtt = TimeSpan.Zero;
    private TimeSpan m_averageEstimateRttVariation = TimeSpan.Zero;
    private TimeSpan m_lastSentPingTime = -DefaultQoSProvider.MinPingInterval;
    private TimeSpan m_lastReceivedPingTime = TimeSpan.Zero;
    private static readonly TimeSpan MinPingInterval = TimeSpan.FromMilliseconds(100.0);
    private static readonly TimeSpan MinPing = TimeSpan.FromMilliseconds(1.0);
    private static readonly NativeArrayAllocator NativeArrayAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("VRage.Net.DefaultQoSProvider"));
    private DefaultQoSProvider.ProcessedSettings m_settings;
    private readonly NativeArray m_controlBuffer;
    private readonly IUnreliableTransportChannel m_transport;
    private readonly Stopwatch m_time;
    private DefaultQoSProvider.CombinedBuffer<DefaultQoSProvider.ReceivedFrame> m_receivingBuffer;
    private DefaultQoSProvider.WindowPointer m_receivingWindowHead;
    private int m_receivedFrameCursor;
    private TimeSpan m_lastAckSent = TimeSpan.Zero;
    private TimeSpan FrameTimeout = DefaultQoSProvider.MinPingInterval;
    private DefaultQoSProvider.CombinedBuffer<DefaultQoSProvider.SendingFrame> m_sendingBuffer;
    private int m_unsentFrameCursor;
    private int m_nextResendFrame;
    private DefaultQoSProvider.WindowPointer m_sendingHead;
    private int m_firstSendQueueFrame;
    private TimeSpan m_averageAckTime = TimeSpan.Zero;
    private int m_framesSent;
    private int m_framesLost;
    private int m_maxWindowSize;
    private float m_targetSendingWindowCapacity;
    private float m_windowGrowthFactor;
    private TimeSpan m_timeSinceLastWindowAdjustment = TimeSpan.Zero;
    private DefaultQoSProvider.StatisticsInternal m_statistics;
    private static readonly ITrace m_trace = MyTrace.GetTrace(TraceWindow.NetworkQoS);
    private static int m_idCtr;
    private string m_id;

    private void GetNewControlFrame(DefaultQoSProvider.FrameType type, out Span<byte> frameData)
    {
      uint sequence = ++this.m_lastSentControlSequence;
      frameData = this.m_controlBuffer.AsSpan();
      Span<byte> destination = frameData;
      DefaultQoSProvider.FrameHeader frameHeader = new DefaultQoSProvider.FrameHeader(sequence, type);
      ref DefaultQoSProvider.FrameHeader local = ref frameHeader;
      this.Write<DefaultQoSProvider.FrameHeader>(destination, in local);
    }

    private void ProcessControlFrame(in DefaultQoSProvider.FrameHeader header, Span<byte> frameData)
    {
      if (header.Sequence > this.m_lastReceivedControlSequence || (ulong) header.Sequence + (ulong) uint.MaxValue - (ulong) this.m_lastReceivedControlSequence < 10000UL)
      {
        this.m_lastReceivedControlSequence = header.Sequence;
        switch (header.Type)
        {
          case DefaultQoSProvider.FrameType.Message:
            throw new ArgumentException("Messages frames should not be handled here.", nameof (header));
          case DefaultQoSProvider.FrameType.AckControl:
            this.ProcessAckControl(frameData);
            break;
          case DefaultQoSProvider.FrameType.Ack:
            this.ProcessAcks(frameData);
            break;
          case DefaultQoSProvider.FrameType.Ping:
            this.ProcessPing(frameData);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        this.m_statistics?.AddFrameReceived(frameData.Length, header.Type);
      }
      else
        this.m_statistics?.DropFrame(frameData.Length, false, DefaultQoSProvider.Statistics.DroppedFrameType.OutOfOrder);
    }

    private void SendControlFrame(Span<byte> frame, DefaultQoSProvider.FrameType type)
    {
      this.m_statistics?.AddFrameSent(frame.Length, type);
      this.m_transport.Send(frame);
    }

    private void ProcessAckControl(Span<byte> frameData)
    {
      int size = DefaultQoSProvider.FrameHeader.Size;
      switch (DefaultQoSProvider.Access<DefaultQoSProvider.FrameHeader>(frameData.Slice(size)).Type)
      {
        case DefaultQoSProvider.FrameType.Message:
          break;
        case DefaultQoSProvider.FrameType.AckControl:
          break;
        case DefaultQoSProvider.FrameType.Ack:
          break;
        case DefaultQoSProvider.FrameType.Ping:
          this.ProcessAckPing(frameData.Slice(size));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void SendAckControl(Span<byte> originalControlFrame)
    {
      int num = DefaultQoSProvider.TraceControlFrames ? 1 : 0;
      Span<byte> frameData;
      this.GetNewControlFrame(DefaultQoSProvider.FrameType.AckControl, out frameData);
      originalControlFrame.CopyTo(frameData.Slice(DefaultQoSProvider.FrameHeader.Size));
      frameData = frameData.Slice(0, DefaultQoSProvider.FrameHeader.Size + originalControlFrame.Length);
      this.SendControlFrame(frameData, DefaultQoSProvider.FrameType.AckControl);
    }

    public TimeSpan EstimateRTT => this.m_averageEstimateRtt;

    public TimeSpan EstimateRTTVariation => this.m_averageEstimateRttVariation;

    private void CheckAndSendPing()
    {
      TimeSpan timeSpan = !(this.m_averageEstimateRtt == TimeSpan.Zero) ? new TimeSpan(Math.Max(this.m_averageEstimateRtt.Ticks, DefaultQoSProvider.MinPingInterval.Ticks)) : DefaultQoSProvider.MinPingInterval;
      TimeSpan time = this.GetTime();
      if (!(time - this.m_lastSentPingTime > timeSpan))
        return;
      if (this.m_lastSentPingTime == TimeSpan.Zero)
      {
        TimeSpan elapsed = time - this.m_lastReceivedPingTime;
        if (this.m_averageEstimateRtt < new TimeSpan((long) ((double) elapsed.Ticks * 1.5)))
        {
          int num = DefaultQoSProvider.TraceControlFrames ? 1 : 0;
          this.UpdateRtt(elapsed);
        }
      }
      this.SendPing();
    }

    private void SendPing()
    {
      Span<byte> frameData;
      this.GetNewControlFrame(DefaultQoSProvider.FrameType.Ping, out frameData);
      this.m_lastSentPingTime = DefaultQoSProvider.Access<DefaultQoSProvider.PingFrame>(frameData).Timestamp = this.GetTime();
      frameData = frameData.Slice(0, DefaultQoSProvider.PingFrame.Size);
      this.SendControlFrame(frameData, DefaultQoSProvider.FrameType.Ping);
      int num = DefaultQoSProvider.TraceControlFrames ? 1 : 0;
    }

    private void ProcessPing(Span<byte> frameData) => this.SendAckControl(frameData);

    private void ProcessAckPing(Span<byte> pingFrameData)
    {
      ref DefaultQoSProvider.PingFrame local = ref DefaultQoSProvider.Access<DefaultQoSProvider.PingFrame>(pingFrameData);
      TimeSpan time = this.GetTime();
      this.UpdateRtt(time - local.Timestamp);
      this.m_lastReceivedPingTime = time;
    }

    private void UpdateRtt(TimeSpan elapsed)
    {
      TimeSpan timeSpan1;
      TimeSpan timeSpan2;
      if (this.m_averageEstimateRtt == TimeSpan.Zero)
      {
        timeSpan1 = elapsed;
        timeSpan2 = TimeSpan.Zero;
      }
      else
      {
        timeSpan2 = !(this.m_averageEstimateRttVariation == TimeSpan.Zero) ? this.UpdateAverage(this.m_averageEstimateRttVariation, (this.m_averageEstimateRtt - elapsed).Duration()) : (this.m_averageEstimateRtt - elapsed).Duration();
        timeSpan1 = this.Max(this.UpdateAverage(this.m_averageEstimateRtt, elapsed), DefaultQoSProvider.MinPing);
      }
      int num = DefaultQoSProvider.TraceControlFrames ? 1 : 0;
      this.m_averageEstimateRtt = timeSpan1;
      this.m_averageEstimateRttVariation = timeSpan2;
      this.FrameTimeout = new TimeSpan((long) ((double) this.m_averageEstimateRtt.Ticks * (double) this.m_settings.FrameTimeoutSlipFactor));
    }

    public DefaultQoSProvider(
      IUnreliableTransportChannel transport,
      DefaultQoSProvider.InitSettings? settings = null)
    {
      if (!BitConverter.IsLittleEndian)
        throw new NotImplementedException("The default QoS provider is not prepared to handle operation in BigEndian systems.");
      this.m_transport = transport;
      int minimumMtu = transport.MinimumMTU;
      settings = new DefaultQoSProvider.InitSettings?(settings ?? DefaultQoSProvider.InitSettings.Default);
      DefaultQoSProvider.InitSettings settings1 = settings.Value;
      this.m_settings = DefaultQoSProvider.ProcessedSettings.Create(in settings1, transport);
      int minimumWindowCapacity = this.m_settings.MinimumWindowCapacity;
      int minimumQueueCapacity = this.m_settings.MinimumQueueCapacity;
      this.m_receivingBuffer = new DefaultQoSProvider.CombinedBuffer<DefaultQoSProvider.ReceivedFrame>(minimumWindowCapacity, minimumQueueCapacity, minimumMtu, this.m_settings.ClearBuffers);
      this.m_sendingBuffer = new DefaultQoSProvider.CombinedBuffer<DefaultQoSProvider.SendingFrame>(minimumWindowCapacity, minimumQueueCapacity, minimumMtu, this.m_settings.ClearBuffers);
      this.m_sendingHead = this.m_receivingWindowHead = new DefaultQoSProvider.WindowPointer(settings.Value.InitialSequenceNumber, 0);
      this.m_controlBuffer = DefaultQoSProvider.NativeArrayAllocator.Allocate(minimumMtu);
      this.m_targetSendingWindowCapacity = (float) this.m_settings.MinimumWindowCapacity;
      this.m_windowGrowthFactor = this.m_settings.WindowGrowthFactor;
      this.m_time = Stopwatch.StartNew();
    }

    public int MaxNonBlockingMessageSize => this.m_settings.MaximumNonBlockingMessageSize;

    public void ProcessReadQueues()
    {
      this.CheckDisposed();
      this.PollAvailableFrames();
    }

    public void ProcessWriteQueues()
    {
      if (this.m_unsentFrameCursor > 0)
      {
        DefaultQoSProvider.FrameInfo frame;
        this.GetNextFrame(out frame);
        this.FinishFrame(in frame);
      }
      this.ProcessSendQueue();
    }

    private TimeSpan GetTime() => this.m_time.Elapsed;

    private void TimedOut() => throw new TimeoutException("Communication with the remote host has exceeded the user defined number of retries/time.");

    private int ComputeResize(int current, int max, int desired)
    {
      while (current < desired)
        current <<= 1;
      return Math.Min(current, max);
    }

    private TimeSpan UpdateAverage(TimeSpan average, TimeSpan newValue, float factor = 0.1f) => TimeSpan.FromTicks((long) ((double) average.Ticks + (double) (newValue.Ticks - average.Ticks) * (double) factor));

    private TimeSpan Max(TimeSpan lhs, TimeSpan rhs) => new TimeSpan(Math.Max(lhs.Ticks, rhs.Ticks));

    public DefaultQoSProvider.ProcessedSettings Settings => this.m_settings;

    private void CheckDisposed()
    {
      if (this.m_controlBuffer.IsDisposed)
        throw new ObjectDisposedException(nameof (DefaultQoSProvider));
    }

    public void Dispose()
    {
      this.CheckDisposed();
      this.m_controlBuffer.Dispose();
      this.m_sendingBuffer.Dispose();
      this.m_receivingBuffer.Dispose();
    }

    public bool MessagesAvailable
    {
      get
      {
        this.CheckDisposed();
        return this.SeekToMessage();
      }
    }

    public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
    {
      this.CheckDisposed();
      if (!this.SeekToMessage())
      {
        channel = (byte) 0;
        return false;
      }
      Span<byte> headFrame1 = this.GetHeadFrame();
      int headerLength;
      DefaultQoSProvider.MessageHeader messageHeader = DefaultQoSProvider.MessageHeader.Read(headFrame1.Slice(this.m_receivedFrameCursor), out headerLength);
      if (!this.IsComplete(this.m_receivedFrameCursor, messageHeader.Length, headerLength))
      {
        channel = (byte) 0;
        return false;
      }
      bool flag = (long) messageHeader.Length > (long) this.Settings.MaximumNonBlockingMessageSize;
      (uint, int) valueTuple1 = (DefaultQoSProvider.Access<DefaultQoSProvider.FrameHeader>(headFrame1).Sequence, this.m_receivedFrameCursor);
      this.m_receivedFrameCursor += headerLength;
      uint length1 = messageHeader.Length;
      int start = 0;
      int length2 = 0;
      do
      {
        Span<byte> headFrame2 = this.GetHeadFrame();
        int val1 = (int) Math.Min((long) length1 - (long) start, (long) (headFrame2.Length - this.m_receivedFrameCursor));
        int length3 = Math.Min(val1, message.Length - start);
        if (length3 > 0)
          headFrame2.Slice(this.m_receivedFrameCursor, length3).CopyTo(message.Slice(start));
        this.m_receivedFrameCursor += val1;
        start += val1;
        length2 += length3;
        (uint, int) valueTuple2 = (DefaultQoSProvider.Access<DefaultQoSProvider.FrameHeader>(headFrame2).Sequence, this.m_receivedFrameCursor);
        if (headFrame2.Length == this.m_receivedFrameCursor)
        {
          this.FinishReadFrame();
          this.m_receivedFrameCursor = DefaultQoSProvider.FrameHeader.Size;
        }
        if (flag && this.m_receivingBuffer.Head == this.m_receivingWindowHead.Position && (long) start < (long) length1)
        {
          this.SendAcks();
          this.WaitForNewFrame();
        }
      }
      while ((long) start < (long) length1);
      message = message.Slice(0, length2);
      channel = messageHeader.Channel;
      return true;
    }

    private void FinishReadFrame()
    {
      this.m_receivingBuffer[this.m_receivingBuffer.Head].SetMissing();
      this.m_receivingBuffer.AdvanceHead(this.m_settings.ClearBuffers);
    }

    public bool TryPeekMessage(out int size, out byte channel)
    {
      this.CheckDisposed();
      if (!this.SeekToMessage())
      {
        size = 0;
        channel = (byte) 0;
        return false;
      }
      int headerLength;
      DefaultQoSProvider.MessageHeader messageHeader = DefaultQoSProvider.MessageHeader.Read(this.GetHeadFrame().Slice(this.m_receivedFrameCursor), out headerLength);
      bool flag = (long) messageHeader.Length > (long) this.Settings.MaximumNonBlockingMessageSize;
      if (!this.IsComplete(this.m_receivedFrameCursor, messageHeader.Length, headerLength) && !flag)
      {
        channel = (byte) 0;
        size = 0;
        return false;
      }
      size = (int) messageHeader.Length;
      channel = messageHeader.Channel;
      return true;
    }

    private Span<byte> GetHeadFrame() => this.m_receivingBuffer.GetFrame(this.m_receivingBuffer.Head).Slice(0, this.m_receivingBuffer[this.m_receivingBuffer.Head].Length);

    private bool IsComplete(int messageStartOffset, uint messageSize, int headerLength)
    {
      if ((long) messageSize > (long) this.m_settings.MaximumNonBlockingMessageSize)
        return true;
      int num1 = this.m_receivingBuffer.FrameSize - messageStartOffset - headerLength;
      int num2 = this.m_receivingBuffer.FrameSize - DefaultQoSProvider.FrameHeader.Size;
      long num3 = ((long) messageSize - (long) num1 + (long) num2 - 1L) / (long) num2 + 1L;
      return (long) this.m_receivingBuffer.Distance(this.m_receivingBuffer.Head, in this.m_receivingWindowHead) >= num3;
    }

    private bool SeekToMessage()
    {
      this.PollAvailableFrames();
      if (this.m_receivingWindowHead.Position == this.m_receivingBuffer.Head)
        return false;
      if (this.m_receivedFrameCursor == 0)
        this.m_receivedFrameCursor = DefaultQoSProvider.FrameHeader.Size;
      return true;
    }

    private bool PollAvailableFrames()
    {
      bool flag1 = false;
      bool flag2 = false;
      while (this.m_transport.PeekFrame(out int _) && !flag2)
      {
        Span<byte> frame = this.m_receivingBuffer.GetFrame(this.m_receivingWindowHead.Position);
        if (this.m_transport.TryGetFrame(ref frame))
        {
          DefaultQoSProvider.FrameHeader header = DefaultQoSProvider.Access<DefaultQoSProvider.FrameHeader>(frame);
          if (header.Type == DefaultQoSProvider.FrameType.Message)
          {
            flag2 = this.ReceiveMessageFrame(header, frame);
            if (this.GetTime() - this.m_lastAckSent > this.FrameTimeout)
            {
              this.SendAcks();
              flag1 = false;
            }
            else
              flag1 = true;
          }
          else
            this.ProcessControlFrame(in header, frame);
        }
      }
      if (flag1)
        this.SendAcks();
      this.CheckAndSendPing();
      return flag2;
    }

    private void WaitForNewFrame()
    {
      int position = this.m_receivingWindowHead.Position;
      while (this.m_receivingWindowHead.Position == position)
        this.PollAvailableFrames();
    }

    private bool ReceiveMessageFrame(DefaultQoSProvider.FrameHeader header, Span<byte> frame)
    {
      if (this.m_receivingWindowHead.Position == this.m_receivingBuffer.Tail)
        this.m_receivingBuffer.AdvanceTail();
      if (this.m_receivingBuffer.Distance(this.m_receivingBuffer.Head, in this.m_receivingWindowHead) >= this.m_receivingBuffer.QueueCapacity)
      {
        if (this.m_receivingBuffer.QueueCapacity == this.m_settings.MaximumQueueCapacity)
        {
          this.m_statistics?.DropFrame(frame.Length, true, DefaultQoSProvider.Statistics.DroppedFrameType.QueueFull);
          return true;
        }
        this.ResizeReceivingBuffer(this.m_receivingBuffer.WindowCapacity, this.ComputeResize(this.m_receivingBuffer.QueueCapacity, this.m_settings.MaximumQueueCapacity, this.m_receivingBuffer.QueueCapacity + 1));
        frame = this.m_receivingBuffer.GetFrame(this.m_receivingWindowHead.Position).Slice(0, frame.Length);
        header = DefaultQoSProvider.Access<DefaultQoSProvider.FrameHeader>(frame);
      }
      uint num1 = header.Sequence + -this.m_receivingWindowHead.Sequence;
      if ((long) num1 > (long) this.m_settings.MaximumWindowCapacity)
      {
        if (this.m_statistics != null)
        {
          if ((int) num1 > this.m_settings.MaximumWindowCapacity)
            this.m_statistics.DropFrame(frame.Length, true, DefaultQoSProvider.Statistics.DroppedFrameType.WindowFull);
          else
            this.m_statistics.DropFrame(frame.Length, true, DefaultQoSProvider.Statistics.DroppedFrameType.OutOfOrder);
        }
        return false;
      }
      if ((int) header.Sequence != (int) this.m_receivingWindowHead.Sequence)
      {
        if ((long) num1 > (long) this.m_receivingBuffer.WindowCapacity)
          this.ResizeReceivingBuffer(this.ComputeResize(this.m_receivingBuffer.WindowCapacity, this.m_settings.MaximumWindowCapacity, (int) num1), this.m_receivingBuffer.QueueCapacity);
        int num2 = this.m_receivingBuffer.Advance(this.m_receivingWindowHead.Position, (int) num1);
        if (!CircularMapping.IsInRange(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail, num2))
          this.m_receivingBuffer.AdvanceTail(this.m_receivingBuffer.Distance(this.m_receivingBuffer.Tail, num2 + 1));
        this.m_receivingBuffer.CopyFrame(this.m_receivingWindowHead.Position, num2);
        this.m_receivingBuffer[num2].SetReceived(frame.Length);
      }
      else
      {
        this.m_receivingBuffer[this.m_receivingWindowHead.Position].SetReceived(frame.Length);
        int amount = 0;
        foreach (int enumerateFrame in this.m_receivingBuffer.EnumerateFrames(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail))
        {
          if (this.m_receivingBuffer[enumerateFrame].State == DefaultQoSProvider.ReceiveState.Received)
          {
            this.m_receivingBuffer[enumerateFrame].State = DefaultQoSProvider.ReceiveState.Unprocessed;
            ++amount;
          }
          else
            break;
        }
        this.m_receivingWindowHead = this.m_receivingBuffer.Advance(this.m_receivingWindowHead, amount);
      }
      foreach (int enumerateFrame in this.m_receivingBuffer.EnumerateFrames(this.m_receivingBuffer.Head, this.m_receivingWindowHead.Position))
        ;
      this.m_statistics?.AddFrameReceived(frame.Length, DefaultQoSProvider.FrameType.Message);
      return false;
    }

    private void ResizeReceivingBuffer(int windowCapacity, int queueCapacity)
    {
      int position = this.m_receivingBuffer.Distance(this.m_receivingBuffer.Head, in this.m_receivingWindowHead);
      this.m_receivingBuffer.Resize(windowCapacity, queueCapacity, this.m_settings.ClearBuffers);
      this.m_receivingWindowHead = new DefaultQoSProvider.WindowPointer(this.m_receivingWindowHead.Sequence, position);
    }

    private void SendAcks()
    {
      Span<byte> frameData;
      this.GetNewControlFrame(DefaultQoSProvider.FrameType.Ack, out frameData);
      this.WriteAckFrame(ref frameData);
      int num = DefaultQoSProvider.m_trace.Enabled ? 1 : 0;
      this.SendControlFrame(frameData, DefaultQoSProvider.FrameType.Ack);
      this.m_lastAckSent = this.GetTime();
    }

    private void WriteAckFrame(ref Span<byte> frame)
    {
      DefaultQoSProvider.Access<DefaultQoSProvider.AckFrameHeader>(frame).FirstMissingSequence = this.m_receivingWindowHead.Sequence;
      int size = DefaultQoSProvider.AckFrameHeader.Size;
      if (this.m_receivingBuffer.Distance(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail) > 1)
      {
        byte num1 = 0;
        int num2 = 0;
        foreach (int enumerateFrame in this.m_receivingBuffer.EnumerateFrames(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail))
        {
          bool flag = this.m_receivingBuffer[enumerateFrame].State == DefaultQoSProvider.ReceiveState.Received;
          num1 |= (byte) ((flag ? 1 : 0) << num2);
          ++num2;
          if (num2 == 8)
          {
            frame[size] = num1;
            ++size;
            num2 = 0;
            num1 = (byte) 0;
          }
        }
        if (num2 > 0)
          frame[size++] = num1;
      }
      frame = frame.Slice(0, size);
    }

    public bool HasPendingDeliveries
    {
      get
      {
        this.CheckDisposed();
        return this.m_sendingBuffer.UsedLength > 0;
      }
    }

    public MessageTransferResult SendMessage(
      Span<byte> message,
      byte channel,
      SendMessageFlags flags = (SendMessageFlags) 0)
    {
      this.CheckDisposed();
      DefaultQoSProvider.MessageHeader header = new DefaultQoSProvider.MessageHeader(channel, message);
      int space = this.m_sendingBuffer.FrameSize - this.m_unsentFrameCursor;
      DefaultQoSProvider.FrameInfo frame;
      if (!header.Fits(space))
      {
        this.GetNextFrame(out frame);
        this.FinishFrame(in frame);
      }
      bool flag = message.Length > this.Settings.MaximumNonBlockingMessageSize;
      if ((flags & SendMessageFlags.NonBlocking) != (SendMessageFlags) 0)
      {
        if (flag)
          return MessageTransferResult.OversizedMessage;
        int num1 = Math.Max((int) this.m_targetSendingWindowCapacity - this.m_sendingBuffer.Distance(in this.m_sendingHead, this.m_firstSendQueueFrame), 0) + (this.m_settings.MaximumQueueCapacity - this.m_sendingBuffer.Distance(this.m_firstSendQueueFrame, this.m_sendingBuffer.Tail));
        int num2 = this.m_sendingBuffer.FrameSize - (this.m_unsentFrameCursor > 0 ? this.m_unsentFrameCursor : DefaultQoSProvider.FrameHeader.Size) - header.Size();
        int num3 = this.m_sendingBuffer.FrameSize - DefaultQoSProvider.FrameHeader.Size;
        int num4 = (message.Length - num2 + num3 - 1) / num3 + 1;
        if (num1 < num4)
          return MessageTransferResult.QueueFull;
      }
      if (this.m_unsentFrameCursor == 0)
        this.GetNewFrame(out frame);
      else
        this.GetNextFrame(out frame);
      (uint, int) valueTuple = (frame.Sequence, this.m_unsentFrameCursor);
      int start = this.WriteMessage(in header, frame.Data, ref this.m_unsentFrameCursor, message);
      while (start < message.Length)
      {
        this.FinishFrame(in frame);
        this.GetNewFrame(out frame, !flag);
        Span<byte> span = message.Slice(start);
        Span<byte> destination = frame.Data.Slice(DefaultQoSProvider.FrameHeader.Size);
        int length = Math.Min(span.Length, destination.Length);
        span.Slice(0, length).CopyTo(destination);
        start += length;
        this.m_unsentFrameCursor += length;
      }
      if ((((flags & SendMessageFlags.HighPriority) != (SendMessageFlags) 0 ? 1 : (this.m_unsentFrameCursor == this.m_sendingBuffer.FrameSize ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        this.FinishFrame(in frame);
      this.PollAvailableFrames();
      this.ProcessSendQueue();
      return MessageTransferResult.QueuedSuccessfully;
    }

    private void GetNextFrame(out DefaultQoSProvider.FrameInfo frame)
    {
      int tail = this.m_sendingBuffer.Tail;
      Span<byte> frame1 = this.m_sendingBuffer.GetFrame(tail);
      uint sequence = this.m_sendingBuffer.CalculateSequence(in this.m_sendingHead, tail);
      frame = new DefaultQoSProvider.FrameInfo(tail, sequence, frame1);
    }

    private void GetNewFrame(out DefaultQoSProvider.FrameInfo frame, bool shouldResize = true)
    {
      int num = this.m_sendingBuffer.Distance(this.m_firstSendQueueFrame, this.m_sendingBuffer.Tail);
      if (num >= this.m_sendingBuffer.QueueCapacity)
      {
        if (shouldResize && num < this.m_settings.MaximumQueueCapacity)
          this.ResizeSendingBuffer(this.m_sendingBuffer.WindowCapacity, this.ComputeResize(this.m_sendingBuffer.QueueCapacity, this.m_settings.MaximumQueueCapacity, this.m_sendingBuffer.QueueCapacity + 1));
        else
          this.ProcessSendQueueBlocking();
      }
      int tail = this.m_sendingBuffer.Tail;
      Span<byte> frame1 = this.m_sendingBuffer.GetFrame(tail);
      uint sequence = this.m_sendingBuffer.CalculateSequence(in this.m_sendingHead, tail);
      Span<byte> destination = frame1;
      DefaultQoSProvider.FrameHeader frameHeader = DefaultQoSProvider.FrameHeader.Message(sequence);
      ref DefaultQoSProvider.FrameHeader local = ref frameHeader;
      this.m_unsentFrameCursor = this.Write<DefaultQoSProvider.FrameHeader>(destination, in local);
      this.m_sendingBuffer[tail].State = DefaultQoSProvider.SendState.Unfinished;
      frame = new DefaultQoSProvider.FrameInfo(tail, sequence, frame1);
    }

    private void FinishFrame(in DefaultQoSProvider.FrameInfo frame)
    {
      ref DefaultQoSProvider.SendingFrame local = ref this.m_sendingBuffer[frame.FramePosition];
      local.Length = this.m_unsentFrameCursor;
      local.State = DefaultQoSProvider.SendState.Queued;
      this.m_sendingBuffer.AdvanceTail();
      this.m_unsentFrameCursor = 0;
    }

    private void SendFrame(int framePosition)
    {
      ref DefaultQoSProvider.SendingFrame local = ref this.m_sendingBuffer[framePosition];
      this.m_transport.Send(this.m_sendingBuffer.GetFrame(framePosition).Slice(0, local.Length));
      ++this.m_framesSent;
      local.LastSentTime = this.GetTime();
      if (local.State == DefaultQoSProvider.SendState.Queued)
        local.EnqueueTime = local.LastSentTime;
      this.m_statistics?.AddFrameSent(local.Length, DefaultQoSProvider.FrameType.Message, local.State != DefaultQoSProvider.SendState.Queued);
      local.State = DefaultQoSProvider.SendState.Sent;
      if (framePosition != this.m_firstSendQueueFrame)
        return;
      this.m_firstSendQueueFrame = this.m_sendingBuffer.Advance(this.m_firstSendQueueFrame);
    }

    private void ResizeSendingBuffer(int windowCapacity, int queueCapacity)
    {
      if (this.m_unsentFrameCursor > 0)
      {
        DefaultQoSProvider.FrameInfo frame;
        this.GetNextFrame(out frame);
        this.FinishFrame(in frame);
      }
      int num1 = this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_firstSendQueueFrame);
      int num2 = this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_nextResendFrame);
      this.m_sendingBuffer.Resize(windowCapacity, queueCapacity, this.m_settings.ClearBuffers);
      this.m_firstSendQueueFrame = num1;
      this.m_nextResendFrame = num2;
      this.m_sendingHead = new DefaultQoSProvider.WindowPointer(this.m_sendingHead.Sequence, 0);
    }

    private void ProcessSendQueueBlocking()
    {
      do
      {
        this.PollAvailableFrames();
        this.ProcessSendQueue(true);
      }
      while (this.m_sendingBuffer.Distance(this.m_firstSendQueueFrame, this.m_sendingBuffer.Tail) >= this.m_sendingBuffer.QueueCapacity);
    }

    private void ProcessSendQueue(bool blocking = false)
    {
      this.CheckAndSendPing();
      this.TryAdjustingWindowCapacity();
      int num1 = this.m_sendingBuffer.Distance(in this.m_sendingHead, this.m_firstSendQueueFrame);
      bool flag = false;
      if (this.m_sendingBuffer.UsedLength - num1 > 0)
      {
        int num2 = Math.Min(Math.Min(this.m_sendingBuffer.WindowCapacity, (int) this.m_targetSendingWindowCapacity) - this.m_sendingBuffer.Distance(in this.m_sendingHead, this.m_firstSendQueueFrame), this.m_sendingBuffer.Distance(this.m_firstSendQueueFrame, this.m_sendingBuffer.Tail));
        for (int index = 0; index < num2; ++index)
          this.SendFrame(this.m_firstSendQueueFrame);
        flag |= num2 > 0;
        this.m_maxWindowSize = Math.Max(this.m_maxWindowSize, num1 + num2);
      }
      if (num1 > 0)
      {
        if (this.m_sendingBuffer[this.m_nextResendFrame].State == DefaultQoSProvider.SendState.Acknowledged)
          this.FindNextResendFrame();
        int nextResendFrame = this.m_nextResendFrame;
        TimeSpan waitTime;
        while (this.ShouldResend(in this.m_sendingBuffer[this.m_nextResendFrame], out waitTime))
        {
          flag = true;
          if (!this.HasRetriedTooManyTimes(in this.m_sendingBuffer[this.m_nextResendFrame]))
          {
            if (this.m_averageEstimateRtt > TimeSpan.Zero && -waitTime > this.m_averageEstimateRttVariation)
              ++this.m_framesLost;
            this.SendFrame(this.m_nextResendFrame);
          }
          this.FindNextResendFrame();
          if (nextResendFrame == this.m_nextResendFrame)
            break;
        }
        if (blocking && waitTime > TimeSpan.Zero)
          Thread.Sleep(waitTime);
      }
      int num3 = flag ? 1 : 0;
    }

    private bool ShouldResend(in DefaultQoSProvider.SendingFrame header, out TimeSpan waitTime)
    {
      TimeSpan timeSpan = this.GetTime() - header.LastSentTime;
      waitTime = this.FrameTimeout - timeSpan;
      return timeSpan > this.FrameTimeout;
    }

    private bool HasRetriedTooManyTimes(in DefaultQoSProvider.SendingFrame header)
    {
      if (this.m_settings.DisconnectTimeout < TimeSpan.Zero || !(this.GetTime() - header.EnqueueTime > this.m_settings.DisconnectTimeout))
        return false;
      this.TimedOut();
      return true;
    }

    private void FindNextResendFrame()
    {
      int capacity = Math.Min(this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_firstSendQueueFrame), (int) this.m_targetSendingWindowCapacity);
      int index1 = this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_nextResendFrame);
      CircularMapping circularMapping = new CircularMapping(capacity);
      foreach (int amount in circularMapping.EnumerateFullRange(circularMapping.Advance(index1)))
      {
        int index2 = this.m_sendingBuffer.Advance(this.m_sendingBuffer.Head, amount);
        if (this.m_sendingBuffer[index2].State == DefaultQoSProvider.SendState.Sent)
        {
          this.m_nextResendFrame = index2;
          break;
        }
      }
    }

    public TimeSpan AverageAckTime => this.m_averageAckTime;

    private void ProcessAcks(Span<byte> ackMessage)
    {
      ref DefaultQoSProvider.AckFrameHeader local = ref Unsafe.As<byte, DefaultQoSProvider.AckFrameHeader>(ref ackMessage[0]);
      int num1 = DefaultQoSProvider.m_trace.Enabled ? 1 : 0;
      int position1 = this.m_sendingHead.Position;
      int amount1 = (int) local.FirstMissingSequence - (int) this.m_sendingHead.Sequence;
      long num2 = (long) this.m_sendingHead.Sequence + (long) this.m_sendingBuffer.Distance(in this.m_sendingHead, this.m_firstSendQueueFrame);
      if ((long) local.FirstMissingSequence > num2)
        return;
      int num3 = this.m_sendingBuffer.Advance(position1, amount1);
      TimeSpan now = this.GetTime();
      foreach (int enumerateFrame in this.m_sendingBuffer.EnumerateFrames(position1, num3))
        AckFrame(enumerateFrame);
      int position2 = this.m_sendingHead.Position;
      this.m_sendingBuffer.AdvanceHead(this.m_settings.ClearBuffers, amount1);
      this.m_sendingHead = new DefaultQoSProvider.WindowPointer(this.m_sendingHead.Sequence + (uint) amount1, this.m_sendingBuffer.Head);
      int size = DefaultQoSProvider.AckFrameHeader.Size;
      if (CircularMapping.IsInRange(position2, num3, this.m_nextResendFrame))
        this.m_nextResendFrame = num3;
      if (size == ackMessage.Length)
        return;
      int amount2 = Math.Min(ackMessage.Length - size << 3, this.m_sendingBuffer.Distance(num3, this.m_firstSendQueueFrame));
      int end = this.m_sendingBuffer.Advance(num3, amount2);
      byte num4 = ackMessage[size];
      int num5 = 0;
      foreach (int enumerateFrame in this.m_sendingBuffer.EnumerateFrames(num3, end))
      {
        if (num5 == 0)
        {
          num5 = 8;
          num4 = ackMessage[size];
          ++size;
        }
        if (((int) num4 & 1) != 0)
          AckFrame(enumerateFrame);
        num4 >>= 1;
        --num5;
      }

      void AckFrame(int index)
      {
        if (this.m_sendingBuffer[index].State == DefaultQoSProvider.SendState.Acknowledged)
          return;
        TimeSpan timeSpan = now - this.m_sendingBuffer[index].EnqueueTime;
        this.UpdateAckAverage(timeSpan);
        this.m_statistics?.AddFrameAcknowledged(timeSpan);
        this.m_sendingBuffer[index].SetAcknowledged();
      }
    }

    private void UpdateAckAverage(TimeSpan ackTime)
    {
      if (this.m_averageAckTime == TimeSpan.Zero)
        this.m_averageAckTime = ackTime;
      else
        this.m_averageAckTime = this.UpdateAverage(this.m_averageEstimateRtt, ackTime, 0.01f);
    }

    public float FrameLoss => this.m_framesSent != 0 ? (float) this.m_framesLost / (float) this.m_framesSent : 0.0f;

    public int SlidingWindowSize => (int) this.m_targetSendingWindowCapacity;

    public int QueueSize => this.m_sendingBuffer.Distance(this.m_firstSendQueueFrame, this.m_sendingBuffer.Tail);

    private void TryAdjustingWindowCapacity()
    {
      if (this.m_averageEstimateRtt == TimeSpan.Zero || !(this.GetTime() - this.m_timeSinceLastWindowAdjustment > this.FrameTimeout) || this.m_framesSent <= this.m_settings.WindowAdjustmentThreshold)
        return;
      if ((double) this.FrameLoss < (double) this.m_settings.FrameLossTolerance)
      {
        if (this.m_maxWindowSize >= (int) this.m_targetSendingWindowCapacity)
        {
          this.m_targetSendingWindowCapacity = Math.Min(this.m_targetSendingWindowCapacity * this.m_windowGrowthFactor, (float) this.m_settings.MaximumWindowCapacity);
          if ((double) this.m_windowGrowthFactor < (double) this.m_settings.WindowGrowthFactor)
            this.m_windowGrowthFactor = (float) ((double) this.m_settings.WindowGrowthFactor * 0.100000001490116 + (double) this.m_windowGrowthFactor * 0.899999976158142);
        }
      }
      else
      {
        this.m_targetSendingWindowCapacity = (float) (int) Math.Max(this.m_targetSendingWindowCapacity * (1f - this.FrameLoss), (float) this.m_settings.MinimumWindowCapacity);
        if ((double) this.FrameLoss > 0.200000002980232)
          this.m_windowGrowthFactor = (float) (1.0 + ((double) this.m_windowGrowthFactor - 1.0) * 0.75);
      }
      int sendingWindowCapacity = (int) this.m_targetSendingWindowCapacity;
      if (this.m_sendingBuffer.WindowCapacity != sendingWindowCapacity)
      {
        int num = this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_firstSendQueueFrame);
        if (sendingWindowCapacity > num)
          this.ResizeSendingBuffer(sendingWindowCapacity, this.m_sendingBuffer.QueueCapacity);
        else if (this.m_sendingBuffer.Distance(this.m_sendingBuffer.Head, this.m_nextResendFrame) >= sendingWindowCapacity)
          this.m_nextResendFrame = this.m_sendingHead.Position;
      }
      this.m_framesSent = this.m_framesLost = 0;
      this.m_timeSinceLastWindowAdjustment = this.GetTime();
      this.m_maxWindowSize = 0;
    }

    private unsafe int Write<T>(Span<byte> destination, in T data) where T : unmanaged
    {
      fixed (byte* numPtr = &destination.GetPinnableReference())
        *(T*) numPtr = data;
      return sizeof (T);
    }

    private int WriteMessage(
      in DefaultQoSProvider.MessageHeader header,
      Span<byte> frame,
      ref int frameWriteHead,
      Span<byte> message)
    {
      frame[frameWriteHead] = header.Channel;
      ++frameWriteHead;
      frameWriteHead += DefaultQoSProvider.Variant.WriteVariant(header.Length, frame.Slice(frameWriteHead));
      int val2 = frame.Length - frameWriteHead;
      int length = Math.Min(message.Length, val2);
      if (length > 0)
        message.Slice(0, length).CopyTo(frame.Slice(frameWriteHead));
      frameWriteHead += length;
      return length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T Access<T>(Span<byte> dataRange) where T : unmanaged => ref Unsafe.As<byte, T>(ref dataRange[0]);

    public DefaultQoSProvider.Statistics GetStatistics() => (DefaultQoSProvider.Statistics) this.m_statistics;

    public void SetStatisticsTracker(DefaultQoSProvider.Statistics instance) => this.m_statistics = (DefaultQoSProvider.StatisticsInternal) instance;

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceInit()
    {
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      this.m_id = string.Format("QoS[{0}::{1}]", (object) (((object) entryAssembly != null ? entryAssembly.GetName().Name : (string) null) ?? Assembly.GetExecutingAssembly().GetName().Name), (object) Interlocked.Increment(ref DefaultQoSProvider.m_idCtr));
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceMessage(string label = null, string content = null)
    {
      if (!DefaultQoSProvider.m_trace.Enabled)
        return;
      if (label == null)
        DefaultQoSProvider.m_trace.Send(this.m_id ?? "", content);
      else
        DefaultQoSProvider.m_trace.Send(this.m_id + ": " + label, content);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceSend(string label = null, string content = null)
    {
      if (!DefaultQoSProvider.m_trace.Enabled)
        return;
      if (label == null)
        DefaultQoSProvider.m_trace.Send(this.m_id + " Snd", content);
      else
        DefaultQoSProvider.m_trace.Send(this.m_id + " Snd: " + label, content);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceReceive(string label = null, string content = null)
    {
      if (!DefaultQoSProvider.m_trace.Enabled)
        return;
      if (label == null)
        DefaultQoSProvider.m_trace.Send(this.m_id + " Rcv", content);
      else
        DefaultQoSProvider.m_trace.Send(this.m_id + " Rcv: " + label, content);
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceSendingBuffer()
    {
      int num = DefaultQoSProvider.m_trace.Enabled ? 1 : 0;
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    private void TraceReceivingBuffer()
    {
      int num = DefaultQoSProvider.m_trace.Enabled ? 1 : 0;
    }

    private string FormatSendAcks()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(string.Format("First missing: {0}", (object) this.m_receivingWindowHead.Sequence));
      if (this.m_receivingBuffer.Distance(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail) == 1)
        return stringBuilder.ToString();
      bool flag = true;
      uint sequence1 = this.m_receivingWindowHead.Sequence;
      foreach (int enumerateFrame in this.m_receivingBuffer.EnumerateFrames(this.m_receivingWindowHead.Position, this.m_receivingBuffer.Tail))
      {
        if (this.m_receivingBuffer[enumerateFrame].State == DefaultQoSProvider.ReceiveState.Received)
        {
          if (!flag)
            stringBuilder.Append(", ");
          else
            flag = false;
          uint sequence2 = this.m_receivingBuffer.CalculateSequence(in this.m_receivingWindowHead, enumerateFrame);
          stringBuilder.Append(sequence2);
        }
        ++sequence1;
      }
      return stringBuilder.ToString();
    }

    private string FormatReceivedAcks(in DefaultQoSProvider.AckFrameHeader header, Span<byte> frame)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(string.Format("First missing: {0}", (object) header.FirstMissingSequence));
      int size = DefaultQoSProvider.AckFrameHeader.Size;
      if (frame.Length > size)
      {
        bool flag = true;
        uint firstMissingSequence = header.FirstMissingSequence;
        int num = 0;
        while (size < frame.Length)
        {
          if (((int) frame[size] >> num & 1) == 1)
          {
            if (!flag)
              stringBuilder.Append(", ");
            else
              flag = false;
            stringBuilder.Append(firstMissingSequence);
          }
          ++num;
          ++firstMissingSequence;
          if (num == 8)
          {
            num = 0;
            ++size;
          }
        }
      }
      return stringBuilder.ToString();
    }

    private string FormatSendingSide()
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendLine(string.Format("Q{0} W{1}", (object) this.m_sendingBuffer.QueueCapacity, (object) this.m_sendingBuffer.WindowCapacity));
      builder.AppendLine(string.Format("NextResendFrame: {0} SendingHead: ({1},#{2}) FirstSendQueueFrame: {3} Loss: {4:F2}% ", (object) this.m_nextResendFrame, (object) this.m_sendingHead.Position, (object) this.m_sendingHead.Sequence, (object) this.m_firstSendQueueFrame, (object) (float) (100.0 * (double) this.FrameLoss)));
      this.FormatSendingFrames(builder);
      return builder.ToString();
    }

    private void FormatSendingFrames(StringBuilder builder) => this.FormatCombinedBuffer<DefaultQoSProvider.SendingFrame>(builder, in this.m_sendingBuffer, this.m_firstSendQueueFrame, (Func<DefaultQoSProvider.SendingFrame, char>) (x =>
    {
      switch (x.State)
      {
        case DefaultQoSProvider.SendState.Acknowledged:
          return 'A';
        case DefaultQoSProvider.SendState.Unfinished:
          return 'U';
        case DefaultQoSProvider.SendState.Queued:
          return 'Q';
        case DefaultQoSProvider.SendState.Sent:
          return 'S';
        default:
          throw new ArgumentOutOfRangeException();
      }
    }));

    private string FormatReceivingSide()
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendLine(string.Format("Q{0} W{1}", (object) this.m_receivingBuffer.QueueCapacity, (object) this.m_receivingBuffer.WindowCapacity));
      builder.AppendLine(string.Format("LastReceivedControlSequence: {0} ReceivingHead: ({1},#{2}) ReceivedFrameCursor: {3}", (object) this.m_lastReceivedControlSequence, (object) this.m_receivingWindowHead.Position, (object) this.m_receivingWindowHead.Sequence, (object) this.m_receivedFrameCursor));
      this.FormatReceivingFrames(builder);
      return builder.ToString();
    }

    private void FormatReceivingFrames(StringBuilder builder) => this.FormatCombinedBuffer<DefaultQoSProvider.ReceivedFrame>(builder, in this.m_receivingBuffer, this.m_receivingWindowHead.Position, (Func<DefaultQoSProvider.ReceivedFrame, char>) (x =>
    {
      switch (x.State)
      {
        case DefaultQoSProvider.ReceiveState.Missing:
          return 'M';
        case DefaultQoSProvider.ReceiveState.Received:
          return 'R';
        case DefaultQoSProvider.ReceiveState.Unprocessed:
          return 'U';
        default:
          throw new ArgumentOutOfRangeException();
      }
    }));

    private void FormatCombinedBuffer<THeader>(
      StringBuilder sb,
      in DefaultQoSProvider.CombinedBuffer<THeader> cb,
      int splitIndex,
      Func<THeader, char> headerState)
      where THeader : unmanaged, DefaultQoSProvider.IHeader
    {
      sb.Append('⎡');
      CircularMapping mapping = cb.Mapping;
      for (int index = 0; index < mapping.Capacity; ++index)
      {
        char ch = ' ';
        if (index == mapping.Head)
          ch = index != mapping.Tail ? '<' : (mapping.ActiveLength > 0 ? '╳' : '◇');
        else if (index == mapping.Tail)
          ch = '>';
        else if (index == splitIndex)
          ch = '⊢';
        else if (mapping.IsInRange(index))
          ch = '-';
        sb.Append(ch);
      }
      sb.AppendLine("⎤");
      sb.Append('⎣');
      for (int index = 0; index < cb.TotalCapacity; ++index)
        sb.Append(mapping.IsInRange(index) ? headerState(cb[index]) : ' ');
      sb.AppendLine("⎦");
    }

    [DebuggerDisplay("Head({Head})-Tail({Tail}) Active({UsedLength})")]
    [DebuggerTypeProxy(typeof (DefaultQoSProvider.CombinedBuffer<>.CombinedBufferDebugProxy))]
    private struct CombinedBuffer<THeader> where THeader : unmanaged, DefaultQoSProvider.IHeader
    {
      public CircularMapping Mapping;
      public readonly int FrameSize;
      private static readonly CircularMapping.Copy<DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet> m_copyDelegate = new CircularMapping.Copy<DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet>(DefaultQoSProvider.CombinedBuffer<THeader>.Copy);

      public THeader[] Headers { get; private set; }

      public NativeArray Buffer { get; private set; }

      public int Head => this.Mapping.Head;

      public int Tail => this.Mapping.Tail;

      public int UsedLength => this.Mapping.ActiveLength;

      public int WindowCapacity { get; private set; }

      public int QueueCapacity { get; private set; }

      public int TotalCapacity => this.Mapping.Capacity;

      public CombinedBuffer(int windowCapacity, int queueCapacity, int frameSize, bool clear)
      {
        int capacity = windowCapacity + queueCapacity;
        this.Headers = new THeader[capacity];
        this.Buffer = DefaultQoSProvider.NativeArrayAllocator.Allocate(capacity * frameSize);
        if (clear)
          this.Buffer.AsSpan().Clear();
        this.Mapping = new CircularMapping(capacity);
        this.FrameSize = frameSize;
        this.WindowCapacity = windowCapacity;
        this.QueueCapacity = queueCapacity;
      }

      public void Rescale(int newWindowSize)
      {
        this.WindowCapacity = newWindowSize <= this.TotalCapacity ? newWindowSize : throw new InvalidOperationException("the new sliding window size is too large.");
        this.QueueCapacity = this.TotalCapacity - newWindowSize;
      }

      public void Resize(int windowSize, int queueSize, bool clear)
      {
        int newLength = windowSize + queueSize;
        THeader[] headers = newLength >= this.UsedLength ? new THeader[newLength] : throw new InvalidOperationException("New window size is not large enough to hold the current number of frames.");
        int size = newLength * this.FrameSize;
        NativeArray frameData = DefaultQoSProvider.NativeArrayAllocator.Allocate(size);
        if (clear)
          frameData.AsSpan().Clear();
        DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet original = new DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet(this.Headers, this.Buffer, this.FrameSize);
        DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet resized = new DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet(headers, frameData, this.FrameSize);
        this.Mapping.ResizeBuffer<DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet>(newLength, original, resized, DefaultQoSProvider.CombinedBuffer<THeader>.m_copyDelegate);
        this.Headers = headers;
        this.Buffer.Dispose();
        this.Buffer = frameData;
        this.WindowCapacity = windowSize;
        this.QueueCapacity = queueSize;
      }

      private static void Copy(
        DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet src,
        int srcIndex,
        DefaultQoSProvider.CombinedBuffer<THeader>.BufferSet dst,
        int dstIndex,
        int length)
      {
        Array.Copy((Array) src.Headers, srcIndex, (Array) dst.Headers, dstIndex, length);
        int start1 = srcIndex * src.FrameSize;
        int start2 = dstIndex * src.FrameSize;
        length *= src.FrameSize;
        Span<byte> span = src.FrameData.AsSpan();
        span = span.Slice(start1, length);
        span.CopyTo(dst.FrameData.AsSpan().Slice(start2));
      }

      public void CopyFrame(int sourcePosition, int destinationPosition) => this.GetFrame(sourcePosition).CopyTo(this.GetFrame(destinationPosition));

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void AdvanceHead(bool clear, int amount = 1)
      {
        int head1 = this.Mapping.Head;
        this.Mapping.AdvanceHead(amount);
        if (!clear)
          return;
        int head2 = this.Mapping.Head;
        foreach (int enumerateFrame in this.EnumerateFrames(head1, head2))
          this.GetFrame(enumerateFrame).Clear();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void AdvanceTail(int amount = 1) => this.Mapping.AdvanceTail(amount);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Advance(int index) => this.Mapping.Advance(index);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Advance(int index, int amount) => this.Mapping.Advance(index, amount);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Retract(int index) => this.Mapping.Retract(index);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Retract(int index, int amount) => this.Mapping.Retract(index, amount);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DefaultQoSProvider.WindowPointer Advance(
        DefaultQoSProvider.WindowPointer index)
      {
        return new DefaultQoSProvider.WindowPointer(index.Sequence + 1U, this.Advance(index.Position));
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public DefaultQoSProvider.WindowPointer Advance(
        DefaultQoSProvider.WindowPointer index,
        int amount)
      {
        return new DefaultQoSProvider.WindowPointer(index.Sequence + (uint) amount, this.Advance(index.Position, amount));
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Distance(int from, int to) => this.Mapping.Distance(from, to);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Distance(in DefaultQoSProvider.WindowPointer from, int to) => this.Distance(from.Position, to);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Distance(int from, in DefaultQoSProvider.WindowPointer to) => this.Distance(from, to.Position);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public int Distance(
        in DefaultQoSProvider.WindowPointer from,
        in DefaultQoSProvider.WindowPointer to)
      {
        return (int) to.Sequence - (int) from.Sequence;
      }

      public uint CalculateSequence(in DefaultQoSProvider.WindowPointer reference, int position) => reference.Sequence + (uint) this.Distance(reference.Position, position);

      public int CalculatePosition(
        in DefaultQoSProvider.WindowPointer reference,
        uint sequenceNumber)
      {
        return reference.Position + ((int) sequenceNumber - (int) reference.Sequence);
      }

      public ref THeader this[int index] => ref this.Headers[index];

      public Span<byte> GetFrame(int frameIndex)
      {
        if (frameIndex < 0 || frameIndex >= this.TotalCapacity)
          throw new ArgumentOutOfRangeException(nameof (frameIndex));
        return this.Buffer.AsSpan().Slice(frameIndex * this.FrameSize, this.FrameSize);
      }

      public CircularMapping.Enumerable EnumerateFrames(int start, int end) => new CircularMapping.Enumerable(start, end, this.TotalCapacity, start == end);

      public unsafe void Dispose()
      {
        this.Buffer.Dispose();
        *(DefaultQoSProvider.CombinedBuffer<THeader>*) ref this = new DefaultQoSProvider.CombinedBuffer<THeader>();
      }

      private struct BufferSet
      {
        public THeader[] Headers;
        public NativeArray FrameData;
        public int FrameSize;

        public BufferSet(THeader[] headers, NativeArray frameData, int frameSize)
        {
          this.Headers = headers;
          this.FrameData = frameData;
          this.FrameSize = frameSize;
        }
      }

      [DebuggerDisplay("Head({m_instance.Head})-Tail({m_instance.Tail}) Active({m_instance.UsedLength})")]
      private class CombinedBufferDebugProxy
      {
        private DefaultQoSProvider.CombinedBuffer<THeader> m_instance;

        public CombinedBufferDebugProxy(
          DefaultQoSProvider.CombinedBuffer<THeader> instance)
        {
          this.m_instance = instance;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe string[] Keys
        {
          get
          {
            THeader[] headers = this.m_instance.Headers;
            string[] strArray = new string[headers.Length];
            for (int index = 0; index < headers.Length; ++index)
            {
              DefaultQoSProvider.FrameHeader header = *(DefaultQoSProvider.FrameHeader*) ((IntPtr) this.m_instance.Buffer.Ptr.ToPointer() + index * this.m_instance.FrameSize);
              strArray[index] = this.m_instance.Mapping.IsInRange(index) || !headers[index].IsUnused ? headers[index].FormatDebug(header) : "Unused";
              string str = "";
              if (this.m_instance.Head == this.m_instance.Tail && index == this.m_instance.Head)
                str = this.m_instance.UsedLength > 0 ? ">< " : "<> ";
              else if (this.m_instance.Head == index)
                str = "> ";
              else if (this.m_instance.Tail == index)
                str = "< ";
              else if (this.m_instance.Mapping.IsInRange(index))
                str = "- ";
              strArray[index] = str + strArray[index];
            }
            return strArray;
          }
        }

        [DebuggerDisplay("{m_debugString}")]
        public struct Entry
        {
          public THeader Header;
          public uint Sequence;
          private string m_debugString;

          public Entry(THeader header, uint sequence, string debugString)
          {
            this.Header = header;
            this.Sequence = sequence;
            this.m_debugString = debugString;
          }
        }
      }
    }

    private interface IHeader
    {
      bool IsUnused { get; }

      string FormatDebug(DefaultQoSProvider.FrameHeader header);
    }

    private readonly struct WindowPointer
    {
      public readonly uint Sequence;
      public readonly int Position;

      public WindowPointer(uint sequence, int position)
      {
        this.Sequence = sequence;
        this.Position = position;
      }

      public void Deconstruct(out uint sequence, out int queuePosition)
      {
        sequence = this.Sequence;
        queuePosition = this.Position;
      }
    }

    public struct InitSettings
    {
      public int MinimumWindowSize;
      public int MaximumWindowSize;
      public int MinimumQueueSize;
      public int MaximumQueueSize;
      public int MaximumMessageSize;
      public uint InitialSequenceNumber;
      public TimeSpan DisconnectTimeout;
      public bool ClearBuffers;
      public float FrameTimeoutSlipFactor;
      public float WindowGrowthFactor;
      public float FrameLossTolerance;
      public int WindowAdjustmentThreshold;
      public static readonly DefaultQoSProvider.InitSettings Default = new DefaultQoSProvider.InitSettings()
      {
        MinimumWindowSize = 8192,
        MaximumWindowSize = 65536,
        MinimumQueueSize = 8192,
        MaximumQueueSize = 262144,
        DisconnectTimeout = TimeSpan.FromSeconds(30.0),
        MaximumMessageSize = int.MaxValue,
        FrameTimeoutSlipFactor = 1.05f,
        WindowGrowthFactor = 2f,
        FrameLossTolerance = 0.05f,
        WindowAdjustmentThreshold = 20
      };
    }

    public struct ProcessedSettings
    {
      public int MinimumWindowCapacity;
      public int MaximumWindowCapacity;
      public int MinimumQueueCapacity;
      public int MaximumQueueCapacity;
      public int MaximumNonBlockingMessageSize;
      public TimeSpan DisconnectTimeout;
      public bool ClearBuffers;
      public float FrameTimeoutSlipFactor;
      public int WindowAdjustmentThreshold;
      public float WindowGrowthFactor;
      public float FrameLossTolerance;

      public static DefaultQoSProvider.ProcessedSettings Create(
        in DefaultQoSProvider.InitSettings settings,
        IUnreliableTransportChannel transport)
      {
        int mtu = transport.MinimumMTU;
        int num1 = mtu - DefaultQoSProvider.AckFrameHeader.Size << 3;
        int bytes = Math.Min(settings.MaximumWindowSize, num1 * mtu);
        DefaultQoSProvider.ProcessedSettings processedSettings;
        processedSettings.MinimumWindowCapacity = ToMtu(settings.MinimumWindowSize);
        processedSettings.MaximumWindowCapacity = ToMtu(bytes);
        processedSettings.MinimumQueueCapacity = Math.Max(ToMtu(settings.MinimumQueueSize), 2);
        processedSettings.MaximumQueueCapacity = Math.Max(ToMtu(settings.MaximumQueueSize), 2);
        processedSettings.DisconnectTimeout = settings.DisconnectTimeout;
        int num2 = mtu - DefaultQoSProvider.FrameHeader.Size;
        processedSettings.MaximumNonBlockingMessageSize = num2 * (processedSettings.MaximumQueueCapacity - 1) - 6;
        processedSettings.ClearBuffers = settings.ClearBuffers;
        processedSettings.FrameTimeoutSlipFactor = Math.Max(settings.FrameTimeoutSlipFactor, 1f);
        processedSettings.WindowGrowthFactor = Math.Max(settings.WindowGrowthFactor, 1.01f);
        if ((double) settings.FrameLossTolerance < 0.0 || (double) settings.FrameLossTolerance > 1.0)
          throw new ArgumentException("Frame loss tolerance out of bounds.");
        processedSettings.FrameLossTolerance = settings.FrameLossTolerance;
        processedSettings.WindowAdjustmentThreshold = settings.WindowAdjustmentThreshold;
        return processedSettings;

        int ToMtu(int bytes) => (bytes + mtu - 1) / mtu;
      }
    }

    private struct ReceivedFrame : DefaultQoSProvider.IHeader
    {
      public DefaultQoSProvider.ReceiveState State;
      public int Length;

      public void SetReceived(int length)
      {
        this.State = DefaultQoSProvider.ReceiveState.Received;
        this.Length = length;
      }

      public void SetMissing()
      {
        this.State = DefaultQoSProvider.ReceiveState.Missing;
        this.Length = 0;
      }

      public bool IsUnused => this.Length == 0 && this.State == DefaultQoSProvider.ReceiveState.Missing;

      public string FormatDebug(DefaultQoSProvider.FrameHeader header)
      {
        switch (this.State)
        {
          case DefaultQoSProvider.ReceiveState.Missing:
            return string.Format("{0:D5}:Missing", (object) header.Sequence);
          case DefaultQoSProvider.ReceiveState.Received:
            return string.Format("{0:D5}:Received({1})", (object) header.Sequence, (object) this.Length);
          case DefaultQoSProvider.ReceiveState.Unprocessed:
            return string.Format("{0:D5}:Unprocessed({1})", (object) header.Sequence, (object) this.Length);
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private enum ReceiveState : byte
    {
      Missing,
      Received,
      Unprocessed,
    }

    [DebuggerDisplay("{State}:{Length}")]
    private struct SendingFrame : DefaultQoSProvider.IHeader
    {
      public DefaultQoSProvider.SendState State;
      public TimeSpan LastSentTime;
      public TimeSpan EnqueueTime;
      public int Length;

      public void SetAcknowledged()
      {
        this.State = DefaultQoSProvider.SendState.Acknowledged;
        this.LastSentTime = TimeSpan.Zero;
        this.EnqueueTime = TimeSpan.Zero;
        this.Length = 0;
      }

      public bool IsUnused => this.State == DefaultQoSProvider.SendState.Acknowledged && this.Length == 0;

      public string FormatDebug(DefaultQoSProvider.FrameHeader header)
      {
        switch (this.State)
        {
          case DefaultQoSProvider.SendState.Queued:
            return string.Format("{0:D5}:Queue({1} bytes)", (object) header.Sequence, (object) this.Length);
          case DefaultQoSProvider.SendState.Sent:
            return string.Format("{0:D5}:Sent({1} bytes, first sent {2}, last {3})", (object) header.Sequence, (object) this.Length, (object) this.EnqueueTime, (object) this.LastSentTime.TotalMilliseconds);
          default:
            return string.Format("{0:D5}:{1}", (object) header.Sequence, (object) this.State);
        }
      }
    }

    private enum SendState : byte
    {
      Acknowledged,
      Unfinished,
      Queued,
      Sent,
    }

    private ref readonly struct FrameInfo
    {
      public readonly int FramePosition;
      public readonly uint Sequence;
      public readonly Span<byte> Data;

      public FrameInfo(int framePosition, uint sequence, Span<byte> data)
      {
        this.FramePosition = framePosition;
        this.Sequence = sequence;
        this.Data = data;
      }
    }

    private enum FrameType : byte
    {
      Message,
      AckControl,
      Ack,
      Ping,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private readonly struct FrameHeader
    {
      public static readonly int Size = sizeof (DefaultQoSProvider.FrameHeader);
      public readonly uint Sequence;
      public readonly DefaultQoSProvider.FrameType Type;

      public FrameHeader(uint sequence, DefaultQoSProvider.FrameType type)
      {
        this.Sequence = sequence;
        this.Type = type;
      }

      public static DefaultQoSProvider.FrameHeader Message(uint sequence) => new DefaultQoSProvider.FrameHeader(sequence, DefaultQoSProvider.FrameType.Message);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct AckFrameHeader
    {
      public static readonly int Size = sizeof (DefaultQoSProvider.AckFrameHeader);
      public DefaultQoSProvider.FrameHeader FrameHeader;
      public uint FirstMissingSequence;

      public AckFrameHeader(uint sequence, uint firstMissingSequence)
      {
        this.FrameHeader = new DefaultQoSProvider.FrameHeader(sequence, DefaultQoSProvider.FrameType.Ack);
        this.FirstMissingSequence = firstMissingSequence;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct PingFrame
    {
      public static readonly int Size = sizeof (DefaultQoSProvider.PingFrame);
      public DefaultQoSProvider.FrameHeader FrameHeader;
      public TimeSpan Timestamp;

      public PingFrame(uint sequence, TimeSpan timestamp)
      {
        this.FrameHeader = new DefaultQoSProvider.FrameHeader(sequence, DefaultQoSProvider.FrameType.Ping);
        this.Timestamp = timestamp;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private readonly struct MessageHeader
    {
      public readonly byte Channel;
      public readonly uint Length;
      public const int MaxSize = 6;

      public MessageHeader(byte channel, uint length)
      {
        this.Length = length;
        this.Channel = channel;
      }

      public MessageHeader(byte channel, Span<byte> message)
      {
        this.Length = (uint) message.Length;
        this.Channel = channel;
      }

      public bool Fits(int space) => space >= 6 || DefaultQoSProvider.Variant.CalcEncodedLength(this.Length) + 1 <= space;

      public int Size() => DefaultQoSProvider.Variant.CalcEncodedLength(this.Length) + 1;

      public static DefaultQoSProvider.MessageHeader Read(
        Span<byte> frame,
        out int headerLength)
      {
        byte channel = frame[0];
        uint length;
        headerLength = 1 + DefaultQoSProvider.Variant.ReadVariant(frame.Slice(1), out length);
        return new DefaultQoSProvider.MessageHeader(channel, length);
      }
    }

    private static class Variant
    {
      public const int Uint32MaximumEncodedLength = 5;

      public static int ReadVariant(Span<byte> source, out uint value)
      {
        value = (uint) source[0];
        if (((int) value & 128) == 0)
          return 1;
        value &= (uint) sbyte.MaxValue;
        uint num1 = (uint) source[1];
        value |= (uint) (((int) num1 & (int) sbyte.MaxValue) << 7);
        if (((int) num1 & 128) == 0)
          return 2;
        uint num2 = (uint) source[2];
        value |= (uint) (((int) num2 & (int) sbyte.MaxValue) << 14);
        if (((int) num2 & 128) == 0)
          return 3;
        uint num3 = (uint) source[3];
        value |= (uint) (((int) num3 & (int) sbyte.MaxValue) << 21);
        if (((int) num3 & 128) == 0)
          return 4;
        uint num4 = (uint) source[4];
        value |= num4 << 28;
        if (((int) num4 & 240) == 0)
          return 5;
        throw new OverflowException("The encoded variant would overflow the capacity of System.Uint32.");
      }

      public static int WriteVariant(uint value, Span<byte> destination)
      {
        int num = 0;
        do
        {
          destination[num++] = (byte) (value | 128U);
        }
        while ((value >>= 7) != 0U);
        destination[num - 1] &= (byte) 127;
        return num;
      }

      public static int CalcEncodedLength(uint value)
      {
        int num = 1;
        while ((value >>= 7) != 0U)
          ++num;
        return num;
      }
    }

    public abstract class Statistics
    {
      public abstract DefaultQoSProvider.Statistics.SentTraffic Sent { get; }

      public abstract DefaultQoSProvider.Statistics.ReceivedTraffic Received { get; }

      public abstract DefaultQoSProvider.Statistics.DroppedTraffic Dropped { get; }

      internal Statistics()
      {
      }

      public void AddTo(DefaultQoSProvider.Statistics instance)
      {
        if (!(instance is DefaultQoSProvider.AbsoluteStatistics absoluteStatistics))
          throw new InvalidOperationException("Accumulator instance is not absolute.");
        absoluteStatistics.AddFrom(this);
      }

      public virtual void Tick()
      {
      }

      public static DefaultQoSProvider.Statistics CreateAbsolute() => (DefaultQoSProvider.Statistics) new DefaultQoSProvider.AbsoluteStatistics();

      public static DefaultQoSProvider.Statistics CreateMovingWindow(
        TimeSpan windowLength)
      {
        return (DefaultQoSProvider.Statistics) new DefaultQoSProvider.WindowedStatistics(windowLength);
      }

      public override string ToString() => string.Join("\n", this.EnumerateFields().Select<(string, object), string>((Func<(string, object), string>) (x => string.Format("{0}: {1}", (object) x.Name, x.Stat))));

      public IEnumerable<(string Name, object Stat)> EnumerateFields()
      {
        DefaultQoSProvider.Statistics.SentTraffic sent = this.Sent;
        DefaultQoSProvider.Statistics.ReceivedTraffic received = this.Received;
        DefaultQoSProvider.Statistics.DroppedTraffic dropped = this.Dropped;
        yield return ("Sent", (object) sent.Total);
        yield return ("\tMessage", (object) sent.Message);
        yield return ("\t\tRe-Sent", (object) sent.MessageResend);
        yield return ("\t\tAcknowledged", (object) sent.Acknowledged);
        yield return ("\tControl", (object) sent.TotalControl);
        yield return ("\t\tAck", (object) sent.Ack);
        yield return ("\t\tAckControl", (object) sent.AckControl);
        yield return ("\t\tPing", (object) sent.Ping);
        yield return ("Received", (object) received.Total);
        yield return ("\tMessage", (object) received.Message);
        yield return ("\tControl", (object) received.TotalControl);
        yield return ("\t\tAck", (object) received.Ack);
        yield return ("\t\tAckControl", (object) received.AckControl);
        yield return ("\t\tPing", (object) received.Ping);
        yield return ("Dropped", (object) dropped.Total);
        yield return ("\tMessage", (object) dropped.TotalMessage);
        yield return ("\t\tQueue Full", (object) dropped.MessageQueueFull);
        yield return ("\t\tWindow Full", (object) dropped.MessageWindowFull);
        yield return ("\t\tOut of Order", (object) dropped.MessageOutOfOrder);
        yield return ("\tControl", (object) dropped.Control);
      }

      [Serializable]
      public struct SentTraffic
      {
        public DefaultQoSProvider.Statistics.Traffic Message;
        public DefaultQoSProvider.Statistics.Traffic MessageResend;
        public DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement Acknowledged;
        public DefaultQoSProvider.Statistics.Traffic Ack;
        public DefaultQoSProvider.Statistics.Traffic Ping;
        public DefaultQoSProvider.Statistics.Traffic AckControl;

        public DefaultQoSProvider.Statistics.Traffic TotalControl
        {
          get
          {
            DefaultQoSProvider.Statistics.Traffic traffic = ref this.Ack + ref this.Ping;
            return ref traffic + ref this.AckControl;
          }
        }

        public DefaultQoSProvider.Statistics.Traffic TotalMessage => ref this.Message + ref this.MessageResend;

        public DefaultQoSProvider.Statistics.Traffic Total
        {
          get
          {
            DefaultQoSProvider.Statistics.Traffic totalMessage = this.TotalMessage;
            ref DefaultQoSProvider.Statistics.Traffic local1 = ref totalMessage;
            DefaultQoSProvider.Statistics.Traffic totalControl = this.TotalControl;
            ref DefaultQoSProvider.Statistics.Traffic local2 = ref totalControl;
            return ref local1 + ref local2;
          }
        }

        public static DefaultQoSProvider.Statistics.SentTraffic operator +(
          in DefaultQoSProvider.Statistics.SentTraffic lhs,
          in DefaultQoSProvider.Statistics.SentTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.SentTraffic()
          {
            Message = ref lhs.Message + ref rhs.Message,
            MessageResend = ref lhs.MessageResend + ref rhs.MessageResend,
            Acknowledged = ref lhs.Acknowledged + ref rhs.Acknowledged,
            Ack = ref lhs.Ack + ref rhs.Ack,
            Ping = ref lhs.Ping + ref rhs.Ping,
            AckControl = ref lhs.AckControl + ref rhs.AckControl
          };
        }

        public static DefaultQoSProvider.Statistics.SentTraffic operator -(
          in DefaultQoSProvider.Statistics.SentTraffic lhs,
          in DefaultQoSProvider.Statistics.SentTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.SentTraffic()
          {
            Message = ref lhs.Message - ref rhs.Message,
            MessageResend = ref lhs.MessageResend - ref rhs.MessageResend,
            Acknowledged = ref lhs.Acknowledged - ref rhs.Acknowledged,
            Ack = ref lhs.Ack - ref rhs.Ack,
            Ping = ref lhs.Ping - ref rhs.Ping,
            AckControl = ref lhs.AckControl - ref rhs.AckControl
          };
        }

        public struct Acknowledgement
        {
          public uint Count;
          public TimeSpan TotalTime;

          public TimeSpan Average => this.Count <= 0U ? TimeSpan.Zero : new TimeSpan(this.TotalTime.Ticks / (long) this.Count);

          public static DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement operator +(
            in DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement lhs,
            in DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement rhs)
          {
            return new DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement()
            {
              Count = lhs.Count + rhs.Count,
              TotalTime = lhs.TotalTime + rhs.TotalTime
            };
          }

          public static DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement operator -(
            in DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement lhs,
            in DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement rhs)
          {
            return new DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement()
            {
              Count = lhs.Count - rhs.Count,
              TotalTime = lhs.TotalTime - rhs.TotalTime
            };
          }

          public void Add(TimeSpan ackDelay)
          {
            try
            {
              this.TotalTime += ackDelay;
              ++this.Count;
            }
            catch (OverflowException ex)
            {
            }
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EMessage\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Message = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Message;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EMessageResend\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.MessageResend = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.MessageResend;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EAcknowledged\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement value)
          {
            owner.Acknowledged = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.SentTraffic.Acknowledgement value)
          {
            value = owner.Acknowledged;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EAck\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Ack = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Ack;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EPing\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Ping = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Ping;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ESentTraffic\u003C\u003EAckControl\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.SentTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.AckControl = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.SentTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.AckControl;
          }
        }
      }

      [Serializable]
      public struct ReceivedTraffic
      {
        public DefaultQoSProvider.Statistics.Traffic Message;
        public DefaultQoSProvider.Statistics.Traffic Ack;
        public DefaultQoSProvider.Statistics.Traffic Ping;
        public DefaultQoSProvider.Statistics.Traffic AckControl;

        public DefaultQoSProvider.Statistics.Traffic TotalControl
        {
          get
          {
            DefaultQoSProvider.Statistics.Traffic traffic = ref this.Ack + ref this.Ping;
            return ref traffic + ref this.AckControl;
          }
        }

        public DefaultQoSProvider.Statistics.Traffic Total
        {
          get
          {
            ref DefaultQoSProvider.Statistics.Traffic local1 = ref this.Message;
            DefaultQoSProvider.Statistics.Traffic totalControl = this.TotalControl;
            ref DefaultQoSProvider.Statistics.Traffic local2 = ref totalControl;
            return ref local1 + ref local2;
          }
        }

        public static DefaultQoSProvider.Statistics.ReceivedTraffic operator +(
          in DefaultQoSProvider.Statistics.ReceivedTraffic lhs,
          in DefaultQoSProvider.Statistics.ReceivedTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.ReceivedTraffic()
          {
            Message = ref lhs.Message + ref rhs.Message,
            Ack = ref lhs.Ack + ref rhs.Ack,
            Ping = ref lhs.Ping + ref rhs.Ping,
            AckControl = ref lhs.AckControl + ref rhs.AckControl
          };
        }

        public static DefaultQoSProvider.Statistics.ReceivedTraffic operator -(
          in DefaultQoSProvider.Statistics.ReceivedTraffic lhs,
          in DefaultQoSProvider.Statistics.ReceivedTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.ReceivedTraffic()
          {
            Message = ref lhs.Message - ref rhs.Message,
            Ack = ref lhs.Ack - ref rhs.Ack,
            Ping = ref lhs.Ping - ref rhs.Ping,
            AckControl = ref lhs.AckControl - ref rhs.AckControl
          };
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EReceivedTraffic\u003C\u003EMessage\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.ReceivedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Message = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Message;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EReceivedTraffic\u003C\u003EAck\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.ReceivedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Ack = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Ack;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EReceivedTraffic\u003C\u003EPing\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.ReceivedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Ping = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Ping;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EReceivedTraffic\u003C\u003EAckControl\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.ReceivedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.AckControl = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.ReceivedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.AckControl;
          }
        }
      }

      [Serializable]
      public struct DroppedTraffic
      {
        public DefaultQoSProvider.Statistics.Traffic MessageOutOfOrder;
        public DefaultQoSProvider.Statistics.Traffic MessageQueueFull;
        public DefaultQoSProvider.Statistics.Traffic MessageWindowFull;
        public DefaultQoSProvider.Statistics.Traffic Control;

        public DefaultQoSProvider.Statistics.Traffic TotalMessage
        {
          get
          {
            DefaultQoSProvider.Statistics.Traffic traffic = ref this.MessageOutOfOrder + ref this.MessageQueueFull;
            return ref traffic + ref this.MessageWindowFull;
          }
        }

        public DefaultQoSProvider.Statistics.Traffic Total
        {
          get
          {
            ref DefaultQoSProvider.Statistics.Traffic local1 = ref this.Control;
            DefaultQoSProvider.Statistics.Traffic totalMessage = this.TotalMessage;
            ref DefaultQoSProvider.Statistics.Traffic local2 = ref totalMessage;
            return ref local1 + ref local2;
          }
        }

        public static DefaultQoSProvider.Statistics.DroppedTraffic operator +(
          in DefaultQoSProvider.Statistics.DroppedTraffic lhs,
          in DefaultQoSProvider.Statistics.DroppedTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.DroppedTraffic()
          {
            MessageOutOfOrder = ref lhs.MessageOutOfOrder + ref rhs.MessageOutOfOrder,
            MessageQueueFull = ref lhs.MessageQueueFull + ref rhs.MessageQueueFull,
            MessageWindowFull = ref lhs.MessageWindowFull + ref rhs.MessageWindowFull,
            Control = ref lhs.Control + ref rhs.Control
          };
        }

        public static DefaultQoSProvider.Statistics.DroppedTraffic operator -(
          in DefaultQoSProvider.Statistics.DroppedTraffic lhs,
          in DefaultQoSProvider.Statistics.DroppedTraffic rhs)
        {
          return new DefaultQoSProvider.Statistics.DroppedTraffic()
          {
            MessageOutOfOrder = ref lhs.MessageOutOfOrder - ref rhs.MessageOutOfOrder,
            MessageQueueFull = ref lhs.MessageQueueFull - ref rhs.MessageQueueFull,
            MessageWindowFull = ref lhs.MessageWindowFull - ref rhs.MessageWindowFull,
            Control = ref lhs.Control - ref rhs.Control
          };
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EDroppedTraffic\u003C\u003EMessageOutOfOrder\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.DroppedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.MessageOutOfOrder = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.MessageOutOfOrder;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EDroppedTraffic\u003C\u003EMessageQueueFull\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.DroppedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.MessageQueueFull = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.MessageQueueFull;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EDroppedTraffic\u003C\u003EMessageWindowFull\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.DroppedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.MessageWindowFull = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.MessageWindowFull;
          }
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003EDroppedTraffic\u003C\u003EControl\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.DroppedTraffic, DefaultQoSProvider.Statistics.Traffic>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            in DefaultQoSProvider.Statistics.Traffic value)
          {
            owner.Control = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref DefaultQoSProvider.Statistics.DroppedTraffic owner,
            out DefaultQoSProvider.Statistics.Traffic value)
          {
            value = owner.Control;
          }
        }
      }

      public enum DroppedFrameType
      {
        OutOfOrder,
        WindowFull,
        QueueFull,
      }

      [Serializable]
      public struct Traffic
      {
        public uint Frames;
        public ulong Bytes;

        public Traffic(uint frames, ulong bytes)
        {
          this.Frames = frames;
          this.Bytes = bytes;
        }

        public void AddFrame(int length)
        {
          ++this.Frames;
          this.Bytes += (ulong) (uint) length;
        }

        public static DefaultQoSProvider.Statistics.Traffic operator +(
          in DefaultQoSProvider.Statistics.Traffic lhs,
          in DefaultQoSProvider.Statistics.Traffic rhs)
        {
          return new DefaultQoSProvider.Statistics.Traffic(lhs.Frames + rhs.Frames, lhs.Bytes + rhs.Bytes);
        }

        public static DefaultQoSProvider.Statistics.Traffic operator -(
          in DefaultQoSProvider.Statistics.Traffic lhs,
          in DefaultQoSProvider.Statistics.Traffic rhs)
        {
          return new DefaultQoSProvider.Statistics.Traffic(lhs.Frames - rhs.Frames, lhs.Bytes - rhs.Bytes);
        }

        public static DefaultQoSProvider.Statistics.TrafficF operator /(
          DefaultQoSProvider.Statistics.Traffic lhs,
          float length)
        {
          return new DefaultQoSProvider.Statistics.TrafficF((float) lhs.Frames / length, (double) lhs.Bytes / (double) length);
        }

        public bool Equals(DefaultQoSProvider.Statistics.Traffic other) => (int) this.Frames == (int) other.Frames && (long) this.Bytes == (long) other.Bytes;

        public override bool Equals(object obj) => obj is DefaultQoSProvider.Statistics.Traffic other && this.Equals(other);

        public override int GetHashCode() => (int) this.Frames * 397 ^ this.Bytes.GetHashCode();

        public static bool operator ==(
          DefaultQoSProvider.Statistics.Traffic left,
          DefaultQoSProvider.Statistics.Traffic right)
        {
          return left.Equals(right);
        }

        public static bool operator !=(
          DefaultQoSProvider.Statistics.Traffic left,
          DefaultQoSProvider.Statistics.Traffic right)
        {
          return !left.Equals(right);
        }

        public override string ToString() => string.Format("{0}, {1}", (object) this.Frames, (object) this.Bytes);

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ETraffic\u003C\u003EFrames\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.Traffic, uint>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(ref DefaultQoSProvider.Statistics.Traffic owner, in uint value) => owner.Frames = value;

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(ref DefaultQoSProvider.Statistics.Traffic owner, out uint value) => value = owner.Frames;
        }

        protected class VRage_Library_Net_DefaultQoSProvider\u003C\u003EStatistics\u003C\u003ETraffic\u003C\u003EBytes\u003C\u003EAccessor : IMemberAccessor<DefaultQoSProvider.Statistics.Traffic, ulong>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(ref DefaultQoSProvider.Statistics.Traffic owner, in ulong value) => owner.Bytes = value;

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(ref DefaultQoSProvider.Statistics.Traffic owner, out ulong value) => value = owner.Bytes;
        }
      }

      public struct TrafficF
      {
        public float Frames;
        public double Bytes;

        public TrafficF(float frames, double bytes)
        {
          this.Frames = frames;
          this.Bytes = bytes;
        }

        public override string ToString() => string.Format("{0}, {1}", (object) this.Frames, (object) this.Bytes);
      }
    }

    private abstract class StatisticsInternal : DefaultQoSProvider.Statistics
    {
      protected void AddFrameSent(
        ref DefaultQoSProvider.Statistics.SentTraffic sentTraffic,
        int length,
        DefaultQoSProvider.FrameType type,
        bool resend)
      {
        switch (type)
        {
          case DefaultQoSProvider.FrameType.Message:
            if (resend)
            {
              sentTraffic.MessageResend.AddFrame(length);
              break;
            }
            sentTraffic.Message.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.AckControl:
            sentTraffic.AckControl.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.Ack:
            sentTraffic.Ack.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.Ping:
            sentTraffic.Ping.AddFrame(length);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        }
      }

      protected void AddFrameReceived(
        ref DefaultQoSProvider.Statistics.ReceivedTraffic received,
        int length,
        DefaultQoSProvider.FrameType type)
      {
        switch (type)
        {
          case DefaultQoSProvider.FrameType.Message:
            received.Message.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.AckControl:
            received.AckControl.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.Ack:
            received.Ack.AddFrame(length);
            break;
          case DefaultQoSProvider.FrameType.Ping:
            received.Ping.AddFrame(length);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        }
      }

      protected void AddFrameAcknowledged(
        ref DefaultQoSProvider.Statistics.SentTraffic received,
        TimeSpan ackDelay)
      {
        received.Acknowledged.Add(ackDelay);
      }

      protected void DropFrame(
        ref DefaultQoSProvider.Statistics.DroppedTraffic dropped,
        int length,
        bool message,
        DefaultQoSProvider.Statistics.DroppedFrameType type)
      {
        if (message)
        {
          switch (type)
          {
            case DefaultQoSProvider.Statistics.DroppedFrameType.OutOfOrder:
              dropped.MessageOutOfOrder.AddFrame(length);
              break;
            case DefaultQoSProvider.Statistics.DroppedFrameType.WindowFull:
              dropped.MessageWindowFull.AddFrame(length);
              break;
            case DefaultQoSProvider.Statistics.DroppedFrameType.QueueFull:
              dropped.MessageQueueFull.AddFrame(length);
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
          }
        }
        else
          dropped.Control.AddFrame(length);
      }

      public abstract void AddFrameSent(int length, DefaultQoSProvider.FrameType type, bool resend = false);

      public abstract void AddFrameAcknowledged(TimeSpan ackDelay);

      public abstract void AddFrameReceived(int length, DefaultQoSProvider.FrameType type);

      public abstract void DropFrame(
        int length,
        bool message,
        DefaultQoSProvider.Statistics.DroppedFrameType type);
    }

    private class AbsoluteStatistics : DefaultQoSProvider.StatisticsInternal
    {
      private DefaultQoSProvider.Statistics.SentTraffic m_sent;
      private DefaultQoSProvider.Statistics.ReceivedTraffic m_received;
      private DefaultQoSProvider.Statistics.DroppedTraffic m_dropped;

      public void AddFrom(DefaultQoSProvider.Statistics stats)
      {
        ref DefaultQoSProvider.Statistics.SentTraffic local1 = ref this.m_sent;
        DefaultQoSProvider.Statistics.SentTraffic sent = stats.Sent;
        ref DefaultQoSProvider.Statistics.SentTraffic local2 = ref sent;
        this.m_sent = ref local1 + ref local2;
        ref DefaultQoSProvider.Statistics.ReceivedTraffic local3 = ref this.m_received;
        DefaultQoSProvider.Statistics.ReceivedTraffic received = stats.Received;
        ref DefaultQoSProvider.Statistics.ReceivedTraffic local4 = ref received;
        this.m_received = ref local3 + ref local4;
        ref DefaultQoSProvider.Statistics.DroppedTraffic local5 = ref this.m_dropped;
        DefaultQoSProvider.Statistics.DroppedTraffic dropped = stats.Dropped;
        ref DefaultQoSProvider.Statistics.DroppedTraffic local6 = ref dropped;
        this.m_dropped = ref local5 + ref local6;
      }

      public override DefaultQoSProvider.Statistics.SentTraffic Sent => this.m_sent;

      public override DefaultQoSProvider.Statistics.ReceivedTraffic Received => this.m_received;

      public override DefaultQoSProvider.Statistics.DroppedTraffic Dropped => this.m_dropped;

      public override void AddFrameSent(int length, DefaultQoSProvider.FrameType type, bool resend = false) => this.AddFrameSent(ref this.m_sent, length, type, resend);

      public override void AddFrameAcknowledged(TimeSpan ackDelay) => this.AddFrameAcknowledged(ref this.m_sent, ackDelay);

      public override void AddFrameReceived(int length, DefaultQoSProvider.FrameType type) => this.AddFrameReceived(ref this.m_received, length, type);

      public override void DropFrame(
        int length,
        bool message,
        DefaultQoSProvider.Statistics.DroppedFrameType type)
      {
        this.DropFrame(ref this.m_dropped, length, message, type);
      }
    }

    private class WindowedStatistics : DefaultQoSProvider.StatisticsInternal
    {
      private MyTimedStatWindow<DefaultQoSProvider.WindowedStatistics.StatFrame> m_window;

      public WindowedStatistics(TimeSpan maxTime) => this.m_window = new MyTimedStatWindow<DefaultQoSProvider.WindowedStatistics.StatFrame>(maxTime, (MyTimedStatWindow.IStatArithmetic<DefaultQoSProvider.WindowedStatistics.StatFrame>) new DefaultQoSProvider.WindowedStatistics.StatFrame());

      public override DefaultQoSProvider.Statistics.SentTraffic Sent => this.m_window.Total.Sent;

      public override DefaultQoSProvider.Statistics.ReceivedTraffic Received => this.m_window.Total.Received;

      public override DefaultQoSProvider.Statistics.DroppedTraffic Dropped => this.m_window.Total.Dropped;

      private ref DefaultQoSProvider.WindowedStatistics.StatFrame Frame => ref this.m_window.Current;

      public override void AddFrameSent(int length, DefaultQoSProvider.FrameType type, bool resend = false) => this.AddFrameSent(ref this.Frame.Sent, length, type, resend);

      public override void AddFrameAcknowledged(TimeSpan ackDelay) => this.AddFrameAcknowledged(ref this.Frame.Sent, ackDelay);

      public override void AddFrameReceived(int length, DefaultQoSProvider.FrameType type) => this.AddFrameReceived(ref this.Frame.Received, length, type);

      public override void DropFrame(
        int length,
        bool message,
        DefaultQoSProvider.Statistics.DroppedFrameType type)
      {
        this.DropFrame(ref this.Frame.Dropped, length, message, type);
      }

      public override void Tick()
      {
        DefaultQoSProvider.WindowedStatistics.StatFrame total = this.m_window.Total;
        DefaultQoSProvider.WindowedStatistics.StatFrame statFrame = this.m_window.Aggregate<DefaultQoSProvider.WindowedStatistics.StatFrame>((Func<DefaultQoSProvider.WindowedStatistics.StatFrame, DefaultQoSProvider.WindowedStatistics.StatFrame, DefaultQoSProvider.WindowedStatistics.StatFrame>) ((lhs, rhs) => new DefaultQoSProvider.WindowedStatistics.StatFrame()
        {
          Sent = ref lhs.Sent + ref rhs.Sent,
          Received = ref lhs.Received + ref rhs.Received,
          Dropped = ref lhs.Dropped + ref rhs.Dropped
        }));
        this.m_window.Advance();
        total = this.m_window.Total;
        statFrame = this.m_window.Aggregate<DefaultQoSProvider.WindowedStatistics.StatFrame>((Func<DefaultQoSProvider.WindowedStatistics.StatFrame, DefaultQoSProvider.WindowedStatistics.StatFrame, DefaultQoSProvider.WindowedStatistics.StatFrame>) ((lhs, rhs) => new DefaultQoSProvider.WindowedStatistics.StatFrame()
        {
          Sent = ref lhs.Sent + ref rhs.Sent,
          Received = ref lhs.Received + ref rhs.Received,
          Dropped = ref lhs.Dropped + ref rhs.Dropped
        }));
      }

      private struct StatFrame : MyTimedStatWindow.IStatArithmetic<DefaultQoSProvider.WindowedStatistics.StatFrame>
      {
        public DefaultQoSProvider.Statistics.SentTraffic Sent;
        public DefaultQoSProvider.Statistics.ReceivedTraffic Received;
        public DefaultQoSProvider.Statistics.DroppedTraffic Dropped;

        public void Add(
          in DefaultQoSProvider.WindowedStatistics.StatFrame lhs,
          in DefaultQoSProvider.WindowedStatistics.StatFrame rhs,
          out DefaultQoSProvider.WindowedStatistics.StatFrame result)
        {
          result.Sent = ref lhs.Sent + ref rhs.Sent;
          result.Received = ref lhs.Received + ref rhs.Received;
          result.Dropped = ref lhs.Dropped + ref rhs.Dropped;
        }

        public void Subtract(
          in DefaultQoSProvider.WindowedStatistics.StatFrame lhs,
          in DefaultQoSProvider.WindowedStatistics.StatFrame rhs,
          out DefaultQoSProvider.WindowedStatistics.StatFrame result)
        {
          result.Sent = ref lhs.Sent - ref rhs.Sent;
          result.Received = ref lhs.Received - ref rhs.Received;
          result.Dropped = ref lhs.Dropped - ref rhs.Dropped;
        }

        void MyTimedStatWindow.IStatArithmetic<DefaultQoSProvider.WindowedStatistics.StatFrame>.Add(
          in DefaultQoSProvider.WindowedStatistics.StatFrame lhs,
          in DefaultQoSProvider.WindowedStatistics.StatFrame rhs,
          out DefaultQoSProvider.WindowedStatistics.StatFrame result)
        {
          this.Add(in lhs, in rhs, out result);
        }

        void MyTimedStatWindow.IStatArithmetic<DefaultQoSProvider.WindowedStatistics.StatFrame>.Subtract(
          in DefaultQoSProvider.WindowedStatistics.StatFrame lhs,
          in DefaultQoSProvider.WindowedStatistics.StatFrame rhs,
          out DefaultQoSProvider.WindowedStatistics.StatFrame result)
        {
          this.Subtract(in lhs, in rhs, out result);
        }
      }
    }
  }
}
