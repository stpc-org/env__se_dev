// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyTerrace
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Noise.Modifiers
{
  internal class MyTerrace : IMyModule
  {
    public List<double> ControlPoints;

    private double Terrace(double value, int countMask)
    {
      int index1 = 0;
      while (index1 <= countMask && value >= this.ControlPoints[index1])
        ++index1;
      int index2 = MathHelper.Clamp(index1 - 1, 0, countMask);
      int index3 = MathHelper.Clamp(index1, 0, countMask);
      if (index2 == index3)
        return this.ControlPoints[index3];
      double num1 = this.ControlPoints[index2];
      double num2 = this.ControlPoints[index3];
      double num3 = (value - num1) / (num2 - num1);
      if (this.Invert)
      {
        num3 = 1.0 - num3;
        double num4 = num1;
        num1 = num2;
        num2 = num4;
      }
      double amount = num3 * num3;
      return MathHelper.Lerp(num1, num2, amount);
    }

    public IMyModule Module { get; set; }

    public bool Invert { get; set; }

    public MyTerrace(IMyModule module, bool invert = false)
    {
      this.Module = module;
      this.Invert = invert;
      this.ControlPoints = new List<double>(2);
    }

    public double GetValue(double x) => this.Terrace(this.Module.GetValue(x), this.ControlPoints.Count - 1);

    public double GetValue(double x, double y) => this.Terrace(this.Module.GetValue(x, y), this.ControlPoints.Count - 1);

    public double GetValue(double x, double y, double z) => this.Terrace(this.Module.GetValue(x, y, z), this.ControlPoints.Count - 1);
  }
}
