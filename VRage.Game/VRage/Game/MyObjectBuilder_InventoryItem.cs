// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_InventoryItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InventoryItem : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [XmlElement("Amount")]
    public MyFixedPoint Amount = (MyFixedPoint) 1;
    [ProtoMember(4)]
    [XmlElement("Scale")]
    public float Scale = 1f;
    [ProtoMember(7)]
    [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_PhysicalObject>))]
    [DynamicObjectBuilder(false)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_PhysicalObject Content;
    [ProtoMember(10)]
    [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_PhysicalObject>))]
    [DynamicObjectBuilder(false)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_PhysicalObject PhysicalContent;
    [ProtoMember(13)]
    public uint ItemId;

    public bool ShouldSerializeScale() => (double) this.Scale != 1.0;

    [XmlElement("AmountDecimal")]
    [NoSerialize]
    public Decimal Obsolete_AmountDecimal
    {
      get => (Decimal) this.Amount;
      set => this.Amount = (MyFixedPoint) value;
    }

    public bool ShouldSerializeObsolete_AmountDecimal() => false;

    public bool ShouldSerializeContent() => false;

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in MyFixedPoint value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out MyFixedPoint value) => value = owner.Amount;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out float value) => value = owner.Scale;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EContent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyObjectBuilder_PhysicalObject>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryItem owner,
        in MyObjectBuilder_PhysicalObject value)
      {
        owner.Content = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryItem owner,
        out MyObjectBuilder_PhysicalObject value)
      {
        value = owner.Content;
      }
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EPhysicalContent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, MyObjectBuilder_PhysicalObject>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryItem owner,
        in MyObjectBuilder_PhysicalObject value)
      {
        owner.PhysicalContent = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryItem owner,
        out MyObjectBuilder_PhysicalObject value)
      {
        value = owner.PhysicalContent;
      }
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in uint value) => owner.ItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out uint value) => value = owner.ItemId;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EObsolete_AmountDecimal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryItem, Decimal>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in Decimal value) => owner.Obsolete_AmountDecimal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out Decimal value) => value = owner.Obsolete_AmountDecimal;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_InventoryItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryItem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryItem();

      MyObjectBuilder_InventoryItem IActivator<MyObjectBuilder_InventoryItem>.CreateInstance() => new MyObjectBuilder_InventoryItem();
    }
  }
}
