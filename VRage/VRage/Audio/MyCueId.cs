// Decompiled with JetBrains decompiler
// Type: VRage.Audio.MyCueId
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.Audio
{
  [ProtoContract]
  public struct MyCueId
  {
    [ProtoMember(1)]
    public MyStringHash Hash;
    public static readonly MyCueId.ComparerType Comparer = new MyCueId.ComparerType();

    public MyCueId(MyStringHash hash) => this.Hash = hash;

    public bool IsNull => this.Hash == MyStringHash.NullOrEmpty;

    public static bool operator ==(MyCueId r, MyCueId l) => r.Hash == l.Hash;

    public static bool operator !=(MyCueId r, MyCueId l) => r.Hash != l.Hash;

    public bool Equals(MyCueId obj) => obj.Hash.Equals(this.Hash);

    public override bool Equals(object obj) => obj is MyCueId myCueId && myCueId.Hash.Equals(this.Hash);

    public override int GetHashCode() => this.Hash.GetHashCode();

    public override string ToString() => this.Hash.ToString();

    public class ComparerType : IEqualityComparer<MyCueId>
    {
      bool IEqualityComparer<MyCueId>.Equals(MyCueId x, MyCueId y) => x.Hash == y.Hash;

      int IEqualityComparer<MyCueId>.GetHashCode(MyCueId obj) => obj.Hash.GetHashCode();
    }

    protected class VRage_Audio_MyCueId\u003C\u003EHash\u003C\u003EAccessor : IMemberAccessor<MyCueId, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCueId owner, in MyStringHash value) => owner.Hash = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCueId owner, out MyStringHash value) => value = owner.Hash;
    }
  }
}
