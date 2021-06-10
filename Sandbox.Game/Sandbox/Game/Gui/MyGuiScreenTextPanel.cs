// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenTextPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenTextPanel : MyGuiScreenText
  {
    public MyGuiScreenTextPanel(
      string missionTitle = null,
      string currentObjectivePrefix = null,
      string currentObjective = null,
      string description = null,
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = null,
      Action saveCodeCallback = null,
      string okButtonCaption = null,
      bool editable = false,
      MyGuiScreenBase previousScreen = null)
      : base(missionTitle, currentObjectivePrefix, currentObjective, description, resultCallback, okButtonCaption, editEnabled: editable)
    {
      this.CanHideOthers = editable;
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }
  }
}
