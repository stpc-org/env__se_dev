// Decompiled with JetBrains decompiler
// Type: System.StreamExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.IO;
using System.IO.Compression;
using System.Text;

namespace System
{
  public static class StreamExtensions
  {
    [ThreadStatic]
    private static byte[] m_buffer;

    private static byte[] Buffer
    {
      get
      {
        if (StreamExtensions.m_buffer == null)
          StreamExtensions.m_buffer = new byte[256];
        return StreamExtensions.m_buffer;
      }
    }

    public static bool CheckGZipHeader(this Stream stream)
    {
      if (!stream.CanSeek)
        return false;
      long position = stream.Position;
      byte[] buffer = new byte[2];
      stream.Seek(0L, SeekOrigin.Begin);
      stream.Read(buffer, 0, 2);
      if (buffer[0] == (byte) 31 && buffer[1] == (byte) 139)
      {
        stream.Seek(position, SeekOrigin.Begin);
        return true;
      }
      stream.Seek(position, SeekOrigin.Begin);
      return false;
    }

    public static Stream UnwrapGZip(this Stream stream) => !stream.CheckGZipHeader() ? stream : (Stream) new GZipStream(stream, CompressionMode.Decompress, false);

    public static Stream WrapGZip(this Stream stream, bool buffered = true, bool leaveOpen = false)
    {
      GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen);
      return !buffered ? (Stream) gzipStream : (Stream) new BufferedStream((Stream) gzipStream, 32768);
    }

    public static int Read7BitEncodedInt(this Stream stream)
    {
      byte[] buffer = StreamExtensions.Buffer;
      int num1 = 0;
      int num2 = 0;
      while (num2 != 35)
      {
        byte num3 = stream.Read(buffer, 0, 1) != 0 ? buffer[0] : throw new EndOfStreamException();
        num1 |= ((int) num3 & (int) sbyte.MaxValue) << num2;
        num2 += 7;
        if (((int) num3 & 128) == 0)
          return num1;
      }
      throw new FormatException("Bad string length. 7bit Int32 format");
    }

    public static void Write7BitEncodedInt(this Stream stream, int value)
    {
      byte[] buffer = StreamExtensions.Buffer;
      int count1 = 0;
      uint num1 = (uint) value;
      while (num1 >= 128U)
      {
        buffer[count1++] = (byte) (num1 | 128U);
        num1 >>= 7;
        if (count1 == buffer.Length)
        {
          stream.Write(buffer, 0, count1);
          count1 = 0;
        }
      }
      byte[] numArray = buffer;
      int index = count1;
      int count2 = index + 1;
      int num2 = (int) (byte) num1;
      numArray[index] = (byte) num2;
      stream.Write(buffer, 0, count2);
    }

    public static byte ReadByteNoAlloc(this Stream stream)
    {
      byte[] buffer = StreamExtensions.Buffer;
      return stream.Read(buffer, 0, 1) != 0 ? buffer[0] : throw new EndOfStreamException();
    }

    public static unsafe void WriteNoAlloc(this Stream stream, byte* bytes, int offset, int count)
    {
      byte[] buffer = StreamExtensions.Buffer;
      int count1 = 0;
      int num1 = offset;
      int num2 = offset + count;
      while (num1 != num2)
      {
        buffer[count1++] = bytes[num1++];
        if (count1 == buffer.Length)
        {
          stream.Write(buffer, 0, count1);
          count1 = 0;
        }
      }
      if (count1 == 0)
        return;
      stream.Write(buffer, 0, count1);
    }

    public static unsafe void ReadNoAlloc(this Stream stream, byte* bytes, int offset, int count)
    {
      byte[] buffer = StreamExtensions.Buffer;
      int num1 = offset;
      int num2 = offset + count;
      while (num1 != num2)
      {
        int count1 = Math.Min(count, buffer.Length);
        stream.Read(buffer, 0, count1);
        count -= count1;
        for (int index = 0; index < count1; ++index)
          bytes[num1++] = buffer[index];
      }
    }

    public static void WriteNoAlloc(this Stream stream, string text, Encoding encoding = null)
    {
      encoding = encoding ?? Encoding.UTF8;
      int byteCount = encoding.GetByteCount(text);
      stream.Write7BitEncodedInt(byteCount);
      byte[] numArray = StreamExtensions.Buffer;
      if (byteCount > numArray.Length)
        numArray = new byte[byteCount];
      int bytes = encoding.GetBytes(text, 0, text.Length, numArray, 0);
      stream.Write(numArray, 0, bytes);
    }

    public static string ReadString(this Stream stream, Encoding encoding = null)
    {
      encoding = encoding ?? Encoding.UTF8;
      int count = stream.Read7BitEncodedInt();
      byte[] numArray = StreamExtensions.Buffer;
      if (count > numArray.Length)
        numArray = new byte[count];
      stream.Read(numArray, 0, count);
      return encoding.GetString(numArray, 0, count);
    }

    public static void WriteNoAlloc(this Stream stream, byte value)
    {
      byte[] buffer = StreamExtensions.Buffer;
      buffer[0] = value;
      stream.Write(buffer, 0, 1);
    }

    public static unsafe void WriteNoAlloc(this Stream stream, short v) => stream.WriteNoAlloc((byte*) &v, 0, 2);

    public static unsafe void WriteNoAlloc(this Stream stream, int v) => stream.WriteNoAlloc((byte*) &v, 0, 4);

    public static unsafe void WriteNoAlloc(this Stream stream, long v) => stream.WriteNoAlloc((byte*) &v, 0, 8);

    public static unsafe void WriteNoAlloc(this Stream stream, ushort v) => stream.WriteNoAlloc((byte*) &v, 0, 2);

    public static unsafe void WriteNoAlloc(this Stream stream, uint v) => stream.WriteNoAlloc((byte*) &v, 0, 4);

    public static unsafe void WriteNoAlloc(this Stream stream, ulong v) => stream.WriteNoAlloc((byte*) &v, 0, 8);

    public static unsafe void WriteNoAlloc(this Stream stream, float v) => stream.WriteNoAlloc((byte*) &v, 0, 4);

    public static unsafe void WriteNoAlloc(this Stream stream, double v) => stream.WriteNoAlloc((byte*) &v, 0, 8);

    public static unsafe void WriteNoAlloc(this Stream stream, Decimal v) => stream.WriteNoAlloc((byte*) &v, 0, 16);

    public static unsafe short ReadInt16(this Stream stream)
    {
      short num;
      stream.ReadNoAlloc((byte*) &num, 0, 2);
      return num;
    }

    public static unsafe int ReadInt32(this Stream stream)
    {
      int num;
      stream.ReadNoAlloc((byte*) &num, 0, 4);
      return num;
    }

    public static unsafe long ReadInt64(this Stream stream)
    {
      long num;
      stream.ReadNoAlloc((byte*) &num, 0, 8);
      return num;
    }

    public static unsafe ushort ReadUInt16(this Stream stream)
    {
      ushort num;
      stream.ReadNoAlloc((byte*) &num, 0, 2);
      return num;
    }

    public static unsafe uint ReadUInt32(this Stream stream)
    {
      uint num;
      stream.ReadNoAlloc((byte*) &num, 0, 4);
      return num;
    }

    public static unsafe ulong ReadUInt64(this Stream stream)
    {
      ulong num;
      stream.ReadNoAlloc((byte*) &num, 0, 8);
      return num;
    }

    public static unsafe float ReadFloat(this Stream stream)
    {
      float num;
      stream.ReadNoAlloc((byte*) &num, 0, 4);
      return num;
    }

    public static unsafe double ReadDouble(this Stream stream)
    {
      double num;
      stream.ReadNoAlloc((byte*) &num, 0, 8);
      return num;
    }

    public static unsafe Decimal ReadDecimal(this Stream stream)
    {
      Decimal num;
      stream.ReadNoAlloc((byte*) &num, 0, 16);
      return num;
    }

    public static void SkipBytes(this Stream stream, int byteCount)
    {
      byte[] buffer = StreamExtensions.Buffer;
      int count;
      for (; byteCount > 0; byteCount -= count)
      {
        count = byteCount > buffer.Length ? buffer.Length : byteCount;
        stream.Read(buffer, 0, count);
      }
    }
  }
}
