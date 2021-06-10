// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyCurve
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Noise.Modifiers
{
  public class MyCurve : IMyModule
  {
    public List<MyCurveControlPoint> ControlPoints;

    public IMyModule Module { get; set; }

    public MyCurve(IMyModule module)
    {
      this.Module = module;
      this.ControlPoints = new List<MyCurveControlPoint>(4);
    }

    public double GetValue(double x)
    {
      double num = this.Module.GetValue(x);
      int max = this.ControlPoints.Count - 1;
      int index1 = 0;
      while (index1 <= max && num >= this.ControlPoints[index1].Input)
        ++index1;
      int index2 = MathHelper.Clamp(index1 - 2, 0, max);
      int index3 = MathHelper.Clamp(index1 - 1, 0, max);
      int index4 = MathHelper.Clamp(index1, 0, max);
      int index5 = MathHelper.Clamp(index1 + 1, 0, max);
      if (index3 == index4)
        return this.ControlPoints[index3].Output;
      double t = (num - this.ControlPoints[index3].Input) / (this.ControlPoints[index4].Input - this.ControlPoints[index3].Input);
      return MathHelper.CubicInterp(this.ControlPoints[index2].Output, this.ControlPoints[index3].Output, this.ControlPoints[index4].Output, this.ControlPoints[index5].Output, t);
    }

    public double GetValue(double x, double y)
    {
      double num = this.Module.GetValue(x, y);
      int max = this.ControlPoints.Count - 1;
      int index1 = 0;
      while (index1 <= max && num >= this.ControlPoints[index1].Input)
        ++index1;
      int index2 = MathHelper.Clamp(index1 - 2, 0, max);
      int index3 = MathHelper.Clamp(index1 - 1, 0, max);
      int index4 = MathHelper.Clamp(index1, 0, max);
      int index5 = MathHelper.Clamp(index1 + 1, 0, max);
      if (index3 == index4)
        return this.ControlPoints[index3].Output;
      double t = (num - this.ControlPoints[index3].Input) / (this.ControlPoints[index4].Input - this.ControlPoints[index3].Input);
      return MathHelper.CubicInterp(this.ControlPoints[index2].Output, this.ControlPoints[index3].Output, this.ControlPoints[index4].Output, this.ControlPoints[index5].Output, t);
    }

    public double GetValue(double x, double y, double z)
    {
      double num = this.Module.GetValue(x, y, z);
      int max = this.ControlPoints.Count - 1;
      int index1 = 0;
      while (index1 <= max && num >= this.ControlPoints[index1].Input)
        ++index1;
      int index2 = MathHelper.Clamp(index1 - 2, 0, max);
      int index3 = MathHelper.Clamp(index1 - 1, 0, max);
      int index4 = MathHelper.Clamp(index1, 0, max);
      int index5 = MathHelper.Clamp(index1 + 1, 0, max);
      if (index3 == index4)
        return this.ControlPoints[index3].Output;
      double t = (num - this.ControlPoints[index3].Input) / (this.ControlPoints[index4].Input - this.ControlPoints[index3].Input);
      return MathHelper.CubicInterp(this.ControlPoints[index2].Output, this.ControlPoints[index3].Output, this.ControlPoints[index4].Output, this.ControlPoints[index5].Output, t);
    }
  }
}
