// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.VoxelOperatorFlags
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.ModAPI
{
  [Flags]
  public enum VoxelOperatorFlags
  {
    Read = 1,
    Write = 2,
    WriteAll = 6,
    None = 0,
    ReadWrite = Write | Read, // 0x00000003
    Default = ReadWrite, // 0x00000003
  }
}
