// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionShowProgressionTree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionShowProgressionTree : MyActionBase
  {
    public override void ExecuteAction() => MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) (MySession.Static.ControlledEntity as MyShipController), (object) "ResearchPage", (object) true, null));
  }
}
