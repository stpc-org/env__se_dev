// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyVoxelRequestFlags
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Voxels
{
  [Flags]
  public enum MyVoxelRequestFlags
  {
    SurfaceMaterial = 1,
    ConsiderContent = 2,
    ForPhysics = 4,
    EmptyData = 8,
    FullContent = 16, // 0x00000010
    OneMaterial = 32, // 0x00000020
    AdviseCache = 64, // 0x00000040
    ContentChecked = 128, // 0x00000080
    ContentCheckedDeep = 256, // 0x00000100
    UseNativeProvider = 512, // 0x00000200
    Postprocess = 1024, // 0x00000400
    DoNotCheck = 65536, // 0x00010000
    PreciseOrePositions = 131072, // 0x00020000
    RequestFlags = ConsiderContent | SurfaceMaterial, // 0x00000003
  }
}
