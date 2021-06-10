// Decompiled with JetBrains decompiler
// Type: VRage.FastResourceLockExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace VRage
{
  public static class FastResourceLockExtensions
  {
    [DebuggerStepThrough]
    public static FastResourceLockExtensions.MySharedLock AcquireSharedUsing(
      this FastResourceLock lockObject)
    {
      return new FastResourceLockExtensions.MySharedLock(lockObject);
    }

    [DebuggerStepThrough]
    public static FastResourceLockExtensions.MyExclusiveLock AcquireExclusiveUsing(
      this FastResourceLock lockObject)
    {
      return new FastResourceLockExtensions.MyExclusiveLock(lockObject);
    }

    [DebuggerStepThrough]
    public static FastResourceLockExtensions.MyOwnedExclusiveLock AcquireExclusiveRecursiveUsing(
      this FastResourceLock lockObject,
      Ref<int> ownerField)
    {
      return lockObject.IsOwnedByCurrentThread(ownerField) ? new FastResourceLockExtensions.MyOwnedExclusiveLock() : new FastResourceLockExtensions.MyOwnedExclusiveLock(lockObject, ownerField);
    }

    [DebuggerStepThrough]
    public static FastResourceLockExtensions.MySharedLock AcquireSharedRecursiveUsing(
      this FastResourceLock lockObject,
      Ref<int> ownerField)
    {
      return lockObject.IsOwnedByCurrentThread(ownerField) ? new FastResourceLockExtensions.MySharedLock() : new FastResourceLockExtensions.MySharedLock(lockObject);
    }

    [DebuggerStepThrough]
    public static bool IsOwnedByCurrentThread(this FastResourceLock lockObject, Ref<int> ownerField) => lockObject.Owned && ownerField.Value == Thread.CurrentThread.ManagedThreadId;

    public struct MySharedLock : IDisposable
    {
      private readonly FastResourceLock m_lockObject;

      [DebuggerStepThrough]
      public MySharedLock(FastResourceLock lockObject)
      {
        this.m_lockObject = lockObject;
        this.m_lockObject.AcquireShared();
      }

      public void Dispose()
      {
        if (this.m_lockObject == null)
          return;
        this.m_lockObject.ReleaseShared();
      }
    }

    public struct MyExclusiveLock : IDisposable
    {
      private readonly FastResourceLock m_lockObject;

      [DebuggerStepThrough]
      public MyExclusiveLock(FastResourceLock lockObject)
      {
        this.m_lockObject = lockObject;
        this.m_lockObject.AcquireExclusive();
      }

      public void Dispose() => this.m_lockObject.ReleaseExclusive();
    }

    public struct MyOwnedExclusiveLock : IDisposable
    {
      private Ref<int> m_owner;
      private FastResourceLockExtensions.MyExclusiveLock m_core;

      public MyOwnedExclusiveLock(FastResourceLock lockObject, Ref<int> ownerField)
      {
        this.m_owner = ownerField;
        this.m_core = new FastResourceLockExtensions.MyExclusiveLock(lockObject);
        this.m_owner.Value = Thread.CurrentThread.ManagedThreadId;
      }

      public void Dispose()
      {
        if (this.m_owner == null)
          return;
        this.m_owner.Value = -1;
        this.m_core.Dispose();
      }
    }
  }
}
