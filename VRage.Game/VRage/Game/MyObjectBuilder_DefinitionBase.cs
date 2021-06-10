// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_DefinitionBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public abstract class MyObjectBuilder_DefinitionBase : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public SerializableDefinitionId Id;
    [ProtoMember(4)]
    [DefaultValue("")]
    public string DisplayName;
    [ProtoMember(7)]
    [DefaultValue("")]
    public string Description;
    [ProtoMember(10)]
    [DefaultValue(new string[] {""})]
    [XmlElement("Icon")]
    [ModdableContentFile(new string[] {"dds", "png"})]
    public string[] Icons;
    [ProtoMember(13)]
    [DefaultValue(true)]
    public bool Public = true;
    [ProtoMember(16)]
    [DefaultValue(true)]
    [XmlAttribute(AttributeName = "Enabled")]
    public bool Enabled = true;
    [ProtoMember(19)]
    [DefaultValue(true)]
    public bool AvailableInSurvival = true;
    [ProtoMember(22)]
    [DefaultValue("")]
    public string DescriptionArgs;
    [ProtoMember(25)]
    [DefaultValue(null)]
    [XmlElement("DLC")]
    public string[] DLCs;

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DefinitionBase owner,
        in SerializableDefinitionId value)
      {
        owner.Id = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DefinitionBase owner,
        out SerializableDefinitionId value)
      {
        value = owner.Id;
      }
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string value) => value = owner.DisplayName;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string value) => value = owner.Description;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string[] value) => owner.Icons = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string[] value) => value = owner.Icons;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value) => owner.Public = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value) => value = owner.Public;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in bool value) => owner.AvailableInSurvival = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out bool value) => value = owner.AvailableInSurvival;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string value) => owner.DescriptionArgs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string value) => value = owner.DescriptionArgs;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DefinitionBase, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string[] value) => owner.DLCs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string[] value) => value = owner.DLCs;
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DefinitionBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DefinitionBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DefinitionBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }
  }
}
