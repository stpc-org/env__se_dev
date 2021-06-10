// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyWorkshopItemState
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.GameServices
{
  [Flags]
  public enum MyWorkshopItemState
  {
    None = 0,
    Subscribed = 1,
    LegacyItem = 2,
    Installed = 4,
    NeedsUpdate = 8,
    Downloading = 16, // 0x00000010
    DownloadPending = 32, // 0x00000020
  }
}
