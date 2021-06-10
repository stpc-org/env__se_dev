// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Combiners.MyAdd
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Noise.Combiners
{
  public class MyAdd : IMyModule
  {
    public IMyModule Source1 { get; set; }

    public IMyModule Source2 { get; set; }

    public MyAdd(IMyModule sourceModule1, IMyModule sourceModule2)
    {
      this.Source1 = sourceModule1;
      this.Source2 = sourceModule2;
    }

    public double GetValue(double x) => (this.Source1.GetValue(x) + this.Source2.GetValue(x)) * 0.5;

    public double GetValue(double x, double y) => (this.Source1.GetValue(x, y) + this.Source2.GetValue(x, y)) * 0.5;

    public double GetValue(double x, double y, double z) => (this.Source1.GetValue(x, y, z) + this.Source2.GetValue(x, y, z)) * 0.5;
  }
}
