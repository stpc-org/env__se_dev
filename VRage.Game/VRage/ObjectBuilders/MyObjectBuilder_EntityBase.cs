// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectBuilder_EntityBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EntityBase : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public long EntityId;
    [ProtoMember(4)]
    public MyPersistentEntityFlags2 PersistentFlags;
    [ProtoMember(7)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Name;
    [ProtoMember(10)]
    public MyPositionAndOrientation? PositionAndOrientation;
    [ProtoMember(11)]
    public MyPositionAndOrientation? LocalPositionAndOrientation;
    [ProtoMember(13)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_ComponentContainer ComponentContainer;
    [ProtoMember(16)]
    [DefaultValue(null)]
    [NoSerialize]
    public SerializableDefinitionId? EntityDefinitionId;

    public bool ShouldSerializePositionAndOrientation() => this.PositionAndOrientation.HasValue;

    public bool ShouldSerializeLocalPositionAndOrientation() => this.PositionAndOrientation.HasValue;

    public bool ShouldSerializeComponentContainer() => this.ComponentContainer != null && this.ComponentContainer.Components != null && this.ComponentContainer.Components.Count > 0;

    public bool ShouldSerializeEntityDefinitionId() => false;

    public virtual void Remap(IMyRemapHelper remapHelper)
    {
      this.EntityId = remapHelper.RemapEntityId(this.EntityId);
      this.Name = remapHelper.RemapEntityName(this.EntityId);
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out long value) => value = owner.EntityId;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityBase owner,
        in MyPersistentEntityFlags2 value)
      {
        owner.PersistentFlags = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityBase owner,
        out MyPersistentEntityFlags2 value)
      {
        value = owner.PersistentFlags;
      }
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out string value) => value = owner.Name;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityBase owner,
        in MyPositionAndOrientation? value)
      {
        owner.PositionAndOrientation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityBase owner,
        out MyPositionAndOrientation? value)
      {
        value = owner.PositionAndOrientation;
      }
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityBase owner,
        in MyPositionAndOrientation? value)
      {
        owner.LocalPositionAndOrientation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityBase owner,
        out MyPositionAndOrientation? value)
      {
        value = owner.LocalPositionAndOrientation;
      }
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityBase owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        owner.ComponentContainer = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityBase owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        value = owner.ComponentContainer;
      }
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityBase owner,
        in SerializableDefinitionId? value)
      {
        owner.EntityDefinitionId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityBase owner,
        out SerializableDefinitionId? value)
      {
        value = owner.EntityDefinitionId;
      }
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityBase();

      MyObjectBuilder_EntityBase IActivator<MyObjectBuilder_EntityBase>.CreateInstance() => new MyObjectBuilder_EntityBase();
    }
  }
}
