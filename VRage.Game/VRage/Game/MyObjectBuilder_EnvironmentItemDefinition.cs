// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EnvironmentItemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EnvironmentItemDefinition : MyObjectBuilder_PhysicalModelDefinition
  {
    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_EnvironmentItemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentItemDefinition();

      MyObjectBuilder_EnvironmentItemDefinition IActivator<MyObjectBuilder_EnvironmentItemDefinition>.CreateInstance() => new MyObjectBuilder_EnvironmentItemDefinition();
    }
  }
}
