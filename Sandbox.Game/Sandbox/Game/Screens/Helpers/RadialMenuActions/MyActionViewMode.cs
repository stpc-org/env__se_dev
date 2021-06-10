// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionViewMode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionViewMode : MyActionBase
  {
    public override void ExecuteAction()
    {
      if (!this.IsEnabled())
        return;
      if (MySession.Static.CameraController == MySpectator.Static)
        MyGuiScreenGamePlay.SetSpectatorNone();
      else
        MyGuiScreenGamePlay.SetSpectatorFree();
    }

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (MySession.Static.LocalHumanPlayer == null || !MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId))
        label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
      label.State = MySession.Static.CameraController != MySpectator.Static ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleViewMode_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_ToggleViewMode_On);
      return label;
    }

    public override bool IsEnabled() => MySession.Static.LocalHumanPlayer != null && MySession.Static.HasPlayerSpectatorRights(MySession.Static.LocalHumanPlayer.Id.SteamId);
  }
}
