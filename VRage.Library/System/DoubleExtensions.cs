// Decompiled with JetBrains decompiler
// Type: System.DoubleExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;

namespace System
{
  public static class DoubleExtensions
  {
    public static bool IsValid(this double f) => !double.IsNaN(f) && !double.IsInfinity(f);

    [Conditional("DEBUG")]
    public static void AssertIsValid(this double f, string message)
    {
    }
  }
}
