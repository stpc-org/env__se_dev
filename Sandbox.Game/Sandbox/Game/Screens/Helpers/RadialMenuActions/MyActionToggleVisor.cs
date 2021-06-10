﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleVisor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleVisor : MyActionBase
  {
    public override void ExecuteAction() => MySession.Static.ControlledEntity?.SwitchHelmet();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      label.State = !MySession.Static.ControlledEntity.EnabledHelmet ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_Visor_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_Visor_On);
      return label;
    }

    public override int GetIconIndex() => MySession.Static.ControlledEntity.EnabledHelmet ? 1 : 0;
  }
}
