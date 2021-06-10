// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyExponent
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Modifiers
{
  public class MyExponent : IMyModule
  {
    public double Exponent { get; set; }

    public IMyModule Module { get; set; }

    public MyExponent(IMyModule module, double exponent = 2.0)
    {
      this.Exponent = exponent;
      this.Module = module;
    }

    public double GetValue(double x) => Math.Pow((this.Module.GetValue(x) + 1.0) * 0.5, this.Exponent) * 2.0 - 1.0;

    public double GetValue(double x, double y) => Math.Pow((this.Module.GetValue(x, y) + 1.0) * 0.5, this.Exponent) * 2.0 - 1.0;

    public double GetValue(double x, double y, double z) => Math.Pow((this.Module.GetValue(x, y, z) + 1.0) * 0.5, this.Exponent) * 2.0 - 1.0;
  }
}
