// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyBinaryStructHeap`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Collections
{
  public class MyBinaryStructHeap<TKey, TValue> where TValue : struct
  {
    private MyBinaryStructHeap<TKey, TValue>.HeapItem[] m_array;
    private int m_count;
    private int m_capacity;
    private IComparer<TKey> m_comparer;

    public int Count => this.m_count;

    public bool Full => this.m_count == this.m_capacity;

    public MyBinaryStructHeap(int initialCapacity = 128, IComparer<TKey> comparer = null)
    {
      this.m_array = new MyBinaryStructHeap<TKey, TValue>.HeapItem[initialCapacity];
      this.m_count = 0;
      this.m_capacity = initialCapacity;
      this.m_comparer = comparer ?? (IComparer<TKey>) Comparer<TKey>.Default;
    }

    public void Insert(TValue value, TKey key)
    {
      if (this.m_count == this.m_capacity)
        this.Reallocate();
      this.m_array[this.m_count] = new MyBinaryStructHeap<TKey, TValue>.HeapItem()
      {
        Key = key,
        Value = value
      };
      this.Up(this.m_count);
      ++this.m_count;
    }

    public TValue Min() => this.m_array[0].Value;

    public TKey MinKey() => this.m_array[0].Key;

    public TValue RemoveMin()
    {
      TValue obj = this.m_array[0].Value;
      if (this.m_count != 1)
      {
        this.MoveItem(this.m_count - 1, 0);
        this.m_array[this.m_count - 1].Key = default (TKey);
        this.m_array[this.m_count - 1].Value = default (TValue);
        --this.m_count;
        this.Down(0);
      }
      else
      {
        --this.m_count;
        this.m_array[0].Key = default (TKey);
        this.m_array[0].Value = default (TValue);
      }
      return obj;
    }

    public TValue RemoveMax()
    {
      int index1 = 0;
      for (int index2 = 1; index2 < this.m_count; ++index2)
      {
        if (this.m_comparer.Compare(this.m_array[index1].Key, this.m_array[index2].Key) < 0)
          index1 = index2;
      }
      TValue obj = this.m_array[index1].Value;
      if (index1 != this.m_count)
      {
        this.MoveItem(this.m_count - 1, index1);
        this.Up(index1);
      }
      --this.m_count;
      return obj;
    }

    public TValue Remove(TValue value, IEqualityComparer<TValue> comparer = null)
    {
      if (this.m_count == 0)
        return default (TValue);
      if (comparer == null)
        comparer = (IEqualityComparer<TValue>) EqualityComparer<TValue>.Default;
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_count; ++index2)
      {
        if (comparer.Equals(value, this.m_array[index2].Value))
          index1 = index2;
      }
      TValue obj;
      if (index1 != this.m_count)
      {
        obj = this.m_array[index1].Value;
        this.MoveItem(this.m_count - 1, index1);
        this.Up(index1);
        this.Down(index1);
        --this.m_count;
      }
      else
        obj = default (TValue);
      return obj;
    }

    public TValue Remove(TKey key)
    {
      int index1 = 0;
      for (int index2 = 1; index2 < this.m_count; ++index2)
      {
        if (this.m_comparer.Compare(key, this.m_array[index2].Key) == 0)
          index1 = index2;
      }
      TValue obj;
      if (index1 != this.m_count)
      {
        obj = this.m_array[index1].Value;
        this.MoveItem(this.m_count - 1, index1);
        this.Up(index1);
        this.Down(index1);
      }
      else
        obj = default (TValue);
      --this.m_count;
      return obj;
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_count; ++index)
      {
        this.m_array[index].Key = default (TKey);
        this.m_array[index].Value = default (TValue);
      }
      this.m_count = 0;
    }

    private void Up(int index)
    {
      if (index == 0)
        return;
      int fromIndex = (index - 1) / 2;
      if (this.m_comparer.Compare(this.m_array[fromIndex].Key, this.m_array[index].Key) <= 0)
        return;
      MyBinaryStructHeap<TKey, TValue>.HeapItem fromItem = this.m_array[index];
      do
      {
        this.MoveItem(fromIndex, index);
        index = fromIndex;
        if (index != 0)
          fromIndex = (index - 1) / 2;
        else
          break;
      }
      while (this.m_comparer.Compare(this.m_array[fromIndex].Key, fromItem.Key) > 0);
      this.MoveItem(ref fromItem, index);
    }

    private void Down(int index)
    {
      if (this.m_count == index + 1)
        return;
      int fromIndex1 = index * 2 + 1;
      int fromIndex2 = fromIndex1 + 1;
      MyBinaryStructHeap<TKey, TValue>.HeapItem fromItem = this.m_array[index];
      while (fromIndex2 <= this.m_count)
      {
        if (fromIndex2 == this.m_count || this.m_comparer.Compare(this.m_array[fromIndex1].Key, this.m_array[fromIndex2].Key) < 0)
        {
          if (this.m_comparer.Compare(fromItem.Key, this.m_array[fromIndex1].Key) > 0)
          {
            this.MoveItem(fromIndex1, index);
            index = fromIndex1;
            fromIndex1 = index * 2 + 1;
            fromIndex2 = fromIndex1 + 1;
          }
          else
            break;
        }
        else if (this.m_comparer.Compare(fromItem.Key, this.m_array[fromIndex2].Key) > 0)
        {
          this.MoveItem(fromIndex2, index);
          index = fromIndex2;
          fromIndex1 = index * 2 + 1;
          fromIndex2 = fromIndex1 + 1;
        }
        else
          break;
      }
      this.MoveItem(ref fromItem, index);
    }

    private void MoveItem(int fromIndex, int toIndex) => this.m_array[toIndex] = this.m_array[fromIndex];

    private void MoveItem(
      ref MyBinaryStructHeap<TKey, TValue>.HeapItem fromItem,
      int toIndex)
    {
      this.m_array[toIndex] = fromItem;
    }

    private void Reallocate()
    {
      MyBinaryStructHeap<TKey, TValue>.HeapItem[] heapItemArray = new MyBinaryStructHeap<TKey, TValue>.HeapItem[this.m_capacity * 2];
      Array.Copy((Array) this.m_array, (Array) heapItemArray, this.m_capacity);
      this.m_array = heapItemArray;
      this.m_capacity *= 2;
    }

    public struct HeapItem
    {
      public TKey Key { get; internal set; }

      public TValue Value { get; internal set; }

      public override string ToString() => this.Key.ToString() + ": " + this.Value.ToString();
    }
  }
}
