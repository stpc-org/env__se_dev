// Decompiled with JetBrains decompiler
// Type: VRage.Collections.ConcurrentCachingList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class ConcurrentCachingList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
  {
    private readonly List<T> m_list;
    private readonly List<T> m_toAdd = new List<T>();
    private readonly List<T> m_toRemove = new List<T>();
    private readonly SpinLockRef m_listLock = new SpinLockRef();
    private readonly SpinLockRef m_cacheLock = new SpinLockRef();
    private bool m_dirty;

    public ConcurrentCachingList()
      : this(0)
    {
    }

    public ConcurrentCachingList(int capacity) => this.m_list = new List<T>(capacity);

    public int Count => this.m_list.Count;

    public bool IsEmpty => this.m_list.Count == 0 && this.m_toAdd.Count == 0 && this.m_toRemove.Count == 0;

    public T this[int index]
    {
      get
      {
        using (this.m_listLock.Acquire())
          return this.m_list[index];
      }
    }

    public void Add(T entity)
    {
      using (this.m_cacheLock.Acquire())
      {
        if (this.m_toRemove.Contains(entity))
        {
          this.m_toRemove.Remove(entity);
        }
        else
        {
          this.m_toAdd.Add(entity);
          this.m_dirty = true;
        }
      }
    }

    public void Remove(T entity, bool immediate = false)
    {
      using (this.m_cacheLock.Acquire())
      {
        if (!this.m_toAdd.Remove(entity))
          this.m_toRemove.Add(entity);
      }
      if (immediate)
      {
        using (this.m_listLock.Acquire())
        {
          using (this.m_cacheLock.Acquire())
          {
            this.m_list.Remove(entity);
            this.m_toRemove.Remove(entity);
          }
        }
      }
      else
        this.m_dirty = true;
    }

    public void RemoveAtImmediately(int index)
    {
      using (this.m_listLock.Acquire())
      {
        if (index < 0 || index >= this.m_list.Count)
          return;
        this.m_list.RemoveAt(index);
      }
    }

    public void ClearList()
    {
      using (this.m_listLock.Acquire())
        this.m_list.Clear();
    }

    public void ClearImmediate()
    {
      using (this.m_listLock.Acquire())
      {
        using (this.m_cacheLock.Acquire())
        {
          this.m_toAdd.Clear();
          this.m_toRemove.Clear();
          this.m_list.Clear();
          this.m_dirty = false;
        }
      }
    }

    public void ApplyChanges()
    {
      if (!this.m_dirty)
        return;
      this.m_dirty = false;
      this.ApplyAdditions();
      this.ApplyRemovals();
    }

    public void ApplyAdditions()
    {
      using (this.m_listLock.Acquire())
      {
        using (this.m_cacheLock.Acquire())
        {
          this.m_list.AddRange((IEnumerable<T>) this.m_toAdd);
          this.m_toAdd.Clear();
        }
      }
    }

    public void ApplyRemovals()
    {
      using (this.m_listLock.Acquire())
      {
        using (this.m_cacheLock.Acquire())
        {
          foreach (T obj in this.m_toRemove)
            this.m_list.Remove(obj);
          this.m_toRemove.Clear();
        }
      }
    }

    public void Sort(IComparer<T> comparer)
    {
      using (this.m_listLock.Acquire())
        this.m_list.Sort(comparer);
    }

    public ConcurrentEnumerator<SpinLockRef.Token, T, List<T>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<SpinLockRef.Token, T, List<T>.Enumerator>(this.m_listLock.Acquire(), this.m_list.GetEnumerator());

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [Conditional("DEBUG")]
    public void DebugCheckEmpty()
    {
    }

    public override string ToString() => string.Format("Count = {0}; ToAdd = {1}; ToRemove = {2}", (object) this.m_list.Count, (object) this.m_toAdd.Count, (object) this.m_toRemove.Count);
  }
}
