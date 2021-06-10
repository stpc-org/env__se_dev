// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyParticleEffectDataSerializer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Render.Particles;
using VRage.Utils;

namespace VRage.Game
{
  public static class MyParticleEffectDataSerializer
  {
    public static void DeserializeFromObjectBuilder(
      MyParticleEffectData data,
      MyObjectBuilder_ParticleEffect builder)
    {
      data.Start(builder.ParticleId, builder.Id.SubtypeName);
      data.Length = builder.Length;
      data.Loop = builder.Loop;
      data.DurationMin = builder.DurationMin;
      data.DurationMax = builder.DurationMax;
      data.DistanceMax = builder.DistanceMax;
      data.Priority = builder.Priority;
      foreach (ParticleGeneration particleGeneration in builder.ParticleGenerations)
      {
        string generationType = particleGeneration.GenerationType;
        if (!(generationType == "CPU"))
        {
          if (generationType == "GPU")
          {
            MyParticleGPUGenerationData generation = new MyParticleGPUGenerationData();
            generation.Start(data);
            generation.DeserializeFromObjectBuilder(particleGeneration);
            data.AddGeneration(generation, true);
          }
        }
        else
          MyLog.Default.WriteLine("CPU Particles are not supported anymore: " + data.Name + " / " + particleGeneration.Name);
      }
      foreach (ParticleLight particleLight1 in builder.ParticleLights)
      {
        MyParticleLightData particleLight2 = new MyParticleLightData();
        particleLight2.Start(data);
        particleLight2.DeserializeFromObjectBuilder(particleLight1);
        data.AddParticleLight(particleLight2, true);
      }
    }

    public static MyObjectBuilder_ParticleEffect SerializeToObjectBuilder(
      MyParticleEffectData data)
    {
      MyObjectBuilder_ParticleEffect builderParticleEffect = new MyObjectBuilder_ParticleEffect();
      builderParticleEffect.ParticleId = data.ID;
      builderParticleEffect.Id.TypeIdString = "MyObjectBuilder_ParticleEffect";
      builderParticleEffect.Id.SubtypeName = data.Name;
      builderParticleEffect.Length = data.Length;
      builderParticleEffect.Loop = data.Loop;
      builderParticleEffect.DurationMin = data.DurationMin;
      builderParticleEffect.DurationMax = data.DurationMax;
      builderParticleEffect.DistanceMax = data.DistanceMax;
      builderParticleEffect.ParticleGenerations = new List<ParticleGeneration>();
      foreach (MyParticleGPUGenerationData generation in data.GetGenerations())
      {
        ParticleGeneration objectBuilder = generation.SerializeToObjectBuilder();
        builderParticleEffect.ParticleGenerations.Add(objectBuilder);
      }
      builderParticleEffect.ParticleLights = new List<ParticleLight>();
      foreach (MyParticleLightData particleLight in data.GetParticleLights())
      {
        ParticleLight objectBuilder = particleLight.SerializeToObjectBuilder();
        builderParticleEffect.ParticleLights.Add(objectBuilder);
      }
      return builderParticleEffect;
    }
  }
}
