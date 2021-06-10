// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyCameraModeControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyCameraModeControlHelper : MyAbstractControlMenuItem
  {
    private string m_value;

    public MyCameraModeControlHelper()
      : base(MyControlsSpace.CAMERA_MODE)
    {
    }

    public override bool Enabled => MyGuiScreenGamePlay.Static.CanSwitchCamera;

    public override string CurrentValue => this.m_value;

    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode);

    public override void Activate() => MyGuiScreenGamePlay.Static.SwitchCamera();

    public override void UpdateValue()
    {
      if (MySession.Static.CameraController.IsInFirstPersonView)
        this.m_value = MyTexts.GetString(MySpaceTexts.ControlMenuItemValue_FPP);
      else
        this.m_value = MyTexts.GetString(MySpaceTexts.ControlMenuItemValue_TPP);
    }

    public override void Next() => this.Activate();

    public override void Previous() => this.Activate();
  }
}
