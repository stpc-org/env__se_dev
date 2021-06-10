// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlSeparator`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlSeparator<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlSeparator, IMyTerminalControl
    where TBlock : MyTerminalBlock
  {
    public MyTerminalControlSeparator()
      : base("Separator")
    {
    }

    protected override MyGuiControlBase CreateGui()
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.Size = new Vector2(0.485f, 0.01f);
      controlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlSeparatorList.AddHorizontal(Vector2.Zero, 0.225f);
      return (MyGuiControlBase) controlSeparatorList;
    }

    string IMyTerminalControl.Id => "";
  }
}
