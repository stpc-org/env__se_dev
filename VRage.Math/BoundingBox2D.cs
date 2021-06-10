// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBox2D
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
  public struct BoundingBox2D : IEquatable<BoundingBox2D>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector2D Min;
    [ProtoMember(4)]
    public Vector2D Max;

    public BoundingBox2D(Vector2D min, Vector2D max)
    {
      this.Min = min;
      this.Max = max;
    }

    public static bool operator ==(BoundingBox2D a, BoundingBox2D b) => a.Equals(b);

    public static bool operator !=(BoundingBox2D a, BoundingBox2D b) => a.Min != b.Min || a.Max != b.Max;

    public Vector2D[] GetCorners() => new Vector2D[8]
    {
      new Vector2D(this.Min.X, this.Max.Y),
      new Vector2D(this.Max.X, this.Max.Y),
      new Vector2D(this.Max.X, this.Min.Y),
      new Vector2D(this.Min.X, this.Min.Y),
      new Vector2D(this.Min.X, this.Max.Y),
      new Vector2D(this.Max.X, this.Max.Y),
      new Vector2D(this.Max.X, this.Min.Y),
      new Vector2D(this.Min.X, this.Min.Y)
    };

    public void GetCorners(Vector2D[] corners)
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

    public unsafe void GetCornersUnsafe(Vector2D* corners)
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

    public bool Equals(BoundingBox2D other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingBox2D other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format("Min:{0} Max:{1}", (object) this.Min, (object) this.Max);

    public static BoundingBox2D CreateMerged(
      BoundingBox2D original,
      BoundingBox2D additional)
    {
      BoundingBox2D boundingBox2D;
      Vector2D.Min(ref original.Min, ref additional.Min, out boundingBox2D.Min);
      Vector2D.Max(ref original.Max, ref additional.Max, out boundingBox2D.Max);
      return boundingBox2D;
    }

    public static void CreateMerged(
      ref BoundingBox2D original,
      ref BoundingBox2D additional,
      out BoundingBox2D result)
    {
      Vector2D result1;
      Vector2D.Min(ref original.Min, ref additional.Min, out result1);
      Vector2D result2;
      Vector2D.Max(ref original.Max, ref additional.Max, out result2);
      result.Min = result1;
      result.Max = result2;
    }

    public static BoundingBox2D CreateFromPoints(IEnumerable<Vector2D> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector2D result1 = new Vector2D(double.MaxValue);
      Vector2D result2 = new Vector2D(double.MinValue);
      foreach (Vector2D point in points)
      {
        Vector2D.Min(ref result1, ref point, out result1);
        Vector2D.Max(ref result2, ref point, out result2);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBox2D(result1, result2);
    }

    public static BoundingBox2D CreateFromHalfExtent(
      Vector2D center,
      double halfExtent)
    {
      return BoundingBox2D.CreateFromHalfExtent(center, new Vector2D(halfExtent));
    }

    public static BoundingBox2D CreateFromHalfExtent(
      Vector2D center,
      Vector2D halfExtent)
    {
      return new BoundingBox2D(center - halfExtent, center + halfExtent);
    }

    public BoundingBox2D Intersect(BoundingBox2D box)
    {
      BoundingBox2D boundingBox2D;
      boundingBox2D.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBox2D.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBox2D.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBox2D.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      return boundingBox2D;
    }

    public bool Intersects(BoundingBox2D box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBox2D box) => this.Max.X >= box.Min.X && this.Min.X <= box.Max.X && this.Max.Y >= box.Min.Y && this.Min.Y <= box.Max.Y;

    public void Intersects(ref BoundingBox2D box, out bool result)
    {
      result = false;
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y))
        return;
      result = true;
    }

    public Vector2D Center => (this.Min + this.Max) / 2.0;

    public Vector2D HalfExtents => (this.Max - this.Min) / 2.0;

    public Vector2D Extents => this.Max - this.Min;

    public double Width => this.Max.X - this.Min.X;

    public double Height => this.Max.Y - this.Min.Y;

    public double Distance(Vector2D point) => Vector2D.Distance(Vector2D.Clamp(point, this.Min, this.Max), point);

    public ContainmentType Contains(BoundingBox2D box)
    {
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y))
        return ContainmentType.Disjoint;
      return this.Min.X <= box.Min.X && box.Max.X <= this.Max.X && (this.Min.Y <= box.Min.Y && box.Max.Y <= this.Max.Y) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox2D box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if (this.Max.X < box.Min.X || this.Min.X > box.Max.X || (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y))
        return;
      result = this.Min.X > box.Min.X || box.Max.X > this.Max.X || (this.Min.Y > box.Min.Y || box.Max.Y > this.Max.Y) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector2D point) => this.Min.X <= point.X && point.X <= this.Max.X && (this.Min.Y <= point.Y && point.Y <= this.Max.Y) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector2D point, out ContainmentType result) => result = this.Min.X > point.X || point.X > this.Max.X || (this.Min.Y > point.Y || point.Y > this.Max.Y) ? ContainmentType.Disjoint : ContainmentType.Contains;

    internal void SupportMapping(ref Vector2D v, out Vector2D result)
    {
      result.X = v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
    }

    public BoundingBox2D Translate(Vector2D vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector2D Size => this.Max - this.Min;

    public BoundingBox2D Include(ref Vector2D point)
    {
      this.Min.X = Math.Min(point.X, this.Min.X);
      this.Min.Y = Math.Min(point.Y, this.Min.Y);
      this.Max.X = Math.Max(point.X, this.Max.X);
      this.Max.Y = Math.Max(point.Y, this.Max.Y);
      return this;
    }

    public BoundingBox2D GetIncluded(Vector2D point)
    {
      BoundingBox2D boundingBox2D = this;
      boundingBox2D.Include(point);
      return boundingBox2D;
    }

    public BoundingBox2D Include(Vector2D point) => this.Include(ref point);

    public BoundingBox2D Include(Vector2D p0, Vector2D p1, Vector2D p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBox2D Include(ref Vector2D p0, ref Vector2D p1, ref Vector2D p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBox2D Include(ref BoundingBox2D box)
    {
      this.Min = Vector2D.Min(this.Min, box.Min);
      this.Max = Vector2D.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBox2D Include(BoundingBox2D box) => this.Include(ref box);

    public static BoundingBox2D CreateInvalid()
    {
      BoundingBox2D boundingBox2D = new BoundingBox2D();
      Vector2D vector2D1 = new Vector2D(double.MaxValue, double.MaxValue);
      Vector2D vector2D2 = new Vector2D(double.MinValue, double.MinValue);
      boundingBox2D.Min = vector2D1;
      boundingBox2D.Max = vector2D2;
      return boundingBox2D;
    }

    public double Perimeter()
    {
      Vector2D vector2D = this.Max - this.Min;
      return 2.0 * (vector2D.X = vector2D.Y);
    }

    public double Area()
    {
      Vector2D vector2D = this.Max - this.Min;
      return vector2D.X * vector2D.Y;
    }

    public void Inflate(double size)
    {
      this.Max += new Vector2D(size);
      this.Min -= new Vector2D(size);
    }

    public void InflateToMinimum(Vector2D minimumSize)
    {
      Vector2D center = this.Center;
      if (this.Size.X < minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X / 2.0;
        this.Max.X = center.X + minimumSize.X / 2.0;
      }
      if (this.Size.Y >= minimumSize.Y)
        return;
      this.Min.Y = center.Y - minimumSize.Y / 2.0;
      this.Max.Y = center.Y + minimumSize.Y / 2.0;
    }

    public void Scale(Vector2D scale)
    {
      Vector2D center = this.Center;
      Vector2D vector2D = this.HalfExtents * scale;
      this.Min = center - vector2D;
      this.Max = center + vector2D;
    }

    protected class VRageMath_BoundingBox2D\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBox2D, Vector2D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2D owner, in Vector2D value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2D owner, out Vector2D value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBox2D\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBox2D, Vector2D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2D owner, in Vector2D value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2D owner, out Vector2D value) => value = owner.Max;
    }
  }
}
