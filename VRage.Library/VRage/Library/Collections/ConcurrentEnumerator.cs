// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ConcurrentEnumerator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public static class ConcurrentEnumerator
  {
    public static ConcurrentEnumerator<TLock, TItem, TEnumerator> Create<TLock, TItem, TEnumerator>(
      TLock @lock,
      TEnumerator enumerator)
      where TLock : struct, IDisposable
      where TEnumerator : IEnumerator<TItem>
    {
      return new ConcurrentEnumerator<TLock, TItem, TEnumerator>(@lock, enumerator);
    }
  }
}
