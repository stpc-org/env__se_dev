// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.AdminSettingsEnum
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.World
{
  [Flags]
  public enum AdminSettingsEnum
  {
    None = 0,
    Invulnerable = 1,
    ShowPlayers = 2,
    UseTerminals = 4,
    Untargetable = 8,
    KeepOriginalOwnershipOnPaste = 16, // 0x00000010
    IgnoreSafeZones = 32, // 0x00000020
    IgnorePcu = 64, // 0x00000040
    AdminOnly = IgnorePcu | IgnoreSafeZones | Untargetable | UseTerminals | Invulnerable, // 0x0000006D
  }
}
