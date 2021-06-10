// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentPool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VRage.Collections
{
  public class MyConcurrentPool<T> : IConcurrentPool where T : new()
  {
    private readonly int m_expectedAllocations;
    private readonly Action<T> m_clear;
    private readonly Stack<T> m_instances;
    private readonly Func<T> m_activator;
    private readonly Action<T> m_deactivator;

    public int Allocated { get; set; }

    public MyConcurrentPool(
      int defaultCapacity = 0,
      Action<T> clear = null,
      int expectedAllocations = 10000,
      Func<T> activator = null,
      Action<T> deactivator = null)
    {
      this.m_clear = clear;
      this.m_expectedAllocations = expectedAllocations;
      this.m_instances = new Stack<T>(defaultCapacity);
      this.m_activator = activator ?? ExpressionExtension.CreateActivator<T>();
      this.m_deactivator = deactivator;
      if (defaultCapacity <= 0)
        return;
      this.Allocated = defaultCapacity;
      for (int index = 0; index < defaultCapacity; ++index)
        this.m_instances.Push(this.m_activator());
    }

    public int Count
    {
      get
      {
        lock (this.m_instances)
          return this.m_instances.Count;
      }
    }

    public T Get()
    {
      lock (this.m_instances)
      {
        if (this.m_instances.Count > 0)
          return this.m_instances.Pop();
      }
      return this.m_activator();
    }

    public void Return(T instance)
    {
      Action<T> clear = this.m_clear;
      if (clear != null)
        clear(instance);
      lock (this.m_instances)
        this.m_instances.Push(instance);
    }

    public void Clean()
    {
      lock (this.m_instances)
      {
        if (this.m_deactivator != null)
        {
          while (this.m_instances.Count > 0)
            this.m_deactivator(this.m_instances.Pop());
        }
        else
          this.m_instances.Clear();
      }
    }

    void IConcurrentPool.Return(object obj) => this.Return((T) obj);

    object IConcurrentPool.Get() => (object) this.Get();
  }
}
