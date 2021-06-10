// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.Guns.Barrels.MyLargeBarrelBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons.Guns.Barrels
{
  public abstract class MyLargeBarrelBase
  {
    protected MyGunBase m_gunBase;
    protected Matrix m_renderLocal;
    protected int m_lastTimeShoot;
    private int m_lastTimeSmooke;
    protected int m_lateStartRandom;
    protected int m_currentLateStart;
    private float m_barrelElevationMin;
    private float m_barrelSinElevationMin;
    protected MyParticleEffect m_shotSmoke;
    protected MyParticleEffect m_muzzleFlash;
    protected bool m_dontTimeOffsetNextShot;
    protected int m_smokeLastTime;
    protected int m_smokeToGenerate;
    protected float m_muzzleFlashLength;
    protected float m_muzzleFlashRadius;
    private bool m_isPreview;
    private bool m_isVisibleOutsidePreview;
    protected MyEntity m_entity;
    protected MyLargeTurretBase m_turretBase;
    protected int m_nextNotificationTime;
    protected MyHudNotification m_reloadNotification;
    protected int m_reloadCompletionTime;

    public MyGunBase GunBase => this.m_gunBase;

    public MyModelDummy CameraDummy { get; private set; }

    public int LateTimeRandom
    {
      set => this.m_lateStartRandom = value;
      get => this.m_lateStartRandom;
    }

    public void ResetCurrentLateStart() => this.m_currentLateStart = 0;

    public float BarrelElevationMin
    {
      get => this.m_barrelElevationMin;
      protected set
      {
        this.m_barrelElevationMin = value;
        this.m_barrelSinElevationMin = (float) Math.Sin((double) this.m_barrelSinElevationMin);
      }
    }

    public float BarrelSinElevationMin => this.m_barrelSinElevationMin;

    public void DontTimeOffsetNextShot() => this.m_dontTimeOffsetNextShot = true;

    public MyLargeBarrelBase()
    {
      this.m_lastTimeShoot = 0;
      this.m_lastTimeSmooke = 0;
      this.BarrelElevationMin = -0.6f;
    }

    public virtual void Draw()
    {
    }

    public virtual void Init(MyEntity entity, MyLargeTurretBase turretBase)
    {
      this.m_entity = entity;
      this.m_turretBase = turretBase;
      this.m_gunBase = turretBase.GunBase;
      this.m_lateStartRandom = turretBase.LateStartRandom;
      if (this.m_entity.Model != null)
      {
        if (this.m_entity.Model.Dummies.ContainsKey("camera"))
          this.CameraDummy = this.m_entity.Model.Dummies["camera"];
        this.m_gunBase.LoadDummies(this.m_entity.Model.Dummies);
      }
      this.m_entity.OnClose += new Action<MyEntity>(this.m_entity_OnClose);
    }

    protected void UpdateReloadNotification()
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds > this.m_nextNotificationTime)
        this.m_reloadNotification = (MyHudNotification) null;
      if (!this.m_gunBase.HasEnoughAmmunition() && MySession.Static.SurvivalMode)
      {
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_reloadNotification);
        this.m_reloadNotification = (MyHudNotification) null;
      }
      else if (!this.m_turretBase.IsControlledByLocalPlayer)
      {
        if (this.m_reloadNotification == null)
          return;
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_reloadNotification);
        this.m_reloadNotification = (MyHudNotification) null;
      }
      else
      {
        if (this.m_reloadCompletionTime <= MySandboxGame.TotalGamePlayTimeInMilliseconds)
          return;
        this.ShowReloadNotification(this.m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
      }
    }

    protected void ShowReloadNotification(int duration)
    {
      int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
      if (this.m_reloadNotification == null)
      {
        duration = Math.Max(0, duration - 250);
        if (duration == 0)
          return;
        this.m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, level: MyNotificationLevel.Important);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_reloadNotification);
        this.m_nextNotificationTime = num;
      }
      else
      {
        this.m_reloadNotification.AddAliveTime(num - this.m_nextNotificationTime);
        this.m_nextNotificationTime = num;
      }
    }

    private void m_entity_OnClose(MyEntity obj)
    {
      if (this.m_shotSmoke != null)
      {
        MyParticlesManager.RemoveParticleEffect(this.m_shotSmoke);
        this.m_shotSmoke = (MyParticleEffect) null;
      }
      if (this.m_muzzleFlash == null)
        return;
      MyParticlesManager.RemoveParticleEffect(this.m_muzzleFlash);
      this.m_muzzleFlash = (MyParticleEffect) null;
    }

    public virtual bool StartShooting()
    {
      this.m_turretBase.Render.NeedsDrawFromParent = true;
      return true;
    }

    public virtual void StopShooting()
    {
      this.m_turretBase.Render.NeedsDrawFromParent = false;
      this.GetWeaponBase().StopShootingSound();
    }

    protected MyLargeTurretBase GetWeaponBase() => this.m_turretBase;

    protected void Shoot(Vector3 muzzlePosition)
    {
      if (this.m_turretBase.Parent.Physics == null)
        return;
      Vector3 forward = (Vector3) this.m_entity.WorldMatrix.Forward;
      Vector3 linearVelocity = this.m_turretBase.Parent.Physics.LinearVelocity;
      this.GetWeaponBase().RemoveAmmoPerShot();
      this.m_gunBase.Shoot(linearVelocity);
    }

    private void DrawCrossHair()
    {
    }

    public bool IsControlledByPlayer() => MySession.Static.ControlledEntity == this;

    protected void IncreaseSmoke()
    {
      this.m_smokeToGenerate += 19;
      this.m_smokeToGenerate = MyUtils.GetClampInt(this.m_smokeToGenerate, 0, 50);
    }

    protected void DecreaseSmoke()
    {
      --this.m_smokeToGenerate;
      this.m_smokeToGenerate = MyUtils.GetClampInt(this.m_smokeToGenerate, 0, 50);
    }

    public bool NeedsPerFrameUpdate => this.m_smokeToGenerate > 0;

    public virtual void UpdateAfterSimulation() => this.DecreaseSmoke();

    public void IsPreviewChanged(bool isPreview)
    {
      if (this.m_isPreview == isPreview)
        return;
      this.m_isPreview = isPreview;
      if (isPreview)
      {
        this.m_isVisibleOutsidePreview = this.Entity.Render.Visible;
        this.Entity.Render.Visible = false;
      }
      else
        this.Entity.Render.Visible = this.m_isVisibleOutsidePreview;
    }

    public void RemoveSmoke() => this.m_smokeToGenerate = 0;

    public MyEntity Entity => this.m_entity;

    public virtual void Close()
    {
    }

    public void WorldPositionChanged(ref Matrix renderLocal)
    {
      this.m_gunBase.WorldMatrix = this.Entity.PositionComp.WorldMatrixRef;
      this.m_renderLocal = renderLocal;
    }

    public void ShootEffect() => this.m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
  }
}
