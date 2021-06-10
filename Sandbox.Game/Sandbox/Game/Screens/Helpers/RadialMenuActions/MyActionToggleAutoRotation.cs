// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleAutoRotation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleAutoRotation : MyActionBase
  {
    public override void ExecuteAction() => MyCubeBuilder.Static.AlignToDefault = !MyCubeBuilder.Static.AlignToDefault;

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      label.State = !MyCubeBuilder.Static.AlignToDefault ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleAutoRotation_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleAutoRotation_On);
      return label;
    }
  }
}
