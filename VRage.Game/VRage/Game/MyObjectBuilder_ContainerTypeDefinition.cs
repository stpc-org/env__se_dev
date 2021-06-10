// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ContainerTypeDefinition
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
  public class MyObjectBuilder_ContainerTypeDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlAttribute]
    [ProtoMember(13)]
    public int CountMin;
    [XmlAttribute]
    [ProtoMember(16)]
    public int CountMax;
    [XmlArrayItem("Item")]
    [ProtoMember(19)]
    public MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem[] Items;

    [ProtoContract]
    public class ContainerTypeItem
    {
      [XmlAttribute]
      [ProtoMember(1)]
      public string AmountMin;
      [XmlAttribute]
      [ProtoMember(4)]
      public string AmountMax;
      [ProtoMember(7)]
      [DefaultValue(1f)]
      public float Frequency = 1f;
      [ProtoMember(10)]
      public SerializableDefinitionId Id;

      protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EContainerTypeItem\u003C\u003EAmountMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          in string value)
        {
          owner.AmountMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          out string value)
        {
          value = owner.AmountMin;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EContainerTypeItem\u003C\u003EAmountMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          in string value)
        {
          owner.AmountMax = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          out string value)
        {
          value = owner.AmountMax;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EContainerTypeItem\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          in float value)
        {
          owner.Frequency = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          out float value)
        {
          value = owner.Frequency;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EContainerTypeItem\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      private class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EContainerTypeItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem();

        MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem IActivator<MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem>.CreateInstance() => new MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem();
      }
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003ECountMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in int value) => owner.CountMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out int value) => value = owner.CountMin;
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003ECountMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in int value) => owner.CountMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out int value) => value = owner.CountMax;
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        in MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem[] value)
      {
        owner.Items = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        out MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem[] value)
      {
        value = owner.Items;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerTypeDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ContainerTypeDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContainerTypeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContainerTypeDefinition();

      MyObjectBuilder_ContainerTypeDefinition IActivator<MyObjectBuilder_ContainerTypeDefinition>.CreateInstance() => new MyObjectBuilder_ContainerTypeDefinition();
    }
  }
}
