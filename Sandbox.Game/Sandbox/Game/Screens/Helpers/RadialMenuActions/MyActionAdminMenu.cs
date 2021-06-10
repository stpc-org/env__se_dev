// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionAdminMenu
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionAdminMenu : MyActionBase
  {
    public override void ExecuteAction() => MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));

    public override bool IsEnabled() => MySession.Static.IsAdminMenuEnabled;

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (!MySession.Static.IsAdminMenuEnabled)
        label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_AdminOnly);
      return label;
    }
  }
}
