// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyRangeIterator`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public struct MyRangeIterator<T> : IEnumerator<T>, IEnumerator, IDisposable
  {
    private IList<T> m_list;
    private int m_start;
    private int m_current;
    private int m_end;

    public static MyRangeIterator<T>.Enumerable ForRange(
      T[] array,
      int start,
      int end)
    {
      return new MyRangeIterator<T>.Enumerable(new MyRangeIterator<T>(array, start, end));
    }

    public static MyRangeIterator<T>.Enumerable ForRange(
      List<T> list,
      int start,
      int end)
    {
      return new MyRangeIterator<T>.Enumerable(new MyRangeIterator<T>((IList<T>) list, start, end));
    }

    public MyRangeIterator(T[] list, int start, int end)
    {
      this.m_list = (IList<T>) list;
      this.m_start = start;
      this.m_current = start - 1;
      this.m_end = end - 1;
    }

    public MyRangeIterator(IList<T> list, int start, int end)
    {
      this.m_list = list;
      this.m_start = start;
      this.m_current = start - 1;
      this.m_end = end - 1;
    }

    public void Dispose() => this.m_list = (IList<T>) null;

    public bool MoveNext()
    {
      if (this.m_current == this.m_end)
        return false;
      ++this.m_current;
      return true;
    }

    public void Reset() => this.m_current = this.m_start - 1;

    public T Current => this.m_list[this.m_current];

    object IEnumerator.Current => (object) this.Current;

    public struct Enumerable : IEnumerable<T>, IEnumerable
    {
      private MyRangeIterator<T> m_enumerator;

      public Enumerable(MyRangeIterator<T> enume) => this.m_enumerator = enume;

      public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.m_enumerator;

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
