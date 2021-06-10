// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyWorldObserver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath.Spatial;

namespace Sandbox.Engine.Physics
{
  internal class MyWorldObserver
  {
    private readonly Dictionary<long, int> m_entityInCluster = new Dictionary<long, int>();
    private readonly Dictionary<int, HashSet<long>> m_clusterReplicablesCount = new Dictionary<int, HashSet<long>>();
    private readonly HashSet<long> m_replicatedEntities = new HashSet<long>();

    internal MyWorldObserver(MyClusterTree clusterTree)
    {
      clusterTree.EntityAdded += new Action<long, int>(this.OnEntityAddedToClusterTree);
      clusterTree.EntityRemoved += new Action<long, int>(this.OnEntityRemovedFromClusterTree);
    }

    internal void CleanUp(MyClusterTree clusterTree)
    {
      if (clusterTree != null)
      {
        clusterTree.EntityAdded -= new Action<long, int>(this.OnEntityAddedToClusterTree);
        clusterTree.EntityRemoved -= new Action<long, int>(this.OnEntityRemovedFromClusterTree);
      }
      this.m_entityInCluster.Clear();
      this.m_clusterReplicablesCount.Clear();
    }

    private void OnEntityAddedToClusterTree(long entityId, int clusterId)
    {
      this.m_entityInCluster[entityId] = clusterId;
      if (!this.m_replicatedEntities.Contains(entityId))
        return;
      this.AddReplicatedEntity(entityId, clusterId);
    }

    private void OnEntityRemovedFromClusterTree(long entityId, int clusterId)
    {
      if (this.m_replicatedEntities.Contains(entityId))
        this.RemoveReplicatedEntity(entityId);
      this.m_entityInCluster.Remove(entityId);
    }

    internal void AddReplicatedEntity(MyEntity entity)
    {
      int clusterId;
      if (this.m_entityInCluster.TryGetValue(entity.EntityId, out clusterId))
        this.AddReplicatedEntity(entity.EntityId, clusterId);
      this.m_replicatedEntities.Add(entity.EntityId);
    }

    private void AddReplicatedEntity(long entityId, int clusterId)
    {
      HashSet<long> longSet;
      if (this.m_clusterReplicablesCount.TryGetValue(clusterId, out longSet))
        longSet.Add(entityId);
      else
        this.m_clusterReplicablesCount[clusterId] = new HashSet<long>()
        {
          entityId
        };
    }

    internal void RemoveReplicatedEntity(MyEntity entity)
    {
      this.RemoveReplicatedEntity(entity.EntityId);
      this.m_replicatedEntities.Remove(entity.EntityId);
    }

    private void RemoveReplicatedEntity(long entityId)
    {
      int key;
      HashSet<long> longSet;
      if (!this.m_entityInCluster.TryGetValue(entityId, out key) || !this.m_clusterReplicablesCount.TryGetValue(key, out longSet))
        return;
      longSet.Remove(entityId);
    }

    internal void RemoveCluster(int clusterId) => this.m_clusterReplicablesCount.Remove(clusterId);

    internal bool IsClusterActive(int clusterId)
    {
      HashSet<long> longSet;
      return this.m_clusterReplicablesCount.TryGetValue(clusterId, out longSet) && longSet.Count > 0;
    }
  }
}
