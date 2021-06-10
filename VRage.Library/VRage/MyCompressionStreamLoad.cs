// Decompiled with JetBrains decompiler
// Type: VRage.MyCompressionStreamLoad
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
  public class MyCompressionStreamLoad : IMyCompressionLoad
  {
    private static byte[] m_intBytesBuffer = new byte[4];
    private MemoryStream m_input;
    private GZipStream m_gz;
    private BufferedStream m_buffer;

    public MyCompressionStreamLoad(byte[] compressedData)
    {
      this.m_input = new MemoryStream(compressedData);
      this.m_input.Read(MyCompressionStreamLoad.m_intBytesBuffer, 0, 4);
      this.m_gz = new GZipStream((Stream) this.m_input, CompressionMode.Decompress);
      this.m_buffer = new BufferedStream((Stream) this.m_gz, 16384);
    }

    public int GetInt32()
    {
      this.m_buffer.Read(MyCompressionStreamLoad.m_intBytesBuffer, 0, 4);
      return BitConverter.ToInt32(MyCompressionStreamLoad.m_intBytesBuffer, 0);
    }

    public byte GetByte() => (byte) this.m_buffer.ReadByte();

    public int GetBytes(int bytes, byte[] output) => this.m_buffer.Read(output, 0, bytes);

    public bool EndOfFile() => this.m_input.Position == this.m_input.Length;
  }
}
