// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyPhysicalInventoryItemExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Inventory
{
  public static class MyPhysicalInventoryItemExtensions
  {
    private const float ITEM_SPAWN_RADIUS = 1f;
    private static List<HkBodyCollision> m_tmpCollisions = new List<HkBodyCollision>();

    public static void Spawn(
      this MyPhysicalInventoryItem thisItem,
      MyFixedPoint amount,
      BoundingBoxD box,
      MyEntity owner = null,
      Action<MyEntity> completionCallback = null)
    {
      if (amount < (MyFixedPoint) 0)
        return;
      MatrixD identity = MatrixD.Identity;
      identity.Translation = box.Center;
      thisItem.Spawn(amount, identity, owner, (Action<MyEntity>) (entity => MyPhysicalInventoryItemExtensions.InitSpawned(entity, box, completionCallback)));
    }

    private static void InitSpawned(
      MyEntity entity,
      BoundingBoxD box,
      Action<MyEntity> completionCallback)
    {
      if (entity == null)
        return;
      float radius = entity.PositionComp.LocalVolume.Radius;
      Vector3D vector3D = (Vector3D) Vector3.Max((Vector3) (box.Size / 2.0 - new Vector3(radius)), Vector3.Zero);
      box = new BoundingBoxD(box.Center - vector3D, box.Center + vector3D);
      Vector3D randomPosition = MyUtils.GetRandomPosition(ref box);
      Vector3 vector3Normalized1 = MyUtils.GetRandomVector3Normalized();
      Vector3 vector3Normalized2 = MyUtils.GetRandomVector3Normalized();
      while (vector3Normalized1 == vector3Normalized2)
        vector3Normalized2 = MyUtils.GetRandomVector3Normalized();
      Vector3 up = Vector3.Cross(Vector3.Cross(vector3Normalized1, vector3Normalized2), vector3Normalized1);
      entity.WorldMatrix = MatrixD.CreateWorld(randomPosition, vector3Normalized1, up);
      if (completionCallback == null)
        return;
      completionCallback(entity);
    }

    public static void Spawn(
      this MyPhysicalInventoryItem thisItem,
      MyFixedPoint amount,
      MatrixD worldMatrix,
      MyEntity owner,
      Action<MyEntity> completionCallback)
    {
      if (amount < (MyFixedPoint) 0 || thisItem.Content == null)
        return;
      if (thisItem.Content is MyObjectBuilder_BlockItem)
      {
        if (!typeof (MyObjectBuilder_CubeBlock).IsAssignableFrom((System.Type) thisItem.Content.GetObjectId().TypeId))
          return;
        MyObjectBuilder_BlockItem content = thisItem.Content as MyObjectBuilder_BlockItem;
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) content.BlockDefId, out blockDefinition);
        if (blockDefinition == null)
          return;
        MyObjectBuilder_CubeGrid newObject1 = MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) typeof (MyObjectBuilder_CubeGrid)) as MyObjectBuilder_CubeGrid;
        newObject1.GridSizeEnum = blockDefinition.CubeSize;
        newObject1.IsStatic = false;
        newObject1.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
        newObject1.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix));
        if (!(MyObjectBuilderSerializer.CreateNewObject(content.BlockDefId) is MyObjectBuilder_CubeBlock newObject))
          return;
        newObject.Min = (SerializableVector3I) (blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One);
        newObject1.CubeBlocks.Add(newObject);
        for (int index = 0; (MyFixedPoint) index < amount; ++index)
        {
          newObject1.EntityId = MyEntityIdentifier.AllocateId();
          newObject.EntityId = MyEntityIdentifier.AllocateId();
          MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) newObject1, true, completionCallback);
        }
      }
      else
      {
        MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
        if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(thisItem.Content.GetObjectId(), out definition))
          return;
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(amount, thisItem.Content), worldMatrix, owner?.Physics, completionCallback);
      }
    }

    public static MyDefinitionBase GetItemDefinition(
      this MyPhysicalInventoryItem thisItem)
    {
      if (thisItem.Content == null)
        return (MyDefinitionBase) null;
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) null;
      if (thisItem.Content is MyObjectBuilder_BlockItem)
      {
        SerializableDefinitionId blockDefId = (thisItem.Content as MyObjectBuilder_BlockItem).BlockDefId;
        MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
        if (MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) blockDefId, out blockDefinition))
          myDefinitionBase = (MyDefinitionBase) blockDefinition;
      }
      else
        myDefinitionBase = (MyDefinitionBase) MyDefinitionManager.Static.TryGetComponentBlockDefinition(thisItem.Content.GetId());
      MyPhysicalItemDefinition definition;
      if (myDefinitionBase == null && MyDefinitionManager.Static.TryGetPhysicalItemDefinition(thisItem.Content.GetId(), out definition))
        myDefinitionBase = (MyDefinitionBase) definition;
      return myDefinitionBase;
    }

    private static bool GetNonPenetratingTransformPosition(
      ref BoundingBox box,
      ref MatrixD transform)
    {
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in transform);
      Vector3 halfExtents = box.HalfExtents;
      try
      {
        for (int index = 0; index < 11; ++index)
        {
          float num = 0.3f * (float) index;
          Vector3D translation = transform.Translation + Vector3D.UnitY * (double) num;
          MyPhysicalInventoryItemExtensions.m_tmpCollisions.Clear();
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref fromRotationMatrix, MyPhysicalInventoryItemExtensions.m_tmpCollisions, 15);
          if (MyPhysicalInventoryItemExtensions.m_tmpCollisions.Count == 0)
          {
            transform.Translation = translation;
            return true;
          }
        }
        return false;
      }
      finally
      {
        MyPhysicalInventoryItemExtensions.m_tmpCollisions.Clear();
      }
    }

    private static void AddItemToLootBag(
      MyEntity itemOwner,
      MyPhysicalInventoryItem item,
      ref MyEntity lootBagEntity)
    {
      MyLootBagDefinition lootBagDefinition = MyDefinitionManager.Static.GetLootBagDefinition();
      if (lootBagDefinition == null)
        return;
      MyDefinitionBase itemDefinition = item.GetItemDefinition();
      if (itemDefinition == null)
        return;
      if (lootBagEntity == null && (double) lootBagDefinition.SearchRadius > 0.0)
      {
        Vector3D position = itemOwner.PositionComp.GetPosition();
        BoundingSphereD boundingSphere = new BoundingSphereD(position, (double) lootBagDefinition.SearchRadius);
        List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
        double num1 = double.MaxValue;
        foreach (MyEntity myEntity in entitiesInSphere)
        {
          if (!myEntity.MarkedForClose && myEntity.GetType() == typeof (MyEntity) && (myEntity.DefinitionId.HasValue && myEntity.DefinitionId.Value == lootBagDefinition.ContainerDefinition))
          {
            double num2 = (myEntity.PositionComp.GetPosition() - position).LengthSquared();
            if (num2 < num1)
            {
              lootBagEntity = myEntity;
              num1 = num2;
            }
          }
        }
        entitiesInSphere.Clear();
      }
      if (lootBagEntity == null || lootBagEntity.Components.Has<MyInventoryBase>() && !(lootBagEntity.Components.Get<MyInventoryBase>() as MyInventory).CanItemsBeAdded(item.Amount, itemDefinition.Id))
      {
        lootBagEntity = (MyEntity) null;
        MyContainerDefinition definition;
        if (MyComponentContainerExtension.TryGetContainerDefinition(lootBagDefinition.ContainerDefinition.TypeId, lootBagDefinition.ContainerDefinition.SubtypeId, out definition))
          lootBagEntity = MyPhysicalInventoryItemExtensions.SpawnBagAround(itemOwner, definition);
      }
      if (lootBagEntity == null || !(lootBagEntity.Components.Get<MyInventoryBase>() is MyInventory myInventory))
        return;
      if (itemDefinition is MyCubeBlockDefinition)
        myInventory.AddBlocks(itemDefinition as MyCubeBlockDefinition, item.Amount);
      else
        myInventory.AddItems(item.Amount, (MyObjectBuilder_Base) item.Content);
    }

    private static MyEntity SpawnBagAround(
      MyEntity itemOwner,
      MyContainerDefinition bagDefinition,
      int sideCheckCount = 3,
      int frontCheckCount = 2,
      int upCheckCount = 5,
      float stepSize = 1f)
    {
      Vector3D? nullable = new Vector3D?();
      MyModel myModel = (MyModel) null;
      foreach (MyContainerDefinition.DefaultComponent defaultComponent in bagDefinition.DefaultComponents)
      {
        if (typeof (MyObjectBuilder_ModelComponent).IsAssignableFrom((System.Type) defaultComponent.BuilderType))
        {
          MyComponentDefinitionBase componentDefinition1 = (MyComponentDefinitionBase) null;
          MyStringHash subtypeId = bagDefinition.Id.SubtypeId;
          if (defaultComponent.SubtypeId.HasValue)
            subtypeId = defaultComponent.SubtypeId.Value;
          if (MyComponentContainerExtension.TryGetComponentDefinition(defaultComponent.BuilderType, subtypeId, out componentDefinition1))
          {
            if (componentDefinition1 is MyModelComponentDefinition componentDefinition)
            {
              myModel = MyModels.GetModelOnlyData(componentDefinition.Model);
              break;
            }
            break;
          }
          break;
        }
      }
      if (myModel == null)
        return (MyEntity) null;
      float radius = myModel.BoundingBox.HalfExtents.Max();
      HkShape shape = (HkShape) new HkSphereShape(radius);
      try
      {
        Vector3D translation = itemOwner.PositionComp.WorldMatrixRef.Translation;
        float num1 = radius * stepSize;
        Vector3 vector2 = -MyGravityProviderSystem.CalculateNaturalGravityInPoint(itemOwner.PositionComp.WorldMatrixRef.Translation);
        if (vector2 == Vector3.Zero)
        {
          vector2 = Vector3.Up;
        }
        else
        {
          double num2 = (double) vector2.Normalize();
        }
        Vector3 result;
        vector2.CalculatePerpendicularVector(out result);
        Vector3 vector3_1 = Vector3.Cross(result, vector2);
        double num3 = (double) vector3_1.Normalize();
        Quaternion identity = Quaternion.Identity;
        Vector3[] vector3Array1 = new Vector3[4]
        {
          result,
          vector3_1,
          -result,
          -vector3_1
        };
        Vector3[] vector3Array2 = new Vector3[4]
        {
          vector3_1,
          -result,
          -vector3_1,
          result
        };
        for (int index1 = 0; index1 < vector3Array1.Length && !nullable.HasValue; ++index1)
        {
          Vector3 vector3_2 = vector3Array1[index1];
          Vector3 vector3_3 = vector3Array2[index1];
          for (int index2 = 0; index2 < frontCheckCount && !nullable.HasValue; ++index2)
          {
            Vector3D vector3D = translation + 0.25f * vector3_2 + radius * vector3_2 + (float) index2 * num1 * vector3_2 - 0.5f * (float) (sideCheckCount - 1) * num1 * vector3_3;
            for (int index3 = 0; index3 < sideCheckCount && !nullable.HasValue; ++index3)
            {
              for (int index4 = 0; index4 < upCheckCount && !nullable.HasValue; ++index4)
              {
                Vector3D position = vector3D + (float) index3 * num1 * vector3_3 + (float) index4 * num1 * vector2;
                if (MyEntities.IsInsideWorld(position) && !MyEntities.IsShapePenetrating(shape, ref position, ref identity))
                {
                  BoundingSphereD sphere = new BoundingSphereD(position, (double) radius);
                  if (MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere) == null)
                  {
                    nullable = new Vector3D?(position);
                    break;
                  }
                }
              }
            }
          }
        }
        if (!nullable.HasValue)
        {
          MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((BoundingBoxD) itemOwner.PositionComp.LocalAABB, itemOwner.PositionComp.WorldMatrixRef);
          Vector3D[] corners = new Vector3D[8];
          orientedBoundingBoxD.GetCorners(corners, 0);
          float val1 = float.MinValue;
          foreach (Vector3D vector3D in corners)
          {
            float val2 = Vector3.Dot((Vector3) (vector3D - orientedBoundingBoxD.Center), vector2);
            val1 = Math.Max(val1, val2);
          }
          nullable = new Vector3D?(itemOwner.PositionComp.WorldMatrixRef.Translation);
          if ((double) val1 > 0.0)
            nullable = new Vector3D?(orientedBoundingBoxD.Center + val1 * vector2);
        }
      }
      finally
      {
        shape.RemoveReference();
      }
      MatrixD worldMatrix = itemOwner.PositionComp.WorldMatrixRef;
      worldMatrix.Translation = nullable.Value;
      MyEntity definitionAndAdd = MyEntities.CreateFromComponentContainerDefinitionAndAdd(bagDefinition.Id, false);
      if (definitionAndAdd == null)
        return (MyEntity) null;
      definitionAndAdd.PositionComp.SetWorldMatrix(ref worldMatrix);
      definitionAndAdd.Physics.LinearVelocity = Vector3.Zero;
      definitionAndAdd.Physics.AngularVelocity = Vector3.Zero;
      return definitionAndAdd;
    }

    public static MyInventoryItem? MakeAPIItem(this MyPhysicalInventoryItem? item) => !item.HasValue ? new MyInventoryItem?() : new MyInventoryItem?(item.Value.MakeAPIItem());

    public static MyInventoryItem MakeAPIItem(this MyPhysicalInventoryItem item) => new MyInventoryItem((MyItemType) item.Content.GetObjectId(), item.ItemId, item.Amount);
  }
}
