// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_PhysicsBodyComponentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_PhysicsBodyComponentDefinition : MyObjectBuilder_PhysicsComponentDefinitionBase
  {
    [ProtoMember(1)]
    public bool CreateFromCollisionObject;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ECreateFromCollisionObject\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        owner.CreateFromCollisionObject = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        value = owner.CreateFromCollisionObject;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EMassPropertiesComputation\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EMassPropertiesComputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ERigidBodyFlags\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ERigidBodyFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, RigidBodyFlag>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in RigidBodyFlag value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out RigidBodyFlag value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ECollisionLayer\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ECollisionLayer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ELinearDamping\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ELinearDamping\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in float? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out float? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EAngularDamping\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EAngularDamping\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in float? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out float? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EForceActivate\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EForceActivate\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EUpdateFlags\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003EUpdateFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ESerialize\u003C\u003EAccessor : MyObjectBuilder_PhysicsComponentDefinitionBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsComponentDefinitionBase\u003C\u003ESerialize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicsComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EComponentType\u003C\u003EAccessor : MyObjectBuilder_ComponentDefinitionBase.VRage_Game_MyObjectBuilder_ComponentDefinitionBase\u003C\u003EComponentType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicsBodyComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PhysicsBodyComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_PhysicsBodyComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PhysicsBodyComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PhysicsBodyComponentDefinition();

      MyObjectBuilder_PhysicsBodyComponentDefinition IActivator<MyObjectBuilder_PhysicsBodyComponentDefinition>.CreateInstance() => new MyObjectBuilder_PhysicsBodyComponentDefinition();
    }
  }
}
