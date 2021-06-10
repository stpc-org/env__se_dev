// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.IFileProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.IO;

namespace VRage.FileSystem
{
  public interface IFileProvider
  {
    Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

    bool DirectoryExists(string path);

    IEnumerable<string> GetFiles(
      string path,
      string filter,
      MySearchOption searchOption);

    bool FileExists(string path);
  }
}
