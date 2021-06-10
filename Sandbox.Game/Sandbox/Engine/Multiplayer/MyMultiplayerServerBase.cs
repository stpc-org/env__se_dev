// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerServerBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Replication;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Multiplayer
{
  public abstract class MyMultiplayerServerBase : MyMultiplayerBase, IReplicationServerCallback
  {
    private readonly MyReplicableFactory m_factory = new MyReplicableFactory();

    protected MyReplicationServer ReplicationLayer => (MyReplicationServer) base.ReplicationLayer;

    protected MyMultiplayerServerBase(MySyncLayer syncLayer, EndpointId localClientEndpoint)
      : base(syncLayer)
    {
      this.SetReplicationLayer((MyReplicationLayer) new MyReplicationServer((IReplicationServerCallback) this, localClientEndpoint, Thread.CurrentThread));
      this.ClientLeft += (Action<ulong, MyChatMemberStateChangeEnum>) ((steamId, e) => MySandboxGame.Static.Invoke((Action) (() => this.ReplicationLayer.OnClientLeft(new EndpointId(steamId))), "P2P Client left"));
      this.ClientJoined += (Action<ulong, string>) ((steamId, steamName) => this.ReplicationLayer.OnClientJoined(new EndpointId(steamId), (MyClientStateBase) this.CreateClientState()));
      MyEntities.OnEntityCreate += new Action<MyEntity>(this.CreateReplicableForObject);
      MyEntityComponentBase.OnAfterAddedToContainer += new Action<MyEntityComponentBase>(this.CreateReplicableForObject);
      MyExternalReplicable.Destroyed += new Action<MyExternalReplicable>(this.DestroyReplicable);
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        this.CreateReplicableForObject((object) entity);
        MyEntityComponentContainer components = entity.Components;
        if (components != null)
        {
          foreach (object obj in (MyComponentContainer) components)
            this.CreateReplicableForObject(obj);
        }
      }
      syncLayer.TransportLayer.Register(MyMessageId.RPC, byte.MaxValue, new Action<MyPacket>(((MyReplicationLayer) this.ReplicationLayer).OnEvent));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_READY, byte.MaxValue, new Action<MyPacket>(this.ReplicationLayer.ReplicableReady));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_REQUEST, byte.MaxValue, new Action<MyPacket>(this.ReplicationLayer.ReplicableRequest));
      syncLayer.TransportLayer.Register(MyMessageId.CLIENT_UPDATE, byte.MaxValue, new Action<MyPacket>(this.ReplicationLayer.OnClientUpdate));
      syncLayer.TransportLayer.Register(MyMessageId.CLIENT_ACKS, byte.MaxValue, new Action<MyPacket>(this.ReplicationLayer.OnClientAcks));
      syncLayer.TransportLayer.Register(MyMessageId.CLIENT_READY, byte.MaxValue, new Action<MyPacket>(this.ClientReady));
    }

    public void RaiseReplicableCreated(object obj) => this.CreateReplicableForObject(obj);

    private void CreateReplicableForObject(object obj)
    {
      switch (obj)
      {
        case null:
          return;
        case MyInventoryAggregate _:
          return;
        case MyEntity myEntity:
          if (myEntity.IsPreview)
            return;
          break;
      }
      Type typeFor = this.m_factory.FindTypeFor(obj);
      if (!(typeFor != (Type) null) || !this.ReplicationLayer.IsTypeReplicated(typeFor))
        return;
      MyExternalReplicable instance = (MyExternalReplicable) Activator.CreateInstance(typeFor);
      instance.Hook(obj);
      this.ReplicationLayer.Replicate((IMyReplicable) instance);
      instance.OnServerReplicate();
    }

    private void DestroyReplicable(MyExternalReplicable obj) => this.ReplicationLayer.Destroy((IMyReplicable) obj);

    public override void Dispose()
    {
      MyEntities.OnEntityCreate -= new Action<MyEntity>(this.CreateReplicableForObject);
      MyEntityComponentBase.OnAfterAddedToContainer -= new Action<MyEntityComponentBase>(this.CreateReplicableForObject);
      MyExternalReplicable.Destroyed -= new Action<MyExternalReplicable>(this.DestroyReplicable);
      base.Dispose();
    }

    private void ClientReady(MyPacket packet)
    {
      this.ReplicationLayer.OnClientReady(packet.Sender, packet);
      packet.Return();
    }

    [Event(null, 129)]
    [Reliable]
    [Server]
    public static void WorldRequest(int appVersion)
    {
      if (appVersion != (int) MyFinalBuildConstants.APP_VERSION)
        (MyMultiplayer.Static.ReplicationLayer as MyReplicationServer).SendWorld(new byte[0], MyEventContext.Current.Sender);
      else
        (MyMultiplayer.Static as MyMultiplayerServerBase).OnWorldRequest(MyEventContext.Current.Sender);
    }

    protected void OnWorldRequest(EndpointId sender)
    {
      MySandboxGame.Log.WriteLineAndConsole("World request received: " + this.GetMemberName(sender.Value));
      if (this.IsClientKickedOrBanned(sender.Value) || MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Banned.Contains(sender.Value))
      {
        MySandboxGame.Log.WriteLineAndConsole("Sending no world, because client has been kicked or banned: " + this.GetMemberName(sender.Value) + " (Client is probably modified.)");
        this.RaiseClientLeft(sender.Value, MyChatMemberStateChangeEnum.Banned);
      }
      else
      {
        if (!this.IsServer || MySession.Static == null)
          return;
        MySandboxGame.Log.WriteLine("...responding");
        long senderIdentity = MySession.Static.Players.TryGetIdentityId(sender.Value, 0);
        MySandboxGame.Log.WriteLine("World snapshot - START");
        MyObjectBuilder_World worldData = MySession.Static.GetWorld(false, true);
        MyObjectBuilder_Checkpoint checkpoint = worldData.Checkpoint;
        checkpoint.WorkshopId = new ulong?();
        checkpoint.CharacterToolbar = (MyObjectBuilder_Toolbar) null;
        checkpoint.Settings.ScenarioEditMode = checkpoint.Settings.ScenarioEditMode && !MySession.Static.LoadedAsMission;
        MyObjectBuilder_Gps objectBuilderGps;
        checkpoint.Gps.Dictionary.TryGetValue(senderIdentity, out objectBuilderGps);
        checkpoint.Gps.Dictionary.Clear();
        if (objectBuilderGps != null)
          checkpoint.Gps.Dictionary.Add(senderIdentity, objectBuilderGps);
        worldData.Clusters = new List<BoundingBoxD>();
        MyPhysics.SerializeClusters(worldData.Clusters);
        worldData.Planets = MySession.Static.GetPlanetObjectBuilders();
        this.SyncLayer.TransportLayer.SendFlush(sender.Value);
        MySandboxGame.Log.WriteLine("World snapshot - END");
        Parallel.Start((Action) (() =>
        {
          MyMultiplayerServerBase.CleanUpData(worldData, sender.Value, senderIdentity);
          using (MemoryStream memoryStream = new MemoryStream())
          {
            MyObjectBuilderSerializer.SerializeXML((Stream) memoryStream, (MyObjectBuilder_Base) worldData, MyObjectBuilderSerializer.XmlCompression.Gzip);
            this.ReplicationLayer.SendWorld(memoryStream.ToArray(), sender);
          }
        }), WorkPriority.Low);
      }
    }

    private static void CleanUpData(
      MyObjectBuilder_World worldData,
      ulong playerId,
      long senderIdentity)
    {
      if (worldData.Checkpoint.Factions?.Factions != null)
      {
        foreach (MyObjectBuilder_Faction faction in worldData.Checkpoint.Factions.Factions)
        {
          foreach (MyObjectBuilder_Station station in faction.Stations)
            station.StoreItems = new List<MyObjectBuilder_StoreItem>();
        }
        List<MyObjectBuilder_FactionsVisEntry> factionsVisEntryList = new List<MyObjectBuilder_FactionsVisEntry>();
        foreach (MyObjectBuilder_FactionsVisEntry playerToFactionsVi in worldData.Checkpoint.Factions.PlayerToFactionsVis)
        {
          if ((long) playerToFactionsVi.PlayerId == (long) playerId || playerToFactionsVi.IdentityId == senderIdentity)
            factionsVisEntryList.Add(playerToFactionsVi);
        }
        worldData.Checkpoint.Factions.PlayerToFactionsVis = factionsVisEntryList;
        List<MyObjectBuilder_PlayerFactionRelation> playerFactionRelationList = new List<MyObjectBuilder_PlayerFactionRelation>();
        foreach (MyObjectBuilder_PlayerFactionRelation relationsWithPlayer in worldData.Checkpoint.Factions.RelationsWithPlayers)
        {
          if (relationsWithPlayer.PlayerId == senderIdentity)
            playerFactionRelationList.Add(relationsWithPlayer);
        }
        worldData.Checkpoint.Factions.RelationsWithPlayers = playerFactionRelationList;
      }
      if (worldData.Checkpoint.AllPlayersData == null)
        return;
      SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> serializableDictionary = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>();
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in worldData.Checkpoint.AllPlayersData.Dictionary)
      {
        MyPlayer.PlayerId playerId1;
        ref MyPlayer.PlayerId local = ref playerId1;
        MyObjectBuilder_Checkpoint.PlayerId key = keyValuePair.Key;
        long clientId = (long) key.GetClientId();
        int serialId = keyValuePair.Key.SerialId;
        local = new MyPlayer.PlayerId((ulong) clientId, serialId);
        key = keyValuePair.Key;
        if ((long) key.GetClientId() == (long) playerId || MySession.Static.Players.IsPlayerOnline(ref playerId1))
          serializableDictionary.Dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      }
      worldData.Checkpoint.AllPlayersData = serializableDictionary;
    }

    private void ProfilerRequestAsync(WorkData data)
    {
      MyMultiplayerServerBase.ProfilerData profilerData = data as MyMultiplayerServerBase.ProfilerData;
      try
      {
        MyObjectBuilder_ProfilerSnapshot objectBuilder = MyObjectBuilder_ProfilerSnapshot.GetObjectBuilder(MyRenderProxy.GetRenderProfiler());
        VRage.Profiler.MyRenderProfiler.AddPause(false);
        MemoryStream memoryStream = new MemoryStream();
        MyObjectBuilderSerializer.SerializeXML((Stream) memoryStream, (MyObjectBuilder_Base) objectBuilder, MyObjectBuilderSerializer.XmlCompression.Gzip);
        profilerData.Buffer = memoryStream.ToArray();
        MyLog.Default.WriteLine("Profiler for " + MySession.Static.Players.TryGetIdentityNameFromSteamId(profilerData.Sender.Value) + " serialized");
      }
      catch
      {
        MyLog.Default.WriteLine("Profiler serialization for " + MySession.Static.Players.TryGetIdentityNameFromSteamId(profilerData.Sender.Value) + " crashed");
      }
    }

    private void OnProfilerRequestComplete(WorkData data)
    {
      MyMultiplayerServerBase.ProfilerData profilerData = data as MyMultiplayerServerBase.ProfilerData;
      this.SyncLayer.TransportLayer.SendFlush(profilerData.Sender.Value);
      MyMultiplayer.RaiseStaticEvent<byte[]>((Func<IMyEventOwner, Action<byte[]>>) (s => new Action<byte[]>(MyMultiplayerClientBase.ReceiveProfiler)), profilerData.Buffer, new EndpointId(profilerData.Sender.Value));
    }

    [Event(null, 297)]
    [Reliable]
    [Server]
    public static void ProfilerRequest() => (MyMultiplayer.Static as MyMultiplayerServerBase).OnProfilerRequest(MyEventContext.Current.Sender);

    private void OnProfilerRequest(EndpointId sender)
    {
      if (this.IsServer)
      {
        MyLog.Default.WriteLine("Profiler request received from " + MySession.Static.Players.TryGetIdentityNameFromSteamId(sender.Value));
        MyMultiplayerServerBase.ProfilerData profilerData1 = new MyMultiplayerServerBase.ProfilerData();
        profilerData1.Sender = sender;
        profilerData1.Priority = WorkPriority.Low;
        MyMultiplayerServerBase.ProfilerData profilerData2 = profilerData1;
        VRage.Profiler.MyRenderProfiler.AddPause(true);
        Parallel.Start(new Action<WorkData>(this.ProfilerRequestAsync), new Action<WorkData>(this.OnProfilerRequestComplete), (WorkData) profilerData2);
      }
      else
        MyLog.Default.WriteLine("Profiler request received from " + MySession.Static.Players.TryGetIdentityNameFromSteamId(sender.Value) + ", but ignored");
    }

    [Event(null, 322)]
    [Reliable]
    [Server]
    public static void RequestBatchConfirmation() => (MyMultiplayer.Static.ReplicationLayer as MyReplicationServer).SetClientBatchConfrmation(new Endpoint(MyEventContext.Current.Sender, (byte) 0), true);

    public void SendPendingReplicablesDone(Endpoint endpoint) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyMultiplayerClientBase.ReceivePendingReplicablesDone)), endpoint.Id);

    void IReplicationServerCallback.SendServerData(
      IPacketData data,
      Endpoint endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.SERVER_DATA, data, true, endpoint.Id, endpoint.Index);
    }

    void IReplicationServerCallback.SendReplicationCreate(
      IPacketData data,
      Endpoint endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_CREATE, data, true, endpoint.Id, endpoint.Index);
    }

    void IReplicationServerCallback.SendReplicationCreateStreamed(
      IPacketData data,
      Endpoint endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_STREAM_BEGIN, data, true, endpoint.Id, endpoint.Index);
    }

    void IReplicationServerCallback.SendReplicationDestroy(
      IPacketData data,
      List<EndpointId> endpoints)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_DESTROY, data, true, endpoints);
    }

    void IReplicationServerCallback.SendReplicationIslandDone(
      IPacketData data,
      Endpoint endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_ISLAND_DONE, data, true, endpoint.Id, endpoint.Index);
    }

    void IReplicationServerCallback.SendStateSync(
      IPacketData data,
      Endpoint endpoint,
      bool reliable)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.SERVER_STATE_SYNC, data, reliable, endpoint.Id, endpoint.Index);
    }

    void IReplicationServerCallback.SendWorldData(
      IPacketData data,
      List<EndpointId> endpoints)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.WORLD_DATA, data, true, endpoints);
    }

    void IReplicationServerCallback.SendWorld(
      IPacketData data,
      EndpointId endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.WORLD, data, true, endpoint);
    }

    void IReplicationServerCallback.SendJoinResult(
      IPacketData data,
      EndpointId endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.JOIN_RESULT, data, true, endpoint);
    }

    void IReplicationServerCallback.SendEvent(
      IPacketData data,
      bool reliable,
      List<EndpointId> endpoints)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, endpoints);
    }

    void IReplicationServerCallback.SentClientJoined(
      IPacketData data,
      EndpointId endpoint)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_CONNECTED, data, true, endpoint);
    }

    void IReplicationServerCallback.SendPlayerData(
      IPacketData data,
      List<EndpointId> endpoints)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.PLAYER_DATA, data, true, endpoints);
    }

    void IReplicationServerCallback.WriteCustomState(BitStream stream)
    {
      stream.WriteFloat(MyPhysics.SimulationRatio);
      float cpuLoad = MySandboxGame.Static.CPULoad;
      stream.WriteFloat(cpuLoad);
      float threadLoad = MySandboxGame.Static.ThreadLoad;
      stream.WriteFloat(threadLoad);
    }

    int IReplicationServerCallback.GetMTUSize() => MyGameService.Peer2Peer.MTUSize - 10;

    IMyReplicable IReplicationServerCallback.GetReplicableByEntityId(
      long entityId)
    {
      MyEntity entity;
      return MyEntities.TryGetEntityById(entityId, out entity) ? (IMyReplicable) MyExternalReplicable.FindByObject((object) entity) : (IMyReplicable) null;
    }

    void IReplicationServerCallback.SendVoxelCacheInvalidated(
      string storageName,
      EndpointId endpoint)
    {
      MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (s => new Action<string>(MyMultiplayerClientBase.InvalidateVoxelCacheClient)), storageName, endpoint);
    }

    public MyTimeSpan GetUpdateTime() => MySandboxGame.Static.SimulationTimeWithSpeed;

    public MyPacketDataBitStreamBase GetBitStreamPacketData() => MyNetworkWriter.GetBitStreamPacketData();

    public override void KickClient(ulong userId, bool kicked = true, bool add = true)
    {
      if (kicked)
      {
        MyControlKickClientMsg message = new MyControlKickClientMsg()
        {
          KickedClient = userId,
          Kicked = (BoolBlit) kicked,
          Add = (BoolBlit) add
        };
        MyLog.Default.WriteLineAndConsole("Player " + this.GetMemberName(userId) + " kicked");
        this.SendControlMessageToAll<MyControlKickClientMsg>(ref message);
        if (add)
          this.AddKickedClient(userId);
        this.RaiseClientLeft(userId, MyChatMemberStateChangeEnum.Kicked);
      }
      else
      {
        MyControlKickClientMsg message = new MyControlKickClientMsg()
        {
          KickedClient = userId,
          Kicked = (BoolBlit) kicked
        };
        MyLog.Default.WriteLineAndConsole("Player " + EndpointId.Format(userId) + " unkicked");
        this.RemoveKickedClient(userId);
        this.SendControlMessageToAll<MyControlKickClientMsg>(ref message);
      }
    }

    protected override void OnClientKick(ref MyControlKickClientMsg data, ulong sender)
    {
      if (!MySession.Static.IsUserAdmin(sender))
        return;
      this.KickClient(data.KickedClient, (bool) data.Kicked, true);
    }

    public void ValidationFailed(
      ulong clientId,
      bool kick = true,
      string additionalInfo = null,
      bool stackTrace = true)
    {
      MyLog.Default.WriteLine(MySession.Static.Players.TryGetIdentityNameFromSteamId(clientId) + (kick ? " was trying to cheat!" : "'s action was blocked."));
      if (additionalInfo != null)
        MyLog.Default.WriteLine(additionalInfo);
      if (stackTrace)
        MyLog.Default.WriteLine(Environment.StackTrace);
      int num = kick ? 1 : 0;
    }

    public override void OnSessionReady()
    {
    }

    private class ProfilerData : WorkData
    {
      public EndpointId Sender;
      public byte[] Buffer;
    }

    protected sealed class WorldRequest\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int appVersion,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerServerBase.WorldRequest(appVersion);
      }
    }

    protected sealed class ProfilerRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerServerBase.ProfilerRequest();
      }
    }

    protected sealed class RequestBatchConfirmation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerServerBase.RequestBatchConfirmation();
      }
    }
  }
}
