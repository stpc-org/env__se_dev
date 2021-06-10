// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyReplicablesHierarchy
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
  public class MyReplicablesHierarchy : MyReplicablesBase
  {
    public MyReplicablesHierarchy(Thread mainThread)
      : base(mainThread)
    {
    }

    public override void IterateRoots(Action<IMyReplicable> p)
    {
    }

    public override void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list)
    {
    }

    protected override void AddRoot(IMyReplicable replicable)
    {
    }

    protected override void RemoveRoot(IMyReplicable replicable)
    {
    }
  }
}
