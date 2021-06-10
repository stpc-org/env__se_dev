// Decompiled with JetBrains decompiler
// Type: VRage.Audio.MyAudio
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Collections;
using VRage.Data.Audio;

namespace VRage.Audio
{
  public class MyAudio
  {
    public static readonly int MAX_SAMPLE_RATE = 48000;

    public static MySoundErrorDelegate OnSoundError { get; private set; }

    public static IMyAudio Static { get; private set; }

    public static void LoadData(
      MyAudioInitParams initParams,
      ListReader<MySoundData> sounds,
      ListReader<MyAudioEffect> effects)
    {
      MyAudio.Static = initParams.Instance;
      MyAudio.OnSoundError = initParams.OnSoundError;
      MyAudio.Static.LoadData(initParams, sounds, effects);
    }

    public static void ReloadData(ListReader<MySoundData> sounds, ListReader<MyAudioEffect> effects) => MyAudio.Static.ReloadData(sounds, effects);

    public static void UnloadData()
    {
      if (MyAudio.Static == null)
        return;
      MyAudio.Static.UnloadData();
      MyAudio.Static.DisposeCache();
      MyAudio.Static = (IMyAudio) null;
    }
  }
}
