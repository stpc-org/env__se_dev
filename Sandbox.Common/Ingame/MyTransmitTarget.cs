// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyTransmitTarget
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Ingame
{
  [Flags]
  public enum MyTransmitTarget
  {
    None = 0,
    Owned = 1,
    Ally = 2,
    Neutral = 4,
    Enemy = 8,
    Everyone = Enemy | Neutral | Ally | Owned, // 0x0000000F
    Default = Ally | Owned, // 0x00000003
  }
}
