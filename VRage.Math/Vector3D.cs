// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3D
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct Vector3D : IEquatable<Vector3D>
  {
    public static Vector3D Zero = new Vector3D();
    public static Vector3D One = new Vector3D(1.0, 1.0, 1.0);
    public static Vector3D Half = new Vector3D(0.5, 0.5, 0.5);
    public static Vector3D PositiveInfinity = new Vector3D(double.PositiveInfinity);
    public static Vector3D NegativeInfinity = new Vector3D(double.NegativeInfinity);
    public static Vector3D UnitX = new Vector3D(1.0, 0.0, 0.0);
    public static Vector3D UnitY = new Vector3D(0.0, 1.0, 0.0);
    public static Vector3D UnitZ = new Vector3D(0.0, 0.0, 1.0);
    public static Vector3D Up = new Vector3D(0.0, 1.0, 0.0);
    public static Vector3D Down = new Vector3D(0.0, -1.0, 0.0);
    public static Vector3D Right = new Vector3D(1.0, 0.0, 0.0);
    public static Vector3D Left = new Vector3D(-1.0, 0.0, 0.0);
    public static Vector3D Forward = new Vector3D(0.0, 0.0, -1.0);
    public static Vector3D Backward = new Vector3D(0.0, 0.0, 1.0);
    public static Vector3D MaxValue = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
    public static Vector3D MinValue = new Vector3D(double.MinValue, double.MinValue, double.MinValue);
    [ProtoMember(1)]
    public double X;
    [ProtoMember(4)]
    public double Y;
    [ProtoMember(7)]
    public double Z;

    public Vector3D(double x, double y, double z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3D(double value) => this.X = this.Y = this.Z = value;

    public Vector3D(Vector2 value, double z)
    {
      this.X = (double) value.X;
      this.Y = (double) value.Y;
      this.Z = z;
    }

    public Vector3D(Vector2D value, double z)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = z;
    }

    public Vector3D(Vector4 xyz)
    {
      this.X = (double) xyz.X;
      this.Y = (double) xyz.Y;
      this.Z = (double) xyz.Z;
    }

    public Vector3D(Vector4D xyz)
    {
      this.X = xyz.X;
      this.Y = xyz.Y;
      this.Z = xyz.Z;
    }

    public Vector3D(Vector3 value)
    {
      this.X = (double) value.X;
      this.Y = (double) value.Y;
      this.Z = (double) value.Z;
    }

    public Vector3D(ref Vector3I value)
    {
      this.X = (double) value.X;
      this.Y = (double) value.Y;
      this.Z = (double) value.Z;
    }

    public Vector3D(Vector3I value)
    {
      this.X = (double) value.X;
      this.Y = (double) value.Y;
      this.Z = (double) value.Z;
    }

    public Vector3D(Vector3D value)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = value.Z;
    }

    public static Vector3D operator -(Vector3D value)
    {
      Vector3D vector3D;
      vector3D.X = -value.X;
      vector3D.Y = -value.Y;
      vector3D.Z = -value.Z;
      return vector3D;
    }

    public static bool operator ==(Vector3D value1, Vector3D value2) => value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;

    public static bool operator ==(Vector3 value1, Vector3D value2) => (double) value1.X == value2.X && (double) value1.Y == value2.Y && (double) value1.Z == value2.Z;

    public static bool operator ==(Vector3D value1, Vector3 value2) => value1.X == (double) value2.X && value1.Y == (double) value2.Y && value1.Z == (double) value2.Z;

    public static bool operator !=(Vector3D value1, Vector3D value2) => value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;

    public static bool operator !=(Vector3 value1, Vector3D value2) => (double) value1.X != value2.X || (double) value1.Y != value2.Y || (double) value1.Z != value2.Z;

    public static bool operator !=(Vector3D value1, Vector3 value2) => value1.X != (double) value2.X || value1.Y != (double) value2.Y || value1.Z != (double) value2.Z;

    public static Vector3D operator %(Vector3D value1, double value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X % value2;
      vector3D.Y = value1.Y % value2;
      vector3D.Z = value1.Z % value2;
      return vector3D;
    }

    public static Vector3D operator %(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X % value2.X;
      vector3D.Y = value1.Y % value2.Y;
      vector3D.Z = value1.Z % value2.Z;
      return vector3D;
    }

    public static Vector3D operator +(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + value2.X;
      vector3D.Y = value1.Y + value2.Y;
      vector3D.Z = value1.Z + value2.Z;
      return vector3D;
    }

    public static Vector3D operator +(Vector3D value1, Vector3 value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + (double) value2.X;
      vector3D.Y = value1.Y + (double) value2.Y;
      vector3D.Z = value1.Z + (double) value2.Z;
      return vector3D;
    }

    public static Vector3D operator +(Vector3 value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = (double) value1.X + value2.X;
      vector3D.Y = (double) value1.Y + value2.Y;
      vector3D.Z = (double) value1.Z + value2.Z;
      return vector3D;
    }

    public static Vector3D operator +(Vector3D value1, Vector3I value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + (double) value2.X;
      vector3D.Y = value1.Y + (double) value2.Y;
      vector3D.Z = value1.Z + (double) value2.Z;
      return vector3D;
    }

    public static Vector3D operator +(Vector3D value1, double value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + value2;
      vector3D.Y = value1.Y + value2;
      vector3D.Z = value1.Z + value2;
      return vector3D;
    }

    public static Vector3D operator +(Vector3D value1, float value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + (double) value2;
      vector3D.Y = value1.Y + (double) value2;
      vector3D.Z = value1.Z + (double) value2;
      return vector3D;
    }

    public static Vector3D operator -(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X - value2.X;
      vector3D.Y = value1.Y - value2.Y;
      vector3D.Z = value1.Z - value2.Z;
      return vector3D;
    }

    public static Vector3D operator -(Vector3D value1, Vector3 value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X - (double) value2.X;
      vector3D.Y = value1.Y - (double) value2.Y;
      vector3D.Z = value1.Z - (double) value2.Z;
      return vector3D;
    }

    public static Vector3D operator -(Vector3 value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = (double) value1.X - value2.X;
      vector3D.Y = (double) value1.Y - value2.Y;
      vector3D.Z = (double) value1.Z - value2.Z;
      return vector3D;
    }

    public static Vector3D operator -(Vector3D value1, double value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X - value2;
      vector3D.Y = value1.Y - value2;
      vector3D.Z = value1.Z - value2;
      return vector3D;
    }

    public static Vector3D operator *(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X * value2.X;
      vector3D.Y = value1.Y * value2.Y;
      vector3D.Z = value1.Z * value2.Z;
      return vector3D;
    }

    public static Vector3D operator *(Vector3D value1, Vector3 value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X * (double) value2.X;
      vector3D.Y = value1.Y * (double) value2.Y;
      vector3D.Z = value1.Z * (double) value2.Z;
      return vector3D;
    }

    public static Vector3D operator *(Vector3D value, double scaleFactor)
    {
      Vector3D vector3D;
      vector3D.X = value.X * scaleFactor;
      vector3D.Y = value.Y * scaleFactor;
      vector3D.Z = value.Z * scaleFactor;
      return vector3D;
    }

    public static Vector3D operator *(double scaleFactor, Vector3D value)
    {
      Vector3D vector3D;
      vector3D.X = value.X * scaleFactor;
      vector3D.Y = value.Y * scaleFactor;
      vector3D.Z = value.Z * scaleFactor;
      return vector3D;
    }

    public static Vector3D operator /(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X / value2.X;
      vector3D.Y = value1.Y / value2.Y;
      vector3D.Z = value1.Z / value2.Z;
      return vector3D;
    }

    public static Vector3D operator /(Vector3D value, double divider)
    {
      double num = 1.0 / divider;
      Vector3D vector3D;
      vector3D.X = value.X * num;
      vector3D.Y = value.Y * num;
      vector3D.Z = value.Z * num;
      return vector3D;
    }

    public static Vector3D operator /(double value, Vector3D divider)
    {
      Vector3D vector3D;
      vector3D.X = value / divider.X;
      vector3D.Y = value / divider.Y;
      vector3D.Z = value / divider.Z;
      return vector3D;
    }

    public static Vector3D Abs(Vector3D value) => new Vector3D(value.X < 0.0 ? -value.X : value.X, value.Y < 0.0 ? -value.Y : value.Y, value.Z < 0.0 ? -value.Z : value.Z);

    public static Vector3D Sign(Vector3D value) => new Vector3D((double) Math.Sign(value.X), (double) Math.Sign(value.Y), (double) Math.Sign(value.Z));

    public static Vector3D SignNonZero(Vector3D value) => new Vector3D(value.X < 0.0 ? -1.0 : 1.0, value.Y < 0.0 ? -1.0 : 1.0, value.Z < 0.0 ? -1.0 : 1.0);

    public void Interpolate3(Vector3D v0, Vector3D v1, double rt)
    {
      double num = 1.0 - rt;
      this.X = num * v0.X + rt * v1.X;
      this.Y = num * v0.Y + rt * v1.Y;
      this.Z = num * v0.Z + rt * v1.Z;
    }

    public bool IsValid() => this.X.IsValid() && this.Y.IsValid() && this.Z.IsValid();

    [Conditional("DEBUG")]
    public void AssertIsValid()
    {
    }

    public static bool IsUnit(ref Vector3D value)
    {
      double num = value.LengthSquared();
      return num >= 0.999899983406067 && num < 1.0001;
    }

    public static bool ArePerpendicular(ref Vector3D a, ref Vector3D b)
    {
      double num = a.Dot(b);
      return num * num < 1E-08 * a.LengthSquared() * b.LengthSquared();
    }

    public static bool IsZero(Vector3D value) => Vector3D.IsZero(value, 0.0001);

    public bool IsZero() => Vector3D.IsZero(this, 0.0001);

    public static bool IsZero(Vector3D value, double epsilon) => Math.Abs(value.X) < epsilon && Math.Abs(value.Y) < epsilon && Math.Abs(value.Z) < epsilon;

    public static Vector3D IsZeroVector(Vector3D value) => new Vector3D(value.X == 0.0 ? 1.0 : 0.0, value.Y == 0.0 ? 1.0 : 0.0, value.Z == 0.0 ? 1.0 : 0.0);

    public static Vector3D IsZeroVector(Vector3D value, double epsilon) => new Vector3D(Math.Abs(value.X) < epsilon ? 1.0 : 0.0, Math.Abs(value.Y) < epsilon ? 1.0 : 0.0, Math.Abs(value.Z) < epsilon ? 1.0 : 0.0);

    public static Vector3D Step(Vector3D value) => new Vector3D(value.X > 0.0 ? 1.0 : (value.X < 0.0 ? -1.0 : 0.0), value.Y > 0.0 ? 1.0 : (value.Y < 0.0 ? -1.0 : 0.0), value.Z > 0.0 ? 1.0 : (value.Z < 0.0 ? -1.0 : 0.0));

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "X:{0} Y:{1} Z:{2}", (object) this.X.ToString((IFormatProvider) currentCulture), (object) this.Y.ToString((IFormatProvider) currentCulture), (object) this.Z.ToString((IFormatProvider) currentCulture));
    }

    public static bool TryParse(string str, out Vector3D retval)
    {
      retval = Vector3D.Zero;
      if (string.IsNullOrWhiteSpace(str))
        return false;
      string[] strArray = str.ToLower().Split(' ');
      if (strArray.Length != 3)
        return false;
      double result1 = 0.0;
      if (!double.TryParse(strArray[0].Replace("x:", ""), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
        return false;
      double result2 = 0.0;
      if (!double.TryParse(strArray[1].Replace("y:", ""), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
        return false;
      double result3 = 0.0;
      if (!double.TryParse(strArray[2].Replace("z:", ""), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
        return false;
      retval = new Vector3D(result1, result2, result3);
      return true;
    }

    public string ToString(string format)
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1} Z:{2}}}", (object) this.X.ToString(format, (IFormatProvider) currentCulture), (object) this.Y.ToString(format, (IFormatProvider) currentCulture), (object) this.Z.ToString(format, (IFormatProvider) currentCulture));
    }

    public bool Equals(Vector3D other) => this.X == other.X && this.Y == other.Y && this.Z == other.Z;

    public bool Equals(Vector3D other, double epsilon) => Math.Abs(this.X - other.X) < epsilon && Math.Abs(this.Y - other.Y) < epsilon && Math.Abs(this.Z - other.Z) < epsilon;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Vector3D other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => ((int) (this.X * 997.0) * 397 ^ (int) (this.Y * 997.0)) * 397 ^ (int) (this.Z * 997.0);

    public long GetHash()
    {
      long num1 = (long) Math.Round(Math.Abs(this.X * 1000.0));
      int num2 = 2;
      long num3 = num1 * 397L ^ (long) Math.Round(Math.Abs(this.Y * 1000.0));
      int num4 = num2 + 4;
      long num5 = num3 * 397L ^ (long) Math.Round(Math.Abs(this.Z * 1000.0));
      int num6 = num4 + 16;
      long num7 = num5 * 397L ^ (long) (Math.Sign(this.X) + 5);
      int num8 = num6 + 256;
      long num9 = num7 * 397L ^ (long) (Math.Sign(this.Y) + 7);
      int num10 = num8 + 65536;
      return (num9 * 397L ^ (long) (Math.Sign(this.Z) + 11)) * 397L ^ (long) (num10 + 1);
    }

    public double Length() => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);

    public double LengthSquared() => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

    public static double Distance(Vector3D value1, Vector3D value2)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      double num3 = value1.Z - value2.Z;
      return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
    }

    public static double Distance(Vector3D value1, Vector3 value2)
    {
      double num1 = value1.X - (double) value2.X;
      double num2 = value1.Y - (double) value2.Y;
      double num3 = value1.Z - (double) value2.Z;
      return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
    }

    public static double Distance(Vector3 value1, Vector3D value2)
    {
      double num1 = (double) value1.X - value2.X;
      double num2 = (double) value1.Y - value2.Y;
      double num3 = (double) value1.Z - value2.Z;
      return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
    }

    public static void Distance(ref Vector3D value1, ref Vector3D value2, out double result)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      double num3 = value1.Z - value2.Z;
      double d = num1 * num1 + num2 * num2 + num3 * num3;
      result = Math.Sqrt(d);
    }

    public static double DistanceSquared(Vector3D value1, Vector3D value2)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      double num3 = value1.Z - value2.Z;
      return num1 * num1 + num2 * num2 + num3 * num3;
    }

    public static void DistanceSquared(ref Vector3D value1, ref Vector3D value2, out double result)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      double num3 = value1.Z - value2.Z;
      result = num1 * num1 + num2 * num2 + num3 * num3;
    }

    public static double RectangularDistance(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D = Vector3D.Abs(value1 - value2);
      return vector3D.X + vector3D.Y + vector3D.Z;
    }

    public static double RectangularDistance(ref Vector3D value1, ref Vector3D value2)
    {
      Vector3D vector3D = Vector3D.Abs(value1 - value2);
      return vector3D.X + vector3D.Y + vector3D.Z;
    }

    public static double Dot(Vector3D vector1, Vector3D vector2) => vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

    public static double Dot(Vector3D vector1, Vector3 vector2) => vector1.X * (double) vector2.X + vector1.Y * (double) vector2.Y + vector1.Z * (double) vector2.Z;

    public static void Dot(ref Vector3D vector1, ref Vector3D vector2, out double result) => result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;

    public static void Dot(ref Vector3D vector1, ref Vector3 vector2, out double result) => result = vector1.X * (double) vector2.X + vector1.Y * (double) vector2.Y + vector1.Z * (double) vector2.Z;

    public static void Dot(ref Vector3 vector1, ref Vector3D vector2, out double result) => result = (double) vector1.X * vector2.X + (double) vector1.Y * vector2.Y + (double) vector1.Z * vector2.Z;

    public double Dot(Vector3D v) => Vector3D.Dot(this, v);

    public double Dot(Vector3 v) => this.X * (double) v.X + this.Y * (double) v.Y + this.Z * (double) v.Z;

    public double Dot(ref Vector3D v) => this.X * v.X + this.Y * v.Y + this.Z * v.Z;

    public Vector3D Cross(Vector3D v) => Vector3D.Cross(this, v);

    public double Normalize()
    {
      double num1 = Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
      double num2 = 1.0 / num1;
      this.X *= num2;
      this.Y *= num2;
      this.Z *= num2;
      return num1;
    }

    public static Vector3D Normalize(Vector3D value)
    {
      double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
      Vector3D vector3D;
      vector3D.X = value.X * num;
      vector3D.Y = value.Y * num;
      vector3D.Z = value.Z * num;
      return vector3D;
    }

    public static void Normalize(ref Vector3D value, out Vector3D result)
    {
      double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
      result.X = value.X * num;
      result.Y = value.Y * num;
      result.Z = value.Z * num;
    }

    public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
    {
      Vector3D vector3D;
      vector3D.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
      vector3D.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
      vector3D.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
      return vector3D;
    }

    public static void Cross(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
    {
      double num1 = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
      double num2 = vector1.Z * vector2.X - vector1.X * vector2.Z;
      double num3 = vector1.X * vector2.Y - vector1.Y * vector2.X;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static Vector3D Reflect(Vector3D vector, Vector3D normal)
    {
      double num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
      Vector3D vector3D;
      vector3D.X = vector.X - 2.0 * num * normal.X;
      vector3D.Y = vector.Y - 2.0 * num * normal.Y;
      vector3D.Z = vector.Z - 2.0 * num * normal.Z;
      return vector3D;
    }

    public static void Reflect(ref Vector3D vector, ref Vector3D normal, out Vector3D result)
    {
      double num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
      result.X = vector.X - 2.0 * num * normal.X;
      result.Y = vector.Y - 2.0 * num * normal.Y;
      result.Z = vector.Z - 2.0 * num * normal.Z;
    }

    public static Vector3D Reject(Vector3D vector, Vector3D direction)
    {
      Vector3D result;
      Vector3D.Reject(ref vector, ref direction, out result);
      return result;
    }

    public static void Reject(ref Vector3D vector, ref Vector3D direction, out Vector3D result)
    {
      double result1;
      Vector3D.Dot(ref direction, ref direction, out result1);
      double num1 = 1.0 / result1;
      double result2;
      Vector3D.Dot(ref direction, ref vector, out result2);
      double num2 = result2 * num1;
      Vector3D vector3D;
      vector3D.X = direction.X * num1;
      vector3D.Y = direction.Y * num1;
      vector3D.Z = direction.Z * num1;
      result.X = vector.X - num2 * vector3D.X;
      result.Y = vector.Y - num2 * vector3D.Y;
      result.Z = vector.Z - num2 * vector3D.Z;
    }

    public double Min() => this.X < this.Y ? (this.X < this.Z ? this.X : this.Z) : (this.Y < this.Z ? this.Y : this.Z);

    public double AbsMin() => Math.Abs(this.X) < Math.Abs(this.Y) ? (Math.Abs(this.X) < Math.Abs(this.Z) ? Math.Abs(this.X) : Math.Abs(this.Z)) : (Math.Abs(this.Y) < Math.Abs(this.Z) ? Math.Abs(this.Y) : Math.Abs(this.Z));

    public double Max() => this.X > this.Y ? (this.X > this.Z ? this.X : this.Z) : (this.Y > this.Z ? this.Y : this.Z);

    public double AbsMax() => Math.Abs(this.X) > Math.Abs(this.Y) ? (Math.Abs(this.X) > Math.Abs(this.Z) ? Math.Abs(this.X) : Math.Abs(this.Z)) : (Math.Abs(this.Y) > Math.Abs(this.Z) ? Math.Abs(this.Y) : Math.Abs(this.Z));

    public int AbsMaxComponent() => Math.Abs(this.X) > Math.Abs(this.Y) ? (Math.Abs(this.X) > Math.Abs(this.Z) ? 0 : 2) : (Math.Abs(this.Y) > Math.Abs(this.Z) ? 1 : 2);

    public static Vector3D Min(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X < value2.X ? value1.X : value2.X;
      vector3D.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
      vector3D.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
      return vector3D;
    }

    public static void Min(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X < value2.X ? value1.X : value2.X;
      result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
      result.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
    }

    public static Vector3D Max(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X > value2.X ? value1.X : value2.X;
      vector3D.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
      vector3D.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
      return vector3D;
    }

    public static void Max(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X > value2.X ? value1.X : value2.X;
      result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
      result.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
    }

    public static void MinMax(ref Vector3D min, ref Vector3D max)
    {
      if (min.X > max.X)
      {
        double x = min.X;
        min.X = max.X;
        max.X = x;
      }
      if (min.Y > max.Y)
      {
        double y = min.Y;
        min.Y = max.Y;
        max.Y = y;
      }
      if (min.Z <= max.Z)
        return;
      double z = min.Z;
      min.Z = max.Z;
      max.Z = z;
    }

    public static Vector3D DominantAxisProjection(Vector3D value1)
    {
      if (Math.Abs(value1.X) > Math.Abs(value1.Y))
      {
        value1.Y = 0.0;
        if (Math.Abs(value1.X) > Math.Abs(value1.Z))
          value1.Z = 0.0;
        else
          value1.X = 0.0;
      }
      else
      {
        value1.X = 0.0;
        if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
          value1.Z = 0.0;
        else
          value1.Y = 0.0;
      }
      return value1;
    }

    public static void DominantAxisProjection(ref Vector3D value1, out Vector3D result)
    {
      if (Math.Abs(value1.X) > Math.Abs(value1.Y))
      {
        if (Math.Abs(value1.X) > Math.Abs(value1.Z))
          result = new Vector3D(value1.X, 0.0, 0.0);
        else
          result = new Vector3D(0.0, 0.0, value1.Z);
      }
      else if (Math.Abs(value1.Y) > Math.Abs(value1.Z))
        result = new Vector3D(0.0, value1.Y, 0.0);
      else
        result = new Vector3D(0.0, 0.0, value1.Z);
    }

    public static Vector3D Clamp(Vector3D value1, Vector3D min, Vector3D max)
    {
      Vector3D result;
      Vector3D.Clamp(ref value1, ref min, ref max, out result);
      return result;
    }

    public static void Clamp(
      ref Vector3D value1,
      ref Vector3D min,
      ref Vector3D max,
      out Vector3D result)
    {
      double x = value1.X;
      double num1 = x > max.X ? max.X : (x < min.X ? min.X : x);
      double y = value1.Y;
      double num2 = y > max.Y ? max.Y : (y < min.Y ? min.Y : y);
      double z = value1.Z;
      double num3 = z > max.Z ? max.Z : (z < min.Z ? min.Z : z);
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static Vector3D ClampToSphere(Vector3D vector, double radius)
    {
      double num1 = vector.LengthSquared();
      double num2 = radius * radius;
      return num1 > num2 ? vector * Math.Sqrt(num2 / num1) : vector;
    }

    public static void ClampToSphere(ref Vector3D vector, double radius)
    {
      double num1 = vector.LengthSquared();
      double num2 = radius * radius;
      if (num1 <= num2)
        return;
      vector *= Math.Sqrt(num2 / num1);
    }

    public static Vector3D Lerp(Vector3D value1, Vector3D value2, double amount)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + (value2.X - value1.X) * amount;
      vector3D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      vector3D.Z = value1.Z + (value2.Z - value1.Z) * amount;
      return vector3D;
    }

    public static void Lerp(
      ref Vector3D value1,
      ref Vector3D value2,
      double amount,
      out Vector3D result)
    {
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
      result.Z = value1.Z + (value2.Z - value1.Z) * amount;
    }

    public static Vector3D Barycentric(
      Vector3D value1,
      Vector3D value2,
      Vector3D value3,
      double amount1,
      double amount2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      vector3D.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
      vector3D.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
      return vector3D;
    }

    public static void Barycentric(
      ref Vector3D value1,
      ref Vector3D value2,
      ref Vector3D value3,
      double amount1,
      double amount2,
      out Vector3D result)
    {
      result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
      result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
    }

    public static void Barycentric(
      Vector3D p,
      Vector3D a,
      Vector3D b,
      Vector3D c,
      out double u,
      out double v,
      out double w)
    {
      Vector3D vector3D1 = b - a;
      Vector3D vector3D2 = c - a;
      Vector3D vector1 = p - a;
      double num1 = Vector3D.Dot(vector3D1, vector3D1);
      double num2 = Vector3D.Dot(vector3D1, vector3D2);
      double num3 = Vector3D.Dot(vector3D2, vector3D2);
      double num4 = Vector3D.Dot(vector1, vector3D1);
      double num5 = Vector3D.Dot(vector1, vector3D2);
      double num6 = num1 * num3 - num2 * num2;
      v = (num3 * num4 - num2 * num5) / num6;
      w = (num1 * num5 - num2 * num4) / num6;
      u = 1.0 - v - w;
    }

    public static Vector3D SmoothStep(Vector3D value1, Vector3D value2, double amount)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      Vector3D vector3D;
      vector3D.X = value1.X + (value2.X - value1.X) * amount;
      vector3D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      vector3D.Z = value1.Z + (value2.Z - value1.Z) * amount;
      return vector3D;
    }

    public static void SmoothStep(
      ref Vector3D value1,
      ref Vector3D value2,
      double amount,
      out Vector3D result)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
      result.Z = value1.Z + (value2.Z - value1.Z) * amount;
    }

    public static Vector3D CatmullRom(
      Vector3D value1,
      Vector3D value2,
      Vector3D value3,
      Vector3D value4,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      Vector3D vector3D;
      vector3D.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      vector3D.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
      vector3D.Z = 0.5 * (2.0 * value2.Z + (-value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num1 + (-value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
      return vector3D;
    }

    public static void CatmullRom(
      ref Vector3D value1,
      ref Vector3D value2,
      ref Vector3D value3,
      ref Vector3D value4,
      double amount,
      out Vector3D result)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      result.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      result.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
      result.Z = 0.5 * (2.0 * value2.Z + (-value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num1 + (-value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
    }

    public static Vector3D Hermite(
      Vector3D value1,
      Vector3D tangent1,
      Vector3D value2,
      Vector3D tangent2,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      double num3 = 2.0 * num2 - 3.0 * num1 + 1.0;
      double num4 = -2.0 * num2 + 3.0 * num1;
      double num5 = num2 - 2.0 * num1 + amount;
      double num6 = num2 - num1;
      Vector3D vector3D;
      vector3D.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
      vector3D.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
      vector3D.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
      return vector3D;
    }

    public static void Hermite(
      ref Vector3D value1,
      ref Vector3D tangent1,
      ref Vector3D value2,
      ref Vector3D tangent2,
      double amount,
      out Vector3D result)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      double num3 = 2.0 * num2 - 3.0 * num1 + 1.0;
      double num4 = -2.0 * num2 + 3.0 * num1;
      double num5 = num2 - 2.0 * num1 + amount;
      double num6 = num2 - num1;
      result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
      result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
      result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
    }

    public static Vector3D Transform(Vector3D position, MatrixD matrix)
    {
      double num1 = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
      double num2 = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
      double num3 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
      double num4 = 1.0 / (position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
      Vector3D vector3D;
      vector3D.X = num1 * num4;
      vector3D.Y = num2 * num4;
      vector3D.Z = num3 * num4;
      return vector3D;
    }

    public static Vector3D Transform(Vector3 position, MatrixD matrix)
    {
      double num1 = (double) position.X * matrix.M11 + (double) position.Y * matrix.M21 + (double) position.Z * matrix.M31 + matrix.M41;
      double num2 = (double) position.X * matrix.M12 + (double) position.Y * matrix.M22 + (double) position.Z * matrix.M32 + matrix.M42;
      double num3 = (double) position.X * matrix.M13 + (double) position.Y * matrix.M23 + (double) position.Z * matrix.M33 + matrix.M43;
      double num4 = 1.0 / ((double) position.X * matrix.M14 + (double) position.Y * matrix.M24 + (double) position.Z * matrix.M34 + matrix.M44);
      Vector3D vector3D;
      vector3D.X = num1 * num4;
      vector3D.Y = num2 * num4;
      vector3D.Z = num3 * num4;
      return vector3D;
    }

    public static Vector3D Transform(Vector3D position, Matrix matrix)
    {
      double num1 = position.X * (double) matrix.M11 + position.Y * (double) matrix.M21 + position.Z * (double) matrix.M31 + (double) matrix.M41;
      double num2 = position.X * (double) matrix.M12 + position.Y * (double) matrix.M22 + position.Z * (double) matrix.M32 + (double) matrix.M42;
      double num3 = position.X * (double) matrix.M13 + position.Y * (double) matrix.M23 + position.Z * (double) matrix.M33 + (double) matrix.M43;
      double num4 = 1.0 / (position.X * (double) matrix.M14 + position.Y * (double) matrix.M24 + position.Z * (double) matrix.M34 + (double) matrix.M44);
      Vector3D vector3D;
      vector3D.X = num1 * num4;
      vector3D.Y = num2 * num4;
      vector3D.Z = num3 * num4;
      return vector3D;
    }

    public static Vector3D Transform(Vector3D position, ref MatrixD matrix)
    {
      Vector3D result;
      Vector3D.Transform(ref position, ref matrix, out result);
      return result;
    }

    public static void Transform(ref Vector3D position, ref MatrixD matrix, out Vector3D result)
    {
      double num1 = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
      double num2 = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
      double num3 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
      double num4 = 1.0 / (position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
      result.X = num1 * num4;
      result.Y = num2 * num4;
      result.Z = num3 * num4;
    }

    public static void Transform(ref Vector3 position, ref MatrixD matrix, out Vector3D result)
    {
      double num1 = (double) position.X * matrix.M11 + (double) position.Y * matrix.M21 + (double) position.Z * matrix.M31 + matrix.M41;
      double num2 = (double) position.X * matrix.M12 + (double) position.Y * matrix.M22 + (double) position.Z * matrix.M32 + matrix.M42;
      double num3 = (double) position.X * matrix.M13 + (double) position.Y * matrix.M23 + (double) position.Z * matrix.M33 + matrix.M43;
      double num4 = 1.0 / ((double) position.X * matrix.M14 + (double) position.Y * matrix.M24 + (double) position.Z * matrix.M34 + matrix.M44);
      result.X = num1 * num4;
      result.Y = num2 * num4;
      result.Z = num3 * num4;
    }

    public static void TransformNoProjection(
      ref Vector3D vector,
      ref MatrixD matrix,
      out Vector3D result)
    {
      double num1 = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41;
      double num2 = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42;
      double num3 = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void RotateAndScale(ref Vector3D vector, ref MatrixD matrix, out Vector3D result)
    {
      double num1 = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31;
      double num2 = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32;
      double num3 = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void Transform(ref Vector3D position, ref MatrixI matrix, out Vector3D result) => result = position.X * new Vector3D(Base6Directions.GetVector(matrix.Right)) + position.Y * new Vector3D(Base6Directions.GetVector(matrix.Up)) + position.Z * new Vector3D(Base6Directions.GetVector(matrix.Backward)) + new Vector3D(matrix.Translation);

    public static Vector3D TransformNormal(Vector3D normal, Matrix matrix)
    {
      double num1 = normal.X * (double) matrix.M11 + normal.Y * (double) matrix.M21 + normal.Z * (double) matrix.M31;
      double num2 = normal.X * (double) matrix.M12 + normal.Y * (double) matrix.M22 + normal.Z * (double) matrix.M32;
      double num3 = normal.X * (double) matrix.M13 + normal.Y * (double) matrix.M23 + normal.Z * (double) matrix.M33;
      Vector3D vector3D;
      vector3D.X = num1;
      vector3D.Y = num2;
      vector3D.Z = num3;
      return vector3D;
    }

    public static Vector3D TransformNormal(Vector3 normal, MatrixD matrix)
    {
      double num1 = (double) normal.X * matrix.M11 + (double) normal.Y * matrix.M21 + (double) normal.Z * matrix.M31;
      double num2 = (double) normal.X * matrix.M12 + (double) normal.Y * matrix.M22 + (double) normal.Z * matrix.M32;
      double num3 = (double) normal.X * matrix.M13 + (double) normal.Y * matrix.M23 + (double) normal.Z * matrix.M33;
      Vector3D vector3D;
      vector3D.X = num1;
      vector3D.Y = num2;
      vector3D.Z = num3;
      return vector3D;
    }

    public static Vector3D TransformNormal(Vector3D normal, MatrixD matrix)
    {
      double num1 = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
      double num2 = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
      double num3 = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
      Vector3D vector3D;
      vector3D.X = num1;
      vector3D.Y = num2;
      vector3D.Z = num3;
      return vector3D;
    }

    public static void TransformNormal(
      ref Vector3D normal,
      ref MatrixD matrix,
      out Vector3D result)
    {
      double num1 = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
      double num2 = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
      double num3 = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void TransformNormal(ref Vector3 normal, ref MatrixD matrix, out Vector3D result)
    {
      double num1 = (double) normal.X * matrix.M11 + (double) normal.Y * matrix.M21 + (double) normal.Z * matrix.M31;
      double num2 = (double) normal.X * matrix.M12 + (double) normal.Y * matrix.M22 + (double) normal.Z * matrix.M32;
      double num3 = (double) normal.X * matrix.M13 + (double) normal.Y * matrix.M23 + (double) normal.Z * matrix.M33;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static void TransformNormal(
      ref Vector3D normal,
      ref MatrixI matrix,
      out Vector3D result)
    {
      result = normal.X * new Vector3D(Base6Directions.GetVector(matrix.Right)) + normal.Y * new Vector3D(Base6Directions.GetVector(matrix.Up)) + normal.Z * new Vector3D(Base6Directions.GetVector(matrix.Backward));
    }

    public static Vector3D TransformNormal(Vector3D normal, MyBlockOrientation orientation)
    {
      Vector3D result;
      Vector3D.TransformNormal(ref normal, orientation, out result);
      return result;
    }

    public static void TransformNormal(
      ref Vector3D normal,
      MyBlockOrientation orientation,
      out Vector3D result)
    {
      result = -normal.X * new Vector3D(Base6Directions.GetVector(orientation.Left)) + normal.Y * new Vector3D(Base6Directions.GetVector(orientation.Up)) - normal.Z * new Vector3D(Base6Directions.GetVector(orientation.Forward));
    }

    public static Vector3D TransformNormal(Vector3D normal, ref MatrixD matrix)
    {
      Vector3D result;
      Vector3D.TransformNormal(ref normal, ref matrix, out result);
      return result;
    }

    public static Vector3D Transform(Vector3D value, Quaternion rotation)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num1;
      double num5 = (double) rotation.W * num2;
      double num6 = (double) rotation.W * num3;
      double num7 = (double) rotation.X * num1;
      double num8 = (double) rotation.X * num2;
      double num9 = (double) rotation.X * num3;
      double num10 = (double) rotation.Y * num2;
      double num11 = (double) rotation.Y * num3;
      double num12 = (double) rotation.Z * num3;
      double num13 = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
      double num14 = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
      double num15 = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
      Vector3D vector3D;
      vector3D.X = num13;
      vector3D.Y = num14;
      vector3D.Z = num15;
      return vector3D;
    }

    public static void Transform(ref Vector3D value, ref Quaternion rotation, out Vector3D result)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num1;
      double num5 = (double) rotation.W * num2;
      double num6 = (double) rotation.W * num3;
      double num7 = (double) rotation.X * num1;
      double num8 = (double) rotation.X * num2;
      double num9 = (double) rotation.X * num3;
      double num10 = (double) rotation.Y * num2;
      double num11 = (double) rotation.Y * num3;
      double num12 = (double) rotation.Z * num3;
      double num13 = value.X * (1.0 - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5);
      double num14 = value.X * (num8 + num6) + value.Y * (1.0 - num7 - num12) + value.Z * (num11 - num4);
      double num15 = value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1.0 - num7 - num10);
      result.X = num13;
      result.Y = num14;
      result.Z = num15;
    }

    public static void Rotate(ref Vector3D vector, ref MatrixD rotationMatrix, out Vector3D result)
    {
      double num1 = vector.X * rotationMatrix.M11 + vector.Y * rotationMatrix.M21 + vector.Z * rotationMatrix.M31;
      double num2 = vector.X * rotationMatrix.M12 + vector.Y * rotationMatrix.M22 + vector.Z * rotationMatrix.M32;
      double num3 = vector.X * rotationMatrix.M13 + vector.Y * rotationMatrix.M23 + vector.Z * rotationMatrix.M33;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
    }

    public static Vector3D Rotate(Vector3D vector, MatrixD rotationMatrix)
    {
      Vector3D result;
      Vector3D.Rotate(ref vector, ref rotationMatrix, out result);
      return result;
    }

    public static void Transform(
      Vector3D[] sourceArray,
      ref MatrixD matrix,
      Vector3D[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        destinationArray[index].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + matrix.M41;
        destinationArray[index].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + matrix.M42;
        destinationArray[index].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + matrix.M43;
      }
    }

    public static unsafe void Transform(
      Vector3D[] sourceArray,
      ref MatrixD matrix,
      Vector3D* destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        destinationArray[index].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + matrix.M41;
        destinationArray[index].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + matrix.M42;
        destinationArray[index].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + matrix.M43;
      }
    }

    public static void Transform(
      Vector3D[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector3D[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        double z = sourceArray[sourceIndex].Z;
        destinationArray[destinationIndex].X = x * (double) matrix.M11 + y * (double) matrix.M21 + z * (double) matrix.M31 + (double) matrix.M41;
        destinationArray[destinationIndex].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + z * (double) matrix.M32 + (double) matrix.M42;
        destinationArray[destinationIndex].Z = x * (double) matrix.M13 + y * (double) matrix.M23 + z * (double) matrix.M33 + (double) matrix.M43;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void TransformNormal(
      Vector3D[] sourceArray,
      ref Matrix matrix,
      Vector3D[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        destinationArray[index].X = x * (double) matrix.M11 + y * (double) matrix.M21 + z * (double) matrix.M31;
        destinationArray[index].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + z * (double) matrix.M32;
        destinationArray[index].Z = x * (double) matrix.M13 + y * (double) matrix.M23 + z * (double) matrix.M33;
      }
    }

    public static unsafe void TransformNormal(
      Vector3D[] sourceArray,
      ref Matrix matrix,
      Vector3D* destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        destinationArray[index].X = x * (double) matrix.M11 + y * (double) matrix.M21 + z * (double) matrix.M31;
        destinationArray[index].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + z * (double) matrix.M32;
        destinationArray[index].Z = x * (double) matrix.M13 + y * (double) matrix.M23 + z * (double) matrix.M33;
      }
    }

    public static void TransformNormal(
      Vector3D[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector3D[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        double z = sourceArray[sourceIndex].Z;
        destinationArray[destinationIndex].X = x * (double) matrix.M11 + y * (double) matrix.M21 + z * (double) matrix.M31;
        destinationArray[destinationIndex].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + z * (double) matrix.M32;
        destinationArray[destinationIndex].Z = x * (double) matrix.M13 + y * (double) matrix.M23 + z * (double) matrix.M33;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void Transform(
      Vector3D[] sourceArray,
      ref Quaternion rotation,
      Vector3D[] destinationArray)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num1;
      double num5 = (double) rotation.W * num2;
      double num6 = (double) rotation.W * num3;
      double num7 = (double) rotation.X * num1;
      double num8 = (double) rotation.X * num2;
      double num9 = (double) rotation.X * num3;
      double num10 = (double) rotation.Y * num2;
      double num11 = (double) rotation.Y * num3;
      double num12 = (double) rotation.Z * num3;
      double num13 = 1.0 - num10 - num12;
      double num14 = num8 - num6;
      double num15 = num9 + num5;
      double num16 = num8 + num6;
      double num17 = 1.0 - num7 - num12;
      double num18 = num11 - num4;
      double num19 = num9 - num5;
      double num20 = num11 + num4;
      double num21 = 1.0 - num7 - num10;
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        destinationArray[index].X = x * num13 + y * num14 + z * num15;
        destinationArray[index].Y = x * num16 + y * num17 + z * num18;
        destinationArray[index].Z = x * num19 + y * num20 + z * num21;
      }
    }

    public static void Transform(
      Vector3D[] sourceArray,
      int sourceIndex,
      ref Quaternion rotation,
      Vector3D[] destinationArray,
      int destinationIndex,
      int length)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num1;
      double num5 = (double) rotation.W * num2;
      double num6 = (double) rotation.W * num3;
      double num7 = (double) rotation.X * num1;
      double num8 = (double) rotation.X * num2;
      double num9 = (double) rotation.X * num3;
      double num10 = (double) rotation.Y * num2;
      double num11 = (double) rotation.Y * num3;
      double num12 = (double) rotation.Z * num3;
      double num13 = 1.0 - num10 - num12;
      double num14 = num8 - num6;
      double num15 = num9 + num5;
      double num16 = num8 + num6;
      double num17 = 1.0 - num7 - num12;
      double num18 = num11 - num4;
      double num19 = num9 - num5;
      double num20 = num11 + num4;
      double num21 = 1.0 - num7 - num10;
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        double z = sourceArray[sourceIndex].Z;
        destinationArray[destinationIndex].X = x * num13 + y * num14 + z * num15;
        destinationArray[destinationIndex].Y = x * num16 + y * num17 + z * num18;
        destinationArray[destinationIndex].Z = x * num19 + y * num20 + z * num21;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static Vector3D Negate(Vector3D value)
    {
      Vector3D vector3D;
      vector3D.X = -value.X;
      vector3D.Y = -value.Y;
      vector3D.Z = -value.Z;
      return vector3D;
    }

    public static void Negate(ref Vector3D value, out Vector3D result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
      result.Z = -value.Z;
    }

    public static Vector3D Add(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X + value2.X;
      vector3D.Y = value1.Y + value2.Y;
      vector3D.Z = value1.Z + value2.Z;
      return vector3D;
    }

    public static void Add(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
      result.Z = value1.Z + value2.Z;
    }

    public static Vector3D Subtract(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X - value2.X;
      vector3D.Y = value1.Y - value2.Y;
      vector3D.Z = value1.Z - value2.Z;
      return vector3D;
    }

    public static void Subtract(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
      result.Z = value1.Z - value2.Z;
    }

    public static Vector3D Multiply(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X * value2.X;
      vector3D.Y = value1.Y * value2.Y;
      vector3D.Z = value1.Z * value2.Z;
      return vector3D;
    }

    public static void Multiply(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
      result.Z = value1.Z * value2.Z;
    }

    public static Vector3D Multiply(Vector3D value1, double scaleFactor)
    {
      Vector3D vector3D;
      vector3D.X = value1.X * scaleFactor;
      vector3D.Y = value1.Y * scaleFactor;
      vector3D.Z = value1.Z * scaleFactor;
      return vector3D;
    }

    public static void Multiply(ref Vector3D value1, double scaleFactor, out Vector3D result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
      result.Z = value1.Z * scaleFactor;
    }

    public static Vector3D Divide(Vector3D value1, Vector3D value2)
    {
      Vector3D vector3D;
      vector3D.X = value1.X / value2.X;
      vector3D.Y = value1.Y / value2.Y;
      vector3D.Z = value1.Z / value2.Z;
      return vector3D;
    }

    public static void Divide(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
      result.Z = value1.Z / value2.Z;
    }

    public static Vector3D Divide(Vector3D value1, double value2)
    {
      double num = 1.0 / value2;
      Vector3D vector3D;
      vector3D.X = value1.X * num;
      vector3D.Y = value1.Y * num;
      vector3D.Z = value1.Z * num;
      return vector3D;
    }

    public static void Divide(ref Vector3D value1, double value2, out Vector3D result)
    {
      double num = 1.0 / value2;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
      result.Z = value1.Z * num;
    }

    public static Vector3D CalculatePerpendicularVector(Vector3D v)
    {
      Vector3D result;
      v.CalculatePerpendicularVector(out result);
      return result;
    }

    public void CalculatePerpendicularVector(out Vector3D result)
    {
      result = Math.Abs(this.Y + this.Z) > 9.99999974737875E-05 || Math.Abs(this.X) > 9.99999974737875E-05 ? new Vector3D(-(this.Y + this.Z), this.X, this.X) : new Vector3D(this.Z, this.Z, -(this.X + this.Y));
      Vector3D.Normalize(ref result, out result);
    }

    public static void GetAzimuthAndElevation(Vector3D v, out double azimuth, out double elevation)
    {
      double result1;
      Vector3D.Dot(ref v, ref Vector3D.Up, out result1);
      v.Y = 0.0;
      v.Normalize();
      double result2;
      Vector3D.Dot(ref v, ref Vector3D.Forward, out result2);
      elevation = Math.Asin(result1);
      if (v.X >= 0.0)
        azimuth = -Math.Acos(result2);
      else
        azimuth = Math.Acos(result2);
    }

    public static void CreateFromAzimuthAndElevation(
      double azimuth,
      double elevation,
      out Vector3D direction)
    {
      MatrixD rotationY = MatrixD.CreateRotationY(azimuth);
      MatrixD rotationX = MatrixD.CreateRotationX(elevation);
      direction = Vector3D.Forward;
      Vector3D.TransformNormal(ref direction, ref rotationX, out direction);
      Vector3D.TransformNormal(ref direction, ref rotationY, out direction);
    }

    public double Sum => this.X + this.Y + this.Z;

    public double Volume => this.X * this.Y * this.Z;

    public long VolumeInt(double multiplier) => (long) (this.X * multiplier) * (long) (this.Y * multiplier) * (long) (this.Z * multiplier);

    public bool IsInsideInclusive(ref Vector3D min, ref Vector3D max) => min.X <= this.X && this.X <= max.X && (min.Y <= this.Y && this.Y <= max.Y) && min.Z <= this.Z && this.Z <= max.Z;

    public static Vector3D SwapYZCoordinates(Vector3D v) => new Vector3D(v.X, v.Z, -v.Y);

    public double GetDim(int i)
    {
      switch (i)
      {
        case 0:
          return this.X;
        case 1:
          return this.Y;
        case 2:
          return this.Z;
        default:
          return this.GetDim((i % 3 + 3) % 3);
      }
    }

    public void SetDim(int i, double value)
    {
      switch (i)
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
          this.SetDim((i % 3 + 3) % 3, value);
          break;
      }
    }

    public static explicit operator Vector3I(Vector3D v) => new Vector3I((int) v.X, (int) v.Y, (int) v.Z);

    public static implicit operator Vector3(Vector3D v) => new Vector3((float) v.X, (float) v.Y, (float) v.Z);

    public static implicit operator Vector3D(Vector3 v) => new Vector3D((double) v.X, (double) v.Y, (double) v.Z);

    public static Vector3I Round(Vector3D vect3d) => new Vector3I(vect3d + 0.5);

    public static Vector3I Floor(Vector3D vect3d) => new Vector3I((int) Math.Floor(vect3d.X), (int) Math.Floor(vect3d.Y), (int) Math.Floor(vect3d.Z));

    public static void Fract(ref Vector3D o, out Vector3D r)
    {
      r.X = o.X - Math.Floor(o.X);
      r.Y = o.Y - Math.Floor(o.Y);
      r.Z = o.Z - Math.Floor(o.Z);
    }

    public static Vector3D Round(Vector3D v, int numDecimals) => new Vector3D(Math.Round(v.X, numDecimals), Math.Round(v.Y, numDecimals), Math.Round(v.Z, numDecimals));

    public static void Abs(ref Vector3D vector3D, out Vector3D abs)
    {
      abs.X = Math.Abs(vector3D.X);
      abs.Y = Math.Abs(vector3D.Y);
      abs.Z = Math.Abs(vector3D.Z);
    }

    public static Vector3D ProjectOnPlane(ref Vector3D vec, ref Vector3D planeNormal)
    {
      double num1 = vec.Dot(planeNormal);
      double num2 = planeNormal.LengthSquared();
      return vec - num1 / num2 * planeNormal;
    }

    public static Vector3D ProjectOnVector(ref Vector3D vec, ref Vector3D guideVector) => Vector3D.IsZero(ref vec) || Vector3D.IsZero(ref guideVector) ? Vector3D.Zero : guideVector * Vector3D.Dot(vec, guideVector) / guideVector.LengthSquared();

    private static bool IsZero(ref Vector3D vec) => Vector3D.IsZero(vec.X) && Vector3D.IsZero(vec.Y) && Vector3D.IsZero(vec.Z);

    private static bool IsZero(double d) => d > -9.99999974737875E-06 && d < 9.99999974737875E-06;

    protected class VRageMath_Vector3D\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3D owner, in double value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3D owner, out double value) => value = owner.X;
    }

    protected class VRageMath_Vector3D\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3D owner, in double value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3D owner, out double value) => value = owner.Y;
    }

    protected class VRageMath_Vector3D\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3D owner, in double value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3D owner, out double value) => value = owner.Z;
    }
  }
}
