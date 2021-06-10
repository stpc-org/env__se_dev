// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentBufferPool`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Collections
{
  public class MyConcurrentBufferPool<TElement> : MyConcurrentBucketPool<TElement>
    where TElement : class
  {
    public MyConcurrentBufferPool(string debugName, IMyElementAllocator<TElement> allocator)
      : base(debugName, allocator)
    {
    }

    public new TElement Get(int bucketId) => base.Get(MyConcurrentBufferPool<TElement>.GetNearestBiggerPowerOfTwo(bucketId));

    private static int GetNearestBiggerPowerOfTwo(int v)
    {
      --v;
      v |= v >> 1;
      v |= v >> 2;
      v |= v >> 4;
      v |= v >> 8;
      v |= v >> 16;
      ++v;
      return v;
    }
  }
}
