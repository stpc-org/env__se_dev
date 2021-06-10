// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyContentPath
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.FileSystem
{
  public struct MyContentPath
  {
    private const string DEFAULT = "";
    private string m_absolutePath;
    private string m_rootFolder;
    private string m_alternatePath;
    public string Path;
    public string ModFolder;

    public string Absolute => this.m_absolutePath;

    public string RootFolder => this.m_rootFolder;

    public string AlternatePath => this.m_alternatePath;

    public bool AbsoluteFileExists => this.m_absolutePath != null && MyFileSystem.FileExists(this.Absolute);

    public bool AbsoluteDirExists => this.m_absolutePath != null && MyFileSystem.DirectoryExists(this.Absolute);

    public bool AlternateFileExists => this.m_alternatePath != null && MyFileSystem.FileExists(this.AlternatePath);

    public bool AlternateDirExists => this.m_alternatePath != null && MyFileSystem.DirectoryExists(this.AlternatePath);

    public MyContentPath(string path = null, string possibleModPath = null)
    {
      this.Path = "";
      this.ModFolder = "";
      this.m_absolutePath = "";
      this.m_rootFolder = "";
      this.m_alternatePath = "";
      this.SetPath(path, possibleModPath);
    }

    public string GetExitingFilePath()
    {
      if (this.AbsoluteFileExists)
        return this.Absolute;
      return this.AlternateFileExists ? this.AlternatePath : "";
    }

    public void SetPath(string path, string possibleModPath = null)
    {
      this.Path = path;
      this.ModFolder = "";
      this.m_absolutePath = "";
      this.m_rootFolder = "";
      this.m_alternatePath = "";
      if (!string.IsNullOrEmpty(path) && !System.IO.Path.IsPathRooted(path))
      {
        string path1 = System.IO.Path.Combine(MyFileSystem.ContentPath, path);
        string path2 = possibleModPath == null ? System.IO.Path.Combine(MyFileSystem.ModsPath, path) : System.IO.Path.Combine(MyFileSystem.ModsPath, possibleModPath, path);
        if (MyFileSystem.FileExists(path2))
        {
          this.Path = path2;
          path = this.Path;
        }
        else
          this.Path = !MyFileSystem.FileExists(path1) ? (!MyFileSystem.DirectoryExists(path2) ? (!MyFileSystem.DirectoryExists(path1) ? "" : path1) : path2) : path1;
      }
      if (string.IsNullOrEmpty(this.Path))
        return;
      if (this.Path.StartsWith(MyFileSystem.ContentPath))
        this.Path = MyFileSystem.ContentPath.Length == this.Path.Length ? "" : this.Path.Remove(0, MyFileSystem.ContentPath.Length + 1);
      else if (this.Path.StartsWith(MyFileSystem.ModsPath))
      {
        this.Path = this.Path.Remove(0, MyFileSystem.ModsPath.Length + 1);
        int length = this.Path.IndexOf('\\');
        if (length == -1)
        {
          this.ModFolder = this.Path;
          this.Path = "";
          this.SetupHelperPaths();
          return;
        }
        this.ModFolder = this.Path.Substring(0, length);
        this.Path = this.Path.Remove(0, length + 1);
      }
      else
        this.Path = path;
      this.SetupHelperPaths();
    }

    private void SetupHelperPaths()
    {
      this.m_absolutePath = string.IsNullOrEmpty(this.ModFolder) ? System.IO.Path.Combine(MyFileSystem.ContentPath, this.Path) : System.IO.Path.Combine(MyFileSystem.ModsPath, this.ModFolder, this.Path);
      this.m_rootFolder = string.IsNullOrEmpty(this.ModFolder) ? MyFileSystem.ContentPath : System.IO.Path.Combine(MyFileSystem.ModsPath, this.ModFolder);
      this.m_alternatePath = string.IsNullOrEmpty(this.ModFolder) ? "" : System.IO.Path.Combine(MyFileSystem.ContentPath, this.Path);
    }

    public static implicit operator MyContentPath(string path) => new MyContentPath(path);
  }
}
