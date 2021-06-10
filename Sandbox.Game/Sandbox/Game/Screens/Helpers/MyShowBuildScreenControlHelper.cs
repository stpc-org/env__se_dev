// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyShowBuildScreenControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyShowBuildScreenControlHelper : MyAbstractControlMenuItem
  {
    private IMyControllableEntity m_entity;

    public MyShowBuildScreenControlHelper()
      : base(MyControlsSpace.BUILD_SCREEN)
    {
    }

    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowBuildScreen);

    public void SetEntity(IMyControllableEntity entity) => this.m_entity = entity;

    public override void Activate()
    {
      MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));
      MyGuiScreenHudSpace.Static.HideScreen();
      MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) (this.m_entity as MyShipController), null));
    }
  }
}
