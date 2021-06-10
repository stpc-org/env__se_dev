// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Semaphore
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

namespace ParallelTasks
{
  public class Semaphore
  {
    private AutoResetEvent gate;
    private int free;
    private object free_lock = new object();

    public Semaphore(int maximumCount)
    {
      this.free = maximumCount;
      this.gate = new AutoResetEvent(this.free > 0);
    }

    public void WaitOne()
    {
      this.gate.WaitOne();
      lock (this.free_lock)
      {
        --this.free;
        if (this.free <= 0)
          return;
        this.gate.Set();
      }
    }

    public void Release()
    {
      lock (this.free_lock)
      {
        ++this.free;
        this.gate.Set();
      }
    }
  }
}
