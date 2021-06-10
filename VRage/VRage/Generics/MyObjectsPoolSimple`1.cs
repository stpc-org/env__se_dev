// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyObjectsPoolSimple`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Generics
{
  public class MyObjectsPoolSimple<T> where T : class, new()
  {
    private T[] m_items;
    private int m_nextAllocateIndex;

    public MyObjectsPoolSimple(int capacity) => this.m_items = new T[capacity];

    public T Allocate()
    {
      int index = Interlocked.Increment(ref this.m_nextAllocateIndex) - 1;
      if (index >= this.m_items.Length)
        return default (T);
      T obj = this.m_items[index];
      if ((object) obj == null)
        this.m_items[index] = obj = new T();
      return obj;
    }

    public int GetAllocatedCount() => Math.Min(this.m_nextAllocateIndex, this.m_items.Length);

    public int GetCapacity() => this.m_items.Length;

    public void ClearAllAllocated()
    {
      if (this.m_nextAllocateIndex > this.m_items.Length)
        Array.Resize<T>(ref this.m_items, Math.Max(this.m_nextAllocateIndex, this.m_items.Length * 2));
      this.m_nextAllocateIndex = 0;
    }

    public T GetAllocatedItem(int index) => this.m_items[index];

    public void Sort(IComparer<T> comparer)
    {
      if (this.m_nextAllocateIndex <= 1)
        return;
      Array.Sort<T>(this.m_items, 0, this.GetAllocatedCount(), comparer);
    }
  }
}
