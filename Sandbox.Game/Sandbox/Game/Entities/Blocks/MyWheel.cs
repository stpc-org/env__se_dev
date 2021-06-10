// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyWheel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Wheel))]
  public class MyWheel : MyMotorRotor, Sandbox.ModAPI.IMyWheel, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor, Sandbox.ModAPI.Ingame.IMyWheel, IMyTrackTrails
  {
    private readonly MyStringHash m_wheelStringHash = MyStringHash.GetOrCompute("Wheel");
    private float m_terrainMaterialDistance = 0.95f;
    private float m_xDecalOffset;
    private float m_yDecalOffset;
    private MyWheelModelsDefinition m_cachedModelsDefinition;
    private Vector3D m_wheelCenterToTrail = Vector3D.Zero;
    private const float DECALSIZE_SPACING_FACTOR = 1.95f;
    private const float GROUND_NORMAL_SMOOTH_FACTOR = 1.5f;
    public Vector3 LastUsedGroundNormal;
    private int m_modelSwapCountUp;
    private bool m_usesAlternativeModel;
    public bool m_isSuspensionMounted;
    private int m_slipCountdown;
    private int m_staticHitCount;
    private int m_contactCountdown;
    private float m_frictionCollector;
    private Vector3 m_lastFrameImpuse;
    private ConcurrentNormalAggregator m_contactNormals = new ConcurrentNormalAggregator(10);
    private readonly VRage.Sync.Sync<MyWheel.ParticleData, SyncDirection.FromServer> m_particleData;
    private readonly VRage.Sync.Sync<MyWheel.TrailContactProperties, SyncDirection.FromServer> m_contactPointTrail;
    private static Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>> activityLog = new Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>>();
    private bool m_eachUpdateCallbackRegistered;

    public float Friction { get; set; }

    public ulong LastContactFrameNumber { get; private set; }

    private MyRenderComponentWheel Render
    {
      get => base.Render as MyRenderComponentWheel;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public ulong FramesSinceLastContact => MySandboxGame.Static.SimulationFrameCounter - this.LastContactFrameNumber;

    public DateTime LastContactTime { get; set; }

    private MyWheelModelsDefinition WheelModelsDefinition
    {
      get
      {
        if (this.m_cachedModelsDefinition == null)
        {
          string subtypeName = this.BlockDefinition.Id.SubtypeName;
          DictionaryReader<string, MyWheelModelsDefinition> modelDefinitions = MyDefinitionManager.Static.GetWheelModelDefinitions();
          if (!modelDefinitions.TryGetValue(subtypeName, out this.m_cachedModelsDefinition))
          {
            MyDefinitionManager.Static.AddMissingWheelModelDefinition(subtypeName);
            this.m_cachedModelsDefinition = modelDefinitions[subtypeName];
          }
        }
        return this.m_cachedModelsDefinition;
      }
    }

    public bool IsConsideredInContactWithStaticSurface => this.m_staticHitCount > 0 || this.m_contactCountdown > 0;

    public MyTrailProperties LastTrail { get; set; }

    public MyWheel()
    {
      this.Friction = 1.5f;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyWheel_IsWorkingChanged);
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.Render = new MyRenderComponentWheel();
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      if (builder is MyObjectBuilder_Wheel objectBuilderWheel && !objectBuilderWheel.YieldLastComponent)
        this.SlimBlock.DisableLastComponentYield();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_particleData.Value = new MyWheel.ParticleData()
        {
          EffectName = "",
          PositionRelative = Vector3.Zero,
          Normal = Vector3.Forward
        };
        this.m_contactPointTrail.Value = new MyWheel.TrailContactProperties()
        {
          ContactEntityId = -1L
        };
        if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
          this.m_contactPointTrail.ValueChanged += new Action<SyncBase>(this.ProcessTrails);
      }
      else
      {
        this.m_particleData.ValueChanged += new Action<SyncBase>(this.m_particleData_ValueChanged);
        this.m_contactPointTrail.ValueChanged += new Action<SyncBase>(this.ProcessTrails);
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void m_particleData_ValueChanged(SyncBase obj)
    {
      this.LastContactTime = DateTime.UtcNow;
      string effectName = this.m_particleData.Value.EffectName;
      Vector3D position = this.PositionComp.WorldMatrixRef.Translation + this.m_particleData.Value.PositionRelative;
      Vector3 normal = this.m_particleData.Value.Normal;
      if (this.Render == null)
        return;
      this.Render.TrySpawnParticle(effectName, ref position, ref normal);
      this.Render.UpdateParticle(ref position, ref normal);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CubeBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy);
      if (!(builderCubeBlock is MyObjectBuilder_Wheel objectBuilderWheel))
        return builderCubeBlock;
      objectBuilderWheel.YieldLastComponent = this.SlimBlock.YieldLastComponent;
      return builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.CubeGrid.Physics == null)
        return;
      this.CubeGrid.Physics.RigidBody.CallbackLimit = 1;
      this.CubeGrid.Physics.RigidBody.CollisionAddedCallback += new HkCollisionEventHandler(this.RigidBody_CollisionAddedCallback);
      this.CubeGrid.Physics.RigidBody.CollisionRemovedCallback += new HkCollisionEventHandler(this.RigidBody_CollisionRemovedCallback);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.CubeGrid.Physics == null)
        return;
      this.CubeGrid.Physics.RigidBody.CollisionAddedCallback -= new HkCollisionEventHandler(this.RigidBody_CollisionAddedCallback);
      this.CubeGrid.Physics.RigidBody.CollisionRemovedCallback -= new HkCollisionEventHandler(this.RigidBody_CollisionRemovedCallback);
    }

    private bool IsAcceptableContact(HkRigidBody rb)
    {
      object userObject = rb.UserObject;
      if (userObject == null || userObject == this.CubeGrid.Physics)
        return false;
      switch (userObject)
      {
        case MyVoxelPhysicsBody _:
          return true;
        case MyGridPhysics myGridPhysics:
          if (myGridPhysics.IsStatic)
            return true;
          break;
      }
      return false;
    }

    private void RigidBody_CollisionAddedCallback(ref HkCollisionEvent e)
    {
      MyGridPhysics physics = this.CubeGrid.Physics;
      if (!this.IsAcceptableContact(e.BodyA) && !this.IsAcceptableContact(e.BodyB))
        return;
      this.m_contactCountdown = 30;
      Interlocked.Increment(ref this.m_staticHitCount);
      this.RegisterPerFrameUpdate();
    }

    private void RigidBody_CollisionRemovedCallback(ref HkCollisionEvent e)
    {
      MyGridPhysics physics = this.CubeGrid.Physics;
      if (!this.IsAcceptableContact(e.BodyA) && !this.IsAcceptableContact(e.BodyB) || Interlocked.Decrement(ref this.m_staticHitCount) >= 0)
        return;
      Interlocked.Increment(ref this.m_staticHitCount);
    }

    private void MyWheel_IsWorkingChanged(MyCubeBlock obj)
    {
      if (this.Stator == null)
        return;
      this.Stator.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.Render == null)
        return;
      this.Render.UpdatePosition();
    }

    public override void ContactPointCallback(ref MyGridContactInfo value)
    {
      Vector3 contactNormal = value.Event.ContactPoint.Normal;
      this.m_contactNormals.PushNext(ref contactNormal);
      MyVoxelMaterialDefinition voxelSurfaceMaterial = value.VoxelSurfaceMaterial;
      if (voxelSurfaceMaterial != null)
        this.m_frictionCollector = voxelSurfaceMaterial.Friction;
      float friction = this.Friction;
      if (this.m_isSuspensionMounted && value.CollidingEntity is MyCubeGrid && (value.OtherBlock != null && value.OtherBlock.FatBlock == null))
      {
        friction *= 0.07f;
        this.m_frictionCollector = 0.7f;
      }
      HkContactPointProperties contactProperties = value.Event.ContactProperties;
      contactProperties.Friction = friction;
      contactProperties.Restitution = 0.5f;
      value.EnableParticles = false;
      value.RubberDeformation = true;
      ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
      Vector3D contactPosition = value.ContactPosition;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.CanProcessTrails(value, voxelSurfaceMaterial))
        this.m_contactPointTrail.Value = new MyWheel.TrailContactProperties()
        {
          ContactEntityId = value.CollidingEntity.EntityId,
          ContactNormal = value.Event.ContactPoint.Normal,
          ContactPosition = (Vector3) value.ContactPosition,
          VoxelMaterial = voxelSurfaceMaterial.Id.SubtypeId,
          PhysicalMaterial = voxelSurfaceMaterial.MaterialTypeNameHash
        };
      else
        this.LastTrail = (MyTrailProperties) null;
      if ((long) simulationFrameCounter == (long) this.LastContactFrameNumber)
        return;
      this.LastContactFrameNumber = simulationFrameCounter;
      Vector3 normal;
      if (this.m_contactNormals.GetAvgNormalCached(out normal))
        contactNormal = normal;
      string effectName = (string) null;
      if (value.CollidingEntity is MyVoxelBase && MyFakes.ENABLE_DRIVING_PARTICLES)
      {
        if (voxelSurfaceMaterial != null)
        {
          MyStringHash materialTypeNameHash = voxelSurfaceMaterial.MaterialTypeNameHash;
          effectName = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, this.m_wheelStringHash, materialTypeNameHash);
        }
      }
      else if (value.CollidingEntity is MyCubeGrid && MyFakes.ENABLE_DRIVING_PARTICLES)
      {
        MyStringHash materialAt = (value.CollidingEntity as MyCubeGrid).Physics.GetMaterialAt(contactPosition);
        effectName = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, this.m_wheelStringHash, materialAt);
      }
      MySandboxGame.Static.Invoke((Action) (() => this.UpdateEffect(effectName, contactPosition, contactNormal)), " MyWheel.ContactPointCallback");
      this.RegisterPerFrameUpdate();
    }

    private bool CanProcessTrails(
      MyGridContactInfo value,
      MyVoxelMaterialDefinition voxelSurfaceMaterial)
    {
      return this.CubeGrid.Physics != null && voxelSurfaceMaterial != null && !MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS && value.CollidingEntity is MyVoxelBase;
    }

    private void ProcessTrails(SyncBase obj)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      MyWheel.TrailContactProperties contactProperties = this.m_contactPointTrail.Value;
      if (contactProperties.ContactEntityId == -1L)
        return;
      Vector3D contactNormal = (Vector3D) contactProperties.ContactNormal;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D down1 = worldMatrix.Down;
      if (Math.Abs(Vector3D.Dot(contactNormal, down1)) > 0.3)
        return;
      Vector3D vector2_1 = -Vector3D.Normalize((Vector3D) contactProperties.ContactNormal);
      worldMatrix = this.WorldMatrix;
      double num1 = Vector3D.Dot(worldMatrix.Down, vector2_1);
      worldMatrix = this.WorldMatrix;
      Vector3D down2 = worldMatrix.Down;
      Vector3D vector3D1 = num1 * down2;
      Vector3D vector2_2 = Vector3D.Normalize(vector2_1 - vector3D1);
      Vector3D vector1 = vector2_2;
      worldMatrix = this.WorldMatrix;
      Vector3D down3 = worldMatrix.Down;
      Vector3D vector3D2 = Vector3D.Cross(vector1, down3);
      worldMatrix = this.WorldMatrix;
      Vector3D translation1 = worldMatrix.Translation;
      worldMatrix = this.WorldMatrix;
      Vector3D vector3D3 = Vector3D.Dot(worldMatrix.Translation - contactProperties.ContactPosition, vector2_2) * vector2_2;
      Vector3D vector3D4 = translation1 - vector3D3;
      IReadOnlyList<MyDecalMaterial> decalMaterials = (IReadOnlyList<MyDecalMaterial>) null;
      MyStringHash subtypeId = this.BlockDefinition.Id.SubtypeId;
      bool decalMaterial = MyDecalMaterials.TryGetDecalMaterial(subtypeId.String, contactProperties.PhysicalMaterial.String, out decalMaterials, contactProperties.VoxelMaterial);
      if (!decalMaterial)
      {
        subtypeId = this.BlockDefinition.Id.SubtypeId;
        decalMaterial = MyDecalMaterials.TryGetDecalMaterial(subtypeId.String, "GenericMaterial", out decalMaterials, contactProperties.VoxelMaterial);
      }
      if (decalMaterial && decalMaterials != null && (decalMaterials.Count > 0 && decalMaterials[0] != null))
      {
        this.m_terrainMaterialDistance = (double) decalMaterials[0].Spacing <= 0.0 ? decalMaterials[0].MinSize * 1.95f : decalMaterials[0].MinSize * decalMaterials[0].Spacing;
        this.m_xDecalOffset = decalMaterials[0].XOffset;
        this.m_yDecalOffset = decalMaterials[0].YOffset;
        Vector3D vector3D5 = vector3D4;
        worldMatrix = this.WorldMatrix;
        Vector3D translation2 = worldMatrix.Translation;
        this.m_wheelCenterToTrail = vector3D5 - translation2;
        if (this.LastTrail == null || contactProperties.VoxelMaterial != this.LastTrail.VoxelMaterial)
          this.LastTrail = new MyTrailProperties()
          {
            EntityId = contactProperties.ContactEntityId,
            PhysicalMaterial = decalMaterials[0].Target,
            VoxelMaterial = contactProperties.VoxelMaterial,
            Position = vector3D4,
            Normal = vector2_2,
            ForwardDirection = vector3D2
          };
        Vector3D vec = vector3D4 - this.LastTrail.Position;
        vec = Vector3D.ProjectOnPlane(ref vec, ref this.LastTrail.ForwardDirection);
        double num2 = vec.LengthSquared();
        if (num2 > (double) this.m_terrainMaterialDistance && num2 < (double) this.m_terrainMaterialDistance * 4.0)
          vec = Vector3D.Normalize(vec) * Math.Sqrt((double) this.m_terrainMaterialDistance);
        this.LastTrail.Position += vec;
      }
      else
      {
        if (this.LastTrail == null)
          return;
        this.LastTrail.PhysicalMaterial = contactProperties.PhysicalMaterial;
        this.LastTrail.VoxelMaterial = contactProperties.VoxelMaterial;
      }
    }

    private void CheckTrail()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS || this.LastTrail == null)
        return;
      Vector3D displacement1 = this.WorldMatrix.Translation + this.m_wheelCenterToTrail - this.LastTrail.Position;
      double num1 = displacement1.LengthSquared();
      if (num1 <= (double) this.m_terrainMaterialDistance)
        return;
      double num2 = Math.Floor(Math.Sqrt(num1 / (double) this.m_terrainMaterialDistance));
      if (num2 > 1.0 && num2 < 16.0)
      {
        Vector3D displacement2 = Vector3D.Normalize(displacement1) * (double) this.m_terrainMaterialDistance;
        for (int index = 0; (double) index < num2; ++index)
          this.AddTrails(displacement2);
      }
      else
        this.AddTrails(displacement1);
    }

    public void AddTrails(Vector3D displacement)
    {
      if (displacement.LengthSquared() < (double) this.m_terrainMaterialDistance)
        return;
      Vector3D vector1 = Vector3D.Normalize(displacement);
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D down = worldMatrix.Down;
      Vector3D vector3D = Vector3D.Normalize(Vector3D.Cross(vector1, down));
      worldMatrix = this.WorldMatrix;
      Vector3D forwardDirection = (Vector3D) Vector3.Normalize(Vector3D.Cross(worldMatrix.Down, this.m_wheelCenterToTrail));
      float num = (float) Math.Sign(Vector3.Dot((Vector3) forwardDirection, (Vector3) displacement));
      this.AddTrails(this.LastTrail.Position + displacement, (Vector3D) Vector3.Normalize(this.LastTrail.Normal * 0.200000002980232 + 0.800000011920929 * vector3D * (double) num), forwardDirection, this.LastTrail.EntityId, this.LastTrail.PhysicalMaterial, this.LastTrail.VoxelMaterial);
    }

    public void AddTrails(MyTrailProperties properties) => this.AddTrails(properties.Position, properties.Normal, properties.ForwardDirection, properties.EntityId, properties.PhysicalMaterial, properties.VoxelMaterial);

    public void AddTrails(
      Vector3D position,
      Vector3D normal,
      Vector3D forwardDirection,
      long entityId,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial)
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS || this.LastTrail == null || (this.LastTrail.Position - position).LengthSquared() < (double) this.m_terrainMaterialDistance)
        return;
      Vector3D vector3D1 = position;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D vector3D2 = worldMatrix.Up * (double) this.m_xDecalOffset;
      Vector3D vector3D3 = vector3D1 - vector3D2;
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(vector3D3 + normal, vector3D3 - normal, 28);
      if (!nullable.HasValue)
        return;
      Vector3D position1 = nullable.Value.Position;
      worldMatrix = this.WorldMatrix;
      Vector3D vector2 = Vector3D.Cross(worldMatrix.Down, (Vector3D) nullable.Value.HkHitInfo.Normal);
      forwardDirection = vector2;
      normal -= Vector3D.Dot(normal, vector2);
      normal += nullable.Value.HkHitInfo.Normal * 1.5f;
      normal = Vector3D.Normalize(normal);
      this.LastTrail.Position = position;
      this.LastTrail.Normal = normal;
      this.LastTrail.ForwardDirection = forwardDirection;
      this.LastTrail.EntityId = entityId;
      this.LastTrail.PhysicalMaterial = physicalMaterial;
      this.LastTrail.VoxelMaterial = voxelMaterial;
      MyHitInfo hitInfo = new MyHitInfo()
      {
        Position = position1,
        Normal = (Vector3) normal
      };
      MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entityId);
      if (entityById == null)
        return;
      MyDecals.HandleAddDecal((VRage.ModAPI.IMyEntity) entityById, hitInfo, (Vector3) forwardDirection, physicalMaterial, this.BlockDefinition.Id.SubtypeId, damage: 30f, voxelMaterial: voxelMaterial, isTrail: true);
    }

    private void UpdateEffect(string effectName, Vector3D contactPosition, Vector3 contactNormal)
    {
      if (this.Render != null)
      {
        if (effectName != null)
          this.Render.TrySpawnParticle(effectName, ref contactPosition, ref contactNormal);
        this.Render.UpdateParticle(ref contactPosition, ref contactNormal);
      }
      if (effectName == null || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_particleData.Value = new MyWheel.ParticleData()
      {
        EffectName = effectName,
        PositionRelative = (Vector3) (contactPosition - this.PositionComp.WorldMatrixRef.Translation),
        Normal = contactNormal
      };
    }

    private bool SteeringLogic()
    {
      if (!this.IsFunctional)
        return false;
      MyGridPhysics physics = this.CubeGrid.Physics;
      if (physics == null || this.Stator != null && MyFixedGrids.IsRooted(this.Stator.CubeGrid))
        return false;
      if (this.m_slipCountdown > 0)
        --this.m_slipCountdown;
      if (this.m_staticHitCount == 0)
      {
        if (this.m_contactCountdown <= 0)
          return false;
        --this.m_contactCountdown;
        if (this.m_contactCountdown == 0)
        {
          this.m_frictionCollector = 0.0f;
          this.m_contactNormals.Clear();
          return false;
        }
      }
      Vector3 linearVelocity = physics.LinearVelocity;
      if (MyUtils.IsZero(ref linearVelocity) || !physics.IsActive)
        return false;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D centerOfMassWorld = physics.CenterOfMassWorld;
      Vector3 normal;
      if (!this.m_contactNormals.GetAvgNormal(out normal))
        return false;
      this.LastUsedGroundNormal = normal;
      Vector3 up = (Vector3) worldMatrix.Up;
      Vector3 guideVector = Vector3.Cross(normal, up);
      Vector3 vec1 = Vector3.ProjectOnPlane(ref linearVelocity, ref normal);
      Vector3 vector3_1 = Vector3.ProjectOnVector(ref vec1, ref guideVector);
      Vector3 vec2 = vector3_1 - vec1;
      if (MyUtils.IsZero(ref vec2))
        return false;
      bool flag1 = false;
      bool flag2 = false;
      float num1 = 6f * this.m_frictionCollector;
      Vector3 vec3 = Vector3.ProjectOnVector(ref vec2, ref up);
      float num2 = vec3.Length();
      bool flag3 = (double) num2 > (double) num1;
      if (flag3 || this.m_slipCountdown != 0)
      {
        float num3 = 1f / num2 * num1;
        Vector3 vector3_2 = vec3 * num3;
        flag1 = true;
        vec3 = vector3_2 * (1f - MyPhysicsConfig.WheelSlipCutAwayRatio);
        if (flag3)
          this.m_slipCountdown = MyPhysicsConfig.WheelSlipCountdown;
      }
      else if ((double) num2 < 0.1)
        flag2 = true;
      if (!flag2)
      {
        if ((double) vec3.LengthSquared() < 1.0 / 1000.0)
          return !flag2;
        vec3 *= (float) (1.0 - (1.0 - (double) this.m_frictionCollector) * (double) MyPhysicsConfig.WheelSurfaceMaterialSteerRatio);
        Vector3 vec4 = vec3;
        Vector3 vector3_2 = Vector3.ProjectOnPlane(ref vec4, ref normal);
        MyMechanicalConnectionBlockBase stator = this.Stator;
        MyPhysicsBody myPhysicsBody = (MyPhysicsBody) null;
        if (stator != null)
          myPhysicsBody = (MyPhysicsBody) this.Stator.CubeGrid.Physics;
        Vector3 vector3_3 = vector3_2 * 0.1f;
        if (myPhysicsBody == null)
        {
          Vector3 dir = vector3_3 * physics.Mass;
          physics.ApplyImpulse(dir, centerOfMassWorld);
        }
        else
        {
          Vector3D vector3D1 = Vector3D.Zero;
          if (stator is MyMotorSuspension myMotorSuspension)
          {
            vector3_3 *= MyMath.Clamp(myMotorSuspension.Friction * 2f, 0.0f, 1f);
            Vector3 adjustmentVector;
            myMotorSuspension.GetCoMVectors(out adjustmentVector);
            vector3D1 = Vector3D.TransformNormal(-adjustmentVector, stator.CubeGrid.WorldMatrix);
          }
          Vector3D vector3D2 = centerOfMassWorld + vector3D1;
          float wheelImpulseBlending = MyPhysicsConfig.WheelImpulseBlending;
          Vector3 vector3_4 = this.m_lastFrameImpuse * wheelImpulseBlending + vector3_3 * (1f - wheelImpulseBlending);
          this.m_lastFrameImpuse = vector3_4;
          Vector3 dir = vector3_4 * myPhysicsBody.Mass;
          myPhysicsBody.ApplyImpulse(dir, vector3D2);
          if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
          {
            MyRenderProxy.DebugDrawArrow3DDir(vector3D2, -vector3D1, Color.Red);
            MyRenderProxy.DebugDrawSphere(vector3D2, 0.1f, Color.Yellow, depthRead: false);
          }
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
      {
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, (Vector3D) vec1, Color.Yellow);
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, (Vector3D) vector3_1, Color.Blue);
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, (Vector3D) vec3, Color.MediumPurple);
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + vec1, (Vector3D) vec2, Color.Red);
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + up, (Vector3D) normal, Color.AliceBlue);
        MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, (Vector3D) Vector3.ProjectOnPlane(ref vec3, ref normal), flag1 ? Color.DarkRed : Color.IndianRed);
        if (this.m_slipCountdown > 0)
          MyRenderProxy.DebugDrawText3D(centerOfMassWorld + up * 2f, "Drift", Color.Red, 1f, false);
        MyRenderProxy.DebugDrawText3D(centerOfMassWorld + up * 1.2f, this.m_staticHitCount.ToString(), Color.Red, 1f, false);
      }
      return !flag2;
    }

    public override void UpdateBeforeSimulation()
    {
      this.CheckTrail();
      base.UpdateBeforeSimulation();
      this.SwapModelLogic();
      bool flag = this.SteeringLogic();
      if (!flag && this.m_contactCountdown == 0)
      {
        this.m_lastFrameImpuse = Vector3.Zero;
        if ((this.Render == null || this.Render != null && !this.Render.UpdateNeeded) && !this.HasDamageEffect)
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
        return;
      MatrixD worldMatrix = this.WorldMatrix;
      MyRenderProxy.DebugDrawCross(worldMatrix.Translation, worldMatrix.Up, worldMatrix.Forward, flag ? Color.Green : Color.Red);
    }

    public override string CalculateCurrentModel(out Matrix orientation)
    {
      string currentModel = base.CalculateCurrentModel(out orientation);
      return this.CubeGrid.Physics == null || this.Stator == null || (!this.IsFunctional || !this.m_usesAlternativeModel) ? currentModel : this.WheelModelsDefinition.AlternativeModel;
    }

    private void SwapModelLogic()
    {
      if (!MyFakes.WHEEL_ALTERNATIVE_MODELS_ENABLED || this.Stator == null || !this.IsFunctional)
      {
        if (!this.m_usesAlternativeModel)
          return;
        this.m_usesAlternativeModel = false;
        this.UpdateVisual();
      }
      else
      {
        float velocityThreshold = this.WheelModelsDefinition.AngularVelocityThreshold;
        float angularVelocityDiff = this.GetObserverAngularVelocityDiff();
        if (((!this.m_usesAlternativeModel ? 0 : ((double) angularVelocityDiff + 5.0 < (double) velocityThreshold ? 1 : 0)) | (this.m_usesAlternativeModel ? (false ? 1 : 0) : ((double) angularVelocityDiff - 5.0 > (double) velocityThreshold ? 1 : 0))) != 0)
        {
          ++this.m_modelSwapCountUp;
          if (this.m_modelSwapCountUp < 5)
            return;
          this.m_usesAlternativeModel = !this.m_usesAlternativeModel;
          this.UpdateVisual();
        }
        else
          this.m_modelSwapCountUp = 0;
      }
    }

    private float GetObserverAngularVelocityDiff()
    {
      MyGridPhysics physics1 = this.CubeGrid.Physics;
      if (physics1 != null)
      {
        Vector3 vector3 = physics1.LinearVelocity;
        if ((double) vector3.LengthSquared() > 16.0)
        {
          IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
          if (controlledEntity != null)
          {
            MyEntity entity = controlledEntity.Entity;
            if (entity != null)
            {
              MyPhysicsComponentBase physics2 = entity.GetTopMostParent((System.Type) null).Physics;
              if (physics2 != null)
              {
                vector3 = physics1.AngularVelocity - physics2.AngularVelocity;
                return vector3.Length();
              }
            }
          }
        }
      }
      return 0.0f;
    }

    public static void WheelExplosionLog(MyCubeGrid grid, MyTerminalBlock block, string message)
    {
    }

    public static void DumpActivityLog()
    {
      lock (MyWheel.activityLog)
      {
        foreach (KeyValuePair<MyCubeGrid, Queue<MyTuple<DateTime, string>>> keyValuePair in MyWheel.activityLog)
        {
          MyLog.Default.WriteLine("GRID: " + keyValuePair.Key.DisplayName);
          foreach (MyTuple<DateTime, string> myTuple in keyValuePair.Value)
            MyLog.Default.WriteLine("[" + myTuple.Item1.ToString("dd/MM hh:mm:ss:FFF") + "] " + myTuple.Item2);
          MyLog.Default.Flush();
        }
        MyWheel.activityLog.Clear();
      }
    }

    public override void Attach(MyMechanicalConnectionBlockBase parent)
    {
      base.Attach(parent);
      this.m_isSuspensionMounted = this.Stator is MyMotorSuspension;
    }

    public override void Detach(bool isWelding)
    {
      this.m_isSuspensionMounted = false;
      base.Detach(isWelding);
    }

    private void RegisterPerFrameUpdate()
    {
      if ((this.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE || this.m_eachUpdateCallbackRegistered)
        return;
      this.m_eachUpdateCallbackRegistered = true;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        this.m_eachUpdateCallbackRegistered = false;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }), "WheelEachUpdate");
    }

    [Serializable]
    protected struct TrailContactProperties
    {
      public long ContactEntityId;
      public Vector3 ContactPosition;
      public Vector3 ContactNormal;
      public MyStringHash PhysicalMaterial;
      public MyStringHash VoxelMaterial;

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003ETrailContactProperties\u003C\u003EContactEntityId\u003C\u003EAccessor : IMemberAccessor<MyWheel.TrailContactProperties, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.TrailContactProperties owner, in long value) => owner.ContactEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.TrailContactProperties owner, out long value) => value = owner.ContactEntityId;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003ETrailContactProperties\u003C\u003EContactPosition\u003C\u003EAccessor : IMemberAccessor<MyWheel.TrailContactProperties, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.TrailContactProperties owner, in Vector3 value) => owner.ContactPosition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.TrailContactProperties owner, out Vector3 value) => value = owner.ContactPosition;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003ETrailContactProperties\u003C\u003EContactNormal\u003C\u003EAccessor : IMemberAccessor<MyWheel.TrailContactProperties, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.TrailContactProperties owner, in Vector3 value) => owner.ContactNormal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.TrailContactProperties owner, out Vector3 value) => value = owner.ContactNormal;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003ETrailContactProperties\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : IMemberAccessor<MyWheel.TrailContactProperties, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.TrailContactProperties owner, in MyStringHash value) => owner.PhysicalMaterial = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.TrailContactProperties owner, out MyStringHash value) => value = owner.PhysicalMaterial;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003ETrailContactProperties\u003C\u003EVoxelMaterial\u003C\u003EAccessor : IMemberAccessor<MyWheel.TrailContactProperties, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.TrailContactProperties owner, in MyStringHash value) => owner.VoxelMaterial = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.TrailContactProperties owner, out MyStringHash value) => value = owner.VoxelMaterial;
      }
    }

    [Serializable]
    private struct ParticleData
    {
      public string EffectName;
      public Vector3 PositionRelative;
      public Vector3 Normal;

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003EParticleData\u003C\u003EEffectName\u003C\u003EAccessor : IMemberAccessor<MyWheel.ParticleData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.ParticleData owner, in string value) => owner.EffectName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.ParticleData owner, out string value) => value = owner.EffectName;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003EParticleData\u003C\u003EPositionRelative\u003C\u003EAccessor : IMemberAccessor<MyWheel.ParticleData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.ParticleData owner, in Vector3 value) => owner.PositionRelative = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.ParticleData owner, out Vector3 value) => value = owner.PositionRelative;
      }

      protected class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003EParticleData\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<MyWheel.ParticleData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyWheel.ParticleData owner, in Vector3 value) => owner.Normal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyWheel.ParticleData owner, out Vector3 value) => value = owner.Normal;
      }
    }

    protected class m_particleData\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyWheel.ParticleData, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyWheel.ParticleData, SyncDirection.FromServer>(obj1, obj2));
        ((MyWheel) obj0).m_particleData = (VRage.Sync.Sync<MyWheel.ParticleData, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_contactPointTrail\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyWheel.TrailContactProperties, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyWheel.TrailContactProperties, SyncDirection.FromServer>(obj1, obj2));
        ((MyWheel) obj0).m_contactPointTrail = (VRage.Sync.Sync<MyWheel.TrailContactProperties, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyWheel\u003C\u003EActor : IActivator, IActivator<MyWheel>
    {
      object IActivator.CreateInstance() => (object) new MyWheel();

      MyWheel IActivator<MyWheel>.CreateInstance() => new MyWheel();
    }
  }
}
