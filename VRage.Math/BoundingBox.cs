// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBox
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
  public struct BoundingBox : IEquatable<BoundingBox>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector3 Min;
    [ProtoMember(4)]
    public Vector3 Max;
    public static readonly BoundingBox Invalid = BoundingBox.CreateInvalid();
    public static readonly BoundingBox.ComparerType Comparer = new BoundingBox.ComparerType();

    public BoundingBox(Vector3 min, Vector3 max)
    {
      this.Min = min;
      this.Max = max;
    }

    public BoundingBox(BoundingBoxD bbd)
    {
      this.Min = (Vector3) bbd.Min;
      this.Max = (Vector3) bbd.Max;
    }

    public BoundingBox(BoundingBoxI bbd)
    {
      this.Min = (Vector3) bbd.Min;
      this.Max = (Vector3) bbd.Max;
    }

    public BoxCornerEnumerator Corners
    {
      get => new BoxCornerEnumerator(this.Min, this.Max);
      set
      {
      }
    }

    public static bool operator ==(BoundingBox a, BoundingBox b) => a.Equals(b);

    public static bool operator !=(BoundingBox a, BoundingBox b) => a.Min != b.Min || a.Max != b.Max;

    public Vector3[] GetCorners() => new Vector3[8]
    {
      new Vector3(this.Min.X, this.Max.Y, this.Max.Z),
      new Vector3(this.Max.X, this.Max.Y, this.Max.Z),
      new Vector3(this.Max.X, this.Min.Y, this.Max.Z),
      new Vector3(this.Min.X, this.Min.Y, this.Max.Z),
      new Vector3(this.Min.X, this.Max.Y, this.Min.Z),
      new Vector3(this.Max.X, this.Max.Y, this.Min.Z),
      new Vector3(this.Max.X, this.Min.Y, this.Min.Z),
      new Vector3(this.Min.X, this.Min.Y, this.Min.Z)
    };

    public void GetCorners(Vector3[] corners)
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

    public unsafe void GetCornersUnsafe(Vector3* corners)
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

    public bool Equals(BoundingBox other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj) => obj is BoundingBox other && this.Equals(other);

    public bool Equals(BoundingBox other, float epsilon) => this.Min.Equals(other.Min, epsilon) && this.Max.Equals(other.Max, epsilon);

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[2]
    {
      (object) this.Min.ToString(),
      (object) this.Max.ToString()
    });

    public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
    {
      BoundingBox boundingBox;
      Vector3.Min(ref original.Min, ref additional.Min, out boundingBox.Min);
      Vector3.Max(ref original.Max, ref additional.Max, out boundingBox.Max);
      return boundingBox;
    }

    public static void CreateMerged(
      ref BoundingBox original,
      ref BoundingBox additional,
      out BoundingBox result)
    {
      Vector3 result1;
      Vector3.Min(ref original.Min, ref additional.Min, out result1);
      Vector3 result2;
      Vector3.Max(ref original.Max, ref additional.Max, out result2);
      result.Min = result1;
      result.Max = result2;
    }

    public static BoundingBox CreateFromSphere(BoundingSphere sphere)
    {
      BoundingBox boundingBox;
      boundingBox.Min.X = sphere.Center.X - sphere.Radius;
      boundingBox.Min.Y = sphere.Center.Y - sphere.Radius;
      boundingBox.Min.Z = sphere.Center.Z - sphere.Radius;
      boundingBox.Max.X = sphere.Center.X + sphere.Radius;
      boundingBox.Max.Y = sphere.Center.Y + sphere.Radius;
      boundingBox.Max.Z = sphere.Center.Z + sphere.Radius;
      return boundingBox;
    }

    public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
    {
      result.Min.X = sphere.Center.X - sphere.Radius;
      result.Min.Y = sphere.Center.Y - sphere.Radius;
      result.Min.Z = sphere.Center.Z - sphere.Radius;
      result.Max.X = sphere.Center.X + sphere.Radius;
      result.Max.Y = sphere.Center.Y + sphere.Radius;
      result.Max.Z = sphere.Center.Z + sphere.Radius;
    }

    public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector3 result1 = new Vector3(float.MaxValue);
      Vector3 result2 = new Vector3(float.MinValue);
      foreach (Vector3 point in points)
      {
        Vector3.Min(ref result1, ref point, out result1);
        Vector3.Max(ref result2, ref point, out result2);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBox(result1, result2);
    }

    public static BoundingBox CreateFromHalfExtent(Vector3 center, float halfExtent) => BoundingBox.CreateFromHalfExtent(center, new Vector3(halfExtent));

    public static BoundingBox CreateFromHalfExtent(Vector3 center, Vector3 halfExtent) => new BoundingBox(center - halfExtent, center + halfExtent);

    public BoundingBox Intersect(BoundingBox box)
    {
      BoundingBox boundingBox;
      boundingBox.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBox.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBox.Min.Z = Math.Max(this.Min.Z, box.Min.Z);
      boundingBox.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBox.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      boundingBox.Max.Z = Math.Min(this.Max.Z, box.Max.Z);
      return boundingBox;
    }

    public bool Intersects(BoundingBox box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBox box) => (double) this.Max.X >= (double) box.Min.X && (double) this.Min.X <= (double) box.Max.X && ((double) this.Max.Y >= (double) box.Min.Y && (double) this.Min.Y <= (double) box.Max.Y) && (double) this.Max.Z >= (double) box.Min.Z && (double) this.Min.Z <= (double) box.Max.Z;

    public void Intersects(ref BoundingBox box, out bool result)
    {
      result = false;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return;
      result = true;
    }

    public bool IntersectsTriangle(Vector3 v0, Vector3 v1, Vector3 v2) => this.IntersectsTriangle(ref v0, ref v1, ref v2);

    public bool IntersectsTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
    {
      Vector3 result1;
      Vector3.Min(ref v0, ref v1, out result1);
      Vector3.Min(ref result1, ref v2, out result1);
      Vector3 result2;
      Vector3.Max(ref v0, ref v1, out result2);
      Vector3.Max(ref result2, ref v2, out result2);
      if ((double) result1.X > (double) this.Max.X || (double) result2.X < (double) this.Min.X || ((double) result1.Y > (double) this.Max.Y || (double) result2.Y < (double) this.Min.Y) || ((double) result1.Z > (double) this.Max.Z || (double) result2.Z < (double) this.Min.Z))
        return false;
      Vector3 vector1 = v1 - v0;
      Vector3 vector2 = v2 - v1;
      Vector3 result3;
      Vector3.Cross(ref vector1, ref vector2, out result3);
      float result4;
      Vector3.Dot(ref v0, ref result3, out result4);
      Plane plane = new Plane(result3, -result4);
      PlaneIntersectionType result5;
      this.Intersects(ref plane, out result5);
      if (result5 == PlaneIntersectionType.Back || result5 == PlaneIntersectionType.Front)
        return false;
      Vector3 center = this.Center;
      Vector3 halfExtents = new BoundingBox(this.Min - center, this.Max - center).HalfExtents;
      Vector3 vector3_1 = v0 - v2;
      Vector3 vector3_2 = v0 - center;
      Vector3 vector3_3 = v1 - center;
      Vector3 vector3_4 = v2 - center;
      float num1 = (float) ((double) halfExtents.Y * (double) Math.Abs(vector1.Z) + (double) halfExtents.Z * (double) Math.Abs(vector1.Y));
      float val1_1 = (float) ((double) vector3_2.Z * (double) vector3_3.Y - (double) vector3_2.Y * (double) vector3_3.Z);
      float val2_1 = (float) ((double) vector3_4.Z * (double) vector1.Y - (double) vector3_4.Y * (double) vector1.Z);
      if ((double) Math.Min(val1_1, val2_1) > (double) num1 || (double) Math.Max(val1_1, val2_1) < -(double) num1)
        return false;
      float num2 = (float) ((double) halfExtents.X * (double) Math.Abs(vector1.Z) + (double) halfExtents.Z * (double) Math.Abs(vector1.X));
      float val1_2 = (float) ((double) vector3_2.X * (double) vector3_3.Z - (double) vector3_2.Z * (double) vector3_3.X);
      float val2_2 = (float) ((double) vector3_4.X * (double) vector1.Z - (double) vector3_4.Z * (double) vector1.X);
      if ((double) Math.Min(val1_2, val2_2) > (double) num2 || (double) Math.Max(val1_2, val2_2) < -(double) num2)
        return false;
      float num3 = (float) ((double) halfExtents.X * (double) Math.Abs(vector1.Y) + (double) halfExtents.Y * (double) Math.Abs(vector1.X));
      float val1_3 = (float) ((double) vector3_2.Y * (double) vector3_3.X - (double) vector3_2.X * (double) vector3_3.Y);
      float val2_3 = (float) ((double) vector3_4.Y * (double) vector1.X - (double) vector3_4.X * (double) vector1.Y);
      if ((double) Math.Min(val1_3, val2_3) > (double) num3 || (double) Math.Max(val1_3, val2_3) < -(double) num3)
        return false;
      float num4 = (float) ((double) halfExtents.Y * (double) Math.Abs(vector2.Z) + (double) halfExtents.Z * (double) Math.Abs(vector2.Y));
      float val1_4 = (float) ((double) vector3_3.Z * (double) vector3_4.Y - (double) vector3_3.Y * (double) vector3_4.Z);
      float val2_4 = (float) ((double) vector3_2.Z * (double) vector2.Y - (double) vector3_2.Y * (double) vector2.Z);
      if ((double) Math.Min(val1_4, val2_4) > (double) num4 || (double) Math.Max(val1_4, val2_4) < -(double) num4)
        return false;
      float num5 = (float) ((double) halfExtents.X * (double) Math.Abs(vector2.Z) + (double) halfExtents.Z * (double) Math.Abs(vector2.X));
      float val1_5 = (float) ((double) vector3_3.X * (double) vector3_4.Z - (double) vector3_3.Z * (double) vector3_4.X);
      float val2_5 = (float) ((double) vector3_2.X * (double) vector2.Z - (double) vector3_2.Z * (double) vector2.X);
      if ((double) Math.Min(val1_5, val2_5) > (double) num5 || (double) Math.Max(val1_5, val2_5) < -(double) num5)
        return false;
      float num6 = (float) ((double) halfExtents.X * (double) Math.Abs(vector2.Y) + (double) halfExtents.Y * (double) Math.Abs(vector2.X));
      float val1_6 = (float) ((double) vector3_3.Y * (double) vector3_4.X - (double) vector3_3.X * (double) vector3_4.Y);
      float val2_6 = (float) ((double) vector3_2.Y * (double) vector2.X - (double) vector3_2.X * (double) vector2.Y);
      if ((double) Math.Min(val1_6, val2_6) > (double) num6 || (double) Math.Max(val1_6, val2_6) < -(double) num6)
        return false;
      float num7 = (float) ((double) halfExtents.Y * (double) Math.Abs(vector3_1.Z) + (double) halfExtents.Z * (double) Math.Abs(vector3_1.Y));
      float val1_7 = (float) ((double) vector3_4.Z * (double) vector3_2.Y - (double) vector3_4.Y * (double) vector3_2.Z);
      float val2_7 = (float) ((double) vector3_3.Z * (double) vector3_1.Y - (double) vector3_3.Y * (double) vector3_1.Z);
      if ((double) Math.Min(val1_7, val2_7) > (double) num7 || (double) Math.Max(val1_7, val2_7) < -(double) num7)
        return false;
      float num8 = (float) ((double) halfExtents.X * (double) Math.Abs(vector3_1.Z) + (double) halfExtents.Z * (double) Math.Abs(vector3_1.X));
      float val1_8 = (float) ((double) vector3_4.X * (double) vector3_2.Z - (double) vector3_4.Z * (double) vector3_2.X);
      float val2_8 = (float) ((double) vector3_3.X * (double) vector3_1.Z - (double) vector3_3.Z * (double) vector3_1.X);
      if ((double) Math.Min(val1_8, val2_8) > (double) num8 || (double) Math.Max(val1_8, val2_8) < -(double) num8)
        return false;
      float num9 = (float) ((double) halfExtents.X * (double) Math.Abs(vector3_1.Y) + (double) halfExtents.Y * (double) Math.Abs(vector3_1.X));
      float val1_9 = (float) ((double) vector3_4.Y * (double) vector3_2.X - (double) vector3_4.X * (double) vector3_2.Y);
      float val2_9 = (float) ((double) vector3_3.Y * (double) vector3_1.X - (double) vector3_3.X * (double) vector3_1.Y);
      return (double) Math.Min(val1_9, val2_9) <= (double) num9 && (double) Math.Max(val1_9, val2_9) >= -(double) num9;
    }

    public Vector3 Center => (this.Min + this.Max) * 0.5f;

    public Vector3 HalfExtents => (this.Max - this.Min) * 0.5f;

    public Vector3 Extents => this.Max - this.Min;

    public float Width => this.Max.X - this.Min.X;

    public float Height => this.Max.Y - this.Min.Y;

    public float Depth => this.Max.Z - this.Min.Z;

    public bool Intersects(BoundingFrustum frustum) => !((BoundingFrustum) null == frustum) ? frustum.Intersects(this) : throw new ArgumentNullException(nameof (frustum));

    public PlaneIntersectionType Intersects(Plane plane)
    {
      Vector3 vector3_1;
      vector3_1.X = (double) plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3_1.Y = (double) plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3_1.Z = (double) plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      if ((double) plane.Normal.X * (double) vector3_1.X + (double) plane.Normal.Y * (double) vector3_1.Y + (double) plane.Normal.Z * (double) vector3_1.Z + (double) plane.D > 0.0)
        return PlaneIntersectionType.Front;
      Vector3 vector3_2;
      vector3_2.X = (double) plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3_2.Y = (double) plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3_2.Z = (double) plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      return (double) plane.Normal.X * (double) vector3_2.X + (double) plane.Normal.Y * (double) vector3_2.Y + (double) plane.Normal.Z * (double) vector3_2.Z + (double) plane.D >= 0.0 ? PlaneIntersectionType.Intersecting : PlaneIntersectionType.Back;
    }

    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
      Vector3 vector3_1;
      vector3_1.X = (double) plane.Normal.X >= 0.0 ? this.Min.X : this.Max.X;
      vector3_1.Y = (double) plane.Normal.Y >= 0.0 ? this.Min.Y : this.Max.Y;
      vector3_1.Z = (double) plane.Normal.Z >= 0.0 ? this.Min.Z : this.Max.Z;
      if ((double) plane.Normal.X * (double) vector3_1.X + (double) plane.Normal.Y * (double) vector3_1.Y + (double) plane.Normal.Z * (double) vector3_1.Z + (double) plane.D > 0.0)
        result = PlaneIntersectionType.Front;
      Vector3 vector3_2;
      vector3_2.X = (double) plane.Normal.X >= 0.0 ? this.Max.X : this.Min.X;
      vector3_2.Y = (double) plane.Normal.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      vector3_2.Z = (double) plane.Normal.Z >= 0.0 ? this.Max.Z : this.Min.Z;
      result = (double) plane.Normal.X * (double) vector3_2.X + (double) plane.Normal.Y * (double) vector3_2.Y + (double) plane.Normal.Z * (double) vector3_2.Z + (double) plane.D < 0.0 ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
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
        float num4 = (this.Min.X - ray.Position.X) * num3;
        float num5 = (this.Max.X - ray.Position.X) * num3;
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
        float num4 = (this.Min.Y - ray.Position.Y) * num3;
        float num5 = (this.Max.Y - ray.Position.Y) * num3;
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
        float num4 = (this.Min.Z - ray.Position.Z) * num3;
        float num5 = (this.Max.Z - ray.Position.Z) * num3;
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
        float num4 = (this.Min.X - ray.Position.X) * num3;
        float num5 = (this.Max.X - ray.Position.X) * num3;
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
        float num4 = (this.Min.Y - ray.Position.Y) * num3;
        float num5 = (this.Max.Y - ray.Position.Y) * num3;
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
        float num4 = (this.Min.Z - ray.Position.Z) * num3;
        float num5 = (this.Max.Z - ray.Position.Z) * num3;
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

    public bool Intersects(BoundingSphere sphere) => this.Intersects(ref sphere);

    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
      Vector3 result1;
      Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref sphere.Center, ref result1, out result2);
      result = (double) result2 <= (double) sphere.Radius * (double) sphere.Radius;
    }

    public bool Intersects(ref BoundingSphere sphere)
    {
      Vector3 result1;
      Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref sphere.Center, ref result1, out result2);
      return (double) result2 <= (double) sphere.Radius * (double) sphere.Radius;
    }

    public bool Intersects(ref BoundingSphereD sphere)
    {
      Vector3 center = (Vector3) sphere.Center;
      Vector3 result1;
      Vector3.Clamp(ref center, ref this.Min, ref this.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref center, ref result1, out result2);
      return (double) result2 <= sphere.Radius * sphere.Radius;
    }

    public float Distance(Vector3 point) => this.Contains(point) == ContainmentType.Contains ? 0.0f : Vector3.Distance(Vector3.Clamp(point, this.Min, this.Max), point);

    public float DistanceSquared(Vector3 point) => this.Contains(point) == ContainmentType.Contains ? 0.0f : Vector3.DistanceSquared(Vector3.Clamp(point, this.Min, this.Max), point);

    public ContainmentType Contains(BoundingBox box)
    {
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return ContainmentType.Disjoint;
      return (double) this.Min.X <= (double) box.Min.X && (double) box.Max.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) box.Min.Y && (double) box.Max.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= (double) box.Min.Z && (double) box.Max.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y) || ((double) this.Max.Z < (double) box.Min.Z || (double) this.Min.Z > (double) box.Max.Z))
        return;
      result = (double) this.Min.X > (double) box.Min.X || (double) box.Max.X > (double) this.Max.X || ((double) this.Min.Y > (double) box.Min.Y || (double) box.Max.Y > (double) this.Max.Y) || ((double) this.Min.Z > (double) box.Min.Z || (double) box.Max.Z > (double) this.Max.Z) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(BoundingFrustum frustum)
    {
      if (!frustum.Intersects(this))
        return ContainmentType.Disjoint;
      foreach (Vector3 corner in frustum.cornerArray)
      {
        if (this.Contains(corner) == ContainmentType.Disjoint)
          return ContainmentType.Intersects;
      }
      return ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector3 point) => (double) this.Min.X <= (double) point.X && (double) point.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) point.Y && (double) point.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= (double) point.Z && (double) point.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public ContainmentType Contains(Vector3D point) => (double) this.Min.X <= point.X && point.X <= (double) this.Max.X && ((double) this.Min.Y <= point.Y && point.Y <= (double) this.Max.Y) && ((double) this.Min.Z <= point.Z && point.Z <= (double) this.Max.Z) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector3 point, out ContainmentType result) => result = (double) this.Min.X > (double) point.X || (double) point.X > (double) this.Max.X || ((double) this.Min.Y > (double) point.Y || (double) point.Y > (double) this.Max.Y) || ((double) this.Min.Z > (double) point.Z || (double) point.Z > (double) this.Max.Z) ? ContainmentType.Disjoint : ContainmentType.Contains;

    public ContainmentType Contains(BoundingSphere sphere)
    {
      Vector3 result1;
      Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref sphere.Center, ref result1, out result2);
      float radius = sphere.Radius;
      if ((double) result2 > (double) radius * (double) radius)
        return ContainmentType.Disjoint;
      return (double) this.Min.X + (double) radius <= (double) sphere.Center.X && (double) sphere.Center.X <= (double) this.Max.X - (double) radius && ((double) this.Max.X - (double) this.Min.X > (double) radius && (double) this.Min.Y + (double) radius <= (double) sphere.Center.Y) && ((double) sphere.Center.Y <= (double) this.Max.Y - (double) radius && (double) this.Max.Y - (double) this.Min.Y > (double) radius && ((double) this.Min.Z + (double) radius <= (double) sphere.Center.Z && (double) sphere.Center.Z <= (double) this.Max.Z - (double) radius)) && (double) this.Max.X - (double) this.Min.X > (double) radius ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
      Vector3 result1;
      Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out result1);
      float result2;
      Vector3.DistanceSquared(ref sphere.Center, ref result1, out result2);
      float radius = sphere.Radius;
      if ((double) result2 > (double) radius * (double) radius)
        result = ContainmentType.Disjoint;
      else
        result = (double) this.Min.X + (double) radius > (double) sphere.Center.X || (double) sphere.Center.X > (double) this.Max.X - (double) radius || ((double) this.Max.X - (double) this.Min.X <= (double) radius || (double) this.Min.Y + (double) radius > (double) sphere.Center.Y) || ((double) sphere.Center.Y > (double) this.Max.Y - (double) radius || (double) this.Max.Y - (double) this.Min.Y <= (double) radius || ((double) this.Min.Z + (double) radius > (double) sphere.Center.Z || (double) sphere.Center.Z > (double) this.Max.Z - (double) radius)) || (double) this.Max.X - (double) this.Min.X <= (double) radius ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    internal void SupportMapping(ref Vector3 v, out Vector3 result)
    {
      result.X = (double) v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = (double) v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
      result.Z = (double) v.Z >= 0.0 ? this.Max.Z : this.Min.Z;
    }

    public BoundingBox Translate(Matrix worldMatrix)
    {
      this.Min += worldMatrix.Translation;
      this.Max += worldMatrix.Translation;
      return this;
    }

    public BoundingBox Translate(Vector3 vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector3 Size => Vector3.Abs(this.Max - this.Min);

    public Matrix Matrix
    {
      get
      {
        Vector3 center = this.Center;
        Vector3 size = this.Size;
        Matrix result;
        Matrix.CreateTranslation(ref center, out result);
        Matrix.Rescale(ref result, ref size);
        return result;
      }
    }

    public BoundingBox Transform(Matrix worldMatrix) => this.Transform(ref worldMatrix);

    public BoundingBoxD Transform(MatrixD worldMatrix) => this.Transform(ref worldMatrix);

    public BoundingBox Transform(ref Matrix m)
    {
      BoundingBox invalid = BoundingBox.CreateInvalid();
      this.Transform(ref m, ref invalid);
      return invalid;
    }

    public void Transform(ref Matrix m, ref BoundingBox bb)
    {
      bb.Min = bb.Max = m.Translation;
      Vector3 min1 = m.Right * this.Min.X;
      Vector3 max1 = m.Right * this.Max.X;
      Vector3.MinMax(ref min1, ref max1);
      bb.Min += min1;
      bb.Max += max1;
      Vector3 min2 = m.Up * this.Min.Y;
      Vector3 max2 = m.Up * this.Max.Y;
      Vector3.MinMax(ref min2, ref max2);
      bb.Min += min2;
      bb.Max += max2;
      Vector3 min3 = m.Backward * this.Min.Z;
      Vector3 max3 = m.Backward * this.Max.Z;
      Vector3.MinMax(ref min3, ref max3);
      bb.Min += min3;
      bb.Max += max3;
    }

    public BoundingBoxD Transform(ref MatrixD m)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      this.Transform(ref m, ref invalid);
      return invalid;
    }

    public void Transform(ref MatrixD m, ref BoundingBoxD bb)
    {
      bb.Min = bb.Max = m.Translation;
      Vector3D min1 = m.Right * (double) this.Min.X;
      Vector3D max1 = m.Right * (double) this.Max.X;
      Vector3D.MinMax(ref min1, ref max1);
      bb.Min += min1;
      bb.Max += max1;
      Vector3D min2 = m.Up * (double) this.Min.Y;
      Vector3D max2 = m.Up * (double) this.Max.Y;
      Vector3D.MinMax(ref min2, ref max2);
      bb.Min += min2;
      bb.Max += max2;
      Vector3D min3 = m.Backward * (double) this.Min.Z;
      Vector3D max3 = m.Backward * (double) this.Max.Z;
      Vector3D.MinMax(ref min3, ref max3);
      bb.Min += min3;
      bb.Max += max3;
    }

    public BoundingBox Include(ref Vector3 point)
    {
      this.Min.X = Math.Min(point.X, this.Min.X);
      this.Min.Y = Math.Min(point.Y, this.Min.Y);
      this.Min.Z = Math.Min(point.Z, this.Min.Z);
      this.Max.X = Math.Max(point.X, this.Max.X);
      this.Max.Y = Math.Max(point.Y, this.Max.Y);
      this.Max.Z = Math.Max(point.Z, this.Max.Z);
      return this;
    }

    public BoundingBox GetIncluded(Vector3 point)
    {
      BoundingBox boundingBox = this;
      boundingBox.Include(point);
      return boundingBox;
    }

    public BoundingBox Include(Vector3 point) => this.Include(ref point);

    public BoundingBox Include(Vector3 p0, Vector3 p1, Vector3 p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBox Include(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBox Include(ref BoundingBox box)
    {
      this.Min = Vector3.Min(this.Min, box.Min);
      this.Max = Vector3.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBox Include(BoundingBox box) => this.Include(ref box);

    public void Include(ref Line line)
    {
      this.Include(ref line.From);
      this.Include(ref line.To);
    }

    public BoundingBox Include(BoundingSphere sphere) => this.Include(ref sphere);

    public BoundingBox Include(ref BoundingSphere sphere)
    {
      Vector3 vector3 = new Vector3(sphere.Radius);
      Vector3 result1 = sphere.Center;
      Vector3 result2 = sphere.Center;
      Vector3.Subtract(ref result1, ref vector3, out result1);
      Vector3.Add(ref result2, ref vector3, out result2);
      this.Include(ref result1);
      this.Include(ref result2);
      return this;
    }

    public unsafe BoundingBox Include(ref BoundingFrustum frustum)
    {
      Vector3* corners = stackalloc Vector3[8];
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

    public static BoundingBox CreateInvalid()
    {
      BoundingBox boundingBox = new BoundingBox();
      Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 vector3_2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      boundingBox.Min = vector3_1;
      boundingBox.Max = vector3_2;
      return boundingBox;
    }

    public float SurfaceArea()
    {
      Vector3 vector3 = this.Max - this.Min;
      return (float) (2.0 * ((double) vector3.X * (double) vector3.Y + (double) vector3.X * (double) vector3.Z + (double) vector3.Y * (double) vector3.Z));
    }

    public float Volume()
    {
      Vector3 vector3 = this.Max - this.Min;
      return vector3.X * vector3.Y * vector3.Z;
    }

    public float ProjectedArea(Vector3 viewDir)
    {
      Vector3 vector3 = this.Max - this.Min;
      Vector3 v = new Vector3(vector3.Y, vector3.Z, vector3.X) * new Vector3(vector3.Z, vector3.X, vector3.Y);
      return Vector3.Abs(viewDir).Dot(v);
    }

    public float Perimeter => (float) (4.0 * ((double) (this.Max.X - this.Min.X) + (double) (this.Max.Y - this.Min.Y) + (double) (this.Max.Z - this.Min.Z)));

    public void Inflate(float size)
    {
      this.Max += new Vector3(size);
      this.Min -= new Vector3(size);
    }

    public void Inflate(Vector3 size)
    {
      this.Max += size;
      this.Min -= size;
    }

    public void InflateToMinimum(Vector3 minimumSize)
    {
      Vector3 center = this.Center;
      if ((double) this.Size.X < (double) minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X * 0.5f;
        this.Max.X = center.X + minimumSize.X * 0.5f;
      }
      if ((double) this.Size.Y < (double) minimumSize.Y)
      {
        this.Min.Y = center.Y - minimumSize.Y * 0.5f;
        this.Max.Y = center.Y + minimumSize.Y * 0.5f;
      }
      if ((double) this.Size.Z >= (double) minimumSize.Z)
        return;
      this.Min.Z = center.Z - minimumSize.Z * 0.5f;
      this.Max.Z = center.Z + minimumSize.Z * 0.5f;
    }

    public void Scale(Vector3 scale)
    {
      Vector3 center = this.Center;
      Vector3 vector3 = this.HalfExtents * scale;
      this.Min = center - vector3;
      this.Max = center + vector3;
    }

    public BoundingBox Round(int decimals) => new BoundingBox(Vector3.Round(this.Min, decimals), Vector3.Round(this.Max, decimals));

    public BoundingBoxI Round() => new BoundingBoxI(Vector3D.Round((Vector3D) this.Min), Vector3D.Round((Vector3D) this.Max));

    public class ComparerType : IEqualityComparer<BoundingBoxD>
    {
      public bool Equals(BoundingBoxD x, BoundingBoxD y) => x.Min == y.Min && x.Max == y.Max;

      public int GetHashCode(BoundingBoxD obj) => obj.Min.GetHashCode() ^ obj.Max.GetHashCode();
    }

    protected class VRageMath_BoundingBox\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBox, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox owner, in Vector3 value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox owner, out Vector3 value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBox\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBox, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox owner, in Vector3 value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox owner, out Vector3 value) => value = owner.Max;
    }

    protected class VRageMath_BoundingBox\u003C\u003ECorners\u003C\u003EAccessor : IMemberAccessor<BoundingBox, BoxCornerEnumerator>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox owner, in BoxCornerEnumerator value) => owner.Corners = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox owner, out BoxCornerEnumerator value) => value = owner.Corners;
    }
  }
}
