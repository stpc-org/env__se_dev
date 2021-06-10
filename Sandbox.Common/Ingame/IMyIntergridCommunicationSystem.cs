// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyIntergridCommunicationSystem
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyIntergridCommunicationSystem
  {
    long Me { get; }

    IMyUnicastListener UnicastListener { get; }

    bool IsEndpointReachable(long address, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay);

    IMyBroadcastListener RegisterBroadcastListener(string tag);

    void DisableBroadcastListener(IMyBroadcastListener broadcastListener);

    void GetBroadcastListeners(
      List<IMyBroadcastListener> broadcastListeners,
      Func<IMyBroadcastListener, bool> collect = null);

    void SendBroadcastMessage<TData>(
      string tag,
      TData data,
      TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay);

    bool SendUnicastMessage<TData>(long addressee, string tag, TData data);
  }
}
