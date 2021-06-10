// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleHandbrake
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleHandbrake : MyActionBase
  {
    public override void ExecuteAction() => MySession.Static.ControlledEntity?.SwitchHandbrake();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (!(MySession.Static.ControlledEntity is MyShipController controlledEntity))
        return label;
      MyGridWheelSystem wheelSystem = controlledEntity?.CubeGrid?.GridSystems?.WheelSystem;
      if (wheelSystem == null)
        return label;
      label.State = !wheelSystem.HandBrake ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleHandbrake_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleHandbrake_On);
      return label;
    }
  }
}
