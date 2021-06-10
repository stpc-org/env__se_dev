// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyQueue`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VRage.Collections
{
  public class MyQueue<T> : IEnumerable<T>, IEnumerable
  {
    protected T[] m_array;
    protected int m_head;
    protected int m_tail;
    protected int m_size;
    private int m_version;

    public MyQueue()
      : this(0)
    {
    }

    public MyQueue(int capacity)
    {
      this.m_array = capacity >= 0 ? new T[capacity] : throw new ArgumentException("Capacity cannot be < 0", nameof (capacity));
      this.m_head = 0;
      this.m_tail = 0;
      this.m_size = 0;
      this.m_version = 0;
    }

    public MyQueue(IEnumerable<T> collection)
    {
      if (collection == null)
        throw new ArgumentException("Collection cannot be empty", nameof (collection));
      this.m_size = 0;
      this.m_version = 0;
      this.m_array = !(collection is ICollection<T> objs) ? new T[4] : new T[objs.Count];
      foreach (T obj in collection)
        this.Enqueue(obj);
    }

    public T[] InternalArray
    {
      get
      {
        T[] objArray = new T[this.Count];
        for (int index = 0; index < this.Count; ++index)
          objArray[index] = this[index];
        return objArray;
      }
    }

    public void Clear()
    {
      if (this.m_head < this.m_tail)
      {
        Array.Clear((Array) this.m_array, this.m_head, this.m_size);
      }
      else
      {
        Array.Clear((Array) this.m_array, this.m_head, this.m_array.Length - this.m_head);
        Array.Clear((Array) this.m_array, 0, this.m_tail);
      }
      this.m_head = 0;
      this.m_tail = 0;
      this.m_size = 0;
      ++this.m_version;
    }

    public int Count => this.m_size;

    public ref T this[int index]
    {
      get
      {
        if (index < 0 || index >= this.Count)
          throw new ArgumentException("Index must be larger or equal to 0 and smaller than Count");
        return ref this.m_array[(this.m_head + index) % this.m_array.Length];
      }
    }

    public void Enqueue(T item)
    {
      if (this.m_size == this.m_array.Length)
      {
        int capacity = (int) ((long) this.m_array.Length * 200L / 100L);
        if (capacity < this.m_array.Length + 4)
          capacity = this.m_array.Length + 4;
        this.SetCapacity(capacity);
      }
      this.m_array[this.m_tail] = item;
      this.m_tail = (this.m_tail + 1) % this.m_array.Length;
      ++this.m_size;
      ++this.m_version;
    }

    public T Peek()
    {
      if (this.m_size == 0)
        throw new InvalidOperationException("Queue is empty");
      return this.m_array[this.m_head];
    }

    public T Last()
    {
      if (this.m_size == 0)
        throw new InvalidOperationException("Queue is empty");
      return this.m_array[(this.m_tail - 1 + this.m_array.Length) % this.m_array.Length];
    }

    public T Dequeue()
    {
      if (this.m_size == 0)
        throw new InvalidOperationException("Queue is empty");
      T obj = this.m_array[this.m_head];
      this.m_array[this.m_head] = default (T);
      this.m_head = (this.m_head + 1) % this.m_array.Length;
      --this.m_size;
      ++this.m_version;
      return obj;
    }

    public bool TryDequeue(out T item)
    {
      if (this.m_size > 0)
      {
        item = this.Dequeue();
        return true;
      }
      item = default (T);
      return false;
    }

    public bool Contains(T item)
    {
      int head = this.m_head;
      int num = 0;
      while (num < this.m_size)
      {
        if (this.m_array[head % this.m_array.Length].Equals((object) item))
          return true;
        ++num;
        ++head;
      }
      return false;
    }

    public bool Remove(T item)
    {
      int head = this.m_head;
      int num;
      for (num = 0; num < this.m_size && !this.m_array[head % this.m_array.Length].Equals((object) item); ++head)
        ++num;
      if (num == this.m_size)
        return false;
      this.Remove(head);
      return true;
    }

    public bool RemoveWhere(Func<T, bool> predicate, out T item)
    {
      int head = this.m_head;
      int num;
      for (num = 0; num < this.m_size && !predicate(this.m_array[head % this.m_array.Length]); ++head)
        ++num;
      if (num == this.m_size)
      {
        item = default (T);
        return false;
      }
      item = this.m_array[head];
      this.Remove(head);
      return true;
    }

    public void Remove(int idx)
    {
      if (idx >= this.m_size)
        throw new InvalidOperationException("Index out of range " + (object) idx + "/" + (object) this.m_size);
      if (idx == 0)
      {
        this.Dequeue();
      }
      else
      {
        int index1 = idx % this.m_array.Length;
        int index2;
        int index3;
        for (index2 = (this.m_tail + this.m_array.Length - 1) % this.m_array.Length; index1 != index2; index1 = index3)
        {
          index3 = (index1 + 1) % this.m_array.Length;
          this.m_array[index1] = this.m_array[index3];
        }
        this.m_array[index2] = default (T);
        this.m_tail = index2;
        --this.m_size;
        ++this.m_version;
      }
    }

    protected void SetCapacity(int capacity)
    {
      T[] objArray = new T[capacity];
      if (this.m_size > 0)
      {
        if (this.m_head < this.m_tail)
        {
          Array.Copy((Array) this.m_array, this.m_head, (Array) objArray, 0, this.m_size);
        }
        else
        {
          Array.Copy((Array) this.m_array, this.m_head, (Array) objArray, 0, this.m_array.Length - this.m_head);
          Array.Copy((Array) this.m_array, 0, (Array) objArray, this.m_array.Length - this.m_head, this.m_tail);
        }
      }
      this.m_array = objArray;
      this.m_head = 0;
      this.m_tail = this.m_size == capacity ? 0 : this.m_size;
    }

    public void TrimExcess()
    {
      if (this.m_size >= (int) ((double) this.m_array.Length * 0.9))
        return;
      this.SetCapacity(this.m_size);
    }

    public MyQueue<T>.Enumerator GetEnumerator() => new MyQueue<T>.Enumerator(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[');
      if (this.Count > 0)
      {
        stringBuilder.Append((object) this[this.Count - 1]);
        for (int index = this.Count - 2; index >= 0; --index)
        {
          stringBuilder.Append(", ");
          stringBuilder.Append((object) this[index]);
        }
      }
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }

    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      private readonly int m_version;
      private readonly MyQueue<T> m_queue;
      private int m_index;
      private bool m_first;

      public Enumerator(MyQueue<T> queue)
      {
        this.m_index = 0;
        this.m_first = true;
        this.m_queue = queue;
        this.m_version = this.m_queue.m_version;
        this.Reset();
      }

      public bool MoveNext()
      {
        if (this.m_version != this.m_queue.m_version)
          throw new InvalidOperationException("Collection modified");
        if (this.m_queue.Count == 0)
          return false;
        ++this.m_index;
        if (this.m_index == this.m_queue.m_array.Length)
          this.m_index = 0;
        if (this.m_first)
          this.m_first = false;
        else if (this.m_index == this.m_queue.m_tail)
          return false;
        return true;
      }

      public void Reset()
      {
        this.m_first = true;
        this.m_index = this.m_queue.m_head - 1;
      }

      public T Current => this.m_queue.m_array[this.m_index];

      object IEnumerator.Current => (object) this.Current;

      public void Dispose()
      {
      }
    }
  }
}
