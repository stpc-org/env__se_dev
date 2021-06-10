// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MySymmetrySettingModeEnum
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities.Cube
{
  [Flags]
  public enum MySymmetrySettingModeEnum
  {
    Disabled = 0,
    NoPlane = 1,
    XPlane = 2,
    XPlaneOdd = 4,
    YPlane = 8,
    YPlaneOdd = 16, // 0x00000010
    ZPlane = 32, // 0x00000020
    ZPlaneOdd = 64, // 0x00000040
  }
}
