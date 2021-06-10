// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentSmartUpdater
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Threading;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 887, typeof (MyObjectBuilder_SessionComponentSmartUpdater), null, false)]
  public class MySessionComponentSmartUpdater : MySessionComponentBase
  {
    private static readonly int TIMER_MAX = 100;
    public MySessionComponentSmartUpdaterDefinition Definition;
    private MyConcurrentHashSet<long> m_updaterGrids = new MyConcurrentHashSet<long>();
    private MyDistributedUpdater<ConcurrentCachingList<MyCubeGrid>, MyCubeGrid> m_updater = new MyDistributedUpdater<ConcurrentCachingList<MyCubeGrid>, MyCubeGrid>(1000);
    private MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_updaterMovement = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);
    private Dictionary<long, int> m_treeGrids = new Dictionary<long, int>();
    private MyDynamicAABBTreeD m_tree = new MyDynamicAABBTreeD();
    private int m_timer;
    private static readonly SpinLockRef m_movedLock = new SpinLockRef();
    private HashSet<MyEntity> m_moved = new HashSet<MyEntity>();
    private HashSet<MyEntity> m_movedSwap = new HashSet<MyEntity>();

    public override MyObjectBuilder_SessionComponent GetObjectBuilder() => (MyObjectBuilder_SessionComponent) (base.GetObjectBuilder() as MyObjectBuilder_SessionComponentSmartUpdater);

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      this.Definition = definition as MySessionComponentSmartUpdaterDefinition;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!MyFakes.ENABLE_SMART_UPDATER || !Sync.IsServer)
        return;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid grid)
          this.AddGridToUpdater(grid);
      }
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.EntityAddedToScene);
      MyEntities.OnEntityRemove += new Action<MyEntity>(this.EntityRemovedFromScene);
      MyEntity.UpdateGamePruningStructureExtCallBack += new Action<MyEntity>(this.EntityMoved);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.EntityAddedToScene);
      MyEntities.OnEntityRemove -= new Action<MyEntity>(this.EntityRemovedFromScene);
      MyEntity.UpdateGamePruningStructureExtCallBack -= new Action<MyEntity>(this.EntityMoved);
    }

    private void EntityMoved(MyEntity obj)
    {
      if (!this.m_treeGrids.ContainsKey(obj.EntityId))
        return;
      using (MySessionComponentSmartUpdater.m_movedLock.Acquire())
        this.m_moved.Add(obj);
    }

    private void EntityRemovedFromScene(MyEntity obj)
    {
      if (!(obj is MyCubeGrid grid))
        return;
      this.RemoveGridFromUpdater(grid);
    }

    private void EntityAddedToScene(MyEntity obj)
    {
      if (!(obj is MyCubeGrid grid))
        return;
      this.AddGridToUpdater(grid);
    }

    private void AddGridToUpdater(MyCubeGrid grid)
    {
      if (this.m_treeGrids.ContainsKey(grid.EntityId))
        return;
      BoundingBoxD entityAabb = MySessionComponentSmartUpdater.GetEntityAABB((MyEntity) grid);
      int num = this.m_tree.AddProxy(ref entityAabb, (object) grid, 0U);
      this.m_treeGrids.Add(grid.EntityId, num);
    }

    private void RemoveGridFromUpdater(MyCubeGrid grid)
    {
      if (!this.m_treeGrids.ContainsKey(grid.EntityId))
        return;
      this.m_tree.RemoveProxy(this.m_treeGrids[grid.EntityId]);
      this.m_treeGrids.Remove(grid.EntityId);
    }

    public bool RegisterToUpdater(MyCubeGrid grid)
    {
      if (this.m_updaterGrids.Contains(grid.EntityId))
        return false;
      this.m_updaterGrids.Add(grid.EntityId);
      this.m_updater.List.Add(grid);
      return true;
    }

    public bool UnregisterFromUpdater(MyCubeGrid grid)
    {
      if (!this.m_updaterGrids.Contains(grid.EntityId))
        return false;
      this.m_updaterGrids.Remove(grid.EntityId);
      this.m_updater.List.Remove(grid);
      return true;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!MyFakes.ENABLE_SMART_UPDATER || !Sync.IsServer)
        return;
      --this.m_timer;
      if (this.m_timer <= 0)
      {
        this.m_timer = MySessionComponentSmartUpdater.TIMER_MAX;
        using (MySessionComponentSmartUpdater.m_movedLock.Acquire())
          MyUtils.Swap<HashSet<MyEntity>>(ref this.m_moved, ref this.m_movedSwap);
        this.m_updaterMovement.List.ClearImmediate();
        foreach (MyEntity entity in this.m_movedSwap)
          this.m_updaterMovement.List.Add(entity);
        this.m_updaterMovement.List.ApplyChanges();
      }
      this.m_updaterMovement.Update();
      this.m_updaterMovement.Iterate((Action<MyEntity>) (x => this.MoveInternal(x)));
      this.m_movedSwap.Clear();
      this.m_updater.List.ApplyChanges();
      this.m_updater.Update();
      this.m_updater.Iterate((Action<MyCubeGrid>) (x =>
      {
        if (x == null)
        {
          MyLog.Default.Error("Grid is null in SmartUpdater. This must not happen.");
          this.m_updater.List.Remove(x);
        }
        else
        {
          BoundingSphereD sphere = new BoundingSphereD(x.PositionComp.GetPosition(), 3000.0);
          List<MyEntity> overlapElementsList = new List<MyEntity>();
          this.m_tree.OverlapAllBoundingSphere<MyEntity>(ref sphere, overlapElementsList);
          if (overlapElementsList.Count == 1 && overlapElementsList[0] == x || overlapElementsList.Count == 0)
            x.OnGridPresenceUpdate(false);
          else
            x.OnGridPresenceUpdate(true);
        }
      }));
    }

    private void MoveInternal(MyEntity moved)
    {
      if (!this.m_treeGrids.ContainsKey(moved.EntityId))
        return;
      BoundingBoxD entityAabb = MySessionComponentSmartUpdater.GetEntityAABB(moved);
      this.m_tree.MoveProxy(this.m_treeGrids[moved.EntityId], ref entityAabb, Vector3D.Zero);
    }

    private static BoundingBoxD GetEntityAABB(MyEntity entity)
    {
      BoundingBoxD boundingBoxD = entity.PositionComp.WorldAABB;
      if (entity.Physics != null)
        boundingBoxD = boundingBoxD.Include(entity.WorldMatrix.Translation + entity.Physics.LinearVelocity * 0.01666667f * 5f);
      return boundingBoxD;
    }
  }
}
