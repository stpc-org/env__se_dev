// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyQuantizer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Utils
{
  internal class MyQuantizer
  {
    private int m_quantizationBits;
    private int m_throwawayBits;
    private int m_minValue;
    private byte[] m_smearBits;
    private uint[] m_bitmask;

    public MyQuantizer(int quantizationBits)
    {
      this.m_quantizationBits = quantizationBits;
      this.m_throwawayBits = 8 - this.m_quantizationBits;
      this.m_smearBits = new byte[1 << this.m_quantizationBits];
      for (uint index = 0; (long) index < (long) (1 << this.m_quantizationBits); ++index)
      {
        uint num1 = index << this.m_throwawayBits;
        uint num2 = num1 + (num1 >> this.m_quantizationBits);
        if (this.m_quantizationBits < 4)
        {
          num2 += num2 >> this.m_quantizationBits * 2;
          if (this.m_quantizationBits < 2)
            num2 += num2 >> this.m_quantizationBits * 4;
        }
        this.m_smearBits[(int) index] = (byte) num2;
      }
      this.m_bitmask = new uint[8]
      {
        ~((uint) byte.MaxValue >> this.m_throwawayBits),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 1),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 2),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 3),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 4),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 5),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 6),
        (uint) ~((int) ((uint) byte.MaxValue >> this.m_throwawayBits) << 7)
      };
      this.m_minValue = 1 << this.m_throwawayBits;
    }

    public byte QuantizeValue(byte val) => this.m_smearBits[(int) val >> this.m_throwawayBits];

    public void SetAllFromUnpacked(byte[] dstPacked, int dstSize, byte[] srcUnpacked)
    {
      Array.Clear((Array) dstPacked, 0, dstPacked.Length);
      int num1 = 0;
      int index1 = 0;
      while (num1 < dstSize * this.m_quantizationBits)
      {
        int index2 = num1 >> 3;
        uint num2 = (uint) srcUnpacked[index1] >> this.m_throwawayBits << (num1 & 7);
        dstPacked[index2] |= (byte) num2;
        dstPacked[index2 + 1] |= (byte) (num2 >> 8);
        num1 += this.m_quantizationBits;
        ++index1;
      }
    }

    public void WriteVal(byte[] packed, int idx, byte val)
    {
      int num1 = idx * this.m_quantizationBits;
      int index1 = num1 & 7;
      int index2 = num1 >> 3;
      uint num2 = (uint) val >> this.m_throwawayBits << index1;
      packed[index2] = (byte) ((uint) packed[index2] & this.m_bitmask[index1] | num2);
      packed[index2 + 1] = (byte) ((uint) packed[index2 + 1] & this.m_bitmask[index1] >> 8 | num2 >> 8);
    }

    public byte ReadVal(byte[] packed, int idx)
    {
      int num = idx * this.m_quantizationBits;
      int index = num >> 3;
      return this.m_smearBits[(long) ((uint) packed[index] + ((uint) packed[index + 1] << 8) >> (num & 7)) & (long) ((int) byte.MaxValue >> this.m_throwawayBits)];
    }

    public int ComputeRequiredPackedSize(int unpackedSize) => (unpackedSize * this.m_quantizationBits + 7) / 8 + 1;

    public int GetMinimumQuantizableValue() => this.m_minValue;
  }
}
