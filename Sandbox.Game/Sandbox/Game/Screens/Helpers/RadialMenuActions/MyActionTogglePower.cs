// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionTogglePower
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionTogglePower : MyActionBase
  {
    public override void ExecuteAction() => MySession.Static.ControlledEntity?.SwitchReactors();

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      label.State = MySession.Static.ControlledEntity != null ? (!MySession.Static.ControlledEntity.EnabledReactors ? MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_Off) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_On)) : MyTexts.GetString(MySpaceTexts.RadialMenuAction_EnablePower_None);
      return label;
    }
  }
}
