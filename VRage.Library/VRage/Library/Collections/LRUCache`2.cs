// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.LRUCache`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Library.Collections
{
  public class LRUCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
  {
    private static HashSet<int> m_debugEntrySet = new HashSet<int>();
    private int m_first;
    private int m_last;
    private readonly IEqualityComparer<TKey> m_comparer;
    private readonly Dictionary<TKey, int> m_index;
    private readonly LRUCache<TKey, TValue>.CacheEntry[] m_entries;
    private readonly FastResourceLock m_lock = new FastResourceLock();
    public Action<TKey, TValue> OnItemDiscarded;
    private const int Null = -1;

    public LRUCache(int cacheSize, IEqualityComparer<TKey> comparer = null)
    {
      this.m_comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      this.m_entries = new LRUCache<TKey, TValue>.CacheEntry[cacheSize];
      this.m_index = new Dictionary<TKey, int>(cacheSize, this.m_comparer);
      this.ResetInternal();
    }

    public float Usage => (float) this.m_index.Count / (float) this.m_entries.Length;

    public int Count => this.m_index.Count;

    public int Capacity => this.m_entries.Length;

    public void Reset()
    {
      if (this.m_index.Count <= 0)
        return;
      if (this.OnItemDiscarded != null)
      {
        for (int index = 0; index < this.m_entries.Length; ++index)
        {
          if ((object) this.m_entries[index].Data != null)
            this.OnItemDiscarded(this.m_entries[index].Key, this.m_entries[index].Data);
        }
      }
      this.ResetInternal();
    }

    private void ResetInternal()
    {
      LRUCache<TKey, TValue>.CacheEntry cacheEntry;
      cacheEntry.Data = default (TValue);
      cacheEntry.Key = default (TKey);
      for (int index = 0; index < this.m_entries.Length; ++index)
      {
        cacheEntry.Prev = index - 1;
        cacheEntry.Next = index + 1;
        this.m_entries[index] = cacheEntry;
      }
      this.m_first = 0;
      this.m_last = this.m_entries.Length - 1;
      this.m_entries[this.m_last].Next = -1;
      this.m_index.Clear();
    }

    public TValue Read(TKey key)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        try
        {
          int entryIndex;
          if (!this.m_index.TryGetValue(key, out entryIndex))
            return default (TValue);
          if (entryIndex != this.m_first)
          {
            this.Remove(entryIndex);
            this.AddFirst(entryIndex);
          }
          return this.m_entries[entryIndex].Data;
        }
        finally
        {
        }
      }
    }

    public bool TryRead(TKey key, out TValue value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        try
        {
          int entryIndex;
          if (this.m_index.TryGetValue(key, out entryIndex))
          {
            if (entryIndex != this.m_first)
            {
              this.Remove(entryIndex);
              this.AddFirst(entryIndex);
            }
            value = this.m_entries[entryIndex].Data;
            return true;
          }
          value = default (TValue);
          return false;
        }
        finally
        {
        }
      }
    }

    public bool TryPeek(TKey key, out TValue value)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        int index;
        if (this.m_index.TryGetValue(key, out index))
        {
          value = this.m_entries[index].Data;
          return true;
        }
        value = default (TValue);
        return false;
      }
    }

    public void Write(TKey key, TValue value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        int index;
        if (this.m_index.TryGetValue(key, out index))
        {
          this.m_entries[index].Data = value;
        }
        else
        {
          int last = this.m_last;
          this.RemoveLast();
          if ((object) this.m_entries[last].Key != null)
            this.m_index.Remove(this.m_entries[last].Key);
          this.m_entries[last].Key = key;
          this.m_entries[last].Data = value;
          this.AddFirst(last);
          this.m_index.Add(key, last);
        }
      }
    }

    public void Remove(TKey key)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        try
        {
          int entryIndex;
          if (!this.m_index.TryGetValue(key, out entryIndex))
            return;
          this.Remove(entryIndex);
          this.CleanEntry(entryIndex);
          this.ReinsertLast(entryIndex);
        }
        finally
        {
        }
      }
    }

    public int RemoveWhere(Func<TKey, TValue, bool> predicate)
    {
      int num1 = 0;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        int num2 = this.m_first;
        while (num2 != -1)
        {
          int entryIndex = num2;
          num2 = this.m_entries[entryIndex].Next;
          if (predicate(this.m_entries[entryIndex].Key, this.m_entries[entryIndex].Data))
          {
            this.Remove(entryIndex);
            this.CleanEntry(entryIndex);
            this.ReinsertLast(entryIndex);
            ++num1;
          }
        }
      }
      return num1;
    }

    private void RemoveLast()
    {
      int prev = this.m_entries[this.m_last].Prev;
      this.m_entries[prev].Next = -1;
      this.m_entries[this.m_last].Prev = -1;
      if (this.OnItemDiscarded != null && (object) this.m_entries[this.m_last].Data != null)
        this.OnItemDiscarded(this.m_entries[this.m_last].Key, this.m_entries[this.m_last].Data);
      this.m_last = prev;
    }

    private void Remove(int entryIndex)
    {
      int prev = this.m_entries[entryIndex].Prev;
      int next = this.m_entries[entryIndex].Next;
      if (prev != -1)
        this.m_entries[prev].Next = this.m_entries[entryIndex].Next;
      else
        this.m_first = this.m_entries[entryIndex].Next;
      if (next != -1)
        this.m_entries[next].Prev = this.m_entries[entryIndex].Prev;
      else
        this.m_last = this.m_entries[entryIndex].Prev;
      this.m_entries[entryIndex].Prev = -1;
      this.m_entries[entryIndex].Next = -1;
    }

    private void ReinsertLast(int entryIndex)
    {
      this.m_entries[this.m_last].Next = entryIndex;
      this.m_entries[entryIndex].Prev = this.m_last;
      this.m_entries[entryIndex].Next = -1;
      this.m_last = entryIndex;
    }

    private void CleanEntry(int entryIndex)
    {
      if (this.OnItemDiscarded != null && (object) this.m_entries[entryIndex].Data != null)
        this.OnItemDiscarded(this.m_entries[entryIndex].Key, this.m_entries[entryIndex].Data);
      this.m_index.Remove(this.m_entries[entryIndex].Key);
      this.m_entries[entryIndex].Key = default (TKey);
      this.m_entries[entryIndex].Data = default (TValue);
    }

    private void AddFirst(int entryIndex)
    {
      this.m_entries[this.m_first].Prev = entryIndex;
      this.m_entries[entryIndex].Next = this.m_first;
      this.m_first = entryIndex;
    }

    [Conditional("__UNUSED__")]
    private void AssertConsistent()
    {
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < this.m_entries.Length; ++index2)
          LRUCache<TKey, TValue>.m_debugEntrySet.Add(index2);
        switch (index1)
        {
          case 0:
            for (int index2 = this.m_first; index2 != -1; index2 = this.m_entries[index2].Next)
              LRUCache<TKey, TValue>.m_debugEntrySet.Remove(index2);
            break;
          case 1:
            for (int index2 = this.m_last; index2 != -1; index2 = this.m_entries[index2].Prev)
              LRUCache<TKey, TValue>.m_debugEntrySet.Remove(index2);
            break;
          case 2:
            foreach (KeyValuePair<TKey, int> keyValuePair in this.m_index)
              LRUCache<TKey, TValue>.m_debugEntrySet.Remove(keyValuePair.Value);
            LRUCache<TKey, TValue>.m_debugEntrySet.Clear();
            break;
        }
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      FastResourceLockExtensions.MySharedLock mySharedLock = this.m_lock.AcquireSharedUsing();
      try
      {
        foreach (int index in this.m_index.Values)
          yield return new KeyValuePair<TKey, TValue>(this.m_entries[index].Key, this.m_entries[index].Data);
      }
      finally
      {
        mySharedLock.Dispose();
      }
      mySharedLock = new FastResourceLockExtensions.MySharedLock();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [DebuggerDisplay("Prev={Prev}, Next={Next}, Key={Key}, Data={Data}")]
    private struct CacheEntry
    {
      public int Prev;
      public int Next;
      public TValue Data;
      public TKey Key;
    }
  }
}
