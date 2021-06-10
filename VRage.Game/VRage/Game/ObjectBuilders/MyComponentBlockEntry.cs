// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyComponentBlockEntry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  public class MyComponentBlockEntry
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Type;
    [ProtoMember(4)]
    [XmlAttribute]
    public string Subtype;
    [ProtoMember(7)]
    [XmlAttribute]
    public bool Main = true;
    [ProtoMember(10)]
    [DefaultValue(true)]
    [XmlAttribute]
    public bool Enabled = true;

    protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyComponentBlockEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyComponentBlockEntry owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyComponentBlockEntry owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyComponentBlockEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyComponentBlockEntry owner, in string value) => owner.Subtype = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyComponentBlockEntry owner, out string value) => value = owner.Subtype;
    }

    protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry\u003C\u003EMain\u003C\u003EAccessor : IMemberAccessor<MyComponentBlockEntry, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyComponentBlockEntry owner, in bool value) => owner.Main = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyComponentBlockEntry owner, out bool value) => value = owner.Main;
    }

    protected class VRage_Game_ObjectBuilders_MyComponentBlockEntry\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyComponentBlockEntry, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyComponentBlockEntry owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyComponentBlockEntry owner, out bool value) => value = owner.Enabled;
    }

    private class VRage_Game_ObjectBuilders_MyComponentBlockEntry\u003C\u003EActor : IActivator, IActivator<MyComponentBlockEntry>
    {
      object IActivator.CreateInstance() => (object) new MyComponentBlockEntry();

      MyComponentBlockEntry IActivator<MyComponentBlockEntry>.CreateInstance() => new MyComponentBlockEntry();
    }
  }
}
