// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyTurretTargetFlags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Weapons
{
  [Flags]
  public enum MyTurretTargetFlags : ushort
  {
    Players = 1,
    SmallShips = 2,
    LargeShips = 4,
    Stations = 8,
    Asteroids = 16, // 0x0010
    Missiles = 32, // 0x0020
    Moving = 64, // 0x0040
    NotNeutrals = 128, // 0x0080
  }
}
