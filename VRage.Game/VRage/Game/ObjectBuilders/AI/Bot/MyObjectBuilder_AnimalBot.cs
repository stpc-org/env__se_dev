// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.AI.Bot.MyObjectBuilder_AnimalBot
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI.Bot
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimalBot : MyObjectBuilder_AgentBot
  {
    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003EAiTarget\u003C\u003EAccessor : MyObjectBuilder_AgentBot.VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EAiTarget\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyObjectBuilder_AiTarget>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBot owner,
        in MyObjectBuilder_AiTarget value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AgentBot&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBot owner,
        out MyObjectBuilder_AiTarget value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AgentBot&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor : MyObjectBuilder_AgentBot.VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in bool value) => this.Set((MyObjectBuilder_AgentBot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out bool value) => this.Get((MyObjectBuilder_AgentBot&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003ERespawnCounter\u003C\u003EAccessor : MyObjectBuilder_AgentBot.VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ERespawnCounter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in int value) => this.Set((MyObjectBuilder_AgentBot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out int value) => this.Get((MyObjectBuilder_AgentBot&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003EBotDefId\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotDefId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBot owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Bot&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBot owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Bot&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003EBotMemory\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotMemory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyObjectBuilder_BotMemory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimalBot owner,
        in MyObjectBuilder_BotMemory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Bot&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimalBot owner,
        out MyObjectBuilder_BotMemory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Bot&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003ELastBehaviorTree\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003ELastBehaviorTree\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in string value) => this.Set((MyObjectBuilder_Bot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out string value) => this.Get((MyObjectBuilder_Bot&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003EAsociatedMyPlayerId\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EAsociatedMyPlayerId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in ulong value) => this.Set((MyObjectBuilder_Bot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out ulong value) => this.Get((MyObjectBuilder_Bot&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimalBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimalBot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimalBot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_AnimalBot\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimalBot>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimalBot();

      MyObjectBuilder_AnimalBot IActivator<MyObjectBuilder_AnimalBot>.CreateInstance() => new MyObjectBuilder_AnimalBot();
    }
  }
}
