// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentBufferPool`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Collections
{
  public class MyConcurrentBufferPool<TElement, TAllocator> : MyConcurrentBufferPool<TElement>
    where TElement : class
    where TAllocator : IMyElementAllocator<TElement>, new()
  {
    public MyConcurrentBufferPool(string debugName)
      : base(debugName, (IMyElementAllocator<TElement>) new TAllocator())
    {
    }
  }
}
