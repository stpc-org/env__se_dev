// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudVoiceChat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Gui
{
  public class MyHudVoiceChat
  {
    public bool Visible { get; private set; }

    public event Action<bool> VisibilityChanged;

    public void Show()
    {
      this.Visible = true;
      if (this.VisibilityChanged == null)
        return;
      this.VisibilityChanged(true);
    }

    public void Hide()
    {
      this.Visible = false;
      if (this.VisibilityChanged == null)
        return;
      this.VisibilityChanged(false);
    }
  }
}
