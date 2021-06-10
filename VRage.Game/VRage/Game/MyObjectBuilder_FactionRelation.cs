// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionRelation
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyObjectBuilder_FactionRelation
  {
    [ProtoMember(1)]
    public long FactionId1;
    [ProtoMember(4)]
    public long FactionId2;
    [ProtoMember(7)]
    public MyRelationsBetweenFactions Relation;
    [ProtoMember(10)]
    public int Reputation;

    protected class VRage_Game_MyObjectBuilder_FactionRelation\u003C\u003EFactionId1\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionRelation owner, in long value) => owner.FactionId1 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionRelation owner, out long value) => value = owner.FactionId1;
    }

    protected class VRage_Game_MyObjectBuilder_FactionRelation\u003C\u003EFactionId2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionRelation owner, in long value) => owner.FactionId2 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionRelation owner, out long value) => value = owner.FactionId2;
    }

    protected class VRage_Game_MyObjectBuilder_FactionRelation\u003C\u003ERelation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, MyRelationsBetweenFactions>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionRelation owner,
        in MyRelationsBetweenFactions value)
      {
        owner.Relation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionRelation owner,
        out MyRelationsBetweenFactions value)
      {
        value = owner.Relation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionRelation\u003C\u003EReputation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionRelation owner, in int value) => owner.Reputation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionRelation owner, out int value) => value = owner.Reputation;
    }

    private class VRage_Game_MyObjectBuilder_FactionRelation\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionRelation>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionRelation();

      MyObjectBuilder_FactionRelation IActivator<MyObjectBuilder_FactionRelation>.CreateInstance() => new MyObjectBuilder_FactionRelation();
    }
  }
}
