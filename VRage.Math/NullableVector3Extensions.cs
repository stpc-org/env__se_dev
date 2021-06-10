// Decompiled with JetBrains decompiler
// Type: VRageMath.NullableVector3Extensions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Diagnostics;

namespace VRageMath
{
  public static class NullableVector3Extensions
  {
    public static bool IsValid(this Vector3? value) => !value.HasValue || value.Value.IsValid();

    [Conditional("DEBUG")]
    public static void AssertIsValid(this Vector3? value)
    {
    }
  }
}
