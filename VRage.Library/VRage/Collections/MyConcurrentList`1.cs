// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Collections
{
  public class MyConcurrentList<T> : IMyQueue<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private readonly System.Collections.Generic.List<T> m_list;
    private readonly FastResourceLock m_lock = new FastResourceLock();

    public ListReader<T> ListUnsafe => new ListReader<T>(this.m_list);

    public System.Collections.Generic.List<T> List => this.m_list;

    public MyConcurrentList(int reserve) => this.m_list = new System.Collections.Generic.List<T>(reserve);

    public MyConcurrentList() => this.m_list = new System.Collections.Generic.List<T>();

    public void Add(T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.Add(value);
    }

    public void AddRange(IEnumerable<T> value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.AddRange(value);
    }

    public void Sort(IComparer<T> comparer)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.Sort(comparer);
    }

    public bool TryDequeueFront(out T value)
    {
      value = default (T);
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_list.Count == 0)
          return false;
        value = this.m_list[0];
        this.m_list.RemoveAt(0);
      }
      return true;
    }

    public bool TryDequeueBack(out T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_list.Count == 0)
        {
          value = default (T);
          return false;
        }
        int index = this.m_list.Count - 1;
        value = this.m_list[index];
        this.m_list.RemoveAt(index);
      }
      return true;
    }

    public T Pop()
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        T obj = this.m_list[this.m_list.Count - 1];
        this.m_list.RemoveAt(this.m_list.Count - 1);
        return obj;
      }
    }

    public int Count
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_list.Count;
      }
    }

    public bool Empty
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_list.Count == 0;
      }
    }

    public int IndexOf(T item)
    {
      using (this.m_lock.AcquireSharedUsing())
        return this.m_list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.RemoveAt(index);
    }

    public T this[int index]
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_list[index];
      }
      set
      {
        using (this.m_lock.AcquireExclusiveUsing())
          this.m_list[index] = value;
      }
    }

    public void Clear()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_list.Clear();
    }

    public bool Contains(T item)
    {
      using (this.m_lock.AcquireSharedUsing())
        return this.m_list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      using (this.m_lock.AcquireSharedUsing())
        this.m_list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        return this.m_list.Remove(item);
    }

    public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, T, System.Collections.Generic.List<T>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, T, System.Collections.Generic.List<T>.Enumerator>(this.m_lock.AcquireSharedUsing(), this.m_list.GetEnumerator());

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool IsReadOnly => false;

    public void RemoveAll(Predicate<T> callback)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        int index = 0;
        while (index < this.Count)
        {
          if (callback(this.m_list[index]))
            this.m_list.RemoveAt(index);
          else
            ++index;
        }
      }
    }
  }
}
