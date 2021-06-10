// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyPhysicsHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Physics
{
  public static class MyPhysicsHelper
  {
    public static void InitSpherePhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      Vector3 sphereCenter,
      float sphereRadius,
      float mass,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      mass = (rbFlag & RigidBodyFlag.RBF_STATIC) != RigidBodyFlag.RBF_DEFAULT ? 0.0f : mass;
      MyPhysicsBody myPhysicsBody1 = new MyPhysicsBody(entity, rbFlag);
      myPhysicsBody1.MaterialType = materialType;
      myPhysicsBody1.AngularDamping = angularDamping;
      myPhysicsBody1.LinearDamping = linearDamping;
      MyPhysicsBody myPhysicsBody2 = myPhysicsBody1;
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(sphereRadius, mass);
      HkSphereShape hkSphereShape = new HkSphereShape(sphereRadius);
      myPhysicsBody2.CreateFromCollisionObject((HkShape) hkSphereShape, sphereCenter, entity.PositionComp.WorldMatrixRef, new HkMassProperties?(volumeMassProperties));
      hkSphereShape.Base.RemoveReference();
      entity.Physics = (MyPhysicsComponentBase) myPhysicsBody2;
    }

    public static void InitSpherePhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      MyModel model,
      float mass,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      entity.InitSpherePhysics(materialType, model.BoundingSphere.Center, model.BoundingSphere.Radius, mass, linearDamping, angularDamping, collisionLayer, rbFlag);
    }

    public static void InitBoxPhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      Vector3 center,
      Vector3 size,
      float mass,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      mass = (rbFlag & RigidBodyFlag.RBF_STATIC) != RigidBodyFlag.RBF_DEFAULT ? 0.0f : mass;
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
      MyPhysicsBody myPhysicsBody1 = new MyPhysicsBody(entity, rbFlag);
      myPhysicsBody1.MaterialType = materialType;
      myPhysicsBody1.AngularDamping = angularDamping;
      myPhysicsBody1.LinearDamping = linearDamping;
      MyPhysicsBody myPhysicsBody2 = myPhysicsBody1;
      HkBoxShape hkBoxShape = new HkBoxShape(size * 0.5f);
      myPhysicsBody2.CreateFromCollisionObject((HkShape) hkBoxShape, center, entity.PositionComp.WorldMatrixRef, new HkMassProperties?(volumeMassProperties));
      hkBoxShape.Base.RemoveReference();
      entity.Physics = (MyPhysicsComponentBase) myPhysicsBody2;
    }

    internal static void InitBoxPhysics(
      this IMyEntity entity,
      Matrix worldMatrix,
      MyStringHash materialType,
      Vector3 center,
      Vector3 size,
      float mass,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      mass = (rbFlag & RigidBodyFlag.RBF_STATIC) != RigidBodyFlag.RBF_DEFAULT ? 0.0f : mass;
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(size / 2f, mass);
      MyPhysicsBody myPhysicsBody1 = new MyPhysicsBody((IMyEntity) null, rbFlag);
      myPhysicsBody1.MaterialType = materialType;
      myPhysicsBody1.AngularDamping = angularDamping;
      myPhysicsBody1.LinearDamping = linearDamping;
      MyPhysicsBody myPhysicsBody2 = myPhysicsBody1;
      HkBoxShape hkBoxShape = new HkBoxShape(size * 0.5f);
      myPhysicsBody2.CreateFromCollisionObject((HkShape) hkBoxShape, center, (MatrixD) ref worldMatrix, new HkMassProperties?(volumeMassProperties));
      hkBoxShape.Base.RemoveReference();
      entity.Physics = (MyPhysicsComponentBase) myPhysicsBody2;
    }

    public static void InitBoxPhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      MyModel model,
      float mass,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      Vector3 center = model.BoundingBox.Center;
      Vector3 boundingBoxSize = model.BoundingBoxSize;
      entity.InitBoxPhysics(materialType, center, boundingBoxSize, mass, 0.0f, angularDamping, collisionLayer, rbFlag);
    }

    public static void InitCharacterPhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      Vector3 center,
      float characterWidth,
      float characterHeight,
      float crouchHeight,
      float ladderHeight,
      float headSize,
      float headHeight,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag,
      float mass,
      bool isOnlyVertical,
      float maxSlope,
      float maxImpulse,
      float maxSpeedRelativeToShip,
      bool networkProxy,
      float? maxForce)
    {
      MyPhysicsBody myPhysicsBody1 = new MyPhysicsBody(entity, rbFlag);
      myPhysicsBody1.MaterialType = materialType;
      myPhysicsBody1.AngularDamping = angularDamping;
      myPhysicsBody1.LinearDamping = linearDamping;
      MyPhysicsBody myPhysicsBody2 = myPhysicsBody1;
      myPhysicsBody2.CreateCharacterCollision(center, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, entity.PositionComp.WorldMatrixRef, mass, collisionLayer, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, networkProxy, maxForce);
      entity.Physics = (MyPhysicsComponentBase) myPhysicsBody2;
    }

    public static void InitCapsulePhysics(
      this IMyEntity entity,
      MyStringHash materialType,
      Vector3 vertexA,
      Vector3 vertexB,
      float radius,
      float mass,
      float linearDamping,
      float angularDamping,
      ushort collisionLayer,
      RigidBodyFlag rbFlag)
    {
      mass = (rbFlag & RigidBodyFlag.RBF_STATIC) != RigidBodyFlag.RBF_DEFAULT ? 0.0f : mass;
      MyPhysicsBody myPhysicsBody1 = new MyPhysicsBody(entity, rbFlag);
      myPhysicsBody1.MaterialType = materialType;
      myPhysicsBody1.AngularDamping = angularDamping;
      myPhysicsBody1.LinearDamping = linearDamping;
      MyPhysicsBody myPhysicsBody2 = myPhysicsBody1;
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(radius, mass);
      myPhysicsBody2.ReportAllContacts = true;
      HkCapsuleShape hkCapsuleShape = new HkCapsuleShape(vertexA, vertexB, radius);
      myPhysicsBody2.CreateFromCollisionObject((HkShape) hkCapsuleShape, (vertexA + vertexB) / 2f, entity.PositionComp.WorldMatrixRef, new HkMassProperties?(volumeMassProperties));
      hkCapsuleShape.Base.RemoveReference();
      entity.Physics = (MyPhysicsComponentBase) myPhysicsBody2;
    }

    public static bool InitModelPhysics(
      this IMyEntity entity,
      RigidBodyFlag rbFlags = RigidBodyFlag.RBF_KINEMATIC,
      int collisionLayers = 17)
    {
      MyEntity myEntity = entity as MyEntity;
      if (myEntity.Closed || myEntity.ModelCollision.HavokCollisionShapes == null || myEntity.ModelCollision.HavokCollisionShapes.Length == 0)
        return false;
      HkShape[] havokCollisionShapes = myEntity.ModelCollision.HavokCollisionShapes;
      HkListShape hkListShape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
      myEntity.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((IMyEntity) myEntity, rbFlags);
      myEntity.Physics.IsPhantom = false;
      (myEntity.Physics as MyPhysicsBody).CreateFromCollisionObject((HkShape) hkListShape, (Vector3) Vector3D.Zero, myEntity.WorldMatrix, collisionFilter: collisionLayers);
      myEntity.Physics.Enabled = true;
      hkListShape.Base.RemoveReference();
      return true;
    }
  }
}
