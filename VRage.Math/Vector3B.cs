// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3B
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector3B
  {
    [ProtoMember(1)]
    public sbyte X;
    [ProtoMember(4)]
    public sbyte Y;
    [ProtoMember(7)]
    public sbyte Z;
    public static readonly Vector3B Zero = new Vector3B();
    public static Vector3B Up = new Vector3B((sbyte) 0, (sbyte) 1, (sbyte) 0);
    public static Vector3B Down = new Vector3B((sbyte) 0, (sbyte) -1, (sbyte) 0);
    public static Vector3B Right = new Vector3B((sbyte) 1, (sbyte) 0, (sbyte) 0);
    public static Vector3B Left = new Vector3B((sbyte) -1, (sbyte) 0, (sbyte) 0);
    public static Vector3B Forward = new Vector3B((sbyte) 0, (sbyte) 0, (sbyte) -1);
    public static Vector3B Backward = new Vector3B((sbyte) 0, (sbyte) 0, (sbyte) 1);

    public Vector3B(sbyte x, sbyte y, sbyte z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3B(Vector3I vec)
    {
      this.X = (sbyte) vec.X;
      this.Y = (sbyte) vec.Y;
      this.Z = (sbyte) vec.Z;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z;

    public override int GetHashCode() => (int) (byte) this.Z << 16 | (int) (byte) this.Y << 8 | (int) (byte) this.X;

    public override bool Equals(object obj)
    {
      if (obj != null)
      {
        Vector3B? nullable = obj as Vector3B?;
        if (nullable.HasValue)
          return this == nullable.Value;
      }
      return false;
    }

    public static Vector3 operator *(Vector3 vector, Vector3B shortVector) => shortVector * vector;

    public static Vector3 operator *(Vector3B shortVector, Vector3 vector) => new Vector3((float) shortVector.X * vector.X, (float) shortVector.Y * vector.Y, (float) shortVector.Z * vector.Z);

    public static implicit operator Vector3I(Vector3B vec) => new Vector3I((int) vec.X, (int) vec.Y, (int) vec.Z);

    public static Vector3B Round(Vector3 vec) => new Vector3B((sbyte) Math.Round((double) vec.X), (sbyte) Math.Round((double) vec.Y), (sbyte) Math.Round((double) vec.Z));

    public static bool operator ==(Vector3B a, Vector3B b) => (int) a.X == (int) b.X && (int) a.Y == (int) b.Y && (int) a.Z == (int) b.Z;

    public static bool operator !=(Vector3B a, Vector3B b) => !(a == b);

    public static Vector3B operator -(Vector3B me) => new Vector3B(-me.X, -me.Y, -me.Z);

    public static Vector3B Fit(Vector3 vec, float range) => Vector3B.Round(vec / range * 128f);

    protected class VRageMath_Vector3B\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3B, sbyte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3B owner, in sbyte value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3B owner, out sbyte value) => value = owner.X;
    }

    protected class VRageMath_Vector3B\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3B, sbyte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3B owner, in sbyte value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3B owner, out sbyte value) => value = owner.Y;
    }

    protected class VRageMath_Vector3B\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3B, sbyte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3B owner, in sbyte value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3B owner, out sbyte value) => value = owner.Z;
    }
  }
}
