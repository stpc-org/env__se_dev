// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyUpdateOrder
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Components
{
  [Flags]
  public enum MyUpdateOrder
  {
    BeforeSimulation = 1,
    Simulation = 2,
    AfterSimulation = 4,
    NoUpdate = 0,
  }
}
