// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleLights
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleLights : MyActionBase
  {
    public override void ExecuteAction() => MySession.Static.ControlledEntity?.SwitchLights();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      label.State = MySession.Static.ControlledEntity != null ? (!MySession.Static.ControlledEntity.EnabledLights ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_On)) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnabledLights_None);
      return label;
    }

    public override int GetIconIndex() => MySession.Static.ControlledEntity.EnabledLights ? 1 : 0;
  }
}
