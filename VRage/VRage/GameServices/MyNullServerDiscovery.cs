// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullServerDiscovery
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
  public class MyNullServerDiscovery : IMyServerDiscovery
  {
    public string ServiceName => "null";

    public string ConnectionStringPrefix => "null://";

    public bool LANSupport => false;

    public event EventHandler<int> OnLANServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

    public bool DedicatedSupport => false;

    public event EventHandler<int> OnDedicatedServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

    public bool FavoritesSupport => false;

    public event EventHandler<int> OnFavoritesServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

    public bool HistorySupport => false;

    public event EventHandler<int> OnHistoryServerListResponded;

    public event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

    public bool FriendSupport => false;

    public bool PingSupport => false;

    public bool GroupSupport => false;

    public event EventHandler<MyGameServerItem> OnPingServerResponded;

    public event EventHandler OnPingServerFailedToRespond;

    public event MyServerChangeRequested OnServerChangeRequested;

    public MyGameServerItem GetFavoritesServerDetails(int server) => (MyGameServerItem) null;

    public bool OnInvite(string dataProtocol) => false;

    public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
    {
    }

    public void CancelFavoritesServersRequest()
    {
    }

    public MyGameServerItem GetDedicatedServerDetails(int serverIndex) => (MyGameServerItem) null;

    public bool SupportsDirectServerSearch => false;

    public void RequestServerItems(
      string[] connectionStrings,
      MySessionSearchFilter filter,
      Action<IEnumerable<MyGameServerItem>> resultCallback)
    {
    }

    public MySupportedPropertyFilters SupportedSearchParameters => MySupportedPropertyFilters.Empty;

    public void RequestInternetServerList(MySessionSearchFilter filter)
    {
    }

    public void CancelInternetServersRequest()
    {
    }

    public MyGameServerItem GetHistoryServerDetails(int server) => (MyGameServerItem) null;

    public void RequestHistoryServerList(MySessionSearchFilter filterOps)
    {
    }

    public void CancelHistoryServersRequest()
    {
    }

    public MyGameServerItem GetLANServerDetails(int server) => (MyGameServerItem) null;

    public void RequestLANServerList()
    {
    }

    public void CancelLANServersRequest()
    {
    }

    public void PingServer(string connectionString)
    {
    }

    public void GetServerRules(
      MyGameServerItem serverItem,
      ServerRulesResponse completedAction,
      Action failedAction)
    {
    }

    public void GetPlayerDetails(
      MyGameServerItem serverItem,
      PlayerDetailsResponse completedAction,
      Action failedAction)
    {
    }

    public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone)
    {
      onDone.InvokeIfNotNull<JoinResult>(JoinResult.OK);
      return true;
    }

    public void Disconnect()
    {
    }

    public void AddFavoriteGame(string connectionString)
    {
    }

    public void AddFavoriteGame(MyGameServerItem serverItem)
    {
    }

    public void RemoveFavoriteGame(MyGameServerItem serverItem)
    {
    }

    public void AddHistoryGame(MyGameServerItem serverItem)
    {
    }

    protected virtual void OnOnLanServerListResponded(int e)
    {
      EventHandler<int> serverListResponded = this.OnLANServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded((object) this, e);
    }

    protected virtual void OnOnLanServersCompleteResponse(MyMatchMakingServerResponse e)
    {
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnLANServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, e);
    }

    protected virtual void OnOnDedicatedServerListResponded(int e)
    {
      EventHandler<int> serverListResponded = this.OnDedicatedServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded((object) this, e);
    }

    protected virtual void OnOnDedicatedServersCompleteResponse(MyMatchMakingServerResponse e)
    {
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnDedicatedServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, e);
    }

    protected virtual void OnOnFavoritesServerListResponded(int e)
    {
      EventHandler<int> serverListResponded = this.OnFavoritesServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded((object) this, e);
    }

    protected virtual void OnOnFavoritesServersCompleteResponse(MyMatchMakingServerResponse e)
    {
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnFavoritesServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, e);
    }

    protected virtual void OnOnHistoryServerListResponded(int e)
    {
      EventHandler<int> serverListResponded = this.OnHistoryServerListResponded;
      if (serverListResponded == null)
        return;
      serverListResponded((object) this, e);
    }

    protected virtual void OnOnHistoryServersCompleteResponse(MyMatchMakingServerResponse e)
    {
      EventHandler<MyMatchMakingServerResponse> completeResponse = this.OnHistoryServersCompleteResponse;
      if (completeResponse == null)
        return;
      completeResponse((object) this, e);
    }

    protected virtual void OnOnPingServerResponded(MyGameServerItem e)
    {
      EventHandler<MyGameServerItem> pingServerResponded = this.OnPingServerResponded;
      if (pingServerResponded == null)
        return;
      pingServerResponded((object) this, e);
    }

    protected virtual void OnOnPingServerFailedToRespond()
    {
      EventHandler serverFailedToRespond = this.OnPingServerFailedToRespond;
      if (serverFailedToRespond == null)
        return;
      serverFailedToRespond((object) this, EventArgs.Empty);
    }

    protected virtual void OnOnServerChangeRequested(string server, string password)
    {
      MyServerChangeRequested serverChangeRequested = this.OnServerChangeRequested;
      if (serverChangeRequested == null)
        return;
      serverChangeRequested(server, password);
    }
  }
}
