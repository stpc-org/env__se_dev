// Decompiled with JetBrains decompiler
// Type: VRageMath.MyOrientedBoundingBox
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public struct MyOrientedBoundingBox : IEquatable<MyOrientedBoundingBox>
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
    public Vector3 Center;
    public Vector3 HalfExtent;
    public Quaternion Orientation;
    [ThreadStatic]
    private static Vector3[] m_cornersTmp;

    public static bool GetNormalBetweenEdges(int axis, int edge0, int edge1, out Vector3 normal)
    {
      normal = Vector3.Zero;
      Vector3[] vector3Array1;
      Vector3[] vector3Array2;
      switch (axis)
      {
        case 0:
          int[] startXvertices = MyOrientedBoundingBox.StartXVertices;
          int[] endXvertices = MyOrientedBoundingBox.EndXVertices;
          vector3Array1 = MyOrientedBoundingBox.XNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBox.XNeighbourVectorsBack;
          break;
        case 1:
          int[] startYvertices = MyOrientedBoundingBox.StartYVertices;
          int[] endYvertices = MyOrientedBoundingBox.EndYVertices;
          vector3Array1 = MyOrientedBoundingBox.YNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBox.YNeighbourVectorsBack;
          break;
        case 2:
          int[] startZvertices = MyOrientedBoundingBox.StartZVertices;
          int[] endZvertices = MyOrientedBoundingBox.EndZVertices;
          vector3Array1 = MyOrientedBoundingBox.ZNeighbourVectorsForw;
          vector3Array2 = MyOrientedBoundingBox.ZNeighbourVectorsBack;
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

    public MyOrientedBoundingBox(ref Matrix matrix)
    {
      this.Center = matrix.Translation;
      Vector3 vector3_1;
      ref Vector3 local = ref vector3_1;
      Vector3 vector3_2 = matrix.Right;
      double num1 = (double) vector3_2.Length();
      vector3_2 = matrix.Up;
      double num2 = (double) vector3_2.Length();
      vector3_2 = matrix.Forward;
      double num3 = (double) vector3_2.Length();
      local = new Vector3((float) num1, (float) num2, (float) num3);
      this.HalfExtent = vector3_1 / 2f;
      matrix.Right /= vector3_1.X;
      matrix.Up /= vector3_1.Y;
      matrix.Forward /= vector3_1.Z;
      Quaternion.CreateFromRotationMatrix(ref matrix, out this.Orientation);
    }

    public MyOrientedBoundingBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
    {
      this.Center = center;
      this.HalfExtent = halfExtents;
      this.Orientation = orientation;
    }

    public static MyOrientedBoundingBox CreateFromBoundingBox(BoundingBox box) => new MyOrientedBoundingBox((box.Min + box.Max) * 0.5f, (box.Max - box.Min) * 0.5f, Quaternion.Identity);

    public MyOrientedBoundingBox Transform(
      Quaternion rotation,
      Vector3 translation)
    {
      return new MyOrientedBoundingBox(Vector3.Transform(this.Center, rotation) + translation, this.HalfExtent, this.Orientation * rotation);
    }

    public MyOrientedBoundingBox Transform(
      float scale,
      Quaternion rotation,
      Vector3 translation)
    {
      return new MyOrientedBoundingBox(Vector3.Transform(this.Center * scale, rotation) + translation, this.HalfExtent * scale, this.Orientation * rotation);
    }

    public void Transform(Matrix matrix)
    {
      this.Center = Vector3.Transform(this.Center, matrix);
      this.Orientation = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromQuaternion(this.Orientation) * matrix);
    }

    public bool Equals(MyOrientedBoundingBox other) => this.Center == other.Center && this.HalfExtent == other.HalfExtent && this.Orientation == other.Orientation;

    public override bool Equals(object obj) => obj != null && obj is MyOrientedBoundingBox orientedBoundingBox && (this.Center == orientedBoundingBox.Center && this.HalfExtent == orientedBoundingBox.HalfExtent) && this.Orientation == orientedBoundingBox.Orientation;

    public override int GetHashCode() => this.Center.GetHashCode() ^ this.HalfExtent.GetHashCode() ^ this.Orientation.GetHashCode();

    public static bool operator ==(MyOrientedBoundingBox a, MyOrientedBoundingBox b) => a.Equals(b);

    public static bool operator !=(MyOrientedBoundingBox a, MyOrientedBoundingBox b) => !a.Equals(b);

    public override string ToString() => "{Center:" + this.Center.ToString() + " Extents:" + this.HalfExtent.ToString() + " Orientation:" + this.Orientation.ToString() + "}";

    public bool Intersects(ref BoundingBox box)
    {
      Vector3 vector3 = (box.Max + box.Min) * 0.5f;
      Vector3 hA = (box.Max - box.Min) * 0.5f;
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.Orientation);
      fromQuaternion.Translation = this.Center - vector3;
      return (uint) MyOrientedBoundingBox.ContainsRelativeBox(ref hA, ref this.HalfExtent, ref fromQuaternion) > 0U;
    }

    public ContainmentType Contains(ref BoundingBox box)
    {
      Vector3 vector3 = (box.Max + box.Min) * 0.5f;
      Vector3 hB = (box.Max - box.Min) * 0.5f;
      Quaternion result;
      Quaternion.Conjugate(ref this.Orientation, out result);
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(result);
      fromQuaternion.Translation = Vector3.TransformNormal(vector3 - this.Center, fromQuaternion);
      return MyOrientedBoundingBox.ContainsRelativeBox(ref this.HalfExtent, ref hB, ref fromQuaternion);
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

    public bool Intersects(ref MyOrientedBoundingBox other) => (uint) this.Contains(ref other) > 0U;

    public ContainmentType Contains(ref MyOrientedBoundingBox other)
    {
      Quaternion result1;
      Quaternion.Conjugate(ref this.Orientation, out result1);
      Quaternion result2;
      Quaternion.Multiply(ref result1, ref other.Orientation, out result2);
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(result2);
      fromQuaternion.Translation = Vector3.Transform(other.Center - this.Center, result1);
      return MyOrientedBoundingBox.ContainsRelativeBox(ref this.HalfExtent, ref other.HalfExtent, ref fromQuaternion);
    }

    public ContainmentType Contains(BoundingFrustum frustum) => this.ConvertToFrustum().Contains(frustum);

    public bool Intersects(BoundingFrustum frustum) => (uint) this.Contains(frustum) > 0U;

    public static ContainmentType Contains(
      BoundingFrustum frustum,
      ref MyOrientedBoundingBox obox)
    {
      return frustum.Contains(obox.ConvertToFrustum());
    }

    public ContainmentType Contains(ref BoundingSphere sphere)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3 vector3 = Vector3.Transform(sphere.Center - this.Center, rotation);
      float val1_1 = Math.Abs(vector3.X) - this.HalfExtent.X;
      float val1_2 = Math.Abs(vector3.Y) - this.HalfExtent.Y;
      float val1_3 = Math.Abs(vector3.Z) - this.HalfExtent.Z;
      float radius = sphere.Radius;
      if ((double) val1_1 <= -(double) radius && (double) val1_2 <= -(double) radius && (double) val1_3 <= -(double) radius)
        return ContainmentType.Contains;
      float num1 = Math.Max(val1_1, 0.0f);
      float num2 = Math.Max(val1_2, 0.0f);
      float num3 = Math.Max(val1_3, 0.0f);
      return (double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 >= (double) radius * (double) radius ? ContainmentType.Disjoint : ContainmentType.Intersects;
    }

    public bool Intersects(ref BoundingSphere sphere)
    {
      Quaternion rotation = Quaternion.Conjugate(this.Orientation);
      Vector3 vector3 = Vector3.Transform(sphere.Center - this.Center, rotation);
      float val1_1 = Math.Abs(vector3.X) - this.HalfExtent.X;
      float val1_2 = Math.Abs(vector3.Y) - this.HalfExtent.Y;
      float val1_3 = Math.Abs(vector3.Z) - this.HalfExtent.Z;
      float num1 = Math.Max(val1_1, 0.0f);
      float num2 = Math.Max(val1_2, 0.0f);
      float num3 = Math.Max(val1_3, 0.0f);
      float radius = sphere.Radius;
      return (double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 < (double) radius * (double) radius;
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
      Vector3 vector3 = Vector3.Transform(point - this.Center, rotation);
      return (double) Math.Abs(vector3.X) <= (double) this.HalfExtent.X && (double) Math.Abs(vector3.Y) <= (double) this.HalfExtent.Y && (double) Math.Abs(vector3.Z) <= (double) this.HalfExtent.Z;
    }

    public float? Intersects(ref Ray ray)
    {
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.Orientation);
      Vector3 vector2 = this.Center - ray.Position;
      float num1 = float.MinValue;
      float num2 = float.MaxValue;
      float num3 = Vector3.Dot(fromQuaternion.Right, vector2);
      float num4 = Vector3.Dot(fromQuaternion.Right, ray.Direction);
      if ((double) num4 >= -9.99999968265523E-21 && (double) num4 <= 9.99999968265523E-21)
      {
        if (-(double) num3 - (double) this.HalfExtent.X > 0.0 || -(double) num3 + (double) this.HalfExtent.X < 0.0)
          return new float?();
      }
      else
      {
        float num5 = (num3 - this.HalfExtent.X) / num4;
        float num6 = (num3 + this.HalfExtent.X) / num4;
        if ((double) num5 > (double) num6)
        {
          double num7 = (double) num5;
          num5 = num6;
          num6 = (float) num7;
        }
        if ((double) num5 > (double) num1)
          num1 = num5;
        if ((double) num6 < (double) num2)
          num2 = num6;
        if ((double) num2 < 0.0 || (double) num1 > (double) num2)
          return new float?();
      }
      float num8 = Vector3.Dot(fromQuaternion.Up, vector2);
      float num9 = Vector3.Dot(fromQuaternion.Up, ray.Direction);
      if ((double) num9 >= -9.99999968265523E-21 && (double) num9 <= 9.99999968265523E-21)
      {
        if (-(double) num8 - (double) this.HalfExtent.Y > 0.0 || -(double) num8 + (double) this.HalfExtent.Y < 0.0)
          return new float?();
      }
      else
      {
        float num5 = (num8 - this.HalfExtent.Y) / num9;
        float num6 = (num8 + this.HalfExtent.Y) / num9;
        if ((double) num5 > (double) num6)
        {
          double num7 = (double) num5;
          num5 = num6;
          num6 = (float) num7;
        }
        if ((double) num5 > (double) num1)
          num1 = num5;
        if ((double) num6 < (double) num2)
          num2 = num6;
        if ((double) num2 < 0.0 || (double) num1 > (double) num2)
          return new float?();
      }
      float num10 = Vector3.Dot(fromQuaternion.Forward, vector2);
      float num11 = Vector3.Dot(fromQuaternion.Forward, ray.Direction);
      if ((double) num11 >= -9.99999968265523E-21 && (double) num11 <= 9.99999968265523E-21)
      {
        if (-(double) num10 - (double) this.HalfExtent.Z > 0.0 || -(double) num10 + (double) this.HalfExtent.Z < 0.0)
          return new float?();
      }
      else
      {
        float num5 = (num10 - this.HalfExtent.Z) / num11;
        float num6 = (num10 + this.HalfExtent.Z) / num11;
        if ((double) num5 > (double) num6)
        {
          double num7 = (double) num5;
          num5 = num6;
          num6 = (float) num7;
        }
        if ((double) num5 > (double) num1)
          num1 = num5;
        if ((double) num6 < (double) num2)
          num2 = num6;
        if ((double) num2 < 0.0 || (double) num1 > (double) num2)
          return new float?();
      }
      return new float?(num1);
    }

    public float? Intersects(ref Line line)
    {
      if (this.Contains(ref line.From))
      {
        Ray ray = new Ray(line.To, -line.Direction);
        float? nullable = this.Intersects(ref ray);
        if (!nullable.HasValue)
          return new float?();
        float num = line.Length - nullable.Value;
        if ((double) num < 0.0)
          return new float?();
        return (double) num > (double) line.Length ? new float?() : new float?(num);
      }
      Ray ray1 = new Ray(line.From, line.Direction);
      float? nullable1 = this.Intersects(ref ray1);
      if (!nullable1.HasValue)
        return new float?();
      if ((double) nullable1.Value < 0.0)
        return new float?();
      return (double) nullable1.Value > (double) line.Length ? new float?() : new float?(nullable1.Value);
    }

    public PlaneIntersectionType Intersects(ref Plane plane)
    {
      float num1 = plane.DotCoordinate(this.Center);
      Vector3 vector3 = Vector3.Transform(plane.Normal, Quaternion.Conjugate(this.Orientation));
      float num2 = Math.Abs(this.HalfExtent.X * vector3.X) + Math.Abs(this.HalfExtent.Y * vector3.Y) + Math.Abs(this.HalfExtent.Z * vector3.Z);
      if ((double) num1 > (double) num2)
        return PlaneIntersectionType.Front;
      return (double) num1 < -(double) num2 ? PlaneIntersectionType.Back : PlaneIntersectionType.Intersecting;
    }

    public void GetCorners(Vector3[] corners, int startIndex)
    {
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.Orientation);
      Vector3 vector3_1 = fromQuaternion.Left * this.HalfExtent.X;
      Vector3 vector3_2 = fromQuaternion.Up * this.HalfExtent.Y;
      Vector3 vector3_3 = fromQuaternion.Backward * this.HalfExtent.Z;
      int num1 = startIndex;
      Vector3[] vector3Array1 = corners;
      int index1 = num1;
      int num2 = index1 + 1;
      Vector3 vector3_4 = this.Center - vector3_1 + vector3_2 + vector3_3;
      vector3Array1[index1] = vector3_4;
      Vector3[] vector3Array2 = corners;
      int index2 = num2;
      int num3 = index2 + 1;
      Vector3 vector3_5 = this.Center + vector3_1 + vector3_2 + vector3_3;
      vector3Array2[index2] = vector3_5;
      Vector3[] vector3Array3 = corners;
      int index3 = num3;
      int num4 = index3 + 1;
      Vector3 vector3_6 = this.Center + vector3_1 - vector3_2 + vector3_3;
      vector3Array3[index3] = vector3_6;
      Vector3[] vector3Array4 = corners;
      int index4 = num4;
      int num5 = index4 + 1;
      Vector3 vector3_7 = this.Center - vector3_1 - vector3_2 + vector3_3;
      vector3Array4[index4] = vector3_7;
      Vector3[] vector3Array5 = corners;
      int index5 = num5;
      int num6 = index5 + 1;
      Vector3 vector3_8 = this.Center - vector3_1 + vector3_2 - vector3_3;
      vector3Array5[index5] = vector3_8;
      Vector3[] vector3Array6 = corners;
      int index6 = num6;
      int num7 = index6 + 1;
      Vector3 vector3_9 = this.Center + vector3_1 + vector3_2 - vector3_3;
      vector3Array6[index6] = vector3_9;
      Vector3[] vector3Array7 = corners;
      int index7 = num7;
      int num8 = index7 + 1;
      Vector3 vector3_10 = this.Center + vector3_1 - vector3_2 - vector3_3;
      vector3Array7[index7] = vector3_10;
      Vector3[] vector3Array8 = corners;
      int index8 = num8;
      int num9 = index8 + 1;
      Vector3 vector3_11 = this.Center - vector3_1 - vector3_2 - vector3_3;
      vector3Array8[index8] = vector3_11;
    }

    public static ContainmentType ContainsRelativeBox(
      ref Vector3 hA,
      ref Vector3 hB,
      ref Matrix mB)
    {
      Vector3 translation = mB.Translation;
      Vector3 vector3_1 = new Vector3(Math.Abs(translation.X), Math.Abs(translation.Y), Math.Abs(translation.Z));
      Vector3 right = mB.Right;
      Vector3 up = mB.Up;
      Vector3 backward = mB.Backward;
      Vector3 vector2_1 = right * hB.X;
      Vector3 vector2_2 = up * hB.Y;
      Vector3 vector2_3 = backward * hB.Z;
      float num1 = Math.Abs(vector2_1.X) + Math.Abs(vector2_2.X) + Math.Abs(vector2_3.X);
      float num2 = Math.Abs(vector2_1.Y) + Math.Abs(vector2_2.Y) + Math.Abs(vector2_3.Y);
      float num3 = Math.Abs(vector2_1.Z) + Math.Abs(vector2_2.Z) + Math.Abs(vector2_3.Z);
      if ((double) vector3_1.X + (double) num1 <= (double) hA.X && (double) vector3_1.Y + (double) num2 <= (double) hA.Y && (double) vector3_1.Z + (double) num3 <= (double) hA.Z)
        return ContainmentType.Contains;
      if ((double) vector3_1.X > (double) hA.X + (double) Math.Abs(vector2_1.X) + (double) Math.Abs(vector2_2.X) + (double) Math.Abs(vector2_3.X) || (double) vector3_1.Y > (double) hA.Y + (double) Math.Abs(vector2_1.Y) + (double) Math.Abs(vector2_2.Y) + (double) Math.Abs(vector2_3.Y) || ((double) vector3_1.Z > (double) hA.Z + (double) Math.Abs(vector2_1.Z) + (double) Math.Abs(vector2_2.Z) + (double) Math.Abs(vector2_3.Z) || (double) Math.Abs(Vector3.Dot(translation, right)) > (double) Math.Abs(hA.X * right.X) + (double) Math.Abs(hA.Y * right.Y) + (double) Math.Abs(hA.Z * right.Z) + (double) hB.X) || ((double) Math.Abs(Vector3.Dot(translation, up)) > (double) Math.Abs(hA.X * up.X) + (double) Math.Abs(hA.Y * up.Y) + (double) Math.Abs(hA.Z * up.Z) + (double) hB.Y || (double) Math.Abs(Vector3.Dot(translation, backward)) > (double) Math.Abs(hA.X * backward.X) + (double) Math.Abs(hA.Y * backward.Y) + (double) Math.Abs(hA.Z * backward.Z) + (double) hB.Z))
        return ContainmentType.Disjoint;
      Vector3 vector3_2 = new Vector3(0.0f, -right.Z, right.Y);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(0.0f, -up.Z, up.Y);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(0.0f, -backward.Z, backward.Y);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(right.Z, 0.0f, -right.X);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(up.Z, 0.0f, -up.X);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(backward.Z, 0.0f, -backward.X);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.Z * vector3_2.Z) + (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(-right.Y, right.X, 0.0f);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(-up.Y, up.X, 0.0f);
      if ((double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_3)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)))
        return ContainmentType.Disjoint;
      vector3_2 = new Vector3(-backward.Y, backward.X, 0.0f);
      return (double) Math.Abs(Vector3.Dot(translation, vector3_2)) > (double) Math.Abs(hA.X * vector3_2.X) + (double) Math.Abs(hA.Y * vector3_2.Y) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_1)) + (double) Math.Abs(Vector3.Dot(vector3_2, vector2_2)) ? ContainmentType.Disjoint : ContainmentType.Intersects;
    }

    public BoundingFrustum ConvertToFrustum()
    {
      Quaternion result1;
      Quaternion.Conjugate(ref this.Orientation, out result1);
      float num1 = 1f / this.HalfExtent.X;
      float num2 = 1f / this.HalfExtent.Y;
      float num3 = 0.5f / this.HalfExtent.Z;
      Matrix result2;
      Matrix.CreateFromQuaternion(ref result1, out result2);
      result2.M11 *= num1;
      result2.M21 *= num1;
      result2.M31 *= num1;
      result2.M12 *= num2;
      result2.M22 *= num2;
      result2.M32 *= num2;
      result2.M13 *= num3;
      result2.M23 *= num3;
      result2.M33 *= num3;
      result2.Translation = Vector3.UnitZ * 0.5f + Vector3.TransformNormal(-this.Center, result2);
      return new BoundingFrustum(result2);
    }

    public BoundingBox GetAABB()
    {
      if (MyOrientedBoundingBox.m_cornersTmp == null)
        MyOrientedBoundingBox.m_cornersTmp = new Vector3[8];
      this.GetCorners(MyOrientedBoundingBox.m_cornersTmp, 0);
      BoundingBox invalid = BoundingBox.CreateInvalid();
      for (int index = 0; index < 8; ++index)
        invalid.Include(MyOrientedBoundingBox.m_cornersTmp[index]);
      return invalid;
    }

    public static MyOrientedBoundingBox Create(
      BoundingBox boundingBox,
      Matrix matrix)
    {
      MyOrientedBoundingBox orientedBoundingBox = new MyOrientedBoundingBox(boundingBox.Center, boundingBox.HalfExtents, Quaternion.Identity);
      orientedBoundingBox.Transform(matrix);
      return orientedBoundingBox;
    }
  }
}
