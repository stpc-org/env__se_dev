// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyImageHeaderUtils
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using VRage.FileSystem;

namespace VRage.Library.Utils
{
  public static class MyImageHeaderUtils
  {
    private const uint DDS_MAGIC = 542327876;
    private const uint PNG_MAGIC = 1196314761;

    public static bool Read_DDS_HeaderData(
      string filePath,
      out MyImageHeaderUtils.DDS_HEADER header)
    {
      header = new MyImageHeaderUtils.DDS_HEADER()
      {
        dwReserved1 = new uint[11]
      };
      if (!MyFileSystem.FileExists(filePath))
        return false;
      using (Stream input = MyFileSystem.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader(input))
        {
          if (binaryReader.ReadUInt32() != 542327876U)
            return false;
          header.dwSize = binaryReader.ReadUInt32();
          header.dwFlags = binaryReader.ReadUInt32();
          header.dwHeight = binaryReader.ReadUInt32();
          header.dwWidth = binaryReader.ReadUInt32();
          header.dwPitchOrLinearSize = binaryReader.ReadUInt32();
          header.dwDepth = binaryReader.ReadUInt32();
          header.dwMipMapCount = binaryReader.ReadUInt32();
          for (int index = 0; index < 11; ++index)
            header.dwReserved1[index] = binaryReader.ReadUInt32();
          header.ddspf.dwSize = binaryReader.ReadUInt32();
          header.ddspf.dwFlags = binaryReader.ReadUInt32();
          header.ddspf.dwFourCC = binaryReader.ReadUInt32();
          header.ddspf.dwRGBBitCount = binaryReader.ReadUInt32();
          header.ddspf.dwRBitMask = binaryReader.ReadUInt32();
          header.ddspf.dwGBitMask = binaryReader.ReadUInt32();
          header.ddspf.dwBBitMask = binaryReader.ReadUInt32();
          header.ddspf.dwABitMask = binaryReader.ReadUInt32();
          header.dwCaps = binaryReader.ReadUInt32();
          header.dwCaps2 = binaryReader.ReadUInt32();
          header.dwCaps3 = binaryReader.ReadUInt32();
          header.dwCaps4 = binaryReader.ReadUInt32();
          header.dwReserved2 = binaryReader.ReadUInt32();
        }
      }
      return true;
    }

    public static bool Read_PNG_Dimensions(string filePath, out int width, out int height)
    {
      width = 0;
      height = 0;
      if (!MyFileSystem.FileExists(filePath))
        return false;
      using (Stream input = MyFileSystem.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader(input))
        {
          if (binaryReader.ReadUInt32() != 1196314761U)
            return false;
          binaryReader.ReadBytes(12);
          width = binaryReader.ReadLittleEndianInt32();
          height = binaryReader.ReadLittleEndianInt32();
          return true;
        }
      }
    }

    private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
    {
      byte[] numArray = new byte[4];
      for (int index = 0; index < 4; ++index)
        numArray[3 - index] = binaryReader.ReadByte();
      return BitConverter.ToInt32(numArray, 0);
    }

    public struct DDS_PIXELFORMAT
    {
      public uint dwSize;
      public uint dwFlags;
      public uint dwFourCC;
      public uint dwRGBBitCount;
      public uint dwRBitMask;
      public uint dwGBitMask;
      public uint dwBBitMask;
      public uint dwABitMask;
    }

    public struct DDS_HEADER
    {
      public uint dwSize;
      public uint dwFlags;
      public uint dwHeight;
      public uint dwWidth;
      public uint dwPitchOrLinearSize;
      public uint dwDepth;
      public uint dwMipMapCount;
      public uint[] dwReserved1;
      public MyImageHeaderUtils.DDS_PIXELFORMAT ddspf;
      public uint dwCaps;
      public uint dwCaps2;
      public uint dwCaps3;
      public uint dwCaps4;
      public uint dwReserved2;
    }
  }
}
