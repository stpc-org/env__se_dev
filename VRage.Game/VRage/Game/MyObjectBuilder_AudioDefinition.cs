// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AudioDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data.Audio;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [XmlType("Sound")]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public sealed class MyObjectBuilder_AudioDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlIgnore]
    public MySoundData SoundData = new MySoundData();

    [ProtoMember(1)]
    public string Category
    {
      get => this.SoundData.Category.ToString();
      set => this.SoundData.Category = MyStringId.GetOrCompute(value);
    }

    [ProtoMember(4)]
    [DefaultValue(MyCurveType.Custom_1)]
    public MyCurveType VolumeCurve
    {
      get => this.SoundData.VolumeCurve;
      set => this.SoundData.VolumeCurve = value;
    }

    [ProtoMember(7)]
    public float MaxDistance
    {
      get => this.SoundData.MaxDistance;
      set => this.SoundData.MaxDistance = value;
    }

    [ProtoMember(10)]
    public float UpdateDistance
    {
      get => this.SoundData.UpdateDistance;
      set => this.SoundData.UpdateDistance = value;
    }

    [ProtoMember(13)]
    [DefaultValue(1f)]
    public float Volume
    {
      get => this.SoundData.Volume;
      set => this.SoundData.Volume = value;
    }

    [ProtoMember(16)]
    [DefaultValue(0.0f)]
    public float VolumeVariation
    {
      get => this.SoundData.VolumeVariation;
      set => this.SoundData.VolumeVariation = value;
    }

    [ProtoMember(19)]
    [DefaultValue(0.0f)]
    public float PitchVariation
    {
      get => this.SoundData.PitchVariation;
      set => this.SoundData.PitchVariation = value;
    }

    [ProtoMember(22)]
    [DefaultValue(0.0f)]
    public float Pitch
    {
      get => this.SoundData.Pitch;
      set => this.SoundData.Pitch = value;
    }

    [ProtoMember(25)]
    [DefaultValue(-1)]
    public int PreventSynchronization
    {
      get => this.SoundData.PreventSynchronization;
      set => this.SoundData.PreventSynchronization = value;
    }

    [ProtoMember(28)]
    public string DynamicMusicCategory
    {
      get => this.SoundData.DynamicMusicCategory.ToString();
      set => this.SoundData.DynamicMusicCategory = MyStringId.GetOrCompute(value);
    }

    [ProtoMember(31)]
    public int DynamicMusicAmount
    {
      get => this.SoundData.DynamicMusicAmount;
      set => this.SoundData.DynamicMusicAmount = value;
    }

    [ProtoMember(34)]
    [DefaultValue(true)]
    public bool ModifiableByHelmetFilters
    {
      get => this.SoundData.ModifiableByHelmetFilters;
      set => this.SoundData.ModifiableByHelmetFilters = value;
    }

    [ProtoMember(37)]
    [DefaultValue(false)]
    public bool AlwaysUseOneMode
    {
      get => this.SoundData.AlwaysUseOneMode;
      set => this.SoundData.AlwaysUseOneMode = value;
    }

    [ProtoMember(40)]
    [DefaultValue(true)]
    public bool CanBeSilencedByVoid
    {
      get => this.SoundData.CanBeSilencedByVoid;
      set => this.SoundData.CanBeSilencedByVoid = value;
    }

    [ProtoMember(43)]
    [DefaultValue(false)]
    public bool StreamSound
    {
      get => this.SoundData.StreamSound;
      set => this.SoundData.StreamSound = value;
    }

    [ProtoMember(46)]
    [DefaultValue(false)]
    public bool DisablePitchEffects
    {
      get => this.SoundData.DisablePitchEffects;
      set => this.SoundData.DisablePitchEffects = value;
    }

    [ProtoMember(49)]
    [DefaultValue(0)]
    public int SoundLimit
    {
      get => this.SoundData.SoundLimit;
      set => this.SoundData.SoundLimit = value;
    }

    [ProtoMember(52)]
    [DefaultValue(false)]
    public bool Loopable
    {
      get => this.SoundData.Loopable;
      set => this.SoundData.Loopable = value;
    }

    [ProtoMember(55)]
    public string Alternative2D
    {
      get => this.SoundData.Alternative2D;
      set => this.SoundData.Alternative2D = value;
    }

    [ProtoMember(58)]
    [DefaultValue(false)]
    public bool UseOcclusion
    {
      get => this.SoundData.UseOcclusion;
      set => this.SoundData.UseOcclusion = value;
    }

    [ProtoMember(61)]
    public List<MyAudioWave> Waves
    {
      get => this.SoundData.Waves;
      set => this.SoundData.Waves = value;
    }

    [ProtoMember(64)]
    public List<DistantSound> DistantSounds
    {
      get => this.SoundData.DistantSounds;
      set => this.SoundData.DistantSounds = value;
    }

    [ProtoMember(67)]
    [DefaultValue("")]
    public string TransitionCategory
    {
      get => this.SoundData.MusicTrack.TransitionCategory.ToString();
      set => this.SoundData.MusicTrack.TransitionCategory = MyStringId.GetOrCompute(value);
    }

    [ProtoMember(70)]
    [DefaultValue("")]
    public string MusicCategory
    {
      get => this.SoundData.MusicTrack.MusicCategory.ToString();
      set => this.SoundData.MusicTrack.MusicCategory = MyStringId.GetOrCompute(value);
    }

    [ProtoMember(73)]
    [DefaultValue("")]
    public string RealisticFilter
    {
      get => this.SoundData.RealisticFilter.String;
      set => this.SoundData.RealisticFilter = MyStringHash.GetOrCompute(value);
    }

    [ProtoMember(76)]
    [DefaultValue(1f)]
    public float RealisticVolumeChange
    {
      get => this.SoundData.RealisticVolumeChange;
      set => this.SoundData.RealisticVolumeChange = value;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ESoundData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, MySoundData>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in MySoundData value) => owner.SoundData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out MySoundData value) => value = owner.SoundData;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.Category = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.Category;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EVolumeCurve\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, MyCurveType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in MyCurveType value) => owner.VolumeCurve = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out MyCurveType value) => value = owner.VolumeCurve;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EMaxDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.MaxDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.MaxDistance;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EUpdateDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.UpdateDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.UpdateDistance;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.Volume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.Volume;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EVolumeVariation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.VolumeVariation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.VolumeVariation;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EPitchVariation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.PitchVariation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.PitchVariation;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EPitch\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.Pitch = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.Pitch;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EPreventSynchronization\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in int value) => owner.PreventSynchronization = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out int value) => value = owner.PreventSynchronization;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDynamicMusicCategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.DynamicMusicCategory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.DynamicMusicCategory;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDynamicMusicAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in int value) => owner.DynamicMusicAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out int value) => value = owner.DynamicMusicAmount;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EModifiableByHelmetFilters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.ModifiableByHelmetFilters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.ModifiableByHelmetFilters;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EAlwaysUseOneMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.AlwaysUseOneMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.AlwaysUseOneMode;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ECanBeSilencedByVoid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.CanBeSilencedByVoid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.CanBeSilencedByVoid;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EStreamSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.StreamSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.StreamSound;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDisablePitchEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.DisablePitchEffects = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.DisablePitchEffects;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ESoundLimit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in int value) => owner.SoundLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out int value) => value = owner.SoundLimit;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ELoopable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.Loopable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.Loopable;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EAlternative2D\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.Alternative2D = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.Alternative2D;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EUseOcclusion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in bool value) => owner.UseOcclusion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out bool value) => value = owner.UseOcclusion;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EWaves\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, List<MyAudioWave>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in List<MyAudioWave> value) => owner.Waves = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioDefinition owner,
        out List<MyAudioWave> value)
      {
        value = owner.Waves;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EDistantSounds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, List<DistantSound>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioDefinition owner,
        in List<DistantSound> value)
      {
        owner.DistantSounds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioDefinition owner,
        out List<DistantSound> value)
      {
        value = owner.DistantSounds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ETransitionCategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.TransitionCategory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.TransitionCategory;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EMusicCategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.MusicCategory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.MusicCategory;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ERealisticFilter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => owner.RealisticFilter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => value = owner.RealisticFilter;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ERealisticVolumeChange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in float value) => owner.RealisticVolumeChange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out float value) => value = owner.RealisticVolumeChange;
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AudioDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AudioDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AudioDefinition();

      MyObjectBuilder_AudioDefinition IActivator<MyObjectBuilder_AudioDefinition>.CreateInstance() => new MyObjectBuilder_AudioDefinition();
    }
  }
}
