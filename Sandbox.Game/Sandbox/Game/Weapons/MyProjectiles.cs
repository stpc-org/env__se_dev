// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyProjectiles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  internal class MyProjectiles : MySessionComponentBase
  {
    private int m_projectileCount;
    private MyProjectile[] m_projectiles;
    private static MyProjectiles m_static;
    private object m_projectileCreationLock = new object();

    public static MyProjectiles Static => MyProjectiles.m_static;

    public override void LoadData() => MyProjectiles.m_static = this;

    protected override void UnloadData() => MyProjectiles.m_static = (MyProjectiles) null;

    private ref MyProjectile AllocatedProjectile()
    {
      ++this.m_projectileCount;
      ArrayExtensions.EnsureCapacity<MyProjectile>(ref this.m_projectiles, this.m_projectileCount, 2f);
      return ref this.m_projectiles[this.m_projectileCount - 1];
    }

    private bool CanSpawnProjectile(MyEntity owner, out ulong owningPlayerId)
    {
      ref ulong local = ref owningPlayerId;
      MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(owner);
      long num = controllingPlayer != null ? (long) controllingPlayer.Id.SteamId : 0L;
      local = (ulong) num;
      return MySessionComponentSafeZones.CanPerformAction(MySafeZoneAction.Shooting, owningPlayerId);
    }

    public void Add(
      MyWeaponPropertiesWrapper props,
      Vector3D origin,
      Vector3 initialVelocity,
      Vector3 directionNormalized,
      IMyGunBaseUser user,
      MyEntity owner)
    {
      MyEntity owner1 = user.Owner ?? (user.IgnoreEntities == null || user.IgnoreEntities.Length == 0 ? (MyEntity) null : user.IgnoreEntities[0]);
      bool supressHitIndicator = false;
      ulong owningPlayerId;
      if (!this.CanSpawnProjectile(owner1, out owningPlayerId))
        return;
      if (user is MyLargeTurretBase myLargeTurretBase)
      {
        if (myLargeTurretBase.ControllerInfo == null || myLargeTurretBase.ControllerInfo.Controller == null)
          supressHitIndicator = true;
        else
          owningPlayerId = myLargeTurretBase.ControllerInfo.Controller.Player.Id.SteamId;
      }
      lock (this.m_projectileCreationLock)
      {
        ref MyProjectile local = ref this.AllocatedProjectile();
        local.Start(props.GetCurrentAmmoDefinitionAs<MyProjectileAmmoDefinition>(), props.WeaponDefinition, user.IgnoreEntities, origin, initialVelocity, directionNormalized, user.Weapon, supressHitIndicator);
        local.OwnerEntity = owner1;
        local.OwnerEntityAbsolute = owner;
        local.OwningPlayer = owningPlayerId;
      }
    }

    public void AddShrapnel(
      MyProjectileAmmoDefinition ammoDefinition,
      MyEntity[] ignoreEntities,
      Vector3 origin,
      Vector3 initialVelocity,
      Vector3 directionNormalized,
      bool groupStart,
      float thicknessMultiplier,
      float trailProbability,
      MyEntity weapon,
      MyEntity ownerEntity = null,
      float projectileCountMultiplier = 1f)
    {
      if (ownerEntity == null && !ignoreEntities.IsNullOrEmpty<MyEntity>())
        ownerEntity = ignoreEntities[0];
      ulong owningPlayerId;
      if (!this.CanSpawnProjectile(ownerEntity, out owningPlayerId))
        return;
      ref MyProjectile local = ref this.AllocatedProjectile();
      local.Start(ammoDefinition, (MyWeaponDefinition) null, ignoreEntities, (Vector3D) origin, initialVelocity, directionNormalized, weapon);
      local.OwnerEntity = ownerEntity;
      local.OwningPlayer = owningPlayerId;
    }

    public override unsafe void UpdateBeforeSimulation()
    {
      MyProjectile.CHECK_INTERSECTION_INTERVAL = MySession.Static.HighSimulationQuality ? 5 : 15;
      for (int index1 = 0; index1 < this.m_projectileCount; ++index1)
      {
        ref MyProjectile local1 = ref this.m_projectiles[index1];
        if (!local1.Update())
        {
          local1.Close();
          int index2 = this.m_projectileCount - 1;
          ref MyProjectile local2 = ref this.m_projectiles[index2];
          if (index1 != index2)
            local1 = local2;
          *(MyProjectile*) ref local2 = new MyProjectile();
          --index1;
          --this.m_projectileCount;
        }
      }
    }

    public override void Draw()
    {
      for (int index = 0; index < this.m_projectileCount; ++index)
        this.m_projectiles[index].Draw();
    }
  }
}
