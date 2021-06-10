// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.PathUtils
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.IO;

namespace VRage.Library.Utils
{
  public static class PathUtils
  {
    public static string[] GetFilesRecursively(string path, string searchPath)
    {
      List<string> paths = new List<string>();
      PathUtils.GetfGetFilesRecursively(path, searchPath, paths);
      return paths.ToArray();
    }

    public static void GetfGetFilesRecursively(string path, string searchPath, List<string> paths)
    {
      paths.AddRange((IEnumerable<string>) Directory.GetFiles(path, searchPath));
      foreach (string directory in Directory.GetDirectories(path))
        PathUtils.GetfGetFilesRecursively(directory, searchPath, paths);
    }
  }
}
