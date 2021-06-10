// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyCachingDynamicObjectsPool`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Generics
{
  public class MyCachingDynamicObjectsPool<ObjectKey, ObjectType> where ObjectType : IDisposable, new()
  {
    private static readonly int DEFAULT_POOL_SIZE = 64;
    private static readonly int DEFAULT_CACHE_SIZE = 8;
    private static readonly int DEFAULT_POOL_GROWTH = 1;
    private int m_cacheSize;
    private int m_poolGrowth;
    private Dictionary<ObjectKey, ObjectType> m_cache;
    private MyQueue<ObjectKey> m_entryAge;
    private Stack<ObjectType> m_objectPool;

    public MyCachingDynamicObjectsPool()
      : this(MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_POOL_SIZE, MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_CACHE_SIZE, MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_POOL_GROWTH)
    {
    }

    public MyCachingDynamicObjectsPool(int poolSize)
      : this(poolSize, MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_CACHE_SIZE, MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_POOL_GROWTH)
    {
    }

    public MyCachingDynamicObjectsPool(int poolSize, int cacheSize)
      : this(poolSize, cacheSize, MyCachingDynamicObjectsPool<ObjectKey, ObjectType>.DEFAULT_POOL_GROWTH)
    {
    }

    public MyCachingDynamicObjectsPool(int poolSize, int cacheSize, int poolGrowth)
    {
      this.m_cacheSize = cacheSize;
      this.m_poolGrowth = poolGrowth;
      this.m_cache = new Dictionary<ObjectKey, ObjectType>(this.m_cacheSize);
      this.m_objectPool = new Stack<ObjectType>(poolSize);
      this.m_entryAge = new MyQueue<ObjectKey>(this.m_cacheSize);
      this.Restock(poolSize);
    }

    public ObjectType Allocate()
    {
      if (this.m_objectPool.Count > 0)
        return this.m_objectPool.Pop();
      if (this.m_entryAge.Count > 0)
      {
        ObjectKey key = this.m_entryAge.Dequeue();
        ObjectType objectType = this.m_cache[key];
        this.m_cache.Remove(key);
        objectType.Dispose();
        return objectType;
      }
      this.Restock(this.m_poolGrowth);
      return this.m_objectPool.Pop();
    }

    public void Deallocate(ObjectType obj)
    {
      obj.Dispose();
      this.m_objectPool.Push(obj);
    }

    public void Deallocate(ObjectKey key, ObjectType obj)
    {
      if (this.m_entryAge.Count == this.m_cacheSize)
      {
        ObjectKey key1 = this.m_entryAge.Dequeue();
        ObjectType objectType = this.m_cache[key1];
        this.m_cache.Remove(key1);
        this.Deallocate(objectType);
      }
      this.m_entryAge.Enqueue(key);
      this.m_cache.Add(key, obj);
    }

    public bool TryAllocateCached(ObjectKey key, out ObjectType obj)
    {
      if (!this.m_cache.TryGetValue(key, out obj))
      {
        obj = this.Allocate();
        return false;
      }
      this.m_entryAge.Remove(key);
      obj = this.m_cache[key];
      this.m_cache.Remove(key);
      return true;
    }

    private void Restock(int amount)
    {
      for (int index = 0; index < amount; ++index)
        this.m_objectPool.Push(new ObjectType());
    }
  }
}
