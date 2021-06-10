// Decompiled with JetBrains decompiler
// Type: VRage.Game.BlueprintClassEntry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class BlueprintClassEntry
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Class;
    [XmlIgnore]
    public MyObjectBuilderType TypeId;
    [ProtoMember(7)]
    [XmlAttribute]
    public string BlueprintSubtypeId;
    [ProtoMember(10)]
    [DefaultValue(true)]
    public bool Enabled = true;

    [ProtoMember(4)]
    [XmlAttribute]
    public string BlueprintTypeId
    {
      get => !this.TypeId.IsNull ? this.TypeId.ToString() : "(null)";
      set => this.TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
    }

    public override bool Equals(object other) => other is BlueprintClassEntry blueprintClassEntry && blueprintClassEntry.Class.Equals(this.Class) && blueprintClassEntry.BlueprintSubtypeId.Equals(this.BlueprintSubtypeId);

    public override int GetHashCode() => this.Class.GetHashCode() * 7607 + this.BlueprintSubtypeId.GetHashCode();

    protected class VRage_Game_BlueprintClassEntry\u003C\u003EClass\u003C\u003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintClassEntry owner, in string value) => owner.Class = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintClassEntry owner, out string value) => value = owner.Class;
    }

    protected class VRage_Game_BlueprintClassEntry\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<BlueprintClassEntry, MyObjectBuilderType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintClassEntry owner, in MyObjectBuilderType value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintClassEntry owner, out MyObjectBuilderType value) => value = owner.TypeId;
    }

    protected class VRage_Game_BlueprintClassEntry\u003C\u003EBlueprintSubtypeId\u003C\u003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintClassEntry owner, in string value) => owner.BlueprintSubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintClassEntry owner, out string value) => value = owner.BlueprintSubtypeId;
    }

    protected class VRage_Game_BlueprintClassEntry\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<BlueprintClassEntry, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintClassEntry owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintClassEntry owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_BlueprintClassEntry\u003C\u003EBlueprintTypeId\u003C\u003EAccessor : IMemberAccessor<BlueprintClassEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintClassEntry owner, in string value) => owner.BlueprintTypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintClassEntry owner, out string value) => value = owner.BlueprintTypeId;
    }

    private class VRage_Game_BlueprintClassEntry\u003C\u003EActor : IActivator, IActivator<BlueprintClassEntry>
    {
      object IActivator.CreateInstance() => (object) new BlueprintClassEntry();

      BlueprintClassEntry IActivator<BlueprintClassEntry>.CreateInstance() => new BlueprintClassEntry();
    }
  }
}
