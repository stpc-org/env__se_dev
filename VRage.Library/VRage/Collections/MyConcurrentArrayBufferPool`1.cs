// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentArrayBufferPool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Collections
{
  public class MyConcurrentArrayBufferPool<TElement> : MyConcurrentBufferPool<TElement[], MyConcurrentArrayBufferPool<TElement>.ArrayAllocator>
  {
    public MyConcurrentArrayBufferPool(string debugName)
      : base(debugName)
    {
    }

    private static int SizeOf<T>() => !typeof (T).IsValueType ? IntPtr.Size : TypeExtensions.SizeOf<T>();

    public class ArrayAllocator : IMyElementAllocator<TElement[]>
    {
      private static readonly int ElementSize = MyConcurrentArrayBufferPool<TElement>.SizeOf<TElement>();

      public bool ExplicitlyDisposeAllElements => false;

      public TElement[] Allocate(int size) => new TElement[size];

      public void Init(TElement[] item)
      {
      }

      public int GetBytes(TElement[] instance) => MyConcurrentArrayBufferPool<TElement>.ArrayAllocator.ElementSize * instance.Length;

      public int GetBucketId(TElement[] instance) => instance.Length;

      public void Dispose(TElement[] instance)
      {
      }
    }
  }
}
