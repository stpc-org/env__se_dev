// Decompiled with JetBrains decompiler
// Type: VRage.FastNoArgsEvent
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage
{
  public class FastNoArgsEvent
  {
    private FastResourceLock m_lock = new FastResourceLock();
    private List<MyNoArgsDelegate> m_delegates = new List<MyNoArgsDelegate>(2);
    private List<MyNoArgsDelegate> m_delegatesIterator = new List<MyNoArgsDelegate>(2);

    public event MyNoArgsDelegate Event
    {
      add
      {
        using (this.m_lock.AcquireExclusiveUsing())
          this.m_delegates.Add(value);
      }
      remove
      {
        using (this.m_lock.AcquireExclusiveUsing())
          this.m_delegates.Remove(value);
      }
    }

    public void Raise()
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        this.m_delegatesIterator.Clear();
        foreach (MyNoArgsDelegate myNoArgsDelegate in this.m_delegates)
          this.m_delegatesIterator.Add(myNoArgsDelegate);
      }
      foreach (MyNoArgsDelegate myNoArgsDelegate in this.m_delegatesIterator)
        myNoArgsDelegate();
      this.m_delegatesIterator.Clear();
    }
  }
}
