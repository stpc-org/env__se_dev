// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyFlightBindingScheme
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game;
using System;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
  internal class MyFlightBindingScheme : IMyControlsBindingScheme
  {
    public void CreateBinding(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
    {
      this.CreateForBase();
      this.CreateForCharacter(invertYAxisCharacter, invertYAxisJetpackVehicle);
      this.CreateForJetpack(invertYAxisCharacter, invertYAxisJetpackVehicle);
      this.CreateForSpaceship(invertYAxisCharacter, invertYAxisJetpackVehicle);
      MyFlightBindingScheme.CreateForSpectator(invertYAxisCharacter, invertYAxisJetpackVehicle);
    }

    private void CreateForBase()
    {
      MyStringId cxBase = MySpaceBindingCreator.CX_BASE;
      MyControllerHelper.AddContext(cxBase);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_MODIFIER_LB, MyJoystickButtonsEnum.J05);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_MODIFIER_RB, MyJoystickButtonsEnum.J06);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.INVENTORY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, false);
      MyControllerHelper.AddControl(cxBase, MyControlsGUI.MAIN_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J08, false);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FORWARD, MyJoystickAxesEnum.Yneg);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.BACKWARD, MyJoystickAxesEnum.Ypos);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.LOOKAROUND, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, true);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.ROLL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, false);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.CAMERA_ZOOM_IN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Yneg, true);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.CAMERA_ZOOM_OUT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Ypos, true);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_LS, '\xE009'.ToString());
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_RS, '\xE00A'.ToString());
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_CAMERA_ZOOM, "\xE005+\xE006+\xE025".ToString());
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.USE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, false);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.DAMPING, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.DAMPING_RELATIVE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, true);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.HEADLIGHTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.CAMERA_MODE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.SYSTEM_RADIAL_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J10, false);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.CUTSCENE_SKIPPER, MyJoystickButtonsEnum.J07);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.WARNING_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J08, true);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_UP, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_DOWN, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_LEFT, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(cxBase, MyControlsSpace.FAKE_RIGHT, MyJoystickButtonsEnum.JDRight);
    }

    private void CreateForCharacter(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
    {
      MyStringId cxCharacter = MySpaceBindingCreator.CX_CHARACTER;
      MyControllerHelper.AddContext(cxCharacter, new MyStringId?(MySpaceBindingCreator.CX_BASE));
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, false);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.STRAFE_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xneg, false);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.STRAFE_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xpos, false);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_LEFT, MyJoystickAxesEnum.RotationXneg);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_RIGHT, MyJoystickAxesEnum.RotationXpos);
      if (invertYAxisCharacter)
      {
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYneg);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYpos);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
      }
      else
      {
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYneg);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYpos);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
        MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
      }
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, true);
      MyControllerHelper.AddControl(cxCharacter, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, true);
      this.CreateCommonForCharacterAndJetpack(cxCharacter);
    }

    private void CreateCommonForCharacterAndJetpack(MyStringId context)
    {
      MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.THRUSTS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.HELMET, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(context, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(context, MyControlsSpace.CONSUME_HEALTH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, true);
      MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, true);
      MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07);
      MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J07);
      MyControllerHelper.AddControl(context, MyControlsSpace.COLOR_TOOL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(context, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, true);
      MyControllerHelper.AddControl(context, MyControlsSpace.RELOAD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, true);
    }

    private void CreateForJetpack(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
    {
      MyStringId cxJetpack = MySpaceBindingCreator.CX_JETPACK;
      MyControllerHelper.AddContext(cxJetpack, new MyStringId?(MySpaceBindingCreator.CX_BASE));
      this.CreateCommonForCharacterAndJetpack(cxJetpack);
      this.CreateCommonForJetpackAndShip(cxJetpack, invertYAxisCharacter, invertYAxisJetpackVehicle);
    }

    private void CreateForSpaceship(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
    {
      MyStringId cxSpaceship = MySpaceBindingCreator.CX_SPACESHIP;
      MyControllerHelper.AddContext(cxSpaceship, new MyStringId?(MySpaceBindingCreator.CX_BASE));
      this.CreateCommonForJetpackAndShip(cxSpaceship, invertYAxisCharacter, invertYAxisJetpackVehicle);
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.LANDING_GEAR, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, false);
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.TOGGLE_REACTORS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.WHEEL_JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03);
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, true);
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.FAKE_LS, "\xE005+\xE006+\xE009");
      MyControllerHelper.AddControl(cxSpaceship, MyControlsSpace.FAKE_RS, "\xE005+\xE006+\xE00A");
    }

    private void CreateCommonForJetpackAndShip(
      MyStringId context,
      bool invertYAxisCharacter,
      bool invertYAxisJetpackVehicle)
    {
      MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg);
      MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos);
      if (invertYAxisJetpackVehicle)
      {
        MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYneg, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYpos, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
        MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
      }
      else
      {
        MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYneg, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYpos, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
        MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
      }
      MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_LEFT, MyJoystickAxesEnum.RotationXneg, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
      MyControllerHelper.AddControl(context, MyControlsSpace.ROTATION_RIGHT, MyJoystickAxesEnum.RotationXpos, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
      MyControllerHelper.AddControl(context, MyControlsSpace.STRAFE_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xneg, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.STRAFE_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xpos, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, false);
      MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, true);
      MyControllerHelper.AddControl(context, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, true);
    }

    private static void CreateForSpectator(
      bool invertYAxisCharacter,
      bool invertYAxisJetpackVehicle)
    {
      MyStringId cxSpectator = MySpaceBindingCreator.CX_SPECTATOR;
      MyControllerHelper.AddContext(cxSpectator, new MyStringId?(MySpaceBindingCreator.CX_BASE));
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_FOCUS_PLAYER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_PLAYER_CONTROL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_LOCK, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_TELEPORT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_SPEED_BOOST, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J10);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.COLOR_TOOL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_LS, '\xE009'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_RS, '\xE00A'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_LS_PRESS, '\xE00B'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_RS_PRESS, '\xE00C'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_LB_RB_LS, "\xE005+\xE006+" + '\xE009'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_LB_RB_RS, "\xE005+\xE006+" + '\xE00A'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.FAKE_LB_ROTATION_H, "\xE005+" + '\xE024'.ToString());
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos);
      if (invertYAxisJetpackVehicle)
      {
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_DOWN, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYneg, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_UP, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYpos, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
      }
      else
      {
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_UP, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYneg, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_DOWN, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationYpos, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYneg, true);
        MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationYpos, true);
      }
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.ROTATION_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos, false, (Func<bool>) (() => !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED)));
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.STRAFE_LEFT, MyJoystickAxesEnum.Xneg);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.STRAFE_RIGHT, MyJoystickAxesEnum.Xpos);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, false);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXneg, true);
      MyControllerHelper.AddControl(cxSpectator, MyControlsSpace.LOOK_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.RotationXpos, true);
    }
  }
}
