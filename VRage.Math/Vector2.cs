// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector2
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
  public struct Vector2 : IEquatable<Vector2>
  {
    public static Vector2 Zero = new Vector2();
    public static Vector2 One = new Vector2(1f, 1f);
    public static Vector2 UnitX = new Vector2(1f, 0.0f);
    public static Vector2 UnitY = new Vector2(0.0f, 1f);
    public static Vector2 PositiveInfinity = Vector2.One * float.PositiveInfinity;
    [ProtoMember(1)]
    public float X;
    [ProtoMember(4)]
    public float Y;

    public Vector2(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public Vector2(float value) => this.X = this.Y = value;

    public float this[int index]
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

    public static explicit operator Vector2I(Vector2 vector) => new Vector2I(vector);

    public static Vector2 operator -(Vector2 value)
    {
      Vector2 vector2;
      vector2.X = -value.X;
      vector2.Y = -value.Y;
      return vector2;
    }

    public static bool operator ==(Vector2 value1, Vector2 value2) => (double) value1.X == (double) value2.X && (double) value1.Y == (double) value2.Y;

    public static bool operator !=(Vector2 value1, Vector2 value2) => (double) value1.X != (double) value2.X || (double) value1.Y != (double) value2.Y;

    public static Vector2 operator +(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X + value2.X;
      vector2.Y = value1.Y + value2.Y;
      return vector2;
    }

    public static Vector2 operator +(Vector2 value1, float value2)
    {
      Vector2 vector2;
      vector2.X = value1.X + value2;
      vector2.Y = value1.Y + value2;
      return vector2;
    }

    public static Vector2 operator -(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X - value2.X;
      vector2.Y = value1.Y - value2.Y;
      return vector2;
    }

    public static Vector2 operator -(Vector2 value1, float value2)
    {
      Vector2 vector2;
      vector2.X = value1.X - value2;
      vector2.Y = value1.Y - value2;
      return vector2;
    }

    public static Vector2 operator *(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X * value2.X;
      vector2.Y = value1.Y * value2.Y;
      return vector2;
    }

    public static Vector2 operator *(Vector2 value, float scaleFactor)
    {
      Vector2 vector2;
      vector2.X = value.X * scaleFactor;
      vector2.Y = value.Y * scaleFactor;
      return vector2;
    }

    public static Vector2 operator *(float scaleFactor, Vector2 value)
    {
      Vector2 vector2;
      vector2.X = value.X * scaleFactor;
      vector2.Y = value.Y * scaleFactor;
      return vector2;
    }

    public static Vector2 operator /(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X / value2.X;
      vector2.Y = value1.Y / value2.Y;
      return vector2;
    }

    public static Vector2 operator /(Vector2 value1, float divider)
    {
      float num = 1f / divider;
      Vector2 vector2;
      vector2.X = value1.X * num;
      vector2.Y = value1.Y * num;
      return vector2;
    }

    public static Vector2 operator /(float value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1 / value2.X;
      vector2.Y = value1 / value2.Y;
      return vector2;
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

    public bool Equals(Vector2 other) => (double) this.X == (double) other.X && (double) this.Y == (double) other.Y;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Vector2 other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public bool IsValid() => (this.X * this.Y).IsValid();

    [Conditional("DEBUG")]
    public void AssertIsValid()
    {
    }

    public float Length() => (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);

    public float LengthSquared() => (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);

    public static float Distance(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      float num3 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      result = (float) Math.Sqrt((double) num3);
    }

    public static float DistanceSquared(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      result = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public static float Dot(Vector2 value1, Vector2 value2) => (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);

    public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result) => result = (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);

    public void Normalize()
    {
      float num = 1f / (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
      this.X *= num;
      this.Y *= num;
    }

    public static Vector2 Normalize(Vector2 value)
    {
      float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
      Vector2 vector2;
      vector2.X = value.X * num;
      vector2.Y = value.Y * num;
      return vector2;
    }

    public static void Normalize(ref Vector2 value, out Vector2 result)
    {
      float num = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
      result.X = value.X * num;
      result.Y = value.Y * num;
    }

    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y);
      Vector2 vector2;
      vector2.X = vector.X - 2f * num * normal.X;
      vector2.Y = vector.Y - 2f * num * normal.Y;
      return vector2;
    }

    public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
    {
      float num = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y);
      result.X = vector.X - 2f * num * normal.X;
      result.Y = vector.Y - 2f * num * normal.Y;
    }

    public static Vector2 Min(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = (double) value1.X < (double) value2.X ? value1.X : value2.X;
      vector2.Y = (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y;
      return vector2;
    }

    public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = (double) value1.X < (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2 Max(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      vector2.Y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
      return vector2;
    }

    public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      result.Y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
    }

    public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
    {
      float x = value1.X;
      float num1 = (double) x > (double) max.X ? max.X : x;
      float num2 = (double) num1 < (double) min.X ? min.X : num1;
      float y = value1.Y;
      float num3 = (double) y > (double) max.Y ? max.Y : y;
      float num4 = (double) num3 < (double) min.Y ? min.Y : num3;
      Vector2 vector2;
      vector2.X = num2;
      vector2.Y = num4;
      return vector2;
    }

    public static void Clamp(
      ref Vector2 value1,
      ref Vector2 min,
      ref Vector2 max,
      out Vector2 result)
    {
      float x = value1.X;
      float num1 = (double) x > (double) max.X ? max.X : x;
      float num2 = (double) num1 < (double) min.X ? min.X : num1;
      float y = value1.Y;
      float num3 = (double) y > (double) max.Y ? max.Y : y;
      float num4 = (double) num3 < (double) min.Y ? min.Y : num3;
      result.X = num2;
      result.Y = num4;
    }

    public static Vector2 ClampToSphere(Vector2 vector, float radius)
    {
      float num1 = vector.LengthSquared();
      float num2 = radius * radius;
      return (double) num1 > (double) num2 ? vector * (float) Math.Sqrt((double) num2 / (double) num1) : vector;
    }

    public static void ClampToSphere(ref Vector2 vector, float radius)
    {
      float num1 = vector.LengthSquared();
      float num2 = radius * radius;
      if ((double) num1 <= (double) num2)
        return;
      vector *= (float) Math.Sqrt((double) num2 / (double) num1);
    }

    public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
    {
      Vector2 vector2;
      vector2.X = value1.X + (value2.X - value1.X) * amount;
      vector2.Y = value1.Y + (value2.Y - value1.Y) * amount;
      return vector2;
    }

    public static void Lerp(
      ref Vector2 value1,
      ref Vector2 value2,
      float amount,
      out Vector2 result)
    {
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
    }

    public static Vector2 Barycentric(
      Vector2 value1,
      Vector2 value2,
      Vector2 value3,
      float amount1,
      float amount2)
    {
      Vector2 vector2;
      vector2.X = (float) ((double) value1.X + (double) amount1 * ((double) value2.X - (double) value1.X) + (double) amount2 * ((double) value3.X - (double) value1.X));
      vector2.Y = (float) ((double) value1.Y + (double) amount1 * ((double) value2.Y - (double) value1.Y) + (double) amount2 * ((double) value3.Y - (double) value1.Y));
      return vector2;
    }

    public static void Barycentric(
      ref Vector2 value1,
      ref Vector2 value2,
      ref Vector2 value3,
      float amount1,
      float amount2,
      out Vector2 result)
    {
      result.X = (float) ((double) value1.X + (double) amount1 * ((double) value2.X - (double) value1.X) + (double) amount2 * ((double) value3.X - (double) value1.X));
      result.Y = (float) ((double) value1.Y + (double) amount1 * ((double) value2.Y - (double) value1.Y) + (double) amount2 * ((double) value3.Y - (double) value1.Y));
    }

    public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      Vector2 vector2;
      vector2.X = value1.X + (value2.X - value1.X) * amount;
      vector2.Y = value1.Y + (value2.Y - value1.Y) * amount;
      return vector2;
    }

    public static void SmoothStep(
      ref Vector2 value1,
      ref Vector2 value2,
      float amount,
      out Vector2 result)
    {
      amount = (double) amount > 1.0 ? 1f : ((double) amount < 0.0 ? 0.0f : amount);
      amount = (float) ((double) amount * (double) amount * (3.0 - 2.0 * (double) amount));
      result.X = value1.X + (value2.X - value1.X) * amount;
      result.Y = value1.Y + (value2.Y - value1.Y) * amount;
    }

    public static Vector2 CatmullRom(
      Vector2 value1,
      Vector2 value2,
      Vector2 value3,
      Vector2 value4,
      float amount)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      Vector2 vector2;
      vector2.X = (float) (0.5 * (2.0 * (double) value2.X + (-(double) value1.X + (double) value3.X) * (double) amount + (2.0 * (double) value1.X - 5.0 * (double) value2.X + 4.0 * (double) value3.X - (double) value4.X) * (double) num1 + (-(double) value1.X + 3.0 * (double) value2.X - 3.0 * (double) value3.X + (double) value4.X) * (double) num2));
      vector2.Y = (float) (0.5 * (2.0 * (double) value2.Y + (-(double) value1.Y + (double) value3.Y) * (double) amount + (2.0 * (double) value1.Y - 5.0 * (double) value2.Y + 4.0 * (double) value3.Y - (double) value4.Y) * (double) num1 + (-(double) value1.Y + 3.0 * (double) value2.Y - 3.0 * (double) value3.Y + (double) value4.Y) * (double) num2));
      return vector2;
    }

    public static void CatmullRom(
      ref Vector2 value1,
      ref Vector2 value2,
      ref Vector2 value3,
      ref Vector2 value4,
      float amount,
      out Vector2 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      result.X = (float) (0.5 * (2.0 * (double) value2.X + (-(double) value1.X + (double) value3.X) * (double) amount + (2.0 * (double) value1.X - 5.0 * (double) value2.X + 4.0 * (double) value3.X - (double) value4.X) * (double) num1 + (-(double) value1.X + 3.0 * (double) value2.X - 3.0 * (double) value3.X + (double) value4.X) * (double) num2));
      result.Y = (float) (0.5 * (2.0 * (double) value2.Y + (-(double) value1.Y + (double) value3.Y) * (double) amount + (2.0 * (double) value1.Y - 5.0 * (double) value2.Y + 4.0 * (double) value3.Y - (double) value4.Y) * (double) num1 + (-(double) value1.Y + 3.0 * (double) value2.Y - 3.0 * (double) value3.Y + (double) value4.Y) * (double) num2));
    }

    public static Vector2 Hermite(
      Vector2 value1,
      Vector2 tangent1,
      Vector2 value2,
      Vector2 tangent2,
      float amount)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + amount;
      float num6 = num2 - num1;
      Vector2 vector2;
      vector2.X = (float) ((double) value1.X * (double) num3 + (double) value2.X * (double) num4 + (double) tangent1.X * (double) num5 + (double) tangent2.X * (double) num6);
      vector2.Y = (float) ((double) value1.Y * (double) num3 + (double) value2.Y * (double) num4 + (double) tangent1.Y * (double) num5 + (double) tangent2.Y * (double) num6);
      return vector2;
    }

    public static void Hermite(
      ref Vector2 value1,
      ref Vector2 tangent1,
      ref Vector2 value2,
      ref Vector2 tangent2,
      float amount,
      out Vector2 result)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + amount;
      float num6 = num2 - num1;
      result.X = (float) ((double) value1.X * (double) num3 + (double) value2.X * (double) num4 + (double) tangent1.X * (double) num5 + (double) tangent2.X * (double) num6);
      result.Y = (float) ((double) value1.Y * (double) num3 + (double) value2.Y * (double) num4 + (double) tangent1.Y * (double) num5 + (double) tangent2.Y * (double) num6);
    }

    public static Vector2 Transform(Vector2 position, Matrix matrix)
    {
      float num1 = (float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M41;
      float num2 = (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M42;
      Vector2 vector2;
      vector2.X = num1;
      vector2.Y = num2;
      return vector2;
    }

    public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
    {
      float num1 = (float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M41;
      float num2 = (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M42;
      result.X = num1;
      result.Y = num2;
    }

    public static Vector2 TransformNormal(Vector2 normal, Matrix matrix)
    {
      float num1 = (float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21);
      float num2 = (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22);
      Vector2 vector2;
      vector2.X = num1;
      vector2.Y = num2;
      return vector2;
    }

    public static void TransformNormal(ref Vector2 normal, ref Matrix matrix, out Vector2 result)
    {
      float num1 = (float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21);
      float num2 = (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22);
      result.X = num1;
      result.Y = num2;
    }

    public static Vector2 Transform(Vector2 value, Quaternion rotation)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      float num6 = rotation.X * num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      float num9 = (float) ((double) value.X * (1.0 - (double) num7 - (double) num8) + (double) value.Y * ((double) num6 - (double) num4));
      float num10 = (float) ((double) value.X * ((double) num6 + (double) num4) + (double) value.Y * (1.0 - (double) num5 - (double) num8));
      Vector2 vector2;
      vector2.X = num9;
      vector2.Y = num10;
      return vector2;
    }

    public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector2 result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      float num6 = rotation.X * num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      float num9 = (float) ((double) value.X * (1.0 - (double) num7 - (double) num8) + (double) value.Y * ((double) num6 - (double) num4));
      float num10 = (float) ((double) value.X * ((double) num6 + (double) num4) + (double) value.Y * (1.0 - (double) num5 - (double) num8));
      result.X = num9;
      result.Y = num10;
    }

    public static void Transform(
      Vector2[] sourceArray,
      ref Matrix matrix,
      Vector2[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        float x = sourceArray[index].X;
        float y = sourceArray[index].Y;
        destinationArray[index].X = (float) ((double) x * (double) matrix.M11 + (double) y * (double) matrix.M21) + matrix.M41;
        destinationArray[index].Y = (float) ((double) x * (double) matrix.M12 + (double) y * (double) matrix.M22) + matrix.M42;
      }
    }

    public static void Transform(
      Vector2[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector2[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        float x = sourceArray[sourceIndex].X;
        float y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = (float) ((double) x * (double) matrix.M11 + (double) y * (double) matrix.M21) + matrix.M41;
        destinationArray[destinationIndex].Y = (float) ((double) x * (double) matrix.M12 + (double) y * (double) matrix.M22) + matrix.M42;
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void TransformNormal(
      Vector2[] sourceArray,
      ref Matrix matrix,
      Vector2[] destinationArray)
    {
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        float x = sourceArray[index].X;
        float y = sourceArray[index].Y;
        destinationArray[index].X = (float) ((double) x * (double) matrix.M11 + (double) y * (double) matrix.M21);
        destinationArray[index].Y = (float) ((double) x * (double) matrix.M12 + (double) y * (double) matrix.M22);
      }
    }

    public static void TransformNormal(
      Vector2[] sourceArray,
      int sourceIndex,
      ref Matrix matrix,
      Vector2[] destinationArray,
      int destinationIndex,
      int length)
    {
      for (; length > 0; --length)
      {
        float x = sourceArray[sourceIndex].X;
        float y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = (float) ((double) x * (double) matrix.M11 + (double) y * (double) matrix.M21);
        destinationArray[destinationIndex].Y = (float) ((double) x * (double) matrix.M12 + (double) y * (double) matrix.M22);
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static void Transform(
      Vector2[] sourceArray,
      ref Quaternion rotation,
      Vector2[] destinationArray)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      double num6 = (double) rotation.X * (double) num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      float num9 = 1f - num7 - num8;
      float num10 = (float) num6 - num4;
      float num11 = (float) num6 + num4;
      float num12 = 1f - num5 - num8;
      for (int index = 0; index < sourceArray.Length; ++index)
      {
        float x = sourceArray[index].X;
        float y = sourceArray[index].Y;
        destinationArray[index].X = (float) ((double) x * (double) num9 + (double) y * (double) num10);
        destinationArray[index].Y = (float) ((double) x * (double) num11 + (double) y * (double) num12);
      }
    }

    public static void Transform(
      Vector2[] sourceArray,
      int sourceIndex,
      ref Quaternion rotation,
      Vector2[] destinationArray,
      int destinationIndex,
      int length)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num3;
      float num5 = rotation.X * num1;
      double num6 = (double) rotation.X * (double) num2;
      float num7 = rotation.Y * num2;
      float num8 = rotation.Z * num3;
      float num9 = 1f - num7 - num8;
      float num10 = (float) num6 - num4;
      float num11 = (float) num6 + num4;
      float num12 = 1f - num5 - num8;
      for (; length > 0; --length)
      {
        float x = sourceArray[sourceIndex].X;
        float y = sourceArray[sourceIndex].Y;
        destinationArray[destinationIndex].X = (float) ((double) x * (double) num9 + (double) y * (double) num10);
        destinationArray[destinationIndex].Y = (float) ((double) x * (double) num11 + (double) y * (double) num12);
        ++sourceIndex;
        ++destinationIndex;
      }
    }

    public static Vector2 Negate(Vector2 value)
    {
      Vector2 vector2;
      vector2.X = -value.X;
      vector2.Y = -value.Y;
      return vector2;
    }

    public static void Negate(ref Vector2 value, out Vector2 result)
    {
      result.X = -value.X;
      result.Y = -value.Y;
    }

    public static Vector2 Add(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X + value2.X;
      vector2.Y = value1.Y + value2.Y;
      return vector2;
    }

    public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X + value2.X;
      result.Y = value1.Y + value2.Y;
    }

    public static Vector2 Subtract(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X - value2.X;
      vector2.Y = value1.Y - value2.Y;
      return vector2;
    }

    public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X - value2.X;
      result.Y = value1.Y - value2.Y;
    }

    public static Vector2 Multiply(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X * value2.X;
      vector2.Y = value1.Y * value2.Y;
      return vector2;
    }

    public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X * value2.X;
      result.Y = value1.Y * value2.Y;
    }

    public static Vector2 Multiply(Vector2 value1, float scaleFactor)
    {
      Vector2 vector2;
      vector2.X = value1.X * scaleFactor;
      vector2.Y = value1.Y * scaleFactor;
      return vector2;
    }

    public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
    {
      result.X = value1.X * scaleFactor;
      result.Y = value1.Y * scaleFactor;
    }

    public static Vector2 Divide(Vector2 value1, Vector2 value2)
    {
      Vector2 vector2;
      vector2.X = value1.X / value2.X;
      vector2.Y = value1.Y / value2.Y;
      return vector2;
    }

    public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
    {
      result.X = value1.X / value2.X;
      result.Y = value1.Y / value2.Y;
    }

    public static Vector2 Divide(Vector2 value1, float divider)
    {
      float num = 1f / divider;
      Vector2 vector2;
      vector2.X = value1.X * num;
      vector2.Y = value1.Y * num;
      return vector2;
    }

    public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
    {
      float num = 1f / divider;
      result.X = value1.X * num;
      result.Y = value1.Y * num;
    }

    public bool Between(ref Vector2 start, ref Vector2 end)
    {
      if ((double) this.X >= (double) start.X && (double) this.X <= (double) end.X)
        return true;
      return (double) this.Y >= (double) start.Y && (double) this.Y <= (double) end.Y;
    }

    public static Vector2 Floor(Vector2 position) => new Vector2((float) Math.Floor((double) position.X), (float) Math.Floor((double) position.Y));

    public void Rotate(double angle)
    {
      float x = this.X;
      this.X = (float) ((double) this.X * Math.Cos(angle) - (double) this.Y * Math.Sin(angle));
      this.Y = (float) ((double) this.Y * Math.Cos(angle) + (double) x * Math.Sin(angle));
    }

    public static bool IsZero(ref Vector2 value) => Vector2.IsZero(ref value, 0.0001f);

    public static bool IsZero(ref Vector2 value, float epsilon) => (double) Math.Abs(value.X) < (double) epsilon && (double) Math.Abs(value.Y) < (double) epsilon;

    public static bool IsZero(Vector2 value, float epsilon) => (double) Math.Abs(value.X) < (double) epsilon && (double) Math.Abs(value.Y) < (double) epsilon;

    public static Vector2 SignNonZero(Vector2 value) => new Vector2((double) value.X < 0.0 ? -1f : 1f, (double) value.Y < 0.0 ? -1f : 1f);

    protected class VRageMath_Vector2\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2 owner, in float value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2 owner, out float value) => value = owner.X;
    }

    protected class VRageMath_Vector2\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector2 owner, in float value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector2 owner, out float value) => value = owner.Y;
    }
  }
}
