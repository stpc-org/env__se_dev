// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleSignals
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleSignals : MyActionBase
  {
    public override void ExecuteAction() => MyHudMarkerRender.ChangeSignalMode();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      switch (MyHudMarkerRender.SignalDisplayMode)
      {
        case MyHudMarkerRender.SignalMode.DefaultMode:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Signal_Default);
          break;
        case MyHudMarkerRender.SignalMode.FullDisplay:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Signal_Full);
          break;
        case MyHudMarkerRender.SignalMode.NoNames:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Signal_NoNames);
          break;
        case MyHudMarkerRender.SignalMode.Off:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Signal_Off);
          break;
        case MyHudMarkerRender.SignalMode.MaxSignalModes:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_Signal_MaxSignals);
          break;
      }
      return label;
    }
  }
}
