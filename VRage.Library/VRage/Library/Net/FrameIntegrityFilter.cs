// Decompiled with JetBrains decompiler
// Type: VRage.Library.Net.FrameIntegrityFilter
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Trace;

namespace VRage.Library.Net
{
  public class FrameIntegrityFilter : IUnreliableTransportChannel, IDisposable
  {
    private readonly IUnreliableTransportChannel m_channel;
    private readonly byte[] m_frameBuffer;
    private readonly Crc32 m_crc;
    private static readonly ITrace m_trace = MyTrace.GetTrace(TraceWindow.NetworkQoS);
    private static int m_idCtr;
    private string m_id;
    private int m_sendCount;
    private int m_receiveCount;
    private const string TraceCondition = "__UNDEFINED_SYMBOL__";

    public event FrameIntegrityFilter.ViolationListener ValidationFailure;

    public FrameIntegrityFilter(IUnreliableTransportChannel channel)
    {
      this.m_channel = channel;
      this.m_frameBuffer = new byte[channel.MinimumMTU];
      this.m_crc = new Crc32();
    }

    public void Dispose() => this.m_channel.Dispose();

    public int MinimumMTU => this.m_channel.MinimumMTU - 4;

    public int Send(Span<byte> frame)
    {
      frame = frame.Slice(0, Math.Min(this.m_frameBuffer.Length - 4, frame.Length));
      frame.CopyTo(this.m_frameBuffer.AsSpan<byte>(4));
      this.m_crc.Initialize();
      this.m_crc.ComputeHash(this.m_frameBuffer, 4, frame.Length);
      Unsafe.As<byte, uint>(ref this.m_frameBuffer[0]) = this.m_crc.CrcValue;
      this.m_channel.Send(this.m_frameBuffer.AsSpan<byte>(0, frame.Length + 4));
      return frame.Length;
    }

    public bool PeekFrame(out int frameSize)
    {
      int frameSize1;
      if (this.m_channel.PeekFrame(out frameSize1))
      {
        frameSize = Math.Min(frameSize1, this.m_frameBuffer.Length - 4);
        return true;
      }
      frameSize = 0;
      return false;
    }

    public bool TryGetFrame(ref Span<byte> frame)
    {
      Span<byte> frame1 = this.m_frameBuffer.AsSpan<byte>();
      if (!this.m_channel.TryGetFrame(ref frame1))
        return false;
      this.m_crc.Initialize();
      this.m_crc.ComputeHash(this.m_frameBuffer, 4, frame1.Length - 4);
      uint receivedCrc = Unsafe.As<byte, uint>(ref this.m_frameBuffer[0]);
      if ((int) receivedCrc != (int) this.m_crc.CrcValue)
      {
        FrameIntegrityFilter.ViolationListener validationFailure = this.ValidationFailure;
        if (validationFailure != null)
          validationFailure(receivedCrc, this.m_crc.CrcValue, this);
        return false;
      }
      frame1.Slice(4).CopyTo(frame);
      frame = frame.Slice(0, frame1.Length - 4);
      return true;
    }

    public void QueryMTU(Action<int> result) => result(this.m_frameBuffer.Length - 4);

    [Conditional("__UNDEFINED_SYMBOL__")]
    private void TraceInit()
    {
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      this.m_id = string.Format("IntegrityFilter[{0}::{1}]", (object) (((object) entryAssembly != null ? entryAssembly.GetName().Name : (string) null) ?? Assembly.GetExecutingAssembly().GetName().Name), (object) Interlocked.Increment(ref FrameIntegrityFilter.m_idCtr));
    }

    [Conditional("__UNDEFINED_SYMBOL__")]
    private void TraceMessage(string label = null, string content = null)
    {
      if (!FrameIntegrityFilter.m_trace.Enabled)
        return;
      if (label == null)
        FrameIntegrityFilter.m_trace.Send(this.m_id ?? "", content);
      else
        FrameIntegrityFilter.m_trace.Send(this.m_id + ": " + label, content);
    }

    public delegate void ViolationListener(
      uint receivedCrc,
      uint computedCrc,
      FrameIntegrityFilter instance);
  }
}
