// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyFileProviderAggregator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.IO;
using VRage.Collections;

namespace VRage.FileSystem
{
  public class MyFileProviderAggregator : IFileProvider
  {
    private HashSet<IFileProvider> m_providers = new HashSet<IFileProvider>();

    public MyFileProviderAggregator(params IFileProvider[] providers)
    {
      foreach (IFileProvider provider in providers)
        this.AddProvider(provider);
    }

    public void AddProvider(IFileProvider provider) => this.m_providers.Add(provider);

    public void RemoveProvider(IFileProvider provider) => this.m_providers.Remove(provider);

    public HashSetReader<IFileProvider> Providers => new HashSetReader<IFileProvider>(this.m_providers);

    public Stream OpenRead(string path) => this.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

    public Stream OpenWrite(string path, FileMode mode = FileMode.OpenOrCreate) => this.Open(path, mode, FileAccess.Write, FileShare.Read);

    public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
      foreach (IFileProvider provider in this.m_providers)
      {
        try
        {
          Stream stream = provider.Open(path, mode, access, share);
          if (stream != null)
            return stream;
        }
        catch
        {
        }
      }
      return (Stream) null;
    }

    public bool DirectoryExists(string path)
    {
      foreach (IFileProvider provider in this.m_providers)
      {
        try
        {
          if (provider.DirectoryExists(path))
            return true;
        }
        catch
        {
        }
      }
      return false;
    }

    public IEnumerable<string> GetFiles(
      string path,
      string filter,
      MySearchOption searchOption)
    {
      foreach (IFileProvider provider in this.m_providers)
      {
        try
        {
          IEnumerable<string> files = provider.GetFiles(path, filter, searchOption);
          if (files != null)
            return files;
        }
        catch
        {
        }
      }
      return (IEnumerable<string>) null;
    }

    public bool FileExists(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return false;
      foreach (IFileProvider provider in this.m_providers)
      {
        try
        {
          if (provider.FileExists(path))
            return true;
        }
        catch
        {
        }
      }
      return false;
    }
  }
}
