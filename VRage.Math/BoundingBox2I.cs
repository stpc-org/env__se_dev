// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingBox2I
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
  public struct BoundingBox2I : IEquatable<BoundingBox2I>
  {
    public const int CornerCount = 8;
    [ProtoMember(1)]
    public Vector2I Min;
    [ProtoMember(4)]
    public Vector2I Max;

    public BoundingBox2I(Vector2I min, Vector2I max)
    {
      this.Min = min;
      this.Max = max;
    }

    public static bool operator ==(BoundingBox2I a, BoundingBox2I b) => a.Equals(b);

    public static bool operator !=(BoundingBox2I a, BoundingBox2I b) => a.Min != b.Min || a.Max != b.Max;

    public Vector2I[] GetCorners() => new Vector2I[8]
    {
      new Vector2I(this.Min.X, this.Max.Y),
      new Vector2I(this.Max.X, this.Max.Y),
      new Vector2I(this.Max.X, this.Min.Y),
      new Vector2I(this.Min.X, this.Min.Y),
      new Vector2I(this.Min.X, this.Max.Y),
      new Vector2I(this.Max.X, this.Max.Y),
      new Vector2I(this.Max.X, this.Min.Y),
      new Vector2I(this.Min.X, this.Min.Y)
    };

    public void GetCorners(Vector2I[] corners)
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

    public unsafe void GetCornersUnsafe(Vector2I* corners)
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

    public bool Equals(BoundingBox2I other) => this.Min == other.Min && this.Max == other.Max;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is BoundingBox2I other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Min.GetHashCode() + this.Max.GetHashCode();

    public override string ToString() => string.Format("Min:{0} Max:{1}", (object) this.Min, (object) this.Max);

    public static BoundingBox2I CreateMerged(
      BoundingBox2I original,
      BoundingBox2I additional)
    {
      BoundingBox2I boundingBox2I;
      Vector2I.Min(ref original.Min, ref additional.Min, out boundingBox2I.Min);
      Vector2I.Max(ref original.Max, ref additional.Max, out boundingBox2I.Max);
      return boundingBox2I;
    }

    public static void CreateMerged(
      ref BoundingBox2I original,
      ref BoundingBox2I additional,
      out BoundingBox2I result)
    {
      Vector2I min;
      Vector2I.Min(ref original.Min, ref additional.Min, out min);
      Vector2I max;
      Vector2I.Max(ref original.Max, ref additional.Max, out max);
      result.Min = min;
      result.Max = max;
    }

    public static BoundingBox2I CreateFromPoints(IEnumerable<Vector2I> points)
    {
      if (points == null)
        throw new ArgumentNullException();
      bool flag = false;
      Vector2I min = new Vector2I(int.MaxValue);
      Vector2I max = new Vector2I(int.MinValue);
      foreach (Vector2I point in points)
      {
        Vector2I.Min(ref min, ref point, out min);
        Vector2I.Max(ref max, ref point, out max);
        flag = true;
      }
      if (!flag)
        throw new ArgumentException();
      return new BoundingBox2I(min, max);
    }

    public static BoundingBox2I CreateFromHalfExtent(Vector2I center, int halfExtent) => BoundingBox2I.CreateFromHalfExtent(center, new Vector2I(halfExtent));

    public static BoundingBox2I CreateFromHalfExtent(
      Vector2I center,
      Vector2I halfExtent)
    {
      return new BoundingBox2I(center - halfExtent, center + halfExtent);
    }

    public BoundingBox2I Intersect(BoundingBox2I box)
    {
      BoundingBox2I boundingBox2I;
      boundingBox2I.Min.X = Math.Max(this.Min.X, box.Min.X);
      boundingBox2I.Min.Y = Math.Max(this.Min.Y, box.Min.Y);
      boundingBox2I.Max.X = Math.Min(this.Max.X, box.Max.X);
      boundingBox2I.Max.Y = Math.Min(this.Max.Y, box.Max.Y);
      return boundingBox2I;
    }

    public bool Intersects(BoundingBox2I box) => this.Intersects(ref box);

    public bool Intersects(ref BoundingBox2I box) => (double) this.Max.X >= (double) box.Min.X && (double) this.Min.X <= (double) box.Max.X && (double) this.Max.Y >= (double) box.Min.Y && (double) this.Min.Y <= (double) box.Max.Y;

    public void Intersects(ref BoundingBox2I box, out bool result)
    {
      result = false;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return;
      result = true;
    }

    public Vector2I Center => (this.Min + this.Max) / 2;

    public Vector2I HalfExtents => (this.Max - this.Min) / 2;

    public Vector2I Extents => this.Max - this.Min;

    public float Width => (float) (this.Max.X - this.Min.X);

    public float Height => (float) (this.Max.Y - this.Min.Y);

    public ContainmentType Contains(BoundingBox2I box)
    {
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return ContainmentType.Disjoint;
      return (double) this.Min.X <= (double) box.Min.X && (double) box.Max.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) box.Min.Y && (double) box.Max.Y <= (double) this.Max.Y) ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingBox2I box, out ContainmentType result)
    {
      result = ContainmentType.Disjoint;
      if ((double) this.Max.X < (double) box.Min.X || (double) this.Min.X > (double) box.Max.X || ((double) this.Max.Y < (double) box.Min.Y || (double) this.Min.Y > (double) box.Max.Y))
        return;
      result = (double) this.Min.X > (double) box.Min.X || (double) box.Max.X > (double) this.Max.X || ((double) this.Min.Y > (double) box.Min.Y || (double) box.Max.Y > (double) this.Max.Y) ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(Vector2I point) => (double) this.Min.X <= (double) point.X && (double) point.X <= (double) this.Max.X && ((double) this.Min.Y <= (double) point.Y && (double) point.Y <= (double) this.Max.Y) ? ContainmentType.Contains : ContainmentType.Disjoint;

    public void Contains(ref Vector2I point, out ContainmentType result) => result = (double) this.Min.X > (double) point.X || (double) point.X > (double) this.Max.X || ((double) this.Min.Y > (double) point.Y || (double) point.Y > (double) this.Max.Y) ? ContainmentType.Disjoint : ContainmentType.Contains;

    internal void SupportMapping(ref Vector2I v, out Vector2I result)
    {
      result.X = (double) v.X >= 0.0 ? this.Max.X : this.Min.X;
      result.Y = (double) v.Y >= 0.0 ? this.Max.Y : this.Min.Y;
    }

    public BoundingBox2I Translate(Vector2I vctTranlsation)
    {
      this.Min += vctTranlsation;
      this.Max += vctTranlsation;
      return this;
    }

    public Vector2I Size => this.Max - this.Min;

    public BoundingBox2I Include(ref Vector2I point)
    {
      this.Min.X = Math.Min(point.X, this.Min.X);
      this.Min.Y = Math.Min(point.Y, this.Min.Y);
      this.Max.X = Math.Max(point.X, this.Max.X);
      this.Max.Y = Math.Max(point.Y, this.Max.Y);
      return this;
    }

    public BoundingBox2I GetIncluded(Vector2I point)
    {
      BoundingBox2I boundingBox2I = this;
      boundingBox2I.Include(point);
      return boundingBox2I;
    }

    public BoundingBox2I Include(Vector2I point) => this.Include(ref point);

    public BoundingBox2I Include(Vector2I p0, Vector2I p1, Vector2I p2) => this.Include(ref p0, ref p1, ref p2);

    public BoundingBox2I Include(ref Vector2I p0, ref Vector2I p1, ref Vector2I p2)
    {
      this.Include(ref p0);
      this.Include(ref p1);
      this.Include(ref p2);
      return this;
    }

    public BoundingBox2I Include(ref BoundingBox2I box)
    {
      this.Min = Vector2I.Min(this.Min, box.Min);
      this.Max = Vector2I.Max(this.Max, box.Max);
      return this;
    }

    public BoundingBox2I Include(BoundingBox2I box) => this.Include(ref box);

    public static BoundingBox2I CreateInvalid()
    {
      BoundingBox2I boundingBox2I = new BoundingBox2I();
      Vector2I vector2I1 = new Vector2I(int.MaxValue, int.MaxValue);
      Vector2I vector2I2 = new Vector2I(int.MinValue, int.MinValue);
      boundingBox2I.Min = vector2I1;
      boundingBox2I.Max = vector2I2;
      return boundingBox2I;
    }

    public float Perimeter()
    {
      Vector2I vector2I = this.Max - this.Min;
      return (float) (2 * (vector2I.X = vector2I.Y));
    }

    public float Area()
    {
      Vector2I vector2I = this.Max - this.Min;
      return (float) (vector2I.X * vector2I.Y);
    }

    public void Inflate(int size)
    {
      this.Max += new Vector2I(size);
      this.Min -= new Vector2I(size);
    }

    public void InflateToMinimum(Vector2I minimumSize)
    {
      Vector2I center = this.Center;
      if (this.Size.X < minimumSize.X)
      {
        this.Min.X = center.X - minimumSize.X / 2;
        this.Max.X = center.X + minimumSize.X / 2;
      }
      if (this.Size.Y >= minimumSize.Y)
        return;
      this.Min.Y = center.Y - minimumSize.Y / 2;
      this.Max.Y = center.Y + minimumSize.Y / 2;
    }

    public void Scale(Vector2I scale)
    {
      Vector2I center = this.Center;
      Vector2I halfExtents = this.HalfExtents;
      halfExtents.X *= scale.X;
      halfExtents.Y *= scale.Y;
      this.Min = center - halfExtents;
      this.Max = center + halfExtents;
    }

    protected class VRageMath_BoundingBox2I\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<BoundingBox2I, Vector2I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2I owner, in Vector2I value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2I owner, out Vector2I value) => value = owner.Min;
    }

    protected class VRageMath_BoundingBox2I\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<BoundingBox2I, Vector2I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingBox2I owner, in Vector2I value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingBox2I owner, out Vector2I value) => value = owner.Max;
    }
  }
}
