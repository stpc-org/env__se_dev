// Decompiled with JetBrains decompiler
// Type: VRageMath.Plane
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct Plane : IEquatable<Plane>
  {
    public Vector3 Normal;
    public float D;
    private static MyRandom _random;

    public Plane(float a, float b, float c, float d)
    {
      this.Normal.X = a;
      this.Normal.Y = b;
      this.Normal.Z = c;
      this.D = d;
    }

    public Plane(Vector3 normal, float d)
    {
      this.Normal = normal;
      this.D = d;
    }

    public Plane(Vector3 position, Vector3 normal)
    {
      this.Normal = normal;
      this.D = -Vector3.Dot(position, normal);
    }

    public Plane(Vector4 value)
    {
      this.Normal.X = value.X;
      this.Normal.Y = value.Y;
      this.Normal.Z = value.Z;
      this.D = value.W;
    }

    public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
    {
      float num1 = point2.X - point1.X;
      float num2 = point2.Y - point1.Y;
      float num3 = point2.Z - point1.Z;
      float num4 = point3.X - point1.X;
      float num5 = point3.Y - point1.Y;
      float num6 = point3.Z - point1.Z;
      float num7 = (float) ((double) num2 * (double) num6 - (double) num3 * (double) num5);
      float num8 = (float) ((double) num3 * (double) num4 - (double) num1 * (double) num6);
      float num9 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
      float num10 = 1f / (float) Math.Sqrt((double) num7 * (double) num7 + (double) num8 * (double) num8 + (double) num9 * (double) num9);
      this.Normal.X = num7 * num10;
      this.Normal.Y = num8 * num10;
      this.Normal.Z = num9 * num10;
      this.D = (float) -((double) this.Normal.X * (double) point1.X + (double) this.Normal.Y * (double) point1.Y + (double) this.Normal.Z * (double) point1.Z);
    }

    public Plane(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3)
    {
      float num1 = point2.X - point1.X;
      float num2 = point2.Y - point1.Y;
      float num3 = point2.Z - point1.Z;
      float num4 = point3.X - point1.X;
      float num5 = point3.Y - point1.Y;
      float num6 = point3.Z - point1.Z;
      float num7 = (float) ((double) num2 * (double) num6 - (double) num3 * (double) num5);
      float num8 = (float) ((double) num3 * (double) num4 - (double) num1 * (double) num6);
      float num9 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
      float num10 = 1f / (float) Math.Sqrt((double) num7 * (double) num7 + (double) num8 * (double) num8 + (double) num9 * (double) num9);
      this.Normal.X = num7 * num10;
      this.Normal.Y = num8 * num10;
      this.Normal.Z = num9 * num10;
      this.D = (float) -((double) this.Normal.X * (double) point1.X + (double) this.Normal.Y * (double) point1.Y + (double) this.Normal.Z * (double) point1.Z);
    }

    public static bool operator ==(Plane lhs, Plane rhs) => lhs.Equals(rhs);

    public static bool operator !=(Plane lhs, Plane rhs) => (double) lhs.Normal.X != (double) rhs.Normal.X || (double) lhs.Normal.Y != (double) rhs.Normal.Y || (double) lhs.Normal.Z != (double) rhs.Normal.Z || (double) lhs.D != (double) rhs.D;

    public bool Equals(Plane other) => (double) this.Normal.X == (double) other.Normal.X && (double) this.Normal.Y == (double) other.Normal.Y && (double) this.Normal.Z == (double) other.Normal.Z && (double) this.D == (double) other.D;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Plane other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Normal.GetHashCode() + this.D.GetHashCode();

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{Normal:{0} D:{1}}}", new object[2]
      {
        (object) this.Normal.ToString(),
        (object) this.D.ToString((IFormatProvider) currentCulture)
      });
    }

    public void Normalize()
    {
      float num1 = (float) ((double) this.Normal.X * (double) this.Normal.X + (double) this.Normal.Y * (double) this.Normal.Y + (double) this.Normal.Z * (double) this.Normal.Z);
      if ((double) Math.Abs(num1 - 1f) < 1.19209289550781E-07)
        return;
      float num2 = 1f / (float) Math.Sqrt((double) num1);
      this.Normal.X *= num2;
      this.Normal.Y *= num2;
      this.Normal.Z *= num2;
      this.D *= num2;
    }

    public static Plane Normalize(Plane value)
    {
      float num1 = (float) ((double) value.Normal.X * (double) value.Normal.X + (double) value.Normal.Y * (double) value.Normal.Y + (double) value.Normal.Z * (double) value.Normal.Z);
      if ((double) Math.Abs(num1 - 1f) < 1.19209289550781E-07)
      {
        Plane plane;
        plane.Normal = value.Normal;
        plane.D = value.D;
        return plane;
      }
      float num2 = 1f / (float) Math.Sqrt((double) num1);
      Plane plane1;
      plane1.Normal.X = value.Normal.X * num2;
      plane1.Normal.Y = value.Normal.Y * num2;
      plane1.Normal.Z = value.Normal.Z * num2;
      plane1.D = value.D * num2;
      return plane1;
    }

    public static void Normalize(ref Plane value, out Plane result)
    {
      float num1 = (float) ((double) value.Normal.X * (double) value.Normal.X + (double) value.Normal.Y * (double) value.Normal.Y + (double) value.Normal.Z * (double) value.Normal.Z);
      if ((double) Math.Abs(num1 - 1f) < 1.19209289550781E-07)
      {
        result.Normal = value.Normal;
        result.D = value.D;
      }
      else
      {
        float num2 = 1f / (float) Math.Sqrt((double) num1);
        result.Normal.X = value.Normal.X * num2;
        result.Normal.Y = value.Normal.Y * num2;
        result.Normal.Z = value.Normal.Z * num2;
        result.D = value.D * num2;
      }
    }

    public static Plane Transform(Plane plane, Matrix matrix)
    {
      Plane result;
      Plane.Transform(ref plane, ref matrix, out result);
      return result;
    }

    public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
    {
      result = new Plane();
      Vector3 result1 = -plane.Normal * plane.D;
      Vector3.TransformNormal(ref plane.Normal, ref matrix, out result.Normal);
      Vector3.Transform(ref result1, ref matrix, out result1);
      Vector3.Dot(ref result1, ref result.Normal, out result.D);
      result.D = -result.D;
    }

    public float Dot(Vector4 value) => (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z + (double) this.D * (double) value.W);

    public void Dot(ref Vector4 value, out float result) => result = (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z + (double) this.D * (double) value.W);

    public float DotCoordinate(Vector3 value) => (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z) + this.D;

    public void DotCoordinate(ref Vector3 value, out float result) => result = (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z) + this.D;

    public float DotNormal(Vector3 value) => (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z);

    public void DotNormal(ref Vector3 value, out float result) => result = (float) ((double) this.Normal.X * (double) value.X + (double) this.Normal.Y * (double) value.Y + (double) this.Normal.Z * (double) value.Z);

    public PlaneIntersectionType Intersects(BoundingBox box)
    {
      Vector3 vector3_1;
      vector3_1.X = (double) this.Normal.X >= 0.0 ? box.Min.X : box.Max.X;
      vector3_1.Y = (double) this.Normal.Y >= 0.0 ? box.Min.Y : box.Max.Y;
      vector3_1.Z = (double) this.Normal.Z >= 0.0 ? box.Min.Z : box.Max.Z;
      Vector3 vector3_2;
      vector3_2.X = (double) this.Normal.X >= 0.0 ? box.Max.X : box.Min.X;
      vector3_2.Y = (double) this.Normal.Y >= 0.0 ? box.Max.Y : box.Min.Y;
      vector3_2.Z = (double) this.Normal.Z >= 0.0 ? box.Max.Z : box.Min.Z;
      if ((double) this.Normal.X * (double) vector3_1.X + (double) this.Normal.Y * (double) vector3_1.Y + (double) this.Normal.Z * (double) vector3_1.Z + (double) this.D > 0.0)
        return PlaneIntersectionType.Front;
      return (double) this.Normal.X * (double) vector3_2.X + (double) this.Normal.Y * (double) vector3_2.Y + (double) this.Normal.Z * (double) vector3_2.Z + (double) this.D >= 0.0 ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref BoundingBox box, out PlaneIntersectionType result)
    {
      Vector3 vector3_1;
      vector3_1.X = (double) this.Normal.X >= 0.0 ? box.Min.X : box.Max.X;
      vector3_1.Y = (double) this.Normal.Y >= 0.0 ? box.Min.Y : box.Max.Y;
      vector3_1.Z = (double) this.Normal.Z >= 0.0 ? box.Min.Z : box.Max.Z;
      Vector3 vector3_2;
      vector3_2.X = (double) this.Normal.X >= 0.0 ? box.Max.X : box.Min.X;
      vector3_2.Y = (double) this.Normal.Y >= 0.0 ? box.Max.Y : box.Min.Y;
      vector3_2.Z = (double) this.Normal.Z >= 0.0 ? box.Max.Z : box.Min.Z;
      if ((double) this.Normal.X * (double) vector3_1.X + (double) this.Normal.Y * (double) vector3_1.Y + (double) this.Normal.Z * (double) vector3_1.Z + (double) this.D > 0.0)
        result = PlaneIntersectionType.Front;
      else if ((double) this.Normal.X * (double) vector3_2.X + (double) this.Normal.Y * (double) vector3_2.Y + (double) this.Normal.Z * (double) vector3_2.Z + (double) this.D < 0.0)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public PlaneIntersectionType Intersects(BoundingFrustum frustum) => frustum.Intersects(this);

    public PlaneIntersectionType Intersects(BoundingSphere sphere)
    {
      float num = (float) ((double) sphere.Center.X * (double) this.Normal.X + (double) sphere.Center.Y * (double) this.Normal.Y + (double) sphere.Center.Z * (double) this.Normal.Z) + this.D;
      if ((double) num > (double) sphere.Radius)
        return PlaneIntersectionType.Front;
      return (double) num >= -(double) sphere.Radius ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
    {
      float num = (float) ((double) sphere.Center.X * (double) this.Normal.X + (double) sphere.Center.Y * (double) this.Normal.Y + (double) sphere.Center.Z * (double) this.Normal.Z) + this.D;
      if ((double) num > (double) sphere.Radius)
        result = PlaneIntersectionType.Front;
      else if ((double) num < -(double) sphere.Radius)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public Vector3 RandomPoint()
    {
      if (Plane._random == null)
        Plane._random = new MyRandom();
      Vector3 vector1 = new Vector3();
      Vector3 vector3;
      do
      {
        vector1.X = (float) (2.0 * Plane._random.NextDouble() - 1.0);
        vector1.Y = (float) (2.0 * Plane._random.NextDouble() - 1.0);
        vector1.Z = (float) (2.0 * Plane._random.NextDouble() - 1.0);
        vector3 = Vector3.Cross(vector1, this.Normal);
      }
      while (vector3 == Vector3.Zero);
      double num = (double) vector3.Normalize();
      return vector3 * (float) Math.Sqrt(Plane._random.NextDouble());
    }

    protected class VRageMath_Plane\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<Plane, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Plane owner, in Vector3 value) => owner.Normal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Plane owner, out Vector3 value) => value = owner.Normal;
    }

    protected class VRageMath_Plane\u003C\u003ED\u003C\u003EAccessor : IMemberAccessor<Plane, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Plane owner, in float value) => owner.D = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Plane owner, out float value) => value = owner.D;
    }
  }
}
