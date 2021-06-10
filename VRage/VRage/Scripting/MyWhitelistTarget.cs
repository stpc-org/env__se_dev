// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.MyWhitelistTarget
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Scripting
{
  [Flags]
  public enum MyWhitelistTarget
  {
    None = 0,
    ModApi = 1,
    Ingame = 2,
    Both = Ingame | ModApi, // 0x00000003
  }
}
