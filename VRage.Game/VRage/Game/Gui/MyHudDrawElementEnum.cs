// Decompiled with JetBrains decompiler
// Type: VRage.Game.Gui.MyHudDrawElementEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Gui
{
  [Flags]
  internal enum MyHudDrawElementEnum
  {
    NONE = 0,
    DIRECTION_INDICATORS = 1,
    CROSSHAIR = 2,
    DAMAGE_INDICATORS = 4,
    AMMO = 8,
    HARVEST_MATERIAL = 16, // 0x00000010
    BARGRAPHS_PLAYER_SHIP = 64, // 0x00000040
    BARGRAPHS_LARGE_WEAPON = 128, // 0x00000080
    DIALOGUES = 256, // 0x00000100
    MISSION_OBJECTIVES = 512, // 0x00000200
    BACK_CAMERA = 1024, // 0x00000400
    WHEEL_CONTROL = 2048, // 0x00000800
    CROSSHAIR_DYNAMIC = 4096, // 0x00001000
  }
}
