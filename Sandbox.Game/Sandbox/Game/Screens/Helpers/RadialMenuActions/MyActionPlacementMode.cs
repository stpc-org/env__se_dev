// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionPlacementMode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents.Clipboard;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionPlacementMode : MyActionBase
  {
    public override void ExecuteAction()
    {
      MyClipboardComponent.Static.ChangeStationRotation();
      MyCubeBuilder.Static.CycleCubePlacementMode();
    }

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (MyCubeBuilder.Static.IsActivated)
      {
        switch (MyCubeBuilder.Static.CubePlacementMode)
        {
          case MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem:
            label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Local);
            break;
          case MyCubeBuilder.CubePlacementModeEnum.FreePlacement:
            label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Free);
            break;
          case MyCubeBuilder.CubePlacementModeEnum.GravityAligned:
            label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_Grid_Gravity);
            break;
        }
      }
      else if (MyClipboardComponent.Static.IsActive)
        label.State = !MyClipboardComponent.Static.IsStationRotationenabled() ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_ClipboardRoattion_Disabled) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_PlacementMode_ClipboardRoattion_Enabled);
      return label;
    }
  }
}
