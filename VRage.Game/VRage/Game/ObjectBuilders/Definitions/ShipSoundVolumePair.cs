// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.ShipSoundVolumePair
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  public class ShipSoundVolumePair
  {
    [ProtoMember(82)]
    [XmlAttribute("Speed")]
    public float Speed;
    [ProtoMember(85)]
    [XmlAttribute("Volume")]
    public float Volume;

    protected class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair\u003C\u003ESpeed\u003C\u003EAccessor : IMemberAccessor<ShipSoundVolumePair, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShipSoundVolumePair owner, in float value) => owner.Speed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShipSoundVolumePair owner, out float value) => value = owner.Speed;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair\u003C\u003EVolume\u003C\u003EAccessor : IMemberAccessor<ShipSoundVolumePair, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShipSoundVolumePair owner, in float value) => owner.Volume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShipSoundVolumePair owner, out float value) => value = owner.Volume;
    }

    private class VRage_Game_ObjectBuilders_Definitions_ShipSoundVolumePair\u003C\u003EActor : IActivator, IActivator<ShipSoundVolumePair>
    {
      object IActivator.CreateInstance() => (object) new ShipSoundVolumePair();

      ShipSoundVolumePair IActivator<ShipSoundVolumePair>.CreateInstance() => new ShipSoundVolumePair();
    }
  }
}
