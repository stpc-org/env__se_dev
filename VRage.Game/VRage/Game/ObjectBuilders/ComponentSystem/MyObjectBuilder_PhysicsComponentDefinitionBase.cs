// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_PhysicsComponentDefinitionBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_PhysicsComponentDefinitionBase : MyObjectBuilder_ComponentDefinitionBase
  {
    [ProtoMember(1)]
    [DefaultValue(MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType.None)]
    public MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType MassPropertiesComputation;
    [ProtoMember(4)]
    [DefaultValue(RigidBodyFlag.RBF_DEFAULT)]
    public RigidBodyFlag RigidBodyFlags;
    [ProtoMember(7)]
    [DefaultValue(null)]
    public string CollisionLayer;
    [ProtoMember(10)]
    [DefaultValue(null)]
    public float? LinearDamping;
    [ProtoMember(13)]
    [DefaultValue(null)]
    public float? AngularDamping;
    [ProtoMember(16)]
    public bool ForceActivate;
    [ProtoMember(19)]
    public MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags UpdateFlags;
    [ProtoMember(22)]
    public bool Serialize;

    public enum MyMassPropertiesComputationType
    {
      None,
      Box,
      Sphere,
      Capsule,
      Cylinder,
    }

    [Flags]
    public enum MyUpdateFlags
    {
      Gravity = 1,
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EMassPropertiesComputation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType value)
      {
        owner.MassPropertiesComputation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType value)
      {
        value = owner.MassPropertiesComputation;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ERigidBodyFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, RigidBodyFlag>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in RigidBodyFlag value)
      {
        owner.RigidBodyFlags = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out RigidBodyFlag value)
      {
        value = owner.RigidBodyFlags;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ECollisionLayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        owner.CollisionLayer = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        value = owner.CollisionLayer;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ELinearDamping\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in float? value)
      {
        owner.LinearDamping = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out float? value)
      {
        value = owner.LinearDamping;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EAngularDamping\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in float? value)
      {
        owner.AngularDamping = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out float? value)
      {
        value = owner.AngularDamping;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EForceActivate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in bool value)
      {
        owner.ForceActivate = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out bool value)
      {
        value = owner.ForceActivate;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EUpdateFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags value)
      {
        owner.UpdateFlags = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags value)
      {
        value = owner.UpdateFlags;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ESerialize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in bool value)
      {
        owner.Serialize = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out bool value)
      {
        value = owner.Serialize;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EComponentType\u003C\u003EAccessor : MyObjectBuilder_ComponentDefinitionBase.VRage_Game_MyObjectBuilder_ComponentDefinitionBase\u003C\u003EComponentType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsComponentDefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsComponentDefinitionBase owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PhysicsComponentDefinitionBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PhysicsComponentDefinitionBase();

      MyObjectBuilder_PhysicsComponentDefinitionBase IActivator<MyObjectBuilder_PhysicsComponentDefinitionBase>.CreateInstance() => new MyObjectBuilder_PhysicsComponentDefinitionBase();
    }
  }
}
