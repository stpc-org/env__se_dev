// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyInventoryBaseReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal class MyInventoryBaseReplicable : MyExternalReplicableEvent<MyInventoryBase>
  {
    private readonly Action<MyEntity> m_destroyEntity;
    private long m_entityId;
    private MyStringHash m_inventoryId;

    private MyInventoryBase Inventory => this.Instance;

    public override bool IsValid => this.Inventory != null && this.Inventory.Entity != null && !this.Inventory.Entity.MarkedForClose;

    public MyInventoryBaseReplicable() => this.m_destroyEntity = (Action<MyEntity>) (entity => this.RaiseDestroyed());

    protected override void OnHook()
    {
      base.OnHook();
      if (this.Inventory == null)
        return;
      ((MyEntity) this.Inventory.Entity).OnClose += this.m_destroyEntity;
      this.Inventory.BeforeRemovedFromContainer += (Action<MyEntityComponentBase>) (component => this.OnRemovedFromContainer());
      if (!(this.Inventory.Entity is MyCubeBlock entity))
        return;
      entity.SlimBlock.CubeGridChanged += new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
    }

    private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
    {
      if (!Sync.IsServer)
        return;
      (MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy((IMyReplicable) this);
    }

    public override IMyReplicable GetParent()
    {
      if (this.Inventory.Entity is MyCharacter)
        return (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Inventory.Entity);
      return this.Inventory.Entity is MyCubeBlock ? (IMyReplicable) MyExternalReplicable.FindByObject((object) (this.Inventory.Entity as MyCubeBlock).CubeGrid) : (IMyReplicable) null;
    }

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      long entityId = this.Inventory.Entity.EntityId;
      MySerializer.Write<long>(stream, ref entityId);
      MyStringHash inventoryId = this.Inventory.InventoryId;
      MySerializer.Write<MyStringHash>(stream, ref inventoryId);
      return true;
    }

    protected override void OnLoad(BitStream stream, Action<MyInventoryBase> loadingDoneHandler)
    {
      if (stream != null)
      {
        MySerializer.CreateAndRead<long>(stream, out this.m_entityId);
        MySerializer.CreateAndRead<MyStringHash>(stream, out this.m_inventoryId);
      }
      Sandbox.Game.Entities.MyEntities.CallAsync((Action) (() => this.LoadAsync(loadingDoneHandler)));
    }

    private void LoadAsync(Action<MyInventoryBase> loadingDoneHandler)
    {
      MyInventoryBase component = (MyInventoryBase) null;
      MyInventoryBase myInventoryBase = (MyInventoryBase) null;
      MyEntity entity;
      if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_entityId, out entity) && entity.Components.TryGet<MyInventoryBase>(out component) && component is MyInventoryAggregate)
        myInventoryBase = (component as MyInventoryAggregate).GetInventory(this.m_inventoryId);
      loadingDoneHandler(myInventoryBase ?? component);
    }

    public override void OnDestroyClient()
    {
      if (this.Inventory == null || this.Inventory.Entity == null)
        return;
      ((MyEntity) this.Inventory.Entity).OnClose -= this.m_destroyEntity;
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
    }

    private void OnRemovedFromContainer()
    {
      if (this.Inventory == null || this.Inventory.Entity == null)
        return;
      ((MyEntity) this.Inventory.Entity).OnClose -= this.m_destroyEntity;
      if (this.Inventory.Entity is MyCubeBlock entity)
        entity.SlimBlock.CubeGridChanged -= new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
      this.RaiseDestroyed();
    }

    public override bool HasToBeChild => true;

    public override BoundingBoxD GetAABB() => BoundingBoxD.CreateInvalid();
  }
}
