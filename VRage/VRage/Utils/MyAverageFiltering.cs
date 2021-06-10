// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyAverageFiltering
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.Utils
{
  public class MyAverageFiltering
  {
    private readonly List<double> m_samples;
    private readonly int m_sampleMaxCount;
    private int m_sampleCursor;
    private double? m_cachedFilteredValue;

    public MyAverageFiltering(int sampleCount)
    {
      this.m_sampleMaxCount = sampleCount;
      this.m_samples = new List<double>(sampleCount);
      this.m_cachedFilteredValue = new double?();
    }

    public void Add(double value)
    {
      this.m_cachedFilteredValue = new double?();
      if (this.m_samples.Count < this.m_sampleMaxCount)
      {
        this.m_samples.Add(value);
      }
      else
      {
        this.m_samples[this.m_sampleCursor++] = value;
        if (this.m_sampleCursor < this.m_sampleMaxCount)
          return;
        this.m_sampleCursor = 0;
      }
    }

    public double Get()
    {
      if (this.m_cachedFilteredValue.HasValue)
        return this.m_cachedFilteredValue.Value;
      double num1 = 0.0;
      foreach (double sample in this.m_samples)
        num1 += sample;
      if (this.m_samples.Count <= 0)
        return 0.0;
      double num2 = num1 / (double) this.m_samples.Count;
      this.m_cachedFilteredValue = new double?(num2);
      return num2;
    }

    public void Clear()
    {
      this.m_samples.Clear();
      this.m_cachedFilteredValue = new double?();
    }
  }
}
