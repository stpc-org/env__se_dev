// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.SimpleNetworkingChat
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Library.Parallelization;

namespace VRage.GameServices
{
  public class SimpleNetworkingChat : IMyNetworkingChat
  {
    private readonly IMyGameService m_service;
    private Action m_onPrivilegeUpdateDone;
    private AtomicFlag m_updatingPrivileges;
    private readonly int m_maxMessageSize;
    private readonly HashSet<ulong> m_mutedPlayers = new HashSet<ulong>();
    private readonly Dictionary<ulong, bool> m_textChatAvailable = new Dictionary<ulong, bool>();
    private readonly Dictionary<ulong, bool> m_voiceChatAvailable = new Dictionary<ulong, bool>();

    public bool IsTextChatAvailable { get; private set; }

    public bool IsCrossTextChatAvailable { get; private set; }

    public bool IsVoiceChatAvailable { get; private set; }

    public bool IsCrossVoiceChatAvailable { get; private set; }

    public SimpleNetworkingChat(IMyGameService service, int maxMessageSize)
    {
      this.m_service = service;
      this.m_maxMessageSize = maxMessageSize;
    }

    public bool IsTextChatAvailableForUserId(ulong userId, bool crossUser) => this.IsChatAvailableForUserId(userId, crossUser, this.IsTextChatAvailable, this.IsCrossTextChatAvailable, this.m_textChatAvailable, Permissions.CommunicationsText);

    public bool IsVoiceChatAvailableForUserId(ulong userId, bool crossUser) => this.IsChatAvailableForUserId(userId, crossUser, this.IsVoiceChatAvailable, this.IsCrossVoiceChatAvailable, this.m_voiceChatAvailable, Permissions.CommunicationsVoice);

    private bool IsChatAvailableForUserId(
      ulong userId,
      bool crossUser,
      bool permitted,
      bool crossPermitted,
      Dictionary<ulong, bool> userPermitted,
      Permissions permissionType)
    {
      if (!permitted)
        return false;
      if (crossUser)
        return crossPermitted;
      bool flag;
      if (!userPermitted.TryGetValue(userId, out flag))
      {
        this.m_service.RequestPermissionsWithTargetUser(permissionType, userId, (Action<PermissionResult>) (x => userPermitted[userId] = x == PermissionResult.Granted));
        flag = true;
      }
      return flag;
    }

    public virtual void UpdateChatAvailability()
    {
      if (!this.m_updatingPrivileges.Set())
        return;
      this.IsTextChatAvailable = false;
      this.IsCrossTextChatAvailable = false;
      this.IsVoiceChatAvailable = false;
      this.IsCrossVoiceChatAvailable = false;
      int remainingRequestChains = 2;
      this.m_service.RequestPermissions(Permissions.CommunicationsText, false, (Action<bool>) (granted =>
      {
        if (granted)
        {
          this.IsTextChatAvailable = true;
          this.m_service.RequestPermissionsWithTargetUser(Permissions.CommunicationsText, 0UL, (Action<bool>) (crossGranted =>
          {
            this.IsCrossTextChatAvailable = crossGranted;
            OnChainDone();
          }));
        }
        else
          OnChainDone();
      }));
      this.m_service.RequestPermissions(Permissions.CommunicationsVoice, false, (Action<bool>) (granted =>
      {
        if (granted)
        {
          this.IsVoiceChatAvailable = true;
          this.m_service.RequestPermissionsWithTargetUser(Permissions.CommunicationsVoice, 0UL, (Action<bool>) (crossGranted =>
          {
            this.IsCrossVoiceChatAvailable = crossGranted;
            OnChainDone();
          }));
        }
        else
          OnChainDone();
      }));

      void OnChainDone()
      {
        --remainingRequestChains;
        if (remainingRequestChains != 0)
          return;
        this.m_updatingPrivileges.Clear();
        this.m_onPrivilegeUpdateDone.InvokeIfNotNull();
        this.m_onPrivilegeUpdateDone = (Action) null;
      }
    }

    public void WarmupPlayerCache(ulong userId, bool crossUser)
    {
      if (this.m_updatingPrivileges.IsSet)
        this.m_onPrivilegeUpdateDone += new Action(UpdateCache);
      else
        UpdateCache();

      void UpdateCache()
      {
        this.IsTextChatAvailableForUserId(userId, crossUser);
        this.IsVoiceChatAvailableForUserId(userId, crossUser);
      }
    }

    public int GetChatMaxMessageSize() => this.m_maxMessageSize;

    public virtual MyPlayerChatState GetPlayerChatState(ulong playerId) => !this.m_mutedPlayers.Contains(playerId) ? MyPlayerChatState.Silent : MyPlayerChatState.Muted;

    public virtual void SetPlayerMuted(ulong playerId, bool muted)
    {
      if (muted)
        this.m_mutedPlayers.Add(playerId);
      else
        this.m_mutedPlayers.Remove(playerId);
    }
  }
}
