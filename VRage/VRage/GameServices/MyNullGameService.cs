// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullGameService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyNullGameService : IMyGameService
  {
    public uint AppId { get; }

    public bool IsActive { get; }

    public bool IsOnline { get; }

    public bool IsOverlayEnabled { get; }

    public bool IsOverlayBrowserAvailable { get; }

    public bool OwnsGame { get; }

    public ulong UserId
    {
      get => ulong.MaxValue;
      set
      {
      }
    }

    public string UserName { get; }

    public MyGameServiceUniverse UserUniverse { get; }

    public string BranchName { get; }

    public string BranchNameFriendly { get; }

    public event Action<bool> OnOverlayActivated;

    public event Action<uint> OnDLCInstalled;

    public event Action<bool> OnUserChanged;

    public event Action OnUpdate;

    public event Action OnUpdateNetworkThread;

    public void OpenOverlayUrl(string url)
    {
    }

    public void SetNotificationPosition(NotificationPosition notificationPosition)
    {
    }

    public void ShutDown()
    {
    }

    public bool IsAppInstalled(uint appId) => true;

    public void OpenDlcInShop(uint dlcId)
    {
    }

    public void OpenInventoryItemInShop(int itemId)
    {
    }

    public void AddDlcPackages(List<MyDlcPackage> packages)
    {
    }

    public bool IsDlcSupported(uint dlcId) => true;

    public bool IsDlcInstalled(uint dlcId) => true;

    public int GetDLCCount() => 0;

    public bool GetDLCDataByIndex(
      int index,
      out uint dlcId,
      out bool available,
      out string name,
      int nameBufferSize)
    {
      dlcId = 0U;
      available = false;
      name = string.Empty;
      return false;
    }

    public void OpenOverlayUser(ulong id)
    {
    }

    public bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
    {
      ticketHandle = 0U;
      length = 0U;
      return false;
    }

    public void LoadStats()
    {
    }

    public IMyAchievement GetAchievement(
      string achievementName,
      string statName,
      float maxValue)
    {
      return (IMyAchievement) null;
    }

    public IMyAchievement GetAchievement(string achievementName) => (IMyAchievement) null;

    public void RegisterAchievement(string achievementName, string XBLId)
    {
    }

    public void ResetAllStats(bool achievementsToo)
    {
    }

    public void StoreStats()
    {
    }

    public string GetPersonaName(ulong userId) => string.Empty;

    public bool HasFriend(ulong userId) => false;

    public string GetClanName(ulong groupId) => string.Empty;

    public void Update()
    {
      Action onUpdate = this.OnUpdate;
      if (onUpdate == null)
        return;
      onUpdate();
    }

    public void UpdateNetworkThread()
    {
      Action updateNetworkThread = this.OnUpdateNetworkThread;
      if (updateNetworkThread == null)
        return;
      updateNetworkThread();
    }

    public bool IsUserInGroup(ulong groupId) => false;

    public bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
    {
      totalBytes = 0UL;
      availableBytes = 0UL;
      return false;
    }

    public int GetRemoteStorageFileCount() => 0;

    public string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
    {
      fileSizeInBytes = 0;
      return string.Empty;
    }

    public bool IsRemoteStorageFilePersisted(string file) => false;

    public bool RemoteStorageFileForget(string file) => false;

    public ulong CreatePublishedFileUpdateRequest(ulong publishedFileId) => 0;

    public void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
    {
    }

    public void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
    {
    }

    public void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName)
    {
    }

    public void FileDelete(string steamItemFileName)
    {
    }

    public bool FileExists(string fileName) => false;

    public int GetFileSize(string fileName) => 0;

    public ulong FileWriteStreamOpen(string fileName) => 0;

    public void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
    {
    }

    public void FileWriteStreamClose(ulong handle)
    {
    }

    public void CommitPublishedFileUpdate(
      ulong updateHandle,
      Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
    {
    }

    public void PublishWorkshopFile(
      string file,
      string previewFile,
      string title,
      string description,
      string longDescription,
      MyPublishedFileVisibility visibility,
      string[] tags,
      Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
    {
    }

    public void SubscribePublishedFile(
      ulong publishedFileId,
      Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
    {
    }

    public void FileShare(
      string file,
      Action<bool, MyRemoteStorageFileShareResult> onCallResult)
    {
    }

    public string ServiceName { get; }

    public int GetFriendsCount() => 0;

    public ulong GetFriendIdByIndex(int index) => 0;

    public string GetFriendNameByIndex(int index) => string.Empty;

    public void SaveToCloudAsync(
      string storageName,
      byte[] buffer,
      Action<CloudResult> completedAction)
    {
      if (completedAction == null)
        return;
      completedAction(CloudResult.Failed);
    }

    public CloudResult SaveToCloud(string fileName, byte[] buffer) => CloudResult.Failed;

    public CloudResult SaveToCloud(string containerName, List<MyCloudFile> fileNames) => CloudResult.Failed;

    public bool LoadFromCloudAsync(string fileName, Action<byte[]> completedAction) => false;

    public List<MyCloudFileInfo> GetCloudFiles(string directoryFilter) => (List<MyCloudFileInfo>) null;

    public byte[] LoadFromCloud(string fileName) => (byte[]) null;

    public bool DeleteFromCloud(string fileName) => false;

    public bool IsProductOwned(uint productId, out DateTime? purchaseTime)
    {
      purchaseTime = new DateTime?();
      return false;
    }

    public void RequestEncryptedAppTicket(string url, Action<bool, string> onDone)
    {
    }

    public void RequestPermissions(
      Permissions permission,
      bool attemptResolution,
      Action<PermissionResult> onDone)
    {
      onDone.InvokeIfNotNull<PermissionResult>(PermissionResult.Granted);
    }

    public void RequestPermissionsWithTargetUser(
      Permissions permission,
      ulong userId,
      Action<PermissionResult> onDone)
    {
      onDone.InvokeIfNotNull<PermissionResult>(PermissionResult.Granted);
    }

    public void OnThreadpoolInitialized()
    {
    }

    public bool GetInstallStatus(out int percentage)
    {
      percentage = 0;
      return false;
    }

    public void Trace(bool enable)
    {
    }

    protected virtual void DoOnOverlayActivated(bool obj)
    {
      Action<bool> overlayActivated = this.OnOverlayActivated;
      if (overlayActivated == null)
        return;
      overlayActivated(obj);
    }

    protected virtual void OnDlcInstalled(uint obj)
    {
      Action<uint> onDlcInstalled = this.OnDLCInstalled;
      if (onDlcInstalled == null)
        return;
      onDlcInstalled(obj);
    }

    protected virtual void DoOnUserChanged()
    {
      Action<bool> onUserChanged = this.OnUserChanged;
      if (onUserChanged == null)
        return;
      onUserChanged(true);
    }

    public void SetPlayerMuted(ulong playerId, bool muted)
    {
    }

    public bool IsPlayerMuted(ulong playerId) => false;

    public void UpdateMutedPlayers(Action onDone) => onDone.InvokeIfNotNull();

    public MyGameServiceAccountType GetServerAccountType(ulong steamId) => MyGameServiceAccountType.Invalid;
  }
}
