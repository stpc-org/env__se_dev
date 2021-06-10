// Decompiled with JetBrains decompiler
// Type: VRage.Library.NativeArrayAllocator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using VRage.Collections;
using VRage.Library.Memory;

namespace VRage.Library
{
  public class NativeArrayAllocator : IMyElementAllocator<NativeArray>
  {
    private readonly MyMemorySystem m_memoryTracker;

    public NativeArrayAllocator(MyMemorySystem memoryTracker) => this.m_memoryTracker = memoryTracker;

    public bool ExplicitlyDisposeAllElements => true;

    NativeArray IMyElementAllocator<NativeArray>.Allocate(
      int size)
    {
      return (NativeArray) new NativeArrayAllocator.NativeArrayImpl(size, this.m_memoryTracker);
    }

    public NativeArray Allocate(int size)
    {
      NativeArray instance = ((IMyElementAllocator<NativeArray>) this).Allocate(size);
      ((IMyElementAllocator<NativeArray>) this).Init(instance);
      return instance;
    }

    public void Dispose(NativeArray instance) => instance.Dispose();

    void IMyElementAllocator<NativeArray>.Init(NativeArray instance)
    {
    }

    public int GetBytes(NativeArray instance) => instance.Size;

    public int GetBucketId(NativeArray instance) => instance.Size;

    private class NativeArrayImpl : NativeArray
    {
      private MyMemorySystem.AllocationRecord m_allocationRecord;

      public NativeArrayImpl(int size, MyMemorySystem memorySystem)
        : base(size)
        => this.m_allocationRecord = memorySystem.RegisterAllocation("NativeArray", (long) size);

      public override void Dispose()
      {
        base.Dispose();
        this.m_allocationRecord.Dispose();
      }
    }
  }
}
