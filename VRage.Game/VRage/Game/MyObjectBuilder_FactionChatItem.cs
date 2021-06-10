// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionChatItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
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
  public class MyObjectBuilder_FactionChatItem : MyObjectBuilder_Base
  {
    [ProtoMember(37)]
    [XmlAttribute("t")]
    public string Text;
    [ProtoMember(40)]
    [XmlElement(ElementName = "I")]
    public long IdentityIdUniqueNumber;
    [ProtoMember(43)]
    [XmlElement(ElementName = "T")]
    public long TimestampMs;
    [ProtoMember(46)]
    [DefaultValue(null)]
    [XmlElement(ElementName = "PTST")]
    public List<long> PlayersToSendToUniqueNumber;
    [ProtoMember(49)]
    [DefaultValue(null)]
    [XmlElement(ElementName = "IAST")]
    public List<bool> IsAlreadySentTo;

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003EIdentityIdUniqueNumber\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in long value) => owner.IdentityIdUniqueNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out long value) => value = owner.IdentityIdUniqueNumber;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003ETimestampMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in long value) => owner.TimestampMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out long value) => value = owner.TimestampMs;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003EPlayersToSendToUniqueNumber\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in List<long> value) => owner.PlayersToSendToUniqueNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out List<long> value) => value = owner.PlayersToSendToUniqueNumber;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003EIsAlreadySentTo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatItem, List<bool>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in List<bool> value) => owner.IsAlreadySentTo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out List<bool> value) => value = owner.IsAlreadySentTo;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_FactionChatItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionChatItem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionChatItem();

      MyObjectBuilder_FactionChatItem IActivator<MyObjectBuilder_FactionChatItem>.CreateInstance() => new MyObjectBuilder_FactionChatItem();
    }
  }
}
