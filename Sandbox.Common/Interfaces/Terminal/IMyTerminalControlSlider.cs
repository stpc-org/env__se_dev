// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControlSlider
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Text;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControlSlider : IMyTerminalControl, IMyTerminalValueControl<float>, ITerminalProperty, IMyTerminalControlTitleTooltip
  {
    void SetLimits(float min, float max);

    void SetLogLimits(float min, float max);

    void SetDualLogLimits(float absMin, float absMax, float centerBand);

    void SetLimits(Func<IMyTerminalBlock, float> minGetter, Func<IMyTerminalBlock, float> maxGetter);

    void SetLogLimits(
      Func<IMyTerminalBlock, float> minGetter,
      Func<IMyTerminalBlock, float> maxGetter);

    void SetDualLogLimits(
      Func<IMyTerminalBlock, float> minGetter,
      Func<IMyTerminalBlock, float> maxGetter,
      float centerBand);

    Action<IMyTerminalBlock, StringBuilder> Writer { get; set; }
  }
}
