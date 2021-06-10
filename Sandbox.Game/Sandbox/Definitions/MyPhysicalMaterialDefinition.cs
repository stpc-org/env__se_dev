// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPhysicalMaterialDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicalMaterialDefinition), null)]
  public class MyPhysicalMaterialDefinition : MyDefinitionBase
  {
    public float Density;
    public float HorisontalTransmissionMultiplier;
    public float HorisontalFragility;
    public float SupportMultiplier;
    public float CollisionMultiplier;
    public Dictionary<MyStringId, Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>> CollisionProperties = new Dictionary<MyStringId, Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    public Dictionary<MyStringId, MySoundPair> GeneralSounds = new Dictionary<MyStringId, MySoundPair>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    public MyStringHash InheritFrom = MyStringHash.NullOrEmpty;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (builder is MyObjectBuilder_PhysicalMaterialDefinition materialDefinition)
      {
        this.Density = materialDefinition.Density;
        this.HorisontalTransmissionMultiplier = materialDefinition.HorisontalTransmissionMultiplier;
        this.HorisontalFragility = materialDefinition.HorisontalFragility;
        this.SupportMultiplier = materialDefinition.SupportMultiplier;
        this.CollisionMultiplier = materialDefinition.CollisionMultiplier;
      }
      if (!(builder is MyObjectBuilder_MaterialPropertiesDefinition propertiesDefinition))
        return;
      this.InheritFrom = MyStringHash.GetOrCompute(propertiesDefinition.InheritFrom);
      foreach (MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty contactProperty in propertiesDefinition.ContactProperties)
      {
        MyStringId orCompute1 = MyStringId.GetOrCompute(contactProperty.Type);
        if (!this.CollisionProperties.ContainsKey(orCompute1))
          this.CollisionProperties[orCompute1] = new Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
        MyStringHash orCompute2 = MyStringHash.GetOrCompute(contactProperty.Material);
        this.CollisionProperties[orCompute1][orCompute2] = new MyPhysicalMaterialDefinition.CollisionProperty(contactProperty.SoundCue, contactProperty.ParticleEffect, contactProperty.AlternativeImpactSounds);
      }
      foreach (MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty generalProperty in propertiesDefinition.GeneralProperties)
        this.GeneralSounds[MyStringId.GetOrCompute(generalProperty.Type)] = new MySoundPair(generalProperty.SoundCue);
    }

    public struct CollisionProperty
    {
      public MySoundPair Sound;
      public string ParticleEffect;
      public List<MyPhysicalMaterialDefinition.ImpactSounds> ImpactSoundCues;

      public CollisionProperty(
        string soundCue,
        string particleEffectName,
        List<AlternativeImpactSounds> impactsounds)
      {
        this.Sound = new MySoundPair(soundCue);
        this.ParticleEffect = particleEffectName;
        if (impactsounds == null || impactsounds.Count == 0)
        {
          this.ImpactSoundCues = (List<MyPhysicalMaterialDefinition.ImpactSounds>) null;
        }
        else
        {
          this.ImpactSoundCues = new List<MyPhysicalMaterialDefinition.ImpactSounds>();
          foreach (AlternativeImpactSounds impactsound in impactsounds)
            this.ImpactSoundCues.Add(new MyPhysicalMaterialDefinition.ImpactSounds(impactsound.mass, impactsound.soundCue, impactsound.minVelocity, impactsound.maxVolumeVelocity));
        }
      }
    }

    public struct ImpactSounds
    {
      public float Mass;
      public MySoundPair SoundCue;
      public float minVelocity;
      public float maxVolumeVelocity;

      public ImpactSounds(float mass, string soundCue, float minVelocity, float maxVolumeVelocity)
      {
        this.Mass = mass;
        this.SoundCue = new MySoundPair(soundCue);
        this.minVelocity = minVelocity;
        this.maxVolumeVelocity = maxVolumeVelocity;
      }
    }

    private class Sandbox_Definitions_MyPhysicalMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyPhysicalMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicalMaterialDefinition();

      MyPhysicalMaterialDefinition IActivator<MyPhysicalMaterialDefinition>.CreateInstance() => new MyPhysicalMaterialDefinition();
    }
  }
}
