// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyControl
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using VRage.Input;
using VRage.Utils;

namespace VRage.ModAPI
{
  public interface IMyControl
  {
    MyKeys GetKeyboardControl();

    MyKeys GetSecondKeyboardControl();

    MyMouseButtonsEnum GetMouseControl();

    bool IsPressed();

    bool IsNewPressed();

    bool IsNewReleased();

    bool IsJoystickPressed();

    bool IsNewJoystickPressed();

    bool IsNewJoystickReleased();

    float GetAnalogState();

    MyStringId GetControlName();

    MyStringId? GetControlDescription();

    MyGuiControlTypeEnum GetControlTypeEnum();

    MyStringId GetGameControlEnum();

    bool IsControlAssigned();

    string GetControlButtonName(MyGuiInputDeviceEnum deviceType);
  }
}
