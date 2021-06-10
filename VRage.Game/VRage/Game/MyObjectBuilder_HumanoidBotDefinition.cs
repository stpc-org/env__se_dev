// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_HumanoidBotDefinition
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
  public class MyObjectBuilder_HumanoidBotDefinition : MyObjectBuilder_AgentDefinition
  {
    [ProtoMember(4)]
    public MyObjectBuilder_HumanoidBotDefinition.Item StartingItem;
    [XmlArrayItem("Item")]
    [ProtoMember(7)]
    public MyObjectBuilder_HumanoidBotDefinition.Item[] InventoryItems;

    [ProtoContract]
    public class Item
    {
      [XmlIgnore]
      public MyObjectBuilderType Type = (MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject);
      [XmlAttribute]
      [ProtoMember(1)]
      public string Subtype;

      protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EItem\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition.Item, MyObjectBuilderType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_HumanoidBotDefinition.Item owner,
          in MyObjectBuilderType value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_HumanoidBotDefinition.Item owner,
          out MyObjectBuilderType value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EItem\u003C\u003ESubtype\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition.Item, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_HumanoidBotDefinition.Item owner,
          in string value)
        {
          owner.Subtype = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_HumanoidBotDefinition.Item owner,
          out string value)
        {
          value = owner.Subtype;
        }
      }

      private class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_HumanoidBotDefinition.Item>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_HumanoidBotDefinition.Item();

        MyObjectBuilder_HumanoidBotDefinition.Item IActivator<MyObjectBuilder_HumanoidBotDefinition.Item>.CreateInstance() => new MyObjectBuilder_HumanoidBotDefinition.Item();
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EStartingItem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyObjectBuilder_HumanoidBotDefinition.Item>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in MyObjectBuilder_HumanoidBotDefinition.Item value)
      {
        owner.StartingItem = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out MyObjectBuilder_HumanoidBotDefinition.Item value)
      {
        value = owner.StartingItem;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EInventoryItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyObjectBuilder_HumanoidBotDefinition.Item[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in MyObjectBuilder_HumanoidBotDefinition.Item[] value)
      {
        owner.InventoryItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out MyObjectBuilder_HumanoidBotDefinition.Item[] value)
      {
        value = owner.InventoryItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EBotModel\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EBotModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ETargetType\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ETargetType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EInventoryContentGenerated\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EInventoryContentGenerated\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EInventoryContainerTypeId\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EInventoryContainerTypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ERespawnTimeMs\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERespawnTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in int value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out int value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ERemoveTimeMs\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERemoveTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in int value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out int value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EBotBehaviorTree\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBotBehaviorTree\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyObjectBuilder_BotDefinition.BotBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in MyObjectBuilder_BotDefinition.BotBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out MyObjectBuilder_BotDefinition.BotBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EBehaviorType\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBehaviorType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EBehaviorSubtype\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBehaviorSubtype\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ECommandable\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003ECommandable\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HumanoidBotDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HumanoidBotDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HumanoidBotDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_HumanoidBotDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_HumanoidBotDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_HumanoidBotDefinition();

      MyObjectBuilder_HumanoidBotDefinition IActivator<MyObjectBuilder_HumanoidBotDefinition>.CreateInstance() => new MyObjectBuilder_HumanoidBotDefinition();
    }
  }
}
