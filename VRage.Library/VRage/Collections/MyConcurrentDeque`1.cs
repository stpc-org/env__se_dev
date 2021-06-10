// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentDeque`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;
using VRage.Library.Collections;

namespace VRage.Collections
{
  [DebuggerDisplay("Count = {Count}")]
  public class MyConcurrentDeque<T> : IMyQueue<T>
  {
    private readonly MyDeque<T> m_deque = new MyDeque<T>();
    private readonly FastResourceLock m_lock = new FastResourceLock();

    public bool Empty
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_deque.Empty;
      }
    }

    public int Count
    {
      get
      {
        using (this.m_lock.AcquireSharedUsing())
          return this.m_deque.Count;
      }
    }

    public void Clear()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_deque.Clear();
    }

    public void EnqueueFront(T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_deque.EnqueueFront(value);
    }

    public void EnqueueBack(T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_deque.EnqueueBack(value);
    }

    public bool TryDequeueFront(out T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_deque.Empty)
        {
          value = default (T);
          return false;
        }
        value = this.m_deque.DequeueFront();
        return true;
      }
    }

    public bool TryDequeueBack(out T value)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_deque.Empty)
        {
          value = default (T);
          return false;
        }
        value = this.m_deque.DequeueBack();
        return true;
      }
    }
  }
}
