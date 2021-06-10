// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Terminal.Controls.MyTerminalControlLabel`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens.Terminal.Controls
{
  public class MyTerminalControlLabel<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlLabel, IMyTerminalControl
    where TBlock : MyTerminalBlock
  {
    public MyStringId Label;
    private MyGuiControlLabel m_label;

    public MyTerminalControlLabel(MyStringId label)
      : base(nameof (Label))
      => this.Label = label;

    protected override MyGuiControlBase CreateGui()
    {
      this.m_label = new MyGuiControlLabel();
      return (MyGuiControlBase) new MyGuiControlBlockProperty(MyTexts.GetString(this.Label), (string) null, (MyGuiControlBase) this.m_label, MyGuiControlBlockPropertyLayoutEnum.Horizontal);
    }

    MyStringId IMyTerminalControlLabel.Label
    {
      get => this.Label;
      set => this.Label = value;
    }
  }
}
