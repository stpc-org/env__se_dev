// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionRequests
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
  public struct MyObjectBuilder_FactionRequests
  {
    [ProtoMember(10)]
    public long FactionId;
    [ProtoMember(13)]
    public List<long> FactionRequests;

    protected class VRage_Game_MyObjectBuilder_FactionRequests\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRequests, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionRequests owner, in long value) => owner.FactionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionRequests owner, out long value) => value = owner.FactionId;
    }

    protected class VRage_Game_MyObjectBuilder_FactionRequests\u003C\u003EFactionRequests\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRequests, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionRequests owner, in List<long> value) => owner.FactionRequests = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionRequests owner, out List<long> value) => value = owner.FactionRequests;
    }

    private class VRage_Game_MyObjectBuilder_FactionRequests\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionRequests>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionRequests();

      MyObjectBuilder_FactionRequests IActivator<MyObjectBuilder_FactionRequests>.CreateInstance() => new MyObjectBuilder_FactionRequests();
    }
  }
}
