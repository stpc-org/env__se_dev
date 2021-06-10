// Decompiled with JetBrains decompiler
// Type: VRage.Replication.IReplicationClientCallback
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Replication
{
  public interface IReplicationClientCallback
  {
    void SendClientUpdate(IPacketData data);

    void SendClientAcks(IPacketData data);

    void SendEvent(IPacketData data, bool reliable);

    void SendReplicableReady(IPacketData data);

    void SendReplicableRequest(IPacketData data);

    void SendConnectRequest(IPacketData data);

    void SendClientReady(MyPacketDataBitStreamBase data);

    void ReadCustomState(BitStream stream);

    MyTimeSpan GetUpdateTime();

    void SetNextFrameDelayDelta(float delay);

    void SetPing(long duration);

    void SetIslandDone(byte index, Dictionary<long, MatrixD> matrices);

    float GetServerSimulationRatio();

    float GetClientSimulationRatio();

    void DisconnectFromHost();

    void UpdateSnapshotCache();

    void PauseClient(bool pause);

    MyPacketDataBitStreamBase GetBitStreamPacketData();
  }
}
