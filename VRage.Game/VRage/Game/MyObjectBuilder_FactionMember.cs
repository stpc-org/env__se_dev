// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionMember
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyObjectBuilder_FactionMember
  {
    [ProtoMember(1)]
    public long PlayerId;
    [ProtoMember(4)]
    public bool IsLeader;
    [ProtoMember(7)]
    public bool IsFounder;

    protected class VRage_Game_MyObjectBuilder_FactionMember\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionMember owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionMember owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_FactionMember\u003C\u003EIsLeader\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionMember owner, in bool value) => owner.IsLeader = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionMember owner, out bool value) => value = owner.IsLeader;
    }

    protected class VRage_Game_MyObjectBuilder_FactionMember\u003C\u003EIsFounder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionMember owner, in bool value) => owner.IsFounder = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionMember owner, out bool value) => value = owner.IsFounder;
    }

    private class VRage_Game_MyObjectBuilder_FactionMember\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionMember>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionMember();

      MyObjectBuilder_FactionMember IActivator<MyObjectBuilder_FactionMember>.CreateInstance() => new MyObjectBuilder_FactionMember();
    }
  }
}
