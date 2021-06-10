// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyWeaponPropertiesWrapper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;

namespace Sandbox.Game.Weapons
{
  public class MyWeaponPropertiesWrapper
  {
    private MyWeaponDefinition m_weaponDefinition;
    private MyAmmoDefinition m_ammoDefinition;
    private MyAmmoMagazineDefinition m_ammoMagazineDefinition;

    public MyDefinitionId WeaponDefinitionId { get; private set; }

    public MyDefinitionId AmmoMagazineId { get; private set; }

    public MyDefinitionId AmmoDefinitionId { get; private set; }

    public MyWeaponPropertiesWrapper(MyDefinitionId weaponDefinitionId)
    {
      this.WeaponDefinitionId = weaponDefinitionId;
      MyDefinitionManager.Static.TryGetWeaponDefinition(this.WeaponDefinitionId, out this.m_weaponDefinition);
    }

    public bool CanChangeAmmoMagazine(MyDefinitionId newAmmoMagazineId) => this.WeaponDefinition.IsAmmoMagazineCompatible(newAmmoMagazineId);

    public void ChangeAmmoMagazine(MyDefinitionId newAmmoMagazineId)
    {
      this.AmmoMagazineId = newAmmoMagazineId;
      this.m_ammoMagazineDefinition = MyDefinitionManager.Static.GetAmmoMagazineDefinition(this.AmmoMagazineId);
      this.AmmoDefinitionId = this.AmmoMagazineDefinition.AmmoDefinitionId;
      this.m_ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(this.AmmoDefinitionId);
    }

    public T GetCurrentAmmoDefinitionAs<T>() where T : MyAmmoDefinition => this.AmmoDefinition as T;

    public MyAmmoDefinition AmmoDefinition => this.m_ammoDefinition;

    public MyWeaponDefinition WeaponDefinition => this.m_weaponDefinition;

    public MyAmmoMagazineDefinition AmmoMagazineDefinition => this.m_ammoMagazineDefinition;

    public int AmmoMagazinesCount => this.WeaponDefinition.AmmoMagazinesId.Length;

    public bool IsAmmoProjectile => this.AmmoDefinition.AmmoType == MyAmmoType.HighSpeed;

    public bool IsAmmoMissile => this.AmmoDefinition.AmmoType == MyAmmoType.Missile;

    public bool IsDeviated => (double) this.WeaponDefinition.DeviateShotAngle != 0.0;

    public bool IsDeviatedWhileAiming => (double) this.WeaponDefinition.DeviateShotAngleAiming != 0.0;

    public int CurrentWeaponRateOfFire => this.m_weaponDefinition.WeaponAmmoDatas[(int) this.AmmoDefinition.AmmoType].RateOfFire;

    public int ShotsInBurst => this.m_weaponDefinition.WeaponAmmoDatas[(int) this.AmmoDefinition.AmmoType].ShotsInBurst;

    public int ReloadTime => this.m_weaponDefinition.ReloadTime;

    public int CurrentWeaponShootIntervalInMiliseconds => this.m_weaponDefinition.WeaponAmmoDatas[(int) this.AmmoDefinition.AmmoType].ShootIntervalInMiliseconds;

    public MySoundPair CurrentWeaponShootSound => this.m_weaponDefinition.WeaponAmmoDatas[(int) this.AmmoDefinition.AmmoType].ShootSound;

    public float RecoilResetTimeMilliseconds => (double) this.m_weaponDefinition.RecoilResetTimeMilliseconds == 0.0 ? (float) (1000.0 / ((double) this.CurrentWeaponRateOfFire / 60.0)) : this.m_weaponDefinition.RecoilResetTimeMilliseconds;
  }
}
