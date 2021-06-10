// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_TriggerBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TriggerBase : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1)]
    public int Type;
    [ProtoMember(4)]
    public SerializableBoundingBoxD AABB;
    [ProtoMember(7)]
    public SerializableBoundingSphereD BoundingSphere;
    [ProtoMember(10)]
    public SerializableVector3D Offset = (SerializableVector3D) Vector3D.Zero;
    [ProtoMember(15)]
    public SerializableOrientedBoundingBoxD OrientedBoundingBox;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in int value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out int value) => value = owner.Type;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EAABB\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBase, SerializableBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerBase owner,
        in SerializableBoundingBoxD value)
      {
        owner.AABB = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerBase owner,
        out SerializableBoundingBoxD value)
      {
        value = owner.AABB;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EBoundingSphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBase, SerializableBoundingSphereD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerBase owner,
        in SerializableBoundingSphereD value)
      {
        owner.BoundingSphere = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerBase owner,
        out SerializableBoundingSphereD value)
      {
        value = owner.BoundingSphere;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBase, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in SerializableVector3D value) => owner.Offset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out SerializableVector3D value) => value = owner.Offset;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOrientedBoundingBox\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBase, SerializableOrientedBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerBase owner,
        in SerializableOrientedBoundingBoxD value)
      {
        owner.OrientedBoundingBox = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerBase owner,
        out SerializableOrientedBoundingBoxD value)
      {
        value = owner.OrientedBoundingBox;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TriggerBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TriggerBase();

      MyObjectBuilder_TriggerBase IActivator<MyObjectBuilder_TriggerBase>.CreateInstance() => new MyObjectBuilder_TriggerBase();
    }
  }
}
