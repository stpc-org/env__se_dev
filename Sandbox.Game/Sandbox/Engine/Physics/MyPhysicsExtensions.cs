// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyPhysicsExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Physics
{
  public static class MyPhysicsExtensions
  {
    [ThreadStatic]
    private static List<IMyEntity> m_entityList;

    public static void SetInBodySpace(
      this HkWheelConstraintData data,
      Vector3 posA,
      Vector3 posB,
      Vector3 axisA,
      Vector3 axisB,
      Vector3 suspension,
      Vector3 steering,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
        axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
        axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
        suspension = Vector3.TransformNormal(suspension, bodyB.WeldInfo.Transform);
        steering = Vector3.TransformNormal(steering, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref suspension, ref steering);
    }

    public static void SetInBodySpace(
      this HkCustomWheelConstraintData data,
      Vector3 posA,
      Vector3 posB,
      Vector3 axisA,
      Vector3 axisB,
      Vector3 suspension,
      Vector3 steering,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
        axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
        axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
        suspension = Vector3.TransformNormal(suspension, bodyB.WeldInfo.Transform);
        steering = Vector3.TransformNormal(steering, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref suspension, ref steering);
    }

    public static void SetInBodySpace(
      this HkLimitedHingeConstraintData data,
      Vector3 posA,
      Vector3 posB,
      Vector3 axisA,
      Vector3 axisB,
      Vector3 axisAPerp,
      Vector3 axisBPerp,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
        axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
        axisAPerp = Vector3.TransformNormal(axisAPerp, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
        axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
        axisBPerp = Vector3.TransformNormal(axisBPerp, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref axisAPerp, ref axisBPerp);
    }

    public static void SetInBodySpace(
      this HkHingeConstraintData data,
      Vector3 posA,
      Vector3 posB,
      Vector3 axisA,
      Vector3 axisB,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
        axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
        axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB);
    }

    public static void SetInBodySpace(
      this HkPrismaticConstraintData data,
      Vector3 posA,
      Vector3 posB,
      Vector3 axisA,
      Vector3 axisB,
      Vector3 axisAPerp,
      Vector3 axisBPerp,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        posA = Vector3.Transform(posA, bodyA.WeldInfo.Transform);
        axisA = Vector3.TransformNormal(axisA, bodyA.WeldInfo.Transform);
        axisAPerp = Vector3.TransformNormal(axisAPerp, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        posB = Vector3.Transform(posB, bodyB.WeldInfo.Transform);
        axisB = Vector3.TransformNormal(axisB, bodyB.WeldInfo.Transform);
        axisBPerp = Vector3.TransformNormal(axisBPerp, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref posA, ref posB, ref axisA, ref axisB, ref axisAPerp, ref axisBPerp);
    }

    public static void SetInBodySpace(
      this HkFixedConstraintData data,
      Matrix pivotA,
      Matrix pivotB,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA != null && bodyA.IsWelded)
        pivotA *= bodyA.WeldInfo.Transform;
      if (bodyB != null && bodyB.IsWelded)
        pivotB *= bodyB.WeldInfo.Transform;
      data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
    }

    public static void SetInBodySpace(
      this HkRopeConstraintData data,
      Vector3 pivotA,
      Vector3 pivotB,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
        pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
      if (bodyB.IsWelded)
        pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
      data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
    }

    public static void SetInBodySpace(
      this HkBallAndSocketConstraintData data,
      Vector3 pivotA,
      Vector3 pivotB,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
        pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
      if (bodyB.IsWelded)
        pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
      data.SetInBodySpaceInternal(ref pivotA, ref pivotB);
    }

    public static void SetInBodySpace(
      this HkCogWheelConstraintData data,
      Vector3 pivotA,
      Vector3 rotationA,
      float radius1,
      Vector3 pivotB,
      Vector3 rotationB,
      float radius2,
      MyPhysicsBody bodyA,
      MyPhysicsBody bodyB)
    {
      if (bodyA.IsWelded)
      {
        pivotA = Vector3.Transform(pivotA, bodyA.WeldInfo.Transform);
        rotationA = Vector3.TransformNormal(rotationA, bodyA.WeldInfo.Transform);
      }
      if (bodyB.IsWelded)
      {
        pivotB = Vector3.Transform(pivotB, bodyB.WeldInfo.Transform);
        rotationB = Vector3.TransformNormal(rotationB, bodyB.WeldInfo.Transform);
      }
      data.SetInBodySpaceInternal(ref pivotA, ref rotationA, radius1, ref pivotB, ref rotationB, radius2);
    }

    public static MyPhysicsBody GetBody(this HkEntity hkEntity) => !((HkReferenceObject) hkEntity != (HkReferenceObject) null) ? (MyPhysicsBody) null : hkEntity.UserObject as MyPhysicsBody;

    public static IMyEntity GetEntity(this HkEntity hkEntity, uint shapeKey)
    {
      MyPhysicsBody myPhysicsBody = hkEntity.GetBody();
      if (myPhysicsBody != null)
      {
        if (shapeKey == 0U)
          return myPhysicsBody.Entity;
        if ((long) shapeKey > (long) myPhysicsBody.WeldInfo.Children.Count)
          return myPhysicsBody.Entity;
        HkRigidBody rigidBody = myPhysicsBody.RigidBody;
        HkShape hkShape = rigidBody != null ? rigidBody.GetShape() : HkShape.Empty;
        if (hkShape.IsValid)
        {
          HkShape shape = hkShape.GetContainer().GetShape(shapeKey);
          if (shape.IsValid)
          {
            HkRigidBody hkEntity1 = HkRigidBody.FromShape(shape);
            myPhysicsBody = hkEntity1 != null ? hkEntity1.GetBody() : (MyPhysicsBody) null;
          }
        }
      }
      return myPhysicsBody?.Entity;
    }

    private static List<IMyEntity> EntityList
    {
      get
      {
        if (MyPhysicsExtensions.m_entityList == null)
          MyPhysicsExtensions.m_entityList = new List<IMyEntity>();
        return MyPhysicsExtensions.m_entityList;
      }
    }

    public static List<IMyEntity> GetAllEntities(this HkEntity hkEntity)
    {
      MyPhysicsBody body = hkEntity.GetBody();
      if (body != null)
      {
        MyPhysicsExtensions.EntityList.Add(body.Entity);
        foreach (MyPhysicsComponentBase child in body.WeldInfo.Children)
          MyPhysicsExtensions.EntityList.Add(child.Entity);
      }
      return MyPhysicsExtensions.EntityList;
    }

    public static IMyEntity GetSingleEntity(this HkEntity hkEntity) => hkEntity.GetBody()?.Entity;

    public static MyPhysicsBody GetOtherPhysicsBody(
      this HkContactPointEvent eventInfo,
      IMyEntity sourceEntity)
    {
      MyPhysicsBody physicsBody1 = eventInfo.GetPhysicsBody(0);
      MyPhysicsBody physicsBody2 = eventInfo.GetPhysicsBody(1);
      IMyEntity myEntity = physicsBody1 == null ? (IMyEntity) null : physicsBody1.Entity;
      if (physicsBody2 != null)
      {
        IMyEntity entity = physicsBody2.Entity;
      }
      return sourceEntity != myEntity ? physicsBody1 : physicsBody2;
    }

    public static IMyEntity GetOtherEntity(
      this HkCollisionEvent eventInfo,
      IMyEntity sourceEntity)
    {
      return eventInfo.GetOtherEntity(sourceEntity, out bool _);
    }

    public static IMyEntity GetOtherEntity(
      this HkContactPointEvent eventInfo,
      IMyEntity sourceEntity)
    {
      return eventInfo.Base.GetOtherEntity(sourceEntity, out bool _);
    }

    public static IMyEntity GetOtherEntity(
      this HkContactPointEvent eventInfo,
      IMyEntity sourceEntity,
      out bool AisThis)
    {
      return eventInfo.Base.GetOtherEntity(sourceEntity, out AisThis);
    }

    public static IMyEntity GetOtherEntity(
      this HkCollisionEvent eventInfo,
      IMyEntity sourceEntity,
      out bool AisThis)
    {
      MyPhysicsBody physicsBody1 = eventInfo.GetPhysicsBody(0);
      MyPhysicsBody physicsBody2 = eventInfo.GetPhysicsBody(1);
      IMyEntity myEntity1 = physicsBody1 == null ? (IMyEntity) null : physicsBody1.Entity;
      IMyEntity myEntity2 = physicsBody2 == null ? (IMyEntity) null : physicsBody2.Entity;
      if (sourceEntity == myEntity1)
      {
        AisThis = true;
        return myEntity2;
      }
      AisThis = false;
      return myEntity1;
    }

    public static MyPhysicsBody GetPhysicsBody(
      this HkContactPointEvent eventInfo,
      int index)
    {
      return eventInfo.Base.GetPhysicsBody(index);
    }

    public static MyPhysicsBody GetPhysicsBody(
      this HkCollisionEvent eventInfo,
      int index)
    {
      HkRigidBody rigidBody = eventInfo.GetRigidBody(index);
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return (MyPhysicsBody) null;
      MyPhysicsBody body = rigidBody.GetBody();
      if (body != null)
      {
        int num = body.IsWelded ? 1 : 0;
      }
      return body;
    }

    public static IMyEntity GetHitEntity(this HkHitInfo hitInfo)
    {
      HkRigidBody body = hitInfo.Body;
      return body == null ? (IMyEntity) null : body.GetEntity(hitInfo.GetShapeKey(0));
    }

    public static float GetConvexRadius(this HkHitInfo hitInfo)
    {
      if ((HkReferenceObject) hitInfo.Body == (HkReferenceObject) null)
        return 0.0f;
      HkShape shape = hitInfo.Body.GetShape();
      int index = 0;
      while (index < hitInfo.ShapeKeyCount && (uint.MaxValue != hitInfo.GetShapeKey(index) && shape.IsContainer()))
        ++index;
      if (shape.ShapeType == HkShapeType.ConvexTransform || shape.ShapeType == HkShapeType.ConvexTranslate || shape.ShapeType == HkShapeType.Transform)
        shape = shape.GetContainer().GetShape(0U);
      if (shape.ShapeType == HkShapeType.Sphere || shape.ShapeType == HkShapeType.Capsule)
        return 0.0f;
      return !shape.IsConvex ? HkConvexShape.DefaultConvexRadius : shape.ConvexRadius;
    }

    public static Vector3 GetFixedPosition(this MyPhysics.HitInfo hitInfo)
    {
      Vector3 position = (Vector3) hitInfo.Position;
      float convexRadius = hitInfo.HkHitInfo.GetConvexRadius();
      if ((double) convexRadius != 0.0)
        position += -hitInfo.HkHitInfo.Normal * convexRadius;
      return position;
    }

    public static IEnumerable<HkShape> GetAllShapes(this HkShape shape)
    {
      if (shape.IsContainer())
      {
        HkShapeContainerIterator iterator = shape.GetContainer();
        while (iterator.CurrentShapeKey != uint.MaxValue)
        {
          foreach (HkShape allShape in iterator.CurrentValue.GetAllShapes())
            yield return allShape;
          iterator.Next();
        }
      }
      else
        yield return shape;
    }

    public static unsafe HkShape GetHitShape(
      this HkShape shape,
      ref HkContactPointEvent contactEvent,
      int bodyIndex,
      bool checkMissingKeys = true,
      bool ImNotSureThatShapeKeysAreStillValid = false)
    {
      uint* numPtr = stackalloc uint[4];
      int num1 = 0;
      for (int shapeIdx = 0; shapeIdx < 4; ++shapeIdx)
      {
        numPtr[shapeIdx] = contactEvent.GetShapeKey(bodyIndex, shapeIdx);
        if (numPtr[shapeIdx] != uint.MaxValue)
          ++num1;
        else
          break;
      }
      for (int index = num1 - 1; index >= 0; --index)
      {
        uint num2 = numPtr[index];
        if (!shape.IsContainer())
        {
          shape = HkShape.Empty;
          break;
        }
        HkShapeContainerIterator container = shape.GetContainer();
        if (!container.IsValid)
        {
          shape = HkShape.Empty;
          break;
        }
        shape = !ImNotSureThatShapeKeysAreStillValid || container.IsShapeKeyValid(num2) ? container.GetShape(num2) : HkShape.Empty;
        if (shape.IsZero)
        {
          int num3 = checkMissingKeys ? 1 : 0;
          return HkShape.Empty;
        }
      }
      int num4 = shape.IsZero ? 1 : 0;
      return shape;
    }

    public static unsafe uint GetHitTriangleMaterial(
      this MyVoxelPhysicsBody voxelBody,
      ref HkContactPointEvent contactEvent,
      int bodyIndex)
    {
      uint* numPtr = stackalloc uint[4];
      int num = 0;
      for (int shapeIdx = 0; shapeIdx < 4; ++shapeIdx)
      {
        numPtr[shapeIdx] = contactEvent.GetShapeKey(bodyIndex, shapeIdx);
        if (numPtr[shapeIdx] != uint.MaxValue)
          ++num;
        else
          break;
      }
      if (num == 2)
      {
        HkShape shape = voxelBody.GetShape();
        if (!shape.IsZero && shape.ShapeType == HkShapeType.BvTree)
        {
          shape = shape.GetContainer().GetShape(numPtr[1]);
          if (!shape.IsZero && shape.ShapeType == HkShapeType.BvCompressedMesh)
          {
            uint userData = (uint) shape.UserData;
            if (userData != uint.MaxValue)
              return userData;
          }
        }
      }
      HkShape hitShape = voxelBody.GetShape().GetHitShape(ref contactEvent, bodyIndex, false, true);
      return hitShape.IsZero ? uint.MaxValue : (uint) hitShape.UserData;
    }

    public static IEnumerable<uint> GetShapeKeys(this HkShape shape)
    {
      if (shape.IsContainer())
      {
        HkShapeContainerIterator it = shape.GetContainer();
        while (it.IsValid)
        {
          yield return it.CurrentShapeKey;
          it.Next();
        }
        it = new HkShapeContainerIterator();
      }
    }

    public static IMyEntity GetCollisionEntity(this HkBodyCollision collision) => !((HkReferenceObject) collision.Body != (HkReferenceObject) null) ? (IMyEntity) null : collision.Body.GetEntity(0U);

    public static bool IsInWorldWelded(this MyPhysicsBody body)
    {
      if (body.IsInWorld)
        return true;
      return body.WeldInfo.Parent != null && body.WeldInfo.Parent.IsInWorld;
    }

    public static bool IsInWorldWelded(this MyPhysicsComponentBase body) => body != null && body is MyPhysicsBody && MyPhysicsExtensions.IsInWorldWelded((MyPhysicsBody) body);

    public static bool ActivateIfNeeded(this MyPhysicsComponentBase body)
    {
      if (body.IsActive || body.IsStatic)
        return false;
      body.ForceActivate();
      return true;
    }

    public static MyGridContactInfo.ContactFlags GetFlags(
      this HkContactPointProperties cp)
    {
      return (MyGridContactInfo.ContactFlags) cp.UserData.AsUint;
    }

    public static bool HasFlag(
      this HkContactPointProperties cp,
      MyGridContactInfo.ContactFlags flag)
    {
      return (uint) ((MyGridContactInfo.ContactFlags) cp.UserData.AsUint & flag) > 0U;
    }

    public static void SetFlag(
      this HkContactPointProperties cp,
      MyGridContactInfo.ContactFlags flag)
    {
      MyGridContactInfo.ContactFlags contactFlags = (MyGridContactInfo.ContactFlags) cp.UserData.AsUint | flag;
      cp.UserData = HkContactUserData.UInt((uint) contactFlags);
    }
  }
}
