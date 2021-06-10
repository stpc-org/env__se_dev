// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyLargeTurretBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Utils;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_TurretBase))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyLargeTurretBase), typeof (Sandbox.ModAPI.Ingame.IMyLargeTurretBase)})]
  public abstract class MyLargeTurretBase : MyUserControllableGun, IMyInventoryOwner, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyUsableEntity, IMyGunBaseUser, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController
  {
    private static uint TIMER_NORMAL_IN_FRAMES = 10;
    private static uint TIMER_TIER1_IN_FRAMES = 0;
    public static float GAMEPAD_ZOOM_SPEED = 0.02f;
    public static float EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE = 0.85f;
    public static bool DEBUG_DRAW_TARGET_PREDICTION = false;
    [ThreadStatic]
    private static List<MyLargeTurretBase.MyEntityWithDistSq> m_allPotentialTargets;
    private static readonly MyStringId ID_WEAPON_LASER = MyStringId.GetOrCompute("WeaponLaser");
    private const int NotVisibleCachePeriod = 4;
    private const int LongTimeNotVisibleCachePeriod = 12;
    private readonly int m_VisibleCachePeriod = 3;
    private bool m_workingFlagCombination = true;
    private bool m_isInRandomRotationDistance;
    private static readonly HashSet<MyEntity> m_allAimedTargets = new HashSet<MyEntity>();
    private MyReplicationServer m_replicableServer;
    private IMyReplicable m_blocksReplicable;
    private MyParallelUpdateFlag m_parallelFlag;
    private float m_targetingTimeInFrames = 100f;
    private MyLargeTurretBase.MyTargetSelectionWorkData m_targetSelectionWorkData = new MyLargeTurretBase.MyTargetSelectionWorkData();
    private bool m_parallelTargetSelectionInProcess;
    private Task m_findNearTargetsTask;
    private bool m_cancelTargetSelection;
    private readonly FastResourceLock m_targetSelectionLock = new FastResourceLock();
    private MyEntity3DSoundEmitter m_soundEmitterForRotation;
    private int m_notVisibleTargetsUpdatesSinceRefresh;
    private ConcurrentDictionary<MyEntity, int> m_notVisibleTargets = new ConcurrentDictionary<MyEntity, int>();
    private ConcurrentDictionary<MyEntity, int> m_lastNotVisibleTargets = new ConcurrentDictionary<MyEntity, int>();
    private ConcurrentDictionary<MyEntity, int> m_visibleTargets = new ConcurrentDictionary<MyEntity, int>();
    private ConcurrentDictionary<MyEntity, int> m_lastVisibleTargets = new ConcurrentDictionary<MyEntity, int>();
    private bool m_hidetoolbar;
    private MyToolbar m_toolbar;
    private bool m_playAimingSound;
    public const float MAX_DISTANCE_FOR_RANDOM_ROTATING_LARGESHIP_GUNS = 600f;
    private const float DEFAULT_MIN_RANGE = 4f;
    private const float DEFAULT_MAX_RANGE = 800f;
    private static readonly float ROTATION_MULTIPLIER_NORMAL = 1f;
    private static readonly float ROTATION_MULTIPLIER_ZOOMED = 0.3f;
    private const float MIN_FOV = 1E-05f;
    private const float MAX_FOV = 3.124139f;
    private static float m_minFov;
    private static float m_maxFov;
    private readonly VRage.Sync.Sync<int, SyncDirection.FromServer> m_lateStartRandom;
    protected MyLargeBarrelBase m_barrel;
    protected MyEntity m_base1;
    protected MyEntity m_base2;
    private MyLargeTurretBase.MyLargeShipGunStatus m_status;
    private float m_rotation;
    private float m_elevation;
    private bool m_isMoving = true;
    private bool m_transformDirty = true;
    private float m_rotationLast;
    private float m_elevationLast;
    protected float m_rotationSpeed;
    protected float m_elevationSpeed;
    protected int m_randomStandbyChange_ms;
    protected int m_randomStandbyChangeConst_ms;
    private float m_randomStandbyRotation;
    private float m_randomStandbyElevation;
    private bool m_randomIsMoving;
    private double m_laserLength = 100.0;
    private int m_shootDelayIntervalConst_ms;
    private int m_shootIntervalConst_ms;
    private int m_shootStatusChanged_ms;
    private int m_shootDelayInterval_ms;
    private int m_shootInterval_ms;
    private int m_shootIntervalVarianceConst_ms;
    private float m_requiredPowerInput;
    private bool m_isPlayerShooting;
    private Sandbox.Game.Entities.IMyControllableEntity m_previousControlledEntity;
    private long? m_savedPreviousControlledEntityId;
    private MyCharacter m_cockpitPilot;
    private MyHudNotification m_outOfAmmoNotification;
    private float m_fov;
    private float m_targetFov;
    private bool m_gunIdleElevationAzimuthUnknown = true;
    private float m_gunIdleElevation;
    private float m_gunIdleAzimuth;
    private MyEntity m_target;
    private bool m_isPotentialTarget;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_shootingRange;
    private float m_searchingRange = 800f;
    private bool m_checkOtherTargets = true;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_enableIdleRotation;
    private bool m_previousIdleRotationState = true;
    private MyDefinitionId m_defId;
    protected MySoundPair m_shootingCueEnum = new MySoundPair();
    protected MySoundPair m_rotatingCueEnum = new MySoundPair();
    protected Vector3D m_hitPosition;
    protected MyGunBase m_gunBase;
    private long m_targetToSet;
    private MyLargeTurretBase.IMyPredicionType m_currentPrediction;
    private MyLargeTurretBase.IMyPredicionType m_targetNoPrediction = (MyLargeTurretBase.IMyPredicionType) new MyLargeTurretBase.MyTargetNoPredictionType();
    private MyLargeTurretBase.IMyPredicionType m_positionNoPrediction = (MyLargeTurretBase.IMyPredicionType) new MyLargeTurretBase.MyPositionNoPredictionType();
    private readonly FastResourceLock m_currentPredictionUsageLock = new FastResourceLock();
    private MyLargeTurretBase.IMyPredicionType m_targetPrediction;
    private MyLargeTurretBase.IMyPredicionType m_positionPrediction;
    private float m_minElevationRadians;
    private float m_maxElevationRadians = 6.283185f;
    private float m_minSinElevationRadians = -1f;
    private float m_maxSinElevationRadians = 1f;
    private float m_minAzimuthRadians;
    private float m_maxAzimuthRadians = 6.283185f;
    private float m_minRangeMeter = 4f;
    private float m_maxRangeMeter = 800f;
    protected bool m_isControlled;
    private MyEntity[] m_shootIgnoreEntities;
    private readonly VRage.Sync.Sync<MyLargeTurretBase.SyncRotationAndElevation, SyncDirection.BothWays> m_rotationAndElevationSync;
    private readonly VRage.Sync.Sync<MyLargeTurretBase.CurrentTargetSync, SyncDirection.FromServer> m_targetSync;
    private readonly VRage.Sync.Sync<MyTurretTargetFlags, SyncDirection.BothWays> m_targetFlags;
    private bool m_isAimed;
    private HashSet<VRage.ModAPI.IMyEntity> m_children = new HashSet<VRage.ModAPI.IMyEntity>();
    private VRage.Sync.Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;
    private MatrixD m_lastTestedGridWM;
    private bool m_canStopShooting;
    private float m_stopShootingTime;
    private float[] m_distanceEntityKeys;
    private MyShipController m_controller;
    private MyGridClientState m_lastNetMoveState;
    private bool m_lastNetCanControl;
    private bool m_lastNetRotateShip;
    private MyControllerInfo m_controllerInfo = new MyControllerInfo();

    public int LateStartRandom => this.m_lateStartRandom.Value;

    private float Rotation
    {
      get => this.m_rotation;
      set
      {
        if ((double) this.m_rotation == (double) value)
          return;
        this.m_rotation = value;
        this.m_transformDirty = true;
      }
    }

    private float Elevation
    {
      get => this.m_elevation;
      set
      {
        if ((double) this.m_elevation == (double) value)
          return;
        this.m_elevation = value;
        this.m_transformDirty = true;
      }
    }

    public MyEntity WeaponOwner { get; set; }

    public MatrixD InitializationMatrix { get; private set; }

    public MatrixD InitializationBarrelMatrix { get; set; }

    public MyEntity Target => this.m_target;

    public bool IsAimed
    {
      get => this.m_isAimed;
      protected set
      {
        if (this.m_isAimed == value)
          return;
        this.m_isAimed = value;
      }
    }

    public MyLargeTurretBase.MyLargeShipGunStatus GetStatus() => this.m_status;

    public Sandbox.Game.Entities.IMyControllableEntity PreviousControlledEntity
    {
      get
      {
        if (this.m_savedPreviousControlledEntityId.HasValue && this.TryFindSavedEntity())
        {
          this.m_savedPreviousControlledEntityId = new long?();
          this.SetCameraOverlay();
        }
        return this.m_previousControlledEntity;
      }
      private set
      {
        if (value == this.m_previousControlledEntity)
          return;
        if (this.m_previousControlledEntity != null)
        {
          this.m_previousControlledEntity.Entity.OnMarkForClose -= new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
          if (this.m_cockpitPilot != null)
            this.m_cockpitPilot.OnMarkForClose -= new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
        }
        this.m_previousControlledEntity = value;
        if (this.m_previousControlledEntity == null)
          return;
        this.AddPreviousControllerEvents();
        if (!(this.PreviousControlledEntity is MyCockpit))
          return;
        this.m_cockpitPilot = (this.PreviousControlledEntity as MyCockpit).Pilot;
        if (this.m_cockpitPilot == null)
          return;
        this.m_cockpitPilot.OnMarkForClose += new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
      }
    }

    public MyModelDummy CameraDummy { get; private set; }

    public MyLargeTurretBaseDefinition BlockDefinition => base.BlockDefinition as MyLargeTurretBaseDefinition;

    private bool AiEnabled => this.BlockDefinition == null || this.BlockDefinition.AiEnabled;

    public bool IsControlled => this.PreviousControlledEntity != null || this.m_isControlled;

    public bool IsPlayerControlled => this.IsControlled || Sandbox.Game.Multiplayer.Sync.Players.GetControllingPlayer((MyEntity) this) != null;

    public bool IsControlledByLocalPlayer => this.IsControlled && this.ControllerInfo.Controller != null && this.ControllerInfo.IsLocallyControlled();

    public MyCharacter Pilot => this.PreviousControlledEntity is MyCharacter controlledEntity ? controlledEntity : this.m_cockpitPilot;

    protected abstract float ForwardCameraOffset { get; }

    protected abstract float UpCameraOffset { get; }

    public MyLargeBarrelBase Barrel => this.m_barrel;

    public MyGunBase GunBase => this.m_gunBase;

    private bool EnableIdleRotation
    {
      get => (bool) this.m_enableIdleRotation;
      set => this.m_enableIdleRotation.Value = value;
    }

    public MyTurretTargetFlags TargetFlags
    {
      get => (MyTurretTargetFlags) this.m_targetFlags;
      set => this.m_targetFlags.Value = value;
    }

    public bool IsSkinnable => false;

    public override bool IsTieredUpdateSupported => true;

    public MyLargeTurretBase()
    {
      this.m_shootIgnoreEntities = new MyEntity[1]
      {
        (MyEntity) this
      };
      this.CreateTerminalControls();
      this.m_status = MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated;
      this.m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(3500, 4500);
      this.m_randomStandbyRotation = 0.0f;
      this.m_randomStandbyElevation = 0.0f;
      this.Rotation = 0.0f;
      this.Elevation = 0.0f;
      this.m_rotationSpeed = 0.005f;
      this.m_elevationSpeed = 0.005f;
      this.m_shootDelayIntervalConst_ms = 200;
      this.m_shootIntervalConst_ms = 1200;
      this.m_shootIntervalVarianceConst_ms = 500;
      this.m_shootStatusChanged_ms = 0;
      this.m_isPotentialTarget = false;
      this.m_targetPrediction = (MyLargeTurretBase.IMyPredicionType) new MyLargeTurretBase.MyTargetPredictionType(this);
      this.m_currentPrediction = this.m_targetPrediction;
      this.m_positionPrediction = (MyLargeTurretBase.IMyPredicionType) new MyLargeTurretBase.MyPositionPredictionType(this);
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_soundEmitterForRotation = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.ControllerInfo.ControlReleased += new Action<MyEntityController>(this.OnControlReleased);
      this.m_gunBase = new MyGunBase();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_gunBase.OnAmmoAmountChanged += new Action(this.OnAmmoAmountChangedOnServer);
      else
        this.m_cachedAmmunitionAmount.ValueChanged += new Action<SyncBase>(this.OnAmmoAmountChangedFromServer);
      this.m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 1000, level: MyNotificationLevel.Important);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
        this.m_parallelFlag.Enable((MyEntity) this);
      this.SyncType.Append((object) this.m_gunBase);
      this.m_shootingRange.ValueChanged += (Action<SyncBase>) (x => this.ShootingRangeChanged());
      this.m_rotationAndElevationSync.ValueChanged += (Action<SyncBase>) (x => this.RotationAndElevationSync());
      this.m_targetSync.AlwaysReject<MyLargeTurretBase.CurrentTargetSync, SyncDirection.FromServer>();
      this.m_targetSync.ValueChanged += (Action<SyncBase>) (x => this.TargetChanged());
      this.m_toolbar = new MyToolbar(this.ToolbarType);
    }

    private void OnAmmoAmountChangedFromServer(SyncBase obj) => this.GunBase.CurrentAmmo = this.m_cachedAmmunitionAmount.Value;

    private void OnAmmoAmountChangedOnServer()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_cachedAmmunitionAmount.Value = this.GunBase.CurrentAmmo;
    }

    private void TargetChanged()
    {
      MyEntity entity = (MyEntity) null;
      if (this.m_targetSync.Value.TargetId != 0L)
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_targetSync.Value.TargetId, out entity);
      this.SetTarget(entity, this.m_targetSync.Value.IsPotential);
    }

    private void RotationAndElevationSync()
    {
      if (this.IsControlledByLocalPlayer)
        return;
      this.UpdateRotationAndElevation(this.m_rotationAndElevationSync.Value.Rotation, this.m_rotationAndElevationSync.Value.Elevation);
    }

    private void ShootingRangeChanged()
    {
      this.AdjustSearchingRange();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsWorking || !this.AiEnabled || !MySession.Static.CreativeMode && !this.HasEnoughAmmo())
        return;
      this.CheckAndSelectNearTargetsParallel();
    }

    private void AdjustSearchingRange() => this.m_searchingRange = (float) this.m_shootingRange + 200f;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyObjectBuilder_TurretBase self = (MyObjectBuilder_TurretBase) objectBuilder;
      MyWeaponBlockDefinition blockDefinition = base.BlockDefinition as MyWeaponBlockDefinition;
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = blockDefinition == null ? new MyInventory(0.384f, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive) : new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(self.Inventory);
      }
      this.m_gunBase.Init(self.GunBase, base.BlockDefinition, (IMyGunBaseUser) this);
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, 1f / 500f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_lateStartRandom.Value = MyUtils.GetRandomInt(0, 30);
        this.LateStartRandom_ValueChanged((SyncBase) null);
      }
      this.m_lateStartRandom.ValueChanged += new Action<SyncBase>(this.LateStartRandom_ValueChanged);
      this.InitializationMatrix = (MatrixD) ref this.PositionComp.LocalMatrixRef;
      this.InitializationBarrelMatrix = MatrixD.Identity;
      this.m_defId = self.GetId();
      this.m_shootingRange.SetLocalValue(Math.Min(this.BlockDefinition.MaxRangeMeters, Math.Max(0.0f, self.Range)));
      this.AdjustSearchingRange();
      this.TargetMeteors = self.TargetMeteors;
      this.TargetMissiles = self.TargetMissiles;
      this.TargetCharacters = self.TargetCharacters;
      this.TargetSmallGrids = self.TargetSmallGrids;
      this.TargetLargeGrids = self.TargetLargeGrids;
      this.TargetStations = self.TargetStations;
      this.TargetNeutrals = self.TargetNeutrals;
      this.OnTargetFlagChanged();
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.m_targetToSet = self.Target;
      this.m_isPotentialTarget = self.IsPotentialTarget;
      this.m_savedPreviousControlledEntityId = self.PreviousControlledEntityId;
      this.Rotation = self.Rotation;
      this.Elevation = self.Elevation;
      this.m_isPlayerShooting = self.IsShooting;
      this.Render.NeedsDrawFromParent = this.m_isPlayerShooting;
      if (this.BlockDefinition != null)
      {
        this.m_maxRangeMeter = this.BlockDefinition.MaxRangeMeters;
        this.m_minElevationRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MinElevationDegrees));
        this.m_maxElevationRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MaxElevationDegrees));
        if ((double) this.m_minElevationRadians > (double) this.m_maxElevationRadians)
          this.m_minElevationRadians -= 6.283185f;
        this.m_minSinElevationRadians = (float) Math.Sin((double) this.m_minElevationRadians);
        this.m_maxSinElevationRadians = (float) Math.Sin((double) this.m_maxElevationRadians);
        this.m_minAzimuthRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MinAzimuthDegrees));
        this.m_maxAzimuthRadians = MathHelper.ToRadians(this.NormalizeAngle(this.BlockDefinition.MaxAzimuthDegrees));
        if ((double) this.m_minAzimuthRadians > (double) this.m_maxAzimuthRadians)
          this.m_minAzimuthRadians -= 6.283185f;
        this.m_rotationSpeed = this.BlockDefinition.RotationSpeed;
        this.m_elevationSpeed = this.BlockDefinition.ElevationSpeed;
        this.m_enableIdleRotation.Value = this.BlockDefinition.IdleRotation;
        this.ClampRotationAndElevation();
      }
      this.m_enableIdleRotation.Value &= self.EnableIdleRotation;
      this.m_previousIdleRotationState = self.PreviousIdleRotationState;
      this.m_gunIdleElevationAzimuthUnknown = true;
      MyLargeTurretBase.m_minFov = this.BlockDefinition.MinFov;
      MyLargeTurretBase.m_maxFov = this.BlockDefinition.MaxFov;
      this.m_fov = this.BlockDefinition.MaxFov;
      this.m_targetFov = this.BlockDefinition.MaxFov;
      this.m_targetFlags.ValueChanged += (Action<SyncBase>) (x => this.OnTargetFlagChanged());
      this.InvalidateOnMove = false;
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame10);
    }

    private void LateStartRandom_ValueChanged(SyncBase obj)
    {
      if (this.m_barrel == null)
        return;
      this.m_barrel.LateTimeRandom = this.m_lateStartRandom.Value;
    }

    private float NormalizeAngle(int angle)
    {
      int num = angle % 360;
      return num == 0 && angle != 0 ? 360f : (float) num;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_TurretBase builderCubeBlock = (MyObjectBuilder_TurretBase) base.GetObjectBuilderCubeBlock(copy);
      MyObjectBuilder_UserControllableGun userControllableGun = (MyObjectBuilder_UserControllableGun) builderCubeBlock;
      if (userControllableGun != null)
        userControllableGun.IsLargeTurret = true;
      builderCubeBlock.Range = (float) this.m_shootingRange;
      builderCubeBlock.RemainingAmmo = this.m_gunBase.CurrentAmmo;
      builderCubeBlock.Target = this.Target != null ? this.Target.EntityId : 0L;
      builderCubeBlock.IsPotentialTarget = this.m_isPotentialTarget;
      builderCubeBlock.TargetMeteors = this.TargetMeteors;
      builderCubeBlock.TargetMissiles = this.TargetMissiles;
      builderCubeBlock.EnableIdleRotation = this.EnableIdleRotation;
      builderCubeBlock.TargetCharacters = this.TargetCharacters;
      builderCubeBlock.TargetSmallGrids = this.TargetSmallGrids;
      builderCubeBlock.TargetLargeGrids = this.TargetLargeGrids;
      builderCubeBlock.TargetStations = this.TargetStations;
      builderCubeBlock.TargetNeutrals = this.TargetNeutrals;
      if (this.PreviousControlledEntity != null)
      {
        builderCubeBlock.PreviousControlledEntityId = new long?(this.PreviousControlledEntity.Entity.EntityId);
        builderCubeBlock.Rotation = this.Rotation;
        builderCubeBlock.Elevation = this.Elevation;
        builderCubeBlock.IsShooting = this.m_isPlayerShooting;
      }
      builderCubeBlock.GunBase = this.m_gunBase.GetObjectBuilder();
      builderCubeBlock.PreviousIdleRotationState = this.m_previousIdleRotationState;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private MatrixD InitializationMatrixWorld => this.InitializationMatrix * this.Parent.WorldMatrix;

    protected override void Closing()
    {
      base.Closing();
      if (this.m_lateStartRandom != null)
        this.m_lateStartRandom.ValueChanged -= new Action<SyncBase>(this.LateStartRandom_ValueChanged);
      try
      {
        this.m_findNearTargetsTask.WaitOrExecute();
      }
      catch
      {
      }
      this.ResourceSink.IsPoweredChanged -= new Action(this.Receiver_IsPoweredChanged);
      this.ReleaseControl();
      this.ResetTarget();
      if (this.m_barrel != null)
      {
        this.m_barrel.Close();
        this.m_barrel = (MyLargeBarrelBase) null;
      }
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      this.m_soundEmitterForRotation.StopSound(true);
    }

    protected override bool CheckIsWorking() => this.ResourceSink != null && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.StopShootingSound();
      this.StopAimingSound();
      if (this.m_barrel != null)
        this.m_barrel.RemoveSmoke();
      if (this.IsControlled)
        this.ReleaseControl();
      this.ResetTarget();
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      if (this.IsWorking)
        this.m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      else
        this.OnStopWorking();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.m_targetToSet != 0L && this.IsWorking)
      {
        MyEntity entity = (MyEntity) null;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_targetToSet, out entity))
          this.ResetTarget();
      }
      if (this.m_savedPreviousControlledEntityId.HasValue)
      {
        MySession.Static.Players.UpdatePlayerControllers(this.EntityId);
        if (this.m_savedPreviousControlledEntityId.HasValue)
        {
          this.TryFindSavedEntity();
          this.m_savedPreviousControlledEntityId = new long?();
        }
      }
      MyCubeGrid cubeGrid = this.CubeGrid;
      MyCubeGridRenderCell orAddCell = cubeGrid.RenderData.GetOrAddCell(this.Position * cubeGrid.GridSize);
      if (orAddCell.ParentCullObject == uint.MaxValue)
        orAddCell.RebuildInstanceParts(cubeGrid.Render.GetRenderFlags());
      if (this.m_base1 != null)
      {
        this.m_base1.Render.SetParent(0, orAddCell.ParentCullObject);
        this.m_base1.NeedsWorldMatrix = false;
        this.m_base1.InvalidateOnMove = false;
      }
      if (this.m_base2 != null)
      {
        this.m_base2.Render.SetParent(0, orAddCell.ParentCullObject);
        this.m_base2.NeedsWorldMatrix = false;
        this.m_base2.InvalidateOnMove = false;
      }
      this.RotateModels();
    }

    private bool TryFindSavedEntity()
    {
      MyEntity entity;
      if (this.ControllerInfo.Controller == null || !Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_savedPreviousControlledEntityId.Value, out entity))
        return false;
      this.PreviousControlledEntity = (Sandbox.Game.Entities.IMyControllableEntity) entity;
      if (this.m_previousControlledEntity is MyCockpit)
        this.m_cockpitPilot = (this.m_previousControlledEntity as MyCockpit).Pilot;
      this.RotateModels();
      return true;
    }

    private bool NeedsPerFrameUpdate
    {
      get
      {
        if (!MyFakes.OPTIMIZE_GRID_UPDATES)
          return true;
        double randomStandbyRotation = (double) this.m_randomStandbyRotation;
        float standbyElevation = this.m_randomStandbyElevation;
        double rotation = (double) this.Rotation;
        float num1 = (float) (randomStandbyRotation - rotation);
        float num2 = standbyElevation - this.Elevation;
        return this.IsControlledByLocalPlayer && MySession.Static.CameraController == this || (this.m_transformDirty || this.m_isMoving) || (this.m_barrel != null && this.m_barrel.NeedsPerFrameUpdate || (this.m_isPlayerShooting || (double) num1 * (double) num1 > 9.99999943962493E-11)) || (double) num2 * (double) num2 > 9.99999943962493E-11 || (bool) this.m_forceShoot;
      }
    }

    public override sealed void UpdateBeforeSimulation() => base.UpdateBeforeSimulation();

    public virtual void UpdateBeforeSimulationParallel()
    {
      if (!this.IsControlledByLocalPlayer || MySession.Static.CameraController != this)
        return;
      if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
        this.ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MyStringId.NullOrEmpty;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_IN, MyControlStateType.PRESSED))
      {
        this.ChangeZoomPrecise(-MyLargeTurretBase.GAMEPAD_ZOOM_SPEED);
      }
      else
      {
        if (!MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_OUT, MyControlStateType.PRESSED))
          return;
        this.ChangeZoomPrecise(MyLargeTurretBase.GAMEPAD_ZOOM_SPEED);
      }
    }

    public virtual void UpdateAfterSimulationParallel()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      this.RotateModels();
    }

    public override sealed void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.m_barrel?.UpdateAfterSimulation();
      if (!this.IsWorking || this.Parent.Physics == null || !this.Parent.Physics.Enabled)
        return;
      if (this.IsControlledByLocalPlayer && MySession.Static.CameraController == this)
      {
        this.m_fov = MathHelper.Lerp(this.m_fov, this.m_targetFov, 0.5f);
        MyLargeTurretBase.SetFov(this.m_fov);
      }
      bool flag = this.Render.IsVisible();
      if (flag && this.IsWorking && this.m_barrel != null)
      {
        if (!this.IsPlayerControlled && this.AiEnabled)
          this.UpdateAiWeapon();
        else if (this.m_isPlayerShooting)
        {
          MyGunStatusEnum status;
          if (this.CanShoot(out status))
            this.UpdateShooting(this.m_isPlayerShooting);
          else if (status == MyGunStatusEnum.OutOfAmmo && this.m_gunBase.SwitchAmmoMagazineToFirstAvailable())
            status = MyGunStatusEnum.OK;
          if (this.IsControlledByLocalPlayer && status == MyGunStatusEnum.OutOfAmmo)
          {
            this.m_outOfAmmoNotification.SetTextFormatArguments((object) this.DisplayNameText);
            MyHud.Notifications.Add((MyHudNotificationBase) this.m_outOfAmmoNotification);
          }
        }
      }
      if (!(bool) this.m_isShooting && !(bool) this.m_forceShoot && this.m_barrel != null)
        this.m_barrel.ResetCurrentLateStart();
      if (!flag || !(bool) this.m_isShooting && !this.m_isPlayerShooting && (this.IsPlayerControlled || !this.AiEnabled))
        this.StopShootingSound();
      if (this.NeedsPerFrameUpdate)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    private void UpdateVisibilityCacheCounters()
    {
      this.m_lastNotVisibleTargets.Clear();
      foreach (KeyValuePair<MyEntity, int> notVisibleTarget in this.m_notVisibleTargets)
      {
        int num = notVisibleTarget.Value - this.m_notVisibleTargetsUpdatesSinceRefresh;
        if (num > 0)
          this.m_lastNotVisibleTargets[notVisibleTarget.Key] = num;
      }
      ConcurrentDictionary<MyEntity, int> notVisibleTargets = this.m_notVisibleTargets;
      this.m_notVisibleTargets = this.m_lastNotVisibleTargets;
      this.m_lastNotVisibleTargets = notVisibleTargets;
      this.m_notVisibleTargetsUpdatesSinceRefresh = 0;
      this.m_lastVisibleTargets.Clear();
      foreach (KeyValuePair<MyEntity, int> visibleTarget in this.m_visibleTargets)
      {
        int num = visibleTarget.Value - 1;
        if (num > 0)
          this.m_lastVisibleTargets.TryAdd(visibleTarget.Key, num);
      }
      ConcurrentDictionary<MyEntity, int> visibleTargets = this.m_visibleTargets;
      this.m_visibleTargets = this.m_lastVisibleTargets;
      this.m_lastVisibleTargets = visibleTargets;
    }

    private void UpdateAiWeapon()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      double targetDistanceSquared = this.GetTargetDistanceSquared();
      MyGunStatusEnum status = MyGunStatusEnum.Cooldown;
      double num = (double) this.m_searchingRange * (double) this.m_searchingRange;
      if (targetDistanceSquared < num || this.m_currentPrediction.ManualTargetPosition)
      {
        bool flag1 = (this.Target != null || this.m_currentPrediction.ManualTargetPosition) && (this.IsTargetVisible(this.Target) || this.m_currentPrediction.ManualTargetPosition) && this.RotationAndElevation() && this.CanShoot(out status) && !this.m_currentPrediction.ManualTargetPosition;
        bool flag2 = this.IsTargetEnemy(this.Target);
        this.UpdateShooting(((!flag1 || this.m_isPotentialTarget ? 0 : (this.IsTarget(this.Target) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0);
        if (flag2)
          return;
        this.ResetTarget();
      }
      else
      {
        if (!(bool) this.m_isShooting)
          this.Deactivate();
        else
          this.UpdateShooting(!this.m_isPotentialTarget);
        if (this.m_isInRandomRotationDistance)
        {
          this.ResetRandomAiming();
          this.RandomMovement();
        }
        else
          this.StopAimingSound();
      }
    }

    private void UpdateShooting(bool shouldShoot)
    {
      if (shouldShoot)
      {
        this.UpdateShootStatus();
        if (this.m_status == MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting)
        {
          this.m_canStopShooting = this.m_barrel.StartShooting() && this.m_soundEmitter != null && this.m_soundEmitter.Loop;
        }
        else
        {
          if (this.m_status == MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying || !this.m_canStopShooting && (this.m_soundEmitter == null || this.m_soundEmitter.Sound == null || (!this.m_soundEmitter.Sound.IsPlaying || !this.m_soundEmitter.Loop)))
            return;
          this.m_barrel.StopShooting();
          this.m_canStopShooting = false;
        }
      }
      else
      {
        this.m_status = MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Searching;
        this.UpdateShootStatus();
        if (!this.m_canStopShooting && (this.m_soundEmitter == null || this.m_soundEmitter.Sound == null || (!this.m_soundEmitter.Sound.IsPlaying || !this.m_soundEmitter.Loop)))
          return;
        this.m_barrel.StopShooting();
        this.m_canStopShooting = false;
      }
    }

    private void Deactivate()
    {
      this.m_status = MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated;
      if (this.m_soundEmitter == null || !this.m_canStopShooting && (this.m_soundEmitter.Sound == null || !this.m_soundEmitter.Sound.IsPlaying || !this.m_soundEmitter.Loop))
        return;
      this.m_barrel.StopShooting();
      this.m_canStopShooting = false;
    }

    private void SetShootInterval(ref int shootInterval, ref int shootIntervalConst) => shootInterval = shootIntervalConst;

    private void UpdateShootStatus()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      switch (this.m_status)
      {
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Searching:
          this.StartShootDelaying();
          this.m_isShooting.Value = false;
          break;
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting:
          if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_shootStatusChanged_ms <= this.m_shootInterval_ms)
            break;
          this.StartShootDelaying();
          this.m_isShooting.Value = true;
          break;
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying:
          if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_shootStatusChanged_ms <= this.m_shootDelayInterval_ms)
            break;
          this.StartShooting();
          this.m_isShooting.Value = true;
          break;
      }
    }

    private void StartShooting()
    {
      this.m_status = MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting;
      this.m_shootStatusChanged_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.SetShootInterval(ref this.m_shootInterval_ms, ref this.m_shootIntervalConst_ms);
    }

    private void StartShootDelaying()
    {
      this.m_status = MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying;
      this.m_shootStatusChanged_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_shootDelayIntervalConst_ms = 0;
      this.SetShootInterval(ref this.m_shootDelayInterval_ms, ref this.m_shootDelayIntervalConst_ms);
    }

    private void ResetRandomAiming()
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_randomStandbyChange_ms <= this.m_randomStandbyChangeConst_ms)
        return;
      this.m_randomStandbyRotation = MyUtils.GetRandomFloat(-3.141593f, 3.141593f);
      this.m_randomStandbyElevation = MyUtils.GetRandomFloat(0.0f, 1.570796f);
      this.m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void RandomMovement()
    {
      if (this.m_barrel == null || !(bool) this.m_enableIdleRotation || (bool) this.m_forceShoot)
        return;
      double randomStandbyRotation = (double) this.m_randomStandbyRotation;
      float standbyElevation = this.m_randomStandbyElevation;
      float max1 = this.m_rotationSpeed * 16f;
      bool flag = false;
      double rotation = (double) this.Rotation;
      float num1 = (float) (randomStandbyRotation - rotation);
      if ((double) num1 * (double) num1 > 9.99999943962493E-11)
      {
        this.Rotation += MathHelper.Clamp(num1, -max1, max1);
        flag = true;
      }
      if ((double) standbyElevation > (double) this.m_barrel.BarrelElevationMin)
      {
        float max2 = this.m_elevationSpeed * 16f;
        float num2 = standbyElevation - this.Elevation;
        if ((double) num2 * (double) num2 > 9.99999943962493E-11)
        {
          this.Elevation += MathHelper.Clamp(num2, -max2, max2);
          flag = true;
        }
      }
      this.m_playAimingSound = flag;
      this.ClampRotationAndElevation();
      if (this.m_randomIsMoving)
      {
        if (flag)
          return;
        this.SetupSearchRaycast();
        this.m_randomIsMoving = false;
      }
      else
      {
        if (!flag)
          return;
        this.m_randomIsMoving = true;
      }
    }

    private void SetupSearchRaycast()
    {
      MatrixD muzzleWorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
      Vector3D vector3D = muzzleWorldMatrix.Translation + muzzleWorldMatrix.Forward * (double) this.m_searchingRange;
      this.m_laserLength = (double) this.m_searchingRange;
      this.m_hitPosition = vector3D;
    }

    protected void GetCameraDummy()
    {
      if (this.m_base2.Model == null || !this.m_base2.Model.Dummies.ContainsKey("camera"))
        return;
      this.CameraDummy = this.m_base2.Model.Dummies["camera"];
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.m_transformDirty = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected override void RotateModels()
    {
      if (this.m_base1 == null || this.m_base2 == null || (this.m_barrel == null || !this.m_base1.Render.IsChild(0)))
        this.m_transformDirty = false;
      else if (this.m_transformDirty && !this.m_isMoving && (this.m_replicableServer != null && Sandbox.Game.Multiplayer.Sync.IsDedicated) && (!this.m_replicableServer.IsReplicated(this.m_blocksReplicable) && this.Target == null))
      {
        this.m_transformDirty = false;
      }
      else
      {
        this.ClampRotationAndElevation();
        Matrix result1;
        Matrix.CreateRotationY(this.Rotation, out result1);
        result1.Translation = this.m_base1.PositionComp.LocalMatrixRef.Translation;
        Matrix matrix2 = this.PositionComp.LocalMatrixRef;
        Matrix result2;
        Matrix.Multiply(ref result1, ref matrix2, out result2);
        this.m_base1.PositionComp.SetLocalMatrix(ref result1, (object) this.m_base1.Physics, false, ref result2, true);
        Matrix result3;
        Matrix.CreateRotationX(this.Elevation, out result3);
        result3.Translation = this.m_base2.PositionComp.LocalMatrixRef.Translation;
        Matrix result4;
        Matrix.Multiply(ref result3, ref result2, out result4);
        this.m_base2.PositionComp.SetLocalMatrix(ref result3, (object) this.m_base2.Physics, true, ref result4, true);
        this.m_barrel.WorldPositionChanged(ref result4);
        this.m_transformDirty = false;
      }
    }

    private Vector3 LookAt(Vector3D target)
    {
      Vector3D muzzleWorldPosition = this.m_gunBase.GetMuzzleWorldPosition();
      float azimuth;
      float elevation;
      Vector3.GetAzimuthAndElevation(Vector3.Normalize(Vector3D.TransformNormal(target - muzzleWorldPosition, this.PositionComp.WorldMatrixInvScaled)), out azimuth, out elevation);
      if (this.m_gunIdleElevationAzimuthUnknown)
      {
        Vector3.GetAzimuthAndElevation((Vector3) this.m_gunBase.GetMuzzleLocalMatrix().Forward, out this.m_gunIdleAzimuth, out this.m_gunIdleElevation);
        this.m_gunIdleElevationAzimuthUnknown = false;
      }
      return new Vector3(elevation - this.m_gunIdleElevation, MathHelper.WrapAngle(azimuth - this.m_gunIdleAzimuth), 0.0f);
    }

    protected void ResetRotation()
    {
      this.Rotation = 0.0f;
      this.Elevation = 0.0f;
      this.ClampRotationAndElevation();
      this.m_randomStandbyElevation = 0.0f;
      this.m_randomStandbyRotation = 0.0f;
      this.m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public bool RotationAndElevation()
    {
      Vector3 vector3 = Vector3.Zero;
      if (this.Target != null || this.m_currentPrediction.ManualTargetPosition)
      {
        Vector3D predictedTargetPosition;
        using (this.m_currentPredictionUsageLock.AcquireExclusiveUsing())
          predictedTargetPosition = this.m_currentPrediction.GetPredictedTargetPosition((VRage.ModAPI.IMyEntity) this.Target);
        vector3 = this.LookAt(predictedTargetPosition);
      }
      float y = vector3.Y;
      float x = vector3.X;
      float max1 = this.m_rotationSpeed * 16f;
      float num1 = MathHelper.WrapAngle(y - this.Rotation);
      this.Rotation += MathHelper.Clamp(num1, -max1, max1);
      bool flag = (double) num1 * (double) num1 > 9.99999943962493E-11;
      if ((double) this.Rotation > 3.14159297943115)
        this.Rotation -= 6.283185f;
      else if ((double) this.Rotation < -3.14159297943115)
        this.Rotation += 6.283185f;
      float max2 = this.m_elevationSpeed * 16f;
      float num2 = Math.Max(x, this.m_barrel.BarrelElevationMin) - this.Elevation;
      this.Elevation += MathHelper.Clamp(num2, -max2, max2);
      this.m_playAimingSound = flag || (double) num2 * (double) num2 > 9.99999943962493E-11;
      this.ClampRotationAndElevation();
      this.RotateModels();
      if (this.Target != null || this.m_currentPrediction.ManualTargetPosition)
      {
        float num3 = Math.Abs(y - this.Rotation);
        float num4 = Math.Abs(x - this.Elevation);
        this.IsAimed = (double) num3 <= 1.40129846432482E-45 && (double) num4 <= 0.00999999977648258;
      }
      else
        this.IsAimed = false;
      return this.IsAimed;
    }

    private void ClampRotationAndElevation()
    {
      this.Rotation = this.ClampRotation(this.Rotation);
      this.Elevation = this.ClampElevation(this.Elevation);
    }

    private float ClampRotation(float value)
    {
      if (this.IsRotationLimited())
        value = Math.Min(this.m_maxAzimuthRadians, Math.Max(this.m_minAzimuthRadians, value));
      return value;
    }

    private bool IsRotationLimited() => (double) Math.Abs((float) ((double) this.m_maxAzimuthRadians - (double) this.m_minAzimuthRadians - 6.28318548202515)) > 0.01;

    private float ClampElevation(float value)
    {
      if (this.IsElevationLimited())
        value = Math.Min(this.m_maxElevationRadians, Math.Max(this.m_minElevationRadians, value));
      return value;
    }

    private bool IsElevationLimited() => (double) Math.Abs((float) ((double) this.m_maxElevationRadians - (double) this.m_minElevationRadians - 6.28318548202515)) > 0.01;

    private void PlayAimingSound()
    {
      if (this.m_soundEmitterForRotation == null || this.m_soundEmitterForRotation.IsPlaying)
        return;
      this.m_soundEmitterForRotation.PlaySound(this.m_rotatingCueEnum, true);
    }

    public void PlayShootingSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.StopAimingSound();
      this.m_gunBase.StartShootSound(this.m_soundEmitter);
    }

    public void StopShootingSound()
    {
      if (this.m_soundEmitter == null || !(this.m_soundEmitter.SoundId == this.m_shootingCueEnum.Arcade) && !(this.m_soundEmitter.SoundId == this.m_shootingCueEnum.Realistic) || !this.m_soundEmitter.Loop)
        return;
      this.m_soundEmitter.StopSound(false);
    }

    internal void StopAimingSound()
    {
      if (this.m_soundEmitterForRotation == null || !(this.m_soundEmitterForRotation.SoundId == this.m_rotatingCueEnum.Arcade) && !(this.m_soundEmitterForRotation.SoundId == this.m_rotatingCueEnum.Realistic))
        return;
      this.m_soundEmitterForRotation.StopSound(false);
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      if (base.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES))
        return true;
      return this.m_barrel != null && this.m_barrel.Entity.GetIntersectionWithLine(ref line, out t);
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out Vector3D? v,
      bool useCollisionModel = true,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      return base.GetIntersectionWithLine(ref line, out v, useCollisionModel) || this.m_barrel.Entity.GetIntersectionWithLine(ref line, out v, useCollisionModel);
    }

    public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

    public float BackkickForcePerSecond => this.m_gunBase.BackkickForcePerSecond;

    public float ShakeAmount { get; protected set; }

    public override bool CanShoot(out MyGunStatusEnum status)
    {
      if (!this.m_gunBase.HasAmmoMagazines || this.IsControlledByLocalPlayer && MySession.Static.CameraController != this)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Shooting))
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!MySession.Static.CreativeMode && !this.HasEnoughAmmo())
      {
        status = MyGunStatusEnum.OutOfAmmo;
        this.m_gunBase.SwitchAmmoMagazineToFirstAvailable();
        return false;
      }
      status = MyGunStatusEnum.OK;
      return true;
    }

    protected int GetRemainingAmmo() => this.m_gunBase.GetTotalAmmunitionAmount();

    protected bool HasEnoughAmmo() => this.m_gunBase.HasEnoughAmmunition();

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      if (!this.HasPlayerAccess(shooter))
      {
        status = MyGunStatusEnum.AccessDenied;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Shooting))
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (action == MyShootActionEnum.PrimaryAction)
      {
        status = MyGunStatusEnum.OK;
        return true;
      }
      status = MyGunStatusEnum.Failed;
      return false;
    }

    public virtual void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      throw new NotImplementedException();
    }

    public bool IsShooting => this.m_isShooting.Value;

    int IMyGunObject<MyGunBase>.ShootDirectionUpdateTime => 0;

    bool IMyGunObject<MyGunBase>.NeedsShootDirectionWhileAiming => false;

    float IMyGunObject<MyGunBase>.MaximumShotLength => 0.0f;

    public Vector3 DirectionToTarget(Vector3D target) => throw new NotImplementedException();

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status) => throw new NotImplementedException();

    public bool PerformFailReaction => throw new NotImplementedException();

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status) => throw new NotImplementedException();

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status) => throw new NotImplementedException();

    public void OnControlAcquired(MyCharacter owner)
    {
    }

    public void OnControlReleased()
    {
    }

    public MyDefinitionId DefinitionId => this.m_defId;

    private bool IsTargetRootValid(MyEntity targetRoot, MyCubeGrid thisTopmostParent)
    {
      if (targetRoot == null || targetRoot.Closed || targetRoot.MarkedForClose)
        return false;
      MyEntity myEntity = targetRoot.GetTopMostParent((System.Type) null) ?? targetRoot;
      if (myEntity.Physics == null || myEntity.MarkedForClose || !myEntity.Physics.Enabled)
        return false;
      bool flag = false;
      MyCubeGrid nodeA = thisTopmostParent;
      if (targetRoot is MyCubeGrid nodeB)
      {
        flag = nodeA.GridSystems.TerminalSystem == nodeB.GridSystems.TerminalSystem && nodeB.BigOwners.Contains(this.OwnerId);
        if (MyCubeGridGroups.Static.Logical.HasSameGroup(nodeA, nodeB))
          return false;
      }
      return flag || nodeB == null || (this.TargetSmallGrids || nodeB.GridSizeEnum != MyCubeSize.Small) && (nodeB.GridSizeEnum != MyCubeSize.Large || (this.TargetLargeGrids || nodeB.IsStatic) && (this.TargetStations || !nodeB.IsStatic));
    }

    private bool IsTarget(MyEntity entity)
    {
      if (entity is MyDebrisBase || !this.TargetMeteors && entity is MyMeteor || (!this.TargetMissiles && entity is MyMissile || entity.Physics != null && !entity.Physics.Enabled))
        return false;
      if (entity is MyDecoy)
        return true;
      return entity is MyCharacter myCharacter ? this.TargetCharacters && !myCharacter.IsDead : this.TargetMeteors && entity is MyMeteor || this.TargetMissiles && entity is MyMissile || entity is IMyComponentOwner<MyIDModule> myComponentOwner && myComponentOwner.GetComponent(out MyIDModule _);
    }

    public bool IsTargetVisible() => this.m_currentPrediction.ManualTargetPosition || this.Target == null || this.IsTargetVisible(this.Target, useVisibilityCache: false);

    private bool IsTargetVisible(
      MyEntity target,
      Vector3D? overridePredictedPos = null,
      bool useVisibilityCache = true)
    {
      if (target == null || target.GetTopMostParent((System.Type) null).Physics == null)
        return false;
      Vector3D predictedTargetPosition;
      if (overridePredictedPos.HasValue)
      {
        predictedTargetPosition = overridePredictedPos.Value;
      }
      else
      {
        using (this.m_currentPredictionUsageLock.AcquireExclusiveUsing())
          predictedTargetPosition = this.m_currentPrediction.GetPredictedTargetPosition((VRage.ModAPI.IMyEntity) target);
      }
      int num;
      if (!overridePredictedPos.HasValue && this.m_notVisibleTargets.TryGetValue(target, out num) && (num > 0 && Sandbox.Game.Multiplayer.Sync.IsServer))
        return false;
      if (useVisibilityCache)
      {
        if (this.m_visibleTargets.ContainsKey(target))
          return true;
        this.SetTargetVisible(target, false, new int?(1));
      }
      MatrixD worldMatrix = this.m_gunBase.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      worldMatrix = this.PositionComp.WorldMatrix;
      Vector3D from = worldMatrix.Translation;
      Vector3D to = predictedTargetPosition;
      if ((double) Vector3.Dot(Vector3.Normalize(to - from), (Vector3) forward) > (double) MyLargeTurretBase.EXACT_VISIBILITY_TEST_TRESHOLD_ANGLE)
        from = this.m_gunBase.GetMuzzleWorldPosition() + forward * 0.5;
      if (useVisibilityCache)
      {
        MyPhysics.CastRayParallel(ref from, ref to, 15, (Action<MyPhysics.HitInfo?>) (physTarget => this.OnVisibilityRayCastCompleted(target, physTarget)));
        return true;
      }
      MyPhysics.HitInfo? physTarget1 = MyPhysics.CastRay(from, to, 15);
      this.OnVisibilityRayCastCompleted(target, physTarget1);
      return true;
    }

    private void OnVisibilityRayCastCompleted(MyEntity target, MyPhysics.HitInfo? physTarget)
    {
      VRage.ModAPI.IMyEntity myEntity = (VRage.ModAPI.IMyEntity) null;
      if (physTarget.HasValue && (HkReferenceObject) physTarget.Value.HkHitInfo.Body != (HkReferenceObject) null && (physTarget.Value.HkHitInfo.Body.UserObject != null && physTarget.Value.HkHitInfo.Body.UserObject is MyPhysicsBody))
        myEntity = ((MyPhysicsComponentBase) physTarget.Value.HkHitInfo.Body.UserObject).Entity;
      if (myEntity != null && target != myEntity && target.Parent != myEntity && (target.Parent == null || target.Parent != myEntity.Parent))
      {
        switch (myEntity)
        {
          case MyMissile _:
          case MyFloatingObject _:
            break;
          default:
            this.SetTargetVisible(target, false);
            return;
        }
      }
      this.SetTargetVisible(target, true);
    }

    private void SetTargetVisible(MyEntity target, bool visible, int? timeout = null)
    {
      if (visible)
      {
        this.m_notVisibleTargets.Remove<MyEntity, int>(target);
        this.m_visibleTargets.TryAdd(target, this.m_VisibleCachePeriod);
      }
      else if (timeout.HasValue)
        this.m_notVisibleTargets[target] = timeout.Value;
      else if (this.m_notVisibleTargets.ContainsKey(target))
        this.m_notVisibleTargets[target] = 12 + MyRandom.Instance.Next(4);
      else
        this.m_notVisibleTargets.TryAdd(target, 4 + MyRandom.Instance.Next(4));
    }

    private MyEntity GetNearestPotentialTarget(
      float rangeSq,
      List<MyLargeTurretBase.MyEntityWithDistSq> outPotentialTargets)
    {
      outPotentialTargets.Clear();
      MyEntity[] array = this.m_targetSelectionWorkData.GridTargeting.TargetRoots.ToArray();
      if (array.Length > 1)
      {
        Vector3D position = this.PositionComp.GetPosition();
        ArrayExtensions.EnsureCapacity<float>(ref this.m_distanceEntityKeys, array.Length);
        for (int index = 0; index < array.Length; ++index)
        {
          MyEntity myEntity = array[index];
          float num = myEntity == null || myEntity.Closed ? float.MaxValue : (float) (myEntity.PositionComp.GetPosition() - position).LengthSquared();
          this.m_distanceEntityKeys[index] = num;
        }
        Array.Sort<float, MyEntity>(this.m_distanceEntityKeys, array, 0, array.Length, (IComparer<float>) FloatComparer.Instance);
      }
      MyEntity nearestTarget = (MyEntity) null;
      double minDistanceSq = (double) rangeSq;
      bool foundDecoy = false;
      if (this.GetTopMostParent((System.Type) null) is MyCubeGrid topMostParent)
      {
        foreach (MyEntity myEntity in array)
        {
          if (myEntity != null && !myEntity.Closed)
          {
            if (this.IsTargetRootValid(myEntity, topMostParent))
            {
              this.m_targetSelectionWorkData.GridTargeting.AllowScanning = false;
              this.TestPotentialTarget(myEntity, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
              this.m_targetSelectionWorkData.GridTargeting.AllowScanning = true;
              if (nearestTarget != null)
                break;
            }
          }
          else
            break;
        }
      }
      return nearestTarget;
    }

    private bool TestPotentialTarget(
      MyEntity target,
      ref MyEntity nearestTarget,
      ref double minDistanceSq,
      ref bool foundDecoy,
      List<MyLargeTurretBase.MyEntityWithDistSq> outPotentialTargets)
    {
      int num1;
      if (target == null || target.MarkedForClose || target.Closed || this.m_notVisibleTargets.TryGetValue(target, out num1) && num1 > 0)
        return false;
      if (target is MyCharacter myCharacter)
      {
        ulong steamId = MySession.Static.Players.TryGetSteamId(myCharacter.GetPlayerIdentityId());
        AdminSettingsEnum adminSettingsEnum;
        if (steamId != 0UL && MySession.Static.RemoteAdminSettings.TryGetValue(steamId, out adminSettingsEnum) && adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.Untargetable))
          return false;
      }
      if (target is MyCubeGrid key)
      {
        if (key.GridSystems.TerminalSystem == this.CubeGrid.GridSystems.TerminalSystem && key.BigOwners.Contains(this.OwnerId) || key.PositionComp.WorldAABB.DistanceSquared(this.PositionComp.GetPosition()) > minDistanceSq)
          return false;
        bool flag = false;
        HashSetReader<MyDecoy> decoys = key.Decoys;
        if (decoys.IsValid)
        {
          foreach (MyDecoy myDecoy in decoys.ToArray())
            flag |= this.TestPotentialTarget((MyEntity) myDecoy, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
        }
        if (flag && this.IsTargetVisible(nearestTarget))
          return true;
        List<MyEntity> orDefault = this.m_targetSelectionWorkData.GridTargeting.TargetBlocks.GetOrDefault(key);
        if (orDefault != null)
        {
          using (this.m_targetSelectionWorkData.GridTargeting.ScanLock.AcquireSharedUsing())
          {
            foreach (MyEntity target1 in orDefault)
              flag |= this.TestPotentialTarget(target1, ref nearestTarget, ref minDistanceSq, ref foundDecoy, outPotentialTargets);
          }
        }
        return flag && this.IsTargetVisible(nearestTarget);
      }
      bool flag1 = this.IsDecoy(target);
      if (foundDecoy && !flag1)
        return false;
      int num2 = this.IsTarget(target) ? 1 : 0;
      bool flag2 = num2 != 0 && this.IsTargetEnemy(target);
      if (num2 == 0 || !flag2)
        return false;
      double num3 = Vector3D.DistanceSquared(target.PositionComp.GetPosition(), this.PositionComp.GetPosition());
      if (num3 >= minDistanceSq)
      {
        outPotentialTargets.Add(new MyLargeTurretBase.MyEntityWithDistSq()
        {
          Entity = target,
          DistSq = num3
        });
        return false;
      }
      if (flag1)
      {
        nearestTarget = target;
        minDistanceSq = num3;
        foundDecoy = true;
        outPotentialTargets.Add(new MyLargeTurretBase.MyEntityWithDistSq()
        {
          Entity = target,
          DistSq = num3
        });
        return true;
      }
      if (this.IsTargetAimedByOtherTurret(target))
        return false;
      minDistanceSq = num3;
      nearestTarget = target;
      outPotentialTargets.Add(new MyLargeTurretBase.MyEntityWithDistSq()
      {
        Entity = target,
        DistSq = num3
      });
      return true;
    }

    private MyEntity GetNearestTarget(
      double maxTargetDistanceSq,
      List<MyLargeTurretBase.MyEntityWithDistSq> validPotentialTargets)
    {
      MyEntity myEntity = (MyEntity) null;
      validPotentialTargets.Sort((Comparison<MyLargeTurretBase.MyEntityWithDistSq>) ((a, b) => a.DistSq.CompareTo(b.DistSq)));
      foreach (MyLargeTurretBase.MyEntityWithDistSq validPotentialTarget in validPotentialTargets)
      {
        if (validPotentialTarget.DistSq <= maxTargetDistanceSq)
        {
          if (this.IsDecoy(validPotentialTarget.Entity) && this.TestTarget(validPotentialTarget.Entity, true))
          {
            myEntity = validPotentialTarget.Entity;
            break;
          }
        }
        else
          break;
      }
      if (myEntity == null)
      {
        foreach (MyLargeTurretBase.MyEntityWithDistSq validPotentialTarget in validPotentialTargets)
        {
          if (validPotentialTarget.DistSq <= maxTargetDistanceSq)
          {
            if (!this.IsDecoy(validPotentialTarget.Entity) && this.TestTarget(validPotentialTarget.Entity, false))
            {
              myEntity = validPotentialTarget.Entity;
              break;
            }
          }
          else
            break;
        }
      }
      return myEntity;
    }

    private bool TestTarget(MyEntity target, bool decoyOnly)
    {
      int num;
      if (this.m_notVisibleTargets.TryGetValue(target, out num) && num > 0)
        return false;
      Vector3D predPos = Vector3D.Zero;
      using (this.m_currentPredictionUsageLock.AcquireExclusiveUsing())
        predPos = this.m_currentPrediction.GetPredictedTargetPosition((VRage.ModAPI.IMyEntity) target);
      return this.IsTargetInView(predPos) && this.IsTargetVisible(target, new Vector3D?(predPos));
    }

    private bool IsDecoy(MyEntity target) => target is MyDecoy myDecoy && myDecoy.IsWorking && target.Parent.Physics != null && target.Parent.Physics.Enabled;

    private bool IsTargetAimedByOtherTurret(MyEntity target) => this.Target != target && MyLargeTurretBase.m_allAimedTargets.Contains(target) && target is IMyDestroyableObject && (double) ((IMyDestroyableObject) target).Integrity < 2.0 * (double) this.m_gunBase.MechanicalDamage;

    private bool IsTargetInView(Vector3D predPos)
    {
      Vector3 lookAtPositionEuler = this.LookAt(predPos);
      return this.m_barrel != null && (double) lookAtPositionEuler.X > (double) this.m_barrel.BarrelElevationMin && this.IsInRange(ref lookAtPositionEuler);
    }

    private bool IsTargetEnemy(MyEntity target)
    {
      switch (target)
      {
        case MyCubeGrid myCubeGrid:
          return myCubeGrid.BigOwners.Count == 0;
        case IMyComponentOwner<MyIDModule> myComponentOwner:
          MyIDModule component;
          return myComponentOwner.GetComponent(out component) && this.IsTargetIdentityEnemy(component.Owner);
        case MyMissile myMissile:
          return this.IsTargetIdentityEnemy(myMissile.Owner);
        default:
          return true;
      }
    }

    private bool IsTargetIdentityEnemy(long targetIidentityId)
    {
      if (this.TargetNeutrals && targetIidentityId == 0L)
        return true;
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.GetUserRelationToOwner(targetIidentityId);
      return this.TargetNeutrals && userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Neutral || userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies;
    }

    private void CheckAndSelectNearTargetsParallel()
    {
      if (this.m_parallelTargetSelectionInProcess)
        return;
      using (this.m_targetSelectionLock.AcquireExclusiveUsing())
        this.m_cancelTargetSelection = false;
      this.m_parallelTargetSelectionInProcess = true;
      this.m_targetSelectionWorkData.SuggestedTarget = this.Target;
      this.m_targetSelectionWorkData.SuggestedTargetIsPotential = this.m_isPotentialTarget;
      this.m_targetSelectionWorkData.GridTargeting = this.CubeGrid.Components.Get<MyGridTargeting>();
      this.m_targetSelectionWorkData.Entity = (MyEntity) this;
      if (true)
      {
        this.m_findNearTargetsTask = Parallel.Start((Action<WorkData>) (data =>
        {
          if (!(data is MyLargeTurretBase.MyTargetSelectionWorkData selectionWorkData) || selectionWorkData.Entity == null || selectionWorkData.Entity.MarkedForClose)
            return;
          this.CheckNearTargets(ref selectionWorkData.SuggestedTarget, ref selectionWorkData.SuggestedTargetIsPotential);
        }), (Action<WorkData>) (data =>
        {
          using (this.m_targetSelectionLock.AcquireSharedUsing())
          {
            if (this.m_cancelTargetSelection)
            {
              this.m_parallelTargetSelectionInProcess = false;
            }
            else
            {
              if (data is MyLargeTurretBase.MyTargetSelectionWorkData selectionWorkData && selectionWorkData.Entity != null && !selectionWorkData.Entity.MarkedForClose)
                this.SetTarget(selectionWorkData.SuggestedTarget, selectionWorkData.SuggestedTargetIsPotential);
              this.m_parallelTargetSelectionInProcess = false;
            }
          }
        }), (WorkData) this.m_targetSelectionWorkData);
      }
      else
      {
        this.CheckNearTargets(ref this.m_targetSelectionWorkData.SuggestedTarget, ref this.m_targetSelectionWorkData.SuggestedTargetIsPotential);
        this.SetTarget(this.m_targetSelectionWorkData.SuggestedTarget, this.m_targetSelectionWorkData.SuggestedTargetIsPotential);
        this.m_parallelTargetSelectionInProcess = false;
      }
    }

    private void CheckNearTargets(
      ref MyEntity suggestedTarget,
      ref bool suggestedTargetIsOnlyPotential)
    {
      if (!this.m_checkOtherTargets)
        return;
      float val1 = (float) this.m_shootingRange * (float) this.m_shootingRange;
      float num1 = this.m_searchingRange * this.m_searchingRange;
      MyEntity myEntity1 = (MyEntity) null;
      MyEntity myEntity2 = suggestedTarget;
      double num2 = (double) num1;
      if (myEntity2 != null)
      {
        if (this.m_barrel != null && this.m_barrel.Entity != null)
          num2 = (myEntity2.PositionComp.GetPosition() - this.m_barrel.Entity.PositionComp.GetPosition()).LengthSquared();
        if (num2 < (double) Math.Min(val1, num1) && this.IsTarget(myEntity2) && this.IsTargetVisible(myEntity2))
          myEntity1 = myEntity2;
      }
      if (myEntity1 == null)
      {
        using (MyUtils.ReuseCollection<MyLargeTurretBase.MyEntityWithDistSq>(ref MyLargeTurretBase.m_allPotentialTargets))
        {
          if (this.GetNearestPotentialTarget(num1, MyLargeTurretBase.m_allPotentialTargets) == null)
          {
            if (MyLargeTurretBase.m_allPotentialTargets.Count <= 0)
              goto label_14;
          }
          MyEntity nearestTarget = this.GetNearestTarget((double) val1, MyLargeTurretBase.m_allPotentialTargets);
          if (nearestTarget != null)
            myEntity1 = nearestTarget;
        }
      }
label_14:
      bool flag = myEntity1 == null;
      if (MyFakes.FakeTarget != null)
      {
        suggestedTarget = MyFakes.FakeTarget;
        suggestedTargetIsOnlyPotential = !this.IsTargetVisible(MyFakes.FakeTarget, new Vector3D?(MyFakes.FakeTarget.WorldMatrix.Translation));
      }
      else
      {
        suggestedTarget = myEntity1;
        suggestedTargetIsOnlyPotential = flag;
      }
    }

    public double GetTargetDistanceSquared() => this.Target != null && this.m_barrel != null && this.m_barrel.Entity != null ? (this.Target.PositionComp.GetPosition() - this.m_barrel.Entity.PositionComp.GetPosition()).LengthSquared() : (double) this.m_searchingRange * (double) this.m_searchingRange;

    public void ResetTarget() => this.SetTarget((MyEntity) null, true);

    public void SetTarget(MyEntity target, bool isPotential)
    {
      this.m_isPotentialTarget = isPotential;
      if (this.m_target == target)
        return;
      MyEntity target1 = this.m_target;
      if (target1 != null)
      {
        target1.OnClose -= new Action<MyEntity>(this.m_target_OnClose);
        if (target1 is MyCharacter)
          (target1 as MyCharacter).CharacterDied -= new Action<MyCharacter>(this.m_target_OnClose);
        MyLargeTurretBase.m_allAimedTargets.Remove(target1);
        MyHud.LargeTurretTargets.UnregisterMarker(target1);
      }
      this.m_target = target;
      if (this.m_target != null)
      {
        this.m_target.OnClose += new Action<MyEntity>(this.m_target_OnClose);
        if (this.m_target is MyCharacter)
          (this.m_target as MyCharacter).CharacterDied += new Action<MyCharacter>(this.m_target_OnClose);
        if (!this.m_isPotentialTarget)
          MyLargeTurretBase.m_allAimedTargets.Add(this.m_target);
        MyHudEntityParams hudParams = new MyHudEntityParams()
        {
          FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ICON
        };
        if (MySession.Static.LocalCharacter != null && !this.m_isPotentialTarget && (Vector3D.DistanceSquared(MySession.Static.LocalCharacter.PositionComp.GetPosition(), this.PositionComp.GetPosition()) < (double) this.ShootingRange * (double) this.ShootingRange && this.HasLocalPlayerAccess()) && this.TestTarget(target, false))
          MyHud.LargeTurretTargets.RegisterMarker(this.m_target, hudParams);
      }
      if (target1 == this.m_target || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_targetSync.Value = new MyLargeTurretBase.CurrentTargetSync()
      {
        TargetId = this.m_target == null ? 0L : this.m_target.EntityId,
        IsPotential = this.m_isPotentialTarget
      };
    }

    private void m_target_OnClose(MyEntity obj)
    {
      this.SetTarget((MyEntity) null, true);
      if (this.m_barrel == null)
        return;
      int num = this.AiEnabled ? 1 : 0;
    }

    public void SetInventory(MyInventory inventory, int index) => this.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);

    public int GetTotalAmmunitionAmount() => this.m_gunBase.GetTotalAmmunitionAmount();

    public int GetAmmunitionAmount() => this.m_gunBase.GetAmmunitionAmount();

    public int GetMagazineAmount() => this.m_gunBase.GetMagazineAmount();

    public void RemoveAmmoPerShot() => this.m_gunBase.ConsumeAmmo();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLargeTurretBase>())
        return;
      base.CreateTerminalControls();
      if (MyFakes.ENABLE_TURRET_CONTROL)
      {
        MyTerminalControlButton<MyLargeTurretBase> button = new MyTerminalControlButton<MyLargeTurretBase>("Control", MySpaceTexts.ControlRemote, MySpaceTexts.Blank, (Action<MyLargeTurretBase>) (t => t.RequestControl()));
        button.Enabled = (Func<MyLargeTurretBase, bool>) (t => t.CanControl());
        button.SupportsMultipleBlocks = false;
        MyTerminalAction<MyLargeTurretBase> myTerminalAction = button.EnableAction<MyLargeTurretBase>(MyTerminalActionIcons.TOGGLE);
        if (myTerminalAction != null)
        {
          myTerminalAction.InvalidToolbarTypes = new List<MyToolbarType>()
          {
            MyToolbarType.ButtonPanel
          };
          myTerminalAction.ValidForGroups = false;
        }
        MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) button);
      }
      MyTerminalControlSlider<MyLargeTurretBase> slider = new MyTerminalControlSlider<MyLargeTurretBase>("Range", MySpaceTexts.BlockPropertyTitle_LargeTurretRadius, MySpaceTexts.BlockPropertyTitle_LargeTurretRadius);
      slider.Normalizer = (MyTerminalControlSlider<MyLargeTurretBase>.FloatFunc) ((x, f) => x.NormalizeRange(f));
      slider.Denormalizer = (MyTerminalControlSlider<MyLargeTurretBase>.FloatFunc) ((x, f) => x.DenormalizeRange(f));
      slider.DefaultValue = new float?(800f);
      slider.Getter = (MyTerminalValueControl<MyLargeTurretBase, float>.GetterDelegate) (x => x.ShootingRange);
      slider.Setter = (MyTerminalValueControl<MyLargeTurretBase, float>.SetterDelegate) ((x, v) => x.ShootingRange = v);
      slider.Writer = (MyTerminalControl<MyLargeTurretBase>.WriterDelegate) ((x, result) => result.AppendInt32((int) (float) x.m_shootingRange).Append(" m"));
      slider.EnableActions<MyLargeTurretBase>();
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) slider);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff1 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("EnableIdleMovement", MySpaceTexts.BlockPropertyTitle_LargeTurretEnableTurretIdleMovement);
      onOff1.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.EnableIdleRotation);
      onOff1.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.EnableIdleRotation = v);
      onOff1.EnableToggleAction<MyLargeTurretBase>();
      onOff1.EnableOnOffActions<MyLargeTurretBase>();
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff1);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) new MyTerminalControlSeparator<MyLargeTurretBase>());
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff2 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetMeteors", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMeteors);
      onOff2.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetMeteors);
      onOff2.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetMeteors = v);
      onOff2.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.METEOR_TOGGLE);
      onOff2.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.METEOR_ON, MyTerminalActionIcons.METEOR_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff2);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff3 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetMissiles", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetMissiles);
      onOff3.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetMissiles);
      onOff3.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetMissiles = v);
      onOff3.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.MISSILE_TOGGLE);
      onOff3.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.MISSILE_ON, MyTerminalActionIcons.MISSILE_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff3);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff4 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetSmallShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetSmallGrids);
      onOff4.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetSmallGrids);
      onOff4.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetSmallGrids = v);
      onOff4.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.SMALLSHIP_TOGGLE);
      onOff4.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.SMALLSHIP_ON, MyTerminalActionIcons.SMALLSHIP_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff4);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff5 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetLargeShips", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetLargeGrids);
      onOff5.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetLargeGrids);
      onOff5.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetLargeGrids = v);
      onOff5.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.LARGESHIP_TOGGLE);
      onOff5.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.LARGESHIP_ON, MyTerminalActionIcons.LARGESHIP_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff5);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff6 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetCharacters", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetCharacters);
      onOff6.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetCharacters);
      onOff6.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetCharacters = v);
      onOff6.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.CHARACTER_TOGGLE);
      onOff6.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.CHARACTER_ON, MyTerminalActionIcons.CHARACTER_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff6);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff7 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetStations", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetStations);
      onOff7.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetStations);
      onOff7.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetStations = v);
      onOff7.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.STATION_TOGGLE);
      onOff7.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.STATION_ON, MyTerminalActionIcons.STATION_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff7);
      MyTerminalControlOnOffSwitch<MyLargeTurretBase> onOff8 = new MyTerminalControlOnOffSwitch<MyLargeTurretBase>("TargetNeutrals", MySpaceTexts.BlockPropertyTitle_LargeTurretTargetNeutrals);
      onOff8.Getter = (MyTerminalValueControl<MyLargeTurretBase, bool>.GetterDelegate) (x => x.TargetNeutrals);
      onOff8.Setter = (MyTerminalValueControl<MyLargeTurretBase, bool>.SetterDelegate) ((x, v) => x.TargetNeutrals = v);
      onOff8.EnableToggleAction<MyLargeTurretBase>(MyTerminalActionIcons.NEUTRALS_TOGGLE);
      onOff8.EnableOnOffActions<MyLargeTurretBase>(MyTerminalActionIcons.NEUTRALS_ON, MyTerminalActionIcons.NEUTRALS_OFF);
      MyTerminalControlFactory.AddControl<MyLargeTurretBase>((MyTerminalControl<MyLargeTurretBase>) onOff8);
    }

    public float ShootingRange
    {
      get => (float) this.m_shootingRange;
      set => this.m_shootingRange.Value = value;
    }

    public float SearchingRange => this.m_searchingRange;

    public bool TargetMeteors
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.Asteroids) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.Asteroids;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.Asteroids;
      }
    }

    public bool TargetMissiles
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.Missiles) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.Missiles;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.Missiles;
      }
    }

    public bool TargetSmallGrids
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.SmallShips) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.SmallShips;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.SmallShips;
      }
    }

    public bool TargetLargeGrids
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.LargeShips) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.LargeShips;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.LargeShips;
      }
    }

    public bool TargetCharacters
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.Players) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.Players;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.Players;
      }
    }

    public bool TargetStations
    {
      get => (uint) (this.TargetFlags & MyTurretTargetFlags.Stations) > 0U;
      set
      {
        if (value)
          this.TargetFlags |= MyTurretTargetFlags.Stations;
        else
          this.TargetFlags &= ~MyTurretTargetFlags.Stations;
      }
    }

    public bool TargetNeutrals
    {
      get => (this.TargetFlags & MyTurretTargetFlags.NotNeutrals) == (MyTurretTargetFlags) 0;
      set
      {
        if (value)
          this.TargetFlags &= ~MyTurretTargetFlags.NotNeutrals;
        else
          this.TargetFlags |= MyTurretTargetFlags.NotNeutrals;
      }
    }

    private bool CanControl()
    {
      if (!this.IsWorking || this.IsPlayerControlled)
        return false;
      return MySession.Static.ControlledEntity is MyCockpit controlledEntity ? !(controlledEntity is MyCryoChamber) && MyAntennaSystem.Static.CheckConnection((MyEntity) controlledEntity.CubeGrid, (MyEntity) this.CubeGrid, controlledEntity.ControllerInfo.Controller.Player) : MySession.Static.ControlledEntity is MyCharacter controlledEntity && MyAntennaSystem.Static.CheckConnection((MyEntity) controlledEntity, (MyEntity) this.CubeGrid, controlledEntity.ControllerInfo.Controller.Player);
    }

    public bool WasControllingCockpitWhenSaved()
    {
      MyEntity entity;
      return this.m_savedPreviousControlledEntityId.HasValue && Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_savedPreviousControlledEntityId.Value, out entity) && entity is MyCockpit;
    }

    private void RequestControl()
    {
      if (!MyFakes.ENABLE_TURRET_CONTROL || !this.CanControl())
        return;
      if (MyGuiScreenTerminal.IsOpen)
        MyGuiScreenTerminal.Hide();
      MySession.Static.GameFocusManager.Clear();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, UseActionEnum, long>(this, (Func<MyLargeTurretBase, Action<UseActionEnum, long>>) (x => new Action<UseActionEnum, long>(x.RequestUseMessage)), UseActionEnum.Manipulate, MySession.Static.ControlledEntity.Entity.EntityId);
    }

    private void AcquireControl(Sandbox.Game.Entities.IMyControllableEntity previousControlledEntity)
    {
      this.PreviousControlledEntity = previousControlledEntity;
      if (previousControlledEntity.ControllerInfo.Controller != null)
        previousControlledEntity.SwitchControl((Sandbox.Game.Entities.IMyControllableEntity) this);
      if (this.IsControlledByLocalPlayer)
      {
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) this, new Vector3D?());
        this.m_targetFov = MyLargeTurretBase.m_maxFov;
        MyLargeTurretBase.SetFov(MyLargeTurretBase.m_maxFov);
        this.UpdateShooting(this.m_isPlayerShooting);
      }
      if (this.PreviousControlledEntity is MyCharacter controlledEntity)
        controlledEntity.CurrentRemoteControl = (Sandbox.Game.Entities.IMyControllableEntity) this;
      this.OnStopAI();
      this.SetTarget((MyEntity) null, true);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void SetCameraOverlay()
    {
      if (!this.IsControlledByLocalPlayer)
        return;
      this.CubeGrid.GridSystems.CameraSystem.ResetCurrentCamera();
      if (this.BlockDefinition != null && this.BlockDefinition.OverlayTexture != null)
      {
        MyHudCameraOverlay.TextureName = this.BlockDefinition.OverlayTexture;
        MyHudCameraOverlay.Enabled = true;
      }
      else
        MyHudCameraOverlay.Enabled = false;
      this.m_hidetoolbar = true;
    }

    private void RemoveCameraOverlay()
    {
      if (!this.IsControlledByLocalPlayer)
        return;
      if (MyGuiScreenHudSpace.Static != null)
        MyGuiScreenHudSpace.Static.SetToolbarVisible(true);
      MyHudCameraOverlay.Enabled = false;
      this.GetFirstRadioReceiver()?.Clear();
      this.ExitView();
    }

    public void ForceReleaseControl() => this.ReleaseControl();

    private void ReleaseControl(bool previousClosed = false)
    {
      if (this.IsControlledByLocalPlayer)
      {
        if (MyGuiScreenHudSpace.Static != null)
          MyGuiScreenHudSpace.Static.SetToolbarVisible(true);
        this.m_hidetoolbar = false;
      }
      if (!this.IsPlayerControlled)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.EndShoot(MyShootActionEnum.PrimaryAction);
      if (this.PreviousControlledEntity is MyCockpit controlledEntity && (previousClosed || controlledEntity.Pilot == null || (controlledEntity.MarkedForClose || controlledEntity.Closed)))
      {
        this.ReturnControl((Sandbox.Game.Entities.IMyControllableEntity) this.m_cockpitPilot);
      }
      else
      {
        if (this.PreviousControlledEntity is MyCharacter controlledEntity)
          controlledEntity.CurrentRemoteControl = (Sandbox.Game.Entities.IMyControllableEntity) null;
        this.CubeGrid.ControlledFromTurret = false;
        this.ReturnControl(this.PreviousControlledEntity);
      }
    }

    public override void SerializeControls(BitStream stream)
    {
      this.m_lastNetMoveState.Serialize(stream);
      stream.WriteBool(this.m_lastNetCanControl);
      stream.WriteBool(this.m_lastNetRotateShip);
    }

    public override void DeserializeControls(BitStream stream, bool outOfOrder)
    {
      this.m_lastNetMoveState = new MyGridClientState(stream);
      this.m_lastNetCanControl = stream.ReadBool();
      this.m_lastNetRotateShip = stream.ReadBool();
      if (!this.m_lastNetMoveState.Valid)
        return;
      this.MoveAndRotate(this.m_lastNetMoveState.Move, this.m_lastNetMoveState.Rotation, this.m_lastNetMoveState.Roll, this.m_lastNetCanControl, this.m_lastNetRotateShip);
    }

    private void OnControlReleased(MyEntityController controller) => this.RemoveCameraOverlay();

    [Event(null, 3311)]
    [Reliable]
    [Broadcast]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void sync_ControlledEntity_Used() => this.ReleaseControl();

    private void sync_UseFailed(
      UseActionEnum action,
      UseActionResult actionResult,
      Sandbox.Game.Entities.IMyControllableEntity user)
    {
      if (user == null || !user.ControllerInfo.IsLocallyHumanControlled())
        return;
      if (actionResult == UseActionResult.UsedBySomeoneElse)
        MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, font: "Red"));
      else if (actionResult == UseActionResult.MissingDLC)
        MySession.Static.CheckDLCAndNotify((MyDefinitionBase) this.BlockDefinition);
      else
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
    }

    private void sync_UseSuccess(UseActionEnum action, Sandbox.Game.Entities.IMyControllableEntity user) => this.AcquireControl(user);

    private void OnStopAI()
    {
      if (this.m_soundEmitter == null)
        return;
      if (this.m_soundEmitter.IsPlaying)
        this.m_soundEmitter.StopSound(true);
      if (!this.m_soundEmitterForRotation.IsPlaying)
        return;
      this.m_soundEmitterForRotation.StopSound(true);
    }

    public void UpdateRotationAndElevation(float newRotation, float newElevation)
    {
      this.Rotation = newRotation;
      this.Elevation = newElevation;
      this.RotateModels();
    }

    private void ReturnControl(Sandbox.Game.Entities.IMyControllableEntity nextControllableEntity)
    {
      if (this.ControllerInfo.Controller != null)
        this.SwitchControl(nextControllableEntity);
      this.PreviousControlledEntity = (Sandbox.Game.Entities.IMyControllableEntity) null;
      this.m_randomStandbyElevation = this.Elevation;
      this.m_randomStandbyRotation = this.Rotation;
      this.m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private MyCharacter GetUser()
    {
      if (this.PreviousControlledEntity != null)
      {
        if (this.PreviousControlledEntity is MyCockpit)
          return (this.PreviousControlledEntity as MyCockpit).Pilot;
        if (this.PreviousControlledEntity is MyCharacter controlledEntity)
          return controlledEntity;
      }
      return (MyCharacter) null;
    }

    private bool IsInRangeAndPlayerHasAccess()
    {
      if (this.ControllerInfo.Controller == null)
        return false;
      if (this.PreviousControlledEntity is MyTerminalBlock controlledEntity)
        return MyAntennaSystem.Static.CheckConnection((MyEntity) controlledEntity.SlimBlock.CubeGrid, (MyEntity) this.CubeGrid, this.ControllerInfo.Controller.Player);
      return !(this.PreviousControlledEntity is MyCharacter controlledEntity) || MyAntennaSystem.Static.CheckConnection((MyEntity) controlledEntity, (MyEntity) this.CubeGrid, this.ControllerInfo.Controller.Player);
    }

    private MyDataReceiver GetFirstRadioReceiver()
    {
      if (this.PreviousControlledEntity is MyCharacter controlledEntity)
        return (MyDataReceiver) controlledEntity.RadioReceiver;
      HashSet<MyDataReceiver> output = new HashSet<MyDataReceiver>();
      MyAntennaSystem.Static.GetEntityReceivers((MyEntity) this.CubeGrid, ref output);
      return output.Count > 0 ? output.FirstElement<MyDataReceiver>() : (MyDataReceiver) null;
    }

    private void AddPreviousControllerEvents()
    {
      this.m_previousControlledEntity.Entity.OnMarkForClose += new Action<MyEntity>(this.Entity_OnPreviousMarkForClose);
      if (!(this.m_previousControlledEntity.Entity is MyTerminalBlock entity))
        return;
      entity.IsWorkingChanged += new Action<MyCubeBlock>(this.PreviousCubeBlock_IsWorkingChanged);
    }

    private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      if (obj.IsWorking || obj.Closed || obj.MarkedForClose)
        return;
      this.ReleaseControl();
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      if (this.PreviousControlledEntity == null || !Sandbox.Game.Multiplayer.Sync.IsServer || (this.ControllerInfo.Controller == null || this.HasPlayerAccess(this.ControllerInfo.Controller.Player.Identity.IdentityId)))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase>(this, (Func<MyLargeTurretBase, Action>) (x => new Action(x.sync_ControlledEntity_Used)));
    }

    private void Entity_OnPreviousMarkForClose(MyEntity obj) => this.ReleaseControl(true);

    public UseActionResult CanUse(
      UseActionEnum actionEnum,
      Sandbox.Game.Entities.IMyControllableEntity user)
    {
      if (!this.IsWorking)
        return UseActionResult.AccessDenied;
      return this.IsPlayerControlled ? UseActionResult.UsedBySomeoneElse : UseActionResult.OK;
    }

    public void RemoveUsers(bool local)
    {
    }

    public bool PrimaryLookaround => false;

    private new MatrixD GetViewMatrix()
    {
      this.RotateModels();
      MatrixD matrix1 = this.m_base2.WorldMatrix;
      if (this.CameraDummy != null)
      {
        Matrix matrix2 = Matrix.Normalize(this.CameraDummy.Matrix);
        matrix1 = MatrixD.Multiply((MatrixD) ref matrix2, matrix1);
      }
      else
      {
        matrix1.Translation += matrix1.Forward * (double) this.ForwardCameraOffset;
        matrix1.Translation += matrix1.Up * (double) this.UpCameraOffset;
      }
      MatrixD result;
      MatrixD.Invert(ref matrix1, out result);
      return result;
    }

    void IMyCameraController.ControlCamera(MyCamera currentCamera) => currentCamera.SetViewMatrix(this.GetViewMatrix());

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
    }

    void IMyCameraController.RotateStopped()
    {
    }

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
      this.SetCameraOverlay();
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
      this.RemoveCameraOverlay();
    }

    bool IMyCameraController.HandleUse()
    {
      if (MySession.Static.LocalCharacter != null)
      {
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (VRage.ModAPI.IMyEntity) MySession.Static.LocalCharacter, new Vector3D?());
        this.m_targetFov = MyLargeTurretBase.m_maxFov;
        MyLargeTurretBase.SetFov(MyLargeTurretBase.m_maxFov);
      }
      return false;
    }

    bool IMyCameraController.HandlePickUp() => false;

    bool IMyCameraController.IsInFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    bool IMyCameraController.ForceFirstPersonCamera { get; set; }

    bool IMyCameraController.AllowCubeBuilding => false;

    bool IMyCameraController.EnableFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    private void ChangeZoom(int deltaZoom)
    {
      if (deltaZoom > 0)
      {
        this.m_targetFov -= 0.15f;
        if ((double) this.m_targetFov < (double) MyLargeTurretBase.m_minFov)
          this.m_targetFov = MyLargeTurretBase.m_minFov;
      }
      else
      {
        this.m_targetFov += 0.15f;
        if ((double) this.m_targetFov > (double) MyLargeTurretBase.m_maxFov)
          this.m_targetFov = MyLargeTurretBase.m_maxFov;
      }
      MyLargeTurretBase.SetFov(this.m_fov);
    }

    private void ChangeZoomPrecise(float deltaZoom)
    {
      this.m_targetFov += deltaZoom;
      if ((double) deltaZoom < 0.0)
      {
        if ((double) this.m_targetFov < (double) MyLargeTurretBase.m_minFov)
          this.m_targetFov = MyLargeTurretBase.m_minFov;
      }
      else if ((double) this.m_targetFov > (double) MyLargeTurretBase.m_maxFov)
        this.m_targetFov = MyLargeTurretBase.m_maxFov;
      MyLargeTurretBase.SetFov(this.m_fov);
    }

    private float GetZoomNormalized() => (float) (((double) this.m_targetFov - (double) MyLargeTurretBase.m_minFov) / ((double) MyLargeTurretBase.m_maxFov - (double) MyLargeTurretBase.m_minFov));

    private float GetRotationMultiplier()
    {
      float zoomNormalized = this.GetZoomNormalized();
      return (float) ((double) zoomNormalized * (double) MyLargeTurretBase.ROTATION_MULTIPLIER_NORMAL + (1.0 - (double) zoomNormalized) * (double) MyLargeTurretBase.ROTATION_MULTIPLIER_ZOOMED);
    }

    public void ExitView() => MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;

    private static void SetFov(float fov)
    {
      fov = MathHelper.Clamp(fov, 1E-05f, 3.124139f);
      MySector.MainCamera.FieldOfView = fov;
    }

    public MyControllerInfo ControllerInfo => this.m_controllerInfo;

    public MyEntity Entity => (MyEntity) this;

    VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => (VRage.ModAPI.IMyEntity) this;

    public bool ForceFirstPersonCamera { get; set; }

    public MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceBoneAnim = false,
      bool forceHeadBone = false)
    {
      return this.GetViewMatrix();
    }

    public void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator, false, MyInput.Static.IsAnyAltKeyPressed());
    }

    public void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator,
      bool overrideControlCheck,
      bool rotateShip)
    {
      this.LastRotationIndicator = new Vector3(rotationIndicator, rollIndicator);
      float rotationMultiplier = this.GetRotationMultiplier();
      this.m_lastNetMoveState = new MyGridClientState()
      {
        Move = moveIndicator,
        Rotation = rotationIndicator * rotationMultiplier,
        Roll = rollIndicator * rotationMultiplier
      };
      this.m_lastNetRotateShip = rotateShip;
      bool flag1 = false;
      if (this.PreviousControlledEntity is MyShipController controlledEntity)
      {
        bool flag2 = true;
        if (!overrideControlCheck && this.CubeGrid.HasMainCockpit() && (controlledEntity.Pilot == null || controlledEntity.Pilot != MySession.Static.LocalCharacter))
          flag2 = false;
        if (flag2 && (overrideControlCheck || controlledEntity.HasLocalPlayerAccess()))
        {
          this.m_lastNetCanControl = true;
          if (rotateShip)
          {
            controlledEntity.MoveAndRotate(moveIndicator, rotationIndicator * rotationMultiplier, rollIndicator * rotationMultiplier);
            flag1 = true;
          }
          else
            controlledEntity.MoveAndRotate(moveIndicator, Vector2.Zero, rollIndicator * rotationMultiplier);
          controlledEntity.MoveAndRotate();
          this.CubeGrid.ControlledFromTurret = true;
        }
        else
          this.m_lastNetCanControl = false;
      }
      if (flag1 || (double) rotationIndicator.X == 0.0 && (double) rotationIndicator.Y == 0.0 || (this.m_barrel == null || this.SyncObject == null))
        return;
      float num = (float) (0.0500000007450581 * (double) this.m_rotationSpeed * 16.0);
      this.Rotation -= rotationIndicator.Y * num * rotationMultiplier;
      this.Elevation -= rotationIndicator.X * num * rotationMultiplier;
      this.Elevation = MathHelper.Clamp(this.Elevation, this.m_minElevationRadians, 1.553343f);
      this.RotateModels();
      this.m_rotationAndElevationSync.Value = new MyLargeTurretBase.SyncRotationAndElevation()
      {
        Rotation = this.Rotation,
        Elevation = this.Elevation
      };
    }

    public float HeadLocalXAngle { get; set; }

    public float HeadLocalYAngle { get; set; }

    public void MoveAndRotateStopped() => this.RotateModels();

    public new void BeginShoot(MyShootActionEnum action) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, MyShootActionEnum>(this, (Func<MyLargeTurretBase, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.OnBeginShoot)), action);

    public new void EndShoot(MyShootActionEnum action) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, MyShootActionEnum>(this, (Func<MyLargeTurretBase, Action<MyShootActionEnum>>) (x => new Action<MyShootActionEnum>(x.OnEndShoot)), action);

    [Event(null, 3804)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnBeginShoot(MyShootActionEnum action)
    {
      this.m_isPlayerShooting = true;
      this.Render.NeedsDrawFromParent = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    [Event(null, 3815)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnEndShoot(MyShootActionEnum action)
    {
      this.UpdateShooting(false);
      this.m_isPlayerShooting = false;
      this.Render.NeedsDrawFromParent = false;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_isShooting.Value = false;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public void Use()
    {
      MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase>(this, (Func<MyLargeTurretBase, Action>) (x => new Action(x.sync_ControlledEntity_Used)));
    }

    public void UseContinues()
    {
    }

    public void UseFinished()
    {
    }

    public void PickUp()
    {
    }

    public void PickUpContinues()
    {
    }

    public void PickUpFinished()
    {
    }

    public void Sprint(bool enabled)
    {
    }

    public void Jump(Vector3 moveIndicator)
    {
    }

    public void SwitchWalk()
    {
    }

    public void Up()
    {
    }

    public void Crouch()
    {
    }

    public void Down()
    {
    }

    public void SwitchBroadcasting()
    {
    }

    public void ShowInventory()
    {
      MyCharacter user = this.GetUser();
      if (user == null)
        return;
      MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, (MyEntity) this);
    }

    public void ShowTerminal() => MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalCharacter, (MyEntity) this);

    public void SwitchThrusts()
    {
    }

    public void SwitchDamping()
    {
      if (!(this.PreviousControlledEntity is MyShipController controlledEntity) || !this.CubeGrid.ControlledFromTurret)
        return;
      controlledEntity.SwitchDamping();
    }

    public void SwitchLights()
    {
      if (!(this.PreviousControlledEntity is MyShipController controlledEntity) || !this.CubeGrid.ControlledFromTurret)
        return;
      controlledEntity.SwitchLights();
    }

    public void SwitchLandingGears()
    {
      if (!(this.PreviousControlledEntity is MyShipController controlledEntity) || !this.CubeGrid.ControlledFromTurret)
        return;
      controlledEntity.SwitchLandingGears();
    }

    public void SwitchHandbrake()
    {
      if (!(this.PreviousControlledEntity is MyShipController controlledEntity) || !this.CubeGrid.ControlledFromTurret)
        return;
      controlledEntity.SwitchHandbrake();
    }

    public bool EnabledThrusts => false;

    public bool EnabledDamping => false;

    public bool EnabledLights => false;

    public bool EnabledLeadingGears => false;

    public bool EnabledReactors => false;

    public bool EnabledBroadcasting => false;

    public bool EnabledHelmet => false;

    public void SwitchToWeapon(MyDefinitionId weaponDefinition)
    {
    }

    public void SwitchToWeapon(MyToolbarItemWeapon weapon)
    {
    }

    public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition) => false;

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      if (MyGuiScreenHudSpace.Static == null)
        return;
      MyGuiScreenHudSpace.Static.SetToolbarVisible(false);
    }

    public void SwitchReactors()
    {
      if (!(this.PreviousControlledEntity is MyShipController controlledEntity) || !this.CubeGrid.ControlledFromTurret)
        return;
      controlledEntity.SwitchReactors();
    }

    public void SwitchHelmet()
    {
    }

    public void Die()
    {
    }

    public MyToolbarType ToolbarType => MyToolbarType.LargeCockpit;

    public MyToolbar Toolbar => this.m_toolbar;

    protected void DrawLasers()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || this.m_barrel == null || !MyFakes.ENABLE_TURRET_LASERS)
        return;
      MyGunStatusEnum status = MyGunStatusEnum.Cooldown;
      if (!this.IsWorking)
        return;
      Vector4 vector4 = Color.Green.ToVector4();
      switch (this.GetStatus())
      {
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated:
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Searching:
          vector4 = Color.Green.ToVector4();
          break;
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Shooting:
        case MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_ShootDelaying:
          vector4 = Color.Red.ToVector4();
          break;
      }
      MyStringId idWeaponLaser = MyLargeTurretBase.ID_WEAPON_LASER;
      float num = 0.1f;
      Vector3D zero1 = Vector3D.Zero;
      Vector3D zero2 = Vector3D.Zero;
      Vector4 color = vector4 * 0.5f;
      Vector3D linePointA;
      Vector3D hitPosition;
      if (this.Target != null && !this.m_isPotentialTarget)
      {
        if (!this.CanShoot(out status))
          color = 0.3f * Color.DarkRed.ToVector4();
        MatrixD muzzleWorldMatrix1 = this.m_gunBase.GetMuzzleWorldMatrix();
        Vector3D translation1 = muzzleWorldMatrix1.Translation;
        muzzleWorldMatrix1 = this.m_gunBase.GetMuzzleWorldMatrix();
        Vector3D vector3D = 2.0 * muzzleWorldMatrix1.Forward;
        linePointA = translation1 + vector3D;
        MatrixD muzzleWorldMatrix2 = this.m_gunBase.GetMuzzleWorldMatrix();
        Vector3D translation2 = muzzleWorldMatrix2.Translation;
        Vector3D to = translation2 + muzzleWorldMatrix2.Forward * (double) this.m_searchingRange;
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(translation2, to);
        this.m_hitPosition = nullable.HasValue ? nullable.Value.Position : to;
        hitPosition = this.m_hitPosition;
      }
      else
      {
        MatrixD muzzleWorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
        this.m_hitPosition = muzzleWorldMatrix.Translation + muzzleWorldMatrix.Forward * this.m_laserLength;
        linePointA = this.m_barrel.Entity.PositionComp.GetPosition();
        hitPosition = this.m_hitPosition;
      }
      Vector3D position = MySector.MainCamera.Position;
      float distanceFromPoint = (float) MySector.MainCamera.GetDistanceFromPoint(MyUtils.GetClosestPointOnLine(ref linePointA, ref hitPosition, ref position));
      float thickness = num * (Math.Min(distanceFromPoint, 10f) * 0.05f);
      MySimpleObjectDraw.DrawLine(linePointA, hitPosition, new MyStringId?(idWeaponLaser), ref color, thickness);
    }

    public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      this.Hierarchy.GetChildrenRecursive(this.m_children);
      foreach (MyEntity child in this.m_children)
      {
        MyModel model = child.Model;
        if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) child, ref aabb))
          return true;
      }
      MyModel model1 = this.Model;
      return model1 != null && model1.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) this, ref aabb);
    }

    private void OnTargetFlagChanged()
    {
      this.m_workingFlagCombination = (uint) (this.TargetFlags & ~MyTurretTargetFlags.NotNeutrals) > 0U;
      MyEntity target = this.m_target;
      double minDistanceSq = 0.0;
      bool foundDecoy = false;
      using (MyUtils.ReuseCollection<MyLargeTurretBase.MyEntityWithDistSq>(ref MyLargeTurretBase.m_allPotentialTargets))
      {
        if (this.m_target == null || this.TestPotentialTarget(this.m_target, ref target, ref minDistanceSq, ref foundDecoy, MyLargeTurretBase.m_allPotentialTargets))
          return;
        using (this.m_targetSelectionLock.AcquireExclusiveUsing())
          this.m_cancelTargetSelection = true;
        this.SetTarget((MyEntity) null, true);
      }
    }

    public void SwitchAmmoMagazine() => this.m_gunBase.SwitchAmmoMagazineToNextAvailable();

    public bool CanSwitchAmmoMagazine() => false;

    MyEntity[] IMyGunBaseUser.IgnoreEntities => this.m_shootIgnoreEntities;

    MyEntity IMyGunBaseUser.Weapon => this.m_barrel == null ? (MyEntity) null : this.m_barrel.Entity;

    MyEntity IMyGunBaseUser.Owner => this.Parent;

    public virtual IMyMissileGunObject Launcher => (IMyMissileGunObject) this;

    MyInventory IMyGunBaseUser.AmmoInventory => MyEntityExtensions.GetInventory(this);

    MyDefinitionId IMyGunBaseUser.PhysicalItemId => new MyDefinitionId();

    MyInventory IMyGunBaseUser.WeaponInventory => (MyInventory) null;

    long IMyGunBaseUser.OwnerId => this.OwnerId;

    string IMyGunBaseUser.ConstraintDisplayName => base.BlockDefinition.DisplayNameText;

    private float NormalizeRange(float value) => (double) value == 0.0 ? 0.0f : MathHelper.Clamp((float) (((double) value - (double) this.m_minRangeMeter) / ((double) this.m_maxRangeMeter - (double) this.m_minRangeMeter)), 0.0f, 1f);

    private float DenormalizeRange(float value) => (double) value == 0.0 ? 0.0f : MathHelper.Clamp(this.m_minRangeMeter + value * (this.m_maxRangeMeter - this.m_minRangeMeter), this.m_minRangeMeter, this.m_maxRangeMeter);

    public override void TakeControlFromTerminal()
    {
      this.m_checkOtherTargets = false;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_rotationAndElevationSync.Value = new MyLargeTurretBase.SyncRotationAndElevation()
      {
        Rotation = this.Rotation,
        Elevation = this.Elevation
      };
    }

    public void ForceTarget(MyEntity entity, bool usePrediction)
    {
      this.SetTarget(entity, false);
      this.m_currentPrediction = usePrediction ? this.m_targetPrediction : this.m_targetNoPrediction;
      this.m_checkOtherTargets = false;
    }

    public void TargetPosition(Vector3D pos, Vector3 velocity, bool usePrediction)
    {
      this.m_checkOtherTargets = false;
      this.ResetTarget();
      if (usePrediction)
      {
        this.m_currentPrediction = this.m_positionPrediction;
        MyLargeTurretBase.MyPositionPredictionType currentPrediction = this.m_currentPrediction as MyLargeTurretBase.MyPositionPredictionType;
        currentPrediction.TrackedPosition = pos;
        currentPrediction.TrackedVelocity = (Vector3D) velocity;
      }
      else
      {
        this.m_currentPrediction = this.m_positionNoPrediction;
        (this.m_currentPrediction as MyLargeTurretBase.MyPositionNoPredictionType).TrackedPosition = pos;
      }
      this.m_isPotentialTarget = false;
    }

    public void ChangeIdleRotation(bool enable) => this.EnableIdleRotation = enable;

    [Event(null, 4260)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void ResetTargetParams()
    {
      this.ResetTarget();
      this.m_currentPrediction = this.m_targetPrediction;
      this.m_checkOtherTargets = true;
      this.EnableIdleRotation = this.BlockDefinition.IdleRotation;
    }

    public void SetManualAzimuth(float value)
    {
      if ((double) this.Rotation == (double) value)
        return;
      this.Rotation = value;
      this.m_checkOtherTargets = false;
      this.ResetTarget();
      this.RotateModels();
    }

    public void SetManualElevation(float value)
    {
      if ((double) this.Elevation == (double) value)
        return;
      this.Elevation = value;
      this.m_checkOtherTargets = false;
      this.ResetTarget();
      this.RotateModels();
    }

    private bool IsInRange(ref Vector3 lookAtPositionEuler)
    {
      float y = lookAtPositionEuler.Y;
      float x = lookAtPositionEuler.X;
      return (double) y > (double) this.m_minAzimuthRadians && (double) y < (double) this.m_maxAzimuthRadians && (double) x > (double) this.m_minElevationRadians && (double) x < (double) this.m_maxElevationRadians;
    }

    public override bool CanOperate() => this.CheckIsWorking();

    public override void ShootFromTerminal(Vector3 direction)
    {
      base.ShootFromTerminal(direction);
      if (this.m_barrel == null)
        return;
      if (((bool) this.m_isShooting || (bool) this.m_forceShoot ? (this.ControllerInfo == null ? 0 : (this.ControllerInfo.Controller != null ? 1 : 0)) : 1) != 0)
        this.m_barrel.DontTimeOffsetNextShot();
      this.m_barrel.StartShooting();
      this.m_checkOtherTargets = true;
    }

    public override void StopShootFromTerminal()
    {
      if (this.m_barrel == null)
        return;
      this.m_barrel.StopShooting();
    }

    public override void SyncRotationAndOrientation() => this.m_rotationAndElevationSync.Value = new MyLargeTurretBase.SyncRotationAndElevation()
    {
      Rotation = this.Rotation,
      Elevation = this.Elevation
    };

    protected override void RememberIdle() => this.m_previousIdleRotationState = this.EnableIdleRotation;

    protected override void RestoreIdle() => this.EnableIdleRotation = this.m_previousIdleRotationState;

    [Event(null, 4340)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void RequestUseMessage(UseActionEnum useAction, long usedById)
    {
      MyEntity entity;
      int num = Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyEntity>(usedById, out entity) ? 1 : 0;
      Sandbox.Game.Entities.IMyControllableEntity user = entity as Sandbox.Game.Entities.IMyControllableEntity;
      UseActionResult useResult = UseActionResult.OK;
      if (num != 0 && (useResult = this.CanUse(useAction, user)) == UseActionResult.OK)
      {
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, UseActionEnum, long, UseActionResult>(this, (Func<MyLargeTurretBase, Action<UseActionEnum, long, UseActionResult>>) (x => new Action<UseActionEnum, long, UseActionResult>(x.UseSuccessCallback)), useAction, usedById, useResult);
        this.UseSuccessCallback(useAction, usedById, useResult);
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, UseActionEnum, long, UseActionResult>(this, (Func<MyLargeTurretBase, Action<UseActionEnum, long, UseActionResult>>) (x => new Action<UseActionEnum, long, UseActionResult>(x.UseFailureCallback)), useAction, usedById, useResult, MyEventContext.Current.Sender);
    }

    [Event(null, 4362)]
    [Reliable]
    [Broadcast]
    private void UseSuccessCallback(
      UseActionEnum useAction,
      long usedById,
      UseActionResult useResult)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyEntity>(usedById, out entity) || !(entity is Sandbox.Game.Entities.IMyControllableEntity user))
        return;
      MyRelationsBetweenPlayerAndBlock relations = MyRelationsBetweenPlayerAndBlock.NoOwnership;
      MyCubeBlock myCubeBlock = (MyCubeBlock) this;
      if (myCubeBlock != null && user.ControllerInfo.Controller != null)
        relations = myCubeBlock.GetUserRelationToOwner(user.ControllerInfo.Controller.Player.Identity.IdentityId);
      if (relations.IsFriendly() || MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
        this.sync_UseSuccess(useAction, user);
      else
        this.sync_UseFailed(useAction, useResult, user);
    }

    [Event(null, 4392)]
    [Reliable]
    [Client]
    private void UseFailureCallback(
      UseActionEnum useAction,
      long usedById,
      UseActionResult useResult)
    {
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyEntity>(usedById, out entity);
      Sandbox.Game.Entities.IMyControllableEntity user = entity as Sandbox.Game.Entities.IMyControllableEntity;
      this.sync_UseFailed(useAction, useResult, user);
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static != null && Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_replicableServer = Sandbox.Engine.Multiplayer.MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
        this.m_blocksReplicable = (IMyReplicable) MyExternalReplicable.FindByObject((object) this);
      }
      if (this.CubeGrid == null)
        return;
      MyCubeGrid cubeGrid = this.CubeGrid;
      cubeGrid.IsPreviewChanged = cubeGrid.IsPreviewChanged + new Action<bool>(this.PreviewChangedCallback);
      this.PreviewChangedCallback(this.CubeGrid.IsPreview);
    }

    public override void OnRemovedFromScene(object source)
    {
      if (this.CubeGrid != null)
      {
        MyCubeGrid cubeGrid = this.CubeGrid;
        cubeGrid.IsPreviewChanged = cubeGrid.IsPreviewChanged - new Action<bool>(this.PreviewChangedCallback);
      }
      base.OnRemovedFromScene(source);
    }

    private void PreviewChangedCallback(bool isPreview)
    {
      if (this.Barrel == null)
        return;
      this.Barrel.IsPreviewChanged(isPreview);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public MyEntity RelativeDampeningEntity { get; set; }

    public void MissileShootEffect()
    {
      if (this.m_barrel == null)
        return;
      this.m_barrel.ShootEffect();
    }

    public void ShootMissile(MyObjectBuilder_Missile builder) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, MyObjectBuilder_Missile>(this, (Func<MyLargeTurretBase, Action<MyObjectBuilder_Missile>>) (x => new Action<MyObjectBuilder_Missile>(x.OnShootMissile)), builder);

    [Event(null, 4497)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnShootMissile(MyObjectBuilder_Missile builder) => MyMissiles.Add(builder);

    public void RemoveMissile(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, long>(this, (Func<MyLargeTurretBase, Action<long>>) (x => new Action<long>(x.OnRemoveMissile)), entityId);

    [Event(null, 4510)]
    [Reliable]
    [Broadcast]
    private void OnRemoveMissile(long entityId) => MyMissiles.Remove(entityId);

    public MyParallelUpdateFlags UpdateFlags => this.m_parallelFlag.GetFlags((MyEntity) this);

    public override void DisableUpdates()
    {
      base.DisableUpdates();
      this.m_parallelFlag.Disable((MyEntity) this);
    }

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      if (this.Render.IsVisible() && this.IsWorking && this.Enabled)
      {
        if (!MySession.Static.CreativeMode && !this.HasEnoughAmmo())
          this.m_gunBase.SwitchAmmoMagazineToNextAvailable();
        this.m_isInRandomRotationDistance = false;
        if (this.m_barrel != null && !this.IsPlayerControlled && this.AiEnabled)
        {
          this.UpdateAiWeapon();
          this.m_isInRandomRotationDistance = MySector.MainCamera.GetDistanceFromPoint(this.PositionComp.GetPosition()) <= 600.0;
          if (this.m_isInRandomRotationDistance)
            this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        }
        ++this.m_notVisibleTargetsUpdatesSinceRefresh;
        if (!this.m_parallelTargetSelectionInProcess)
          this.UpdateVisibilityCacheCounters();
        if (this.IsControlled)
        {
          if (!this.IsInRangeAndPlayerHasAccess())
          {
            this.ReleaseControl();
            if (MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity == this)
              MyGuiScreenTerminal.Hide();
          }
          else
            this.GetFirstRadioReceiver()?.UpdateHud(true);
        }
        else
        {
          if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.m_currentPrediction.ManualTargetPosition && this.m_workingFlagCombination && (MySession.Static.CreativeMode || this.HasEnoughAmmo()))
            this.CheckAndSelectNearTargetsParallel();
          if (this.GetStatus() == MyLargeTurretBase.MyLargeShipGunStatus.MyWeaponStatus_Deactivated && this.m_randomIsMoving || this.Target != null && this.m_isPotentialTarget)
            this.SetupSearchRaycast();
        }
        if (this.m_playAimingSound)
          this.PlayAimingSound();
        else
          this.StopAimingSound();
      }
      this.m_isMoving = false;
      if (this.CubeGrid?.Physics == null)
        return;
      MatrixD worldMatrix = this.CubeGrid.WorldMatrix;
      if (worldMatrix.EqualsFast(ref this.m_lastTestedGridWM))
        return;
      this.m_isMoving = true;
      this.m_lastTestedGridWM = worldMatrix;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override bool GetTimerEnabledState() => this.Enabled;

    protected override void TiersChanged()
    {
      MyUpdateTiersPlayerPresence playerPresenceTier = this.CubeGrid.PlayerPresenceTier;
      MyUpdateTiersGridPresence gridPresenceTier = this.CubeGrid.GridPresenceTier;
      if (playerPresenceTier == MyUpdateTiersPlayerPresence.Normal || gridPresenceTier == MyUpdateTiersGridPresence.Normal)
      {
        this.ChangeTimerTick(this.GetTimerTime(0));
      }
      else
      {
        if (playerPresenceTier != MyUpdateTiersPlayerPresence.Tier1 && playerPresenceTier != MyUpdateTiersPlayerPresence.Tier2 && gridPresenceTier != MyUpdateTiersGridPresence.Tier1)
          return;
        this.ChangeTimerTick(this.GetTimerTime(1));
      }
    }

    protected override uint GetDefaultTimeForUpdateTimer(int index)
    {
      if (index == 0)
        return MyLargeTurretBase.TIMER_NORMAL_IN_FRAMES;
      return index == 1 ? MyLargeTurretBase.TIMER_TIER1_IN_FRAMES : 0U;
    }

    public Vector3D GetMuzzlePosition() => this.m_gunBase.GetMuzzleWorldPosition();

    VRage.ModAPI.IMyEntity Sandbox.ModAPI.IMyLargeTurretBase.Target => (VRage.ModAPI.IMyEntity) this.Target;

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.IsAimed => this.IsAimed;

    void Sandbox.ModAPI.IMyLargeTurretBase.TrackTarget(VRage.ModAPI.IMyEntity entity)
    {
      if (entity == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, long, bool>(this, (Func<MyLargeTurretBase, Action<long, bool>>) (x => new Action<long, bool>(x.SetTargetRequest)), entity.EntityId, true);
    }

    [Event(null, 40)]
    [Reliable]
    [Server]
    [Broadcast]
    private void SetTargetRequest(long entityId, bool usePrediction)
    {
      MyEntity entity = (MyEntity) null;
      if (entityId != 0L)
        Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity);
      this.ForceTarget(entity, usePrediction);
    }

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.TrackTarget(
      Vector3D pos,
      Vector3 velocity)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, Vector3D, Vector3, bool>(this, (Func<MyLargeTurretBase, Action<Vector3D, Vector3, bool>>) (x => new Action<Vector3D, Vector3, bool>(x.SetTargetPosition)), pos, velocity, true);
    }

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SetTarget(Vector3D pos) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, Vector3D, Vector3, bool>(this, (Func<MyLargeTurretBase, Action<Vector3D, Vector3, bool>>) (x => new Action<Vector3D, Vector3, bool>(x.SetTargetPosition)), pos, Vector3.Zero, false);

    [Event(null, 62)]
    [Reliable]
    [Server]
    [Broadcast]
    private void SetTargetPosition(Vector3D targetPos, Vector3 velocity, bool usePrediction) => this.TargetPosition(targetPos, velocity, usePrediction);

    void Sandbox.ModAPI.IMyLargeTurretBase.SetTarget(VRage.ModAPI.IMyEntity entity)
    {
      if (entity == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase, long, bool>(this, (Func<MyLargeTurretBase, Action<long, bool>>) (x => new Action<long, bool>(x.SetTargetRequest)), entity.EntityId, false);
    }

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.HasTarget => this.Target != null;

    IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => (IMyControllerInfo) this.ControllerInfo;

    public Vector3 LastMotionIndicator => (Vector3) Vector3D.Zero;

    public Vector3 LastRotationIndicator { get; set; }

    float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Elevation
    {
      get => this.Elevation;
      set => this.SetManualElevation(value);
    }

    float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Azimuth
    {
      get => this.Rotation;
      set => this.SetManualAzimuth(value);
    }

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.EnableIdleRotation
    {
      get => this.EnableIdleRotation;
      set => this.EnableIdleRotation = value;
    }

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.AIEnabled => this.AiEnabled;

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.IsUnderControl => this.ControllerInfo.Controller != null;

    bool Sandbox.ModAPI.Ingame.IMyLargeTurretBase.CanControl => this.CanControl();

    float Sandbox.ModAPI.Ingame.IMyLargeTurretBase.Range => this.ShootingRange;

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.ResetTargetingToDefault() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeTurretBase>(this, (Func<MyLargeTurretBase, Action>) (x => new Action(x.ResetTargetParams)));

    MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMyLargeTurretBase.GetTargetedEntity() => MyDetectedEntityInfoHelper.Create(this.Target, this.OwnerId);

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncEnableIdleRotation()
    {
    }

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncAzimuth() => this.SyncRotationAndOrientation();

    void Sandbox.ModAPI.Ingame.IMyLargeTurretBase.SyncElevation() => this.SyncRotationAndOrientation();

    public MyEntityCameraSettings GetCameraEntitySettings() => (MyEntityCameraSettings) null;

    public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

    public MyStringId AuxiliaryContext => MySpaceBindingCreator.AX_ACTIONS;

    public bool SupressShootAnimation() => false;

    public bool ShouldEndShootingOnPause(MyShootActionEnum action) => true;

    [Serializable]
    private struct SyncRotationAndElevation
    {
      public float Rotation;
      public float Elevation;

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003ESyncRotationAndElevation\u003C\u003ERotation\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.SyncRotationAndElevation, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyLargeTurretBase.SyncRotationAndElevation owner,
          in float value)
        {
          owner.Rotation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyLargeTurretBase.SyncRotationAndElevation owner,
          out float value)
        {
          value = owner.Rotation;
        }
      }

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003ESyncRotationAndElevation\u003C\u003EElevation\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.SyncRotationAndElevation, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyLargeTurretBase.SyncRotationAndElevation owner,
          in float value)
        {
          owner.Elevation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyLargeTurretBase.SyncRotationAndElevation owner,
          out float value)
        {
          value = owner.Elevation;
        }
      }
    }

    [Serializable]
    private struct CurrentTargetSync
    {
      public long TargetId;
      public bool IsPotential;

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003ECurrentTargetSync\u003C\u003ETargetId\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.CurrentTargetSync, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLargeTurretBase.CurrentTargetSync owner, in long value) => owner.TargetId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLargeTurretBase.CurrentTargetSync owner, out long value) => value = owner.TargetId;
      }

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003ECurrentTargetSync\u003C\u003EIsPotential\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.CurrentTargetSync, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLargeTurretBase.CurrentTargetSync owner, in bool value) => owner.IsPotential = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLargeTurretBase.CurrentTargetSync owner, out bool value) => value = owner.IsPotential;
      }
    }

    [Serializable]
    private struct MyEntityWithDistSq
    {
      public MyEntity Entity;
      public double DistSq;

      public override string ToString() => this.Entity.ToString();

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003EMyEntityWithDistSq\u003C\u003EEntity\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.MyEntityWithDistSq, MyEntity>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLargeTurretBase.MyEntityWithDistSq owner, in MyEntity value) => owner.Entity = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLargeTurretBase.MyEntityWithDistSq owner, out MyEntity value) => value = owner.Entity;
      }

      protected class Sandbox_Game_Weapons_MyLargeTurretBase\u003C\u003EMyEntityWithDistSq\u003C\u003EDistSq\u003C\u003EAccessor : IMemberAccessor<MyLargeTurretBase.MyEntityWithDistSq, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyLargeTurretBase.MyEntityWithDistSq owner, in double value) => owner.DistSq = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyLargeTurretBase.MyEntityWithDistSq owner, out double value) => value = owner.DistSq;
      }
    }

    private interface IMyPredicionType
    {
      bool ManualTargetPosition { get; }

      Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity entity);
    }

    private class MyTargetPredictionType : MyLargeTurretBase.IMyPredicionType
    {
      private MyLargeTurretBase m_turret;
      private MyAmmoDefinition m_turretAmmoDefinition;
      private Vector3D m_muzzleWorldPosition;
      private int m_lastQueryTimeMs;
      private Vector3D m_lastResult = Vector3D.Zero;
      private VRage.ModAPI.IMyEntity m_lastTarget;

      public bool ManualTargetPosition => false;

      public MyLargeTurretBase Turret
      {
        get => this.m_turret;
        set => this.m_turret = value;
      }

      public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
      {
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds == this.m_lastQueryTimeMs && this.m_lastTarget == target)
          return this.m_lastResult;
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds != this.m_lastQueryTimeMs)
          this.m_muzzleWorldPosition = this.Turret.GunBase.GetMuzzleWorldPosition();
        this.m_lastTarget = target;
        this.m_lastQueryTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        if (target == null)
        {
          this.m_lastResult = new Vector3D();
          return this.m_lastResult;
        }
        if (target.MarkedForClose)
        {
          this.m_lastResult = target.PositionComp.GetPosition();
          return this.m_lastResult;
        }
        Vector3D center = target.PositionComp.WorldAABB.Center;
        Vector3D deltaPos = center - this.m_muzzleWorldPosition;
        if (MyLargeTurretBase.DEBUG_DRAW_TARGET_PREDICTION)
        {
          MyRenderProxy.DebugDrawLine3D(this.m_muzzleWorldPosition, center, Color.Lime, Color.Lime, true);
          MyRenderProxy.DebugDrawText3D(this.m_muzzleWorldPosition, string.Format("Distance: {0:####.00}m", (object) deltaPos.Length()), Color.Lime, 0.5f, true, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
        }
        float projectileVel = 0.0f;
        float num1 = 0.0f;
        bool flag = false;
        if (this.m_turretAmmoDefinition == null && this.m_turret != null && (this.m_turret.GunBase != null && this.m_turret.GunBase.CurrentAmmoMagazineDefinition != null))
          this.m_turretAmmoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(this.m_turret.GunBase.CurrentAmmoMagazineDefinition.AmmoDefinitionId);
        if (this.m_turretAmmoDefinition != null)
        {
          projectileVel = this.m_turretAmmoDefinition.DesiredSpeed;
          num1 = this.m_turretAmmoDefinition.MaxTrajectory;
          if (this.m_turretAmmoDefinition is MyMissileAmmoDefinition turretAmmoDefinition)
          {
            num1 += turretAmmoDefinition.MissileExplosionRadius;
            flag = !turretAmmoDefinition.MissileSkipAcceleration;
          }
        }
        float num2 = (double) projectileVel < 9.99999974737875E-06 ? 1E-06f : num1 / projectileVel;
        Vector3 vector3_1 = Vector3.Zero;
        if (target.Physics != null)
        {
          vector3_1 = target.Physics.LinearVelocityUnsafe;
        }
        else
        {
          VRage.ModAPI.IMyEntity topMostParent = target.GetTopMostParent();
          if (topMostParent != null && topMostParent.Physics != null)
            vector3_1 = topMostParent.Physics.LinearVelocityUnsafe;
        }
        Vector3 vector3_2 = this.Turret.Parent != null ? this.Turret.Parent.Physics.LinearVelocityUnsafe : Vector3.Zero;
        Vector3 vector3_3 = vector3_1 - vector3_2;
        double num3 = MathHelper.Clamp(this.Intercept(deltaPos, (Vector3D) vector3_3, projectileVel), 0.0, (double) num2);
        Vector3D vector3D = center + (float) num3 * vector3_1;
        this.m_lastResult = flag ? vector3D - (float) num3 / num2 * vector3_2 : vector3D - (float) num3 * vector3_2;
        if (MyLargeTurretBase.DEBUG_DRAW_TARGET_PREDICTION)
        {
          MyRenderProxy.DebugDrawLine3D(this.m_muzzleWorldPosition, this.m_lastResult, Color.Orange, Color.Orange, true);
          MyRenderProxy.DebugDrawText3D(this.m_muzzleWorldPosition, string.Format("Time of Impact: {0:00.00}s", (object) num3), Color.Orange, 0.5f, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
          MyRenderProxy.DebugDrawText3D(this.m_muzzleWorldPosition, string.Format("Distance of Impact: {0:####.00}m", (object) (this.m_lastResult - this.m_muzzleWorldPosition).Length()), Color.Orange, 0.5f, true);
        }
        return this.m_lastResult;
      }

      private double Intercept(Vector3D deltaPos, Vector3D deltaVel, float projectileVel)
      {
        double num1 = Vector3D.Dot(deltaVel, deltaVel) - (double) projectileVel * (double) projectileVel;
        double num2 = 2.0 * Vector3D.Dot(deltaVel, deltaPos);
        double num3 = Vector3D.Dot(deltaPos, deltaPos);
        double d = num2 * num2 - 4.0 * num1 * num3;
        return d <= 0.0 ? -1.0 : 2.0 * num3 / (Math.Sqrt(d) - num2);
      }

      public MyTargetPredictionType(MyLargeTurretBase turret) => this.Turret = turret;
    }

    private class MyTargetNoPredictionType : MyLargeTurretBase.IMyPredicionType
    {
      public bool ManualTargetPosition => false;

      public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target) => target.PositionComp.WorldAABB.Center;
    }

    private class MyPositionNoPredictionType : MyLargeTurretBase.IMyPredicionType
    {
      public bool ManualTargetPosition => true;

      public Vector3D TrackedPosition { get; set; }

      public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target) => this.TrackedPosition;
    }

    private class MyPositionPredictionType : MyLargeTurretBase.IMyPredicionType
    {
      public bool ManualTargetPosition => true;

      public MyLargeTurretBase Turret { get; set; }

      public Vector3D TrackedPosition { get; set; }

      public Vector3D TrackedVelocity { get; set; }

      public Vector3D GetPredictedTargetPosition(VRage.ModAPI.IMyEntity target)
      {
        Vector3D vector3D1 = this.ManualTargetPosition || target == null ? this.TrackedPosition : target.PositionComp.WorldMatrix.Translation;
        Vector3D vector3D2 = Vector3D.Normalize(vector3D1 - this.Turret.GunBase.GetMuzzleWorldPosition());
        float num1 = 0.0f;
        if (this.Turret.GunBase.CurrentAmmoMagazineDefinition != null)
        {
          MyAmmoDefinition ammoDefinition = MyDefinitionManager.Static.GetAmmoDefinition(this.Turret.GunBase.CurrentAmmoMagazineDefinition.AmmoDefinitionId);
          num1 = ammoDefinition.DesiredSpeed;
          if (ammoDefinition.AmmoType == MyAmmoType.Missile)
          {
            MyMissileAmmoDefinition missileAmmoDefinition = (MyMissileAmmoDefinition) ammoDefinition;
            if ((double) missileAmmoDefinition.MissileInitialSpeed == 100.0 && (double) missileAmmoDefinition.MissileAcceleration == 600.0 && (double) ammoDefinition.DesiredSpeed == 700.0)
              num1 = (float) (800.0 - 238431.0 / (397.420013427734 + (vector3D1 - this.Turret.GunBase.GetMuzzleWorldPosition()).Length()));
          }
        }
        Vector3 vector1 = (Vector3) this.TrackedVelocity - this.Turret.Parent.Physics.LinearVelocityUnsafe;
        Vector3 vector3_1 = (Vector3) ((double) Vector3.Dot(vector1, (Vector3) vector3D2) * vector3D2);
        Vector3 vector3_2 = vector1 - vector3_1;
        float num2 = vector3_2.Length();
        if ((double) num2 > (double) num1)
          return vector3D1;
        float num3 = (float) Math.Sqrt((double) num1 * (double) num1 - (double) num2 * (double) num2);
        Vector3 vector3_3 = (Vector3) (vector3D2 * (double) num3);
        float num4 = vector3_3.Length() - vector3_1.Length();
        double num5 = (double) num4 != 0.0 ? (this.Turret.PositionComp.GetPosition() - vector3D1).Length() / (double) num4 : 0.0;
        Vector3 vector3_4 = vector3_3 + vector3_2;
        return num5 > 0.00999999977648258 ? this.Turret.GunBase.GetMuzzleWorldPosition() + (Vector3D) vector3_4 * num5 : vector3D1;
      }

      public MyPositionPredictionType(MyLargeTurretBase turret) => this.Turret = turret;
    }

    public enum MyLargeShipGunStatus
    {
      MyWeaponStatus_Deactivated,
      MyWeaponStatus_Searching,
      MyWeaponStatus_Shooting,
      MyWeaponStatus_ShootDelaying,
    }

    private class MyTargetSelectionWorkData : WorkData
    {
      public MyEntity SuggestedTarget;
      public bool SuggestedTargetIsPotential;
      public MyGridTargeting GridTargeting;
      public MyEntity Entity;
    }

    protected sealed class sync_ControlledEntity_Used\u003C\u003E : ICallSite<MyLargeTurretBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.sync_ControlledEntity_Used();
      }
    }

    protected sealed class OnBeginShoot\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyLargeTurretBase, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnBeginShoot(action);
      }
    }

    protected sealed class OnEndShoot\u003C\u003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyLargeTurretBase, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in MyShootActionEnum action,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnEndShoot(action);
      }
    }

    protected sealed class ResetTargetParams\u003C\u003E : ICallSite<MyLargeTurretBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ResetTargetParams();
      }
    }

    protected sealed class RequestUseMessage\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64 : ICallSite<MyLargeTurretBase, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in UseActionEnum useAction,
        in long usedById,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RequestUseMessage(useAction, usedById);
      }
    }

    protected sealed class UseSuccessCallback\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64\u0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyLargeTurretBase, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in UseActionEnum useAction,
        in long usedById,
        in UseActionResult useResult,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UseSuccessCallback(useAction, usedById, useResult);
      }
    }

    protected sealed class UseFailureCallback\u003C\u003EVRage_Game_Entity_UseObject_UseActionEnum\u0023System_Int64\u0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyLargeTurretBase, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in UseActionEnum useAction,
        in long usedById,
        in UseActionResult useResult,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UseFailureCallback(useAction, usedById, useResult);
      }
    }

    protected sealed class OnShootMissile\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MyLargeTurretBase, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in MyObjectBuilder_Missile builder,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnShootMissile(builder);
      }
    }

    protected sealed class OnRemoveMissile\u003C\u003ESystem_Int64 : ICallSite<MyLargeTurretBase, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveMissile(entityId);
      }
    }

    protected sealed class SetTargetRequest\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<MyLargeTurretBase, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in long entityId,
        in bool usePrediction,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetTargetRequest(entityId, usePrediction);
      }
    }

    protected sealed class SetTargetPosition\u003C\u003EVRageMath_Vector3D\u0023VRageMath_Vector3\u0023System_Boolean : ICallSite<MyLargeTurretBase, Vector3D, Vector3, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeTurretBase @this,
        in Vector3D targetPos,
        in Vector3 velocity,
        in bool usePrediction,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetTargetPosition(targetPos, velocity, usePrediction);
      }
    }

    protected class m_lateStartRandom\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_lateStartRandom = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_shootingRange\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_shootingRange = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_enableIdleRotation\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_enableIdleRotation = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_rotationAndElevationSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyLargeTurretBase.SyncRotationAndElevation, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyLargeTurretBase.SyncRotationAndElevation, SyncDirection.BothWays>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_rotationAndElevationSync = (VRage.Sync.Sync<MyLargeTurretBase.SyncRotationAndElevation, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_targetSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyLargeTurretBase.CurrentTargetSync, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyLargeTurretBase.CurrentTargetSync, SyncDirection.FromServer>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_targetSync = (VRage.Sync.Sync<MyLargeTurretBase.CurrentTargetSync, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_targetFlags\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyTurretTargetFlags, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyTurretTargetFlags, SyncDirection.BothWays>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_targetFlags = (VRage.Sync.Sync<MyTurretTargetFlags, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_cachedAmmunitionAmount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyLargeTurretBase) obj0).m_cachedAmmunitionAmount = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
