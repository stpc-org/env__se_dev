// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDetectedEntityInfoHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI.Ingame;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public static class MyDetectedEntityInfoHelper
  {
    public static MyDetectedEntityInfo Create(
      MyEntity entity,
      long sensorOwner,
      Vector3D? hitPosition = null)
    {
      if (entity == null)
        return new MyDetectedEntityInfo();
      MatrixD orientation1 = MatrixD.Zero;
      Vector3 velocity = (Vector3) Vector3D.Zero;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      BoundingBoxD worldAabb = entity.PositionComp.WorldAABB;
      if (entity.Physics != null)
      {
        orientation1 = entity.Physics.GetWorldMatrix().GetOrientation();
        velocity = entity.Physics.LinearVelocity;
      }
      if (entity.GetTopMostParent((Type) null) is MyCubeGrid topMostParent)
      {
        MyDetectedEntityType type = topMostParent.GridSizeEnum != MyCubeSize.Small ? MyDetectedEntityType.LargeGrid : MyDetectedEntityType.SmallGrid;
        MyRelationsBetweenPlayerAndBlock relationship = topMostParent.BigOwners.Count != 0 ? MyIDModule.GetRelationPlayerBlock(sensorOwner, topMostParent.BigOwners[0], MyOwnershipShareModeEnum.Faction) : MyRelationsBetweenPlayerAndBlock.NoOwnership;
        string name = relationship == MyRelationsBetweenPlayerAndBlock.Owner || relationship == MyRelationsBetweenPlayerAndBlock.FactionShare || relationship == MyRelationsBetweenPlayerAndBlock.Friends ? topMostParent.DisplayName : (topMostParent.GridSizeEnum != MyCubeSize.Small ? MyTexts.GetString(MySpaceTexts.DetectedEntity_LargeGrid) : MyTexts.GetString(MySpaceTexts.DetectedEntity_SmallGrid));
        MatrixD orientation2 = topMostParent.WorldMatrix.GetOrientation();
        Vector3 linearVelocity = topMostParent.Physics.LinearVelocity;
        return new MyDetectedEntityInfo(topMostParent.EntityId, name, type, hitPosition, orientation2, linearVelocity, relationship, worldAabb, (long) timeInMilliseconds);
      }
      if (entity is MyCharacter myCharacter)
      {
        MyDetectedEntityType type = !myCharacter.IsPlayer ? MyDetectedEntityType.CharacterOther : MyDetectedEntityType.CharacterHuman;
        MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(sensorOwner, myCharacter.GetPlayerIdentityId(), MyOwnershipShareModeEnum.Faction);
        string name;
        switch (relationPlayerBlock)
        {
          case MyRelationsBetweenPlayerAndBlock.Owner:
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
          case MyRelationsBetweenPlayerAndBlock.Friends:
            name = myCharacter.DisplayNameText;
            break;
          default:
            name = !myCharacter.IsPlayer ? MyTexts.GetString(MySpaceTexts.DetectedEntity_CharacterOther) : MyTexts.GetString(MySpaceTexts.DetectedEntity_CharacterHuman);
            break;
        }
        BoundingBoxD boundingBox = myCharacter.Model.BoundingBox.Transform(myCharacter.WorldMatrix);
        return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation1, velocity, relationPlayerBlock, boundingBox, (long) timeInMilliseconds);
      }
      MyRelationsBetweenPlayerAndBlock relationship1 = MyRelationsBetweenPlayerAndBlock.Neutral;
      if (entity is MyFloatingObject myFloatingObject)
      {
        MyDetectedEntityType type = MyDetectedEntityType.FloatingObject;
        string subtypeName = myFloatingObject.Item.Content.SubtypeName;
        return new MyDetectedEntityInfo(entity.EntityId, subtypeName, type, hitPosition, orientation1, velocity, relationship1, worldAabb, (long) timeInMilliseconds);
      }
      if (entity is MyInventoryBagEntity inventoryBagEntity)
      {
        MyDetectedEntityType type = MyDetectedEntityType.FloatingObject;
        string displayName = inventoryBagEntity.DisplayName;
        return new MyDetectedEntityInfo(entity.EntityId, displayName, type, hitPosition, orientation1, velocity, relationship1, worldAabb, (long) timeInMilliseconds);
      }
      if (entity is MyPlanet myPlanet)
      {
        MyDetectedEntityType type = MyDetectedEntityType.Planet;
        string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Planet);
        BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(new BoundingSphereD(myPlanet.PositionComp.GetPosition(), (double) myPlanet.MaximumRadius));
        return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation1, velocity, relationship1, fromSphere, (long) timeInMilliseconds);
      }
      if (entity is MyVoxelPhysics myVoxelPhysics)
      {
        MyDetectedEntityType type = MyDetectedEntityType.Planet;
        string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Planet);
        BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(new BoundingSphereD(myVoxelPhysics.Parent.PositionComp.GetPosition(), (double) myVoxelPhysics.Parent.MaximumRadius));
        return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation1, velocity, relationship1, fromSphere, (long) timeInMilliseconds);
      }
      if (entity is MyVoxelMap)
      {
        MyDetectedEntityType type = MyDetectedEntityType.Asteroid;
        string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Asteroid);
        return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation1, velocity, relationship1, worldAabb, (long) timeInMilliseconds);
      }
      if (entity is MyMeteor)
      {
        MyDetectedEntityType type = MyDetectedEntityType.Meteor;
        string name = MyTexts.GetString(MySpaceTexts.DetectedEntity_Meteor);
        return new MyDetectedEntityInfo(entity.EntityId, name, type, hitPosition, orientation1, velocity, relationship1, worldAabb, (long) timeInMilliseconds);
      }
      if (!(entity is MyMissile))
        return new MyDetectedEntityInfo(0L, string.Empty, MyDetectedEntityType.Unknown, new Vector3D?(), new MatrixD(), new Vector3(), MyRelationsBetweenPlayerAndBlock.NoOwnership, new BoundingBoxD(), (long) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      MyDetectedEntityType type1 = MyDetectedEntityType.Missile;
      string displayName1 = entity.DisplayName;
      return new MyDetectedEntityInfo(entity.EntityId, displayName1, type1, hitPosition, orientation1, velocity, relationship1, worldAabb, (long) timeInMilliseconds);
    }
  }
}
