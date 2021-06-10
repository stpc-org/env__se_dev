// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.UrlOpenMode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Graphics.GUI
{
  [Flags]
  public enum UrlOpenMode
  {
    SteamOverlay = 1,
    ExternalBrowser = 2,
    ConfirmExternal = 4,
    ExternalWithConfirm = ConfirmExternal | ExternalBrowser, // 0x00000006
    SteamOrExternalWithConfirm = ExternalWithConfirm | SteamOverlay, // 0x00000007
  }
}
