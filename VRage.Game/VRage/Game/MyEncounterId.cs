// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyEncounterId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyEncounterId : IEquatable<MyEncounterId>
  {
    [ProtoMember(1)]
    public BoundingBoxD BoundingBox;
    [ProtoMember(4)]
    public int Seed;
    [ProtoMember(7)]
    public int EncounterId;

    public MyEncounterId(BoundingBoxD box, int seed, int encounterId)
    {
      this.Seed = seed;
      this.EncounterId = encounterId;
      this.BoundingBox = box.Round(2);
    }

    public static bool operator ==(MyEncounterId x, MyEncounterId y) => x.BoundingBox.Equals(y.BoundingBox, 2.0) && x.Seed == y.Seed && x.EncounterId == y.EncounterId;

    public static bool operator !=(MyEncounterId x, MyEncounterId y) => !(x == y);

    public override bool Equals(object o) => o is MyEncounterId other && this.Equals(other);

    public bool Equals(MyEncounterId other) => this == other;

    public override int GetHashCode() => this.Seed;

    public override string ToString() => string.Format("{0}:{1}_{2}", (object) this.Seed, (object) this.EncounterId, (object) this.BoundingBox);

    protected class VRage_Game_MyEncounterId\u003C\u003EBoundingBox\u003C\u003EAccessor : IMemberAccessor<MyEncounterId, BoundingBoxD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEncounterId owner, in BoundingBoxD value) => owner.BoundingBox = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEncounterId owner, out BoundingBoxD value) => value = owner.BoundingBox;
    }

    protected class VRage_Game_MyEncounterId\u003C\u003ESeed\u003C\u003EAccessor : IMemberAccessor<MyEncounterId, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEncounterId owner, in int value) => owner.Seed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEncounterId owner, out int value) => value = owner.Seed;
    }

    protected class VRage_Game_MyEncounterId\u003C\u003EEncounterId\u003C\u003EAccessor : IMemberAccessor<MyEncounterId, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEncounterId owner, in int value) => owner.EncounterId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEncounterId owner, out int value) => value = owner.EncounterId;
    }

    private class VRage_Game_MyEncounterId\u003C\u003EActor : IActivator, IActivator<MyEncounterId>
    {
      object IActivator.CreateInstance() => (object) new MyEncounterId();

      MyEncounterId IActivator<MyEncounterId>.CreateInstance() => new MyEncounterId();
    }
  }
}
