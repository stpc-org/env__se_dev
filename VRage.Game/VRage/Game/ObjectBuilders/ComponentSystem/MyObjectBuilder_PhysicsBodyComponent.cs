// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_PhysicsBodyComponent
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
  public class MyObjectBuilder_PhysicsBodyComponent : MyObjectBuilder_PhysicsComponentBase
  {
    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003ELinearVelocity\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003ELinearVelocity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003EAngularVelocity\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentBase\u003C\u003EAngularVelocity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsBodyComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsBodyComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicsBodyComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsBodyComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicsBodyComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicsBodyComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PhysicsBodyComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PhysicsBodyComponent();

      MyObjectBuilder_PhysicsBodyComponent IActivator<MyObjectBuilder_PhysicsBodyComponent>.CreateInstance() => new MyObjectBuilder_PhysicsBodyComponent();
    }
  }
}
