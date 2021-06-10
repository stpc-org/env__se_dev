// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MySwapQueue`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Collections
{
  public class MySwapQueue<T> where T : class
  {
    private T m_read;
    private T m_write;
    private T m_waitingData;
    private T m_unusedData;
    private object m_lock = new object();

    public T Read => this.m_read;

    public T Write => this.m_write;

    public MySwapQueue(Func<T> factoryMethod)
      : this(factoryMethod(), factoryMethod(), factoryMethod())
    {
    }

    public MySwapQueue(T first, T second, T third)
    {
      this.m_read = first;
      this.m_write = second;
      this.m_unusedData = third;
      this.m_waitingData = default (T);
    }

    public bool RefreshRead()
    {
      lock (this.m_lock)
      {
        if ((object) this.m_unusedData != null)
          return false;
        this.m_unusedData = this.m_read;
        this.m_read = this.m_waitingData;
        this.m_waitingData = default (T);
        return true;
      }
    }

    public void CommitWrite()
    {
      lock (this.m_lock)
      {
        if ((object) this.m_waitingData == null)
        {
          this.m_waitingData = this.m_write;
          this.m_write = this.m_unusedData;
          this.m_unusedData = default (T);
        }
        else
        {
          T write = this.m_write;
          this.m_write = this.m_waitingData;
          this.m_waitingData = write;
        }
      }
    }
  }
}
