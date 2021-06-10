// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControlOnOffSwitch
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Utils;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControlOnOffSwitch : IMyTerminalControl, IMyTerminalValueControl<bool>, ITerminalProperty, IMyTerminalControlTitleTooltip
  {
    MyStringId OnText { get; set; }

    MyStringId OffText { get; set; }
  }
}
