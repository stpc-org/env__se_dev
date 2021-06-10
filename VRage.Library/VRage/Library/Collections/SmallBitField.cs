// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.SmallBitField
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Library.Collections
{
  public struct SmallBitField
  {
    public const int BitCount = 64;
    public const ulong BitsEmpty = 0;
    public const ulong BitsFull = 18446744073709551615;
    public static readonly SmallBitField Empty = new SmallBitField(false);
    public static readonly SmallBitField Full = new SmallBitField(true);
    public ulong Bits;

    public SmallBitField(bool value) => this.Bits = value ? ulong.MaxValue : 0UL;

    public void Reset(bool value) => this.Bits = value ? ulong.MaxValue : 0UL;

    public bool this[int index]
    {
      get => (this.Bits >> index & 1UL) > 0UL;
      set
      {
        if (value)
          this.Bits |= (ulong) (uint) (1 << index);
        else
          this.Bits &= (ulong) (uint) ~(1 << index);
      }
    }
  }
}
