// Decompiled with JetBrains decompiler
// Type: Deque`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

internal class Deque<T>
{
  private const int INITIAL_SIZE = 32;
  private T[] m_array = new T[32];
  private int m_mask = 31;
  private volatile int m_headIndex;
  private volatile int m_tailIndex;
  private object m_foreignLock = new object();

  public bool IsEmpty => this.m_headIndex >= this.m_tailIndex;

  public int Count => this.m_tailIndex - this.m_headIndex;

  public void LocalPush(T obj)
  {
    lock (this.m_foreignLock)
    {
      int num1 = this.m_tailIndex;
      if (num1 < this.m_headIndex + this.m_mask)
      {
        this.m_array[num1 & this.m_mask] = obj;
        this.m_tailIndex = num1 + 1;
      }
      else
      {
        int headIndex = this.m_headIndex;
        int num2 = this.m_tailIndex - this.m_headIndex;
        if (num2 >= this.m_mask)
        {
          T[] objArray = new T[this.m_array.Length << 1];
          for (int index = 0; index < num2; ++index)
            objArray[index] = this.m_array[index + headIndex & this.m_mask];
          this.m_array = objArray;
          this.m_headIndex = 0;
          this.m_tailIndex = num1 = num2;
          this.m_mask = this.m_mask << 1 | 1;
        }
        this.m_array[num1 & this.m_mask] = obj;
        this.m_tailIndex = num1 + 1;
      }
    }
  }

  public bool LocalPop(ref T obj)
  {
    lock (this.m_foreignLock)
    {
      int tailIndex = this.m_tailIndex;
      if (this.m_headIndex >= tailIndex)
        return false;
      int num = tailIndex - 1;
      Interlocked.Exchange(ref this.m_tailIndex, num);
      if (this.m_headIndex <= num)
      {
        obj = this.m_array[num & this.m_mask];
        return true;
      }
      if (this.m_headIndex <= num)
      {
        obj = this.m_array[num & this.m_mask];
        return true;
      }
      this.m_tailIndex = num + 1;
      return false;
    }
  }

  public bool TrySteal(ref T obj)
  {
    bool flag = false;
    try
    {
      flag = Monitor.TryEnter(this.m_foreignLock);
      if (flag)
      {
        int headIndex = this.m_headIndex;
        Interlocked.Exchange(ref this.m_headIndex, headIndex + 1);
        if (headIndex < this.m_tailIndex)
        {
          obj = this.m_array[headIndex & this.m_mask];
          return true;
        }
        this.m_headIndex = headIndex;
        return false;
      }
    }
    finally
    {
      if (flag)
        Monitor.Exit(this.m_foreignLock);
    }
    return false;
  }

  public void Clear()
  {
    for (int index = 0; index < this.m_array.Length; ++index)
      this.m_array[index] = default (T);
    this.m_headIndex = 0;
    this.m_tailIndex = 0;
  }
}
