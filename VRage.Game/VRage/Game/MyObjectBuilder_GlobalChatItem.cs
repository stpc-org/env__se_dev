// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GlobalChatItem
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
  public class MyObjectBuilder_GlobalChatItem : MyObjectBuilder_Base
  {
    [ProtoMember(52)]
    [XmlAttribute("t")]
    public string Text;
    [ProtoMember(55)]
    [XmlElement(ElementName = "I")]
    public long IdentityIdUniqueNumber;
    [ProtoMember(58)]
    [XmlAttribute("a")]
    [DefaultValue("")]
    public string Author;
    [ProtoMember(61)]
    [XmlAttribute("f")]
    [DefaultValue("Blue")]
    public string Font;

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003EIdentityIdUniqueNumber\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in long value) => owner.IdentityIdUniqueNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out long value) => value = owner.IdentityIdUniqueNumber;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003EAuthor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value) => owner.Author = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value) => value = owner.Author;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value) => owner.Font = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value) => value = owner.Font;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalChatItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalChatItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalChatItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GlobalChatItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GlobalChatItem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GlobalChatItem();

      MyObjectBuilder_GlobalChatItem IActivator<MyObjectBuilder_GlobalChatItem>.CreateInstance() => new MyObjectBuilder_GlobalChatItem();
    }
  }
}
