// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyReplicablesLinear
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
  public class MyReplicablesLinear : MyReplicablesBase
  {
    private const int UPDATE_INTERVAL = 60;
    private readonly HashSet<IMyReplicable> m_roots = new HashSet<IMyReplicable>();
    private readonly MyDistributedUpdater<List<IMyReplicable>, IMyReplicable> m_updateList = new MyDistributedUpdater<List<IMyReplicable>, IMyReplicable>(60);

    public MyReplicablesLinear(Thread mainThread)
      : base(mainThread)
    {
    }

    public override void IterateRoots(Action<IMyReplicable> p)
    {
      foreach (IMyReplicable root in this.m_roots)
        p(root);
    }

    public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list) => throw new NotImplementedException();

    protected override void AddRoot(IMyReplicable replicable)
    {
      this.m_roots.Add(replicable);
      this.m_updateList.List.Add(replicable);
    }

    protected override void RemoveRoot(IMyReplicable replicable)
    {
      if (!this.m_roots.Contains(replicable))
        return;
      this.m_roots.Remove(replicable);
      this.m_updateList.List.Remove(replicable);
    }
  }
}
