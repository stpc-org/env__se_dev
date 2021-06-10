// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyValueAggregator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Utils
{
  public class MyValueAggregator
  {
    private readonly int m_bufferSize;
    private readonly int[] m_percentiles;
    private readonly double[] m_data;
    private readonly double[] m_percentileSums;
    private int m_dataSize;
    private int m_flushNumber;
    private readonly object m_lock = new object();

    public MyValueAggregator(int bufferSize, params int[] percentiles)
    {
      if (percentiles.Length == 0)
        throw new ArgumentException("at least one percentile should be specified", nameof (percentiles));
      foreach (int percentile in percentiles)
      {
        if (percentile < 0 || percentile > 100)
          throw new ArgumentOutOfRangeException(nameof (percentiles), "percentile should have value in range [0, 100]");
      }
      if (bufferSize < percentiles.Length)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), "should not be less than number of percentiles");
      this.m_bufferSize = bufferSize;
      this.m_percentiles = percentiles;
      this.m_data = new double[this.m_bufferSize];
      this.m_percentileSums = new double[this.m_percentiles.Length];
    }

    public void Push(double value)
    {
      lock (this.m_lock)
      {
        this.m_data[this.m_dataSize] = value;
        ++this.m_dataSize;
        if (this.m_dataSize != this.m_bufferSize)
          return;
        this.Flush();
      }
    }

    public void GetPercentileValues(double[] valuesBuffer)
    {
      if (valuesBuffer == null)
        throw new ArgumentNullException(nameof (valuesBuffer));
      if (valuesBuffer.Length != this.m_percentiles.Length)
        throw new ArgumentOutOfRangeException(nameof (valuesBuffer), "should be exact same length as percentiles array");
      lock (this.m_lock)
      {
        if (this.CanForceFlush)
          this.Flush();
        double num = 1.0 / (double) this.m_flushNumber;
        for (int index = 0; index < this.m_percentileSums.Length; ++index)
          valuesBuffer[index] = this.m_percentileSums[index] * num;
      }
    }

    public double[] PercentileValues
    {
      get
      {
        double[] valuesBuffer = new double[this.m_percentiles.Length];
        this.GetPercentileValues(valuesBuffer);
        return valuesBuffer;
      }
    }

    public bool HasData
    {
      get
      {
        lock (this.m_lock)
          return this.m_flushNumber > 0 || this.CanForceFlush;
      }
    }

    private void Flush()
    {
      Array.Sort<double>(this.m_data, 0, this.m_dataSize);
      for (int index1 = 0; index1 < this.m_percentiles.Length; ++index1)
      {
        int index2 = this.m_percentiles[index1] * (this.m_dataSize - 1) / 100;
        this.m_percentileSums[index1] += this.m_data[index2];
      }
      ++this.m_flushNumber;
      this.m_dataSize = 0;
    }

    private bool CanForceFlush => this.m_dataSize > this.m_bufferSize * 100 / 20;

    public void Clear()
    {
      this.m_flushNumber = 0;
      this.m_dataSize = 0;
      Array.Clear((Array) this.m_percentileSums, 0, this.m_percentileSums.Length);
    }
  }
}
