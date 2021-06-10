// Decompiled with JetBrains decompiler
// Type: VRage.Game.EnvironmentItemsEntry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class EnvironmentItemsEntry
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Type;
    [ProtoMember(4)]
    [XmlAttribute]
    public string Subtype;
    [ProtoMember(7)]
    [XmlAttribute]
    public string ItemSubtype;
    [ProtoMember(10)]
    [DefaultValue(true)]
    public bool Enabled = true;
    [ProtoMember(13)]
    [XmlAttribute]
    public float Frequency = 1f;

    public override bool Equals(object other) => other is EnvironmentItemsEntry environmentItemsEntry && environmentItemsEntry.Type.Equals(this.Type) && environmentItemsEntry.Subtype.Equals(this.Subtype) && environmentItemsEntry.ItemSubtype.Equals(this.ItemSubtype);

    public override int GetHashCode() => this.Type.GetHashCode() * 1572869 ^ this.Subtype.GetHashCode() * 49157 ^ this.ItemSubtype.GetHashCode();

    protected class VRage_Game_EnvironmentItemsEntry\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EnvironmentItemsEntry owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EnvironmentItemsEntry owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_EnvironmentItemsEntry\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EnvironmentItemsEntry owner, in string value) => owner.Subtype = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EnvironmentItemsEntry owner, out string value) => value = owner.Subtype;
    }

    protected class VRage_Game_EnvironmentItemsEntry\u003C\u003EItemSubtype\u003C\u003EAccessor : IMemberAccessor<EnvironmentItemsEntry, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EnvironmentItemsEntry owner, in string value) => owner.ItemSubtype = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EnvironmentItemsEntry owner, out string value) => value = owner.ItemSubtype;
    }

    protected class VRage_Game_EnvironmentItemsEntry\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<EnvironmentItemsEntry, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EnvironmentItemsEntry owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EnvironmentItemsEntry owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_EnvironmentItemsEntry\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<EnvironmentItemsEntry, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EnvironmentItemsEntry owner, in float value) => owner.Frequency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EnvironmentItemsEntry owner, out float value) => value = owner.Frequency;
    }

    private class VRage_Game_EnvironmentItemsEntry\u003C\u003EActor : IActivator, IActivator<EnvironmentItemsEntry>
    {
      object IActivator.CreateInstance() => (object) new EnvironmentItemsEntry();

      EnvironmentItemsEntry IActivator<EnvironmentItemsEntry>.CreateInstance() => new EnvironmentItemsEntry();
    }
  }
}
