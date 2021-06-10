// Decompiled with JetBrains decompiler
// Type: VRage.Input.Keyboard.MyKeyboardBuffer
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

namespace VRage.Input.Keyboard
{
  public struct MyKeyboardBuffer
  {
    public unsafe fixed byte Data[32];

    public unsafe void SetBit(byte bit, bool value)
    {
      if (bit == (byte) 0)
        return;
      byte num = (byte) (1 << (int) bit % 8);
      fixed (byte* numPtr1 = this.Data)
      {
        if (value)
        {
          byte* numPtr2 = numPtr1 + (int) bit / 8;
          *numPtr2 = (byte) ((uint) *numPtr2 | (uint) num);
        }
        else
        {
          byte* numPtr2 = numPtr1 + (int) bit / 8;
          *numPtr2 = (byte) ((uint) *numPtr2 & (uint) ~num);
        }
      }
    }

    public unsafe bool AnyBitSet()
    {
      fixed (byte* numPtr = this.Data)
        return (ulong) (*(long*) numPtr + ((long*) numPtr)[1] + ((long*) numPtr)[2] + ((long*) numPtr)[3]) > 0UL;
    }

    public unsafe bool GetBit(byte bit)
    {
      byte num = (byte) (1 << (int) bit % 8);
      fixed (byte* numPtr = this.Data)
        return ((uint) numPtr[(int) bit / 8] & (uint) num) > 0U;
    }
  }
}
