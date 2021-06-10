// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MySpaceBindingCreator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
  public static class MySpaceBindingCreator
  {
    public static readonly MyStringId CX_BASE = MyControllerHelper.CX_BASE;
    public static readonly MyStringId CX_GUI = MyControllerHelper.CX_GUI;
    public static readonly MyStringId CX_CHARACTER = MyControllerHelper.CX_CHARACTER;
    public static readonly MyStringId CX_JETPACK = MyStringId.GetOrCompute("JETPACK");
    public static readonly MyStringId CX_SPACESHIP = MyStringId.GetOrCompute("SPACESHIP");
    public static readonly MyStringId CX_SPECTATOR = MyStringId.GetOrCompute("SPECTATOR");
    public static readonly MyStringId AX_BASE = MyStringId.GetOrCompute("ABASE");
    public static readonly MyStringId AX_TOOLS = MyStringId.GetOrCompute("TOOLS");
    public static readonly MyStringId AX_BUILD = MyStringId.GetOrCompute("BUILD");
    public static readonly MyStringId AX_SYMMETRY = MyStringId.GetOrCompute(nameof (AX_SYMMETRY));
    public static readonly MyStringId AX_VOXEL = MyStringId.GetOrCompute("VOXEL");
    public static readonly MyStringId AX_ACTIONS = MyStringId.GetOrCompute("ACTIONS");
    public static readonly MyStringId AX_COLOR_PICKER = MyStringId.GetOrCompute("COLOR_PICKER");
    public static readonly MyStringId AX_CLIPBOARD = MyStringId.GetOrCompute("CLIPBOARD");
    private static MyFlightBindingScheme m_flightBindingScheme = new MyFlightBindingScheme();
    private static MyFlightAlternativeBindingScheme m_flightAltBindingScheme = new MyFlightAlternativeBindingScheme();
    public static readonly ITextEvaluator BindingEvaluator = (ITextEvaluator) new MySpaceBindingCreator.SpaceBindingEvaluator();
    public static readonly ITextEvaluator JoystickEvaluator = (ITextEvaluator) new MySpaceBindingCreator.JoystickBindingEvaluator();

    public static void CreateBindingDefault() => MySpaceBindingCreator.CreateBinding(MyInput.Static.GetJoystickYInversionCharacter(), MyInput.Static.GetJoystickYInversionVehicle());

    public static void CreateBinding(bool invertYAxisCharacter, bool invertYAxisJetpackVehicle)
    {
      MyControllerHelper.ClearBindings();
      switch (MySandboxGame.Config.GamepadSchemeId)
      {
        case 0:
          MySpaceBindingCreator.m_flightBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
          break;
        case 1:
          MySpaceBindingCreator.m_flightAltBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
          break;
        default:
          MySpaceBindingCreator.m_flightBindingScheme.CreateBinding(invertYAxisCharacter, invertYAxisJetpackVehicle);
          break;
      }
      MySpaceBindingCreator.CreateForGUI();
      MySpaceBindingCreator.CreateForAuxiliaryBase();
      MySpaceBindingCreator.CreateForTools();
      MySpaceBindingCreator.CreateForBuildMode();
      MySpaceBindingCreator.CreateForSymmetry();
      MySpaceBindingCreator.CreateForVoxelHands();
      MySpaceBindingCreator.CreateForClipboard();
      MySpaceBindingCreator.CreateForActions();
      MySpaceBindingCreator.CreateForColorPicker();
    }

    public static void RegisterEvaluators()
    {
      MyTexts.RegisterEvaluator("CONTROL", MySpaceBindingCreator.BindingEvaluator);
      if (MyInput.Static is ITextEvaluator eval)
        MyTexts.RegisterEvaluator("GAME_CONTROL", eval);
      MyTexts.RegisterEvaluator("GAMEPAD", MyControllerHelper.ButtonTextEvaluator);
      MyTexts.RegisterEvaluator("GAMEPAD_CONTROL", MySpaceBindingCreator.JoystickEvaluator);
    }

    private static void CreateForAuxiliaryBase()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_BASE);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SELECT_1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SELECT_2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SELECT_3, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SELECT_4, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.VOXEL_SELECT_SPHERE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, (Func<bool>) (() => MySession.Static.CreativeMode));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.TOGGLE_SIGNALS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, (Func<bool>) (() => MySession.Static.SurvivalMode));
    }

    private static void CreateForTools()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_TOOLS, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOOL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOOL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOOL_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOOL_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOGGLE_HUD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.CHAT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.PROGRESSION_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
    }

    private static void CreateForBuildMode()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_BUILD, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.USE_SYMMETRY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_CANCEL, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_REMOVE, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_ADD, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH_ALTERNATIVE, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.SurvivalMode));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.CreativeMode));
    }

    private static void CreateForSymmetry()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_SYMMETRY, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.SurvivalMode));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.CreativeMode));
    }

    private static void CreateForVoxelHands()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_VOXEL, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_PLACE_DUMMY_RELEASE, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_REVERT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_PAINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
    }

    private static void CreateForClipboard()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_CLIPBOARD, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_CANCEL, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.SWITCH_BUILDING_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_CLIPBOARD, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
    }

    private static void CreateForColorPicker()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_COLOR_PICKER, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SATURATION_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.VALUE_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.VALUE_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SATURATION_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.COPY_COLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.RECOLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J09, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.SurvivalMode));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, (Func<bool>) (() => MySession.Static.CreativeMode));
    }

    private static void CreateForActions()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.AX_ACTIONS, new MyStringId?(MySpaceBindingCreator.AX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CUBE_COLOR_CHANGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTION_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTION_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTION_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTION_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOGGLE_HUD, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CHAT_SCREEN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J01, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT, MyJoystickButtonsEnum.J09, MyJoystickButtonsEnum.J02, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03);
    }

    private static void CreateForGUI()
    {
      MyControllerHelper.AddContext(MySpaceBindingCreator.CX_GUI, new MyStringId?(MySpaceBindingCreator.CX_BASE));
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.CANCEL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.CANCEL_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION1_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION2_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_UP, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_DOWN, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_LEFT, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_RIGHT, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT, MyJoystickAxesEnum.Zpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT, MyJoystickAxesEnum.Zneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyJoystickButtonsEnum.J05);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyJoystickButtonsEnum.J06);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_UP, MyJoystickAxesEnum.Yneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_DOWN, MyJoystickAxesEnum.Ypos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_LEFT, MyJoystickAxesEnum.Xneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.PAGE_RIGHT, MyJoystickAxesEnum.Xpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SCROLL_UP, MyJoystickAxesEnum.RotationYneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SCROLL_DOWN, MyJoystickAxesEnum.RotationYpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SCROLL_LEFT, MyJoystickAxesEnum.RotationXneg);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SCROLL_RIGHT, MyJoystickAxesEnum.RotationXpos);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON, MyJoystickButtonsEnum.J09);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON, MyJoystickButtonsEnum.J10);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDRight);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_ITEM_UP, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A, MyJoystickButtonsEnum.J01);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_B, MyJoystickButtonsEnum.J02);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X, MyJoystickButtonsEnum.J03);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y, MyJoystickButtonsEnum.J04);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON, MyJoystickButtonsEnum.J05);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON, MyJoystickButtonsEnum.J06);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LISTBOX_TOGGLE_SELECTION, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LISTBOX_SELECT_RANGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LISTBOX_SELECT_ALL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, true);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LISTBOX_SELECT_ONLY_FOCUSED, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LISTBOX_SIMPLE_SELECT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, false);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.FAKE_RS, '\xE00A'.ToString());
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MODIF_L, MyJoystickButtonsEnum.J05);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MODIF_R, MyJoystickButtonsEnum.J06);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW, MyJoystickButtonsEnum.J07);
      MyControllerHelper.AddControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MENU, MyJoystickButtonsEnum.J08);
    }

    private class JoystickBindingEvaluator : ITextEvaluator
    {
      public static void ParseToken(
        ref string token,
        out MyStringId controlContext,
        out MyStringId control)
      {
        int length = token.IndexOf(':');
        if (length >= 0)
        {
          controlContext = MyStringId.GetOrCompute(token.Substring(0, length));
          token = token.Substring(length + 1);
          control = MyStringId.GetOrCompute(token);
        }
        else
        {
          control = MyStringId.GetOrCompute(token);
          IMyControllableEntity controlledEntity = MySession.Static?.ControlledEntity;
          if (controlledEntity == null)
          {
            controlContext = MySpaceBindingCreator.CX_BASE;
          }
          else
          {
            MyStringId auxiliaryContext = controlledEntity.AuxiliaryContext;
            if (MyControllerHelper.IsDefined(auxiliaryContext, control))
              controlContext = auxiliaryContext;
            else
              controlContext = controlledEntity.ControlContext;
          }
        }
      }

      public string TokenEvaluate(string token, string context)
      {
        MyStringId controlContext;
        MyStringId control;
        MySpaceBindingCreator.JoystickBindingEvaluator.ParseToken(ref token, out controlContext, out control);
        return MyControllerHelper.GetCodeForControl(controlContext, control);
      }
    }

    private class SpaceBindingEvaluator : ITextEvaluator
    {
      public string TokenEvaluate(string token, string context)
      {
        if (MyInput.Static.IsJoystickLastUsed)
          return MySpaceBindingCreator.JoystickEvaluator.TokenEvaluate(token, context);
        if (!(MyInput.Static is ITextEvaluator textEvaluator))
          return token;
        MySpaceBindingCreator.JoystickBindingEvaluator.ParseToken(ref token, out MyStringId _, out MyStringId _);
        return "[" + textEvaluator.TokenEvaluate(token, context) + "]";
      }
    }
  }
}
