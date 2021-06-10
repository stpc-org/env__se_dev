// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MemoryCompressor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.IO;
using System.IO.Compression;

namespace Sandbox.Game.Replication.StateGroups
{
  internal static class MemoryCompressor
  {
    private static void CopyTo(Stream src, Stream dest)
    {
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) != 0)
        dest.Write(buffer, 0, count);
    }

    public static byte[] Compress(byte[] bytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(bytes))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream2, CompressionMode.Compress))
            MemoryCompressor.CopyTo((Stream) memoryStream1, (Stream) gzipStream);
          return memoryStream2.ToArray();
        }
      }
    }

    public static byte[] Decompress(byte[] bytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(bytes))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream1, CompressionMode.Decompress))
            MemoryCompressor.CopyTo((Stream) gzipStream, (Stream) memoryStream2);
          return memoryStream2.ToArray();
        }
      }
    }
  }
}
