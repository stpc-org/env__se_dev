// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionChatHistory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_FactionChatHistory : MyObjectBuilder_Base
  {
    [ProtoMember(16)]
    [XmlArrayItem("FCI")]
    public List<MyObjectBuilder_FactionChatItem> Chat;
    [ProtoMember(19)]
    [XmlElement(ElementName = "ID1")]
    public long FactionId1;
    [XmlElement(ElementName = "ID2")]
    public long FactionId2;

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003EChat\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatHistory, List<MyObjectBuilder_FactionChatItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionChatHistory owner,
        in List<MyObjectBuilder_FactionChatItem> value)
      {
        owner.Chat = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionChatHistory owner,
        out List<MyObjectBuilder_FactionChatItem> value)
      {
        value = owner.Chat;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003EFactionId1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatHistory, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in long value) => owner.FactionId1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out long value) => value = owner.FactionId1;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003EFactionId2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionChatHistory, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in long value) => owner.FactionId2 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out long value) => value = owner.FactionId2;
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_FactionChatHistory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionChatHistory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionChatHistory();

      MyObjectBuilder_FactionChatHistory IActivator<MyObjectBuilder_FactionChatHistory>.CreateInstance() => new MyObjectBuilder_FactionChatHistory();
    }
  }
}
