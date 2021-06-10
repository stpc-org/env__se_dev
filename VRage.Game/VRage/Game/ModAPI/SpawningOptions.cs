// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.SpawningOptions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI
{
  [Flags]
  public enum SpawningOptions
  {
    None = 0,
    RotateFirstCockpitTowardsDirection = 2,
    SpawnRandomCargo = 4,
    DisableDampeners = 8,
    SetNeutralOwner = 16, // 0x00000010
    TurnOffReactors = 32, // 0x00000020
    DisableSave = 64, // 0x00000040
    UseGridOrigin = 128, // 0x00000080
    SetAuthorship = 256, // 0x00000100
    ReplaceColor = 512, // 0x00000200
    UseOnlyWorldMatrix = 1024, // 0x00000400
  }
}
