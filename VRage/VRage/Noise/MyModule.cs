// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyModule
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Noise
{
  public abstract class MyModule : IMyModule
  {
    private double GradNoise(double fx, int ix, long seed)
    {
      long num = (long) (1619 * ix) + 1013L * seed & (long) uint.MaxValue;
      long index = (num >> 8 ^ num) & (long) byte.MaxValue;
      return MyNoiseDefaults.RandomVectors[index] * (fx - (double) ix);
    }

    private double GradNoise(double fx, double fy, int ix, int iy, long seed)
    {
      long num1 = (long) (1619 * ix + 31337 * iy) + 1013L * seed & (long) uint.MaxValue;
      long index = ((num1 >> 8 ^ num1) & (long) byte.MaxValue) << 1;
      double randomVector1 = MyNoiseDefaults.RandomVectors[index];
      double randomVector2 = MyNoiseDefaults.RandomVectors[index + 1L];
      double num2 = fx - (double) ix;
      double num3 = fy - (double) iy;
      double num4 = num2;
      return randomVector1 * num4 + randomVector2 * num3;
    }

    private double GradNoise(
      double fx,
      double fy,
      double fz,
      int ix,
      int iy,
      int iz,
      long seed)
    {
      long num1 = (long) (1619 * ix + 31337 * iy + 6971 * iz) + 1013L * seed & (long) int.MaxValue;
      long index = ((num1 >> 8 ^ num1) & (long) byte.MaxValue) * 3L;
      double randomVector1 = MyNoiseDefaults.RandomVectors[index];
      double randomVector2 = MyNoiseDefaults.RandomVectors[index + 1L];
      double randomVector3 = MyNoiseDefaults.RandomVectors[index + 2L];
      double num2 = fx - (double) ix;
      double num3 = fy - (double) iy;
      double num4 = fz - (double) iz;
      double num5 = num2;
      return randomVector1 * num5 + randomVector2 * num3 + randomVector3 * num4;
    }

    protected double GradCoherentNoise(double x, int seed, MyNoiseQuality quality)
    {
      int ix = MathHelper.Floor(x);
      double amount = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount = x - (double) ix;
          break;
        case MyNoiseQuality.Standard:
          amount = MathHelper.SCurve3(x - (double) ix);
          break;
        case MyNoiseQuality.High:
          amount = MathHelper.SCurve5(x - (double) ix);
          break;
      }
      return MathHelper.Lerp(this.GradNoise(x, ix, (long) seed), this.GradNoise(x, ix + 1, (long) seed), amount);
    }

    protected double GradCoherentNoise(double x, double y, int seed, MyNoiseQuality quality)
    {
      int ix1 = MathHelper.Floor(x);
      int iy1 = MathHelper.Floor(y);
      int ix2 = ix1 + 1;
      int iy2 = iy1 + 1;
      double amount1 = 0.0;
      double amount2 = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount1 = x - (double) ix1;
          amount2 = y - (double) iy1;
          break;
        case MyNoiseQuality.Standard:
          amount1 = MathHelper.SCurve3(x - (double) ix1);
          amount2 = MathHelper.SCurve3(y - (double) iy1);
          break;
        case MyNoiseQuality.High:
          amount1 = MathHelper.SCurve5(x - (double) ix1);
          amount2 = MathHelper.SCurve5(y - (double) iy1);
          break;
      }
      return MathHelper.Lerp(MathHelper.Lerp(this.GradNoise(x, y, ix1, iy1, (long) seed), this.GradNoise(x, y, ix2, iy1, (long) seed), amount1), MathHelper.Lerp(this.GradNoise(x, y, ix1, iy2, (long) seed), this.GradNoise(x, y, ix2, iy2, (long) seed), amount1), amount2);
    }

    protected double GradCoherentNoise(
      double x,
      double y,
      double z,
      int seed,
      MyNoiseQuality quality)
    {
      int ix1 = MathHelper.Floor(x);
      int iy1 = MathHelper.Floor(y);
      int iz1 = MathHelper.Floor(z);
      int ix2 = ix1 + 1;
      int iy2 = iy1 + 1;
      int iz2 = iz1 + 1;
      double amount1 = 0.0;
      double amount2 = 0.0;
      double amount3 = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount1 = x - (double) ix1;
          amount2 = y - (double) iy1;
          amount3 = z - (double) iz1;
          break;
        case MyNoiseQuality.Standard:
          amount1 = MathHelper.SCurve3(x - (double) ix1);
          amount2 = MathHelper.SCurve3(y - (double) iy1);
          amount3 = MathHelper.SCurve3(z - (double) iz1);
          break;
        case MyNoiseQuality.High:
          amount1 = MathHelper.SCurve5(x - (double) ix1);
          amount2 = MathHelper.SCurve5(y - (double) iy1);
          amount3 = MathHelper.SCurve5(z - (double) iz1);
          break;
      }
      return MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp(this.GradNoise(x, y, z, ix1, iy1, iz1, (long) seed), this.GradNoise(x, y, z, ix2, iy1, iz1, (long) seed), amount1), MathHelper.Lerp(this.GradNoise(x, y, z, ix1, iy2, iz1, (long) seed), this.GradNoise(x, y, z, ix2, iy2, iz1, (long) seed), amount1), amount2), MathHelper.Lerp(MathHelper.Lerp(this.GradNoise(x, y, z, ix1, iy1, iz2, (long) seed), this.GradNoise(x, y, z, ix2, iy1, iz2, (long) seed), amount1), MathHelper.Lerp(this.GradNoise(x, y, z, ix1, iy2, iz2, (long) seed), this.GradNoise(x, y, z, ix2, iy2, iz2, (long) seed), amount1), amount2), amount3);
    }

    public abstract double GetValue(double x);

    public abstract double GetValue(double x, double y);

    public abstract double GetValue(double x, double y, double z);
  }
}
