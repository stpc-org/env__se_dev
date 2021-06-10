// Decompiled with JetBrains decompiler
// Type: VRage.Replication.IReplicationServerCallback
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace VRage.Replication
{
  public interface IReplicationServerCallback
  {
    void SendServerData(IPacketData data, Endpoint endpoint);

    void SendReplicationCreate(IPacketData data, Endpoint endpoint);

    void SendReplicationCreateStreamed(IPacketData data, Endpoint endpoint);

    void SendReplicationDestroy(IPacketData data, List<EndpointId> endpoints);

    void SendReplicationIslandDone(IPacketData data, Endpoint endpoint);

    void SendStateSync(IPacketData data, Endpoint endpoint, bool reliable);

    void SendJoinResult(IPacketData data, EndpointId endpoint);

    void SendWorldData(IPacketData data, List<EndpointId> endpoints);

    void SendWorld(IPacketData data, EndpointId endpoint);

    void SendPlayerData(IPacketData data, List<EndpointId> endpoints);

    void WriteCustomState(BitStream stream);

    void SentClientJoined(IPacketData data, EndpointId endpoint);

    void SendEvent(IPacketData data, bool reliable, List<EndpointId> endpoints);

    int GetMTUSize();

    IMyReplicable GetReplicableByEntityId(long entityId);

    void DisconnectClient(ulong clientId);

    void ValidationFailed(ulong clientId, bool kick = true, string additionalInfo = null, bool stackTrace = true);

    MyTimeSpan GetUpdateTime();

    MyPacketDataBitStreamBase GetBitStreamPacketData();

    void SendPendingReplicablesDone(Endpoint endpoint);

    void SendVoxelCacheInvalidated(string storageName, EndpointId endpoint);
  }
}
