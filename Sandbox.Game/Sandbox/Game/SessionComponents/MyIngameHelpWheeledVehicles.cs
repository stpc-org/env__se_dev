// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpWheeledVehicles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_WheeledVehicles", 230)]
  internal class MyIngameHelpWheeledVehicles : MyIngameHelpObjective
  {
    private bool m_powerSwitched;
    private bool m_toggleLandingGear;
    private bool m_forwardPressed;
    private bool m_backPressed;
    private bool m_leftPressed;
    private bool m_rightPressed;
    private bool m_spacePressed;
    private bool m_originalHandbrake;

    public MyIngameHelpWheeledVehicles()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.InsidePoweredWheeledGrid);
      this.Details = new MyIngameHelpDetail[5]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail2,
          FinishCondition = new Func<bool>(this.PowerSwitched)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail3,
          FinishCondition = new Func<bool>(this.ToggleLandingGear)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail4,
          FinishCondition = new Func<bool>(this.WSADCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail5,
          FinishCondition = new Func<bool>(this.BrakeCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MyShipController controller;
      if (!this.TryGetPoweredWheeledGrid(out controller))
        return;
      this.m_originalHandbrake = controller.CubeGrid.GridSystems.WheelSystem.HandBrake;
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

    private bool PowerSwitched()
    {
      if (!this.m_powerSwitched)
        this.m_powerSwitched = MySession.Static.ControlledEntity is MyCockpit controlledEntity && !controlledEntity.CubeGrid.IsPowered;
      return this.m_powerSwitched;
    }

    private bool ToggleLandingGear()
    {
      MyShipController controller;
      if (this.TryGetPoweredWheeledGrid(out controller))
        this.m_toggleLandingGear |= this.m_originalHandbrake != controller.CubeGrid.GridSystems.WheelSystem.HandBrake;
      return this.m_toggleLandingGear;
    }

    private bool WSADCondition()
    {
      MyShipController controller;
      if (!this.TryGetPoweredWheeledGrid(out controller))
        return false;
      this.m_forwardPressed |= (double) controller.LastMotionIndicator.Z > 0.0;
      this.m_backPressed |= (double) controller.LastMotionIndicator.Z < 0.0;
      this.m_leftPressed |= (double) controller.LastMotionIndicator.X < 0.0;
      this.m_rightPressed |= (double) controller.LastMotionIndicator.X > 0.0;
      return this.m_forwardPressed && this.m_backPressed && this.m_leftPressed && this.m_rightPressed;
    }

    private bool BrakeCondition()
    {
      MyShipController controller;
      if (this.TryGetPoweredWheeledGrid(out controller))
        this.m_spacePressed |= controller.CubeGrid.GridSystems.WheelSystem.Brake;
      return this.m_spacePressed;
    }
  }
}
