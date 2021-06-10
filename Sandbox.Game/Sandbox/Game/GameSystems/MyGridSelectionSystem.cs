// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridSelectionSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyGridSelectionSystem
  {
    private MyConcurrentHashSet<IMyGunObject<MyDeviceBase>> m_currentGuns = new MyConcurrentHashSet<IMyGunObject<MyDeviceBase>>();
    private MyDefinitionId? m_gunId;
    private bool m_useSingleGun;
    private MyShipController m_shipController;
    private MyGridWeaponSystem m_weaponSystem;
    private int m_gunTimer;
    private int m_gunTimer_Max;
    private static int PerFrameMax = 10;
    private int m_curentDrawHudIndex;

    public MyGridWeaponSystem WeaponSystem
    {
      get => this.m_weaponSystem;
      set
      {
        if (this.m_weaponSystem == value)
          return;
        if (this.m_weaponSystem != null)
        {
          this.m_weaponSystem.WeaponRegistered -= new Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs>(this.WeaponSystem_WeaponRegistered);
          this.m_weaponSystem.WeaponUnregistered -= new Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs>(this.WeaponSystem_WeaponUnregistered);
        }
        this.m_weaponSystem = value;
        if (this.m_weaponSystem == null)
          return;
        this.m_weaponSystem.WeaponRegistered += new Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs>(this.WeaponSystem_WeaponRegistered);
        this.m_weaponSystem.WeaponUnregistered += new Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs>(this.WeaponSystem_WeaponUnregistered);
      }
    }

    public MyGridSelectionSystem(MyShipController shipController) => this.m_shipController = shipController;

    private void WeaponSystem_WeaponRegistered(
      MyGridWeaponSystem sender,
      MyGridWeaponSystem.EventArgs args)
    {
      if (this.m_shipController.Pilot == null)
        return;
      MyDefinitionId definitionId = args.Weapon.DefinitionId;
      MyDefinitionId? gunId = this.m_gunId;
      if ((gunId.HasValue ? (definitionId == gunId.GetValueOrDefault() ? 1 : 0) : 0) == 0)
        return;
      if (this.m_useSingleGun)
      {
        if (this.m_currentGuns.Count >= 1)
          return;
        args.Weapon.OnControlAcquired(this.m_shipController.Pilot);
        this.m_currentGuns.Add(args.Weapon);
      }
      else
      {
        args.Weapon.OnControlAcquired(this.m_shipController.Pilot);
        this.m_currentGuns.Add(args.Weapon);
      }
    }

    private void WeaponSystem_WeaponUnregistered(
      MyGridWeaponSystem sender,
      MyGridWeaponSystem.EventArgs args)
    {
      if (this.m_shipController.Pilot == null)
        return;
      MyDefinitionId definitionId = args.Weapon.DefinitionId;
      MyDefinitionId? gunId = this.m_gunId;
      if ((gunId.HasValue ? (definitionId == gunId.GetValueOrDefault() ? 1 : 0) : 0) == 0 || !this.m_currentGuns.Contains(args.Weapon))
        return;
      args.Weapon.OnControlReleased();
      this.m_currentGuns.Remove(args.Weapon);
    }

    internal bool CanShoot(
      MyShootActionEnum action,
      out MyGunStatusEnum status,
      out IMyGunObject<MyDeviceBase> FailedGun)
    {
      FailedGun = (IMyGunObject<MyDeviceBase>) null;
      if (this.m_currentGuns == null)
      {
        status = MyGunStatusEnum.NotSelected;
        return false;
      }
      bool flag = false;
      status = MyGunStatusEnum.OK;
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
      {
        MyGunStatusEnum status1;
        flag |= currentGun.CanShoot(action, this.m_shipController.ControllerInfo.Controller != null ? this.m_shipController.ControllerInfo.Controller.Player.Identity.IdentityId : this.m_shipController.OwnerId, out status1);
        if (status1 != MyGunStatusEnum.OK)
        {
          FailedGun = currentGun;
          status = status1;
        }
      }
      return flag;
    }

    internal void Shoot(MyShootActionEnum action)
    {
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
      {
        if (!currentGun.EnabledInWorldRules)
        {
          if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
            MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
        }
        else if (currentGun.CanShoot(action, this.m_shipController.ControllerInfo.Controller != null ? this.m_shipController.ControllerInfo.ControllingIdentityId : this.m_shipController.OwnerId, out MyGunStatusEnum _))
          currentGun.Shoot(action, (Vector3) ((MyEntity) currentGun).WorldMatrix.Forward, new Vector3D?());
      }
    }

    internal void BeginShoot(MyShootActionEnum action)
    {
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
      {
        if (!currentGun.EnabledInWorldRules)
        {
          if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
            MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
        }
        else
          currentGun.BeginShoot(action);
      }
    }

    internal void EndShoot(MyShootActionEnum action)
    {
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
      {
        if (!currentGun.EnabledInWorldRules)
        {
          if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
            MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
        }
        else
          currentGun.EndShoot(action);
      }
    }

    public bool CanSwitchAmmoMagazine()
    {
      bool flag = true;
      if (this.m_currentGuns != null)
      {
        foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
        {
          if (currentGun.GunBase == null)
            return false;
          flag &= currentGun.GunBase.CanSwitchAmmoMagazine();
        }
      }
      return flag;
    }

    internal void SwitchAmmoMagazine()
    {
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
      {
        if (!currentGun.EnabledInWorldRules)
        {
          if (MyEventContext.Current.IsLocallyInvoked || MyMultiplayer.Static == null)
            MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
        }
        else
          currentGun.GunBase.SwitchToNextAmmoMagazine();
      }
    }

    internal void SwitchTo(MyDefinitionId? gunId, bool useSingle = false)
    {
      this.m_gunId = gunId;
      this.m_useSingleGun = useSingle;
      if (this.m_currentGuns != null)
      {
        foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
          currentGun.OnControlReleased();
      }
      if (!gunId.HasValue)
      {
        this.m_currentGuns.Clear();
      }
      else
      {
        this.m_currentGuns.Clear();
        if (useSingle)
        {
          IMyGunObject<MyDeviceBase> gunWithAmmo = this.WeaponSystem.GetGunWithAmmo(gunId.Value, this.m_shipController.OwnerId);
          if (gunWithAmmo != null)
            this.m_currentGuns.Add(gunWithAmmo);
        }
        else
        {
          HashSet<IMyGunObject<MyDeviceBase>> gunsById = this.WeaponSystem.GetGunsById(gunId.Value);
          if (gunsById != null)
          {
            foreach (IMyGunObject<MyDeviceBase> instance in gunsById)
            {
              if (instance != null)
                this.m_currentGuns.Add(instance);
            }
          }
        }
        foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
          currentGun.OnControlAcquired(this.m_shipController.Pilot);
      }
    }

    public MyDefinitionId? GetGunId() => this.m_gunId;

    internal void DrawHud(IMyCameraController camera, long playerId)
    {
      if (this.m_currentGuns == null)
        return;
      if (this.m_gunTimer <= 0)
      {
        this.m_gunTimer_Max = this.m_currentGuns.Count / MyGridSelectionSystem.PerFrameMax + 1;
        this.m_gunTimer = this.m_gunTimer_Max;
      }
      --this.m_gunTimer;
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
        currentGun.DrawHud(camera, playerId, (currentGun.GetHashCode() + (int) MySandboxGame.Static.SimulationFrameCounter) % this.m_gunTimer_Max == 0);
    }

    internal void OnControlAcquired()
    {
      if (this.m_currentGuns == null)
        return;
      this.SwitchTo(this.m_gunId, this.m_useSingleGun);
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
        currentGun.OnControlAcquired(this.m_shipController.Pilot);
    }

    internal void OnControlReleased()
    {
      if (this.m_currentGuns == null)
        return;
      foreach (IMyGunObject<MyDeviceBase> currentGun in this.m_currentGuns)
        currentGun.OnControlReleased();
    }
  }
}
