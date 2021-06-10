// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.UpdateType
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Ingame
{
  [Flags]
  public enum UpdateType
  {
    None = 0,
    Terminal = 1,
    Trigger = 2,
    Mod = 8,
    Script = 16, // 0x00000010
    Update1 = 32, // 0x00000020
    Update10 = 64, // 0x00000040
    Update100 = 128, // 0x00000080
    Once = 256, // 0x00000100
    IGC = 512, // 0x00000200
  }
}
