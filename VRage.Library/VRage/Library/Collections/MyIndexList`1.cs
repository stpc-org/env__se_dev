// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyIndexList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyIndexList<T> : IEnumerable<T>, IEnumerable where T : class
  {
    private List<T> m_list;
    private Queue<int> m_freeList;
    private int m_version;

    public int Count => this.m_list.Count;

    public T this[int index] => this.m_list[index];

    public int NextIndex => this.m_freeList.Count <= 0 ? this.m_list.Count : this.m_freeList.Peek();

    public MyIndexList(int capacity = 0)
    {
      this.m_list = new List<T>(capacity);
      this.m_freeList = new Queue<int>(capacity);
    }

    public int Add(T item)
    {
      if ((object) item == null)
        throw new ArgumentException("Null cannot be stored in IndexList, it's used as 'empty' indicator");
      int result;
      if (this.m_freeList.TryDequeue<int>(out result))
      {
        this.m_list[result] = item;
        ++this.m_version;
        return result;
      }
      this.m_list.Add(item);
      ++this.m_version;
      return this.m_list.Count - 1;
    }

    public void Remove(int index)
    {
      if (!this.TryRemove(index))
        throw new InvalidOperationException(string.Format("Item at index {0} is already empty", (object) index));
    }

    public void Remove(int index, out T removedItem)
    {
      if (!this.TryRemove(index, out removedItem))
        throw new InvalidOperationException(string.Format("Item at index {0} is already empty", (object) index));
    }

    public bool TryRemove(int index) => this.TryRemove(index, out T _);

    public bool TryRemove(int index, out T removedItem)
    {
      removedItem = this.m_list[index];
      if ((object) removedItem == null)
        return false;
      ++this.m_version;
      this.m_list[index] = default (T);
      this.m_freeList.Enqueue(index);
      return true;
    }

    private MyIndexList<T>.Enumerator GetEnumerator() => new MyIndexList<T>.Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      private MyIndexList<T> m_list;
      private int m_index;
      private int m_version;

      public Enumerator(MyIndexList<T> list)
      {
        this.m_list = list;
        this.m_index = -1;
        this.m_version = list.m_version;
      }

      public T Current
      {
        get
        {
          if (this.m_version != this.m_list.m_version)
            throw new InvalidOperationException("Collection was modified after enumerator was created");
          return this.m_list[this.m_index];
        }
      }

      public bool MoveNext()
      {
        do
        {
          ++this.m_index;
          if (this.m_index >= this.m_list.Count)
            return false;
        }
        while ((object) this.m_list[this.m_index] == null);
        return true;
      }

      void IDisposable.Dispose()
      {
      }

      object IEnumerator.Current => (object) this.Current;

      void IEnumerator.Reset()
      {
        this.m_index = -1;
        this.m_version = this.m_list.m_version;
      }
    }
  }
}
