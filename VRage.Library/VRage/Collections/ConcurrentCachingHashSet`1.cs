// Decompiled with JetBrains decompiler
// Type: VRage.Collections.ConcurrentCachingHashSet`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class ConcurrentCachingHashSet<T> : IEnumerable<T>, IEnumerable
  {
    private readonly HashSet<T> m_hashSet = new HashSet<T>();
    private readonly HashSet<T> m_toAdd = new HashSet<T>();
    private readonly HashSet<T> m_toRemove = new HashSet<T>();
    private readonly SpinLockRef m_setLock = new SpinLockRef();
    private readonly SpinLockRef m_changelistLock = new SpinLockRef();

    public int Count
    {
      get
      {
        using (this.m_setLock.Acquire())
          return this.m_hashSet.Count;
      }
    }

    public void Clear(bool clearCache = true)
    {
      using (this.m_setLock.Acquire())
      {
        using (this.m_changelistLock.Acquire())
        {
          this.m_hashSet.Clear();
          if (!clearCache)
            return;
          this.m_toAdd.Clear();
          this.m_toRemove.Clear();
        }
      }
    }

    public bool Contains(T item)
    {
      using (this.m_setLock.Acquire())
        return this.m_hashSet.Contains(item);
    }

    public void Add(T item)
    {
      using (this.m_changelistLock.Acquire())
      {
        this.m_toRemove.Remove(item);
        this.m_toAdd.Add(item);
      }
    }

    public void Remove(T item, bool immediate = false)
    {
      if (immediate)
      {
        using (this.m_setLock.Acquire())
        {
          using (this.m_changelistLock.Acquire())
          {
            this.m_toAdd.Remove(item);
            this.m_toRemove.Remove(item);
            this.m_hashSet.Remove(item);
          }
        }
      }
      else
      {
        using (this.m_changelistLock.Acquire())
        {
          this.m_toAdd.Remove(item);
          this.m_toRemove.Add(item);
        }
      }
    }

    public void ApplyChanges()
    {
      this.ApplyAdditions();
      this.ApplyRemovals();
    }

    public void ApplyAdditions()
    {
      using (this.m_setLock.Acquire())
      {
        using (this.m_changelistLock.Acquire())
        {
          foreach (T obj in this.m_toAdd)
            this.m_hashSet.Add(obj);
          this.m_toAdd.Clear();
        }
      }
    }

    public void ApplyRemovals()
    {
      using (this.m_setLock.Acquire())
      {
        using (this.m_changelistLock.Acquire())
        {
          foreach (T obj in this.m_toRemove)
            this.m_hashSet.Remove(obj);
          this.m_toRemove.Clear();
        }
      }
    }

    public HashSet<T>.Enumerator GetEnumerator()
    {
      using (this.m_setLock.Acquire())
        return this.m_hashSet.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override string ToString() => string.Format("Count = {0}; ToAdd = {1}; ToRemove = {2}", (object) this.m_hashSet.Count, (object) this.m_toAdd.Count, (object) this.m_toRemove.Count);
  }
}
