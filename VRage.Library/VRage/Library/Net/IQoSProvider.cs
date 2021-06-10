// Decompiled with JetBrains decompiler
// Type: VRage.Library.Net.IQoSProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Net
{
  public interface IQoSProvider : IDisposable
  {
    bool MessagesAvailable { get; }

    bool HasPendingDeliveries { get; }

    int MaxNonBlockingMessageSize { get; }

    MessageTransferResult SendMessage(
      Span<byte> message,
      byte channel,
      SendMessageFlags flags = (SendMessageFlags) 0);

    bool TryReceiveMessage(ref Span<byte> message, out byte channel);

    bool TryPeekMessage(out int size, out byte channel);

    void ProcessWriteQueues();

    void ProcessReadQueues();
  }
}
