// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Network;
using VRage.Replication;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [StaticEventOwner]
  public static class MyMultiplayer
  {
    public const int CONTROL_CHANNEL = 0;
    public const int GAME_EVENT_CHANNEL = 2;
    public static byte[] Channels = new byte[2]
    {
      (byte) 0,
      (byte) 2
    };
    public const string HOST_NAME_TAG = "host";
    public const string WORLD_NAME_TAG = "world";
    public const string HOST_STEAM_ID_TAG = "host_steamId";
    public const string WORLD_SIZE_TAG = "worldSize";
    public const string APP_VERSION_TAG = "appVersion";
    public const string GAME_MODE_TAG = "gameMode";
    public const string DATA_HASH_TAG = "dataHash";
    public const string MOD_COUNT_TAG = "mods";
    public const string MOD_ITEM_TAG = "mod";
    public const string VIEW_DISTANCE_TAG = "view";
    public const string INVENTORY_MULTIPLIER_TAG = "inventoryMultiplier";
    public const string BLOCKS_INVENTORY_MULTIPLIER_TAG = "blocksInventoryMultiplier";
    public const string ASSEMBLER_MULTIPLIER_TAG = "assemblerMultiplier";
    public const string REFINERY_MULTIPLIER_TAG = "refineryMultiplier";
    public const string WELDER_MULTIPLIER_TAG = "welderMultiplier";
    public const string GRINDER_MULTIPLIER_TAG = "grinderMultiplier";
    public const string SCENARIO_TAG = "scenario";
    public const string SCENARIO_BRIEFING_TAG = "scenarioBriefing";
    public const string SCENARIO_START_TIME_TAG = "scenarioStartTime";
    public const string EXPERIMENTAL_MODE_TAG = "experimentalMode";
    public const string SESSION_CONFIG_TAG = "sc";
    private static MyReplicationSingle m_replicationOffline;

    public static MyMultiplayerBase Static
    {
      get => (MyMultiplayerBase) MyMultiplayerMinimalBase.Instance;
      set => MyMultiplayerMinimalBase.Instance = (MyMultiplayerMinimalBase) value;
    }

    public static Task? InitOfflineReplicationLayer(bool dispatchRegistration = true)
    {
      if (MyMultiplayer.m_replicationOffline == null)
      {
        MyReplicationSingle replicationLayer = new MyReplicationSingle(new EndpointId(Sync.MyId));
        MyMultiplayer.m_replicationOffline = replicationLayer;
        if (dispatchRegistration)
          return new Task?(Parallel.Start(new Action(Register), WorkPriority.VeryLow));
        Register();

        void Register() => replicationLayer.RegisterFromGameAssemblies();
      }
      return new Task?();
    }

    public static MyReplicationLayerBase ReplicationLayer
    {
      get
      {
        if (MyMultiplayer.Static != null)
          return (MyReplicationLayerBase) MyMultiplayer.Static.ReplicationLayer;
        MyMultiplayer.InitOfflineReplicationLayer(false);
        return (MyReplicationLayerBase) MyMultiplayer.m_replicationOffline;
      }
    }

    public static MyMultiplayerHostResult HostLobby(
      MyLobbyType lobbyType,
      int maxPlayers,
      MySyncLayer syncLayer)
    {
      MyMultiplayerHostResult ret = new MyMultiplayerHostResult();
      MyMultiplayerLobby mpLobby = (MyMultiplayerLobby) null;
      mpLobby = new MyMultiplayerLobby(syncLayer, lobbyType, (uint) maxPlayers, (MyLobbyCreated) ((lobby, success, reason) =>
      {
        bool flag = MyMultiplayer.Static != mpLobby;
        if (!ret.Cancelled | flag)
        {
          if (success && (flag || (long) lobby.OwnerId != (long) Sync.MyId))
          {
            success = false;
            lobby.Leave();
          }
          if (success)
          {
            lobby.LobbyType = lobbyType;
            mpLobby.SetLobby(lobby);
            mpLobby.ExperimentalMode = true;
          }
          else if (!flag)
            MyMultiplayer.Static = (MyMultiplayerBase) null;
          ret.RaiseDone(success, reason, MyMultiplayer.Static);
        }
        else
          MyMultiplayer.Static = (MyMultiplayerBase) null;
      }));
      MyMultiplayer.Static = (MyMultiplayerBase) mpLobby;
      return ret;
    }

    public static MyMultiplayerJoinResult JoinLobby(ulong lobbyId)
    {
      MyMultiplayerJoinResult ret = new MyMultiplayerJoinResult();
      MyGameService.JoinLobby(lobbyId, (MyJoinResponseDelegate) ((success, lobby, response) =>
      {
        if (ret.Cancelled)
          return;
        if (success && response == MyLobbyStatusCode.Success && (long) lobby.OwnerId == (long) Sync.MyId)
        {
          response = MyLobbyStatusCode.DoesntExist;
          lobby.Leave();
        }
        success &= response == MyLobbyStatusCode.Success;
        MyMultiplayerJoinResult multiplayerJoinResult = ret;
        int num1 = success ? 1 : 0;
        IMyLobby lobby1 = lobby;
        int num2 = (int) response;
        MyMultiplayerLobbyClient multiplayerLobbyClient;
        if (!success)
          multiplayerLobbyClient = (MyMultiplayerLobbyClient) null;
        else
          MyMultiplayer.Static = (MyMultiplayerBase) (multiplayerLobbyClient = new MyMultiplayerLobbyClient(lobby, new MySyncLayer(new MyTransportLayer(2))));
        multiplayerJoinResult.RaiseJoined(num1 != 0, lobby1, (MyLobbyStatusCode) num2, (MyMultiplayerBase) multiplayerLobbyClient);
      }));
      return ret;
    }

    public static void RaiseStaticEvent(
      Func<IMyEventOwner, Action> action,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2>(
      Func<IMyEventOwner, Action<T2>> action,
      T2 arg2,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2, T3>(
      Func<IMyEventOwner, Action<T2, T3>> action,
      T2 arg2,
      T3 arg3,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, arg3, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2, T3, T4>(
      Func<IMyEventOwner, Action<T2, T3, T4>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, arg3, arg4, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2, T3, T4, T5>(
      Func<IMyEventOwner, Action<T2, T3, T4, T5>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2, T3, T4, T5, T6>(
      Func<IMyEventOwner, Action<T2, T3, T4, T5, T6>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, T6, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint, position);
    }

    public static void RaiseStaticEvent<T2, T3, T4, T5, T6, T7>(
      Func<IMyEventOwner, Action<T2, T3, T4, T5, T6, T7>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      T7 arg7,
      EndpointId targetEndpoint = default (EndpointId),
      Vector3D? position = null)
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<IMyEventOwner, T2, T3, T4, T5, T6, T7, IMyEventOwner>((IMyEventOwner) null, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, arg6, arg7, targetEndpoint, position);
    }

    public static void RaiseEvent<T1>(T1 arg1, Func<T1, Action> action, EndpointId targetEndpoint = default (EndpointId)) where T1 : IMyEventOwner => MyMultiplayer.ReplicationLayer.RaiseEvent<T1, IMyEventOwner>(arg1, (IMyEventOwner) null, action, targetEndpoint);

    public static void RaiseEvent<T1, T2>(
      T1 arg1,
      Func<T1, Action<T2>> action,
      T2 arg2,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, targetEndpoint);
    }

    public static void RaiseEvent<T1, T2, T3>(
      T1 arg1,
      Func<T1, Action<T2, T3>> action,
      T2 arg2,
      T3 arg3,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, arg3, targetEndpoint);
    }

    public static void RaiseEvent<T1, T2, T3, T4>(
      T1 arg1,
      Func<T1, Action<T2, T3, T4>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, arg3, arg4, targetEndpoint);
    }

    public static void RaiseEvent<T1, T2, T3, T4, T5>(
      T1 arg1,
      Func<T1, Action<T2, T3, T4, T5>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, targetEndpoint);
    }

    public static void RaiseEvent<T1, T2, T3, T4, T5, T6>(
      T1 arg1,
      Func<T1, Action<T2, T3, T4, T5, T6>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint);
    }

    public static void RaiseEvent<T1, T2, T3, T4, T5, T6, T7>(
      T1 arg1,
      Func<T1, Action<T2, T3, T4, T5, T6, T7>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      T7 arg7,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6, T7, IMyEventOwner>(arg1, (IMyEventOwner) null, action, arg2, arg3, arg4, arg5, arg6, arg7, targetEndpoint);
    }

    public static void RaiseBlockingEvent<T1, T2, T3, T4, T5, T6>(
      T1 arg1,
      T6 arg6,
      Func<T1, Action<T2, T3, T4, T5>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
      where T6 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6>(arg1, arg6, action, arg2, arg3, arg4, arg5, targetEndpoint);
    }

    public static void RaiseBlockingEvent<T1, T2, T3, T4, T5, T6, T7>(
      T1 arg1,
      T7 arg7,
      Func<T1, Action<T2, T3, T4, T5, T6>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      EndpointId targetEndpoint = default (EndpointId))
      where T1 : IMyEventOwner
      where T7 : IMyEventOwner
    {
      MyMultiplayer.ReplicationLayer.RaiseEvent<T1, T2, T3, T4, T5, T6, T7>(arg1, arg7, action, arg2, arg3, arg4, arg5, arg6, targetEndpoint);
    }

    internal static MyReplicationServer GetReplicationServer() => MyMultiplayer.Static != null ? MyMultiplayer.Static.ReplicationLayer as MyReplicationServer : (MyReplicationServer) null;

    internal static MyReplicationClient GetReplicationClient() => MyMultiplayer.Static != null ? MyMultiplayer.Static.ReplicationLayer as MyReplicationClient : (MyReplicationClient) null;

    public static void ReplicateImmediatelly(IMyReplicable replicable, IMyReplicable dependency = null) => MyMultiplayer.GetReplicationServer()?.ForceReplicable(replicable, dependency);

    public static void RemoveForClientIfIncomplete(IMyEventProxy obj) => MyMultiplayer.GetReplicationServer()?.RemoveForClientIfIncomplete(obj);

    public static void TeleportControlledEntity(Vector3D location) => MyMultiplayer.RaiseStaticEvent<ulong, Vector3D>((Func<IMyEventOwner, Action<ulong, Vector3D>>) (x => new Action<ulong, Vector3D>(MyMultiplayer.OnTeleport)), MySession.Static.LocalHumanPlayer.Id.SteamId, location);

    [Event(null, 443)]
    [Reliable]
    [Server]
    private static void OnTeleport(ulong userId, Vector3D location)
    {
      if (Sync.IsValidEventOnServer && (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value)))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
      else
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(userId));
        if (playerById.Controller.ControlledEntity == null)
          return;
        if (playerById.Controller.ControlledEntity is MyCharacter controlledEntity)
        {
          if (controlledEntity.IsOnLadder)
            controlledEntity.GetOffLadder();
          if (controlledEntity.IsUsing is MyCockpit isUsing)
            isUsing.RemovePilot();
        }
        MyEntity topMostParent = playerById.Controller.ControlledEntity.Entity.GetTopMostParent((Type) null);
        MatrixD worldMatrix = topMostParent.WorldMatrix;
        worldMatrix.Translation = location;
        topMostParent.Teleport(worldMatrix, (object) null, false);
      }
    }

    public static string GetMultiplayerStats() => MyMultiplayer.Static != null ? MyMultiplayer.Static.ReplicationLayer.GetMultiplayerStat() : string.Empty;

    protected sealed class OnTeleport\u003C\u003ESystem_UInt64\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, ulong, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong userId,
        in Vector3D location,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyMultiplayer.OnTeleport(userId, location);
      }
    }
  }
}
