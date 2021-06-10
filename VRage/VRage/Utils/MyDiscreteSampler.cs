// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyDiscreteSampler
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Library.Utils;

namespace VRage.Utils
{
  public class MyDiscreteSampler
  {
    private MyDiscreteSampler.SamplingBin[] m_bins;
    private int m_binCount;
    private bool m_initialized;

    public bool Initialized => this.m_initialized;

    public MyDiscreteSampler()
    {
      this.m_binCount = 0;
      this.m_bins = (MyDiscreteSampler.SamplingBin[]) null;
      this.m_initialized = false;
    }

    public MyDiscreteSampler(int prealloc)
      : this()
      => this.m_bins = new MyDiscreteSampler.SamplingBin[prealloc];

    public void Prepare(IEnumerable<float> densities)
    {
      float num = 0.0f;
      int numDensities = 0;
      foreach (float density in densities)
      {
        num += density;
        ++numDensities;
      }
      if (numDensities == 0)
        return;
      float normalizationFactor = (float) numDensities / num;
      this.AllocateBins(numDensities);
      this.InitializeBins(densities, normalizationFactor);
      this.ProcessDonators();
      this.m_initialized = true;
    }

    private void InitializeBins(IEnumerable<float> densities, float normalizationFactor)
    {
      int index = 0;
      foreach (float density in densities)
      {
        this.m_bins[index].BinIndex = index;
        this.m_bins[index].Split = density * normalizationFactor;
        this.m_bins[index].Donator = 0;
        ++index;
      }
      Array.Sort<MyDiscreteSampler.SamplingBin>(this.m_bins, 0, this.m_binCount, (IComparer<MyDiscreteSampler.SamplingBin>) MyDiscreteSampler.BinComparer.Static);
    }

    private void AllocateBins(int numDensities)
    {
      if (this.m_bins == null || this.m_binCount < numDensities)
        this.m_bins = new MyDiscreteSampler.SamplingBin[numDensities];
      this.m_binCount = numDensities;
    }

    private void ProcessDonators()
    {
      int index1 = 0;
      int num = 1;
      int index2 = this.m_binCount - 1;
      while (num <= index2)
      {
        this.m_bins[index1].Donator = this.m_bins[index2].BinIndex;
        this.m_bins[index2].Split -= 1f - this.m_bins[index1].Split;
        if ((double) this.m_bins[index2].Split < 1.0)
        {
          index1 = index2;
          --index2;
        }
        else
        {
          index1 = num;
          ++num;
        }
      }
    }

    public int Sample(MyRandom rng)
    {
      MyDiscreteSampler.SamplingBin bin = this.m_bins[rng.Next(this.m_binCount)];
      return (double) rng.NextFloat() <= (double) bin.Split ? bin.BinIndex : bin.Donator;
    }

    public int Sample(float rate)
    {
      double num1;
      int index = (int) (num1 = (double) this.m_binCount * (double) rate);
      if (index == this.m_binCount)
        --index;
      MyDiscreteSampler.SamplingBin bin = this.m_bins[index];
      double num2 = (double) index;
      return num1 - num2 < (double) bin.Split ? bin.BinIndex : bin.Donator;
    }

    public int Sample()
    {
      MyDiscreteSampler.SamplingBin bin = this.m_bins[MyUtils.GetRandomInt(this.m_binCount)];
      return (double) MyUtils.GetRandomFloat() <= (double) bin.Split ? bin.BinIndex : bin.Donator;
    }

    public MyDiscreteSampler.SamplingBin[] ReadBins()
    {
      MyDiscreteSampler.SamplingBin[] samplingBinArray = new MyDiscreteSampler.SamplingBin[this.m_binCount];
      Array.Copy((Array) this.m_bins, (Array) samplingBinArray, samplingBinArray.Length);
      return samplingBinArray;
    }

    public struct SamplingBin
    {
      public float Split;
      public int BinIndex;
      public int Donator;

      public override string ToString() => "[" + (object) this.BinIndex + "] <- (" + (object) this.Donator + ") : " + (object) this.Split;
    }

    private class BinComparer : IComparer<MyDiscreteSampler.SamplingBin>
    {
      public static MyDiscreteSampler.BinComparer Static = new MyDiscreteSampler.BinComparer();

      public int Compare(MyDiscreteSampler.SamplingBin x, MyDiscreteSampler.SamplingBin y)
      {
        float num = x.Split - y.Split;
        if ((double) num < 0.0)
          return -1;
        return (double) num > 0.0 ? 1 : 0;
      }
    }
  }
}
