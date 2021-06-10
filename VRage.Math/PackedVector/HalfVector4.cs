// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.HalfVector4
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
  [Serializable]
  public struct HalfVector4 : IPackedVector<ulong>, IPackedVector, IEquatable<HalfVector4>
  {
    public ulong PackedValue;

    ulong IPackedVector<ulong>.PackedValue
    {
      get => this.PackedValue;
      set => this.PackedValue = value;
    }

    public HalfVector4(float x, float y, float z, float w) => this.PackedValue = HalfVector4.PackHelper(x, y, z, w);

    public HalfVector4(Vector4 vector) => this.PackedValue = HalfVector4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    public HalfVector4(HalfVector3 vector3, ushort w) => this.PackedValue = vector3.ToHalfVector4().PackedValue | (ulong) w << 48;

    public HalfVector4(ulong packedValue) => this.PackedValue = packedValue;

    public static bool operator ==(HalfVector4 a, HalfVector4 b) => a.Equals(b);

    public static bool operator !=(HalfVector4 a, HalfVector4 b) => !a.Equals(b);

    void IPackedVector.PackFromVector4(Vector4 vector) => this.PackedValue = HalfVector4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    private static ulong PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW) => (ulong) ((long) HalfUtils.Pack(vectorX) | (long) HalfUtils.Pack(vectorY) << 16 | (long) HalfUtils.Pack(vectorZ) << 32 | (long) HalfUtils.Pack(vectorW) << 48);

    public Vector4 ToVector4()
    {
      Vector4 vector4;
      vector4.X = HalfUtils.Unpack((ushort) this.PackedValue);
      vector4.Y = HalfUtils.Unpack((ushort) (this.PackedValue >> 16));
      vector4.Z = HalfUtils.Unpack((ushort) (this.PackedValue >> 32));
      vector4.W = HalfUtils.Unpack((ushort) (this.PackedValue >> 48));
      return vector4;
    }

    public override string ToString() => this.ToVector4().ToString();

    public override int GetHashCode() => this.PackedValue.GetHashCode();

    public override bool Equals(object obj) => obj is HalfVector4 other && this.Equals(other);

    public bool Equals(HalfVector4 other) => this.PackedValue.Equals(other.PackedValue);

    protected class VRageMath_PackedVector_HalfVector4\u003C\u003EPackedValue\u003C\u003EAccessor : IMemberAccessor<HalfVector4, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector4 owner, in ulong value) => owner.PackedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector4 owner, out ulong value) => value = owner.PackedValue;
    }

    protected class VRageMath_PackedVector_HalfVector4\u003C\u003EVRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt64\u003E\u002EPackedValue\u003C\u003EAccessor : IMemberAccessor<HalfVector4, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector4 owner, in ulong value) => owner.VRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt64\u003E\u002EPackedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector4 owner, out ulong value) => value = owner.VRageMath\u002EPackedVector\u002EIPackedVector\u003CSystem\u002EUInt64\u003E\u002EPackedValue;
    }
  }
}
