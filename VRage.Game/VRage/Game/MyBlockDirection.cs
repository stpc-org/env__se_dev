﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBlockDirection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game
{
  [Flags]
  public enum MyBlockDirection
  {
    Horizontal = 1,
    Vertical = 2,
    Both = Vertical | Horizontal, // 0x00000003
  }
}
