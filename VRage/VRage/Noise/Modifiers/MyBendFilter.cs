// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyBendFilter
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Modifiers
{
  public class MyBendFilter : IMyModule
  {
    private double m_rangeSizeInverted;
    private double m_clampingMin;
    private double m_clampingMax;

    public IMyModule Module { get; set; }

    public double OutOfRangeMin { get; set; }

    public double OutOfRangeMax { get; set; }

    public double ClampingMin
    {
      get => this.m_clampingMin;
      set
      {
        this.m_clampingMin = value;
        this.m_rangeSizeInverted = 1.0 / (this.m_clampingMax - this.m_clampingMin);
      }
    }

    public double ClampingMax
    {
      get => this.m_clampingMax;
      set
      {
        this.m_clampingMax = value;
        this.m_rangeSizeInverted = 1.0 / (this.m_clampingMax - this.m_clampingMin);
      }
    }

    public MyBendFilter(
      IMyModule module,
      double clampRangeMin,
      double clampRangeMax,
      double outOfRangeMin,
      double outOfRangeMax)
    {
      this.Module = module;
      this.m_rangeSizeInverted = 1.0 / (clampRangeMax - clampRangeMin);
      this.m_clampingMin = clampRangeMin;
      this.m_clampingMax = clampRangeMax;
      this.OutOfRangeMin = outOfRangeMin;
      this.OutOfRangeMax = outOfRangeMax;
    }

    public double GetValue(double x) => this.expandRange(this.Module.GetValue(x));

    public double GetValue(double x, double y) => this.expandRange(this.Module.GetValue(x, y));

    public double GetValue(double x, double y, double z) => this.expandRange(this.Module.GetValue(x, y, z));

    private double expandRange(double value)
    {
      if (value < this.m_clampingMin)
        return this.OutOfRangeMin;
      return value > this.m_clampingMax ? this.OutOfRangeMax : (value - this.m_clampingMin) * this.m_rangeSizeInverted;
    }
  }
}
