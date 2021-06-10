// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MySyncEnvironmentItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.EnvironmentItems;
using System;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public static class MySyncEnvironmentItems
  {
    public static Action<MyEntity, int> OnRemoveEnvironmentItem;

    public static void RemoveEnvironmentItem(long entityId, int itemInstanceId) => MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MySyncEnvironmentItems.OnRemoveEnvironmentItemMessage)), entityId, itemInstanceId);

    [Event(null, 29)]
    [Reliable]
    [Server]
    [BroadcastExcept]
    private static void OnRemoveEnvironmentItemMessage(long entityId, int itemInstanceId)
    {
      MyEntity entity;
      if (MyEntities.TryGetEntityById(entityId, out entity))
      {
        if (MySyncEnvironmentItems.OnRemoveEnvironmentItem == null)
          return;
        MySyncEnvironmentItems.OnRemoveEnvironmentItem(entity, itemInstanceId);
      }
      else
      {
        int num = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG ? 1 : 0;
      }
    }

    public static void SendModifyModelMessage(
      long entityId,
      int instanceId,
      MyStringHash subtypeId)
    {
      MyMultiplayer.RaiseStaticEvent<long, int, MyStringHash>((Func<IMyEventOwner, Action<long, int, MyStringHash>>) (s => new Action<long, int, MyStringHash>(MySyncEnvironmentItems.OnModifyModelMessage)), entityId, instanceId, subtypeId);
    }

    [Event(null, 51)]
    [Reliable]
    [Broadcast]
    private static void OnModifyModelMessage(long entityId, int instanceId, MyStringHash subtypeId)
    {
      MyEnvironmentItems entity;
      if (MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
      {
        entity.ModifyItemModel(instanceId, subtypeId, true, false);
      }
      else
      {
        int num = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG ? 1 : 0;
      }
    }

    public static void SendBeginBatchAddMessage(long entityId) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySyncEnvironmentItems.OnBeginBatchAddMessage)), entityId);

    [Event(null, 72)]
    [Reliable]
    [Broadcast]
    private static void OnBeginBatchAddMessage(long entityId)
    {
      MyEnvironmentItems entity;
      if (!MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
        return;
      entity.BeginBatch(false);
    }

    public static void SendBatchAddItemMessage(
      long entityId,
      Vector3D position,
      MyStringHash subtypeId)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3D, MyStringHash>((Func<IMyEventOwner, Action<long, Vector3D, MyStringHash>>) (s => new Action<long, Vector3D, MyStringHash>(MySyncEnvironmentItems.OnBatchAddItemMessage)), entityId, position, subtypeId);
    }

    [Event(null, 88)]
    [Reliable]
    [Broadcast]
    private static void OnBatchAddItemMessage(
      long entityId,
      Vector3D position,
      MyStringHash subtypeId)
    {
      MyEnvironmentItems entity;
      if (!MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
        return;
      entity.BatchAddItem(position, subtypeId, false);
    }

    public static void SendBatchModifyItemMessage(
      long entityId,
      int localId,
      MyStringHash subtypeId)
    {
      MyMultiplayer.RaiseStaticEvent<long, int, MyStringHash>((Func<IMyEventOwner, Action<long, int, MyStringHash>>) (s => new Action<long, int, MyStringHash>(MySyncEnvironmentItems.OnBatchModifyItemMessage)), entityId, localId, subtypeId);
    }

    [Event(null, 104)]
    [Reliable]
    [Broadcast]
    private static void OnBatchModifyItemMessage(
      long entityId,
      int localId,
      MyStringHash subtypeId)
    {
      MyEnvironmentItems entity;
      if (!MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
        return;
      entity.BatchModifyItem(localId, subtypeId, false);
    }

    public static void SendBatchRemoveItemMessage(long entityId, int localId) => MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MySyncEnvironmentItems.OnBatchRemoveItemMessage)), entityId, localId);

    [Event(null, 120)]
    [Reliable]
    [Broadcast]
    private static void OnBatchRemoveItemMessage(long entityId, int localId)
    {
      MyEnvironmentItems entity;
      if (!MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
        return;
      entity.BatchRemoveItem(localId, false);
    }

    public static void SendEndBatchAddMessage(long entityId) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MySyncEnvironmentItems.OnEndBatchAddMessage)), entityId);

    [Event(null, 136)]
    [Reliable]
    [Broadcast]
    private static void OnEndBatchAddMessage(long entityId)
    {
      MyEnvironmentItems entity;
      if (!MyEntities.TryGetEntityById<MyEnvironmentItems>(entityId, out entity))
        return;
      entity.EndBatch(false);
    }

    protected sealed class OnRemoveEnvironmentItemMessage\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int itemInstanceId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnRemoveEnvironmentItemMessage(entityId, itemInstanceId);
      }
    }

    protected sealed class OnModifyModelMessage\u003C\u003ESystem_Int64\u0023System_Int32\u0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, int, MyStringHash, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int instanceId,
        in MyStringHash subtypeId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnModifyModelMessage(entityId, instanceId, subtypeId);
      }
    }

    protected sealed class OnBeginBatchAddMessage\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnBeginBatchAddMessage(entityId);
      }
    }

    protected sealed class OnBatchAddItemMessage\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D\u0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, Vector3D, MyStringHash, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D position,
        in MyStringHash subtypeId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnBatchAddItemMessage(entityId, position, subtypeId);
      }
    }

    protected sealed class OnBatchModifyItemMessage\u003C\u003ESystem_Int64\u0023System_Int32\u0023VRage_Utils_MyStringHash : ICallSite<IMyEventOwner, long, int, MyStringHash, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int localId,
        in MyStringHash subtypeId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnBatchModifyItemMessage(entityId, localId, subtypeId);
      }
    }

    protected sealed class OnBatchRemoveItemMessage\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in int localId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnBatchRemoveItemMessage(entityId, localId);
      }
    }

    protected sealed class OnEndBatchAddMessage\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncEnvironmentItems.OnEndBatchAddMessage(entityId);
      }
    }
  }
}
