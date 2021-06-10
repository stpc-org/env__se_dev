// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyPhysicsComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using Havok;
using System;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.SessionComponents;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Components
{
  [MyComponentType(typeof (MyPhysicsComponentBase))]
  public abstract class MyPhysicsComponentBase : MyEntityComponentBase
  {
    protected Vector3 m_lastLinearVelocity;
    protected Vector3 m_lastAngularVelocity;
    private Vector3 m_linearVelocity;
    private Vector3 m_angularVelocity;
    private Vector3 m_supportNormal;
    public ushort ContactPointDelay = ushort.MaxValue;
    public Action EnabledChanged;
    public RigidBodyFlag Flags;
    public bool IsPhantom;
    protected bool m_enabled;

    public bool ReportAllContacts
    {
      get => this.ContactPointDelay == (ushort) 0;
      set => this.ContactPointDelay = value ? (ushort) 0 : ushort.MaxValue;
    }

    public new IMyEntity Entity { get; protected set; }

    public bool CanUpdateAccelerations { get; set; }

    public MyStringHash MaterialType { get; set; }

    public virtual MyStringHash GetMaterialAt(Vector3D worldPos) => this.MaterialType;

    public virtual bool IsStatic => (this.Flags & RigidBodyFlag.RBF_STATIC) == RigidBodyFlag.RBF_STATIC;

    public virtual bool IsKinematic => (this.Flags & RigidBodyFlag.RBF_KINEMATIC) == RigidBodyFlag.RBF_KINEMATIC || (this.Flags & RigidBodyFlag.RBF_DOUBLED_KINEMATIC) == RigidBodyFlag.RBF_DOUBLED_KINEMATIC;

    public virtual bool Enabled
    {
      get => this.m_enabled;
      set
      {
        if (this.m_enabled == value)
          return;
        this.m_enabled = value;
        if (this.EnabledChanged != null)
          this.EnabledChanged();
        if (value)
        {
          if (!this.Entity.InScene)
            return;
          this.Activate();
        }
        else
          this.Deactivate();
      }
    }

    public bool PlayCollisionCueEnabled { get; set; }

    public abstract float Mass { get; }

    public Vector3 Center { get; set; }

    public virtual Vector3 LinearVelocity
    {
      get => this.m_linearVelocity;
      set => this.m_linearVelocity = value;
    }

    public virtual Vector3 LinearVelocityUnsafe
    {
      get => this.m_linearVelocity;
      set => this.m_linearVelocity = value;
    }

    public virtual Vector3 LinearVelocityLocal
    {
      get => this.m_linearVelocity;
      set => this.m_linearVelocity = value;
    }

    public virtual Vector3 AngularVelocity
    {
      get => this.m_angularVelocity;
      set => this.m_angularVelocity = value;
    }

    public virtual Vector3 AngularVelocityLocal
    {
      get => this.m_angularVelocity;
      set => this.m_angularVelocity = value;
    }

    public virtual Vector3 SupportNormal
    {
      get => this.m_supportNormal;
      set => this.m_supportNormal = value;
    }

    public virtual Vector3 LinearAcceleration { get; protected set; }

    public virtual Vector3 AngularAcceleration { get; protected set; }

    public abstract Vector3 GetVelocityAtPoint(Vector3D worldPos);

    public abstract void GetVelocityAtPointLocal(ref Vector3D worldPos, out Vector3 linearVelocity);

    public abstract float LinearDamping { get; set; }

    public abstract float AngularDamping { get; set; }

    public abstract float Speed { get; }

    public abstract float Friction { get; set; }

    public abstract HkRigidBody RigidBody { get; protected set; }

    public abstract HkRigidBody RigidBody2 { get; protected set; }

    public abstract HkdBreakableBody BreakableBody { get; set; }

    public abstract bool IsMoving { get; }

    public abstract Vector3 Gravity { get; set; }

    public MyPhysicsComponentDefinitionBase Definition { get; private set; }

    public abstract bool IsActive { get; }

    public abstract event Action<MyPhysicsComponentBase, bool> OnBodyActiveStateChanged;

    public virtual void Close()
    {
      this.Deactivate();
      this.CloseRigidBody();
    }

    protected abstract void CloseRigidBody();

    public abstract void AddForce(
      MyPhysicsForceType type,
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      float? maxSpeed = null,
      bool applyImmediately = true,
      bool activeOnly = false);

    public abstract void ApplyImpulse(Vector3 dir, Vector3D pos);

    public abstract void ClearSpeed();

    public abstract void Clear();

    public abstract void CreateCharacterCollision(
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
      float maxLimit,
      float maxSpeedRelativeToShip,
      bool networkProxy,
      float? maxForce);

    public abstract void DebugDraw();

    public abstract void Activate();

    public abstract void Deactivate();

    public abstract void ForceActivate();

    public void UpdateAccelerations()
    {
      Vector3 linearVelocity = this.LinearVelocity;
      this.LinearAcceleration = (linearVelocity - this.m_lastLinearVelocity) * 60f;
      this.m_lastLinearVelocity = linearVelocity;
      Vector3 angularVelocity = this.AngularVelocity;
      this.AngularAcceleration = (angularVelocity - this.m_lastAngularVelocity) * 60f;
      this.m_lastAngularVelocity = angularVelocity;
    }

    public void SetSpeeds(Vector3 linear, Vector3 angular)
    {
      this.LinearVelocity = linear;
      this.AngularVelocity = angular;
      this.ClearAccelerations();
      this.SetActualSpeedsAsPrevious();
    }

    private void ClearAccelerations()
    {
      this.LinearAcceleration = Vector3.Zero;
      this.AngularAcceleration = Vector3.Zero;
    }

    private void SetActualSpeedsAsPrevious()
    {
      this.m_lastLinearVelocity = this.LinearVelocity;
      this.m_lastAngularVelocity = this.AngularVelocity;
    }

    public abstract Vector3D WorldToCluster(Vector3D worldPos);

    public abstract Vector3D ClusterToWorld(Vector3 clusterPos);

    public MatrixD GetWorldMatrix()
    {
      MatrixD matrix;
      this.GetWorldMatrix(out matrix);
      return matrix;
    }

    public abstract void GetWorldMatrix(out MatrixD matrix);

    public abstract bool HasRigidBody { get; }

    public abstract Vector3 CenterOfMassLocal { get; }

    public abstract Vector3D CenterOfMassWorld { get; }

    public abstract void UpdateFromSystem();

    public abstract void OnWorldPositionChanged(object source);

    public virtual bool IsInWorld { get; protected set; }

    public virtual bool ShapeChangeInProgress { get; set; }

    public override string ComponentTypeDebugString => "Physics";

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      this.Definition = definition as MyPhysicsComponentDefinitionBase;
      if (this.Definition == null)
        return;
      this.Flags = this.Definition.RigidBodyFlags;
      if (this.Definition.LinearDamping.HasValue)
        this.LinearDamping = this.Definition.LinearDamping.Value;
      if (!this.Definition.AngularDamping.HasValue)
        return;
      this.AngularDamping = this.Definition.AngularDamping.Value;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Entity = this.Container.Entity;
      if (this.Definition == null || this.Definition.UpdateFlags == (MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags) 0)
        return;
      MyPhysicsComponentSystem.Static.Register(this);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (this.Definition == null || this.Definition.UpdateFlags == (MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags) 0 || MyPhysicsComponentSystem.Static == null)
        return;
      MyPhysicsComponentSystem.Static.Unregister(this);
    }

    public override bool IsSerialized() => this.Definition != null && this.Definition.Serialize;

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_PhysicsComponentBase objectBuilder = MyComponentFactory.CreateObjectBuilder((MyComponentBase) this) as MyObjectBuilder_PhysicsComponentBase;
      objectBuilder.LinearVelocity = (SerializableVector3) this.LinearVelocity;
      objectBuilder.AngularVelocity = (SerializableVector3) this.AngularVelocity;
      return (MyObjectBuilder_ComponentBase) objectBuilder;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
    {
      MyObjectBuilder_PhysicsComponentBase physicsComponentBase = baseBuilder as MyObjectBuilder_PhysicsComponentBase;
      this.LinearVelocity = (Vector3) physicsComponentBase.LinearVelocity;
      this.AngularVelocity = (Vector3) physicsComponentBase.AngularVelocity;
    }
  }
}
