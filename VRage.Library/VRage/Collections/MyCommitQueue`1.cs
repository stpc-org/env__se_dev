// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyCommitQueue`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class MyCommitQueue<T>
  {
    private Queue<T> m_commited = new Queue<T>();
    private SpinLock m_commitLock;
    private List<T> m_dirty = new List<T>();
    private SpinLock m_dirtyLock;

    public int Count
    {
      get
      {
        this.m_commitLock.Enter();
        try
        {
          return this.m_commited.Count;
        }
        finally
        {
          this.m_commitLock.Exit();
        }
      }
    }

    public int UncommitedCount
    {
      get
      {
        this.m_dirtyLock.Enter();
        try
        {
          return this.m_dirty.Count;
        }
        finally
        {
          this.m_dirtyLock.Exit();
        }
      }
    }

    public void Enqueue(T obj)
    {
      this.m_dirtyLock.Enter();
      try
      {
        this.m_dirty.Add(obj);
      }
      finally
      {
        this.m_dirtyLock.Exit();
      }
    }

    public void Commit()
    {
      this.m_dirtyLock.Enter();
      try
      {
        this.m_commitLock.Enter();
        try
        {
          foreach (T obj in this.m_dirty)
            this.m_commited.Enqueue(obj);
        }
        finally
        {
          this.m_commitLock.Exit();
        }
        this.m_dirty.Clear();
      }
      finally
      {
        this.m_dirtyLock.Exit();
      }
    }

    public bool TryDequeue(out T obj)
    {
      this.m_commitLock.Enter();
      try
      {
        if (this.m_commited.Count > 0)
        {
          obj = this.m_commited.Dequeue();
          return true;
        }
      }
      finally
      {
        this.m_commitLock.Exit();
      }
      obj = default (T);
      return false;
    }
  }
}
