// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Localization.MyKeysToString
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Input;

namespace Sandbox.Game.Localization
{
  public class MyKeysToString : IMyControlNameLookup
  {
    private readonly string[] m_systemKeyNamesLower = new string[256];
    private readonly string[] m_systemKeyNamesUpper = new string[256];
    private readonly MyUtilKeyToString[] m_keyToString = new MyUtilKeyToString[86]
    {
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Left, MyCommonTexts.KeysLeft),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Right, MyCommonTexts.KeysRight),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Up, MyCommonTexts.KeysUp),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Down, MyCommonTexts.KeysDown),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Home, MyCommonTexts.KeysHome),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.End, MyCommonTexts.KeysEnd),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Delete, MyCommonTexts.KeysDelete),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Back, MyCommonTexts.KeysBackspace),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Insert, MyCommonTexts.KeysInsert),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.PageDown, MyCommonTexts.KeysPageDown),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.PageUp, MyCommonTexts.KeysPageUp),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Alt, MyCommonTexts.KeysAlt),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Control, MyCommonTexts.KeysControl),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Shift, MyCommonTexts.KeysShift),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.LeftAlt, MyCommonTexts.KeysLeftAlt),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.LeftControl, MyCommonTexts.KeysLeftControl),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.LeftShift, MyCommonTexts.KeysLeftShift),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.RightAlt, MyCommonTexts.KeysRightAlt),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.RightControl, MyCommonTexts.KeysRightControl),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.RightShift, MyCommonTexts.KeysRightShift),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.CapsLock, MyCommonTexts.KeysCapsLock),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Enter, MyCommonTexts.KeysEnter),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Tab, MyCommonTexts.KeysTab),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemOpenBrackets, MyCommonTexts.KeysOpenBracket),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemCloseBrackets, MyCommonTexts.KeysCloseBracket),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Multiply, MyCommonTexts.KeysMultiply),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Subtract, MyCommonTexts.KeysSubtract),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Add, MyCommonTexts.KeysAdd),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Divide, MyCommonTexts.KeysDivide),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad0, MyCommonTexts.KeysNumPad0),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad1, MyCommonTexts.KeysNumPad1),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad2, MyCommonTexts.KeysNumPad2),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad3, MyCommonTexts.KeysNumPad3),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad4, MyCommonTexts.KeysNumPad4),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad5, MyCommonTexts.KeysNumPad5),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad6, MyCommonTexts.KeysNumPad6),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad7, MyCommonTexts.KeysNumPad7),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad8, MyCommonTexts.KeysNumPad8),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.NumPad9, MyCommonTexts.KeysNumPad9),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Decimal, MyCommonTexts.KeysDecimal),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemBackslash, MyCommonTexts.KeysBackslash),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemComma, MyCommonTexts.KeysComma),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemMinus, MyCommonTexts.KeysMinus),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemPeriod, MyCommonTexts.KeysPeriod),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemPipe, MyCommonTexts.KeysPipe),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemPlus, MyCommonTexts.KeysPlus),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemQuestion, MyCommonTexts.KeysQuestion),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemQuotes, MyCommonTexts.KeysQuotes),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemSemicolon, MyCommonTexts.KeysSemicolon),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.OemTilde, MyCommonTexts.KeysTilde),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Space, MyCommonTexts.KeysSpace),
      (MyUtilKeyToString) new MyUtilKeyToStringLocalized(MyKeys.Pause, MyCommonTexts.KeysPause),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D0, "0"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D1, "1"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D2, "2"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D3, "3"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D4, "4"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D5, "5"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D6, "6"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D7, "7"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D8, "8"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.D9, "9"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F1, "F1"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F2, "F2"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F3, "F3"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F4, "F4"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F5, "F5"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F6, "F6"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F7, "F7"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F8, "F8"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F9, "F9"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F10, "F10"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F11, "F11"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F12, "F12"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F13, "F13"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F14, "F14"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F15, "F15"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F16, "F16"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F17, "F17"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F18, "F18"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F19, "F19"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F20, "F20"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F21, "F21"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F22, "F22"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F23, "F23"),
      (MyUtilKeyToString) new MyUtilKeyToStringSimple(MyKeys.F24, "F24")
    };

    public MyKeysToString()
    {
      for (int index1 = 0; index1 < this.m_systemKeyNamesLower.Length; ++index1)
      {
        string[] systemKeyNamesLower = this.m_systemKeyNamesLower;
        int index2 = index1;
        char ch = (char) index1;
        string lower = ch.ToString().ToLower();
        systemKeyNamesLower[index2] = lower;
        string[] systemKeyNamesUpper = this.m_systemKeyNamesUpper;
        int index3 = index1;
        ch = (char) index1;
        string upper = ch.ToString().ToUpper();
        systemKeyNamesUpper[index3] = upper;
      }
    }

    string IMyControlNameLookup.UnassignedText => MyTexts.GetString(MyCommonTexts.UnknownControl_Unassigned);

    string IMyControlNameLookup.GetKeyName(MyKeys key)
    {
      if (key >= (MyKeys) this.m_systemKeyNamesUpper.Length)
        return (string) null;
      string name = this.m_systemKeyNamesUpper[(int) key];
      for (int index = 0; index < this.m_keyToString.Length; ++index)
      {
        if (this.m_keyToString[index].Key == key)
        {
          name = this.m_keyToString[index].Name;
          break;
        }
      }
      return name;
    }

    string IMyControlNameLookup.GetName(MyMouseButtonsEnum button)
    {
      switch (button)
      {
        case MyMouseButtonsEnum.Left:
          return MyTexts.GetString(MyCommonTexts.LeftMouseButton);
        case MyMouseButtonsEnum.Middle:
          return MyTexts.GetString(MyCommonTexts.MiddleMouseButton);
        case MyMouseButtonsEnum.Right:
          return MyTexts.GetString(MyCommonTexts.RightMouseButton);
        case MyMouseButtonsEnum.XButton1:
          return MyTexts.GetString(MyCommonTexts.MouseXButton1);
        case MyMouseButtonsEnum.XButton2:
          return MyTexts.GetString(MyCommonTexts.MouseXButton2);
        default:
          return MyTexts.GetString(MySpaceTexts.Blank);
      }
    }

    string IMyControlNameLookup.GetName(MyJoystickButtonsEnum joystickButton)
    {
      switch (joystickButton)
      {
        case MyJoystickButtonsEnum.None:
          return "";
        case MyJoystickButtonsEnum.JDLeft:
          return MyTexts.GetString(MyCommonTexts.JoystickButtonLeft);
        case MyJoystickButtonsEnum.JDRight:
          return MyTexts.GetString(MyCommonTexts.JoystickButtonRight);
        case MyJoystickButtonsEnum.JDUp:
          return MyTexts.GetString(MyCommonTexts.JoystickButtonUp);
        case MyJoystickButtonsEnum.JDDown:
          return MyTexts.GetString(MyCommonTexts.JoystickButtonDown);
        default:
          return "JB" + (object) (int) (joystickButton - (byte) 4);
      }
    }

    string IMyControlNameLookup.GetName(MyJoystickAxesEnum joystickAxis)
    {
      switch (joystickAxis)
      {
        case MyJoystickAxesEnum.Xpos:
          return "JX+";
        case MyJoystickAxesEnum.Xneg:
          return "JX-";
        case MyJoystickAxesEnum.Ypos:
          return "JY+";
        case MyJoystickAxesEnum.Yneg:
          return "JY-";
        case MyJoystickAxesEnum.Zpos:
          return "JZ+";
        case MyJoystickAxesEnum.Zneg:
          return "JZ-";
        case MyJoystickAxesEnum.RotationXpos:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationXpos);
        case MyJoystickAxesEnum.RotationXneg:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationXneg);
        case MyJoystickAxesEnum.RotationYpos:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationYpos);
        case MyJoystickAxesEnum.RotationYneg:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationYneg);
        case MyJoystickAxesEnum.RotationZpos:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationZpos);
        case MyJoystickAxesEnum.RotationZneg:
          return MyTexts.GetString(MyCommonTexts.JoystickRotationZneg);
        case MyJoystickAxesEnum.Slider1pos:
          return MyTexts.GetString(MyCommonTexts.JoystickSlider1pos);
        case MyJoystickAxesEnum.Slider1neg:
          return MyTexts.GetString(MyCommonTexts.JoystickSlider1neg);
        case MyJoystickAxesEnum.Slider2pos:
          return MyTexts.GetString(MyCommonTexts.JoystickSlider2pos);
        case MyJoystickAxesEnum.Slider2neg:
          return MyTexts.GetString(MyCommonTexts.JoystickSlider2neg);
        default:
          return "";
      }
    }
  }
}
