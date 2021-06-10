// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CompoundBlockTemplateDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
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
  public class MyObjectBuilder_CompoundBlockTemplateDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlArrayItem("Binding")]
    [ProtoMember(16)]
    public MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding[] Bindings;

    [ProtoContract]
    public class CompoundBlockRotationBinding
    {
      [XmlAttribute]
      [ProtoMember(1)]
      public string BuildTypeReference;
      [XmlArrayItem("Rotation")]
      [ProtoMember(4)]
      public SerializableBlockOrientation[] Rotations;

      protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockRotationBinding\u003C\u003EBuildTypeReference\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding owner,
          in string value)
        {
          owner.BuildTypeReference = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding owner,
          out string value)
        {
          value = owner.BuildTypeReference;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockRotationBinding\u003C\u003ERotations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding, SerializableBlockOrientation[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding owner,
          in SerializableBlockOrientation[] value)
        {
          owner.Rotations = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding owner,
          out SerializableBlockOrientation[] value)
        {
          value = owner.Rotations;
        }
      }

      private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockRotationBinding\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding();

        MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding>.CreateInstance() => new MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding();
      }
    }

    [ProtoContract]
    public class CompoundBlockBinding
    {
      [XmlAttribute]
      [ProtoMember(7)]
      public string BuildType;
      [XmlAttribute]
      [ProtoMember(10)]
      [DefaultValue(false)]
      public bool Multiple;
      [XmlArrayItem("RotationBind")]
      [ProtoMember(13)]
      public MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding[] RotationBinds;

      protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockBinding\u003C\u003EBuildType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          in string value)
        {
          owner.BuildType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          out string value)
        {
          value = owner.BuildType;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockBinding\u003C\u003EMultiple\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          in bool value)
        {
          owner.Multiple = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          out bool value)
        {
          value = owner.Multiple;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockBinding\u003C\u003ERotationBinds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding, MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          in MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding[] value)
        {
          owner.RotationBinds = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding owner,
          out MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockRotationBinding[] value)
        {
          value = owner.RotationBinds;
        }
      }

      private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ECompoundBlockBinding\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding();

        MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding>.CreateInstance() => new MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding();
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EBindings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding[] value)
      {
        owner.Bindings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out MyObjectBuilder_CompoundBlockTemplateDefinition.CompoundBlockBinding[] value)
      {
        value = owner.Bindings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundBlockTemplateDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundBlockTemplateDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_CompoundBlockTemplateDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CompoundBlockTemplateDefinition();

      MyObjectBuilder_CompoundBlockTemplateDefinition IActivator<MyObjectBuilder_CompoundBlockTemplateDefinition>.CreateInstance() => new MyObjectBuilder_CompoundBlockTemplateDefinition();
    }
  }
}
