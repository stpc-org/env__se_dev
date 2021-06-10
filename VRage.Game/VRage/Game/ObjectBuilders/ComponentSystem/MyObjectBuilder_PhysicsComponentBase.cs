// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_PhysicsComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public abstract class MyObjectBuilder_PhysicsComponentBase : MyObjectBuilder_ComponentBase
  {
    public SerializableVector3 LinearVelocity;
    public SerializableVector3 AngularVelocity;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003ELinearVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        in SerializableVector3 value)
      {
        owner.LinearVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        out SerializableVector3 value)
      {
        value = owner.LinearVelocity;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003EAngularVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        in SerializableVector3 value)
      {
        owner.AngularVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        out SerializableVector3 value)
      {
        value = owner.AngularVelocity;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsComponentBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsComponentBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicsComponentBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsComponentBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsComponentBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicsComponentBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }
  }
}
