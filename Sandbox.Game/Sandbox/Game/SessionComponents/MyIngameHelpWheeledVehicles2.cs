// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpWheeledVehicles2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_WheeledVehicles2", 230)]
  internal class MyIngameHelpWheeledVehicles2 : MyIngameHelpObjective
  {
    private bool m_xPressed;

    public MyIngameHelpWheeledVehicles2()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_WheeledVehicles"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.InsidePoweredWheeledGrid);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles2_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles2_Detail2,
          FinishCondition = new Func<bool>(this.XPressed)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_WheeledVehiclesTip";
    }

    private bool InsidePoweredWheeledGrid() => this.TryGetPoweredWheeledGrid(out MyShipController _);

    private bool TryGetPoweredWheeledGrid(out MyShipController controller)
    {
      if (MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.CubeGrid.IsPowered && (controlledEntity.BlockDefinition.EnableShipControl && controlledEntity.ControlWheels) && controlledEntity.CubeGrid.GridSystems.WheelSystem.WheelCount > 0)
      {
        controller = (MyShipController) controlledEntity;
        return true;
      }
      controller = (MyShipController) null;
      return false;
    }

    private bool XPressed()
    {
      MyShipController controller;
      if (this.TryGetPoweredWheeledGrid(out controller) && !this.m_xPressed && controller.CubeGrid.GridSystems.WheelSystem.LastJumpInput)
        this.m_xPressed = true;
      return this.m_xPressed;
    }
  }
}
