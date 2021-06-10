// Decompiled with JetBrains decompiler
// Type: VRage.Network.ValidationResult
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Network
{
  [Flags]
  public enum ValidationResult
  {
    Passed = 0,
    Kick = 1,
    Access = 2,
    Controlled = 4,
    Ownership = 8,
    BigOwner = 16, // 0x00000010
    BigOwnerSpaceMaster = 32, // 0x00000020
  }
}
