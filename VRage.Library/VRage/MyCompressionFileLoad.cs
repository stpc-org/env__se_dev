// Decompiled with JetBrains decompiler
// Type: VRage.MyCompressionFileLoad
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
  public class MyCompressionFileLoad : IMyCompressionLoad, IDisposable
  {
    [ThreadStatic]
    private static byte[] m_intBytesBuffer;
    private FileStream m_input;
    private GZipStream m_gz;
    private BufferedStream m_buffer;

    public MyCompressionFileLoad(string fileName)
    {
      if (MyCompressionFileLoad.m_intBytesBuffer == null)
        MyCompressionFileLoad.m_intBytesBuffer = new byte[4];
      this.m_input = File.OpenRead(fileName);
      this.m_input.Read(MyCompressionFileLoad.m_intBytesBuffer, 0, 4);
      this.m_gz = new GZipStream((Stream) this.m_input, CompressionMode.Decompress);
      this.m_buffer = new BufferedStream((Stream) this.m_gz, 16384);
    }

    public void Dispose()
    {
      if (this.m_buffer == null)
        return;
      try
      {
        this.m_buffer.Close();
      }
      finally
      {
        this.m_buffer = (BufferedStream) null;
      }
      try
      {
        this.m_gz.Close();
      }
      finally
      {
        this.m_gz = (GZipStream) null;
      }
      try
      {
        this.m_input.Close();
      }
      finally
      {
        this.m_input = (FileStream) null;
      }
    }

    public int GetInt32()
    {
      this.m_buffer.Read(MyCompressionFileLoad.m_intBytesBuffer, 0, 4);
      return BitConverter.ToInt32(MyCompressionFileLoad.m_intBytesBuffer, 0);
    }

    public byte GetByte() => (byte) this.m_buffer.ReadByte();

    public int GetBytes(int bytes, byte[] output) => this.m_buffer.Read(output, 0, bytes);

    public bool EndOfFile() => this.m_input.Position == this.m_input.Length;

    public byte[] GetCompressedBuffer()
    {
      this.m_input.Position = 0L;
      byte[] buffer = new byte[this.m_input.Length];
      this.m_input.Read(buffer, 0, (int) this.m_input.Length);
      return buffer;
    }
  }
}
