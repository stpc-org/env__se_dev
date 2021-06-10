// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyStorageDataTypeFlags
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Voxels
{
  [Flags]
  public enum MyStorageDataTypeFlags : byte
  {
    None = 0,
    Content = 1,
    Material = 2,
    ContentAndMaterial = Material | Content, // 0x03
    All = ContentAndMaterial, // 0x03
  }
}
