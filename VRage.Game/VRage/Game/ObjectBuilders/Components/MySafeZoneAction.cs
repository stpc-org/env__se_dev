// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MySafeZoneAction
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ObjectBuilders.Components
{
  [Flags]
  public enum MySafeZoneAction
  {
    Damage = 1,
    Shooting = 2,
    Drilling = 4,
    Welding = 8,
    Grinding = 16, // 0x00000010
    VoxelHand = 32, // 0x00000020
    Building = 64, // 0x00000040
    LandingGearLock = 128, // 0x00000080
    ConvertToStation = 256, // 0x00000100
    BuildingProjections = 512, // 0x00000200
    All = BuildingProjections | ConvertToStation | LandingGearLock | Building | VoxelHand | Grinding | Welding | Drilling | Shooting | Damage, // 0x000003FF
    AdminIgnore = BuildingProjections | ConvertToStation | Building | VoxelHand | Grinding | Welding | Drilling | Shooting, // 0x0000037E
  }
}
