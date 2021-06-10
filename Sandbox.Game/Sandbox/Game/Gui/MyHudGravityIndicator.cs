// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudGravityIndicator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Gui
{
  public class MyHudGravityIndicator
  {
    internal MyEntity Entity;

    public bool Visible { get; private set; }

    public void Show(Action<MyHudGravityIndicator> propertiesInit)
    {
      this.Visible = true;
      if (propertiesInit == null)
        return;
      propertiesInit(this);
    }

    public void Clean() => this.Entity = (MyEntity) null;

    public void Hide() => this.Visible = false;
  }
}
