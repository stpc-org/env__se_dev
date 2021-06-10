// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpStuck
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Input;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Stuck", 470)]
  internal class MyIngameHelpStuck : MyIngameHelpObjective
  {
    private Queue<float> m_averageSpeed = new Queue<float>();
    private int CountForAverage = 60;

    public MyIngameHelpStuck()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Stuck_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.StuckedInsideGrid);
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.TOGGLE_REACTORS)
          },
          FinishCondition = new Func<bool>(this.MovingInsideGrid)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Stuck_Detail3,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.LANDING_GEAR)
          },
          FinishCondition = new Func<bool>(this.MovingInsideGrid)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_StuckTip";
    }

    public override bool IsCritical() => this.StuckedInsideGrid();

    private bool StuckedInsideGrid()
    {
      if (MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.BlockDefinition.EnableShipControl && (controlledEntity.ControlThrusters && controlledEntity.EntityThrustComponent != null) && controlledEntity.EntityThrustComponent.ThrustCount > 0)
      {
        if (MyInput.Static.IsGameControlPressed(MyControlsSpace.FORWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.BACKWARD) || (MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT)))
        {
          this.m_averageSpeed.Enqueue(controlledEntity.CubeGrid.Physics.LinearVelocity.LengthSquared());
          if (this.m_averageSpeed.Count < this.CountForAverage)
            return false;
          if (this.m_averageSpeed.Count > this.CountForAverage)
          {
            double num = (double) this.m_averageSpeed.Dequeue();
          }
          return (double) this.m_averageSpeed.Average() < 1.0 / 1000.0;
        }
        this.m_averageSpeed.Clear();
      }
      return false;
    }

    private bool MovingInsideGrid()
    {
      if (MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.BlockDefinition.EnableShipControl && (controlledEntity.ControlThrusters && controlledEntity.EntityThrustComponent != null) && controlledEntity.EntityThrustComponent.ThrustCount > 0)
      {
        if (MyInput.Static.IsGameControlPressed(MyControlsSpace.FORWARD) || MyInput.Static.IsGameControlPressed(MyControlsSpace.BACKWARD) || (MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyInput.Static.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT)))
        {
          this.m_averageSpeed.Enqueue(controlledEntity.CubeGrid.Physics.LinearVelocity.LengthSquared());
          if (this.m_averageSpeed.Count < this.CountForAverage)
            return false;
          if (this.m_averageSpeed.Count > this.CountForAverage)
          {
            double num = (double) this.m_averageSpeed.Dequeue();
          }
          return (double) this.m_averageSpeed.Average() > 1.0;
        }
        this.m_averageSpeed.Clear();
      }
      return false;
    }
  }
}
