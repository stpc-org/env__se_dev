// Decompiled with JetBrains decompiler
// Type: VRage.DirectoryExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;

namespace VRage
{
  public static class DirectoryExtensions
  {
    public static void CopyAll(string source, string target)
    {
      DirectoryExtensions.EnsureDirectoryExists(target);
      foreach (FileInfo file in new DirectoryInfo(source).GetFiles())
        file.CopyTo(Path.Combine(target, file.Name), true);
      foreach (DirectoryInfo directory1 in new DirectoryInfo(source).GetDirectories())
      {
        DirectoryInfo directory2 = Directory.CreateDirectory(Path.Combine(target, directory1.Name));
        DirectoryExtensions.CopyAll(directory1.FullName, directory2.FullName);
      }
    }

    public static void EnsureDirectoryExists(string path)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      if (directoryInfo.Parent != null)
        DirectoryExtensions.EnsureDirectoryExists(directoryInfo.Parent.FullName);
      if (directoryInfo.Exists)
        return;
      directoryInfo.Create();
    }

    public static bool IsParentOf(this DirectoryInfo dir, string absPath)
    {
      string str = dir.FullName.TrimEnd(Path.DirectorySeparatorChar);
      for (DirectoryInfo directoryInfo = new DirectoryInfo(absPath); directoryInfo.Exists; directoryInfo = directoryInfo.Parent)
      {
        if (directoryInfo.FullName.TrimEnd(Path.DirectorySeparatorChar).Equals(str, StringComparison.OrdinalIgnoreCase))
          return true;
        if (!directoryInfo.FullName.TrimEnd(Path.DirectorySeparatorChar).StartsWith(str) || directoryInfo.Parent == null)
          return false;
      }
      return false;
    }

    public static ulong GetStorageSize(string sessionPath)
    {
      ulong num = 0;
      foreach (string enumerateFileSystemEntry in Directory.EnumerateFileSystemEntries(sessionPath))
      {
        if (Directory.Exists(enumerateFileSystemEntry))
          num += DirectoryExtensions.GetStorageSize(enumerateFileSystemEntry);
        else
          num += (ulong) new FileInfo(enumerateFileSystemEntry).Length;
      }
      return num;
    }
  }
}
