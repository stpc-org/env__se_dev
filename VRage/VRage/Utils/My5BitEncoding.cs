// Decompiled with JetBrains decompiler
// Type: VRage.Utils.My5BitEncoding
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace VRage.Utils
{
  public class My5BitEncoding
  {
    private static My5BitEncoding m_default;
    private char[] m_encodeTable;
    private Dictionary<char, byte> m_decodeTable;

    public static My5BitEncoding Default
    {
      get
      {
        if (My5BitEncoding.m_default == null)
          My5BitEncoding.m_default = new My5BitEncoding();
        return My5BitEncoding.m_default;
      }
    }

    public My5BitEncoding()
      : this(new char[32]
      {
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'J',
        'K',
        'L',
        'M',
        'N',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z'
      })
    {
    }

    public My5BitEncoding(char[] characters)
    {
      if (characters.Length != 32)
        throw new ArgumentException("Characters array must have 32 characters!");
      this.m_encodeTable = new char[32];
      characters.CopyTo((Array) this.m_encodeTable, 0);
      this.m_decodeTable = this.CreateDecodeDict();
    }

    private Dictionary<char, byte> CreateDecodeDict()
    {
      Dictionary<char, byte> dictionary = new Dictionary<char, byte>(this.m_encodeTable.Length);
      for (byte index = 0; (int) index < (int) (byte) this.m_encodeTable.Length; ++index)
        dictionary.Add(this.m_encodeTable[(int) index], index);
      return dictionary;
    }

    public char[] Encode(byte[] data)
    {
      StringBuilder stringBuilder = new StringBuilder(data.Length * 8 / 5);
      int index1 = 0;
      int num1 = 0;
      foreach (byte num2 in data)
      {
        index1 += (int) num2 << num1;
        num1 += 8;
        while (num1 >= 5)
        {
          int index2 = index1 & 31;
          index1 >>= 5;
          num1 -= 5;
          stringBuilder.Append(this.m_encodeTable[index2]);
        }
      }
      if (num1 > 0)
        stringBuilder.Append(this.m_encodeTable[index1]);
      return stringBuilder.ToString().ToCharArray();
    }

    public byte[] Decode(char[] encoded5BitText)
    {
      List<byte> byteList = new List<byte>();
      int num1 = 0;
      int num2 = 0;
      foreach (char key in encoded5BitText)
      {
        byte num3;
        if (!this.m_decodeTable.TryGetValue(key, out num3))
          throw new ArgumentException("Encoded text is not valid for this encoding!");
        num1 += (int) num3 << num2;
        num2 += 5;
        while (num2 >= 8)
        {
          int num4 = num1 & (int) byte.MaxValue;
          num1 >>= 8;
          num2 -= 8;
          byteList.Add((byte) num4);
        }
      }
      return byteList.ToArray();
    }
  }
}
