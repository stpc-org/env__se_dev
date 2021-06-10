// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyUpdateOrchestrator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  public class MyUpdateOrchestrator : IMyUpdateOrchestrator
  {
    private readonly CachingList<MyEntity> m_entitiesForUpdateOnce;
    private readonly MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity> m_entitiesForUpdate;
    private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate10;
    private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate100;
    private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForSimulate;
    private readonly Dictionary<string, int> m_typesStats;

    public MyUpdateOrchestrator()
    {
      this.m_entitiesForUpdateOnce = new CachingList<MyEntity>();
      this.m_entitiesForUpdate = new MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity>(1);
      this.m_entitiesForUpdate10 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(10);
      this.m_entitiesForUpdate100 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);
      this.m_entitiesForSimulate = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(1);
      this.m_typesStats = new Dictionary<string, int>();
    }

    public void Unload()
    {
    }

    public void AddEntity(MyEntity entity)
    {
      if ((entity.NeedsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) > MyEntityUpdateEnum.NONE)
        this.m_entitiesForUpdateOnce.Add(entity);
      if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) > MyEntityUpdateEnum.NONE)
        this.m_entitiesForUpdate.List.Add(entity);
      if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) > MyEntityUpdateEnum.NONE)
        this.m_entitiesForUpdate10.List.Add(entity);
      if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) > MyEntityUpdateEnum.NONE)
        this.m_entitiesForUpdate100.List.Add(entity);
      if ((entity.NeedsUpdate & MyEntityUpdateEnum.SIMULATE) <= MyEntityUpdateEnum.NONE)
        return;
      this.m_entitiesForSimulate.List.Add(entity);
    }

    public void RemoveEntity(MyEntity entity, bool immediate)
    {
      if ((entity.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != (EntityFlags) 0)
        this.m_entitiesForUpdateOnce.Remove(entity, immediate);
      if ((entity.Flags & EntityFlags.NeedsUpdate) != (EntityFlags) 0)
        this.m_entitiesForUpdate.List.Remove(entity, immediate);
      if ((entity.Flags & EntityFlags.NeedsUpdate10) != (EntityFlags) 0)
        this.m_entitiesForUpdate10.List.Remove(entity, immediate);
      if ((entity.Flags & EntityFlags.NeedsUpdate100) != (EntityFlags) 0)
        this.m_entitiesForUpdate100.List.Remove(entity, immediate);
      if ((entity.Flags & EntityFlags.NeedsSimulate) == (EntityFlags) 0)
        return;
      this.m_entitiesForSimulate.List.Remove(entity, immediate);
    }

    public void EntityFlagsChanged(MyEntity entity)
    {
      this.RemoveEntity(entity, false);
      this.AddEntity(entity);
    }

    public void InvokeLater(Action action, string debugName = null) => MySandboxGame.Static.Invoke(action, debugName ?? MyDebugUtils.GetDebugName((MemberInfo) action.Method));

    public void DispatchOnceBeforeFrame()
    {
      this.m_entitiesForUpdateOnce.ApplyChanges();
      foreach (MyEntity myEntity in this.m_entitiesForUpdateOnce)
      {
        myEntity.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if (!myEntity.MarkedForClose)
          myEntity.UpdateOnceBeforeFrame();
      }
    }

    public void DispatchBeforeSimulation()
    {
      this.m_entitiesForUpdate.List.ApplyChanges();
      this.m_entitiesForUpdate.Update();
      MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (DispatchBeforeSimulation));
      this.m_entitiesForUpdate.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateBeforeSimulation();
      }));
      this.m_entitiesForUpdate10.List.ApplyChanges();
      this.m_entitiesForUpdate10.Update();
      this.m_entitiesForUpdate10.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateBeforeSimulation10();
      }));
      this.m_entitiesForUpdate100.List.ApplyChanges();
      this.m_entitiesForUpdate100.Update();
      this.m_entitiesForUpdate100.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateBeforeSimulation100();
      }));
      MySimpleProfiler.End(nameof (DispatchBeforeSimulation));
    }

    public void DispatchSimulate()
    {
      this.m_entitiesForSimulate.List.ApplyChanges();
      this.m_entitiesForSimulate.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.Simulate();
      }));
    }

    public void DispatchAfterSimulation()
    {
      this.m_entitiesForUpdate.List.ApplyChanges();
      MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (DispatchAfterSimulation));
      this.m_entitiesForUpdate.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateAfterSimulation();
      }));
      this.m_entitiesForUpdate10.List.ApplyChanges();
      this.m_entitiesForUpdate10.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateAfterSimulation10();
      }));
      this.m_entitiesForUpdate100.List.ApplyChanges();
      this.m_entitiesForUpdate100.Iterate((Action<MyEntity>) (x =>
      {
        if (x.MarkedForClose)
          return;
        x.UpdateAfterSimulation100();
      }));
      MySimpleProfiler.End(nameof (DispatchAfterSimulation));
    }

    public void DispatchUpdatingStopped()
    {
      for (int index = 0; index < this.m_entitiesForUpdate.List.Count; ++index)
        this.m_entitiesForUpdate.List[index].UpdatingStopped();
    }

    public void DebugDraw()
    {
      Vector2 screenCoord = new Vector2(100f, 0.0f);
      MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
      foreach (object obj in this.m_entitiesForUpdate.List)
      {
        string name = obj.GetType().Name;
        if (!this.m_typesStats.ContainsKey(name))
          this.m_typesStats.Add(name, 0);
        this.m_typesStats[name]++;
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
      foreach (object obj in this.m_entitiesForUpdate10.List)
      {
        string name = obj.GetType().Name;
        if (!this.m_typesStats.ContainsKey(name))
          this.m_typesStats.Add(name, 0);
        this.m_typesStats[name]++;
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
      foreach (object obj in this.m_entitiesForUpdate100.List)
      {
        string name = obj.GetType().Name;
        if (!this.m_typesStats.ContainsKey(name))
          this.m_typesStats.Add(name, 0);
        this.m_typesStats[name]++;
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
  }
}
