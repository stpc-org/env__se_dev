// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_UpdateTrigger
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
  public class MyObjectBuilder_UpdateTrigger : MyObjectBuilder_TriggerBase
  {
    [ProtoMember(1)]
    public int Size = 25000;
    [ProtoMember(2)]
    public bool IsPirateStation;
    [Nullable]
    [ProtoMember(3)]
    public MyObjectBuilder_CubeGrid SerializedPirateStation;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value) => value = owner.Size;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EIsPirateStation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in bool value) => owner.IsPirateStation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out bool value) => value = owner.IsPirateStation;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003ESerializedPirateStation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_CubeGrid>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_UpdateTrigger owner,
        in MyObjectBuilder_CubeGrid value)
      {
        owner.SerializedPirateStation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_UpdateTrigger owner,
        out MyObjectBuilder_CubeGrid value)
      {
        value = owner.SerializedPirateStation;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EType\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value) => this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value) => this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EAABB\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EAABB\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_UpdateTrigger owner,
        in SerializableBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_UpdateTrigger owner,
        out SerializableBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EBoundingSphere\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EBoundingSphere\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingSphereD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_UpdateTrigger owner,
        in SerializableBoundingSphereD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_UpdateTrigger owner,
        out SerializableBoundingSphereD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EOffset\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_UpdateTrigger owner,
        in SerializableVector3D value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_UpdateTrigger owner,
        out SerializableVector3D value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EOrientedBoundingBox\u003C\u003EAccessor : MyObjectBuilder_TriggerBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase\u003C\u003EOrientedBoundingBox\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableOrientedBoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_UpdateTrigger owner,
        in SerializableOrientedBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_TriggerBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_UpdateTrigger owner,
        out SerializableOrientedBoundingBoxD value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_TriggerBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_UpdateTrigger>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_UpdateTrigger();

      MyObjectBuilder_UpdateTrigger IActivator<MyObjectBuilder_UpdateTrigger>.CreateInstance() => new MyObjectBuilder_UpdateTrigger();
    }
  }
}
