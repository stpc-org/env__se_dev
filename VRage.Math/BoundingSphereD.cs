// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingSphereD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct BoundingSphereD : IEquatable<BoundingSphereD>
  {
    public Vector3D Center;
    public double Radius;

    public BoundingSphereD(Vector3D center, double radius)
    {
      this.Center = center;
      this.Radius = radius;
    }

    public static bool operator ==(BoundingSphereD a, BoundingSphereD b) => a.Equals(b);

    public static bool operator !=(BoundingSphereD a, BoundingSphereD b) => a.Center != b.Center || a.Radius != b.Radius;

    public bool Equals(BoundingSphereD other) => this.Center == other.Center && this.Radius == other.Radius;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingSphereD other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Center.GetHashCode() + this.Radius.GetHashCode();

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{Center:{0} Radius:{1}}}", new object[2]
      {
        (object) this.Center.ToString(),
        (object) this.Radius.ToString((IFormatProvider) currentCulture)
      });
    }

    public static BoundingSphereD CreateMerged(
      BoundingSphereD original,
      BoundingSphereD additional)
    {
      Vector3D result;
      Vector3D.Subtract(ref additional.Center, ref original.Center, out result);
      double num1 = result.Length();
      double radius1 = original.Radius;
      double radius2 = additional.Radius;
      if (radius1 + radius2 >= num1)
      {
        if (radius1 - radius2 >= num1)
          return original;
        if (radius2 - radius1 >= num1)
          return additional;
      }
      Vector3D vector3D = result * (1.0 / num1);
      double num2 = MathHelper.Min(-radius1, num1 - radius2);
      double num3 = (MathHelper.Max(radius1, num1 + radius2) - num2) * 0.5;
      BoundingSphereD boundingSphereD;
      boundingSphereD.Center = original.Center + vector3D * (num3 + num2);
      boundingSphereD.Radius = num3;
      return boundingSphereD;
    }

    public static void CreateMerged(
      ref BoundingSphereD original,
      ref BoundingSphereD additional,
      out BoundingSphereD result)
    {
      Vector3D result1;
      Vector3D.Subtract(ref additional.Center, ref original.Center, out result1);
      double num1 = result1.Length();
      double radius1 = original.Radius;
      double radius2 = additional.Radius;
      if (radius1 + radius2 >= num1)
      {
        if (radius1 - radius2 >= num1)
        {
          result = original;
          return;
        }
        if (radius2 - radius1 >= num1)
        {
          result = additional;
          return;
        }
      }
      Vector3D vector3D = result1 * (1.0 / num1);
      double num2 = MathHelper.Min(-radius1, num1 - radius2);
      double num3 = (MathHelper.Max(radius1, num1 + radius2) - num2) * 0.5;
      result.Center = original.Center + vector3D * (num3 + num2);
      result.Radius = num3;
    }

    public static BoundingSphereD CreateFromBoundingBox(BoundingBoxD box)
    {
      BoundingSphereD boundingSphereD;
      Vector3D.Lerp(ref box.Min, ref box.Max, 0.5, out boundingSphereD.Center);
      double result;
      Vector3D.Distance(ref box.Min, ref box.Max, out result);
      boundingSphereD.Radius = result * 0.5;
      return boundingSphereD;
    }

    public static void CreateFromBoundingBox(ref BoundingBoxD box, out BoundingSphereD result)
    {
      Vector3D.Lerp(ref box.Min, ref box.Max, 0.5, out result.Center);
      double result1;
      Vector3D.Distance(ref box.Min, ref box.Max, out result1);
      result.Radius = result1 * 0.5;
    }

    public static BoundingSphereD CreateFromPoints(Vector3D[] points)
    {
      Vector3D point1;
      Vector3D vector3D1 = point1 = points[0];
      Vector3D vector3D2 = point1;
      Vector3D vector3D3 = point1;
      Vector3D vector3D4 = point1;
      Vector3D vector3D5 = point1;
      Vector3D vector3D6 = point1;
      foreach (Vector3D point2 in points)
      {
        if (point2.X < vector3D6.X)
          vector3D6 = point2;
        if (point2.X > vector3D5.X)
          vector3D5 = point2;
        if (point2.Y < vector3D4.Y)
          vector3D4 = point2;
        if (point2.Y > vector3D3.Y)
          vector3D3 = point2;
        if (point2.Z < vector3D2.Z)
          vector3D2 = point2;
        if (point2.Z > vector3D1.Z)
          vector3D1 = point2;
      }
      double result1;
      Vector3D.Distance(ref vector3D5, ref vector3D6, out result1);
      double result2;
      Vector3D.Distance(ref vector3D3, ref vector3D4, out result2);
      double result3;
      Vector3D.Distance(ref vector3D1, ref vector3D2, out result3);
      Vector3D result4;
      double num1;
      if (result1 > result2)
      {
        if (result1 > result3)
        {
          Vector3D.Lerp(ref vector3D5, ref vector3D6, 0.5, out result4);
          num1 = result1 * 0.5;
        }
        else
        {
          Vector3D.Lerp(ref vector3D1, ref vector3D2, 0.5, out result4);
          num1 = result3 * 0.5;
        }
      }
      else if (result2 > result3)
      {
        Vector3D.Lerp(ref vector3D3, ref vector3D4, 0.5, out result4);
        num1 = result2 * 0.5;
      }
      else
      {
        Vector3D.Lerp(ref vector3D1, ref vector3D2, 0.5, out result4);
        num1 = result3 * 0.5;
      }
      foreach (Vector3D point2 in points)
      {
        Vector3D vector3D7;
        vector3D7.X = point2.X - result4.X;
        vector3D7.Y = point2.Y - result4.Y;
        vector3D7.Z = point2.Z - result4.Z;
        double num2 = vector3D7.Length();
        if (num2 > num1)
        {
          num1 = (num1 + num2) * 0.5;
          result4 += (1.0 - num1 / num2) * vector3D7;
        }
      }
      BoundingSphereD boundingSphereD;
      boundingSphereD.Center = result4;
      boundingSphereD.Radius = num1;
      return boundingSphereD;
    }

    public static BoundingSphereD CreateFromFrustum(BoundingFrustumD frustum) => !(frustum == (BoundingFrustumD) null) ? BoundingSphereD.CreateFromPoints(frustum.CornerArray) : throw new ArgumentNullException(nameof (frustum));

    public bool Intersects(BoundingBoxD box)
    {
      Vector3D result1;
      Vector3D.Clamp(ref this.Center, ref box.Min, ref box.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref this.Center, ref result1, out result2);
      return result2 <= this.Radius * this.Radius;
    }

    public void Intersects(ref BoundingBoxD box, out bool result)
    {
      Vector3D result1;
      Vector3D.Clamp(ref this.Center, ref box.Min, ref box.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref this.Center, ref result1, out result2);
      result = result2 <= this.Radius * this.Radius;
    }

    public double? Intersects(RayD ray) => ray.Intersects(this);

    public bool Intersects(BoundingFrustumD frustum)
    {
      bool result;
      frustum.Intersects(ref this, out result);
      return result;
    }

    public bool Intersects(BoundingSphereD sphere)
    {
      double result;
      Vector3D.DistanceSquared(ref this.Center, ref sphere.Center, out result);
      double radius1 = this.Radius;
      double radius2 = sphere.Radius;
      return radius1 * radius1 + 2.0 * radius1 * radius2 + radius2 * radius2 > result;
    }

    public void Intersects(ref BoundingSphereD sphere, out bool result)
    {
      double result1;
      Vector3D.DistanceSquared(ref this.Center, ref sphere.Center, out result1);
      double radius1 = this.Radius;
      double radius2 = sphere.Radius;
      result = radius1 * radius1 + 2.0 * radius1 * radius2 + radius2 * radius2 > result1;
    }

    public ContainmentType Contains(BoundingBoxD box)
    {
      if (!box.Intersects(this))
        return ContainmentType.Disjoint;
      double num = this.Radius * this.Radius;
      Vector3D vector3D;
      vector3D.X = this.Center.X - box.Min.X;
      vector3D.Y = this.Center.Y - box.Max.Y;
      vector3D.Z = this.Center.Z - box.Max.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Max.X;
      vector3D.Y = this.Center.Y - box.Max.Y;
      vector3D.Z = this.Center.Z - box.Max.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Max.X;
      vector3D.Y = this.Center.Y - box.Min.Y;
      vector3D.Z = this.Center.Z - box.Max.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Min.X;
      vector3D.Y = this.Center.Y - box.Min.Y;
      vector3D.Z = this.Center.Z - box.Max.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Min.X;
      vector3D.Y = this.Center.Y - box.Max.Y;
      vector3D.Z = this.Center.Z - box.Min.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Max.X;
      vector3D.Y = this.Center.Y - box.Max.Y;
      vector3D.Z = this.Center.Z - box.Min.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Max.X;
      vector3D.Y = this.Center.Y - box.Min.Y;
      vector3D.Z = this.Center.Z - box.Min.Z;
      if (vector3D.LengthSquared() > num)
        return ContainmentType.Intersects;
      vector3D.X = this.Center.X - box.Min.X;
      vector3D.Y = this.Center.Y - box.Min.Y;
      vector3D.Z = this.Center.Z - box.Min.Z;
      return vector3D.LengthSquared() <= num ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBoxD box, out ContainmentType result)
    {
      bool result1;
      box.Intersects(ref this, out result1);
      if (!result1)
      {
        result = ContainmentType.Disjoint;
      }
      else
      {
        double num = this.Radius * this.Radius;
        result = ContainmentType.Intersects;
        Vector3D vector3D;
        vector3D.X = this.Center.X - box.Min.X;
        vector3D.Y = this.Center.Y - box.Max.Y;
        vector3D.Z = this.Center.Z - box.Max.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Max.X;
        vector3D.Y = this.Center.Y - box.Max.Y;
        vector3D.Z = this.Center.Z - box.Max.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Max.X;
        vector3D.Y = this.Center.Y - box.Min.Y;
        vector3D.Z = this.Center.Z - box.Max.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Min.X;
        vector3D.Y = this.Center.Y - box.Min.Y;
        vector3D.Z = this.Center.Z - box.Max.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Min.X;
        vector3D.Y = this.Center.Y - box.Max.Y;
        vector3D.Z = this.Center.Z - box.Min.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Max.X;
        vector3D.Y = this.Center.Y - box.Max.Y;
        vector3D.Z = this.Center.Z - box.Min.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Max.X;
        vector3D.Y = this.Center.Y - box.Min.Y;
        vector3D.Z = this.Center.Z - box.Min.Z;
        if (vector3D.LengthSquared() > num)
          return;
        vector3D.X = this.Center.X - box.Min.X;
        vector3D.Y = this.Center.Y - box.Min.Y;
        vector3D.Z = this.Center.Z - box.Min.Z;
        if (vector3D.LengthSquared() > num)
          return;
        result = ContainmentType.Contains;
      }
    }

    public ContainmentType Contains(BoundingFrustumD frustum)
    {
      if (!frustum.Intersects(this))
        return ContainmentType.Disjoint;
      double num = this.Radius * this.Radius;
      foreach (Vector3D corner in frustum.CornerArray)
      {
        Vector3D vector3D;
        vector3D.X = corner.X - this.Center.X;
        vector3D.Y = corner.Y - this.Center.Y;
        vector3D.Z = corner.Z - this.Center.Z;
        if (vector3D.LengthSquared() > num)
          return ContainmentType.Intersects;
      }
      return ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3D point) => Vector3D.DistanceSquared(point, this.Center) < this.Radius * this.Radius ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector3D point, out ContainmentType result)
    {
      double result1;
      Vector3D.DistanceSquared(ref point, ref this.Center, out result1);
      result = result1 < this.Radius * this.Radius ? ContainmentType.Contains : ContainmentType.Disjoint;
    }

    public ContainmentType Contains(BoundingSphereD sphere)
    {
      double result;
      Vector3D.Distance(ref this.Center, ref sphere.Center, out result);
      double radius1 = this.Radius;
      double radius2 = sphere.Radius;
      if (radius1 + radius2 < result)
        return ContainmentType.Disjoint;
      return radius1 - radius2 >= result ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
    {
      double result1;
      Vector3D.Distance(ref this.Center, ref sphere.Center, out result1);
      double radius1 = this.Radius;
      double radius2 = sphere.Radius;
      result = radius1 + radius2 >= result1 ? (radius1 - radius2 >= result1 ? ContainmentType.Contains : ContainmentType.Intersects) : ContainmentType.Disjoint;
    }

    internal void SupportMapping(ref Vector3D v, out Vector3D result)
    {
      double num = this.Radius / v.Length();
      result.X = this.Center.X + v.X * num;
      result.Y = this.Center.Y + v.Y * num;
      result.Z = this.Center.Z + v.Z * num;
    }

    public BoundingSphereD Transform(MatrixD matrix)
    {
      BoundingSphereD boundingSphereD = new BoundingSphereD();
      boundingSphereD.Center = Vector3D.Transform(this.Center, matrix);
      double d = Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33));
      boundingSphereD.Radius = this.Radius * Math.Sqrt(d);
      return boundingSphereD;
    }

    public void Transform(ref MatrixD matrix, out BoundingSphereD result)
    {
      result.Center = Vector3D.Transform(this.Center, matrix);
      double d = Math.Max(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13, Math.Max(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23, matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33));
      result.Radius = this.Radius * Math.Sqrt(d);
    }

    public bool IntersectRaySphere(RayD ray, out double tmin, out double tmax)
    {
      tmin = 0.0;
      tmax = 0.0;
      Vector3D v = ray.Position - this.Center;
      double num1 = ray.Direction.Dot(ray.Direction);
      double num2 = 2.0 * v.Dot(ray.Direction);
      double num3 = v.Dot(v) - this.Radius * this.Radius;
      double d = num2 * num2 - 4.0 * num1 * num3;
      if (d < 0.0)
        return false;
      tmin = (-num2 - Math.Sqrt(d)) / (2.0 * num1);
      tmax = (-num2 + Math.Sqrt(d)) / (2.0 * num1);
      if (tmin > tmax)
      {
        double num4 = tmin;
        tmin = tmax;
        tmax = num4;
      }
      return true;
    }

    public BoundingSphereD Include(BoundingSphereD sphere)
    {
      BoundingSphereD.Include(ref this, ref sphere);
      return this;
    }

    public static void Include(ref BoundingSphereD sphere, ref BoundingSphereD otherSphere)
    {
      if (sphere.Radius == double.MinValue)
      {
        sphere.Center = otherSphere.Center;
        sphere.Radius = otherSphere.Radius;
      }
      else
      {
        double num1 = Vector3D.Distance(sphere.Center, otherSphere.Center);
        if (num1 + otherSphere.Radius <= sphere.Radius)
          return;
        if (num1 + sphere.Radius <= otherSphere.Radius)
        {
          sphere = otherSphere;
        }
        else
        {
          double amount = (num1 + otherSphere.Radius - sphere.Radius) / (2.0 * num1);
          Vector3D vector3D = Vector3D.Lerp(sphere.Center, otherSphere.Center, amount);
          double num2 = (num1 + sphere.Radius + otherSphere.Radius) / 2.0;
          sphere.Center = vector3D;
          sphere.Radius = num2;
        }
      }
    }

    public static BoundingSphereD CreateInvalid() => new BoundingSphereD(Vector3D.Zero, double.MinValue);

    public static implicit operator BoundingSphereD(BoundingSphere b) => new BoundingSphereD((Vector3D) b.Center, (double) b.Radius);

    public static implicit operator BoundingSphere(BoundingSphereD b) => new BoundingSphere((Vector3) b.Center, (float) b.Radius);

    public Vector3D RandomToUniformPointInSphere(double ranX, double ranY, double ranZ)
    {
      double num1 = 2.0 * Math.PI * ranX;
      double num2 = Math.Acos(2.0 * ranY - 1.0);
      double num3 = this.Radius * Math.Pow(ranZ, 1.0 / 3.0);
      return new Vector3D(num3 * Math.Cos(num1) * Math.Sin(num2), num3 * Math.Sin(num1) * Math.Sin(num2), num3 * Math.Cos(num2)) + this.Center;
    }

    public Vector3D? RandomToUniformPointInSphereWithInnerCutout(
      double ranX,
      double ranY,
      double ranZ,
      double cutoutRadius)
    {
      if (cutoutRadius < 0.0 || cutoutRadius >= this.Radius || this.Radius <= 0.0)
        return new Vector3D?();
      double num1 = cutoutRadius / this.Radius;
      double num2 = (this.Radius - cutoutRadius) / this.Radius;
      double ranZ1 = ranZ * num2 + num1;
      return new Vector3D?(this.RandomToUniformPointInSphere(ranX, ranY, ranZ1));
    }

    public Vector3D RandomToUniformPointOnSphere(double ranX, double ranY)
    {
      double num1 = 2.0 * Math.PI * ranX;
      double num2 = Math.Acos(2.0 * ranY - 1.0);
      double radius = this.Radius;
      return new Vector3D(radius * Math.Cos(num1) * Math.Sin(num2), radius * Math.Sin(num1) * Math.Sin(num2), radius * Math.Cos(num2)) + this.Center;
    }

    public BoundingBoxD GetBoundingBox() => new BoundingBoxD(this.Center - this.Radius, this.Center + this.Radius);

    protected class VRageMath_BoundingSphereD\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<BoundingSphereD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingSphereD owner, in Vector3D value) => owner.Center = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingSphereD owner, out Vector3D value) => value = owner.Center;
    }

    protected class VRageMath_BoundingSphereD\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<BoundingSphereD, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingSphereD owner, in double value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingSphereD owner, out double value) => value = owner.Radius;
    }
  }
}
