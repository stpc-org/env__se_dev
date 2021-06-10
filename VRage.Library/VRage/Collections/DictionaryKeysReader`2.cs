// Decompiled with JetBrains decompiler
// Type: VRage.Collections.DictionaryKeysReader`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct DictionaryKeysReader<K, V> : IEnumerable<K>, IEnumerable
  {
    private readonly Dictionary<K, V> m_collection;

    public int Count => this.m_collection.Count;

    public DictionaryKeysReader(Dictionary<K, V> collection) => this.m_collection = collection;

    public Dictionary<K, V>.KeyCollection.Enumerator GetEnumerator() => this.m_collection.Keys.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<K> IEnumerable<K>.GetEnumerator() => (IEnumerator<K>) this.GetEnumerator();
  }
}
