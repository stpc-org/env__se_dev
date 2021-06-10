// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyInvert
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Modifiers
{
  public class MyInvert : IMyModule
  {
    public IMyModule Module { get; set; }

    public MyInvert(IMyModule module) => this.Module = module;

    public double GetValue(double x) => -this.Module.GetValue(x);

    public double GetValue(double x, double y) => -this.Module.GetValue(x, y);

    public double GetValue(double x, double y, double z) => -this.Module.GetValue(x, y, z);
  }
}
