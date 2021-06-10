// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyLobby
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.GameServices
{
  public interface IMyLobby
  {
    ulong LobbyId { get; }

    bool IsValid { get; }

    ulong OwnerId { get; }

    MyLobbyType LobbyType { get; set; }

    ConnectionStrategy ConnectionStrategy { get; }

    int MemberCount { get; }

    int MemberLimit { get; set; }

    IEnumerable<ulong> MemberList { get; }

    string GetMemberName(ulong userId);

    void Leave();

    event KickedDelegate OnKicked;

    event MyLobbyDataUpdated OnDataReceived;

    bool RequestData();

    string GetData(string key);

    bool SetData(string key, string value, bool important = true);

    bool DeleteData(string key);

    event MessageReceivedDelegate OnChatReceived;

    event MyLobbyChatUpdated OnChatUpdated;

    bool SendChatMessage(string text, byte channel, long targetId = 0, string customAuthor = null);
  }
}
