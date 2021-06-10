// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Storage.MyVoxelOperationsSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Engine.Voxels.Storage
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyVoxelOperationsSessionComponent : MySessionComponentBase
  {
    private const int WaitForLazy = 300;
    public static MyVoxelOperationsSessionComponent Static;
    public static bool EnableCache;
    private readonly MyConcurrentHashSet<MyVoxelOperationsSessionComponent.StorageData> m_storagesWithCache = new MyConcurrentHashSet<MyVoxelOperationsSessionComponent.StorageData>();
    private Action<WorkData> m_flushCachesCallback;
    private Action<WorkData> m_writePendingCallback;
    private volatile int m_scheduledCount;
    private int m_waitForFlush;
    private int m_waitForWrite;
    public bool ShouldWrite = true;
    public bool ShouldFlush = true;

    public IEnumerable<MyStorageBase> QueuedStorages => this.m_storagesWithCache.Select<MyVoxelOperationsSessionComponent.StorageData, MyStorageBase>((Func<MyVoxelOperationsSessionComponent.StorageData, MyStorageBase>) (x => x.Storage));

    public MyVoxelOperationsSessionComponent()
    {
      this.m_flushCachesCallback = new Action<WorkData>(this.FlushCaches);
      this.m_writePendingCallback = new Action<WorkData>(this.WritePending);
    }

    public override void BeforeStart() => MyVoxelOperationsSessionComponent.Static = this;

    public override void UpdateAfterSimulation()
    {
      if (this.m_storagesWithCache.Count == this.m_scheduledCount)
        return;
      ++this.m_waitForWrite;
      if (this.m_waitForWrite > 10)
        this.m_waitForWrite = 0;
      ++this.m_waitForFlush;
      if (this.m_waitForFlush >= 300 && this.ShouldFlush)
      {
        this.m_waitForFlush = 0;
        foreach (MyVoxelOperationsSessionComponent.StorageData storageData in this.m_storagesWithCache)
        {
          if (!storageData.Scheduled && storageData.Storage.HasCachedChunks)
          {
            Interlocked.Increment(ref this.m_scheduledCount);
            storageData.Scheduled = true;
            Parallel.Start(this.m_flushCachesCallback, (Action<WorkData>) null, (WorkData) storageData);
          }
        }
      }
      else
      {
        if (this.m_waitForWrite != 0 || !this.ShouldWrite)
          return;
        foreach (MyVoxelOperationsSessionComponent.StorageData storageData in this.m_storagesWithCache)
        {
          if (!storageData.Scheduled && storageData.Storage.HasPendingWrites)
          {
            Interlocked.Increment(ref this.m_scheduledCount);
            storageData.Scheduled = true;
            Parallel.Start(this.m_writePendingCallback, (Action<WorkData>) null, (WorkData) storageData);
          }
        }
      }
    }

    public void Add(MyStorageBase storage) => this.m_storagesWithCache.Add(new MyVoxelOperationsSessionComponent.StorageData(storage));

    public void WritePending(WorkData data)
    {
      MyVoxelOperationsSessionComponent.StorageData storageData = (MyVoxelOperationsSessionComponent.StorageData) data;
      storageData.Storage.WritePending();
      storageData.Scheduled = false;
      Interlocked.Decrement(ref this.m_scheduledCount);
    }

    public void FlushCaches(WorkData data)
    {
      MyVoxelOperationsSessionComponent.StorageData storageData = (MyVoxelOperationsSessionComponent.StorageData) data;
      storageData.Storage.CleanCachedChunks();
      storageData.Scheduled = false;
      Interlocked.Decrement(ref this.m_scheduledCount);
    }

    public void Remove(MyStorageBase storage) => this.m_storagesWithCache.Remove(new MyVoxelOperationsSessionComponent.StorageData(storage));

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_storagesWithCache.Clear();
      this.m_flushCachesCallback = (Action<WorkData>) null;
      this.m_writePendingCallback = (Action<WorkData>) null;
      MyVoxelOperationsSessionComponent.Static = (MyVoxelOperationsSessionComponent) null;
      this.Session = (IMySession) null;
    }

    private class StorageData : WorkData, IEquatable<MyVoxelOperationsSessionComponent.StorageData>
    {
      public readonly MyStorageBase Storage;
      public bool Scheduled;

      public StorageData(MyStorageBase storage) => this.Storage = storage;

      public bool Equals(
        MyVoxelOperationsSessionComponent.StorageData other)
      {
        return this.Storage == other.Storage;
      }

      public override int GetHashCode() => this.Storage.GetHashCode();
    }
  }
}
