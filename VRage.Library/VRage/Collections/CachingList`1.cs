// Decompiled with JetBrains decompiler
// Type: VRage.Collections.CachingList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Collections
{
  public class CachingList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
  {
    private List<T> m_list = new List<T>();
    private List<T> m_toAdd = new List<T>();
    private List<T> m_toRemove = new List<T>();

    public CachingList()
    {
    }

    public CachingList(int capacity) => this.m_list = new List<T>(capacity);

    public int Count => this.m_list.Count;

    public bool HasChanges => this.m_toAdd.Count > 0 || this.m_toRemove.Count > 0;

    public T this[int index] => this.m_list[index];

    public void Add(T entity)
    {
      if (this.m_toRemove.Contains(entity))
        this.m_toRemove.Remove(entity);
      else
        this.m_toAdd.Add(entity);
    }

    public void Remove(T entity, bool immediate = false)
    {
      int index = this.m_toAdd.IndexOf(entity);
      if (index >= 0)
        this.m_toAdd.RemoveAt(index);
      else
        this.m_toRemove.Add(entity);
      if (!immediate)
        return;
      this.m_list.Remove(entity);
      this.m_toRemove.Remove(entity);
    }

    public void RemoveAtImmediately(int index)
    {
      if (index < 0 || index >= this.m_list.Count)
        return;
      this.m_list.RemoveAt(index);
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_list.Count; ++index)
        this.Remove(this.m_list[index]);
    }

    public void ClearImmediate()
    {
      this.m_toAdd.Clear();
      this.m_toRemove.Clear();
      this.m_list.Clear();
    }

    public void ApplyChanges()
    {
      this.ApplyAdditions();
      this.ApplyRemovals();
    }

    public void ApplyAdditions()
    {
      this.m_list.AddRange((IEnumerable<T>) this.m_toAdd);
      this.m_toAdd.Clear();
    }

    public void ApplyRemovals()
    {
      foreach (T obj in this.m_toRemove)
        this.m_list.Remove(obj);
      this.m_toRemove.Clear();
    }

    public List<T> CopyWithChanges()
    {
      List<T> objList = new List<T>((IEnumerable<T>) this.m_list);
      objList.AddRange((IEnumerable<T>) this.m_toAdd);
      foreach (T obj in this.m_toRemove)
        objList.Remove(obj);
      return objList;
    }

    public void Sort(IComparer<T> comparer) => this.m_list.Sort(comparer);

    public List<T>.Enumerator GetEnumerator() => this.m_list.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [Conditional("DEBUG")]
    public void DebugCheckEmpty()
    {
    }

    public override string ToString() => string.Format("Count = {0}; ToAdd = {1}; ToRemove = {2}", (object) this.m_list.Count, (object) this.m_toAdd.Count, (object) this.m_toRemove.Count);
  }
}
