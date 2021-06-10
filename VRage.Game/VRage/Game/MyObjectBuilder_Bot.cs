// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Bot
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
  public class MyObjectBuilder_Bot : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public SerializableDefinitionId BotDefId;
    [ProtoMember(4)]
    public MyObjectBuilder_BotMemory BotMemory;
    [ProtoMember(7)]
    public string LastBehaviorTree;
    [ProtoMember(10)]
    public ulong AsociatedMyPlayerId;

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotDefId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Bot, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in SerializableDefinitionId value) => owner.BotDefId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out SerializableDefinitionId value) => value = owner.BotDefId;
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003EBotMemory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Bot, MyObjectBuilder_BotMemory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in MyObjectBuilder_BotMemory value) => owner.BotMemory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out MyObjectBuilder_BotMemory value) => value = owner.BotMemory;
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003ELastBehaviorTree\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Bot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in string value) => owner.LastBehaviorTree = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out string value) => value = owner.LastBehaviorTree;
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003EAsociatedMyPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Bot, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in ulong value) => owner.AsociatedMyPlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out ulong value) => value = owner.AsociatedMyPlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Bot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Bot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Bot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Bot\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Bot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Bot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Bot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Bot\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Bot>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Bot();

      MyObjectBuilder_Bot IActivator<MyObjectBuilder_Bot>.CreateInstance() => new MyObjectBuilder_Bot();
    }
  }
}
