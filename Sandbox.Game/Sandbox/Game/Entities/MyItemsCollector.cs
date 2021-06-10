// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyItemsCollector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public static class MyItemsCollector
  {
    private static List<MyFracturedPiece> m_tmpFracturePieceList = new List<MyFracturedPiece>();
    private static List<MyEnvironmentItems.ItemInfo> m_tmpEnvItemList = new List<MyEnvironmentItems.ItemInfo>();
    private static List<MyItemsCollector.ItemInfo> m_tmpItemInfoList = new List<MyItemsCollector.ItemInfo>();
    private static List<MyItemsCollector.ComponentInfo> m_retvalBlockInfos = new List<MyItemsCollector.ComponentInfo>();
    private static List<MyItemsCollector.CollectibleInfo> m_retvalCollectibleInfos = new List<MyItemsCollector.CollectibleInfo>();

    public static bool FindClosestTreeInRadius(
      Vector3D fromPosition,
      float radius,
      out MyItemsCollector.ItemInfo result)
    {
      result = new MyItemsCollector.ItemInfo();
      BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, (double) radius);
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      double num1 = double.MaxValue;
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyTrees myTrees)
        {
          myTrees.GetPhysicalItemsInRadius(fromPosition, radius, MyItemsCollector.m_tmpEnvItemList);
          foreach (MyEnvironmentItems.ItemInfo tmpEnvItem in MyItemsCollector.m_tmpEnvItemList)
          {
            double num2 = Vector3D.DistanceSquared(fromPosition, tmpEnvItem.Transform.Position);
            if (num2 < num1)
            {
              result.ItemsEntityId = myEntity.EntityId;
              result.ItemId = tmpEnvItem.LocalId;
              result.Target = tmpEnvItem.Transform.Position;
              num1 = num2;
            }
          }
        }
      }
      entitiesInSphere.Clear();
      return num1 != double.MaxValue;
    }

    public static bool FindFallingTreeInRadius(
      Vector3D position,
      float radius,
      out MyItemsCollector.EntityInfo result)
    {
      result = new MyItemsCollector.EntityInfo();
      BoundingSphereD searchSphere = new BoundingSphereD(position, (double) radius);
      MyItemsCollector.m_tmpFracturePieceList.Clear();
      MyFracturedPiecesManager.Static.GetFracturesInSphere(ref searchSphere, ref MyItemsCollector.m_tmpFracturePieceList);
      foreach (MyFracturedPiece tmpFracturePiece in MyItemsCollector.m_tmpFracturePieceList)
      {
        if ((HkReferenceObject) tmpFracturePiece.Physics.RigidBody != (HkReferenceObject) null && tmpFracturePiece.Physics.RigidBody.IsActive && (!Vector3.IsZero(tmpFracturePiece.Physics.AngularVelocity) && !Vector3.IsZero(tmpFracturePiece.Physics.LinearVelocity)))
        {
          result.Target = Vector3D.Transform(tmpFracturePiece.Shape.CoM, tmpFracturePiece.PositionComp.WorldMatrixRef);
          result.EntityId = tmpFracturePiece.EntityId;
          MyItemsCollector.m_tmpFracturePieceList.Clear();
          return true;
        }
      }
      MyItemsCollector.m_tmpFracturePieceList.Clear();
      return false;
    }

    public static bool FindCollectableItemInRadius(
      Vector3D position,
      float radius,
      HashSet<MyDefinitionId> itemDefs,
      Vector3D initialPosition,
      float ignoreRadius,
      out MyItemsCollector.ComponentInfo result)
    {
      BoundingSphereD boundingSphere = new BoundingSphereD(position, (double) radius);
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      result = new MyItemsCollector.ComponentInfo();
      double num1 = double.MaxValue;
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
          if (myCubeGrid.BlocksCount == 1)
          {
            MySlimBlock block = myCubeGrid.CubeBlocks.First<MySlimBlock>();
            if (itemDefs.Contains(block.BlockDefinition.Id))
            {
              Vector3D world = myCubeGrid.GridIntegerToWorld(block.Position);
              if (Vector3D.DistanceSquared(world, initialPosition) > (double) ignoreRadius * (double) ignoreRadius)
              {
                double num2 = Vector3D.DistanceSquared(world, position);
                if (num2 < num1)
                {
                  num1 = num2;
                  result.EntityId = myCubeGrid.EntityId;
                  result.BlockPosition = block.Position;
                  result.ComponentDefinitionId = MyItemsCollector.GetComponentId(block);
                  result.IsBlock = true;
                }
              }
              else
                continue;
            }
          }
        }
        if (myEntity is MyFloatingObject)
        {
          MyFloatingObject myFloatingObject = myEntity as MyFloatingObject;
          MyDefinitionId id = myFloatingObject.Item.Content.GetId();
          if (itemDefs.Contains(id))
          {
            double num2 = Vector3D.DistanceSquared(myFloatingObject.PositionComp.WorldMatrixRef.Translation, position);
            if (num2 < num1)
            {
              num1 = num2;
              result.EntityId = myFloatingObject.EntityId;
              result.IsBlock = false;
            }
          }
        }
      }
      entitiesInSphere.Clear();
      return num1 != double.MaxValue;
    }

    public static List<MyItemsCollector.ComponentInfo> FindComponentsInRadius(
      Vector3D fromPosition,
      double radius)
    {
      BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, radius);
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      foreach (MyEntity entity in entitiesInSphere)
      {
        if (entity is MyFloatingObject)
        {
          MyFloatingObject myFloatingObject = entity as MyFloatingObject;
          if (myFloatingObject.Item.Content is MyObjectBuilder_Component)
            MyItemsCollector.m_retvalBlockInfos.Add(new MyItemsCollector.ComponentInfo()
            {
              EntityId = myFloatingObject.EntityId,
              BlockPosition = Vector3I.Zero,
              ComponentDefinitionId = myFloatingObject.Item.Content.GetObjectId(),
              IsBlock = false,
              ComponentCount = (int) myFloatingObject.Item.Amount
            });
        }
        else
        {
          MyCubeBlock block = (MyCubeBlock) null;
          MyCubeGrid asComponent = MyItemsCollector.TryGetAsComponent(entity, out block);
          if (asComponent != null)
            MyItemsCollector.m_retvalBlockInfos.Add(new MyItemsCollector.ComponentInfo()
            {
              IsBlock = true,
              EntityId = asComponent.EntityId,
              BlockPosition = block.Position,
              ComponentDefinitionId = MyItemsCollector.GetComponentId(block.SlimBlock),
              ComponentCount = block.BlockDefinition.Components == null ? 0 : block.BlockDefinition.Components[0].Count
            });
        }
      }
      entitiesInSphere.Clear();
      return MyItemsCollector.m_retvalBlockInfos;
    }

    public static List<MyItemsCollector.CollectibleInfo> FindCollectiblesInRadius(
      Vector3D fromPosition,
      double radius,
      bool doRaycast = false)
    {
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, radius);
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      foreach (MyEntity entity in entitiesInSphere)
      {
        bool flag1 = false;
        MyItemsCollector.CollectibleInfo collectibleInfo = new MyItemsCollector.CollectibleInfo();
        MyCubeBlock block1 = (MyCubeBlock) null;
        MyCubeGrid asComponent = MyItemsCollector.TryGetAsComponent(entity, out block1);
        if (asComponent != null)
        {
          collectibleInfo.EntityId = asComponent.EntityId;
          collectibleInfo.DefinitionId = MyItemsCollector.GetComponentId(block1.SlimBlock);
          collectibleInfo.Amount = block1.BlockDefinition.Components == null ? (MyFixedPoint) 0 : (MyFixedPoint) block1.BlockDefinition.Components[0].Count;
          flag1 = true;
        }
        else if (entity is MyFloatingObject)
        {
          MyFloatingObject myFloatingObject = entity as MyFloatingObject;
          MyDefinitionId objectId = myFloatingObject.Item.Content.GetObjectId();
          if (MyDefinitionManager.Static.GetPhysicalItemDefinition(objectId).Public)
          {
            collectibleInfo.EntityId = myFloatingObject.EntityId;
            collectibleInfo.DefinitionId = objectId;
            collectibleInfo.Amount = myFloatingObject.Item.Amount;
            flag1 = true;
          }
        }
        if (flag1)
        {
          bool flag2 = false;
          MyPhysics.CastRay(fromPosition, entity.WorldMatrix.Translation, toList, 15);
          foreach (MyPhysics.HitInfo hitInfo in toList)
          {
            IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
            if (hitEntity != entity)
            {
              switch (hitEntity)
              {
                case MyCharacter _:
                case MyFracturedPiece _:
                case MyFloatingObject _:
                  continue;
                default:
                  MyCubeBlock block2 = (MyCubeBlock) null;
                  if (MyItemsCollector.TryGetAsComponent(hitEntity as MyEntity, out block2) == null)
                  {
                    flag2 = true;
                    goto label_16;
                  }
                  else
                    continue;
              }
            }
          }
label_16:
          if (!flag2)
            MyItemsCollector.m_retvalCollectibleInfos.Add(collectibleInfo);
        }
      }
      entitiesInSphere.Clear();
      return MyItemsCollector.m_retvalCollectibleInfos;
    }

    public static MyCubeGrid TryGetAsComponent(
      MyEntity entity,
      out MyCubeBlock block,
      bool blockManipulatedEntity = true,
      Vector3D? hitPosition = null)
    {
      block = (MyCubeBlock) null;
      if (entity.MarkedForClose)
        return (MyCubeGrid) null;
      if (!(entity is MyCubeGrid myCubeGrid))
        return (MyCubeGrid) null;
      if (myCubeGrid.GridSizeEnum != MyCubeSize.Small)
        return (MyCubeGrid) null;
      MyCubeGrid myCubeGrid1 = (MyCubeGrid) null;
      if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && hitPosition.HasValue)
      {
        Vector3D vector3D = Vector3D.Transform(hitPosition.Value, myCubeGrid.PositionComp.WorldMatrixNormalizedInv);
        Vector3I cube;
        myCubeGrid.FixTargetCube(out cube, (Vector3) (vector3D / (double) myCubeGrid.GridSize));
        MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(cube);
        if (cubeBlock != null && cubeBlock.IsFullIntegrity)
          block = cubeBlock.FatBlock;
      }
      else
      {
        if (myCubeGrid.CubeBlocks.Count != 1)
          return (MyCubeGrid) null;
        if (myCubeGrid.IsStatic)
          return (MyCubeGrid) null;
        if (!MyCubeGrid.IsGridInCompleteState(myCubeGrid))
          return (MyCubeGrid) null;
        if (MyCubeGridSmallToLargeConnection.Static.TestGridSmallToLargeConnection(myCubeGrid))
          return (MyCubeGrid) null;
        HashSet<MySlimBlock>.Enumerator enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
        enumerator.MoveNext();
        block = enumerator.Current.FatBlock;
        enumerator.Dispose();
        myCubeGrid1 = myCubeGrid;
      }
      if (block == null)
        return (MyCubeGrid) null;
      if (!MyDefinitionManager.Static.IsComponentBlock(block.BlockDefinition.Id))
        return (MyCubeGrid) null;
      if (block.IsSubBlock)
        return (MyCubeGrid) null;
      DictionaryReader<string, MySlimBlock> subBlocks = block.GetSubBlocks();
      return subBlocks.IsValid && subBlocks.Count > 0 ? (MyCubeGrid) null : myCubeGrid1;
    }

    private static MyDefinitionId GetComponentId(MySlimBlock block)
    {
      MyCubeBlockDefinition.Component[] components = block.BlockDefinition.Components;
      return components == null || components.Length == 0 ? new MyDefinitionId() : components[0].Definition.Id;
    }

    private static bool IsFracturedTreeStump(MyFracturedPiece fracture)
    {
      Vector4 min;
      Vector4 max;
      fracture.Shape.GetShape().GetLocalAABB(0.0f, out min, out max);
      return (double) max.Y - (double) min.Y < 3.5 * ((double) max.X - (double) min.X);
    }

    private static bool FindClosestPointOnFracturedTree(
      Vector3D fromPositionFractureLocal,
      MyFracturedPiece fracture,
      out Vector3D closestPoint)
    {
      closestPoint = new Vector3D();
      if (fracture == null)
        return false;
      Vector4 min1;
      Vector4 max1;
      fracture.Shape.GetShape().GetLocalAABB(0.0f, out min1, out max1);
      Vector3D min2 = new Vector3D(min1);
      Vector3D max2 = new Vector3D(max1);
      closestPoint = Vector3D.Clamp(fromPositionFractureLocal, min2, max2);
      closestPoint.X = (closestPoint.X + 2.0 * (max2.X + min2.X) / 2.0) / 3.0;
      closestPoint.Y = MathHelper.Clamp(closestPoint.Y + 0.25 * (closestPoint.Y - min2.Y < max2.Y - closestPoint.Y ? 1.0 : -1.0), min2.Y, max2.Y);
      closestPoint.Z = (closestPoint.Z + 2.0 * (max2.Z + min2.Z) / 2.0) / 3.0;
      closestPoint = Vector3D.Transform(closestPoint, fracture.PositionComp.WorldMatrixRef);
      return true;
    }

    public struct ItemInfo
    {
      public Vector3D Target;
      public long ItemsEntityId;
      public int ItemId;
    }

    public struct EntityInfo
    {
      public Vector3D Target;
      public long EntityId;
    }

    public struct ComponentInfo
    {
      public long EntityId;
      public Vector3I BlockPosition;
      public MyDefinitionId ComponentDefinitionId;
      public int ComponentCount;
      public bool IsBlock;
    }

    public struct CollectibleInfo
    {
      public long EntityId;
      public MyDefinitionId DefinitionId;
      public MyFixedPoint Amount;
    }
  }
}
