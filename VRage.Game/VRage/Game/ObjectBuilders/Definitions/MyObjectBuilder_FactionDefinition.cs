// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_FactionDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_FactionDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Tag;
    [ProtoMember(4)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(7)]
    [XmlAttribute]
    public string Founder;
    [ProtoMember(10)]
    public bool AcceptHumans;
    [ProtoMember(13)]
    public bool AutoAcceptMember = true;
    [ProtoMember(16)]
    public bool EnableFriendlyFire;
    [ProtoMember(19)]
    public bool IsDefault;
    [ProtoMember(22)]
    public MyRelationsBetweenFactions DefaultRelation = MyRelationsBetweenFactions.Enemies;
    [ProtoMember(25)]
    public long StartingBalance;
    [ProtoMember(28)]
    public MyFactionTypes Type;
    [ProtoMember(31)]
    public bool DiscoveredByDefault;
    [ProtoMember(34)]
    [ModdableContentFile(new string[] {"dds", "png"})]
    public string FactionIcon;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003ETag\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => owner.Tag = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => value = owner.Tag;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EFounder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => owner.Founder = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => value = owner.Founder;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EAcceptHumans\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => owner.AcceptHumans = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => value = owner.AcceptHumans;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EAutoAcceptMember\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => owner.AutoAcceptMember = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => value = owner.AutoAcceptMember;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EEnableFriendlyFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => owner.EnableFriendlyFire = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => value = owner.EnableFriendlyFire;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EIsDefault\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => owner.IsDefault = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => value = owner.IsDefault;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDefaultRelation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, MyRelationsBetweenFactions>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionDefinition owner,
        in MyRelationsBetweenFactions value)
      {
        owner.DefaultRelation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionDefinition owner,
        out MyRelationsBetweenFactions value)
      {
        value = owner.DefaultRelation;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EStartingBalance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in long value) => owner.StartingBalance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out long value) => value = owner.StartingBalance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, MyFactionTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in MyFactionTypes value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out MyFactionTypes value) => value = owner.Type;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDiscoveredByDefault\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => owner.DiscoveredByDefault = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => value = owner.DiscoveredByDefault;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EFactionIcon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => owner.FactionIcon = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => value = owner.FactionIcon;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionDefinition();

      MyObjectBuilder_FactionDefinition IActivator<MyObjectBuilder_FactionDefinition>.CreateInstance() => new MyObjectBuilder_FactionDefinition();
    }
  }
}
