// Decompiled with JetBrains decompiler
// Type: VRage.Data.Audio.MySoundData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Data.Audio
{
  public sealed class MySoundData
  {
    public MyStringId Category = MyStringId.GetOrCompute("Undefined");
    public MyCurveType VolumeCurve = MyCurveType.Custom_1;
    public float MaxDistance;
    public float UpdateDistance;
    public float Volume = 1f;
    public float VolumeVariation;
    public float PitchVariation;
    public float Pitch;
    public int SoundLimit;
    public bool DisablePitchEffects;
    public bool AlwaysUseOneMode;
    public bool StreamSound;
    public bool Loopable;
    public string Alternative2D;
    public bool UseOcclusion;
    public List<MyAudioWave> Waves;
    public List<DistantSound> DistantSounds;
    public int DynamicMusicAmount = 10;
    public MyStringId DynamicMusicCategory = MyStringId.NullOrEmpty;
    public MyMusicTrack MusicTrack;
    public int PreventSynchronization = -1;
    public bool ModifiableByHelmetFilters = true;
    public bool CanBeSilencedByVoid = true;
    public MyStringHash RealisticFilter = MyStringHash.NullOrEmpty;
    public float RealisticVolumeChange = 1f;
    public MyStringHash SubtypeId;

    public bool IsHudCue => StringComparer.InvariantCultureIgnoreCase.Equals(this.Category.ToString(), "hud");
  }
}
