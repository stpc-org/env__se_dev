// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionSwitchCamera
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionSwitchCamera : MyActionBase
  {
    public override void ExecuteAction() => MyGuiScreenGamePlay.Static.SwitchCamera();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      label.State = MySession.Static.CameraController != null ? (!MySession.Static.CameraController.IsInFirstPersonView ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_ThirdPerson) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_FirstPerson)) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_SwitchCamera_None);
      return label;
    }
  }
}
