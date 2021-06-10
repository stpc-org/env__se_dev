// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalAction
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;

namespace Sandbox.ModAPI.Interfaces.Terminal
{
  public interface IMyTerminalAction : ITerminalAction
  {
    Func<IMyTerminalBlock, bool> Enabled { get; set; }

    List<MyToolbarType> InvalidToolbarTypes { get; set; }

    bool ValidForGroups { get; set; }

    new StringBuilder Name { get; set; }

    new string Icon { get; set; }

    System.Action<IMyTerminalBlock> Action { get; set; }

    System.Action<IMyTerminalBlock, StringBuilder> Writer { get; set; }
  }
}
