// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyDebugWorkTrackerExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Diagnostics;
using VRageMath;

namespace VRage.Utils
{
  public static class MyDebugWorkTrackerExtensions
  {
    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void Hit(this MyDebugWorkTracker<int> self) => ++self.Current;

    public static int Min(this MyDebugWorkTracker<int> self)
    {
      int num1 = int.MaxValue;
      int count = self.History.Count;
      for (int index = 0; index < count; ++index)
      {
        int num2 = self.History[index];
        if (num1 > num2)
          num1 = num2;
      }
      return num1;
    }

    public static int Max(this MyDebugWorkTracker<int> self)
    {
      int num1 = int.MinValue;
      int count = self.History.Count;
      for (int index = 0; index < count; ++index)
      {
        int num2 = self.History[index];
        if (num1 < num2)
          num1 = num2;
      }
      return num1;
    }

    public static int Average(this MyDebugWorkTracker<int> self)
    {
      long num = 0;
      int count = self.History.Count;
      if (count == 0)
        return 0;
      for (int index = 0; index < count; ++index)
        num += (long) self.History[index];
      return (int) (num / (long) count);
    }

    public static Vector4I Stats(this MyDebugWorkTracker<int> self)
    {
      if (self.History.Count == 0)
        return new Vector4I(0, 0, 0, 0);
      long num1 = 0;
      int num2 = int.MaxValue;
      int num3 = int.MinValue;
      int count = self.History.Count;
      for (int index = 0; index < count; ++index)
      {
        int num4 = self.History[index];
        if (num3 < num4)
          num3 = num4;
        if (num2 > num4)
          num2 = num4;
        num1 += (long) num4;
      }
      Vector4I vector4I;
      vector4I.X = self.History[count - 1];
      vector4I.Y = num2;
      vector4I.Z = (int) (num1 / (long) count);
      vector4I.W = num3;
      return vector4I;
    }
  }
}
