// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyInventoryReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
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
  internal class MyInventoryReplicable : MyExternalReplicableEvent<MyInventory>
  {
    private MyPropertySyncStateGroup m_propertySync;
    private MyEntityInventoryStateGroup m_stateGroup;
    private long m_entityId;
    private int m_inventoryId;

    public override bool IsValid => this.Instance != null && this.Instance.Entity != null && !this.Instance.Entity.MarkedForClose;

    protected override void OnHook()
    {
      base.OnHook();
      if (this.Instance == null)
        return;
      this.m_stateGroup = new MyEntityInventoryStateGroup(this.Instance, Sync.IsServer, (IMyReplicable) this);
      this.Instance.BeforeRemovedFromContainer += (Action<MyEntityComponentBase>) (component => this.OnRemovedFromContainer());
      this.m_propertySync = new MyPropertySyncStateGroup((IMyReplicable) this, this.Instance.SyncType);
      if (this.Instance.Owner is MyCubeBlock owner)
      {
        owner.SlimBlock.CubeGridChanged += new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
        this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) owner.CubeGrid);
      }
      else
        this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Instance.Owner);
    }

    private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
    {
      this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) (this.Instance.Owner as MyCubeBlock).CubeGrid);
      (MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy((IMyReplicable) this);
    }

    public override IMyReplicable GetParent()
    {
      if (this.m_parent == null)
        this.m_parent = (IMyReplicable) MyExternalReplicable.FindByObject((object) this.Instance.Owner);
      return this.m_parent;
    }

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      long entityId = this.Instance.Owner.EntityId;
      MySerializer.Write<long>(stream, ref entityId);
      int num = 0;
      for (int index = 0; index < this.Instance.Owner.InventoryCount; ++index)
      {
        if (this.Instance == MyEntityExtensions.GetInventory(this.Instance.Owner, index))
        {
          num = index;
          break;
        }
      }
      MySerializer.Write<int>(stream, ref num);
      return true;
    }

    protected override void OnLoad(BitStream stream, Action<MyInventory> loadingDoneHandler)
    {
      if (stream != null)
      {
        MySerializer.CreateAndRead<long>(stream, out this.m_entityId);
        MySerializer.CreateAndRead<int>(stream, out this.m_inventoryId);
      }
      Sandbox.Game.Entities.MyEntities.CallAsync((Action) (() => this.LoadAsync(loadingDoneHandler)));
    }

    private void LoadAsync(Action<MyInventory> loadingDoneHandler)
    {
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_entityId, out entity);
      MyInventory myInventory = (MyInventory) null;
      MyEntity thisEntity = entity == null || !entity.HasInventory ? (MyEntity) null : entity;
      if (thisEntity != null && !thisEntity.GetTopMostParent((Type) null).MarkedForClose)
        myInventory = MyEntityExtensions.GetInventory(thisEntity, this.m_inventoryId);
      loadingDoneHandler(myInventory);
    }

    public override void OnDestroyClient()
    {
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
      if (this.m_stateGroup != null)
        resultList.Add((IMyStateGroup) this.m_stateGroup);
      resultList.Add((IMyStateGroup) this.m_propertySync);
    }

    public override string ToString() => string.Format("MyInventoryReplicable, Owner id: " + (this.Instance != null ? (this.Instance.Owner != null ? this.Instance.Owner.EntityId.ToString() : "<owner null>") : "<inventory null>"));

    private void OnRemovedFromContainer()
    {
      if (this.Instance == null || this.Instance.Owner == null)
        return;
      if (this.Instance.Owner is MyCubeBlock owner)
        owner.SlimBlock.CubeGridChanged -= new Action<MySlimBlock, MyCubeGrid>(this.OnBlockCubeGridChanged);
      this.RaiseDestroyed();
    }

    public override bool HasToBeChild => true;

    public override BoundingBoxD GetAABB() => BoundingBoxD.CreateInvalid();

    public override ValidationResult HasRights(
      EndpointId endpointId,
      ValidationType validationFlags)
    {
      MyExternalReplicable byObject = MyExternalReplicable.FindByObject((object) this.Instance.Owner);
      return byObject != null ? byObject.HasRights(endpointId, validationFlags) : base.HasRights(endpointId, validationFlags);
    }

    public void RefreshClientData(Endpoint currentSerializationDestinationEndpoint) => this.m_stateGroup.RefreshClientData(currentSerializationDestinationEndpoint);
  }
}
