// Decompiled with JetBrains decompiler
// Type: VRage.Game.Gui.MyHudIndicatorFlagsEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Gui
{
  [Flags]
  public enum MyHudIndicatorFlagsEnum
  {
    NONE = 0,
    SHOW_TEXT = 1,
    SHOW_BORDER_INDICATORS = 2,
    SHOW_DISTANCE = 16, // 0x00000010
    ALPHA_CORRECTION_BY_DISTANCE = 32, // 0x00000020
    SHOW_ICON = 1024, // 0x00000400
    SHOW_FOCUS_MARK = 2048, // 0x00000800
    SHOW_ALL = -1, // 0xFFFFFFFF
  }
}
