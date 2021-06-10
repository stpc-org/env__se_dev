// Decompiled with JetBrains decompiler
// Type: VRage.Collections.ListReader`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct ListReader<T> : IEnumerable<T>, IEnumerable
  {
    public static ListReader<T> Empty = new ListReader<T>(new List<T>(0));
    private readonly List<T> m_list;

    public ListReader(List<T> list) => this.m_list = list ?? ListReader<T>.Empty.m_list;

    public static implicit operator ListReader<T>(List<T> list) => new ListReader<T>(list);

    public int Count => this.m_list.Count;

    public T this[int index] => this.m_list[index];

    public T ItemAt(int index) => this.m_list[index];

    public int IndexOf(T item) => this.m_list.IndexOf(item);

    public List<T>.Enumerator GetEnumerator() => this.m_list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();
  }
}
