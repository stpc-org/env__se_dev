﻿// Decompiled with JetBrains decompiler
// Type: VRage.Input.IMyInput
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Input.Keyboard;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
  public interface IMyInput
  {
    string JoystickInstanceName { get; set; }

    void SearchForJoystick();

    void LoadData(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons);

    bool LoadControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons);

    void LoadContent();

    ListReader<char> TextInput { get; }

    bool OverrideUpdate { get; set; }

    MyMouseState ActualMouseState { get; }

    MyJoystickState ActualJoystickState { get; }

    void UnloadData();

    List<string> EnumerateJoystickNames();

    bool Update(bool gameFocused);

    void UpdateJoystickChanged();

    void UpdateStates();

    void UpdateStatesFromPlayback(
      MyKeyboardState currentKeyboard,
      MyKeyboardState previousKeyboard,
      MyMouseState currentMouse,
      MyMouseState previousMouse,
      MyJoystickState currentJoystick,
      MyJoystickState previousJoystick,
      int x,
      int y,
      List<char> keyboardSnapshotText);

    void ClearStates();

    void SetControlBlock(MyStringId controlEnum, bool block = false);

    bool IsControlBlocked(MyStringId controlEnum);

    void ClearBlacklist();

    bool IsAnyKeyPress();

    bool IsAnyMousePressed();

    bool IsAnyNewMousePressed();

    bool IsAnyShiftKeyPressed();

    bool IsAnyAltKeyPressed();

    bool IsAnyCtrlKeyPressed();

    void GetPressedKeys(List<MyKeys> keys);

    bool IsKeyPress(MyKeys key);

    bool WasKeyPress(MyKeys key);

    bool IsNewKeyPressed(MyKeys key);

    bool IsNewKeyReleased(MyKeys key);

    bool IsMousePressed(MyMouseButtonsEnum button);

    bool IsMouseReleased(MyMouseButtonsEnum button);

    bool IsNewMousePressed(MyMouseButtonsEnum button);

    bool IsNewLeftMousePressed();

    bool IsNewLeftMouseReleased();

    bool IsLeftMousePressed();

    bool IsLeftMouseReleased();

    bool IsRightMousePressed();

    bool IsNewRightMousePressed();

    bool IsNewRightMouseReleased();

    bool WasRightMousePressed();

    bool WasRightMouseReleased();

    bool IsMiddleMousePressed();

    bool IsNewMiddleMousePressed();

    bool IsNewMiddleMouseReleased();

    bool WasMiddleMousePressed();

    bool WasMiddleMouseReleased();

    bool IsXButton1MousePressed();

    bool IsNewXButton1MousePressed();

    bool IsNewXButton1MouseReleased();

    bool WasXButton1MousePressed();

    bool WasXButton1MouseReleased();

    bool IsXButton2MousePressed();

    bool IsNewXButton2MousePressed();

    bool IsNewXButton2MouseReleased();

    bool WasXButton2MousePressed();

    bool WasXButton2MouseReleased();

    bool IsJoystickButtonPressed(MyJoystickButtonsEnum button);

    bool IsJoystickButtonNewPressed(MyJoystickButtonsEnum button);

    bool IsJoystickButtonNewReleased(MyJoystickButtonsEnum button);

    float GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis);

    Vector3 GetJoystickPositionForGameplay(RequestedJoystickAxis requestedAxis = RequestedJoystickAxis.All);

    Vector3 GetJoystickRotationForGameplay(RequestedJoystickAxis requestedAxis = RequestedJoystickAxis.All);

    bool IsJoystickAxisPressed(MyJoystickAxesEnum axis);

    bool IsJoystickAxisNewPressed(MyJoystickAxesEnum axis);

    bool IsNewJoystickAxisReleased(MyJoystickAxesEnum axis);

    bool IsJoystickAxisNewPressedXinput(MyJoystickAxesEnum axis);

    bool IsNewJoystickAxisReleasedXinput(MyJoystickAxesEnum axis);

    float GetJoystickSensitivity();

    void SetJoystickSensitivity(float newSensitivity);

    float GetJoystickExponent();

    void SetJoystickExponent(float newExponent);

    float GetJoystickDeadzone();

    void SetJoystickDeadzone(float newDeadzone);

    bool IsAnyMouseOrJoystickPressed();

    bool IsAnyNewMouseOrJoystickPressed();

    bool IsNewPrimaryButtonPressed();

    bool IsNewSecondaryButtonPressed();

    bool IsNewPrimaryButtonReleased();

    bool IsNewSecondaryButtonReleased();

    bool IsPrimaryButtonReleased();

    bool IsSecondaryButtonReleased();

    bool IsPrimaryButtonPressed();

    bool IsSecondaryButtonPressed();

    bool IsNewButtonPressed(MySharedButtonsEnum button);

    bool IsButtonPressed(MySharedButtonsEnum button);

    bool IsNewButtonReleased(MySharedButtonsEnum button);

    bool IsButtonReleased(MySharedButtonsEnum button);

    int MouseScrollWheelValue();

    int PreviousMouseScrollWheelValue();

    int DeltaMouseScrollWheelValue();

    bool IsEnabled();

    void EnableInput(bool enable);

    int GetMouseXForGamePlay();

    int GetMouseYForGamePlay();

    float GetMouseXForGamePlayF();

    float GetMouseYForGamePlayF();

    int GetMouseX();

    int GetMouseY();

    bool GetMouseXInversion();

    bool GetMouseYInversion();

    bool GetMouseScrollBlockSelectionInversion();

    void SetMouseXInversion(bool inverted);

    void SetMouseYInversion(bool inverted);

    void SetMouseScrollBlockSelectionInversion(bool inverted);

    bool GetJoystickYInversionCharacter();

    void SetJoystickYInversionCharacter(bool inverted);

    bool GetJoystickYInversionVehicle();

    void SetJoystickYInversionVehicle(bool inverted);

    float GetMouseSensitivity();

    void SetMouseSensitivity(float sensitivity);

    void SetMousePositionScale(float scaleFactor);

    Vector2 GetMousePosition();

    Vector2 GetMouseAreaSize();

    void SetMousePosition(int x, int y);

    bool IsNewGameControlPressed(MyStringId controlEnum);

    bool IsGameControlPressed(MyStringId controlEnum);

    bool IsNewGameControlReleased(MyStringId controlEnum);

    float GetGameControlAnalogState(MyStringId controlEnum);

    bool IsGameControlReleased(MyStringId controlEnum);

    bool IsKeyValid(MyKeys key);

    bool IsKeyDigit(MyKeys key);

    bool IsMouseButtonValid(MyMouseButtonsEnum button);

    bool IsJoystickButtonValid(MyJoystickButtonsEnum button);

    bool IsJoystickAxisValid(MyJoystickAxesEnum axis);

    bool IsJoystickConnected();

    bool JoystickAsMouse { get; set; }

    bool IsJoystickLastUsed { get; set; }

    event Action<bool> JoystickConnected;

    MyControl GetControl(MyKeys key);

    MyControl GetControl(MyMouseButtonsEnum button);

    void GetListOfPressedKeys(List<MyKeys> keys);

    void GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result);

    DictionaryValuesReader<MyStringId, MyControl> GetGameControlsList();

    void TakeSnapshot();

    void RevertChanges();

    MyControl GetGameControl(MyStringId controlEnum);

    void RevertToDefaultControls();

    void AddDefaultControl(MyStringId stringId, MyControl control);

    void SaveControls(
      SerializableDictionary<string, string> controlsGeneral,
      SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons);

    bool IsDirectInputInitialized { get; }

    string GetKeyName(MyKeys key);

    string GetName(MyMouseButtonsEnum mouseButton);

    string GetName(MyJoystickButtonsEnum joystickButton);

    string GetName(MyJoystickAxesEnum joystickAxis);

    string GetUnassignedName();

    bool IsGamepadKeyRightPressed();

    bool IsGamepadKeyLeftPressed();

    bool IsNewGamepadKeyDownPressed();

    bool IsNewGamepadKeyUpPressed();

    void GetActualJoystickState(StringBuilder text);

    bool IsNewGameControlJoystickOnlyPressed(MyStringId controlId);

    void DeviceChangeCallback();

    void NegateEscapePress();

    bool IsJoystickIdle();
  }
}
