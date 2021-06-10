// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyBillow
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Noise
{
  public class MyBillow : MyModule
  {
    public MyNoiseQuality Quality { get; set; }

    public int LayerCount { get; set; }

    public int Seed { get; set; }

    public double Frequency { get; set; }

    public double Lacunarity { get; set; }

    public double Persistence { get; set; }

    public MyBillow(
      MyNoiseQuality quality = MyNoiseQuality.Standard,
      int layerCount = 6,
      int seed = 0,
      double frequency = 1.0,
      double lacunarity = 2.0,
      double persistence = 0.5)
    {
      this.Quality = quality;
      this.LayerCount = layerCount;
      this.Seed = seed;
      this.Frequency = frequency;
      this.Lacunarity = lacunarity;
      this.Persistence = persistence;
    }

    public override double GetValue(double x)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        long num3 = (long) (this.Seed + index) & (long) uint.MaxValue;
        double num4 = 2.0 * Math.Abs(this.GradCoherentNoise(x, (int) num3, this.Quality)) - 1.0;
        num1 += num4 * num2;
        x *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return MathHelper.Clamp(num1 + 0.5, -1.0, 1.0);
    }

    public override double GetValue(double x, double y)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      y *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        long num3 = (long) (this.Seed + index) & (long) uint.MaxValue;
        double num4 = 2.0 * Math.Abs(this.GradCoherentNoise(x, y, (int) num3, this.Quality)) - 1.0;
        num1 += num4 * num2;
        x *= this.Lacunarity;
        y *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return MathHelper.Clamp(num1 + 0.5, -1.0, 1.0);
    }

    public override double GetValue(double x, double y, double z)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      y *= this.Frequency;
      z *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        long num3 = (long) (this.Seed + index) & (long) uint.MaxValue;
        double num4 = 2.0 * Math.Abs(this.GradCoherentNoise(x, y, z, (int) num3, this.Quality)) - 1.0;
        num1 += num4 * num2;
        x *= this.Lacunarity;
        y *= this.Lacunarity;
        z *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return MathHelper.Clamp(num1 + 0.5, -1.0, 1.0);
    }
  }
}
