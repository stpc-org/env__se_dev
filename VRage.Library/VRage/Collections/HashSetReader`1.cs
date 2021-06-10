// Decompiled with JetBrains decompiler
// Type: VRage.Collections.HashSetReader`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct HashSetReader<T> : IEnumerable<T>, IEnumerable
  {
    private readonly HashSet<T> m_hashset;

    public HashSetReader(HashSet<T> set) => this.m_hashset = set;

    public static implicit operator HashSetReader<T>(HashSet<T> v) => new HashSetReader<T>(v);

    public bool IsValid => this.m_hashset != null;

    public int Count => this.m_hashset.Count;

    public bool Contains(T item) => this.m_hashset.Contains(item);

    public T First()
    {
      using (HashSet<T>.Enumerator enumerator = this.GetEnumerator())
        return enumerator.MoveNext() ? enumerator.Current : throw new InvalidOperationException("No elements in collection!");
    }

    public T[] ToArray()
    {
      T[] array = new T[this.m_hashset.Count];
      this.m_hashset.CopyTo(array);
      return array;
    }

    public HashSet<T>.Enumerator GetEnumerator() => this.m_hashset.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();
  }
}
