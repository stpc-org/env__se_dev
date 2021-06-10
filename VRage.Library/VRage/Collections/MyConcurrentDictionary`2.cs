// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentDictionary`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Utils;

namespace VRage.Collections
{
  public class MyConcurrentDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
  {
    private Dictionary<TKey, TValue> m_dictionary;
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

    public MyConcurrentDictionary(IEqualityComparer<TKey> comparer) => this.m_dictionary = new Dictionary<TKey, TValue>(comparer);

    public MyConcurrentDictionary(int capacity = 0, IEqualityComparer<TKey> comparer = null) => this.m_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);

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

    public bool TryRemove(TKey key, out TValue value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_dictionary.TryGetValue(key, out value))
        {
          this.m_dictionary.Remove(key);
          return true;
        }
      }
      return false;
    }

    public bool TryRemoveConditionally<TCondition>(
      TKey key,
      out TValue value,
      TCondition condition)
      where TCondition : IMyCondition<TValue>
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_dictionary.TryGetValue(key, out value))
        {
          if (condition.Evaluate(value))
          {
            this.m_dictionary.Remove(key);
            return true;
          }
        }
      }
      return false;
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

    public TValue GetValueOrDefault(TKey key, TValue defaultValue)
    {
      TValue obj;
      return !this.TryGetValue(key, out obj) ? defaultValue : obj;
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        TValue obj;
        if (!this.m_dictionary.TryGetValue(key, out obj))
          this.m_dictionary[key] = obj = factory(key);
        return obj;
      }
    }

    public KeyValuePair<TKey, TValue> FirstPair()
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        Dictionary<TKey, TValue>.Enumerator enumerator = this.m_dictionary.GetEnumerator();
        enumerator.MoveNext();
        return enumerator.Current;
      }
    }

    [DebuggerHidden]
    public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator>(this.m_lock.AcquireSharedUsing(), this.m_dictionary.GetEnumerator());

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TValue>>) this.GetEnumerator();

    [DebuggerHidden]
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [DebuggerHidden]
    public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TKey, Dictionary<TKey, TValue>.KeyCollection> Keys => ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TKey, Dictionary<TKey, TValue>.KeyCollection>(this.m_lock.AcquireSharedUsing(), (IEnumerable<TKey>) this.m_dictionary.Keys);

    [DebuggerHidden]
    public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TValue, Dictionary<TKey, TValue>.ValueCollection> Values => ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TValue, Dictionary<TKey, TValue>.ValueCollection>(this.m_lock.AcquireSharedUsing(), (IEnumerable<TValue>) this.m_dictionary.Values);
  }
}
