// Decompiled with JetBrains decompiler
// Type: VRage.Library.Algorithms.SequenceIdGenerator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Threading;

namespace VRage.Library.Algorithms
{
  public class SequenceIdGenerator
  {
    private uint m_maxId;
    private Queue<SequenceIdGenerator.Item> m_reuseQueue;
    private int m_protecionCount;
    private uint m_reuseProtectionTime;
    private Func<uint> m_timeFunc;
    private SpinLockRef m_lock = new SpinLockRef();

    public int WaitingInQueue => this.m_reuseQueue.Count;

    public uint ReservedCount { get; private set; }

    public SequenceIdGenerator(
      int reuseProtectionCount = 2048,
      uint reuseProtectionTime = 60,
      Func<uint> timeFunc = null)
    {
      this.m_reuseQueue = new Queue<SequenceIdGenerator.Item>(reuseProtectionCount);
      this.m_protecionCount = Math.Max(0, reuseProtectionCount);
      this.m_reuseProtectionTime = reuseProtectionTime;
      this.m_timeFunc = timeFunc;
    }

    public static SequenceIdGenerator CreateWithStopwatch(
      TimeSpan reuseProtectionTime,
      int reuseProtectionCount = 2048)
    {
      Stopwatch sw = Stopwatch.StartNew();
      if (reuseProtectionTime.TotalSeconds > 5.0)
        return new SequenceIdGenerator(reuseProtectionCount, (uint) reuseProtectionTime.TotalSeconds, (Func<uint>) (() => (uint) sw.Elapsed.TotalSeconds));
      if (reuseProtectionTime.TotalMilliseconds > 500.0)
        return new SequenceIdGenerator(reuseProtectionCount, (uint) (reuseProtectionTime.TotalSeconds * 10.0), (Func<uint>) (() => (uint) (sw.Elapsed.TotalSeconds * 10.0)));
      return reuseProtectionTime.TotalMilliseconds > 50.0 ? new SequenceIdGenerator(reuseProtectionCount, (uint) (reuseProtectionTime.TotalSeconds * 100.0), (Func<uint>) (() => (uint) (sw.Elapsed.TotalSeconds * 100.0))) : new SequenceIdGenerator(reuseProtectionCount, (uint) reuseProtectionTime.TotalMilliseconds, (Func<uint>) (() => (uint) sw.Elapsed.TotalMilliseconds));
    }

    public void Reserve(uint reservedIdCount)
    {
      this.m_maxId = this.m_maxId == 0U ? reservedIdCount : throw new InvalidOperationException("Reserve can be called only once and before any IDs are generated.");
      this.ReservedCount = reservedIdCount;
    }

    private bool CheckFirstItemTime()
    {
      if (this.m_timeFunc == null)
        return true;
      uint num = this.m_timeFunc();
      uint time = this.m_reuseQueue.Peek().Time;
      if (num >= time)
        return (ulong) time + (ulong) this.m_reuseProtectionTime < (ulong) num;
      int count = this.m_reuseQueue.Count;
      for (int index = 0; index < count; ++index)
      {
        SequenceIdGenerator.Item obj = this.m_reuseQueue.Dequeue();
        obj.Time = num;
        this.m_reuseQueue.Enqueue(obj);
      }
      return false;
    }

    public uint NextId()
    {
      using (this.m_lock.Acquire())
        return this.m_reuseQueue.Count > this.m_protecionCount && this.CheckFirstItemTime() ? this.m_reuseQueue.Dequeue().Id : ++this.m_maxId;
    }

    public void Return(uint id)
    {
      using (this.m_lock.Acquire())
        this.m_reuseQueue.Enqueue(new SequenceIdGenerator.Item(id, this.m_timeFunc()));
    }

    private struct Item
    {
      public uint Id;
      public uint Time;

      public Item(uint id, uint time)
      {
        this.Id = id;
        this.Time = time;
      }
    }
  }
}
