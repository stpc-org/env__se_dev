// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyEntityReplicableBase`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Replication;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
  public abstract class MyEntityReplicableBase<T> : MyExternalReplicable<T>, IMyEntityReplicable
    where T : MyEntity
  {
    private Action<MyEntity> m_onCloseAction;
    private readonly List<IMyReplicable> m_tmpReplicables = new List<IMyReplicable>();
    private readonly HashSet<IMyReplicable> m_physicalDependencies = new HashSet<IMyReplicable>();
    private MyTimeSpan m_lastPhysicalDependencyUpdate;
    private bool m_destroyed;
    private int m_replicationCounter;
    protected const double MIN_DITHER_DISTANCE_SQR = 1000000.0;

    public override bool IsValid => (object) this.Instance != null && !this.Instance.MarkedForClose;

    public MatrixD WorldMatrix => (object) this.Instance != null ? this.Instance.WorldMatrix : MatrixD.Identity;

    public long EntityId => (object) this.Instance != null ? this.Instance.EntityId : 0L;

    protected override void OnLoad(BitStream stream, Action<T> loadingDoneHandler)
    {
      MyObjectBuilder_EntityBase andRead = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
      this.TryRemoveExistingEntity(andRead.EntityId);
      T fromObjectBuilder = (T) MyEntities.CreateFromObjectBuilder(andRead, false);
      if ((object) fromObjectBuilder != null)
        MyEntities.Add((MyEntity) fromObjectBuilder);
      loadingDoneHandler(fromObjectBuilder);
    }

    protected override void OnHook()
    {
      this.m_physicsSync = this.CreatePhysicsGroup();
      this.m_onCloseAction = new Action<MyEntity>(this.OnClose);
      this.Instance.OnClose += this.m_onCloseAction;
      this.Instance.OnMarkForClose += new Action<MyEntity>(this.CheckClosing);
      if (!Sync.IsServer)
        return;
      this.Instance.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.Instance.PositionComp.OnLocalAABBChanged += new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
    }

    private void CheckClosing(MyEntity obj)
    {
      if (Sync.IsServer || this.m_destroyed)
        return;
      int num = MySession.Static.Ready ? 1 : 0;
    }

    private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
    {
      if (this.OnAABBChanged == null)
        return;
      this.OnAABBChanged((IMyReplicable) this);
    }

    private void OnClose(MyEntity ent) => this.RaiseDestroyed();

    protected virtual IMyStateGroup CreatePhysicsGroup() => (IMyStateGroup) new MyEntityPhysicsStateGroup((MyEntity) this.Instance, (IMyReplicable) this);

    public override IMyReplicable GetParent() => (IMyReplicable) null;

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      MyObjectBuilder_EntityBase objectBuilder;
      using (MyReplicationLayer.StartSerializingReplicable((IMyReplicable) this, clientEndpoint))
        objectBuilder = this.Instance.GetObjectBuilder(false);
      MySerializer.Write<MyObjectBuilder_EntityBase>(stream, ref objectBuilder, MyObjectBuilderSerializer.Dynamic);
      return true;
    }

    public override sealed void OnDestroyClient()
    {
      if ((object) this.Instance != null)
      {
        this.m_destroyed = true;
        this.OnDestroyClientInternal();
      }
      this.m_physicsSync = (IMyStateGroup) null;
    }

    protected virtual void OnDestroyClientInternal()
    {
      if ((this.Instance.PositionComp.GetPosition() - MySector.MainCamera.Position).LengthSquared() > 1000000.0)
        this.Instance.Render.FadeOut = true;
      this.Instance.Close();
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      if (this.m_physicsSync == null)
        return;
      resultList.Add(this.m_physicsSync);
    }

    public override bool HasToBeChild => false;

    public override BoundingBoxD GetAABB() => (object) this.Instance == null ? BoundingBoxD.CreateInvalid() : this.Instance.PositionComp.WorldAABB;

    public override HashSet<IMyReplicable> GetPhysicalDependencies(
      MyTimeSpan timeStamp,
      MyReplicablesBase replicables)
    {
      if (this.m_lastPhysicalDependencyUpdate != timeStamp && this.IncludeInIslands && this.CheckConsistency())
      {
        this.m_lastPhysicalDependencyUpdate = timeStamp;
        this.m_physicalDependencies.Clear();
        bool flag = true;
        BoundingBoxD aabb = this.GetAABB();
        while (flag)
        {
          flag = false;
          this.m_tmpReplicables.Clear();
          this.m_physicalDependencies.Add((IMyReplicable) this);
          replicables.GetReplicablesInBox(aabb.GetInflated(2.5), this.m_tmpReplicables);
          foreach (IMyReplicable tmpReplicable in this.m_tmpReplicables)
          {
            if (tmpReplicable.CheckConsistency() && !this.m_physicalDependencies.Contains(tmpReplicable) && tmpReplicable.IncludeInIslands)
            {
              this.m_physicalDependencies.Add(tmpReplicable);
              aabb.Include(tmpReplicable.GetAABB());
              flag = true;
            }
          }
        }
      }
      return this.m_physicalDependencies;
    }

    protected void TryRemoveExistingEntity(long entityId)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      entity.EntityId = MyEntityIdentifier.AllocateId();
      if (entity.MarkedForClose)
        return;
      entity.Close();
    }

    public override void OnReplication()
    {
      base.OnReplication();
      ++this.m_replicationCounter;
      if (this.m_replicationCounter != 1)
        return;
      this.Instance.OnReplicationStarted();
    }

    public override void OnUnreplication()
    {
      base.OnUnreplication();
      --this.m_replicationCounter;
      if (this.m_replicationCounter > 0)
        return;
      this.Instance.OnReplicationEnded();
    }
  }
}
