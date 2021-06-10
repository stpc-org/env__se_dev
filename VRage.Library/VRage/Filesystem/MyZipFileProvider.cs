// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyZipFileProvider
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Compression;

namespace VRage.FileSystem
{
  public class MyZipFileProvider : IFileProvider
  {
    public readonly char[] Separators = new char[2]
    {
      Path.AltDirectorySeparatorChar,
      Path.DirectorySeparatorChar
    };

    public Stream Open(string path, FileMode mode, FileAccess access, FileShare share) => mode != FileMode.Open || access != FileAccess.Read ? (Stream) null : this.TryDoZipAction<Stream>(path, new Func<string, string, Stream>(this.TryOpen), (Stream) null);

    private T TryDoZipAction<T>(string path, Func<string, string, T> action, T defaultValue)
    {
      for (int length = path.Length; length >= 0; length = path.LastIndexOfAny(this.Separators, length - 1))
      {
        string path1 = path.Substring(0, length);
        if (File.Exists(path1))
          return action(path1, path.Substring(Math.Min(path.Length, length + 1)));
      }
      return defaultValue;
    }

    private Stream TryOpen(string zipFile, string subpath)
    {
      MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
      try
      {
        if (!myZipArchive.FileExists(subpath))
          return (Stream) null;
        MyZipFileInfo file = myZipArchive.GetFile(subpath);
        return (Stream) new MyStreamWrapper(file, (IDisposable) myZipArchive, file.Length);
      }
      catch
      {
        myZipArchive.Dispose();
        return (Stream) null;
      }
    }

    public bool DirectoryExists(string path) => this.TryDoZipAction<bool>(path, new Func<string, string, bool>(this.DirectoryExistsInZip), false);

    private bool DirectoryExistsInZip(string zipFile, string subpath)
    {
      MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
      try
      {
        return subpath == string.Empty || myZipArchive.DirectoryExists(subpath + "/");
      }
      finally
      {
        myZipArchive.Dispose();
      }
    }

    private MyZipArchive TryGetZipArchive(string zipFile, string subpath)
    {
      MyZipArchive myZipArchive1 = MyZipArchive.OpenOnFile(zipFile);
      try
      {
        return myZipArchive1;
      }
      catch
      {
        MyZipArchive myZipArchive2;
        myZipArchive2.Dispose();
        return (MyZipArchive) null;
      }
    }

    private string TryGetSubpath(string zipFile, string subpath) => subpath;

    public IEnumerable<string> GetFiles(
      string path,
      string filter,
      MySearchOption searchOption)
    {
      MyZipFileProvider myZipFileProvider = this;
      MyZipArchive zipFile = myZipFileProvider.TryDoZipAction<MyZipArchive>(path, new Func<string, string, MyZipArchive>(myZipFileProvider.TryGetZipArchive), (MyZipArchive) null);
      string subpath = "";
      if (searchOption == MySearchOption.TopDirectoryOnly)
        subpath = myZipFileProvider.TryDoZipAction<string>(path, new Func<string, string, string>(myZipFileProvider.TryGetSubpath), (string) null);
      if (zipFile != null)
      {
        string pattern = Regex.Escape(filter).Replace("\\*", ".*").Replace("\\?", ".");
        pattern += "$";
        foreach (string fileName in zipFile.FileNames)
        {
          if ((searchOption != MySearchOption.TopDirectoryOnly || fileName.Count<char>((Func<char, bool>) (x => x == '\\')) == subpath.Count<char>((Func<char, bool>) (x => x == '\\')) + 1) && Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            yield return Path.Combine(zipFile.ZipPath, fileName);
        }
        zipFile.Dispose();
        pattern = (string) null;
      }
    }

    public bool FileExists(string path) => this.TryDoZipAction<bool>(path, new Func<string, string, bool>(this.FileExistsInZip), false);

    private bool FileExistsInZip(string zipFile, string subpath)
    {
      MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
      try
      {
        return myZipArchive.FileExists(subpath);
      }
      finally
      {
        myZipArchive.Dispose();
      }
    }

    public static bool IsZipFile(string path) => !Directory.Exists(path);
  }
}
