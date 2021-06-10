// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.UseObject.UseActionEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Entity.UseObject
{
  [Flags]
  public enum UseActionEnum
  {
    None = 0,
    Manipulate = 1,
    OpenTerminal = 2,
    OpenInventory = 4,
    UseFinished = 8,
    Close = 16, // 0x00000010
    PickUp = 32, // 0x00000020
    BuildPlanner = 64, // 0x00000040
  }
}
