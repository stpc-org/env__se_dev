// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyRidgedMultifractal
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Noise
{
  public class MyRidgedMultifractal : MyModule
  {
    private const int MAX_OCTAVES = 20;
    private double m_lacunarity;
    private double[] m_spectralWeights = new double[20];

    private void CalculateSpectralWeights()
    {
      double num = 1.0;
      double x = 1.0;
      for (int index = 0; index < 20; ++index)
      {
        this.m_spectralWeights[index] = Math.Pow(x, -num);
        x *= this.Lacunarity;
      }
    }

    public MyNoiseQuality Quality { get; set; }

    public int LayerCount { get; set; }

    public int Seed { get; set; }

    public double Frequency { get; set; }

    public double Gain { get; set; }

    public double Lacunarity
    {
      get => this.m_lacunarity;
      set
      {
        this.m_lacunarity = value;
        this.CalculateSpectralWeights();
      }
    }

    public double Offset { get; set; }

    public MyRidgedMultifractal(
      MyNoiseQuality quality = MyNoiseQuality.Standard,
      int layerCount = 6,
      int seed = 0,
      double frequency = 1.0,
      double gain = 2.0,
      double lacunarity = 2.0,
      double offset = 1.0)
    {
      this.Quality = quality;
      this.LayerCount = layerCount;
      this.Seed = seed;
      this.Frequency = frequency;
      this.Gain = gain;
      this.Lacunarity = lacunarity;
      this.Offset = offset;
    }

    public override double GetValue(double x)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        long num3 = (long) (this.Seed + index & int.MaxValue);
        double num4 = this.Offset - Math.Abs(this.GradCoherentNoise(x, (int) num3, this.Quality));
        double num5 = num4 * num4 * num2;
        num2 = MathHelper.Saturate(num5 * this.Gain);
        num1 += num5 * this.m_spectralWeights[index];
        x *= this.Lacunarity;
      }
      return num1 - 1.0;
    }

    public override double GetValue(double x, double y)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      y *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        long num3 = (long) (this.Seed + index & int.MaxValue);
        double num4 = this.Offset - Math.Abs(this.GradCoherentNoise(x, y, (int) num3, this.Quality));
        double num5 = num4 * num4 * num2;
        num2 = MathHelper.Saturate(num5 * this.Gain);
        num1 += num5 * this.m_spectralWeights[index];
        x *= this.Lacunarity;
        y *= this.Lacunarity;
      }
      return num1 - 1.0;
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
        long num3 = (long) (this.Seed + index & int.MaxValue);
        double num4 = this.Offset - Math.Abs(this.GradCoherentNoise(x, y, z, (int) num3, this.Quality));
        double num5 = num4 * num4 * num2;
        num2 = MathHelper.Saturate(num5 * this.Gain);
        num1 += num5 * this.m_spectralWeights[index];
        x *= this.Lacunarity;
        y *= this.Lacunarity;
        z *= this.Lacunarity;
      }
      return num1 * 1.25 - 1.0;
    }
  }
}
