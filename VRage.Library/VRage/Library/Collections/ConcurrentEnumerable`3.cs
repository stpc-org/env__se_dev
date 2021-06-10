// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ConcurrentEnumerable`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public struct ConcurrentEnumerable<TLock, TItem, TEnumerable> : IEnumerable<TItem>, IEnumerable
    where TLock : struct, IDisposable
    where TEnumerable : IEnumerable<TItem>
  {
    private IEnumerable<TItem> m_enumerable;
    private TLock m_lock;

    public ConcurrentEnumerable(TLock lk, IEnumerable<TItem> enumerable)
    {
      this.m_enumerable = enumerable;
      this.m_lock = lk;
    }

    public ConcurrentEnumerator<TLock, TItem, IEnumerator<TItem>> GetEnumerator() => ConcurrentEnumerator.Create<TLock, TItem, IEnumerator<TItem>>(this.m_lock, this.m_enumerable.GetEnumerator());

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => (IEnumerator<TItem>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
