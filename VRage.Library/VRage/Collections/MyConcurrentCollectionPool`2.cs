// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentCollectionPool`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Collections
{
  public class MyConcurrentCollectionPool<TCollection, TItem> : IConcurrentPool where TCollection : ICollection<TItem>, new()
  {
    private readonly Stack<TCollection> m_instances;

    public MyConcurrentCollectionPool(int defaultCapacity = 0)
    {
      this.m_instances = new Stack<TCollection>(defaultCapacity);
      if (defaultCapacity <= 0)
        return;
      for (int index = 0; index < defaultCapacity; ++index)
        this.m_instances.Push(new TCollection());
    }

    public int Count
    {
      get
      {
        lock (this.m_instances)
          return this.m_instances.Count;
      }
    }

    public TCollection Get()
    {
      lock (this.m_instances)
      {
        if (this.m_instances.Count > 0)
          return this.m_instances.Pop();
      }
      return new TCollection();
    }

    public void Return(TCollection instance)
    {
      instance.Clear();
      lock (this.m_instances)
        this.m_instances.Push(instance);
    }

    void IConcurrentPool.Return(object obj) => this.Return((TCollection) obj);

    object IConcurrentPool.Get() => (object) this.Get();
  }
}
