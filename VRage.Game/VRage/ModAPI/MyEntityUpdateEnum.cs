// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.MyEntityUpdateEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.ModAPI
{
  [Flags]
  public enum MyEntityUpdateEnum
  {
    NONE = 0,
    EACH_FRAME = 1,
    EACH_10TH_FRAME = 2,
    EACH_100TH_FRAME = 4,
    BEFORE_NEXT_FRAME = 8,
    SIMULATE = 16, // 0x00000010
  }
}
