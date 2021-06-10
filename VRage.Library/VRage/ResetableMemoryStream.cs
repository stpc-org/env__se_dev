// Decompiled with JetBrains decompiler
// Type: VRage.ResetableMemoryStream
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;

namespace VRage
{
  public class ResetableMemoryStream : Stream
  {
    private byte[] m_baseArray;
    private int m_position;
    private int m_length;

    public ResetableMemoryStream()
    {
    }

    public ResetableMemoryStream(byte[] baseArray, int length) => this.Reset(baseArray, length);

    public void Reset(byte[] newBaseArray, int length)
    {
      if (newBaseArray.Length < length)
        throw new ArgumentException("Length must be >= newBaseArray.Length");
      this.m_baseArray = newBaseArray;
      this.m_length = length;
      this.m_position = 0;
    }

    public byte[] GetInternalBuffer() => this.m_baseArray;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override void Flush()
    {
    }

    public override long Length => (long) this.m_length;

    public override long Position
    {
      get => (long) this.m_position;
      set => this.m_position = (int) value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int count1 = this.m_length - this.m_position;
      if (count1 > count)
        count1 = count;
      if (count1 <= 0)
        return 0;
      if (count1 <= 8)
      {
        int num = count1;
        while (--num >= 0)
          buffer[offset + num] = this.m_baseArray[this.m_position + num];
      }
      else
        Buffer.BlockCopy((Array) this.m_baseArray, this.m_position, (Array) buffer, offset, count1);
      this.m_position += count1;
      return count1;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.m_position = (int) offset;
          break;
        case SeekOrigin.Current:
          this.m_position += (int) offset;
          break;
        case SeekOrigin.End:
          this.m_position = this.m_length + (int) offset;
          break;
        default:
          throw new ArgumentException("Invalid seek origin");
      }
      return (long) this.m_position;
    }

    public override void SetLength(long value) => throw new InvalidOperationException("Operation not supported");

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this.m_length < this.m_position + count)
        throw new EndOfStreamException();
      int num1 = this.m_position + count;
      if (count <= 8 && buffer != this.m_baseArray)
      {
        int num2 = count;
        while (--num2 >= 0)
          this.m_baseArray[this.m_position + num2] = buffer[offset + num2];
      }
      else
        Buffer.BlockCopy((Array) buffer, offset, (Array) this.m_baseArray, this.m_position, count);
      this.m_position = num1;
    }
  }
}
