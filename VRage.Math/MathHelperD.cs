// Decompiled with JetBrains decompiler
// Type: VRageMath.MathHelperD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public static class MathHelperD
  {
    public const double E = 2.71828182845905;
    public const double Pi = 3.14159265358979;
    public const double TwoPi = 6.28318530717959;
    public const double FourPi = 12.5663706143592;
    public const double PiOver2 = 1.5707963267949;
    public const double PiOver4 = 0.785398163397448;

    public static double ToRadians(double degrees) => degrees / 180.0 * Math.PI;

    public static double ToDegrees(double radians) => radians * 180.0 / Math.PI;

    public static double Distance(double value1, double value2) => Math.Abs(value1 - value2);

    public static double Min(double value1, double value2) => Math.Min(value1, value2);

    public static double Max(double value1, double value2) => Math.Max(value1, value2);

    public static double Clamp(double value, double min, double max)
    {
      value = value > max ? max : value;
      value = value < min ? min : value;
      return value;
    }

    public static float MonotonicAcos(float cos) => (double) cos > 1.0 ? (float) Math.Acos(2.0 - (double) cos) : (float) -Math.Acos((double) cos);
  }
}
