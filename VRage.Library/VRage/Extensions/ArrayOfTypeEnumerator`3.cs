// Decompiled with JetBrains decompiler
// Type: VRage.Extensions.ArrayOfTypeEnumerator`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
  public struct ArrayOfTypeEnumerator<T, TInner, TOfType> : IEnumerator<TOfType>, IEnumerator, IDisposable
    where TInner : struct, IEnumerator<T>
    where TOfType : T
  {
    private TInner m_inner;

    public ArrayOfTypeEnumerator(TInner enumerator) => this.m_inner = enumerator;

    public ArrayOfTypeEnumerator<T, TInner, TOfType> GetEnumerator() => this;

    public TOfType Current => (TOfType) (object) this.m_inner.Current;

    public void Dispose() => this.m_inner.Dispose();

    object IEnumerator.Current => (object) this.m_inner.Current;

    public bool MoveNext()
    {
      while (this.m_inner.MoveNext())
      {
        if ((object) this.m_inner.Current is TOfType)
          return true;
      }
      return false;
    }

    public void Reset() => this.m_inner.Reset();
  }
}
