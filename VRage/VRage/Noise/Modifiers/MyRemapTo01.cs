// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyRemapTo01
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Modifiers
{
  public class MyRemapTo01 : IMyModule
  {
    public IMyModule Module { get; set; }

    public MyRemapTo01(IMyModule module) => this.Module = module;

    public double GetValue(double x) => (this.Module.GetValue(x) + 1.0) * 0.5;

    public double GetValue(double x, double y) => (this.Module.GetValue(x, y) + 1.0) * 0.5;

    public double GetValue(double x, double y, double z) => (this.Module.GetValue(x, y, z) + 1.0) * 0.5;
  }
}
