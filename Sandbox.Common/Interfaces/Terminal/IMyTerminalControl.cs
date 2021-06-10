// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControl
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControl
  {
    string Id { get; }

    Func<IMyTerminalBlock, bool> Enabled { get; set; }

    Func<IMyTerminalBlock, bool> Visible { get; set; }

    bool SupportsMultipleBlocks { get; set; }

    void RedrawControl();

    void UpdateVisual();
  }
}
