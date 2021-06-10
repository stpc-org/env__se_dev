// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyObjectFlags
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Serialization
{
  [Flags]
  public enum MyObjectFlags
  {
    None = 0,
    DefaultZero = 1,
    Nullable = DefaultZero, // 0x00000001
    Dynamic = 2,
    DefaultValueOrEmpty = 4,
    DynamicDefault = 8,
  }
}
