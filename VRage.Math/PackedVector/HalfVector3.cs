// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.HalfVector3
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath.PackedVector
{
  [Serializable]
  public struct HalfVector3
  {
    public ushort X;
    public ushort Y;
    public ushort Z;

    public HalfVector3(float x, float y, float z)
    {
      this.X = HalfUtils.Pack(x);
      this.Y = HalfUtils.Pack(y);
      this.Z = HalfUtils.Pack(z);
    }

    public HalfVector3(Vector3 vector)
      : this(vector.X, vector.Y, vector.Z)
    {
    }

    public Vector3 ToVector3()
    {
      Vector3 vector3;
      vector3.X = HalfUtils.Unpack(this.X);
      vector3.Y = HalfUtils.Unpack(this.Y);
      vector3.Z = HalfUtils.Unpack(this.Z);
      return vector3;
    }

    public HalfVector4 ToHalfVector4()
    {
      HalfVector4 halfVector4;
      halfVector4.PackedValue = (ulong) ((long) this.X | (long) this.Y << 16 | (long) this.Z << 32);
      return halfVector4;
    }

    public static implicit operator HalfVector3(Vector3 v) => new HalfVector3(v);

    public static implicit operator Vector3(HalfVector3 v) => v.ToVector3();

    public override string ToString() => this.ToVector3().ToString();

    protected class VRageMath_PackedVector_HalfVector3\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<HalfVector3, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector3 owner, in ushort value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector3 owner, out ushort value) => value = owner.X;
    }

    protected class VRageMath_PackedVector_HalfVector3\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<HalfVector3, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector3 owner, in ushort value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector3 owner, out ushort value) => value = owner.Y;
    }

    protected class VRageMath_PackedVector_HalfVector3\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<HalfVector3, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref HalfVector3 owner, in ushort value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref HalfVector3 owner, out ushort value) => value = owner.Z;
    }
  }
}
