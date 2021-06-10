// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
  internal abstract class MyTerminalController
  {
    protected bool m_dirtyDraw;

    public virtual void InvalidateBeforeDraw() => this.m_dirtyDraw = true;

    public virtual void UpdateBeforeDraw(MyGuiScreenBase screen)
    {
    }

    public virtual void HandleInput()
    {
    }
  }
}
