// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyDistributedUpdater`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public class MyDistributedUpdater<TList, TElement> : IEnumerable<TElement>, IEnumerable
    where TList : IReadOnlyList<TElement>, new()
  {
    private TList m_list = new TList();
    private int m_updateIndex;

    public int UpdateInterval { get; set; }

    public MyDistributedUpdater(int updateInterval) => this.UpdateInterval = updateInterval;

    public void Iterate(Action<TElement> p)
    {
      for (int updateIndex = this.m_updateIndex; updateIndex < this.m_list.Count; updateIndex += this.UpdateInterval)
        p(this.m_list[updateIndex]);
    }

    public void Update()
    {
      ++this.m_updateIndex;
      this.m_updateIndex %= this.UpdateInterval;
    }

    public TList List => this.m_list;

    public MyDistributedUpdater<TList, TElement>.Enumerator GetEnumerator() => new MyDistributedUpdater<TList, TElement>.Enumerator(this);

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator() => (IEnumerator<TElement>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Enumerator : IEnumerator<TElement>, IEnumerator, IDisposable
    {
      private MyDistributedUpdater<TList, TElement> m_updater;
      private int m_index;

      public Enumerator(MyDistributedUpdater<TList, TElement> updater)
      {
        this.m_updater = updater;
        this.m_index = updater.m_updateIndex - updater.UpdateInterval;
      }

      public bool MoveNext()
      {
        int num = this.m_index + this.m_updater.UpdateInterval;
        if (num >= this.m_updater.List.Count)
          return false;
        this.m_index = num;
        return true;
      }

      public void Reset() => this.m_index = -this.m_updater.UpdateInterval;

      public TElement Current => this.m_updater.List[this.m_index];

      object IEnumerator.Current => (object) this.Current;

      public void Dispose()
      {
        this.m_updater = (MyDistributedUpdater<TList, TElement>) null;
        this.m_index = -1;
      }
    }
  }
}
