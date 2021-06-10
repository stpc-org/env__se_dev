// Decompiled with JetBrains decompiler
// Type: VRageMath.MyMath
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public static class MyMath
  {
    private const float Size = 10000f;
    private static int ANGLE_GRANULARITY = 0;
    private static float[] m_precomputedValues = (float[]) null;
    private static Vector3[] m_corners = new Vector3[8];
    private static readonly float OneOverRoot3 = (float) Math.Pow(3.0, -0.5);
    public static Vector3 Vector3One = Vector3.One;

    public static void InitializeFastSin()
    {
      if (MyMath.m_precomputedValues != null)
        return;
      MyMath.ANGLE_GRANULARITY = 62830;
      MyMath.m_precomputedValues = new float[MyMath.ANGLE_GRANULARITY];
      for (int index = 0; index < MyMath.ANGLE_GRANULARITY; ++index)
        MyMath.m_precomputedValues[index] = (float) Math.Sin((double) index / 10000.0);
    }

    public static float FastSin(float angle)
    {
      int index = (int) ((double) angle * 10000.0) % MyMath.ANGLE_GRANULARITY;
      if (index < 0)
        index += MyMath.ANGLE_GRANULARITY;
      return MyMath.m_precomputedValues[index];
    }

    public static float FastCos(float angle) => MyMath.FastSin(angle + 1.570796f);

    public static float FastTanH(float x)
    {
      if ((double) x < -3.0)
        return -1f;
      return (double) x > 3.0 ? 1f : (float) ((double) x * (27.0 + (double) x * (double) x) / (27.0 + 9.0 * (double) x * (double) x));
    }

    public static float NormalizeAngle(float angle, float center = 0.0f) => angle - 6.283185f * (float) Math.Floor(((double) angle + 3.14159297943115 - (double) center) / 6.28318548202515);

    public static float ArcTanAngle(float x, float y)
    {
      if ((double) x == 0.0)
        return (double) y == 1.0 ? 1.570796f : -1.570796f;
      if ((double) x > 0.0)
        return (float) Math.Atan((double) y / (double) x);
      if ((double) x >= 0.0)
        return 0.0f;
      return (double) y > 0.0 ? (float) Math.Atan((double) y / (double) x) + 3.141593f : (float) Math.Atan((double) y / (double) x) - 3.141593f;
    }

    public static Vector3 Abs(ref Vector3 vector) => new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));

    public static Vector3 MaxComponents(ref Vector3 a, ref Vector3 b) => new Vector3(MathHelper.Max(a.X, b.X), MathHelper.Max(a.Y, b.Y), MathHelper.Max(a.Z, b.Z));

    public static Vector3 AngleTo(Vector3 From, Vector3 Location)
    {
      Vector3 zero = Vector3.Zero;
      Vector3 vector3 = Vector3.Normalize(Location - From);
      zero.X = (float) Math.Asin((double) vector3.Y);
      zero.Y = MyMath.ArcTanAngle(-vector3.Z, -vector3.X);
      return zero;
    }

    public static float AngleBetween(Vector3 a, Vector3 b)
    {
      float num = Vector3.Dot(a, b) / (a.Length() * b.Length());
      return (double) Math.Abs(1f - num) < 1.0 / 1000.0 ? 0.0f : (float) Math.Acos((double) num);
    }

    public static float CosineDistance(ref Vector3 a, ref Vector3 b)
    {
      float result;
      Vector3.Dot(ref a, ref b, out result);
      return result / (a.Length() * b.Length());
    }

    public static double CosineDistance(ref Vector3D a, ref Vector3D b)
    {
      double result;
      Vector3D.Dot(ref a, ref b, out result);
      return result / (a.Length() * b.Length());
    }

    public static int Mod(int x, int m) => (x % m + m) % m;

    public static long Mod(long x, int m) => (x % (long) m + (long) m) % (long) m;

    public static Vector3 QuaternionToEuler(Quaternion Rotation)
    {
      Vector3 Location = Vector3.Transform(Vector3.Forward, Rotation);
      Vector3 position = Vector3.Transform(Vector3.Up, Rotation);
      Vector3 vector3_1 = MyMath.AngleTo(new Vector3(), Location);
      if ((double) vector3_1.X == 1.57079601287842)
      {
        vector3_1.Y = MyMath.ArcTanAngle(position.Z, position.X);
        vector3_1.Z = 0.0f;
      }
      else if ((double) vector3_1.X == -1.57079601287842)
      {
        vector3_1.Y = MyMath.ArcTanAngle(-position.Y, -position.X);
        vector3_1.Z = 0.0f;
      }
      else
      {
        Vector3 vector3_2 = Vector3.Transform(Vector3.Transform(position, Matrix.CreateRotationY(-vector3_1.Y)), Matrix.CreateRotationX(-vector3_1.X));
        vector3_1.Z = MyMath.ArcTanAngle(vector3_2.Y, -vector3_2.X);
      }
      return vector3_1;
    }

    public static Vector3 ForwardVectorProjection(
      Vector3 forwardVector,
      Vector3 projectedVector)
    {
      return (double) Vector3.Dot(projectedVector, forwardVector) > 0.0 ? forwardVector.Project(projectedVector + forwardVector) : Vector3.Zero;
    }

    public static BoundingBox CreateFromInsideRadius(float radius)
    {
      float num = MyMath.OneOverRoot3 * radius;
      return new BoundingBox(-new Vector3(num), new Vector3(num));
    }

    public static Vector3 VectorFromColor(byte red, byte green, byte blue) => new Vector3((float) red / (float) byte.MaxValue, (float) green / (float) byte.MaxValue, (float) blue / (float) byte.MaxValue);

    public static Vector4 VectorFromColor(byte red, byte green, byte blue, byte alpha) => new Vector4((float) red / (float) byte.MaxValue, (float) green / (float) byte.MaxValue, (float) blue / (float) byte.MaxValue, (float) alpha / (float) byte.MaxValue);

    public static float DistanceSquaredFromLineSegment(Vector3 v, Vector3 w, Vector3 p)
    {
      Vector3 vector2 = w - v;
      float num1 = vector2.LengthSquared();
      if ((double) num1 == 0.0)
        return Vector3.DistanceSquared(p, v);
      float num2 = Vector3.Dot(p - v, vector2);
      if ((double) num2 <= 0.0)
        return Vector3.DistanceSquared(p, v);
      return (double) num2 >= (double) num1 ? Vector3.DistanceSquared(p, w) : Vector3.DistanceSquared(p, v + num2 / num1 * vector2);
    }

    public static float Clamp(float val, float min, float max)
    {
      if ((double) val < (double) min)
        return min;
      return (double) val > (double) max ? max : val;
    }
  }
}
