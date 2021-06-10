// Decompiled with JetBrains decompiler
// Type: VRage.Game.ToolSound
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
  [XmlType("ToolSound")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public struct ToolSound
  {
    [ProtoMember(169)]
    [XmlAttribute]
    public string type;
    [ProtoMember(172)]
    [XmlAttribute]
    public string subtype;
    [ProtoMember(175)]
    [XmlAttribute]
    public string sound;

    protected class VRage_Game_ToolSound\u003C\u003Etype\u003C\u003EAccessor : IMemberAccessor<ToolSound, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolSound owner, in string value) => owner.type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolSound owner, out string value) => value = owner.type;
    }

    protected class VRage_Game_ToolSound\u003C\u003Esubtype\u003C\u003EAccessor : IMemberAccessor<ToolSound, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolSound owner, in string value) => owner.subtype = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolSound owner, out string value) => value = owner.subtype;
    }

    protected class VRage_Game_ToolSound\u003C\u003Esound\u003C\u003EAccessor : IMemberAccessor<ToolSound, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolSound owner, in string value) => owner.sound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolSound owner, out string value) => value = owner.sound;
    }

    private class VRage_Game_ToolSound\u003C\u003EActor : IActivator, IActivator<ToolSound>
    {
      object IActivator.CreateInstance() => (object) new ToolSound();

      ToolSound IActivator<ToolSound>.CreateInstance() => new ToolSound();
    }
  }
}
