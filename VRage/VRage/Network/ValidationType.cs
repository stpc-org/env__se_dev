// Decompiled with JetBrains decompiler
// Type: VRage.Network.ValidationType
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Network
{
  [Flags]
  public enum ValidationType
  {
    None = 0,
    Access = 1,
    Controlled = 2,
    Ownership = 4,
    BigOwner = 8,
    BigOwnerSpaceMaster = 16, // 0x00000010
    IgnoreDLC = 32, // 0x00000020
  }
}
