// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpFlyingAShip
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_FlyingAShip", 170)]
  internal class MyIngameHelpFlyingAShip : MyIngameHelpObjective
  {
    private bool m_cPressed;
    private bool m_spacePressed;
    private bool m_qPressed;
    private bool m_ePressed;
    private bool m_powerSwitched;

    public MyIngameHelpFlyingAShip()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.InsidePoweredGrid);
      this.Details = new MyIngameHelpDetail[4]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail2,
          FinishCondition = new Func<bool>(this.PowerSwitched)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail3,
          FinishCondition = new Func<bool>(this.FlyCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail4,
          FinishCondition = new Func<bool>(this.RollCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_FlyingAShipTip";
    }

    private bool TryGetPoweredGrid(out MyShipController controller)
    {
      if (MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.CubeGrid.IsPowered && (controlledEntity.BlockDefinition.EnableShipControl && controlledEntity.ControlThrusters) && (controlledEntity.EntityThrustComponent != null && controlledEntity.EntityThrustComponent.ThrustCount > 0))
      {
        controller = (MyShipController) controlledEntity;
        return true;
      }
      controller = (MyShipController) null;
      return false;
    }

    private bool InsidePoweredGrid() => this.TryGetPoweredGrid(out MyShipController _);

    private bool PowerSwitched()
    {
      if (!this.m_powerSwitched)
        this.m_powerSwitched = MySession.Static.ControlledEntity is MyCockpit controlledEntity && !controlledEntity.CubeGrid.IsPowered;
      return this.m_powerSwitched;
    }

    private bool FlyCondition()
    {
      MyShipController controller;
      if (this.TryGetPoweredGrid(out controller))
      {
        this.m_cPressed |= (double) controller.LastMotionIndicator.Y < 0.0;
        this.m_spacePressed |= (double) controller.LastMotionIndicator.Y > 0.0;
      }
      return this.m_cPressed && this.m_spacePressed;
    }

    private bool RollCondition()
    {
      MyShipController controller;
      if (this.TryGetPoweredGrid(out controller))
      {
        this.m_qPressed |= (double) controller.LastRotationIndicator.Z < 0.0;
        this.m_ePressed |= (double) controller.LastRotationIndicator.Z > 0.0;
      }
      return this.m_qPressed && this.m_ePressed;
    }
  }
}
