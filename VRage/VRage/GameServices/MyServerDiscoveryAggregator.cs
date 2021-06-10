// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyServerDiscoveryAggregator
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
  public class MyServerDiscoveryAggregator : IMyServerDiscovery
  {
    private const int MAX_INDEX = 65536;
    private readonly List<IMyServerDiscovery> m_serverDiscoveryList = new List<IMyServerDiscovery>();
    private int m_lanResponseCount;
    private MyMatchMakingServerResponse m_lanResponse;
    private int m_internetResponseCount;
    private MyMatchMakingServerResponse m_internetResponse;
    private int m_favoritesResponseCount;
    private MyMatchMakingServerResponse m_favoritesResponse;
    private int m_historyResponseCount;
    private MyMatchMakingServerResponse m_historyResponse;

    public string ServiceName => "null";

    public string ConnectionStringPrefix => "null://";

    public void AddAggregate(IMyServerDiscovery aggregate)
    {
      aggregate.OnLANServerListResponded += new EventHandler<int>(this.OnLanServerList);
      aggregate.OnLANServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnLanServersComplete);
      aggregate.OnDedicatedServerListResponded += new EventHandler<int>(this.OnDedicatedServerList);
      aggregate.OnDedicatedServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnDedicatedServersComplete);
      aggregate.OnFavoritesServerListResponded += new EventHandler<int>(this.OnFavoritesServerList);
      aggregate.OnFavoritesServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnFavoritesServersComplete);
      aggregate.OnHistoryServerListResponded += new EventHandler<int>(this.OnHistoryServerList);
      aggregate.OnHistoryServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnHistoryServersComplete);
      aggregate.OnPingServerResponded += new EventHandler<MyGameServerItem>(this.OnPingServer);
      aggregate.OnPingServerFailedToRespond += new EventHandler(this.OnPingServerFailed);
      aggregate.OnServerChangeRequested += this.OnServerChangeRequested;
      this.m_serverDiscoveryList.Add(aggregate);
    }

    private int LocalToGlobalIndex(IMyServerDiscovery aggregate, int idx) => idx + this.m_serverDiscoveryList.FindIndex((Predicate<IMyServerDiscovery>) (x => x == aggregate)) * 65536;

    private int GlobalToLocalIndex(int idx) => idx % 65536;

    private IMyServerDiscovery GlobalToAggregate(int idx) => this.m_serverDiscoveryList[idx / 65536];

    private int GetProviderIndex(string connectionString)
    {
      int num = 0;
      for (int index = 0; index < this.m_serverDiscoveryList.Count; ++index)
      {
        if (connectionString.StartsWith(this.m_serverDiscoveryList[index].ConnectionStringPrefix))
        {
          num = index;
          break;
        }
      }
      return num;
    }

    public IMyServerDiscovery FindAggregate(string connectionString) => this.m_serverDiscoveryList[this.GetProviderIndex(connectionString)];

    public bool LANSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.LANSupport));

    public event EventHandler<int> OnLANServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

    public MyGameServerItem GetLANServerDetails(int server) => this.GlobalToAggregate(server).GetLANServerDetails(this.GlobalToLocalIndex(server));

    public void RequestLANServerList()
    {
      this.m_lanResponseCount = 0;
      this.m_lanResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
      this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.RequestLANServerList()));
    }

    public void CancelLANServersRequest() => this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.CancelLANServersRequest()));

    private void OnLanServerList(object sender, int idx)
    {
      EventHandler<int> serverListResponded = this.OnLANServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded(sender, this.LocalToGlobalIndex((IMyServerDiscovery) sender, idx));
    }

    private void OnLanServersComplete(object sender, MyMatchMakingServerResponse response)
    {
      ++this.m_lanResponseCount;
      if (response < this.m_lanResponse)
        this.m_lanResponse = response;
      if (this.m_lanResponseCount != this.m_serverDiscoveryList.Count)
        return;
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnLANServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, this.m_lanResponse);
    }

    public bool DedicatedSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.DedicatedSupport));

    public event EventHandler<int> OnDedicatedServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

    public MyGameServerItem GetDedicatedServerDetails(int serverIndex) => this.GlobalToAggregate(serverIndex).GetDedicatedServerDetails(this.GlobalToLocalIndex(serverIndex));

    public bool SupportsDirectServerSearch => this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => x.SupportsDirectServerSearch));

    public void RequestServerItems(
      string[] connectionStrings,
      MySessionSearchFilter filter,
      Action<IEnumerable<MyGameServerItem>> resultCallback)
    {
      throw new NotImplementedException();
    }

    public MySupportedPropertyFilters SupportedSearchParameters => MySupportedPropertyFilters.Empty;

    public void RequestInternetServerList(MySessionSearchFilter filter)
    {
      this.m_internetResponseCount = 0;
      this.m_internetResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
      this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.RequestInternetServerList(filter)));
    }

    public void CancelInternetServersRequest() => this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.CancelInternetServersRequest()));

    private void OnDedicatedServerList(object sender, int idx)
    {
      EventHandler<int> serverListResponded = this.OnDedicatedServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded(sender, this.LocalToGlobalIndex((IMyServerDiscovery) sender, idx));
    }

    private void OnDedicatedServersComplete(object sender, MyMatchMakingServerResponse response)
    {
      ++this.m_internetResponseCount;
      if (response < this.m_internetResponse)
        this.m_internetResponse = response;
      if (this.m_internetResponseCount != this.m_serverDiscoveryList.Count)
        return;
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnDedicatedServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, response);
    }

    public bool FavoritesSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.FavoritesSupport));

    public event EventHandler<int> OnFavoritesServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

    public MyGameServerItem GetFavoritesServerDetails(int server) => this.GlobalToAggregate(server).GetFavoritesServerDetails(this.GlobalToLocalIndex(server));

    public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
    {
      this.m_favoritesResponseCount = 0;
      this.m_favoritesResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
      this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.RequestFavoritesServerList(filterOps)));
    }

    public void CancelFavoritesServersRequest() => this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.CancelFavoritesServersRequest()));

    private void OnFavoritesServerList(object sender, int idx)
    {
      EventHandler<int> serverListResponded = this.OnFavoritesServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded(sender, this.LocalToGlobalIndex((IMyServerDiscovery) sender, idx));
    }

    private void OnFavoritesServersComplete(object sender, MyMatchMakingServerResponse response)
    {
      ++this.m_favoritesResponseCount;
      if (response < this.m_favoritesResponse)
        this.m_favoritesResponse = response;
      if (this.m_favoritesResponseCount != this.m_serverDiscoveryList.Count)
        return;
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnFavoritesServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, response);
    }

    public void AddFavoriteGame(string connectionString) => this.m_serverDiscoveryList[this.GetProviderIndex(connectionString)].AddFavoriteGame(connectionString);

    public void AddFavoriteGame(MyGameServerItem serverItem) => this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].AddFavoriteGame(serverItem);

    public void RemoveFavoriteGame(MyGameServerItem serverItem) => this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].RemoveFavoriteGame(serverItem);

    public bool HistorySupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.HistorySupport));

    public event EventHandler<int> OnHistoryServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

    public MyGameServerItem GetHistoryServerDetails(int server) => this.GlobalToAggregate(server).GetHistoryServerDetails(this.GlobalToLocalIndex(server));

    public void RequestHistoryServerList(MySessionSearchFilter filterOps)
    {
      this.m_historyResponseCount = 0;
      this.m_historyResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
      this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.RequestHistoryServerList(filterOps)));
    }

    public void CancelHistoryServersRequest() => this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.CancelHistoryServersRequest()));

    private void OnHistoryServerList(object sender, int idx)
    {
      EventHandler<int> serverListResponded = this.OnHistoryServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded(sender, this.LocalToGlobalIndex((IMyServerDiscovery) sender, idx));
    }

    private void OnHistoryServersComplete(object sender, MyMatchMakingServerResponse response)
    {
      ++this.m_historyResponseCount;
      if (response < this.m_historyResponse)
        this.m_historyResponse = response;
      if (this.m_historyResponseCount != this.m_serverDiscoveryList.Count)
        return;
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnHistoryServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, response);
    }

    public void AddHistoryGame(MyGameServerItem serverItem) => this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].AddHistoryGame(serverItem);

    public bool PingSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.PingSupport));

    public event EventHandler<MyGameServerItem> OnPingServerResponded;

    public event EventHandler OnPingServerFailedToRespond;

    public void PingServer(string connectionString) => this.m_serverDiscoveryList[this.GetProviderIndex(connectionString)].PingServer(connectionString);

    private void OnPingServer(object sender, MyGameServerItem e)
    {
      EventHandler<MyGameServerItem> pingServerResponded = this.OnPingServerResponded;
      if (pingServerResponded == null)
        return;
      pingServerResponded(sender, e);
    }

    private void OnPingServerFailed(object sender, EventArgs e)
    {
      EventHandler serverFailedToRespond = this.OnPingServerFailedToRespond;
      if (serverFailedToRespond == null)
        return;
      serverFailedToRespond((object) this, EventArgs.Empty);
    }

    public event MyServerChangeRequested OnServerChangeRequested;

    public bool OnInvite(string dataProtocol)
    {
      bool ret = false;
      this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => ret |= x.OnInvite(dataProtocol)));
      return ret;
    }

    public void GetServerRules(
      MyGameServerItem serverItem,
      ServerRulesResponse completedAction,
      Action failedAction)
    {
      this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].GetServerRules(serverItem, (ServerRulesResponse) (y => completedAction(y)), (Action) (() => failedAction()));
    }

    public void GetPlayerDetails(
      MyGameServerItem serverItem,
      PlayerDetailsResponse completedAction,
      Action failedAction)
    {
      int rulesCounter = 0;
      bool completed = false;
      this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].GetPlayerDetails(serverItem, (PlayerDetailsResponse) (y =>
      {
        ++rulesCounter;
        completed = true;
        completedAction(y);
      }), (Action) (() =>
      {
        ++rulesCounter;
        if (rulesCounter != this.m_serverDiscoveryList.Count || completed)
          return;
        failedAction();
      }));
    }

    public bool FriendSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.FriendSupport));

    public bool GroupSupport => !this.m_serverDiscoveryList.TrueForAll((Predicate<IMyServerDiscovery>) (x => !x.GroupSupport));

    public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone) => this.m_serverDiscoveryList[this.GetProviderIndex(serverItem.ConnectionString)].Connect(serverItem, onDone);

    public void Disconnect() => this.m_serverDiscoveryList.ForEach((Action<IMyServerDiscovery>) (x => x.Disconnect()));

    public List<IMyServerDiscovery> GetAggregates() => this.m_serverDiscoveryList;
  }
}
