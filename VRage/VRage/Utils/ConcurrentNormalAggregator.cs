// Decompiled with JetBrains decompiler
// Type: VRage.Utils.ConcurrentNormalAggregator
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Concurrent;
using System.Threading;
using VRageMath;

namespace VRage.Utils
{
  public class ConcurrentNormalAggregator
  {
    private int m_averageWindowSize;
    private NormalAggregator m_normalAggregator;
    private int m_newNormalsCount;
    private FastResourceLock m_lock = new FastResourceLock();
    private ConcurrentQueue<Vector3> m_newNormals = new ConcurrentQueue<Vector3>();

    public ConcurrentNormalAggregator(int averageWindowSize)
    {
      this.m_averageWindowSize = averageWindowSize;
      this.m_normalAggregator = new NormalAggregator(averageWindowSize);
    }

    public void PushNext(ref Vector3 normal)
    {
      this.m_newNormals.Enqueue(normal);
      if (Interlocked.Increment(ref this.m_newNormalsCount) <= this.m_averageWindowSize)
        return;
      Interlocked.Decrement(ref this.m_newNormalsCount);
      using (this.m_lock.AcquireSharedUsing())
        this.m_newNormals.TryDequeue(out Vector3 _);
    }

    public bool GetAvgNormal(out Vector3 normal)
    {
      using (this.m_lock.AcquireExclusiveUsing())
      {
        while (this.m_newNormals.TryDequeue(out normal))
        {
          Interlocked.Decrement(ref this.m_newNormalsCount);
          this.m_normalAggregator.PushNext(ref normal);
        }
      }
      return this.m_normalAggregator.GetAvgNormal(out normal);
    }

    public bool GetAvgNormalCached(out Vector3 normal) => this.m_normalAggregator.GetAvgNormal(out normal);

    public void Clear()
    {
      using (this.m_lock.AcquireSharedUsing())
        this.m_normalAggregator.Clear();
    }
  }
}
