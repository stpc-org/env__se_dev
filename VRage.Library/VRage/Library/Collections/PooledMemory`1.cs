// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.PooledMemory`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;

namespace VRage.Library.Collections
{
  [DebuggerDisplay("{ToString(),raw}")]
  [DebuggerTypeProxy(typeof (PooledMemory<>.DebugView))]
  public readonly struct PooledMemory<T>
  {
    internal readonly T[] Array;
    public readonly int Length;

    public System.Span<T> Span => new System.Span<T>(this.Array, 0, this.Length);

    public bool IsEmpty => this.Length == 0;

    internal PooledMemory(T[] array, int length)
    {
      this.Array = array;
      this.Length = length;
    }

    public ref T this[int index]
    {
      get
      {
        if (index < 0 || index >= this.Length)
          throw new ArgumentOutOfRangeException();
        return ref this.Array[index];
      }
    }

    public static implicit operator Memory<T>(PooledMemory<T> pooled) => new Memory<T>(pooled.Array, 0, pooled.Length);

    public static implicit operator ReadOnlyMemory<T>(PooledMemory<T> pooled) => new ReadOnlyMemory<T>(pooled.Array, 0, pooled.Length);

    public static implicit operator System.Span<T>(PooledMemory<T> pooled) => pooled.Span;

    public static implicit operator ReadOnlySpan<T>(PooledMemory<T> pooled) => (ReadOnlySpan<T>) pooled.Span;

    public override string ToString() => typeof (T) != typeof (char) ? string.Format("PooledMemory<{0}>[{1}]", (object) typeof (T).Name, (object) this.Length) : this.Span.ToString();

    private sealed class DebugView
    {
      private readonly T[] _array;

      public DebugView(PooledMemory<T> memory) => this._array = memory.Span.ToArray();

      [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
      public T[] Items => this._array;
    }
  }
}
