// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWeaponDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WeaponDefinition), null)]
  public class MyWeaponDefinition : MyDefinitionBase
  {
    private static readonly string ErrorMessageTemplate = "No weapon ammo data specified for {0} ammo (<{1}AmmoData> tag is missing in weapon definition)";
    public MySoundPair NoAmmoSound;
    public MySoundPair ReloadSound;
    public MySoundPair SecondarySound;
    public float DeviateShotAngle;
    public float DeviateShotAngleAiming;
    public float ReleaseTimeAfterFire;
    public int MuzzleFlashLifeSpan;
    public MyDefinitionId[] AmmoMagazinesId;
    public MyWeaponDefinition.MyWeaponAmmoData[] WeaponAmmoDatas;
    public MyWeaponDefinition.MyWeaponEffect[] WeaponEffects;
    public MyStringHash PhysicalMaterial;
    public bool UseDefaultMuzzleFlash;
    public int ReloadTime = 2000;
    public float DamageMultiplier = 1f;
    public float RangeMultiplier = 1f;
    public bool UseRandomizedRange = true;
    public float RecoilJetpackVertical;
    public float RecoilJetpackHorizontal;
    public float RecoilGroundVertical;
    public float RecoilGroundHorizontal;
    public float RecoilResetTimeMilliseconds;
    public int ShootDirectionUpdateTime = 200;
    public float EquipDuration = 0.5f;
    public Dictionary<MyShootActionEnum, bool> ShakeOnAction = new Dictionary<MyShootActionEnum, bool>();
    public Dictionary<string, Tuple<float, float>> RecoilMultiplierData = new Dictionary<string, Tuple<float, float>>();

    public bool HasProjectileAmmoDefined => this.WeaponAmmoDatas[0] != null;

    public bool HasMissileAmmoDefined => this.WeaponAmmoDatas[1] != null;

    public bool HasSpecificAmmoData(MyAmmoDefinition ammoDefinition) => this.WeaponAmmoDatas[(int) ammoDefinition.AmmoType] != null;

    public bool HasAmmoMagazines() => this.AmmoMagazinesId != null && (uint) this.AmmoMagazinesId.Length > 0U;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WeaponDefinition weaponDefinition = builder as MyObjectBuilder_WeaponDefinition;
      this.WeaponAmmoDatas = new MyWeaponDefinition.MyWeaponAmmoData[Enum.GetValues(typeof (MyAmmoType)).Length];
      this.WeaponEffects = new MyWeaponDefinition.MyWeaponEffect[weaponDefinition.Effects == null ? 0 : weaponDefinition.Effects.Length];
      if (weaponDefinition.Effects != null)
      {
        for (int index = 0; index < weaponDefinition.Effects.Length; ++index)
          this.WeaponEffects[index] = new MyWeaponDefinition.MyWeaponEffect(weaponDefinition.Effects[index].Action, weaponDefinition.Effects[index].Dummy, weaponDefinition.Effects[index].Particle, weaponDefinition.Effects[index].Loop, weaponDefinition.Effects[index].InstantStop);
      }
      this.PhysicalMaterial = MyStringHash.GetOrCompute(weaponDefinition.PhysicalMaterial);
      this.UseDefaultMuzzleFlash = weaponDefinition.UseDefaultMuzzleFlash;
      this.NoAmmoSound = new MySoundPair(weaponDefinition.NoAmmoSoundName);
      this.ReloadSound = new MySoundPair(weaponDefinition.ReloadSoundName);
      this.SecondarySound = new MySoundPair(weaponDefinition.SecondarySoundName);
      this.DeviateShotAngle = MathHelper.ToRadians(weaponDefinition.DeviateShotAngle);
      this.DeviateShotAngleAiming = MathHelper.ToRadians(weaponDefinition.DeviateShotAngleAiming);
      this.ReleaseTimeAfterFire = weaponDefinition.ReleaseTimeAfterFire;
      this.MuzzleFlashLifeSpan = weaponDefinition.MuzzleFlashLifeSpan;
      this.ReloadTime = weaponDefinition.ReloadTime;
      this.DamageMultiplier = weaponDefinition.DamageMultiplier;
      this.RangeMultiplier = weaponDefinition.RangeMultiplier;
      this.UseRandomizedRange = weaponDefinition.UseRandomizedRange;
      this.RecoilJetpackVertical = weaponDefinition.RecoilJetpackVertical;
      this.RecoilJetpackHorizontal = weaponDefinition.RecoilJetpackHorizontal;
      this.RecoilGroundVertical = weaponDefinition.RecoilGroundVertical;
      this.RecoilGroundHorizontal = weaponDefinition.RecoilGroundHorizontal;
      this.RecoilResetTimeMilliseconds = weaponDefinition.RecoilResetTimeMilliseconds;
      this.ShootDirectionUpdateTime = weaponDefinition.ShootDirectionUpdateTime;
      this.EquipDuration = weaponDefinition.EquipDuration;
      this.ShakeOnAction.Add(MyShootActionEnum.PrimaryAction, weaponDefinition.ShakeOnActionPrimary);
      this.ShakeOnAction.Add(MyShootActionEnum.SecondaryAction, weaponDefinition.ShakeOnActionSecondary);
      this.ShakeOnAction.Add(MyShootActionEnum.TertiaryAction, weaponDefinition.ShakeOnActionTertiary);
      this.AmmoMagazinesId = new MyDefinitionId[weaponDefinition.AmmoMagazines.Length];
      for (int index = 0; index < this.AmmoMagazinesId.Length; ++index)
      {
        MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine ammoMagazine = weaponDefinition.AmmoMagazines[index];
        this.AmmoMagazinesId[index] = new MyDefinitionId(ammoMagazine.Type, ammoMagazine.Subtype);
        MyAmmoType ammoType = MyDefinitionManager.Static.GetAmmoDefinition(MyDefinitionManager.Static.GetAmmoMagazineDefinition(this.AmmoMagazinesId[index]).AmmoDefinitionId).AmmoType;
        string message = (string) null;
        if (ammoType != MyAmmoType.HighSpeed)
        {
          if (ammoType != MyAmmoType.Missile)
            throw new NotImplementedException();
          if (weaponDefinition.MissileAmmoData != null)
            this.WeaponAmmoDatas[1] = new MyWeaponDefinition.MyWeaponAmmoData(weaponDefinition.MissileAmmoData);
          else
            message = string.Format(MyWeaponDefinition.ErrorMessageTemplate, (object) "missile", (object) "Missile");
        }
        else if (weaponDefinition.ProjectileAmmoData != null)
          this.WeaponAmmoDatas[0] = new MyWeaponDefinition.MyWeaponAmmoData(weaponDefinition.ProjectileAmmoData);
        else
          message = string.Format(MyWeaponDefinition.ErrorMessageTemplate, (object) "projectile", (object) "Projectile");
        if (!string.IsNullOrEmpty(message))
          MyDefinitionErrors.Add(this.Context, message, TErrorSeverity.Critical);
      }
      if (weaponDefinition.RecoilMultiplierDataNames.Count == 0 || weaponDefinition.RecoilMultiplierDataNames.Count != weaponDefinition.RecoilMultiplierDataVerticals.Count || weaponDefinition.RecoilMultiplierDataVerticals.Count != weaponDefinition.RecoilMultiplierDataHorizontals.Count)
        return;
      this.RecoilMultiplierData.Clear();
      for (int index = 0; index < weaponDefinition.RecoilMultiplierDataNames.Count; ++index)
      {
        if (!this.RecoilMultiplierData.ContainsKey(weaponDefinition.RecoilMultiplierDataNames[index]))
          this.RecoilMultiplierData.Add(weaponDefinition.RecoilMultiplierDataNames[index], new Tuple<float, float>(weaponDefinition.RecoilMultiplierDataVerticals[index], weaponDefinition.RecoilMultiplierDataHorizontals[index]));
      }
    }

    public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineDefinitionId)
    {
      for (int index = 0; index < this.AmmoMagazinesId.Length; ++index)
      {
        if (ammoMagazineDefinitionId.SubtypeId == this.AmmoMagazinesId[index].SubtypeId)
          return true;
      }
      return false;
    }

    public int GetAmmoMagazineIdArrayIndex(MyDefinitionId ammoMagazineId)
    {
      for (int index = 0; index < this.AmmoMagazinesId.Length; ++index)
      {
        if (ammoMagazineId.SubtypeId == this.AmmoMagazinesId[index].SubtypeId)
          return index;
      }
      return -1;
    }

    public class MyWeaponAmmoData
    {
      public int RateOfFire;
      public int ShotsInBurst;
      public MySoundPair ShootSound;
      public int ShootIntervalInMiliseconds;

      public MyWeaponAmmoData(
        MyObjectBuilder_WeaponDefinition.WeaponAmmoData data)
        : this(data.RateOfFire, data.ShootSoundName, data.ShotsInBurst)
      {
      }

      public MyWeaponAmmoData(int rateOfFire, string soundName, int shotsInBurst)
      {
        this.RateOfFire = rateOfFire;
        this.ShotsInBurst = shotsInBurst;
        this.ShootSound = new MySoundPair(soundName);
        this.ShootIntervalInMiliseconds = (int) (1000.0 / ((double) this.RateOfFire / 60.0));
      }
    }

    public enum WeaponEffectAction
    {
      Unknown,
      Shoot,
    }

    public class MyWeaponEffect
    {
      public MyWeaponDefinition.WeaponEffectAction Action;
      public string Dummy = "";
      public string Particle = "";
      public bool Loop;
      public bool InstantStop;

      public MyWeaponEffect(
        string action,
        string dummy,
        string particle,
        bool loop,
        bool instantStop)
      {
        this.Dummy = dummy;
        this.Particle = particle;
        this.Loop = loop;
        this.InstantStop = instantStop;
        foreach (MyWeaponDefinition.WeaponEffectAction weaponEffectAction in Enum.GetValues(typeof (MyWeaponDefinition.WeaponEffectAction)))
        {
          if (weaponEffectAction.ToString().Equals(action))
          {
            this.Action = weaponEffectAction;
            break;
          }
        }
      }
    }

    private class Sandbox_Definitions_MyWeaponDefinition\u003C\u003EActor : IActivator, IActivator<MyWeaponDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWeaponDefinition();

      MyWeaponDefinition IActivator<MyWeaponDefinition>.CreateInstance() => new MyWeaponDefinition();
    }
  }
}
