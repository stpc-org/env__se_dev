// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyBinaryReader
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.IO;
using System.Security;
using System.Text;

namespace VRage.Utils
{
  public class MyBinaryReader : BinaryReader
  {
    private Decoder m_decoder;
    private int m_maxCharsSize;
    private byte[] m_charBytes;
    private char[] m_charBuffer;

    public MyBinaryReader(Stream stream)
      : this(stream, (Encoding) new UTF8Encoding())
    {
    }

    public MyBinaryReader(Stream stream, Encoding encoding)
      : base(stream, encoding)
    {
      this.m_decoder = encoding.GetDecoder();
      this.m_maxCharsSize = encoding.GetMaxCharCount(128);
      encoding.GetMaxByteCount(1);
    }

    public new int Read7BitEncodedInt()
    {
      int num1 = 0;
      int num2 = 0;
      while (num2 != 35)
      {
        byte num3 = this.ReadByte();
        num1 |= ((int) num3 & (int) sbyte.MaxValue) << num2;
        num2 += 7;
        if (((int) num3 & 128) == 0)
          return num1;
      }
      return -1;
    }

    [SecuritySafeCritical]
    public string ReadStringIncomplete(out bool isComplete)
    {
      if (this.BaseStream == null)
      {
        isComplete = false;
        return string.Empty;
      }
      int num = 0;
      int capacity = this.Read7BitEncodedInt();
      if (capacity < 0)
      {
        isComplete = false;
        return string.Empty;
      }
      if (capacity == 0)
      {
        isComplete = true;
        return string.Empty;
      }
      if (this.m_charBytes == null)
        this.m_charBytes = new byte[128];
      if (this.m_charBuffer == null)
        this.m_charBuffer = new char[this.m_maxCharsSize];
      StringBuilder stringBuilder = (StringBuilder) null;
      do
      {
        int byteCount = this.BaseStream.Read(this.m_charBytes, 0, capacity - num > 128 ? 128 : capacity - num);
        if (byteCount == 0)
        {
          isComplete = false;
          return stringBuilder == null ? string.Empty : stringBuilder.ToString();
        }
        int chars = this.m_decoder.GetChars(this.m_charBytes, 0, byteCount, this.m_charBuffer, 0);
        if (num == 0 && byteCount == capacity)
        {
          isComplete = true;
          return new string(this.m_charBuffer, 0, chars);
        }
        if (stringBuilder == null)
          stringBuilder = new StringBuilder(capacity);
        stringBuilder.Append(this.m_charBuffer, 0, chars);
        num += byteCount;
      }
      while (num < capacity);
      isComplete = true;
      return stringBuilder.ToString();
    }
  }
}
