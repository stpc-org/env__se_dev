// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyPhysicsBody
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace Sandbox.Engine.Physics
{
  [MyComponentBuilder(typeof (MyObjectBuilder_PhysicsBodyComponent), true)]
  public class MyPhysicsBody : MyPhysicsComponentBase, MyClusterTree.IMyActivationHandler
  {
    private MyPhysicsBody.PhysicsContactHandler m_contactPointCallbackHandler;
    private Action<MyPhysicsComponentBase, bool> m_onBodyActiveStateChangedHandler;
    private bool m_activationCallbackRegistered;
    private bool m_contactPointCallbackRegistered;
    private static MyStringHash m_character = MyStringHash.GetOrCompute("Character");
    private int m_motionCounter;
    protected float m_angularDamping;
    protected float m_linearDamping;
    private ulong m_clusterObjectID = ulong.MaxValue;
    private Vector3D m_offset = Vector3D.Zero;
    protected Matrix m_bodyMatrix;
    protected HkWorld m_world;
    private HkWorld m_lastWorld;
    private HkRigidBody m_rigidBody;
    private HkRigidBody m_rigidBody2;
    private float m_animatedClientMass;
    private readonly HashSet<HkConstraint> m_constraints = new HashSet<HkConstraint>();
    private readonly List<HkConstraint> m_constraintsAddBatch = new List<HkConstraint>();
    private readonly List<HkConstraint> m_constraintsRemoveBatch = new List<HkConstraint>();
    public HkSolverDeactivation InitialSolverDeactivation = HkSolverDeactivation.Low;
    private bool m_isInWorld;
    private bool m_shapeChangeInProgress;
    private HashSet<IMyEntity> m_batchedChildren = new HashSet<IMyEntity>();
    private List<MyPhysicsBody> m_batchedBodies = new List<MyPhysicsBody>();
    private Vector3D? m_lastComPosition;
    private Vector3 m_lastComLocal;
    private bool m_isStaticForCluster;
    private bool m_batchRequest;
    private static List<HkConstraint> m_notifyConstraints = new List<HkConstraint>();
    private HkdBreakableBody m_breakableBody;
    private List<HkdBreakableBodyInfo> m_tmpLst = new List<HkdBreakableBodyInfo>();
    protected HkRagdoll m_ragdoll;
    private bool m_ragdollDeadMode;
    private readonly MyPhysicsBody.MyWeldInfo m_weldInfo = new MyPhysicsBody.MyWeldInfo();
    private List<HkShape> m_tmpShapeList = new List<HkShape>();

    private bool NeedsActivationCallback => (HkReferenceObject) this.m_rigidBody2 != (HkReferenceObject) null || this.m_onBodyActiveStateChangedHandler != null;

    private bool NeedsContactPointCallback => this.m_contactPointCallbackHandler != null;

    public override event Action<MyPhysicsComponentBase, bool> OnBodyActiveStateChanged
    {
      add
      {
        int num = this.IsInWorld ? 1 : 0;
        this.m_onBodyActiveStateChangedHandler += value;
        this.RegisterActivationCallbacksIfNeeded();
      }
      remove
      {
        int num = this.IsInWorld ? 1 : 0;
        this.m_onBodyActiveStateChangedHandler -= value;
        if (this.NeedsActivationCallback)
          return;
        this.UnregisterActivationCallbacks();
      }
    }

    public event MyPhysicsBody.PhysicsContactHandler ContactPointCallback
    {
      add
      {
        int num = this.IsInWorld ? 1 : 0;
        this.m_contactPointCallbackHandler += value;
        this.RegisterContactPointCallbackIfNeeded();
      }
      remove
      {
        int num = this.IsInWorld ? 1 : 0;
        this.m_contactPointCallbackHandler -= value;
        if (this.NeedsContactPointCallback)
          return;
        this.UnregisterContactPointCallback();
      }
    }

    private void OnContactPointCallback(ref HkContactPointEvent e)
    {
      if (this.m_contactPointCallbackHandler == null)
        return;
      MyPhysics.MyContactPointEvent e1 = new MyPhysics.MyContactPointEvent()
      {
        ContactPointEvent = e,
        Position = e.ContactPoint.Position + this.Offset
      };
      this.m_contactPointCallbackHandler(ref e1);
    }

    private void OnDynamicRigidBodyActivated(HkEntity entity)
    {
      this.SynchronizeKeyframedRigidBody();
      this.InvokeOnBodyActiveStateChanged(true);
    }

    private void OnDynamicRigidBodyDeactivated(HkEntity entity)
    {
      this.SynchronizeKeyframedRigidBody();
      this.InvokeOnBodyActiveStateChanged(false);
    }

    protected void InvokeOnBodyActiveStateChanged(bool active) => this.m_onBodyActiveStateChangedHandler.InvokeIfNotNull<MyPhysicsComponentBase, bool>((MyPhysicsComponentBase) this, active);

    private void RegisterActivationCallbacksIfNeeded()
    {
      if (this.m_activationCallbackRegistered || !this.NeedsActivationCallback || (HkReferenceObject) this.m_rigidBody == (HkReferenceObject) null)
        return;
      this.m_activationCallbackRegistered = true;
      this.m_rigidBody.Activated += new HkEntityHandler(this.OnDynamicRigidBodyActivated);
      this.m_rigidBody.Deactivated += new HkEntityHandler(this.OnDynamicRigidBodyDeactivated);
    }

    private void RegisterContactPointCallbackIfNeeded()
    {
      if (this.m_contactPointCallbackRegistered || !this.NeedsContactPointCallback || (HkReferenceObject) this.m_rigidBody == (HkReferenceObject) null)
        return;
      this.m_contactPointCallbackRegistered = true;
      this.m_rigidBody.ContactPointCallback += new HkContactPointEventHandler(this.OnContactPointCallback);
    }

    private void UnregisterActivationCallbacks()
    {
      if (!this.m_activationCallbackRegistered)
        return;
      this.m_activationCallbackRegistered = false;
      if ((HkReferenceObject) this.m_rigidBody == (HkReferenceObject) null)
        return;
      this.m_rigidBody.Activated -= new HkEntityHandler(this.OnDynamicRigidBodyActivated);
      this.m_rigidBody.Deactivated -= new HkEntityHandler(this.OnDynamicRigidBodyDeactivated);
    }

    private void UnregisterContactPointCallback()
    {
      if (!this.m_contactPointCallbackRegistered)
        return;
      this.m_contactPointCallbackRegistered = false;
      if ((HkReferenceObject) this.m_rigidBody == (HkReferenceObject) null)
        return;
      this.m_rigidBody.ContactPointCallback -= new HkContactPointEventHandler(this.OnContactPointCallback);
    }

    protected ulong ClusterObjectID
    {
      get => this.m_clusterObjectID;
      set
      {
        this.m_clusterObjectID = value;
        this.Offset = value == ulong.MaxValue ? Vector3D.Zero : MyPhysics.GetObjectOffset(value);
        foreach (MyPhysicsBody child in this.WeldInfo.Children)
          child.Offset = this.Offset;
      }
    }

    protected Vector3D Offset
    {
      get
      {
        IMyEntity topMostParent = this.Entity.GetTopMostParent();
        return topMostParent != this.Entity && topMostParent.Physics != null ? ((MyPhysicsBody) topMostParent.Physics).Offset : this.m_offset;
      }
      set => this.m_offset = value;
    }

    public MyPhysicsBodyComponentDefinition Definition { get; private set; }

    public HkWorld HavokWorld
    {
      get
      {
        if (this.IsWelded)
          return this.WeldInfo.Parent.m_world;
        IMyEntity topMostParent = this.Entity.GetTopMostParent();
        return topMostParent != this.Entity && topMostParent.Physics != null ? ((MyPhysicsBody) topMostParent.Physics).HavokWorld : this.m_world;
      }
    }

    public virtual int HavokCollisionSystemID
    {
      get => !((HkReferenceObject) this.RigidBody != (HkReferenceObject) null) ? 0 : HkGroupFilter.GetSystemGroupFromFilterInfo(this.RigidBody.GetCollisionFilterInfo());
      protected set
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(this.RigidBody.Layer, value, 1, 1));
        if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
          return;
        this.RigidBody2.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(this.RigidBody2.Layer, value, 1, 1));
      }
    }

    public override HkRigidBody RigidBody
    {
      get => this.WeldInfo.Parent == null ? this.m_rigidBody : this.WeldInfo.Parent.RigidBody;
      protected set
      {
        if (!((HkReferenceObject) this.m_rigidBody != (HkReferenceObject) value))
          return;
        if ((HkReferenceObject) this.m_rigidBody != (HkReferenceObject) null && !this.m_rigidBody.IsDisposed)
        {
          this.m_rigidBody.ContactSoundCallback -= new HkContactPointEventHandler(this.OnContactSoundCallback);
          this.UnregisterContactPointCallback();
          this.UnregisterActivationCallbacks();
        }
        this.m_rigidBody = value;
        this.m_activationCallbackRegistered = false;
        this.m_contactPointCallbackRegistered = false;
        if (!((HkReferenceObject) this.m_rigidBody != (HkReferenceObject) null))
          return;
        this.RegisterActivationCallbacksIfNeeded();
        this.RegisterContactPointCallbackIfNeeded();
        this.m_rigidBody.ContactSoundCallback += new HkContactPointEventHandler(this.OnContactSoundCallback);
      }
    }

    public override HkRigidBody RigidBody2
    {
      get => this.WeldInfo.Parent == null ? this.m_rigidBody2 : this.WeldInfo.Parent.RigidBody2;
      protected set
      {
        if (!((HkReferenceObject) this.m_rigidBody2 != (HkReferenceObject) value))
          return;
        this.m_rigidBody2 = value;
        if (this.NeedsActivationCallback)
          this.RegisterActivationCallbacksIfNeeded();
        else
          this.UnregisterActivationCallbacks();
      }
    }

    public override float Mass
    {
      get
      {
        if (this.CharacterProxy != null)
          return this.CharacterProxy.Mass;
        return (HkReferenceObject) this.RigidBody != (HkReferenceObject) null ? (MyMultiplayer.Static != null && !Sync.IsServer ? this.m_animatedClientMass : this.RigidBody.Mass) : (this.Ragdoll != null ? this.Ragdoll.Mass : 0.0f);
      }
    }

    public override float Speed => this.LinearVelocity.Length();

    public override float Friction
    {
      get => this.RigidBody.Friction;
      set => this.RigidBody.Friction = value;
    }

    public override bool IsStatic => (HkReferenceObject) this.RigidBody != (HkReferenceObject) null && this.RigidBody.IsFixed;

    public override bool IsKinematic => (HkReferenceObject) this.RigidBody != (HkReferenceObject) null && !this.RigidBody.IsFixed && this.RigidBody.IsFixedOrKeyframed;

    public bool IsSubpart { get; set; }

    public override bool IsActive
    {
      get
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          return this.RigidBody.IsActive;
        if (this.CharacterProxy != null)
          return this.CharacterProxy.GetHitRigidBody().IsActive;
        return this.Ragdoll != null && this.Ragdoll.IsActive;
      }
    }

    protected override void CloseRigidBody()
    {
      if (this.IsWelded)
        this.WeldInfo.Parent.Unweld(this, false);
      if (this.WeldInfo.Children.Count != 0)
        MyWeldingGroups.ReplaceParent(MyWeldingGroups.Static.GetGroup((MyEntity) this.Entity), (MyEntity) this.Entity, (MyEntity) null);
      this.CheckRBNotInWorld();
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        if (!this.RigidBody.IsDisposed)
          this.RigidBody.Dispose();
        this.RigidBody = (HkRigidBody) null;
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        this.RigidBody2.Dispose();
        this.RigidBody2 = (HkRigidBody) null;
      }
      if ((HkReferenceObject) this.BreakableBody != (HkReferenceObject) null)
      {
        this.BreakableBody.Dispose();
        this.BreakableBody = (HkdBreakableBody) null;
      }
      if (!((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null))
        return;
      this.WeldedRigidBody.Dispose();
      this.WeldedRigidBody = (HkRigidBody) null;
    }

    public MyCharacterProxy CharacterProxy { get; set; }

    public int CharacterSystemGroupCollisionFilterID { get; private set; }

    public uint CharacterCollisionFilter { get; private set; }

    public override bool IsInWorld
    {
      get => this.m_isInWorld;
      protected set => this.m_isInWorld = value;
    }

    public override bool ShapeChangeInProgress
    {
      get => this.m_shapeChangeInProgress;
      set => this.m_shapeChangeInProgress = value;
    }

    public override Vector3 AngularVelocityLocal
    {
      get
      {
        if (!this.Enabled)
          return Vector3.Zero;
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          return MyMultiplayer.Static != null && !Sync.IsServer && this.IsStatic ? base.AngularVelocity : this.RigidBody.AngularVelocity;
        if (this.CharacterProxy != null)
          return this.CharacterProxy.AngularVelocity;
        return this.Ragdoll != null && this.Ragdoll.IsActive ? this.Ragdoll.GetRootRigidBody().AngularVelocity : base.AngularVelocity;
      }
    }

    public override Vector3 LinearVelocityLocal
    {
      get
      {
        if (!this.Enabled)
          return Vector3.Zero;
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          return MyMultiplayer.Static != null && !Sync.IsServer && this.IsStatic ? base.LinearVelocity : this.RigidBody.LinearVelocity;
        if (this.CharacterProxy != null)
          return this.CharacterProxy.LinearVelocity;
        return this.Ragdoll != null && this.Ragdoll.IsActive ? this.Ragdoll.GetRootRigidBody().LinearVelocity : base.LinearVelocity;
      }
    }

    public override Vector3 LinearVelocity
    {
      get => this.GetLinearVelocity(true);
      set => this.SetLinearVelocity(value, true);
    }

    public override Vector3 LinearVelocityUnsafe
    {
      get => this.GetLinearVelocity(false);
      set => this.SetLinearVelocity(value, true);
    }

    private Vector3 GetLinearVelocity(bool safe)
    {
      if (!this.Enabled)
        return Vector3.Zero;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        if (MyMultiplayer.Static != null && !Sync.IsServer)
        {
          if (this.Entity is MyCubeGrid entity)
          {
            MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entity.ClosestParentId);
            if (entityById != null && entityById.Physics != null)
              return safe ? entityById.Physics.LinearVelocity + this.RigidBody.LinearVelocity : entityById.Physics.LinearVelocityUnsafe + this.RigidBody.LinearVelocityUnsafe;
            if (this.IsStatic)
              return safe ? base.LinearVelocity : base.LinearVelocityUnsafe;
          }
          else if (this.IsStatic)
          {
            if (this.Entity is MyCharacter entity)
            {
              MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entity.ClosestParentId);
              if (entityById != null && entityById.Physics != null)
              {
                if (entity.InheritRotation)
                {
                  Vector3D position = this.Entity.PositionComp.GetPosition();
                  Vector3 linearVelocity;
                  entityById.Physics.GetVelocityAtPointLocal(ref position, out linearVelocity);
                  return safe ? linearVelocity + base.LinearVelocity : linearVelocity + base.LinearVelocityUnsafe;
                }
                return safe ? entityById.Physics.LinearVelocity + base.LinearVelocity : entityById.Physics.LinearVelocityUnsafe + base.LinearVelocityUnsafe;
              }
            }
            return safe ? base.LinearVelocity : base.LinearVelocityUnsafe;
          }
        }
        return safe ? this.RigidBody.LinearVelocity : this.RigidBody.LinearVelocityUnsafe;
      }
      if (this.CharacterProxy != null)
      {
        if (MyMultiplayer.Static != null && !Sync.IsServer)
        {
          MyCharacter entity = (MyCharacter) this.Entity;
          MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entity.ClosestParentId);
          if (entityById != null && entityById.Physics != null)
          {
            if (entity.InheritRotation)
            {
              Vector3D position = this.Entity.PositionComp.GetPosition();
              Vector3 linearVelocity;
              entityById.Physics.GetVelocityAtPointLocal(ref position, out linearVelocity);
              return linearVelocity + this.CharacterProxy.LinearVelocity;
            }
            return safe ? entityById.Physics.LinearVelocity + this.CharacterProxy.LinearVelocity : entityById.Physics.LinearVelocityUnsafe + this.CharacterProxy.LinearVelocity;
          }
        }
        return this.CharacterProxy.LinearVelocity;
      }
      return this.Ragdoll != null && this.Ragdoll.IsActive ? (safe ? this.Ragdoll.GetRootRigidBody().LinearVelocity : this.Ragdoll.GetRootRigidBody().LinearVelocityUnsafe) : (safe ? base.LinearVelocity : base.LinearVelocityUnsafe);
    }

    private void SetLinearVelocity(Vector3 value, bool safe)
    {
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        if (safe)
          this.RigidBody.LinearVelocity = value;
        else
          this.RigidBody.LinearVelocityUnsafe = value;
      }
      if (this.CharacterProxy != null)
        this.CharacterProxy.LinearVelocity = value;
      if (this.Ragdoll != null && this.Ragdoll.IsActive)
      {
        foreach (HkRigidBody rigidBody in this.Ragdoll.RigidBodies)
        {
          if (safe)
            rigidBody.LinearVelocity = value;
          else
            rigidBody.LinearVelocityUnsafe = value;
        }
      }
      if (safe)
        base.LinearVelocity = value;
      else
        base.LinearVelocityUnsafe = value;
    }

    public override float LinearDamping
    {
      get => this.RigidBody.LinearDamping;
      set
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.RigidBody.LinearDamping = value;
        this.m_linearDamping = value;
      }
    }

    public override float AngularDamping
    {
      get => this.RigidBody.AngularDamping;
      set
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.RigidBody.AngularDamping = value;
        this.m_angularDamping = value;
      }
    }

    public override Vector3 AngularVelocity
    {
      get
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          return MyMultiplayer.Static != null && !Sync.IsServer && this.IsStatic ? base.AngularVelocity : this.RigidBody.AngularVelocity;
        if (this.CharacterProxy != null)
          return this.CharacterProxy.AngularVelocity;
        return this.Ragdoll != null && this.Ragdoll.IsActive ? this.Ragdoll.GetRootRigidBody().AngularVelocity : base.AngularVelocity;
      }
      set
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.RigidBody.AngularVelocity = value;
        if (this.CharacterProxy != null)
          this.CharacterProxy.AngularVelocity = value;
        if (this.Ragdoll != null && this.Ragdoll.IsActive)
        {
          foreach (HkEntity rigidBody in this.Ragdoll.RigidBodies)
            rigidBody.AngularVelocity = value;
        }
        base.AngularVelocity = value;
      }
    }

    public override Vector3 SupportNormal
    {
      get => this.CharacterProxy != null ? this.CharacterProxy.SupportNormal : base.SupportNormal;
      set => base.SupportNormal = value;
    }

    public MyPhysicsBody()
    {
    }

    public MyPhysicsBody(IMyEntity entity, RigidBodyFlag flags)
    {
      this.Entity = entity;
      this.m_enabled = false;
      this.Flags = flags;
      this.IsSubpart = false;
    }

    private void OnContactSoundCallback(ref HkContactPointEvent e)
    {
      if (!Sync.IsServer || !MyAudioComponent.ShouldPlayContactSound(this.Entity.EntityId, e.EventType))
        return;
      ContactPointWrapper wrap = new ContactPointWrapper(ref e);
      wrap.WorldPosition = this.ClusterToWorld(wrap.position);
      MySandboxGame.Static.Invoke((Action) (() => MyAudioComponent.PlayContactSound(wrap, this.Entity)), "MyAudioComponent::PlayContactSound");
    }

    public override void Close()
    {
      this.CloseRagdoll();
      base.Close();
      if (this.CharacterProxy == null)
        return;
      this.CharacterProxy.Dispose();
      this.CharacterProxy = (MyCharacterProxy) null;
    }

    public override void AddForce(
      MyPhysicsForceType type,
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      float? maxSpeed = null,
      bool applyImmediately = true,
      bool activeOnly = false)
    {
      if (applyImmediately)
      {
        this.AddForceInternal(type, force, position, torque, maxSpeed, activeOnly);
      }
      else
      {
        if (!activeOnly && !this.IsActive)
        {
          if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
            this.RigidBody.Activate();
          else if (this.CharacterProxy != null)
            this.CharacterProxy.GetHitRigidBody().Activate();
          else if (this.Ragdoll != null)
            this.Ragdoll.Activate();
        }
        lock (MyPhysics.QueuedForces)
          MyPhysics.QueuedForces.Enqueue(new MyPhysics.ForceInfo(this, activeOnly, maxSpeed, force, torque, position, type));
      }
    }

    private void AddForceInternal(
      MyPhysicsForceType type,
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      float? maxSpeed,
      bool activeOnly)
    {
      if (this.IsStatic || activeOnly && !this.IsActive)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_FORCES)
        MyPhysicsDebugDraw.DebugDrawAddForce(this, type, force, position, torque);
      switch (type)
      {
        case MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE:
          this.ApplyImplusesWorld(force, position, torque, this.RigidBody);
          if (this.CharacterProxy != null && force.HasValue)
            this.CharacterProxy.ApplyLinearImpulse(force.Value);
          if (this.Ragdoll != null && this.Ragdoll.InWorld && !this.Ragdoll.IsKeyframed)
          {
            this.ApplyImpuseOnRagdoll(force, position, torque, this.Ragdoll);
            break;
          }
          break;
        case MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE:
          Matrix matrix;
          if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          {
            this.RigidBody.GetRigidBodyMatrix(out matrix);
            Vector3? force1 = force;
            Vector3? torque1 = torque;
            Vector3D? nullable = position;
            Vector3? position1 = nullable.HasValue ? new Vector3?((Vector3) nullable.GetValueOrDefault()) : new Vector3?();
            HkRigidBody rigidBody = this.RigidBody;
            ref Matrix local = ref matrix;
            this.AddForceTorqueBody(force1, torque1, position1, rigidBody, ref local);
          }
          MatrixD worldMatrix;
          if (this.CharacterProxy != null && (HkReferenceObject) this.CharacterProxy.GetHitRigidBody() != (HkReferenceObject) null)
          {
            worldMatrix = this.Entity.WorldMatrix;
            matrix = (Matrix) ref worldMatrix;
            Vector3? force1 = force;
            Vector3? torque1 = torque;
            Vector3D? nullable = position;
            Vector3? position1 = nullable.HasValue ? new Vector3?((Vector3) nullable.GetValueOrDefault()) : new Vector3?();
            HkRigidBody hitRigidBody = this.CharacterProxy.GetHitRigidBody();
            ref Matrix local = ref matrix;
            this.AddForceTorqueBody(force1, torque1, position1, hitRigidBody, ref local);
          }
          if (this.Ragdoll != null && this.Ragdoll.InWorld && !this.Ragdoll.IsKeyframed)
          {
            worldMatrix = this.Entity.WorldMatrix;
            matrix = (Matrix) ref worldMatrix;
            this.ApplyForceTorqueOnRagdoll(force, torque, this.Ragdoll, ref matrix);
            break;
          }
          break;
        case MyPhysicsForceType.APPLY_WORLD_FORCE:
          this.ApplyForceWorld(force, position, this.RigidBody);
          if (this.CharacterProxy != null)
          {
            if (this.CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
              this.CharacterProxy.ApplyLinearImpulse(force.Value / this.Mass * 10f);
            else
              this.CharacterProxy.ApplyLinearImpulse(force.Value / this.Mass);
          }
          if (this.Ragdoll != null && this.Ragdoll.InWorld && !this.Ragdoll.IsKeyframed)
          {
            this.ApplyForceOnRagdoll(force, position, this.Ragdoll);
            break;
          }
          break;
      }
      double num1 = (double) this.LinearVelocity.LengthSquared();
      float? nullable1 = maxSpeed;
      float? nullable2 = maxSpeed;
      float? nullable3 = nullable1.HasValue & nullable2.HasValue ? new float?(nullable1.GetValueOrDefault() * nullable2.GetValueOrDefault()) : new float?();
      double valueOrDefault = (double) nullable3.GetValueOrDefault();
      if (!(num1 > valueOrDefault & nullable3.HasValue))
        return;
      Vector3 linearVelocity = this.LinearVelocity;
      double num2 = (double) linearVelocity.Normalize();
      Vector3 vector3 = linearVelocity * maxSpeed.Value;
      MyEntity entity;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null && MyMultiplayer.Static != null && (!Sync.IsServer && this.Entity is MyCubeGrid) && Sandbox.Game.Entities.MyEntities.TryGetEntityById(((MyCubeGrid) this.Entity).ClosestParentId, out entity))
        vector3 -= entity.Physics.LinearVelocity;
      this.LinearVelocity = vector3;
    }

    private void ApplyForceWorld(Vector3? force, Vector3D? position, HkRigidBody rigidBody)
    {
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null || !force.HasValue || MyUtils.IsZero(force.Value))
        return;
      if (position.HasValue)
      {
        Vector3 point = (Vector3) (position.Value - this.Offset);
        rigidBody.ApplyForce(0.01666667f, force.Value, point);
      }
      else
        rigidBody.ApplyForce(0.01666667f, force.Value);
    }

    private void ApplyImplusesWorld(
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      HkRigidBody rigidBody)
    {
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return;
      if (force.HasValue && position.HasValue)
        rigidBody.ApplyPointImpulse(force.Value, (Vector3) (position.Value - this.Offset));
      if (!torque.HasValue)
        return;
      rigidBody.ApplyAngularImpulse(torque.Value * 0.01666667f * MyFakes.SIMULATION_SPEED);
    }

    private void AddForceTorqueBody(
      Vector3? force,
      Vector3? torque,
      Vector3? position,
      HkRigidBody rigidBody,
      ref Matrix transform)
    {
      if (force.HasValue && !MyUtils.IsZero(force.Value))
      {
        Vector3 result1 = force.Value;
        Vector3.TransformNormal(ref result1, ref transform, out result1);
        if (position.HasValue)
        {
          Vector3 result2 = position.Value;
          Vector3.Transform(ref result2, ref transform, out result2);
          this.ApplyForceWorld(new Vector3?(result1), new Vector3D?(result2 + this.Offset), rigidBody);
        }
        else
          rigidBody.ApplyLinearImpulse(result1 * 0.01666667f * MyFakes.SIMULATION_SPEED);
      }
      if (!torque.HasValue || MyUtils.IsZero(torque.Value))
        return;
      Vector3 vector3_1 = Vector3.TransformNormal(torque.Value, transform);
      rigidBody.ApplyAngularImpulse(vector3_1 * 0.01666667f * MyFakes.SIMULATION_SPEED);
      Vector3 angularVelocity = rigidBody.AngularVelocity;
      float maxAngularVelocity = rigidBody.MaxAngularVelocity;
      if ((double) angularVelocity.LengthSquared() <= (double) maxAngularVelocity * (double) maxAngularVelocity)
        return;
      double num = (double) angularVelocity.Normalize();
      Vector3 vector3_2 = angularVelocity * maxAngularVelocity;
      rigidBody.AngularVelocity = vector3_2;
    }

    public override void ApplyImpulse(Vector3 impulse, Vector3D pos) => this.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(impulse), new Vector3D?(pos), new Vector3?(), new float?(), true, false);

    public override void ClearSpeed()
    {
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        this.RigidBody.LinearVelocity = Vector3.Zero;
        this.RigidBody.AngularVelocity = Vector3.Zero;
      }
      if (this.CharacterProxy == null)
        return;
      this.CharacterProxy.LinearVelocity = Vector3.Zero;
      this.CharacterProxy.AngularVelocity = Vector3.Zero;
      this.CharacterProxy.PosX = 0.0f;
      this.CharacterProxy.PosY = 0.0f;
      this.CharacterProxy.Elevate = 0.0f;
    }

    public override void Clear() => this.ClearSpeed();

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS)
      {
        int num = 0;
        foreach (HkConstraint constraint in this.Constraints)
        {
          if (!constraint.IsDisposed)
          {
            Color color = Color.Green;
            if (!MyPhysicsBody.IsConstraintValid(constraint))
              color = Color.Red;
            else if (!constraint.Enabled)
              color = Color.Yellow;
            Vector3 pivotA;
            Vector3 pivotB;
            constraint.GetPivotsInWorld(out pivotA, out pivotB);
            Vector3D world1 = this.ClusterToWorld(pivotA);
            Vector3D world2 = this.ClusterToWorld(pivotB);
            MyRenderProxy.DebugDrawLine3D(world1, world2, color, color, false);
            MyRenderProxy.DebugDrawSphere(world1, 0.2f, color, depthRead: false);
            MyRenderProxy.DebugDrawText3D(world1, num.ToString() + " A", Color.White, 0.7f, true);
            MyRenderProxy.DebugDrawSphere(world2, 0.2f, color, depthRead: false);
            MyRenderProxy.DebugDrawText3D(world2, num.ToString() + " B", Color.White, 0.7f, true);
            ++num;
          }
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_INERTIA_TENSORS && (HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        Vector3D world = this.ClusterToWorld(this.RigidBody.CenterOfMassWorld);
        MyRenderProxy.DebugDrawLine3D(world, world + this.RigidBody.AngularVelocity, Color.Blue, Color.Red, false);
        float num1 = 1f / this.RigidBody.Mass;
        Matrix matrix1 = this.RigidBody.InertiaTensor;
        Vector3 scale = matrix1.Scale;
        float num2 = (float) (((double) scale.X - (double) scale.Y + (double) scale.Z) * (double) num1 * 6.0);
        float num3 = (float) ((double) scale.X * (double) num1 * 12.0) - num2;
        double d = (double) scale.Z * (double) num1 * 12.0 - (double) num2;
        float num4 = 0.505f;
        Vector3 vector3 = new Vector3(Math.Sqrt(d), Math.Sqrt((double) num2), Math.Sqrt((double) num3)) * num4;
        MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(new BoundingBoxD((Vector3D) -vector3, (Vector3D) vector3), MatrixD.Identity);
        ref MyOrientedBoundingBoxD local = ref obb;
        matrix1 = this.RigidBody.GetRigidBodyMatrix();
        MatrixD matrix2 = (MatrixD) ref matrix1;
        local.Transform(matrix2);
        obb.Center = this.CenterOfMassWorld;
        MyRenderProxy.DebugDrawOBB(obb, Color.Purple, 0.05f, false, false);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_MOTION_TYPES && (HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
        MyRenderProxy.DebugDrawText3D(this.CenterOfMassWorld, this.RigidBody.GetMotionType().ToString(), Color.Purple, 0.5f, false);
      if (!MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES || this.IsWelded)
        return;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null && (HkReferenceObject) this.BreakableBody != (HkReferenceObject) null)
      {
        Vector3D vector3D = Vector3D.Transform((Vector3D) this.BreakableBody.BreakableShape.CoM, this.RigidBody.GetRigidBodyMatrix()) + this.Offset;
        MyRenderProxy.DebugDrawSphere(this.RigidBody.CenterOfMassWorld + this.Offset, 0.2f, this.RigidBody.GetMotionType() != HkMotionType.Box_Inertia ? Color.Gray : (this.RigidBody.IsActive ? Color.Red : Color.Blue), depthRead: false);
        MyRenderProxy.DebugDrawAxis(this.Entity.PositionComp.WorldMatrixRef, 0.2f, false);
      }
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        int shapeIndex = 0;
        Matrix rigidBodyMatrix = this.RigidBody.GetRigidBodyMatrix();
        MatrixD world = MatrixD.CreateWorld(rigidBodyMatrix.Translation + this.Offset, rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
        MyPhysicsDebugDraw.DrawCollisionShape(this.RigidBody.GetShape(), world, 0.3f, ref shapeIndex);
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        int shapeIndex = 0;
        Matrix rigidBodyMatrix = this.RigidBody2.GetRigidBodyMatrix();
        MatrixD world = MatrixD.CreateWorld(rigidBodyMatrix.Translation + this.Offset, rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
        MyPhysicsDebugDraw.DrawCollisionShape(this.RigidBody2.GetShape(), world, 0.3f, ref shapeIndex);
      }
      if (this.CharacterProxy == null)
        return;
      int shapeIndex1 = 0;
      Matrix rigidBodyTransform = this.CharacterProxy.GetRigidBodyTransform();
      MatrixD world3 = MatrixD.CreateWorld(rigidBodyTransform.Translation + this.Offset, rigidBodyTransform.Forward, rigidBodyTransform.Up);
      MyPhysicsDebugDraw.DrawCollisionShape(this.CharacterProxy.GetShape(), world3, 0.3f, ref shapeIndex1);
    }

    public virtual void CreateFromCollisionObject(
      HkShape shape,
      Vector3 center,
      MatrixD worldTransform,
      HkMassProperties? massProperties = null,
      int collisionFilter = 15)
    {
      this.CloseRigidBody();
      this.Center = center;
      this.CanUpdateAccelerations = true;
      this.CreateBody(ref shape, massProperties);
      this.RigidBody.UserObject = (object) this;
      this.RigidBody.SetWorldMatrix((Matrix) ref worldTransform);
      this.RigidBody.Layer = collisionFilter;
      if ((this.Flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) <= RigidBodyFlag.RBF_DEFAULT)
        return;
      this.RigidBody.Layer = 19;
    }

    protected virtual void CreateBody(ref HkShape shape, HkMassProperties? massProperties)
    {
      HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
      hkRigidBodyCinfo.AngularDamping = this.m_angularDamping;
      hkRigidBodyCinfo.LinearDamping = this.m_linearDamping;
      hkRigidBodyCinfo.Shape = shape;
      hkRigidBodyCinfo.SolverDeactivation = this.InitialSolverDeactivation;
      hkRigidBodyCinfo.ContactPointCallbackDelay = this.ContactPointDelay;
      if (massProperties.HasValue)
      {
        this.m_animatedClientMass = massProperties.Value.Mass;
        hkRigidBodyCinfo.SetMassProperties(massProperties.Value);
      }
      MyPhysicsBody.GetInfoFromFlags(hkRigidBodyCinfo, this.Flags);
      this.RigidBody = new HkRigidBody(hkRigidBodyCinfo);
    }

    protected static void GetInfoFromFlags(HkRigidBodyCinfo rbInfo, RigidBodyFlag flags)
    {
      if ((flags & RigidBodyFlag.RBF_STATIC) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Fixed;
        rbInfo.QualityType = HkCollidableQualityType.Fixed;
      }
      else if ((flags & RigidBodyFlag.RBF_BULLET) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Dynamic;
        rbInfo.QualityType = HkCollidableQualityType.Bullet;
      }
      else if ((flags & RigidBodyFlag.RBF_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Keyframed;
        rbInfo.QualityType = HkCollidableQualityType.Keyframed;
      }
      else if ((flags & RigidBodyFlag.RBF_DOUBLED_KINEMATIC) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Dynamic;
        rbInfo.QualityType = HkCollidableQualityType.Moving;
      }
      else if ((flags & RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Fixed;
        rbInfo.QualityType = HkCollidableQualityType.Fixed;
      }
      else if ((flags & RigidBodyFlag.RBF_DEBRIS) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Dynamic;
        rbInfo.QualityType = HkCollidableQualityType.Debris;
        rbInfo.SolverDeactivation = HkSolverDeactivation.Max;
      }
      else if ((flags & RigidBodyFlag.RBF_KEYFRAMED_REPORTING) > RigidBodyFlag.RBF_DEFAULT)
      {
        rbInfo.MotionType = HkMotionType.Keyframed;
        rbInfo.QualityType = HkCollidableQualityType.KeyframedReporting;
      }
      else
      {
        rbInfo.MotionType = HkMotionType.Dynamic;
        rbInfo.QualityType = HkCollidableQualityType.Moving;
      }
      if ((flags & RigidBodyFlag.RBF_UNLOCKED_SPEEDS) <= RigidBodyFlag.RBF_DEFAULT)
        return;
      rbInfo.MaxLinearVelocity = MyGridPhysics.LargeShipMaxLinearVelocity() * 10f;
      rbInfo.MaxAngularVelocity = MyGridPhysics.GetLargeShipMaxAngularVelocity() * 10f;
    }

    public override void CreateCharacterCollision(
      Vector3 center,
      float characterWidth,
      float characterHeight,
      float crouchHeight,
      float ladderHeight,
      float headSize,
      float headHeight,
      MatrixD worldTransform,
      float mass,
      ushort collisionLayer,
      bool isOnlyVertical,
      float maxSlope,
      float maxImpulse,
      float maxSpeedRelativeToShip,
      bool networkProxy,
      float? maxForce = null)
    {
      this.Center = center;
      this.CanUpdateAccelerations = false;
      if (networkProxy)
      {
        float downOffset = ((MyCharacter) this.Entity).IsCrouching ? 1f : 0.0f;
        this.CreateFromCollisionObject(MyCharacterProxy.CreateCharacterShape(characterHeight, characterWidth, characterHeight + headHeight, headSize, 0.0f, downOffset), center, worldTransform, collisionFilter: ((int) collisionLayer));
        this.CanUpdateAccelerations = false;
      }
      else
      {
        Matrix world = Matrix.CreateWorld((Vector3) (Vector3.TransformNormal(this.Center, worldTransform) + worldTransform.Translation), (Vector3) worldTransform.Forward, (Vector3) worldTransform.Up);
        this.CharacterProxy = new MyCharacterProxy(true, true, characterWidth, characterHeight, crouchHeight, ladderHeight, headSize, headHeight, world.Translation, (Vector3) worldTransform.Up, (Vector3) worldTransform.Forward, mass, this, isOnlyVertical, maxSlope, maxImpulse, maxSpeedRelativeToShip, maxForce);
        this.CharacterProxy.GetRigidBody().ContactPointCallbackDelay = 0;
      }
    }

    protected virtual void ActivateCollision() => ((MyEntity) this.Entity).RaisePhysicsChanged();

    public override void Deactivate()
    {
      if (this.ClusterObjectID != ulong.MaxValue)
      {
        if (this.IsWelded)
        {
          this.Unweld(false);
        }
        else
        {
          MyPhysics.RemoveObject(this.ClusterObjectID);
          this.ClusterObjectID = ulong.MaxValue;
          this.CheckRBNotInWorld();
        }
      }
      else
      {
        IMyEntity topMostParent = this.Entity.GetTopMostParent();
        if (topMostParent.Physics == null || !this.IsInWorld)
          return;
        if (((MyPhysicsBody) topMostParent.Physics).HavokWorld != null)
        {
          this.Deactivate((object) this.m_world);
        }
        else
        {
          this.RigidBody = (HkRigidBody) null;
          this.RigidBody2 = (HkRigidBody) null;
          this.CharacterProxy = (MyCharacterProxy) null;
        }
      }
    }

    private void CheckRBNotInWorld()
    {
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null && this.RigidBody.InWorld)
        this.RigidBody.RemoveFromWorld();
      if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null) || !this.RigidBody2.InWorld)
        return;
      this.RigidBody2.RemoveFromWorld();
    }

    public virtual void Deactivate(object world)
    {
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null && !this.RigidBody.InWorld)
        return;
      if (this.IsRagdollModeActive)
      {
        this.ReactivateRagdoll = true;
        this.CloseRagdollMode(world as HkWorld);
      }
      if (this.IsInWorld && (HkReferenceObject) this.RigidBody != (HkReferenceObject) null && !this.RigidBody.IsActive)
      {
        if (!this.RigidBody.IsFixed)
        {
          this.RigidBody.Activate();
        }
        else
        {
          BoundingBoxD worldAabb = this.Entity.PositionComp.WorldAABB;
          worldAabb.Inflate(0.5);
          MyPhysics.ActivateInBox(ref worldAabb);
        }
      }
      if (this.m_constraints.Count > 0)
      {
        this.m_world.LockCriticalOperations();
        foreach (HkConstraint constraint in this.m_constraints)
        {
          if (!constraint.IsDisposed)
            this.m_world.RemoveConstraint(constraint);
        }
        this.m_world.UnlockCriticalOperations();
      }
      if ((HkReferenceObject) this.BreakableBody != (HkReferenceObject) null && (HkReferenceObject) this.m_world.DestructionWorld != (HkReferenceObject) null)
        this.m_world.DestructionWorld.RemoveBreakableBody(this.BreakableBody);
      else if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null && !this.RigidBody.IsDisposed)
        this.m_world.RemoveRigidBody(this.RigidBody);
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null && !this.RigidBody2.IsDisposed)
        this.m_world.RemoveRigidBody(this.RigidBody2);
      if (this.CharacterProxy != null)
        this.CharacterProxy.Deactivate(this.m_world);
      this.CheckRBNotInWorld();
      this.m_world = (HkWorld) null;
      this.IsInWorld = false;
    }

    private void DeactivateBatchInternal(object world)
    {
      if (this.m_world == null)
        return;
      if (this.IsRagdollModeActive)
      {
        this.ReactivateRagdoll = true;
        this.CloseRagdollMode(world as HkWorld);
      }
      if ((HkReferenceObject) this.BreakableBody != (HkReferenceObject) null && (HkReferenceObject) this.m_world.DestructionWorld != (HkReferenceObject) null)
        this.m_world.DestructionWorld.RemoveBreakableBody(this.BreakableBody);
      else if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
        this.m_world.RemoveRigidBodyBatch(this.RigidBody);
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
        this.m_world.RemoveRigidBodyBatch(this.RigidBody2);
      if (this.CharacterProxy != null)
        this.CharacterProxy.Deactivate(this.m_world);
      foreach (HkConstraint constraint in this.m_constraints)
      {
        if (MyPhysicsBody.IsConstraintValid(constraint, false))
          this.m_constraintsRemoveBatch.Add(constraint);
      }
      this.m_enabled = false;
      if (this.EnabledChanged != null)
        this.EnabledChanged();
      this.m_world = (HkWorld) null;
      this.IsInWorld = false;
    }

    public virtual void DeactivateBatch(object world)
    {
      MyHierarchyComponentBase hierarchy = this.Entity.Hierarchy;
      if (hierarchy != null)
      {
        hierarchy.GetChildrenRecursive(this.m_batchedChildren);
        foreach (IMyEntity batchedChild in this.m_batchedChildren)
        {
          if (batchedChild.Physics != null && batchedChild.Physics.Enabled)
            this.m_batchedBodies.Add((MyPhysicsBody) batchedChild.Physics);
        }
        this.m_batchedChildren.Clear();
      }
      foreach (MyPhysicsBody batchedBody in this.m_batchedBodies)
        batchedBody.DeactivateBatchInternal(world);
      this.DeactivateBatchInternal(world);
    }

    public void FinishAddBatch()
    {
      this.ActivateCollision();
      if (this.EnabledChanged != null)
        this.EnabledChanged();
      foreach (HkConstraint constraint in this.m_constraintsAddBatch)
      {
        if (MyPhysicsBody.IsConstraintValid(constraint))
          this.m_world.AddConstraint(constraint);
      }
      this.m_constraintsAddBatch.Clear();
      if (this.CharacterProxy != null && (HkReferenceObject) this.CharacterProxy.GetRigidBody() != (HkReferenceObject) null)
        MyPhysics.RefreshCollisionFilter(this);
      if (!this.ReactivateRagdoll)
        return;
      this.GetRigidBodyMatrix(out this.m_bodyMatrix, false);
      this.ActivateRagdoll(this.m_bodyMatrix);
      this.ReactivateRagdoll = false;
    }

    public void FinishRemoveBatch(object userData)
    {
      HkWorld world = (HkWorld) userData;
      foreach (HkConstraint constraint in this.m_constraintsRemoveBatch)
      {
        if (MyPhysicsBody.IsConstraintValid(constraint, false))
          world.RemoveConstraint(constraint);
      }
      if (this.IsRagdollModeActive)
      {
        this.ReactivateRagdoll = true;
        this.CloseRagdollMode(world);
      }
      this.m_constraintsRemoveBatch.Clear();
    }

    public override void ForceActivate()
    {
      if (!this.IsInWorld || !((HkReferenceObject) this.RigidBody != (HkReferenceObject) null))
        return;
      this.RigidBody.ForceActivate();
      this.m_world.RigidBodyActivated((HkEntity) this.RigidBody);
    }

    public override bool IsMoving => !Vector3.IsZero(this.LinearVelocity) || !Vector3.IsZero(this.AngularVelocity);

    public override Vector3 Gravity
    {
      get
      {
        if (!this.Enabled)
          return Vector3.Zero;
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          return this.RigidBody.Gravity;
        return this.CharacterProxy != null ? this.CharacterProxy.Gravity : Vector3.Zero;
      }
      set
      {
        HkRigidBody rigidBody = this.RigidBody;
        if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
          rigidBody.Gravity = value;
        if (this.CharacterProxy == null)
          return;
        this.CharacterProxy.Gravity = value;
      }
    }

    public void OnMotionKinematic()
    {
      HkRigidBody rigidBody = this.RigidBody;
      if (!rigidBody.MarkedForVelocityRecompute)
        return;
      rigidBody.SetCustomVelocity(rigidBody.LinearVelocity, true);
    }

    public void OnMotionDynamic()
    {
      IMyEntity entity = this.Entity;
      if (entity == null || this.IsPhantom || (this.Flags & (RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE | RigidBodyFlag.RBF_NO_POSITION_UPDATES)) != RigidBodyFlag.RBF_DEFAULT || !this.IsSubpart && entity.Parent != null)
        return;
      if (this.CanUpdateAccelerations)
        this.UpdateAccelerations();
      HkRigidBody rigidBody = this.RigidBody;
      Vector3 translation = this.m_bodyMatrix.Translation;
      rigidBody.GetRigidBodyMatrix(out this.m_bodyMatrix);
      bool flag = (double) (this.m_bodyMatrix.Translation - translation).LengthSquared() > 9.99999997475243E-07;
      if (!flag)
      {
        flag = (double) rigidBody.AngularVelocity.LengthSquared() > 9.99999974737875E-05;
        if (!flag)
        {
          flag = (double) rigidBody.LinearVelocity.LengthSquared() > 9.99999974737875E-05;
          if (!flag)
            flag = this.m_motionCounter++ > (entity is MyFloatingObject ? 600 : 60);
        }
      }
      if (flag)
      {
        this.m_motionCounter = 0;
        MatrixD matrix;
        this.GetWorldMatrix(out matrix);
        entity.PositionComp.SetWorldMatrix(ref matrix, (object) this);
        this.UpdateCluster();
        this.UpdateInterpolatedVelocities(rigidBody, true);
      }
      else
        this.UpdateInterpolatedVelocities(rigidBody, false);
    }

    private void UpdateInterpolatedVelocities(HkRigidBody rb, bool moved)
    {
      if (rb.MarkedForVelocityRecompute)
      {
        Vector3D centerOfMassWorld = this.CenterOfMassWorld;
        if (this.m_lastComPosition.HasValue && this.m_lastComLocal == rb.CenterOfMassLocal)
        {
          Vector3 velocity = Vector3.Zero;
          if (moved)
            velocity = (Vector3) ((centerOfMassWorld - this.m_lastComPosition.Value) / 0.0166666675359011);
          rb.SetCustomVelocity(velocity, true);
        }
        rb.MarkedForVelocityRecompute = false;
        this.m_lastComPosition = new Vector3D?(centerOfMassWorld);
        this.m_lastComLocal = rb.CenterOfMassLocal;
      }
      else
      {
        if (!this.m_lastComPosition.HasValue)
          return;
        this.m_lastComPosition = new Vector3D?();
        rb.SetCustomVelocity(Vector3.Zero, false);
      }
    }

    public void SynchronizeKeyframedRigidBody()
    {
      if (!((HkReferenceObject) this.RigidBody != (HkReferenceObject) null) || !((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null) || this.RigidBody.IsActive == this.RigidBody2.IsActive)
        return;
      if (this.RigidBody.IsActive)
      {
        this.RigidBody2.IsActive = true;
      }
      else
      {
        this.RigidBody2.LinearVelocity = Vector3.Zero;
        this.RigidBody2.AngularVelocity = Vector3.Zero;
        this.RigidBody2.IsActive = false;
      }
    }

    public override sealed void GetWorldMatrix(out MatrixD entityMatrix)
    {
      if (this.WeldInfo.Parent != null)
      {
        MatrixD matrix;
        this.WeldInfo.Parent.GetWorldMatrix(out matrix);
        MatrixD.Multiply(ref this.WeldInfo.Transform, ref matrix, out entityMatrix);
      }
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        this.RigidBody.GetRigidBodyMatrix(out entityMatrix);
        entityMatrix.Translation += this.Offset;
      }
      else if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        this.RigidBody2.GetRigidBodyMatrix(out entityMatrix);
        entityMatrix.Translation += this.Offset;
      }
      else if (this.CharacterProxy != null)
      {
        Matrix rigidBodyTransform = this.CharacterProxy.GetRigidBodyTransform();
        MatrixD matrixD = (MatrixD) ref rigidBodyTransform;
        matrixD.Translation = this.CharacterProxy.Position + this.Offset;
        entityMatrix = matrixD;
      }
      else
      {
        if (this.Ragdoll != null & this.IsRagdollModeActive)
        {
          ref MatrixD local = ref entityMatrix;
          Matrix worldMatrix = this.Ragdoll.WorldMatrix;
          MatrixD matrixD = (MatrixD) ref worldMatrix;
          local = matrixD;
          entityMatrix.Translation += this.Offset;
          return;
        }
        entityMatrix = MatrixD.Identity;
      }
      if (!(this.Center != Vector3.Zero))
        return;
      entityMatrix.Translation -= Vector3D.TransformNormal((Vector3D) this.Center, ref entityMatrix);
    }

    public override Vector3 GetVelocityAtPoint(Vector3D worldPos)
    {
      Vector3 cluster = (Vector3) this.WorldToCluster(worldPos);
      return (HkReferenceObject) this.RigidBody != (HkReferenceObject) null ? this.RigidBody.GetVelocityAtPoint(cluster) : Vector3.Zero;
    }

    public override void GetVelocityAtPointLocal(ref Vector3D worldPos, out Vector3 linearVelocity)
    {
      Vector3 vector2 = (Vector3) (worldPos - this.CenterOfMassWorld);
      linearVelocity = Vector3.Cross(this.AngularVelocityLocal, vector2);
      linearVelocity.Add(this.LinearVelocity);
    }

    public override void OnWorldPositionChanged(object source)
    {
      if (!this.IsInWorld)
        return;
      Vector3 velocity = Vector3.Zero;
      IMyEntity topMostParent = this.Entity.GetTopMostParent();
      if (topMostParent.Physics != null)
        velocity = topMostParent.Physics.LinearVelocity;
      if (!this.IsWelded && this.ClusterObjectID != ulong.MaxValue)
        MyPhysics.MoveObject(this.ClusterObjectID, topMostParent.WorldAABB, velocity);
      Matrix m1;
      this.GetRigidBodyMatrix(out m1);
      if (m1.EqualsFast(ref this.m_bodyMatrix) && this.CharacterProxy == null)
        return;
      this.m_bodyMatrix = m1;
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
        this.RigidBody.SetWorldMatrix(this.m_bodyMatrix);
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
        this.RigidBody2.SetWorldMatrix(this.m_bodyMatrix);
      if (this.CharacterProxy != null)
      {
        this.CharacterProxy.Speed = 0.0f;
        this.CharacterProxy.SetRigidBodyTransform(ref this.m_bodyMatrix);
      }
      if (this.Ragdoll == null || !this.IsRagdollModeActive)
        return;
      bool flag1 = source is MyCockpit;
      bool flag2 = source == MyGridPhysicalHierarchy.Static;
      if (!(flag1 | flag2))
        return;
      if (flag1)
        this.Ragdoll.ResetToRigPose();
      Matrix m2;
      this.GetRigidBodyMatrix(out m2, false);
      MyCharacter entity = (MyCharacter) this.Entity;
      bool flag3 = flag2 && !entity.IsClientPredicted;
      bool flag4 = entity.m_positionResetFromServer || Vector3D.DistanceSquared((Vector3D) this.Ragdoll.WorldMatrix.Translation, (Vector3D) m2.Translation) > 0.5;
      this.Ragdoll.SetWorldMatrix(m2, !flag4 & flag3, true);
      if (!flag1)
        return;
      this.SetRagdollVelocities();
    }

    protected void GetRigidBodyMatrix(out Matrix m, bool useCenterOffset = true)
    {
      MatrixD worldMatrix = this.Entity.WorldMatrix;
      if (this.Center != Vector3.Zero & useCenterOffset)
        worldMatrix.Translation += Vector3.TransformNormal(this.Center, worldMatrix);
      worldMatrix.Translation -= this.Offset;
      m = (Matrix) ref worldMatrix;
    }

    protected Matrix GetRigidBodyMatrix()
    {
      Vector3 vector3 = Vector3.TransformNormal(this.Center, this.Entity.WorldMatrix);
      Vector3D objectOffset = MyPhysics.GetObjectOffset(this.ClusterObjectID);
      Vector3 position = (Vector3) ((Vector3D) vector3 + this.Entity.GetPosition() - objectOffset);
      MatrixD worldMatrix = this.Entity.WorldMatrix;
      Vector3 forward = (Vector3) worldMatrix.Forward;
      worldMatrix = this.Entity.WorldMatrix;
      Vector3 up = (Vector3) worldMatrix.Up;
      return Matrix.CreateWorld(position, forward, up);
    }

    protected Matrix GetRigidBodyMatrix(MatrixD worldMatrix)
    {
      if (this.Center != Vector3.Zero)
        worldMatrix.Translation += Vector3D.TransformNormal((Vector3D) this.Center, ref worldMatrix);
      worldMatrix.Translation -= this.Offset;
      return (Matrix) ref worldMatrix;
    }

    public virtual void ChangeQualityType(HkCollidableQualityType quality) => this.RigidBody.Quality = quality;

    public override bool HasRigidBody => (HkReferenceObject) this.RigidBody != (HkReferenceObject) null;

    public override Vector3 CenterOfMassLocal => this.RigidBody.CenterOfMassLocal;

    public override Vector3D CenterOfMassWorld => this.RigidBody.CenterOfMassWorld + this.Offset;

    private static bool IsConstraintValid(HkConstraint constraint, bool checkBodiesInWorld)
    {
      if ((HkReferenceObject) constraint == (HkReferenceObject) null || constraint.IsDisposed)
        return false;
      HkRigidBody rigidBodyA = constraint.RigidBodyA;
      HkRigidBody rigidBodyB = constraint.RigidBodyB;
      return !((HkReferenceObject) rigidBodyA == (HkReferenceObject) null) && !((HkReferenceObject) rigidBodyB == (HkReferenceObject) null) && (!checkBodiesInWorld || rigidBodyA.InWorld && rigidBodyB.InWorld && ((MyPhysicsBody) rigidBodyA.UserObject).HavokWorld == ((MyPhysicsBody) rigidBodyB.UserObject).HavokWorld);
    }

    public static bool IsConstraintValid(HkConstraint constraint) => MyPhysicsBody.IsConstraintValid(constraint, true);

    public void AddConstraint(HkConstraint constraint)
    {
      if (this.IsWelded)
      {
        this.WeldInfo.Parent.AddConstraint(constraint);
      }
      else
      {
        if (this.HavokWorld == null || (HkReferenceObject) this.RigidBody == (HkReferenceObject) null || !MyPhysicsBody.IsConstraintValid(constraint))
          return;
        this.m_constraints.Add(constraint);
        this.HavokWorld.AddConstraint(constraint);
        if (MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS || Sync.IsServer)
          return;
        this.UpdateConstraintForceDisable(constraint);
      }
    }

    public void UpdateConstraintsForceDisable()
    {
      if (MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS || Sync.IsServer)
        return;
      foreach (HkConstraint constraint in this.m_constraints)
        this.UpdateConstraintForceDisable(constraint);
    }

    public void UpdateConstraintForceDisable(HkConstraint constraint)
    {
      if (constraint.RigidBodyA.GetEntity(0U) is MyCharacter || constraint.RigidBodyB.GetEntity(0U) is MyCharacter || !constraint.InWorld)
        return;
      if ((!(this.Entity is MyCubeGrid entity) || !entity.IsClientPredicted) && (!MyPhysicsBody.IsPhantomOrSubPart(constraint.RigidBodyA) && !MyPhysicsBody.IsPhantomOrSubPart(constraint.RigidBodyB)))
        constraint.ForceDisabled = true;
      else
        constraint.ForceDisabled = false;
    }

    private static bool IsPhantomOrSubPart(HkRigidBody rigidBody)
    {
      MyPhysicsBody userObject = (MyPhysicsBody) rigidBody.UserObject;
      return userObject.IsPhantom || userObject.IsSubpart;
    }

    public bool RemoveConstraint(HkConstraint constraint)
    {
      if (this.IsWelded)
      {
        this.m_constraints.Remove(constraint);
        this.WeldInfo.Parent.RemoveConstraint(constraint);
        return true;
      }
      this.m_constraints.Remove(constraint);
      if (this.HavokWorld == null)
        return false;
      this.HavokWorld.RemoveConstraint(constraint);
      return true;
    }

    public HashSetReader<HkConstraint> Constraints => (HashSetReader<HkConstraint>) this.m_constraints;

    public virtual bool IsStaticForCluster
    {
      get => this.m_isStaticForCluster;
      set => this.m_isStaticForCluster = value;
    }

    public override Vector3D WorldToCluster(Vector3D worldPos) => worldPos - this.Offset;

    public override Vector3D ClusterToWorld(Vector3 clusterPos) => (Vector3D) clusterPos + this.Offset;

    public void EnableBatched()
    {
      if (this.m_enabled)
        return;
      this.m_enabled = true;
      if (this.Entity.InScene)
      {
        this.m_batchRequest = true;
        this.Activate();
        this.m_batchRequest = false;
      }
      if (this.EnabledChanged == null)
        return;
      this.EnabledChanged();
    }

    public override void Activate()
    {
      if (!this.Enabled)
        return;
      IMyEntity topMostParent = this.Entity.GetTopMostParent();
      if (topMostParent != this.Entity && topMostParent.Physics != null)
      {
        this.Activate((object) ((MyPhysicsBody) topMostParent.Physics).HavokWorld, ulong.MaxValue);
      }
      else
      {
        if (this.ClusterObjectID != ulong.MaxValue)
          return;
        this.ClusterObjectID = MyPhysics.TryAddEntity(this.Entity, this, this.m_batchRequest);
      }
    }

    public virtual void Activate(object world, ulong clusterObjectID)
    {
      this.m_world = (HkWorld) world;
      if (this.m_world == null)
        return;
      this.ClusterObjectID = clusterObjectID;
      this.ActivateCollision();
      this.IsInWorld = true;
      this.GetRigidBodyMatrix(out this.m_bodyMatrix);
      if ((HkReferenceObject) this.BreakableBody != (HkReferenceObject) null)
      {
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.RigidBody.SetWorldMatrix(this.m_bodyMatrix);
        if (Sync.IsServer)
          this.m_world.DestructionWorld.AddBreakableBody(this.BreakableBody);
        else if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
          this.m_world.AddRigidBody(this.RigidBody);
      }
      else if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        this.RigidBody.SetWorldMatrix(this.m_bodyMatrix);
        this.m_world.AddRigidBody(this.RigidBody);
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        this.RigidBody2.SetWorldMatrix(this.m_bodyMatrix);
        this.m_world.AddRigidBody(this.RigidBody2);
      }
      if (this.CharacterProxy != null)
      {
        this.RagdollSystemGroupCollisionFilterID = 0;
        this.CharacterSystemGroupCollisionFilterID = this.m_world.GetCollisionFilter().GetNewSystemGroup();
        this.CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, this.CharacterSystemGroupCollisionFilterID, 0, 0);
        this.CharacterProxy.SetCollisionFilterInfo(this.CharacterCollisionFilter);
        this.CharacterProxy.SetRigidBodyTransform(ref this.m_bodyMatrix);
        this.CharacterProxy.Activate(this.m_world);
      }
      if (this.ReactivateRagdoll)
      {
        this.GetRigidBodyMatrix(out this.m_bodyMatrix, false);
        this.ActivateRagdoll(this.m_bodyMatrix);
        this.ReactivateRagdoll = false;
      }
      if (this.SwitchToRagdollModeOnActivate)
      {
        int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
        this.SwitchToRagdollModeOnActivate = false;
        this.SwitchToRagdollMode(this.m_ragdollDeadMode);
      }
      this.m_world.LockCriticalOperations();
      foreach (HkConstraint constraint in this.m_constraints)
      {
        if (MyPhysicsBody.IsConstraintValid(constraint))
          this.m_world.AddConstraint(constraint);
      }
      this.m_world.UnlockCriticalOperations();
      this.m_enabled = true;
    }

    private void ActivateBatchInternal(object world)
    {
      this.m_world = (HkWorld) world;
      this.IsInWorld = true;
      this.GetRigidBodyMatrix(out this.m_bodyMatrix);
      if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)
      {
        this.RigidBody.SetWorldMatrix(this.m_bodyMatrix);
        this.m_world.AddRigidBodyBatch(this.RigidBody);
      }
      if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
      {
        this.RigidBody2.SetWorldMatrix(this.m_bodyMatrix);
        this.m_world.AddRigidBodyBatch(this.RigidBody2);
      }
      if (this.CharacterProxy != null)
      {
        this.RagdollSystemGroupCollisionFilterID = 0;
        this.CharacterSystemGroupCollisionFilterID = this.m_world.GetCollisionFilter().GetNewSystemGroup();
        this.CharacterCollisionFilter = HkGroupFilter.CalcFilterInfo(18, this.CharacterSystemGroupCollisionFilterID, 1, 1);
        this.CharacterProxy.SetCollisionFilterInfo(this.CharacterCollisionFilter);
        this.CharacterProxy.SetRigidBodyTransform(ref this.m_bodyMatrix);
        this.CharacterProxy.Activate(this.m_world);
      }
      if (this.SwitchToRagdollModeOnActivate)
      {
        int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
        this.SwitchToRagdollModeOnActivate = false;
        this.SwitchToRagdollMode(this.m_ragdollDeadMode);
      }
      foreach (HkConstraint constraint in this.m_constraints)
        this.m_constraintsAddBatch.Add(constraint);
      this.m_enabled = true;
    }

    public virtual void ActivateBatch(object world, ulong clusterObjectID)
    {
      IMyEntity topMostParent = this.Entity.GetTopMostParent();
      if (topMostParent != this.Entity && topMostParent.Physics != null)
        return;
      this.ClusterObjectID = clusterObjectID;
      foreach (MyPhysicsBody batchedBody in this.m_batchedBodies)
        batchedBody.ActivateBatchInternal(world);
      this.m_batchedBodies.Clear();
      this.ActivateBatchInternal(world);
    }

    public void UpdateCluster()
    {
      if (MyPerGameSettings.LimitedWorld || this.Entity == null || this.Entity.Closed)
        return;
      MyPhysics.MoveObject(this.ClusterObjectID, this.Entity.WorldAABB, this.LinearVelocity);
    }

    [Conditional("DEBUG")]
    private void CheckUnlockedSpeeds()
    {
      if (!this.IsPhantom && !this.IsSubpart && this.Entity.Parent == null)
        return;
      int num = (HkReferenceObject) this.RigidBody == (HkReferenceObject) null ? 1 : 0;
    }

    void MyClusterTree.IMyActivationHandler.Activate(
      object userData,
      ulong clusterObjectID)
    {
      this.Activate(userData, clusterObjectID);
    }

    void MyClusterTree.IMyActivationHandler.Deactivate(object userData) => this.Deactivate(userData);

    void MyClusterTree.IMyActivationHandler.ActivateBatch(
      object userData,
      ulong clusterObjectID)
    {
      this.ActivateBatch(userData, clusterObjectID);
    }

    void MyClusterTree.IMyActivationHandler.DeactivateBatch(object userData) => this.DeactivateBatch(userData);

    void MyClusterTree.IMyActivationHandler.FinishAddBatch() => this.FinishAddBatch();

    void MyClusterTree.IMyActivationHandler.FinishRemoveBatch(object userData) => this.FinishRemoveBatch(userData);

    bool MyClusterTree.IMyActivationHandler.IsStaticForCluster => this.IsStaticForCluster;

    public virtual HkShape GetShape()
    {
      if ((HkReferenceObject) this.WeldedRigidBody != (HkReferenceObject) null)
        return this.WeldedRigidBody.GetShape();
      HkShape shape1 = this.RigidBody.GetShape();
      if (shape1.ShapeType == HkShapeType.List)
      {
        HkShapeContainerIterator container = this.RigidBody.GetShape().GetContainer();
        while (container.IsValid)
        {
          HkShape shape2 = container.GetShape(container.CurrentShapeKey);
          if ((long) this.RigidBody.GetGcRoot() == (long) shape2.UserData)
            return shape2;
          container.Next();
        }
      }
      return shape1;
    }

    private static HkMassProperties? GetMassPropertiesFromDefinition(
      MyPhysicsBodyComponentDefinition physicsBodyComponentDefinition,
      MyModelComponentDefinition modelComponentDefinition)
    {
      HkMassProperties? nullable = new HkMassProperties?();
      switch (physicsBodyComponentDefinition.MassPropertiesComputation)
      {
        case MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType.Box:
          nullable = new HkMassProperties?(HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(modelComponentDefinition.Size / 2f, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(modelComponentDefinition.Mass) : modelComponentDefinition.Mass));
          break;
      }
      return nullable;
    }

    private void OnModelChanged(
      MyEntityContainerEventExtensions.EntityEventParams eventParams)
    {
      this.Close();
      this.InitializeRigidBodyFromModel();
    }

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      this.Definition = definition as MyPhysicsBodyComponentDefinition;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (this.Definition == null)
        return;
      this.InitializeRigidBodyFromModel();
      this.RegisterForEntityEvent(MyModelComponent.ModelChanged, new MyEntityContainerEventExtensions.EntityEventHandler(this.OnModelChanged));
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (this.Definition == null)
        return;
      this.Enabled = true;
      if (!this.Definition.ForceActivate)
        return;
      this.ForceActivate();
    }

    private void InitializeRigidBodyFromModel()
    {
      if (this.Definition == null || !((HkReferenceObject) this.RigidBody == (HkReferenceObject) null) || (!this.Definition.CreateFromCollisionObject || !this.Container.Has<MyModelComponent>()))
        return;
      MyModelComponent myModelComponent = this.Container.Get<MyModelComponent>();
      if (myModelComponent.Definition == null || myModelComponent.ModelCollision == null || myModelComponent.ModelCollision.HavokCollisionShapes.Length < 1)
        return;
      HkMassProperties? propertiesFromDefinition = MyPhysicsBody.GetMassPropertiesFromDefinition(this.Definition, myModelComponent.Definition);
      int collisionFilter = this.Definition.CollisionLayer != null ? MyPhysics.GetCollisionLayer(this.Definition.CollisionLayer) : 15;
      this.CreateFromCollisionObject(myModelComponent.ModelCollision.HavokCollisionShapes[0], Vector3.Zero, this.Entity.WorldMatrix, propertiesFromDefinition, collisionFilter);
    }

    public override void UpdateFromSystem()
    {
      if (this.Definition == null || (this.Definition.UpdateFlags & MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags.Gravity) == (MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags) 0 || (!MyFakes.ENABLE_PLANETS || this.Entity == null) || (this.Entity.PositionComp == null || !this.Enabled || !((HkReferenceObject) this.RigidBody != (HkReferenceObject) null)))
        return;
      this.RigidBody.Gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.Entity.PositionComp.GetPosition());
    }

    public Vector3 LastLinearVelocity => this.m_lastLinearVelocity;

    public Vector3 LastAngularVelocity => this.m_lastAngularVelocity;

    protected void NotifyConstraintsAddedToWorld()
    {
      foreach (HkConstraint notifyConstraint in MyPhysicsBody.m_notifyConstraints)
        notifyConstraint.NotifyAddedToWorld();
      MyPhysicsBody.m_notifyConstraints.Clear();
    }

    protected void NotifyConstraintsRemovedFromWorld()
    {
      MyPhysicsBody.m_notifyConstraints.AssertEmpty<HkConstraint>();
      HkRigidBody rigidBody = this.RigidBody;
      if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
        HkConstraint.GetAttachedConstraints(rigidBody, MyPhysicsBody.m_notifyConstraints);
      foreach (HkConstraint notifyConstraint in MyPhysicsBody.m_notifyConstraints)
        notifyConstraint.NotifyRemovedFromWorld();
    }

    public override HkdBreakableBody BreakableBody
    {
      get => this.m_breakableBody;
      set
      {
        this.m_breakableBody = value;
        this.RigidBody = (HkRigidBody) value;
      }
    }

    public virtual void FracturedBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
    {
      if (!Sync.IsServer)
        return;
      e.GetNewBodies(this.m_tmpLst);
      if (this.m_tmpLst.Count == 0)
        return;
      MyPhysics.RemoveDestructions(this.RigidBody);
      foreach (HkdBreakableBodyInfo bodyInfo in this.m_tmpLst)
      {
        HkdBreakableBody breakableBody = MyFracturedPiecesManager.Static.GetBreakableBody(bodyInfo);
        Matrix rigidBodyMatrix = breakableBody.GetRigidBody().GetRigidBodyMatrix();
        MatrixD worldMatrix = (MatrixD) ref rigidBodyMatrix;
        worldMatrix.Translation = this.ClusterToWorld((Vector3) worldMatrix.Translation);
        if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, (this.Entity as MyFracturedPiece).OriginalBlocks) == null)
          MyFracturedPiecesManager.Static.ReturnToPool(breakableBody);
      }
      this.m_tmpLst.Clear();
      this.BreakableBody.AfterReplaceBody -= new BreakableBodyReplaced(this.FracturedBody_AfterReplaceBody);
      MyFracturedPiecesManager.Static.RemoveFracturePiece(this.Entity as MyFracturedPiece, 0.0f);
    }

    public int RagdollSystemGroupCollisionFilterID { get; private set; }

    public bool IsRagdollModeActive => this.Ragdoll != null && this.Ragdoll.InWorld;

    public HkRagdoll Ragdoll
    {
      get => this.m_ragdoll;
      set
      {
        this.m_ragdoll = value;
        if (this.m_ragdoll == null)
          return;
        this.m_ragdoll.AddedToWorld += new Action<HkRagdoll>(this.OnRagdollAddedToWorld);
      }
    }

    public void CloseRagdoll()
    {
      if (this.Ragdoll == null)
        return;
      if (this.IsRagdollModeActive)
        this.CloseRagdollMode(this.HavokWorld);
      if (this.Ragdoll.InWorld)
        this.HavokWorld.RemoveRagdoll(this.Ragdoll);
      this.Ragdoll.Dispose();
      this.Ragdoll = (HkRagdoll) null;
    }

    public void SwitchToRagdollMode(bool deadMode = true, int firstRagdollSubID = 1)
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode");
      if (this.HavokWorld == null || !this.Enabled)
      {
        this.SwitchToRagdollModeOnActivate = true;
        this.m_ragdollDeadMode = deadMode;
      }
      else
      {
        if (this.IsRagdollModeActive)
          return;
        MatrixD worldMatrix = this.Entity.WorldMatrix;
        worldMatrix.Translation = this.WorldToCluster(worldMatrix.Translation);
        if (this.RagdollSystemGroupCollisionFilterID == 0)
          this.RagdollSystemGroupCollisionFilterID = this.m_world.GetCollisionFilter().GetNewSystemGroup();
        this.Ragdoll.SetToKeyframed();
        this.Ragdoll.GenerateRigidBodiesCollisionFilters(deadMode ? 18 : 31, this.RagdollSystemGroupCollisionFilterID, firstRagdollSubID);
        this.Ragdoll.ResetToRigPose();
        this.Ragdoll.SetWorldMatrix((Matrix) ref worldMatrix, false, false);
        if (deadMode)
        {
          this.Ragdoll.SetToDynamic();
          this.SetRagdollVelocities(leadingBody: this.RigidBody);
        }
        else
          this.SetRagdollVelocities();
        if (this.CharacterProxy != null & deadMode)
        {
          this.CharacterProxy.Deactivate(this.HavokWorld);
          this.CharacterProxy.Dispose();
          this.CharacterProxy = (MyCharacterProxy) null;
        }
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null & deadMode)
        {
          this.RigidBody.Deactivate();
          this.HavokWorld.RemoveRigidBody(this.RigidBody);
          this.RigidBody.Dispose();
          this.RigidBody = (HkRigidBody) null;
        }
        if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null & deadMode)
        {
          this.RigidBody2.Deactivate();
          this.HavokWorld.RemoveRigidBody(this.RigidBody2);
          this.RigidBody2.Dispose();
          this.RigidBody2 = (HkRigidBody) null;
        }
        foreach (HkRigidBody rigidBody in this.Ragdoll.RigidBodies)
        {
          rigidBody.UserObject = (object) this;
          rigidBody.Motion.SetDeactivationClass(deadMode ? HkSolverDeactivation.High : HkSolverDeactivation.Medium);
        }
        this.Ragdoll.OptimizeInertiasOfConstraintTree();
        if (!this.Ragdoll.InWorld)
          this.HavokWorld.AddRagdoll(this.Ragdoll);
        this.Ragdoll.EnableConstraints();
        this.Ragdoll.Activate();
        this.m_ragdollDeadMode = deadMode;
        if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
          return;
        MyLog.Default.WriteLine("MyPhysicsBody.SwitchToRagdollMode - FINISHED");
      }
    }

    private void ActivateRagdoll(Matrix worldMatrix)
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll");
      if (this.Ragdoll == null || this.HavokWorld == null || this.IsRagdollModeActive)
        return;
      foreach (HkEntity rigidBody in this.Ragdoll.RigidBodies)
        rigidBody.UserObject = (object) this;
      this.Ragdoll.SetWorldMatrix(worldMatrix, false, false);
      this.HavokWorld.AddRagdoll(this.Ragdoll);
      if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
        this.DisableRagdollBodiesCollisions();
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyPhysicsBody.ActivateRagdoll - FINISHED");
    }

    private void OnRagdollAddedToWorld(HkRagdoll ragdoll)
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      this.Ragdoll.Activate();
      this.Ragdoll.EnableConstraints();
      HkConstraintStabilizationUtil.StabilizeRagdollInertias(ragdoll, 1f, 0.0f);
    }

    public void CloseRagdollMode() => this.CloseRagdollMode(this.HavokWorld);

    public void CloseRagdollMode(HkWorld world)
    {
      int num1 = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (!this.IsRagdollModeActive || world == null)
        return;
      foreach (HkEntity rigidBody in this.Ragdoll.RigidBodies)
        rigidBody.UserObject = (object) null;
      this.Ragdoll.Deactivate();
      world.RemoveRagdoll(this.Ragdoll);
      int num2 = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
    }

    public void SetRagdollDefaults()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults");
      bool isKeyframed = this.Ragdoll.IsKeyframed;
      this.Ragdoll.SetToDynamic();
      float num1 = (this.Entity as MyCharacter).Definition.Mass;
      if ((double) num1 <= 1.0)
        num1 = 80f;
      float num2 = 0.0f;
      foreach (HkRigidBody rigidBody in this.Ragdoll.RigidBodies)
      {
        Vector4 min;
        Vector4 max;
        rigidBody.GetShape().GetLocalAABB(0.01f, out min, out max);
        float num3 = (max - min).Length();
        num2 += num3;
      }
      if ((double) num2 <= 0.0)
        num2 = 1f;
      foreach (HkRigidBody rigidBody in this.Ragdoll.RigidBodies)
      {
        rigidBody.MaxLinearVelocity = 1000f;
        rigidBody.MaxAngularVelocity = 1000f;
        HkShape shape = rigidBody.GetShape();
        Vector4 min;
        Vector4 max;
        shape.GetLocalAABB(0.01f, out min, out max);
        float num3 = (max - min).Length();
        float m = num1 / num2 * num3;
        rigidBody.Mass = MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(m) : m;
        float convexRadius = shape.ConvexRadius;
        if (shape.ShapeType == HkShapeType.Capsule)
        {
          HkCapsuleShape hkCapsuleShape = (HkCapsuleShape) shape;
          HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(hkCapsuleShape.VertexA, hkCapsuleShape.VertexB, convexRadius, rigidBody.Mass);
          rigidBody.InertiaTensor = volumeMassProperties.InertiaTensor;
        }
        else
        {
          HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(Vector3.One * num3 * 0.5f, rigidBody.Mass);
          rigidBody.InertiaTensor = volumeMassProperties.InertiaTensor;
        }
        rigidBody.AngularDamping = 0.005f;
        rigidBody.LinearDamping = 0.0f;
        rigidBody.Friction = 6f;
        rigidBody.AllowedPenetrationDepth = 0.1f;
        rigidBody.Restitution = 0.05f;
      }
      this.Ragdoll.OptimizeInertiasOfConstraintTree();
      if (isKeyframed)
        this.Ragdoll.SetToKeyframed();
      foreach (HkConstraint constraint in this.Ragdoll.Constraints)
      {
        if (constraint.ConstraintData is HkRagdollConstraintData)
          (constraint.ConstraintData as HkRagdollConstraintData).MaxFrictionTorque = MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f;
        else if (constraint.ConstraintData is HkFixedConstraintData)
        {
          HkFixedConstraintData constraintData = constraint.ConstraintData as HkFixedConstraintData;
          constraintData.MaximumLinearImpulse = 3.40282E+28f;
          constraintData.MaximumAngularImpulse = 3.40282E+28f;
        }
        else if (constraint.ConstraintData is HkHingeConstraintData)
        {
          HkHingeConstraintData constraintData = constraint.ConstraintData as HkHingeConstraintData;
          constraintData.MaximumAngularImpulse = 3.40282E+28f;
          constraintData.MaximumLinearImpulse = 3.40282E+28f;
        }
        else if (constraint.ConstraintData is HkLimitedHingeConstraintData)
          (constraint.ConstraintData as HkLimitedHingeConstraintData).MaxFrictionTorque = MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(0.5f) : 3f;
      }
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyPhysicsBody.SetRagdollDefaults FINISHED");
    }

    public bool ReactivateRagdoll { get; set; }

    public bool SwitchToRagdollModeOnActivate { get; set; }

    internal void DisableRagdollBodiesCollisions()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
      {
        HkWorld havokWorld = this.HavokWorld;
      }
      if (this.Ragdoll == null)
        return;
      foreach (HkRigidBody rigidBody in this.Ragdoll.RigidBodies)
      {
        rigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(31, 0, 0, 0));
        rigidBody.LinearVelocity = Vector3.Zero;
        rigidBody.AngularVelocity = Vector3.Zero;
        MyPhysics.RefreshCollisionFilter(this);
      }
    }

    private void ApplyForceTorqueOnRagdoll(
      Vector3? force,
      Vector3? torque,
      HkRagdoll ragdoll,
      ref Matrix transform)
    {
      foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
      {
        if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
        {
          Vector3 vector3 = force.Value * rigidBody.Mass / ragdoll.Mass;
          transform = rigidBody.GetRigidBodyMatrix();
          this.AddForceTorqueBody(new Vector3?(vector3), torque, new Vector3?(), rigidBody, ref transform);
        }
      }
    }

    private void ApplyImpuseOnRagdoll(
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      HkRagdoll ragdoll)
    {
      foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
        this.ApplyImplusesWorld(new Vector3?(force.Value * rigidBody.Mass / ragdoll.Mass), position, torque, rigidBody);
    }

    private void ApplyForceOnRagdoll(Vector3? force, Vector3D? position, HkRagdoll ragdoll)
    {
      foreach (HkRigidBody rigidBody in ragdoll.RigidBodies)
        this.ApplyForceWorld(new Vector3?(force.Value * rigidBody.Mass / ragdoll.Mass), position, rigidBody);
    }

    public void SetRagdollVelocities(List<int> bodiesToUpdate = null, HkRigidBody leadingBody = null)
    {
      List<HkRigidBody> rigidBodies = this.Ragdoll.RigidBodies;
      if ((HkReferenceObject) leadingBody == (HkReferenceObject) null && this.CharacterProxy != null)
      {
        HkRigidBody hitRigidBody = this.CharacterProxy.GetHitRigidBody();
        if ((HkReferenceObject) hitRigidBody != (HkReferenceObject) null)
          leadingBody = hitRigidBody;
      }
      if ((HkReferenceObject) leadingBody == (HkReferenceObject) null)
        leadingBody = rigidBodies[0];
      Vector3 angularVelocity = leadingBody.AngularVelocity;
      if (bodiesToUpdate != null)
      {
        foreach (int index in bodiesToUpdate)
        {
          HkRigidBody hkRigidBody = rigidBodies[index];
          hkRigidBody.AngularVelocity = angularVelocity;
          hkRigidBody.LinearVelocity = leadingBody.GetVelocityAtPoint(hkRigidBody.Position);
        }
      }
      else
      {
        foreach (HkRigidBody hkRigidBody in rigidBodies)
        {
          hkRigidBody.AngularVelocity = angularVelocity;
          hkRigidBody.LinearVelocity = leadingBody.GetVelocityAtPoint(hkRigidBody.Position);
        }
      }
    }

    public bool IsWelded => this.WeldInfo.Parent != null;

    [Obsolete]
    public MyPhysicsBody.MyWeldInfo WeldInfo => this.m_weldInfo;

    public void Weld(MyPhysicsComponentBase other, bool recreateShape = true) => this.Weld(other as MyPhysicsBody, recreateShape);

    public void Weld(MyPhysicsBody other, bool recreateShape = true)
    {
      if (other.WeldInfo.Parent == this)
        return;
      if (other.IsWelded && !this.IsWelded)
        other.Weld(this, true);
      else if (this.IsWelded)
      {
        this.WeldInfo.Parent.Weld(other, true);
      }
      else
      {
        if (other.WeldInfo.Children.Count > 0)
          other.UnweldAll(false);
        if (this.WeldInfo.Children.Count == 0)
        {
          this.WeldedRigidBody = this.RigidBody;
          HkShape shape = this.RigidBody.GetShape();
          if (this.HavokWorld != null)
            this.HavokWorld.RemoveRigidBody(this.WeldedRigidBody);
          this.RigidBody = HkRigidBody.Clone(this.WeldedRigidBody);
          if (this.HavokWorld != null)
            this.HavokWorld.AddRigidBody(this.RigidBody);
          HkRigidBody rigidBody = this.RigidBody;
          HkShape.SetUserData(shape, rigidBody);
          this.Entity.OnPhysicsChanged += new Action<IMyEntity>(this.WeldedEntity_OnPhysicsChanged);
          this.WeldInfo.UpdateMassProps(this.RigidBody);
        }
        else
          this.GetShape();
        other.Deactivate();
        MatrixD matrixD = other.Entity.WorldMatrix * this.Entity.WorldMatrixInvScaled;
        other.WeldInfo.Transform = (Matrix) ref matrixD;
        other.WeldInfo.UpdateMassProps(other.RigidBody);
        other.WeldedRigidBody = other.RigidBody;
        other.RigidBody = this.RigidBody;
        other.WeldInfo.Parent = this;
        other.ClusterObjectID = this.ClusterObjectID;
        this.WeldInfo.Children.Add(other);
        this.OnWelded(other);
        other.OnWelded(this);
      }
    }

    private void Entity_OnClose(IMyEntity obj) => this.UnweldAll(true);

    private void WeldedEntity_OnPhysicsChanged(IMyEntity obj)
    {
      if (this.Entity == null || this.Entity.Physics == null)
        return;
      foreach (MyPhysicsBody child in this.WeldInfo.Children)
      {
        if (child.Entity == null)
        {
          child.WeldInfo.Parent = (MyPhysicsBody) null;
          this.WeldInfo.Children.Remove(child);
          if (obj.Physics != null)
          {
            this.Weld(obj.Physics as MyPhysicsBody, true);
            break;
          }
          break;
        }
      }
      this.RecreateWeldedShape(this.GetShape());
    }

    public void RecreateWeldedShape()
    {
      if (this.WeldInfo.Children.Count == 0)
        return;
      this.RecreateWeldedShape(this.GetShape());
    }

    public void UpdateMassProps()
    {
      if (this.RigidBody.IsFixedOrKeyframed)
        return;
      if (this.WeldInfo.Parent != null)
      {
        this.WeldInfo.Parent.UpdateMassProps();
      }
      else
      {
        int num1 = 1 + this.WeldInfo.Children.Count;
        HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num1);
        array[0] = this.WeldInfo.MassElement;
        int num2 = 1;
        foreach (MyPhysicsBody child in this.WeldInfo.Children)
          array[num2++] = child.WeldInfo.MassElement;
        HkMassProperties massProperties;
        HkInertiaTensorComputer.CombineMassProperties(new Span<HkMassElement>(array, 0, num1), out massProperties);
        this.RigidBody.SetMassProperties(ref massProperties);
        ArrayPool<HkMassElement>.Shared.Return(array);
      }
    }

    private void RecreateWeldedShape(HkShape thisShape)
    {
      if ((HkReferenceObject) this.RigidBody == (HkReferenceObject) null || this.RigidBody.IsDisposed)
        return;
      if (this.WeldInfo.Children.Count == 0)
      {
        int num1 = (int) this.RigidBody.SetShape(thisShape);
        if (!((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null))
          return;
        int num2 = (int) this.RigidBody2.SetShape(thisShape);
      }
      else
      {
        this.m_tmpShapeList.Add(thisShape);
        foreach (MyPhysicsBody child in this.WeldInfo.Children)
        {
          HkTransformShape hkTransformShape = new HkTransformShape(child.WeldedRigidBody.GetShape(), ref child.WeldInfo.Transform);
          HkShape.SetUserData((HkShape) hkTransformShape, child.WeldedRigidBody);
          this.m_tmpShapeList.Add((HkShape) hkTransformShape);
          if (this.m_tmpShapeList.Count == 128)
            break;
        }
        HkSmartListShape hkSmartListShape = new HkSmartListShape(0);
        foreach (HkShape tmpShape in this.m_tmpShapeList)
          hkSmartListShape.AddShape(tmpShape);
        int num1 = (int) this.RigidBody.SetShape((HkShape) hkSmartListShape);
        if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
        {
          int num2 = (int) this.RigidBody2.SetShape((HkShape) hkSmartListShape);
        }
        hkSmartListShape.Base.RemoveReference();
        this.WeldedMarkBreakable();
        for (int index = 1; index < this.m_tmpShapeList.Count; ++index)
          this.m_tmpShapeList[index].RemoveReference();
        this.m_tmpShapeList.Clear();
        this.UpdateMassProps();
      }
    }

    private void WeldedMarkBreakable()
    {
      if (this.HavokWorld == null)
        return;
      if (this is MyGridPhysics myGridPhysics && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
        this.HavokWorld.BreakOffPartsUtil.MarkPieceBreakable((HkEntity) this.RigidBody, 0U, myGridPhysics.Shape.BreakImpulse);
      uint shapeKey = 1;
      foreach (MyPhysicsBody child in this.WeldInfo.Children)
      {
        if (child is MyGridPhysics myGridPhysics && (myGridPhysics.Entity as MyCubeGrid).BlocksDestructionEnabled)
          this.HavokWorld.BreakOffPartsUtil.MarkPieceBreakable((HkEntity) this.RigidBody, shapeKey, myGridPhysics.Shape.BreakImpulse);
        ++shapeKey;
      }
    }

    public void UnweldAll(bool insertInWorld)
    {
      while (this.WeldInfo.Children.Count > 1)
        this.Unweld(this.WeldInfo.Children.First<MyPhysicsBody>(), insertInWorld, false);
      if (this.WeldInfo.Children.Count <= 0)
        return;
      this.Unweld(this.WeldInfo.Children.First<MyPhysicsBody>(), insertInWorld);
    }

    public void Unweld(MyPhysicsBody other, bool insertToWorld = true, bool recreateShape = true)
    {
      if (this.IsWelded)
        this.WeldInfo.Parent.Unweld(other, insertToWorld, recreateShape);
      else if (other.IsInWorld || (HkReferenceObject) this.RigidBody == (HkReferenceObject) null || (HkReferenceObject) other.WeldedRigidBody == (HkReferenceObject) null)
      {
        this.WeldInfo.Children.Remove(other);
      }
      else
      {
        Matrix rigidBodyMatrix = this.RigidBody.GetRigidBodyMatrix();
        other.WeldInfo.Parent = (MyPhysicsBody) null;
        this.WeldInfo.Children.Remove(other);
        HkRigidBody rigidBody = other.RigidBody;
        other.RigidBody = other.WeldedRigidBody;
        other.WeldedRigidBody = (HkRigidBody) null;
        if (!other.RigidBody.IsDisposed)
        {
          other.RigidBody.SetWorldMatrix(other.WeldInfo.Transform * rigidBodyMatrix);
          other.RigidBody.LinearVelocity = rigidBody.LinearVelocity;
          other.WeldInfo.MassElement.Tranform = Matrix.Identity;
          other.WeldInfo.Transform = Matrix.Identity;
          int num = (HkReferenceObject) other.RigidBody2 != (HkReferenceObject) null ? 1 : 0;
        }
        other.ClusterObjectID = ulong.MaxValue;
        if (insertToWorld)
          other.Activate();
        if (this.WeldInfo.Children.Count == 0)
        {
          recreateShape = false;
          this.Entity.OnPhysicsChanged -= new Action<IMyEntity>(this.WeldedEntity_OnPhysicsChanged);
          this.Entity.OnClose -= new Action<IMyEntity>(this.Entity_OnClose);
          this.WeldedRigidBody.LinearVelocity = this.RigidBody.LinearVelocity;
          this.WeldedRigidBody.AngularVelocity = this.RigidBody.AngularVelocity;
          if (this.HavokWorld != null)
            this.HavokWorld.RemoveRigidBody(this.RigidBody);
          this.RigidBody.Dispose();
          this.RigidBody = this.WeldedRigidBody;
          this.WeldedRigidBody = (HkRigidBody) null;
          this.RigidBody.SetWorldMatrix(rigidBodyMatrix);
          this.WeldInfo.Transform = Matrix.Identity;
          if (this.HavokWorld != null)
          {
            this.HavokWorld.AddRigidBody(this.RigidBody);
            this.ActivateCollision();
          }
          else if (!this.Entity.MarkedForClose)
            this.Activate();
          if ((HkReferenceObject) this.RigidBody2 != (HkReferenceObject) null)
          {
            int num = (int) this.RigidBody2.SetShape(this.RigidBody.GetShape());
          }
        }
        if ((HkReferenceObject) this.RigidBody != (HkReferenceObject) null & recreateShape)
          this.RecreateWeldedShape(this.GetShape());
        this.OnUnwelded(other);
        other.OnUnwelded(this);
      }
    }

    public void Unweld(bool insertInWorld = true) => this.WeldInfo.Parent.Unweld(this, insertInWorld);

    public HkRigidBody WeldedRigidBody { get; protected set; }

    protected virtual void OnWelded(MyPhysicsBody other)
    {
    }

    protected virtual void OnUnwelded(MyPhysicsBody other)
    {
    }

    private void RemoveConstraints(HkRigidBody hkRigidBody)
    {
      foreach (HkConstraint constraint in this.m_constraints)
      {
        if (constraint.IsDisposed || (HkReferenceObject) constraint.RigidBodyA == (HkReferenceObject) hkRigidBody || (HkReferenceObject) constraint.RigidBodyB == (HkReferenceObject) hkRigidBody)
          this.m_constraintsRemoveBatch.Add(constraint);
      }
      foreach (HkConstraint constraint in this.m_constraintsRemoveBatch)
      {
        this.m_constraints.Remove(constraint);
        if (!constraint.IsDisposed && constraint.InWorld)
          this.HavokWorld.RemoveConstraint(constraint);
      }
      this.m_constraintsRemoveBatch.Clear();
    }

    public delegate void PhysicsContactHandler(ref MyPhysics.MyContactPointEvent e);

    public class MyWeldInfo
    {
      public MyPhysicsBody Parent;
      public Matrix Transform = Matrix.Identity;
      public readonly HashSet<MyPhysicsBody> Children = new HashSet<MyPhysicsBody>();
      public HkMassElement MassElement;

      internal void UpdateMassProps(HkRigidBody rb)
      {
        HkMassProperties hkMassProperties = new HkMassProperties();
        hkMassProperties.InertiaTensor = rb.InertiaTensor;
        hkMassProperties.Mass = rb.Mass;
        hkMassProperties.CenterOfMass = rb.CenterOfMassLocal;
        this.MassElement = new HkMassElement();
        this.MassElement.Properties = hkMassProperties;
        this.MassElement.Tranform = this.Transform;
      }

      internal void SetMassProps(HkMassProperties mp)
      {
        this.MassElement = new HkMassElement();
        this.MassElement.Properties = mp;
        this.MassElement.Tranform = this.Transform;
      }
    }

    private class Sandbox_Engine_Physics_MyPhysicsBody\u003C\u003EActor : IActivator, IActivator<MyPhysicsBody>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicsBody();

      MyPhysicsBody IActivator<MyPhysicsBody>.CreateInstance() => new MyPhysicsBody();
    }
  }
}
