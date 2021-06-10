// Decompiled with JetBrains decompiler
// Type: VRage.Input.Keyboard.MyKeyboardState
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System.Collections.Generic;

namespace VRage.Input.Keyboard
{
  public struct MyKeyboardState
  {
    private MyKeyboardBuffer m_buffer;

    public void GetPressedKeys(List<MyKeys> keys)
    {
      keys.Clear();
      for (int index = 1; index < (int) byte.MaxValue; ++index)
      {
        if (this.m_buffer.GetBit((byte) index))
          keys.Add((MyKeys) index);
      }
    }

    public bool IsAnyKeyPressed() => this.m_buffer.AnyBitSet();

    public void SetKey(MyKeys key, bool value) => this.m_buffer.SetBit((byte) key, value);

    public static MyKeyboardState FromBuffer(MyKeyboardBuffer buffer) => new MyKeyboardState()
    {
      m_buffer = buffer
    };

    public bool IsKeyDown(MyKeys key) => this.m_buffer.GetBit((byte) key);

    public bool IsKeyUp(MyKeys key) => !this.IsKeyDown(key);

    public void AddKey(MyKeys key, bool value) => this.m_buffer.SetBit((byte) key, true);
  }
}
