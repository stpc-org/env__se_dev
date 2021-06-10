// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyShipMiningSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyShipMiningSystem : MyUpdateableGridSystem
  {
    private readonly MyDynamicAABBTree m_drillTree = new MyDynamicAABBTree(Vector3.Zero);
    private readonly HashSet<MyShipMiningSystem.DrillCluster> m_dirtyClusters = new HashSet<MyShipMiningSystem.DrillCluster>();
    public static bool DebugDisable = false;
    public static bool DebugDrawClusters = false;
    public static bool DebugDrawCutOuts = false;
    public static float DebugDrawCutOutPermanence = 15f;
    public static float DebugOperationDelay = 0.0f;
    private readonly MyList<MyShipMiningSystem.CutOutRequest> m_pendingCutOuts = new MyList<MyShipMiningSystem.CutOutRequest>();
    private readonly HashSet<(MyShipMiningSystem.DrillCluster, MyVoxelBase)> m_cutOutsPerClusterVoxel = new HashSet<(MyShipMiningSystem.DrillCluster, MyVoxelBase)>();
    private readonly HashSet<MyShipMiningSystem.ClusterCutOut> m_executingCutOuts = new HashSet<MyShipMiningSystem.ClusterCutOut>();
    private const float VOXEL_CONTENT_HACK_MULTIPLIER = 4.62f;
    private readonly MyFreeList<MyShipMiningSystem.DrillData> m_drills = new MyFreeList<MyShipMiningSystem.DrillData>();
    private readonly CachingHashSet<MyShipDrill> m_drillsForUpdate10 = new CachingHashSet<MyShipDrill>();

    private void AddDrillToCluster(MyShipDrill drill)
    {
      ref MyShipMiningSystem.DrillData local = ref this.m_drills[drill.ShipDrillId];
      BoundingSphere bondingSphere = local.BondingSphere;
      bondingSphere.Radius *= 3f;
      BoundingBox boundingBox = bondingSphere.GetBoundingBox();
      ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode> nodes = this.m_drillTree.Nodes;
      int root = this.m_drillTree.GetRoot();
      bool flag = false;
      MyShipMiningSystem.DrillCluster cc = (MyShipMiningSystem.DrillCluster) null;
      if (root != -1)
      {
        Stack<int> poolObject;
        using (PoolManager.Get<Stack<int>>(out poolObject))
        {
          poolObject.Push(root);
          while (poolObject.Count > 0)
          {
            MyDynamicAABBTree.DynamicTreeNode dynamicTreeNode = nodes[poolObject.Pop()];
            if (!dynamicTreeNode.IsLeaf())
            {
              if (nodes[dynamicTreeNode.Child1].Aabb.Intersects(boundingBox))
                poolObject.Push(dynamicTreeNode.Child1);
              if (dynamicTreeNode.Child1 != -1 && nodes[dynamicTreeNode.Child2].Aabb.Intersects(boundingBox))
                poolObject.Push(dynamicTreeNode.Child2);
            }
            else
            {
              MyShipMiningSystem.DrillCluster cluster = this.m_drills[((MyShipDrill) dynamicTreeNode.UserData).ShipDrillId].Cluster;
              if (cc == null)
                cc = cluster;
              else if (cluster != cc)
              {
                flag = true;
                Mark(cc);
                Mark(cluster);
              }
            }
          }
        }
      }
      if (cc == null)
        cc = new MyShipMiningSystem.DrillCluster();
      cc.AddDrill(drill.ShipDrillId, in local);
      local.TreeProxyId = this.m_drillTree.AddProxy(ref boundingBox, (object) drill, 0U);
      local.Cluster = cc;
      if (!flag)
        return;
      this.Schedule();

      void Mark(MyShipMiningSystem.DrillCluster cc)
      {
        if (cc.NeedsReconstruction)
          return;
        this.m_dirtyClusters.Add(cc);
        cc.NeedsReconstruction = true;
      }
    }

    private void RemoveDrillFromCluster(MyShipDrill drill)
    {
      ref MyShipMiningSystem.DrillData local = ref this.m_drills[drill.ShipDrillId];
      MyShipMiningSystem.DrillCluster cluster = local.Cluster;
      cluster.RemoveDrill(drill.ShipDrillId);
      this.m_drillTree.RemoveProxy(local.TreeProxyId);
      if (cluster.IsEmpty)
      {
        this.m_dirtyClusters.Remove(cluster);
        if (this.m_dirtyClusters.Count != 0)
          return;
        this.DeSchedule();
      }
      else
      {
        this.Schedule();
        this.m_dirtyClusters.Add(cluster);
      }
    }

    private void UpdateClusters()
    {
      List<int> intList = (List<int>) null;
      foreach (MyShipMiningSystem.DrillCluster dirtyCluster in this.m_dirtyClusters)
      {
        if (!dirtyCluster.NeedsReconstruction && !dirtyCluster.RemoveThresholdReached)
        {
          dirtyCluster.UpdateBounds(this);
        }
        else
        {
          intList = intList ?? PoolManager.Get<List<int>>();
          intList.AddRange((IEnumerable<int>) dirtyCluster.Drills);
          foreach (int drill in dirtyCluster.Drills)
            this.m_drills[drill].Cluster = (MyShipMiningSystem.DrillCluster) null;
        }
      }
      this.m_dirtyClusters.Clear();
      if (intList == null)
        return;
      MyUnionFind sets = new MyUnionFind(this.m_drills.UsedLength);
      for (int index = 0; index < intList.Count; ++index)
        this.CollectSiblings(intList[index], sets);
      for (int index = 0; index < intList.Count; ++index)
      {
        int num = intList[index];
        ref MyShipMiningSystem.DrillData local1 = ref this.m_drills[num];
        ref MyShipMiningSystem.DrillData local2 = ref this.m_drills[sets.Find(num)];
        if (local2.Cluster == null)
          local2.Cluster = new MyShipMiningSystem.DrillCluster();
        local2.Cluster.AddDrill(num, in local1);
        local1.Cluster = local2.Cluster;
      }
      PoolManager.Return<List<int>>(ref intList);
    }

    private void CollectSiblings(int drillIndex, MyUnionFind sets)
    {
      BoundingSphere bondingSphere = this.m_drills[drillIndex].BondingSphere;
      bondingSphere.Radius *= 3f;
      BoundingBox boundingBox = bondingSphere.GetBoundingBox();
      ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode> nodes = this.m_drillTree.Nodes;
      int root = this.m_drillTree.GetRoot();
      Stack<int> poolObject;
      using (PoolManager.Get<Stack<int>>(out poolObject))
      {
        poolObject.Push(root);
        while (poolObject.Count > 0)
        {
          MyDynamicAABBTree.DynamicTreeNode dynamicTreeNode = nodes[poolObject.Pop()];
          if (!dynamicTreeNode.IsLeaf())
          {
            if (nodes[dynamicTreeNode.Child1].Aabb.Intersects(boundingBox))
              poolObject.Push(dynamicTreeNode.Child1);
            if (dynamicTreeNode.Child1 != -1 && nodes[dynamicTreeNode.Child2].Aabb.Intersects(boundingBox))
              poolObject.Push(dynamicTreeNode.Child2);
          }
          else
          {
            MyShipDrill userData = (MyShipDrill) dynamicTreeNode.UserData;
            sets.Union(drillIndex, userData.ShipDrillId);
          }
        }
      }
    }

    public MyShipMiningSystem(MyCubeGrid grid)
      : base(grid)
    {
    }

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceAfterSimulation;

    public override bool UpdateInParallel => true;

    protected override void Update()
    {
      if (this.m_dirtyClusters.Count > 0)
        this.UpdateClusters();
      if (this.m_pendingCutOuts.Count <= 0)
        return;
      this.ScheduleCutouts();
    }

    public void DebugDraw()
    {
      if (MyShipMiningSystem.DebugDrawClusters)
      {
        foreach (MyShipMiningSystem.DrillCluster drillCluster in this.m_drillTree.Leaves.Select<MyDynamicAABBTree.DynamicTreeNode, MyShipMiningSystem.DrillCluster>((Func<MyDynamicAABBTree.DynamicTreeNode, MyShipMiningSystem.DrillCluster>) (x => this.GetDrillData((MyShipDrill) x.UserData).Cluster)).Distinct<MyShipMiningSystem.DrillCluster>())
          drillCluster.DebugDraw(this, this.Grid.PositionComp.WorldMatrixRef);
      }
      if (!MyShipMiningSystem.DebugDrawCutOuts)
        return;
      List<MyShipMiningSystem.ClusterCutOut> clusterCutOutList = (List<MyShipMiningSystem.ClusterCutOut>) null;
      foreach (MyShipMiningSystem.ClusterCutOut executingCutOut in this.m_executingCutOuts)
      {
        if (executingCutOut.DebugDraw())
        {
          clusterCutOutList = clusterCutOutList ?? new List<MyShipMiningSystem.ClusterCutOut>();
          clusterCutOutList.Add(executingCutOut);
        }
      }
      if (clusterCutOutList == null)
        return;
      foreach (MyShipMiningSystem.ClusterCutOut clusterCutOut in clusterCutOutList)
        clusterCutOut.Return(this.m_executingCutOuts);
    }

    private void ScheduleCutouts()
    {
      this.m_pendingCutOuts.Sort(MyShipMiningSystem.CutOutRequest.CutOutDataComparer);
      int index = 0;
      while (index < this.m_pendingCutOuts.Count)
      {
        MyShipMiningSystem.CutOutRequest pendingCutOut1 = this.m_pendingCutOuts[index];
        (MyShipMiningSystem.DrillCluster, MyVoxelBase) valueTuple = (pendingCutOut1.Cluster, pendingCutOut1.Voxel);
        int start = index;
        for (; index < this.m_pendingCutOuts.Count; ++index)
        {
          MyShipMiningSystem.CutOutRequest pendingCutOut2 = this.m_pendingCutOuts[index];
          if (pendingCutOut2.Cluster != valueTuple.Item1 || pendingCutOut2.Voxel != valueTuple.Item2)
            break;
        }
        int length = index - start;
        MyShipMiningSystem.ClusterCutOut.GetAndPrepare(this, pendingCutOut1.Cluster, pendingCutOut1.Voxel, this.m_pendingCutOuts.Slice(start, length)).Dispatch(this.m_executingCutOuts);
      }
      this.m_cutOutsPerClusterVoxel.Clear();
      this.m_pendingCutOuts.Clear();
    }

    public static void PerformCutoutClient(
      MyVoxelBase target,
      in MyShipMiningSystem.NetworkCutoutData data)
    {
      MyShipMiningSystem.ClusterCutOut clusterCutOut = PoolManager.Get<MyShipMiningSystem.ClusterCutOut>();
      clusterCutOut.Prepare(in data, target);
      clusterCutOut.Dispatch((HashSet<MyShipMiningSystem.ClusterCutOut>) null);
    }

    public void RegisterDrill(MyShipDrill drill)
    {
      drill.ShipDrillId = this.m_drills.Allocate(new MyShipMiningSystem.DrillData(drill));
      this.AddDrillToCluster(drill);
    }

    public void UnRegisterDrill(MyShipDrill drill)
    {
      this.RemoveDrillFromCluster(drill);
      this.m_drills.Free(drill.ShipDrillId);
      drill.ShipDrillId = -1;
    }

    public void RequestCutOut(
      MyShipDrill drill,
      bool applyDiscardingModifier,
      bool applyDamagedMaterial,
      Vector3D hitPosition,
      MyVoxelBase voxel)
    {
      this.Schedule();
      ref MyShipMiningSystem.DrillData local = ref this.m_drills[drill.ShipDrillId];
      this.m_pendingCutOuts.Add(new MyShipMiningSystem.CutOutRequest(drill, local.Cluster, applyDiscardingModifier, applyDamagedMaterial, hitPosition, voxel));
    }

    private ref readonly MyShipMiningSystem.DrillData GetDrillData(
      MyShipDrill drill)
    {
      if (drill.ShipDrillId != -1)
        return ref this.m_drills[drill.ShipDrillId];
      throw new KeyNotFoundException();
    }

    private ref readonly MyShipMiningSystem.DrillData GetDrillData(int drillId) => ref this.m_drills[drillId];

    public void AddDrillUpdate(MyShipDrill drill)
    {
      if (MyShipMiningSystem.DebugDisable)
      {
        drill.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
      else
      {
        if (!this.m_drillsForUpdate10.Add(drill))
          return;
        MyShipDrill other = (MyShipDrill) null;
        foreach (MyShipDrill myShipDrill in this.m_drillsForUpdate10)
        {
          if (myShipDrill != drill)
          {
            other = myShipDrill;
            break;
          }
        }
        if (other == null)
          return;
        drill.SynchronizeWith(other);
      }
    }

    public void RemoveDrillUpdate(MyShipDrill drill)
    {
      if (MyShipMiningSystem.DebugDisable)
        drill.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
      else
        this.m_drillsForUpdate10.Remove(drill);
    }

    internal void UpdateBeforeSimulation10()
    {
      if (MyShipMiningSystem.DebugDisable)
        return;
      this.m_drillsForUpdate10.ApplyChanges();
      foreach (MyShipDrill myShipDrill in this.m_drillsForUpdate10)
      {
        if (myShipDrill.Closed)
          this.m_drillsForUpdate10.Remove(myShipDrill);
        else
          myShipDrill.UpdateBeforeSimulation10();
      }
    }

    private class DrillCluster
    {
      private readonly HashSet<int> m_drills = new HashSet<int>();
      private BoundingBox m_bounds = BoundingBox.CreateInvalid();
      private BoundingBox m_expandedBounds = BoundingBox.CreateInvalid();
      private bool m_dirtyBounds;
      private int m_removalsSinceLastRebuild;
      public bool NeedsReconstruction;

      public BoundingBox Bounds => this.m_bounds;

      public BoundingBox ExpandedBounds => this.m_expandedBounds;

      public bool RemoveThresholdReached => this.m_removalsSinceLastRebuild >= 10 || this.m_removalsSinceLastRebuild >= this.m_drills.Count;

      public bool IsEmpty => this.m_drills.Count == 0;

      public HashSetReader<int> Drills => (HashSetReader<int>) this.m_drills;

      public void AddDrill(int drillIndex, in MyShipMiningSystem.DrillData data)
      {
        this.m_drills.Add(drillIndex);
        BoundingSphere bondingSphere = data.BondingSphere;
        this.m_bounds.Include(bondingSphere);
        bondingSphere.Radius *= 3f;
        this.m_expandedBounds.Include(bondingSphere);
      }

      public void RemoveDrill(int drillIndex)
      {
        this.m_drills.Remove(drillIndex);
        this.m_dirtyBounds = true;
        ++this.m_removalsSinceLastRebuild;
      }

      public void UpdateBounds(MyShipMiningSystem system)
      {
        if (!this.m_dirtyBounds)
          return;
        this.m_dirtyBounds = false;
        this.m_bounds = BoundingBox.CreateInvalid();
        this.m_expandedBounds = BoundingBox.CreateInvalid();
        foreach (int drill in this.m_drills)
        {
          BoundingSphere bondingSphere = system.GetDrillData(drill).BondingSphere;
          this.m_bounds.Include(bondingSphere);
          bondingSphere.Radius *= 3f;
          this.m_expandedBounds.Include(bondingSphere);
        }
      }

      public void DebugDraw(MyShipMiningSystem system, MatrixD gridMatrix)
      {
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) this.Bounds, gridMatrix), Color.Orange, 0.7f, true, false);
        foreach (int drill in this.m_drills)
        {
          ref readonly MyShipMiningSystem.DrillData local = ref system.GetDrillData(drill);
          Vector3 center = local.BondingSphere.Center;
          Vector3D result;
          Vector3D.Transform(ref center, ref gridMatrix, out result);
          MyRenderProxy.DebugDrawArrow3D(local.Drill.PositionComp.GetPosition(), result, Color.Green);
          MyRenderProxy.DebugDrawSphere(result, local.BondingSphere.Radius, Color.Green);
        }
      }
    }

    [DebuggerDisplay("{DebugText}")]
    private readonly struct CutOutRequest
    {
      public readonly MyShipDrill Drill;
      public readonly bool ApplyDiscardingMultiplier;
      public readonly bool ApplyDamagedMaterial;
      public readonly Vector3D HitPosition;
      public readonly MyVoxelBase Voxel;
      internal readonly MyShipMiningSystem.DrillCluster Cluster;

      public CutOutRequest(
        MyShipDrill drill,
        MyShipMiningSystem.DrillCluster cluster,
        bool applyDiscardingMultiplier,
        bool applyDamagedMaterial,
        Vector3D hitPosition,
        MyVoxelBase voxel)
      {
        this.Drill = drill;
        this.ApplyDiscardingMultiplier = applyDiscardingMultiplier;
        this.ApplyDamagedMaterial = applyDamagedMaterial;
        this.HitPosition = hitPosition;
        this.Voxel = voxel;
        this.Cluster = cluster;
      }

      public static IComparer<MyShipMiningSystem.CutOutRequest> CutOutDataComparer { get; } = (IComparer<MyShipMiningSystem.CutOutRequest>) new MyShipMiningSystem.CutOutRequest.CutOutDataClusteringComparer();

      private string DebugText => string.Format("{0} vs {1}{2}{3}", (object) this.Drill, (object) this.Voxel.DebugName, this.ApplyDiscardingMultiplier ? (object) " Discarding" : (object) "", this.ApplyDamagedMaterial ? (object) " ApplyDamage" : (object) "");

      private sealed class CutOutDataClusteringComparer : IComparer<MyShipMiningSystem.CutOutRequest>
      {
        public int Compare(MyShipMiningSystem.CutOutRequest x, MyShipMiningSystem.CutOutRequest y)
        {
          int num = x.Cluster.GetHashCode().CompareTo(y.Cluster.GetHashCode());
          return num == 0 ? x.Voxel.GetHashCode().CompareTo(y.Voxel.GetHashCode()) : num;
        }
      }
    }

    private class ClusterCutOut : IWork
    {
      private const int ChunkBits = 4;
      private const int ChunkSize = 16;
      private VRage.Game.Voxels.IMyStorage m_storage;
      private MyVoxelBase m_targetVoxel;
      private Matrix m_drillToStorageMatrix;
      private Vector3I m_storageOffset;
      private BoundingBox m_clusterBounds;
      private BoundingBoxI m_totalAffectedRange;
      private bool m_materialsChanged;
      private PooledMemory<MyShipMiningSystem.ClusterCutOut.CutOutData> m_cutOuts;
      private readonly List<(Vector3I Chunk, int CutOut)> m_chunkCutOutPairs = new List<(Vector3I, int)>();
      private MyShipMiningSystem.ClusterCutOut.State m_state;
      private bool IsClient;
      private static readonly IComparer<(Vector3I Chunk, int Drill)> m_sortPerChunk = (IComparer<(Vector3I, int)>) new MyShipMiningSystem.ClusterCutOut.DrillChunkComparer();
      [ThreadStatic]
      private static MyStorageData m_storageData;
      private MatrixD m_debugDrawMatrix;
      private int m_debugChunkProgress;
      private DateTime m_debugFinishedTime;

      public static MyShipMiningSystem.ClusterCutOut GetAndPrepare(
        MyShipMiningSystem system,
        MyShipMiningSystem.DrillCluster cluster,
        MyVoxelBase target,
        Span<MyShipMiningSystem.CutOutRequest> slice)
      {
        MyShipMiningSystem.ClusterCutOut clusterCutOut = PoolManager.Get<MyShipMiningSystem.ClusterCutOut>();
        clusterCutOut.Prepare(system, cluster, target, slice);
        return clusterCutOut;
      }

      private void Prepare(
        MyShipMiningSystem system,
        MyShipMiningSystem.DrillCluster cluster,
        MyVoxelBase target,
        Span<MyShipMiningSystem.CutOutRequest> slice)
      {
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Idle;
        this.IsClient = false;
        this.m_storage = target.Storage;
        this.m_targetVoxel = target;
        MatrixD result;
        MatrixD.Multiply(ref Unsafe.AsRef<MatrixD>(in system.Grid.PositionComp.WorldMatrixRef), ref Unsafe.AsRef<MatrixD>(in target.PositionComp.WorldMatrixInvScaled), out result);
        result.Translation += (Vector3) target.StorageMin + target.SizeInMetresHalf;
        this.m_storageOffset = Vector3I.Floor(Vector3D.Transform((Vector3D) cluster.Bounds.Center, ref result));
        result.Translation -= (Vector3D) this.m_storageOffset;
        this.m_drillToStorageMatrix = (Matrix) ref result;
        this.m_debugDrawMatrix = target.PositionComp.WorldMatrixRef;
        this.m_debugDrawMatrix = MatrixD.CreateTranslation((Vector3) this.m_storageOffset - target.SizeInMetresHalf - (Vector3) target.StorageMin) * this.m_debugDrawMatrix;
        PoolManager.BorrowMemory<MyShipMiningSystem.ClusterCutOut.CutOutData>(slice.Length, out this.m_cutOuts);
        bool flag = false;
        for (int index = 0; index < slice.Length; ++index)
        {
          ref MyShipMiningSystem.CutOutRequest local = ref slice[index];
          this.m_cutOuts[index] = new MyShipMiningSystem.ClusterCutOut.CutOutData(in local, in system.GetDrillData(local.Drill));
          flag |= local.ApplyDiscardingMultiplier;
        }
        this.m_clusterBounds = !flag ? cluster.Bounds : cluster.ExpandedBounds;
        this.m_clusterBounds = this.m_clusterBounds.Transform(this.m_drillToStorageMatrix);
      }

      public void Prepare(in MyShipMiningSystem.NetworkCutoutData data, MyVoxelBase target)
      {
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Idle;
        this.IsClient = true;
        this.m_targetVoxel = target;
        this.m_storage = target.Storage;
        this.m_storageOffset = data.StorageOffset;
        this.m_totalAffectedRange = data.AffectedRange;
        MyShipMiningSystem.NetworkCutoutData.CutOut[] cutOuts = data.CutOuts;
        PoolManager.BorrowMemory<MyShipMiningSystem.ClusterCutOut.CutOutData>(cutOuts.Length, out this.m_cutOuts);
        for (int index = 0; index < cutOuts.Length; ++index)
          this.m_cutOuts[index] = new MyShipMiningSystem.ClusterCutOut.CutOutData(in cutOuts[index], this.m_storageOffset);
      }

      public void Dispatch(
        HashSet<MyShipMiningSystem.ClusterCutOut> operationQueue)
      {
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Queued;
        operationQueue?.Add(this);
        Parallel.Start((IWork) this, (Action) (() => this.Finish(operationQueue)));
      }

      public void DoWork(WorkData workData = null)
      {
        MyShipMiningSystem.ClusterCutOut.m_storageData = MyShipMiningSystem.ClusterCutOut.m_storageData ?? new MyStorageData(MyStorageDataTypeFlags.ContentAndMaterial);
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Preparing;
        BoundingBoxI bounds;
        if (!this.IsClient)
        {
          for (int index = 0; index < this.m_cutOuts.Length; ++index)
          {
            ref MyShipMiningSystem.ClusterCutOut.CutOutData local1 = ref this.m_cutOuts[index];
            ref BoundingSphere local2 = ref local1.SphereInVoxelSpace;
            local2 = local2.Transform(this.m_drillToStorageMatrix);
            local1.StorageBounds = new BoundingBoxI(Vector3I.Floor(local2.Center - local2.Radius) + this.m_storageOffset - 1, Vector3I.Ceiling(local2.Center + local2.Radius) + this.m_storageOffset + 1);
          }
          BoundingBox clusterBounds = this.m_clusterBounds;
          bounds = new BoundingBoxI(Vector3I.Floor(clusterBounds.Min), Vector3I.Ceiling(clusterBounds.Max));
          bounds.Min += this.m_storageOffset - 1;
          bounds.Max += this.m_storageOffset + 1;
          this.m_totalAffectedRange = BoundingBoxI.CreateInvalid();
        }
        else
          bounds = this.m_totalAffectedRange;
        if (bounds.Size.AbsMax() <= 16)
          this.CutSmallArea(in bounds);
        else
          this.CutLargeArea();
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Finished;
        this.m_debugFinishedTime = DateTime.Now;
      }

      private void CutSmallArea(in BoundingBoxI bounds)
      {
        MyShipMiningSystem.ClusterCutOut.m_storageData.Resize(bounds.Size);
        this.m_storage.ReadRange(MyShipMiningSystem.ClusterCutOut.m_storageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, bounds.Min, bounds.Max - 1);
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Executing;
        this.m_debugChunkProgress = 0;
        (bool, bool) changes = (false, false);
        for (int index = 0; index < this.m_cutOuts.Length; ++index)
        {
          ref MyShipMiningSystem.ClusterCutOut.CutOutData local = ref this.m_cutOuts[index];
          BoundingSphere sphereInVoxelSpace = local.SphereInVoxelSpace;
          Vector3I vector3I = this.m_storageOffset - bounds.Min;
          sphereInVoxelSpace.Center += (Vector3) vector3I;
          this.CutOutChunk(sphereInVoxelSpace, MyShipMiningSystem.ClusterCutOut.m_storageData, ref local, ref changes);
          if ((double) MyShipMiningSystem.DebugOperationDelay > 0.0)
            Thread.Sleep((int) ((double) MyShipMiningSystem.DebugOperationDelay * 1000.0));
        }
        this.CommitDataToStorage(in bounds, in changes);
        this.m_materialsChanged = changes.Item2;
      }

      private void CutLargeArea()
      {
        for (int index = 0; index < this.m_cutOuts.Length; ++index)
        {
          BoundingBoxI storageBounds = this.m_cutOuts[index].StorageBounds;
          storageBounds.Min >>= 4;
          storageBounds.Max = storageBounds.Max + 15 >> 4;
          foreach (Vector3I enumeratePoint in BoundingBoxI.EnumeratePoints(storageBounds))
            this.m_chunkCutOutPairs.Add((enumeratePoint, index));
        }
        this.m_chunkCutOutPairs.Sort(MyShipMiningSystem.ClusterCutOut.m_sortPerChunk);
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Executing;
        this.m_debugChunkProgress = 0;
        MyShipMiningSystem.ClusterCutOut.m_storageData.Resize(new Vector3I(16));
        BoundingBoxI range = new BoundingBoxI();
        Vector3I vector3I1 = -Vector3I.One;
        Vector3I vector3I2 = new Vector3I();
        (bool, bool) changes = (false, false);
        foreach ((Vector3I Chunk, int num) in this.m_chunkCutOutPairs)
        {
          if (Chunk != vector3I1)
          {
            if (vector3I1 != -Vector3I.One)
              this.CommitDataToStorage(in range, in changes);
            range = new BoundingBoxI(Chunk << 4, Chunk + 1 << 4);
            this.m_storage.ReadRange(MyShipMiningSystem.ClusterCutOut.m_storageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, range.Min, range.Max - 1);
            vector3I2 = this.m_storageOffset - range.Min;
            vector3I1 = Chunk;
          }
          ref MyShipMiningSystem.ClusterCutOut.CutOutData local_11 = ref this.m_cutOuts[num];
          BoundingSphere local_12 = local_11.SphereInVoxelSpace;
          local_12.Center += (Vector3) vector3I2;
          this.CutOutChunk(local_12, MyShipMiningSystem.ClusterCutOut.m_storageData, ref local_11, ref changes);
          if ((double) MyShipMiningSystem.DebugOperationDelay > 0.0)
            Thread.Sleep((int) ((double) MyShipMiningSystem.DebugOperationDelay * 1000.0));
          ++this.m_debugChunkProgress;
        }
        this.CommitDataToStorage(in range, in changes);
        this.m_materialsChanged = changes.Item2;
      }

      private void CommitDataToStorage(
        in BoundingBoxI range,
        in (bool Content, bool Material) changes)
      {
        if (!changes.Item1)
          return;
        this.m_totalAffectedRange.Include(range);
        this.m_storage.WriteRange(MyShipMiningSystem.ClusterCutOut.m_storageData, changes.Item2 ? MyStorageDataTypeFlags.ContentAndMaterial : MyStorageDataTypeFlags.Content, range.Min, range.Max - 1, false);
      }

      private void CutOutChunk(
        BoundingSphere sphere,
        MyStorageData data,
        ref MyShipMiningSystem.ClusterCutOut.CutOutData cutOutData,
        ref (bool Content, bool Material) changes)
      {
        changes.Item2 |= cutOutData.ApplyDamagedMaterial;
        Vector3 vector3_1 = -sphere.Center;
        Vector3 vector3_2 = vector3_1;
        float num1 = (float) (((double) sphere.Radius + 1.0) * ((double) sphere.Radius + 1.0));
        float num2 = (float) (((double) sphere.Radius - 1.0) * ((double) sphere.Radius - 1.0));
        if ((double) sphere.Radius < 0.5)
          num2 = 0.0f;
        int linearIdx = 0;
        bool flag = this.IsClient;
        Vector3I size3D = data.Size3D;
        Vector3I vector3I;
        for (vector3I.Z = 0; vector3I.Z < size3D.Z; ++vector3I.Z)
        {
          for (vector3I.Y = 0; vector3I.Y < size3D.Y; ++vector3I.Y)
          {
            for (vector3I.X = 0; vector3I.X < size3D.X; ++vector3I.X)
            {
              float num3 = vector3_1.LengthSquared();
              if ((double) num3 < (double) num1)
              {
                byte num4 = data.Content(linearIdx);
                byte content;
                if ((double) num3 < (double) num2)
                {
                  content = (byte) 0;
                }
                else
                {
                  float num5 = (float) Math.Sqrt((double) num3) - sphere.Radius;
                  content = (byte) (((double) num5 + 1.0) * 127.5);
                  if ((int) content >= (int) num4)
                    content = (byte) ((double) num4 * (double) num5);
                }
                if ((int) content < (int) num4)
                {
                  if (!this.IsClient)
                  {
                    flag = true;
                    int num5 = (int) num4 - (int) content;
                    if (MyFakes.ENABLE_REMOVED_VOXEL_CONTENT_HACK)
                      num5 = (int) ((double) num5 * 4.61999988555908);
                    byte num6 = data.Material(linearIdx);
                    if (num6 >= (byte) 0 && (int) num6 < cutOutData.CutOutMaterials.Length)
                      cutOutData.CutOutMaterials[(int) num6] += num5;
                  }
                  data.Content(linearIdx, content);
                }
              }
              ++linearIdx;
              ++vector3_1.X;
            }
            vector3_1.X = vector3_2.X;
            ++vector3_1.Y;
          }
          vector3_1.Y = vector3_2.Y;
          ++vector3_1.Z;
        }
        cutOutData.CausedChange |= flag;
        changes.Item1 |= flag;
      }

      private void Finish(
        HashSet<MyShipMiningSystem.ClusterCutOut> operationQueue)
      {
        if (this.m_totalAffectedRange.IsValid)
        {
          if (!this.IsClient)
          {
            for (int index1 = 0; index1 < this.m_cutOuts.Length; ++index1)
            {
              ref MyShipMiningSystem.ClusterCutOut.CutOutData local = ref this.m_cutOuts[index1];
              Dictionary<MyVoxelMaterialDefinition, int> materials = new Dictionary<MyVoxelMaterialDefinition, int>();
              for (int index2 = 0; index2 < local.CutOutMaterials.Length; ++index2)
                materials[MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) index2)] = local.CutOutMaterials[index2];
              local.Drill.OnDrillResults(materials, local.HitPosition, !local.DiscardResults);
            }
          }
          Vector3I min = this.m_totalAffectedRange.Min;
          Vector3I voxelRangeMax = this.m_totalAffectedRange.Max - 1;
          this.m_storage.NotifyRangeChanged(ref min, ref voxelRangeMax, this.m_materialsChanged ? MyStorageDataTypeFlags.ContentAndMaterial : MyStorageDataTypeFlags.Content);
          BoundingBoxD totalAffectedRange = (BoundingBoxD) this.m_totalAffectedRange;
          totalAffectedRange.Translate((Vector3D) (this.m_targetVoxel.SizeInMetresHalf + (Vector3) this.m_targetVoxel.StorageMin));
          totalAffectedRange.TransformFast(this.m_targetVoxel.PositionComp.WorldMatrixRef);
          MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, this.m_targetVoxel, ref totalAffectedRange);
          if (Sync.IsServer && Sync.Clients.Count > 1)
          {
            int length = 0;
            for (int index = 0; index < this.m_cutOuts.Length; ++index)
            {
              if (this.m_cutOuts[index].CausedChange)
                ++length;
            }
            MyShipMiningSystem.NetworkCutoutData.CutOut[] cutOuts = new MyShipMiningSystem.NetworkCutoutData.CutOut[length];
            int num = 0;
            for (int index = 0; index < this.m_cutOuts.Length; ++index)
            {
              ref MyShipMiningSystem.ClusterCutOut.CutOutData local = ref this.m_cutOuts[index];
              if (local.CausedChange)
                cutOuts[num++] = new MyShipMiningSystem.NetworkCutoutData.CutOut(local.SphereInVoxelSpace, local.ApplyDamagedMaterial);
            }
            MyVoxelBase rootVoxel = this.m_targetVoxel.RootVoxel;
            MyShipMiningSystem.NetworkCutoutData networkCutoutData = new MyShipMiningSystem.NetworkCutoutData(this.m_storageOffset, this.m_totalAffectedRange, cutOuts);
            ref MyShipMiningSystem.NetworkCutoutData local1 = ref networkCutoutData;
            rootVoxel.BroadcastShipCutout(in local1);
          }
        }
        if (!this.IsClient && MyShipMiningSystem.DebugDrawCutOuts && (double) MyShipMiningSystem.DebugDrawCutOutPermanence > 0.0)
          return;
        this.Return(operationQueue);
      }

      public void Return(
        HashSet<MyShipMiningSystem.ClusterCutOut> operationQueue)
      {
        this.m_state = MyShipMiningSystem.ClusterCutOut.State.Pooled;
        operationQueue?.Remove(this);
        this.m_chunkCutOutPairs.Clear();
        PoolManager.ReturnBorrowedMemory<MyShipMiningSystem.ClusterCutOut.CutOutData>(ref this.m_cutOuts);
        MyShipMiningSystem.ClusterCutOut clusterCutOut = this;
        PoolManager.Return<MyShipMiningSystem.ClusterCutOut>(ref clusterCutOut);
      }

      public WorkOptions Options => new WorkOptions()
      {
        DebugName = "Drill Cluster Cutout",
        MaximumThreads = 1,
        TaskType = MyProfiler.TaskType.Voxels
      };

      public bool DebugDraw()
      {
        Color color1;
        switch (this.m_state)
        {
          case MyShipMiningSystem.ClusterCutOut.State.Pooled:
            color1 = Color.Red;
            break;
          case MyShipMiningSystem.ClusterCutOut.State.Queued:
            color1 = Color.Blue;
            break;
          case MyShipMiningSystem.ClusterCutOut.State.Preparing:
            color1 = Color.Orange;
            break;
          case MyShipMiningSystem.ClusterCutOut.State.Executing:
            color1 = Color.Yellow;
            break;
          case MyShipMiningSystem.ClusterCutOut.State.Finished:
            color1 = Color.Green;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) this.m_clusterBounds, this.m_debugDrawMatrix), color1, 0.1f, true, true);
        if (this.m_state > MyShipMiningSystem.ClusterCutOut.State.Preparing)
        {
          for (int index = 0; index < this.m_cutOuts.Length; ++index)
          {
            ref BoundingSphere local = ref this.m_cutOuts[index].SphereInVoxelSpace;
            Vector3D result;
            Vector3D.Transform(ref local.Center, ref this.m_debugDrawMatrix, out result);
            MyRenderProxy.DebugDrawSphere(result, local.Radius, color1.Alpha(0.2f), smooth: true);
            MyRenderProxy.DebugDrawSphere(result, local.Radius, color1);
          }
          Vector3D pointFrom = Vector3D.Zero;
          Vector3I vector3I = new Vector3I(int.MinValue);
          for (int index = 0; index < this.m_chunkCutOutPairs.Count; ++index)
          {
            (Vector3I Chunk4, int num4) = this.m_chunkCutOutPairs[index];
            if (vector3I != Chunk4)
            {
              Color color2 = Color.Blue;
              if (this.m_debugChunkProgress >= index)
                color2 = this.m_debugChunkProgress == this.m_chunkCutOutPairs.Count || this.m_chunkCutOutPairs[this.m_debugChunkProgress].Chunk != Chunk4 ? Color.Green : Color.Yellow;
              BoundingBox boundingBox = new BoundingBox((Vector3) (Chunk4 << 4), (Vector3) (Chunk4 + 1 << 4));
              boundingBox.Translate((Vector3) -this.m_storageOffset);
              pointFrom = Vector3D.Transform(boundingBox.Center, this.m_debugDrawMatrix);
              MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) boundingBox, this.m_debugDrawMatrix), color2, 0.1f, true, true);
            }
            Vector3D result;
            Vector3D.Transform(ref this.m_cutOuts[num4].SphereInVoxelSpace.Center, ref this.m_debugDrawMatrix, out result);
            Color colorFrom = this.m_debugChunkProgress > index ? Color.Green : (this.m_debugChunkProgress == index ? Color.Yellow : Color.Blue);
            MyRenderProxy.DebugDrawArrow3D(pointFrom, result, colorFrom);
            vector3I = Chunk4;
          }
        }
        return this.m_state == MyShipMiningSystem.ClusterCutOut.State.Finished && (DateTime.Now - this.m_debugFinishedTime).TotalSeconds > (double) MyShipMiningSystem.DebugDrawCutOutPermanence;
      }

      [DebuggerDisplay("{DebugText}")]
      private struct CutOutData
      {
        public BoundingSphere SphereInVoxelSpace;
        public BoundingBoxI StorageBounds;
        public readonly bool ApplyDamagedMaterial;
        public readonly bool DiscardResults;
        public Vector3D HitPosition;
        public bool CausedChange;
        public readonly MyShipDrill Drill;
        public readonly int[] CutOutMaterials;

        public CutOutData(
          in MyShipMiningSystem.CutOutRequest request,
          in MyShipMiningSystem.DrillData data)
        {
          this.SphereInVoxelSpace = data.BondingSphere;
          if (request.ApplyDiscardingMultiplier)
            this.SphereInVoxelSpace.Radius *= 3f;
          this.StorageBounds = new BoundingBoxI();
          this.ApplyDamagedMaterial = request.ApplyDamagedMaterial;
          this.DiscardResults = request.ApplyDiscardingMultiplier;
          this.Drill = request.Drill;
          this.CutOutMaterials = new int[MyDefinitionManager.Static.VoxelMaterialCount];
          this.CausedChange = false;
          this.HitPosition = request.HitPosition;
        }

        public CutOutData(
          in MyShipMiningSystem.NetworkCutoutData.CutOut cutout,
          Vector3I storageOffset)
        {
          this.SphereInVoxelSpace = cutout.Sphere;
          this.StorageBounds = new BoundingBoxI(Vector3I.Floor(this.SphereInVoxelSpace.Center - this.SphereInVoxelSpace.Radius) + storageOffset - 1, Vector3I.Ceiling(this.SphereInVoxelSpace.Center + this.SphereInVoxelSpace.Radius) + storageOffset + 1);
          this.Drill = (MyShipDrill) null;
          this.CutOutMaterials = (int[]) null;
          this.CausedChange = false;
          this.ApplyDamagedMaterial = cutout.ApplyDamagedMaterial;
          this.DiscardResults = true;
          this.HitPosition = new Vector3D();
        }

        private string DebugText => string.Format("{0}{1}", (object) this.SphereInVoxelSpace, this.ApplyDamagedMaterial ? (object) " ApplyDamage" : (object) "");
      }

      private class DrillChunkComparer : IComparer<(Vector3I Chunk, int Drill)>
      {
        public int Compare((Vector3I Chunk, int Drill) x, (Vector3I Chunk, int Drill) y) => x.Chunk.CompareTo(y.Chunk);
      }

      private enum State : byte
      {
        Pooled,
        Idle,
        Queued,
        Preparing,
        Executing,
        Finished,
      }
    }

    [Serializable]
    public struct NetworkCutoutData
    {
      public Vector3I StorageOffset;
      public BoundingBoxI AffectedRange;
      public MyShipMiningSystem.NetworkCutoutData.CutOut[] CutOuts;

      public NetworkCutoutData(
        Vector3I storageOffset,
        BoundingBoxI affectedRange,
        MyShipMiningSystem.NetworkCutoutData.CutOut[] cutOuts)
      {
        this.StorageOffset = storageOffset;
        this.CutOuts = cutOuts;
        this.AffectedRange = affectedRange;
      }

      [DebuggerDisplay("{DebugText}")]
      [Serializable]
      public struct CutOut
      {
        public BoundingSphere Sphere;
        public bool ApplyDamagedMaterial;

        public CutOut(BoundingSphere sphere, bool applyDamagedMaterial)
        {
          this.Sphere = sphere;
          this.ApplyDamagedMaterial = applyDamagedMaterial;
        }

        private string DebugText => string.Format("{0}{1}", (object) this.Sphere, this.ApplyDamagedMaterial ? (object) " ApplyDamage" : (object) "");

        protected class Sandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData\u003C\u003ECutOut\u003C\u003ESphere\u003C\u003EAccessor : IMemberAccessor<MyShipMiningSystem.NetworkCutoutData.CutOut, BoundingSphere>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref MyShipMiningSystem.NetworkCutoutData.CutOut owner,
            in BoundingSphere value)
          {
            owner.Sphere = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref MyShipMiningSystem.NetworkCutoutData.CutOut owner,
            out BoundingSphere value)
          {
            value = owner.Sphere;
          }
        }

        protected class Sandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData\u003C\u003ECutOut\u003C\u003EApplyDamagedMaterial\u003C\u003EAccessor : IMemberAccessor<MyShipMiningSystem.NetworkCutoutData.CutOut, bool>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref MyShipMiningSystem.NetworkCutoutData.CutOut owner,
            in bool value)
          {
            owner.ApplyDamagedMaterial = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref MyShipMiningSystem.NetworkCutoutData.CutOut owner,
            out bool value)
          {
            value = owner.ApplyDamagedMaterial;
          }
        }
      }

      protected class Sandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData\u003C\u003EStorageOffset\u003C\u003EAccessor : IMemberAccessor<MyShipMiningSystem.NetworkCutoutData, Vector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyShipMiningSystem.NetworkCutoutData owner, in Vector3I value) => owner.StorageOffset = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyShipMiningSystem.NetworkCutoutData owner, out Vector3I value) => value = owner.StorageOffset;
      }

      protected class Sandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData\u003C\u003EAffectedRange\u003C\u003EAccessor : IMemberAccessor<MyShipMiningSystem.NetworkCutoutData, BoundingBoxI>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyShipMiningSystem.NetworkCutoutData owner,
          in BoundingBoxI value)
        {
          owner.AffectedRange = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyShipMiningSystem.NetworkCutoutData owner,
          out BoundingBoxI value)
        {
          value = owner.AffectedRange;
        }
      }

      protected class Sandbox_Game_GameSystems_MyShipMiningSystem\u003C\u003ENetworkCutoutData\u003C\u003ECutOuts\u003C\u003EAccessor : IMemberAccessor<MyShipMiningSystem.NetworkCutoutData, MyShipMiningSystem.NetworkCutoutData.CutOut[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyShipMiningSystem.NetworkCutoutData owner,
          in MyShipMiningSystem.NetworkCutoutData.CutOut[] value)
        {
          owner.CutOuts = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyShipMiningSystem.NetworkCutoutData owner,
          out MyShipMiningSystem.NetworkCutoutData.CutOut[] value)
        {
          value = owner.CutOuts;
        }
      }
    }

    private struct DrillData
    {
      public readonly MyShipDrill Drill;
      public MyShipMiningSystem.DrillCluster Cluster;
      public int TreeProxyId;
      public readonly BoundingSphere BondingSphere;

      public DrillData(MyShipDrill drill)
      {
        this.Drill = drill;
        this.BondingSphere = drill.GetDrillingSphere();
        this.Cluster = (MyShipMiningSystem.DrillCluster) null;
        this.TreeProxyId = -1;
      }
    }
  }
}
