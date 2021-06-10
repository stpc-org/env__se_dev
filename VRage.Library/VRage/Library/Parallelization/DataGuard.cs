// Decompiled with JetBrains decompiler
// Type: VRage.Library.Parallelization.DataGuard
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.InteropServices;

namespace VRage.Library.Parallelization
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DataGuard
  {
    private static bool stop;

    public DataGuard.AccessToken Read(string reason = "") => this.Shared(reason);

    public DataGuard.AccessToken Shared(string reason = "") => new DataGuard.AccessToken();

    public DataGuard.AccessToken Write(string reason = "") => this.Exclusive(reason);

    public DataGuard.AccessToken Exclusive(string reason = "") => new DataGuard.AccessToken();

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct AccessToken : IDisposable
    {
      void IDisposable.Dispose()
      {
      }
    }
  }
}
