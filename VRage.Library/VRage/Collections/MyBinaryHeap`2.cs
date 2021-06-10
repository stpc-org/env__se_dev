// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyBinaryHeap`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Collections
{
  public class MyBinaryHeap<K, V> where V : HeapItem<K>
  {
    private HeapItem<K>[] m_array;
    private int m_count;
    private int m_capacity;
    private IComparer<K> m_comparer;

    public int Count => this.m_count;

    public bool Full => this.m_count == this.m_capacity;

    public MyBinaryHeap()
    {
      this.m_array = new HeapItem<K>[128];
      this.m_count = 0;
      this.m_capacity = 128;
      this.m_comparer = (IComparer<K>) Comparer<K>.Default;
    }

    public MyBinaryHeap(int initialCapacity)
    {
      this.m_array = new HeapItem<K>[initialCapacity];
      this.m_count = 0;
      this.m_capacity = initialCapacity;
      this.m_comparer = (IComparer<K>) Comparer<K>.Default;
    }

    public MyBinaryHeap(int initialCapacity, IComparer<K> comparer)
    {
      this.m_array = new HeapItem<K>[initialCapacity];
      this.m_count = 0;
      this.m_capacity = initialCapacity;
      this.m_comparer = comparer;
    }

    public void Insert(V value, K key)
    {
      if (this.m_count == this.m_capacity)
        this.Reallocate();
      value.HeapKey = key;
      this.MoveItem((HeapItem<K>) value, this.m_count);
      this.Up(this.m_count);
      ++this.m_count;
    }

    public V GetItem(int index) => this.m_array[index] as V;

    public V Min() => (V) this.m_array[0];

    public V RemoveMin()
    {
      V v = (V) this.m_array[0];
      if (this.m_count != 1)
      {
        this.MoveItem(this.m_count - 1, 0);
        this.m_array[this.m_count - 1] = (HeapItem<K>) null;
        --this.m_count;
        this.Down(0);
      }
      else
      {
        --this.m_count;
        this.m_array[0] = (HeapItem<K>) null;
      }
      return v;
    }

    public V RemoveMax()
    {
      int index1 = 0;
      for (int index2 = 1; index2 < this.m_count; ++index2)
      {
        if (this.m_comparer.Compare(this.m_array[index1].HeapKey, this.m_array[index2].HeapKey) < 0)
          index1 = index2;
      }
      V v = this.m_array[index1] as V;
      if (index1 != this.m_count)
      {
        this.MoveItem(this.m_count - 1, index1);
        this.Up(index1);
      }
      --this.m_count;
      return v;
    }

    public void Remove(V item)
    {
      if (this.m_count != 1)
      {
        if (this.m_count - 1 == item.HeapIndex)
        {
          this.m_array[this.m_count - 1] = (HeapItem<K>) null;
          --this.m_count;
        }
        else
        {
          this.MoveItem(this.m_count - 1, item.HeapIndex);
          this.m_array[this.m_count - 1] = (HeapItem<K>) null;
          --this.m_count;
          if (this.m_comparer.Compare(item.HeapKey, this.m_array[item.HeapIndex].HeapKey) < 0)
            this.Down(item.HeapIndex);
          else
            this.Up(item.HeapIndex);
        }
      }
      else
      {
        --this.m_count;
        this.m_array[0] = (HeapItem<K>) null;
      }
    }

    public void Modify(V item, K newKey)
    {
      K heapKey = item.HeapKey;
      item.HeapKey = newKey;
      if (this.m_comparer.Compare(heapKey, newKey) <= 0)
        this.Down(item.HeapIndex);
      else
        this.Up(item.HeapIndex);
    }

    public void ModifyUp(V item, K newKey)
    {
      item.HeapKey = newKey;
      this.Up(item.HeapIndex);
    }

    public void ModifyDown(V item, K newKey)
    {
      item.HeapKey = newKey;
      this.Down(item.HeapIndex);
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_count; ++index)
        this.m_array[index] = (HeapItem<K>) null;
      this.m_count = 0;
    }

    private void Up(int index)
    {
      if (index == 0)
        return;
      int fromIndex = (index - 1) / 2;
      if (this.m_comparer.Compare(this.m_array[fromIndex].HeapKey, this.m_array[index].HeapKey) <= 0)
        return;
      HeapItem<K> fromItem = this.m_array[index];
      do
      {
        this.MoveItem(fromIndex, index);
        index = fromIndex;
        if (index != 0)
          fromIndex = (index - 1) / 2;
        else
          break;
      }
      while (this.m_comparer.Compare(this.m_array[fromIndex].HeapKey, fromItem.HeapKey) > 0);
      this.MoveItem(fromItem, index);
    }

    private void Down(int index)
    {
      if (this.m_count == index + 1)
        return;
      int fromIndex1 = index * 2 + 1;
      int fromIndex2 = fromIndex1 + 1;
      HeapItem<K> fromItem = this.m_array[index];
      while (fromIndex2 <= this.m_count)
      {
        if (fromIndex2 == this.m_count || this.m_comparer.Compare(this.m_array[fromIndex1].HeapKey, this.m_array[fromIndex2].HeapKey) < 0)
        {
          if (this.m_comparer.Compare(fromItem.HeapKey, this.m_array[fromIndex1].HeapKey) > 0)
          {
            this.MoveItem(fromIndex1, index);
            index = fromIndex1;
            fromIndex1 = index * 2 + 1;
            fromIndex2 = fromIndex1 + 1;
          }
          else
            break;
        }
        else if (this.m_comparer.Compare(fromItem.HeapKey, this.m_array[fromIndex2].HeapKey) > 0)
        {
          this.MoveItem(fromIndex2, index);
          index = fromIndex2;
          fromIndex1 = index * 2 + 1;
          fromIndex2 = fromIndex1 + 1;
        }
        else
          break;
      }
      this.MoveItem(fromItem, index);
    }

    private void MoveItem(int fromIndex, int toIndex)
    {
      this.m_array[toIndex] = this.m_array[fromIndex];
      this.m_array[toIndex].HeapIndex = toIndex;
    }

    private void MoveItem(HeapItem<K> fromItem, int toIndex)
    {
      this.m_array[toIndex] = fromItem;
      this.m_array[toIndex].HeapIndex = toIndex;
    }

    private void Reallocate()
    {
      HeapItem<K>[] heapItemArray = new HeapItem<K>[this.m_capacity * 2];
      Array.Copy((Array) this.m_array, (Array) heapItemArray, this.m_capacity);
      this.m_array = heapItemArray;
      this.m_capacity *= 2;
    }

    public void QueryAll(List<V> list)
    {
      foreach (HeapItem<K> heapItem in this.m_array)
      {
        if (heapItem != null)
          list.Add((V) heapItem);
      }
    }
  }
}
