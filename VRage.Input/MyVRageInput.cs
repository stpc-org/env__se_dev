// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyVRageInput
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Collections;
using VRage.Input.Joystick;
using VRage.Input.Keyboard;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
  public class MyVRageInput : VRage.ModAPI.IMyInput, IMyInput, ITextEvaluator
  {
    private bool m_overrideUpdate;
    private Vector2 m_absoluteMousePosition;
    private MyMouseState m_previousMouseState;
    private MyJoystickState m_previousJoystickState;
    private MyGuiLocalizedKeyboardState m_keyboardState;
    private MyMouseState m_actualMouseState;
    private MyMouseState m_actualMouseStateRaw;
    private MyJoystickState m_actualJoystickState;
    private bool m_mouseXIsInverted;
    private bool m_mouseYIsInverted;
    private bool m_mouseScrollBlockSelectionInverted;
    private bool m_joystickYInvertedChar;
    private bool m_joystickYInvertedVehicle;
    private float m_mousePositionScale = 1f;
    private float m_mouseSensitivity;
    private string m_joystickInstanceName;
    private float m_joystickSensitivity;
    private float m_joystickDeadzone;
    private float m_joystickExponent;
    private const bool IS_MOUSE_X_INVERTED_DEFAULT = false;
    private const bool IS_MOUSE_Y_INVERTED_DEFAULT = false;
    private const bool IS_MOUSE_SCROLL_BLOCK_SELECTION_INVERTED_DEFAULT = false;
    private const bool IS_JOYSTICK_Y_INVERTED_CHAR_DEFAULT = false;
    private const bool IS_JOYSTICK_Y_INVERTED_VEHICLE_DEFAULT = false;
    private const float MOUSE_SENSITIVITY_DEFAULT = 1.655f;
    private const string JOYSTICK_INSTANCE_NAME_DEFAULT = null;
    private const float JOYSTICK_SENSITIVITY_DEFAULT = 2f;
    private const float JOYSTICK_EXPONENT_DEFAULT = 2f;
    private const float JOYSTICK_DEADZONE_DEFAULT = 0.2f;
    private string m_joystickInstanceNameSnapshot;
    private bool m_enabled = true;
    private readonly MyKeyHasher m_hasher = new MyKeyHasher();
    private readonly Dictionary<MyStringId, MyControl> m_defaultGameControlsList;
    private readonly Dictionary<MyStringId, MyControl> m_gameControlsList;
    private readonly Dictionary<MyStringId, MyControl> m_gameControlsSnapshot;
    private readonly HashSet<MyStringId> m_gameControlsBlacklist = new HashSet<MyStringId>();
    private readonly List<MyKeys> m_validKeyboardKeys = new List<MyKeys>();
    private readonly List<MyJoystickButtonsEnum> m_validJoystickButtons = new List<MyJoystickButtonsEnum>();
    private readonly List<MyJoystickAxesEnum> m_validJoystickAxes = new List<MyJoystickAxesEnum>();
    private readonly List<MyMouseButtonsEnum> m_validMouseButtons = new List<MyMouseButtonsEnum>();
    private readonly List<MyKeys> m_digitKeys = new List<MyKeys>();
    private readonly IVRageInput m_platformInput;
    private readonly IMyControlNameLookup m_nameLookup;
    private List<char> m_currentTextInput = new List<char>();
    private bool? m_isJoystickYAxisState_Reversing;
    private Action m_onActivated;
    private bool m_enableF12Menu;
    private string m_joystickInstanceNameForSearch;
    private bool m_gameWasFocused;
    private List<string> m_joysticks;
    private bool m_joystickConnected;
    private bool m_initializeJoystick;

    string VRage.ModAPI.IMyInput.JoystickInstanceName => this.JoystickInstanceName;

    ListReader<char> VRage.ModAPI.IMyInput.TextInput => this.TextInput;

    List<string> VRage.ModAPI.IMyInput.EnumerateJoystickNames() => this.EnumerateJoystickNames();

    bool VRage.ModAPI.IMyInput.IsAnyKeyPress() => this.IsAnyKeyPress();

    bool VRage.ModAPI.IMyInput.IsAnyMousePressed() => this.IsAnyMousePressed();

    bool VRage.ModAPI.IMyInput.IsAnyNewMousePressed() => this.IsAnyNewMousePressed();

    bool VRage.ModAPI.IMyInput.IsAnyShiftKeyPressed() => this.IsAnyShiftKeyPressed();

    bool VRage.ModAPI.IMyInput.IsAnyAltKeyPressed() => this.IsAnyAltKeyPressed();

    bool VRage.ModAPI.IMyInput.IsAnyCtrlKeyPressed() => this.IsAnyCtrlKeyPressed();

    void VRage.ModAPI.IMyInput.GetPressedKeys(List<MyKeys> keys) => this.GetPressedKeys(keys);

    bool VRage.ModAPI.IMyInput.IsKeyPress(MyKeys key) => this.IsKeyPress(key);

    bool VRage.ModAPI.IMyInput.WasKeyPress(MyKeys key) => this.WasKeyPress(key);

    bool VRage.ModAPI.IMyInput.IsNewKeyPressed(MyKeys key) => this.IsNewKeyPressed(key);

    bool VRage.ModAPI.IMyInput.IsNewKeyReleased(MyKeys key) => this.IsNewKeyReleased(key);

    bool VRage.ModAPI.IMyInput.IsMousePressed(MyMouseButtonsEnum button) => this.IsMousePressed(button);

    bool VRage.ModAPI.IMyInput.IsMouseReleased(MyMouseButtonsEnum button) => this.IsMouseReleased(button);

    bool VRage.ModAPI.IMyInput.IsNewMousePressed(MyMouseButtonsEnum button) => this.IsNewMousePressed(button);

    bool VRage.ModAPI.IMyInput.IsNewLeftMousePressed() => this.IsNewLeftMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewLeftMouseReleased() => this.IsNewLeftMouseReleased();

    bool VRage.ModAPI.IMyInput.IsLeftMousePressed() => this.IsLeftMousePressed();

    bool VRage.ModAPI.IMyInput.IsLeftMouseReleased() => this.IsLeftMouseReleased();

    bool VRage.ModAPI.IMyInput.IsRightMousePressed() => this.IsRightMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewRightMousePressed() => this.IsNewRightMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewRightMouseReleased() => this.IsNewRightMouseReleased();

    bool VRage.ModAPI.IMyInput.WasRightMousePressed() => this.WasRightMousePressed();

    bool VRage.ModAPI.IMyInput.WasRightMouseReleased() => this.WasRightMouseReleased();

    bool VRage.ModAPI.IMyInput.IsMiddleMousePressed() => this.IsMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewMiddleMousePressed() => this.IsNewMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewMiddleMouseReleased() => this.IsNewMiddleMouseReleased();

    bool VRage.ModAPI.IMyInput.WasMiddleMousePressed() => this.WasMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.WasMiddleMouseReleased() => this.WasMiddleMouseReleased();

    bool VRage.ModAPI.IMyInput.IsXButton1MousePressed() => this.IsXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton1MousePressed() => this.IsNewXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton1MouseReleased() => this.IsNewXButton1MouseReleased();

    bool VRage.ModAPI.IMyInput.WasXButton1MousePressed() => this.WasXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.WasXButton1MouseReleased() => this.WasXButton1MouseReleased();

    bool VRage.ModAPI.IMyInput.IsXButton2MousePressed() => this.IsXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton2MousePressed() => this.IsNewXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton2MouseReleased() => this.IsNewXButton2MouseReleased();

    bool VRage.ModAPI.IMyInput.WasXButton2MousePressed() => this.WasXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.WasXButton2MouseReleased() => this.WasXButton2MouseReleased();

    bool VRage.ModAPI.IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button) => this.IsJoystickButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button) => this.IsJoystickButtonNewPressed(button);

    bool VRage.ModAPI.IMyInput.IsNewJoystickButtonReleased(MyJoystickButtonsEnum button) => this.IsJoystickButtonNewReleased(button);

    float VRage.ModAPI.IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis) => this.GetJoystickAxisStateForGameplay(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis) => this.IsJoystickAxisPressed(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis) => this.IsJoystickAxisNewPressed(axis);

    bool VRage.ModAPI.IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis) => this.IsNewJoystickAxisReleased(axis);

    bool VRage.ModAPI.IMyInput.IsAnyMouseOrJoystickPressed() => this.IsAnyMouseOrJoystickPressed();

    bool VRage.ModAPI.IMyInput.IsAnyNewMouseOrJoystickPressed() => this.IsAnyNewMouseOrJoystickPressed();

    bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonPressed() => this.IsNewPrimaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonPressed() => this.IsNewSecondaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonReleased() => this.IsNewPrimaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonReleased() => this.IsNewSecondaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsPrimaryButtonReleased() => this.IsPrimaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsSecondaryButtonReleased() => this.IsSecondaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsPrimaryButtonPressed() => this.IsPrimaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsSecondaryButtonPressed() => this.IsSecondaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewButtonPressed(MySharedButtonsEnum button) => this.IsNewButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsButtonPressed(MySharedButtonsEnum button) => this.IsButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsNewButtonReleased(MySharedButtonsEnum button) => this.IsNewButtonReleased(button);

    bool VRage.ModAPI.IMyInput.IsButtonReleased(MySharedButtonsEnum button) => this.IsButtonReleased(button);

    int VRage.ModAPI.IMyInput.MouseScrollWheelValue() => this.MouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.PreviousMouseScrollWheelValue() => this.PreviousMouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.DeltaMouseScrollWheelValue() => this.DeltaMouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.GetMouseXForGamePlay() => this.GetMouseXForGamePlay();

    int VRage.ModAPI.IMyInput.GetMouseYForGamePlay() => this.GetMouseYForGamePlay();

    int VRage.ModAPI.IMyInput.GetMouseX() => this.GetMouseX();

    int VRage.ModAPI.IMyInput.GetMouseY() => this.GetMouseY();

    bool VRage.ModAPI.IMyInput.GetMouseXInversion() => this.GetMouseXInversion();

    bool VRage.ModAPI.IMyInput.GetMouseYInversion() => this.GetMouseYInversion();

    float VRage.ModAPI.IMyInput.GetMouseSensitivity() => this.GetMouseSensitivity();

    Vector2 VRage.ModAPI.IMyInput.GetMousePosition() => this.GetMousePosition();

    Vector2 VRage.ModAPI.IMyInput.GetMouseAreaSize() => this.GetMouseAreaSize();

    bool VRage.ModAPI.IMyInput.IsNewGameControlPressed(MyStringId controlEnum) => this.IsNewGameControlPressed(controlEnum);

    bool VRage.ModAPI.IMyInput.IsGameControlPressed(MyStringId controlEnum) => this.IsGameControlPressed(controlEnum);

    bool VRage.ModAPI.IMyInput.IsNewGameControlReleased(MyStringId controlEnum) => this.IsNewGameControlReleased(controlEnum);

    float VRage.ModAPI.IMyInput.GetGameControlAnalogState(MyStringId controlEnum) => this.GetGameControlAnalogState(controlEnum);

    bool VRage.ModAPI.IMyInput.IsGameControlReleased(MyStringId controlEnum) => this.IsGameControlReleased(controlEnum);

    bool VRage.ModAPI.IMyInput.IsKeyValid(MyKeys key) => this.IsKeyValid(key);

    bool VRage.ModAPI.IMyInput.IsKeyDigit(MyKeys key) => this.IsKeyDigit(key);

    bool VRage.ModAPI.IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button) => this.IsMouseButtonValid(button);

    bool VRage.ModAPI.IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button) => this.IsJoystickButtonValid(button);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis) => this.IsJoystickAxisValid(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickConnected() => this.IsJoystickConnected();

    bool VRage.ModAPI.IMyInput.JoystickAsMouse => this.JoystickAsMouse;

    bool VRage.ModAPI.IMyInput.IsJoystickLastUsed => this.IsJoystickLastUsed;

    event Action<bool> VRage.ModAPI.IMyInput.JoystickConnected
    {
      add => this.JoystickConnected += value;
      remove => this.JoystickConnected -= value;
    }

    IMyControl VRage.ModAPI.IMyInput.GetControl(MyKeys key) => (IMyControl) this.GetControl(key);

    IMyControl VRage.ModAPI.IMyInput.GetControl(MyMouseButtonsEnum button) => (IMyControl) this.GetControl(button);

    void VRage.ModAPI.IMyInput.GetListOfPressedKeys(List<MyKeys> keys) => this.GetListOfPressedKeys(keys);

    void VRage.ModAPI.IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result) => this.GetListOfPressedMouseButtons(result);

    IMyControl VRage.ModAPI.IMyInput.GetGameControl(MyStringId controlEnum) => (IMyControl) this.GetGameControl(controlEnum);

    string VRage.ModAPI.IMyInput.GetKeyName(MyKeys key) => this.GetKeyName(key);

    string VRage.ModAPI.IMyInput.GetName(MyMouseButtonsEnum mouseButton) => this.GetName(mouseButton);

    string VRage.ModAPI.IMyInput.GetName(MyJoystickButtonsEnum joystickButton) => this.GetName(joystickButton);

    string VRage.ModAPI.IMyInput.GetName(MyJoystickAxesEnum joystickAxis) => this.GetName(joystickAxis);

    string VRage.ModAPI.IMyInput.GetUnassignedName() => this.GetUnassignedName();

    public string JoystickInstanceName
    {
      get => this.m_joystickInstanceName;
      set
      {
        if (!(this.m_joystickInstanceName != value))
          return;
        this.m_joystickInstanceName = this.m_joystickInstanceNameForSearch = value;
        this.m_initializeJoystick = true;
      }
    }

    private bool Enabled
    {
      get => this.m_enabled;
      set
      {
        if (this.m_enabled == value)
          return;
        this.ClearStates();
        this.m_enabled = value;
      }
    }

    public bool IsEnabled() => this.m_enabled;

    public void EnableInput(bool enable) => this.Enabled = enable;

    public bool OverrideUpdate
    {
      get => this.m_overrideUpdate;
      set => this.m_overrideUpdate = value;
    }

    public MyMouseState ActualMouseState => this.m_actualMouseState;

    public MyJoystickState ActualJoystickState => this.m_actualJoystickState;

    public MyVRageInput(
      IVRageInput textInputBuffer,
      IMyControlNameLookup nameLookup,
      Dictionary<MyStringId, MyControl> gameControls,
      bool enableDevKeys,
      Action onActivated)
    {
      this.m_platformInput = textInputBuffer;
      this.m_nameLookup = nameLookup;
      this.m_defaultGameControlsList = gameControls;
      this.m_gameControlsList = new Dictionary<MyStringId, MyControl>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
      this.m_gameControlsSnapshot = new Dictionary<MyStringId, MyControl>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
      this.CloneControls(this.m_defaultGameControlsList, this.m_gameControlsList);
      this.m_onActivated = onActivated;
      this.ResetJoystickState();
    }

    public void AddDefaultControl(MyStringId stringId, MyControl control)
    {
      this.m_gameControlsList[stringId] = control;
      this.m_defaultGameControlsList[stringId] = control;
    }

    public void SearchForJoystick() => this.m_initializeJoystick = true;

    public void LoadData(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
      this.m_mouseXIsInverted = false;
      this.m_mouseYIsInverted = false;
      this.m_joystickYInvertedChar = false;
      this.m_joystickYInvertedVehicle = false;
      this.m_mouseSensitivity = 1.655f;
      this.m_joystickInstanceName = this.m_joystickInstanceNameForSearch = (string) null;
      this.m_joystickSensitivity = 2f;
      this.m_joystickDeadzone = 0.2f;
      this.m_joystickExponent = 2f;
      this.m_digitKeys.Add(MyKeys.D0);
      this.m_digitKeys.Add(MyKeys.D1);
      this.m_digitKeys.Add(MyKeys.D2);
      this.m_digitKeys.Add(MyKeys.D3);
      this.m_digitKeys.Add(MyKeys.D4);
      this.m_digitKeys.Add(MyKeys.D5);
      this.m_digitKeys.Add(MyKeys.D6);
      this.m_digitKeys.Add(MyKeys.D7);
      this.m_digitKeys.Add(MyKeys.D8);
      this.m_digitKeys.Add(MyKeys.D9);
      this.m_digitKeys.Add(MyKeys.NumPad0);
      this.m_digitKeys.Add(MyKeys.NumPad1);
      this.m_digitKeys.Add(MyKeys.NumPad2);
      this.m_digitKeys.Add(MyKeys.NumPad3);
      this.m_digitKeys.Add(MyKeys.NumPad4);
      this.m_digitKeys.Add(MyKeys.NumPad5);
      this.m_digitKeys.Add(MyKeys.NumPad6);
      this.m_digitKeys.Add(MyKeys.NumPad7);
      this.m_digitKeys.Add(MyKeys.NumPad8);
      this.m_digitKeys.Add(MyKeys.NumPad9);
      this.m_validKeyboardKeys.Add(MyKeys.A);
      this.m_validKeyboardKeys.Add(MyKeys.Add);
      this.m_validKeyboardKeys.Add(MyKeys.B);
      this.m_validKeyboardKeys.Add(MyKeys.Back);
      this.m_validKeyboardKeys.Add(MyKeys.C);
      this.m_validKeyboardKeys.Add(MyKeys.CapsLock);
      this.m_validKeyboardKeys.Add(MyKeys.D);
      this.m_validKeyboardKeys.Add(MyKeys.D0);
      this.m_validKeyboardKeys.Add(MyKeys.D1);
      this.m_validKeyboardKeys.Add(MyKeys.D2);
      this.m_validKeyboardKeys.Add(MyKeys.D3);
      this.m_validKeyboardKeys.Add(MyKeys.D4);
      this.m_validKeyboardKeys.Add(MyKeys.D5);
      this.m_validKeyboardKeys.Add(MyKeys.D6);
      this.m_validKeyboardKeys.Add(MyKeys.D7);
      this.m_validKeyboardKeys.Add(MyKeys.D8);
      this.m_validKeyboardKeys.Add(MyKeys.D9);
      this.m_validKeyboardKeys.Add(MyKeys.Decimal);
      this.m_validKeyboardKeys.Add(MyKeys.Delete);
      this.m_validKeyboardKeys.Add(MyKeys.Divide);
      this.m_validKeyboardKeys.Add(MyKeys.Down);
      this.m_validKeyboardKeys.Add(MyKeys.E);
      this.m_validKeyboardKeys.Add(MyKeys.End);
      this.m_validKeyboardKeys.Add(MyKeys.Enter);
      this.m_validKeyboardKeys.Add(MyKeys.F);
      this.m_validKeyboardKeys.Add(MyKeys.G);
      this.m_validKeyboardKeys.Add(MyKeys.H);
      this.m_validKeyboardKeys.Add(MyKeys.Home);
      this.m_validKeyboardKeys.Add(MyKeys.I);
      this.m_validKeyboardKeys.Add(MyKeys.Insert);
      this.m_validKeyboardKeys.Add(MyKeys.J);
      this.m_validKeyboardKeys.Add(MyKeys.K);
      this.m_validKeyboardKeys.Add(MyKeys.L);
      this.m_validKeyboardKeys.Add(MyKeys.Left);
      this.m_validKeyboardKeys.Add(MyKeys.LeftAlt);
      this.m_validKeyboardKeys.Add(MyKeys.LeftControl);
      this.m_validKeyboardKeys.Add(MyKeys.LeftShift);
      this.m_validKeyboardKeys.Add(MyKeys.M);
      this.m_validKeyboardKeys.Add(MyKeys.Multiply);
      this.m_validKeyboardKeys.Add(MyKeys.N);
      this.m_validKeyboardKeys.Add(MyKeys.None);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad0);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad1);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad2);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad3);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad4);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad5);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad6);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad7);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad8);
      this.m_validKeyboardKeys.Add(MyKeys.NumPad9);
      this.m_validKeyboardKeys.Add(MyKeys.O);
      this.m_validKeyboardKeys.Add(MyKeys.OemCloseBrackets);
      this.m_validKeyboardKeys.Add(MyKeys.OemComma);
      this.m_validKeyboardKeys.Add(MyKeys.OemMinus);
      this.m_validKeyboardKeys.Add(MyKeys.OemOpenBrackets);
      this.m_validKeyboardKeys.Add(MyKeys.OemPeriod);
      this.m_validKeyboardKeys.Add(MyKeys.OemPipe);
      this.m_validKeyboardKeys.Add(MyKeys.OemPlus);
      this.m_validKeyboardKeys.Add(MyKeys.OemQuestion);
      this.m_validKeyboardKeys.Add(MyKeys.OemQuotes);
      this.m_validKeyboardKeys.Add(MyKeys.OemSemicolon);
      this.m_validKeyboardKeys.Add(MyKeys.OemTilde);
      this.m_validKeyboardKeys.Add(MyKeys.OemBackslash);
      this.m_validKeyboardKeys.Add(MyKeys.P);
      this.m_validKeyboardKeys.Add(MyKeys.PageDown);
      this.m_validKeyboardKeys.Add(MyKeys.PageUp);
      this.m_validKeyboardKeys.Add(MyKeys.Pause);
      this.m_validKeyboardKeys.Add(MyKeys.Q);
      this.m_validKeyboardKeys.Add(MyKeys.R);
      this.m_validKeyboardKeys.Add(MyKeys.Right);
      this.m_validKeyboardKeys.Add(MyKeys.RightAlt);
      this.m_validKeyboardKeys.Add(MyKeys.RightControl);
      this.m_validKeyboardKeys.Add(MyKeys.RightShift);
      this.m_validKeyboardKeys.Add(MyKeys.Shift);
      this.m_validKeyboardKeys.Add(MyKeys.Alt);
      this.m_validKeyboardKeys.Add(MyKeys.S);
      this.m_validKeyboardKeys.Add(MyKeys.Space);
      this.m_validKeyboardKeys.Add(MyKeys.Subtract);
      this.m_validKeyboardKeys.Add(MyKeys.T);
      this.m_validKeyboardKeys.Add(MyKeys.Tab);
      this.m_validKeyboardKeys.Add(MyKeys.U);
      this.m_validKeyboardKeys.Add(MyKeys.Up);
      this.m_validKeyboardKeys.Add(MyKeys.V);
      this.m_validKeyboardKeys.Add(MyKeys.W);
      this.m_validKeyboardKeys.Add(MyKeys.X);
      this.m_validKeyboardKeys.Add(MyKeys.Y);
      this.m_validKeyboardKeys.Add(MyKeys.Z);
      this.m_validKeyboardKeys.Add(MyKeys.F1);
      this.m_validKeyboardKeys.Add(MyKeys.F2);
      this.m_validKeyboardKeys.Add(MyKeys.F3);
      this.m_validKeyboardKeys.Add(MyKeys.F4);
      this.m_validKeyboardKeys.Add(MyKeys.F5);
      this.m_validKeyboardKeys.Add(MyKeys.F6);
      this.m_validKeyboardKeys.Add(MyKeys.F7);
      this.m_validKeyboardKeys.Add(MyKeys.F8);
      this.m_validKeyboardKeys.Add(MyKeys.F9);
      this.m_validKeyboardKeys.Add(MyKeys.F10);
      this.m_validKeyboardKeys.Add(MyKeys.F11);
      this.m_validKeyboardKeys.Add(MyKeys.F12);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.Left);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.Middle);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.Right);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.XButton1);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.XButton2);
      this.m_validMouseButtons.Add(MyMouseButtonsEnum.None);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J01);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J02);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J03);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J04);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J05);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J06);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J07);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J08);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J09);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J10);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J11);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J12);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J13);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J14);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J15);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.J16);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDLeft);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDRight);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDUp);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDDown);
      this.m_validJoystickButtons.Add(MyJoystickButtonsEnum.None);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Xpos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Xneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Ypos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Yneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Zpos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Zneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationXpos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationXneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationYpos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationYneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationZpos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationZneg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider1pos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider1neg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider2pos);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider2neg);
      this.m_validJoystickAxes.Add(MyJoystickAxesEnum.None);
      this.CheckValidControls(this.m_defaultGameControlsList);
      this.LoadControls(controlsGeneral, controlsButtons);
      this.InitializeJoystickIfPossible();
      this.TakeSnapshot();
      this.ClearBlacklist();
    }

    public void LoadContent()
    {
      this.IsDirectInputInitialized = MyVRage.Platform.CreateInput2();
      if (this.m_enableF12Menu)
        MyLog.Default.WriteLine("DEVELOPER KEYS ENABLED");
      this.m_keyboardState = new MyGuiLocalizedKeyboardState(MyVRage.Platform.Input2);
    }

    public ListReader<char> TextInput => new ListReader<char>(this.m_currentTextInput);

    public void UnloadData()
    {
    }

    public void NegateEscapePress() => this.m_keyboardState.NegateEscapePress();

    public unsafe bool IsJoystickIdle()
    {
      if (!this.m_joystickConnected)
        return true;
      for (int index = 0; index < 16; ++index)
      {
        if (this.m_actualJoystickState.Buttons[index] > (byte) 0)
          return false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_actualJoystickState.X >> 12 == 8 && this.m_actualJoystickState.Y >> 12 == 8 && (this.m_actualJoystickState.Z >> 12 == 8 && this.m_actualJoystickState.RotationX >> 12 == 8) && (this.m_actualJoystickState.RotationY >> 12 == 8 && this.m_actualJoystickState.Sliders.FixedElementField >> 12 == 8 && this.m_actualJoystickState.Sliders[1] >> 12 == 8) && this.m_actualJoystickState.PointOfViewControllers.FixedElementField == -1;
    }

    private void CheckValidControls(Dictionary<MyStringId, MyControl> controls)
    {
      foreach (MyControl myControl in controls.Values)
        ;
    }

    public List<string> EnumerateJoystickNames() => this.m_joysticks;

    private void InitializeJoystickIfPossible()
    {
      this.m_joysticks = MyVRage.Platform.Input2.EnumerateJoystickNames();
      this.SetConnectedJoystick(MyVRage.Platform.Input2.InitializeJoystickIfPossible(this.m_joystickInstanceName));
    }

    private void SearchForJoystickNow()
    {
      this.m_joystickInstanceNameForSearch = MyVRage.Platform.Input2?.InitializeJoystickIfPossible(this.m_joystickInstanceNameForSearch);
      this.SetConnectedJoystick(this.m_joystickInstanceNameForSearch);
    }

    public void UpdateJoystickChanged() => this.InitializeJoystickIfPossible();

    public void DeviceChangeCallback()
    {
      if (!MyVRage.Platform.Input2.IsJoystickConnected() || this.m_initializeJoystick)
      {
        this.m_initializeJoystick = false;
        this.InitializeJoystickIfPossible();
      }
      else
        this.m_joysticks = MyVRage.Platform.Input2.EnumerateJoystickNames();
    }

    public void ClearStates()
    {
      this.m_keyboardState.ClearStates();
      this.m_previousMouseState = this.m_actualMouseState;
      this.m_actualMouseState = new MyMouseState();
      this.m_actualMouseStateRaw = new MyMouseState();
      this.m_absoluteMousePosition = -Vector2.One;
    }

    public void UpdateStatesFromPlayback(
      MyKeyboardState currentKeyboard,
      MyKeyboardState previousKeyboard,
      MyMouseState currentMouse,
      MyMouseState previousMouse,
      MyJoystickState currentJoystick,
      MyJoystickState previousJoystick,
      int x,
      int y,
      List<char> keyboardSnapshotText)
    {
      this.m_keyboardState.UpdateStatesFromSnapshot(currentKeyboard, previousKeyboard);
      this.m_previousMouseState = previousMouse;
      this.m_actualMouseState = currentMouse;
      this.m_actualJoystickState = currentJoystick;
      this.m_previousJoystickState = previousJoystick;
      this.m_absoluteMousePosition = new Vector2((float) x, (float) y);
      if (this.m_gameWasFocused)
        this.m_platformInput.MousePosition = this.m_absoluteMousePosition;
      if (keyboardSnapshotText == null)
        return;
      foreach (char ch in keyboardSnapshotText)
        this.m_platformInput.AddChar(ch);
    }

    public void UpdateStates()
    {
      if (!this.m_enabled)
        return;
      this.m_keyboardState.UpdateStates();
      this.m_hasher.Keys.Clear();
      this.GetPressedKeys(this.m_hasher.Keys);
      uint[] developerKeys = MyVRage.Platform.Input2.DeveloperKeys;
      if (!this.m_enableF12Menu && this.m_hasher.TestHash(developerKeys[0], developerKeys[1], developerKeys[2], developerKeys[3], "salt!@#"))
      {
        this.m_enableF12Menu = true;
        this.m_onActivated();
        MyLog.Default.WriteLine("DEVELOPER KEYS ENABLED");
      }
      this.m_previousMouseState = this.m_actualMouseState;
      MyVRage.Platform.Input2.GetMouseState(out this.m_actualMouseStateRaw);
      int num1 = this.m_actualMouseState.ScrollWheelValue + this.m_actualMouseStateRaw.ScrollWheelValue;
      this.m_actualMouseState = this.m_actualMouseStateRaw;
      this.m_actualMouseState.ScrollWheelValue = num1;
      this.m_absoluteMousePosition = this.m_platformInput.MousePosition;
      if (this.m_initializeJoystick)
        this.DeviceChangeCallback();
      if (this.IsJoystickConnected())
      {
        try
        {
          this.m_previousJoystickState = this.m_actualJoystickState;
          MyVRage.Platform.Input2.GetJoystickState(ref this.m_actualJoystickState);
          if (this.JoystickAsMouse)
          {
            float stateForGameplay1 = this.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xpos);
            float num2 = -this.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xneg);
            double stateForGameplay2 = (double) this.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Ypos);
            float num3 = -this.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Yneg);
            float num4 = (float) (((double) stateForGameplay1 + (double) num2) * 4.0);
            double num5 = (double) num3;
            float num6 = (float) ((stateForGameplay2 + num5) * 4.0);
            if ((double) num4 == 0.0)
            {
              if ((double) num6 == 0.0)
                goto label_13;
            }
            this.m_absoluteMousePosition.X += num4;
            this.m_absoluteMousePosition.Y += num6;
            this.m_platformInput.MousePosition = this.m_absoluteMousePosition;
          }
        }
        catch
        {
          this.SetConnectedJoystick((string) null);
        }
label_13:
        if (!MyVRage.Platform.Input2.IsJoystickConnected())
          this.SetConnectedJoystick((string) null);
      }
      else
        this.ResetJoystickState();
      if (this.IsJoystickLastUsed)
      {
        if (!this.IsAnyMousePressed() && !this.IsAnyKeyPress() && (!this.IsMouseMoved() && !this.IsScrolled()))
          return;
        this.IsJoystickLastUsed = false;
      }
      else
      {
        if (!this.IsAnyJoystickButtonPressed() && !this.IsAnyJoystickAxisPressed())
          return;
        this.IsJoystickLastUsed = true;
      }
    }

    private void ResetJoystickState()
    {
      if (!this.m_joystickConnected)
        this.SearchForJoystickNow();
      MyJoystickState myJoystickState = new MyJoystickState();
      myJoystickState.X = 32768;
      myJoystickState.Y = 32768;
      myJoystickState.RotationX = 32768;
      myJoystickState.RotationY = 32768;
      myJoystickState.Z_Left = 0;
      myJoystickState.Z_Right = 0;
      this.m_actualJoystickState = myJoystickState = myJoystickState;
      this.m_previousJoystickState = myJoystickState;
      this.m_isJoystickYAxisState_Reversing = new bool?();
    }

    public bool Update(bool gameFocused)
    {
      if (!this.m_gameWasFocused & gameFocused && !this.m_overrideUpdate)
        this.UpdateStates();
      this.m_gameWasFocused = gameFocused;
      if (!gameFocused && !this.m_overrideUpdate)
      {
        this.ClearStates();
        return false;
      }
      if (!this.m_overrideUpdate)
        this.UpdateStates();
      this.m_platformInput.GetBufferedTextInput(ref this.m_currentTextInput);
      return true;
    }

    public bool IsAnyKeyPress() => this.m_keyboardState.IsAnyKeyPressed();

    public bool IsAnyNewKeyPress() => this.m_keyboardState.IsAnyKeyPressed() && !this.m_keyboardState.GetPreviousKeyboardState().IsAnyKeyPressed();

    public bool IsAnyMousePressed() => this.m_actualMouseState.LeftButton || this.m_actualMouseState.MiddleButton || (this.m_actualMouseState.RightButton || this.m_actualMouseState.XButton1) || this.m_actualMouseState.XButton2;

    public bool IsAnyNewMousePressed() => this.IsNewLeftMousePressed() || this.IsNewMiddleMousePressed() || (this.IsNewRightMousePressed() || this.IsNewXButton1MousePressed()) || this.IsNewXButton2MousePressed();

    private unsafe bool IsAnyJoystickButtonPressed()
    {
      if (this.m_joystickConnected)
      {
        if ((this.IsGamepadKeyDownPressed() || this.IsGamepadKeyLeftPressed() || this.IsGamepadKeyRightPressed() ? 1 : (this.IsGamepadKeyUpPressed() ? 1 : 0)) != 0)
          return true;
        for (int index = 0; index < 16; ++index)
        {
          if (this.m_actualJoystickState.Buttons[index] > (byte) 0)
            return true;
        }
      }
      return false;
    }

    private unsafe bool IsAnyNewJoystickButtonPressed()
    {
      if (this.m_joystickConnected)
      {
        for (int index = 0; index < 16; ++index)
        {
          if (this.m_actualJoystickState.Buttons[index] > (byte) 0 && this.m_previousJoystickState.Buttons[index] == (byte) 0)
            return true;
        }
      }
      return false;
    }

    public bool IsNewGameControlJoystickOnlyPressed(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsNewJoystickPressed();
    }

    public string TokenEvaluate(string token, string context)
    {
      MyControl myControl;
      if (!this.m_gameControlsList.TryGetValue(MyStringId.GetOrCompute(token), out myControl))
        return "";
      string controlButtonName1 = myControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName2 = myControl.GetControlButtonName(MyGuiInputDeviceEnum.Mouse);
      if (string.IsNullOrEmpty(controlButtonName1))
        return controlButtonName2;
      return !string.IsNullOrEmpty(controlButtonName2) ? controlButtonName1 + "'/'" + controlButtonName2 : controlButtonName1;
    }

    public static object GetHighlightedControl(MyStringId controlId)
    {
      string str1 = MyInput.Static.GetGameControl(controlId) != null ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : (string) null;
      string str2 = MyInput.Static.GetGameControl(controlId) != null ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Mouse) : (string) null;
      if (string.IsNullOrEmpty(str1))
        return (object) ("[" + str2 + "]");
      if (string.IsNullOrEmpty(str2))
        return (object) ("[" + str1 + "]");
      return (object) ("[" + str1 + "'/'" + str2 + "]");
    }

    public bool IsGameControlJoystickOnlyPressed(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsJoystickPressed();
    }

    public bool IsNewGameControlJoystickOnlyReleased(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsNewJoystickReleased();
    }

    private bool IsAnyJoystickAxisPressed()
    {
      if (this.m_joystickConnected)
      {
        foreach (MyJoystickAxesEnum validJoystickAx in this.m_validJoystickAxes)
        {
          if (validJoystickAx != MyJoystickAxesEnum.None && this.IsJoystickAxisPressed(validJoystickAx))
            return true;
        }
      }
      return false;
    }

    public bool IsAnyMouseOrJoystickPressed() => this.IsAnyMousePressed() || this.IsAnyJoystickButtonPressed();

    public bool IsAnyNewMouseOrJoystickPressed() => this.IsAnyNewMousePressed() || this.IsAnyNewJoystickButtonPressed();

    public bool IsNewPrimaryButtonPressed() => this.IsNewLeftMousePressed() || this.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01);

    public bool IsNewSecondaryButtonPressed() => this.IsNewRightMousePressed() || this.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J02);

    public bool IsNewPrimaryButtonReleased() => this.IsNewLeftMouseReleased() || this.IsJoystickButtonNewReleased(MyJoystickButtonsEnum.J01);

    public bool IsNewSecondaryButtonReleased() => this.IsNewRightMouseReleased() || this.IsJoystickButtonNewReleased(MyJoystickButtonsEnum.J02);

    public bool IsPrimaryButtonReleased() => this.IsLeftMouseReleased() || this.IsJoystickButtonReleased(MyJoystickButtonsEnum.J01);

    public bool IsSecondaryButtonReleased() => this.IsRightMouseReleased() || this.IsJoystickButtonReleased(MyJoystickButtonsEnum.J02);

    public bool IsPrimaryButtonPressed() => this.IsLeftMousePressed() || this.IsJoystickButtonPressed(MyJoystickButtonsEnum.J01);

    public bool IsSecondaryButtonPressed() => this.IsRightMousePressed() || this.IsJoystickButtonPressed(MyJoystickButtonsEnum.J02);

    public bool IsNewButtonPressed(MySharedButtonsEnum button)
    {
      if (button == MySharedButtonsEnum.Primary)
        return this.IsNewPrimaryButtonPressed();
      return button == MySharedButtonsEnum.Secondary && this.IsNewSecondaryButtonPressed();
    }

    public bool IsButtonPressed(MySharedButtonsEnum button)
    {
      if (button == MySharedButtonsEnum.Primary)
        return this.IsPrimaryButtonPressed();
      return button == MySharedButtonsEnum.Secondary && this.IsSecondaryButtonPressed();
    }

    public bool IsNewButtonReleased(MySharedButtonsEnum button)
    {
      if (button == MySharedButtonsEnum.Primary)
        return this.IsNewPrimaryButtonReleased();
      return button == MySharedButtonsEnum.Secondary && this.IsNewSecondaryButtonReleased();
    }

    public bool IsButtonReleased(MySharedButtonsEnum button)
    {
      if (button == MySharedButtonsEnum.Primary)
        return this.IsPrimaryButtonReleased();
      return button == MySharedButtonsEnum.Secondary && this.IsSecondaryButtonReleased();
    }

    public bool IsAnyWinKeyPressed() => this.IsKeyPress(MyKeys.LeftWindows) || this.IsKeyPress(MyKeys.RightWindows);

    public bool IsAnyShiftKeyPressed() => this.IsKeyPress(MyKeys.Shift) || this.IsKeyPress(MyKeys.LeftShift) || this.IsKeyPress(MyKeys.RightShift);

    public bool IsAnyAltKeyPressed() => this.IsKeyPress(MyKeys.Alt) || this.IsKeyPress(MyKeys.LeftAlt) || this.IsKeyPress(MyKeys.RightAlt);

    public bool IsAnyCtrlKeyPressed() => this.IsKeyPress(MyKeys.Control) || this.IsKeyPress(MyKeys.LeftControl) || this.IsKeyPress(MyKeys.RightControl);

    public void GetPressedKeys(List<MyKeys> keys) => this.m_keyboardState.GetActualPressedKeys(keys);

    public bool IsKeyPress(MyKeys key)
    {
      if (MyInput.EnableModifierKeyEmulation)
      {
        switch (key)
        {
          case MyKeys.Shift:
          case MyKeys.LeftShift:
          case MyKeys.RightShift:
            return this.m_keyboardState.IsKeyDown(MyKeys.LeftShift) || this.m_keyboardState.IsKeyDown(MyKeys.RightShift) || this.m_keyboardState.IsKeyDown(MyKeys.Shift);
          case MyKeys.Control:
          case MyKeys.LeftControl:
          case MyKeys.RightControl:
            return this.m_keyboardState.IsKeyDown(MyKeys.LeftControl) || this.m_keyboardState.IsKeyDown(MyKeys.RightControl) || this.m_keyboardState.IsKeyDown(MyKeys.Control);
          case MyKeys.Alt:
          case MyKeys.LeftAlt:
          case MyKeys.RightAlt:
            return this.m_keyboardState.IsKeyDown(MyKeys.LeftAlt) || this.m_keyboardState.IsKeyDown(MyKeys.RightAlt) || this.m_keyboardState.IsKeyDown(MyKeys.Alt);
        }
      }
      return this.m_keyboardState.IsKeyDown(key);
    }

    public bool WasKeyPress(MyKeys key) => this.m_keyboardState.IsPreviousKeyDown(key);

    public bool IsNewKeyPressed(MyKeys key) => this.m_keyboardState.IsKeyDown(key) && this.m_keyboardState.IsPreviousKeyUp(key);

    public bool IsNewKeyReleased(MyKeys key) => this.m_keyboardState.IsKeyUp(key) && this.m_keyboardState.IsPreviousKeyDown(key);

    public bool IsMousePressed(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.IsLeftMousePressed();
        case MyMouseButtonsEnum.Middle:
          return this.IsMiddleMousePressed();
        case MyMouseButtonsEnum.Right:
          return this.IsRightMousePressed();
        case MyMouseButtonsEnum.XButton1:
          return this.IsXButton1MousePressed();
        case MyMouseButtonsEnum.XButton2:
          return this.IsXButton2MousePressed();
        default:
          return false;
      }
    }

    public bool IsMouseReleased(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.IsLeftMouseReleased();
        case MyMouseButtonsEnum.Middle:
          return this.IsMiddleMouseReleased();
        case MyMouseButtonsEnum.Right:
          return this.IsRightMouseReleased();
        case MyMouseButtonsEnum.XButton1:
          return this.IsXButton1MouseReleased();
        case MyMouseButtonsEnum.XButton2:
          return this.IsXButton2MouseReleased();
        default:
          return false;
      }
    }

    public bool WasMousePressed(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.WasLeftMousePressed();
        case MyMouseButtonsEnum.Middle:
          return this.WasMiddleMousePressed();
        case MyMouseButtonsEnum.Right:
          return this.WasRightMousePressed();
        case MyMouseButtonsEnum.XButton1:
          return this.WasXButton1MousePressed();
        case MyMouseButtonsEnum.XButton2:
          return this.WasXButton2MousePressed();
        default:
          return false;
      }
    }

    public bool WasMouseReleased(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.WasLeftMouseReleased();
        case MyMouseButtonsEnum.Middle:
          return this.WasMiddleMouseReleased();
        case MyMouseButtonsEnum.Right:
          return this.WasRightMouseReleased();
        case MyMouseButtonsEnum.XButton1:
          return this.WasXButton1MouseReleased();
        case MyMouseButtonsEnum.XButton2:
          return this.WasXButton2MouseReleased();
        default:
          return false;
      }
    }

    public bool IsNewMousePressed(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.IsNewLeftMousePressed();
        case MyMouseButtonsEnum.Middle:
          return this.IsNewMiddleMousePressed();
        case MyMouseButtonsEnum.Right:
          return this.IsNewRightMousePressed();
        case MyMouseButtonsEnum.XButton1:
          return this.IsNewXButton1MousePressed();
        case MyMouseButtonsEnum.XButton2:
          return this.IsNewXButton2MousePressed();
        default:
          return false;
      }
    }

    public bool IsNewMouseReleased(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return this.IsNewLeftMouseReleased();
        case MyMouseButtonsEnum.Middle:
          return this.IsNewMiddleMouseReleased();
        case MyMouseButtonsEnum.Right:
          return this.IsNewRightMouseReleased();
        case MyMouseButtonsEnum.XButton1:
          return this.IsNewXButton1MouseReleased();
        case MyMouseButtonsEnum.XButton2:
          return this.IsNewXButton2MouseReleased();
        default:
          return false;
      }
    }

    public bool IsNewLeftMousePressed() => this.IsLeftMousePressed() && this.WasLeftMouseReleased();

    public bool IsNewLeftMouseReleased() => this.IsLeftMouseReleased() && this.WasLeftMousePressed();

    public bool IsLeftMousePressed() => this.m_actualMouseState.LeftButton;

    public bool IsLeftMouseReleased() => !this.m_actualMouseState.LeftButton;

    public bool WasLeftMouseReleased() => !this.m_previousMouseState.LeftButton;

    public bool WasLeftMousePressed() => this.m_previousMouseState.LeftButton;

    public bool IsRightMousePressed() => this.m_actualMouseState.RightButton;

    public bool IsRightMouseReleased() => !this.m_actualMouseState.RightButton;

    public bool IsNewRightMousePressed() => this.m_actualMouseState.RightButton && !this.m_previousMouseState.RightButton;

    public bool IsNewRightMouseReleased() => !this.m_actualMouseState.RightButton && this.m_previousMouseState.RightButton;

    public bool WasRightMousePressed() => this.m_previousMouseState.RightButton;

    public bool WasRightMouseReleased() => !this.m_previousMouseState.RightButton;

    public bool IsMiddleMousePressed() => this.m_actualMouseState.MiddleButton;

    public bool IsMiddleMouseReleased() => !this.m_actualMouseState.MiddleButton;

    public bool IsNewMiddleMousePressed() => this.m_actualMouseState.MiddleButton && !this.m_previousMouseState.MiddleButton;

    public bool IsNewMiddleMouseReleased() => !this.m_actualMouseState.MiddleButton && this.m_previousMouseState.MiddleButton;

    public bool WasMiddleMousePressed() => this.m_previousMouseState.MiddleButton;

    public bool WasMiddleMouseReleased() => !this.m_previousMouseState.MiddleButton;

    public bool IsXButton1MousePressed() => this.m_actualMouseState.XButton1;

    public bool IsXButton1MouseReleased() => !this.m_actualMouseState.XButton1;

    public bool IsNewXButton1MousePressed() => this.m_actualMouseState.XButton1 && !this.m_previousMouseState.XButton1;

    public bool IsNewXButton1MouseReleased() => !this.m_actualMouseState.XButton1 && this.m_previousMouseState.XButton1;

    public bool WasXButton1MousePressed() => this.m_previousMouseState.XButton1;

    public bool WasXButton1MouseReleased() => !this.m_previousMouseState.XButton1;

    public bool IsXButton2MousePressed() => this.m_actualMouseState.XButton2;

    public bool IsXButton2MouseReleased() => !this.m_actualMouseState.XButton2;

    public bool IsNewXButton2MousePressed() => this.m_actualMouseState.XButton2 && !this.m_previousMouseState.XButton2;

    public bool IsNewXButton2MouseReleased() => !this.m_actualMouseState.XButton2 && this.m_previousMouseState.XButton2;

    public bool WasXButton2MousePressed() => this.m_previousMouseState.XButton2;

    public bool WasXButton2MouseReleased() => !this.m_previousMouseState.XButton2;

    public unsafe bool IsJoystickButtonPressed(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = this.IsGamepadKeyLeftPressed();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = this.IsGamepadKeyRightPressed();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = this.IsGamepadKeyUpPressed();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = this.IsGamepadKeyDownPressed();
            break;
          default:
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            flag = ^(byte&) ((byte) this.m_actualJoystickState.Buttons + (button - (byte) 5)) > (byte) 0;
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public bool IsJoystickButtonNewPressed(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = this.IsNewGamepadKeyLeftPressed();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = this.IsNewGamepadKeyRightPressed();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = this.IsNewGamepadKeyUpPressed();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = this.IsNewGamepadKeyDownPressed();
            break;
          default:
            flag = this.m_actualJoystickState.IsPressed((int) (button - (byte) 5)) && !this.m_previousJoystickState.IsPressed((int) (button - (byte) 5));
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public bool IsJoystickButtonNewReleased(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = this.IsNewGamepadKeyLeftReleased();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = this.IsNewGamepadKeyRightReleased();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = this.IsNewGamepadKeyUpReleased();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = this.IsNewGamepadKeyDownReleased();
            break;
          default:
            flag = this.m_actualJoystickState.IsReleased((int) (button - (byte) 5)) && this.m_previousJoystickState.IsPressed((int) (button - (byte) 5));
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public bool IsJoystickButtonReleased(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = !this.IsGamepadKeyLeftPressed();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = !this.IsGamepadKeyRightPressed();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = !this.IsGamepadKeyUpPressed();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = !this.IsGamepadKeyDownPressed();
            break;
          default:
            flag = this.m_actualJoystickState.IsReleased((int) (button - (byte) 5));
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public unsafe bool WasJoystickButtonPressed(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = this.WasGamepadKeyLeftPressed();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = this.WasGamepadKeyRightPressed();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = this.WasGamepadKeyUpPressed();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = this.WasGamepadKeyDownPressed();
            break;
          default:
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            flag = ^(byte&) ((byte) this.m_previousJoystickState.Buttons + (button - (byte) 5)) > (byte) 0;
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public bool WasJoystickButtonReleased(MyJoystickButtonsEnum button)
    {
      bool flag = false;
      if (this.m_joystickConnected)
      {
        switch (button)
        {
          case MyJoystickButtonsEnum.None:
            break;
          case MyJoystickButtonsEnum.JDLeft:
            flag = !this.WasGamepadKeyLeftPressed();
            break;
          case MyJoystickButtonsEnum.JDRight:
            flag = !this.WasGamepadKeyRightPressed();
            break;
          case MyJoystickButtonsEnum.JDUp:
            flag = !this.WasGamepadKeyUpPressed();
            break;
          case MyJoystickButtonsEnum.JDDown:
            flag = !this.WasGamepadKeyDownPressed();
            break;
          default:
            flag = this.m_previousJoystickState.IsReleased((int) (button - (byte) 5));
            break;
        }
      }
      return !flag && button == MyJoystickButtonsEnum.None || flag;
    }

    public unsafe float GetJoystickAxisStateRaw(MyJoystickAxesEnum axis)
    {
      int num = 32768;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None && this.IsJoystickAxisSupported(axis))
      {
        switch (axis)
        {
          case MyJoystickAxesEnum.Xpos:
          case MyJoystickAxesEnum.Xneg:
            num = this.m_actualJoystickState.X;
            break;
          case MyJoystickAxesEnum.Ypos:
          case MyJoystickAxesEnum.Yneg:
            num = this.m_actualJoystickState.Y;
            break;
          case MyJoystickAxesEnum.Zpos:
          case MyJoystickAxesEnum.Zneg:
            num = this.m_actualJoystickState.Z;
            break;
          case MyJoystickAxesEnum.RotationXpos:
          case MyJoystickAxesEnum.RotationXneg:
            num = this.m_actualJoystickState.RotationX;
            break;
          case MyJoystickAxesEnum.RotationYpos:
          case MyJoystickAxesEnum.RotationYneg:
            num = this.m_actualJoystickState.RotationY;
            break;
          case MyJoystickAxesEnum.RotationZpos:
          case MyJoystickAxesEnum.RotationZneg:
            num = this.m_actualJoystickState.RotationZ;
            break;
          case MyJoystickAxesEnum.Slider1pos:
          case MyJoystickAxesEnum.Slider1neg:
            // ISSUE: reference to a compiler-generated field
            num = this.m_actualJoystickState.Sliders.FixedElementField;
            break;
          case MyJoystickAxesEnum.Slider2pos:
          case MyJoystickAxesEnum.Slider2neg:
            num = this.m_actualJoystickState.Sliders[1];
            break;
          case MyJoystickAxesEnum.ZLeft:
            num = this.m_actualJoystickState.Z_Left;
            break;
          case MyJoystickAxesEnum.ZRight:
            num = this.m_actualJoystickState.Z_Right;
            break;
        }
      }
      return (float) num;
    }

    public unsafe float GetPreviousJoystickAxisStateRaw(MyJoystickAxesEnum axis)
    {
      int num = 32768;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None && this.IsJoystickAxisSupported(axis))
      {
        switch (axis)
        {
          case MyJoystickAxesEnum.Xpos:
          case MyJoystickAxesEnum.Xneg:
            num = this.m_previousJoystickState.X;
            break;
          case MyJoystickAxesEnum.Ypos:
          case MyJoystickAxesEnum.Yneg:
            num = this.m_previousJoystickState.Y;
            break;
          case MyJoystickAxesEnum.Zpos:
          case MyJoystickAxesEnum.Zneg:
            num = this.m_previousJoystickState.Z;
            break;
          case MyJoystickAxesEnum.RotationXpos:
          case MyJoystickAxesEnum.RotationXneg:
            num = this.m_previousJoystickState.RotationX;
            break;
          case MyJoystickAxesEnum.RotationYpos:
          case MyJoystickAxesEnum.RotationYneg:
            num = this.m_previousJoystickState.RotationY;
            break;
          case MyJoystickAxesEnum.RotationZpos:
          case MyJoystickAxesEnum.RotationZneg:
            num = this.m_previousJoystickState.RotationZ;
            break;
          case MyJoystickAxesEnum.Slider1pos:
          case MyJoystickAxesEnum.Slider1neg:
            // ISSUE: reference to a compiler-generated field
            num = this.m_previousJoystickState.Sliders.FixedElementField;
            break;
          case MyJoystickAxesEnum.Slider2pos:
          case MyJoystickAxesEnum.Slider2neg:
            num = this.m_previousJoystickState.Sliders[1];
            break;
          case MyJoystickAxesEnum.ZLeft:
            num = this.m_previousJoystickState.Z_Left;
            break;
          case MyJoystickAxesEnum.ZRight:
            num = this.m_previousJoystickState.Z_Right;
            break;
        }
      }
      return (float) num;
    }

    public float GetJoystickX() => this.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xpos);

    public float GetJoystickY() => this.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Ypos);

    public float GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
    {
      if (this.m_joystickConnected && this.IsJoystickAxisSupported(axis))
      {
        float num1 = (float) (((double) this.GetJoystickAxisStateRaw(axis) - (double) short.MaxValue) / (double) short.MaxValue);
        switch (axis)
        {
          case MyJoystickAxesEnum.Xpos:
          case MyJoystickAxesEnum.Ypos:
          case MyJoystickAxesEnum.Zpos:
          case MyJoystickAxesEnum.RotationXpos:
          case MyJoystickAxesEnum.RotationYpos:
          case MyJoystickAxesEnum.RotationZpos:
          case MyJoystickAxesEnum.Slider1pos:
          case MyJoystickAxesEnum.Slider2pos:
          case MyJoystickAxesEnum.ZLeft:
          case MyJoystickAxesEnum.ZRight:
            if ((double) num1 <= 0.0)
              return 0.0f;
            break;
          case MyJoystickAxesEnum.Xneg:
          case MyJoystickAxesEnum.Yneg:
          case MyJoystickAxesEnum.Zneg:
          case MyJoystickAxesEnum.RotationXneg:
          case MyJoystickAxesEnum.RotationYneg:
          case MyJoystickAxesEnum.RotationZneg:
          case MyJoystickAxesEnum.Slider1neg:
          case MyJoystickAxesEnum.Slider2neg:
            if ((double) num1 >= 0.0)
              return 0.0f;
            break;
        }
        float num2 = Math.Abs(num1);
        if ((double) num2 > (double) this.m_joystickDeadzone)
          return this.m_joystickSensitivity * (float) Math.Pow(((double) num2 - (double) this.m_joystickDeadzone) / (1.0 - (double) this.m_joystickDeadzone), (double) this.m_joystickExponent);
      }
      return 0.0f;
    }

    public float GetJoystickAxisStateForCarGameplay(MyJoystickAxesEnum axis)
    {
      if (axis != MyJoystickAxesEnum.Yneg && axis != MyJoystickAxesEnum.Ypos)
        return this.GetJoystickAxisStateForGameplay(axis);
      float num1 = 6553.501f;
      int joystickAxisStateRaw1 = (int) this.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
      int joystickAxisStateRaw2 = (int) this.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Yneg);
      if ((double) joystickAxisStateRaw1 > (double) num1 && (double) joystickAxisStateRaw1 < (double) ushort.MaxValue - (double) num1 && ((double) joystickAxisStateRaw2 > (double) num1 && (double) joystickAxisStateRaw2 < (double) ushort.MaxValue - (double) num1))
        return this.GetJoystickAxisStateForGameplay(axis);
      if (!this.m_joystickConnected || !this.IsJoystickAxisSupported(axis))
        return 0.0f;
      int joystickAxisStateRaw3 = (int) this.GetJoystickAxisStateRaw(axis);
      int num2 = (int) ((double) ushort.MaxValue * (double) this.m_joystickDeadzone);
      int num3 = (int) short.MaxValue - num2 / 2;
      int num4 = (int) short.MaxValue + num2 / 2;
      if (joystickAxisStateRaw3 > num3 && joystickAxisStateRaw3 < num4)
      {
        int joystickAxisStateRaw4 = (int) this.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
        if (joystickAxisStateRaw4 > num3 && joystickAxisStateRaw4 < num4)
        {
          if (this.m_isJoystickYAxisState_Reversing.HasValue)
            this.m_isJoystickYAxisState_Reversing = new bool?();
          return 0.0f;
        }
      }
      if (!this.m_isJoystickYAxisState_Reversing.HasValue)
        this.m_isJoystickYAxisState_Reversing = joystickAxisStateRaw3 < (int) short.MaxValue ? new bool?(false) : new bool?(true);
      float maxValue = (float) ushort.MaxValue;
      float num5 = 22937.25f;
      float num6 = (float) ushort.MaxValue - maxValue;
      float num7 = (float) ushort.MaxValue - num5;
      float num8 = num7 - num6;
      float num9;
      if (this.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 >= (double) maxValue || !this.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 <= (double) num6)
      {
        num9 = 1f;
      }
      else
      {
        if (this.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 < (double) num5 || !this.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 > (double) num7)
        {
          this.m_isJoystickYAxisState_Reversing = new bool?(!this.m_isJoystickYAxisState_Reversing.Value);
          return this.GetJoystickAxisStateForCarGameplay(axis);
        }
        float num10;
        if (axis == MyJoystickAxesEnum.Yneg)
        {
          double num11 = (double) Math.Abs(num6 - (float) joystickAxisStateRaw3);
          num10 = Math.Abs(num7 - (float) joystickAxisStateRaw3);
        }
        else
        {
          double num11 = (double) Math.Abs(maxValue - (float) joystickAxisStateRaw3);
          num10 = Math.Abs(num5 - (float) joystickAxisStateRaw3);
        }
        num9 = (float) ((double) num10 / ((double) num8 / 100.0) / 100.0);
        if ((double) num9 > 1.0)
          num9 = 1f;
      }
      return axis == MyJoystickAxesEnum.Yneg && this.m_isJoystickYAxisState_Reversing.Value || axis == MyJoystickAxesEnum.Ypos && !this.m_isJoystickYAxisState_Reversing.Value ? 0.0f : this.m_joystickSensitivity * (float) Math.Pow((double) num9, (double) this.m_joystickExponent);
    }

    public float GetPreviousJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
    {
      if (this.m_joystickConnected && this.IsJoystickAxisSupported(axis))
      {
        float num1 = (float) (((double) this.GetPreviousJoystickAxisStateRaw(axis) - (double) short.MaxValue) / (double) short.MaxValue);
        switch (axis)
        {
          case MyJoystickAxesEnum.Xpos:
          case MyJoystickAxesEnum.Ypos:
          case MyJoystickAxesEnum.Zpos:
          case MyJoystickAxesEnum.RotationXpos:
          case MyJoystickAxesEnum.RotationYpos:
          case MyJoystickAxesEnum.RotationZpos:
          case MyJoystickAxesEnum.Slider1pos:
          case MyJoystickAxesEnum.Slider2pos:
          case MyJoystickAxesEnum.ZLeft:
          case MyJoystickAxesEnum.ZRight:
            if ((double) num1 <= 0.0)
              return 0.0f;
            break;
          case MyJoystickAxesEnum.Xneg:
          case MyJoystickAxesEnum.Yneg:
          case MyJoystickAxesEnum.Zneg:
          case MyJoystickAxesEnum.RotationXneg:
          case MyJoystickAxesEnum.RotationYneg:
          case MyJoystickAxesEnum.RotationZneg:
          case MyJoystickAxesEnum.Slider1neg:
          case MyJoystickAxesEnum.Slider2neg:
            if ((double) num1 >= 0.0)
              return 0.0f;
            break;
        }
        float num2 = Math.Abs(num1);
        if ((double) num2 > (double) this.m_joystickDeadzone)
          return this.m_joystickSensitivity * (float) Math.Pow(((double) num2 - (double) this.m_joystickDeadzone) / (1.0 - (double) this.m_joystickDeadzone), (double) this.m_joystickExponent);
      }
      return 0.0f;
    }

    public Vector3 GetJoystickPositionForGameplay(RequestedJoystickAxis requestedAxis) => this.FilterAndNormalizeJoystickInput(new Vector3((float) this.m_actualJoystickState.X, (float) this.m_actualJoystickState.Y, (float) this.m_actualJoystickState.Z) - (float) short.MaxValue, requestedAxis);

    public Vector3 GetJoystickRotationForGameplay(RequestedJoystickAxis requestedAxis) => this.FilterAndNormalizeJoystickInput(new Vector3((float) (this.m_actualJoystickState.RotationX - (int) short.MaxValue), (float) (this.m_actualJoystickState.RotationY - (int) short.MaxValue), (float) this.m_actualJoystickState.RotationZ), requestedAxis);

    private Vector3 FilterAndNormalizeJoystickInput(
      Vector3 input,
      RequestedJoystickAxis requestedAxis)
    {
      input /= (float) short.MaxValue;
      if ((requestedAxis & RequestedJoystickAxis.X) == (RequestedJoystickAxis) 0)
        input.X = 0.0f;
      if ((requestedAxis & RequestedJoystickAxis.Y) == (RequestedJoystickAxis) 0)
        input.Y = 0.0f;
      if ((requestedAxis & RequestedJoystickAxis.Z) == (RequestedJoystickAxis) 0)
        input.Z = 0.0f;
      float num = input.Length();
      return (double) num <= (double) this.m_joystickDeadzone ? Vector3.Zero : this.m_joystickSensitivity * (float) Math.Pow(((double) num - (double) this.m_joystickDeadzone) / (1.0 - (double) this.m_joystickDeadzone), (double) this.m_joystickExponent) / num * input;
    }

    public bool IsJoystickAxisPressed(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetJoystickAxisStateForGameplay(axis) > 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool IsJoystickAxisNewPressed(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
      {
        double stateForGameplay1 = (double) this.GetJoystickAxisStateForGameplay(axis);
        float stateForGameplay2 = this.GetPreviousJoystickAxisStateForGameplay(axis);
        flag = stateForGameplay1 > 0.5 && (double) stateForGameplay2 <= 0.5;
      }
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool IsNewJoystickAxisReleased(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetJoystickAxisStateForGameplay(axis) <= 0.5 && (double) this.GetPreviousJoystickAxisStateForGameplay(axis) > 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool IsJoystickAxisReleased(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetJoystickAxisStateForGameplay(axis) <= 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool WasJoystickAxisPressed(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetPreviousJoystickAxisStateForGameplay(axis) > 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool WasJoystickAxisReleased(MyJoystickAxesEnum axis)
    {
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetPreviousJoystickAxisStateForGameplay(axis) <= 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public bool IsJoystickAxisNewPressedXinput(MyJoystickAxesEnum axis)
    {
      if (axis == MyJoystickAxesEnum.None)
        return true;
      if (!this.IsJoystickAxisSupported(axis))
        return false;
      switch (axis)
      {
        case MyJoystickAxesEnum.Zpos:
          axis = MyJoystickAxesEnum.ZLeft;
          break;
        case MyJoystickAxesEnum.Zneg:
          axis = MyJoystickAxesEnum.ZRight;
          break;
        default:
          return false;
      }
      if (!this.m_joystickConnected)
        return false;
      double stateForGameplay1 = (double) this.GetJoystickAxisStateForGameplay(axis);
      float stateForGameplay2 = this.GetPreviousJoystickAxisStateForGameplay(axis);
      return stateForGameplay1 > 0.5 && (double) stateForGameplay2 <= 0.5;
    }

    public bool IsNewJoystickAxisReleasedXinput(MyJoystickAxesEnum axis)
    {
      switch (axis)
      {
        case MyJoystickAxesEnum.Zpos:
          axis = MyJoystickAxesEnum.ZLeft;
          break;
        case MyJoystickAxesEnum.Zneg:
          axis = MyJoystickAxesEnum.ZRight;
          break;
        default:
          return false;
      }
      bool flag = false;
      if (this.m_joystickConnected && axis != MyJoystickAxesEnum.None)
        flag = (double) this.GetJoystickAxisStateForGameplay(axis) <= 0.5 && (double) this.GetPreviousJoystickAxisStateForGameplay(axis) > 0.5;
      if (!flag && axis == MyJoystickAxesEnum.None)
        return true;
      return this.IsJoystickAxisSupported(axis) && flag;
    }

    public float GetJoystickSensitivity() => this.m_joystickSensitivity;

    public void SetJoystickSensitivity(float newSensitivity) => this.m_joystickSensitivity = newSensitivity;

    public float GetJoystickExponent() => this.m_joystickExponent;

    public void SetJoystickExponent(float newExponent) => this.m_joystickExponent = newExponent;

    public float GetJoystickDeadzone() => this.m_joystickDeadzone;

    public void SetJoystickDeadzone(float newDeadzone) => this.m_joystickDeadzone = newDeadzone;

    public int MouseScrollWheelValue() => this.m_actualMouseState.ScrollWheelValue;

    public int PreviousMouseScrollWheelValue() => this.m_previousMouseState.ScrollWheelValue;

    public int DeltaMouseScrollWheelValue() => this.MouseScrollWheelValue() - this.PreviousMouseScrollWheelValue();

    public int GetMouseX() => this.m_actualMouseState.X;

    public int GetMouseY() => this.m_actualMouseState.Y;

    public int GetMouseXForGamePlay() => (int) ((double) this.m_mouseSensitivity * (double) ((this.m_mouseXIsInverted ? -1 : 1) * this.m_actualMouseState.X));

    public int GetMouseYForGamePlay() => (int) ((double) this.m_mouseSensitivity * (double) ((this.m_mouseYIsInverted ? -1 : 1) * this.m_actualMouseState.Y));

    public float GetMouseXForGamePlayF() => this.m_mouseSensitivity * ((this.m_mouseXIsInverted ? -1f : 1f) * (float) this.m_actualMouseState.X);

    public float GetMouseYForGamePlayF() => this.m_mouseSensitivity * ((this.m_mouseYIsInverted ? -1f : 1f) * (float) this.m_actualMouseState.Y);

    public bool GetMouseXInversion() => this.m_mouseXIsInverted;

    public bool GetMouseYInversion() => this.m_mouseYIsInverted;

    public bool GetMouseScrollBlockSelectionInversion() => this.m_mouseScrollBlockSelectionInverted;

    public void SetMouseXInversion(bool inverted) => this.m_mouseXIsInverted = inverted;

    public void SetMouseYInversion(bool inverted) => this.m_mouseYIsInverted = inverted;

    public void SetMouseScrollBlockSelectionInversion(bool inverted) => this.m_mouseScrollBlockSelectionInverted = inverted;

    public bool GetJoystickYInversionCharacter() => this.m_joystickYInvertedChar;

    public void SetJoystickYInversionCharacter(bool inverted) => this.m_joystickYInvertedChar = inverted;

    public bool GetJoystickYInversionVehicle() => this.m_joystickYInvertedVehicle;

    public void SetJoystickYInversionVehicle(bool inverted) => this.m_joystickYInvertedVehicle = inverted;

    public float GetMouseSensitivity() => this.m_mouseSensitivity;

    public void SetMouseSensitivity(float sensitivity) => this.m_mouseSensitivity = sensitivity;

    public void SetMousePositionScale(float scaleFactor) => this.m_mousePositionScale = scaleFactor;

    public Vector2 GetMousePosition() => (this.m_absoluteMousePosition - this.m_platformInput.MouseAreaSize / 2f * (1f - this.m_mousePositionScale)) / this.m_mousePositionScale;

    public Vector2 GetMouseAreaSize() => this.m_platformInput.MouseAreaSize;

    public void SetMousePosition(int x, int y) => this.m_platformInput.MousePosition = new Vector2((float) x, (float) y);

    public bool IsJoystickConnected() => this.m_joystickConnected;

    private void SetConnectedJoystick(string joystickInstanceName)
    {
      if (joystickInstanceName != null)
        this.m_joystickInstanceName = this.m_joystickInstanceNameForSearch = joystickInstanceName;
      bool flag = joystickInstanceName != null;
      if (this.m_joystickConnected == flag)
        return;
      this.m_joystickConnected = flag;
      Action<bool> joystickConnected = this.JoystickConnected;
      if (joystickConnected == null)
        return;
      joystickConnected(this.m_joystickConnected);
    }

    public bool JoystickAsMouse { get; set; }

    public bool IsJoystickLastUsed { get; set; }

    public event Action<bool> JoystickConnected;

    public unsafe bool GetGamepadKeyDirections(out int actual, out int previous)
    {
      if (this.m_joystickConnected)
      {
        // ISSUE: reference to a compiler-generated field
        actual = this.m_actualJoystickState.PointOfViewControllers.FixedElementField;
        // ISSUE: reference to a compiler-generated field
        previous = this.m_previousJoystickState.PointOfViewControllers.FixedElementField;
        return true;
      }
      actual = -1;
      previous = -1;
      return false;
    }

    public bool IsGamepadKeyRightPressed()
    {
      int actual;
      return this.GetGamepadKeyDirections(out actual, out int _) && actual >= 4500 && actual <= 13500;
    }

    public bool IsGamepadKeyLeftPressed()
    {
      int actual;
      return this.GetGamepadKeyDirections(out actual, out int _) && actual >= 22500 && actual <= 31500;
    }

    public bool IsGamepadKeyDownPressed()
    {
      int actual;
      return this.GetGamepadKeyDirections(out actual, out int _) && actual >= 13500 && actual <= 22500;
    }

    public bool IsGamepadKeyUpPressed()
    {
      int actual;
      if (!this.GetGamepadKeyDirections(out actual, out int _))
        return false;
      if (actual >= 0 && actual <= 4500)
        return true;
      return actual >= 31500 && actual <= 36000;
    }

    public bool WasGamepadKeyRightPressed()
    {
      int previous;
      return this.GetGamepadKeyDirections(out int _, out previous) && previous >= 4500 && previous <= 13500;
    }

    public bool WasGamepadKeyLeftPressed()
    {
      int previous;
      return this.GetGamepadKeyDirections(out int _, out previous) && previous >= 22500 && previous <= 31500;
    }

    public bool WasGamepadKeyDownPressed()
    {
      int previous;
      return this.GetGamepadKeyDirections(out int _, out previous) && previous >= 13500 && previous <= 22500;
    }

    public bool WasGamepadKeyUpPressed()
    {
      int previous;
      if (!this.GetGamepadKeyDirections(out int _, out previous))
        return false;
      if (previous >= 0 && previous <= 4500)
        return true;
      return previous >= 31500 && previous <= 36000;
    }

    public bool IsNewGamepadKeyRightPressed() => !this.WasGamepadKeyRightPressed() && this.IsGamepadKeyRightPressed();

    public bool IsNewGamepadKeyLeftPressed() => !this.WasGamepadKeyLeftPressed() && this.IsGamepadKeyLeftPressed();

    public bool IsNewGamepadKeyDownPressed() => !this.WasGamepadKeyDownPressed() && this.IsGamepadKeyDownPressed();

    public bool IsNewGamepadKeyUpPressed() => !this.WasGamepadKeyUpPressed() && this.IsGamepadKeyUpPressed();

    public bool IsNewGamepadKeyRightReleased() => this.WasGamepadKeyRightPressed() && !this.IsGamepadKeyRightPressed();

    public bool IsNewGamepadKeyLeftReleased() => this.WasGamepadKeyLeftPressed() && !this.IsGamepadKeyLeftPressed();

    public bool IsNewGamepadKeyDownReleased() => this.WasGamepadKeyDownPressed() && !this.IsGamepadKeyDownPressed();

    public bool IsNewGamepadKeyUpReleased() => this.WasGamepadKeyUpPressed() && !this.IsGamepadKeyUpPressed();

    public unsafe void GetActualJoystickState(StringBuilder text)
    {
      MyJoystickState actualJoystickState = this.m_actualJoystickState;
      text.Append("Supported axes: ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.Xpos))
        text.Append("X ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.Ypos))
        text.Append("Y ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.Zpos))
        text.Append("Z ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.RotationXpos))
        text.Append("Rx ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.RotationYpos))
        text.Append("Ry ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.RotationZpos))
        text.Append("Rz ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.Slider1pos))
        text.Append("S1 ");
      if (this.IsJoystickAxisSupported(MyJoystickAxesEnum.Slider2pos))
        text.Append("S2 ");
      text.AppendLine();
      text.Append("accX: ");
      text.AppendInt32(actualJoystickState.AccelerationX);
      text.AppendLine();
      text.Append("accY: ");
      text.AppendInt32(actualJoystickState.AccelerationY);
      text.AppendLine();
      text.Append("accZ: ");
      text.AppendInt32(actualJoystickState.AccelerationZ);
      text.AppendLine();
      text.Append("angAccX: ");
      text.AppendInt32(actualJoystickState.AngularAccelerationX);
      text.AppendLine();
      text.Append("angAccY: ");
      text.AppendInt32(actualJoystickState.AngularAccelerationY);
      text.AppendLine();
      text.Append("angAccZ: ");
      text.AppendInt32(actualJoystickState.AngularAccelerationZ);
      text.AppendLine();
      text.Append("angVelX: ");
      text.AppendInt32(actualJoystickState.AngularVelocityX);
      text.AppendLine();
      text.Append("angVelY: ");
      text.AppendInt32(actualJoystickState.AngularVelocityY);
      text.AppendLine();
      text.Append("angVelZ: ");
      text.AppendInt32(actualJoystickState.AngularVelocityZ);
      text.AppendLine();
      text.Append("forX: ");
      text.AppendInt32(actualJoystickState.ForceX);
      text.AppendLine();
      text.Append("forY: ");
      text.AppendInt32(actualJoystickState.ForceY);
      text.AppendLine();
      text.Append("forZ: ");
      text.AppendInt32(actualJoystickState.ForceZ);
      text.AppendLine();
      text.Append("rotX: ");
      text.AppendInt32(actualJoystickState.RotationX);
      text.AppendLine();
      text.Append("rotY: ");
      text.AppendInt32(actualJoystickState.RotationY);
      text.AppendLine();
      text.Append("rotZ: ");
      text.AppendInt32(actualJoystickState.RotationZ);
      text.AppendLine();
      text.Append("torqX: ");
      text.AppendInt32(actualJoystickState.TorqueX);
      text.AppendLine();
      text.Append("torqY: ");
      text.AppendInt32(actualJoystickState.TorqueY);
      text.AppendLine();
      text.Append("torqZ: ");
      text.AppendInt32(actualJoystickState.TorqueZ);
      text.AppendLine();
      text.Append("velX: ");
      text.AppendInt32(actualJoystickState.VelocityX);
      text.AppendLine();
      text.Append("velY: ");
      text.AppendInt32(actualJoystickState.VelocityY);
      text.AppendLine();
      text.Append("velZ: ");
      text.AppendInt32(actualJoystickState.VelocityZ);
      text.AppendLine();
      text.Append("X: ");
      text.AppendInt32(actualJoystickState.X);
      text.AppendLine();
      text.Append("Y: ");
      text.AppendInt32(actualJoystickState.Y);
      text.AppendLine();
      text.Append("Z: ");
      text.AppendInt32(actualJoystickState.Z);
      text.AppendLine();
      text.AppendLine();
      text.Append("AccSliders: ");
      for (int index = 0; index < 2; ++index)
      {
        text.AppendInt32(actualJoystickState.AccelerationSliders[index]);
        text.Append(" ");
      }
      text.AppendLine();
      text.Append("Buttons: ");
      for (int index = 0; index < 128; ++index)
      {
        text.Append(actualJoystickState.Buttons[index] > (byte) 0 ? "#" : "_");
        text.Append(" ");
      }
      text.AppendLine();
      text.Append("ForSliders: ");
      for (int index = 0; index < 2; ++index)
      {
        text.AppendInt32(actualJoystickState.ForceSliders[index]);
        text.Append(" ");
      }
      text.AppendLine();
      text.Append("POVControllers: ");
      for (int index = 0; index < 4; ++index)
      {
        text.AppendInt32(actualJoystickState.PointOfViewControllers[index]);
        text.Append(" ");
      }
      text.AppendLine();
      text.Append("Sliders: ");
      for (int index = 0; index < 2; ++index)
      {
        text.AppendInt32(actualJoystickState.Sliders[index]);
        text.Append(" ");
      }
      text.AppendLine();
      text.Append("VelocitySliders: ");
      for (int index = 0; index < 2; ++index)
      {
        text.AppendInt32(actualJoystickState.VelocitySliders[index]);
        text.Append(" ");
      }
      text.AppendLine();
    }

    public bool IsJoystickAxisSupported(MyJoystickAxesEnum axis) => this.m_joystickConnected && MyVRage.Platform.Input2.IsJoystickAxisSupported(axis);

    public bool IsNewGameControlPressed(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsNewPressed();
    }

    public bool IsGameControlPressed(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsPressed();
    }

    public bool IsNewGameControlReleased(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsNewReleased();
    }

    public float GetGameControlAnalogState(MyStringId controlId)
    {
      MyControl myControl;
      return this.IsControlBlocked(controlId) || !this.m_gameControlsList.TryGetValue(controlId, out myControl) ? 0.0f : myControl.GetAnalogState();
    }

    public bool IsGameControlReleased(MyStringId controlId)
    {
      MyControl myControl;
      return !this.IsControlBlocked(controlId) && this.m_gameControlsList.TryGetValue(controlId, out myControl) && myControl.IsNewReleased();
    }

    public bool IsKeyValid(MyKeys key)
    {
      foreach (MyKeys validKeyboardKey in this.m_validKeyboardKeys)
      {
        if (validKeyboardKey == key)
          return true;
      }
      return false;
    }

    public bool IsKeyDigit(MyKeys key) => this.m_digitKeys.Contains(key);

    public bool IsMouseButtonValid(MyMouseButtonsEnum button)
    {
      foreach (MyMouseButtonsEnum validMouseButton in this.m_validMouseButtons)
      {
        if (validMouseButton == button)
          return true;
      }
      return false;
    }

    public bool IsJoystickButtonValid(MyJoystickButtonsEnum button)
    {
      foreach (MyJoystickButtonsEnum validJoystickButton in this.m_validJoystickButtons)
      {
        if (validJoystickButton == button)
          return true;
      }
      return false;
    }

    public bool IsJoystickAxisValid(MyJoystickAxesEnum axis)
    {
      foreach (MyJoystickAxesEnum validJoystickAx in this.m_validJoystickAxes)
      {
        if (validJoystickAx == axis)
          return true;
      }
      return false;
    }

    public MyControl GetControl(MyKeys key)
    {
      foreach (MyControl myControl in this.m_gameControlsList.Values)
      {
        if (myControl.GetKeyboardControl() == key || myControl.GetSecondKeyboardControl() == key)
          return myControl;
      }
      return (MyControl) null;
    }

    public MyControl GetControl(MyMouseButtonsEnum button)
    {
      foreach (MyControl myControl in this.m_gameControlsList.Values)
      {
        if (myControl.GetMouseControl() == button)
          return myControl;
      }
      return (MyControl) null;
    }

    public void GetListOfPressedKeys(List<MyKeys> keys) => this.GetPressedKeys(keys);

    public void GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
    {
      result.Clear();
      if (this.IsLeftMousePressed())
        result.Add(MyMouseButtonsEnum.Left);
      if (this.IsRightMousePressed())
        result.Add(MyMouseButtonsEnum.Right);
      if (this.IsMiddleMousePressed())
        result.Add(MyMouseButtonsEnum.Middle);
      if (this.IsXButton1MousePressed())
        result.Add(MyMouseButtonsEnum.XButton1);
      if (!this.IsXButton2MousePressed())
        return;
      result.Add(MyMouseButtonsEnum.XButton2);
    }

    public DictionaryValuesReader<MyStringId, MyControl> GetGameControlsList() => (DictionaryValuesReader<MyStringId, MyControl>) this.m_gameControlsList;

    public void TakeSnapshot()
    {
      this.m_joystickInstanceNameSnapshot = this.JoystickInstanceName;
      this.CloneControls(this.m_gameControlsList, this.m_gameControlsSnapshot);
    }

    public void RevertChanges()
    {
      this.JoystickInstanceName = this.m_joystickInstanceNameSnapshot;
      this.CloneControls(this.m_gameControlsSnapshot, this.m_gameControlsList);
    }

    public string GetGameControlTextEnum(MyStringId controlId) => this.m_gameControlsList[controlId].GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);

    public MyControl GetGameControl(MyStringId controlId)
    {
      MyControl myControl;
      this.m_gameControlsList.TryGetValue(controlId, out myControl);
      return myControl;
    }

    private void CloneControls(
      Dictionary<MyStringId, MyControl> original,
      Dictionary<MyStringId, MyControl> copy)
    {
      foreach (KeyValuePair<MyStringId, MyControl> keyValuePair in original)
      {
        MyControl myControl;
        if (copy.TryGetValue(keyValuePair.Key, out myControl))
          myControl.CopyFrom(keyValuePair.Value);
        else
          copy[keyValuePair.Key] = new MyControl(keyValuePair.Value);
      }
    }

    public void SaveControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
      controlsGeneral.Dictionary.Clear();
      controlsGeneral.Dictionary.Add("mouseXIsInverted", this.m_mouseXIsInverted.ToString());
      controlsGeneral.Dictionary.Add("mouseYIsInverted", this.m_mouseYIsInverted.ToString());
      controlsGeneral.Dictionary.Add("mouseScrollBlockSelectionInverted", this.m_mouseScrollBlockSelectionInverted.ToString());
      controlsGeneral.Dictionary.Add("joystickYInvertedChar", this.m_joystickYInvertedChar.ToString());
      controlsGeneral.Dictionary.Add("joystickYInvertedVehicle", this.m_joystickYInvertedVehicle.ToString());
      controlsGeneral.Dictionary.Add("mouseSensitivity", this.m_mouseSensitivity.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      controlsGeneral.Dictionary.Add("joystickInstanceName", this.m_joystickInstanceName);
      controlsGeneral.Dictionary.Add("joystickSensitivity", this.m_joystickSensitivity.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      controlsGeneral.Dictionary.Add("joystickExponent", this.m_joystickExponent.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      controlsGeneral.Dictionary.Add("joystickDeadzone", this.m_joystickDeadzone.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      controlsButtons.Dictionary.Clear();
      foreach (MyControl myControl in this.m_gameControlsList.Values)
      {
        SerializableDictionary<string, string> serializableDictionary = new SerializableDictionary<string, string>();
        controlsButtons[myControl.GetGameControlEnum().ToString()] = serializableDictionary;
        serializableDictionary["Keyboard"] = myControl.GetKeyboardControl().ToString();
        serializableDictionary["Keyboard2"] = myControl.GetSecondKeyboardControl().ToString();
        serializableDictionary["Mouse"] = MyEnumsToStrings.MouseButtonsEnum[(int) myControl.GetMouseControl()];
      }
    }

    public bool LoadControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
      if (controlsGeneral.Dictionary.Count == 0)
      {
        MyLog.Default.WriteLine("    Loading default controls");
        this.RevertToDefaultControls();
        return false;
      }
      try
      {
        this.m_mouseXIsInverted = this.GetBoolFromDictionary(controlsGeneral, "mouseXIsInverted", false);
        this.m_mouseYIsInverted = this.GetBoolFromDictionary(controlsGeneral, "mouseYIsInverted", false);
        this.m_mouseScrollBlockSelectionInverted = this.GetBoolFromDictionary(controlsGeneral, "mouseScrollBlockSelectionInverted", false);
        this.m_joystickYInvertedChar = this.GetBoolFromDictionary(controlsGeneral, "joystickYInvertedChar", false);
        this.m_joystickYInvertedVehicle = this.GetBoolFromDictionary(controlsGeneral, "joystickYInvertedVehicle", false);
        this.m_mouseSensitivity = this.GetFloatFromDictionary(controlsGeneral, "mouseSensitivity", 1.655f);
        string str;
        this.JoystickInstanceName = !controlsGeneral.Dictionary.TryGetValue("joystickInstanceName", out str) ? (string) null : str;
        this.m_joystickSensitivity = this.GetFloatFromDictionary(controlsGeneral, "joystickSensitivity", 2f);
        this.m_joystickExponent = this.GetFloatFromDictionary(controlsGeneral, "joystickExponent", 2f);
        this.m_joystickDeadzone = this.GetFloatFromDictionary(controlsGeneral, "joystickDeadzone", 0.2f);
        this.LoadGameControls(controlsButtons);
        return true;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("    Error loading controls from config:");
        MyLog.Default.WriteLine(ex);
        MyLog.Default.WriteLine("    Loading default controls");
        this.RevertToDefaultControls();
        return false;
      }
    }

    public void RevertToDefaultControls()
    {
      this.m_mouseXIsInverted = false;
      this.m_mouseYIsInverted = false;
      this.m_mouseSensitivity = 1.655f;
      this.m_joystickYInvertedChar = false;
      this.m_joystickYInvertedVehicle = false;
      this.m_joystickSensitivity = 2f;
      this.m_joystickDeadzone = 0.2f;
      this.m_joystickExponent = 2f;
      this.CloneControls(this.m_defaultGameControlsList, this.m_gameControlsList);
    }

    private bool GetBoolFromDictionary(
      SerializableDictionary<string, string> controlsGeneral,
      string key,
      bool defaultValue)
    {
      string str;
      bool result;
      return controlsGeneral.Dictionary.TryGetValue(key, out str) && bool.TryParse(str, out result) ? result : defaultValue;
    }

    private float GetFloatFromDictionary(
      SerializableDictionary<string, string> controlsGeneral,
      string key,
      float defaultValue)
    {
      string s;
      float result;
      return controlsGeneral.Dictionary.TryGetValue(key, out s) && float.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : defaultValue;
    }

    private void LoadGameControls(
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
      if (controlsButtons.Dictionary.Count == 0)
        throw new Exception("ControlsButtons config parameter is empty.");
      foreach (KeyValuePair<string, SerializableDictionary<string, string>> keyValuePair in controlsButtons.Dictionary)
      {
        MyStringId? gameControlEnums = this.TryParseMyGameControlEnums(keyValuePair.Key);
        if (gameControlEnums.HasValue)
        {
          this.m_gameControlsList[gameControlEnums.Value].SetNoControl();
          SerializableDictionary<string, string> serializableDictionary = keyValuePair.Value;
          this.LoadGameControl(serializableDictionary["Keyboard"], gameControlEnums.Value, this.ParseMyGuiInputDeviceEnum("Keyboard"));
          this.LoadGameControl(serializableDictionary["Keyboard2"], gameControlEnums.Value, this.ParseMyGuiInputDeviceEnum("KeyboardSecond"));
          this.LoadGameControl(serializableDictionary["Mouse"], gameControlEnums.Value, this.ParseMyGuiInputDeviceEnum("Mouse"));
        }
      }
    }

    private void LoadGameControl(
      string controlName,
      MyStringId controlType,
      MyGuiInputDeviceEnum device)
    {
      switch (device)
      {
        case MyGuiInputDeviceEnum.Keyboard:
          MyKeys key1 = (MyKeys) Enum.Parse(typeof (MyKeys), controlName);
          if (!this.IsKeyValid(key1))
            throw new Exception("Key \"" + key1.ToString() + "\" is already assigned or is not valid.");
          this.FindNotAssignedGameControl(controlType, device).SetControl(MyGuiInputDeviceEnum.Keyboard, key1);
          break;
        case MyGuiInputDeviceEnum.Mouse:
          MyMouseButtonsEnum mouseButtonsEnum = this.ParseMyMouseButtonsEnum(controlName);
          if (!this.IsMouseButtonValid(mouseButtonsEnum))
            throw new Exception("Mouse button \"" + mouseButtonsEnum.ToString() + "\" is already assigned or is not valid.");
          this.FindNotAssignedGameControl(controlType, device).SetControl(mouseButtonsEnum);
          break;
        case MyGuiInputDeviceEnum.KeyboardSecond:
          MyKeys key2 = (MyKeys) Enum.Parse(typeof (MyKeys), controlName);
          if (!this.IsKeyValid(key2))
            throw new Exception("Key \"" + key2.ToString() + "\" is already assigned or is not valid.");
          this.FindNotAssignedGameControl(controlType, device).SetControl(MyGuiInputDeviceEnum.KeyboardSecond, key2);
          break;
      }
    }

    public MyGuiInputDeviceEnum ParseMyGuiInputDeviceEnum(string s)
    {
      for (int index = 0; index < MyEnumsToStrings.GuiInputDeviceEnum.Length; ++index)
      {
        if (MyEnumsToStrings.GuiInputDeviceEnum[index] == s)
          return (MyGuiInputDeviceEnum) index;
      }
      throw new ArgumentException("Value \"" + s + "\" is not from GuiInputDeviceEnum.", nameof (s));
    }

    public MyJoystickButtonsEnum ParseMyJoystickButtonsEnum(string s)
    {
      for (int index = 0; index < MyEnumsToStrings.JoystickButtonsEnum.Length; ++index)
      {
        if (MyEnumsToStrings.JoystickButtonsEnum[index] == s)
          return (MyJoystickButtonsEnum) index;
      }
      throw new ArgumentException("Value \"" + s + "\" is not from JoystickButtonsEnum.", nameof (s));
    }

    public MyJoystickAxesEnum ParseMyJoystickAxesEnum(string s)
    {
      for (int index = 0; index < MyEnumsToStrings.JoystickAxesEnum.Length; ++index)
      {
        if (MyEnumsToStrings.JoystickAxesEnum[index] == s)
          return (MyJoystickAxesEnum) index;
      }
      throw new ArgumentException("Value \"" + s + "\" is not from JoystickAxesEnum.", nameof (s));
    }

    public MyMouseButtonsEnum ParseMyMouseButtonsEnum(string s)
    {
      for (int index = 0; index < MyEnumsToStrings.MouseButtonsEnum.Length; ++index)
      {
        if (MyEnumsToStrings.MouseButtonsEnum[index] == s)
          return (MyMouseButtonsEnum) index;
      }
      throw new ArgumentException("Value \"" + s + "\" is not from MouseButtonsEnum.", nameof (s));
    }

    public MyStringId? TryParseMyGameControlEnums(string s)
    {
      MyStringId orCompute = MyStringId.GetOrCompute(s);
      return this.m_gameControlsList.ContainsKey(orCompute) ? new MyStringId?(orCompute) : new MyStringId?();
    }

    public MyGuiControlTypeEnum ParseMyGuiControlTypeEnum(string s)
    {
      for (int index = 0; index < MyEnumsToStrings.ControlTypeEnum.Length; ++index)
      {
        if (MyEnumsToStrings.ControlTypeEnum[index] == s)
          return (MyGuiControlTypeEnum) index;
      }
      throw new ArgumentException("Value \"" + s + "\" is not from MyGuiInputTypeEnum.", nameof (s));
    }

    private MyControl FindNotAssignedGameControl(
      MyStringId controlId,
      MyGuiInputDeviceEnum deviceType)
    {
      MyControl myControl;
      if (!this.m_gameControlsList.TryGetValue(controlId, out myControl))
        throw new Exception("Game control \"" + controlId.ToString() + "\" not found in control list.");
      return !myControl.IsControlAssigned(deviceType) ? myControl : throw new Exception("Game control \"" + controlId.ToString() + "\" is already assigned.");
    }

    public string GetKeyName(MyStringId controlId) => this.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);

    public bool Trichording { get; set; }

    public bool IsDirectInputInitialized { get; private set; }

    public string GetKeyName(MyKeys key) => this.m_nameLookup.GetKeyName(key);

    public string GetName(MyMouseButtonsEnum mouseButton) => this.m_nameLookup.GetName(mouseButton);

    public string GetName(MyJoystickButtonsEnum joystickButton) => this.m_nameLookup.GetName(joystickButton);

    public string GetName(MyJoystickAxesEnum joystickAxis) => this.m_nameLookup.GetName(joystickAxis);

    public string GetUnassignedName() => this.m_nameLookup.UnassignedText;

    public void SetControlBlock(MyStringId controlEnum, bool block = false)
    {
      if (block)
        this.m_gameControlsBlacklist.Add(controlEnum);
      else
        this.m_gameControlsBlacklist.Remove(controlEnum);
    }

    public bool IsControlBlocked(MyStringId controlEnum) => this.m_gameControlsBlacklist.Contains(controlEnum);

    public void ClearBlacklist() => this.m_gameControlsBlacklist.Clear();

    public bool IsMouseMoved() => this.m_actualMouseState.X != this.m_previousMouseState.X || this.m_actualMouseState.Y != this.m_previousMouseState.Y;

    public bool IsScrolled() => this.MouseScrollWheelValue() != this.PreviousMouseScrollWheelValue();
  }
}
