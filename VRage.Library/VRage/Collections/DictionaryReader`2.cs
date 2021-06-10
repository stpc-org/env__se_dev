// Decompiled with JetBrains decompiler
// Type: VRage.Collections.DictionaryReader`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct DictionaryReader<K, V> : IEnumerable<KeyValuePair<K, V>>, IEnumerable
  {
    private readonly Dictionary<K, V> m_collection;
    public static readonly DictionaryReader<K, V> Empty;

    public DictionaryReader(Dictionary<K, V> collection) => this.m_collection = collection;

    public bool IsValid => this.m_collection != null;

    public bool ContainsKey(K key) => this.m_collection.ContainsKey(key);

    public bool TryGetValue(K key, out V value) => this.m_collection.TryGetValue(key, out value);

    public int Count => this.m_collection.Count;

    public V this[K key] => this.m_collection[key];

    public IEnumerable<K> Keys => (IEnumerable<K>) this.m_collection.Keys;

    public IEnumerable<V> Values => (IEnumerable<V>) this.m_collection.Values;

    public Dictionary<K, V>.Enumerator GetEnumerator() => this.m_collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() => (IEnumerator<KeyValuePair<K, V>>) this.GetEnumerator();

    public static implicit operator DictionaryReader<K, V>(Dictionary<K, V> v) => new DictionaryReader<K, V>(v);
  }
}
