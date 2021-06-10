// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.EntityFlags
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.ModAPI
{
  [Flags]
  public enum EntityFlags
  {
    None = 1,
    Visible = 2,
    Save = 8,
    Near = 16, // 0x00000010
    NeedsUpdate = 32, // 0x00000020
    NeedsResolveCastShadow = 64, // 0x00000040
    FastCastShadowResolve = 128, // 0x00000080
    SkipIfTooSmall = 256, // 0x00000100
    NeedsUpdate10 = 512, // 0x00000200
    NeedsUpdate100 = 1024, // 0x00000400
    NeedsDraw = 2048, // 0x00000800
    InvalidateOnMove = 4096, // 0x00001000
    Sync = 8192, // 0x00002000
    NeedsDrawFromParent = 16384, // 0x00004000
    ShadowBoxLod = 32768, // 0x00008000
    Transparent = 65536, // 0x00010000
    NeedsUpdateBeforeNextFrame = 131072, // 0x00020000
    DrawOutsideViewDistance = 262144, // 0x00040000
    IsGamePrunningStructureObject = 524288, // 0x00080000
    NeedsWorldMatrix = 1048576, // 0x00100000
    IsNotGamePrunningStructureObject = 2097152, // 0x00200000
    NeedsSimulate = 4194304, // 0x00400000
    UpdateRender = 8388608, // 0x00800000
    Default = UpdateRender | NeedsWorldMatrix | InvalidateOnMove | SkipIfTooSmall | NeedsResolveCastShadow | Save | Visible, // 0x0090114A
  }
}
