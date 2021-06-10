// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MyClamp
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Noise.Modifiers
{
  public class MyClamp : IMyModule
  {
    public double LowerBound { get; set; }

    public double UpperBound { get; set; }

    public IMyModule Module { get; set; }

    public MyClamp(IMyModule module, double lowerBound = -1.0, double upperBound = 1.0)
    {
      this.LowerBound = lowerBound;
      this.UpperBound = upperBound;
      this.Module = module;
    }

    public double GetValue(double x) => MathHelper.Clamp(this.Module.GetValue(x), this.LowerBound, this.UpperBound);

    public double GetValue(double x, double y) => MathHelper.Clamp(this.Module.GetValue(x, y), this.LowerBound, this.UpperBound);

    public double GetValue(double x, double y, double z) => MathHelper.Clamp(this.Module.GetValue(x, y, z), this.LowerBound, this.UpperBound);
  }
}
