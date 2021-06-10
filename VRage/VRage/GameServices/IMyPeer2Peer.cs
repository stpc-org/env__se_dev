// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyPeer2Peer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public interface IMyPeer2Peer
  {
    int MTUSize { get; }

    int NetworkUpdateLatency { get; }

    event Action<ulong> SessionRequest;

    event Action<ulong, string> ConnectionFailed;

    bool AcceptSession(ulong remotePeerId);

    bool CloseSession(ulong remotePeerId);

    bool SendPacket(
      ulong remoteUser,
      byte[] data,
      int byteCount,
      MyP2PMessageEnum msgType,
      int channel);

    bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel);

    bool IsPacketAvailable(out uint msgSize, int channel);

    bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state);

    void SetServer(bool server);

    string DetailedStats { get; }

    IEnumerable<(string Name, double Value)> Stats { get; }

    IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats { get; }

    void BeginFrameProcessing();

    void EndFrameProcessing();
  }
}
