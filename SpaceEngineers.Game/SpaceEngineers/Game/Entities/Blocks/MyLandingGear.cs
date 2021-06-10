// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyLandingGear
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_LandingGear))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyLandingGear), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear)})]
  public class MyLandingGear : MyFunctionalBlock, Sandbox.Game.Entities.Interfaces.IMyLandingGear, SpaceEngineers.Game.ModAPI.IMyLandingGear, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear, IMyTrackTrails
  {
    private MySoundPair m_lockSound;
    private MySoundPair m_unlockSound;
    private MySoundPair m_failedAttachSound;
    private bool m_firstlockAttempt;
    private int m_firstLockTimer;
    private MyConcurrentPool<List<HkBodyCollision>> m_pentrationsPool = new MyConcurrentPool<List<HkBodyCollision>>(3, (Action<List<HkBodyCollision>>) (x => x.Clear()));
    private MyEntity m_possibleAttachedEntity;
    private int m_parallelQueriesCount;
    private bool m_converted;
    private const int FIRST_LOCK_TIME = 100;
    private static List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();
    private Matrix[] m_lockPositions;
    private HkConstraint m_constraint;
    private readonly VRage.Sync.Sync<LandingGearMode, SyncDirection.FromServer> m_lockModeSync;
    private LandingGearMode m_lockMode;
    private Action<VRage.ModAPI.IMyEntity> m_physicsChangedHandler;
    private VRage.ModAPI.IMyEntity m_attachedTo;
    private bool m_needsToRetryLock;
    private int m_autolockTimer;
    private float m_breakForce;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_autoLock;
    private readonly VRage.Sync.Sync<MyLandingGear.State, SyncDirection.FromServer> m_attachedState;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_breakForceSync;
    private long? m_attachedEntityId;
    private int m_retryCounter;

    public Matrix[] LockPositions => this.m_lockPositions;

    public bool LockedToStatic => this.m_converted;

    private HkConstraint SafeConstraint
    {
      get
      {
        if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && !this.m_constraint.InWorld)
          this.Detach();
        return this.m_constraint;
      }
    }

    public LandingGearMode LockMode
    {
      get => this.m_lockMode;
      private set
      {
        if (this.m_lockMode == value)
          return;
        LandingGearMode lockMode = this.m_lockMode;
        this.m_lockMode = value;
        this.SetEmissiveStateWorking();
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
        LockModeChangedHandler lockModeChanged = this.LockModeChanged;
        if (lockModeChanged == null)
          return;
        lockModeChanged((Sandbox.Game.Entities.Interfaces.IMyLandingGear) this, lockMode);
      }
    }

    public bool IsLocked => this.LockMode == LandingGearMode.Locked;

    public event LockModeChangedHandler LockModeChanged;

    public MyLandingGear()
    {
      this.CreateTerminalControls();
      this.m_physicsChangedHandler = new Action<VRage.ModAPI.IMyEntity>(this.PhysicsChanged);
      this.m_attachedState.AlwaysReject<MyLandingGear.State, SyncDirection.FromServer>();
      this.m_attachedState.ValueChanged += (Action<SyncBase>) (x => this.AttachedValueChanged());
      this.m_autoLock.ValueChanged += (Action<SyncBase>) (x => this.AutolockChanged());
      this.m_lockModeSync.AlwaysReject<LandingGearMode, SyncDirection.FromServer>();
      this.m_lockModeSync.ValueChanged += (Action<SyncBase>) (x => this.OnLockModeChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLandingGear>())
        return;
      base.CreateTerminalControls();
      MyTerminalControl<MyLandingGear>.WriterDelegate writer = (MyTerminalControl<MyLandingGear>.WriterDelegate) ((b, sb) => b.WriteLockStateValue(sb));
      MyTerminalControlButton<MyLandingGear> button1 = new MyTerminalControlButton<MyLandingGear>("Lock", MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.Blank, (Action<MyLandingGear>) (b => b.RequestLandingGearLock()));
      button1.Enabled = (Func<MyLandingGear, bool>) (b => b.IsWorking);
      button1.EnableAction<MyLandingGear>(MyTerminalActionIcons.TOGGLE, writer: writer);
      MyTerminalControlFactory.AddControl<MyLandingGear>((MyTerminalControl<MyLandingGear>) button1);
      MyTerminalControlButton<MyLandingGear> button2 = new MyTerminalControlButton<MyLandingGear>("Unlock", MySpaceTexts.BlockActionTitle_Unlock, MySpaceTexts.Blank, (Action<MyLandingGear>) (b => b.RequestLandingGearUnlock()));
      button2.Enabled = (Func<MyLandingGear, bool>) (b => b.IsWorking);
      button2.EnableAction<MyLandingGear>(MyTerminalActionIcons.TOGGLE, writer: writer);
      MyTerminalControlFactory.AddControl<MyLandingGear>((MyTerminalControl<MyLandingGear>) button2);
      MyTerminalControlFactory.AddAction<MyLandingGear>(new MyTerminalAction<MyLandingGear>("SwitchLock", MyTexts.Get(MySpaceTexts.BlockActionTitle_SwitchLock), MyTerminalActionIcons.TOGGLE)
      {
        Action = (Action<MyLandingGear>) (b => b.RequestLandingGearSwitch()),
        Writer = writer
      });
      MyTerminalControlCheckbox<MyLandingGear> checkbox = new MyTerminalControlCheckbox<MyLandingGear>("Autolock", MySpaceTexts.BlockPropertyTitle_LandGearAutoLock, MySpaceTexts.Blank);
      checkbox.Getter = (MyTerminalValueControl<MyLandingGear, bool>.GetterDelegate) (b => (bool) b.m_autoLock);
      checkbox.Setter = (MyTerminalValueControl<MyLandingGear, bool>.SetterDelegate) ((b, v) => b.m_autoLock.Value = v);
      checkbox.EnableAction<MyLandingGear>();
      MyTerminalControlFactory.AddControl<MyLandingGear>((MyTerminalControl<MyLandingGear>) checkbox);
      if (!MyFakes.LANDING_GEAR_BREAKABLE)
        return;
      MyTerminalControlSlider<MyLandingGear> slider = new MyTerminalControlSlider<MyLandingGear>("BreakForce", MySpaceTexts.BlockPropertyTitle_BreakForce, MySpaceTexts.BlockPropertyDescription_BreakForce);
      slider.Getter = (MyTerminalValueControl<MyLandingGear, float>.GetterDelegate) (x => x.BreakForce);
      slider.Setter = (MyTerminalValueControl<MyLandingGear, float>.SetterDelegate) ((x, v) => x.m_breakForceSync.Value = v);
      slider.DefaultValue = new float?(1f);
      slider.Writer = (MyTerminalControl<MyLandingGear>.WriterDelegate) ((x, result) =>
      {
        if ((double) x.BreakForce >= 100000000.0)
          result.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
        else
          MyValueFormatter.AppendForceInBestUnit(x.BreakForce, result);
      });
      slider.Normalizer = (MyTerminalControlSlider<MyLandingGear>.FloatFunc) ((b, v) => MyLandingGear.ThresholdToRatio(v));
      slider.Denormalizer = (MyTerminalControlSlider<MyLandingGear>.FloatFunc) ((b, v) => MyLandingGear.RatioToThreshold(v));
      slider.EnableActions<MyLandingGear>();
      MyTerminalControlFactory.AddControl<MyLandingGear>((MyTerminalControl<MyLandingGear>) slider);
    }

    private void OnLockModeChanged() => this.LockMode = (LandingGearMode) this.m_lockModeSync;

    private void BreakForceChanged() => this.BreakForce = (float) this.m_breakForceSync;

    private void AutolockChanged()
    {
      this.m_autolockTimer = 0;
      this.SetEmissiveStateWorking();
      this.RaisePropertiesChanged();
    }

    public bool IsBreakable => (double) this.BreakForce < 100000000.0;

    public void RequestLandingGearSwitch()
    {
      if (this.LockMode == LandingGearMode.Locked)
        this.RequestLandingGearUnlock();
      else
        this.RequestLandingGearLock();
    }

    public void RequestLandingGearLock()
    {
      if (this.LockMode != LandingGearMode.ReadyToLock)
        return;
      this.RequestLock(true);
    }

    public void RequestLandingGearUnlock()
    {
      if (this.LockMode != LandingGearMode.Locked)
        return;
      this.RequestLock(false);
    }

    public float BreakForce
    {
      get => this.m_breakForce;
      set
      {
        if ((double) this.m_breakForce == (double) value)
          return;
        int num1 = this.IsBreakable ? 1 : 0;
        this.m_breakForce = value;
        int num2 = this.IsBreakable ? 1 : 0;
        if (num1 != num2)
        {
          this.m_breakForce = value;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            this.ResetLockConstraint(this.LockMode == LandingGearMode.Locked);
          this.RaisePropertiesChanged();
        }
        else
        {
          if (!this.IsBreakable)
            return;
          this.UpdateBrakeThreshold();
          this.RaisePropertiesChanged();
        }
      }
    }

    private static float RatioToThreshold(float ratio) => (double) ratio < 1.0 ? MathHelper.InterpLog(ratio, 500f, 1E+08f) : 1E+08f;

    private static float ThresholdToRatio(float threshold) => (double) threshold < 100000000.0 ? MathHelper.InterpLogInv(threshold, 500f, 1E+08f) : 1f;

    private void UpdateBrakeThreshold()
    {
      if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null) || !(this.m_constraint.ConstraintData is HkBreakableConstraintData))
        return;
      ((HkBreakableConstraintData) this.m_constraint.ConstraintData).Threshold = this.BreakForce;
      if (this.m_attachedTo == null || this.m_attachedTo.Physics == null)
        return;
      this.m_attachedTo.Physics.RigidBody.Activate();
    }

    private bool CanAutoLock => (bool) this.m_autoLock && this.m_autolockTimer == 0;

    public bool AutoLock
    {
      get => (bool) this.m_autoLock;
      set => this.m_autoLock.Value = value;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (this.BlockDefinition is MyLandingGearDefinition)
      {
        MyLandingGearDefinition blockDefinition = (MyLandingGearDefinition) this.BlockDefinition;
        this.m_lockSound = new MySoundPair(blockDefinition.LockSound);
        this.m_unlockSound = new MySoundPair(blockDefinition.UnlockSound);
        this.m_failedAttachSound = new MySoundPair(blockDefinition.FailedAttachSound);
        if (blockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
          blockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("ConnectBlock");
      }
      else
      {
        this.m_lockSound = new MySoundPair("ShipLandGearOn");
        this.m_unlockSound = new MySoundPair("ShipLandGearOff");
        this.m_failedAttachSound = new MySoundPair("ShipLandGearNothing01");
      }
      this.Flags |= EntityFlags.NeedsUpdate | EntityFlags.NeedsUpdate10 | EntityFlags.NeedsUpdateBeforeNextFrame;
      this.LoadDummies();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      MyObjectBuilder_LandingGear builderLandingGear = objectBuilder as MyObjectBuilder_LandingGear;
      if (builderLandingGear.IsLocked)
      {
        this.LockMode = LandingGearMode.Locked;
        this.m_needsToRetryLock = true;
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.m_attachedEntityId = builderLandingGear.AttachedEntityId;
          this.m_attachedState.Value = new MyLandingGear.State()
          {
            OtherEntityId = builderLandingGear.AttachedEntityId,
            GearPivotPosition = builderLandingGear.GearPivotPosition,
            OtherPivot = builderLandingGear.OtherPivot,
            MasterToSlave = builderLandingGear.MasterToSlave
          };
        }
      }
      this.m_breakForceSync.Value = !MyFakes.LANDING_GEAR_BREAKABLE ? MyLandingGear.RatioToThreshold(1E+08f) : MyLandingGear.RatioToThreshold(builderLandingGear.BrakeForce);
      this.m_breakForceSync.ValueChanged += (Action<SyncBase>) (x => this.BreakForceChanged());
      this.AutoLock = builderLandingGear.AutoLock;
      this.m_lockModeSync.SetLocalValue(builderLandingGear.LockMode);
      this.m_firstlockAttempt = builderLandingGear.FirstLockAttempt;
      this.m_firstLockTimer = 0;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyLandingGear_IsWorkingChanged);
      this.SetDetailedInfoDirty();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentLandingGear(this));
    }

    private void MyLandingGear_IsWorkingChanged(MyCubeBlock obj) => this.RaisePropertiesChanged();

    public override void ContactPointCallback(ref MyGridContactInfo info)
    {
      if (info.CollidingEntity == null || this.m_attachedTo != info.CollidingEntity)
        return;
      info.EnableDeformation = false;
      info.EnableParticles = false;
    }

    private void LoadDummies() => this.m_lockPositions = this.Model.Dummies.Where<KeyValuePair<string, MyModelDummy>>((Func<KeyValuePair<string, MyModelDummy>, bool>) (s => s.Key.ToLower().Contains("gear_lock"))).Select<KeyValuePair<string, MyModelDummy>, Matrix>((Func<KeyValuePair<string, MyModelDummy>, Matrix>) (s => s.Value.Matrix)).ToArray<Matrix>();

    public void GetBoxFromMatrix(
      MatrixD m,
      out Vector3 halfExtents,
      out Vector3D position,
      out Quaternion orientation)
    {
      MatrixD matrix = MatrixD.Normalize(m) * this.WorldMatrix;
      orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      halfExtents = Vector3.Abs((Vector3) m.Scale) / 2f;
      position = matrix.Translation;
    }

    private void FindBodyParallel()
    {
      if (this.CubeGrid.IsPreview || this.m_parallelQueriesCount != 0)
        return;
      foreach (Matrix lockPosition in this.m_lockPositions)
      {
        Vector3 halfExtents;
        Vector3D position;
        Quaternion orientation;
        this.GetBoxFromMatrix((MatrixD) ref lockPosition, out halfExtents, out position, out orientation);
        halfExtents *= new Vector3(2f, 1f, 2f);
        orientation.Normalize();
        ++this.m_parallelQueriesCount;
        Vector3D tempPivot = position;
        MyPhysics.GetPenetrationsBoxParallel(ref halfExtents, ref position, ref orientation, this.m_pentrationsPool.Get(), 15, (Action<List<HkBodyCollision>>) (penetrations => MySandboxGame.Static.Invoke((Action) (() => this.FindBodyResult(penetrations, tempPivot)), "Landing Gear - FindBodyResult")));
      }
    }

    private void FindBodyResult(List<HkBodyCollision> penetrations, Vector3D tempPivot)
    {
      foreach (HkBodyCollision penetration in penetrations)
      {
        if (penetration.GetCollisionEntity() is MyEntity collisionEntity)
        {
          MyEntity topMostParent = collisionEntity.GetTopMostParent((System.Type) null);
          if (topMostParent.GetPhysicsBody() != null && this.CanAttachTo(penetration, topMostParent, tempPivot))
          {
            this.m_possibleAttachedEntity = topMostParent;
            break;
          }
        }
      }
      this.m_pentrationsPool.Return(penetrations);
      --this.m_parallelQueriesCount;
    }

    private MyEntity FindBody(out Vector3D pivot)
    {
      pivot = Vector3D.Zero;
      if (this.CubeGrid.Physics == null)
        return (MyEntity) null;
      foreach (Matrix lockPosition in this.m_lockPositions)
      {
        Vector3 halfExtents;
        Quaternion orientation;
        this.GetBoxFromMatrix((MatrixD) ref lockPosition, out halfExtents, out pivot, out orientation);
        try
        {
          halfExtents *= new Vector3(2f, 1f, 2f);
          orientation.Normalize();
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref pivot, ref orientation, MyLandingGear.m_penetrations, 15);
          foreach (HkBodyCollision penetration in MyLandingGear.m_penetrations)
          {
            if (penetration.GetCollisionEntity() is MyEntity collisionEntity)
            {
              MyEntity topMostParent = collisionEntity.GetTopMostParent((System.Type) null);
              if (topMostParent.GetPhysicsBody() != null && this.CanAttachTo(penetration, topMostParent, pivot))
                return topMostParent;
            }
          }
        }
        finally
        {
          MyLandingGear.m_penetrations.Clear();
        }
      }
      return (MyEntity) null;
    }

    private bool CanAttachTo(HkBodyCollision obj, MyEntity entity, Vector3D worldPos)
    {
      MyCubeGrid cubeGrid = this.CubeGrid;
      if (entity == cubeGrid || entity.MarkedForClose)
        return false;
      MyEntity topMostParent = entity.GetTopMostParent((System.Type) null);
      if (topMostParent == cubeGrid || topMostParent.MarkedForClose || topMostParent is MyCubeGrid myCubeGrid && myCubeGrid.IsPreview)
        return false;
      HkRigidBody body1 = obj.Body;
      if (body1.IsDisposed)
      {
        MyPhysicsBody body2 = body1.GetBody();
        if (body2 != null)
        {
          VRage.ModAPI.IMyEntity entity1 = body2.Entity;
        }
        return false;
      }
      MyGridPhysics physics1 = cubeGrid.Physics;
      MyPhysicsComponentBase physics2 = entity.Physics;
      if (physics1 != null && physics2 != null && this.BlockDefinition is MyLandingGearDefinition blockDefinition)
      {
        float separatingVelocity = blockDefinition.MaxLockSeparatingVelocity;
        Vector3 velocityAtPoint = physics2.GetVelocityAtPoint(worldPos);
        if ((double) (physics1.LinearVelocity - velocityAtPoint).LengthSquared() > (double) separatingVelocity * (double) separatingVelocity)
          return false;
      }
      if (!body1.IsFixed && body1.IsFixedOrKeyframed)
        return false;
      switch (entity)
      {
        case MyCharacter _:
        case MySafeZone _:
          return false;
        default:
          return true;
      }
    }

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      switch (this.LockMode)
      {
        case LandingGearMode.Unlocked:
          return this.CanAutoLock && this.Enabled ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Autolock, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
        case LandingGearMode.ReadyToLock:
          return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, this.Render.RenderObjectIDs[0]);
        case LandingGearMode.Locked:
          return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, this.Render.RenderObjectIDs[0]);
        default:
          return false;
      }
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_LockState));
      this.WriteLockStateValue(detailedInfo);
    }

    private void WriteLockStateValue(StringBuilder sb)
    {
      if (this.LockMode == LandingGearMode.Locked)
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Locked));
      else if (this.LockMode == LandingGearMode.ReadyToLock)
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_ReadyToLock));
      else
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Unlocked));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_LandingGear builderCubeBlock = (MyObjectBuilder_LandingGear) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.IsLocked = this.LockMode == LandingGearMode.Locked;
      builderCubeBlock.BrakeForce = MyLandingGear.ThresholdToRatio(this.BreakForce);
      builderCubeBlock.AutoLock = this.AutoLock;
      builderCubeBlock.LockSound = this.m_lockSound.ToString();
      builderCubeBlock.FirstLockAttempt = this.m_firstlockAttempt;
      builderCubeBlock.UnlockSound = this.m_unlockSound.ToString();
      builderCubeBlock.FailedAttachSound = this.m_failedAttachSound.ToString();
      builderCubeBlock.AttachedEntityId = this.m_attachedEntityId;
      if (this.m_attachedEntityId.HasValue)
      {
        builderCubeBlock.MasterToSlave = this.m_attachedState.Value.MasterToSlave;
        builderCubeBlock.GearPivotPosition = this.m_attachedState.Value.GearPivotPosition;
        builderCubeBlock.OtherPivot = this.m_attachedState.Value.OtherPivot;
      }
      builderCubeBlock.LockMode = this.m_lockModeSync.Value;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.EnqueueRetryLock();
      this.Detach();
    }

    public override void UpdateBeforeSimulation()
    {
      this.RetryLock();
      base.UpdateBeforeSimulation();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.CheckEmissiveState();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (!this.IsWorking && !this.m_firstlockAttempt)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (this.m_firstlockAttempt)
        {
          if (this.m_firstLockTimer >= 100)
            this.m_firstlockAttempt = false;
          ++this.m_firstLockTimer;
        }
        if (this.LockMode != LandingGearMode.Locked)
        {
          this.FindBodyParallel();
          if (this.m_possibleAttachedEntity != null)
          {
            if (this.CanAutoLock)
              this.AttachRequest(true);
            else
              this.m_lockModeSync.Value = LandingGearMode.ReadyToLock;
            this.m_possibleAttachedEntity = (MyEntity) null;
          }
          else
            this.m_lockModeSync.Value = LandingGearMode.Unlocked;
        }
      }
      if (this.m_autolockTimer == 0 || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_autolockTimer <= 3000)
        return;
      this.AutoLock = true;
      this.AutolockChanged();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_needsToRetryLock = true;
      this.RetryLock();
    }

    private void RetryLock()
    {
      if (!this.m_needsToRetryLock)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.RetryLockServer();
      else
        this.RetryLockClient();
    }

    private void RetryLockServer()
    {
      MyEntity body = this.FindBody(out Vector3D _);
      if (body == null && this.m_retryCounter < 3)
      {
        ++this.m_retryCounter;
      }
      else
      {
        this.m_retryCounter = 0;
        if (body != null && (this.m_attachedEntityId.HasValue && body.EntityId == this.m_attachedEntityId.Value || !this.m_attachedEntityId.HasValue))
        {
          if (this.m_attachedTo == null || this.m_attachedTo.Physics == null || (HkReferenceObject) body.Physics.RigidBody != (HkReferenceObject) this.m_attachedTo.Physics.RigidBody)
            this.ResetLockConstraint(true);
        }
        else
        {
          long? attachedEntityId = this.m_attachedEntityId;
          this.ResetLockConstraint(false);
          this.m_attachedEntityId = attachedEntityId;
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }
        this.m_needsToRetryLock = false;
      }
    }

    private void RetryLockClient()
    {
      MyLandingGear.State state = this.m_attachedState.Value;
      if (!state.OtherEntityId.HasValue)
      {
        this.m_needsToRetryLock = false;
      }
      else
      {
        long entityId = state.OtherEntityId.Value;
        MyEntity entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity);
        if (entity == null)
        {
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }
        else
        {
          if (this.m_attachedEntityId.HasValue && (this.m_attachedEntityId.Value != entityId || state.Force))
            this.Detach();
          this.m_attachedEntityId = new long?(entityId);
          if (state.MasterToSlave.HasValue && state.GearPivotPosition.HasValue && state.OtherPivot.HasValue)
          {
            if (!(entity is MyFloatingObject))
              this.CubeGrid.WorldMatrix = MatrixD.Multiply((MatrixD) state.MasterToSlave.Value, entity.WorldMatrix);
            this.Attach(entity, state.GearPivotPosition.Value, state.OtherPivot.Value.Matrix);
          }
          this.m_needsToRetryLock = false;
        }
      }
    }

    protected override void Closing()
    {
      this.Detach();
      base.Closing();
    }

    public void ResetAutolock()
    {
      this.m_autolockTimer = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.SetEmissiveStateWorking();
    }

    [Event(null, 795)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private void AttachFailed() => this.StartSound(this.m_failedAttachSound);

    private void Attach(
      long entityID,
      Vector3 gearSpacePivot,
      CompressedPositionOrientation otherBodySpacePivot,
      bool force = false)
    {
      this.m_attachedEntityId = new long?(entityID);
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityID, out entity))
        return;
      this.Attach(entity, gearSpacePivot, otherBodySpacePivot.Matrix);
    }

    private void Attach(MyEntity entity, Vector3 gearSpacePivot, Matrix otherBodySpacePivot)
    {
      if (entity == null)
        return;
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated && entity is MyVoxelBase)
      {
        Vector3D pivot;
        this.FindBody(out pivot);
        this.ProcessTrails((MyVoxelBase) entity, pivot);
      }
      this.m_attachedEntityId = new long?(entity.EntityId);
      if (this.CubeGrid?.Physics == null || !this.CubeGrid.Physics.Enabled || (this.m_attachedTo != null || (HkReferenceObject) this.m_constraint != (HkReferenceObject) null))
        return;
      MyEntity topMostParent = entity.GetTopMostParent((System.Type) null);
      if (topMostParent?.Physics == null)
        return;
      if (entity.Physics == null)
        MyLog.Default.WriteLine("entity " + (object) entity + " has no physics, topmost parent " + (object) entity.GetTopMostParent((System.Type) null) + " has physics");
      Action<bool> stateChanged = this.StateChanged;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && entity is MyCubeGrid myCubeGrid)
        myCubeGrid.OnGridSplit += new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      if (MyFakes.WELD_LANDING_GEARS && this.CanWeldTo(entity, ref otherBodySpacePivot))
      {
        if (entity is MyVoxelBase)
        {
          Vector3D zero = (Vector3D) Vector3.Zero;
          MyVoxelMaterialDefinition materialAt = ((MyVoxelBase) entity).GetMaterialAt(ref zero);
          if (materialAt != null)
          {
            this.AddTrails(zero, this.WorldMatrix.Up, this.WorldMatrix.Forward, entity.EntityId, materialAt.MaterialTypeNameHash, materialAt.Id.SubtypeId);
            if ((HkReferenceObject) this.CubeGrid.Physics.RigidBody == (HkReferenceObject) null)
              MyLog.Default.WriteLine("Rigid body null for CubeGrid " + (object) this.CubeGrid);
            if (!this.m_converted && this.AnyGearLockedToStatic())
              this.m_converted = true;
            if (!this.m_converted)
            {
              HkRigidBody rigidBody = this.CubeGrid.Physics.RigidBody;
              if ((rigidBody != null ? (!rigidBody.IsFixed ? 1 : 0) : 0) != 0 && !this.CubeGrid.IsStatic)
              {
                MySandboxGame.Static.Invoke((Action) (() =>
                {
                  if (this.CubeGrid.Physics == null)
                    return;
                  this.CubeGrid.Physics.ConvertToStatic();
                }), "MyLandingGear / Convert to static");
                this.m_converted = true;
              }
            }
          }
        }
        else if (topMostParent is MyCubeGrid childNode)
          MyGridPhysicalHierarchy.Static.CreateLink(this.EntityId, this.CubeGrid, childNode);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_lockModeSync.Value = LandingGearMode.Locked;
        this.m_attachedTo = (VRage.ModAPI.IMyEntity) entity;
        this.m_attachedTo.OnPhysicsChanged += this.m_physicsChangedHandler;
        this.OnPhysicsChanged += (Action<MyEntity>) this.m_physicsChangedHandler;
        if (this.CanAutoLock)
          this.ResetAutolock();
        this.OnConstraintAdded(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) entity);
        if (!this.m_needsToRetryLock)
          this.StartSound(this.m_lockSound);
        if (stateChanged == null)
          return;
        stateChanged(true);
      }
      else
      {
        HkRigidBody rigidBody = topMostParent.Physics.RigidBody;
        if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        {
          MyLog.Default.WriteLine("Rigid body null for Entity " + (object) entity);
        }
        else
        {
          rigidBody.Activate();
          this.CubeGrid.Physics.RigidBody.Activate();
          this.m_attachedTo = (VRage.ModAPI.IMyEntity) entity;
          this.m_attachedTo.OnPhysicsChanged += this.m_physicsChangedHandler;
          this.OnPhysicsChanged += (Action<MyEntity>) this.m_physicsChangedHandler;
          Matrix identity = Matrix.Identity;
          identity.Translation = gearSpacePivot;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            HkFixedConstraintData data = new HkFixedConstraintData();
            if (MyFakes.OVERRIDE_LANDING_GEAR_INERTIA)
              data.SetInertiaStabilizationFactor(MyFakes.LANDING_GEAR_INTERTIA);
            data.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
            data.SetInBodySpace(identity, otherBodySpacePivot, (MyPhysicsBody) this.CubeGrid.Physics, entity.Physics as MyPhysicsBody);
            HkConstraintData constraintData = (HkConstraintData) data;
            if (MyFakes.LANDING_GEAR_BREAKABLE && (double) this.BreakForce < 100000000.0)
            {
              HkBreakableConstraintData breakableConstraintData = new HkBreakableConstraintData((HkConstraintData) data);
              data.Dispose();
              breakableConstraintData.Threshold = this.BreakForce;
              breakableConstraintData.ReapplyVelocityOnBreak = true;
              breakableConstraintData.RemoveFromWorldOnBrake = true;
              constraintData = (HkConstraintData) breakableConstraintData;
            }
            this.m_constraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, rigidBody, constraintData);
            this.CubeGrid.Physics.AddConstraint(this.m_constraint);
            this.m_constraint.Enabled = true;
          }
          if (!this.m_needsToRetryLock)
            this.StartSound(this.m_lockSound);
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            this.m_lockModeSync.Value = LandingGearMode.Locked;
          if (this.CanAutoLock)
            this.ResetAutolock();
          this.OnConstraintAdded(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) entity);
          if (topMostParent is MyCubeGrid myCubeGrid)
          {
            MyFixedGrids.Link(this.CubeGrid, myCubeGrid, (MyCubeBlock) this);
            MyGridPhysicalHierarchy.Static.CreateLink(this.EntityId, this.CubeGrid, myCubeGrid);
          }
          stateChanged.InvokeIfNotNull<bool>(true);
          if (!(entity is MyVoxelBase))
            return;
          MyFixedGrids.MarkGridRoot(this.CubeGrid);
          MyGridPhysicalHierarchy.Static.UpdateRoot(this.CubeGrid);
          this.m_converted = true;
        }
      }
    }

    private void ProcessTrails(MyVoxelBase entity, Vector3D pivot)
    {
      IReadOnlyList<MyDecalMaterial> decalMaterials = (IReadOnlyList<MyDecalMaterial>) null;
      MyVoxelMaterialDefinition materialAt = entity.GetMaterialAt(ref pivot);
      if (materialAt == null)
        return;
      Vector3D zero = Vector3D.Zero;
      MyStringHash myStringHash = this.BlockDefinition.Id.SubtypeId;
      string source = myStringHash.String;
      myStringHash = materialAt.MaterialTypeNameHash;
      string target1 = myStringHash.String;
      ref IReadOnlyList<MyDecalMaterial> local = ref decalMaterials;
      MyStringHash subtypeId1 = materialAt.Id.SubtypeId;
      bool decalMaterial = MyDecalMaterials.TryGetDecalMaterial(source, target1, out local, subtypeId1);
      if (!decalMaterial)
      {
        myStringHash = this.BlockDefinition.Id.SubtypeId;
        decalMaterial = MyDecalMaterials.TryGetDecalMaterial(myStringHash.String, "GenericMaterial", out decalMaterials, materialAt.Id.SubtypeId);
      }
      if (!decalMaterial || decalMaterials == null || (decalMaterials.Count <= 0 || decalMaterials[0] == null))
        return;
      Vector3D vector3D = (double) decalMaterials[0].XOffset * this.WorldMatrix.Right + (double) decalMaterials[0].YOffset * this.WorldMatrix.Forward;
      Vector3D position = pivot;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      long entityId = entity.EntityId;
      MyStringHash target2 = decalMaterials[0].Target;
      MyStringHash subtypeId2 = materialAt.Id.SubtypeId;
      this.AddTrails(position, up, forward, entityId, target2, subtypeId2);
    }

    private bool CanWeldTo(MyEntity entity, ref Matrix otherBodySpacePivot)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer && (double) this.BreakForce < 100000000.0)
        return false;
      if (!(entity is MyCubeGrid myCubeGrid) && entity is MyCubeBlock myCubeBlock)
        myCubeGrid = myCubeBlock.CubeGrid;
      if (myCubeGrid != null)
      {
        Vector3I cube;
        myCubeGrid.FixTargetCube(out cube, otherBodySpacePivot.Translation * myCubeGrid.GridSizeR);
        MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(cube);
        if (cubeBlock != null && cubeBlock.FatBlock is MyAirtightHangarDoor)
          return false;
      }
      else if (entity.Parent != null)
        return false;
      return true;
    }

    private bool AnyGearLockedToStatic()
    {
      foreach (MyCubeGrid groupNode in MyGridPhysicalHierarchy.Static.GetGroupNodes(this.CubeGrid))
      {
        foreach (MyLandingGear fatBlock in groupNode.GetFatBlocks<MyLandingGear>())
        {
          if (fatBlock.LockedToStatic)
            return true;
        }
      }
      return false;
    }

    private void Detach()
    {
      if (this.CubeGrid.Physics == null || this.m_attachedTo == null)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_attachedTo is MyCubeGrid)
        (this.m_attachedTo as MyCubeGrid).OnGridSplit -= new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      this.OnPhysicsChanged -= (Action<MyEntity>) this.m_physicsChangedHandler;
      bool flag1 = false;
      VRage.ModAPI.IMyEntity attachedTo = this.m_attachedTo;
      if (this.m_attachedTo != null)
        this.m_attachedTo.OnPhysicsChanged -= this.m_physicsChangedHandler;
      if (MyFakes.WELD_LANDING_GEARS && MyWeldingGroups.Static.LinkExists(this.EntityId, (MyEntity) this.CubeGrid, (MyEntity) attachedTo))
      {
        MyWeldingGroups.Static.BreakLink(this.EntityId, (MyEntity) this.CubeGrid, (MyEntity) attachedTo);
        flag1 = true;
      }
      else if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
      {
        flag1 = this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        if (flag1)
        {
          this.m_constraint.Dispose();
          this.m_constraint = (HkConstraint) null;
          this.OnConstraintRemoved(GridLinkTypeEnum.NoContactDamage, attachedTo);
        }
      }
      else if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        flag1 = true;
      if (flag1)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_lockModeSync.Value = LandingGearMode.Unlocked;
        this.m_attachedTo = (VRage.ModAPI.IMyEntity) null;
        this.m_attachedEntityId = new long?();
        if (attachedTo is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = (MyCubeGrid) attachedTo;
          MyFixedGrids.BreakLink(this.CubeGrid, myCubeGrid, (MyCubeBlock) this);
          MyGridPhysicalHierarchy.Static.BreakLink(this.EntityId, this.CubeGrid, myCubeGrid);
          if (!myCubeGrid.IsStatic && myCubeGrid.Physics != null)
            myCubeGrid.Physics.ForceActivate();
        }
        if (!this.m_needsToRetryLock && !this.MarkedForClose)
          this.StartSound(this.m_unlockSound);
        this.OnConstraintRemoved(GridLinkTypeEnum.Physical, attachedTo);
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_attachedState.Value = new MyLandingGear.State()
          {
            OtherEntityId = new long?()
          };
        this.StateChanged.InvokeIfNotNull<bool>(false);
        if (!(attachedTo is MyVoxelBase) || this.CubeGrid.IsStatic)
          return;
        this.m_converted = false;
        bool flag2 = false;
        foreach (MyLandingGear fatBlock in this.CubeGrid.GetFatBlocks<MyLandingGear>())
        {
          if (fatBlock.LockedToStatic)
          {
            flag2 = true;
            break;
          }
        }
        if (flag2)
          return;
        MyFixedGrids.UnmarkGridRoot(this.CubeGrid);
        MyGridPhysicalHierarchy.Static.UpdateRoot(this.CubeGrid);
      }
      else
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_attachedTo is MyCubeGrid)
          (this.m_attachedTo as MyCubeGrid).OnGridSplit += new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
        this.OnPhysicsChanged += (Action<MyEntity>) this.m_physicsChangedHandler;
        if (this.m_attachedTo == null)
          return;
        this.m_attachedTo.OnPhysicsChanged += this.m_physicsChangedHandler;
      }
    }

    private void PhysicsChanged(VRage.ModAPI.IMyEntity entity)
    {
      if (entity is MyVoxelBase && entity.Physics == null)
        return;
      if (entity.Physics == null)
      {
        if (this.LockMode == LandingGearMode.Locked && Sandbox.Game.Multiplayer.Sync.IsServer)
          this.m_needsToRetryLock = true;
        this.Detach();
      }
      else
      {
        if (this.LockMode != LandingGearMode.Locked || !Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.m_needsToRetryLock = true;
      }
    }

    public void EnqueueRetryLock()
    {
      if (this.m_needsToRetryLock || this.LockMode != LandingGearMode.Locked || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_needsToRetryLock = true;
    }

    private void ComponentStack_IsFunctionalChanged()
    {
    }

    [Event(null, 1209)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void ResetLockConstraint(bool locked, bool force = false)
    {
      if (this.CubeGrid == null || this.CubeGrid.Physics == null)
        return;
      this.Detach();
      if (locked)
      {
        Vector3D pivot;
        MyEntity body = this.FindBody(out pivot);
        if (body != null)
          this.AttachEntity(pivot, body, force);
      }
      else if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_lockModeSync.Value = LandingGearMode.Unlocked;
      this.m_needsToRetryLock = false;
    }

    public void RequestLock(bool enable)
    {
      if (!this.IsWorking)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLandingGear, bool>(this, (Func<MyLandingGear, Action<bool>>) (x => new Action<bool>(x.AttachRequest)), enable);
    }

    private void StartSound(MySoundPair cueEnum)
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(cueEnum, true);
    }

    private event Action<bool> StateChanged;

    VRage.ModAPI.IMyEntity SpaceEngineers.Game.ModAPI.IMyLandingGear.GetAttachedEntity() => this.m_attachedTo;

    [Event(null, 1258)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void AttachRequest(bool enable)
    {
      if (enable)
      {
        if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.LandingGearLock))
          return;
        Vector3D pivot;
        MyEntity body = this.FindBody(out pivot);
        if (body != null)
          this.AttachEntity(pivot, body);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLandingGear>(this, (Func<MyLandingGear, Action>) (x => new Action(x.AttachFailed)));
      }
      else
      {
        this.m_attachedState.Value = new MyLandingGear.State()
        {
          OtherEntityId = new long?()
        };
        this.ResetLockConstraint(false);
        if (this.CanAutoLock)
          this.ResetAutolock();
        if (MyVisualScriptLogicProvider.LandingGearUnlocked == null)
          return;
        MyVisualScriptLogicProvider.LandingGearUnlocked(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name);
      }
    }

    private void AttachEntity(Vector3D pivot, MyEntity otherEntity, bool force = false)
    {
      Matrix rigidBodyMatrix1 = this.CubeGrid.Physics.RigidBody.GetRigidBodyMatrix();
      Matrix rigidBodyMatrix2 = otherEntity.Physics.RigidBody.GetRigidBodyMatrix();
      Matrix matrix1 = rigidBodyMatrix1;
      matrix1.Translation = (Vector3) this.CubeGrid.Physics.WorldToCluster(pivot);
      Vector3 translation = (matrix1 * Matrix.Invert(rigidBodyMatrix1)).Translation;
      Matrix matrix2 = matrix1 * Matrix.Invert(rigidBodyMatrix2);
      CompressedPositionOrientation positionOrientation = new CompressedPositionOrientation(ref matrix2);
      long entityId = otherEntity.EntityId;
      MatrixD matrixD = this.CubeGrid.WorldMatrix * MatrixD.Invert(otherEntity.WorldMatrix);
      this.m_attachedState.Value = new MyLandingGear.State()
      {
        Force = force,
        OtherEntityId = new long?(entityId),
        GearPivotPosition = new Vector3?(translation),
        OtherPivot = new CompressedPositionOrientation?(positionOrientation),
        MasterToSlave = new MyDeltaTransform?((MyDeltaTransform) matrixD)
      };
      this.Attach(otherEntity, translation, matrix2);
    }

    private void AttachedValueChanged()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyLandingGear.State state = this.m_attachedState.Value;
      if (state.OtherEntityId.HasValue)
      {
        long num = state.OtherEntityId.Value;
        long? attachedEntityId = this.m_attachedEntityId;
        long valueOrDefault = attachedEntityId.GetValueOrDefault();
        if (num == valueOrDefault & attachedEntityId.HasValue && !state.Force)
          return;
        this.m_needsToRetryLock = true;
        this.Detach();
        this.m_attachedEntityId = new long?(state.OtherEntityId.Value);
        MyEntity entity;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(state.OtherEntityId.Value, out entity))
          return;
        this.EntityReattach(entity, state);
        this.m_needsToRetryLock = false;
      }
      else
      {
        this.ResetLockConstraint(false);
        if (!this.CanAutoLock)
          return;
        this.ResetAutolock();
      }
    }

    private void EntityReattach(MyEntity otherEntity, MyLandingGear.State state)
    {
      if (otherEntity is MyFloatingObject || this.CubeGrid.IsStatic)
        otherEntity.WorldMatrix = MatrixD.Multiply(MatrixD.Invert((MatrixD) state.MasterToSlave.Value), this.CubeGrid.WorldMatrix);
      else
        this.CubeGrid.WorldMatrix = MatrixD.Multiply((MatrixD) state.MasterToSlave.Value, otherEntity.WorldMatrix);
      this.Attach(otherEntity, state.GearPivotPosition.Value, state.OtherPivot.Value.Matrix);
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.CubeGrid.OnGridSplit -= new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      this.CubeGrid.OnIsStaticChanged -= new Action<bool>(this.CubeGrid_OnIsStaticChanged);
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      if (this.m_attachedState.Value.OtherEntityId.HasValue)
      {
        if (this.CubeGrid.Physics == null)
        {
          this.m_needsToRetryLock = true;
        }
        else
        {
          this.RetryLockServer();
          MyLandingGear.State state = this.m_attachedState.Value;
          state.Force = true;
          this.m_attachedState.Value = state;
        }
      }
      this.CubeGrid.OnGridSplit += new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      this.CubeGrid.OnIsStaticChanged += new Action<bool>(this.CubeGrid_OnIsStaticChanged);
    }

    protected void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
    {
      if (!this.IsLocked)
        return;
      this.ResetLockConstraint(this.IsLocked, true);
    }

    protected void CubeGrid_OnIsStaticChanged(bool isStatic)
    {
      if (isStatic || !(this.m_attachedTo is MyVoxelBase))
        return;
      this.ResetLockConstraint(false, true);
      this.m_needsToRetryLock = true;
    }

    public VRage.ModAPI.IMyEntity GetAttachedEntity() => this.m_attachedTo;

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.LoadDummies();
    }

    public override void OnTeleport()
    {
      base.OnTeleport();
      if (!(this.m_attachedTo is MyVoxelBase))
        return;
      this.Detach();
    }

    private LockModeChangedHandler GetDelegate(
      Action<SpaceEngineers.Game.ModAPI.IMyLandingGear, LandingGearMode> value)
    {
      return (LockModeChangedHandler) Delegate.CreateDelegate(typeof (LockModeChangedHandler), value.Target, value.Method);
    }

    event Action<SpaceEngineers.Game.ModAPI.IMyLandingGear, LandingGearMode> SpaceEngineers.Game.ModAPI.IMyLandingGear.LockModeChanged
    {
      add => this.LockModeChanged += this.GetDelegate(value);
      remove => this.LockModeChanged -= this.GetDelegate(value);
    }

    event Action<bool> SpaceEngineers.Game.ModAPI.IMyLandingGear.StateChanged
    {
      add => this.StateChanged += value;
      remove => this.StateChanged -= value;
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.IsBreakable => this.IsBreakable;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.IsLocked => this.IsLocked;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.AutoLock
    {
      get => this.AutoLock;
      set => this.AutoLock = value;
    }

    LandingGearMode SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.LockMode => this.LockMode;

    public MyTrailProperties LastTrail { get; set; }

    void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.ToggleLock() => this.RequestLandingGearSwitch();

    void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.Lock() => this.RequestLandingGearLock();

    void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.Unlock() => this.RequestLandingGearUnlock();

    void SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear.ResetAutoLock()
    {
      if (!this.CanAutoLock)
        return;
      this.ResetAutolock();
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
      MyHitInfo hitInfo = new MyHitInfo()
      {
        Position = position,
        Normal = (Vector3) normal
      };
      MyDecals.HandleAddDecal((VRage.ModAPI.IMyEntity) Sandbox.Game.Entities.MyEntities.GetEntityById(entityId), hitInfo, (Vector3) forwardDirection, physicalMaterial, this.BlockDefinition.Id.SubtypeId, damage: 30f, voxelMaterial: voxelMaterial);
    }

    [Serializable]
    protected struct State
    {
      public bool Force;
      public long? OtherEntityId;
      public MyDeltaTransform? MasterToSlave;
      public Vector3? GearPivotPosition;
      public CompressedPositionOrientation? OtherPivot;

      protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear\u003C\u003EState\u003C\u003EForce\u003C\u003EAccessor : IMemberAccessor<MyLandingGear.State, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLandingGear.State owner, in bool value) => owner.Force = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLandingGear.State owner, out bool value) => value = owner.Force;
      }

      protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear\u003C\u003EState\u003C\u003EOtherEntityId\u003C\u003EAccessor : IMemberAccessor<MyLandingGear.State, long?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLandingGear.State owner, in long? value) => owner.OtherEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLandingGear.State owner, out long? value) => value = owner.OtherEntityId;
      }

      protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear\u003C\u003EState\u003C\u003EMasterToSlave\u003C\u003EAccessor : IMemberAccessor<MyLandingGear.State, MyDeltaTransform?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLandingGear.State owner, in MyDeltaTransform? value) => owner.MasterToSlave = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLandingGear.State owner, out MyDeltaTransform? value) => value = owner.MasterToSlave;
      }

      protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear\u003C\u003EState\u003C\u003EGearPivotPosition\u003C\u003EAccessor : IMemberAccessor<MyLandingGear.State, Vector3?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLandingGear.State owner, in Vector3? value) => owner.GearPivotPosition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLandingGear.State owner, out Vector3? value) => value = owner.GearPivotPosition;
      }

      protected class SpaceEngineers_Game_Entities_Blocks_MyLandingGear\u003C\u003EState\u003C\u003EOtherPivot\u003C\u003EAccessor : IMemberAccessor<MyLandingGear.State, CompressedPositionOrientation?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyLandingGear.State owner,
          in CompressedPositionOrientation? value)
        {
          owner.OtherPivot = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyLandingGear.State owner,
          out CompressedPositionOrientation? value)
        {
          value = owner.OtherPivot;
        }
      }
    }

    protected sealed class AttachFailed\u003C\u003E : ICallSite<MyLandingGear, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLandingGear @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AttachFailed();
      }
    }

    protected sealed class ResetLockConstraint\u003C\u003ESystem_Boolean\u0023System_Boolean : ICallSite<MyLandingGear, bool, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLandingGear @this,
        in bool locked,
        in bool force,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ResetLockConstraint(locked, force);
      }
    }

    protected sealed class AttachRequest\u003C\u003ESystem_Boolean : ICallSite<MyLandingGear, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLandingGear @this,
        in bool enable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AttachRequest(enable);
      }
    }

    protected class m_lockModeSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<LandingGearMode, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<LandingGearMode, SyncDirection.FromServer>(obj1, obj2));
        ((MyLandingGear) obj0).m_lockModeSync = (VRage.Sync.Sync<LandingGearMode, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_autoLock\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyLandingGear) obj0).m_autoLock = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_attachedState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyLandingGear.State, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyLandingGear.State, SyncDirection.FromServer>(obj1, obj2));
        ((MyLandingGear) obj0).m_attachedState = (VRage.Sync.Sync<MyLandingGear.State, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_breakForceSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLandingGear) obj0).m_breakForceSync = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
