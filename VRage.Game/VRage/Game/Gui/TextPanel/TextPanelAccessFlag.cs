// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.TextPanelAccessFlag
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.GUI.TextPanel
{
  [Flags]
  public enum TextPanelAccessFlag : byte
  {
    NONE = 0,
    READ_FACTION = 2,
    WRITE_FACTION = 4,
    READ_AND_WRITE_FACTION = WRITE_FACTION | READ_FACTION, // 0x06
    READ_ENEMY = 8,
    WRITE_ENEMY = 16, // 0x10
    READ_ALL = READ_ENEMY | READ_FACTION, // 0x0A
    WRITE_ALL = WRITE_ENEMY | WRITE_FACTION, // 0x14
    READ_AND_WRITE_ALL = WRITE_ALL | READ_ALL, // 0x1E
  }
}
