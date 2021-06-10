// Decompiled with JetBrains decompiler
// Type: VRageMath.MyOrientedBoundingBoxD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public struct MyOrientedBoundingBoxD : IEquatable<MyOrientedBoundingBox>
  {
    public const int CornerCount = 8;
    private const float RAY_EPSILON = 1E-20f;
    public static readonly int[] StartVertices = new int[12]
    {
      0,
      1,
      5,
      4,
      3,
      2,
      6,
      7,
      0,
      1,
      5,
      4
    };
    public static readonly int[] EndVertices = new int[12]
    {
      1,
      5,
      4,
      0,
      2,
      6,
      7,
      3,
      3,
      2,
      6,
      7
    };
    public static readonly int[] StartXVertices = new int[4]
    {
      0,
      4,
      7,
      3
    };
    public static readonly int[] EndXVertices = new int[4]
    {
      1,
      5,
      6,
      2
    };
    public static readonly int[] StartYVertices = new int[4]
    {
      0,
      1,
      5,
      4
    };
    public static readonly int[] EndYVertices = new int[4]
    {
      3,
      2,
      6,
      7
    };
    public static readonly int[] StartZVertices = new int[4]
    {
      0,
      3,
      2,
      1
    };
    public static readonly int[] EndZVertices = new int[4]
    {
      4,
      7,
      6,
      5
    };
    public static readonly Vector3[] XNeighbourVectorsBack = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, -1f, 0.0f)
    };
    public static readonly Vector3[] XNeighbourVectorsForw = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 1f, 0.0f)
    };
    public static readonly Vector3[] YNeighbourVectorsBack = new Vector3[4]
    {
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f)
    };
    public static readonly Vector3[] YNeighbourVectorsForw = new Vector3[4]
    {
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 1f)
    };
    public static readonly Vector3[] ZNeighbourVectorsBack = new Vector3[4]
    {
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f)
    };
    public static readonly Vector3[] ZNeighbourVectorsForw = new Vector3[4]
    {
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f)
    };
    public Vector3D Center;
    public Vector3D HalfExtent;
    public Quaternion Orientation;
    [ThreadStatic]
    private static Vector3D[] m_cornersTmp;

    public static bool GetNormalBetweenEdges(int axis, int edge0, int edge1, out Vector3 normal)
    {
      normal = Vector3.Zero;
      Vector3[] vector3Array1;
      Vector3[] vector3Array2;
      switch (axis)
      {
        case 0:
          int[] startXvertices = MyOrientedBoundingBoxD.StartXVertices;
          int[] endXvertices = MyOrientedBoundingBoxD.EndXVertices;
          vector3Array1 = MyOrientedBoundingBoxD.XNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBoxD.XNeighbourVectorsBack;
          break;
        case 1:
          int[] startYvertices = MyOrientedBoundingBoxD.StartYVertices;
          int[] endYvertices = MyOrientedBoundingBoxD.EndYVertices;
          vector3Array1 = MyOrientedBoundingBoxD.YNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBoxD.YNeighbourVectorsBack;
          break;
        case 2:
          int[] startZvertices = MyOrientedBoundingBoxD.StartZVertices;
          int[] endZvertices = MyOrientedBoundingBoxD.EndZVertices;
          vector3Array1 = MyOrientedBoundingBoxD.ZNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBoxD.ZNeighbourVectorsBack;
          break;
        default:
          return false;
      }
      if (edge0 == -1)
        edge0 = 3;
      if (edge0 == 4)
        edge0 = 0;
      if (edge1 == -1)
        edge1 = 3;
      if (edge1 == 4)
        edge1 = 0;
      if (edge0 == 3 && edge1 == 0)
      {
        normal = vector3Array1[3];
        return true;
      }
      if (edge0 == 0 && edge1 == 3)
      {
        normal = vector3Array2[3];
        return true;
      }
      if (edge0 + 1 == edge1)
      {
        normal = vector3Array1[edge0];
        return true;
      }
      if (edge0 != edge1 + 1)
        return false;
      normal = vector3Array2[edge1];
      return true;
    }

    public MyOrientedBoundingBoxD(MatrixD matrix)
    {
      this.Center = matrix.Translation;
      Vector3D vector3D1;
      ref Vector3D local = ref vector3D1;
      Vector3D vector3D2 = matrix.Right;
      double x = vector3D2.Length();
      vector3D2 = matrix.Up;
      double y = vector3D2.Length();
      vector3D2 = matrix.Forward;
      double z = vector3D2.Length();
      local = new Vector3D(x, y, z);
      this.HalfExtent = vector3D1 / 2.0;
      matrix.Right /= vector3D1.X;
      matrix.Up /= vector3D1.Y;
      matrix.Forward /= vector3D1.Z;
      Quaternion.CreateFromRotationMatrix(ref matrix, out this.Orientation);
    }

    public MyOrientedBoundingBoxD(Vector3D center, Vector3D halfExtents, Quaternion orientation)
    {
      this.Center = center;
      this.HalfExtent = halfExtents;
      this.Orientation = orientation;
    }

    public MyOrientedBoundingBoxD(BoundingBoxD box, MatrixD transform)
    {
      this.Center = (box.Min + box.Max) * 0.5;
      this.HalfExtent = (box.Max - box.Min) * 0.5;
      this.Center = Vector3D.Transform(this.Center, transform);
      this.Orientation = Quaternion.CreateFromRotationMatrix(in transform);
    }

    public static MyOrientedBoundingBoxD CreateFromBoundingBox(
      BoundingBoxD box)
    {
      return new MyOrientedBoundingBoxD((box.Min + box.Max) * 0.5, (box.Max - box.Min) * 0.5, Quaternion.Identity);
    }

    public MyOrientedBoundingBoxD Transform(
      Quaternion rotation,
      Vector3D translation)
    {
      return new MyOrientedBoundingBoxD(Vector3D.Transform(this.Center, rotation) + translation, this.HalfExtent, this.Orientation * rotation);
    }

    public MyOrientedBoundingBoxD Transform(
      float scale,
      Quaternion rotation,
      Vector3D translation)
    {
      return new MyOrientedBoundingBoxD(Vector3D.Transform(this.Center * (double) scale, rotation) + translation, this.HalfExtent * (double) scale, this.Orientation * rotation);
    }

    public void Transform(MatrixD matrix)
    {
      this.Center = Vector3D.Transform(this.Center, matrix);
      this.Orientation *= Quaternion.CreateFromRotationMatrix(in matrix);
    }

    public void Transform(ref MatrixD matrix)
    {
      Vector3D.Transform(ref this.Center, ref matrix, out this.Center);
      this.Orientation *= Quaternion.CreateFromRotationMatrix(in matrix);
    }

    public bool Equals(MyOrientedBoundingBox other) => this.Center == other.Center && this.HalfExtent == other.HalfExtent && this.Orientation == other.Orientation;

    public override bool Equals(object obj) => obj != null && obj is MyOrientedBoundingBox orientedBoundingBox && (this.Center == orientedBoundingBox.Center && this.HalfExtent == orientedBoundingBox.HalfExtent) && this.Orientation == orientedBoundingBox.Orientation;

    public float Distance(RayD ray)
    {
      Vector3D position = ray.Position;
      Vector3D direction = ray.Direction;
      Vector3D[] corners = new Vector3D[8];
      this.GetCorners(corners, 0);
      float num1 = float.MaxValue;
      foreach (Vector3D vector3D in corners)
      {
        Vector3 vector1 = (Vector3) (vector3D - position);
        float num2 = Vector3.Dot(vector1, (Vector3) direction);
        if ((double) num2 >= 0.0)
        {
          float num3 = (vector1 - num2 * (Vector3) direction).LengthSquared();
          if ((double) num3 < (double) num1)
            num1 = num3;
        }
      }
      return (float) Math.Sqrt((double) num1);
    }

    public override int GetHashCode() => this.Center.GetHashCode() ^ this.HalfExtent.GetHashCode() ^ this.Orientation.GetHashCode();

    public static bool operator ==(MyOrientedBoundingBoxD a, MyOrientedBoundingBoxD b) => a.Equals((object) b);

    public static bool operator !=(MyOrientedBoundingBoxD a, MyOrientedBoundingBoxD b) => !a.Equals((object) b);

    public override string ToString() => "{Center:" + this.Center.ToString() + " Extents:" + this.HalfExtent.ToString() + " Orientation:" + this.Orientation.ToString() + "}";

    public bool Intersects(ref BoundingBox box)
    {
      Vector3D vector3D = (Vector3D) ((box.Max + box.Min) * 0.5f);
      Vector3D hA = (Vector3D) ((box.Max - box.Min) * 0.5f);
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(this.Orientation);
      fromQuaternion.Translation = this.Center - vector3D;
      return (uint) MyOrientedBoundingBoxD.ContainsRelativeBox(ref hA, ref this.HalfExtent, ref fromQuaternion) > 0U;
    }

    public bool Intersects(ref BoundingBoxD box)
    {
      Vector3D vector3D = (box.Max + box.Min) * 0.5;
      Vector3D hA = (box.Max - box.Min) * 0.5;
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(this.Orientation);
      fromQuaternion.Translation = this.Center - vector3D;
      return (uint) MyOrientedBoundingBoxD.ContainsRelativeBox(ref hA, ref this.HalfExtent, ref fromQuaternion) > 0U;
    }

    public ContainmentType Contains(ref BoundingBox box)
    {
      BoundingBoxD box1 = (BoundingBoxD) box;
      return this.Contains(ref box1);
    }

    public ContainmentType Contains(ref BoundingBoxD box)
    {
      Vector3D vector3D = (box.Max + box.Min) * 0.5;
      Vector3D hB = (box.Max - box.Min) * 0.5;
      Quaternion result;
      Quaternion.Conjugate(ref this.Orientation, out result);
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(result);
      fromQuaternion.Translation = Vector3D.TransformNormal(vector3D - this.Center, fromQuaternion);
      return MyOrientedBoundingBoxD.ContainsRelativeBox(ref this.HalfExtent, ref hB, ref fromQuaternion);
    }

    public static ContainmentType Contains(
      ref BoundingBox boxA,
      ref MyOrientedBoundingBox oboxB)
    {
      Vector3 hA = (boxA.Max - boxA.Min) * 0.5f;
      Vector3 vector3 = (boxA.Max + boxA.Min) * 0.5f;
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(oboxB.Orientation);
      fromQuaternion.Translation = oboxB.Center - vector3;
      return MyOrientedBoundingBox.ContainsRelativeBox(ref hA, ref oboxB.HalfExtent, ref fromQuaternion);
    }

    public bool Intersects(ref MyOrientedBoundingBoxD other) => (uint) this.Contains(ref other) > 0U;

    public ContainmentType Contains(ref MyOrientedBoundingBoxD other)
    {
      Quaternion result1;
      Quaternion.Conjugate(ref this.Orientation, out result1);
      Quaternion result2;
      Quaternion.Multiply(ref result1, ref other.Orientation, out result2);
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(result2);
      fromQuaternion.Translation = Vector3D.Transform(other.Center - this.Center, result1);
      return MyOrientedBoundingBoxD.ContainsRelativeBox(ref this.HalfExtent, ref other.HalfExtent, ref fromQuaternion);
    }

    public ContainmentType Contains(BoundingFrustumD frustum) => this.ConvertToFrustum().Contains(frustum);

    public bool Intersects(BoundingFrustumD frustum) => (uint) this.Contains(frustum) > 0U;

    public static ContainmentType Contains(
      BoundingFrustumD frustum,
      ref MyOrientedBoundingBoxD obox)
    {
      return frustum.Contains(obox.ConvertToFrustum());
    }

    public ContainmentType Contains(ref BoundingSphereD sphere)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3 vector3 = Vector3.Transform((Vector3) (sphere.Center - this.Center), rotation);
      double val1_1 = (double) Math.Abs(vector3.X) - this.HalfExtent.X;
      double val1_2 = (double) Math.Abs(vector3.Y) - this.HalfExtent.Y;
      double val1_3 = (double) Math.Abs(vector3.Z) - this.HalfExtent.Z;
      double radius = sphere.Radius;
      if (val1_1 <= -radius && val1_2 <= -radius && val1_3 <= -radius)
        return ContainmentType.Contains;
      double num1 = Math.Max(val1_1, 0.0);
      double num2 = Math.Max(val1_2, 0.0);
      double num3 = Math.Max(val1_3, 0.0);
      return num1 * num1 + num2 * num2 + num3 * num3 >= radius * radius ? ContainmentType.Disjoint : ContainmentType.Intersects;
    }

    public bool Intersects(ref BoundingSphereD sphere)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3 vector3 = Vector3.Transform((Vector3) (sphere.Center - this.Center), rotation);
      double val1_1 = (double) Math.Abs(vector3.X) - this.HalfExtent.X;
      double val1_2 = (double) Math.Abs(vector3.Y) - this.HalfExtent.Y;
      double val1_3 = (double) Math.Abs(vector3.Z) - this.HalfExtent.Z;
      double num1 = Math.Max(val1_1, 0.0);
      double num2 = Math.Max(val1_2, 0.0);
      double num3 = Math.Max(val1_3, 0.0);
      double radius = sphere.Radius;
      return num1 * num1 + num2 * num2 + num3 * num3 < radius * radius;
    }

    public static ContainmentType Contains(
      ref BoundingSphere sphere,
      ref MyOrientedBoundingBox box)
    {
      Quaternion rotation = Quaternion.Conjugate(box.Orientation);
      Vector3 vector3_1 = Vector3.Transform(sphere.Center - box.Center, rotation);
      vector3_1.X = Math.Abs(vector3_1.X);
      vector3_1.Y = Math.Abs(vector3_1.Y);
      vector3_1.Z = Math.Abs(vector3_1.Z);
      float num = sphere.Radius * sphere.Radius;
      if ((double) (vector3_1 + box.HalfExtent).LengthSquared() <= (double) num)
        return ContainmentType.Contains;
      Vector3 vector3_2 = vector3_1 - box.HalfExtent;
      vector3_2.X = Math.Max(vector3_2.X, 0.0f);
      vector3_2.Y = Math.Max(vector3_2.Y, 0.0f);
      vector3_2.Z = Math.Max(vector3_2.Z, 0.0f);
      return (double) vector3_2.LengthSquared() >= (double) num ? ContainmentType.Disjoint : ContainmentType.Intersects;
    }

    public bool Contains(ref Vector3 point)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3 vector3 = (Vector3) Vector3D.Transform(point - this.Center, rotation);
      return (double) Math.Abs(vector3.X) <= this.HalfExtent.X && (double) Math.Abs(vector3.Y) <= this.HalfExtent.Y && (double) Math.Abs(vector3.Z) <= this.HalfExtent.Z;
    }

    public bool Contains(ref Vector3D point)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3D vector3D = Vector3D.Transform(point - this.Center, rotation);
      return Math.Abs(vector3D.X) <= this.HalfExtent.X && Math.Abs(vector3D.Y) <= this.HalfExtent.Y && Math.Abs(vector3D.Z) <= this.HalfExtent.Z;
    }

    public double? Intersects(ref RayD ray)
    {
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.Orientation);
      MatrixD matrixD = (MatrixD) ref fromQuaternion;
      Vector3D vector2 = this.Center - ray.Position;
      double num1 = double.MinValue;
      double num2 = double.MaxValue;
      double num3 = Vector3D.Dot(matrixD.Right, vector2);
      double num4 = Vector3D.Dot(matrixD.Right, ray.Direction);
      if (num4 >= -9.99999968265523E-21 && num4 <= 9.99999968265523E-21)
      {
        if (-num3 - this.HalfExtent.X > 0.0 || -num3 + this.HalfExtent.X < 0.0)
          return new double?();
      }
      else
      {
        double num5 = (num3 - this.HalfExtent.X) / num4;
        double num6 = (num3 + this.HalfExtent.X) / num4;
        if (num5 > num6)
        {
          double num7 = num5;
          num5 = num6;
          num6 = num7;
        }
        if (num5 > num1)
          num1 = num5;
        if (num6 < num2)
          num2 = num6;
        if (num2 < 0.0 || num1 > num2)
          return new double?();
      }
      double num8 = Vector3D.Dot(matrixD.Up, vector2);
      double num9 = Vector3D.Dot(matrixD.Up, ray.Direction);
      if (num9 >= -9.99999968265523E-21 && num9 <= 9.99999968265523E-21)
      {
        if (-num8 - this.HalfExtent.Y > 0.0 || -num8 + this.HalfExtent.Y < 0.0)
          return new double?();
      }
      else
      {
        double num5 = (num8 - this.HalfExtent.Y) / num9;
        double num6 = (num8 + this.HalfExtent.Y) / num9;
        if (num5 > num6)
        {
          double num7 = num5;
          num5 = num6;
          num6 = num7;
        }
        if (num5 > num1)
          num1 = num5;
        if (num6 < num2)
          num2 = num6;
        if (num2 < 0.0 || num1 > num2)
          return new double?();
      }
      double num10 = Vector3D.Dot(matrixD.Forward, vector2);
      double num11 = Vector3D.Dot(matrixD.Forward, ray.Direction);
      if (num11 >= -9.99999968265523E-21 && num11 <= 9.99999968265523E-21)
      {
        if (-num10 - this.HalfExtent.Z > 0.0 || -num10 + this.HalfExtent.Z < 0.0)
          return new double?();
      }
      else
      {
        double num5 = (num10 - this.HalfExtent.Z) / num11;
        double num6 = (num10 + this.HalfExtent.Z) / num11;
        if (num5 > num6)
        {
          double num7 = num5;
          num5 = num6;
          num6 = num7;
        }
        if (num5 > num1)
          num1 = num5;
        if (num6 < num2)
          num2 = num6;
        if (num2 < 0.0 || num1 > num2)
          return new double?();
      }
      return new double?(num1);
    }

    public double? Intersects(ref LineD line)
    {
      if (this.Contains(ref line.From))
      {
        RayD ray = new RayD(line.To, -line.Direction);
        double? nullable = this.Intersects(ref ray);
        if (!nullable.HasValue)
          return new double?();
        double num = line.Length - nullable.Value;
        if (num < 0.0)
          return new double?();
        return num > line.Length ? new double?() : new double?(num);
      }
      RayD ray1 = new RayD(line.From, line.Direction);
      double? nullable1 = this.Intersects(ref ray1);
      if (!nullable1.HasValue)
        return new double?();
      if (nullable1.Value < 0.0)
        return new double?();
      return nullable1.Value > line.Length ? new double?() : new double?(nullable1.Value);
    }

    public double? IntersectsOrContains(ref LineD line)
    {
      if (this.Contains(ref line.From))
        return new double?(0.0);
      RayD ray = new RayD(line.From, line.Direction);
      double? nullable = this.Intersects(ref ray);
      if (!nullable.HasValue)
        return new double?();
      if (nullable.Value < 0.0)
        return new double?();
      return nullable.Value > line.Length ? new double?() : new double?(nullable.Value);
    }

    public PlaneIntersectionType Intersects(ref PlaneD plane)
    {
      double num1 = plane.DotCoordinate(this.Center);
      Vector3D vector3D = Vector3D.Transform(plane.Normal, Quaternion.Conjugate(this.Orientation));
      double num2 = Math.Abs(this.HalfExtent.X * vector3D.X) + Math.Abs(this.HalfExtent.Y * vector3D.Y) + Math.Abs(this.HalfExtent.Z * vector3D.Z);
      if (num1 > num2)
        return PlaneIntersectionType.Front;
      return num1 < -num2 ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public void GetCorners(Vector3D[] corners, int startIndex)
    {
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(this.Orientation);
      Vector3D vector3D1 = fromQuaternion.Left * this.HalfExtent.X;
      Vector3D vector3D2 = fromQuaternion.Up * this.HalfExtent.Y;
      Vector3D vector3D3 = fromQuaternion.Backward * this.HalfExtent.Z;
      int num1 = startIndex;
      Vector3D[] vector3DArray1 = corners;
      int index1 = num1;
      int num2 = index1 + 1;
      Vector3D vector3D4 = this.Center - vector3D1 + vector3D2 + vector3D3;
      vector3DArray1[index1] = vector3D4;
      Vector3D[] vector3DArray2 = corners;
      int index2 = num2;
      int num3 = index2 + 1;
      Vector3D vector3D5 = this.Center + vector3D1 + vector3D2 + vector3D3;
      vector3DArray2[index2] = vector3D5;
      Vector3D[] vector3DArray3 = corners;
      int index3 = num3;
      int num4 = index3 + 1;
      Vector3D vector3D6 = this.Center + vector3D1 - vector3D2 + vector3D3;
      vector3DArray3[index3] = vector3D6;
      Vector3D[] vector3DArray4 = corners;
      int index4 = num4;
      int num5 = index4 + 1;
      Vector3D vector3D7 = this.Center - vector3D1 - vector3D2 + vector3D3;
      vector3DArray4[index4] = vector3D7;
      Vector3D[] vector3DArray5 = corners;
      int index5 = num5;
      int num6 = index5 + 1;
      Vector3D vector3D8 = this.Center - vector3D1 + vector3D2 - vector3D3;
      vector3DArray5[index5] = vector3D8;
      Vector3D[] vector3DArray6 = corners;
      int index6 = num6;
      int num7 = index6 + 1;
      Vector3D vector3D9 = this.Center + vector3D1 + vector3D2 - vector3D3;
      vector3DArray6[index6] = vector3D9;
      Vector3D[] vector3DArray7 = corners;
      int index7 = num7;
      int num8 = index7 + 1;
      Vector3D vector3D10 = this.Center + vector3D1 - vector3D2 - vector3D3;
      vector3DArray7[index7] = vector3D10;
      Vector3D[] vector3DArray8 = corners;
      int index8 = num8;
      int num9 = index8 + 1;
      Vector3D vector3D11 = this.Center - vector3D1 - vector3D2 - vector3D3;
      vector3DArray8[index8] = vector3D11;
    }

    public static ContainmentType ContainsRelativeBox(
      ref Vector3D hA,
      ref Vector3D hB,
      ref MatrixD mB)
    {
      Vector3D translation = mB.Translation;
      Vector3D vector3D1 = new Vector3D(Math.Abs(translation.X), Math.Abs(translation.Y), Math.Abs(translation.Z));
      Vector3D right = mB.Right;
      Vector3D up = mB.Up;
      Vector3D backward = mB.Backward;
      Vector3D vector2_1 = right * hB.X;
      Vector3D vector2_2 = up * hB.Y;
      Vector3D vector2_3 = backward * hB.Z;
      double num1 = Math.Abs(vector2_1.X) + Math.Abs(vector2_2.X) + Math.Abs(vector2_3.X);
      double num2 = Math.Abs(vector2_1.Y) + Math.Abs(vector2_2.Y) + Math.Abs(vector2_3.Y);
      double num3 = Math.Abs(vector2_1.Z) + Math.Abs(vector2_2.Z) + Math.Abs(vector2_3.Z);
      if (vector3D1.X + num1 <= hA.X && vector3D1.Y + num2 <= hA.Y && vector3D1.Z + num3 <= hA.Z)
        return ContainmentType.Contains;
      if (vector3D1.X > hA.X + Math.Abs(vector2_1.X) + Math.Abs(vector2_2.X) + Math.Abs(vector2_3.X) || vector3D1.Y > hA.Y + Math.Abs(vector2_1.Y) + Math.Abs(vector2_2.Y) + Math.Abs(vector2_3.Y) || (vector3D1.Z > hA.Z + Math.Abs(vector2_1.Z) + Math.Abs(vector2_2.Z) + Math.Abs(vector2_3.Z) || Math.Abs(Vector3D.Dot(translation, right)) > Math.Abs(hA.X * right.X) + Math.Abs(hA.Y * right.Y) + Math.Abs(hA.Z * right.Z) + hB.X) || (Math.Abs(Vector3D.Dot(translation, up)) > Math.Abs(hA.X * up.X) + Math.Abs(hA.Y * up.Y) + Math.Abs(hA.Z * up.Z) + hB.Y || Math.Abs(Vector3D.Dot(translation, backward)) > Math.Abs(hA.X * backward.X) + Math.Abs(hA.Y * backward.Y) + Math.Abs(hA.Z * backward.Z) + hB.Z))
        return ContainmentType.Disjoint;
      Vector3D vector3D2 = new Vector3D(0.0, -right.Z, right.Y);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(0.0, -up.Z, up.Y);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(0.0, -backward.Z, backward.Y);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(right.Z, 0.0, -right.X);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(up.Z, 0.0, -up.X);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(backward.Z, 0.0, -backward.X);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.Z * vector3D2.Z) + Math.Abs(hA.X * vector3D2.X) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(-right.Y, right.X, 0.0);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(-up.Y, up.X, 0.0);
      if (Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector2_3)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3D2 = new Vector3D(-backward.Y, backward.X, 0.0);
      return Math.Abs(Vector3D.Dot(translation, vector3D2)) > Math.Abs(hA.X * vector3D2.X) + Math.Abs(hA.Y * vector3D2.Y) + Math.Abs(Vector3D.Dot(vector3D2, vector2_1)) + Math.Abs(Vector3D.Dot(vector3D2, vector2_2)) ? ContainmentType.Disjoint : ContainmentType.Intersects;
    }

    public BoundingFrustumD ConvertToFrustum()
    {
      Quaternion result1;
      Quaternion.Conjugate(ref this.Orientation, out result1);
      double num1 = 1.0 / this.HalfExtent.X;
      double num2 = 1.0 / this.HalfExtent.Y;
      double num3 = 0.5 / this.HalfExtent.Z;
      MatrixD result2;
      MatrixD.CreateFromQuaternion(ref result1, out result2);
      result2.M11 *= num1;
      result2.M21 *= num1;
      result2.M31 *= num1;
      result2.M12 *= num2;
      result2.M22 *= num2;
      result2.M32 *= num2;
      result2.M13 *= num3;
      result2.M23 *= num3;
      result2.M33 *= num3;
      result2.Translation = Vector3.UnitZ * 0.5f + Vector3D.TransformNormal(-this.Center, result2);
      return new BoundingFrustumD(result2);
    }

    public BoundingBoxD GetAABB()
    {
      if (MyOrientedBoundingBoxD.m_cornersTmp == null)
        MyOrientedBoundingBoxD.m_cornersTmp = new Vector3D[8];
      this.GetCorners(MyOrientedBoundingBoxD.m_cornersTmp, 0);
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      for (int index = 0; index < 8; ++index)
        invalid.Include(MyOrientedBoundingBoxD.m_cornersTmp[index]);
      return invalid;
    }

    public static MyOrientedBoundingBoxD Create(
      BoundingBoxD boundingBox,
      MatrixD matrix)
    {
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(boundingBox.Center, boundingBox.HalfExtents, Quaternion.Identity);
      orientedBoundingBoxD.Transform(ref matrix);
      return orientedBoundingBoxD;
    }
  }
}
