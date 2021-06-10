// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyCharacterMovementFlags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities.Character
{
  [Flags]
  public enum MyCharacterMovementFlags : byte
  {
    Jump = 1,
    Sprint = 2,
    FlyUp = 4,
    FlyDown = 8,
    Crouch = 16, // 0x10
    Walk = 32, // 0x20
  }
}
