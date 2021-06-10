// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector4D
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct Vector4D : IEquatable<Vector4>
  {
    public static Vector4D Zero = new Vector4D();
    public static Vector4D One = new Vector4D(1.0, 1.0, 1.0, 1.0);
    public static Vector4D UnitX = new Vector4D(1.0, 0.0, 0.0, 0.0);
    public static Vector4D UnitY = new Vector4D(0.0, 1.0, 0.0, 0.0);
    public static Vector4D UnitZ = new Vector4D(0.0, 0.0, 1.0, 0.0);
    public static Vector4D UnitW = new Vector4D(0.0, 0.0, 0.0, 1.0);
    [ProtoMember(1)]
    public double X;
    [ProtoMember(4)]
    public double Y;
    [ProtoMember(7)]
    public double Z;
    [ProtoMember(10)]
    public double W;

    public Vector4D(double x, double y, double z, double w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Vector4D(Vector2 value, double z, double w)
    {
      this.X = (double) value.X;
      this.Y = (double) value.Y;
      this.Z = z;
      this.W = w;
    }

    public Vector4D(Vector3D value, double w)
    {
      this.X = value.X;
      this.Y = value.Y;
      this.Z = value.Z;
      this.W = w;
    }

    public Vector4D(double value) => this.X = this.Y = this.Z = this.W = value;

    public static Vector4D operator -(Vector4D value)
    {
      Vector4D vector4D;
      vector4D.X = -value.X;
      vector4D.Y = -value.Y;
      vector4D.Z = -value.Z;
      vector4D.W = -value.W;
      return vector4D;
    }

    public static bool operator ==(Vector4D value1, Vector4D value2) => value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;

    public static bool operator !=(Vector4D value1, Vector4D value2) => value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;

    public static Vector4D operator +(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X + value2.X;
      vector4D.Y = value1.Y + value2.Y;
      vector4D.Z = value1.Z + value2.Z;
      vector4D.W = value1.W + value2.W;
      return vector4D;
    }

    public static Vector4D operator -(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X - value2.X;
      vector4D.Y = value1.Y - value2.Y;
      vector4D.Z = value1.Z - value2.Z;
      vector4D.W = value1.W - value2.W;
      return vector4D;
    }

    public static Vector4D operator *(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X * value2.X;
      vector4D.Y = value1.Y * value2.Y;
      vector4D.Z = value1.Z * value2.Z;
      vector4D.W = value1.W * value2.W;
      return vector4D;
    }

    public static Vector4D operator *(Vector4D value1, double scaleFactor)
    {
      Vector4D vector4D;
      vector4D.X = value1.X * scaleFactor;
      vector4D.Y = value1.Y * scaleFactor;
      vector4D.Z = value1.Z * scaleFactor;
      vector4D.W = value1.W * scaleFactor;
      return vector4D;
    }

    public static Vector4D operator *(double scaleFactor, Vector4D value1)
    {
      Vector4D vector4D;
      vector4D.X = value1.X * scaleFactor;
      vector4D.Y = value1.Y * scaleFactor;
      vector4D.Z = value1.Z * scaleFactor;
      vector4D.W = value1.W * scaleFactor;
      return vector4D;
    }

    public static Vector4D operator /(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X / value2.X;
      vector4D.Y = value1.Y / value2.Y;
      vector4D.Z = value1.Z / value2.Z;
      vector4D.W = value1.W / value2.W;
      return vector4D;
    }

    public static Vector4D operator /(Vector4D value1, double divider)
    {
      double num = 1.0 / divider;
      Vector4D vector4D;
      vector4D.X = value1.X * num;
      vector4D.Y = value1.Y * num;
      vector4D.Z = value1.Z * num;
      vector4D.W = value1.W * num;
      return vector4D;
    }

    public static Vector4D operator /(double lhs, Vector4D rhs)
    {
      Vector4D vector4D;
      vector4D.X = lhs / rhs.X;
      vector4D.Y = lhs / rhs.Y;
      vector4D.Z = lhs / rhs.Z;
      vector4D.W = lhs / rhs.W;
      return vector4D;
    }

    public static Vector4D PackOrthoMatrix(Vector3D position, Vector3 forward, Vector3 up)
    {
      int direction1 = (int) Base6Directions.GetDirection(forward);
      int direction2 = (int) Base6Directions.GetDirection(up);
      return new Vector4D(position, (double) (direction1 * 6 + direction2));
    }

    public static Vector4D PackOrthoMatrix(ref MatrixD matrix)
    {
      int direction1 = (int) Base6Directions.GetDirection((Vector3) matrix.Forward);
      int direction2 = (int) Base6Directions.GetDirection((Vector3) matrix.Up);
      return new Vector4D(matrix.Translation, (double) (direction1 * 6 + direction2));
    }

    public static MatrixD UnpackOrthoMatrix(ref Vector4D packed)
    {
      int w = (int) packed.W;
      return MatrixD.CreateWorld((Vector3D) new Vector3((Vector4) packed), Base6Directions.GetVector(w / 6), Base6Directions.GetVector(w % 6));
    }

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", (object) this.X.ToString((IFormatProvider) currentCulture), (object) this.Y.ToString((IFormatProvider) currentCulture), (object) this.Z.ToString((IFormatProvider) currentCulture), (object) this.W.ToString((IFormatProvider) currentCulture));
    }

    public bool Equals(Vector4 other) => this.X == (double) other.X && this.Y == (double) other.Y && this.Z == (double) other.Z && this.W == (double) other.W;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Vector4 other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();

    public double Length() => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);

    public double LengthSquared() => this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;

    public static double Distance(Vector4 value1, Vector4 value2)
    {
      double num1 = (double) value1.X - (double) value2.X;
      double num2 = (double) value1.Y - (double) value2.Y;
      double num3 = (double) value1.Z - (double) value2.Z;
      double num4 = (double) value1.W - (double) value2.W;
      return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4);
    }

    public static void Distance(ref Vector4 value1, ref Vector4 value2, out double result)
    {
      double num1 = (double) value1.X - (double) value2.X;
      double num2 = (double) value1.Y - (double) value2.Y;
      double num3 = (double) value1.Z - (double) value2.Z;
      double num4 = (double) value1.W - (double) value2.W;
      double d = num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4;
      result = Math.Sqrt(d);
    }

    public static double DistanceSquared(Vector4 value1, Vector4 value2)
    {
      double num1 = (double) value1.X - (double) value2.X;
      double num2 = (double) value1.Y - (double) value2.Y;
      double num3 = (double) value1.Z - (double) value2.Z;
      double num4 = (double) value1.W - (double) value2.W;
      return num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4;
    }

    public static void DistanceSquared(ref Vector4 value1, ref Vector4 value2, out double result)
    {
      double num1 = (double) value1.X - (double) value2.X;
      double num2 = (double) value1.Y - (double) value2.Y;
      double num3 = (double) value1.Z - (double) value2.Z;
      double num4 = (double) value1.W - (double) value2.W;
      result = num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4;
    }

    public static double Dot(Vector4 vector1, Vector4 vector2) => (double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z + (double) vector1.W * (double) vector2.W;

    public static void Dot(ref Vector4 vector1, ref Vector4 vector2, out double result) => result = (double) vector1.X * (double) vector2.X + (double) vector1.Y * (double) vector2.Y + (double) vector1.Z * (double) vector2.Z + (double) vector1.W * (double) vector2.W;

    public void Normalize()
    {
      double num = 1.0 / Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
      this.X *= num;
      this.Y *= num;
      this.Z *= num;
      this.W *= num;
    }

    public static Vector4D Normalize(Vector4D vector)
    {
      double num = 1.0 / Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W);
      Vector4D vector4D;
      vector4D.X = vector.X * num;
      vector4D.Y = vector.Y * num;
      vector4D.Z = vector.Z * num;
      vector4D.W = vector.W * num;
      return vector4D;
    }

    public static void Normalize(ref Vector4D vector, out Vector4D result)
    {
      double num = 1.0 / Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W);
      result.X = vector.X * num;
      result.Y = vector.Y * num;
      result.Z = vector.Z * num;
      result.W = vector.W * num;
    }

    public static Vector4 Min(Vector4 value1, Vector4 value2)
    {
      Vector4 vector4;
      vector4.X = (double) value1.X < (double) value2.X ? value1.X : value2.X;
      vector4.Y = (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y;
      vector4.Z = (double) value1.Z < (double) value2.Z ? value1.Z : value2.Z;
      vector4.W = (double) value1.W < (double) value2.W ? value1.W : value2.W;
      return vector4;
    }

    public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.X = (double) value1.X < (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y;
      result.Z = (double) value1.Z < (double) value2.Z ? value1.Z : value2.Z;
      result.W = (double) value1.W < (double) value2.W ? value1.W : value2.W;
    }

    public static Vector4 Max(Vector4 value1, Vector4 value2)
    {
      Vector4 vector4;
      vector4.X = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      vector4.Y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
      vector4.Z = (double) value1.Z > (double) value2.Z ? value1.Z : value2.Z;
      vector4.W = (double) value1.W > (double) value2.W ? value1.W : value2.W;
      return vector4;
    }

    public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.X = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
      result.Z = (double) value1.Z > (double) value2.Z ? value1.Z : value2.Z;
      result.W = (double) value1.W > (double) value2.W ? value1.W : value2.W;
    }

    public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max)
    {
      double x = value1.X;
      double num1 = x > max.X ? max.X : x;
      double num2 = num1 < min.X ? min.X : num1;
      double y = value1.Y;
      double num3 = y > max.Y ? max.Y : y;
      double num4 = num3 < min.Y ? min.Y : num3;
      double z = value1.Z;
      double num5 = z > max.Z ? max.Z : z;
      double num6 = num5 < min.Z ? min.Z : num5;
      double w = value1.W;
      double num7 = w > max.W ? max.W : w;
      double num8 = num7 < min.W ? min.W : num7;
      Vector4D vector4D;
      vector4D.X = num2;
      vector4D.Y = num4;
      vector4D.Z = num6;
      vector4D.W = num8;
      return vector4D;
    }

    public static void Clamp(
      ref Vector4D value1,
      ref Vector4D min,
      ref Vector4D max,
      out Vector4D result)
    {
      double x = value1.X;
      double num1 = x > max.X ? max.X : x;
      double num2 = num1 < min.X ? min.X : num1;
      double y = value1.Y;
      double num3 = y > max.Y ? max.Y : y;
      double num4 = num3 < min.Y ? min.Y : num3;
      double z = value1.Z;
      double num5 = z > max.Z ? max.Z : z;
      double num6 = num5 < min.Z ? min.Z : num5;
      double w = value1.W;
      double num7 = w > max.W ? max.W : w;
      double num8 = num7 < min.W ? min.W : num7;
      result.X = num2;
      result.Y = num4;
      result.Z = num6;
      result.W = num8;
    }

    public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount)
    {
      Vector4D vector4D;
      vector4D.X = value1.X + (value2.X - value1.X) * amount;
      vector4D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      vector4D.Z = value1.Z + (value2.Z - value1.Z) * amount;
      vector4D.W = value1.W + (value2.W - value1.W) * amount;
      return vector4D;
    }

    public static void Lerp(
      ref Vector4D value1,
      ref Vector4D value2,
      double amount,
      out Vector4D result)
    {
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
      result.Z = value1.Z + (value2.Z - value1.Z) * amount;
      result.W = value1.W + (value2.W - value1.W) * amount;
    }

    public static Vector4D Barycentric(
      Vector4D value1,
      Vector4D value2,
      Vector4D value3,
      double amount1,
      double amount2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      vector4D.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
      vector4D.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
      vector4D.W = value1.W + amount1 * (value2.W - value1.W) + amount2 * (value3.W - value1.W);
      return vector4D;
    }

    public static void Barycentric(
      ref Vector4D value1,
      ref Vector4D value2,
      ref Vector4D value3,
      double amount1,
      double amount2,
      out Vector4D result)
    {
      result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
      result.Z = value1.Z + amount1 * (value2.Z - value1.Z) + amount2 * (value3.Z - value1.Z);
      result.W = value1.W + amount1 * (value2.W - value1.W) + amount2 * (value3.W - value1.W);
    }

    public static Vector4D SmoothStep(Vector4D value1, Vector4D value2, double amount)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      Vector4D vector4D;
      vector4D.X = value1.X + (value2.X - value1.X) * amount;
      vector4D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      vector4D.Z = value1.Z + (value2.Z - value1.Z) * amount;
      vector4D.W = value1.W + (value2.W - value1.W) * amount;
      return vector4D;
    }

    public static void SmoothStep(
      ref Vector4D value1,
      ref Vector4D value2,
      double amount,
      out Vector4D result)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
      result.Z = value1.Z + (value2.Z - value1.Z) * amount;
      result.W = value1.W + (value2.W - value1.W) * amount;
    }

    public static Vector4D CatmullRom(
      Vector4D value1,
      Vector4D value2,
      Vector4D value3,
      Vector4D value4,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      Vector4D vector4D;
      vector4D.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      vector4D.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
      vector4D.Z = 0.5 * (2.0 * value2.Z + (-value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num1 + (-value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
      vector4D.W = 0.5 * (2.0 * value2.W + (-value1.W + value3.W) * amount + (2.0 * value1.W - 5.0 * value2.W + 4.0 * value3.W - value4.W) * num1 + (-value1.W + 3.0 * value2.W - 3.0 * value3.W + value4.W) * num2);
      return vector4D;
    }

    public static void CatmullRom(
      ref Vector4D value1,
      ref Vector4D value2,
      ref Vector4D value3,
      ref Vector4D value4,
      double amount,
      out Vector4D result)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      result.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      result.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
      result.Z = 0.5 * (2.0 * value2.Z + (-value1.Z + value3.Z) * amount + (2.0 * value1.Z - 5.0 * value2.Z + 4.0 * value3.Z - value4.Z) * num1 + (-value1.Z + 3.0 * value2.Z - 3.0 * value3.Z + value4.Z) * num2);
      result.W = 0.5 * (2.0 * value2.W + (-value1.W + value3.W) * amount + (2.0 * value1.W - 5.0 * value2.W + 4.0 * value3.W - value4.W) * num1 + (-value1.W + 3.0 * value2.W - 3.0 * value3.W + value4.W) * num2);
    }

    public static Vector4D Hermite(
      Vector4D value1,
      Vector4D tangent1,
      Vector4D value2,
      Vector4D tangent2,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      double num3 = 2.0 * num2 - 3.0 * num1 + 1.0;
      double num4 = -2.0 * num2 + 3.0 * num1;
      double num5 = num2 - 2.0 * num1 + amount;
      double num6 = num2 - num1;
      Vector4D vector4D;
      vector4D.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
      vector4D.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
      vector4D.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
      vector4D.W = value1.W * num3 + value2.W * num4 + tangent1.W * num5 + tangent2.W * num6;
      return vector4D;
    }

    public static void Hermite(
      ref Vector4D value1,
      ref Vector4D tangent1,
      ref Vector4D value2,
      ref Vector4D tangent2,
      double amount,
      out Vector4D result)
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
      result.W = value1.W * num3 + value2.W * num4 + tangent1.W * num5 + tangent2.W * num6;
    }

    public static Vector4D Transform(Vector2 position, MatrixD matrix)
    {
      double num1 = (double) position.X * matrix.M11 + (double) position.Y * matrix.M21 + matrix.M41;
      double num2 = (double) position.X * matrix.M12 + (double) position.Y * matrix.M22 + matrix.M42;
      double num3 = (double) position.X * matrix.M13 + (double) position.Y * matrix.M23 + matrix.M43;
      double num4 = (double) position.X * matrix.M14 + (double) position.Y * matrix.M24 + matrix.M44;
      Vector4D vector4D;
      vector4D.X = num1;
      vector4D.Y = num2;
      vector4D.Z = num3;
      vector4D.W = num4;
      return vector4D;
    }

    public static void Transform(ref Vector2 position, ref MatrixD matrix, out Vector4D result)
    {
      double num1 = (double) position.X * matrix.M11 + (double) position.Y * matrix.M21 + matrix.M41;
      double num2 = (double) position.X * matrix.M12 + (double) position.Y * matrix.M22 + matrix.M42;
      double num3 = (double) position.X * matrix.M13 + (double) position.Y * matrix.M23 + matrix.M43;
      double num4 = (double) position.X * matrix.M14 + (double) position.Y * matrix.M24 + matrix.M44;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
      result.W = num4;
    }

    public static Vector4D Transform(Vector3D position, MatrixD matrix)
    {
      double num1 = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
      double num2 = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
      double num3 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
      double num4 = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
      Vector4D vector4D;
      vector4D.X = num1;
      vector4D.Y = num2;
      vector4D.Z = num3;
      vector4D.W = num4;
      return vector4D;
    }

    public static void Transform(ref Vector3D position, ref MatrixD matrix, out Vector4D result)
    {
      double num1 = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
      double num2 = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
      double num3 = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
      double num4 = position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
      result.W = num4;
    }

    public static Vector4D Transform(Vector4D vector, MatrixD matrix)
    {
      double num1 = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41;
      double num2 = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42;
      double num3 = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43;
      double num4 = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44;
      Vector4D vector4D;
      vector4D.X = num1;
      vector4D.Y = num2;
      vector4D.Z = num3;
      vector4D.W = num4;
      return vector4D;
    }

    public static void Transform(ref Vector4D vector, ref MatrixD matrix, out Vector4D result)
    {
      double num1 = vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41;
      double num2 = vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42;
      double num3 = vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43;
      double num4 = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44;
      result.X = num1;
      result.Y = num2;
      result.Z = num3;
      result.W = num4;
    }

    public static Vector4D Transform(Vector2 value, Quaternion rotation)
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
      double num13 = (double) value.X * (1.0 - num10 - num12) + (double) value.Y * (num8 - num6);
      double num14 = (double) value.X * (num8 + num6) + (double) value.Y * (1.0 - num7 - num12);
      double num15 = (double) value.X * (num9 - num5) + (double) value.Y * (num11 + num4);
      Vector4D vector4D;
      vector4D.X = num13;
      vector4D.Y = num14;
      vector4D.Z = num15;
      vector4D.W = 1.0;
      return vector4D;
    }

    public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector4D result)
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
      double num13 = (double) value.X * (1.0 - num10 - num12) + (double) value.Y * (num8 - num6);
      double num14 = (double) value.X * (num8 + num6) + (double) value.Y * (1.0 - num7 - num12);
      double num15 = (double) value.X * (num9 - num5) + (double) value.Y * (num11 + num4);
      result.X = num13;
      result.Y = num14;
      result.Z = num15;
      result.W = 1.0;
    }

    public static Vector4D Transform(Vector3D value, Quaternion rotation)
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
      Vector4D vector4D;
      vector4D.X = num13;
      vector4D.Y = num14;
      vector4D.Z = num15;
      vector4D.W = 1.0;
      return vector4D;
    }

    public static void Transform(ref Vector3D value, ref Quaternion rotation, out Vector4D result)
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
      result.W = 1.0;
    }

    public static Vector4D Transform(Vector4D value, Quaternion rotation)
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
      Vector4D vector4D;
      vector4D.X = num13;
      vector4D.Y = num14;
      vector4D.Z = num15;
      vector4D.W = value.W;
      return vector4D;
    }

    public static void Transform(ref Vector4D value, ref Quaternion rotation, out Vector4D result)
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
      result.W = value.W;
    }

    public static void Transform(
      Vector4D[] sourceArray,
      ref MatrixD matrix,
      Vector4D[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        double z = sourceArray[index].Z;
        double w = sourceArray[index].W;
        destinationArray[index].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + w * matrix.M41;
        destinationArray[index].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + w * matrix.M42;
        destinationArray[index].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + w * matrix.M43;
        destinationArray[index].W = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + w * matrix.M44;
      }
    }

    public static void Transform(
      Vector4D[] sourceArray,
      int sourceIndex,
      ref MatrixD matrix,
      Vector4D[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        double z = sourceArray[sourceIndex].Z;
        double w = sourceArray[sourceIndex].W;
        destinationArray[destinationIndex].X = x * matrix.M11 + y * matrix.M21 + z * matrix.M31 + w * matrix.M41;
        destinationArray[destinationIndex].Y = x * matrix.M12 + y * matrix.M22 + z * matrix.M32 + w * matrix.M42;
        destinationArray[destinationIndex].Z = x * matrix.M13 + y * matrix.M23 + z * matrix.M33 + w * matrix.M43;
        destinationArray[destinationIndex].W = x * matrix.M14 + y * matrix.M24 + z * matrix.M34 + w * matrix.M44;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void Transform(
      Vector4D[] sourceArray,
      ref Quaternion rotation,
      Vector4D[] destinationArray)
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
        destinationArray[index].W = sourceArray[index].W;
      }
    }

    public static void Transform(
      Vector4D[] sourceArray,
      int sourceIndex,
      ref Quaternion rotation,
      Vector4D[] destinationArray,
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
        double w = sourceArray[sourceIndex].W;
        destinationArray[destinationIndex].X = x * num13 + y * num14 + z * num15;
        destinationArray[destinationIndex].Y = x * num16 + y * num17 + z * num18;
        destinationArray[destinationIndex].Z = x * num19 + y * num20 + z * num21;
        destinationArray[destinationIndex].W = w;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static Vector4D Negate(Vector4D value)
    {
      Vector4D vector4D;
      vector4D.X = -value.X;
      vector4D.Y = -value.Y;
      vector4D.Z = -value.Z;
      vector4D.W = -value.W;
      return vector4D;
    }

    public static void Negate(ref Vector4D value, out Vector4D result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
      result.Z = -value.Z;
      result.W = -value.W;
    }

    public static Vector4D Add(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X + value2.X;
      vector4D.Y = value1.Y + value2.Y;
      vector4D.Z = value1.Z + value2.Z;
      vector4D.W = value1.W + value2.W;
      return vector4D;
    }

    public static void Add(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
      result.Z = value1.Z + value2.Z;
      result.W = value1.W + value2.W;
    }

    public static Vector4 Subtract(Vector4 value1, Vector4 value2)
    {
      Vector4 vector4;
      vector4.X = value1.X - value2.X;
      vector4.Y = value1.Y - value2.Y;
      vector4.Z = value1.Z - value2.Z;
      vector4.W = value1.W - value2.W;
      return vector4;
    }

    public static void Subtract(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
      result.Z = value1.Z - value2.Z;
      result.W = value1.W - value2.W;
    }

    public static Vector4D Multiply(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X * value2.X;
      vector4D.Y = value1.Y * value2.Y;
      vector4D.Z = value1.Z * value2.Z;
      vector4D.W = value1.W * value2.W;
      return vector4D;
    }

    public static void Multiply(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
      result.Z = value1.Z * value2.Z;
      result.W = value1.W * value2.W;
    }

    public static Vector4D Multiply(Vector4D value1, double scaleFactor)
    {
      Vector4D vector4D;
      vector4D.X = value1.X * scaleFactor;
      vector4D.Y = value1.Y * scaleFactor;
      vector4D.Z = value1.Z * scaleFactor;
      vector4D.W = value1.W * scaleFactor;
      return vector4D;
    }

    public static void Multiply(ref Vector4D value1, double scaleFactor, out Vector4D result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
      result.Z = value1.Z * scaleFactor;
      result.W = value1.W * scaleFactor;
    }

    public static Vector4D Divide(Vector4D value1, Vector4D value2)
    {
      Vector4D vector4D;
      vector4D.X = value1.X / value2.X;
      vector4D.Y = value1.Y / value2.Y;
      vector4D.Z = value1.Z / value2.Z;
      vector4D.W = value1.W / value2.W;
      return vector4D;
    }

    public static void Divide(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
      result.Z = value1.Z / value2.Z;
      result.W = value1.W / value2.W;
    }

    public static Vector4D Divide(Vector4D value1, double divider)
    {
      double num = 1.0 / divider;
      Vector4D vector4D;
      vector4D.X = value1.X * num;
      vector4D.Y = value1.Y * num;
      vector4D.Z = value1.Z * num;
      vector4D.W = value1.W * num;
      return vector4D;
    }

    public static void Divide(ref Vector4D value1, double divider, out Vector4D result)
    {
      double num = 1.0 / divider;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
      result.Z = value1.Z * num;
      result.W = value1.W * num;
    }

    public static implicit operator Vector4(Vector4D v) => new Vector4((float) v.X, (float) v.Y, (float) v.Z, (float) v.W);

    public static implicit operator Vector4D(Vector4 v) => new Vector4D((double) v.X, (double) v.Y, (double) v.Z, (double) v.W);

    protected class VRageMath_Vector4D\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector4D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4D owner, in double value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4D owner, out double value) => value = owner.X;
    }

    protected class VRageMath_Vector4D\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector4D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4D owner, in double value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4D owner, out double value) => value = owner.Y;
    }

    protected class VRageMath_Vector4D\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector4D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4D owner, in double value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4D owner, out double value) => value = owner.Z;
    }

    protected class VRageMath_Vector4D\u003C\u003EW\u003C\u003EAccessor : IMemberAccessor<Vector4D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4D owner, in double value) => owner.W = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4D owner, out double value) => value = owner.W;
    }
  }
}
