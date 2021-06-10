// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyObjectsPool`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;
using VRage.Library.Threading;

namespace VRage.Generics
{
  public class MyObjectsPool<T> where T : class, new()
  {
    private MyConcurrentQueue<T> m_unused;
    private HashSet<T> m_active;
    private HashSet<T> m_marked;
    private SpinLockRef m_activeLock = new SpinLockRef();
    private Func<T> m_activator;
    private Action<T> m_clearFunction;
    private int m_baseCapacity;

    public SpinLockRef ActiveLock => this.m_activeLock;

    public HashSetReader<T> ActiveWithoutLock => new HashSetReader<T>(this.m_active);

    public HashSetReader<T> Active
    {
      get
      {
        using (this.m_activeLock.Acquire())
          return new HashSetReader<T>(this.m_active);
      }
    }

    public int ActiveCount
    {
      get
      {
        using (this.m_activeLock.Acquire())
          return this.m_active.Count;
      }
    }

    public int BaseCapacity => this.m_baseCapacity;

    public int Capacity
    {
      get
      {
        using (this.m_activeLock.Acquire())
          return this.m_unused.Count + this.m_active.Count;
      }
    }

    public MyObjectsPool(int baseCapacity, Func<T> activator = null, Action<T> clearFunction = null)
    {
      this.m_clearFunction = clearFunction;
      this.m_activator = activator ?? ExpressionExtension.CreateActivator<T>();
      this.m_baseCapacity = baseCapacity;
      this.m_unused = new MyConcurrentQueue<T>(this.m_baseCapacity);
      this.m_active = new HashSet<T>();
      this.m_marked = new HashSet<T>();
      for (int index = 0; index < this.m_baseCapacity; ++index)
        this.m_unused.Enqueue(this.m_activator());
    }

    public bool AllocateOrCreate(out T item)
    {
      bool flag = false;
      using (this.m_activeLock.Acquire())
      {
        flag = this.m_unused.Count == 0;
        item = !flag ? this.m_unused.Dequeue() : this.m_activator();
        this.m_active.Add(item);
      }
      return flag;
    }

    public T Allocate(bool nullAllowed = false)
    {
      T obj = default (T);
      using (this.m_activeLock.Acquire())
      {
        if (this.m_unused.Count > 0)
        {
          obj = this.m_unused.Dequeue();
          this.m_active.Add(obj);
        }
      }
      return obj;
    }

    public void Deallocate(T item)
    {
      using (this.m_activeLock.Acquire())
      {
        this.m_active.Remove(item);
        Action<T> clearFunction = this.m_clearFunction;
        if (clearFunction != null)
          clearFunction(item);
        this.m_unused.Enqueue(item);
      }
    }

    public void MarkForDeallocate(T item)
    {
      using (this.m_activeLock.Acquire())
        this.m_marked.Add(item);
    }

    public void MarkAllActiveForDeallocate()
    {
      using (this.m_activeLock.Acquire())
        this.m_marked.UnionWith((IEnumerable<T>) this.m_active);
    }

    public void DeallocateAllMarked()
    {
      using (this.m_activeLock.Acquire())
      {
        foreach (T instance in this.m_marked)
        {
          this.m_active.Remove(instance);
          Action<T> clearFunction = this.m_clearFunction;
          if (clearFunction != null)
            clearFunction(instance);
          this.m_unused.Enqueue(instance);
        }
        this.m_marked.Clear();
      }
    }

    public void DeallocateAll()
    {
      using (this.m_activeLock.Acquire())
      {
        foreach (T instance in this.m_active)
        {
          Action<T> clearFunction = this.m_clearFunction;
          if (clearFunction != null)
            clearFunction(instance);
          this.m_unused.Enqueue(instance);
        }
        this.m_active.Clear();
        this.m_marked.Clear();
      }
    }
  }
}
