// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyCollectionDictionary`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public abstract class MyCollectionDictionary<TKey, TCollection, TValue> : IEnumerable<KeyValuePair<TKey, TCollection>>, IEnumerable
    where TCollection : ICollection<TValue>, new()
  {
    private readonly Stack<TCollection> m_collectionCache = new Stack<TCollection>();
    private readonly Dictionary<TKey, TCollection> m_dictionary;

    private TCollection Get() => this.m_collectionCache.Count > 0 ? this.m_collectionCache.Pop() : this.CreateCollection();

    protected virtual TCollection CreateCollection() => new TCollection();

    private void Return(TCollection list)
    {
      list.Clear();
      this.m_collectionCache.Push(list);
    }

    public MyCollectionDictionary()
      : this((IEqualityComparer<TKey>) null)
    {
    }

    public MyCollectionDictionary(IEqualityComparer<TKey> keyComparer = null) => this.m_dictionary = new Dictionary<TKey, TCollection>(keyComparer);

    public bool TryGet(TKey key, out TCollection list) => this.m_dictionary.TryGetValue(key, out list);

    public TCollection GetOrDefault(TKey key) => this.m_dictionary.GetValueOrDefault<TKey, TCollection>(key);

    public TCollection this[TKey key] => this.m_dictionary[key];

    public TCollection GetOrAdd(TKey key)
    {
      TCollection collection;
      if (!this.m_dictionary.TryGetValue(key, out collection))
      {
        collection = this.Get();
        this.m_dictionary.Add(key, collection);
      }
      return collection;
    }

    public Dictionary<TKey, TCollection>.ValueCollection Values => this.m_dictionary.Values;

    public Dictionary<TKey, TCollection>.KeyCollection Keys => this.m_dictionary.Keys;

    public void Add(TKey key, TValue value) => this.GetOrAdd(key).Add(value);

    public void Add(TKey key, IEnumerable<TValue> values)
    {
      TCollection orAdd = this.GetOrAdd(key);
      foreach (TValue obj in values)
        orAdd.Add(obj);
    }

    public void Add(TKey key, TValue first, TValue second)
    {
      TCollection orAdd = this.GetOrAdd(key);
      orAdd.Add(first);
      orAdd.Add(second);
    }

    public void Add(TKey key, TValue first, TValue second, TValue third)
    {
      TCollection orAdd = this.GetOrAdd(key);
      orAdd.Add(first);
      orAdd.Add(second);
      orAdd.Add(third);
    }

    public void Add(TKey key, params TValue[] values)
    {
      TCollection orAdd = this.GetOrAdd(key);
      foreach (TValue obj in values)
        orAdd.Add(obj);
    }

    public bool Remove(TKey key)
    {
      TCollection list;
      if (!this.m_dictionary.TryGetValue(key, out list))
        return false;
      this.m_dictionary.Remove(key);
      this.Return(list);
      return true;
    }

    public bool Remove(TKey key, TValue value)
    {
      TCollection collection;
      return this.m_dictionary.TryGetValue(key, out collection) && collection.Remove(value);
    }

    public void Clear()
    {
      foreach (KeyValuePair<TKey, TCollection> keyValuePair in this.m_dictionary)
        this.Return(keyValuePair.Value);
      this.m_dictionary.Clear();
    }

    public int KeyCount => this.m_dictionary.Count;

    public IEnumerator<KeyValuePair<TKey, TCollection>> GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TCollection>>) this.m_dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
