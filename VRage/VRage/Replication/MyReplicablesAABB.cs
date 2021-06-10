// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyReplicablesAABB
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
  public class MyReplicablesAABB : MyReplicablesBase
  {
    private readonly MyDynamicAABBTreeD m_rootsAABB = new MyDynamicAABBTreeD(Vector3D.One);
    private readonly HashSet<IMyReplicable> m_roots = new HashSet<IMyReplicable>();
    private readonly CacheList<IMyReplicable> m_tmp = new CacheList<IMyReplicable>();
    private readonly Dictionary<IMyReplicable, int> m_proxies = new Dictionary<IMyReplicable, int>();

    public MyReplicablesAABB(Thread mainThread)
      : base(mainThread)
    {
    }

    public override void IterateRoots(Action<IMyReplicable> p)
    {
      using (this.m_tmp)
      {
        this.m_rootsAABB.GetAll<IMyReplicable>((List<IMyReplicable>) this.m_tmp, false);
        foreach (IMyReplicable myReplicable in (List<IMyReplicable>) this.m_tmp)
          p(myReplicable);
      }
    }

    public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list) => this.m_rootsAABB.OverlapAllBoundingBox<IMyReplicable>(ref aabb, list);

    protected override void AddRoot(IMyReplicable replicable)
    {
      this.m_roots.Add(replicable);
      if (!replicable.IsSpatial)
        return;
      BoundingBoxD aabb = replicable.GetAABB();
      this.m_proxies.Add(replicable, this.m_rootsAABB.AddProxy(ref aabb, (object) replicable, 0U));
      replicable.OnAABBChanged += new Action<IMyReplicable>(this.OnRootMoved);
    }

    private void OnRootMoved(IMyReplicable replicable)
    {
      BoundingBoxD aabb = replicable.GetAABB();
      this.m_rootsAABB.MoveProxy(this.m_proxies[replicable], ref aabb, Vector3D.One);
    }

    protected override void RemoveRoot(IMyReplicable replicable)
    {
      if (!this.m_roots.Contains(replicable))
        return;
      this.m_roots.Remove(replicable);
      if (!this.m_proxies.ContainsKey(replicable))
        return;
      replicable.OnAABBChanged -= new Action<IMyReplicable>(this.OnRootMoved);
      this.m_rootsAABB.RemoveProxy(this.m_proxies[replicable]);
      this.m_proxies.Remove(replicable);
    }

    protected override void AddChild(IMyReplicable replicable, IMyReplicable parent)
    {
      base.AddChild(replicable, parent);
      if (!replicable.IsSpatial)
        return;
      BoundingBoxD aabb = replicable.GetAABB();
      this.m_proxies.Add(replicable, this.m_rootsAABB.AddProxy(ref aabb, (object) replicable, 0U));
      replicable.OnAABBChanged += new Action<IMyReplicable>(this.OnRootMoved);
    }

    protected override void RemoveChild(IMyReplicable replicable, IMyReplicable parent)
    {
      base.RemoveChild(replicable, parent);
      if (!this.m_proxies.ContainsKey(replicable))
        return;
      replicable.OnAABBChanged -= new Action<IMyReplicable>(this.OnRootMoved);
      this.m_rootsAABB.RemoveProxy(this.m_proxies[replicable]);
      this.m_proxies.Remove(replicable);
    }
  }
}
