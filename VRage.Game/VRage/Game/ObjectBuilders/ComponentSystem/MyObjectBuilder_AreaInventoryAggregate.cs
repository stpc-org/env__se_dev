// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_AreaInventoryAggregate
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AreaInventoryAggregate : MyObjectBuilder_InventoryAggregate
  {
    [ProtoMember(1)]
    public float Radius;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out float value) => value = owner.Radius;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003EInventories\u003C\u003EAccessor : MyObjectBuilder_InventoryAggregate.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryAggregate\u003C\u003EInventories\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, List<MyObjectBuilder_InventoryBase>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        in List<MyObjectBuilder_InventoryBase> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_InventoryAggregate&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        out List<MyObjectBuilder_InventoryBase> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_InventoryAggregate&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003EInventoryId\u003C\u003EAccessor : MyObjectBuilder_InventoryBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryBase\u003C\u003EInventoryId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_InventoryBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_InventoryBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaInventoryAggregate owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaInventoryAggregate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaInventoryAggregate owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaInventoryAggregate owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaInventoryAggregate\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AreaInventoryAggregate>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AreaInventoryAggregate();

      MyObjectBuilder_AreaInventoryAggregate IActivator<MyObjectBuilder_AreaInventoryAggregate>.CreateInstance() => new MyObjectBuilder_AreaInventoryAggregate();
    }
  }
}
