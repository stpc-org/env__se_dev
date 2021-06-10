// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ShipSoundsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_ShipSoundsDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(16)]
    public float MinWeight = 3000f;
    [ProtoMember(19)]
    public bool AllowSmallGrid = true;
    [ProtoMember(22)]
    public bool AllowLargeGrid = true;
    [ProtoMember(25)]
    public float EnginePitchRangeInSemitones = 4f;
    [ProtoMember(28)]
    public float EngineTimeToTurnOn = 4f;
    [ProtoMember(31)]
    public float EngineTimeToTurnOff = 3f;
    [ProtoMember(34)]
    public float SpeedUpSoundChangeVolumeTo = 1f;
    [ProtoMember(37)]
    public float SpeedDownSoundChangeVolumeTo = 1f;
    [ProtoMember(40)]
    public float SpeedUpDownChangeSpeed = 0.2f;
    [ProtoMember(43)]
    public float WheelsPitchRangeInSemitones = 4f;
    [ProtoMember(46)]
    public float WheelsLowerThrusterVolumeBy = 0.33f;
    [ProtoMember(49)]
    public float WheelsFullSpeed = 32f;
    [ProtoMember(52)]
    public float WheelsGroundMinVolume = 0.5f;
    [ProtoMember(55)]
    public float ThrusterPitchRangeInSemitones = 4f;
    [ProtoMember(58)]
    public float ThrusterCompositionMinVolume = 0.4f;
    [ProtoMember(61)]
    public float ThrusterCompositionChangeSpeed = 0.025f;
    [ProtoMember(64)]
    [XmlArrayItem("WheelsVolume")]
    public List<ShipSoundVolumePair> WheelsVolumes;
    [ProtoMember(67)]
    [XmlArrayItem("ThrusterVolume")]
    public List<ShipSoundVolumePair> ThrusterVolumes;
    [ProtoMember(70)]
    [XmlArrayItem("EngineVolume")]
    public List<ShipSoundVolumePair> EngineVolumes;
    [ProtoMember(73)]
    [XmlArrayItem("Sound")]
    public List<ShipSound> Sounds;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EMinWeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.MinWeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.MinWeight;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EAllowSmallGrid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value) => owner.AllowSmallGrid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value) => value = owner.AllowSmallGrid;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EAllowLargeGrid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value) => owner.AllowLargeGrid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value) => value = owner.AllowLargeGrid;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EEnginePitchRangeInSemitones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.EnginePitchRangeInSemitones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.EnginePitchRangeInSemitones;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EEngineTimeToTurnOn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.EngineTimeToTurnOn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.EngineTimeToTurnOn;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EEngineTimeToTurnOff\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.EngineTimeToTurnOff = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.EngineTimeToTurnOff;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003ESpeedUpSoundChangeVolumeTo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.SpeedUpSoundChangeVolumeTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.SpeedUpSoundChangeVolumeTo;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003ESpeedDownSoundChangeVolumeTo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.SpeedDownSoundChangeVolumeTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.SpeedDownSoundChangeVolumeTo;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003ESpeedUpDownChangeSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.SpeedUpDownChangeSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.SpeedUpDownChangeSpeed;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EWheelsPitchRangeInSemitones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.WheelsPitchRangeInSemitones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.WheelsPitchRangeInSemitones;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EWheelsLowerThrusterVolumeBy\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.WheelsLowerThrusterVolumeBy = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.WheelsLowerThrusterVolumeBy;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EWheelsFullSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.WheelsFullSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.WheelsFullSpeed;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EWheelsGroundMinVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.WheelsGroundMinVolume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.WheelsGroundMinVolume;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EThrusterPitchRangeInSemitones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.ThrusterPitchRangeInSemitones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.ThrusterPitchRangeInSemitones;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EThrusterCompositionMinVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.ThrusterCompositionMinVolume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.ThrusterCompositionMinVolume;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EThrusterCompositionChangeSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in float value) => owner.ThrusterCompositionChangeSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out float value) => value = owner.ThrusterCompositionChangeSpeed;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EWheelsVolumes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        in List<ShipSoundVolumePair> value)
      {
        owner.WheelsVolumes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out List<ShipSoundVolumePair> value)
      {
        value = owner.WheelsVolumes;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EThrusterVolumes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        in List<ShipSoundVolumePair> value)
      {
        owner.ThrusterVolumes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out List<ShipSoundVolumePair> value)
      {
        value = owner.ThrusterVolumes;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EEngineVolumes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSoundVolumePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        in List<ShipSoundVolumePair> value)
      {
        owner.EngineVolumes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out List<ShipSoundVolumePair> value)
      {
        value = owner.EngineVolumes;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003ESounds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, List<ShipSound>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        in List<ShipSound> value)
      {
        owner.Sounds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out List<ShipSound> value)
      {
        value = owner.Sounds;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundsDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipSoundsDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipSoundsDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundsDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ShipSoundsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ShipSoundsDefinition();

      MyObjectBuilder_ShipSoundsDefinition IActivator<MyObjectBuilder_ShipSoundsDefinition>.CreateInstance() => new MyObjectBuilder_ShipSoundsDefinition();
    }
  }
}
