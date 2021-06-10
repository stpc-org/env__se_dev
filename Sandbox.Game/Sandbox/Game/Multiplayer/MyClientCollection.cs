// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyClientCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
  public class MyClientCollection
  {
    private readonly Dictionary<ulong, MyNetworkClient> m_clients = new Dictionary<ulong, MyNetworkClient>();
    private HashSet<ulong> m_disconnectedClients = new HashSet<ulong>();
    private ulong m_localSteamId;
    public Action<ulong> ClientAdded;
    private object m_clientRemovedLock = new object();
    private Action<ulong> m_clientRemoved;

    public event Action<ulong> ClientRemoved
    {
      add
      {
        lock (this.m_clientRemovedLock)
          this.m_clientRemoved += value;
      }
      remove
      {
        lock (this.m_clientRemovedLock)
          this.m_clientRemoved -= value;
      }
    }

    public int Count => this.m_clients.Count;

    public MyNetworkClient LocalClient
    {
      get
      {
        MyNetworkClient myNetworkClient = (MyNetworkClient) null;
        this.m_clients.TryGetValue(this.m_localSteamId, out myNetworkClient);
        return myNetworkClient;
      }
    }

    public void SetLocalSteamId(ulong localSteamId, bool createLocalClient, string userName)
    {
      this.m_localSteamId = localSteamId;
      if (!createLocalClient || this.m_clients.ContainsKey(this.m_localSteamId))
        return;
      this.AddClient(this.m_localSteamId, userName);
    }

    public void Clear()
    {
      this.m_clients.Clear();
      this.m_disconnectedClients.Clear();
    }

    public bool TryGetClient(ulong steamId, out MyNetworkClient client)
    {
      client = (MyNetworkClient) null;
      return this.m_clients.TryGetValue(steamId, out client);
    }

    public bool HasClient(ulong steamId) => this.m_clients.ContainsKey(steamId);

    public MyNetworkClient AddClient(ulong steamId, string senderName)
    {
      if (this.m_clients.ContainsKey(steamId))
      {
        MyLog.Default.WriteLine("ERROR: Added client already present: " + this.m_clients[steamId].DisplayName);
        return this.m_clients[steamId];
      }
      MyNetworkClient myNetworkClient = new MyNetworkClient(steamId, senderName);
      this.m_clients.Add(steamId, myNetworkClient);
      this.m_disconnectedClients.Remove(steamId);
      this.RaiseClientAdded(steamId);
      return myNetworkClient;
    }

    public void RemoveClient(ulong steamId)
    {
      MyNetworkClient myNetworkClient;
      this.m_clients.TryGetValue(steamId, out myNetworkClient);
      if (myNetworkClient == null)
      {
        if (this.m_disconnectedClients.Contains(steamId))
          return;
        MyLog.Default.WriteLine("ERROR: Removed client not present: " + EndpointId.Format(steamId));
      }
      else
      {
        this.m_clients.Remove(steamId);
        this.m_disconnectedClients.Add(steamId);
        this.RaiseClientRemoved(steamId);
      }
    }

    private void RaiseClientAdded(ulong steamId) => this.ClientAdded.InvokeIfNotNull<ulong>(steamId);

    private void RaiseClientRemoved(ulong steamId) => this.m_clientRemoved.InvokeIfNotNull<ulong>(steamId);

    public Dictionary<ulong, MyNetworkClient>.ValueCollection GetClients() => this.m_clients.Values;
  }
}
