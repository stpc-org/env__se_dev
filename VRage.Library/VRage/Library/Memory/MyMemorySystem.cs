// Decompiled with JetBrains decompiler
// Type: VRage.Library.Memory.MyMemorySystem
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Library.Collections;

namespace VRage.Library.Memory
{
  public class MyMemorySystem
  {
    private const int ID_BITS = 24;
    private const int CHECKSUM_BITS = 8;
    private const int CHECKSUM_SHIFT = 24;
    private const int ID_MAX = 16777215;
    private const int CHECKSUM_MAX = 255;
    private readonly string m_systemName;
    private readonly MyMemorySystem m_parent;
    private List<MyMemorySystem> m_subsystems;
    private ushort m_refId;
    private readonly MyFreeList<MyMemorySystem.AllocationInfo> m_allocatedMemory;
    private long m_thisSystemMemory;
    private long m_totalMemoryCache;

    private MyMemorySystem(MyMemorySystem parent, string systemName)
    {
      this.m_parent = parent;
      this.m_systemName = systemName;
      this.m_refId = (ushort) 1;
      this.m_allocatedMemory = new MyFreeList<MyMemorySystem.AllocationInfo>(100);
    }

    public static MyMemorySystem CreateRootMemorySystem(string systemName) => new MyMemorySystem((MyMemorySystem) null, systemName);

    public MyMemorySystem RegisterSubsystem(string systemName)
    {
      MyMemorySystem myMemorySystem = new MyMemorySystem(this, systemName);
      if (this.m_subsystems == null)
        Interlocked.CompareExchange<List<MyMemorySystem>>(ref this.m_subsystems, new List<MyMemorySystem>(), (List<MyMemorySystem>) null);
      lock (this.m_subsystems)
        this.m_subsystems.Add(myMemorySystem);
      return myMemorySystem;
    }

    public void LogMemoryStats<TLogger>(ref TLogger logger) where TLogger : struct, MyMemoryTracker.ILogger
    {
      logger.BeginSystem(this.m_systemName);
      if (this.m_subsystems != null)
      {
        lock (this.m_subsystems)
        {
          foreach (MyMemorySystem subsystem in this.m_subsystems)
            subsystem.LogMemoryStats<TLogger>(ref logger);
        }
      }
      ref TLogger local = ref logger;
      long totalMemory = this.GetTotalMemory();
      MyFreeList<MyMemorySystem.AllocationInfo> allocatedMemory = this.m_allocatedMemory;
      int totalAllocations = allocatedMemory != null ? allocatedMemory.Count : 0;
      local.EndSystem(totalMemory, totalAllocations);
    }

    public MyMemorySystem.AllocationRecord RegisterAllocation(
      string debugName,
      long size)
    {
      lock (this.m_allocatedMemory)
      {
        int num1 = this.m_allocatedMemory.Allocate();
        if (num1 > 16777215)
        {
          this.m_allocatedMemory.Free(num1);
          return new MyMemorySystem.AllocationRecord();
        }
        ushort num2 = this.m_refId++;
        if (this.m_refId >= (ushort) byte.MaxValue)
          this.m_refId = (ushort) 1;
        int allocationId = (int) num2 << 24 | num1;
        this.m_allocatedMemory[num1] = new MyMemorySystem.AllocationInfo()
        {
          DebugName = debugName,
          RefId = num2,
          Size = size
        };
        Interlocked.Add(ref this.m_thisSystemMemory, size);
        this.InvalidateTotalMemoryCache();
        return new MyMemorySystem.AllocationRecord(allocationId, this);
      }
    }

    private void FreeAllocation(int allocationId)
    {
      int num1 = allocationId & 16777215;
      int num2 = allocationId >> 24 & (int) byte.MaxValue;
      lock (this.m_allocatedMemory)
      {
        if (num1 < 0 || num1 >= this.m_allocatedMemory.Capacity)
          return;
        MyMemorySystem.AllocationInfo allocationInfo = this.m_allocatedMemory[num1];
        if ((int) allocationInfo.RefId != num2)
          return;
        Interlocked.Add(ref this.m_thisSystemMemory, -allocationInfo.Size);
        this.InvalidateTotalMemoryCache();
        this.m_allocatedMemory.Free(num1);
      }
    }

    private void InvalidateTotalMemoryCache()
    {
      this.m_totalMemoryCache = -1L;
      this.m_parent?.InvalidateTotalMemoryCache();
    }

    public long GetTotalMemory()
    {
      long num = this.m_totalMemoryCache;
      if (num < 0L)
      {
        Thread.MemoryBarrier();
        num = this.m_thisSystemMemory;
        if (this.m_subsystems != null)
        {
          lock (this.m_subsystems)
          {
            foreach (MyMemorySystem subsystem in this.m_subsystems)
              num += subsystem.GetTotalMemory();
          }
        }
        this.m_totalMemoryCache = num;
      }
      return num;
    }

    private struct AllocationInfo
    {
      public ushort RefId;
      public long Size;
      public string DebugName;
    }

    public struct AllocationRecord : IDisposable
    {
      private int m_allocationId;
      private MyMemorySystem m_memorySystem;

      public AllocationRecord(int allocationId, MyMemorySystem memorySystem)
      {
        this.m_allocationId = allocationId;
        this.m_memorySystem = memorySystem;
      }

      public void Dispose()
      {
        int allocationId = Interlocked.Exchange(ref this.m_allocationId, 0);
        if (allocationId == 0)
          return;
        this.m_memorySystem.FreeAllocation(allocationId);
        this.m_memorySystem = (MyMemorySystem) null;
      }
    }
  }
}
