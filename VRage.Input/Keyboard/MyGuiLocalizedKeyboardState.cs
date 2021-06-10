// Decompiled with JetBrains decompiler
// Type: VRage.Input.Keyboard.MyGuiLocalizedKeyboardState
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System.Collections.Generic;

namespace VRage.Input.Keyboard
{
  internal class MyGuiLocalizedKeyboardState
  {
    private static HashSet<byte> m_localKeys;
    private MyKeyboardState m_previousKeyboardState;
    private MyKeyboardState m_actualKeyboardState;
    private readonly IVRageInput2 m_platformInput;

    public MyGuiLocalizedKeyboardState(IVRageInput2 platformInput)
    {
      this.m_platformInput = platformInput;
      this.m_actualKeyboardState = this.GetCurrentState();
      if (MyGuiLocalizedKeyboardState.m_localKeys != null)
        return;
      MyGuiLocalizedKeyboardState.m_localKeys = new HashSet<byte>();
      this.AddLocalKey(MyKeys.LeftControl);
      this.AddLocalKey(MyKeys.LeftAlt);
      this.AddLocalKey(MyKeys.LeftShift);
      this.AddLocalKey(MyKeys.RightAlt);
      this.AddLocalKey(MyKeys.RightControl);
      this.AddLocalKey(MyKeys.RightShift);
      this.AddLocalKey(MyKeys.Delete);
      this.AddLocalKey(MyKeys.NumPad0);
      this.AddLocalKey(MyKeys.NumPad1);
      this.AddLocalKey(MyKeys.NumPad2);
      this.AddLocalKey(MyKeys.NumPad3);
      this.AddLocalKey(MyKeys.NumPad4);
      this.AddLocalKey(MyKeys.NumPad5);
      this.AddLocalKey(MyKeys.NumPad6);
      this.AddLocalKey(MyKeys.NumPad7);
      this.AddLocalKey(MyKeys.NumPad8);
      this.AddLocalKey(MyKeys.NumPad9);
      this.AddLocalKey(MyKeys.Decimal);
      this.AddLocalKey(MyKeys.LeftWindows);
      this.AddLocalKey(MyKeys.RightWindows);
      this.AddLocalKey(MyKeys.Apps);
      this.AddLocalKey(MyKeys.Pause);
      this.AddLocalKey(MyKeys.Divide);
    }

    private unsafe MyKeyboardState GetCurrentState()
    {
      MyKeyboardBuffer buffer = new MyKeyboardBuffer();
      this.m_platformInput.GetAsyncKeyStates(buffer.Data);
      if (buffer.GetBit((byte) 165))
      {
        buffer.SetBit((byte) 162, false);
        buffer.SetBit((byte) 17, false);
      }
      return MyKeyboardState.FromBuffer(buffer);
    }

    private void AddLocalKey(MyKeys key) => MyGuiLocalizedKeyboardState.m_localKeys.Add((byte) key);

    public void ClearStates()
    {
      this.m_previousKeyboardState = this.m_actualKeyboardState;
      this.m_actualKeyboardState = new MyKeyboardState();
    }

    public void UpdateStates()
    {
      this.m_previousKeyboardState = this.m_actualKeyboardState;
      this.m_actualKeyboardState = this.GetCurrentState();
    }

    public void UpdateStatesFromSnapshot(MyKeyboardState state)
    {
      this.m_previousKeyboardState = this.m_actualKeyboardState;
      this.m_actualKeyboardState = state;
    }

    public void UpdateStatesFromSnapshot(
      MyKeyboardState currentState,
      MyKeyboardState previousState)
    {
      this.m_previousKeyboardState = previousState;
      this.m_actualKeyboardState = currentState;
    }

    public MyKeyboardState GetActualKeyboardState() => this.m_actualKeyboardState;

    public MyKeyboardState GetPreviousKeyboardState() => this.m_previousKeyboardState;

    public void SetKey(MyKeys key, bool value) => this.m_actualKeyboardState.SetKey(key, value);

    public bool IsPreviousKeyDown(MyKeys key, bool isLocalKey)
    {
      if (!isLocalKey)
        key = MyGuiLocalizedKeyboardState.LocalToUSEnglish(key);
      return this.m_previousKeyboardState.IsKeyDown(key);
    }

    public bool IsPreviousKeyDown(MyKeys key) => this.IsPreviousKeyDown(key, this.IsKeyLocal(key));

    public void NegateEscapePress()
    {
      this.m_previousKeyboardState.SetKey(MyKeys.Escape, true);
      this.m_actualKeyboardState.SetKey(MyKeys.Escape, true);
    }

    public bool IsPreviousKeyUp(MyKeys key, bool isLocalKey)
    {
      if (!isLocalKey)
        key = MyGuiLocalizedKeyboardState.LocalToUSEnglish(key);
      return this.m_previousKeyboardState.IsKeyUp(key);
    }

    public bool IsPreviousKeyUp(MyKeys key) => this.IsPreviousKeyUp(key, this.IsKeyLocal(key));

    public bool IsKeyDown(MyKeys key, bool isLocalKey)
    {
      if (!isLocalKey)
        key = MyGuiLocalizedKeyboardState.LocalToUSEnglish(key);
      return this.m_actualKeyboardState.IsKeyDown(key);
    }

    public bool IsKeyUp(MyKeys key, bool isLocalKey)
    {
      if (!isLocalKey)
        key = MyGuiLocalizedKeyboardState.LocalToUSEnglish(key);
      return this.m_actualKeyboardState.IsKeyUp(key);
    }

    public bool IsKeyDown(MyKeys key) => this.IsKeyDown(key, this.IsKeyLocal(key));

    private bool IsKeyLocal(MyKeys key) => MyGuiLocalizedKeyboardState.m_localKeys.Contains((byte) key);

    public bool IsKeyUp(MyKeys key) => this.IsKeyUp(key, this.IsKeyLocal(key));

    private static MyKeys USEnglishToLocal(MyKeys key) => key;

    private static MyKeys LocalToUSEnglish(MyKeys key) => key;

    public bool IsAnyKeyPressed() => this.m_actualKeyboardState.IsAnyKeyPressed();

    public void GetActualPressedKeys(List<MyKeys> keys)
    {
      this.m_actualKeyboardState.GetPressedKeys(keys);
      for (int index = 0; index < keys.Count; ++index)
      {
        if (!this.IsKeyLocal(keys[index]))
          keys[index] = MyGuiLocalizedKeyboardState.USEnglishToLocal(keys[index]);
      }
    }
  }
}
