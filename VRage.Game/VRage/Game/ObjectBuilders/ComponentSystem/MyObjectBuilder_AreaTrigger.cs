// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_AreaTrigger
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

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AreaTrigger : MyObjectBuilder_TriggerBase
  {
    [ProtoMember(1)]
    public string Name;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EType\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in int value) => this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out int value) => this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EAABB\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EAABB\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaTrigger owner,
        in SerializableBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaTrigger owner,
        out SerializableBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EBoundingSphere\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EBoundingSphere\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableBoundingSphereD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaTrigger owner,
        in SerializableBoundingSphereD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaTrigger owner,
        out SerializableBoundingSphereD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EOffset\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in SerializableVector3D value) => this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out SerializableVector3D value) => this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EOrientedBoundingBox\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOrientedBoundingBox\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, SerializableOrientedBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AreaTrigger owner,
        in SerializableOrientedBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AreaTrigger owner,
        out SerializableOrientedBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AreaTrigger, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AreaTrigger owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AreaTrigger owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_AreaTrigger\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AreaTrigger>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AreaTrigger();

      MyObjectBuilder_AreaTrigger IActivator<MyObjectBuilder_AreaTrigger>.CreateInstance() => new MyObjectBuilder_AreaTrigger();
    }
  }
}
