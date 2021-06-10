// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PlayerChatItem
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_PlayerChatItem : MyObjectBuilder_Base
  {
    [ProtoMember(25)]
    [XmlAttribute("t")]
    public string Text;
    [ProtoMember(28)]
    [XmlElement(ElementName = "I")]
    public long IdentityIdUniqueNumber;
    [ProtoMember(31)]
    [XmlElement(ElementName = "T")]
    public long TimestampMs;
    [ProtoMember(34)]
    [DefaultValue(true)]
    [XmlElement(ElementName = "S")]
    public bool Sent = true;

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003EIdentityIdUniqueNumber\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in long value) => owner.IdentityIdUniqueNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out long value) => value = owner.IdentityIdUniqueNumber;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003ETimestampMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in long value) => owner.TimestampMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out long value) => value = owner.TimestampMs;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003ESent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatItem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in bool value) => owner.Sent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out bool value) => value = owner.Sent;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_PlayerChatItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlayerChatItem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlayerChatItem();

      MyObjectBuilder_PlayerChatItem IActivator<MyObjectBuilder_PlayerChatItem>.CreateInstance() => new MyObjectBuilder_PlayerChatItem();
    }
  }
}
