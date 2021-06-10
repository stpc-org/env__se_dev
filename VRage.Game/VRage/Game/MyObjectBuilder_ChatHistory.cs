// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ChatHistory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_ChatHistory : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public long IdentityId;
    [ProtoMember(4)]
    public List<MyObjectBuilder_PlayerChatHistory> PlayerChatHistory;
    [ProtoMember(7)]
    public MyObjectBuilder_GlobalChatHistory GlobalChatHistory;

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ChatHistory owner, in long value) => owner.IdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ChatHistory owner, out long value) => value = owner.IdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003EPlayerChatHistory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, List<MyObjectBuilder_PlayerChatHistory>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ChatHistory owner,
        in List<MyObjectBuilder_PlayerChatHistory> value)
      {
        owner.PlayerChatHistory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ChatHistory owner,
        out List<MyObjectBuilder_PlayerChatHistory> value)
      {
        value = owner.PlayerChatHistory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003EGlobalChatHistory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ChatHistory, MyObjectBuilder_GlobalChatHistory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ChatHistory owner,
        in MyObjectBuilder_GlobalChatHistory value)
      {
        owner.GlobalChatHistory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ChatHistory owner,
        out MyObjectBuilder_GlobalChatHistory value)
      {
        value = owner.GlobalChatHistory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ChatHistory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ChatHistory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ChatHistory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ChatHistory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ChatHistory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ChatHistory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ChatHistory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ChatHistory();

      MyObjectBuilder_ChatHistory IActivator<MyObjectBuilder_ChatHistory>.CreateInstance() => new MyObjectBuilder_ChatHistory();
    }
  }
}
