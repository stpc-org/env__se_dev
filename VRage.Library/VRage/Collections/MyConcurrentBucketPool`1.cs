// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentBucketPool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Collections
{
  public class MyConcurrentBucketPool<T> : MyConcurrentBucketPool where T : class
  {
    private readonly IMyElementAllocator<T> m_allocator;
    private readonly ConcurrentDictionary<int, ConcurrentStack<T>> m_instances;
    private MyBufferStatistics m_statistics;

    public MyConcurrentBucketPool(string debugName, IMyElementAllocator<T> allocator)
      : base(allocator.ExplicitlyDisposeAllElements)
    {
      this.m_allocator = allocator;
      this.m_instances = new ConcurrentDictionary<int, ConcurrentStack<T>>();
      this.m_statistics.Name = debugName;
    }

    public T Get(int bucketId)
    {
      T result = default (T);
      ConcurrentStack<T> concurrentStack;
      if (this.m_instances.TryGetValue(bucketId, out concurrentStack))
        concurrentStack.TryPop(out result);
      if ((object) result == null)
      {
        result = this.m_allocator.Allocate(bucketId);
        Interlocked.Increment(ref this.m_statistics.TotalBuffersAllocated);
        Interlocked.Add(ref this.m_statistics.TotalBytesAllocated, this.m_allocator.GetBytes(result));
      }
      Interlocked.Add(ref this.m_statistics.ActiveBytes, this.m_allocator.GetBytes(result));
      Interlocked.Increment(ref this.m_statistics.ActiveBuffers);
      this.m_allocator.Init(result);
      return result;
    }

    public void Return(T instance)
    {
      int bytes = this.m_allocator.GetBytes(instance);
      int bucketId = this.m_allocator.GetBucketId(instance);
      Interlocked.Add(ref this.m_statistics.ActiveBytes, -bytes);
      Interlocked.Decrement(ref this.m_statistics.ActiveBuffers);
      if (MyConcurrentBucketPool.EnablePooling)
      {
        ConcurrentStack<T> orAdd;
        if (!this.m_instances.TryGetValue(bucketId, out orAdd))
        {
          ConcurrentStack<T> concurrentStack = new ConcurrentStack<T>();
          orAdd = this.m_instances.GetOrAdd(bucketId, concurrentStack);
        }
        orAdd.Push(instance);
      }
      else
        this.m_allocator.Dispose(instance);
    }

    public MyBufferStatistics GetReport() => this.m_statistics;

    public void Clear()
    {
      foreach (KeyValuePair<int, ConcurrentStack<T>> instance in this.m_instances)
      {
        T result;
        while (instance.Value.TryPop(out result))
          this.m_allocator.Dispose(result);
      }
      this.m_instances.Clear();
      this.m_statistics = new MyBufferStatistics()
      {
        Name = this.m_statistics.Name
      };
    }

    protected override void DisposeInternal() => this.Clear();
  }
}
