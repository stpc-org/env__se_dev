// Decompiled with JetBrains decompiler
// Type: VRage.Input.IVRageInput2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Input
{
  public interface IVRageInput2 : IDisposable
  {
    void GetMouseState(out MyMouseState state);

    List<string> EnumerateJoystickNames();

    string InitializeJoystickIfPossible(string joystickInstanceName);

    bool IsJoystickAxisSupported(MyJoystickAxesEnum axis);

    bool IsJoystickConnected();

    void GetJoystickState(ref MyJoystickState state);

    void ShowVirtualKeyboardIfNeeded(
      Action<string> onSuccess,
      Action onCancel = null,
      string defaultText = null,
      string title = null,
      int maxLength = 0);

    unsafe void GetAsyncKeyStates(byte* data);

    uint[] DeveloperKeys { get; }

    bool IsCorrectlyInitialized { get; }
  }
}
