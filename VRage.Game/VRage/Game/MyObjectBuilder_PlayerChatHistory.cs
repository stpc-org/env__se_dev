// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PlayerChatHistory
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
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_PlayerChatHistory : MyObjectBuilder_Base
  {
    [ProtoMember(10)]
    [XmlArrayItem("PCI")]
    public List<MyObjectBuilder_PlayerChatItem> Chat;
    [ProtoMember(13)]
    [XmlElement(ElementName = "ID")]
    public long IdentityId;

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003EChat\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatHistory, List<MyObjectBuilder_PlayerChatItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlayerChatHistory owner,
        in List<MyObjectBuilder_PlayerChatItem> value)
      {
        owner.Chat = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlayerChatHistory owner,
        out List<MyObjectBuilder_PlayerChatItem> value)
      {
        value = owner.Chat;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerChatHistory, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatHistory owner, in long value) => owner.IdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatHistory owner, out long value) => value = owner.IdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlayerChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_PlayerChatHistory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlayerChatHistory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlayerChatHistory();

      MyObjectBuilder_PlayerChatHistory IActivator<MyObjectBuilder_PlayerChatHistory>.CreateInstance() => new MyObjectBuilder_PlayerChatHistory();
    }
  }
}
