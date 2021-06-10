// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyShipDrill
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Drill))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyShipDrill), typeof (Sandbox.ModAPI.Ingame.IMyShipDrill)})]
  public class MyShipDrill : MyFunctionalBlock, IMyGunObject<MyToolBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    public const float HEAD_MAX_ROTATION_SPEED = 12.56637f;
    public const float HEAD_SLOWDOWN_TIME_IN_SECONDS = 0.5f;
    public const float DRILL_RANGE_SQ = 0.9604f;
    private static int m_countdownDistributor;
    private int m_blockLength;
    private float m_cubeSideLength;
    private MyDefinitionId m_defId;
    private int m_headLastUpdateTime;
    private bool m_isControlled;
    private MyDrillBase m_drillBase;
    private int m_drillFrameCountdown = 90;
    private bool m_wantsToDrill;
    private bool m_wantsToCollect;
    private MyShipDrill.MyDrillHead m_drillHeadEntity;
    private MyCharacter m_owner;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private IMyConveyorEndpoint m_multilineConveyorEndpoint;
    internal int ShipDrillId = -1;
    private float m_drillMultiplier = 1f;
    private float m_powerConsumptionMultiplier = 1f;

    private bool WantsToDrill
    {
      get => this.m_wantsToDrill;
      set
      {
        this.m_wantsToDrill = value;
        this.WantstoDrillChanged();
      }
    }

    public MyShipDrill.MyDrillHead DrillHeadEntity => this.m_drillHeadEntity;

    public bool IsDeconstructor => false;

    public MyCharacter Owner => this.m_owner;

    public float BackkickForcePerSecond => 0.0f;

    public float ShakeAmount { get; protected set; }

    public bool EnabledInWorldRules => true;

    public MyDefinitionId DefinitionId => this.m_defId;

    public bool IsSkinnable => false;

    public MyShipDrill()
    {
      this.Render = (MyRenderComponentBase) new MyShipDrillRenderComponent();
      this.CreateTerminalControls();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.SetupDrillFrameCountdown();
      this.NeedsWorldMatrix = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyShipDrill>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyShipDrill> onOff = new MyTerminalControlOnOffSwitch<MyShipDrill>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyShipDrill, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyShipDrill, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyShipDrill>();
      MyTerminalControlFactory.AddControl<MyShipDrill>((MyTerminalControl<MyShipDrill>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      this.m_defId = builder.GetId();
      MyShipDrillDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(this.m_defId) as MyShipDrillDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(cubeBlockDefinition.ResourceSinkGroup, this.ComputeMaxRequiredPower(), new Func<float>(this.ComputeRequiredPower));
      this.ResourceSink = resourceSinkComponent;
      this.m_drillBase = new MyDrillBase((MyEntity) this, "Smoke_DrillDust", "Smoke_DrillDust", "Smoke_DrillDust_Metal", (MyDrillSensorBase) new MyDrillSensorSphere(cubeBlockDefinition.SensorRadius, cubeBlockDefinition.SensorOffset, (MyDefinitionBase) this.BlockDefinition), new MyDrillCutOut(cubeBlockDefinition.CutOutOffset, cubeBlockDefinition.CutOutRadius), 0.5f, -0.4f, 0.4f, 1f, (Action<float, string, string>) ((amount, typeId, subtypeId) =>
      {
        if (MyVisualScriptLogicProvider.ShipDrillCollected == null)
          return;
        MyVisualScriptLogicProvider.ShipDrillCollected(this.Name, this.EntityId, this.CubeGrid.Name, this.CubeGrid.EntityId, typeId, subtypeId, amount);
      }));
      base.Init(builder, cubeGrid);
      this.m_blockLength = cubeBlockDefinition.Size.Z;
      this.m_cubeSideLength = MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize);
      float maxVolume = (float) ((double) (cubeBlockDefinition.Size.X * cubeBlockDefinition.Size.Y * cubeBlockDefinition.Size.Z) * (double) this.m_cubeSideLength * (double) this.m_cubeSideLength * (double) this.m_cubeSideLength * 0.5);
      Vector3 size = new Vector3((float) cubeBlockDefinition.Size.X, (float) cubeBlockDefinition.Size.Y, (float) cubeBlockDefinition.Size.Z * 0.5f);
      if (MyEntityExtensions.GetInventory(this) == null)
        this.Components.Add<MyInventoryBase>((MyInventoryBase) new MyInventory(maxVolume, size, MyInventoryFlags.CanSend));
      MyEntityExtensions.GetInventory(this).Constraint = new MyInventoryConstraint(MySpaceTexts.ToolTipItemFilter_AnyOre).AddObjectBuilderType((MyObjectBuilderType) typeof (MyObjectBuilder_Ore));
      this.m_drillBase.OutputInventory = MyEntityExtensions.GetInventory(this);
      this.m_drillBase.IgnoredEntities.Add((MyEntity) this);
      this.m_drillBase.IgnoredEntities.Add((MyEntity) cubeGrid);
      this.m_drillBase.UpdatePosition(this.WorldMatrix);
      this.m_wantsToCollect = false;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderCompomentDrawDrillBase(this.m_drillBase));
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerReciever(this.ResourceSink, (VRage.ModAPI.IMyEntity) this));
      MyObjectBuilder_Drill objectBuilderDrill = (MyObjectBuilder_Drill) builder;
      MyEntityExtensions.GetInventory(this).Init(objectBuilderDrill.Inventory);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.m_useConveyorSystem.SetLocalValue(objectBuilderDrill.UseConveyorSystem);
      this.SetDetailedInfoDirty();
      this.m_wantsToDrill = objectBuilderDrill.Enabled;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
      this.m_drillBase.m_drillMaterial = MyStringHash.GetOrCompute("ShipDrill");
      this.m_baseIdleSound = cubeBlockDefinition.PrimarySound;
      this.m_drillBase.m_idleSoundLoop = this.m_baseIdleSound;
      this.m_drillBase.ParticleOffset = cubeBlockDefinition.ParticleOffset;
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory) => base.OnInventoryComponentAdded(inventory);

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory) => base.OnInventoryComponentRemoved(inventory);

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void OnEnabledChanged()
    {
      this.WantsToDrill = this.Enabled;
      base.OnEnabledChanged();
    }

    private void OnIsWorkingChanged(MyCubeBlock obj) => this.WantstoDrillChanged();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.WantstoDrillChanged();
    }

    private void SetupDrillFrameCountdown()
    {
      MyShipDrill.m_countdownDistributor += 10;
      if (MyShipDrill.m_countdownDistributor > 10)
        MyShipDrill.m_countdownDistributor = -10;
      this.m_drillFrameCountdown = 90 + (MyShipMiningSystem.DebugDisable ? MyShipDrill.m_countdownDistributor : 0);
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void WantstoDrillChanged()
    {
      this.ResourceSink.Update();
      if ((this.Enabled || this.WantsToDrill) && (this.IsFunctional && this.ResourceSink != null) && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        if (this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out MyGunStatusEnum _))
        {
          if (!this.m_drillBase.IsDrilling)
            this.m_drillBase.StartDrillingAnimation(true);
        }
        else
          this.m_drillBase.StopDrill();
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        this.CubeGrid.GridSystems.MiningSystem?.AddDrillUpdate(this);
      }
      else
      {
        this.CubeGrid.GridSystems.MiningSystem?.RemoveDrillUpdate(this);
        this.SetupDrillFrameCountdown();
        this.m_drillBase.StopDrill();
      }
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Drill builderCubeBlock = (MyObjectBuilder_Drill) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void Closing()
    {
      base.Closing();
      this.m_drillBase.Close();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      if (this.m_drillBase == null)
        return;
      this.m_drillBase.UpdatePosition(this.WorldMatrix);
    }

    public override void UpdateAfterSimulation100()
    {
      this.ResourceSink.Update();
      base.UpdateAfterSimulation100();
      this.m_drillBase.UpdateSoundEmitter(Vector3.Zero);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsFunctional || !(bool) this.m_useConveyorSystem)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory.GetItemsCount() <= 0)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, inventory);
    }

    public override void UpdateBeforeSimulation10()
    {
      this.Receiver_IsPoweredChanged();
      base.UpdateBeforeSimulation10();
      if (this.Parent == null || this.Parent.Physics == null)
        return;
      this.m_drillFrameCountdown -= 10;
      if (this.m_drillFrameCountdown > 0)
        return;
      this.m_drillFrameCountdown += 90;
      if (this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out MyGunStatusEnum _))
      {
        if (this.m_drillBase.Drill(this.Enabled || this.m_wantsToCollect, speedMultiplier: 0.1f))
          this.ShakeAmount = 1f;
        else
          this.ShakeAmount = 0.5f;
      }
      else
        this.ShakeAmount = 0.0f;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.IsPreview)
        return;
      this.m_drillBase.UpdateAfterSimulation();
      if ((this.WantsToDrill || this.Enabled) && (this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out MyGunStatusEnum _) && (double) this.m_drillBase.AnimationMaxSpeedRatio > 0.0))
      {
        if (this.CheckPlayerControl() && MySession.Static.CameraController != null && (MySession.Static.CameraController.IsInFirstPersonView || MySession.Static.CameraController.ForceFirstPersonCamera))
          this.m_drillBase.PerformCameraShake(this.ShakeAmount);
        if (MySession.Static.EnableToolShake && MyFakes.ENABLE_TOOL_SHAKE && !this.CubeGrid.Physics.IsStatic)
          this.ApplyShakeForce();
        if (this.WantsToDrill || this.Enabled)
          this.CheckDustEffect();
        if (this.m_drillHeadEntity == null)
          return;
        this.m_drillHeadEntity.Render.UpdateSpeed(12.56637f);
      }
      else
      {
        if (this.m_drillHeadEntity != null)
          this.m_drillHeadEntity.Render.UpdateSpeed(0.0f);
        if (this.HasDamageEffect)
          return;
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    private bool CheckPlayerControl()
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      return localHumanPlayer != null && localHumanPlayer.Controller != null && (localHumanPlayer.Controller.ControlledEntity is MyCubeBlock controlledEntity && !(controlledEntity is MyRemoteControl)) && controlledEntity.CubeGrid == this.CubeGrid;
    }

    private void CheckDustEffect()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || this.m_drillBase.DustParticles == null || this.m_drillBase.DustParticles.IsEmittingStopped)
        return;
      float num1 = float.MaxValue;
      Vector3D vector3D1 = Vector3D.Zero;
      Vector3D center = this.m_drillBase.Sensor.Center;
      foreach (KeyValuePair<long, MyDrillSensorBase.DetectionInfo> keyValuePair in this.m_drillBase.Sensor.CachedEntitiesInRange)
      {
        MyDrillSensorBase.DetectionInfo detectionInfo = keyValuePair.Value;
        if (detectionInfo.Entity is MyVoxelBase)
        {
          float num2 = Vector3.DistanceSquared((Vector3) detectionInfo.DetectionPoint, (Vector3) center);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            vector3D1 = detectionInfo.DetectionPoint;
          }
        }
      }
      if ((double) num1 == 3.40282346638529E+38)
        return;
      ref readonly MatrixD local = ref this.PositionComp.WorldMatrixRef;
      Vector3D vector3D2 = (vector3D1 + center) / 2.0;
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.AppendFormat("\n");
    }

    public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      if (action != MyShootActionEnum.PrimaryAction && action != MyShootActionEnum.SecondaryAction)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!this.IsFunctional)
      {
        status = MyGunStatusEnum.NotFunctional;
        return false;
      }
      if (!this.HasPlayerAccess(shooter))
      {
        status = MyGunStatusEnum.AccessDenied;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Drilling, shooter))
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (this.ResourceSink.IsPowered)
        return true;
      status = MyGunStatusEnum.OutOfPower;
      return false;
    }

    public void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (action != MyShootActionEnum.PrimaryAction && action != MyShootActionEnum.SecondaryAction)
        return;
      this.ShakeAmount = 0.5f;
      this.m_wantsToCollect = action == MyShootActionEnum.PrimaryAction;
      this.WantsToDrill = true;
    }

    public void BeginShoot(MyShootActionEnum action)
    {
    }

    public void EndShoot(MyShootActionEnum action)
    {
      this.WantsToDrill = false;
      this.ResourceSink.Update();
    }

    public void OnControlAcquired(MyCharacter owner)
    {
      this.m_owner = owner;
      this.m_isControlled = true;
      if (this.Owner != MySession.Static.LocalCharacter)
        return;
      MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void OnControlReleased()
    {
      this.m_owner = (MyCharacter) null;
      this.m_isControlled = false;
      if (!this.Enabled)
        this.m_drillBase.StopDrill();
      if (MySession.Static.TopMostControlledEntity != this.CubeGrid)
        return;
      MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      MyHud.BlockInfo.MissingComponentIndex = -1;
      MyHud.BlockInfo.DefinitionId = this.BlockDefinition.Id;
      MyHud.BlockInfo.BlockName = this.BlockDefinition.DisplayNameText;
      MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) this.BlockDefinition);
      MyHud.BlockInfo.PCUCost = 0;
      MyHud.BlockInfo.BlockIcons = this.BlockDefinition.Icons;
      MyHud.BlockInfo.BlockIntegrity = 1f;
      MyHud.BlockInfo.CriticalIntegrity = 0.0f;
      MyHud.BlockInfo.CriticalComponentIndex = 0;
      MyHud.BlockInfo.OwnershipIntegrity = 0.0f;
      MyHud.BlockInfo.BlockBuiltBy = 0L;
      MyHud.BlockInfo.GridSize = MyCubeSize.Small;
      MyHud.BlockInfo.Components.Clear();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnDestroy();
    }

    private void ApplyShakeForce(float standbyRotationRatio = 1f)
    {
      MyGridPhysics physics = this.CubeGrid.Physics;
      MyPositionComponentBase positionComp = this.PositionComp;
      if (physics == null || positionComp == null)
        return;
      int hashCode = this.GetHashCode();
      float num1 = this.CubeGrid.GridSizeEnum == MyCubeSize.Small ? 1f : 5f;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      Vector3D right = worldMatrix.Right;
      float ms = (float) MyPerformanceCounter.TicksToMs(MyPerformanceCounter.ElapsedTicks);
      Vector3 zero = Vector3.Zero;
      float num2 = (float) hashCode + ms;
      Vector3 vector3 = (Vector3) ((Vector3) (zero + up * Math.Sin((double) num2 * 13.3500003814697 / 5.0)) + right * Math.Sin((double) num2 * 18.1539993286133 / 5.0)) * ((float) ((double) standbyRotationRatio * (double) num1 * 240.0) * this.m_drillBase.AnimationMaxSpeedRatio * this.m_drillBase.AnimationMaxSpeedRatio);
      physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(vector3), new Vector3D?(positionComp.GetPosition()), new Vector3?(), new float?(), true, false);
    }

    private Vector3 ComputeDrillSensorCenter()
    {
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D vector3D = worldMatrix.Forward * (double) (this.m_blockLength - 2) * (double) this.m_cubeSideLength;
      worldMatrix = this.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      return (Vector3) (vector3D + translation);
    }

    private float ComputeMaxRequiredPower() => 1f / 500f * this.m_powerConsumptionMultiplier;

    private float ComputeRequiredPower() => !this.IsFunctional || !this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out MyGunStatusEnum _) || !this.Enabled && !this.WantsToDrill ? 1E-06f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public int GetTotalAmmunitionAmount() => 0;

    public int GetAmmunitionAmount() => 0;

    public int GetMagazineAmount() => 0;

    public IMyConveyorEndpoint ConveyorEndpoint => this.m_multilineConveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_multilineConveyorEndpoint = (IMyConveyorEndpoint) new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint(this.m_multilineConveyorEndpoint));
    }

    public bool IsShooting => this.m_drillBase.IsDrilling;

    int IMyGunObject<MyToolBase>.ShootDirectionUpdateTime => 0;

    bool IMyGunObject<MyToolBase>.NeedsShootDirectionWhileAiming => false;

    float IMyGunObject<MyToolBase>.MaximumShotLength => 0.0f;

    public Vector3 DirectionToTarget(Vector3D target) => throw new NotImplementedException();

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public MyToolBase GunBase => (MyToolBase) null;

    bool Sandbox.ModAPI.Ingame.IMyShipDrill.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    float Sandbox.ModAPI.IMyShipDrill.DrillHarvestMultiplier
    {
      get => this.m_drillMultiplier;
      set
      {
        this.m_drillMultiplier = value;
        if (this.m_drillBase == null)
          return;
        this.m_drillBase.VoxelHarvestRatio = 0.009f * this.m_drillMultiplier * MySession.Static.Settings.HarvestRatioMultiplier;
        this.m_drillBase.VoxelHarvestRatio = MathHelper.Clamp(this.m_drillBase.VoxelHarvestRatio, 0.0f, 1f);
      }
    }

    float Sandbox.ModAPI.IMyShipDrill.PowerConsumptionMultiplier
    {
      get => this.m_powerConsumptionMultiplier;
      set
      {
        this.m_powerConsumptionMultiplier = value;
        if ((double) this.m_powerConsumptionMultiplier < 0.00999999977648258)
          this.m_powerConsumptionMultiplier = 0.01f;
        if (this.ResourceSink == null)
          return;
        this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, this.ComputeMaxRequiredPower() * this.m_powerConsumptionMultiplier);
        this.ResourceSink.Update();
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      }
    }

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public bool SupressShootAnimation() => false;

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      this.m_drillHeadEntity = new MyShipDrill.MyDrillHead(this);
      this.m_drillHeadEntity.OnClosing += (Action<MyEntity>) (x =>
      {
        if (!(x is MyShipDrill.MyDrillHead head) || head.DrillParent == null)
          return;
        head.DrillParent.UnregisterHead(head);
      });
      return (MyEntitySubpart) this.m_drillHeadEntity;
    }

    public void UnregisterHead(MyShipDrill.MyDrillHead head)
    {
      if (this.m_drillHeadEntity != head)
        return;
      this.m_drillHeadEntity = (MyShipDrill.MyDrillHead) null;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      this.m_drillBase.IgnoredEntities.Remove((MyEntity) oldGrid);
      this.m_drillBase.IgnoredEntities.Add((MyEntity) this.CubeGrid);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_drillHeadEntity == null)
        return;
      this.m_drillHeadEntity.Render.UpdateSpeed(0.0f);
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.CubeGrid.GridSystems.GetOrCreateMiningSystem().RegisterDrill(this);
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      this.CubeGrid.GridSystems.MiningSystem?.UnRegisterDrill(this);
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation()
    {
      PullInformation pullInformation = new PullInformation()
      {
        Inventory = MyEntityExtensions.GetInventory(this),
        OwnerID = this.OwnerId
      };
      pullInformation.Constraint = pullInformation.Inventory.Constraint;
      return pullInformation;
    }

    public bool AllowSelfPulling() => false;

    public BoundingSphere GetDrillingSphere()
    {
      ref readonly Matrix local = ref this.PositionComp.LocalMatrixRef;
      Matrix matrix = local;
      Vector3 translation = matrix.Translation;
      matrix = local;
      Vector3 vector3 = matrix.Forward * this.m_drillBase.CutOut.CenterOffset;
      return new BoundingSphere(translation + vector3, this.m_drillBase.CutOut.Radius);
    }

    public void TryDrillVoxel(
      MyVoxelBase voxel,
      Vector3D hitPosition,
      bool collectOre,
      bool applyDamagedMaterial)
    {
      this.CubeGrid.GridSystems.MiningSystem.RequestCutOut(this, !collectOre, applyDamagedMaterial, hitPosition, voxel);
    }

    public void OnDrillResults(
      Dictionary<MyVoxelMaterialDefinition, int> materials,
      Vector3D hitPosition,
      bool collectOre)
    {
      this.m_drillBase.OnDrillResults(materials, hitPosition, collectOre);
    }

    public void SynchronizeWith(MyShipDrill other) => this.m_drillFrameCountdown = other.m_drillFrameCountdown;

    public Vector3D GetMuzzlePosition() => this.PositionComp.GetPosition();

    [SpecialName]
    int IMyInventoryOwner.get_InventoryCount() => this.InventoryCount;

    [SpecialName]
    bool IMyInventoryOwner.get_HasInventory() => this.HasInventory;

    public class MyDrillHead : MyEntitySubpart
    {
      public MyShipDrill DrillParent;

      public MyShipDrillRenderComponent.MyDrillHeadRenderComponent Render => (MyShipDrillRenderComponent.MyDrillHeadRenderComponent) base.Render;

      public MyDrillHead(MyShipDrill parent)
      {
        this.DrillParent = parent;
        this.Render = (MyRenderComponentBase) new MyShipDrillRenderComponent.MyDrillHeadRenderComponent();
        this.InvalidateOnMove = false;
        this.NeedsWorldMatrix = false;
      }

      private class Sandbox_Game_Weapons_MyShipDrill\u003C\u003EMyDrillHead\u003C\u003EActor
      {
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipDrill) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Weapons_MyShipDrill\u003C\u003EActor : IActivator, IActivator<MyShipDrill>
    {
      object IActivator.CreateInstance() => (object) new MyShipDrill();

      MyShipDrill IActivator<MyShipDrill>.CreateInstance() => new MyShipDrill();
    }
  }
}
