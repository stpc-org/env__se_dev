// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeatherEffectDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeatherEffectDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(5)]
    public Vector3 FogColor;
    [ProtoMember(10)]
    public float FogDensity;
    [ProtoMember(15)]
    public float FogMultiplier;
    [ProtoMember(20)]
    public float FogSkyboxMultiplier;
    [ProtoMember(25)]
    public float FogAtmoMultiplier;
    [ProtoMember(30)]
    public string AmbientSound;
    [ProtoMember(35)]
    public float AmbientVolume;
    [ProtoMember(40)]
    public string EffectName;
    [ProtoMember(45)]
    public float ParticleRadius;
    [ProtoMember(50)]
    public int ParticleCount;
    [ProtoMember(55)]
    public float ParticleScale;
    [ProtoMember(61)]
    public float LightningIntervalMin;
    [ProtoMember(62)]
    public float LightningIntervalMax;
    [ProtoMember(63)]
    public float LightningGridHitIntervalMin;
    [ProtoMember(64)]
    public float LightningGridHitIntervalMax;
    [ProtoMember(66)]
    public float LightningCharacterHitIntervalMin;
    [ProtoMember(69)]
    public float LightningCharacterHitIntervalMax;
    [ProtoMember(70)]
    public float ParticleAlphaMultiplier;
    [ProtoMember(75)]
    [DefaultValue(150)]
    public float SunIntensity = 150f;
    [ProtoMember(76)]
    public Vector3 SunColor;
    [ProtoMember(77)]
    public Vector3 SunSpecularColor;
    [ProtoMember(80)]
    public float ShadowFadeout;
    [ProtoMember(85)]
    public float WindOutputModifier = 1f;
    [ProtoMember(90)]
    public float SolarOutputModifier = 1f;
    [ProtoMember(95)]
    public float TemperatureModifier = 1f;
    [ProtoMember(100)]
    public float OxygenLevelModifier = 1f;
    [ProtoMember(105)]
    public MyObjectBuilder_WeatherLightning Lightning;
    [ProtoMember(110)]
    public float FoliageWindModifier = 1f;

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFogColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value) => owner.FogColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value) => value = owner.FogColor;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFogDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.FogDensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.FogDensity;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFogMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.FogMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.FogMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFogSkyboxMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.FogSkyboxMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.FogSkyboxMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFogAtmoMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.FogAtmoMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.FogAtmoMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EAmbientSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => owner.AmbientSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => value = owner.AmbientSound;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EAmbientVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.AmbientVolume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.AmbientVolume;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EEffectName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => owner.EffectName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => value = owner.EffectName;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EParticleRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.ParticleRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.ParticleRadius;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EParticleCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in int value) => owner.ParticleCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out int value) => value = owner.ParticleCount;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EParticleScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.ParticleScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.ParticleScale;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningIntervalMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningIntervalMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningIntervalMin;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningIntervalMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningIntervalMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningIntervalMax;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningGridHitIntervalMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningGridHitIntervalMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningGridHitIntervalMin;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningGridHitIntervalMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningGridHitIntervalMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningGridHitIntervalMax;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningCharacterHitIntervalMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningCharacterHitIntervalMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningCharacterHitIntervalMin;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightningCharacterHitIntervalMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.LightningCharacterHitIntervalMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.LightningCharacterHitIntervalMax;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EParticleAlphaMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.ParticleAlphaMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.ParticleAlphaMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ESunIntensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.SunIntensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.SunIntensity;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ESunColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value) => owner.SunColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value) => value = owner.SunColor;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ESunSpecularColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in Vector3 value) => owner.SunSpecularColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out Vector3 value) => value = owner.SunSpecularColor;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EShadowFadeout\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.ShadowFadeout = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.ShadowFadeout;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EWindOutputModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.WindOutputModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.WindOutputModifier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ESolarOutputModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.SolarOutputModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.SolarOutputModifier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ETemperatureModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.TemperatureModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.TemperatureModifier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EOxygenLevelModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.OxygenLevelModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.OxygenLevelModifier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ELightning\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyObjectBuilder_WeatherLightning>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        in MyObjectBuilder_WeatherLightning value)
      {
        owner.Lightning = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        out MyObjectBuilder_WeatherLightning value)
      {
        value = owner.Lightning;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EFoliageWindModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in float value) => owner.FoliageWindModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out float value) => value = owner.FoliageWindModifier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeatherEffectDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WeatherEffectDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeatherEffectDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeatherEffectDefinition();

      MyObjectBuilder_WeatherEffectDefinition IActivator<MyObjectBuilder_WeatherEffectDefinition>.CreateInstance() => new MyObjectBuilder_WeatherEffectDefinition();
    }
  }
}
