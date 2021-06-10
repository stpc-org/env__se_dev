// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MySyncDamage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using System;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public class MySyncDamage
  {
    public static void DoDamageSynced(
      MyEntity entity,
      float damage,
      MyStringHash type,
      long attackerId)
    {
      if (!(entity is IMyDestroyableObject destroyableObject))
        return;
      double num = (double) damage;
      MyStringHash damageSource = type;
      MyHitInfo? hitInfo = new MyHitInfo?();
      long attackerId1 = attackerId;
      destroyableObject.DoDamage((float) num, damageSource, false, hitInfo, attackerId1);
      MyMultiplayer.RaiseStaticEvent<long, float, MyStringHash, long>((Func<IMyEventOwner, Action<long, float, MyStringHash, long>>) (s => new Action<long, float, MyStringHash, long>(MySyncDamage.OnDoDamage)), entity.EntityId, damage, type, attackerId);
    }

    [Event(null, 34)]
    [Reliable]
    [Broadcast]
    private static void OnDoDamage(
      long destroyableId,
      float damage,
      MyStringHash type,
      long attackerId)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(destroyableId, out entity) || !(entity is IMyDestroyableObject destroyableObject))
        return;
      double num = (double) damage;
      MyStringHash damageSource = type;
      MyHitInfo? hitInfo = new MyHitInfo?();
      long attackerId1 = attackerId;
      destroyableObject.DoDamage((float) num, damageSource, false, hitInfo, attackerId1);
    }

    protected sealed class OnDoDamage\u003C\u003ESystem_Int64\u0023System_Single\u0023VRage_Utils_MyStringHash\u0023System_Int64 : ICallSite<IMyEventOwner, long, float, MyStringHash, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long destroyableId,
        in float damage,
        in MyStringHash type,
        in long attackerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySyncDamage.OnDoDamage(destroyableId, damage, type, attackerId);
      }
    }
  }
}
