// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalValueControl`1
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalValueControl<TValue> : ITerminalProperty
  {
    Func<IMyTerminalBlock, TValue> Getter { get; set; }

    Action<IMyTerminalBlock, TValue> Setter { get; set; }
  }
}
