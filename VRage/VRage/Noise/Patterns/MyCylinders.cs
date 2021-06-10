// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Patterns.MyCylinders
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Noise.Patterns
{
  internal class MyCylinders : IMyModule
  {
    public double Frequency { get; set; }

    public MyCylinders(double frequnecy = 1.0) => this.Frequency = frequnecy;

    public double GetValue(double x)
    {
      x *= this.Frequency;
      double n = Math.Sqrt(x * x + x * x);
      double val1 = n - (double) MathHelper.Floor(n);
      double val2 = 1.0 - val1;
      return 1.0 - Math.Min(val1, val2) * 4.0;
    }

    public double GetValue(double x, double z)
    {
      x *= this.Frequency;
      z *= this.Frequency;
      double n = Math.Sqrt(x * x + z * z);
      double val1 = n - (double) MathHelper.Floor(n);
      double val2 = 1.0 - val1;
      return 1.0 - Math.Min(val1, val2) * 4.0;
    }

    public double GetValue(double x, double y, double z) => throw new NotImplementedException();
  }
}
