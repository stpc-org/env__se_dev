// Decompiled with JetBrains decompiler
// Type: VRage.Compression.MyStreamWrapper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;

namespace VRage.Compression
{
  public class MyStreamWrapper : Stream
  {
    private readonly IDisposable m_obj;
    private Stream m_innerStream;
    private readonly long m_length;
    private readonly MyZipFileInfo m_file;
    private long m_position;

    public MyStreamWrapper(MyZipFileInfo file, IDisposable objectToClose, long length)
    {
      this.m_file = file;
      this.m_innerStream = file.GetStream();
      this.m_obj = objectToClose;
      this.m_length = length;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.m_obj != null)
        this.m_obj.Dispose();
      base.Dispose(disposing);
    }

    public override bool CanRead => this.m_innerStream.CanRead;

    public override bool CanSeek => this.m_innerStream.CanSeek;

    public override bool CanWrite => this.m_innerStream.CanWrite;

    public override void Flush() => this.m_innerStream.Flush();

    public override long Length => this.m_length;

    public override long Position
    {
      get => this.CanSeek ? this.m_innerStream.Position : this.m_position;
      set => this.Seek(value, SeekOrigin.Begin);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int num = this.m_innerStream.Read(buffer, offset, count);
      this.m_position += (long) num;
      return num;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      if (this.m_innerStream.CanSeek)
      {
        this.m_position = this.m_innerStream.Seek(offset, origin);
        return this.m_position;
      }
      switch (origin)
      {
        case SeekOrigin.Begin:
          if (this.m_position <= offset)
          {
            this.m_innerStream.SkipBytes((int) (offset - this.m_position));
            this.m_position = offset;
            return this.m_position;
          }
          break;
        case SeekOrigin.Current:
          if (offset >= 0L)
          {
            this.m_innerStream.SkipBytes((int) offset);
            this.m_position += offset;
            return this.m_position;
          }
          break;
        case SeekOrigin.End:
          if (this.m_position <= this.Length - offset)
          {
            this.m_innerStream.SkipBytes((int) (this.Length - offset - this.m_position));
            this.m_position += this.Length - offset;
            return this.m_position;
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.m_innerStream = this.m_file.GetStream();
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.m_innerStream.SkipBytes((int) offset);
          this.m_position = offset;
          return this.m_position;
        case SeekOrigin.Current:
          this.m_position += offset;
          this.m_innerStream.SkipBytes((int) this.m_position);
          return this.m_position;
        case SeekOrigin.End:
          this.m_innerStream.SkipBytes((int) (this.Length - offset));
          this.m_position = this.Length - offset;
          return this.m_position;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override void SetLength(long value) => this.m_innerStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.m_position += (long) count;
      this.m_innerStream.Write(buffer, offset, count);
    }
  }
}
