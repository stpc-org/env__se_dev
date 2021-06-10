// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingSphere
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct BoundingSphere : IEquatable<BoundingSphere>
  {
    public Vector3 Center;
    public float Radius;

    public BoundingSphere(Vector3 center, float radius)
    {
      this.Center = center;
      this.Radius = radius;
    }

    public static bool operator ==(BoundingSphere a, BoundingSphere b) => a.Equals(b);

    public static bool operator !=(BoundingSphere a, BoundingSphere b) => a.Center != b.Center || (double) a.Radius != (double) b.Radius;

    public bool Equals(BoundingSphere other) => this.Center == other.Center && (double) this.Radius == (double) other.Radius;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingSphere other)
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

    public static BoundingSphere CreateMerged(
      BoundingSphere original,
      BoundingSphere additional)
    {
      Vector3 result;
      Vector3.Subtract(ref additional.Center, ref original.Center, out result);
      float num1 = result.Length();
      float radius1 = original.Radius;
      float radius2 = additional.Radius;
      if ((double) radius1 + (double) radius2 >= (double) num1)
      {
        if ((double) radius1 - (double) radius2 >= (double) num1)
          return original;
        if ((double) radius2 - (double) radius1 >= (double) num1)
          return additional;
      }
      Vector3 vector3 = result * (1f / num1);
      float num2 = MathHelper.Min(-radius1, num1 - radius2);
      float num3 = (float) (((double) MathHelper.Max(radius1, num1 + radius2) - (double) num2) * 0.5);
      BoundingSphere boundingSphere;
      boundingSphere.Center = original.Center + vector3 * (num3 + num2);
      boundingSphere.Radius = num3;
      return boundingSphere;
    }

    public static void CreateMerged(
      ref BoundingSphere original,
      ref BoundingSphere additional,
      out BoundingSphere result)
    {
      Vector3 result1;
      Vector3.Subtract(ref additional.Center, ref original.Center, out result1);
      float num1 = result1.Length();
      float radius1 = original.Radius;
      float radius2 = additional.Radius;
      if ((double) radius1 + (double) radius2 >= (double) num1)
      {
        if ((double) radius1 - (double) radius2 >= (double) num1)
        {
          result = original;
          return;
        }
        if ((double) radius2 - (double) radius1 >= (double) num1)
        {
          result = additional;
          return;
        }
      }
      Vector3 vector3 = result1 * (1f / num1);
      float num2 = MathHelper.Min(-radius1, num1 - radius2);
      float num3 = (float) (((double) MathHelper.Max(radius1, num1 + radius2) - (double) num2) * 0.5);
      result.Center = original.Center + vector3 * (num3 + num2);
      result.Radius = num3;
    }

    public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
    {
      BoundingSphere boundingSphere;
      boundingSphere.Center = (box.Min + box.Max) * 0.5f;
      Vector3.Distance(ref boundingSphere.Center, ref box.Max, out boundingSphere.Radius);
      return boundingSphere;
    }

    public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
    {
      result.Center = (box.Min + box.Max) * 0.5f;
      Vector3.Distance(ref result.Center, ref box.Max, out result.Radius);
    }

    public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
    {
      IEnumerator<Vector3> enumerator = points.GetEnumerator();
      enumerator.MoveNext();
      Vector3 current;
      Vector3 vector3_1 = current = enumerator.Current;
      Vector3 vector3_2 = current;
      Vector3 vector3_3 = current;
      Vector3 vector3_4 = current;
      Vector3 vector3_5 = current;
      Vector3 vector3_6 = current;
      foreach (Vector3 point in points)
      {
        if ((double) point.X < (double) vector3_6.X)
          vector3_6 = point;
        if ((double) point.X > (double) vector3_5.X)
          vector3_5 = point;
        if ((double) point.Y < (double) vector3_4.Y)
          vector3_4 = point;
        if ((double) point.Y > (double) vector3_3.Y)
          vector3_3 = point;
        if ((double) point.Z < (double) vector3_2.Z)
          vector3_2 = point;
        if ((double) point.Z > (double) vector3_1.Z)
          vector3_1 = point;
      }
      float result1;
      Vector3.Distance(ref vector3_5, ref vector3_6, out result1);
      float result2;
      Vector3.Distance(ref vector3_3, ref vector3_4, out result2);
      float result3;
      Vector3.Distance(ref vector3_1, ref vector3_2, out result3);
      Vector3 result4;
      float num1;
      if ((double) result1 > (double) result2)
      {
        if ((double) result1 > (double) result3)
        {
          Vector3.Lerp(ref vector3_5, ref vector3_6, 0.5f, out result4);
          num1 = result1 * 0.5f;
        }
        else
        {
          Vector3.Lerp(ref vector3_1, ref vector3_2, 0.5f, out result4);
          num1 = result3 * 0.5f;
        }
      }
      else if ((double) result2 > (double) result3)
      {
        Vector3.Lerp(ref vector3_3, ref vector3_4, 0.5f, out result4);
        num1 = result2 * 0.5f;
      }
      else
      {
        Vector3.Lerp(ref vector3_1, ref vector3_2, 0.5f, out result4);
        num1 = result3 * 0.5f;
      }
      foreach (Vector3 point in points)
      {
        Vector3 vector3_7;
        vector3_7.X = point.X - result4.X;
        vector3_7.Y = point.Y - result4.Y;
        vector3_7.Z = point.Z - result4.Z;
        float num2 = vector3_7.Length();
        if ((double) num2 > (double) num1)
        {
          num1 = (float) (((double) num1 + (double) num2) * 0.5);
          result4 += (float) (1.0 - (double) num1 / (double) num2) * vector3_7;
        }
      }
      BoundingSphere boundingSphere;
      boundingSphere.Center = result4;
      boundingSphere.Radius = num1;
      return boundingSphere;
    }

    public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum) => !(frustum == (BoundingFrustum) null) ? BoundingSphere.CreateFromPoints((IEnumerable<Vector3>) frustum.cornerArray) : throw new ArgumentNullException(nameof (frustum));

    public bool Intersects(BoundingBox box)
    {
      Vector3 result1;
      Vector3.Clamp(ref this.Center, ref box.Min, ref box.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref this.Center, ref result1, out result2);
      return (double) result2 <= (double) this.Radius * (double) this.Radius;
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      Vector3 result1;
      Vector3.Clamp(ref this.Center, ref box.Min, ref box.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref this.Center, ref result1, out result2);
      result = (double) result2 <= (double) this.Radius * (double) this.Radius;
    }

    public bool Intersects(BoundingFrustum frustum)
    {
      bool result;
      frustum.Intersects(ref this, out result);
      return result;
    }

    public PlaneIntersectionType Intersects(Plane plane) => plane.Intersects(this);

    public void Intersects(ref Plane plane, out PlaneIntersectionType result) => plane.Intersects(ref this, out result);

    public float? Intersects(Ray ray) => ray.Intersects(this);

    public void Intersects(ref Ray ray, out float? result) => ray.Intersects(ref this, out result);

    public bool Intersects(BoundingSphere sphere)
    {
      float result;
      Vector3.DistanceSquared(ref this.Center, ref sphere.Center, out result);
      float radius1 = this.Radius;
      float radius2 = sphere.Radius;
      return (double) radius1 * (double) radius1 + 2.0 * (double) radius1 * (double) radius2 + (double) radius2 * (double) radius2 > (double) result;
    }

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      float result1;
      Vector3.DistanceSquared(ref this.Center, ref sphere.Center, out result1);
      float radius1 = this.Radius;
      float radius2 = sphere.Radius;
      result = (double) radius1 * (double) radius1 + 2.0 * (double) radius1 * (double) radius2 + (double) radius2 * (double) radius2 > (double) result1;
    }

    public ContainmentType Contains(BoundingBox box)
    {
      if (!box.Intersects(this))
        return ContainmentType.Disjoint;
      float num = this.Radius * this.Radius;
      Vector3 vector3;
      vector3.X = this.Center.X - box.Min.X;
      vector3.Y = this.Center.Y - box.Max.Y;
      vector3.Z = this.Center.Z - box.Max.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Max.X;
      vector3.Y = this.Center.Y - box.Max.Y;
      vector3.Z = this.Center.Z - box.Max.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Max.X;
      vector3.Y = this.Center.Y - box.Min.Y;
      vector3.Z = this.Center.Z - box.Max.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Min.X;
      vector3.Y = this.Center.Y - box.Min.Y;
      vector3.Z = this.Center.Z - box.Max.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Min.X;
      vector3.Y = this.Center.Y - box.Max.Y;
      vector3.Z = this.Center.Z - box.Min.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Max.X;
      vector3.Y = this.Center.Y - box.Max.Y;
      vector3.Z = this.Center.Z - box.Min.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Max.X;
      vector3.Y = this.Center.Y - box.Min.Y;
      vector3.Z = this.Center.Z - box.Min.Z;
      if ((double) vector3.LengthSquared() > (double) num)
        return ContainmentType.Intersects;
      vector3.X = this.Center.X - box.Min.X;
      vector3.Y = this.Center.Y - box.Min.Y;
      vector3.Z = this.Center.Z - box.Min.Z;
      return (double) vector3.LengthSquared() <= (double) num ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      bool result1;
      box.Intersects(ref this, out result1);
      if (!result1)
      {
        result = ContainmentType.Disjoint;
      }
      else
      {
        float num = this.Radius * this.Radius;
        result = ContainmentType.Intersects;
        Vector3 vector3;
        vector3.X = this.Center.X - box.Min.X;
        vector3.Y = this.Center.Y - box.Max.Y;
        vector3.Z = this.Center.Z - box.Max.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Max.X;
        vector3.Y = this.Center.Y - box.Max.Y;
        vector3.Z = this.Center.Z - box.Max.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Max.X;
        vector3.Y = this.Center.Y - box.Min.Y;
        vector3.Z = this.Center.Z - box.Max.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Min.X;
        vector3.Y = this.Center.Y - box.Min.Y;
        vector3.Z = this.Center.Z - box.Max.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Min.X;
        vector3.Y = this.Center.Y - box.Max.Y;
        vector3.Z = this.Center.Z - box.Min.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Max.X;
        vector3.Y = this.Center.Y - box.Max.Y;
        vector3.Z = this.Center.Z - box.Min.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Max.X;
        vector3.Y = this.Center.Y - box.Min.Y;
        vector3.Z = this.Center.Z - box.Min.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        vector3.X = this.Center.X - box.Min.X;
        vector3.Y = this.Center.Y - box.Min.Y;
        vector3.Z = this.Center.Z - box.Min.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return;
        result = ContainmentType.Contains;
      }
    }

    public ContainmentType Contains(BoundingFrustum frustum)
    {
      if (!frustum.Intersects(this))
        return ContainmentType.Disjoint;
      float num = this.Radius * this.Radius;
      foreach (Vector3 corner in frustum.cornerArray)
      {
        Vector3 vector3;
        vector3.X = corner.X - this.Center.X;
        vector3.Y = corner.Y - this.Center.Y;
        vector3.Z = corner.Z - this.Center.Z;
        if ((double) vector3.LengthSquared() > (double) num)
          return ContainmentType.Intersects;
      }
      return ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3 point) => (double) Vector3.DistanceSquared(point, this.Center) < (double) this.Radius * (double) this.Radius ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector3 point, out ContainmentType result)
    {
      float result1;
      Vector3.DistanceSquared(ref point, ref this.Center, out result1);
      result = (double) result1 < (double) this.Radius * (double) this.Radius ? ContainmentType.Contains : ContainmentType.Disjoint;
    }

    public ContainmentType Contains(BoundingSphere sphere)
    {
      float result;
      Vector3.Distance(ref this.Center, ref sphere.Center, out result);
      float radius1 = this.Radius;
      float radius2 = sphere.Radius;
      if ((double) radius1 + (double) radius2 < (double) result)
        return ContainmentType.Disjoint;
      return (double) radius1 - (double) radius2 >= (double) result ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      float result1;
      Vector3.Distance(ref this.Center, ref sphere.Center, out result1);
      float radius1 = this.Radius;
      float radius2 = sphere.Radius;
      result = (double) radius1 + (double) radius2 >= (double) result1 ? ((double) radius1 - (double) radius2 >= (double) result1 ? ContainmentType.Contains : ContainmentType.Intersects) : ContainmentType.Disjoint;
    }

    internal void SupportMapping(ref Vector3 v, out Vector3 result)
    {
      float num = this.Radius / v.Length();
      result.X = this.Center.X + v.X * num;
      result.Y = this.Center.Y + v.Y * num;
      result.Z = this.Center.Z + v.Z * num;
    }

    public BoundingSphere Transform(Matrix matrix)
    {
      BoundingSphere boundingSphere = new BoundingSphere();
      boundingSphere.Center = Vector3.Transform(this.Center, matrix);
      float num = Math.Max((float) ((double) matrix.M11 * (double) matrix.M11 + (double) matrix.M12 * (double) matrix.M12 + (double) matrix.M13 * (double) matrix.M13), Math.Max((float) ((double) matrix.M21 * (double) matrix.M21 + (double) matrix.M22 * (double) matrix.M22 + (double) matrix.M23 * (double) matrix.M23), (float) ((double) matrix.M31 * (double) matrix.M31 + (double) matrix.M32 * (double) matrix.M32 + (double) matrix.M33 * (double) matrix.M33)));
      boundingSphere.Radius = this.Radius * (float) Math.Sqrt((double) num);
      return boundingSphere;
    }

    public void Transform(ref Matrix matrix, out BoundingSphere result)
    {
      result.Center = Vector3.Transform(this.Center, matrix);
      float num = Math.Max((float) ((double) matrix.M11 * (double) matrix.M11 + (double) matrix.M12 * (double) matrix.M12 + (double) matrix.M13 * (double) matrix.M13), Math.Max((float) ((double) matrix.M21 * (double) matrix.M21 + (double) matrix.M22 * (double) matrix.M22 + (double) matrix.M23 * (double) matrix.M23), (float) ((double) matrix.M31 * (double) matrix.M31 + (double) matrix.M32 * (double) matrix.M32 + (double) matrix.M33 * (double) matrix.M33)));
      result.Radius = this.Radius * (float) Math.Sqrt((double) num);
    }

    public BoundingSphere Translate(ref Vector3 translation) => new BoundingSphere(this.Center + translation, this.Radius);

    public bool IntersectRaySphere(Ray ray, out float tmin, out float tmax)
    {
      tmin = 0.0f;
      tmax = 0.0f;
      Vector3 v = ray.Position - this.Center;
      float num1 = ray.Direction.Dot(ray.Direction);
      float num2 = 2f * v.Dot(ray.Direction);
      float num3 = v.Dot(v) - this.Radius * this.Radius;
      float num4 = (float) ((double) num2 * (double) num2 - 4.0 * (double) num1 * (double) num3);
      if ((double) num4 < 0.0)
        return false;
      tmin = (float) ((-(double) num2 - Math.Sqrt((double) num4)) / (2.0 * (double) num1));
      tmax = (float) ((-(double) num2 + Math.Sqrt((double) num4)) / (2.0 * (double) num1));
      if ((double) tmin > (double) tmax)
      {
        float num5 = tmin;
        tmin = tmax;
        tmax = num5;
      }
      return true;
    }

    public BoundingSphere Include(BoundingSphere sphere)
    {
      BoundingSphere.Include(ref this, ref sphere);
      return this;
    }

    public static void Include(ref BoundingSphere sphere, ref BoundingSphere otherSphere)
    {
      if ((double) sphere.Radius == -3.40282346638529E+38)
      {
        sphere.Center = otherSphere.Center;
        sphere.Radius = otherSphere.Radius;
      }
      else
      {
        float num1 = Vector3.Distance(sphere.Center, otherSphere.Center);
        if ((double) num1 + (double) otherSphere.Radius <= (double) sphere.Radius)
          return;
        if ((double) num1 + (double) sphere.Radius <= (double) otherSphere.Radius)
        {
          sphere = otherSphere;
        }
        else
        {
          float amount = (float) (((double) num1 + (double) otherSphere.Radius - (double) sphere.Radius) / (2.0 * (double) num1));
          Vector3 vector3 = Vector3.Lerp(sphere.Center, otherSphere.Center, amount);
          float num2 = (float) (((double) num1 + (double) sphere.Radius + (double) otherSphere.Radius) / 2.0);
          sphere.Center = vector3;
          sphere.Radius = num2;
        }
      }
    }

    public static BoundingSphere CreateInvalid() => new BoundingSphere(Vector3.Zero, float.MinValue);

    public BoundingBox GetBoundingBox() => new BoundingBox(this.Center - this.Radius, this.Center + this.Radius);

    protected class VRageMath_BoundingSphere\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<BoundingSphere, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingSphere owner, in Vector3 value) => owner.Center = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingSphere owner, out Vector3 value) => value = owner.Center;
    }

    protected class VRageMath_BoundingSphere\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<BoundingSphere, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingSphere owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingSphere owner, out float value) => value = owner.Radius;
    }
  }
}
