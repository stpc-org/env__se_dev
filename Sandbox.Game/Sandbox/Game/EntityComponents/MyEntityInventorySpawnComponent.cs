// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityInventorySpawnComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentType(typeof (MyEntityInventorySpawnComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_InventorySpawnComponent), true)]
  public class MyEntityInventorySpawnComponent : MyEntityComponentBase
  {
    private MyDefinitionId m_containerDefinition;

    public override string ComponentTypeDebugString => "Inventory Spawn Component";

    public bool SpawnInventoryContainer(bool spawnAboveEntity = true)
    {
      if (MySession.Static == null || !MySession.Static.Ready)
        return false;
      MyEntity entity1 = this.Entity as MyEntity;
      for (int index1 = 0; index1 < entity1.InventoryCount; ++index1)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(entity1, index1);
        if (inventory != null && inventory.GetItemsCount() > 0)
        {
          MyEntity entity2 = this.Entity as MyEntity;
          MatrixD worldMatrix = entity2.WorldMatrix;
          if (spawnAboveEntity)
          {
            Vector3 v = -MyGravityProviderSystem.CalculateNaturalGravityInPoint(entity2.PositionComp.GetPosition());
            if (v == Vector3.Zero)
              v = Vector3.Up;
            double num = (double) v.Normalize();
            Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(v);
            Vector3D translation = worldMatrix.Translation;
            BoundingBoxD worldAabb = entity2.PositionComp.WorldAABB;
            for (int index2 = 0; index2 < 20; ++index2)
            {
              Vector3D vector3D = translation + 0.1f * (float) index2 * v + 0.1f * (float) index2 * perpendicularVector;
              if (!new BoundingBoxD(vector3D - 0.25 * Vector3D.One, vector3D + 0.25 * Vector3D.One).Intersects(ref worldAabb))
              {
                worldMatrix.Translation = vector3D + 0.25f * v;
                break;
              }
            }
            if (worldMatrix.Translation == translation)
              worldMatrix.Translation += v + perpendicularVector;
          }
          else if (entity2.Render.ModelStorage is MyModel modelStorage)
          {
            Vector3 vector3 = (Vector3) Vector3.Transform(modelStorage.BoundingBox.Center, worldMatrix);
            worldMatrix.Translation = (Vector3D) vector3;
          }
          MyContainerDefinition definition;
          if (!MyComponentContainerExtension.TryGetContainerDefinition(this.m_containerDefinition.TypeId, this.m_containerDefinition.SubtypeId, out definition))
            return false;
          MyEntity definitionAndAdd = MyEntities.CreateFromComponentContainerDefinitionAndAdd(definition.Id, false);
          if (definitionAndAdd == null)
            return false;
          definitionAndAdd.PositionComp.SetWorldMatrix(ref worldMatrix);
          if (entity2.InventoryCount == 1)
          {
            entity2.Components.Remove<MyInventoryBase>();
          }
          else
          {
            if (!(entity2.GetInventoryBase() is MyInventoryAggregate inventoryBase))
              return false;
            inventoryBase.RemoveComponent((MyComponentBase) inventory);
          }
          definitionAndAdd.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);
          inventory.RemoveEntityOnEmpty = true;
          definitionAndAdd.Physics.LinearVelocity = Vector3.Zero;
          definitionAndAdd.Physics.AngularVelocity = Vector3.Zero;
          if (entity1.Physics != null)
          {
            definitionAndAdd.Physics.LinearVelocity = entity1.Physics.LinearVelocity;
            definitionAndAdd.Physics.AngularVelocity = entity1.Physics.AngularVelocity;
          }
          else if (entity1 is MyCubeBlock)
          {
            MyCubeGrid cubeGrid = (entity1 as MyCubeBlock).CubeGrid;
            if (cubeGrid.Physics != null)
            {
              definitionAndAdd.Physics.LinearVelocity = cubeGrid.Physics.LinearVelocity;
              definitionAndAdd.Physics.AngularVelocity = cubeGrid.Physics.AngularVelocity;
            }
          }
          return true;
        }
      }
      return false;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (!Sync.IsServer)
        return;
      this.Entity.OnClosing += new Action<IMyEntity>(this.OnEntityClosing);
    }

    private void OnEntityClosing(IMyEntity obj)
    {
      MyEntity myEntity = obj as MyEntity;
      if (!myEntity.HasInventory || !myEntity.InScene)
        return;
      this.SpawnInventoryContainer();
    }

    public override bool IsSerialized() => true;

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (!Sync.IsServer)
        return;
      this.Entity.OnClosing -= new Action<IMyEntity>(this.OnEntityClosing);
    }

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      this.m_containerDefinition = (definition as MyEntityInventorySpawnComponent_Definition).ContainerDefinition;
    }

    private class Sandbox_Game_EntityComponents_MyEntityInventorySpawnComponent\u003C\u003EActor : IActivator, IActivator<MyEntityInventorySpawnComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityInventorySpawnComponent();

      MyEntityInventorySpawnComponent IActivator<MyEntityInventorySpawnComponent>.CreateInstance() => new MyEntityInventorySpawnComponent();
    }
  }
}
