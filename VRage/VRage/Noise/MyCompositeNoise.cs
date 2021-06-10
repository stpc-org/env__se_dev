// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyCompositeNoise
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Utils;

namespace VRage.Noise
{
  public class MyCompositeNoise : MyModule
  {
    private IMyModule[] m_noises;
    private float[] m_amplitudeScales;
    private float m_normalizationFactor = 1f;
    private int m_numNoises;

    public MyCompositeNoise(int numNoises, float startFrequency)
    {
      this.m_numNoises = numNoises;
      this.m_noises = new IMyModule[this.m_numNoises];
      this.m_amplitudeScales = new float[this.m_numNoises];
      this.m_normalizationFactor = (float) (2.0 - 1.0 / Math.Pow(2.0, (double) (this.m_numNoises - 1)));
      float num = startFrequency;
      for (int index = 0; index < this.m_numNoises; ++index)
      {
        this.m_amplitudeScales[index] = 1f / (float) Math.Pow(2.0, (double) index);
        this.m_noises[index] = (IMyModule) new MySimplexFast(MyRandom.Instance.Next(), (double) num);
        num *= 2.01f;
      }
    }

    private double NormalizeValue(double value) => 0.5 * value / (double) this.m_normalizationFactor + 0.5;

    public override double GetValue(double x)
    {
      double num = 0.0;
      for (int index = 0; index < this.m_numNoises; ++index)
        num += (double) this.m_amplitudeScales[index] * this.m_noises[index].GetValue(x);
      return this.NormalizeValue(num);
    }

    public override double GetValue(double x, double y)
    {
      double num = 0.0;
      for (int index = 0; index < this.m_numNoises; ++index)
        num += (double) this.m_amplitudeScales[index] * this.m_noises[index].GetValue(x, y);
      return this.NormalizeValue(num);
    }

    public override double GetValue(double x, double y, double z)
    {
      double num = 0.0;
      for (int index = 0; index < this.m_numNoises; ++index)
        num += (double) this.m_amplitudeScales[index] * this.m_noises[index].GetValue(x, y, z);
      return this.NormalizeValue(num);
    }

    public float GetValue(double x, double y, double z, int numNoises)
    {
      double num = 0.0;
      for (int index = 0; index < numNoises; ++index)
        num += (double) this.m_amplitudeScales[index] * this.m_noises[index].GetValue(x, y, z);
      return (float) (0.5 * num + 0.5);
    }
  }
}
