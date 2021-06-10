// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyNullVerifier
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;

namespace VRage.FileSystem
{
  public class MyNullVerifier : IFileVerifier
  {
    public event Action<IFileVerifier, string> ChecksumNotFound
    {
      add
      {
      }
      remove
      {
      }
    }

    public event Action<string, string> ChecksumFailed
    {
      add
      {
      }
      remove
      {
      }
    }

    public Stream Verify(string filename, Stream stream) => stream;
  }
}
