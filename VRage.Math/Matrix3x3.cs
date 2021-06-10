// Decompiled with JetBrains decompiler
// Type: VRageMath.Matrix3x3
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  [StructLayout(LayoutKind.Explicit)]
  public struct Matrix3x3 : IEquatable<Matrix3x3>
  {
    public static Matrix3x3 Identity = new Matrix3x3(1f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 1f);
    public static Matrix3x3 Zero = new Matrix3x3(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    [FieldOffset(0)]
    private Matrix3x3.F9 M;
    [ProtoMember(1)]
    [FieldOffset(0)]
    public float M11;
    [ProtoMember(4)]
    [FieldOffset(4)]
    public float M12;
    [ProtoMember(7)]
    [FieldOffset(8)]
    public float M13;
    [ProtoMember(10)]
    [FieldOffset(12)]
    public float M21;
    [ProtoMember(13)]
    [FieldOffset(16)]
    public float M22;
    [ProtoMember(16)]
    [FieldOffset(20)]
    public float M23;
    [ProtoMember(19)]
    [FieldOffset(24)]
    public float M31;
    [ProtoMember(22)]
    [FieldOffset(28)]
    public float M32;
    [ProtoMember(25)]
    [FieldOffset(32)]
    public float M33;

    public Vector3 Up
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M21;
        vector3.Y = this.M22;
        vector3.Z = this.M23;
        return vector3;
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
        this.M23 = value.Z;
      }
    }

    public Vector3 Down
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M21;
        vector3.Y = -this.M22;
        vector3.Z = -this.M23;
        return vector3;
      }
      set
      {
        this.M21 = -value.X;
        this.M22 = -value.Y;
        this.M23 = -value.Z;
      }
    }

    public Vector3 Right
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M11;
        vector3.Y = this.M12;
        vector3.Z = this.M13;
        return vector3;
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
        this.M13 = value.Z;
      }
    }

    public Vector3 Col0
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M11;
        vector3.Y = this.M21;
        vector3.Z = this.M31;
        return vector3;
      }
    }

    public Vector3 Col1
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M12;
        vector3.Y = this.M22;
        vector3.Z = this.M32;
        return vector3;
      }
    }

    public Vector3 Col2
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M13;
        vector3.Y = this.M23;
        vector3.Z = this.M33;
        return vector3;
      }
    }

    public Vector3 Left
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M11;
        vector3.Y = -this.M12;
        vector3.Z = -this.M13;
        return vector3;
      }
      set
      {
        this.M11 = -value.X;
        this.M12 = -value.Y;
        this.M13 = -value.Z;
      }
    }

    public Vector3 Forward
    {
      get
      {
        Vector3 vector3;
        vector3.X = -this.M31;
        vector3.Y = -this.M32;
        vector3.Z = -this.M33;
        return vector3;
      }
      set
      {
        this.M31 = -value.X;
        this.M32 = -value.Y;
        this.M33 = -value.Z;
      }
    }

    public Vector3 Backward
    {
      get
      {
        Vector3 vector3;
        vector3.X = this.M31;
        vector3.Y = this.M32;
        vector3.Z = this.M33;
        return vector3;
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
        this.M33 = value.Z;
      }
    }

    public Vector3 GetDirectionVector(Base6Directions.Direction direction)
    {
      switch (direction)
      {
        case Base6Directions.Direction.Forward:
          return this.Forward;
        case Base6Directions.Direction.Backward:
          return this.Backward;
        case Base6Directions.Direction.Left:
          return this.Left;
        case Base6Directions.Direction.Right:
          return this.Right;
        case Base6Directions.Direction.Up:
          return this.Up;
        case Base6Directions.Direction.Down:
          return this.Down;
        default:
          return Vector3.Zero;
      }
    }

    public void SetDirectionVector(Base6Directions.Direction direction, Vector3 newValue)
    {
      switch (direction)
      {
        case Base6Directions.Direction.Forward:
          this.Forward = newValue;
          break;
        case Base6Directions.Direction.Backward:
          this.Backward = newValue;
          break;
        case Base6Directions.Direction.Left:
          this.Left = newValue;
          break;
        case Base6Directions.Direction.Right:
          this.Right = newValue;
          break;
        case Base6Directions.Direction.Up:
          this.Up = newValue;
          break;
        case Base6Directions.Direction.Down:
          this.Down = newValue;
          break;
      }
    }

    public Base6Directions.Direction GetClosestDirection(Vector3 referenceVector) => this.GetClosestDirection(ref referenceVector);

    public Base6Directions.Direction GetClosestDirection(ref Vector3 referenceVector)
    {
      float num1 = Vector3.Dot(referenceVector, this.Right);
      float num2 = Vector3.Dot(referenceVector, this.Up);
      float num3 = Vector3.Dot(referenceVector, this.Backward);
      float num4 = Math.Abs(num1);
      float num5 = Math.Abs(num2);
      float num6 = Math.Abs(num3);
      return (double) num4 > (double) num5 ? ((double) num4 > (double) num6 ? ((double) num1 > 0.0 ? Base6Directions.Direction.Right : Base6Directions.Direction.Left) : ((double) num3 > 0.0 ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward)) : ((double) num5 > (double) num6 ? ((double) num2 > 0.0 ? Base6Directions.Direction.Up : Base6Directions.Direction.Down) : ((double) num3 > 0.0 ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward));
    }

    public Vector3 Scale
    {
      get
      {
        Vector3 vector3 = this.Right;
        double num1 = (double) vector3.Length();
        vector3 = this.Up;
        double num2 = (double) vector3.Length();
        vector3 = this.Forward;
        double num3 = (double) vector3.Length();
        return new Vector3((float) num1, (float) num2, (float) num3);
      }
    }

    public static void Rescale(ref Matrix3x3 matrix, float scale)
    {
      matrix.M11 *= scale;
      matrix.M12 *= scale;
      matrix.M13 *= scale;
      matrix.M21 *= scale;
      matrix.M22 *= scale;
      matrix.M23 *= scale;
      matrix.M31 *= scale;
      matrix.M32 *= scale;
      matrix.M33 *= scale;
    }

    public static void Rescale(ref Matrix3x3 matrix, ref Vector3 scale)
    {
      matrix.M11 *= scale.X;
      matrix.M12 *= scale.X;
      matrix.M13 *= scale.X;
      matrix.M21 *= scale.Y;
      matrix.M22 *= scale.Y;
      matrix.M23 *= scale.Y;
      matrix.M31 *= scale.Z;
      matrix.M32 *= scale.Z;
      matrix.M33 *= scale.Z;
    }

    public static Matrix3x3 Rescale(Matrix3x3 matrix, float scale)
    {
      Matrix3x3.Rescale(ref matrix, scale);
      return matrix;
    }

    public static Matrix3x3 Rescale(Matrix3x3 matrix, Vector3 scale)
    {
      Matrix3x3.Rescale(ref matrix, ref scale);
      return matrix;
    }

    public Matrix3x3(
      float m11,
      float m12,
      float m13,
      float m21,
      float m22,
      float m23,
      float m31,
      float m32,
      float m33)
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M13 = m13;
      this.M21 = m21;
      this.M22 = m22;
      this.M23 = m23;
      this.M31 = m31;
      this.M32 = m32;
      this.M33 = m33;
    }

    public Matrix3x3(Matrix3x3 other)
    {
      this.M11 = other.M11;
      this.M12 = other.M12;
      this.M13 = other.M13;
      this.M21 = other.M21;
      this.M22 = other.M22;
      this.M23 = other.M23;
      this.M31 = other.M31;
      this.M32 = other.M32;
      this.M33 = other.M33;
    }

    public Matrix3x3(MatrixD other)
    {
      this.M11 = (float) other.M11;
      this.M12 = (float) other.M12;
      this.M13 = (float) other.M13;
      this.M21 = (float) other.M21;
      this.M22 = (float) other.M22;
      this.M23 = (float) other.M23;
      this.M31 = (float) other.M31;
      this.M32 = (float) other.M32;
      this.M33 = (float) other.M33;
    }

    public static Matrix3x3 CreateScale(float xScale, float yScale, float zScale)
    {
      float num1 = xScale;
      float num2 = yScale;
      float num3 = zScale;
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = num1;
      matrix3x3.M12 = 0.0f;
      matrix3x3.M13 = 0.0f;
      matrix3x3.M21 = 0.0f;
      matrix3x3.M22 = num2;
      matrix3x3.M23 = 0.0f;
      matrix3x3.M31 = 0.0f;
      matrix3x3.M32 = 0.0f;
      matrix3x3.M33 = num3;
      return matrix3x3;
    }

    public static void CreateScale(float xScale, float yScale, float zScale, out Matrix3x3 result)
    {
      float num1 = xScale;
      float num2 = yScale;
      float num3 = zScale;
      result.M11 = num1;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = num2;
      result.M23 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = num3;
    }

    public static Matrix3x3 CreateScale(Vector3 scales)
    {
      float x = scales.X;
      float y = scales.Y;
      float z = scales.Z;
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = x;
      matrix3x3.M12 = 0.0f;
      matrix3x3.M13 = 0.0f;
      matrix3x3.M21 = 0.0f;
      matrix3x3.M22 = y;
      matrix3x3.M23 = 0.0f;
      matrix3x3.M31 = 0.0f;
      matrix3x3.M32 = 0.0f;
      matrix3x3.M33 = z;
      return matrix3x3;
    }

    public static void CreateScale(ref Vector3 scales, out Matrix3x3 result)
    {
      float x = scales.X;
      float y = scales.Y;
      float z = scales.Z;
      result.M11 = x;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = y;
      result.M23 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = z;
    }

    public static Matrix3x3 CreateScale(float scale)
    {
      float num = scale;
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = num;
      matrix3x3.M12 = 0.0f;
      matrix3x3.M13 = 0.0f;
      matrix3x3.M21 = 0.0f;
      matrix3x3.M22 = num;
      matrix3x3.M23 = 0.0f;
      matrix3x3.M31 = 0.0f;
      matrix3x3.M32 = 0.0f;
      matrix3x3.M33 = num;
      return matrix3x3;
    }

    public static void CreateScale(float scale, out Matrix3x3 result)
    {
      float num = scale;
      result.M11 = num;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = num;
      result.M23 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = num;
    }

    public static Matrix3x3 CreateRotationX(float radians)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = 1f;
      matrix3x3.M12 = 0.0f;
      matrix3x3.M13 = 0.0f;
      matrix3x3.M21 = 0.0f;
      matrix3x3.M22 = num1;
      matrix3x3.M23 = num2;
      matrix3x3.M31 = 0.0f;
      matrix3x3.M32 = -num2;
      matrix3x3.M33 = num1;
      return matrix3x3;
    }

    public static void CreateRotationX(float radians, out Matrix3x3 result)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M11 = 1f;
      result.M12 = 0.0f;
      result.M13 = 0.0f;
      result.M21 = 0.0f;
      result.M22 = num1;
      result.M23 = num2;
      result.M31 = 0.0f;
      result.M32 = -num2;
      result.M33 = num1;
    }

    public static Matrix3x3 CreateRotationY(float radians)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = num1;
      matrix3x3.M12 = 0.0f;
      matrix3x3.M13 = -num2;
      matrix3x3.M21 = 0.0f;
      matrix3x3.M22 = 1f;
      matrix3x3.M23 = 0.0f;
      matrix3x3.M31 = num2;
      matrix3x3.M32 = 0.0f;
      matrix3x3.M33 = num1;
      return matrix3x3;
    }

    public static void CreateRotationY(float radians, out Matrix3x3 result)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M11 = num1;
      result.M12 = 0.0f;
      result.M13 = -num2;
      result.M21 = 0.0f;
      result.M22 = 1f;
      result.M23 = 0.0f;
      result.M31 = num2;
      result.M32 = 0.0f;
      result.M33 = num1;
    }

    public static Matrix3x3 CreateRotationZ(float radians)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = num1;
      matrix3x3.M12 = num2;
      matrix3x3.M13 = 0.0f;
      matrix3x3.M21 = -num2;
      matrix3x3.M22 = num1;
      matrix3x3.M23 = 0.0f;
      matrix3x3.M31 = 0.0f;
      matrix3x3.M32 = 0.0f;
      matrix3x3.M33 = 1f;
      return matrix3x3;
    }

    public static void CreateRotationZ(float radians, out Matrix3x3 result)
    {
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = 0.0f;
      result.M21 = -num2;
      result.M22 = num1;
      result.M23 = 0.0f;
      result.M31 = 0.0f;
      result.M32 = 0.0f;
      result.M33 = 1f;
    }

    public static Matrix3x3 CreateFromAxisAngle(Vector3 axis, float angle)
    {
      float x = axis.X;
      float y = axis.Y;
      float z = axis.Z;
      float num1 = (float) Math.Sin((double) angle);
      float num2 = (float) Math.Cos((double) angle);
      float num3 = x * x;
      float num4 = y * y;
      float num5 = z * z;
      float num6 = x * y;
      float num7 = x * z;
      float num8 = y * z;
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = num3 + num2 * (1f - num3);
      matrix3x3.M12 = (float) ((double) num6 - (double) num2 * (double) num6 + (double) num1 * (double) z);
      matrix3x3.M13 = (float) ((double) num7 - (double) num2 * (double) num7 - (double) num1 * (double) y);
      matrix3x3.M21 = (float) ((double) num6 - (double) num2 * (double) num6 - (double) num1 * (double) z);
      matrix3x3.M22 = num4 + num2 * (1f - num4);
      matrix3x3.M23 = (float) ((double) num8 - (double) num2 * (double) num8 + (double) num1 * (double) x);
      matrix3x3.M31 = (float) ((double) num7 - (double) num2 * (double) num7 + (double) num1 * (double) y);
      matrix3x3.M32 = (float) ((double) num8 - (double) num2 * (double) num8 - (double) num1 * (double) x);
      matrix3x3.M33 = num5 + num2 * (1f - num5);
      return matrix3x3;
    }

    public static void CreateFromAxisAngle(ref Vector3 axis, float angle, out Matrix3x3 result)
    {
      float x = axis.X;
      float y = axis.Y;
      float z = axis.Z;
      float num1 = (float) Math.Sin((double) angle);
      float num2 = (float) Math.Cos((double) angle);
      float num3 = x * x;
      float num4 = y * y;
      float num5 = z * z;
      float num6 = x * y;
      float num7 = x * z;
      float num8 = y * z;
      result.M11 = num3 + num2 * (1f - num3);
      result.M12 = (float) ((double) num6 - (double) num2 * (double) num6 + (double) num1 * (double) z);
      result.M13 = (float) ((double) num7 - (double) num2 * (double) num7 - (double) num1 * (double) y);
      result.M21 = (float) ((double) num6 - (double) num2 * (double) num6 - (double) num1 * (double) z);
      result.M22 = num4 + num2 * (1f - num4);
      result.M23 = (float) ((double) num8 - (double) num2 * (double) num8 + (double) num1 * (double) x);
      result.M31 = (float) ((double) num7 - (double) num2 * (double) num7 + (double) num1 * (double) y);
      result.M32 = (float) ((double) num8 - (double) num2 * (double) num8 - (double) num1 * (double) x);
      result.M33 = num5 + num2 * (1f - num5);
    }

    public static void CreateRotationFromTwoVectors(
      ref Vector3 fromVector,
      ref Vector3 toVector,
      out Matrix3x3 resultMatrix)
    {
      Vector3 vector1 = Vector3.Normalize(fromVector);
      Vector3 vector3 = Vector3.Normalize(toVector);
      Vector3 result1;
      Vector3.Cross(ref vector1, ref vector3, out result1);
      double num = (double) result1.Normalize();
      Vector3 result2;
      Vector3.Cross(ref vector1, ref result1, out result2);
      Matrix3x3 matrix1 = new Matrix3x3(vector1.X, result1.X, result2.X, vector1.Y, result1.Y, result2.Y, vector1.Z, result1.Z, result2.Z);
      Vector3.Cross(ref vector3, ref result1, out result2);
      Matrix3x3 matrix2 = new Matrix3x3(vector3.X, vector3.Y, vector3.Z, result1.X, result1.Y, result1.Z, result2.X, result2.Y, result2.Z);
      Matrix3x3.Multiply(ref matrix1, ref matrix2, out resultMatrix);
    }

    public static Matrix3x3 CreateFromQuaternion(Quaternion quaternion)
    {
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.Y * quaternion.Y;
      float num3 = quaternion.Z * quaternion.Z;
      float num4 = quaternion.X * quaternion.Y;
      float num5 = quaternion.Z * quaternion.W;
      float num6 = quaternion.Z * quaternion.X;
      float num7 = quaternion.Y * quaternion.W;
      float num8 = quaternion.Y * quaternion.Z;
      float num9 = quaternion.X * quaternion.W;
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      matrix3x3.M12 = (float) (2.0 * ((double) num4 + (double) num5));
      matrix3x3.M13 = (float) (2.0 * ((double) num6 - (double) num7));
      matrix3x3.M21 = (float) (2.0 * ((double) num4 - (double) num5));
      matrix3x3.M22 = (float) (1.0 - 2.0 * ((double) num3 + (double) num1));
      matrix3x3.M23 = (float) (2.0 * ((double) num8 + (double) num9));
      matrix3x3.M31 = (float) (2.0 * ((double) num6 + (double) num7));
      matrix3x3.M32 = (float) (2.0 * ((double) num8 - (double) num9));
      matrix3x3.M33 = (float) (1.0 - 2.0 * ((double) num2 + (double) num1));
      return matrix3x3;
    }

    public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix3x3 result)
    {
      float num1 = quaternion.X * quaternion.X;
      float num2 = quaternion.Y * quaternion.Y;
      float num3 = quaternion.Z * quaternion.Z;
      float num4 = quaternion.X * quaternion.Y;
      float num5 = quaternion.Z * quaternion.W;
      float num6 = quaternion.Z * quaternion.X;
      float num7 = quaternion.Y * quaternion.W;
      float num8 = quaternion.Y * quaternion.Z;
      float num9 = quaternion.X * quaternion.W;
      result.M11 = (float) (1.0 - 2.0 * ((double) num2 + (double) num3));
      result.M12 = (float) (2.0 * ((double) num4 + (double) num5));
      result.M13 = (float) (2.0 * ((double) num6 - (double) num7));
      result.M21 = (float) (2.0 * ((double) num4 - (double) num5));
      result.M22 = (float) (1.0 - 2.0 * ((double) num3 + (double) num1));
      result.M23 = (float) (2.0 * ((double) num8 + (double) num9));
      result.M31 = (float) (2.0 * ((double) num6 + (double) num7));
      result.M32 = (float) (2.0 * ((double) num8 - (double) num9));
      result.M33 = (float) (1.0 - 2.0 * ((double) num2 + (double) num1));
    }

    public static Matrix3x3 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix3x3 result2;
      Matrix3x3.CreateFromQuaternion(ref result1, out result2);
      return result2;
    }

    public static void CreateFromYawPitchRoll(
      float yaw,
      float pitch,
      float roll,
      out Matrix3x3 result)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out result1);
      Matrix3x3.CreateFromQuaternion(ref result1, out result);
    }

    public static void Transform(
      ref Matrix3x3 value,
      ref Quaternion rotation,
      out Matrix3x3 result)
    {
      float num1 = rotation.X + rotation.X;
      float num2 = rotation.Y + rotation.Y;
      float num3 = rotation.Z + rotation.Z;
      float num4 = rotation.W * num1;
      float num5 = rotation.W * num2;
      float num6 = rotation.W * num3;
      float num7 = rotation.X * num1;
      double num8 = (double) rotation.X * (double) num2;
      float num9 = rotation.X * num3;
      float num10 = rotation.Y * num2;
      float num11 = rotation.Y * num3;
      float num12 = rotation.Z * num3;
      float num13 = 1f - num10 - num12;
      float num14 = (float) num8 - num6;
      float num15 = num9 + num5;
      float num16 = (float) num8 + num6;
      float num17 = 1f - num7 - num12;
      float num18 = num11 - num4;
      float num19 = num9 - num5;
      float num20 = num11 + num4;
      float num21 = 1f - num7 - num10;
      float num22 = (float) ((double) value.M11 * (double) num13 + (double) value.M12 * (double) num14 + (double) value.M13 * (double) num15);
      float num23 = (float) ((double) value.M11 * (double) num16 + (double) value.M12 * (double) num17 + (double) value.M13 * (double) num18);
      float num24 = (float) ((double) value.M11 * (double) num19 + (double) value.M12 * (double) num20 + (double) value.M13 * (double) num21);
      float num25 = (float) ((double) value.M21 * (double) num13 + (double) value.M22 * (double) num14 + (double) value.M23 * (double) num15);
      float num26 = (float) ((double) value.M21 * (double) num16 + (double) value.M22 * (double) num17 + (double) value.M23 * (double) num18);
      float num27 = (float) ((double) value.M21 * (double) num19 + (double) value.M22 * (double) num20 + (double) value.M23 * (double) num21);
      float num28 = (float) ((double) value.M31 * (double) num13 + (double) value.M32 * (double) num14 + (double) value.M33 * (double) num15);
      float num29 = (float) ((double) value.M31 * (double) num16 + (double) value.M32 * (double) num17 + (double) value.M33 * (double) num18);
      float num30 = (float) ((double) value.M31 * (double) num19 + (double) value.M32 * (double) num20 + (double) value.M33 * (double) num21);
      result.M11 = num22;
      result.M12 = num23;
      result.M13 = num24;
      result.M21 = num25;
      result.M22 = num26;
      result.M23 = num27;
      result.M31 = num28;
      result.M32 = num29;
      result.M33 = num30;
    }

    public unsafe Vector3 GetRow(int row)
    {
      if (row < 0 || row > 2)
        throw new ArgumentOutOfRangeException();
      fixed (float* numPtr1 = &this.M11)
      {
        float* numPtr2 = numPtr1 + row * 3;
        return new Vector3(*numPtr2, numPtr2[1], numPtr2[2]);
      }
    }

    public unsafe void SetRow(int row, Vector3 value)
    {
      if (row < 0 || row > 2)
        throw new ArgumentOutOfRangeException();
      fixed (float* numPtr1 = &this.M11)
      {
        float* numPtr2 = numPtr1 + row * 3;
        *numPtr2 = value.X;
        numPtr2[1] = value.Y;
        numPtr2[2] = value.Z;
      }
    }

    public unsafe float this[int row, int column]
    {
      get
      {
        if (row < 0 || row > 2 || (column < 0 || column > 2))
          throw new ArgumentOutOfRangeException();
        fixed (float* numPtr = &this.M11)
          return numPtr[row * 3 + column];
      }
      set
      {
        if (row < 0 || row > 2 || (column < 0 || column > 2))
          throw new ArgumentOutOfRangeException();
        fixed (float* numPtr = &this.M11)
          numPtr[row * 3 + column] = value;
      }
    }

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return "{ " + string.Format((IFormatProvider) currentCulture, "{{M11:{0} M12:{1} M13:{2}}} ", (object) this.M11.ToString((IFormatProvider) currentCulture), (object) this.M12.ToString((IFormatProvider) currentCulture), (object) (this.M13.ToString((IFormatProvider) currentCulture) + string.Format((IFormatProvider) currentCulture, "{{M21:{0} M22:{1} M23:{2}}} ", (object) this.M21.ToString((IFormatProvider) currentCulture), (object) this.M22.ToString((IFormatProvider) currentCulture), (object) (this.M23.ToString((IFormatProvider) currentCulture) + string.Format((IFormatProvider) currentCulture, "{{M31:{0} M32:{1} M33:{2}}} ", (object) this.M31.ToString((IFormatProvider) currentCulture), (object) this.M32.ToString((IFormatProvider) currentCulture), (object) this.M33.ToString((IFormatProvider) currentCulture)))))) + "}";
    }

    public bool Equals(Matrix3x3 other) => (double) this.M11 == (double) other.M11 && (double) this.M22 == (double) other.M22 && ((double) this.M33 == (double) other.M33 && (double) this.M12 == (double) other.M12) && ((double) this.M13 == (double) other.M13 && (double) this.M21 == (double) other.M21 && ((double) this.M23 == (double) other.M23 && (double) this.M31 == (double) other.M31)) && (double) this.M32 == (double) other.M32;

    public bool EqualsFast(ref Matrix3x3 other, float epsilon = 0.0001f)
    {
      double num1 = (double) this.M21 - (double) other.M21;
      float num2 = this.M22 - other.M22;
      float num3 = this.M23 - other.M23;
      float num4 = this.M31 - other.M31;
      float num5 = this.M32 - other.M32;
      float num6 = this.M33 - other.M33;
      float num7 = epsilon * epsilon;
      return num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 < (double) num7 & (double) num4 * (double) num4 + (double) num5 * (double) num5 + (double) num6 * (double) num6 < (double) num7;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Matrix3x3 other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode();

    public static void Transpose(ref Matrix3x3 matrix, out Matrix3x3 result)
    {
      float m11 = matrix.M11;
      float m12 = matrix.M12;
      float m13 = matrix.M13;
      float m21 = matrix.M21;
      float m22 = matrix.M22;
      float m23 = matrix.M23;
      float m31 = matrix.M31;
      float m32 = matrix.M32;
      float m33 = matrix.M33;
      result.M11 = m11;
      result.M12 = m21;
      result.M13 = m31;
      result.M21 = m12;
      result.M22 = m22;
      result.M23 = m32;
      result.M31 = m13;
      result.M32 = m23;
      result.M33 = m33;
    }

    public void Transpose()
    {
      float m12 = this.M12;
      float m13 = this.M13;
      float m21 = this.M21;
      float m23 = this.M23;
      float m31 = this.M31;
      float m32 = this.M32;
      this.M12 = m21;
      this.M13 = m31;
      this.M21 = m12;
      this.M23 = m32;
      this.M31 = m13;
      this.M32 = m23;
    }

    public float Determinant() => (float) ((double) this.M11 * ((double) this.M22 * (double) this.M33 - (double) this.M32 * (double) this.M23) - (double) this.M12 * ((double) this.M21 * (double) this.M33 - (double) this.M23 * (double) this.M31) + (double) this.M13 * ((double) this.M21 * (double) this.M32 - (double) this.M22 * (double) this.M31));

    public static void Invert(ref Matrix3x3 matrix, out Matrix3x3 result)
    {
      float num = 1f / matrix.Determinant();
      result.M11 = (float) ((double) matrix.M22 * (double) matrix.M33 - (double) matrix.M32 * (double) matrix.M23) * num;
      result.M12 = (float) ((double) matrix.M13 * (double) matrix.M32 - (double) matrix.M12 * (double) matrix.M33) * num;
      result.M13 = (float) ((double) matrix.M12 * (double) matrix.M23 - (double) matrix.M13 * (double) matrix.M22) * num;
      result.M21 = (float) ((double) matrix.M23 * (double) matrix.M31 - (double) matrix.M21 * (double) matrix.M33) * num;
      result.M22 = (float) ((double) matrix.M11 * (double) matrix.M33 - (double) matrix.M13 * (double) matrix.M31) * num;
      result.M23 = (float) ((double) matrix.M21 * (double) matrix.M13 - (double) matrix.M11 * (double) matrix.M23) * num;
      result.M31 = (float) ((double) matrix.M21 * (double) matrix.M32 - (double) matrix.M31 * (double) matrix.M22) * num;
      result.M32 = (float) ((double) matrix.M31 * (double) matrix.M12 - (double) matrix.M11 * (double) matrix.M32) * num;
      result.M33 = (float) ((double) matrix.M11 * (double) matrix.M22 - (double) matrix.M21 * (double) matrix.M12) * num;
    }

    public static void Lerp(
      ref Matrix3x3 matrix1,
      ref Matrix3x3 matrix2,
      float amount,
      out Matrix3x3 result)
    {
      result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
      result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
      result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
      result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
      result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
      result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
      result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
      result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
      result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
    }

    public static void Slerp(
      ref Matrix3x3 matrix1,
      ref Matrix3x3 matrix2,
      float amount,
      out Matrix3x3 result)
    {
      Quaternion result1;
      Quaternion.CreateFromRotationMatrix(ref matrix1, out result1);
      Quaternion result2;
      Quaternion.CreateFromRotationMatrix(ref matrix2, out result2);
      Quaternion result3;
      Quaternion.Slerp(ref result1, ref result2, amount, out result3);
      Matrix3x3.CreateFromQuaternion(ref result3, out result);
    }

    public static void SlerpScale(
      ref Matrix3x3 matrix1,
      ref Matrix3x3 matrix2,
      float amount,
      out Matrix3x3 result)
    {
      Vector3 scale1 = matrix1.Scale;
      Vector3 scale2 = matrix2.Scale;
      if ((double) scale1.LengthSquared() < 9.99999997475243E-07 || (double) scale2.LengthSquared() < 9.99999997475243E-07)
      {
        result = Matrix3x3.Zero;
      }
      else
      {
        Matrix3x3 matrix3 = Matrix3x3.Normalize(matrix1);
        Matrix3x3 matrix4 = Matrix3x3.Normalize(matrix2);
        Quaternion result1;
        Quaternion.CreateFromRotationMatrix(ref matrix3, out result1);
        Quaternion result2;
        Quaternion.CreateFromRotationMatrix(ref matrix4, out result2);
        Quaternion result3;
        Quaternion.Slerp(ref result1, ref result2, amount, out result3);
        Matrix3x3.CreateFromQuaternion(ref result3, out result);
        Vector3 scale3 = Vector3.Lerp(scale1, scale2, amount);
        Matrix3x3.Rescale(ref result, ref scale3);
      }
    }

    public static void Negate(ref Matrix3x3 matrix, out Matrix3x3 result)
    {
      result.M11 = -matrix.M11;
      result.M12 = -matrix.M12;
      result.M13 = -matrix.M13;
      result.M21 = -matrix.M21;
      result.M22 = -matrix.M22;
      result.M23 = -matrix.M23;
      result.M31 = -matrix.M31;
      result.M32 = -matrix.M32;
      result.M33 = -matrix.M33;
    }

    public static void Add(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
    {
      result.M11 = matrix1.M11 + matrix2.M11;
      result.M12 = matrix1.M12 + matrix2.M12;
      result.M13 = matrix1.M13 + matrix2.M13;
      result.M21 = matrix1.M21 + matrix2.M21;
      result.M22 = matrix1.M22 + matrix2.M22;
      result.M23 = matrix1.M23 + matrix2.M23;
      result.M31 = matrix1.M31 + matrix2.M31;
      result.M32 = matrix1.M32 + matrix2.M32;
      result.M33 = matrix1.M33 + matrix2.M33;
    }

    public static void Subtract(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
    {
      result.M11 = matrix1.M11 - matrix2.M11;
      result.M12 = matrix1.M12 - matrix2.M12;
      result.M13 = matrix1.M13 - matrix2.M13;
      result.M21 = matrix1.M21 - matrix2.M21;
      result.M22 = matrix1.M22 - matrix2.M22;
      result.M23 = matrix1.M23 - matrix2.M23;
      result.M31 = matrix1.M31 - matrix2.M31;
      result.M32 = matrix1.M32 - matrix2.M32;
      result.M33 = matrix1.M33 - matrix2.M33;
    }

    public static void Multiply(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
    {
      float num1 = (float) ((double) matrix1.M11 * (double) matrix2.M11 + (double) matrix1.M12 * (double) matrix2.M21 + (double) matrix1.M13 * (double) matrix2.M31);
      float num2 = (float) ((double) matrix1.M11 * (double) matrix2.M12 + (double) matrix1.M12 * (double) matrix2.M22 + (double) matrix1.M13 * (double) matrix2.M32);
      float num3 = (float) ((double) matrix1.M11 * (double) matrix2.M13 + (double) matrix1.M12 * (double) matrix2.M23 + (double) matrix1.M13 * (double) matrix2.M33);
      float num4 = (float) ((double) matrix1.M21 * (double) matrix2.M11 + (double) matrix1.M22 * (double) matrix2.M21 + (double) matrix1.M23 * (double) matrix2.M31);
      float num5 = (float) ((double) matrix1.M21 * (double) matrix2.M12 + (double) matrix1.M22 * (double) matrix2.M22 + (double) matrix1.M23 * (double) matrix2.M32);
      float num6 = (float) ((double) matrix1.M21 * (double) matrix2.M13 + (double) matrix1.M22 * (double) matrix2.M23 + (double) matrix1.M23 * (double) matrix2.M33);
      float num7 = (float) ((double) matrix1.M31 * (double) matrix2.M11 + (double) matrix1.M32 * (double) matrix2.M21 + (double) matrix1.M33 * (double) matrix2.M31);
      float num8 = (float) ((double) matrix1.M31 * (double) matrix2.M12 + (double) matrix1.M32 * (double) matrix2.M22 + (double) matrix1.M33 * (double) matrix2.M32);
      float num9 = (float) ((double) matrix1.M31 * (double) matrix2.M13 + (double) matrix1.M32 * (double) matrix2.M23 + (double) matrix1.M33 * (double) matrix2.M33);
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = num3;
      result.M21 = num4;
      result.M22 = num5;
      result.M23 = num6;
      result.M31 = num7;
      result.M32 = num8;
      result.M33 = num9;
    }

    public static void Multiply(ref Matrix3x3 matrix1, float scaleFactor, out Matrix3x3 result)
    {
      float num = scaleFactor;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
    }

    public static void Divide(ref Matrix3x3 matrix1, ref Matrix3x3 matrix2, out Matrix3x3 result)
    {
      result.M11 = matrix1.M11 / matrix2.M11;
      result.M12 = matrix1.M12 / matrix2.M12;
      result.M13 = matrix1.M13 / matrix2.M13;
      result.M21 = matrix1.M21 / matrix2.M21;
      result.M22 = matrix1.M22 / matrix2.M22;
      result.M23 = matrix1.M23 / matrix2.M23;
      result.M31 = matrix1.M31 / matrix2.M31;
      result.M32 = matrix1.M32 / matrix2.M32;
      result.M33 = matrix1.M33 / matrix2.M33;
    }

    public static void Divide(ref Matrix3x3 matrix1, float divider, out Matrix3x3 result)
    {
      float num = 1f / divider;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
    }

    public Matrix3x3 GetOrientation()
    {
      Matrix3x3 identity = Matrix3x3.Identity;
      identity.Forward = this.Forward;
      identity.Up = this.Up;
      identity.Right = this.Right;
      return identity;
    }

    [Conditional("DEBUG")]
    public void AssertIsValid()
    {
    }

    public bool IsValid() => (this.M11 + this.M12 + this.M13 + this.M21 + this.M22 + this.M23 + this.M31 + this.M32 + this.M33).IsValid();

    public bool IsNan() => float.IsNaN(this.M11 + this.M12 + this.M13 + this.M21 + this.M22 + this.M23 + this.M31 + this.M32 + this.M33);

    public bool IsRotation()
    {
      float num = 0.01f;
      return (double) Math.Abs(this.Right.Dot(this.Up)) <= (double) num && (double) Math.Abs(this.Right.Dot(this.Backward)) <= (double) num && ((double) Math.Abs(this.Up.Dot(this.Backward)) <= (double) num && (double) Math.Abs(this.Right.LengthSquared() - 1f) <= (double) num) && ((double) Math.Abs(this.Up.LengthSquared() - 1f) <= (double) num && (double) Math.Abs(this.Backward.LengthSquared() - 1f) <= (double) num);
    }

    public static Matrix3x3 CreateFromDir(Vector3 dir)
    {
      Vector3 vector2 = new Vector3(0.0f, 0.0f, 1f);
      float z = dir.Z;
      Vector3 vector3;
      if ((double) z > -0.99999 && (double) z < 0.99999)
      {
        vector2 = Vector3.Normalize(vector2 - dir * z);
        vector3 = Vector3.Cross(dir, vector2);
      }
      else
      {
        vector2 = new Vector3(dir.Z, 0.0f, -dir.X);
        vector3 = new Vector3(0.0f, 1f, 0.0f);
      }
      Matrix3x3 identity = Matrix3x3.Identity;
      identity.Right = vector2;
      identity.Up = vector3;
      identity.Forward = dir;
      return identity;
    }

    public static Matrix3x3 CreateWorld(ref Vector3 forward, ref Vector3 up)
    {
      Vector3 result1;
      Vector3.Normalize(ref forward, out result1);
      Vector3 vector3 = -result1;
      Vector3 result2;
      Vector3.Cross(ref up, ref vector3, out result2);
      Vector3 result3;
      Vector3.Normalize(ref result2, out result3);
      Vector3 result4;
      Vector3.Cross(ref vector3, ref result3, out result4);
      Matrix3x3 matrix3x3;
      matrix3x3.M11 = result3.X;
      matrix3x3.M12 = result3.Y;
      matrix3x3.M13 = result3.Z;
      matrix3x3.M21 = result4.X;
      matrix3x3.M22 = result4.Y;
      matrix3x3.M23 = result4.Z;
      matrix3x3.M31 = vector3.X;
      matrix3x3.M32 = vector3.Y;
      matrix3x3.M33 = vector3.Z;
      return matrix3x3;
    }

    public static Matrix3x3 CreateFromDir(Vector3 dir, Vector3 suggestedUp)
    {
      Vector3 up = Vector3.Cross(Vector3.Cross(dir, suggestedUp), dir);
      return Matrix3x3.CreateWorld(ref dir, ref up);
    }

    public static Matrix3x3 Normalize(Matrix3x3 matrix)
    {
      Matrix3x3 matrix3x3 = matrix;
      matrix3x3.Right = Vector3.Normalize(matrix3x3.Right);
      matrix3x3.Up = Vector3.Normalize(matrix3x3.Up);
      matrix3x3.Forward = Vector3.Normalize(matrix3x3.Forward);
      return matrix3x3;
    }

    public static Matrix3x3 Orthogonalize(Matrix3x3 rotationMatrix)
    {
      Matrix3x3 matrix3x3 = rotationMatrix;
      matrix3x3.Right = Vector3.Normalize(matrix3x3.Right);
      matrix3x3.Up = Vector3.Normalize(matrix3x3.Up - matrix3x3.Right * matrix3x3.Up.Dot(matrix3x3.Right));
      matrix3x3.Backward = Vector3.Normalize(matrix3x3.Backward - matrix3x3.Right * matrix3x3.Backward.Dot(matrix3x3.Right) - matrix3x3.Up * matrix3x3.Backward.Dot(matrix3x3.Up));
      return matrix3x3;
    }

    public static Matrix3x3 Round(ref Matrix3x3 matrix)
    {
      Matrix3x3 matrix3x3 = matrix;
      matrix3x3.Right = (Vector3) Vector3I.Round(matrix3x3.Right);
      matrix3x3.Up = (Vector3) Vector3I.Round(matrix3x3.Up);
      matrix3x3.Forward = (Vector3) Vector3I.Round(matrix3x3.Forward);
      return matrix3x3;
    }

    public static Matrix3x3 AlignRotationToAxes(
      ref Matrix3x3 toAlign,
      ref Matrix3x3 axisDefinitionMatrix)
    {
      Matrix3x3 identity = Matrix3x3.Identity;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      float num1 = toAlign.Right.Dot(axisDefinitionMatrix.Right);
      float num2 = toAlign.Right.Dot(axisDefinitionMatrix.Up);
      float num3 = toAlign.Right.Dot(axisDefinitionMatrix.Backward);
      if ((double) Math.Abs(num1) > (double) Math.Abs(num2))
      {
        if ((double) Math.Abs(num1) > (double) Math.Abs(num3))
        {
          identity.Right = (double) num1 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
          flag1 = true;
        }
        else
        {
          identity.Right = (double) num3 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
          flag3 = true;
        }
      }
      else if ((double) Math.Abs(num2) > (double) Math.Abs(num3))
      {
        identity.Right = (double) num2 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
        flag2 = true;
      }
      else
      {
        identity.Right = (double) num3 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
        flag3 = true;
      }
      Vector3 vector3 = toAlign.Up;
      float num4 = vector3.Dot(axisDefinitionMatrix.Right);
      vector3 = toAlign.Up;
      float num5 = vector3.Dot(axisDefinitionMatrix.Up);
      vector3 = toAlign.Up;
      float num6 = vector3.Dot(axisDefinitionMatrix.Backward);
      bool flag4;
      if (flag2 || (double) Math.Abs(num4) > (double) Math.Abs(num5) && !flag1)
      {
        if ((double) Math.Abs(num4) > (double) Math.Abs(num6) | flag3)
        {
          identity.Up = (double) num4 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
          flag1 = true;
        }
        else
        {
          identity.Up = (double) num6 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
          flag4 = true;
        }
      }
      else if ((double) Math.Abs(num5) > (double) Math.Abs(num6) | flag3)
      {
        identity.Up = (double) num5 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
        flag2 = true;
      }
      else
      {
        identity.Up = (double) num6 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
        flag4 = true;
      }
      if (!flag1)
      {
        vector3 = toAlign.Backward;
        float num7 = vector3.Dot(axisDefinitionMatrix.Right);
        identity.Backward = (double) num7 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
      }
      else if (!flag2)
      {
        vector3 = toAlign.Backward;
        float num7 = vector3.Dot(axisDefinitionMatrix.Up);
        identity.Backward = (double) num7 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
      }
      else
      {
        vector3 = toAlign.Backward;
        float num7 = vector3.Dot(axisDefinitionMatrix.Backward);
        identity.Backward = (double) num7 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
      }
      return identity;
    }

    public static bool GetEulerAnglesXYZ(ref Matrix3x3 mat, out Vector3 xyz)
    {
      float x1 = mat.GetRow(0).X;
      float y1 = mat.GetRow(0).Y;
      float z1 = mat.GetRow(0).Z;
      float x2 = mat.GetRow(1).X;
      float y2 = mat.GetRow(1).Y;
      float z2 = mat.GetRow(1).Z;
      mat.GetRow(2);
      mat.GetRow(2);
      float z3 = mat.GetRow(2).Z;
      float num = z1;
      if ((double) num < 1.0)
      {
        if ((double) num > -1.0)
        {
          xyz = new Vector3((float) Math.Atan2(-(double) z2, (double) z3), (float) Math.Asin((double) z1), (float) Math.Atan2(-(double) y1, (double) x1));
          return true;
        }
        xyz = new Vector3((float) -Math.Atan2((double) x2, (double) y2), -1.570796f, 0.0f);
        return false;
      }
      xyz = new Vector3((float) Math.Atan2((double) x2, (double) y2), -1.570796f, 0.0f);
      return false;
    }

    public bool IsMirrored() => (double) this.Determinant() < 0.0;

    public bool IsOrthogonal() => (double) Math.Abs(this.Up.LengthSquared()) - 1.0 < 9.99999974737875E-05 && (double) Math.Abs(this.Right.LengthSquared()) - 1.0 < 9.99999974737875E-05 && ((double) Math.Abs(this.Forward.LengthSquared()) - 1.0 < 9.99999974737875E-05 && (double) Math.Abs(this.Right.Dot(this.Up)) < 9.99999974737875E-05) && (double) Math.Abs(this.Right.Dot(this.Forward)) < 9.99999974737875E-05;

    private struct F9
    {
      public unsafe fixed float data[9];
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Matrix3x3.F9>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Matrix3x3.F9 value) => owner.M = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Matrix3x3.F9 value) => value = owner.M;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM11\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M11 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M11;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM12\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M12 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M12;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM13\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M13 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M13;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM21\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M21 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M21;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM22\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M22 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M22;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM23\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M23 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M23;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM31\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M31 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M31;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM32\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M32 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M32;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EM33\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in float value) => owner.M33 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out float value) => value = owner.M33;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Up;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EDown\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Down = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Down;
    }

    protected class VRageMath_Matrix3x3\u003C\u003ERight\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Right = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Right;
    }

    protected class VRageMath_Matrix3x3\u003C\u003ELeft\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Left = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Left;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Forward;
    }

    protected class VRageMath_Matrix3x3\u003C\u003EBackward\u003C\u003EAccessor : IMemberAccessor<Matrix3x3, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Matrix3x3 owner, in Vector3 value) => owner.Backward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Matrix3x3 owner, out Vector3 value) => value = owner.Backward;
    }
  }
}
