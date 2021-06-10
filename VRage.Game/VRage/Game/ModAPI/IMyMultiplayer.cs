// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyMultiplayer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI
{
  public interface IMyMultiplayer
  {
    bool MultiplayerActive { get; }

    bool IsServer { get; }

    ulong ServerId { get; }

    ulong MyId { get; }

    string MyName { get; }

    IMyPlayerCollection Players { get; }

    bool IsServerPlayer(IMyNetworkClient player);

    void SendEntitiesCreated(List<MyObjectBuilder_EntityBase> objectBuilders);

    bool SendMessageToServer(ushort id, byte[] message, bool reliable = true);

    bool SendMessageToOthers(ushort id, byte[] message, bool reliable = true);

    bool SendMessageTo(ushort id, byte[] message, ulong recipient, bool reliable = true);

    void JoinServer(string address);

    [Obsolete("Use RegisterSecureMessageHandler && UnregisterSecureMessageHandler pair instead", false)]
    void RegisterMessageHandler(ushort id, Action<byte[]> messageHandler);

    [Obsolete("Use RegisterSecureMessageHandler && UnregisterSecureMessageHandler pair instead", false)]
    void UnregisterMessageHandler(ushort id, Action<byte[]> messageHandler);

    void RegisterSecureMessageHandler(ushort id, Action<ushort, byte[], ulong, bool> messageHandler);

    void UnregisterSecureMessageHandler(
      ushort id,
      Action<ushort, byte[], ulong, bool> messageHandler);

    void ReplicateEntityForClient(long entityId, ulong steamId);
  }
}
