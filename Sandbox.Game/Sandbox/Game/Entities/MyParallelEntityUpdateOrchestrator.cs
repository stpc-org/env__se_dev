// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyParallelEntityUpdateOrchestrator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Library.Parallelization;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  public class MyParallelEntityUpdateOrchestrator : IMyUpdateOrchestrator
  {
    private readonly Dictionary<MyEntity, MyParallelUpdateFlags> m_lastUpdateRecord;
    private readonly HashSet<MyEntity> m_entitiesToAdd;
    private readonly HashSet<MyEntity> m_entitiesToRemove;
    private readonly List<MyEntity> m_entitiesForUpdateOnce;
    private readonly HashSet<MyEntity> m_entitiesForUpdate;
    private readonly HashSet<IMyParallelUpdateable> m_entitiesForUpdateParallel;
    private readonly MyDistributedUpdater<List<MyEntity>, MyEntity> m_entitiesForUpdate10;
    private readonly MyDistributedUpdater<List<MyEntity>, MyEntity> m_entitiesForUpdate100;
    private List<(Action Callback, string DebugName)> m_callbacksPendingExecution;
    private List<(Action Callback, string DebugName)> m_callbacksPendingExecutionSwap;
    private readonly List<MyEntity> m_entitiesForSimulate;
    private readonly Action<IMyParallelUpdateable> m_parallelUpdateHandlerBeforeSimulation;
    private readonly Action<IMyParallelUpdateable> m_parallelUpdateHandlerAfterSimulation;
    public static WorkPriority WorkerPriority;
    public static bool ForceSerialUpdate;
    private readonly object m_lock;
    private DataGuard EntitySyncGuard;
    private readonly Dictionary<string, int> m_typesStats;

    public MyParallelEntityUpdateOrchestrator()
    {
      this.m_entitiesForUpdateOnce = new List<MyEntity>();
      this.m_entitiesForUpdate = new HashSet<MyEntity>();
      this.m_entitiesForUpdateParallel = new HashSet<IMyParallelUpdateable>();
      this.m_entitiesForUpdate10 = new MyDistributedUpdater<List<MyEntity>, MyEntity>(10);
      this.m_entitiesForUpdate100 = new MyDistributedUpdater<List<MyEntity>, MyEntity>(100);
      this.m_entitiesForSimulate = new List<MyEntity>();
      this.m_callbacksPendingExecution = new List<(Action, string)>();
      this.m_callbacksPendingExecutionSwap = new List<(Action, string)>();
      this.m_entitiesToRemove = new HashSet<MyEntity>();
      this.m_entitiesToAdd = new HashSet<MyEntity>();
      this.m_lastUpdateRecord = new Dictionary<MyEntity, MyParallelUpdateFlags>();
      this.m_lock = (object) this.m_lastUpdateRecord;
      this.m_typesStats = new Dictionary<string, int>();
      this.m_parallelUpdateHandlerBeforeSimulation = new Action<IMyParallelUpdateable>(this.ParallelUpdateHandlerBeforeSimulation);
      this.m_parallelUpdateHandlerAfterSimulation = new Action<IMyParallelUpdateable>(this.ParallelUpdateHandlerAfterSimulation);
    }

    public void Unload()
    {
      this.m_entitiesForUpdateOnce.Clear();
      this.m_entitiesForUpdate.Clear();
      this.m_entitiesForUpdateParallel.Clear();
      this.m_entitiesForUpdate10.List.Clear();
      this.m_entitiesForUpdate100.List.Clear();
      this.m_entitiesForSimulate.Clear();
      this.m_entitiesToAdd.Clear();
      this.m_entitiesToRemove.Clear();
      this.m_lastUpdateRecord.Clear();
    }

    public void AddEntity(MyEntity entity)
    {
      lock (this.m_lock)
      {
        using (this.EntitySyncGuard.Exclusive("Can't add entities from parallel while syncing"))
        {
          this.m_entitiesToRemove.Remove(entity);
          this.m_entitiesToAdd.Add(entity);
        }
      }
    }

    private void AddEntityInternal(MyEntity entity)
    {
      MyParallelUpdateFlags parallelUpdateFlags1;
      if (entity is IMyParallelUpdateable parallelUpdateable)
      {
        parallelUpdateFlags1 = parallelUpdateable.UpdateFlags;
      }
      else
      {
        parallelUpdateFlags1 = entity.NeedsUpdate.GetParallel();
        parallelUpdateable = (IMyParallelUpdateable) null;
      }
      MyParallelUpdateFlags parallelUpdateFlags2 = parallelUpdateFlags1;
      MyParallelUpdateFlags parallelUpdateFlags3;
      if (this.m_lastUpdateRecord.TryGetValue(entity, out parallelUpdateFlags3))
      {
        parallelUpdateFlags2 = parallelUpdateFlags1 & ~parallelUpdateFlags3;
        MyParallelUpdateFlags flags = parallelUpdateFlags3 & ~parallelUpdateFlags1;
        this.RemoveWithFlags(entity, flags);
      }
      if (parallelUpdateFlags2 != MyParallelUpdateFlags.NONE)
      {
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.BEFORE_NEXT_FRAME) != MyParallelUpdateFlags.NONE)
          this.m_entitiesForUpdateOnce.Add(entity);
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != MyParallelUpdateFlags.NONE)
          this.AddOnce<IMyParallelUpdateable>(this.m_entitiesForUpdateParallel, parallelUpdateable);
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.EACH_FRAME) != MyParallelUpdateFlags.NONE)
          this.AddOnce<MyEntity>(this.m_entitiesForUpdate, entity);
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.EACH_10TH_FRAME) != MyParallelUpdateFlags.NONE)
          this.m_entitiesForUpdate10.List.Add(entity);
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.EACH_100TH_FRAME) != MyParallelUpdateFlags.NONE)
          this.m_entitiesForUpdate100.List.Add(entity);
        if ((parallelUpdateFlags2 & MyParallelUpdateFlags.SIMULATE) != MyParallelUpdateFlags.NONE)
          this.m_entitiesForSimulate.Add(entity);
      }
      this.m_lastUpdateRecord[entity] = parallelUpdateFlags1;
    }

    private void AddOnce<T>(HashSet<T> set, T value) => set.Add(value);

    public void RemoveEntity(MyEntity entity, bool immediate)
    {
      lock (this.m_lock)
      {
        using (this.EntitySyncGuard.Exclusive("Can't remove entities from parallel while syncing"))
        {
          if (this.m_entitiesToAdd.Remove(entity) || !this.m_lastUpdateRecord.ContainsKey(entity))
            return;
          if (immediate)
            this.RemoveEntityInternal(entity);
          else
            this.m_entitiesToRemove.Add(entity);
        }
      }
    }

    public void EntityFlagsChanged(MyEntity entity)
    {
      lock (this.m_lock)
        this.m_entitiesToAdd.Add(entity);
    }

    private void RemoveEntityInternal(MyEntity entity)
    {
      MyParallelUpdateFlags flags;
      if (!this.m_lastUpdateRecord.TryGetValue(entity, out flags))
        return;
      this.RemoveWithFlags(entity, flags);
      this.m_lastUpdateRecord.Remove(entity);
    }

    private void RemoveWithFlags(MyEntity entity, MyParallelUpdateFlags flags)
    {
      if ((flags & MyParallelUpdateFlags.BEFORE_NEXT_FRAME) != MyParallelUpdateFlags.NONE)
        this.m_entitiesForUpdateOnce.Remove(entity);
      if ((flags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != MyParallelUpdateFlags.NONE)
        this.m_entitiesForUpdateParallel.Remove((IMyParallelUpdateable) entity);
      if ((flags & MyParallelUpdateFlags.EACH_FRAME) != MyParallelUpdateFlags.NONE)
        this.m_entitiesForUpdate.Remove(entity);
      if ((flags & MyParallelUpdateFlags.EACH_10TH_FRAME) != MyParallelUpdateFlags.NONE)
        this.m_entitiesForUpdate10.List.Remove(entity);
      if ((flags & MyParallelUpdateFlags.EACH_100TH_FRAME) != MyParallelUpdateFlags.NONE)
        this.m_entitiesForUpdate100.List.Remove(entity);
      if ((flags & MyParallelUpdateFlags.SIMULATE) == MyParallelUpdateFlags.NONE)
        return;
      this.m_entitiesForSimulate.Remove(entity);
    }

    public void InvokeLater(Action action, string debugName = null)
    {
      lock (this.m_lock)
        this.m_callbacksPendingExecution.Add((action, debugName));
    }

    public void DispatchOnceBeforeFrame()
    {
      this.ApplyChanges();
      for (int index = 0; index < this.m_entitiesForUpdateOnce.Count; ++index)
      {
        MyEntity key = this.m_entitiesForUpdateOnce[index];
        this.m_lastUpdateRecord[key] &= ~MyParallelUpdateFlags.BEFORE_NEXT_FRAME;
        EntityFlags flags = key.Flags;
        if ((flags & EntityFlags.NeedsUpdateBeforeNextFrame) != (EntityFlags) 0)
        {
          key.Flags = flags & ~EntityFlags.NeedsUpdateBeforeNextFrame;
          if (!key.MarkedForClose && key.InScene)
            key.UpdateOnceBeforeFrame();
        }
      }
      this.m_entitiesForUpdateOnce.Clear();
      this.ProcessInvokeLater();
    }

    public void DispatchBeforeSimulation()
    {
      this.ApplyChanges();
      MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (DispatchBeforeSimulation));
      this.PerformParallelUpdate(this.m_parallelUpdateHandlerBeforeSimulation);
      this.ProcessInvokeLater();
      this.ApplyChanges();
      this.UpdateBeforeSimulation();
      this.UpdateBeforeSimulation10();
      this.UpdateBeforeSimulation100();
      MySimpleProfiler.End(nameof (DispatchBeforeSimulation));
    }

    private void ParallelUpdateHandlerBeforeSimulation(IMyParallelUpdateable entity)
    {
      if (entity.MarkedForClose || (entity.UpdateFlags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) == MyParallelUpdateFlags.NONE || !entity.InScene)
        return;
      entity.UpdateBeforeSimulationParallel();
    }

    private void UpdateBeforeSimulation()
    {
      foreach (MyEntity myEntity in this.m_entitiesForUpdate)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateBeforeSimulation();
      }
    }

    private void UpdateBeforeSimulation10()
    {
      this.m_entitiesForUpdate10.Update();
      foreach (MyEntity myEntity in this.m_entitiesForUpdate10)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate10) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateBeforeSimulation10();
      }
    }

    private void UpdateBeforeSimulation100()
    {
      this.m_entitiesForUpdate100.Update();
      foreach (MyEntity myEntity in this.m_entitiesForUpdate100)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate100) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateBeforeSimulation100();
      }
    }

    public void DispatchSimulate()
    {
      this.ApplyChanges();
      for (int index = this.m_entitiesForSimulate.Count - 1; index >= 0; --index)
      {
        MyEntity myEntity = this.m_entitiesForSimulate[index];
        if (!myEntity.MarkedForClose)
          myEntity.Simulate();
      }
      this.ProcessInvokeLater();
    }

    public void DispatchAfterSimulation()
    {
      this.ApplyChanges();
      MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (DispatchAfterSimulation));
      this.PerformParallelUpdate(this.m_parallelUpdateHandlerAfterSimulation);
      this.ProcessInvokeLater();
      this.ApplyChanges();
      this.UpdateAfterSimulation();
      this.UpdateAfterSimulation10();
      this.UpdateAfterSimulation100();
      MySimpleProfiler.End(nameof (DispatchAfterSimulation));
    }

    private void ParallelUpdateHandlerAfterSimulation(IMyParallelUpdateable entity)
    {
      if (entity.MarkedForClose || (entity.UpdateFlags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) == MyParallelUpdateFlags.NONE || !entity.InScene)
        return;
      entity.UpdateAfterSimulationParallel();
    }

    private void UpdateAfterSimulation()
    {
      foreach (MyEntity myEntity in this.m_entitiesForUpdate)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateAfterSimulation();
      }
    }

    private void UpdateAfterSimulation10()
    {
      foreach (MyEntity myEntity in this.m_entitiesForUpdate10)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate10) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateAfterSimulation10();
      }
    }

    private void UpdateAfterSimulation100()
    {
      foreach (MyEntity myEntity in this.m_entitiesForUpdate100)
      {
        if (!myEntity.MarkedForClose && (myEntity.Flags & EntityFlags.NeedsUpdate100) != (EntityFlags) 0 && myEntity.InScene)
          myEntity.UpdateAfterSimulation100();
      }
    }

    private void PerformParallelUpdate(Action<IMyParallelUpdateable> updateFunction)
    {
      using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
      {
        if (!MyParallelEntityUpdateOrchestrator.ForceSerialUpdate)
        {
          using (MyEntities.StartAsyncUpdateBlock())
            Parallel.ForEach<IMyParallelUpdateable>((IEnumerable<IMyParallelUpdateable>) this.m_entitiesForUpdateParallel, updateFunction, MyParallelEntityUpdateOrchestrator.WorkerPriority, blocking: true);
        }
        else
        {
          foreach (IMyParallelUpdateable parallelUpdateable in this.m_entitiesForUpdateParallel)
            updateFunction(parallelUpdateable);
        }
      }
    }

    public void ProcessInvokeLater()
    {
      if (this.m_callbacksPendingExecution.Count == 0)
        return;
      lock (this.m_lock)
        MyUtils.Swap<List<(Action, string)>>(ref this.m_callbacksPendingExecution, ref this.m_callbacksPendingExecutionSwap);
      int count = this.m_callbacksPendingExecutionSwap.Count;
      for (int index = 0; index < count; ++index)
        this.m_callbacksPendingExecutionSwap[index].Callback();
      this.m_callbacksPendingExecutionSwap.Clear();
    }

    private void ApplyChanges()
    {
      lock (this.m_lock)
      {
        using (this.EntitySyncGuard.Exclusive("Can't sync entities"))
        {
          foreach (MyEntity entity in this.m_entitiesToRemove)
            this.RemoveEntityInternal(entity);
          foreach (MyEntity entity in this.m_entitiesToAdd)
            this.AddEntityInternal(entity);
        }
        this.m_entitiesToAdd.Clear();
        this.m_entitiesToRemove.Clear();
      }
    }

    public void DispatchUpdatingStopped()
    {
      foreach (MyEntity myEntity in this.m_entitiesForUpdate)
        myEntity.UpdatingStopped();
    }

    public void DebugDraw()
    {
      Vector2 screenCoord = new Vector2(100f, 0.0f);
      MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
      foreach (object @object in this.m_entitiesForUpdate)
      {
        string debugName = MyDebugUtils.GetDebugName(@object);
        if (!this.m_typesStats.ContainsKey(debugName))
          this.m_typesStats.Add(debugName, 0);
        this.m_typesStats[debugName]++;
      }
      float scale = 0.7f;
      screenCoord.Y += 50f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update:", Color.Yellow, scale);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.m_typesStats.OrderByDescending<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (x => x.Value)))
      {
        MyRenderProxy.DebugDrawText2D(screenCoord, keyValuePair.Key + ": " + (object) keyValuePair.Value + "x", Color.Yellow, scale);
        screenCoord.Y += 20f;
      }
      this.m_typesStats.Clear();
      screenCoord.Y = 0.0f;
      foreach (object @object in this.m_entitiesForUpdate10.List)
      {
        string debugName = MyDebugUtils.GetDebugName(@object);
        if (!this.m_typesStats.ContainsKey(debugName))
          this.m_typesStats.Add(debugName, 0);
        this.m_typesStats[debugName]++;
      }
      screenCoord.X += 300f;
      screenCoord.Y += 50f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update10:", Color.Yellow, scale);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.m_typesStats.OrderByDescending<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (x => x.Value)))
      {
        MyRenderProxy.DebugDrawText2D(screenCoord, keyValuePair.Key + ": " + (object) keyValuePair.Value + "x", Color.Yellow, scale);
        screenCoord.Y += 20f;
      }
      this.m_typesStats.Clear();
      screenCoord.Y = 0.0f;
      foreach (object @object in this.m_entitiesForUpdate100.List)
      {
        string debugName = MyDebugUtils.GetDebugName(@object);
        if (!this.m_typesStats.ContainsKey(debugName))
          this.m_typesStats.Add(debugName, 0);
        this.m_typesStats[debugName]++;
      }
      screenCoord.X += 300f;
      screenCoord.Y += 50f;
      MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update100:", Color.Yellow, scale);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.m_typesStats.OrderByDescending<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (x => x.Value)))
      {
        MyRenderProxy.DebugDrawText2D(screenCoord, keyValuePair.Key + ": " + (object) keyValuePair.Value + "x", Color.Yellow, scale);
        screenCoord.Y += 20f;
      }
      this.m_typesStats.Clear();
      screenCoord.Y = 0.0f;
    }

    [Conditional("DEBUG")]
    private void CheckConsistent(MyEntity entity)
    {
      this.m_lastUpdateRecord.TryGetValue(entity, out MyParallelUpdateFlags _);
      CheckCollection<MyEntity>((ICollection<MyEntity>) this.m_entitiesForUpdate, MyParallelUpdateFlags.EACH_FRAME);
      CheckCollection<IMyParallelUpdateable>((ICollection<IMyParallelUpdateable>) this.m_entitiesForUpdateParallel, MyParallelUpdateFlags.EACH_FRAME_PARALLEL);
      CheckCollection<MyEntity>((ICollection<MyEntity>) this.m_entitiesForUpdate10.List, MyParallelUpdateFlags.EACH_10TH_FRAME);
      CheckCollection<MyEntity>((ICollection<MyEntity>) this.m_entitiesForUpdate100.List, MyParallelUpdateFlags.EACH_100TH_FRAME);

      void CheckCollection<T>(ICollection<T> collection, MyParallelUpdateFlags flag) where T : class, IMyEntity
      {
      }
    }

    public static bool ParallelUpdateInProgress => MyEntities.IsAsyncUpdateInProgress;
  }
}
