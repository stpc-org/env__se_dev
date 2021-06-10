// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_SessionComponentResearch
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SessionComponentResearch : MyObjectBuilder_SessionComponent
  {
    [XmlElement("Research")]
    public List<MyObjectBuilder_SessionComponentResearch.ResearchData> Researches;
    public bool WhitelistMode;

    public struct ResearchData
    {
      [XmlAttribute("Identity")]
      public long IdentityId;
      [XmlElement("Entry")]
      public List<SerializableDefinitionId> Definitions;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003EResearches\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentResearch, List<MyObjectBuilder_SessionComponentResearch.ResearchData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentResearch owner,
        in List<MyObjectBuilder_SessionComponentResearch.ResearchData> value)
      {
        owner.Researches = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentResearch owner,
        out List<MyObjectBuilder_SessionComponentResearch.ResearchData> value)
      {
        value = owner.Researches;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003EWhitelistMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentResearch, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentResearch owner, in bool value) => owner.WhitelistMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentResearch owner, out bool value) => value = owner.WhitelistMode;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentResearch, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentResearch owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentResearch owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentResearch, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentResearch owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentResearch owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentResearch, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentResearch owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentResearch owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentResearch, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentResearch owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentResearch owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentResearch, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SessionComponentResearch owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SessionComponentResearch owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentResearch\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentResearch>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionComponentResearch();

      MyObjectBuilder_SessionComponentResearch IActivator<MyObjectBuilder_SessionComponentResearch>.CreateInstance() => new MyObjectBuilder_SessionComponentResearch();
    }
  }
}
