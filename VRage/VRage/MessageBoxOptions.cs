// Decompiled with JetBrains decompiler
// Type: VRage.MessageBoxOptions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage
{
  [Flags]
  public enum MessageBoxOptions
  {
    OkOnly = 0,
    OkCancel = 1,
    AbortRetryIgnore = 2,
    YesNoCancel = AbortRetryIgnore | OkCancel, // 0x00000003
    YesNo = 4,
    RetryCancel = YesNo | OkCancel, // 0x00000005
    CancelTryContinue = YesNo | AbortRetryIgnore, // 0x00000006
    IconHand = 16, // 0x00000010
    IconQuestion = 32, // 0x00000020
    IconExclamation = IconQuestion | IconHand, // 0x00000030
    IconAsterisk = 64, // 0x00000040
  }
}
