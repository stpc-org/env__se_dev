// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MySensorFilterFlags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities.Blocks
{
  [Flags]
  public enum MySensorFilterFlags : ushort
  {
    Players = 1,
    FloatingObjects = 2,
    SmallShips = 4,
    LargeShips = 8,
    Stations = 16, // 0x0010
    Asteroids = 32, // 0x0020
    Subgrids = 64, // 0x0040
    Owner = 256, // 0x0100
    Friendly = 512, // 0x0200
    Neutral = 1024, // 0x0400
    Enemy = 2048, // 0x0800
  }
}
