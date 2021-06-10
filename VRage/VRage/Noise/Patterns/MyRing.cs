// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Patterns.MyRing
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Patterns
{
  public class MyRing : IMyModule
  {
    private double m_thickness;
    private double m_thicknessSqr;

    public double Radius { get; set; }

    public double Thickness
    {
      get => this.m_thickness;
      set
      {
        this.m_thickness = value;
        this.m_thicknessSqr = value * value;
      }
    }

    public MyRing(double radius, double thickness)
    {
      this.Radius = radius;
      this.Thickness = thickness;
    }

    public double GetValue(double x)
    {
      double num = Math.Sqrt(x * x) - this.Radius;
      return this.clampToRing(num * num);
    }

    public double GetValue(double x, double y)
    {
      double num = Math.Sqrt(x * x + y * y) - this.Radius;
      return this.clampToRing(num * num);
    }

    public double GetValue(double x, double y, double z)
    {
      if (Math.Abs(z) >= this.Thickness)
        return 0.0;
      double num1 = Math.Sqrt(x * x + y * y);
      if (Math.Abs(num1 - this.Radius) >= this.Thickness)
        return 0.0;
      double num2 = x / num1 * this.Radius - x;
      double num3 = y / num1 * this.Radius - y;
      return this.clampToRing(num2 * num2 + num3 * num3 + z * z);
    }

    private double clampToRing(double squareDstFromRing) => squareDstFromRing < this.m_thicknessSqr ? 1.0 - squareDstFromRing / this.m_thicknessSqr : 0.0;
  }
}
