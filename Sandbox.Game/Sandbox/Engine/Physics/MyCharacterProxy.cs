// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyCharacterProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Engine.Physics
{
  public class MyCharacterProxy
  {
    public const float MAX_SPRINT_SPEED = 7f;
    private bool m_isDynamic;
    private Vector3 m_gravity;
    private bool m_jump;
    private float m_posX;
    private float m_posY;
    private Vector3 m_angularVelocity;
    private float m_speed;
    private float m_maxSpeedRelativeToShip = 7f;
    private int m_airFrameCounter;
    private float m_mass;
    private float m_maxImpulse;
    private float m_maxCharacterSpeedSq;
    private bool m_isCrouching;
    private HkCharacterProxy CharacterProxy;
    private HkSimpleShapePhantom CharacterPhantom;
    private HkShape m_characterShape = HkShape.Empty;
    private HkShape m_crouchShape = HkShape.Empty;
    private HkShape m_characterCollisionShape = HkShape.Empty;
    private MyPhysicsBody m_physicsBody;
    private bool m_flyingStateEnabled;
    private HkRigidBody m_oldRigidBody;

    public event HkContactPointEventHandler ContactPointCallback;

    public HkCharacterRigidBody CharacterRigidBody { get; private set; }

    public static HkShape CreateCharacterShape(
      float height,
      float width,
      float headHeight,
      float headSize,
      float headForwardOffset,
      float downOffset = 0.0f,
      float upOffset = 0.0f,
      bool capsuleForHead = false)
    {
      HkCapsuleShape hkCapsuleShape = new HkCapsuleShape(Vector3.Up * (height - downOffset) / 2f, Vector3.Down * (height - upOffset) / 2f, width / 2f);
      if ((double) headSize <= 0.0)
        return (HkShape) hkCapsuleShape;
      HkConvexShape childShape = !capsuleForHead ? (HkConvexShape) new HkSphereShape(headSize) : (HkConvexShape) new HkCapsuleShape(new Vector3(0.0f, 0.0f, -0.3f), new Vector3(0.0f, 0.0f, 0.3f), headSize);
      HkShape[] shapes = new HkShape[2]
      {
        (HkShape) hkCapsuleShape,
        (HkShape) new HkConvexTranslateShape(childShape, Vector3.Up * (headHeight - downOffset) / 2f + Vector3.Forward * headForwardOffset, HkReferencePolicy.TakeOwnership)
      };
      return (HkShape) new HkListShape(shapes, shapes.Length, HkReferencePolicy.TakeOwnership);
    }

    public MyCharacterProxy(
      bool isDynamic,
      bool isCapsule,
      float characterWidth,
      float characterHeight,
      float crouchHeight,
      float ladderHeight,
      float headSize,
      float headHeight,
      Vector3 position,
      Vector3 up,
      Vector3 forward,
      float mass,
      MyPhysicsBody body,
      bool isOnlyVertical,
      float maxSlope,
      float maxImpulse,
      float maxSpeedRelativeToShip,
      float? maxForce = null,
      HkRagdoll ragDoll = null)
    {
      this.m_isDynamic = isDynamic;
      this.m_physicsBody = body;
      this.m_mass = mass;
      this.m_maxImpulse = maxImpulse;
      this.m_maxSpeedRelativeToShip = maxSpeedRelativeToShip;
      if (isCapsule)
      {
        this.m_characterShape = MyCharacterProxy.CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0.0f, upOffset: (-1.5f * MyPerGameSettings.PhysicsConvexRadius));
        this.m_characterCollisionShape = MyCharacterProxy.CreateCharacterShape(characterHeight * 0.9f, characterWidth * 0.9f, characterHeight * 0.9f + headHeight, headSize * 0.9f, 0.0f, upOffset: (-1.5f * MyPerGameSettings.PhysicsConvexRadius));
        this.m_crouchShape = MyCharacterProxy.CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0.0f, 1f, -1.5f * MyPerGameSettings.PhysicsConvexRadius);
        if (!this.m_isDynamic)
          this.CharacterPhantom = new HkSimpleShapePhantom(this.m_characterShape, 18);
      }
      else
      {
        HkBoxShape hkBoxShape = new HkBoxShape(new Vector3(characterWidth / 2f, characterHeight / 2f, characterWidth / 2f));
        if (!this.m_isDynamic)
          this.CharacterPhantom = new HkSimpleShapePhantom((HkShape) hkBoxShape, 18);
        this.m_characterShape = (HkShape) hkBoxShape;
      }
      if (!this.m_isDynamic)
      {
        HkCharacterProxyCinfo characterProxyCinfo = new HkCharacterProxyCinfo();
        characterProxyCinfo.StaticFriction = 1f;
        characterProxyCinfo.DynamicFriction = 1f;
        characterProxyCinfo.ExtraDownStaticFriction = 1000f;
        characterProxyCinfo.MaxCharacterSpeedForSolver = 10000f;
        characterProxyCinfo.RefreshManifoldInCheckSupport = true;
        characterProxyCinfo.Up = up;
        characterProxyCinfo.Forward = forward;
        characterProxyCinfo.UserPlanes = 4;
        characterProxyCinfo.MaxSlope = MathHelper.ToRadians(maxSlope);
        characterProxyCinfo.Position = position;
        characterProxyCinfo.CharacterMass = mass;
        characterProxyCinfo.CharacterStrength = 100f;
        characterProxyCinfo.ShapePhantom = (HkpShapePhantom) this.CharacterPhantom;
        this.CharacterProxy = new HkCharacterProxy(characterProxyCinfo);
        characterProxyCinfo.Dispose();
      }
      else
      {
        HkCharacterRigidBodyCinfo characterRigidBodyCinfo = new HkCharacterRigidBodyCinfo();
        characterRigidBodyCinfo.Shape = this.m_characterShape;
        characterRigidBodyCinfo.CrouchShape = this.m_crouchShape;
        characterRigidBodyCinfo.Friction = 0.0f;
        characterRigidBodyCinfo.MaxSlope = MathHelper.ToRadians(maxSlope);
        characterRigidBodyCinfo.Up = up;
        characterRigidBodyCinfo.Mass = mass;
        characterRigidBodyCinfo.CollisionFilterInfo = 18;
        characterRigidBodyCinfo.MaxLinearVelocity = 1000000f;
        characterRigidBodyCinfo.MaxForce = maxForce.HasValue ? maxForce.Value : 100000f;
        characterRigidBodyCinfo.AllowedPenetrationDepth = MyFakes.ENABLE_LIMITED_CHARACTER_BODY ? 0.3f : 0.1f;
        characterRigidBodyCinfo.JumpHeight = 0.8f;
        float maxCharacterSpeed = MyGridPhysics.ShipMaxLinearVelocity() + this.m_maxSpeedRelativeToShip;
        this.CharacterRigidBody = new HkCharacterRigidBody(characterRigidBodyCinfo, maxCharacterSpeed, (object) body);
        this.m_maxCharacterSpeedSq = maxCharacterSpeed * maxCharacterSpeed;
        this.CharacterRigidBody.GetRigidBody().ContactPointCallbackEnabled = true;
        this.CharacterRigidBody.GetRigidBody().ContactPointCallback -= new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
        this.CharacterRigidBody.GetRigidBody().ContactPointCallback += new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
        this.CharacterRigidBody.GetRigidBody().ContactPointCallbackDelay = 0;
        Matrix inertiaTensor = this.CharacterRigidBody.GetHitRigidBody().InertiaTensor;
        inertiaTensor.M11 = 1000f;
        inertiaTensor.M22 = 1000f;
        inertiaTensor.M33 = 1000f;
        this.CharacterRigidBody.GetHitRigidBody().InertiaTensor = inertiaTensor;
        characterRigidBodyCinfo.Dispose();
      }
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
    {
      if (this.ContactPointCallback == null)
        return;
      this.ContactPointCallback(ref value);
    }

    public void Dispose()
    {
      if ((HkReferenceObject) this.CharacterProxy != (HkReferenceObject) null)
      {
        this.CharacterProxy.Dispose();
        this.CharacterProxy = (HkCharacterProxy) null;
      }
      if ((HkReferenceObject) this.CharacterPhantom != (HkReferenceObject) null)
      {
        this.CharacterPhantom.Dispose();
        this.CharacterPhantom = (HkSimpleShapePhantom) null;
      }
      if ((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null)
      {
        if ((HkReferenceObject) this.CharacterRigidBody.GetRigidBody() != (HkReferenceObject) null)
          this.CharacterRigidBody.GetRigidBody().ContactPointCallback -= new HkContactPointEventHandler(this.RigidBody_ContactPointCallback);
        this.CharacterRigidBody.Dispose();
        this.CharacterRigidBody = (HkCharacterRigidBody) null;
      }
      this.m_characterShape.RemoveReference();
      this.m_characterCollisionShape.RemoveReference();
      this.m_crouchShape.RemoveReference();
    }

    public void SetCollisionFilterInfo(uint info)
    {
      if (!this.m_isDynamic)
        return;
      this.CharacterRigidBody.SetCollisionFilterInfo(info);
    }

    public void Activate(HkWorld world)
    {
      if ((HkReferenceObject) this.CharacterPhantom != (HkReferenceObject) null)
        world.AddPhantom((HkPhantom) this.CharacterPhantom);
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      world.AddCharacterRigidBody(this.CharacterRigidBody);
      if (float.IsInfinity(this.m_maxImpulse))
        return;
      world.BreakOffPartsUtil.MarkEntityBreakable(this.CharacterRigidBody.GetRigidBody(), this.m_maxImpulse);
    }

    public void Deactivate(HkWorld world)
    {
      if ((HkReferenceObject) this.CharacterPhantom != (HkReferenceObject) null)
        world.RemovePhantom((HkPhantom) this.CharacterPhantom);
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      world.RemoveCharacterRigidBody(this.CharacterRigidBody);
    }

    public Vector3 LinearVelocity
    {
      get => this.m_isDynamic ? this.CharacterRigidBody.LinearVelocity : this.CharacterProxy.LinearVelocity;
      set
      {
        if (this.m_isDynamic)
          this.CharacterRigidBody.LinearVelocity = value;
        else
          this.CharacterProxy.LinearVelocity = value;
      }
    }

    public Vector3 Forward => this.m_isDynamic ? this.CharacterRigidBody.Forward : this.CharacterProxy.Forward;

    public Vector3 Up => this.m_isDynamic ? this.CharacterRigidBody.Up : this.CharacterProxy.Up;

    public void SetForwardAndUp(Vector3 forward, Vector3 up)
    {
      Matrix rigidBodyTransform = this.GetRigidBodyTransform();
      rigidBodyTransform.Up = up;
      rigidBodyTransform.Forward = forward;
      rigidBodyTransform.Right = Vector3.Cross(forward, up);
      this.SetRigidBodyTransform(ref rigidBodyTransform);
    }

    public HkCharacterStateType GetState()
    {
      if (!this.m_isDynamic)
        return this.CharacterProxy.GetState();
      HkCharacterStateType characterStateType = this.CharacterRigidBody.GetState();
      if (characterStateType != HkCharacterStateType.HK_CHARACTER_ON_GROUND)
        ++this.m_airFrameCounter;
      if (characterStateType == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
        this.m_airFrameCounter = 0;
      if (characterStateType == HkCharacterStateType.HK_CHARACTER_IN_AIR && this.m_airFrameCounter < 8)
        characterStateType = HkCharacterStateType.HK_CHARACTER_ON_GROUND;
      if (this.AtLadder)
        characterStateType = HkCharacterStateType.HK_CHARACTER_CLIMBING;
      return characterStateType;
    }

    public void SetState(HkCharacterStateType state)
    {
      if (this.m_isDynamic)
        this.CharacterRigidBody.SetState(state);
      else
        this.CharacterProxy.SetState(state);
    }

    public Vector3 Gravity
    {
      get => this.m_gravity;
      set => this.m_gravity = value;
    }

    public float Elevate
    {
      get => this.m_isDynamic ? this.CharacterRigidBody.Elevate : 0.0f;
      set
      {
        if (!this.m_isDynamic)
          return;
        this.CharacterRigidBody.Elevate = value;
      }
    }

    public bool AtLadder
    {
      get => this.m_isDynamic && this.CharacterRigidBody.AtLadder;
      set
      {
        if (!this.m_isDynamic)
          return;
        this.CharacterRigidBody.AtLadder = value;
      }
    }

    public Vector3 ElevateVector
    {
      get => this.m_isDynamic ? this.CharacterRigidBody.ElevateVector : Vector3.Zero;
      set
      {
        if (!this.m_isDynamic)
          return;
        this.CharacterRigidBody.ElevateVector = value;
      }
    }

    public Vector3 ElevateUpVector
    {
      get => this.m_isDynamic ? this.CharacterRigidBody.ElevateUpVector : Vector3.Zero;
      set
      {
        if (!this.m_isDynamic)
          return;
        this.CharacterRigidBody.ElevateUpVector = value;
      }
    }

    public bool Jump
    {
      set => this.m_jump = value;
    }

    public Vector3 Position
    {
      get => this.m_isDynamic ? this.CharacterRigidBody.Position : this.CharacterProxy.Position;
      set
      {
        if (this.m_isDynamic)
          this.CharacterRigidBody.Position = value;
        else
          this.CharacterProxy.Position = value;
      }
    }

    public float PosX
    {
      set => this.m_posX = MathHelper.Clamp(value, -1f, 1f);
    }

    public float PosY
    {
      set => this.m_posY = MathHelper.Clamp(value, -1f, 1f);
    }

    public Vector3 AngularVelocity
    {
      set
      {
        this.m_angularVelocity = value;
        if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
          return;
        this.CharacterRigidBody.AngularVelocity = this.m_angularVelocity;
        this.CharacterRigidBody.SetAngularVelocity(this.m_angularVelocity);
      }
      get => !((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null) ? this.m_angularVelocity : this.CharacterRigidBody.GetAngularVelocity();
    }

    public float Speed
    {
      set => this.m_speed = value;
      get => this.m_speed;
    }

    public bool Supported { get; private set; }

    public Vector3 SupportNormal { get; private set; }

    public Vector3 GroundVelocity { get; private set; }

    public Vector3 GroundAngularVelocity { get; private set; }

    public void SetSupportedState(bool supported)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.SetPreviousSupportedState(supported);
    }

    public void StepSimulation(float stepSizeInSeconds)
    {
      if (this.AtLadder)
        return;
      if ((HkReferenceObject) this.CharacterProxy != (HkReferenceObject) null)
      {
        this.CharacterProxy.PosX = this.m_posX;
        this.CharacterProxy.PosY = this.m_posY;
        this.CharacterProxy.Jump = this.m_jump;
        this.m_jump = false;
        this.CharacterProxy.Gravity = this.m_gravity;
        this.CharacterProxy.StepSimulation(stepSizeInSeconds);
      }
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.PosX = this.m_posX;
      this.CharacterRigidBody.PosY = this.m_posY;
      this.CharacterRigidBody.Jump = this.m_jump;
      this.m_jump = false;
      this.CharacterRigidBody.Gravity = this.m_gravity;
      this.CharacterRigidBody.Speed = this.Speed;
      this.CharacterRigidBody.StepSimulation(stepSizeInSeconds);
      this.CharacterRigidBody.Elevate = this.Elevate;
      this.Supported = this.CharacterRigidBody.Supported;
      this.SupportNormal = this.CharacterRigidBody.SupportNormal;
      this.GroundVelocity = this.CharacterRigidBody.GroundVelocity;
      this.GroundAngularVelocity = this.CharacterRigidBody.AngularVelocity;
    }

    public void UpdateSupport(float stepSizeInSeconds)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.UpdateSupport(stepSizeInSeconds);
      this.Supported = this.CharacterRigidBody.Supported;
      this.SupportNormal = this.CharacterRigidBody.SupportNormal;
      this.GroundVelocity = this.CharacterRigidBody.GroundVelocity;
    }

    public void SkipSimulation(MatrixD mat)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.Position = (Vector3) mat.Translation;
      this.CharacterRigidBody.Forward = (Vector3) mat.Forward;
      this.CharacterRigidBody.Up = (Vector3) mat.Up;
      this.Supported = this.CharacterRigidBody.Supported;
      this.SupportNormal = this.CharacterRigidBody.SupportNormal;
      this.GroundVelocity = this.CharacterRigidBody.GroundVelocity;
    }

    public void EnableFlyingState(bool enable)
    {
      float maxCharacterSpeed = MyGridPhysics.ShipMaxLinearVelocity() + this.m_maxSpeedRelativeToShip;
      float maxFlyingSpeed = MyGridPhysics.ShipMaxLinearVelocity() + this.m_maxSpeedRelativeToShip;
      float maxAcceleration = 9f;
      this.m_physicsBody.ShapeChangeInProgress = true;
      this.EnableFlyingState(enable, maxCharacterSpeed, maxFlyingSpeed, maxAcceleration);
      this.m_physicsBody.ShapeChangeInProgress = false;
    }

    public void EnableFlyingState(
      bool enable,
      float maxCharacterSpeed,
      float maxFlyingSpeed,
      float maxAcceleration)
    {
      if (this.m_flyingStateEnabled == enable)
        return;
      if ((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null)
      {
        this.m_physicsBody.ShapeChangeInProgress = true;
        this.CharacterRigidBody.EnableFlyingState(enable, maxCharacterSpeed, maxFlyingSpeed, maxAcceleration);
        this.m_physicsBody.ShapeChangeInProgress = false;
      }
      this.StepSimulation(0.01666667f);
      this.m_flyingStateEnabled = enable;
    }

    public void EnableLadderState(bool enable) => this.EnableLadderState(enable, MyGridPhysics.ShipMaxLinearVelocity(), 1f);

    public void EnableLadderState(bool enable, float maxCharacterSpeed, float maxAcceleration)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.EnableLadderState(enable, maxCharacterSpeed, maxAcceleration);
    }

    public bool IsCrouching => this.m_isCrouching;

    public void SetShapeForCrouch(HkWorld world, bool enable)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null) || world == null)
        return;
      world.Lock();
      this.m_physicsBody.ShapeChangeInProgress = true;
      if (enable)
        this.CharacterRigidBody.SetShapeForCrouch();
      else
        this.CharacterRigidBody.SetDefaultShape();
      if (this.m_physicsBody.IsInWorld)
        world.ReintegrateCharacter(this.CharacterRigidBody);
      this.m_physicsBody.ShapeChangeInProgress = false;
      world.Unlock();
      this.m_isCrouching = enable;
    }

    public void ApplyLinearImpulse(Vector3 impulse)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.ApplyLinearImpulse(impulse);
    }

    public void ApplyAngularImpulse(Vector3 impulse)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.ApplyAngularImpulse(impulse);
    }

    public void ApplyGravity(Vector3 gravity)
    {
      this.CharacterRigidBody.LinearVelocity += gravity * 0.01666667f;
      if ((double) this.CharacterRigidBody.LinearVelocity.LengthSquared() <= (double) this.m_maxCharacterSpeedSq)
        return;
      Vector3 linearVelocity = this.CharacterRigidBody.LinearVelocity;
      float num1 = MyGridPhysics.ShipMaxLinearVelocity() + this.MaxSpeedRelativeToShip;
      double num2 = (double) linearVelocity.Normalize();
      this.CharacterRigidBody.LinearVelocity = linearVelocity * num1;
    }

    public bool ImmediateSetWorldTransform { get; set; }

    public void SetRigidBodyTransform(ref Matrix m)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.SetRigidBodyTransform(ref m);
    }

    public HkShape GetShape() => this.m_characterShape;

    public HkShape GetCollisionShape() => this.m_characterCollisionShape;

    public void SetSupportDistance(float distance)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.SetSupportDistance(distance);
    }

    public void SetHardSupportDistance(float distance)
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.SetHardSupportDistance(distance);
    }

    public bool ContactPointCallbackEnabled
    {
      set
      {
        if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
          return;
        this.CharacterRigidBody.ContactPointCallbackEnabled = value;
      }
      get => (HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null && this.CharacterRigidBody.ContactPointCallbackEnabled;
    }

    public MyPhysicsBody GetPhysicsBody() => this.m_physicsBody != null ? this.m_physicsBody : (MyPhysicsBody) null;

    public HkEntity GetRigidBody() => (HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null ? this.CharacterRigidBody.GetRigidBody() : (HkEntity) null;

    public HkRigidBody GetHitRigidBody() => (HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null ? this.CharacterRigidBody.GetHitRigidBody() : (HkRigidBody) null;

    public Matrix GetRigidBodyTransform() => (HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null ? this.CharacterRigidBody.GetRigidBodyTransform() : Matrix.Identity;

    public float Mass => this.m_mass;

    public float MaxSpeedRelativeToShip => this.m_maxSpeedRelativeToShip;

    public float CharacterFlyingMaxLinearVelocity() => this.m_maxSpeedRelativeToShip + MyGridPhysics.ShipMaxLinearVelocity();

    public float CharacterWalkingMaxLinearVelocity() => this.m_maxSpeedRelativeToShip + MyGridPhysics.ShipMaxLinearVelocity();

    public void GetSupportingEntities(List<MyEntity> outEntities)
    {
      if ((HkReferenceObject) this.CharacterRigidBody == (HkReferenceObject) null)
        return;
      foreach (HkEntity hkEntity in this.CharacterRigidBody.GetSupportInfo())
      {
        MyPhysicsBody userObject = (MyPhysicsBody) hkEntity.UserObject;
        if (userObject != null)
        {
          MyEntity entity = (MyEntity) userObject.Entity;
          if (entity != null && !entity.MarkedForClose)
            outEntities.Add(entity);
        }
      }
    }

    public void Stand()
    {
      if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
        return;
      this.CharacterRigidBody.ResetSurfaceVelocity();
    }

    public float MaxSlope
    {
      get => (HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null ? this.CharacterRigidBody.MaxSlope : 0.0f;
      set
      {
        if (!((HkReferenceObject) this.CharacterRigidBody != (HkReferenceObject) null))
          return;
        this.CharacterRigidBody.MaxSlope = value;
      }
    }
  }
}
