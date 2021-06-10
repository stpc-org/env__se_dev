// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.Barrels.MyLargeGatlingBarrel
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
  internal class MyLargeGatlingBarrel : MyLargeBarrelBase
  {
    private Vector3D m_muzzleFlashPosition;
    private float m_rotationTimeout;
    private int m_shotsLeftInBurst;

    public int ShotsInBurst => this.m_gunBase.ShotsInBurst;

    public MyLargeGatlingBarrel() => this.m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);

    public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
    {
      base.Init(entity, turretBase);
      this.m_shotsLeftInBurst = this.ShotsInBurst;
      if (this.m_gunBase.HasDummies)
        return;
      this.m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation((Vector3) (2.0 * entity.PositionComp.WorldMatrixRef.Forward)));
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (Sync.IsDedicated)
        return;
      float radians = (float) ((double) MathHelper.SmoothStep(0.0f, 1f, 1f - MathHelper.Clamp((float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot) / this.m_rotationTimeout, 0.0f, 1f)) * 12.5663709640503 * 0.0166666675359011);
      if ((double) radians != 0.0)
      {
        Matrix localMatrix = Matrix.CreateRotationZ(radians) * this.Entity.PositionComp.LocalMatrixRef;
        this.Entity.PositionComp.SetLocalMatrix(ref localMatrix);
      }
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
      if (this.m_muzzleFlash != null)
      {
        if (this.m_smokeToGenerate == 0)
        {
          this.m_muzzleFlash.Stop();
          this.m_muzzleFlash = (MyParticleEffect) null;
        }
        else
          this.m_muzzleFlash.WorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
      }
      this.UpdateReloadNotification();
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
      if (this.m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds || !base.StartShooting() || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.m_gunBase.ShootIntervalInMiliseconds || this.m_turretBase != null && !this.m_turretBase.IsTargetVisible())
        return false;
      if (this.m_lateStartRandom > this.m_currentLateStart && !this.m_dontTimeOffsetNextShot)
      {
        ++this.m_currentLateStart;
        return false;
      }
      this.m_dontTimeOffsetNextShot = false;
      if (this.m_shotsLeftInBurst <= 0 && this.ShotsInBurst != 0)
        return false;
      this.m_muzzleFlashLength = MyUtils.GetRandomFloat(4f, 6f);
      this.m_muzzleFlashRadius = MyUtils.GetRandomFloat(1.2f, 2f);
      if (this.m_turretBase.IsControlledByLocalPlayer)
        this.m_muzzleFlashRadius *= 0.33f;
      if (!Sync.IsDedicated)
      {
        this.IncreaseSmoke();
        this.m_muzzleFlashPosition = this.m_gunBase.GetMuzzleWorldPosition();
        if (this.m_shotSmoke == null)
        {
          if (this.m_smokeToGenerate > 0)
            MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", MatrixD.CreateTranslation(this.m_muzzleFlashPosition), out this.m_shotSmoke);
        }
        else if (this.m_shotSmoke.IsEmittingStopped)
          this.m_shotSmoke.Play();
        if (this.m_muzzleFlash == null)
          MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", MatrixD.CreateTranslation(this.m_muzzleFlashPosition), out this.m_muzzleFlash);
        this.m_shotSmoke?.SetTranslation(ref this.m_muzzleFlashPosition);
        this.m_muzzleFlash?.SetTranslation(ref this.m_muzzleFlashPosition);
        this.GetWeaponBase().PlayShootingSound();
      }
      this.Shoot((Vector3) this.Entity.PositionComp.GetPosition());
      if (this.ShotsInBurst > 0)
      {
        --this.m_shotsLeftInBurst;
        if (this.m_shotsLeftInBurst <= 0)
        {
          this.m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_gunBase.ReloadTime;
          this.m_turretBase.OnReloadStarted(this.m_gunBase.ReloadTime);
          this.m_shotsLeftInBurst = this.ShotsInBurst;
          if (this.m_muzzleFlash != null)
          {
            this.m_muzzleFlash.Stop();
            this.m_muzzleFlash = (MyParticleEffect) null;
          }
          this.m_currentLateStart = 0;
        }
      }
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
