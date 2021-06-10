// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyNetworkingChat
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public interface IMyNetworkingChat
  {
    int GetChatMaxMessageSize();

    void SetPlayerMuted(ulong playerId, bool muted);

    MyPlayerChatState GetPlayerChatState(ulong playerId);

    bool IsTextChatAvailable { get; }

    bool IsCrossTextChatAvailable { get; }

    bool IsVoiceChatAvailable { get; }

    bool IsCrossVoiceChatAvailable { get; }

    bool IsTextChatAvailableForUserId(ulong userId, bool crossUser);

    bool IsVoiceChatAvailableForUserId(ulong userId, bool crossUser);

    void UpdateChatAvailability();

    void WarmupPlayerCache(ulong userId, bool crossUser);
  }
}
