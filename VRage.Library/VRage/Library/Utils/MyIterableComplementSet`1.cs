// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyIterableComplementSet`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Library.Utils
{
  public class MyIterableComplementSet<T> : IEnumerable<T>, IEnumerable
  {
    private Dictionary<T, int> m_index = new Dictionary<T, int>();
    private List<T> m_data = new List<T>();
    private int m_split;

    public void Add(T item)
    {
      this.m_index.Add(item, this.m_data.Count);
      this.m_data.Add(item);
    }

    public void AddToComplement(T item)
    {
      this.m_index.Add(item, this.m_data.Count);
      this.m_data.Add(item);
      this.MoveToComplement(item);
    }

    public void Remove(T item)
    {
      int split = this.m_index[item];
      if (this.m_split > split)
      {
        --this.m_split;
        T key = this.m_data[this.m_split];
        this.m_index[key] = split;
        this.m_data[split] = key;
        split = this.m_split;
      }
      int index = this.m_data.Count - 1;
      this.m_data[split] = this.m_data[index];
      this.m_index[this.m_data[index]] = split;
      this.m_index.Remove(item);
      this.m_data.RemoveAt(index);
    }

    public void MoveToComplement(T item)
    {
      T key = this.m_data[this.m_split];
      int index = this.m_index[item];
      this.m_data[this.m_split] = item;
      this.m_index[item] = this.m_split;
      this.m_data[index] = key;
      this.m_index[key] = index;
      ++this.m_split;
    }

    public bool Contains(T item) => this.m_index.ContainsKey(item);

    public bool IsInComplement(T item) => this.m_index[item] < this.m_split;

    public void MoveToSet(T item)
    {
      --this.m_split;
      T key = this.m_data[this.m_split];
      int index = this.m_index[item];
      this.m_data[this.m_split] = item;
      this.m_index[item] = this.m_split;
      this.m_data[index] = key;
      this.m_index[key] = index;
    }

    public void ClearSet()
    {
      for (int split = this.m_split; split < this.m_data.Count; ++split)
        this.m_index.Remove(this.m_data[split]);
      this.m_data.RemoveRange(this.m_split, this.m_data.Count - this.m_split);
    }

    public void ClearComplement()
    {
      for (int split = this.m_split; split < this.m_data.Count; ++split)
        this.m_index.Remove(this.m_data[split]);
      this.m_data.RemoveRange(this.m_split, this.m_data.Count - this.m_split);
    }

    public void AllToComplement() => this.m_split = this.m_data.Count;

    public void AllToSet() => this.m_split = 0;

    public IEnumerable<T> Set() => (IEnumerable<T>) MyRangeIterator<T>.ForRange(this.m_data, this.m_split, this.m_data.Count);

    public IEnumerable<T> Complement() => (IEnumerable<T>) MyRangeIterator<T>.ForRange(this.m_data, 0, this.m_split);

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.m_data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Clear()
    {
      this.m_split = 0;
      this.m_index.Clear();
      this.m_data.Clear();
    }
  }
}
