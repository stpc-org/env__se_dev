// Decompiled with JetBrains decompiler
// Type: VRage.Library.NativeArray
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VRage.Library
{
  public abstract class NativeArray : IDisposable
  {
    private IntPtr m_ptr;
    public readonly int Size;

    public bool IsDisposed => this.m_ptr == IntPtr.Zero;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IntPtr Ptr => this.m_ptr;

    protected NativeArray(int size)
    {
      this.Size = size;
      this.m_ptr = Marshal.AllocHGlobal(size);
    }

    [Conditional("DEBUG")]
    public void CheckDisposed()
    {
    }

    [Conditional("DEBUG")]
    public void UpdateAllocationTrace()
    {
    }

    public unsafe Span<T> AsSpan<T>(int length) => length * Unsafe.SizeOf<T>() <= this.Size ? new Span<T>(this.Ptr.ToPointer(), length) : throw new ArgumentException("Requested length is too long for the native array.");

    public unsafe Span<byte> AsSpan(int length = -1)
    {
      if (length == -1)
        length = this.Size;
      return length <= this.Size ? new Span<byte>(this.Ptr.ToPointer(), length) : throw new ArgumentException("Requested length is too long for the native array.");
    }

    public virtual void Dispose()
    {
      Marshal.FreeHGlobal(this.Ptr);
      this.m_ptr = IntPtr.Zero;
    }
  }
}
