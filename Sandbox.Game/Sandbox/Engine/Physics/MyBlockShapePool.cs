// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyBlockShapePool
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Game.Voxels;
using VRage.Utils;

namespace Sandbox.Engine.Physics
{
  public class MyBlockShapePool
  {
    public const int PREALLOCATE_COUNT = 50;
    private const int MAX_CLONE_PER_FRAME = 3;
    private Dictionary<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> m_pools = new Dictionary<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>>();
    private MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob> m_tracker = new MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob>();
    private FastResourceLock m_poolLock = new FastResourceLock();
    private int m_missing;
    private bool m_dequeuedThisFrame;

    public void Preallocate()
    {
      MySandboxGame.Log.WriteLine("Preallocate shape pool - START");
      foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
        if (definitionGroup.Large != null && definitionGroup.Large.Public)
        {
          MyCubeBlockDefinition large = definitionGroup.Large;
          this.AllocateForDefinition(definitionGroup.Large.Model, (MyPhysicalModelDefinition) large, 50);
          foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in definitionGroup.Large.BuildProgressModels)
            this.AllocateForDefinition(buildProgressModel.File, (MyPhysicalModelDefinition) large, 50);
        }
        if (definitionGroup.Small != null && definitionGroup.Small.Public)
        {
          this.AllocateForDefinition(definitionGroup.Small.Model, (MyPhysicalModelDefinition) definitionGroup.Small, 50);
          foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in definitionGroup.Small.BuildProgressModels)
            this.AllocateForDefinition(buildProgressModel.File, (MyPhysicalModelDefinition) definitionGroup.Small, 50);
        }
      }
      MySandboxGame.Log.WriteLine("Preallocate shape pool - END");
    }

    public void AllocateForDefinition(
      string model,
      MyPhysicalModelDefinition definition,
      int count)
    {
      if (string.IsNullOrEmpty(model))
        return;
      MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
      if (modelOnlyData.HavokBreakableShapes == null)
        MyDestructionData.Static.LoadModelDestruction(model, definition, modelOnlyData.BoundingBoxSize);
      if (modelOnlyData.HavokBreakableShapes == null || modelOnlyData.HavokBreakableShapes.Length == 0)
        return;
      ConcurrentQueue<HkdBreakableShape> concurrentQueue;
      using (this.m_poolLock.AcquireExclusiveUsing())
      {
        if (!this.m_pools.ContainsKey(definition.Id))
          this.m_pools[definition.Id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
        if (!this.m_pools[definition.Id].ContainsKey(model))
          this.m_pools[definition.Id][model] = new ConcurrentQueue<HkdBreakableShape>();
        concurrentQueue = this.m_pools[definition.Id][model];
      }
      for (int index = 0; index < count; ++index)
      {
        HkdBreakableShape hkdBreakableShape = modelOnlyData.HavokBreakableShapes[0].Clone();
        concurrentQueue.Enqueue(hkdBreakableShape);
        if (index == 0)
        {
          HkMassProperties massProperties = new HkMassProperties();
          hkdBreakableShape.BuildMassProperties(ref massProperties);
          if (!massProperties.InertiaTensor.IsValid())
          {
            MyLog.Default.WriteLine(string.Format("Block with wrong destruction! (q.isOk): {0}", (object) definition.Model));
            break;
          }
        }
      }
    }

    public void RefillPools()
    {
      if (this.m_missing == 0)
        return;
      if (this.m_dequeuedThisFrame && !MyFakes.CLONE_SHAPES_ON_WORKER)
      {
        this.m_dequeuedThisFrame = false;
      }
      else
      {
        int num = 0;
        if (MyFakes.CLONE_SHAPES_ON_WORKER)
        {
          this.StartJobs();
        }
        else
        {
          using (this.m_poolLock.AcquireSharedUsing())
          {
            foreach (KeyValuePair<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> pool in this.m_pools)
            {
              foreach (KeyValuePair<string, ConcurrentQueue<HkdBreakableShape>> keyValuePair in pool.Value)
              {
                if (pool.Value.Count < 50)
                {
                  MyCubeBlockDefinition definition;
                  MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>(pool.Key, out definition);
                  int count = Math.Min(50 - pool.Value.Count, 3 - num);
                  this.AllocateForDefinition(keyValuePair.Key, (MyPhysicalModelDefinition) definition, count);
                  num += count;
                }
                if (num >= 3)
                  break;
              }
            }
          }
        }
        this.m_missing -= num;
      }
    }

    private void StartJobs()
    {
      using (this.m_poolLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<MyDefinitionId, Dictionary<string, ConcurrentQueue<HkdBreakableShape>>> pool in this.m_pools)
        {
          foreach (KeyValuePair<string, ConcurrentQueue<HkdBreakableShape>> keyValuePair in pool.Value)
          {
            if (keyValuePair.Value.Count < 50 && !this.m_tracker.Exists(pool.Key))
            {
              MyPhysicalModelDefinition definition;
              MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(pool.Key, out definition);
              MyModel modelOnlyData = MyModels.GetModelOnlyData(definition.Model);
              if (modelOnlyData.HavokBreakableShapes != null)
                MyBreakableShapeCloneJob.Start(new MyBreakableShapeCloneJob.Args()
                {
                  Model = keyValuePair.Key,
                  DefId = pool.Key,
                  ShapeToClone = modelOnlyData.HavokBreakableShapes[0],
                  Count = 50 - pool.Value.Count,
                  Tracker = this.m_tracker
                });
            }
          }
        }
      }
    }

    public HkdBreakableShape GetBreakableShape(
      string model,
      MyCubeBlockDefinition block)
    {
      this.m_dequeuedThisFrame = true;
      if (!block.Public || MyFakes.LAZY_LOAD_DESTRUCTION)
      {
        using (this.m_poolLock.AcquireExclusiveUsing())
        {
          if (!this.m_pools.ContainsKey(block.Id))
            this.m_pools[block.Id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
          if (!this.m_pools[block.Id].ContainsKey(model))
            this.m_pools[block.Id][model] = new ConcurrentQueue<HkdBreakableShape>();
        }
      }
      ConcurrentQueue<HkdBreakableShape> concurrentQueue = this.m_pools[block.Id][model];
      if (concurrentQueue.Count == 0)
        this.AllocateForDefinition(model, (MyPhysicalModelDefinition) block, 1);
      else
        ++this.m_missing;
      HkdBreakableShape result;
      concurrentQueue.TryDequeue(out result);
      return result;
    }

    internal void Free()
    {
      HashSet<IntPtr> numSet = new HashSet<IntPtr>();
      this.m_tracker.CancelAll();
      using (this.m_poolLock.AcquireExclusiveUsing())
      {
        foreach (Dictionary<string, ConcurrentQueue<HkdBreakableShape>> dictionary in this.m_pools.Values)
        {
          foreach (ConcurrentQueue<HkdBreakableShape> concurrentQueue in dictionary.Values)
          {
            foreach (HkdBreakableShape hkdBreakableShape in concurrentQueue)
            {
              if (numSet.Contains(hkdBreakableShape.NativeDebug))
                MyLog.Default.WriteLine("Shape " + hkdBreakableShape.Name + " was referenced twice in the pool!");
              numSet.Add(hkdBreakableShape.NativeDebug);
            }
          }
        }
        foreach (Dictionary<string, ConcurrentQueue<HkdBreakableShape>> dictionary in this.m_pools.Values)
        {
          foreach (ConcurrentQueue<HkdBreakableShape> concurrentQueue in dictionary.Values)
          {
            foreach (HkdBreakableShape hkdBreakableShape in concurrentQueue)
            {
              IntPtr nativeDebug = hkdBreakableShape.NativeDebug;
              hkdBreakableShape.RemoveReference();
            }
          }
        }
        this.m_pools.Clear();
      }
    }

    public void EnqueShapes(string model, MyDefinitionId id, List<HkdBreakableShape> shapes)
    {
      using (this.m_poolLock.AcquireExclusiveUsing())
      {
        if (!this.m_pools.ContainsKey(id))
          this.m_pools[id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
        if (!this.m_pools[id].ContainsKey(model))
          this.m_pools[id][model] = new ConcurrentQueue<HkdBreakableShape>();
      }
      foreach (HkdBreakableShape shape in shapes)
        this.m_pools[id][model].Enqueue(shape);
      this.m_missing -= shapes.Count;
    }

    public void EnqueShape(string model, MyDefinitionId id, HkdBreakableShape shape)
    {
      using (this.m_poolLock.AcquireExclusiveUsing())
      {
        if (!this.m_pools.ContainsKey(id))
          this.m_pools[id] = new Dictionary<string, ConcurrentQueue<HkdBreakableShape>>();
        if (!this.m_pools[id].ContainsKey(model))
          this.m_pools[id][model] = new ConcurrentQueue<HkdBreakableShape>();
      }
      this.m_pools[id][model].Enqueue(shape);
      --this.m_missing;
    }
  }
}
