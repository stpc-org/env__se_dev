// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAudioEffectDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AudioEffectDefinition), null)]
  public class MyAudioEffectDefinition : MyDefinitionBase
  {
    public MyAudioEffect Effect = new MyAudioEffect();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AudioEffectDefinition effectDefinition = builder as MyObjectBuilder_AudioEffectDefinition;
      this.Effect.EffectId = this.Id.SubtypeId;
      foreach (MyObjectBuilder_AudioEffectDefinition.SoundList sound in effectDefinition.Sounds)
      {
        List<MyAudioEffect.SoundEffect> soundEffectList = new List<MyAudioEffect.SoundEffect>();
        foreach (MyObjectBuilder_AudioEffectDefinition.SoundEffect soundEffect in sound.SoundEffects)
        {
          MyCurveDefinition definition;
          soundEffectList.Add(new MyAudioEffect.SoundEffect()
          {
            VolumeCurve = MyDefinitionManager.Static.TryGetDefinition<MyCurveDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CurveDefinition), soundEffect.VolumeCurve), out definition) ? definition.Curve : (Curve) null,
            Duration = soundEffect.Duration,
            Filter = soundEffect.Filter,
            Frequency = MathHelper.Clamp((float) (2.0 * Math.Sin(3.14 * (double) soundEffect.Frequency / 44100.0)), 0.0f, 1f),
            OneOverQ = MathHelper.Clamp(1f / soundEffect.Q, 0.0f, 1.5f),
            StopAfter = soundEffect.StopAfter
          });
        }
        this.Effect.SoundsEffects.Add(soundEffectList);
      }
      if (effectDefinition.OutputSound == 0)
        this.Effect.ResultEmitterIdx = this.Effect.SoundsEffects.Count - 1;
      else
        this.Effect.ResultEmitterIdx = effectDefinition.OutputSound - 1;
    }

    private class Sandbox_Definitions_MyAudioEffectDefinition\u003C\u003EActor : IActivator, IActivator<MyAudioEffectDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAudioEffectDefinition();

      MyAudioEffectDefinition IActivator<MyAudioEffectDefinition>.CreateInstance() => new MyAudioEffectDefinition();
    }
  }
}
