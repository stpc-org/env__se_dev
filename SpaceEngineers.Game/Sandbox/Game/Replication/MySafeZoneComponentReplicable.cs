// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MySafeZoneComponentReplicable
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using SpaceEngineers.Game.Entities.Blocks.SafeZone;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
  public class MySafeZoneComponentReplicable : MyExternalReplicableEvent<MySafeZoneComponent>
  {
    private readonly Action<MyEntity> m_destroyEntity;
    private long m_entityId;

    private MySafeZoneComponent SafeZoneManager => this.Instance;

    public override bool IsValid => this.SafeZoneManager != null && this.SafeZoneManager.Entity != null && !this.SafeZoneManager.Entity.MarkedForClose;

    public MySafeZoneComponentReplicable() => this.m_destroyEntity = (Action<MyEntity>) (entity => this.RaiseDestroyed());

    protected override void OnHook()
    {
      base.OnHook();
      if (this.SafeZoneManager == null)
        return;
      ((MyEntity) this.SafeZoneManager.Entity).OnClose += this.m_destroyEntity;
      this.SafeZoneManager.BeforeRemovedFromContainer += (Action<MyEntityComponentBase>) (component => this.OnRemovedFromContainer());
      if (!(this.SafeZoneManager.Entity is MyCubeBlock entity))
        return;
      entity.SlimBlock.CubeGridChanged += new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
    }

    private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
    {
      if (!Sync.IsServer)
        return;
      (MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy((IMyReplicable) this);
    }

    public override IMyReplicable GetParent() => this.SafeZoneManager.Entity is MyCubeBlock ? (IMyReplicable) MyExternalReplicable.FindByObject((object) (this.SafeZoneManager.Entity as MyCubeBlock)) : (IMyReplicable) null;

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      long entityId = this.SafeZoneManager.Entity.EntityId;
      MySerializer.Write<long>(stream, ref entityId);
      return true;
    }

    protected override void OnLoad(BitStream stream, Action<MySafeZoneComponent> loadingDoneHandler)
    {
      if (stream != null)
        MySerializer.CreateAndRead<long>(stream, out this.m_entityId);
      Sandbox.Game.Entities.MyEntities.CallAsync((Action) (() => loadingDoneHandler(this.FindComponent())));
    }

    private MySafeZoneComponent FindComponent()
    {
      MySafeZoneComponent component = (MySafeZoneComponent) null;
      MyEntity entity;
      return Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_entityId, out entity) && entity != null && entity.Components.TryGet<MySafeZoneComponent>(out component) ? component : (MySafeZoneComponent) null;
    }

    public override void OnDestroyClient()
    {
      if (this.SafeZoneManager == null || this.SafeZoneManager.Entity == null)
        return;
      ((MyEntity) this.SafeZoneManager.Entity).OnClose -= this.m_destroyEntity;
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
    }

    private void OnRemovedFromContainer()
    {
      if (this.SafeZoneManager == null || this.SafeZoneManager.Entity == null)
        return;
      ((MyEntity) this.SafeZoneManager.Entity).OnClose -= this.m_destroyEntity;
      if (this.SafeZoneManager.Entity is MyCubeBlock entity)
        entity.SlimBlock.CubeGridChanged -= new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
      this.RaiseDestroyed();
    }

    public override bool HasToBeChild => true;

    public override BoundingBoxD GetAABB() => BoundingBoxD.CreateInvalid();
  }
}
