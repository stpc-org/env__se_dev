// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyClientState
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
  [StaticEventOwner]
  public abstract class MyClientState : MyClientStateBase
  {
    public readonly Dictionary<long, HashSet<long>> KnownSectors = new Dictionary<long, HashSet<long>>();
    private MyEntity m_positionEntityServer;
    private bool m_pcuLimitSent;
    private float? m_lastSentReplicationRange;

    public MyClientState.MyContextKind Context { get; protected set; }

    public MyEntity ContextEntity { get; protected set; }

    public override Vector3D? Position
    {
      get => this.m_positionEntityServer == null || this.m_positionEntityServer.Closed ? base.Position : new Vector3D?(this.m_positionEntityServer.WorldMatrix.Translation);
      protected set => base.Position = value;
    }

    public override void Serialize(BitStream stream, bool outOfOrder)
    {
      if (stream.Writing)
        this.Write(stream);
      else
        this.Read(stream, outOfOrder);
    }

    public override void Update()
    {
      MyEntity controlledEntity;
      bool hasControl;
      this.GetControlledEntity(out controlledEntity, out hasControl);
      if (hasControl && controlledEntity != null)
        controlledEntity.ApplyLastControls();
      this.UpdateConrtolledEntityStates(controlledEntity, hasControl);
      float? replicationRange1 = this.m_lastSentReplicationRange;
      float? replicationRange2 = this.ReplicationRange;
      if ((double) replicationRange1.GetValueOrDefault() == (double) replicationRange2.GetValueOrDefault() & replicationRange1.HasValue == replicationRange2.HasValue)
        return;
      this.m_lastSentReplicationRange = this.ReplicationRange;
      MyMultiplayer.RaiseStaticEvent<float?>((Func<IMyEventOwner, Action<float?>>) (x => new Action<float?>(MyClientState.UpdateReplicationRange)), this.m_lastSentReplicationRange, this.EndpointId.Id);
    }

    private void UpdateConrtolledEntityStates(MyEntity controlledEntity, bool hasControl)
    {
      if (hasControl && controlledEntity != null)
      {
        if (controlledEntity is MyCharacter myCharacter)
        {
          this.IsControllingCharacter = !myCharacter.JetpackRunning;
          this.IsControllingJetpack = myCharacter.JetpackRunning;
          this.IsControllingGrid = false;
        }
        else
        {
          this.IsControllingCharacter = false;
          this.IsControllingJetpack = false;
          this.IsControllingGrid = controlledEntity is MyCubeGrid;
        }
      }
      else
        this.IsControllingCharacter = this.IsControllingJetpack = this.IsControllingGrid = false;
    }

    private void GetControlledEntity(out MyEntity controlledEntity, out bool hasControl)
    {
      controlledEntity = (MyEntity) null;
      hasControl = false;
      if (!Sync.IsServer && this.EndpointId.Index == (byte) 0 && (MySession.Static.HasCreativeRights && MySession.Static.CameraController == MySpectatorCameraController.Static) && ((MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.UserControlled || MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.Orbit) && (!(MySession.Static.TopMostControlledEntity is MyCharacter controlledEntity1) || !controlledEntity1.UpdateRotationsOverride)))
        return;
      foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity1 in Sync.Players.ControlledEntities)
      {
        if (controlledEntity1.Value == new MyPlayer.PlayerId(this.EndpointId.Id.Value, (int) this.EndpointId.Index))
        {
          controlledEntity = MyEntities.GetEntityById(controlledEntity1.Key);
          if (controlledEntity != null)
          {
            MyEntity topMostParent = controlledEntity.GetTopMostParent((Type) null);
            MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(topMostParent);
            if (controllingPlayer != null)
            {
              if (controlledEntity1.Value == controllingPlayer.Id)
              {
                controlledEntity = topMostParent;
                break;
              }
              break;
            }
            break;
          }
        }
      }
      if (controlledEntity == null)
        return;
      if (!Sync.IsServer)
      {
        MyPlayer player = this.GetPlayer();
        hasControl = MySession.Static.LocalHumanPlayer == player;
      }
      else
        hasControl = true;
    }

    private void Write(BitStream stream)
    {
      MyEntity controlledEntity = (MyEntity) null;
      bool hasControl = false;
      if (this.PlayerSerialId > 0)
      {
        MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(this.EndpointId.Id.Value, this.PlayerSerialId));
        if (playerById.Controller.ControlledEntity != null)
        {
          controlledEntity = playerById.Controller.ControlledEntity.Entity.GetTopMostParent((Type) null);
          hasControl = true;
        }
      }
      else
        this.GetControlledEntity(out controlledEntity, out hasControl);
      if (controlledEntity == null)
        controlledEntity = MySession.Static.CameraController.Entity;
      if (MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
        controlledEntity = (MyEntity) null;
      this.WriteShared(stream, controlledEntity, hasControl);
      long bitPosition1 = stream.BitPosition;
      stream.WriteInt16((short) 16);
      if (controlledEntity != null)
      {
        this.WriteInternal(stream, controlledEntity);
        controlledEntity.SerializeControls(stream);
        long bitPosition2 = stream.BitPosition;
        short num = (short) (stream.BitPosition - bitPosition1);
        stream.SetBitPositionWrite(bitPosition1);
        stream.WriteInt16(num);
        stream.SetBitPositionWrite(bitPosition2);
      }
      stream.WriteInt16(this.Ping);
      if (this.m_pcuLimitSent)
        return;
      this.m_pcuLimitSent = true;
      if (!MyPlatformGameSettings.DYNAMIC_REPLICATION_RADIUS || !MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX.HasValue)
        return;
      MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MyClientState.SendPCULimit)), MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX.Value);
    }

    private void Read(BitStream stream, bool outOfOrder)
    {
      MyEntity controlledEntity;
      this.ReadShared(stream, out controlledEntity);
      long bitPosition = stream.BitPosition;
      short num = stream.ReadInt16();
      bool flag = controlledEntity != null;
      if (flag)
      {
        MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(controlledEntity);
        flag = ((flag ? 1 : 0) & (controllingPlayer == null ? 0 : ((long) controllingPlayer.Client.SteamUserId == (long) this.EndpointId.Id.Value ? 1 : 0))) != 0;
      }
      if (flag)
      {
        this.ReadInternal(stream, controlledEntity);
        controlledEntity.DeserializeControls(stream, outOfOrder);
      }
      else
        stream.SetBitPositionRead(bitPosition + (long) num);
      this.Ping = stream.ReadInt16();
    }

    protected abstract void WriteInternal(BitStream stream, MyEntity controlledEntity);

    protected abstract void ReadInternal(BitStream stream, MyEntity controlledEntity);

    private void WriteShared(BitStream stream, MyEntity controlledEntity, bool hasControl)
    {
      stream.WriteBool(controlledEntity != null);
      if (controlledEntity == null)
      {
        if (!MySpectatorCameraController.Static.Initialized)
        {
          stream.WriteBool(false);
        }
        else
        {
          stream.WriteBool(true);
          Vector3D position = MySpectatorCameraController.Static.Position;
          stream.Serialize(ref position);
        }
      }
      else
      {
        stream.WriteInt64(controlledEntity.EntityId);
        stream.WriteBool(hasControl);
      }
    }

    private void ReadShared(BitStream stream, out MyEntity controlledEntity)
    {
      controlledEntity = (MyEntity) null;
      bool hasControl = stream.ReadBool();
      if (!hasControl)
      {
        if (stream.ReadBool())
        {
          Vector3D zero = Vector3D.Zero;
          stream.Serialize(ref zero);
          this.m_positionEntityServer = (MyEntity) null;
          this.Position = new Vector3D?(zero);
        }
      }
      else
      {
        long entityId = stream.ReadInt64();
        bool flag = stream.ReadBool();
        MyEntity myEntity;
        ref MyEntity local = ref myEntity;
        if (!MyEntities.TryGetEntityById(entityId, out local, true) || myEntity.GetTopMostParent((Type) null).MarkedForClose)
        {
          this.m_positionEntityServer = (MyEntity) null;
          return;
        }
        this.m_positionEntityServer = myEntity;
        if (!flag || !(myEntity.SyncObject is MySyncEntity))
          return;
        controlledEntity = myEntity;
      }
      this.UpdateConrtolledEntityStates(controlledEntity, hasControl);
    }

    public override IMyReplicable ControlledReplicable
    {
      get
      {
        MyPlayer player = this.GetPlayer();
        if (player == null)
          return (IMyReplicable) null;
        MyCharacter character = player.Character;
        return character == null ? (IMyReplicable) null : (IMyReplicable) MyExternalReplicable.FindByObject((object) character.GetTopMostParent((Type) null));
      }
    }

    public override IMyReplicable CharacterReplicable
    {
      get
      {
        MyPlayer player = this.GetPlayer();
        if (player == null)
          return (IMyReplicable) null;
        MyCharacter character = player.Character;
        return character == null ? (IMyReplicable) null : (IMyReplicable) MyExternalReplicable.FindByObject((object) character);
      }
    }

    public override void ResetControlledEntityControls()
    {
      MyEntity controlledEntity;
      this.GetControlledEntity(out controlledEntity, out bool _);
      controlledEntity?.ResetControls();
    }

    [Event(null, 323)]
    [Reliable]
    [Server]
    public static void AddKnownSector(long planetId, long sectorId)
    {
      MyReplicationServer replicationServer = MyMultiplayer.GetReplicationServer();
      if (replicationServer == null)
        return;
      MyClientState clientData = (MyClientState) replicationServer.GetClientData(new Endpoint(MyEventContext.Current.Sender, (byte) 0));
      if (clientData == null)
        return;
      HashSet<long> longSet;
      if (!clientData.KnownSectors.TryGetValue(planetId, out longSet))
      {
        longSet = new HashSet<long>();
        clientData.KnownSectors.Add(planetId, longSet);
      }
      longSet.Add(sectorId);
    }

    [Event(null, 348)]
    [Reliable]
    [Server]
    public static void RemoveKnownSector(long planetId, long sectorId)
    {
      MyReplicationServer replicationServer = MyMultiplayer.GetReplicationServer();
      if (replicationServer == null)
        return;
      MyClientState clientData = (MyClientState) replicationServer.GetClientData(new Endpoint(MyEventContext.Current.Sender, (byte) 0));
      HashSet<long> longSet;
      if (clientData == null || !clientData.KnownSectors.TryGetValue(planetId, out longSet))
        return;
      longSet.Remove(sectorId);
      if (longSet.Count != 0)
        return;
      clientData.KnownSectors.Remove(planetId);
    }

    public float? ServerReplicationRange { get; private set; }

    [Event(null, 381)]
    [Reliable]
    [Server]
    private static void SendPCULimit(int pcuLimit)
    {
      if (!(MyMultiplayer.Static.ReplicationLayer is MyReplicationServer replicationLayer))
        return;
      replicationLayer.SetClientPCULimit(MyEventContext.Current.Sender, pcuLimit);
    }

    [Event(null, 388)]
    [Reliable]
    [Client]
    private static void UpdateReplicationRange(float? range)
    {
      if (!(MyMultiplayer.Static.ReplicationLayer is MyReplicationClient replicationLayer))
        return;
      replicationLayer.SetClientReplicationRange(range);
    }

    public enum MyContextKind
    {
      None,
      Terminal,
      Inventory,
      Production,
      Building,
    }

    protected sealed class AddKnownSector\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long planetId,
        in long sectorId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyClientState.AddKnownSector(planetId, sectorId);
      }
    }

    protected sealed class RemoveKnownSector\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long planetId,
        in long sectorId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyClientState.RemoveKnownSector(planetId, sectorId);
      }
    }

    protected sealed class SendPCULimit\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int pcuLimit,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyClientState.SendPCULimit(pcuLimit);
      }
    }

    protected sealed class UpdateReplicationRange\u003C\u003ESystem_Nullable`1\u003CSystem_Single\u003E : ICallSite<IMyEventOwner, float?, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float? range,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyClientState.UpdateReplicationRange(range);
      }
    }
  }
}
