// Decompiled with JetBrains decompiler
// Type: VRage.Game.AlternativeImpactSounds
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  [XmlType("AlternativeImpactSound")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public sealed class AlternativeImpactSounds
  {
    [ProtoMember(31)]
    [XmlAttribute]
    public float mass;
    [ProtoMember(34)]
    [XmlAttribute]
    public string soundCue = "";
    [ProtoMember(37)]
    [XmlAttribute]
    public float minVelocity;
    [ProtoMember(40)]
    [XmlAttribute]
    public float maxVolumeVelocity;

    protected class VRage_Game_AlternativeImpactSounds\u003C\u003Emass\u003C\u003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AlternativeImpactSounds owner, in float value) => owner.mass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AlternativeImpactSounds owner, out float value) => value = owner.mass;
    }

    protected class VRage_Game_AlternativeImpactSounds\u003C\u003EsoundCue\u003C\u003EAccessor : IMemberAccessor<AlternativeImpactSounds, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AlternativeImpactSounds owner, in string value) => owner.soundCue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AlternativeImpactSounds owner, out string value) => value = owner.soundCue;
    }

    protected class VRage_Game_AlternativeImpactSounds\u003C\u003EminVelocity\u003C\u003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AlternativeImpactSounds owner, in float value) => owner.minVelocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AlternativeImpactSounds owner, out float value) => value = owner.minVelocity;
    }

    protected class VRage_Game_AlternativeImpactSounds\u003C\u003EmaxVolumeVelocity\u003C\u003EAccessor : IMemberAccessor<AlternativeImpactSounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AlternativeImpactSounds owner, in float value) => owner.maxVolumeVelocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AlternativeImpactSounds owner, out float value) => value = owner.maxVolumeVelocity;
    }

    private class VRage_Game_AlternativeImpactSounds\u003C\u003EActor : IActivator, IActivator<AlternativeImpactSounds>
    {
      object IActivator.CreateInstance() => (object) new AlternativeImpactSounds();

      AlternativeImpactSounds IActivator<AlternativeImpactSounds>.CreateInstance() => new AlternativeImpactSounds();
    }
  }
}
