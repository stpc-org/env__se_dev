// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBox2
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct BoundingBox2 : IEquatable<BoundingBox2>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector2 Min;
    [ProtoMember(4)]
    public Vector2 Max;

    public BoundingBox2(Vector2 min, Vector2 max)
    {
      this.Min = min;
      this.Max = max;
    }

    public static bool operator ==(BoundingBox2 a, BoundingBox2 b) => a.Equals(b);

    public static bool operator !=(BoundingBox2 a, BoundingBox2 b) => a.Min != b.Min || a.Max != b.Max;

    public Vector2[] GetCorners() => new Vector2[8]
    {
      new Vector2(this.Min.X, this.Max.Y),
      new Vector2(this.Max.X, this.Max.Y),
      new Vector2(this.Max.X, this.Min.Y),
      new Vector2(this.Min.X, this.Min.Y),
      new Vector2(this.Min.X, this.Max.Y),
      new Vector2(this.Max.X, this.Max.Y),
      new Vector2(this.Max.X, this.Min.Y),
      new Vector2(this.Min.X, this.Min.Y)
    };

    public void GetCorners(Vector2[] corners)
    {
      corners[0].X = this.Min.X;
      corners[0].Y = this.Max.Y;
      corners[1].X = this.Max.X;
      corners[1].Y = this.Max.Y;
      corners[2].X = this.Max.X;
      corners[2].Y = this.Min.Y;
      corners[3].X = this.Min.X;
      corners[3].Y = this.Min.Y;
      corners[4].X = this.Min.X;
      corners[4].Y = this.Max.Y;
      corners[5].X = this.Max.X;
      corners[5].Y = this.Max.Y;
      corners[6].X = this.Max.X;
      corners[6].Y = this.Min.Y;
      corners[7].X = this.Min.X;
      corners[7].Y = this.Min.Y;
    }

    public unsafe void GetCornersUnsafe(Vector2* corners)
    {
      corners->X = this.Min.X;
      corners->Y = this.Max.Y;
      corners[1].X = this.Max.X;
      corners[1].Y = this.Max.Y;
      corners[2].X = this.Max.X;
      corners[2].Y = this.Min.Y;
      corners[3].X = this.Min.X;
      corners[3].Y = this.Min.Y;
      corners[4].X = this.Min.X;
      corners[4].Y = this.Max.Y;
      corners[5].X = this.Max.X;
      corners[5].Y = this.Max.Y;
      corners[6].X = this.Max.X;
      corners[6].Y = this.Min.Y;
      corners[7].X = this.Min.X;
      corners[7].Y = this.Min.Y;
    }

    public bool Equals(BoundingBox2 other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingBox2 other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format("Min:{0} Max:{1}", (object) this.Min, (object) this.Max);

    public static BoundingBox2 CreateMerged(
      BoundingBox2 original,
      BoundingBox2 additional)
    {
      BoundingBox2 boundingBox2;
      Vector2.Min(ref original.Min, ref additional.Min, out boundingBox2.Min);
      Vector2.Max(ref original.Max, ref additional.Max, out boundingBox2.Max);
      return boundingBox2;
    }

    public static void CreateMerged(
      ref BoundingBox2 original,
      ref BoundingBox2 additional,
      out BoundingBox2 result)
    {
      Vector2 result1;
      Vector2.Min(ref original.Min, ref additional.Min, out result1);
      Vector2 result2;
      Vector2.Max(ref original.Max, ref additional.Max, out result2);
      result.Min = result1;
      result.Max = result2;
    }

    public static BoundingBox2 CreateFromPoints(IEnumerable<Vector2> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector2 result1 = new Vector2(float.MaxValue);
      Vector2 result2 = new Vector2(float.MinValue);
      foreach (Vector2 point in points)
      {
        Vector2.Min(ref result1, ref point, out result1);
        Vector2.Max(ref result2, ref point, out result2);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBox2(result1, result2);
    }

    public static BoundingBox2 CreateFromHalfExtent(Vector2 center, float halfExtent) => BoundingBox2.CreateFromHalfExtent(center, new Vector2(halfExtent));

    public static BoundingBox2 CreateFromHalfExtent(Vector2 center, Vector2 halfExtent) => new BoundingBox2(center - halfExtent, center + halfExtent);

    public BoundingBox2 Intersect(BoundingBox2 box)
    {
      BoundingBox2 boundingBox2;
      boundingBox2.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBox2.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBox2.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBox2.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      return boundingBox2;
    }

    public bool Intersects(BoundingBox2 box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBox2 box) => (double) this.Max.X >= (double) box.Min.X && (double) this.Min.X <= (double) box.Max.X && (double) this.Max.Y >= (double) box.Min.Y && (double) this.Min.Y <= (double) box.Max.Y;

    public void Intersects(ref BoundingBox2 box, out bool result)
    {
      result = false;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return;
      result = true;
    }

    public Vector2 Center => (this.Min + this.Max) / 2f;

    public Vector2 HalfExtents => (this.Max - this.Min) / 2f;

    public Vector2 Extents => this.Max - this.Min;

    public float Width => this.Max.X - this.Min.X;

    public float Height => this.Max.Y - this.Min.Y;

    public float Distance(Vector2 point) => Vector2.Distance(Vector2.Clamp(point, this.Min, this.Max), point);

    public ContainmentType Contains(BoundingBox2 box)
    {
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return ContainmentType.Disjoint;
      return (double) this.Min.X <= (double) box.Min.X && (double) box.Max.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) box.Min.Y && (double) box.Max.Y <= (double) this.Max.Y) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox2 box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return;
      result = (double) this.Min.X > (double) box.Min.X || (double) box.Max.X > (double) this.Max.X || ((double) this.Min.Y > (double) box.Min.Y || (double) box.Max.Y > (double) this.Max.Y) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector2 point) => (double) this.Min.X <= (double) point.X && (double) point.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) point.Y && (double) point.Y <= (double) this.Max.Y) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector2 point, out ContainmentType result) => result = (double) this.Min.X > (double) point.X || (double) point.X > (double) this.Max.X || ((double) this.Min.Y > (double) point.Y || (double) point.Y > (double) this.Max.Y) ? ContainmentType.Disjoint : ContainmentType.Contains;

    internal void SupportMapping(ref Vector2 v, out Vector2 result)
    {
      result.X = (double) v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = (double) v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
    }

    public BoundingBox2 Translate(Vector2 vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector2 Size => this.Max - this.Min;

    public BoundingBox2 Include(ref Vector2 point)
    {
      this.Min.X = Math.Min(point.X, this.Min.X);
      this.Min.Y = Math.Min(point.Y, this.Min.Y);
      this.Max.X = Math.Max(point.X, this.Max.X);
      this.Max.Y = Math.Max(point.Y, this.Max.Y);
      return this;
    }

    public BoundingBox2 GetIncluded(Vector2 point)
    {
      BoundingBox2 boundingBox2 = this;
      boundingBox2.Include(point);
      return boundingBox2;
    }

    public BoundingBox2 Include(Vector2 point) => this.Include(ref point);

    public BoundingBox2 Include(Vector2 p0, Vector2 p1, Vector2 p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBox2 Include(ref Vector2 p0, ref Vector2 p1, ref Vector2 p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBox2 Include(ref BoundingBox2 box)
    {
      this.Min = Vector2.Min(this.Min, box.Min);
      this.Max = Vector2.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBox2 Include(BoundingBox2 box) => this.Include(ref box);

    public static BoundingBox2 CreateInvalid()
    {
      BoundingBox2 boundingBox2 = new BoundingBox2();
      Vector2 vector2_1 = new Vector2(float.MaxValue, float.MaxValue);
      Vector2 vector2_2 = new Vector2(float.MinValue, float.MinValue);
      boundingBox2.Min = vector2_1;
      boundingBox2.Max = vector2_2;
      return boundingBox2;
    }

    public float Perimeter()
    {
      Vector2 vector2 = this.Max - this.Min;
      return 2f * (vector2.X = vector2.Y);
    }

    public float Area()
    {
      Vector2 vector2 = this.Max - this.Min;
      return vector2.X * vector2.Y;
    }

    public void Inflate(float size)
    {
      this.Max += new Vector2(size);
      this.Min -= new Vector2(size);
    }

    public void InflateToMinimum(Vector2 minimumSize)
    {
      Vector2 center = this.Center;
      if ((double) this.Size.X < (double) minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X / 2f;
        this.Max.X = center.X + minimumSize.X / 2f;
      }
      if ((double) this.Size.Y >= (double) minimumSize.Y)
        return;
      this.Min.Y = center.Y - minimumSize.Y / 2f;
      this.Max.Y = center.Y + minimumSize.Y / 2f;
    }

    public void Scale(Vector2 scale)
    {
      Vector2 center = this.Center;
      Vector2 vector2 = this.HalfExtents * scale;
      this.Min = center - vector2;
      this.Max = center + vector2;
    }

    protected class VRageMath_BoundingBox2\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBox2, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2 owner, in Vector2 value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2 owner, out Vector2 value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBox2\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBox2, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2 owner, in Vector2 value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2 owner, out Vector2 value) => value = owner.Max;
    }
  }
}
