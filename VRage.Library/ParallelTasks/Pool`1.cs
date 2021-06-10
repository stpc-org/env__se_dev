// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Pool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Library;

namespace ParallelTasks
{
  public class Pool<T> : Singleton<Pool<T>> where T : class, new()
  {
    private readonly ConcurrentDictionary<Thread, MyConcurrentQueue<T>> m_instances;

    public Pool() => this.m_instances = new ConcurrentDictionary<Thread, MyConcurrentQueue<T>>(MyEnvironment.ProcessorCount, MyEnvironment.ProcessorCount);

    public T Get(Thread thread)
    {
      MyConcurrentQueue<T> myConcurrentQueue;
      if (!this.m_instances.TryGetValue(thread, out myConcurrentQueue))
      {
        myConcurrentQueue = new MyConcurrentQueue<T>();
        this.m_instances.TryAdd(thread, myConcurrentQueue);
      }
      T instance;
      if (!myConcurrentQueue.TryDequeue(out instance))
        instance = new T();
      return instance;
    }

    public void Return(Thread thread, T instance) => this.m_instances[thread].Enqueue(instance);

    public void Clean()
    {
      foreach (KeyValuePair<Thread, MyConcurrentQueue<T>> instance in this.m_instances)
        instance.Value.Clear();
    }
  }
}
