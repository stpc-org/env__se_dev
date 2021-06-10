// Decompiled with JetBrains decompiler
// Type: VRage.Stats.MyStatTypeEnum
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Stats
{
  public enum MyStatTypeEnum : byte
  {
    Unset = 0,
    CurrentValue = 1,
    Min = 2,
    Max = 3,
    Avg = 4,
    MinMax = 5,
    MinMaxAvg = 6,
    Sum = 7,
    Counter = 8,
    CounterSum = 9,
    DontDisappearFlag = 16, // 0x10
    KeepInactiveLongerFlag = 32, // 0x20
    LongFlag = 64, // 0x40
    FormatFlag = 128, // 0x80
    AllFlags = 240, // 0xF0
  }
}
