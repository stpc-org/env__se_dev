// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MySupportKeysEnum
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Screens.Helpers
{
  [Flags]
  public enum MySupportKeysEnum : byte
  {
    NONE = 0,
    CTRL = 1,
    ALT = 2,
    SHIFT = 4,
    CTRL_ALT = ALT | CTRL, // 0x03
    CTRL_SHIFT = SHIFT | CTRL, // 0x05
    ALT_SHIFT = SHIFT | ALT, // 0x06
    CTRL_ALT_SHIFT = ALT_SHIFT | CTRL, // 0x07
  }
}
