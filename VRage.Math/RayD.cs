// Decompiled with JetBrains decompiler
// Type: VRageMath.RayD
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
  public struct RayD : IEquatable<RayD>
  {
    public Vector3D Position;
    public Vector3D Direction;

    public RayD(Vector3D position, Vector3D direction)
    {
      this.Position = position;
      this.Direction = direction;
    }

    public RayD(ref Vector3D position, ref Vector3D direction)
    {
      this.Position = position;
      this.Direction = direction;
    }

    public static bool operator ==(RayD a, RayD b) => a.Position.X == b.Position.X && a.Position.Y == b.Position.Y && (a.Position.Z == b.Position.Z && a.Direction.X == b.Direction.X) && a.Direction.Y == b.Direction.Y && a.Direction.Z == b.Direction.Z;

    public static bool operator !=(RayD a, RayD b) => a.Position.X != b.Position.X || a.Position.Y != b.Position.Y || (a.Position.Z != b.Position.Z || a.Direction.X != b.Direction.X) || a.Direction.Y != b.Direction.Y || a.Direction.Z != b.Direction.Z;

    public bool Equals(RayD other) => this.Position.X == other.Position.X && this.Position.Y == other.Position.Y && (this.Position.Z == other.Position.Z && this.Direction.X == other.Direction.X) && this.Direction.Y == other.Direction.Y && this.Direction.Z == other.Direction.Z;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj != null && obj is RayD other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.Position.GetHashCode() + this.Direction.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Position:{0} Direction:{1}}}", new object[2]
    {
      (object) this.Position.ToString(),
      (object) this.Direction.ToString()
    });

    public double? Intersects(BoundingBoxD box) => box.Intersects(this);

    public void Intersects(ref BoundingBoxD box, out double? result) => box.Intersects(ref this, out result);

    public double? Intersects(BoundingFrustumD frustum) => !(frustum == (BoundingFrustumD) null) ? frustum.Intersects(this) : throw new ArgumentNullException(nameof (frustum));

    public double? Intersects(PlaneD plane)
    {
      double num1 = plane.Normal.X * this.Direction.X + plane.Normal.Y * this.Direction.Y + plane.Normal.Z * this.Direction.Z;
      if (Math.Abs(num1) < 9.99999974737875E-06)
        return new double?();
      double num2 = plane.Normal.X * this.Position.X + plane.Normal.Y * this.Position.Y + plane.Normal.Z * this.Position.Z;
      double num3 = (-plane.D - num2) / num1;
      if (num3 < 0.0)
      {
        if (num3 < -9.99999974737875E-06)
          return new double?();
        num3 = 0.0;
      }
      return new double?(num3);
    }

    public void Intersects(ref PlaneD plane, out double? result)
    {
      double num1 = plane.Normal.X * this.Direction.X + plane.Normal.Y * this.Direction.Y + plane.Normal.Z * this.Direction.Z;
      if (Math.Abs(num1) < 9.99999974737875E-06)
      {
        result = new double?();
      }
      else
      {
        double num2 = plane.Normal.X * this.Position.X + plane.Normal.Y * this.Position.Y + plane.Normal.Z * this.Position.Z;
        double num3 = (-plane.D - num2) / num1;
        if (num3 < 0.0)
        {
          if (num3 < -9.99999974737875E-06)
          {
            result = new double?();
            return;
          }
          result = new double?(0.0);
        }
        result = new double?(num3);
      }
    }

    public double? Intersects(BoundingSphereD sphere)
    {
      double num1 = sphere.Center.X - this.Position.X;
      double num2 = sphere.Center.Y - this.Position.Y;
      double num3 = sphere.Center.Z - this.Position.Z;
      double num4 = num1 * num1 + num2 * num2 + num3 * num3;
      double num5 = sphere.Radius * sphere.Radius;
      if (num4 <= num5)
        return new double?(0.0);
      double num6 = num1 * this.Direction.X + num2 * this.Direction.Y + num3 * this.Direction.Z;
      if (num6 < 0.0)
        return new double?();
      double num7 = num4 - num6 * num6;
      if (num7 > num5)
        return new double?();
      double num8 = Math.Sqrt(num5 - num7);
      return new double?(num6 - num8);
    }

    public void Intersects(ref BoundingSphere sphere, out double? result)
    {
      double num1 = (double) sphere.Center.X - this.Position.X;
      double num2 = (double) sphere.Center.Y - this.Position.Y;
      double num3 = (double) sphere.Center.Z - this.Position.Z;
      double num4 = num1 * num1 + num2 * num2 + num3 * num3;
      double num5 = (double) sphere.Radius * (double) sphere.Radius;
      if (num4 <= num5)
      {
        result = new double?(0.0);
      }
      else
      {
        result = new double?();
        double num6 = num1 * this.Direction.X + num2 * this.Direction.Y + num3 * this.Direction.Z;
        if (num6 < 0.0)
          return;
        double num7 = num4 - num6 * num6;
        if (num7 > num5)
          return;
        double num8 = Math.Sqrt(num5 - num7);
        result = new double?(num6 - num8);
      }
    }

    protected class VRageMath_RayD\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<RayD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RayD owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RayD owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRageMath_RayD\u003C\u003EDirection\u003C\u003EAccessor : IMemberAccessor<RayD, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RayD owner, in Vector3D value) => owner.Direction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RayD owner, out Vector3D value) => value = owner.Direction;
    }
  }
}
