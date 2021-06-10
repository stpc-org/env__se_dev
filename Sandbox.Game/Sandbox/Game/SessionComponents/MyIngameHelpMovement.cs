// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpMovement
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using VRage.Game;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Movement", 30)]
  internal class MyIngameHelpMovement : MyIngameHelpObjective
  {
    private bool m_crouched;
    private bool m_jumped;
    private bool m_forwardPressed;
    private bool m_backPressed;
    private bool m_leftPressed;
    private bool m_rightPressed;
    private bool m_running;

    public MyIngameHelpMovement()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Movement_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = new Func<bool>(this.StandingCondition);
      this.Details = new MyIngameHelpDetail[5]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Movement_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Movement_Detail2,
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
          TextEnum = MySpaceTexts.IngameHelp_Movement_Detail3,
          Args = new object[2]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SPRINT),
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.FORWARD)
          },
          FinishCondition = new Func<bool>(this.SprintCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Movement_Detail4,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.JUMP)
          },
          FinishCondition = new Func<bool>(this.JumpCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Movement_Detail5,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CROUCH)
          },
          FinishCondition = new Func<bool>(this.CrouchCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }

    private bool StandingCondition() => MySession.Static.ControlledEntity is MyCharacter controlledEntity && controlledEntity.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND;

    private bool WSADCondition()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return false;
      if (this.StandingCondition())
      {
        this.m_forwardPressed |= (double) controlledEntity.LastMotionIndicator.Z > 0.0;
        this.m_backPressed |= (double) controlledEntity.LastMotionIndicator.Z < 0.0;
        this.m_leftPressed |= (double) controlledEntity.LastMotionIndicator.X < 0.0;
        this.m_rightPressed |= (double) controlledEntity.LastMotionIndicator.X > 0.0;
      }
      return this.m_forwardPressed && this.m_backPressed && this.m_leftPressed && this.m_rightPressed;
    }

    private bool JumpCondition()
    {
      if (MySession.Static.ControlledEntity is MyCharacter controlledEntity && controlledEntity.CurrentMovementState == MyCharacterMovementEnum.Jump)
        this.m_jumped = true;
      return this.m_jumped;
    }

    private bool SprintCondition()
    {
      if (this.StandingCondition() && MySession.Static.ControlledEntity is MyCharacter controlledEntity && (controlledEntity.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND && controlledEntity.IsSprinting))
        this.m_running = true;
      return this.m_running;
    }

    private bool CrouchCondition()
    {
      if (this.StandingCondition() && MySession.Static.ControlledEntity is MyCharacter controlledEntity && (controlledEntity.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND && controlledEntity.IsCrouching))
        this.m_crouched = true;
      return this.m_crouched;
    }
  }
}
