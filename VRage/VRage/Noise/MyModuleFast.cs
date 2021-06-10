// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyModuleFast
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Noise
{
  public abstract class MyModuleFast : IMyModule
  {
    private int m_seed;
    private byte[] m_perm = new byte[512];
    private float[] m_grad = new float[512];

    protected double GradCoherentNoise(double x, MyNoiseQuality quality)
    {
      int num = MathHelper.Floor(x);
      int index = num & (int) byte.MaxValue;
      double amount = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount = x - (double) num;
          break;
        case MyNoiseQuality.Standard:
          amount = MathHelper.SCurve3(x - (double) num);
          break;
        case MyNoiseQuality.High:
          amount = MathHelper.SCurve5(x - (double) num);
          break;
      }
      return MathHelper.Lerp((double) this.m_grad[(int) this.m_perm[index]], (double) this.m_grad[(int) this.m_perm[index + 1]], amount);
    }

    protected double GradCoherentNoise(double x, double y, MyNoiseQuality quality)
    {
      int num1 = MathHelper.Floor(x);
      int num2 = MathHelper.Floor(y);
      int index1 = num1 & (int) byte.MaxValue;
      int num3 = num2 & (int) byte.MaxValue;
      double amount1 = 0.0;
      double amount2 = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount1 = x - (double) num1;
          amount2 = y - (double) num2;
          break;
        case MyNoiseQuality.Standard:
          amount1 = MathHelper.SCurve3(x - (double) num1);
          amount2 = MathHelper.SCurve3(y - (double) num2);
          break;
        case MyNoiseQuality.High:
          amount1 = MathHelper.SCurve5(x - (double) num1);
          amount2 = MathHelper.SCurve5(y - (double) num2);
          break;
      }
      int index2 = (int) this.m_perm[index1] + num3;
      int index3 = (int) this.m_perm[index1 + 1] + num3;
      int index4 = (int) this.m_perm[index2];
      int index5 = (int) this.m_perm[index2 + 1];
      int index6 = (int) this.m_perm[index3];
      int index7 = (int) this.m_perm[index3 + 1];
      return MathHelper.Lerp(MathHelper.Lerp((double) this.m_grad[index4], (double) this.m_grad[index6], amount1), MathHelper.Lerp((double) this.m_grad[index5], (double) this.m_grad[index7], amount1), amount2);
    }

    protected double GradCoherentNoise(double x, double y, double z, MyNoiseQuality quality)
    {
      int num1 = MathHelper.Floor(x);
      int num2 = MathHelper.Floor(y);
      int num3 = MathHelper.Floor(z);
      int index1 = num1 & (int) byte.MaxValue;
      int num4 = num2 & (int) byte.MaxValue;
      int num5 = num3 & (int) byte.MaxValue;
      double amount1 = 0.0;
      double amount2 = 0.0;
      double amount3 = 0.0;
      switch (quality)
      {
        case MyNoiseQuality.Low:
          amount1 = x - (double) num1;
          amount2 = y - (double) num2;
          amount3 = z - (double) num3;
          break;
        case MyNoiseQuality.Standard:
          amount1 = MathHelper.SCurve3(x - (double) num1);
          amount2 = MathHelper.SCurve3(y - (double) num2);
          amount3 = MathHelper.SCurve3(z - (double) num3);
          break;
        case MyNoiseQuality.High:
          amount1 = MathHelper.SCurve5(x - (double) num1);
          amount2 = MathHelper.SCurve5(y - (double) num2);
          amount3 = MathHelper.SCurve5(z - (double) num3);
          break;
      }
      int index2 = (int) this.m_perm[index1] + num4;
      int index3 = (int) this.m_perm[index1 + 1] + num4;
      int index4 = (int) this.m_perm[index2] + num5;
      int index5 = (int) this.m_perm[index2 + 1] + num5;
      int index6 = (int) this.m_perm[index3] + num5;
      int index7 = (int) this.m_perm[index3 + 1] + num5;
      return MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp((double) this.m_grad[index4], (double) this.m_grad[index6], amount1), MathHelper.Lerp((double) this.m_grad[index5], (double) this.m_grad[index7], amount1), amount2), MathHelper.Lerp(MathHelper.Lerp((double) this.m_grad[index4 + 1], (double) this.m_grad[index6 + 1], amount1), MathHelper.Lerp((double) this.m_grad[index5 + 1], (double) this.m_grad[index7 + 1], amount1), amount2), amount3);
    }

    public virtual int Seed
    {
      get => this.m_seed;
      set
      {
        this.m_seed = value;
        Random random = new Random(MyRandom.EnableDeterminism ? 1 : this.m_seed);
        for (int index = 0; index < 256; ++index)
        {
          byte num = (byte) random.Next((int) byte.MaxValue);
          this.m_perm[index] = num;
          this.m_perm[256 + index] = num;
          this.m_grad[index] = (float) (2.0 * ((double) this.m_perm[index] / (double) byte.MaxValue) - 1.0);
          this.m_grad[256 + index] = this.m_grad[index];
        }
      }
    }

    public abstract double GetValue(double x);

    public abstract double GetValue(double x, double y);

    public abstract double GetValue(double x, double y, double z);
  }
}
