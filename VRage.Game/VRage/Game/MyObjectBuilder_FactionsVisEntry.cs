// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionsVisEntry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyObjectBuilder_FactionsVisEntry
  {
    [ProtoMember(1)]
    public ulong PlayerId;
    [ProtoMember(3)]
    public int SerialId;
    [ProtoMember(4)]
    public long IdentityId;
    [ProtoMember(5)]
    public List<long> DiscoveredFactions;

    protected class VRage_Game_MyObjectBuilder_FactionsVisEntry\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionsVisEntry owner, in ulong value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionsVisEntry owner, out ulong value) => value = owner.PlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_FactionsVisEntry\u003C\u003ESerialId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionsVisEntry owner, in int value) => owner.SerialId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionsVisEntry owner, out int value) => value = owner.SerialId;
    }

    protected class VRage_Game_MyObjectBuilder_FactionsVisEntry\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionsVisEntry owner, in long value) => owner.IdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionsVisEntry owner, out long value) => value = owner.IdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_FactionsVisEntry\u003C\u003EDiscoveredFactions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionsVisEntry owner, in List<long> value) => owner.DiscoveredFactions = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionsVisEntry owner, out List<long> value) => value = owner.DiscoveredFactions;
    }

    private class VRage_Game_MyObjectBuilder_FactionsVisEntry\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionsVisEntry>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionsVisEntry();

      MyObjectBuilder_FactionsVisEntry IActivator<MyObjectBuilder_FactionsVisEntry>.CreateInstance() => new MyObjectBuilder_FactionsVisEntry();
    }
  }
}
