// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3I
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
  [Serializable]
  public struct Vector3I : IEquatable<Vector3I>, IComparable<Vector3I>
  {
    public static readonly Vector3I.EqualityComparer Comparer = new Vector3I.EqualityComparer();
    public static Vector3I UnitX = new Vector3I(1, 0, 0);
    public static Vector3I UnitY = new Vector3I(0, 1, 0);
    public static Vector3I UnitZ = new Vector3I(0, 0, 1);
    public static Vector3I Zero = new Vector3I(0, 0, 0);
    public static Vector3I MaxValue = new Vector3I(int.MaxValue, int.MaxValue, int.MaxValue);
    public static Vector3I MinValue = new Vector3I(int.MinValue, int.MinValue, int.MinValue);
    public static Vector3I Up = new Vector3I(0, 1, 0);
    public static Vector3I Down = new Vector3I(0, -1, 0);
    public static Vector3I Right = new Vector3I(1, 0, 0);
    public static Vector3I Left = new Vector3I(-1, 0, 0);
    public static Vector3I Forward = new Vector3I(0, 0, -1);
    public static Vector3I Backward = new Vector3I(0, 0, 1);
    [ProtoMember(1)]
    public int X;
    [ProtoMember(4)]
    public int Y;
    [ProtoMember(7)]
    public int Z;
    public static Vector3I One = new Vector3I(1, 1, 1);

    public int this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          case 2:
            this.Z = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public Vector3I(int xyz)
    {
      this.X = xyz;
      this.Y = xyz;
      this.Z = xyz;
    }

    public Vector3I(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3I(Vector2I xy, int z)
    {
      this.X = xy.X;
      this.Y = xy.Y;
      this.Z = z;
    }

    public Vector3I(Vector3 xyz)
    {
      this.X = (int) xyz.X;
      this.Y = (int) xyz.Y;
      this.Z = (int) xyz.Z;
    }

    public Vector3I(Vector3D xyz)
    {
      this.X = (int) xyz.X;
      this.Y = (int) xyz.Y;
      this.Z = (int) xyz.Z;
    }

    public Vector3I(Vector3S xyz)
    {
      this.X = (int) xyz.X;
      this.Y = (int) xyz.Y;
      this.Z = (int) xyz.Z;
    }

    public Vector3I(float x, float y, float z)
    {
      this.X = (int) x;
      this.Y = (int) y;
      this.Z = (int) z;
    }

    public Vector3I(byte[] bytes, int index)
    {
      this.X = BitConverter.ToInt32(bytes, index);
      this.Y = BitConverter.ToInt32(bytes, index + 4);
      this.Z = BitConverter.ToInt32(bytes, index + 8);
    }

    public static explicit operator Vector3I(Vector3 value) => new Vector3I((int) value.X, (int) value.Y, (int) value.Z);

    public override string ToString() => string.Format("[X:{0}, Y:{1}, Z:{2}]", (object) this.X, (object) this.Y, (object) this.Z);

    public bool Equals(Vector3I other) => other.X == this.X && other.Y == this.Y && other.Z == this.Z;

    public bool IsPowerOfTwo => MathHelper.IsPowerOfTwo(this.X) && MathHelper.IsPowerOfTwo(this.Y) && MathHelper.IsPowerOfTwo(this.Z);

    public override bool Equals(object obj) => obj != null && !(obj.GetType() != typeof (Vector3I)) && this.Equals((Vector3I) obj);

    public override int GetHashCode() => (this.X * 397 ^ this.Y) * 397 ^ this.Z;

    public bool IsInsideInclusiveEnd(ref Vector3I min, ref Vector3I max) => min.X <= this.X && this.X <= max.X && (min.Y <= this.Y && this.Y <= max.Y) && min.Z <= this.Z && this.Z <= max.Z;

    public bool IsInsideInclusiveEnd(Vector3I min, Vector3I max) => this.IsInsideInclusiveEnd(ref min, ref max);

    public bool IsInside(ref Vector3I inclusiveMin, ref Vector3I exclusiveMax) => inclusiveMin.X <= this.X && this.X < exclusiveMax.X && (inclusiveMin.Y <= this.Y && this.Y < exclusiveMax.Y) && inclusiveMin.Z <= this.Z && this.Z < exclusiveMax.Z;

    public bool IsInside(Vector3I inclusiveMin, Vector3I exclusiveMax) => this.IsInside(ref inclusiveMin, ref exclusiveMax);

    public int RectangularDistance(Vector3I otherVector) => Math.Abs(this.X - otherVector.X) + Math.Abs(this.Y - otherVector.Y) + Math.Abs(this.Z - otherVector.Z);

    public int RectangularLength() => Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);

    public int Length() => (int) Math.Sqrt((double) Vector3I.Dot(this, this));

    public static bool BoxIntersects(Vector3I minA, Vector3I maxA, Vector3I minB, Vector3I maxB) => Vector3I.BoxIntersects(ref minA, ref maxA, ref minB, ref maxB);

    public static bool BoxIntersects(
      ref Vector3I minA,
      ref Vector3I maxA,
      ref Vector3I minB,
      ref Vector3I maxB)
    {
      return maxA.X >= minB.X && minA.X <= maxB.X && (maxA.Y >= minB.Y && minA.Y <= maxB.Y) && maxA.Z >= minB.Z && minA.Z <= maxB.Z;
    }

    public static bool BoxContains(Vector3I boxMin, Vector3I boxMax, Vector3I pt) => boxMax.X >= pt.X && boxMin.X <= pt.X && (boxMax.Y >= pt.Y && boxMin.Y <= pt.Y) && boxMax.Z >= pt.Z && boxMin.Z <= pt.Z;

    public static bool BoxContains(ref Vector3I boxMin, ref Vector3I boxMax, ref Vector3I pt) => boxMax.X >= pt.X && boxMin.X <= pt.X && (boxMax.Y >= pt.Y && boxMin.Y <= pt.Y) && boxMax.Z >= pt.Z && boxMin.Z <= pt.Z;

    public static Vector3I operator *(Vector3I a, Vector3I b) => new Vector3I(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    public static bool operator ==(Vector3I a, Vector3I b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

    public static bool operator !=(Vector3I a, Vector3I b) => !(a == b);

    public static Vector3 operator +(Vector3I a, float b) => new Vector3((float) a.X + b, (float) a.Y + b, (float) a.Z + b);

    public static Vector3 operator *(Vector3I a, Vector3 b) => new Vector3((float) a.X * b.X, (float) a.Y * b.Y, (float) a.Z * b.Z);

    public static Vector3 operator *(Vector3 a, Vector3I b) => new Vector3(a.X * (float) b.X, a.Y * (float) b.Y, a.Z * (float) b.Z);

    public static Vector3D operator *(Vector3I a, Vector3D b) => new Vector3D((double) a.X * b.X, (double) a.Y * b.Y, (double) a.Z * b.Z);

    public static Vector3D operator *(Vector3D a, Vector3I b) => new Vector3D(a.X * (double) b.X, a.Y * (double) b.Y, a.Z * (double) b.Z);

    public static Vector3 operator *(float num, Vector3I b) => new Vector3(num * (float) b.X, num * (float) b.Y, num * (float) b.Z);

    public static Vector3 operator *(Vector3I a, float num) => new Vector3(num * (float) a.X, num * (float) a.Y, num * (float) a.Z);

    public static Vector3D operator *(double num, Vector3I b) => new Vector3D(num * (double) b.X, num * (double) b.Y, num * (double) b.Z);

    public static Vector3D operator *(Vector3I a, double num) => new Vector3D(num * (double) a.X, num * (double) a.Y, num * (double) a.Z);

    public static Vector3 operator /(Vector3I a, float num) => new Vector3((float) a.X / num, (float) a.Y / num, (float) a.Z / num);

    public static Vector3 operator /(float num, Vector3I a) => new Vector3(num / (float) a.X, num / (float) a.Y, num / (float) a.Z);

    public static Vector3I operator /(Vector3I a, int num) => new Vector3I(a.X / num, a.Y / num, a.Z / num);

    public static Vector3I operator /(Vector3I a, Vector3I b) => new Vector3I(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    public static Vector3I operator %(Vector3I a, int num) => new Vector3I(a.X % num, a.Y % num, a.Z % num);

    public static Vector3I operator >>(Vector3I v, int shift) => new Vector3I(v.X >> shift, v.Y >> shift, v.Z >> shift);

    public static Vector3I operator <<(Vector3I v, int shift) => new Vector3I(v.X << shift, v.Y << shift, v.Z << shift);

    public static Vector3I operator &(Vector3I v, int mask) => new Vector3I(v.X & mask, v.Y & mask, v.Z & mask);

    public static Vector3I operator |(Vector3I v, int mask) => new Vector3I(v.X | mask, v.Y | mask, v.Z | mask);

    public static Vector3I operator ^(Vector3I v, int mask) => new Vector3I(v.X ^ mask, v.Y ^ mask, v.Z ^ mask);

    public static Vector3I operator ~(Vector3I v) => new Vector3I(~v.X, ~v.Y, ~v.Z);

    public static Vector3I operator *(int num, Vector3I b) => new Vector3I(num * b.X, num * b.Y, num * b.Z);

    public static Vector3I operator *(Vector3I a, int num) => new Vector3I(num * a.X, num * a.Y, num * a.Z);

    public static Vector3I operator +(Vector3I a, Vector3I b) => new Vector3I(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3I operator +(Vector3I a, int b) => new Vector3I(a.X + b, a.Y + b, a.Z + b);

    public static Vector3I operator -(Vector3I a, Vector3I b) => new Vector3I(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3I operator -(Vector3I a, int b) => new Vector3I(a.X - b, a.Y - b, a.Z - b);

    public static Vector3I operator -(Vector3I a) => new Vector3I(-a.X, -a.Y, -a.Z);

    public static Vector3I Min(Vector3I value1, Vector3I value2)
    {
      Vector3I vector3I;
      vector3I.X = value1.X < value2.X ? value1.X : value2.X;
      vector3I.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
      vector3I.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
      return vector3I;
    }

    public static void Min(ref Vector3I value1, ref Vector3I value2, out Vector3I result)
    {
      result.X = value1.X < value2.X ? value1.X : value2.X;
      result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
      result.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
    }

    public int AbsMin() => Math.Abs(this.X) < Math.Abs(this.Y) ? (Math.Abs(this.X) < Math.Abs(this.Z) ? Math.Abs(this.X) : Math.Abs(this.Z)) : (Math.Abs(this.Y) < Math.Abs(this.Z) ? Math.Abs(this.Y) : Math.Abs(this.Z));

    public static Vector3I Max(Vector3I value1, Vector3I value2)
    {
      Vector3I vector3I;
      vector3I.X = value1.X > value2.X ? value1.X : value2.X;
      vector3I.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
      vector3I.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
      return vector3I;
    }

    public static void Max(ref Vector3I value1, ref Vector3I value2, out Vector3I result)
    {
      result.X = value1.X > value2.X ? value1.X : value2.X;
      result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
      result.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
    }

    public int AbsMax() => Math.Abs(this.X) > Math.Abs(this.Y) ? (Math.Abs(this.X) > Math.Abs(this.Z) ? Math.Abs(this.X) : Math.Abs(this.Z)) : (Math.Abs(this.Y) > Math.Abs(this.Z) ? Math.Abs(this.Y) : Math.Abs(this.Z));

    public static void MinMax(ref Vector3I min, ref Vector3I max)
    {
      if (min.X > max.X)
      {
        int x = min.X;
        min.X = max.X;
        max.X = x;
      }
      if (min.Y > max.Y)
      {
        int y = min.Y;
        min.Y = max.Y;
        max.Y = y;
      }
      if (min.Z <= max.Z)
        return;
      int z = min.Z;
      min.Z = max.Z;
      max.Z = z;
    }

    public int AxisValue(Base6Directions.Axis axis)
    {
      if (axis == Base6Directions.Axis.ForwardBackward)
        return this.Z;
      return axis == Base6Directions.Axis.LeftRight ? this.X : this.Y;
    }

    public static CubeFace GetDominantDirection(Vector3I val) => Math.Abs(val.X) > Math.Abs(val.Y) ? (Math.Abs(val.X) > Math.Abs(val.Z) ? (val.X > 0 ? CubeFace.Right : CubeFace.Left) : (val.Z > 0 ? CubeFace.Backward : CubeFace.Forward)) : (Math.Abs(val.Y) > Math.Abs(val.Z) ? (val.Y > 0 ? CubeFace.Up : CubeFace.Down) : (val.Z > 0 ? CubeFace.Backward : CubeFace.Forward));

    public static Vector3I GetDominantDirectionVector(Vector3I val)
    {
      if (Math.Abs(val.X) > Math.Abs(val.Y))
      {
        val.Y = 0;
        if (Math.Abs(val.X) > Math.Abs(val.Z))
        {
          val.Z = 0;
          val.X = val.X <= 0 ? -1 : 1;
        }
        else
        {
          val.X = 0;
          val.Z = val.Z <= 0 ? -1 : 1;
        }
      }
      else
      {
        val.X = 0;
        if (Math.Abs(val.Y) > Math.Abs(val.Z))
        {
          val.Z = 0;
          val.Y = val.Y <= 0 ? -1 : 1;
        }
        else
        {
          val.Y = 0;
          val.Z = val.Z <= 0 ? -1 : 1;
        }
      }
      return val;
    }

    public static Vector3I DominantAxisProjection(Vector3I value1)
    {
      if (Math.Abs(value1.X) > Math.Abs(value1.Y))
      {
        value1.Y = 0;
        if (Math.Abs(value1.X) > Math.Abs(value1.Z))
          value1.Z = 0;
        else
          value1.X = 0;
      }
      else
      {
        value1.X = 0;
        if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
          value1.Z = 0;
        else
          value1.Y = 0;
      }
      return value1;
    }

    public static void DominantAxisProjection(ref Vector3I value1, out Vector3I result)
    {
      if (Math.Abs(value1.X) > Math.Abs(value1.Y))
      {
        if (Math.Abs(value1.X) > Math.Abs(value1.Z))
          result = new Vector3I(value1.X, 0, 0);
        else
          result = new Vector3I(0, 0, value1.Z);
      }
      else if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
        result = new Vector3I(0, value1.Y, 0);
      else
        result = new Vector3I(0, 0, value1.Z);
    }

    public static Vector3I Sign(Vector3 value) => new Vector3I(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));

    public static Vector3I Sign(Vector3I value) => new Vector3I(Math.Sign(value.X), Math.Sign(value.Y), Math.Sign(value.Z));

    public static Vector3I Round(Vector3 value)
    {
      Vector3I r;
      Vector3I.Round(ref value, out r);
      return r;
    }

    public static Vector3I Round(Vector3D value)
    {
      Vector3I r;
      Vector3I.Round(ref value, out r);
      return r;
    }

    public static void Round(ref Vector3 v, out Vector3I r)
    {
      r.X = (int) Math.Round((double) v.X, MidpointRounding.AwayFromZero);
      r.Y = (int) Math.Round((double) v.Y, MidpointRounding.AwayFromZero);
      r.Z = (int) Math.Round((double) v.Z, MidpointRounding.AwayFromZero);
    }

    public static void Round(ref Vector3D v, out Vector3I r)
    {
      r.X = (int) Math.Round(v.X, MidpointRounding.AwayFromZero);
      r.Y = (int) Math.Round(v.Y, MidpointRounding.AwayFromZero);
      r.Z = (int) Math.Round(v.Z, MidpointRounding.AwayFromZero);
    }

    public static Vector3I Floor(Vector3 value) => new Vector3I((int) Math.Floor((double) value.X), (int) Math.Floor((double) value.Y), (int) Math.Floor((double) value.Z));

    public static Vector3I Floor(Vector3D value) => new Vector3I((int) Math.Floor(value.X), (int) Math.Floor(value.Y), (int) Math.Floor(value.Z));

    public static void Floor(ref Vector3 v, out Vector3I r)
    {
      r.X = (int) Math.Floor((double) v.X);
      r.Y = (int) Math.Floor((double) v.Y);
      r.Z = (int) Math.Floor((double) v.Z);
    }

    public static void Floor(ref Vector3D v, out Vector3I r)
    {
      r.X = (int) Math.Floor(v.X);
      r.Y = (int) Math.Floor(v.Y);
      r.Z = (int) Math.Floor(v.Z);
    }

    public static Vector3I Ceiling(Vector3 value) => new Vector3I((int) Math.Ceiling((double) value.X), (int) Math.Ceiling((double) value.Y), (int) Math.Ceiling((double) value.Z));

    public static Vector3I Trunc(Vector3 value) => new Vector3I((int) value.X, (int) value.Y, (int) value.Z);

    public static Vector3I Shift(Vector3I value) => new Vector3I(value.Z, value.X, value.Y);

    public static implicit operator Vector3(Vector3I value) => new Vector3((float) value.X, (float) value.Y, (float) value.Z);

    public static implicit operator Vector3D(Vector3I value) => new Vector3D((double) value.X, (double) value.Y, (double) value.Z);

    public static implicit operator Vector3L(Vector3I value) => new Vector3L((long) value.X, (long) value.Y, (long) value.Z);

    public static void Transform(ref Vector3I position, ref Matrix matrix, out Vector3I result)
    {
      int num1 = position.X * (int) Math.Round((double) matrix.M11) + position.Y * (int) Math.Round((double) matrix.M21) + position.Z * (int) Math.Round((double) matrix.M31) + (int) Math.Round((double) matrix.M41);
      int num2 = position.X * (int) Math.Round((double) matrix.M12) + position.Y * (int) Math.Round((double) matrix.M22) + position.Z * (int) Math.Round((double) matrix.M32) + (int) Math.Round((double) matrix.M42);
      int num3 = position.X * (int) Math.Round((double) matrix.M13) + position.Y * (int) Math.Round((double) matrix.M23) + position.Z * (int) Math.Round((double) matrix.M33) + (int) Math.Round((double) matrix.M43);
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void Transform(ref Vector3I value, ref Quaternion rotation, out Vector3I result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      float num8 = rotation.X * num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      float num13 = (float) ((double) value.X * (1.0 - (double) num10 - (double) num12) + (double) value.Y * ((double) num8 - (double) num6) + (double) value.Z * ((double) num9 + (double) num5));
      float num14 = (float) ((double) value.X * ((double) num8 + (double) num6) + (double) value.Y * (1.0 - (double) num7 - (double) num12) + (double) value.Z * ((double) num11 - (double) num4));
      float num15 = (float) ((double) value.X * ((double) num9 - (double) num5) + (double) value.Y * ((double) num11 + (double) num4) + (double) value.Z * (1.0 - (double) num7 - (double) num10));
      result.X = (int) Math.Round((double) num13);
      result.Y = (int) Math.Round((double) num14);
      result.Z = (int) Math.Round((double) num15);
    }

    public static Vector3I Transform(Vector3I value, Quaternion rotation)
    {
      Vector3I result;
      Vector3I.Transform(ref value, ref rotation, out result);
      return result;
    }

    public static void Transform(ref Vector3I value, ref MatrixI matrix, out Vector3I result) => result = value.X * Base6Directions.GetIntVector(matrix.Right) + value.Y * Base6Directions.GetIntVector(matrix.Up) + value.Z * Base6Directions.GetIntVector(matrix.Backward) + matrix.Translation;

    public static Vector3I Transform(Vector3I value, MatrixI transformation)
    {
      Vector3I result;
      Vector3I.Transform(ref value, ref transformation, out result);
      return result;
    }

    public static Vector3I Transform(Vector3I value, ref MatrixI transformation)
    {
      Vector3I result;
      Vector3I.Transform(ref value, ref transformation, out result);
      return result;
    }

    public static Vector3I TransformNormal(Vector3I value, ref MatrixI transformation)
    {
      Vector3I result;
      Vector3I.TransformNormal(ref value, ref transformation, out result);
      return result;
    }

    public static void TransformNormal(ref Vector3I normal, ref Matrix matrix, out Vector3I result)
    {
      int num1 = normal.X * (int) Math.Round((double) matrix.M11) + normal.Y * (int) Math.Round((double) matrix.M21) + normal.Z * (int) Math.Round((double) matrix.M31);
      int num2 = normal.X * (int) Math.Round((double) matrix.M12) + normal.Y * (int) Math.Round((double) matrix.M22) + normal.Z * (int) Math.Round((double) matrix.M32);
      int num3 = normal.X * (int) Math.Round((double) matrix.M13) + normal.Y * (int) Math.Round((double) matrix.M23) + normal.Z * (int) Math.Round((double) matrix.M33);
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void TransformNormal(
      ref Vector3I normal,
      ref MatrixI matrix,
      out Vector3I result)
    {
      result = normal.X * Base6Directions.GetIntVector(matrix.Right) + normal.Y * Base6Directions.GetIntVector(matrix.Up) + normal.Z * Base6Directions.GetIntVector(matrix.Backward);
    }

    public static void Cross(ref Vector3I vector1, ref Vector3I vector2, out Vector3I result)
    {
      int num1 = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
      int num2 = vector1.Z * vector2.X - vector1.X * vector2.Z;
      int num3 = vector1.X * vector2.Y - vector1.Y * vector2.X;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public int Size => Math.Abs(this.X * this.Y * this.Z);

    public long SizeLong => Math.Abs((long) this.X * (long) this.Y * (long) this.Z);

    public int CompareTo(Vector3I other)
    {
      int num1 = this.X - other.X;
      int num2 = this.Y - other.Y;
      int num3 = this.Z - other.Z;
      if (num1 != 0)
        return num1;
      return num2 == 0 ? num3 : num2;
    }

    public static Vector3I Abs(Vector3I value) => new Vector3I(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));

    public static void Abs(ref Vector3I value, out Vector3I result)
    {
      result.X = Math.Abs(value.X);
      result.Y = Math.Abs(value.Y);
      result.Z = Math.Abs(value.Z);
    }

    public static Vector3I Clamp(Vector3I value1, Vector3I min, Vector3I max)
    {
      Vector3I result;
      Vector3I.Clamp(ref value1, ref min, ref max, out result);
      return result;
    }

    public static void Clamp(
      ref Vector3I value1,
      ref Vector3I min,
      ref Vector3I max,
      out Vector3I result)
    {
      int x = value1.X;
      int num1 = x > max.X ? max.X : x;
      result.X = num1 < min.X ? min.X : num1;
      int y = value1.Y;
      int num2 = y > max.Y ? max.Y : y;
      result.Y = num2 < min.Y ? min.Y : num2;
      int z = value1.Z;
      int num3 = z > max.Z ? max.Z : z;
      result.Z = num3 < min.Z ? min.Z : num3;
    }

    public static int DistanceManhattan(Vector3I first, Vector3I second)
    {
      Vector3I vector3I = Vector3I.Abs(first - second);
      return vector3I.X + vector3I.Y + vector3I.Z;
    }

    public int Dot(ref Vector3I v) => this.X * v.X + this.Y * v.Y + this.Z * v.Z;

    public static int Dot(Vector3I vector1, Vector3I vector2) => Vector3I.Dot(ref vector1, ref vector2);

    public static int Dot(ref Vector3I vector1, ref Vector3I vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

    public static void Dot(ref Vector3I vector1, ref Vector3I vector2, out int dot) => dot = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

    public static bool TryParseFromString(string p, out Vector3I vec)
    {
      string[] strArray = p.Split(';');
      if (strArray.Length != 3)
      {
        vec = Vector3I.Zero;
        return false;
      }
      try
      {
        vec.X = int.Parse(strArray[0]);
        vec.Y = int.Parse(strArray[1]);
        vec.Z = int.Parse(strArray[2]);
      }
      catch (FormatException ex)
      {
        vec = Vector3I.Zero;
        return false;
      }
      return true;
    }

    public int Volume() => this.X * this.Y * this.Z;

    public static IEnumerable<Vector3I> EnumerateRange(
      Vector3I minInclusive,
      Vector3I maxExclusive)
    {
      Vector3I vec;
      for (vec.Z = minInclusive.Z; vec.Z < maxExclusive.Z; ++vec.Z)
      {
        for (vec.Y = minInclusive.Y; vec.Y < maxExclusive.Y; ++vec.Y)
        {
          for (vec.X = minInclusive.X; vec.X < maxExclusive.X; ++vec.X)
            yield return vec;
        }
      }
    }

    public void ToBytes(List<byte> result)
    {
      result.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.X));
      result.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Y));
      result.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Z));
    }

    public bool IsAxisAligned()
    {
      int num = 0;
      if (this.X == 0)
        ++num;
      if (this.Y == 0)
        ++num;
      if (this.Z == 0)
        ++num;
      return num == 2;
    }

    public class EqualityComparer : IEqualityComparer<Vector3I>, IComparer<Vector3I>
    {
      public bool Equals(Vector3I x, Vector3I y) => x.X == y.X & x.Y == y.Y & x.Z == y.Z;

      public int GetHashCode(Vector3I obj) => (obj.X * 397 ^ obj.Y) * 397 ^ obj.Z;

      public int Compare(Vector3I x, Vector3I y) => x.CompareTo(y);
    }

    protected class VRageMath_Vector3I\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3I owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3I owner, out int value) => value = owner.X;
    }

    protected class VRageMath_Vector3I\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3I owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3I owner, out int value) => value = owner.Y;
    }

    protected class VRageMath_Vector3I\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3I owner, in int value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3I owner, out int value) => value = owner.Z;
    }
  }
}
