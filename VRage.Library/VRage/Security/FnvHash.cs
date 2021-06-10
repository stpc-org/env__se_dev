// Decompiled with JetBrains decompiler
// Type: VRage.Security.FnvHash
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Security
{
  public static class FnvHash
  {
    private const uint InitialFNV = 2166136261;
    private const uint FNVMultiple = 16777619;

    public static uint Compute(string s)
    {
      uint num = 2166136261;
      for (int index = 0; index < s.Length; ++index)
        num = (num ^ (uint) s[index]) * 16777619U;
      return num;
    }

    public static uint ComputeAscii(string s)
    {
      uint num = 2166136261;
      for (int index = 0; index < s.Length; ++index)
        num = (num ^ (uint) (byte) s[index]) * 16777619U;
      return num;
    }

    public static uint Compute(Span<byte> data)
    {
      uint num = 2166136261;
      for (int index = 0; index < data.Length; ++index)
        num = (num ^ (uint) data[index]) * 16777619U;
      return num;
    }
  }
}
