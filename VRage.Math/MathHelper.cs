// Decompiled with JetBrains decompiler
// Type: VRageMath.MathHelper
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.InteropServices;
using VRage;

namespace VRageMath
{
  public static class MathHelper
  {
    public const float E = 2.718282f;
    public const float Log2E = 1.442695f;
    public const float Log10E = 0.4342945f;
    public const float Pi = 3.141593f;
    public const float TwoPi = 6.283185f;
    public const float FourPi = 12.56637f;
    public const float PiOver2 = 1.570796f;
    public const float PiOver4 = 0.7853982f;
    public const float Sqrt2 = 1.414214f;
    public const float Sqrt3 = 1.732051f;
    public const float RadiansPerSecondToRPM = 9.549296f;
    public const float RPMToRadiansPerSecond = 0.1047198f;
    public const float RPMToRadiansPerMillisec = 0.0001047198f;
    public const float EPSILON = 1E-05f;
    public const float EPSILON10 = 1E-06f;
    private static readonly int[] lof2floor_lut = new int[32]
    {
      0,
      9,
      1,
      10,
      13,
      21,
      2,
      29,
      11,
      14,
      16,
      18,
      22,
      25,
      3,
      30,
      8,
      12,
      20,
      28,
      15,
      17,
      24,
      7,
      19,
      27,
      23,
      6,
      26,
      5,
      4,
      31
    };
    private const float SMOOTHING = 0.95f;

    public static float ToRadians(float degrees) => degrees * ((float) Math.PI / 180f);

    public static Vector3 ToRadians(Vector3 v) => v * ((float) Math.PI / 180f);

    public static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);

    public static float ToDegrees(float radians) => radians * 57.29578f;

    public static double ToDegrees(double radians) => radians * (180.0 / Math.PI);

    public static float Distance(float value1, float value2) => Math.Abs(value1 - value2);

    public static float Min(float value1, float value2) => Math.Min(value1, value2);

    public static float Max(float value1, float value2) => Math.Max(value1, value2);

    public static double Min(double value1, double value2) => Math.Min(value1, value2);

    public static double Max(double value1, double value2) => Math.Max(value1, value2);

    public static float Clamp(float value, float min, float max)
    {
      value = (double) value > (double) max ? max : value;
      value = (double) value < (double) min ? min : value;
      return value;
    }

    public static double Clamp(double value, double min, double max)
    {
      value = value > max ? max : value;
      value = value < min ? min : value;
      return value;
    }

    public static MyFixedPoint Clamp(
      MyFixedPoint value,
      MyFixedPoint min,
      MyFixedPoint max)
    {
      value = value > max ? max : value;
      value = value < min ? min : value;
      return value;
    }

    public static int Clamp(int value, int min, int max)
    {
      value = value > max ? max : value;
      value = value < min ? min : value;
      return value;
    }

    public static float Lerp(float value1, float value2, float amount) => value1 + (value2 - value1) * amount;

    public static double Lerp(double value1, double value2, double amount) => value1 + (value2 - value1) * amount;

    public static float InterpLog(float value, float amount1, float amount2) => (float) (Math.Pow((double) amount1, 1.0 - (double) value) * Math.Pow((double) amount2, (double) value));

    public static float InterpLogInv(float value, float amount1, float amount2) => (float) Math.Log((double) value / (double) amount1, (double) amount2 / (double) amount1);

    public static float Barycentric(
      float value1,
      float value2,
      float value3,
      float amount1,
      float amount2)
    {
      return (float) ((double) value1 + (double) amount1 * ((double) value2 - (double) value1) + (double) amount2 * ((double) value3 - (double) value1));
    }

    public static float SmoothStep(float value1, float value2, float amount) => MathHelper.Lerp(value1, value2, MathHelper.SCurve3(amount));

    public static double SmoothStep(double value1, double value2, double amount) => MathHelper.Lerp(value1, value2, MathHelper.SCurve3(amount));

    public static float SmoothStepStable(float amount)
    {
      float num1 = 1f - amount;
      double num2 = (double) amount;
      float num3 = (float) num2 * amount;
      float num4 = (float) num2 * num1 + amount;
      return (float) ((double) num3 * (double) num1 + (double) num4 * (double) amount);
    }

    public static double SmoothStepStable(double amount)
    {
      double num1 = 1.0 - amount;
      double num2 = amount;
      double num3 = num2 * amount;
      double num4 = num2 * num1 + amount;
      return num3 * num1 + num4 * amount;
    }

    public static float CatmullRom(
      float value1,
      float value2,
      float value3,
      float value4,
      float amount)
    {
      float num1 = amount * amount;
      float num2 = amount * num1;
      return (float) (0.5 * (2.0 * (double) value2 + (-(double) value1 + (double) value3) * (double) amount + (2.0 * (double) value1 - 5.0 * (double) value2 + 4.0 * (double) value3 - (double) value4) * (double) num1 + (-(double) value1 + 3.0 * (double) value2 - 3.0 * (double) value3 + (double) value4) * (double) num2));
    }

    public static float Hermite(
      float value1,
      float tangent1,
      float value2,
      float tangent2,
      float amount)
    {
      float num1 = amount;
      float num2 = num1 * num1;
      float num3 = num1 * num2;
      float num4 = (float) (2.0 * (double) num3 - 3.0 * (double) num2 + 1.0);
      float num5 = (float) (-2.0 * (double) num3 + 3.0 * (double) num2);
      float num6 = num3 - 2f * num2 + num1;
      float num7 = num3 - num2;
      return (float) ((double) value1 * (double) num4 + (double) value2 * (double) num5 + (double) tangent1 * (double) num6 + (double) tangent2 * (double) num7);
    }

    public static Vector3D CalculateBezierPoint(
      double t,
      Vector3D p0,
      Vector3D p1,
      Vector3D p2,
      Vector3D p3)
    {
      double num1 = 1.0 - t;
      double num2 = t * t;
      double num3 = num1 * num1;
      double num4 = num3 * num1;
      double num5 = num2 * t;
      Vector3D vector3D = p0;
      return num4 * vector3D + 3.0 * num3 * t * p1 + 3.0 * num1 * num2 * p2 + num5 * p3;
    }

    public static float WrapAngle(float angle)
    {
      angle = (float) Math.IEEERemainder((double) angle, 6.28318548202515);
      if ((double) angle <= -3.14159274101257)
        angle += 6.283185f;
      else if ((double) angle > 3.14159274101257)
        angle -= 6.283185f;
      return angle;
    }

    public static int GetNearestBiggerPowerOfTwo(int v)
    {
      --v;
      v |= v >> 1;
      v |= v >> 2;
      v |= v >> 4;
      v |= v >> 8;
      v |= v >> 16;
      ++v;
      return v;
    }

    public static uint GetNearestBiggerPowerOfTwo(uint v)
    {
      --v;
      v |= v >> 1;
      v |= v >> 2;
      v |= v >> 4;
      v |= v >> 8;
      v |= v >> 16;
      ++v;
      return v;
    }

    public static int GetNumberOfMipmaps(int v)
    {
      int num = 0;
      while (v > 0)
      {
        v >>= 1;
        ++num;
      }
      return num;
    }

    public static int GetNearestBiggerPowerOfTwo(float f)
    {
      int num = 1;
      while ((double) num < (double) f)
        num <<= 1;
      return num;
    }

    public static int GetNearestBiggerPowerOfTwo(double f)
    {
      int num = 1;
      while ((double) num < f)
        num <<= 1;
      return num;
    }

    public static float Max(float a, float b, float c)
    {
      float num = (double) a > (double) b ? a : b;
      return (double) num <= (double) c ? c : num;
    }

    public static int Max(int a, int b, int c)
    {
      int num = a > b ? a : b;
      return num <= c ? c : num;
    }

    public static float Min(float a, float b, float c)
    {
      float num = (double) a < (double) b ? a : b;
      return (double) num >= (double) c ? c : num;
    }

    public static double Max(double a, double b, double c)
    {
      double num = a > b ? a : b;
      return num <= c ? c : num;
    }

    public static double Min(double a, double b, double c)
    {
      double num = a < b ? a : b;
      return num >= c ? c : num;
    }

    public static unsafe int ComputeHashFromBytes(byte[] bytes)
    {
      int length = bytes.Length;
      int num1 = length - length % 4;
      GCHandle gcHandle = GCHandle.Alloc((object) bytes, GCHandleType.Pinned);
      int num2 = 0;
      try
      {
        int* pointer = (int*) gcHandle.AddrOfPinnedObject().ToPointer();
        int num3 = 0;
        while (num3 < num1)
        {
          num2 ^= *pointer;
          num3 += 4;
          ++pointer;
        }
        return num2;
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static float RoundOn2(float x) => (float) (int) ((double) x * 100.0) / 100f;

    public static int RoundToInt(float x) => (int) Math.Round((double) x);

    public static int RoundToInt(double x) => (int) Math.Round(x);

    public static int FloorToInt(float x) => (int) Math.Floor((double) x);

    public static int FloorToInt(double x) => (int) Math.Floor(x);

    public static int CeilToInt(float x) => (int) Math.Ceiling((double) x);

    public static int CeilToInt(double x) => (int) Math.Ceiling(x);

    public static bool IsPowerOfTwo(int x) => x > 0 && (x & x - 1) == 0;

    public static float SCurve3(float t) => (float) ((double) t * (double) t * (3.0 - 2.0 * (double) t));

    public static double SCurve3(double t) => t * t * (3.0 - 2.0 * t);

    public static float SCurve5(float t) => (float) ((double) t * (double) t * (double) t * ((double) t * ((double) t * 6.0 - 15.0) + 10.0));

    public static double SCurve5(double t) => t * t * t * (t * (t * 6.0 - 15.0) + 10.0);

    public static float Saturate(float n)
    {
      if ((double) n < 0.0)
        return 0.0f;
      return (double) n <= 1.0 ? n : 1f;
    }

    public static double Saturate(double n)
    {
      if (n < 0.0)
        return 0.0;
      return n <= 1.0 ? n : 1.0;
    }

    public static int Floor(float n) => (double) n >= 0.0 ? (int) n : (int) n - 1;

    public static int Floor(double n) => n >= 0.0 ? (int) n : (int) n - 1;

    public static int Log2Floor(int value)
    {
      value |= value >> 1;
      value |= value >> 2;
      value |= value >> 4;
      value |= value >> 8;
      value |= value >> 16;
      return MathHelper.lof2floor_lut[(int) ((uint) (value * 130329821) >> 27)];
    }

    public static int Log2Ceiling(int value)
    {
      value |= value >> 1;
      value |= value >> 2;
      value |= value >> 4;
      value |= value >> 8;
      value |= value >> 16;
      value = MathHelper.lof2floor_lut[(int) ((uint) (value * 130329821) >> 27)];
      return (value & value - 1) == 0 ? value : value + 1;
    }

    public static int Log2(int n)
    {
      int num = 0;
      while ((n >>= 1) > 0)
        ++num;
      return num;
    }

    public static int Log2(uint n)
    {
      int num = 0;
      while ((n >>= 1) > 0U)
        ++num;
      return num;
    }

    public static int Pow2(int n) => 1 << n;

    public static double CubicInterp(double p0, double p1, double p2, double p3, double t)
    {
      double num1 = p3 - p2 - (p0 - p1);
      double num2 = p0 - p1 - num1;
      double num3 = t * t;
      return num1 * num3 * t + num2 * num3 + (p2 - p0) * t + p1;
    }

    public static void LimitRadians2PI(ref double angle)
    {
      if (angle > 6.28318548202515)
      {
        angle %= 6.28318548202515;
      }
      else
      {
        if (angle >= 0.0)
          return;
        angle = angle % 6.28318548202515 + 6.28318548202515;
      }
    }

    public static void LimitRadians(ref float angle)
    {
      if ((double) angle > 6.28318548202515)
      {
        angle %= 6.283185f;
      }
      else
      {
        if ((double) angle >= 0.0)
          return;
        angle = (float) ((double) angle % 6.28318548202515 + 6.28318548202515);
      }
    }

    public static void LimitRadiansPI(ref double angle)
    {
      if (angle > 3.14159297943115)
      {
        angle = angle % 3.14159297943115 - 3.14159297943115;
      }
      else
      {
        if (angle >= -3.14159297943115)
          return;
        angle = angle % 3.14159297943115 + 3.14159297943115;
      }
    }

    public static void LimitRadiansPI(ref float angle)
    {
      if ((double) angle > 3.14159297943115)
      {
        angle = (float) ((double) angle % 3.14159297943115 - 3.14159297943115);
      }
      else
      {
        if ((double) angle >= 3.14159297943115)
          return;
        angle = (float) ((double) angle % 3.14159297943115 + 3.14159297943115);
      }
    }

    public static Vector3 CalculateVectorOnSphere(
      Vector3 northPoleDir,
      float phi,
      float theta)
    {
      double num = Math.Sin((double) theta);
      return Vector3.TransformNormal(new Vector3(Math.Cos((double) phi) * num, Math.Sin((double) phi) * num, Math.Cos((double) theta)), Matrix.CreateFromDir(northPoleDir));
    }

    public static float MonotonicCosine(float radians) => (double) radians > 0.0 ? 2f - (float) Math.Cos((double) radians) : (float) Math.Cos((double) radians);

    public static float MonotonicAcos(float cos) => (double) cos > 1.0 ? (float) Math.Acos(2.0 - (double) cos) : (float) -Math.Acos((double) cos);

    public static float Atan(float x) => (float) (0.785374999046326 * (double) x - (double) x * ((double) Math.Abs(x) - 1.0) * (0.244699999690056 + 0.0662999972701073 * (double) Math.Abs(x)));

    public static double Atan(double x) => 0.785375 * x - x * (Math.Abs(x) - 1.0) * (0.2447 + 0.0663 * Math.Abs(x));

    public static bool IsEqual(float value1, float value2) => MathHelper.IsZero(value1 - value2, 1E-05f);

    public static bool IsEqual(Vector2 value1, Vector2 value2) => MathHelper.IsZero(value1.X - value2.X, 1E-05f) && MathHelper.IsZero(value1.Y - value2.Y, 1E-05f);

    public static bool IsEqual(Vector3 value1, Vector3 value2) => MathHelper.IsZero(value1.X - value2.X, 1E-05f) && MathHelper.IsZero(value1.Y - value2.Y, 1E-05f) && MathHelper.IsZero(value1.Z - value2.Z, 1E-05f);

    public static bool IsEqual(Quaternion value1, Quaternion value2) => MathHelper.IsZero(value1.X - value2.X, 1E-05f) && MathHelper.IsZero(value1.Y - value2.Y, 1E-05f) && MathHelper.IsZero(value1.Z - value2.Z, 1E-05f) && MathHelper.IsZero(value1.W - value2.W, 1E-05f);

    public static bool IsEqual(QuaternionD value1, QuaternionD value2) => MathHelper.IsZero(value1.X - value2.X) && MathHelper.IsZero(value1.Y - value2.Y) && MathHelper.IsZero(value1.Z - value2.Z) && MathHelper.IsZero(value1.W - value2.W);

    public static bool IsEqual(Matrix value1, Matrix value2) => MathHelper.IsZero(value1.Left - value2.Left) && MathHelper.IsZero(value1.Up - value2.Up) && MathHelper.IsZero(value1.Forward - value2.Forward) && MathHelper.IsZero(value1.Translation - value2.Translation);

    public static bool IsValid(Matrix matrix) => matrix.Up.IsValid() && matrix.Left.IsValid() && (matrix.Forward.IsValid() && matrix.Translation.IsValid()) && matrix != Matrix.Zero;

    public static bool IsValid(MatrixD matrix) => matrix.Up.IsValid() && matrix.Left.IsValid() && (matrix.Forward.IsValid() && matrix.Translation.IsValid()) && matrix != MatrixD.Zero;

    public static bool IsValid(Vector3 vec) => MathHelper.IsValid(vec.X) && MathHelper.IsValid(vec.Y) && MathHelper.IsValid(vec.Z);

    public static bool IsValid(Vector3D vec) => MathHelper.IsValid(vec.X) && MathHelper.IsValid(vec.Y) && MathHelper.IsValid(vec.Z);

    public static bool IsValid(Vector2 vec) => MathHelper.IsValid(vec.X) && MathHelper.IsValid(vec.Y);

    public static bool IsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    public static bool IsValid(double f) => !double.IsNaN(f) && !double.IsInfinity(f);

    public static bool IsValid(Vector3? vec)
    {
      if (!vec.HasValue)
        return true;
      return MathHelper.IsValid(vec.Value.X) && MathHelper.IsValid(vec.Value.Y) && MathHelper.IsValid(vec.Value.Z);
    }

    public static bool IsValid(Quaternion q) => MathHelper.IsValid(q.X) && MathHelper.IsValid(q.Y) && (MathHelper.IsValid(q.Z) && MathHelper.IsValid(q.W)) && !MathHelper.IsZero(q);

    public static bool IsValidNormal(Vector3 vec)
    {
      float num = vec.LengthSquared();
      return vec.IsValid() && (double) num > 0.999000012874603 && (double) num < 1.00100004673004;
    }

    public static bool IsValidOrZero(Matrix matrix) => MathHelper.IsValid(matrix.Up) && MathHelper.IsValid(matrix.Left) && MathHelper.IsValid(matrix.Forward) && MathHelper.IsValid(matrix.Translation);

    public static bool IsZero(float value, float epsilon = 1E-05f) => (double) value > -(double) epsilon && (double) value < (double) epsilon;

    public static bool IsZero(double value, float epsilon = 1E-05f) => value > -(double) epsilon && value < (double) epsilon;

    public static bool IsZero(Vector3 value, float epsilon = 1E-05f) => MathHelper.IsZero(value.X, epsilon) && MathHelper.IsZero(value.Y, epsilon) && MathHelper.IsZero(value.Z, epsilon);

    public static bool IsZero(Vector3D value, float epsilon = 1E-05f) => MathHelper.IsZero(value.X, epsilon) && MathHelper.IsZero(value.Y, epsilon) && MathHelper.IsZero(value.Z, epsilon);

    public static bool IsZero(Quaternion value, float epsilon = 1E-05f) => MathHelper.IsZero(value.X, epsilon) && MathHelper.IsZero(value.Y, epsilon) && MathHelper.IsZero(value.Z, epsilon) && MathHelper.IsZero(value.W, epsilon);

    public static bool IsZero(Vector4 value) => MathHelper.IsZero(value.X, 1E-05f) && MathHelper.IsZero(value.Y, 1E-05f) && MathHelper.IsZero(value.Z, 1E-05f) && MathHelper.IsZero(value.W, 1E-05f);

    public static int Smooth(int newValue, int lastSmooth) => (int) ((double) lastSmooth * 0.949999988079071 + (double) newValue * 0.050000011920929);

    public static float Smooth(float newValue, float lastSmooth) => (float) ((double) lastSmooth * 0.949999988079071 + (double) newValue * 0.050000011920929);

    public static int Align(int value, int alignment) => (value + alignment - 1) / alignment * alignment;
  }
}
