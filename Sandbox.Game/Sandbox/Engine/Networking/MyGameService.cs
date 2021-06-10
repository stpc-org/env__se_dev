// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyGameService
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform;
using System;
using System.Collections.Generic;
using VRage;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
  public static class MyGameService
  {
    private static IMyGameService m_gameServiceCache;
    private static IMyNetworking m_networkingCache;
    private static IMyGameServer m_gameServer;
    private static IMyInventoryService m_inventoryCache;
    private static IMyServerDiscovery m_serverDiscovery;
    private static IMyLobbyDiscovery m_lobbyDiscovery;
    private static IMyMicrophoneService m_microphoneCache;

    public static MyUGCAggregator WorkshopService { get; private set; }

    public static bool AtLeastOneUGCServiceConsented
    {
      get
      {
        foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
        {
          if (aggregate.IsConsentGiven)
            return true;
        }
        return false;
      }
    }

    static MyGameService()
    {
      MyGameService.WorkshopService = new MyUGCAggregator();
      MyServiceManager.Instance.OnChanged += new Action<object>(MyGameService.OnServiceChanged);
    }

    private static void OnServiceChanged(object obj) => MyGameService.ClearCache();

    public static void ClearCache()
    {
      MyGameService.m_gameServiceCache = (IMyGameService) null;
      MyGameService.m_networkingCache = (IMyNetworking) null;
      MyGameService.m_gameServer = (IMyGameServer) null;
      MyGameService.m_inventoryCache = (IMyInventoryService) null;
      MyGameService.m_serverDiscovery = (IMyServerDiscovery) null;
      MyGameService.m_lobbyDiscovery = (IMyLobbyDiscovery) null;
      MyGameService.m_microphoneCache = (IMyMicrophoneService) null;
    }

    private static bool EnsureGameService()
    {
      if (MyGameService.m_gameServiceCache == null)
        MyGameService.m_gameServiceCache = MyServiceManager.Instance.GetService<IMyGameService>();
      return MyGameService.m_gameServiceCache != null;
    }

    private static bool EnsureInventoryService()
    {
      if (MyGameService.m_inventoryCache == null)
        MyGameService.m_inventoryCache = MyServiceManager.Instance.GetService<IMyInventoryService>();
      return MyGameService.m_inventoryCache != null;
    }

    private static bool EnsureLobbyDiscovery()
    {
      if (MyGameService.m_lobbyDiscovery == null)
        MyGameService.m_lobbyDiscovery = MyServiceManager.Instance.GetService<IMyLobbyDiscovery>();
      return MyGameService.m_lobbyDiscovery != null;
    }

    private static bool EnsureServerDiscovery()
    {
      if (MyGameService.m_serverDiscovery == null)
        MyGameService.m_serverDiscovery = MyServiceManager.Instance.GetService<IMyServerDiscovery>();
      return MyGameService.m_serverDiscovery != null;
    }

    private static IMyNetworking EnsureNetworking()
    {
      IMyNetworking myNetworking = MyGameService.m_networkingCache;
      if (myNetworking == null)
      {
        myNetworking = MyGameService.m_networkingCache = MyServiceManager.Instance.GetService<IMyNetworking>();
        MyGameService.m_networkingCache?.Chat?.UpdateChatAvailability();
      }
      return myNetworking;
    }

    private static bool EnsureMicrophone()
    {
      if (MyGameService.m_microphoneCache == null)
        MyGameService.m_microphoneCache = MyServiceManager.Instance.GetService<IMyMicrophoneService>();
      return MyGameService.m_microphoneCache != null;
    }

    private static bool EnsureGameServer()
    {
      if (MyGameService.m_gameServer == null)
        MyGameService.m_gameServer = MyServiceManager.Instance.GetService<IMyGameServer>();
      return MyGameService.m_gameServer != null;
    }

    public static IMyGameService Service
    {
      get
      {
        MyGameService.EnsureGameService();
        return MyGameService.m_gameServiceCache;
      }
    }

    public static IMyNetworking Networking => MyGameService.EnsureNetworking();

    public static IMyServerDiscovery ServerDiscovery
    {
      get
      {
        MyGameService.EnsureServerDiscovery();
        return MyGameService.m_serverDiscovery;
      }
    }

    public static IMyLobbyDiscovery LobbyDiscovery
    {
      get
      {
        MyGameService.EnsureLobbyDiscovery();
        return MyGameService.m_lobbyDiscovery;
      }
    }

    public static IMyGameServer GameServer => !MyGameService.EnsureGameServer() ? (IMyGameServer) null : MyGameService.m_gameServer;

    public static IMyPeer2Peer Peer2Peer => MyGameService.EnsureNetworking()?.Peer2Peer;

    public static uint AppId => !MyGameService.EnsureGameService() ? 0U : MyGameService.m_gameServiceCache.AppId;

    public static bool IsActive => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsActive;

    public static bool IsOnline
    {
      get
      {
        if (MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsOnline)
          return true;
        return MyGameService.EnsureGameServer() && MyGameService.m_gameServer != null && MyGameService.m_gameServer.Running;
      }
    }

    public static bool IsOverlayBrowserAvailable => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsOverlayBrowserAvailable;

    public static bool IsOverlayEnabled => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsOverlayEnabled;

    public static bool OwnsGame => MyGameService.EnsureGameService() && MyGameService.IsActive && MyGameService.m_gameServiceCache.OwnsGame;

    public static ulong UserId
    {
      get => !MyGameService.EnsureGameService() ? ulong.MaxValue : MyGameService.m_gameServiceCache.UserId;
      set
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.UserId = value;
      }
    }

    public static string UserName
    {
      get
      {
        string str = (string) null;
        if (MyGameService.EnsureGameService())
          str = MyGameService.m_gameServiceCache.UserName;
        return str ?? string.Empty;
      }
    }

    public static MyGameServiceUniverse UserUniverse => MyGameService.EnsureGameService() ? MyGameService.m_gameServiceCache.UserUniverse : MyGameServiceUniverse.Invalid;

    public static string BranchName
    {
      get
      {
        if (Game.IsDedicated)
          return "DedicatedServer";
        if (!MyGameService.IsActive)
          return "SVN";
        return !MyGameService.EnsureGameService() || string.IsNullOrEmpty(MyGameService.m_gameServiceCache.BranchName) ? "" : MyGameService.m_gameServiceCache.BranchName;
      }
    }

    public static int RecycleTokens => MyGameService.EnsureInventoryService() ? MyGameService.m_inventoryCache.RecycleTokens : 0;

    public static string BranchNameFriendly => string.IsNullOrEmpty(MyGameService.BranchName) ? "default" : MyGameService.BranchName;

    public static ICollection<MyGameInventoryItem> InventoryItems => !MyGameService.EnsureInventoryService() ? (ICollection<MyGameInventoryItem>) null : MyGameService.m_inventoryCache.InventoryItems;

    public static IDictionary<int, MyGameInventoryItemDefinition> Definitions => !MyGameService.EnsureInventoryService() ? (IDictionary<int, MyGameInventoryItemDefinition>) null : MyGameService.m_inventoryCache.Definitions;

    public static IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(
      MyGameInventoryItemSlot slot)
    {
      return !MyGameService.EnsureInventoryService() ? (IEnumerable<MyGameInventoryItemDefinition>) null : MyGameService.m_inventoryCache.GetDefinitionsForSlot(slot);
    }

    public static event MyLobbyJoinRequested LobbyJoinRequested
    {
      add
      {
        if (!MyGameService.EnsureLobbyDiscovery())
          return;
        MyGameService.m_lobbyDiscovery.OnJoinLobbyRequested += value;
      }
      remove
      {
        if (!MyGameService.EnsureLobbyDiscovery())
          return;
        MyGameService.m_lobbyDiscovery.OnJoinLobbyRequested -= value;
      }
    }

    public static event MyServerChangeRequested ServerChangeRequested
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnServerChangeRequested += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnServerChangeRequested -= value;
      }
    }

    public static event EventHandler<MyGameServerItem> OnPingServerResponded
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnPingServerResponded += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnPingServerResponded -= value;
      }
    }

    public static event EventHandler OnPingServerFailedToRespond
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnPingServerFailedToRespond += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnPingServerFailedToRespond -= value;
      }
    }

    public static event EventHandler<int> OnFavoritesServerListResponded
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnFavoritesServerListResponded += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnFavoritesServerListResponded -= value;
      }
    }

    public static event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnFavoritesServersCompleteResponse += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnFavoritesServersCompleteResponse -= value;
      }
    }

    public static event EventHandler<int> OnHistoryServerListResponded
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnHistoryServerListResponded += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnHistoryServerListResponded -= value;
      }
    }

    public static event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnHistoryServersCompleteResponse += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnHistoryServersCompleteResponse -= value;
      }
    }

    public static event EventHandler<int> OnLANServerListResponded
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnLANServerListResponded += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnLANServerListResponded -= value;
      }
    }

    public static event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnLANServersCompleteResponse += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnLANServersCompleteResponse -= value;
      }
    }

    public static event EventHandler<int> OnDedicatedServerListResponded
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnDedicatedServerListResponded += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnDedicatedServerListResponded -= value;
      }
    }

    public static event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse
    {
      add
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnDedicatedServersCompleteResponse += value;
      }
      remove
      {
        if (!MyGameService.EnsureServerDiscovery())
          return;
        MyGameService.m_serverDiscovery.OnDedicatedServersCompleteResponse -= value;
      }
    }

    public static event EventHandler InventoryRefreshed
    {
      add
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.InventoryRefreshed += value;
      }
      remove
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.InventoryRefreshed -= value;
      }
    }

    public static event EventHandler<MyGameItemsEventArgs> CheckItemDataReady
    {
      add
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.CheckItemDataReady += value;
      }
      remove
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.CheckItemDataReady -= value;
      }
    }

    public static event EventHandler<MyGameItemsEventArgs> ItemsAdded
    {
      add
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.ItemsAdded += value;
      }
      remove
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.ItemsAdded -= value;
      }
    }

    public static event EventHandler NoItemsReceived
    {
      add
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.NoItemsReceived += value;
      }
      remove
      {
        if (!MyGameService.EnsureInventoryService())
          return;
        MyGameService.m_inventoryCache.NoItemsReceived -= value;
      }
    }

    public static event Action<bool> OnOverlayActivated
    {
      add
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnOverlayActivated += value;
      }
      remove
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnOverlayActivated -= value;
      }
    }

    public static event Action<uint> OnDLCInstalled
    {
      add
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnDLCInstalled += value;
      }
      remove
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnDLCInstalled -= value;
      }
    }

    public static event Action<bool> OnUserChanged
    {
      add
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnUserChanged += value;
      }
      remove
      {
        if (!MyGameService.EnsureGameService())
          return;
        MyGameService.m_gameServiceCache.OnUserChanged -= value;
      }
    }

    public static void OpenOverlayUrl(string url)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.OpenOverlayUrl(url);
    }

    public static void OpenInviteOverlay() => MyGameService.EnsureNetworking()?.Invite.OpenInviteOverlay();

    public static bool IsInviteSupported()
    {
      IMyNetworking myNetworking = MyGameService.EnsureNetworking();
      return myNetworking != null && myNetworking.Invite.IsInviteSupported();
    }

    internal static void SetNotificationPosition(NotificationPosition notificationPosition)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.SetNotificationPosition(notificationPosition);
    }

    public static void ShutDown()
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.ShutDown();
    }

    public static IMyAchievement GetAchievement(
      string achievementName,
      string statName,
      float statMaxValue)
    {
      if (!MyGameService.EnsureGameService())
        return (IMyAchievement) new MyGameService.MyNullAchievement();
      IMyAchievement myAchievement = MyGameService.m_gameServiceCache.GetAchievement(achievementName, statName, statMaxValue);
      if (myAchievement == null)
      {
        MyLog.Default.Error("Achievement " + achievementName + " is not implemented in game service " + MyGameService.m_gameServiceCache.ServiceName);
        myAchievement = (IMyAchievement) new MyGameService.MyNullAchievement();
      }
      return myAchievement;
    }

    public static bool IsAppInstalled(uint appId) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsActive && MyGameService.m_gameServiceCache.IsAppInstalled(appId);

    public static void OpenDlcInShop(uint dlcId)
    {
      if (!MyGameService.EnsureGameService() || !MyGameService.m_gameServiceCache.IsActive)
        return;
      MyGameService.m_gameServiceCache.OpenDlcInShop(dlcId);
    }

    public static void OpenInventoryItemInShop(int itemId)
    {
      if (!MyGameService.EnsureGameService() || !MyGameService.m_gameServiceCache.IsActive)
        return;
      MyGameService.m_gameServiceCache.OpenInventoryItemInShop(itemId);
    }

    public static bool IsDlcInstalled(uint dlcId) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsActive && MyGameService.m_gameServiceCache.IsDlcInstalled(dlcId);

    public static int GetDLCCount() => !MyGameService.EnsureGameService() || !MyGameService.m_gameServiceCache.IsActive ? 0 : MyGameService.m_gameServiceCache.GetDLCCount();

    public static bool GetDLCDataByIndex(
      int index,
      out uint dlcId,
      out bool available,
      out string name,
      int nameBufferSize)
    {
      if (MyGameService.EnsureGameService())
        return MyGameService.m_gameServiceCache.GetDLCDataByIndex(index, out dlcId, out available, out name, nameBufferSize);
      dlcId = 0U;
      available = false;
      name = (string) null;
      return false;
    }

    public static void OpenOverlayUser(ulong id)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.OpenOverlayUser(id);
    }

    public static bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
    {
      length = 0U;
      ticketHandle = 0U;
      return MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.GetAuthSessionTicket(out ticketHandle, buffer, out length);
    }

    public static void PingServer(string connectionString)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.PingServer(connectionString);
    }

    public static void OnThreadpoolInitialized()
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.OnThreadpoolInitialized();
    }

    public static void LoadStats()
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.LoadStats();
    }

    public static void ResetAllStats(bool achievementsToo)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.ResetAllStats(achievementsToo);
    }

    public static void StoreStats()
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.StoreStats();
    }

    public static void GetServerRules(
      MyGameServerItem serverItem,
      ServerRulesResponse completedAction,
      Action failedAction)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.GetServerRules(serverItem, completedAction, failedAction);
    }

    public static string GetPersonaName(ulong userId) => !MyGameService.EnsureGameService() ? string.Empty : MyGameService.m_gameServiceCache.GetPersonaName(userId);

    public static bool HasFriend(ulong userId) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.HasFriend(userId);

    public static string GetClanName(ulong groupId) => !MyGameService.EnsureGameService() ? string.Empty : MyGameService.m_gameServiceCache.GetClanName(groupId);

    public static bool HasGameServer => MyGameService.EnsureGameServer() && MyGameService.m_gameServer != null;

    public static void GetPlayerDetails(
      MyGameServerItem serverItem,
      PlayerDetailsResponse completedAction,
      Action failedAction)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.GetPlayerDetails(serverItem, completedAction, failedAction);
    }

    public static void AddFavoriteGame(string connectionString)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.AddFavoriteGame(connectionString);
    }

    public static void AddFavoriteGame(MyGameServerItem serverItem)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.AddFavoriteGame(serverItem);
    }

    public static void RemoveFavoriteGame(MyGameServerItem serverItem)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RemoveFavoriteGame(serverItem);
    }

    public static void AddHistoryGame(MyGameServerItem serverItem)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.AddHistoryGame(serverItem);
    }

    public static void Update()
    {
      if (MyGameService.EnsureGameService())
        MyGameService.m_gameServiceCache.Update();
      MyGameService.WorkshopService.Update();
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.Update();
    }

    public static bool IsUserInGroup(ulong groupId) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsUserInGroup(groupId);

    public static bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
    {
      totalBytes = availableBytes = 0UL;
      return !MyGameService.EnsureGameService() || MyGameService.m_gameServiceCache.GetRemoteStorageQuota(out totalBytes, out availableBytes);
    }

    public static int GetRemoteStorageFileCount() => !MyGameService.EnsureGameService() ? 0 : MyGameService.m_gameServiceCache.GetRemoteStorageFileCount();

    public static string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
    {
      fileSizeInBytes = 0;
      return !MyGameService.EnsureGameService() ? string.Empty : MyGameService.m_gameServiceCache.GetRemoteStorageFileNameAndSize(fileIndex, out fileSizeInBytes);
    }

    public static bool IsRemoteStorageFilePersisted(string file) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsRemoteStorageFilePersisted(file);

    public static bool RemoteStorageFileForget(string file) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.RemoteStorageFileForget(file);

    internal static void RequestFavoritesServerList(MySessionSearchFilter filterOps)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RequestFavoritesServerList(filterOps);
    }

    internal static void CancelFavoritesServersRequest()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.CancelFavoritesServersRequest();
    }

    internal static MyGameServerItem GetFavoritesServerDetails(int server) => !MyGameService.EnsureServerDiscovery() ? (MyGameServerItem) null : MyGameService.m_serverDiscovery.GetFavoritesServerDetails(server);

    internal static void RequestServerItems(
      string[] connectionStrings,
      MySessionSearchFilter filter,
      Action<IEnumerable<MyGameServerItem>> resultCallback)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RequestServerItems(connectionStrings, filter, resultCallback);
    }

    internal static void RequestInternetServerList(MySessionSearchFilter filterOps)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RequestInternetServerList(filterOps);
    }

    internal static void CancelInternetServersRequest()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.CancelInternetServersRequest();
    }

    internal static MyGameServerItem GetDedicatedServerDetails(int server) => !MyGameService.EnsureServerDiscovery() ? (MyGameServerItem) null : MyGameService.m_serverDiscovery.GetDedicatedServerDetails(server);

    internal static void RequestHistoryServerList(MySessionSearchFilter filterOps)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RequestHistoryServerList(filterOps);
    }

    internal static MyGameServerItem GetHistoryServerDetails(int server) => !MyGameService.EnsureServerDiscovery() ? (MyGameServerItem) null : MyGameService.m_serverDiscovery.GetHistoryServerDetails(server);

    internal static void CancelHistoryServersRequest()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.CancelHistoryServersRequest();
    }

    public static void RequestLANServerList()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.RequestLANServerList();
    }

    public static MyGameServerItem GetLANServerDetails(int server) => !MyGameService.EnsureServerDiscovery() ? (MyGameServerItem) null : MyGameService.m_serverDiscovery.GetLANServerDetails(server);

    public static void CancelLANServersRequest()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.CancelLANServersRequest();
    }

    internal static ulong CreatePublishedFileUpdateRequest(ulong publishedFileId) => !MyGameService.EnsureGameService() ? 0UL : MyGameService.m_gameServiceCache.CreatePublishedFileUpdateRequest(publishedFileId);

    internal static void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.UpdatePublishedFileTags(updateHandle, tags);
    }

    internal static void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.UpdatePublishedFileFile(updateHandle, steamItemFileName);
    }

    internal static void UpdatePublishedFilePreviewFile(
      ulong updateHandle,
      string steamPreviewFileName)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.UpdatePublishedFilePreviewFile(updateHandle, steamPreviewFileName);
    }

    internal static void CommitPublishedFileUpdate(
      ulong updateHandle,
      Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.CommitPublishedFileUpdate(updateHandle, onCallResult);
    }

    internal static void FileDelete(string steamItemFileName)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.FileDelete(steamItemFileName);
    }

    internal static void PublishWorkshopFile(
      string file,
      string previewFile,
      string title,
      string description,
      string longDescription,
      MyPublishedFileVisibility visibility,
      string[] tags,
      Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.PublishWorkshopFile(file, previewFile, title, description, longDescription, visibility, tags, onCallResult);
    }

    internal static void SubscribePublishedFile(
      ulong publishedFileId,
      Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.SubscribePublishedFile(publishedFileId, onCallResult);
    }

    internal static bool FileExists(string fileName) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.FileExists(fileName);

    internal static int GetFileSize(string fileName) => !MyGameService.EnsureGameService() ? 0 : MyGameService.m_gameServiceCache.GetFileSize(fileName);

    internal static ulong FileWriteStreamOpen(string fileName) => !MyGameService.EnsureGameService() ? 0UL : MyGameService.m_gameServiceCache.FileWriteStreamOpen(fileName);

    internal static void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.FileWriteStreamWriteChunk(handle, buffer, size);
    }

    internal static void FileWriteStreamClose(ulong handle)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.FileWriteStreamClose(handle);
    }

    internal static void FileShare(
      string file,
      Action<bool, MyRemoteStorageFileShareResult> onCallResult)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.FileShare(file, onCallResult);
    }

    internal static void GetAllInventoryItems()
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.GetAllItems();
    }

    internal static MyGameInventoryItemDefinition GetInventoryItemDefinition(
      string assetModifierId)
    {
      return !MyGameService.EnsureInventoryService() ? (MyGameInventoryItemDefinition) null : MyGameService.m_inventoryCache.GetInventoryItemDefinition(assetModifierId);
    }

    internal static bool HasInventoryItemWithDefinitionId(int id) => MyGameService.EnsureInventoryService() && MyGameService.m_inventoryCache.HasInventoryItemWithDefinitionId(id);

    internal static bool HasInventoryItem(ulong id) => MyGameService.EnsureInventoryService() && MyGameService.m_inventoryCache.HasInventoryItem(id);

    internal static bool HasInventoryItem(string assetModifierId) => MyGameService.EnsureInventoryService() && MyGameService.m_inventoryCache.HasInventoryItem(assetModifierId);

    internal static void TriggerPersonalContainer()
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.TriggerPersonalContainer();
    }

    internal static void TriggerCompetitiveContainer()
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.TriggerCompetitiveContainer();
    }

    internal static void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.GetItemCheckData(item, completedAction);
    }

    internal static void GetItemsCheckData(
      List<MyGameInventoryItem> items,
      Action<byte[]> completedAction)
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.GetItemsCheckData(items, completedAction);
    }

    internal static List<MyGameInventoryItem> CheckItemData(
      byte[] checkData,
      out bool checkResult)
    {
      if (MyGameService.EnsureInventoryService())
        return MyGameService.m_inventoryCache.CheckItemData(checkData, out checkResult);
      checkResult = false;
      return (List<MyGameInventoryItem>) null;
    }

    internal static void ConsumeItem(MyGameInventoryItem item)
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.ConsumeItem(item);
    }

    internal static void JoinLobby(ulong lobbyId, MyJoinResponseDelegate reponseDelegate)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.JoinLobby(lobbyId, reponseDelegate);
    }

    internal static IMyLobby CreateLobby(ulong lobbyId) => !MyGameService.EnsureLobbyDiscovery() ? (IMyLobby) null : MyGameService.m_lobbyDiscovery.CreateLobby(lobbyId);

    internal static void CreateLobby(
      MyLobbyType type,
      uint maxPlayers,
      MyLobbyCreated createdResponse)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.CreateLobby(type, maxPlayers, createdResponse);
    }

    internal static void AddFriendLobbies(List<IMyLobby> lobbies)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.AddFriendLobbies(lobbies);
    }

    internal static void AddPublicLobbies(List<IMyLobby> lobbies)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.AddPublicLobbies(lobbies);
    }

    internal static void RequestLobbyList(Action<bool> completed)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.RequestLobbyList(completed);
    }

    internal static void AddLobbyFilter(string key, string value)
    {
      if (!MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.AddLobbyFilter(key, value);
    }

    internal static int GetChatMaxMessageSize()
    {
      IMyNetworking myNetworking = MyGameService.EnsureNetworking();
      return myNetworking == null ? MyGameServiceConstants.MAX_CHAT_MESSAGE_SIZE : myNetworking.Chat.GetChatMaxMessageSize();
    }

    public static bool IsTextChatAvailable
    {
      get
      {
        IMyNetworking myNetworking = MyGameService.EnsureNetworking();
        return myNetworking != null && myNetworking.Chat.IsTextChatAvailable;
      }
    }

    public static bool IsVoiceChatAvailable
    {
      get
      {
        IMyNetworking myNetworking = MyGameService.EnsureNetworking();
        return myNetworking != null && myNetworking.Chat.IsVoiceChatAvailable;
      }
    }

    internal static MyGameServiceAccountType GetServerAccountType(
      ulong steamId)
    {
      return !MyGameService.EnsureGameService() ? MyGameServiceAccountType.Invalid : MyGameService.m_gameServiceCache.GetServerAccountType(steamId);
    }

    internal static void SetServerModTemporaryDirectory()
    {
      if (!MyGameService.EnsureGameServer())
        return;
      MyGameService.m_gameServer.SetServerModTemporaryDirectory();
    }

    public static void InitializeVoiceRecording()
    {
      if (!MyGameService.EnsureMicrophone())
        return;
      MyGameService.m_microphoneCache.InitializeVoiceRecording();
    }

    internal static void DisposeVoiceRecording()
    {
      if (!MyGameService.EnsureMicrophone())
        return;
      MyGameService.m_microphoneCache.DisposeVoiceRecording();
    }

    public static void StartVoiceRecording()
    {
      if (!MyGameService.EnsureMicrophone())
        return;
      MyGameService.m_microphoneCache.StartVoiceRecording();
    }

    public static void StopVoiceRecording()
    {
      if (!MyGameService.EnsureMicrophone())
        return;
      MyGameService.m_microphoneCache.StopVoiceRecording();
    }

    public static byte[] GetVoiceDataFormat() => !MyGameService.EnsureMicrophone() ? (byte[]) null : MyGameService.m_microphoneCache.GetVoiceDataFormat();

    public static bool IsMicrophoneFilteringSilence() => MyGameService.EnsureMicrophone() && MyGameService.m_microphoneCache.FiltersSilence;

    public static MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size)
    {
      if (MyGameService.EnsureMicrophone())
        return MyGameService.m_microphoneCache.GetAvailableVoice(ref buffer, out size);
      size = 0U;
      return MyVoiceResult.NotInitialized;
    }

    public static MyPlayerChatState GetPlayerMutedState(ulong playerId)
    {
      if (MyGameService.IsPlayerMutedInCloud(playerId))
        return MyPlayerChatState.Muted;
      IMyNetworking myNetworking = MyGameService.EnsureNetworking();
      return myNetworking == null ? MyPlayerChatState.Silent : myNetworking.Chat.GetPlayerChatState(playerId);
    }

    internal static bool IsPlayerMutedInCloud(ulong playerId) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsPlayerMuted(playerId);

    public static void UpdateMutedPlayersFromCloud(Action onDone = null)
    {
      if (MyGameService.EnsureGameService())
        MyGameService.m_gameServiceCache.UpdateMutedPlayers(onDone);
      else
        onDone.InvokeIfNotNull();
    }

    internal static void SetPlayerMuted(ulong playerId, bool muted)
    {
      if (MyGameService.EnsureGameService())
        MyGameService.m_gameServiceCache.SetPlayerMuted(playerId, muted);
      MyGameService.EnsureNetworking()?.Chat.SetPlayerMuted(playerId, muted);
    }

    internal static bool RecycleItem(MyGameInventoryItem item) => MyGameService.EnsureInventoryService() && MyGameService.m_inventoryCache.RecycleItem(item);

    internal static bool CraftSkin(MyGameInventoryItemQuality quality) => MyGameService.EnsureInventoryService() && MyGameService.m_inventoryCache.CraftSkin(quality);

    internal static uint GetCraftingCost(MyGameInventoryItemQuality quality) => !MyGameService.EnsureInventoryService() ? 0U : MyGameService.m_inventoryCache.GetCraftingCost(quality);

    internal static uint GetRecyclingReward(MyGameInventoryItemQuality quality) => !MyGameService.EnsureInventoryService() ? 0U : MyGameService.m_inventoryCache.GetRecyclingReward(quality);

    public static int GetFriendsCount() => !MyGameService.EnsureGameService() ? -1 : MyGameService.m_gameServiceCache.GetFriendsCount();

    public static ulong GetFriendIdByIndex(int index) => !MyGameService.EnsureGameService() ? 0UL : MyGameService.m_gameServiceCache.GetFriendIdByIndex(index);

    public static string GetFriendNameByIndex(int index) => !MyGameService.EnsureGameService() ? string.Empty : MyGameService.m_gameServiceCache.GetFriendNameByIndex(index);

    public static void SaveToCloudAsync(
      string fileName,
      byte[] buffer,
      Action<CloudResult> completedAction)
    {
      if (!MyGameService.EnsureGameService() && completedAction != null)
        completedAction(CloudResult.Failed);
      MyGameService.m_gameServiceCache.SaveToCloudAsync(fileName, buffer, completedAction);
    }

    public static CloudResult SaveToCloud(string fileName, byte[] buffer) => !MyGameService.EnsureGameService() ? CloudResult.Failed : MyGameService.m_gameServiceCache.SaveToCloud(fileName, buffer);

    public static CloudResult SaveToCloud(string containerName, List<MyCloudFile> files) => !MyGameService.EnsureGameService() ? CloudResult.Failed : MyGameService.m_gameServiceCache.SaveToCloud(containerName, files);

    public static List<MyCloudFileInfo> GetCloudFiles(string directoryFilter) => !MyGameService.EnsureGameService() ? (List<MyCloudFileInfo>) null : MyGameService.m_gameServiceCache.GetCloudFiles(directoryFilter);

    public static byte[] LoadFromCloud(string fileName) => !MyGameService.EnsureGameService() ? (byte[]) null : MyGameService.m_gameServiceCache.LoadFromCloud(fileName);

    public static bool LoadFromCloudAsync(string fileName, Action<byte[]> onDone) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.LoadFromCloudAsync(fileName, onDone);

    public static bool DeleteFromCloud(string fileName) => MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.DeleteFromCloud(fileName);

    public static bool IsUpdateAvailable() => false;

    public static MyWorkshopItem CreateWorkshopItem(string serviceName) => MyGameService.GetUGC(serviceName)?.CreateWorkshopItem();

    public static MyWorkshopItemPublisher CreateWorkshopPublisher(
      string serviceName)
    {
      return MyGameService.GetUGC(serviceName)?.CreateWorkshopPublisher();
    }

    public static MyWorkshopItemPublisher CreateWorkshopPublisher(
      MyWorkshopItem item)
    {
      return MyGameService.WorkshopService.CreateWorkshopPublisher(item);
    }

    public static MyWorkshopQuery CreateWorkshopQuery(string serviceName) => MyGameService.GetUGC(serviceName)?.CreateWorkshopQuery();

    public static IMyUGCService GetUGC(string serviceName) => MyGameService.WorkshopService.GetAggregate(serviceName);

    public static bool IsProductOwned(uint productId, out DateTime? purchaseTime)
    {
      purchaseTime = new DateTime?();
      return MyGameService.EnsureGameService() && MyGameService.m_gameServiceCache.IsProductOwned(productId, out purchaseTime);
    }

    public static void SuspendWorkshopDownloads() => MyGameService.WorkshopService.SuspendWorkshopDownloads();

    public static void ResumeWorkshopDownloads() => MyGameService.WorkshopService.ResumeWorkshopDownloads();

    public static void AddUnownedItems()
    {
      if (!MyGameService.EnsureInventoryService())
        return;
      MyGameService.m_inventoryCache.AddUnownedItems();
    }

    public static void OpenInShop(string shopUrl)
    {
      string[] strArray = shopUrl.Split(':');
      uint result;
      if (strArray.Length != 2 || !uint.TryParse(strArray[1], out result))
        return;
      if (strArray[0] == "app")
        MyGameService.OpenDlcInShop(result);
      else
        MyGameService.OpenInventoryItemInShop((int) result);
    }

    public static void Trace(bool enable)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.Trace(enable);
    }

    public static void AddDlcPackages(List<MyDlcPackage> packages)
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.AddDlcPackages(packages);
    }

    public static void ConnectToServer(MyGameServerItem server, Action<JoinResult> onDone)
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.Connect(server, onDone);
    }

    public static void DisconnectFromServer()
    {
      if (!MyGameService.EnsureServerDiscovery())
        return;
      MyGameService.m_serverDiscovery.Disconnect();
    }

    public static void UpdateNetworkThread()
    {
      if (!MyGameService.EnsureGameService())
        return;
      MyGameService.m_gameServiceCache.UpdateNetworkThread();
    }

    public static void OnInvite(string dataProtocol)
    {
      bool flag = false;
      if (MyGameService.EnsureServerDiscovery())
        flag = MyGameService.m_serverDiscovery.OnInvite(dataProtocol);
      if (flag || !MyGameService.EnsureLobbyDiscovery())
        return;
      MyGameService.m_lobbyDiscovery.OnInvite(dataProtocol);
    }

    public static void RequestPermissions(
      Permissions permission,
      bool attemptResolution,
      Action<PermissionResult> onDone)
    {
      if (MyGameService.EnsureGameService())
        MyGameService.m_gameServiceCache.RequestPermissions(permission, attemptResolution, onDone);
      else
        onDone.InvokeIfNotNull<PermissionResult>(PermissionResult.Error);
    }

    public static void RequestPermissionsWithTargetUser(
      Permissions permission,
      ulong userId,
      Action<PermissionResult> onDone)
    {
      if (MyGameService.EnsureGameService())
        MyGameService.m_gameServiceCache.RequestPermissionsWithTargetUser(permission, userId, onDone);
      else
        onDone.InvokeIfNotNull<PermissionResult>(PermissionResult.Error);
    }

    public static IMyUGCService GetDefaultUGC() => MyGameService.WorkshopService.GetAggregates()[0];

    public static string[] GetUGCNamesList() => MyGameService.WorkshopService.GetAggregates().ConvertAll<string>((Converter<IMyUGCService, string>) (x => x.ServiceName)).ToArray();

    public static int GetUGCIndex(string serviceName) => MyGameService.WorkshopService.GetAggregates().FindIndex((Predicate<IMyUGCService>) (x => x.ServiceName == serviceName));

    private class MyNullAchievement : IMyAchievement
    {
      public event Action OnStatValueChanged;

      public event Action OnUnlocked;

      public bool IsUnlocked => false;

      public int StatValueInt { get; set; }

      public float StatValueFloat { get; set; }

      public int StatValueConditionBitField { get; set; }

      public void Unlock()
      {
      }

      public void IndicateProgress(uint value)
      {
      }
    }
  }
}
