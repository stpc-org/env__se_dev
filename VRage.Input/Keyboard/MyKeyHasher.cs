// Decompiled with JetBrains decompiler
// Type: VRage.Input.Keyboard.MyKeyHasher
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;
using System.Collections.Generic;
using VRage.Security;

namespace VRage.Input.Keyboard
{
  internal class MyKeyHasher
  {
    public readonly List<MyKeys> Keys = new List<MyKeys>(10);
    private readonly Md5.Hash m_hash = new Md5.Hash();
    private readonly byte[] m_tmpHashData = new byte[256];

    private void ComputeHash(string salt)
    {
      this.Keys.Sort((Comparison<MyKeys>) ((x, y) => ((byte) x).CompareTo((byte) y)));
      int num1 = 0;
      foreach (MyKeys key in this.Keys)
        this.m_tmpHashData[num1++] = (byte) key;
      foreach (char ch in salt)
      {
        byte[] tmpHashData1 = this.m_tmpHashData;
        int index1 = num1;
        int num2 = index1 + 1;
        int num3 = (int) (byte) ch;
        tmpHashData1[index1] = (byte) num3;
        byte[] tmpHashData2 = this.m_tmpHashData;
        int index2 = num2;
        num1 = index2 + 1;
        int num4 = (int) (byte) ((uint) ch >> 8);
        tmpHashData2[index2] = (byte) num4;
      }
      Md5.ComputeHash(this.m_tmpHashData, this.m_hash);
    }

    private static byte HexToByte(char c)
    {
      if (c >= 'a')
        return (byte) (10 + (int) c - 97);
      return c >= 'A' ? (byte) (10 + (int) c - 65) : (byte) ((uint) c - 48U);
    }

    private static byte HexToByte(char c1, char c2) => (byte) ((uint) MyKeyHasher.HexToByte(c1) * 16U + (uint) MyKeyHasher.HexToByte(c2));

    public unsafe bool TestHash(string hash, string salt)
    {
      uint* numPtr = stackalloc uint[4];
      for (int index = 0; index < Math.Min(hash.Length, 32) / 2; ++index)
        *(sbyte*) ((IntPtr) numPtr + index) = (sbyte) MyKeyHasher.HexToByte(hash[index * 2], hash[index * 2 + 1]);
      return this.TestHash(numPtr[0], numPtr[1], numPtr[2], numPtr[3], salt);
    }

    public bool TestHash(uint h0, uint h1, uint h2, uint h3, string salt)
    {
      this.ComputeHash(salt);
      return (int) this.m_hash.A == (int) h0 && (int) this.m_hash.B == (int) h1 && (int) this.m_hash.C == (int) h2 && (int) this.m_hash.D == (int) h3;
    }
  }
}
