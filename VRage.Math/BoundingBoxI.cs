// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBoxI
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct BoundingBoxI : IEquatable<BoundingBoxI>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector3I Min;
    [ProtoMember(4)]
    public Vector3I Max;

    public BoundingBoxI(BoundingBox box)
    {
      this.Min = new Vector3I(box.Min);
      this.Max = new Vector3I(box.Max);
    }

    public BoundingBoxI(Vector3I min, Vector3I max)
    {
      this.Min = min;
      this.Max = max;
    }

    public BoundingBoxI(int min, int max)
    {
      this.Min = new Vector3I(min);
      this.Max = new Vector3I(max);
    }

    public static explicit operator BoundingBoxI(BoundingBoxD box) => new BoundingBoxI((Vector3I) box.Min, (Vector3I) box.Max);

    public static explicit operator BoundingBoxI(BoundingBox box) => new BoundingBoxI((Vector3I) box.Min, (Vector3I) box.Max);

    public static bool operator ==(BoundingBoxI a, BoundingBoxI b) => a.Equals(b);

    public static bool operator !=(BoundingBoxI a, BoundingBoxI b) => a.Min != b.Min || a.Max != b.Max;

    public Vector3I[] GetCorners() => new Vector3I[8]
    {
      new Vector3I(this.Min.X, this.Max.Y, this.Max.Z),
      new Vector3I(this.Max.X, this.Max.Y, this.Max.Z),
      new Vector3I(this.Max.X, this.Min.Y, this.Max.Z),
      new Vector3I(this.Min.X, this.Min.Y, this.Max.Z),
      new Vector3I(this.Min.X, this.Max.Y, this.Min.Z),
      new Vector3I(this.Max.X, this.Max.Y, this.Min.Z),
      new Vector3I(this.Max.X, this.Min.Y, this.Min.Z),
      new Vector3I(this.Min.X, this.Min.Y, this.Min.Z)
    };

    public void GetCorners(Vector3I[] corners)
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

    public unsafe void GetCornersUnsafe(Vector3I* corners)
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

    public bool Equals(BoundingBoxI other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingBoxI other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[2]
    {
      (object) this.Min.ToString(),
      (object) this.Max.ToString()
    });

    public static BoundingBoxI CreateMerged(
      BoundingBoxI original,
      BoundingBoxI additional)
    {
      BoundingBoxI boundingBoxI;
      Vector3I.Min(ref original.Min, ref additional.Min, out boundingBoxI.Min);
      Vector3I.Max(ref original.Max, ref additional.Max, out boundingBoxI.Max);
      return boundingBoxI;
    }

    public static void CreateMerged(
      ref BoundingBoxI original,
      ref BoundingBoxI additional,
      out BoundingBoxI result)
    {
      Vector3I result1;
      Vector3I.Min(ref original.Min, ref additional.Min, out result1);
      Vector3I result2;
      Vector3I.Max(ref original.Max, ref additional.Max, out result2);
      result.Min = result1;
      result.Max = result2;
    }

    public static BoundingBoxI CreateFromSphere(BoundingSphere sphere)
    {
      BoundingBoxI boundingBoxI;
      boundingBoxI.Min.X = (int) ((double) sphere.Center.X - (double) sphere.Radius);
      boundingBoxI.Min.Y = (int) ((double) sphere.Center.Y - (double) sphere.Radius);
      boundingBoxI.Min.Z = (int) ((double) sphere.Center.Z - (double) sphere.Radius);
      boundingBoxI.Max.X = (int) ((double) sphere.Center.X + (double) sphere.Radius);
      boundingBoxI.Max.Y = (int) ((double) sphere.Center.Y + (double) sphere.Radius);
      boundingBoxI.Max.Z = (int) ((double) sphere.Center.Z + (double) sphere.Radius);
      return boundingBoxI;
    }

    public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBoxI result)
    {
      result.Min.X = (int) ((double) sphere.Center.X - (double) sphere.Radius);
      result.Min.Y = (int) ((double) sphere.Center.Y - (double) sphere.Radius);
      result.Min.Z = (int) ((double) sphere.Center.Z - (double) sphere.Radius);
      result.Max.X = (int) ((double) sphere.Center.X + (double) sphere.Radius);
      result.Max.Y = (int) ((double) sphere.Center.Y + (double) sphere.Radius);
      result.Max.Z = (int) ((double) sphere.Center.Z + (double) sphere.Radius);
    }

    public static BoundingBoxI CreateFromPoints(IEnumerable<Vector3I> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector3I result1 = new Vector3I(int.MaxValue);
      Vector3I result2 = new Vector3I(int.MinValue);
      foreach (Vector3I point in points)
      {
        Vector3I.Min(ref result1, ref point, out result1);
        Vector3I.Max(ref result2, ref point, out result2);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBoxI(result1, result2);
    }

    public void IntersectWith(ref BoundingBoxI box)
    {
      this.Min.X = Math.Max(this.Min.X, box.Min.X);
      this.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      this.Min.Z = Math.Max(this.Min.Z, box.Min.Z);
      this.Max.X = Math.Min(this.Max.X, box.Max.X);
      this.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      this.Max.Z = Math.Min(this.Max.Z, box.Max.Z);
    }

    public BoundingBoxI Intersect(BoundingBoxI box)
    {
      BoundingBoxI boundingBoxI;
      boundingBoxI.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBoxI.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBoxI.Min.Z = Math.Max(this.Min.Z, box.Min.Z);
      boundingBoxI.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBoxI.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      boundingBoxI.Max.Z = Math.Min(this.Max.Z, box.Max.Z);
      return boundingBoxI;
    }

    public bool Intersects(BoundingBoxI box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBoxI box) => (double) this.Max.X >= (double) box.Min.X && (double) this.Min.X <= (double) box.Max.X && ((double) this.Max.Y >= (double) box.Min.Y && (double) this.Min.Y <= (double) box.Max.Y) && (double) this.Max.Z >= (double) box.Min.Z && (double) this.Min.Z <= (double) box.Max.Z;

    public void Intersects(ref BoundingBoxI box, out bool result)
    {
      result = false;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return;
      result = true;
    }

    public bool IntersectsTriangle(Vector3I v0, Vector3I v1, Vector3I v2) => this.IntersectsTriangle(ref v0, ref v1, ref v2);

    public bool IntersectsTriangle(ref Vector3I v0, ref Vector3I v1, ref Vector3I v2)
    {
      Vector3I result1;
      Vector3I.Min(ref v0, ref v1, out result1);
      Vector3I.Min(ref result1, ref v2, out result1);
      Vector3I result2;
      Vector3I.Max(ref v0, ref v1, out result2);
      Vector3I.Max(ref result2, ref v2, out result2);
      if (result1.X > this.Max.X || result2.X < this.Min.X || (result1.Y > this.Max.Y || result2.Y < this.Min.Y) || (result1.Z > this.Max.Z || result2.Z < this.Min.Z))
        return false;
      Vector3I vector1 = v1 - v0;
      Vector3I vector2 = v2 - v1;
      Vector3I result3;
      Vector3I.Cross(ref vector1, ref vector2, out result3);
      int dot;
      Vector3I.Dot(ref v0, ref result3, out dot);
      Plane plane = new Plane((Vector3) result3, (float) -dot);
      PlaneIntersectionType result4;
      this.Intersects(ref plane, out result4);
      if (result4 == PlaneIntersectionType.Back || result4 == PlaneIntersectionType.Front)
        return false;
      Vector3I center = this.Center;
      Vector3I halfExtents = new BoundingBoxI(this.Min - center, this.Max - center).HalfExtents;
      Vector3I vector3I1 = v0 - v2;
      Vector3I vector3I2 = v0 - center;
      Vector3I vector3I3 = v1 - center;
      Vector3I vector3I4 = v2 - center;
      float num1 = (float) (halfExtents.Y * Math.Abs(vector1.Z) + halfExtents.Z * Math.Abs(vector1.Y));
      float val1_1 = (float) (vector3I2.Z * vector3I3.Y - vector3I2.Y * vector3I3.Z);
      float val2_1 = (float) (vector3I4.Z * vector1.Y - vector3I4.Y * vector1.Z);
      if ((double) Math.Min(val1_1, val2_1) > (double) num1 || (double) Math.Max(val1_1, val2_1) < -(double) num1)
        return false;
      float num2 = (float) (halfExtents.X * Math.Abs(vector1.Z) + halfExtents.Z * Math.Abs(vector1.X));
      float val1_2 = (float) (vector3I2.X * vector3I3.Z - vector3I2.Z * vector3I3.X);
      float val2_2 = (float) (vector3I4.X * vector1.Z - vector3I4.Z * vector1.X);
      if ((double) Math.Min(val1_2, val2_2) > (double) num2 || (double) Math.Max(val1_2, val2_2) < -(double) num2)
        return false;
      float num3 = (float) (halfExtents.X * Math.Abs(vector1.Y) + halfExtents.Y * Math.Abs(vector1.X));
      float val1_3 = (float) (vector3I2.Y * vector3I3.X - vector3I2.X * vector3I3.Y);
      float val2_3 = (float) (vector3I4.Y * vector1.X - vector3I4.X * vector1.Y);
      if ((double) Math.Min(val1_3, val2_3) > (double) num3 || (double) Math.Max(val1_3, val2_3) < -(double) num3)
        return false;
      float num4 = (float) (halfExtents.Y * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.Y));
      float val1_4 = (float) (vector3I3.Z * vector3I4.Y - vector3I3.Y * vector3I4.Z);
      float val2_4 = (float) (vector3I2.Z * vector2.Y - vector3I2.Y * vector2.Z);
      if ((double) Math.Min(val1_4, val2_4) > (double) num4 || (double) Math.Max(val1_4, val2_4) < -(double) num4)
        return false;
      float num5 = (float) (halfExtents.X * Math.Abs(vector2.Z) + halfExtents.Z * Math.Abs(vector2.X));
      float val1_5 = (float) (vector3I3.X * vector3I4.Z - vector3I3.Z * vector3I4.X);
      float val2_5 = (float) (vector3I2.X * vector2.Z - vector3I2.Z * vector2.X);
      if ((double) Math.Min(val1_5, val2_5) > (double) num5 || (double) Math.Max(val1_5, val2_5) < -(double) num5)
        return false;
      float num6 = (float) (halfExtents.X * Math.Abs(vector2.Y) + halfExtents.Y * Math.Abs(vector2.X));
      float val1_6 = (float) (vector3I3.Y * vector3I4.X - vector3I3.X * vector3I4.Y);
      float val2_6 = (float) (vector3I2.Y * vector2.X - vector3I2.X * vector2.Y);
      if ((double) Math.Min(val1_6, val2_6) > (double) num6 || (double) Math.Max(val1_6, val2_6) < -(double) num6)
        return false;
      float num7 = (float) (halfExtents.Y * Math.Abs(vector3I1.Z) + halfExtents.Z * Math.Abs(vector3I1.Y));
      float val1_7 = (float) (vector3I4.Z * vector3I2.Y - vector3I4.Y * vector3I2.Z);
      float val2_7 = (float) (vector3I3.Z * vector3I1.Y - vector3I3.Y * vector3I1.Z);
      if ((double) Math.Min(val1_7, val2_7) > (double) num7 || (double) Math.Max(val1_7, val2_7) < -(double) num7)
        return false;
      float num8 = (float) (halfExtents.X * Math.Abs(vector3I1.Z) + halfExtents.Z * Math.Abs(vector3I1.X));
      float val1_8 = (float) (vector3I4.X * vector3I2.Z - vector3I4.Z * vector3I2.X);
      float val2_8 = (float) (vector3I3.X * vector3I1.Z - vector3I3.Z * vector3I1.X);
      if ((double) Math.Min(val1_8, val2_8) > (double) num8 || (double) Math.Max(val1_8, val2_8) < -(double) num8)
        return false;
      float num9 = (float) (halfExtents.X * Math.Abs(vector3I1.Y) + halfExtents.Y * Math.Abs(vector3I1.X));
      float val1_9 = (float) (vector3I4.Y * vector3I2.X - vector3I4.X * vector3I2.Y);
      float val2_9 = (float) (vector3I3.Y * vector3I1.X - vector3I3.X * vector3I1.Y);
      return (double) Math.Min(val1_9, val2_9) <= (double) num9 && (double) Math.Max(val1_9, val2_9) >= -(double) num9;
    }

    public Vector3I Center => (this.Min + this.Max) / 2;

    public Vector3I HalfExtents => (this.Max - this.Min) / 2;

    public PlaneIntersectionType Intersects(Plane plane)
    {
      Vector3I vector3I1;
      vector3I1.X = (double) plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3I1.Y = (double) plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3I1.Z = (double) plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      Vector3I vector3I2;
      vector3I2.X = (double) plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3I2.Y = (double) plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3I2.Z = (double) plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      if ((double) plane.Normal.X * (double) vector3I1.X + (double) plane.Normal.Y * (double) vector3I1.Y + (double) plane.Normal.Z * (double) vector3I1.Z + (double) plane.D > 0.0)
        return PlaneIntersectionType.Front;
      return (double) plane.Normal.X * (double) vector3I2.X + (double) plane.Normal.Y * (double) vector3I2.Y + (double) plane.Normal.Z * (double) vector3I2.Z + (double) plane.D >= 0.0 ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      Vector3I vector3I1;
      vector3I1.X = (double) plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3I1.Y = (double) plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3I1.Z = (double) plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      Vector3I vector3I2;
      vector3I2.X = (double) plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3I2.Y = (double) plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3I2.Z = (double) plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      if ((double) plane.Normal.X * (double) vector3I1.X + (double) plane.Normal.Y * (double) vector3I1.Y + (double) plane.Normal.Z * (double) vector3I1.Z + (double) plane.D > 0.0)
        result = PlaneIntersectionType.Front;
      else if ((double) plane.Normal.X * (double) vector3I2.X + (double) plane.Normal.Y * (double) vector3I2.Y + (double) plane.Normal.Z * (double) vector3I2.Z + (double) plane.D < 0.0)
        result = PlaneIntersectionType.Back;
      else
        result = PlaneIntersectionType.Intersecting;
    }

    public bool Intersects(Line line, out float distance)
    {
      distance = 0.0f;
      float? nullable = this.Intersects(new Ray(line.From, line.Direction));
      if (!nullable.HasValue || (double) nullable.Value < 0.0 || (double) nullable.Value > (double) line.Length)
        return false;
      distance = nullable.Value;
      return true;
    }

    public float? Intersects(Ray ray)
    {
      float num1 = 0.0f;
      float num2 = float.MaxValue;
      if ((double) Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.X < (double) this.Min.X || (double) ray.Position.X > (double) this.Max.X)
          return new float?();
      }
      else
      {
        float num3 = 1f / ray.Direction.X;
        float num4 = ((float) this.Min.X - ray.Position.X) * num3;
        float num5 = ((float) this.Max.X - ray.Position.X) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num2)
          return new float?();
      }
      if ((double) Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Y < (double) this.Min.Y || (double) ray.Position.Y > (double) this.Max.Y)
          return new float?();
      }
      else
      {
        float num3 = 1f / ray.Direction.Y;
        float num4 = ((float) this.Min.Y - ray.Position.Y) * num3;
        float num5 = ((float) this.Max.Y - ray.Position.Y) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num2)
          return new float?();
      }
      if ((double) Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Z < (double) this.Min.Z || (double) ray.Position.Z > (double) this.Max.Z)
          return new float?();
      }
      else
      {
        float num3 = 1f / ray.Direction.Z;
        float num4 = ((float) this.Min.Z - ray.Position.Z) * num3;
        float num5 = ((float) this.Max.Z - ray.Position.Z) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        float num7 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num7)
          return new float?();
      }
      return new float?(num1);
    }

    public void Intersects(ref Ray ray, out float? result)
    {
      result = new float?();
      float num1 = 0.0f;
      float num2 = float.MaxValue;
      if ((double) Math.Abs(ray.Direction.X) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.X < (double) this.Min.X || (double) ray.Position.X > (double) this.Max.X)
          return;
      }
      else
      {
        float num3 = 1f / ray.Direction.X;
        float num4 = ((float) this.Min.X - ray.Position.X) * num3;
        float num5 = ((float) this.Max.X - ray.Position.X) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num2)
          return;
      }
      if ((double) Math.Abs(ray.Direction.Y) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Y < (double) this.Min.Y || (double) ray.Position.Y > (double) this.Max.Y)
          return;
      }
      else
      {
        float num3 = 1f / ray.Direction.Y;
        float num4 = ((float) this.Min.Y - ray.Position.Y) * num3;
        float num5 = ((float) this.Max.Y - ray.Position.Y) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        num2 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num2)
          return;
      }
      if ((double) Math.Abs(ray.Direction.Z) < 9.99999997475243E-07)
      {
        if ((double) ray.Position.Z < (double) this.Min.Z || (double) ray.Position.Z > (double) this.Max.Z)
          return;
      }
      else
      {
        float num3 = 1f / ray.Direction.Z;
        float num4 = ((float) this.Min.Z - ray.Position.Z) * num3;
        float num5 = ((float) this.Max.Z - ray.Position.Z) * num3;
        if ((double) num4 > (double) num5)
        {
          double num6 = (double) num4;
          num4 = num5;
          num5 = (float) num6;
        }
        num1 = MathHelper.Max(num4, num1);
        float num7 = MathHelper.Min(num5, num2);
        if ((double) num1 > (double) num7)
          return;
      }
      result = new float?(num1);
    }

    public float Distance(Vector3I point) => (float) (Vector3I.Clamp(point, this.Min, this.Max) - point).Length();

    public ContainmentType Contains(BoundingBoxI box)
    {
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return ContainmentType.Disjoint;
      return (double) this.Min.X <= (double) box.Min.X && (double) box.Max.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) box.Min.Y && (double) box.Max.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= (double) box.Min.Z && (double) box.Max.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBoxI box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return;
      result = (double) this.Min.X > (double) box.Min.X || (double) box.Max.X > (double) this.Max.X || ((double) this.Min.Y > (double) box.Min.Y || (double) box.Max.Y > (double) this.Max.Y) || ((double) this.Min.Z > (double) box.Min.Z || (double) box.Max.Z > (double) this.Max.Z) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3I point) => (double) this.Min.X <= (double) point.X && (double) point.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) point.Y && (double) point.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= (double) point.Z && (double) point.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public ContainmentType Contains(Vector3 point) => (double) this.Min.X <= (double) point.X && (double) point.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) point.Y && (double) point.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= (double) point.Z && (double) point.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector3I point, out ContainmentType result) => result = (double) this.Min.X > (double) point.X || (double) point.X > (double) this.Max.X || ((double) this.Min.Y > (double) point.Y || (double) point.Y > (double) this.Max.Y) || ((double) this.Min.Z > (double) point.Z || (double) point.Z > (double) this.Max.Z) ? ContainmentType.Disjoint : ContainmentType.Contains;

    internal void SupportMapping(ref Vector3I v, out Vector3I result)
    {
      result.X = (double) v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = (double) v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      result.Z = (double) v.Z >= 0.0 ? this.Max.Z : this.Min.Z;
    }

    public BoundingBoxI Translate(Vector3I vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector3I Size => this.Max - this.Min;

    public BoundingBoxI Include(ref Vector3I point)
    {
      if (point.X < this.Min.X)
        this.Min.X = point.X;
      if (point.Y < this.Min.Y)
        this.Min.Y = point.Y;
      if (point.Z < this.Min.Z)
        this.Min.Z = point.Z;
      if (point.X > this.Max.X)
        this.Max.X = point.X;
      if (point.Y > this.Max.Y)
        this.Max.Y = point.Y;
      if (point.Z > this.Max.Z)
        this.Max.Z = point.Z;
      return this;
    }

    public BoundingBoxI GetIncluded(Vector3I point)
    {
      BoundingBoxI boundingBoxI = this;
      boundingBoxI.Include(point);
      return boundingBoxI;
    }

    public BoundingBoxI Include(Vector3I point) => this.Include(ref point);

    public BoundingBoxI Include(Vector3I p0, Vector3I p1, Vector3I p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBoxI Include(ref Vector3I p0, ref Vector3I p1, ref Vector3I p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBoxI Include(ref BoundingBoxI box)
    {
      this.Min = Vector3I.Min(this.Min, box.Min);
      this.Max = Vector3I.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBoxI Include(BoundingBoxI box) => this.Include(ref box);

    public static BoundingBoxI CreateInvalid()
    {
      BoundingBoxI boundingBoxI = new BoundingBoxI();
      Vector3I vector3I1 = new Vector3I(int.MaxValue, int.MaxValue, int.MaxValue);
      Vector3I vector3I2 = new Vector3I(int.MinValue, int.MinValue, int.MinValue);
      boundingBoxI.Min = vector3I1;
      boundingBoxI.Max = vector3I2;
      return boundingBoxI;
    }

    public float SurfaceArea()
    {
      Vector3I vector3I = this.Max - this.Min;
      return (float) (2 * (vector3I.X * vector3I.Y + vector3I.X * vector3I.Z + vector3I.Y * vector3I.Z));
    }

    public float Volume()
    {
      Vector3I vector3I = this.Max - this.Min;
      return (float) (vector3I.X * vector3I.Y * vector3I.Z);
    }

    public float Perimeter => (float) (4.0 * ((double) (this.Max.X - this.Min.X) + (double) (this.Max.Y - this.Min.Y) + (double) (this.Max.Z - this.Min.Z)));

    public void Inflate(int size)
    {
      this.Max += new Vector3I(size);
      this.Min -= new Vector3I(size);
    }

    public void InflateToMinimum(Vector3I minimumSize)
    {
      Vector3I center = this.Center;
      if (this.Size.X < minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X / 2;
        this.Max.X = center.X + minimumSize.X / 2;
      }
      if (this.Size.Y < minimumSize.Y)
      {
        this.Min.Y = center.Y - minimumSize.Y / 2;
        this.Max.Y = center.Y + minimumSize.Y / 2;
      }
      if (this.Size.Z >= minimumSize.Z)
        return;
      this.Min.Z = center.Z - minimumSize.Z / 2;
      this.Max.Z = center.Z + minimumSize.Z / 2;
    }

    public bool IsValid => this.Min.X <= this.Max.X && this.Min.Y <= this.Max.Y && this.Min.Z <= this.Max.Z;

    public static IEnumerable<Vector3I> IterateDifference(
      BoundingBoxI left,
      BoundingBoxI right)
    {
      Vector3I min = left.Min;
      Vector3I max = new Vector3I(Math.Min(left.Max.X, right.Min.X), left.Max.Y, left.Max.Z);
      Vector3I vec;
      for (vec.X = min.X; vec.X < max.X; ++vec.X)
      {
        for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
        {
          for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
            yield return vec;
        }
      }
      min = new Vector3I(Math.Max(left.Min.X, right.Max.X), left.Min.Y, left.Min.Z);
      max = left.Max;
      for (vec.X = min.X; vec.X < max.X; ++vec.X)
      {
        for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
        {
          for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
            yield return vec;
        }
      }
      left.Min.X = Math.Max(left.Min.X, right.Min.X);
      left.Max.X = Math.Min(left.Max.X, right.Max.X);
      min = left.Min;
      max = new Vector3I(left.Max.X, Math.Min(left.Max.Y, right.Min.Y), left.Max.Z);
      for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
      {
        for (vec.X = min.X; vec.X < max.X; ++vec.X)
        {
          for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
            yield return vec;
        }
      }
      min = new Vector3I(left.Min.X, Math.Max(left.Min.Y, right.Max.Y), left.Min.Z);
      max = left.Max;
      for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
      {
        for (vec.X = min.X; vec.X < max.X; ++vec.X)
        {
          for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
            yield return vec;
        }
      }
      left.Min.Y = Math.Max(left.Min.Y, right.Min.Y);
      left.Max.Y = Math.Min(left.Max.Y, right.Max.Y);
      min = left.Min;
      max = new Vector3I(left.Max.X, left.Max.Y, Math.Min(left.Max.Z, right.Min.Z));
      for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
      {
        for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
        {
          for (vec.X = min.X; vec.X < max.X; ++vec.X)
            yield return vec;
        }
      }
      min = new Vector3I(left.Min.X, left.Min.Y, Math.Max(left.Min.Z, right.Max.Z));
      max = left.Max;
      for (vec.Z = min.Z; vec.Z < max.Z; ++vec.Z)
      {
        for (vec.Y = min.Y; vec.Y < max.Y; ++vec.Y)
        {
          for (vec.X = min.X; vec.X < max.X; ++vec.X)
            yield return vec;
        }
      }
    }

    public static IEnumerable<Vector3I> EnumeratePoints(
      BoundingBoxI rangeInclusive)
    {
      Vector3I vec;
      for (vec.Z = rangeInclusive.Min.Z; vec.Z < rangeInclusive.Max.Z; ++vec.Z)
      {
        for (vec.Y = rangeInclusive.Min.Y; vec.Y < rangeInclusive.Max.Y; ++vec.Y)
        {
          for (vec.X = rangeInclusive.Min.X; vec.X < rangeInclusive.Max.X; ++vec.X)
            yield return vec;
        }
      }
    }

    protected class VRageMath_BoundingBoxI\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBoxI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBoxI owner, in Vector3I value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBoxI owner, out Vector3I value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBoxI\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBoxI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBoxI owner, in Vector3I value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBoxI owner, out Vector3I value) => value = owner.Max;
    }
  }
}
