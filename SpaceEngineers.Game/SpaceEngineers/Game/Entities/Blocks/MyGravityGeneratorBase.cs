// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyGravityGeneratorBase
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase)})]
  public abstract class MyGravityGeneratorBase : MyFunctionalBlock, IMyGizmoDrawableObject, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider
  {
    protected Color m_gizmoColor = (Color) new Vector4(0.0f, 1f, 0.0f, 0.196f);
    protected const float m_maxGizmoDrawDistance = 1000f;
    protected bool m_oldEmissiveState;
    protected readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_gravityAcceleration;
    protected MyConcurrentHashSet<VRage.ModAPI.IMyEntity> m_containedEntities = new MyConcurrentHashSet<VRage.ModAPI.IMyEntity>();

    private MyGravityGeneratorBaseDefinition BlockDefinition => (MyGravityGeneratorBaseDefinition) base.BlockDefinition;

    public float GravityAcceleration
    {
      get => (float) this.m_gravityAcceleration;
      set
      {
        if ((double) (float) this.m_gravityAcceleration == (double) value)
          return;
        this.m_gravityAcceleration.Value = value;
      }
    }

    public abstract bool IsPositionInRange(Vector3D worldPoint);

    public abstract Vector3 GetWorldGravity(Vector3D worldPoint);

    protected abstract float CalculateRequiredPowerInput();

    protected abstract HkShape GetHkShape();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.InitializeSinkComponent();
      base.Init(objectBuilder, cubeGrid);
      if (this.CubeGrid.CreatePhysics)
      {
        if (MyFakes.ENABLE_GRAVITY_PHANTOM)
        {
          HkBvShape fieldShape = this.CreateFieldShape();
          this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_KINEMATIC);
          this.Physics.IsPhantom = true;
          this.Physics.CreateFromCollisionObject((HkShape) fieldShape, this.PositionComp.LocalVolume.Center, this.WorldMatrix, collisionFilter: 21);
          fieldShape.Base.RemoveReference();
          this.Physics.Enabled = this.IsWorking;
        }
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
        this.ResourceSink.Update();
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_baseIdleSound.Init("BlockGravityGen");
    }

    protected abstract void InitializeSinkComponent();

    protected void UpdateFieldShape()
    {
      if (MyFakes.ENABLE_GRAVITY_PHANTOM && this.Physics != null)
      {
        HkBvShape fieldShape = this.CreateFieldShape();
        int num = (int) this.Physics.RigidBody.SetShape((HkShape) fieldShape);
        fieldShape.Base.RemoveReference();
        this.UpdateGeneratorProxy();
      }
      this.ResourceSink.Update();
    }

    private HkBvShape CreateFieldShape()
    {
      HkPhantomCallbackShape phantomCallbackShape = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_Enter), new HkPhantomHandler(this.phantom_Leave));
      return new HkBvShape(this.GetHkShape(), (HkShape) phantomCallbackShape, HkReferencePolicy.TakeOwnership);
    }

    protected override bool CheckIsWorking() => (this.ResourceSink != null ? (this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? 1 : 0) : 1) != 0 && base.CheckIsWorking();

    protected MyGravityGeneratorBase()
    {
      this.m_gravityAcceleration.ValueChanged += (Action<SyncBase>) (x => this.AccelerationChanged());
      this.NeedsWorldMatrix = true;
    }

    private void AccelerationChanged() => this.ResourceSink.Update();

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      MyGravityProviderSystem.AddGravityGenerator((IMyGravityProvider) this);
      if (this.ResourceSink == null)
        return;
      this.ResourceSink.Update();
    }

    public override void OnRemovedFromScene(object source)
    {
      MyGravityProviderSystem.RemoveGravityGenerator((IMyGravityProvider) this);
      base.OnRemovedFromScene(source);
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      this.UpdateGeneratorProxy();
    }

    private void UpdateGeneratorProxy()
    {
      MyGridPhysics physics = this.CubeGrid.Physics;
      if (physics == null || !this.InScene)
        return;
      Vector3 linearVelocity = physics.LinearVelocity;
      MyGravityProviderSystem.OnGravityGeneratorMoved((IMyGravityProvider) this, ref linearVelocity);
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.IsWorking)
      {
        foreach (VRage.ModAPI.IMyEntity containedEntity in this.m_containedEntities)
        {
          MyEntity myEntity = containedEntity as MyEntity;
          MyCharacter myCharacter = myEntity as MyCharacter;
          SpaceEngineers.Game.ModAPI.IMyVirtualMass myVirtualMass = myEntity as SpaceEngineers.Game.ModAPI.IMyVirtualMass;
          MatrixD worldMatrix = myEntity.WorldMatrix;
          float strengthMultiplier = MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(worldMatrix.Translation));
          if ((double) strengthMultiplier > 0.0)
          {
            worldMatrix = myEntity.WorldMatrix;
            Vector3 vector3_1 = this.GetWorldGravity(worldMatrix.Translation) * strengthMultiplier;
            if (myVirtualMass != null && myEntity.Physics.RigidBody.IsActive)
            {
              if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)
              {
                worldMatrix = myEntity.WorldMatrix;
                MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, 0.2f, myVirtualMass.IsWorking ? Color.Blue : Color.Red, depthRead: false);
              }
              if (myVirtualMass.IsWorking && !myVirtualMass.CubeGrid.IsStatic && !myVirtualMass.CubeGrid.Physics.IsStatic)
              {
                MyPhysicsComponentBase physics = myVirtualMass.CubeGrid.Physics;
                Vector3? force = new Vector3?(vector3_1 * myVirtualMass.VirtualMass);
                worldMatrix = myEntity.WorldMatrix;
                Vector3D? position = new Vector3D?(worldMatrix.Translation);
                Vector3? torque = new Vector3?();
                float? maxSpeed = new float?();
                physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, force, position, torque, maxSpeed, false);
              }
            }
            else if (!myEntity.Physics.IsKinematic && !myEntity.Physics.IsStatic && ((HkReferenceObject) myEntity.Physics.RigidBody2 == (HkReferenceObject) null && myCharacter == null) && (HkReferenceObject) myEntity.Physics.RigidBody != (HkReferenceObject) null)
            {
              switch (myEntity)
              {
                case MyFloatingObject myFloatingObject:
                  Vector3 vector3_2 = (myFloatingObject.HasConstraints() ? 2f : 1f) * vector3_1;
                  myFloatingObject.GeneratedGravity += vector3_2;
                  continue;
                case MyInventoryBagEntity inventoryBagEntity:
                  inventoryBagEntity.GeneratedGravity += vector3_1;
                  continue;
                default:
                  myEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, new Vector3?(vector3_1 * myEntity.Physics.RigidBody.Mass), new Vector3D?(), new Vector3?());
                  continue;
              }
            }
          }
        }
      }
      if (this.m_containedEntities.Count != 0)
        return;
      this.NeedsUpdate = this.HasDamageEffect ? MyEntityUpdateEnum.EACH_FRAME : MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    protected void OnIsWorkingChanged(MyCubeBlock obj) => this.UpdateGenerator();

    protected void Receiver_IsPoweredChanged() => this.UpdateGenerator();

    private void UpdateGenerator()
    {
      this.UpdateIsWorking();
      if (this.Physics != null)
        this.Physics.Enabled = this.IsWorking;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected void Receiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
    {
      VRage.ModAPI.IMyEntity entity = body.GetEntity(0U);
      if (entity == null || entity is MyCubeGrid || !this.m_containedEntities.Add(entity))
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        MyPhysicsComponentBase physics = entity.Physics;
        if (physics == null || !physics.HasRigidBody)
          return;
        physics.RigidBody.Activate();
      }), "MyGravityGeneratorBase/Activate physics");
    }

    private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
    {
      VRage.ModAPI.IMyEntity entity = body.GetEntity(0U);
      if (entity == null)
        return;
      this.m_containedEntities.Remove(entity);
    }

    public Color GetGizmoColor() => this.m_gizmoColor;

    public bool CanBeDrawn() => MyCubeGrid.ShowGravityGizmos && this.ShowOnHUD && (this.IsWorking && this.HasLocalPlayerAccess()) && this.GetDistanceBetweenCameraAndBoundingSphere() <= 1000.0;

    public MatrixD GetWorldMatrix() => this.WorldMatrix;

    public virtual BoundingBox? GetBoundingBox() => new BoundingBox?();

    public virtual float GetRadius() => -1f;

    public Vector3 GetPositionInGrid() => (Vector3) this.Position;

    protected override void Closing()
    {
      base.Closing();
      if (!this.CubeGrid.CreatePhysics || this.ResourceSink == null)
        return;
      this.ResourceSink.IsPoweredChanged -= new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.RequiredInputChanged -= new MyRequiredResourceChangeDelegate(this.Receiver_RequiredInputChanged);
    }

    public bool EnableLongDrawDistance() => false;

    public float GetGravityMultiplier(Vector3D worldPoint) => !this.IsPositionInRange(worldPoint) ? 0.0f : 1f;

    public abstract void GetProxyAABB(out BoundingBoxD aabb);

    float SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase.GravityAcceleration
    {
      get => this.GravityAcceleration;
      set => this.GravityAcceleration = MathHelper.Clamp(value, this.BlockDefinition.MinGravityAcceleration, this.BlockDefinition.MaxGravityAcceleration);
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.Gravity => this.GravityAcceleration / 9.81f;

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.GravityAcceleration
    {
      get => this.GravityAcceleration;
      set => this.GravityAcceleration = MathHelper.Clamp(value, this.BlockDefinition.MinGravityAcceleration, this.BlockDefinition.MaxGravityAcceleration);
    }

    protected class m_gravityAcceleration\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyGravityGeneratorBase) obj0).m_gravityAcceleration = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
