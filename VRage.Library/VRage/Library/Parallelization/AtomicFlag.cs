// Decompiled with JetBrains decompiler
// Type: VRage.Library.Parallelization.AtomicFlag
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

namespace VRage.Library.Parallelization
{
  public struct AtomicFlag
  {
    private int m_state;

    public bool IsSet
    {
      get => (uint) Volatile.Read(ref this.m_state) > 0U;
      set
      {
        if (value)
          this.Set();
        else
          this.Clear();
      }
    }

    public bool Set() => Interlocked.Exchange(ref this.m_state, 1) == 0;

    public bool Clear() => (uint) Interlocked.Exchange(ref this.m_state, 0) > 0U;
  }
}
