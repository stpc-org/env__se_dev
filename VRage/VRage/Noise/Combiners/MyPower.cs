// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Combiners.MyPower
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Combiners
{
  public class MyPower : IMyModule
  {
    private double powerOffset;

    public IMyModule Base { get; set; }

    public IMyModule Power { get; set; }

    public MyPower(IMyModule baseModule, IMyModule powerModule, double powerOffset = 0.0)
    {
      this.Base = baseModule;
      this.Power = powerModule;
      this.powerOffset = powerOffset;
    }

    public double GetValue(double x) => Math.Pow(this.Base.GetValue(x), this.powerOffset + this.Power.GetValue(x));

    public double GetValue(double x, double y) => Math.Pow(this.Base.GetValue(x, y), this.powerOffset + this.Power.GetValue(x, y));

    public double GetValue(double x, double y, double z) => Math.Pow(this.Base.GetValue(x, y, z), this.powerOffset + this.Power.GetValue(x, y, z));
  }
}
