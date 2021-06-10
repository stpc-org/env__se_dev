// Decompiled with JetBrains decompiler
// Type: System.FloatExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;

namespace System
{
  public static class FloatExtensions
  {
    public static bool IsValid(this float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    [Conditional("DEBUG")]
    public static void AssertIsValid(this float f)
    {
    }

    public static bool IsEqual(this float f, float other, float epsilon = 0.0001f) => (double) f == (double) other || (f - other).IsZero(epsilon);

    public static bool IsZero(this float f, float epsilon = 0.0001f) => (double) Math.Abs(f) < (double) epsilon;

    public static bool IsInt(this float f, float epsilon = 0.0001f) => (double) Math.Abs(f % 1f) <= (double) epsilon;
  }
}
