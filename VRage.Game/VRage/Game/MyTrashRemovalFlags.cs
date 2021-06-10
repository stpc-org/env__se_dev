// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyTrashRemovalFlags
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game
{
  [Flags]
  public enum MyTrashRemovalFlags
  {
    None = 0,
    Default = 7706, // 0x00001E1A
    Fixed = 1,
    Stationary = 2,
    Linear = 8,
    Accelerating = 16, // 0x00000010
    Powered = 32, // 0x00000020
    Controlled = 64, // 0x00000040
    WithProduction = 128, // 0x00000080
    WithMedBay = 256, // 0x00000100
    WithBlockCount = 512, // 0x00000200
    DistanceFromPlayer = 1024, // 0x00000400
    RevertMaterials = 2048, // 0x00000800
    RevertAsteroids = 4096, // 0x00001000
    RevertWithFloatingsPresent = 8192, // 0x00002000
    Indestructible = 16384, // 0x00004000
    RevertBoulders = 32768, // 0x00008000
    RevertCloseToNPCGrids = 65536, // 0x00010000
  }
}
