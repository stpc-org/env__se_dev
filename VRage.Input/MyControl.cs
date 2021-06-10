// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyControl
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System.Text;
using VRage.ModAPI;
using VRage.Utils;

namespace VRage.Input
{
  public class MyControl : IMyControl
  {
    private const int DEFAULT_CAPACITY = 16;
    private static StringBuilder m_toStringCache = new StringBuilder(16);
    private MyControl.Data m_data;

    private MyStringId m_name
    {
      get => this.m_data.Name;
      set => this.m_data.Name = value;
    }

    private MyStringId m_controlId
    {
      get => this.m_data.ControlId;
      set => this.m_data.ControlId = value;
    }

    private MyGuiControlTypeEnum m_controlType
    {
      get => this.m_data.ControlType;
      set => this.m_data.ControlType = value;
    }

    private MyKeys m_keyboardKey
    {
      get => this.m_data.KeyboardKey;
      set => this.m_data.KeyboardKey = value;
    }

    private MyKeys m_KeyboardKey2
    {
      get => this.m_data.KeyboardKey2;
      set => this.m_data.KeyboardKey2 = value;
    }

    private MyMouseButtonsEnum m_mouseButton
    {
      get => this.m_data.MouseButton;
      set => this.m_data.MouseButton = value;
    }

    public MyControl(
      MyStringId controlId,
      MyStringId name,
      MyGuiControlTypeEnum controlType,
      MyMouseButtonsEnum? defaultControlMouse,
      MyKeys? defaultControlKey,
      MyStringId? helpText = null,
      MyKeys? defaultControlKey2 = null,
      MyStringId? description = null)
    {
      this.m_controlId = controlId;
      this.m_name = name;
      this.m_controlType = controlType;
      MyMouseButtonsEnum? nullable1 = defaultControlMouse;
      this.m_mouseButton = nullable1.HasValue ? nullable1.GetValueOrDefault() : MyMouseButtonsEnum.None;
      MyKeys? nullable2 = defaultControlKey;
      this.m_keyboardKey = nullable2.HasValue ? nullable2.GetValueOrDefault() : MyKeys.None;
      nullable2 = defaultControlKey2;
      this.m_KeyboardKey2 = nullable2.HasValue ? nullable2.GetValueOrDefault() : MyKeys.None;
      this.m_data.Description = description;
    }

    public MyControl(MyControl other) => this.CopyFrom(other);

    public void SetControl(MyGuiInputDeviceEnum device, MyKeys key)
    {
      if (device == MyGuiInputDeviceEnum.Keyboard)
      {
        this.m_keyboardKey = key;
      }
      else
      {
        if (device != MyGuiInputDeviceEnum.KeyboardSecond)
          return;
        this.m_KeyboardKey2 = key;
      }
    }

    public void SetControl(MyMouseButtonsEnum mouseButton) => this.m_mouseButton = mouseButton;

    public void SetNoControl()
    {
      this.m_mouseButton = MyMouseButtonsEnum.None;
      this.m_keyboardKey = MyKeys.None;
      this.m_KeyboardKey2 = MyKeys.None;
    }

    public MyKeys GetKeyboardControl() => this.m_keyboardKey;

    public MyKeys GetSecondKeyboardControl() => this.m_KeyboardKey2;

    public MyMouseButtonsEnum GetMouseControl() => this.m_mouseButton;

    public bool IsPressed()
    {
      bool flag = false;
      if (this.m_keyboardKey != MyKeys.None)
        flag = MyInput.Static.IsKeyPress(this.m_keyboardKey);
      if (this.m_KeyboardKey2 != MyKeys.None && !flag)
        flag = MyInput.Static.IsKeyPress(this.m_KeyboardKey2);
      if (this.m_mouseButton != MyMouseButtonsEnum.None && !flag)
        flag = MyInput.Static.IsMousePressed(this.m_mouseButton);
      return flag;
    }

    public bool IsNewPressed()
    {
      bool flag = false;
      if (this.m_keyboardKey != MyKeys.None)
        flag = MyInput.Static.IsNewKeyPressed(this.m_keyboardKey);
      if (this.m_KeyboardKey2 != MyKeys.None && !flag)
        flag = MyInput.Static.IsNewKeyPressed(this.m_KeyboardKey2);
      if (this.m_mouseButton != MyMouseButtonsEnum.None && !flag)
        flag = MyInput.Static.IsNewMousePressed(this.m_mouseButton);
      return flag;
    }

    public bool IsNewReleased()
    {
      bool flag = false;
      if (this.m_keyboardKey != MyKeys.None)
        flag = MyInput.Static.IsNewKeyReleased(this.m_keyboardKey);
      if (this.m_KeyboardKey2 != MyKeys.None && !flag)
        flag = MyInput.Static.IsNewKeyReleased(this.m_KeyboardKey2);
      if (this.m_mouseButton != MyMouseButtonsEnum.None && !flag)
      {
        switch (this.m_mouseButton)
        {
          case MyMouseButtonsEnum.Left:
            flag = MyInput.Static.IsNewLeftMouseReleased();
            break;
          case MyMouseButtonsEnum.Middle:
            flag = MyInput.Static.IsNewMiddleMouseReleased();
            break;
          case MyMouseButtonsEnum.Right:
            flag = MyInput.Static.IsNewRightMouseReleased();
            break;
          case MyMouseButtonsEnum.XButton1:
            flag = MyInput.Static.IsNewXButton1MouseReleased();
            break;
          case MyMouseButtonsEnum.XButton2:
            flag = MyInput.Static.IsNewXButton2MouseReleased();
            break;
        }
      }
      return flag;
    }

    public bool IsJoystickPressed() => false;

    public bool IsNewJoystickPressed() => false;

    public bool IsNewJoystickReleased() => false;

    public float GetAnalogState()
    {
      bool flag = false;
      if (this.m_keyboardKey != MyKeys.None)
        flag = MyInput.Static.IsKeyPress(this.m_keyboardKey);
      if (this.m_KeyboardKey2 != MyKeys.None && !flag)
        flag = MyInput.Static.IsKeyPress(this.m_KeyboardKey2);
      if (this.m_mouseButton != MyMouseButtonsEnum.None && !flag)
      {
        switch (this.m_mouseButton)
        {
          case MyMouseButtonsEnum.Left:
            flag = MyInput.Static.IsLeftMousePressed();
            break;
          case MyMouseButtonsEnum.Middle:
            flag = MyInput.Static.IsMiddleMousePressed();
            break;
          case MyMouseButtonsEnum.Right:
            flag = MyInput.Static.IsRightMousePressed();
            break;
          case MyMouseButtonsEnum.XButton1:
            flag = MyInput.Static.IsXButton1MousePressed();
            break;
          case MyMouseButtonsEnum.XButton2:
            flag = MyInput.Static.IsXButton2MousePressed();
            break;
        }
      }
      return flag ? 1f : 0.0f;
    }

    public MyStringId GetControlName() => this.m_name;

    public MyStringId? GetControlDescription() => this.m_data.Description;

    public MyGuiControlTypeEnum GetControlTypeEnum() => this.m_controlType;

    public MyStringId GetGameControlEnum() => this.m_controlId;

    public bool IsControlAssigned() => this.m_keyboardKey != MyKeys.None || (uint) this.m_mouseButton > 0U;

    public bool IsControlAssigned(MyGuiInputDeviceEnum deviceType)
    {
      bool flag = false;
      switch (deviceType)
      {
        case MyGuiInputDeviceEnum.Keyboard:
          flag = (uint) this.m_keyboardKey > 0U;
          break;
        case MyGuiInputDeviceEnum.Mouse:
          flag = (uint) this.m_mouseButton > 0U;
          break;
      }
      return flag;
    }

    public void CopyFrom(MyControl other) => this.m_data = other.m_data;

    public override string ToString() => this.ButtonNames.UpdateControlsToNotificationFriendly();

    public string ButtonNames
    {
      get
      {
        lock (MyControl.m_toStringCache)
        {
          MyControl.m_toStringCache.Clear();
          this.AppendBoundButtonNames(ref MyControl.m_toStringCache, unassignedText: MyInput.Static.GetUnassignedName());
          return MyControl.m_toStringCache.ToString();
        }
      }
    }

    public string ButtonNamesIgnoreSecondary
    {
      get
      {
        lock (MyControl.m_toStringCache)
        {
          MyControl.m_toStringCache.Clear();
          this.AppendBoundButtonNames(ref MyControl.m_toStringCache, includeSecondary: false);
          return MyControl.m_toStringCache.ToString();
        }
      }
    }

    public StringBuilder ToStringBuilder(string unassignedText)
    {
      lock (MyControl.m_toStringCache)
      {
        MyControl.m_toStringCache.Clear();
        this.AppendBoundButtonNames(ref MyControl.m_toStringCache, unassignedText: unassignedText);
        return new StringBuilder(MyControl.m_toStringCache.Length).AppendStringBuilder(MyControl.m_toStringCache);
      }
    }

    public string GetControlButtonName(MyGuiInputDeviceEnum deviceType)
    {
      lock (MyControl.m_toStringCache)
      {
        MyControl.m_toStringCache.Clear();
        this.AppendBoundButtonNames(ref MyControl.m_toStringCache, deviceType);
        return MyControl.m_toStringCache.ToString();
      }
    }

    public void AppendBoundKeyJustOne(ref StringBuilder output)
    {
      MyControl.EnsureExists(ref output);
      if (this.m_keyboardKey != MyKeys.None)
        MyControl.AppendName(ref output, this.m_keyboardKey);
      else
        MyControl.AppendName(ref output, this.m_KeyboardKey2);
    }

    public void AppendBoundButtonNames(
      ref StringBuilder output,
      MyGuiInputDeviceEnum device,
      string separator = null)
    {
      MyControl.EnsureExists(ref output);
      switch (device)
      {
        case MyGuiInputDeviceEnum.Keyboard:
          if (separator == null)
          {
            MyControl.AppendName(ref output, this.m_keyboardKey);
            break;
          }
          MyControl.AppendName(ref output, this.m_keyboardKey, this.m_KeyboardKey2, separator);
          break;
        case MyGuiInputDeviceEnum.Mouse:
          MyControl.AppendName(ref output, this.m_mouseButton);
          break;
        case MyGuiInputDeviceEnum.KeyboardSecond:
          if (separator == null)
          {
            MyControl.AppendName(ref output, this.m_KeyboardKey2);
            break;
          }
          MyControl.AppendName(ref output, this.m_keyboardKey, this.m_KeyboardKey2, separator);
          break;
      }
    }

    public void AppendBoundButtonNames(
      ref StringBuilder output,
      string separator = ", ",
      string unassignedText = null,
      bool includeSecondary = true)
    {
      MyControl.EnsureExists(ref output);
      MyGuiInputDeviceEnum[] guiInputDeviceEnumArray = new MyGuiInputDeviceEnum[2]
      {
        MyGuiInputDeviceEnum.Keyboard,
        MyGuiInputDeviceEnum.Mouse
      };
      int num = 0;
      foreach (MyGuiInputDeviceEnum guiInputDeviceEnum in guiInputDeviceEnumArray)
      {
        if (this.IsControlAssigned(guiInputDeviceEnum))
        {
          if (num > 0)
            output.Append(separator);
          this.AppendBoundButtonNames(ref output, guiInputDeviceEnum, includeSecondary ? separator : (string) null);
          ++num;
        }
      }
      if (num != 0 || unassignedText == null)
        return;
      output.Append(unassignedText);
    }

    public static void AppendName(ref StringBuilder output, MyKeys key)
    {
      MyControl.EnsureExists(ref output);
      if (key == MyKeys.None)
        return;
      output.Append(MyInput.Static.GetKeyName(key));
    }

    public static void AppendName(
      ref StringBuilder output,
      MyKeys key1,
      MyKeys key2,
      string separator)
    {
      MyControl.EnsureExists(ref output);
      string str1 = (string) null;
      string str2 = (string) null;
      if (key1 != MyKeys.None)
        str1 = MyInput.Static.GetKeyName(key1);
      if (key2 != MyKeys.None)
        str2 = MyInput.Static.GetKeyName(key2);
      if (str1 != null && str2 != null)
        output.Append(str1).Append(separator).Append(str2);
      else if (str1 != null)
      {
        output.Append(str1);
      }
      else
      {
        if (str2 == null)
          return;
        output.Append(str2);
      }
    }

    public static void AppendName(ref StringBuilder output, MyMouseButtonsEnum mouseButton)
    {
      MyControl.EnsureExists(ref output);
      output.Append(MyInput.Static.GetName(mouseButton));
    }

    public static void AppendName(ref StringBuilder output, MyJoystickButtonsEnum joystickButton)
    {
      MyControl.EnsureExists(ref output);
      output.Append(MyInput.Static.GetName(joystickButton));
    }

    public static void AppendName(ref StringBuilder output, MyJoystickAxesEnum joystickAxis)
    {
      MyControl.EnsureExists(ref output);
      output.Append(MyInput.Static.GetName(joystickAxis));
    }

    public static void AppendUnknownTextIfNeeded(ref StringBuilder output, string unassignedText)
    {
      MyControl.EnsureExists(ref output);
      if (output.Length != 0)
        return;
      output.Append(unassignedText);
    }

    private static void EnsureExists(ref StringBuilder output)
    {
      if (output != null)
        return;
      output = new StringBuilder(16);
    }

    private struct Data
    {
      public MyStringId Name;
      public MyStringId ControlId;
      public MyGuiControlTypeEnum ControlType;
      public MyKeys KeyboardKey;
      public MyKeys KeyboardKey2;
      public MyMouseButtonsEnum MouseButton;
      public MyStringId? Description;
    }
  }
}
