// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyFileSystem
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage.Library.Filesystem;

namespace VRage.FileSystem
{
  public static class MyFileSystem
  {
    public static readonly Assembly MainAssembly;
    public static readonly string MainAssemblyName;
    public static string ExePath;
    public static string RootPath;
    private static string m_shadersBasePath;
    private static string m_contentPath;
    private static string m_modsPath;
    private static string m_cachePath;
    private static string m_tempPath;
    private static string m_userDataPath;
    private static string m_savesPath;
    public static IFileVerifier FileVerifier;
    private static MyFileProviderAggregator m_fileProvider;

    public static string ShadersBasePath
    {
      get
      {
        MyFileSystem.CheckInitialized();
        return MyFileSystem.m_shadersBasePath;
      }
    }

    public static string ContentPath
    {
      get
      {
        MyFileSystem.CheckInitialized();
        return MyFileSystem.m_contentPath;
      }
    }

    public static string ModsPath
    {
      get
      {
        MyFileSystem.CheckInitialized();
        return MyFileSystem.m_modsPath;
      }
    }

    public static string UserDataPath
    {
      get
      {
        MyFileSystem.CheckInitialized();
        return MyFileSystem.m_userDataPath;
      }
    }

    public static string SavesPath
    {
      get
      {
        MyFileSystem.CheckUserSpecificInitialized();
        return MyFileSystem.m_savesPath;
      }
    }

    public static string CachePath
    {
      get
      {
        MyFileSystem.CheckUserSpecificInitialized();
        return MyFileSystem.m_cachePath;
      }
    }

    public static string TempPath
    {
      get
      {
        MyFileSystem.CheckUserSpecificInitialized();
        return MyFileSystem.m_tempPath;
      }
    }

    public static bool IsInitialized => MyFileSystem.m_contentPath != null;

    private static void CheckInitialized()
    {
      if (!MyFileSystem.IsInitialized)
        throw new InvalidOperationException("Paths are not initialized, call 'Init'");
    }

    private static void CheckUserSpecificInitialized()
    {
      if (MyFileSystem.m_userDataPath == null)
        throw new InvalidOperationException("User specific path not initialized, call 'InitUserSpecific'");
    }

    public static void Init(
      string contentPath,
      string userData,
      string modDirName = "Mods",
      string shadersBasePath = null)
    {
      MyFileSystem.m_contentPath = MyFileSystem.m_contentPath == null ? new DirectoryInfo(contentPath).FullName : throw new InvalidOperationException("Paths already initialized");
      MyFileSystem.m_contentPath = MyFileSystem.UnTerminatePath(MyFileSystem.m_contentPath);
      MyFileSystem.m_shadersBasePath = string.IsNullOrEmpty(shadersBasePath) ? MyFileSystem.m_contentPath : Path.GetFullPath(shadersBasePath);
      MyFileSystem.m_userDataPath = Path.GetFullPath(userData);
      MyFileSystem.m_modsPath = Path.Combine(MyFileSystem.m_userDataPath, modDirName);
      MyFileSystem.m_cachePath = Path.Combine(MyFileSystem.m_userDataPath, "cache");
      MyFileSystem.m_tempPath = Path.Combine(MyFileSystem.m_userDataPath, "temp");
      Directory.CreateDirectory(MyFileSystem.m_modsPath);
      Directory.CreateDirectory(MyFileSystem.m_cachePath);
      Directory.CreateDirectory(MyFileSystem.m_tempPath);
      string str = Path.Combine(contentPath, "Content.index");
      if (!File.Exists(str))
        return;
      ContentIndex.Load(str);
    }

    public static void InitUserSpecific(string userSpecificName, string saveDirName = "Saves")
    {
      MyFileSystem.CheckInitialized();
      if (MyFileSystem.m_savesPath != null)
        throw new InvalidOperationException("User specific paths already initialized");
      MyFileSystem.m_savesPath = Path.Combine(MyFileSystem.m_userDataPath, saveDirName, userSpecificName ?? string.Empty);
      Directory.CreateDirectory(MyFileSystem.m_savesPath);
    }

    public static void Reset()
    {
      // ISSUE: variable of the null type
      __Null local;
      MyFileSystem.m_savesPath = (string) (local = null);
      MyFileSystem.m_userDataPath = (string) local;
      MyFileSystem.m_modsPath = (string) local;
      MyFileSystem.m_shadersBasePath = (string) local;
      MyFileSystem.m_contentPath = (string) local;
    }

    public static void ReplaceFileProvider<TReplaced>(IFileProvider instance)
    {
      IFileProvider provider1 = (IFileProvider) null;
      foreach (IFileProvider provider2 in MyFileSystem.m_fileProvider.Providers)
      {
        if (provider2 is TReplaced)
        {
          provider1 = provider2;
          break;
        }
      }
      if (provider1 != null)
        MyFileSystem.m_fileProvider.RemoveProvider(provider1);
      MyFileSystem.m_fileProvider.AddProvider(instance);
    }

    public static Stream Open(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share)
    {
      bool flag = mode == FileMode.Open && access != FileAccess.Write;
      Stream stream = MyFileSystem.m_fileProvider.Open(path, mode, access, share);
      return !flag || stream == null ? stream : MyFileSystem.FileVerifier.Verify(path, stream);
    }

    public static Stream OpenRead(string path) => MyFileSystem.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

    public static Stream OpenRead(string path, string subpath) => MyFileSystem.OpenRead(Path.Combine(path, subpath));

    public static Stream OpenWrite(string path, FileMode mode = FileMode.Create)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      return (Stream) File.Open(path, mode, FileAccess.Write, FileShare.Read);
    }

    public static Stream OpenWrite(string path, string subpath, FileMode mode = FileMode.Create) => MyFileSystem.OpenWrite(Path.Combine(path, subpath), mode);

    public static bool CheckFileWriteAccess(string path)
    {
      try
      {
        using (MyFileSystem.OpenWrite(path, FileMode.Append))
          return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool FileExists(string path)
    {
      if (string.IsNullOrEmpty(path))
        return false;
      if (MyFileSystem.IsInitialized && ContentIndex.IsLoaded)
      {
        string fullPath = Path.GetFullPath(path);
        if (fullPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase))
          return fullPath.Length != MyFileSystem.m_contentPath.Length && ContentIndex.FileExists(fullPath.Substring(MyFileSystem.m_contentPath.Length + 1));
      }
      return MyFileSystem.m_fileProvider.FileExists(path);
    }

    public static bool DirectoryExists(string path) => MyFileSystem.m_fileProvider.DirectoryExists(path);

    public static IEnumerable<string> GetFiles(string path) => MyFileSystem.m_fileProvider.GetFiles(path, "*", MySearchOption.AllDirectories);

    public static IEnumerable<string> GetFiles(string path, string filter) => MyFileSystem.m_fileProvider.GetFiles(path, filter, MySearchOption.AllDirectories);

    public static IEnumerable<string> GetFiles(
      string path,
      string filter,
      MySearchOption searchOption)
    {
      return MyFileSystem.m_fileProvider.GetFiles(path, filter, searchOption);
    }

    public static string MakeRelativePath(string fromPath, string toPath)
    {
      if (string.IsNullOrEmpty(fromPath))
        throw new ArgumentNullException(nameof (fromPath));
      if (string.IsNullOrEmpty(toPath))
        throw new ArgumentNullException(nameof (toPath));
      Uri uri1 = new Uri(fromPath);
      Uri uri2 = new Uri(toPath);
      if (uri1.Scheme != uri2.Scheme)
        return toPath;
      string str = Uri.UnescapeDataString(uri1.MakeRelativeUri(uri2).ToString());
      if (uri2.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
        str = str.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      return str;
    }

    public static string TerminatePath(string path) => !string.IsNullOrEmpty(path) && (int) path[path.Length - 1] == (int) Path.DirectorySeparatorChar ? path : path + Path.DirectorySeparatorChar.ToString();

    public static string UnTerminatePath(string path) => path.Length > 1 && (int) path[MyFileSystem.m_contentPath.Length - 1] == (int) Path.DirectorySeparatorChar ? path.Substring(0, path.Length - 1) : path;

    public static void CopyAll(string source, string target)
    {
      MyFileSystem.EnsureDirectoryExists(target);
      foreach (FileInfo file in new DirectoryInfo(source).GetFiles())
        file.CopyTo(Path.Combine(target, file.Name), true);
      foreach (DirectoryInfo directory1 in new DirectoryInfo(source).GetDirectories())
      {
        DirectoryInfo directory2 = Directory.CreateDirectory(Path.Combine(target, directory1.Name));
        MyFileSystem.CopyAll(directory1.FullName, directory2.FullName);
      }
    }

    public static void CopyAll(string source, string target, Predicate<string> condition)
    {
      MyFileSystem.EnsureDirectoryExists(target);
      foreach (FileInfo file in new DirectoryInfo(source).GetFiles())
      {
        if (condition(file.FullName))
          file.CopyTo(Path.Combine(target, file.Name), true);
      }
      foreach (DirectoryInfo directory1 in new DirectoryInfo(source).GetDirectories())
      {
        if (condition(directory1.FullName))
        {
          DirectoryInfo directory2 = Directory.CreateDirectory(Path.Combine(target, directory1.Name));
          MyFileSystem.CopyAll(directory1.FullName, directory2.FullName, condition);
        }
      }
    }

    public static void EnsureDirectoryExists(string path)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      if (directoryInfo.Parent != null)
        MyFileSystem.EnsureDirectoryExists(directoryInfo.Parent.FullName);
      if (directoryInfo.Exists)
        return;
      directoryInfo.Create();
    }

    public static bool IsDirectory(string path) => MyFileSystem.DirectoryExists(path) && File.GetAttributes(path).HasFlag((Enum) FileAttributes.Directory);

    public static void CreateDirectoryRecursive(string path)
    {
      if (string.IsNullOrEmpty(path) || MyFileSystem.DirectoryExists(path))
        return;
      MyFileSystem.CreateDirectoryRecursive(Path.GetDirectoryName(path));
      Directory.CreateDirectory(path);
    }

    public static bool IsGameContent(string path)
    {
      if (!Path.IsPathRooted(path) || path.StartsWith(MyFileSystem.ContentPath, StringComparison.OrdinalIgnoreCase))
        return true;
      string path1 = path.Replace('/', '\\');
      return (object) path != (object) path1 && MyFileSystem.IsGameContent(path1);
    }

    static MyFileSystem()
    {
      Assembly assembly = Assembly.GetEntryAssembly();
      if ((object) assembly == null)
        assembly = Assembly.GetCallingAssembly();
      MyFileSystem.MainAssembly = assembly;
      MyFileSystem.MainAssemblyName = MyFileSystem.MainAssembly.GetName().Name;
      MyFileSystem.ExePath = new FileInfo(MyFileSystem.MainAssembly.Location).DirectoryName;
      MyFileSystem.RootPath = new FileInfo(MyFileSystem.ExePath).Directory?.FullName ?? Path.GetFullPath(MyFileSystem.ExePath);
      MyFileSystem.FileVerifier = (IFileVerifier) new MyNullVerifier();
      MyFileSystem.m_fileProvider = new MyFileProviderAggregator(new IFileProvider[2]
      {
        (IFileProvider) new MyClassicFileProvider(),
        (IFileProvider) new MyZipFileProvider()
      });
    }
  }
}
