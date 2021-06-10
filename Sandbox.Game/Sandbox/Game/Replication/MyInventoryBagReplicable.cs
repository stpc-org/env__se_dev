// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyInventoryBagReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Replication.StateGroups;
using System;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication
{
  public class MyInventoryBagReplicable : MyEntityReplicableBase<MyInventoryBagEntity>
  {
    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      MyObjectBuilder_InventoryBagEntity objectBuilder;
      using (MyReplicationLayer.StartSerializingReplicable((IMyReplicable) this, clientEndpoint))
        objectBuilder = (MyObjectBuilder_InventoryBagEntity) this.Instance.GetObjectBuilder(false);
      if (string.IsNullOrEmpty(objectBuilder.SubtypeName) || MyInventoryBagEntity.GetPhysicsComponentBuilder(objectBuilder) == null)
        return false;
      MySerializer.Write<MyObjectBuilder_InventoryBagEntity>(stream, ref objectBuilder, MyObjectBuilderSerializer.Dynamic);
      return true;
    }

    protected override void OnLoad(
      BitStream stream,
      Action<MyInventoryBagEntity> loadingDoneHandler)
    {
      MyObjectBuilder_InventoryBagEntity andRead = (MyObjectBuilder_InventoryBagEntity) MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
      if (MyInventoryBagEntity.GetPhysicsComponentBuilder(andRead) == null)
        return;
      this.TryRemoveExistingEntity(andRead.EntityId);
      MyInventoryBagEntity objectBuilderAndAdd = (MyInventoryBagEntity) MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) andRead, false);
      loadingDoneHandler(objectBuilderAndAdd);
    }

    protected override IMyStateGroup CreatePhysicsGroup() => (IMyStateGroup) new MyEntityPhysicsStateGroup((MyEntity) this.Instance, (IMyReplicable) this);
  }
}
