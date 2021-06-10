// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyRuntimeObjectsPool`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;

namespace VRage.Generics
{
  public class MyRuntimeObjectsPool<TPool> where TPool : class
  {
    private readonly Queue<TPool> m_unused;
    private readonly Func<TPool> m_constructor;
    private readonly HashSet<TPool> m_active;
    private readonly HashSet<TPool> m_marked;
    private readonly int m_baseCapacity;

    public QueueReader<TPool> Unused => new QueueReader<TPool>(this.m_unused);

    public HashSetReader<TPool> Active => new HashSetReader<TPool>(this.m_active);

    public int ActiveCount => this.m_active.Count;

    public int BaseCapacity => this.m_baseCapacity;

    public int Capacity => this.m_unused.Count + this.m_active.Count;

    public MyRuntimeObjectsPool(int baseCapacity, Type type)
      : this(baseCapacity, ExpressionExtension.CreateActivator<TPool>(type))
    {
    }

    public MyRuntimeObjectsPool(int baseCapacity, Func<TPool> constructor)
    {
      this.m_constructor = constructor;
      this.m_baseCapacity = baseCapacity;
      this.m_unused = new Queue<TPool>(this.m_baseCapacity);
      this.m_active = new HashSet<TPool>();
      this.m_marked = new HashSet<TPool>();
      for (int index = 0; index < this.m_baseCapacity; ++index)
        this.m_unused.Enqueue(this.m_constructor());
    }

    public bool AllocateOrCreate(out TPool item)
    {
      int num = this.m_unused.Count == 0 ? 1 : 0;
      item = num == 0 ? this.m_unused.Dequeue() : this.m_constructor();
      this.m_active.Add(item);
      return num != 0;
    }

    public TPool Allocate(bool nullAllowed = false)
    {
      TPool pool = default (TPool);
      if (this.m_unused.Count > 0)
      {
        pool = this.m_unused.Dequeue();
        this.m_active.Add(pool);
      }
      return pool;
    }

    public void Deallocate(TPool item)
    {
      this.m_active.Remove(item);
      this.m_unused.Enqueue(item);
    }

    public void MarkForDeallocate(TPool item) => this.m_marked.Add(item);

    public void MarkAllActiveForDeallocate() => this.m_marked.UnionWith((IEnumerable<TPool>) this.m_active);

    public void DeallocateAllMarked()
    {
      foreach (TPool pool in this.m_marked)
        this.Deallocate(pool);
      this.m_marked.Clear();
    }

    public void DeallocateAll()
    {
      foreach (TPool pool in this.m_active)
        this.m_unused.Enqueue(pool);
      this.m_active.Clear();
      this.m_marked.Clear();
    }

    public void TrimToBaseCapacity()
    {
      while (this.Capacity > this.BaseCapacity && this.m_unused.Count > 0)
        this.m_unused.Dequeue();
      this.m_unused.TrimExcess();
      this.m_active.TrimExcess();
      this.m_marked.TrimExcess();
    }
  }
}
