// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyServerDiscovery
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
  public interface IMyServerDiscovery
  {
    string ServiceName { get; }

    string ConnectionStringPrefix { get; }

    bool LANSupport { get; }

    event EventHandler<int> OnLANServerListResponded;

    event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

    bool DedicatedSupport { get; }

    event EventHandler<int> OnDedicatedServerListResponded;

    event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

    bool FavoritesSupport { get; }

    event EventHandler<int> OnFavoritesServerListResponded;

    event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

    bool HistorySupport { get; }

    event EventHandler<int> OnHistoryServerListResponded;

    event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

    bool FriendSupport { get; }

    bool PingSupport { get; }

    bool GroupSupport { get; }

    event EventHandler<MyGameServerItem> OnPingServerResponded;

    event EventHandler OnPingServerFailedToRespond;

    event MyServerChangeRequested OnServerChangeRequested;

    bool OnInvite(string dataProtocol);

    void RequestFavoritesServerList(MySessionSearchFilter filterOps);

    void CancelFavoritesServersRequest();

    MyGameServerItem GetFavoritesServerDetails(int server);

    MyGameServerItem GetDedicatedServerDetails(int serverIndex);

    bool SupportsDirectServerSearch { get; }

    void RequestServerItems(
      string[] connectionStrings,
      MySessionSearchFilter filter,
      Action<IEnumerable<MyGameServerItem>> resultCallback);

    MySupportedPropertyFilters SupportedSearchParameters { get; }

    void RequestInternetServerList(MySessionSearchFilter filter);

    void CancelInternetServersRequest();

    MyGameServerItem GetHistoryServerDetails(int server);

    void RequestHistoryServerList(MySessionSearchFilter filterOps);

    void CancelHistoryServersRequest();

    MyGameServerItem GetLANServerDetails(int server);

    void RequestLANServerList();

    void CancelLANServersRequest();

    void AddFavoriteGame(MyGameServerItem serverItem);

    void AddFavoriteGame(string connectionString);

    void RemoveFavoriteGame(MyGameServerItem serverItem);

    void AddHistoryGame(MyGameServerItem serverItem);

    void PingServer(string connectionString);

    void GetServerRules(
      MyGameServerItem serverItem,
      ServerRulesResponse completedAction,
      Action failedAction);

    void GetPlayerDetails(
      MyGameServerItem serverItem,
      PlayerDetailsResponse completedAction,
      Action failedAction);

    bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone);

    void Disconnect();
  }
}
