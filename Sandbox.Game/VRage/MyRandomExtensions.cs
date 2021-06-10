// Decompiled with JetBrains decompiler
// Type: VRage.MyRandomExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Library.Utils;
using VRageMath;

namespace VRage
{
  public static class MyRandomExtensions
  {
    public static float FloatNormal(this MyRandom rnd)
    {
      double d = rnd.NextDouble();
      double num = rnd.NextDouble();
      return (float) (Math.Sqrt(-2.0 * Math.Log(d)) * Math.Sin(2.0 * Math.PI * num));
    }

    public static float FloatNormal(this MyRandom rnd, float mean, float standardDeviation)
    {
      if ((double) standardDeviation <= 0.0)
        throw new ArgumentOutOfRangeException(string.Format("Shape must be positive. Received {0}.", (object) standardDeviation));
      return mean + standardDeviation * rnd.FloatNormal();
    }

    public static float FloatExponential(this MyRandom rnd, float mean)
    {
      if ((double) mean <= 0.0)
        throw new ArgumentOutOfRangeException(string.Format("Mean of exponential distribution must be positive. Received {0}.", (object) mean));
      return (float) -Math.Log(rnd.NextDouble()) * mean;
    }

    public static float phi(float x)
    {
      float num1 = 1f;
      if ((double) x < 0.0)
        num1 = -1f;
      x = Math.Abs(x) / (float) Math.Sqrt(2.0);
      float num2 = (float) (1.0 / (1.0 + 0.327591091394424 * (double) x));
      float num3 = (float) (1.0 - ((((1.06140542030334 * (double) num2 - 1.45315206050873) * (double) num2 + 1.42141377925873) * (double) num2 - 0.28449672460556) * (double) num2 + 0.254829585552216) * (double) num2 * Math.Exp(-(double) x * (double) x));
      return (float) (0.5 * (1.0 + (double) num1 * (double) num3));
    }

    public static float NextFloat(this MyRandom random, float minValue, float maxValue) => (float) random.NextDouble() * (maxValue - minValue) + minValue;

    public static Vector3 NextDeviatingVector(
      this MyRandom random,
      ref Matrix matrix,
      float maxAngle)
    {
      float angle1 = random.NextFloat(-maxAngle, maxAngle);
      float angle2 = random.NextFloat(0.0f, 6.283185f);
      return Vector3.TransformNormal(-new Vector3(MyMath.FastSin(angle1) * MyMath.FastCos(angle2), MyMath.FastSin(angle1) * MyMath.FastSin(angle2), MyMath.FastCos(angle1)), matrix);
    }
  }
}
