// Decompiled with JetBrains decompiler
// Type: VRage.Noise.Patterns.MyCheckerBoard
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Noise.Patterns
{
  public class MyCheckerBoard : IMyModule
  {
    public double GetValue(double x) => (MathHelper.Floor(x) & 1) != 1 ? 1.0 : -1.0;

    public double GetValue(double x, double y) => (MathHelper.Floor(x) & 1 ^ MathHelper.Floor(y) & 1) != 1 ? 1.0 : -1.0;

    public double GetValue(double x, double y, double z)
    {
      int num1 = MathHelper.Floor(x) & 1;
      int num2 = MathHelper.Floor(y) & 1;
      int num3 = MathHelper.Floor(z) & 1;
      int num4 = num2;
      return (num1 ^ num4 ^ num3) != 1 ? 1.0 : -1.0;
    }
  }
}
