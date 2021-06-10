// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyNullInput
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Input.Keyboard;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
  public class MyNullInput : IMyInput, VRage.ModAPI.IMyInput
  {
    private MyControl m_nullControl = new MyControl(new MyStringId(), new MyStringId(), MyGuiControlTypeEnum.General, new MyMouseButtonsEnum?(), new MyKeys?());
    private List<char> m_listChars = new List<char>();
    private List<string> m_listStrings = new List<string>();

    string IMyInput.JoystickInstanceName
    {
      get => "";
      set
      {
      }
    }

    public void SearchForJoystick()
    {
    }

    void IMyInput.LoadData(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
    }

    public bool LoadControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
      return true;
    }

    void IMyInput.LoadContent()
    {
    }

    ListReader<char> IMyInput.TextInput => (ListReader<char>) this.m_listChars;

    void IMyInput.UnloadData()
    {
    }

    List<string> IMyInput.EnumerateJoystickNames() => this.m_listStrings;

    bool IMyInput.Update(bool gameFocused) => false;

    public void SetControlBlock(MyStringId controlEnum, bool block = false)
    {
    }

    public bool IsControlBlocked(MyStringId controlEnum) => false;

    public void ClearBlacklist()
    {
    }

    bool IMyInput.IsAnyKeyPress() => false;

    bool IMyInput.IsAnyMousePressed() => false;

    bool IMyInput.IsAnyNewMousePressed() => false;

    bool IMyInput.IsAnyShiftKeyPressed() => false;

    bool IMyInput.IsAnyAltKeyPressed() => false;

    bool IMyInput.IsAnyCtrlKeyPressed() => false;

    void IMyInput.GetPressedKeys(List<MyKeys> keys)
    {
    }

    bool IMyInput.IsKeyPress(MyKeys key) => false;

    bool IMyInput.WasKeyPress(MyKeys key) => false;

    bool IMyInput.IsNewKeyPressed(MyKeys key) => false;

    bool IMyInput.IsNewKeyReleased(MyKeys key) => false;

    bool IMyInput.IsMousePressed(MyMouseButtonsEnum button) => false;

    bool IMyInput.IsMouseReleased(MyMouseButtonsEnum button) => false;

    bool IMyInput.IsNewMousePressed(MyMouseButtonsEnum button) => false;

    bool IMyInput.IsNewLeftMousePressed() => false;

    bool IMyInput.IsNewLeftMouseReleased() => false;

    bool IMyInput.IsLeftMousePressed() => false;

    bool IMyInput.IsLeftMouseReleased() => false;

    bool IMyInput.IsRightMousePressed() => false;

    bool IMyInput.IsNewRightMousePressed() => false;

    bool IMyInput.IsNewRightMouseReleased() => false;

    bool IMyInput.WasRightMousePressed() => false;

    bool IMyInput.WasRightMouseReleased() => false;

    bool IMyInput.IsMiddleMousePressed() => false;

    bool IMyInput.IsNewMiddleMousePressed() => false;

    bool IMyInput.IsNewMiddleMouseReleased() => false;

    bool IMyInput.WasMiddleMousePressed() => false;

    bool IMyInput.WasMiddleMouseReleased() => false;

    bool IMyInput.IsXButton1MousePressed() => false;

    bool IMyInput.IsNewXButton1MousePressed() => false;

    bool IMyInput.IsNewXButton1MouseReleased() => false;

    bool IMyInput.WasXButton1MousePressed() => false;

    bool IMyInput.WasXButton1MouseReleased() => false;

    bool IMyInput.IsXButton2MousePressed() => false;

    bool IMyInput.IsNewXButton2MousePressed() => false;

    bool IMyInput.IsNewXButton2MouseReleased() => false;

    bool IMyInput.WasXButton2MousePressed() => false;

    bool IMyInput.WasXButton2MouseReleased() => false;

    bool IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button) => false;

    bool IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button) => false;

    bool IMyInput.IsJoystickButtonNewReleased(MyJoystickButtonsEnum button) => false;

    float IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis) => 0.0f;

    public Vector3 GetJoystickPositionForGameplay(RequestedJoystickAxis requestedAxis) => Vector3.Zero;

    public Vector3 GetJoystickRotationForGameplay(RequestedJoystickAxis requestedAxis) => Vector3.Zero;

    public Vector2 GetJoystickAxesStateForGameplay(
      MyJoystickAxesEnum axis1,
      MyJoystickAxesEnum axis2)
    {
      return Vector2.Zero;
    }

    bool IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis) => false;

    bool IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis) => false;

    bool IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis) => false;

    bool IMyInput.IsNewGameControlJoystickOnlyPressed(MyStringId controlId) => false;

    public void DeviceChangeCallback()
    {
    }

    public void NegateEscapePress()
    {
    }

    public bool IsJoystickIdle() => true;

    public string ReplaceControlsInText(string text) => throw new NotImplementedException();

    float IMyInput.GetJoystickSensitivity() => 0.0f;

    void IMyInput.SetJoystickSensitivity(float newSensitivity)
    {
    }

    float IMyInput.GetJoystickExponent() => 0.0f;

    void IMyInput.SetJoystickExponent(float newExponent)
    {
    }

    float IMyInput.GetJoystickDeadzone() => 0.0f;

    void IMyInput.SetJoystickDeadzone(float newDeadzone)
    {
    }

    int IMyInput.MouseScrollWheelValue() => 0;

    int IMyInput.PreviousMouseScrollWheelValue() => 0;

    int IMyInput.DeltaMouseScrollWheelValue() => 0;

    int IMyInput.GetMouseXForGamePlay() => 0;

    int IMyInput.GetMouseYForGamePlay() => 0;

    float IMyInput.GetMouseXForGamePlayF() => 0.0f;

    float IMyInput.GetMouseYForGamePlayF() => 0.0f;

    int IMyInput.GetMouseX() => 0;

    int IMyInput.GetMouseY() => 0;

    bool IMyInput.GetMouseXInversion() => false;

    bool IMyInput.GetMouseYInversion() => false;

    bool IMyInput.GetMouseScrollBlockSelectionInversion() => false;

    void IMyInput.SetMouseXInversion(bool inverted)
    {
    }

    void IMyInput.SetMouseYInversion(bool inverted)
    {
    }

    void IMyInput.SetMouseScrollBlockSelectionInversion(bool inverted)
    {
    }

    float IMyInput.GetMouseSensitivity() => 0.0f;

    void IMyInput.SetMouseSensitivity(float sensitivity)
    {
    }

    public void SetMousePositionScale(float scaleFactor)
    {
    }

    Vector2 IMyInput.GetMousePosition() => Vector2.Zero;

    void IMyInput.SetMousePosition(int x, int y)
    {
    }

    bool IMyInput.IsGamepadKeyRightPressed() => false;

    bool IMyInput.IsGamepadKeyLeftPressed() => false;

    bool IMyInput.IsNewGamepadKeyDownPressed() => false;

    bool IMyInput.IsNewGamepadKeyUpPressed() => false;

    void IMyInput.GetActualJoystickState(StringBuilder text)
    {
    }

    bool IMyInput.IsAnyMouseOrJoystickPressed() => false;

    bool IMyInput.IsAnyNewMouseOrJoystickPressed() => false;

    bool IMyInput.IsNewPrimaryButtonPressed() => false;

    bool IMyInput.IsNewSecondaryButtonPressed() => false;

    bool IMyInput.IsNewPrimaryButtonReleased() => false;

    bool IMyInput.IsNewSecondaryButtonReleased() => false;

    bool IMyInput.IsPrimaryButtonReleased() => false;

    bool IMyInput.IsSecondaryButtonReleased() => false;

    bool IMyInput.IsPrimaryButtonPressed() => false;

    bool IMyInput.IsSecondaryButtonPressed() => false;

    bool IMyInput.IsNewButtonPressed(MySharedButtonsEnum button) => false;

    bool IMyInput.IsButtonPressed(MySharedButtonsEnum button) => false;

    bool IMyInput.IsNewButtonReleased(MySharedButtonsEnum button) => false;

    bool IMyInput.IsButtonReleased(MySharedButtonsEnum button) => false;

    bool IMyInput.IsNewGameControlPressed(MyStringId controlEnum) => false;

    bool IMyInput.IsGameControlPressed(MyStringId controlEnum) => false;

    bool IMyInput.IsNewGameControlReleased(MyStringId controlEnum) => false;

    float IMyInput.GetGameControlAnalogState(MyStringId controlEnum) => 0.0f;

    bool IMyInput.IsGameControlReleased(MyStringId controlEnum) => false;

    bool IMyInput.IsKeyValid(MyKeys key) => false;

    bool IMyInput.IsKeyDigit(MyKeys key) => false;

    bool IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button) => false;

    bool IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button) => false;

    bool IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis) => false;

    bool IMyInput.IsJoystickConnected() => false;

    bool IMyInput.JoystickAsMouse
    {
      get => false;
      set
      {
      }
    }

    bool IMyInput.IsJoystickLastUsed
    {
      get => false;
      set
      {
      }
    }

    event Action<bool> IMyInput.JoystickConnected
    {
      add
      {
      }
      remove
      {
      }
    }

    MyControl IMyInput.GetControl(MyKeys key) => (MyControl) null;

    MyControl IMyInput.GetControl(MyMouseButtonsEnum button) => (MyControl) null;

    void IMyInput.GetListOfPressedKeys(List<MyKeys> keys)
    {
    }

    void IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
    {
    }

    DictionaryValuesReader<MyStringId, MyControl> IMyInput.GetGameControlsList() => (DictionaryValuesReader<MyStringId, MyControl>) (Dictionary<MyStringId, MyControl>) null;

    void IMyInput.TakeSnapshot()
    {
    }

    void IMyInput.RevertChanges()
    {
    }

    MyControl IMyInput.GetGameControl(MyStringId controlEnum) => this.m_nullControl;

    void IMyInput.RevertToDefaultControls()
    {
    }

    void IMyInput.SaveControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
    {
    }

    public bool OverrideUpdate
    {
      get => false;
      set
      {
      }
    }

    public MyMouseState ActualMouseState => new MyMouseState();

    public MyJoystickState ActualJoystickState => new MyJoystickState();

    public bool IsDirectInputInitialized => true;

    Vector2 IMyInput.GetMouseAreaSize() => Vector2.Zero;

    string IMyInput.GetName(MyMouseButtonsEnum mouseButton) => "";

    string IMyInput.GetName(MyJoystickButtonsEnum joystickButton) => "";

    string IMyInput.GetName(MyJoystickAxesEnum joystickAxis) => "";

    string IMyInput.GetUnassignedName() => "";

    string IMyInput.GetKeyName(MyKeys key) => "";

    public void UpdateStates()
    {
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
    }

    public void ClearStates()
    {
    }

    public void UpdateJoystickChanged()
    {
    }

    public bool IsMouseMoved() => false;

    public bool GetJoystickYInversionCharacter() => false;

    public void SetJoystickYInversionCharacter(bool inverted)
    {
    }

    public bool GetJoystickYInversionVehicle() => false;

    public void SetJoystickYInversionVehicle(bool inverted)
    {
    }

    public bool IsJoystickAxisNewPressedXinput(MyJoystickAxesEnum axis) => throw new NotImplementedException();

    public bool IsNewJoystickAxisReleasedXinput(MyJoystickAxesEnum axis) => throw new NotImplementedException();

    string VRage.ModAPI.IMyInput.JoystickInstanceName => ((IMyInput) this).JoystickInstanceName;

    ListReader<char> VRage.ModAPI.IMyInput.TextInput => ((IMyInput) this).TextInput;

    List<string> VRage.ModAPI.IMyInput.EnumerateJoystickNames() => ((IMyInput) this).EnumerateJoystickNames();

    bool VRage.ModAPI.IMyInput.IsAnyKeyPress() => ((IMyInput) this).IsAnyKeyPress();

    bool VRage.ModAPI.IMyInput.IsAnyMousePressed() => ((IMyInput) this).IsAnyMousePressed();

    bool VRage.ModAPI.IMyInput.IsAnyNewMousePressed() => ((IMyInput) this).IsAnyNewMousePressed();

    bool VRage.ModAPI.IMyInput.IsAnyShiftKeyPressed() => ((IMyInput) this).IsAnyShiftKeyPressed();

    bool VRage.ModAPI.IMyInput.IsAnyAltKeyPressed() => ((IMyInput) this).IsAnyAltKeyPressed();

    bool VRage.ModAPI.IMyInput.IsAnyCtrlKeyPressed() => ((IMyInput) this).IsAnyCtrlKeyPressed();

    void VRage.ModAPI.IMyInput.GetPressedKeys(List<MyKeys> keys) => ((IMyInput) this).GetPressedKeys(keys);

    public void AddDefaultControl(MyStringId stringId, MyControl control)
    {
    }

    bool VRage.ModAPI.IMyInput.IsKeyPress(MyKeys key) => ((IMyInput) this).IsKeyPress(key);

    bool VRage.ModAPI.IMyInput.WasKeyPress(MyKeys key) => ((IMyInput) this).WasKeyPress(key);

    bool VRage.ModAPI.IMyInput.IsNewKeyPressed(MyKeys key) => ((IMyInput) this).IsNewKeyPressed(key);

    bool VRage.ModAPI.IMyInput.IsNewKeyReleased(MyKeys key) => ((IMyInput) this).IsNewKeyReleased(key);

    bool VRage.ModAPI.IMyInput.IsMousePressed(MyMouseButtonsEnum button) => ((IMyInput) this).IsMousePressed(button);

    bool VRage.ModAPI.IMyInput.IsMouseReleased(MyMouseButtonsEnum button) => ((IMyInput) this).IsMouseReleased(button);

    bool VRage.ModAPI.IMyInput.IsNewMousePressed(MyMouseButtonsEnum button) => ((IMyInput) this).IsNewMousePressed(button);

    bool VRage.ModAPI.IMyInput.IsNewLeftMousePressed() => ((IMyInput) this).IsNewLeftMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewLeftMouseReleased() => ((IMyInput) this).IsNewLeftMouseReleased();

    bool VRage.ModAPI.IMyInput.IsLeftMousePressed() => ((IMyInput) this).IsLeftMousePressed();

    bool VRage.ModAPI.IMyInput.IsLeftMouseReleased() => ((IMyInput) this).IsLeftMouseReleased();

    bool VRage.ModAPI.IMyInput.IsRightMousePressed() => ((IMyInput) this).IsRightMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewRightMousePressed() => ((IMyInput) this).IsNewRightMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewRightMouseReleased() => ((IMyInput) this).IsNewRightMouseReleased();

    bool VRage.ModAPI.IMyInput.WasRightMousePressed() => ((IMyInput) this).WasRightMousePressed();

    bool VRage.ModAPI.IMyInput.WasRightMouseReleased() => ((IMyInput) this).WasRightMouseReleased();

    bool VRage.ModAPI.IMyInput.IsMiddleMousePressed() => ((IMyInput) this).IsMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewMiddleMousePressed() => ((IMyInput) this).IsNewMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.IsNewMiddleMouseReleased() => ((IMyInput) this).IsNewMiddleMouseReleased();

    bool VRage.ModAPI.IMyInput.WasMiddleMousePressed() => ((IMyInput) this).WasMiddleMousePressed();

    bool VRage.ModAPI.IMyInput.WasMiddleMouseReleased() => ((IMyInput) this).WasMiddleMouseReleased();

    bool VRage.ModAPI.IMyInput.IsXButton1MousePressed() => ((IMyInput) this).IsXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton1MousePressed() => ((IMyInput) this).IsNewXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton1MouseReleased() => ((IMyInput) this).IsNewXButton1MouseReleased();

    bool VRage.ModAPI.IMyInput.WasXButton1MousePressed() => ((IMyInput) this).WasXButton1MousePressed();

    bool VRage.ModAPI.IMyInput.WasXButton1MouseReleased() => ((IMyInput) this).WasXButton1MouseReleased();

    bool VRage.ModAPI.IMyInput.IsXButton2MousePressed() => ((IMyInput) this).IsXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton2MousePressed() => ((IMyInput) this).IsNewXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.IsNewXButton2MouseReleased() => ((IMyInput) this).IsNewXButton2MouseReleased();

    bool VRage.ModAPI.IMyInput.WasXButton2MousePressed() => ((IMyInput) this).WasXButton2MousePressed();

    bool VRage.ModAPI.IMyInput.WasXButton2MouseReleased() => ((IMyInput) this).WasXButton2MouseReleased();

    bool VRage.ModAPI.IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button) => ((IMyInput) this).IsJoystickButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button) => ((IMyInput) this).IsJoystickButtonNewPressed(button);

    bool VRage.ModAPI.IMyInput.IsNewJoystickButtonReleased(MyJoystickButtonsEnum button) => ((IMyInput) this).IsJoystickButtonNewReleased(button);

    float VRage.ModAPI.IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis) => ((IMyInput) this).GetJoystickAxisStateForGameplay(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis) => ((IMyInput) this).IsJoystickAxisPressed(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis) => ((IMyInput) this).IsJoystickAxisNewPressed(axis);

    bool VRage.ModAPI.IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis) => ((IMyInput) this).IsNewJoystickAxisReleased(axis);

    bool VRage.ModAPI.IMyInput.IsAnyMouseOrJoystickPressed() => ((IMyInput) this).IsAnyMouseOrJoystickPressed();

    bool VRage.ModAPI.IMyInput.IsAnyNewMouseOrJoystickPressed() => ((IMyInput) this).IsAnyNewMouseOrJoystickPressed();

    bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonPressed() => ((IMyInput) this).IsNewPrimaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonPressed() => ((IMyInput) this).IsNewSecondaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonReleased() => ((IMyInput) this).IsNewPrimaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonReleased() => ((IMyInput) this).IsNewSecondaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsPrimaryButtonReleased() => ((IMyInput) this).IsPrimaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsSecondaryButtonReleased() => ((IMyInput) this).IsSecondaryButtonReleased();

    bool VRage.ModAPI.IMyInput.IsPrimaryButtonPressed() => ((IMyInput) this).IsPrimaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsSecondaryButtonPressed() => ((IMyInput) this).IsSecondaryButtonPressed();

    bool VRage.ModAPI.IMyInput.IsNewButtonPressed(MySharedButtonsEnum button) => ((IMyInput) this).IsNewButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsButtonPressed(MySharedButtonsEnum button) => ((IMyInput) this).IsButtonPressed(button);

    bool VRage.ModAPI.IMyInput.IsNewButtonReleased(MySharedButtonsEnum button) => ((IMyInput) this).IsNewButtonReleased(button);

    bool VRage.ModAPI.IMyInput.IsButtonReleased(MySharedButtonsEnum button) => ((IMyInput) this).IsButtonReleased(button);

    int VRage.ModAPI.IMyInput.MouseScrollWheelValue() => ((IMyInput) this).MouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.PreviousMouseScrollWheelValue() => ((IMyInput) this).PreviousMouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.DeltaMouseScrollWheelValue() => ((IMyInput) this).DeltaMouseScrollWheelValue();

    int VRage.ModAPI.IMyInput.GetMouseXForGamePlay() => ((IMyInput) this).GetMouseXForGamePlay();

    int VRage.ModAPI.IMyInput.GetMouseYForGamePlay() => ((IMyInput) this).GetMouseYForGamePlay();

    int VRage.ModAPI.IMyInput.GetMouseX() => ((IMyInput) this).GetMouseX();

    int VRage.ModAPI.IMyInput.GetMouseY() => ((IMyInput) this).GetMouseY();

    bool VRage.ModAPI.IMyInput.GetMouseXInversion() => ((IMyInput) this).GetMouseXInversion();

    bool VRage.ModAPI.IMyInput.GetMouseYInversion() => ((IMyInput) this).GetMouseYInversion();

    float VRage.ModAPI.IMyInput.GetMouseSensitivity() => ((IMyInput) this).GetMouseSensitivity();

    Vector2 VRage.ModAPI.IMyInput.GetMousePosition() => ((IMyInput) this).GetMousePosition();

    Vector2 VRage.ModAPI.IMyInput.GetMouseAreaSize() => ((IMyInput) this).GetMouseAreaSize();

    bool VRage.ModAPI.IMyInput.IsNewGameControlPressed(MyStringId controlEnum) => ((IMyInput) this).IsNewGameControlPressed(controlEnum);

    bool VRage.ModAPI.IMyInput.IsGameControlPressed(MyStringId controlEnum) => ((IMyInput) this).IsGameControlPressed(controlEnum);

    bool VRage.ModAPI.IMyInput.IsNewGameControlReleased(MyStringId controlEnum) => ((IMyInput) this).IsNewGameControlReleased(controlEnum);

    float VRage.ModAPI.IMyInput.GetGameControlAnalogState(MyStringId controlEnum) => ((IMyInput) this).GetGameControlAnalogState(controlEnum);

    bool VRage.ModAPI.IMyInput.IsGameControlReleased(MyStringId controlEnum) => ((IMyInput) this).IsGameControlReleased(controlEnum);

    bool VRage.ModAPI.IMyInput.IsKeyValid(MyKeys key) => ((IMyInput) this).IsKeyValid(key);

    bool VRage.ModAPI.IMyInput.IsKeyDigit(MyKeys key) => ((IMyInput) this).IsKeyDigit(key);

    bool VRage.ModAPI.IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button) => ((IMyInput) this).IsMouseButtonValid(button);

    bool VRage.ModAPI.IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button) => ((IMyInput) this).IsJoystickButtonValid(button);

    bool VRage.ModAPI.IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis) => ((IMyInput) this).IsJoystickAxisValid(axis);

    bool VRage.ModAPI.IMyInput.IsJoystickConnected() => ((IMyInput) this).IsJoystickConnected();

    bool VRage.ModAPI.IMyInput.JoystickAsMouse => ((IMyInput) this).JoystickAsMouse;

    bool VRage.ModAPI.IMyInput.IsJoystickLastUsed => ((IMyInput) this).IsJoystickLastUsed;

    event Action<bool> VRage.ModAPI.IMyInput.JoystickConnected
    {
      add => ((IMyInput) this).JoystickConnected += value;
      remove => ((IMyInput) this).JoystickConnected -= value;
    }

    IMyControl VRage.ModAPI.IMyInput.GetControl(MyKeys key) => (IMyControl) ((IMyInput) this).GetControl(key);

    IMyControl VRage.ModAPI.IMyInput.GetControl(MyMouseButtonsEnum button) => (IMyControl) ((IMyInput) this).GetControl(button);

    void VRage.ModAPI.IMyInput.GetListOfPressedKeys(List<MyKeys> keys) => ((IMyInput) this).GetListOfPressedKeys(keys);

    void VRage.ModAPI.IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result) => ((IMyInput) this).GetListOfPressedMouseButtons(result);

    IMyControl VRage.ModAPI.IMyInput.GetGameControl(MyStringId controlEnum) => (IMyControl) ((IMyInput) this).GetGameControl(controlEnum);

    string VRage.ModAPI.IMyInput.GetKeyName(MyKeys key) => ((IMyInput) this).GetKeyName(key);

    string VRage.ModAPI.IMyInput.GetName(MyMouseButtonsEnum mouseButton) => ((IMyInput) this).GetName(mouseButton);

    string VRage.ModAPI.IMyInput.GetName(MyJoystickButtonsEnum joystickButton) => ((IMyInput) this).GetName(joystickButton);

    string VRage.ModAPI.IMyInput.GetName(MyJoystickAxesEnum joystickAxis) => ((IMyInput) this).GetName(joystickAxis);

    string VRage.ModAPI.IMyInput.GetUnassignedName() => ((IMyInput) this).GetUnassignedName();

    public void EnableInput(bool enable)
    {
    }

    public bool IsEnabled() => false;
  }
}
