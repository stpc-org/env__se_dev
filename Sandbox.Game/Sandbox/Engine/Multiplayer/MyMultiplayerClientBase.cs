// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerClientBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Replication;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Multiplayer
{
  public abstract class MyMultiplayerClientBase : MyMultiplayerBase, IReplicationClientCallback
  {
    private const int CONNECTION_STATE_TICKS = 12;
    private int m_ticks;
    private bool m_removingVoxelCacheFromServer;

    protected MyReplicationClient ReplicationLayer => (MyReplicationClient) base.ReplicationLayer;

    void IReplicationClientCallback.SendClientUpdate(IPacketData data) => this.SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_UPDATE, data, false, new EndpointId(this.ServerId));

    void IReplicationClientCallback.SendClientAcks(IPacketData data) => this.SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_ACKS, data, true, new EndpointId(this.ServerId));

    void IReplicationClientCallback.SendEvent(
      IPacketData data,
      bool reliable)
    {
      this.SyncLayer.TransportLayer.SendMessage(MyMessageId.RPC, data, reliable, new EndpointId(this.ServerId));
    }

    void IReplicationClientCallback.SendReplicableReady(IPacketData data) => this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_READY, data, true, new EndpointId(this.ServerId));

    void IReplicationClientCallback.SendReplicableRequest(IPacketData data) => this.SyncLayer.TransportLayer.SendMessage(MyMessageId.REPLICATION_REQUEST, data, true, new EndpointId(this.ServerId));

    void IReplicationClientCallback.SendConnectRequest(IPacketData data) => this.SyncLayer.TransportLayer.SendMessage(MyMessageId.CLIENT_CONNECTED, data, true, new EndpointId(this.ServerId));

    void IReplicationClientCallback.SendClientReady(
      MyPacketDataBitStreamBase data)
    {
      Sync.Layer.TransportLayer.SendMessage(MyMessageId.CLIENT_READY, (IPacketData) data, true, new EndpointId(Sync.ServerId));
    }

    void IReplicationClientCallback.ReadCustomState(BitStream stream)
    {
      Sync.ServerSimulationRatio = stream.ReadFloat();
      Sync.ServerCPULoadSmooth = Sync.ServerCPULoad = stream.ReadFloat();
      Sync.ServerThreadLoadSmooth = Sync.ServerThreadLoad = stream.ReadFloat();
    }

    public MyTimeSpan GetUpdateTime() => MySandboxGame.Static.SimulationTimeWithSpeed;

    public void SetNextFrameDelayDelta(float delay) => MySandboxGame.Static.SetNextFrameDelayDelta(delay);

    public void SetPing(long duration) => MyGeneralStats.Static.Ping = duration;

    public void SetIslandDone(byte index, Dictionary<long, MatrixD> matrices) => MyEntities.ReleaseWaitingAsync(index, matrices);

    public float GetServerSimulationRatio() => Sync.ServerSimulationRatio;

    public float GetClientSimulationRatio() => Math.Min(100f / MySandboxGame.Static.CPULoadSmooth, 1f);

    public void DisconnectFromHost() => this.DisconnectClient(0UL);

    public void UpdateSnapshotCache() => MySnapshotCache.Apply();

    public MyPacketDataBitStreamBase GetBitStreamPacketData() => MyNetworkWriter.GetBitStreamPacketData();

    public void PauseClient(bool pause)
    {
      if (pause)
      {
        MySandboxGame.PausePush();
        MyHud.Notifications.Add(MyNotificationSingletons.ConnectionProblem);
      }
      else
      {
        MySandboxGame.PausePop();
        MyHud.Notifications.Remove(MyNotificationSingletons.ConnectionProblem);
      }
    }

    protected MyMultiplayerClientBase(MySyncLayer syncLayer)
      : base(syncLayer)
    {
      this.SetReplicationLayer((MyReplicationLayer) new MyReplicationClient(new Endpoint(Sync.MyId, (byte) 0), (IReplicationClientCallback) this, (MyClientStateBase) this.CreateClientState(), 16.66667f, new Action<string>(this.JoinFailCallback), MyFakes.MULTIPLAYER_PREDICTION_RESET_CLIENT_FALLING_BEHIND, Thread.CurrentThread));
      this.ReplicationLayer.UseSmoothPing = MyFakes.MULTIPLAYER_SMOOTH_PING;
      MyReplicationClient.SynchronizationTimingType = MyReplicationClient.TimingType.LastServerTime;
      syncLayer.TransportLayer.Register(MyMessageId.SERVER_DATA, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.OnServerData));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_CREATE, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.ProcessReplicationCreate));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_DESTROY, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.ProcessReplicationDestroy));
      syncLayer.TransportLayer.Register(MyMessageId.SERVER_STATE_SYNC, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.OnServerStateSync));
      syncLayer.TransportLayer.Register(MyMessageId.RPC, (byte) 0, new Action<MyPacket>(((MyReplicationLayer) this.ReplicationLayer).OnEvent));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_STREAM_BEGIN, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.ProcessReplicationCreateBegin));
      syncLayer.TransportLayer.Register(MyMessageId.REPLICATION_ISLAND_DONE, (byte) 0, new Action<MyPacket>(this.ReplicationLayer.ProcessReplicationIslandDone));
      syncLayer.TransportLayer.Register(MyMessageId.WORLD, (byte) 0, new Action<MyPacket>(this.ReceiveWorld));
      syncLayer.TransportLayer.Register(MyMessageId.PLAYER_DATA, (byte) 0, new Action<MyPacket>(this.ReceivePlayerData));
      MyNetworkMonitor.OnTick += new Action(this.OnTick);
      this.m_voxelMapData = new LRUCache<string, byte[]>(100);
      this.m_voxelMapData.OnItemDiscarded += (Action<string, byte[]>) ((name, _) =>
      {
        if (this.m_removingVoxelCacheFromServer)
          return;
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (s => new Action<string>(MyMultiplayerBase.InvalidateVoxelCache)), name);
      });
    }

    public override void Dispose()
    {
      base.Dispose();
      MyNetworkMonitor.OnTick -= new Action(this.OnTick);
    }

    private void OnTick()
    {
      ++this.m_ticks;
      if (this.m_ticks <= 12)
        return;
      MyP2PSessionState state = new MyP2PSessionState();
      MyGameService.Peer2Peer?.GetSessionState(this.ServerId, ref state);
      this.IsConnectionDirect = !state.UsingRelay;
      this.IsConnectionAlive = state.ConnectionActive;
      this.m_ticks = 0;
    }

    private void JoinFailCallback(string message) => MyGuiSandbox.Show(new StringBuilder(message));

    public override void Tick()
    {
      base.Tick();
      MySession.Static.VirtualClients.Tick();
    }

    public override void OnSessionReady()
    {
      ClientReadyDataMsg msg = new ClientReadyDataMsg()
      {
        ForcePlayoutDelayBuffer = MyFakes.ForcePlayoutDelayBuffer,
        UsePlayoutDelayBufferForCharacter = true,
        UsePlayoutDelayBufferForJetpack = true,
        UsePlayoutDelayBufferForGrids = true
      };
      this.ReplicationLayer.SendClientReady(ref msg);
    }

    public override void DownloadWorld(int appVersion) => MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyMultiplayerServerBase.WorldRequest)), appVersion);

    public override void DownloadProfiler() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyMultiplayerServerBase.ProfilerRequest)));

    public void RequestBatchConfirmation() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MyMultiplayerServerBase.RequestBatchConfirmation)));

    public void ReceiveWorld(MyPacket packet)
    {
      byte[] andRead = MySerializer.CreateAndRead<byte[]>(packet.BitStream);
      if (andRead == null || andRead.Length == 0)
      {
        MyJoinGameHelper.WorldReceived((MyObjectBuilder_World) null, MyMultiplayer.Static);
      }
      else
      {
        MyObjectBuilder_World objectBuilder;
        MyObjectBuilderSerializer.DeserializeGZippedXML<MyObjectBuilder_World>((Stream) new MemoryStream(andRead), out objectBuilder);
        MyJoinGameHelper.WorldReceived(objectBuilder, MyMultiplayer.Static);
      }
      packet.Return();
    }

    public void ReceivePlayerData(MyPacket packet)
    {
      PlayerDataMsg andRead = MySerializer.CreateAndRead<PlayerDataMsg>(packet.BitStream);
      if (this.SyncLayer.TransportLayer.IsBuffering && (long) andRead.ClientSteamId != (long) Sync.MyId)
      {
        packet.BitStream.SetBitPositionRead(0L);
        this.SyncLayer.TransportLayer.AddMessageToBuffer(packet);
      }
      else
      {
        MySession.Static.Players.OnInitialPlayerCreated(andRead.ClientSteamId, andRead.PlayerSerialId, andRead.NewIdentity, andRead.PlayerBuilder);
        packet.Return();
      }
    }

    [Event(null, 261)]
    [Reliable]
    [Client]
    public static void ReceiveProfiler(byte[] profilerData)
    {
      MemoryStream memoryStream = new MemoryStream(profilerData);
      try
      {
        MyObjectBuilder_ProfilerSnapshot objectBuilder;
        MyObjectBuilderSerializer.DeserializeGZippedXML<MyObjectBuilder_ProfilerSnapshot>((Stream) memoryStream, out objectBuilder);
        objectBuilder.Init(MyRenderProxy.GetRenderProfiler(), VRage.Profiler.SnapshotType.Server, false);
        MyMultiplayer.Static.ProfilerDone.InvokeIfNotNull<string>("ProfilerDownload: Done.");
      }
      catch
      {
        MyMultiplayer.Static.ProfilerDone.InvokeIfNotNull<string>("ProfilerDownload: Could not parse data.");
      }
    }

    [Event(null, 279)]
    [Reliable]
    [Client]
    public static void ReceivePendingReplicablesDone() => MyMultiplayer.Static.ReceivePendingReplicablesDone();

    public override void KickClient(ulong client, bool kicked = true, bool add = true)
    {
      MyControlKickClientMsg message = new MyControlKickClientMsg()
      {
        KickedClient = client,
        Kicked = (BoolBlit) kicked
      };
      this.SendControlMessage<MyControlKickClientMsg>(this.ServerId, ref message);
    }

    protected override void OnClientKick(ref MyControlKickClientMsg data, ulong sender)
    {
      if ((bool) data.Kicked)
      {
        if ((long) data.KickedClient == (long) Sync.MyId)
        {
          MySessionLoader.UnloadAndExitToMenu();
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionKicked);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextYouHaveBeenKicked), messageCaption: messageCaption));
        }
        else
        {
          if ((bool) data.Add)
            this.AddKickedClient(data.KickedClient);
          this.RaiseClientLeft(data.KickedClient, MyChatMemberStateChangeEnum.Kicked);
        }
      }
      else
        this.RemoveKickedClient(data.KickedClient);
    }

    [Event(null, 315)]
    [Reliable]
    [Client]
    public static void InvalidateVoxelCacheClient(string storageName)
    {
      MyMultiplayerClientBase multiplayerClientBase = (MyMultiplayerClientBase) MyMultiplayer.Static;
      multiplayerClientBase.m_removingVoxelCacheFromServer = true;
      multiplayerClientBase.m_voxelMapData.Remove(storageName);
      multiplayerClientBase.m_removingVoxelCacheFromServer = false;
    }

    protected sealed class ReceiveProfiler\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in byte[] profilerData,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerClientBase.ReceiveProfiler(profilerData);
      }
    }

    protected sealed class ReceivePendingReplicablesDone\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyMultiplayerClientBase.ReceivePendingReplicablesDone();
      }
    }

    protected sealed class InvalidateVoxelCacheClient\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string storageName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayerClientBase.InvalidateVoxelCacheClient(storageName);
      }
    }
  }
}
