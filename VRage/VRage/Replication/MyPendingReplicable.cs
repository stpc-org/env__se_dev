// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyPendingReplicable
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Network;

namespace VRage.Replication
{
  public class MyPendingReplicable
  {
    public readonly List<NetworkId> StateGroupIds = new List<NetworkId>();
    public Dictionary<NetworkId, MyPendingReplicable> DependentReplicables;
    public IMyReplicable Replicable;
    public bool IsStreaming;
    public NetworkId StreamingGroupId;
    public NetworkId ParentID;
  }
}
