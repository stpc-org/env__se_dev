// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControlLabel
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Utils;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalControlLabel : IMyTerminalControl
  {
    MyStringId Label { get; set; }
  }
}
