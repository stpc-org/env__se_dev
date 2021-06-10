// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyConnectorControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyConnectorControlHelper : MyControllableEntityControlHelper
  {
    private MyShipController ShipController => this.m_entity as MyShipController;

    public override bool Enabled => this.ShipController.CubeGrid.GridSystems.ConveyorSystem.IsInteractionPossible;

    public MyConnectorControlHelper()
      : base(MyControlsSpace.LANDING_GEAR, (Action<IMyControllableEntity>) (x => x.SwitchLandingGears()), (Func<IMyControllableEntity, bool>) (x => MyConnectorControlHelper.GetConnectorStatus(x)), MySpaceTexts.ControlMenuItemLabel_Connectors)
    {
    }

    public new void SetEntity(IMyControllableEntity entity) => this.m_entity = (IMyControllableEntity) (entity as MyShipController);

    private static bool GetConnectorStatus(IMyControllableEntity shipController) => (shipController as MyShipController).CubeGrid.GridSystems.ConveyorSystem.Connected;
  }
}
