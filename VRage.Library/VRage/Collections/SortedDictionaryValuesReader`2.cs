// Decompiled with JetBrains decompiler
// Type: VRage.Collections.SortedDictionaryValuesReader`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct SortedDictionaryValuesReader<K, V> : IEnumerable<V>, IEnumerable
  {
    private readonly SortedDictionary<K, V> m_collection;

    public SortedDictionaryValuesReader(SortedDictionary<K, V> collection) => this.m_collection = collection;

    public int Count => this.m_collection.Count;

    public V this[K key] => this.m_collection[key];

    public bool TryGetValue(K key, out V result) => this.m_collection.TryGetValue(key, out result);

    public SortedDictionary<K, V>.ValueCollection.Enumerator GetEnumerator() => this.m_collection.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<V> IEnumerable<V>.GetEnumerator() => (IEnumerator<V>) this.GetEnumerator();

    public static implicit operator SortedDictionaryValuesReader<K, V>(
      SortedDictionary<K, V> v)
    {
      return new SortedDictionaryValuesReader<K, V>(v);
    }
  }
}
