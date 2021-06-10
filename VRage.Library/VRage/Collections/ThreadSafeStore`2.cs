// Decompiled with JetBrains decompiler
// Type: VRage.Collections.ThreadSafeStore`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Collections
{
  public class ThreadSafeStore<TKey, TValue>
  {
    private readonly object m_lock = new object();
    private Dictionary<TKey, TValue> m_store;
    private readonly Func<TKey, TValue> m_creator;

    public ThreadSafeStore(Func<TKey, TValue> creator)
    {
      this.m_creator = creator != null ? creator : throw new ArgumentNullException(nameof (creator));
      this.m_store = new Dictionary<TKey, TValue>();
    }

    public TValue Get(TKey key)
    {
      TValue obj;
      return !this.m_store.TryGetValue(key, out obj) ? this.AddValue(key) : obj;
    }

    public TValue Get(TKey key, Func<TKey, TValue> creator)
    {
      TValue obj;
      return !this.m_store.TryGetValue(key, out obj) ? this.AddValue(key, creator) : obj;
    }

    private TValue AddValue(TKey key, Func<TKey, TValue> creator = null)
    {
      TValue obj1 = (creator ?? this.m_creator)(key);
      lock (this.m_lock)
      {
        if (this.m_store == null)
        {
          this.m_store = new Dictionary<TKey, TValue>();
          this.m_store[key] = obj1;
        }
        else
        {
          TValue obj2;
          if (this.m_store.TryGetValue(key, out obj2))
            return obj2;
          Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this.m_store);
          dictionary[key] = obj1;
          Thread.MemoryBarrier();
          this.m_store = dictionary;
        }
        return obj1;
      }
    }
  }
}
