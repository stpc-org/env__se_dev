// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControlCombobox
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage.ModAPI;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControlCombobox : IMyTerminalControl, IMyTerminalValueControl<long>, ITerminalProperty, IMyTerminalControlTitleTooltip
  {
    Action<List<MyTerminalControlComboBoxItem>> ComboBoxContent { get; set; }
  }
}
