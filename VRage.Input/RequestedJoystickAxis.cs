// Decompiled with JetBrains decompiler
// Type: VRage.Input.RequestedJoystickAxis
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;

namespace VRage.Input
{
  [Flags]
  public enum RequestedJoystickAxis : byte
  {
    X = 1,
    Y = 2,
    Z = 4,
    NoZ = Y | X, // 0x03
    All = NoZ | Z, // 0x07
  }
}
