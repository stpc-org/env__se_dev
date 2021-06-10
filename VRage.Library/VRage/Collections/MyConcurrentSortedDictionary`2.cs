// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentSortedDictionary`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Collections
{
  public class MyConcurrentSortedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
  {
    private SortedDictionary<TKey, TValue> m_dictionary;
    private FastResourceLock m_lock = new FastResourceLock();

    public int Count
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_dictionary.Count;
      }
    }

    public TValue this[TKey key]
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_dictionary[key];
      }
      set
      {
        using (this.m_lock.AcquireExclusiveUsing())
          this.m_dictionary[key] = value;
      }
    }

    public MyConcurrentSortedDictionary(IComparer<TKey> comparer = null) => this.m_dictionary = new SortedDictionary<TKey, TValue>(comparer);

    public TValue ChangeKey(TKey oldKey, TKey newKey)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        TValue obj = this.m_dictionary[oldKey];
        this.m_dictionary.Remove(oldKey);
        this.m_dictionary[newKey] = obj;
        return obj;
      }
    }

    public void Clear()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_dictionary.Clear();
    }

    public void Add(TKey key, TValue value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_dictionary.Add(key, value);
    }

    public bool TryAdd(TKey key, TValue value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_dictionary.ContainsKey(key))
          return false;
        this.m_dictionary.Add(key, value);
        return true;
      }
    }

    public bool ContainsKey(TKey key)
    {
      using (this.m_lock.AcquireSharedUsing())
        return this.m_dictionary.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
      using (this.m_lock.AcquireSharedUsing())
        return this.m_dictionary.ContainsValue(value);
    }

    public bool Remove(TKey key)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        return this.m_dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      using (this.m_lock.AcquireSharedUsing())
        return this.m_dictionary.TryGetValue(key, out value);
    }

    public void GetValues(List<TValue> result)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        foreach (TValue obj in this.m_dictionary.Values)
          result.Add(obj);
      }
    }

    public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator>(this.m_lock.AcquireSharedUsing(), this.m_dictionary.GetEnumerator());

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TValue>>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TKey, SortedDictionary<TKey, TValue>.KeyCollection> Keys => ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TKey, SortedDictionary<TKey, TValue>.KeyCollection>(this.m_lock.AcquireSharedUsing(), (IEnumerable<TKey>) this.m_dictionary.Keys);

    public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TValue, SortedDictionary<TKey, TValue>.ValueCollection> Values => ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TValue, SortedDictionary<TKey, TValue>.ValueCollection>(this.m_lock.AcquireSharedUsing(), (IEnumerable<TValue>) this.m_dictionary.Values);
  }
}
