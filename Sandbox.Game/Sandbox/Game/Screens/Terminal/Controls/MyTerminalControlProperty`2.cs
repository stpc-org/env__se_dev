// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Terminal.Controls.MyTerminalControlProperty`2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;

namespace Sandbox.Game.Screens.Terminal.Controls
{
  public class MyTerminalControlProperty<TBlock, TValue> : MyTerminalValueControl<TBlock, TValue>, IMyTerminalControlProperty<TValue>, IMyTerminalControl, IMyTerminalValueControl<TValue>, ITerminalProperty
    where TBlock : MyTerminalBlock
  {
    public MyTerminalControlProperty(string id)
      : base(id)
      => this.Visible = (Func<TBlock, bool>) (x => false);

    public override TValue GetDefaultValue(TBlock block) => default (TValue);

    public override TValue GetMaximum(TBlock block) => this.GetDefaultValue(block);

    public override TValue GetMinimum(TBlock block) => this.GetDefaultValue(block);

    protected override MyGuiControlBase CreateGui()
    {
      this.Visible = (Func<TBlock, bool>) (x => false);
      return (MyGuiControlBase) null;
    }
  }
}
