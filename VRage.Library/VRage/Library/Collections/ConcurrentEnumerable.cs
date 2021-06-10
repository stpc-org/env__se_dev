// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ConcurrentEnumerable
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public static class ConcurrentEnumerable
  {
    public static ConcurrentEnumerable<TLock, TItem, TEnumerable> Create<TLock, TItem, TEnumerable>(
      TLock @lock,
      IEnumerable<TItem> enumerator)
      where TLock : struct, IDisposable
      where TEnumerable : IEnumerable<TItem>
    {
      return new ConcurrentEnumerable<TLock, TItem, TEnumerable>(@lock, enumerator);
    }
  }
}
