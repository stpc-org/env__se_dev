// Decompiled with JetBrains decompiler
// Type: VRageMath.HyperSphereHelpers
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public static class HyperSphereHelpers
  {
    public static double DistanceToTangentProjected(
      ref Vector3D center,
      ref Vector3D point,
      double radius,
      out double distance)
    {
      double result;
      Vector3D.Distance(ref point, ref center, out result);
      double num1 = radius * radius;
      double num2 = result;
      double num3 = radius;
      double num4 = Math.Sqrt(num2 * num2 - num1);
      double num5 = (num2 + num3 + num4) / 2.0;
      double num6 = 2.0 * Math.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4)) / num2;
      distance = num2 - Math.Sqrt(num1 - num6 * num6);
      return num6;
    }

    public static double DistanceToTangent(ref Vector3D center, ref Vector3D point, double radius)
    {
      double result;
      Vector3D.Distance(ref point, ref center, out result);
      return Math.Sqrt(result * result - radius * radius);
    }

    public static double DistanceToTangent(ref Vector2D center, ref Vector2D point, double radius)
    {
      double result;
      Vector2D.Distance(ref point, ref center, out result);
      return Math.Sqrt(result * result - radius * radius);
    }
  }
}
