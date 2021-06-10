// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MySlidingWindow`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Collections
{
  public class MySlidingWindow<T>
  {
    private MyQueue<T> m_items;
    public int Size;
    public T DefaultValue;
    public Func<MyQueue<T>, T> AverageFunc;

    public T Average => this.m_items.Count == 0 ? this.DefaultValue : this.AverageFunc(this.m_items);

    public T Last => this.m_items.Count <= 0 ? this.DefaultValue : this.m_items[this.m_items.Count - 1];

    public MySlidingWindow(int size, Func<MyQueue<T>, T> avg, T defaultValue = null)
    {
      this.AverageFunc = avg;
      this.Size = size;
      this.DefaultValue = defaultValue;
      this.m_items = new MyQueue<T>(size + 1);
    }

    public void Add(T item)
    {
      this.m_items.Enqueue(item);
      this.RemoveExcess();
    }

    public void Clear() => this.m_items.Clear();

    private void RemoveExcess()
    {
      while (this.m_items.Count > this.Size)
        this.m_items.Dequeue();
    }
  }
}
