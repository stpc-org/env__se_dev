// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyLobbyDiscovery
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public interface IMyLobbyDiscovery
  {
    bool Supported { get; }

    bool FriendSupport { get; }

    bool ContinueToLobbySupported { get; }

    event MyLobbyJoinRequested OnJoinLobbyRequested;

    bool OnInvite(string protocolData);

    void JoinLobby(ulong lobbyId, MyJoinResponseDelegate responseDelegate);

    IMyLobby CreateLobby(ulong lobbyId);

    void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse);

    void RequestLobbyList(Action<bool> completed);

    void AddPublicLobbies(List<IMyLobby> lobbyList);

    void AddFriendLobbies(List<IMyLobby> lobbyList);

    void AddLobbyFilter(string key, string value);
  }
}
