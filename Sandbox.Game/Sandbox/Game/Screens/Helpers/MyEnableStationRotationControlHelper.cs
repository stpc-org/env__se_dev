// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyEnableStationRotationControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyEnableStationRotationControlHelper : MyAbstractControlMenuItem
  {
    private IMyControllableEntity m_entity;

    public MyEnableStationRotationControlHelper()
      : base(MyControlsSpace.FREE_ROTATION)
    {
    }

    public override void Activate() => MyScreenManager.CloseScreen(typeof (MyGuiScreenControlMenu));

    public override string Label => MyTexts.GetString(MySpaceTexts.StationRotation_Static);
  }
}
