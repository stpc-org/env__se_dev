// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.UpdateFrequency
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Ingame
{
  [Flags]
  public enum UpdateFrequency : byte
  {
    None = 0,
    Update1 = 1,
    Update10 = 2,
    Update100 = 4,
    Once = 8,
  }
}
