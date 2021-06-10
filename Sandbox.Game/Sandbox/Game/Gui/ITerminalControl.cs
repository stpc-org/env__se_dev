// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.ITerminalControl
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
  public interface ITerminalControl
  {
    string Id { get; }

    bool SupportsMultipleBlocks { get; }

    MyGuiControlBase GetGuiControl();

    MyTerminalBlock[] TargetBlocks { get; set; }

    void UpdateVisual();

    bool IsVisible(MyTerminalBlock block);

    ITerminalAction[] Actions { get; }
  }
}
