// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControlListbox
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage.ModAPI;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControlListbox : IMyTerminalControl, IMyTerminalControlTitleTooltip
  {
    bool Multiselect { get; set; }

    int VisibleRowsCount { get; set; }

    Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> ListContent { set; }

    Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> ItemSelected { set; }
  }
}
