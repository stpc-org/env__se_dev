// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyClassicFileProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace VRage.FileSystem
{
  public class MyClassicFileProvider : IFileProvider
  {
    public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
      if (!File.Exists(path))
        return (Stream) null;
      try
      {
        return (Stream) File.Open(path, mode, access, share);
      }
      catch (Exception ex)
      {
        return (Stream) null;
      }
    }

    public bool DirectoryExists(string path) => Directory.Exists(path);

    public IEnumerable<string> GetFiles(
      string path,
      string filter,
      MySearchOption searchOption)
    {
      return !Directory.Exists(path) ? (IEnumerable<string>) null : (IEnumerable<string>) Directory.GetFiles(path, filter, (SearchOption) searchOption);
    }

    public bool FileExists(string path) => File.Exists(path);
  }
}
