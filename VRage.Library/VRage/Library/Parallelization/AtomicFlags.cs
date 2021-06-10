// Decompiled with JetBrains decompiler
// Type: VRage.Library.Parallelization.AtomicFlags
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

namespace VRage.Library.Parallelization
{
  public struct AtomicFlags
  {
    private int m_state;

    public bool IsSet(int flags) => (uint) (Volatile.Read(ref this.m_state) & flags) > 0U;

    public int Set(int flags)
    {
      int comparand = this.m_state;
      int num1;
      while (true)
      {
        num1 = comparand | flags;
        int num2 = Interlocked.CompareExchange(ref this.m_state, num1, comparand);
        if (comparand != num2)
          comparand = num2;
        else
          break;
      }
      return num1 ^ comparand;
    }

    public int Clear(int flags)
    {
      int comparand = this.m_state;
      int num1;
      while (true)
      {
        num1 = comparand & ~flags;
        int num2 = Interlocked.CompareExchange(ref this.m_state, num1, comparand);
        if (comparand != num2)
          comparand = num2;
        else
          break;
      }
      return num1 ^ comparand;
    }
  }
}
