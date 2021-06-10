// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentHashSet`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
  public class MyConcurrentHashSet<T> : IEnumerable<T>, IEnumerable
  {
    private HashSet<T> m_set;
    private SpinLockRef m_lock = new SpinLockRef();

    public MyConcurrentHashSet() => this.m_set = new HashSet<T>();

    public MyConcurrentHashSet(IEqualityComparer<T> comparer) => this.m_set = new HashSet<T>(comparer);

    public int Count
    {
      get
      {
        using (this.m_lock.Acquire())
          return this.m_set.Count;
      }
    }

    public void Clear()
    {
      using (this.m_lock.Acquire())
        this.m_set.Clear();
    }

    public bool Add(T instance)
    {
      using (this.m_lock.Acquire())
        return this.m_set.Add(instance);
    }

    public bool Remove(T value)
    {
      using (this.m_lock.Acquire())
        return this.m_set.Remove(value);
    }

    public bool Contains(T value)
    {
      using (this.m_lock.Acquire())
        return this.m_set.Contains(value);
    }

    public ConcurrentEnumerator<SpinLockRef.Token, T, HashSet<T>.Enumerator> GetEnumerator() => ConcurrentEnumerator.Create<SpinLockRef.Token, T, HashSet<T>.Enumerator>(this.m_lock.Acquire(), this.m_set.GetEnumerator());

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
