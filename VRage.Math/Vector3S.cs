// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3S
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
  public struct Vector3S
  {
    [ProtoMember(1)]
    public short X;
    [ProtoMember(4)]
    public short Y;
    [ProtoMember(7)]
    public short Z;
    public static Vector3S Up = new Vector3S((short) 0, (short) 1, (short) 0);
    public static Vector3S Down = new Vector3S((short) 0, (short) -1, (short) 0);
    public static Vector3S Right = new Vector3S((short) 1, (short) 0, (short) 0);
    public static Vector3S Left = new Vector3S((short) -1, (short) 0, (short) 0);
    public static Vector3S Forward = new Vector3S((short) 0, (short) 0, (short) -1);
    public static Vector3S Backward = new Vector3S((short) 0, (short) 0, (short) 1);

    public Vector3S(Vector3I vec)
      : this(ref vec)
    {
    }

    public Vector3S(ref Vector3I vec)
    {
      this.X = (short) vec.X;
      this.Y = (short) vec.Y;
      this.Z = (short) vec.Z;
    }

    public Vector3S(short x, short y, short z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3S(float x, float y, float z)
    {
      this.X = (short) x;
      this.Y = (short) y;
      this.Z = (short) z;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z;

    public override int GetHashCode() => ((int) this.X * 397 ^ (int) this.Y) * 397 ^ (int) this.Z;

    public override bool Equals(object obj)
    {
      if (obj != null)
      {
        Vector3S? nullable = obj as Vector3S?;
        if (nullable.HasValue)
          return this == nullable.Value;
      }
      return false;
    }

    public static Vector3S operator *(Vector3S v, short t) => new Vector3S((short) ((int) t * (int) v.X), (short) ((int) t * (int) v.Y), (short) ((int) t * (int) v.Z));

    public static Vector3 operator *(Vector3S v, float t) => new Vector3(t * (float) v.X, t * (float) v.Y, t * (float) v.Z);

    public static Vector3 operator *(Vector3 vector, Vector3S shortVector) => shortVector * vector;

    public static Vector3 operator *(Vector3S shortVector, Vector3 vector) => new Vector3((float) shortVector.X * vector.X, (float) shortVector.Y * vector.Y, (float) shortVector.Z * vector.Z);

    public static bool operator ==(Vector3S v1, Vector3S v2) => (int) v1.X == (int) v2.X && (int) v1.Y == (int) v2.Y && (int) v1.Z == (int) v2.Z;

    public static bool operator !=(Vector3S v1, Vector3S v2) => (int) v1.X != (int) v2.X || (int) v1.Y != (int) v2.Y || (int) v1.Z != (int) v2.Z;

    public static Vector3S Round(Vector3 v) => new Vector3S((short) Math.Round((double) v.X), (short) Math.Round((double) v.Y), (short) Math.Round((double) v.Z));

    public static implicit operator Vector3I(Vector3S me) => new Vector3I((int) me.X, (int) me.Y, (int) me.Z);

    public static Vector3I operator -(Vector3S op1, Vector3B op2) => new Vector3I((int) op1.X - (int) op2.X, (int) op1.Y - (int) op2.Y, (int) op1.Z - (int) op2.Z);

    protected class VRageMath_Vector3S\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3S, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3S owner, in short value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3S owner, out short value) => value = owner.X;
    }

    protected class VRageMath_Vector3S\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3S, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3S owner, in short value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3S owner, out short value) => value = owner.Y;
    }

    protected class VRageMath_Vector3S\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3S, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3S owner, in short value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3S owner, out short value) => value = owner.Z;
    }
  }
}
