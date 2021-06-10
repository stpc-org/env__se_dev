// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.BitReader
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using System.Text;

namespace VRage.Library.Collections
{
  public struct BitReader
  {
    private unsafe ulong* m_buffer;
    private int m_bitLength;
    public int BitPosition;
    private const long Int64Msb = -9223372036854775808;
    private const int Int32Msb = -2147483648;

    public unsafe BitReader(IntPtr data, int bitLength)
    {
      this.m_buffer = (ulong*) (void*) data;
      this.m_bitLength = bitLength;
      this.BitPosition = 0;
    }

    public unsafe void Reset(IntPtr data, int bitLength)
    {
      this.m_buffer = (ulong*) (void*) data;
      this.m_bitLength = bitLength;
      this.BitPosition = 0;
    }

    private unsafe ulong ReadInternal(int bitSize)
    {
      if (this.m_bitLength < this.BitPosition + bitSize)
        throw new BitStreamException((Exception) new EndOfStreamException("Cannot read from bit stream, end of stream"));
      int index1 = this.BitPosition >> 6;
      int index2 = this.BitPosition + bitSize - 1 >> 6;
      ulong num1 = ulong.MaxValue >> 64 - bitSize;
      int num2 = this.BitPosition & -65;
      ulong num3 = this.m_buffer[index1] >> num2;
      if (index2 != index1)
        num3 |= this.m_buffer[index2] << 64 - num2;
      this.BitPosition += bitSize;
      return num3 & num1;
    }

    public unsafe double ReadDouble() => *(double*) &this.ReadInternal(64);

    public unsafe float ReadFloat() => *(float*) &this.ReadInternal(32);

    public unsafe Decimal ReadDecimal()
    {
      Decimal num;
      *(long*) &num = (long) this.ReadInternal(64);
      *(long*) ((IntPtr) &num + 8) = (long) this.ReadInternal(64);
      return num;
    }

    public bool ReadBool() => this.ReadInternal(1) > 0UL;

    public sbyte ReadSByte(int bitCount = 8) => (sbyte) this.ReadInternal(bitCount);

    public short ReadInt16(int bitCount = 16) => (short) this.ReadInternal(bitCount);

    public int ReadInt32(int bitCount = 32) => (int) this.ReadInternal(bitCount);

    public long ReadInt64(int bitCount = 64) => (long) this.ReadInternal(bitCount);

    public byte ReadByte(int bitCount = 8) => (byte) this.ReadInternal(bitCount);

    public ushort ReadUInt16(int bitCount = 16) => (ushort) this.ReadInternal(bitCount);

    public uint ReadUInt32(int bitCount = 32) => (uint) this.ReadInternal(bitCount);

    public ulong ReadUInt64(int bitCount = 64) => this.ReadInternal(bitCount);

    private static int Zag(uint ziggedValue)
    {
      int num = (int) ziggedValue;
      return -(num & 1) ^ num >> 1 & int.MaxValue;
    }

    private static long Zag(ulong ziggedValue)
    {
      long num = (long) ziggedValue;
      return -(num & 1L) ^ num >> 1 & long.MaxValue;
    }

    public int ReadInt32Variant() => BitReader.Zag(this.ReadUInt32Variant());

    public long ReadInt64Variant() => BitReader.Zag(this.ReadUInt64Variant());

    public uint ReadUInt32Variant()
    {
      uint num1 = (uint) this.ReadByte();
      if (((int) num1 & 128) == 0)
        return num1;
      uint num2 = num1 & (uint) sbyte.MaxValue;
      uint num3 = (uint) this.ReadByte();
      uint num4 = num2 | (uint) (((int) num3 & (int) sbyte.MaxValue) << 7);
      if (((int) num3 & 128) == 0)
        return num4;
      uint num5 = (uint) this.ReadByte();
      uint num6 = num4 | (uint) (((int) num5 & (int) sbyte.MaxValue) << 14);
      if (((int) num5 & 128) == 0)
        return num6;
      uint num7 = (uint) this.ReadByte();
      uint num8 = num6 | (uint) (((int) num7 & (int) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return num8;
      uint num9 = (uint) this.ReadByte();
      uint num10 = num8 | num9 << 28;
      if (((int) num9 & 240) == 0)
        return num10;
      throw new BitStreamException((Exception) new OverflowException("Error when deserializing variant uint32"));
    }

    public ulong ReadUInt64Variant()
    {
      ulong num1 = (ulong) this.ReadByte();
      if (((long) num1 & 128L) == 0L)
        return num1;
      ulong num2 = num1 & (ulong) sbyte.MaxValue;
      ulong num3 = (ulong) this.ReadByte();
      ulong num4 = num2 | (ulong) (((long) num3 & (long) sbyte.MaxValue) << 7);
      if (((long) num3 & 128L) == 0L)
        return num4;
      ulong num5 = (ulong) this.ReadByte();
      ulong num6 = num4 | (ulong) (((long) num5 & (long) sbyte.MaxValue) << 14);
      if (((long) num5 & 128L) == 0L)
        return num6;
      ulong num7 = (ulong) this.ReadByte();
      ulong num8 = num6 | (ulong) (((long) num7 & (long) sbyte.MaxValue) << 21);
      if (((long) num7 & 128L) == 0L)
        return num8;
      ulong num9 = (ulong) this.ReadByte();
      ulong num10 = num8 | (ulong) (((long) num9 & (long) sbyte.MaxValue) << 28);
      if (((long) num9 & 128L) == 0L)
        return num10;
      ulong num11 = (ulong) this.ReadByte();
      ulong num12 = num10 | (ulong) (((long) num11 & (long) sbyte.MaxValue) << 35);
      if (((long) num11 & 128L) == 0L)
        return num12;
      ulong num13 = (ulong) this.ReadByte();
      ulong num14 = num12 | (ulong) (((long) num13 & (long) sbyte.MaxValue) << 42);
      if (((long) num13 & 128L) == 0L)
        return num14;
      ulong num15 = (ulong) this.ReadByte();
      ulong num16 = num14 | (ulong) (((long) num15 & (long) sbyte.MaxValue) << 49);
      if (((long) num15 & 128L) == 0L)
        return num16;
      ulong num17 = (ulong) this.ReadByte();
      ulong num18 = num16 | (ulong) (((long) num17 & (long) sbyte.MaxValue) << 56);
      if (((long) num17 & 128L) == 0L)
        return num18;
      ulong num19 = (ulong) this.ReadByte();
      ulong num20 = num18 | num19 << 63;
      if (((long) num19 & -2L) != 0L)
        throw new BitStreamException((Exception) new OverflowException("Error when deserializing variant uint64"));
      return num20;
    }

    public char ReadChar(int bitCount = 16) => (char) this.ReadInternal(bitCount);

    public unsafe void ReadMemory(IntPtr ptr, int bitSize) => this.ReadMemory((void*) ptr, bitSize);

    public unsafe void ReadMemory(void* ptr, int bitSize)
    {
      int num = bitSize / 8 / 8;
      ulong* numPtr1 = (ulong*) ptr;
      for (int index = 0; index < num; ++index)
        numPtr1[index] = this.ReadUInt64();
      int val1 = bitSize - num * 8 * 8;
      byte* numPtr2 = (byte*) (numPtr1 + num);
      while (val1 > 0)
      {
        int bitCount = Math.Min(val1, 8);
        *numPtr2 = this.ReadByte(bitCount);
        val1 -= bitCount;
        ++numPtr2;
      }
    }

    public unsafe string ReadPrefixLengthString(Encoding encoding)
    {
      int length = (int) this.ReadUInt32Variant();
      if (length <= 1024)
      {
        byte* bytes = stackalloc byte[length];
        this.ReadMemory((void*) bytes, length * 8);
        int charCount = encoding.GetCharCount(bytes, length);
        char* chars = stackalloc char[charCount];
        encoding.GetChars(bytes, length, chars, charCount);
        return new string(chars, 0, charCount);
      }
      byte[] bytes1 = new byte[length];
      fixed (byte* numPtr = bytes1)
        this.ReadMemory((void*) numPtr, length * 8);
      return new string(encoding.GetChars(bytes1));
    }

    public unsafe void ReadBytes(byte[] bytes, int start, int count)
    {
      fixed (byte* numPtr = bytes)
        this.ReadMemory((void*) (numPtr + start), count * 8);
    }
  }
}
