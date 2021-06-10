// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpJetpack
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Jetpack", 40)]
  internal class MyIngameHelpJetpack : MyIngameHelpObjective
  {
    private bool m_jetpackEnabled;
    private bool m_downPressed;
    private bool m_upPressed;
    private bool m_forwardPressed;
    private bool m_backPressed;
    private bool m_leftPressed;
    private bool m_rightPressed;
    private bool m_rollLeftPressed;
    private bool m_rollRightPressed;

    public MyIngameHelpJetpack()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Jetpack_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = new Func<bool>(this.JetpackInWorldSettings);
      this.Details = new MyIngameHelpDetail[5]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail2,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.THRUSTS)
          },
          FinishCondition = new Func<bool>(this.JetpackCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail3,
          Args = new object[2]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.JUMP),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CROUCH)
          },
          FinishCondition = new Func<bool>(this.FlyCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail4,
          Args = new object[4]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.FORWARD),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.BACKWARD),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.STRAFE_LEFT),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.STRAFE_RIGHT)
          },
          FinishCondition = new Func<bool>(this.WSADCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail5,
          Args = new object[2]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.ROLL_LEFT),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.ROLL_RIGHT)
          },
          FinishCondition = new Func<bool>(this.RollCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_Jetpack2";
    }

    private bool JetpackInWorldSettings() => MySession.Static != null && MySession.Static.Settings.EnableJetpack && MySession.Static.LocalCharacter != null && !MySession.Static.LocalCharacter.IsSitting;

    private bool JetpackCondition()
    {
      if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.EnabledThrusts)
        this.m_jetpackEnabled = true;
      return this.m_jetpackEnabled;
    }

    private bool FlyCondition()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || !controlledEntity.EnabledThrusts)
        return false;
      this.m_downPressed |= (double) controlledEntity.LastMotionIndicator.Y < 0.0;
      this.m_upPressed |= (double) controlledEntity.LastMotionIndicator.Y > 0.0;
      return this.m_downPressed && this.m_upPressed;
    }

    private bool WSADCondition()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || !controlledEntity.EnabledThrusts)
        return false;
      this.m_forwardPressed |= (double) controlledEntity.LastMotionIndicator.Z > 0.0;
      this.m_backPressed |= (double) controlledEntity.LastMotionIndicator.Z < 0.0;
      this.m_leftPressed |= (double) controlledEntity.LastMotionIndicator.X < 0.0;
      this.m_rightPressed |= (double) controlledEntity.LastMotionIndicator.X > 0.0;
      return this.m_forwardPressed && this.m_backPressed && this.m_leftPressed && this.m_rightPressed;
    }

    private bool RollCondition()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || !controlledEntity.EnabledThrusts)
        return false;
      this.m_rollLeftPressed |= (double) controlledEntity.LastRotationIndicator.Z < 0.0;
      this.m_rollRightPressed |= (double) controlledEntity.LastRotationIndicator.Z > 0.0;
      return this.m_rollLeftPressed && this.m_rollRightPressed;
    }
  }
}
