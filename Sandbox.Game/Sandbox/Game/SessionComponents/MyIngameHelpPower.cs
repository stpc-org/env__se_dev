// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpPower
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Power", 100)]
  internal class MyIngameHelpPower : MyIngameHelpObjective
  {
    private bool m_powerEnabled;

    public MyIngameHelpPower()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Power_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.InsideUnpoweredGrid);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Power_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Power_Detail2,
          FinishCondition = new Func<bool>(this.PowerEnabled)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_PowerTip";
    }

    private bool InsideUnpoweredGrid() => MySession.Static.ControlledEntity is MyCockpit controlledEntity && !controlledEntity.CubeGrid.IsPowered && controlledEntity.BlockDefinition.EnableShipControl && controlledEntity.ControlThrusters;

    private bool PowerEnabled()
    {
      if (!this.m_powerEnabled)
        this.m_powerEnabled = MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.CubeGrid.IsPowered;
      return this.m_powerEnabled;
    }
  }
}
