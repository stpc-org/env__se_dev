// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_InventoryAggregate
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InventoryAggregate : MyObjectBuilder_InventoryBase
  {
    [ProtoMember(1)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    [DynamicNullableObjectBuilderItem(false)]
    [XmlArrayItem("MyObjectBuilder_InventoryBase", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_InventoryBase>))]
    public List<MyObjectBuilder_InventoryBase> Inventories;

    public override void Clear()
    {
      foreach (MyObjectBuilder_InventoryBase inventory in this.Inventories)
        inventory.Clear();
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003EInventories\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryAggregate, List<MyObjectBuilder_InventoryBase>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryAggregate owner,
        in List<MyObjectBuilder_InventoryBase> value)
      {
        owner.Inventories = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryAggregate owner,
        out List<MyObjectBuilder_InventoryBase> value)
      {
        value = owner.Inventories;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003EInventoryId\u003C\u003EAccessor : MyObjectBuilder_InventoryBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryBase\u003C\u003EInventoryId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_InventoryBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_InventoryBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryAggregate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryAggregate owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryAggregate owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryAggregate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryAggregate owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryAggregate owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryAggregate>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryAggregate();

      MyObjectBuilder_InventoryAggregate IActivator<MyObjectBuilder_InventoryAggregate>.CreateInstance() => new MyObjectBuilder_InventoryAggregate();
    }
  }
}
