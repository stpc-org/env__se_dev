// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.Barrels.MyLargeMissileBarrel
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
  internal class MyLargeMissileBarrel : MyLargeBarrelBase
  {
    private int m_nextShootTime;
    private int m_shotsLeftInBurst;
    private MyEntity3DSoundEmitter m_soundEmitter;

    public int ShotsInBurst => this.m_gunBase.ShotsInBurst;

    public MyLargeMissileBarrel() => this.m_soundEmitter = new MyEntity3DSoundEmitter(this.m_entity);

    public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
    {
      base.Init(entity, turretBase);
      if (!this.m_gunBase.HasDummies)
      {
        Matrix identity = Matrix.Identity;
        ref Matrix local = ref identity;
        local.Translation = (Vector3) (local.Translation + entity.PositionComp.WorldMatrixRef.Forward * 3.0);
        this.m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, identity);
      }
      this.m_shotsLeftInBurst = this.ShotsInBurst;
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Entity = (MyEntity) turretBase;
    }

    public void Init(Matrix localMatrix, MyLargeTurretBase parentObject) => this.m_shotsLeftInBurst = this.ShotsInBurst;

    public override bool StartShooting()
    {
      if (this.m_turretBase == null || this.m_turretBase.Parent == null || this.m_turretBase.Parent.Physics == null)
        return false;
      bool timeOffsetNextShot = this.m_dontTimeOffsetNextShot;
      if (Sync.IsServer)
      {
        if (this.m_lateStartRandom > this.m_currentLateStart && !this.m_dontTimeOffsetNextShot && !this.m_turretBase.IsControlled)
        {
          ++this.m_currentLateStart;
          return false;
        }
        this.m_dontTimeOffsetNextShot = false;
      }
      if (this.m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds || this.m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
        return false;
      if ((this.m_shotsLeftInBurst > 0 || this.ShotsInBurst == 0) && ((this.m_turretBase.Target != null ? 1 : (this.m_turretBase.IsControlled ? 1 : 0)) | (timeOffsetNextShot ? 1 : 0)) != 0)
      {
        if (Sync.IsServer)
          this.GetWeaponBase().RemoveAmmoPerShot();
        this.m_gunBase.Shoot(this.m_turretBase.Parent.Physics.LinearVelocity);
        this.m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_gunBase.ShootIntervalInMiliseconds;
        if (this.ShotsInBurst > 0)
        {
          --this.m_shotsLeftInBurst;
          if (this.m_shotsLeftInBurst <= 0)
          {
            this.m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_gunBase.ReloadTime;
            this.m_turretBase.OnReloadStarted(this.m_gunBase.ReloadTime);
            this.m_shotsLeftInBurst = this.ShotsInBurst;
          }
        }
      }
      return true;
    }

    public override void StopShooting()
    {
      base.StopShooting();
      this.m_currentLateStart = 0;
      this.m_soundEmitter.StopSound(true);
    }

    private void StartSound() => this.m_gunBase.StartShootSound(this.m_soundEmitter);

    public override void Close()
    {
      base.Close();
      this.m_soundEmitter.StopSound(true);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (Sync.IsDedicated)
        return;
      this.UpdateReloadNotification();
    }
  }
}
