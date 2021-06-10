// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyVirtualClients
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [StaticEventOwner]
  internal class MyVirtualClients
  {
    private readonly List<MyVirtualClient> m_clients = new List<MyVirtualClient>();

    public void Init() => Sync.Players.NewPlayerRequestSucceeded += new Action<MyPlayer.PlayerId>(this.OnNewPlayerSuccess);

    public void Add(int idx)
    {
      int num = this.m_clients.Count + 1;
      MyPlayer.PlayerId freePlayerId = Sync.Players.FindFreePlayerId(Sync.MyId);
      Sync.Players.RequestNewPlayer(Sync.MyId, new MyPlayer.PlayerId(freePlayerId.SteamId, freePlayerId.SerialId + idx).SerialId, "Virtual " + Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, 0)).DisplayName + " #" + (object) (num + idx), (string) null, true, false);
    }

    private void OnNewPlayerSuccess(MyPlayer.PlayerId playerId)
    {
      if ((long) playerId.SteamId != (long) Sync.MyId || playerId.SerialId == 0 || !Sync.Players.GetPlayerById(playerId).IsRealPlayer)
        return;
      MyPlayerCollection.RespawnRequest(true, true, 0L, string.Empty, playerId.SerialId, (string) null, Color.Red);
      int num = this.m_clients.Count + 1;
      this.m_clients.Add(new MyVirtualClient(new Endpoint(Sync.MyId, (byte) num), (MyClientStateBase) MyVirtualClients.CreateClientState(), playerId));
      MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MyVirtualClients.OnVirtualClientAdded)), num);
    }

    [Event(null, 47)]
    [Reliable]
    [Server]
    private static void OnVirtualClientAdded(int index)
    {
      EndpointId id = MyEventContext.Current.IsLocallyInvoked ? new EndpointId(Sync.MyId) : MyEventContext.Current.Sender;
      MyReplicationServer replicationLayer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
      MyClientState clientState = MyVirtualClients.CreateClientState();
      Endpoint endpoint = new Endpoint(id, (byte) index);
      replicationLayer.AddClient(endpoint, (MyClientStateBase) clientState);
      ClientReadyDataMsg msg = new ClientReadyDataMsg()
      {
        ForcePlayoutDelayBuffer = MyFakes.ForcePlayoutDelayBuffer,
        UsePlayoutDelayBufferForCharacter = true,
        UsePlayoutDelayBufferForJetpack = true,
        UsePlayoutDelayBufferForGrids = true
      };
      replicationLayer.OnClientReady(endpoint, ref msg);
    }

    private static MyClientState CreateClientState() => Activator.CreateInstance(MyPerGameSettings.ClientStateType) as MyClientState;

    public void Tick()
    {
      foreach (MyVirtualClient client in this.m_clients)
        client.Tick();
    }

    public bool Any() => this.m_clients.Count > 0;

    public MyPlayer GetNextControlledPlayer(MyPlayer controllingPlayer)
    {
      if (!this.Any())
        return (MyPlayer) null;
      for (int index = 0; index < this.m_clients.Count; ++index)
      {
        if (Sync.Players.GetPlayerById(this.m_clients[index].PlayerId) == controllingPlayer)
          return index == this.m_clients.Count - 1 ? (MyPlayer) null : Sync.Players.GetPlayerById(this.m_clients[index + 1].PlayerId);
      }
      return Sync.Players.GetPlayerById(this.m_clients[0].PlayerId);
    }

    protected sealed class OnVirtualClientAdded\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int index,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVirtualClients.OnVirtualClientAdded(index);
      }
    }
  }
}
