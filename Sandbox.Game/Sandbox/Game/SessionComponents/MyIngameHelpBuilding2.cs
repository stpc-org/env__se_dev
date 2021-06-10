// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBuilding2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Building2", 70)]
  internal class MyIngameHelpBuilding2 : MyIngameHelpObjective
  {
    private bool m_blockSizeChanged;

    public MyIngameHelpBuilding2()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Building"
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockSizeChanged += new Action(this.Static_OnBlockSizeChanged);
    }

    public override void CleanUp()
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockSizeChanged -= new Action(this.Static_OnBlockSizeChanged);
    }

    private void Static_OnBlockSizeChanged() => this.m_blockSizeChanged = true;

    private bool SizeSelectCondition() => this.m_blockSizeChanged;

    public override void OnBeforeActivate()
    {
      base.OnBeforeActivate();
      if (MyInput.Static.IsJoystickLastUsed)
      {
        MyIngameHelpBuilding2.GamepadVersion gamepadVersion = new MyIngameHelpBuilding2.GamepadVersion(this);
      }
      else
      {
        MyIngameHelpBuilding2.MouseAndKeyboardVersion andKeyboardVersion = new MyIngameHelpBuilding2.MouseAndKeyboardVersion(this);
      }
    }

    private class MouseAndKeyboardVersion
    {
      private bool m_insertPressed;
      private bool m_deletePressed;
      private bool m_homePressed;
      private bool m_endPressed;
      private bool m_pageUpPressed;
      private bool m_pageDownPressed;

      public MouseAndKeyboardVersion(MyIngameHelpBuilding2 help) => help.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail2,
          FinishCondition = new Func<bool>(help.SizeSelectCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail3,
          FinishCondition = new Func<bool>(this.RotateCondition)
        }
      };

      private bool RotateCondition()
      {
        if (!MyCubeBuilder.Static.IsActivated || MyCubeBuilder.Static.ToolbarBlockDefinition == null || MyScreenManager.FocusedControl != null)
          return false;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE))
          this.m_insertPressed = true;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE))
          this.m_deletePressed = true;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE))
          this.m_homePressed = true;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE))
          this.m_endPressed = true;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE))
          this.m_pageUpPressed = true;
        if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE))
          this.m_pageDownPressed = true;
        return this.m_insertPressed && this.m_deletePressed && (this.m_homePressed && this.m_endPressed) && this.m_pageUpPressed && this.m_pageDownPressed;
      }
    }

    private class GamepadVersion
    {
      private bool m_axisChanged;
      private bool m_rotatedLeftOnAxis;
      private bool m_rotatedRightOnAxis;

      public GamepadVersion(MyIngameHelpBuilding2 help) => help.Details = new MyIngameHelpDetail[4]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail2_Gamepad,
          FinishCondition = new Func<bool>(help.SizeSelectCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail3_Gamepad,
          FinishCondition = new Func<bool>(this.ChangeAxisCondition)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building2_Detail4_Gamepad,
          FinishCondition = new Func<bool>(this.RotateOnAxisCondition)
        }
      };

      private bool ChangeAxisCondition()
      {
        IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        this.m_axisChanged |= MyControllerHelper.IsControl(controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty, MyControlsSpace.CHANGE_ROTATION_AXIS);
        return this.m_axisChanged;
      }

      private bool RotateOnAxisCondition()
      {
        IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
        this.m_rotatedLeftOnAxis |= MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED);
        this.m_rotatedRightOnAxis |= MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED);
        return this.m_rotatedLeftOnAxis && this.m_rotatedRightOnAxis;
      }
    }
  }
}
