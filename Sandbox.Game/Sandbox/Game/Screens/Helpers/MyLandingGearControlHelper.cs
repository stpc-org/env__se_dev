// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLandingGearControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyLandingGearControlHelper : MyControllableEntityControlHelper
  {
    private MyShipController ShipController => this.m_entity as MyShipController;

    public override bool Enabled => (uint) this.ShipController.CubeGrid.GridSystems.LandingSystem.Locked > 0U;

    public MyLandingGearControlHelper()
      : base(MyControlsSpace.LANDING_GEAR, (Action<IMyControllableEntity>) (x => x.SwitchLandingGears()), (Func<IMyControllableEntity, bool>) (x => x.EnabledLeadingGears), MySpaceTexts.ControlMenuItemLabel_LandingGear)
    {
    }

    public new void SetEntity(IMyControllableEntity entity) => this.m_entity = (IMyControllableEntity) (entity as MyShipController);
  }
}
