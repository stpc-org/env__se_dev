// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionShowPlayers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionShowPlayers : MyActionBase
  {
    public override void ExecuteAction()
    {
      if (!Sync.MultiplayerActive)
        return;
      MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
    }

    public override bool IsEnabled() => Sync.MultiplayerActive;

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (!Sync.MultiplayerActive)
        label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_MultiplayerOnly);
      return label;
    }
  }
}
