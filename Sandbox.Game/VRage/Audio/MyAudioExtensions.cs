// Decompiled with JetBrains decompiler
// Type: VRage.Audio.MyAudioExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Linq;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Utils;

namespace VRage.Audio
{
  public static class MyAudioExtensions
  {
    public static readonly MySoundErrorDelegate OnSoundError = (MySoundErrorDelegate) ((cue, message) =>
    {
      MyAudioDefinition soundDefinition = MyDefinitionManager.Static.GetSoundDefinition(cue.SubtypeId);
      MyDefinitionErrors.Add(soundDefinition != null ? soundDefinition.Context : MyModContext.UnknownContext, message, TErrorSeverity.Error);
    });

    public static MyCueId GetCueId(this IMyAudio self, string cueName)
    {
      MyStringHash id;
      if (self == null || !MyStringHash.TryGet(cueName, out id))
        id = MyStringHash.NullOrEmpty;
      return new MyCueId(id);
    }

    internal static ListReader<MySoundData> GetSoundDataFromDefinitions() => (ListReader<MySoundData>) MyDefinitionManager.Static.GetSoundDefinitions().Where<MyAudioDefinition>((Func<MyAudioDefinition, bool>) (x => x.Enabled)).Select<MyAudioDefinition, MySoundData>((Func<MyAudioDefinition, MySoundData>) (x => x.SoundData)).ToList<MySoundData>();

    internal static ListReader<MyAudioEffect> GetEffectData() => (ListReader<MyAudioEffect>) MyDefinitionManager.Static.GetAudioEffectDefinitions().Select<MyAudioEffectDefinition, MyAudioEffect>((Func<MyAudioEffectDefinition, MyAudioEffect>) (x => x.Effect)).ToList<MyAudioEffect>();
  }
}
