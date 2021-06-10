// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyAiTargetManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI
{
  [StaticEventOwner]
  [PreloadRequired]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyAiTargetManager : MySessionComponentBase
  {
    private readonly HashSet<MyAiTargetBase> m_aiTargets = new HashSet<MyAiTargetBase>();
    private static Dictionary<KeyValuePair<long, long>, MyAiTargetManager.ReservedEntityData> m_reservedEntities;
    private static Dictionary<string, Dictionary<long, MyAiTargetManager.ReservedAreaData>> m_reservedAreas;
    private static Queue<KeyValuePair<long, long>> m_removeReservedEntities;
    private static Queue<KeyValuePair<string, long>> m_removeReservedAreas;
    private static long m_areaReservationCounter;
    public static MyAiTargetManager Static;

    public static event MyAiTargetManager.ReservationHandler OnReservationResult;

    public static event MyAiTargetManager.AreaReservationHandler OnAreaReservationResult;

    public bool IsEntityReserved(long entityId, long localId) => Sync.IsServer && MyAiTargetManager.m_reservedEntities.TryGetValue(new KeyValuePair<long, long>(entityId, localId), out MyAiTargetManager.ReservedEntityData _);

    public bool IsEntityReserved(long entityId) => this.IsEntityReserved(entityId, 0L);

    public void UnreserveEntity(long entityId, long localId)
    {
      if (!Sync.IsServer)
        return;
      MyAiTargetManager.m_reservedEntities.Remove(new KeyValuePair<long, long>(entityId, localId));
    }

    public void UnreserveEntity(long entityId) => this.UnreserveEntity(entityId, 0L);

    [Event(null, 84)]
    [Reliable]
    [Server]
    private static void OnReserveEntityRequest(
      long entityId,
      long reservationTimeMs,
      int senderSerialId)
    {
      EndpointId targetEndpoint = !MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
      bool flag = true;
      KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, 0L);
      MyAiTargetManager.ReservedEntityData reservedEntityData;
      if (MyAiTargetManager.m_reservedEntities.TryGetValue(key, out reservedEntityData))
      {
        if (reservedEntityData.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
          reservedEntityData.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L;
        else
          flag = false;
      }
      else
        MyAiTargetManager.m_reservedEntities.Add(key, new MyAiTargetManager.ReservedEntityData()
        {
          EntityId = entityId,
          ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L,
          ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
        });
      if (MyEventContext.Current.IsLocallyInvoked)
      {
        if (flag)
          MyAiTargetManager.OnReserveEntitySuccess(entityId, senderSerialId);
        else
          MyAiTargetManager.OnReserveEntityFailure(entityId, senderSerialId);
      }
      else if (flag)
        MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MyAiTargetManager.OnReserveEntitySuccess)), entityId, senderSerialId, targetEndpoint);
      else
        MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MyAiTargetManager.OnReserveEntityFailure)), entityId, senderSerialId, targetEndpoint);
    }

    [Event(null, 126)]
    [Reliable]
    [Client]
    private static void OnReserveEntitySuccess(long entityId, int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.ENTITY,
        EntityId = entityId,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, true);
    }

    [Event(null, 136)]
    [Reliable]
    [Client]
    private static void OnReserveEntityFailure(long entityId, int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.ENTITY,
        EntityId = entityId,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, false);
    }

    [Event(null, 146)]
    [Reliable]
    [Server]
    private static void OnReserveEnvironmentItemRequest(
      long entityId,
      int localId,
      long reservationTimeMs,
      int senderSerialId)
    {
      EndpointId targetEndpoint = !MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
      bool flag = true;
      KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, (long) localId);
      MyAiTargetManager.ReservedEntityData reservedEntityData;
      if (MyAiTargetManager.m_reservedEntities.TryGetValue(key, out reservedEntityData))
      {
        if (reservedEntityData.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
          reservedEntityData.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L;
        else
          flag = false;
      }
      else
        MyAiTargetManager.m_reservedEntities.Add(key, new MyAiTargetManager.ReservedEntityData()
        {
          EntityId = entityId,
          LocalId = localId,
          ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L,
          ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
        });
      if (MyEventContext.Current.IsLocallyInvoked)
      {
        if (flag)
          MyAiTargetManager.OnReserveEnvironmentItemSuccess(entityId, localId, senderSerialId);
        else
          MyAiTargetManager.OnReserveEnvironmentItemFailure(entityId, localId, senderSerialId);
      }
      else if (flag)
        MyMultiplayer.RaiseStaticEvent<long, int, int>((Func<IMyEventOwner, Action<long, int, int>>) (s => new Action<long, int, int>(MyAiTargetManager.OnReserveEnvironmentItemSuccess)), entityId, localId, senderSerialId, targetEndpoint);
      else
        MyMultiplayer.RaiseStaticEvent<long, int, int>((Func<IMyEventOwner, Action<long, int, int>>) (s => new Action<long, int, int>(MyAiTargetManager.OnReserveEnvironmentItemFailure)), entityId, localId, senderSerialId, targetEndpoint);
    }

    [Event(null, 189)]
    [Reliable]
    [Client]
    private static void OnReserveEnvironmentItemSuccess(
      long entityId,
      int localId,
      int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.ENVIRONMENT_ITEM,
        EntityId = entityId,
        LocalId = localId,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, true);
    }

    [Event(null, 205)]
    [Reliable]
    [Client]
    private static void OnReserveEnvironmentItemFailure(
      long entityId,
      int localId,
      int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.ENVIRONMENT_ITEM,
        EntityId = entityId,
        LocalId = localId,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, false);
    }

    [Event(null, 221)]
    [Reliable]
    [Server]
    private static void OnReserveVoxelPositionRequest(
      long entityId,
      Vector3I voxelPosition,
      long reservationTimeMs,
      int senderSerialId)
    {
      EndpointId targetEndpoint = !MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
      bool flag = true;
      MyVoxelBase result = (MyVoxelBase) null;
      if (!MySession.Static.VoxelMaps.Instances.TryGetValue(entityId, out result))
        return;
      Vector3I vector3I = result.StorageMax - result.StorageMin;
      KeyValuePair<long, long> key = new KeyValuePair<long, long>(entityId, (long) (voxelPosition.X + voxelPosition.Y * vector3I.X + voxelPosition.Z * vector3I.X * vector3I.Y));
      MyAiTargetManager.ReservedEntityData reservedEntityData;
      if (MyAiTargetManager.m_reservedEntities.TryGetValue(key, out reservedEntityData))
      {
        if (reservedEntityData.ReserverId == new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId))
          reservedEntityData.ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L;
        else
          flag = false;
      }
      else
        MyAiTargetManager.m_reservedEntities.Add(key, new MyAiTargetManager.ReservedEntityData()
        {
          EntityId = entityId,
          GridPos = voxelPosition,
          ReservationTimer = Stopwatch.GetTimestamp() + Stopwatch.Frequency * reservationTimeMs / 1000L,
          ReserverId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId)
        });
      if (MyEventContext.Current.IsLocallyInvoked)
      {
        if (flag)
          MyAiTargetManager.OnReserveVoxelPositionSuccess(entityId, voxelPosition, senderSerialId);
        else
          MyAiTargetManager.OnReserveVoxelPositionFailure(entityId, voxelPosition, senderSerialId);
      }
      else if (flag)
        MyMultiplayer.RaiseStaticEvent<long, Vector3I, int>((Func<IMyEventOwner, Action<long, Vector3I, int>>) (s => new Action<long, Vector3I, int>(MyAiTargetManager.OnReserveVoxelPositionSuccess)), entityId, voxelPosition, senderSerialId, targetEndpoint);
      else
        MyMultiplayer.RaiseStaticEvent<long, Vector3I, int>((Func<IMyEventOwner, Action<long, Vector3I, int>>) (s => new Action<long, Vector3I, int>(MyAiTargetManager.OnReserveVoxelPositionFailure)), entityId, voxelPosition, senderSerialId, targetEndpoint);
    }

    [Event(null, 271)]
    [Reliable]
    [Client]
    private static void OnReserveVoxelPositionSuccess(
      long entityId,
      Vector3I voxelPosition,
      int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.VOXEL,
        EntityId = entityId,
        GridPos = voxelPosition,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, true);
    }

    [Event(null, 287)]
    [Reliable]
    [Client]
    private static void OnReserveVoxelPositionFailure(
      long entityId,
      Vector3I voxelPosition,
      int senderSerialId)
    {
      if (MyAiTargetManager.OnReservationResult == null)
        return;
      MyAiTargetManager.ReservedEntityData entityData = new MyAiTargetManager.ReservedEntityData()
      {
        Type = MyReservedEntityType.VOXEL,
        EntityId = entityId,
        GridPos = voxelPosition,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnReservationResult(ref entityData, false);
    }

    public void RequestEntityReservation(long entityId, long reservationTimeMs, int senderSerialId) => MyMultiplayer.RaiseStaticEvent<long, long, int>((Func<IMyEventOwner, Action<long, long, int>>) (s => new Action<long, long, int>(MyAiTargetManager.OnReserveEntityRequest)), entityId, reservationTimeMs, senderSerialId);

    public void RequestEnvironmentItemReservation(
      long entityId,
      int localId,
      long reservationTimeMs,
      int senderSerialId)
    {
      MyMultiplayer.RaiseStaticEvent<long, int, long, int>((Func<IMyEventOwner, Action<long, int, long, int>>) (s => new Action<long, int, long, int>(MyAiTargetManager.OnReserveEnvironmentItemRequest)), entityId, localId, reservationTimeMs, senderSerialId);
    }

    public void RequestVoxelPositionReservation(
      long entityId,
      Vector3I voxelPosition,
      long reservationTimeMs,
      int senderSerialId)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3I, long, int>((Func<IMyEventOwner, Action<long, Vector3I, long, int>>) (s => new Action<long, Vector3I, long, int>(MyAiTargetManager.OnReserveVoxelPositionRequest)), entityId, voxelPosition, reservationTimeMs, senderSerialId);
    }

    public void RequestAreaReservation(
      string reservationName,
      Vector3D position,
      float radius,
      long reservationTimeMs,
      int senderSerialId)
    {
      MyMultiplayer.RaiseStaticEvent<string, Vector3D, float, long, int>((Func<IMyEventOwner, Action<string, Vector3D, float, long, int>>) (s => new Action<string, Vector3D, float, long, int>(MyAiTargetManager.OnReserveAreaRequest)), reservationName, position, radius, reservationTimeMs, senderSerialId);
    }

    [Event(null, 327)]
    [Reliable]
    [Server]
    private static void OnReserveAreaRequest(
      string reservationName,
      Vector3D position,
      float radius,
      long reservationTimeMs,
      int senderSerialId)
    {
      EndpointId targetEndpoint = !MyEventContext.Current.IsLocallyInvoked ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
      if (!MyAiTargetManager.m_reservedAreas.ContainsKey(reservationName))
        MyAiTargetManager.m_reservedAreas.Add(reservationName, new Dictionary<long, MyAiTargetManager.ReservedAreaData>());
      Dictionary<long, MyAiTargetManager.ReservedAreaData> reservedArea = MyAiTargetManager.m_reservedAreas[reservationName];
      bool flag = false;
      MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(targetEndpoint.Value, senderSerialId);
      foreach (KeyValuePair<long, MyAiTargetManager.ReservedAreaData> keyValuePair in reservedArea)
      {
        MyAiTargetManager.ReservedAreaData reservedAreaData = keyValuePair.Value;
        if ((reservedAreaData.WorldPosition - position).LengthSquared() <= (double) reservedAreaData.Radius * (double) reservedAreaData.Radius)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        reservedArea[MyAiTargetManager.m_areaReservationCounter++] = new MyAiTargetManager.ReservedAreaData()
        {
          WorldPosition = position,
          Radius = radius,
          ReservationTimer = MySandboxGame.Static.TotalTime + MyTimeSpan.FromMilliseconds((double) reservationTimeMs),
          ReserverId = playerId
        };
        MyMultiplayer.RaiseStaticEvent<long, string, Vector3D, float>((Func<IMyEventOwner, Action<long, string, Vector3D, float>>) (s => new Action<long, string, Vector3D, float>(MyAiTargetManager.OnReserveAreaAllSuccess)), MyAiTargetManager.m_areaReservationCounter, reservationName, position, radius);
        if (MyEventContext.Current.IsLocallyInvoked)
          MyAiTargetManager.OnReserveAreaSuccess(position, radius, senderSerialId);
        else
          MyMultiplayer.RaiseStaticEvent<Vector3D, float, int>((Func<IMyEventOwner, Action<Vector3D, float, int>>) (s => new Action<Vector3D, float, int>(MyAiTargetManager.OnReserveAreaSuccess)), position, radius, senderSerialId, targetEndpoint);
      }
      else if (MyEventContext.Current.IsLocallyInvoked)
        MyAiTargetManager.OnReserveAreaFailure(position, radius, senderSerialId);
      else
        MyMultiplayer.RaiseStaticEvent<Vector3D, float, int>((Func<IMyEventOwner, Action<Vector3D, float, int>>) (s => new Action<Vector3D, float, int>(MyAiTargetManager.OnReserveAreaFailure)), position, radius, senderSerialId, targetEndpoint);
    }

    [Event(null, 382)]
    [Reliable]
    [Client]
    private static void OnReserveAreaSuccess(Vector3D position, float radius, int senderSerialId)
    {
      if (MyAiTargetManager.OnAreaReservationResult == null)
        return;
      MyAiTargetManager.ReservedAreaData entityData = new MyAiTargetManager.ReservedAreaData()
      {
        WorldPosition = position,
        Radius = radius,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnAreaReservationResult(ref entityData, true);
    }

    [Event(null, 397)]
    [Reliable]
    [Client]
    private static void OnReserveAreaFailure(Vector3D position, float radius, int senderSerialId)
    {
      if (MyAiTargetManager.OnAreaReservationResult == null)
        return;
      MyAiTargetManager.ReservedAreaData entityData = new MyAiTargetManager.ReservedAreaData()
      {
        WorldPosition = position,
        Radius = radius,
        ReserverId = new MyPlayer.PlayerId(0UL, senderSerialId)
      };
      MyAiTargetManager.OnAreaReservationResult(ref entityData, false);
    }

    [Event(null, 412)]
    [Reliable]
    [Broadcast]
    private static void OnReserveAreaAllSuccess(
      long id,
      string reservationName,
      Vector3D position,
      float radius)
    {
      if (!MyAiTargetManager.m_reservedAreas.ContainsKey(reservationName))
        MyAiTargetManager.m_reservedAreas[reservationName] = new Dictionary<long, MyAiTargetManager.ReservedAreaData>();
      MyAiTargetManager.m_reservedAreas[reservationName].Add(id, new MyAiTargetManager.ReservedAreaData()
      {
        WorldPosition = position,
        Radius = radius
      });
    }

    [Event(null, 421)]
    [Reliable]
    [Broadcast]
    private static void OnReserveAreaCancel(string reservationName, long id)
    {
      Dictionary<long, MyAiTargetManager.ReservedAreaData> dictionary;
      if (!MyAiTargetManager.m_reservedAreas.TryGetValue(reservationName, out dictionary))
        return;
      dictionary.Remove(id);
    }

    public override void LoadData()
    {
      MyAiTargetManager.Static = this;
      MyAiTargetManager.m_reservedEntities = new Dictionary<KeyValuePair<long, long>, MyAiTargetManager.ReservedEntityData>();
      MyAiTargetManager.m_removeReservedEntities = new Queue<KeyValuePair<long, long>>();
      MyAiTargetManager.m_removeReservedAreas = new Queue<KeyValuePair<string, long>>();
      MyAiTargetManager.m_reservedAreas = new Dictionary<string, Dictionary<long, MyAiTargetManager.ReservedAreaData>>();
      MyEntities.OnEntityRemove += new Action<MyEntity>(this.OnEntityRemoved);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_aiTargets.Clear();
      MyEntities.OnEntityRemove -= new Action<MyEntity>(this.OnEntityRemoved);
      MyAiTargetManager.Static = (MyAiTargetManager) null;
    }

    public override bool IsRequiredByGame => true;

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!Sync.IsServer)
        return;
      foreach (KeyValuePair<KeyValuePair<long, long>, MyAiTargetManager.ReservedEntityData> reservedEntity in MyAiTargetManager.m_reservedEntities)
      {
        if (Stopwatch.GetTimestamp() > reservedEntity.Value.ReservationTimer)
          MyAiTargetManager.m_removeReservedEntities.Enqueue(reservedEntity.Key);
      }
      foreach (KeyValuePair<long, long> removeReservedEntity in MyAiTargetManager.m_removeReservedEntities)
        MyAiTargetManager.m_reservedEntities.Remove(removeReservedEntity);
      MyAiTargetManager.m_removeReservedEntities.Clear();
      foreach (KeyValuePair<string, Dictionary<long, MyAiTargetManager.ReservedAreaData>> reservedArea in MyAiTargetManager.m_reservedAreas)
      {
        foreach (KeyValuePair<long, MyAiTargetManager.ReservedAreaData> keyValuePair in reservedArea.Value)
        {
          if (MySandboxGame.Static.TotalTime > keyValuePair.Value.ReservationTimer)
            MyAiTargetManager.m_removeReservedAreas.Enqueue(new KeyValuePair<string, long>(reservedArea.Key, keyValuePair.Key));
        }
      }
      foreach (KeyValuePair<string, long> removeReservedArea in MyAiTargetManager.m_removeReservedAreas)
      {
        MyAiTargetManager.m_reservedAreas[removeReservedArea.Key].Remove(removeReservedArea.Value);
        MyMultiplayer.RaiseStaticEvent<string, long>((Func<IMyEventOwner, Action<string, long>>) (s => new Action<string, long>(MyAiTargetManager.OnReserveAreaCancel)), removeReservedArea.Key, removeReservedArea.Value);
      }
      MyAiTargetManager.m_removeReservedAreas.Clear();
    }

    public static void AddAiTarget(MyAiTargetBase aiTarget)
    {
      if (MyAiTargetManager.Static == null)
        return;
      MyAiTargetManager.Static.m_aiTargets.Add(aiTarget);
    }

    public static void RemoveAiTarget(MyAiTargetBase aiTarget) => MyAiTargetManager.Static?.m_aiTargets.Remove(aiTarget);

    private void OnEntityRemoved(MyEntity entity)
    {
      foreach (MyAiTargetBase aiTarget in this.m_aiTargets)
      {
        if (aiTarget.TargetEntity == entity)
          aiTarget.UnsetTarget();
      }
    }

    public bool IsInReservedArea(string areaName, Vector3D position)
    {
      Dictionary<long, MyAiTargetManager.ReservedAreaData> dictionary;
      if (MyAiTargetManager.m_reservedAreas.TryGetValue(areaName, out dictionary))
      {
        foreach (MyAiTargetManager.ReservedAreaData reservedAreaData in dictionary.Values)
        {
          if ((reservedAreaData.WorldPosition - position).LengthSquared() < (double) reservedAreaData.Radius * (double) reservedAreaData.Radius)
            return true;
        }
      }
      return false;
    }

    public struct ReservedEntityData
    {
      public MyReservedEntityType Type;
      public long EntityId;
      public int LocalId;
      public Vector3I GridPos;
      public long ReservationTimer;
      public MyPlayer.PlayerId ReserverId;
    }

    public struct ReservedAreaData
    {
      public Vector3D WorldPosition;
      public float Radius;
      public MyTimeSpan ReservationTimer;
      public MyPlayer.PlayerId ReserverId;
    }

    public delegate void ReservationHandler(
      ref MyAiTargetManager.ReservedEntityData entityData,
      bool success);

    public delegate void AreaReservationHandler(
      ref MyAiTargetManager.ReservedAreaData entityData,
      bool success);

    protected sealed class OnReserveEntityRequest\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in long reservationTimeMs,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEntityRequest(entityId, reservationTimeMs, senderSerialId);
      }
    }

    protected sealed class OnReserveEntitySuccess\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int senderSerialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEntitySuccess(entityId, senderSerialId);
      }
    }

    protected sealed class OnReserveEntityFailure\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int senderSerialId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEntityFailure(entityId, senderSerialId);
      }
    }

    protected sealed class OnReserveEnvironmentItemRequest\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, long, int, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int localId,
        in long reservationTimeMs,
        in int senderSerialId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEnvironmentItemRequest(entityId, localId, reservationTimeMs, senderSerialId);
      }
    }

    protected sealed class OnReserveEnvironmentItemSuccess\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int localId,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEnvironmentItemSuccess(entityId, localId, senderSerialId);
      }
    }

    protected sealed class OnReserveEnvironmentItemFailure\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int localId,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveEnvironmentItemFailure(entityId, localId, senderSerialId);
      }
    }

    protected sealed class OnReserveVoxelPositionRequest\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, long, int, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3I voxelPosition,
        in long reservationTimeMs,
        in int senderSerialId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveVoxelPositionRequest(entityId, voxelPosition, reservationTimeMs, senderSerialId);
      }
    }

    protected sealed class OnReserveVoxelPositionSuccess\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3I voxelPosition,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveVoxelPositionSuccess(entityId, voxelPosition, senderSerialId);
      }
    }

    protected sealed class OnReserveVoxelPositionFailure\u003C\u003ESystem_Int64\u0023VRageMath_Vector3I\u0023System_Int32 : ICallSite<IMyEventOwner, long, Vector3I, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3I voxelPosition,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveVoxelPositionFailure(entityId, voxelPosition, senderSerialId);
      }
    }

    protected sealed class OnReserveAreaRequest\u003C\u003ESystem_String\u0023VRageMath_Vector3D\u0023System_Single\u0023System_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, string, Vector3D, float, long, int, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string reservationName,
        in Vector3D position,
        in float radius,
        in long reservationTimeMs,
        in int senderSerialId,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveAreaRequest(reservationName, position, radius, reservationTimeMs, senderSerialId);
      }
    }

    protected sealed class OnReserveAreaSuccess\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in float radius,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveAreaSuccess(position, radius, senderSerialId);
      }
    }

    protected sealed class OnReserveAreaFailure\u003C\u003EVRageMath_Vector3D\u0023System_Single\u0023System_Int32 : ICallSite<IMyEventOwner, Vector3D, float, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in float radius,
        in int senderSerialId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveAreaFailure(position, radius, senderSerialId);
      }
    }

    protected sealed class OnReserveAreaAllSuccess\u003C\u003ESystem_Int64\u0023System_String\u0023VRageMath_Vector3D\u0023System_Single : ICallSite<IMyEventOwner, long, string, Vector3D, float, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long id,
        in string reservationName,
        in Vector3D position,
        in float radius,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveAreaAllSuccess(id, reservationName, position, radius);
      }
    }

    protected sealed class OnReserveAreaCancel\u003C\u003ESystem_String\u0023System_Int64 : ICallSite<IMyEventOwner, string, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string reservationName,
        in long id,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAiTargetManager.OnReserveAreaCancel(reservationName, id);
      }
    }
  }
}
