// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyParallelUpdateFlags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities
{
  [Flags]
  public enum MyParallelUpdateFlags
  {
    NONE = 0,
    EACH_FRAME = 1,
    EACH_10TH_FRAME = 2,
    EACH_100TH_FRAME = 4,
    BEFORE_NEXT_FRAME = 8,
    SIMULATE = 16, // 0x00000010
    EACH_FRAME_PARALLEL = 32, // 0x00000020
  }
}
