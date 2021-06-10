// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.StringCompressor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sandbox.Game.Entities.Blocks
{
  internal static class StringCompressor
  {
    public static void CopyTo(Stream src, Stream dest)
    {
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) != 0)
        dest.Write(buffer, 0, count);
    }

    public static byte[] CompressString(string str)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(Encoding.UTF8.GetBytes(str)))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream2, CompressionMode.Compress))
            StringCompressor.CopyTo((Stream) memoryStream1, (Stream) gzipStream);
          return memoryStream2.ToArray();
        }
      }
    }

    public static string DecompressString(byte[] bytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(bytes))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream1, CompressionMode.Decompress))
            StringCompressor.CopyTo((Stream) gzipStream, (Stream) memoryStream2);
          return Encoding.UTF8.GetString(memoryStream2.ToArray());
        }
      }
    }
  }
}
