// Decompiled with JetBrains decompiler
// Type: VRage.Network.IMyStateGroup
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Network
{
  public interface IMyStateGroup : IMyNetObject, IMyEventOwner
  {
    bool IsStreaming { get; }

    bool NeedsUpdate { get; }

    bool IsHighPriority { get; }

    void CreateClientData(MyClientStateBase forClient);

    void DestroyClientData(MyClientStateBase forClient);

    void ClientUpdate(MyTimeSpan clientTimestamp);

    void Destroy();

    void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData);

    void OnAck(MyClientStateBase forClient, byte packetId, bool delivered);

    void ForceSend(MyClientStateBase clientData);

    void Reset(bool reinit, MyTimeSpan clientTimestamp);

    bool IsStillDirty(Endpoint forClient);

    MyStreamProcessingState IsProcessingForClient(Endpoint forClient);

    IMyReplicable Owner { get; }
  }
}
