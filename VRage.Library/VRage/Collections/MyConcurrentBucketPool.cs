// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyConcurrentBucketPool
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Collections
{
  public abstract class MyConcurrentBucketPool
  {
    public static bool EnablePooling = true;
    private static List<MyConcurrentBucketPool> m_poolsForDispose = new List<MyConcurrentBucketPool>();

    public static void OnExit()
    {
      lock (MyConcurrentBucketPool.m_poolsForDispose)
      {
        foreach (MyConcurrentBucketPool concurrentBucketPool in MyConcurrentBucketPool.m_poolsForDispose)
          concurrentBucketPool.DisposeInternal();
        MyConcurrentBucketPool.m_poolsForDispose.Clear();
      }
    }

    protected MyConcurrentBucketPool(bool requiresDispose)
    {
      if (!requiresDispose)
        return;
      lock (MyConcurrentBucketPool.m_poolsForDispose)
        MyConcurrentBucketPool.m_poolsForDispose.Add(this);
    }

    protected abstract void DisposeInternal();
  }
}
