// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeaponDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeaponDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(4)]
    public MyObjectBuilder_WeaponDefinition.WeaponAmmoData ProjectileAmmoData;
    [ProtoMember(7)]
    public MyObjectBuilder_WeaponDefinition.WeaponAmmoData MissileAmmoData;
    [ProtoMember(10)]
    public string NoAmmoSoundName;
    [ProtoMember(13)]
    public string ReloadSoundName;
    [ProtoMember(16)]
    public string SecondarySoundName;
    [ProtoMember(19)]
    public string PhysicalMaterial = "Metal";
    [ProtoMember(22)]
    public float DeviateShotAngle;
    [ProtoMember(23)]
    public float DeviateShotAngleAiming;
    [ProtoMember(25)]
    public float ReleaseTimeAfterFire;
    [ProtoMember(28)]
    public int MuzzleFlashLifeSpan;
    [ProtoMember(31)]
    public int ReloadTime = 2000;
    [XmlArrayItem("AmmoMagazine")]
    [ProtoMember(34)]
    public MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine[] AmmoMagazines;
    [XmlArrayItem("Effect")]
    [ProtoMember(37)]
    public MyObjectBuilder_WeaponDefinition.WeaponEffect[] Effects;
    [ProtoMember(40)]
    public bool UseDefaultMuzzleFlash = true;
    [ProtoMember(58)]
    [DefaultValue(1)]
    public float DamageMultiplier = 1f;
    [ProtoMember(61, IsRequired = false)]
    [DefaultValue(1)]
    public float RangeMultiplier = 1f;
    [ProtoMember(64, IsRequired = false)]
    [DefaultValue(true)]
    public bool UseRandomizedRange = true;
    [ProtoMember(65)]
    public float RecoilJetpackVertical;
    [ProtoMember(67)]
    public float RecoilJetpackHorizontal;
    [ProtoMember(69)]
    public float RecoilGroundVertical;
    [ProtoMember(71)]
    public float RecoilGroundHorizontal;
    [ProtoMember(73)]
    public List<string> RecoilMultiplierDataNames = new List<string>();
    [ProtoMember(75)]
    public List<float> RecoilMultiplierDataVerticals = new List<float>();
    [ProtoMember(77)]
    public List<float> RecoilMultiplierDataHorizontals = new List<float>();
    [ProtoMember(79)]
    public float RecoilResetTimeMilliseconds;
    [ProtoMember(81)]
    public int ShootDirectionUpdateTime = 200;
    [ProtoMember(82)]
    public float EquipDuration = 0.5f;
    [ProtoMember(83)]
    public bool ShakeOnActionPrimary = true;
    [ProtoMember(84)]
    public bool ShakeOnActionSecondary = true;
    [ProtoMember(85)]
    public bool ShakeOnActionTertiary = true;

    [ProtoContract]
    public class WeaponAmmoData
    {
      [XmlAttribute]
      public int RateOfFire;
      [XmlAttribute]
      public string ShootSoundName;
      [XmlAttribute]
      public int ShotsInBurst;

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoData\u003C\u003ERateOfFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponAmmoData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          in int value)
        {
          owner.RateOfFire = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          out int value)
        {
          value = owner.RateOfFire;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoData\u003C\u003EShootSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponAmmoData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          in string value)
        {
          owner.ShootSoundName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          out string value)
        {
          value = owner.ShootSoundName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoData\u003C\u003EShotsInBurst\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponAmmoData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          in int value)
        {
          owner.ShotsInBurst = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoData owner,
          out int value)
        {
          value = owner.ShotsInBurst;
        }
      }

      private class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponDefinition.WeaponAmmoData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponDefinition.WeaponAmmoData();

        MyObjectBuilder_WeaponDefinition.WeaponAmmoData IActivator<MyObjectBuilder_WeaponDefinition.WeaponAmmoData>.CreateInstance() => new MyObjectBuilder_WeaponDefinition.WeaponAmmoData();
      }
    }

    [ProtoContract]
    public class WeaponAmmoMagazine
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_AmmoMagazine);
      [XmlAttribute]
      [ProtoMember(1)]
      public string Subtype;

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoMagazine\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoMagazine\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      private class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponAmmoMagazine\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine();

        MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine IActivator<MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine>.CreateInstance() => new MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine();
      }
    }

    [ProtoContract]
    public class WeaponEffect
    {
      [XmlAttribute]
      [ProtoMember(43)]
      public string Action = "";
      [XmlAttribute]
      [ProtoMember(46)]
      public string Dummy = "";
      [XmlAttribute]
      [ProtoMember(49)]
      public string Particle = "";
      [XmlAttribute]
      [ProtoMember(52)]
      public bool Loop;
      [XmlAttribute]
      [ProtoMember(55, IsRequired = false)]
      public bool InstantStop = true;

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003EAction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          in string value)
        {
          owner.Action = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          out string value)
        {
          value = owner.Action;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003EDummy\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          in string value)
        {
          owner.Dummy = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          out string value)
        {
          value = owner.Dummy;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003EParticle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          in string value)
        {
          owner.Particle = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          out string value)
        {
          value = owner.Particle;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponEffect, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          in bool value)
        {
          owner.Loop = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          out bool value)
        {
          value = owner.Loop;
        }
      }

      protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003EInstantStop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition.WeaponEffect, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          in bool value)
        {
          owner.InstantStop = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_WeaponDefinition.WeaponEffect owner,
          out bool value)
        {
          value = owner.InstantStop;
        }
      }

      private class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EWeaponEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponDefinition.WeaponEffect>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponDefinition.WeaponEffect();

        MyObjectBuilder_WeaponDefinition.WeaponEffect IActivator<MyObjectBuilder_WeaponDefinition.WeaponEffect>.CreateInstance() => new MyObjectBuilder_WeaponDefinition.WeaponEffect();
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EProjectileAmmoData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyObjectBuilder_WeaponDefinition.WeaponAmmoData>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponDefinition owner,
        in MyObjectBuilder_WeaponDefinition.WeaponAmmoData value)
      {
        owner.ProjectileAmmoData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponDefinition owner,
        out MyObjectBuilder_WeaponDefinition.WeaponAmmoData value)
      {
        value = owner.ProjectileAmmoData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EMissileAmmoData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyObjectBuilder_WeaponDefinition.WeaponAmmoData>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponDefinition owner,
        in MyObjectBuilder_WeaponDefinition.WeaponAmmoData value)
      {
        owner.MissileAmmoData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponDefinition owner,
        out MyObjectBuilder_WeaponDefinition.WeaponAmmoData value)
      {
        value = owner.MissileAmmoData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ENoAmmoSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => owner.NoAmmoSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => value = owner.NoAmmoSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EReloadSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => owner.ReloadSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => value = owner.ReloadSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ESecondarySoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => owner.SecondarySoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => value = owner.SecondarySoundName;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => owner.PhysicalMaterial = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => value = owner.PhysicalMaterial;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDeviateShotAngle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.DeviateShotAngle = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.DeviateShotAngle;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDeviateShotAngleAiming\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.DeviateShotAngleAiming = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.DeviateShotAngleAiming;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EReleaseTimeAfterFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.ReleaseTimeAfterFire = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.ReleaseTimeAfterFire;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EMuzzleFlashLifeSpan\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in int value) => owner.MuzzleFlashLifeSpan = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out int value) => value = owner.MuzzleFlashLifeSpan;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EReloadTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in int value) => owner.ReloadTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out int value) => value = owner.ReloadTime;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EAmmoMagazines\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponDefinition owner,
        in MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine[] value)
      {
        owner.AmmoMagazines = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponDefinition owner,
        out MyObjectBuilder_WeaponDefinition.WeaponAmmoMagazine[] value)
      {
        value = owner.AmmoMagazines;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyObjectBuilder_WeaponDefinition.WeaponEffect[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponDefinition owner,
        in MyObjectBuilder_WeaponDefinition.WeaponEffect[] value)
      {
        owner.Effects = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponDefinition owner,
        out MyObjectBuilder_WeaponDefinition.WeaponEffect[] value)
      {
        value = owner.Effects;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EUseDefaultMuzzleFlash\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => owner.UseDefaultMuzzleFlash = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => value = owner.UseDefaultMuzzleFlash;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDamageMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.DamageMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.DamageMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERangeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RangeMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RangeMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EUseRandomizedRange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => owner.UseRandomizedRange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => value = owner.UseRandomizedRange;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilJetpackVertical\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RecoilJetpackVertical = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RecoilJetpackVertical;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilJetpackHorizontal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RecoilJetpackHorizontal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RecoilJetpackHorizontal;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilGroundVertical\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RecoilGroundVertical = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RecoilGroundVertical;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilGroundHorizontal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RecoilGroundHorizontal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RecoilGroundHorizontal;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilMultiplierDataNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in List<string> value) => owner.RecoilMultiplierDataNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out List<string> value) => value = owner.RecoilMultiplierDataNames;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilMultiplierDataVerticals\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, List<float>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in List<float> value) => owner.RecoilMultiplierDataVerticals = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out List<float> value) => value = owner.RecoilMultiplierDataVerticals;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilMultiplierDataHorizontals\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, List<float>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in List<float> value) => owner.RecoilMultiplierDataHorizontals = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out List<float> value) => value = owner.RecoilMultiplierDataHorizontals;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ERecoilResetTimeMilliseconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.RecoilResetTimeMilliseconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.RecoilResetTimeMilliseconds;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EShootDirectionUpdateTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in int value) => owner.ShootDirectionUpdateTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out int value) => value = owner.ShootDirectionUpdateTime;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EEquipDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in float value) => owner.EquipDuration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out float value) => value = owner.EquipDuration;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EShakeOnActionPrimary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => owner.ShakeOnActionPrimary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => value = owner.ShakeOnActionPrimary;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EShakeOnActionSecondary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => owner.ShakeOnActionSecondary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => value = owner.ShakeOnActionSecondary;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EShakeOnActionTertiary\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => owner.ShakeOnActionTertiary = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => value = owner.ShakeOnActionTertiary;
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeaponDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeaponDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeaponDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeaponDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeaponDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WeaponDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeaponDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeaponDefinition();

      MyObjectBuilder_WeaponDefinition IActivator<MyObjectBuilder_WeaponDefinition>.CreateInstance() => new MyObjectBuilder_WeaponDefinition();
    }
  }
}
