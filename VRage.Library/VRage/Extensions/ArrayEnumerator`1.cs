// Decompiled with JetBrains decompiler
// Type: VRage.Extensions.ArrayEnumerator`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
  public struct ArrayEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
  {
    private T[] m_array;
    private int m_currentIndex;

    public ArrayEnumerator(T[] array)
    {
      this.m_array = array;
      this.m_currentIndex = -1;
    }

    public T Current => this.m_array[this.m_currentIndex];

    public void Dispose()
    {
    }

    object IEnumerator.Current => (object) this.Current;

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_array.Length;
    }

    public void Reset() => this.m_currentIndex = -1;
  }
}
