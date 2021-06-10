﻿// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyBillowFast
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise
{
  public class MyBillowFast : MyModuleFast
  {
    public MyNoiseQuality Quality { get; set; }

    public int LayerCount { get; set; }

    public double Frequency { get; set; }

    public double Lacunarity { get; set; }

    public double Persistence { get; set; }

    public MyBillowFast(
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
        int seed = this.Seed;
        double num3 = 2.0 * Math.Abs(this.GradCoherentNoise(x, this.Quality)) - 1.0;
        num1 += num3 * num2;
        x *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return num1 + 0.5;
    }

    public override double GetValue(double x, double y)
    {
      double num1 = 0.0;
      double num2 = 1.0;
      x *= this.Frequency;
      y *= this.Frequency;
      for (int index = 0; index < this.LayerCount; ++index)
      {
        int seed = this.Seed;
        double num3 = 2.0 * Math.Abs(this.GradCoherentNoise(x, y, this.Quality)) - 1.0;
        num1 += num3 * num2;
        x *= this.Lacunarity;
        y *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return num1 + 0.5;
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
        int seed = this.Seed;
        double num3 = 2.0 * Math.Abs(this.GradCoherentNoise(x, y, z, this.Quality)) - 1.0;
        num1 += num3 * num2;
        x *= this.Lacunarity;
        y *= this.Lacunarity;
        z *= this.Lacunarity;
        num2 *= this.Persistence;
      }
      return num1 + 0.5;
    }
  }
}
