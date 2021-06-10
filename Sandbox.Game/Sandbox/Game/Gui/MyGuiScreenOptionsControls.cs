// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenOptionsControls
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenOptionsControls : MyGuiScreenBase
  {
    private MyGuiControlTypeEnum m_currentControlType;
    private MyGuiControlCombobox m_controlTypeList;
    private Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>> m_allControls = new Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>>();
    private List<MyGuiControlButton> m_key1Buttons;
    private List<MyGuiControlButton> m_key2Buttons;
    private List<MyGuiControlButton> m_mouseButtons;
    private List<MyGuiControlButton> m_joystickButtons;
    private List<MyGuiControlButton> m_joystickAxes;
    private MyGuiControlCheckbox m_invertMouseXCheckbox;
    private MyGuiControlCheckbox m_invertMouseYCheckbox;
    private MyGuiControlCheckbox m_InvertMouseScrollBlockSelectionCheckbox;
    private MyGuiControlSlider m_mouseSensitivitySlider;
    private MyGuiControlSlider m_joystickSensitivitySlider;
    private MyGuiControlSlider m_zoomMultiplierSlider;
    private MyGuiControlSlider m_joystickDeadzoneSlider;
    private MyGuiControlSlider m_joystickExponentSlider;
    private MyGuiControlCombobox m_joystickCombobox;
    private MyGuiControlCombobox m_gamepadScheme;
    private MyGuiControlCheckbox m_invertYCharCheckbox;
    private MyGuiControlCheckbox m_invertYVehicleCheckbox;
    private Vector2 m_controlsOriginLeft;
    private Vector2 m_controlsOriginRight;
    private MyGuiControlElementGroup m_elementGroup;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;

    public MyGuiScreenOptionsControls()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.9465649f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      this.AddCaption(MyCommonTexts.ScreenCaptionControls, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyInput.Static.TakeSnapshot();
      Vector2 vector2_1 = this.m_size.Value * new Vector2(0.0f, -0.5f);
      Vector2 vector2_2 = this.m_size.Value * new Vector2(0.0f, 0.5f);
      Vector2 vector2_3 = this.m_size.Value * -0.5f;
      this.m_controlsOriginLeft = (this.m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(-1f, -1f) + new Vector2(0.0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
      this.m_controlsOriginRight = (this.m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(1f, -1f) + new Vector2(0.0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.143999993801117)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
      Vector2 vector2_4 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_5 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float num1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x = 25f;
      float screenCaptionDeltaY = MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
      float num2 = 0.0015f;
      Vector2 vector2_6 = new Vector2(0.0f, 0.045f);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_4) * new Vector2(-1f, -1f) + new Vector2(0.0f, screenCaptionDeltaY);
      Vector2 vector2_8 = (this.m_size.Value / 2f - vector2_4) * new Vector2(1f, -1f) + new Vector2(0.0f, screenCaptionDeltaY);
      Vector2 vector2_9 = (this.m_size.Value / 2f - vector2_5) * new Vector2(0.0f, 1f);
      Vector2 vector2_10 = new Vector2(vector2_8.X - (num1 + num2), vector2_8.Y);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      this.m_okButton = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_okButton.Position = vector2_9 + new Vector2(-x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_cancelButton.Position = vector2_9 + new Vector2(x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      Vector2? position = new Vector2?();
      Vector2? size = new Vector2?(MyGuiConstants.MESSAGE_BOX_BUTTON_SIZE_SMALL);
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.Revert);
      string toolTip = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Defaults);
      StringBuilder text = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = new Action<MyGuiControlButton>(this.OnResetDefaultsClick);
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ComboBoxButton, size, colorMask, toolTip: toolTip, text: text, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      guiControlButton.Position = new Vector2(0.0f, 0.0f) - new Vector2((float) (0.0 - (double) this.m_size.Value.X * 0.832000017166138 / 2.0 + (double) guiControlButton.Size.X / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.112999998033047));
      guiControlButton.TextScale = 0.7f;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_cancelButton);
      this.Controls.Add((MyGuiControlBase) guiControlButton);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton);
      this.m_currentControlType = MyGuiControlTypeEnum.General;
      this.m_controlTypeList = new MyGuiControlCombobox(new Vector2?(new Vector2((float) (0.0 - (double) guiControlButton.Size.X / 2.0 - 0.00899999961256981), 0.0f) - new Vector2(0.0f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.109999999403954))));
      this.m_controlTypeList.Size = new Vector2(this.m_size.Value.X * 0.595f, 1f);
      this.m_controlTypeList.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Category));
      this.m_controlTypeList.AddItem(0L, MyCommonTexts.ControlTypeGeneral);
      this.m_controlTypeList.AddItem(1L, MyCommonTexts.ControlTypeNavigation);
      this.m_controlTypeList.AddItem(5L, MyCommonTexts.ControlTypeSystems1);
      this.m_controlTypeList.AddItem(6L, MyCommonTexts.ControlTypeSystems2);
      this.m_controlTypeList.AddItem(7L, MyCommonTexts.ControlTypeSystems3);
      this.m_controlTypeList.AddItem(3L, MyCommonTexts.ControlTypeToolsOrWeapons);
      this.m_controlTypeList.AddItem(8L, MyCommonTexts.ControlTypeView);
      this.m_controlTypeList.SelectItemByKey((long) this.m_currentControlType);
      this.Controls.Add((MyGuiControlBase) this.m_controlTypeList);
      this.AddControls();
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(vector2_7.X, this.m_okButton.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.ActivateControls(this.m_currentControlType);
      this.FocusedControl = (MyGuiControlBase) this.m_okButton;
      this.CloseButtonEnabled = true;
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ControlsOptions_Help_Screen);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (this.FocusedControl != this.m_gamepadScheme || !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
        return;
      switch (this.m_gamepadScheme.GetSelectedKey())
      {
        case 0:
          MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenGamepadBindingsHelp(ControlScheme.Default));
          break;
        case 1:
          MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenGamepadBindingsHelp(ControlScheme.Alternative));
          break;
      }
    }

    private void AddControls()
    {
      this.m_key1Buttons = new List<MyGuiControlButton>();
      this.m_key2Buttons = new List<MyGuiControlButton>();
      this.m_mouseButtons = new List<MyGuiControlButton>();
      if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
      {
        this.m_joystickButtons = new List<MyGuiControlButton>();
        this.m_joystickAxes = new List<MyGuiControlButton>();
      }
      this.AddControlsByType(MyGuiControlTypeEnum.General);
      this.AddControlsByType(MyGuiControlTypeEnum.Navigation);
      this.AddControlsByType(MyGuiControlTypeEnum.Systems1);
      this.AddControlsByType(MyGuiControlTypeEnum.Systems2);
      this.AddControlsByType(MyGuiControlTypeEnum.Systems3);
      this.AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
      this.AddControlsByType(MyGuiControlTypeEnum.Spectator);
      foreach (KeyValuePair<MyGuiControlTypeEnum, List<MyGuiControlBase>> allControl in this.m_allControls)
      {
        foreach (MyGuiControlBase control in allControl.Value)
          this.Controls.Add(control);
        this.DeactivateControls(allControl.Key);
      }
      if (!MyFakes.ENABLE_JOYSTICK_SETTINGS)
        return;
      this.RefreshJoystickControlEnabling();
    }

    private MyGuiControlLabel MakeLabel(
      float deltaMultip,
      MyStringId textEnum,
      bool isAutoScaleEnabled = false,
      float maxWidth = float.PositiveInfinity,
      bool isAutoEllipsisEnabled = false)
    {
      Vector2? position = new Vector2?(this.m_controlsOriginLeft + deltaMultip * MyGuiConstants.CONTROLS_DELTA);
      Vector2? size = new Vector2?();
      string text = MyTexts.GetString(textEnum);
      Vector4? colorMask = new Vector4?();
      bool flag = isAutoScaleEnabled;
      int num1 = isAutoEllipsisEnabled ? 1 : 0;
      double num2 = (double) maxWidth;
      int num3 = flag ? 1 : 0;
      return new MyGuiControlLabel(position, size, text, colorMask, isAutoEllipsisEnabled: (num1 != 0), maxWidth: ((float) num2), isAutoScaleEnabled: (num3 != 0));
    }

    private MyGuiControlLabel MakeLabel(MyStringId textEnum, Vector2 position) => new MyGuiControlLabel(new Vector2?(position), text: MyTexts.GetString(textEnum), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);

    private MyGuiControlButton MakeControlButton(
      MyControl control,
      Vector2 position,
      MyGuiInputDeviceEnum device)
    {
      StringBuilder output = (StringBuilder) null;
      control.AppendBoundButtonNames(ref output, device);
      MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(position), MyGuiControlButtonStyleEnum.ControlSetting, text: output, onButtonClick: new Action<MyGuiControlButton>(this.OnControlClick), onSecondaryButtonClick: new Action<MyGuiControlButton>(this.OnSecondaryControlClick));
      guiControlButton.UserData = (object) new MyGuiScreenOptionsControls.ControlButtonData(control, device);
      return guiControlButton;
    }

    private void AddControlsByType(MyGuiControlTypeEnum type)
    {
      if (type == MyGuiControlTypeEnum.General)
      {
        this.AddGeneralControls();
      }
      else
      {
        MyGuiControlButton.StyleDefinition visualStyle = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ControlSetting);
        Vector2 controlsOriginRight = this.m_controlsOriginRight;
        controlsOriginRight.X -= 0.02f;
        controlsOriginRight.Y -= 0.01f;
        this.m_allControls[type] = new List<MyGuiControlBase>();
        float num1 = 2f;
        float num2 = 0.85f;
        DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
        MyGuiControlLabel myGuiControlLabel1 = this.MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard1, Vector2.Zero);
        MyGuiControlLabel myGuiControlLabel2 = this.MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard2, Vector2.Zero);
        MyGuiControlLabel myGuiControlLabel3 = this.MakeLabel(MyCommonTexts.ScreenOptionsControls_Mouse, Vector2.Zero);
        if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
          this.MakeLabel(MyCommonTexts.ScreenOptionsControls_Gamepad, Vector2.Zero);
        if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
          this.MakeLabel(MyCommonTexts.ScreenOptionsControls_AnalogAxes, Vector2.Zero);
        float num3 = 1.1f * Math.Max(Math.Max(myGuiControlLabel1.Size.X, myGuiControlLabel2.Size.X), Math.Max(myGuiControlLabel3.Size.X, visualStyle.SizeOverride.Value.X));
        Vector2 position = (num1 - 1f) * MyGuiConstants.CONTROLS_DELTA + controlsOriginRight;
        position.X += (float) ((double) num3 * 0.5 - 0.264999985694885);
        position.Y -= 0.015f;
        myGuiControlLabel1.Position = position;
        position.X += num3;
        myGuiControlLabel2.Position = position;
        position.X += num3;
        myGuiControlLabel3.Position = position;
        this.m_allControls[type].Add((MyGuiControlBase) myGuiControlLabel1);
        this.m_allControls[type].Add((MyGuiControlBase) myGuiControlLabel2);
        this.m_allControls[type].Add((MyGuiControlBase) myGuiControlLabel3);
        int num4 = MyFakes.ENABLE_JOYSTICK_SETTINGS ? 1 : 0;
        foreach (MyControl control in gameControlsList)
        {
          if (control.GetControlTypeEnum() == type)
          {
            this.m_allControls[type].Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_controlsOriginLeft + num1 * MyGuiConstants.CONTROLS_DELTA - new Vector2(0.0f, 0.03f)), text: MyTexts.GetString(control.GetControlName()), isAutoEllipsisEnabled: true, maxWidth: 0.24f, isAutoScaleEnabled: true));
            position = controlsOriginRight + num1 * MyGuiConstants.CONTROLS_DELTA - new Vector2(0.265f, 0.015f);
            position.X += num3 * 0.5f;
            MyGuiControlButton guiControlButton1 = this.MakeControlButton(control, position, MyGuiInputDeviceEnum.Keyboard);
            guiControlButton1.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
            guiControlButton1.IsAutoEllipsisEnabled = true;
            guiControlButton1.IsAutoScaleEnabled = true;
            this.m_allControls[type].Add((MyGuiControlBase) guiControlButton1);
            this.m_key1Buttons.Add(guiControlButton1);
            position.X += num3;
            MyGuiControlButton guiControlButton2 = this.MakeControlButton(control, position, MyGuiInputDeviceEnum.KeyboardSecond);
            guiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
            guiControlButton2.IsAutoEllipsisEnabled = true;
            guiControlButton2.IsAutoScaleEnabled = true;
            this.m_allControls[type].Add((MyGuiControlBase) guiControlButton2);
            this.m_key2Buttons.Add(guiControlButton2);
            position.X += num3;
            MyGuiControlButton guiControlButton3 = this.MakeControlButton(control, position, MyGuiInputDeviceEnum.Mouse);
            guiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
            guiControlButton3.IsAutoEllipsisEnabled = true;
            guiControlButton3.IsAutoScaleEnabled = true;
            this.m_allControls[type].Add((MyGuiControlBase) guiControlButton3);
            this.m_mouseButtons.Add(guiControlButton3);
            position.X += num3;
            int num5 = MyFakes.ENABLE_JOYSTICK_SETTINGS ? 1 : 0;
            num1 += num2;
          }
        }
      }
    }

    private void AddGeneralControls()
    {
      float length = this.m_size.Value.X * 0.83f;
      float deltaMultip1 = 1.7f;
      this.m_controlsOriginRight.Y -= 0.025f;
      this.m_controlsOriginLeft.Y -= 0.025f;
      this.m_allControls[MyGuiControlTypeEnum.General] = new List<MyGuiControlBase>();
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip1, MySpaceTexts.ZoomMultiplier));
      Vector2? position1 = new Vector2?(this.m_controlsOriginRight + deltaMultip1 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f));
      string str1 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ZoomMultiplier);
      float? defaultValue1 = new float?(MySandboxGame.Config.ZoomMultiplier);
      Vector4? color1 = new Vector4?();
      string toolTip1 = str1;
      this.m_zoomMultiplierSlider = new MyGuiControlSlider(position1, defaultValue: defaultValue1, color: color1, toolTip: toolTip1);
      this.m_zoomMultiplierSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
      this.m_zoomMultiplierSlider.Value = MySandboxGame.Config.ZoomMultiplier;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_zoomMultiplierSlider);
      float num1 = deltaMultip1 + 0.585f;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(num1 * MyGuiConstants.CONTROLS_DELTA + new Vector2((float) (-(double) length / 2.0), this.m_controlsOriginRight.Y), length);
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) controlSeparatorList1);
      float deltaMultip2 = num1 + 0.585f;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip2, MyCommonTexts.MouseSensitivity));
      this.m_mouseSensitivitySlider = new MyGuiControlSlider(new Vector2?(this.m_controlsOriginRight + deltaMultip2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f)), 0.1f, 3f, defaultValue: new float?(MyInput.Static.GetMouseSensitivity()), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_MouseSensitivity), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_mouseSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
      this.m_mouseSensitivitySlider.Value = MyInput.Static.GetMouseSensitivity();
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_mouseSensitivitySlider);
      float deltaMultip3 = deltaMultip2 + 0.97f;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip3, MyCommonTexts.InvertMouseX));
      Vector2? position2 = new Vector2?(this.m_controlsOriginRight + deltaMultip3 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f));
      Vector4? color2 = new Vector4?();
      bool mouseXinversion = MyInput.Static.GetMouseXInversion();
      string toolTip2 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseX);
      int num2 = mouseXinversion ? 1 : 0;
      this.m_invertMouseXCheckbox = new MyGuiControlCheckbox(position2, color2, toolTip2, num2 != 0, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_invertMouseXCheckbox);
      float deltaMultip4 = deltaMultip3 + 0.85f;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip4, MyCommonTexts.InvertMouseY));
      Vector2? position3 = new Vector2?(this.m_controlsOriginRight + deltaMultip4 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f));
      Vector4? color3 = new Vector4?();
      bool mouseYinversion = MyInput.Static.GetMouseYInversion();
      string toolTip3 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseY);
      int num3 = mouseYinversion ? 1 : 0;
      this.m_invertMouseYCheckbox = new MyGuiControlCheckbox(position3, color3, toolTip3, num3 != 0, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_invertMouseYCheckbox);
      float deltaMultip5 = deltaMultip4 + 0.85f;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip5, MyCommonTexts.InvertMouseScrollBlockSelection, true, 0.25f, true));
      Vector2? position4 = new Vector2?(this.m_controlsOriginRight + deltaMultip5 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f));
      Vector4? color4 = new Vector4?();
      bool selectionInversion = MyInput.Static.GetMouseScrollBlockSelectionInversion();
      string toolTip4 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseScrollBlockSelection);
      int num4 = selectionInversion ? 1 : 0;
      this.m_InvertMouseScrollBlockSelectionCheckbox = new MyGuiControlCheckbox(position4, color4, toolTip4, num4 != 0, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_InvertMouseScrollBlockSelectionCheckbox);
      float num5 = deltaMultip5 + 0.585f;
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(num5 * MyGuiConstants.CONTROLS_DELTA + new Vector2((float) (-(double) length / 2.0), this.m_controlsOriginRight.Y), length);
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) controlSeparatorList2);
      float deltaMultip6 = num5 + 0.585f;
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip6, MyCommonTexts.Joystick));
        this.m_joystickCombobox = new MyGuiControlCombobox(new Vector2?(this.m_controlsOriginRight + deltaMultip6 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f)));
        this.m_joystickCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickOrGamepad));
        this.m_joystickCombobox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnSelectJoystick);
        this.AddJoysticksToComboBox();
        this.m_joystickCombobox.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
        this.m_joystickCombobox.Enabled = !MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected();
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_joystickCombobox);
      }
      float deltaMultip7 = deltaMultip6 + 0.97f;
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip7, MyCommonTexts.GamepadScheme));
      this.m_gamepadScheme = new MyGuiControlCombobox(new Vector2?(this.m_controlsOriginRight + deltaMultip7 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f)));
      this.m_gamepadScheme.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipOptionsControls_GamepadScheme));
      this.m_gamepadScheme.AddItem(0L, MyTexts.Get(MyCommonTexts.GamepadScheme_Default));
      this.m_gamepadScheme.AddItem(1L, MyTexts.Get(MyCommonTexts.GamepadScheme_1));
      this.m_gamepadScheme.SelectItemByKey((long) MySandboxGame.Config.GamepadSchemeId);
      this.m_gamepadScheme.GamepadHelpTextId = MySpaceTexts.ControlsOptions_Help_Scheme;
      this.m_gamepadScheme.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
      this.m_gamepadScheme.Enabled = !MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected();
      this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_gamepadScheme);
      float deltaMultip8 = deltaMultip7 + 0.97f;
      if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
      {
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip8, MyCommonTexts.JoystickSensitivity));
        Vector2? position5 = new Vector2?(this.m_controlsOriginRight + deltaMultip8 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f));
        string str2 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickSensitivity);
        float? defaultValue2 = new float?(MyInput.Static.GetJoystickSensitivity());
        Vector4? color5 = new Vector4?();
        string toolTip5 = str2;
        this.m_joystickSensitivitySlider = new MyGuiControlSlider(position5, 0.1f, 6f, defaultValue: defaultValue2, color: color5, toolTip: toolTip5);
        this.m_joystickSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
        this.m_joystickSensitivitySlider.Value = MyInput.Static.GetJoystickSensitivity();
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_joystickSensitivitySlider);
        float deltaMultip9 = deltaMultip8 + 0.97f;
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip9, MyCommonTexts.JoystickExponent));
        Vector2? position6 = new Vector2?(this.m_controlsOriginRight + deltaMultip9 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f));
        string str3 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickGradualPrecision);
        float? defaultValue3 = new float?(MyInput.Static.GetJoystickExponent());
        Vector4? color6 = new Vector4?();
        string toolTip6 = str3;
        this.m_joystickExponentSlider = new MyGuiControlSlider(position6, 1f, 8f, defaultValue: defaultValue3, color: color6, toolTip: toolTip6);
        this.m_joystickExponentSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
        this.m_joystickExponentSlider.Value = MyInput.Static.GetJoystickExponent();
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_joystickExponentSlider);
        float deltaMultip10 = deltaMultip9 + 0.97f;
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip10, MyCommonTexts.JoystickDeadzone));
        Vector2? position7 = new Vector2?(this.m_controlsOriginRight + deltaMultip10 * MyGuiConstants.CONTROLS_DELTA - new Vector2((float) (455.0 / (double) MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2.0), 0.0f));
        string str4 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickDeadzoneWidth);
        float? defaultValue4 = new float?(MyInput.Static.GetJoystickDeadzone());
        Vector4? color7 = new Vector4?();
        string toolTip7 = str4;
        this.m_joystickDeadzoneSlider = new MyGuiControlSlider(position7, maxValue: 0.5f, defaultValue: defaultValue4, color: color7, toolTip: toolTip7);
        this.m_joystickDeadzoneSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f);
        this.m_joystickDeadzoneSlider.Value = MyInput.Static.GetJoystickDeadzone();
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_joystickDeadzoneSlider);
        float deltaMultip11 = deltaMultip10 + 0.85f;
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip11, MyCommonTexts.InvertGamepadYChar));
        Vector2? position8 = new Vector2?(this.m_controlsOriginRight + deltaMultip11 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f));
        Vector4? color8 = new Vector4?();
        bool yinversionCharacter = MyInput.Static.GetJoystickYInversionCharacter();
        string toolTip8 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertGamepadYChar);
        int num6 = yinversionCharacter ? 1 : 0;
        this.m_invertYCharCheckbox = new MyGuiControlCheckbox(position8, color8, toolTip8, num6 != 0, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_invertYCharCheckbox);
        deltaMultip8 = deltaMultip11 + 0.85f;
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.MakeLabel(deltaMultip8, MyCommonTexts.InvertGamepadYVehicle, true, 0.24f));
        Vector2? position9 = new Vector2?(this.m_controlsOriginRight + deltaMultip8 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.0f));
        Vector4? color9 = new Vector4?();
        bool yinversionVehicle = MyInput.Static.GetJoystickYInversionVehicle();
        string toolTip9 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertGamepadYVehicle);
        int num7 = yinversionVehicle ? 1 : 0;
        this.m_invertYVehicleCheckbox = new MyGuiControlCheckbox(position9, color9, toolTip9, num7 != 0, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        this.m_allControls[MyGuiControlTypeEnum.General].Add((MyGuiControlBase) this.m_invertYVehicleCheckbox);
      }
      float num8 = deltaMultip8 + 0.85f;
      this.m_controlsOriginRight.Y += 0.025f;
      this.m_controlsOriginLeft.Y += 0.025f;
    }

    private void DeactivateControls(MyGuiControlTypeEnum type)
    {
      foreach (MyGuiControlBase myGuiControlBase in this.m_allControls[type])
        myGuiControlBase.Visible = false;
    }

    private void ActivateControls(MyGuiControlTypeEnum type)
    {
      foreach (MyGuiControlBase myGuiControlBase in this.m_allControls[type])
        myGuiControlBase.Visible = true;
    }

    private void AddJoysticksToComboBox()
    {
      if (this.m_joystickCombobox == null)
        return;
      int num1 = 0;
      bool flag = false;
      MyGuiControlCombobox joystickCombobox = this.m_joystickCombobox;
      int num2 = num1;
      int index = num2 + 1;
      long key = (long) num2;
      StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.Disabled);
      int? sortOrder = new int?();
      joystickCombobox.AddItem(key, stringBuilder, sortOrder);
      foreach (string enumerateJoystickName in MyInput.Static.EnumerateJoystickNames())
      {
        this.m_joystickCombobox.AddItem((long) index, new StringBuilder(enumerateJoystickName));
        if (MyInput.Static.JoystickInstanceName == enumerateJoystickName)
        {
          flag = true;
          this.m_joystickCombobox.SelectItemByIndex(index);
        }
        ++index;
      }
      if (flag)
        return;
      this.m_joystickCombobox.SelectItemByIndex(0);
    }

    private void OnSelectJoystick()
    {
      if (this.m_joystickCombobox == null)
        return;
      MyInput.Static.JoystickInstanceName = this.m_joystickCombobox.GetSelectedIndex() == 0 ? (string) null : this.m_joystickCombobox.GetSelectedValue().ToString();
      this.RefreshJoystickControlEnabling();
    }

    private void RefreshJoystickControlEnabling()
    {
      if (this.m_joystickCombobox == null)
        return;
      bool flag = (uint) this.m_joystickCombobox.GetSelectedIndex() > 0U;
      foreach (MyGuiControlBase joystickButton in this.m_joystickButtons)
        joystickButton.Enabled = flag;
      foreach (MyGuiControlBase joystickAx in this.m_joystickAxes)
        joystickAx.Enabled = flag;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenOptionsControls);

    public override bool Update(bool hasFocus)
    {
      if (this.m_controlTypeList.GetSelectedKey() != (long) this.m_currentControlType)
      {
        this.DeactivateControls(this.m_currentControlType);
        this.m_currentControlType = (MyGuiControlTypeEnum) this.m_controlTypeList.GetSelectedKey();
        this.ActivateControls(this.m_currentControlType);
      }
      if (hasFocus)
      {
        if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
          this.OnOkClick((MyGuiControlButton) null);
        this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      }
      return base.Update(hasFocus);
    }

    private void OnControlClick(MyGuiControlButton button)
    {
      MyGuiScreenOptionsControls.ControlButtonData userData = (MyGuiScreenOptionsControls.ControlButtonData) button.UserData;
      MyStringId messageText = MyCommonTexts.AssignControlKeyboard;
      if (userData.Device == MyGuiInputDeviceEnum.Mouse)
        messageText = MyCommonTexts.AssignControlMouse;
      MyGuiScreenOptionsControls.MyGuiControlAssignKeyMessageBox assignKeyMessageBox = new MyGuiScreenOptionsControls.MyGuiControlAssignKeyMessageBox(userData.Device, userData.Control, messageText);
      assignKeyMessageBox.Closed += (MyGuiScreenBase.ScreenHandler) ((s, isUnloading) => this.RefreshButtonTexts());
      MyGuiSandbox.AddScreen((MyGuiScreenBase) assignKeyMessageBox);
    }

    private void OnSecondaryControlClick(MyGuiControlButton button)
    {
      MyGuiScreenOptionsControls.ControlButtonData data = (MyGuiScreenOptionsControls.ControlButtonData) button.UserData;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextRemoveControlBinding), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        if (data.Device == MyGuiInputDeviceEnum.Mouse)
          data.Control.SetControl(MyMouseButtonsEnum.None);
        else if (data.Device == MyGuiInputDeviceEnum.Keyboard || data.Device == MyGuiInputDeviceEnum.KeyboardSecond)
          data.Control.SetControl(data.Device, MyKeys.None);
        this.RefreshButtonTexts();
      }))));
    }

    private void OnResetDefaultsClick(MyGuiControlButton sender)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionResetControlsToDefault);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextResetControlsToDefault), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (res =>
      {
        if (res != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyInput.Static.RevertToDefaultControls();
        this.DeactivateControls(this.m_currentControlType);
        this.AddControls();
        this.ActivateControls(this.m_currentControlType);
      }))));
    }

    protected override void Canceling()
    {
      MyInput.Static.RevertChanges();
      base.Canceling();
    }

    private void OnCancelClick(MyGuiControlButton sender)
    {
      MyInput.Static.RevertChanges();
      this.CloseScreen();
    }

    private void OnOkClick(MyGuiControlButton sender) => this.CloseScreenAndSave();

    private void CloseScreenAndSave()
    {
      MyInput.Static.SetMouseXInversion(this.m_invertMouseXCheckbox.IsChecked);
      MyInput.Static.SetMouseYInversion(this.m_invertMouseYCheckbox.IsChecked);
      MyInput.Static.SetMouseScrollBlockSelectionInversion(this.m_InvertMouseScrollBlockSelectionCheckbox.IsChecked);
      MyInput.Static.SetMouseSensitivity(this.m_mouseSensitivitySlider.Value);
      MySandboxGame.Config.ZoomMultiplier = this.m_zoomMultiplierSlider.Value;
      if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
      {
        if (this.m_joystickCombobox != null)
          MyInput.Static.JoystickInstanceName = this.m_joystickCombobox.GetSelectedIndex() == 0 ? (string) null : this.m_joystickCombobox.GetSelectedValue().ToString();
        MyInput.Static.SetJoystickSensitivity(this.m_joystickSensitivitySlider.Value);
        MyInput.Static.SetJoystickExponent(this.m_joystickExponentSlider.Value);
        MyInput.Static.SetJoystickDeadzone(this.m_joystickDeadzoneSlider.Value);
        MyInput.Static.UpdateJoystickChanged();
        MySandboxGame.Config.GamepadSchemeId = (int) this.m_gamepadScheme.GetSelectedKey();
        MyInput.Static.SetJoystickYInversionCharacter(this.m_invertYCharCheckbox.IsChecked);
        MyInput.Static.SetJoystickYInversionVehicle(this.m_invertYVehicleCheckbox.IsChecked);
        MySpaceBindingCreator.CreateBindingDefault();
      }
      MyInput.Static.SaveControls(MySandboxGame.Config.ControlsGeneral, MySandboxGame.Config.ControlsButtons);
      MySandboxGame.Config.Save();
      MyScreenManager.RecreateControls();
      this.CloseScreen();
    }

    private void RefreshButtonTexts()
    {
      this.RefreshButtonTexts(this.m_key1Buttons);
      this.RefreshButtonTexts(this.m_key2Buttons);
      this.RefreshButtonTexts(this.m_mouseButtons);
      if (!MyFakes.ENABLE_JOYSTICK_SETTINGS)
        return;
      this.RefreshButtonTexts(this.m_joystickButtons);
      this.RefreshButtonTexts(this.m_joystickAxes);
    }

    private void RefreshButtonTexts(List<MyGuiControlButton> buttons)
    {
      StringBuilder output = (StringBuilder) null;
      foreach (MyGuiControlButton button in buttons)
      {
        MyGuiScreenOptionsControls.ControlButtonData userData = (MyGuiScreenOptionsControls.ControlButtonData) button.UserData;
        userData.Control.AppendBoundButtonNames(ref output, userData.Device);
        MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
        button.Text = output.ToString();
        output.Clear();
      }
    }

    private class ControlButtonData
    {
      public readonly MyControl Control;
      public readonly MyGuiInputDeviceEnum Device;

      public ControlButtonData(MyControl control, MyGuiInputDeviceEnum device)
      {
        this.Control = control;
        this.Device = device;
      }
    }

    private class MyGuiControlAssignKeyMessageBox : MyGuiScreenMessageBox
    {
      private MyControl m_controlBeingSet;
      private MyGuiInputDeviceEnum m_deviceType;
      private List<MyKeys> m_newPressedKeys = new List<MyKeys>();
      private List<MyMouseButtonsEnum> m_newPressedMouseButtons = new List<MyMouseButtonsEnum>();
      private List<MyJoystickButtonsEnum> m_newPressedJoystickButtons = new List<MyJoystickButtonsEnum>();
      private List<MyJoystickAxesEnum> m_newPressedJoystickAxes = new List<MyJoystickAxesEnum>();
      private List<MyKeys> m_oldPressedKeys = new List<MyKeys>();
      private List<MyMouseButtonsEnum> m_oldPressedMouseButtons = new List<MyMouseButtonsEnum>();
      private List<MyJoystickButtonsEnum> m_oldPressedJoystickButtons = new List<MyJoystickButtonsEnum>();
      private List<MyJoystickAxesEnum> m_oldPressedJoystickAxes = new List<MyJoystickAxesEnum>();

      public MyGuiControlAssignKeyMessageBox(
        MyGuiInputDeviceEnum deviceType,
        MyControl controlBeingSet,
        MyStringId messageText)
        : base(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE, MyTexts.Get(messageText), MyTexts.Get(MyCommonTexts.SelectControl), new MyStringId(), new MyStringId(), new MyStringId(), new MyStringId(), (Action<MyGuiScreenMessageBox.ResultEnum>) null, 0, MyGuiScreenMessageBox.ResultEnum.YES, true, new Vector2?())
      {
        this.DrawMouseCursor = false;
        this.m_isTopMostScreen = false;
        this.m_controlBeingSet = controlBeingSet;
        this.m_deviceType = deviceType;
        MyInput.Static.GetListOfPressedKeys(this.m_oldPressedKeys);
        MyInput.Static.GetListOfPressedMouseButtons(this.m_oldPressedMouseButtons);
        this.m_closeOnEsc = false;
        this.CanBeHidden = true;
      }

      public override void HandleInput(bool receivedFocusInThisUpdate)
      {
        base.HandleInput(receivedFocusInThisUpdate);
        if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))
          this.Canceling();
        if (this.State == MyGuiScreenState.CLOSING || this.State == MyGuiScreenState.HIDING)
          return;
        switch (this.m_deviceType)
        {
          case MyGuiInputDeviceEnum.Keyboard:
          case MyGuiInputDeviceEnum.KeyboardSecond:
            this.HandleKey();
            break;
          case MyGuiInputDeviceEnum.Mouse:
            this.HandleMouseButton();
            break;
        }
      }

      private void HandleKey()
      {
        this.ReadPressedKeys();
        foreach (MyKeys newPressedKey in this.m_newPressedKeys)
        {
          MyKeys key = newPressedKey;
          if (!this.m_oldPressedKeys.Contains(key))
          {
            if (!MyInput.Static.IsKeyValid(key))
            {
              this.ShowControlIsNotValidMessageBox();
              break;
            }
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            MyControl ctrl = MyInput.Static.GetControl(key);
            if (ctrl != null)
            {
              if (ctrl.Equals((object) this.m_controlBeingSet))
              {
                this.OverwriteAssignment(ctrl, key);
                this.CloseScreen(false);
                break;
              }
              StringBuilder output = (StringBuilder) null;
              MyControl.AppendName(ref output, key);
              this.ShowControlIsAlreadyAssigned(ctrl, output, (Action) (() => this.AnywayAssignment(ctrl, key)));
              break;
            }
            this.m_controlBeingSet.SetControl(this.m_deviceType, key);
            this.CloseScreen(false);
            break;
          }
        }
        this.m_oldPressedKeys.Clear();
        MyUtils.Swap<List<MyKeys>>(ref this.m_oldPressedKeys, ref this.m_newPressedKeys);
      }

      private void HandleMouseButton()
      {
        MyInput.Static.GetListOfPressedMouseButtons(this.m_newPressedMouseButtons);
        foreach (MyMouseButtonsEnum pressedMouseButton in this.m_newPressedMouseButtons)
        {
          MyMouseButtonsEnum button = pressedMouseButton;
          if (!this.m_oldPressedMouseButtons.Contains(button))
          {
            MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
            if (!MyInput.Static.IsMouseButtonValid(button))
            {
              this.ShowControlIsNotValidMessageBox();
              break;
            }
            MyControl ctrl = MyInput.Static.GetControl(button);
            if (ctrl != null)
            {
              if (ctrl.Equals((object) this.m_controlBeingSet))
              {
                this.OverwriteAssignment(ctrl, button);
                this.CloseScreen(false);
                break;
              }
              StringBuilder output = (StringBuilder) null;
              MyControl.AppendName(ref output, button);
              this.ShowControlIsAlreadyAssigned(ctrl, output, (Action) (() => this.AnywayAssignment(ctrl, button)));
              break;
            }
            this.m_controlBeingSet.SetControl(button);
            this.CloseScreen(false);
            break;
          }
        }
        this.m_oldPressedMouseButtons.Clear();
        MyUtils.Swap<List<MyMouseButtonsEnum>>(ref this.m_oldPressedMouseButtons, ref this.m_newPressedMouseButtons);
      }

      private void ReadPressedKeys()
      {
        MyInput.Static.GetListOfPressedKeys(this.m_newPressedKeys);
        this.m_newPressedKeys.Remove(MyKeys.Control);
        this.m_newPressedKeys.Remove(MyKeys.Shift);
        this.m_newPressedKeys.Remove(MyKeys.Alt);
        if (!this.m_newPressedKeys.Contains(MyKeys.LeftControl) || !this.m_newPressedKeys.Contains(MyKeys.RightAlt))
          return;
        this.m_newPressedKeys.Remove(MyKeys.LeftControl);
      }

      private void ShowControlIsNotValidMessageBox() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.ControlIsNotValid), messageCaption: MyTexts.Get(MyCommonTexts.CanNotAssignControl)));

      private void ShowControlIsAlreadyAssigned(
        MyControl controlAlreadySet,
        StringBuilder controlButtonName,
        Action overwriteAssignmentCallback)
      {
        MyGuiScreenMessageBox screenMessageBox = this.MakeControlIsAlreadyAssignedDialog(controlAlreadySet, controlButtonName);
        screenMessageBox.ResultCallback = (Action<MyGuiScreenMessageBox.ResultEnum>) (r =>
        {
          if (r == MyGuiScreenMessageBox.ResultEnum.YES)
          {
            overwriteAssignmentCallback();
            this.CloseScreen(false);
          }
          else
          {
            MyInput.Static.GetListOfPressedKeys(this.m_oldPressedKeys);
            MyInput.Static.GetListOfPressedMouseButtons(this.m_oldPressedMouseButtons);
          }
        });
        MyGuiSandbox.AddScreen((MyGuiScreenBase) screenMessageBox);
      }

      private MyGuiScreenMessageBox MakeControlIsAlreadyAssignedDialog(
        MyControl controlAlreadySet,
        StringBuilder controlButtonName)
      {
        return MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ControlAlreadyAssigned), (object) controlButtonName, (object) MyTexts.Get(controlAlreadySet.GetControlName()))), messageCaption: MyTexts.Get(MyCommonTexts.CanNotAssignControl));
      }

      private void OverwriteAssignment(MyControl controlAlreadySet, MyKeys key)
      {
        if (controlAlreadySet.GetKeyboardControl() == key)
          controlAlreadySet.SetControl(MyGuiInputDeviceEnum.Keyboard, MyKeys.None);
        else
          controlAlreadySet.SetControl(MyGuiInputDeviceEnum.KeyboardSecond, MyKeys.None);
        this.m_controlBeingSet.SetControl(this.m_deviceType, key);
      }

      private void AnywayAssignment(MyControl controlAlreadySet, MyKeys key) => this.m_controlBeingSet.SetControl(this.m_deviceType, key);

      private void OverwriteAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button)
      {
        controlAlreadySet.SetControl(MyMouseButtonsEnum.None);
        this.m_controlBeingSet.SetControl(button);
      }

      private void AnywayAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button) => this.m_controlBeingSet.SetControl(button);

      public override bool CloseScreen(bool isUnloading = false)
      {
        this.DrawMouseCursor = true;
        return base.CloseScreen(isUnloading);
      }
    }
  }
}
