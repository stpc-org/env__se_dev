// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AnimalBotDefinition
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
  public class MyObjectBuilder_AnimalBotDefinition : MyObjectBuilder_AgentDefinition
  {
    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EBotModel\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EBotModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ETargetType\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ETargetType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EInventoryContentGenerated\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EInventoryContentGenerated\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EInventoryContainerTypeId\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EInventoryContainerTypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ERespawnTimeMs\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERespawnTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in int value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out int value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ERemoveTimeMs\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003ERemoveTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in int value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out int value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_AgentDefinition.VRage_Game_MyObjectBuilder_AgentDefinition\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_AgentDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_AgentDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EBotBehaviorTree\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBotBehaviorTree\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, MyObjectBuilder_BotDefinition.BotBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        in MyObjectBuilder_BotDefinition.BotBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        out MyObjectBuilder_BotDefinition.BotBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EBehaviorType\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBehaviorType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EBehaviorSubtype\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003EBehaviorSubtype\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ECommandable\u003C\u003EAccessor : MyObjectBuilder_BotDefinition.VRage_Game_MyObjectBuilder_BotDefinition\u003C\u003ECommandable\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_BotDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_BotDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBotDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBotDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBotDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBotDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AnimalBotDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimalBotDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimalBotDefinition();

      MyObjectBuilder_AnimalBotDefinition IActivator<MyObjectBuilder_AnimalBotDefinition>.CreateInstance() => new MyObjectBuilder_AnimalBotDefinition();
    }
  }
}
