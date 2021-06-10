// Decompiled with JetBrains decompiler
// Type: VRage.Network.CallSiteFlags
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Network
{
  [Flags]
  public enum CallSiteFlags
  {
    None = 0,
    Client = 1,
    Server = 2,
    Broadcast = 4,
    Reliable = 8,
    RefreshReplicable = 16, // 0x00000010
    BroadcastExcept = 32, // 0x00000020
    Blocking = 64, // 0x00000040
    ServerInvoked = 128, // 0x00000080
    DistanceRadius = 256, // 0x00000100
  }
}
