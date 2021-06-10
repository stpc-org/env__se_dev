// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MySearchConditionFlags
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.GameServices
{
  [Flags]
  public enum MySearchConditionFlags : byte
  {
    Equal = 1,
    Contains = 2,
    GreaterOrEqual = 4,
    LesserOrEqual = 8,
  }
}
