// Decompiled with JetBrains decompiler
// Type: VRage.Data.Audio.MyAudioEffect
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace VRage.Data.Audio
{
  public class MyAudioEffect
  {
    public int ResultEmitterIdx;
    public List<List<MyAudioEffect.SoundEffect>> SoundsEffects = new List<List<MyAudioEffect.SoundEffect>>();
    public MyStringHash EffectId;

    public enum FilterType
    {
      LowPass,
      BandPass,
      HighPass,
      Notch,
      None,
    }

    public struct SoundEffect
    {
      public Curve VolumeCurve;
      public float Duration;
      public MyAudioEffect.FilterType Filter;
      public float Frequency;
      public bool StopAfter;
      public float OneOverQ;
    }
  }
}
