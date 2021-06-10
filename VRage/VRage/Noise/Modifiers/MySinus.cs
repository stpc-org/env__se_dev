// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Modifiers.MySinus
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Noise.Modifiers
{
  public class MySinus : IMyModule
  {
    private IMyModule module;

    public MySinus(IMyModule module) => this.module = module;

    public double GetValue(double x) => Math.Sin(this.module.GetValue(x) * Math.PI);

    public double GetValue(double x, double y) => Math.Sin(this.module.GetValue(x, y) * Math.PI);

    public double GetValue(double x, double y, double z) => Math.Sin(this.module.GetValue(x, y, z) * Math.PI);
  }
}
