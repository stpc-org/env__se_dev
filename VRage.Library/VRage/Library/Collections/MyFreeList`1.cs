// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyFreeList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage.Library.Collections
{
  public class MyFreeList<TItem>
  {
    private TItem[] m_list;
    private int m_size;
    private readonly Queue<int> m_freePositions;
    private readonly TItem m_default;

    public MyFreeList(int capacity = 16, TItem defaultValue = null)
    {
      this.m_list = new TItem[16];
      this.m_freePositions = new Queue<int>(capacity / 2);
      this.m_default = defaultValue;
    }

    public int Allocate()
    {
      int num;
      if (this.m_freePositions.Count > 0)
      {
        num = this.m_freePositions.Dequeue();
      }
      else
      {
        if (this.m_size == this.m_list.Length)
          Array.Resize<TItem>(ref this.m_list, this.m_list.Length << 1);
        num = this.m_size++;
      }
      return num;
    }

    public int Allocate(TItem value)
    {
      int index = this.Allocate();
      this.m_list[index] = value;
      return index;
    }

    public void Free(int position)
    {
      this.m_list[position] = this.m_default;
      if (position == this.m_size)
        --this.m_size;
      else
        this.m_freePositions.Enqueue(position);
    }

    public ref TItem this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => ref this.m_list[index];
    }

    public int UsedLength => this.m_size;

    public int Count => this.m_size - this.m_freePositions.Count;

    public int Capacity => this.m_list.Length;

    public TItem[] GetInternalArray() => this.m_list;

    public bool KeyValid(int key) => (long) (uint) key < (long) this.m_size;

    public void Clear()
    {
      for (int index = 0; index < this.m_size; ++index)
        this.m_list[index] = default (TItem);
      this.m_size = 0;
      this.m_freePositions.Clear();
    }
  }
}
