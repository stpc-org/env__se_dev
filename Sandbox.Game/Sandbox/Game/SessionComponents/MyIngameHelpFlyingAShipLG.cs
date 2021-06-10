// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpFlyingAShipLG
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_FlyingAShipLG", 165)]
  internal class MyIngameHelpFlyingAShipLG : MyIngameHelpObjective
  {
    private bool m_toggleLandingGear;
    private bool m_initialLandingGear;

    public MyIngameHelpFlyingAShipLG()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.InsidePoweredGridWithLG);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLG_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLG_Detail2,
          FinishCondition = new Func<bool>(this.ToggleLandingGear)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_FlyingAShipLGTip";
    }

    public override void OnActivated()
    {
      base.OnActivated();
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return;
      this.m_initialLandingGear = controlledEntity.EnabledLeadingGears;
    }

    private bool InsidePoweredGridWithLG() => (!(MySession.Static.ControlledEntity is MyCockpit controlledEntity) || !controlledEntity.CubeGrid.IsPowered || !controlledEntity.BlockDefinition.EnableShipControl ? 0 : (controlledEntity.ControlThrusters ? 1 : 0)) != 0 && controlledEntity.CubeGrid.GridSystems.LandingSystem.TotalGearCount > 0;

    private bool ToggleLandingGear()
    {
      if (this.InsidePoweredGridWithLG() && !this.m_toggleLandingGear)
        this.m_toggleLandingGear = this.m_initialLandingGear != MySession.Static.ControlledEntity.EnabledLeadingGears;
      return this.m_toggleLandingGear;
    }
  }
}
