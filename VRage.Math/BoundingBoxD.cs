// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBoxD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct BoundingBoxD : IEquatable<BoundingBoxD>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector3D Min;
    [ProtoMember(4)]
    public Vector3D Max;
    public static readonly BoundingBoxD.ComparerType Comparer = new BoundingBoxD.ComparerType();

    public BoundingBoxD(Vector3D min, Vector3D max)
    {
      this.Min = min;
      this.Max = max;
    }

    public static implicit operator BoundingBoxD(BoundingBoxI box) => new BoundingBoxD((Vector3D) box.Min, (Vector3D) box.Max);

    public static implicit operator BoundingBoxD(BoundingBox box) => new BoundingBoxD((Vector3D) box.Min, (Vector3D) box.Max);

    public static bool operator ==(BoundingBoxD a, BoundingBoxD b) => a.Equals(b);

    public static bool operator !=(BoundingBoxD a, BoundingBoxD b) => a.Min != b.Min || a.Max != b.Max;

    public static BoundingBoxD operator +(BoundingBoxD a, Vector3D b)
    {
      BoundingBoxD boundingBoxD;
      boundingBoxD.Max = a.Max + b;
      boundingBoxD.Min = a.Min + b;
      return boundingBoxD;
    }

    public void Centerize(Vector3D center)
    {
      Vector3D size = this.Size;
      this.Min = center - size / 2.0;
      this.Max = center + size / 2.0;
    }

    public Vector3D[] GetCorners() => new Vector3D[8]
    {
      new Vector3D(this.Min.X, this.Max.Y, this.Max.Z),
      new Vector3D(this.Max.X, this.Max.Y, this.Max.Z),
      new Vector3D(this.Max.X, this.Min.Y, this.Max.Z),
      new Vector3D(this.Min.X, this.Min.Y, this.Max.Z),
      new Vector3D(this.Min.X, this.Max.Y, this.Min.Z),
      new Vector3D(this.Max.X, this.Max.Y, this.Min.Z),
      new Vector3D(this.Max.X, this.Min.Y, this.Min.Z),
      new Vector3D(this.Min.X, this.Min.Y, this.Min.Z)
    };

    public void GetCorners(Vector3D[] corners)
    {
      corners[0].X = this.Min.X;
      corners[0].Y = this.Max.Y;
      corners[0].Z = this.Max.Z;
      corners[1].X = this.Max.X;
      corners[1].Y = this.Max.Y;
      corners[1].Z = this.Max.Z;
      corners[2].X = this.Max.X;
      corners[2].Y = this.Min.Y;
      corners[2].Z = this.Max.Z;
      corners[3].X = this.Min.X;
      corners[3].Y = this.Min.Y;
      corners[3].Z = this.Max.Z;
      corners[4].X = this.Min.X;
      corners[4].Y = this.Max.Y;
      corners[4].Z = this.Min.Z;
      corners[5].X = this.Max.X;
      corners[5].Y = this.Max.Y;
      corners[5].Z = this.Min.Z;
      corners[6].X = this.Max.X;
      corners[6].Y = this.Min.Y;
      corners[6].Z = this.Min.Z;
      corners[7].X = this.Min.X;
      corners[7].Y = this.Min.Y;
      corners[7].Z = this.Min.Z;
    }

    public unsafe void GetCornersUnsafe(Vector3D* corners)
    {
      corners->X = this.Min.X;
      corners->Y = this.Max.Y;
      corners->Z = this.Max.Z;
      corners[1].X = this.Max.X;
      corners[1].Y = this.Max.Y;
      corners[1].Z = this.Max.Z;
      corners[2].X = this.Max.X;
      corners[2].Y = this.Min.Y;
      corners[2].Z = this.Max.Z;
      corners[3].X = this.Min.X;
      corners[3].Y = this.Min.Y;
      corners[3].Z = this.Max.Z;
      corners[4].X = this.Min.X;
      corners[4].Y = this.Max.Y;
      corners[4].Z = this.Min.Z;
      corners[5].X = this.Max.X;
      corners[5].Y = this.Max.Y;
      corners[5].Z = this.Min.Z;
      corners[6].X = this.Max.X;
      corners[6].Y = this.Min.Y;
      corners[6].Z = this.Min.Z;
      corners[7].X = this.Min.X;
      corners[7].Y = this.Min.Y;
      corners[7].Z = this.Min.Z;
    }

    public bool Equals(BoundingBoxD other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj) => obj is BoundingBoxD other && this.Equals(other);

    public bool Equals(BoundingBoxD other, double epsilon) => this.Min.Equals(other.Min, epsilon) && this.Max.Equals(other.Max, epsilon);

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[2]
    {
      (object) this.Min.ToString(),
      (object) this.Max.ToString()
    });

    public static BoundingBoxD CreateMerged(
      BoundingBoxD original,
      BoundingBoxD additional)
    {
      BoundingBoxD boundingBoxD;
      Vector3D.Min(ref original.Min, ref additional.Min, out boundingBoxD.Min);
      Vector3D.Max(ref original.Max, ref additional.Max, out boundingBoxD.Max);
      return boundingBoxD;
    }

    public static void CreateMerged(
      ref BoundingBoxD original,
      ref BoundingBoxD additional,
      out BoundingBoxD result)
    {
      Vector3D result1;
      Vector3D.Min(ref original.Min, ref additional.Min, out result1);
      Vector3D result2;
      Vector3D.Max(ref original.Max, ref additional.Max, out result2);
      result.Min = result1;
      result.Max = result2;
    }

    public static BoundingBoxD CreateFromSphere(BoundingSphereD sphere)
    {
      BoundingBoxD boundingBoxD;
      boundingBoxD.Min.X = sphere.Center.X - sphere.Radius;
      boundingBoxD.Min.Y = sphere.Center.Y - sphere.Radius;
      boundingBoxD.Min.Z = sphere.Center.Z - sphere.Radius;
      boundingBoxD.Max.X = sphere.Center.X + sphere.Radius;
      boundingBoxD.Max.Y = sphere.Center.Y + sphere.Radius;
      boundingBoxD.Max.Z = sphere.Center.Z + sphere.Radius;
      return boundingBoxD;
    }

    public static void CreateFromSphere(ref BoundingSphereD sphere, out BoundingBoxD result)
    {
      result.Min.X = sphere.Center.X - sphere.Radius;
      result.Min.Y = sphere.Center.Y - sphere.Radius;
      result.Min.Z = sphere.Center.Z - sphere.Radius;
      result.Max.X = sphere.Center.X + sphere.Radius;
      result.Max.Y = sphere.Center.Y + sphere.Radius;
      result.Max.Z = sphere.Center.Z + sphere.Radius;
    }

    public static BoundingBoxD CreateFromPoints(IEnumerable<Vector3D> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector3D result1 = new Vector3D(double.MaxValue);
      Vector3D result2 = new Vector3D(double.MinValue);
      foreach (Vector3D point in points)
      {
        Vector3D.Min(ref result1, ref point, out result1);
        Vector3D.Max(ref result2, ref point, out result2);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBoxD(result1, result2);
    }

    public BoundingBoxD Intersect(BoundingBoxD box)
    {
      BoundingBoxD boundingBoxD;
      boundingBoxD.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBoxD.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBoxD.Min.Z = Math.Max(this.Min.Z, box.Min.Z);
      boundingBoxD.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBoxD.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      boundingBoxD.Max.Z = Math.Min(this.Max.Z, box.Max.Z);
      return boundingBoxD;
    }

    public bool Intersects(BoundingBoxD box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBoxD box) => this.Max.X >= box.Min.X && this.Min.X <= box.Max.X && (this.Max.Y >= box.Min.Y && this.Min.Y <= box.Max.Y) && this.Max.Z >= box.Min.Z && this.Min.Z <= box.Max.Z;

    public void Intersects(ref BoundingBoxD box, out bool result)
    {
      result = false;
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y) || (this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z))
        return;
      result = true;
    }

    public void Intersects(ref BoundingBox box, out bool result)
    {
      result = false;
      if (this.Max.X < (double) box.Min.X || this.Min.X > (double) box.Max.X || (this.Max.Y < (double) box.Min.Y || this.Min.Y > (double) box.Max.Y) || (this.Max.Z < (double) box.Min.Z || this.Min.Z > (double) box.Max.Z))
        return;
      result = true;
    }

    public bool IntersectsTriangle(Vector3D v0, Vector3D v1, Vector3D v2) => this.IntersectsTriangle(ref v0, ref v1, ref v2);

    public bool IntersectsTriangle(ref Vector3D v0, ref Vector3D v1, ref Vector3D v2)
    {
      Vector3D result1;
      Vector3D.Min(ref v0, ref v1, out result1);
      Vector3D.Min(ref result1, ref v2, out result1);
      Vector3D result2;
      Vector3D.Max(ref v0, ref v1, out result2);
      Vector3D.Max(ref result2, ref v2, out result2);
      if (result1.X > this.Max.X || result2.X < this.Min.X || (result1.Y > this.Max.Y || result2.Y < this.Min.Y) || (result1.Z > this.Max.Z || result2.Z < this.Min.Z))
        return false;
      Vector3D vector1 = v1 - v0;
      Vector3D vector2 = v2 - v1;
      Vector3D result3;
      Vector3D.Cross(ref vector1, ref vector2, out result3);
      double result4;
      Vector3D.Dot(ref v0, ref result3, out result4);
      PlaneD plane = new PlaneD(result3, -result4);
      PlaneIntersectionType result5;
      this.Intersects(ref plane, out result5);
      if (result5 == PlaneIntersectionType.Back || result5 == PlaneIntersectionType.Front)
        return false;
      Vector3D center = this.Center;
      Vector3D halfExtents = new BoundingBoxD(this.Min - center, this.Max - center).HalfExtents;
      Vector3D vector3D1 = v0 - v2;
      Vector3D vector3D2 = v0 - center;
      Vector3D vector3D3 = v1 - center;
      Vector3D vector3D4 = v2 - center;
      double num1 = halfExtents.Y * Math.Abs(vector1.Z) + halfExtents.Z * Math.Abs(vector1.Y);
      double val1_1 = vector3D2.Z * vector3D3.Y - vector3D2.Y * vector3D3.Z;
      double val2_1 = vector3D4.Z * vector1.Y - vector3D4.Y * vector1.Z;
      if (Math.Min(val1_1, val2_1) > num1 || Math.Max(val1_1, val2_1) < -num1)
        return false;
      double num2 = halfExtents.X * Math.Abs(vector1.Z) + halfExtents.Z * Math.Abs(vector1.X);
      double val1_2 = vector3D2.X * vector3D3.Z - vector3D2.Z * vector3D3.X;
      double val2_2 = vector3D4.X * vector1.Z - vector3D4.Z * vector1.X;
      if (Math.Min(val1_2, val2_2) > num2 || Math.Max(val1_2, val2_2) < -num2)
        return false;
      double num3 = halfExtents.X * Math.Abs(vector1.Y) + halfExtents.Y * Math.Abs(vector1.X);
      double val1_3 = vector3D2.Y * vector3D3.X - vector3D2.X * vector3D3.Y;
      double val2_3 = vector3D4.Y * vector1.X - vector3D4.X * vector1.Y;
      if (Math.Min(val1_3, val2_3) > num3 || Math.Max(val1_3, val2_3) < -num3)
        return false;
      double num4 = halfExtents.Y * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.Y);
      double val1_4 = vector3D3.Z * vector3D4.Y - vector3D3.Y * vector3D4.Z;
      double val2_4 = vector3D2.Z * vector2.Y - vector3D2.Y * vector2.Z;
      if (Math.Min(val1_4, val2_4) > num4 || Math.Max(val1_4, val2_4) < -num4)
        return false;
      double num5 = halfExtents.X * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.X);
      double val1_5 = vector3D3.X * vector3D4.Z - vector3D3.Z * vector3D4.X;
      double val2_5 = vector3D2.X * vector2.Z - vector3D2.Z * vector2.X;
      if (Math.Min(val1_5, val2_5) > num5 || Math.Max(val1_5, val2_5) < -num5)
        return false;
      double num6 = halfExtents.X * Math.Abs(vector2.Y) + halfExtents.Y * Math.Abs(vector2.X);
      double val1_6 = vector3D3.Y * vector3D4.X - vector3D3.X * vector3D4.Y;
      double val2_6 = vector3D2.Y * vector2.X - vector3D2.X * vector2.Y;
      if (Math.Min(val1_6, val2_6) > num6 || Math.Max(val1_6, val2_6) < -num6)
        return false;
      double num7 = halfExtents.Y * Math.Abs(vector3D1.Z) + halfExtents.Z * Math.Abs(vector3D1.Y);
      double val1_7 = vector3D4.Z * vector3D2.Y - vector3D4.Y * vector3D2.Z;
      double val2_7 = vector3D3.Z * vector3D1.Y - vector3D3.Y * vector3D1.Z;
      if (Math.Min(val1_7, val2_7) > num7 || Math.Max(val1_7, val2_7) < -num7)
        return false;
      double num8 = halfExtents.X * Math.Abs(vector3D1.Z) + halfExtents.Z * Math.Abs(vector3D1.X);
      double val1_8 = vector3D4.X * vector3D2.Z - vector3D4.Z * vector3D2.X;
      double val2_8 = vector3D3.X * vector3D1.Z - vector3D3.Z * vector3D1.X;
      if (Math.Min(val1_8, val2_8) > num8 || Math.Max(val1_8, val2_8) < -num8)
        return false;
      double num9 = halfExtents.X * Math.Abs(vector3D1.Y) + halfExtents.Y * Math.Abs(vector3D1.X);
      double val1_9 = vector3D4.Y * vector3D2.X - vector3D4.X * vector3D2.Y;
      double val2_9 = vector3D3.Y * vector3D1.X - vector3D3.X * vector3D1.Y;
      return Math.Min(val1_9, val2_9) <= num9 && Math.Max(val1_9, val2_9) >= -num9;
    }

    public Vector3D Center => (this.Min + this.Max) * 0.5;

    public Vector3D HalfExtents => (this.Max - this.Min) * 0.5;

    public Vector3D Extents => this.Max - this.Min;

    public bool Intersects(BoundingFrustumD frustum) => !((BoundingFrustumD) null == frustum) ? frustum.Intersects(this) : throw new ArgumentNullException(nameof (frustum));

    public PlaneIntersectionType Intersects(PlaneD plane)
    {
      Vector3D vector3D1;
      vector3D1.X = plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3D1.Y = plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3D1.Z = plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      if (plane.Normal.X * vector3D1.X + plane.Normal.Y * vector3D1.Y + plane.Normal.Z * vector3D1.Z + plane.D > 0.0)
        return PlaneIntersectionType.Front;
      Vector3D vector3D2;
      vector3D2.X = plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3D2.Y = plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3D2.Z = plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      return plane.Normal.X * vector3D2.X + plane.Normal.Y * vector3D2.Y + plane.Normal.Z * vector3D2.Z + plane.D >= 0.0 ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref PlaneD plane, out PlaneIntersectionType result)
    {
      Vector3D vector3D1;
      vector3D1.X = plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3D1.Y = plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3D1.Z = plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      Vector3D vector3D2;
      vector3D2.X = plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3D2.Y = plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3D2.Z = plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      if (plane.Normal.X * vector3D1.X + plane.Normal.Y * vector3D1.Y + plane.Normal.Z * vector3D1.Z + plane.D > 0.0)
        result = PlaneIntersectionType.Front;
      else if (plane.Normal.X * vector3D2.X + plane.Normal.Y * vector3D2.Y + plane.Normal.Z * vector3D2.Z + plane.D < 0.0)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public bool Intersects(ref LineD line)
    {
      double? nullable = this.Intersects(new RayD(line.From, line.Direction));
      return nullable.HasValue && nullable.Value >= 0.0 && nullable.Value <= line.Length;
    }

    public bool Intersects(ref LineD line, out double distance)
    {
      distance = 0.0;
      double? nullable = this.Intersects(new RayD(line.From, line.Direction));
      if (!nullable.HasValue || nullable.Value < 0.0 || nullable.Value > line.Length)
        return false;
      distance = nullable.Value;
      return true;
    }

    public double? Intersects(Ray ray) => this.Intersects(new RayD((Vector3D) ray.Position, (Vector3D) ray.Direction));

    public double? Intersects(RayD ray)
    {
      double num1 = 0.0;
      double num2 = double.MaxValue;
      if (Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
      {
        if (ray.Position.X < this.Min.X || ray.Position.X > this.Max.X)
          return new double?();
      }
      else
      {
        double num3 = 1.0 / ray.Direction.X;
        double num4 = (this.Min.X - ray.Position.X) * num3;
        double num5 = (this.Max.X - ray.Position.X) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if (num1 > num2)
          return new double?();
      }
      if (Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
      {
        if (ray.Position.Y < this.Min.Y || ray.Position.Y > this.Max.Y)
          return new double?();
      }
      else
      {
        double num3 = 1.0 / ray.Direction.Y;
        double num4 = (this.Min.Y - ray.Position.Y) * num3;
        double num5 = (this.Max.Y - ray.Position.Y) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if (num1 > num2)
          return new double?();
      }
      if (Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
      {
        if (ray.Position.Z < this.Min.Z || ray.Position.Z > this.Max.Z)
          return new double?();
      }
      else
      {
        double num3 = 1.0 / ray.Direction.Z;
        double num4 = (this.Min.Z - ray.Position.Z) * num3;
        double num5 = (this.Max.Z - ray.Position.Z) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        double num7 = MathHelper.Min(num5, num2);
        if (num1 > num7)
          return new double?();
      }
      return new double?(num1);
    }

    public void Intersects(ref RayD ray, out double? result)
    {
      result = new double?();
      double num1 = 0.0;
      double num2 = double.MaxValue;
      if (Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
      {
        if (ray.Position.X < this.Min.X || ray.Position.X > this.Max.X)
          return;
      }
      else
      {
        double num3 = 1.0 / ray.Direction.X;
        double num4 = (this.Min.X - ray.Position.X) * num3;
        double num5 = (this.Max.X - ray.Position.X) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if (num1 > num2)
          return;
      }
      if (Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
      {
        if (ray.Position.Y < this.Min.Y || ray.Position.Y > this.Max.Y)
          return;
      }
      else
      {
        double num3 = 1.0 / ray.Direction.Y;
        double num4 = (this.Min.Y - ray.Position.Y) * num3;
        double num5 = (this.Max.Y - ray.Position.Y) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if (num1 > num2)
          return;
      }
      if (Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
      {
        if (ray.Position.Z < this.Min.Z || ray.Position.Z > this.Max.Z)
          return;
      }
      else
      {
        double num3 = 1.0 / ray.Direction.Z;
        double num4 = (this.Min.Z - ray.Position.Z) * num3;
        double num5 = (this.Max.Z - ray.Position.Z) * num3;
        if (num4 > num5)
        {
          double num6 = num4;
          num4 = num5;
          num5 = num6;
        }
        num1 = MathHelper.Max(num4, num1);
        double num7 = MathHelper.Min(num5, num2);
        if (num1 > num7)
          return;
      }
      result = new double?(num1);
    }

    public bool Intersect(ref LineD line, out LineD intersectedLine)
    {
      RayD ray = new RayD(line.From, line.Direction);
      double tmin;
      double tmax;
      if (!this.Intersect(ref ray, out tmin, out tmax))
      {
        intersectedLine = line;
        return false;
      }
      double num1 = Math.Max(tmin, 0.0);
      double num2 = Math.Min(tmax, line.Length);
      intersectedLine.From = line.From + line.Direction * num1;
      intersectedLine.To = line.From + line.Direction * num2;
      intersectedLine.Direction = line.Direction;
      intersectedLine.Length = num2 - num1;
      return true;
    }

    public bool Intersect(ref LineD line, out double t1, out double t2)
    {
      RayD ray = new RayD(line.From, line.Direction);
      return this.Intersect(ref ray, out t1, out t2);
    }

    public bool Intersect(ref RayD ray, out double tmin, out double tmax)
    {
      double num1 = 1.0 / ray.Direction.X;
      double num2 = 1.0 / ray.Direction.Y;
      double num3 = 1.0 / ray.Direction.Z;
      double val1_1 = (this.Min.X - ray.Position.X) * num1;
      double val2_1 = (this.Max.X - ray.Position.X) * num1;
      double val1_2 = (this.Min.Y - ray.Position.Y) * num2;
      double val2_2 = (this.Max.Y - ray.Position.Y) * num2;
      double val1_3 = (this.Min.Z - ray.Position.Z) * num3;
      double val2_3 = (this.Max.Z - ray.Position.Z) * num3;
      tmin = Math.Max(Math.Max(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2)), Math.Min(val1_3, val2_3));
      tmax = Math.Min(Math.Min(Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2)), Math.Max(val1_3, val2_3));
      return tmax >= 0.0 && tmin <= tmax;
    }

    public bool Intersects(BoundingSphereD sphere) => this.Intersects(ref sphere);

    public void Intersects(ref BoundingSphereD sphere, out bool result)
    {
      Vector3D result1;
      Vector3D.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref sphere.Center, ref result1, out result2);
      result = result2 <= sphere.Radius * sphere.Radius;
    }

    public bool Intersects(ref BoundingSphereD sphere)
    {
      Vector3D result1;
      Vector3D.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref sphere.Center, ref result1, out result2);
      return result2 <= sphere.Radius * sphere.Radius;
    }

    public double Distance(Vector3D point) => this.Contains(point) == ContainmentType.Contains ? 0.0 : Vector3D.Distance(Vector3D.Clamp(point, this.Min, this.Max), point);

    public double DistanceSquared(Vector3D point) => this.DistanceSquared(ref point);

    public double DistanceSquared(ref Vector3D point)
    {
      Vector3D result1;
      Vector3D.Clamp(ref point, ref this.Min, ref this.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref result1, ref point, out result2);
      return result2;
    }

    public double Distance(ref BoundingBoxD other) => Math.Sqrt(this.DistanceSquared(ref other));

    public double DistanceSquared(ref BoundingBoxD other)
    {
      Vector3D min1 = this.Min;
      Vector3D min2 = other.Min;
      Vector3D max1 = this.Max;
      Vector3D max2 = other.Max;
      double num1 = 0.0;
      if (max2.X < min1.X)
      {
        double num2 = min1.X - max2.X;
        num1 += num2 * num2;
      }
      else if (max1.X < min2.X)
      {
        double num2 = min2.X - max1.X;
        num1 += num2 * num2;
      }
      if (max2.Y < min1.Y)
      {
        double num2 = min1.Y - max2.Y;
        num1 += num2 * num2;
      }
      else if (max1.Y < min2.Y)
      {
        double num2 = min2.Y - max1.Y;
        num1 += num2 * num2;
      }
      if (max2.Z < min1.Z)
      {
        double num2 = min1.Z - max2.Z;
        num1 += num2 * num2;
      }
      else if (max1.Z < min2.Z)
      {
        double num2 = min2.Z - max1.Z;
        num1 += num2 * num2;
      }
      return num1;
    }

    public ContainmentType Contains(BoundingBoxD box)
    {
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y) || (this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z))
        return ContainmentType.Disjoint;
      return this.Min.X <= box.Min.X && box.Max.X <= this.Max.X && (this.Min.Y <= box.Min.Y && box.Max.Y <= this.Max.Y) && (this.Min.Z <= box.Min.Z && box.Max.Z <= this.Max.Z) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBoxD box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y) || (this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z))
        return;
      result = this.Min.X > box.Min.X || box.Max.X > this.Max.X || (this.Min.Y > box.Min.Y || box.Max.Y > this.Max.Y) || (this.Min.Z > box.Min.Z || box.Max.Z > this.Max.Z) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(BoundingFrustumD frustum)
    {
      if (!frustum.Intersects(this))
        return ContainmentType.Disjoint;
      foreach (Vector3D corner in frustum.CornerArray)
      {
        if (this.Contains(corner) == ContainmentType.Disjoint)
          return ContainmentType.Intersects;
      }
      return ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3D point) => this.Min.X <= point.X && point.X <= this.Max.X && (this.Min.Y <= point.Y && point.Y <= this.Max.Y) && (this.Min.Z <= point.Z && point.Z <= this.Max.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector3D point, out ContainmentType result) => result = this.Min.X > point.X || point.X > this.Max.X || (this.Min.Y > point.Y || point.Y > this.Max.Y) || (this.Min.Z > point.Z || point.Z > this.Max.Z) ? ContainmentType.Disjoint : ContainmentType.Contains;

    public ContainmentType Contains(BoundingSphereD sphere)
    {
      Vector3D result1;
      Vector3D.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref sphere.Center, ref result1, out result2);
      double radius = sphere.Radius;
      if (result2 > radius * radius)
        return ContainmentType.Disjoint;
      return this.Min.X + radius <= sphere.Center.X && sphere.Center.X <= this.Max.X - radius && (this.Max.X - this.Min.X > radius && this.Min.Y + radius <= sphere.Center.Y) && (sphere.Center.Y <= this.Max.Y - radius && this.Max.Y - this.Min.Y > radius && (this.Min.Z + radius <= sphere.Center.Z && sphere.Center.Z <= this.Max.Z - radius)) && this.Max.X - this.Min.X > radius ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
    {
      Vector3D result1;
      Vector3D.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      double result2;
      Vector3D.DistanceSquared(ref sphere.Center, ref result1, out result2);
      double radius = sphere.Radius;
      if (result2 > radius * radius)
        result = ContainmentType.Disjoint;
      else
        result = this.Min.X + radius > sphere.Center.X || sphere.Center.X > this.Max.X - radius || (this.Max.X - this.Min.X <= radius || this.Min.Y + radius > sphere.Center.Y) || (sphere.Center.Y > this.Max.Y - radius || this.Max.Y - this.Min.Y <= radius || (this.Min.Z + radius > sphere.Center.Z || sphere.Center.Z > this.Max.Z - radius)) || this.Max.X - this.Min.X <= radius ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    internal void SupportMapping(ref Vector3D v, out Vector3D result)
    {
      result.X = v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      result.Z = v.Z >= 0.0 ? this.Max.Z : this.Min.Z;
    }

    public BoundingBoxD Translate(MatrixD worldMatrix)
    {
      this.Min += worldMatrix.Translation;
      this.Max += worldMatrix.Translation;
      return this;
    }

    public BoundingBoxD Translate(Vector3D vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector3D Size => this.Max - this.Min;

    public MatrixD Matrix
    {
      get
      {
        Vector3D center = this.Center;
        Vector3D size = this.Size;
        MatrixD result;
        MatrixD.CreateTranslation(ref center, out result);
        MatrixD.Rescale(ref result, ref size);
        return result;
      }
    }

    public BoundingBoxD TransformSlow(MatrixD m) => this.TransformSlow(ref m);

    public unsafe BoundingBoxD TransformSlow(ref MatrixD worldMatrix)
    {
      BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
      Vector3D* corners = stackalloc Vector3D[8];
      this.GetCornersUnsafe(corners);
      for (int index = 0; index < 8; ++index)
      {
        Vector3D point = Vector3D.Transform(corners[index], worldMatrix);
        boundingBoxD = boundingBoxD.Include(ref point);
      }
      return boundingBoxD;
    }

    public BoundingBoxD TransformFast(MatrixD m)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      this.TransformFast(ref m, ref invalid);
      return invalid;
    }

    public BoundingBoxD TransformFast(ref MatrixD m)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      this.TransformFast(ref m, ref invalid);
      return invalid;
    }

    public void TransformFast(ref MatrixD m, ref BoundingBoxD bb)
    {
      bb.Min = bb.Max = m.Translation;
      Vector3D min1 = m.Right * this.Min.X;
      Vector3D max1 = m.Right * this.Max.X;
      Vector3D.MinMax(ref min1, ref max1);
      bb.Min += min1;
      bb.Max += max1;
      Vector3D min2 = m.Up * this.Min.Y;
      Vector3D max2 = m.Up * this.Max.Y;
      Vector3D.MinMax(ref min2, ref max2);
      bb.Min += min2;
      bb.Max += max2;
      Vector3D min3 = m.Backward * this.Min.Z;
      Vector3D max3 = m.Backward * this.Max.Z;
      Vector3D.MinMax(ref min3, ref max3);
      bb.Min += min3;
      bb.Max += max3;
    }

    public BoundingBoxD Include(ref Vector3D point)
    {
      this.Min.X = Math.Min(point.X, this.Min.X);
      this.Min.Y = Math.Min(point.Y, this.Min.Y);
      this.Min.Z = Math.Min(point.Z, this.Min.Z);
      this.Max.X = Math.Max(point.X, this.Max.X);
      this.Max.Y = Math.Max(point.Y, this.Max.Y);
      this.Max.Z = Math.Max(point.Z, this.Max.Z);
      return this;
    }

    public BoundingBoxD Include(Vector3D point) => this.Include(ref point);

    public BoundingBoxD Include(Vector3D p0, Vector3D p1, Vector3D p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBoxD Include(ref Vector3D p0, ref Vector3D p1, ref Vector3D p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBoxD Include(ref BoundingBoxD box)
    {
      this.Min = Vector3D.Min(this.Min, box.Min);
      this.Max = Vector3D.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBoxD Include(BoundingBoxD box) => this.Include(ref box);

    public void Include(ref LineD line)
    {
      this.Include(ref line.From);
      this.Include(ref line.To);
    }

    public BoundingBoxD Include(BoundingSphereD sphere) => this.Include(ref sphere);

    public BoundingBoxD Include(ref BoundingSphereD sphere)
    {
      Vector3D vector3D = new Vector3D(sphere.Radius);
      Vector3D result1 = sphere.Center;
      Vector3D result2 = sphere.Center;
      Vector3D.Subtract(ref result1, ref vector3D, out result1);
      Vector3D.Add(ref result2, ref vector3D, out result2);
      this.Include(ref result1);
      this.Include(ref result2);
      return this;
    }

    public unsafe BoundingBoxD Include(ref BoundingFrustumD frustum)
    {
      Vector3D* corners = stackalloc Vector3D[8];
      frustum.GetCornersUnsafe(corners);
      this.Include(ref corners[0]);
      this.Include(ref corners[1]);
      this.Include(ref corners[2]);
      this.Include(ref corners[3]);
      this.Include(ref corners[4]);
      this.Include(ref corners[5]);
      this.Include(ref corners[6]);
      this.Include(ref corners[7]);
      return this;
    }

    public static BoundingBoxD CreateInvalid() => new BoundingBoxD(new Vector3D(double.MaxValue), new Vector3D(double.MinValue));

    public double SurfaceArea
    {
      get
      {
        Vector3D vector3D = this.Max - this.Min;
        return 2.0 * (vector3D.X * vector3D.Y + vector3D.X * vector3D.Z + vector3D.Y * vector3D.Z);
      }
    }

    public double Volume
    {
      get
      {
        Vector3D vector3D = this.Max - this.Min;
        return vector3D.X * vector3D.Y * vector3D.Z;
      }
    }

    public double ProjectedArea(Vector3D viewDir)
    {
      Vector3D vector3D = this.Max - this.Min;
      Vector3D v = new Vector3D(vector3D.Y, vector3D.Z, vector3D.X) * new Vector3D(vector3D.Z, vector3D.X, vector3D.Y);
      return Vector3D.Abs(viewDir).Dot(v);
    }

    public double Perimeter => 4.0 * (this.Max.X - this.Min.X + (this.Max.Y - this.Min.Y) + (this.Max.Z - this.Min.Z));

    public bool Valid => this.Min != new Vector3D(double.MaxValue) && this.Max != new Vector3D(double.MinValue);

    public BoundingBoxD Inflate(double size)
    {
      this.Max += new Vector3D(size);
      this.Min -= new Vector3D(size);
      return this;
    }

    public BoundingBoxD Inflate(Vector3D size)
    {
      this.Max += size;
      this.Min -= size;
      return this;
    }

    public BoundingBoxD GetInflated(double size)
    {
      BoundingBoxD boundingBoxD = this;
      boundingBoxD.Inflate(size);
      return boundingBoxD;
    }

    public BoundingBoxD GetInflated(Vector3 size)
    {
      BoundingBoxD boundingBoxD = this;
      boundingBoxD.Inflate((Vector3D) size);
      return boundingBoxD;
    }

    public BoundingBoxD GetInflated(Vector3D size)
    {
      BoundingBoxD boundingBoxD = this;
      boundingBoxD.Inflate(size);
      return boundingBoxD;
    }

    public static explicit operator BoundingBox(BoundingBoxD b) => new BoundingBox((Vector3) b.Min, (Vector3) b.Max);

    public void InflateToMinimum(Vector3D minimumSize)
    {
      Vector3D center = this.Center;
      if (this.Size.X < minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X * 0.5;
        this.Max.X = center.X + minimumSize.X * 0.5;
      }
      if (this.Size.Y < minimumSize.Y)
      {
        this.Min.Y = center.Y - minimumSize.Y * 0.5;
        this.Max.Y = center.Y + minimumSize.Y * 0.5;
      }
      if (this.Size.Z >= minimumSize.Z)
        return;
      this.Min.Z = center.Z - minimumSize.Z * 0.5;
      this.Max.Z = center.Z + minimumSize.Z * 0.5;
    }

    public void InflateToMinimum(double minimumSize)
    {
      Vector3D center = this.Center;
      if (this.Size.X < minimumSize)
      {
        this.Min.X = center.X - minimumSize * 0.5;
        this.Max.X = center.X + minimumSize * 0.5;
      }
      if (this.Size.Y < minimumSize)
      {
        this.Min.Y = center.Y - minimumSize * 0.5;
        this.Max.Y = center.Y + minimumSize * 0.5;
      }
      if (this.Size.Z >= minimumSize)
        return;
      this.Min.Z = center.Z - minimumSize * 0.5;
      this.Max.Z = center.Z + minimumSize * 0.5;
    }

    public BoundingBoxD Round(int decimals) => new BoundingBoxD(Vector3D.Round(this.Min, decimals), Vector3D.Round(this.Max, decimals));

    public BoundingBoxI Round() => new BoundingBoxI(Vector3D.Round(this.Min), Vector3D.Round(this.Max));

    [Conditional("DEBUG")]
    public void AssertIsValid()
    {
    }

    public class ComparerType : IEqualityComparer<BoundingBox>
    {
      public bool Equals(BoundingBox x, BoundingBox y) => x.Min == y.Min && x.Max == y.Max;

      public int GetHashCode(BoundingBox obj) => obj.Min.GetHashCode() ^ obj.Max.GetHashCode();
    }

    protected class VRageMath_BoundingBoxD\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBoxD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBoxD owner, in Vector3D value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBoxD owner, out Vector3D value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBoxD\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBoxD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBoxD owner, in Vector3D value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBoxD owner, out Vector3D value) => value = owner.Max;
    }
  }
}
