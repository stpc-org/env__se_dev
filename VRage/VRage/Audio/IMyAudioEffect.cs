// Decompiled with JetBrains decompiler
// Type: VRage.Audio.IMyAudioEffect
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Audio
{
  public interface IMyAudioEffect
  {
    bool AutoUpdate { get; set; }

    void Update(int stepInMsec);

    void SetPosition(float msecs);

    void SetPositionRelative(float position);

    IMySourceVoice OutputSound { get; }

    bool Finished { get; }
  }
}
