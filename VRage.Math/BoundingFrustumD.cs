// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingFrustumD
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
  public class BoundingFrustumD : IEquatable<BoundingFrustumD>
  {
    private readonly PlaneD[] m_planes = new PlaneD[6];
    internal readonly Vector3D[] CornerArray = new Vector3D[8];
    public const int CornerCount = 8;
    private const int NearPlaneIndex = 0;
    private const int FarPlaneIndex = 1;
    private const int LeftPlaneIndex = 2;
    private const int RightPlaneIndex = 3;
    private const int TopPlaneIndex = 4;
    private const int BottomPlaneIndex = 5;
    private const int NumPlanes = 6;
    private MatrixD matrix;
    private GjkD gjk;

    public PlaneD this[int index] => this.m_planes[index];

    public PlaneD Near => this.m_planes[0];

    public PlaneD Far => this.m_planes[1];

    public PlaneD Left => this.m_planes[2];

    public PlaneD Right => this.m_planes[3];

    public PlaneD Top => this.m_planes[4];

    public PlaneD Bottom => this.m_planes[5];

    public MatrixD Matrix
    {
      get => this.matrix;
      set => this.SetMatrix(ref value);
    }

    public BoundingFrustumD()
    {
    }

    public BoundingFrustumD(MatrixD value) => this.SetMatrix(ref value);

    public static bool operator ==(BoundingFrustumD a, BoundingFrustumD b) => object.Equals((object) a, (object) b);

    public static bool operator !=(BoundingFrustumD a, BoundingFrustumD b) => !object.Equals((object) a, (object) b);

    public Vector3D[] GetCorners() => (Vector3D[]) this.CornerArray.Clone();

    public void GetCorners(Vector3D[] corners) => this.CornerArray.CopyTo((Array) corners, 0);

    public unsafe void GetCornersUnsafe(Vector3D* corners)
    {
      *corners = this.CornerArray[0];
      corners[1] = this.CornerArray[1];
      corners[2] = this.CornerArray[2];
      corners[3] = this.CornerArray[3];
      corners[4] = this.CornerArray[4];
      corners[5] = this.CornerArray[5];
      corners[6] = this.CornerArray[6];
      corners[7] = this.CornerArray[7];
    }

    public bool Equals(BoundingFrustumD other) => !(other == (BoundingFrustumD) null) && this.matrix == other.matrix;

    public override bool Equals(object obj)
    {
      bool flag = false;
      BoundingFrustumD boundingFrustumD = obj as BoundingFrustumD;
      if (boundingFrustumD != (BoundingFrustumD) null)
        flag = this.matrix == boundingFrustumD.matrix;
      return flag;
    }

    public override int GetHashCode() => this.matrix.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Near:{0} Far:{1} Left:{2} Right:{3} Top:{4} Bottom:{5}}}", (object) this.Near.ToString(), (object) this.Far.ToString(), (object) this.Left.ToString(), (object) this.Right.ToString(), (object) this.Top.ToString(), (object) this.Bottom.ToString());

    private void SetMatrix(ref MatrixD value)
    {
      this.matrix = value;
      this.m_planes[2].Normal.X = -value.M14 - value.M11;
      this.m_planes[2].Normal.Y = -value.M24 - value.M21;
      this.m_planes[2].Normal.Z = -value.M34 - value.M31;
      this.m_planes[2].D = -value.M44 - value.M41;
      this.m_planes[3].Normal.X = -value.M14 + value.M11;
      this.m_planes[3].Normal.Y = -value.M24 + value.M21;
      this.m_planes[3].Normal.Z = -value.M34 + value.M31;
      this.m_planes[3].D = -value.M44 + value.M41;
      this.m_planes[4].Normal.X = -value.M14 + value.M12;
      this.m_planes[4].Normal.Y = -value.M24 + value.M22;
      this.m_planes[4].Normal.Z = -value.M34 + value.M32;
      this.m_planes[4].D = -value.M44 + value.M42;
      this.m_planes[5].Normal.X = -value.M14 - value.M12;
      this.m_planes[5].Normal.Y = -value.M24 - value.M22;
      this.m_planes[5].Normal.Z = -value.M34 - value.M32;
      this.m_planes[5].D = -value.M44 - value.M42;
      this.m_planes[0].Normal.X = -value.M13;
      this.m_planes[0].Normal.Y = -value.M23;
      this.m_planes[0].Normal.Z = -value.M33;
      this.m_planes[0].D = -value.M43;
      this.m_planes[1].Normal.X = -value.M14 + value.M13;
      this.m_planes[1].Normal.Y = -value.M24 + value.M23;
      this.m_planes[1].Normal.Z = -value.M34 + value.M33;
      this.m_planes[1].D = -value.M44 + value.M43;
      for (int index = 0; index < 6; ++index)
      {
        double num = this.m_planes[index].Normal.Length();
        this.m_planes[index].Normal /= num;
        this.m_planes[index].D /= num;
      }
      RayD intersectionLine1 = BoundingFrustumD.ComputeIntersectionLine(ref this.m_planes[0], ref this.m_planes[2]);
      this.CornerArray[0] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[4], ref intersectionLine1);
      this.CornerArray[3] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[5], ref intersectionLine1);
      RayD intersectionLine2 = BoundingFrustumD.ComputeIntersectionLine(ref this.m_planes[3], ref this.m_planes[0]);
      this.CornerArray[1] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[4], ref intersectionLine2);
      this.CornerArray[2] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[5], ref intersectionLine2);
      RayD intersectionLine3 = BoundingFrustumD.ComputeIntersectionLine(ref this.m_planes[2], ref this.m_planes[1]);
      this.CornerArray[4] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[4], ref intersectionLine3);
      this.CornerArray[7] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[5], ref intersectionLine3);
      RayD intersectionLine4 = BoundingFrustumD.ComputeIntersectionLine(ref this.m_planes[1], ref this.m_planes[3]);
      this.CornerArray[5] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[4], ref intersectionLine4);
      this.CornerArray[6] = BoundingFrustumD.ComputeIntersection(ref this.m_planes[5], ref intersectionLine4);
    }

    private static RayD ComputeIntersectionLine(ref PlaneD p1, ref PlaneD p2)
    {
      RayD rayD = new RayD();
      rayD.Direction = Vector3D.Cross(p1.Normal, p2.Normal);
      double num = rayD.Direction.LengthSquared();
      rayD.Position = Vector3D.Cross(-p1.D * p2.Normal + p2.D * p1.Normal, rayD.Direction) / num;
      return rayD;
    }

    private static Vector3D ComputeIntersection(ref PlaneD plane, ref RayD ray)
    {
      double num = (-plane.D - Vector3D.Dot(plane.Normal, ray.Position)) / Vector3D.Dot(plane.Normal, ray.Direction);
      return ray.Position + ray.Direction * num;
    }

    public bool Intersects(BoundingBoxD box)
    {
      bool result;
      this.Intersects(ref box, out result);
      return result;
    }

    public void Intersects(ref BoundingBoxD box, out bool result)
    {
      if (this.gjk == null)
        this.gjk = new GjkD();
      this.gjk.Reset();
      Vector3D result1;
      Vector3D.Subtract(ref this.CornerArray[0], ref box.Min, out result1);
      if (result1.LengthSquared() < 9.99999974737875E-06)
        Vector3D.Subtract(ref this.CornerArray[0], ref box.Max, out result1);
      double num1 = double.MaxValue;
      result = false;
      double num2;
      do
      {
        Vector3D v;
        v.X = -result1.X;
        v.Y = -result1.Y;
        v.Z = -result1.Z;
        Vector3D result2;
        this.SupportMapping(ref v, out result2);
        Vector3D result3;
        box.SupportMapping(ref result1, out result3);
        Vector3D result4;
        Vector3D.Subtract(ref result2, ref result3, out result4);
        if (result1.X * result4.X + result1.Y * result4.Y + result1.Z * result4.Z > 0.0)
          return;
        this.gjk.AddSupportPoint(ref result4);
        result1 = this.gjk.ClosestPoint;
        double num3 = num1;
        num1 = result1.LengthSquared();
        if (num3 - num1 <= 9.99999974737875E-06 * num3)
          return;
        num2 = 3.9999998989515E-05 * this.gjk.MaxLengthSquared;
      }
      while (!this.gjk.FullSimplex && num1 >= num2);
      result = true;
    }

    public bool Intersects(BoundingFrustumD frustum)
    {
      if (frustum == (BoundingFrustumD) null)
        throw new ArgumentNullException(nameof (frustum));
      if (this.gjk == null)
        this.gjk = new GjkD();
      this.gjk.Reset();
      Vector3D result1;
      Vector3D.Subtract(ref this.CornerArray[0], ref frustum.CornerArray[0], out result1);
      if (result1.LengthSquared() < 9.99999974737875E-06)
        Vector3D.Subtract(ref this.CornerArray[0], ref frustum.CornerArray[1], out result1);
      double num1 = double.MaxValue;
      double num2;
      do
      {
        Vector3D v;
        v.X = -result1.X;
        v.Y = -result1.Y;
        v.Z = -result1.Z;
        Vector3D result2;
        this.SupportMapping(ref v, out result2);
        Vector3D result3;
        frustum.SupportMapping(ref result1, out result3);
        Vector3D result4;
        Vector3D.Subtract(ref result2, ref result3, out result4);
        if (result1.X * result4.X + result1.Y * result4.Y + result1.Z * result4.Z > 0.0)
          return false;
        this.gjk.AddSupportPoint(ref result4);
        result1 = this.gjk.ClosestPoint;
        double num3 = num1;
        num1 = result1.LengthSquared();
        num2 = 3.9999998989515E-05 * this.gjk.MaxLengthSquared;
        if (num3 - num1 <= 9.99999974737875E-06 * num3)
          return false;
      }
      while (!this.gjk.FullSimplex && num1 >= num2);
      return true;
    }

    public PlaneIntersectionType Intersects(PlaneD plane)
    {
      int num = 0;
      for (int index = 0; index < 8; ++index)
      {
        double result;
        Vector3D.Dot(ref this.CornerArray[index], ref plane.Normal, out result);
        if (result + plane.D > 0.0)
          num |= 1;
        else
          num |= 2;
        if (num == 3)
          return PlaneIntersectionType.Intersecting;
      }
      return num == 1 ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
    }

    public void Intersects(ref PlaneD plane, out PlaneIntersectionType result)
    {
      int num = 0;
      for (int index = 0; index < 8; ++index)
      {
        double result1;
        Vector3D.Dot(ref this.CornerArray[index], ref plane.Normal, out result1);
        if (result1 + plane.D > 0.0)
          num |= 1;
        else
          num |= 2;
        if (num == 3)
        {
          result = PlaneIntersectionType.Intersecting;
          return;
        }
      }
      result = num == 1 ? PlaneIntersectionType.Front : PlaneIntersectionType.Back;
    }

    public double? Intersects(RayD ray)
    {
      double? result;
      this.Intersects(ref ray, out result);
      return result;
    }

    public void Intersects(ref RayD ray, out double? result)
    {
      ContainmentType result1;
      this.Contains(ref ray.Position, out result1);
      if (result1 == ContainmentType.Contains)
      {
        result = new double?(0.0);
      }
      else
      {
        double num1 = double.MinValue;
        double num2 = double.MaxValue;
        result = new double?();
        foreach (PlaneD plane in this.m_planes)
        {
          Vector3D normal = plane.Normal;
          double result2;
          Vector3D.Dot(ref ray.Direction, ref normal, out result2);
          double result3;
          Vector3D.Dot(ref ray.Position, ref normal, out result3);
          result3 += plane.D;
          if (Math.Abs(result2) < 9.99999974737875E-06)
          {
            if (result3 > 0.0)
              return;
          }
          else
          {
            double num3 = -result3 / result2;
            if (result2 < 0.0)
            {
              if (num3 > num2)
                return;
              if (num3 > num1)
                num1 = num3;
            }
            else
            {
              if (num3 < num1)
                return;
              if (num3 < num2)
                num2 = num3;
            }
          }
        }
        double num4 = num1 >= 0.0 ? num1 : num2;
        if (num4 < 0.0)
          return;
        result = new double?(num4);
      }
    }

    public bool Intersects(BoundingSphereD sphere)
    {
      bool result;
      this.Intersects(ref sphere, out result);
      return result;
    }

    public void Intersects(ref BoundingSphereD sphere, out bool result)
    {
      if (this.gjk == null)
        this.gjk = new GjkD();
      this.gjk.Reset();
      Vector3D result1;
      Vector3D.Subtract(ref this.CornerArray[0], ref sphere.Center, out result1);
      if (result1.LengthSquared() < 9.99999974737875E-06)
        result1 = Vector3D.UnitX;
      double num1 = double.MaxValue;
      result = false;
      double num2;
      do
      {
        Vector3D v;
        v.X = -result1.X;
        v.Y = -result1.Y;
        v.Z = -result1.Z;
        Vector3D result2;
        this.SupportMapping(ref v, out result2);
        Vector3D result3;
        sphere.SupportMapping(ref result1, out result3);
        Vector3D result4;
        Vector3D.Subtract(ref result2, ref result3, out result4);
        if (result1.X * result4.X + result1.Y * result4.Y + result1.Z * result4.Z > 0.0)
          return;
        this.gjk.AddSupportPoint(ref result4);
        result1 = this.gjk.ClosestPoint;
        double num3 = num1;
        num1 = result1.LengthSquared();
        if (num3 - num1 <= 9.99999974737875E-06 * num3)
          return;
        num2 = 3.9999998989515E-05 * this.gjk.MaxLengthSquared;
      }
      while (!this.gjk.FullSimplex && num1 >= num2);
      result = true;
    }

    public ContainmentType Contains(BoundingBoxD box)
    {
      bool flag = false;
      foreach (PlaneD plane in this.m_planes)
      {
        switch (box.Intersects(plane))
        {
          case PlaneIntersectionType.Front:
            return ContainmentType.Disjoint;
          case PlaneIntersectionType.Intersecting:
            flag = true;
            break;
        }
      }
      return flag ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public void Contains(ref BoundingBoxD box, out ContainmentType result)
    {
      bool flag = false;
      foreach (PlaneD plane in this.m_planes)
      {
        switch (box.Intersects(plane))
        {
          case PlaneIntersectionType.Front:
            result = ContainmentType.Disjoint;
            return;
          case PlaneIntersectionType.Intersecting:
            flag = true;
            break;
        }
      }
      result = flag ? ContainmentType.Intersects : ContainmentType.Contains;
    }

    public ContainmentType Contains(BoundingFrustumD frustum)
    {
      if (frustum == (BoundingFrustumD) null)
        throw new ArgumentNullException(nameof (frustum));
      ContainmentType containmentType = ContainmentType.Disjoint;
      if (this.Intersects(frustum))
      {
        containmentType = ContainmentType.Contains;
        for (int index = 0; index < this.CornerArray.Length; ++index)
        {
          if (this.Contains(frustum.CornerArray[index]) == ContainmentType.Disjoint)
          {
            containmentType = ContainmentType.Intersects;
            break;
          }
        }
      }
      return containmentType;
    }

    public ContainmentType Contains(Vector3D point)
    {
      foreach (PlaneD plane in this.m_planes)
      {
        if (plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z + plane.D > 9.99999974737875E-06)
          return ContainmentType.Disjoint;
      }
      return ContainmentType.Contains;
    }

    public void Contains(ref Vector3D point, out ContainmentType result)
    {
      foreach (PlaneD plane in this.m_planes)
      {
        if (plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z + plane.D > 9.99999974737875E-06)
        {
          result = ContainmentType.Disjoint;
          return;
        }
      }
      result = ContainmentType.Contains;
    }

    public ContainmentType Contains(BoundingSphereD sphere)
    {
      Vector3D center = sphere.Center;
      double radius = sphere.Radius;
      int num1 = 0;
      foreach (PlaneD plane in this.m_planes)
      {
        double num2 = plane.Normal.X * center.X + plane.Normal.Y * center.Y + plane.Normal.Z * center.Z + plane.D;
        if (num2 > radius)
          return ContainmentType.Disjoint;
        if (num2 < -radius)
          ++num1;
      }
      return num1 == 6 ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
    {
      Vector3D center = sphere.Center;
      double radius = sphere.Radius;
      int num1 = 0;
      foreach (PlaneD plane in this.m_planes)
      {
        double num2 = plane.Normal.X * center.X + plane.Normal.Y * center.Y + plane.Normal.Z * center.Z + plane.D;
        if (num2 > radius)
        {
          result = ContainmentType.Disjoint;
          return;
        }
        if (num2 < -radius)
          ++num1;
      }
      result = num1 == 6 ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    internal void SupportMapping(ref Vector3D v, out Vector3D result)
    {
      int index1 = 0;
      double result1;
      Vector3D.Dot(ref this.CornerArray[0], ref v, out result1);
      for (int index2 = 1; index2 < this.CornerArray.Length; ++index2)
      {
        double result2;
        Vector3D.Dot(ref this.CornerArray[index2], ref v, out result2);
        if (result2 > result1)
        {
          index1 = index2;
          result1 = result2;
        }
      }
      result = this.CornerArray[index1];
    }

    protected class VRageMath_BoundingFrustumD\u003C\u003Em_planes\u003C\u003EAccessor : IMemberAccessor<BoundingFrustumD, PlaneD[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingFrustumD owner, in PlaneD[] value) => owner.m_planes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingFrustumD owner, out PlaneD[] value) => value = owner.m_planes;
    }

    protected class VRageMath_BoundingFrustumD\u003C\u003ECornerArray\u003C\u003EAccessor : IMemberAccessor<BoundingFrustumD, Vector3D[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingFrustumD owner, in Vector3D[] value) => owner.CornerArray = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingFrustumD owner, out Vector3D[] value) => value = owner.CornerArray;
    }

    protected class VRageMath_BoundingFrustumD\u003C\u003Ematrix\u003C\u003EAccessor : IMemberAccessor<BoundingFrustumD, MatrixD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingFrustumD owner, in MatrixD value) => owner.matrix = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingFrustumD owner, out MatrixD value) => value = owner.matrix;
    }

    protected class VRageMath_BoundingFrustumD\u003C\u003Egjk\u003C\u003EAccessor : IMemberAccessor<BoundingFrustumD, GjkD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingFrustumD owner, in GjkD value) => owner.gjk = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingFrustumD owner, out GjkD value) => value = owner.gjk;
    }

    protected class VRageMath_BoundingFrustumD\u003C\u003EMatrix\u003C\u003EAccessor : IMemberAccessor<BoundingFrustumD, MatrixD>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoundingFrustumD owner, in MatrixD value) => owner.Matrix = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoundingFrustumD owner, out MatrixD value) => value = owner.Matrix;
    }
  }
}
