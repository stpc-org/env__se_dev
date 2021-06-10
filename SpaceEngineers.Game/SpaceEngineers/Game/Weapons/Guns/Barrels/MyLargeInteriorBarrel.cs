// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.Barrels.MyLargeInteriorBarrel
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
  internal class MyLargeInteriorBarrel : MyLargeBarrelBase
  {
    public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
    {
      base.Init(entity, turretBase);
      if (this.m_gunBase.HasDummies)
        return;
      this.m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation((Vector3) (-this.Entity.PositionComp.WorldMatrixRef.Forward * 0.800000011920929)));
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (Sync.IsDedicated)
        return;
      if (this.m_shotSmoke != null)
      {
        this.m_shotSmoke.WorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
        if (this.m_smokeToGenerate > 0)
        {
          this.m_shotSmoke.UserBirthMultiplier = (float) this.m_smokeToGenerate;
        }
        else
        {
          this.m_shotSmoke.Stop(false);
          this.m_shotSmoke = (MyParticleEffect) null;
        }
      }
      if (this.m_muzzleFlash == null)
        return;
      if (this.m_smokeToGenerate == 0)
      {
        this.m_muzzleFlash.Stop();
        this.m_muzzleFlash = (MyParticleEffect) null;
      }
      else
        this.m_muzzleFlash.WorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
    }

    public override void Draw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      MyRenderProxy.DebugDrawLine3D(this.m_entity.PositionComp.GetPosition(), this.m_entity.PositionComp.GetPosition() + this.m_entity.WorldMatrix.Forward, Color.Green, Color.GreenYellow, false);
      if (this.GetWeaponBase().Target == null)
        return;
      MyRenderProxy.DebugDrawSphere(this.GetWeaponBase().Target.PositionComp.GetPosition(), 0.4f, Color.Green, depthRead: false);
    }

    public override bool StartShooting()
    {
      if (this.m_lateStartRandom > this.m_currentLateStart && !this.m_dontTimeOffsetNextShot)
      {
        ++this.m_currentLateStart;
        return false;
      }
      this.m_dontTimeOffsetNextShot = false;
      if (!base.StartShooting() || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.m_gunBase.ShootIntervalInMiliseconds || this.m_turretBase != null && !this.m_turretBase.IsTargetVisible())
        return false;
      if (!Sync.IsDedicated)
      {
        this.m_muzzleFlashLength = MyUtils.GetRandomFloat(1f, 2f);
        this.m_muzzleFlashRadius = MyUtils.GetRandomFloat(0.3f, 0.5f);
        if (this.m_turretBase.IsControlledByLocalPlayer)
        {
          this.m_muzzleFlashLength *= 0.33f;
          this.m_muzzleFlashRadius *= 0.33f;
        }
        this.IncreaseSmoke();
        if (this.m_shotSmoke == null)
        {
          if (this.m_smokeToGenerate > 0)
            MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", this.m_gunBase.GetMuzzleWorldMatrix(), out this.m_shotSmoke);
        }
        else if (this.m_shotSmoke.IsEmittingStopped)
          this.m_shotSmoke.Play();
        if (this.m_muzzleFlash == null)
          MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", this.m_gunBase.GetMuzzleWorldMatrix(), out this.m_muzzleFlash);
        if (this.m_shotSmoke != null)
          this.m_shotSmoke.WorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
        if (this.m_muzzleFlash != null)
          this.m_muzzleFlash.WorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
        this.GetWeaponBase().PlayShootingSound();
      }
      this.Shoot((Vector3) this.Entity.PositionComp.GetPosition());
      this.m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      return true;
    }

    public override void StopShooting()
    {
      base.StopShooting();
      this.m_currentLateStart = 0;
      if (this.m_muzzleFlash == null)
        return;
      this.m_muzzleFlash.Stop();
      this.m_muzzleFlash = (MyParticleEffect) null;
    }

    public override void Close()
    {
      if (this.m_shotSmoke != null)
      {
        this.m_shotSmoke.Stop();
        this.m_shotSmoke = (MyParticleEffect) null;
      }
      if (this.m_muzzleFlash == null)
        return;
      this.m_muzzleFlash.Stop();
      this.m_muzzleFlash = (MyParticleEffect) null;
    }
  }
}
