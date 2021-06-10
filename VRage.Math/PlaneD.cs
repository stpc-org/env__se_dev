// Decompiled with JetBrains decompiler
// Type: VRageMath.PlaneD
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
  public struct PlaneD : IEquatable<PlaneD>
  {
    public Vector3D Normal;
    public double D;
    private static MyRandom _random;

    public PlaneD(double a, double b, double c, double d)
    {
      this.Normal.X = a;
      this.Normal.Y = b;
      this.Normal.Z = c;
      this.D = d;
    }

    public PlaneD(Vector3D normal, double d)
    {
      this.Normal = normal;
      this.D = d;
    }

    public PlaneD(Vector3D position, Vector3D normal)
    {
      this.Normal = normal;
      this.D = -Vector3D.Dot(position, normal);
    }

    public PlaneD(Vector3D position, Vector3 normal)
    {
      this.Normal = (Vector3D) normal;
      this.D = -Vector3D.Dot(position, normal);
    }

    public PlaneD(Vector4 value)
    {
      this.Normal.X = (double) value.X;
      this.Normal.Y = (double) value.Y;
      this.Normal.Z = (double) value.Z;
      this.D = (double) value.W;
    }

    public PlaneD(Vector3D point1, Vector3D point2, Vector3D point3)
    {
      double num1 = point2.X - point1.X;
      double num2 = point2.Y - point1.Y;
      double num3 = point2.Z - point1.Z;
      double num4 = point3.X - point1.X;
      double num5 = point3.Y - point1.Y;
      double num6 = point3.Z - point1.Z;
      double num7 = num2 * num6 - num3 * num5;
      double num8 = num3 * num4 - num1 * num6;
      double num9 = num1 * num5 - num2 * num4;
      double num10 = 1.0 / Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
      this.Normal.X = num7 * num10;
      this.Normal.Y = num8 * num10;
      this.Normal.Z = num9 * num10;
      this.D = -(this.Normal.X * point1.X + this.Normal.Y * point1.Y + this.Normal.Z * point1.Z);
    }

    public static bool operator ==(PlaneD lhs, PlaneD rhs) => lhs.Equals(rhs);

    public static bool operator !=(PlaneD lhs, PlaneD rhs) => lhs.Normal.X != rhs.Normal.X || lhs.Normal.Y != rhs.Normal.Y || lhs.Normal.Z != rhs.Normal.Z || lhs.D != rhs.D;

    public bool Equals(PlaneD other) => this.Normal.X == other.Normal.X && this.Normal.Y == other.Normal.Y && this.Normal.Z == other.Normal.Z && this.D == other.D;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is PlaneD other)
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
      double d = this.Normal.X * this.Normal.X + this.Normal.Y * this.Normal.Y + this.Normal.Z * this.Normal.Z;
      if (Math.Abs(d - 1.0) < 1.19209289550781E-07)
        return;
      double num = 1.0 / Math.Sqrt(d);
      this.Normal.X *= num;
      this.Normal.Y *= num;
      this.Normal.Z *= num;
      this.D *= num;
    }

    public static PlaneD Normalize(PlaneD value)
    {
      double d = value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z;
      if (Math.Abs(d - 1.0) < 1.19209289550781E-07)
      {
        PlaneD planeD;
        planeD.Normal = value.Normal;
        planeD.D = value.D;
        return planeD;
      }
      double num = 1.0 / Math.Sqrt(d);
      PlaneD planeD1;
      planeD1.Normal.X = value.Normal.X * num;
      planeD1.Normal.Y = value.Normal.Y * num;
      planeD1.Normal.Z = value.Normal.Z * num;
      planeD1.D = value.D * num;
      return planeD1;
    }

    public static void Normalize(ref PlaneD value, out PlaneD result)
    {
      double d = value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z;
      if (Math.Abs(d - 1.0) < 1.19209289550781E-07)
      {
        result.Normal = value.Normal;
        result.D = value.D;
      }
      else
      {
        double num = 1.0 / Math.Sqrt(d);
        result.Normal.X = value.Normal.X * num;
        result.Normal.Y = value.Normal.Y * num;
        result.Normal.Z = value.Normal.Z * num;
        result.D = value.D * num;
      }
    }

    public static PlaneD Transform(PlaneD plane, MatrixD matrix)
    {
      PlaneD result;
      PlaneD.Transform(ref plane, ref matrix, out result);
      return result;
    }

    public static void Transform(ref PlaneD plane, ref MatrixD matrix, out PlaneD result)
    {
      result = new PlaneD();
      Vector3D result1 = -plane.Normal * plane.D;
      Vector3D.TransformNormal(ref plane.Normal, ref matrix, out result.Normal);
      Vector3D.Transform(ref result1, ref matrix, out result1);
      Vector3D.Dot(ref result1, ref result.Normal, out result.D);
      result.D = -result.D;
    }

    public double Dot(Vector4 value) => this.Normal.X * (double) value.X + this.Normal.Y * (double) value.Y + this.Normal.Z * (double) value.Z + this.D * (double) value.W;

    public void Dot(ref Vector4 value, out double result) => result = this.Normal.X * (double) value.X + this.Normal.Y * (double) value.Y + this.Normal.Z * (double) value.Z + this.D * (double) value.W;

    public double DotCoordinate(Vector3D value) => this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D;

    public void DotCoordinate(ref Vector3D value, out double result) => result = this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z + this.D;

    public double DotNormal(Vector3D value) => this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z;

    public void DotNormal(ref Vector3D value, out double result) => result = this.Normal.X * value.X + this.Normal.Y * value.Y + this.Normal.Z * value.Z;

    public PlaneIntersectionType Intersects(BoundingBoxD box)
    {
      Vector3D vector3D1;
      vector3D1.X = this.Normal.X >= 0.0 ? box.Min.X : box.Max.X;
      vector3D1.Y = this.Normal.Y >= 0.0 ? box.Min.Y : box.Max.Y;
      vector3D1.Z = this.Normal.Z >= 0.0 ? box.Min.Z : box.Max.Z;
      Vector3D vector3D2;
      vector3D2.X = this.Normal.X >= 0.0 ? box.Max.X : box.Min.X;
      vector3D2.Y = this.Normal.Y >= 0.0 ? box.Max.Y : box.Min.Y;
      vector3D2.Z = this.Normal.Z >= 0.0 ? box.Max.Z : box.Min.Z;
      if (this.Normal.X * vector3D1.X + this.Normal.Y * vector3D1.Y + this.Normal.Z * vector3D1.Z + this.D > 0.0)
        return PlaneIntersectionType.Front;
      return this.Normal.X * vector3D2.X + this.Normal.Y * vector3D2.Y + this.Normal.Z * vector3D2.Z + this.D >= 0.0 ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref BoundingBoxD box, out PlaneIntersectionType result)
    {
      Vector3D vector3D1;
      vector3D1.X = this.Normal.X >= 0.0 ? box.Min.X : box.Max.X;
      vector3D1.Y = this.Normal.Y >= 0.0 ? box.Min.Y : box.Max.Y;
      vector3D1.Z = this.Normal.Z >= 0.0 ? box.Min.Z : box.Max.Z;
      Vector3D vector3D2;
      vector3D2.X = this.Normal.X >= 0.0 ? box.Max.X : box.Min.X;
      vector3D2.Y = this.Normal.Y >= 0.0 ? box.Max.Y : box.Min.Y;
      vector3D2.Z = this.Normal.Z >= 0.0 ? box.Max.Z : box.Min.Z;
      if (this.Normal.X * vector3D1.X + this.Normal.Y * vector3D1.Y + this.Normal.Z * vector3D1.Z + this.D > 0.0)
        result = PlaneIntersectionType.Front;
      else if (this.Normal.X * vector3D2.X + this.Normal.Y * vector3D2.Y + this.Normal.Z * vector3D2.Z + this.D < 0.0)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public PlaneIntersectionType Intersects(BoundingFrustumD frustum) => frustum.Intersects(this);

    public PlaneIntersectionType Intersects(BoundingSphereD sphere)
    {
      double num = sphere.Center.X * this.Normal.X + sphere.Center.Y * this.Normal.Y + sphere.Center.Z * this.Normal.Z + this.D;
      if (num > sphere.Radius)
        return PlaneIntersectionType.Front;
      return num >= -sphere.Radius ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref BoundingSphere sphere, out PlaneIntersectionType result)
    {
      double num = (double) sphere.Center.X * this.Normal.X + (double) sphere.Center.Y * this.Normal.Y + (double) sphere.Center.Z * this.Normal.Z + this.D;
      if (num > (double) sphere.Radius)
        result = PlaneIntersectionType.Front;
      else if (num < -(double) sphere.Radius)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public Vector3D RandomPoint()
    {
      if (PlaneD._random == null)
        PlaneD._random = new MyRandom();
      Vector3D vector1 = new Vector3D();
      Vector3D vector3D;
      do
      {
        vector1.X = 2.0 * PlaneD._random.NextDouble() - 1.0;
        vector1.Y = 2.0 * PlaneD._random.NextDouble() - 1.0;
        vector1.Z = 2.0 * PlaneD._random.NextDouble() - 1.0;
        vector3D = Vector3D.Cross(vector1, this.Normal);
      }
      while (vector3D == Vector3D.Zero);
      vector3D.Normalize();
      return vector3D * Math.Sqrt(PlaneD._random.NextDouble());
    }

    public double DistanceToPoint(Vector3D point) => Vector3D.Dot(this.Normal, point) + this.D;

    public double DistanceToPoint(ref Vector3D point) => Vector3D.Dot(this.Normal, point) + this.D;

    public Vector3D ProjectPoint(ref Vector3D point) => point - this.Normal * this.DistanceToPoint(ref point);

    public Vector3D Intersection(ref Vector3D from, ref Vector3D direction)
    {
      double num = -(this.DotNormal(from) + this.D) / this.DotNormal(direction);
      return from + num * direction;
    }

    protected class VRageMath_PlaneD\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<PlaneD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlaneD owner, in Vector3D value) => owner.Normal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlaneD owner, out Vector3D value) => value = owner.Normal;
    }

    protected class VRageMath_PlaneD\u003C\u003ED\u003C\u003EAccessor : IMemberAccessor<PlaneD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlaneD owner, in double value) => owner.D = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlaneD owner, out double value) => value = owner.D;
    }
  }
}
