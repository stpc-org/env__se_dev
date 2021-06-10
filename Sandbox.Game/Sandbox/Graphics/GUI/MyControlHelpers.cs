// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyControlHelpers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using System.Text;

namespace Sandbox.Graphics.GUI
{
  internal static class MyControlHelpers
  {
    public static void SetDetailedInfo<TBlock>(
      this MyGuiControlBlockProperty control,
      MyTerminalControl<TBlock>.WriterDelegate writer,
      TBlock block)
      where TBlock : MyTerminalBlock
    {
      StringBuilder textToDraw = control.ExtraInfoLabel.TextToDraw;
      textToDraw.Clear();
      if (writer != null && (object) block != null)
        writer(block, textToDraw);
      control.ExtraInfoLabel.TextToDraw = textToDraw;
      control.ExtraInfoLabel.Visible = textToDraw.Length > 0;
    }

    public static void SetDetailedInfo<TBlock>(
      this MyGuiControlBlockProperty control,
      MyTerminalControl<TBlock>.AdvancedWriterDelegate writer,
      TBlock block)
      where TBlock : MyTerminalBlock
    {
      StringBuilder textToDraw = control.ExtraInfoLabel.TextToDraw;
      textToDraw.Clear();
      if (writer != null && (object) block != null)
        writer(block, control, textToDraw);
      control.ExtraInfoLabel.TextToDraw = textToDraw;
      control.ExtraInfoLabel.Visible = textToDraw.Length > 0;
    }
  }
}
