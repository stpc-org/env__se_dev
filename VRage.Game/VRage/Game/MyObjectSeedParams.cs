// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectSeedParams
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectSeedParams
  {
    [ProtoMember(1)]
    public int Index;
    [ProtoMember(4)]
    public int Seed;
    [ProtoMember(7)]
    public MyObjectSeedType Type;
    [ProtoMember(10)]
    public bool Generated;
    [ProtoMember(13)]
    public int m_proxyId = -1;
    [ProtoMember(16, IsRequired = false)]
    public int GeneratorSeed;

    public override bool Equals(object obj)
    {
      MyObjectSeedParams objectSeedParams = obj as MyObjectSeedParams;
      return this.Seed == objectSeedParams.Seed && this.Index == objectSeedParams.Index && this.GeneratorSeed == objectSeedParams.GeneratorSeed;
    }

    public override int GetHashCode() => this.Seed;

    protected class VRage_Game_MyObjectSeedParams\u003C\u003EIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in int value) => owner.Index = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out int value) => value = owner.Index;
    }

    protected class VRage_Game_MyObjectSeedParams\u003C\u003ESeed\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in int value) => owner.Seed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out int value) => value = owner.Seed;
    }

    protected class VRage_Game_MyObjectSeedParams\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, MyObjectSeedType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in MyObjectSeedType value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out MyObjectSeedType value) => value = owner.Type;
    }

    protected class VRage_Game_MyObjectSeedParams\u003C\u003EGenerated\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in bool value) => owner.Generated = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out bool value) => value = owner.Generated;
    }

    protected class VRage_Game_MyObjectSeedParams\u003C\u003Em_proxyId\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in int value) => owner.m_proxyId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out int value) => value = owner.m_proxyId;
    }

    protected class VRage_Game_MyObjectSeedParams\u003C\u003EGeneratorSeed\u003C\u003EAccessor : IMemberAccessor<MyObjectSeedParams, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectSeedParams owner, in int value) => owner.GeneratorSeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectSeedParams owner, out int value) => value = owner.GeneratorSeed;
    }

    private class VRage_Game_MyObjectSeedParams\u003C\u003EActor : IActivator, IActivator<MyObjectSeedParams>
    {
      object IActivator.CreateInstance() => (object) new MyObjectSeedParams();

      MyObjectSeedParams IActivator<MyObjectSeedParams>.CreateInstance() => new MyObjectSeedParams();
    }
  }
}
