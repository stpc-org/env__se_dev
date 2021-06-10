// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Patterns.MyConstant
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Patterns
{
  public class MyConstant : IMyModule
  {
    public double Constant { get; set; }

    public MyConstant(double constant) => this.Constant = constant;

    public double GetValue(double x) => this.Constant;

    public double GetValue(double x, double y) => this.Constant;

    public double GetValue(double x, double y, double z) => this.Constant;
  }
}
