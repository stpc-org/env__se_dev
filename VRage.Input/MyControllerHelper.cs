// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyControllerHelper
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;

namespace VRage.Input
{
  public static class MyControllerHelper
  {
    private static readonly Dictionary<MyJoystickAxesEnum, char> XBOX_AXES_CODES = new Dictionary<MyJoystickAxesEnum, char>()
    {
      {
        MyJoystickAxesEnum.Xneg,
        '\xE016'
      },
      {
        MyJoystickAxesEnum.Xpos,
        '\xE015'
      },
      {
        MyJoystickAxesEnum.Ypos,
        '\xE014'
      },
      {
        MyJoystickAxesEnum.Yneg,
        '\xE017'
      },
      {
        MyJoystickAxesEnum.RotationXneg,
        '\xE020'
      },
      {
        MyJoystickAxesEnum.RotationXpos,
        '\xE019'
      },
      {
        MyJoystickAxesEnum.RotationYneg,
        '\xE021'
      },
      {
        MyJoystickAxesEnum.RotationYpos,
        '\xE018'
      },
      {
        MyJoystickAxesEnum.Zneg,
        '\xE007'
      },
      {
        MyJoystickAxesEnum.Zpos,
        '\xE008'
      }
    };
    private static readonly Dictionary<MyJoystickButtonsEnum, char> XBOX_BUTTONS_CODES = new Dictionary<MyJoystickButtonsEnum, char>()
    {
      {
        MyJoystickButtonsEnum.J01,
        '\xE001'
      },
      {
        MyJoystickButtonsEnum.J02,
        '\xE003'
      },
      {
        MyJoystickButtonsEnum.J03,
        '\xE002'
      },
      {
        MyJoystickButtonsEnum.J04,
        '\xE004'
      },
      {
        MyJoystickButtonsEnum.J05,
        '\xE005'
      },
      {
        MyJoystickButtonsEnum.J06,
        '\xE006'
      },
      {
        MyJoystickButtonsEnum.J07,
        '\xE00D'
      },
      {
        MyJoystickButtonsEnum.J08,
        '\xE00E'
      },
      {
        MyJoystickButtonsEnum.J09,
        '\xE00B'
      },
      {
        MyJoystickButtonsEnum.J10,
        '\xE00C'
      },
      {
        MyJoystickButtonsEnum.JDLeft,
        '\xE010'
      },
      {
        MyJoystickButtonsEnum.JDUp,
        '\xE011'
      },
      {
        MyJoystickButtonsEnum.JDRight,
        '\xE012'
      },
      {
        MyJoystickButtonsEnum.JDDown,
        '\xE013'
      },
      {
        MyJoystickButtonsEnum.J11,
        '\xE007'
      },
      {
        MyJoystickButtonsEnum.J12,
        '\xE008'
      }
    };
    public static readonly MyStringId CX_BASE = MyStringId.GetOrCompute("BASE");
    public static readonly MyStringId CX_GUI = MyStringId.GetOrCompute("GUI");
    public static readonly MyStringId CX_CHARACTER = MyStringId.GetOrCompute("CHARACTER");
    public static readonly ITextEvaluator ButtonTextEvaluator = (ITextEvaluator) new MyControllerHelper.ButtonEvaluator();
    public static float GAMEPAD_ANALOG_SCROLL_SPEED = 0.08f;
    public const float JOYSTICK_CARSTEERINGXYAXISPERCENTAGE_DEFAULT = 0.9f;
    public const float JOYSTICK_REVERSEPOINT_DEFAULT = 0.35f;
    private static readonly TimeSpan REPEAT_START_TIME = new TimeSpan(0, 0, 0, 0, 500);
    private static readonly TimeSpan REPEAT_TIME = new TimeSpan(0, 0, 0, 0, 100);
    private static bool? m_isJoystickYAxisState_Reversing;
    private static MyControllerHelper.EmptyControl m_nullControl = new MyControllerHelper.EmptyControl();
    private static Dictionary<MyStringId, MyControllerHelper.Context> m_bindings = new Dictionary<MyStringId, MyControllerHelper.Context>((IEqualityComparer<MyStringId>) MyStringId.Comparer);

    static MyControllerHelper() => MyControllerHelper.Initialize();

    private static void Initialize() => MyControllerHelper.m_bindings.Add(MyStringId.NullOrEmpty, new MyControllerHelper.Context());

    public static void AddContext(MyStringId context, MyStringId? parent = null)
    {
      if (MyControllerHelper.m_bindings.ContainsKey(context))
        return;
      MyControllerHelper.Context context1 = new MyControllerHelper.Context();
      MyControllerHelper.m_bindings.Add(context, context1);
      if (!parent.HasValue || !MyControllerHelper.m_bindings.ContainsKey(parent.Value))
        return;
      context1.ParentContext = MyControllerHelper.m_bindings[parent.Value];
    }

    public static void AddControl(MyStringId context, MyStringId stringId, string fakeCode) => MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.FakeControl(fakeCode);

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickAxesEnum axis,
      Func<bool> condition = null)
    {
      MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickAxis(axis, condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum button,
      Func<bool> condition = null)
    {
      MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickButton(button, condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum modifier,
      MyJoystickButtonsEnum control,
      bool pressed,
      Func<bool> condition = null)
    {
      if (pressed)
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickPressedModifier((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
      else
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickReleasedModifier((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum modifier,
      MyJoystickAxesEnum control,
      bool pressed,
      Func<bool> condition = null)
    {
      if (pressed)
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickPressedModifier((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier), (IMyControllerControl) new MyControllerHelper.JoystickAxis(control), condition);
      else
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickReleasedModifier((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier), (IMyControllerControl) new MyControllerHelper.JoystickAxis(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum modifier1,
      MyJoystickButtonsEnum modifier2,
      MyJoystickButtonsEnum control,
      bool pressed,
      Func<bool> condition = null)
    {
      if (pressed)
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickPressedTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
      else
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickReleasedTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum modifier1,
      MyJoystickButtonsEnum modifier2,
      MyJoystickButtonsEnum modifier3,
      MyJoystickButtonsEnum control,
      bool pressed,
      Func<bool> condition = null)
    {
      if (pressed)
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickPressedThreeModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier3), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
      else
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickReleasedThreeModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier3), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum modifier1,
      MyJoystickButtonsEnum modifier2,
      MyJoystickAxesEnum control,
      bool pressed,
      Func<bool> condition = null)
    {
      if (pressed)
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickPressedTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickAxis(control), condition);
      else
        MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickReleasedTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(modifier1), (IMyControllerControl) new MyControllerHelper.JoystickButton(modifier2), (IMyControllerControl) new MyControllerHelper.JoystickAxis(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum pressedModifier,
      MyJoystickButtonsEnum releasedModifier,
      MyJoystickAxesEnum control,
      Func<bool> condition = null)
    {
      MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickOneOfTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(pressedModifier), (IMyControllerControl) new MyControllerHelper.JoystickButton(releasedModifier), (IMyControllerControl) new MyControllerHelper.JoystickAxis(control), condition);
    }

    public static void AddControl(
      MyStringId context,
      MyStringId stringId,
      MyJoystickButtonsEnum pressedModifier,
      MyJoystickButtonsEnum releasedModifier,
      MyJoystickButtonsEnum control,
      Func<bool> condition = null)
    {
      MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) new MyControllerHelper.JoystickOneOfTwoModifiers((IMyControllerControl) new MyControllerHelper.JoystickButton(pressedModifier), (IMyControllerControl) new MyControllerHelper.JoystickButton(releasedModifier), (IMyControllerControl) new MyControllerHelper.JoystickButton(control), condition);
    }

    public static void NullControl(MyStringId context, MyStringId stringId) => MyControllerHelper.m_bindings[context][stringId] = (IMyControllerControl) MyControllerHelper.m_nullControl;

    public static void NullControl(MyStringId context, MyJoystickAxesEnum axis)
    {
      MyStringId id = MyStringId.NullOrEmpty;
      foreach (KeyValuePair<MyStringId, IMyControllerControl> binding in MyControllerHelper.m_bindings[context].Bindings)
      {
        if (binding.Value is MyControllerHelper.JoystickAxis && (MyJoystickAxesEnum) binding.Value.Code == axis)
        {
          id = binding.Key;
          break;
        }
      }
      if (!(id != MyStringId.NullOrEmpty))
        return;
      MyControllerHelper.m_bindings[context][id] = (IMyControllerControl) MyControllerHelper.m_nullControl;
    }

    public static bool IsNullControl(IMyControllerControl control) => control == MyControllerHelper.m_nullControl;

    public static IMyControllerControl GetNullControl() => (IMyControllerControl) MyControllerHelper.m_nullControl;

    public static void NullControl(MyStringId context, MyJoystickButtonsEnum button)
    {
      MyStringId id = MyStringId.NullOrEmpty;
      foreach (KeyValuePair<MyStringId, IMyControllerControl> binding in MyControllerHelper.m_bindings[context].Bindings)
      {
        if (binding.Value is MyControllerHelper.JoystickButton && (MyJoystickButtonsEnum) binding.Value.Code == button)
        {
          id = binding.Key;
          break;
        }
      }
      if (!(id != MyStringId.NullOrEmpty))
        return;
      MyControllerHelper.m_bindings[context][id] = (IMyControllerControl) MyControllerHelper.m_nullControl;
    }

    public static bool IsControl(
      MyStringId context,
      MyStringId stringId,
      MyControlStateType type = MyControlStateType.NEW_PRESSED,
      bool joystickOnly = false,
      bool useXinput = false)
    {
      IMyControllerControl controllerControl = MyControllerHelper.m_bindings[context][stringId];
      if (useXinput && (controllerControl.Code == (byte) 6 || controllerControl.Code == (byte) 5) && (type == MyControlStateType.NEW_PRESSED || type == MyControlStateType.NEW_RELEASED))
      {
        if (type != MyControlStateType.NEW_PRESSED)
        {
          if (type == MyControlStateType.NEW_RELEASED)
            return !joystickOnly && MyInput.Static.IsNewGameControlReleased(stringId) || controllerControl.IsNewReleasedXinput();
        }
        else
          return !joystickOnly && MyInput.Static.IsNewGameControlPressed(stringId) || controllerControl.IsNewPressedXinput();
      }
      switch (type)
      {
        case MyControlStateType.NEW_PRESSED:
          return !joystickOnly && MyInput.Static.IsNewGameControlPressed(stringId) || controllerControl.IsNewPressed();
        case MyControlStateType.PRESSED:
          return !joystickOnly && MyInput.Static.IsGameControlPressed(stringId) || controllerControl.IsPressed();
        case MyControlStateType.NEW_RELEASED:
          return !joystickOnly && MyInput.Static.IsNewGameControlReleased(stringId) || controllerControl.IsNewReleased();
        case MyControlStateType.NEW_PRESSED_REPEATING:
          return !joystickOnly && MyInput.Static.IsNewGameControlPressed(stringId) || controllerControl.IsNewPressedRepeating();
        default:
          return false;
      }
    }

    public static float IsControlAnalog(
      MyStringId context,
      MyStringId stringId,
      bool gamepadShipControl = false)
    {
      return gamepadShipControl ? MyInput.Static.GetGameControlAnalogState(stringId) + MyControllerHelper.GetJoystickAxisStateForShipGameplay((MyJoystickAxesEnum) MyControllerHelper.m_bindings[context][stringId].Code) : MyInput.Static.GetGameControlAnalogState(stringId) + MyControllerHelper.m_bindings[context][stringId].AnalogValue();
    }

    public static bool IsDefined(MyStringId contextId, MyStringId controlId)
    {
      MyControllerHelper.Context context;
      return MyControllerHelper.m_bindings.TryGetValue(contextId, out context) && context[controlId] != MyControllerHelper.m_nullControl;
    }

    public static string GetCodeForControl(MyStringId context, MyStringId stringId) => (string) MyControllerHelper.m_bindings.GetValueOrDefault<MyStringId, MyControllerHelper.Context>(context, MyControllerHelper.Context.Empty)[stringId].ControlCode();

    public static IMyControllerControl GetControl(
      MyStringId context,
      MyStringId stringId)
    {
      return MyControllerHelper.m_bindings[context][stringId];
    }

    public static IMyControllerControl TryGetControl(
      MyStringId context,
      MyStringId stringId)
    {
      return !MyControllerHelper.m_bindings.ContainsKey(context) ? (IMyControllerControl) null : MyControllerHelper.m_bindings[context][stringId];
    }

    public static void ClearBindings()
    {
      MyControllerHelper.m_bindings.Clear();
      MyControllerHelper.Initialize();
    }

    public static float GetJoystickAxisStateForShipGameplay(MyJoystickAxesEnum axis)
    {
      if (MyInput.Static.GetType() != typeof (MyVRageInput))
        return 0.0f;
      MyVRageInput myVrageInput = (MyVRageInput) MyInput.Static;
      if (axis != MyJoystickAxesEnum.Yneg && axis != MyJoystickAxesEnum.Ypos)
        return myVrageInput.GetJoystickAxisStateForGameplay(axis);
      float num1 = 6553.501f;
      int joystickAxisStateRaw1 = (int) myVrageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
      int joystickAxisStateRaw2 = (int) myVrageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Yneg);
      if ((double) joystickAxisStateRaw1 > (double) num1 && (double) joystickAxisStateRaw1 < (double) ushort.MaxValue - (double) num1 && ((double) joystickAxisStateRaw2 > (double) num1 && (double) joystickAxisStateRaw2 < (double) ushort.MaxValue - (double) num1))
        return myVrageInput.GetJoystickAxisStateForGameplay(axis);
      if (!myVrageInput.IsJoystickLastUsed || !myVrageInput.IsJoystickAxisSupported(axis))
        return 0.0f;
      int joystickAxisStateRaw3 = (int) myVrageInput.GetJoystickAxisStateRaw(axis);
      int num2 = (int) ((double) ushort.MaxValue * (double) myVrageInput.GetJoystickDeadzone());
      int num3 = (int) short.MaxValue - num2 / 2;
      int num4 = (int) short.MaxValue + num2 / 2;
      if (joystickAxisStateRaw3 > num3 && joystickAxisStateRaw3 < num4)
      {
        int joystickAxisStateRaw4 = (int) myVrageInput.GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xneg);
        if (joystickAxisStateRaw4 > num3 && joystickAxisStateRaw4 < num4)
        {
          if (MyControllerHelper.m_isJoystickYAxisState_Reversing.HasValue)
            MyControllerHelper.m_isJoystickYAxisState_Reversing = new bool?();
          return 0.0f;
        }
      }
      if (!MyControllerHelper.m_isJoystickYAxisState_Reversing.HasValue)
        MyControllerHelper.m_isJoystickYAxisState_Reversing = joystickAxisStateRaw3 < (int) short.MaxValue ? new bool?(false) : new bool?(true);
      float maxValue = (float) ushort.MaxValue;
      float num5 = 22937.25f;
      float num6 = (float) ushort.MaxValue - maxValue;
      float num7 = (float) ushort.MaxValue - num5;
      float num8 = num7 - num6;
      float num9;
      if (MyControllerHelper.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 >= (double) maxValue || !MyControllerHelper.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 <= (double) num6)
      {
        num9 = 1f;
      }
      else
      {
        if (MyControllerHelper.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 < (double) num5 || !MyControllerHelper.m_isJoystickYAxisState_Reversing.Value && (double) joystickAxisStateRaw3 > (double) num7)
        {
          MyControllerHelper.m_isJoystickYAxisState_Reversing = new bool?(!MyControllerHelper.m_isJoystickYAxisState_Reversing.Value);
          return MyControllerHelper.GetJoystickAxisStateForShipGameplay(axis);
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
      return axis == MyJoystickAxesEnum.Yneg && MyControllerHelper.m_isJoystickYAxisState_Reversing.Value || axis == MyJoystickAxesEnum.Ypos && !MyControllerHelper.m_isJoystickYAxisState_Reversing.Value ? 0.0f : myVrageInput.GetJoystickSensitivity() * (float) Math.Pow((double) num9, (double) myVrageInput.GetJoystickExponent());
    }

    private class ButtonEvaluator : ITextEvaluator
    {
      private readonly Dictionary<string, string> ButtonCodes = new Dictionary<string, string>();

      public ButtonEvaluator()
      {
        char v;
        foreach (KeyValuePair<MyJoystickAxesEnum, char> pair in MyControllerHelper.XBOX_AXES_CODES)
        {
          MyJoystickAxesEnum k;
          pair.Deconstruct<MyJoystickAxesEnum, char>(out k, out v);
          MyJoystickAxesEnum joystickAxesEnum = k;
          char ch = v;
          this.ButtonCodes.Add("AXIS_" + joystickAxesEnum.ToString().ToUpperInvariant(), ch.ToString());
        }
        this.ButtonCodes.Add("AXIS_MOTION", "\xE009");
        this.ButtonCodes.Add("AXIS_MOTION_X", "\xE022");
        this.ButtonCodes.Add("AXIS_MOTION_Y", "\xE023");
        this.ButtonCodes.Add("AXIS_ROTATION", "\xE00A");
        this.ButtonCodes.Add("AXIS_ROTATION_X", "\xE024");
        this.ButtonCodes.Add("AXIS_ROTATION_Y", "\xE025");
        this.ButtonCodes.Add("AXIS_DPAD", "\xE00F");
        this.ButtonCodes.Add("LEFT_STICK_LEFTRIGHT", "\xE022");
        this.ButtonCodes.Add("LEFT_STICK_UPDOWN", "\xE023");
        this.ButtonCodes.Add("RIGHT_STICK_LEFTRIGHT", "\xE024");
        this.ButtonCodes.Add("RIGHT_STICK_UPDOWN", "\xE025");
        this.ButtonCodes.Add("DPAD_LEFTRIGHT", "\xE026");
        this.ButtonCodes.Add("DPAD_UPDOWN", "\xE027");
        foreach (KeyValuePair<MyJoystickButtonsEnum, char> pair in MyControllerHelper.XBOX_BUTTONS_CODES)
        {
          MyJoystickButtonsEnum k;
          pair.Deconstruct<MyJoystickButtonsEnum, char>(out k, out v);
          MyJoystickButtonsEnum joystickButtonsEnum = k;
          char ch = v;
          this.ButtonCodes.Add("BUTTON_" + joystickButtonsEnum.ToString().ToUpperInvariant(), ch.ToString());
        }
      }

      public string TokenEvaluate(string token, string context)
      {
        string str;
        return this.ButtonCodes.TryGetValue(token, out str) ? str : "<Invalid Button>";
      }
    }

    private class Context
    {
      public MyControllerHelper.Context ParentContext;
      public Dictionary<MyStringId, IMyControllerControl> Bindings;
      public static readonly MyControllerHelper.Context Empty = new MyControllerHelper.Context();

      public IMyControllerControl this[MyStringId id]
      {
        get
        {
          if (this.Bindings.ContainsKey(id))
            return this.Bindings[id];
          return this.ParentContext != null ? this.ParentContext[id] : (IMyControllerControl) MyControllerHelper.m_nullControl;
        }
        set => this.Bindings[id] = value;
      }

      public Context() => this.Bindings = new Dictionary<MyStringId, IMyControllerControl>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    }

    private class EmptyControl : IMyControllerControl
    {
      public byte Code => 0;

      public Func<bool> Condition { get; private set; }

      public bool IsNewPressed() => false;

      public bool IsNewPressedRepeating() => false;

      public bool IsPressed() => false;

      public bool IsNewReleased() => false;

      public float AnalogValue() => 0.0f;

      public object ControlCode() => (object) " ";

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => false;

      public bool IsNewReleasedXinput() => false;
    }

    private class JoystickAxis : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public MyJoystickAxesEnum Axis;

      public byte Code => (byte) this.Axis;

      public Func<bool> Condition { get; private set; }

      public JoystickAxis(MyJoystickAxesEnum axis, Func<bool> condition = null)
      {
        this.Axis = axis;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = MyInput.Static.IsJoystickAxisNewPressed(this.Axis) ? 1 : 0;
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && MyInput.Static.IsJoystickAxisPressed(this.Axis);

      public bool IsNewReleased() => (this.Condition == null || this.Condition()) && MyInput.Static.IsNewJoystickAxisReleased(this.Axis);

      public float AnalogValue() => this.Condition != null && !this.Condition() ? 0.0f : MyInput.Static.GetJoystickAxisStateForGameplay(this.Axis);

      public object ControlCode() => (object) MyControllerHelper.XBOX_AXES_CODES[this.Axis].ToString();

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = MyInput.Static.IsJoystickAxisNewPressedXinput(this.Axis) ? 1 : 0;
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewReleasedXinput() => (this.Condition == null || this.Condition()) && MyInput.Static.IsNewJoystickAxisReleasedXinput(this.Axis);
    }

    private class JoystickButton : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public MyJoystickButtonsEnum Button;

      public byte Code => (byte) this.Button;

      public Func<bool> Condition { get; private set; }

      public JoystickButton(MyJoystickButtonsEnum button, Func<bool> condition = null)
      {
        this.Button = button;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = MyInput.Static.IsJoystickButtonNewPressed(this.Button) ? 1 : 0;
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && MyInput.Static.IsJoystickButtonPressed(this.Button);

      public bool IsNewReleased() => (this.Condition == null || this.Condition()) && MyInput.Static.IsJoystickButtonNewReleased(this.Button);

      public float AnalogValue() => this.Condition != null && !this.Condition() ? 0.0f : (this.IsPressed() ? 1f : 0.0f);

      public object ControlCode() => (object) MyControllerHelper.XBOX_BUTTONS_CODES[this.Button].ToString();

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class FakeControl : IMyControllerControl
    {
      private string m_fakeCode;

      public byte Code => 0;

      public Func<bool> Condition { get; }

      public FakeControl(string fakecode) => this.m_fakeCode = fakecode;

      public float AnalogValue() => 0.0f;

      public object ControlCode() => (object) this.m_fakeCode;

      public bool IsNewPressed() => false;

      public bool IsNewPressedRepeating() => false;

      public bool IsNewReleased() => false;

      public bool IsPressed() => false;

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickPressedModifier : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickPressedModifier(
        IMyControllerControl modifier,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier = modifier;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = !this.Modifier.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && this.Modifier.IsPressed() && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Modifier.IsNewReleased() && this.Control.IsPressed())
          return true;
        return this.Modifier.IsPressed() && this.Control.IsNewReleased();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || !this.Modifier.IsPressed() ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => (object) (this.Modifier.ControlCode().ToString() + " + " + this.Control.ControlCode());

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickReleasedModifier : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickReleasedModifier(
        IMyControllerControl modifier,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier = modifier;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = this.Modifier.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && !this.Modifier.IsPressed() && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Control.IsPressed() && this.Modifier.IsNewPressed())
          return true;
        return this.Control.IsNewReleased() && !this.Modifier.IsPressed();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || this.Modifier.IsPressed() ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => this.Control.ControlCode();

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickPressedTwoModifiers : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier1;
      public IMyControllerControl Modifier2;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickPressedTwoModifiers(
        IMyControllerControl modifier1,
        IMyControllerControl modifier2,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier1 = modifier1;
        this.Modifier2 = modifier2;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = !this.Modifier1.IsPressed() || !this.Modifier2.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && (this.Modifier1.IsPressed() && this.Modifier2.IsPressed()) && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Modifier1.IsNewReleased() && this.Modifier2.IsPressed() && this.Control.IsPressed() || this.Modifier1.IsPressed() && this.Modifier2.IsNewReleased() && this.Control.IsPressed())
          return true;
        return this.Modifier1.IsPressed() && this.Modifier2.IsPressed() && this.Control.IsNewReleased();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || (!this.Modifier1.IsPressed() || !this.Modifier2.IsPressed()) ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => (object) (this.Modifier1.ControlCode().ToString() + " + " + this.Modifier2.ControlCode() + " + " + this.Control.ControlCode());

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickReleasedTwoModifiers : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier1;
      public IMyControllerControl Modifier2;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickReleasedTwoModifiers(
        IMyControllerControl modifier1,
        IMyControllerControl modifier2,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier1 = modifier1;
        this.Modifier2 = modifier2;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = this.Modifier1.IsPressed() || this.Modifier2.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && (!this.Modifier1.IsPressed() && !this.Modifier2.IsPressed()) && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Control.IsPressed() && (this.Modifier1.IsNewPressed() || this.Modifier2.IsNewPressed()))
          return true;
        return this.Control.IsNewReleased() && !this.Modifier1.IsPressed() && !this.Modifier2.IsPressed();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || (this.Modifier1.IsPressed() || this.Modifier2.IsPressed()) ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => this.Control.ControlCode();

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickPressedThreeModifiers : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier1;
      public IMyControllerControl Modifier2;
      public IMyControllerControl Modifier3;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickPressedThreeModifiers(
        IMyControllerControl modifier1,
        IMyControllerControl modifier2,
        IMyControllerControl modifier3,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier1 = modifier1;
        this.Modifier2 = modifier2;
        this.Modifier3 = modifier3;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = !this.Modifier1.IsPressed() || !this.Modifier2.IsPressed() || !this.Modifier3.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && (this.Modifier1.IsPressed() && this.Modifier2.IsPressed()) && this.Modifier3.IsPressed() && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Modifier1.IsNewReleased() && this.Modifier2.IsPressed() && (this.Modifier3.IsPressed() && this.Control.IsPressed()) || this.Modifier1.IsPressed() && this.Modifier2.IsNewReleased() && (this.Modifier3.IsPressed() && this.Control.IsPressed()) || this.Modifier1.IsPressed() && this.Modifier2.IsPressed() && (this.Modifier3.IsNewReleased() && this.Control.IsPressed()))
          return true;
        return this.Modifier1.IsPressed() && this.Modifier2.IsPressed() && this.Modifier3.IsPressed() && this.Control.IsNewReleased();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || (!this.Modifier1.IsPressed() || !this.Modifier2.IsPressed()) || !this.Modifier3.IsPressed() ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => (object) (this.Modifier1.ControlCode().ToString() + " + " + this.Modifier2.ControlCode() + " + " + this.Control.ControlCode());

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickReleasedThreeModifiers : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl Modifier1;
      public IMyControllerControl Modifier2;
      public IMyControllerControl Modifier3;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickReleasedThreeModifiers(
        IMyControllerControl modifier1,
        IMyControllerControl modifier2,
        IMyControllerControl modifier3,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.Modifier1 = modifier1;
        this.Modifier2 = modifier2;
        this.Modifier3 = modifier3;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = this.Modifier1.IsPressed() || this.Modifier2.IsPressed() || this.Modifier3.IsPressed() ? 0 : (this.Control.IsNewPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && (!this.Modifier1.IsPressed() && !this.Modifier2.IsPressed()) && !this.Modifier3.IsPressed() && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        if (this.Control.IsPressed() && (this.Modifier1.IsNewPressed() || this.Modifier2.IsNewPressed() || this.Modifier3.IsNewPressed()))
          return true;
        return this.Control.IsNewReleased() && !this.Modifier1.IsPressed() && !this.Modifier2.IsPressed() && !this.Modifier3.IsPressed();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || (this.Modifier1.IsPressed() || this.Modifier2.IsPressed()) || this.Modifier3.IsPressed() ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => this.Control.ControlCode();

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }

    private class JoystickOneOfTwoModifiers : IMyControllerControl
    {
      private DateTime m_lastNewPress;
      private bool m_repeatStarted;
      public IMyControllerControl PressedModifier;
      public IMyControllerControl ReleasedModifier;
      public IMyControllerControl Control;

      public byte Code => this.Control.Code;

      public Func<bool> Condition { get; private set; }

      public JoystickOneOfTwoModifiers(
        IMyControllerControl pressedModifier,
        IMyControllerControl releasedModifier,
        IMyControllerControl control,
        Func<bool> condition = null)
      {
        this.PressedModifier = pressedModifier;
        this.ReleasedModifier = releasedModifier;
        this.Control = control;
        this.Condition = condition;
      }

      public bool IsNewPressed()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        int num = !this.Control.IsNewPressed() || !this.PressedModifier.IsPressed() ? 0 : (!this.ReleasedModifier.IsPressed() ? 1 : 0);
        if (num == 0)
          return num != 0;
        this.m_lastNewPress = DateTime.Now;
        return num != 0;
      }

      public bool IsNewPressedRepeating()
      {
        bool flag = false;
        int num1 = this.IsNewPressed() ? 1 : 0;
        if (this.IsPressed())
        {
          flag = DateTime.Now - this.m_lastNewPress > (this.m_repeatStarted ? MyControllerHelper.REPEAT_TIME : MyControllerHelper.REPEAT_START_TIME);
          if (flag)
          {
            this.m_repeatStarted = true;
            this.m_lastNewPress = DateTime.Now;
          }
        }
        else
          this.m_repeatStarted = false;
        int num2 = flag ? 1 : 0;
        return (num1 | num2) != 0;
      }

      public bool IsPressed() => (this.Condition == null || this.Condition()) && (this.PressedModifier.IsPressed() && !this.ReleasedModifier.IsPressed()) && this.Control.IsPressed();

      public bool IsNewReleased()
      {
        if (this.Condition != null && !this.Condition())
          return false;
        return this.PressedModifier.IsNewReleased() || this.ReleasedModifier.IsNewPressed() || this.Control.IsNewReleased();
      }

      public float AnalogValue() => this.Condition != null && !this.Condition() || (!this.PressedModifier.IsPressed() || this.ReleasedModifier.IsPressed()) ? 0.0f : this.Control.AnalogValue();

      public object ControlCode() => (object) (this.PressedModifier.ControlCode().ToString() + " + " + this.Control.ControlCode());

      public override string ToString() => (string) this.ControlCode();

      public bool IsNewPressedXinput() => this.IsNewPressed();

      public bool IsNewReleasedXinput() => this.IsNewReleased();
    }
  }
}
