// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyDebugHitCounter
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Utils
{
  public class MyDebugHitCounter : IEnumerable<MyDebugHitCounter.Sample>, IEnumerable
  {
    public readonly MyQueue<MyDebugHitCounter.Sample> History;
    private MyDebugHitCounter.Sample current;
    private readonly uint m_sampleCycle;
    private readonly uint m_historyLength;
    private SpinLockRef m_lock = new SpinLockRef();

    public MyDebugHitCounter(uint cycleSize = 100000)
    {
      this.m_sampleCycle = cycleSize;
      this.m_historyLength = 10U;
      this.History = new MyQueue<MyDebugHitCounter.Sample>((int) this.m_historyLength);
    }

    public float CurrentHitRatio
    {
      get
      {
        using (this.m_lock.Acquire())
          return this.current.Value;
      }
    }

    public float LastCycleHitRatio
    {
      get
      {
        using (this.m_lock.Acquire())
          return this.History.Count > 1 ? this.History[1].Value : 0.0f;
      }
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public void Hit()
    {
      using (this.m_lock.Acquire())
        ++this.current.Count;
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public void Miss()
    {
      using (this.m_lock.Acquire())
      {
        ++this.current.Cycle;
        if ((int) this.current.Cycle != (int) this.m_sampleCycle)
          return;
        this.Cycle();
      }
    }

    public void Cycle()
    {
      using (this.m_lock.Acquire())
      {
        if ((long) this.History.Count >= (long) this.m_historyLength)
          this.History.Dequeue();
        this.History.Enqueue(this.current);
        this.current = new MyDebugHitCounter.Sample();
      }
    }

    public float ValueAndCycle()
    {
      this.Cycle();
      return this.LastCycleHitRatio;
    }

    public void CycleWork()
    {
      if (this.current.Count <= 0U)
        return;
      this.Cycle();
    }

    private IEnumerator<MyDebugHitCounter.Sample> GetEnumeratorInternal()
    {
      yield return this.current;
      foreach (MyDebugHitCounter.Sample sample in this.History)
        yield return sample;
    }

    public ConcurrentEnumerator<SpinLockRef.Token, MyDebugHitCounter.Sample, IEnumerator<MyDebugHitCounter.Sample>> GetEnumerator() => ConcurrentEnumerator.Create<SpinLockRef.Token, MyDebugHitCounter.Sample, IEnumerator<MyDebugHitCounter.Sample>>(this.m_lock.Acquire(), this.GetEnumeratorInternal());

    IEnumerator<MyDebugHitCounter.Sample> IEnumerable<MyDebugHitCounter.Sample>.GetEnumerator() => (IEnumerator<MyDebugHitCounter.Sample>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Sample
    {
      public uint Count;
      public uint Cycle;

      public float Value => (float) this.Count / (float) this.Cycle;

      public override string ToString() => this.Value.ToString();
    }
  }
}
