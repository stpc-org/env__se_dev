// Decompiled with JetBrains decompiler
// Type: VRage.Compression.MyZipArchive
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;

namespace VRage.Compression
{
  public class MyZipArchive : IDisposable
  {
    private readonly ZipArchive m_zip;
    private readonly Dictionary<string, string> m_mixedCaseHelper = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);

    public string ZipPath { get; private set; }

    private MyZipArchive(ZipArchive zipObject, string path = null)
    {
      this.m_zip = zipObject;
      this.ZipPath = path;
      if (this.m_zip.Mode == ZipArchiveMode.Create)
        return;
      foreach (ZipArchiveEntry file in this.Files)
        this.m_mixedCaseHelper[MyZipArchive.FixName(file.FullName)] = file.FullName;
    }

    public IEnumerable<string> FileNames => (IEnumerable<string>) this.Files.Select<ZipArchiveEntry, string>((Func<ZipArchiveEntry, string>) (p => MyZipArchive.FixName(p.FullName))).OrderBy<string, string>((Func<string, string>) (p => p));

    public ReadOnlyCollection<ZipArchiveEntry> Files => this.m_zip.Entries;

    private static string FixName(string name) => name.Replace('/', '\\');

    public static MyZipArchive OpenOnFile(string path, ZipArchiveMode mode = ZipArchiveMode.Read) => new MyZipArchive(ZipFile.Open(path, mode), path);

    public MyZipFileInfo AddFile(string path, CompressionLevel level) => new MyZipFileInfo(this.m_zip.CreateEntry(path, level));

    public MyZipFileInfo GetFile(string name) => new MyZipFileInfo(this.m_zip.GetEntry(this.m_mixedCaseHelper[MyZipArchive.FixName(name)]));

    public bool FileExists(string name) => this.m_mixedCaseHelper.ContainsKey(MyZipArchive.FixName(name));

    public bool DirectoryExists(string name)
    {
      name = MyZipArchive.FixName(name);
      foreach (string key in this.m_mixedCaseHelper.Keys)
      {
        if (key.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
          return true;
      }
      return false;
    }

    public void Dispose() => this.m_zip.Dispose();

    public static void ExtractToDirectory(
      string sourceArchiveFileName,
      string destinationDirectoryName)
    {
      ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
    }
  }
}
