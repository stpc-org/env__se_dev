// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Door))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyDoor), typeof (Sandbox.ModAPI.Ingame.IMyDoor)})]
  public class MyDoor : MyDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor
  {
    private const float CLOSED_DISSASEMBLE_RATIO = 3.3f;
    private static readonly float EPSILON = 1E-09f;
    private static readonly float IMPULSE_THRESHOLD = 1f;
    private MySoundPair m_openSound;
    private MySoundPair m_closeSound;
    private float m_currOpening;
    private float m_currSpeed;
    private float m_openingSpeed;
    private int m_lastUpdateTime;
    private bool m_physicsInitiated;
    private MyEntitySubpart m_leftSubpart;
    private MyEntitySubpart m_rightSubpart;
    private HkFixedConstraintData m_leftConstraintData;
    private HkConstraint m_leftConstraint;
    private HkFixedConstraintData m_rightConstraintData;
    private HkConstraint m_rightConstraint;
    public float MaxOpen = 1.2f;

    public event Action<bool> DoorStateChanged;

    public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override float DisassembleRatio => base.DisassembleRatio * 3.3f;

    public MyDoor()
    {
      this.m_currOpening = 0.0f;
      this.m_currSpeed = 0.0f;
      this.m_open.AlwaysReject<bool, SyncDirection.BothWays>();
      this.m_open.ValueChanged += (Action<SyncBase>) (x => this.OnStateChange());
    }

    DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status => (bool) this.m_open ? ((double) this.MaxOpen - (double) this.m_currOpening >= (double) MyDoor.EPSILON ? DoorStatus.Opening : DoorStatus.Open) : ((double) this.m_currOpening >= (double) MyDoor.EPSILON ? DoorStatus.Closing : DoorStatus.Closed);

    public float OpenRatio => this.m_currOpening / this.MaxOpen;

    void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Opening:
        case DoorStatus.Open:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Closing:
        case DoorStatus.Closed:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
    {
      if (!this.IsWorking)
        return;
      this.SetOpenRequest(!this.Open, this.OwnerId);
    }

    bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => (double) this.m_currOpening < (double) MyDoor.EPSILON;

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      this.m_physicsInitiated = false;
      MyStringHash orCompute;
      if (this.BlockDefinition is MyDoorDefinition blockDefinition)
      {
        this.MaxOpen = blockDefinition.MaxOpen;
        this.m_openSound = new MySoundPair(blockDefinition.OpenSound);
        this.m_closeSound = new MySoundPair(blockDefinition.CloseSound);
        orCompute = MyStringHash.GetOrCompute(blockDefinition.ResourceSinkGroup);
        this.m_openingSpeed = blockDefinition.OpeningSpeed;
      }
      else
      {
        this.MaxOpen = 1.2f;
        this.m_openSound = new MySoundPair("BlockDoorSmallOpen");
        this.m_closeSound = new MySoundPair("BlockDoorSmallClose");
        orCompute = MyStringHash.GetOrCompute("Doors");
        this.m_openingSpeed = 1f;
      }
      MyResourceSinkComponent sinkComp = new MyResourceSinkComponent();
      sinkComp.Init(orCompute, 3E-05f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : sinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      sinkComp.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = sinkComp;
      base.Init(builder, cubeGrid);
      this.NeedsWorldMatrix = false;
      MyObjectBuilder_Door objectBuilderDoor = (MyObjectBuilder_Door) builder;
      this.m_open.SetLocalValue(objectBuilderDoor.State);
      if ((double) objectBuilderDoor.Opening == -1.0)
      {
        this.m_currOpening = this.IsFunctional ? 0.0f : this.MaxOpen;
        this.m_open.SetLocalValue(!this.IsFunctional);
      }
      else
        this.m_currOpening = MathHelper.Clamp(objectBuilderDoor.Opening, 0.0f, this.MaxOpen);
      if (!this.Enabled || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        this.UpdateSlidingDoorsPosition();
      this.OnStateChange();
      if ((bool) this.m_open && this.Open && (double) this.m_currOpening == (double) this.MaxOpen)
        this.UpdateSlidingDoorsPosition();
      sinkComp.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsWorldMatrix = true;
    }

    private void InitSubparts()
    {
      this.DisposeSubpartConstraint(ref this.m_leftConstraint, ref this.m_leftConstraintData);
      this.DisposeSubpartConstraint(ref this.m_rightConstraint, ref this.m_rightConstraintData);
      this.Subparts.TryGetValue("DoorLeft", out this.m_leftSubpart);
      this.Subparts.TryGetValue("DoorRight", out this.m_rightSubpart);
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      if (this.m_leftSubpart != null)
      {
        this.m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
        this.m_leftSubpart.NeedsWorldMatrix = false;
        this.m_leftSubpart.InvalidateOnMove = false;
      }
      if (this.m_rightSubpart != null)
      {
        this.m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
        this.m_rightSubpart.NeedsWorldMatrix = false;
        this.m_rightSubpart.InvalidateOnMove = false;
      }
      if (this.CubeGrid.Projector != null)
        this.UpdateSlidingDoorsPosition();
      else if (!this.CubeGrid.CreatePhysics)
      {
        this.UpdateSlidingDoorsPosition();
      }
      else
      {
        if (this.m_leftSubpart != null && this.m_leftSubpart.Physics != null)
        {
          this.m_leftSubpart.Physics.Close();
          this.m_leftSubpart.Physics = (MyPhysicsComponentBase) null;
        }
        if (this.m_rightSubpart != null && this.m_rightSubpart.Physics != null)
        {
          this.m_rightSubpart.Physics.Close();
          this.m_rightSubpart.Physics = (MyPhysicsComponentBase) null;
        }
        this.CreateConstraints();
        this.m_physicsInitiated = true;
        this.UpdateSlidingDoorsPosition();
      }
    }

    private void CreateConstraints()
    {
      this.UpdateSlidingDoorsPosition();
      this.CreateConstraint(this.m_leftSubpart, ref this.m_leftConstraint, ref this.m_leftConstraintData);
      this.CreateConstraint(this.m_rightSubpart, ref this.m_rightConstraint, ref this.m_rightConstraintData);
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
    }

    private void CreateConstraint(
      MyEntitySubpart subpart,
      ref HkConstraint constraint,
      ref HkFixedConstraintData constraintData)
    {
      if (subpart == null)
        return;
      bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer;
      if (subpart.Physics == null)
      {
        HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
        if (havokCollisionShapes != null && havokCollisionShapes.Length != 0)
        {
          MyPhysicsBody myPhysicsBody = new MyPhysicsBody((VRage.ModAPI.IMyEntity) subpart, flag ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DOUBLED_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
          myPhysicsBody.IsSubpart = true;
          subpart.Physics = (MyPhysicsComponentBase) myPhysicsBody;
          HkShape shape = havokCollisionShapes[0];
          MyPositionComponentBase positionComp = subpart.PositionComp;
          Vector3 zero = Vector3.Zero;
          HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(positionComp.LocalAABB.HalfExtents, 100f);
          int collisionFilter = this.CubeGrid.IsStatic ? 9 : 16;
          myPhysicsBody.CreateFromCollisionObject(shape, zero, positionComp.WorldMatrixRef, new HkMassProperties?(volumeMassProperties), collisionFilter);
        }
      }
      if (!flag)
      {
        this.CreateSubpartConstraint((MyEntity) subpart, out constraintData, out constraint);
        this.CubeGrid.Physics.AddConstraint(constraint);
        constraint.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
      }
      else
        subpart.Physics.Enabled = true;
    }

    public override void OnAddedToScene(object source)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      base.OnAddedToScene(source);
    }

    public override void OnRemovedFromScene(object source)
    {
      this.DisposeSubpartConstraint(ref this.m_leftConstraint, ref this.m_leftConstraintData);
      this.DisposeSubpartConstraint(ref this.m_rightConstraint, ref this.m_rightConstraintData);
      base.OnRemovedFromScene(source);
    }

    protected override void BeforeDelete()
    {
      this.DisposeSubpartConstraint(ref this.m_leftConstraint, ref this.m_leftConstraintData);
      this.DisposeSubpartConstraint(ref this.m_rightConstraint, ref this.m_rightConstraintData);
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      base.BeforeDelete();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Door builderCubeBlock = (MyObjectBuilder_Door) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.State = this.Open;
      builderCubeBlock.Opening = this.m_currOpening;
      builderCubeBlock.OpenSound = this.m_openSound.ToString();
      builderCubeBlock.CloseSound = this.m_closeSound.ToString();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      oldGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.CubeGrid.Physics != null)
        this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, true);
      if (this.InScene)
      {
        MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
        if (this.m_leftSubpart != null)
          this.m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
        if (this.m_rightSubpart != null)
          this.m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
      }
      base.OnCubeGridChanged(oldGrid);
    }

    private void OnStateChange()
    {
      if (this.m_leftSubpart == null && this.m_rightSubpart == null)
        return;
      this.m_currSpeed = (bool) this.m_open ? this.m_openingSpeed : -this.m_openingSpeed;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.UpdateCurrentOpening();
      this.UpdateSlidingDoorsPosition();
      if (!(bool) this.m_open)
        return;
      this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
      this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
    }

    private void StartSound(MySoundPair cuePair)
    {
      if (this.m_soundEmitter == null || this.m_soundEmitter.Sound != null && this.m_soundEmitter.Sound.IsPlaying && (this.m_soundEmitter.SoundId == cuePair.Arcade || this.m_soundEmitter.SoundId == cuePair.Realistic))
        return;
      this.m_soundEmitter.StopSound(true);
      this.m_soundEmitter.PlaySingleSound(cuePair, true);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.Physics == null || ((double) this.m_currOpening == 0.0 || (double) this.m_currOpening > (double) this.MaxOpen) && (double) this.m_currSpeed == 0.0)
        return;
      this.UpdateSlidingDoorsPosition();
    }

    public override void UpdateBeforeSimulation()
    {
      if (this.Open && (double) this.m_currOpening == (double) this.MaxOpen || !this.Open && (double) this.m_currOpening == 0.0)
      {
        if (this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying && this.m_soundEmitter.Loop && (this.BlockDefinition.DamagedSound == null || this.m_soundEmitter.SoundId != this.BlockDefinition.DamagedSound.SoundId))
          this.m_soundEmitter.StopSound(false);
        if (this.m_physicsInitiated && !this.HasDamageEffect)
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
        this.m_currSpeed = 0.0f;
        if ((bool) this.m_open)
          return;
        this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
        this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
      }
      else
      {
        if (this.m_soundEmitter != null && this.Enabled && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        {
          if (this.Open)
            this.StartSound(this.m_openSound);
          else
            this.StartSound(this.m_closeSound);
        }
        base.UpdateBeforeSimulation();
        this.UpdateCurrentOpening();
        this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        if (!MyFakes.ENABLE_DOOR_SAFETY_2 || !this.IsClosing())
          return;
        float num = -1f;
        if ((HkReferenceObject) this.m_leftConstraint != (HkReferenceObject) null)
        {
          float impulseInLastStep = HkFixedConstraintData.GetSolverImpulseInLastStep(this.m_leftConstraint, (byte) 0);
          if ((double) impulseInLastStep > (double) num)
            num = impulseInLastStep;
        }
        if ((HkReferenceObject) this.m_rightConstraint != (HkReferenceObject) null)
        {
          float impulseInLastStep = HkFixedConstraintData.GetSolverImpulseInLastStep(this.m_rightConstraint, (byte) 0);
          if ((double) impulseInLastStep > (double) num)
            num = impulseInLastStep;
        }
        if ((double) num <= (double) MyDoor.IMPULSE_THRESHOLD)
          return;
        this.Open = true;
      }
    }

    private void UpdateCurrentOpening()
    {
      if ((double) this.m_currSpeed == 0.0 || !this.Enabled || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        return;
      this.m_currOpening = MathHelper.Clamp(this.m_currOpening + this.m_currSpeed * ((float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime) / 1000f), 0.0f, this.MaxOpen);
    }

    private void UpdateSlidingDoorsPosition()
    {
      if (this.CubeGrid.Physics == null)
        return;
      bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer;
      float x = this.m_currOpening * 0.65f;
      Vector3 position;
      if (this.m_leftSubpart != null)
      {
        position = new Vector3(-x, 0.0f, 0.0f);
        Matrix result1;
        Matrix.CreateTranslation(ref position, out result1);
        Matrix renderLocal = result1 * this.PositionComp.LocalMatrixRef;
        Matrix result2 = Matrix.Identity;
        result2.Translation = Vector3.Zero;
        Matrix.Multiply(ref result2, ref result1, out result2);
        this.m_leftSubpart.PositionComp.SetLocalMatrix(ref result2, flag ? (object) (MyPhysicsComponentBase) null : (object) this.m_leftSubpart.Physics, true, ref renderLocal, true);
        if ((HkReferenceObject) this.m_leftConstraintData != (HkReferenceObject) null)
        {
          if (this.CubeGrid.Physics != null)
            this.CubeGrid.Physics.RigidBody.Activate();
          this.m_leftSubpart.Physics.RigidBody.Activate();
          position = new Vector3(x, 0.0f, 0.0f);
          Matrix result3;
          Matrix.CreateTranslation(ref position, out result3);
          this.m_leftConstraintData.SetInBodySpace(this.PositionComp.LocalMatrixRef, result3, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) this.m_leftSubpart.Physics);
        }
      }
      if (this.m_rightSubpart == null)
        return;
      position = new Vector3(x, 0.0f, 0.0f);
      Matrix result4;
      Matrix.CreateTranslation(ref position, out result4);
      Matrix renderLocal1 = result4 * this.PositionComp.LocalMatrixRef;
      Matrix result5 = Matrix.Identity;
      result5.Translation = Vector3.Zero;
      Matrix.Multiply(ref result5, ref result4, out result5);
      this.m_rightSubpart.PositionComp.SetLocalMatrix(ref result5, flag ? (object) (MyPhysicsComponentBase) null : (object) this.m_rightSubpart.Physics, true, ref renderLocal1, true);
      if (!((HkReferenceObject) this.m_rightConstraintData != (HkReferenceObject) null))
        return;
      if (this.CubeGrid.Physics != null)
        this.CubeGrid.Physics.RigidBody.Activate();
      this.m_rightSubpart.Physics.RigidBody.Activate();
      this.m_rightConstraintData.SetInBodySpace(this.PositionComp.LocalMatrixRef, Matrix.CreateTranslation(new Vector3(-x, 0.0f, 0.0f)), (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) this.m_rightSubpart.Physics);
    }

    protected override void Closing()
    {
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.InitSubparts();
      this.RecreateConstraints((MyEntity) this.CubeGrid, false);
    }

    private void CubeGrid_OnHavokSystemIDChanged(int id)
    {
      MyEntitySubpart leftSubpart = this.m_leftSubpart;
      bool? isInWorld;
      int num1;
      if (leftSubpart == null)
      {
        num1 = 0;
      }
      else
      {
        isInWorld = leftSubpart.Physics?.IsInWorld;
        bool flag = true;
        num1 = isInWorld.GetValueOrDefault() == flag & isInWorld.HasValue ? 1 : 0;
      }
      int num2;
      if (num1 == 0)
      {
        MyEntitySubpart rightSubpart = this.m_rightSubpart;
        if (rightSubpart == null)
        {
          num2 = 0;
        }
        else
        {
          isInWorld = rightSubpart.Physics?.IsInWorld;
          bool flag = true;
          num2 = isInWorld.GetValueOrDefault() == flag & isInWorld.HasValue ? 1 : 0;
        }
      }
      else
        num2 = 1;
      if (!(this.CubeGrid.Physics != null & num2 != 0))
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, true);
    }

    private void RecreateConstraints(MyEntity obj, bool refreshInPlace)
    {
      if (obj == null || obj.MarkedForClose || (obj.GetPhysicsBody() == null || obj.IsPreview) || this.CubeGrid.Projector != null || (this.m_leftSubpart != null && (this.m_leftSubpart.MarkedForClose || this.m_leftSubpart.Closed) || this.m_rightSubpart != null && (this.m_rightSubpart.MarkedForClose || this.m_rightSubpart.Closed)))
        return;
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      if (this.m_leftSubpart != null)
        this.m_leftSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
      if (this.m_rightSubpart != null)
        this.m_rightSubpart.Render.SetParent(0, orAddCell.ParentCullObject);
      this.DisposeSubpartConstraint(ref this.m_leftConstraint, ref this.m_leftConstraintData);
      this.DisposeSubpartConstraint(ref this.m_rightConstraint, ref this.m_rightConstraintData);
      if (this.InScene && this.CubeGrid.Physics != null && (this.CubeGrid.Physics.IsInWorld || MyPhysicsExtensions.IsInWorldWelded(this.CubeGrid.Physics)))
        this.CreateConstraints();
      if (obj.Physics != null)
        this.UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID, refreshInPlace);
      this.UpdateSlidingDoorsPosition();
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    internal void UpdateHavokCollisionSystemID(int havokCollisionSystemID, bool refreshInPlace)
    {
      MyEntitySubpart[] myEntitySubpartArray = new MyEntitySubpart[2]
      {
        this.m_rightSubpart,
        this.m_leftSubpart
      };
      foreach (MyEntitySubpart subpart in myEntitySubpartArray)
      {
        if (subpart != null)
          MyDoorBase.SetupDoorSubpart(subpart, havokCollisionSystemID, refreshInPlace);
      }
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      this.UpdateSlidingDoorsPosition();
    }

    public override void ContactCallbackInternal()
    {
      base.ContactCallbackInternal();
      if ((bool) this.m_open || (double) this.OpenRatio <= 0.0)
        return;
      this.Open = true;
    }

    public override bool EnableContactCallbacks() => base.EnableContactCallbacks();

    public override bool IsClosing() => !(bool) this.m_open && (double) this.OpenRatio > 0.0;

    private class Sandbox_Game_Entities_MyDoor\u003C\u003EActor : IActivator, IActivator<MyDoor>
    {
      object IActivator.CreateInstance() => (object) new MyDoor();

      MyDoor IActivator<MyDoor>.CreateInstance() => new MyDoor();
    }
  }
}
