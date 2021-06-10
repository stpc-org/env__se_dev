// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.HalfVector2
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
  [Serializable]
  public struct HalfVector2 : IPackedVector<uint>, IPackedVector, IEquatable<HalfVector2>
  {
    private uint packedValue;

    public uint PackedValue
    {
      get => this.packedValue;
      set => this.packedValue = value;
    }

    public HalfVector2(float x, float y) => this.packedValue = HalfVector2.PackHelper(x, y);

    public HalfVector2(Vector2 vector) => this.packedValue = HalfVector2.PackHelper(vector.X, vector.Y);

    public static bool operator ==(HalfVector2 a, HalfVector2 b) => a.Equals(b);

    public static bool operator !=(HalfVector2 a, HalfVector2 b) => !a.Equals(b);

    void IPackedVector.PackFromVector4(Vector4 vector) => this.packedValue = HalfVector2.PackHelper(vector.X, vector.Y);

    private static uint PackHelper(float vectorX, float vectorY) => (uint) HalfUtils.Pack(vectorX) | (uint) HalfUtils.Pack(vectorY) << 16;

    public Vector2 ToVector2()
    {
      Vector2 vector2;
      vector2.X = HalfUtils.Unpack((ushort) this.packedValue);
      vector2.Y = HalfUtils.Unpack((ushort) (this.packedValue >> 16));
      return vector2;
    }

    Vector4 IPackedVector.ToVector4()
    {
      Vector2 vector2 = this.ToVector2();
      return new Vector4(vector2.X, vector2.Y, 0.0f, 1f);
    }

    public override string ToString() => this.ToVector2().ToString();

    public override int GetHashCode() => this.packedValue.GetHashCode();

    public override bool Equals(object obj) => obj is HalfVector2 other && this.Equals(other);

    public bool Equals(HalfVector2 other) => this.packedValue.Equals(other.packedValue);

    protected class VRageMath_PackedVector_HalfVector2\u003C\u003EpackedValue\u003C\u003EAccessor : IMemberAccessor<HalfVector2, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector2 owner, in uint value) => owner.packedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector2 owner, out uint value) => value = owner.packedValue;
    }

    protected class VRageMath_PackedVector_HalfVector2\u003C\u003EPackedValue\u003C\u003EAccessor : IMemberAccessor<HalfVector2, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector2 owner, in uint value) => owner.PackedValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector2 owner, out uint value) => value = owner.PackedValue;
    }
  }
}
