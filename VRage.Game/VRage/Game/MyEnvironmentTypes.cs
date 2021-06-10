// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyEnvironmentTypes
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game
{
  [Flags]
  public enum MyEnvironmentTypes
  {
    None = 0,
    Space = 1,
    PlanetWithAtmosphere = 2,
    PlanetWithoutAtmosphere = 4,
    All = PlanetWithoutAtmosphere | PlanetWithAtmosphere | Space, // 0x00000007
  }
}
