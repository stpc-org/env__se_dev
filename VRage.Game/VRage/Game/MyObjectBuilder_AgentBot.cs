// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AgentBot
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
  public class MyObjectBuilder_AgentBot : MyObjectBuilder_Bot
  {
    [ProtoMember(1)]
    public MyObjectBuilder_AiTarget AiTarget;
    [ProtoMember(4)]
    public bool RemoveAfterDeath;
    [ProtoMember(7)]
    public int RespawnCounter;

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EAiTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, MyObjectBuilder_AiTarget>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in MyObjectBuilder_AiTarget value) => owner.AiTarget = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AgentBot owner,
        out MyObjectBuilder_AiTarget value)
      {
        value = owner.AiTarget;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ERemoveAfterDeath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in bool value) => owner.RemoveAfterDeath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out bool value) => value = owner.RemoveAfterDeath;
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ERespawnCounter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AgentBot, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in int value) => owner.RespawnCounter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out int value) => value = owner.RespawnCounter;
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EBotDefId\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotDefId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in SerializableDefinitionId value) => this.Set((MyObjectBuilder_Bot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AgentBot owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Bot&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EBotMemory\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotMemory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyObjectBuilder_BotMemory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AgentBot owner,
        in MyObjectBuilder_BotMemory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Bot&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AgentBot owner,
        out MyObjectBuilder_BotMemory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Bot&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ELastBehaviorTree\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003ELastBehaviorTree\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in string value) => this.Set((MyObjectBuilder_Bot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out string value) => this.Get((MyObjectBuilder_Bot&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EAsociatedMyPlayerId\u003C\u003EAccessor : MyObjectBuilder_Bot.VRage_Game_MyObjectBuilder_Bot\u003C\u003EAsociatedMyPlayerId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in ulong value) => this.Set((MyObjectBuilder_Bot&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out ulong value) => this.Get((MyObjectBuilder_Bot&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AgentBot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AgentBot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AgentBot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AgentBot\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AgentBot>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AgentBot();

      MyObjectBuilder_AgentBot IActivator<MyObjectBuilder_AgentBot>.CreateInstance() => new MyObjectBuilder_AgentBot();
    }
  }
}
