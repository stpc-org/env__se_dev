// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PlayerFactionRelation
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyObjectBuilder_PlayerFactionRelation
  {
    [ProtoMember(1)]
    public long PlayerId;
    [ProtoMember(4)]
    public long FactionId;
    [ProtoMember(7)]
    public MyRelationsBetweenFactions Relation;
    [ProtoMember(10)]
    public int Reputation;

    protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in long value) => owner.FactionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out long value) => value = owner.FactionId;
    }

    protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation\u003C\u003ERelation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, MyRelationsBetweenFactions>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlayerFactionRelation owner,
        in MyRelationsBetweenFactions value)
      {
        owner.Relation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlayerFactionRelation owner,
        out MyRelationsBetweenFactions value)
      {
        value = owner.Relation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation\u003C\u003EReputation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in int value) => owner.Reputation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out int value) => value = owner.Reputation;
    }

    private class VRage_Game_MyObjectBuilder_PlayerFactionRelation\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlayerFactionRelation>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlayerFactionRelation();

      MyObjectBuilder_PlayerFactionRelation IActivator<MyObjectBuilder_PlayerFactionRelation>.CreateInstance() => new MyObjectBuilder_PlayerFactionRelation();
    }
  }
}
