// Decompiled with JetBrains decompiler
// Type: VRage.Library.Threading.SpinLock
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;
using System;
using System.Threading;

namespace VRage.Library.Threading
{
  public struct SpinLock
  {
    private Thread owner;
    private int recursion;

    public void Enter()
    {
      Thread currentThread = Thread.CurrentThread;
      if (this.owner == currentThread)
      {
        Interlocked.Increment(ref this.recursion);
      }
      else
      {
        MySpinWait mySpinWait = new MySpinWait();
        while (Interlocked.CompareExchange<Thread>(ref this.owner, currentThread, (Thread) null) != null)
          mySpinWait.SpinOnce();
        Interlocked.Increment(ref this.recursion);
      }
    }

    public bool TryEnter()
    {
      Thread currentThread = Thread.CurrentThread;
      if (this.owner == currentThread)
      {
        Interlocked.Increment(ref this.recursion);
        return true;
      }
      bool flag = Interlocked.CompareExchange<Thread>(ref this.owner, currentThread, (Thread) null) == null;
      if (flag)
        Interlocked.Increment(ref this.recursion);
      return flag;
    }

    public void Exit()
    {
      if (Thread.CurrentThread != this.owner)
        throw new InvalidOperationException("Exit cannot be called by a thread which does not currently own the lock.");
      Interlocked.Decrement(ref this.recursion);
      if (this.recursion != 0)
        return;
      this.owner = (Thread) null;
    }
  }
}
