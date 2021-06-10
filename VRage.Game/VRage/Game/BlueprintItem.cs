// Decompiled with JetBrains decompiler
// Type: VRage.Game.BlueprintItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class BlueprintItem
  {
    [XmlIgnore]
    [ProtoMember(1)]
    public SerializableDefinitionId Id;
    [XmlAttribute]
    [ProtoMember(4)]
    public string Amount;

    [XmlAttribute]
    public string TypeId
    {
      get => this.Id.TypeId.IsNull ? "(null)" : this.Id.TypeId.ToString();
      set => this.Id.TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
    }

    [XmlAttribute]
    public string SubtypeId
    {
      get => this.Id.SubtypeId;
      set => this.Id.SubtypeId = value;
    }

    protected class VRage_Game_BlueprintItem\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<BlueprintItem, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintItem owner, in SerializableDefinitionId value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintItem owner, out SerializableDefinitionId value) => value = owner.Id;
    }

    protected class VRage_Game_BlueprintItem\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<BlueprintItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintItem owner, in string value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintItem owner, out string value) => value = owner.Amount;
    }

    protected class VRage_Game_BlueprintItem\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<BlueprintItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintItem owner, in string value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintItem owner, out string value) => value = owner.TypeId;
    }

    protected class VRage_Game_BlueprintItem\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<BlueprintItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BlueprintItem owner, in string value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BlueprintItem owner, out string value) => value = owner.SubtypeId;
    }

    private class VRage_Game_BlueprintItem\u003C\u003EActor : IActivator, IActivator<BlueprintItem>
    {
      object IActivator.CreateInstance() => (object) new BlueprintItem();

      BlueprintItem IActivator<BlueprintItem>.CreateInstance() => new BlueprintItem();
    }
  }
}
