// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Patterns.MySphere
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Patterns
{
  public class MySphere : IMyModule
  {
    private double m_outerRadiusBlendingSqrDist;
    private double m_innerRadius;
    private double m_innerRadiusSqr;
    private double m_outerRadius;
    private double m_outerRadiusSqr;

    public double InnerRadius
    {
      get => this.m_innerRadius;
      set
      {
        this.m_innerRadius = value;
        this.m_innerRadiusSqr = value * value;
        this.UpdateBlendingDistnace();
      }
    }

    public double OuterRadius
    {
      get => this.m_outerRadius;
      set
      {
        this.m_outerRadius = value;
        this.m_outerRadiusSqr = value * value;
        this.UpdateBlendingDistnace();
      }
    }

    private void UpdateBlendingDistnace() => this.m_outerRadiusBlendingSqrDist = this.m_outerRadiusSqr - this.m_innerRadiusSqr;

    public MySphere(double innerRadius, double outerRadius)
    {
      this.InnerRadius = innerRadius;
      this.OuterRadius = outerRadius;
    }

    public double GetValue(double x) => this.ClampDistanceToRadius(x * x);

    public double GetValue(double x, double y) => this.ClampDistanceToRadius(x * x + y * y);

    public double GetValue(double x, double y, double z) => this.ClampDistanceToRadius(x * x + y * y + z * z);

    private double ClampDistanceToRadius(double distanceSqr)
    {
      if (distanceSqr >= this.m_outerRadiusSqr)
        return 0.0;
      return distanceSqr < this.m_innerRadiusSqr ? 1.0 : 1.0 - (distanceSqr - this.m_innerRadiusSqr) / this.m_outerRadiusBlendingSqrDist;
    }
  }
}
