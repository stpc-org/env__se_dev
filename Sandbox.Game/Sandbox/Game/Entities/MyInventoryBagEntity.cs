// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyInventoryBagEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_ReplicableEntity), false)]
  [MyEntityType(typeof (MyObjectBuilder_InventoryBagEntity), true)]
  public class MyInventoryBagEntity : MyEntity, IMyInventoryBag, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    private Vector3 m_gravity = Vector3.Zero;
    private MyDefinitionId m_definitionId;
    public long OwnerIdentityId;

    public Vector3 GeneratedGravity { get; set; }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (objectBuilder.EntityDefinitionId.HasValue && objectBuilder.EntityDefinitionId.Value.TypeId != typeof (MyObjectBuilder_InventoryBagEntity))
        objectBuilder.EntityDefinitionId = new SerializableDefinitionId?(new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_InventoryBagEntity), objectBuilder.EntityDefinitionId.Value.SubtypeName));
      base.Init(objectBuilder);
      if (!Sync.IsServer)
      {
        HkShape shape = this.Physics.RigidBody.GetShape();
        HkMassProperties hkMassProperties = new HkMassProperties();
        hkMassProperties.Mass = this.Physics.RigidBody.Mass;
        MyPhysicsBody physics = this.Physics as MyPhysicsBody;
        physics.Close();
        physics.ReportAllContacts = true;
        physics.Flags = RigidBodyFlag.RBF_STATIC;
        physics.CreateFromCollisionObject(shape, Vector3.Zero, this.WorldMatrix, new HkMassProperties?(hkMassProperties), 10);
        physics.RigidBody.ContactPointCallbackEnabled = true;
        physics.ContactPointCallback += new MyPhysicsBody.PhysicsContactHandler(this.OnPhysicsContactPointCallback);
      }
      this.Physics.Friction = 2f;
      switch (objectBuilder)
      {
        case MyObjectBuilder_InventoryBagEntity _:
          MyObjectBuilder_InventoryBagEntity builder = (MyObjectBuilder_InventoryBagEntity) objectBuilder;
          if (MyInventoryBagEntity.GetPhysicsComponentBuilder(builder) == null)
          {
            this.Physics.LinearVelocity = (Vector3) builder.LinearVelocity;
            this.Physics.AngularVelocity = (Vector3) builder.AngularVelocity;
          }
          if (builder != null)
          {
            this.OwnerIdentityId = builder.OwnerIdentityId;
            break;
          }
          break;
        case MyObjectBuilder_ReplicableEntity _:
          MyObjectBuilder_ReplicableEntity replicableEntity = (MyObjectBuilder_ReplicableEntity) objectBuilder;
          this.Physics.LinearVelocity = (Vector3) replicableEntity.LinearVelocity;
          this.Physics.AngularVelocity = (Vector3) replicableEntity.AngularVelocity;
          break;
      }
      this.OnClosing += new Action<MyEntity>(this.MyInventoryBagEntity_OnClosing);
    }

    internal static MyObjectBuilder_PhysicsComponentBase GetPhysicsComponentBuilder(
      MyObjectBuilder_InventoryBagEntity builder)
    {
      if (builder.ComponentContainer != null && builder.ComponentContainer.Components.Count > 0)
      {
        foreach (MyObjectBuilder_ComponentContainer.ComponentData component in builder.ComponentContainer.Components)
        {
          if (component.Component is MyObjectBuilder_PhysicsComponentBase)
            return component.Component as MyObjectBuilder_PhysicsComponentBase;
        }
      }
      return (MyObjectBuilder_PhysicsComponentBase) null;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
      if (!(objectBuilder is MyObjectBuilder_InventoryBagEntity inventoryBagEntity))
        return objectBuilder;
      inventoryBagEntity.OwnerIdentityId = this.OwnerIdentityId;
      return objectBuilder;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateGravity();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.Physics == null || this.Physics.IsStatic)
        return;
      this.Physics.RigidBody.Gravity = this.m_gravity + this.GeneratedGravity;
      this.GeneratedGravity = Vector3.Zero;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.UpdateGravity();
    }

    private void UpdateGravity()
    {
      if (this.Physics == null || this.Physics.IsStatic)
        return;
      this.m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.PositionComp.GetPosition());
    }

    private void MyInventoryBagEntity_OnClosing(MyEntity obj)
    {
      if (!Sync.IsServer)
        return;
      MyGps gpsByEntityId = MySession.Static.Gpss.GetGpsByEntityId(this.OwnerIdentityId, this.EntityId);
      if (gpsByEntityId == null)
        return;
      MySession.Static.Gpss.SendDelete(this.OwnerIdentityId, gpsByEntityId.Hash);
    }

    private void OnPhysicsContactPointCallback(ref MyPhysics.MyContactPointEvent e)
    {
      if ((double) this.Physics.LinearVelocity.LengthSquared() <= 225.0 || !(e.ContactPointEvent.GetOtherEntity((VRage.ModAPI.IMyEntity) this) is MyCharacter))
        return;
      e.ContactPointEvent.Base.Disable();
    }

    public override void OnReplicationStarted()
    {
      base.OnReplicationStarted();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted((MyEntity) this);
    }

    public override void OnReplicationEnded()
    {
      base.OnReplicationEnded();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded((MyEntity) this);
    }

    private class Sandbox_Game_Entities_MyInventoryBagEntity\u003C\u003EActor : IActivator, IActivator<MyInventoryBagEntity>
    {
      object IActivator.CreateInstance() => (object) new MyInventoryBagEntity();

      MyInventoryBagEntity IActivator<MyInventoryBagEntity>.CreateInstance() => new MyInventoryBagEntity();
    }
  }
}
