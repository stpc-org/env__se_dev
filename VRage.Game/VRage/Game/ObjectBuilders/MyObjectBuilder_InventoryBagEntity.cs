// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_InventoryBagEntity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InventoryBagEntity : MyObjectBuilder_EntityBase
  {
    [ProtoMember(1)]
    public SerializableVector3 LinearVelocity;
    [ProtoMember(4)]
    public SerializableVector3 AngularVelocity;
    [ProtoMember(7)]
    public float Mass = 5f;
    [ProtoMember(10)]
    public long OwnerIdentityId;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003ELinearVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryBagEntity, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in SerializableVector3 value)
      {
        owner.LinearVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out SerializableVector3 value)
      {
        value = owner.LinearVelocity;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EAngularVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryBagEntity, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in SerializableVector3 value)
      {
        owner.AngularVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out SerializableVector3 value)
      {
        value = owner.AngularVelocity;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryBagEntity, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in float value) => owner.Mass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out float value) => value = owner.Mass;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EOwnerIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryBagEntity, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in long value) => owner.OwnerIdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out long value) => value = owner.OwnerIdentityId;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryBagEntity owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryBagEntity owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryBagEntity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryBagEntity owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryBagEntity owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_InventoryBagEntity\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryBagEntity>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryBagEntity();

      MyObjectBuilder_InventoryBagEntity IActivator<MyObjectBuilder_InventoryBagEntity>.CreateInstance() => new MyObjectBuilder_InventoryBagEntity();
    }
  }
}
