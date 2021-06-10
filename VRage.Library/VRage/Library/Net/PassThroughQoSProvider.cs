// Decompiled with JetBrains decompiler
// Type: VRage.Library.Net.PassThroughQoSProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Net
{
  public class PassThroughQoSProvider : IQoSProvider, IDisposable
  {
    private readonly IUnreliableTransportChannel m_channel;

    public PassThroughQoSProvider(IUnreliableTransportChannel transport) => this.m_channel = transport;

    public bool MessagesAvailable => this.m_channel.PeekFrame(out int _);

    public bool HasPendingDeliveries => false;

    public int MaxNonBlockingMessageSize => this.m_channel.MinimumMTU;

    public MessageTransferResult SendMessage(
      Span<byte> message,
      byte channel,
      SendMessageFlags flags = (SendMessageFlags) 0)
    {
      this.m_channel.Send(message);
      return MessageTransferResult.QueuedSuccessfully;
    }

    public bool TryReceiveMessage(ref Span<byte> message, out byte channel)
    {
      channel = (byte) 0;
      return this.m_channel.TryGetFrame(ref message);
    }

    public bool TryPeekMessage(out int size, out byte channel)
    {
      channel = (byte) 0;
      return this.m_channel.PeekFrame(out size);
    }

    public void ProcessWriteQueues()
    {
    }

    public void ProcessReadQueues()
    {
    }

    public void Dispose()
    {
    }
  }
}
