// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpJetpack2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Jetpack2", 50)]
  internal class MyIngameHelpJetpack2 : MyIngameHelpObjective
  {
    private bool m_dampenersChanged;
    private bool m_dampenersInitialState;

    public MyIngameHelpJetpack2()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Jetpack_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Jetpack"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack2_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack2_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.DAMPING)
          },
          FinishCondition = new Func<bool>(this.DampenersCondition)
        }
      };
      this.FollowingId = "IngameHelp_JetpackTip";
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }

    public override void OnActivated()
    {
      base.OnActivated();
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || !controlledEntity.EnabledThrusts)
        return;
      this.m_dampenersInitialState = controlledEntity.EnabledDamping;
    }

    private bool DampenersCondition()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null && controlledEntity.EnabledThrusts && this.m_dampenersInitialState != controlledEntity.EnabledDamping)
        this.m_dampenersChanged = true;
      return this.m_dampenersChanged;
    }
  }
}
