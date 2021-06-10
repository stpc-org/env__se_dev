// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.ConcurrentEnumerator`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public struct ConcurrentEnumerator<TLock, TItem, TEnumerator> : IEnumerator<TItem>, IEnumerator, IDisposable
    where TLock : struct, IDisposable
    where TEnumerator : IEnumerator<TItem>
  {
    private TEnumerator m_enumerator;
    private TLock m_lock;

    public ConcurrentEnumerator(TLock @lock, TEnumerator enumerator)
    {
      this.m_enumerator = enumerator;
      this.m_lock = @lock;
    }

    public void Dispose()
    {
      this.m_enumerator.Dispose();
      this.m_lock.Dispose();
    }

    public bool MoveNext() => this.m_enumerator.MoveNext();

    public void Reset() => this.m_enumerator.Reset();

    public TItem Current => this.m_enumerator.Current;

    object IEnumerator.Current => (object) this.Current;
  }
}
