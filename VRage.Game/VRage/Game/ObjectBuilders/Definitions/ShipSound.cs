// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.ShipSound
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
  public class ShipSound
  {
    [ProtoMember(76)]
    [XmlAttribute("Type")]
    public ShipSystemSoundsEnum SoundType = ShipSystemSoundsEnum.MainLoopMedium;
    [ProtoMember(79)]
    [XmlAttribute("SoundName")]
    public string SoundName = "";

    protected class VRage_Game_ObjectBuilders_Definitions_ShipSound\u003C\u003ESoundType\u003C\u003EAccessor : IMemberAccessor<ShipSound, ShipSystemSoundsEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShipSound owner, in ShipSystemSoundsEnum value) => owner.SoundType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShipSound owner, out ShipSystemSoundsEnum value) => value = owner.SoundType;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_ShipSound\u003C\u003ESoundName\u003C\u003EAccessor : IMemberAccessor<ShipSound, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShipSound owner, in string value) => owner.SoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShipSound owner, out string value) => value = owner.SoundName;
    }

    private class VRage_Game_ObjectBuilders_Definitions_ShipSound\u003C\u003EActor : IActivator, IActivator<ShipSound>
    {
      object IActivator.CreateInstance() => (object) new ShipSound();

      ShipSound IActivator<ShipSound>.CreateInstance() => new ShipSound();
    }
  }
}
