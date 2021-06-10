// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.PoolManager
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Library.Collections
{
  public static class PoolManager
  {
    private static readonly ConcurrentDictionary<Type, IConcurrentPool> Pools = new ConcurrentDictionary<Type, IConcurrentPool>();

    public static void Preallocate<TPooled>(int size) where TPooled : new()
    {
      Type type = typeof (TPooled);
      if (PoolManager.Pools.ContainsKey(type))
        return;
      PoolManager.Pools[type] = PoolManager.GetPool<TPooled>(type, size);
    }

    public static TPooled Get<TPooled>() where TPooled : new()
    {
      Type type = typeof (TPooled);
      IConcurrentPool pool;
      if (!PoolManager.Pools.TryGetValue(type, out pool))
        PoolManager.Pools[type] = pool = PoolManager.GetPool<TPooled>(type);
      return (TPooled) pool.Get();
    }

    public static PoolManager.ReturnHandle<TPooled> Get<TPooled>(out TPooled poolObject) where TPooled : new()
    {
      Type type = typeof (TPooled);
      IConcurrentPool pool;
      if (!PoolManager.Pools.TryGetValue(type, out pool))
        PoolManager.Pools[type] = pool = PoolManager.GetPool<TPooled>(type);
      poolObject = (TPooled) pool.Get();
      return new PoolManager.ReturnHandle<TPooled>(poolObject);
    }

    private static IConcurrentPool GetPool<TPooled>(Type type, int preallocated = 0) where TPooled : new()
    {
      Type type1 = typeof (ICollection<>);
      foreach (Type type2 in type.GetInterfaces())
      {
        if (type2.IsGenericType && type2.GetGenericTypeDefinition() == type1)
          return (IConcurrentPool) Activator.CreateInstance(typeof (MyConcurrentCollectionPool<,>).MakeGenericType(type, type2.GetGenericArguments()[0]), (object) preallocated);
      }
      return (IConcurrentPool) new MyConcurrentPool<TPooled>(preallocated);
    }

    public static void Return<TPooled>(ref TPooled obj) where TPooled : new()
    {
      Type key = typeof (TPooled);
      IConcurrentPool concurrentPool;
      if (PoolManager.Pools.TryGetValue(key, out concurrentPool))
        concurrentPool.Return((object) obj);
      obj = default (TPooled);
    }

    public static PoolManager.ArrayReturnHandle<TElement> BorrowArray<TElement>(
      int size,
      out TElement[] array)
    {
      array = ArrayPool<TElement>.Shared.Rent(size);
      return new PoolManager.ArrayReturnHandle<TElement>(array);
    }

    public static void ReturnBorrowedArray<TElement>(ref TElement[] array)
    {
      ArrayPool<TElement>.Shared.Return(array);
      array = (TElement[]) null;
    }

    public static PoolManager.ArrayReturnHandle<TElement> BorrowMemory<TElement>(
      int size,
      out PooledMemory<TElement> memory)
    {
      TElement[] elementArray = ArrayPool<TElement>.Shared.Rent(size);
      memory = new PooledMemory<TElement>(elementArray, size);
      return new PoolManager.ArrayReturnHandle<TElement>(elementArray);
    }

    public static void ReturnBorrowedMemory<TElement>(ref PooledMemory<TElement> memory)
    {
      ArrayPool<TElement>.Shared.Return(memory.Array);
      memory = new PooledMemory<TElement>();
    }

    public static PoolManager.ArrayReturnHandle<TElement> BorrowSpan<TElement>(
      int size,
      out Span<TElement> span)
    {
      TElement[] elementArray = ArrayPool<TElement>.Shared.Rent(size);
      span = new Span<TElement>(elementArray, 0, size);
      return new PoolManager.ArrayReturnHandle<TElement>(elementArray);
    }

    public struct ReturnHandle<TObject> : IDisposable where TObject : new()
    {
      private TObject m_obj;

      public ReturnHandle(TObject data)
        : this()
        => this.m_obj = data;

      public void Dispose() => PoolManager.Return<TObject>(ref this.m_obj);
    }

    public struct ArrayReturnHandle<TElement> : IDisposable
    {
      private TElement[] m_array;

      public ArrayReturnHandle(TElement[] data) => this.m_array = data;

      public void Dispose() => PoolManager.ReturnBorrowedArray<TElement>(ref this.m_array);
    }
  }
}
