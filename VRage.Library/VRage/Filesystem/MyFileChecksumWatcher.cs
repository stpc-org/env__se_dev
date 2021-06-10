// Decompiled with JetBrains decompiler
// Type: VRage.Filesystem.MyFileChecksumWatcher
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage.FileSystem;

namespace VRage.Filesystem
{
  internal class MyFileChecksumWatcher : IDisposable
  {
    public bool ChecksumFound { get; private set; }

    public bool ChecksumValid { get; private set; }

    public MyFileChecksumWatcher()
    {
      this.ChecksumFound = true;
      this.ChecksumValid = true;
      MyFileSystem.FileVerifier.ChecksumFailed += new Action<string, string>(this.FileVerifier_ChecksumFailed);
      MyFileSystem.FileVerifier.ChecksumNotFound += new Action<IFileVerifier, string>(this.FileVerifier_ChecksumNotFound);
    }

    public void Reset()
    {
      this.ChecksumValid = true;
      this.ChecksumFound = true;
    }

    private void FileVerifier_ChecksumNotFound(IFileVerifier arg1, string arg2)
    {
      this.ChecksumFound = false;
      this.ChecksumValid = false;
    }

    private void FileVerifier_ChecksumFailed(string arg1, string arg2)
    {
      this.ChecksumFound = true;
      this.ChecksumValid = false;
    }

    void IDisposable.Dispose()
    {
      MyFileSystem.FileVerifier.ChecksumFailed -= new Action<string, string>(this.FileVerifier_ChecksumFailed);
      MyFileSystem.FileVerifier.ChecksumNotFound -= new Action<IFileVerifier, string>(this.FileVerifier_ChecksumNotFound);
    }
  }
}
