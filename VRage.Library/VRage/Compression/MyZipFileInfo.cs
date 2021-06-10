// Decompiled with JetBrains decompiler
// Type: VRage.Compression.MyZipFileInfo
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.IO;
using System.IO.Compression;

namespace VRage.Compression
{
  public struct MyZipFileInfo
  {
    private readonly ZipArchiveEntry m_fileInfo;

    internal MyZipFileInfo(ZipArchiveEntry fileInfo) => this.m_fileInfo = fileInfo;

    public string Name => this.m_fileInfo.Name;

    public Stream GetStream() => this.m_fileInfo.Open();

    public long Length => this.m_fileInfo.Length;

    public override string ToString() => this.Name;
  }
}
