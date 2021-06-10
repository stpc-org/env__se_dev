// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector2D
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
  public struct Vector2D : IEquatable<Vector2D>
  {
    public static Vector2D Zero = new Vector2D();
    public static Vector2D One = new Vector2D(1.0, 1.0);
    public static Vector2D UnitX = new Vector2D(1.0, 0.0);
    public static Vector2D UnitY = new Vector2D(0.0, 1.0);
    public static Vector2D PositiveInfinity = Vector2D.One * double.PositiveInfinity;
    [ProtoMember(1)]
    public double X;
    [ProtoMember(4)]
    public double Y;

    public Vector2D(double x, double y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2D(double value) => this.X = this.Y = value;

    public double this[int index]
    {
      set
      {
        if (index == 0)
        {
          this.X = value;
        }
        else
        {
          if (index != 1)
            throw new ArgumentException();
          this.Y = value;
        }
      }
      get
      {
        if (index == 0)
          return this.X;
        if (index == 1)
          return this.Y;
        throw new ArgumentException();
      }
    }

    public static explicit operator Vector2I(Vector2D vector) => new Vector2I(vector);

    public static Vector2D operator -(Vector2D value)
    {
      Vector2D vector2D;
      vector2D.X = -value.X;
      vector2D.Y = -value.Y;
      return vector2D;
    }

    public static bool operator ==(Vector2D value1, Vector2D value2) => value1.X == value2.X && value1.Y == value2.Y;

    public static bool operator !=(Vector2D value1, Vector2D value2) => value1.X != value2.X || value1.Y != value2.Y;

    public static Vector2D operator +(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X + value2.X;
      vector2D.Y = value1.Y + value2.Y;
      return vector2D;
    }

    public static Vector2D operator +(Vector2D value1, double value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X + value2;
      vector2D.Y = value1.Y + value2;
      return vector2D;
    }

    public static Vector2D operator -(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X - value2.X;
      vector2D.Y = value1.Y - value2.Y;
      return vector2D;
    }

    public static Vector2D operator -(Vector2D value1, double value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X - value2;
      vector2D.Y = value1.Y - value2;
      return vector2D;
    }

    public static Vector2D operator *(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X * value2.X;
      vector2D.Y = value1.Y * value2.Y;
      return vector2D;
    }

    public static Vector2D operator *(Vector2D value, double scaleFactor)
    {
      Vector2D vector2D;
      vector2D.X = value.X * scaleFactor;
      vector2D.Y = value.Y * scaleFactor;
      return vector2D;
    }

    public static Vector2D operator *(double scaleFactor, Vector2D value)
    {
      Vector2D vector2D;
      vector2D.X = value.X * scaleFactor;
      vector2D.Y = value.Y * scaleFactor;
      return vector2D;
    }

    public static Vector2D operator /(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X / value2.X;
      vector2D.Y = value1.Y / value2.Y;
      return vector2D;
    }

    public static Vector2D operator /(Vector2D value1, double divider)
    {
      double num = 1.0 / divider;
      Vector2D vector2D;
      vector2D.X = value1.X * num;
      vector2D.Y = value1.Y * num;
      return vector2D;
    }

    public static Vector2D operator /(double value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1 / value2.X;
      vector2D.Y = value1 / value2.Y;
      return vector2D;
    }

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1}}}", new object[2]
      {
        (object) this.X.ToString((IFormatProvider) currentCulture),
        (object) this.Y.ToString((IFormatProvider) currentCulture)
      });
    }

    public bool Equals(Vector2D other) => this.X == other.X && this.Y == other.Y;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Vector2D other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public bool IsValid() => (this.X * this.Y).IsValid();

    [Conditional("DEBUG")]
    public void AssertIsValid()
    {
    }

    public double Length() => Math.Sqrt(this.X * this.X + this.Y * this.Y);

    public double LengthSquared() => this.X * this.X + this.Y * this.Y;

    public static double Distance(Vector2D value1, Vector2D value2)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      return Math.Sqrt(num1 * num1 + num2 * num2);
    }

    public static void Distance(ref Vector2D value1, ref Vector2D value2, out double result)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      double d = num1 * num1 + num2 * num2;
      result = Math.Sqrt(d);
    }

    public static double DistanceSquared(Vector2D value1, Vector2D value2)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      return num1 * num1 + num2 * num2;
    }

    public static void DistanceSquared(ref Vector2D value1, ref Vector2D value2, out double result)
    {
      double num1 = value1.X - value2.X;
      double num2 = value1.Y - value2.Y;
      result = num1 * num1 + num2 * num2;
    }

    public static double Dot(Vector2D value1, Vector2D value2) => value1.X * value2.X + value1.Y * value2.Y;

    public static void Dot(ref Vector2D value1, ref Vector2D value2, out double result) => result = value1.X * value2.X + value1.Y * value2.Y;

    public void Normalize()
    {
      double num = 1.0 / Math.Sqrt(this.X * this.X + this.Y * this.Y);
      this.X *= num;
      this.Y *= num;
    }

    public static Vector2D Normalize(Vector2D value)
    {
      double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y);
      Vector2D vector2D;
      vector2D.X = value.X * num;
      vector2D.Y = value.Y * num;
      return vector2D;
    }

    public static void Normalize(ref Vector2D value, out Vector2D result)
    {
      double num = 1.0 / Math.Sqrt(value.X * value.X + value.Y * value.Y);
      result.X = value.X * num;
      result.Y = value.Y * num;
    }

    public static Vector2D Reflect(Vector2D vector, Vector2D normal)
    {
      double num = vector.X * normal.X + vector.Y * normal.Y;
      Vector2D vector2D;
      vector2D.X = vector.X - 2.0 * num * normal.X;
      vector2D.Y = vector.Y - 2.0 * num * normal.Y;
      return vector2D;
    }

    public static void Reflect(ref Vector2D vector, ref Vector2D normal, out Vector2D result)
    {
      double num = vector.X * normal.X + vector.Y * normal.Y;
      result.X = vector.X - 2.0 * num * normal.X;
      result.Y = vector.Y - 2.0 * num * normal.Y;
    }

    public static Vector2D Min(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X < value2.X ? value1.X : value2.X;
      vector2D.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
      return vector2D;
    }

    public static void Min(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X < value2.X ? value1.X : value2.X;
      result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2D Max(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X > value2.X ? value1.X : value2.X;
      vector2D.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
      return vector2D;
    }

    public static void Max(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X > value2.X ? value1.X : value2.X;
      result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2D Clamp(Vector2D value1, Vector2D min, Vector2D max)
    {
      double x = value1.X;
      double num1 = x > max.X ? max.X : x;
      double num2 = num1 < min.X ? min.X : num1;
      double y = value1.Y;
      double num3 = y > max.Y ? max.Y : y;
      double num4 = num3 < min.Y ? min.Y : num3;
      Vector2D vector2D;
      vector2D.X = num2;
      vector2D.Y = num4;
      return vector2D;
    }

    public static void Clamp(
      ref Vector2D value1,
      ref Vector2D min,
      ref Vector2D max,
      out Vector2D result)
    {
      double x = value1.X;
      double num1 = x > max.X ? max.X : x;
      double num2 = num1 < min.X ? min.X : num1;
      double y = value1.Y;
      double num3 = y > max.Y ? max.Y : y;
      double num4 = num3 < min.Y ? min.Y : num3;
      result.X = num2;
      result.Y = num4;
    }

    public static Vector2D ClampToSphere(Vector2D vector, double radius)
    {
      double num1 = vector.LengthSquared();
      double num2 = radius * radius;
      return num1 > num2 ? vector * Math.Sqrt(num2 / num1) : vector;
    }

    public static void ClampToSphere(ref Vector2D vector, double radius)
    {
      double num1 = vector.LengthSquared();
      double num2 = radius * radius;
      if (num1 <= num2)
        return;
      vector *= Math.Sqrt(num2 / num1);
    }

    public static Vector2D Lerp(Vector2D value1, Vector2D value2, double amount)
    {
      Vector2D vector2D;
      vector2D.X = value1.X + (value2.X - value1.X) * amount;
      vector2D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      return vector2D;
    }

    public static void Lerp(
      ref Vector2D value1,
      ref Vector2D value2,
      double amount,
      out Vector2D result)
    {
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
    }

    public static Vector2D Barycentric(
      Vector2D value1,
      Vector2D value2,
      Vector2D value3,
      double amount1,
      double amount2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      vector2D.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
      return vector2D;
    }

    public static void Barycentric(
      ref Vector2D value1,
      ref Vector2D value2,
      ref Vector2D value3,
      double amount1,
      double amount2,
      out Vector2D result)
    {
      result.X = value1.X + amount1 * (value2.X - value1.X) + amount2 * (value3.X - value1.X);
      result.Y = value1.Y + amount1 * (value2.Y - value1.Y) + amount2 * (value3.Y - value1.Y);
    }

    public static Vector2D SmoothStep(Vector2D value1, Vector2D value2, double amount)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      Vector2D vector2D;
      vector2D.X = value1.X + (value2.X - value1.X) * amount;
      vector2D.Y = value1.Y + (value2.Y - value1.Y) * amount;
      return vector2D;
    }

    public static void SmoothStep(
      ref Vector2D value1,
      ref Vector2D value2,
      double amount,
      out Vector2D result)
    {
      amount = amount > 1.0 ? 1.0 : (amount < 0.0 ? 0.0 : amount);
      amount = amount * amount * (3.0 - 2.0 * amount);
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
    }

    public static Vector2D CatmullRom(
      Vector2D value1,
      Vector2D value2,
      Vector2D value3,
      Vector2D value4,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      Vector2D vector2D;
      vector2D.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      vector2D.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
      return vector2D;
    }

    public static void CatmullRom(
      ref Vector2D value1,
      ref Vector2D value2,
      ref Vector2D value3,
      ref Vector2D value4,
      double amount,
      out Vector2D result)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      result.X = 0.5 * (2.0 * value2.X + (-value1.X + value3.X) * amount + (2.0 * value1.X - 5.0 * value2.X + 4.0 * value3.X - value4.X) * num1 + (-value1.X + 3.0 * value2.X - 3.0 * value3.X + value4.X) * num2);
      result.Y = 0.5 * (2.0 * value2.Y + (-value1.Y + value3.Y) * amount + (2.0 * value1.Y - 5.0 * value2.Y + 4.0 * value3.Y - value4.Y) * num1 + (-value1.Y + 3.0 * value2.Y - 3.0 * value3.Y + value4.Y) * num2);
    }

    public static Vector2D Hermite(
      Vector2D value1,
      Vector2D tangent1,
      Vector2D value2,
      Vector2D tangent2,
      double amount)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      double num3 = 2.0 * num2 - 3.0 * num1 + 1.0;
      double num4 = -2.0 * num2 + 3.0 * num1;
      double num5 = num2 - 2.0 * num1 + amount;
      double num6 = num2 - num1;
      Vector2D vector2D;
      vector2D.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
      vector2D.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
      return vector2D;
    }

    public static void Hermite(
      ref Vector2D value1,
      ref Vector2D tangent1,
      ref Vector2D value2,
      ref Vector2D tangent2,
      double amount,
      out Vector2D result)
    {
      double num1 = amount * amount;
      double num2 = amount * num1;
      double num3 = 2.0 * num2 - 3.0 * num1 + 1.0;
      double num4 = -2.0 * num2 + 3.0 * num1;
      double num5 = num2 - 2.0 * num1 + amount;
      double num6 = num2 - num1;
      result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
      result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
    }

    public static Vector2D Transform(Vector2D position, Matrix matrix)
    {
      double num1 = position.X * (double) matrix.M11 + position.Y * (double) matrix.M21 + (double) matrix.M41;
      double num2 = position.X * (double) matrix.M12 + position.Y * (double) matrix.M22 + (double) matrix.M42;
      Vector2D vector2D;
      vector2D.X = num1;
      vector2D.Y = num2;
      return vector2D;
    }

    public static void Transform(ref Vector2D position, ref Matrix matrix, out Vector2D result)
    {
      double num1 = position.X * (double) matrix.M11 + position.Y * (double) matrix.M21 + (double) matrix.M41;
      double num2 = position.X * (double) matrix.M12 + position.Y * (double) matrix.M22 + (double) matrix.M42;
      result.X = num1;
      result.Y = num2;
    }

    public static Vector2D TransformNormal(Vector2D normal, Matrix matrix)
    {
      double num1 = normal.X * (double) matrix.M11 + normal.Y * (double) matrix.M21;
      double num2 = normal.X * (double) matrix.M12 + normal.Y * (double) matrix.M22;
      Vector2D vector2D;
      vector2D.X = num1;
      vector2D.Y = num2;
      return vector2D;
    }

    public static void TransformNormal(ref Vector2D normal, ref Matrix matrix, out Vector2D result)
    {
      double num1 = normal.X * (double) matrix.M11 + normal.Y * (double) matrix.M21;
      double num2 = normal.X * (double) matrix.M12 + normal.Y * (double) matrix.M22;
      result.X = num1;
      result.Y = num2;
    }

    public static Vector2D Transform(Vector2D value, Quaternion rotation)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num3;
      double num5 = (double) rotation.X * num1;
      double num6 = (double) rotation.X * num2;
      double num7 = (double) rotation.Y * num2;
      double num8 = (double) rotation.Z * num3;
      double num9 = value.X * (1.0 - num7 - num8) + value.Y * (num6 - num4);
      double num10 = value.X * (num6 + num4) + value.Y * (1.0 - num5 - num8);
      Vector2D vector2D;
      vector2D.X = num9;
      vector2D.Y = num10;
      return vector2D;
    }

    public static void Transform(ref Vector2D value, ref Quaternion rotation, out Vector2D result)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num3;
      double num5 = (double) rotation.X * num1;
      double num6 = (double) rotation.X * num2;
      double num7 = (double) rotation.Y * num2;
      double num8 = (double) rotation.Z * num3;
      double num9 = value.X * (1.0 - num7 - num8) + value.Y * (num6 - num4);
      double num10 = value.X * (num6 + num4) + value.Y * (1.0 - num5 - num8);
      result.X = num9;
      result.Y = num10;
    }

    public static void Transform(
      Vector2D[] sourceArray,
      ref Matrix matrix,
      Vector2D[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        destinationArray[index].X = x * (double) matrix.M11 + y * (double) matrix.M21 + (double) matrix.M41;
        destinationArray[index].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + (double) matrix.M42;
      }
    }

    public static void Transform(
      Vector2D[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector2D[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = x * (double) matrix.M11 + y * (double) matrix.M21 + (double) matrix.M41;
        destinationArray[destinationIndex].Y = x * (double) matrix.M12 + y * (double) matrix.M22 + (double) matrix.M42;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void TransformNormal(
      Vector2D[] sourceArray,
      ref Matrix matrix,
      Vector2D[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        destinationArray[index].X = x * (double) matrix.M11 + y * (double) matrix.M21;
        destinationArray[index].Y = x * (double) matrix.M12 + y * (double) matrix.M22;
      }
    }

    public static void TransformNormal(
      Vector2D[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector2D[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = x * (double) matrix.M11 + y * (double) matrix.M21;
        destinationArray[destinationIndex].Y = x * (double) matrix.M12 + y * (double) matrix.M22;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void Transform(
      Vector2D[] sourceArray,
      ref Quaternion rotation,
      Vector2D[] destinationArray)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num3;
      double num5 = (double) rotation.X * num1;
      double num6 = (double) rotation.X * num2;
      double num7 = (double) rotation.Y * num2;
      double num8 = (double) rotation.Z * num3;
      double num9 = 1.0 - num7 - num8;
      double num10 = num6 - num4;
      double num11 = num6 + num4;
      double num12 = 1.0 - num5 - num8;
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        double x = sourceArray[index].X;
        double y = sourceArray[index].Y;
        destinationArray[index].X = x * num9 + y * num10;
        destinationArray[index].Y = x * num11 + y * num12;
      }
    }

    public static void Transform(
      Vector2D[] sourceArray,
      int sourceIndex,
      ref Quaternion rotation,
      Vector2D[] destinationArray,
      int destinationIndex,
      int length)
    {
      double num1 = (double) rotation.X + (double) rotation.X;
      double num2 = (double) rotation.Y + (double) rotation.Y;
      double num3 = (double) rotation.Z + (double) rotation.Z;
      double num4 = (double) rotation.W * num3;
      double num5 = (double) rotation.X * num1;
      double num6 = (double) rotation.X * num2;
      double num7 = (double) rotation.Y * num2;
      double num8 = (double) rotation.Z * num3;
      double num9 = 1.0 - num7 - num8;
      double num10 = num6 - num4;
      double num11 = num6 + num4;
      double num12 = 1.0 - num5 - num8;
      for (; length > 0; --length)
      {
        double x = sourceArray[sourceIndex].X;
        double y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = x * num9 + y * num10;
        destinationArray[destinationIndex].Y = x * num11 + y * num12;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static Vector2D Negate(Vector2D value)
    {
      Vector2D vector2D;
      vector2D.X = -value.X;
      vector2D.Y = -value.Y;
      return vector2D;
    }

    public static void Negate(ref Vector2D value, out Vector2D result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
    }

    public static Vector2D Add(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X + value2.X;
      vector2D.Y = value1.Y + value2.Y;
      return vector2D;
    }

    public static void Add(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
    }

    public static Vector2D Subtract(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X - value2.X;
      vector2D.Y = value1.Y - value2.Y;
      return vector2D;
    }

    public static void Subtract(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
    }

    public static Vector2D Multiply(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X * value2.X;
      vector2D.Y = value1.Y * value2.Y;
      return vector2D;
    }

    public static void Multiply(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
    }

    public static Vector2D Multiply(Vector2D value1, double scaleFactor)
    {
      Vector2D vector2D;
      vector2D.X = value1.X * scaleFactor;
      vector2D.Y = value1.Y * scaleFactor;
      return vector2D;
    }

    public static void Multiply(ref Vector2D value1, double scaleFactor, out Vector2D result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
    }

    public static Vector2D Divide(Vector2D value1, Vector2D value2)
    {
      Vector2D vector2D;
      vector2D.X = value1.X / value2.X;
      vector2D.Y = value1.Y / value2.Y;
      return vector2D;
    }

    public static void Divide(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
    }

    public static Vector2D Divide(Vector2D value1, double divider)
    {
      double num = 1.0 / divider;
      Vector2D vector2D;
      vector2D.X = value1.X * num;
      vector2D.Y = value1.Y * num;
      return vector2D;
    }

    public static void Divide(ref Vector2D value1, double divider, out Vector2D result)
    {
      double num = 1.0 / divider;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
    }

    public bool Between(ref Vector2D start, ref Vector2D end)
    {
      if (this.X >= start.X && this.X <= end.X)
        return true;
      return this.Y >= start.Y && this.Y <= end.Y;
    }

    public static Vector2D Floor(Vector2D position) => new Vector2D(Math.Floor(position.X), Math.Floor(position.Y));

    public void Rotate(double angle)
    {
      double x = this.X;
      this.X = this.X * Math.Cos(angle) - this.Y * Math.Sin(angle);
      this.Y = this.Y * Math.Cos(angle) + x * Math.Sin(angle);
    }

    protected class VRageMath_Vector2D\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector2D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2D owner, in double value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2D owner, out double value) => value = owner.X;
    }

    protected class VRageMath_Vector2D\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector2D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2D owner, in double value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2D owner, out double value) => value = owner.Y;
    }
  }
}
