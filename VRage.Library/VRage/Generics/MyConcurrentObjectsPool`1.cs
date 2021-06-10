// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyConcurrentObjectsPool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Generics
{
  public class MyConcurrentObjectsPool<T> where T : class, new()
  {
    private readonly FastResourceLock m_lock = new FastResourceLock();
    private readonly MyQueue<T> m_unused;
    private readonly HashSet<T> m_active;
    private readonly HashSet<T> m_marked;
    private readonly int m_baseCapacity;

    public void ApplyActionOnAllActives(Action<T> action)
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        foreach (T obj in this.m_active)
          action(obj);
      }
    }

    public int ActiveCount
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_active.Count;
      }
    }

    public int BaseCapacity
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
        {
          this.m_lock.AcquireShared();
          return this.m_baseCapacity;
        }
      }
    }

    public int Capacity
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
        {
          this.m_lock.AcquireShared();
          return this.m_unused.Count + this.m_active.Count;
        }
      }
    }

    private MyConcurrentObjectsPool()
    {
    }

    public MyConcurrentObjectsPool(int baseCapacity)
    {
      this.m_baseCapacity = baseCapacity;
      this.m_unused = new MyQueue<T>(this.m_baseCapacity);
      this.m_active = new HashSet<T>();
      this.m_marked = new HashSet<T>();
      for (int index = 0; index < this.m_baseCapacity; ++index)
        this.m_unused.Enqueue(new T());
    }

    public bool AllocateOrCreate(out T item)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        bool flag = this.m_unused.Count == 0;
        item = !flag ? this.m_unused.Dequeue() : new T();
        this.m_active.Add(item);
        return flag;
      }
    }

    public T Allocate(bool nullAllowed = false)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        T obj = default (T);
        if (this.m_unused.Count > 0)
        {
          obj = this.m_unused.Dequeue();
          this.m_active.Add(obj);
        }
        return obj;
      }
    }

    public void Deallocate(T item)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        this.m_active.Remove(item);
        this.m_unused.Enqueue(item);
      }
    }

    public void MarkForDeallocate(T item)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_marked.Add(item);
    }

    public void DeallocateAllMarked()
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        foreach (T obj in this.m_marked)
        {
          this.m_active.Remove(obj);
          this.m_unused.Enqueue(obj);
        }
        this.m_marked.Clear();
      }
    }

    public void DeallocateAll()
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        foreach (T obj in this.m_active)
          this.m_unused.Enqueue(obj);
        this.m_active.Clear();
        this.m_marked.Clear();
      }
    }
  }
}
