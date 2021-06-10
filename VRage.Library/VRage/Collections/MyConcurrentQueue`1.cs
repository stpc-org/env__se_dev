// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentQueue`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class MyConcurrentQueue<T> : IEnumerable<T>, IEnumerable
  {
    private MyQueue<T> m_queue;
    private SpinLockRef m_lock = new SpinLockRef();

    public MyConcurrentQueue(int capacity) => this.m_queue = new MyQueue<T>(capacity);

    public MyConcurrentQueue() => this.m_queue = new MyQueue<T>(8);

    public int Count
    {
      get
      {
        using (this.m_lock.Acquire())
          return this.m_queue.Count;
      }
    }

    public void Clear()
    {
      using (this.m_lock.Acquire())
        this.m_queue.Clear();
    }

    public void Remove(T instance)
    {
      using (this.m_lock.Acquire())
        this.m_queue.Remove(instance);
    }

    public void Enqueue(T instance)
    {
      using (this.m_lock.Acquire())
        this.m_queue.Enqueue(instance);
    }

    public T Dequeue()
    {
      using (this.m_lock.Acquire())
        return this.m_queue.Dequeue();
    }

    public bool TryDequeue(out T instance)
    {
      using (this.m_lock.Acquire())
      {
        if (this.m_queue.Count > 0)
        {
          instance = this.m_queue.Dequeue();
          return true;
        }
      }
      instance = default (T);
      return false;
    }

    public bool TryPeek(out T instance)
    {
      using (this.m_lock.Acquire())
      {
        if (this.m_queue.Count > 0)
        {
          instance = this.m_queue.Peek();
          return true;
        }
      }
      instance = default (T);
      return false;
    }

    public ConcurrentEnumerator<SpinLockRef.Token, T, MyQueue<T>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<SpinLockRef.Token, T, MyQueue<T>.Enumerator>(this.m_lock.Acquire(), this.m_queue.GetEnumerator());

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
