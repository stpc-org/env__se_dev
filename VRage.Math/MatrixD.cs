// Decompiled with JetBrains decompiler
// Type: VRageMath.MatrixD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  [StructLayout(LayoutKind.Explicit)]
  public struct MatrixD : IEquatable<MatrixD>
  {
    public static MatrixD Identity = new MatrixD(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
    public static MatrixD Zero = new MatrixD(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    [FieldOffset(0)]
    public double M11;
    [FieldOffset(8)]
    public double M12;
    [FieldOffset(16)]
    public double M13;
    [FieldOffset(24)]
    public double M14;
    [FieldOffset(32)]
    public double M21;
    [FieldOffset(40)]
    public double M22;
    [FieldOffset(48)]
    public double M23;
    [FieldOffset(56)]
    public double M24;
    [FieldOffset(64)]
    public double M31;
    [FieldOffset(72)]
    public double M32;
    [FieldOffset(80)]
    public double M33;
    [FieldOffset(88)]
    public double M34;
    [FieldOffset(96)]
    public double M41;
    [FieldOffset(104)]
    public double M42;
    [FieldOffset(112)]
    public double M43;
    [FieldOffset(120)]
    public double M44;

    public Vector3D Col0
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M11;
        vector3D.Y = this.M21;
        vector3D.Z = this.M31;
        return vector3D;
      }
    }

    public Vector3D Col1
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M12;
        vector3D.Y = this.M22;
        vector3D.Z = this.M32;
        return vector3D;
      }
    }

    public Vector3D Col2
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M13;
        vector3D.Y = this.M23;
        vector3D.Z = this.M33;
        return vector3D;
      }
    }

    public Vector3D Up
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M21;
        vector3D.Y = this.M22;
        vector3D.Z = this.M23;
        return vector3D;
      }
      set
      {
        this.M21 = value.X;
        this.M22 = value.Y;
        this.M23 = value.Z;
      }
    }

    public Vector3D Down
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = -this.M21;
        vector3D.Y = -this.M22;
        vector3D.Z = -this.M23;
        return vector3D;
      }
      set
      {
        this.M21 = -value.X;
        this.M22 = -value.Y;
        this.M23 = -value.Z;
      }
    }

    public Vector3D Right
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M11;
        vector3D.Y = this.M12;
        vector3D.Z = this.M13;
        return vector3D;
      }
      set
      {
        this.M11 = value.X;
        this.M12 = value.Y;
        this.M13 = value.Z;
      }
    }

    public Vector3D Left
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = -this.M11;
        vector3D.Y = -this.M12;
        vector3D.Z = -this.M13;
        return vector3D;
      }
      set
      {
        this.M11 = -value.X;
        this.M12 = -value.Y;
        this.M13 = -value.Z;
      }
    }

    public Vector3D Forward
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = -this.M31;
        vector3D.Y = -this.M32;
        vector3D.Z = -this.M33;
        return vector3D;
      }
      set
      {
        this.M31 = -value.X;
        this.M32 = -value.Y;
        this.M33 = -value.Z;
      }
    }

    public Vector3D Backward
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M31;
        vector3D.Y = this.M32;
        vector3D.Z = this.M33;
        return vector3D;
      }
      set
      {
        this.M31 = value.X;
        this.M32 = value.Y;
        this.M33 = value.Z;
      }
    }

    public Vector3D GetDirectionVector(Base6Directions.Direction direction)
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
          return Vector3D.Zero;
      }
    }

    public void SetDirectionVector(Base6Directions.Direction direction, Vector3D newValue)
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

    public Base6Directions.Direction GetClosestDirection(Vector3D referenceVector) => this.GetClosestDirection(ref referenceVector);

    public Base6Directions.Direction GetClosestDirection(ref Vector3D referenceVector)
    {
      double num1 = Vector3D.Dot(referenceVector, this.Right);
      double num2 = Vector3D.Dot(referenceVector, this.Up);
      double num3 = Vector3D.Dot(referenceVector, this.Backward);
      double num4 = Math.Abs(num1);
      double num5 = Math.Abs(num2);
      double num6 = Math.Abs(num3);
      return num4 > num5 ? (num4 > num6 ? (num1 > 0.0 ? Base6Directions.Direction.Right : Base6Directions.Direction.Left) : (num3 > 0.0 ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward)) : (num5 > num6 ? (num2 > 0.0 ? Base6Directions.Direction.Up : Base6Directions.Direction.Down) : (num3 > 0.0 ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward));
    }

    public Vector3D Scale
    {
      get
      {
        Vector3D vector3D = this.Right;
        double x = vector3D.Length();
        vector3D = this.Up;
        double y = vector3D.Length();
        vector3D = this.Forward;
        double z = vector3D.Length();
        return new Vector3D(x, y, z);
      }
    }

    public static void Rescale(ref MatrixD matrix, double scale)
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

    public static void Rescale(ref MatrixD matrix, float scale)
    {
      matrix.M11 *= (double) scale;
      matrix.M12 *= (double) scale;
      matrix.M13 *= (double) scale;
      matrix.M21 *= (double) scale;
      matrix.M22 *= (double) scale;
      matrix.M23 *= (double) scale;
      matrix.M31 *= (double) scale;
      matrix.M32 *= (double) scale;
      matrix.M33 *= (double) scale;
    }

    public static void Rescale(ref MatrixD matrix, ref Vector3D scale)
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

    public static MatrixD Rescale(MatrixD matrix, double scale)
    {
      MatrixD.Rescale(ref matrix, scale);
      return matrix;
    }

    public static MatrixD Rescale(MatrixD matrix, Vector3D scale)
    {
      MatrixD.Rescale(ref matrix, ref scale);
      return matrix;
    }

    public Vector3D Translation
    {
      get
      {
        Vector3D vector3D;
        vector3D.X = this.M41;
        vector3D.Y = this.M42;
        vector3D.Z = this.M43;
        return vector3D;
      }
      set
      {
        this.M41 = value.X;
        this.M42 = value.Y;
        this.M43 = value.Z;
      }
    }

    public Matrix3x3 Rotation => new Matrix3x3((float) this.M11, (float) this.M12, (float) this.M13, (float) this.M21, (float) this.M22, (float) this.M23, (float) this.M31, (float) this.M32, (float) this.M33);

    public MatrixD(
      double m11,
      double m12,
      double m13,
      double m14,
      double m21,
      double m22,
      double m23,
      double m24,
      double m31,
      double m32,
      double m33,
      double m34,
      double m41,
      double m42,
      double m43,
      double m44)
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M13 = m13;
      this.M14 = m14;
      this.M21 = m21;
      this.M22 = m22;
      this.M23 = m23;
      this.M24 = m24;
      this.M31 = m31;
      this.M32 = m32;
      this.M33 = m33;
      this.M34 = m34;
      this.M41 = m41;
      this.M42 = m42;
      this.M43 = m43;
      this.M44 = m44;
    }

    public MatrixD(
      double m11,
      double m12,
      double m13,
      double m21,
      double m22,
      double m23,
      double m31,
      double m32,
      double m33)
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M13 = m13;
      this.M14 = 0.0;
      this.M21 = m21;
      this.M22 = m22;
      this.M23 = m23;
      this.M24 = 0.0;
      this.M31 = m31;
      this.M32 = m32;
      this.M33 = m33;
      this.M34 = 0.0;
      this.M41 = 0.0;
      this.M42 = 0.0;
      this.M43 = 0.0;
      this.M44 = 1.0;
    }

    public MatrixD(Matrix m)
    {
      this.M11 = (double) m.M11;
      this.M12 = (double) m.M12;
      this.M13 = (double) m.M13;
      this.M14 = (double) m.M14;
      this.M21 = (double) m.M21;
      this.M22 = (double) m.M22;
      this.M23 = (double) m.M23;
      this.M24 = (double) m.M24;
      this.M31 = (double) m.M31;
      this.M32 = (double) m.M32;
      this.M33 = (double) m.M33;
      this.M34 = (double) m.M34;
      this.M41 = (double) m.M41;
      this.M42 = (double) m.M42;
      this.M43 = (double) m.M43;
      this.M44 = (double) m.M44;
    }

    public static MatrixD operator -(MatrixD matrix1)
    {
      MatrixD matrixD;
      matrixD.M11 = -matrix1.M11;
      matrixD.M12 = -matrix1.M12;
      matrixD.M13 = -matrix1.M13;
      matrixD.M14 = -matrix1.M14;
      matrixD.M21 = -matrix1.M21;
      matrixD.M22 = -matrix1.M22;
      matrixD.M23 = -matrix1.M23;
      matrixD.M24 = -matrix1.M24;
      matrixD.M31 = -matrix1.M31;
      matrixD.M32 = -matrix1.M32;
      matrixD.M33 = -matrix1.M33;
      matrixD.M34 = -matrix1.M34;
      matrixD.M41 = -matrix1.M41;
      matrixD.M42 = -matrix1.M42;
      matrixD.M43 = -matrix1.M43;
      matrixD.M44 = -matrix1.M44;
      return matrixD;
    }

    public static bool operator ==(MatrixD matrix1, MatrixD matrix2) => matrix1.M11 == matrix2.M11 && matrix1.M22 == matrix2.M22 && (matrix1.M33 == matrix2.M33 && matrix1.M44 == matrix2.M44) && (matrix1.M12 == matrix2.M12 && matrix1.M13 == matrix2.M13 && (matrix1.M14 == matrix2.M14 && matrix1.M21 == matrix2.M21)) && (matrix1.M23 == matrix2.M23 && matrix1.M24 == matrix2.M24 && (matrix1.M31 == matrix2.M31 && matrix1.M32 == matrix2.M32) && (matrix1.M34 == matrix2.M34 && matrix1.M41 == matrix2.M41 && matrix1.M42 == matrix2.M42)) && matrix1.M43 == matrix2.M43;

    public static bool operator !=(MatrixD matrix1, MatrixD matrix2) => matrix1.M11 != matrix2.M11 || matrix1.M12 != matrix2.M12 || (matrix1.M13 != matrix2.M13 || matrix1.M14 != matrix2.M14) || (matrix1.M21 != matrix2.M21 || matrix1.M22 != matrix2.M22 || (matrix1.M23 != matrix2.M23 || matrix1.M24 != matrix2.M24)) || (matrix1.M31 != matrix2.M31 || matrix1.M32 != matrix2.M32 || (matrix1.M33 != matrix2.M33 || matrix1.M34 != matrix2.M34) || (matrix1.M41 != matrix2.M41 || matrix1.M42 != matrix2.M42 || matrix1.M43 != matrix2.M43)) || matrix1.M44 != matrix2.M44;

    public static MatrixD operator +(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 + matrix2.M11;
      matrixD.M12 = matrix1.M12 + matrix2.M12;
      matrixD.M13 = matrix1.M13 + matrix2.M13;
      matrixD.M14 = matrix1.M14 + matrix2.M14;
      matrixD.M21 = matrix1.M21 + matrix2.M21;
      matrixD.M22 = matrix1.M22 + matrix2.M22;
      matrixD.M23 = matrix1.M23 + matrix2.M23;
      matrixD.M24 = matrix1.M24 + matrix2.M24;
      matrixD.M31 = matrix1.M31 + matrix2.M31;
      matrixD.M32 = matrix1.M32 + matrix2.M32;
      matrixD.M33 = matrix1.M33 + matrix2.M33;
      matrixD.M34 = matrix1.M34 + matrix2.M34;
      matrixD.M41 = matrix1.M41 + matrix2.M41;
      matrixD.M42 = matrix1.M42 + matrix2.M42;
      matrixD.M43 = matrix1.M43 + matrix2.M43;
      matrixD.M44 = matrix1.M44 + matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator -(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 - matrix2.M11;
      matrixD.M12 = matrix1.M12 - matrix2.M12;
      matrixD.M13 = matrix1.M13 - matrix2.M13;
      matrixD.M14 = matrix1.M14 - matrix2.M14;
      matrixD.M21 = matrix1.M21 - matrix2.M21;
      matrixD.M22 = matrix1.M22 - matrix2.M22;
      matrixD.M23 = matrix1.M23 - matrix2.M23;
      matrixD.M24 = matrix1.M24 - matrix2.M24;
      matrixD.M31 = matrix1.M31 - matrix2.M31;
      matrixD.M32 = matrix1.M32 - matrix2.M32;
      matrixD.M33 = matrix1.M33 - matrix2.M33;
      matrixD.M34 = matrix1.M34 - matrix2.M34;
      matrixD.M41 = matrix1.M41 - matrix2.M41;
      matrixD.M42 = matrix1.M42 - matrix2.M42;
      matrixD.M43 = matrix1.M43 - matrix2.M43;
      matrixD.M44 = matrix1.M44 - matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator *(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
      matrixD.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
      matrixD.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
      matrixD.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
      matrixD.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
      matrixD.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
      matrixD.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
      matrixD.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
      matrixD.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
      matrixD.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
      matrixD.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
      matrixD.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
      matrixD.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
      matrixD.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
      matrixD.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
      matrixD.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator *(MatrixD matrix1, Matrix matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * (double) matrix2.M11 + matrix1.M12 * (double) matrix2.M21 + matrix1.M13 * (double) matrix2.M31 + matrix1.M14 * (double) matrix2.M41;
      matrixD.M12 = matrix1.M11 * (double) matrix2.M12 + matrix1.M12 * (double) matrix2.M22 + matrix1.M13 * (double) matrix2.M32 + matrix1.M14 * (double) matrix2.M42;
      matrixD.M13 = matrix1.M11 * (double) matrix2.M13 + matrix1.M12 * (double) matrix2.M23 + matrix1.M13 * (double) matrix2.M33 + matrix1.M14 * (double) matrix2.M43;
      matrixD.M14 = matrix1.M11 * (double) matrix2.M14 + matrix1.M12 * (double) matrix2.M24 + matrix1.M13 * (double) matrix2.M34 + matrix1.M14 * (double) matrix2.M44;
      matrixD.M21 = matrix1.M21 * (double) matrix2.M11 + matrix1.M22 * (double) matrix2.M21 + matrix1.M23 * (double) matrix2.M31 + matrix1.M24 * (double) matrix2.M41;
      matrixD.M22 = matrix1.M21 * (double) matrix2.M12 + matrix1.M22 * (double) matrix2.M22 + matrix1.M23 * (double) matrix2.M32 + matrix1.M24 * (double) matrix2.M42;
      matrixD.M23 = matrix1.M21 * (double) matrix2.M13 + matrix1.M22 * (double) matrix2.M23 + matrix1.M23 * (double) matrix2.M33 + matrix1.M24 * (double) matrix2.M43;
      matrixD.M24 = matrix1.M21 * (double) matrix2.M14 + matrix1.M22 * (double) matrix2.M24 + matrix1.M23 * (double) matrix2.M34 + matrix1.M24 * (double) matrix2.M44;
      matrixD.M31 = matrix1.M31 * (double) matrix2.M11 + matrix1.M32 * (double) matrix2.M21 + matrix1.M33 * (double) matrix2.M31 + matrix1.M34 * (double) matrix2.M41;
      matrixD.M32 = matrix1.M31 * (double) matrix2.M12 + matrix1.M32 * (double) matrix2.M22 + matrix1.M33 * (double) matrix2.M32 + matrix1.M34 * (double) matrix2.M42;
      matrixD.M33 = matrix1.M31 * (double) matrix2.M13 + matrix1.M32 * (double) matrix2.M23 + matrix1.M33 * (double) matrix2.M33 + matrix1.M34 * (double) matrix2.M43;
      matrixD.M34 = matrix1.M31 * (double) matrix2.M14 + matrix1.M32 * (double) matrix2.M24 + matrix1.M33 * (double) matrix2.M34 + matrix1.M34 * (double) matrix2.M44;
      matrixD.M41 = matrix1.M41 * (double) matrix2.M11 + matrix1.M42 * (double) matrix2.M21 + matrix1.M43 * (double) matrix2.M31 + matrix1.M44 * (double) matrix2.M41;
      matrixD.M42 = matrix1.M41 * (double) matrix2.M12 + matrix1.M42 * (double) matrix2.M22 + matrix1.M43 * (double) matrix2.M32 + matrix1.M44 * (double) matrix2.M42;
      matrixD.M43 = matrix1.M41 * (double) matrix2.M13 + matrix1.M42 * (double) matrix2.M23 + matrix1.M43 * (double) matrix2.M33 + matrix1.M44 * (double) matrix2.M43;
      matrixD.M44 = matrix1.M41 * (double) matrix2.M14 + matrix1.M42 * (double) matrix2.M24 + matrix1.M43 * (double) matrix2.M34 + matrix1.M44 * (double) matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator *(Matrix matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = (double) matrix1.M11 * matrix2.M11 + (double) matrix1.M12 * matrix2.M21 + (double) matrix1.M13 * matrix2.M31 + (double) matrix1.M14 * matrix2.M41;
      matrixD.M12 = (double) matrix1.M11 * matrix2.M12 + (double) matrix1.M12 * matrix2.M22 + (double) matrix1.M13 * matrix2.M32 + (double) matrix1.M14 * matrix2.M42;
      matrixD.M13 = (double) matrix1.M11 * matrix2.M13 + (double) matrix1.M12 * matrix2.M23 + (double) matrix1.M13 * matrix2.M33 + (double) matrix1.M14 * matrix2.M43;
      matrixD.M14 = (double) matrix1.M11 * matrix2.M14 + (double) matrix1.M12 * matrix2.M24 + (double) matrix1.M13 * matrix2.M34 + (double) matrix1.M14 * matrix2.M44;
      matrixD.M21 = (double) matrix1.M21 * matrix2.M11 + (double) matrix1.M22 * matrix2.M21 + (double) matrix1.M23 * matrix2.M31 + (double) matrix1.M24 * matrix2.M41;
      matrixD.M22 = (double) matrix1.M21 * matrix2.M12 + (double) matrix1.M22 * matrix2.M22 + (double) matrix1.M23 * matrix2.M32 + (double) matrix1.M24 * matrix2.M42;
      matrixD.M23 = (double) matrix1.M21 * matrix2.M13 + (double) matrix1.M22 * matrix2.M23 + (double) matrix1.M23 * matrix2.M33 + (double) matrix1.M24 * matrix2.M43;
      matrixD.M24 = (double) matrix1.M21 * matrix2.M14 + (double) matrix1.M22 * matrix2.M24 + (double) matrix1.M23 * matrix2.M34 + (double) matrix1.M24 * matrix2.M44;
      matrixD.M31 = (double) matrix1.M31 * matrix2.M11 + (double) matrix1.M32 * matrix2.M21 + (double) matrix1.M33 * matrix2.M31 + (double) matrix1.M34 * matrix2.M41;
      matrixD.M32 = (double) matrix1.M31 * matrix2.M12 + (double) matrix1.M32 * matrix2.M22 + (double) matrix1.M33 * matrix2.M32 + (double) matrix1.M34 * matrix2.M42;
      matrixD.M33 = (double) matrix1.M31 * matrix2.M13 + (double) matrix1.M32 * matrix2.M23 + (double) matrix1.M33 * matrix2.M33 + (double) matrix1.M34 * matrix2.M43;
      matrixD.M34 = (double) matrix1.M31 * matrix2.M14 + (double) matrix1.M32 * matrix2.M24 + (double) matrix1.M33 * matrix2.M34 + (double) matrix1.M34 * matrix2.M44;
      matrixD.M41 = (double) matrix1.M41 * matrix2.M11 + (double) matrix1.M42 * matrix2.M21 + (double) matrix1.M43 * matrix2.M31 + (double) matrix1.M44 * matrix2.M41;
      matrixD.M42 = (double) matrix1.M41 * matrix2.M12 + (double) matrix1.M42 * matrix2.M22 + (double) matrix1.M43 * matrix2.M32 + (double) matrix1.M44 * matrix2.M42;
      matrixD.M43 = (double) matrix1.M41 * matrix2.M13 + (double) matrix1.M42 * matrix2.M23 + (double) matrix1.M43 * matrix2.M33 + (double) matrix1.M44 * matrix2.M43;
      matrixD.M44 = (double) matrix1.M41 * matrix2.M14 + (double) matrix1.M42 * matrix2.M24 + (double) matrix1.M43 * matrix2.M34 + (double) matrix1.M44 * matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator *(MatrixD matrix, double scaleFactor)
    {
      double num = scaleFactor;
      MatrixD matrixD;
      matrixD.M11 = matrix.M11 * num;
      matrixD.M12 = matrix.M12 * num;
      matrixD.M13 = matrix.M13 * num;
      matrixD.M14 = matrix.M14 * num;
      matrixD.M21 = matrix.M21 * num;
      matrixD.M22 = matrix.M22 * num;
      matrixD.M23 = matrix.M23 * num;
      matrixD.M24 = matrix.M24 * num;
      matrixD.M31 = matrix.M31 * num;
      matrixD.M32 = matrix.M32 * num;
      matrixD.M33 = matrix.M33 * num;
      matrixD.M34 = matrix.M34 * num;
      matrixD.M41 = matrix.M41 * num;
      matrixD.M42 = matrix.M42 * num;
      matrixD.M43 = matrix.M43 * num;
      matrixD.M44 = matrix.M44 * num;
      return matrixD;
    }

    public static MatrixD operator *(double scaleFactor, MatrixD matrix)
    {
      double num = scaleFactor;
      MatrixD matrixD;
      matrixD.M11 = matrix.M11 * num;
      matrixD.M12 = matrix.M12 * num;
      matrixD.M13 = matrix.M13 * num;
      matrixD.M14 = matrix.M14 * num;
      matrixD.M21 = matrix.M21 * num;
      matrixD.M22 = matrix.M22 * num;
      matrixD.M23 = matrix.M23 * num;
      matrixD.M24 = matrix.M24 * num;
      matrixD.M31 = matrix.M31 * num;
      matrixD.M32 = matrix.M32 * num;
      matrixD.M33 = matrix.M33 * num;
      matrixD.M34 = matrix.M34 * num;
      matrixD.M41 = matrix.M41 * num;
      matrixD.M42 = matrix.M42 * num;
      matrixD.M43 = matrix.M43 * num;
      matrixD.M44 = matrix.M44 * num;
      return matrixD;
    }

    public static MatrixD operator /(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 / matrix2.M11;
      matrixD.M12 = matrix1.M12 / matrix2.M12;
      matrixD.M13 = matrix1.M13 / matrix2.M13;
      matrixD.M14 = matrix1.M14 / matrix2.M14;
      matrixD.M21 = matrix1.M21 / matrix2.M21;
      matrixD.M22 = matrix1.M22 / matrix2.M22;
      matrixD.M23 = matrix1.M23 / matrix2.M23;
      matrixD.M24 = matrix1.M24 / matrix2.M24;
      matrixD.M31 = matrix1.M31 / matrix2.M31;
      matrixD.M32 = matrix1.M32 / matrix2.M32;
      matrixD.M33 = matrix1.M33 / matrix2.M33;
      matrixD.M34 = matrix1.M34 / matrix2.M34;
      matrixD.M41 = matrix1.M41 / matrix2.M41;
      matrixD.M42 = matrix1.M42 / matrix2.M42;
      matrixD.M43 = matrix1.M43 / matrix2.M43;
      matrixD.M44 = matrix1.M44 / matrix2.M44;
      return matrixD;
    }

    public static MatrixD operator /(MatrixD matrix1, double divider)
    {
      double num = 1.0 / divider;
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * num;
      matrixD.M12 = matrix1.M12 * num;
      matrixD.M13 = matrix1.M13 * num;
      matrixD.M14 = matrix1.M14 * num;
      matrixD.M21 = matrix1.M21 * num;
      matrixD.M22 = matrix1.M22 * num;
      matrixD.M23 = matrix1.M23 * num;
      matrixD.M24 = matrix1.M24 * num;
      matrixD.M31 = matrix1.M31 * num;
      matrixD.M32 = matrix1.M32 * num;
      matrixD.M33 = matrix1.M33 * num;
      matrixD.M34 = matrix1.M34 * num;
      matrixD.M41 = matrix1.M41 * num;
      matrixD.M42 = matrix1.M42 * num;
      matrixD.M43 = matrix1.M43 * num;
      matrixD.M44 = matrix1.M44 * num;
      return matrixD;
    }

    public static MatrixD CreateBillboard(
      Vector3D objectPosition,
      Vector3D cameraPosition,
      Vector3D cameraUpVector,
      Vector3D? cameraForwardVector)
    {
      Vector3D result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      double d = result1.LengthSquared();
      if (d < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3D.Forward;
      else
        Vector3D.Multiply(ref result1, 1.0 / Math.Sqrt(d), out result1);
      Vector3D result2;
      Vector3D.Cross(ref cameraUpVector, ref result1, out result2);
      result2.Normalize();
      Vector3D result3;
      Vector3D.Cross(ref result1, ref result2, out result3);
      MatrixD matrixD;
      matrixD.M11 = result2.X;
      matrixD.M12 = result2.Y;
      matrixD.M13 = result2.Z;
      matrixD.M14 = 0.0;
      matrixD.M21 = result3.X;
      matrixD.M22 = result3.Y;
      matrixD.M23 = result3.Z;
      matrixD.M24 = 0.0;
      matrixD.M31 = result1.X;
      matrixD.M32 = result1.Y;
      matrixD.M33 = result1.Z;
      matrixD.M34 = 0.0;
      matrixD.M41 = objectPosition.X;
      matrixD.M42 = objectPosition.Y;
      matrixD.M43 = objectPosition.Z;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateBillboard(
      ref Vector3D objectPosition,
      ref Vector3D cameraPosition,
      ref Vector3D cameraUpVector,
      Vector3D? cameraForwardVector,
      out MatrixD result)
    {
      Vector3D result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      double d = result1.LengthSquared();
      if (d < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3D.Forward;
      else
        Vector3D.Multiply(ref result1, 1.0 / Math.Sqrt(d), out result1);
      Vector3D result2;
      Vector3D.Cross(ref cameraUpVector, ref result1, out result2);
      result2.Normalize();
      Vector3D result3;
      Vector3D.Cross(ref result1, ref result2, out result3);
      result.M11 = result2.X;
      result.M12 = result2.Y;
      result.M13 = result2.Z;
      result.M14 = 0.0;
      result.M21 = result3.X;
      result.M22 = result3.Y;
      result.M23 = result3.Z;
      result.M24 = 0.0;
      result.M31 = result1.X;
      result.M32 = result1.Y;
      result.M33 = result1.Z;
      result.M34 = 0.0;
      result.M41 = objectPosition.X;
      result.M42 = objectPosition.Y;
      result.M43 = objectPosition.Z;
      result.M44 = 1.0;
    }

    public static MatrixD CreateConstrainedBillboard(
      Vector3D objectPosition,
      Vector3D cameraPosition,
      Vector3D rotateAxis,
      Vector3D? cameraForwardVector,
      Vector3D? objectForwardVector)
    {
      Vector3D result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      double d = result1.LengthSquared();
      if (d < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3D.Forward;
      else
        Vector3D.Multiply(ref result1, 1.0 / Math.Sqrt(d), out result1);
      Vector3D vector2 = rotateAxis;
      double result2;
      Vector3D.Dot(ref rotateAxis, ref result1, out result2);
      Vector3D result3;
      Vector3D result4;
      if (Math.Abs(result2) > 0.998254656791687)
      {
        if (objectForwardVector.HasValue)
        {
          result3 = objectForwardVector.Value;
          Vector3D.Dot(ref rotateAxis, ref result3, out result2);
          if (Math.Abs(result2) > 0.998254656791687)
            result3 = Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687 ? Vector3D.Right : Vector3D.Forward;
        }
        else
          result3 = Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687 ? Vector3D.Right : Vector3D.Forward;
        Vector3D.Cross(ref rotateAxis, ref result3, out result4);
        result4.Normalize();
        Vector3D.Cross(ref result4, ref rotateAxis, out result3);
        result3.Normalize();
      }
      else
      {
        Vector3D.Cross(ref rotateAxis, ref result1, out result4);
        result4.Normalize();
        Vector3D.Cross(ref result4, ref vector2, out result3);
        result3.Normalize();
      }
      MatrixD matrixD;
      matrixD.M11 = result4.X;
      matrixD.M12 = result4.Y;
      matrixD.M13 = result4.Z;
      matrixD.M14 = 0.0;
      matrixD.M21 = vector2.X;
      matrixD.M22 = vector2.Y;
      matrixD.M23 = vector2.Z;
      matrixD.M24 = 0.0;
      matrixD.M31 = result3.X;
      matrixD.M32 = result3.Y;
      matrixD.M33 = result3.Z;
      matrixD.M34 = 0.0;
      matrixD.M41 = objectPosition.X;
      matrixD.M42 = objectPosition.Y;
      matrixD.M43 = objectPosition.Z;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateConstrainedBillboard(
      ref Vector3D objectPosition,
      ref Vector3D cameraPosition,
      ref Vector3D rotateAxis,
      Vector3D? cameraForwardVector,
      Vector3D? objectForwardVector,
      out MatrixD result)
    {
      Vector3D result1;
      result1.X = objectPosition.X - cameraPosition.X;
      result1.Y = objectPosition.Y - cameraPosition.Y;
      result1.Z = objectPosition.Z - cameraPosition.Z;
      double d = result1.LengthSquared();
      if (d < 9.99999974737875E-05)
        result1 = cameraForwardVector.HasValue ? -cameraForwardVector.Value : Vector3D.Forward;
      else
        Vector3D.Multiply(ref result1, 1.0 / Math.Sqrt(d), out result1);
      Vector3D vector2 = rotateAxis;
      double result2;
      Vector3D.Dot(ref rotateAxis, ref result1, out result2);
      Vector3D result3;
      Vector3D result4;
      if (Math.Abs(result2) > 0.998254656791687)
      {
        if (objectForwardVector.HasValue)
        {
          result3 = objectForwardVector.Value;
          Vector3D.Dot(ref rotateAxis, ref result3, out result2);
          if (Math.Abs(result2) > 0.998254656791687)
            result3 = Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687 ? Vector3D.Right : Vector3D.Forward;
        }
        else
          result3 = Math.Abs(rotateAxis.X * Vector3D.Forward.X + rotateAxis.Y * Vector3D.Forward.Y + rotateAxis.Z * Vector3D.Forward.Z) > 0.998254656791687 ? Vector3D.Right : Vector3D.Forward;
        Vector3D.Cross(ref rotateAxis, ref result3, out result4);
        result4.Normalize();
        Vector3D.Cross(ref result4, ref rotateAxis, out result3);
        result3.Normalize();
      }
      else
      {
        Vector3D.Cross(ref rotateAxis, ref result1, out result4);
        result4.Normalize();
        Vector3D.Cross(ref result4, ref vector2, out result3);
        result3.Normalize();
      }
      result.M11 = result4.X;
      result.M12 = result4.Y;
      result.M13 = result4.Z;
      result.M14 = 0.0;
      result.M21 = vector2.X;
      result.M22 = vector2.Y;
      result.M23 = vector2.Z;
      result.M24 = 0.0;
      result.M31 = result3.X;
      result.M32 = result3.Y;
      result.M33 = result3.Z;
      result.M34 = 0.0;
      result.M41 = objectPosition.X;
      result.M42 = objectPosition.Y;
      result.M43 = objectPosition.Z;
      result.M44 = 1.0;
    }

    public static MatrixD CreateTranslation(Vector3D position)
    {
      MatrixD matrixD;
      matrixD.M11 = 1.0;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = 1.0;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = 1.0;
      matrixD.M34 = 0.0;
      matrixD.M41 = position.X;
      matrixD.M42 = position.Y;
      matrixD.M43 = position.Z;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static MatrixD CreateTranslation(Vector3 position)
    {
      MatrixD matrixD;
      matrixD.M11 = 1.0;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = 1.0;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = 1.0;
      matrixD.M34 = 0.0;
      matrixD.M41 = (double) position.X;
      matrixD.M42 = (double) position.Y;
      matrixD.M43 = (double) position.Z;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateTranslation(ref Vector3D position, out MatrixD result)
    {
      result.M11 = 1.0;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = 1.0;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = 1.0;
      result.M34 = 0.0;
      result.M41 = position.X;
      result.M42 = position.Y;
      result.M43 = position.Z;
      result.M44 = 1.0;
    }

    public static MatrixD CreateTranslation(
      double xPosition,
      double yPosition,
      double zPosition)
    {
      MatrixD matrixD;
      matrixD.M11 = 1.0;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = 1.0;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = 1.0;
      matrixD.M34 = 0.0;
      matrixD.M41 = xPosition;
      matrixD.M42 = yPosition;
      matrixD.M43 = zPosition;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateTranslation(
      double xPosition,
      double yPosition,
      double zPosition,
      out MatrixD result)
    {
      result.M11 = 1.0;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = 1.0;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = 1.0;
      result.M34 = 0.0;
      result.M41 = xPosition;
      result.M42 = yPosition;
      result.M43 = zPosition;
      result.M44 = 1.0;
    }

    public static MatrixD CreateScale(double xScale, double yScale, double zScale)
    {
      double num1 = xScale;
      double num2 = yScale;
      double num3 = zScale;
      MatrixD matrixD;
      matrixD.M11 = num1;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = num2;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = num3;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateScale(
      double xScale,
      double yScale,
      double zScale,
      out MatrixD result)
    {
      double num1 = xScale;
      double num2 = yScale;
      double num3 = zScale;
      result.M11 = num1;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = num2;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = num3;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateScale(Vector3D scales)
    {
      double x = scales.X;
      double y = scales.Y;
      double z = scales.Z;
      MatrixD matrixD;
      matrixD.M11 = x;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = y;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = z;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateScale(ref Vector3D scales, out MatrixD result)
    {
      double x = scales.X;
      double y = scales.Y;
      double z = scales.Z;
      result.M11 = x;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = y;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = z;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateScale(double scale)
    {
      double num = scale;
      MatrixD matrixD;
      matrixD.M11 = num;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = num;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = num;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateScale(double scale, out MatrixD result)
    {
      double num = scale;
      result.M11 = num;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = num;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = num;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateRotationX(double radians)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      MatrixD matrixD;
      matrixD.M11 = 1.0;
      matrixD.M12 = 0.0;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = num1;
      matrixD.M23 = num2;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = -num2;
      matrixD.M33 = num1;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateRotationX(double radians, out MatrixD result)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      result.M11 = 1.0;
      result.M12 = 0.0;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = num1;
      result.M23 = num2;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = -num2;
      result.M33 = num1;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateRotationY(double radians)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      MatrixD matrixD;
      matrixD.M11 = num1;
      matrixD.M12 = 0.0;
      matrixD.M13 = -num2;
      matrixD.M14 = 0.0;
      matrixD.M21 = 0.0;
      matrixD.M22 = 1.0;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = num2;
      matrixD.M32 = 0.0;
      matrixD.M33 = num1;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateRotationY(double radians, out MatrixD result)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      result.M11 = num1;
      result.M12 = 0.0;
      result.M13 = -num2;
      result.M14 = 0.0;
      result.M21 = 0.0;
      result.M22 = 1.0;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = num2;
      result.M32 = 0.0;
      result.M33 = num1;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateRotationZ(double radians)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      MatrixD matrixD;
      matrixD.M11 = num1;
      matrixD.M12 = num2;
      matrixD.M13 = 0.0;
      matrixD.M14 = 0.0;
      matrixD.M21 = -num2;
      matrixD.M22 = num1;
      matrixD.M23 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M31 = 0.0;
      matrixD.M32 = 0.0;
      matrixD.M33 = 1.0;
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateRotationZ(double radians, out MatrixD result)
    {
      double num1 = Math.Cos(radians);
      double num2 = Math.Sin(radians);
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = 0.0;
      result.M14 = 0.0;
      result.M21 = -num2;
      result.M22 = num1;
      result.M23 = 0.0;
      result.M24 = 0.0;
      result.M31 = 0.0;
      result.M32 = 0.0;
      result.M33 = 1.0;
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateFromAxisAngle(Vector3D axis, double angle)
    {
      double x = axis.X;
      double y = axis.Y;
      double z = axis.Z;
      double num1 = Math.Sin(angle);
      double num2 = Math.Cos(angle);
      double num3 = x * x;
      double num4 = y * y;
      double num5 = z * z;
      double num6 = x * y;
      double num7 = x * z;
      double num8 = y * z;
      MatrixD matrixD;
      matrixD.M11 = num3 + num2 * (1.0 - num3);
      matrixD.M12 = num6 - num2 * num6 + num1 * z;
      matrixD.M13 = num7 - num2 * num7 - num1 * y;
      matrixD.M14 = 0.0;
      matrixD.M21 = num6 - num2 * num6 - num1 * z;
      matrixD.M22 = num4 + num2 * (1.0 - num4);
      matrixD.M23 = num8 - num2 * num8 + num1 * x;
      matrixD.M24 = 0.0;
      matrixD.M31 = num7 - num2 * num7 + num1 * y;
      matrixD.M32 = num8 - num2 * num8 - num1 * x;
      matrixD.M33 = num5 + num2 * (1.0 - num5);
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateFromAxisAngle(ref Vector3D axis, double angle, out MatrixD result)
    {
      double x = axis.X;
      double y = axis.Y;
      double z = axis.Z;
      double num1 = Math.Sin(angle);
      double num2 = Math.Cos(angle);
      double num3 = x * x;
      double num4 = y * y;
      double num5 = z * z;
      double num6 = x * y;
      double num7 = x * z;
      double num8 = y * z;
      result.M11 = num3 + num2 * (1.0 - num3);
      result.M12 = num6 - num2 * num6 + num1 * z;
      result.M13 = num7 - num2 * num7 - num1 * y;
      result.M14 = 0.0;
      result.M21 = num6 - num2 * num6 - num1 * z;
      result.M22 = num4 + num2 * (1.0 - num4);
      result.M23 = num8 - num2 * num8 + num1 * x;
      result.M24 = 0.0;
      result.M31 = num7 - num2 * num7 + num1 * y;
      result.M32 = num8 - num2 * num8 - num1 * x;
      result.M33 = num5 + num2 * (1.0 - num5);
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreatePerspectiveFieldOfView(
      double fieldOfView,
      double aspectRatio,
      double nearPlaneDistance,
      double farPlaneDistance)
    {
      if (fieldOfView <= 0.0 || fieldOfView >= 3.14159274101257)
        throw new ArgumentOutOfRangeException(nameof (fieldOfView), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
        {
          (object) nameof (fieldOfView)
        }));
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      double num1 = 1.0 / Math.Tan(fieldOfView * 0.5);
      double num2 = num1 / aspectRatio;
      MatrixD matrixD;
      matrixD.M11 = num2;
      matrixD.M12 = matrixD.M13 = matrixD.M14 = 0.0;
      matrixD.M22 = num1;
      matrixD.M21 = matrixD.M23 = matrixD.M24 = 0.0;
      matrixD.M31 = matrixD.M32 = 0.0;
      matrixD.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrixD.M34 = -1.0;
      matrixD.M41 = matrixD.M42 = matrixD.M44 = 0.0;
      matrixD.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      return matrixD;
    }

    public static void CreatePerspectiveFieldOfView(
      double fieldOfView,
      double aspectRatio,
      double nearPlaneDistance,
      double farPlaneDistance,
      out MatrixD result)
    {
      if (fieldOfView <= 0.0 || fieldOfView >= 3.14159274101257)
        throw new ArgumentOutOfRangeException(nameof (fieldOfView), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "OutRangeFieldOfView", new object[1]
        {
          (object) nameof (fieldOfView)
        }));
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      double num1 = 1.0 / Math.Tan(fieldOfView * 0.5);
      double num2 = num1 / aspectRatio;
      result.M11 = num2;
      result.M12 = result.M13 = result.M14 = 0.0;
      result.M22 = num1;
      result.M21 = result.M23 = result.M24 = 0.0;
      result.M31 = result.M32 = 0.0;
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M34 = -1.0;
      result.M41 = result.M42 = result.M44 = 0.0;
      result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
    }

    public static MatrixD CreatePerspective(
      double width,
      double height,
      double nearPlaneDistance,
      double farPlaneDistance)
    {
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      MatrixD matrixD;
      matrixD.M11 = 2.0 * nearPlaneDistance / width;
      matrixD.M12 = matrixD.M13 = matrixD.M14 = 0.0;
      matrixD.M22 = 2.0 * nearPlaneDistance / height;
      matrixD.M21 = matrixD.M23 = matrixD.M24 = 0.0;
      matrixD.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrixD.M31 = matrixD.M32 = 0.0;
      matrixD.M34 = -1.0;
      matrixD.M41 = matrixD.M42 = matrixD.M44 = 0.0;
      matrixD.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      return matrixD;
    }

    public static void CreatePerspective(
      double width,
      double height,
      double nearPlaneDistance,
      double farPlaneDistance,
      out MatrixD result)
    {
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      result.M11 = 2.0 * nearPlaneDistance / width;
      result.M12 = result.M13 = result.M14 = 0.0;
      result.M22 = 2.0 * nearPlaneDistance / height;
      result.M21 = result.M23 = result.M24 = 0.0;
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M31 = result.M32 = 0.0;
      result.M34 = -1.0;
      result.M41 = result.M42 = result.M44 = 0.0;
      result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
    }

    public static MatrixD CreatePerspectiveOffCenter(
      double left,
      double right,
      double bottom,
      double top,
      double nearPlaneDistance,
      double farPlaneDistance)
    {
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      MatrixD matrixD;
      matrixD.M11 = 2.0 * nearPlaneDistance / (right - left);
      matrixD.M12 = matrixD.M13 = matrixD.M14 = 0.0;
      matrixD.M22 = 2.0 * nearPlaneDistance / (top - bottom);
      matrixD.M21 = matrixD.M23 = matrixD.M24 = 0.0;
      matrixD.M31 = (left + right) / (right - left);
      matrixD.M32 = (top + bottom) / (top - bottom);
      matrixD.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrixD.M34 = -1.0;
      matrixD.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      matrixD.M41 = matrixD.M42 = matrixD.M44 = 0.0;
      return matrixD;
    }

    public static void CreatePerspectiveOffCenter(
      double left,
      double right,
      double bottom,
      double top,
      double nearPlaneDistance,
      double farPlaneDistance,
      out MatrixD result)
    {
      if (nearPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (nearPlaneDistance)
        }));
      if (farPlaneDistance <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (farPlaneDistance), string.Format((IFormatProvider) CultureInfo.CurrentCulture, "NegativePlaneDistance", new object[1]
        {
          (object) nameof (farPlaneDistance)
        }));
      if (nearPlaneDistance >= farPlaneDistance)
        throw new ArgumentOutOfRangeException(nameof (nearPlaneDistance), "OppositePlanes");
      result.M11 = 2.0 * nearPlaneDistance / (right - left);
      result.M12 = result.M13 = result.M14 = 0.0;
      result.M22 = 2.0 * nearPlaneDistance / (top - bottom);
      result.M21 = result.M23 = result.M24 = 0.0;
      result.M31 = (left + right) / (right - left);
      result.M32 = (top + bottom) / (top - bottom);
      result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M34 = -1.0;
      result.M43 = nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
      result.M41 = result.M42 = result.M44 = 0.0;
    }

    public static MatrixD CreateOrthographic(
      double width,
      double height,
      double zNearPlane,
      double zFarPlane)
    {
      MatrixD matrixD;
      matrixD.M11 = 2.0 / width;
      matrixD.M12 = matrixD.M13 = matrixD.M14 = 0.0;
      matrixD.M22 = 2.0 / height;
      matrixD.M21 = matrixD.M23 = matrixD.M24 = 0.0;
      matrixD.M33 = 1.0 / (zNearPlane - zFarPlane);
      matrixD.M31 = matrixD.M32 = matrixD.M34 = 0.0;
      matrixD.M41 = matrixD.M42 = 0.0;
      matrixD.M43 = zNearPlane / (zNearPlane - zFarPlane);
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateOrthographic(
      double width,
      double height,
      double zNearPlane,
      double zFarPlane,
      out MatrixD result)
    {
      result.M11 = 2.0 / width;
      result.M12 = result.M13 = result.M14 = 0.0;
      result.M22 = 2.0 / height;
      result.M21 = result.M23 = result.M24 = 0.0;
      result.M33 = 1.0 / (zNearPlane - zFarPlane);
      result.M31 = result.M32 = result.M34 = 0.0;
      result.M41 = result.M42 = 0.0;
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1.0;
    }

    public static MatrixD CreateOrthographicOffCenter(
      double left,
      double right,
      double bottom,
      double top,
      double zNearPlane,
      double zFarPlane)
    {
      MatrixD matrixD;
      matrixD.M11 = 2.0 / (right - left);
      matrixD.M12 = matrixD.M13 = matrixD.M14 = 0.0;
      matrixD.M22 = 2.0 / (top - bottom);
      matrixD.M21 = matrixD.M23 = matrixD.M24 = 0.0;
      matrixD.M33 = 1.0 / (zNearPlane - zFarPlane);
      matrixD.M31 = matrixD.M32 = matrixD.M34 = 0.0;
      matrixD.M41 = (left + right) / (left - right);
      matrixD.M42 = (top + bottom) / (bottom - top);
      matrixD.M43 = zNearPlane / (zNearPlane - zFarPlane);
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateOrthographicOffCenter(
      double left,
      double right,
      double bottom,
      double top,
      double zNearPlane,
      double zFarPlane,
      out MatrixD result)
    {
      result.M11 = 2.0 / (right - left);
      result.M12 = result.M13 = result.M14 = 0.0;
      result.M22 = 2.0 / (top - bottom);
      result.M21 = result.M23 = result.M24 = 0.0;
      result.M33 = 1.0 / (zNearPlane - zFarPlane);
      result.M31 = result.M32 = result.M34 = 0.0;
      result.M41 = (left + right) / (left - right);
      result.M42 = (top + bottom) / (bottom - top);
      result.M43 = zNearPlane / (zNearPlane - zFarPlane);
      result.M44 = 1.0;
    }

    public static MatrixD CreateLookAt(
      Vector3D cameraPosition,
      Vector3D cameraTarget,
      Vector3 cameraUpVector)
    {
      return MatrixD.CreateLookAt(cameraPosition, cameraTarget, (Vector3D) cameraUpVector);
    }

    public static MatrixD CreateLookAt(
      Vector3D cameraPosition,
      Vector3D cameraTarget,
      Vector3D cameraUpVector)
    {
      Vector3D vector3D1 = Vector3D.Normalize(cameraPosition - cameraTarget);
      Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D1));
      Vector3D vector1 = Vector3D.Cross(vector3D1, vector3D2);
      MatrixD matrixD;
      matrixD.M11 = vector3D2.X;
      matrixD.M12 = vector1.X;
      matrixD.M13 = vector3D1.X;
      matrixD.M14 = 0.0;
      matrixD.M21 = vector3D2.Y;
      matrixD.M22 = vector1.Y;
      matrixD.M23 = vector3D1.Y;
      matrixD.M24 = 0.0;
      matrixD.M31 = vector3D2.Z;
      matrixD.M32 = vector1.Z;
      matrixD.M33 = vector3D1.Z;
      matrixD.M34 = 0.0;
      matrixD.M41 = -Vector3D.Dot(vector3D2, cameraPosition);
      matrixD.M42 = -Vector3D.Dot(vector1, cameraPosition);
      matrixD.M43 = -Vector3D.Dot(vector3D1, cameraPosition);
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static Matrix CreateLookAtInverse(
      Vector3D cameraPosition,
      Vector3D cameraTarget,
      Vector3D cameraUpVector)
    {
      Vector3D vector3D1 = Vector3D.Normalize(cameraPosition - cameraTarget);
      Vector3D vector2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D1));
      Vector3D vector3D2 = Vector3D.Cross(vector3D1, vector2);
      MatrixD matrixD;
      matrixD.M11 = vector2.X;
      matrixD.M12 = vector2.Y;
      matrixD.M13 = vector2.Z;
      matrixD.M14 = 0.0;
      matrixD.M21 = vector3D2.X;
      matrixD.M22 = vector3D2.Y;
      matrixD.M23 = vector3D2.Z;
      matrixD.M24 = 0.0;
      matrixD.M31 = vector3D1.X;
      matrixD.M32 = vector3D1.Y;
      matrixD.M33 = vector3D1.Z;
      matrixD.M34 = 0.0;
      matrixD.M41 = cameraPosition.X;
      matrixD.M42 = cameraPosition.Y;
      matrixD.M43 = cameraPosition.Z;
      matrixD.M44 = 1.0;
      return (Matrix) ref matrixD;
    }

    public static void CreateLookAt(
      ref Vector3D cameraPosition,
      ref Vector3D cameraTarget,
      ref Vector3D cameraUpVector,
      out MatrixD result)
    {
      Vector3D vector3D1 = Vector3D.Normalize(cameraPosition - cameraTarget);
      Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vector3D1));
      Vector3D vector1 = Vector3D.Cross(vector3D1, vector3D2);
      result.M11 = vector3D2.X;
      result.M12 = vector1.X;
      result.M13 = vector3D1.X;
      result.M14 = 0.0;
      result.M21 = vector3D2.Y;
      result.M22 = vector1.Y;
      result.M23 = vector3D1.Y;
      result.M24 = 0.0;
      result.M31 = vector3D2.Z;
      result.M32 = vector1.Z;
      result.M33 = vector3D1.Z;
      result.M34 = 0.0;
      result.M41 = -Vector3D.Dot(vector3D2, cameraPosition);
      result.M42 = -Vector3D.Dot(vector1, cameraPosition);
      result.M43 = -Vector3D.Dot(vector3D1, cameraPosition);
      result.M44 = 1.0;
    }

    public static MatrixD CreateWorld(Vector3D position, Vector3 forward, Vector3 up) => MatrixD.CreateWorld(position, (Vector3D) forward, (Vector3D) up);

    public static MatrixD CreateWorld(Vector3D position) => MatrixD.CreateWorld(position, Vector3D.Forward, Vector3D.Up);

    public static MatrixD CreateWorld(Vector3D position, Vector3D forward, Vector3D up)
    {
      Vector3D vector3D1 = Vector3D.Normalize(-forward);
      Vector3D vector2 = Vector3D.Normalize(Vector3D.Cross(up, vector3D1));
      Vector3D vector3D2 = Vector3D.Cross(vector3D1, vector2);
      MatrixD matrixD;
      matrixD.M11 = vector2.X;
      matrixD.M12 = vector2.Y;
      matrixD.M13 = vector2.Z;
      matrixD.M14 = 0.0;
      matrixD.M21 = vector3D2.X;
      matrixD.M22 = vector3D2.Y;
      matrixD.M23 = vector3D2.Z;
      matrixD.M24 = 0.0;
      matrixD.M31 = vector3D1.X;
      matrixD.M32 = vector3D1.Y;
      matrixD.M33 = vector3D1.Z;
      matrixD.M34 = 0.0;
      matrixD.M41 = position.X;
      matrixD.M42 = position.Y;
      matrixD.M43 = position.Z;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateWorld(
      ref Vector3D position,
      ref Vector3D forward,
      ref Vector3D up,
      out MatrixD result)
    {
      Vector3D vector3D1 = Vector3D.Normalize(-forward);
      Vector3D vector2 = Vector3D.Normalize(Vector3D.Cross(up, vector3D1));
      Vector3D vector3D2 = Vector3D.Cross(vector3D1, vector2);
      result.M11 = vector2.X;
      result.M12 = vector2.Y;
      result.M13 = vector2.Z;
      result.M14 = 0.0;
      result.M21 = vector3D2.X;
      result.M22 = vector3D2.Y;
      result.M23 = vector3D2.Z;
      result.M24 = 0.0;
      result.M31 = vector3D1.X;
      result.M32 = vector3D1.Y;
      result.M33 = vector3D1.Z;
      result.M34 = 0.0;
      result.M41 = position.X;
      result.M42 = position.Y;
      result.M43 = position.Z;
      result.M44 = 1.0;
    }

    public static MatrixD CreateFromQuaternion(Quaternion quaternion)
    {
      double num1 = (double) quaternion.X * (double) quaternion.X;
      double num2 = (double) quaternion.Y * (double) quaternion.Y;
      double num3 = (double) quaternion.Z * (double) quaternion.Z;
      double num4 = (double) quaternion.X * (double) quaternion.Y;
      double num5 = (double) quaternion.Z * (double) quaternion.W;
      double num6 = (double) quaternion.Z * (double) quaternion.X;
      double num7 = (double) quaternion.Y * (double) quaternion.W;
      double num8 = (double) quaternion.Y * (double) quaternion.Z;
      double num9 = (double) quaternion.X * (double) quaternion.W;
      MatrixD matrixD;
      matrixD.M11 = 1.0 - 2.0 * (num2 + num3);
      matrixD.M12 = 2.0 * (num4 + num5);
      matrixD.M13 = 2.0 * (num6 - num7);
      matrixD.M14 = 0.0;
      matrixD.M21 = 2.0 * (num4 - num5);
      matrixD.M22 = 1.0 - 2.0 * (num3 + num1);
      matrixD.M23 = 2.0 * (num8 + num9);
      matrixD.M24 = 0.0;
      matrixD.M31 = 2.0 * (num6 + num7);
      matrixD.M32 = 2.0 * (num8 - num9);
      matrixD.M33 = 1.0 - 2.0 * (num2 + num1);
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static MatrixD CreateFromQuaternion(QuaternionD quaternion)
    {
      double num1 = quaternion.X * quaternion.X;
      double num2 = quaternion.Y * quaternion.Y;
      double num3 = quaternion.Z * quaternion.Z;
      double num4 = quaternion.X * quaternion.Y;
      double num5 = quaternion.Z * quaternion.W;
      double num6 = quaternion.Z * quaternion.X;
      double num7 = quaternion.Y * quaternion.W;
      double num8 = quaternion.Y * quaternion.Z;
      double num9 = quaternion.X * quaternion.W;
      MatrixD matrixD;
      matrixD.M11 = 1.0 - 2.0 * (num2 + num3);
      matrixD.M12 = 2.0 * (num4 + num5);
      matrixD.M13 = 2.0 * (num6 - num7);
      matrixD.M14 = 0.0;
      matrixD.M21 = 2.0 * (num4 - num5);
      matrixD.M22 = 1.0 - 2.0 * (num3 + num1);
      matrixD.M23 = 2.0 * (num8 + num9);
      matrixD.M24 = 0.0;
      matrixD.M31 = 2.0 * (num6 + num7);
      matrixD.M32 = 2.0 * (num8 - num9);
      matrixD.M33 = 1.0 - 2.0 * (num2 + num1);
      matrixD.M34 = 0.0;
      matrixD.M41 = 0.0;
      matrixD.M42 = 0.0;
      matrixD.M43 = 0.0;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateFromQuaternion(ref Quaternion quaternion, out MatrixD result)
    {
      double num1 = (double) quaternion.X * (double) quaternion.X;
      double num2 = (double) quaternion.Y * (double) quaternion.Y;
      double num3 = (double) quaternion.Z * (double) quaternion.Z;
      double num4 = (double) quaternion.X * (double) quaternion.Y;
      double num5 = (double) quaternion.Z * (double) quaternion.W;
      double num6 = (double) quaternion.Z * (double) quaternion.X;
      double num7 = (double) quaternion.Y * (double) quaternion.W;
      double num8 = (double) quaternion.Y * (double) quaternion.Z;
      double num9 = (double) quaternion.X * (double) quaternion.W;
      result.M11 = 1.0 - 2.0 * (num2 + num3);
      result.M12 = 2.0 * (num4 + num5);
      result.M13 = 2.0 * (num6 - num7);
      result.M14 = 0.0;
      result.M21 = 2.0 * (num4 - num5);
      result.M22 = 1.0 - 2.0 * (num3 + num1);
      result.M23 = 2.0 * (num8 + num9);
      result.M24 = 0.0;
      result.M31 = 2.0 * (num6 + num7);
      result.M32 = 2.0 * (num8 - num9);
      result.M33 = 1.0 - 2.0 * (num2 + num1);
      result.M34 = 0.0;
      result.M41 = 0.0;
      result.M42 = 0.0;
      result.M43 = 0.0;
      result.M44 = 1.0;
    }

    public static MatrixD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll((float) yaw, (float) pitch, (float) roll, out result1);
      MatrixD result2;
      MatrixD.CreateFromQuaternion(ref result1, out result2);
      return result2;
    }

    public static void CreateFromYawPitchRoll(
      double yaw,
      double pitch,
      double roll,
      out MatrixD result)
    {
      Quaternion result1;
      Quaternion.CreateFromYawPitchRoll((float) yaw, (float) pitch, (float) roll, out result1);
      MatrixD.CreateFromQuaternion(ref result1, out result);
    }

    public static MatrixD CreateFromTransformScale(
      Quaternion orientation,
      Vector3D position,
      Vector3D scale)
    {
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(orientation);
      fromQuaternion.Translation = position;
      MatrixD.Rescale(ref fromQuaternion, ref scale);
      return fromQuaternion;
    }

    public static MatrixD CreateShadow(Vector3D lightDirection, Plane plane)
    {
      Plane result;
      Plane.Normalize(ref plane, out result);
      double num1 = (double) result.Normal.X * lightDirection.X + (double) result.Normal.Y * lightDirection.Y + (double) result.Normal.Z * lightDirection.Z;
      double num2 = -(double) result.Normal.X;
      double num3 = -(double) result.Normal.Y;
      double num4 = -(double) result.Normal.Z;
      double num5 = -(double) result.D;
      MatrixD matrixD;
      matrixD.M11 = num2 * lightDirection.X + num1;
      matrixD.M21 = num3 * lightDirection.X;
      matrixD.M31 = num4 * lightDirection.X;
      matrixD.M41 = num5 * lightDirection.X;
      matrixD.M12 = num2 * lightDirection.Y;
      matrixD.M22 = num3 * lightDirection.Y + num1;
      matrixD.M32 = num4 * lightDirection.Y;
      matrixD.M42 = num5 * lightDirection.Y;
      matrixD.M13 = num2 * lightDirection.Z;
      matrixD.M23 = num3 * lightDirection.Z;
      matrixD.M33 = num4 * lightDirection.Z + num1;
      matrixD.M43 = num5 * lightDirection.Z;
      matrixD.M14 = 0.0;
      matrixD.M24 = 0.0;
      matrixD.M34 = 0.0;
      matrixD.M44 = num1;
      return matrixD;
    }

    public static void CreateShadow(
      ref Vector3D lightDirection,
      ref Plane plane,
      out MatrixD result)
    {
      Plane result1;
      Plane.Normalize(ref plane, out result1);
      double num1 = (double) result1.Normal.X * lightDirection.X + (double) result1.Normal.Y * lightDirection.Y + (double) result1.Normal.Z * lightDirection.Z;
      double num2 = -(double) result1.Normal.X;
      double num3 = -(double) result1.Normal.Y;
      double num4 = -(double) result1.Normal.Z;
      double num5 = -(double) result1.D;
      result.M11 = num2 * lightDirection.X + num1;
      result.M21 = num3 * lightDirection.X;
      result.M31 = num4 * lightDirection.X;
      result.M41 = num5 * lightDirection.X;
      result.M12 = num2 * lightDirection.Y;
      result.M22 = num3 * lightDirection.Y + num1;
      result.M32 = num4 * lightDirection.Y;
      result.M42 = num5 * lightDirection.Y;
      result.M13 = num2 * lightDirection.Z;
      result.M23 = num3 * lightDirection.Z;
      result.M33 = num4 * lightDirection.Z + num1;
      result.M43 = num5 * lightDirection.Z;
      result.M14 = 0.0;
      result.M24 = 0.0;
      result.M34 = 0.0;
      result.M44 = num1;
    }

    public static MatrixD CreateReflection(Plane value)
    {
      value.Normalize();
      double x = (double) value.Normal.X;
      double y = (double) value.Normal.Y;
      double z = (double) value.Normal.Z;
      double num1 = -2.0 * x;
      double num2 = -2.0 * y;
      double num3 = -2.0 * z;
      MatrixD matrixD;
      matrixD.M11 = num1 * x + 1.0;
      matrixD.M12 = num2 * x;
      matrixD.M13 = num3 * x;
      matrixD.M14 = 0.0;
      matrixD.M21 = num1 * y;
      matrixD.M22 = num2 * y + 1.0;
      matrixD.M23 = num3 * y;
      matrixD.M24 = 0.0;
      matrixD.M31 = num1 * z;
      matrixD.M32 = num2 * z;
      matrixD.M33 = num3 * z + 1.0;
      matrixD.M34 = 0.0;
      matrixD.M41 = num1 * (double) value.D;
      matrixD.M42 = num2 * (double) value.D;
      matrixD.M43 = num3 * (double) value.D;
      matrixD.M44 = 1.0;
      return matrixD;
    }

    public static void CreateReflection(ref Plane value, out MatrixD result)
    {
      Plane result1;
      Plane.Normalize(ref value, out result1);
      value.Normalize();
      double x = (double) result1.Normal.X;
      double y = (double) result1.Normal.Y;
      double z = (double) result1.Normal.Z;
      double num1 = -2.0 * x;
      double num2 = -2.0 * y;
      double num3 = -2.0 * z;
      result.M11 = num1 * x + 1.0;
      result.M12 = num2 * x;
      result.M13 = num3 * x;
      result.M14 = 0.0;
      result.M21 = num1 * y;
      result.M22 = num2 * y + 1.0;
      result.M23 = num3 * y;
      result.M24 = 0.0;
      result.M31 = num1 * z;
      result.M32 = num2 * z;
      result.M33 = num3 * z + 1.0;
      result.M34 = 0.0;
      result.M41 = num1 * (double) result1.D;
      result.M42 = num2 * (double) result1.D;
      result.M43 = num3 * (double) result1.D;
      result.M44 = 1.0;
    }

    public static MatrixD Transform(MatrixD value, Quaternion rotation)
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
      MatrixD matrixD;
      matrixD.M11 = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
      matrixD.M12 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
      matrixD.M13 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
      matrixD.M14 = value.M14;
      matrixD.M21 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
      matrixD.M22 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
      matrixD.M23 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
      matrixD.M24 = value.M24;
      matrixD.M31 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
      matrixD.M32 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
      matrixD.M33 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
      matrixD.M34 = value.M34;
      matrixD.M41 = value.M41 * num13 + value.M42 * num14 + value.M43 * num15;
      matrixD.M42 = value.M41 * num16 + value.M42 * num17 + value.M43 * num18;
      matrixD.M43 = value.M41 * num19 + value.M42 * num20 + value.M43 * num21;
      matrixD.M44 = value.M44;
      return matrixD;
    }

    public static void Transform(ref MatrixD value, ref Quaternion rotation, out MatrixD result)
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
      double num22 = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
      double num23 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
      double num24 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
      double m14 = value.M14;
      double num25 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
      double num26 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
      double num27 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
      double m24 = value.M24;
      double num28 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
      double num29 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
      double num30 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
      double m34 = value.M34;
      double num31 = value.M41 * num13 + value.M42 * num14 + value.M43 * num15;
      double num32 = value.M41 * num16 + value.M42 * num17 + value.M43 * num18;
      double num33 = value.M41 * num19 + value.M42 * num20 + value.M43 * num21;
      double m44 = value.M44;
      result.M11 = num22;
      result.M12 = num23;
      result.M13 = num24;
      result.M14 = m14;
      result.M21 = num25;
      result.M22 = num26;
      result.M23 = num27;
      result.M24 = m24;
      result.M31 = num28;
      result.M32 = num29;
      result.M33 = num30;
      result.M34 = m34;
      result.M41 = num31;
      result.M42 = num32;
      result.M43 = num33;
      result.M44 = m44;
    }

    public unsafe Vector4 GetRow(int row)
    {
      if (row < 0 || row > 3)
        throw new ArgumentOutOfRangeException();
      fixed (double* numPtr1 = &this.M11)
      {
        double* numPtr2 = numPtr1 + row * 4;
        return new Vector4((float) *numPtr2, (float) numPtr2[1], (float) numPtr2[2], (float) numPtr2[3]);
      }
    }

    public unsafe void SetRow(int row, Vector4 value)
    {
      if (row < 0 || row > 3)
        throw new ArgumentOutOfRangeException();
      fixed (double* numPtr1 = &this.M11)
      {
        double* numPtr2 = numPtr1 + row * 4;
        *numPtr2 = (double) value.X;
        numPtr2[1] = (double) value.Y;
        numPtr2[2] = (double) value.Z;
        numPtr2[3] = (double) value.W;
      }
    }

    public unsafe double this[int row, int column]
    {
      get
      {
        if (row < 0 || row > 3 || (column < 0 || column > 3))
          throw new ArgumentOutOfRangeException();
        fixed (double* numPtr = &this.M11)
          return numPtr[row * 4 + column];
      }
      set
      {
        if (row < 0 || row > 3 || (column < 0 || column > 3))
          throw new ArgumentOutOfRangeException();
        fixed (double* numPtr = &this.M11)
          numPtr[row * 4 + column] = value;
      }
    }

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return "{ " + string.Format((IFormatProvider) currentCulture, "{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", (object) this.M11.ToString((IFormatProvider) currentCulture), (object) this.M12.ToString((IFormatProvider) currentCulture), (object) this.M13.ToString((IFormatProvider) currentCulture), (object) this.M14.ToString((IFormatProvider) currentCulture)) + string.Format((IFormatProvider) currentCulture, "{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", (object) this.M21.ToString((IFormatProvider) currentCulture), (object) this.M22.ToString((IFormatProvider) currentCulture), (object) this.M23.ToString((IFormatProvider) currentCulture), (object) this.M24.ToString((IFormatProvider) currentCulture)) + string.Format((IFormatProvider) currentCulture, "{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", (object) this.M31.ToString((IFormatProvider) currentCulture), (object) this.M32.ToString((IFormatProvider) currentCulture), (object) this.M33.ToString((IFormatProvider) currentCulture), (object) this.M34.ToString((IFormatProvider) currentCulture)) + string.Format((IFormatProvider) currentCulture, "{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", (object) this.M41.ToString((IFormatProvider) currentCulture), (object) this.M42.ToString((IFormatProvider) currentCulture), (object) this.M43.ToString((IFormatProvider) currentCulture), (object) this.M44.ToString((IFormatProvider) currentCulture)) + "}";
    }

    public bool Equals(MatrixD other) => this.M11 == other.M11 && this.M22 == other.M22 && (this.M33 == other.M33 && this.M44 == other.M44) && (this.M12 == other.M12 && this.M13 == other.M13 && (this.M14 == other.M14 && this.M21 == other.M21)) && (this.M23 == other.M23 && this.M24 == other.M24 && (this.M31 == other.M31 && this.M32 == other.M32) && (this.M34 == other.M34 && this.M41 == other.M41 && this.M42 == other.M42)) && this.M43 == other.M43;

    public bool EqualsFast(ref MatrixD other, double epsilon = 0.0001)
    {
      double num1 = this.M21 - other.M21;
      double num2 = this.M22 - other.M22;
      double num3 = this.M23 - other.M23;
      double num4 = this.M31 - other.M31;
      double num5 = this.M32 - other.M32;
      double num6 = this.M33 - other.M33;
      double num7 = this.M41 - other.M41;
      double num8 = this.M42 - other.M42;
      double num9 = this.M43 - other.M43;
      double num10 = epsilon * epsilon;
      return num1 * num1 + num2 * num2 + num3 * num3 < num10 & num4 * num4 + num5 * num5 + num6 * num6 < num10 & num7 * num7 + num8 * num8 + num9 * num9 < num10;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is MatrixD other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M14.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M24.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode() + this.M34.GetHashCode() + this.M41.GetHashCode() + this.M42.GetHashCode() + this.M43.GetHashCode() + this.M44.GetHashCode();

    public static MatrixD Transpose(MatrixD matrix)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix.M11;
      matrixD.M12 = matrix.M21;
      matrixD.M13 = matrix.M31;
      matrixD.M14 = matrix.M41;
      matrixD.M21 = matrix.M12;
      matrixD.M22 = matrix.M22;
      matrixD.M23 = matrix.M32;
      matrixD.M24 = matrix.M42;
      matrixD.M31 = matrix.M13;
      matrixD.M32 = matrix.M23;
      matrixD.M33 = matrix.M33;
      matrixD.M34 = matrix.M43;
      matrixD.M41 = matrix.M14;
      matrixD.M42 = matrix.M24;
      matrixD.M43 = matrix.M34;
      matrixD.M44 = matrix.M44;
      return matrixD;
    }

    public static void Transpose(ref MatrixD matrix, out MatrixD result)
    {
      double m11 = matrix.M11;
      double m12 = matrix.M12;
      double m13 = matrix.M13;
      double m14 = matrix.M14;
      double m21 = matrix.M21;
      double m22 = matrix.M22;
      double m23 = matrix.M23;
      double m24 = matrix.M24;
      double m31 = matrix.M31;
      double m32 = matrix.M32;
      double m33 = matrix.M33;
      double m34 = matrix.M34;
      double m41 = matrix.M41;
      double m42 = matrix.M42;
      double m43 = matrix.M43;
      double m44 = matrix.M44;
      result.M11 = m11;
      result.M12 = m21;
      result.M13 = m31;
      result.M14 = m41;
      result.M21 = m12;
      result.M22 = m22;
      result.M23 = m32;
      result.M24 = m42;
      result.M31 = m13;
      result.M32 = m23;
      result.M33 = m33;
      result.M34 = m43;
      result.M41 = m14;
      result.M42 = m24;
      result.M43 = m34;
      result.M44 = m44;
    }

    public double Determinant()
    {
      double m11 = this.M11;
      double m12 = this.M12;
      double m13 = this.M13;
      double m14 = this.M14;
      double m21 = this.M21;
      double m22 = this.M22;
      double m23 = this.M23;
      double m24 = this.M24;
      double m31 = this.M31;
      double m32 = this.M32;
      double m33 = this.M33;
      double m34 = this.M34;
      double m41 = this.M41;
      double m42 = this.M42;
      double m43 = this.M43;
      double m44 = this.M44;
      double num1 = m33 * m44 - m34 * m43;
      double num2 = m32 * m44 - m34 * m42;
      double num3 = m32 * m43 - m33 * m42;
      double num4 = m31 * m44 - m34 * m41;
      double num5 = m31 * m43 - m33 * m41;
      double num6 = m31 * m42 - m32 * m41;
      return m11 * (m22 * num1 - m23 * num2 + m24 * num3) - m12 * (m21 * num1 - m23 * num4 + m24 * num5) + m13 * (m21 * num2 - m22 * num4 + m24 * num6) - m14 * (m21 * num3 - m22 * num5 + m23 * num6);
    }

    public static MatrixD Invert(MatrixD matrix) => MatrixD.Invert(ref matrix);

    public static MatrixD Invert(ref MatrixD matrix)
    {
      double m11 = matrix.M11;
      double m12 = matrix.M12;
      double m13 = matrix.M13;
      double m14 = matrix.M14;
      double m21 = matrix.M21;
      double m22 = matrix.M22;
      double m23 = matrix.M23;
      double m24 = matrix.M24;
      double m31 = matrix.M31;
      double m32 = matrix.M32;
      double m33 = matrix.M33;
      double m34 = matrix.M34;
      double m41 = matrix.M41;
      double m42 = matrix.M42;
      double m43 = matrix.M43;
      double m44 = matrix.M44;
      double num1 = m33 * m44 - m34 * m43;
      double num2 = m32 * m44 - m34 * m42;
      double num3 = m32 * m43 - m33 * m42;
      double num4 = m31 * m44 - m34 * m41;
      double num5 = m31 * m43 - m33 * m41;
      double num6 = m31 * m42 - m32 * m41;
      double num7 = m22 * num1 - m23 * num2 + m24 * num3;
      double num8 = -(m21 * num1 - m23 * num4 + m24 * num5);
      double num9 = m21 * num2 - m22 * num4 + m24 * num6;
      double num10 = -(m21 * num3 - m22 * num5 + m23 * num6);
      double num11 = 1.0 / (m11 * num7 + m12 * num8 + m13 * num9 + m14 * num10);
      MatrixD matrixD;
      matrixD.M11 = num7 * num11;
      matrixD.M21 = num8 * num11;
      matrixD.M31 = num9 * num11;
      matrixD.M41 = num10 * num11;
      matrixD.M12 = -(m12 * num1 - m13 * num2 + m14 * num3) * num11;
      matrixD.M22 = (m11 * num1 - m13 * num4 + m14 * num5) * num11;
      matrixD.M32 = -(m11 * num2 - m12 * num4 + m14 * num6) * num11;
      matrixD.M42 = (m11 * num3 - m12 * num5 + m13 * num6) * num11;
      double num12 = m23 * m44 - m24 * m43;
      double num13 = m22 * m44 - m24 * m42;
      double num14 = m22 * m43 - m23 * m42;
      double num15 = m21 * m44 - m24 * m41;
      double num16 = m21 * m43 - m23 * m41;
      double num17 = m21 * m42 - m22 * m41;
      matrixD.M13 = (m12 * num12 - m13 * num13 + m14 * num14) * num11;
      matrixD.M23 = -(m11 * num12 - m13 * num15 + m14 * num16) * num11;
      matrixD.M33 = (m11 * num13 - m12 * num15 + m14 * num17) * num11;
      matrixD.M43 = -(m11 * num14 - m12 * num16 + m13 * num17) * num11;
      double num18 = m23 * m34 - m24 * m33;
      double num19 = m22 * m34 - m24 * m32;
      double num20 = m22 * m33 - m23 * m32;
      double num21 = m21 * m34 - m24 * m31;
      double num22 = m21 * m33 - m23 * m31;
      double num23 = m21 * m32 - m22 * m31;
      matrixD.M14 = -(m12 * num18 - m13 * num19 + m14 * num20) * num11;
      matrixD.M24 = (m11 * num18 - m13 * num21 + m14 * num22) * num11;
      matrixD.M34 = -(m11 * num19 - m12 * num21 + m14 * num23) * num11;
      matrixD.M44 = (m11 * num20 - m12 * num22 + m13 * num23) * num11;
      return matrixD;
    }

    public static void Invert(ref MatrixD matrix, out MatrixD result)
    {
      double m11 = matrix.M11;
      double m12 = matrix.M12;
      double m13 = matrix.M13;
      double m14 = matrix.M14;
      double m21 = matrix.M21;
      double m22 = matrix.M22;
      double m23 = matrix.M23;
      double m24 = matrix.M24;
      double m31 = matrix.M31;
      double m32 = matrix.M32;
      double m33 = matrix.M33;
      double m34 = matrix.M34;
      double m41 = matrix.M41;
      double m42 = matrix.M42;
      double m43 = matrix.M43;
      double m44 = matrix.M44;
      double num1 = m33 * m44 - m34 * m43;
      double num2 = m32 * m44 - m34 * m42;
      double num3 = m32 * m43 - m33 * m42;
      double num4 = m31 * m44 - m34 * m41;
      double num5 = m31 * m43 - m33 * m41;
      double num6 = m31 * m42 - m32 * m41;
      double num7 = m22 * num1 - m23 * num2 + m24 * num3;
      double num8 = -(m21 * num1 - m23 * num4 + m24 * num5);
      double num9 = m21 * num2 - m22 * num4 + m24 * num6;
      double num10 = -(m21 * num3 - m22 * num5 + m23 * num6);
      double num11 = 1.0 / (m11 * num7 + m12 * num8 + m13 * num9 + m14 * num10);
      result.M11 = num7 * num11;
      result.M21 = num8 * num11;
      result.M31 = num9 * num11;
      result.M41 = num10 * num11;
      result.M12 = -(m12 * num1 - m13 * num2 + m14 * num3) * num11;
      result.M22 = (m11 * num1 - m13 * num4 + m14 * num5) * num11;
      result.M32 = -(m11 * num2 - m12 * num4 + m14 * num6) * num11;
      result.M42 = (m11 * num3 - m12 * num5 + m13 * num6) * num11;
      double num12 = m23 * m44 - m24 * m43;
      double num13 = m22 * m44 - m24 * m42;
      double num14 = m22 * m43 - m23 * m42;
      double num15 = m21 * m44 - m24 * m41;
      double num16 = m21 * m43 - m23 * m41;
      double num17 = m21 * m42 - m22 * m41;
      result.M13 = (m12 * num12 - m13 * num13 + m14 * num14) * num11;
      result.M23 = -(m11 * num12 - m13 * num15 + m14 * num16) * num11;
      result.M33 = (m11 * num13 - m12 * num15 + m14 * num17) * num11;
      result.M43 = -(m11 * num14 - m12 * num16 + m13 * num17) * num11;
      double num18 = m23 * m34 - m24 * m33;
      double num19 = m22 * m34 - m24 * m32;
      double num20 = m22 * m33 - m23 * m32;
      double num21 = m21 * m34 - m24 * m31;
      double num22 = m21 * m33 - m23 * m31;
      double num23 = m21 * m32 - m22 * m31;
      result.M14 = -(m12 * num18 - m13 * num19 + m14 * num20) * num11;
      result.M24 = (m11 * num18 - m13 * num21 + m14 * num22) * num11;
      result.M34 = -(m11 * num19 - m12 * num21 + m14 * num23) * num11;
      result.M44 = (m11 * num20 - m12 * num22 + m13 * num23) * num11;
    }

    public static MatrixD Lerp(MatrixD matrix1, MatrixD matrix2, double amount)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
      matrixD.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
      matrixD.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
      matrixD.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
      matrixD.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
      matrixD.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
      matrixD.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
      matrixD.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
      matrixD.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
      matrixD.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
      matrixD.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
      matrixD.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
      matrixD.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
      matrixD.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
      matrixD.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
      matrixD.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
      return matrixD;
    }

    public static void Lerp(
      ref MatrixD matrix1,
      ref MatrixD matrix2,
      double amount,
      out MatrixD result)
    {
      result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
      result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
      result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
      result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
      result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
      result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
      result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
      result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
      result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
      result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
      result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
      result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
      result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
      result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
      result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
      result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
    }

    public static void Slerp(
      ref MatrixD matrix1,
      ref MatrixD matrix2,
      float amount,
      out MatrixD result)
    {
      Quaternion result1;
      Quaternion.CreateFromRotationMatrix(ref matrix1, out result1);
      Quaternion result2;
      Quaternion.CreateFromRotationMatrix(ref matrix2, out result2);
      Quaternion result3;
      Quaternion.Slerp(ref result1, ref result2, amount, out result3);
      MatrixD.CreateFromQuaternion(ref result3, out result);
      result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * (double) amount;
      result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * (double) amount;
      result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * (double) amount;
    }

    public bool IsOrthogonal() => this.IsOrthogonal(0.0001);

    public bool IsOrthogonal(double epsilon) => Math.Abs(this.Up.LengthSquared()) - 1.0 < epsilon && Math.Abs(this.Right.LengthSquared()) - 1.0 < epsilon && (Math.Abs(this.Forward.LengthSquared()) - 1.0 < epsilon && Math.Abs(this.Right.Dot(this.Up)) < epsilon) && Math.Abs(this.Right.Dot(this.Forward)) < epsilon;

    public static void SlerpScale(
      ref MatrixD matrix1,
      ref MatrixD matrix2,
      float amount,
      out MatrixD result)
    {
      Vector3D scale1 = matrix1.Scale;
      Vector3D scale2 = matrix2.Scale;
      if (scale1.LengthSquared() < 0.00999999977648258 || scale2.LengthSquared() < 0.00999999977648258)
      {
        result = MatrixD.Zero;
      }
      else
      {
        MatrixD matrix3 = MatrixD.Normalize(matrix1);
        MatrixD matrix4 = MatrixD.Normalize(matrix2);
        Quaternion result1;
        Quaternion.CreateFromRotationMatrix(ref matrix3, out result1);
        Quaternion result2;
        Quaternion.CreateFromRotationMatrix(ref matrix4, out result2);
        Quaternion result3;
        Quaternion.Slerp(ref result1, ref result2, amount, out result3);
        MatrixD.CreateFromQuaternion(ref result3, out result);
        Vector3D scale3 = Vector3D.Lerp(scale1, scale2, (double) amount);
        MatrixD.Rescale(ref result, ref scale3);
        result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * (double) amount;
        result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * (double) amount;
        result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * (double) amount;
      }
    }

    public static void Slerp(MatrixD matrix1, MatrixD matrix2, float amount, out MatrixD result) => MatrixD.Slerp(ref matrix1, ref matrix2, amount, out result);

    public static MatrixD Slerp(MatrixD matrix1, MatrixD matrix2, float amount)
    {
      MatrixD result;
      MatrixD.Slerp(ref matrix1, ref matrix2, amount, out result);
      return result;
    }

    public static void SlerpScale(
      MatrixD matrix1,
      MatrixD matrix2,
      float amount,
      out MatrixD result)
    {
      MatrixD.SlerpScale(ref matrix1, ref matrix2, amount, out result);
    }

    public static MatrixD SlerpScale(MatrixD matrix1, MatrixD matrix2, float amount)
    {
      MatrixD result;
      MatrixD.SlerpScale(ref matrix1, ref matrix2, amount, out result);
      return result;
    }

    public static MatrixD Negate(MatrixD matrix)
    {
      MatrixD matrixD;
      matrixD.M11 = -matrix.M11;
      matrixD.M12 = -matrix.M12;
      matrixD.M13 = -matrix.M13;
      matrixD.M14 = -matrix.M14;
      matrixD.M21 = -matrix.M21;
      matrixD.M22 = -matrix.M22;
      matrixD.M23 = -matrix.M23;
      matrixD.M24 = -matrix.M24;
      matrixD.M31 = -matrix.M31;
      matrixD.M32 = -matrix.M32;
      matrixD.M33 = -matrix.M33;
      matrixD.M34 = -matrix.M34;
      matrixD.M41 = -matrix.M41;
      matrixD.M42 = -matrix.M42;
      matrixD.M43 = -matrix.M43;
      matrixD.M44 = -matrix.M44;
      return matrixD;
    }

    public static void Negate(ref MatrixD matrix, out MatrixD result)
    {
      result.M11 = -matrix.M11;
      result.M12 = -matrix.M12;
      result.M13 = -matrix.M13;
      result.M14 = -matrix.M14;
      result.M21 = -matrix.M21;
      result.M22 = -matrix.M22;
      result.M23 = -matrix.M23;
      result.M24 = -matrix.M24;
      result.M31 = -matrix.M31;
      result.M32 = -matrix.M32;
      result.M33 = -matrix.M33;
      result.M34 = -matrix.M34;
      result.M41 = -matrix.M41;
      result.M42 = -matrix.M42;
      result.M43 = -matrix.M43;
      result.M44 = -matrix.M44;
    }

    public static MatrixD Add(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 + matrix2.M11;
      matrixD.M12 = matrix1.M12 + matrix2.M12;
      matrixD.M13 = matrix1.M13 + matrix2.M13;
      matrixD.M14 = matrix1.M14 + matrix2.M14;
      matrixD.M21 = matrix1.M21 + matrix2.M21;
      matrixD.M22 = matrix1.M22 + matrix2.M22;
      matrixD.M23 = matrix1.M23 + matrix2.M23;
      matrixD.M24 = matrix1.M24 + matrix2.M24;
      matrixD.M31 = matrix1.M31 + matrix2.M31;
      matrixD.M32 = matrix1.M32 + matrix2.M32;
      matrixD.M33 = matrix1.M33 + matrix2.M33;
      matrixD.M34 = matrix1.M34 + matrix2.M34;
      matrixD.M41 = matrix1.M41 + matrix2.M41;
      matrixD.M42 = matrix1.M42 + matrix2.M42;
      matrixD.M43 = matrix1.M43 + matrix2.M43;
      matrixD.M44 = matrix1.M44 + matrix2.M44;
      return matrixD;
    }

    public static void Add(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
    {
      result.M11 = matrix1.M11 + matrix2.M11;
      result.M12 = matrix1.M12 + matrix2.M12;
      result.M13 = matrix1.M13 + matrix2.M13;
      result.M14 = matrix1.M14 + matrix2.M14;
      result.M21 = matrix1.M21 + matrix2.M21;
      result.M22 = matrix1.M22 + matrix2.M22;
      result.M23 = matrix1.M23 + matrix2.M23;
      result.M24 = matrix1.M24 + matrix2.M24;
      result.M31 = matrix1.M31 + matrix2.M31;
      result.M32 = matrix1.M32 + matrix2.M32;
      result.M33 = matrix1.M33 + matrix2.M33;
      result.M34 = matrix1.M34 + matrix2.M34;
      result.M41 = matrix1.M41 + matrix2.M41;
      result.M42 = matrix1.M42 + matrix2.M42;
      result.M43 = matrix1.M43 + matrix2.M43;
      result.M44 = matrix1.M44 + matrix2.M44;
    }

    public static Matrix Subtract(Matrix matrix1, Matrix matrix2)
    {
      Matrix matrix;
      matrix.M11 = matrix1.M11 - matrix2.M11;
      matrix.M12 = matrix1.M12 - matrix2.M12;
      matrix.M13 = matrix1.M13 - matrix2.M13;
      matrix.M14 = matrix1.M14 - matrix2.M14;
      matrix.M21 = matrix1.M21 - matrix2.M21;
      matrix.M22 = matrix1.M22 - matrix2.M22;
      matrix.M23 = matrix1.M23 - matrix2.M23;
      matrix.M24 = matrix1.M24 - matrix2.M24;
      matrix.M31 = matrix1.M31 - matrix2.M31;
      matrix.M32 = matrix1.M32 - matrix2.M32;
      matrix.M33 = matrix1.M33 - matrix2.M33;
      matrix.M34 = matrix1.M34 - matrix2.M34;
      matrix.M41 = matrix1.M41 - matrix2.M41;
      matrix.M42 = matrix1.M42 - matrix2.M42;
      matrix.M43 = matrix1.M43 - matrix2.M43;
      matrix.M44 = matrix1.M44 - matrix2.M44;
      return matrix;
    }

    public static void Subtract(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
    {
      result.M11 = matrix1.M11 - matrix2.M11;
      result.M12 = matrix1.M12 - matrix2.M12;
      result.M13 = matrix1.M13 - matrix2.M13;
      result.M14 = matrix1.M14 - matrix2.M14;
      result.M21 = matrix1.M21 - matrix2.M21;
      result.M22 = matrix1.M22 - matrix2.M22;
      result.M23 = matrix1.M23 - matrix2.M23;
      result.M24 = matrix1.M24 - matrix2.M24;
      result.M31 = matrix1.M31 - matrix2.M31;
      result.M32 = matrix1.M32 - matrix2.M32;
      result.M33 = matrix1.M33 - matrix2.M33;
      result.M34 = matrix1.M34 - matrix2.M34;
      result.M41 = matrix1.M41 - matrix2.M41;
      result.M42 = matrix1.M42 - matrix2.M42;
      result.M43 = matrix1.M43 - matrix2.M43;
      result.M44 = matrix1.M44 - matrix2.M44;
    }

    public static MatrixD Multiply(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
      matrixD.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
      matrixD.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
      matrixD.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
      matrixD.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
      matrixD.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
      matrixD.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
      matrixD.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
      matrixD.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
      matrixD.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
      matrixD.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
      matrixD.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
      matrixD.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
      matrixD.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
      matrixD.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
      matrixD.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
      return matrixD;
    }

    public static MatrixD Multiply(MatrixD matrix1, Matrix matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * (double) matrix2.M11 + matrix1.M12 * (double) matrix2.M21 + matrix1.M13 * (double) matrix2.M31 + matrix1.M14 * (double) matrix2.M41;
      matrixD.M12 = matrix1.M11 * (double) matrix2.M12 + matrix1.M12 * (double) matrix2.M22 + matrix1.M13 * (double) matrix2.M32 + matrix1.M14 * (double) matrix2.M42;
      matrixD.M13 = matrix1.M11 * (double) matrix2.M13 + matrix1.M12 * (double) matrix2.M23 + matrix1.M13 * (double) matrix2.M33 + matrix1.M14 * (double) matrix2.M43;
      matrixD.M14 = matrix1.M11 * (double) matrix2.M14 + matrix1.M12 * (double) matrix2.M24 + matrix1.M13 * (double) matrix2.M34 + matrix1.M14 * (double) matrix2.M44;
      matrixD.M21 = matrix1.M21 * (double) matrix2.M11 + matrix1.M22 * (double) matrix2.M21 + matrix1.M23 * (double) matrix2.M31 + matrix1.M24 * (double) matrix2.M41;
      matrixD.M22 = matrix1.M21 * (double) matrix2.M12 + matrix1.M22 * (double) matrix2.M22 + matrix1.M23 * (double) matrix2.M32 + matrix1.M24 * (double) matrix2.M42;
      matrixD.M23 = matrix1.M21 * (double) matrix2.M13 + matrix1.M22 * (double) matrix2.M23 + matrix1.M23 * (double) matrix2.M33 + matrix1.M24 * (double) matrix2.M43;
      matrixD.M24 = matrix1.M21 * (double) matrix2.M14 + matrix1.M22 * (double) matrix2.M24 + matrix1.M23 * (double) matrix2.M34 + matrix1.M24 * (double) matrix2.M44;
      matrixD.M31 = matrix1.M31 * (double) matrix2.M11 + matrix1.M32 * (double) matrix2.M21 + matrix1.M33 * (double) matrix2.M31 + matrix1.M34 * (double) matrix2.M41;
      matrixD.M32 = matrix1.M31 * (double) matrix2.M12 + matrix1.M32 * (double) matrix2.M22 + matrix1.M33 * (double) matrix2.M32 + matrix1.M34 * (double) matrix2.M42;
      matrixD.M33 = matrix1.M31 * (double) matrix2.M13 + matrix1.M32 * (double) matrix2.M23 + matrix1.M33 * (double) matrix2.M33 + matrix1.M34 * (double) matrix2.M43;
      matrixD.M34 = matrix1.M31 * (double) matrix2.M14 + matrix1.M32 * (double) matrix2.M24 + matrix1.M33 * (double) matrix2.M34 + matrix1.M34 * (double) matrix2.M44;
      matrixD.M41 = matrix1.M41 * (double) matrix2.M11 + matrix1.M42 * (double) matrix2.M21 + matrix1.M43 * (double) matrix2.M31 + matrix1.M44 * (double) matrix2.M41;
      matrixD.M42 = matrix1.M41 * (double) matrix2.M12 + matrix1.M42 * (double) matrix2.M22 + matrix1.M43 * (double) matrix2.M32 + matrix1.M44 * (double) matrix2.M42;
      matrixD.M43 = matrix1.M41 * (double) matrix2.M13 + matrix1.M42 * (double) matrix2.M23 + matrix1.M43 * (double) matrix2.M33 + matrix1.M44 * (double) matrix2.M43;
      matrixD.M44 = matrix1.M41 * (double) matrix2.M14 + matrix1.M42 * (double) matrix2.M24 + matrix1.M43 * (double) matrix2.M34 + matrix1.M44 * (double) matrix2.M44;
      return matrixD;
    }

    public static void Multiply(ref MatrixD matrix1, ref Matrix matrix2, out MatrixD result)
    {
      double num1 = matrix1.M11 * (double) matrix2.M11 + matrix1.M12 * (double) matrix2.M21 + matrix1.M13 * (double) matrix2.M31 + matrix1.M14 * (double) matrix2.M41;
      double num2 = matrix1.M11 * (double) matrix2.M12 + matrix1.M12 * (double) matrix2.M22 + matrix1.M13 * (double) matrix2.M32 + matrix1.M14 * (double) matrix2.M42;
      double num3 = matrix1.M11 * (double) matrix2.M13 + matrix1.M12 * (double) matrix2.M23 + matrix1.M13 * (double) matrix2.M33 + matrix1.M14 * (double) matrix2.M43;
      double num4 = matrix1.M11 * (double) matrix2.M14 + matrix1.M12 * (double) matrix2.M24 + matrix1.M13 * (double) matrix2.M34 + matrix1.M14 * (double) matrix2.M44;
      double num5 = matrix1.M21 * (double) matrix2.M11 + matrix1.M22 * (double) matrix2.M21 + matrix1.M23 * (double) matrix2.M31 + matrix1.M24 * (double) matrix2.M41;
      double num6 = matrix1.M21 * (double) matrix2.M12 + matrix1.M22 * (double) matrix2.M22 + matrix1.M23 * (double) matrix2.M32 + matrix1.M24 * (double) matrix2.M42;
      double num7 = matrix1.M21 * (double) matrix2.M13 + matrix1.M22 * (double) matrix2.M23 + matrix1.M23 * (double) matrix2.M33 + matrix1.M24 * (double) matrix2.M43;
      double num8 = matrix1.M21 * (double) matrix2.M14 + matrix1.M22 * (double) matrix2.M24 + matrix1.M23 * (double) matrix2.M34 + matrix1.M24 * (double) matrix2.M44;
      double num9 = matrix1.M31 * (double) matrix2.M11 + matrix1.M32 * (double) matrix2.M21 + matrix1.M33 * (double) matrix2.M31 + matrix1.M34 * (double) matrix2.M41;
      double num10 = matrix1.M31 * (double) matrix2.M12 + matrix1.M32 * (double) matrix2.M22 + matrix1.M33 * (double) matrix2.M32 + matrix1.M34 * (double) matrix2.M42;
      double num11 = matrix1.M31 * (double) matrix2.M13 + matrix1.M32 * (double) matrix2.M23 + matrix1.M33 * (double) matrix2.M33 + matrix1.M34 * (double) matrix2.M43;
      double num12 = matrix1.M31 * (double) matrix2.M14 + matrix1.M32 * (double) matrix2.M24 + matrix1.M33 * (double) matrix2.M34 + matrix1.M34 * (double) matrix2.M44;
      double num13 = matrix1.M41 * (double) matrix2.M11 + matrix1.M42 * (double) matrix2.M21 + matrix1.M43 * (double) matrix2.M31 + matrix1.M44 * (double) matrix2.M41;
      double num14 = matrix1.M41 * (double) matrix2.M12 + matrix1.M42 * (double) matrix2.M22 + matrix1.M43 * (double) matrix2.M32 + matrix1.M44 * (double) matrix2.M42;
      double num15 = matrix1.M41 * (double) matrix2.M13 + matrix1.M42 * (double) matrix2.M23 + matrix1.M43 * (double) matrix2.M33 + matrix1.M44 * (double) matrix2.M43;
      double num16 = matrix1.M41 * (double) matrix2.M14 + matrix1.M42 * (double) matrix2.M24 + matrix1.M43 * (double) matrix2.M34 + matrix1.M44 * (double) matrix2.M44;
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = num3;
      result.M14 = num4;
      result.M21 = num5;
      result.M22 = num6;
      result.M23 = num7;
      result.M24 = num8;
      result.M31 = num9;
      result.M32 = num10;
      result.M33 = num11;
      result.M34 = num12;
      result.M41 = num13;
      result.M42 = num14;
      result.M43 = num15;
      result.M44 = num16;
    }

    public static void Multiply(ref Matrix matrix1, ref MatrixD matrix2, out MatrixD result)
    {
      double num1 = (double) matrix1.M11 * matrix2.M11 + (double) matrix1.M12 * matrix2.M21 + (double) matrix1.M13 * matrix2.M31 + (double) matrix1.M14 * matrix2.M41;
      double num2 = (double) matrix1.M11 * matrix2.M12 + (double) matrix1.M12 * matrix2.M22 + (double) matrix1.M13 * matrix2.M32 + (double) matrix1.M14 * matrix2.M42;
      double num3 = (double) matrix1.M11 * matrix2.M13 + (double) matrix1.M12 * matrix2.M23 + (double) matrix1.M13 * matrix2.M33 + (double) matrix1.M14 * matrix2.M43;
      double num4 = (double) matrix1.M11 * matrix2.M14 + (double) matrix1.M12 * matrix2.M24 + (double) matrix1.M13 * matrix2.M34 + (double) matrix1.M14 * matrix2.M44;
      double num5 = (double) matrix1.M21 * matrix2.M11 + (double) matrix1.M22 * matrix2.M21 + (double) matrix1.M23 * matrix2.M31 + (double) matrix1.M24 * matrix2.M41;
      double num6 = (double) matrix1.M21 * matrix2.M12 + (double) matrix1.M22 * matrix2.M22 + (double) matrix1.M23 * matrix2.M32 + (double) matrix1.M24 * matrix2.M42;
      double num7 = (double) matrix1.M21 * matrix2.M13 + (double) matrix1.M22 * matrix2.M23 + (double) matrix1.M23 * matrix2.M33 + (double) matrix1.M24 * matrix2.M43;
      double num8 = (double) matrix1.M21 * matrix2.M14 + (double) matrix1.M22 * matrix2.M24 + (double) matrix1.M23 * matrix2.M34 + (double) matrix1.M24 * matrix2.M44;
      double num9 = (double) matrix1.M31 * matrix2.M11 + (double) matrix1.M32 * matrix2.M21 + (double) matrix1.M33 * matrix2.M31 + (double) matrix1.M34 * matrix2.M41;
      double num10 = (double) matrix1.M31 * matrix2.M12 + (double) matrix1.M32 * matrix2.M22 + (double) matrix1.M33 * matrix2.M32 + (double) matrix1.M34 * matrix2.M42;
      double num11 = (double) matrix1.M31 * matrix2.M13 + (double) matrix1.M32 * matrix2.M23 + (double) matrix1.M33 * matrix2.M33 + (double) matrix1.M34 * matrix2.M43;
      double num12 = (double) matrix1.M31 * matrix2.M14 + (double) matrix1.M32 * matrix2.M24 + (double) matrix1.M33 * matrix2.M34 + (double) matrix1.M34 * matrix2.M44;
      double num13 = (double) matrix1.M41 * matrix2.M11 + (double) matrix1.M42 * matrix2.M21 + (double) matrix1.M43 * matrix2.M31 + (double) matrix1.M44 * matrix2.M41;
      double num14 = (double) matrix1.M41 * matrix2.M12 + (double) matrix1.M42 * matrix2.M22 + (double) matrix1.M43 * matrix2.M32 + (double) matrix1.M44 * matrix2.M42;
      double num15 = (double) matrix1.M41 * matrix2.M13 + (double) matrix1.M42 * matrix2.M23 + (double) matrix1.M43 * matrix2.M33 + (double) matrix1.M44 * matrix2.M43;
      double num16 = (double) matrix1.M41 * matrix2.M14 + (double) matrix1.M42 * matrix2.M24 + (double) matrix1.M43 * matrix2.M34 + (double) matrix1.M44 * matrix2.M44;
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = num3;
      result.M14 = num4;
      result.M21 = num5;
      result.M22 = num6;
      result.M23 = num7;
      result.M24 = num8;
      result.M31 = num9;
      result.M32 = num10;
      result.M33 = num11;
      result.M34 = num12;
      result.M41 = num13;
      result.M42 = num14;
      result.M43 = num15;
      result.M44 = num16;
    }

    public static void Multiply(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
    {
      double num1 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
      double num2 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
      double num3 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
      double num4 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;
      double num5 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
      double num6 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
      double num7 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
      double num8 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;
      double num9 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
      double num10 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
      double num11 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
      double num12 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;
      double num13 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
      double num14 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
      double num15 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
      double num16 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
      result.M11 = num1;
      result.M12 = num2;
      result.M13 = num3;
      result.M14 = num4;
      result.M21 = num5;
      result.M22 = num6;
      result.M23 = num7;
      result.M24 = num8;
      result.M31 = num9;
      result.M32 = num10;
      result.M33 = num11;
      result.M34 = num12;
      result.M41 = num13;
      result.M42 = num14;
      result.M43 = num15;
      result.M44 = num16;
    }

    public static MatrixD Multiply(MatrixD matrix1, double scaleFactor)
    {
      double num = scaleFactor;
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * num;
      matrixD.M12 = matrix1.M12 * num;
      matrixD.M13 = matrix1.M13 * num;
      matrixD.M14 = matrix1.M14 * num;
      matrixD.M21 = matrix1.M21 * num;
      matrixD.M22 = matrix1.M22 * num;
      matrixD.M23 = matrix1.M23 * num;
      matrixD.M24 = matrix1.M24 * num;
      matrixD.M31 = matrix1.M31 * num;
      matrixD.M32 = matrix1.M32 * num;
      matrixD.M33 = matrix1.M33 * num;
      matrixD.M34 = matrix1.M34 * num;
      matrixD.M41 = matrix1.M41 * num;
      matrixD.M42 = matrix1.M42 * num;
      matrixD.M43 = matrix1.M43 * num;
      matrixD.M44 = matrix1.M44 * num;
      return matrixD;
    }

    public static void Multiply(ref MatrixD matrix1, double scaleFactor, out MatrixD result)
    {
      double num = scaleFactor;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M14 = matrix1.M14 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M24 = matrix1.M24 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
      result.M34 = matrix1.M34 * num;
      result.M41 = matrix1.M41 * num;
      result.M42 = matrix1.M42 * num;
      result.M43 = matrix1.M43 * num;
      result.M44 = matrix1.M44 * num;
    }

    public static MatrixD Divide(MatrixD matrix1, MatrixD matrix2)
    {
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 / matrix2.M11;
      matrixD.M12 = matrix1.M12 / matrix2.M12;
      matrixD.M13 = matrix1.M13 / matrix2.M13;
      matrixD.M14 = matrix1.M14 / matrix2.M14;
      matrixD.M21 = matrix1.M21 / matrix2.M21;
      matrixD.M22 = matrix1.M22 / matrix2.M22;
      matrixD.M23 = matrix1.M23 / matrix2.M23;
      matrixD.M24 = matrix1.M24 / matrix2.M24;
      matrixD.M31 = matrix1.M31 / matrix2.M31;
      matrixD.M32 = matrix1.M32 / matrix2.M32;
      matrixD.M33 = matrix1.M33 / matrix2.M33;
      matrixD.M34 = matrix1.M34 / matrix2.M34;
      matrixD.M41 = matrix1.M41 / matrix2.M41;
      matrixD.M42 = matrix1.M42 / matrix2.M42;
      matrixD.M43 = matrix1.M43 / matrix2.M43;
      matrixD.M44 = matrix1.M44 / matrix2.M44;
      return matrixD;
    }

    public static void Divide(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
    {
      result.M11 = matrix1.M11 / matrix2.M11;
      result.M12 = matrix1.M12 / matrix2.M12;
      result.M13 = matrix1.M13 / matrix2.M13;
      result.M14 = matrix1.M14 / matrix2.M14;
      result.M21 = matrix1.M21 / matrix2.M21;
      result.M22 = matrix1.M22 / matrix2.M22;
      result.M23 = matrix1.M23 / matrix2.M23;
      result.M24 = matrix1.M24 / matrix2.M24;
      result.M31 = matrix1.M31 / matrix2.M31;
      result.M32 = matrix1.M32 / matrix2.M32;
      result.M33 = matrix1.M33 / matrix2.M33;
      result.M34 = matrix1.M34 / matrix2.M34;
      result.M41 = matrix1.M41 / matrix2.M41;
      result.M42 = matrix1.M42 / matrix2.M42;
      result.M43 = matrix1.M43 / matrix2.M43;
      result.M44 = matrix1.M44 / matrix2.M44;
    }

    public static MatrixD Divide(MatrixD matrix1, double divider)
    {
      double num = 1.0 / divider;
      MatrixD matrixD;
      matrixD.M11 = matrix1.M11 * num;
      matrixD.M12 = matrix1.M12 * num;
      matrixD.M13 = matrix1.M13 * num;
      matrixD.M14 = matrix1.M14 * num;
      matrixD.M21 = matrix1.M21 * num;
      matrixD.M22 = matrix1.M22 * num;
      matrixD.M23 = matrix1.M23 * num;
      matrixD.M24 = matrix1.M24 * num;
      matrixD.M31 = matrix1.M31 * num;
      matrixD.M32 = matrix1.M32 * num;
      matrixD.M33 = matrix1.M33 * num;
      matrixD.M34 = matrix1.M34 * num;
      matrixD.M41 = matrix1.M41 * num;
      matrixD.M42 = matrix1.M42 * num;
      matrixD.M43 = matrix1.M43 * num;
      matrixD.M44 = matrix1.M44 * num;
      return matrixD;
    }

    public static void Divide(ref MatrixD matrix1, double divider, out MatrixD result)
    {
      double num = 1.0 / divider;
      result.M11 = matrix1.M11 * num;
      result.M12 = matrix1.M12 * num;
      result.M13 = matrix1.M13 * num;
      result.M14 = matrix1.M14 * num;
      result.M21 = matrix1.M21 * num;
      result.M22 = matrix1.M22 * num;
      result.M23 = matrix1.M23 * num;
      result.M24 = matrix1.M24 * num;
      result.M31 = matrix1.M31 * num;
      result.M32 = matrix1.M32 * num;
      result.M33 = matrix1.M33 * num;
      result.M34 = matrix1.M34 * num;
      result.M41 = matrix1.M41 * num;
      result.M42 = matrix1.M42 * num;
      result.M43 = matrix1.M43 * num;
      result.M44 = matrix1.M44 * num;
    }

    public MatrixD GetOrientation()
    {
      MatrixD identity = MatrixD.Identity;
      identity.Forward = this.Forward;
      identity.Up = this.Up;
      identity.Right = this.Right;
      return identity;
    }

    [Conditional("DEBUG")]
    public void AssertIsValid(string message = null)
    {
    }

    public bool IsValid() => (this.M11 + this.M12 + this.M13 + this.M14 + this.M21 + this.M22 + this.M23 + this.M24 + this.M31 + this.M32 + this.M33 + this.M34 + this.M41 + this.M42 + this.M43 + this.M44).IsValid();

    public bool IsNan() => double.IsNaN(this.M11 + this.M12 + this.M13 + this.M14 + this.M21 + this.M22 + this.M23 + this.M24 + this.M31 + this.M32 + this.M33 + this.M34 + this.M41 + this.M42 + this.M43 + this.M44);

    public bool IsRotation()
    {
      double num = 0.01;
      return this.HasNoTranslationOrPerspective() && Math.Abs(this.Right.Dot(this.Up)) <= num && (Math.Abs(this.Right.Dot(this.Backward)) <= num && Math.Abs(this.Up.Dot(this.Backward)) <= num) && (Math.Abs(this.Right.LengthSquared() - 1.0) <= num && Math.Abs(this.Up.LengthSquared() - 1.0) <= num && Math.Abs(this.Backward.LengthSquared() - 1.0) <= num);
    }

    public bool HasNoTranslationOrPerspective()
    {
      double num = 9.99999974737875E-05;
      return this.M41 + this.M42 + this.M43 + this.M34 + this.M24 + this.M14 <= num && Math.Abs(this.M44 - 1.0) <= num;
    }

    public static MatrixD CreateFromDir(Vector3D dir) => MatrixD.CreateFromDir(dir, Vector3D.Up);

    public static MatrixD CreateFromDir(Vector3D dir, Vector3D suggestedUp)
    {
      Vector3D up = Vector3D.Cross(Vector3D.Cross(dir, suggestedUp), dir);
      return MatrixD.CreateWorld(Vector3D.Zero, dir, up);
    }

    public static MatrixD Normalize(MatrixD matrix)
    {
      MatrixD matrixD = matrix;
      matrixD.Right = Vector3D.Normalize(matrixD.Right);
      matrixD.Up = Vector3D.Normalize(matrixD.Up);
      matrixD.Forward = Vector3D.Normalize(matrixD.Forward);
      return matrixD;
    }

    public void Orthogonalize()
    {
      Vector3D v1 = Vector3D.Normalize(this.Right);
      Vector3D v2 = Vector3D.Normalize(this.Up - v1 * this.Up.Dot(v1));
      Vector3D vector3D = Vector3D.Normalize(this.Backward - v1 * this.Backward.Dot(v1) - v2 * this.Backward.Dot(v2));
      this.Right = v1;
      this.Up = v2;
      this.Backward = vector3D;
    }

    public static MatrixD Orthogonalize(MatrixD rotationMatrix)
    {
      MatrixD matrixD = rotationMatrix;
      matrixD.Right = Vector3D.Normalize(matrixD.Right);
      matrixD.Up = Vector3D.Normalize(matrixD.Up - matrixD.Right * matrixD.Up.Dot(matrixD.Right));
      matrixD.Backward = Vector3D.Normalize(matrixD.Backward - matrixD.Right * matrixD.Backward.Dot(matrixD.Right) - matrixD.Up * matrixD.Backward.Dot(matrixD.Up));
      return matrixD;
    }

    public static MatrixD AlignRotationToAxes(
      ref MatrixD toAlign,
      ref MatrixD axisDefinitionMatrix)
    {
      MatrixD identity = MatrixD.Identity;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      double num1 = toAlign.Right.Dot(axisDefinitionMatrix.Right);
      double num2 = toAlign.Right.Dot(axisDefinitionMatrix.Up);
      double num3 = toAlign.Right.Dot(axisDefinitionMatrix.Backward);
      if (Math.Abs(num1) > Math.Abs(num2))
      {
        if (Math.Abs(num1) > Math.Abs(num3))
        {
          identity.Right = num1 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
          flag1 = true;
        }
        else
        {
          identity.Right = num3 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
          flag3 = true;
        }
      }
      else if (Math.Abs(num2) > Math.Abs(num3))
      {
        identity.Right = num2 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
        flag2 = true;
      }
      else
      {
        identity.Right = num3 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
        flag3 = true;
      }
      Vector3D vector3D = toAlign.Up;
      double num4 = vector3D.Dot(axisDefinitionMatrix.Right);
      vector3D = toAlign.Up;
      double num5 = vector3D.Dot(axisDefinitionMatrix.Up);
      vector3D = toAlign.Up;
      double num6 = vector3D.Dot(axisDefinitionMatrix.Backward);
      bool flag4;
      if (flag2 || Math.Abs(num4) > Math.Abs(num5) && !flag1)
      {
        if (Math.Abs(num4) > Math.Abs(num6) | flag3)
        {
          identity.Up = num4 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
          flag1 = true;
        }
        else
        {
          identity.Up = num6 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
          flag4 = true;
        }
      }
      else if (Math.Abs(num5) > Math.Abs(num6) | flag3)
      {
        identity.Up = num5 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
        flag2 = true;
      }
      else
      {
        identity.Up = num6 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
        flag4 = true;
      }
      if (!flag1)
      {
        vector3D = toAlign.Backward;
        double num7 = vector3D.Dot(axisDefinitionMatrix.Right);
        identity.Backward = num7 > 0.0 ? axisDefinitionMatrix.Right : axisDefinitionMatrix.Left;
      }
      else if (!flag2)
      {
        vector3D = toAlign.Backward;
        double num7 = vector3D.Dot(axisDefinitionMatrix.Up);
        identity.Backward = num7 > 0.0 ? axisDefinitionMatrix.Up : axisDefinitionMatrix.Down;
      }
      else
      {
        vector3D = toAlign.Backward;
        double num7 = vector3D.Dot(axisDefinitionMatrix.Backward);
        identity.Backward = num7 > 0.0 ? axisDefinitionMatrix.Backward : axisDefinitionMatrix.Forward;
      }
      return identity;
    }

    public static bool GetEulerAnglesXYZ(ref MatrixD mat, out Vector3D xyz)
    {
      double x1 = (double) mat.GetRow(0).X;
      double y1 = (double) mat.GetRow(0).Y;
      double z1 = (double) mat.GetRow(0).Z;
      double x2 = (double) mat.GetRow(1).X;
      double y2 = (double) mat.GetRow(1).Y;
      double z2 = (double) mat.GetRow(1).Z;
      mat.GetRow(2);
      mat.GetRow(2);
      double z3 = (double) mat.GetRow(2).Z;
      double num = z1;
      if (num < 1.0)
      {
        if (num > -1.0)
        {
          xyz = new Vector3D(Math.Atan2(-z2, z3), Math.Asin(z1), Math.Atan2(-y1, x1));
          return true;
        }
        xyz = new Vector3D(-Math.Atan2(x2, y2), -1.57079601287842, 0.0);
        return false;
      }
      xyz = new Vector3D(Math.Atan2(x2, y2), -1.57079601287842, 0.0);
      return false;
    }

    public static MatrixD SwapYZCoordinates(MatrixD m)
    {
      MatrixD matrixD = m;
      Vector3D right = m.Right;
      Vector3D up = m.Up;
      Vector3D forward = m.Forward;
      matrixD.Right = new Vector3D(right.X, right.Z, -right.Y);
      matrixD.Up = new Vector3D(forward.X, forward.Z, -forward.Y);
      matrixD.Forward = new Vector3D(-up.X, -up.Z, up.Y);
      Vector3D translation = m.Translation;
      matrixD.Translation = Vector3D.SwapYZCoordinates(translation);
      return matrixD;
    }

    public bool IsMirrored() => this.Determinant() < 0.0;

    public void SetFrom(in Matrix m)
    {
      this.M11 = (double) m.M11;
      this.M12 = (double) m.M12;
      this.M13 = (double) m.M13;
      this.M14 = (double) m.M14;
      this.M21 = (double) m.M21;
      this.M22 = (double) m.M22;
      this.M23 = (double) m.M23;
      this.M24 = (double) m.M24;
      this.M31 = (double) m.M31;
      this.M32 = (double) m.M32;
      this.M33 = (double) m.M33;
      this.M34 = (double) m.M34;
      this.M41 = (double) m.M41;
      this.M42 = (double) m.M42;
      this.M43 = (double) m.M43;
      this.M44 = (double) m.M44;
    }

    public void SetRotationAndScale(in Matrix m)
    {
      this.M11 = (double) m.M11;
      this.M12 = (double) m.M12;
      this.M13 = (double) m.M13;
      this.M21 = (double) m.M21;
      this.M22 = (double) m.M22;
      this.M23 = (double) m.M23;
      this.M31 = (double) m.M31;
      this.M32 = (double) m.M32;
      this.M33 = (double) m.M33;
    }

    public static implicit operator Matrix(in MatrixD m) => new Matrix()
    {
      M11 = (float) m.M11,
      M12 = (float) m.M12,
      M13 = (float) m.M13,
      M14 = (float) m.M14,
      M21 = (float) m.M21,
      M22 = (float) m.M22,
      M23 = (float) m.M23,
      M24 = (float) m.M24,
      M31 = (float) m.M31,
      M32 = (float) m.M32,
      M33 = (float) m.M33,
      M34 = (float) m.M34,
      M41 = (float) m.M41,
      M42 = (float) m.M42,
      M43 = (float) m.M43,
      M44 = (float) m.M44
    };

    public static implicit operator MatrixD(in Matrix m) => new MatrixD()
    {
      M11 = (double) m.M11,
      M12 = (double) m.M12,
      M13 = (double) m.M13,
      M14 = (double) m.M14,
      M21 = (double) m.M21,
      M22 = (double) m.M22,
      M23 = (double) m.M23,
      M24 = (double) m.M24,
      M31 = (double) m.M31,
      M32 = (double) m.M32,
      M33 = (double) m.M33,
      M34 = (double) m.M34,
      M41 = (double) m.M41,
      M42 = (double) m.M42,
      M43 = (double) m.M43,
      M44 = (double) m.M44
    };

    private struct F16
    {
      public unsafe fixed double data[16];
    }

    protected class VRageMath_MatrixD\u003C\u003EM11\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M11 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M11;
    }

    protected class VRageMath_MatrixD\u003C\u003EM12\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M12 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M12;
    }

    protected class VRageMath_MatrixD\u003C\u003EM13\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M13 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M13;
    }

    protected class VRageMath_MatrixD\u003C\u003EM14\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M14 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M14;
    }

    protected class VRageMath_MatrixD\u003C\u003EM21\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M21 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M21;
    }

    protected class VRageMath_MatrixD\u003C\u003EM22\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M22 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M22;
    }

    protected class VRageMath_MatrixD\u003C\u003EM23\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M23 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M23;
    }

    protected class VRageMath_MatrixD\u003C\u003EM24\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M24 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M24;
    }

    protected class VRageMath_MatrixD\u003C\u003EM31\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M31 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M31;
    }

    protected class VRageMath_MatrixD\u003C\u003EM32\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M32 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M32;
    }

    protected class VRageMath_MatrixD\u003C\u003EM33\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M33 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M33;
    }

    protected class VRageMath_MatrixD\u003C\u003EM34\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M34 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M34;
    }

    protected class VRageMath_MatrixD\u003C\u003EM41\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M41 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M41;
    }

    protected class VRageMath_MatrixD\u003C\u003EM42\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M42 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M42;
    }

    protected class VRageMath_MatrixD\u003C\u003EM43\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M43 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M43;
    }

    protected class VRageMath_MatrixD\u003C\u003EM44\u003C\u003EAccessor : IMemberAccessor<MatrixD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in double value) => owner.M44 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out double value) => value = owner.M44;
    }

    protected class VRageMath_MatrixD\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Up;
    }

    protected class VRageMath_MatrixD\u003C\u003EDown\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Down = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Down;
    }

    protected class VRageMath_MatrixD\u003C\u003ERight\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Right = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Right;
    }

    protected class VRageMath_MatrixD\u003C\u003ELeft\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Left = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Left;
    }

    protected class VRageMath_MatrixD\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Forward;
    }

    protected class VRageMath_MatrixD\u003C\u003EBackward\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Backward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Backward;
    }

    protected class VRageMath_MatrixD\u003C\u003ETranslation\u003C\u003EAccessor : IMemberAccessor<MatrixD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixD owner, in Vector3D value) => owner.Translation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixD owner, out Vector3D value) => value = owner.Translation;
    }
  }
}
