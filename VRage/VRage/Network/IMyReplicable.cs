// Decompiled with JetBrains decompiler
// Type: VRage.Network.IMyReplicable
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Replication;
using VRageMath;

namespace VRage.Network
{
  public interface IMyReplicable : IMyNetObject, IMyEventOwner
  {
    bool HasToBeChild { get; }

    bool IsSpatial { get; }

    bool PriorityUpdate { get; }

    bool IncludeInIslands { get; }

    int? PCU { get; }

    string InstanceName { get; }

    bool ShouldReplicate(MyClientInfo client);

    IMyReplicable GetParent();

    bool OnSave(BitStream stream, Endpoint clientEndpoint);

    void OnLoad(BitStream stream, Action<bool> loadingDoneHandler);

    void Reload(Action<bool> loadingDoneHandler);

    void OnDestroyClient();

    void OnRemovedFromReplication();

    void GetStateGroups(List<IMyStateGroup> resultList);

    bool IsReadyForReplication { get; }

    Dictionary<IMyReplicable, Action> ReadyForReplicationAction { get; }

    BoundingBoxD GetAABB();

    Action<IMyReplicable> OnAABBChanged { get; set; }

    HashSet<IMyReplicable> GetDependencies(bool forPlayer);

    HashSet<IMyReplicable> GetCriticalDependencies();

    HashSet<IMyReplicable> GetPhysicalDependencies(
      MyTimeSpan timeStamp,
      MyReplicablesBase replicables);

    bool CheckConsistency();

    ValidationResult HasRights(EndpointId client, ValidationType validationFlags);

    void OnReplication();

    void OnUnreplication();
  }
}
