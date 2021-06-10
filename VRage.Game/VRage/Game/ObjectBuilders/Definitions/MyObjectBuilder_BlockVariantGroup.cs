// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_BlockVariantGroup
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

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_BlockVariantGroup : MyObjectBuilder_DefinitionBase
  {
    [XmlArrayItem("Block")]
    [ProtoMember(1)]
    [DefaultValue(null)]
    public SerializableDefinitionId[] Blocks;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BlockVariantGroup, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BlockVariantGroup owner,
        in SerializableDefinitionId[] value)
      {
        owner.Blocks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BlockVariantGroup owner,
        out SerializableDefinitionId[] value)
      {
        value = owner.Blocks;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BlockVariantGroup owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BlockVariantGroup owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BlockVariantGroup, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BlockVariantGroup owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BlockVariantGroup owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_BlockVariantGroup\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BlockVariantGroup>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BlockVariantGroup();

      MyObjectBuilder_BlockVariantGroup IActivator<MyObjectBuilder_BlockVariantGroup>.CreateInstance() => new MyObjectBuilder_BlockVariantGroup();
    }
  }
}
