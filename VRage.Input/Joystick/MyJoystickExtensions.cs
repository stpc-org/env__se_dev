// Decompiled with JetBrains decompiler
// Type: VRage.Input.Joystick.MyJoystickExtensions
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

namespace VRage.Input.Joystick
{
  internal static class MyJoystickExtensions
  {
    public static unsafe bool IsPressed(this MyJoystickState state, int button) => state.Buttons[button] > (byte) 0;

    public static bool IsReleased(this MyJoystickState state, int button) => !state.IsPressed(button);
  }
}
