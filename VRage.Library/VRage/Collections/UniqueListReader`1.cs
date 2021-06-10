// Decompiled with JetBrains decompiler
// Type: VRage.Collections.UniqueListReader`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct UniqueListReader<T> : IEnumerable<T>, IEnumerable
  {
    public static UniqueListReader<T> Empty = new UniqueListReader<T>(new MyUniqueList<T>());
    private readonly MyUniqueList<T> m_list;

    public UniqueListReader(MyUniqueList<T> list) => this.m_list = list;

    public static implicit operator ListReader<T>(UniqueListReader<T> list) => list.m_list.ItemList;

    public static implicit operator UniqueListReader<T>(MyUniqueList<T> list) => new UniqueListReader<T>(list);

    public int Count => this.m_list.Count;

    public T ItemAt(int index) => this.m_list[index];

    public List<T>.Enumerator GetEnumerator() => this.m_list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();
  }
}
