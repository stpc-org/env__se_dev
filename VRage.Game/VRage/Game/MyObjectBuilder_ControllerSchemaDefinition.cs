// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ControllerSchemaDefinition
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ControllerSchemaDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlArrayItem("DeviceId")]
    [ProtoMember(25)]
    public List<string> CompatibleDeviceIds;
    [ProtoMember(28)]
    public List<MyObjectBuilder_ControllerSchemaDefinition.Schema> Schemas;

    [ProtoContract]
    public class ControlDef
    {
      [XmlAttribute]
      [ProtoMember(1)]
      public string Type;
      [XmlAttribute]
      [ProtoMember(4)]
      public MyControllerSchemaEnum Control;

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlDef\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.ControlDef, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlDef owner,
          in string value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlDef owner,
          out string value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlDef\u003C\u003EControl\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.ControlDef, MyControllerSchemaEnum>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlDef owner,
          in MyControllerSchemaEnum value)
        {
          owner.Control = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlDef owner,
          out MyControllerSchemaEnum value)
        {
          value = owner.Control;
        }
      }

      private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlDef\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition.ControlDef>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ControllerSchemaDefinition.ControlDef();

        MyObjectBuilder_ControllerSchemaDefinition.ControlDef IActivator<MyObjectBuilder_ControllerSchemaDefinition.ControlDef>.CreateInstance() => new MyObjectBuilder_ControllerSchemaDefinition.ControlDef();
      }
    }

    [ProtoContract]
    public class ControlGroup
    {
      [ProtoMember(7)]
      public string Type;
      [ProtoMember(10)]
      public string Name;
      [ProtoMember(13)]
      public List<MyObjectBuilder_ControllerSchemaDefinition.ControlDef> ControlDefs;

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlGroup\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          in string value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          out string value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlGroup\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlGroup\u003C\u003EControlDefs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup, List<MyObjectBuilder_ControllerSchemaDefinition.ControlDef>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          in List<MyObjectBuilder_ControllerSchemaDefinition.ControlDef> value)
        {
          owner.ControlDefs = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.ControlGroup owner,
          out List<MyObjectBuilder_ControllerSchemaDefinition.ControlDef> value)
        {
          value = owner.ControlDefs;
        }
      }

      private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EControlGroup\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ControllerSchemaDefinition.ControlGroup();

        MyObjectBuilder_ControllerSchemaDefinition.ControlGroup IActivator<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup>.CreateInstance() => new MyObjectBuilder_ControllerSchemaDefinition.ControlGroup();
      }
    }

    [ProtoContract]
    public class CompatibleDevice
    {
      [ProtoMember(16)]
      public string DeviceId;

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ECompatibleDevice\u003C\u003EDeviceId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice owner,
          in string value)
        {
          owner.DeviceId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice owner,
          out string value)
        {
          value = owner.DeviceId;
        }
      }

      private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ECompatibleDevice\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice();

        MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice IActivator<MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice>.CreateInstance() => new MyObjectBuilder_ControllerSchemaDefinition.CompatibleDevice();
      }
    }

    [ProtoContract]
    public class Schema
    {
      [ProtoMember(19)]
      public string SchemaName;
      [ProtoMember(22)]
      public List<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup> ControlGroups;

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ESchema\u003C\u003ESchemaName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.Schema, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.Schema owner,
          in string value)
        {
          owner.SchemaName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.Schema owner,
          out string value)
        {
          value = owner.SchemaName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ESchema\u003C\u003EControlGroups\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition.Schema, List<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ControllerSchemaDefinition.Schema owner,
          in List<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup> value)
        {
          owner.ControlGroups = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ControllerSchemaDefinition.Schema owner,
          out List<MyObjectBuilder_ControllerSchemaDefinition.ControlGroup> value)
        {
          value = owner.ControlGroups;
        }
      }

      private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ESchema\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition.Schema>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ControllerSchemaDefinition.Schema();

        MyObjectBuilder_ControllerSchemaDefinition.Schema IActivator<MyObjectBuilder_ControllerSchemaDefinition.Schema>.CreateInstance() => new MyObjectBuilder_ControllerSchemaDefinition.Schema();
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ECompatibleDeviceIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in List<string> value)
      {
        owner.CompatibleDeviceIds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out List<string> value)
      {
        value = owner.CompatibleDeviceIds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ESchemas\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, List<MyObjectBuilder_ControllerSchemaDefinition.Schema>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in List<MyObjectBuilder_ControllerSchemaDefinition.Schema> value)
      {
        owner.Schemas = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out List<MyObjectBuilder_ControllerSchemaDefinition.Schema> value)
      {
        value = owner.Schemas;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ControllerSchemaDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ControllerSchemaDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_ControllerSchemaDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ControllerSchemaDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ControllerSchemaDefinition();

      MyObjectBuilder_ControllerSchemaDefinition IActivator<MyObjectBuilder_ControllerSchemaDefinition>.CreateInstance() => new MyObjectBuilder_ControllerSchemaDefinition();
    }
  }
}
