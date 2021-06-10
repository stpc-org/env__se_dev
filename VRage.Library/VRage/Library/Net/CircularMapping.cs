// Decompiled with JetBrains decompiler
// Type: VRage.Library.Net.CircularMapping
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage.Library.Net
{
  public struct CircularMapping
  {
    public int Head;
    public int Tail;
    public int Capacity;
    public int ActiveLength;

    public CircularMapping(int capacity)
    {
      this.Head = this.Tail = 0;
      this.Capacity = capacity;
      this.ActiveLength = 0;
    }

    public CircularMapping(int capacity, int head, int tail, bool empty = false)
    {
      if (empty && head != tail)
        throw new InvalidOperationException("Empty flag should only be used to disambiguate state when head and tail are the same.");
      this.Head = head;
      this.Tail = tail;
      this.Capacity = capacity;
      this.ActiveLength = empty ? 0 : CircularMapping.Distance(this.Head, this.Tail, this.Capacity);
    }

    public void ResizeBuffer<TBuffer>(
      int newLength,
      TBuffer original,
      TBuffer resized,
      CircularMapping.Copy<TBuffer> copyFunction)
    {
      if (newLength < this.ActiveLength)
        throw new InvalidOperationException("New length would be too small to encompass the active segment.");
      if (this.ActiveLength > 0)
      {
        if (this.Head < this.Tail)
        {
          copyFunction(original, this.Head, resized, 0, this.ActiveLength);
        }
        else
        {
          copyFunction(original, this.Head, resized, 0, this.Capacity - this.Head);
          copyFunction(original, 0, resized, this.Capacity - this.Head, this.Tail);
        }
      }
      this.Head = 0;
      this.Tail = this.ActiveLength;
      this.Capacity = newLength;
    }

    public void Resize(int newLength)
    {
      if (newLength < this.ActiveLength)
        throw new InvalidOperationException("New length would be too small to encompass the active segment.");
      this.Head = 0;
      this.Tail = this.ActiveLength;
      this.Capacity = newLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AdvanceHead(int amount = 1)
    {
      if (amount < 0 || amount > this.ActiveLength)
        throw new ArgumentOutOfRangeException(nameof (amount));
      this.Head = this.Advance(this.Head, amount);
      this.ActiveLength -= amount;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AdvanceTail(int amount = 1)
    {
      if (amount < 0 || this.ActiveLength + amount > this.Capacity)
        throw new ArgumentOutOfRangeException(nameof (amount));
      this.Tail = this.Advance(this.Tail, amount);
      this.ActiveLength += amount;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Advance(int index)
    {
      ++index;
      if (index == this.Capacity)
        index = 0;
      return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Advance(int index, int amount)
    {
      if (amount > this.Capacity)
        amount %= this.Capacity;
      index += amount;
      if (index >= this.Capacity)
        index -= this.Capacity;
      return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Retract(int index, int amount)
    {
      if (amount > this.Capacity)
        amount %= this.Capacity;
      index -= amount;
      if (index < 0)
        index += this.Capacity;
      return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Retract(int index)
    {
      --index;
      if (index < 0)
        index = this.Capacity - 1;
      return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Distance(int from, int to) => CircularMapping.Distance(from, to, this.Capacity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Distance(int from, int to, int capacity) => to >= from ? to - from : to + capacity - from;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsInRange(int index)
    {
      if (index < 0 || index >= this.Capacity)
        return false;
      return this.ActiveLength == this.Capacity || CircularMapping.IsInRange(this.Head, this.Tail, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRange(int start, int end, int position) => end < start ? position >= start || position < end : position >= start && position < end;

    public CircularMapping.Enumerable EnumerateActiveSegment() => new CircularMapping.Enumerable(this.Head, this.Tail, this.Capacity, this.ActiveLength == 0);

    public CircularMapping.Enumerable EnumerateRange(int start, int end) => new CircularMapping.Enumerable(start, end, this.Capacity, start == end);

    public CircularMapping.Enumerable EnumerateFullRange(int start) => new CircularMapping.Enumerable(start, start, this.Capacity, false);

    public delegate void Copy<TBuffer>(
      TBuffer source,
      int sourceIndex,
      TBuffer destinationBuffer,
      int destinationIndex,
      int length);

    public readonly struct Enumerable : IEnumerable<int>, IEnumerable
    {
      private readonly int m_head;
      private readonly int m_tail;
      private readonly int m_length;
      private readonly bool m_empty;

      public Enumerable(int head, int tail, int length, bool empty)
      {
        this.m_head = head;
        this.m_tail = tail;
        this.m_length = length;
        this.m_empty = empty;
      }

      public CircularMapping.Enumerator GetEnumerator() => new CircularMapping.Enumerator(this.m_head, this.m_tail, this.m_length, this.m_empty);

      IEnumerator<int> IEnumerable<int>.GetEnumerator() => (IEnumerator<int>) this.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    public struct Enumerator : IEnumerator<int>, IEnumerator, IDisposable
    {
      private readonly int m_head;
      private readonly int m_tail;
      private int m_index;
      private int m_length;
      private bool m_first;
      private readonly bool m_notEmpty;

      public Enumerator(int head, int tail, int length, bool empty)
      {
        this.m_index = 0;
        this.m_first = true;
        this.m_head = head;
        this.m_tail = tail;
        this.m_length = length;
        this.m_notEmpty = !empty;
        this.Reset();
      }

      public bool MoveNext()
      {
        if (!this.m_notEmpty)
          return false;
        ++this.m_index;
        if (this.m_index == this.m_length)
          this.m_index = 0;
        if (this.m_first)
          this.m_first = false;
        else if (this.m_index == this.m_tail)
          return false;
        return true;
      }

      public void Reset()
      {
        this.m_first = true;
        this.m_index = this.m_head - 1;
      }

      public int Current => this.m_index;

      object IEnumerator.Current => (object) this.Current;

      public void Dispose() => this.m_length = 0;
    }
  }
}
