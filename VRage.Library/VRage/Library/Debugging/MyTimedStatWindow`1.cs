// Decompiled with JetBrains decompiler
// Type: VRage.Library.Debugging.MyTimedStatWindow`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;

namespace VRage.Library.Debugging
{
  public class MyTimedStatWindow<TStats> : MyTimedStatWindow, IEnumerable<TStats>, IEnumerable
  {
    private readonly MyQueue<MyTimedStatWindow<TStats>.Frame> m_frames;
    private readonly Stopwatch m_timer;
    private readonly MyTimedStatWindow.IStatArithmetic<TStats> m_arithmetic;
    private TStats m_currentTotal;
    public readonly TimeSpan MaxTime;

    public TStats Total
    {
      get
      {
        TStats result;
        this.m_arithmetic.Add(in this.m_currentTotal, in this.Current, out result);
        return result;
      }
    }

    public ref TStats Current => ref this.m_frames[this.m_frames.Count - 1].Data;

    public MyTimedStatWindow(
      TimeSpan maxTime,
      Func<TStats, TStats, TStats> accumulator,
      Func<TStats, TStats, TStats> subtractor)
      : this(maxTime, (MyTimedStatWindow.IStatArithmetic<TStats>) new MyTimedStatWindow<TStats>.FuncArithmetic(accumulator, subtractor))
    {
    }

    public MyTimedStatWindow(
      TimeSpan maxTime,
      MyTimedStatWindow.IStatArithmetic<TStats> arithmetic)
    {
      this.MaxTime = maxTime;
      this.m_arithmetic = arithmetic;
      this.m_frames = new MyQueue<MyTimedStatWindow<TStats>.Frame>();
      this.m_timer = Stopwatch.StartNew();
      this.m_frames.Enqueue(new MyTimedStatWindow<TStats>.Frame()
      {
        Time = this.m_timer.Elapsed
      });
    }

    public void Advance()
    {
      TimeSpan elapsed = this.m_timer.Elapsed;
      while (this.m_frames.Count > 0 && elapsed - this.m_frames[0].Time > this.MaxTime)
      {
        TStats rhs = this.m_frames.Dequeue().Data;
        if (this.m_frames.Count > 0)
          this.m_arithmetic.Subtract(in this.m_currentTotal, in rhs, out this.m_currentTotal);
      }
      if (this.m_frames.Count > 0)
        this.m_arithmetic.Add(in this.m_currentTotal, in this.Current, out this.m_currentTotal);
      this.m_frames.Enqueue(new MyTimedStatWindow<TStats>.Frame()
      {
        Time = elapsed
      });
    }

    public IEnumerator<TStats> GetEnumerator()
    {
      foreach (MyTimedStatWindow<TStats>.Frame frame in this.m_frames)
        yield return frame.Data;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private struct Frame
    {
      public TimeSpan Time;
      public TStats Data;
    }

    private class FuncArithmetic : MyTimedStatWindow.IStatArithmetic<TStats>
    {
      private readonly Func<TStats, TStats, TStats> m_accumulator;
      private readonly Func<TStats, TStats, TStats> m_subtractor;

      public FuncArithmetic(
        Func<TStats, TStats, TStats> accumulator,
        Func<TStats, TStats, TStats> subtractor)
      {
        this.m_accumulator = accumulator;
        this.m_subtractor = subtractor;
      }

      public void Add(in TStats lhs, in TStats rhs, out TStats result) => result = this.m_accumulator(lhs, rhs);

      public void Subtract(in TStats lhs, in TStats rhs, out TStats result) => result = this.m_subtractor(lhs, rhs);

      void MyTimedStatWindow.IStatArithmetic<TStats>.Add(
        in TStats lhs,
        in TStats rhs,
        out TStats result)
      {
        this.Add(in lhs, in rhs, out result);
      }

      void MyTimedStatWindow.IStatArithmetic<TStats>.Subtract(
        in TStats lhs,
        in TStats rhs,
        out TStats result)
      {
        this.Subtract(in lhs, in rhs, out result);
      }
    }
  }
}
