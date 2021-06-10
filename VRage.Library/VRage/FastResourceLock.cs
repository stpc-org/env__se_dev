// Decompiled with JetBrains decompiler
// Type: VRage.FastResourceLock
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace VRage
{
  public sealed class FastResourceLock : IDisposable, IResourceLock
  {
    private const int LOCK_OWNED = 1;
    private const int LOCK_EXCLUSIVE_WAKING = 2;
    private const int LOCK_SHARED_OWNERS_SHIFT = 2;
    private const int LOCK_SHARED_OWNERS_MASK = 1023;
    private const int LOCK_SHARED_OWNERS_INCREMENT = 4;
    private const int LOCK_SHARED_WAITERS_SHIFT = 12;
    private const int LOCK_SHARED_WAITERS_MASK = 1023;
    private const int LOCK_SHARED_WAITERS_INCREMENT = 4096;
    private const int LOCK_EXCLUSIVE_WAITERS_SHIFT = 22;
    private const int LOCK_EXCLUSIVE_WAITERS_MASK = 1023;
    private const int LOCK_EXCLUSIVE_WAITERS_INCREMENT = 4194304;
    private const int EXCLUSIVE_MASK = -4194302;
    private static readonly int SPIN_COUNT = Environment.ProcessorCount != 1 ? 4000 : 0;
    private int m_value;
    private Semaphore m_sharedWakeEvent;
    private Semaphore m_exclusiveWakeEvent;

    public FastResourceLock()
    {
      this.m_value = 0;
      this.m_sharedWakeEvent = new Semaphore(0, int.MaxValue);
      this.m_exclusiveWakeEvent = new Semaphore(0, int.MaxValue);
    }

    ~FastResourceLock() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      if (this.m_sharedWakeEvent != null)
      {
        this.m_sharedWakeEvent.Dispose();
        this.m_sharedWakeEvent = (Semaphore) null;
      }
      if (this.m_exclusiveWakeEvent == null)
        return;
      this.m_exclusiveWakeEvent.Dispose();
      this.m_exclusiveWakeEvent = (Semaphore) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public int ExclusiveWaiters => this.m_value >> 22 & 1023;

    public bool Owned => (uint) (this.m_value & 1) > 0U;

    public int SharedOwners => this.m_value >> 2 & 1023;

    public int SharedWaiters => this.m_value >> 12 & 1023;

    [DebuggerStepThrough]
    public void AcquireExclusive()
    {
      int num = 0;
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand & 3) == 0)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand + 1, comparand) == comparand)
            break;
        }
        else if (num >= FastResourceLock.SPIN_COUNT && Interlocked.CompareExchange(ref this.m_value, comparand + 4194304, comparand) == comparand)
          goto label_5;
        ++num;
      }
      return;
label_5:
      this.m_exclusiveWakeEvent.WaitOne();
      SpinWait spinWait = new SpinWait();
      for (int comparand = this.m_value; Interlocked.CompareExchange(ref this.m_value, comparand + 1 - 2, comparand) != comparand; comparand = this.m_value)
        spinWait.SpinOnce();
    }

    [DebuggerStepThrough]
    public void AcquireShared()
    {
      int num = 0;
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand & -4190209) == 0)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand + 1 + 4, comparand) == comparand)
            break;
        }
        else if ((comparand & 1) != 0 && (comparand >> 2 & 1023) != 0 && (comparand & -4194302) == 0)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand + 4, comparand) == comparand)
            goto label_3;
        }
        else if (num >= FastResourceLock.SPIN_COUNT && Interlocked.CompareExchange(ref this.m_value, comparand + 4096, comparand) == comparand)
        {
          this.m_sharedWakeEvent.WaitOne();
          continue;
        }
        ++num;
      }
      return;
label_3:;
    }

    public void ConvertExclusiveToShared()
    {
      SpinWait spinWait = new SpinWait();
      int releaseCount;
      while (true)
      {
        int comparand = this.m_value;
        releaseCount = comparand >> 12 & 1023;
        if (Interlocked.CompareExchange(ref this.m_value, comparand + 4 & -4190209, comparand) != comparand)
          spinWait.SpinOnce();
        else
          break;
      }
      if (releaseCount == 0)
        return;
      this.m_sharedWakeEvent.Release(releaseCount);
    }

    public void ReleaseExclusive()
    {
      SpinWait spinWait = new SpinWait();
      int releaseCount;
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand >> 22 & 1023) != 0)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand - 1 + 2 - 4194304, comparand) == comparand)
            break;
        }
        else
        {
          releaseCount = comparand >> 12 & 1023;
          if (Interlocked.CompareExchange(ref this.m_value, comparand & -4190210, comparand) == comparand)
            goto label_5;
        }
        spinWait.SpinOnce();
      }
      this.m_exclusiveWakeEvent.Release(1);
      return;
label_5:
      if (releaseCount == 0)
        return;
      this.m_sharedWakeEvent.Release(releaseCount);
    }

    public void ReleaseShared()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand >> 2 & 1023) > 1)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand - 4, comparand) == comparand)
            break;
        }
        else if ((comparand >> 22 & 1023) != 0)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand - 1 + 2 - 4 - 4194304, comparand) == comparand)
            goto label_6;
        }
        else if (Interlocked.CompareExchange(ref this.m_value, comparand - 1 - 4, comparand) == comparand)
          goto label_3;
        spinWait.SpinOnce();
      }
      return;
label_6:
      this.m_exclusiveWakeEvent.Release(1);
      return;
label_3:;
    }

    [DebuggerStepThrough]
    public void SpinAcquireExclusive()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand & 3) != 0 || Interlocked.CompareExchange(ref this.m_value, comparand + 1, comparand) != comparand)
          spinWait.SpinOnce();
        else
          break;
      }
    }

    [DebuggerStepThrough]
    public void SpinAcquireShared()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand & -4194302) == 0)
        {
          if ((comparand & 1) == 0)
          {
            if (Interlocked.CompareExchange(ref this.m_value, comparand + 1 + 4, comparand) == comparand)
              break;
          }
          else if ((comparand >> 2 & 1023) != 0 && Interlocked.CompareExchange(ref this.m_value, comparand + 4, comparand) == comparand)
            goto label_4;
        }
        spinWait.SpinOnce();
      }
      return;
label_4:;
    }

    [DebuggerStepThrough]
    public void SpinConvertSharedToExclusive()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand >> 2 & 1023) != 1 || Interlocked.CompareExchange(ref this.m_value, comparand - 4, comparand) != comparand)
          spinWait.SpinOnce();
        else
          break;
      }
    }

    public bool TryAcquireExclusive()
    {
      int comparand = this.m_value;
      return (comparand & 3) == 0 && Interlocked.CompareExchange(ref this.m_value, comparand + 1, comparand) == comparand;
    }

    public bool TryAcquireShared()
    {
      int comparand = this.m_value;
      if ((comparand & -4194302) != 0)
        return false;
      if ((comparand & 1) == 0)
        return Interlocked.CompareExchange(ref this.m_value, comparand + 1 + 4, comparand) == comparand;
      return (comparand >> 2 & 1023) != 0 && Interlocked.CompareExchange(ref this.m_value, comparand + 4, comparand) == comparand;
    }

    public bool TryConvertSharedToExclusive()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int comparand = this.m_value;
        if ((comparand >> 2 & 1023) == 1)
        {
          if (Interlocked.CompareExchange(ref this.m_value, comparand - 4, comparand) != comparand)
            spinWait.SpinOnce();
          else
            goto label_4;
        }
        else
          break;
      }
      return false;
label_4:
      return true;
    }
  }
}
