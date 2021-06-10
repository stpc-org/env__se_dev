// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyPrimitiveFlags
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Serialization
{
  public enum MyPrimitiveFlags
  {
    None = 0,
    Signed = 1,
    Normalized = 2,
    Variant = 4,
    VariantSigned = 5,
    Ascii = 8,
    Utf8 = 16, // 0x00000010
    FixedPoint8 = 32, // 0x00000020
    FixedPoint16 = 64, // 0x00000040
  }
}
