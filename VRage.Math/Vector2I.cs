// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector2I
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector2I
  {
    public static readonly Vector2I.ComparerClass Comparer = new Vector2I.ComparerClass();
    public static Vector2I Zero = new Vector2I();
    public static Vector2I One = new Vector2I(1, 1);
    public static Vector2I UnitX = new Vector2I(1, 0);
    public static Vector2I UnitY = new Vector2I(0, 1);
    [ProtoMember(1)]
    public int X;
    [ProtoMember(4)]
    public int Y;

    public Vector2I(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2I(int width)
    {
      this.X = width;
      this.Y = width;
    }

    public Vector2I(Vector2 vec)
    {
      this.X = (int) vec.X;
      this.Y = (int) vec.Y;
    }

    public Vector2I(Vector2D vec)
    {
      this.X = (int) vec.X;
      this.Y = (int) vec.Y;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y;

    public int Size() => Math.Abs(this.X * this.Y);

    public static implicit operator Vector2(Vector2I intVector) => new Vector2((float) intVector.X, (float) intVector.Y);

    public static Vector2I operator +(Vector2I left, Vector2I right) => new Vector2I(left.X + right.X, left.Y + right.Y);

    public static Vector2I operator +(Vector2I left, int right) => new Vector2I(left.X + right, left.Y + right);

    public static Vector2I operator -(Vector2I left, Vector2I right) => new Vector2I(left.X - right.X, left.Y - right.Y);

    public static Vector2I operator -(Vector2I left, int value) => new Vector2I(left.X - value, left.Y - value);

    public static Vector2I operator -(Vector2I left) => new Vector2I(-left.X, -left.Y);

    public static Vector2I operator *(Vector2I value1, int multiplier) => new Vector2I(value1.X * multiplier, value1.Y * multiplier);

    public static Vector2I operator /(Vector2I value1, int divider) => new Vector2I(value1.X / divider, value1.Y / divider);

    public static bool operator ==(Vector2I left, Vector2I right) => left.X == right.X && left.Y == right.Y;

    public static bool operator !=(Vector2I left, Vector2I right) => left.X != right.X || left.Y != right.Y;

    public static Vector2I operator <<(Vector2I left, int bits) => new Vector2I(left.X << bits, left.Y << bits);

    public static Vector2I operator >>(Vector2I left, int bits) => new Vector2I(left.X >> bits, left.Y >> bits);

    public static Vector2I Floor(Vector2 value) => new Vector2I((int) Math.Floor((double) value.X), (int) Math.Floor((double) value.Y));

    public static Vector2I Round(Vector2 value) => new Vector2I((int) Math.Round((double) value.X), (int) Math.Round((double) value.Y));

    public bool Between(ref Vector2I start, ref Vector2I end)
    {
      if (this.X >= start.X && this.X <= end.X)
        return true;
      return this.Y >= start.Y && this.Y <= end.Y;
    }

    public override bool Equals(object obj) => obj is Vector2I vector2I && this == vector2I;

    public override int GetHashCode() => this.X * 397 ^ this.Y;

    public static void Min(ref Vector2I v1, ref Vector2I v2, out Vector2I min)
    {
      min.X = Math.Min(v1.X, v2.X);
      min.Y = Math.Min(v1.Y, v2.Y);
    }

    public static void Max(ref Vector2I v1, ref Vector2I v2, out Vector2I max)
    {
      max.X = Math.Max(v1.X, v2.X);
      max.Y = Math.Max(v1.Y, v2.Y);
    }

    public static Vector2I Min(Vector2I v1, Vector2I v2) => new Vector2I(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y));

    public static Vector2I Max(Vector2I v1, Vector2I v2) => new Vector2I(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y));

    public class ComparerClass : IEqualityComparer<Vector2I>
    {
      public bool Equals(Vector2I x, Vector2I y) => x.X == y.X && x.Y == y.Y;

      public int GetHashCode(Vector2I obj) => obj.X * 397 ^ obj.Y;
    }

    protected class VRageMath_Vector2I\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2I owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2I owner, out int value) => value = owner.X;
    }

    protected class VRageMath_Vector2I\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector2I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2I owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2I owner, out int value) => value = owner.Y;
    }
  }
}
