// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullPeer2Peer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyNullPeer2Peer : IMyPeer2Peer
  {
    public int MTUSize => 1200;

    public int NetworkUpdateLatency { get; }

    public event Action<ulong> SessionRequest
    {
      add
      {
      }
      remove
      {
      }
    }

    public event Action<ulong, string> ConnectionFailed
    {
      add
      {
      }
      remove
      {
      }
    }

    public bool AcceptSession(ulong remotePeerId) => false;

    public bool CloseSession(ulong remotePeerId) => false;

    public bool RequestChannel(int channel) => true;

    public bool SendPacket(
      ulong remoteUser,
      byte[] data,
      int byteCount,
      MyP2PMessageEnum msgType,
      int channel)
    {
      return false;
    }

    public bool ReadPacket(byte[] buffer, ref uint dataSize, out ulong remoteUser, int channel)
    {
      dataSize = 0U;
      remoteUser = 0UL;
      return false;
    }

    public bool IsPacketAvailable(out uint msgSize, int channel)
    {
      msgSize = 0U;
      return false;
    }

    public bool GetSessionState(ulong remoteUser, ref MyP2PSessionState state)
    {
      state = new MyP2PSessionState();
      return false;
    }

    public void SetServer(bool server)
    {
    }

    public string DetailedStats => "";

    public IEnumerable<(string Name, double Value)> Stats
    {
      get
      {
        yield break;
      }
    }

    public IEnumerable<(string Client, IEnumerable<(string Stat, double Value)> Stats)> ClientStats
    {
      get
      {
        yield break;
      }
    }

    public void BeginFrameProcessing()
    {
    }

    public void EndFrameProcessing()
    {
    }
  }
}
