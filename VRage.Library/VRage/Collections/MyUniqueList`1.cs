// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyUniqueList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class MyUniqueList<T>
  {
    private List<T> m_list = new List<T>();
    private HashSet<T> m_hashSet = new HashSet<T>();
    private SpinLockRef m_lock = new SpinLockRef();

    public int Count => this.m_list.Count;

    public T this[int index] => this.m_list[index];

    public bool Add(T item)
    {
      using (this.m_lock.Acquire())
      {
        if (!this.m_hashSet.Add(item))
          return false;
        this.m_list.Add(item);
        return true;
      }
    }

    public bool Insert(int index, T item)
    {
      using (this.m_lock.Acquire())
      {
        if (this.m_hashSet.Add(item))
        {
          this.m_list.Insert(index, item);
          return true;
        }
        this.m_list.Remove(item);
        this.m_list.Insert(index, item);
        return false;
      }
    }

    public bool Remove(T item)
    {
      using (this.m_lock.Acquire())
      {
        if (!this.m_hashSet.Remove(item))
          return false;
        this.m_list.Remove(item);
        return true;
      }
    }

    public void Clear()
    {
      this.m_list.Clear();
      this.m_hashSet.Clear();
    }

    public bool Contains(T item) => this.m_hashSet.Contains(item);

    public UniqueListReader<T> Items => new UniqueListReader<T>(this);

    public ListReader<T> ItemList => new ListReader<T>(this.m_list);

    public List<T>.Enumerator GetEnumerator() => this.m_list.GetEnumerator();
  }
}
