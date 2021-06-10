// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyChecksumVerifier
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;
using VRage.Common.Utils;

namespace VRage.FileSystem
{
  public class MyChecksumVerifier : IFileVerifier
  {
    public readonly string BaseChecksumDir;
    public readonly byte[] PublicKey;
    private Dictionary<string, string> m_checksums;

    public event Action<IFileVerifier, string> ChecksumNotFound;

    public event Action<string, string> ChecksumFailed;

    public MyChecksumVerifier(MyChecksums checksums, string baseChecksumDir)
    {
      this.PublicKey = checksums.PublicKeyAsArray;
      this.BaseChecksumDir = baseChecksumDir;
      this.m_checksums = checksums.Items.Dictionary;
    }

    public Stream Verify(string filename, Stream stream)
    {
      Action<string, string> checksumFailed = this.ChecksumFailed;
      Action<IFileVerifier, string> checksumNotFound = this.ChecksumNotFound;
      if ((checksumFailed != null || checksumNotFound != null) && filename.StartsWith(this.BaseChecksumDir, StringComparison.InvariantCultureIgnoreCase))
      {
        string s;
        if (this.m_checksums.TryGetValue(filename.Substring(this.BaseChecksumDir.Length + 1), out s))
        {
          if (checksumFailed != null)
            return (Stream) new MyCheckSumStream(stream, filename, Convert.FromBase64String(s), this.PublicKey, checksumFailed);
        }
        else if (checksumNotFound != null)
          checksumNotFound((IFileVerifier) this, filename);
      }
      return stream;
    }
  }
}
