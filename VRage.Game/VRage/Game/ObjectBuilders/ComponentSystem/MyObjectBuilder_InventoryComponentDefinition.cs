// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_InventoryComponentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InventoryComponentDefinition : MyObjectBuilder_ComponentDefinitionBase
  {
    [ProtoMember(13)]
    public SerializableVector3? Size;
    [ProtoMember(16)]
    public float Volume = float.MaxValue;
    [ProtoMember(19)]
    public float Mass = float.MaxValue;
    [ProtoMember(22)]
    public bool RemoveEntityOnEmpty;
    [ProtoMember(25)]
    public bool MultiplierEnabled = true;
    [ProtoMember(28)]
    public int MaxItemCount = int.MaxValue;
    [ProtoMember(31)]
    [DefaultValue(null)]
    public MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition InputConstraint;

    [ProtoContract]
    public class InventoryConstraintDefinition
    {
      [XmlAttribute("Description")]
      [DefaultValue(null)]
      [ProtoMember(1)]
      public string Description;
      [XmlAttribute("Icon")]
      [DefaultValue(null)]
      [ProtoMember(4)]
      [ModdableContentFile(new string[] {"dds", "png"})]
      public string Icon;
      [XmlAttribute("Whitelist")]
      [ProtoMember(7)]
      public bool IsWhitelist = true;
      [XmlElement("Entry")]
      [ProtoMember(10)]
      public List<SerializableDefinitionId> Entries = new List<SerializableDefinitionId>();

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInventoryConstraintDefinition\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          in string value)
        {
          owner.Description = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          out string value)
        {
          value = owner.Description;
        }
      }

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInventoryConstraintDefinition\u003C\u003EIcon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          in string value)
        {
          owner.Icon = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          out string value)
        {
          value = owner.Icon;
        }
      }

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInventoryConstraintDefinition\u003C\u003EIsWhitelist\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          in bool value)
        {
          owner.IsWhitelist = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          out bool value)
        {
          value = owner.IsWhitelist;
        }
      }

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInventoryConstraintDefinition\u003C\u003EEntries\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition, List<SerializableDefinitionId>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          in List<SerializableDefinitionId> value)
        {
          owner.Entries = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition owner,
          out List<SerializableDefinitionId> value)
        {
          value = owner.Entries;
        }
      }

      private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInventoryConstraintDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition();

        MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition IActivator<MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition>.CreateInstance() => new MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition();
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in SerializableVector3? value)
      {
        owner.Size = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out SerializableVector3? value)
      {
        value = owner.Size;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in float value)
      {
        owner.Volume = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out float value)
      {
        value = owner.Volume;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in float value)
      {
        owner.Mass = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out float value)
      {
        value = owner.Mass;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003ERemoveEntityOnEmpty\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in bool value)
      {
        owner.RemoveEntityOnEmpty = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out bool value)
      {
        value = owner.RemoveEntityOnEmpty;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EMultiplierEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in bool value)
      {
        owner.MultiplierEnabled = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out bool value)
      {
        value = owner.MultiplierEnabled;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EMaxItemCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in int value)
      {
        owner.MaxItemCount = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out int value)
      {
        value = owner.MaxItemCount;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EInputConstraint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition value)
      {
        owner.InputConstraint = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition value)
      {
        value = owner.InputConstraint;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EComponentType\u003C\u003EAccessor : MyObjectBuilder_ComponentDefinitionBase.VRage_Game_MyObjectBuilder_ComponentDefinitionBase\u003C\u003EComponentType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ComponentDefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ComponentDefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InventoryComponentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InventoryComponentDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryComponentDefinition();

      MyObjectBuilder_InventoryComponentDefinition IActivator<MyObjectBuilder_InventoryComponentDefinition>.CreateInstance() => new MyObjectBuilder_InventoryComponentDefinition();
    }
  }
}
