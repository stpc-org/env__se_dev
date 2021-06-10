// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Combiners.MyMax
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Combiners
{
  public class MyMax : IMyModule
  {
    public IMyModule Source1 { get; set; }

    public IMyModule Source2 { get; set; }

    public MyMax(IMyModule sourceModule1, IMyModule sourceModule2)
    {
      this.Source1 = sourceModule1;
      this.Source2 = sourceModule2;
    }

    public double GetValue(double x) => Math.Max(this.Source1.GetValue(x), this.Source2.GetValue(x));

    public double GetValue(double x, double y) => Math.Max(this.Source1.GetValue(x, y), this.Source2.GetValue(x, y));

    public double GetValue(double x, double y, double z) => Math.Max(this.Source1.GetValue(x, y, z), this.Source2.GetValue(x, y, z));
  }
}
